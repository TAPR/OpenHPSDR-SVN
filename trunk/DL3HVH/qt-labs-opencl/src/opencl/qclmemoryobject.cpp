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

#include "qclmemoryobject.h"
#include "qclcontext.h"

QT_BEGIN_NAMESPACE

/*!
    \class QCLMemoryObject
    \brief The QCLMemoryObject class represents an OpenCL memory object.
    \since 4.7
    \ingroup opencl
*/

/*!
    \enum QCLMemoryObject::Access
    This enum defines the access mode to use when creating OpenCL
    memory objects.

    \value ReadWrite The memory object will be read and written by a kernel.
    \value WriteOnly The memory object will be written, but not read,
           by a kernel.
    \value ReadOnly The memory object will be read, but not written,
           by a kernel.
*/

/*!
    \fn QCLMemoryObject::QCLMemoryObject(QCLContext *context)

    Constructs a null OpenCL memory object and associates it with \a context.
*/

/*!
    \fn QCLMemoryObject::QCLMemoryObject(QCLContext *context, cl_mem id)

    Constructs an OpenCL memory object from the native identifier \a id,
    and associates it with \a context.  This class takes over ownership
    of \a id and will release it in the destructor.
*/

/*!
    \fn QCLMemoryObject::~QCLMemoryObject()

    Releases this OpenCL memory object.  If this is the last reference
    to the memory object, it will be destroyed.
*/

/*!
    \fn bool QCLMemoryObject::isNull() const

    Returns true if this OpenCL memory object is null; false otherwise.
*/

/*!
    \fn cl_mem QCLMemoryObject::memoryId() const

    Returns the native OpenCL identifier for this memory object.
*/

/*!
    \fn QCLContext *QCLMemoryObject::context() const

    Returns the OpenCL context that created this memory object.
*/

#define QCL_MEM_ACCESS_FLAGS    (CL_MEM_READ_WRITE | \
                                 CL_MEM_READ_ONLY | \
                                 CL_MEM_WRITE_ONLY)

/*!
    Returns the access mode that was used to create this memory object.

    \sa flags()
*/
QCLMemoryObject::Access QCLMemoryObject::access() const
{
    cl_mem_flags flags;
    if (clGetMemObjectInfo(m_id, CL_MEM_FLAGS,
                           sizeof(flags), &flags, 0) != CL_SUCCESS)
        return QCLMemoryObject::ReadWrite; // Have to return something.
    else
        return QCLMemoryObject::Access(flags & QCL_MEM_ACCESS_FLAGS);
}

/*!
    Returns the flags that were used to create this memory object.

    \sa access()
*/
cl_mem_flags QCLMemoryObject::flags() const
{
    cl_mem_flags flags;
    if (clGetMemObjectInfo(m_id, CL_MEM_FLAGS,
                           sizeof(flags), &flags, 0) != CL_SUCCESS)
        return 0;
    else
        return flags;
}

/*!
    Returns the host data pointer that was used to create this memory
    object, or null if the memory object is not directly accessible
    to the host.

    \sa size()
*/
void *QCLMemoryObject::hostPointer() const
{
    void *ptr;
    if (clGetMemObjectInfo(m_id, CL_MEM_HOST_PTR,
                           sizeof(ptr), &ptr, 0) != CL_SUCCESS)
        return 0;
    else
        return ptr;
}

/*!
    Returns the actual size of the data storage associated with
    this memory object.

    \sa hostPointer()
*/
size_t QCLMemoryObject::size() const
{
    size_t size;
    if (clGetMemObjectInfo(m_id, CL_MEM_SIZE,
                           sizeof(size), &size, 0) != CL_SUCCESS)
        return 0;
    else
        return size;
}

/*!
    Requests that the region at \a ptr that was previously mapped from an
    OpenCL buffer or image be unmapped.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa unmapAsync(), QCLBuffer::map()
*/
void QCLMemoryObject::unmap(void *ptr)
{
    cl_event event = 0;
    cl_int error = clEnqueueUnmapMemObject
        (context()->activeQueue(), memoryId(), ptr, 0, 0, &event);
    context()->reportError("QCLMemoryObject::unmap:", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
    }
}

/*!
    Requests that the region at \a ptr that was previously mapped from
    an OpenCL buffer or image be unmapped.

    The request will not start until all of the events in \a after
    have been signaled as finished.

    Returns an event object that can be used to wait for the
    request to finish.  The request is executed on the active
    command queue for context().

    \sa unmap(), QCLBuffer::mapAsync()
*/
QCLEvent QCLMemoryObject::unmapAsync(void *ptr, const QCLEventList &after)
{
    cl_event event;
    cl_int error = clEnqueueUnmapMemObject
        (context()->activeQueue(), memoryId(), ptr,
        after.size(), after.eventData(), &event);
    context()->reportError("QCLMemoryObject::unmapAsync:", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    \fn bool QCLMemoryObject::operator==(const QCLMemoryObject &other) const

    Returns true if this OpenCL memory object is the same as \a other;
    false otherwise.

    \sa operator!=()
*/

/*!
    \fn bool QCLMemoryObject::operator!=(const QCLMemoryObject &other) const

    Returns true if this OpenCL memory object is not the same as \a other;
    false otherwise.

    \sa operator==()
*/

/*!
    \fn void QCLMemoryObject::setId(QCLContext *context, cl_mem id)
    \internal
*/

QT_END_NAMESPACE
