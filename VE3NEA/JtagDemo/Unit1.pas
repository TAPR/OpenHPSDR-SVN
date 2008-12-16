unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  ComCtrls, StdCtrls, Spin, MercDev;

type
  TForm1 = class(TForm)
    Button1: TButton;
    Button2: TButton;
    Label1: TLabel;
    SpinEdit1: TSpinEdit;
    Label2: TLabel;
    OpenDialog1: TOpenDialog;
    Label3: TLabel;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
  private
    procedure ProgressEvent(Sender: TObject; Current, Total: integer);
    { Private declarations }
  public
    Cnt: integer;
  end;

var
  Form1: TForm1;

implementation

{$R *.DFM}

procedure TForm1.Button1Click(Sender: TObject);
begin
  Label1.Caption := '';

  with TMercuryDevice.Create do
    try
      OnFpgaProgress := ProgressEvent;
      OpenNoThread;
      Label1.Caption := ListJtagDevices;
    finally
      Free;
    end;
end;

procedure TForm1.Button2Click(Sender: TObject);
begin
  OpenDialog1.FileName := ExtractFilePath(ParamStr(0)) + 'Merc_fpga.rbf';
  if not OpenDialog1.Execute then Exit;

  Label1.Caption := '';

  with TMercuryDevice.Create do
    try
      OnFpgaProgress := ProgressEvent;
      OpenNoThread;

      Label1.Caption := 'Loading FPGA via JTAG   ';
      LoadMercuryFpga(OpenDialog1.FileName, SpinEdit1.Value);
    finally
      Free;
    end;

end;

procedure TForm1.ProgressEvent(Sender: TObject; Current, Total: integer);
begin
  if Label1.Caption = '' then Label1.Caption := 'Loading Ozy FPGA   ';

  Inc(Cnt);
  if (Cnt and $3F) = 0 then
    begin
    Label1.Caption := Label1.Caption + '|';
    Label1.Update;
    end;

  if Current = -1 then Label1.Caption := 'DONE';
end;

end.

