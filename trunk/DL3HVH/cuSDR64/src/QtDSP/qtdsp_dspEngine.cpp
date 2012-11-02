/**
* @file  qtdsp_dspEngine.cpp
* @brief QtDSP DSP engine class
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-04-07
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
#define LOG_DSP_ENGINE

// use: DSP_ENGINE_DEBUG << "debug message";

#include "qtdsp_dspEngine.h"


QDSPEngine::QDSPEngine(QObject *parent, int rx, int size)
	: QObject(parent)
	, set(Settings::instance())
	, m_qtdspOn(false)
	, m_rx(rx)
	, m_size(size)
	, m_samplerate(set->getSampleRate())
	, m_volume(0.0f)
{
	fft    		= new QFFT(this, m_size);
	filter 		= new QFilter(this, m_size, 2, 12);//8);
	//agc    		= new QAGC(this, m_size);
	wpagc  		= new QWPAGC(this, m_size);
	spectrum 	= new PowerSpectrum(this, m_size * 2);
	signalmeter = new SignalMeter(this, m_size);
	demod 		= new Demodulation(this, m_size);

	m_rxData = set->getReceiverDataList().at(rx);
	m_agcMode = m_rxData.agcMode;

	wpagc->setReceiver(m_rx);

    InitCPX(tmp1CPX, m_size, 0.0f);
    InitCPX(tmp2CPX, m_size, 0.0f);

	DSP_ENGINE_DEBUG << "init DSPEngine with size: " << m_size;
	SleeperThread::msleep(100);

	setupConnections();
}

QDSPEngine::~QDSPEngine() {

	tmp1CPX.clear();
	tmp2CPX.clear();

	//if (agc)
	//	delete agc;

	if (fft)
		delete fft;

	if (filter)
		delete filter;

	if (wpagc)
		delete wpagc;

	if (spectrum)
		delete spectrum;

	if (signalmeter)
		delete signalmeter;

	if (demod)
		delete demod;
}

void QDSPEngine::setupConnections() {

//	CHECKED_CONNECT(
//		set,
//		SIGNAL(sampleRateChanged(QObject *, int)),
//		this,
//		SLOT(setSampleRate(QObject *, int)));

	CHECKED_CONNECT(
		wpagc,
		SIGNAL(agcMaximumGainChanged(qreal)),
		this,
		SLOT(setAGCMaximumGain(qreal)));

	CHECKED_CONNECT(
		wpagc,
		SIGNAL(agcHangThresholdChanged(qreal)),
		this,
		SLOT(setAGCHangThreshold(qreal)));

//	CHECKED_CONNECT(
//		wpagc,
//		SIGNAL(hangLeveldBLineChanged(qreal)),
//		this,
//		SLOT(setAGCHangLeveldBLine(qreal)));
//
//	CHECKED_CONNECT(
//		wpagc,
//		SIGNAL(minimumVoltageChanged(QObject *, int, qreal)),
//		this,
//		SLOT(setAGCThresholdLine(QObject *, int, qreal)));

	CHECKED_CONNECT(
		wpagc,
		SIGNAL(displayValues(QObject *, int, qreal, qreal)),
		this,
		SLOT(setAGCLineValues(QObject *, int, qreal, qreal)));
}

void QDSPEngine::processDSP(CPX &in, CPX &out, int size) {

	m_mutex.lock();

	spectrum->ProcessSpectrum(in, size*2);
	filter->ProcessFilter(in, tmp1CPX, size);
	signalmeter->ProcessBlock(tmp1CPX, size);
	//agc->ProcessAGC(tmp1CPX, tmp1CPX, size);
	wpagc->ProcessAGC(tmp1CPX, tmp2CPX, size);
	demod->ProcessBlock(tmp2CPX, out, size);
	
	//memcpy(out.data(), in.data(), size * sizeof(cpx));
	//out = in;

	for (int i = 0; i < size; i++) {

		out[i] = ScaleCPX(out.at(i), m_volume);
	}
	m_mutex.unlock();
}

int QDSPEngine::getSpectrum(float *buffer) {

	return spectrum->psdBmResults(buffer);
}

float QDSPEngine::getSMeterInstValue() {

	return signalmeter->getInstFValue();
}

void QDSPEngine::setVolume(float value) {

	if (m_volume == value) return;

	m_volume = value;
}

void QDSPEngine::setQtDSPStatus(bool value) { 
	
	m_qtdspOn = value; 
}

void QDSPEngine::setDSPMode(DSPMode mode) {

	demod->setDemodMode(mode);
}

void QDSPEngine::setAGCMode(AGCMode mode) {

	//agc->setMode(mode);
	wpagc->setMode(mode);
}

void QDSPEngine::setAGCMaximumGain(qreal value) {

	qreal maxGain = 20.0 * log10(value);
	set->setAGCMaximumGain_dB(this, m_rx, maxGain);
}

void QDSPEngine::setAGCHangThreshold(qreal value) {

	set->setAGCHangThresholdSlider(this, m_rx, value);
}

void QDSPEngine::setAGCLineValues(QObject *sender, int rx, qreal thresh, qreal hang) {

	if (m_rx != rx) return;

	qreal noiseOffset = 10.0 * log10(qAbs(filter->filterHi() - filter->filterLo()) * 2 * m_size / m_samplerate);
	qreal threshold = 20.0 * log10(thresh) - noiseOffset + AGCOFFSET;

	set->setAGCLineLevels(sender, m_rx, threshold, hang + AGCOFFSET);
}

//void QDSPEngine::setAGCThresholdLine(QObject *sender, int rx, qreal value) {
//
//	if (m_rx != rx) return;
//
//	qreal noiseOffset = 10.0 * log10(qAbs(filter->filterHi() - filter->filterLo()) * 2 * m_size / m_samplerate);
//	set->setAGCThresholdLine_dBm(sender, m_rx, 20.0 * log10(value) - noiseOffset + AGCOFFSET);
//
//}

//void QDSPEngine::setAGCHangLeveldBLine(qreal value) {
//
//	set->setAGCHangLevelLine_dBm(this, m_rx, value + AGCOFFSET);
//}

void QDSPEngine::setSampleRate(QObject *sender, int value) {

	Q_UNUSED(sender)

	if (m_samplerate == value) return;

	m_mutex.lock();
	switch (value) {

		case 48000:
			m_samplerate = value;
			break;

		case 96000:
			m_samplerate = value;
			break;

		case 192000:
			m_samplerate = value;
			break;

		default:
			DSP_ENGINE_DEBUG << "invalid sample rate (48000, 96000, 192000)!\n";
			break;
	}
	filter->setSampleRate(this, m_samplerate);
	demod->setSampleRate(this, m_samplerate);
	wpagc->setSampleRate(this, m_samplerate);

	m_mutex.unlock();

}
