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

VOID vCOMWriteThread(PVOID pContext)
{
	PDEVICE_OBJECT 				pDeviceObject = pContext;  
    	PvCOM_DEVICE_EXTENSION 	pDeviceExtension =  pDeviceObject->DeviceExtension;
	PvCOM_DEVICE_EXTENSION 	pTwinDeviceExtension = pDeviceExtension->pTwin->DeviceExtension;
    	PIRP 						pIrp;
    	NTSTATUS    					status;
    	PIO_STACK_LOCATION 		pIrpStackLocation;
    	PIO_STACK_LOCATION 		pTwinIrpStackLocation;
    	KIRQL 						write_oldIrql;
	KIRQL 						read_oldIrql;
    	ULONG 						dataLength = 0;
    	PCHAR 						pData = NULL;
    	ULONG 						count = 0;	
	
    	//KeSetPriorityThread(KeGetCurrentThread(), LOW_REALTIME_PRIORITY );
	
	while (TRUE)
	{
		KeWaitForSingleObject(&pDeviceExtension->writeIRPQueueSemaphore, Executive, KernelMode, FALSE, NULL);

		KdPrint(("write thread  for device %d...\n", pDeviceExtension->deviceNumber));

		status = STATUS_SUCCESS;
		
		if (pDeviceExtension->terminateWriteThread) 
			PsTerminateSystemThread(STATUS_SUCCESS);		
	        
		// Remove a pending IRP from the queue.
	       pIrp = IoCsqRemoveNextIrp(&pDeviceExtension->cancelSafeWriteQueue, NULL);

		if (!pIrp)
		{
			continue; // IRP was cancelled go back to waiting
		}

		IoSetCancelRoutine(pIrp, vCOMCancelCurrentWriteIrp);
		
		pDeviceExtension->pWriteIrpCurrent = pIrp;
		pIrpStackLocation = IoGetCurrentIrpStackLocation(pIrp); 
		dataLength = pIrpStackLocation->Parameters.Read.Length;
		pData = pIrp->AssociatedIrp.SystemBuffer;

		if (dataLength == 0)
		{	
			vCOMWriteThreadCompleteRequest(pDeviceObject, pIrp,  STATUS_SUCCESS);
			continue; // no data to write 
		}

		pDeviceExtension->writeTimeoutType = vCOMCalculateWriteTimeoutValues(pDeviceObject, pIrp);

		if (pDeviceExtension->writeTimeoutType == TIMEOUTS_TOTAL_TIMEOUT)
			KeSetTimer(&pDeviceExtension->writeTotalTimer, pDeviceExtension->writeTotalTimeoutValue, NULL);

		//check to see if room in buffer		
		while (dataLength > (pDeviceExtension->inBufferSize - pDeviceExtension->inBufferCount)) //not enough room in buffer...wait...
		{
			if (pDeviceExtension->terminateWriteThread) 
				PsTerminateSystemThread(STATUS_SUCCESS);
			
			KeDelayExecutionThread(KernelMode, FALSE, &pDeviceExtension->writeDelayLoopTimerValue);

			if (pDeviceExtension->terminateWriteThread) 
				PsTerminateSystemThread(STATUS_SUCCESS);
			
			if (KeReadStateEvent(&pDeviceExtension->currentWriteIrpWasCancelledEvent))
			{
				status = STATUS_TIMEOUT;
				break;
			}
			if (KeReadStateTimer(&pDeviceExtension->writeTotalTimer))
			{
				status = STATUS_TIMEOUT;
				KeCancelTimer(&pDeviceExtension->writeTotalTimer);
				vCOMWriteThreadCompleteRequest(pDeviceObject, pIrp,  STATUS_TIMEOUT);
				break;
			}			
		}

		if (status == STATUS_TIMEOUT)
			continue; //timeout occured or irp was cancelled


		KeCancelTimer(&pDeviceExtension->writeTotalTimer);	
		
		KeAcquireSpinLock(&pDeviceExtension->writeSpinLock, &write_oldIrql);
		
		if (pDeviceExtension->inBufferCount == 0)
		{
			pDeviceExtension->inBufferWritePosition = 0;
			pDeviceExtension->inBufferReadPosition = 0;
		}
		
		for (count = 0; count < dataLength; count++)
		{
			pDeviceExtension->inBuffer[pDeviceExtension->inBufferWritePosition] = pData[count];
			pDeviceExtension->inBufferWritePosition++;
			pDeviceExtension->inBufferCount++;
			if (pDeviceExtension->inBufferCount >= pDeviceExtension->inBufferSize)
				break;
		}
							
		KeReleaseSpinLock(&pDeviceExtension->writeSpinLock, write_oldIrql);

		pIrp->IoStatus.Information = count;
		
		vCOMWriteThreadCompleteRequest(pDeviceObject, pIrp,  STATUS_SUCCESS);
		vCOMCheckComEvents(pDeviceExtension, SERIAL_EV_TXEMPTY);

		KeSetEvent(&pDeviceExtension->transferNeededEvent, 0, FALSE);
				
	}
	
}


NTSTATUS vCOMWriteThreadStart(PDEVICE_OBJECT pDeviceObject)
{
	NTSTATUS    					status;
	HANDLE      					threadHandle;
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension =  pDeviceObject->DeviceExtension;
	
	pDeviceExtension->terminateWriteThread = FALSE;
		
	status = PsCreateSystemThread(&threadHandle, THREAD_ALL_ACCESS, NULL, NULL, NULL,
			vCOMWriteThread, pDeviceObject);
	
	if (status != STATUS_SUCCESS)
	{
		return status;
	}

	status = ObReferenceObjectByHandle(threadHandle, THREAD_ALL_ACCESS, NULL, KernelMode,
			&pDeviceExtension->pWriteThread, NULL);

	if (status != STATUS_SUCCESS)
	{
		pDeviceExtension->terminateWriteThread= TRUE;
		KeReleaseSemaphore(&pDeviceExtension->writeIRPQueueSemaphore, 0, 1, FALSE);
	}
	else
		ZwClose(threadHandle);

	return status;
	
}

VOID vCOMWriteThreadStop(PDEVICE_OBJECT pDeviceObject)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension;

	if (pDeviceObject == NULL)
		return;
	else
		pDeviceExtension =  pDeviceObject->DeviceExtension;
	
	if (pDeviceExtension->pWriteThread != NULL)
	{
		pDeviceExtension->terminateWriteThread= TRUE;

		KeReleaseSemaphore(&pDeviceExtension->writeIRPQueueSemaphore, 0, 1, FALSE);

		KeWaitForSingleObject(pDeviceExtension->pWriteThread, UserRequest, KernelMode, FALSE, NULL);

		ObDereferenceObject(pDeviceExtension->pWriteThread);

		pDeviceExtension->pWriteThread= NULL;
	}
}

VOID vCOMWriteThreadCompleteRequest(PDEVICE_OBJECT pDeviceObject, PIRP pIrp, NTSTATUS status)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension =  pDeviceObject->DeviceExtension;
	PDRIVER_CANCEL  			pOldCancelRoutine;
		
	if (pIrp != NULL && !pIrp->Cancel && pIrp == pDeviceExtension->pWriteIrpCurrent)
	{
		pOldCancelRoutine = IoSetCancelRoutine(pIrp, NULL);

		if (pOldCancelRoutine != NULL)
		{
			pIrp->IoStatus.Status = status;
			IoCompleteRequest(pIrp, IO_NO_INCREMENT);	
			
			pDeviceExtension->pWriteIrpCurrent = NULL;			
		}		
	}		
}


