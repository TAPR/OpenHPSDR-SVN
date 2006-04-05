/******************************************************************************/
/*                                                                            */
/*                    PortTalk Driver for Windows NT/2000/XP                  */
/*                        Version 2.0, 12th January 2002                      */
/*                          http://www.beyondlogic.org                        */
/*                                                                            */
/* Copyright © 2002 Craig Peacock. Craig.Peacock@beyondlogic.org              */
/* Any publication or distribution of this code in source form is prohibited  */
/* without prior written permission of the copyright holder. This source code */
/* is provided "as is", without any guarantee made as to its suitability or   */
/* fitness for any particular use. Permission is herby granted to modify or   */
/* enhance this sample code to produce a derivative program which may only be */
/* distributed in compiled object form only.                                  */
/******************************************************************************/

#include <ntddk.h>
#include <porttalk_IOCTL.h>

#define    IOPM_SIZE    0x2000

typedef UCHAR IOPM[IOPM_SIZE];

IOPM *IOPM_local = 0;

void Ke386SetIoAccessMap(int, IOPM *);
void Ke386QueryIoAccessMap(int, IOPM *);
void Ke386IoSetAccessProcess(PEPROCESS, int);

NTSTATUS PortTalkDeviceControl(IN PDEVICE_OBJECT DeviceObject, IN PIRP Irp);
NTSTATUS PsLookupProcessByProcessId(IN ULONG ulProcId,OUT struct _EPROCESS ** pEProcess);
VOID PortTalkUnload(IN PDRIVER_OBJECT DriverObject);

NTSTATUS PortTalkCreateDispatch(
    IN PDEVICE_OBJECT DeviceObject,
    IN PIRP Irp
    )
{
    Irp->IoStatus.Information = 0;
    Irp->IoStatus.Status = STATUS_SUCCESS;
    IoCompleteRequest(Irp, IO_NO_INCREMENT);
    return STATUS_SUCCESS;
}

NTSTATUS DriverEntry(
    IN PDRIVER_OBJECT DriverObject,
    IN PUNICODE_STRING RegistryPath
    )
{
    PDEVICE_OBJECT deviceObject;
    NTSTATUS status;
    WCHAR NameBuffer[] = L"\\Device\\PortTalk";
    WCHAR DOSNameBuffer[] = L"\\DosDevices\\PortTalk";
    UNICODE_STRING uniNameString, uniDOSString;

    KdPrint( ("PORTTALK: Porttalk V2.0 12/01/2002 has Loaded") );

    IOPM_local = MmAllocateNonCachedMemory(sizeof(IOPM));
    if(IOPM_local == 0) return STATUS_INSUFFICIENT_RESOURCES;

    RtlFillMemory(IOPM_local, sizeof(IOPM), 0xFF);

    KdPrint( ("PORTTALK: Memory Allocated at %X\n",IOPM_local) );

    RtlInitUnicodeString(&uniNameString, NameBuffer);
    RtlInitUnicodeString(&uniDOSString, DOSNameBuffer);

    status = IoCreateDevice(DriverObject, 
                            0,
                            &uniNameString,
                            FILE_DEVICE_UNKNOWN,
                            0, 
                            FALSE, 
                            &deviceObject);

    if(!NT_SUCCESS(status))
        return status;

    status = IoCreateSymbolicLink (&uniDOSString, &uniNameString);

    if (!NT_SUCCESS(status))
        return status;

    DriverObject->MajorFunction[IRP_MJ_CREATE] = PortTalkCreateDispatch;
    DriverObject->MajorFunction[IRP_MJ_DEVICE_CONTROL] = PortTalkDeviceControl;
    DriverObject->DriverUnload = PortTalkUnload;

    return STATUS_SUCCESS;
}

NTSTATUS
PortTalkDeviceControl(
    IN PDEVICE_OBJECT DeviceObject,
    IN PIRP pIrp
    )

{
    PIO_STACK_LOCATION  irpSp;
    NTSTATUS            ntStatus = STATUS_SUCCESS;   

    ULONG               inBufLength;   /* Input buffer length */
    ULONG               outBufLength;  /* Output buffer length */
    ULONG               inBuf;         /* Pointer to Input and output buffer */

    PUCHAR              CharBuffer; 
    PUSHORT             ShortBuffer;
    PULONG              LongBuffer;
    PVOID               ioBuffer;

    USHORT Offset;
    UCHAR Value;
 
    ULONG ProcessID;    
    struct _EPROCESS *Process;

    irpSp = IoGetCurrentIrpStackLocation( pIrp );
    inBufLength = irpSp->Parameters.DeviceIoControl.InputBufferLength;
    outBufLength = irpSp->Parameters.DeviceIoControl.OutputBufferLength;

    ioBuffer    = pIrp->AssociatedIrp.SystemBuffer;

    CharBuffer  = (PUCHAR) ioBuffer;
    ShortBuffer = (PUSHORT) ioBuffer;
    LongBuffer  = (PULONG) ioBuffer;

    switch ( irpSp->Parameters.DeviceIoControl.IoControlCode )
     {
      case IOCTL_IOPM_RESTRICT_ALL_ACCESS:

            KdPrint( ("PORTTALK: IOCTL_IOPM_RESTRICT_ALL_ACCESS - RTLFillMemory") );
            RtlFillMemory(IOPM_local, sizeof(IOPM), 0xFF);
            pIrp->IoStatus.Information = 0; /* Output Buffer Size */
            ntStatus = STATUS_SUCCESS;
            break;

      case IOCTL_IOPM_ALLOW_EXCUSIVE_ACCESS:

            KdPrint( ("PORTTALK: IOCTL_IOPM_ALLOW_EXCUSIVE_ACCESS - RTLZeroMemory") );
            RtlZeroMemory(IOPM_local, sizeof(IOPM));
            pIrp->IoStatus.Information = 0; /* Output Buffer Size */
            ntStatus = STATUS_SUCCESS;
            break;

      case IOCTL_SET_IOPM: 

            KdPrint( ("PORTTALK: IOCTL_SET_IOPM - Set IO Permission Bitmap") );
            if (inBufLength >= 3) {
                Offset = ShortBuffer[0];
                if (Offset >= 0x2000) {
                      ntStatus = STATUS_ARRAY_BOUNDS_EXCEEDED;
                      break;
                     }
                Value  = CharBuffer[2];
                KdPrint( ("PORTTALK: Offset = %X, Value = %X\n",Offset,Value) );
                *(*IOPM_local + Offset) = Value;
                ntStatus = STATUS_SUCCESS;
            } else ntStatus = STATUS_BUFFER_TOO_SMALL;
            pIrp->IoStatus.Information = 0; /* Output Buffer Size */
            ntStatus = STATUS_SUCCESS;
            break;

      case IOCTL_ENABLE_IOPM_ON_PROCESSID: 

            KdPrint( ("PORTTALK: IOCTL_ENABLE_IOPM_ON_PROCESSID") );
            if (inBufLength >= 4) {
                ProcessID = LongBuffer[0];
                KdPrint( ("PORTTALK: ProcessID Received is %d\n",ProcessID) );
                PsLookupProcessByProcessId(ProcessID, &Process);
                KdPrint( ("PORTTALK: Pointer to Process is %X\n",Process) );
                KdPrint( ("PORTTALK: Address = %X\n",*(*IOPM_local + 0x6F) ) );
                Ke386SetIoAccessMap(1, IOPM_local);
                Ke386IoSetAccessProcess(Process, 1);
                ntStatus = STATUS_SUCCESS;
            } else ntStatus = STATUS_BUFFER_TOO_SMALL;
            pIrp->IoStatus.Information = 0; /* Output Buffer Size */
            ntStatus = STATUS_SUCCESS;
            break;
        
      case IOCTL_READ_PORT_UCHAR:
 
            if ((inBufLength >= 2) && (outBufLength >= 1)) {
                KdPrint( ("PORTTALK: IOCTL_READ_PORT_UCHAR 0x%X",ShortBuffer[0]) );
                (UCHAR)Value = READ_PORT_UCHAR((PUCHAR)ShortBuffer[0]);
                KdPrint( ("PORTTALK: Value Read %X",Value) );
                CharBuffer[0] = Value;
            } else ntStatus = STATUS_BUFFER_TOO_SMALL;
            pIrp->IoStatus.Information = 1; /* Output Buffer Size */
            ntStatus = STATUS_SUCCESS;
            break;

      case IOCTL_WRITE_PORT_UCHAR:
            if (inBufLength >= 3) {
                KdPrint( ("PORTTALK: IOCTL_WRITE_PORT_UCHAR(0x%X,0x%X)",ShortBuffer[0], CharBuffer[2]) );
                WRITE_PORT_UCHAR((PUCHAR)ShortBuffer[0], CharBuffer[2]);
            } else ntStatus = STATUS_BUFFER_TOO_SMALL;
            pIrp->IoStatus.Information = 0; /* Output Buffer Size */
            ntStatus = STATUS_SUCCESS;
            break;

      default:
            KdPrint( ("PORTTALK: Unsupported IOCTL Call\n") );
            ntStatus = STATUS_UNSUCCESSFUL;
            pIrp->IoStatus.Information = 0;
            break;

    }
    pIrp->IoStatus.Status = ntStatus;
    IoCompleteRequest( pIrp, IO_NO_INCREMENT );
    return ntStatus;
}

VOID PortTalkUnload(IN PDRIVER_OBJECT DriverObject)
{
    WCHAR DOSNameBuffer[] = L"\\DosDevices\\PortTalk";
    UNICODE_STRING uniDOSString;

    KdPrint( ("PORTTALK: PortTalk is Unloading . .\n") );

    if(IOPM_local) MmFreeNonCachedMemory(IOPM_local, sizeof(IOPM));
    RtlInitUnicodeString(&uniDOSString, DOSNameBuffer);
    IoDeleteSymbolicLink (&uniDOSString);
    IoDeleteDevice(DriverObject->DeviceObject);
}