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
 *   The ProcessFrequencyShift method is adpated from cuteSDR by (C) Moe Wheatley, AE4JY.
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
	, m_fftMultiplcator(1)
	, m_volume(0.0f)
{
	qRegisterMetaType<QVector<cpx> >();
	qRegisterMetaType<CPX>();

	fft    		= new QFFT(m_size);
	filter 		= new QFilter(this, m_size, 2, 12);//8);
	wpagc  		= new QWPAGC(this, m_size);
	spectrum 	= new PowerSpectrum(this, m_size*2);

	QString str = "Initializing DSP engine for rx %1: 8k FFT ...please wait";
	set->setSystemMessage(str.arg(m_rx), 0);
		
	if (m_rx == 0) {
		
		spectrum2 	= new PowerSpectrum(this, m_size*4);

		str = "Initializing DSP engine for rx %1: 16k FFT ...please wait";
		set->setSystemMessage(str.arg(m_rx), 0);
		spectrum4 	= new PowerSpectrum(this, m_size*8);

		str = "Initializing DSP engine for rx %1: 32k FFT ...please wait";
		set->setSystemMessage(str.arg(m_rx), 0);
		spectrum8 	= new PowerSpectrum(this, m_size*16);
	}

	m_spectrumSize = m_size*4;

	signalmeter = new SignalMeter(this, m_size);
	demod 		= new Demodulation(this, m_size);


	m_rxData = set->getReceiverDataList().at(rx);
	m_agcMode = m_rxData.agcMode;

	wpagc->setReceiver(m_rx);

    InitCPX(tmp1CPX, m_size, 0.0f);
    InitCPX(tmp2CPX, m_size, 0.0f);

	osc1cpx.re = 1.0f;
	osc1cpx.im = 0.0f;

	m_NcoInc = 0.0;
	m_NcoTime = 0.0;
	m_NcoFreq = 0.0;
	m_CWoffset = 0.0;

	//DSP_ENGINE_DEBUG << "set NCO to " << m_rxData.vfoFrequency - m_rxData.ctrFrequency;
	setNCOFrequency(m_rx, m_rxData.vfoFrequency - m_rxData.ctrFrequency);

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

	CHECKED_CONNECT(
		set,
		SIGNAL(ncoFrequencyChanged(int, long)),
		this,
		SLOT(setNCOFrequency(int, long)));

	CHECKED_CONNECT(
		set,
		SIGNAL(sampleSizeChanged(int, int)),
		this,
		SLOT(setSampleSize(int, int)));

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

	switch (m_fftMultiplcator) {

		case 1:
			spectrum->ProcessSpectrum(in, size*2, 1);
			break;

		case 2:
			spectrum2->ProcessSpectrum(in, size*4, 3);
			break;

		case 4:
			spectrum4->ProcessSpectrum(in, size*8, 7);
			break;

		case 8:
			spectrum8->ProcessSpectrum(in, size*16, 15);
			break;
	}

	if (m_NcoFreq != 0)
		ProcessFrequencyShift(in, in, size);

	filter->ProcessFilter(in, tmp1CPX, size);
	signalmeter->ProcessBlock(tmp1CPX, size);
	wpagc->ProcessAGC(tmp1CPX, tmp2CPX, size);
	demod->ProcessBlock(tmp2CPX, out, size);
	
	//memcpy(out.data(), in.data(), size * sizeof(cpx));
	//out = in;

	for (int i = 0; i < size; i++) {

		out[i] = ScaleCPX(out.at(i), m_volume);
	}
	m_mutex.unlock();
}

int	QDSPEngine::getSpectrum(qVectorFloat &buffer, int mult) {

	if (m_rx == 0) {
		
		m_fftMultiplcator = mult;
		switch (m_fftMultiplcator) {
			
			case 1:
				return spectrum->spectrumResult(buffer, 0);
				break;

			case 2:
				return spectrum2->spectrumResult(buffer, 2048);
				break;

			case 4:
				return spectrum4->spectrumResult(buffer, 6144);
				break;

			case 8:
				return spectrum8->spectrumResult(buffer, 14336);
				break;

			default:
				return spectrum->spectrumResult(buffer, 0);
				break;
		}
	}
	else {

		return spectrum->spectrumResult(buffer, 0);
	}
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

		case 384000:
			m_samplerate = value;
			break;

		default:
			DSP_ENGINE_DEBUG << "invalid sample rate (possible values are: 48, 96, 192, or 384 kHz)!\n";
			break;
	}

	//DSP_ENGINE_DEBUG << "set sample rate to " << m_samplerate;
	//setNCOFrequency(m_rx, m_rxData.vfoFrequency - m_rxData.ctrFrequency);
	m_NcoInc = TWOPI * m_NcoFreq/m_samplerate;
	m_OscCos = qCos(m_NcoInc);
	m_OscSin = qSin(m_NcoInc);

	filter->setSampleRate(this, m_samplerate);
	demod->setSampleRate(this, m_samplerate);
	wpagc->setSampleRate(this, m_samplerate);

	m_mutex.unlock();

}

void QDSPEngine::setNCOFrequency(int rx, long ncoFreq) {

	if (m_rx != rx) return;

	qreal tmp = ncoFreq + m_CWoffset;

	m_NcoFreq = tmp;
	m_NcoInc = TWOPI * m_NcoFreq/m_samplerate;
	m_OscCos = qCos(m_NcoInc);
	m_OscSin = qSin(m_NcoInc);
	
	//DSP_ENGINE_DEBUG << "NCO: " << m_NcoFreq;
}

void QDSPEngine::setSampleSize(int rx, int size) {

	Q_UNUSED(rx)

	if (m_rx == 0) {

		m_mutex.lock();
		m_spectrumSize = size;
		m_mutex.unlock();
	}
}

void QDSPEngine::ProcessFrequencyShift(CPX &in, CPX &out, int size) {

	cpx tmp;
	CPX Osc;

	Osc.resize(size);

	for (int i = 0; i < size; i++) {

		tmp = in.at(i);

		qreal OscGn;
		Osc[i].re = osc1cpx.re * m_OscCos - osc1cpx.im * m_OscSin;
		Osc[i].im = osc1cpx.im * m_OscCos + osc1cpx.re * m_OscSin;

		OscGn = 1.95 - (osc1cpx.re * osc1cpx.re + osc1cpx.im * osc1cpx.im);

		osc1cpx.re = OscGn * Osc.at(i).re;
		osc1cpx.im = OscGn * Osc.at(i).im;

		//Cpx multiply by shift frequency
		out[i].re = ((tmp.re * Osc.at(i).re) - (tmp.im * Osc.at(i).im));
		out[i].im = ((tmp.re * Osc.at(i).im) + (tmp.im * Osc.at(i).re));
	}
}

