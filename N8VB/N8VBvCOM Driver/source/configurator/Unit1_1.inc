{ KOL MCK } // Do not remove this line!
const Sizeof_TForm1 = Sizeof(TForm1);
type TControl_ = object( TControl ) end;
type TObj_ = object( TObj ) end;
type _TObj_ = object( _TObj ) end;
const mnuFile = 0;
const mnuFileSave = 1;
const mnuFileExit = 3;
const mnuHelp = 4;
const mnuHelpAbout = 5;


{$IFDEF Pcode}
procedure NewForm1( var Result: PForm1; AParent: PControl );
{$IFDEF Psource}
 PROC(2)
 Load4 ####@@formvmt L(0)
 TObj.Create<2> RESULT
 SetSELF
 DUP C2 Store
 LoadAnsiStr 'VComConfigurator' #0
 C4 
 NewForm<2> RESULT
 DUP C3 AddByte_Store #TForm1.Form
 L(187) L(265) C2 TControl.SetPosition<3> L(382) L(329) C2 TControl.SetClientSize<3>
 DUP StoreVar ####Applet
 LoadSELF C1 
 TControl.Add2AutoFree<2>
 IFDEF(USE_NAMES)
 LoadAnsiStr 'Form1' #0
 C2 TObj.SetName<2>
 DelAnsiStr
 ENDIF
 L(14211288)
 C1 TControl_.SetCtlColor<2>
 L(11) LoadPCharArray #0
')' #0
'About VComConfigurator...' #0
'(' #0
'Help' #0
')' #0
'Exit' #0
'-' #0
'Save Configuration' #0
'(' #0
'File' #0
 L(10)
 L(0) L(0)
 LoadStack L(12) xyAdd
 L(0) LoadSELF AddByte_LoadRef #TForm1.Form NewMenu<3> RESULT DUP LoadSELF AddWord_Store ##TForm1.mnuMain
 LoadSELF Load4 ####TForm1.mnuMainmnuFileSaveMenu
 LoadStack L(0) xySwap L(1)
 C5 TMenu_.AssignEvents<3> DEL DEL
 LoadSELF Load4 ####TForm1.KOLForm1mnuFileExitMenu
 LoadStack L(0) xySwap L(3)
 C5 TMenu_.AssignEvents<3> DEL DEL
 LoadSELF Load4 ####TForm1.KOLForm1mnuHelpAboutMenu
 LoadStack L(0) xySwap L(5)
 C5 TMenu_.AssignEvents<3> DEL DEL
 DEL //mnuMain
 L(11) DELN
 L(2)  C1
 NewPanel<2>
 RESULT
L(5) C1 TControl_.SetAlign<2>
 DUP LoadSELF AddWord_Store ##TForm1.Panel1
 IFDEF(USE_NAMES)
 LoadAnsiStr 'Panel1' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 L(4)
 C1 AddWord_Store ##TControl_.fMargin
 L(1) C1 TControl_.SetTransparent<2>
 DEL //Panel1
 L(2)  C1
 NewPanel<2>
 RESULT
L(2) C1 TControl_.SetAlign<2> L(375) L(0) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.Panel2
 IFDEF(USE_NAMES)
 LoadAnsiStr 'Panel2' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 L(0)
 C1 AddWord_Store ##TControl_.fMargin
 L(1) C1 TControl_.SetTransparent<2>
 L(15780518)
 L(15780518)
 LoadSELF AddWord_LoadRef ##TForm1.Panel2
 NewGradientPanel<3>
 RESULT
L(8) L(64) C2 TControl.SetPosition<3> L(329) L(187) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel5
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel5' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 L(2)
 C1 AddWord_Store ##TControl_.fMargin
 L(1) C1 TControl_.SetHasBorder<2>
 LoadAnsiStr 'Use?' #0 C2
 NewLabel<2>
 RESULT
 xySwap DelAnsiStr L(46) L(79) C2 TControl.SetPosition<3> L(1) C1 TControl.AutoSize<2>
 DUP LoadSELF AddWord_Store ##TForm1.Label1
 IFDEF(USE_NAMES)
 LoadAnsiStr 'Label1' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(1) C1 TControl_.SetTransparent<2>
 DEL //Label1
 LoadAnsiStr 'Virtual Port Pairs' #0
 L(0) xySwap
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewLabelEffect<3>
 RESULT
 xySwap DelAnsiStr L(26) L(49) C2 TControl.SetPosition<3> L(1) C1 TControl.AutoSize<2>
 DUP LoadSELF AddWord_Store ##TForm1.LabelEffect1
 IFDEF(USE_NAMES)
 LoadAnsiStr 'LabelEffect1' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(0)
 C1 TGraphTool.SetColor<2>
 L(16)
 L(4)
 C2 TGraphTool.SetInt<3>
 LoadAnsiStr 'Comic Sans MS' #0
 C2 TGraphTool.SetFontName<2>
 DelAnsiStr
 DEL // Font
 L(0)  C1 TControl_.SetTextAlign<2>
 L(15780518)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetColor2<2>
 DEL //LabelEffect1
 LoadAnsiStr 'N8VBvCOM Driver' #0
 L(1) xySwap
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewLabelEffect<3>
 RESULT
 xySwap DelAnsiStr L(2) L(22) C2 TControl.SetPosition<3> L(1) C1 TControl.AutoSize<2>
 DUP LoadSELF AddWord_Store ##TForm1.LabelEffect6
 IFDEF(USE_NAMES)
 LoadAnsiStr 'LabelEffect6' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(8421376)
 C1 TGraphTool.SetColor<2>
 L(4)
 C1 TGraphTool.SetFontStyle<2>
 L(23)
 L(4)
 C2 TGraphTool.SetInt<3>
 LoadAnsiStr 'Comic Sans MS' #0
 C2 TGraphTool.SetFontName<2>
 DelAnsiStr
 DEL // Font
 L(0)  C1 TControl_.SetTextAlign<2>
 L(15780518)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetColor2<2>
 DEL //LabelEffect6
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(57) L(5) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboLeft1
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboLeft1' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboLeft1
 LoadAnsiStr  #0
 C2
 NewCheckBox<2>
 RESULT
 xySwap DelAnsiStr L(57) L(85) C2 TControl.SetPosition<3> L(0) L(17) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.CheckBox1
 IFDEF(USE_NAMES)
 LoadAnsiStr 'CheckBox1' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(1) C1 TControl_.SetTransparent<2>
 L(15780518)
 C1 TControl_.SetCtlColor<2>
 DEL //CheckBox1
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(57) L(121) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboRight1
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboRight1' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboRight1
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(81) L(5) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboLeft2
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboLeft2' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboLeft2
 LoadAnsiStr  #0
 C2
 NewCheckBox<2>
 RESULT
 xySwap DelAnsiStr L(81) L(85) C2 TControl.SetPosition<3> L(0) L(17) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.CheckBox2
 IFDEF(USE_NAMES)
 LoadAnsiStr 'CheckBox2' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(1) C1 TControl_.SetTransparent<2>
 L(15780518)
 C1 TControl_.SetCtlColor<2>
 DEL //CheckBox2
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(81) L(121) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboRight2
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboRight2' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboRight2
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(105) L(5) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboLeft3
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboLeft3' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboLeft3
 LoadAnsiStr  #0
 C2
 NewCheckBox<2>
 RESULT
 xySwap DelAnsiStr L(105) L(85) C2 TControl.SetPosition<3> L(0) L(17) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.CheckBox3
 IFDEF(USE_NAMES)
 LoadAnsiStr 'CheckBox3' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(1) C1 TControl_.SetTransparent<2>
 L(15780518)
 C1 TControl_.SetCtlColor<2>
 DEL //CheckBox3
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(105) L(121) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboRight3
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboRight3' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboRight3
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(129) L(5) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboLeft4
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboLeft4' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboLeft4
 LoadAnsiStr  #0
 C2
 NewCheckBox<2>
 RESULT
 xySwap DelAnsiStr L(129) L(85) C2 TControl.SetPosition<3> L(0) L(17) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.CheckBox4
 IFDEF(USE_NAMES)
 LoadAnsiStr 'CheckBox4' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(1) C1 TControl_.SetTransparent<2>
 L(15780518)
 C1 TControl_.SetCtlColor<2>
 DEL //CheckBox4
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(129) L(121) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboRight4
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboRight4' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboRight4
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(153) L(5) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboLeft5
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboLeft5' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboLeft5
 LoadAnsiStr  #0
 C2
 NewCheckBox<2>
 RESULT
 xySwap DelAnsiStr L(153) L(85) C2 TControl.SetPosition<3> L(0) L(17) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.CheckBox5
 IFDEF(USE_NAMES)
 LoadAnsiStr 'CheckBox5' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(1) C1 TControl_.SetTransparent<2>
 L(15780518)
 C1 TControl_.SetCtlColor<2>
 DEL //CheckBox5
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(153) L(121) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboRight5
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboRight5' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboRight5
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(66) L(65) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel10
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel10' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel10
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(177) L(5) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboLeft6
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboLeft6' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboLeft6
 LoadAnsiStr  #0
 C2
 NewCheckBox<2>
 RESULT
 xySwap DelAnsiStr L(177) L(85) C2 TControl.SetPosition<3> L(0) L(17) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.CheckBox6
 IFDEF(USE_NAMES)
 LoadAnsiStr 'CheckBox6' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(1) C1 TControl_.SetTransparent<2>
 L(15780518)
 C1 TControl_.SetCtlColor<2>
 DEL //CheckBox6
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(177) L(121) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboRight6
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboRight6' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboRight6
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(201) L(5) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboLeft7
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboLeft7' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboLeft7
 LoadAnsiStr  #0
 C2
 NewCheckBox<2>
 RESULT
 xySwap DelAnsiStr L(201) L(85) C2 TControl.SetPosition<3> L(0) L(17) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.CheckBox7
 IFDEF(USE_NAMES)
 LoadAnsiStr 'CheckBox7' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(1) C1 TControl_.SetTransparent<2>
 L(15780518)
 C1 TControl_.SetCtlColor<2>
 DEL //CheckBox7
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(201) L(121) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboRight7
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboRight7' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboRight7
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(225) L(5) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboLeft8
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboLeft8' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboLeft8
 LoadAnsiStr  #0
 C2
 NewCheckBox<2>
 RESULT
 xySwap DelAnsiStr L(225) L(85) C2 TControl.SetPosition<3> L(0) L(17) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.CheckBox8
 IFDEF(USE_NAMES)
 LoadAnsiStr 'CheckBox8' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(1) C1 TControl_.SetTransparent<2>
 L(15780518)
 C1 TControl_.SetCtlColor<2>
 DEL //CheckBox8
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(225) L(121) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboRight8
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboRight8' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboRight8
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(249) L(5) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboLeft9
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboLeft9' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboLeft9
 LoadAnsiStr  #0
 C2
 NewCheckBox<2>
 RESULT
 xySwap DelAnsiStr L(249) L(85) C2 TControl.SetPosition<3> L(0) L(17) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.CheckBox9
 IFDEF(USE_NAMES)
 LoadAnsiStr 'CheckBox9' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(1) C1 TControl_.SetTransparent<2>
 L(15780518)
 C1 TControl_.SetCtlColor<2>
 DEL //CheckBox9
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(249) L(121) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboRight9
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboRight9' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboRight9
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(273) L(5) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboLeft10
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboLeft10' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboLeft10
 LoadAnsiStr  #0
 C2
 NewCheckBox<2>
 RESULT
 xySwap DelAnsiStr L(273) L(85) C2 TControl.SetPosition<3> L(0) L(17) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.CheckBox10
 IFDEF(USE_NAMES)
 LoadAnsiStr 'CheckBox10' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(1) C1 TControl_.SetTransparent<2>
 L(15780518)
 C1 TControl_.SetCtlColor<2>
 DEL //CheckBox10
 L(17)
 C1
 NewComboBox<2>
 RESULT
L(273) L(121) C2 TControl.SetPosition<3> L(0) L(61) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.cboRight10
 IFDEF(USE_NAMES)
 LoadAnsiStr 'cboRight10' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0) C1 TControl_.SetCtl3D<2>
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 L(-2147483643)
 C1 TControl_.SetCtlColor<2>
 L(0) C1 TControl_.SetCurIndex<2>
 DEL //cboRight10
 LoadAnsiStr 'Save Configuration' #0
 C2
 NewButton<2>
 RESULT
L(301) L(46) C2 TControl.SetPosition<3> L(0) L(105) C2 TControl.SetSize<3> xySwap DelAnsiStr
 DUP LoadSELF AddWord_Store ##TForm1.btnSave
 IFDEF(USE_NAMES)
 LoadAnsiStr 'btnSave' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 DEL //btnSave
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(66) L(100) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel1
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel1' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel1
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(162) L(100) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel11
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel11' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel11
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(186) L(65) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel12
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel12' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel12
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(186) L(100) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel13
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel13' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel13
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(210) L(65) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel14
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel14' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel14
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(210) L(100) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel15
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel15' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel15
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(234) L(65) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel16
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel16' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel16
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(234) L(100) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel17
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel17' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel17
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(258) L(65) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel18
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel18' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel18
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(258) L(100) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel19
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel19' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel19
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(90) L(65) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel2
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel2' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel2
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(282) L(65) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel20
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel20' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel20
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(282) L(100) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel21
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel21' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel21
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(90) L(100) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel3
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel3' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel3
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(114) L(65) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel4
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel4' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel4
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(114) L(100) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel6
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel6' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel6
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(138) L(65) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel7
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel7' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel7
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(138) L(100) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel8
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel8' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel8
 L(8388608)
 L(8388608)
 LoadSELF AddWord_LoadRef ##TForm1.GradientPanel5
 NewGradientPanel<3>
 RESULT
L(162) L(65) C2 TControl.SetPosition<3> L(4) L(22) C2 TControl.SetSize<3>
 DUP LoadSELF AddWord_Store ##TForm1.GradientPanel9
 IFDEF(USE_NAMES)
 LoadAnsiStr 'GradientPanel9' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 L(0)
 C1 TControl_.SetHasBorder<2>
 L(0) C1 AddWord_Store ##TControl_.fTabStop
 DEL //GradientPanel9
 DEL //GradientPanel5
 LoadAnsiStr 'Install Driver' #0
 C2
 NewButton<2>
 RESULT
L(349) L(12) C2 TControl.SetPosition<3> L(0) L(91) C2 TControl.SetSize<3> xySwap DelAnsiStr
 DUP LoadSELF AddWord_Store ##TForm1.Button1
 IFDEF(USE_NAMES)
 LoadAnsiStr 'Button1' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 DEL //Button1
 LoadAnsiStr 'Update Driver' #0
 C2
 NewButton<2>
 RESULT
L(349) L(116) C2 TControl.SetPosition<3> L(0) L(91) C2 TControl.SetSize<3> xySwap DelAnsiStr
 DUP LoadSELF AddWord_Store ##TForm1.Button2
 IFDEF(USE_NAMES)
 LoadAnsiStr 'Button2' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 DEL //Button2
 LoadAnsiStr 'Remove Driver' #0
 C2
 NewButton<2>
 RESULT
L(349) L(220) C2 TControl.SetPosition<3> L(0) L(91) C2 TControl.SetSize<3> xySwap DelAnsiStr
 DUP LoadSELF AddWord_Store ##TForm1.Button3
 IFDEF(USE_NAMES)
 LoadAnsiStr 'Button3' #0
 C2 TObj_.SetName<2>
 DelAnsiStr
 ENDIF
 DUP TControl_.GetFont<1> RESULT
 L(10)
 L(4)
 C2 TGraphTool.SetInt<3>
 DEL // Font
 DEL //Button3
 DEL //Panel2
 LoadSELF AddWord_LoadRef ##TForm1.cboLeft1
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboLeft1
 LoadSELF AddWord_LoadRef ##TForm1.cboLeft2
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboLeft2
 LoadSELF AddWord_LoadRef ##TForm1.cboLeft3
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboLeft3
 LoadSELF AddWord_LoadRef ##TForm1.cboLeft4
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboLeft4
 LoadSELF AddWord_LoadRef ##TForm1.cboRight1
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboRight1
 LoadSELF AddWord_LoadRef ##TForm1.cboRight2
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboRight2
 LoadSELF AddWord_LoadRef ##TForm1.cboRight3
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboRight3
 LoadSELF AddWord_LoadRef ##TForm1.cboRight4
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboRight4
 LoadSELF AddWord_LoadRef ##TForm1.btnSave
 Load4 ####TForm1.btnSaveClick
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //btnSave
 LoadSELF AddWord_LoadRef ##TForm1.CheckBox1
 Load4 ####TForm1.chkBoxClick
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //CheckBox1
 LoadSELF AddWord_LoadRef ##TForm1.CheckBox2
 Load4 ####TForm1.chkBoxClick
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //CheckBox2
 LoadSELF AddWord_LoadRef ##TForm1.CheckBox3
 Load4 ####TForm1.chkBoxClick
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //CheckBox3
 LoadSELF AddWord_LoadRef ##TForm1.CheckBox4
 Load4 ####TForm1.chkBoxClick
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //CheckBox4
 LoadSELF AddWord_LoadRef ##TForm1.CheckBox5
 Load4 ####TForm1.chkBoxClick
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //CheckBox5
 LoadSELF AddWord_LoadRef ##TForm1.cboLeft5
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboLeft5
 LoadSELF AddWord_LoadRef ##TForm1.cboRight5
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboRight5
 LoadSELF AddWord_LoadRef ##TForm1.cboLeft7
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboLeft7
 LoadSELF AddWord_LoadRef ##TForm1.cboLeft8
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboLeft8
 LoadSELF AddWord_LoadRef ##TForm1.cboLeft9
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboLeft9
 LoadSELF AddWord_LoadRef ##TForm1.cboRight6
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboRight6
 LoadSELF AddWord_LoadRef ##TForm1.cboRight7
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboRight7
 LoadSELF AddWord_LoadRef ##TForm1.cboRight8
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboRight8
 LoadSELF AddWord_LoadRef ##TForm1.cboRight9
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboRight9
 LoadSELF AddWord_LoadRef ##TForm1.CheckBox6
 Load4 ####TForm1.chkBoxClick
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //CheckBox6
 LoadSELF AddWord_LoadRef ##TForm1.CheckBox7
 Load4 ####TForm1.chkBoxClick
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //CheckBox7
 LoadSELF AddWord_LoadRef ##TForm1.CheckBox8
 Load4 ####TForm1.chkBoxClick
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //CheckBox8
 LoadSELF AddWord_LoadRef ##TForm1.CheckBox9
 Load4 ####TForm1.chkBoxClick
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //CheckBox9
 LoadSELF AddWord_LoadRef ##TForm1.CheckBox10
 Load4 ####TForm1.chkBoxClick
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //CheckBox10
 LoadSELF AddWord_LoadRef ##TForm1.cboLeft10
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboLeft10
 LoadSELF AddWord_LoadRef ##TForm1.cboRight10
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboRight10
 LoadSELF AddWord_LoadRef ##TForm1.Button1
 Load4 ####TForm1.Button1Click
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //Button1
 LoadSELF AddWord_LoadRef ##TForm1.Button2
 Load4 ####TForm1.Button2Click
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //Button2
 LoadSELF AddWord_LoadRef ##TForm1.Button3
 Load4 ####TForm1.Button3Click
 C1 AddWord_Store ##TControl_.fOnClick
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnClick)
 DEL //Button3
 LoadSELF AddWord_LoadRef ##TForm1.cboLeft6
 Load4 ####TForm1.cboPortsChange
 C1 AddWord_Store ##TControl_.fOnChange
 LoadSELF C1 AddWord_Store ##(4+TControl_.fOnChange)
 DEL //cboLeft6
 LoadSELF DUP TForm1.KOLForm1FormCreate<2>
 DEL DelAnsiStr DEL DEL DEL EXIT
           ####Sizeof_TForm1
           ####0
@@formvmt: ####_TObj_.Init
           ####TObj.Destroy
 ENDP
{$ENDIF Psource}
{$ELSE OldCode}
procedure NewForm1( var Result: PForm1; AParent: PControl );
begin

  {$IFDEF KOLCLASSES}
  Result := PForm1.Create;
  {$ELSE OBJECTS}
  New( Result, Create );
  {$ENDIF KOL CLASSES/OBJECTS}
  Result.Form := NewForm( AParent, 'VComConfigurator' ).SetPosition( 265, 187 ){MainMenuHeight=20}.SetClientSize( 329, 402 );
  Applet :=  Result.Form;
  Result.Form.Add2AutoFree( Result );
   {$IFDEF USE_NAMES}
    Result.Form.Name := 'Form1';
   {$ENDIF}
    Result.Form.Color := $D8D8D8;

    Result.mnuMain := NewMenu( Result.Form, 0, [ 'File', '('
      , 'Save Configuration', '-' + '', 'Exit', ')', 'Help', '('
      , 'About VComConfigurator...', ')', '' ], nil );
   {$IFDEF USE_NAMES}
    Result.mnuMain.Name := 'mnuMain';
   {$ENDIF}
    Result.mnuMain.AssignEvents( 1, [ Result.mnuMainmnuFileSaveMenu ] );
    Result.mnuMain.AssignEvents( 3, [ Result.KOLForm1mnuFileExitMenu ] );
    Result.mnuMain.AssignEvents( 5, [ Result.KOLForm1mnuHelpAboutMenu ] );
    // Result.Panel1.TabOrder = 0
    Result.Panel1 := NewPanel( Result.Form, esNone ).SetAlign ( caClient );
   {$IFDEF USE_NAMES}
    Result.Panel1.Name := 'Panel1';
   {$ENDIF}
    Result.Panel1.Ctl3D := False;
    Result.Panel1.Border := 4;
    Result.Panel1.Transparent := True;
    // Result.Panel2.TabOrder = 1
    Result.Panel2 := NewPanel( Result.Form, esNone ).SetAlign ( caTop ).SetSize( 0, 375 );
   {$IFDEF USE_NAMES}
    Result.Panel2.Name := 'Panel2';
   {$ENDIF}
    Result.Panel2.Ctl3D := False;
    Result.Panel2.Border := 0;
    Result.Panel2.Transparent := True;
    // Result.GradientPanel5.TabOrder = 0
      Result.GradientPanel5 := NewGradientPanel( Result.Panel2, $F0CAA6, $F0CAA6 ).SetPosition( 64, 8 ).SetSize( 187, 329 );
   {$IFDEF USE_NAMES}
      Result.GradientPanel5.Name := 'GradientPanel5';
   {$ENDIF}
    Result.GradientPanel5.TabStop := FALSE;
      Result.GradientPanel5.Border := 2;
      Result.GradientPanel5.HasBorder := TRUE;
    // Result.Label1.TabOrder = -1
        Result.Label1 := NewLabel( Result.GradientPanel5, 'Use?' ).SetPosition( 79, 46 ).AutoSize( TRUE );
   {$IFDEF USE_NAMES}
        Result.Label1.Name := 'Label1';
   {$ENDIF}
    Result.Label1.Font.FontHeight := 10;
        Result.Label1.Transparent := True;
    // Result.LabelEffect1.TabOrder = -1
        Result.LabelEffect1 := NewLabelEffect( Result.GradientPanel5, 'Virtual Port Pairs', 0 ).SetPosition( 49, 26 ).AutoSize( TRUE );
   {$IFDEF USE_NAMES}
        Result.LabelEffect1.Name := 'LabelEffect1';
   {$ENDIF}
        Result.LabelEffect1.Ctl3D := False;
    Result.LabelEffect1.Font.Color := clBlack;
    Result.LabelEffect1.Font.FontHeight := 16;
    Result.LabelEffect1.Font.FontName := 'Comic Sans MS';
    Result.LabelEffect1.TextAlign := taLeft;
    Result.LabelEffect1.Color := $F0CAA6;
        Result.LabelEffect1.Color2 := clBlack;
    // Result.LabelEffect6.TabOrder = -1
        Result.LabelEffect6 := NewLabelEffect( Result.GradientPanel5, 'N8VBvCOM Driver', 1 ).SetPosition( 22, 2 ).AutoSize( TRUE );
   {$IFDEF USE_NAMES}
        Result.LabelEffect6.Name := 'LabelEffect6';
   {$ENDIF}
        Result.LabelEffect6.Ctl3D := False;
    Result.LabelEffect6.Font.Color := clTeal;
    Result.LabelEffect6.Font.FontStyle := [ fsUnderline ];
    Result.LabelEffect6.Font.FontHeight := 23;
    Result.LabelEffect6.Font.FontName := 'Comic Sans MS';
    Result.LabelEffect6.TextAlign := taLeft;
    Result.LabelEffect6.Color := $F0CAA6;
        Result.LabelEffect6.Color2 := clBlack;
    // Result.cboLeft1.TabOrder = 0
        Result.cboLeft1 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 5, 57 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboLeft1.Name := 'cboLeft1';
   {$ENDIF}
        Result.cboLeft1.Ctl3D := False;
    Result.cboLeft1.Font.FontHeight := 10;
    Result.cboLeft1.Color := clWindow;
        Result.cboLeft1.CurIndex := 0;
    // Result.CheckBox1.TabOrder = 1
        Result.CheckBox1 := NewCheckBox( Result.GradientPanel5, '' ).SetPosition( 85, 57 ).SetSize( 17, 0 );
   {$IFDEF USE_NAMES}
        Result.CheckBox1.Name := 'CheckBox1';
   {$ENDIF}
        Result.CheckBox1.Transparent := True;
    Result.CheckBox1.Color := $F0CAA6;
    // Result.cboRight1.TabOrder = 2
        Result.cboRight1 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 121, 57 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboRight1.Name := 'cboRight1';
   {$ENDIF}
        Result.cboRight1.Ctl3D := False;
    Result.cboRight1.Font.FontHeight := 10;
    Result.cboRight1.Color := clWindow;
        Result.cboRight1.CurIndex := 0;
    // Result.cboLeft2.TabOrder = 3
        Result.cboLeft2 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 5, 81 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboLeft2.Name := 'cboLeft2';
   {$ENDIF}
        Result.cboLeft2.Ctl3D := False;
    Result.cboLeft2.Font.FontHeight := 10;
    Result.cboLeft2.Color := clWindow;
        Result.cboLeft2.CurIndex := 0;
    // Result.CheckBox2.TabOrder = 4
        Result.CheckBox2 := NewCheckBox( Result.GradientPanel5, '' ).SetPosition( 85, 81 ).SetSize( 17, 0 );
   {$IFDEF USE_NAMES}
        Result.CheckBox2.Name := 'CheckBox2';
   {$ENDIF}
        Result.CheckBox2.Transparent := True;
    Result.CheckBox2.Color := $F0CAA6;
    // Result.cboRight2.TabOrder = 5
        Result.cboRight2 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 121, 81 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboRight2.Name := 'cboRight2';
   {$ENDIF}
        Result.cboRight2.Ctl3D := False;
    Result.cboRight2.Font.FontHeight := 10;
    Result.cboRight2.Color := clWindow;
        Result.cboRight2.CurIndex := 0;
    // Result.cboLeft3.TabOrder = 6
        Result.cboLeft3 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 5, 105 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboLeft3.Name := 'cboLeft3';
   {$ENDIF}
        Result.cboLeft3.Ctl3D := False;
    Result.cboLeft3.Font.FontHeight := 10;
    Result.cboLeft3.Color := clWindow;
        Result.cboLeft3.CurIndex := 0;
    // Result.CheckBox3.TabOrder = 7
        Result.CheckBox3 := NewCheckBox( Result.GradientPanel5, '' ).SetPosition( 85, 105 ).SetSize( 17, 0 );
   {$IFDEF USE_NAMES}
        Result.CheckBox3.Name := 'CheckBox3';
   {$ENDIF}
        Result.CheckBox3.Transparent := True;
    Result.CheckBox3.Color := $F0CAA6;
    // Result.cboRight3.TabOrder = 8
        Result.cboRight3 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 121, 105 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboRight3.Name := 'cboRight3';
   {$ENDIF}
        Result.cboRight3.Ctl3D := False;
    Result.cboRight3.Font.FontHeight := 10;
    Result.cboRight3.Color := clWindow;
        Result.cboRight3.CurIndex := 0;
    // Result.cboLeft4.TabOrder = 9
        Result.cboLeft4 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 5, 129 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboLeft4.Name := 'cboLeft4';
   {$ENDIF}
        Result.cboLeft4.Ctl3D := False;
    Result.cboLeft4.Font.FontHeight := 10;
    Result.cboLeft4.Color := clWindow;
        Result.cboLeft4.CurIndex := 0;
    // Result.CheckBox4.TabOrder = 10
        Result.CheckBox4 := NewCheckBox( Result.GradientPanel5, '' ).SetPosition( 85, 129 ).SetSize( 17, 0 );
   {$IFDEF USE_NAMES}
        Result.CheckBox4.Name := 'CheckBox4';
   {$ENDIF}
        Result.CheckBox4.Transparent := True;
    Result.CheckBox4.Color := $F0CAA6;
    // Result.cboRight4.TabOrder = 11
        Result.cboRight4 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 121, 129 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboRight4.Name := 'cboRight4';
   {$ENDIF}
        Result.cboRight4.Ctl3D := False;
    Result.cboRight4.Font.FontHeight := 10;
    Result.cboRight4.Color := clWindow;
        Result.cboRight4.CurIndex := 0;
    // Result.cboLeft5.TabOrder = 12
        Result.cboLeft5 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 5, 153 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboLeft5.Name := 'cboLeft5';
   {$ENDIF}
        Result.cboLeft5.Ctl3D := False;
    Result.cboLeft5.Font.FontHeight := 10;
    Result.cboLeft5.Color := clWindow;
        Result.cboLeft5.CurIndex := 0;
    // Result.CheckBox5.TabOrder = 13
        Result.CheckBox5 := NewCheckBox( Result.GradientPanel5, '' ).SetPosition( 85, 153 ).SetSize( 17, 0 );
   {$IFDEF USE_NAMES}
        Result.CheckBox5.Name := 'CheckBox5';
   {$ENDIF}
        Result.CheckBox5.Transparent := True;
    Result.CheckBox5.Color := $F0CAA6;
    // Result.cboRight5.TabOrder = 14
        Result.cboRight5 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 121, 153 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboRight5.Name := 'cboRight5';
   {$ENDIF}
        Result.cboRight5.Ctl3D := False;
    Result.cboRight5.Font.FontHeight := 10;
    Result.cboRight5.Color := clWindow;
        Result.cboRight5.CurIndex := 0;
    // Result.GradientPanel10.TabOrder = 15
        Result.GradientPanel10 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 65, 66 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel10.Name := 'GradientPanel10';
   {$ENDIF}
        Result.GradientPanel10.HasBorder := FALSE;
    Result.GradientPanel10.TabStop := FALSE;
    // Result.cboLeft6.TabOrder = 16
        Result.cboLeft6 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 5, 177 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboLeft6.Name := 'cboLeft6';
   {$ENDIF}
        Result.cboLeft6.Ctl3D := False;
    Result.cboLeft6.Font.FontHeight := 10;
    Result.cboLeft6.Color := clWindow;
        Result.cboLeft6.CurIndex := 0;
    // Result.CheckBox6.TabOrder = 17
        Result.CheckBox6 := NewCheckBox( Result.GradientPanel5, '' ).SetPosition( 85, 177 ).SetSize( 17, 0 );
   {$IFDEF USE_NAMES}
        Result.CheckBox6.Name := 'CheckBox6';
   {$ENDIF}
        Result.CheckBox6.Transparent := True;
    Result.CheckBox6.Color := $F0CAA6;
    // Result.cboRight6.TabOrder = 18
        Result.cboRight6 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 121, 177 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboRight6.Name := 'cboRight6';
   {$ENDIF}
        Result.cboRight6.Ctl3D := False;
    Result.cboRight6.Font.FontHeight := 10;
    Result.cboRight6.Color := clWindow;
        Result.cboRight6.CurIndex := 0;
    // Result.cboLeft7.TabOrder = 19
        Result.cboLeft7 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 5, 201 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboLeft7.Name := 'cboLeft7';
   {$ENDIF}
        Result.cboLeft7.Ctl3D := False;
    Result.cboLeft7.Font.FontHeight := 10;
    Result.cboLeft7.Color := clWindow;
        Result.cboLeft7.CurIndex := 0;
    // Result.CheckBox7.TabOrder = 20
        Result.CheckBox7 := NewCheckBox( Result.GradientPanel5, '' ).SetPosition( 85, 201 ).SetSize( 17, 0 );
   {$IFDEF USE_NAMES}
        Result.CheckBox7.Name := 'CheckBox7';
   {$ENDIF}
        Result.CheckBox7.Transparent := True;
    Result.CheckBox7.Color := $F0CAA6;
    // Result.cboRight7.TabOrder = 21
        Result.cboRight7 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 121, 201 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboRight7.Name := 'cboRight7';
   {$ENDIF}
        Result.cboRight7.Ctl3D := False;
    Result.cboRight7.Font.FontHeight := 10;
    Result.cboRight7.Color := clWindow;
        Result.cboRight7.CurIndex := 0;
    // Result.cboLeft8.TabOrder = 22
        Result.cboLeft8 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 5, 225 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboLeft8.Name := 'cboLeft8';
   {$ENDIF}
        Result.cboLeft8.Ctl3D := False;
    Result.cboLeft8.Font.FontHeight := 10;
    Result.cboLeft8.Color := clWindow;
        Result.cboLeft8.CurIndex := 0;
    // Result.CheckBox8.TabOrder = 23
        Result.CheckBox8 := NewCheckBox( Result.GradientPanel5, '' ).SetPosition( 85, 225 ).SetSize( 17, 0 );
   {$IFDEF USE_NAMES}
        Result.CheckBox8.Name := 'CheckBox8';
   {$ENDIF}
        Result.CheckBox8.Transparent := True;
    Result.CheckBox8.Color := $F0CAA6;
    // Result.cboRight8.TabOrder = 24
        Result.cboRight8 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 121, 225 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboRight8.Name := 'cboRight8';
   {$ENDIF}
        Result.cboRight8.Ctl3D := False;
    Result.cboRight8.Font.FontHeight := 10;
    Result.cboRight8.Color := clWindow;
        Result.cboRight8.CurIndex := 0;
    // Result.cboLeft9.TabOrder = 25
        Result.cboLeft9 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 5, 249 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboLeft9.Name := 'cboLeft9';
   {$ENDIF}
        Result.cboLeft9.Ctl3D := False;
    Result.cboLeft9.Font.FontHeight := 10;
    Result.cboLeft9.Color := clWindow;
        Result.cboLeft9.CurIndex := 0;
    // Result.CheckBox9.TabOrder = 26
        Result.CheckBox9 := NewCheckBox( Result.GradientPanel5, '' ).SetPosition( 85, 249 ).SetSize( 17, 0 );
   {$IFDEF USE_NAMES}
        Result.CheckBox9.Name := 'CheckBox9';
   {$ENDIF}
        Result.CheckBox9.Transparent := True;
    Result.CheckBox9.Color := $F0CAA6;
    // Result.cboRight9.TabOrder = 27
        Result.cboRight9 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 121, 249 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboRight9.Name := 'cboRight9';
   {$ENDIF}
        Result.cboRight9.Ctl3D := False;
    Result.cboRight9.Font.FontHeight := 10;
    Result.cboRight9.Color := clWindow;
        Result.cboRight9.CurIndex := 0;
    // Result.cboLeft10.TabOrder = 28
        Result.cboLeft10 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 5, 273 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboLeft10.Name := 'cboLeft10';
   {$ENDIF}
        Result.cboLeft10.Ctl3D := False;
    Result.cboLeft10.Font.FontHeight := 10;
    Result.cboLeft10.Color := clWindow;
        Result.cboLeft10.CurIndex := 0;
    // Result.CheckBox10.TabOrder = 29
        Result.CheckBox10 := NewCheckBox( Result.GradientPanel5, '' ).SetPosition( 85, 273 ).SetSize( 17, 0 );
   {$IFDEF USE_NAMES}
        Result.CheckBox10.Name := 'CheckBox10';
   {$ENDIF}
        Result.CheckBox10.Transparent := True;
    Result.CheckBox10.Color := $F0CAA6;
    // Result.cboRight10.TabOrder = 30
        Result.cboRight10 := NewComboBox( Result.GradientPanel5, [ coReadOnly, coNoIntegralHeight ] ).SetPosition( 121, 273 ).SetSize( 61, 0 );
   {$IFDEF USE_NAMES}
        Result.cboRight10.Name := 'cboRight10';
   {$ENDIF}
        Result.cboRight10.Ctl3D := False;
    Result.cboRight10.Font.FontHeight := 10;
    Result.cboRight10.Color := clWindow;
        Result.cboRight10.CurIndex := 0;
    // Result.btnSave.TabOrder = 31
        Result.btnSave := NewButton( Result.GradientPanel5, 'Save Configuration' ).SetPosition( 46, 301 ).SetSize( 105, 0 );
   {$IFDEF USE_NAMES}
        Result.btnSave.Name := 'btnSave';
   {$ENDIF}
    Result.btnSave.Font.FontHeight := 10;
    // Result.GradientPanel1.TabOrder = 32
        Result.GradientPanel1 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 100, 66 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel1.Name := 'GradientPanel1';
   {$ENDIF}
        Result.GradientPanel1.HasBorder := FALSE;
    Result.GradientPanel1.TabStop := FALSE;
    // Result.GradientPanel11.TabOrder = 32
        Result.GradientPanel11 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 100, 162 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel11.Name := 'GradientPanel11';
   {$ENDIF}
        Result.GradientPanel11.HasBorder := FALSE;
    Result.GradientPanel11.TabStop := FALSE;
    // Result.GradientPanel12.TabOrder = 32
        Result.GradientPanel12 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 65, 186 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel12.Name := 'GradientPanel12';
   {$ENDIF}
        Result.GradientPanel12.HasBorder := FALSE;
    Result.GradientPanel12.TabStop := FALSE;
    // Result.GradientPanel13.TabOrder = 32
        Result.GradientPanel13 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 100, 186 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel13.Name := 'GradientPanel13';
   {$ENDIF}
        Result.GradientPanel13.HasBorder := FALSE;
    Result.GradientPanel13.TabStop := FALSE;
    // Result.GradientPanel14.TabOrder = 32
        Result.GradientPanel14 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 65, 210 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel14.Name := 'GradientPanel14';
   {$ENDIF}
        Result.GradientPanel14.HasBorder := FALSE;
    Result.GradientPanel14.TabStop := FALSE;
    // Result.GradientPanel15.TabOrder = 32
        Result.GradientPanel15 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 100, 210 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel15.Name := 'GradientPanel15';
   {$ENDIF}
        Result.GradientPanel15.HasBorder := FALSE;
    Result.GradientPanel15.TabStop := FALSE;
    // Result.GradientPanel16.TabOrder = 32
        Result.GradientPanel16 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 65, 234 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel16.Name := 'GradientPanel16';
   {$ENDIF}
        Result.GradientPanel16.HasBorder := FALSE;
    Result.GradientPanel16.TabStop := FALSE;
    // Result.GradientPanel17.TabOrder = 32
        Result.GradientPanel17 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 100, 234 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel17.Name := 'GradientPanel17';
   {$ENDIF}
        Result.GradientPanel17.HasBorder := FALSE;
    Result.GradientPanel17.TabStop := FALSE;
    // Result.GradientPanel18.TabOrder = 32
        Result.GradientPanel18 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 65, 258 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel18.Name := 'GradientPanel18';
   {$ENDIF}
        Result.GradientPanel18.HasBorder := FALSE;
    Result.GradientPanel18.TabStop := FALSE;
    // Result.GradientPanel19.TabOrder = 32
        Result.GradientPanel19 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 100, 258 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel19.Name := 'GradientPanel19';
   {$ENDIF}
        Result.GradientPanel19.HasBorder := FALSE;
    Result.GradientPanel19.TabStop := FALSE;
    // Result.GradientPanel2.TabOrder = 32
        Result.GradientPanel2 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 65, 90 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel2.Name := 'GradientPanel2';
   {$ENDIF}
        Result.GradientPanel2.HasBorder := FALSE;
    Result.GradientPanel2.TabStop := FALSE;
    // Result.GradientPanel20.TabOrder = 32
        Result.GradientPanel20 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 65, 282 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel20.Name := 'GradientPanel20';
   {$ENDIF}
        Result.GradientPanel20.HasBorder := FALSE;
    Result.GradientPanel20.TabStop := FALSE;
    // Result.GradientPanel21.TabOrder = 32
        Result.GradientPanel21 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 100, 282 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel21.Name := 'GradientPanel21';
   {$ENDIF}
        Result.GradientPanel21.HasBorder := FALSE;
    Result.GradientPanel21.TabStop := FALSE;
    // Result.GradientPanel3.TabOrder = 32
        Result.GradientPanel3 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 100, 90 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel3.Name := 'GradientPanel3';
   {$ENDIF}
        Result.GradientPanel3.HasBorder := FALSE;
    Result.GradientPanel3.TabStop := FALSE;
    // Result.GradientPanel4.TabOrder = 32
        Result.GradientPanel4 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 65, 114 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel4.Name := 'GradientPanel4';
   {$ENDIF}
        Result.GradientPanel4.HasBorder := FALSE;
    Result.GradientPanel4.TabStop := FALSE;
    // Result.GradientPanel6.TabOrder = 32
        Result.GradientPanel6 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 100, 114 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel6.Name := 'GradientPanel6';
   {$ENDIF}
        Result.GradientPanel6.HasBorder := FALSE;
    Result.GradientPanel6.TabStop := FALSE;
    // Result.GradientPanel7.TabOrder = 32
        Result.GradientPanel7 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 65, 138 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel7.Name := 'GradientPanel7';
   {$ENDIF}
        Result.GradientPanel7.HasBorder := FALSE;
    Result.GradientPanel7.TabStop := FALSE;
    // Result.GradientPanel8.TabOrder = 32
        Result.GradientPanel8 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 100, 138 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel8.Name := 'GradientPanel8';
   {$ENDIF}
        Result.GradientPanel8.HasBorder := FALSE;
    Result.GradientPanel8.TabStop := FALSE;
    // Result.GradientPanel9.TabOrder = 32
        Result.GradientPanel9 := NewGradientPanel( Result.GradientPanel5, clNavy, clNavy ).SetPosition( 65, 162 ).SetSize( 22, 4 );
   {$IFDEF USE_NAMES}
        Result.GradientPanel9.Name := 'GradientPanel9';
   {$ENDIF}
        Result.GradientPanel9.HasBorder := FALSE;
    Result.GradientPanel9.TabStop := FALSE;
    // Result.Button1.TabOrder = 1
      Result.Button1 := NewButton( Result.Panel2, 'Install Driver' ).SetPosition( 12, 349 ).SetSize( 91, 0 );
   {$IFDEF USE_NAMES}
      Result.Button1.Name := 'Button1';
   {$ENDIF}
    Result.Button1.Font.FontHeight := 10;
    // Result.Button2.TabOrder = 2
      Result.Button2 := NewButton( Result.Panel2, 'Update Driver' ).SetPosition( 116, 349 ).SetSize( 91, 0 );
   {$IFDEF USE_NAMES}
      Result.Button2.Name := 'Button2';
   {$ENDIF}
    Result.Button2.Font.FontHeight := 10;
    // Result.Button3.TabOrder = 3
      Result.Button3 := NewButton( Result.Panel2, 'Remove Driver' ).SetPosition( 220, 349 ).SetSize( 91, 0 );
   {$IFDEF USE_NAMES}
      Result.Button3.Name := 'Button3';
   {$ENDIF}
    Result.Button3.Font.FontHeight := 10;
      Result.cboLeft1.OnChange := Result.cboPortsChange;
      Result.cboLeft2.OnChange := Result.cboPortsChange;
      Result.cboLeft3.OnChange := Result.cboPortsChange;
      Result.cboLeft4.OnChange := Result.cboPortsChange;
      Result.cboRight1.OnChange := Result.cboPortsChange;
      Result.cboRight2.OnChange := Result.cboPortsChange;
      Result.cboRight3.OnChange := Result.cboPortsChange;
      Result.cboRight4.OnChange := Result.cboPortsChange;
      Result.btnSave.OnClick := Result.btnSaveClick;
      Result.CheckBox1.OnClick := Result.chkBoxClick;
      Result.CheckBox2.OnClick := Result.chkBoxClick;
      Result.CheckBox3.OnClick := Result.chkBoxClick;
      Result.CheckBox4.OnClick := Result.chkBoxClick;
      Result.CheckBox5.OnClick := Result.chkBoxClick;
      Result.cboLeft5.OnChange := Result.cboPortsChange;
      Result.cboRight5.OnChange := Result.cboPortsChange;
      Result.cboLeft7.OnChange := Result.cboPortsChange;
      Result.cboLeft8.OnChange := Result.cboPortsChange;
      Result.cboLeft9.OnChange := Result.cboPortsChange;
      Result.cboRight6.OnChange := Result.cboPortsChange;
      Result.cboRight7.OnChange := Result.cboPortsChange;
      Result.cboRight8.OnChange := Result.cboPortsChange;
      Result.cboRight9.OnChange := Result.cboPortsChange;
      Result.CheckBox6.OnClick := Result.chkBoxClick;
      Result.CheckBox7.OnClick := Result.chkBoxClick;
      Result.CheckBox8.OnClick := Result.chkBoxClick;
      Result.CheckBox9.OnClick := Result.chkBoxClick;
      Result.CheckBox10.OnClick := Result.chkBoxClick;
      Result.cboLeft10.OnChange := Result.cboPortsChange;
      Result.cboRight10.OnChange := Result.cboPortsChange;
      Result.Button1.OnClick := Result.Button1Click;
      Result.Button2.OnClick := Result.Button2Click;
      Result.Button3.OnClick := Result.Button3Click;
      Result.cboLeft6.OnChange := Result.cboPortsChange;
    Result.KOLForm1FormCreate( Result );

end;
{$ENDIF OldCode}

