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

// HermesProxy.h
//
// Proxy for Hermes board. Each HermesNB module communicates with
// only one hardware module. Multiple hardware modules need to use
// multiple instantiations of the HermesNB within GNURadio.
// Note: multiple receivers on one Hermes is not implemented. 
//
// Version:  November 16, 2012


#ifndef HermesProxy_H
#define HermesProxy_H

#define NUMRXIQBUFS	128		// number of receiver IQ buffers in circular queue.
					// Must be integral power of 2 (2,4,8,16,32,64, etc.)

#define RXBUFSIZE	256		// number of floats in one RxIQBuf, #complexes is half
					// Must be integral power of 2 (2,4,8,16,32,64, etc.)

typedef float* IQBuf_t;			// IQ buffer type (IQ samples as floats)

class HermesProxy
{

private:

	IQBuf_t RxIQBuf[NUMRXIQBUFS];	// ReceiveIQ buffers
	unsigned RxWriteCounter;	// Which Rx buffer to write to
	unsigned RxReadCounter;		// Which Rx buffer to read from
	unsigned RxWriteFill;		// Fill level of the RxWrite buffer

	unsigned long LostRxBufCount;	// Lost-buffer counter
	unsigned long TotalRxBufCount;	// Total buffer count (will roll over often)

public:

	unsigned ReceiveFrequency;
	unsigned TransmitFrequency;
	int RxSampleRate;
	unsigned char TxDrive;
	bool TxPTT;
	bool RxPreamp;
	bool ADCdither;
	bool ADCrandom;
	bool ADCoverload;
	unsigned char HermesVersion;

	HermesProxy(int);		// constructor
	~HermesProxy();			// destructor
	void SendTxIQ();		// send an IQ buffer to Hermes transmit hardware
	void ReceiveRxIQ(unsigned char * buffer); // receive an IQ buffer from Hermes hardware via metis.cc thread
	void UpdateHermes();		// update control registers in Hermes without any Tx data
	IQBuf_t GetRxIQ();		// Gnurario pickup a received RxIQ buffer if available

	// Not yet implemented
	void PutTxIQ(unsigned char * buffer);	// post a transmit TxIQ buffer
	void SendAudioLR();		// send an LR audio buffer to Hermes hardware
	void ReceiveMicLR();		// receive an LR audio bufer from Hermes hardware

};

#endif  // #ifndef HermesProxy_H

