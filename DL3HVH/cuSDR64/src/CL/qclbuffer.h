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

#ifndef QCLBUFFER_H
#define QCLBUFFER_H

#include "qclmemoryobject.h"
#include "qclevent.h"
#include <QtCore/qrect.h>

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(CL)

class QCLImage2D;
class QCLImage3D;

class Q_CL_EXPORT QCLBuffer : public QCLMemoryObject
{
public:
    QCLBuffer() {}
    QCLBuffer(QCLContext *context, cl_mem id)
        : QCLMemoryObject(context, id) {}
    QCLBuffer(const QCLBuffer &other)
        : QCLMemoryObject() { setId(other.context(), other.memoryId()); }

    QCLBuffer &operator=(const QCLBuffer &other)
    {
        setId(other.context(), other.memoryId());
        return *this;
    }

    bool read(void *data, size_t size);
    bool read(size_t offset, void *data, size_t size);
    QCLEvent readAsync(size_t offset, void *data, size_t size,
                       const QCLEventList &after = QCLEventList());

    bool readRect(const QRect &rect, void *data,
                  size_t bufferBytesPerLine, size_t hostBytesPerLine);
    bool readRect(const size_t origin[3], const size_t size[3], void *data,
                  size_t bufferBytesPerLine, size_t bufferBytesPerSlice,
                  size_t hostBytesPerLine, size_t hostBytesPerSlice);
    QCLEvent readRectAsync
            (const QRect &rect, void *data,
             size_t bufferBytesPerLine, size_t hostBytesPerLine,
             const QCLEventList &after = QCLEventList());
    QCLEvent readRectAsync
            (const size_t origin[3], const size_t size[3], void *data,
             size_t bufferBytesPerLine, size_t bufferBytesPerSlice,
             size_t hostBytesPerLine, size_t hostBytesPerSlice,
             const QCLEventList &after = QCLEventList());

    bool write(const void *data, size_t size);
    bool write(size_t offset, const void *data, size_t size);
    QCLEvent writeAsync(size_t offset, const void *data, size_t size,
                        const QCLEventList &after = QCLEventList());

    bool writeRect(const QRect &rect, const void *data,
                   size_t bufferBytesPerLine, size_t hostBytesPerLine);
    bool writeRect(const size_t origin[3], const size_t size[3],
                   const void *data, size_t bufferBytesPerLine,
                   size_t bufferBytesPerSlice, size_t hostBytesPerLine,
                   size_t hostBytesPerSlice);
    QCLEvent writeRectAsync
            (const QRect &rect, const void *data,
             size_t bufferBytesPerLine, size_t hostBytesPerLine,
             const QCLEventList &after = QCLEventList());
    QCLEvent writeRectAsync
            (const size_t origin[3], const size_t size[3], const void *data,
             size_t bufferBytesPerLine, size_t bufferBytesPerSlice,
             size_t hostBytesPerLine, size_t hostBytesPerSlice,
             const QCLEventList &after = QCLEventList());

    bool copyTo(size_t offset, size_t size,
                const QCLBuffer &dest, size_t destOffset);
    bool copyTo(size_t offset, const QCLImage2D &dest, const QRect &rect);
    bool copyTo(size_t offset, const QCLImage3D &dest,
                const size_t origin[3], const size_t size[3]);

    QCLEvent copyToAsync
        (size_t offset, size_t size,
         const QCLBuffer &dest, size_t destOffset,
         const QCLEventList &after = QCLEventList());
    QCLEvent copyToAsync
        (size_t offset, const QCLImage2D &dest, const QRect &rect,
         const QCLEventList &after = QCLEventList());
    QCLEvent copyToAsync
        (size_t offset, const QCLImage3D &dest,
         const size_t origin[3], const size_t size[3],
         const QCLEventList &after = QCLEventList());

    bool copyToRect(const QRect &rect, const QCLBuffer &dest,
                    const QPoint &destPoint, size_t bufferBytesPerLine,
                    size_t destBytesPerLine);
    bool copyToRect(const size_t origin[3], const size_t size[3],
                    const QCLBuffer &dest, const size_t destOrigin[3],
                    size_t bufferBytesPerLine, size_t bufferBytesPerSlice,
                    size_t destBytesPerLine, size_t destBytesPerSlice);
    QCLEvent copyToRectAsync
            (const QRect &rect, const QCLBuffer &dest, const QPoint &destPoint,
             size_t bufferBytesPerLine, size_t destBytesPerLine,
             const QCLEventList &after = QCLEventList());
    QCLEvent copyToRectAsync
            (const size_t origin[3], const size_t size[3],
             const QCLBuffer &dest, const size_t destOrigin[3],
             size_t bufferBytesPerLine, size_t bufferBytesPerSlice,
             size_t destBytesPerLine, size_t destBytesPerSlice,
             const QCLEventList &after = QCLEventList());

    void *map(size_t offset, size_t size, QCLMemoryObject::Access access);
    void *map(QCLMemoryObject::Access access);
    QCLEvent mapAsync(void **ptr, size_t offset, size_t size,
                      QCLMemoryObject::Access access,
                      const QCLEventList &after = QCLEventList());

    QCLBuffer createSubBuffer
        (size_t offset, size_t size, QCLMemoryObject::Access access);

    QCLBuffer parentBuffer() const;
    size_t offset() const;
};

QT_END_NAMESPACE

QT_END_HEADER

#endif
