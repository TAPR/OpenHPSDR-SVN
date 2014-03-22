/** 
* @file dttsp.h
* @brief DttSP interface definitions
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-11
*/
// dttsp.h

/* Copyright (C) 
* 2009 - John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
* 
*/



//
// what we know about DttSP
//

#define MAXRX 4

enum TRXMODE { RX, TX };

enum SDRMODE {
  LSB,                          //  0
  USB,                          //  1
  DSB,                          //  2
  CWL,                          //  3
  CWU,                          //  4
  FMN,                          //  5
  AM,                           //  6
  DIGU,                         //  7
  SPEC,                         //  8
  DIGL,                         //  9
  SAM,                          // 10
  DRM                           // 11
};

enum RUNMODE {
  RUN_MUTE, RUN_PASS, RUN_PLAY, RUN_SWCH
};

#ifdef __cplusplus
 extern "C" {
#endif 

/* --------------------------------------------------------------------------*/
/** 
* @brief Setup_SDR  
* 
* @return
*/
extern void Setup_SDR();

/* --------------------------------------------------------------------------*/
/** 
* @brief Release_Update  
* 
* @return
*/
extern void Release_Update();

/* --------------------------------------------------------------------------*/
/** 
* @brief SetThreadCom  
*
* @param thread
* 
* @return
*/
extern void SetThreadCom(int thread);

/* --------------------------------------------------------------------------*/
/** 
* @brief DttSP audio callback 
* 
* @param input_l
* @param input_r
* @param output_l
* @param output_r
* @param nframes
* 
* @return 
*/
extern void Audio_Callback (float *input_l, float *input_r, float *output_l,
                            float *output_r, unsigned int nframes, int thread);

/* --------------------------------------------------------------------------*/
/** 
* @brief Process the spectrum 
* 
* @param thread
* @param results
* 
* @return 
*/
extern void Process_Spectrum (int thread, float *results);
/* --------------------------------------------------------------------------*/
/** 
* @brief Process Panadapter
* 
* @param thread
* @param results
* 
* @return 
*/
extern void Process_Panadapter (int thread, float *results);
/* --------------------------------------------------------------------------*/
/** 
* @brief Process Phase 
* 
* @param thread
* @param results
* @param numpoints
* 
* @return 
*/
extern void Process_Phase (int thread, float *results, int numpoints);
/* --------------------------------------------------------------------------*/
/** 
* @brief Process scope 
* 
* @param thread
* @param results
* @param numpoints
* 
* @return 
*/
extern void Process_Scope (int thread, float *results, int numpoints);
/* --------------------------------------------------------------------------*/
/** 
* @brief Calculate the RX meter 
* 
* @param subrx
* @param mt
* 
* @return 
*/
extern float CalculateRXMeter(int thread,unsigned int subrx, int mt);

extern float CalculateTXMeter(int thread, int mt);
/* --------------------------------------------------------------------------*/
/** 
* @brief Set the sample rate 
* 
* @param sampleRate
* 
* @return 
*/
extern int SetSampleRate(double sampleRate);

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the Oscillator frequency 
* 
* @param frequency
* 
* @return 
*/
extern int SetRXOsc(unsigned int thread, unsigned subrx, double freq);
extern int SetTXOsc(unsigned int thread, double freq);

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the receiver output gain
* 
* @param gain
* 
* @return 
*/
extern int SetRXOutputGain(unsigned int thread, unsigned subrx, double gain);

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the receiver pan position
* 
* @param pos
* 
* @return 
*/
extern int SetRXPan(unsigned int thread, unsigned subrx, float pos);

extern int SetRXFilter(unsigned int thread, unsigned int subrx, double low_frequency, double high_frequency);

extern int SetTXFilter(unsigned int thread, double low_frequency, double high_frequency);

extern void SetRingBufferOffset(unsigned int thread, int offset);

extern int SetMode(unsigned int thread, unsigned int subrx, SDRMODE m);

extern void SetTRX(unsigned int thread, TRXMODE setit);

extern int SetSubRXSt(unsigned int thread, unsigned int subrx, unsigned int state);

extern void SetThreadProcessingMode(unsigned int thread, RUNMODE runmode);

extern void SetRXAGC(unsigned int thread,unsigned int subrx,int agc);
extern void SetNR(unsigned int thread,unsigned int subrx,int state);
extern void SetNB(unsigned int thread,unsigned int subrx,int state);
extern void SetSDROM(unsigned int thread,unsigned int subrx,int state);
extern void SetANF(unsigned int thread,unsigned int subrx,int state);
extern void SetANFvals(unsigned int thread,unsigned int subrx,int taps,int delay,float gain,float leakage);
extern void SetNRvals(unsigned int thread,unsigned int subrx,int taps,int delay,float gain,float leakage);
extern void SetNBvals(unsigned int thread,unsigned int subrx,double threshold);
extern void SetSDROMvals(unsigned int thread,unsigned int subrx,double threshold);
extern void SetRXDCBlock(unsigned int thread,unsigned int subrx,int state);
extern void SetSquelchVal(unsigned int thread,unsigned int subrx,float value);
extern void SetSquelchState(unsigned int thread,unsigned int subrx,int state);
extern void SetSpectrumPolyphase(unsigned int thread,unsigned int subrx,int state);

extern int reset_for_buflen (unsigned int thread, int new_buflen);

#ifdef __cplusplus
 }
#endif 
