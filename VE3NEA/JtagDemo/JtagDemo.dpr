program JtagDemo;

uses
  Forms,
  Unit1 in 'Unit1.pas' {Form1},
  MercDev in 'Skimmer_Mercury\MercDev.pas',
  Fx2LpDev in 'Skimmer_Mercury\Fx2LpDev.pas',
  UsbDev in 'Skimmer_Mercury\UsbDev.pas',
  SndTypes in 'Skimmer_Mercury\SndTypes.pas',
  LibUSB in 'Skimmer_Mercury\LibUSB.pas';

{$R *.RES}

begin
  Application.Initialize;
  Application.CreateForm(TForm1, Form1);
  Application.Run;
end.

