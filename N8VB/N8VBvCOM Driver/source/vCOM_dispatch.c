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

NTSTATUS vCOMDispatchCreate(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
    PvCOM_DEVICE_EXTENSION  pDeviceExtension = DeviceObject->DeviceExtension;   	
   	
    return vCOMCreateDevice(DeviceObject, pIrp);    
}

NTSTATUS vCOMDispatchClose(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
    PvCOM_DEVICE_EXTENSION  pDeviceExtension = DeviceObject->DeviceExtension;   
    
    return vCOMCloseDevice(DeviceObject, pIrp);    
}

NTSTATUS vCOMDispatchCleanup(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION  pDeviceExtension = DeviceObject->DeviceExtension;
	
	KdPrint(("In vCOMDispatchCleanup for device %d\n", pDeviceExtension->deviceNumber));

	return vCOMCleanupDevice(DeviceObject, pIrp);      
}

NTSTATUS vCOMDispatchPower(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	NTSTATUS status;
	PvCOM_DEVICE_EXTENSION pDeviceExtension;
	PIO_STACK_LOCATION pDeviceStackLocation;

	KdPrint(("In vCOMDispatchPower...\n"));

	pDeviceExtension = DeviceObject->DeviceExtension;
	pDeviceStackLocation = IoGetCurrentIrpStackLocation(pIrp);

	if (pDeviceExtension->isRemoved)
	{
		PoStartNextPowerIrp(pIrp);

		pIrp->IoStatus.Status = STATUS_DELETE_PENDING;
		IoCompleteRequest(pIrp, IO_NO_INCREMENT);

		return STATUS_DELETE_PENDING;
	}

	pIrp->IoStatus.Status = STATUS_SUCCESS;
	PoStartNextPowerIrp(pIrp);
	IoSkipCurrentIrpStackLocation(pIrp);
	return PoCallDriver(pDeviceExtension->pNextDriver, pIrp); ;
	
}

NTSTATUS vCOMDispatchQueryInformation(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	NTSTATUS status;
	PIO_STACK_LOCATION pIrpStackLocation;
	PFILE_STANDARD_INFORMATION  pStdInfo;
	PFILE_POSITION_INFORMATION  pPosInfo;

	KdPrint(("In vCOMDispatchQueryInformation...\n"));

	pIrpStackLocation = IoGetCurrentIrpStackLocation(pIrp);

	pIrp->IoStatus.Information = 0;

	switch (pIrpStackLocation->Parameters.QueryFile.FileInformationClass)
	{
		case FileStandardInformation:
			if (pIrpStackLocation->Parameters.QueryFile.Length < sizeof(FILE_STANDARD_INFORMATION))
			{
				status = STATUS_BUFFER_TOO_SMALL;
			}
			else
			{
				pStdInfo = pIrp->AssociatedIrp.SystemBuffer;

				pStdInfo->AllocationSize.QuadPart = 0;
				pStdInfo->EndOfFile               = pStdInfo->AllocationSize;
				pStdInfo->NumberOfLinks           = 0;
				pStdInfo->DeletePending           = FALSE;
				pStdInfo->Directory               = FALSE;

				pIrp->IoStatus.Information = sizeof(FILE_STANDARD_INFORMATION);
				status = STATUS_SUCCESS;
			}

			break;

		case FilePositionInformation:
			if (pIrpStackLocation->Parameters.QueryFile.Length < sizeof(FILE_POSITION_INFORMATION))
			{
				status = STATUS_BUFFER_TOO_SMALL;
			}
			else
			{
				pPosInfo = pIrp->AssociatedIrp.SystemBuffer;

				pPosInfo->CurrentByteOffset.QuadPart = 0;

				pIrp->IoStatus.Information = sizeof(FILE_POSITION_INFORMATION);
				status = STATUS_SUCCESS;
			}

			break;

		default:
			status = STATUS_INVALID_PARAMETER;
			break;
			
	}

	pIrp->IoStatus.Status = status;
	IoCompleteRequest(pIrp, IO_NO_INCREMENT);

	KdPrint(("vCOMDispatchQueryInformation done...\n"));
	return status;
	
}

NTSTATUS vCOMDispatchSetInformation(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	NTSTATUS status;
	PIO_STACK_LOCATION pIrpStackLocation;

	KdPrint(("In vCOMDispatchSetInformation...\n"));

	pIrpStackLocation = IoGetCurrentIrpStackLocation(pIrp);

	pIrp->IoStatus.Information = 0;

	switch (pIrpStackLocation->Parameters.QueryFile.FileInformationClass)
	{
		case FileEndOfFileInformation:
			status = STATUS_SUCCESS;
			break;

		case 0x13:  //undocumented FileInformationClass = FileAllocationInformation
			status = STATUS_SUCCESS;
			break;

		default:
			status = STATUS_INVALID_PARAMETER;
			break;
	}

	pIrp->IoStatus.Status = status;
	IoCompleteRequest(pIrp, IO_NO_INCREMENT);

	KdPrint(("vCOMDispatchSetInformation done...\n"));
	return status;
	
}

NTSTATUS vCOMDispatchSystemControl(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION pDeviceExtension;

	KdPrint(("In vCOMDispatchSystemControl...\n"));

	pDeviceExtension = DeviceObject->DeviceExtension;

	pIrp->IoStatus.Status = STATUS_SUCCESS;
	IoSkipCurrentIrpStackLocation(pIrp);	
	return IoCallDriver(pDeviceExtension->pNextDriver, pIrp);	
}
