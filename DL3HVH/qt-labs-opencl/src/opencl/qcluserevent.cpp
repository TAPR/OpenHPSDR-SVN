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

#include "qcluserevent.h"
#include "qclcontext.h"
#include "qclext_p.h"
#include <QtCore/qdebug.h>

QT_BEGIN_NAMESPACE

/*!
    \class QCLUserEvent
    \brief The QCLUserEvent class represents OpenCL 1.1 user events.
    \since 4.7
    \ingroup opencl

    User events are a feature of OpenCL 1.1 which allows an application
    to insert a marker into the command queue.  Commands that depend
    upon the marker will not be executed until the application triggers
    the user event with setFinished().

    User events are constructed with QCLContext::createUserEvent(),
    which will return null if the OpenCL implementation does not
    support OpenCL 1.1.

    \sa QCLEvent, QCLContext::createUserEvent()
*/

/*!
    \fn QCLUserEvent::QCLUserEvent()

    Constructs a null user event.
*/

/*!
    Constructs an OpenCL event object from the native identifier \a id.
    This class takes over ownership of \a id and will release it in
    the destructor.

    If \a id is not a user event, then the newly constructed event
    will be set to null, and \a id will be released.
*/
QCLUserEvent::QCLUserEvent(cl_event id)
    : QCLEvent(id)
{
    validateEvent();
}

/*!
    Constructs a copy of \a other.  The \c{clRetainEvent()} function
    will be called to update the reference count on eventId().

    If \a other is not a user event, then the newly constructed event
    will be set to null.
*/
QCLUserEvent::QCLUserEvent(const QCLEvent &other)
    : QCLEvent(other)
{
    validateEvent();
}

/*!
    Assigns \a other to this OpenCL event object.  The current eventId() will
    be released with \c{clReleaseEvent()}, and the new eventId() will be
    retained with \c{clRetainEvent()}.

    If \a other is not a user event, then this event will be
    set to null.
*/
QCLUserEvent &QCLUserEvent::operator=(const QCLEvent &other)
{
    if (m_id != other.m_id) {
        if (m_id)
            clReleaseEvent(m_id);
        m_id = other.m_id;
        if (m_id)
            clRetainEvent(m_id);
        validateEvent();
    }
    return *this;
}

/*!
    \fn void QCLUserEvent::setFinished()

    Sets this user event to the finished state.  Any queued
    commands that depend upon this event can now proceed.

    \sa setStatus()
*/

/*!
    Sets the \a status of this user event.  The \a status should
    be either \c{CL_COMPLETE} or a negative OpenCL error code.

    \sa setFinished()
*/
void QCLUserEvent::setStatus(cl_int status)
{
#ifdef QT_OPENCL_1_1
    if (m_id) {
        cl_int error = clSetUserEventStatus(m_id, status);
        if (error != CL_SUCCESS) {
            qWarning() << "QCLUserEvent::setStatus:"
                    << QCLContext::errorName(error);
        }
    }
#else
    Q_UNUSED(status);
#endif
}

/*!
    \internal
*/
void QCLUserEvent::validateEvent()
{
    if (m_id && commandType() != CL_COMMAND_USER) {
        clReleaseEvent(m_id);
        m_id = 0;
    }
}

QT_END_NAMESPACE
