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

// Evaluate a Bezier patch at a (s, t) determined by the global work item.
__kernel void evaluateBezier
    (__global __write_only float4 *positions,
     __global __write_only float2 *texCoords,
     float16 matrixX, float16 matrixY, float16 matrixZ,
     int size)
{
    float s = ((float)(get_global_id(0))) / (size - 1);
    float s2 = s * s;
    float t = ((float)(get_global_id(1))) / (size - 1);
    float t2 = t * t;
    float4 S = (float4)(s2 * s, s2, s, 1);
    float4 T = (float4)(t2 * t, t2, t, 1);

    // Calculate the position of the item at (s, t).
    float4 smx = (float4)(dot(S, matrixX.lo.lo),
                          dot(S, matrixX.lo.hi),
                          dot(S, matrixX.hi.lo),
                          dot(S, matrixX.hi.hi));
    float4 smy = (float4)(dot(S, matrixY.lo.lo),
                          dot(S, matrixY.lo.hi),
                          dot(S, matrixY.hi.lo),
                          dot(S, matrixY.hi.hi));
    float4 smz = (float4)(dot(S, matrixZ.lo.lo),
                          dot(S, matrixZ.lo.hi),
                          dot(S, matrixZ.hi.lo),
                          dot(S, matrixZ.hi.hi));

    // Output the results.
    int offset = get_global_id(0) + get_global_id(1) * size;
    positions[offset] = (float4)(dot(smx, T), dot(smy, T), dot(smz, T), 1);
    texCoords[offset] = (float2)(s, t);
}
