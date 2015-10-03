/*********************************************************************
 * FractResampler.cpp: implementation of the CFractResampler class.
 *
 * This class implements a fractional resampler that can be used to
 * convert between different sample rates.  A windowes sinc interpolator
 * is used to create samples "in between" input samples.
 *
 * History:
 *      2010-09-15  Initial creation MSW
 *      2011-03-27  Initial release
 *********************************************************************/

//==========================================================================================
// + + +   This Software is released under the "Simplified BSD License"  + + +
//Copyright 2010 Moe Wheatley. All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are
//permitted provided that the following conditions are met:
//
//   1. Redistributions of source code must retain the above copyright notice, this list of
//	  conditions and the following disclaimer.
//
//   2. Redistributions in binary form must reproduce the above copyright notice, this list
//	  of conditions and the following disclaimer in the documentation and/or other materials
//	  provided with the distribution.
//
//THIS SOFTWARE IS PROVIDED BY Moe Wheatley ``AS IS'' AND ANY EXPRESS OR IMPLIED
//WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL Moe Wheatley OR
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//The views and conclusions contained in the software and documentation are those of the
//authors and should not be interpreted as representing official policies, either expressed
//or implied, of Moe Wheatley.
//==========================================================================================

#include "fractresampler.h"
#include <QDir>
#include <QFile>
#include <QDebug>
#include <QtCore>

//////////////////////////////////////////////////////////////////////
// Local defines
//////////////////////////////////////////////////////////////////////
#define SINC_PERIOD_PTS 10000	//number of points in sinc table between "zero crossings"
								//smaller value increases noise floor

#define SINC_PERIODS 28	//number of input sample periods("zero crossings"-1) in
						//sinc function(should be even)
						//decreasing reduces alias free bandwidth

#define SINC_LENGTH	( (SINC_PERIODS)*SINC_PERIOD_PTS + 1)//number of total points in sinc table

#define MAX_SOUNDCARDVAL 32767.0


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
CFractResampler::CFractResampler()
{
	m_pSinc = NULL;
	m_pInputBuf = NULL;

}

CFractResampler::~CFractResampler()
{
	if(m_pSinc)
		delete m_pSinc;
	if(m_pInputBuf)
		delete m_pInputBuf;
}
//

//////////////////////////////////////////////////////////////////////
// Initialize resampler memory and create windowed sinc table
// MaxInputSize is the largest number of input samples expected to be processed
//////////////////////////////////////////////////////////////////////
void CFractResampler::Init(int MaxInputSize)
{
int i;
TYPEREAL fi;
TYPEREAL window;
	MaxInputSize += SINC_PERIODS;	//expand buffer size  to include wrap around
	if(NULL == m_pSinc)
		m_pSinc = new  TYPEREAL[SINC_LENGTH];
	if(m_pInputBuf)
		delete m_pInputBuf;
	m_pInputBuf = new TYPECPX[MaxInputSize];
	for(i=0; i<MaxInputSize; i++)
	{
		m_pInputBuf[i].re = 0.0;
		m_pInputBuf[i].im = 0.0;
	}
	for(i=0; i<SINC_LENGTH; i++)
	{	//calc Blackman-Harris window points
		window = (0.35875
				- 0.48829*cos( (2.0*M_PI*i)/(SINC_LENGTH-1) )
				+ 0.14128*cos( (4.0*M_PI*i)/(SINC_LENGTH-1) )
				- 0.01168*cos( (6.0*M_PI*i)/(SINC_LENGTH-1) ) );
		//calculate sin(x)/x    sinc point * window
		fi = M_PI*(double)(i - SINC_LENGTH/2)/(double)SINC_PERIOD_PTS ;
		if(i != SINC_LENGTH/2)
			m_pSinc[i] = window * (TYPEREAL)sin( (double)fi )/(double)fi;
		else
			m_pSinc[i] = 1.0;

	}
	m_FloatTime = 0.0;		//init floating point time accumulator

#if 0		//debug hack to write m_pSinc to a file for analysis
	QDir::setCurrent("d:/");
	QFile File;
	File.setFileName("Sinc.txt");
	if(File.open(QIODevice::WriteOnly))
	{
		qDebug()<<"file Opened OK";
		char Buf[30000];
		for( i=0; i<SINC_LENGTH; i++)
		{
			sprintf( Buf, "%19.12g\r\n", m_pSinc[i]);
			File.write(Buf);
		}
	}
	else
		qDebug()<<"file Failed to Open";

#endif
}

//////////////////////////////////////////////////////////////////////
// Resample InLength samples in pInBuf and place into pOutBuf
// using Rate = input rate / output rate
//   !!!! Make sure pOutBuf from caller is large enough to hold all
//  the generated samples, especially if up converting  !!!!!
// COMPLEX version
//////////////////////////////////////////////////////////////////////
int CFractResampler::Resample( int InLength, TYPEREAL Rate, TYPECPX* pInBuf, TYPECPX* pOutBuf)
{
int i;
int j;
int IntegerTime = (int)m_FloatTime;	//integer input time accumulator
double dt = Rate;	//output delta time as function of input sample time (input rate/output rate)
int outsamples = 0;
TYPECPX acc;

	//copy input samples into buffer starting at position SINC_PERIODS
	j = SINC_PERIODS;
	for(i=0; i<InLength; i++)
		m_pInputBuf[j++] = pInBuf[i];
	//now calculate output samples by looping until end of input buffer
	// is reached.  The output position is incremented in fractional time
	// of input sample time until all the possible input samples are
	//processed.
	while(IntegerTime < InLength )
	{	//convolve sinc function with input samples where sinc
		//function is centered at the output fractional time position
		acc.re = 0.0; acc.im = 0.0;
		for(i=1; i<=SINC_PERIODS; i++)
		{
			j = IntegerTime + i;	//temp integer time position for convolution loop
			int sindx =  (int)(( (double)j - m_FloatTime) * (double)SINC_PERIOD_PTS );
			acc.re += (m_pInputBuf[j].re * m_pSinc[sindx] );
			acc.im += (m_pInputBuf[j].im * m_pSinc[sindx] );
		}
		pOutBuf[outsamples++] = acc;
		m_FloatTime += dt;		//inc floating pt output time step
		IntegerTime = (int)m_FloatTime;	//truncate to integer
	}
	m_FloatTime -= (double)InLength;	//move floating time position back for next call
										//keeping leftover fraction
	//need to copy last SINC_PERIODS input samples in buffer to beginning of buffer
	// for FIR wrap around management. j points to last input sample processed
	j = InLength;
	for(i=0; i<SINC_PERIODS; i++)
		m_pInputBuf[i] = m_pInputBuf[j++];
	return outsamples;		//return number of output samples processed
}


//////////////////////////////////////////////////////////////////////
// Resample InLength samples in pInBuf and place into pOutBuf
// using Rate = input rate / output rate
//   !!!! Make sure pOutBuf from caller is large enough to hold all
//  the generated samples, especially if up converting  !!!!!
// stereo Integer version
//////////////////////////////////////////////////////////////////////
int CFractResampler::Resample( int InLength, TYPEREAL Rate, TYPECPX* pInBuf, TYPESTEREO16* pOutBuf, TYPEREAL gain)
{
int i;
int j;
int IntegerTime = (int)m_FloatTime;	//integer input time accumulator
double dt = Rate;	//output delta time as function of input sample time (input rate/output rate)
int outsamples = 0;
TYPECPX acc;

	//copy input samples into buffer starting at position SINC_PERIODS
	j = SINC_PERIODS;
	for(i=0; i<InLength; i++)
	{
		m_pInputBuf[j++] = pInBuf[i];
	}
	//now calculate output samples by looping until end of input buffer
	// is reached.  The output position is incremented in fractional time
	// of input sample time until all the possible input samples are
	//processed.
	while(IntegerTime < InLength )
	{	//convolve sinc function with input samples where sinc
		//function is centered at the output fractional time position
		acc.re = 0.0; acc.im = 0.0;
		for(i=1; i<=SINC_PERIODS; i++)
		{
			j = IntegerTime + i;	//temp integer time position for convolution loop
			int sindx =  (int)(( (double)j - m_FloatTime) * (double)SINC_PERIOD_PTS );
			acc.re += (m_pInputBuf[j].re * m_pSinc[sindx] );
			acc.im += (m_pInputBuf[j].im * m_pSinc[sindx] );
		}
		TYPECPX tmp;
		tmp.re = (acc.re * gain);;
		tmp.im = (acc.im * gain);;
		if(tmp.re > MAX_SOUNDCARDVAL)
			tmp.re = MAX_SOUNDCARDVAL;
		if(tmp.re < -MAX_SOUNDCARDVAL)
			tmp.re = -MAX_SOUNDCARDVAL;
		if(tmp.im>MAX_SOUNDCARDVAL)
			tmp.im = MAX_SOUNDCARDVAL;
		if(tmp.im < -MAX_SOUNDCARDVAL)
			tmp.im = -MAX_SOUNDCARDVAL;
		pOutBuf[outsamples].re = (qint16)tmp.re;
		pOutBuf[outsamples++].im = (qint16)tmp.im;

		m_FloatTime += dt;	//inc floating pt output time step
		IntegerTime = (int)m_FloatTime;	//truncate to integer
	}
	m_FloatTime -= (double)InLength;	//move floating time position back for next call
										//keeping leftover fraction
	//need to copy last SINC_PERIODS input samples in buffer to beginning of buffer
	// for FIR wrap around management. j points to last input sample processed
	j = InLength;
	for(i=0; i<SINC_PERIODS; i++)
		m_pInputBuf[i] = m_pInputBuf[j++];
	return outsamples;		//return number of output samples processed
}

//////////////////////////////////////////////////////////////////////
// Resample InLength samples in pInBuf and place into pOutBuf
// using Rate = input rate / output rate
//   !!!! Make sure pOutBuf from caller is large enough to hold all
//  the generated samples, especially if up converting  !!!!!
// REAL version
//////////////////////////////////////////////////////////////////////
int CFractResampler::Resample( int InLength, TYPEREAL Rate, TYPEREAL* pInBuf, TYPEREAL* pOutBuf)
{
int i;
int j;
int IntegerTime = (int)m_FloatTime;	//integer input time accumulator
double dt = Rate;	//output delta time as function of input sample time (input rate/output rate)
int outsamples = 0;
TYPEREAL acc;

	//copy input samples into buffer starting at position SINC_PERIODS
	j = SINC_PERIODS;
	for(i=0; i<InLength; i++)
		m_pInputBuf[j++].re = pInBuf[i];
	//now calculate output samples by looping until end of input buffer
	// is reached.  The output position is incremented in fractional time
	// of input sample time until all the possible input samples are
	//processed.
	while(IntegerTime < InLength )
	{	//convolve sinc function with input samples where sinc
		//function is centered at the output fractional time position
		acc = 0.0;
		for(i=1; i<=SINC_PERIODS; i++)
		{
			j = IntegerTime + i;	//temp integer time position for convolution loop
			int sindx =  (int)(( (double)j - m_FloatTime) * (double)SINC_PERIOD_PTS );
			acc += (m_pInputBuf[j].re * m_pSinc[sindx] );
		}
		pOutBuf[outsamples++] = acc;
		m_FloatTime += dt;
		IntegerTime = (int)m_FloatTime;
	}
	m_FloatTime -= (double)InLength;	//move floating time position back for next call
										//keeping leftover fraction
	//need to copy last SINC_PERIODS input samples in buffer to beginning of buffer
	// for FIR wrap around management. j points to last input sample processed
	j = InLength;
	for(i=0; i<SINC_PERIODS; i++)
		m_pInputBuf[i].re = m_pInputBuf[j++].re;
	return outsamples;
}

//////////////////////////////////////////////////////////////////////
// Resample InLength samples in pInBuf and place into pOutBuf
// using Rate = input rate / output rate
//   !!!! Make sure pOutBuf from caller is large enough to hold all
//  the generated samples, especially if up converting  !!!!!
// short Integer version
//////////////////////////////////////////////////////////////////////
int CFractResampler::Resample( int InLength, TYPEREAL Rate, TYPEREAL* pInBuf, TYPEMONO16* pOutBuf, TYPEREAL gain)
{
int i;
int j;
int IntegerTime = (int)m_FloatTime;	//integer input time accumulator
double dt = Rate;	//output delta time as function of input sample time (input rate/output rate)
int outsamples = 0;
TYPEREAL acc;

	//copy input samples into buffer starting at position SINC_PERIODS
	j = SINC_PERIODS;
	for(i=0; i<InLength; i++)
		m_pInputBuf[j++].re = pInBuf[i];
	//now calculate output samples by looping until end of input buffer
	// is reached.  The output position is incremented in fractional time
	// of input sample time until all the possible input samples are
	//processed.
	while(IntegerTime < InLength )
	{	//convolve sinc function with input samples where sinc
		//function is centered at the output fractional time position
		acc = 0.0;
		for(i=1; i<=SINC_PERIODS; i++)
		{
			j = IntegerTime + i;	//temp integer time position for convolution loop
			int sindx =  (int)(( (double)j - m_FloatTime) * (double)SINC_PERIOD_PTS );
			acc += (m_pInputBuf[j].re * m_pSinc[sindx] );
		}
		TYPEREAL tmp;
		tmp = (acc * gain);;
		if(tmp > MAX_SOUNDCARDVAL)
			tmp = MAX_SOUNDCARDVAL;
		if(tmp < -MAX_SOUNDCARDVAL)
			tmp = -MAX_SOUNDCARDVAL;
		pOutBuf[outsamples++] = (TYPEMONO16)tmp;

		m_FloatTime += dt;
		IntegerTime = (int)m_FloatTime;
	}
	m_FloatTime -= (double)InLength;	//move floating time position back for next call
										//keeping leftover fraction
	//need to copy last SINC_PERIODS input samples in buffer to beginning of buffer
	// for FIR wrap around management. j points to last input sample processed
	j = InLength;
	for(i=0; i<SINC_PERIODS; i++)
		m_pInputBuf[i].re = m_pInputBuf[j++].re;
	return outsamples;
}
