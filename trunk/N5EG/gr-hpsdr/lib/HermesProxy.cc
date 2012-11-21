/* -*- c++ -*- */
/* 
 * Copyright 2012 Tom McDermott, N5EG
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
// Hermes Proxy
//
// Encapsulates the Hermes module for access/control by GNU Radio.
// Used by the HermesNB (Narrow Band) and HermesWB (Wide Band) modules
// that provide Hermes sink and source to GNU Radio.
//
// Data+Control --> 2 x USB-like 512-byte frames.
// 2 x USB-like frames --> UDP IP-packet.
// UDP IP packet --> Ethernet frame --> Send to Hermes.
// Reverse happens for data from Hermes. 
// See the HPSDR documentation for USB and Ethernet frame formats.
//
// Uses the Metis Ethernet interface module to send/receive Ethernet
// frames to/from Hermes.
//
// HermesNB uses this proxy to convert raw data and control flags
// and send/receive them to Hermes.
//
// Version:  November 16, 2012

#include "HermesProxy.h"
#include "metis.h"
#include <stdio.h>	// for DEBUG PRINTF's


HermesProxy::HermesProxy(int RxFreq)	// constructor
{

	//fprintf(stderr, "HermesProxy constructor called\n");

//	TxControlCycler = 0;
//	TxIQCount = 0;
//	ModuleIndex = -1;	// no hardware module selected
//	NumberReceivers = 1;

	RxSampleRate = 192000;
	TxDrive = 0;
	TxPTT = false;
	RxPreamp = false;
	ADCdither = false;
	ADCrandom = false;


	RxWriteCounter = 0;	//
	RxReadCounter = 0;	// These control the Rx buffers to Gnuradio
	RxWriteFill = 0;	//

	LostRxBufCount = 0;
	TotalRxBufCount = 0;
	
	// allocate the receiver buffers
	for(int i=0; i<NUMRXIQBUFS; i++)
		RxIQBuf[i] = new float[RXBUFSIZE];	// 128 real + 128 imag floats


	ReceiveFrequency = (unsigned)RxFreq; 
	TransmitFrequency = 3500000;

	const char* interface = "eth0";	// Discover Hermes connected to eth0
	metis_discover(interface);

	while (!metis_found())
		;			// wait until Hermes responds

	metis_receive_stream_control(RxStream_NB_On);	// turn on Narrow Band data

	UpdateHermes();			// send control update to Hermes
};


HermesProxy::~HermesProxy()
{
	//fprintf(stderr, "HermesProxy destructor called\n");
	fprintf(stderr, "\nLostRxBufCount = %lu   TotalRxBufCount = %lu\n",
	        LostRxBufCount, TotalRxBufCount);

	metis_receive_stream_control(RxStream_Off);	// stop Hermes data stream
	
	metis_stop_receive_thread();	// stop receive_thread & close socket

	for(int i=0; i<NUMRXIQBUFS; i++)
		delete [] RxIQBuf[i];
}

void HermesProxy::SendTxIQ()		// send Tx Buffer to IQ
{

	// Accumulate samples until >= ?? are available.
	// Wrap into appropriate packet with controls and send to Hermes.
	// Decrement available samples, and send more if >= ?? still available.

};


void HermesProxy::ReceiveRxIQ(unsigned char * inbuf)	// called by metis Rx thread.
{
	//fprintf(stderr,"Raw buffer from Hermes:  ");
	//for(int i=0; i<24; i++) { fprintf(stderr,"%02X:",inbuf[i]); }
	//fprintf(stderr,"\n");

	// Metis Rx thread gives us collection of samples past the Ethernet header
	// consisting of 2 x HPSSDR USB frames.

	// FIXME - Temporary: discard Mic audio from Hermes.
	// TODO  - Handle more than 1 receiver.

	// For 1 Rx, the frame comes in with I2 I1 I0 Q2 Q1 Q0 M1 M0 repeating
	// starting at location 8 through 511. At total of (512-8)/8 = 63 complex pairs.
	// I2 I1 I0 is 24-bit 2's complement format.
	// There are two of the USB HPSDR frames in the received ethernet buffer.
	// A buffer of 126 complex pairs is about
	//	0.6 milliseconds at 192,000 sample rate
	//	2.4 milliseconds at 48,000 sample rate


	// We get 126 complex samples per input buffer, but Gnuradio seems happier
	// with 128 complex samples per output buffer; QtGUI is smoother, and the
	// work pipeline is faster.
	//
 	//    RxWriteCounter - the current Rx buffer we are writing to
	//    RxWriteFill    - #floats we have written to the current Rx buffer (0..255)
	//    RxReadCounter  - the Rx buffer that gnuradio can read
	//



	unsigned char* inptr;		// pointer along the input buffer
	int outctr = 0;			// counter along output buffer

	unsigned char c0 = inbuf[3];	// control register 0
	unsigned char c1 = inbuf[4];	// control register 1
	unsigned char c2 = inbuf[5];	// control register 2
	unsigned char c3 = inbuf[6];	// control register 3
	unsigned char c4 = inbuf[7];	// control register 4

	int I,Q;			// temporary I and Q samples as int
	IQBuf_t outbuf;			// RxWrite output buffer selector
	
	outbuf = RxIQBuf[RxWriteCounter];	// initialize buffer pointer

	TotalRxBufCount++;

	// check for proper frame sync
	if(inbuf[0] == 0x7f && inbuf[1] == 0x7f && inbuf[2] == 0x7f)
	{
		if(c0 == 0) 
		{
		  if(c1 & 0x01)
		    ADCoverload = true;
		  else
		    ADCoverload = false;

		  HermesVersion = c4;
		}

		// Use write and read counters to select from the Rx buffers,
		// these are circular.

		if(RxWriteFill & RXBUFSIZE)  // if we need a new buffer
		{
		  if (((RxWriteCounter+1) & (NUMRXIQBUFS - 1)) == RxReadCounter)
		  {
			LostRxBufCount++;	// Rx Buffers are full. Throw away the data
			return;
		  }
		  ++RxWriteCounter &= (NUMRXIQBUFS - 1); // get next writeable buffer
		  outbuf = RxIQBuf[RxWriteCounter];
		  RxWriteFill = 0;
		}

		inptr = inbuf + 8;		// skip received ethernet HPSDR header

		// Convert 24-bit 2's complement integer samples to float with
		// maximum value of +1.0 and minimum of -1.0

		for (int i=0; i<63; i++)	// 63 complex samples per USB frame
		{
		  I = (int)(((signed char)*inptr)<<16);
		  inptr++;
		  I += ((int)((unsigned char)*inptr)<<8);
		  inptr++;
		  I += (int)((unsigned char)*inptr);
		  inptr++;
		  if(I<0) I = -(~I + 1);
		  outbuf[RxWriteFill++] = (float)I/838860.7;
		  Q = (int)(((signed char)*inptr)<<16);
		  inptr++;
		  Q += ((int)((unsigned char)*inptr)<<8);
		  inptr++;
		  Q += (int)((unsigned char)*inptr);
		  inptr++;
		  if(Q<0) Q = -(~Q + 1);
		  outbuf[RxWriteFill++] = (float)Q/838860.7;
		  inptr+= 2;	//skip Mic samples

		  if(RxWriteFill & RXBUFSIZE)  // Did we fill buffer? Need another buffer.
		  {
		    if(((RxWriteCounter+1) & (NUMRXIQBUFS - 1)) == RxReadCounter)
		    {		    
		      LostRxBufCount++;	// Can't get next Rxbuffer (full). Throw away remaining inbuf.
		      return;
		    }			    
		    ++RxWriteCounter &= (NUMRXIQBUFS - 1);	// get next writeable buffer
		    outbuf = RxIQBuf[RxWriteCounter];
		    RxWriteFill = 0;
		  }
		} 

		inptr = inbuf + 520;
		for (int i=63; i<126; i++)
		{
		  I = (int)(((signed char)*inptr)<<16);
		  inptr++;
		  I += ((int)((unsigned char)*inptr)<<8);
		  inptr++;
		  I += (int)((unsigned char)*inptr);
		  inptr++;
		  if(I<0) I = -(~I + 1);
		  outbuf[RxWriteFill++] = (float)I/838860.7;
		  Q = (int)(((signed char)*inptr)<<16);
		  inptr++;
		  Q += ((int)((unsigned char)*inptr)<<8);
		  inptr++;
		  Q += (int)((unsigned char)*inptr);
		  inptr++;
		  if(Q<0) Q = -(~Q + 1);
		  outbuf[RxWriteFill++] = (float)Q/838860.7;
		  inptr+= 2;	//skip Mic samples

		  if(RxWriteFill & RXBUFSIZE)  // if we filled the 256 float buffer
		  {
		    if (((RxWriteCounter+1) & (NUMRXIQBUFS - 1)) == RxReadCounter)
		    {			   
		      LostRxBufCount++;	// Can't get next Rxbuffer (full). Throw away remaining inbuf
		      return;
		    }  
		    ++RxWriteCounter &= (NUMRXIQBUFS - 1);	// get next writeable buffer
		    outbuf = RxIQBuf[RxWriteCounter];
		    RxWriteFill = 0;
		  }
		}
	}
	else
		fprintf(stderr, "HermesProxy: EP6 received from Hermes failed sync header check.\n");
	return;
};

IQBuf_t HermesProxy::GetRxIQ()		// called by HermesNB to pickup any RxIQ
{
	if(RxReadCounter == RxWriteCounter)
		return NULL;				// empty - no buffers to return

	IQBuf_t ReturnBuffer = RxIQBuf[RxReadCounter];		// get the next receiver buffer
	++RxReadCounter &= (NUMRXIQBUFS - 1);		// increment read counter modulo

	return ReturnBuffer;
};

void HermesProxy::UpdateHermes()	// send a set of control registers to hardware with naught Tx data
{
	unsigned char buffer[512];	// dummy up a USB HPSDR buffer;
	for(int i=0; i<512; i++)
		buffer[i] = 0;

	int length = 512;		// metis_write ignores this value
	unsigned char ep = 0x02;	// all Hermes data is sent to end point 2
	unsigned char Speed = 0;	// Rx sample rate
	unsigned char RxCtrl = 0;	// Rx controls

	if(RxSampleRate == 192000)
		Speed = 0x02;
	if(RxSampleRate == 96000)
		Speed = 0x01;
	if(RxSampleRate == 48000)
		Speed = 0x00;

	if(RxPreamp)
		RxCtrl += 0x04;
	if(ADCdither)
		RxCtrl += 0x08;
	if(ADCrandom)
		RxCtrl += 0x10;

	// metis_write requires two 512 byte USB buffers to make one ethernet write to the hardware

	buffer[0] = buffer[1] = buffer[2] = 0x7f;	// HPSDR sync
	buffer[3] = 0;					// c0=0 --> RxControls, no PTT
	buffer[4] = Speed;				// c1
	buffer[5] = 0;					// c2
	buffer[6] = RxCtrl;				// c3
	buffer[7] = 0;					// c4 (0x0 = one Rx, 0x7 = eight Rx)

	metis_write(ep, buffer, length);


	buffer[0] = buffer[1] = buffer[2] = 0x7f;	// HPSDR sync
	buffer[3] = 0x02;				// c0=2 --> Tx and Rx[0] Frequency, no PTT
							// this is only Rx[0] if one Rx selected and not in duplex.
	buffer[4] = (unsigned char)(ReceiveFrequency >> 24) & 0xff;	// c1 RxFreq MSB
	buffer[5] = (unsigned char)(ReceiveFrequency >> 16) & 0xff;	// c2
	buffer[6] = (unsigned char)(ReceiveFrequency >> 8) & 0xff;	// c3
	buffer[7] = (unsigned char)(ReceiveFrequency) & 0xff;		// c4 RxFreq LSB

	metis_write(ep, buffer, length);

	return;
}

// TODO not yet implemented 
void HermesProxy::PutTxIQ(unsigned char * buffer) // called by HermesNB to post some TxIQ
{
	// need to keep track of how many samples we've been given by GnuRadio and block it into
	// 63 sample sets. Metis write will take two sets and make one Ethernet frame then send.
	// Metis write expects the samples to be pre-formatted for Hermes.
	// Metis write also expects the command registers to be pre-formatted.

//	unsigned char ep = 0x02;		// data is sent to end point 2

//	metis_write(ep, char* buffer,int length);

	return;
};

// TODO not yet implemented
void HermesProxy::SendAudioLR() {};	// send an LR audio buffer to Hermes hardware
void HermesProxy::ReceiveMicLR() {};	// receive an LR audio bufer from Hermes hardware


