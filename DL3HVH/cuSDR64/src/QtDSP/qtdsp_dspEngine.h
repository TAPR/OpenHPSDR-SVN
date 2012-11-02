/**
* @file  qtdsp_dspEngine.h
* @brief header file for QtDSP
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

#ifndef _QTDSP_DSP_ENGINE_H
#define _QTDSP_DSP_ENGINE_H

#define AGCOFFSET -18.0//-63.0

//#include <QObject>
//#include <QThread>
//#include <QMetaType>
//#include <QMutexLocker>
//#include <QMutex>
//#include <QWaitCondition>
//#include <QVariant>
//#include <QElapsedTimer>

#include "../cusdr_settings.h"
#include "qtdsp_qComplex.h"
#include "qtdsp_filter.h"
#include "qtdsp_fft.h"
#include "qtdsp_agc.h"
#include "qtdsp_wpagc.h"
#include "qtdsp_powerSpectrum.h"
#include "qtdsp_signalMeter.h"
#include "qtdsp_demodulation.h"


#ifdef LOG_DSP_ENGINE
#   define DSP_ENGINE_DEBUG qDebug().nospace() << "DSPEngine::\t"
#else
#   define DSP_ENGINE_DEBUG nullDebug()
#endif


class QDSPEngine : public QObject {

	Q_OBJECT

public:
	QDSPEngine(QObject *parent = 0, int rx = 0, int size = 0);
	~QDSPEngine();

	QFFT				*fft;
	QFilter				*filter;
	//QAGC				*agc;
	QWPAGC				*wpagc;
	PowerSpectrum		*spectrum;
	SignalMeter			*signalmeter;
	Demodulation		*demod;

	void processDSP(CPX &in, CPX &out,  int size);

	int 	getSpectrum(float *buffer);
	float	getSMeterInstValue();

public slots:
	bool getQtDSPStatus() { return m_qtdspOn; }
	
	void setSampleRate(QObject *sender, int value);
	void setQtDSPStatus(bool value);
	void setVolume(float value);
	void setDSPMode(DSPMode mode);
	void setAGCMode(AGCMode mode);

private:
	Settings	*set;
	TReceiver	m_rxData;
	AGCMode		m_agcMode;

	CPX		tmp1CPX;
	CPX		tmp2CPX;

	QMutex	m_mutex;

	bool	m_qtdspOn;

	int		m_rx;
	int		m_size;
	int		m_samplerate;

	float	m_volume;
	//qreal	m_calOffset;

	void	setupConnections();

private slots:
	void	setAGCMaximumGain(qreal);
	void	setAGCHangThreshold(qreal);
	//void	setAGCHangLeveldBLine(qreal value);
	//void	setAGCThresholdLine(QObject *sender, int rx, qreal value);
	void	setAGCLineValues(QObject *sender, int rx, qreal thresh, qreal hang);
};

#endif // _QTDSP_DSP_ENGINE_H
