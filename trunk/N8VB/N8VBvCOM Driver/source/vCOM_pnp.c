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

NTSTATUS vCOMDispatchPnP(PDEVICE_OBJECT pFDO, PIRP pIrp)
{
		NTSTATUS                			status;
		PvCOM_DEVICE_EXTENSION  	pvCOMDevEx;
		PIO_STACK_LOCATION      		pIrpStackLocation;
    		KEVENT                  			event;        
    
    pvCOMDevEx = pFDO->DeviceExtension;
    pIrpStackLocation = IoGetCurrentIrpStackLocation(pIrp);
    
    switch (pIrpStackLocation->MinorFunction)
    {
        case IRP_MN_START_DEVICE:
                        
            pIrp->IoStatus.Status = STATUS_SUCCESS;
            IoSkipCurrentIrpStackLocation(pIrp);
    	     return IoCallDriver(pvCOMDevEx->pNextDriver, pIrp);      

        case IRP_MN_QUERY_STOP_DEVICE:
        case IRP_MN_QUERY_REMOVE_DEVICE:
            
            pIrp->IoStatus.Status = STATUS_SUCCESS;
            IoSkipCurrentIrpStackLocation(pIrp);
    	     return IoCallDriver(pvCOMDevEx->pNextDriver, pIrp);          
                
        case IRP_MN_SURPRISE_REMOVAL:
            
            pvCOMDevEx->isRemoved = TRUE;

            pIrp->IoStatus.Status = STATUS_SUCCESS;
            IoSkipCurrentIrpStackLocation(pIrp);
    	     return IoCallDriver(pvCOMDevEx->pNextDriver, pIrp);      
            
        case IRP_MN_REMOVE_DEVICE:
            
            pvCOMDevEx->isRemoved = TRUE;

            pIrp->IoStatus.Status = STATUS_SUCCESS;
            IoSkipCurrentIrpStackLocation(pIrp);
            status = IoCallDriver(pvCOMDevEx->pNextDriver, pIrp);
	     vCOMDeleteDevice(pFDO);
		           
            return status;

        case IRP_MN_STOP_DEVICE:
        case IRP_MN_CANCEL_REMOVE_DEVICE: 
        case IRP_MN_CANCEL_STOP_DEVICE: 
            
            pIrp->IoStatus.Status = STATUS_SUCCESS;

            break;

        default:
            
            break;
    }

    IoSkipCurrentIrpStackLocation(pIrp);
    return IoCallDriver(pvCOMDevEx->pNextDriver, pIrp);          
     
}

NTSTATUS vCOMCompletionRoutine(PDEVICE_OBJECT DeviceObject, PIRP pIrp, PKEVENT pEvent)
{
    KeSetEvent(pEvent, IO_NO_INCREMENT, FALSE);

    return STATUS_MORE_PROCESSING_REQUIRED;
    
}