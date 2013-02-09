/**
* @file qtdsp_dualModeAverager.cpp
* @brief Dual mode averager class for QtDSP;
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

#include "qtdsp_dualModeAverager.h"

DualModeAverager::DualModeAverager(int rx, int size)
	: QObject()
	, set(Settings::instance())
	, m_receiver(rx)
	, m_size(size)
	, m_length(set->getSpectrumAveragingCnt(m_receiver))
{
	m_tmp.resize(m_size);
	m_tmp.fill(-20.0);
	
	cnt = 0;
	k = 1.0f/m_length;

	CHECKED_CONNECT(
		set, 
		SIGNAL(spectrumAveragingCntChanged(QObject*, int, int)), 
		this, 
		SLOT(setAveragingLength(QObject*, int, int)));

	/*CHECKED_CONNECT(
		set, 
		SIGNAL(widebandAveragingCntChanged(QObject*, int)), 
		this, 
		SLOT(setWidebandAveragingLength(QObject*, int)));*/
}

DualModeAverager::~DualModeAverager() {

	m_tmp.clear();
}

void DualModeAverager::ProcessDBAverager(qVectorFloat &in, qVectorFloat &out) {

	mutex.lock();
	if (cnt < m_length) {

		for (int i = 0; i < m_size; i++)
			out[i] = m_tmp.at(i) + k * in.at(i);

		cnt++;
	}
	else {
		
		for (int i = 0; i < m_size; i++)
			out[i] = m_tmp.at(i) + k * (in.at(i) - m_tmp.at(i));
	}
	mutex.unlock();

	m_tmp = out;
}

void DualModeAverager::setAveragingLength(QObject* sender, int rx, int value) {

	Q_UNUSED (sender)

	if (m_receiver != rx) return;
	
	mutex.lock();
	m_length = value;
	k = 1.0f/m_length;
	cnt = 0;

	m_tmp.fill(0.0f);
	mutex.unlock();
}

//void DualModeAverager::setWidebandAveragingLength(QObject* sender, int value) {
//
//	Q_UNUSED (sender)
//	
//	mutex.lock();
//	m_length = value;
//	k = 1.0f/m_length;
//	cnt = 0;
//
//	m_tmp.fill(0.0f);
//	mutex.unlock();
//}

void DualModeAverager::clearBuffer() {

	m_tmp.clear();
	m_tmp.resize(m_size);
	m_tmp.fill(-20.0);

}
