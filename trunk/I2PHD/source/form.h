/*
---- Begin Copyright Notice

     This is part of the interfacing DLL between Winrad and the HPSDR, based on WinUSB
     Copyright (C) 2009  Alberto di Bene,  I2PHD

     This library is free software; you can redistribute it and/or
     modify it under the terms of the GNU Lesser General Public
     License as published by the Free Software Foundation; either
     version 2.1 of the License, or (at your option) any later version.

     This library is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
     Lesser General Public License for more details.

     You should have received a copy of the GNU Lesser General Public
     License along with this library; if not, write to the Free Software
     Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, 
     MA  02110-1301  USA


--- End Copyright Notice
*/
//---------------------------------------------------------------------------

#ifndef formH
#define formH
//---------------------------------------------------------------------------
#include <Classes.hpp>
#include <Controls.hpp>
#include <StdCtrls.hpp>
#include <Forms.hpp>
#include "DrLabel.hpp"
#include <ExtCtrls.hpp>
#include <Graphics.hpp>
#include <inifiles.hpp>

#include "worker.h"
#include "wusbfunc.h"
#include "AutoLed.hpp"


#ifdef _DEBUG
  inline void __fastcall OutputDebug(AnsiString Message)
  {
    OutputDebugString(Message.c_str());
  }
#else
  #define AdB_dslash(s) s##s
  #define AdB_comment AdB_dslash(/)
  #define OutputDebug(p) AdB_comment
#endif

#define    ININAME            "ExtIO_HPSDR.ini"
#define    SectionParameters  "Parameters"
#define    KeyClock           "10MHzClock"
#define    KeyMClock          "MasterClock"
#define    KeySrate           "SamplingRate"
#define    KeyPreamp          "Preamplifier"
#define    KeyDither          "ADC_Dither"
#define    KeyRandom          "ADC_Random"
#define    KeyCardM           "Mercury_Installed"
#define    KeyCardP           "Penelope_Installed"
#define    KeyDACOut          "DAC_Output"
#define    KeyCardOut         "Card_Output"

//----------------------------------------------------------------------
class TVRing
{
private:
   char *ring;
   int  Top, Que, headroom, size, elsize;

public:
   __fastcall TVRing(int len, int elementsize);
   __fastcall ~TVRing(void);

   bool __fastcall get(char* el, int len);
   bool __fastcall put(char* el, int len);
   int  __fastcall getlen(void);
   void __fastcall clear(void);
};
//---------------------------------------------------------------------------
class TDLLForm : public TForm
{
__published:  // IDE-managed Components
  TDRLabel *Title;
  TDRLabel *LClock;
  TDRLabel *LRate;
  TDRLabel *LNCO;
  TDRLabel *LFW;
  TDRLabel *NCO;
  TDRLabel *FW;
  TCheckBox *ClM;
  TCheckBox *ClP;
  TCheckBox *ClA;
  TDRLabel *DRLabel2;
  TDRLabel *DRLabel5;
  TDRLabel *DRLabel6;
  TDRLabel *DRLabel7;
  TCheckBox *Rate48;
  TCheckBox *Rate96;
  TDRLabel *DRLabel9;
  TCheckBox *Rate192;
  TDRLabel *DRLabel10;
  TDRLabel *DRLabel8;
  TDRLabel *DRLabel1;
  TImage *Fondo;
  TDRLabel *LMclock;
  TDRLabel *DRLabel4;
  TDRLabel *DRLabel11;
  TCheckBox *MClP;
  TCheckBox *MClM;
  TDRLabel *LAudio;
  TDRLabel *DRLabel13;
  TCheckBox *SCard;
  TDRLabel *DRLabel14;
  TCheckBox *DAC;
  TDRLabel *LPream;
  TDRLabel *LDither;
  TDRLabel *LRandom;
  TCheckBox *Preamp;
  TCheckBox *Dither;
  TCheckBox *Random;
  TDRLabel *LOverload;
  TAutoLed *Overload;
  TDRLabel *LCards;
  TDRLabel *DRLabel18;
  TCheckBox *CardP;
  TDRLabel *DRLabel19;
  TCheckBox *CardM;
  void __fastcall FondoMouseDown(TObject *Sender, TMouseButton Button, TShiftState Shift,
      int X, int Y);
  void __fastcall FondoMouseMove(TObject *Sender, TShiftState Shift, int X, int Y);
  void __fastcall FondoMouseUp(TObject *Sender, TMouseButton Button, TShiftState Shift,
      int X, int Y);
  void __fastcall ClMClick(TObject *Sender);
  void __fastcall ClPClick(TObject *Sender);
  void __fastcall ClAClick(TObject *Sender);
  void __fastcall Rate48Click(TObject *Sender);
  void __fastcall Rate96Click(TObject *Sender);
  void __fastcall Rate192Click(TObject *Sender);
  void __fastcall MClMClick(TObject *Sender);
  void __fastcall MClPClick(TObject *Sender);
  void __fastcall PreampClick(TObject *Sender);
  void __fastcall DitherClick(TObject *Sender);
  void __fastcall RandomClick(TObject *Sender);
  void __fastcall CardMClick(TObject *Sender);
  void __fastcall CardPClick(TObject *Sender);
  void __fastcall SCardClick(TObject *Sender);
  void __fastcall DACClick(TObject *Sender);


private:

  POINT  PT;
  bool   moving, shield;
  typedef enum   {Mercury, Penelope, Atlas} clsource;
  clsource clsrc;
  int      savecl, savemcl, savepre, savedith, saverand, savecm, savecp;
  AnsiString FIniFileName;
  TIniFile   *IniFile;

  void __fastcall SetClock(clsource src);
  void __fastcall SetMasterClock(clsource src);
  void __fastcall SetRate(int rate);
  bool __fastcall RestoreParms(void);
  void __fastcall SaveParms(void);

public: 

  struct   HPSDR_WUSB_INFO hpwi;
  TWorker* Worker;
  TTalker* Talker;
  TVRing*  DACRing;
  bool     CanSend;
  unsigned long     LOfreq;
  char     FX2fname[256], FPGAfname[256];
  int      savehpCard, savehpDAC, Srate;
  unsigned char config_byte, control_byte, rate_byte, clbits;

  bool __fastcall Initialize(void);

  __fastcall TDLLForm(TComponent* Owner);
  __fastcall ~TDLLForm();

  friend class TWorker;
  friend class TTalker;
};

extern int (* cback)(int, int, float, void *);

//---------------------------------------------------------------------------
extern PACKAGE TDLLForm *DLLForm;
//---------------------------------------------------------------------------
#endif
