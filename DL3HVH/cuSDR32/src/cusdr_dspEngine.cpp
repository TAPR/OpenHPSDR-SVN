/**
* @file  cusdr_dspEngine.cpp
* @brief QtDSP DSP engine class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-04-07
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

#include "cusdr_dspEngine.h"


QDSPEngine::QDSPEngine(QObject *parent, int rx, int size)
	: QObject(parent)
	, m_rx(rx)
	, m_size(size)
	, m_settings(Settings::instance())
	, m_qtdspOn(false)
	, m_volume(0.0f)
{
	fft    = new QFFT(this, size);
	filter = new QFilter(this, size, 2, 8);
	agc    = new QAGC(this, size);
	wpagc  = new QWPAGC(this, size);
	spectrum = new QInlineSpectrum(this, size);

	m_rxData = m_settings->getReceiverDataList().at(rx);
	m_agcMode = m_rxData.agcMode;
}

QDSPEngine::~QDSPEngine() {

	if (agc)
		delete agc;

	if (fft)
		delete fft;

	if (filter)
		delete filter;
}

void QDSPEngine::processDSP(CPX* in, CPX* out,  int size) {

	spectrum->ProcessSpectrum(in, size);
	filter->ProcessFilter(in, out, size);
	//agc->ProcessAGC(out, in, size);
	wpagc->ProcessAGC(out, in, size);
	
	memcpy(out, in, size * sizeof(CPX));
	for (int i = 0; i < size; i++) {

		out[i].re *= m_volume;
		out[i].im *= m_volume;
		/*in[i].re *= m_volume;
		in[i].im *= m_volume;*/
	}
}

int QDSPEngine::getSpectrum(float* buffer) {

	return spectrum->psdBmResults(buffer);
}

void QDSPEngine::setVolume(float value) {

	if (m_volume == value) return;

	m_volume = value;
}

void QDSPEngine::setQtDSPStatus(bool value) { 
	
	m_qtdspOn = value; 
}

void QDSPEngine::setAGCMode(AGCMode mode) {

	agc->setMode(mode);
	wpagc->setMode(mode);
}