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
// Version:  November 21, 2012


#ifndef HermesProxy_H
#define HermesProxy_H

#include <gr_block.h>


#define NUMRXIQBUFS	128		// number of receiver IQ buffers in circular queue.
					// Must be integral power of 2 (2,4,8,16,32,64, etc.)

#define RXBUFSIZE	256		// number of floats in one RxIQBuf, #complexes is half
					// Must be integral power of 2 (2,4,8,16,32,64, etc.)

#define NUMTXBUFS	16		// number of transmit buffers in circular queue
					// Must be integral power of 2

#define TXBUFSIZE	512		// number of bytes in one TxBuf
					

typedef float* IQBuf_t;			// IQ buffer type (IQ samples as floats)
typedef unsigned char * RawBuf_t;	// Raw transmit buffer type

enum {  PTTOff,				// PTT disabled
	PTTVox,				// PTT vox mode (examines TxFrame to decide whether to Tx)
	PTTOn };			// PTT force Tx on

class HermesProxy
{

private:

	IQBuf_t RxIQBuf[NUMRXIQBUFS];	// ReceiveIQ buffers
	unsigned RxWriteCounter;	// Which Rx buffer to write to
	unsigned RxReadCounter;		// Which Rx buffer to read from
	unsigned RxWriteFill;		// Fill level of the RxWrite buffer

	RawBuf_t TxBuf[NUMTXBUFS]; 	// Transmit buffers
	unsigned TxWriteCounter;	// Which Tx buffer to write to
	unsigned TxReadCounter;		// Which Tx buffer to read from
	unsigned TxControlCycler;	// Which Tx control register set to send
	unsigned TxFrameIdleCount;	// How long we've gone since sending a TxFrame

	unsigned long LostRxBufCount;	// Lost-buffer counter
	unsigned long TotalRxBufCount;	// Total buffer count (may roll over)

public:

	unsigned ReceiveFrequency;
	unsigned TransmitFrequency;
	int RxSampleRate;
	unsigned char TxDrive;
	unsigned char RxAtten;		// not yet used (requires Hermes firmware V1.9)
	int PTTMode;
	bool RxPreamp;
	bool ADCdither;
	bool ADCrandom;
	bool ADCoverload;
	bool Duplex;
	unsigned char HermesVersion;
	bool PTTOffMutesTx;		// PTT Off mutes the transmitter
	bool PTTOnMutesRx;		// PTT On receiver

	HermesProxy(int);		// constructor
	~HermesProxy();			// destructor

	void SendTxIQ();		// send an IQ buffer to Hermes transmit hardware
	int PutTxIQ(const gr_complex *, int);	// post a transmit TxIQ buffer
	void ScheduleTxFrame(unsigned long);    // Schedule a Tx frame
	RawBuf_t GetNextTxBuf(); // get an empty Tx Buffer

	void UpdateHermes();		// update control registers in Hermes without any Tx data

	void ReceiveRxIQ(unsigned char *); // receive an IQ buffer from Hermes hardware via metis.cc thread
	IQBuf_t GetRxIQ();		// Gnuradio pickup a received RxIQ buffer if available
	IQBuf_t GetNextRxBuf(IQBuf_t);  // return existing out buffer, next output buffer (if needed),
					// or NULL if no new one available
	void UnpackIQ(unsigned char*, IQBuf_t);	// unpack a received IQ sample

	// Not yet implemented

	void SendAudioLR();		// send an LR audio buffer to Hermes hardware
	void ReceiveMicLR();		// receive an LR audio bufer from Hermes hardware

};

#endif  // #ifndef HermesProxy_H

