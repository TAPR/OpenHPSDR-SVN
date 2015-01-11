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

#ifndef QCLCONTEXT_H
#define QCLCONTEXT_H

#include "qcldevice.h"
#include "qclcommandqueue.h"
#include "qclbuffer.h"
#include "qclvector.h"
#include "qclimage.h"
#include "qclsampler.h"
#include "qclprogram.h"
#include "qcluserevent.h"
#include <QtCore/qscopedpointer.h>
#include <QtCore/qsize.h>
#include <QtCore/qbytearray.h>
#include <QtCore/qstring.h>

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(CL)

class QCLContextPrivate;
class QCLKernel;
class QCLVectorBase;

class Q_CL_EXPORT QCLContext
{
public:
    QCLContext();
    virtual ~QCLContext();

    bool isCreated() const;

    bool create(QCLDevice::DeviceTypes type = QCLDevice::Default);
    bool create(const QList<QCLDevice> &devices);
    virtual void release();

    cl_context contextId() const;
    void setContextId(cl_context id);

    QList<QCLDevice> devices() const;
    QCLDevice defaultDevice() const;

    cl_int lastError() const;
    void setLastError(cl_int error);

    static QString errorName(cl_int code);

    QCLCommandQueue commandQueue();
    void setCommandQueue(const QCLCommandQueue &queue);

    QCLCommandQueue defaultCommandQueue();
    QCLCommandQueue createCommandQueue
        (cl_command_queue_properties properties,
         const QCLDevice &device = QCLDevice());

    QCLBuffer createBufferDevice
        (size_t size, QCLMemoryObject::Access access);
    QCLBuffer createBufferHost
        (void *data, size_t size, QCLMemoryObject::Access access);
    QCLBuffer createBufferCopy
        (const void *data, size_t size, QCLMemoryObject::Access access);

    template <typename T>
    QCLVector<T> createVector(int size, QCLMemoryObject::Access access = QCLMemoryObject::ReadWrite);

    QCLImage2D createImage2DDevice
        (const QCLImageFormat &format, const QSize &size, QCLMemoryObject::Access access);
    QCLImage2D createImage2DHost
        (const QCLImageFormat &format, void *data, const QSize &size,
         QCLMemoryObject::Access access, int bytesPerLine = 0);
    QCLImage2D createImage2DHost(QImage *image, QCLMemoryObject::Access access);
    QCLImage2D createImage2DCopy
        (const QCLImageFormat &format, const void *data, const QSize &size,
         QCLMemoryObject::Access access, int bytesPerLine = 0);
    QCLImage2D createImage2DCopy
        (const QImage &image, QCLMemoryObject::Access access);

    QCLImage3D createImage3DDevice
        (const QCLImageFormat &format, int width, int height, int depth,
         QCLMemoryObject::Access access);
    QCLImage3D createImage3DHost
        (const QCLImageFormat &format, void *data,
         int width, int height, int depth, QCLMemoryObject::Access access,
         int bytesPerLine = 0, int bytesPerSlice = 0);
    QCLImage3D createImage3DCopy
        (const QCLImageFormat &format, const void *data,
         int width, int height, int depth, QCLMemoryObject::Access access,
         int bytesPerLine = 0, int bytesPerSlice = 0);

    QCLProgram createProgramFromSourceCode(const QByteArray &sourceCode);
    QCLProgram createProgramFromSourceFile(const QString &fileName);
    QCLProgram createProgramFromBinaryCode(const QByteArray &binary);
    QCLProgram createProgramFromBinaryFile(const QString &fileName);
    QCLProgram createProgramFromBinaries
        (const QList<QCLDevice> &devices, const QList<QByteArray> &binaries);

    QCLProgram buildProgramFromSourceCode(const QByteArray &sourceCode);
    QCLProgram buildProgramFromSourceFile(const QString &fileName);
    QCLProgram buildProgramFromBinaryCode(const QByteArray &binary);
    QCLProgram buildProgramFromBinaryFile(const QString &fileName);
    QCLProgram buildProgramFromBinaries
        (const QList<QCLDevice> &devices, const QList<QByteArray> &binaries);

    QList<QCLImageFormat> supportedImage2DFormats(cl_mem_flags flags) const;
    QList<QCLImageFormat> supportedImage3DFormats(cl_mem_flags flags) const;

    QCLSampler createSampler
        (bool normalizedCoordinates, QCLSampler::AddressingMode addressingMode,
         QCLSampler::FilterMode filterMode);

    QCLUserEvent createUserEvent();

    void flush();
    void finish();

    QCLEvent marker();

    void sync();

    void barrier();
    void barrier(const QCLEventList &events);

protected:
    void setDefaultDevice(const QCLDevice &device);

private:
    QScopedPointer<QCLContextPrivate> d_ptr;

    Q_DISABLE_COPY(QCLContext)
    Q_DECLARE_PRIVATE(QCLContext)

    cl_command_queue activeQueue(); // For quicker access from friends.

    friend class QCLMemoryObject;
    friend class QCLBuffer;
    friend class QCLImage2D;
    friend class QCLImage3D;
    friend class QCLKernel;
    friend class QCLCommandQueue;
    friend class QCLProgram;
    friend class QCLVectorBase;
    friend class QCLSampler;

    void reportError(const char *name, cl_int error);
};

template <typename T>
Q_INLINE_TEMPLATE QCLVector<T> QCLContext::createVector
    (int size, QCLMemoryObject::Access access)
{
    Q_ASSERT(size >= 1);
    return QCLVector<T>(this, size, access);
}

QT_END_NAMESPACE

QT_END_HEADER

#endif
