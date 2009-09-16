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
//---------------------------------------------------------------------------
#include "main.h"
//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
//   Important note about DLL memory management when your DLL uses the
//   static version of the RunTime Library:
//
//   If your DLL exports any functions that pass String objects (or structs/
//   classes containing nested Strings) as parameter or function results,
//   you will need to add the library MEMMGR.LIB to both the DLL project and
//   any other projects that use the DLL.  You will also need to use MEMMGR.LIB
//   if any other projects which use the DLL will be performing new or delete
//   operations on any non-TObject-derived classes which are exported from the
//   DLL. Adding MEMMGR.LIB to your project will change the DLL and its calling
//   EXE's to use the BORLNDMM.DLL as their memory manager.  In these cases,
//   the file BORLNDMM.DLL should be deployed along with your DLL.
//
//   To avoid using BORLNDMM.DLL, pass string information using "char *" or
//   ShortString parameters.
//
//   If your DLL uses the dynamic version of the RTL, you do not need to
//   explicitly add MEMMGR.LIB as this will be done implicitly for you
//---------------------------------------------------------------------------
#pragma argsused

//---------------------------------------------------------------------------
int WINAPI DllEntryPoint(HINSTANCE hinst, unsigned long reason, void* lpReserved)
{
        return 1;
}
//---------------------------------------------------------------------------
extern "C"
bool __stdcall __declspec(dllexport) InitHW(char *name, char *model, int& index)
{
   static bool first = true;

   index = 8;    // 8 = HPSDR, HW does the digitizing, 24-bit values

   cback = NULL;

   if(first)
   {
     first = false;
     Application->CreateForm(__classid(TDLLForm), &DLLForm);
     DLLForm->Left = -9999;
     DLLForm->Visible = true;
     ZeroMemory(&DLLForm->hpwi, sizeof(DLLForm->hpwi));
     DLLForm->DACRing = new TVRing(4096, 4);    // 2 chars each Left and Right
     strcpy(DLLForm->FX2fname,  "ozyfw-sdr1k.hex");
     strcpy(DLLForm->FPGAfname, "Ozy_Janus.rbf");

     DLLForm->hpwi.FX2fname  = DLLForm->FX2fname;
     DLLForm->hpwi.FPGAfname = DLLForm->FPGAfname;
   }

   strcpy(name, "HPSDR with Mercury");
   strcpy(model, "V1.0");

   if((Initialize_HPSDR(DLLForm->hpwi) % 1000) != 3)
      return false;

   DLLForm->Visible = false;
   DLLForm->Left = Screen->Width - DLLForm->Width;
   DLLForm->Top = Screen->Height - DLLForm->Height;
   return DLLForm->Initialize();
}
//---------------------------------------------------------------------------
extern "C"
bool __stdcall __declspec(dllexport) OpenHW(void)
{
   DLLForm->Show();
   ::SetWindowPos(DLLForm->Handle, HWND_TOPMOST,
          0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);

  DLLForm->Worker = new TWorker(false);  // thread created not suspended
  DLLForm->Talker = new TTalker(false);  // thread created not suspended
  ::SetThreadPriority((HANDLE) DLLForm->Worker->Handle, THREAD_PRIORITY_TIME_CRITICAL);
  ::SetThreadPriority((HANDLE) DLLForm->Talker->Handle, THREAD_PRIORITY_TIME_CRITICAL);

  return true;
}
//---------------------------------------------------------------------------
extern "C"
void __stdcall __declspec(dllexport) SetHWLO(long freq)
{
   DLLForm->NCO->Caption = FormatFloat("##,###,##0", (long double)freq);
   DLLForm->LOfreq = (unsigned long)freq;
}
//---------------------------------------------------------------------------
extern "C"
int __stdcall __declspec(dllexport) StartHW(long freq)
{
   SetHWLO(freq);
   DLLForm->Show();
   DLLForm->Worker->firstpass = true;
   DLLForm->DACRing->clear();
   DLLForm->CanSend = true;
   return NUMIQPAIRS;      // number of complex elements returned each time
}
//---------------------------------------------------------------------------
extern "C"
int __stdcall __declspec(dllexport) GetStatus(void)
{
   return 0;  
}
//---------------------------------------------------------------------------
extern "C"
long __stdcall __declspec(dllexport) GetHWSR(void)
{
   return DLLForm->Srate;

}//---------------------------------------------------------------------------
extern "C"
void __stdcall __declspec(dllexport) StopHW(void)
{
   DLLForm->CanSend = false;
   return;
}
//---------------------------------------------------------------------------
extern "C"
void __stdcall __declspec(dllexport) CloseHW(void)
{
   DLLForm->CanSend = false;
   DLLForm->Hide();

   SetEvent(DLLForm->Talker->CloseDown);
   DLLForm->Talker->Terminate();
   DLLForm->Talker->WaitFor();
   DLLForm->Worker->Terminate();
   DLLForm->Worker->WaitFor();
}
//---------------------------------------------------------------------------
extern "C"
long __stdcall __declspec(dllexport) GetHWLO(void)
{
   return DLLForm->LOfreq;
}
//---------------------------------------------------------------------------
extern "C"
void __stdcall __declspec(dllexport) SetCallback(int (* Callback)(int, int, float, void *))
{
   int code;

   cback = Callback;    // address of callback function

   code = (DLLForm->savehpCard != 0) ? 112 : 113;

   if(cback) cback(-1, code, 0, NULL);   // -1 = status change

   code = (DLLForm->savehpDAC != 0)  ? 110 : 111;

   if(cback) cback(-1, code, 0, NULL);   // -1 = status change

   return;
}
//---------------------------------------------------------------------------
extern "C"
void __stdcall __declspec(dllexport) ShowGUI(void)
{
   DLLForm->Show();
}
//---------------------------------------------------------------------------
extern "C"
void __stdcall __declspec(dllexport) HideGUI(void)
{
   DLLForm->Hide();
}
//---------------------------------------------------------------------------

#pragma package(smart_init)





