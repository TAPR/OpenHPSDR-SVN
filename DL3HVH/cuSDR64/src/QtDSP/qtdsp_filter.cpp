/**
* @file  qtdsp_filter.cpp
* @brief Filter class for QtDSP
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-02-18
*/

/*   
 *   Copyright (C) 2007, 2008 Philip A Covington, N8VB
 *
 *	 adapted for QtDSP by (C) 2011 Hermann von Hasseln, DL3HVH
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Library General Public License version 2 as
 *   published by the Free Software Foundation
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details
 *
 *   You should have received a copy of the GNU Library General Public
 *   License along with this program; if not, write to the
 *   Free Software Foundation, Inc.,
 *   51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */


#include "qtdsp_filter.h"

//#include <cstring>

#define LOG_QTDSP_FILTER

// use: FILTER_DEBUG

#ifndef NULL
#define NULL 0
#endif

QFilter::QFilter(QObject *parent, int size, const int ftype, const int wtype)
	: QObject(parent)
	, set(Settings::instance())
	, m_streamMode(true)
	, m_size(size)
	, m_ftype(ftype)
	, m_wtype(wtype)
	, m_samplerate(set->getSampleRate())
	, m_filter_lo(-3050.0f)
	, m_filter_hi(-150.0f)
{
	InitCPX(ovlp, 		m_size, 0.0f);
	InitCPX(tmp, 		m_size, 0.0f);
	InitCPX(res, 		m_size, 0.0f);
	InitCPX(filter, 	m_size * 2, 0.0f);
	InitCPX(tmp0, 		m_size * 2, 0.0f);
	InitCPX(tmp1, 		m_size * 2, 0.0f);
	InitCPX(tmp2, 		m_size * 2, 0.0f);
	InitCPX(tmpfilt0, 	m_size * 2, 0.0f);
	InitCPX(tmpfilt1, 	m_size * 2, 0.0f);

    ovlpfft = new QFFT(this, m_size * 2);
    filtfft = new QFFT(this, m_size * 2);

    MakeFilter(m_filter_lo, m_filter_hi, m_ftype, m_wtype);
}

QFilter::~QFilter() {

	if (ovlpfft) delete ovlpfft;
	if (filtfft) delete filtfft;

    ovlp.clear();
    filter.clear();
    tmpfilt0.clear();
    tmpfilt1.clear();
	tmp.clear();
    tmp0.clear();
    tmp1.clear();

}

void QFilter::DoConvolutionCPX() {

    mutex.lock();
    for (int i = 0; i < m_size * 2; i++)  //convolution in frequency space here
    {
        tmp0[i].re = (filter.at(i).re * tmp1.at(i).re) - (filter.at(i).im * tmp1.at(i).im);
        tmp0[i].im = (filter.at(i).re * tmp1.at(i).im) + (filter.at(i).im * tmp1.at(i).re);
    }
    mutex.unlock();
}

void QFilter::DoOverlapAddCPX() {

    //SSEAddCPX(tmp0, tmp1, ovlp, m_size);
	for (int i = 0; i < m_size; i++) {

		//tmp0[i] = AddCPX(tmp1.at(i), ovlp.at(i));
		//ovlp[i] = tmp1.at(i + m_size);
		res[i] = AddCPX(tmp2.at(i), ovlp.at(i));
		ovlp[i] = tmp2.at(i + m_size);
	}
}

void QFilter::ProcessFilter(CPX &in, CPX &out, int bsize) {

	Q_UNUSED (bsize)

	memcpy(tmp0.data(), in.data(), sizeof(cpx) * m_size);
    ovlpfft->DoFFTWForward(tmp0, tmp1, m_size * 2);
    DoConvolutionCPX();
    ovlpfft->DoFFTWInverse(tmp0, tmp1, m_size * 2);

    if (m_streamMode) {

    	//Normalize(tmp1, tmp2, m_size * 2);

    	// Overlap-Add
    	for (int i = 0; i < m_size; i++) {

    		out[i] = AddCPX(tmp1.at(i), ovlp.at(i));
    		ovlp[i] = tmp1.at(i + m_size);
    	}
		//DoOverlapAddCPX();
		//Normalize(tmp0, out, m_size * 2);
		//memcpy(out.data(), res.data(), sizeof(cpx) * m_size);
    }
    else {
		
		memcpy(out.data(), tmp1.data(), sizeof(cpx) * m_size);
    }
}

void QFilter::ProcessChirpFilter(CPX &in, CPX &out, int bsize) {

    Q_UNUSED(in)
    Q_UNUSED(out)
    Q_UNUSED(bsize)
}

void QFilter::ProcessForwardFilter(CPX &in, CPX &out, int bsize) {

	Q_UNUSED (bsize)

	//memcpy(tmp0, in, sizeof(CPX) * m_size);
	memcpy(tmp0.data(), in.data(), sizeof(cpx) * m_size);
    ovlpfft->DoFFTWForward(tmp0, tmp1, m_size * 2);

    DoConvolutionCPX();
    //ovlpfft->DoFFTWInverse(tmp0, tmp1, m_size * 2);

    if (m_streamMode) {

        DoOverlapAddCPX();
        //memcpy(out, tmp0, sizeof(CPX) * m_size);
        memcpy(out.data(), tmp0.data(), sizeof(cpx) * m_size);
    }
    else {
		
		//memcpy(out, tmp1, sizeof(CPX) * m_size);
    	memcpy(out.data(), tmp1.data(), sizeof(cpx) * m_size);
    }
}

int QFilter::ProcessAndDecimate(CPX &in, CPX &out, int bsize) {

    Q_UNUSED(in)
    Q_UNUSED(bsize)

    //memset(tmp0, 0, sizeof(CPX) * m_size * 2);
    //memset(tmp1, 0, sizeof(CPX) * m_size * 2);
    //memcpy(tmp0, in, sizeof(CPX) * m_size);
	tmp0.resize(m_size * 2);
	tmp1.resize(m_size * 2);
	tmp0.resize(m_size);

    ovlpfft->DoFFTWForward(tmp0, tmp1, m_size * 2);
    DoConvolutionCPX();
    ovlpfft->DoFFTWInverse(tmp0, tmp1, m_size * 2);

    if (m_streamMode) {
		
		DoOverlapAddCPX();
        for (int i = 0, j = 0; i < m_size; i+=2, j++) {
			
			*(out.data()+j) = *(tmp0.data()+i);
        }
    }
	else {
		
		for (int i = 0, j = 0; i < m_size; i+=2, j++) {
			
			*(out.data()+j) = *(tmp1.data()+i);
        }
    }
    return m_size/2;
}

void QFilter::Normalize(CPX &in, CPX &out, int size) {

	float norm = 1.0f/size;
	for (int i = 0; i < size; i++) {

		out[i].re = in.at(i).re * norm;
		out[i].im = in.at(i).im * norm;
	}
}

void QFilter::Decimate(CPX &in, CPX &out, int downrate) {

	int newsize = qRound(m_size/downrate);

	//memset(out, 0, newsize * sizeof(CPX));
	//memset(tmp, 0, m_size * sizeof(CPX));
	out.resize(newsize);
	tmp.resize(m_size);
	//memcpy(tmp, in, m_size * sizeof(CPX));
	memcpy(tmp.data(), in.data(), m_size * sizeof(cpx));

	for (int j = 0; j < newsize; j++) {
		for (int k = 0; k < downrate; k++) {
		//for (int k = 0; k < downrate - 1; k++) {

			if (j * downrate + k < m_size) {

				out[j].re += tmp.at(j * downrate + k).re;
				out[j].im += tmp.at(j * downrate + k).im;
			}
		}
	}
}

void QFilter::setSampleRate(QObject *sender, int value) {

	Q_UNUSED(sender)

	m_samplerate = (float)value;
	//FILTER_DEBUG << "set sample rate to " << m_samplerate;
	MakeFilter(m_filter_lo, m_filter_hi, m_ftype, m_wtype);
}

void QFilter::setFilterLo(const float value) {

    if (value != m_filter_lo) {
		
		m_filter_lo = value;
        MakeFilter(m_filter_lo, m_filter_hi, m_ftype, m_wtype);
    }
}

void QFilter::setFilterHi(const float value) {

    if (value != m_filter_hi) {

        m_filter_hi = value;
        MakeFilter(m_filter_lo, m_filter_hi, m_ftype, m_wtype);
    }
}

void QFilter::setFilter(const float loval, const float hival) {

    if (m_filter_lo != loval || m_filter_hi != hival) {

        m_filter_lo = loval;
        m_filter_hi = hival;
        MakeFilter(m_filter_lo, m_filter_hi, m_ftype, m_wtype);
    }
}

float QFilter::filterLo() const {

    return m_filter_lo;
}

float QFilter::filterHi() const {

    return m_filter_hi;
}

int QFilter::isStreamMode() const {

    return m_streamMode;
}

void QFilter::setStreamMode(int value) {

	m_streamMode = value;
}

void QFilter::MakeFilter(const float lo, const float hi, const int ftype = 2, const int wtype = 12) {
    
    // size x 4 adjusts for no gain
    //float one_over_norm = 1.0 / (m_size * 4);
	float one_over_norm = 1.0 / (m_size * 2);
	
    filter.resize(m_size * 2);
    tmpfilt0.resize(m_size * 2);
    tmpfilt1.resize(m_size  * 2);

    switch (ftype) {

        case 1: // lowpass            
            MakeFirLowpass( hi,
                            m_samplerate,
                            wtype,
                            tmpfilt0,
                            m_size);
            break;
        case 2: // bandpass            
            MakeFirBandpass(lo,
                            hi,
                            m_samplerate,
                            wtype,
                            tmpfilt0,
                            m_size);
            break;
        case 3: // loadable coeff
            LoadFilter(tmpfilt0);
            break;
        case 4: // bandstop
            MakeFirBandstop(lo,
                            hi,
							m_samplerate,
							tmpfilt0,
							m_size);
            break;
        default:            
            MakeFirBandpass(lo,
							hi,
							m_samplerate,
							wtype,
							tmpfilt0,
							m_size);
            break;
    }

    mutex.lock();
    //filtfft->DoFFTWForward(tmpfilt0, tmpfilt1, m_size * 2);
	filtfft->DoFFTWForward(tmpfilt0, filter, m_size * 2);

	for (int i = 0; i < m_size * 2; i++) {

			filter[i].re *= one_over_norm;
			filter[i].im *= one_over_norm;
	}
	mutex.unlock();

    // Do compensation here instead of in inverse FFT
    //filter_mutex.lock();
    //SSEScaleCPX(filter, tmpfilt1, one_over_norm, m_size * 2);
    //filter_mutex.unlock();
}

//void QFilter::LoadFilter(CPX * taps) {
void QFilter::LoadFilter(CPX &taps) {

    if (FILTERCOEFFSIZE > m_size) return;

    for (int i = 0; i < FILTERCOEFFSIZE; i++) {

        taps[i].re = FILTERCOEFF[i];
        taps[i].im = FILTERCOEFF[i];
    }
}

void QFilter::MakeFirLowpass(float cutoff, 
							 float samplerate, 
							 int wtype, 
							 float *taps_re,
							 float *taps_im,
							int length)
{
    
    //float window[length];
	float * window = 0;

    float fc = cutoff / samplerate;

    if (fc > 0.5) 
		return;

    int midpoint = length >> 1;

	window = (float *)malloc(length * sizeof(float));
	
	if (!window) return;

    MakeWindow(wtype, length, &window[0]);

    for (int i = 1; i <= length; i++) {

        int j = i - 1;
        if (i != midpoint) {

            taps_re[j] = (sin(TWOPI * (i - midpoint) * fc) / (ONEPI * (i - midpoint))) * window[j];
            taps_im[j] = (cos(TWOPI * (i - midpoint) * fc) / (ONEPI * (i - midpoint))) * window[j];
        }
        else {

            taps_re[midpoint - 1] = 2.0 * fc;
            taps_im[midpoint - 1] = 2.0 * fc;
        }
    }
	free(window);
}

void QFilter::MakeFirLowpass(float cutoff,
							 float samplerate,
							 int wtype,
							 CPX &taps,
							 int length) 
{

    //float window[length];
	float * window = 0;

    float fc = cutoff / samplerate;

    if (fc > 0.5)
		return;

    int midpoint = length >> 1;

	window = (float *)malloc(length * sizeof(float));
	
	if (!window) 
		return;

    MakeWindow(wtype, length, &window[0]);

    for (int i = 1; i <= length; i++) {

        int j = i - 1;
        if (i != midpoint) {

            taps[j].re = (sin(TWOPI * (i - midpoint) * fc) / (ONEPI * (i - midpoint))) * window[j];
            taps[j].im = (cos(TWOPI * (i - midpoint) * fc) / (ONEPI * (i - midpoint))) * window[j];
        }
        else {

            taps[midpoint - 1].re = 2.0 * fc;
            //taps[midpoint - 1].re = 2.0 * fc;
			taps[midpoint - 1].im = 2.0 * fc;
        }
    }
	free(window);
}

void QFilter::MakeFirBandpass(float lo,
							  float hi,
							  float samplerate,
							  int wtype,
							  float * taps_re,
							  float * taps_im,
							  int length) 
{
    
    //float window[length];
	float * window = 0;

    float fl = lo / samplerate;
    float fh = hi / samplerate;
    float fc = (fh - fl) / 2.0;
    float ff = (fl + fh) * ONEPI;

    int midpoint = length >> 1;

	window = (float *)malloc(length * sizeof(float));
	
	if (!window) return;

    MakeWindow(wtype, length, &window[0]);

    for (int i = 1; i <= length; i++) {

        int j = i - 1;
        int k = i - midpoint;
        float temp = 0.0;
        float phase = k * ff * -1;
        if (i != midpoint)
            temp = ((sin(TWOPI * k * fc) / (ONEPI * k))) * window[j];
        else
            temp = 2.0 * fc;
        temp *= 2.0;
        taps_re[j] = temp * (cos(phase));
        taps_im[j] = temp * (sin(phase));
    }
	free(window);
}

void QFilter::MakeFirBandpass(float lo,
							  float hi,
							  float samplerate,
							  int wtype,
							  CPX &taps,
							  int length) 
{

    //float window[length];
	float *window = 0;

    float fl = lo / samplerate;
    float fh = hi / samplerate;
    float fc = (fh - fl) / 2.0f;
    float ff = (fl + fh) * ONEPI;

    int midpoint = length >> 1;

	window = (float *)malloc(length * sizeof(float));
	
	if (!window) return;

    MakeWindow(wtype, length, &window[0]);

    for (int i = 1; i <= length; i++) {

        int j = i - 1;
        int k = i - midpoint;
        float temp = 0.0;
        float phase = k * ff * -1;

        if (i != midpoint)
            temp = ((qSin(TWOPI * k * fc) / (ONEPI * k))) * window[j];
        else
            temp = 2.0 * fc;

        temp *= 2.0;

        taps[j].re = temp * (qCos(phase));
        taps[j].im = temp * (qSin(phase));
    }
	free(window);
}

void QFilter::MakeFirBandstop(float lo, float hi, float samplerate, CPX &taps, int length) {

    //float window[length];
	float * window = 0;

    float fl = lo / samplerate;
    float fh = hi / samplerate;
    float fc = (fh - fl) / 2.0;
    float ff = (fl + fh) * ONEPI;

    int midpoint = (length >> 1) | 1;

	window = (float *)malloc(length * sizeof(float));
	
	if (!window) return;

    MakeWindow(12, length, &window[0]);

    for (int i = 1; i <= length; i++) {

        int j = i - 1;
        int k = i - midpoint;
        float temp = 0.0;
        float phase = k * ff * -1.0;

        if (i != midpoint) {

            temp = ((sin(TWOPI * k * fc) / (ONEPI * k))) * window[j];
            taps[j].re = -2.0 * temp * (cos(phase));
            taps[j].im = -2.0 * temp * (sin(phase));
        }
        else {

            temp = 4.0 * fc;
            taps[midpoint - 1].re = 1.0 - taps[midpoint - 1].re;
            taps[midpoint - 1].im = 0.0 - taps[midpoint - 1].im;
        }
    }
	free(window);
}

void QFilter::MakeWindow(int wtype, int size, float *window) {

    int i, j, midn, midp1, midm1;
    float freq, rate, sr1, angle, expn, expsum, cx, two_pi;

    midn = size / 2;
    midp1 = (size + 1) / 2;
    midm1 = (size - 1) / 2;
    two_pi = 8.0f * qAtan(1.0);
    freq = two_pi / size;
    rate = 1.0 /  midn;
    angle = 0.0;
    expn = log(2.0) / midn + 1.0;
    expsum = 1.0;

    switch (wtype) {

        case 1: // RECTANGULAR_WINDOW
            for (i = 0; i < size; i++)
                window[i] = 1.0;
            break;
        case 2:	// HANNING_WINDOW
            for (i = 0, j = size - 1, angle = 0.0; i <= midn; i++, j--, angle += freq)
                window[j] = (window[i] = 0.5 - 0.5 * cos(angle));
            break;
        case 3: // WELCH_WINDOW
            for (i = 0, j = size - 1; i <= midn; i++, j--)
                window[j] = (window[i] = 1.0 - (float)sqrt((float)((i - midm1) / midp1)));
            break;
        case 4: // PARZEN_WINDOW  
            for (i = 0, j = size - 1; i <= midn; i++, j--)
                window[j] = (window[i] = 1.0 - ((float)fabs((float)(i - midm1) / midp1)));             
            break;
        case 5: // BARTLETT_WINDOW
            for (i = 0, j = size - 1, angle = 0.0; i <= midn; i++, j--, angle += rate)
                window[j] = (window[i] = angle);
            break;
        case 6: // HAMMING_WINDOW
            for (i = 0, j = size - 1, angle = 0.0; i <= midn; i++, j--, angle += freq)
                window[j] = (window[i] = 0.5F - 0.46 * cos(angle));
            break;
        case 7:	// BLACKMAN2_WINDOW
            for (i = 0, j = size - 1, angle = 0.0; i <= midn; i++, j--, angle += freq) {
                cx = cos(angle);
                window[j] = (window[i] = (.34401 + (cx * (-.49755 + (cx * .15844)))));
            }
            break;
        case 8: // BLACKMAN3_WINDOW
            for (i = 0, j = size - 1, angle = 0.0; i <= midn; i++, j--, angle += freq) {
                cx = cos(angle);
                window[j] = (window[i] = (.21747 + (cx * (-.45325 + (cx * (.28256 - (cx * .04672)))))));
            }
            break;
        case 9: // BLACKMAN4_WINDOW
            for (i = 0, j = size - 1, angle = 0.0; i <= midn; i++, j--, angle += freq) 
            {                    
                cx = cos(angle);
                window[j] = (window[i] =
                            (.084037 +
                            (cx *
                            (-.29145 +
                            (cx *
                            (.375696 + (cx * (-.20762 + (cx * .041194)))))))));
            }
            break;
        case 10: // EXPONENTIAL_WINDOW
            for (i = 0, j = size - 1; i <= midn; i++, j--) {
                window[j] = (window[i] = expsum - 1.0);
                expsum *= expn;
            }
            break;
        case 11: // RIEMANN_WINDOW
            sr1 = two_pi / size;
            for (i = 0, j = size - 1; i <= midn; i++, j--) {
                if (i == midn) window[j] = (window[i] = 1.0);
                else {
                    cx = sr1 * (midn - i);
                    window[i] = sin(cx) / cx;
                    window[j] = window[i];
                }
            }
            break;
        case 12: // BLACKMANHARRIS_WINDOW
            {
                float 
                        a0 = 0.35875f,
                        a1 = 0.48829f,
                        a2 = 0.14128f,
                        a3 = 0.01168f;


                for (i = 0; i < size; i++) 
                {
                    window[i] = a0 - a1* qCos(TWOPI * (i+0.5)/size) 
                            + a2* qCos(2.0 * TWOPI * (i+0.5)/size)
                            - a3* qCos(3.0 * TWOPI * (i+0.5)/size);
                }
            }
            break;

        default:
            return;
    }    
}
