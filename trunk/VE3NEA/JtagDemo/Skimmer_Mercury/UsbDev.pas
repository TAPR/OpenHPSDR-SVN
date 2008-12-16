//------------------------------------------------------------------------------
//The contents of this file are subject to the Mozilla Public License
//Version 1.1 (the "License"); you may not use this file except in compliance
//with the License. You may obtain a copy of the License at
//http://www.mozilla.org/MPL/ Software distributed under the License is
//distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express
//or implied. See the License for the specific language governing rights and
//limitations under the License.
//
//The Original Code is UsbDev.pas.
//
//The Initial Developer of the Original Code is Alex Shovkoplyas, VE3NEA.
//Portions created by Alex Shovkoplyas are
//Copyright (C) 2008 Alex Shovkoplyas. All Rights Reserved.
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// TUsbDevice: low level USB device functions
//------------------------------------------------------------------------------
// requires LibUsb.pas
//------------------------------------------------------------------------------

unit UsbDev;

interface

uses
  Windows, SysUtils, Classes, Forms, SndTypes, LibUsb;

type
  TUsbDevice = class;


  TReadThread = class(TTHread)
  protected
    Papa: TUsbDevice;
    Buf: TByteArray;
    ControlPoint: integer;
    procedure Execute; override;
  end;


  TUsbDevice = class
  protected
    FInitialized: boolean;
    FStatusText: string;
    FLib: THandle;
    Thr: TReadThread;
    FProductId: integer;
    FVendorId: integer;
    FHandle: pusb_dev_handle;

    FunUsbInit: function: integer; cdecl;
    FunUsbFindBuses: function: integer; cdecl;
    FunUsbFindDefices: function: integer; cdecl;
    FunUsbGetBuses: function: pusb_bus; cdecl;
    FunUsbOpen: function(dev: pusb_device): pusb_dev_handle; cdecl;
    FunUsbSetConfiguration: function  (dev: pusb_dev_handle;configuration: longword): integer; cdecl;
    FunUsbClaimInterface: function  (dev: pusb_dev_handle;iinterface: longword): integer;  cdecl;
    FunUsbReleaseInterface: function  (dev: pusb_dev_handle;iinterface: longword): integer; cdecl;
    FunUsbSetAltInterface: function  (dev: pusb_dev_handle;alternate: longword): integer; cdecl;
    FunUsbClearHalt: function  (dev: pusb_dev_handle;ep: longword): integer; cdecl;
    FunUsbClose: function  (dev: pusb_dev_handle): longword; cdecl;
    FunUsbControlMsg: function  (dev: pusb_dev_handle;requesttype, request, value, index: longword;var bytes;size, timeout: longword): integer; cdecl;
    FunUsbBulkRead: function  (dev: pusb_dev_handle;ep: longword; var bytes; size,timeout:longword): integer; cdecl;
    FunUsbBulkWrite: function (dev: pusb_dev_handle;ep : longword; var bytes;size,timeout:longword): longword; cdecl;
    FunUsbStrError: function : pchar; cdecl;


    procedure Err(Msg: string);
    procedure Initialize;
    procedure LoadDll;
    procedure OpenDevice;
    procedure CloseDevice; virtual;
    procedure ReadDataInternal(AControlPoint: integer; var ABuf: TByteArray);
    procedure ReadData(AControlPoint: integer; var ABuf: TByteArray);
    procedure ReadDataAsync(AControlPoint, ACount: integer);
    procedure WriteData(AControlPoint: integer; var ABuf: TByteArray);
    procedure DataArrived;
    procedure DoData(AData: TByteArray; var ADone: boolean); virtual; abstract;
  public
    constructor Create;
    destructor Destroy; override;
    function IsRunning: boolean;

    property VendorId: integer read FVendorId write FVendorId;
    property ProductId: integer read FProductId write FProductId;
    property Initialized: boolean read FInitialized;
    property StatusText: string read FStatusText;
  end;


implementation

{ TReadThread }

procedure TReadThread.Execute;
begin
  repeat
    if Terminated then Exit;

    try
      Papa.ReadDataInternal(ControlPoint, Buf);
    except on E: Exception do
      begin
      Papa.FStatusText := E.Message;
      Papa.Thr := nil;
      Terminate;
      end;
    end;

    if Terminated then Exit;
    Synchronize(Papa.DataArrived);
  until false;
end;




{ TUsbDevice }

constructor TUsbDevice.Create;
begin
  Initialize;
end;


destructor TUsbDevice.Destroy;
begin
  CloseDevice;
  if FLib <> 0 then FreeLibrary(FLib);
  inherited;
end;


procedure TUsbDevice.Err(Msg: string);
begin
  CloseDevice;
  FStatusText := Msg;
  raise Exception.Create(Msg);
end;


procedure TUsbDevice.Initialize;
begin
  try
    LoadDll;
    FInitialized := true;
    FStatusText := 'OK';
  except
    FStatusText := 'Unable to load ' + LIBUSB_DLL_NAME;
  end;
end;


procedure TUsbDevice.LoadDll;
begin
    FLib := LoadLibrary(LIBUSB_DLL_NAME);
    if FLib = 0 then Abort;

  try
    @FunUsbInit := GetProcAddress(FLib, 'usb_init');
    if not Assigned(FunUsbInit) then Abort;

    @FunUsbFindBuses := GetProcAddress(FLib, 'usb_find_busses');
    if not Assigned(FunUsbFindBuses) then Abort;

    @FunUsbFindDefices := GetProcAddress(FLib, 'usb_find_devices');
    if not Assigned(FunUsbFindDefices) then Abort;

    @FunUsbGetBuses := GetProcAddress(FLib, 'usb_get_busses');
    if not Assigned(FunUsbGetBuses) then Abort;

    @FunUsbOpen := GetProcAddress(FLib, 'usb_open');
    if not Assigned(FunUsbOpen) then Abort;

    @FunUsbSetConfiguration := GetProcAddress(FLib, 'usb_set_configuration');
    if not Assigned(FunUsbSetConfiguration) then Abort;

    @FunUsbClaimInterface := GetProcAddress(FLib, 'usb_claim_interface');
    if not Assigned(FunUsbClaimInterface) then Abort;

    @FunUsbReleaseInterface := GetProcAddress(FLib, 'usb_release_interface');
    if not Assigned(FunUsbReleaseInterface) then Abort;

    @FunUsbSetAltInterface := GetProcAddress(FLib, 'usb_set_altinterface');
    if not Assigned(FunUsbSetAltInterface) then Abort;

    @FunUsbClearHalt := GetProcAddress(FLib, 'usb_clear_halt');
    if not Assigned(FunUsbClearHalt) then Abort;

    @FunUsbClose := GetProcAddress(FLib, 'usb_close');
    if not Assigned(FunUsbClose) then Abort;

    @FunUsbControlMsg := GetProcAddress(FLib, 'usb_control_msg');
    if not Assigned(FunUsbControlMsg) then Abort;

    @FunUsbBulkRead := GetProcAddress(FLib, 'usb_bulk_read');
    if not Assigned(FunUsbBulkRead) then Abort;

    @FunUsbBulkWrite := GetProcAddress(FLib, 'usb_bulk_write');
    if not Assigned(FunUsbBulkWrite) then Abort;

    @FunUsbStrError := GetProcAddress(FLib, 'usb_strerror');
    if not Assigned(FunUsbStrError) then Abort;
  except
    FreeLibrary(FLib);
    FLib := 0;
    Abort;
  end;
end;


procedure TUsbDevice.OpenDevice;
var
  Bus: pusb_bus;
  Dev: pusb_device;
begin
  if not FInitialized then Err(FStatusText);

  FStatusText := 'OK';

  FunUsbInit;
  FunUsbFindBuses;
  FunUsbFindDefices;
  Bus := FunUsbGetBuses;
  
  while Bus <> nil do
    begin
    Dev := Bus.devices;

    while Dev <> nil do
      with Dev.descriptor do
        if (idVendor = FVendorId) and (idProduct = FProductId)
          then
            begin
            FHandle := FunUsbOpen(Dev);
            if FHandle <> nil then Exit else Err('usb_open failed');
            end
          else
            Dev := Dev.next;

    Bus := Bus.next;
    end;

  Err('USB device not found');
end;



procedure TUsbDevice.CloseDevice;
begin
  if FHandle = nil then Exit;

  if Thr <> nil then
    if Thr.Suspended
      then
        FreeAndNil(Thr)
      else
        begin
        Thr.Terminate;
        Thr := nil;
        end;

  FunUsbClose(FHandle);
  FHandle := nil;
end;


//calls Err() if an error occurs
procedure TUsbDevice.ReadData(AControlPoint: integer; var ABuf: TByteArray);
begin
  if FHandle = nil then Err('ReadData failed: device not open');

  try
    ReadDataInternal(AControlPoint, ABuf);
  except on E: Exception do
    Err(E.Message);
  end;
end;


//raises an exception if an error occurs
procedure TUsbDevice.ReadDataInternal(AControlPoint: integer; var ABuf: TByteArray);
var
  Cnt: integer;
begin
  Cnt := FunUsbBulkRead(FHandle, AControlPoint, ABuf[0], Length(ABuf), 5000);
  if Cnt <> Length(ABuf)
    then raise Exception.Create('usb_bulk_read failed');
end;


procedure TUsbDevice.WriteData(AControlPoint: integer; var ABuf: TByteArray);
var
  Cnt: integer;
begin
  Cnt := FunUsbBulkWrite(FHandle, AControlPoint, ABuf[0], Length(ABuf), 5000);
  if Cnt <> Length(ABuf)
    then raise Exception.Create('usb_bulk_write failed');
end;


procedure TUsbDevice.ReadDataAsync(AControlPoint, ACount: integer);
begin
  if FHandle = nil then Err('ReadDataAsync failed: device not open');

  Thr := TReadThread.Create(true);
  Thr.FreeOnTerminate := true;
  Thr.Papa := Self;
  Thr.ControlPoint := AControlPoint;
  SetLength(Thr.Buf, ACount);
  Thr.Resume;
end;


procedure TUsbDevice.DataArrived;
var
  Done: boolean;
begin
  if Thr = nil then Exit;

  Done := false;
  try
    DoData(Thr.Buf, Done);
  except
    //Application.HandleException(Self);
  end;

  if Done then begin Thr.Terminate; Thr := nil; end;
end;



function TUsbDevice.IsRunning: boolean;
begin
  Result := (FHandle <> nil);
end;

end.

