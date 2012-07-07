/**
* @file  cusdr_agc.h
* @brief AGC header file for cuSDR
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

#ifndef _CUSDR_AGC_H
#define	_CUSDR_AGC_H

#include "cusdr_complex.h"
#include "cusdr_settings.h"

#include <QObject>
#include <QMutex>

const int FASTLEAD = 72;


class QAGC : public QObject {

	Q_OBJECT 

public:
    QAGC(QObject *parent = 0, int size = 0);
	~QAGC();

	void ProcessAGC(CPX* in, CPX* out, int size);

    //AGCMode Mode() const;

    float gain() const;
    float fastGain() const;
    float hangTime() const;
    float gainTop() const;
    float gainBottom() const;
    float attack() const;
    float decay() const;
    float fixedGain() const;
    
    void setGain(float gain);
    void setHangTime(float time);
    void setGainTop(float gain);
    void setGainBottom(float gain);
    void setAttack(float attack);
    void setDecay(float decay);
    void setFixedGain(float gain);

public slots:
	void setMode(AGCMode mode);

private:
	Settings*	m_settings;

	QMutex		mutex;

	//TReceiver	m_rxData;
	AGCMode		m_agcMode;

	CPX*		G;

	int		m_size;

	qint16	m_mask;
    qint16	m_index;
    qint16	m_sndex;
    qint16	m_hangIndex;
    qint16	m_fastIndex;
    qint16	m_fastHang;

	float	m_samplerate;
	float	m_gainTop;
    float	m_gainNow;
    float	m_gainFastNow;
    float	m_gainBottom;
    float	m_gainLimit;
    float	m_gainFix;
    float	m_attack;
    float	m_oneMAttack;
    float	m_decay;
    float	m_oneMDecay;
    float	m_slope;
    float	m_fastAttack;
    float	m_oneMFastAttack;
    float	m_fastDecay;
    float	m_oneMFastDecay;
    float	m_hangTime;
    float	m_hangThresh;
    float	m_fastHangTime;

	float	Scale(float in_val, float scalevalue);
};

#endif // _CUSDR_AGC_H