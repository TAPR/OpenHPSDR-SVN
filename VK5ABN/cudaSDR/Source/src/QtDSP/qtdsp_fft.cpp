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
#include <cuda.h>
#include "qtdsp_fft.h"
#include <string.h>

#include <helper_functions.h>
#include <helper_cuda.h>

#include <QDebug>

QFFT::QFFT(int size)
	: QObject()
	, m_size(size)
	, half_sz(size/2)
{
    checkCudaErrors(cudaMalloc((void **) &cpxbuf, sizeof(cufftComplex) * m_size));

    checkCudaErrors(cudaStreamCreate(&stream1));
    checkCudaErrors(cudaStreamCreate(&stream2));

    checkCudaErrors(cufftPlan1d(&plan_fwd, m_size, CUFFT_C2C, 1));
    checkCudaErrors(cufftPlan1d(&plan_rev, m_size, CUFFT_C2C, 1));

    checkCudaErrors(cufftSetStream(plan_fwd, stream1));
    checkCudaErrors(cufftSetStream(plan_rev, stream2));

    checkCudaErrors(cudaMemset(cpxbuf, 0, m_size * sizeof(cufftComplex)));

    InitCPX(buf, m_size, 0.0f);
}

QFFT::~QFFT() {

    checkCudaErrors(cudaStreamSynchronize(stream1));
    checkCudaErrors(cudaStreamSynchronize(stream2));

    checkCudaErrors(cudaStreamDestroy(stream1));
    checkCudaErrors(cudaStreamDestroy(stream2));

    checkCudaErrors(cufftDestroy(plan_fwd));
    checkCudaErrors(cufftDestroy(plan_rev));

    checkCudaErrors(cudaFree(cpxbuf));

    buf.clear();
}

void QFFT::DoFFTWForward(CPX &in, CPX &out, int size) {

    checkCudaErrors(cudaMemcpyAsync(cpxbuf, in.data(), sizeof(cpx) * size, cudaMemcpyHostToDevice, stream1));
    checkCudaErrors(cufftExecC2C(plan_fwd, cpxbuf, cpxbuf, CUFFT_FORWARD));
    checkCudaErrors(cudaMemcpyAsync(out.data(), cpxbuf, sizeof(cpx) * size, cudaMemcpyDeviceToHost, stream1));
}


void QFFT::DoFFTWInverse(CPX &in, CPX &out, int size)  {

    checkCudaErrors(cudaMemcpyAsync(cpxbuf, in.data(), sizeof(cpx) * size, cudaMemcpyHostToDevice, stream2));
    checkCudaErrors(cufftExecC2C(plan_rev, cpxbuf, cpxbuf, CUFFT_INVERSE));
    checkCudaErrors(cudaMemcpyAsync(out.data(), cpxbuf, sizeof(cpx) * size, cudaMemcpyDeviceToHost, stream2));
}


void QFFT::DoFFTWMagnForward(CPX &in, int size, float baseline, float correction, float *fbr) {

     CPX temp;
     InitCPX(temp, size, 0.0f);

    checkCudaErrors(cudaMemcpyAsync(cpxbuf, in.data(), sizeof(cpx) * size, cudaMemcpyHostToDevice, stream1));
    checkCudaErrors(cufftExecC2C(plan_fwd, cpxbuf, cpxbuf, CUFFT_FORWARD));
    checkCudaErrors(cudaMemcpyAsync(temp.data(), cpxbuf, sizeof(cpx) * size, cudaMemcpyDeviceToHost, stream1));
/*
    for (int i = 0, j = size-1; i < size; i++, j--) {
        *(buf.data()+j) = *(cpx *)(temp.data()+i);
    }

    for (int i = 0, j = half_sz; i < half_sz; i++, j++) {

        *(fbr+i) = 10.0 * log10(MagCPX(*(buf.data()+j)) + baseline) + correction;
        *(fbr+j) = 10.0 * log10(MagCPX(*(buf.data()+i)) + baseline) + correction;
    }
*/
    for (int i = size - 1, j = half_sz - 1; i > half_sz - 1; i--, j--) {

        *(fbr+size-i-1) = 10.0 * log10(MagCPX(*(temp.data()+j)) + baseline) + correction;
        *(fbr+size-j-1) = 10.0 * log10(MagCPX(*(temp.data()+i)) + baseline) + correction;
    }
}
