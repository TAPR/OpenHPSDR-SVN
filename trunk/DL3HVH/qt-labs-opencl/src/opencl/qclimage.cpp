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

#include "qclimage.h"
#include "qclbuffer.h"
#include "qclcontext.h"
#include <QtGui/qpainter.h>
#include <QtGui/qpaintdevice.h>
#ifdef QT_BUILD_INTERNAL
#include <QtGui/private/qpixmap_raster_p.h>
#endif
#include <QtCore/qdebug.h>

QT_BEGIN_NAMESPACE

/*!
    \class QCLImage2D
    \brief The QCLImage2D class represents a 2D OpenCL image object.
    \since 4.7
    \ingroup opencl
*/

class QCLImage2DPrivate
{
public:
    QCLImage2DPrivate() {}
    QCLImage2DPrivate(const QCLImage2DPrivate *other)
        : format(other->format)
        , cachedImage(other->cachedImage)
    {
    }

    void assign(const QCLImage2DPrivate *other)
    {
        format = other->format;
        cachedImage = other->cachedImage;
    }

    QCLImageFormat format;
    QImage cachedImage;
};

/*!
    \fn QCLImage2D::QCLImage2D()

    Constructs a null 2D OpenCL image object.
*/

/*!
    \fn QCLImage2D::QCLImage2D(QCLContext *context, cl_mem id)

    Constructs a 2D OpenCL image object that is initialized with the
    native OpenCL identifier \a id, and associates it with \a context.
    This class will take over ownership of \a id and will release
    it in the destructor.
*/

/*!
    \internal
*/
QCLImage2D::QCLImage2D(QCLContext *context, cl_mem id,
                       const QCLImageFormat& format)
    : QCLMemoryObject(context, id), d_ptr(new QCLImage2DPrivate())
{
    d_ptr->format = format;
}

/*!
    Constructs a copy of \a other.
*/
QCLImage2D::QCLImage2D(const QCLImage2D &other)
    : QCLMemoryObject()
    , d_ptr(other.d_ptr ? new QCLImage2DPrivate(other.d_ptr) : 0)
{
    setId(other.context(), other.memoryId());
}

/*!
    Destroys this 2D OpenCL image.
*/
QCLImage2D::~QCLImage2D()
{
    delete d_ptr;
}

/*!
    Assigns \a other to this object.
*/
QCLImage2D &QCLImage2D::operator=(const QCLImage2D &other)
{
    if (this != &other) {
        setId(other.context(), other.memoryId());
        if (!d_ptr && other.d_ptr) {
            d_ptr = new QCLImage2DPrivate(other.d_ptr);
        } else if (other.d_ptr) {
            d_ptr->assign(other.d_ptr);
        } else {
            delete d_ptr;
            d_ptr = 0;
        }
    }
    return *this;
}

/*!
    Returns the format descriptor for this OpenCL image.
*/
QCLImageFormat QCLImage2D::format() const
{
    if (!d_ptr) {
        d_ptr = new QCLImage2DPrivate();
        cl_image_format iformat;
        if (clGetImageInfo
                (memoryId(), CL_IMAGE_FORMAT, sizeof(iformat), &iformat, 0)
                    == CL_SUCCESS) {
            d_ptr->format = QCLImageFormat
                (QCLImageFormat::ChannelOrder(iformat.image_channel_order),
                 QCLImageFormat::ChannelType(iformat.image_channel_data_type));
        }
    }
    return d_ptr->format;
}

static int qt_cl_imageParam(cl_mem id, cl_image_info name)
{
    size_t value = 0;
    if (clGetImageInfo(id, name, sizeof(value), &value, 0) != CL_SUCCESS)
        return 0;
    else
        return int(value);
}

/*!
    Returns the width of this OpenCL image.

    \sa height(), bytesPerLine()
*/
int QCLImage2D::width() const
{
    return qt_cl_imageParam(memoryId(), CL_IMAGE_WIDTH);
}

/*!
    Returns the height of this OpenCL image.

    \sa width()
*/
int QCLImage2D::height() const
{
    return qt_cl_imageParam(memoryId(), CL_IMAGE_HEIGHT);
}

/*!
    Returns the number of bytes per element in this OpenCL image.

    \sa bytesPerLine()
*/
int QCLImage2D::bytesPerElement() const
{
    return qt_cl_imageParam(memoryId(), CL_IMAGE_ELEMENT_SIZE);
}

/*!
    Returns the number of bytes per line in this OpenCL image.

    \sa bytesPerElement()
*/
int QCLImage2D::bytesPerLine() const
{
    return qt_cl_imageParam(memoryId(), CL_IMAGE_ROW_PITCH);
}

/*!
    Reads the contents of \a rect from within this image into \a data.
    Returns true if the read was successful; false otherwise.
    If \a bytesPerLine is not zero, it indicates the number of bytes
    between lines in \a data.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa readAsync(), write(), toQImage()
*/
bool QCLImage2D::read(void *data, const QRect &rect, int bytesPerLine)
{
    size_t origin[3] = {rect.x(), rect.y(), 0};
    size_t region[3] = {rect.width(), rect.height(), 1};
    cl_int error = clEnqueueReadImage
        (context()->activeQueue(), memoryId(), CL_TRUE,
         origin, region, bytesPerLine, 0, data, 0, 0, 0);
    context()->reportError("QCLImage2D::read:", error);
    return error == CL_SUCCESS;
}

/*!
    \overload

    Reads the contents of \a rect from within this 2D OpenCL image
    into \a image.  Returns true if the read was successful; false otherwise.
    If \a rect is null, then the entire image is read.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().
*/
bool QCLImage2D::read(QImage *image, const QRect &rect)
{
    if (rect.isNull()) {
        return read(image->bits(),
                    QRect(0, 0, image->width(), image->height()),
                    image->bytesPerLine());
    } else {
        return read(image->bits(), rect, image->bytesPerLine());
    }
}

/*!
    Reads the contents of \a rect from within this image into \a data.
    Returns true if the read was successful; false otherwise.
    If \a bytesPerLine is not zero, it indicates the number of bytes
    between lines in \a data.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa read(), writeAsync()
*/
QCLEvent QCLImage2D::readAsync
    (void *data, const QRect &rect,
     const QCLEventList &after, int bytesPerLine)
{
    size_t origin[3] = {rect.x(), rect.y(), 0};
    size_t region[3] = {rect.width(), rect.height(), 1};
    cl_event event;
    cl_int error = clEnqueueReadImage
        (context()->activeQueue(), memoryId(), CL_FALSE,
         origin, region, bytesPerLine, 0, data,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLImage2D::readAsync:", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Writes the contents \a data to \a rect within this image.
    Returns true if the write was successful; false otherwise.
    If \a bytesPerLine is not zero, it indicates the number of bytes
    between lines in \a data.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa writeAsync(), read()
*/
bool QCLImage2D::write(const void *data, const QRect &rect, int bytesPerLine)
{
    size_t origin[3] = {rect.x(), rect.y(), 0};
    size_t region[3] = {rect.width(), rect.height(), 1};
    cl_int error = clEnqueueWriteImage
        (context()->activeQueue(), memoryId(), CL_TRUE,
         origin, region, bytesPerLine, 0, data, 0, 0, 0);
    context()->reportError("QCLImage2D::write:", error);
    return error == CL_SUCCESS;
}

/*!
    \overload

    Writes the contents of \a image to \a rect within this 2D OpenCL image.
    Returns true if the write was successful; false otherwise.
    If \a rect is null, then the entire image is read.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().
*/
bool QCLImage2D::write(const QImage &image, const QRect &rect)
{
    if (rect.isNull()) {
        return write(image.bits(),
                     QRect(0, 0, image.width(), image.height()),
                     image.bytesPerLine());
    } else {
        return write(image.bits(), rect, image.bytesPerLine());
    }
}

/*!
    Writes the contents of \a data into \a rect within this image.
    Returns true if the write was successful; false otherwise.
    If \a bytesPerLine is not zero, it indicates the number of bytes
    between lines in \a data.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa write(), readAsync()
*/
QCLEvent QCLImage2D::writeAsync
    (const void *data, const QRect &rect,
     const QCLEventList &after, int bytesPerLine)
{
    size_t origin[3] = {rect.x(), rect.y(), 0};
    size_t region[3] = {rect.width(), rect.height(), 1};
    cl_event event;
    cl_int error = clEnqueueWriteImage
        (context()->activeQueue(), memoryId(), CL_FALSE,
         origin, region, bytesPerLine, 0, data,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLImage2D::writeAsync:", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Copies the contents of \a rect from this image to \a destOffset
    in \a dest.  Returns true if the copy was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa copyToAsync()
*/
bool QCLImage2D::copyTo
    (const QRect &rect, const QCLImage2D &dest, const QPoint &destOffset)
{
    size_t src_origin[3] = {rect.x(), rect.y(), 0};
    size_t dst_origin[3] = {destOffset.x(), destOffset.y(), 0};
    size_t region[3] = {rect.width(), rect.height(), 1};
    cl_event event;
    cl_int error = clEnqueueCopyImage
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         src_origin, dst_origin, region, 0, 0, &event);
    context()->reportError("QCLImage2D::copyTo(QCLImage2D):", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
        return true;
    } else {
        return false;
    }
}

/*!
    Copies the contents of \a rect from this image to \a destOffset
    in \a dest.  Returns true if the copy was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa copyToAsync()
*/
bool QCLImage2D::copyTo
    (const QRect &rect, const QCLImage3D &dest, const size_t destOffset[3])
{
    size_t src_origin[3] = {rect.x(), rect.y(), 0};
    size_t region[3] = {rect.width(), rect.height(), 1};
    cl_event event;
    cl_int error = clEnqueueCopyImage
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         src_origin, destOffset, region, 0, 0, &event);
    context()->reportError("QCLImage2D::copyTo(QCLImage3D):", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
        return true;
    } else {
        return false;
    }
}

/*!
    Copies the contents of \a rect from this image to \a destOffset
    in \a dest.  Returns true if the copy was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa copyToAsync()
*/
bool QCLImage2D::copyTo
    (const QRect &rect, const QCLBuffer &dest, size_t destOffset)
{
    size_t src_origin[3] = {rect.x(), rect.y(), 0};
    size_t region[3] = {rect.width(), rect.height(), 1};
    cl_event event;
    cl_int error = clEnqueueCopyImageToBuffer
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         src_origin, region, destOffset, 0, 0, &event);
    context()->reportError("QCLImage2D::copyTo(QCLBuffer):", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
        return true;
    } else {
        return false;
    }
}

/*!
    Requests that the contents of \a rect from this image be copied
    to \a destOffset in \a dest.  Returns an event object that can be
    used to wait for the request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa copyTo()
*/
QCLEvent QCLImage2D::copyToAsync
    (const QRect &rect, const QCLImage2D &dest, const QPoint &destOffset,
     const QCLEventList &after)
{
    size_t src_origin[3] = {rect.x(), rect.y(), 0};
    size_t dst_origin[3] = {destOffset.x(), destOffset.y(), 0};
    size_t region[3] = {rect.width(), rect.height(), 1};
    cl_event event;
    cl_int error = clEnqueueCopyImage
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         src_origin, dst_origin, region,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLImage2D::copyToAsync(QCLImage2D):", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Requests that the contents of \a rect from this image be copied
    to \a destOffset in \a dest.  Returns an event object that can be
    used to wait for the request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa copyTo()
*/
QCLEvent QCLImage2D::copyToAsync
    (const QRect &rect, const QCLImage3D &dest, const size_t destOffset[3],
     const QCLEventList &after)
{
    size_t src_origin[3] = {rect.x(), rect.y(), 0};
    size_t region[3] = {rect.width(), rect.height(), 1};
    cl_event event;
    cl_int error = clEnqueueCopyImage
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         src_origin, destOffset, region,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLImage2D::copyToAsync(QCLImage3D):", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Requests that the contents of \a rect from this image be copied
    to \a destOffset in \a dest.  Returns an event object that can be
    used to wait for the request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa copyTo()
*/
QCLEvent QCLImage2D::copyToAsync
    (const QRect &rect, const QCLBuffer &dest, size_t destOffset,
     const QCLEventList &after)
{
    size_t src_origin[3] = {rect.x(), rect.y(), 0};
    size_t region[3] = {rect.width(), rect.height(), 1};
    cl_event event;
    cl_int error = clEnqueueCopyImageToBuffer
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         src_origin, region, destOffset,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLImage2D::copyToAsync(QCLBuffer):", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

// Defined in qclbuffer.cpp.
extern cl_map_flags qt_cl_map_flags(QCLMemoryObject::Access access);

/*!
    Maps the image region \a rect into host memory for the
    specified \a access mode.  Returns a pointer to the mapped region.

    If \a bytesPerLine is not null, it will return the number of
    bytes per line in the mapped image.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa mapAsync(), unmap()
*/
void *QCLImage2D::map
    (const QRect &rect, QCLMemoryObject::Access access, int *bytesPerLine)
{
    size_t origin[3] = {rect.x(), rect.y(), 0};
    size_t region[3] = {rect.width(), rect.height(), 1};
    cl_int error;
    size_t rowPitch;
    void *data = clEnqueueMapImage
        (context()->activeQueue(), memoryId(), CL_TRUE,
         qt_cl_map_flags(access), origin, region,
         &rowPitch, 0, 0, 0, 0, &error);
    context()->reportError("QCLImage2D::map:", error);
    if (bytesPerLine)
        *bytesPerLine = int(rowPitch);
    return data;
}

/*!
    Maps the image region \a rect into host memory for the specified
    \a access mode.  Returns a pointer to the mapped region in \a ptr,
    which will be valid only after the request finishes.

    If \a bytesPerLine is not null, it will return the number of
    bytes per line in the mapped image.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa map(), unmapAsync()
*/
QCLEvent QCLImage2D::mapAsync
    (void **ptr, const QRect &rect, QCLMemoryObject::Access access,
     const QCLEventList &after, int *bytesPerLine)
{
    size_t origin[3] = {rect.x(), rect.y(), 0};
    size_t region[3] = {rect.width(), rect.height(), 1};
    cl_int error;
    size_t rowPitch;
    cl_event event;
    *ptr = clEnqueueMapImage
        (context()->activeQueue(), memoryId(), CL_FALSE,
         qt_cl_map_flags(access), origin, region, &rowPitch, 0,
         after.size(), after.eventData(), &event, &error);
    context()->reportError("QCLImage2D::mapAsync:", error);
    if (bytesPerLine)
        *bytesPerLine = int(rowPitch);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Reads the contents of this 2D OpenCL image and returns it
    as a QImage.  Returns a null QImage if the OpenCL image's
    format cannot be converted into a QImage format.

    If \a cached is true (the default), then this will allocate
    memory for a QImage object internally and return the same
    object each time.  Otherwise a new QImage object will be created.

    \sa read(), drawImage()
*/
QImage QCLImage2D::toQImage(bool cached)
{
    if (!memoryId())
        return QImage();
    QImage::Format qformat = format().toQImageFormat();
    if (qformat == QImage::Format_Invalid)
        return QImage();
    Q_D(QCLImage2D);
    if (cached) {
        if (d->cachedImage.isNull())
            d->cachedImage = QImage(width(), height(), qformat);
        if (!read(d->cachedImage.bits(),
                  QRect(0, 0, d->cachedImage.width(), d->cachedImage.height()),
                  d->cachedImage.bytesPerLine()))
            return QImage();
        return d->cachedImage;
    } else {
        QImage image(width(), height(), qformat);
        if (!read(image.bits(), QRect(0, 0, image.width(), image.height()),
                  image.bytesPerLine()))
            return QImage();
        return image;
    }
}

// Returns the surface of a pixmap paint device as a QImage
// if it is raster-based.  If we have a -developer-build version
// of Qt, then we can optimize pixmaps and window surfaces from
// the raster graphics system.  If we don't have a -developer-build
// version of Qt, then we can only optimize raster window surfaces.
static QImage *qt_cl_pixmap_image(QPaintDevice *device)
{
#ifdef QT_BUILD_INTERNAL
    QPixmapData *pd = static_cast<QPixmap *>(device)->pixmapData();
    if (pd->classId() == QPixmapData::RasterClass) {
        QRasterPixmapData *rpd = static_cast<QRasterPixmapData *>(pd);
        return rpd->buffer();
    }
    return 0;
#else
    Q_UNUSED(device);
    return 0;
#endif
}

// Returns the surface of "painter" as a QImage, or null if
// it is not represented as a QImage.
static QImage *qt_cl_surface_image(QPainter *painter, QPoint *offset)
{
    QPaintDevice *device = painter->device();

    if (device->devType() == QInternal::Image) {
        *offset = QPoint(0, 0);
        return static_cast<QImage *>(device);
    } else if (device->devType() == QInternal::Pixmap) {
        *offset = QPoint(0, 0);
        return qt_cl_pixmap_image(device);
    } else if (device->devType() == QInternal::Widget) {
        QPaintDevice *redirect = QPainter::redirected(device, offset);
        if (redirect) {
            if (redirect->devType() == QInternal::Image)
                return static_cast<QImage *>(redirect);
            else if (redirect->devType() == QInternal::Pixmap)
                return qt_cl_pixmap_image(redirect);
        }
    }

    return 0;
}

// Determine if a QPainter is in a simple state where we
// can draw into it by direct copying.
static bool qt_cl_is_painter_simple(QPainter *painter, QPoint *offset)
{
    if (painter->hasClipping())
        return false;
    const QTransform &transform = painter->transform();
    if (transform.type() > QTransform::TxTranslate)
        return false;
    QPainter::CompositionMode mode = painter->compositionMode();
    if (mode != QPainter::CompositionMode_SourceOver)
        return false;
    *offset += QPoint(transform.dx(), transform.dy());
    return true;
}

/*!
    \fn void QCLImage2D::drawImage(QPainter *painter, const QPoint &point, const QRect &subRect, Qt::ImageConversionFlags flags)

    Draws this 2D OpenCL image on \a painter at \a point.

    If \a subRect is null, the entire image is drawn; otherwise only
    the indicated sub-rectangle of the image will be drawn.

    If scaling is required to transform the image to the \a painter,
    then \a flags is used to specify how to transform colors during
    scaling.

    This function is equivalent to calling QPainter::drawImage() on
    the result of toQImage() but it may be implemented more efficiently
    by directly copying the OpenCL image data to the painting surface.
    If it isn't possible to optimize the draw, this function will be no
    worse than calling QPainter::drawImage() on the result of toQImage().
*/

// Define this to map the image into host memory for drawing.
// This may be faster or slower than reading the full QImage
// back from the GPU depending upon the system configuration.
//#define QT_CL_MAP_QIMAGE 1

/*!
    Draws this 2D OpenCL image on \a painter, scaled to fit \a targetRect.

    If \a subRect is null, the entire image is drawn; otherwise only
    the indicated sub-rectangle of the image will be drawn.

    The \a flags are used to specify how to transform colors during
    scaling.

    This function is equivalent to calling QPainter::drawImage() on
    the result of toQImage() but it may be implemented more efficiently
    by directly copying the OpenCL image data to the painting surface.
    If it isn't possible to optimize the draw, this function will be no
    worse than calling QPainter::drawImage() on the result of toQImage().
*/
void QCLImage2D::drawImage
    (QPainter *painter, const QRect &targetRect,
     const QRect &subRect, Qt::ImageConversionFlags flags)
{
    // Bail out if the OpenCL image doesn't have a drawable format.
    if (isNull())
        return;
    QImage::Format qformat = format().toQImageFormat();
    if (qformat == QImage::Format_Invalid)
        return;
    int wid = width();
    int ht = height();

    // Can we draw directly into the painter's surface as a QImage?
    Q_D(QCLImage2D);
    QPoint offset;
    QImage *surfaceImage = qt_cl_surface_image(painter, &offset);
    if (surfaceImage && qformat == surfaceImage->format() &&
            !surfaceImage->hasAlphaChannel() &&
            qt_cl_is_painter_simple(painter, &offset)) {
        // Normalize the subRect and targetRect.
        QRect srect, trect;
        if (!subRect.isValid())
            srect = QRect(0, 0, wid, ht);
        else
            srect = subRect;
        if (!targetRect.isValid()) {
            trect = QRect(targetRect.x(), targetRect.y(),
                          srect.width(), srect.height());
        } else {
            trect = targetRect;
        }

        // Translate the target according to the redirection offset.
        trect.translate(offset.x(), offset.y());

        // We need the transformation to be 1-to-1, and for the
        // sub-rectangle to be contained within the source image.
        if (srect.width() == trect.width() &&
                srect.height() == trect.height() &&
                srect.left() >= 0 && srect.top() >= 0 &&
                srect.right() < wid && srect.bottom() < ht) {
            // Clip the target rectangle to the surface extents
            // and modify the source sub-rectangle to match.
            QRect trect2 = trect.intersected
                (QRect(0, 0, surfaceImage->width(), surfaceImage->height()));
            srect.setLeft(srect.left() + trect2.left() - trect.left());
            srect.setTop(srect.top() + trect2.top() - trect.top());
            srect.setRight(srect.right() + trect2.right() - trect.right());
            srect.setBottom(srect.bottom() + trect2.bottom() - trect.bottom());
            if (!srect.isEmpty()) {
                uchar *bits = surfaceImage->bits();
                bits += surfaceImage->bytesPerLine() * trect2.top();
                bits += surfaceImage->depth() * trect2.left() / 8;
                read(bits, srect, surfaceImage->bytesPerLine());
            }
            return;
        }
    }

    // Convert the OpenCL image into a QImage and draw it normally.
#ifdef QT_CL_MAP_QIMAGE
    int bytesPerLine;
    void *mapped = map(QRect(0, 0, wid, ht), QCLMemoryObject::ReadOnly, &bytesPerLine);
    if (mapped) {
        QImage image(reinterpret_cast<const uchar *>(mapped),
                     wid, ht, bytesPerLine, qformat);
        painter->drawImage(targetRect, image, subRect, flags);
        unmap(mapped);
        return;
    }
#endif
    if (d->cachedImage.isNull())
        d->cachedImage = QImage(wid, ht, qformat);
    if (!read(d->cachedImage.bits(),
              QRect(0, 0, d->cachedImage.width(), d->cachedImage.height()),
              d->cachedImage.bytesPerLine()))
        return;
    painter->drawImage(targetRect, d->cachedImage, subRect, flags);
}

/*!
    \class QCLImage3D
    \brief The QCLImage3D class represents a 3D OpenCL image object.
    \since 4.7
    \ingroup opencl
*/

/*!
    \fn QCLImage3D::QCLImage3D()

    Constructs a null 3D OpenCL image object.
*/

/*!
    \fn QCLImage3D::QCLImage3D(QCLContext *context, cl_mem id)

    Constructs a 3D OpenCL image object that is initialized with the
    native OpenCL identifier \a id, and associates it with \a context.
    This class will take over ownership of \a id and will release
    it in the destructor.
*/

/*!
    \fn QCLImage3D::QCLImage3D(const QCLImage3D &other)

    Constructs a copy of \a other.
*/

/*!
    \fn QCLImage3D &QCLImage3D::operator=(const QCLImage3D &other)

    Assigns \a other to this object.
*/

/*!
    Returns the format descriptor for this OpenCL image.
*/
QCLImageFormat QCLImage3D::format() const
{
    cl_image_format iformat;
    if (clGetImageInfo
            (memoryId(), CL_IMAGE_FORMAT, sizeof(iformat), &iformat, 0)
                != CL_SUCCESS)
        return QCLImageFormat();
    else
        return QCLImageFormat
            (QCLImageFormat::ChannelOrder(iformat.image_channel_order),
             QCLImageFormat::ChannelType(iformat.image_channel_data_type));
}

/*!
    Returns the width of this OpenCL image.

    \sa height(), depth(), bytesPerLine()
*/
int QCLImage3D::width() const
{
    return qt_cl_imageParam(memoryId(), CL_IMAGE_WIDTH);
}

/*!
    Returns the height of this OpenCL image.

    \sa width(), depth()
*/
int QCLImage3D::height() const
{
    return qt_cl_imageParam(memoryId(), CL_IMAGE_HEIGHT);
}

/*!
    Returns the depth of this 3D OpenCL image; 0 if the image is 2D.

    \sa width(), height(), bytesPerSlice()
*/
int QCLImage3D::depth() const
{
    return qt_cl_imageParam(memoryId(), CL_IMAGE_DEPTH);
}

/*!
    Returns the number of bytes per element in this OpenCL image.

    \sa bytesPerLine(), bytesPerSlice()
*/
int QCLImage3D::bytesPerElement() const
{
    return qt_cl_imageParam(memoryId(), CL_IMAGE_ELEMENT_SIZE);
}

/*!
    Returns the number of bytes per line in this OpenCL image.

    \sa bytesPerElement(), bytesPerSlice()
*/
int QCLImage3D::bytesPerLine() const
{
    return qt_cl_imageParam(memoryId(), CL_IMAGE_ROW_PITCH);
}

/*!
    Returns the number of bytes per 2D slice in this 3D OpenCL image;
    0 if the image is 2D.

    \sa bytesPerElement(), bytesPerLine()
*/
int QCLImage3D::bytesPerSlice() const
{
    return qt_cl_imageParam(memoryId(), CL_IMAGE_SLICE_PITCH);
}

/*!
    Reads the contents of this 3D image, starting at \a origin,
    and extending for \a size, into \a data.  Returns true if the read
    was successful; false otherwise.  If \a bytesPerLine is not zero,
    it indicates the number of bytes between lines in \a data.
    If \a bytesPerSlice is not zero, it indicates the number of bytes
    between slices in \a data.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa readAsync(), write()
*/
bool QCLImage3D::read
    (void *data, const size_t origin[3], const size_t size[3],
     int bytesPerLine, int bytesPerSlice)
{
    cl_int error = clEnqueueReadImage
        (context()->activeQueue(), memoryId(), CL_TRUE,
         origin, size, bytesPerLine, bytesPerSlice, data, 0, 0, 0);
    context()->reportError("QCLImage3D::read:", error);
    return error == CL_SUCCESS;
}

/*!
    Reads the contents of this 3D image, starting at \a origin,
    and extending for \a size, into \a data.  Returns true if the read
    was successful; false otherwise.  If \a bytesPerLine is not zero,
    it indicates the number of bytes between lines in \a data.
    If \a bytesPerSlice is not zero, it indicates the number of bytes
    between slices in \a data.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa read(), writeAsync()
*/
QCLEvent QCLImage3D::readAsync
    (void *data, const size_t origin[3], const size_t size[3],
     const QCLEventList &after, int bytesPerLine, int bytesPerSlice)
{
    cl_event event;
    cl_int error = clEnqueueReadImage
        (context()->activeQueue(), memoryId(), CL_FALSE,
         origin, size, bytesPerLine, bytesPerSlice, data,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLImage3D::readAsync:", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Writes the contents of this 3D image, starting at \a origin,
    and extending for \a size, to \a data.  Returns true if the write
    was successful; false otherwise.  If \a bytesPerLine is not zero,
    it indicates the number of bytes between lines in \a data.
    If \a bytesPerSlice is not zero, it indicates the number of bytes
    between slices in \a data.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa read(), writeAsync()
*/
bool QCLImage3D::write
    (const void *data, const size_t origin[3], const size_t size[3],
     int bytesPerLine, int bytesPerSlice)
{
    cl_int error = clEnqueueWriteImage
        (context()->activeQueue(), memoryId(), CL_TRUE,
         origin, size, bytesPerLine, bytesPerSlice, data, 0, 0, 0);
    context()->reportError("QCLImage3D::write:", error);
    return error == CL_SUCCESS;
}

/*!
    Writes the contents of this 3D image, starting at \a origin,
    and extending for \a size, to \a data.  Returns true if the write
    was successful; false otherwise.  If \a bytesPerLine is not zero,
    it indicates the number of bytes between lines in \a data.
    If \a bytesPerSlice is not zero, it indicates the number of bytes
    between slices in \a data.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa readAsync(), write()
*/
QCLEvent QCLImage3D::writeAsync
    (const void *data, const size_t origin[3], const size_t size[3],
     const QCLEventList &after, int bytesPerLine, int bytesPerSlice)
{
    cl_event event;
    cl_int error = clEnqueueWriteImage
        (context()->activeQueue(), memoryId(), CL_FALSE,
         origin, size, bytesPerLine, bytesPerSlice, data,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLImage3D::writeAsync:", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Copies the contents of this 3D image, starting at \a origin, and
    extending for \a size, to \a destOffset in \a dest.  Returns true
    if the copy was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa copyToAsync()
*/
bool QCLImage3D::copyTo
    (const size_t origin[3], const size_t size[3],
     const QCLImage3D &dest, const size_t destOffset[3])
{
    cl_event event;
    cl_int error = clEnqueueCopyImage
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         origin, destOffset, size, 0, 0, &event);
    context()->reportError("QCLImage3D::copyTo(QCLImage3D):", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
        return true;
    } else {
        return false;
    }
}

/*!
    Copies the contents of a single slice within this 3D image,
    starting at \a origin, and extending for \a size,
    to \a destOffset in \a dest.  Returns true if the copy was
    successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa copyToAsync()
*/
bool QCLImage3D::copyTo
    (const size_t origin[3], const QSize &size, const QCLImage2D &dest,
     const QPoint &destOffset)
{
    size_t dst_origin[3] = {destOffset.x(), destOffset.y(), 0};
    size_t region[3] = {size.width(), size.height(), 1};
    cl_event event;
    cl_int error = clEnqueueCopyImage
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         origin, dst_origin, region, 0, 0, &event);
    context()->reportError("QCLImage3D::copyTo(QCLImage2D):", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
        return true;
    } else {
        return false;
    }
}

/*!
    Copies the contents of this 3D image, starting at \a origin, and
    extending for \a size, to \a destOffset in \a dest.  Returns true
    if the copy was successful; false otherwise.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa copyToAsync()
*/
bool QCLImage3D::copyTo
    (const size_t origin[3], const size_t size[3],
     const QCLBuffer &dest, size_t destOffset)
{
    cl_event event;
    cl_int error = clEnqueueCopyImageToBuffer
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         origin, size, destOffset, 0, 0, &event);
    context()->reportError("QCLImage3D::copyTo(QCLBuffer):", error);
    if (error == CL_SUCCESS) {
        clWaitForEvents(1, &event);
        clReleaseEvent(event);
        return true;
    } else {
        return false;
    }
}

/*!
    Copies the contents of this 3D image, starting at \a origin, and
    extending for \a size, to \a destOffset in \a dest.  Returns true
    if the copy was successful; false otherwise.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa copyTo()
*/
QCLEvent QCLImage3D::copyToAsync
    (const size_t origin[3], const size_t size[3],
     const QCLImage3D &dest, const size_t destOffset[3],
     const QCLEventList &after)
{
    cl_event event;
    cl_int error = clEnqueueCopyImage
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         origin, destOffset, size,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLImage3D::copyToAsync(QCLImage3D):", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Copies the contents of a single slice within this 3D image,
    starting at \a origin, and extending for \a size,
    to \a destOffset in \a dest.  Returns true if the copy was
    successful; false otherwise.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa copyTo()
*/
QCLEvent QCLImage3D::copyToAsync
    (const size_t origin[3], const QSize &size,
     const QCLImage2D &dest, const QPoint &destOffset,
     const QCLEventList &after)
{
    size_t dst_origin[3] = {destOffset.x(), destOffset.y(), 0};
    size_t region[3] = {size.width(), size.height(), 1};
    cl_event event;
    cl_int error = clEnqueueCopyImage
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         origin, dst_origin, region,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLImage3D::copyToAsync(QCLImage2D):", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Copies the contents of this 3D image, starting at \a origin, and
    extending for \a size, to \a destOffset in \a dest.  Returns true
    if the copy was successful; false otherwise.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa copyTo()
*/
QCLEvent QCLImage3D::copyToAsync
    (const size_t origin[3], const size_t size[3],
     const QCLBuffer &dest, size_t destOffset,
     const QCLEventList &after)
{
    cl_event event;
    cl_int error = clEnqueueCopyImageToBuffer
        (context()->activeQueue(), memoryId(), dest.memoryId(),
         origin, size, destOffset,
         after.size(), after.eventData(), &event);
    context()->reportError("QCLImage3D::copyToAsync(QCLBuffer):", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

/*!
    Maps the image region starting at \a origin and extending for
    \a size into host memory for the specified \a access mode.
    Returns a pointer to the mapped region.

    If \a bytesPerLine is not null, it will return the number of
    bytes per line in the mapped image.  If \a bytesPerSlice is not null,
    it will return the number of bytes per slice in the mapped image.

    This function will block until the request finishes.
    The request is executed on the active command queue for context().

    \sa mapAsync(), unmap()
*/
void *QCLImage3D::map
    (const size_t origin[3], const size_t size[3],
     QCLMemoryObject::Access access, int *bytesPerLine, int *bytesPerSlice)
{
    cl_int error;
    size_t rowPitch, slicePitch;
    void *data = clEnqueueMapImage
        (context()->activeQueue(), memoryId(), CL_TRUE,
         qt_cl_map_flags(access), origin, size,
         &rowPitch, &slicePitch, 0, 0, 0, &error);
    context()->reportError("QCLImage3D::map:", error);
    if (bytesPerLine)
        *bytesPerLine = int(rowPitch);
    if (bytesPerSlice)
        *bytesPerSlice = int(slicePitch);
    return data;
}

/*!
    Maps the image region starting at \a origin and extending for
    \a size into host memory for the specified \a access mode.
    Returns a pointer to the mapped region in \a ptr, which will be
    valid only after the request finishes.

    If \a bytesPerLine is not null, it will return the number of
    bytes per line in the mapped image.  If \a bytesPerSlice is not null,
    it will return the number of bytes per slice in the mapped image.

    This function will queue the request and return immediately.
    Returns an event object that can be used to wait for the
    request to finish.

    The request will not start until all of the events in \a after
    have been signaled as finished.  The request is executed on
    the active command queue for context().

    \sa map(), unmap()
*/
QCLEvent QCLImage3D::mapAsync
    (void **ptr, const size_t origin[3], const size_t size[3],
     QCLMemoryObject::Access access, const QCLEventList &after,
     int *bytesPerLine, int *bytesPerSlice)
{
    cl_int error;
    size_t rowPitch, slicePitch;
    cl_event event;
    *ptr = clEnqueueMapImage
        (context()->activeQueue(), memoryId(),
         CL_FALSE, qt_cl_map_flags(access),
         origin, size, &rowPitch, &slicePitch,
         after.size(), after.eventData(), &event, &error);
    context()->reportError("QCLImage3D::mapAsync:", error);
    if (bytesPerLine)
        *bytesPerLine = int(rowPitch);
    if (bytesPerSlice)
        *bytesPerSlice = int(slicePitch);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
}

QT_END_NAMESPACE
