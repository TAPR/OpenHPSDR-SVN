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

#include "cltexture2d.h"
#include "qclcontextgl.h"
#include "glproxy.h"
#if QT_VERSION >= 0x040700 && !defined(QT_OPENGL_ES)
#include <QtOpenGL/qglbuffer.h>
#define USE_PIXEL_UNPACK_BUFFERS 1
#endif

QT_BEGIN_NAMESPACE

/*!
    \class CLTexture2D
    \brief The CLTexture2D class represents a 2D OpenCL texture object.
    \since 4.7
    \ingroup openclgl

    Normally applications render into OpenGL textures by calling
    QCLContextGL::createTexture2D() to wrap an existing texture identifier
    with a QCLImage2D object.  However, some systems do not support
    the OpenCL/OpenGL sharing mechanisms that are needed to make that work.

    CLTexture2D abstracts the creation and management of \c{GL_RGBA}
    textures so that applications can render into them with OpenCL
    kernels without needing to implement special handling for
    OpenCL implementations that lack sharing.
*/

class CLTexture2DPrivate : public QObject
{
    Q_OBJECT
public:
    CLTexture2DPrivate()
        : context(0)
        , clContext(0)
        , textureId(0)
        , directRender(false)
#ifdef USE_PIXEL_UNPACK_BUFFERS
        , pixelBuffer(0)
#endif
    {
    }
    ~CLTexture2DPrivate()
    {
#ifdef USE_PIXEL_UNPACK_BUFFERS
        delete pixelBuffer;
#endif
    }

    const QGLContext *context;
    QCLContextGL *clContext;
    GLuint textureId;
    QSize size;
    bool directRender;
#ifdef USE_PIXEL_UNPACK_BUFFERS
    QGLBuffer *pixelBuffer;
#endif

    void setContextAndId(const QGLContext *ctx, GLuint id);

private slots:
    void aboutToDestroyContext(const QGLContext *ctx);
};

void CLTexture2DPrivate::aboutToDestroyContext(const QGLContext *ctx)
{
    if (context == ctx) {
        context = 0;
        textureId = 0;
    }
}

void CLTexture2DPrivate::setContextAndId(const QGLContext *ctx, GLuint id)
{
    context = ctx;
    textureId = id;
    connect(QGLSignalProxy::instance(),
            SIGNAL(aboutToDestroyContext(const QGLContext *)),
            this,
            SLOT(aboutToDestroyContext(const QGLContext *)));
}

/*!
    Constructs an uninitialized OpenCL texture object.
*/
CLTexture2D::CLTexture2D()
    : QCLImage2D(), d_ptr(new CLTexture2DPrivate())
{
}

/*!
    Destroys this OpenCL texture object.
*/
CLTexture2D::~CLTexture2D()
{
    destroy();
}

/*!
    Constructs an OpenCL texture of \a size in \a context.
    Returns true if the texture was created; false otherwise.

    \sa destroy(), textureId()
*/
bool CLTexture2D::create(QCLContextGL *context, const QSize &size)
{
    Q_D(CLTexture2D);
    Q_ASSERT(context && size.width() > 0 && size.height() > 0);
    Q_ASSERT(memoryId() == 0);    // Must not be created already.
    d->clContext = context;

    // Create the texture in the GL context.
    GLuint textureId;
    glGenTextures(1, &textureId);
    glBindTexture(GL_TEXTURE_2D, textureId);
#ifdef GL_CLAMP_TO_EDGE
    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
#else
    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);
#endif
    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, size.width(), size.height(), 0,
                 GL_RGBA, GL_UNSIGNED_BYTE, 0);
    glBindTexture(GL_TEXTURE_2D, 0);

    // If the context supports object sharing, then this is really easy.
    if (context->supportsObjectSharing()) {
        QCLImage2D image = context->createTexture2D
            (GL_TEXTURE_2D, textureId, 0, QCLMemoryObject::WriteOnly);
        if (image.isNull()) {
            glDeleteTextures(1, &textureId);
            return false;
        }
        d->setContextAndId(QGLContext::currentContext(), textureId);
        setId(image.context(), image.memoryId());
        d->size = size;
        d->directRender = true;
        return true;
    }

#ifdef USE_PIXEL_UNPACK_BUFFERS
    // Create a pixel unpack buffer for downloading image data
    // out of OpenCL and uploading it into OpenGL.
    d->pixelBuffer = new QGLBuffer(QGLBuffer::PixelUnpackBuffer);
    d->pixelBuffer->setUsagePattern(QGLBuffer::DynamicDraw);
    if (d->pixelBuffer->create()) {
        d->pixelBuffer->bind();
        d->pixelBuffer->allocate(size.width() * size.height() * 4);
        d->pixelBuffer->release();
    } else {
        delete d->pixelBuffer;
        d->pixelBuffer = 0;
    }
#endif

    // Create a 2D image in the OpenCL device for rendering with OpenCL.
    QCLImage2D image = context->createImage2DDevice
        (QCLImageFormat(QCLImageFormat::Order_RGBA,
                        QCLImageFormat::Type_Normalized_UInt8),
         size, QCLMemoryObject::WriteOnly);
    if (image.isNull()) {
        glDeleteTextures(1, &textureId);
#ifdef USE_PIXEL_UNPACK_BUFFERS
        delete d->pixelBuffer;
        d->pixelBuffer = 0;
#endif
        return false;
    }
    d->setContextAndId(QGLContext::currentContext(), textureId);
    setId(image.context(), image.memoryId());
    d->size = size;
    d->directRender = false;
    return true;
}

/*!
    \fn bool CLTexture2D::create(QCLContextGL *context, int width, int height)
    \overload

    Constructs an OpenCL texture of size (\a width, \a height)
    in \a context.  Returns true if the texture was created; false otherwise.

    \sa destroy()
*/

/*!
    Destroys this OpenCL texture object.
*/
void CLTexture2D::destroy()
{
    Q_D(CLTexture2D);
    setId(0, 0);
    GLuint textureId = d->textureId;
    if (textureId) {
        QGLContext *oldContext;
        QGLContext *currentContext = const_cast<QGLContext *>(QGLContext::currentContext());
        if (currentContext != d->context && !QGLContext::areSharing(d->context, currentContext)) {
            oldContext = currentContext;
            const_cast<QGLContext *>(d->context)->makeCurrent();
        } else {
            oldContext = 0;
        }
        glDeleteTextures(1, &textureId);
        if (oldContext)
            oldContext->makeCurrent();
    }
#ifdef USE_PIXEL_UNPACK_BUFFERS
    delete d->pixelBuffer;
    d->pixelBuffer = 0;
#endif
    d->context = 0;
    d->textureId = 0;
    d->size = QSize();
    d->directRender = false;
}

/*!
    Acquires access to this texture so that OpenCL kernels
    can render into it.  OpenGL cannot use the texture until
    release() is called.

    \sa release()
*/
void CLTexture2D::acquire()
{
    Q_D(CLTexture2D);
    if (d->directRender)
        d->clContext->acquire(*this).waitForFinished();
}

/*!
    Releases access to this texture so that OpenGL can use it again.
    The textureId() will also be bound to the current OpenGL context.

    \sa acquire()
*/
void CLTexture2D::release()
{
    Q_D(CLTexture2D);
    if (!d->textureId)
        return;

    // If we are doing direct rendering, then just release the OpenCL object.
    if (d->directRender) {
        d->clContext->release(*this).waitForFinished();
        glBindTexture(GL_TEXTURE_2D, d->textureId);
        return;
    }

    // Wait for the current OpenCL commands to finish.
    context()->marker().waitForFinished();

    // Upload the contents of the OpenCL buffer into the texture.
    void *ptr;
#ifdef USE_PIXEL_UNPACK_BUFFERS
    if (d->pixelBuffer) {
        d->pixelBuffer->bind();
        ptr = d->pixelBuffer->map(QGLBuffer::ReadWrite);
        if (ptr) {
            read(ptr, QRect(QPoint(0, 0), d->size), d->size.width() * 4);
            d->pixelBuffer->unmap();
            glBindTexture(GL_TEXTURE_2D, d->textureId);
            glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0,
                            d->size.width(), d->size.height(),
                            GL_RGBA, GL_UNSIGNED_BYTE, 0);
            d->pixelBuffer->release();
            return;
        }
        // Pixel buffer cannot be mapped, so it is of no use to us.
        d->pixelBuffer->release();
        delete d->pixelBuffer;
        d->pixelBuffer = 0;
    }
#endif
    ptr = map(QRect(QPoint(0, 0), d->size), QCLMemoryObject::ReadOnly);
    glBindTexture(GL_TEXTURE_2D, d->textureId);
    glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0,
                    d->size.width(), d->size.height(),
                    GL_RGBA, GL_UNSIGNED_BYTE, ptr);
    unmap(ptr);
}

/*!
    Returns the OpenGL texture identifier for this OpenCL texture object.
*/
GLuint CLTexture2D::textureId() const
{
    Q_D(const CLTexture2D);
    return d->textureId;
}

QT_END_NAMESPACE

#include "cltexture2d.moc"
