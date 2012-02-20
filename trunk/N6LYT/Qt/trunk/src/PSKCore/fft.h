// fft.h: interface for the Cfft class.
//
//  This is a slightly modified version of Takuya OOURA's
//     original radix 4 FFT package.
//Copyright(C) 1996-1998 Takuya OOURA
//    (email: ooura@mmm.t.u-tokyo.ac.jp).
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_FFT_H__C6E9A1D3_2D5F_11D3_A208_00A0C996E7F5__INCLUDED_)
#define AFX_FFT_H__C6E9A1D3_2D5F_11D3_A208_00A0C996E7F5__INCLUDED_

#include "types.h"

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#define FFT_SIZE 2048

class Cfft  
{
public:
	LONG GetClosestPeak( INT Start, INT Stop);
	INT FindPKfreq( INT Center, INT range);
	void CalcFFT(double * InBuf);
	void ResetFFT();
	void SetFFTParams(  INT ave, double gain, INT type,double clkerror);
	BOOL GetFFTData( INT start, INT stop, LONG* OutBuf, BOOL UseOffset);
	Cfft();
	virtual ~Cfft();
private:
	BOOL m_Overload;
	BOOL m_LogMode;
	BOOL m_LastLogMode;
	INT m_LastAve;
	INT m_AveSize;
	INT m_OffsetIndxTable[FFT_SIZE/2];
	double m_Gain;
	double m_Clip;
	double * SinCosTbl;
	double * WindowTbl;
	double* pFFTAveBuf;
	double* pFFTInBuf;
	INT * WorkArea;
	void makewt(int nw, int *ip, double *w);
	void makect(int nc, int *ip, double *c);
	void bitrv2(int n, int *ip, double *a);
	void cftfsub(int n, double *a, double *w);
	void rftfsub(int n, double *a, int nc, double *c);
    void cft1st(int n, double *a, double *w);
    void cftmdl(int n, int l, double *a, double *w);
};

#endif // !defined(AFX_FFT_H__C6E9A1D3_2D5F_11D3_A208_00A0C996E7F5__INCLUDED_)
