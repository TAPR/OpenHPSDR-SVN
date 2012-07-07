/**
* @file  cusdr_wpagc.h
* @brief Warren Pratt's ingenious AGC header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-05-14
*/

/*   
 *   Copyright (C) 2011, 2012 Warren Pratt, NR0V
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
 *
 *   The author can be reached by email at  
 *
 *	 warren@wpratt.com
 */

#ifndef _CUSDR_WPAGC_H
#define	_CUSDR_WPAGC_H

#include "cusdr_complex.h"
#include "cusdr_settings.h"

#include <QObject>
#include <QMutex>

#define RINGBUFFERSIZE 19200
#define SINAVERAGE 0.637f

class QWPAGC : public QObject {

	Q_OBJECT 

public:
    QWPAGC(QObject *parent = 0, int size = 0);
	~QWPAGC();

	void ProcessAGC(CPX* in, CPX* out, int size);

public slots:
	void setSampleRate(QObject *sender, int value);
	void setMode(AGCMode mode);
	void setAGCHangEnable(bool value);
	void setAGCFixedGain(qreal value);
	void setAGCFixedGainDb(qreal value);
	void setTauAttack(qreal value);
	void setTauDecay(qreal value);
	bool setMaximumGain(qreal value);
	bool setMaximumGainDb(qreal value);
	void setVarGain(qreal value);
	void setVarGainDb(qreal value);
	void setHangThresh(qreal value);
	void setHangTime(qreal value);
	void setHangLevelDb(qreal value);
	void setAGCThreshDb(qreal filt_high, qreal filt_low, int spec_size, qreal thresh);

	qreal getAGCThreshDb(qreal filt_high, qreal filt_low, int spec_size);

private:
	Settings*	m_settings;

	QMutex			mutex;
	//QVector<qreal>	m_abs_ring;

	AGCMode		m_agcMode;

	CPX*	m_ring;
	CPX*	m_buf;
	CPX		m_out_sample;

	bool	m_agcHangEnable;

	int		m_size;
	int		m_samplerate;
	int		m_n_tau;
	int		m_out_index;
	int		m_in_index;
	int		m_attack_buffsize;
	int		m_state;
	int		m_hang_counter;
	int		m_decay_type;

	qreal	m_fixed_gain;
	qreal	m_tau_attack;
	qreal	m_tau_decay;
	
	qreal	m_max_gain;
	qreal	m_var_gain;
	qreal	m_min_volts;
	qreal	m_max_input;
	qreal	m_out_targ;
	qreal	m_out_target;
	qreal	m_inv_max_input;
	qreal	m_slope_constant;
	qreal	m_ring_max;

	qreal	m_abs_ring[RINGBUFFERSIZE];
	
	qreal	m_attack_mult;
	qreal	m_decay_mult;
	qreal	m_volts;
	qreal	m_save_volts;
	qreal	m_abs_out_sample;
	qreal	m_tau_fast_backaverage;
	qreal	m_fast_backmult;
	qreal	m_onemfast_backmult;
	qreal	m_fast_backaverage;
	qreal	m_tau_fast_decay;
	qreal	m_fast_decay_mult;
	qreal	m_pop_ratio;
	
	qreal	m_hang_backaverage;
	qreal	m_tau_hang_backmult;
	qreal	m_hang_backmult;
	qreal	m_onemhang_backmult;
	
	
	
	qreal	m_hangtime;
	qreal	m_hang_thresh;
	qreal	m_hang_level;
	
	qreal	m_tau_hang_decay;
	qreal	m_hang_decay_mult;
	
	// average of the absolute value of a sin wave of magnitude 1.0
	qreal	SinAverage;// = 0.637;
	
	//private DSPBuffer d = null;
	//private DSPState s = null;
	void	initWcpAGC();
	void	loadWcpAGC();

	bool	getAGCHangEnable()	{ return m_agcHangEnable; }
};

#endif // _CUSDR_WPAGC_H