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

#include "qclkernel.h"
#include "qclprogram.h"
#include "qclbuffer.h"
#include "qclcontext.h"
#include "qclext_p.h"
#include <QtCore/qvarlengtharray.h>
#include <QtCore/qpoint.h>
#include <QtGui/qvector2d.h>
#include <QtGui/qvector3d.h>
#include <QtGui/qvector4d.h>
#include <QtGui/qmatrix4x4.h>
#include <QtGui/qcolor.h>

QT_BEGIN_NAMESPACE

/*!
    \class QCLKernel
    \brief The QCLKernel class represents an executable entry point function in an OpenCL program.
    \since 4.7
    \ingroup opencl

    \section1 Executing kernels

    QCLKernel corresponds to an instance of an OpenCL kernel, decorated
    with a specific globalWorkSize() and localWorkSize().  It is possible
    to use the same OpenCL kernel with different work sizes by altering
    the size for each execution request:

    \code
    QCLKernel kernel = program.createKernel("foo");

    kernel.setGlobalWorkSize(100, 100);
    kernel.setArg(0, a1);
    kernel.setArg(1, b1);
    kernel.run();

    kernel.setGlobalWorkSize(200, 200);
    kernel.setArg(0, a2);
    kernel.setArg(1, b2);
    kernel.run();
    \endcode

    Alternatively, operator()() can be used to avoid the setArg() calls:

    \code
    QCLKernel kernel = program.createKernel("foo");

    kernel.setGlobalWorkSize(100, 100);
    kernel(a1, b1);

    kernel.setGlobalWorkSize(200, 200);
    kernel(a2, b2);
    \endcode

    Up to 10 arguments can be provided to operator()().  Use explicit
    setArg() and run() calls with kernels that have more than
    10 arguments.

    The following types are handled specially via setArg() and operator()():
    \c cl_int, \c cl_uint, \c cl_long, \c cl_ulong, \c float,
    QVector2D, QVector3D, QVector4D, QPointF, QPoint, QMatrix4x4,
    QCLBuffer, QCLImage2D, QCLImage3D, QCLVector, and QCLSampler.
    Other argument types must be set explicitly by calling the
    setArg() override that takes a buffer and size.

    \section1 Asynchronous execution

    Note that both run() and operator()() return immediately;
    they will not block until execution is finished.  Both functions
    return a QCLEvent object that can be used to wait for the
    request to finish:

    \code
    kernel.setGlobalWorkSize(100, 100);
    QCLEvent event = kernel(a1, b1);
    event.waitForFinished();
    \endcode

    Usually it isn't necessary for an explicit QCLEvent wait
    because the next OpenCL request will implicitly block until
    the kernel finishes execution:

    \code
    QCLBuffer buffer = ...;

    kernel.setGlobalWorkSize(100, 100);
    kernel(buffer);

    buffer.read(...);
    \endcode

    With the default in-order command execution policy, OpenCL will ensure
    that the QCLBuffer::read() request will not begin execution until the
    kernel execution finishes.

    \section1 Kernels and QtConcurrent

    QtConcurrent::run() can also be used to execute a kernel:

    \code
    kernel.setGlobalWorkSize(100, 100);
    QFuture<void> future = QtConcurrent::run(kernel, a1, b1);
    future.waitForFinished();
    \endcode

    This will create a background thread on the main CPU to enqueue
    the kernel for execution and to wait for the kernel to complete.

    Only 5 arguments can be passed to a kernel using
    QtConcurrent::run(), which is the same as for regular functions
    and QtConcurrent.  Use explicit setArg() calls and runInThread()
    for kernels with more than 5 arguments.

    Because kernels do not have return values, QtConcurrent::run()
    on a QCLKernel will always return a QFuture<void>.

    The main advantage of QFuture<void> compared to QCLEvent is
    that it can be used with QFutureWatcher to receive signal
    notification of when a kernel completes execution:

    \code
    QFutureWatcher<void> *watcher = new QFutureWatcher<void>(this);
    watcher->setFuture(QtConcurrent::run(kernel, a1, b1));
    connect(watcher, SIGNAL(finished()), this, SLOT(eventFinished()));
    \endcode

    QCLEvent objects can be implicitly converted into a QFuture<void>,
    so any function that returns a QCLEvent can be used with QFutureWatcher.

    \sa QCLProgram, {OpenCL and QtConcurrent}
*/

class QCLKernelPrivate
{
public:
    QCLKernelPrivate(QCLContext *ctx, cl_kernel kid)
        : context(ctx)
        , id(kid)
        , globalWorkSize(1)
        , localWorkSize(0)
    {}
    QCLKernelPrivate(const QCLKernelPrivate *other)
        : context(other->context)
        , id(other->id)
        , globalWorkSize(other->globalWorkSize)
        , localWorkSize(other->localWorkSize)
    {
        if (id)
            clRetainKernel(id);
    }
    ~QCLKernelPrivate()
    {
        if (id)
            clReleaseKernel(id);
    }

    void copy(const QCLKernelPrivate *other)
    {
        context = other->context;
        globalWorkSize = other->globalWorkSize;
        localWorkSize = other->localWorkSize;
        if (id != other->id) {
            if (id)
                clReleaseKernel(id);
            id = other->id;
            if (id)
                clRetainKernel(id);
        }
    }

    QCLContext *context;
    cl_kernel id;
    QCLWorkSize globalWorkSize;
    QCLWorkSize localWorkSize;
};

/*!
    Constructs a null OpenCL kernel object.
*/
QCLKernel::QCLKernel()
    : d_ptr(new QCLKernelPrivate(0, 0)), m_kernelId(0)
{
}

/*!
    Constructs an OpenCL kernel object from the native identifier \a id,
    and associates it with \a context.  This class will take over
    ownership of \a id and release it in the destructor.
*/
QCLKernel::QCLKernel(QCLContext *context, cl_kernel id)
    : d_ptr(new QCLKernelPrivate(context, id)), m_kernelId(id)
{
}

/*!
    Constructs a copy of \a other.
*/
QCLKernel::QCLKernel(const QCLKernel &other)
    : d_ptr(new QCLKernelPrivate(other.d_ptr.data()))
    , m_kernelId(other.m_kernelId)
{
}

/*!
    Releases this OpenCL kernel object.  If this is the last
    reference to the kernel, it will be destroyed.
*/
QCLKernel::~QCLKernel()
{
}

/*!
    Assigns \a other to this object.
*/
QCLKernel &QCLKernel::operator=(const QCLKernel &other)
{
    d_ptr->copy(other.d_ptr.data());
    m_kernelId = other.m_kernelId;
    return *this;
}

/*!
    Returns true if this OpenCL kernel object is null; false otherwise.
*/
bool QCLKernel::isNull() const
{
    Q_D(const QCLKernel);
    return d->id == 0;
}

/*!
    Returns true if this OpenCL kernel object has the same
    identifier as \a other; false otherwise.

    Note: this operator checks for equality solely on kernelId().
    The two objects may have different global and local work sizes.

    \sa operator!=(), kernelId()
*/
bool QCLKernel::operator==(const QCLKernel &other) const
{
    return d_ptr->id == other.d_ptr->id;
}

/*!
    Returns true if this OpenCL kernel object does not have the same
    identifier as \a other; false otherwise.

    \sa operator==(), kernelId()
*/
bool QCLKernel::operator!=(const QCLKernel &other) const
{
    return d_ptr->id != other.d_ptr->id;
}

/*!
    Returns the native OpenCL identifier for this kernel.
*/
cl_kernel QCLKernel::kernelId() const
{
    Q_D(const QCLKernel);
    return d->id;
}

/*!
    Returns the OpenCL context that this kernel was created within.
*/
QCLContext *QCLKernel::context() const
{
    Q_D(const QCLKernel);
    return d->context;
}

/*!
    Returns the OpenCL program that this kernel is associated with.
*/
QCLProgram QCLKernel::program() const
{
    Q_D(const QCLKernel);
    if (!d->id)
        return QCLProgram();
    cl_program prog = 0;
    if (clGetKernelInfo(d->id, CL_KERNEL_PROGRAM,
                        sizeof(prog), &prog, 0) != CL_SUCCESS)
        return QCLProgram();
    return QCLProgram(d->context, prog);
}

/*!
    Returns the name of this OpenCL kernel's entry point function.
*/
QString QCLKernel::name() const
{
    Q_D(const QCLKernel);
    size_t size = 0;
    if (clGetKernelInfo(d->id, CL_KERNEL_FUNCTION_NAME,
                        0, 0, &size) != CL_SUCCESS || !size)
        return QString();
    QVarLengthArray<char> buf(size);
    if (clGetKernelInfo(d->id, CL_KERNEL_FUNCTION_NAME,
                        size, buf.data(), 0) != CL_SUCCESS)
        return QString();
    return QString::fromLatin1(buf.constData(), size);
}

/*!
    Returns the number of arguments that are expected by
    this OpenCL kernel.

    \sa setArg()
*/
int QCLKernel::argCount() const
{
    Q_D(const QCLKernel);
    cl_uint count = 0;
    if (clGetKernelInfo(d->id, CL_KERNEL_NUM_ARGS, sizeof(count), &count, 0)
            != CL_SUCCESS)
        return 0;
    return int(count);
}

/*!
    Returns the work group size that was declared in the kernel's
    source code using a \c{reqd_work_group_size} qualifier.
    Returns (0, 0, 0) if the size is not declared.

    The default device for context() is used to retrieve the
    work group size.
*/
QCLWorkSize QCLKernel::declaredWorkGroupSize() const
{
    Q_D(const QCLKernel);
    size_t sizes[3];
    if (clGetKernelWorkGroupInfo
            (d->id, d->context->defaultDevice().deviceId(),
             CL_KERNEL_COMPILE_WORK_GROUP_SIZE,
             sizeof(sizes), sizes, 0) != CL_SUCCESS)
        return QCLWorkSize(0, 0, 0);
    else
        return QCLWorkSize(sizes[0], sizes[1], sizes[2]);
}

/*!
    \overload

    Returns the work group size that was declared in the kernel's
    source code using a \c{reqd_work_group_size} qualifier.
    Returns (0, 0, 0) if the size is not declared.

    The specified \a device is used to retrieve the work group size.
*/
QCLWorkSize QCLKernel::declaredWorkGroupSize(const QCLDevice &device) const
{
    Q_D(const QCLKernel);
    size_t sizes[3];
    if (clGetKernelWorkGroupInfo
            (d->id, device.deviceId(),
             CL_KERNEL_COMPILE_WORK_GROUP_SIZE,
             sizeof(sizes), sizes, 0) != CL_SUCCESS)
        return QCLWorkSize(0, 0, 0);
    else
        return QCLWorkSize(sizes[0], sizes[1], sizes[2]);
}

/*!
    Returns the global work size for this instance of the kernel.
    The default value is 1.

    \sa setGlobalWorkSize(), localWorkSize()
*/
QCLWorkSize QCLKernel::globalWorkSize() const
{
    Q_D(const QCLKernel);
    return d->globalWorkSize;
}

/*!
    Sets the global work size for this instance of the kernel to \a size.

    \sa globalWorkSize(), setLocalWorkSize(), setRoundedGlobalWorkSize()
*/
void QCLKernel::setGlobalWorkSize(const QCLWorkSize &size)
{
    Q_D(QCLKernel);
    d->globalWorkSize = size;
}

/*!
    \fn void QCLKernel::setGlobalWorkSize(size_t width, size_t height)
    \overload

    Sets the global work size for this instance of the kernel to
    \a width x \a height.
*/

/*!
    \fn void QCLKernel::setGlobalWorkSize(size_t width, size_t height, size_t depth)
    \overload

    Sets the global work size for this instance of the kernel to
    \a width x \a height x \a depth.
*/

/*!
    \fn void QCLKernel::setRoundedGlobalWorkSize(const QCLWorkSize &size)

    Sets the global work size for this instance of the kernel to \a size,
    after rounding it up to the next multiple of localWorkSize().

    \sa globalWorkSize(), QCLWorkSize::roundTo()
*/

/*!
    \fn void QCLKernel::setRoundedGlobalWorkSize(size_t width, size_t height)
    \overload

    Sets the global work size for this instance of the kernel to
    \a width x \a height, after rounding it up to the next multiple
    of localWorkSize().
*/

/*!
    \fn void QCLKernel::setRoundedGlobalWorkSize(size_t width, size_t height, size_t depth)
    \overload

    Sets the global work size for this instance of the kernel to
    \a width x \a height x \a depth, after rounding it up to the
    next multiple of localWorkSize().
*/

/*!
    \fn void QCLKernel::setLocalWorkSize(size_t width, size_t height)
    \overload

    Sets the local work size for this instance of the kernel to
    \a width x \a height.
*/

/*!
    \fn void QCLKernel::setLocalWorkSize(size_t width, size_t height, size_t depth)
    \overload

    Sets the local work size for this instance of the kernel to
    \a width x \a height x \a depth.
*/

/*!
    Returns the local work size for this instance of the kernel.
    The default value is 0, which indicates that the local
    work size is not used.

    \sa setLocalWorkSize(), globalWorkSize()
*/
QCLWorkSize QCLKernel::localWorkSize() const
{
    Q_D(const QCLKernel);
    return d->localWorkSize;
}

/*!
    Sets the local work size for this instance of the kernel to \a size.

    \sa localWorkSize(), setGlobalWorkSize()
*/
void QCLKernel::setLocalWorkSize(const QCLWorkSize &size)
{
    Q_D(QCLKernel);
    d->localWorkSize = size;
}

/*!
    Returns the recommended best local work size for 2D image processing
    on this kernel.  Default value is 8x8 unless the maximum work size
    is not large enough to accomodate 8x8 items.

    \sa bestLocalWorkSizeImage3D()
*/
QCLWorkSize QCLKernel::bestLocalWorkSizeImage2D() const
{
    QList<QCLDevice> devices = program().devices();
    size_t maxItems = devices.isEmpty() ? 1 : devices.at(0). maximumWorkItemsPerGroup();
    size_t size = 8;
    while (size > 1 && (size * size) > maxItems)
        size /= 2;
    return QCLWorkSize(size, size);
}

/*!
    Returns the recommended best local work size for 3D image processing
    on this kernel.  Default value is 8x8x8 unless the maximum work size
    is not large enough to accomodate 8x8x8 items.

    \sa bestLocalWorkSizeImage2D()
*/
QCLWorkSize QCLKernel::bestLocalWorkSizeImage3D() const
{
    QList<QCLDevice> devices = program().devices();
    size_t maxItems = devices.isEmpty() ? 1 : devices.at(0). maximumWorkItemsPerGroup();
    size_t size = 8;
    while (size > 1 && (size * size * size) > maxItems)
        size /= 2;
    return QCLWorkSize(size, size, size);
}

/*!
    Returns the preferred work group size multiple, which is a
    performance hint for the local work group size on OpenCL 1.1
    systems.  Returns zero if the system is OpenCL 1.0, or a
    preferred work group size multiple is not available.
*/
size_t QCLKernel::preferredWorkSizeMultiple() const
{
    Q_D(const QCLKernel);
    size_t size;
    if (clGetKernelWorkGroupInfo
            (d->id, d->context->defaultDevice().deviceId(),
             CL_KERNEL_PREFERRED_WORK_GROUP_SIZE_MULTIPLE,
             sizeof(size), &size, 0) != CL_SUCCESS)
        return 0;
    else
        return size;
}

/*!
    \fn void QCLKernel::setArg(int index, cl_int value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c int.
*/

/*!
    \fn void QCLKernel::setArg(int index, cl_uint value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c uint.
*/

/*!
    \fn void QCLKernel::setArg(int index, cl_long value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c long.
*/

/*!
    \fn void QCLKernel::setArg(int index, cl_ulong value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c ulong.
*/

/*!
    \fn void QCLKernel::setArg(int index, float value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c float.
*/

/*!
    \fn void QCLKernel::setArg(int index, const QVector2D &value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c float2.
*/

/*!
    \fn void QCLKernel::setArg(int index, const QVector3D &value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c float4 (OpenCL does not have a \c float3 type).
    The value will be passed to the kernel as (x, y, z, 1).
*/

/*!
    \fn void QCLKernel::setArg(int index, const QVector4D &value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c float4.
*/

/*!
    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c float4 as is passed the red, green, blue, and
    alpha components of \a value as floating-point values
    between 0 and 1.
*/
void QCLKernel::setArg(int index, const QColor &value)
{
    float values[4] =
        {value.redF(), value.greenF(), value.blueF(), value.alphaF()};
    clSetKernelArg(m_kernelId, index, sizeof(values), values);
}

/*!
    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c float4 as is passed the red, green, blue, and
    alpha components of \a value as floating-point values
    between 0 and 1.
*/
void QCLKernel::setArg(int index, Qt::GlobalColor value)
{
    QColor color(value);
    float values[4] =
        {color.redF(), color.greenF(), color.blueF(), color.alphaF()};
    clSetKernelArg(m_kernelId, index, sizeof(values), values);
}

/*!
    \fn void QCLKernel::setArg(int index, const QPoint &value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c int2.
*/

/*!
    \fn void QCLKernel::setArg(int index, const QPointF &value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c float2.
*/

/*!
    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c float16.
*/
void QCLKernel::setArg(int index, const QMatrix4x4 &value)
{
    if (sizeof(qreal) == sizeof(float)) {
        clSetKernelArg(m_kernelId, index, sizeof(float) * 16, value.constData());
    } else {
        float values[16];
        for (int posn = 0; posn < 16; ++posn)
            values[posn] = float(value.constData()[posn]);
        clSetKernelArg(m_kernelId, index, sizeof(values), values);
    }
}

/*!
    \fn void QCLKernel::setArg(int index, const QCLMemoryObject &value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c image2d_t, \c image3d_t, or be a pointer to a buffer,
    according to the type of memory object represented by \a value.
*/

/*!
    \fn void QCLKernel::setArg(int index, const QCLVector<T> &value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared as a pointer
    to a buffer.
*/

/*!
    \fn void QCLKernel::setArg(int index, const QCLSampler &value)

    Sets argument \a index for this kernel to \a value.

    The argument is assumed to have been declared with the
    type \c sampler_t.
*/

/*!
    \fn void QCLKernel::setArg(int index, const void *data, size_t size)

    Sets argument \a index to the \a size bytes at \a data.
*/

/*!
    Requests that this kernel instance be run on globalWorkSize() items,
    optionally subdivided into work groups of localWorkSize() items.

    Returns an event object that can be used to wait for the kernel
    to finish execution.  The request is executed on the active
    command queue for context().

    \sa operator()(), runInThread()
*/
QCLEvent QCLKernel::run()
{
    Q_D(const QCLKernel);
    cl_event event;
    cl_int error = clEnqueueNDRangeKernel
        (d->context->activeQueue(), m_kernelId, d->globalWorkSize.dimensions(),
         0, d->globalWorkSize.sizes(),
         (d->localWorkSize.width() ? d->localWorkSize.sizes() : 0),
         0, 0, &event);
    d->context->reportError("QCLKernel::run:", error);
    if (error != CL_SUCCESS)
        return QCLEvent();
    else
        return QCLEvent(event);
}

/*!
    \overload

    Requests that this kernel instance be run on globalWorkSize() items,
    optionally subdivided into work groups of localWorkSize() items.

    If \a after is not an empty list, it indicates the events that must
    be signaled as finished before this kernel instance can begin executing.

    Returns an event object that can be used to wait for the kernel
    to finish execution.  The request is executed on the active
    command queue for context().
*/
QCLEvent QCLKernel::run(const QCLEventList &after)
{
    Q_D(const QCLKernel);
    cl_event event;
    cl_int error = clEnqueueNDRangeKernel
        (d->context->activeQueue(), m_kernelId, d->globalWorkSize.dimensions(),
         0, d->globalWorkSize.sizes(),
         (d->localWorkSize.width() ? d->localWorkSize.sizes() : 0),
         after.size(), after.eventData(), &event);
    d->context->reportError("QCLKernel::run:", error);
    if (error != CL_SUCCESS)
        return QCLEvent();
    else
        return QCLEvent(event);
}

#ifndef QT_NO_CONCURRENT

static void qt_run_kernel
    (cl_kernel kernel, cl_command_queue queue,
     const QCLWorkSize &globalWorkSize, const QCLWorkSize &localWorkSize)
{
    cl_event event;
    cl_int error = clEnqueueNDRangeKernel
        (queue, kernel, globalWorkSize.dimensions(), 0, globalWorkSize.sizes(),
         (localWorkSize.width() ? localWorkSize.sizes() : 0),
         0, 0, &event);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
    } else {
        qWarning() << "QCLKernel::runInThread:" << QCLContext::errorName(error);
    }
    clReleaseKernel(kernel);
    clReleaseCommandQueue(queue);
}

/*!
    Requests that this kernel instance be run on globalWorkSize() items,
    optionally subdivided into work groups of localWorkSize() items.
    The kernel will be enqueued and executed in a background thread.

    Returns a QFuture object that can be used to wait for the kernel
    to finish execution.  The request is executed on the active
    command queue for context().

    Usually runInThread() is called implicitly via QtConcurrent::run():

    \code
    kernel.setGlobalWorkSize(100, 100);
    QFuture<void> future = QtConcurrent::run(kernel, a1, b1);
    future.waitForFinished();
    \endcode

    The kernel object must not be reused until the background
    thread finishes execution of the kernel.  Thus, the following
    code will have unexpected effects:

    \code
    QFuture<void> future1 = QtConcurrent::run(kernel, a1, b1);
    QFuture<void> future2 = QtConcurrent::run(kernel, a2, b2);
    future1.waitForFinished();
    future2.waitForFinished();
    \endcode

    The recommended method to run the same kernel multiple times in a
    background thread is as follows:

    \code
    void runKernelTwice(QCLKernel &kernel)
    {
        kernel(a1, b1);
        kernel(a2, b2).waitForFinished();
    }

    QFuture<void> future = QtConcurrent::run(runKernelTwice, kernel);
    \endcode

    \sa run()
*/
QFuture<void> QCLKernel::runInThread()
{
    Q_D(const QCLKernel);
    cl_kernel kernel = m_kernelId;
    cl_command_queue queue = d->context->activeQueue();
    if (!kernel || !queue)
        return QFuture<void>();
    clRetainKernel(kernel);
    clRetainCommandQueue(queue);
    return QtConcurrent::run
        (qt_run_kernel, kernel, queue, d->globalWorkSize, d->localWorkSize);
}

#endif

/*!
    \fn QCLEvent QCLKernel::operator()()

    Runs this kernel instance with zero arguments.
    Returns an event object that can be used to wait for the
    kernel to finish execution.
*/

/*!
    \fn QCLEvent QCLKernel::operator()(const T1 &arg1)

    Runs this kernel instance with the argument \a arg1.
    Returns an event object that can be used to wait for the
    kernel to finish execution.
*/

/*!
    \fn QCLEvent QCLKernel::operator()(const T1 &arg1, const T2 &arg2)

    Runs this kernel instance with the arguments \a arg1 and \a arg2.
    Returns an event object that can be used to wait for the
    kernel to finish execution.
*/

/*!
    \fn QCLEvent QCLKernel::operator()(const T1 &arg1, const T2 &arg2, const T3 &arg3)

    Runs this kernel instance with the arguments \a arg1, \a arg2,
    and \a arg3.  Returns an event object that can be used to wait for the
    kernel to finish execution.
*/

/*!
    \fn QCLEvent QCLKernel::operator()(const T1 &arg1, const T2 &arg2, const T3 &arg3, const T4 &arg4)

    Runs this kernel instance with the arguments \a arg1, \a arg2,
    \a arg3, and \a arg4.  Returns an event object that can be used to
    wait for the kernel to finish execution.
*/

/*!
    \fn QCLEvent QCLKernel::operator()(const T1 &arg1, const T2 &arg2, const T3 &arg3, const T4 &arg4, const T5 &arg5)

    Runs this kernel instance with the arguments \a arg1, \a arg2,
    \a arg3, \a arg4, and \a arg5.  Returns an event object that can be
    used to wait for the kernel to finish execution.
*/

/*!
    \fn QCLEvent QCLKernel::operator()(const T1 &arg1, const T2 &arg2, const T3 &arg3, const T4 &arg4, const T5 &arg5, const T6 &arg6)

    Runs this kernel instance with the arguments \a arg1, \a arg2,
    \a arg3, \a arg4, \a arg5, and \a arg6.  Returns an event object that
    can be used to wait for the kernel to finish execution.
*/

/*!
    \fn QCLEvent QCLKernel::operator()(const T1 &arg1, const T2 &arg2, const T3 &arg3, const T4 &arg4, const T5 &arg5, const T6 &arg6, const T7 &arg7)

    Runs this kernel instance with the arguments \a arg1, \a arg2,
    \a arg3, \a arg4, \a arg5, \a arg6, and \a arg7.  Returns an event
    object that can be used to wait for the kernel to finish execution.
*/

/*!
    \fn QCLEvent QCLKernel::operator()(const T1 &arg1, const T2 &arg2, const T3 &arg3, const T4 &arg4, const T5 &arg5, const T6 &arg6, const T7 &arg7, const T8 &arg8)

    Runs this kernel instance with the arguments \a arg1, \a arg2,
    \a arg3, \a arg4, \a arg5, \a arg6, \a arg7, and \a arg8.  Returns
    an event object that can be used to wait for the kernel to finish
    execution.
*/

/*!
    \fn QCLEvent QCLKernel::operator()(const T1 &arg1, const T2 &arg2, const T3 &arg3, const T4 &arg4, const T5 &arg5, const T6 &arg6, const T7 &arg7, const T8 &arg8, const T9 &arg9)

    Runs this kernel instance with the arguments \a arg1, \a arg2,
    \a arg3, \a arg4, \a arg5, \a arg6, \a arg7, \a arg8, and \a arg9.
    Returns an event object that can be used to wait for the kernel
    to finish execution.
*/

/*!
    \fn QCLEvent QCLKernel::operator()(const T1 &arg1, const T2 &arg2, const T3 &arg3, const T4 &arg4, const T5 &arg5, const T6 &arg6, const T7 &arg7, const T8 &arg8, const T9 &arg9, const T10 &arg10)

    Runs this kernel instance with the arguments \a arg1, \a arg2,
    \a arg3, \a arg4, \a arg5, \a arg6, \a arg7, \a arg8, \a arg9,
    and \a arg10.  Returns an event object that can be used to wait
    for the kernel to finish execution.
*/

QT_END_NAMESPACE
