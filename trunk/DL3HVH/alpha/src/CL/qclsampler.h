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

#ifndef QCLSAMPLER_H
#define QCLSAMPLER_H

#include "qclglobal.h"
#include <QtCore/qscopedpointer.h>

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(CL)

class QCLSamplerPrivate;
class QCLContext;

class Q_CL_EXPORT QCLSampler
{
public:
    QCLSampler() : m_context(0), m_id(0) {}
    QCLSampler(QCLContext *context, cl_sampler id)
        : m_context(context), m_id(id) {}
    QCLSampler(const QCLSampler &other);
    ~QCLSampler();

    QCLSampler &operator=(const QCLSampler &other);

    enum AddressingMode
    {
        None                = 0x1130,   // CL_ADDRESS_NONE
        ClampToEdge         = 0x1131,   // CL_ADDRESS_CLAMP_TO_EDGE
        Clamp               = 0x1132,   // CL_ADDRESS_CLAMP
        Repeat              = 0x1133    // CL_ADDRESS_REPEAT
    };

    enum FilterMode
    {
        Nearest             = 0x1140,   // CL_FILTER_NEAREST
        Linear              = 0x1141    // CL_FILTER_LINEAR
    };

    bool isNull() const { return m_id == 0; }

    bool normalizedCoordinates() const;
    QCLSampler::AddressingMode addressingMode() const;
    QCLSampler::FilterMode filterMode() const;

    cl_sampler samplerId() const { return m_id; }
    QCLContext *context() const { return m_context; }

    bool operator==(const QCLSampler &other) const;
    bool operator!=(const QCLSampler &other) const;

private:
    QCLContext *m_context;
    cl_sampler m_id;
};

inline QCLSampler::QCLSampler(const QCLSampler &other)
    : m_context(other.m_context), m_id(other.m_id)
{
    if (m_id)
        clRetainSampler(m_id);
}

inline QCLSampler::~QCLSampler()
{
    if (m_id)
        clReleaseSampler(m_id);
}

inline QCLSampler &QCLSampler::operator=(const QCLSampler &other)
{
    m_context = other.m_context;
    if (other.m_id)
        clRetainSampler(other.m_id);
    if (m_id)
        clReleaseSampler(m_id);
    m_id = other.m_id;
    return *this;
}

inline bool QCLSampler::operator==(const QCLSampler &other) const
{
    return m_id == other.m_id;
}

inline bool QCLSampler::operator!=(const QCLSampler &other) const
{
    return m_id != other.m_id;
}

QT_END_NAMESPACE

QT_END_HEADER

#endif
