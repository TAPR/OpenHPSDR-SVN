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

#ifndef QCLVECTOR_H
#define QCLVECTOR_H

#include "qclbuffer.h"
#include <QtCore/qscopedpointer.h>
#include <QtCore/qvector.h>

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(CL)

class QCLContext;
class QCLKernel;
class QCLVectorBasePrivate;

class Q_CL_EXPORT QCLVectorBase
{
protected:
    QCLVectorBase(size_t elemSize);
    QCLVectorBase(size_t elemSize, const QCLVectorBase &other);
    ~QCLVectorBase();

    QCLVectorBasePrivate *d_ptr;
    size_t m_elemSize;
    size_t m_size;
    mutable void *m_mapped;

    void assign(const QCLVectorBase &other);

    void create(QCLContext *context, int size, QCLMemoryObject::Access access);
    void release();

    void map();
    void unmap() const;

    void read(void *data, int count, int offset);
    void write(const void *data, int count, int offset);

    cl_mem memoryId() const;
    QCLContext *context() const;

    cl_mem kernelArg() const;

    friend class QCLKernel;
};

template <typename T>
class QCLVector : public QCLVectorBase
{
public:
    QCLVector();
    QCLVector(const QCLVector<T> &other);
    ~QCLVector();

    QCLVector<T> &operator=(const QCLVector<T> &other);

    bool isNull() const;

    void release();

    inline bool isEmpty() const { return m_size == 0; }
    inline int size() const { return m_size; }

    T &operator[](int index);
    const T &operator[](int index) const;

    void read(T *data, int count, int offset = 0);
    void write(const T *data, int count, int offset = 0);
    void write(const QVector<T> &data, int offset = 0);

    QCLContext *context() const;
    QCLBuffer toBuffer() const;

private:
    QCLVector(QCLContext *context, int size, QCLMemoryObject::Access access);

    friend class QCLContext;
};

template <typename T>
Q_INLINE_TEMPLATE QCLVector<T>::QCLVector()
    : QCLVectorBase(sizeof(T)) {}

template <typename T>
Q_INLINE_TEMPLATE QCLVector<T>::QCLVector
        (QCLContext *context, int size, QCLMemoryObject::Access access)
    : QCLVectorBase(sizeof(T))
{
    QCLVectorBase::create(context, size, access);
}

template <typename T>
Q_INLINE_TEMPLATE QCLVector<T>::QCLVector(const QCLVector<T> &other)
    : QCLVectorBase(sizeof(T), other)
{
}

template <typename T>
Q_INLINE_TEMPLATE QCLVector<T>::~QCLVector() {}

template <typename T>
QCLVector<T> &QCLVector<T>::operator=(const QCLVector<T> &other)
{
    assign(other);
    return *this;
}

template <typename T>
Q_INLINE_TEMPLATE bool QCLVector<T>::isNull() const
{
    return d_ptr == 0;
}

template <typename T>
Q_INLINE_TEMPLATE void QCLVector<T>::release()
{
    QCLVectorBase::release();
}

template <typename T>
Q_INLINE_TEMPLATE T &QCLVector<T>::operator[](int index)
{
    Q_ASSERT_X(index >= 0 && index < int(m_size), "QCLVector<T>::operator[]",
               "index out of range");
    if (!m_mapped)
        map();
    return (reinterpret_cast<T *>(m_mapped))[index];
}

template <typename T>
Q_INLINE_TEMPLATE const T &QCLVector<T>::operator[](int index) const
{
    Q_ASSERT_X(index >= 0 && index < int(m_size), "QCLVector<T>::operator[]",
               "index out of range");
    if (!m_mapped)
        const_cast<QCLVector<T> *>(this)->map();
    return (reinterpret_cast<T *>(m_mapped))[index];
}

template <typename T>
Q_INLINE_TEMPLATE void QCLVector<T>::write
    (const T *data, int count, int offset)
{
    Q_ASSERT(count >= 0 && offset >= 0 && (offset + count) <= int(m_size));
    QCLVectorBase::write(data, count * sizeof(T), offset * sizeof(T));
}

template <typename T>
Q_INLINE_TEMPLATE void QCLVector<T>::read
    (T *data, int count, int offset)
{
    Q_ASSERT(count >= 0 && offset >= 0 && (offset + count) <= int(m_size));
    QCLVectorBase::read(data, count * sizeof(T), offset * sizeof(T));
}

template <typename T>
Q_INLINE_TEMPLATE void QCLVector<T>::write
    (const QVector<T> &data, int offset)
{
    write(data.constData(), data.size(), offset);
}

template <typename T>
Q_INLINE_TEMPLATE QCLContext *QCLVector<T>::context() const
{
    return QCLVectorBase::context();
}

template <typename T>
Q_INLINE_TEMPLATE QCLBuffer QCLVector<T>::toBuffer() const
{
    cl_mem id = QCLVectorBase::memoryId();
    if (id) {
        clRetainMemObject(id);
        return QCLBuffer(context(), id);
    } else {
        return QCLBuffer();
    }
}

QT_END_NAMESPACE

QT_END_HEADER

#endif
