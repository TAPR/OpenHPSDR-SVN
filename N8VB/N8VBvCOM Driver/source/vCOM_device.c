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

const SERIAL_COMMPROP   vCOMProperties = {sizeof(SERIAL_COMMPROP),      // PacketLength
                                          2,                            				// PacketVersion
                                          SERIAL_SP_SERIALCOMM,         		// ServiceMask
                                          0,                            				// Reserved1
                                          BUFFERLENGTH,                            	// MaxTxQueue
                                          BUFFERLENGTH,                			// MaxRxQueue
                                          SERIAL_BAUD_USER,             		// MaxBaud
                                          SERIAL_SP_RS232,              		// ProvSubType
                                          SERIAL_PCF_DTRDSR |
                                          SERIAL_PCF_RTSCTS |
                                          SERIAL_PCF_CD |
                                          SERIAL_PCF_PARITY_CHECK |
                                          SERIAL_PCF_TOTALTIMEOUTS |
                                          SERIAL_PCF_INTTIMEOUTS,       		// ProvCapabilities
                                          SERIAL_SP_PARITY |
                                          SERIAL_SP_BAUD |
                                          SERIAL_SP_DATABITS |
                                          SERIAL_SP_STOPBITS |
                                          SERIAL_SP_HANDSHAKING |
                                          SERIAL_SP_PARITY_CHECK |
                                          SERIAL_SP_CARRIER_DETECT,    	// SettableParams
                                          SERIAL_BAUD_075 |
                                          SERIAL_BAUD_110 |
                                          SERIAL_BAUD_150 |
                                          SERIAL_BAUD_300 |
                                          SERIAL_BAUD_600 |
                                          SERIAL_BAUD_1200 |
                                          SERIAL_BAUD_1800 |
                                          SERIAL_BAUD_2400 |
                                          SERIAL_BAUD_4800 |
                                          SERIAL_BAUD_7200 |
                                          SERIAL_BAUD_9600 |
                                          SERIAL_BAUD_14400 |
                                          SERIAL_BAUD_19200 |
                                          SERIAL_BAUD_38400 |
                                          SERIAL_BAUD_56K |
                                          SERIAL_BAUD_128K |
                                          SERIAL_BAUD_115200 |
                                          SERIAL_BAUD_57600 |
                                          SERIAL_BAUD_USER,             		// SettableBaud
                                          SERIAL_DATABITS_5 |
                                          SERIAL_DATABITS_6 |
                                          SERIAL_DATABITS_7 |
                                          SERIAL_DATABITS_8,            		// SettableData
                                          SERIAL_STOPBITS_10 |
                                          SERIAL_STOPBITS_15 |
                                          SERIAL_STOPBITS_20 |
                                          SERIAL_PARITY_NONE |
                                          SERIAL_PARITY_ODD |
                                          SERIAL_PARITY_EVEN |
                                          SERIAL_PARITY_MARK |
                                          SERIAL_PARITY_SPACE,          		// SettableStopParity
                                          BUFFERLENGTH,                            	// CurrentTxQueue
                                          BUFFERLENGTH,                			// CurrentRxQueue
                                          0,                            				// ProvSpec1
                                          0,                            				// ProvSpec2
                                          0,                            				// ProvChar
                                         };
                                       
NTSTATUS vCOMAddDevice(PDRIVER_OBJECT DriverObject, PDEVICE_OBJECT pPDO)
{
	NTSTATUS                    		status;
	PDEVICE_OBJECT              		pFDO;
	PDEVICE_OBJECT									pTwinFDO;
	PvCOM_DEVICE_EXTENSION      		pDeviceExtension;
	PvCOM_DEVICE_EXTENSION    			pTwinDeviceExtension;
	HANDLE                  				hRegistry;
	ULONG                  					pCount = 0;
	ULONG														i;

	status = IoOpenDeviceRegistryKey(pPDO, PLUGPLAY_REGKEY_DEVICE,	STANDARD_RIGHTS_READ, &hRegistry);

	if (!NT_SUCCESS(status))
	{
		KdPrint(("error in AddDevice IoOpenDeviceRegistryKey...\n"));
		return status;
	}
		
	status = vCOMGetRegistryKeyValue(hRegistry, L"NumberOfPairs", &pCount, sizeof(ULONG));
		
	if (!NT_SUCCESS(status))
	{		
		ZwClose(hRegistry);
		KdPrint(("AddDevice vCOMGetRegistryKeyValue failed...\n"));
		return status;
	}

	devpaircount = (ULONG)pCount;
		
	ZwClose(hRegistry);	

	if (devpaircount > MAX_PAIR_COUNT)
		devpaircount = MAX_PAIR_COUNT;

	KdPrint(("AddDevice will create %d pairs or %d devices...\n", devpaircount, devpaircount*2));
		
	for (i=0; i < devpaircount*2; i+=2)
	{
		status = vCOMCreateFDO(DriverObject, pPDO, &pFDO, i);

		if (!NT_SUCCESS(status))
		{
			return status;
		}

		KdPrint(("Device %d created...\n", i));
		
		pFDO->Flags &= ~DO_DEVICE_INITIALIZING;

		status = vCOMCreateFDO(DriverObject, pPDO, &pTwinFDO, i+1);

		if (!NT_SUCCESS(status))
		{
			return status;
		}

		KdPrint(("Device %d created...\n", i+1));

		pTwinFDO->Flags &= ~DO_DEVICE_INITIALIZING;

		pDeviceExtension = (PvCOM_DEVICE_EXTENSION)pFDO->DeviceExtension;
		pTwinDeviceExtension = (PvCOM_DEVICE_EXTENSION)pTwinFDO->DeviceExtension;

		pDeviceExtension->pMe = pFDO;
		pDeviceExtension->pTwin = pTwinFDO;
		pTwinDeviceExtension->pMe = pTwinFDO;
		pTwinDeviceExtension->pTwin = pFDO;

		pTwinDeviceExtension->pNextDriver = pDeviceExtension->pNextDriver;

		pDeviceExtension->inBuffer = (PUCHAR) ExAllocatePool(NonPagedPool, BUFFERLENGTH);

		if (pDeviceExtension->inBuffer == NULL)
			return STATUS_INSUFFICIENT_RESOURCES;
			
		pDeviceExtension->outBuffer = (PUCHAR) ExAllocatePool(NonPagedPool, BUFFERLENGTH);

		if (pDeviceExtension->outBuffer == NULL)
			return STATUS_INSUFFICIENT_RESOURCES;
		
		pDeviceExtension->inBufferSize = BUFFERLENGTH;
		pDeviceExtension->outBufferSize = BUFFERLENGTH;
			
		pDeviceExtension->inBufferReadPosition = 0;
		pDeviceExtension->inBufferWritePosition = 0;
		pDeviceExtension->outBufferReadPosition = 0;
		pDeviceExtension->outBufferWritePosition = 0;
		pDeviceExtension->inBufferCount = 0;
		pDeviceExtension->outBufferCount = 0;
		pDeviceExtension->pReadIrpCurrent = NULL;
		pDeviceExtension->pWriteIrpCurrent = NULL;
		pDeviceExtension->fastRead = FALSE;

		//default delay loop timeout is 20 mS
		pDeviceExtension->writeDelayLoopTimerValue.QuadPart = (LONGLONG)(-10000 * 20);
		// 60 secs
		pDeviceExtension->readWaitTimeoutValue.QuadPart = (LONGLONG)(-10000 * 60000);
		// 5 secs
		pDeviceExtension->transferWaitTimeoutValue.QuadPart = (LONGLONG)(-10000 * 5000);
	
		pTwinDeviceExtension->inBuffer = (PUCHAR) ExAllocatePool(NonPagedPool, BUFFERLENGTH);

		if (pTwinDeviceExtension->inBuffer == NULL)
			return STATUS_INSUFFICIENT_RESOURCES;
			
		pTwinDeviceExtension->outBuffer = (PUCHAR) ExAllocatePool(NonPagedPool, BUFFERLENGTH);

		if (pTwinDeviceExtension->outBuffer == NULL)
			return STATUS_INSUFFICIENT_RESOURCES;
		
		pTwinDeviceExtension->inBufferSize = BUFFERLENGTH;
		pTwinDeviceExtension->outBufferSize = BUFFERLENGTH;
		
		pTwinDeviceExtension->inBufferReadPosition = 0;
		pTwinDeviceExtension->inBufferWritePosition = 0;
		pTwinDeviceExtension->outBufferReadPosition = 0;
		pTwinDeviceExtension->outBufferWritePosition = 0;
		pTwinDeviceExtension->inBufferCount = 0;
		pTwinDeviceExtension->outBufferCount = 0;	
		pTwinDeviceExtension->pReadIrpCurrent = NULL;
		pTwinDeviceExtension->pWriteIrpCurrent = NULL;
		pTwinDeviceExtension->fastRead = FALSE;	

		//default delay loop timeout is 20 mS
		pTwinDeviceExtension->writeDelayLoopTimerValue.QuadPart = (LONGLONG)(-10000 * 20);
		// 60 secs
		pTwinDeviceExtension->readWaitTimeoutValue.QuadPart = (LONGLONG)(-10000 * 60000);
		// 5 secs
		pTwinDeviceExtension->transferWaitTimeoutValue.QuadPart = (LONGLONG)(-10000 * 5000);

		pDevices[i] = pFDO;
		pDevices[i+1] = pTwinFDO;

		if (pDeviceExtension->deviceNumber == 0) //only register once
		{
			status = IoRegisterDeviceInterface(pPDO, &vCOMGUID,  NULL, &pDeviceExtension->interfaceName);

			if (!NT_SUCCESS(status))
			{
				pDeviceExtension->interfaceName.Buffer = NULL;
				KdPrint(("IoRegisterDeviceInterface failed...\n"));
				goto StartThreads;
			}	
			
			status = IoSetDeviceInterfaceState(&pDeviceExtension->interfaceName, TRUE);

			if (!NT_SUCCESS(status))
			{
				pDeviceExtension->isSet = FALSE;
				KdPrint(("IoSetDeviceInterfaceState failed...\n"));
				goto StartThreads;
			}

			pDeviceExtension->isSet = TRUE;
		}

StartThreads:
	
		vCOMTransferThreadStart(pFDO);
		vCOMTransferThreadStart(pTwinFDO);
		vCOMReadThreadStart(pFDO);			 
		vCOMReadThreadStart(pTwinFDO); 
		vCOMWriteThreadStart(pFDO);		
		vCOMWriteThreadStart(pTwinFDO); 
	
	}
				
	return STATUS_SUCCESS;
	
}

NTSTATUS vCOMCreateFDO(PDRIVER_OBJECT DriverObject, PDEVICE_OBJECT pPDO, PDEVICE_OBJECT* pvCOMFDO, ULONG devNumber)
{
	NTSTATUS                			status;
	PvCOM_DEVICE_EXTENSION  	pDeviceExtension;
	UNICODE_STRING          		ntDeviceName;
	PUNICODE_STRING         		pSymbolicName;
	HANDLE                  			hRegistry;
	WCHAR*                  			pRegistryName;

	RtlInitUnicodeString(&ntDeviceName, vCOMDevNames[devNumber]);

	status = IoCreateDevice(DriverObject, sizeof(vCOM_DEVICE_EXTENSION), &ntDeviceName,
			FILE_DEVICE_SERIAL_PORT, 0, TRUE, pvCOMFDO);

	if (!NT_SUCCESS(status))
	{
		KdPrint(("error creating device...\n"));
		return status;
	}

	KdPrint(("IOCreateDevice succeeded...\n"));

	pDeviceExtension = (PvCOM_DEVICE_EXTENSION)(*pvCOMFDO)->DeviceExtension;

	status = IoOpenDeviceRegistryKey(pPDO, PLUGPLAY_REGKEY_DEVICE,	STANDARD_RIGHTS_READ, &hRegistry);

	if (!NT_SUCCESS(status))
	{
		vCOMDeleteFailedDevice(*pvCOMFDO, devNumber);
		KdPrint(("error in IoOpenDeviceRegistryKey...\n"));
		return status;
	}

	KdPrint(("IoOpenDeviceRegistryKey succeeded...\n"));

	pRegistryName = ExAllocatePool(PagedPool, 129 * sizeof(WCHAR));

	if (pRegistryName == NULL)
	{
		ZwClose(hRegistry);
		vCOMDeleteFailedDevice(*pvCOMFDO, devNumber);
		KdPrint(("error in ExAllocatePool for registry name...\n"));
		return STATUS_INSUFFICIENT_RESOURCES;
	}

	pSymbolicName = &pDeviceExtension->symbolicLinkName;

	pSymbolicName->MaximumLength = 128 * sizeof(WCHAR);
	pSymbolicName->Buffer = ExAllocatePool(PagedPool, 129 * sizeof(WCHAR));

	if (pSymbolicName->Buffer == NULL)
	{
		ZwClose(hRegistry);
		vCOMDeleteFailedDevice(*pvCOMFDO, devNumber);
		KdPrint(("error in ExAllocatePool for symbolic name...\n"));
		return STATUS_INSUFFICIENT_RESOURCES;
	}

	KdPrint(("calling vCOMGetRegistryKeyValue...\n"));

	status = vCOMGetRegistryKeyValue(hRegistry, vCOMPortNumbers[devNumber], pRegistryName, 128 * sizeof(WCHAR));
		
	KdPrint(("Device %d will be created with name: %ws -- %ws\n", devNumber, pRegistryName, vCOMPortNumbers[devNumber]));
	
	if (!NT_SUCCESS(status))
	{		
		ZwClose(hRegistry);
		vCOMDeleteFailedDevice(*pvCOMFDO, devNumber);
		KdPrint(("vCOMGetRegistryKeyValue failed...\n"));
		return status;
	}

	KdPrint(("vCOMGetRegistryKeyValue succeeded...\n"));

	RtlZeroMemory(pSymbolicName->Buffer, 129 * sizeof(WCHAR));
	RtlAppendUnicodeToString(pSymbolicName, L"\\DosDevices\\");
	RtlAppendUnicodeToString(pSymbolicName, pRegistryName);

	status = IoCreateSymbolicLink(pSymbolicName, &ntDeviceName);

	if (!NT_SUCCESS(status))  //try to delete the symbolic link first
	{
		IoDeleteSymbolicLink(pSymbolicName);
		status = IoCreateSymbolicLink(pSymbolicName, &ntDeviceName);
	}
	
	if (!NT_SUCCESS(status)) //deleting failed
	{
		pDeviceExtension->symbolicLinkIsDone = FALSE;
		ZwClose(hRegistry);		
		vCOMDeleteFailedDevice(*pvCOMFDO, devNumber);
		KdPrint(("IoCreateSymbolicLink failed...\n"));
		return status;
	}

	KdPrint(("IoCreateSymbolicLink succeeded: %ws\n", pSymbolicName));
	
	pDeviceExtension->symbolicLinkIsDone = TRUE;
		
	status = RtlWriteRegistryValue(RTL_REGISTRY_DEVICEMAP, SERIAL_DEVICE_MAP, ntDeviceName.Buffer, REG_SZ,
			pRegistryName, (wcslen(pRegistryName) + 1) * sizeof(WCHAR));

	if (!NT_SUCCESS(status))
	{
		pDeviceExtension->deviceMapEntryIsCreated = FALSE;
		ZwClose(hRegistry);
		vCOMDeleteFailedDevice(*pvCOMFDO, devNumber);
		KdPrint(("RtlWriteRegistryValue failed...\n"));
		return status;
	}

	pDeviceExtension->deviceMapEntryIsCreated = TRUE;
		
	ZwClose(hRegistry);

	(*pvCOMFDO)->Flags |= DO_POWER_PAGABLE + DO_BUFFERED_IO;
	
	pDeviceExtension->pMe = pPDO;

	if (devNumber == 0) //don't attach device n+1 or it will be above device 1!!!
	{
		pDeviceExtension->pNextDriver = IoAttachDeviceToDeviceStack(*pvCOMFDO, pPDO);

		if (pDeviceExtension->pNextDriver == NULL)
		{
			vCOMDeleteFailedDevice(*pvCOMFDO, devNumber);
			return STATUS_NO_SUCH_DEVICE;
		}  

		pDeviceExtension->isAttached = TRUE;		
	}
	else
	{
		pDeviceExtension->isAttached = FALSE;
	}

	pDeviceExtension->deviceNumber = devNumber;
	
	// initialize objects
	ExInitializeFastMutex(&pDeviceExtension->openMutex);
	
	KeInitializeSpinLock(&pDeviceExtension->readSpinLock);    
	KeInitializeSpinLock(&pDeviceExtension->writeSpinLock);
	KeInitializeSpinLock(&pDeviceExtension->controlSpinLock);
	KeInitializeSpinLock(&pDeviceExtension->readQueueSpinLock);
	KeInitializeSpinLock(&pDeviceExtension->writeQueueSpinLock);

	KeInitializeSemaphore(&pDeviceExtension->readIRPQueueSemaphore, 0, MAXLONG);
	KeInitializeSemaphore(&pDeviceExtension->writeIRPQueueSemaphore, 0, MAXLONG);
		
	KeInitializeTimerEx(&pDeviceExtension->readTotalTimer, SynchronizationTimer);
	KeInitializeTimerEx(&pDeviceExtension->readIntervalTimer, SynchronizationTimer);
	KeInitializeTimerEx(&pDeviceExtension->writeTotalTimer, SynchronizationTimer);
	
	KeInitializeEvent(&pDeviceExtension->transferNeededEvent, SynchronizationEvent, FALSE);
	KeInitializeEvent(&pDeviceExtension->transferOccurredEvent, SynchronizationEvent, FALSE);
	KeInitializeEvent(&pDeviceExtension->readOccurredEvent, SynchronizationEvent, FALSE);
	KeInitializeEvent(&pDeviceExtension->currentReadIrpWasCancelledEvent, SynchronizationEvent, FALSE);
	KeInitializeEvent(&pDeviceExtension->currentWriteIrpWasCancelledEvent, SynchronizationEvent, FALSE);
	
	InitializeListHead(&pDeviceExtension->readPendingQueue);
	InitializeListHead(&pDeviceExtension->writePendingQueue);

	IoCsqInitialize (&pDeviceExtension->cancelSafeReadQueue,
		                     vCOMInsertReadIrp,
		                     vCOMRemoveReadIrp,
		                     vCOMPeekNextReadIrp,
		                     vCOMAcquireReadLock,
		                     vCOMReleaseReadLock,
		                     vCOMCompleteCanceledReadIrp );

	IoCsqInitialize (&pDeviceExtension->cancelSafeWriteQueue,
		                     vCOMInsertWriteIrp,
		                     vCOMRemoveWriteIrp,
		                     vCOMPeekNextWriteIrp,
		                     vCOMAcquireWriteLock,
		                     vCOMReleaseWriteLock,
		                     vCOMCompleteCanceledWriteIrp );
	
	KdPrint(("Device %d created\n", devNumber));

	return STATUS_SUCCESS;
		
}

VOID vCOMDeleteFailedDevice(PDEVICE_OBJECT pFDO, ULONG devNumber)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension = pFDO->DeviceExtension;
	UNICODE_STRING          		ntDeviceName;

	vCOMTransferThreadStop(pFDO);
	vCOMReadThreadStop(pFDO);
	vCOMWriteThreadStop(pFDO);
	
	pDeviceExtension->isOpened = FALSE;
	pDeviceExtension->isRemoved = TRUE;	

	if (pDeviceExtension->symbolicLinkIsDone)
		IoDeleteSymbolicLink(&pDeviceExtension->symbolicLinkName);	

	if (pDeviceExtension->symbolicLinkName.Buffer != NULL)
		ExFreePool(pDeviceExtension->symbolicLinkName.Buffer);	

	if ((pDeviceExtension->isSet == TRUE) && (pDeviceExtension->deviceNumber == 0))
		IoSetDeviceInterfaceState(&pDeviceExtension->interfaceName, FALSE);
	
	if (pDeviceExtension->interfaceName.Buffer != NULL)
	{		
		ExFreePool(pDeviceExtension->interfaceName.Buffer);
	}	

	if (pDeviceExtension->deviceMapEntryIsCreated)
	{
		RtlInitUnicodeString(&ntDeviceName, vCOMDevNames[devNumber]);
		RtlDeleteRegistryValue(RTL_REGISTRY_DEVICEMAP, SERIAL_DEVICE_MAP, ntDeviceName.Buffer);
	}
		
	if (pDeviceExtension->isAttached && devNumber == 0)
		IoDetachDevice(pDeviceExtension->pNextDriver);
	
	if (pFDO != NULL)
		IoDeleteDevice(pFDO);
}

VOID vCOMDeleteDevice(PDEVICE_OBJECT pFDO)
{
	PvCOM_DEVICE_EXTENSION		pCurrentDeviceExtension; 
	UNICODE_STRING          		ntDeviceName;	
	ULONG						i;
	
	for (i=0; i < MAX_PAIR_COUNT * 2; i++)
	{
		KdPrint(("Deleting device %d\n", i));
		
		if (pDevices[i] == NULL)
		{
			KdPrint(("pDevices %d was NULL - break...\n", i));
			break;
		}
		
		pCurrentDeviceExtension = pDevices[i]->DeviceExtension;

		if (pCurrentDeviceExtension == NULL)
		{
			KdPrint(("pCurrentDeviceExtension %d was NULL - break...\n", i));
			break;
		}			

		KdPrint(("Stopping Threads for %d \n", i));
		
		vCOMTransferThreadStop(pCurrentDeviceExtension->pMe);	
		vCOMReadThreadStop(pCurrentDeviceExtension->pMe);
		vCOMWriteThreadStop(pCurrentDeviceExtension->pMe);
				
		pCurrentDeviceExtension->isOpened = FALSE;
				
		pCurrentDeviceExtension->isRemoved = TRUE;
				
		if (pCurrentDeviceExtension->symbolicLinkIsDone)
			IoDeleteSymbolicLink(&pCurrentDeviceExtension->symbolicLinkName);
		
		if (pCurrentDeviceExtension->symbolicLinkName.Buffer != NULL)
			ExFreePool(pCurrentDeviceExtension->symbolicLinkName.Buffer);		

		if ((pCurrentDeviceExtension->isSet == TRUE) && (pCurrentDeviceExtension->deviceNumber == 0))
			IoSetDeviceInterfaceState(&pCurrentDeviceExtension->interfaceName, FALSE);
		
		if (pCurrentDeviceExtension->interfaceName.Buffer != NULL)
		{
			ExFreePool(pCurrentDeviceExtension->interfaceName.Buffer);
		}
				
		if (pCurrentDeviceExtension->deviceMapEntryIsCreated)
		{
			RtlInitUnicodeString(&ntDeviceName, vCOMDevNames[i]);
			RtlDeleteRegistryValue(RTL_REGISTRY_DEVICEMAP, SERIAL_DEVICE_MAP, ntDeviceName.Buffer);
		}
				
		KeCancelTimer(&pCurrentDeviceExtension->readIntervalTimer);
		KeCancelTimer(&pCurrentDeviceExtension->readTotalTimer);
		KeCancelTimer(&pCurrentDeviceExtension->writeTotalTimer);
		
	}	

	KdPrint(("IoDetachDevice device 0\n"));
	
	pCurrentDeviceExtension = pFDO->DeviceExtension;

	if ((pCurrentDeviceExtension->isAttached == TRUE) & (pCurrentDeviceExtension->deviceNumber == 0))
			IoDetachDevice(pCurrentDeviceExtension->pNextDriver);
	
	for (i=1; i<MAX_PAIR_COUNT * 2; i++)
	{
		if (pDevices[i] != NULL)
		{
			IoDeleteDevice(pDevices[i]);
			pDevices[i] = NULL;
			KdPrint(("IoDeleteDevice for device %d\n", i));
		}
		else
		{
			KdPrint(("IoDeleteDevice pDevices %d was NULL - break...\n", i));
			break;
		}
	}		

	KdPrint(("IoDeleteDevice for device 0\n"));
	
	if (pFDO != NULL)
			IoDeleteDevice(pFDO);
	
}

NTSTATUS vCOMCreateDevice(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	NTSTATUS 					status;
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension;

	pDeviceExtension = DeviceObject->DeviceExtension;
	
	if ((IoGetCurrentIrpStackLocation(pIrp)->Parameters.Create.Options & FILE_DIRECTORY_FILE) != 0)
		status = STATUS_NOT_A_DIRECTORY;
	else
	{
			ExAcquireFastMutex(&pDeviceExtension->openMutex);

		if (pDeviceExtension->isOpened)
		{
			status = STATUS_ACCESS_DENIED;
			ExReleaseFastMutex(&pDeviceExtension->openMutex);
		}
		else
		{
			pDeviceExtension->eventMask = 0;
			pDeviceExtension->eventHistory = 0;
			pDeviceExtension->isOpened = TRUE;
						
			ExReleaseFastMutex(&pDeviceExtension->openMutex);

			status = STATUS_SUCCESS;
		}
	}

	KdPrint(("Creating File device %d done...", pDeviceExtension->deviceNumber));	
	
	return vCOMCompleteRequest(pIrp, status, 0);
		
}

NTSTATUS vCOMCloseDevice(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	NTSTATUS 					status;
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension;

	pDeviceExtension = DeviceObject->DeviceExtension;

	pDeviceExtension->isOpened = FALSE;	
	
	KdPrint(("Closing File device %d done...", pDeviceExtension->deviceNumber));
	
	return vCOMCompleteRequest(pIrp, STATUS_SUCCESS, 0);
	
}

NTSTATUS vCOMReadDevice(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension;
    			
	pDeviceExtension = DeviceObject->DeviceExtension;    	

	//this is a fix to make the driver work with MSCOMM in return with any mode
	//see http://support.microsoft.com/default.aspx?scid=kb;en-us;318784
	//driver mode is TIMEOUTS_IMMEDIATE_RETURN_WITH_BYTES
	if (vCOMFastRead(DeviceObject, pIrp) == STATUS_SUCCESS)
	{
		pIrp->IoStatus.Status = STATUS_SUCCESS;
		IoCompleteRequest(pIrp, IO_NO_INCREMENT);	
		return STATUS_SUCCESS;
	}
	else
	{
		IoCsqInsertIrp(&pDeviceExtension->cancelSafeReadQueue, pIrp, NULL);
		KeReleaseSemaphore(&pDeviceExtension->readIRPQueueSemaphore, 0, 1, FALSE);	
		return STATUS_PENDING;
	}		
	
}

NTSTATUS vCOMFastRead(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension;
	PvCOM_DEVICE_EXTENSION 	pTwinDeviceExtension;
    	PIO_STACK_LOCATION  		pIrpStackLocation;
	KIRQL 						read_oldIrql;
	ULONG 						bufferLength;
    	PCHAR 						pBuffer;
	
	pDeviceExtension = DeviceObject->DeviceExtension; 
	pTwinDeviceExtension = pDeviceExtension->pTwin->DeviceExtension;
	
	pIrpStackLocation = IoGetCurrentIrpStackLocation(pIrp);
	pBuffer = pIrp->AssociatedIrp.SystemBuffer;		
	bufferLength = pIrpStackLocation->Parameters.Read.Length;

	if (bufferLength == 0)
		return STATUS_SUCCESS;
		
	if (pDeviceExtension->outBufferCount >= bufferLength || pDeviceExtension->fastRead)
	{				
		pDeviceExtension->readBufferCount = 0;

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
		
		KeSetEvent(&pTwinDeviceExtension->readOccurredEvent, 0, FALSE);
		
		return STATUS_SUCCESS;
		
	}
	else
	{		
		return STATUS_PENDING;
	}	
	
}

NTSTATUS vCOMWriteDevice(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension;
    	PIO_STACK_LOCATION 		pIrpStackLocation;
    			
	pDeviceExtension = DeviceObject->DeviceExtension;

	if (vCOMFastWrite(DeviceObject, pIrp) == STATUS_SUCCESS)
	{
		pIrp->IoStatus.Status = STATUS_SUCCESS;
		IoCompleteRequest(pIrp, IO_NO_INCREMENT);	
		return STATUS_SUCCESS;
	}
	else
	{
		IoCsqInsertIrp(&pDeviceExtension->cancelSafeWriteQueue, pIrp, NULL);
		KeReleaseSemaphore(&pDeviceExtension->writeIRPQueueSemaphore, 0, 1, FALSE);
		return STATUS_PENDING;
	}	
}

NTSTATUS vCOMFastWrite(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension =  DeviceObject->DeviceExtension;
	PvCOM_DEVICE_EXTENSION 	pTwinDeviceExtension = pDeviceExtension->pTwin->DeviceExtension;
	PIO_STACK_LOCATION 		pIrpStackLocation;
    	KIRQL 						write_oldIrql;
	KIRQL 						read_oldIrql;
    	ULONG 						dataLength = 0;
    	PCHAR 						pData = NULL;
    	ULONG 						count = 0;	
	
	pIrpStackLocation = IoGetCurrentIrpStackLocation(pIrp); 
	dataLength = pIrpStackLocation->Parameters.Read.Length;
	pData = pIrp->AssociatedIrp.SystemBuffer;

	if (dataLength == 0)
		return STATUS_SUCCESS;
		
	if ((pDeviceExtension->inBufferSize - pDeviceExtension->inBufferCount) >= dataLength) //room in buffer
	{
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
		
		vCOMCheckComEvents(pDeviceExtension, SERIAL_EV_TXEMPTY);

		if (vCOMFastTransfer(DeviceObject, pIrp) == STATUS_PENDING)
			KeSetEvent(&pDeviceExtension->transferNeededEvent, 8, FALSE);

		return STATUS_SUCCESS;
		
	}
	else //not enough room
	{
		return STATUS_PENDING;
	}		
}

NTSTATUS vCOMFastTransfer(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension =  DeviceObject->DeviceExtension;
	PvCOM_DEVICE_EXTENSION 	pTwinDeviceExtension = pDeviceExtension->pTwin->DeviceExtension;
    	KIRQL                       			oldIrql;
	ULONG                       			count;
	ULONG						events;
	UCHAR						byte = 0;
	BOOLEAN						eventCharRcvd = FALSE;
	KIRQL 						read_oldIrql;
	KIRQL 						write_oldIrql;
	
	eventCharRcvd = FALSE;
			
	if ((pTwinDeviceExtension->outBufferSize - pTwinDeviceExtension->outBufferCount) < pDeviceExtension->inBufferCount)
	{
		return STATUS_PENDING;
	}
	else
	{
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
		KeSetEvent(&pTwinDeviceExtension->transferOccurredEvent, 6, FALSE);

		return STATUS_SUCCESS;	
	}				
		
}

NTSTATUS vCOMDeviceControl(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	NTSTATUS 					status;
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension;
	PvCOM_DEVICE_EXTENSION 	pTwinDeviceExtension;
    	PIO_STACK_LOCATION 		pIrpStackLocation; 
	KIRQL 						oldIrql;
    	KIRQL 						write_oldIrql;
	KIRQL 						read_oldIrql;
	KIRQL						cancelIrql;
	PIRP            					pOldWaitIrp;
	PDRIVER_CANCEL 				cancelRoutine;
	PDRIVER_CANCEL 				pOldCancelRoutine;
	
	status = STATUS_SUCCESS;

	pDeviceExtension = DeviceObject->DeviceExtension;
	pTwinDeviceExtension = pDeviceExtension->pTwin->DeviceExtension;

	pIrpStackLocation = IoGetCurrentIrpStackLocation(pIrp);                     

	pIrp->IoStatus.Information = 0;

	if (status == STATUS_CANCELLED)
	{
		return vCOMCompleteRequest(pIrp,  status, 0);
	}
		
	KdPrint(("vCOMDeviceControl: IOControlCode = %x\n", pIrpStackLocation->Parameters.DeviceIoControl.IoControlCode));
						
	switch (pIrpStackLocation->Parameters.DeviceIoControl.IoControlCode)
	{
		
		case IOCTL_SERIAL_SET_TIMEOUTS:
		{
				
			PSERIAL_TIMEOUTS	pNewTimeouts;
			
			pNewTimeouts = (PSERIAL_TIMEOUTS)pIrp->AssociatedIrp.SystemBuffer;

			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_SET_TIMEOUTS\n"));
						
			if (pIrpStackLocation->Parameters.DeviceIoControl.InputBufferLength < sizeof(SERIAL_TIMEOUTS))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			if ((pNewTimeouts->ReadIntervalTimeout == MAXULONG) &&
			    (pNewTimeouts->ReadTotalTimeoutMultiplier == MAXULONG) &&
			    (pNewTimeouts->ReadTotalTimeoutConstant == MAXULONG))
			{
				status = STATUS_INVALID_PARAMETER;
				break;
			}

			//this is a fix to make the driver work with MSCOMM in return with any mode
			//see http://support.microsoft.com/default.aspx?scid=kb;en-us;318784
			//driver mode is TIMEOUTS_IMMEDIATE_RETURN_WITH_BYTES
			if ((pNewTimeouts->ReadIntervalTimeout == MAXULONG) &&
			    (pNewTimeouts->ReadTotalTimeoutMultiplier == 0) &&
			    (pNewTimeouts->ReadTotalTimeoutConstant == 0))
			{
				pDeviceExtension->fastRead = TRUE;
			}
			else
				pDeviceExtension->fastRead = FALSE;
				
			KeAcquireSpinLock(&pDeviceExtension->readSpinLock, &oldIrql);

			RtlMoveMemory(&pDeviceExtension->serTimeouts, pNewTimeouts, sizeof(SERIAL_TIMEOUTS));

			KeReleaseSpinLock(&pDeviceExtension->readSpinLock, oldIrql);

			KdPrint(("ReadIntervalTimeout: %lu\n", pNewTimeouts->ReadIntervalTimeout));
			KdPrint(("ReadTotalTimeoutMultiplier: %lu\n", pNewTimeouts->ReadTotalTimeoutMultiplier));
			KdPrint(("ReadTotalTimeoutConstant: %lu\n", pNewTimeouts->ReadTotalTimeoutConstant));
						
		      	break;
		}
	    
		case IOCTL_SERIAL_GET_TIMEOUTS:
	  	
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_GET_TIMEOUTS\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(SERIAL_TIMEOUTS))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			pIrp->IoStatus.Information = sizeof(SERIAL_TIMEOUTS);

			KeAcquireSpinLock(&pDeviceExtension->readSpinLock, &oldIrql);

			RtlMoveMemory(pIrp->AssociatedIrp.SystemBuffer, &pDeviceExtension->serTimeouts, sizeof(SERIAL_TIMEOUTS));

			KeReleaseSpinLock(&pDeviceExtension->readSpinLock, oldIrql);

			break;	    

		case IOCTL_SERIAL_GET_WAIT_MASK:
				
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_GET_WAIT_MASK\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(ULONG))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			pIrp->IoStatus.Information = sizeof(ULONG);

			*(PULONG)pIrp->AssociatedIrp.SystemBuffer = pDeviceExtension->eventMask;

			break;
	     	
	     
		case IOCTL_SERIAL_SET_WAIT_MASK:
		{
			ULONG   newMask;
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_SET_WAIT_MASK\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.InputBufferLength < sizeof(ULONG))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			newMask = *(PULONG)pIrp->AssociatedIrp.SystemBuffer;

			if ((newMask & ~(SERIAL_EV_RXCHAR | SERIAL_EV_RXFLAG | SERIAL_EV_TXEMPTY | SERIAL_EV_CTS |
							SERIAL_EV_DSR | SERIAL_EV_RLSD | SERIAL_EV_BREAK | SERIAL_EV_ERR |
							SERIAL_EV_RING | SERIAL_EV_PERR | SERIAL_EV_RX80FULL | SERIAL_EV_EVENT1 |
							SERIAL_EV_EVENT2)) != 0)
			{
				status = STATUS_INVALID_PARAMETER;
				break;
			}

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			pOldWaitIrp = pDeviceExtension->pWaitIrp;

			if (pOldWaitIrp != NULL)
			{
				pOldCancelRoutine = IoSetCancelRoutine(pOldWaitIrp, NULL);

				if (pOldCancelRoutine != NULL)
				{
					pOldWaitIrp->IoStatus.Information = sizeof(ULONG);

					*(PULONG)pOldWaitIrp->AssociatedIrp.SystemBuffer = 0;

					pOldWaitIrp->IoStatus.Status = STATUS_SUCCESS;

					pDeviceExtension->pWaitIrp = NULL;
				}
				else
				{
				pOldWaitIrp = NULL;
				}
			}

			pDeviceExtension->eventMask = newMask;

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);

			if (pOldWaitIrp != NULL)
				IoCompleteRequest(pOldWaitIrp, IO_NO_INCREMENT);

			break;
		}
	    
		case IOCTL_SERIAL_WAIT_ON_MASK:			

			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_WAIT_ON_MASK\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(ULONG))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			if ((pDeviceExtension->pWaitIrp != NULL) || (pDeviceExtension->eventMask == 0))
			{
				status = STATUS_INVALID_PARAMETER;
			}
			else if ((pDeviceExtension->eventMask & pDeviceExtension->eventHistory) != 0)
			{
				pIrp->IoStatus.Information = sizeof(ULONG);
				*(PULONG)pIrp->AssociatedIrp.SystemBuffer = pDeviceExtension->eventMask & pDeviceExtension->eventHistory;

				pDeviceExtension->eventHistory = 0;

				KdPrint(("IOCTL_SERIAL_WAIT_ON_MASK set with STATUS_SUCCESS...\n"));
				status = STATUS_SUCCESS;
			}
			else
			{
				pDeviceExtension->pWaitIrp = pIrp;

				status = STATUS_PENDING;

				IoSetCancelRoutine(pIrp, vCOMCancelWaitIrp);

				if (pIrp->Cancel)
				{
				pOldCancelRoutine = IoSetCancelRoutine(pIrp, NULL);

					if (pOldCancelRoutine != NULL)
					{
						KdPrint(("IOCTL_SERIAL_WAIT_ON_MASK set with STATUS_CANCELLED...\n"));
						status = STATUS_CANCELLED;
						pDeviceExtension->pWaitIrp = NULL;
					}
					else
					{
						KdPrint(("IOCTL_SERIAL_WAIT_ON_MASK set with STATUS_PENDING...\n"));
						IoMarkIrpPending(pIrp);
					}
				}
				else
				{
					KdPrint(("IOCTL_SERIAL_WAIT_ON_MASK set with STATUS_PENDING...\n"));
					IoMarkIrpPending(pIrp);
				}
				
			}

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);

			break;
	  	
	    	
		case IOCTL_SERIAL_PURGE:
		{
			ULONG   mask;
			ULONG   channels;	

			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_PURGE\n"));
			
			if (pIrpStackLocation->Parameters.DeviceIoControl.InputBufferLength < sizeof(ULONG))
			{
				pIrp->IoStatus.Information = 0;
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			mask = *(PULONG)pIrp->AssociatedIrp.SystemBuffer;

			KdPrint(("purge mask: %x\n", mask));
			
			if ((mask == 0) || ((mask & ~(SERIAL_PURGE_TXABORT | SERIAL_PURGE_RXABORT |
								SERIAL_PURGE_TXCLEAR | SERIAL_PURGE_RXCLEAR)) != 0))
			{
				pIrp->IoStatus.Information = 0;
				status = STATUS_INVALID_PARAMETER;
				break;
			}

			if (mask & SERIAL_PURGE_RXABORT)
			{
				KdPrint(("purge SERIAL_PURGE_RXABORT\n"));

				if (pDeviceExtension->pReadIrpCurrent != NULL)
				{
					IoAcquireCancelSpinLock(&cancelIrql);

					cancelRoutine = pDeviceExtension->pReadIrpCurrent->CancelRoutine;
					pDeviceExtension->pReadIrpCurrent->Cancel = TRUE;

					if (cancelRoutine)
					{
						pDeviceExtension->pReadIrpCurrent->CancelRoutine = NULL;
						pDeviceExtension->pReadIrpCurrent->CancelIrql = cancelIrql;
						cancelRoutine(DeviceObject,  pDeviceExtension->pReadIrpCurrent);
					}
					else
					{
						IoReleaseCancelSpinLock(cancelIrql);
					}				
				}
								
				pIrp->IoStatus.Information = sizeof(ULONG);				
			}

			if (mask & SERIAL_PURGE_TXABORT)
			{
				KdPrint(("purge SERIAL_PURGE_TXABORT\n"));

				if (pDeviceExtension->pWriteIrpCurrent != NULL)
				{
					
					IoAcquireCancelSpinLock(&cancelIrql);

					cancelRoutine = pDeviceExtension->pWriteIrpCurrent->CancelRoutine;
					pDeviceExtension->pWriteIrpCurrent->Cancel = TRUE;

					if (cancelRoutine)
					{
						pDeviceExtension->pWriteIrpCurrent->CancelRoutine = NULL;
						pDeviceExtension->pWriteIrpCurrent->CancelIrql = cancelIrql;
						cancelRoutine(DeviceObject,  pDeviceExtension->pWriteIrpCurrent);
					}
					else
					{
						IoReleaseCancelSpinLock(cancelIrql);
					}
				}
				
				pIrp->IoStatus.Information = sizeof(ULONG);				
			}

			if (mask & SERIAL_PURGE_RXCLEAR)
			{
				KdPrint(("purge SERIAL_PURGE_RXCLEAR\n"));
				KeAcquireSpinLock(&pDeviceExtension->readSpinLock, &read_oldIrql);
				pDeviceExtension->outBufferReadPosition = 0;
				pDeviceExtension->outBufferWritePosition = 0;
				pDeviceExtension->outBufferCount = 0;
				pDeviceExtension->readBufferCount = 0;
				RtlZeroMemory(pDeviceExtension->outBuffer, BUFFERLENGTH);
				pDeviceExtension->eventHistory = 0;
				KeReleaseSpinLock(&pDeviceExtension->readSpinLock, read_oldIrql);
				
				pIrp->IoStatus.Information = sizeof(ULONG);
				
			}

			if (mask & SERIAL_PURGE_TXCLEAR)
			{
				KdPrint(("purge SERIAL_PURGE_TXCLEAR\n"));
				KeAcquireSpinLock(&pDeviceExtension->writeSpinLock, &write_oldIrql);
				pDeviceExtension->inBufferReadPosition = 0;
				pDeviceExtension->inBufferWritePosition = 0;
				pDeviceExtension->inBufferCount = 0;	
				RtlZeroMemory(pDeviceExtension->inBuffer, BUFFERLENGTH);
				pDeviceExtension->eventHistory = 0;
				KeReleaseSpinLock(&pDeviceExtension->writeSpinLock, write_oldIrql);
				
				pIrp->IoStatus.Information = sizeof(ULONG);
			}
			
			break;
		}
	        
		case IOCTL_SERIAL_GET_BAUD_RATE:
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_GET_BAUD_RATE\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(SERIAL_BAUD_RATE))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			pIrp->IoStatus.Information = sizeof(SERIAL_BAUD_RATE);

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			RtlMoveMemory(pIrp->AssociatedIrp.SystemBuffer, &pDeviceExtension->serBaudRate, sizeof(SERIAL_BAUD_RATE));

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);

			break;			
		      
		case IOCTL_SERIAL_SET_BAUD_RATE:
		{
			PSERIAL_BAUD_RATE pNewBaudRate = (PSERIAL_BAUD_RATE)pIrp->AssociatedIrp.SystemBuffer;

			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_SET_BAUD_RATE\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.InputBufferLength < sizeof(SERIAL_BAUD_RATE))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			RtlMoveMemory(&pDeviceExtension->serBaudRate, pNewBaudRate, sizeof(SERIAL_BAUD_RATE));

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);

			break;
		}
		    
		case IOCTL_SERIAL_GET_LINE_CONTROL:
		
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_GET_LINE_CONTROL\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(SERIAL_LINE_CONTROL))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			pIrp->IoStatus.Information = sizeof(SERIAL_LINE_CONTROL);

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			RtlMoveMemory(pIrp->AssociatedIrp.SystemBuffer, &pDeviceExtension->serLineControl, sizeof(SERIAL_LINE_CONTROL));

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);

			break;		
		      
		case IOCTL_SERIAL_SET_LINE_CONTROL:
		{
			PSERIAL_LINE_CONTROL pNewLineControl = (PSERIAL_LINE_CONTROL)pIrp->AssociatedIrp.SystemBuffer;

			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_SET_LINE_CONTROL\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.InputBufferLength < sizeof(SERIAL_LINE_CONTROL))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			RtlMoveMemory(&pDeviceExtension->serLineControl, pNewLineControl, sizeof(SERIAL_LINE_CONTROL));

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);

			break;
		}
		    		
		case IOCTL_SERIAL_GET_CHARS:
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_GET_CHARS\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(SERIAL_CHARS))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			pIrp->IoStatus.Information = sizeof(SERIAL_CHARS);

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			RtlMoveMemory(pIrp->AssociatedIrp.SystemBuffer, &pDeviceExtension->serChars, sizeof(SERIAL_CHARS));

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);

			break;
	      
		case IOCTL_SERIAL_SET_CHARS:
		{
			PSERIAL_CHARS pNewChars = (PSERIAL_CHARS)pIrp->AssociatedIrp.SystemBuffer;

			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_SET_CHARS\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.InputBufferLength < sizeof(SERIAL_CHARS))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			RtlMoveMemory(&pDeviceExtension->serChars, pNewChars, sizeof(SERIAL_CHARS));

			pDeviceExtension->serEventCharacter = pDeviceExtension->serChars.EventChar;
			
			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);

			break;
		}
			
		case IOCTL_SERIAL_GET_HANDFLOW:
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_GET_HANDFLOW\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(SERIAL_HANDFLOW))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			pIrp->IoStatus.Information = sizeof(SERIAL_HANDFLOW);

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			RtlMoveMemory(pIrp->AssociatedIrp.SystemBuffer, &pDeviceExtension->serHandFlow, sizeof(SERIAL_HANDFLOW));

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);	        	     

			break;
			
		case IOCTL_SERIAL_SET_HANDFLOW:
		{
			PSERIAL_HANDFLOW pNewHandFlow = (PSERIAL_HANDFLOW)pIrp->AssociatedIrp.SystemBuffer;

			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_SET_HANDFLOW\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.InputBufferLength < sizeof(SERIAL_HANDFLOW))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			RtlMoveMemory(&pDeviceExtension->serHandFlow, pNewHandFlow, sizeof(SERIAL_HANDFLOW));

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);

			break;
		}
		    	  
		case IOCTL_SERIAL_GET_MODEMSTATUS:
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_GET_MODEMSTATUS\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(ULONG))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);
			
			pDeviceExtension->serModemStatusRegister = 0;
			
			if (pDeviceExtension->serCTS)
				pDeviceExtension->serModemStatusRegister |= SERIAL_MSR_CTS;
			
			if (pDeviceExtension->serDSR)
				pDeviceExtension->serModemStatusRegister |= SERIAL_MSR_DSR;
						
			pIrp->IoStatus.Information = sizeof(ULONG);

			*(PULONG)pIrp->AssociatedIrp.SystemBuffer = (ULONG) pDeviceExtension->serModemStatusRegister;

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);
			
			break;
	      
		case IOCTL_SERIAL_GET_COMMSTATUS:
		{
			PSERIAL_STATUS pStatus = (PSERIAL_STATUS)pIrp->AssociatedIrp.SystemBuffer;

			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_GET_COMMSTATUS\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(SERIAL_STATUS))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			KeAcquireSpinLock(&pDeviceExtension->readSpinLock, &read_oldIrql);
			
			pDeviceExtension->serStatus.Errors = 0;
			pDeviceExtension->serStatus.HoldReasons = 0;
			pDeviceExtension->serStatus.AmountInInQueue = pDeviceExtension->outBufferCount;
			pDeviceExtension->serStatus.AmountInOutQueue = pDeviceExtension->inBufferCount;
			pDeviceExtension->serStatus.EofReceived = 0;
			pDeviceExtension->serStatus.WaitForImmediate = 0;		
									
			RtlMoveMemory(pStatus, &pDeviceExtension->serStatus, sizeof(SERIAL_STATUS));

			KeReleaseSpinLock(&pDeviceExtension->readSpinLock, read_oldIrql);	        	  

			pIrp->IoStatus.Information = sizeof(SERIAL_STATUS);

			KdPrint(("IOCTL_SERIAL_GET_COMMSTATUS: InQue=%d\n", pDeviceExtension->outBufferCount));
			KdPrint(("IOCTL_SERIAL_GET_COMMSTATUS: OutQue=%d\n", pDeviceExtension->inBufferCount));
			
			break;
		}
	    
		case IOCTL_SERIAL_GET_PROPERTIES:
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_GET_PROPERTIES\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(SERIAL_COMMPROP))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			pIrp->IoStatus.Information = sizeof(SERIAL_COMMPROP);

			RtlMoveMemory(pIrp->AssociatedIrp.SystemBuffer, &vCOMProperties, sizeof(SERIAL_COMMPROP));

			break;
	      
		case IOCTL_SERIAL_CONFIG_SIZE:
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_CONFIG_SIZE\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(ULONG))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			pIrp->IoStatus.Information = sizeof(ULONG);

			*(PULONG)pIrp->AssociatedIrp.SystemBuffer = 0;

			break;
	           	        
		case IOCTL_SERIAL_LSRMST_INSERT:
				
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_LSRMST_INSERT\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.InputBufferLength < sizeof(UCHAR))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			break;			
	        
		case IOCTL_SERIAL_SET_DTR:
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_SET_DTR\n"));
			pDeviceExtension->serDTR = TRUE;			
			if (!pTwinDeviceExtension->serDSR)
			{
				pTwinDeviceExtension->serDSR = TRUE;
				vCOMCheckComEvents(pTwinDeviceExtension, SERIAL_EV_DSR);
			}
			pIrp->IoStatus.Information = 0;
			break;

		case IOCTL_SERIAL_CLR_DTR:
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_CLR_DTR\n"));
			pDeviceExtension->serDTR = FALSE;
			if (pTwinDeviceExtension->serDSR)
			{
				pTwinDeviceExtension->serDSR = FALSE;
				vCOMCheckComEvents(pTwinDeviceExtension, SERIAL_EV_DSR);
			}			
			pIrp->IoStatus.Information = 0;			
			break;
			
		case IOCTL_SERIAL_SET_RTS:
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_SET_RTS\n"));
			pDeviceExtension->serRTS = TRUE;
			if (!pTwinDeviceExtension->serCTS)
			{
				pTwinDeviceExtension->serCTS = TRUE;			
				vCOMCheckComEvents(pTwinDeviceExtension, SERIAL_EV_CTS);
			}
			pIrp->IoStatus.Information = 0;
			break;
			
		case IOCTL_SERIAL_CLR_RTS:
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_CLR_RTS\n"));
			pDeviceExtension->serRTS = FALSE;
			if (pTwinDeviceExtension->serCTS)
			{
				pTwinDeviceExtension->serCTS = FALSE;			
				vCOMCheckComEvents(pTwinDeviceExtension, SERIAL_EV_CTS);
			}
			pIrp->IoStatus.Information = 0;
			break;
	      
		case IOCTL_SERIAL_GET_STATS:
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_GET_STATS\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(SERIALPERF_STATS ))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			pIrp->IoStatus.Information = sizeof(SERIALPERF_STATS );

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			RtlMoveMemory(pIrp->AssociatedIrp.SystemBuffer, &pDeviceExtension->serPerformanceStats, sizeof(SERIALPERF_STATS ));

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);	        	     

			break;

		case IOCTL_SERIAL_CLEAR_STATS:
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_CLEAR_STATS\n"));

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			RtlZeroMemory(&pDeviceExtension->serPerformanceStats, sizeof(SERIALPERF_STATS));

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);	

			break;
	    
		case IOCTL_SERIAL_SET_QUEUE_SIZE:        
		{
			PSERIAL_QUEUE_SIZE pNewQueueSize = (PSERIAL_QUEUE_SIZE)pIrp->AssociatedIrp.SystemBuffer;

			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_SET_QUEUE_SIZE\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.InputBufferLength < sizeof(SERIAL_QUEUE_SIZE))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			RtlMoveMemory(&pDeviceExtension->serQueueSize, pNewQueueSize, sizeof(SERIAL_QUEUE_SIZE));

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);

			KeAcquireSpinLock(&pDeviceExtension->readSpinLock, &oldIrql);

			if (pDeviceExtension->serQueueSize.InSize <= BUFFERLENGTH)
			{			
				pDeviceExtension->outBufferReadPosition = 0;
				pDeviceExtension->outBufferWritePosition = 0;
				pDeviceExtension->outBufferCount = 0;
				RtlZeroMemory(pDeviceExtension->outBuffer, BUFFERLENGTH);
				pDeviceExtension->outBufferSize = pDeviceExtension->serQueueSize.InSize;
			}

			KeReleaseSpinLock(&pDeviceExtension->readSpinLock, oldIrql);

			KeAcquireSpinLock(&pDeviceExtension->writeSpinLock, &oldIrql);

			if (pDeviceExtension->serQueueSize.OutSize <= BUFFERLENGTH)
			{				
				pDeviceExtension->inBufferReadPosition = 0;
				pDeviceExtension->inBufferWritePosition = 0;
				pDeviceExtension->inBufferCount = 0;
				RtlZeroMemory(pDeviceExtension->inBuffer, BUFFERLENGTH);
				pDeviceExtension->inBufferSize = pDeviceExtension->serQueueSize.OutSize;
			}

			KeReleaseSpinLock(&pDeviceExtension->writeSpinLock, oldIrql);

			break;
		}

		case IOCTL_SERIAL_GET_DTRRTS:
		{
			ULONG		serState = 0;
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_GET_DTRRTS\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(ULONG))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			pIrp->IoStatus.Information = sizeof(ULONG );

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);
						
			if (pDeviceExtension->serRTS)
				serState |= SERIAL_RTS_STATE;
			if (pDeviceExtension->serDTR)
				serState |= SERIAL_DTR_STATE;
			
			*(PULONG)pIrp->AssociatedIrp.SystemBuffer = serState;
			
			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);	        	     

			break;
		}

	   	case IOCTL_SERIAL_GET_MODEM_CONTROL:
		{
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_GET_MODEM_CONTROL\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.OutputBufferLength < sizeof(ULONG))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			pIrp->IoStatus.Information = sizeof(ULONG );

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);
			
			*(PULONG)pIrp->AssociatedIrp.SystemBuffer = pDeviceExtension->serModemControlRegister;

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);	        	     

			break;
		}

		case IOCTL_SERIAL_SET_MODEM_CONTROL:
		{
			ULONG	mcr;
			
			KdPrint(("vCOMDeviceControl: IOCTL_SERIAL_SET_MODEM_CONTROL\n"));

			if (pIrpStackLocation->Parameters.DeviceIoControl.InputBufferLength < sizeof(ULONG))
			{
				status = STATUS_BUFFER_TOO_SMALL;
				break;
			}

			mcr = *(PULONG)pIrp->AssociatedIrp.SystemBuffer;

			KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

			pDeviceExtension->serModemControlRegister = mcr;

			KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);
			
			if ((mcr & SERIAL_MCR_DTR) == SERIAL_MCR_DTR)
				{
					pDeviceExtension->serDTR = TRUE;
					if (!pTwinDeviceExtension->serDSR)
					{
						pTwinDeviceExtension->serDSR = TRUE;
						vCOMCheckComEvents(pTwinDeviceExtension, SERIAL_EV_DSR);
					}					
				}
			else
				{
					pDeviceExtension->serDTR = FALSE;
					if (pTwinDeviceExtension->serDSR)
					{
						pTwinDeviceExtension->serDSR = FALSE;
						vCOMCheckComEvents(pTwinDeviceExtension, SERIAL_EV_DSR);
					}		
				}
			
			if ((mcr & SERIAL_MCR_RTS) == SERIAL_MCR_RTS)
				{
					pDeviceExtension->serRTS = TRUE;
					if (!pTwinDeviceExtension->serCTS)
					{
						pTwinDeviceExtension->serCTS= TRUE;
						vCOMCheckComEvents(pTwinDeviceExtension, SERIAL_EV_CTS);
					}		
				}
			else
				{
					pDeviceExtension->serRTS = FALSE;
					if (pTwinDeviceExtension->serCTS)
					{
						pTwinDeviceExtension->serCTS= FALSE;
						vCOMCheckComEvents(pTwinDeviceExtension, SERIAL_EV_CTS);
					}	
				}

			break;
		}

		case IOCTL_SERIAL_XOFF_COUNTER:
		case IOCTL_SERIAL_SET_XON:
		case IOCTL_SERIAL_SET_XOFF:
		case IOCTL_SERIAL_SET_FIFO_CONTROL:
		case IOCTL_SERIAL_SET_BREAK_ON:
		case IOCTL_SERIAL_SET_BREAK_OFF:		
		case IOCTL_SERIAL_RESET_DEVICE:              
			KdPrint(("vCOMDeviceControl: Other IOControl\n"));
			break;

		default:        
			status = STATUS_INVALID_PARAMETER;
			break;  
			
	}

	if (status != STATUS_PENDING)
	{		
		vCOMCompleteRequest(pIrp, status, pIrp->IoStatus.Information);
	}
    	
	return status;
}

NTSTATUS vCOMCleanupDevice(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION  	pDeviceExtension;
	LIST_ENTRY             			tempQueue;   
	PLIST_ENTRY            			thisEntry;
	KIRQL						OldIrql;
	PIRP                   				pendingIrp;
	PIO_STACK_LOCATION    		pendingIrpStack;
	PIO_STACK_LOCATION			irpStack;

	pDeviceExtension = DeviceObject->DeviceExtension;

	irpStack = IoGetCurrentIrpStackLocation(pIrp);

	while (pendingIrp = IoCsqRemoveNextIrp (&pDeviceExtension->cancelSafeReadQueue, irpStack->FileObject))
	{
		// Cancel the IRP	
		vCOMCompleteRequest(pendingIrp, STATUS_CANCELLED, 0);
	}

	while (pendingIrp = IoCsqRemoveNextIrp (&pDeviceExtension->cancelSafeWriteQueue, irpStack->FileObject))
	{
		// Cancel the IRP	
		vCOMCompleteRequest(pendingIrp, STATUS_CANCELLED, 0);
	}
	
	// Finally complete the cleanup IRP	
	
	vCOMCompleteRequest(pIrp, STATUS_SUCCESS, 0);
	
	return STATUS_SUCCESS;
	
}

NTSTATUS vCOMGetRegistryKeyValue(HANDLE hRegistry, PCWSTR keyName, PVOID data, ULONG dataLength)
{
	NTSTATUS                        			status;
	UNICODE_STRING                  		keyNameString;
	ULONG                           			length;
	PKEY_VALUE_PARTIAL_INFORMATION 	pInfo;

	status = STATUS_INSUFFICIENT_RESOURCES;

	RtlInitUnicodeString(&keyNameString, keyName);

	length = sizeof(KEY_VALUE_PARTIAL_INFORMATION) + dataLength;
	pInfo  = ExAllocatePool(PagedPool, length); 

	if (pInfo != NULL)
	{
		status = ZwQueryValueKey(hRegistry, &keyNameString, KeyValuePartialInformation,
		pInfo, length, &length);

		if (NT_SUCCESS(status))
		{
			if (dataLength >= pInfo->DataLength)
				RtlCopyMemory(data, &pInfo->Data, pInfo->DataLength);
			else
				status = STATUS_BUFFER_TOO_SMALL;
		}

		ExFreePool(pInfo);
	}

	return status;
	
}

VOID vCOMCancelWaitIrp(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION  	pDeviceExtension;
	KIRQL                   			oldIrql;

	pDeviceExtension = DeviceObject->DeviceExtension;

	IoReleaseCancelSpinLock(pIrp->CancelIrql);

	KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

	pDeviceExtension-> pWaitIrp = NULL;        

	KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);    
	
	vCOMCompleteRequest(pIrp, STATUS_CANCELLED, 0);
}

VOID vCOMCancelCurrentReadIrp(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION  	pDeviceExtension;
	KIRQL                   			oldIrql;

	pDeviceExtension = DeviceObject->DeviceExtension;

	IoReleaseCancelSpinLock(pIrp->CancelIrql);

	KeAcquireSpinLock(&pDeviceExtension->readSpinLock, &oldIrql);

	pDeviceExtension-> pReadIrpCurrent = NULL;        

	KeReleaseSpinLock(&pDeviceExtension->readSpinLock, oldIrql);    

	vCOMCompleteRequest(pIrp, STATUS_CANCELLED, 0);
	KeSetEvent(&pDeviceExtension->currentReadIrpWasCancelledEvent, 0, FALSE);
}

VOID vCOMCancelCurrentWriteIrp(PDEVICE_OBJECT DeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION  	pDeviceExtension;
	KIRQL                   			oldIrql;

	pDeviceExtension = DeviceObject->DeviceExtension;

	IoReleaseCancelSpinLock(pIrp->CancelIrql);

	KeAcquireSpinLock(&pDeviceExtension->writeSpinLock, &oldIrql);

	pDeviceExtension-> pWriteIrpCurrent= NULL;        

	KeReleaseSpinLock(&pDeviceExtension->writeSpinLock, oldIrql);    

	vCOMCompleteRequest(pIrp, STATUS_CANCELLED, 0);
	KeSetEvent(&pDeviceExtension->currentWriteIrpWasCancelledEvent, 0, FALSE);
}

ULONG vCOMCalculateReadTimeoutValues(PDEVICE_OBJECT pDeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension;
    	PIO_STACK_LOCATION 		pIrpStackLocation;
    	ULONG 						dataLength;
    	ULONG 						readIntervalTimeout;
  	ULONG 						readTotalTimeoutMultiplier;
  	ULONG 						readTotalTimeoutConstant;
	ULONG						readCombinedTimeout;
	
	pDeviceExtension = pDeviceObject->DeviceExtension;
	pIrpStackLocation = IoGetCurrentIrpStackLocation(pIrp); 
		
	dataLength = pIrpStackLocation->Parameters.Read.Length;
		
	readIntervalTimeout 		= pDeviceExtension->serTimeouts.ReadIntervalTimeout;
	readTotalTimeoutMultiplier 	= pDeviceExtension->serTimeouts.ReadTotalTimeoutMultiplier;
	readTotalTimeoutConstant 	= pDeviceExtension->serTimeouts.ReadTotalTimeoutConstant;

	KdPrint(("readIntervalTimeout is %lu\n", readIntervalTimeout));
	KdPrint(("readTotalTimeoutMultiplier is %lu\n", readTotalTimeoutMultiplier));
	KdPrint(("readTotalTimeoutConstant is %lu\n", readTotalTimeoutConstant));
	KdPrint(("MAXULONG is %lu\n", MAXULONG));

	/* NOTE:
	The timeout values for serTimeouts are in milliseconds.
	The kernel timers want the timeouts to be specified in multiples of 100 nS intervals.
	100 nS = 0.000000100 * 10000 = 0.001 or 1 mS
	So we need to multiply the values in mS by 10000 to get the proper time timeout values.
	10,000 1nS intervals is 1 mS.
	*/
	
	if (readIntervalTimeout == MAXULONG && readTotalTimeoutMultiplier == 0 && readTotalTimeoutConstant == 0)
	{
		KdPrint(("return immediately with whatever bytes there are or none...\n"));
		//return immediately with whatever bytes there are
		pDeviceExtension->readTotalTimeoutValue.QuadPart = 0;
		pDeviceExtension->readIntervalTimeoutValue.QuadPart = 0;
		return TIMEOUTS_IMMEDIATE_RETURN_WITH_BYTES;
	}
	else if (readIntervalTimeout == 0 && readTotalTimeoutMultiplier == 0 && readTotalTimeoutConstant == 0)
	{
		KdPrint(("wait forever...\n"));
		//total timeout is not used
		pDeviceExtension->readTotalTimeoutValue.QuadPart = 0;
		pDeviceExtension->readIntervalTimeoutValue.QuadPart = 0;	
		return TIMEOUTS_WAIT_FOREVER;
	}
	else if (readIntervalTimeout > 0 && readTotalTimeoutMultiplier == 0 && readTotalTimeoutConstant == 0)
	{
		KdPrint(("interval timeout...\n"));
		pDeviceExtension->readTotalTimeoutValue.QuadPart = 0;
		pDeviceExtension->readIntervalTimeoutValue.QuadPart = ((LONGLONG)readIntervalTimeout) * -10000;
		return TIMEOUTS_INTERVAL_TIMEOUT;
	}
	else if (readIntervalTimeout == MAXULONG && readTotalTimeoutMultiplier == MAXULONG && readTotalTimeoutConstant > 0 && readTotalTimeoutConstant != MAXULONG)
	{
		KdPrint(("if any bytes available return immediately with them, if none wait or total timeout...\n"));
		//if any bytes avaliable return immediately with them
		//if no bytes available wait and then return with any
		//otherwise timeout when readTotalTimeoutConstant time expires
		pDeviceExtension->readTotalTimeoutValue.QuadPart = ((LONGLONG)readTotalTimeoutConstant) * -10000;
		pDeviceExtension->readIntervalTimeoutValue.QuadPart = 0;	
		return TIMEOUTS_RETURNBYTES_OR_WAIT;
	}
	else if (readIntervalTimeout == 0 && readTotalTimeoutMultiplier != 0 && readTotalTimeoutConstant != 0)
	{
		KdPrint(("Total timeout only...\n"));
		//use total timeouts
		readCombinedTimeout = readTotalTimeoutConstant + readTotalTimeoutMultiplier * dataLength;
		pDeviceExtension->readTotalTimeoutValue.QuadPart = ((LONGLONG)readCombinedTimeout) * -10000;
		pDeviceExtension->readIntervalTimeoutValue.QuadPart = 0;	
		return TIMEOUTS_TOTAL_TIMEOUT;
	}
	else if (readIntervalTimeout != 0 && readTotalTimeoutMultiplier != 0 && readTotalTimeoutConstant != 0)
	{
		KdPrint(("use combined timeouts...\n"));
		//use total timeouts and interval
		readCombinedTimeout = readTotalTimeoutConstant + readTotalTimeoutMultiplier * dataLength;
		pDeviceExtension->readTotalTimeoutValue.QuadPart = ((LONGLONG)readCombinedTimeout) * -10000;
		pDeviceExtension->readIntervalTimeoutValue.QuadPart = ((LONGLONG)readIntervalTimeout) * -10000;
		return TIMEOUTS_COMBINED_TIMEOUT;
	}
	else
	{	
		KdPrint(("default: use total timeout only...\n"));
		//use total timeouts
		readCombinedTimeout = readTotalTimeoutConstant + readTotalTimeoutMultiplier * dataLength;
		pDeviceExtension->readTotalTimeoutValue.QuadPart = ((LONGLONG)readCombinedTimeout) * -10000;
		pDeviceExtension->readIntervalTimeoutValue.QuadPart = 0;
		return TIMEOUTS_TOTAL_TIMEOUT;
	}
	
}

ULONG vCOMCalculateWriteTimeoutValues(PDEVICE_OBJECT pDeviceObject, PIRP pIrp)
{
	PvCOM_DEVICE_EXTENSION 	pDeviceExtension;
	PIO_STACK_LOCATION 		pIrpStackLocation;
	ULONG 						dataLength;
	ULONG 						writeTotalTimeoutMultiplier;
	ULONG 						writeTotalTimeoutConstant;
	ULONG						writeCombinedTimeout;
	
	pDeviceExtension = pDeviceObject->DeviceExtension;
	pIrpStackLocation = IoGetCurrentIrpStackLocation(pIrp); 
		
	dataLength = pIrpStackLocation->Parameters.Read.Length;
		
	writeTotalTimeoutMultiplier 	= pDeviceExtension->serTimeouts.WriteTotalTimeoutMultiplier;
	writeTotalTimeoutConstant 	= pDeviceExtension->serTimeouts.WriteTotalTimeoutConstant;

	/* NOTE:
	The timeout values for serTimeouts are in milliseconds.
	The kernel timers want the timeouts to be specified in multiples of 100 nS intervals.
	100 nS = 0.000000100 * 10000 = 0.001 or 1 mS
	So we need to multiply the values in mS by 10000 to get the proper time timeout values.
	10,000 1nS intervals is 1 mS.
	*/
	
	KdPrint(("writeTotalTimeoutMultiplier is %lu\n", writeTotalTimeoutMultiplier));
	KdPrint(("writeTotalTimeoutConstant is %lu\n", writeTotalTimeoutConstant));
	KdPrint(("MAXULONG is %lu\n", MAXULONG));
			
	 if (writeTotalTimeoutMultiplier != 0 || writeTotalTimeoutConstant != 0)
	{
		KdPrint(("use write timeout...\n"));
		//use total timeouts and interval
		writeCombinedTimeout = writeTotalTimeoutConstant + writeTotalTimeoutMultiplier * dataLength;
		pDeviceExtension->writeTotalTimeoutValue.QuadPart = ((LONGLONG)writeCombinedTimeout) * -10000;
		pDeviceExtension->writeDelayLoopTimerValue.QuadPart = (LONGLONG)(-10000 * 10); //10mS
		return TIMEOUTS_TOTAL_TIMEOUT;
	}
	else
	{	
		KdPrint(("default: no write timeout...\n"));
		//use total timeouts
		pDeviceExtension->writeTotalTimeoutValue.QuadPart = 0;
		pDeviceExtension->writeDelayLoopTimerValue.QuadPart = (LONGLONG)(-10000 * 100); //100mS
		return TIMEOUTS_WAIT_FOREVER;
	}
	
}

VOID vCOMCheckComEvents(PvCOM_DEVICE_EXTENSION pDeviceExtension, ULONG events)
{
	PIRP            			pOldWaitIrp = NULL;
	PDRIVER_CANCEL  	pOldCancelRoutine;
	KIRQL          			oldIrql;


	KeAcquireSpinLock(&pDeviceExtension->controlSpinLock, &oldIrql);

	pDeviceExtension->eventHistory |= events;
	
	events &= pDeviceExtension->eventMask;

	if ((pDeviceExtension->pWaitIrp != NULL) && (events != 0))
	{
		pOldWaitIrp = pDeviceExtension->pWaitIrp;
		pOldCancelRoutine = IoSetCancelRoutine(pOldWaitIrp, NULL);

		if (pOldCancelRoutine != NULL)
		{
			pOldWaitIrp->IoStatus.Information = sizeof(ULONG);
			*(PULONG)pOldWaitIrp->AssociatedIrp.SystemBuffer = events;
			pOldWaitIrp->IoStatus.Status = STATUS_SUCCESS;

			pDeviceExtension->pWaitIrp = NULL;
			pDeviceExtension->eventHistory= 0;
		}
		else
		{
			pOldWaitIrp = NULL;
		}
	}

	KeReleaseSpinLock(&pDeviceExtension->controlSpinLock, oldIrql);

	if (pOldWaitIrp != NULL)
	{
		IoCompleteRequest(pOldWaitIrp, IO_NO_INCREMENT);		
	}
	
}

NTSTATUS vCOMCompleteRequest(PIRP pIrp, NTSTATUS status, ULONG_PTR info)
{	
	if (pIrp)
	{
		pIrp->IoStatus.Status = status;
		pIrp->IoStatus.Information = info;
		IoCompleteRequest(pIrp, IO_NO_INCREMENT);
	}
	return status;
}

