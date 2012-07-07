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

#ifndef QCLPLATFORM_H
#define QCLPLATFORM_H

#include "qclglobal.h"
#include <QtCore/qlist.h>
#include <QtCore/qstring.h>
#include <QtCore/qstringlist.h>

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(CL)

class Q_CL_EXPORT QCLPlatform
{
public:
    QCLPlatform() : m_id(0), m_flags(0) {}
    QCLPlatform(cl_platform_id id) : m_id(id), m_flags(0) {}

    bool isNull() const { return m_id == 0; }

    bool isFullProfile() const;
    bool isEmbeddedProfile() const;

    QString profile() const;
    QString version() const;
    QString name() const;
    QString vendor() const;
    QString extensionSuffix() const;
    QStringList extensions() const;

    bool hasExtension(const char *name) const;

    enum VersionFlag
    {
        Version_1_0     = 0x0001,
        Version_1_1     = 0x0002
    };
    Q_DECLARE_FLAGS(VersionFlags, VersionFlag)

    QCLPlatform::VersionFlags versionFlags() const;

    cl_platform_id platformId() const { return m_id; }

    static QList<QCLPlatform> platforms();

    bool operator==(const QCLPlatform &other) const;
    bool operator!=(const QCLPlatform &other) const;

private:
    cl_platform_id m_id;
    mutable int m_flags;
};

Q_DECLARE_OPERATORS_FOR_FLAGS(QCLPlatform::VersionFlags)

inline bool QCLPlatform::operator==(const QCLPlatform &other) const
{
    return m_id == other.m_id;
}

inline bool QCLPlatform::operator!=(const QCLPlatform &other) const
{
    return m_id != other.m_id;
}

#ifndef QT_NO_DEBUG_STREAM
Q_CL_EXPORT QDebug operator<<(QDebug, const QCLPlatform &);
#endif

QT_END_NAMESPACE

QT_END_HEADER

#endif
