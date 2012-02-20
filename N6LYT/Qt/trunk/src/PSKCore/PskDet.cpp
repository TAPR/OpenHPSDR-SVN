//////////////////////////////////////////////////////////////////////
// PskDet.cpp: implementation of the CPSKDet class.
//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
// PSK31Core Library for transmission and reception of PSK31 signals
//    using a PC soundcard  or .wav files.
//	               Copyright 2000, Moe Wheatley, AE4JY
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// http://www.gnu.org/copyleft/lesser.html
//////////////////////////////////////////////////////////////////////
//

#include <QDebug>

#include <stdlib.h>  // rand

//#include "stdafx.h"
#include "PskDet.h"
#include "PSKTables.h"
#include "PSKCore.h"
//#include "perform.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

// define some constants
#define PHZ_180_BMIN	(0.0)			// 0
#define PHZ_180_BMAX	(PI2/2.0)		// Pi
#define PHZ_0_BMIN	(PI2/2.0)			// Pi
#define PHZ_0_BMAX	(PI2)				// 2Pi
//
#define PHZ_180_QMIN	(PI2/8.0)		// Pi/4
#define PHZ_180_QMAX	(3.0*PI2/8.0)	// 3Pi/4
#define PHZ_0_QMIN	(5.0*PI2/8.0)		// 5Pi/4
#define PHZ_0_QMAX	(7.0*PI2/8.0)		// 7Pi/4
#define Ts (.032+.00000)			// Ts == symbol period
//#define Ts (.032+.000072)		//to simulate off freq soundcards

//define the PSK31 symbols
#define SYM_NOCHANGE 0	//Stay the same phase
#define SYM_P90 1		//Plus 90  deg
#define SYM_P180 2		//Plus 180 deg
#define SYM_M90 3		//Minus 90 deg

#define AFC_OFF 0
#define AFC_ON 1
#define AFC_TIMELIMIT 10
#define AFC_FTIMELIMIT 2

#define NLP_K (100.0)		//narrow phase derived afc constans
#define FNLP_K (30.0)

extern double TestData[];
extern double DebugFP1;
extern double DebugFP2;
extern INT DebugINT;

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CPSKDet::CPSKDet()
{
	m_RxFrequency = 1500;
	m_SampleClkAdj = 0;
	m_Fs = 8000;
	m_IQPhzIndex = 0;
	m_AFCmode = AFC_ON;
	m_IMDValid = FALSE;
	m_FreqError = 0.0;
	m_RxMode = BPSK_MODE;
	m_PSK63Mode = FALSE;
	m_PSK125Mode = FALSE;
	//(dec) 25-Aug-01 The stock setting is not getting translated. Need to set it explicitly.
	//m_SquelchSpeed = SQMODESLOW;
	m_SquelchSpeed = 75;
	m_SQThresh = 50;
	m_NCOphzinc = (PI2*(double)m_RxFrequency/(double)m_Fs);
	m_AFClimit = 50.0*PI2/(double)m_Fs;
	m_AFCmax =  m_NCOphzinc + m_AFClimit;
	m_AFCmin =  m_NCOphzinc - m_AFClimit;
	SetSampleClkAdj(0);
}

CPSKDet::~CPSKDet()
{
}

/////////////////////////////////////////////////////////////////
//       Initialize PskDet buffers and pointers
/////////////////////////////////////////////////////////////////
void CPSKDet::Init( INT Fs, INT BlockSize, /*HWND hWnd,*/ INT chan )
{
WORD wTemp;
//INT i;

qDebug()<<"CPSKDet::Init: Fs:"<<Fs<<" BlockSize:"<<BlockSize<<" chan:"<<chan;
	m_Fs = Fs;
    //m_hWnd = hWnd;
	m_RxChannel = chan;
	m_BlockSize = BlockSize;	//size data input buffer
// init circular delay lines.(data stays put and the pointers move)
	for( WORD j=0; j<2048; j++)		//init inverse varicode lookup decoder table
    {
		m_VaricodeDecTbl[j] = 0;
        for( INT i=0; i<256;i++)
		{
            wTemp = VARICODE_TABLE[i];
			wTemp >>= 4;
			while( !(wTemp&1) )
                wTemp >>= 1;
            wTemp >>= 1;
            if( wTemp == j) {
                m_VaricodeDecTbl[j] = (BYTE)i;
            }
		}
	}
//
	ResetDetector();
	SetSampleClkAdj( 0 );

	m_BlockSize = BlockSize;	//size data input buffer
	m_SQLevel = 50;
	m_BitPhasePos = 0.0;
	m_BitAcc = 0;
	m_LastBitZero = FALSE;
	m_SampCnt = 0;
	m_OnCount = 0;
	m_OffCount = 0;
// Init a bunch of "static" variables used in various member fuctions
	m_AGCave = 0.0;
	m_VcoPhz = 0.0;
	m_PkPos = 0;
	m_NewPkPos = 5;
	m_BitPos = 0;
	m_I1 = 0.0;
	m_I0 = 0.0;
	m_Q1 = 0.0;
	m_Q0 = 0.0;
	m_DevAve = .4;
	m_z1.x = 0.0; m_z1.y = 0.0;
	m_z2.x = 0.0; m_z2.y = 0.0;
	m_FferrAve = 0.0;
	m_FferrAve = 0.0;
	m_QFreqError = 0.0;
	m_LastPkPos = 0;
	m_SQOpen = FALSE;
	m_AFCTimer = 0;
	m_AFCCaptureOn = FALSE;
	m_FastAFCMode = FALSE;
	m_NLPk = NLP_K;
}

//////////////////////////////////////////////////////////////////////
// Called to adjust the sample clock
//////////////////////////////////////////////////////////////////////
void CPSKDet::SetSampleClkAdj(INT ppm)
{
	m_SampleClkAdj = (m_Fs*ppm)/1000000;
	m_SampleFreq = (double)(m_Fs + m_SampleClkAdj);		//adj sample rate
	m_NCOphzinc = (PI2/m_SampleFreq)*(double)m_RxFrequency;	//new center freq inc
	m_BitPhaseInc = 16.0/m_SampleFreq;		//bit oversampling period
}

//////////////////////////////////////////////////////////////////////
// Called to get the symbol vectors
//////////////////////////////////////////////////////////////////////
void CPSKDet::GetVectorData(qint32 *VectData)
{
	for(INT i=0; i<16; i++)
		VectData[i] = m_IQPhaseArray[i];
}

//////////////////////////////////////////////////////////////////////
// Called to get the symbol vectors
//////////////////////////////////////////////////////////////////////
void CPSKDet::GetSyncData(long *SyncData)
{
	for(INT i=0; i<16; i++)
		SyncData[i] = m_SyncArray[i];
}

//////////////////////////////////////////////////////////////////////
// Called to change the Rx frequency
//////////////////////////////////////////////////////////////////////
void CPSKDet::SetRXFrequency(INT freq)
{
	if( freq != m_RxFrequency)
	{
		m_RxFrequency = freq;
		m_NCOphzinc = PI2*(double)freq/m_SampleFreq;
		m_FferrAve = 0.0;
		m_FperrAve = 0.0;
		if(m_FastAFCMode)
			m_AFCTimer = AFC_FTIMELIMIT;
		else
			m_AFCTimer = AFC_TIMELIMIT;
// calculate new limits around new receive frequency
		m_AFCmax =  m_NCOphzinc + m_AFClimit;
		m_AFCmin =  m_NCOphzinc - m_AFClimit;
		if(m_AFCmin<=0.0)
			m_AFCmin = 0.0;
		m_Pcnt = 0;
		m_Ncnt = 0;
	}
}

//////////////////////////////////////////////////////////////////////
// Called to change the AFC limit
//////////////////////////////////////////////////////////////////////
void CPSKDet::SetAFCLimit(INT limit)
{
	if(limit==0)
		m_AFCmode = AFC_OFF;
	else
		m_AFCmode = AFC_ON;
	if(limit==3000)
		m_FastAFCMode = TRUE;
	else
		m_FastAFCMode = FALSE;
	m_AFClimit = (double)limit*PI2/m_SampleFreq;
// calculate new limits around current receive frequency
	m_AFCmax =  m_NCOphzinc + m_FreqError + m_AFClimit;
	m_AFCmin =  m_NCOphzinc + m_FreqError - m_AFClimit;
	if(m_AFCmin<=0.0)
		m_AFCmin = 0.0;
	if(m_FastAFCMode)
	{
		m_NLPk = FNLP_K;
	}
	else
	{
		m_NLPk = NLP_K;
	}
}

//////////////////////////////////////////////////////////////////////
// Called to reset the detector before going into xmit
//////////////////////////////////////////////////////////////////////
void CPSKDet::ResetDetector()
{
	for(INT i=0; i<DEC4_LPFIR_LENGTH; i++)
	{
		m_pQue1[i].x = 0.0;	// fill delay buffer with zero
		m_pQue1[i].y = 0.0;
	}
	for(int i=0; i<DEC4_LPFIR_LENGTH; i++)
	{
		m_pQue2[i].x = 0.0;	// fill delay buffer with zero
		m_pQue2[i].y = 0.0;
	}
	for(int i=0; i<BITFIR_LENGTH; i++)
	{
		m_pQue3[i].x = 0.0;	// fill delay buffer with zero
		m_pQue3[i].y = 0.0;
	}
	for(int i=0; i<16; i++)
	{
		m_SurvivorStates[i].Pathdistance = 0.0;
		m_SurvivorStates[i].BitEstimates = 0;
		m_IQPhaseArray[i] = 1;
	}
	for(int i=0; i<21; i++)
	{
		m_SyncAve[i] = 0.0;				// initialize the array
		ViterbiDecode( 3.0*PI2/4.0);	// init the Viterbi decoder
	}
	m_Fir1State = DEC4_LPFIR_LENGTH-1;	//initialize FIR states
	m_Fir2State = DEC4_LPFIR_LENGTH-1;
	m_Fir3State = BITFIR_LENGTH-1;
	m_SQLevel = 10;
	m_ClkErrCounter = 0;
	m_ClkErrTimer = 0;
	m_ClkError = 0;
	m_DevAve = 90.0;
}

//////////////////////////////////////////////////////////////////////
// Main routine called to process the next block of data 'pIn'.
//////////////////////////////////////////////////////////////////////
void CPSKDet::ProcPSKDet( double* pIn)
{
INT i,j;
const double* Kptr;
_complex acc;
_complex* Firptr;
double vcophz = m_VcoPhz;

INT mod16_8 = 16;

if(m_PSK63Mode)
		mod16_8 = 8;

	if(	m_AFCTimer )
	{
		if(--m_AFCTimer <= 0)
		{
			m_AFCTimer = 0;
			m_AFCCaptureOn = FALSE;
			// calculate new limits around latest receive frequency
			m_AFCmax =  m_NCOphzinc + m_AFClimit;
			m_AFCmin =  m_NCOphzinc - m_AFClimit;
			if(m_AFCmin<=0.0)
				m_AFCmin = 0.0;
		}
		else
			m_AFCCaptureOn = TRUE;
	}
	for( i = 0; i<m_BlockSize; i++ )	// put new samples into Queue
	{
//Generate complex sample by mixing input sample with NCO's sin/cos
		m_pQue1[m_Fir1State].x = pIn[i] * cos( vcophz );	//place in circular Queue
		m_pQue1[m_Fir1State].y = pIn[i] * sin( vcophz );
		vcophz = vcophz + m_NCOphzinc + m_FreqError;
		if(vcophz > PI2)		//handle 2 Pi wrap around
			vcophz -= PI2;
//decimate by 4 filter
		if( ( (++m_SampCnt)%4 ) == 0 )	//calc first decimation filter every 4 samples
		{
			acc.x = 0.0;
			acc.y = 0.0;
			Firptr = m_pQue1;
			Kptr = Dec4LPCoef + DEC4_LPFIR_LENGTH - m_Fir1State;
			for(j=0; j<	DEC4_LPFIR_LENGTH; j++)	//do the MAC's
			{
				acc.x += ( (Firptr->x)*(*Kptr) );
				acc.y += ( (Firptr++->y)*(*Kptr++) );
			}
			m_pQue2[m_Fir2State].x = acc.x;	//place in circular Queue of next filter
			m_pQue2[m_Fir2State].y = acc.y;		

			//1.21 AA6YQ second decimation filter not required for PSK125
			
			if( m_PSK125Mode || ((m_SampCnt%mod16_8) == 0) ) //calc second decimation filter every 8 or 16samples
			{
				if (!m_PSK125Mode)	//decimate by 4 or 2 filter (PSK31 or PSK63)
				{ 
					acc.x = 0.0;
					acc.y = 0.0;
					Firptr = m_pQue2;
					Kptr = Dec4LPCoef + DEC4_LPFIR_LENGTH - m_Fir2State;
					for(j=0; j<	DEC4_LPFIR_LENGTH; j++)	//do the MAC's
					{
						acc.x += ( (Firptr->x)*(*Kptr) );
						acc.y += ( (Firptr++->y)*(*Kptr++) );
					}
				}

// here at Fs/16 == 500.0 Hz or 1000.0 Hz rate with latest sample in acc.
// Matched Filter the I and Q data and also a frequency error filter
//	filter puts filtered signals in variables m_FreqSignal and m_BitSignal.

				CalcBitFilter( acc );

// Perform AGC operation
				CalcAGC( m_FreqSignal );
// Calculate frequency error
				if(m_FastAFCMode)
					CalcFFreqError(m_FreqSignal);
				else
					CalcFreqError(m_FreqSignal);
// Bit Timing synchronization
				if( SymbSync(m_BitSignal) )
					DecodeSymb( m_BitSignal);
// Calculate IMD if only idles have been received and the energies collected
				if( m_IMDValid  )
				{
					if( CalcIMD.CalcIMDEnergies(acc) )
					{
                        if(CalcIMD.CalcIMDValue( m_IMDValue ) ) {
                            qDebug()<<"PskDet: IMD:"<<m_IMDValue;
                            emit IMDReady(m_IMDValue,m_RxChannel);
                            //::PostMessage(m_hWnd, MSG_IMDRDY, m_IMDValue, m_RxChannel);
                        } else {
                            emit IMDReady(m_IMDValue,m_RxChannel+0x0080);
                            qDebug()<<"PskDet: IMD:"<<m_IMDValue;
                            //::PostMessage(m_hWnd, MSG_IMDRDY, m_IMDValue, m_RxChannel+0x0080);
                        }
					}
				}
				else
					CalcIMD.ResetIMDEnergies();
			}
			if( --m_Fir2State < 0)	//deal with FIR pointer wraparound
				m_Fir2State = DEC4_LPFIR_LENGTH-1;
		}
		if( --m_Fir1State < 0)	//deal with FIR pointer wraparound
			m_Fir1State = DEC4_LPFIR_LENGTH-1;
	}
	m_SampCnt = m_SampCnt%16;
	m_VcoPhz = vcophz;
	m_RxFrequency = (INT)(0.5+((m_NCOphzinc + m_FreqError)*m_SampleFreq/PI2 ) );
//	m_RxFrequency = (INT)(0.5+((m_NCOphzinc)*m_SampleFreq/PI2 ) );
}

//////////////////////////////////////////////////////////////////////
//  Frequency error calculator for fast AFC satellite mode
// returns frequency error for mainNCO.
//////////////////////////////////////////////////////////////////////
void CPSKDet::CalcFFreqError( _complex IQ )
{
#define FP_GN 0.008
#define FI_GN 3.0E-5
#define FP_CGN 0.002
#define FI_CGN 1.50E-5
#define FWIDE_GN (1.0/.02)			//gain to make error in Hz
#define FWLP_K (300.0)
	if(m_AFCmode == AFC_OFF)
	{
		m_FferrAve = 0.0;
		m_FperrAve = 0.0;
		m_FreqError = 0.0;
		return;
	}
double ferror;
	ferror = (IQ.x - m_z2.x) * m_z1.y - (IQ.y - m_z2.y) * m_z1.x;
	m_z2.x = m_z1.x;
	m_z2.y = m_z1.y;
	m_z1.x = IQ.x;
	m_z1.y = IQ.y;
	// error at this point is abt .02 per Hz error
	if( ferror > .30 )		//clamp range
		ferror = .30;
	if( ferror < -.30 )
		ferror = -.30;
	m_FferrAve = (1.0-1.0/FWLP_K)*m_FferrAve + ((1.0*FWIDE_GN)/FWLP_K)*ferror;
DebugFP2 = m_FferrAve;
	ferror=m_FferrAve;
	if( (ferror > 6.0) || (ferror < -6.0 ) )
	{
		m_NCOphzinc = m_NCOphzinc + (ferror*FI_CGN);
		m_FreqError = ferror*FP_CGN;
	}
	else
	{
DebugFP1 = m_FperrAve;
		if( (m_FferrAve*m_FperrAve)>0.0)	//make sure both errors agree
			ferror = m_FperrAve;
		else
			ferror = 0.0;
		if( (ferror > 0.3) || (ferror < -0.3 ) )
			m_NCOphzinc = m_NCOphzinc + (ferror*FI_GN);
		m_FreqError = ferror*FP_GN;
	}
	//clamp frequency within range
	if( (m_NCOphzinc+m_FreqError) > m_AFCmax )
	{
		m_NCOphzinc = m_AFCmax;
		m_FreqError = 0.0;
	}
	else if( (m_NCOphzinc+m_FreqError) < m_AFCmin )
	{
		m_NCOphzinc = m_AFCmin;
		m_FreqError = 0.0;
	}

//DebugFP2 = (m_NCOphzinc+m_FreqError)*8000.0/PI2;
}

//////////////////////////////////////////////////////////////////////
//  Frequency error calculator
// calculates the derivative of the tan(I/Q).
// returns frequency error for mainNCO.
//////////////////////////////////////////////////////////////////////
void CPSKDet::CalcFreqError( _complex IQ )
{
#define P_GN 0.001			//AFC constants
#define I_GN 1.5E-6
#define P_CGN 0.0004
#define I_CGN 3.0E-6
#define WIDE_GN (1.0/.02)			//gain to make error in Hz
#define WLP_K (200.0)
	if(m_AFCmode == AFC_OFF)
	{
		m_FferrAve = 0.0;
		m_FperrAve = 0.0;
		m_FreqError = 0.0;
		return;
	}
double ferror;
	ferror = (IQ.x - m_z2.x) * m_z1.y - (IQ.y - m_z2.y) * m_z1.x;
	m_z2.x = m_z1.x;
	m_z2.y = m_z1.y;
	m_z1.x = IQ.x;
	m_z1.y = IQ.y;
	// error at this point is abt .02 per Hz error
	if( ferror > .30 )		//clamp range
		ferror = .30;
	if( ferror < -.30 )
		ferror = -.30;
	m_FferrAve = (1.0-1.0/WLP_K)*m_FferrAve + ((1.0*WIDE_GN)/WLP_K)*ferror;
	if( m_AFCCaptureOn )
	{
		ferror=m_FferrAve;
		if( (ferror > 0.3) || (ferror < -0.3 ) )
			m_NCOphzinc = m_NCOphzinc + (ferror*I_CGN);
		m_FreqError = ferror*P_CGN;
	}
	else
	{
DebugFP2 = m_FperrAve;
		if( (m_FferrAve*m_FperrAve)>0.0)	//make sure both errors agree
			ferror = m_FperrAve;
		else
			ferror = 0.0;
		if( (ferror > 0.3) || (ferror < -0.3 ) )
			m_NCOphzinc = m_NCOphzinc + (ferror*I_GN);
		m_FreqError = ferror*P_GN;
	}

	//clamp frequency within range
	if( (m_NCOphzinc+m_FreqError) > m_AFCmax )
	{
		m_NCOphzinc = m_AFCmax;
		m_FreqError = 0.0;
	}
	else if( (m_NCOphzinc+m_FreqError) < m_AFCmin )
	{
		m_NCOphzinc = m_AFCmin;
		m_FreqError = 0.0;
	}
//DebugFP1 = m_FerrAve;//m_FreqError*8000/PI2;
//DebugFP1 = (m_NCOphzinc+m_FreqError)*8000.0/PI2;
//DebugFP1 = m_AFCmin*8000/PI2;
//DebugFP2 = m_AFCmax*8000.0/PI2;
}



//////////////////////////////////////////////////////////////////////
// Automatic gain control calculator
//////////////////////////////////////////////////////////////////////
void CPSKDet::CalcAGC( _complex Samp)
{
double mag;
//	mag = fabs(Samp.x) + fabs(Samp.y);
	mag = sqrt(Samp.x*Samp.x + Samp.y*Samp.y);
	if( mag > m_AGCave )
		m_AGCave = (1.0-1.0/200.0)*m_AGCave + (1.0/200.0)*mag;
	else
		m_AGCave = (1.0-1.0/500.0)*m_AGCave + (1.0/500.0)*mag;
	if( m_AGCave >= 1.0 )	// divide signal by ave if not almost zero
	{
		m_BitSignal.x /= m_AGCave;
		m_BitSignal.y /= m_AGCave;
		m_FreqSignal.x /= m_AGCave;
		m_FreqSignal.y /= m_AGCave;
	}
//	DebugFP1 = m_AGCave;
}

//////////////////////////////////////////////////////////////////////
// Called at Fs/16 rate to calculate the symbol sync position
// Returns TRUE if at center of symbol.
// Sums up the energy at each sample time, averages it, and picks the
//   sample time with the highest energy content.
//////////////////////////////////////////////////////////////////////
BOOL CPSKDet::SymbSync(_complex sample)
{
BOOL Trigger=FALSE;
double max;
double energy;
INT BitPos = m_BitPos;
	if(BitPos<16)
	{
		energy = (sample.x*sample.x) + (sample.y*sample.y);
		if( energy > 4.0)		//wait for AGC to settle down
			energy = 1.0;
		m_SyncAve[BitPos] = (1.0-1.0/82.0)*m_SyncAve[BitPos] + (1.0/82.0)*energy;
		if( BitPos == m_PkPos )	// see if at middle of symbol
		{
			Trigger = TRUE;
			m_SyncArray[m_PkPos] = (INT)(900.0*m_SyncAve[m_PkPos]);
		}
		else
		{
			Trigger = FALSE;
			m_SyncArray[BitPos] = (INT)(750.0*m_SyncAve[BitPos]);
		}
		if( BitPos == HALF_TBL[m_NewPkPos] )	//don't change pk pos until
			m_PkPos = m_NewPkPos;			// halfway into next bit.
		BitPos++;
	}

	m_BitPhasePos += (m_BitPhaseInc);
	if( m_BitPhasePos >= Ts )
	{									// here every symbol time
		m_BitPhasePos = fmod(m_BitPhasePos, Ts);	//keep phase bounded
		if((BitPos==15) && (m_PkPos==15))	//if missed the 15 bin before rollover
			Trigger = TRUE;
		BitPos = 0;
		max = -1e10;
		for( INT i=0; i<16; i++)		//find maximum energy pk
		{
			energy = m_SyncAve[i];
			if( energy > max )
			{
				m_NewPkPos = i;
				max = energy;
			}
		}
		if(m_SQOpen)
		{
			if( m_PkPos == m_LastPkPos+1 )	//calculate clock error
				m_ClkErrCounter++;
			else 
				if( m_PkPos == m_LastPkPos-1 )
					m_ClkErrCounter--;
			if( m_ClkErrTimer++ > 313 )	// every 10 seconds sample clk drift
			{
				m_ClkError = m_ClkErrCounter*200;	//each count is 200ppm
				m_ClkErrCounter = 0;
				m_ClkErrTimer = 0;
                //qDebug()<<"PskDet: clock error:"<<m_ClkError;
                emit clkError(m_ClkError,m_RxChannel);
                //::PostMessage(m_hWnd, MSG_CLKERROR, m_ClkError, m_RxChannel);
			}
		}
		else
		{
			m_ClkError = 0;
			m_ClkErrCounter = 0;
			m_ClkErrTimer = 0;
		}
		m_LastPkPos = m_PkPos;
	}
	m_BitPos = BitPos;
	return Trigger;
}

//////////////////////////////////////////////////////////////////////
//  Decode the new symbol sample
//////////////////////////////////////////////////////////////////////
void CPSKDet::DecodeSymb(_complex newsamp)
{
_complex vect;
double angle;
double energy;
BYTE ch = 0;
BOOL bit;
BOOL GotChar = FALSE;
	m_I1 = m_I0;		//form the multi delayed symbol samples
	m_Q1 = m_Q0;
	m_I0 = newsamp.x;
	m_Q0 = newsamp.y;
// calculate difference angle for QPSK, BPSK, and IQPSK decoding
//create vector whose angle is the difference angle by multiplying the
// current sample by the complex conjugate of the previous sample.
//swap I and Q axis to keep away from  the +/-Pi discontinuity and
//  add Pi to make make range from 0 to 2Pi.
// 180 deg phase changes center at Pi/4
// 0 deg phase changes center at 3Pi/2
// +90 deg phase changes center at 2Pi or 0
// -90 deg phase changes center at Pi
//  if using lower sideband must flip sign of difference angle.
//
// first calculate normalized vectors for vector display
	vect.y = (m_I1*m_I0 + m_Q1*m_Q0);
	vect.x = (m_I1*m_Q0 - m_I0*m_Q1);
	energy = sqrt(vect.x*vect.x + vect.y*vect.y)/1.0E3;
	if( m_AGCave > 10.0 )
	{
		m_IQPhaseArray[m_IQPhzIndex++] = (LONG)(vect.x/energy);
		m_IQPhaseArray[m_IQPhzIndex++] = (LONG)(vect.y/energy);
	}
	else
	{
		m_IQPhaseArray[m_IQPhzIndex++] = 2;
		m_IQPhaseArray[m_IQPhzIndex++] = 2;
	}
	m_IQPhzIndex &= 0x000F;		//mod 16 index
	if(m_RxMode == QPSKL_MODE)
		angle = (PI2/2) + atan2( vect.y, -vect.x); //QPSK lower sideband;
	else
		angle = (PI2/2) + atan2( vect.y, vect.x); //QPSK upper sideband or BPSK
	CalcQuality( angle);
	if(m_RxMode == BPSK_MODE)
	{
		//calc BPSK symbol over 2 chips
		vect.y = m_I1 * m_I0 +  m_Q1 * m_Q0;
		bit = (BOOL)(vect.y > 0.0);
	}
	else
		bit = ViterbiDecode( angle );
	if( (bit==0) && m_LastBitZero )	//if character delimiter
	{
		if(m_BitAcc != 0 )
		{
			m_BitAcc >>= 2;				//get rid of last zero and one
			m_BitAcc &= 0x07FF;
			ch = m_VaricodeDecTbl[m_BitAcc];

            //qDebug()<<"DecodeSymb: GotChar:"<<ch<<" m_BitAcc"<<m_BitAcc;
			m_BitAcc = 0;
			GotChar = TRUE;
		}
	}
	else
	{
		m_BitAcc <<= 1;
		m_BitAcc |= bit;
		if(bit==0)
			m_LastBitZero = TRUE;
		else
			m_LastBitZero = FALSE;
	}
	if(GotChar && (ch!=0) && m_SQOpen )
	{

        //qDebug()<<"PskDet: "<<ch;
        emit pskCharReady(ch,m_RxChannel);
        //::PostMessage(m_hWnd, MSG_PSKCHARRDY, ch, m_RxChannel);
	}

    //qDebug()<<"DecodeSymb: GotChar:"<<GotChar<<" ch:"<<ch<<" m_SQOpen:"<<m_SQOpen;
	GotChar = FALSE;
}

//////////////////////////////////////////////////////////////////////
// Calculate signal quality based on the statistics of the phase
//	difference angle.  The more dispersion of the "0" and "180" degree
//  phase shifts, the worse the signal quality.  This information is used
//  to activate the squelch control.  If 20 consecutive "180" degree shifts
//  occur, the squelch is forced on, and if 20 consecutive "0" degree
//  shifts occur, the squelch is forced off quickly.
//////////////////////////////////////////////////////////////////////
void CPSKDet::CalcQuality( double angle )
{
#define ELIMIT 5
#define PHZDERIVED_GN (1.0/.2)		//gain to make error in Hz
double temp;
double SqTimeK;
	SqTimeK = (double)m_SquelchSpeed;
	if( (m_RxMode && ((angle >= PHZ_180_QMIN) && (angle <= PHZ_180_QMAX) ) ) ||
		( !m_RxMode && ((angle >= PHZ_180_BMIN) && (angle <= PHZ_180_BMAX))) )
	{	//look +/-45 or +/-180 deg. around 180 deg.
		if(m_RxMode == QPSKL_MODE )
			temp = PI2/4.0 - angle;
		else
			temp = angle - PI2/4.0;
		m_QFreqError = temp;
		if( m_RxMode ) //if QPSK
			temp = 280.0*fabs(temp);
		else
			temp = 150.0*fabs(temp);
		if( temp < m_DevAve)
			m_DevAve=  (1.0-1.0/SqTimeK)*m_DevAve + (1.0/SqTimeK)*temp;
		else
			m_DevAve=  (1.0-1.0/(SqTimeK*2.0))*m_DevAve + (1.0/(SqTimeK*2.0))*temp;
		if(m_OnCount > 20 )		// fast squelch counter
			m_DevAve = 100.0-75.0;	//set to 75%
		else
			m_OnCount++;
		m_OffCount = 0;
		if( m_QFreqError >= 0.0 )
		{
			m_Pcnt++;
			m_Ncnt = 0;
		}
		else
		{
			m_Ncnt++;
			m_Pcnt = 0;
		}
		if( (m_Pcnt<ELIMIT) && (m_Ncnt<ELIMIT) )
			m_QFreqError = 0.0;
	}
	else
	{
		if( (m_RxMode && ((angle >= PHZ_0_QMIN) && (angle <= PHZ_0_QMAX) ) ) ||
			(!m_RxMode && ((angle >= PHZ_0_BMIN) && (angle <= PHZ_0_BMAX) ) ) )

		{		//look +/-45 or +/- 180 deg. around 0 deg.
			if(QPSKL_MODE==m_RxMode)
				temp = 3*PI2/4.0 - angle;
			else
				temp = angle - 3*PI2/4.0;
			m_QFreqError = temp;
			if( m_RxMode ) //if QPSK
				temp = 280.0*fabs(temp);
			else
				temp = 150.0*fabs(temp);
			if( temp < m_DevAve)
				m_DevAve=  (1.0-1.0/SqTimeK)*m_DevAve + (1.0/SqTimeK)*temp;
			else
				m_DevAve=  (1.0-1.0/(SqTimeK*2.0))*m_DevAve + (1.0/(SqTimeK*2.0))*temp;
			if(m_OffCount > 20 )	// fast squelch counter
				if( BPSK_MODE==m_RxMode ) //if BPSK
					m_DevAve = 100.0 - 0.0;		//set to 0%
			else
				m_OffCount++;
			m_OnCount = 0;
			if( m_QFreqError >= 0.0 )
			{
				m_Pcnt++;
				m_Ncnt = 0;
			}
			else
			{
				m_Ncnt++;
				m_Pcnt = 0;
			}
			if( (m_Pcnt<ELIMIT) && (m_Ncnt<ELIMIT) )
				m_QFreqError = 0.0;
		}

	}
	if(m_OnCount >2)
		m_IMDValid = TRUE;
	else
		m_IMDValid = FALSE;
//DebugINT = m_Pcnt+m_Ncnt;

	if( m_AGCave > 10.0 )
	{
		if( m_RxMode ) //if QPSK
			m_SQLevel = 100 - (INT)m_DevAve;
		else
			m_SQLevel = 100 - (INT)m_DevAve;
		if( m_SQLevel >= m_SQThresh )
			m_SQOpen = TRUE;
		else
			m_SQOpen = FALSE;
	}
	else
	{
		m_SQLevel = 0;
		m_SQOpen = FALSE;
	}
	if(m_RxMode)
	{
		if( m_QFreqError > .6 )//  clamp range to +/- 3 Hz
			m_QFreqError = .6;
		if( m_QFreqError < -.6 )
			m_QFreqError = -.6;
	}
	else
	{
		if( m_QFreqError > 1.0 )//  clamp range to +/- 5 Hz
			m_QFreqError = 1.0;
		if( m_QFreqError < -1.0 )
			m_QFreqError = -1.0;
	}
	m_FperrAve = (1.0-1.0/m_NLPk)*m_FperrAve + 
					( (1.0*PHZDERIVED_GN)/m_NLPk)*m_QFreqError;
}

//////////////////////////////////////////////////////////////////////
// BIT FIR filters. A narrow matched(?) data filter for data
//                  and wider filter for the AFC/AGC functions
//////////////////////////////////////////////////////////////////////
void CPSKDet::CalcBitFilter(_complex Samp)
{
_complex acc1;
_complex acc2;
const double* Kptr1;
const double* Kptr2;
_complex* Firptr;
	m_pQue3[m_Fir3State].x = Samp.x;	//place in circular Queue
	m_pQue3[m_Fir3State].y = Samp.y;
	acc1.x = 0.0;
	acc1.y = 0.0;
	acc2.x = 0.0;
	acc2.y = 0.0;
	Firptr = m_pQue3;
	Kptr1 = FreqFirCoef + BITFIR_LENGTH - m_Fir3State;	//frequency error filter
	Kptr2 = BitFirCoef + BITFIR_LENGTH - m_Fir3State;	//bit data filter
	for(INT j=0; j<	BITFIR_LENGTH;j++)	//do the MACs
	{
		acc1.x += ( (Firptr->x)*(*Kptr1) );
		acc1.y += ( (Firptr->y)*(*Kptr1++) );
		acc2.x += ( (Firptr->x)*(*Kptr2) );
		acc2.y += ( (Firptr++->y)*(*Kptr2++) );
	}
	if( --m_Fir3State < 0)
		m_Fir3State = BITFIR_LENGTH-1;
	m_FreqSignal.x = acc1.x;
	m_FreqSignal.y = acc1.y;
	m_BitSignal.x = acc2.x;
	m_BitSignal.y = acc2.y;
}



///////////////////////////////////////////////////////////////////////
// Soft-decision Viterbi decoder function.
///////////////////////////////////////////////////////////////////////
BOOL CPSKDet::ViterbiDecode( double newangle)
{
double pathdist[32];
double min;
INT bitestimates[32];
INT ones;
INT i;
const double* pAngleTbl;
	min = 1.0e100;		// make sure can find a minimum value
	if( newangle >= PI2/2 )		//deal with ambiguity at +/- 2PI
		pAngleTbl = ANGLE_TBL2;	// by using two different tables
	else
		pAngleTbl = ANGLE_TBL1;
	for(i = 0; i < 32; i++)		// calculate all possible distances
	{							//lsb of 'i' is newest bit estimate
		pathdist[i] = m_SurvivorStates[i / 2].Pathdistance + 
				fabs(newangle - pAngleTbl[ ConvolutionCodeTable[i] ]);
		if(pathdist[i] < min)	// keep track of minimum distance
			min = pathdist[i];
		// shift in newest bit estimates
		bitestimates[i] = ((m_SurvivorStates[i / 2].BitEstimates) << 1) + (i & 1);
	}
	for(i = 0; i < 16; i++)	//compare path lengths with the same end state
							// and keep only the smallest path in m_SurvivorStates[].
	{
		if(pathdist[i] < pathdist[16 + i])
		{
			m_SurvivorStates[i].Pathdistance = pathdist[i] - min;
			m_SurvivorStates[i].BitEstimates = bitestimates[i];
		}
		else
		{
			m_SurvivorStates[i].Pathdistance = pathdist[16 + i] - min;
			m_SurvivorStates[i].BitEstimates = bitestimates[16 + i];
		}
	}
	ones = 0;
	for(i = 0; i < 16; i++)		// find if more ones than zeros at bit 20 position
		ones += (m_SurvivorStates[i].BitEstimates&(1L << 20));
	if( ones == (8L << 20 ) )
		return ( rand() & 0x1000 );	//if a tie then guess
	else
		return(ones > (8L << 20) );	//else return most likely bit value
}

