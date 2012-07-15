/**
* @file cusdr_dualModeAverager.cpp
* @brief Dual mode averager class for cuSDR;
		 Dual-Mode Averaging implmemented following "Understanding Digital Signal Processing" by Richard G. Lyons, 3rd ed., p.791
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-07-12
*/

/*
 *   
 *   Copyright 2012 Hermann von Hasseln, DL3HVH
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

#include "cusdr_dualModeAverager.h"

DualModeAverager::DualModeAverager(QObject *parent, int size)
	: QObject(parent)
	, m_settings(Settings::instance())
	, m_size(size)
	, m_length(m_settings->getSpectrumAveragingCnt())
{
	m_tmp.resize(m_size);
	
	cnt = 0;

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(spectrumAveragingCntChanged(int)), 
		this, 
		SLOT(setAveragingLength(int)));
}

DualModeAverager::~DualModeAverager() {
}

void DualModeAverager::ProcessDBAverager(qVectorFloat &in, qVectorFloat &out) {

	mutex.lock();
	if (cnt < m_length) {

		for (int i = 0; i < m_size; i++)
			out[i] = m_tmp.at(i) + (1.0f/m_length) * in.at(i);

		cnt++;
	}
	else {
		
		for (int i = 0; i < m_size; i++)
			out[i] = m_tmp.at(i) + (1.0f/m_length) * (in.at(i) - m_tmp.at(i));
	}
	mutex.unlock();

	m_tmp = out;
}

void DualModeAverager::setAveragingLength(int value) {

	mutex.lock();
	m_length = value;
	cnt = 0;

	m_tmp.fill(0.0f);
	mutex.unlock();
}
