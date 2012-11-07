/**
* @file  qtdsp_demodulation.h
* @brief Demodulation header file for QtDSP
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-09-20
*/

/*
 *   Copyright (C) 2007, 2008 Philip A Covington, N8VB
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

#ifndef _QTDSP_DEMODULATION_H
#define	_QTDSP_DEMODULATION_H

#include <cmath>
#include "qtdsp_qComplex.h"
#include "../cusdr_settings.h"


class Demodulation : public QObject {

	Q_OBJECT

public:
	Demodulation(QObject *parent = 0, int size = 0);
	~Demodulation();

    void ProcessBlock(CPX &in, CPX &out, int bsize);

    DSPMode demodMode() const;

public slots:
    void 	setDemodMode(DSPMode mode);
    void	setSampleRate(QObject *sender, int value);

private:
    Settings	*set;

    cpx 		tmp0;
    cpx 		delay0;

    DSPMode 	m_mode;
    
    int 		m_size;
    
    float 		m_samplerate;
    float 		m_phase;
    float 		m_delay_real;
    float 		m_delay_imag;
    float 		m_alpha;
    float 		m_beta;
    float 		m_lockcurrent;
    float 		m_lockprevious;
    float 		m_dc;
    float 		m_afc;
    float 		m_cvt;
    float 		m_smooth;
    float 		m_twopi_over_sr;
    float 		m_cvt_sr_mult;
    float 		m_pll_lo_limit;
    float 		m_pll_hi_limit;
    float 		m_pll_bandwidth;
    float 		m_pll_frequency;
    

    void 		DoMagnitude(CPX &in, CPX &out);
    void 		DoSAM(CPX &in, CPX &out);
    void 		DoFMN(CPX &in, CPX &out);
    void 		DoFMW(CPX &in, CPX &out);
};

#endif	// _QTDSP_DEMODULATION_H

