/**
* @file  qtdsp_signalMeter.h
* @brief Signal meter header file for QtDSP
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

#ifndef _QTDSP_SIGNALMETER_H
#define	_QTDSP_SIGNALMETER_H

#define SPECDBMOFFSET 100.50

#include <cmath>
#include "qtdsp_qComplex.h"
#include "../cusdr_settings.h"

#include <QObject>
#include <QMutex>

class SignalMeter : public QObject {

	Q_OBJECT 

public:
	SignalMeter(QObject *parent = 0, int size = 0);
	~SignalMeter();

	void 	ProcessBlock(CPX &in, int bsize);

	float 	getInstFValue() const;
	float 	getCorrection() const;

public slots:
	void 	setCorrection(const float value);

private:
	Settings	*set;

    int 		m_size;
    
    float 		m_instValue;
    float 		m_correction;
};

#endif // _QTDSP_SIGNALMETER_H
