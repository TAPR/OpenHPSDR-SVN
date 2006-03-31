//============================================================================
//
//  N8VB_vcom - Virtual COM Port
//  Copyright (c) 2005 Philip A Covington, N8VB
//
//	Email: p.covington@gmail.com
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
//============================================================================

#include "vCOM.h"

#define EV_TRANSFER_OCCURRED         	0
#define EV_READ_TOTAL_TIMEOUT      	1
#define EV_READ_INTERVAL_TIMEOUT  	2
#define EV_IRP_WAS_CANCELLED		3

VOID vCOMReadThread(PVOID pContext)
{
	PDEVICE_OBJECT 				pDeviceObject; 
    	PvCOM_DEVICE_EXTENSION 	pDeviceExtension;
	PvCOM_DEVICE_EXTENSION 	pTwinDeviceExtension;
	PIRP 						pIrp;
    	NTSTATUS    					status;
    	PVOID                       			events[4];
	KWAIT_BLOCK                 		waitBlocks[4];
	PIO_STACK_LOCATION 		pIrpStackLocation;
	KIRQL 						read_oldIrql;
	KIRQL 						write_oldIrql;
    	ULONG 						bufferLength;
    	PCHAR 						pBuffer;
	ULONG						lastCount;	

	pDeviceObject = pContext; 
	pDeviceExtension = pDeviceObject->DeviceExtension;
	pTwinDeviceExtension = pDeviceExtension->pTwin->DeviceExtension;
	
	events[EV_TRANSFER_OCCURRED]     	= &pDeviceExtension->transferOccurredEvent;
	events[EV_READ_TOTAL_TIMEOUT] 		= &pDeviceExtension->readTotalTimer;
	events[EV_READ_INTERVAL_TIMEOUT]	= &pDeviceExtension->readIntervalTimer;
	events[EV_IRP_WAS_CANCELLED]		= &pDeviceExtension->currentReadIrpWasCancelledEvent;
	
    	//KeSetPriorityThread(KeGetCurrentThread(), LOW_REALTIME_PRIORITY);
	
	while (TRUE)
	{
		KeWaitForSingleObject(&pDeviceExtension->readIRPQueueSemaphore, Executive, KernelMode, FALSE, NULL);

		KdPrint(("read thread  for device %d...\n", pDeviceExtension->deviceNumber));
		
		if (pDeviceExtension->terminateReadThread) 
		{
            		PsTerminateSystemThread(STATUS_SUCCESS);
		}
	        
		// Remove a pending IRP from the queue.
	       pIrp = IoCsqRemoveNextIrp(&pDeviceExtension->cancelSafeReadQueue, NULL);

		if (!pIrp)
		{
			continue; // IRP was cancelled go back to waiting
		}
				
		IoSetCancelRoutine(pIrp, vCOMCancelCurrentReadIrp);
		
		pDeviceExtension->pReadIrpCurrent = pIrp;

		pIrpStackLocation = IoGetCurrentIrpStackLocation(pIrp);
		pBuffer = pIrp->AssociatedIrp.SystemBuffer;		
		bufferLength = pIrpStackLocation->Parameters.Read.Length;

		if (bufferLength == 0)
		{
			vCOMReadThreadCompleteRequest(pDeviceObject, pIrp, STATUS_SUCCESS);
			continue;
		}
		
		pDeviceExtension->readTimeoutType = vCOMCalculateReadTimeoutValues(pDeviceObject,  pIrp);		
				
		if (pDeviceExtension->readTimeoutType == TIMEOUTS_TOTAL_TIMEOUT ||
			pDeviceExtension->readTimeoutType == TIMEOUTS_COMBINED_TIMEOUT ||
			pDeviceExtension->readTimeoutType == TIMEOUTS_RETURNBYTES_OR_WAIT)
				KeSetTimer(&pDeviceExtension->readTotalTimer, pDeviceExtension->readTotalTimeoutValue, NULL);				

		pDeviceExtension->readBufferCount = 0; //reset read buffer count for next read
		lastCount = 0;
		
		while (TRUE)
		{			
			KdPrint(("XXX requested bytes is %d for device %d\n", bufferLength, pDeviceExtension->deviceNumber));

		tryread:
			
			switch (pDeviceExtension->readTimeoutType)
			{
				case TIMEOUTS_TOTAL_TIMEOUT:
				case TIMEOUTS_WAIT_FOREVER:
					
					if (pDeviceExtension->outBufferCount >= bufferLength)
					{
						KeCancelTimer(&pDeviceExtension->readTotalTimer);
						status = STATUS_SUCCESS;
					}
					else
						status = STATUS_PENDING;
					break;

				case TIMEOUTS_IMMEDIATE_RETURN_WITH_BYTES:
					//return immediately with whatever there is
					KeCancelTimer(&pDeviceExtension->readTotalTimer);
					KeCancelTimer(&pDeviceExtension->readIntervalTimer);	
					status = STATUS_SUCCESS;
					break;

				case TIMEOUTS_RETURNBYTES_OR_WAIT:
					//return with at least a byte or wait
					if (pDeviceExtension->outBufferCount > 0)
					{	
						KeCancelTimer(&pDeviceExtension->readTotalTimer);
						KeCancelTimer(&pDeviceExtension->readIntervalTimer);	
						status = STATUS_SUCCESS;
					}
					else
						status = STATUS_PENDING;
					break;

				case TIMEOUTS_INTERVAL_TIMEOUT:	
				case TIMEOUTS_COMBINED_TIMEOUT:
					
					if (pDeviceExtension->outBufferCount != 0 && pDeviceExtension->outBufferCount >= bufferLength)
					{
						KeCancelTimer(&pDeviceExtension->readIntervalTimer);	
						KeCancelTimer(&pDeviceExtension->readTotalTimer);
						status = STATUS_SUCCESS;
					}
					else if (pDeviceExtension->outBufferCount > lastCount)
					{	//reset the interval timer if a character was rcvd
						lastCount = pDeviceExtension->outBufferCount;
						KeCancelTimer(&pDeviceExtension->readIntervalTimer);	
						KeSetTimer(&pDeviceExtension->readIntervalTimer, pDeviceExtension->readIntervalTimeoutValue, NULL);
						status = STATUS_PENDING;
					}
					else
						status = STATUS_PENDING;					
					break;
					
				default:
					status = STATUS_PENDING;
					break;
					
			}

			if (status == STATUS_PENDING)
			{	
				status = KeWaitForMultipleObjects(4, 
												events, 
												WaitAny, 
												Executive, 
												KernelMode, 
												FALSE, 
												&pDeviceExtension->readWaitTimeoutValue, 
												waitBlocks) ;
				
				if (pDeviceExtension->terminateReadThread) 
				{
		            		PsTerminateSystemThread(STATUS_SUCCESS);
				}
				
				if (status == EV_IRP_WAS_CANCELLED)
				{
					//irp was cancelled while waiting
					KeCancelTimer(&pDeviceExtension->readIntervalTimer);
					KeCancelTimer(&pDeviceExtension->readTotalTimer);
					break; //get next irp
				}

				if (status == STATUS_TIMEOUT)
					goto waittimeout;

				if (status == EV_TRANSFER_OCCURRED)
				{
					KeCancelTimer(&pDeviceExtension->readIntervalTimer);
					goto tryread;
				}

				if (status == EV_READ_TOTAL_TIMEOUT || status == EV_READ_INTERVAL_TIMEOUT)
				{	
				
				waittimeout:
					
					KeAcquireSpinLock(&pDeviceExtension->readSpinLock, &read_oldIrql);
					
					while (pDeviceExtension->outBufferCount)
					{
						pBuffer[pDeviceExtension->readBufferCount] = pDeviceExtension->outBuffer[pDeviceExtension->outBufferReadPosition];
						pDeviceExtension->readBufferCount++;
						pDeviceExtension->outBufferReadPosition++;
						pDeviceExtension->outBufferCount--;
						if (pDeviceExtension->readBufferCount >= bufferLength)
							break;
					}

					if (pDeviceExtension->outBufferCount == 0)
					{			
						pDeviceExtension->outBufferReadPosition = 0;
						pDeviceExtension->outBufferWritePosition = 0;			
					}

					pIrp->IoStatus.Information = pDeviceExtension->readBufferCount;
					
					KeReleaseSpinLock(&pDeviceExtension->readSpinLock, read_oldIrql);
				
					vCOMReadThreadCompleteRequest(pDeviceObject, pIrp, STATUS_TIMEOUT);
										
					//notify the transfer thread if it is waiting for room in the outbuffer
					KeSetEvent(&pTwinDeviceExtension->readOccurredEvent, 0, FALSE);
					break; //get next irp
				}
				
			}
			else if (status == STATUS_SUCCESS)
			{					
				// success, complete the IRP
				KeAcquireSpinLock(&pDeviceExtension->readSpinLock, &read_oldIrql);
				
				while (pDeviceExtension->outBufferCount)
				{
					pBuffer[pDeviceExtension->readBufferCount] = pDeviceExtension->outBuffer[pDeviceExtension->outBufferReadPosition];
					pDeviceExtension->readBufferCount++;
					pDeviceExtension->outBufferReadPosition++;
					pDeviceExtension->outBufferCount--;
					if (pDeviceExtension->readBufferCount >= bufferLength)
						break;
				}

				if (pDeviceExtension->outBufferCount <= 0)
				{
					pDeviceExtension->outBufferCount = 0;
					pDeviceExtension->outBufferReadPosition = 0;
					pDeviceExtension->outBufferWritePosition = 0;			
				}

				KeReleaseSpinLock(&pDeviceExtension->readSpinLock, read_oldIrql);		

				pIrp->IoStatus.Information = pDeviceExtension->readBufferCount;
								
				vCOMReadThreadCompleteRequest(pDeviceObject, pIrp, STATUS_SUCCESS);
				
				//notify the transfer thread if it is waiting for room in the outbuffer
				KeSetEvent(&pTwinDeviceExtension->readOccurredEvent, 0, FALSE);
				
				break;  //get next irp
			}
			else
				goto tryread;
						
		}
		
	}
	
}

NTSTATUS vCOMReadThreadStart(PDEVICE_OBJECT pDeviceObject)
{
	NTSTATUS    					status;
	HANDLE      					threadHandle;
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension =  pDeviceObject->DeviceExtension;
	
	pDeviceExtension->terminateReadThread = FALSE;
				
	status = PsCreateSystemThread(&threadHandle, THREAD_ALL_ACCESS, NULL, NULL, NULL,
			vCOMReadThread, pDeviceObject);
	
	if (status != STATUS_SUCCESS)
	{
		return status;
	}

	status = ObReferenceObjectByHandle(threadHandle, THREAD_ALL_ACCESS, NULL, KernelMode,
			&pDeviceExtension->pReadThread, NULL);

	if (status != STATUS_SUCCESS)
	{
		pDeviceExtension->terminateReadThread= TRUE;
		KeReleaseSemaphore(&pDeviceExtension->readIRPQueueSemaphore, 0, 1, FALSE);
	}
	else
		ZwClose(threadHandle);

	return status;
	
}

VOID vCOMReadThreadStop(PDEVICE_OBJECT pDeviceObject)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension;

	if (pDeviceObject == NULL)
		return;
	else
		pDeviceExtension =  pDeviceObject->DeviceExtension;
	
	if (pDeviceExtension->pReadThread != NULL)
	{
		pDeviceExtension->terminateReadThread= TRUE;

		KeReleaseSemaphore(&pDeviceExtension->readIRPQueueSemaphore, 0, 1, FALSE);
		KeSetEvent(&pDeviceExtension->transferOccurredEvent, 0, FALSE);

		KeWaitForSingleObject(pDeviceExtension->pReadThread, UserRequest, KernelMode, FALSE, NULL);

		ObDereferenceObject(pDeviceExtension->pReadThread);

		pDeviceExtension->pReadThread= NULL;
	}
}

VOID vCOMReadThreadCompleteRequest(PDEVICE_OBJECT pDeviceObject, PIRP pIrp, NTSTATUS status)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension =  pDeviceObject->DeviceExtension;
	PDRIVER_CANCEL  			pOldCancelRoutine;
	
	KeCancelTimer(&pDeviceExtension->readIntervalTimer);
	KeCancelTimer(&pDeviceExtension->readTotalTimer);
	
	if (pIrp != NULL && !pIrp->Cancel && pIrp == pDeviceExtension->pReadIrpCurrent)
	{
		pOldCancelRoutine = IoSetCancelRoutine(pIrp, NULL);

		if (pOldCancelRoutine != NULL)
		{
			pIrp->IoStatus.Status = status;
			IoCompleteRequest(pIrp, IO_NO_INCREMENT);	
			
			pDeviceExtension->pReadIrpCurrent= NULL;			
		}		
	}		
}

