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

#ifndef QCLIMAGE_H
#define QCLIMAGE_H

#include "qclmemoryobject.h"
#include "qclimageformat.h"
#include "qclevent.h"
#include <QtCore/qrect.h>

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(CL)

class QCLImage2DPrivate;
class QCLImage3D;
class QCLBuffer;
class QPainter;

class Q_CL_EXPORT QCLImage2D : public QCLMemoryObject
{
public:
    QCLImage2D() : d_ptr(0) {}
    QCLImage2D(QCLContext *context, cl_mem id)
        : QCLMemoryObject(context, id), d_ptr(0) {}
    QCLImage2D(const QCLImage2D &other);
    ~QCLImage2D();

    QCLImage2D &operator=(const QCLImage2D &other);

    QCLImageFormat format() const;

    int width() const;
    int height() const;

    int bytesPerElement() const;
    int bytesPerLine() const;

    bool read(void *data, const QRect &rect, int bytesPerLine = 0);
    bool read(QImage *image, const QRect &rect = QRect());
    QCLEvent readAsync(void *data, const QRect &rect,
                       const QCLEventList &after = QCLEventList(),
                       int bytesPerLine = 0);

    bool write(const void *data, const QRect &rect, int bytesPerLine = 0);
    bool write(const QImage &image, const QRect &rect = QRect());
    QCLEvent writeAsync
        (const void *data, const QRect &rect,
         const QCLEventList &after = QCLEventList(),
         int bytesPerLine = 0);

    bool copyTo(const QRect &rect, const QCLImage2D &dest,
                const QPoint &destOffset);
    bool copyTo(const QRect &rect, const QCLImage3D &dest,
                const size_t destOffset[3]);
    bool copyTo(const QRect &rect, const QCLBuffer &dest,
                size_t destOffset);
    QCLEvent copyToAsync
        (const QRect &rect, const QCLImage2D &dest, const QPoint &destOffset,
         const QCLEventList &after = QCLEventList());
    QCLEvent copyToAsync
        (const QRect &rect, const QCLImage3D &dest, const size_t destOffset[3],
         const QCLEventList &after = QCLEventList());
    QCLEvent copyToAsync
        (const QRect &rect, const QCLBuffer &dest, size_t destOffset,
         const QCLEventList &after = QCLEventList());

    void *map(const QRect &rect, QCLMemoryObject::Access access,
              int *bytesPerLine = 0);
    QCLEvent mapAsync(void **ptr, const QRect &rect,
                      QCLMemoryObject::Access access,
                      const QCLEventList &after = QCLEventList(),
                      int *bytesPerLine = 0);

    QImage toQImage(bool cached = true);

    void drawImage(QPainter *painter, const QPoint &point,
                   const QRect &subRect = QRect(),
                   Qt::ImageConversionFlags flags = Qt::AutoColor);
    void drawImage(QPainter *painter, const QRect &targetRect,
                   const QRect &subRect = QRect(),
                   Qt::ImageConversionFlags flags = Qt::AutoColor);

private:
    mutable QCLImage2DPrivate *d_ptr;

    Q_DECLARE_PRIVATE(QCLImage2D)

    QCLImage2D(QCLContext *context, cl_mem id, const QCLImageFormat& format);

    friend class QCLContext;
};

class Q_CL_EXPORT QCLImage3D : public QCLMemoryObject
{
public:
    QCLImage3D() {}
    QCLImage3D(QCLContext *context, cl_mem id)
        : QCLMemoryObject(context, id) {}
    QCLImage3D(const QCLImage3D &other)
        : QCLMemoryObject() { setId(other.context(), other.memoryId()); }

    QCLImage3D &operator=(const QCLImage3D &other)
    {
        setId(other.context(), other.memoryId());
        return *this;
    }

    QCLImageFormat format() const;

    int width() const;
    int height() const;
    int depth() const;

    int bytesPerElement() const;
    int bytesPerLine() const;
    int bytesPerSlice() const;

    bool read(void *data, const size_t origin[3], const size_t size[3],
              int bytesPerLine = 0, int bytesPerSlice = 0);
    QCLEvent readAsync
        (void *data, const size_t origin[3], const size_t size[3],
         const QCLEventList &after = QCLEventList(),
         int bytesPerLine = 0, int bytesPerSlice = 0);

    bool write(const void *data, const size_t origin[3], const size_t size[3],
               int bytesPerLine = 0, int bytesPerSlice = 0);
    QCLEvent writeAsync
        (const void *data, const size_t origin[3], const size_t size[3],
         const QCLEventList &after = QCLEventList(),
         int bytesPerLine = 0, int bytesPerSlice = 0);

    bool copyTo(const size_t origin[3], const size_t size[3],
                const QCLImage3D &dest, const size_t destOffset[3]);
    bool copyTo(const size_t origin[3], const QSize &size,
                const QCLImage2D &dest, const QPoint &destOffset);
    bool copyTo(const size_t origin[3], const size_t size[3],
                const QCLBuffer &dest, size_t destOffset);
    QCLEvent copyToAsync
        (const size_t origin[3], const size_t size[3],
         const QCLImage3D &dest, const size_t destOffset[3],
         const QCLEventList &after = QCLEventList());
    QCLEvent copyToAsync
        (const size_t origin[3], const QSize &size,
         const QCLImage2D &dest, const QPoint &destOffset,
         const QCLEventList &after = QCLEventList());
    QCLEvent copyToAsync
        (const size_t origin[3], const size_t size[3],
         const QCLBuffer &dest, size_t destOffset,
         const QCLEventList &after = QCLEventList());

    void *map(const size_t origin[3], const size_t size[3],
              QCLMemoryObject::Access access,
              int *bytesPerLine = 0, int *bytesPerSlice = 0);
    QCLEvent mapAsync
        (void **ptr, const size_t origin[3], const size_t size[3],
         QCLMemoryObject::Access access,
         const QCLEventList &after = QCLEventList(),
         int *bytesPerLine = 0, int *bytesPerSlice = 0);
};

inline void QCLImage2D::drawImage
    (QPainter *painter, const QPoint &point,
     const QRect &subRect, Qt::ImageConversionFlags flags)
{
    drawImage(painter, QRect(point.x(), point.y(), -1, -1), subRect, flags);
}

QT_END_NAMESPACE

QT_END_HEADER

#endif
