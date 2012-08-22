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

#include "qclbuffer.h"
#include "qclimage.h"
#include "qclcontext.h"
#include "qclext_p.h"

QT_BEGIN_NAMESPACE

/*!
    \class QCLBuffer
    \brief The QCLBuffer class represents an OpenCL buffer object.
    \since 4.7
    \ingroup opencl

    \sa QCLVector
*/

/*!
    \fn QCLBuffer::QCLBuffer()

    Constructs a null OpenCL buffer object.
*/

/*!
    \fn QCLBuffer::QCLBuffer(QCLContext *context, cl_mem id)

    Constructs an OpenCL buffer object that is initialized with the
    native OpenCL identifier \a id, and associates it with \a context.
    This class will take over ownership of \a id and will release
    it in the destructor.
*/

/*!
    \fn QCLBuffer::QCLBuffer(const QCLBuffer &other)

    Constructs a copy of \a other.
*/

/*!
    \fn QCLBuffer &QCLBuffer::operator=(const QCLBuffer &other)

    Assigns \a other to this object.
*/

/*!
    Reads \a size bytes from this buffer, starting at \a offset,
    into the supplied \a data array.  Returns true if the read
    was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa readAsync(), write()
*/
bool QCLBuffer::read(size_t offset, void *data, size_t size)
{
    cl_int error = clEnqueueReadBuffer
        (context()->activeQueue(), memoryId(),
         CL_TRUE, offset, size, data, 0, 0, 0);
    context()->reportError("QCLBuffer::read:", error);
    return error == CL_SUCCESS;
}

/*!
    \overload

    Reads \a size bytes from this buffer, starting at offset zero,
    into the supplied \a data array.  Returns true if the read
    was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa readAsync(), write()
*/
bool QCLBuffer::read(void *data, size_t size)
{
    cl_int error = clEnqueueReadBuffer
        (context()->activeQueue(), memoryId(),
         CL_TRUE, 0, size, data, 0, 0, 0);
    context()->reportError("QCLBuffer::read:", error);
    return error == CL_SUCCESS;
}

/*!
    Reads \a size bytes from this buffer, starting at \a offset,
    into the supplied \a data array.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa read(), writeAsync()
*/
QCLEvent QCLBuffer::readAsync(size_t offset, void *data, size_t size,
                              const QCLEventList &after)
{
    cl_event event;
    cl_int error = clEnqueueReadBuffer
        (context()->activeQueue(), memoryId(), CL_FALSE, offset, size, data,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLBuffer::readAsync:", error);
    if (error != CL_SUCCESS)
        return QCLEvent();
    else
        return QCLEvent(event);
}

/*!
    Reads the bytes defined by \a rect and \a bufferBytesPerLine
    from this buffer into the supplied \a data array, with a line
    pitch of \a hostBytesPerLine.  Returns true if the read
    was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    This function is only supported in OpenCL 1.1 and higher.

    \sa readRectAsync(), writeRect()
*/
bool QCLBuffer::readRect
    (const QRect &rect, void *data,
     size_t bufferBytesPerLine, size_t hostBytesPerLine)
{
#ifdef QT_OPENCL_1_1
    size_t bufferOrigin[3] = {rect.x(), rect.y(), 0};
    size_t bufferRegion[3] = {rect.width(), rect.height(), 1};
    static size_t const hostOrigin[3] = {0, 0, 0};
    cl_int error = clEnqueueReadBufferRect
        (context()->activeQueue(), memoryId(),
         CL_TRUE, bufferOrigin, hostOrigin, bufferRegion,
         bufferBytesPerLine, 0, hostBytesPerLine, 0,
         data, 0, 0, 0);
    context()->reportError("QCLBuffer::readRect:", error);
    return error == CL_SUCCESS;
#else
    context()->reportError("QCLBuffer::readRect:", CL_INVALID_OPERATION);
    Q_UNUSED(rect);
    Q_UNUSED(data);
    Q_UNUSED(bufferBytesPerLine);
    Q_UNUSED(hostBytesPerLine);
    return false;
#endif
}

/*!
    Reads the bytes in the 3D region defined by \a origin, \a size,
    \a bufferBytesPerLine, and \a bufferBytesPerSlice from this buffer
    into the supplied \a data array, with a line pitch of
    \a hostBytesPerLine, and a slice pitch of \a hostBytesPerSlice.
    Returns true if the read was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    This function is only supported in OpenCL 1.1 and higher.

    \sa readRectAsync(), writeRect()
*/
bool QCLBuffer::readRect
    (const size_t origin[3], const size_t size[3], void *data,
     size_t bufferBytesPerLine, size_t bufferBytesPerSlice,
     size_t hostBytesPerLine, size_t hostBytesPerSlice)
{
#ifdef QT_OPENCL_1_1
    static size_t const hostOrigin[3] = {0, 0, 0};
    cl_int error = clEnqueueReadBufferRect
        (context()->activeQueue(), memoryId(),
         CL_TRUE, origin, hostOrigin, size,
         bufferBytesPerLine, bufferBytesPerSlice,
         hostBytesPerLine, hostBytesPerSlice, data, 0, 0, 0);
    context()->reportError("QCLBuffer::readRect(3D):", error);
    return error == CL_SUCCESS;
#else
    context()->reportError("QCLBuffer::readRect(3D):", CL_INVALID_OPERATION);
    Q_UNUSED(origin);
    Q_UNUSED(size);
    Q_UNUSED(data);
    Q_UNUSED(bufferBytesPerLine);
    Q_UNUSED(bufferBytesPerSlice);
    Q_UNUSED(hostBytesPerLine);
    Q_UNUSED(hostBytesPerSlice);
    return false;
#endif
}

/*!
    Reads the bytes defined by \a rect and \a bufferBytesPerLine
    from this buffer into the supplied \a data array, with a line
    pitch of \a hostBytesPerLine.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    This function is only supported in OpenCL 1.1 and higher.

    \sa readRect(), writeRectAsync()
*/
QCLEvent QCLBuffer::readRectAsync
    (const QRect &rect, void *data,
     size_t bufferBytesPerLine, size_t hostBytesPerLine,
     const QCLEventList &after)
{
#ifdef QT_OPENCL_1_1
    size_t bufferOrigin[3] = {rect.x(), rect.y(), 0};
    size_t bufferRegion[3] = {rect.width(), rect.height(), 1};
    static size_t const hostOrigin[3] = {0, 0, 0};
    cl_event event;
    cl_int error = clEnqueueReadBufferRect
        (context()->activeQueue(), memoryId(),
         CL_FALSE, bufferOrigin, hostOrigin, bufferRegion,
         bufferBytesPerLine, 0, hostBytesPerLine, 0, data,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLBuffer::readRectAsync:", error);
    if (error != CL_SUCCESS)
        return QCLEvent();
    else
        return QCLEvent(event);
#else
    context()->reportError("QCLBuffer::readRectAsync:", CL_INVALID_OPERATION);
    Q_UNUSED(rect);
    Q_UNUSED(data);
    Q_UNUSED(bufferBytesPerLine);
    Q_UNUSED(hostBytesPerLine);
    Q_UNUSED(after);
    return QCLEvent();
#endif
}

/*!
    Reads the bytes in the 3D region defined by \a origin, \a size,
    \a bufferBytesPerLine, and \a bufferBytesPerSlice from this buffer
    into the supplied \a data array, with a line pitch of
    \a hostBytesPerLine, and a slice pitch of \a hostBytesPerSlice.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    This function is only supported in OpenCL 1.1 and higher.

    \sa readRect(), writeRectAsync()
*/
QCLEvent QCLBuffer::readRectAsync
    (const size_t origin[3], const size_t size[3], void *data,
     size_t bufferBytesPerLine, size_t bufferBytesPerSlice,
     size_t hostBytesPerLine, size_t hostBytesPerSlice,
     const QCLEventList &after)
{
#ifdef QT_OPENCL_1_1
    static size_t const hostOrigin[3] = {0, 0, 0};
    cl_event event;
    cl_int error = clEnqueueReadBufferRect
        (context()->activeQueue(), memoryId(),
         CL_FALSE, origin, hostOrigin, size,
         bufferBytesPerLine, bufferBytesPerSlice,
         hostBytesPerLine, hostBytesPerSlice, data,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLBuffer::readRectAsync(3D):", error);
    if (error != CL_SUCCESS)
        return QCLEvent();
    else
        return QCLEvent(event);
#else
    context()->reportError("QCLBuffer::readRectAsync(3D):", CL_INVALID_OPERATION);
    Q_UNUSED(origin);
    Q_UNUSED(size);
    Q_UNUSED(data);
    Q_UNUSED(bufferBytesPerLine);
    Q_UNUSED(bufferBytesPerSlice);
    Q_UNUSED(hostBytesPerLine);
    Q_UNUSED(hostBytesPerSlice);
    Q_UNUSED(after);
    return QCLEvent();
#endif
}

/*!
    Writes \a size bytes to this buffer, starting at \a offset,
    from the supplied \a data array.  Returns true if the write
    was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa writeAsync(), read()
*/
bool QCLBuffer::write(size_t offset, const void *data, size_t size)
{
    cl_int error = clEnqueueWriteBuffer
        (context()->activeQueue(), memoryId(),
         CL_TRUE, offset, size, data, 0, 0, 0);
    context()->reportError("QCLBuffer::write:", error);
    return error == CL_SUCCESS;
}

/*!
    Writes \a size bytes to this buffer, starting at offset zero,
    from the supplied \a data array.  Returns true if the write
    was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa writeAsync(), read()
*/
bool QCLBuffer::write(const void *data, size_t size)
{
    cl_int error = clEnqueueWriteBuffer
        (context()->activeQueue(), memoryId(),
         CL_TRUE, 0, size, data, 0, 0, 0);
    context()->reportError("QCLBuffer::write:", error);
    return error == CL_SUCCESS;
}

/*!
    Writes \a size bytes to this buffer, starting at \a offset,
    from the supplied \a data array.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa write(), readAsync()
*/
QCLEvent QCLBuffer::writeAsync(size_t offset, const void *data, size_t size,
                               const QCLEventList &after)
{
    cl_event event;
    cl_int error = clEnqueueWriteBuffer
        (context()->activeQueue(), memoryId(), CL_FALSE, offset, size, data,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLBuffer::writeAsync:", error);
    if (error != CL_SUCCESS)
        return QCLEvent();
    else
        return QCLEvent(event);
}

/*!
    Writes the bytes at \a data, with a line pitch of \a hostBytesPerLine
    to the region of this buffer defined by \a rect and \a bufferBytesPerLine.
    Returns true if the write was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    This function is only supported in OpenCL 1.1 and higher.

    \sa writeRectAsync(), readRect()
*/
bool QCLBuffer::writeRect
    (const QRect &rect, const void *data,
     size_t bufferBytesPerLine, size_t hostBytesPerLine)
{
#ifdef QT_OPENCL_1_1
    size_t bufferOrigin[3] = {rect.x(), rect.y(), 0};
    size_t bufferRegion[3] = {rect.width(), rect.height(), 1};
    static size_t const hostOrigin[3] = {0, 0, 0};
    cl_int error = clEnqueueWriteBufferRect
        (context()->activeQueue(), memoryId(),
         CL_TRUE, bufferOrigin, hostOrigin, bufferRegion,
         bufferBytesPerLine, 0, hostBytesPerLine, 0,
         data, 0, 0, 0);
    context()->reportError("QCLBuffer::writeRect:", error);
    return error == CL_SUCCESS;
#else
    context()->reportError("QCLBuffer::writeRect:", CL_INVALID_OPERATION);
    Q_UNUSED(rect);
    Q_UNUSED(data);
    Q_UNUSED(bufferBytesPerLine);
    Q_UNUSED(hostBytesPerLine);
    return false;
#endif
}

/*!
    Writes the bytes at \a data, with a line pitch of \a hostBytesPerLine,
    and a slice pitch of \a hostBytesPerSlice, to the 3D region defined
    by \a origin, \a size, \a bufferBytesPerLine, and \a bufferBytesPerSlice
    in this buffer.  Returns true if the write was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    This function is only supported in OpenCL 1.1 and higher.

    \sa writeRectAsync(), readRect()
*/
bool QCLBuffer::writeRect
    (const size_t origin[3], const size_t size[3], const void *data,
     size_t bufferBytesPerLine, size_t bufferBytesPerSlice,
     size_t hostBytesPerLine, size_t hostBytesPerSlice)
{
#ifdef QT_OPENCL_1_1
    static size_t const hostOrigin[3] = {0, 0, 0};
    cl_int error = clEnqueueWriteBufferRect
        (context()->activeQueue(), memoryId(),
         CL_TRUE, origin, hostOrigin, size,
         bufferBytesPerLine, bufferBytesPerSlice,
         hostBytesPerLine, hostBytesPerSlice, data, 0, 0, 0);
    context()->reportError("QCLBuffer::writeRect(3D):", error);
    return error == CL_SUCCESS;
#else
    context()->reportError("QCLBuffer::writeRect(3D):", CL_INVALID_OPERATION);
    Q_UNUSED(origin);
    Q_UNUSED(size);
    Q_UNUSED(data);
    Q_UNUSED(bufferBytesPerLine);
    Q_UNUSED(bufferBytesPerSlice);
    Q_UNUSED(hostBytesPerLine);
    Q_UNUSED(hostBytesPerSlice);
    return false;
#endif
}

/*!
    Writes the bytes at \a data, with a line pitch of \a hostBytesPerLine
    to the region of this buffer defined by \a rect and \a bufferBytesPerLine.
    Returns true if the write was successful; false otherwise.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    This function is only supported in OpenCL 1.1 and higher.

    \sa writeRect(), readRectAsync()
*/
QCLEvent QCLBuffer::writeRectAsync
    (const QRect &rect, const void *data,
     size_t bufferBytesPerLine, size_t hostBytesPerLine,
     const QCLEventList &after)
{
#ifdef QT_OPENCL_1_1
    size_t bufferOrigin[3] = {rect.x(), rect.y(), 0};
    size_t bufferRegion[3] = {rect.width(), rect.height(), 1};
    static size_t const hostOrigin[3] = {0, 0, 0};
    cl_event event;
    cl_int error = clEnqueueWriteBufferRect
        (context()->activeQueue(), memoryId(),
         CL_FALSE, bufferOrigin, hostOrigin, bufferRegion,
         bufferBytesPerLine, 0, hostBytesPerLine, 0, data,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLBuffer::writeRectAsync:", error);
    if (error != CL_SUCCESS)
        return QCLEvent();
    else
        return QCLEvent(event);
#else
    context()->reportError("QCLBuffer::writeRectAsync:", CL_INVALID_OPERATION);
    Q_UNUSED(rect);
    Q_UNUSED(data);
    Q_UNUSED(bufferBytesPerLine);
    Q_UNUSED(hostBytesPerLine);
    Q_UNUSED(after);
    return QCLEvent();
#endif
}

/*!
    Writes the bytes at \a data, with a line pitch of \a hostBytesPerLine,
    and a slice pitch of \a hostBytesPerSlice, to the 3D region defined
    by \a origin, \a size, \a bufferBytesPerLine, and \a bufferBytesPerSlice
    in this buffer.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    This function is only supported in OpenCL 1.1 and higher.

    \sa writeRect(), readRectAsync()
*/
QCLEvent QCLBuffer::writeRectAsync
    (const size_t origin[3], const size_t size[3], const void *data,
     size_t bufferBytesPerLine, size_t bufferBytesPerSlice,
     size_t hostBytesPerLine, size_t hostBytesPerSlice,
     const QCLEventList &after)
{
#ifdef QT_OPENCL_1_1
    static size_t const hostOrigin[3] = {0, 0, 0};
    cl_event event;
    cl_int error = clEnqueueWriteBufferRect
        (context()->activeQueue(), memoryId(),
         CL_FALSE, origin, hostOrigin, size,
         bufferBytesPerLine, bufferBytesPerSlice,
         hostBytesPerLine, hostBytesPerSlice, data,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLBuffer::writeRectAsync(3D):", error);
    if (error != CL_SUCCESS)
        return QCLEvent();
    else
        return QCLEvent(event);
#else
    context()->reportError("QCLBuffer::writeRectAsync(3D):", CL_INVALID_OPERATION);
    Q_UNUSED(origin);
    Q_UNUSED(size);
    Q_UNUSED(data);
    Q_UNUSED(bufferBytesPerLine);
    Q_UNUSED(bufferBytesPerSlice);
    Q_UNUSED(hostBytesPerLine);
    Q_UNUSED(hostBytesPerSlice);
    Q_UNUSED(after);
    return QCLEvent();
#endif
}

/*!
    Copies the \a size bytes at \a offset in this buffer
    be copied to \a destOffset in the buffer \a dest.  Returns true
    if the copy was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa copyToAsync()
*/
bool QCLBuffer::copyTo
    (size_t offset, size_t size, const QCLBuffer &dest, size_t destOffset)
{
    cl_event event;
    cl_int error = clEnqueueCopyBuffer
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         offset, destOffset, size, 0, 0, &event);
    context()->reportError("QCLBuffer::copyTo(QCLBuffer):", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
        return true;
    } else {
        return false;
    }
}

/*!
    Copies the contents of this buffer, starting at \a offset to
    \a rect within \a dest.  Returns true if the copy was successful;
    false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa copyToAsync()
*/
bool QCLBuffer::copyTo
    (size_t offset, const QCLImage2D &dest, const QRect &rect)
{
    const size_t dst_origin[3] = {rect.x(), rect.y(), 0};
    const size_t region[3] = {rect.width(), rect.height(), 1};
    cl_event event;
    cl_int error = clEnqueueCopyBufferToImage
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         offset, dst_origin, region, 0, 0, &event);
    context()->reportError("QCLBuffer::copyTo(QCLImage2D):", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
        return true;
    } else {
        return false;
    }
}

/*!
    Copies the contents of this buffer, starting at \a offset to
    \a origin within \a dest, extending for \a size.  Returns true if
    the copy was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa copyToAsync()
*/
bool QCLBuffer::copyTo
    (size_t offset, const QCLImage3D &dest,
     const size_t origin[3], const size_t size[3])
{
    cl_event event;
    cl_int error = clEnqueueCopyBufferToImage
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         offset, origin, size, 0, 0, &event);
    context()->reportError("QCLBuffer::copyTo(QCLImage3D):", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
        return true;
    } else {
        return false;
    }
}

/*!
    Requests that the \a size bytes at \a offset in this buffer
    be copied to \a destOffset in the buffer \a dest.  Returns an
    event object that can be used to wait for the request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa copyTo()
*/
QCLEvent QCLBuffer::copyToAsync
    (size_t offset, size_t size, const QCLBuffer &dest, size_t destOffset,
     const QCLEventList &after)
{
    cl_event event;
    cl_int error = clEnqueueCopyBuffer
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         offset, destOffset, size,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLBuffer::copyToAsync:", error);
    if (error != CL_SUCCESS)
        return QCLEvent();
    else
        return QCLEvent(event);
}

/*!
    Copies the contents of this buffer, starting at \a offset to
    \a rect within \a dest.  Returns an event object that can be used
    to wait for the request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa copyTo()
*/
QCLEvent QCLBuffer::copyToAsync
    (size_t offset, const QCLImage2D &dest, const QRect &rect,
     const QCLEventList &after)
{
    const size_t dst_origin[3] = {rect.x(), rect.y(), 0};
    const size_t region[3] = {rect.width(), rect.height(), 1};
    cl_event event;
    cl_int error = clEnqueueCopyBufferToImage
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         offset, dst_origin, region,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLBuffer::copyToAsync(QCLImage2D):", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Copies the contents of this buffer, starting at \a offset to
    \a origin within \a dest, extending for \a size.  Returns an event
    object that can be used to wait for the request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa copyTo()
*/
QCLEvent QCLBuffer::copyToAsync
    (size_t offset, const QCLImage3D &dest,
     const size_t origin[3], const size_t size[3],
     const QCLEventList &after)
{
    cl_event event;
    cl_int error = clEnqueueCopyBufferToImage
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         offset, origin, size,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLBuffer::copyToAsync(QCLImage3D):", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Copies the contents of \a rect within this buffer to \a dest,
    starting at \a destPoint.  The source and destination line pitch
    values are given by \a bufferBytesPerLine and \a destBytesPerLine
    respectively.  Returns true if the copy was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    This function is only supported in OpenCL 1.1 and higher.

    \sa copyToRectAsync()
*/
bool QCLBuffer::copyToRect
    (const QRect &rect, const QCLBuffer &dest,
     const QPoint &destPoint, size_t bufferBytesPerLine,
     size_t destBytesPerLine)
{
#ifdef QT_OPENCL_1_1
    const size_t src_origin[3] = {rect.x(), rect.y(), 0};
    const size_t dst_origin[3] = {destPoint.x(), destPoint.y(), 0};
    const size_t region[3] = {rect.width(), rect.height(), 1};
    cl_event event;
    cl_int error = clEnqueueCopyBufferRect
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         src_origin, dst_origin, region,
         bufferBytesPerLine, 0, destBytesPerLine, 0, 0, 0, &event);
    context()->reportError("QCLBuffer::copyToRect:", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
        return true;
    } else {
        return false;
    }
#else
    context()->reportError("QCLBuffer::copyToRect:", CL_INVALID_OPERATION);
    Q_UNUSED(rect);
    Q_UNUSED(dest);
    Q_UNUSED(destPoint);
    Q_UNUSED(bufferBytesPerLine);
    Q_UNUSED(destBytesPerLine);
    return false;
#endif
}

/*!
    Copies the 3D rectangle defined by \a origin and \a size within
    this buffer to \a destOrigin within \a dest.  The source and destination
    pitch values are given by \a bufferBytesPerLine, \a bufferBytesPerSlice,
    \a destBytesPerLine, and \a destBytesPerSlice.  Returns true if
    the copy was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    This function is only supported in OpenCL 1.1 and higher.

    \sa copyToRectAsync()
*/
bool QCLBuffer::copyToRect
    (const size_t origin[3], const size_t size[3],
     const QCLBuffer &dest, const size_t destOrigin[3],
     size_t bufferBytesPerLine, size_t bufferBytesPerSlice,
     size_t destBytesPerLine, size_t destBytesPerSlice)
{
#ifdef QT_OPENCL_1_1
    cl_event event;
    cl_int error = clEnqueueCopyBufferRect
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         origin, destOrigin, size,
         bufferBytesPerLine, bufferBytesPerSlice,
         destBytesPerLine, destBytesPerSlice, 0, 0, &event);
    context()->reportError("QCLBuffer::copyToRect(3D):", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
        return true;
    } else {
        return false;
    }
#else
    context()->reportError("QCLBuffer::copyToRect(3D):", CL_INVALID_OPERATION);
    Q_UNUSED(origin);
    Q_UNUSED(size);
    Q_UNUSED(dest);
    Q_UNUSED(destOrigin);
    Q_UNUSED(bufferBytesPerLine);
    Q_UNUSED(bufferBytesPerSlice);
    Q_UNUSED(destBytesPerLine);
    Q_UNUSED(destBytesPerSlice);
    return false;
#endif
}

/*!
    Copies the contents of \a rect within this buffer to \a dest,
    starting at \a destPoint.  The source and destination line pitch
    values are given by \a bufferBytesPerLine and \a destBytesPerLine
    respectively.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    This function is only supported in OpenCL 1.1 and higher.

    \sa copyToRect()
*/
QCLEvent QCLBuffer::copyToRectAsync
    (const QRect &rect, const QCLBuffer &dest, const QPoint &destPoint,
     size_t bufferBytesPerLine, size_t destBytesPerLine,
     const QCLEventList &after)
{
#ifdef QT_OPENCL_1_1
    const size_t src_origin[3] = {rect.x(), rect.y(), 0};
    const size_t dst_origin[3] = {destPoint.x(), destPoint.y(), 0};
    const size_t region[3] = {rect.width(), rect.height(), 1};
    cl_event event;
    cl_int error = clEnqueueCopyBufferRect
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         src_origin, dst_origin, region,
         bufferBytesPerLine, 0, destBytesPerLine, 0,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLBuffer::copyToRectAsync:", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
#else
    context()->reportError("QCLBuffer::copyToRectAsync:", CL_INVALID_OPERATION);
    Q_UNUSED(rect);
    Q_UNUSED(dest);
    Q_UNUSED(destPoint);
    Q_UNUSED(bufferBytesPerLine);
    Q_UNUSED(destBytesPerLine);
    Q_UNUSED(after);
    return false;
#endif
}

/*!
    Copies the 3D rectangle defined by \a origin and \a size within
    this buffer to \a destOrigin within \a dest.  The source and destination
    pitch values are given by \a bufferBytesPerLine, \a bufferBytesPerSlice,
    \a destBytesPerLine, and \a destBytesPerSlice.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    This function is only supported in OpenCL 1.1 and higher.

    \sa copyToRectAsync()
*/
QCLEvent QCLBuffer::copyToRectAsync
    (const size_t origin[3], const size_t size[3],
     const QCLBuffer &dest, const size_t destOrigin[3],
     size_t bufferBytesPerLine, size_t bufferBytesPerSlice,
     size_t destBytesPerLine, size_t destBytesPerSlice,
     const QCLEventList &after)
{
#ifdef QT_OPENCL_1_1
    cl_event event;
    cl_int error = clEnqueueCopyBufferRect
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         origin, destOrigin, size,
         bufferBytesPerLine, bufferBytesPerSlice,
         destBytesPerLine, destBytesPerSlice,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLBuffer::copyToRectAsync(3D):", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
#else
    context()->reportError("QCLBuffer::copyToRectAsync(3D):", CL_INVALID_OPERATION);
    Q_UNUSED(origin);
    Q_UNUSED(size);
    Q_UNUSED(dest);
    Q_UNUSED(destOrigin);
    Q_UNUSED(bufferBytesPerLine);
    Q_UNUSED(bufferBytesPerSlice);
    Q_UNUSED(destBytesPerLine);
    Q_UNUSED(destBytesPerSlice);
    Q_UNUSED(after);
    return false;
#endif
}

// We use a single enum in the public API for both memory objects
// and memory mapping, but the values are slightly different in
// the OpenCL C API.  This function corrects for the difference.
cl_map_flags qt_cl_map_flags(QCLMemoryObject::Access access)
{
    if (access == QCLMemoryObject::ReadOnly)
        return CL_MAP_READ;
    else if (access == QCLMemoryObject::WriteOnly)
        return CL_MAP_WRITE;
    else
        return CL_MAP_READ | CL_MAP_WRITE;
}

/*!
    Maps the \a size bytes starting at \a offset in this buffer
    into host memory for the specified \a access mode.  Returns a
    pointer to the mapped region.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa mapAsync(), unmap()
*/
void *QCLBuffer::map
    (size_t offset, size_t size, QCLMemoryObject::Access access)
{
    cl_int error;
    void *data = clEnqueueMapBuffer
        (context()->activeQueue(), memoryId(), CL_TRUE,
         qt_cl_map_flags(access), offset, size, 0, 0, 0, &error);
    context()->reportError("QCLBuffer::map:", error);
    return data;
}

/*!
    \overload

    Maps the entire contents of this buffer into host memory for
    the specified \a access mode.  Returns a pointer to the
    mapped region.
*/
void *QCLBuffer::map(QCLMemoryObject::Access access)
{
    return map(0, size(), access);
}

/*!
    Maps the \a size bytes starting at \a offset in this buffer
    into host memory for the specified \a access mode.  Returns a
    pointer to the mapped region in \a ptr, which will be valid
    only after the request finishes.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa map(), unmapAsync()
*/
QCLEvent QCLBuffer::mapAsync
    (void **ptr, size_t offset, size_t size,
     QCLMemoryObject::Access access, const QCLEventList &after)
{
    cl_int error;
    cl_event event;
    *ptr = clEnqueueMapBuffer
        (context()->activeQueue(), memoryId(), CL_FALSE,
         qt_cl_map_flags(access), offset, size,
         after.size(), after.eventData(), &event, &error);
    context()->reportError("QCLBuffer::mapAsync:", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Creates a new buffer that refers to the \a size bytes,
    starting at \a offset within this buffer.  The data in
    the new buffer will be accessed according to \a access.

    Sub-buffers are an OpenCL 1.1 feature.  On OpenCL 1.0,
    this function will return a null buffer.

    \sa parentBuffer(), offset()
*/
QCLBuffer QCLBuffer::createSubBuffer
    (size_t offset, size_t size, QCLMemoryObject::Access access)
{
#ifdef QT_OPENCL_1_1
    cl_int error;
    cl_buffer_region region;
    region.origin = offset;
    region.size = size;
    cl_mem mem = clCreateSubBuffer
        (memoryId(), cl_mem_flags(access),
         CL_BUFFER_CREATE_TYPE_REGION, &region, &error);
    context()->reportError("QCLBuffer::createSubBuffer:", error);
    return QCLBuffer(context(), mem);
#else
    Q_UNUSED(offset);
    Q_UNUSED(size);
    Q_UNUSED(access);
    return QCLBuffer();
#endif
}

/*!
    Returns the parent of this buffer if it is a sub-buffer;
    null otherwise.

    \sa createSubBuffer(), offset()
*/
QCLBuffer QCLBuffer::parentBuffer() const
{
    cl_mem parent;
    if (clGetMemObjectInfo(memoryId(), CL_MEM_ASSOCIATED_MEMOBJECT,
                           sizeof(parent), &parent, 0) != CL_SUCCESS)
        return QCLBuffer();
    if (parent)
        clRetainMemObject(parent);
    return QCLBuffer(context(), parent);
}

/*!
    Returns the offset of this buffer within its parentBuffer()
    if it is a sub-buffer; zero otherwise.

    \sa createSubBuffer(), parentBuffer()
*/
size_t QCLBuffer::offset() const
{
    size_t value;
    if (clGetMemObjectInfo(memoryId(), CL_MEM_OFFSET,
                           sizeof(value), &value, 0) != CL_SUCCESS)
        return 0;
    else
        return value;
}

QT_END_NAMESPACE
