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

VOID vCOMTransferThread(PVOID pContext)
{
	PDEVICE_OBJECT 				pDeviceObject = pContext;  
    	PvCOM_DEVICE_EXTENSION 	pDeviceExtension =  pDeviceObject->DeviceExtension;
	PvCOM_DEVICE_EXTENSION 	pTwinDeviceExtension = pDeviceExtension->pTwin->DeviceExtension;
    	PIRP 						pIrp;
    	NTSTATUS    					status;
	KIRQL 						write_oldIrql;
	KIRQL 						read_oldIrql;
	ULONG                       			count;
	ULONG						events;
	UCHAR						byte = 0;
	BOOLEAN						eventCharRcvd = FALSE;

	//KeSetPriorityThread(KeGetCurrentThread(), LOW_REALTIME_PRIORITY);
	
	while (TRUE)
	{
		KeWaitForSingleObject(&pDeviceExtension->transferNeededEvent, Executive, KernelMode, FALSE, NULL);

		KdPrint(("transfer thread  for device %d...\n", pDeviceExtension->deviceNumber));
		
		if (pDeviceExtension->terminateTransferThread)
			PsTerminateSystemThread(STATUS_SUCCESS);		

		// wait here until there is room in the twin device's outbuffer
		while ((pTwinDeviceExtension->outBufferSize - pTwinDeviceExtension->outBufferCount) < pDeviceExtension->inBufferCount)
		{
			status = KeWaitForSingleObject(&pDeviceExtension->readOccurredEvent, 
											Executive, 
											KernelMode, 
											FALSE, 
											&pDeviceExtension->transferWaitTimeoutValue);	
											
			if (pDeviceExtension->terminateTransferThread)
				PsTerminateSystemThread(STATUS_SUCCESS);	
			if (status == STATUS_TIMEOUT)
				break; //timeout occurred, go back to waiting for transferNeededEvent
		}
		
		//Do buffer transfer
		
		eventCharRcvd = FALSE;

		KeAcquireSpinLock(&pDeviceExtension->writeSpinLock, &write_oldIrql);

		for (count = 0; count < pDeviceExtension->inBufferCount; count++)
		{
			byte = pDeviceExtension->inBuffer[pDeviceExtension->inBufferReadPosition];
			if (byte == pTwinDeviceExtension->serEventCharacter)
				eventCharRcvd = TRUE;
			pTwinDeviceExtension->outBuffer[pTwinDeviceExtension->outBufferWritePosition] = byte;			
			pTwinDeviceExtension->outBufferWritePosition++;
			pDeviceExtension->inBufferReadPosition++;
			pTwinDeviceExtension->outBufferCount++;
			if (pTwinDeviceExtension->outBufferCount >= pTwinDeviceExtension->outBufferSize)
				break;
		}

		pDeviceExtension->inBufferCount = 0;
		pDeviceExtension->inBufferWritePosition = 0;
		pDeviceExtension->inBufferReadPosition = 0;

		KeReleaseSpinLock(&pDeviceExtension->writeSpinLock, write_oldIrql);
		
		if (eventCharRcvd)
			vCOMCheckComEvents(pTwinDeviceExtension, SERIAL_EV_RXFLAG | SERIAL_EV_RXCHAR);
		else
			vCOMCheckComEvents(pTwinDeviceExtension, SERIAL_EV_RXCHAR);		
				
		// notify the read thread for the twin device that a transfer occurred
		KeSetEvent(&pTwinDeviceExtension->transferOccurredEvent, 0, FALSE);				
		
	}
	
}

NTSTATUS vCOMTransferThreadStart(PDEVICE_OBJECT pDeviceObject)
{
	NTSTATUS    					status;
	HANDLE      					threadHandle;
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension =  pDeviceObject->DeviceExtension;
	
	pDeviceExtension->terminateTransferThread= FALSE;
		
	status = PsCreateSystemThread(&threadHandle, THREAD_ALL_ACCESS, NULL, NULL, NULL,
			vCOMTransferThread, pDeviceObject);	

	if (status != STATUS_SUCCESS)
	{
		return status;
	}

	status = ObReferenceObjectByHandle(threadHandle, THREAD_ALL_ACCESS, NULL, KernelMode,
			&pDeviceExtension->pTransferThread, NULL);

	if (status != STATUS_SUCCESS)
	{
		pDeviceExtension->terminateTransferThread = TRUE;
		
		KeSetEvent(&pDeviceExtension->transferNeededEvent, 0, FALSE);
		
	}
	else
		ZwClose(threadHandle);

	return status;
	
}

VOID vCOMTransferThreadStop(PDEVICE_OBJECT pDeviceObject)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension;

	if (pDeviceObject == NULL)
		return;
	else
		pDeviceExtension =  pDeviceObject->DeviceExtension;
	
	if (pDeviceExtension->pTransferThread != NULL)
	{
		pDeviceExtension->terminateTransferThread = TRUE;

		KeSetEvent(&pDeviceExtension->transferNeededEvent, 0, FALSE);
		KeSetEvent(&pDeviceExtension->readOccurredEvent, 0, FALSE);
		
		KeWaitForSingleObject(pDeviceExtension->pTransferThread, UserRequest, KernelMode, FALSE, NULL);

		ObDereferenceObject(pDeviceExtension->pTransferThread);

		pDeviceExtension->pTransferThread = NULL;
	}
}

