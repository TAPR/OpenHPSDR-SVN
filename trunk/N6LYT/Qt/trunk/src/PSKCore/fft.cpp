// fft.cpp: implementation of the Cfft class.
//  This is a slightly modified version of Takuya OOURA's
//     original radix 4 FFT package.
//Copyright(C) 1996-1998 Takuya OOURA
//    (email: ooura@mmm.t.u-tokyo.ac.jp).
//////////////////////////////////////////////////////////////////////

//#include "stdafx.h"
#include <math.h>
#include "fft.h"

//////////////////////////////////////////////////////////////////////
// Local Defines
//////////////////////////////////////////////////////////////////////
#define SQRT_FFT_SIZE 46//sqrt(2048)

//#define K_2PI (8.0*atan(1))  AA6YQ atan is ambiguous
#define K_2PI 6.28318530717959

#define FREQ_SCALE (8000.0/2048.0)
#define SAMPLE_RATE 8000

//////////////////////////////////////////////////////////////////////
// A pure input sin wave ... Asin(wt)... will produce an fft output 
//   peak of (N*A/4)^2  where N is FFT_SIZE.
// To convert to a Power dB range:
//   PdBmax = 10*log10( (N*A/4)^2 + K_C ) + K_B
//   PdBmin = 10*log10( 0 + K_C ) + K_B
//  if (N*A/4)^2 >> K_C 
//  Then K_B = PdBmax - 20*log10( N*A/4 )
//       K_C = 10 ^ ( (PdBmin-K_B)/10 )
//  for power range of 0 to 100 dB with input(A) of 32767 and N=2048
//			K_B = -44.494132  and K_C = 2.81458e4
// To eliminate the multiply by 10, divide by 10 so for an output
//		range of 0 to 100dB the stored value is 0.0 to 10.0
//   so final constant K_B = -4.4494132
#define K_B (-4.4494132)
#define K_C (2.81458e4)

#define K_ROOT (1.0/4.0)		
#define K_ROOTGN 289.626		//(A*N/8)^(2/4) / 10

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
Cfft::Cfft()
{
	WindowTbl = new double[FFT_SIZE];
	SinCosTbl = new double[FFT_SIZE/2];
	WorkArea = new int[SQRT_FFT_SIZE+2];
	pFFTAveBuf = new double[FFT_SIZE];	//Even index's hold average
	pFFTInBuf = new double[FFT_SIZE];
	WorkArea[0] = 0;
	makewt(FFT_SIZE/4, WorkArea, SinCosTbl);
    makect(FFT_SIZE/4, WorkArea, SinCosTbl + WorkArea[0]);
	for(INT i=0; i<FFT_SIZE; i++)
	{
		pFFTAveBuf[i] = 0.0;
// Pick a data windowing function:
//		WindowTbl[i] = 1.0;										//rectangle
//		WindowTbl[i] = .54 - .46*cos( (K_2PI*i)/(FFT_SIZE-1) );	//Hamming
		WindowTbl[i] = (.5 - .5*cos( (K_2PI*i)/(FFT_SIZE-1) ));	//Hanning
	}
	m_LastAve = 0;
	m_LastLogMode = FALSE;
	SetFFTParams( 1, 10.0, TRUE, 1.0 );
	m_Clip = 0.0;
	m_Overload = FALSE;
}

Cfft::~Cfft()
{							// free all resources
	if(WorkArea)
	{
		delete WorkArea;
		WorkArea = NULL;
	}
	if(SinCosTbl)
	{
		delete SinCosTbl;
		SinCosTbl = NULL;
	}
	if(WindowTbl)
	{
		delete WindowTbl;
		WindowTbl = NULL;
	}
	if(pFFTAveBuf)
	{
		delete pFFTAveBuf;
		pFFTAveBuf = NULL;
	}
	if(pFFTInBuf)
	{
		delete pFFTInBuf;
		pFFTInBuf = NULL;
	}

}

void Cfft::SetFFTParams(INT ave, double gain, INT type, double clkerror )
{
	if( type==0 )
		m_LogMode = FALSE;
	else
		m_LogMode = TRUE;
	if( (type>=10) && (type<=99) )
		m_Clip = (double)type/10.0;
	else
		m_Clip = 0.0;
	if(ave>0)
		m_AveSize = ave;
	else
		m_AveSize = 1;
	if( (m_LastAve != ave) || (m_LastLogMode != m_LogMode) )
		ResetFFT();
	m_LastLogMode = m_LogMode;
	m_LastAve = m_AveSize;
	m_Gain = gain*10.0/(10.0-m_Clip);
	for(int i=0; i<(FFT_SIZE/2); i++)
	{
		m_OffsetIndxTable[i] = (INT)( clkerror*(double)i )<<1;
	}

}

void Cfft::ResetFFT()
{
	for(INT i=0; i<FFT_SIZE;i++)
		pFFTAveBuf[i] = 0.0;

}


//////////////////////////////////////////////////////////////////////
// "InBuf[]" is first multiplied by a window function and then
//  calculates an "FFT_SIZE" point FFT on "InBuf[]".
//  The result is converted to dB or 4th root and stored in pFFTAveBuf
//  If "Ave" is > 1, a LP smoothing filter 
//  is calculated on the output.
//////////////////////////////////////////////////////////////////////
void Cfft::CalcFFT(double * InBuf)
{
INT i;
	m_Overload = FALSE;
	for(i=0; i<FFT_SIZE; i++)
	{
		if( InBuf[i] > 32768.0*0.90 )	//flag overload if within 10% of max
			m_Overload = TRUE;
		pFFTInBuf[i] = WindowTbl[i] * InBuf[i];		//window the data
	}
//Calculate the FFT
	bitrv2(FFT_SIZE, WorkArea + 2, pFFTInBuf);
	cftfsub(FFT_SIZE, pFFTInBuf, SinCosTbl);
	rftfsub(FFT_SIZE, pFFTInBuf, WorkArea[1], SinCosTbl + WorkArea[0]);
}

//////////////////////////////////////////////////////////////////////
//	the range "start" to "stop" is multiplied by "gain" and copied
//   into "OutBuf[]".
// The function returns TRUE if the input is overloaded
//////////////////////////////////////////////////////////////////////
BOOL Cfft::GetFFTData(INT start, INT stop, LONG* OutBuf, BOOL UseOffset )
{
INT j;
	if(UseOffset)
	{
		for( INT i=start; i<=stop; i++ )		//copy and scale into OutBuf[]
		{
			j = m_OffsetIndxTable[i];
			OutBuf[i] = (LONG)( m_Gain*pFFTAveBuf[j] );
		}
	}
	else
	{
		for( INT i=start; i<=stop; i++ )		//copy and scale into OutBuf[]
		{
			OutBuf[i] = (LONG)( m_Gain*pFFTAveBuf[i<<1] );
		}
	}
	return m_Overload;
}
//////////////////////////////////////////////////////////////////////
// returns the nearest maximum frequency peak around the center freq
//  and +/- the range frequency.  
//////////////////////////////////////////////////////////////////////
INT Cfft::FindPKfreq(INT center, INT range)
{
#define INTLEN 9
double sum;
INT fStart = (INT)( (double)(center-range)/FREQ_SCALE );
INT fEnd = (INT)( (double)(center+range)/FREQ_SCALE );
INT integral;
double max = -1e30;
INT peakpos = 0;
	if( (fEnd-fStart) > INTLEN )
		integral = INTLEN;
	else
		integral = fEnd-fStart;

	for( INT i=fStart; i<=(fEnd-integral+1); i++)
	{
		sum = 0.0;
		for( INT j=0; j<integral; j++)
		{
			sum = sum + ( pFFTAveBuf[(i+j)*2] );
		}
		if( sum > max )
		{
			max = sum;
			peakpos = i;
		}
	}
	if(peakpos == fStart )
		return (center-range);
	else if(peakpos == (fEnd-integral) )
		return (center+range);
	else
		return (INT)( (double)(peakpos+((integral+1)/2))*FREQ_SCALE);
}


//////////////////////////////////////////////////////////////////////
// returns The closest frequency peak if found between Stop and Start.
// returns the start frequency if none is found.
// frequencies are in Hz.
//////////////////////////////////////////////////////////////////////
LONG Cfft::GetClosestPeak(INT Startf, INT Stopf)
{
#define CINTLEN 9
#define CRATIO 1.2
double last = 0.0;
BOOL pslope;
BOOL lastslope;
double sum;
double min = 0.0;
double max = 0.0;
INT Start = (Startf*FFT_SIZE)/SAMPLE_RATE;
INT Stop = (Stopf*FFT_SIZE)/SAMPLE_RATE;
	if( Startf < Stopf )		//Forward search
	{
		for( INT i=Start+CINTLEN/2; i<(Stop-CINTLEN); i++)
		{
			sum = 0.0;
			for( INT j=0; j<CINTLEN; j++)
			{
				sum += (pFFTAveBuf[(i+j)*2]);
			}
			if( sum > last )	//if positive slope
			{
				pslope = TRUE;
				if( pslope != lastslope ) //if is at a min
					min = sum;
			}
			else
			{
				pslope = FALSE;
				if(pslope!=lastslope)	//if is at a max
				{
					max = sum;
					if( max > min*CRATIO )
					{
						Start = (INT)( FREQ_SCALE*(double)(i+CINTLEN/2) );
						return FindPKfreq(Start, 40);
					}
				}
			}
			lastslope = pslope;
			last = sum;
		}
		return Startf;
	}	
	else		//Backward search
	{
		for( INT i=Start-CINTLEN/2; i>(Stop+CINTLEN); i--)
		{
			sum = 0.0;
			for( INT j=0; j<CINTLEN; j++)
			{
				sum += (pFFTAveBuf[(i-j)*2]);
			}
			if( sum > last )
			{
				pslope = TRUE;
				if( pslope != lastslope)
					min = sum;
			}
			else
			{
				pslope = FALSE;
				if(pslope!=lastslope)
				{
					max = sum;
					if( max > min*CRATIO )
					{
						Start = (INT)( FREQ_SCALE*(double)(i-CINTLEN/2) );
						return FindPKfreq(Start, 40);
					}
				}
			}
			lastslope = pslope;
			last = sum;
		}
		return Startf;
	}
}

// Nitty gritty fft routines by Takuya OOURA
void Cfft::rftfsub(int n, double *a, int nc, double *c)
{
double tmp;
    int j, k, kk, ks, m;
    double wkr, wki, xr, xi, yr, yi;
    
    ks = (nc << 2) / n;
    kk = 0;
    m = n >> 1;
	if(m_LogMode)
	{
		for (k = 2; k < m; k += 2 ) 
		{
			j = n - k;
			kk += ks;
			wkr = 0.5 - c[nc - kk];
			wki = c[kk];
			xr = a[k] - a[j];
			xi = a[k + 1] + a[j + 1];
			yr = wkr * xr - wki * xi;
			yi = wkr * xi + wki * xr;
			a[k] -= yr;
			a[k+1] -= yi;
			xi = a[k]*a[k];
			xi += ( a[k+1]*a[k+1]);
			a[j] += yr;
			xr = a[j]*a[j];
			a[j+1] -= yi;
			xr += (a[j+1]*a[j+1]);
			tmp = log10(xi+K_C) + K_B;
			if( (tmp -= m_Clip)<0.0 )
				pFFTAveBuf[k] = (1.0-1.0/m_AveSize)*pFFTAveBuf[k];
			else
 				pFFTAveBuf[k] = (1.0-1.0/m_AveSize)*pFFTAveBuf[k] +
									(1.0/m_AveSize)*tmp;
			tmp = log10(xr+K_C) + K_B;
			if( (tmp -= m_Clip)<0.0 )
				pFFTAveBuf[j] = (1.0-1.0/m_AveSize)*pFFTAveBuf[j];
			else
 				pFFTAveBuf[j] = (1.0-1.0/m_AveSize)*pFFTAveBuf[j] +
									(1.0/m_AveSize)*tmp;
		}
		a[0] *= a[0];						//calc DC term
		xr = a[m]*a[m]+a[m+1]*a[m+1];		//calculate N/4(middle) term
		tmp = log10(a[0]+K_C) + K_B;
		if( (tmp -= m_Clip)<0.0 )
			pFFTAveBuf[0] = (1.0-1.0/m_AveSize)*pFFTAveBuf[0];
		else
 			pFFTAveBuf[0] = (1.0-1.0/m_AveSize)*pFFTAveBuf[0] +
								(1.0/m_AveSize)*tmp;

		tmp = log10(xr+K_C) + K_B;
		if( (tmp -= m_Clip)<0.0 )
			pFFTAveBuf[n/2] = (1.0-1.0/m_AveSize)*pFFTAveBuf[n/2];
		else
 			pFFTAveBuf[n/2] = (1.0-1.0/m_AveSize)*pFFTAveBuf[n/2] +
								(1.0/m_AveSize)*tmp;
	}
	else
	{
		for (k = 2; k < m; k += 2 ) 
		{
			j = n - k;
			kk += ks;
			wkr = 0.5 - c[nc - kk];
			wki = c[kk];
			xr = a[k] - a[j];
			xi = a[k + 1] + a[j + 1];
			yr = wkr * xr - wki * xi;
			yi = wkr * xi + wki * xr;
			a[k] -= yr;
			xi = a[k]*a[k];
			a[k+1] -= yi;
			xi += ( a[k+1]*a[k+1]);
			a[j] += yr;
			xr = a[j]*a[j];
			a[j+1] -= yi;
			xr += (a[j+1]*a[j+1]);

			pFFTAveBuf[k] = (1.0-1.0/m_AveSize)*pFFTAveBuf[k] + 
					(1.0/m_AveSize)*pow(xi , K_ROOT)/K_ROOTGN;
 			pFFTAveBuf[j] = (1.0-1.0/m_AveSize)*pFFTAveBuf[j] +
					(1.0/m_AveSize)*pow(xr , K_ROOT)/K_ROOTGN;

		}
		a[0] *= a[0];						//calc DC term
		xr = a[m]*a[m]+a[m+1]*a[m+1];		//calculate N/4(middle) term
		pFFTAveBuf[0] = (1.0-1.0/m_AveSize)*pFFTAveBuf[0] + 
				(1.0/m_AveSize)*pow(a[0] , K_ROOT)/K_ROOTGN;
		pFFTAveBuf[n/2] = (1.0-1.0/m_AveSize)*pFFTAveBuf[n/2] + 
				(1.0/m_AveSize)*pow(xr , K_ROOT)/K_ROOTGN;
	}
}

/* -------- initializing routines -------- */
void Cfft::makewt(int nw, int *ip, double *w)
{
    int nwh, j;
    double delta, x, y;
    
    ip[0] = nw;
    ip[1] = 1;
    if (nw > 2) {
        nwh = nw >> 1;
        delta = atan(1.0) / nwh;
        w[0] = 1;
        w[1] = 0;
        w[nwh] = cos(delta * nwh);
        w[nwh + 1] = w[nwh];
        for (j = 2; j < nwh; j += 2) {
            x = cos(delta * j);
            y = sin(delta * j);
            w[j] = x;
            w[j + 1] = y;
            w[nw - j] = y;
            w[nw - j + 1] = x;
        }
        bitrv2(nw, ip + 2, w);
    }
}


void Cfft::makect(int nc, int *ip, double *c)
{
    int nch, j;
    double delta;
    
    ip[1] = nc;
    if (nc > 1) {
        nch = nc >> 1;
        delta = atan(1.0) / nch;
        c[0] = cos(delta * nch);
        c[nch] = 0.5 * c[0];
        for (j = 1; j < nch; j++) {
            c[j] = 0.5 * cos(delta * j);
            c[nc - j] = 0.5 * sin(delta * j);
        }
    }
}


/* -------- child routines -------- */
void Cfft::bitrv2(int n, int *ip, double *a)
{
    int j, j1, k, k1, l, m, m2;
    double xr, xi;
    
    ip[0] = 0;
    l = n;
    m = 1;
    while ((m << 2) < l) {
        l >>= 1;
        for (j = 0; j < m; j++) {
            ip[m + j] = ip[j] + l;
        }
        m <<= 1;
    }
    if ((m << 2) > l) {
        for (k = 1; k < m; k++) {
            for (j = 0; j < k; j++) {
                j1 = (j << 1) + ip[k];
                k1 = (k << 1) + ip[j];
                xr = a[j1];
                xi = a[j1 + 1];
                a[j1] = a[k1];
                a[j1 + 1] = a[k1 + 1];
                a[k1] = xr;
                a[k1 + 1] = xi;
            }
        }
    } else {
        m2 = m << 1;
        for (k = 1; k < m; k++) {
            for (j = 0; j < k; j++) {
                j1 = (j << 1) + ip[k];
                k1 = (k << 1) + ip[j];
                xr = a[j1];
                xi = a[j1 + 1];
                a[j1] = a[k1];
                a[j1 + 1] = a[k1 + 1];
                a[k1] = xr;
                a[k1 + 1] = xi;
                j1 += m2;
                k1 += m2;
                xr = a[j1];
                xi = a[j1 + 1];
                a[j1] = a[k1];
                a[j1 + 1] = a[k1 + 1];
                a[k1] = xr;
                a[k1 + 1] = xi;
            }
        }
    }
}

void Cfft::cftfsub(int n, double *a, double *w)
{
    int j, j1, j2, j3, l;
    double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;
    
    l = 2;
    if (n > 8) {
        cft1st(n, a, w);
        l = 8;
        while ((l << 2) < n) {
            cftmdl(n, l, a, w);
            l <<= 2;
        }
    }
    if ((l << 2) == n) {
        for (j = 0; j < l; j += 2) {
            j1 = j + l;
            j2 = j1 + l;
            j3 = j2 + l;
            x0r = a[j] + a[j1];
            x0i = a[j + 1] + a[j1 + 1];
            x1r = a[j] - a[j1];
            x1i = a[j + 1] - a[j1 + 1];
            x2r = a[j2] + a[j3];
            x2i = a[j2 + 1] + a[j3 + 1];
            x3r = a[j2] - a[j3];
            x3i = a[j2 + 1] - a[j3 + 1];
            a[j] = x0r + x2r;
            a[j + 1] = x0i + x2i;
            a[j2] = x0r - x2r;
            a[j2 + 1] = x0i - x2i;
            a[j1] = x1r - x3i;
            a[j1 + 1] = x1i + x3r;
            a[j3] = x1r + x3i;
            a[j3 + 1] = x1i - x3r;
        }
    } else {
        for (j = 0; j < l; j += 2) {
            j1 = j + l;
            x0r = a[j] - a[j1];
            x0i = a[j + 1] - a[j1 + 1];
            a[j] += a[j1];
            a[j + 1] += a[j1 + 1];
            a[j1] = x0r;
            a[j1 + 1] = x0i;
        }
    }
}



void Cfft::cft1st(int n, double *a, double *w)
{
    int j, k1, k2;
    double wk1r, wk1i, wk2r, wk2i, wk3r, wk3i;
    double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;
    
    x0r = a[0] + a[2];
    x0i = a[1] + a[3];
    x1r = a[0] - a[2];
    x1i = a[1] - a[3];
    x2r = a[4] + a[6];
    x2i = a[5] + a[7];
    x3r = a[4] - a[6];
    x3i = a[5] - a[7];
    a[0] = x0r + x2r;
    a[1] = x0i + x2i;
    a[4] = x0r - x2r;
    a[5] = x0i - x2i;
    a[2] = x1r - x3i;
    a[3] = x1i + x3r;
    a[6] = x1r + x3i;
    a[7] = x1i - x3r;
    wk1r = w[2];
    x0r = a[8] + a[10];
    x0i = a[9] + a[11];
    x1r = a[8] - a[10];
    x1i = a[9] - a[11];
    x2r = a[12] + a[14];
    x2i = a[13] + a[15];
    x3r = a[12] - a[14];
    x3i = a[13] - a[15];
    a[8] = x0r + x2r;
    a[9] = x0i + x2i;
    a[12] = x2i - x0i;
    a[13] = x0r - x2r;
    x0r = x1r - x3i;
    x0i = x1i + x3r;
    a[10] = wk1r * (x0r - x0i);
    a[11] = wk1r * (x0r + x0i);
    x0r = x3i + x1r;
    x0i = x3r - x1i;
    a[14] = wk1r * (x0i - x0r);
    a[15] = wk1r * (x0i + x0r);
    k1 = 0;
    for (j = 16; j < n; j += 16) {
        k1 += 2;
        k2 = k1 << 1;
        wk2r = w[k1];
        wk2i = w[k1 + 1];
        wk1r = w[k2];
        wk1i = w[k2 + 1];
        wk3r = wk1r - 2 * wk2i * wk1i;
        wk3i = 2 * wk2i * wk1r - wk1i;
        x0r = a[j] + a[j + 2];
        x0i = a[j + 1] + a[j + 3];
        x1r = a[j] - a[j + 2];
        x1i = a[j + 1] - a[j + 3];
        x2r = a[j + 4] + a[j + 6];
        x2i = a[j + 5] + a[j + 7];
        x3r = a[j + 4] - a[j + 6];
        x3i = a[j + 5] - a[j + 7];
        a[j] = x0r + x2r;
        a[j + 1] = x0i + x2i;
        x0r -= x2r;
        x0i -= x2i;
        a[j + 4] = wk2r * x0r - wk2i * x0i;
        a[j + 5] = wk2r * x0i + wk2i * x0r;
        x0r = x1r - x3i;
        x0i = x1i + x3r;
        a[j + 2] = wk1r * x0r - wk1i * x0i;
        a[j + 3] = wk1r * x0i + wk1i * x0r;
        x0r = x1r + x3i;
        x0i = x1i - x3r;
        a[j + 6] = wk3r * x0r - wk3i * x0i;
        a[j + 7] = wk3r * x0i + wk3i * x0r;
        wk1r = w[k2 + 2];
        wk1i = w[k2 + 3];
        wk3r = wk1r - 2 * wk2r * wk1i;
        wk3i = 2 * wk2r * wk1r - wk1i;
        x0r = a[j + 8] + a[j + 10];
        x0i = a[j + 9] + a[j + 11];
        x1r = a[j + 8] - a[j + 10];
        x1i = a[j + 9] - a[j + 11];
        x2r = a[j + 12] + a[j + 14];
        x2i = a[j + 13] + a[j + 15];
        x3r = a[j + 12] - a[j + 14];
        x3i = a[j + 13] - a[j + 15];
        a[j + 8] = x0r + x2r;
        a[j + 9] = x0i + x2i;
        x0r -= x2r;
        x0i -= x2i;
        a[j + 12] = -wk2i * x0r - wk2r * x0i;
        a[j + 13] = -wk2i * x0i + wk2r * x0r;
        x0r = x1r - x3i;
        x0i = x1i + x3r;
        a[j + 10] = wk1r * x0r - wk1i * x0i;
        a[j + 11] = wk1r * x0i + wk1i * x0r;
        x0r = x1r + x3i;
        x0i = x1i - x3r;
        a[j + 14] = wk3r * x0r - wk3i * x0i;
        a[j + 15] = wk3r * x0i + wk3i * x0r;
    }
}


void Cfft::cftmdl(int n, int l, double *a, double *w)
{
    int j, j1, j2, j3, k, k1, k2, m, m2;
    double wk1r, wk1i, wk2r, wk2i, wk3r, wk3i;
    double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;
    
    m = l << 2;
    for (j = 0; j < l; j += 2) {
        j1 = j + l;
        j2 = j1 + l;
        j3 = j2 + l;
        x0r = a[j] + a[j1];
        x0i = a[j + 1] + a[j1 + 1];
        x1r = a[j] - a[j1];
        x1i = a[j + 1] - a[j1 + 1];
        x2r = a[j2] + a[j3];
        x2i = a[j2 + 1] + a[j3 + 1];
        x3r = a[j2] - a[j3];
        x3i = a[j2 + 1] - a[j3 + 1];
        a[j] = x0r + x2r;
        a[j + 1] = x0i + x2i;
        a[j2] = x0r - x2r;
        a[j2 + 1] = x0i - x2i;
        a[j1] = x1r - x3i;
        a[j1 + 1] = x1i + x3r;
        a[j3] = x1r + x3i;
        a[j3 + 1] = x1i - x3r;
    }
    wk1r = w[2];
    for (j = m; j < l + m; j += 2) {
        j1 = j + l;
        j2 = j1 + l;
        j3 = j2 + l;
        x0r = a[j] + a[j1];
        x0i = a[j + 1] + a[j1 + 1];
        x1r = a[j] - a[j1];
        x1i = a[j + 1] - a[j1 + 1];
        x2r = a[j2] + a[j3];
        x2i = a[j2 + 1] + a[j3 + 1];
        x3r = a[j2] - a[j3];
        x3i = a[j2 + 1] - a[j3 + 1];
        a[j] = x0r + x2r;
        a[j + 1] = x0i + x2i;
        a[j2] = x2i - x0i;
        a[j2 + 1] = x0r - x2r;
        x0r = x1r - x3i;
        x0i = x1i + x3r;
        a[j1] = wk1r * (x0r - x0i);
        a[j1 + 1] = wk1r * (x0r + x0i);
        x0r = x3i + x1r;
        x0i = x3r - x1i;
        a[j3] = wk1r * (x0i - x0r);
        a[j3 + 1] = wk1r * (x0i + x0r);
    }
    k1 = 0;
    m2 = m << 1;
    for (k = m2; k < n; k += m2) {
        k1 += 2;
        k2 = k1 << 1;
        wk2r = w[k1];
        wk2i = w[k1 + 1];
        wk1r = w[k2];
        wk1i = w[k2 + 1];
        wk3r = wk1r - 2 * wk2i * wk1i;
        wk3i = 2 * wk2i * wk1r - wk1i;
        for (j = k; j < l + k; j += 2) {
            j1 = j + l;
            j2 = j1 + l;
            j3 = j2 + l;
            x0r = a[j] + a[j1];
            x0i = a[j + 1] + a[j1 + 1];
            x1r = a[j] - a[j1];
            x1i = a[j + 1] - a[j1 + 1];
            x2r = a[j2] + a[j3];
            x2i = a[j2 + 1] + a[j3 + 1];
            x3r = a[j2] - a[j3];
            x3i = a[j2 + 1] - a[j3 + 1];
            a[j] = x0r + x2r;
            a[j + 1] = x0i + x2i;
            x0r -= x2r;
            x0i -= x2i;
            a[j2] = wk2r * x0r - wk2i * x0i;
            a[j2 + 1] = wk2r * x0i + wk2i * x0r;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[j1] = wk1r * x0r - wk1i * x0i;
            a[j1 + 1] = wk1r * x0i + wk1i * x0r;
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[j3] = wk3r * x0r - wk3i * x0i;
            a[j3 + 1] = wk3r * x0i + wk3i * x0r;
        }
        wk1r = w[k2 + 2];
        wk1i = w[k2 + 3];
        wk3r = wk1r - 2 * wk2r * wk1i;
        wk3i = 2 * wk2r * wk1r - wk1i;
        for (j = k + m; j < l + (k + m); j += 2) {
            j1 = j + l;
            j2 = j1 + l;
            j3 = j2 + l;
            x0r = a[j] + a[j1];
            x0i = a[j + 1] + a[j1 + 1];
            x1r = a[j] - a[j1];
            x1i = a[j + 1] - a[j1 + 1];
            x2r = a[j2] + a[j3];
            x2i = a[j2 + 1] + a[j3 + 1];
            x3r = a[j2] - a[j3];
            x3i = a[j2 + 1] - a[j3 + 1];
            a[j] = x0r + x2r;
            a[j + 1] = x0i + x2i;
            x0r -= x2r;
            x0i -= x2i;
            a[j2] = -wk2i * x0r - wk2r * x0i;
            a[j2 + 1] = -wk2i * x0i + wk2r * x0r;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[j1] = wk1r * x0r - wk1i * x0i;
            a[j1 + 1] = wk1r * x0i + wk1i * x0r;
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[j3] = wk3r * x0r - wk3i * x0i;
            a[j3 + 1] = wk3r * x0i + wk3i * x0r;
        }
    }
}


