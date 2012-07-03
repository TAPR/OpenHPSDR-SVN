/**
* @file  cusdr_dspEngine.h
* @brief header file for the QtDSP engine for cuSDR
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

#ifndef _QTDSP_DSP_ENGINE_H
#define _QTDSP_DSP_ENGINE_H


#include <QObject>
#include <QThread>
#include <QMetaType>
#include <QMutexLocker>
#include <QMutex>
#include <QWaitCondition>
#include <QVariant>
#include <QElapsedTimer>

#include "cusdr_settings.h"
#include "cusdr_complex.h"
#include "cusdr_filter.h"
#include "cusdr_fft.h"
#include "cusdr_agc.h"
#include "cusdr_wpagc.h"
#include "cusdr_inlineSpectrum.h"


// *********************************************************************
// data engine class

class QDSPEngine : public QObject {

	Q_OBJECT

public:
	QDSPEngine(QObject *parent = 0, int rx = 0, int size = 0);
	~QDSPEngine();

	QFilter*			filter;
	QFFT*				fft;
	QAGC*				agc;
	QWPAGC*				wpagc;
	QInlineSpectrum*	spectrum;

	void processDSP(CPX* in, CPX* out,  int size);

	int getSpectrum(float* buffer);

public slots:
	bool getQtDSPStatus() { return m_qtdspOn; }
	
	void setQtDSPStatus(bool value);
	void setVolume(float value);
	void setAGCMode(AGCMode mode);

private:
	Settings*	m_settings;
	TReceiver	m_rxData;
	AGCMode		m_agcMode;

	bool	m_qtdspOn;

	int		m_rx;
	int		m_size;

	float	m_volume;
};

#endif // _QTDSP_DSP_ENGINE_H