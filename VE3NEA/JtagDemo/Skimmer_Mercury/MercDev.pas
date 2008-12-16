//------------------------------------------------------------------------------
//The contents of this file are subject to the Mozilla Public License
//Version 1.1 (the "License"); you may not use this file except in compliance
//with the License. You may obtain a copy of the License at
//http://www.mozilla.org/MPL/ Software distributed under the License is
//distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express
//or implied. See the License for the specific language governing rights and
//limitations under the License.
//
//The Original Code is MercDev.pas.
//
//The Initial Developer of the Original Code is Alex Shovkoplyas, VE3NEA.
//Portions created by Alex Shovkoplyas are
//Copyright (C) 2008 Alex Shovkoplyas. All Rights Reserved.
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// TUsbDevice -> TFx2LpDevice -> TMercuryDevice: Mercury hardware ctrl functions
//------------------------------------------------------------------------------
unit MercDev;

interface

uses
  Windows, Messages, SysUtils, Classes, Forms, SyncObjs, UsbDev, Fx2LpDev,
  SndTypes;


const
  OZY_IN_EP = $86;
  OZY_OUT_EP = $02;

  VRQ_FPGA_LOAD = 2;
  VRQ_I2C_WRITE = $08;
  VRQ_I2C_READ = $81;


  FL_BEGIN = 0;
  FL_XFER = 1;
  FL_END = 2;

  WM_RX_DATA = WM_USER + 45;
  USB_BLOCK_SIZE = 4096;


type
  TByteKind = (bkUnk, bkC0, bkC1, bkC2, bkC3, bkC4, bkI0, bkI1, bkI2, bkQ0, bkQ1, bkQ2, bkM0, bkM1);


  TProgressEvent = procedure(Sender: TObject; Current, Total: integer) of object;

  TMercuryDevice = class;


  TMercuryThread = class(TThread)
  private
    procedure SendFrequency;
    procedure SendCtrlBytes;
  public
    Papa: TMercuryDevice;
    TempRxData, RxData: TDataBufferF;

    SampleIdx: integer;
    ByteKind: TByteKind;
    IntValue: integer;
    RxDataReady: boolean;

    procedure Execute; override;
    procedure ProcessInputBlock(Buf: TByteArray);
    procedure InputBlockReady;
  end;


  TMercuryDevice = class(TFx2LpDevice)
  private
    FOnFpgaProgress: TProgressEvent;
    FOnData: TNotifyEvent;
    FLoFrequency: integer;
    Thr: TMercuryThread;
    WinH: THandle;

    procedure WriteI2C(Addr: integer; Data: TByteArray);
    procedure WriteI2C_TwoBytes(Addr: integer; Byte1, Byte2: Byte);
    procedure SetLoFrequency(const Value: integer);
    procedure MercWndProc(var Msg: TMessage);
    procedure GetRxData;
  protected
    procedure CloseDevice; override;
    procedure Start;
    procedure SendFpgaCommand(AIdx: integer; AData: TByteArray);
    procedure DoData(AData: TByteArray; var ADone: boolean); override;
  public
    FirmwareFileName, OzyFpgaFileName, MercFpgaFileName: TFileName;
    SamplesPerBuf, InputSamplingRate: integer;
    RxData: TDataBufferF;
    InCritSect, SettCritSect: TCriticalSection;
    InCtrlBytes: array[bkC0..bkC4] of Byte;
    OutCtrlBytes: array[bkC1..bkC4] of Byte;
    FreqChanged, CtrlBytesChanged: boolean;

    constructor Create;
    destructor Destroy; override;

    procedure Open;
    procedure OpenNoThread;
    procedure Close;
    procedure ReadData(AControlPoint: integer; var ABuf: TByteArray);
    procedure WriteData(AControlPoint: integer; var ABuf: TByteArray);

    procedure LoadOzyFpga(FileName: TFileName);
    function ListJtagDevices: string;
    procedure LoadMercuryFpga(AFileName: TFileName; DevIdx: integer);


    property LoFrequency: integer read FLoFrequency write SetLoFrequency;
    property OnFpgaProgress: TProgressEvent read FOnFpgaProgress write FOnFpgaProgress;
    property OnData: TNotifyEvent read FOnData write FOnData;
  end;



implementation


{ TMercuryThread }

procedure TMercuryThread.Execute;
var
  Buf: TByteArray;
begin
  Priority := tpTimeCritical;

  //init vars
  ByteKind := bkUnk;
  IntValue := 0;
  SampleIdx := 0;

  //create buffers
  SetLength(TempRxData, 2, Papa.SamplesPerBuf);
  SetLength(RxData, 2, Papa.SamplesPerBuf);
  SetLength(Buf, USB_BLOCK_SIZE);

  //ctrl bytes need to be sent on startup
  Papa.CtrlBytesChanged := true;
  Papa.FreqChanged := true;

  //read data in a loop
  repeat
    if Terminated then Exit;

    try
      if Papa.CtrlBytesChanged then SendCtrlBytes
      else if Papa.FreqChanged then SendFrequency;

      Papa.ReadDataInternal(OZY_IN_EP, Buf);
      ProcessInputBlock(Buf);

    except on E: Exception do
      begin
      Papa.FStatusText := E.Message;
      Terminate;
      end;
    end;

    if Terminated then Break;
  until false;
end;



procedure TMercuryThread.SendCtrlBytes;
var
  Buf: TByteArray;
  SamplingRateBits: Byte;
begin
  SetLength(Buf, 512);

  Buf[0] := $7F;
  Buf[1] := $7F;
  Buf[2] := $7F;
  Buf[3] := $00;

  case Papa.InputSamplingRate of
     48000: SamplingRateBits := 0;
     96000: SamplingRateBits := 1;
    192000: SamplingRateBits := 2;
    end;

  Papa.SettCritSect.Enter;
  try
    Papa.CtrlBytesChanged := false;
    Buf[4] := (Papa.OutCtrlBytes[bkC1] and $FC) or SamplingRateBits;
    Buf[5] := Papa.OutCtrlBytes[bkC2];
    Buf[6] := Papa.OutCtrlBytes[bkC3] or $10{rand};
    Buf[7] := Papa.OutCtrlBytes[bkC4];
  finally
    Papa.SettCritSect.Leave;
  end;

  Papa.WriteData(OZY_OUT_EP, Buf);
end;


procedure TMercuryThread.SendFrequency;
var
  Freq: array[0..3] of Byte;
  Buf: TByteArray;
begin
  //read frequency safely
  Papa.SettCritSect.Enter;
  try
    Papa.FreqChanged := false;
    PInteger(@Freq)^ := Papa.FLoFrequency;
  finally
    Papa.SettCritSect.Leave;
  end;

  //send frequency to the radio
  SetLength(Buf, 512);
  Buf[0] := $7F;
  Buf[1] := $7F;
  Buf[2] := $7F;
  Buf[3] := 2;
  Buf[4] := Freq[3];
  Buf[5] := Freq[2];
  Buf[6] := Freq[1];
  Buf[7] := Freq[0];

  Papa.WriteData(OZY_OUT_EP, Buf);
end;


procedure TMercuryThread.ProcessInputBlock(Buf: TByteArray);
var
  i: integer;
begin
  for i:=0 to High(Buf) do
    if (i > 1) and (Buf[i] = $7F) and (Buf[i-1] = $7F) and (Buf[i-2] = $7F)
      then //synchronize
        begin IntValue := 0; ByteKind := bkC0; end

      else //read
        case ByteKind of
          bkC0..bkC4:
            begin Papa.InCtrlBytes[ByteKind] := Buf[i]; Inc(ByteKind); end;

          bkI0, bkI1, bkQ0, bkQ1:
            begin IntValue := (IntValue or Buf[i]) shl 8; Inc(ByteKind); end;

          bkI2:
            begin
            IntValue := (IntValue or Buf[i]) shl 8;
            TempRxData[0, SampleIdx] := IntValue div 256;
            IntValue := 0;
            Inc(ByteKind);
            end;

          bkQ2:
            begin
            IntValue := (IntValue or Buf[i]) shl 8;
            TempRxData[1, SampleIdx] := IntValue div 256;

            Inc(SampleIdx);
            if SampleIdx = Papa.SamplesPerBuf then
              begin InputBlockReady; SampleIdx := 0; end;

            IntValue := 0;
            Inc(ByteKind);
            end;

          bkM0:
            Inc(ByteKind);

          bkM1:
            ByteKind := bkI0;
        end;
end;


procedure TMercuryThread.InputBlockReady;
begin
  Papa.InCritSect.Enter;
  try
    RxData[0] := Copy(TempRxData[0]);
    RxData[1] := Copy(TempRxData[1]);
    RxDataReady := true;
  finally
    Papa.InCritSect.Leave;
  end;

  PostMessage(Papa.WinH, WM_RX_DATA, 0, 0);
end;






//------------------------------------------------------------------------------
//                                 init
//------------------------------------------------------------------------------
constructor TMercuryDevice.Create;
begin
  inherited;

  InCritSect := TCriticalSection.Create;
  SettCritSect := TCriticalSection.Create;
  WinH := AllocateHWnd(MercWndProc);

  FVendorId := $FFFE;
  FProductId := 7;
  InputSamplingRate := 48000;
  SamplesPerBuf := 512;

  OutCtrlBytes[bkC1] := $D8;
  OutCtrlBytes[bkC2] := $00;
  OutCtrlBytes[bkC3] := $10;
  OutCtrlBytes[bkC4] := $00;

  //default file names
  FirmwareFileName := ExtractFilePath(ParamStr(0)) + 'Ozy_firmware.hex';
  OzyFpgaFileName := ExtractFilePath(ParamStr(0)) + 'Ozy_fpga.rbf';
  MercFpgaFileName := ExtractFilePath(ParamStr(0)) + 'Merc_fpga.rbf';
end;


destructor TMercuryDevice.Destroy;
begin
  CloseDevice;
  DeallocateHWnd(WinH);
  SettCritSect.Free;
  InCritSect.Free;
end;



//------------------------------------------------------------------------------
//                               open/close
//------------------------------------------------------------------------------
procedure TMercuryDevice.OpenNoThread;
begin
  try
    Start; //fails if firmware is not loaded
  except
    LoadFirmware(FirmwareFileName);
    Sleep(1500);
    LoadOzyFpga(OzyFpgaFileName);
    Sleep(1500);
    Start;
  end;
end;


procedure TMercuryDevice.Open;
begin
  OpenNoThread;
  Thr := TMercuryThread.Create(true);
  Thr.Papa := Self;
  Thr.Resume;
end;


procedure TMercuryDevice.Start;
begin
  OpenDevice;

  if FunUsbSetConfiguration(FHandle, 1) < 0 then Err('usb_set_configuration failed');
  if FunUsbClaimInterface(FHandle, 0) < 0 then Err('usb_claim_interface failed');
  if FunUsbSetAltInterface(FHandle, 0) < 0 then Err('usb_set_altinterface failed');

  if FunUsbClearHalt(FHandle, OZY_IN_EP) < 0 then Err('usb_clear_halt(OZY_IN_EP) failed');
  if FunUsbClearHalt(FHandle, OZY_OUT_EP) < 0 then Err('usb_clear_halt(OZY_OUT_EP) failed');
end;


procedure TMercuryDevice.Close;
begin
  CloseDevice;
end;


procedure TMercuryDevice.CloseDevice;
begin
  if Thr <> nil then
    begin
    Thr.Terminate;
    Thr.WaitFor;
    FreeAndNil(Thr);
    end;

  if FHandle <> nil then
    FunUsbReleaseInterface(FHandle, 0);

  inherited;
end;






//------------------------------------------------------------------------------
//                                fpga
//------------------------------------------------------------------------------
procedure TMercuryDevice.LoadOzyFpga(FileName: TFileName);
var
  Data: TByteArray;
begin
  if not FInitialized then Err('LoadOzyFpga failed: ' + FStatusText);
  if not FileExists(FileName) then Err('File not found: ' + FileName);

  OpenDevice;
  try
    SendFpgaCommand(FL_BEGIN, nil);

    with TFileStream.Create(FileName, fmOpenRead) do
      try
        repeat
          SetLength(Data, MAX_EP0_PACKET_SIZE);
          SetLength(Data, Read(Data[0], MAX_EP0_PACKET_SIZE));
          if Data = nil then Break;
          SendFpgaCommand(FL_XFER, Data);
          if Assigned(FOnFpgaProgress) then FOnFpgaProgress(Self, Position, Size);
        until false;
      finally
        Free;
        if Assigned(FOnFpgaProgress) then FOnFpgaProgress(Self, -1, -1);
      end;

    SendFpgaCommand(FL_END, nil);
  finally
    CloseDevice;
  end;
end;


procedure TMercuryDevice.SendFpgaCommand(AIdx: integer; AData: TByteArray);
var
  Data: TByteArray;
begin
  if AData = nil then SetLength(Data, 1) else Data := AData;

  if FunUsbControlMsg(FHandle, VRT_VENDOR_OUT, VRQ_FPGA_LOAD, 0, AIdx,
      Data[0], Length(AData), 1000) <> Length(AData) then
    Err('SendFpgaCommand failed');
end;






//------------------------------------------------------------------------------
//                                data
//------------------------------------------------------------------------------
procedure TMercuryDevice.DoData(AData: TByteArray; var ADone: boolean);
begin
end;


procedure TMercuryDevice.ReadData(AControlPoint: integer; var ABuf: TByteArray);
begin
  inherited ReadData(AControlPoint, ABuf);
end;


procedure TMercuryDevice.WriteI2C(Addr: integer; Data: TByteArray);
begin
  if (Data = nil) or (Length(Data) > MAX_EP0_PACKET_SIZE) then
    Err('Bad data passed to WriteI2C');

  if FunUsbControlMsg(FHandle, VRT_VENDOR_OUT, VRQ_I2C_WRITE, Addr, 0,
      Data[0], Length(Data), 1000) <> Length(Data) then
    Err('WriteI2C failed');
end;


procedure TMercuryDevice.WriteI2C_TwoBytes(Addr: integer; Byte1, Byte2: Byte);
var
  Data: TByteArray;
begin
  SetLength(Data, 2);
  Data[0] := Byte1;
  Data[1] := Byte2;
  WriteI2C(Addr, Data);
end;


procedure TMercuryDevice.WriteData(AControlPoint: integer; var ABuf: TByteArray);
begin
  inherited WriteData(AControlPoint, ABuf);
end;



procedure TMercuryDevice.SetLoFrequency(const Value: integer);
begin
  SettCritSect.Enter;
  try
    begin
    FLoFrequency := Value;
    FreqChanged := true;
    end;
  finally SettCritSect.Leave; end;
end;


procedure TMercuryDevice.MercWndProc(var Msg: TMessage);
begin
  try
    if Msg.Msg = WM_RX_DATA
      then GetRxData
      else Msg.Result := DefWindowProc(WinH, Msg.Msg, Msg.wParam, Msg.lParam);
  except
    Application.HandleException(Self);
  end
end;


procedure TMercuryDevice.GetRxData;
begin
  if Thr = nil then Exit;

  InCritSect.Enter;
  try
    if not Thr.RxDataReady then Exit;

    SetLength(RxData, 2);
    RxData[0] := Copy(Thr.RxData[0]);
    RxData[1] := Copy(Thr.RxData[1]);

    Thr.RxDataReady := false;

    if Assigned(FOnData) then FOnData(Self);
  finally
    InCritSect.Leave;
  end;
end;




const
  VRQ_JTAG_FUNC = $0F;

  JF_PORT       = 0;
  JF_LIST_DEV   = 1;
  JF_START_CFG  = 2;
  JF_XFER_CFG   = 3;
  JF_END_CFG    = 4;


function TMercuryDevice.ListJtagDevices: string;
var
  rc: integer;
  Buf: TByteArray;
begin
  if FHandle = nil then Err('ListJtagDevices failed: device not open');
  
  SetLength(Buf, 22);
  rc := FunUsbControlMsg(FHandle, VRT_VENDOR_IN, VRQ_JTAG_FUNC, 0, JF_LIST_DEV, Buf[0], 22, 1000);

  Result := (Format('rc=%d  err=%d  cnt=%d  id= $%.8x $%.8x $%.8x $%.8x $%.8x',
    [rc, Buf[0], Buf[1],
    PInteger(@Buf[2])^, PInteger(@Buf[6])^,
    PInteger(@Buf[10])^, PInteger(@Buf[14])^,
    PInteger(@Buf[18])^
    ]));
end;


procedure TMercuryDevice.LoadMercuryFpga(AFileName: TFileName; DevIdx: integer);
var
  Dummy, rc: integer;
  Data: TByteArray;
begin
  if not FInitialized then Err('LoadMercuryFpga failed: ' + FStatusText);
  if FHandle = nil then Err('LoadMercuryFpga failed: device not open');
  if not FileExists(AFileName) then Err('File not found: ' + AFileName);

  rc := FunUsbControlMsg(FHandle, VRT_VENDOR_OUT, VRQ_JTAG_FUNC, DevIdx, JF_START_CFG, Dummy, 0, 1000);
  if rc < 0 then Err('JF_START_CFG failed');

  with TFileStream.Create(AFileName, fmOpenRead) do
    try
      repeat
        SetLength(Data, MAX_EP0_PACKET_SIZE);
        SetLength(Data, Read(Data[0], MAX_EP0_PACKET_SIZE));
        if Data = nil then Break;

        rc := FunUsbControlMsg(FHandle, VRT_VENDOR_OUT, VRQ_JTAG_FUNC, DevIdx, JF_XFER_CFG, Data[0], Length(Data), 1000);

        if Assigned(FOnFpgaProgress) then FOnFpgaProgress(Self, Position, Size);
      until false;
    finally
      Free;
      if Assigned(FOnFpgaProgress) then FOnFpgaProgress(Self, -1, -1);
    end;

  rc := FunUsbControlMsg(FHandle, VRT_VENDOR_OUT, VRQ_JTAG_FUNC, DevIdx, JF_END_CFG, Dummy, 0, 1000);
end;


end.

