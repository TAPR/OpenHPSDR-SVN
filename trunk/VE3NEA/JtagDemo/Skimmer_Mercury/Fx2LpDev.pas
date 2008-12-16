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
// TUsbDevice -> TFx2LpDevice: USB device functions specific to the FX2LP family
//------------------------------------------------------------------------------
unit Fx2LpDev;

interface

uses
  SysUtils, Classes, SndTypes, UsbDev;


const
  MAX_EP0_PACKET_SIZE = 64;

  VRT_VENDOR_IN = $C0;
  VRT_VENDOR_OUT = $40;

type
  TFx2LpDevice = class(TUsbDevice)
  private
    procedure LoadFirmwareString(S: string);
  protected
    procedure ResetCpu(Value: boolean);
    procedure WriteRam(Addr: integer; AData: TByteArray);
    procedure LoadFirmware(FileName: TFileName);
  end;

  
implementation

//------------------------------------------------------------------------------
//                               firmware
//------------------------------------------------------------------------------
procedure TFx2LpDevice.LoadFirmware(FileName: TFileName);
var
  i: integer;
begin
  if not FInitialized then Err('LoadFirmware failed: ' + FStatusText);

  if not FileExists(FileName) then Err('File not found: ' + FileName);

  OpenDevice;
  ResetCpu(true);

  with TStringList.Create do
    try
      LoadFromFile(FileName);
      for i:=0 to Count-1 do
        try
          LoadFirmwareString(Strings[i]);
        except
          Err(Format('Error in %s, line %d', [FileName, i]));
        end;
    finally
      Free;
    end;

  ResetCpu(false);
  CloseDevice;
end;


procedure TFx2LpDevice.LoadFirmwareString(S: string);
var
  i: integer;
  Addr, Len, Typ, Sum: Word;
  b: Byte;
  Data: TByteArray;
begin
  //parse header
  if S[1] <> ':' then Abort;

  Len := StrToInt('$' + Copy(S, 2, 2));
  if Length(S) <> (Len*2 + 11) then Abort;

  Addr := StrToInt('$' + Copy(S, 4, 4));

  Typ := StrToInt('$' + Copy(S, 8, 2));
  if not (Typ in [0,1]) then  Abort;
  if Typ = 1 then Exit;

  //initialize checksum
  Sum := StrToInt('$' + Copy(S, 10+Len*2, 2));
  Inc(Sum, Len + Lo(Addr) + Hi(Addr));

  //extract bytes
  SetLength(Data, Len);
  for i:=0 to Len-1 do
    begin
    b := StrToInt('$' + Copy(S, 10+i*2, 2));
    Data[i] := b;
    Inc(Sum, b);
    end;

  //verify checksum
  if (Sum and $FF)<> 0 then Abort;

  //write
  WriteRam(Addr, Data);
end;



//------------------------------------------------------------------------------
//                                 CPU
//------------------------------------------------------------------------------
procedure TFx2LpDevice.WriteRam(Addr: integer; AData: TByteArray);
var
  Data: TByteArray;
begin
  if FHandle = nil then Err('WriteRam failed: device not open');

  Data := Copy(AData, 0, MAX_EP0_PACKET_SIZE);
  while Data <> nil do
    begin
    if FunUsbControlMsg(FHandle, VRT_VENDOR_OUT, $A0, Addr, 0, Data[0], Length(Data), 1000) <> Length(Data)
      then Err('WriteRam/usb_control_msg failed');

    Inc(Addr, Length(Data));
    AData := Copy(AData, Length(Data), MAXINT);
    Data := Copy(AData, 0, MAX_EP0_PACKET_SIZE);
    end;
end;


procedure TFx2LpDevice.ResetCpu(Value: boolean);
var
  Data: TByteArray;
begin
  if FHandle = nil then Err('ResetCpu failed: device not open');

  SetLength(Data, 1);
  if Value then Data[0] := 1 else Data[0] := 0;
  WriteRam($E600, Data);
end;






end.

