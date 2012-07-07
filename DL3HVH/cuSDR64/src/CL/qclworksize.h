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

#ifndef QCLWORKSIZE_H
#define QCLWORKSIZE_H

#include "qclglobal.h"
#include <QtCore/qsize.h>

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(CL)

class QCLDevice;

class Q_CL_EXPORT QCLWorkSize
{
public:
    QCLWorkSize()
        : m_dim(1) { m_sizes[0] = 1; m_sizes[1] = 1; m_sizes[2] = 1; }
    QCLWorkSize(size_t size)
        : m_dim(1) { m_sizes[0] = size; m_sizes[1] = 1; m_sizes[2] = 1; }
    QCLWorkSize(size_t width, size_t height)
        : m_dim(2) { m_sizes[0] = width; m_sizes[1] = height; m_sizes[2] = 1; }
    QCLWorkSize(const QSize &size)
        : m_dim(2) { m_sizes[0] = size.width(); m_sizes[1] = size.height(); m_sizes[2] = 1; }
    QCLWorkSize(size_t width, size_t height, size_t depth)
        : m_dim(3)
        { m_sizes[0] = width; m_sizes[1] = height; m_sizes[2] = depth; }

    size_t dimensions() const { return m_dim; }
    size_t width() const { return m_sizes[0]; }
    size_t height() const { return m_sizes[1]; }
    size_t depth() const { return m_sizes[2]; }

    const size_t *sizes() const { return m_sizes; }

    bool operator==(const QCLWorkSize &other) const;
    bool operator!=(const QCLWorkSize &other) const;

    QCLWorkSize toLocalWorkSize
        (const QCLWorkSize &maxWorkItemSize, size_t maxItemsPerGroup) const;
    QCLWorkSize toLocalWorkSize(const QCLDevice &device) const;

    QCLWorkSize roundTo(const QCLWorkSize &size) const;

    QString toString() const;
    static QCLWorkSize fromString(const QString &str);

private:
    size_t m_dim;
    size_t m_sizes[3];
};

Q_DECLARE_TYPEINFO(QCLWorkSize, Q_MOVABLE_TYPE);

inline bool QCLWorkSize::operator==(const QCLWorkSize &other) const
{
    return m_dim == other.m_dim &&
           m_sizes[0] == other.m_sizes[0] &&
           m_sizes[1] == other.m_sizes[1] &&
           m_sizes[2] == other.m_sizes[2];
}

inline bool QCLWorkSize::operator!=(const QCLWorkSize &other) const
{
    return m_dim != other.m_dim ||
           m_sizes[0] != other.m_sizes[0] ||
           m_sizes[1] != other.m_sizes[1] ||
           m_sizes[2] != other.m_sizes[2];
}

#ifndef QT_NO_DATASTREAM
Q_CL_EXPORT QDataStream &operator<<(QDataStream &, const QCLWorkSize &);
Q_CL_EXPORT QDataStream &operator>>(QDataStream &, QCLWorkSize &);
#endif

#ifndef QT_NO_DEBUG_STREAM
Q_CL_EXPORT QDebug operator<<(QDebug, const QCLWorkSize &);
#endif

QT_END_NAMESPACE

QT_END_HEADER

#endif
