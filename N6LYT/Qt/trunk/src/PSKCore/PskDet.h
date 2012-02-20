//////////////////////////////////////////////////////////////////////
// PskDet.h: interface for the CPSKDet class.
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

#if !defined(AFX_PskDet_H__0C099641_E9AB_11D2_88B9_444553540000__INCLUDED_)
#define AFX_PskDet_H__0C099641_E9AB_11D2_88B9_444553540000__INCLUDED_

#include "types.h"

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#include <math.h>
#include "PskDetTbl.h"
#include "CalcIMD.h"

#include <QObject>

#define BPSK_MODE 0
#define QPSKU_MODE 1
#define QPSKL_MODE 2

#define PSK63_MODE (1<<3)
#define PSK125_MODE (1<<4)	//AA6YQ 1.21

#define SQMODEFAST 0
#define SQMODESLOW 1

struct SurvivorStates
{
	double Pathdistance;	// sum of all metrics for a given survivor path
	LONG BitEstimates;		// the bit pattern estimate associated with given survivor path
};


class CPSKDet : public QObject
{
    Q_OBJECT
public:
	CPSKDet();
	virtual ~CPSKDet();

    void Init(INT Fs ,INT BlockSize, /*HWND hWnd,*/ INT chan );
	void ProcPSKDet( double* pIn);
    void GetVectorData(qint32* VectData);
	void GetSyncData(long* SyncData);
	void ResetDetector();
	void SetRXFrequency(INT freq);

	void SetRXPSKMode(INT mode)
	{
		//char buf[200];
		//sprintf( buf, " Mode=%32u ",mode);
		//AfxMessageBox( buf );

		m_PSK63Mode = FALSE;		//AA6YQ 1.21
		m_PSK125Mode = FALSE;

		if(mode&PSK63_MODE) 			
			m_PSK63Mode = TRUE;

		if(mode&PSK125_MODE)  
			m_PSK125Mode = TRUE;
		
		m_RxMode = mode & (~PSK63_MODE) & (~PSK125_MODE);
	};

	void SetAFCLimit(INT limit);
	void SetSampleClkAdj(INT ppm);
	INT GetRXFrequency(){return m_RxFrequency;};
	INT GetSignalLevel()
	{ 
		if(m_SQLevel>0)
			return m_SQLevel;
		else
			return 0;
	};
	void SetSquelchThresh(INT thresh,INT speed)
	{
		m_SQThresh = thresh;
		if( speed == SQMODEFAST )
			m_SquelchSpeed = 20;
		else
			if( speed == SQMODESLOW )
				m_SquelchSpeed = 75;
			else
				m_SquelchSpeed = speed;
	};
//public variables
    //HWND m_hWnd;

signals:
    void IMDReady(int,int);
    void clkError(int,int);
    void pskCharReady(int,int);

private:
//methods
	void CalcBitFilter(  _complex Samp);
	void CalcAGC( _complex Samp );
	void DecodeSymb( _complex newsamp );
	void CalcQuality(double angle );
	BOOL ViterbiDecode( double newangle);
	BOOL SymbSync(_complex sample);
	double CalcDistance( _complex newsymb, BYTE symb);
	void CalcFreqError( _complex IQ );
	void CalcFFreqError( _complex IQ );
// other classes
	CCalcIMD CalcIMD;
//variables
	BOOL m_FastAFCMode;
	BOOL m_AFCCaptureOn;
	BOOL m_IMDValid;
	BOOL m_SQOpen;
	BOOL m_LastBitZero;
	BOOL m_PSK63Mode;
	BOOL m_PSK125Mode;				//AA6YQ 1.21
	BYTE m_VaricodeDecTbl[2048];
	WORD m_BitAcc;
	LONG m_IQPhaseArray[20];
	LONG m_SyncArray[20];
	INT m_AFCTimer;
	INT m_AFCmode;
	INT m_RxMode;
	INT m_SampleClkAdj;
	INT m_AFCcounter;
	INT m_IMDValue;
	INT m_IQPhzIndex;
	INT m_SquelchSpeed;
	INT m_RxChannel;
	INT m_SQLevel;
	INT m_SQThresh;
	INT m_ClkErrTimer;
	INT m_ClkErrCounter;
	INT m_ClkError;
	INT m_LastPkPos;
	INT m_OnCount;
	INT m_OffCount;
	INT m_TrigInhib;
	INT m_SampCnt;
	INT m_BlockSize;
	INT m_RxFrequency;
	INT m_Fs;
	INT m_Fir1State;
	INT m_Fir2State;
	INT m_Fir3State;
	double m_FreqError;
	double m_QPSKprob[4];
	double m_DevAve;
	double m_I0;		// 4 stage I/Q delay line variables
	double m_I1;
	double m_Q0;
	double m_Q1;
	double m_BitPhaseInc;
	double m_BitPhasePos;
	double m_SyncAve[21];
	double m_NCOphzinc;
	double m_SampleFreq;
	double m_AFClimit;
	double m_AFCmax;
	double m_AFCmin;
	double m_NLPk;
	_complex m_pQue1[DEC4_LPFIR_LENGTH];
	_complex m_pQue2[DEC4_LPFIR_LENGTH];
	_complex m_pQue3[BITFIR_LENGTH];
	_complex m_FreqSignal;
	_complex m_BitSignal;

	struct SurvivorStates m_SurvivorStates[16]; /* Survivor path trellis */

// Local variables for various functions that need to be saved between calls
	INT	m_PkPos;
	INT	m_NewPkPos;
	INT m_BitPos;
	INT m_Pcnt;
	INT m_Ncnt;
	double m_AGCave;
	double m_FperrAve;
	double m_FferrAve;
	double m_QFreqError;
	double m_VcoPhz;
	_complex m_z1;
	_complex m_z2;
};

#endif // !defined(AFX_PskDet_H__0C099641_E9AB_11D2_88B9_444553540000__INCLUDED_)
