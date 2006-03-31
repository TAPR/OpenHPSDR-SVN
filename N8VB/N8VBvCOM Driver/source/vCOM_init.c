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

NTSTATUS DriverEntry(PDRIVER_OBJECT  pDriverObject, PUNICODE_STRING pRegistryPath)
{       
	pDriverObject->DriverExtension->AddDevice              				= vCOMAddDevice;
	pDriverObject->DriverUnload                            					= vCOMUnload;    

	pDriverObject->MajorFunction[IRP_MJ_CREATE]            			= vCOMDispatchCreate; 
	pDriverObject->MajorFunction[IRP_MJ_CLOSE]             			= vCOMDispatchClose;
	pDriverObject->MajorFunction[IRP_MJ_CLEANUP]           			= vCOMDispatchCleanup;
	pDriverObject->MajorFunction[IRP_MJ_READ]              			= vCOMReadDevice;
	pDriverObject->MajorFunction[IRP_MJ_WRITE]             			= vCOMWriteDevice;
	pDriverObject->MajorFunction[IRP_MJ_DEVICE_CONTROL]    		= vCOMDeviceControl;
	        
	pDriverObject->MajorFunction[IRP_MJ_QUERY_INFORMATION] 	= vCOMDispatchQueryInformation;
	pDriverObject->MajorFunction[IRP_MJ_SET_INFORMATION]   		= vCOMDispatchSetInformation;

	pDriverObject->MajorFunction[IRP_MJ_POWER]             			= vCOMDispatchPower;
	pDriverObject->MajorFunction[IRP_MJ_SYSTEM_CONTROL]    		= vCOMDispatchSystemControl;
	pDriverObject->MajorFunction[IRP_MJ_PNP]               			= vCOMDispatchPnP;
	
    return STATUS_SUCCESS;
}

VOID vCOMUnload(IN PDRIVER_OBJECT pDriverObject)
{    
}