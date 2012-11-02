/**
* @file  qtdsp_fft.cpp
* @brief QFFT FFTW class for QtDSP
* @author by Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-02-18
*/

/*   
 *   Copyright (C) 2007, 2008 Philip A Covington, N8VB
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

#include "qtdsp_fft.h"
#include <string.h>

#include <QDebug>

QFFT::QFFT(QObject *parent, int size)
	: QObject(parent)
	, m_size(size)
	, half_sz(size/2)
{
    cpxbuf = (fftwf_complex *) fftwf_malloc(sizeof(fftwf_complex) * m_size);
    plan_fwd = fftwf_plan_dft_1d(m_size , cpxbuf, cpxbuf, FFTW_FORWARD, FFTW_MEASURE);
    plan_rev = fftwf_plan_dft_1d(m_size , cpxbuf, cpxbuf, FFTW_BACKWARD, FFTW_MEASURE);

    memset(cpxbuf, 0, m_size * sizeof(cpxbuf));

    InitCPX(buf, m_size, 0.0f);
}

QFFT::~QFFT() {
	
	fftwf_destroy_plan(plan_fwd);
	fftwf_destroy_plan(plan_rev);
	
	if (cpxbuf) 
		fftwf_free(cpxbuf);

	buf.clear();
}

void QFFT::DoFFTWForward(CPX &in, CPX &out, int size) {

    memcpy(cpxbuf, in.data(), sizeof(cpx) * size);
    fftwf_execute(plan_fwd);
    memcpy(out.data(), cpxbuf, sizeof(cpx) * size);
}


void QFFT::DoFFTWInverse(CPX &in, CPX &out, int size)  {

    memcpy(cpxbuf, in.data(), sizeof(cpx) * size);
    fftwf_execute(plan_rev);
    memcpy(out.data(), cpxbuf, sizeof(cpx) * size);
}


void QFFT::DoFFTWMagnForward(CPX &in, int size, float baseline, float correction, float *fbr) {

    memcpy(cpxbuf, in.data(), sizeof(cpx) * size);

    fftwf_execute(plan_fwd);

    for (int i = 0, j = size-1; i < size; i++, j--) {

		*(buf.data()+j) = *(cpx *)(cpxbuf+i);
    }

    for (int i = 0, j = half_sz; i < half_sz; i++, j++) {

        *(fbr+i) = 10.0 * log10(MagCPX(*(buf.data()+j)) + baseline) + correction;
        *(fbr+j) = 10.0 * log10(MagCPX(*(buf.data()+i)) + baseline) + correction;
    }
}
