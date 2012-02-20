//////////////////////////////////////////////////////////////////////
// PSKMod.h: interface for the CPSKMod class.
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

#if !defined(AFX_PSKMod_H__ACB4CEA2_C356_11D2_88B9_444553540000__INCLUDED_)
#define AFX_PSKMod_H__ACB4CEA2_C356_11D2_88B9_444553540000__INCLUDED_

// used to specify which mode to use
#define BPSK_MODE 0
#define QPSKU_MODE 1
#define QPSKL_MODE 2
#define TUNE_MODE 3
#define TUNE_MODE_WID 4
#define CW_MODE 5

#include <types.h>

#include <QObject>

class CPSKMod : public QObject
{
    Q_OBJECT

public:
	CPSKMod();
	virtual ~CPSKMod();
// PSK31 and CW modulator
    void InitPSKMod(INT Fs, /*HWND hWnd,*/ BOOL SatMode);
	void CalcPSK( double* pData , INT n);
	void PutTxQue(INT txchar, BOOL cntrl );
	void ClrQue();
	void SetCWIDSpeed(LONG speed);
	INT GetTXCharsRemaining();
	void SetTXCWID( PCHAR lpszIDstrg){m_CWIdString = lpszIDstrg;}
	void SetTXFreq(INT freq){m_TxFreq = (double)freq;}
	void SetTXMode(INT mode);
	void SetAutoShutoff(BOOL val){m_NeedShutoff = val;}
	void SetAutoCWID(BOOL val){ m_NeedCWid = val;}
	INT GetTXState(){return m_TXState;}
	void SetSampleClkAdj(INT ppm){m_ClkError = 1.0;};
//	void SetSampleClkAdj(INT ppm){m_ClkError = 1.0 - (double)ppm/1000000.0;};
    void GetVectorData(qint32* VectData);

//	HWND m_hWnd;

signals:
    void PskCharReady(INT);

private:
	BOOL m_PSK63Mode;
	BOOL m_PSK125Mode;		//AA6YQ 1.21
	BOOL m_SatMode;
	BOOL m_NoSquelchTail;
	BOOL m_NeedCWid;
	BOOL m_NeedShutoff;
	BOOL m_TempNoSquelchTail;
	BOOL m_TempNeedCWid;
	BOOL m_TempNeedShutoff;
	LONG m_IQPhaseArray[20];
	INT m_Fs;
	INT m_CWIDSpeed;
	INT m_SavedMode;
	INT m_Channel;
	INT m_VectLookup[6][2];
	INT m_pTail;
	INT m_pHead;
	INT GetTxChar();
	INT m_AmblePtr;
	INT m_Preamble[33];
	INT m_Postamble[33];
	INT m_TXState;
	INT m_PSKmode;
	INT m_CWState;
	INT m_CWtimer;
	INT m_Ramp;
	INT m_CWSetSpeed;
	PINT m_pXmitQue;
    CString m_CWIdString;
// PSK31 and CW generator variables
	BOOL m_AddEndingZero;
	BYTE m_PresentPhase;
	BYTE m_Lastsymb;
	WORD m_TxShiftReg;
	WORD m_TxCodeWord;
	double m_SymbolRate;
	double m_TxFreq;
	double m_SampleFreq;
	double m_ClkError;
	double m_t;
	double m_RMSConstant;
	double m_2PI;
	double m_PSKSecPerSamp;
	double m_PSKTime;
	double m_PSKPeriodUpdate;
	double m_PSKPhaseInc;
	double* m_pPSKtxI;
	double* m_pPSKtxQ;
	CRITICAL_SECTION m_CriticalSection;	// use for keeping threads
										// from stomping on each other


// PSK31 and CW modulator private functions
	INT GetChar( );
	BYTE GetNextBPSKSymbol(void);
	BYTE GetNextQPSKSymbol(void);
	BYTE GetNextCWSymbol(void);
	BYTE GetNextTuneSymbol(void);
};

#endif // !defined(AFX_PSKMod_H__ACB4CEA2_C356_11D2_88B9_444553540000__INCLUDED_)
