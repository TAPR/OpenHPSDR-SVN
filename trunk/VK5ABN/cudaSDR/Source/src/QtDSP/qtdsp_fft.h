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

//#include <QObject>

#include <cmath>
#include <cufftw.h>
#include <cuda_runtime.h>
//#include "qtdsp_qComplex.h"


//class QFFT : public QObject {
class QFFT {

public:
    QFFT(int size = 0);
    ~QFFT();

    void DoFFTWForward(cufftComplex *in, cufftComplex *out, int size);
    void DoFFTWInverse(cufftComplex *in, cufftComplex *out, int size);
    void DoFFTWMagnForward(cufftComplex *in, int size, float baseline, float correction, float* fbr);

    float magCPX(cufftComplex in);
    float sqrtMagCPX(cufftComplex in);
    float pwrMagCPX(cufftComplex in);

private:
    cufftComplex	*cpxbuf1, *cpxbuf2, *cpxbuf3;

    cudaStream_t stream1, stream2, stream3;

    cufftHandle		plan_fwd;
    cufftHandle		plan_rev;
    cufftHandle     plan_mag;

    int		m_size;
    int		half_sz;
};

__global__ void pwrCPX(cufftComplex *in);

#endif	// _QTDSP_FFT_H
