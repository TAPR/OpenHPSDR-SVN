/****************************************************************************
**
** Copyright (C) 2010 Nokia Corporation and/or its subsidiary(-ies).
** All rights reserved.
** Contact: Nokia Corporation (qt-info@nokia.com)
**
** This file is part of the QtOpenCL module of the Qt Toolkit.
**
** $QT_BEGIN_LICENSE:LGPL$
** No Commercial Usage
** This file contains pre-release code and may not be distributed.
** You may use this file in accordance with the terms and conditions
** contained in the Technology Preview License Agreement accompanying
** this package.
**
** GNU Lesser General Public License Usage
** Alternatively, this file may be used under the terms of the GNU Lesser
** General Public License version 2.1 as published by the Free Software
** Foundation and appearing in the file LICENSE.LGPL included in the
** packaging of this file.  Please review the following information to
** ensure the GNU Lesser General Public License version 2.1 requirements
** will be met: http://www.gnu.org/licenses/old-licenses/lgpl-2.1.html.
**
** In addition, as a special exception, Nokia gives you certain additional
** rights.  These rights are described in the Nokia Qt LGPL Exception
** version 1.1, included in the file LGPL_EXCEPTION.txt in this package.
**
** If you have questions regarding the use of this file, please contact
** Nokia at qt-info@nokia.com.
**
**
**
**
**
**
**
**
** $QT_END_LICENSE$
**
****************************************************************************/

__kernel void mandelbrot_per_pixel
        (__global __write_only int *data,
         __global __read_only int *colors,
         const float regionx, const float regiony,
         const float regionwidth, const float regionheight,
         const int pixelwidth, const int pixelheight,
         const int maxIterations)
{
    int xpixel = get_global_id(0);
    int ypixel = get_global_id(1);
    float xin = regionx + xpixel * regionwidth / pixelwidth;
    float yin = regiony + ypixel * regionheight / pixelheight;
    int iteration = 0;
    float x = 0;
    float y = 0;
    while (iteration < maxIterations) {
        float x2 = x * x;
        float y2 = y * y;
        if ((x2 + y2) > 4.0)
            break;
        float xtemp = x2 - y2 + xin;
        y = 2 * x * y + yin;
        x = xtemp;
        ++iteration;
    }
    if (iteration < maxIterations)
        data[ypixel * pixelwidth + xpixel] = colors[iteration];
    else
        data[ypixel * pixelwidth + xpixel] = 0xff000000;
}

__kernel void mandelbrot_per_group
        (__global __write_only int *data,
         __global __read_only int *colors,
         const float regionx, const float regiony,
         const float regionwidth, const float regionheight,
         const int pixelwidth, const int pixelheight,
         const int maxIterations, const int groupSize)
{
    int xpixel = get_global_id(0) * groupSize;
    int ypixel = get_global_id(1) * groupSize;
    float xstep = regionwidth / pixelwidth;
    float ystep = regionheight / pixelheight;
    int xpos, ypos;
    float yin = regiony + ypixel * ystep;
    for (ypos = 0; ypos < groupSize; ++ypos) {
        float xin = regionx + xpixel * xstep;
        int datapos = (ypixel + ypos) * pixelwidth + xpixel;
        for (xpos = 0; xpos < groupSize; ++xpos) {
            int iteration = 0;
            float x = 0;
            float y = 0;
            while (iteration < maxIterations) {
                float x2 = x * x;
                float y2 = y * y;
                if ((x2 + y2) > 4.0)
                    break;
                float xtemp = x2 - y2 + xin;
                y = 2 * x * y + yin;
                x = xtemp;
                ++iteration;
            }
            if (iteration < maxIterations)
                data[datapos++] = colors[iteration];
            else
                data[datapos++] = 0xff000000;
            xin += xstep;
        }
        yin += ystep;
    }
}

__kernel void mandelbrot_image2d
        (__write_only image2d_t data,
         __global __read_only float4 *colors,
         const float regionx, const float regiony,
         const float regionwidth, const float regionheight,
         const int pixelwidth, const int pixelheight,
         const int maxIterations)
{
    int xpixel = get_global_id(0);
    int ypixel = get_global_id(1);
    float xin = regionx + xpixel * regionwidth / pixelwidth;
    float yin = regiony + ypixel * regionheight / pixelheight;
    int iteration = 0;
    float x = 0;
    float y = 0;
    while (iteration < maxIterations) {
        float x2 = x * x;
        float y2 = y * y;
        if ((x2 + y2) > 4.0)
            break;
        float xtemp = x2 - y2 + xin;
        y = 2 * x * y + yin;
        x = xtemp;
        ++iteration;
    }
    if (iteration < maxIterations)
        write_imagef(data, (int2)(xpixel, ypixel), colors[iteration]);
    else
        write_imagef(data, (int2)(xpixel, ypixel), (float4)(0, 0, 0, 1));
}
