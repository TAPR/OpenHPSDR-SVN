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

#include "qclvector.h"
#include "qclcontext.h"
#include <QtCore/qatomic.h>

QT_BEGIN_NAMESPACE

/*!
    \class QCLVector
    \brief The QCLVector class represents a typed OpenCL buffer object.
    \since 4.7
    \ingroup opencl

    QCLVector is a convenience template class that wraps an
    OpenCL buffer object to make it appear as a host-accessible array
    of elements of type T.

    Whenever the host CPU calls operator[](), the array's contents
    are copied into host-accessible memory for direct access.  When the
    host sets the vector on a QCLKernel as an argument, the data is
    copied back to the OpenCL compute device (e.g., the GPU).

    The type T is restricted to primitive and movable types that do
    not require explicit construction, destruction, or operator=().
    T can have constructors, but they will not be called.  This is
    because the data will be copied to the OpenCL device in a different
    address space, and the OpenCL device will not know how to construct,
    destruct, or copy the data in a C++-compatible manner.

    Types such as float, char, int, QPointF, and QVector3D can be
    used as the type T, but types such as QString cannot.
*/

#if defined(__APPLE__) || defined(__MACOSX)
#define QT_CL_COPY_VECTOR 1
#endif

enum QCLVectorState
{
    State_Uninitialized,    // Buffer contains uninitialized contents.
    State_InHost,           // Data is currently in the host.
    State_InKernel          // Data is currently in a kernel.
};

class QCLVectorBasePrivate
{
public:
    QCLVectorBasePrivate()
        : state(State_Uninitialized)
        , context(0)
        , id(0)
        , hostCopy(0)
    {
        ref = 1;
    }

    QBasicAtomicInt ref;
    QCLVectorState state;
    QCLContext *context;
    cl_mem id;
    void *hostCopy;
    QList<QCLVectorBase *> owners;

    void *hostPointer(size_t size)
    {
        if (!hostCopy)
            hostCopy = qMalloc(size);
        return hostCopy;
    }
};

QCLVectorBase::QCLVectorBase(size_t elemSize)
    : d_ptr(0)
    , m_elemSize(elemSize)
    , m_size(0)
    , m_mapped(0)
{
}

QCLVectorBase::QCLVectorBase(size_t elemSize, const QCLVectorBase &other)
    : d_ptr(other.d_ptr)
    , m_elemSize(elemSize)
    , m_size(other.m_size)
    , m_mapped(other.m_mapped)
{
    if (d_ptr) {
        d_ptr->ref.ref();
        d_ptr->owners.append(this);
    }
}

QCLVectorBase::~QCLVectorBase()
{
    release();
}

void QCLVectorBase::assign(const QCLVectorBase &other)
{
    if (d_ptr == other.d_ptr)
        return;
    release();
    d_ptr = other.d_ptr;
    m_size = other.m_size;
    m_mapped = other.m_mapped;
    if (d_ptr) {
        d_ptr->ref.ref();
        d_ptr->owners.append(this);
    }
}

void QCLVectorBase::create
    (QCLContext *context, int size, QCLMemoryObject::Access access)
{
    release();
    d_ptr = new QCLVectorBasePrivate();
    Q_CHECK_PTR(d_ptr);
    d_ptr->owners.append(this);
    cl_int error;
    cl_mem id = clCreateBuffer
        (context->contextId(),
#ifndef QT_CL_COPY_VECTOR
            cl_mem_flags(access) | CL_MEM_ALLOC_HOST_PTR,
#else
            cl_mem_flags(access),
#endif
         size * m_elemSize, 0, &error);
    context->reportError("QCLVector<T>::create:", error);
    if (id) {
        d_ptr->context = context;
        d_ptr->id = id;
        d_ptr->state = State_Uninitialized;
        d_ptr->hostCopy = 0;
        m_size = size;
    }
}

void QCLVectorBase::release()
{
    if (!d_ptr)
        return;
    if (d_ptr->ref.deref()) {
        d_ptr = 0;
        m_size = 0;
        m_mapped = 0;
        return;
    }
#ifndef QT_CL_COPY_VECTOR
    unmap();
#else
    // No need to write back if we will discard the contents anyway.
    m_mapped = 0;
#endif
    if (d_ptr->id) {
        clReleaseMemObject(d_ptr->id);
        d_ptr->id = 0;
    }
    d_ptr->context = 0;
    d_ptr->state = State_Uninitialized;
    m_size = 0;
    if (d_ptr->hostCopy) {
        qFree(d_ptr->hostCopy);
        d_ptr->hostCopy = 0;
    }
    delete d_ptr;
    d_ptr = 0;
}

void QCLVectorBase::map()
{
    // Bail out if no buffer, or already mapped.
    if (!d_ptr || !d_ptr->id || m_mapped)
        return;

#ifndef QT_CL_COPY_VECTOR
    cl_int error;
    m_mapped = clEnqueueMapBuffer
        (d_ptr->context->activeQueue(), d_ptr->id,
         CL_TRUE, CL_MAP_READ | CL_MAP_WRITE,
         0, m_size * m_elemSize, 0, 0, 0, &error);
    d_ptr->context->reportError("QCLVector<T>::map:", error);
#else
    // We cannot map the buffer directly, so do an explicit read-back.
    // We skip the read-back if the buffer was not recently in a kernel.
    void *hostPtr = d_ptr->hostPointer(m_size * m_elemSize);
    if (d_ptr->state == State_InKernel) {
        cl_int error = clEnqueueReadBuffer
            (d_ptr->context->activeQueue(), d_ptr->id, CL_TRUE,
             0, m_size * m_elemSize, hostPtr, 0, 0, 0);
        d_ptr->context->reportError("QCLVector<T>::map(read):", error);
        if (error == CL_SUCCESS)
            m_mapped = hostPtr;
    } else {
        m_mapped = hostPtr;
    }
    d_ptr->state = State_InHost;
#endif

    // Update all of the other owners with the map state.
    if (d_ptr->owners.size() > 1) {
        QList<QCLVectorBase *>::Iterator it;
        for (it = d_ptr->owners.begin(); it != d_ptr->owners.end(); ++it) {
            if (*it != this)
                (*it)->m_mapped = m_mapped;
        }
    }
}

void QCLVectorBase::unmap() const
{
    if (m_mapped) {
#ifndef QT_CL_COPY_VECTOR
        cl_int error = clEnqueueUnmapMemObject
            (d_ptr->context->activeQueue(), d_ptr->id, m_mapped, 0, 0, 0);
        d_ptr->context->reportError("QCLVector<T>::unmap:", error);
#else
        // Write the local copy back to the OpenCL device.
        if (d_ptr->hostCopy && d_ptr->state == State_InHost) {
            cl_int error = clEnqueueWriteBuffer
                (d_ptr->context->activeQueue(), d_ptr->id, CL_FALSE,
                 0, m_size * m_elemSize, d_ptr->hostCopy, 0, 0, 0);
            d_ptr->context->reportError("QCLVector<T>::unmap(write):", error);
        }
        d_ptr->state = State_InKernel;
#endif
        m_mapped = 0;

        // Update all of the other owners with the unmap state.
        if (d_ptr->owners.size() > 1) {
            QList<QCLVectorBase *>::Iterator it;
            for (it = d_ptr->owners.begin(); it != d_ptr->owners.end(); ++it) {
                if (*it != this)
                    (*it)->m_mapped = 0;
            }
        }
    }
}

void QCLVectorBase::read(void *data, int count, int offset)
{
    if (count <= 0)
        return;
    if (m_mapped) {
        qMemCopy(data, reinterpret_cast<uchar *>(m_mapped) + offset, count);
    } else if (d_ptr && d_ptr->id) {
        cl_int error = clEnqueueReadBuffer
            (d_ptr->context->activeQueue(), d_ptr->id, CL_TRUE,
             offset, count, data, 0, 0, 0);
        d_ptr->context->reportError("QCLVector<T>::read:", error);
        d_ptr->state = State_InKernel;
    }
}

void QCLVectorBase::write(const void *data, int count, int offset)
{
    if (count <= 0)
        return;
    if (m_mapped) {
        qMemCopy(reinterpret_cast<uchar *>(m_mapped) + offset, data, count);
    } else if (d_ptr && d_ptr->id) {
        cl_int error = clEnqueueWriteBuffer
            (d_ptr->context->activeQueue(), d_ptr->id, CL_TRUE,
             offset, count, data, 0, 0, 0);
        d_ptr->context->reportError("QCLVector<T>::write:", error);
        d_ptr->state = State_InKernel;
    }
}

cl_mem QCLVectorBase::memoryId() const
{
    return d_ptr ? d_ptr->id : 0;
}

QCLContext *QCLVectorBase::context() const
{
    return d_ptr ? d_ptr->context : 0;
}

cl_mem QCLVectorBase::kernelArg() const
{
    if (d_ptr) {
        unmap();
        d_ptr->state = State_InKernel;
        return d_ptr->id;
    } else {
        return 0;
    }
}

/*!
    \fn QCLVector::QCLVector()

    Creates a null OpenCL vector.

    \sa isNull()
*/

/*!
    \fn QCLVector::QCLVector(const QCLVector<T> &other)

    Creates a copy of the \a other vector reference.  The vector's
    contents are not duplicated: modifications to this vector
    will also affect \a other.

    \sa operator=()
*/

/*!
    \fn QCLVector::~QCLVector()

    Destroys this vector reference.  If this is the last reference
    to the underlying data, the vector will be unmapped and deallocated.
*/

/*!
    \fn QCLVector<T> QCLVector::operator=(const QCLVector<T> &other)

    Assigns the \a other vector reference to this object.
    The vector's contents are not duplicated: modifications to
    this vector will also affect \a other.
*/

/*!
    \fn bool QCLVector::isNull() const

    Returns true if this vector is null; false otherwise.
*/

/*!
    \fn void QCLVector::release()

    Releases the contents of this OpenCL vector.  If not explicitly
    released, the contents will be implicitly released when the
    vector is destroyed.
*/

/*!
    \fn bool QCLVector::isEmpty() const

    Returns true if this OpenCL vector is empty; false otherwise.
*/

/*!
    \fn int QCLVector::size() const

    Returns the number of elements of type T in this OpenCL vector.
*/

/*!
    \fn T &QCLVector::operator[](int index)

    Returns a reference to the element at \a index in this OpenCL vector.
    The vector will be copied to host memory if necessary.
*/

/*!
    \fn const T &QCLVector::operator[](int index) const

    Returns a const reference to the element at \a index in this
    OpenCL vector.  The vector will be copied to host memory
    if necessary.
*/

/*!
    \fn void QCLVector::read(T *data, int count, int offset)

    Reads the \a count elements starting \a offset in this vector
    into \a data.

    \sa write()
*/

/*!
    \fn void QCLVector::write(const T *data, int count, int offset)

    Writes the \a count elements from \a data to \a offset in this vector.

    \sa read()
*/

/*!
    \fn void QCLVector::write(const QVector<T> &data, int offset)
    \overload

    Writes the contents of \a data to \a offset in this vector.
*/

/*!
    \fn QCLContext *QCLVector::context() const

    Returns the OpenCL context that was used to create this vector.
*/

/*!
    \fn QCLBuffer QCLVector::toBuffer() const

    Returns the OpenCL buffer handle for this vector.
*/

QT_END_NAMESPACE
