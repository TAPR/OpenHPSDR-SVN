/**
* @file  qtdsp_wpagc.cpp
* @brief Warren Pratt's ingenious AGC class for QtDSP
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-05-14
*/

/*   
 *   Copyright (C) 2011, 2012 Warren Pratt, NR0V
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
 *
 *   The author can be reached by email at  
 *
 *	 warren@wpratt.com
 */
#define LOG_WPAGC

#include "qtdsp_wpagc.h"

QWPAGC::QWPAGC(QObject *parent, int size)
	: QObject(parent)
	, set(Settings::instance())
	, m_size(size)
	, m_samplerate(set->getSampleRate())
	, m_nTau(4)
	, m_outIndex(-1)
	, m_inIndex(0)
	, m_attackBuffersize(0)
	, m_state(0)
	, m_hangCounter(0)
	, m_decayType(0)
	, m_fixedGain(1000)
	, m_tauAttack(0.001)
	, m_tauDecay(0.250)
	//, m_maxGain(100000.0)
	, m_varGain(1.0)
	, m_minVolts(0.0)

	// Warren NR0V reports that this change needs to be made in order for things to be
    // in the right position on the screen and other factors:
	, m_maxInput(500.0)

	, m_out_targ(1.0)
	, m_out_target(0.0)
	, m_inv_max_input(0.0)
	, m_slope_constant(0.0)
	, m_ring_max(0.0)
	, m_attack_mult(0.0)
	, m_decay_mult(0.0)
	, m_volts(0.0)
	, m_save_volts(0.0)
	, m_abs_out_sample(0.0)
	, m_tau_fast_backaverage(0.250)
	, m_fast_backmult(0.0)
	, m_onemfast_backmult(1.0)
	, m_fast_backaverage(0.0)
	, m_tau_fast_decay(0.005)
	, m_fast_decay_mult(0.0)
	, m_pop_ratio(5.0)
	, m_hang_backaverage(0.0)
	, m_tau_hang_backmult(0.500)
	, m_hang_backmult(0.0)
	, m_onemhang_backmult(1.0)
	, m_hangtime(0.250)
	, m_hangThresh(0.01)
	, m_hangLevel(0.0)
	, m_tau_hang_decay(0.100)
	, m_hang_decay_mult(0.0)
	, SinAverage(0.637)
{
	InitCPX(buf, m_size, 0.0f);
	InitCPX(ring, RINGBUFFERSIZE, 0.0f);

	outSample.re = 0.0f;
	outSample.im = 0.0f;

	absRing.resize(RINGBUFFERSIZE);
	absRing.fill(0.0);

	initWcpAGC();
}

QWPAGC::~QWPAGC() {

	buf.clear();
	ring.clear();
}

//void QWPAGC::setupConnections() {
//
//}

void  QWPAGC::ProcessAGC(CPX &in, CPX &out, int size) {

	Q_UNUSED(size)

	if (m_agcMode == agcOFF) {
		
		for (int i = 0; i < m_size; i++)
			out[i] = ScaleCPX(in.at(i), m_fixedGain);

		return;
	}

	memcpy(buf.data(), in.data(), sizeof(cpx) * m_size);

	int i, j, k;
	//qreal mult;
	
	mutex.lock();
	for (i = 0; i < m_size; i++) {

		if (++m_outIndex >= RINGBUFFERSIZE)
			m_outIndex -= RINGBUFFERSIZE;

		if (++m_inIndex >= RINGBUFFERSIZE)
			m_inIndex -= RINGBUFFERSIZE;
		
		outSample.re = ring.at(m_outIndex).re;
		outSample.im = ring.at(m_outIndex).im;

		//m_abs_out_sample = absRing[m_out_index];
		m_abs_out_sample = absRing.at(m_outIndex);

		ring[m_inIndex].re = buf.at(i).re;
		ring[m_inIndex].im = buf.at(i).im;

		absRing[m_inIndex] = qMax(qAbs(ring.at(m_inIndex).re), qAbs(ring.at(m_inIndex).im));
		
		m_fast_backaverage = m_fast_backmult * m_abs_out_sample + m_onemfast_backmult * m_fast_backaverage;
		m_hang_backaverage = m_hang_backmult * m_abs_out_sample + m_onemhang_backmult * m_hang_backaverage;
		
		if ((m_abs_out_sample >= m_ring_max) && (m_abs_out_sample > 0)) {
			
			m_ring_max = 0.0;
			k = m_outIndex;
			
			for (j = 0; j < m_attackBuffersize; j++) {
				
				if (++k == RINGBUFFERSIZE)
					k = 0;
				if (absRing[k] > m_ring_max)
					//m_ring_max = absRing[k];
					m_ring_max = absRing.at(k);
			}
		}
		
		if (absRing[m_inIndex] > m_ring_max)
			//m_ring_max = absRing[m_in_index];
			m_ring_max = absRing.at(m_inIndex);
		
		if (m_hangCounter > 0)
			--m_hangCounter;
		
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
						
						if (m_hang_backaverage > m_hangLevel) {
							
							m_state = 2;
							m_hangCounter = (int)(m_hangtime * m_samplerate);
							m_decayType = 1;
						}
						else {
							
							m_state = 3;
							m_volts += (m_ring_max - m_volts) * m_decay_mult;
							m_decayType = 0;
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
						
						if (m_hangCounter > 0) {

							m_state = 2;
						}
						else {
							
							if (m_decayType == 0) {
								
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
					
					if (m_hangCounter == 0) {
						
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
		
		if (m_volts < m_minVolts)
			m_volts = m_minVolts;
		
		mult = (m_out_target - m_slope_constant * qMin(0.0, log10 (m_inv_max_input * m_volts))) / m_volts;
		
		out[i].re = (float)(outSample.re * mult);
		out[i].im = (float)(outSample.im * mult);
	}
	mutex.unlock();
}

void QWPAGC::initWcpAGC() {

	m_outIndex = -1;
	m_ring_max = 0.0;
	m_volts = 0.0;
	m_save_volts = 0.0;
	m_fast_backaverage = 0.0;
	m_hang_backaverage = 0.0;
	m_hangCounter = 0;
	m_state = 0;
	
	//memset(absRing, 0, RINGBUFFERSIZE * sizeof(qreal));
	absRing.fill(0.0);
	for (int i = 0; i < RINGBUFFERSIZE; i++) {
		
		ring[i].re = 0.0f;
		ring[i].im = 0.0f;
	}
	
	outSample.re = 0.0f;
	outSample.im = 0.0f;
	m_abs_out_sample = 0.0f;
	m_decayType = 0;
}

void QWPAGC::loadWcpAGC() {

	qreal tmp;
	m_attackBuffersize = (int)qCeil(m_samplerate * m_nTau * m_tauAttack);
	
	m_inIndex = m_attackBuffersize + m_outIndex;
	
	m_attack_mult = 1.0 - qExp(-1.0 / (m_samplerate * m_tauAttack));
	m_decay_mult = 1.0 - qExp(-1.0 / (m_samplerate * m_tauDecay));
	m_fast_decay_mult = 1.0 - qExp(-1.0 / (m_samplerate * m_tau_fast_decay));
	m_fast_backmult = 1.0 - qExp(-1.0 / (m_samplerate * m_tau_fast_backaverage));
	m_onemfast_backmult = 1.0 - m_fast_backmult;
	
	m_out_target = m_out_targ * (1.0 - qExp(-m_nTau)) * 0.99;
	m_minVolts = m_out_target / (m_varGain * m_maxGain);

	//WPAGC_DEBUG << "emit m_min_volts = " << m_min_volts;
	//WPAGC_DEBUG << "emit m_max_gain = " << maxGain;
	//emit minimumVoltageChanged(this, m_receiver, m_min_volts);
	//emit agcMaximumGainChanged(m_max_gain);

	tmp = log10(m_out_target / (m_maxInput * m_varGain * m_maxGain));
	if (tmp == 0.0)
		tmp = 1.5E-45;
	
	m_slope_constant = (m_out_target * (1.0 - (1.0 / m_varGain))) / tmp;
	//m_slope_constant = (m_out_target * (1.0 - (1.0 / m_var_gain))) / (tmp + 1.5E-45);
	
	m_inv_max_input = 1.0 / m_maxInput;
	
	tmp = qPow(10.0, ((m_agcHangEnable ? m_hangThresh : 1.0) - 1.0) / 0.125);
	m_hangLevel = (m_maxInput * tmp + (m_out_target / (m_varGain * m_maxGain)) * (1.0 - tmp)) * SinAverage;

	// send the hang level value out for display
	//WPAGC_DEBUG << "hangLevel_dB = " << 20.0 * log10(m_hangLevel / SinAverage);
	//emit hangLeveldBLineChanged(20.0 * log10(m_hangLevel / SinAverage));
	
	m_hang_backmult = 1.0 - qExp(-1.0 / (m_samplerate * m_tau_hang_backmult));
	m_onemhang_backmult = 1.0 - m_hang_backmult;
	
	m_hang_decay_mult = 1.0 - qExp(-1.0 / (m_samplerate * m_tau_hang_decay));

	emit displayValues(this, m_receiver, m_minVolts, 20.0 * log10(m_hangLevel / SinAverage));
}

void QWPAGC::setReceiver(int rx) {

	m_receiver = rx;
}

void QWPAGC::setMode(AGCMode mode) {

    //mutex.lock();
	if ((m_agcMode == (AGCMode) agcOFF) && (mode != 0)) initWcpAGC();

    m_agcMode = mode;
	
    switch (mode) {

        case agcOFF:
            break;

        case agcSLOW:

			m_agcHangEnable = true;
			m_hangtime = 1.0;
			m_tauDecay = 0.500;
            break;

        case agcMED:

			m_agcHangEnable = false;
			m_hangtime = 0.0;
			m_tauDecay = 0.250;
            break;

        case agcFAST:

			m_agcHangEnable = false;
			m_hangtime = 0.0;
			m_tauDecay = 0.050;
            break;

        case agcLONG:
            
			m_agcHangEnable = true;
			m_hangtime = 2.0;
			m_tauDecay = 2.0;
            break;

		case agcUser:

			m_agcHangEnable = true;
			m_hangtime = 2.0;
			m_tauDecay = 2.0;
			break;

		default:

			m_agcHangEnable = true;
			m_hangtime = 2.0;
			m_tauDecay = 2.0;
			break;
    }
    //mutex.unlock();
}

void QWPAGC::setAGCHangEnable(bool value) {

	m_agcHangEnable = value;
}

// fixed_gain when AGC is OFF (set to 'fixed'), linear
void QWPAGC::setAGCFixedGain(qreal value) {

	m_fixedGain = value;
}

qreal QWPAGC::getAGCFixedGain() {

	return m_fixedGain;
}

// fixed_gain when AGC is OFF (set to 'fixed'), in dB
void QWPAGC::setAGCFixedGainDb(qreal value) {
	
	qreal tmp = value;
	if (tmp > 60.0) tmp = 60.0;

	m_fixedGain = qPow(10.0, tmp / 20.0);
	//WPAGC_DEBUG << "m_fixedGain = " << m_fixedGain;
}

qreal QWPAGC::getAGCFixedGainDb() {

	return 20.0 * log10(m_fixedGain);
}

void QWPAGC::setSampleRate(QObject *sender, int value) {

	Q_UNUSED(sender)

	//mutex.lock();
	m_samplerate = value;
	initWcpAGC();
	loadWcpAGC();
	//mutex.unlock();
}

// attack time constant in SECONDS
void QWPAGC::setTauAttack(qreal value) {
	
	mutex.lock();
	m_tauAttack = value;
	loadWcpAGC();
	mutex.unlock();
}

qreal QWPAGC::getTauAttack() {

	return m_tauAttack;
}

//decay time constant in SECONDS
void QWPAGC::setTauDecay(qreal value) {
	
	mutex.lock();
	m_tauDecay = value;
	loadWcpAGC();
	mutex.unlock();
}

qreal QWPAGC::getTauDecay() {

	return m_tauDecay;
}

// maximum AGC gain = gain below the "knee", linear
bool QWPAGC::setMaximumGain(qreal value) {
	
	mutex.lock();
	m_maxGain = value;
	
	if (ValidQReal(m_maxGain) == "NaN" || ValidQReal(m_maxGain) == "+Inf" || (m_maxGain <= 0)) {
		
		mutex.unlock();
		return false; // illegal values!
	}
	else
		loadWcpAGC();

	mutex.unlock();
	return true;
}

qreal QWPAGC::getAGCMaximumGain_dBm() {

	return 20.0 * log10(m_maxGain);
}

// maximum AGC gain = gain below the "knee", in dB
bool QWPAGC::setMaximumGainDb(qreal value) {
	
	mutex.lock();
	m_maxGain = qPow(10.0, value / 20.0);
	//WPAGC_DEBUG << "maxGain from Slider = " << m_maxGain << " (" << value << " dB)";
	if (ValidQReal(m_maxGain) == "NaN" || ValidQReal(m_maxGain) == "+Inf" || (m_maxGain <= 0)) {

		mutex.unlock();
		WPAGC_DEBUG << "illegal value for maximum gain !";
		return false; // illegal values!
	}
	else
		loadWcpAGC();

	mutex.unlock();
	return true;
}

// variable AGC gain = "Slope", linear
void QWPAGC::setVarGain(qreal value) {
	
	mutex.lock();
	m_varGain = value;
	loadWcpAGC();
	mutex.unlock();
}

// variable AGC gain = "Slope", in dB
void QWPAGC::setVarGainDb(qreal value) {
	
	mutex.lock();
	m_varGain = qPow(10.0, value / 20.0);
	loadWcpAGC();
	mutex.unlock();
}

// for hang threshold slider, range 0.0 to 1.0
void QWPAGC::setHangThresh(qreal value) {
	
	mutex.lock();
	//WPAGC_DEBUG << "m_hang_thresh = " << value;
	m_hangThresh = value;
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
	if (m_maxInput > m_minVolts) {
		
		// BUG: found by Warren, 15 Jan 2012:  'SinAverage' term NOT needed!
		// double convert = SinAverage * qPow(10.0, value / 20.0);
		qreal convert = qPow(10.0, value / 20.0);
		qreal tmp = qMax(1.0e-8, (convert - m_minVolts) / (m_maxInput - m_minVolts));
		m_hangThresh = 1.0 + 0.125 * log10(tmp);
	}
    else
    	m_hangThresh = 1.0;

    loadWcpAGC();
	mutex.unlock();

	emit agcHangThresholdChanged(m_hangThresh * 100.0);
}

// for line on bandscope
qreal QWPAGC::getHangLevelDb() {

	return 20.0 * log10(m_hangLevel / SinAverage);
}

// for line on bandscope
//qreal QWPAGC::getAGCThreshDb(qreal filt_high, qreal filt_low, int spec_size) {
//
//	qreal noise_offset = 10.0 * log10(qAbs(filt_high - filt_low) * spec_size / m_samplerate);
//	return 20.0 * log10(m_min_volts) - noise_offset;
//}

// for line on bandscope
void QWPAGC::setAGCThreshDb(qreal filt_high, qreal filt_low, int spec_size, qreal thresh) {
	
	mutex.lock();
	qreal noise_offset = 10.0 * log10(qAbs(filt_high - filt_low) * spec_size / m_samplerate);
	m_maxGain = m_out_target / (m_varGain * qPow(10.0, (thresh + noise_offset) / 20.0));
	
	loadWcpAGC();
	mutex.unlock();

	//WPAGC_DEBUG << "maxGain = " << m_maxGain;
	emit agcMaximumGainChanged(m_maxGain);
}

void QWPAGC::filterChanged() {

	emit displayValues(this, m_receiver, m_minVolts, 20.0 * log10(m_hangLevel / SinAverage));
}
