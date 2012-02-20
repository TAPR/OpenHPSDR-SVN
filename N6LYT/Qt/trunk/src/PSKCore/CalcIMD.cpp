//////////////////////////////////////////////////////////////////////
// CalcIMD.cpp: implementation of the CCalcIMD class.
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

//#include "stdafx.h"
#include "math.h"
#include "CalcIMD.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//#define PI2 (8.0*atan(1)) AA6YQ ATN call is ambiguous
#define PI2 6.28318530717959

#define N 288		//96 x 2 = Goertzel length(must be an integer value)
#define FS 500.0	// sample frequency

#define F0 15.625	// bin frequencies
#define F1 31.25
#define F2 46.875

#define K0 ((double)N*F0/FS)
#define K1 ((double)N*F1/FS)
#define K2 ((double)N*F2/FS)


static const double COEF[NUM_FILTERS] = 
{
	2.0*cos(PI2*K0/(double)N),
	2.0*cos(PI2*K1/(double)N),
	2.0*cos(PI2*K2/(double)N)
};


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CCalcIMD::CCalcIMD()
{
	for(INT i=0; i<NUM_FILTERS;i++)
	{
		I1[i] = 0.0; I2[i] = 0.0;
		Q1[i] = 0.0; Q2[i] = 0.0;
		m_Energy[i] = 0.0;
	}
	m_NCount = 0;
	m_Snr = 0;
	m_Imd = 0;
}

CCalcIMD::~CCalcIMD()
{

}
//////////////////////////////////////////////////////////////////
//  This routine calculates the energy in the frequency bands of
//   carier=F0(15.625), noise=F1(31.25), and 
//   3rd order product=F2(46.875)
//  It is called with complex data samples at 500 Hz.
//////////////////////////////////////////////////////////////////
BOOL CCalcIMD::CalcIMDEnergies(_complex samp)
{
INT i;
_complex temp;

	for(i=0; i<NUM_FILTERS;i++)
	{
		temp.x = I1[i]; temp.y = Q1[i];
		I1[i] = I1[i]*COEF[i]-I2[i]+samp.x;
		Q1[i] = Q1[i]*COEF[i]-Q2[i]+samp.y;
		I2[i] = temp.x; Q2[i] = temp.y;
	}
	if( ++m_NCount >= N )
	{
		m_NCount = 0;
		for(i=0; i<NUM_FILTERS;i++)
		{
			m_Energy[i] = I1[i]*I1[i] + I2[i]*I2[i] - I1[i]*I2[i]*COEF[i] +
							Q1[i]*Q1[i] + Q2[i]*Q2[i] - Q1[i]*Q2[i]*COEF[i];
			I1[i] = I2[i] = Q1[i] = Q2[i] = 0.0;
		}
		return TRUE;
	}
	else
		return FALSE;	
}
//////////////////////////////////////////////////////////////////
//  This routine calculates the IMD using the frequency bands of
//   carrier=F0(15.625), noise=F1(31.25), and 
//   3rd order product=F2(46.875)
//////////////////////////////////////////////////////////////////
BOOL CCalcIMD::CalcIMDValue( INT &imdval)
{
	m_Snr = 10.0*log10(m_Energy[0]/m_Energy[1]);
	m_Imd = 10.0*log10(m_Energy[2]/m_Energy[0]);
	imdval = (INT)m_Imd;
	if( m_Snr > (-m_Imd+6) )
		return TRUE;
	else
		return FALSE;
}

void CCalcIMD::ResetIMDEnergies()
{
	for(INT i=0; i<NUM_FILTERS;i++)
	{
		I1[i] = I2[i] = Q1[i] = Q2[i] = 0.0;
		m_NCount = 0;
	}

}
