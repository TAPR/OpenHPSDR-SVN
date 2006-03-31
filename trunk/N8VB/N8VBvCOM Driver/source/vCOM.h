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

#include <wdm.h>
#include <initguid.h>
#include <ntddser.h>
#include <csq.h>


#define MAX_PAIR_COUNT		10

// interface GUIDs
static GUID vCOMGUID	= {0xB03C1DEB, 0xD080, 0x4E83, 0xBF, 0xDF, 0x90, 0x15, 0x95, 0x67, 0xBF, 0xE9};
							

static PCWSTR vCOMDevNames[20] = 	{
									L"\\Device\\vComDrv0", 
									L"\\Device\\vComDrv1", 
									L"\\Device\\vComDrv2",
									L"\\Device\\vComDrv3", 
									L"\\Device\\vComDrv4", 
									L"\\Device\\vComDrv5",
									L"\\Device\\vComDrv6", 
									L"\\Device\\vComDrv7", 
									L"\\Device\\vComDrv8",
									L"\\Device\\vComDrv9",
									L"\\Device\\vComDrv10", 
									L"\\Device\\vComDrv11", 
									L"\\Device\\vComDrv12",
									L"\\Device\\vComDrv13", 
									L"\\Device\\vComDrv14", 
									L"\\Device\\vComDrv15",
									L"\\Device\\vComDrv16", 
									L"\\Device\\vComDrv17", 
									L"\\Device\\vComDrv18",
									L"\\Device\\vComDrv19"
									};

static PCWSTR vCOMPortNumbers[20] = {
									L"PortNumber1", 
									L"PortNumber2", 
									L"PortNumber3", 
									L"PortNumber4",
									L"PortNumber5", 
									L"PortNumber6", 
									L"PortNumber7", 
									L"PortNumber8",
									L"PortNumber9", 
									L"PortNumber10",
									L"PortNumber11", 
									L"PortNumber12", 
									L"PortNumber13", 
									L"PortNumber14",
									L"PortNumber15", 
									L"PortNumber16", 
									L"PortNumber17", 
									L"PortNumber18",
									L"PortNumber19", 
									L"PortNumber20"
									};

static ULONG devpaircount = 1;

static PDEVICE_OBJECT pDevices[20] =	{
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL,
									NULL
									};

#define SERIAL_DEVICE_MAP           	L"SERIALCOMM"

#define BUFFERLENGTH									65536

#define TIMEOUTS_WAIT_FOREVER 						0
#define TIMEOUTS_IMMEDIATE_RETURN_WITH_BYTES  		1
#define TIMEOUTS_RETURNBYTES_OR_WAIT 				2
#define TIMEOUTS_INTERVAL_TIMEOUT 					3
#define TIMEOUTS_TOTAL_TIMEOUT 						4
#define TIMEOUTS_COMBINED_TIMEOUT 					5

#define SERIAL_MCR_DTR            							0x01
#define SERIAL_MCR_RTS            							0x02
#define SERIAL_MSR_CTS      								0x10
#define SERIAL_MSR_DSR      								0x20

// device extension structure
typedef struct _vCOM_DEVICE_EXTENSION 
{
	BOOLEAN                 		isSet;    
	BOOLEAN                 		isOpened;
	BOOLEAN                 		isAttached;
	BOOLEAN                 		isRemoved;
	BOOLEAN					symbolicLinkIsDone;
	BOOLEAN					deviceMapEntryIsCreated;
	BOOLEAN					terminateTransferThread;
	BOOLEAN                 		terminateReadThread;
	BOOLEAN					terminateWriteThread;
	BOOLEAN					serDTR;
	BOOLEAN					serRTS;
	BOOLEAN					serDSR;
	BOOLEAN					serCTS;
	BOOLEAN					fastRead;
	    
	PIRP                    			pReadIrpCurrent;
	PIRP                   			pWriteIrpCurrent;
	PIRP                    			pWaitIrp;
	
	PDEVICE_OBJECT     		pMe;
	PDEVICE_OBJECT			pTwin;
	PDEVICE_OBJECT     		pNextDriver;
		    
	UNICODE_STRING    		symbolicLinkName;
	UNICODE_STRING    		interfaceName;

	LIST_ENTRY              		readPendingQueue;
	LIST_ENTRY              		writePendingQueue;

	IO_CSQ 					cancelSafeReadQueue;
	IO_CSQ					cancelSafeWriteQueue;
		
	ULONG                   		eventMask;
	ULONG					eventHistory;
	ULONG					deviceNumber;
	ULONG 					outBufferReadPosition;
	ULONG					outBufferWritePosition;
	ULONG					outBufferCount;	
	ULONG					inBufferReadPosition;
	ULONG					inBufferWritePosition;
	ULONG					inBufferCount;	
	ULONG					readTimeoutType;
	ULONG					writeTimeoutType;
	ULONG					readBufferCount;
	ULONG					inBufferSize;
	ULONG					outBufferSize;	
	
	PVOID                   		pReadThread;
	PVOID					pWriteThread;
	PVOID                   		pTransferThread;
	
	KSPIN_LOCK            		readSpinLock;
	KSPIN_LOCK            		writeSpinLock;
	KSPIN_LOCK            		controlSpinLock;
	KSPIN_LOCK				readQueueSpinLock;
	KSPIN_LOCK				writeQueueSpinLock;
	
	KSEMAPHORE				readIRPQueueSemaphore;
	KSEMAPHORE				writeIRPQueueSemaphore;
		
	FAST_MUTEX            		openMutex;

	KTIMER					readTotalTimer;
	KTIMER					readIntervalTimer;
	KTIMER					writeTotalTimer;

	KEVENT					transferNeededEvent;
	KEVENT					transferOccurredEvent;	
	KEVENT					readOccurredEvent;
	KEVENT					currentReadIrpWasCancelledEvent;
	KEVENT					currentWriteIrpWasCancelledEvent;
	
	LARGE_INTEGER			readTotalTimeoutValue;
	LARGE_INTEGER			readIntervalTimeoutValue;
	LARGE_INTEGER			writeTotalTimeoutValue;
	LARGE_INTEGER			writeDelayLoopTimerValue;
	LARGE_INTEGER			transferWaitTimeoutValue;
	LARGE_INTEGER			readWaitTimeoutValue;
		
	SERIAL_TIMEOUTS        	serTimeouts;
	SERIAL_LINE_CONTROL		serLineControl;
	SERIAL_BAUD_RATE		serBaudRate;
	SERIAL_HANDFLOW		serHandFlow;
	SERIAL_CHARS			serChars;
	SERIALPERF_STATS        	serPerformanceStats;
	SERIAL_QUEUE_SIZE 		serQueueSize;
	SERIAL_STATUS			serStatus;
	ULONG					serModemControlRegister;
	ULONG					serModemStatusRegister;
	UCHAR					serEventCharacter;
	
	PUCHAR 					inBuffer;
	PUCHAR                    		outBuffer;	
        
} vCOM_DEVICE_EXTENSION, *PvCOM_DEVICE_EXTENSION;
                                         
// Function Prototypes

// vCOM_init.c

NTSTATUS DriverEntry(PDRIVER_OBJECT  pDriverObject, PUNICODE_STRING pRegistryPath);
VOID vCOMUnload(IN PDRIVER_OBJECT pDriverObject);

// vCOM_device.c

NTSTATUS vCOMAddDevice(PDRIVER_OBJECT DriverObject, PDEVICE_OBJECT pPDO);
NTSTATUS vCOMCreateFDO(PDRIVER_OBJECT DriverObject, PDEVICE_OBJECT pPDO, PDEVICE_OBJECT* pvCOMFDO, ULONG devNumber);
NTSTATUS vCOMGetRegistryKeyValue(HANDLE hRegistry, PCWSTR keyName, PVOID data, ULONG dataLength);
NTSTATUS vCOMCreateDevice(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMCloseDevice(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMReadDevice(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMFastRead(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMWriteDevice(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMFastWrite(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMFastTransfer(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMDeviceControl(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
VOID vCOMDeleteFailedDevice(PDEVICE_OBJECT pFDO, ULONG devNumber);
VOID vCOMDeleteDevice(PDEVICE_OBJECT);
NTSTATUS vCOMCleanupDevice(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
VOID vCOMCancelWaitIrp(PDEVICE_OBJECT DeviceObject,  PIRP pIrp);
VOID vCOMCancelCurrentReadIrp(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
VOID vCOMCancelCurrentWriteIrp(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
ULONG vCOMCalculateReadTimeoutValues(PDEVICE_OBJECT pDeviceObject, PIRP pIrp);
ULONG vCOMCalculateWriteTimeoutValues(PDEVICE_OBJECT pDeviceObject, PIRP pIrp);
VOID vCOMCheckComEvents(PvCOM_DEVICE_EXTENSION pDeviceExtension, ULONG events);

// vCOM_csq.c

VOID vCOMInsertReadIrp (PIO_CSQ   pCsq,  PIRP pIrp);
VOID vCOMRemoveReadIrp(PIO_CSQ pCsq,  PIRP pIrp);
PIRP vCOMPeekNextReadIrp(PIO_CSQ pCsq, PIRP pIrp,  PVOID pPeekContext);
VOID vCOMAcquireReadLock(PIO_CSQ pCsq, PKIRQL pIrql);
VOID vCOMReleaseReadLock(PIO_CSQ pCsq, KIRQL pIrql);
VOID vCOMCompleteCanceledReadIrp(PIO_CSQ pCsq, PIRP pIrp);
VOID vCOMInsertWriteIrp (PIO_CSQ   pCsq,  PIRP pIrp);
VOID vCOMRemoveWriteIrp(PIO_CSQ pCsq,  PIRP pIrp);
PIRP vCOMPeekNextWriteIrp(PIO_CSQ pCsq, PIRP pIrp,  PVOID pPeekContext);
VOID vCOMAcquireWriteLock(PIO_CSQ pCsq, PKIRQL pIrql);
VOID vCOMReleaseWriteLock(PIO_CSQ pCsq, KIRQL pIrql);
VOID vCOMCompleteCanceledWriteIrp(PIO_CSQ pCsq, PIRP pIrp);
NTSTATUS vCOMCompleteRequest(PIRP pIrp, NTSTATUS status, ULONG_PTR info);
	
// vCOM_dispatch.c

NTSTATUS vCOMDispatchCreate(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMDispatchClose(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMDispatchCleanup(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMDispatchPower(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMDispatchQueryInformation(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMDispatchSetInformation(PDEVICE_OBJECT DeviceObject, PIRP pIrp);
NTSTATUS vCOMDispatchSystemControl(PDEVICE_OBJECT DeviceObject, PIRP pIrp);

// vCOM_pnp.c

NTSTATUS vCOMDispatchPnP(PDEVICE_OBJECT, PIRP);
NTSTATUS vCOMCompletionRoutine(PDEVICE_OBJECT, PIRP, PKEVENT);

// vCOM_readthread.c

VOID vCOMReadThread(PVOID pContext);
NTSTATUS vCOMReadThreadStart(PDEVICE_OBJECT pDeviceObject);
VOID vCOMReadThreadStop(PDEVICE_OBJECT pDeviceObject);
VOID vCOMReadThreadCompleteRequest(PDEVICE_OBJECT pDeviceObject, PIRP pIrp, NTSTATUS status);
	
//vCOM_writethread.c

VOID vCOMWriteThread(PVOID pContext);
NTSTATUS vCOMWriteThreadStart(PDEVICE_OBJECT pDeviceObject);
VOID vCOMWriteThreadStop(PDEVICE_OBJECT pDeviceObject);
VOID vCOMWriteThreadCompleteRequest(PDEVICE_OBJECT pDeviceObject, PIRP pIrp, NTSTATUS status);

//vCOM_transferthread.c

VOID vCOMTransferThread(PVOID pContext);
NTSTATUS vCOMTransferThreadStart(PDEVICE_OBJECT pDeviceObject);
VOID vCOMTransferThreadStop(PDEVICE_OBJECT pDeviceObject);

