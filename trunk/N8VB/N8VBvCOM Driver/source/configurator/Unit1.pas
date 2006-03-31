{ KOL MCK } // Do not remove this line!
{$DEFINE KOL_MCK}
unit Unit1;
//============================================================================
//
//  VComConfigurator - Configuration Tool
//  Copyright (c) 2006 Philip A Covington, N8VB
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
interface

{$IFDEF KOL_MCK}
uses Windows, Messages, ShellAPI, KOL {$IFNDEF KOL_MCK}, mirror, Classes, Controls, mckControls, mckObjs, Graphics,  ExtCtrls, mckCtrls {$ENDIF (place your units here->)};
{$ELSE}
{$I uses.inc}
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, mirror;
{$ENDIF}

type
  {$IFDEF KOL_MCK}
  {$I MCKfakeClasses.inc}
  {$IFDEF KOLCLASSES} TForm1 = class; PForm1 = TForm1; {$ELSE OBJECTS} PForm1 = ^TForm1; {$ENDIF CLASSES/OBJECTS}
  {$IFDEF KOLCLASSES}{$I TForm1.inc}{$ELSE} TForm1 = object(TObj) {$ENDIF}
    Form: PControl;
  {$ELSE not_KOL_MCK}
  TForm1 = class(TForm)
  {$ENDIF KOL_MCK}
    KOLProject1: TKOLProject;
    KOLForm1: TKOLForm;
    mnuMain: TKOLMainMenu;
    Panel1: TKOLPanel;
    Panel2: TKOLPanel;
    GradientPanel5: TKOLGradientPanel;
    LabelEffect6: TKOLLabelEffect;
    cboLeft1: TKOLComboBox;
    cboLeft2: TKOLComboBox;
    cboLeft3: TKOLComboBox;
    cboLeft4: TKOLComboBox;
    cboRight1: TKOLComboBox;
    cboRight2: TKOLComboBox;
    cboRight3: TKOLComboBox;
    cboRight4: TKOLComboBox;
    GradientPanel10: TKOLGradientPanel;
    btnSave: TKOLButton;
    CheckBox1: TKOLCheckBox;
    GradientPanel1: TKOLGradientPanel;
    Label1: TKOLLabel;
    GradientPanel2: TKOLGradientPanel;
    CheckBox2: TKOLCheckBox;
    GradientPanel3: TKOLGradientPanel;
    GradientPanel4: TKOLGradientPanel;
    CheckBox3: TKOLCheckBox;
    GradientPanel6: TKOLGradientPanel;
    GradientPanel7: TKOLGradientPanel;
    CheckBox4: TKOLCheckBox;
    GradientPanel8: TKOLGradientPanel;
    GradientPanel9: TKOLGradientPanel;
    CheckBox5: TKOLCheckBox;
    GradientPanel11: TKOLGradientPanel;
    cboLeft5: TKOLComboBox;
    cboRight5: TKOLComboBox;
    cboLeft6: TKOLComboBox;
    cboLeft7: TKOLComboBox;
    cboLeft8: TKOLComboBox;
    cboLeft9: TKOLComboBox;
    cboRight6: TKOLComboBox;
    cboRight7: TKOLComboBox;
    cboRight8: TKOLComboBox;
    cboRight9: TKOLComboBox;
    GradientPanel12: TKOLGradientPanel;
    CheckBox6: TKOLCheckBox;
    GradientPanel13: TKOLGradientPanel;
    GradientPanel14: TKOLGradientPanel;
    CheckBox7: TKOLCheckBox;
    GradientPanel15: TKOLGradientPanel;
    GradientPanel16: TKOLGradientPanel;
    CheckBox8: TKOLCheckBox;
    GradientPanel17: TKOLGradientPanel;
    GradientPanel18: TKOLGradientPanel;
    CheckBox9: TKOLCheckBox;
    GradientPanel19: TKOLGradientPanel;
    GradientPanel20: TKOLGradientPanel;
    CheckBox10: TKOLCheckBox;
    GradientPanel21: TKOLGradientPanel;
    cboLeft10: TKOLComboBox;
    cboRight10: TKOLComboBox;
    LabelEffect1: TKOLLabelEffect;
    Button1: TKOLButton;
    Button2: TKOLButton;
    Button3: TKOLButton;
    procedure KOLForm1FormCreate(Sender: PObj);
    procedure mnuMainmnuFileSaveMenu(Sender: PMenu; Item: Integer);
    procedure btnSaveClick(Sender: PObj);
    procedure KOLForm1mnuHelpAboutMenu(Sender: PMenu; Item: Integer);
    procedure KOLForm1mnuFileExitMenu(Sender: PMenu; Item: Integer);
    procedure chkBoxClick(Sender: PObj);
    procedure cboPortsChange(Sender: PObj);
    procedure Button1Click(Sender: PObj);
    procedure Button2Click(Sender: PObj);
    procedure Button3Click(Sender: PObj);
  private
    { Private declarations }
    mChanged: boolean;
    // COM
    VirtEnabledPorts: array[1..10] of boolean;
    VirtPorts: array[1..10] of string;
    VirtClientPorts: array[1..10] of string;
    procedure SaveConfiguration;
    procedure ReadConfiguration;
    procedure WriteConfiguration;
    function AskSave: integer;
  public
    { Public declarations }
  end;

var
  Form1 {$IFDEF KOL_MCK} : PForm1 {$ELSE} : TForm1 {$ENDIF} ;

{$IFDEF KOL_MCK}
procedure NewForm1( var Result: PForm1; AParent: PControl );
{$ENDIF}

implementation

{$IFNDEF KOL_MCK} {$R *.DFM} {$ENDIF}

{$IFDEF KOL_MCK}
{$I Unit1_1.inc}
{$ENDIF}

const
  ConfiguratorName = 'N8VBvCOM.inf';
  PortsSection = 'Strings';

procedure TForm1.KOLForm1FormCreate(Sender: PObj);
var
  i: integer;
  str: string;
begin
  mChanged := false;
  for i := 1 to 50 do begin
    str := 'COM' + int2Str(i);
    cboLeft1.Add(str);
    cboRight1.Add(str);
    cboLeft2.Add(str);
    cboRight2.Add(str);
    cboLeft3.Add(str);
    cboRight3.Add(str);
    cboLeft4.Add(str);
    cboRight4.Add(str);
    cboLeft5.Add(str);
    cboRight5.Add(str);
    cboLeft6.Add(str);
    cboRight6.Add(str);
    cboLeft7.Add(str);
    cboRight7.Add(str);
    cboLeft8.Add(str);
    cboRight8.Add(str);
    cboLeft9.Add(str);
    cboRight9.Add(str);
    cboLeft10.Add(str);
    cboRight10.Add(str);
  end;

  ReadConfiguration;

  CheckBox1.Checked := self.VirtEnabledPorts[1];
  cboLeft1.ItemSelected[cboLeft1.IndexOf(VirtClientPorts[1])] := true;
  cboRight1.ItemSelected[cboLeft1.IndexOf(VirtPorts[1])] := true;
  CheckBox2.Checked := self.VirtEnabledPorts[2];
  cboLeft2.ItemSelected[cboLeft2.IndexOf(VirtClientPorts[2])] := true;
  cboRight2.ItemSelected[cboLeft2.IndexOf(VirtPorts[2])] := true;
  CheckBox3.Checked := self.VirtEnabledPorts[3];
  cboLeft3.ItemSelected[cboLeft3.IndexOf(VirtClientPorts[3])] := true;
  cboRight3.ItemSelected[cboLeft3.IndexOf(VirtPorts[3])] := true;
  CheckBox4.Checked := self.VirtEnabledPorts[4];
  cboLeft4.ItemSelected[cboLeft4.IndexOf(VirtClientPorts[4])] := true;
  cboRight4.ItemSelected[cboLeft4.IndexOf(VirtPorts[4])] := true;
  CheckBox5.Checked := self.VirtEnabledPorts[5];
  cboLeft5.ItemSelected[cboLeft5.IndexOf(VirtClientPorts[5])] := true;
  cboRight5.ItemSelected[cboLeft5.IndexOf(VirtPorts[5])] := true;
  CheckBox6.Checked := self.VirtEnabledPorts[6];
  cboLeft6.ItemSelected[cboLeft6.IndexOf(VirtClientPorts[6])] := true;
  cboRight6.ItemSelected[cboLeft6.IndexOf(VirtPorts[6])] := true;
  CheckBox7.Checked := self.VirtEnabledPorts[7];
  cboLeft7.ItemSelected[cboLeft7.IndexOf(VirtClientPorts[7])] := true;
  cboRight7.ItemSelected[cboLeft7.IndexOf(VirtPorts[7])] := true;
  CheckBox8.Checked := self.VirtEnabledPorts[8];
  cboLeft8.ItemSelected[cboLeft8.IndexOf(VirtClientPorts[8])] := true;
  cboRight8.ItemSelected[cboLeft8.IndexOf(VirtPorts[8])] := true;
  CheckBox9.Checked := self.VirtEnabledPorts[9];
  cboLeft9.ItemSelected[cboLeft9.IndexOf(VirtClientPorts[9])] := true;
  cboRight9.ItemSelected[cboLeft9.IndexOf(VirtPorts[9])] := true;
  CheckBox10.Checked := self.VirtEnabledPorts[10];
  cboLeft10.ItemSelected[cboLeft10.IndexOf(VirtClientPorts[10])] := true;
  cboRight10.ItemSelected[cboLeft10.IndexOf(VirtPorts[10])] := true;
end;

procedure TForm1.SaveConfiguration;
var
  ports: set of byte;
  doubles: boolean;
begin
  doubles := false;
  ports := [];
  if CheckBox1.Checked then begin
    ports := ports + [cboLeft1.CurIndex];
    if (cboRight1.CurIndex in ports) then doubles := true;
    ports := ports + [cboRight1.CurIndex];
  end;
  if CheckBox2.Checked then begin
    if (cboLeft2.CurIndex in ports) then doubles := true;
    ports := ports + [cboLeft2.CurIndex];
    if (cboRight2.CurIndex in ports) then doubles := true;
    ports := ports + [cboRight2.CurIndex];
  end;
  if CheckBox3.Checked then begin
    if (cboLeft3.CurIndex in ports) then doubles := true;
    ports := ports + [cboLeft3.CurIndex];
    if (cboRight3.CurIndex in ports) then doubles := true;
    ports := ports + [cboRight3.CurIndex];
  end;
  if CheckBox4.Checked then begin
    if (cboLeft4.CurIndex in ports) then doubles := true;
    ports := ports + [cboLeft4.CurIndex];
    if (cboRight4.CurIndex in ports) then doubles := true;
    ports := ports + [cboRight4.CurIndex];
  end;
  if CheckBox5.Checked then begin
    if (cboLeft5.CurIndex in ports) then doubles := true;
    ports := ports + [cboLeft5.CurIndex];
    if (cboRight5.CurIndex in ports) then doubles := true;
    ports := ports + [cboRight5.CurIndex];
  end;
  if CheckBox6.Checked then begin
    if (cboLeft6.CurIndex in ports) then doubles := true;
    ports := ports + [cboLeft6.CurIndex];
    if (cboRight6.CurIndex in ports) then doubles := true;
    ports := ports + [cboRight6.CurIndex];
  end;
  if CheckBox7.Checked then begin
    if (cboLeft7.CurIndex in ports) then doubles := true;
    ports := ports + [cboLeft7.CurIndex];
    if (cboRight7.CurIndex in ports) then doubles := true;
    ports := ports + [cboRight7.CurIndex];
  end;
  if CheckBox8.Checked then begin
    if (cboLeft8.CurIndex in ports) then doubles := true;
    ports := ports + [cboLeft8.CurIndex];
    if (cboRight8.CurIndex in ports) then doubles := true;
    ports := ports + [cboRight8.CurIndex];
  end;
  if CheckBox9.Checked then begin
    if (cboLeft9.CurIndex in ports) then doubles := true;
    ports := ports + [cboLeft9.CurIndex];
    if (cboRight9.CurIndex in ports) then doubles := true;
    ports := ports + [cboRight9.CurIndex];
  end;
  if CheckBox10.Checked then begin
    if (cboLeft10.CurIndex in ports) then doubles := true;
    ports := ports + [cboLeft10.CurIndex];
    if (cboRight10.CurIndex in ports) then doubles := true;
    ports := ports + [cboRight10.CurIndex];
  end;
  if (doubles) then begin
    MessageBox(Form1.Form.Handle, 'Cannot save. All the selected port numbers must be unique.', 'VComConfigurator Save Error', MB_OK or MB_ICONEXCLAMATION);
  end else begin
    VirtEnabledPorts[1] := CheckBox1.Checked;
    VirtClientPorts[1] := cboLeft1.Text;
    VirtPorts[1] := cboRight1.Text;
    VirtEnabledPorts[2] := CheckBox2.Checked;
    VirtClientPorts[2] := cboLeft2.Text;
    VirtPorts[2] := cboRight2.Text;
    VirtEnabledPorts[3] := CheckBox3.Checked;
    VirtClientPorts[3] := cboLeft3.Text;
    VirtPorts[3] := cboRight3.Text;
    VirtEnabledPorts[4] := CheckBox4.Checked;
    VirtClientPorts[4] := cboLeft4.Text;
    VirtPorts[4] := cboRight4.Text;
    VirtEnabledPorts[5] := CheckBox5.Checked;
    VirtClientPorts[5] := cboLeft5.Text;
    VirtPorts[5] := cboRight5.Text;
    VirtEnabledPorts[6] := CheckBox6.Checked;
    VirtClientPorts[6] := cboLeft6.Text;
    VirtPorts[6] := cboRight6.Text;
    VirtEnabledPorts[7] := CheckBox7.Checked;
    VirtClientPorts[7] := cboLeft7.Text;
    VirtPorts[7] := cboRight7.Text;
    VirtEnabledPorts[8] := CheckBox8.Checked;
    VirtClientPorts[8] := cboLeft8.Text;
    VirtPorts[8] := cboRight8.Text;
    VirtEnabledPorts[9] := CheckBox9.Checked;
    VirtClientPorts[9] := cboLeft9.Text;
    VirtPorts[9] := cboRight9.Text;
    VirtEnabledPorts[10] := CheckBox10.Checked;
    VirtClientPorts[10] := cboLeft10.Text;
    VirtPorts[10] := cboRight10.Text;

    WriteConfiguration;
  end;
end;

procedure TForm1.ReadConfiguration;
var
  i: integer;
  portCount: integer;
  iniFile: PIniFile;
  str: string;
begin
  // read values from VCom driver inf file
  iniFile := OpenIniFile('.\N8VBvCOM.inf');
  try
    iniFile.Mode := ifmRead;
    iniFile.Section := 'Strings';
    // read enabled port pair count
    try
      str := iniFile.ValueString('PAIRS', '0x0');
      portCount := hex2Int(Copy(str, 3, Length(str)-1));
    except
      portCount := 0;
    end;
    for i := Low(VirtPorts) to High(VirtPorts) do begin
      VirtEnabledPorts[i] := (i <= portCount);
      VirtClientPorts[i] := iniFile.ValueString('PORT' + int2Str(((i-1) * 2)+1), 'COM' + int2Str(i+5));
      VirtPorts[i] := iniFile.ValueString('PORT' + int2Str(((i-1) * 2)+2), 'COM' + int2Str(i+15));
    end;
  finally
    iniFile.Free;
  end;
  mChanged := false;
end;

procedure TForm1.WriteConfiguration;
var
  i: integer;
  portCount: integer;
  iniFile: PIniFile;
begin
  // write changes to VCom driver inf file
  iniFile := OpenIniFile('.\N8VBvCOM.inf');
  try
    iniFile.Mode := ifmWrite;
    iniFile.Section := 'Strings';
    // write enabled port pairs
    portCount := 0;
    for i := Low(VirtPorts) to High(VirtPorts) do begin
      if (VirtEnabledPorts[i]) then begin
        Inc(portCount);
        iniFile.ValueString('PORT' + int2Str(((portCount-1) * 2)+1), VirtClientPorts[i]);
        iniFile.ValueString('PORT' + int2Str(((portCount-1) * 2)+2), VirtPorts[i]);
      end;
    end;
    iniFile.ValueString('PAIRS', '0x' + Int2Hex(portCount, 2));
    // write unused port pairs
    for i := Low(VirtPorts) to High(VirtPorts) do begin
      if (not VirtEnabledPorts[i]) then begin
        Inc(portCount);
        iniFile.ValueString('PORT' + int2Str(((portCount-1) * 2)+1), VirtClientPorts[i]);
        iniFile.ValueString('PORT' + int2Str(((portCount-1) * 2)+2), VirtPorts[i]);
      end;
    end;
  finally
    iniFile.Free;
  end;
  mChanged := false;
end;

procedure TForm1.mnuMainmnuFileSaveMenu(Sender: PMenu; Item: Integer);
begin
  SaveConfiguration;
end;

procedure TForm1.btnSaveClick(Sender: PObj);
begin
  SaveConfiguration;
end;

procedure TForm1.KOLForm1mnuHelpAboutMenu(Sender: PMenu; Item: Integer);
begin
  MessageBox(Form1.Form.Handle,
  'VComConfigurator' + #13#10#13#10
  + 'Configures N8VBvCOM, the open source virtual COM port driver.' + #13#10
  + 'N8VBvCOM driver & Configurator Copyright ©2005,2006 Philip A. Covington' + #13#10
  + 'License: GNU General Public License',
  'About VComConfigurator', MB_OK);
end;

procedure TForm1.KOLForm1mnuFileExitMenu(Sender: PMenu; Item: Integer);
begin
  Form1.Form.Close;
end;

procedure TForm1.chkBoxClick(Sender: PObj);
begin
  mChanged := true;
end;

procedure TForm1.cboPortsChange(Sender: PObj);
begin
  mChanged := true;
end;

procedure TForm1.Button1Click(Sender: PObj);
var
  answer: integer;
begin
  if (mChanged) then begin
    answer := AskSave;
    if (answer = IDCANCEL) then exit; // cancel
    if (answer = IDYES) then begin
      SaveConfiguration;
      if (mChanged) then exit; // error while saving
    end;
  end;
  ExecuteWait('installDriver.bat', '', '', SW_SHOW, 15000, nil);
end;

procedure TForm1.Button2Click(Sender: PObj);
var
  answer: integer;
begin
  if (mChanged) then begin
    answer := AskSave;
    if (answer = IDCANCEL) then exit; // cancel
    if (answer = IDYES) then begin
      SaveConfiguration;
      if (mChanged) then exit; // error while saving
    end;
  end;
  ExecuteWait('update.bat', '', '', SW_SHOW, 15000, nil);
end;

procedure TForm1.Button3Click(Sender: PObj);
var
  answer: integer;
begin
  if (mChanged) then begin
    answer := AskSave;
    if (answer = IDCANCEL) then exit; // cancel
    if (answer = IDYES) then begin
      SaveConfiguration;
      if (mChanged) then exit; // error while saving
    end;
  end;
  ExecuteWait('removeDriver.bat', '', '', SW_SHOW, 15000, nil);
end;

function TForm1.AskSave: integer;
begin
  Result := MessageBox(Form1.Form.Handle,
  'You have unsaved configuration changes. Save them now?',
  'VComConfigurator', MB_YESNOCANCEL);
end;

end.





