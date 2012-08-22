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

#include "qclcontextgl.h"
#include "qcl_gl_p.h"
#include <QtCore/qdebug.h>
#include <QtCore/qvarlengtharray.h>

#if !defined(QT_NO_CL_OPENGL)
#if defined(QT_OPENGL_ES_2)
#include <EGL/egl.h>
#elif defined(QT_OPENGL_ES)
#include <GLES/egl.h>
#elif defined(Q_WS_X11)
#include <GL/glx.h>
#endif
#endif

QT_BEGIN_NAMESPACE

/*!
    \class QCLContextGL
    \brief The QCLContextGL class represents an OpenCL context that is suitable for use with OpenGL objects.
    \since 4.7
    \ingroup openclgl
*/

class QCLContextGLPrivate
{
public:
    QCLContextGLPrivate()
        : supportsSharing(false)
    {
    }

    bool supportsSharing;
};

/*!
    Constructs a new OpenCL context object that is suitable for use
    with OpenGL objects.
*/
QCLContextGL::QCLContextGL()
    : QCLContext(), d_ptr(new QCLContextGLPrivate())
{
}

/*!
    Destroys this OpenCL context.
*/
QCLContextGL::~QCLContextGL()
{
}

extern "C" {

static void qt_clgl_context_notify(const char *errinfo,
                                   const void *private_info,
                                   size_t cb,
                                   void *user_data)
{
    Q_UNUSED(private_info);
    Q_UNUSED(cb);
    Q_UNUSED(user_data);
    qWarning() << "OpenCL/GL context notification: " << errinfo;
}

};

#if !defined(CL_CURRENT_DEVICE_FOR_GL_CONTEXT_KHR)
#define CL_CURRENT_DEVICE_FOR_GL_CONTEXT_KHR 0x2006
#endif

#if !defined(CL_DEVICES_FOR_GL_CONTEXT_KHR)
#define CL_DEVICES_FOR_GL_CONTEXT_KHR 0x2007
#endif

#if !defined(CL_GL_CONTEXT_KHR)
#define CL_GL_CONTEXT_KHR           0x2008
#define CL_EGL_DISPLAY_KHR          0x2009
#define CL_GLX_DISPLAY_KHR          0x200A
#define CL_WGL_HDC_KHR              0x200B
#define CL_CGL_SHAREGROUP_KHR       0x200C
#endif

/*!
    Creates an OpenCL context that is compatible with the current
    QGLContext and \a platform.  Returns false if there is no OpenGL
    context current or the OpenCL context could not be created for
    some reason.

    This function will first try to create a QCLDevice::GPU device,
    and will then fall back to QCLDevice::Default if a GPU is not found.

    If \a platform is null, then the first platform that has a GPU
    will be used.  If there is no GPU, then the first platform with a
    default device will be used.

    \sa supportsObjectSharing()
*/
bool QCLContextGL::create(const QCLPlatform &platform)
{
    Q_D(QCLContextGL);

    // Bail out if the context already exists.
    if (isCreated())
        return true;

    // Bail out if we don't have an OpenGL context.
    if (!QGLContext::currentContext()) {
        qWarning() << "QCLContextGL::create: needs a current GL context";
        setLastError(CL_INVALID_CONTEXT);
        return false;
    }

    // Find the first gpu device.
    QList<QCLDevice> devices;
    cl_device_type deviceType = CL_DEVICE_TYPE_GPU;
    devices = QCLDevice::devices(QCLDevice::GPU, platform);
    if (devices.isEmpty()) {
        // Find the first default device.
        devices = QCLDevice::devices(QCLDevice::Default, platform);
        deviceType = CL_DEVICE_TYPE_DEFAULT;
    }
    if (devices.isEmpty()) {
        qWarning() << "QCLContextGL::create: no gpu devices found";
        setLastError(CL_DEVICE_NOT_FOUND);
        return false;
    }
    QCLDevice gpu = devices[0];
    QVarLengthArray<cl_device_id> devs;
    foreach (QCLDevice dev, devices)
        devs.append(dev.deviceId());

    // Add the platform identifier to the properties.
    QVarLengthArray<cl_context_properties> properties;
    properties.append(CL_CONTEXT_PLATFORM);
    properties.append(cl_context_properties(gpu.platform().platformId()));

    bool hasSharing = false;
#ifndef QT_NO_CL_OPENGL
    // Determine what kind of OpenCL-OpenGL sharing we have and enable it.
#if defined(__APPLE__) || defined(__MACOSX)
    bool appleSharing = gpu.hasExtension("cl_apple_gl_sharing");
    if (appleSharing) {
        CGLContextObj cglContext = CGLGetCurrentContext();
        CGLShareGroupObj cglShareGroup = CGLGetShareGroup(cglContext);
        properties.append(CL_CGL_SHAREGROUP_KHR);
        properties.append(cl_context_properties(cglShareGroup));
        hasSharing = true;
    }
#else
    bool khrSharing = gpu.hasExtension("cl_khr_gl_sharing");
#if defined(QT_OPENGL_ES_2) || defined(QT_OPENGL_ES)
    if (khrSharing) {
        properties.append(CL_EGL_DISPLAY_KHR);
        properties.append(cl_context_properties(eglGetCurrentDisplay()));
#ifdef EGL_OPENGL_ES_API
        eglBindAPI(EGL_OPENGL_ES_API);
#endif
        properties.append(CL_GL_CONTEXT_KHR);
        properties.append(cl_context_properties(eglGetCurrentContext()));
        hasSharing = true;
    }
#elif defined(Q_WS_X11)
    if (khrSharing) {
        properties.append(CL_GLX_DISPLAY_KHR);
        properties.append(cl_context_properties(glXGetCurrentDisplay()));
        properties.append(CL_GL_CONTEXT_KHR);
        properties.append(cl_context_properties(glXGetCurrentContext()));
        hasSharing = true;
    }
#else
    // Needs to be ported to other platforms.
    if (khrSharing)
        qWarning() << "QCLContextGL::create: do not know how to enable sharing";
#endif
#endif
#endif // !QT_NO_CL_OPENGL
    properties.append(0);

#ifndef QT_NO_CL_OPENGL
    // Query the actual OpenCL devices we should use with the OpenGL context.
    typedef cl_int (*q_PFNCLGETGLCONTEXTINFOKHR)
        (const cl_context_properties *, cl_uint, size_t, void *, size_t *);
    q_PFNCLGETGLCONTEXTINFOKHR getGLContextInfo =
        (q_PFNCLGETGLCONTEXTINFOKHR)clGetExtensionFunctionAddress
            ("clGetGLContextInfoKHR");
    if (getGLContextInfo && hasSharing) {
        size_t size;
        cl_device_id currentDev;
        if(getGLContextInfo(properties.data(),
                            CL_DEVICES_FOR_GL_CONTEXT_KHR,
                            0, 0, &size) == CL_SUCCESS && size > 0) {
            QVarLengthArray<cl_device_id> buf(size / sizeof(cl_device_id));
            getGLContextInfo(properties.data(),
                             CL_DEVICES_FOR_GL_CONTEXT_KHR,
                             size, buf.data(), 0);
            devs = buf;
            gpu = QCLDevice(devs[0]);
        }
        if (getGLContextInfo(properties.data(),
                             CL_CURRENT_DEVICE_FOR_GL_CONTEXT_KHR,
                             sizeof(currentDev), &currentDev, 0)
                == CL_SUCCESS) {
            gpu = QCLDevice(currentDev);
        }
    }
#endif

    // Create the OpenCL context.
    cl_context id;
    cl_int error;
    id = clCreateContext
        (properties.data(), devs.size(), devs.data(),
         qt_clgl_context_notify, 0, &error);
    if (!id && hasSharing) {
        // Try again without the sharing parameters.
        properties.resize(2);
        properties.append(0);
        hasSharing = false;
        id = clCreateContext
            (properties.data(), devs.size(), devs.data(),
             qt_clgl_context_notify, 0, &error);
    }
    setLastError(error);
    if (id == 0) {
        qWarning() << "QCLContextGL::create:" << errorName(error);
        d->supportsSharing = false;
    } else {
        setContextId(id);
        clReleaseContext(id);   // setContextId() adds an extra reference.
        setDefaultDevice(gpu);
        d->supportsSharing = hasSharing;
    }
    return id != 0;
}

/*!
    \reimp
*/
void QCLContextGL::release()
{
    Q_D(QCLContextGL);
    d->supportsSharing = false;
    QCLContext::release();
}

/*!
    Returns true if this OpenCL context supports object sharing
    with OpenGL; false otherwise.

    \sa createGLBuffer(), createTexture2D(), createTexture3D()
    \sa createRenderbuffer()
*/
bool QCLContextGL::supportsObjectSharing() const
{
    Q_D(const QCLContextGL);
    return d->supportsSharing;
}

/*!
    Creates an OpenCL memory buffer from the OpenGL buffer object
    \a bufobj, with the specified \a access mode.

    This function will only work if supportsObjectSharing() is true.
*/
QCLBuffer QCLContextGL::createGLBuffer(GLuint bufobj, QCLMemoryObject::Access access)
{
#ifndef QT_NO_CL_OPENGL
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access);
    cl_mem mem = clCreateFromGLBuffer
        (contextId(), flags, bufobj, &error);
    reportError("QCLContextGL::createGLBuffer:", error);
    if (mem)
        return QCLBuffer(this, mem);
    else
        return QCLBuffer();
#else
    Q_UNUSED(bufobj);
    Q_UNUSED(access);
    reportError("QCLContextGL::createGLBuffer:", CL_INVALID_VALUE);
    return QCLBuffer();
#endif
}

#if QT_VERSION >= 0x040700 || defined(Q_QDOC)

/*!
    \overload

    Creates an OpenCL memory buffer from the OpenGL buffer object
    \a bufobj, with the specified \a access mode.

    This function will only work if supportsObjectSharing() is true.
*/
QCLBuffer QCLContextGL::createGLBuffer(QGLBuffer *bufobj, QCLMemoryObject::Access access)
{
    if (!bufobj)
        return QCLBuffer();
    return createGLBuffer(GLuint(bufobj->bufferId()), access);
}

#endif

/*!
    Creates a 2D OpenCL image object from the specified \a mipmapLevel,
    OpenGL \a texture object, and \a access mode.

    The \a type must be one of \c{GL_TEXTURE_2D},
    \c{GL_TEXTURE_CUBE_MAP_POSITIVE_X}, \c{GL_TEXTURE_CUBE_MAP_POSITIVE_Y},
    \c{GL_TEXTURE_CUBE_MAP_POSITIVE_Z}, \c{GL_TEXTURE_CUBE_MAP_NEGATIVE_X},
    \c{GL_TEXTURE_CUBE_MAP_NEGATIVE_Y}, \c{GL_TEXTURE_CUBE_MAP_NEGATIVE_Z},
    or \c{GL_TEXTURE_RECTANGLE}.  The \a texture does not need to be
    bound to an OpenGL texture target.

    This function will only work if supportsObjectSharing() is true.

    \sa createTexture3D(), createRenderbuffer()
*/
QCLImage2D QCLContextGL::createTexture2D
    (GLenum type, GLuint texture, GLint mipmapLevel, QCLMemoryObject::Access access)
{
#ifndef QT_NO_CL_OPENGL
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access);
    cl_mem mem = clCreateFromGLTexture2D
        (contextId(), flags, type, mipmapLevel, texture, &error);
    reportError("QCLContextGL::createGLTexture2D:", error);
    if (mem)
        return QCLImage2D(this, mem);
    else
        return QCLImage2D();
#else
    Q_UNUSED(type);
    Q_UNUSED(texture);
    Q_UNUSED(mipmapLevel);
    Q_UNUSED(access);
    reportError("QCLContextGL::createGLTexture2D:", CL_INVALID_VALUE);
    return QCLImage2D();
#endif
}

/*!
    \overload

    Creates a 2D OpenCL image object from the specified OpenGL
    \a texture object, and the \a access mode.  If texture type is
    assumed to be \c{GL_TEXTURE_2D} and the mipmap level is
    assumed to be 0.

    This function will only work if supportsObjectSharing() is true.
*/
QCLImage2D QCLContextGL::createTexture2D(GLuint texture, QCLMemoryObject::Access access)
{
    return createTexture2D(GL_TEXTURE_2D, texture, 0, access);
}

#ifndef GL_TEXTURE_3D
#define GL_TEXTURE_3D 0x806F
#endif

/*!
    Creates a 3D OpenCL image object from the specified \a mipmapLevel,
    OpenGL \a texture object, and \a access mode.

    The \a type must be \c{GL_TEXTURE_3D}.  The \a texture does not need
    to be bound to an OpenGL texture target.

    This function will only work if supportsObjectSharing() is true.

    \sa createTexture2D()
*/
QCLImage3D QCLContextGL::createTexture3D
    (GLenum type, GLuint texture, GLint mipmapLevel, QCLMemoryObject::Access access)
{
#ifndef QT_NO_CL_OPENGL
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access);
    cl_mem mem = clCreateFromGLTexture3D
        (contextId(), flags, type, mipmapLevel, texture, &error);
    reportError("QCLContextGL::createGLTexture3D:", error);
    if (mem)
        return QCLImage3D(this, mem);
    else
        return QCLImage3D();
#else
    Q_UNUSED(type);
    Q_UNUSED(texture);
    Q_UNUSED(mipmapLevel);
    Q_UNUSED(access);
    reportError("QCLContextGL::createGLTexture3D:", CL_INVALID_VALUE);
    return QCLImage3D();
#endif
}

/*!
    \overload

    Creates a 3D OpenCL image object from the specified OpenGL
    \a texture object, and \a access mode.  If texture type is
    assumed to be \c{GL_TEXTURE_3D} and the mipmap level is
    assumed to be 0.

    This function will only work if supportsObjectSharing() is true.
*/
QCLImage3D QCLContextGL::createTexture3D(GLuint texture, QCLMemoryObject::Access access)
{
    return createTexture3D(GL_TEXTURE_3D, texture, 0, access);
}

/*!
    Creates a 2D OpenCL image object from the specified OpenGL
    \a renderbuffer object, and the \a access mode.

    This function will only work if supportsObjectSharing() is true.

    \sa createTexture2D()
*/
QCLImage2D QCLContextGL::createRenderbuffer
    (GLuint renderbuffer, QCLMemoryObject::Access access)
{
#ifndef QT_NO_CL_OPENGL
    cl_int error = CL_INVALID_CONTEXT;
    cl_mem_flags flags = cl_mem_flags(access);
    cl_mem mem = clCreateFromGLRenderbuffer
        (contextId(), flags, renderbuffer, &error);
    reportError("QCLContextGL::createGLRenderbuffer:", error);
    if (mem)
        return QCLImage2D(this, mem);
    else
        return QCLImage2D();
#else
    Q_UNUSED(renderbuffer);
    Q_UNUSED(access);
    reportError("QCLContextGL::createRenderbuffer:", CL_INVALID_VALUE);
    return QCLImage2D();
#endif
}

/*!
    \internal
*/
void QCLContextGL::reportError(const char *name, cl_int error)
{
    setLastError(error);
    if (error != CL_SUCCESS)
        qWarning() << name << QCLContext::errorName(error);
}

#ifdef Q_MAC_COMPAT_GL_FUNCTIONS

/*!
    \internal
*/
QCLBuffer QCLContextGL::createGLBuffer
    (QMacCompatGLuint bufobj, QCLMemoryObject::Access access)
{
    return createGLBuffer(GLuint(bufobj), access);
}

/*!
    \internal
*/
QCLImage2D QCLContextGL::createTexture2D
    (QMacCompatGLenum type, QMacCompatGLuint texture,
     QMacCompatGLint mipmapLevel, QCLMemoryObject::Access access)
{
    return createTexture2D(GLenum(type), GLuint(texture),
                           GLint(mipmapLevel), access);
}

/*!
    \internal
*/
QCLImage2D QCLContextGL::createTexture2D
    (QMacCompatGLuint texture, QCLMemoryObject::Access access)
{
    return createTexture2D(GLenum(GL_TEXTURE_2D), GLuint(texture),
                           GLint(0), access);
}

/*!
    \internal
*/
QCLImage3D QCLContextGL::createTexture3D
    (QMacCompatGLenum type, QMacCompatGLuint texture,
     QMacCompatGLint mipmapLevel, QCLMemoryObject::Access access)
{
    return createTexture3D(GLenum(type), GLuint(texture),
                           GLint(mipmapLevel), access);
}

/*!
    \internal
*/
QCLImage3D QCLContextGL::createTexture3D
    (QMacCompatGLuint texture, QCLMemoryObject::Access access)
{
    return createTexture3D(GLenum(GL_TEXTURE_3D), GLuint(texture),
                           GLint(0), access);
}

/*!
    \internal
*/
QCLImage2D QCLContextGL::createRenderbuffer
    (QMacCompatGLuint renderbuffer, QCLMemoryObject::Access access)
{
    return createRenderbuffer(GLuint(renderbuffer), access);
}

#endif

/*!
    Returns true if the OpenCL \a buffer object is also an OpenGL
    buffer object; false otherwise.
*/
bool QCLContextGL::isGLBuffer(const QCLBuffer &buffer)
{
#ifndef QT_NO_CL_OPENGL
    cl_gl_object_type objectType;
    if (clGetGLObjectInfo
            (buffer.memoryId(), &objectType, 0) != CL_SUCCESS)
        return false;
    return objectType == CL_GL_OBJECT_BUFFER;
#else
    Q_UNUSED(buffer);
    return false;
#endif
}

/*!
    Returns true if the 2D OpenCL \a image object is also an OpenGL
    2D texture object; false otherwise.

    \sa isRenderbuffer(), isTexture3D()
*/
bool QCLContextGL::isTexture2D(const QCLImage2D &image)
{
#ifndef QT_NO_CL_OPENGL
    cl_gl_object_type objectType;
    if (clGetGLObjectInfo
            (image.memoryId(), &objectType, 0) != CL_SUCCESS)
        return false;
    return objectType == CL_GL_OBJECT_TEXTURE2D;
#else
    Q_UNUSED(image);
    return false;
#endif
}

/*!
    Returns true if the 3D OpenCL \a image object is also an OpenGL
    3D texture object; false otherwise.

    \sa isTexture2D()
*/
bool QCLContextGL::isTexture3D(const QCLImage3D &image)
{
#ifndef QT_NO_CL_OPENGL
    cl_gl_object_type objectType;
    if (clGetGLObjectInfo
            (image.memoryId(), &objectType, 0) != CL_SUCCESS)
        return false;
    return objectType == CL_GL_OBJECT_TEXTURE3D;
#else
    Q_UNUSED(image);
    return false;
#endif
}

/*!
    Returns true if the 2D OpenCL \a image object is also an OpenGL
    renderbuffer object; false otherwise.

    \sa isTexture2D()
*/
bool QCLContextGL::isRenderbuffer(const QCLImage2D &image)
{
#ifndef QT_NO_CL_OPENGL
    cl_gl_object_type objectType;
    if (clGetGLObjectInfo
            (image.memoryId(), &objectType, 0) != CL_SUCCESS)
        return false;
    return objectType == CL_GL_OBJECT_RENDERBUFFER;
#else
    Q_UNUSED(image);
    return false;
#endif
}

/*!
    Acquires access to the OpenGL object behind the OpenCL memory
    object \a mem.  This function must be called before performing
    an OpenCL operation on any OpenGL memory object.

    Returns an event object that can be used to wait for the
    request to finish.  The request is executed on the active
    command queue for this context.

    \sa release()
*/
QCLEvent QCLContextGL::acquire(const QCLMemoryObject &mem)
{
#ifndef QT_NO_CL_OPENGL
    cl_event event;
    cl_mem id = mem.memoryId();
    cl_int error = clEnqueueAcquireGLObjects
        (commandQueue().queueId(), 1, &id, 0, 0, &event);
    reportError("QCLContextGL::acquire:", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
#else
    Q_UNUSED(mem);
    return QCLEvent();
#endif
}

/*!
    \overload

    Acquires access to the OpenGL object behind the OpenCL memory
    object \a mem.  This function must be called before performing
    an OpenCL operation on any OpenGL memory object.

    The request will not start until all of the events in \a after
    have been signaled as finished.

    Returns an event object that can be used to wait for the
    request to finish.  The request is executed on the active
    command queue for this context.

    \sa release()
*/
QCLEvent QCLContextGL::acquire
    (const QCLMemoryObject &mem, const QCLEventList &after)
{
#ifndef QT_NO_CL_OPENGL
    cl_event event;
    cl_mem id = mem.memoryId();
    cl_int error = clEnqueueAcquireGLObjects
        (commandQueue().queueId(), 1, &id,
         after.size(), after.eventData(), &event);
    reportError("QCLContextGL::acquire(after):", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
#else
    Q_UNUSED(mem);
    Q_UNUSED(after);
    return QCLEvent();
#endif
}

/*!
    Releases access to the OpenGL object behind the OpenCL memory
    object \a mem.  This function must be called after performing
    an OpenCL operation on any OpenGL memory object, and before
    performing OpenGL operations on the object.

    Returns an event object that can be used to wait for the
    request to finish.  The request is executed on the active
    command queue for this context.

    \sa acquire()
*/
QCLEvent QCLContextGL::release(const QCLMemoryObject &mem)
{
#ifndef QT_NO_CL_OPENGL
    cl_event event;
    cl_mem id = mem.memoryId();
    cl_int error = clEnqueueReleaseGLObjects
        (commandQueue().queueId(), 1, &id, 0, 0, &event);
    reportError("QCLContextGL::release:", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
#else
    Q_UNUSED(mem);
    return QCLEvent();
#endif
}

/*!
    \overload

    Releases access to the OpenGL object behind the OpenCL memory
    object \a mem.  This function must be called after performing
    an OpenCL operation on any OpenGL memory object, and before
    performing OpenGL operations on the object.

    The request will not start until all of the events in \a after
    have been signaled as finished.

    Returns an event object that can be used to wait for the
    request to finish.  The request is executed on the active
    command queue for this context.

    \sa acquire()
*/
QCLEvent QCLContextGL::release
    (const QCLMemoryObject &mem, const QCLEventList &after)
{
#ifndef QT_NO_CL_OPENGL
    cl_event event;
    cl_mem id = mem.memoryId();
    cl_int error = clEnqueueReleaseGLObjects
        (commandQueue().queueId(), 1, &id,
         after.size(), after.eventData(), &event);
    reportError("QCLContextGL::release(after):", error);
    if (error == CL_SUCCESS)
        return QCLEvent(event);
    else
        return QCLEvent();
#else
    Q_UNUSED(mem);
    Q_UNUSED(after);
    return QCLEvent();
#endif
}

QT_END_NAMESPACE
