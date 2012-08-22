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

#ifndef QCLCONTEXTGL_H
#define QCLCONTEXTGL_H

#include "qclcontext.h"
#include <QtOpenGL/qgl.h>
#if QT_VERSION >= 0x040700
#include <QtOpenGL/qglbuffer.h>
#endif

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(CLGL)

class QCLContextGLPrivate;

// Note: this class probably should be in the QtOpenGL library.
class Q_CLGL_EXPORT QCLContextGL : public QCLContext
{
public:
    QCLContextGL();
    ~QCLContextGL();

    bool create(const QCLPlatform &platform = QCLPlatform());
    void release();

    bool supportsObjectSharing() const;

    QCLBuffer createGLBuffer(GLuint bufobj, QCLMemoryObject::Access access);
#if QT_VERSION >= 0x040700 || defined(Q_QDOC)
    QCLBuffer createGLBuffer(QGLBuffer *bufobj, QCLMemoryObject::Access access);
#endif

    QCLImage2D createTexture2D
        (GLenum type, GLuint texture, GLint mipmapLevel, QCLMemoryObject::Access access);
    QCLImage2D createTexture2D(GLuint texture, QCLMemoryObject::Access access);

    QCLImage3D createTexture3D
        (GLenum type, GLuint texture, GLint mipmapLevel, QCLMemoryObject::Access access);
    QCLImage3D createTexture3D(GLuint texture, QCLMemoryObject::Access access);

    QCLImage2D createRenderbuffer(GLuint renderbuffer, QCLMemoryObject::Access access);

#ifdef Q_MAC_COMPAT_GL_FUNCTIONS
    QCLBuffer createGLBuffer(QMacCompatGLuint bufobj, QCLMemoryObject::Access access);

    QCLImage2D createTexture2D
        (QMacCompatGLenum type, QMacCompatGLuint texture,
         QMacCompatGLint mipmapLevel, QCLMemoryObject::Access access);
    QCLImage2D createTexture2D(QMacCompatGLuint texture, QCLMemoryObject::Access access);

    QCLImage3D createTexture3D
        (QMacCompatGLenum type, QMacCompatGLuint texture,
         QMacCompatGLint mipmapLevel, QCLMemoryObject::Access access);
    QCLImage3D createTexture3D(QMacCompatGLuint texture, QCLMemoryObject::Access access);

    QCLImage2D createRenderbuffer
        (QMacCompatGLuint renderbuffer, QCLMemoryObject::Access access);
#endif

    static bool isGLBuffer(const QCLBuffer &buffer);
    static bool isTexture2D(const QCLImage2D &image);
    static bool isTexture3D(const QCLImage3D &image);
    static bool isRenderbuffer(const QCLImage2D &image);

    QCLEvent acquire(const QCLMemoryObject &mem);
    QCLEvent acquire
        (const QCLMemoryObject &mem, const QCLEventList &after);

    QCLEvent release(const QCLMemoryObject &mem);
    QCLEvent release
        (const QCLMemoryObject &mem, const QCLEventList &after);

private:
    QScopedPointer<QCLContextGLPrivate> d_ptr;

    Q_DISABLE_COPY(QCLContextGL)
    Q_DECLARE_PRIVATE(QCLContextGL)

    void reportError(const char *name, cl_int error);
};

QT_END_NAMESPACE

QT_END_HEADER

#endif
