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
// Version:  November 24, 2012

#include "HermesProxy.h"
#include "metis.h"
#include <stdio.h>	// for DEBUG PRINTF's


HermesProxy::HermesProxy(int RxFreq)	// constructor
{

	//fprintf(stderr, "HermesProxy constructor called\n");


	RxSampleRate = 192000;
	TxDrive = 0x00;		// default to (almost) off
	PTTMode = PTTOff;
	RxPreamp = false;
	ADCdither = false;
	ADCrandom = false;
	Duplex = true;		// Allows TxF to program separately from RxF
	PTTOffMutesTx = true;   // PTT Off mutes the transmitter
	PTTOnMutesRx = true;	// PTT On mutes receiver


	RxWriteCounter = 0;	//
	RxReadCounter = 0;	// These control the Rx buffers to Gnuradio
	RxWriteFill = 0;	//

	TxWriteCounter = 0;	//
 	TxReadCounter = 0;	// These control the Tx buffers to Hermes
	TxControlCycler = 0;	//
	TxFrameIdleCount = 0;	//

	LostRxBufCount = 0;
	TotalRxBufCount = 0;
	
	// allocate the receiver buffers
	for(int i=0; i<NUMRXIQBUFS; i++)
		RxIQBuf[i] = new float[RXBUFSIZE];

	// allocate the transmit buffers
	for(int i=0; i<NUMTXBUFS; i++)
		TxBuf[i] = new unsigned char[TXBUFSIZE];

	ReceiveFrequency = (unsigned)RxFreq; 
	TransmitFrequency = ReceiveFrequency;		// initialize transceive

	const char* interface = "eth0";	// Discover Hermes connected to eth0
	metis_discover(interface);

	while (!metis_found())
		;			// wait until Hermes responds

	metis_receive_stream_control(RxStream_NB_On);	// turn on Narrow Band data
};


HermesProxy::~HermesProxy()
{
	//fprintf(stderr, "HermesProxy destructor called\n");
	fprintf(stderr, "\nLostRxBufCount = %lu   TotalRxBufCount = %lu\n",
	        LostRxBufCount, TotalRxBufCount);

	metis_receive_stream_control(RxStream_Off);	// stop Hermes data stream
	
	metis_stop_receive_thread();	// stop receive_thread & close socket

	for(int i=0; i<NUMTXBUFS; i++)
		delete [] TxBuf[i];

	for(int i=0; i<NUMRXIQBUFS; i++)
		delete [] RxIQBuf[i];
}


// ********** Routines to receive data from hardware and send to Gnuradio ****************

void HermesProxy::ReceiveRxIQ(unsigned char * inbuf)	// called by metis Rx thread.
{
	//fprintf(stderr,"Raw buffer from Hermes:  ");
	//for(int i=0; i<24; i++) { fprintf(stderr,"%02X:",inbuf[i]); }
	//fprintf(stderr,"\n");

	// Metis Rx thread gives us collection of samples past the Ethernet header
	// consisting of 2 x HPSSDR USB frames.

	// TODO - Handle Mic audio from Hermes.
	// TODO - Handle more than 1 receiver.

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

	unsigned char c0 = inbuf[3];	// control register 0
	unsigned char c1 = inbuf[4];	// control register 1
	unsigned char c2 = inbuf[5];	// control register 2
	unsigned char c3 = inbuf[6];	// control register 3
	unsigned char c4 = inbuf[7];	// control register 4

	IQBuf_t outbuf;			// RxWrite output buffer selector
	
	outbuf = RxIQBuf[RxWriteCounter];	// initialize buffer pointer

	TotalRxBufCount++;

	ScheduleTxFrame(TotalRxBufCount); // Schedule a Tx ethernet frame to Hermes if ready.

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

		if ((outbuf = GetNextRxBuf(outbuf)) == NULL)
			return;			// all buffers full. Throw away data

		// Convert 24-bit 2's complement integer samples to float with
		// maximum value of +1.0 and minimum of -1.0
		// skip sync/register headers (i=0 and i=64)

		for (int i=1; i<128; i++)	// 63 complex samples per USB frame * 2 frames
		{
		  if (i==64)			// skip header for 2nd frame
		    continue;

		  UnpackIQ(inbuf + i*8, outbuf);  // convert 2's comp to float and place in outbuf

		  if ((outbuf = GetNextRxBuf(outbuf)) == NULL)  // if needed, get next buffer
			return;			// all buffers full. Throw away data
		} 
	}
	else
		fprintf(stderr, "HermesProxy: EP6 received from Hermes failed sync header check.\n");
	return;
};


void HermesProxy::ScheduleTxFrame(unsigned long RxBufCount) // Transmit a ethernet frame to Hermes if ready.
{
	if(RxSampleRate == 48000)	// one Tx frame for each Rx frame
		SendTxIQ();

	if(RxSampleRate == 96000)	// one Tx frame for each two Rx frames
	  if((RxBufCount & 0x1) == 0)
		SendTxIQ();
	
	if(RxSampleRate == 192000)	// on Tx frame for each four Tx frames
	  if((RxBufCount & 0x3) == 0)
		SendTxIQ();

	return;
};


IQBuf_t HermesProxy::GetNextRxBuf(IQBuf_t current_outbuf) // get new Rx buffer if we've filled current one
{
	if(RxWriteFill & RXBUFSIZE)  // need a new buffer?
	{
	  if (((RxWriteCounter+1) & (NUMRXIQBUFS - 1)) == RxReadCounter)
	  {
		LostRxBufCount++;	// No Rx Buffers available. Throw away the data
		return NULL;
	  }
	  ++RxWriteCounter &= (NUMRXIQBUFS - 1); // get next writeable buffer
	  RxWriteFill = 0;
	  return RxIQBuf[RxWriteCounter];
	}
	else				// don't need a new buffer
	  return current_outbuf;
};

void HermesProxy::UnpackIQ(unsigned char* inptr, IQBuf_t outbuf)
{
	// Unpack 8 bytes in the HPSDR USB frame to I and Q.
	// 24 bit 2's complement --> float (-1.0 ... +1.0)
	// Ignore the 16-bit Mic sample from Hermes/Metis

	int I, Q;

	I = (int)(((signed char)*inptr)<<16);
	inptr++;
	I += ((int)((unsigned char)*inptr)<<8);
	inptr++;
	I += (int)((unsigned char)*inptr);
	inptr++;
	if(I<0) I = -(~I + 1);
	if ((PTTOnMutesRx) & (PTTMode == PTTOn))
	  outbuf[RxWriteFill++] = 0.0;
	else
	  outbuf[RxWriteFill++] = (float)I/8388607.0;

	Q = (int)(((signed char)*inptr)<<16);
	inptr++;
	Q += ((int)((unsigned char)*inptr)<<8);
	inptr++;
	Q += (int)((unsigned char)*inptr);
	inptr++;
	if(Q<0) Q = -(~Q + 1);
	if ((PTTOnMutesRx) & (PTTMode == PTTOn))
	  outbuf[RxWriteFill++] = 0.0;
	else
	  outbuf[RxWriteFill++] = (float)Q/8388607.0;

	// inptr+= 2;	//skip Mic samples
};

IQBuf_t HermesProxy::GetRxIQ()		// called by HermesNB to pickup any RxIQ
{
	if(RxReadCounter == RxWriteCounter)
		return NULL;				// empty - no buffers to return

	IQBuf_t ReturnBuffer = RxIQBuf[RxReadCounter];	// get the next receiver buffer
	++RxReadCounter &= (NUMRXIQBUFS - 1);		// increment read counter modulo

	return ReturnBuffer;
};



// ************  Routines to send data from gnuradio to the transmitter ***************

void HermesProxy::UpdateHermes()	// send a set of control registers to hardware with naught Tx data
{

	// DEBUG
	//fprintf(stderr, "UpdateHermes called\n");

	unsigned char buffer[512];	// dummy up a USB HPSDR buffer;
	for(int i=0; i<512; i++)
		buffer[i] = 0;

	int length = 512;		// metis_write ignores this value
	unsigned char ep = 0x02;	// all Hermes data is sent to end point 2
	unsigned char Speed = 0;	// Rx sample rate
	unsigned char RxCtrl = 0;	// Rx controls
	unsigned char Ctrl4 = 0;	// Rx number and duplex

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

	if(Duplex)
		Ctrl4 |= 0x04;

	// metis_write requires two 512 byte USB buffers to make one ethernet write to the hardware

	buffer[0] = buffer[1] = buffer[2] = 0x7f;	// HPSDR sync
	buffer[3] = 0x00;					// c0=0 --> RxControls, no PTT
	buffer[4] = Speed;				// c1
	buffer[5] = 0;					// c2
	buffer[6] = RxCtrl;				// c3
	buffer[7] = Ctrl4;				// c4 (0x0 = one Rx, 0x7 = eight Rx)

	metis_write(ep, buffer, length);


	buffer[0] = buffer[1] = buffer[2] = 0x7f;	// HPSDR sync
	buffer[3] = 0x02;				// c0=2 --> Tx Frequency, no PTT
							
	buffer[4] = (unsigned char)(TransmitFrequency >> 24) & 0xff;	// c1 RxFreq MSB
	buffer[5] = (unsigned char)(TransmitFrequency >> 16) & 0xff;	// c2
	buffer[6] = (unsigned char)(TransmitFrequency >> 8) & 0xff;	// c3
	buffer[7] = (unsigned char)(TransmitFrequency) & 0xff;		// c4 RxFreq LSB

	metis_write(ep, buffer, length);


	buffer[0] = buffer[1] = buffer[2] = 0x7f;	// HPSDR sync
	buffer[3] = 0x04;				// c0=4 --> Rx1 Frequency, no PTT
	buffer[4] = (unsigned char)(ReceiveFrequency >> 24) & 0xff;	// c1 RxFreq MSB
	buffer[5] = (unsigned char)(ReceiveFrequency >> 16) & 0xff;	// c2
	buffer[6] = (unsigned char)(ReceiveFrequency >> 8) & 0xff;	// c3
	buffer[7] = (unsigned char)(ReceiveFrequency) & 0xff;		// c4 RxFreq LSB

	metis_write(ep, buffer, length);

	buffer[0] = buffer[1] = buffer[2] = 0x7f;	// HPSDR sync
	buffer[3] = 0x12;				// c0=12 --> TxDrive
	buffer[4] = TxDrive;
	buffer[5] = 0x00;				// c2
	buffer[6] = 0x00;				// c3
	buffer[7] = 0x00;				// c4 RxFreq LSB

	return;
}


// hermesNB calls this routine to give IQ data from the block input connector to the proxy.
// Packs transformed data into one HPSDR USB buffer with control registers.

int HermesProxy::PutTxIQ(const gr_complex * in, int nsamples) // called by HermesNB to give us IQ data to send
{
	unsigned char Speed = 0;	// Rx sample rate
	unsigned char RxCtrl = 0;	// Rx controls
	unsigned char Ctrl4 = 0;	// Rx register C4 control

        RawBuf_t outbuf;
	int A, B, I, Q;

	outbuf = GetNextTxBuf();	// get a Txbuffer

	if (outbuf == NULL)		// Could not get a Tx buffer
	  return 0;		 	// Tell hermeNB we didn't consume any input

	TxControlCycler += 2;		// advance to next register bank, modulo
	if (TxControlCycler >= 20)	// 10 register banks (0..9). Note: not yet using bank 10
	  TxControlCycler = 0;		//    (Hermes attenuator) which requires firmware V1.9

	// format a HPSDR USB frame to send to Hermes.
	// First 8 bytes are the control registers.
	// Next 63 * 8 bytes are the IQ data and the Audio data.
	// TODO - the L/R audio data to Hermes is not implemented yet.

	outbuf[0] = outbuf[1] = outbuf[2] = 0x7f;	// HPSDR USB sync

	outbuf[3] = TxControlCycler;		// C0 Control Register (Bank Sel + PTT)
	if (PTTMode == PTTOn)
	  outbuf[3] |= 0x01;				// set MOX bit

	switch(TxControlCycler)
	{
	  case 0:					// general config registers
	    if(RxSampleRate == 192000)
		Speed = 0x02;
	    if(RxSampleRate == 96000)
		Speed = 0x01;
	    if(RxSampleRate == 48000)
		Speed = 0x00;

	    if(Duplex)
		Ctrl4 |= 0x04;

	    if(RxPreamp)
		RxCtrl += 0x04;
	    if(ADCdither)
		RxCtrl += 0x08;
	    if(ADCrandom)
		RxCtrl += 0x10;

	    outbuf[4] = Speed;				// C1
	    outbuf[5] = 0;				// C2
	    outbuf[6] = RxCtrl;				// C3
	    outbuf[7] = Ctrl4;				// C4 - #Rx, Duplex

            break;

	  case 2:					// Tx NCO freq (and Rx NCO for special case)
	    outbuf[4] = (unsigned char)(TransmitFrequency >> 24) & 0xff;	// c1 RxFreq MSB
	    outbuf[5] = (unsigned char)(TransmitFrequency >> 16) & 0xff;	// c2
	    outbuf[6] = (unsigned char)(TransmitFrequency >> 8) & 0xff;	// c3
	    outbuf[7] = (unsigned char)(TransmitFrequency) & 0xff;		// c4 RxFreq LSB
           break;

	  case 4:					// Rx1 NCO freq (normal case)
	    outbuf[4] = (unsigned char)(ReceiveFrequency >> 24) & 0xff;	// c1 RxFreq MSB
	    outbuf[5] = (unsigned char)(ReceiveFrequency >> 16) & 0xff;	// c2
	    outbuf[6] = (unsigned char)(ReceiveFrequency >> 8) & 0xff;	// c3
	    outbuf[7] = (unsigned char)(ReceiveFrequency) & 0xff;		// c4 RxFreq LSB
	  break;

	  case 6:					// Rx2 NCO freq
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
		outbuf[4] = TxDrive;

	    outbuf[5] = 0;				// Apollo selections
	    outbuf[6] = 0;				// Alex filter selections 1
	    outbuf[7] = 0;				// Alex filter selections 2
	  break;

	  case 20:					// Hermes input attenuator setting
	    outbuf[4] = RxAtten;			// 0..31 dB attenuator setting (not used yet)
	    outbuf[5] = 0;				// Not implemented yet, should not be called by
	    outbuf[6] = 0;				// TxControlCycler yet.
	    outbuf[7] = 0;				// If it is called, then
	    // break;					// fall through to error message for now...

	  default:
	    fprintf(stderr, "Invalid Hermes/Metis register selection: %d\n", TxControlCycler);
	    break;
	};

	for (int i=0; i<nsamples; i++)			// put 63 IQ samples into frame
        {
	  outbuf[i*8 + 8] = 0;		// L1 MSB audio channel out
	  outbuf[i*8 + 9] = 0;		// L0 LSB
	  outbuf[i*8 + 10] = 0;		// R1 MSB audio channel out
	  outbuf[i*8 + 11] = 0;		// R0 LSB

 	  // convert float to 2's complement 16-bit

	  A = (int)(in[i].real() * 32767.0);	// scale to 16 bits
	  B = (int)(in[i].imag() * 32767.0);	// scale to 16 bits
          I = (unsigned int)A;
	  Q = (unsigned int)B;

	  if(PTTOffMutesTx & (PTTMode == PTTOff))	// Kill Tx if in Rx and PTTControls the Tx
	  {
	    I = 0;
	    Q = 0;
	  };

	  outbuf[i*8 + 12] = (unsigned char)((I & 0xff00) >> 8); // I1 MSB
	  outbuf[i*8 + 13] = (unsigned char)(I & 0xff);		 // I0 LSB
	  outbuf[i*8 + 14] = (unsigned char)((Q & 0xff00) >> 8); // Q1 MSB
	  outbuf[i*8 + 15] = (unsigned char)(Q & 0xff);		 // Q0 LSB
        };

	if(PTTMode == PTTVox)		// if we are in Vox mode, check frame IQ contents
	{
          bool activity = false;

	  for (int i=0; i<nsamples; i++)	 // if any IQ sample is nonzero (VOX) then key Tx
	    if ((outbuf[i*8 + 12] != 0) ||  
	        (outbuf[i*8 + 13] != 0) ||
	        (outbuf[i*8 + 14] != 0) ||
	        (outbuf[i*8 + 15] != 0) )
	    {
		activity = true;
		break;
	    };

	    if(activity)
		outbuf[3] |= 1;		// enable MOX PTT	    
	};

//	fprintf(stderr, "PutTxIQ: Consumed %d samples, TxControlCycler = %d\n",
//		nsamples, TxControlCycler);

	return nsamples;
};

RawBuf_t HermesProxy::GetNextTxBuf()		// get a TXBuf if available
{
	  if (((TxWriteCounter+1) & (NUMTXBUFS - 1)) == TxReadCounter)
	    return NULL;
	 
	  ++TxWriteCounter &= (NUMTXBUFS - 1); // get next writeable buffer
	  return TxBuf[TxWriteCounter];
};


// SendTxIQ() is called on a periodic basis to send Tx Ethernet frames to the 
// Hermes/Metis hardware. The Hermes hardware does not have any method to indicate
// when it wants a frame, nor any back pressure mechanism. We derive the Tx timing by
// counting the Rx frames Hermes is sending to us. This depends on the Rx Sample rate
// and the number of receivers because the Tx sample rate is fixed at 48000.
//
// With one Rx and a Rx sample rate of 48000, we should send one Tx frame for every
// one Rx frame we receive.
//
// With one Rx and a Rx sample rate of 192000, we should send one Tx frame for every
// four Rx frames we receive.
//
// If no data to transmit, periodically send a frame so that basic control registers
// get updated. 
//

void HermesProxy::SendTxIQ()
{
	unsigned char ep = 0x2;			// Tx data goes to end point 2

	//fprintf(stderr, "SendTxIQ: TxReadCounter = %d   TxWriteCounter = %d  TxFrameIdleCount = %d\n",
	//	TxReadCounter, TxWriteCounter, TxFrameIdleCount); 

	// Time to send one Tx Eth frame (2 x USB frames).
	// If there are at least two buffers in the queue, send then free them.
	// Otherwise, call UpdateHermes() periodically so that control registers get updated.

	bool bufempty = (TxReadCounter == TxWriteCounter);
	bool bufone = ((TxReadCounter+1 & (NUMTXBUFS - 1)) == TxWriteCounter);
 

	if ( bufempty | bufone )    // zero or one buffer ready	
	{

	//fprintf(stderr, "SendTxIQ01: TxReadCounter = %d   TxWriteCounter = %d  TxFrameIdleCount = %d  bufempty = %d   bufone = %d\n",
		//TxReadCounter, TxWriteCounter, TxFrameIdleCount, bufempty, bufone); 

	  if(TxFrameIdleCount++ > 400)
	  {
	    UpdateHermes();             // Force basic register update if 
	    TxFrameIdleCount = 0;	//   we've not sent for about 1 second
	  }
	}
	else	// two or more buffers ready
	{

	//fprintf(stderr, "SendTxIQ02: TxReadCounter = %d   TxWriteCounter = %d  TxFrameIdleCount = %d\n",
		//TxReadCounter, TxWriteCounter, TxFrameIdleCount); 

	  metis_write(ep, TxBuf[TxReadCounter], 512);	// write one USB frame to metis
	  ++TxReadCounter &= (NUMTXBUFS - 1);		// and free it

	  metis_write(ep, TxBuf[TxReadCounter], 512);	// write next USB frame to metis
	  ++TxReadCounter &= (NUMTXBUFS - 1);		// and free it

	  TxFrameIdleCount = 0;				// have just sent a frame
	};

	return;
};


// TODO not yet implemented
void HermesProxy::SendAudioLR() {};	// send an LR audio buffer to Hermes hardware
void HermesProxy::ReceiveMicLR() {};	// receive an LR audio bufer from Hermes hardware


