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

#ifndef QCLUSEREVENT_H
#define QCLUSEREVENT_H

#include "qclevent.h"

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(CL)

class QCLContext;

class Q_CL_EXPORT QCLUserEvent : public QCLEvent
{
public:
    QCLUserEvent() : QCLEvent() {}
    QCLUserEvent(cl_event id);
    QCLUserEvent(const QCLEvent &other);

    QCLUserEvent &operator=(const QCLEvent &other);

    void setFinished();
    void setStatus(cl_int status);

private:
    void validateEvent();

    // Used by QCLContext::createUserEvent() to avoid
    // the overhead of validateEvent().
    QCLUserEvent(cl_event id, bool dummy)
        : QCLEvent(id) { Q_UNUSED(dummy); }

    friend class QCLContext;
};

inline void QCLUserEvent::setFinished()
{
    setStatus(CL_COMPLETE);
}

QT_END_NAMESPACE

QT_END_HEADER

#endif
