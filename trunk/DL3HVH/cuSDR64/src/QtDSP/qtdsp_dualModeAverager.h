/**
* @file qtdsp_dualModeAverager.h
* @brief Dual mode averager header file for QtDSP
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
 
#ifndef _QTDSP_DUAL_MODE_AVERAGER_H
#define _QTDSP_DUAL_MODE_AVERAGER_H

#include <QObject>

#include "../cusdr_settings.h"

class DualModeAverager : public QObject {

	Q_OBJECT

public:
	DualModeAverager(QObject *parent = 0, int size = 0);
	~DualModeAverager();

	void ProcessDBAverager(qVectorFloat &in, qVectorFloat &out);
	void clearBuffer();
	//void setAveragingLength(int value);

private:
	Settings*		set;
	QMutex			mutex;
	qVectorFloat	m_tmp;

	int		m_size;
	int		m_length;
	int		cnt;

	float	k;

private slots:
	void setAveragingLength(int value);
};

#endif // _QTDSP_DUAL_MODE_AVERAGER_H
