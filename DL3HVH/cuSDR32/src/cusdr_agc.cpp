/**
* @file  cusdr_agc.cpp
* @brief AGC class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-05-14
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

#include "cusdr_agc.h"

QAGC::QAGC(QObject *parent, int size)
	: QObject(parent)
	, m_settings(Settings::instance())
	, m_size(size)
	, m_samplerate(m_settings->getSampleRate())
	//, m_agcMode(agcMED)
	, m_gainTop(pow(10.0, 120.0/20.0))
	, m_gainNow(1.0)
	, m_gainFastNow(1.0)
	, m_gainBottom(.001)
	, m_gainLimit(1.0)
	//, m_gainFix(pow(10.0, 60.0/20.0))
	, m_gainFix(pow(10.0, 80.0/20.0))
	, m_attack(0.0)
	, m_oneMAttack(0.0)
	, m_decay(0.0)
	, m_oneMDecay(0.0)
	, m_slope(1.0)
	, m_fastAttack(0.0)
	, m_oneMFastAttack(0.0)
	, m_fastDecay(0.0)
	, m_oneMFastDecay(0.0)
	, m_hangTime(480.0 * 0.001)
	, m_hangThresh(0.001)
	, m_fastHangTime(48.0 * 0.001)
	, m_mask(size * 2)
	, m_index(0)
	, m_sndex(0)
	, m_hangIndex(0)
	, m_fastIndex(FASTLEAD)
	, m_fastHang(0)
{
	setAttack(1.0);
    setDecay(1.0);
    setMode(agcLONG);
    
    m_fastAttack     = 1.0 - qExp(-1000.0 / (0.2f * m_samplerate));
    m_oneMFastAttack = qExp(-1000.0 / (0.2 * m_samplerate));

    m_fastDecay     = 1.0 - qExp(-1000.0 / (3.0 * m_samplerate));
    m_oneMFastDecay = qExp(-1000.0 / (3.0 * m_samplerate));

    G = mallocCPX(m_mask);    
    m_mask -= 1;

	/*m_rxData = m_settings->getReceiverDataList().at(0);
	m_agcMode = m_rxData.agcMode;*/
}

QAGC::~QAGC() {

}

void  QAGC::ProcessAGC(CPX* in, CPX* out, int size) {
	
	if (m_agcMode == agcOFF) {
		
		for (int i = 0; i < size; i++)
			ScaleCPX(in[i], m_gainFix);

		memcpy(out, in, size * sizeof(CPX));
		return;
	}
	
	unsigned int hangTime     = (unsigned int)(m_samplerate * m_hangTime);
    unsigned int fastHangTime = (unsigned int)(m_samplerate * m_fastHangTime);

    float hangThresh = 0.0;

    if (m_hangThresh > 0.0)
		hangThresh = m_gainTop * m_hangThresh + m_gainBottom * (1.0 - m_hangThresh);

    for (int i = 0; i < m_size; i++) {

        G[m_index] = in[i];

        float tmp = 1.1 * SqrMagCPX(G[m_index]);

        if (tmp == 0.0)
            tmp = m_gainNow;
        else
            tmp = m_gainLimit / tmp;

        if (tmp < hangThresh)
            m_hangIndex = hangTime;

        if (tmp > m_gainNow) {

            if (m_hangIndex++ > hangTime)
				m_gainNow = m_oneMDecay * m_gainNow + m_decay * qMin(m_gainTop, tmp);
        } 
		else {

            m_hangIndex = 0;
			m_gainNow = m_oneMAttack * m_gainNow + m_attack * qMax(tmp, m_gainBottom);
        }

        tmp = 1.2 * SqrMagCPX(G[m_fastIndex]);

        if (tmp != 0.0)
            tmp = m_gainLimit / tmp;
        else
            tmp = m_gainFastNow;

        if (tmp > m_gainFastNow) {

            if (m_fastHang++ > fastHangTime)
				m_gainFastNow = qMin(m_oneMFastDecay * m_gainFastNow + m_fastDecay * qMin(m_gainTop, tmp), m_gainTop);
        } 
		else {

            m_fastHang = 0;
            m_gainFastNow = qMax(m_oneMFastAttack * m_gainFastNow + m_fastAttack * qMax(tmp, m_gainBottom), m_gainBottom);
        }

        m_gainFastNow = qMax(qMin(m_gainFastNow, m_gainTop), m_gainBottom);
        m_gainNow     = qMax(qMin(m_gainNow,     m_gainTop), m_gainBottom);

        out[i].re = Scale(G[m_sndex].re, qMin(m_gainFastNow, qMin(m_slope * m_gainNow, m_gainTop)));
        out[i].im = Scale(G[m_sndex].im, qMin(m_gainFastNow, qMin(m_slope * m_gainNow, m_gainTop)));

        m_index = (m_index + m_mask) & m_mask;
        m_sndex = (m_sndex + m_mask) & m_mask;

        m_fastIndex = (m_fastIndex + m_mask) & m_mask;
    }
}

void QAGC::setMode(AGCMode mode) {

    mutex.lock();
    m_agcMode = mode;
	
    switch (mode) {

        case agcOFF:
            break;

        case agcSLOW:

            m_hangTime = 0.5;
            m_fastHangTime = 0.1F;
            m_decay = 1.0 - qExp(-2.0 / m_samplerate);
            m_oneMDecay = 1.0 - m_decay;
            break;

        case agcMED:

            m_hangTime = 0.25;
            m_fastHangTime = 0.1;
            m_decay = 1.0 - qExp(-4.0 / m_samplerate);
            m_oneMDecay = 1.0 - m_decay;
            break;

        case agcFAST:

            m_hangTime = 0.1;
            m_fastHangTime = 0.1;
            m_decay = 1.0 - qExp(-10.0 / m_samplerate);
            m_oneMDecay = 1.0 - m_decay;
            break;

        case agcLONG:
            m_hangTime = 0.75;
            m_fastHangTime = 0.1;
            m_decay = 1.0 - qExp(-0.5 / m_samplerate);
            m_oneMDecay = 1.0 - m_decay;
            break;
    }
    mutex.unlock();
}

float QAGC::gain() const {

    return 20.0 * log10(m_gainNow);
}

void QAGC::setGain(float gain) {

    mutex.lock();            
    m_gainNow = pow(10.0, gain/20.0);
    mutex.unlock();
}

float QAGC::fastGain() const {

    return 20.0 * log10(m_gainFastNow);
}

float QAGC::hangTime() const {

    return m_hangTime / 0.001;
}
    
void QAGC::setHangTime(float time) {

    mutex.lock();
    m_hangTime = time * 0.001;
    mutex.unlock();
}

float QAGC::gainTop() const {

    return 20.0 * log10(m_gainTop);
}
   
void QAGC::setGainTop(float gain) {

    mutex.lock();
    m_gainTop = pow(10.0, gain/20.0);
    mutex.unlock();
}

float QAGC::gainBottom() const {

    return 20.0 * log10(m_gainBottom);
}

void QAGC::setGainBottom(float gain) {

    mutex.lock();
    m_gainBottom = pow(10.0, gain/20.0);
    mutex.unlock();
}

float QAGC::attack() const { return m_attack; }
    
void QAGC::setAttack(float attack) {

    mutex.lock();
    m_attack = 1.0 - qExp(-1000.0 / (attack * m_samplerate));
    m_oneMAttack = qExp (-1000.0 / (attack * m_samplerate));

    m_sndex = (m_index + (int)(0.003 * m_samplerate * attack)) & m_mask;
    m_fastIndex = (m_sndex + FASTLEAD * m_mask) & m_mask;

    m_fastHangTime = 0.1;   
    mutex.unlock();
}

float QAGC::decay() const {

    return m_decay;
}
    
void QAGC::setDecay(float decay) {

    mutex.lock();
    m_decay = 1.0 - qExp(-1000.0 / (decay * m_samplerate));
    m_oneMDecay = qExp(-1000.0 / (decay * m_samplerate));   
    mutex.unlock();
}

float QAGC::fixedGain() const {

    return 20.0 * log10(m_gainFix);    
}

void QAGC::setFixedGain(float gain) {

    mutex.lock();
    m_gainFix = qPow(10.0, gain/20.0);
    mutex.unlock();
}

inline float QAGC::Scale(float in_val, float scalevalue) {

    return in_val * scalevalue;    
}
