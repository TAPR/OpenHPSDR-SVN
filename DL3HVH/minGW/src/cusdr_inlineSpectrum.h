/**
* @file  cusdr_inlineSpectrum.h
* @brief Inline Power Spectrum header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-05-14
*/

/*   
 *   Copyright (C) 2007, 2008, 2009, 2010 Philip A Covington, N8VB
 *
 *	 adapted for cuSDR by (C) 2011 Hermann von Hasseln, DL3HVH
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

#ifndef _CUSDR_INLINESPECTRUM_H
#define	_CUSDR_INLINESPECTRUM_H

#include "cusdr_complex.h"
#include "cusdr_fft.h"
#include "cusdr_filter.h"
#include "cusdr_settings.h"

#include <QObject>
#include <QMutex>

class QInlineSpectrum : public QObject {

	Q_OBJECT 

public:
    QInlineSpectrum(QObject *parent = 0, int size = 0);
	~QInlineSpectrum();

	void	ProcessSpectrum(CPX* in, int size);
	
	int		psdBmResults(float * buffer);
	
	float	grabPsPoint(int index);

    void	setBaseLine(float value);
    void	setCorrection(float value);
    void	setPsOn(int value);
	void	setAverages(int value);

	int		dBmSize() const { return m_size * 2; }
	int		psIsOn() const  { return m_psswitch;  }
	int		averages() const { return m_averages; }
	float	baseLine() const { return m_baseline; }
	float	correction() const { return m_correction; }

private:
	Settings*	m_settings;

	QMutex	mutex;

	CPX*	m_windowCPX;
    CPX*	m_tmpCPX;

	QFFT*	m_fft;

	int		m_size;
	int		m_psswitch;
	int		m_averages;

	float	m_samplerate;
	float	m_baseline;
    float	m_correction;
    
    float*	m_window;
	float*	m_fPsdBm;
	float*	m_fAvePsdBm;
};

#endif // _CUSDR_INLINESPECTRUM_H