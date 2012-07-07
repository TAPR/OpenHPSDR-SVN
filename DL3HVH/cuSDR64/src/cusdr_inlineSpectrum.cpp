/**
* @file  cusdr_inlineSpectrum.cpp
* @brief Inline Power Spectrum class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-05-14
*/

/*   
 *   Copyright (C) 2007, 2008, 2009, 2010 Philip A Covington, N8VB
 *
 *	 adapted for cuSDR by (C) 2012 Hermann von Hasseln, DL3HVH
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

#include "cusdr_inlineSpectrum.h"

QInlineSpectrum::QInlineSpectrum(QObject *parent, int size)
	: QObject(parent)
	, m_settings(Settings::instance())
	, m_size(size)
	, m_samplerate(m_settings->getSampleRate())
	, m_baseline(1.0e-15)
	, m_correction(0.0)
	, m_psswitch(0)
	, m_averages(4)
{
    m_window = new float[m_size];
	m_fPsdBm = new float[m_size * 2];
	m_fAvePsdBm = new float[m_size * 2];

    m_windowCPX = mallocCPX(m_size);
    m_tmpCPX = mallocCPX(m_size * 2);

    m_fft = new QFFT(this, m_size * 2);

    memset(m_fPsdBm, 0, m_size * 2 * sizeof(float));
	memset(m_fAvePsdBm, 0, m_size * 2 * sizeof(float));
    memset(m_window, 0, m_size  * sizeof(float));
    memset(m_tmpCPX, 0, m_size * 2 * sizeof(CPX));

    QFilter::MakeWindow(BLACKMANHARRIS_WINDOW, size, m_window);

    for (int i = 0; i < m_size; i++) {

        m_windowCPX[i].re = m_window[i];
        m_windowCPX[i].im = m_window[i];
    }
}

QInlineSpectrum::~QInlineSpectrum() {

	if (m_fft)
		delete m_fft;
}

void QInlineSpectrum::ProcessSpectrum(CPX* in, int size) {

    //if (m_psswitch == 0) return;

    memset(m_tmpCPX, 0, sizeof(CPX) * m_size * 2);
    //SSEMultCPX(tmp_cpx, in, window_cpx, m_size);
	/*inline CPX MultCPX(CPX x, CPX y) {
		CPX z;
		z.re = x.re * y.re - x.im * y.im;
		z.im = x.im * y.re + x.re * y.im;
		return z;
	}*/
	for (int i = 0; i < m_size; i++) {

		m_tmpCPX[i].re = in[i].re * m_windowCPX[i].re - in[i].im * m_windowCPX[i].im;
		m_tmpCPX[i].im = in[i].im * m_windowCPX[i].re + in[i].re * m_windowCPX[i].im;
	}

    mutex.lock();
    m_fft->DoFFTWMagnForward(m_tmpCPX, m_size * 2, m_baseline, m_correction, m_fPsdBm);
    mutex.unlock();
}

void QInlineSpectrum::setBaseLine(float value) {

    m_baseline = value;
}

void QInlineSpectrum::setCorrection(float value) {

    m_correction = value;
}
  
void QInlineSpectrum::setPsOn(int value) {

    m_psswitch = value;
}

int QInlineSpectrum::psdBmResults(float* buffer) {

    if (buffer == NULL) return 0;
    
	mutex.lock();
    memcpy(buffer, m_fPsdBm,  dBmSize() * sizeof(float));
    mutex.unlock();

    return dBmSize();
}

void QInlineSpectrum::setAverages(int value) {

	m_averages = value;
}

float QInlineSpectrum::grabPsPoint(int index) { 
	
	return m_fPsdBm[index]; 
}