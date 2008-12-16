object Form1: TForm1
  Left = 281
  Top = 116
  BorderStyle = bsDialog
  Caption = 'Load FPGA via JTAG'
  ClientHeight = 103
  ClientWidth = 658
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 168
    Top = 24
    Width = 6
    Height = 13
    Caption = '_'
  end
  object Label2: TLabel
    Left = 168
    Top = 60
    Width = 44
    Height = 13
    Caption = 'Device #'
  end
  object Label3: TLabel
    Left = 276
    Top = 60
    Width = 85
    Height = 13
    Caption = 'in the JTAG chain'
  end
  object Button1: TButton
    Left = 24
    Top = 16
    Width = 117
    Height = 25
    Caption = 'List JTAG devices'
    TabOrder = 0
    OnClick = Button1Click
  end
  object Button2: TButton
    Left = 24
    Top = 56
    Width = 117
    Height = 25
    Caption = 'Load FPGA'
    TabOrder = 1
    OnClick = Button2Click
  end
  object SpinEdit1: TSpinEdit
    Left = 220
    Top = 56
    Width = 45
    Height = 22
    MaxLength = 1
    MaxValue = 4
    MinValue = 0
    TabOrder = 2
    Value = 0
  end
  object OpenDialog1: TOpenDialog
    DefaultExt = 'rbf'
    Filter = 'RBF files (*.rbf)|*.rbf'
    Left = 332
    Top = 16
  end
end
