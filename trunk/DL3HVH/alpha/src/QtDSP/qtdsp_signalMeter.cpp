/**
* @file  qtdsp_signalMeter.cpp
* @brief Signal Meter class for QtDSP
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-09-19
*/

/*   
 *   Copyright (C) 2007, 2008, Philip A Covington, N8VB
 *
 *	 adapted for QtDSP by (C) 2012 Hermann von Hasseln, DL3HVH
 *
 *   With Algorithms borrowed from DttSP
 *   Copyright (C) 2004, 2005, 2006 by Frank Brickle, AB2KT and Bob McGwier, N4HY
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

#include "qtdsp_signalMeter.h"

SignalMeter::SignalMeter(QObject *parent, int size)
	: QObject(parent)
	, set(Settings::instance())
	, m_size(size)
	, m_instValue(-80.0)
	//, m_correction(59.0f)
	, m_correction(-8.0f)
{
}

SignalMeter::~SignalMeter() {
}

void SignalMeter::ProcessBlock(CPX &in, int bsize) {

    float tmp = 0.0f;

    for (int i = 0; i < m_size; i++)
    	tmp += (in.at(i).re * in.at(i).re + in.at(i).im * in.at(i).im);

    m_instValue = (float)(10.0f * log10(tmp + 1.5E-45));
}

float SignalMeter::getInstFValue() const {

	return m_instValue + m_correction;
}

float SignalMeter::getCorrection() const {

	return m_correction;
}

void SignalMeter::setCorrection(const float value) {

	if (m_correction == value) return;
	
	m_correction = value;
}
