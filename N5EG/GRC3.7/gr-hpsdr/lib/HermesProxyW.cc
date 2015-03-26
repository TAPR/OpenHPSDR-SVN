/* -*- c++ -*- */
/* 
 * Copyright 2013-2015 Tom McDermott, N5EG
 * 
 * This is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3, or (at your option)
 * any later version.
 * 
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this software; see the file COPYING.  If not, write to
 * the Free Software Foundation, Inc., 51 Franklin Street,
 * Boston, MA 02110-1301, USA.
 */

//
// Hermes Wideband Proxy
//
// Receive-only encapsulates the Hermes module for access/control by GNU Radio.
// Used by the HermesWB (Wide Band) module that provide Hermes source to GNU Radio.
//
// See the HPSDR documentation for USB and Ethernet frame formats.
//
// Uses the Metis Ethernet interface module to send/receive Ethernet
// frames to/from Hermes.
//
// HermesWB uses this proxy to convert raw data and control flags
// and send/receive them to Hermes.
//
// Version:  March 21, 2015

#include <gnuradio/io_signature.h>
#include "HermesProxyW.h"
#include "HermesProxy.h"
#include "metis.h"
#include <stdio.h>
#include <cstring>


HermesProxyW::HermesProxyW(bool RxPre, const char* Intfc, const char * ClkS,
			 int AlexRA, int AlexTA, int AlexHPF, int AlexLPF)	// constructor
{

	strcpy(interface, Intfc);	// Ethernet interface to use (defaults to eth0)
	unsigned int cs;		// Convert ClockSource strings to unsigned, then intitalize
	sscanf(ClkS, "%x", &cs);
	ClockSource = (cs & 0xFC);

//	Initialize the Alex control registers.

	AlexRxAnt = AlexRA;		// Select Alex Receive Antenna or from T/R relay
	AlexTxAnt = AlexTA;		// Select Alex Tx Antenna
	AlexRxHPF = AlexHPF;		// Select Alex Receive High Pass Filter
	AlexTxLPF = AlexLPF;		// Select Alex Transmit Low Pass Filter


	Receive0Frequency = 0;
	Receive1Frequency = 0; 
	TransmitFrequency = 0;		// initialize frequencies
	TxDrive = 0;		// default to (almost) off
	PTTMode = 0;
	RxPreamp = RxPre;
	PTTOffMutesTx = 0;   // PTT Off mutes the transmitter
	PTTOnMutesRx = 0;	// PTT On mutes receiver

	ADCdither = false;
	ADCrandom = false;
	RxAtten = 0;		// Hermes V2.0
	Duplex = true;		// Allows TxF to program separately from RxF

	TxStop = false;

	RxWriteCounter = 0;	//
	RxReadCounter = 0;	// These control the Rx buffers to Gnuradio
	RxWriteFill = 0;	//

	TxWriteCounter = 0;	//
 	TxReadCounter = 0;	// These control the Tx buffers to Hermes
	TxControlCycler = 0;	//
	TxFrameIdleCount = 0;	//

	LostRxBufCount = 0;	//
	TotalRxBufCount = 0;	//
	LostTxBufCount = 0;	//
	TotalTxBufCount = 0;	// diagnostics
	CorruptRxCount = 0;	//
	LostEthernetRx = 0;	//
	CurrentEthSeqNum = 0;	//

	
	// allocate the receiver buffers
	for(int i=0; i<NUMRXIQBUFS; i++)
		RxIQBuf[i] = new float[RXBUFSIZE];

	// allocate the transmit buffers
	for(int i=0; i<NUMTXBUFS; i++)
		TxBuf[i] = new unsigned char[TXBUFSIZE];

	metis_discover((const char *)(interface));

	while (!metis_found())
		;					// wait until Hermes responds

	metis_receive_stream_control(RxStream_Off);	// turn off Hermes -> PC streams

	UpdateHermes();					// send specific control registers
							// and initialize 1st Tx buffer
							// before allowing scheduler to Start()
};

HermesProxyW::~HermesProxyW()
{
	fprintf(stderr, "\nLostRxBufCount = %lu  TotalRxBufCount = %lu"
		"  LostTxBufCount = %lu  TotalTxBufCount = %lu"
		"  CorruptRxCount = %lu  LostEthernetRx = %lu\n",
	        LostRxBufCount, TotalRxBufCount, LostTxBufCount,
		TotalTxBufCount, CorruptRxCount, LostEthernetRx);

	metis_receive_stream_control(RxStream_Off);	// stop Hermes data stream
	
	metis_stop_receive_thread();	// stop receive_thread & close socket

	for(int i=0; i<NUMTXBUFS; i++)
		delete [] TxBuf[i];

	for(int i=0; i<NUMRXIQBUFS; i++)
		delete [] RxIQBuf[i];
}


void HermesProxyW::Stop()	// stop ethernet I/O
{
	metis_receive_stream_control(RxStream_Off);	// stop Hermes Rx data stream
	TxStop = true;					// stop Tx data to Hermes
};

void HermesProxyW::Start()	// start rx stream
{
	TxStop = false;					// allow Tx data to Hermes
	// Note: just turning on the WB stream does not work. Have to throw away the NB samples.
	metis_receive_stream_control(RxStream_NBWB_On);	// start Hermes Wideband Rx data stream
};

void HermesProxyW::PrintRawBuf(RawBuf_t inbuf)	// for debugging
{
	fprintf(stderr,"Raw buffer from Hermes:  0x000:: ");
	for(int i=0; i<8; i++)
	  fprintf(stderr,"%02X:",inbuf[i]); 
	fprintf(stderr,"\n");

	inbuf += 8;

	fprintf(stderr, " | 0x008:: ");
	for(int i=0; i<8; i++)
	  fprintf(stderr,"%02X:",inbuf[i]); 
	fprintf(stderr, "\n  ");

	for (int j=0; j<5; j++) {
	  for(int i=0; i<14; i++)
	    fprintf(stderr,"%02X:",inbuf[i+j*14+8]); 
	  fprintf(stderr, "\n  ");
	}
	
	fprintf(stderr, "\n");

	fprintf(stderr, " | 0x208:: ");
	for(int i=0; i<8; i++)
	  fprintf(stderr,"%02X:",inbuf[i+512]); 
	fprintf(stderr,"\n  ");

	for (int j=0; j<5; j++) {
	  for(int i=0; i<14; i++)
	    fprintf(stderr,"%02X:",inbuf[i+j*14+520]);
	  fprintf(stderr, "\n  ");
	}

	fprintf(stderr, "\n");


};

// ********** Routines to receive data from Hermes/Metis and give to Gnuradio ****************

void HermesProxyW::ReceiveRxIQ(unsigned char * inbuf)	// called by metis Rx thread.
{

	// look for lost receive packets based on skips in the HPSDR ethernet header
	// sequence number.

	unsigned int SequenceNum = (unsigned char)(inbuf[4]) << 24;
	SequenceNum += (unsigned char)(inbuf[5]) << 16;
	SequenceNum += (unsigned char)(inbuf[6]) << 8;
	SequenceNum += (unsigned char)(inbuf[7]);

	if(SequenceNum > CurrentEthSeqNum + 1)
	{
	    LostEthernetRx += (SequenceNum - CurrentEthSeqNum);
	    CurrentEthSeqNum = SequenceNum;
	}
	else
	{
	  if(SequenceNum == CurrentEthSeqNum + 1)
	    CurrentEthSeqNum++;
	}
	

//fprintf(stderr, "Sequence number: %u\n", SequenceNum);

	// Metis Rx thread gives us collection of samples including the Ethernet header
	// plus 2 x HPSDR USB frames.

	// In the wideband mode we get 256 float samples per USB input buffer.
	// When the least significant 5 bits of the sequence number are zero, the USB
	// frame is the first of {16,384 samples / 256 floats-per-frame = } 64 USB frames.
	// Output 64 consecutive frames as one float vector. 
	//

	inbuf += 8;			// skip past Ethernet header
	
	if ((SequenceNum & 0x001f) == 0)  // 32 Ethernet frame boundary --> start of vector.
	{
	  ScheduleTxFrame(); // Schedule a control bits Tx ethernet frame

	  if (!RxWriteBufAligned()) // not aligned - we have a problem
	    for (int i=0; i<63; i++)
	    {
	      if (RxBufFillCount() >= (NUMRXIQBUFS - 1)) // buffers full, drop ethernet frame
		return;		
	      IQBuf_t dummy = GetNextRxWriteBuf();  	// fill a buffer with trash
	      if (RxWriteBufAligned())
		break;						// now aligned
	    }
	}

	if (RxBufFillCount() >= (NUMRXIQBUFS - 2))	// We're full. throw away ethernet frame
	  return;

	IQBuf_t outbuf = GetCurrentRxWriteBuf();
	for (int j = 0; j<256; j++)	// read 256 floats
	{
	  int I = (((inbuf[j*2+1]) << 8) & 0xff00) | (inbuf[j*2+0] & 0xff);
	  if(I >= 32768) I -= 65536;
	  outbuf[j] = ((float)I/32767.0);  // should exactly fill one buffer
	}

	outbuf = GetNextRxWriteBuf();
	for (int j = 0; j<256; j++)	// read 256 floats
	{
	  int I = (((inbuf[j*2+1]) << 8) & 0xff00) | (inbuf[j*2+0] & 0xff);
	  if(I >= 32768) I -= 65536;
 	  outbuf[j] = ((float)I/32767.0);  // should exactly fill one buffer
	}

	outbuf = GetNextRxWriteBuf();

	return;
};

bool HermesProxyW::RxReadBufAligned()  //Check if RcvRead buffer is aligned on a 64-buffer boundary
{	
	if ((RxReadCounter & 0x3f) == 0)
	    return true;
	
	return false;
};

bool HermesProxyW::RxWriteBufAligned()  //Check if Rcv Write buffer is aligned on a 64-buffer boundary
{	
	if ((RxWriteCounter & 0x3f) == 0)
	    return true;
	
	return false;
};

int HermesProxyW::RxBufFillCount()		// how many RxBuffers are filled?
{
	if (RxWriteCounter >= RxReadCounter)
	  return(RxWriteCounter - RxReadCounter);
	else
	  return(RxWriteCounter + NUMRXIQBUFS - RxReadCounter);
};

IQBuf_t HermesProxyW::GetNextRxReadBuf()	
{						// used to be called GetIQBuf()

	if(RxReadCounter == RxWriteCounter)
	  return NULL;				// empty - no buffers to return

	IQBuf_t ReturnBuffer = RxIQBuf[RxReadCounter];	// get the next receiver buffer
	++RxReadCounter &= (NUMRXIQBUFS - 1);		// increment read counter modulo

	return ReturnBuffer;
};

IQBuf_t HermesProxyW::GetCurrentRxReadBuf()
{
	return RxIQBuf[RxReadCounter];
};

IQBuf_t HermesProxyW::GetNextRxWriteBuf()	
{						// used to be called GetIQBuf()
	  if (((RxWriteCounter+1) & (NUMRXIQBUFS - 1)) == RxReadCounter)
	  {
		LostRxBufCount++;	// No Rx Buffers available. Throw away the data
		return NULL;
	  }
	  ++RxWriteCounter &= (NUMRXIQBUFS - 1); // get next writeable buffer
	  RxWriteFill = 0;
	  return RxIQBuf[RxWriteCounter];
};

IQBuf_t HermesProxyW::GetCurrentRxWriteBuf()
{
	return RxIQBuf[RxWriteCounter];
};



// ************  Routines to send data from gnuradio to the transmitter ***************


// Queue two transmit USB frames to Hermes each group of 64 receive buffers. It is used to
// to keep the transmit data zeroed out and to send controls to Hermes

void HermesProxyW::ScheduleTxFrame() // Queue two USB frames to Hermes if ready.
{
	PutTxIQ();			// Queue a pair of USB frames
	PutTxIQ();
	SendTxIQ();			// Then send it as one ethernet frame
	return;
};


void HermesProxyW::UpdateHermes()	// send a set of control registers to hardware with naught Tx data
{

	// Repurposed to send the initial registers to Hermes before starting the stream.
	// Ought to rename this as InitializeHermes or something similar.

	// DEBUG
	//fprintf(stderr, "UpdateHermes called\n");

	unsigned char buffer[512];	// dummy up a USB HPSDR buffer;
	for(int i=0; i<512; i++)
		buffer[i] = 0;

	int length = 512;		// metis_write ignores this value
	unsigned char ep = 0x02;	// all Hermes data is sent to end point 2

	// metis_write needs to be called twice to make one ethernet write to the hardware
	// Set these registers before starting the receive stream

	BuildControlRegs(0, buffer);
	metis_write(ep, buffer, length);
	BuildControlRegs(2, buffer);
	metis_write(ep, buffer, length);

	BuildControlRegs(0, buffer);
	metis_write(ep, buffer, length);
	BuildControlRegs(4, buffer);
	metis_write(ep, buffer, length);

	BuildControlRegs(0, buffer);
	metis_write(ep, buffer, length);
	BuildControlRegs(6, buffer);
	metis_write(ep, buffer, length);

	// Initialize the first TxBuffer (currently empty) with a valid control frame (on startup only)
	
	BuildControlRegs(0, buffer);
	RawBuf_t initial = TxBuf[0];
	for(int i=0; i<512; i++)
		initial[i] = buffer[i];

	return;
}


void HermesProxyW::BuildControlRegs(unsigned RegNum, RawBuf_t outbuf)
{
	// create the sync + control register values to send to Hermes
	// base on RegNum and the various parameter values.
	// RegNum must be even.

	unsigned char Speed = 0;	// Rx sample rate
	unsigned char RxCtrl = 0;	// Rx controls
	unsigned char Ctrl4 = 0;	// Rx register C4 control

	outbuf[0] = outbuf[1] = outbuf[2] = 0x7f;	// HPSDR USB sync

	outbuf[3] = RegNum;		// C0 Control Register (Bank Sel + PTT)
	if (PTTMode == PTTOn)
	  outbuf[3] |= 0x01;				// set MOX bit

	switch(RegNum)
	{
	  case 0:
	    Speed = ClockSource;	// Set clock Source from user input
	    if(RxSampleRate == 384000)
		Speed |= 0x03;
	    if(RxSampleRate == 192000)
		Speed |= 0x02;
	    if(RxSampleRate == 96000)
		Speed |= 0x01;
	    if(RxSampleRate == 48000)
		Speed |= 0x00;

	    RxCtrl = 0x00;
	    if(RxPreamp)
		RxCtrl |= 0x04;
	    if(ADCdither)
		RxCtrl |= 0x08;
	    if(ADCrandom)
		RxCtrl |= 0x10;

	    if(NumReceivers == 2)
		Ctrl4 |= 0x08;
	    if(Duplex)
		Ctrl4 |= 0x04;

	    outbuf[4] = Speed;				// C1
	    outbuf[5] = 0x00;				// C2
	    outbuf[6] = RxCtrl | AlexRxAnt;		// C3
	    outbuf[7] = Ctrl4 | AlexTxAnt;		// C4 - #Rx, Duplex
          break;

	  case 2:					// Tx NCO freq (and Rx1 NCO for special case)
	    outbuf[4] = ((unsigned char)(TransmitFrequency >> 24)) & 0xff;	// c1 RxFreq MSB
	    outbuf[5] = ((unsigned char)(TransmitFrequency >> 16)) & 0xff;	// c2
	    outbuf[6] = ((unsigned char)(TransmitFrequency >> 8)) & 0xff;	// c3
	    outbuf[7] = ((unsigned char)(TransmitFrequency)) & 0xff;		// c4 RxFreq LSB
          break;

	  case 4:					// Rx1 NCO freq
	    outbuf[4] = ((unsigned char)(Receive0Frequency >> 24)) & 0xff;	// c1 RxFreq MSB
	    outbuf[5] = ((unsigned char)(Receive0Frequency >> 16)) & 0xff;	// c2
	    outbuf[6] = ((unsigned char)(Receive0Frequency >> 8)) & 0xff;	// c3
	    outbuf[7] = ((unsigned char)(Receive0Frequency)) & 0xff;	// c4 RxFreq LSB
	  break;

	  case 6:					// Rx2 NCO freq
	    outbuf[4] = ((unsigned char)(Receive1Frequency >> 24)) & 0xff; // c1 RxFreq MSB
	    outbuf[5] = ((unsigned char)(Receive1Frequency >> 16)) & 0xff; // c2
	    outbuf[6] = ((unsigned char)(Receive1Frequency >> 8)) & 0xff;	 // c3
	    outbuf[7] = ((unsigned char)(Receive1Frequency)) & 0xff;	 // c4 RxFreq LSB
	  break;

	  case 8:					// Rx3 NCO freq
	  case 10:					// Rx4 NCO freq
	  case 12:					// Rx5 NCO freq
	  case 14:					// Rx6 NCO freq
	  case 16:					// Rx7 NCO freq
	    outbuf[4] = 0;				// c1 RxFreq MSB
	    outbuf[5] = 0;				// c2
	    outbuf[6] = 0;				// c3
	    outbuf[7] = 0;				// c4 RxFreq LSB
	  break;

	  case 18:					// drive level & filt select (if Alex)
	    if (PTTOffMutesTx & (PTTMode == PTTOff))
		outbuf[4] = 0;				// (almost) kill Tx when PTTOff and PTTControlsTx
	    else
		outbuf[4] = TxDrive;			// c1


	    unsigned char RxHPF, TxLPF;

	    RxHPF = AlexRxHPF;
	    if (AlexRxHPF == 0)				// if Rx autotrack
	    {
		if (Receive0Frequency < 1500000)
		  RxHPF = 0x20;				// bypass
		else if (Receive0Frequency < 6500000)
	          RxHPF = 0x10;				// 1.5 MHz HPF
		else if (Receive0Frequency < 9500000)
		  RxHPF = 0x08;				// 6.5 MHz HPF
		else if (Receive0Frequency < 13000000)
		  RxHPF = 0x04;				// 9.5 mHz HPF
		else if (Receive0Frequency < 20000000)
		  RxHPF = 0x01;				// 13 Mhz HPF
		else if (Receive0Frequency < 50000000)
		  RxHPF = 0x02;				// 20 MHz HPF
		else RxHPF = 0x40;			// 6M BPF + LNA
	    }

	    TxLPF = AlexTxLPF;
	    if (AlexTxLPF == 0)				// if Tx autotrack
	    {
		if (TransmitFrequency > 30000000)
		  TxLPF = 0x10;				// 6m LPF
		else if (TransmitFrequency > 19000000)
		  TxLPF = 0x20;				// 10/12m LPF
		else if (TransmitFrequency > 14900000)
		  TxLPF = 0x40;				// 15/17m LPF
		else if (TransmitFrequency > 9900000)
		  TxLPF = 0x01;				// 30/20m LPF
		else if (TransmitFrequency > 4900000)
		  TxLPF = 0x02;				// 60/40m LPF
		else if (TransmitFrequency > 3400000)
		  TxLPF = 0x04;				// 80m LPF
		else TxLPF = 0x08;			// 160m LPF
	    }

	    outbuf[5] = 0x40;				// c2 - Alex Manual filter control enabled
	    outbuf[6] = RxHPF & 0x7f;			// c3 - Alex HPF filter selection
	    outbuf[7] = TxLPF & 0x7f;			// c4 - Alex LPF filter selection
	  break;

	  case 20:					// Hermes input attenuator setting
	    outbuf[4] = 0;				//
	    outbuf[5] = 0x17;				// Not implemented yet, should not be called by
	    outbuf[6] = 0;				// TxControlCycler yet.
	    outbuf[7] = RxAtten;			// 0..31 db attenuator setting (same function as preamp)
	  break;
	
	  case 22:
	    outbuf[4] = 0;				// Register not documented, but zeroed by
	    outbuf[5] = 0;				// PowerSDR...
	    outbuf[6] = 0;				//
	    outbuf[7] = 0;				//
	  break;					

	  default:
	    fprintf(stderr, "Invalid Hermes/Metis register selection: %d\n", RegNum);
	    break;
	};

};


void HermesProxyW::PutTxIQ() 	// Send next control registers plus all zero data into an outbuf
{

        RawBuf_t outbuf;
	int A, B, I, Q;

	outbuf = GetNextTxBuf();	// get a Txbuffer

	if (outbuf == NULL)		// Could not get a Tx buffer
	  return;		 	// WB cannot consume any input

	// format a HPSDR USB frame to send to Hermes.

	TxControlCycler += 2;		// advance to next register bank, modulo
	if (TxControlCycler > 0x14)	// 11 register banks (0..10). Note: Bank 10
	  TxControlCycler = 0;		//    (Hermes attenuator) requires firmware V2.0

	BuildControlRegs(TxControlCycler, outbuf);	// First 8 bytes are the control registers.

	for (int i=0; i<63; i++)			// put 63 IQ samples into frame
        {
	  // Zero out the audio Left and Right channel outputs.

	  outbuf[i*8 + 8] = 0;		// L1 MSB audio channel out
	  outbuf[i*8 + 9] = 0;		// L0 LSB
	  outbuf[i*8 + 10] = 0;		// R1 MSB audio channel out
	  outbuf[i*8 + 11] = 0;		// R0 LSB   

 	  // Zero out the I and Q transmit data

	  outbuf[i*8 + 12] = 0; 	// I1 MSB
	  outbuf[i*8 + 13] = 0;		// I0 LSB
	  outbuf[i*8 + 14] = 0; 	// Q1 MSB
	  outbuf[i*8 + 15] = 0;		// Q0 LSB
	};

	return;
};


RawBuf_t HermesProxyW::GetNextTxBuf()		// get a TXBuf if available
{
	  if (((TxWriteCounter+1) & (NUMTXBUFS - 1)) == TxReadCounter)
	    return NULL;
	 
	  ++TxWriteCounter &= (NUMTXBUFS - 1); // get next writeable buffer

	  return TxBuf[TxWriteCounter];
};


// SendTxIQ() is called on a periodic basis to send Tx Ethernet frames to the 
// Hermes/Metis hardware. It sends 2 USB frames in one Ethernet Frame.

void HermesProxyW::SendTxIQ()
{

	if(TxStop)				// Kill Tx frames if stopped
		return;

	unsigned char ep = 0x2;			// Tx data goes to end point 2

//	fprintf(stderr, "SendTxIQ: TxReadCounter = %d   TxWriteCounter = %d  TxFrameIdleCount = %d\n",
//		TxReadCounter, TxWriteCounter, TxFrameIdleCount); 

	// Time to send one Tx Eth frame (2 x USB frames).
	// If there are at least two buffers in the queue, send then free them.

	bool bufempty = (TxReadCounter == TxWriteCounter);
	bool bufone = ((TxReadCounter+1 & (NUMTXBUFS - 1)) == TxWriteCounter);

	TotalTxBufCount++;

	if ( bufempty | bufone )    // zero or one buffer ready	
	{
	  LostTxBufCount++;		// Not necessarily a lost buffer for hermesWB

	//fprintf(stderr, "SendTxIQ01: TxReadCounter = %d   TxWriteCounter = %d  "
		//"TxFrameIdleCount = %d  bufempty = %d   bufone = %d\n",
		//TxReadCounter, TxWriteCounter, TxFrameIdleCount, bufempty, bufone); 

	}
	else	// two or more buffers ready
	{

	//fprintf(stderr, "SendTxIQ02: TxReadCounter = %d   TxWriteCounter = %d  TxFrameIdleCount = %d\n",
		//TxReadCounter, TxWriteCounter, TxFrameIdleCount); 

	  metis_write(ep, TxBuf[TxReadCounter], 512);	// write one USB frame to metis
	  ++TxReadCounter &= (NUMTXBUFS - 1);		// and free it

	  metis_write(ep, TxBuf[TxReadCounter], 512);	// write next USB frame to metis
	  ++TxReadCounter &= (NUMTXBUFS - 1);		// and free it
	};

	return;
};



