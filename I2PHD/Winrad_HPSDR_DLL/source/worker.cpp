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

#include "worker.h"

//---------------------------------------------------------------------------
__fastcall TWorker::TWorker(bool CreateSuspended)
    : TThread(CreateSuspended), firstpass(true)
{
   CloseDown    = CreateEvent(NULL, FALSE, FALSE, NULL);
   rolap.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
   wolap.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
   toPC         = CreateEvent(NULL, FALSE, FALSE, NULL);
   toHPSDR      = CreateEvent(NULL, FALSE, FALSE, NULL);

   ResetEvent(CloseDown);

}
//---------------------------------------------------------------------------
void __fastcall TWorker::Execute()
{
   HANDLE Events[2] = {rolap.hEvent, CloseDown};
   DWORD which;

   ResetEvent(rolap.hEvent);
   ZeroMemory(Outb.outbuf, 512);
   Outb.outbuf[0] = Outb.outbuf[1] = Outb.outbuf[2] = 0x7f;   // Sync
   Outb.outbuf[3] = 0x0;
   Outb.outbuf[4] = DLLForm->config_byte;
   Outb.outbuf[5] = 0x0;
   Outb.outbuf[6] = DLLForm->control_byte;
   Outb.outbuf[7] = 0x0;

   WriteToHPSDR(Outb.outbuf, 512);

   Outb.outbuf[3] = 0x02;
   Outb.outbuf[4] = (DLLForm->LOfreq)  >> 24;
   Outb.outbuf[5] = ((DLLForm->LOfreq) >> 16) & 0x000000ff;
   Outb.outbuf[6] = ((DLLForm->LOfreq) >> 8)  & 0x000000ff;
   Outb.outbuf[7] = (DLLForm->LOfreq)         & 0x000000ff;

   WriteToHPSDR(Outb.outbuf, 512);
   numpasses = 0;

   while (!Terminated)
   {
     if(firstpass)
     {
       firstpass = false;
       ReadFromHPSDR(In.inbuf, 512);

       if((In.bytes[0].C0 & 0x08) == 0)
       {
          OV = In.bytes[0].C4;  MV = In.bytes[0].C2;  PV = In.bytes[0].C3;
          sprintf(s, "%4.1f  %4.1f  %4.1f", OV/10.F,  MV/10.F,  PV/10.F);
          DLLForm->FW->Caption = s;
       }

       DLLForm->ClP->Enabled   = PV > 0;
       DLLForm->MClP->Enabled  = PV > 0;
      //     DLLForm->CardP->Enabled = PV > 0;
       DLLForm->CardP->Checked = PV > 0;
       DLLForm->DRLabel5->Font->Color  = (PV > 0) ? clWhite : clGray;
       DLLForm->DRLabel11->Font->Color = (PV > 0) ? clWhite : clGray;
       DLLForm->DRLabel18->Font->Color = (PV > 0) ? clWhite : clGray;

       passcounter = 0;
       qued = 0;
       idx = 0;
       flipflop    = true;

       if(++numpasses < 16)
       {
         firstpass = true;
         continue;
       }
     }


//------------------

     ReadNBFromHPSDR(In.inbuf, 512 * NUMIOBLOCKS, rolap);

     if((passcounter += 48000) >= DLLForm->Srate)
     {
       passcounter = 0;

       for(int n = 0; n < NUMIOBLOCKS; n++)
       {
         Outb.bytes[n].sync1 = 0x7f;
         Outb.bytes[n].sync2 = 0x7f;
         Outb.bytes[n].sync3 = 0x7f;

         if(flipflop)
         {
           Outb.bytes[n].C0 = 0x0;
           Outb.bytes[n].C1 = DLLForm->config_byte;
           Outb.bytes[n].C2 = 0x0;
           Outb.bytes[n].C3 = DLLForm->control_byte;
           Outb.bytes[n].C4 = 0x0;
         }
         else
         {
           Outb.bytes[n].C0 = 0x02;
           Outb.bytes[n].C1 = (DLLForm->LOfreq)  >> 24;
           Outb.bytes[n].C2 = ((DLLForm->LOfreq) >> 16) & 0x000000ff;
           Outb.bytes[n].C3 = ((DLLForm->LOfreq) >> 8)  & 0x000000ff;
           Outb.bytes[n].C4 = (DLLForm->LOfreq)         & 0x000000ff;
         }

         flipflop = !flipflop;

         if(DLLForm->CanSend)
            DLLForm->DACRing->get((unsigned char*)SpkrData, 63);
         else
            ZeroMemory(SpkrData, sizeof(SpkrData));

         if(!DLLForm->savehpDAC)
             ZeroMemory(SpkrData, sizeof(SpkrData));

         for(int k = 0; k < 63; k++)
         {
           Outb.bytes[n].samples[k].L_MSB = SpkrData[k].L_MSB;
           Outb.bytes[n].samples[k].L_LSB = SpkrData[k].L_LSB;
           Outb.bytes[n].samples[k].R_MSB = SpkrData[k].R_MSB;
           Outb.bytes[n].samples[k].R_LSB = SpkrData[k].R_LSB;
           Outb.bytes[n].samples[k].I1 = 0;  // IQ data for Penelope
           Outb.bytes[n].samples[k].I2 = 0;  //       ...
           Outb.bytes[n].samples[k].Q1 = 0;  //       ...
           Outb.bytes[n].samples[k].Q2 = 0;  //       ...
         }
       }

       WriteToHPSDR(Outb.outbuf, 512 * NUMIOBLOCKS);
     }

     which = WaitForMultipleObjects(2, Events, FALSE, INFINITE);

     if(which == WAIT_OBJECT_0 + 1)  // CloseDown was posted
        break;

     SendToPC();
   }
}
//---------------------------------------------------------------------------
void __fastcall TWorker::SendToPC(void)
{
  for(int m = 0; m < NUMIOBLOCKS; m++)
  {
    if((In.bytes[m].C0 & 0x08 == 0) && (In.bytes[m].C1 & 0x01))
    DLLForm->Overload->Value = true;

    for(int k = 0; k < 63; k++)
    {
      LRdata[idx][qued].R1 = In.bytes[m].samples[k].L3; // HPSDR data are big-endian...
      LRdata[idx][qued].R2 = In.bytes[m].samples[k].L2; // and the Q signal is on the Left channel
      LRdata[idx][qued].R3 = In.bytes[m].samples[k].L1; // following the FlexRadio conventions...

      LRdata[idx][qued].L1 = In.bytes[m].samples[k].R3; // HPSDR data are big-endian...
      LRdata[idx][qued].L2 = In.bytes[m].samples[k].R2; // and the Q signal is on the Left channel
      LRdata[idx][qued].L3 = In.bytes[m].samples[k].R1; // following the FlexRadio conventions...

      if(++qued == NUMIQPAIRS)
      {
        qued = 0;
        idx = 1 - idx;
        if(DLLForm->CanSend)
          SetEvent(DLLForm->Worker->toPC);
      }
    }
  }
}
//------------------------------- Talker ------------------------------------

__fastcall TTalker::TTalker(bool CreateSuspended)
    : TThread(CreateSuspended)
{
   CloseDown = CreateEvent(NULL, FALSE, FALSE, NULL);

   ResetEvent(CloseDown);
   ResetEvent(DLLForm->Worker->toPC);
   ResetEvent(DLLForm->Worker->toHPSDR);
   ResetEvent(DLLForm->Worker->wolap.hEvent);
}
//---------------------------------------------------------------------------
void __fastcall TTalker::Execute()
{
   HANDLE Events[2] = {DLLForm->Worker->toPC, CloseDown};
   DWORD which;
   int ix, k, r;

   while (!Terminated)
   {
     which = WaitForMultipleObjects(2, Events, FALSE, INFINITE);

     if(which == WAIT_OBJECT_0 + 2)  // CloseDown was posted
       break;

     ix = 1 - DLLForm->Worker->idx;

     if(which == WAIT_OBJECT_0)   // toPC
     {
       memcpy(DLLForm->Worker->IObuf.outData, DLLForm->Worker->LRdata[ix],
              sizeof(DLLForm->Worker->IObuf.outData));

       r = cback(NUMIQPAIRS, 0, 0.F, DLLForm->Worker->IObuf.audioData);

       if(r > 0)
          DLLForm->DACRing->put((unsigned char *)DLLForm->Worker->IObuf.audioData, 512 * 4);
     }
   }
}
//---------------------------------------------------------------------------
#pragma package(smart_init)
