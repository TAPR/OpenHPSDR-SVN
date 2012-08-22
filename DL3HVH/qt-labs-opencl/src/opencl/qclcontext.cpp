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

#include "qclcontext.h"
#include "qclext_p.h"
#include <QtCore/qdebug.h>
#include <QtCore/qvarlengtharray.h>
#include <QtCore/qfile.h>

QT_BEGIN_NAMESPACE

/*!
    \class QCLContext
    \brief The QCLContext class represents an OpenCL context.
    \since 4.7
    \ingroup opencl

    \sa QCLContextGL
*/

class QCLContextPrivate
{
public:
    QCLContextPrivate()
        : id(0)
        , isCreated(false)
        , lastError(CL_SUCCESS)
    {
    }
    ~QCLContextPrivate()
    {
        // Release the command queues for the context.
        commandQueue = QCLCommandQueue();
        defaultCommandQueue = QCLCommandQueue();

        // Release the context.
        if (isCreated)
            clReleaseContext(id);
    }

    cl_context id;
    bool isCreated;
    QCLCommandQueue commandQueue;
    QCLCommandQueue defaultCommandQueue;
    QCLDevice defaultDevice;
    cl_int lastError;
};

/*!
    Constructs a new OpenCL context object.  This constructor is
    typically followed by calls to setPlatform() and create().
*/
QCLContext::QCLContext()
    : d_ptr(new QCLContextPrivate())
{
}

/*!
    Destroys this OpenCL context object.  If the underlying
    contextId() has been created, then it will be released.
*/
QCLContext::~QCLContext()
{
}

/*!
    Returns true if the underlying OpenCL contextId() has been
    created; false otherwise.

    \sa create(), setContextId()
*/
bool QCLContext::isCreated() const
{
    Q_D(const QCLContext);
    return d->isCreated;
}

extern "C" {

static void qt_cl_context_notify(const char *errinfo,
                                 const void *private_info,
                                 size_t cb,
                                 void *user_data)
{
    Q_UNUSED(private_info);
    Q_UNUSED(cb);
    Q_UNUSED(user_data);
    qWarning() << "OpenCL context notification: " << errinfo;
}

};

/*!
    Creates a new OpenCL context that matches \a type.  Does nothing
    if the context has already been created.  The default value for
    \a type is QCLDevice::Default.

    This function will search for the first platform that has a device
    that matches \a type.  The following code can be used to select
    devices that match \a type on a specific platform:

    \code
    context.create(QCLDevice::devices(type, platform));
    \endcode

    Returns true if the context was created; false otherwise.
    On error, the status can be retrieved by calling lastError().

    \sa isCreated(), setContextId(), release()
*/
bool QCLContext::create(QCLDevice::DeviceTypes type)
{
    Q_D(QCLContext);
    if (d->isCreated)
        return true;
    // The "cl_khr_icd" extension says that a null platform cannot
    // be supplied to OpenCL any more, so find the first platform
    // that has devices that match "type".
    QList<QCLDevice> devices = QCLDevice::devices(type);
    if (!devices.isEmpty()) {
        QVector<cl_device_id> devs;
        foreach (QCLDevice dev, devices)
            devs.append(dev.deviceId());
        cl_context_properties props[] = {
            CL_CONTEXT_PLATFORM,
            cl_context_properties(devices[0].platform().platformId()),
            0
        };
        d->id = clCreateContext
            (props, devs.size(), devs.constData(),
             qt_cl_context_notify, 0, &(d->lastError));
    } else {
        d->lastError = CL_DEVICE_NOT_FOUND;
        d->id = 0;
    }
    d->isCreated = (d->id != 0);
    if (!d->isCreated) {
        qWarning() << "QCLContext::create(type:" << int(type) << "):"
                   << errorName(d->lastError);
    }
    return d->isCreated;
}

/*!
    Creates a new OpenCL context that matches \a devices.  Does nothing
    if the context has already been created.  All of the \a devices must
    be associated with the same platform.

    Returns true if the context was created; false otherwise.
    On error, the status can be retrieved by calling lastError().

    \sa isCreated(), setContextId(), release()
*/
bool QCLContext::create(const QList<QCLDevice> &devices)
{
    Q_D(QCLContext);
    if (d->isCreated)
        return true;
    if (devices.isEmpty()) {
        reportError("QCLContext::create:", CL_INVALID_VALUE);
        return false;
    }
    QVector<cl_device_id> devs;
    foreach (QCLDevice dev, devices)
        devs.append(dev.deviceId());
    cl_platform_id platform = devices[0].platform().platformId();
    cl_context_properties props[] = {
        CL_CONTEXT_PLATFORM,
        intptr_t(platform),
        0
    };
    d->id = clCreateContext
        (props, devs.size(), devs.constData(),
         qt_cl_context_notify, 0, &(d->lastError));
    d->isCreated = (d->id != 0);
    if (!d->isCreated)
        qWarning() << "QCLContext::create:" << errorName(d->lastError);
    return d->isCreated;
}

/*!
    Releases this context; destroying it if the reference count is zero.
    Does nothing if the context has not been created or is already released.

    \sa create()
*/
void QCLContext::release()
{
    Q_D(QCLContext);
    if (d->isCreated) {
        d->commandQueue = QCLCommandQueue();
        d->defaultCommandQueue = QCLCommandQueue();
        clReleaseContext(d->id);
        d->id = 0;
        d->defaultDevice = QCLDevice();
        d->isCreated = false;
    }
}

/*!
    Returns the native OpenCL context identifier associated
    with this object.

    \sa setContextId()
*/
cl_context QCLContext::contextId() const
{
    Q_D(const QCLContext);
    return d->id;
}

/*!
    Sets the native OpenCL context identifier associated with this
    object to \a id.

    This function will call \c{clRetainContext()} to increase the
    reference count on \a id.  If the identifier was previously set
    to something else, then \c{clReleaseContext()} will be called
    on the previous value.

    \sa contextId(), create()
*/
void QCLContext::setContextId(cl_context id)
{
    Q_D(QCLContext);
    if (d->id == id || !id)
        return;
    release();
    clRetainContext(id);
    d->id = id;
    d->isCreated = true;
}

/*!
    Returns the list of devices that are in use by this context.
    If the context has not been created, returns an empty list.

    \sa defaultDevice()
*/
QList<QCLDevice> QCLContext::devices() const
{
    Q_D(const QCLContext);
    QList<QCLDevice> devs;
    if (d->isCreated) {
        size_t size = 0;
        if (clGetContextInfo(d->id, CL_CONTEXT_DEVICES, 0, 0, &size)
                == CL_SUCCESS && size > 0) {
            QVarLengthArray<cl_device_id> buf(size);
            if (clGetContextInfo(d->id, CL_CONTEXT_DEVICES,
                                 size, buf.data(), 0) == CL_SUCCESS) {
                for (size_t index = 0; index < size; ++index)
                    devs.append(QCLDevice(buf[index]));
            }
        }
    }
    return devs;
}

/*!
    Returns the default device in use by this context, which is typically
    the first element of the devices() list; or a null QCLDevice if the
    context has not been created yet.

    \sa devices()
*/
QCLDevice QCLContext::defaultDevice() const
{
    Q_D(const QCLContext);
    if (d->isCreated) {
        if (!d->defaultDevice.isNull())
            return d->defaultDevice;
        size_t size = 0;
        if (clGetContextInfo(d->id, CL_CONTEXT_DEVICES, 0, 0, &size)
                == CL_SUCCESS && size > 0) {
            QVarLengthArray<cl_device_id> buf(size);
            if (clGetContextInfo(d->id, CL_CONTEXT_DEVICES,
                                 size, buf.data(), 0) == CL_SUCCESS) {
                return QCLDevice(buf[0]);
            }
        }
    }
    return QCLDevice();
}

/*!
    Returns the last OpenCL error that occurred while executing an
    operation on this context or any of the objects created by
    the context.  Returns CL_SUCCESS if the last operation succeeded.

    \sa setLastError(), errorName()
*/
cl_int QCLContext::lastError() const
{
    Q_D(const QCLContext);
    return d->lastError;
}

/*!
    Sets the last error code to \a error.

    \sa lastError(), errorName()
*/
void QCLContext::setLastError(cl_int error)
{
    Q_D(QCLContext);
    d->lastError = error;
}

/*!
    Returns the name of the supplied OpenCL error \a code.  For example,
    \c{CL_SUCCESS}, \c{CL_INVALID_CONTEXT}, etc.

    \sa lastError()
*/
QString QCLContext::errorName(cl_int code)
{
    switch (code) {
#ifdef CL_SUCCESS
    case CL_SUCCESS: return QLatin1String("CL_SUCCESS");
#endif
#ifdef CL_DEVICE_NOT_FOUND
    case CL_DEVICE_NOT_FOUND: return QLatin1String("CL_DEVICE_NOT_FOUND");
#endif
#ifdef CL_DEVICE_NOT_AVAILABLE
    case CL_DEVICE_NOT_AVAILABLE: return QLatin1String("CL_DEVICE_NOT_AVAILABLE");
#endif
#ifdef CL_COMPILER_NOT_AVAILABLE
    case CL_COMPILER_NOT_AVAILABLE: return QLatin1String("CL_COMPILER_NOT_AVAILABLE");
#endif
#ifdef CL_MEM_OBJECT_ALLOCATION_FAILURE
    case CL_MEM_OBJECT_ALLOCATION_FAILURE: return QLatin1String("CL_MEM_OBJECT_ALLOCATION_FAILURE");
#endif
#ifdef CL_OUT_OF_RESOURCES
    case CL_OUT_OF_RESOURCES: return QLatin1String("CL_OUT_OF_RESOURCES");
#endif
#ifdef CL_OUT_OF_HOST_MEMORY
    case CL_OUT_OF_HOST_MEMORY: return QLatin1String("CL_OUT_OF_HOST_MEMORY");
#endif
#ifdef CL_PROFILING_INFO_NOT_AVAILABLE
    case CL_PROFILING_INFO_NOT_AVAILABLE: return QLatin1String("CL_PROFILING_INFO_NOT_AVAILABLE");
#endif
#ifdef CL_MEM_COPY_OVERLAP
    case CL_MEM_COPY_OVERLAP: return QLatin1String("CL_MEM_COPY_OVERLAP");
#endif
#ifdef CL_IMAGE_FORMAT_MISMATCH
    case CL_IMAGE_FORMAT_MISMATCH: return QLatin1String("CL_IMAGE_FORMAT_MISMATCH");
#endif
#ifdef CL_IMAGE_FORMAT_NOT_SUPPORTED
    case CL_IMAGE_FORMAT_NOT_SUPPORTED: return QLatin1String("CL_IMAGE_FORMAT_NOT_SUPPORTED");
#endif
#ifdef CL_BUILD_PROGRAM_FAILURE
    case CL_BUILD_PROGRAM_FAILURE: return QLatin1String("CL_BUILD_PROGRAM_FAILURE");
#endif
#ifdef CL_MAP_FAILURE
    case CL_MAP_FAILURE: return QLatin1String("CL_MAP_FAILURE");
#endif
#ifdef CL_INVALID_VALUE
    case CL_INVALID_VALUE: return QLatin1String("CL_INVALID_VALUE");
#endif
#ifdef CL_INVALID_DEVICE_TYPE
    case CL_INVALID_DEVICE_TYPE: return QLatin1String("CL_INVALID_DEVICE_TYPE");
#endif
#ifdef CL_INVALID_PLATFORM
    case CL_INVALID_PLATFORM: return QLatin1String("CL_INVALID_PLATFORM");
#endif
#ifdef CL_INVALID_DEVICE
    case CL_INVALID_DEVICE: return QLatin1String("CL_INVALID_DEVICE");
#endif
#ifdef CL_INVALID_CONTEXT
    case CL_INVALID_CONTEXT: return QLatin1String("CL_INVALID_CONTEXT");
#endif
#ifdef CL_INVALID_QUEUE_PROPERTIES
    case CL_INVALID_QUEUE_PROPERTIES: return QLatin1String("CL_INVALID_QUEUE_PROPERTIES");
#endif
#ifdef CL_INVALID_COMMAND_QUEUE
    case CL_INVALID_COMMAND_QUEUE: return QLatin1String("CL_INVALID_COMMAND_QUEUE");
#endif
#ifdef CL_INVALID_HOST_PTR
    case CL_INVALID_HOST_PTR: return QLatin1String("CL_INVALID_HOST_PTR");
#endif
#ifdef CL_INVALID_MEM_OBJECT
    case CL_INVALID_MEM_OBJECT: return QLatin1String("CL_INVALID_MEM_OBJECT");
#endif
#ifdef CL_INVALID_IMAGE_FORMAT_DESCRIPTOR
    case CL_INVALID_IMAGE_FORMAT_DESCRIPTOR: return QLatin1String("CL_INVALID_IMAGE_FORMAT_DESCRIPTOR");
#endif
#ifdef CL_INVALID_IMAGE_SIZE
    case CL_INVALID_IMAGE_SIZE: return QLatin1String("CL_INVALID_IMAGE_SIZE");
#endif
#ifdef CL_INVALID_SAMPLER
    case CL_INVALID_SAMPLER: return QLatin1String("CL_INVALID_SAMPLER");
#endif
#ifdef CL_INVALID_BINARY
    case CL_INVALID_BINARY: return QLatin1String("CL_INVALID_BINARY");
#endif
#ifdef CL_INVALID_BUILD_OPTIONS
    case CL_INVALID_BUILD_OPTIONS: return QLatin1String("CL_INVALID_BUILD_OPTIONS");
#endif
#ifdef CL_INVALID_PROGRAM
    case CL_INVALID_PROGRAM: return QLatin1String("CL_INVALID_PROGRAM");
#endif
#ifdef CL_INVALID_PROGRAM_EXECUTABLE
    case CL_INVALID_PROGRAM_EXECUTABLE: return QLatin1String("CL_INVALID_PROGRAM_EXECUTABLE");
#endif
#ifdef CL_INVALID_KERNEL_NAME
    case CL_INVALID_KERNEL_NAME: return QLatin1String("CL_INVALID_KERNEL_NAME");
#endif
#ifdef CL_INVALID_KERNEL_DEFINITION
    case CL_INVALID_KERNEL_DEFINITION: return QLatin1String("CL_INVALID_KERNEL_DEFINITION");
#endif
#ifdef CL_INVALID_KERNEL
    case CL_INVALID_KERNEL: return QLatin1String("CL_INVALID_KERNEL");
#endif
#ifdef CL_INVALID_ARG_INDEX
    case CL_INVALID_ARG_INDEX: return QLatin1String("CL_INVALID_ARG_INDEX");
#endif
#ifdef CL_INVALID_ARG_VALUE
    case CL_INVALID_ARG_VALUE: return QLatin1String("CL_INVALID_ARG_VALUE");
#endif
#ifdef CL_INVALID_ARG_SIZE
    case CL_INVALID_ARG_SIZE: return QLatin1String("CL_INVALID_ARG_SIZE");
#endif
#ifdef CL_INVALID_KERNEL_ARGS
    case CL_INVALID_KERNEL_ARGS: return QLatin1String("CL_INVALID_KERNEL_ARGS");
#endif
#ifdef CL_INVALID_WORK_DIMENSION
    case CL_INVALID_WORK_DIMENSION: return QLatin1String("CL_INVALID_WORK_DIMENSION");
#endif
#ifdef CL_INVALID_WORK_GROUP_SIZE
    case CL_INVALID_WORK_GROUP_SIZE: return QLatin1String("CL_INVALID_WORK_GROUP_SIZE");
#endif
#ifdef CL_INVALID_WORK_ITEM_SIZE
    case CL_INVALID_WORK_ITEM_SIZE: return QLatin1String("CL_INVALID_WORK_ITEM_SIZE");
#endif
#ifdef CL_INVALID_GLOBAL_OFFSET
    case CL_INVALID_GLOBAL_OFFSET: return QLatin1String("CL_INVALID_GLOBAL_OFFSET");
#endif
#ifdef CL_INVALID_EVENT_WAIT_LIST
    case CL_INVALID_EVENT_WAIT_LIST: return QLatin1String("CL_INVALID_EVENT_WAIT_LIST");
#endif
#ifdef CL_INVALID_EVENT
    case CL_INVALID_EVENT: return QLatin1String("CL_INVALID_EVENT");
#endif
#ifdef CL_INVALID_OPERATION
    case CL_INVALID_OPERATION: return QLatin1String("CL_INVALID_OPERATION");
#endif
#ifdef CL_INVALID_GL_OBJECT
    case CL_INVALID_GL_OBJECT: return QLatin1String("CL_INVALID_GL_OBJECT");
#endif
#ifdef CL_INVALID_BUFFER_SIZE
    case CL_INVALID_BUFFER_SIZE: return QLatin1String("CL_INVALID_BUFFER_SIZE");
#endif
#ifdef CL_INVALID_MIP_LEVEL
    case CL_INVALID_MIP_LEVEL: return QLatin1String("CL_INVALID_MIP_LEVEL");
#endif
#ifdef CL_INVALID_GLOBAL_WORK_SIZE
    case CL_INVALID_GLOBAL_WORK_SIZE: return QLatin1String("CL_INVALID_GLOBAL_WORK_SIZE");
#endif

    // OpenCL 1.1
    case CL_MISALIGNED_SUB_BUFFER_OFFSET: return QLatin1String("CL_MISALIGNED_SUB_BUFFER_OFFSET");
    case CL_EXEC_STATUS_ERROR_FOR_EVENTS_IN_WAIT_LIST: return QLatin1String("CL_EXEC_STATUS_ERROR_FOR_EVENTS_IN_WAIT_LIST");

    // OpenCL-OpenGL sharing extension error codes.
    case CL_INVALID_CL_SHAREGROUP_REFERENCE_KHR: return QLatin1String("CL_INVALID_CL_SHAREGROUP_REFERENCE_KHR");

    // cl_khr_icd extension.
    case CL_PLATFORM_NOT_FOUND_KHR: return QLatin1String("CL_PLATFORM_NOT_FOUND_KHR");

    // cl_ext_device_fission extension.
    case CL_DEVICE_PARTITION_FAILED_EXT: return QLatin1String("CL_DEVICE_PARTITION_FAILED_EXT");
    case CL_INVALID_PARTITION_COUNT_EXT: return QLatin1String("CL_INVALID_PARTITION_COUNT_EXT");
    case CL_INVALID_PARTITION_NAME_EXT: return QLatin1String("CL_INVALID_PARTITION_NAME_EXT");

    default: break;
    }
    return QLatin1String("Error ") + QString::number(code);
}

/*!
    Returns the context's active command queue, which will be
    defaultCommandQueue() if the queue has not yet been set.

    \sa setCommandQueue(), defaultCommandQueue()
*/
QCLCommandQueue QCLContext::commandQueue()
{
    Q_D(QCLContext);
    if (!d->commandQueue.isNull())
        return d->commandQueue;
    else
        return defaultCommandQueue();
}

/*!
    Sets the context's active command \a queue.  If \a queue is
    null, then defaultCommandQueue() will be used.

    \sa commandQueue(), defaultCommandQueue()
*/
void QCLContext::setCommandQueue(const QCLCommandQueue &queue)
{
    Q_D(QCLContext);
    d->commandQueue = queue;
}

/*!
    Returns the default command queue for defaultDevice().  If the queue
    has not been created, it will be created with the default properties
    of in-order execution of commands, and profiling disabled.

    Use createCommandQueue() to create a queue that supports
    out-of-order execution or profiling.  For example:

    \code
    QCLCommandQueue queue =
        context.createCommandQueue
            (CL_QUEUE_OUT_OF_ORDER_EXEC_MODE_ENABLE);
    context.setCommandQueue(queue);
    \endcode

    \sa commandQueue(), createCommandQueue(), lastError()
*/
QCLCommandQueue QCLContext::defaultCommandQueue()
{
    Q_D(QCLContext);
    if (d->defaultCommandQueue.isNull()) {
        if (!d->isCreated)
            return QCLCommandQueue();
        QCLDevice dev = defaultDevice();
        if (dev.isNull())
            return QCLCommandQueue();
        cl_command_queue queue;
        cl_int error = CL_INVALID_VALUE;
        queue = clCreateCommandQueue(d->id, dev.deviceId(), 0, &error);
        d->lastError = error;
        if (!queue) {
            qWarning() << "QCLContext::defaultCommandQueue:"
                       << errorName(error);
            return QCLCommandQueue();
        }
        d->defaultCommandQueue = QCLCommandQueue(this, queue);
    }
    return d->defaultCommandQueue;
}

// Returns the active queue handle without incurring retain/release overhead.
cl_command_queue QCLContext::activeQueue()
{
    Q_D(QCLContext);
    cl_command_queue queue = d->commandQueue.queueId();
    if (queue)
        return queue;
    queue = d->defaultCommandQueue.queueId();
    if (queue)
        return queue;
    return defaultCommandQueue().queueId();
}

/*!
    Creates a new command queue on this context for \a device with
    the specified \a properties.  If \a device is null, then
    defaultDevice() will be used instead.

    Unlike defaultCommandQueue(), this function will create a new queue
    every time it is called.  The queue will be deleted when the last
    reference to the returned object is removed.

    \sa defaultCommandQueue(), lastError()
*/
QCLCommandQueue QCLContext::createCommandQueue
    (cl_command_queue_properties properties, const QCLDevice &device)
{
    Q_D(QCLContext);
    cl_command_queue queue;
    cl_int error = CL_INVALID_VALUE;
    if (device.isNull())
        queue = clCreateCommandQueue(d->id, defaultDevice().deviceId(), properties, &error);
    else
        queue = clCreateCommandQueue(d->id, device.deviceId(), properties, &error);
    reportError("QCLContext::createCommandQueue:", error);
    if (queue)
        return QCLCommandQueue(this, queue);
    else
        return QCLCommandQueue();
}

/*!
    Creates an OpenCL memory buffer of \a size bytes in length,
    with the specified \a access mode.

    The memory is created on the device and will not be accessible
    to the host via a direct pointer.  Use createBufferHost() to
    create a host-accessible buffer.

    Returns the new OpenCL memory buffer object, or a null object
    if the buffer could not be created.

    \sa createBufferHost(), createBufferCopy(), createVector()
*/
QCLBuffer QCLContext::createBufferDevice(size_t size, QCLMemoryObject::Access access)
{
    Q_D(QCLContext);
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access);
    cl_mem mem = clCreateBuffer(d->id, flags, size, 0, &error);
    reportError("QCLContext::createBufferDevice:", error);
    if (mem)
        return QCLBuffer(this, mem);
    else
        return QCLBuffer();
}

/*!
    Creates an OpenCL memory buffer of \a size bytes in length,
    with the specified \a access mode.

    If \a data is not null, then it will be used as the storage
    for the buffer.  If \a data is null, then a new block of
    host-accessible memory will be allocated.

    Returns the new OpenCL memory buffer object, or a null object
    if the buffer could not be created.

    \sa createBufferDevice(), createBufferCopy(), createVector()
*/
QCLBuffer QCLContext::createBufferHost
    (void *data, size_t size, QCLMemoryObject::Access access)
{
    Q_D(QCLContext);
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access);
    if (data)
        flags |= CL_MEM_USE_HOST_PTR;
    else
        flags |= CL_MEM_ALLOC_HOST_PTR;
    cl_mem mem = clCreateBuffer(d->id, flags, size, data, &error);
    reportError("QCLContext::createBufferHost:", error);
    if (mem)
        return QCLBuffer(this, mem);
    else
        return QCLBuffer();
}

/*!
    Creates an OpenCL memory buffer of \a size bytes in length,
    with the specified \a access mode.

    The buffer is initialized with a copy of the contents of \a data.
    The application's \a data can be discarded after the buffer
    is created.

    Returns the new OpenCL memory buffer object, or a null object
    if the buffer could not be created.

    \sa createBufferDevice(), createBufferHost(), createVector()
*/
QCLBuffer QCLContext::createBufferCopy
    (const void *data, size_t size, QCLMemoryObject::Access access)
{
    Q_ASSERT(data);
    Q_D(QCLContext);
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access);
    flags |= CL_MEM_COPY_HOST_PTR;
    cl_mem mem = clCreateBuffer
        (d->id, flags, size, const_cast<void *>(data), &error);
    reportError("QCLContext::createBufferCopy:", error);
    if (mem)
        return QCLBuffer(this, mem);
    else
        return QCLBuffer();
}

/*!
    \fn QCLVector<T> QCLContext::createVector(int size, QCLMemoryObject::Access access)

    Creates a host-accessible vector of \a size elements of type T
    on this context and returns it.  The elements will be initially in
    an undefined state.

    Note that the \a access mode indicates how the OpenCL device (e.g. GPU)
    will access the vector.  When the host maps the vector, it will always
    be mapped as ReadWrite.

    \sa createBufferHost()
*/

/*!
    Creates a 2D OpenCL image object with the specified \a format,
    \a size, and \a access mode.

    The image memory is created on the device and will not be accessible
    to the host via a direct pointer.  Use createImage2DHost() to
    create a host-accessible image.

    Returns the new 2D OpenCL image object, or a null object
    if the image could not be created.

    \sa createImage2DHost(), createImage2DCopy()
*/
QCLImage2D QCLContext::createImage2DDevice
    (const QCLImageFormat &format, const QSize &size, QCLMemoryObject::Access access)
{
    Q_D(QCLContext);
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access);
    cl_mem mem = clCreateImage2D
        (d->id, flags, &(format.m_format), size.width(), size.height(), 0,
         0, &error);
    reportError("QCLContext::createImage2DDevice:", error);
    if (mem)
        return QCLImage2D(this, mem, format);
    else
        return QCLImage2D();
}

/*!
    Creates a 2D OpenCL image object with the specified \a format,
    \a size, and \a access mode.  If \a bytesPerLine is not zero,
    it indicates the number of bytes between lines in \a data.

    If \a data is not null, then it will be used as the storage
    for the image.  If \a data is null, then a new block of
    host-accessible memory will be allocated.

    Returns the new 2D OpenCL image object, or a null object
    if the image could not be created.

    \sa createImage2DDevice(), createImage2DCopy()
*/
QCLImage2D QCLContext::createImage2DHost
    (const QCLImageFormat &format, void *data, const QSize &size,
     QCLMemoryObject::Access access, int bytesPerLine)
{
    Q_D(QCLContext);
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access);
    if (data)
        flags |= CL_MEM_USE_HOST_PTR;
    else
        flags |= CL_MEM_ALLOC_HOST_PTR;
    cl_mem mem = clCreateImage2D
        (d->id, flags, &(format.m_format),
         size.width(), size.height(), bytesPerLine,
         data, &error);
    reportError("QCLContext::createImage2DHost:", error);
    if (mem)
        return QCLImage2D(this, mem, format);
    else
        return QCLImage2D();
}

/*!
    Creates a 2D OpenCL image object from \a image with the
    specified \a access mode.

    OpenCL kernels that access the image will read and write
    the QImage contents directly.

    Returns the new 2D OpenCL image object, or a null object
    if the image could not be created.  If \a image is null or
    has a zero size, this function will return a null
    QCLImage2D object.

    \sa createImage2DDevice(), createImage2DCopy()
*/
QCLImage2D QCLContext::createImage2DHost(QImage *image, QCLMemoryObject::Access access)
{
    Q_D(QCLContext);

    // Validate the image.
    if (!image || image->width() < 1 || image->height() < 1)
        return QCLImage2D();
    QCLImageFormat format(image->format());
    if (format.isNull()) {
        qWarning("QCLContext::createImage2DHost: QImage format %d "
                 "does not have an OpenCL equivalent", int(image->format()));
        return QCLImage2D();
    }

    // Create the image object.
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access) | CL_MEM_USE_HOST_PTR;
    cl_mem mem = clCreateImage2D
        (d->id, flags, &(format.m_format),
         image->width(), image->height(), image->bytesPerLine(),
         image->bits(), &error);
    reportError("QCLContext::createImage2DHost:", error);
    if (mem)
        return QCLImage2D(this, mem, format);
    else
        return QCLImage2D();
}

/*!
    Creates a 2D OpenCL image object with the specified \a format,
    \a size, and \a access mode.  If \a bytesPerLine is not zero,
    it indicates the number of bytes between lines in \a data.

    The image is initialized with a copy of the contents of \a data.
    The application's \a data can be discarded after the image
    is created.

    Returns the new 2D OpenCL image object, or a null object
    if the image could not be created.

    \sa createImage2DDevice(), createImage2DHost()
*/
QCLImage2D QCLContext::createImage2DCopy
    (const QCLImageFormat &format, const void *data, const QSize &size,
     QCLMemoryObject::Access access, int bytesPerLine)
{
    Q_D(QCLContext);
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access) | CL_MEM_COPY_HOST_PTR;
    cl_mem mem = clCreateImage2D
        (d->id, flags, &(format.m_format),
         size.width(), size.height(), bytesPerLine,
         const_cast<void *>(data), &error);
    reportError("QCLContext::createImage2DCopy:", error);
    if (mem)
        return QCLImage2D(this, mem);
    else
        return QCLImage2D();
}

/*!
    Creates a 2D OpenCL image object from \a image with the
    specified \a access mode.

    The OpenCL image is initialized with a copy of the contents of
    \a image.  The application's \a image can be discarded after the
    OpenCL image is created.

    Returns the new 2D OpenCL image object, or a null object
    if the image could not be created.  If \a image has a zero size,
    this function will return a null QCLImage2D object.

    \sa createImage2DDevice(), createImage2DHost()
*/
QCLImage2D QCLContext::createImage2DCopy
    (const QImage &image, QCLMemoryObject::Access access)
{
    Q_D(QCLContext);

    // Validate the image.
    if (image.width() < 1 || image.height() < 1)
        return QCLImage2D();
    QCLImageFormat format(image.format());
    if (format.isNull()) {
        qWarning("QCLContext::createImage2DCopy: QImage format %d "
                 "does not have an OpenCL equivalent", int(image.format()));
        return QCLImage2D();
    }

    // Create the image object.
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access) | CL_MEM_COPY_HOST_PTR;
    cl_mem mem = clCreateImage2D
        (d->id, flags, &(format.m_format),
         image.width(), image.height(), image.bytesPerLine(),
         const_cast<uchar *>(image.bits()), &error);
    reportError("QCLContext::createImage2DCopy:", error);
    if (mem)
        return QCLImage2D(this, mem);
    else
        return QCLImage2D();
}

/*!
    Creates a 3D OpenCL image object with the specified \a format,
    \a width, \a height, \a depth, and \a access mode.

    The image memory is created on the device and will not be accessible
    to the host via a direct pointer.  Use createImage3DHost() to
    create a host-accessible image.

    Returns the new 3D OpenCL image object, or a null object
    if the image could not be created.

    \sa createImage3DHost(), createImage3DCopy()
*/
QCLImage3D QCLContext::createImage3DDevice
    (const QCLImageFormat &format, int width, int height, int depth,
     QCLMemoryObject::Access access)
{
    Q_D(QCLContext);
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access);
    cl_mem mem = clCreateImage3D
        (d->id, flags, &(format.m_format), width, height, depth, 0, 0,
         0, &error);
    reportError("QCLContext::createImage3DDevice:", error);
    if (mem)
        return QCLImage3D(this, mem);
    else
        return QCLImage3D();
}

/*!
    Creates a 3D OpenCL image object with the specified \a format,
    \a width, \a height, \a depth, and \a access mode.
    If \a bytesPerLine is not zero, it indicates the number of
    bytes between lines in \a data.  If \a bytesPerSlice is not zero,
    it indicates the number of bytes between slices in \a data.

    If \a data is not null, then it will be used as the storage
    for the image.  If \a data is null, then a new block of
    host-accessible memory will be allocated.

    Returns the new 3D OpenCL image object, or a null object
    if the image could not be created.

    \sa createImage3DDevice(), createImage3DCopy()
*/
QCLImage3D QCLContext::createImage3DHost
    (const QCLImageFormat &format, void *data,
     int width, int height, int depth, QCLMemoryObject::Access access,
     int bytesPerLine, int bytesPerSlice)
{
    Q_D(QCLContext);
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access);
    if (data)
        flags |= CL_MEM_USE_HOST_PTR;
    else
        flags |= CL_MEM_ALLOC_HOST_PTR;
    cl_mem mem = clCreateImage3D
        (d->id, flags, &(format.m_format),
         width, height, depth, bytesPerLine, bytesPerSlice,
         data, &error);
    reportError("QCLContext::createImage3DHost:", error);
    if (mem)
        return QCLImage3D(this, mem);
    else
        return QCLImage3D();
}

/*!
    Creates a 3D OpenCL image object with the specified \a format,
    \a width, \a height, \a depth, and \a access mode.
    If \a bytesPerLine is not zero, it indicates the number of bytes
    between lines in \a data.  If \a bytesPerSlice is not zero,
    it indicates the number of bytes between slices in \a data.

    The image is initialized with a copy of the contents of \a data.
    The application's \a data can be discarded after the image
    is created.

    Returns the new 3D OpenCL image object, or a null object
    if the image could not be created.

    \sa createImage3DDevice(), createImage3DHost()
*/
QCLImage3D QCLContext::createImage3DCopy
    (const QCLImageFormat &format, const void *data,
     int width, int height, int depth,
     QCLMemoryObject::Access access, int bytesPerLine, int bytesPerSlice)
{
    Q_D(QCLContext);
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access) | CL_MEM_COPY_HOST_PTR;
    cl_mem mem = clCreateImage3D
        (d->id, flags, &(format.m_format),
         width, height, depth, bytesPerLine, bytesPerSlice,
         const_cast<void *>(data), &error);
    reportError("QCLContext::createImage3DCopy:", error);
    if (mem)
        return QCLImage3D(this, mem);
    else
        return QCLImage3D();
}

/*!
    Creates an OpenCL program object from the supplied \a sourceCode.

    \sa createProgramFromSourceFile(), buildProgramFromSourceCode()
*/
QCLProgram QCLContext::createProgramFromSourceCode(const QByteArray &sourceCode)
{
    Q_D(QCLContext);
    Q_ASSERT(!sourceCode.isEmpty());
    cl_int error = CL_INVALID_CONTEXT;
    const char *code = sourceCode.constData();
    size_t length = sourceCode.size();
    cl_program prog = clCreateProgramWithSource
        (d->id, 1, &code, &length, &error);
    reportError("QCLContext::createProgramFromSourceCode:", error);
    if (prog)
        return QCLProgram(this, prog);
    else
        return QCLProgram();
}

/*!
    Creates an OpenCL program object from the contents of the specified
    \a fileName.

    \sa createProgramFromSourceCode(), buildProgramFromSourceFile()
*/
QCLProgram QCLContext::createProgramFromSourceFile(const QString &fileName)
{
    QFile file(fileName);
    if (!file.open(QFile::ReadOnly)) {
        qWarning() << "QCLContext::createProgramFromSourceFile: Unable to open file" << fileName;
        return QCLProgram();
    }
    qint64 size = file.size();
    uchar *data;
    if (size > 0 && size <= 0x7fffffff && (data = file.map(0, size)) != 0) {
        QByteArray array = QByteArray::fromRawData
            (reinterpret_cast<char *>(data), int(size));
        QCLProgram program = createProgramFromSourceCode(array);
        file.unmap(data);
        return program;
    }
    QByteArray contents = file.readAll();
    return createProgramFromSourceCode(contents.constData());
}

/*!
    Creates an OpenCL program object from \a binary for defaultDevice().

    This function can only load the binary for a single device.  For multiple
    devices, use createProgramFromBinaries() instead.

    \sa createProgramFromBinaryFile(), createProgramFromBinaries()
*/
QCLProgram QCLContext::createProgramFromBinaryCode(const QByteArray &binary)
{
    Q_D(QCLContext);
    Q_ASSERT(!binary.isEmpty());
    cl_int error = CL_INVALID_CONTEXT;
    const uchar *code = reinterpret_cast<const uchar *>(binary.constData());
    size_t length = binary.size();
    cl_device_id device = defaultDevice().deviceId();
    cl_program prog = clCreateProgramWithBinary
        (d->id, 1, &device, &length, &code, 0, &error);
    reportError("QCLContext::createProgramFromBinaryCode:", error);
    if (prog)
        return QCLProgram(this, prog);
    else
        return QCLProgram();
}

/*!
    Creates an OpenCL program object from the binary data in \a fileName
    for defaultDevice().

    \sa createProgramFromBinaryCode(), createProgramFromBinaries()
*/
QCLProgram QCLContext::createProgramFromBinaryFile(const QString &fileName)
{
    QFile file(fileName);
    if (!file.open(QFile::ReadOnly)) {
        qWarning() << "QCLContext::createProgramFromBinaryFile: Unable to open file" << fileName;
        return QCLProgram();
    }
    qint64 size = file.size();
    uchar *data;
    if (size > 0 && size <= 0x7fffffff && (data = file.map(0, size)) != 0) {
        QByteArray array = QByteArray::fromRawData
            (reinterpret_cast<char *>(data), int(size));
        QCLProgram program = createProgramFromBinaryCode(array);
        file.unmap(data);
        return program;
    }
    QByteArray contents = file.readAll();
    return createProgramFromBinaryCode(contents.constData());
}

/*!
    Creates an OpenCL program object from the list of \a binaries
    for \a devices.  The \a binaries and \a devices lists must have
    the same number of elements.

    \sa createProgramFromBinaryCode(), createProgramFromBinaryFile()
*/
QCLProgram QCLContext::createProgramFromBinaries
    (const QList<QCLDevice> &devices, const QList<QByteArray> &binaries)
{
    Q_D(QCLContext);
    if (devices.size() != binaries.size() || devices.isEmpty()) {
        reportError("QCLContext::createProgramFromBinaries:", CL_INVALID_VALUE);
        return QCLProgram();
    }
    QVarLengthArray<cl_device_id> devs;
    QVarLengthArray<const uchar *> bins;
    QVarLengthArray<size_t> lens;
    for (int index = 0; index < devices.size(); ++index) {
        devs.append(devices.at(index).deviceId());
        bins.append(reinterpret_cast<const uchar *>
            (binaries.at(index).constData()));
        lens.append(binaries.at(index).size());
    }
    cl_int error = CL_INVALID_CONTEXT;
    cl_program prog = clCreateProgramWithBinary
        (d->id, devs.size(), devs.data(), lens.data(), bins.data(), 0, &error);
    reportError("QCLContext::createProgramFromBinaries:", error);
    if (prog)
        return QCLProgram(this, prog);
    else
        return QCLProgram();
}

/*!
    Creates an OpenCL program object from the supplied \a sourceCode
    and then builds it.  Returns a null QCLProgram if the program
    could not be built.

    \sa createProgramFromSourceCode(), buildProgramFromSourceFile()
*/
QCLProgram QCLContext::buildProgramFromSourceCode(const QByteArray &sourceCode)
{
    QCLProgram program = createProgramFromSourceCode(sourceCode);
    if (program.isNull() || program.build())
        return program;
    return QCLProgram();
}

/*!
    Creates an OpenCL program object from the contents of the supplied
    \a fileName and then builds it.  Returns a null QCLProgram if the
    program could not be built.

    \sa createProgramFromSourceFile(), buildProgramFromSourceCode()
*/
QCLProgram QCLContext::buildProgramFromSourceFile(const QString &fileName)
{
    QCLProgram program = createProgramFromSourceFile(fileName);
    if (program.isNull() || program.build())
        return program;
    return QCLProgram();
}

/*!
    Creates an OpenCL program object from the supplied \a binary
    for defaultDevice() and then builds it.  Returns a null QCLProgram
    if the program could not be built.

    This function can only load the binary for a single device.  For multiple
    devices, use createProgramFromBinaries() instead.

    \sa createProgramFromBinaryCode(), buildProgramFromBinaryFile()
    \sa buildProgramFromBinaries()
*/
QCLProgram QCLContext::buildProgramFromBinaryCode(const QByteArray &binary)
{
    QCLProgram program = createProgramFromBinaryCode(binary);
    if (program.isNull() || program.build())
        return program;
    return QCLProgram();
}

/*!
    Creates an OpenCL program object from the binary contents of the supplied
    \a fileName for defaultDevice() and then builds it.  Returns a null
    QCLProgram if the program could not be built.

    \sa createProgramFromBinaryFile(), buildProgramFromBinaryCode()
*/
QCLProgram QCLContext::buildProgramFromBinaryFile(const QString &fileName)
{
    QCLProgram program = createProgramFromBinaryFile(fileName);
    if (program.isNull() || program.build())
        return program;
    return QCLProgram();
}

/*!
    Creates an OpenCL program object from the list of \a binaries for
    \a devices and then builds the program.  Returns a null QCLProgram if
    the program could not be built.  The \a binaries and \a devices lists
    must have the same number of elements.

    \sa createProgramFromBinaries(), buildProgramFromBinaryCode()
*/
QCLProgram QCLContext::buildProgramFromBinaries
    (const QList<QCLDevice> &devices, const QList<QByteArray> &binaries)
{
    QCLProgram program = createProgramFromBinaries(devices, binaries);
    if (program.isNull() || program.build())
        return program;
    return QCLProgram();
}

static QList<QCLImageFormat> qt_cl_supportedImageFormats
    (cl_context ctx, cl_mem_flags flags, cl_mem_object_type image_type)
{
    cl_uint count = 0;
    QList<QCLImageFormat> list;
    if (clGetSupportedImageFormats
            (ctx, flags, image_type,
             0, 0, &count) != CL_SUCCESS || !count)
        return list;
    QVarLengthArray<cl_image_format> buf(count);
    if (clGetSupportedImageFormats
            (ctx, flags, image_type,
             count, buf.data(), 0) != CL_SUCCESS)
        return list;
    for (cl_uint index = 0; index < count; ++index) {
        list.append(QCLImageFormat
            (QCLImageFormat::ChannelOrder(buf[index].image_channel_order),
             QCLImageFormat::ChannelType(buf[index].image_channel_data_type)));
    }
    return list;
}

/*!
    Returns the list of supported 2D image formats for processing
    images with the specified memory \a flags.

    \sa supportedImage3DFormats()
*/
QList<QCLImageFormat> QCLContext::supportedImage2DFormats
    (cl_mem_flags flags) const
{
    Q_D(const QCLContext);
    return qt_cl_supportedImageFormats(d->id, flags, CL_MEM_OBJECT_IMAGE2D);
}

/*!
    Returns the list of supported 3D image formats for processing
    images with the specified memory \a flags.

    \sa supportedImage2DFormats()
*/
QList<QCLImageFormat> QCLContext::supportedImage3DFormats
    (cl_mem_flags flags) const
{
    Q_D(const QCLContext);
    return qt_cl_supportedImageFormats(d->id, flags, CL_MEM_OBJECT_IMAGE3D);
}

/*!
    Creates a sampler for this context from the arguments
    \a normalizedCoordinates, \a addressingMode, and \a filterMode.
*/
QCLSampler QCLContext::createSampler
    (bool normalizedCoordinates, QCLSampler::AddressingMode addressingMode,
     QCLSampler::FilterMode filterMode)
{
    Q_D(QCLContext);
    cl_int error;
    cl_sampler sampler = clCreateSampler
        (d->id, normalizedCoordinates ? CL_TRUE : CL_FALSE,
         cl_addressing_mode(addressingMode),
         cl_filter_mode(filterMode), &error);
    reportError("QCLContext::createSampler:", error);
    if (sampler)
        return QCLSampler(this, sampler);
    else
        return QCLSampler();
}

/*!
    Creates a user event.  Returns null if user events are not
    supported.

    User events are a feature of OpenCL 1.1 which allows an application
    to insert a marker into the command queue.  Commands that depend
    upon the marker will not be executed until the application triggers
    the user event with QCLUserEvent::setFinished().
*/
QCLUserEvent QCLContext::createUserEvent()
{
#ifdef QT_OPENCL_1_1
    Q_D(QCLContext);
    cl_int error = CL_INVALID_CONTEXT;
    cl_event event = clCreateUserEvent(d->id, &error);
    reportError("QCLContext::createUserEvent:", error);
    return QCLUserEvent(event, true);
#else
    return QCLUserEvent();
#endif
}

/*!
    Flushes all previously queued commands to the device associated
    with the active command queue.  The commands are delivered to
    the device, but no guarantees are given that they will be executed.

    \sa finish()
*/
void QCLContext::flush()
{
    clFlush(activeQueue());
}

/*!
    Blocks until all previously queued commands on the active
    command queue have finished execution.

    \sa flush()
*/
void QCLContext::finish()
{
    clFinish(activeQueue());
}

/*!
    Returns a marker event for the active command queue.  The event
    will be signalled when all commands that were queued before this
    point have finished.

    \sa barrier(), sync()
*/
QCLEvent QCLContext::marker()
{
    cl_event evid;
    cl_int error = clEnqueueMarker(activeQueue(), &evid);
    reportError("QCLContext::marker:", error);
    if (error != CL_SUCCESS)
        return QCLEvent();
    else
        return QCLEvent(evid);
}

/*!
    Synchronizes the host against the active command queue.
    This function will block until all currently queued commands
    have finished execution.
*/
void QCLContext::sync()
{
    cl_event event;
    cl_int error = clEnqueueMarker(activeQueue(), &event);
    reportError("QCLContext::sync:", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
    }
}

/*!
    Adds a barrier to the active command queue.  All commands that
    were queued before this point must finish before any further
    commands added after this point are executed.

    This function will return immediately and will not block waiting
    for the commands to finish.  Use sync() to block until all queued
    commands finish.

    \sa marker(), sync()
*/
void QCLContext::barrier()
{
    cl_int error = clEnqueueBarrier(activeQueue());
    reportError("QCLContext::barrier:", error);
}

/*!
    \overload

    Adds a barrier to the active command queue that will prevent future
    commands from being executed until after all members of \a events
    have been signalled.

    \sa marker()
*/
void QCLContext::barrier(const QCLEventList &events)
{
    if (events.isEmpty())
        return;
    cl_int error = clEnqueueWaitForEvents
        (activeQueue(), events.size(), events.eventData());
    reportError("QCLContext::barrier(QCLEventList):", error);
}

/*!
    \internal

    Used by QCLContextGL::create() to set the default device found
    by querying CL_CURRENT_DEVICE_FOR_GL_CONTEXT_KHR.
*/
void QCLContext::setDefaultDevice(const QCLDevice &device)
{
    Q_D(QCLContext);
    d->defaultDevice = device;
}

/*!
    \internal
*/
void QCLContext::reportError(const char *name, cl_int error)
{
    Q_D(QCLContext);
    d->lastError = error;
    if (error != CL_SUCCESS)
        qWarning() << name << errorName(error);
}

QT_END_NAMESPACE
