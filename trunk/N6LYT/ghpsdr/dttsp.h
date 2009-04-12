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

/* --------------------------------------------------------------------------*/
/** 
* @brief DttSP main  
* 
* @param argc
* @param argv[]
* 
* @return 
*/
extern void dttsp_main(int argc,char* argv[]);

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
extern void audio_callback (float *input_l, float *input_r, float *output_l,
                            float *output_r, unsigned int nframes);

/* --------------------------------------------------------------------------*/
/** 
* @brief Process the spectrum 
* 
* @param results
* 
* @return 
*/
extern void Process_Spectrum (float *results);
/* --------------------------------------------------------------------------*/
/** 
* @brief Process Panadapter
* 
* @param results
* 
* @return 
*/
extern void Process_Panadapter (float *results);
/* --------------------------------------------------------------------------*/
/** 
* @brief Process Phase 
* 
* @param results
* @param numpoints
* 
* @return 
*/
extern void Process_Phase (float *results, int numpoints);
/* --------------------------------------------------------------------------*/
/** 
* @brief Process scope 
* 
* @param results
* @param numpoints
* 
* @return 
*/
extern void Process_Scope (float *results, int numpoints);
/* --------------------------------------------------------------------------*/
/** 
* @brief Calculate the RX meter 
* 
* @param subrx
* @param mt
* 
* @return 
*/
extern float CalculateRXMeter(unsigned int subrx, int mt);

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the sample rate 
* 
* @param sampleRate
* 
* @return 
*/
extern int SetSampleRate(double sampleRate);
