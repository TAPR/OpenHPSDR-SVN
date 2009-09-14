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

#ifndef workerH
#define workerH

#include <mmsystem.h>
#include <math.h>

#define NUMIQPAIRS  512
#define NUMIOBLOCKS 2     // use 4 for the hiLatency_lowCPU version


#define PI 3.141592653589793

//---------------------------------------------------------------------------
class TWorker : public TThread
{
private:

	char s[256];
	float OV, MV, PV, IQoffs;
	int   cnt, status, passcounter, qued, numpasses;
	bool flipflop;
	OVERLAPPED rolap;

	union
	{
	  unsigned char inbuf[512 * NUMIOBLOCKS];
	  struct
	  {
		 unsigned char sync1;
		 unsigned char sync2;
		 unsigned char sync3;
		 unsigned char C0;
		 unsigned char C1;
		 unsigned char C2;
		 unsigned char C3;
		 unsigned char C4;

		 struct
		 {
		   unsigned char L1;
		   unsigned char L2;
		   unsigned char L3;
		   unsigned char R1;
		   unsigned char R2;
		   unsigned char R3;
		   unsigned char M1;
		   unsigned char M2;
		 } samples[63];
	  } bytes[NUMIOBLOCKS];
	} In;

	void __fastcall SendToPC(void);

protected:
	void __fastcall Execute();

public:

	union
	{
	  unsigned char outbuf[512 * NUMIOBLOCKS];
	  struct
	  {
		 unsigned char sync1;
		 unsigned char sync2;
		 unsigned char sync3;
		 unsigned char C0;
		 unsigned char C1;
		 unsigned char C2;
		 unsigned char C3;
		 unsigned char C4;

		 struct
		 {
		   unsigned char L_MSB;
		   unsigned char L_LSB;
		   unsigned char R_MSB;
		   unsigned char R_LSB;
		   unsigned char I1;
		   unsigned char I2;
		   unsigned char Q1;
		   unsigned char Q2;
		 } samples[63];
	  } bytes[NUMIOBLOCKS];
	} Outb;

	HANDLE CloseDown;

	struct
	{
	  unsigned char L_LSB;
	  unsigned char L_MSB;
	  unsigned char R_LSB;
	  unsigned char R_MSB;
	} SpkrData[63];

	struct
	{
	  unsigned char L1;
	  unsigned char L2;
	  unsigned char L3;
	  unsigned char R1;
	  unsigned char R2;
	  unsigned char R3;
	} LRdata[2][NUMIQPAIRS];

	union
	{
	  unsigned char outData[6 * NUMIQPAIRS]; // will contain either LRdata [0] or [1]

	  struct aData
	  {
		 unsigned char L_LSB;
		 unsigned char L_MSB;
		 unsigned char R_LSB;
		 unsigned char R_MSB;
	  } audioData[512 * 4];   // must match what Winrad sends...
	} IObuf;

	int idx;
	bool   firstpass;
	HANDLE toPC, toHPSDR;
	OVERLAPPED wolap;

	__fastcall TWorker(bool CreateSuspended);
};
//---------------------------------------------------------------------------
class TTalker : public TThread
{
private:

protected:
	void __fastcall Execute();

public:
	HANDLE CloseDown;

	__fastcall TTalker(bool CreateSuspended);

};
//---------------------------------------------------------------------------

#include "form.h"

extern int  (* cback)(int, int, float, void *);
extern unsigned long ReadFromHPSDR(UCHAR *buf, int size);
extern bool WriteToHPSDR(UCHAR *buf, int size);
extern bool ReadNBFromHPSDR(UCHAR *buf, int size, OVERLAPPED& olap);
extern bool WriteNBToHPSDR(UCHAR *buf, int size, OVERLAPPED& olap);
//---------------------------------------------------------------------------
#endif
