/**
* @file  qtdsp_powerSpectrum.cpp
* @brief Power Spectrum class for QtDSP
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-05-14
*/

/*   
 *   Copyright (C) 2007, 2008, 2009, 2010 Philip A Covington, N8VB
 *
 *	 adapted for QtDSP by (C) 2012 Hermann von Hasseln, DL3HVH
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

#include "qtdsp_powerSpectrum.h"

PowerSpectrum::PowerSpectrum(QObject *parent, int size)
	: QObject(parent)
	, set(Settings::instance())
	, first(true)
	, m_size(size)
	, m_psswitch(0)
	, m_averages(4)
	, m_samplerate(set->getSampleRate())
	, m_baseline((float)1.0e-15)
	, m_correction(0.0f)
{
    m_window = new float[m_size];
	m_fPsdBm = new float[m_size * 2];
	m_fAvePsdBm = new float[m_size * 2];

    windowCPX.resize(m_size);
    tmpCPX.resize(m_size * 2);
    dataCPX.resize(0);

    zero.re = 0.0f;
    zero.im = 0.0f;

    m_fft = new QFFT(this, m_size * 2);

    memset(m_fPsdBm, 0, m_size * 2 * sizeof(float));
	memset(m_fAvePsdBm, 0, m_size * 2 * sizeof(float));
    memset(m_window, 0, m_size  * sizeof(float));

    QFilter::MakeWindow(BLACKMANHARRIS_WINDOW, size, m_window);

    for (int i = 0; i < m_size; i++) {

        windowCPX[i].re = m_window[i];
        windowCPX[i].im = m_window[i];
    }
}

PowerSpectrum::~PowerSpectrum() {

	if (m_fft)
		delete m_fft;

	windowCPX.clear();
    tmpCPX.clear();
    dataCPX.clear();

    if (m_window)
    	delete m_window;

    if (m_fPsdBm)
    	delete m_fPsdBm;

    if (m_fAvePsdBm)
    	delete m_fAvePsdBm;
}

void PowerSpectrum::ProcessSpectrum(CPX &in, int size) {

	Q_UNUSED(size)

	if (first && dataCPX.size() == 0) {

    	dataCPX << in;
    	first = false;
		return;
	}
	else {

		dataCPX << in;
		tmpCPX.fill(zero, m_size*2);

		if (dataCPX.size() == m_size) {

			for (int i = 0; i < m_size; i++)
				tmpCPX[i] = MultCPX(dataCPX.at(i), windowCPX.at(i));

			mutex.lock();
			m_fft->DoFFTWMagnForward(tmpCPX, m_size * 2, m_baseline, m_correction, m_fPsdBm);
			mutex.unlock();
		}

		first = true;
		dataCPX.resize(0);
	}
}

void PowerSpectrum::setBaseLine(float value) {

    m_baseline = value;
}

void PowerSpectrum::setCorrection(float value) {

    m_correction = value;
}
  
void PowerSpectrum::setPsOn(int value) {

    m_psswitch = value;
}

int PowerSpectrum::psdBmResults(float* buffer) {

    if (buffer == NULL) return 0;
    
	mutex.lock();
    memcpy(buffer, m_fPsdBm,  dBmSize() * sizeof(float));
    mutex.unlock();

    return dBmSize();
}

void PowerSpectrum::setAverages(int value) {

	m_averages = value;
}

float PowerSpectrum::grabPsPoint(int index) {
	
	return m_fPsdBm[index]; 
}
