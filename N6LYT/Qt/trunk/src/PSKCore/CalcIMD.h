//////////////////////////////////////////////////////////////////////
// CalcIMD.h: interface for the CCalcIMD class.
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

#if !defined(AFX_CALCIMD_H__997190E0_030A_11D4_B4E7_D0AA4EC10108__INCLUDED_)
#define AFX_CALCIMD_H__997190E0_030A_11D4_B4E7_D0AA4EC10108__INCLUDED_

#include "types.h"

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#define NUM_FILTERS 3

class CCalcIMD  
{
public:
	void ResetIMDEnergies();
    bool CalcIMDEnergies(_complex samp);
    bool CalcIMDValue( INT &imdval);
	CCalcIMD();
	virtual ~CCalcIMD();

private:
	double I1[NUM_FILTERS];
	double I2[NUM_FILTERS];
	double Q1[NUM_FILTERS];
	double Q2[NUM_FILTERS];
	double m_Energy[NUM_FILTERS];
    int m_NCount;
	double m_Snr;
	double m_Imd;
	
};

#endif // !defined(AFX_CALCIMD_H__997190E0_030A_11D4_B4E7_D0AA4EC10108__INCLUDED_)
