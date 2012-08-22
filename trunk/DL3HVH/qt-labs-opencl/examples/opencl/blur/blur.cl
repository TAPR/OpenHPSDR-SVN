/****************************************************************************
**
** Copyright (C) 2010 Nokia Corporation and/or its subsidiary(-ies).
** All rights reserved.
** Contact: Nokia Corporation (qt-info@nokia.com)
**
** This file is part of the examples of the Qt Toolkit.
**
** $QT_BEGIN_LICENSE:BSD$
** You may use this file under the terms of the BSD license as follows:
**
** "Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions are
** met:
**   * Redistributions of source code must retain the above copyright
**     notice, this list of conditions and the following disclaimer.
**   * Redistributions in binary form must reproduce the above copyright
**     notice, this list of conditions and the following disclaimer in
**     the documentation and/or other materials provided with the
**     distribution.
**   * Neither the name of Nokia Corporation and its Subsidiary(-ies) nor
**     the names of its contributors may be used to endorse or promote
**     products derived from this software without specific prior written
**     permission.
**
** THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
** "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
** LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
** A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
** OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
** SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
** LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
** OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE."
** $QT_END_LICENSE$
**
****************************************************************************/

const sampler_t samp = CLK_ADDRESS_CLAMP_TO_EDGE |
                       CLK_FILTER_LINEAR;

//! [1]
__kernel void hgaussian(__read_only image2d_t srcImage,
                        __write_only image2d_t dstImage,
                        __global __read_only float *weights,
                        __global __read_only float *offsets,
                        const int numWeights)
//! [1]
{
    int2 pos = (int2)(get_global_id(0), get_global_id(1));
    float2 srcpos = (float2)(get_global_id(0), get_global_id(1));
    int index;
    float4 pixel = (float4)(0, 0, 0, 0);
    for (index = 0; index < numWeights; ++index) {
        pixel += read_imagef(srcImage, samp,
                             srcpos + (float2)(offsets[index], 0.0f)) *
                 weights[index];
    }
    write_imagef(dstImage, pos, clamp(pixel, 0.0f, 1.0f));
}

__kernel void vgaussian(__read_only image2d_t srcImage,
                        __write_only image2d_t dstImage,
                        __global __read_only float *weights,
                        __global __read_only float *offsets,
                        const int numWeights)
{
    int2 pos = (int2)(get_global_id(0), get_global_id(1));
    float2 srcpos = (float2)(get_global_id(0), get_global_id(1));
    int index;
    float4 pixel = (float4)(0, 0, 0, 0);
    for (index = 0; index < numWeights; ++index) {
        pixel += read_imagef(srcImage, samp,
                             srcpos + (float2)(0.0f, offsets[index])) *
                 weights[index];
    }
    write_imagef(dstImage, pos, clamp(pixel, 0.0f, 1.0f));
}
