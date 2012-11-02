/**
* @file  qtdsp_demodulation.cpp
* @brief Demodulation class for QtDSP
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

#include "qtdsp_demodulation.h"


Demodulation::Demodulation(QObject *parent, int size)
	: QObject(parent)
	, set(Settings::instance())
	, m_mode((DSPMode) LSB)
	, m_size(size)
	, m_samplerate((float)set->getSampleRate())
	, m_phase(0.0f)
	, m_delay_real(0.0f)
	, m_delay_imag(1.0f)
	, m_alpha(0.3f * 500.0f * TWOPI / m_samplerate)
	, m_beta(m_alpha * m_alpha * 0.25f)
	, m_lockcurrent(0.5f)
	, m_lockprevious(1.0f)
	, m_dc(0.0f)
	, m_afc(0.0f)
	, m_cvt(0.45f * m_samplerate / (ONEPI * 500.0))
	, m_smooth(0.0f)
	, m_twopi_over_sr(TWOPI/m_samplerate)
	, m_cvt_sr_mult((0.45f * m_samplerate) / ONEPI)
	, m_pll_lo_limit(-1000.0f)
	, m_pll_hi_limit(1000.0f)
	, m_pll_bandwidth(500.0f)
	, m_pll_frequency(0.0f)
{   
    setDemodMode((DSPMode) LSB);

    delay0.re = 0.0f;
    delay0.im = 0.0f;
    tmp0.re = 0.0f;
    tmp0.im = 0.0f;
}

Demodulation::~Demodulation() {

}

void Demodulation::ProcessBlock(CPX &in, CPX &out, int bsize) {

    Q_UNUSED(bsize)

    switch(m_mode) {

        case (DSPMode) AM:

            DoMagnitude(in, out);
            break;

        case (DSPMode) SAM:

            DoSAM(in, out);
            break;

        case (DSPMode)FMN:

            DoFMN(in, out);
            break;

        //case (DSPMode) FMW:

            //DoFMN(in, out);
            //break;

        default:

            memcpy(out.data(), in.data(), sizeof(cpx) * m_size);
            break;
    }
}

inline void Demodulation::DoMagnitude(CPX &in, CPX &out) {

    for (int i = 0; i < m_size; i++) {

		float magn = SqrMagCPX(in.at(i));

		out[i].re = magn;
		out[i].im = magn;
	}
}

void Demodulation::DoSAM(CPX &in, CPX &out ) {

    float difference;

    for (int i = 0; i < m_size; i++) {

        tmp0.re = qCos(m_phase);
        tmp0.im = qSin(m_phase);

        delay0.re = tmp0.re * in.at(i).re + tmp0.im * in.at(i).im;
        delay0.im = -tmp0.im * in.at(i).re + tmp0.re * in.at(i).im;

        if ((delay0.im) == 0.0 && (delay0.re == 0.0)) {

             delay0.re = 0.000000000001;
        }

        difference = sqrt(in.at(i).re * in.at(i).re + in.at(i).im * in.at(i).im) * (qAtan2(delay0.im, delay0.re));

        m_pll_frequency += m_beta * difference;

        if (m_pll_frequency < m_pll_lo_limit)
        	m_pll_frequency = m_pll_lo_limit;

        if (m_pll_frequency > m_pll_hi_limit)
        	m_pll_frequency = m_pll_hi_limit;

        m_phase += m_pll_frequency + m_alpha * difference;

        while (m_phase >= TWOPI)
        	m_phase -= TWOPI;

        while (m_phase < 0)
            m_phase += TWOPI;

        m_lockcurrent = 0.999 * m_lockcurrent + 0.001 * qAbs(delay0.re);
        m_lockprevious = m_lockcurrent;
        m_dc = (0.999 * m_dc) + (0.001 * delay0.re);

        out[i].re = delay0.re - m_dc;
        out[i].im = out.at(i).re;
    }
}

void Demodulation::DoFMN(CPX &in, CPX &out ) {

    float difference;

    for (int i = 0; i < m_size; i++) {

        tmp0.re = qCos(m_phase);
        tmp0.im = qSin(m_phase);

        delay0.re = tmp0.re * in.at(i).re + tmp0.im * in.at(i).im;
        delay0.im = -tmp0.im * in.at(i).re + tmp0.re * in.at(i).im;

        if ((delay0.im) == 0.0 && (delay0.re == 0.0)) {

             delay0.re = 0.000000000001;
        }
        difference = qAtan2(delay0.im, delay0.re);

        m_pll_frequency += m_beta * difference;

        if (m_pll_frequency < m_pll_lo_limit)
            m_pll_frequency = m_pll_lo_limit;
        if (m_pll_frequency > m_pll_hi_limit)
            m_pll_frequency = m_pll_hi_limit;

        m_phase += m_pll_frequency + m_alpha * difference;

        while (m_phase >= TWOPI)
            m_phase -= TWOPI;
        while (m_phase < 0)
            m_phase += TWOPI;

        m_afc = 0.99 * m_afc + 0.01 * m_pll_frequency;
        out[i].re = (m_pll_frequency - m_afc) * m_cvt;
        out[i].im = out.at(i).re;
    }
}

void Demodulation::DoFMW(CPX &in, CPX &out ) {

    memcpy(out.data(), in.data(), sizeof(CPX) * m_size);
}

void Demodulation::setDemodMode(DSPMode mode) {

    m_mode = mode;
    switch(m_mode) {

        case AM: // AM
            break;

        case SAM: // SAM

            m_pll_bandwidth = 500.0;
            m_alpha = 0.3 * m_pll_bandwidth * m_twopi_over_sr;
            m_beta = m_alpha * m_alpha * 0.25;
            m_cvt = m_cvt_sr_mult / m_pll_bandwidth;
            break;

        case FMN: // FMN

            m_pll_bandwidth = 10000.0;
            m_alpha = 0.3 * m_pll_bandwidth * m_twopi_over_sr;
            m_beta = m_alpha * m_alpha * 0.25;
            m_cvt = m_cvt_sr_mult / m_pll_bandwidth;
            break;

//        case dmFMW: // FMW
//            m_pll_bandwidth = 90000.0;
//            m_alpha = 0.3 * m_pll_bandwidth * m_twopi_over_sr;
//            m_beta = m_alpha * m_alpha * 0.25;
//            m_cvt = m_cvt_sr_mult / m_pll_bandwidth;
//            break;

        case CWL:
            break;

        case CWU:
             break;

        case DIGL:
            break;

        case DIGU:
             break;

        case DSB:
            break;

        case LSB:
            break;

        case USB:
            break;

        default:
			//std::cout << "Unknown mode:" << m_mode << std::endl;
            break;
    }
}

DSPMode Demodulation::demodMode() const {

    return m_mode;
}

void Demodulation::setSampleRate(QObject *sender, int value) {

	Q_UNUSED(sender)

	m_samplerate = value;

	m_alpha = 0.3f * 500.0f * TWOPI / m_samplerate;
	m_beta = m_alpha * m_alpha * 0.25f;

	m_cvt = 0.45f * m_samplerate / (ONEPI * 500.0f);
	m_twopi_over_sr = TWOPI / m_samplerate;
	m_cvt_sr_mult = (0.45f * m_samplerate) / ONEPI;
}
