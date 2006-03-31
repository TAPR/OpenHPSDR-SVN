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

VOID vCOMInsertReadIrp (PIO_CSQ pCsq,  PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION   	deviceExtension;

	KdPrint(("insert read irp: %p\n", pIrp));
	
    	deviceExtension = CONTAINING_RECORD(pCsq,  vCOM_DEVICE_EXTENSION, cancelSafeReadQueue);

    	InsertTailList(&deviceExtension->readPendingQueue, &pIrp->Tail.Overlay.ListEntry);
}

VOID vCOMRemoveReadIrp(PIO_CSQ pCsq,  PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION	 	deviceExtension;

	KdPrint(("remove read irp: %p\n", pIrp));
	
    	deviceExtension = CONTAINING_RECORD(pCsq, vCOM_DEVICE_EXTENSION, cancelSafeReadQueue);
    
    	RemoveEntryList(&pIrp->Tail.Overlay.ListEntry);
}

PIRP vCOMPeekNextReadIrp(PIO_CSQ pCsq, PIRP pIrp,  PVOID pPeekContext)
{
	PvCOM_DEVICE_EXTENSION      	deviceExtension;
    	PIRP                    				nextIrp = NULL;
    	PLIST_ENTRY             			nextEntry;
    	PLIST_ENTRY             			listHead;
    	PIO_STACK_LOCATION     		irpStack;

	KdPrint(("peek next read irp: %p\n", pIrp));
	
	deviceExtension = CONTAINING_RECORD(pCsq, vCOM_DEVICE_EXTENSION, cancelSafeReadQueue);

	listHead = &deviceExtension->readPendingQueue;
	    
	if (pIrp == NULL) 
	{       
	    nextEntry = listHead->Flink;
	} 
	else 
	{
	    nextEntry = pIrp->Tail.Overlay.ListEntry.Flink;
	}

	while (nextEntry != listHead) 
	{
		nextIrp = CONTAINING_RECORD(nextEntry, IRP, Tail.Overlay.ListEntry);

	    	irpStack = IoGetCurrentIrpStackLocation(nextIrp);
	    	    	    
		if (pPeekContext) 
		{
			if (irpStack->FileObject == (PFILE_OBJECT) pPeekContext) 
			{       
		       	break;
		    	}
		}
		
		else 
		{
		    break;
		}
		
		nextIrp = NULL;
		nextEntry = nextEntry->Flink;

	}

	return nextIrp;
	
}

VOID vCOMAcquireReadLock(PIO_CSQ pCsq, PKIRQL pIrql)
{
	PvCOM_DEVICE_EXTENSION   deviceExtension;

    	deviceExtension = CONTAINING_RECORD(pCsq, vCOM_DEVICE_EXTENSION, cancelSafeReadQueue);

    	KeAcquireSpinLock(&deviceExtension->readQueueSpinLock, pIrql);
}

VOID vCOMReleaseReadLock(PIO_CSQ pCsq, KIRQL pIrql)
{
	PvCOM_DEVICE_EXTENSION   deviceExtension;

    	deviceExtension = CONTAINING_RECORD(pCsq, vCOM_DEVICE_EXTENSION, cancelSafeReadQueue);
    
    	KeReleaseSpinLock(&deviceExtension->readQueueSpinLock, pIrql);
}

VOID vCOMCompleteCanceledReadIrp(PIO_CSQ pCsq, PIRP pIrp)
{
	KdPrint(("complete canceled read irp: %p\n", pIrp));
	
	pIrp->IoStatus.Status = STATUS_CANCELLED;
    	pIrp->IoStatus.Information = 0;
    	
    	IoCompleteRequest(pIrp, IO_NO_INCREMENT);
}

VOID vCOMInsertWriteIrp (PIO_CSQ pCsq,  PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION   	deviceExtension;

    	deviceExtension = CONTAINING_RECORD(pCsq,  vCOM_DEVICE_EXTENSION, cancelSafeWriteQueue);

    	InsertTailList(&deviceExtension->writePendingQueue, &pIrp->Tail.Overlay.ListEntry);
}

VOID vCOMRemoveWriteIrp(PIO_CSQ pCsq,  PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION	 	deviceExtension;

    	deviceExtension = CONTAINING_RECORD(pCsq, vCOM_DEVICE_EXTENSION, cancelSafeWriteQueue);
    
    	RemoveEntryList(&pIrp->Tail.Overlay.ListEntry);
}

PIRP vCOMPeekNextWriteIrp(PIO_CSQ pCsq, PIRP pIrp,  PVOID pPeekContext)
{
	PvCOM_DEVICE_EXTENSION      	deviceExtension;
    	PIRP                    				nextIrp = NULL;
    	PLIST_ENTRY             			nextEntry;
    	PLIST_ENTRY             			listHead;
    	PIO_STACK_LOCATION     		irpStack;

	deviceExtension = CONTAINING_RECORD(pCsq, vCOM_DEVICE_EXTENSION, cancelSafeWriteQueue);

	listHead = &deviceExtension->writePendingQueue;
	    
	if (pIrp == NULL) 
	{       
	    nextEntry = listHead->Flink;
	} 
	else 
	{
	    nextEntry = pIrp->Tail.Overlay.ListEntry.Flink;
	}

	while (nextEntry != listHead) 
	{
		nextIrp = CONTAINING_RECORD(nextEntry, IRP, Tail.Overlay.ListEntry);

	    	irpStack = IoGetCurrentIrpStackLocation(nextIrp);
	    	    	    
		if (pPeekContext) 
		{
			if (irpStack->FileObject == (PFILE_OBJECT)pPeekContext) 
			{       
		       	break;
		    	}
		}
		
		else 
		{
		    break;
		}
		
		nextIrp = NULL;
		nextEntry = nextEntry->Flink;

	}

	return nextIrp;
	
}

VOID vCOMAcquireWriteLock(PIO_CSQ pCsq, PKIRQL pIrql)
{
	PvCOM_DEVICE_EXTENSION   deviceExtension;

    	deviceExtension = CONTAINING_RECORD(pCsq, vCOM_DEVICE_EXTENSION, cancelSafeWriteQueue);

    	KeAcquireSpinLock(&deviceExtension->writeQueueSpinLock, pIrql);
}

VOID vCOMReleaseWriteLock(PIO_CSQ pCsq, KIRQL pIrql)
{
	PvCOM_DEVICE_EXTENSION   deviceExtension;

    	deviceExtension = CONTAINING_RECORD(pCsq, vCOM_DEVICE_EXTENSION, cancelSafeWriteQueue);
    
    	KeReleaseSpinLock(&deviceExtension->writeQueueSpinLock, pIrql);
}

VOID vCOMCompleteCanceledWriteIrp(PIO_CSQ pCsq, PIRP pIrp)
{
	pIrp->IoStatus.Status = STATUS_CANCELLED;
    	pIrp->IoStatus.Information = 0;
    	
    	IoCompleteRequest(pIrp, IO_NO_INCREMENT);
}

