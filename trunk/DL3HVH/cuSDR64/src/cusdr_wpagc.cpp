/**
* @file  cusdr_wpagc.cpp
* @brief Warren Pratt's ingenious AGC class for cuSDR
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

#include "cusdr_wpagc.h"

QWPAGC::QWPAGC(QObject *parent, int size)
	: QObject(parent)
	, m_settings(Settings::instance())
	, m_size(size)
	, m_samplerate(m_settings->getSampleRate())
	//, m_fixed_gain(1000)
	, m_fixed_gain(0.5)
	, m_n_tau(4)
	, m_tau_attack(0.001)
	, m_tau_decay(0.250)
	, m_tau_fast_decay(0.005)
	, m_tau_fast_backaverage(0.250)
	, m_pop_ratio(5)
	, m_out_targ(1.0)
	, m_var_gain(1.0)
	, m_max_gain(100000.0)
	, m_tau_hang_decay(0.100)
	, m_hang_thresh(0.01)
	, m_hangtime(0.250)
	, m_tau_hang_backmult(0.500)
	// Warren NR0V reports that this change needs to be made in order for things to be
    // in the right position on the screen and other factors.
    , m_max_input(500.0)
{
	m_buf = mallocCPX(m_size);
	m_ring = mallocCPX(RINGBUFFERSIZE);

	memset(m_buf, 0, m_size * sizeof(CPX));
	memset(m_abs_ring, 0, RINGBUFFERSIZE * sizeof(CPX));
	memset(m_abs_ring, 0, RINGBUFFERSIZE * sizeof(qreal));
	/*m_rxData = m_settings->getReceiverDataList().at(0);
	m_agcMode = m_rxData.agcMode;*/

	initWcpAGC();
	loadWcpAGC();
}

QWPAGC::~QWPAGC() {

}

void  QWPAGC::ProcessAGC(CPX* in, CPX* out, int size) {

	if (m_agcMode == agcOFF) {
		
		for (int i = 0; i < m_size; i++)
			out[i] = ScaleCPX(in[i], m_fixed_gain);

		return;
	}

	memcpy(m_buf, in, sizeof(CPX) * m_size);

	int i, j, k;
	qreal mult;
	
	for (i = 0; i < m_size; i++) {

		if (++m_out_index >= RINGBUFFERSIZE)
			m_out_index -= RINGBUFFERSIZE;
		if (++m_in_index >= RINGBUFFERSIZE)
			m_in_index -= RINGBUFFERSIZE;
		
		m_out_sample = m_ring[m_out_index];
		m_abs_out_sample = m_abs_ring[m_out_index];
		m_ring[m_in_index] = m_buf[i];
		m_abs_ring[m_in_index] = qMax(qAbs(m_ring[m_in_index].re), qAbs(m_ring[m_in_index].im));
		
		m_fast_backaverage = m_fast_backmult * m_abs_out_sample + m_onemfast_backmult * m_fast_backaverage;
		m_hang_backaverage = m_hang_backmult * m_abs_out_sample + m_onemhang_backmult * m_hang_backaverage;
		
		if ((m_abs_out_sample >= m_ring_max) && (m_abs_out_sample > 0)) {
			
			m_ring_max = 0.0;
			k = m_out_index;
			
			for (j = 0; j < m_attack_buffsize; j++) {
				
				if (++k == RINGBUFFERSIZE)
					k = 0;
				if (m_abs_ring[k] > m_ring_max)
					m_ring_max = m_abs_ring[k];
			}
		}
		
		if (m_abs_ring[m_in_index] > m_ring_max)
			m_ring_max = m_abs_ring[m_in_index];
		
		if (m_hang_counter > 0)
			--m_hang_counter;
		
		switch (m_state) {
		
			case 0:
				
				if (m_ring_max >= m_volts) {

					m_volts += (m_ring_max - m_volts) * m_attack_mult;
				}				
				else {
					if (m_volts > m_pop_ratio * m_fast_backaverage) {
						
						m_state = 1;
						m_volts += (m_ring_max - m_volts) * m_fast_decay_mult;
					}
					else {
						
						if (m_hang_backaverage > m_hang_level) {
							
							m_state = 2;
							m_hang_counter = (int)(m_hangtime * m_samplerate);
							m_decay_type = 1;
						}
						else {
							
							m_state = 3;
							m_volts += (m_ring_max - m_volts) * m_decay_mult;
							m_decay_type = 0;
						}
					}
				}
				break;
			
			case 1:
				
				if (m_ring_max >= m_volts) {
					
					m_state = 0;
					m_volts += (m_ring_max - m_volts) * m_attack_mult;
				}
				else {
					
					if (m_volts > m_save_volts) {

						m_volts += (m_ring_max - m_volts) * m_fast_decay_mult;
					}
					else {
						
						if (m_hang_counter > 0) {

							m_state = 2;
						}
						else {
							
							if (m_decay_type == 0) {
								
								m_state = 3;
								m_volts += (m_ring_max - m_volts) * m_decay_mult;
							}
							else {
								
								m_state = 4;
								m_volts += (m_ring_max - m_volts) * m_hang_decay_mult;
							}
						}
					}
				}
				break;
			
			case 2:
				
				if (m_ring_max >= m_volts) {
					
					m_state = 0;
					m_save_volts = m_volts;
					m_volts += (m_ring_max - m_volts) * m_attack_mult;
				}
				else {
					
					if (m_hang_counter == 0) {
						
						m_state = 4;
						m_volts += (m_ring_max - m_volts) * m_hang_decay_mult;
					}
				}
				break;
			
			case 3:
				
				if (m_ring_max >= m_volts) {
					
					m_state = 0;
					m_save_volts = m_volts;
					m_volts += (m_ring_max - m_volts) * m_attack_mult;
				}
				else {

					m_volts += (m_ring_max - m_volts) * m_decay_mult;
				}
				break;
			
			case 4:
				
				if (m_ring_max >= m_volts) {
					
					m_state = 0;
					m_save_volts = m_volts;
					m_volts += (m_ring_max - m_volts) * m_attack_mult;
				}
				else {
					
					m_volts += (m_ring_max - m_volts) * m_hang_decay_mult;
				}
				
				break;
		} // end switch on state
		
		if (m_volts < m_min_volts)
			m_volts = m_min_volts;
		
		mult = (m_out_target - m_slope_constant * qMin(0.0, log10 (m_inv_max_input * m_volts))) / m_volts;
		
		out[i].re = (float)(m_out_sample.re * mult);
		out[i].im = (float)(m_out_sample.im * mult);
	}
}

void QWPAGC::initWcpAGC() {

	m_out_index = -1;
	m_ring_max = 0.0;
	m_volts = 0.0;
	m_save_volts = 0.0;
	m_fast_backaverage = 0.0;
	m_hang_backaverage = 0.0;
	m_hang_counter = 0;
	m_state = 0;
	
	memset(m_abs_ring, 0, RINGBUFFERSIZE * sizeof(qreal));
	for (int i = 0; i < RINGBUFFERSIZE; i++) {
		
		m_ring[i].re = 0.0f;
		m_ring[i].im = 0.0f;
	}
	
	m_out_sample.re = 0.0f;
	m_out_sample.im = 0.0f;
	m_abs_out_sample = 0.0f;
	m_decay_type = 0;
}

void QWPAGC::loadWcpAGC() {

	qreal tmp;
	m_attack_buffsize = (int)qCeil(m_samplerate * m_n_tau * m_tau_attack);
	
	m_in_index = m_attack_buffsize + m_out_index;
	
	m_attack_mult = 1.0 - qExp(-1.0 / (m_samplerate * m_tau_attack));
	m_decay_mult = 1.0 - qExp(-1.0 / (m_samplerate * m_tau_decay));
	m_fast_decay_mult = 1.0 - qExp(-1.0 / (m_samplerate * m_tau_fast_decay));
	m_fast_backmult = 1.0 - qExp(-1.0 / (m_samplerate * m_tau_fast_backaverage));
	m_onemfast_backmult = 1.0 - m_fast_backmult;
	
	m_out_target = m_out_targ * (1.0 - qExp(-m_n_tau)) * 0.99;
	m_min_volts = m_out_target / (m_var_gain * m_max_gain);
	
	tmp = log10(m_out_target / (m_max_input * m_var_gain * m_max_gain));
	if (tmp == 0.0)
		tmp = 1e-16;
	
	m_slope_constant = (m_out_target * (1.0 - 1.0 / m_var_gain)) / tmp;
	
	m_inv_max_input = 1.0 / m_max_input;
	
	tmp = qPow(10.0, ((m_agcHangEnable ? m_hang_thresh : 1.0) - 1.0) / 0.125);
	m_hang_level = (m_max_input * tmp + (m_out_target / (m_var_gain * m_max_gain)) * (1.0 - tmp)) * SinAverage;
	
	m_hang_backmult = 1.0 - qExp(-1.0 / (m_samplerate * m_tau_hang_backmult));
	m_onemhang_backmult = 1.0 - m_hang_backmult;
	
	m_hang_decay_mult = 1.0 - qExp(-1.0 / (m_samplerate * m_tau_hang_decay));
}

void QWPAGC::setMode(AGCMode mode) {

    mutex.lock();
	if ((m_agcMode == 0) && (mode != 0)) initWcpAGC();

    m_agcMode = mode;
	
    switch (mode) {

        case agcOFF:
            break;

        case agcSLOW:

			m_agcHangEnable = true;
			m_hangtime = 1.0;
			m_tau_decay = 0.500;
            break;

        case agcMED:

			m_agcHangEnable = false;
			m_hangtime = 0.0;
			m_tau_decay = 0.250;
            break;

        case agcFAST:

			m_agcHangEnable = false;
			m_hangtime = 0.0;
			m_tau_decay = 0.050;
            break;

        case agcLONG:
            
			m_agcHangEnable = true;
			m_hangtime = 2.0;
			m_tau_decay = 2.0;
            break;

		case agcUser:

			m_agcHangEnable = true;
			m_hangtime = 2.0;
			m_tau_decay = 2.0;
			break;
    }
    mutex.unlock();
}

void QWPAGC::setAGCHangEnable(bool value) {

	m_agcHangEnable = value;
}

// fixed_gain when AGC is OFF (set to 'fixed'), linear
void QWPAGC::setAGCFixedGain(qreal value) {

	m_fixed_gain = value;
}

// fixed_gain when AGC is OFF (set to 'fixed'), in dB
void QWPAGC::setAGCFixedGainDb(qreal value) {
	
	m_fixed_gain = qPow(10.0, value / 20.0);
}

void QWPAGC::setSampleRate(QObject *sender, int value) {

	Q_UNUSED(sender)

	mutex.lock();
	m_samplerate = value;
	initWcpAGC();
	loadWcpAGC();
	mutex.unlock();
}

// attack time constant in SECONDS
void QWPAGC::setTauAttack(qreal value) {
	
	mutex.lock();
	m_tau_attack = value;
	loadWcpAGC();
	mutex.unlock();
}

//decay time constant in SECONDS
void QWPAGC::setTauDecay(qreal value) {
	
	mutex.lock();
	m_tau_decay = value;
	loadWcpAGC();
	mutex.unlock();
}

// maximum AGC gain = gain below the "knee", linear
bool QWPAGC::setMaximumGain(qreal value) {
	
	mutex.lock();
	m_max_gain = value;
	
	//if (m_max_gain == Qt::n NaN || Double.IsInfinity(max_gain) || (max_gain <= 0)) {
	if (m_max_gain <= 0) {
		
		// illegal values!
		loadWcpAGC();
		mutex.unlock();
		return false;
	}
	mutex.unlock();
	return true;
}

// maximum AGC gain = gain below the "knee", in dB
bool QWPAGC::setMaximumGainDb(qreal value) {
	
	mutex.lock();
	m_max_gain = qPow(10.0, value / 20.0);
	//if (Double.IsNaN(max_gain) || Double.IsInfinity(max_gain) || (max_gain <= 0))
	if (m_max_gain <= 0) {
		
		// illegal values!
		loadWcpAGC();
		mutex.unlock();
		return false;
	}
	mutex.unlock();
	return true;
}

// variable AGC gain = "Slope", linear
void QWPAGC::setVarGain(qreal value) {
	
	mutex.lock();
	m_var_gain = value;
	loadWcpAGC();
	mutex.unlock();
}

// variable AGC gain = "Slope", in dB
void QWPAGC::setVarGainDb(qreal value) {
	
	mutex.lock();
	m_var_gain = qPow(10.0, value / 20.0);
	loadWcpAGC();
	mutex.unlock();
}

// for hang threshold slider, range 0.0 to 1.0
void QWPAGC::setHangThresh(qreal value) {
	
	mutex.lock();
	m_hang_thresh = value;
	loadWcpAGC();
	mutex.unlock();
}

// hangtime in SECONDS
void QWPAGC::setHangTime(qreal value) {
	
	mutex.lock();
	m_hangtime = value;
	loadWcpAGC();
	mutex.unlock();
}

// for line on bandscope
void QWPAGC::setHangLevelDb(qreal value) {
	
	mutex.lock();
	if (m_max_input > m_min_volts) {
		
		// BUG: found by Warren, 15 Jan 2012:  'SinAverage' term NOT needed!
		// double convert = SinAverage * qPow(10.0, value / 20.0);
		qreal convert = qPow(10.0, value / 20.0);
		qreal tmp = qMax(1.0e-8, (convert - m_min_volts) / (m_max_input - m_min_volts));
		m_hang_thresh = 1.0 + 0.125 * log10(tmp);
	}
    else {
		
		m_hang_thresh = 1.0;
	}

    loadWcpAGC();
	mutex.unlock();
}

// for line on bandscope
qreal QWPAGC::getAGCThreshDb(qreal filt_high, qreal filt_low, int spec_size) {
	
	qreal noise_offset = 10.0 * log10(qAbs(filt_high - filt_low) * spec_size / m_samplerate);
	return 20.0 * log10(m_min_volts) - noise_offset;
}

// for line on bandscope
void QWPAGC::setAGCThreshDb(qreal filt_high, qreal filt_low, int spec_size, qreal thresh) {
	
	mutex.lock();
	qreal noise_offset = 10.0 * log10(qAbs(filt_high - filt_low) * spec_size / m_samplerate);
	m_max_gain = m_out_target / (m_var_gain * qPow(10.0, (thresh + noise_offset) / 20.0));
	
	loadWcpAGC();
	mutex.unlock();
}