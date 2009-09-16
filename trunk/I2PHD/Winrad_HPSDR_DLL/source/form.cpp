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

#include <vcl.h>
#include <windows.h>
#pragma hdrstop

#include "form.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma link "DrLabel"
#pragma link "AutoLed"
#pragma resource "*.dfm"

TDLLForm *DLLForm;

//---------------------------------------------------------------------------
__fastcall TDLLForm::TDLLForm(TComponent* Owner)
  : TForm(Owner), moving(false), shield(false), Srate(192000)
{
}
//---------------------------------------------------------------------------
__fastcall TDLLForm::~TDLLForm()
{
  SaveParms();
}
//---------------------------------------------------------------------------
bool __fastcall TDLLForm::Initialize(void)
{

  RestoreParms();
  return true;
}
//---------------------------------------------------------------------------
void __fastcall TDLLForm::FondoMouseDown(TObject *Sender, TMouseButton Button, TShiftState Shift,
      int X, int Y)
{
   PT.x = X;   PT.y = Y;
   ::ClientToScreen(DLLForm->Handle , &PT);
   moving = true;
}
//---------------------------------------------------------------------------

void __fastcall TDLLForm::FondoMouseMove(TObject *Sender, TShiftState Shift, int X,
      int Y)
{
   POINT LPT;

   if(!moving) return;
   LPT.x = X;     LPT.y = Y;
   ::ClientToScreen(DLLForm->Handle , &LPT);
   DLLForm->Left += LPT.x - PT.x;
   DLLForm->Top  += LPT.y - PT.y;
   PT.x = LPT.x;  PT.y = LPT.y;
}
//---------------------------------------------------------------------------

void __fastcall TDLLForm::FondoMouseUp(TObject *Sender, TMouseButton Button, TShiftState Shift,
      int X, int Y)
{
   moving = false;
}
//---------------------------------------------------------------------------
void __fastcall TDLLForm::SetClock(clsource src)
{
   switch(src)
   {
     case Mercury   : clbits = 0x08; savecl = 0; break;
     case Penelope  : clbits = 0x04; savecl = 1; break;
     case Atlas     : clbits = 0x00; savecl = 2; break;
   }

   config_byte &= 0xf3;   // set the 10 MHz clock bits to zero
   config_byte |= clbits;
}
//---------------------------------------------------------------------------
void __fastcall TDLLForm::SetMasterClock(clsource src)
{
   switch(src)
   {
     case Mercury   : clbits = 0x10; savemcl = 0;  break;
     case Penelope  : clbits = 0x00; savemcl = 1;  break;
   }

   config_byte &= 0xef;   // set the master clock bit to zero
   config_byte |= clbits;
}
//---------------------------------------------------------------------------
void __fastcall TDLLForm::SetRate(int rate)
{
   Srate = rate;
   config_byte &= 0xfc;   // set the last twp bits to zero
   config_byte |= (Srate / 48000) >> 1;

   if(cback) cback(-1, 100, 0, NULL);   // -1 = status change,  100 = new sampling speed
}
//---------------------------------------------------------------------------

void __fastcall TDLLForm::ClMClick(TObject *Sender)
{
   if(shield) return;         shield = true;
   ClM->Checked = true;       ClP->Checked = false;   ClA->Checked = false;
   SetClock(Mercury);         shield = false;
}
//---------------------------------------------------------------------------

void __fastcall TDLLForm::ClPClick(TObject *Sender)
{
   if(shield) return;         shield = true;
   ClM->Checked = false;      ClP->Checked = true;   ClA->Checked = false;
   SetClock(Penelope);        shield = false;
}
//---------------------------------------------------------------------------

void __fastcall TDLLForm::ClAClick(TObject *Sender)
{
   if(shield) return;         shield = true;
   ClM->Checked = false;      ClP->Checked = false;   ClA->Checked = true;
   SetClock(Atlas);           shield = false;
}
//---------------------------------------------------------------------------

void __fastcall TDLLForm::Rate48Click(TObject *Sender)
{
   if(shield) return;         shield = true;
   Rate48->Checked = true;    Rate96->Checked = false;   Rate192->Checked = false;
   SetRate(48000);            shield = false;
}
//---------------------------------------------------------------------------

void __fastcall TDLLForm::Rate96Click(TObject *Sender)
{
   if(shield) return;         shield = true;
   Rate48->Checked = false;   Rate96->Checked = true;   Rate192->Checked = false;
   SetRate(96000);            shield = false;
}
//---------------------------------------------------------------------------

void __fastcall TDLLForm::Rate192Click(TObject *Sender)
{
   if(shield) return;         shield = true;
   Rate48->Checked = false;   Rate96->Checked = false;   Rate192->Checked = true;
   SetRate(192000);           shield = false;
}
//---------------------------------------------------------------------------

void __fastcall TDLLForm::MClMClick(TObject *Sender)
{
   if(shield) return;        shield = true;
   MClM->Checked = true;     MClP->Checked = false;
   SetMasterClock(Mercury);  shield = false;
}
//---------------------------------------------------------------------------

void __fastcall TDLLForm::MClPClick(TObject *Sender)
{
   if(shield) return;        shield = true;
   MClM->Checked = false;    MClP->Checked = true;
   SetMasterClock(Penelope); shield = false;
}
//---------------------------------------------------------------------------
bool __fastcall TDLLForm::RestoreParms(void)
{
   char s[128], path[256];
   long rate;
   TSearchRec sr;
   int clk, mclk, pre, dith, rand, audio, cardm, cardp, dac, scard;

   strcpy(path, (char *)ExtractFilePath(Application->ExeName).c_str());
   strcpy(s, path);  strcat(s, ININAME);
   FIniFileName = s;

   if(FindFirst(s, faAnyFile, sr) != 0)  // if INI file missing, load defaults
   {
     clk   = 0;      // 10 MHz clock from Mercury
     mclk  = 0;      // 122.88 MHz clock from Mercury
     rate  = 192000; // 192 kHz as default
     pre   = 0;      // preamp off
     dith  = 0;      // dithering off
     rand  = 0;      // output randomizer off
     cardm = 1;      // Mercury present
     cardp = 1;      // Penelope present
     dac   = 1;      // sound output on Mercury DAC
     scard = 1;      // sound ouput on PC sound card

     FindClose(sr);
   }
   else
   {
     FindClose(sr);
     IniFile = new TIniFile(FIniFileName);

     clk        = IniFile->ReadInteger(SectionParameters, KeyClock, 0);   // 0 = Mercury
     mclk       = IniFile->ReadInteger(SectionParameters, KeyMClock, 0);  // 0 = Mercury
     rate       = IniFile->ReadInteger(SectionParameters, KeySrate, 192000); // 192 kHz
     pre        = IniFile->ReadInteger(SectionParameters, KeyPreamp, 0);  // 0 = Pre OFF
     dith       = IniFile->ReadInteger(SectionParameters, KeyDither, 0);  // 0 = Dither OFF
     rand       = IniFile->ReadInteger(SectionParameters, KeyRandom, 0);  // 0 = Random OFF
     dac        = IniFile->ReadInteger(SectionParameters, KeyDACOut, 1); // 1 = DAC out
     scard      = IniFile->ReadInteger(SectionParameters, KeyCardOut, 1); // 1 = Sound card output
   }

   cardm = 1;    // ----------------- Mercury *must* be present -----------------


   config_byte  = 0x80;  // mic source = Penelope by default
   control_byte = 0;

   switch(rate)
   {
     case 48000  : Rate48->Checked = true;  break;
     case 96000  : Rate96->Checked = true;  break;
     case 192000 : Rate192->Checked = true; break;
   }

   switch(mclk)
   {
     case 0 : MClM->Checked = true; break;
     case 1 : MClP->Checked = true; break;
   }

   switch(clk)
   {
     case 0 : ClM->Checked = true; break;
     case 1 : ClP->Checked = true; break;
     case 2 : ClA->Checked = true; break;
   }

   if(pre)   Preamp->Checked = true;
   if(dith)  Dither->Checked = true;
   if(rand)  Random->Checked = true;
   if(cardm) CardM->Checked  = true;
   if(dac)   DAC->Checked    = true;
   if(scard) SCard->Checked  = true;

   delete IniFile;   // delete accepts a null pointer....
   return true;
}
//---------------------------------------------------------------------------
void __fastcall TDLLForm::SaveParms(void)
{
   char s[128], path[256];

   if(FIniFileName.IsEmpty())  // if INI file seems to not exist, create one
   {
     strcpy(path, (char *)ExtractFilePath(Application->ExeName).c_str());
     strcpy(s, path);  strcat(s, ININAME);
     FIniFileName = s;
   }

   IniFile = new TIniFile(FIniFileName);

   IniFile->WriteInteger(SectionParameters, KeyClock,    savecl);
   IniFile->WriteInteger(SectionParameters, KeyMClock,   savemcl);
   IniFile->WriteInteger(SectionParameters, KeySrate,    Srate);
   IniFile->WriteInteger(SectionParameters, KeyPreamp,   savepre);
   IniFile->WriteInteger(SectionParameters, KeyDither,   savedith);
   IniFile->WriteInteger(SectionParameters, KeyRandom,   saverand);
   IniFile->WriteInteger(SectionParameters, KeyCardM,    savecm);
   IniFile->WriteInteger(SectionParameters, KeyCardP,    savecp);
   IniFile->WriteInteger(SectionParameters, KeyDACOut,   savehpDAC);
   IniFile->WriteInteger(SectionParameters, KeyCardOut,  savehpCard);

   delete IniFile;
}
//---------------------------------------------------------------------------
void __fastcall TDLLForm::PreampClick(TObject *Sender)
{
   savepre = Preamp->Checked;  clbits = (savepre != 0) ? 0x04 : 0;
   control_byte &= 0xfb;   // set the preamp bit to zero
   control_byte |= clbits;
}
//---------------------------------------------------------------------------
void __fastcall TDLLForm::DitherClick(TObject *Sender)
{
   savedith = Dither->Checked;  clbits = (savedith != 0) ? 0x08 : 0;
   control_byte &= 0xf7;   // set the dither bit to zero
   control_byte |= clbits;
}
//---------------------------------------------------------------------------
void __fastcall TDLLForm::RandomClick(TObject *Sender)
{
   saverand = Random->Checked;  clbits = (saverand != 0) ? 0x10 : 0;
   control_byte &= 0xef;   // set the random bit to zero
   control_byte |= clbits;
}
//---------------------------------------------------------------------------
void __fastcall TDLLForm::CardMClick(TObject *Sender)
{
   savecm = CardM->Checked;     clbits = (savecm != 0) ? 0x40 : 0;
   config_byte &= 0xbf;    // set the Mercury present bit to zero
   config_byte |= clbits;
}
//---------------------------------------------------------------------------
void __fastcall TDLLForm::CardPClick(TObject *Sender)
{
   savecp = CardP->Checked;     clbits = (savecp != 0) ? 0x20 : 0;
   config_byte &= 0xdf;    // set the Penelope present bit to zero
   config_byte |= clbits;
}
//---------------------------------------------------------------------------
void __fastcall TDLLForm::SCardClick(TObject *Sender)
{
   int code;

   savehpCard = SCard->Checked;   code = (savehpCard != 0) ? 112 : 113;
   if(cback) cback(-1, code, 0, NULL);   // -1 = status change
}
//---------------------------------------------------------------------------
void __fastcall TDLLForm::DACClick(TObject *Sender)
{
   int code;

   savehpDAC = DAC->Checked;      code = (savehpDAC != 0) ? 110 : 111;
   if(cback) cback(-1, code, 0, NULL);   // -1 = status change
}
//---------------------------------------------------------------------------
__fastcall TVRing::TVRing(int numel, int elementsize)
{
   size = numel;
   elsize = elementsize;
   ring = new char[size * elsize];
   for(int k = 0; k < size * elsize; k++) ring[k] = 0;
   Top = Que = 0;
}
//---------------------------------------------------------------------------
__fastcall TVRing::~TVRing()
{
   delete[] ring;
}
//---------------------------------------------------------------------------
bool __fastcall TVRing::get(char* el, int numel)
{
   headroom = size - Que;

   if(numel <= headroom)
   {
     memmove(el, ring + Que * elsize, numel * elsize);
   }
   else
   {
     memmove(el, ring + Que * elsize, headroom * elsize);
     memmove(el + headroom * elsize, ring, (numel - headroom) * elsize);
   }

   Que = (Que + numel) % size;

   return true;    // error return to be done (overrun and underrun)
}
//---------------------------------------------------------------------------
bool __fastcall TVRing::put(char* el, int numel)
{
   headroom = size - Top;

   if(numel <= headroom)
   {
     memmove(ring + Top * elsize, el, numel * elsize);
   }
   else
   {
     memmove(ring + Top * elsize, el, headroom * elsize);
     memmove(ring, el + headroom * elsize, (numel - headroom) * elsize);
   }

   Top = (Top + numel) % size;

   return true;    // error return to be done (overrun and underrun)
}
//---------------------------------------------------------------------------
int  __fastcall TVRing::getlen(void)
{
   int numel = (Top - Que);
   if(numel < 0) numel += size;

   return numel;
}
//---------------------------------------------------------------------------
void __fastcall TVRing::clear(void)
{
   for(int k = 0; k < size * elsize; k++) ring[k] = 0;
   Top = Que = 0;
}
//---------------------------------------------------------------------------

