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

#include "imagecl.h"
#include "palette.h"
#include <QtCore/qvarlengtharray.h>
#include <QtGui/qcolor.h>

class ImageCLContext
{
public:
    ImageCLContext() : context(0), glContext(0) {}
    ~ImageCLContext();

    void init(bool useGL, int wid, int ht);

    QCLContext *context;
    QCLContextGL *glContext;
    QCLProgram program;
    QCLKernel mandelbrot;
};

void ImageCLContext::init(bool useGL, int wid, int ht)
{
    if (context) {
        mandelbrot.setGlobalWorkSize(wid, ht);
        return;
    }

    if (useGL) {
        glContext = new QCLContextGL();
        context = glContext;
        if (!glContext->create())
            return;
    } else {
        context = new QCLContext();
        if (!context->create())
            return;
    }

    program = context->buildProgramFromSourceFile
        (QLatin1String(":/mandelbrot.cl"));
    mandelbrot = program.createKernel("mandelbrot");
    mandelbrot.setGlobalWorkSize(wid, ht);
    mandelbrot.setLocalWorkSize(mandelbrot.bestLocalWorkSizeImage2D());
}

ImageCLContext::~ImageCLContext()
{
    delete context;
}

Q_GLOBAL_STATIC(ImageCLContext, image_context)

ImageCL::ImageCL(int width, int height)
    : Image(width, height)
    , img(width, height, QImage::Format_RGB32)
    , lastIterations(-1)
    , initialized(false)
{
}

ImageCL::~ImageCL()
{
}

void ImageCL::init(bool useGL)
{
    if (initialized)
        return;
    initialized = true;

    // Initialize the context for GL or non-GL.
    ImageCLContext *ctx = image_context();
    ctx->init(useGL, wid, ht);
}

GLuint ImageCL::textureId()
{
    init(true);

    ImageCLContext *ctx = image_context();
    if (!textureBuffer.create(ctx->glContext, wid, ht))
        qWarning("Could not create the OpenCL texture to render into.");

    return textureBuffer.textureId();
}

void ImageCL::initialize()
{
    init(false);
}

static bool openclDisabled = false;

bool ImageCL::hasOpenCL()
{
    if (openclDisabled)
        return false;
    return !QCLDevice::devices(QCLDevice::Default).isEmpty();
}

void ImageCL::disableCL()
{
    openclDisabled = true;
}

void ImageCL::generate(int maxIterations, const Palette &palette)
{
    QRectF region = rgn;

    init();

    ImageCLContext *ctx = image_context();
    QCLKernel mandelbrot = ctx->mandelbrot;

    // Upload the color table into a buffer in the device.
    if (colorBuffer.isNull() || lastIterations != maxIterations) {
        QVector<QRgb> colors = palette.createTable(maxIterations);
        if (lastIterations != maxIterations)
            colorBuffer = QCLBuffer();
        if (colorBuffer.isNull()) {
            colorBuffer = ctx->context->createBufferDevice
                (maxIterations * sizeof(float) * 4, QCLMemoryObject::ReadOnly);
        }
        QVarLengthArray<float> floatColors;
        for (int index = 0; index < maxIterations; ++index) {
            QColor color(colors[index]);
            floatColors.append(float(color.redF()));
            floatColors.append(float(color.greenF()));
            floatColors.append(float(color.blueF()));
            floatColors.append(float(color.alphaF()));
        }
        colorBuffer.write(floatColors.constData(),
                          maxIterations * sizeof(float) * 4);
        lastIterations = maxIterations;
    }

    if (!textureBuffer.textureId()) {
        // Create a buffer for the image in the OpenCL device.
        if (imageBuffer.isNull()) {
            imageBuffer = ctx->context->createImage2DDevice
                (QImage::Format_RGB32, QSize(wid, ht), QCLMemoryObject::WriteOnly);
        }

        // Execute the "mandelbrot" kernel.
        mandelbrot(imageBuffer, float(region.x()), float(region.y()),
                   float(region.width()), float(region.height()),
                   wid, ht, maxIterations, colorBuffer);
    } else {
        // Finish previous GL operations on the texture.
        if (ctx->glContext->supportsObjectSharing())
            glFinish();

        // Acquire the GL texture object.
        textureBuffer.acquire();

        // Execute the "mandelbrot" kernel.
        mandelbrot(textureBuffer, float(region.x()), float(region.y()),
                   float(region.width()), float(region.height()),
                   wid, ht, maxIterations, colorBuffer);

        // Release the GL texture object and wait for it complete.
        // After the release is complete, the texture can be used by GL.
        textureBuffer.release();
    }
}

void ImageCL::paint(QPainter *painter, const QPoint& point)
{
    imageBuffer.drawImage(painter, point);
}
