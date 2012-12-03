/**
* @file  qtdsp_fft.h
* @brief QFFT header FFTW for QtDSP
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-02-18
*/

/*   
 *   Copyright (C) 2007, 2008, 2009, 2010 Philip A Covington, N8VB
 *
 *	 adapted for QtDSP by (C) 2011 Hermann von Hasseln, DL3HVH
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

#ifndef _QTDSP_FFT_H
#define	_QTDSP_FFT_H

#include <QObject>

#include <cmath>
#include "fftw3.h"
#include "qtdsp_qComplex.h"


class QFFT : public QObject {

	Q_OBJECT

public:
    QFFT(QObject *parent = 0, int size = 0);
    ~QFFT();

public slots:
    void DoFFTWForward(CPX &in, CPX &out, int size);
	void DoFFTWInverse(CPX &in, CPX &out, int size);
    void DoFFTWMagnForward(CPX &in, int size, float baseline, float correction, float* fbr);

private:    
    fftwf_complex	*cpxbuf;

    fftwf_plan		plan_fwd;
    fftwf_plan		plan_rev;

    CPX		buf;

    int		m_size;
    int		half_sz;
};

#endif	// _QTDSP_FFT_H
