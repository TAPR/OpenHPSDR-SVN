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

__kernel void fillRectWithColor
    (__write_only image2d_t dstImage,
     int offsetX, int offsetY, int limitX, int limitY,
     float4 color)
{
    int x = get_global_id(0) + offsetX;
    int y = get_global_id(1) + offsetY;
    if (x < limitX && y < limitY)
        write_imagef(dstImage, (int2)(x, y), color);
}

const sampler_t lsamp = CLK_ADDRESS_CLAMP_TO_EDGE |
                        CLK_FILTER_LINEAR;
const sampler_t nsamp = CLK_ADDRESS_CLAMP_TO_EDGE |
                        CLK_FILTER_NEAREST;

__kernel void drawImage
    (__write_only image2d_t dstImage,
     __read_only image2d_t dst2Image,
     __read_only image2d_t srcImage,
     int dstx, int dsty, int dstw, int dsth,
     float4 src, float opacity)
{
    int2 dstPos = (int2)(get_global_id(0) + dstx,
                         get_global_id(1) + dsty);
    float2 srcPos = (float2)(get_global_id(0) * src.z / dstw + src.x,
                             get_global_id(1) * src.w / dsth + src.y);
    if (dstPos.x < dstx || dstPos.x >= (dstx + dstw) ||
            dstPos.y < dsty || dstPos.y >= (dsty + dsth))
        return;     // Undrawn extra pixel due to 8x8 round up.
    float4 scolor = read_imagef(srcImage, lsamp, srcPos);
    float4 dcolor = read_imagef(dst2Image, nsamp, dstPos);
    // Destination is assumed to be RGB, source may be RGBA.
    dcolor = (float4)(dcolor.xyz * (1.0f - scolor.w * opacity) + scolor.xyz * scolor.w * opacity, 1.0f);
    write_imagef(dstImage, dstPos, clamp(dcolor, 0.0f, 1.0f));
}
