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

#ifndef QCLPROGRAM_H
#define QCLPROGRAM_H

#include "qcldevice.h"
#include "qclkernel.h"
#include <QtCore/qstring.h>
#include <QtCore/qbytearray.h>

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(CL)

class QCLContext;

class Q_CL_EXPORT QCLProgram
{
public:
    QCLProgram() : m_context(0), m_id(0) {}
    QCLProgram(QCLContext *context, cl_program id)
        : m_context(context), m_id(id) {}
    QCLProgram(const QCLProgram &other);
    ~QCLProgram();

    QCLProgram &operator=(const QCLProgram &other);

    bool isNull() const { return m_id == 0; }

    cl_program programId() const { return m_id; }
    QCLContext *context() const { return m_context; }

    bool build(const QString &options = QString());
    bool build(const QList<QCLDevice> &devices, const QString &options = QString());

    QString log() const;

    QList<QCLDevice> devices() const;
    QByteArray sourceCode() const;
    QList<QByteArray> binaries() const;

    QCLKernel createKernel(const char *name) const;
    QCLKernel createKernel(const QByteArray &name) const;
    QCLKernel createKernel(const QString &name) const;

    QList<QCLKernel> createKernels() const;

    static void unloadCompiler();

    bool operator==(const QCLProgram &other) const;
    bool operator!=(const QCLProgram &other) const;

private:
    QCLContext *m_context;
    cl_program m_id;
};

inline bool QCLProgram::operator==(const QCLProgram &other) const
{
    return m_id == other.m_id;
}

inline QCLProgram::QCLProgram(const QCLProgram &other)
    : m_context(other.m_context), m_id(other.m_id)
{
    if (m_id)
        clRetainProgram(m_id);
}

inline QCLProgram::~QCLProgram()
{
    if (m_id)
        clReleaseProgram(m_id);
}

inline QCLProgram &QCLProgram::operator=(const QCLProgram &other)
{
    m_context = other.m_context;
    if (other.m_id)
        clRetainProgram(other.m_id);
    if (m_id)
        clReleaseProgram(m_id);
    m_id = other.m_id;
    return *this;
}

inline bool QCLProgram::operator!=(const QCLProgram &other) const
{
    return m_id != other.m_id;
}

QT_END_NAMESPACE

QT_END_HEADER

#endif
