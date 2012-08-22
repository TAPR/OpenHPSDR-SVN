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

// tst_QCL::argumentPassing()

__kernel void storeFloat(__global __write_only float *output, float input)
{
    output[0] = input;
}

__kernel void storeInt(__global __write_only int *output, int input)
{
    output[0] = input;
}

__kernel void storeUInt(__global __write_only uint *output, uint input)
{
    output[0] = input;
}

__kernel void storeLong(__global __write_only long *output, long input)
{
    output[0] = input;
}

__kernel void storeULong(__global __write_only ulong *output, ulong input)
{
    output[0] = input;
}

__kernel void storeVec2(__global __write_only float *output, float2 input)
{
    output[0] = input.x;
    output[1] = input.y;
}

__kernel void storeVec2i(__global __write_only int *output, int2 input)
{
    output[0] = input.x;
    output[1] = input.y;
}

__kernel void storeVec3(__global __write_only float *output, float4 input)
{
    output[0] = input.x;
    output[1] = input.y;
    output[2] = input.z;
    output[3] = input.w;
}

__kernel void storeVec4(__global __write_only float *output, float4 input)
{
    output[0] = input.x;
    output[1] = input.y;
    output[2] = input.z;
    output[3] = input.w;
}

__kernel void storeMat4(__global __write_only float4 *output, float16 input)
{
    output[0] = input.lo.lo;
    output[1] = input.lo.hi;
    output[2] = input.hi.lo;
    output[3] = input.hi.hi;
}

__kernel void addToVector(__global float *vector, float value)
{
    vector[get_global_id(0)] += value;
}
