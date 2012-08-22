/****************************************************************************
**
** Copyright (C) 2010 Nokia Corporation and/or its subsidiary(-ies).
** All rights reserved.
** Contact: Nokia Corporation (qt-info@nokia.com)
**
** This file is part of the examples of the Qt Toolkit.
**
** $QT_BEGIN_LICENSE:BSD$
** You may use this file under the terms of the BSD license as follows:
**
** "Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions are
** met:
**   * Redistributions of source code must retain the above copyright
**     notice, this list of conditions and the following disclaimer.
**   * Redistributions in binary form must reproduce the above copyright
**     notice, this list of conditions and the following disclaimer in
**     the documentation and/or other materials provided with the
**     distribution.
**   * Neither the name of Nokia Corporation and its Subsidiary(-ies) nor
**     the names of its contributors may be used to endorse or promote
**     products derived from this software without specific prior written
**     permission.
**
** THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
** "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
** LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
** A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
** OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
** SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
** LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
** OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE."
** $QT_END_LICENSE$
**
****************************************************************************/

#include "imagewidget.h"
#include <QtGui/qpainter.h>
#include <QtGui/qvector4d.h>

ImageWidget::ImageWidget(QWidget *parent)
    : QWidget(parent)
{
    setAttribute(Qt::WA_OpaquePaintEvent);

    if (!context.create())
        qFatal("Could not create OpenCL context");

    program = context.buildProgramFromSourceFile(QLatin1String(":/imagedrawing.cl"));

    fillRectWithColor = program.createKernel("fillRectWithColor");
    fillRectWithColor.setLocalWorkSize(fillRectWithColor.bestLocalWorkSizeImage2D());

    drawImageKernel = program.createKernel("drawImage");
    drawImageKernel.setLocalWorkSize(drawImageKernel.bestLocalWorkSizeImage2D());

    flower = context.createImage2DCopy
        (QImage(QLatin1String(":/flower.jpg")), QCLMemoryObject::ReadOnly);
    bg1 = context.createImage2DCopy
        (QImage(QLatin1String(":/bg1.jpg")), QCLMemoryObject::ReadOnly);
    beavis = context.createImage2DCopy
        (QImage(QLatin1String(":/beavis.jpg")), QCLMemoryObject::ReadOnly);
    qtlogo = context.createImage2DCopy
        (QImage(QLatin1String(":/qtlogo.png")), QCLMemoryObject::ReadOnly);
}

ImageWidget::~ImageWidget()
{
}

void ImageWidget::paintEvent(QPaintEvent *)
{
    // Create a QCLImage2D object for the window surface.
    QSize wsize = size();
    if (wsize != windowSize)
        surfaceImage = QCLImage2D();
    if (surfaceImage.isNull()) {
        windowSize = wsize;
        surfaceImage = context.createImage2DDevice
            (QImage::Format_RGB32, windowSize, QCLMemoryObject::ReadWrite);
    }

    // Clear to the background color.
    QColor bgcolor = palette().color(backgroundRole());
    fillRect(0, 0, wsize.width(), wsize.height(), bgcolor);

    // Draw the images.
    drawImage(flower, 50, 50, 1.0f);
    drawImage(beavis, 200, 100, 0.9f);
    drawScaledImage(beavis, 550, 150, 250, 250, 0.9f);
    drawImage(bg1, 0, 0, 0.7f);
    drawImage(qtlogo, 200, 350, 1.0f);

    // Draw the window surface image to the actual window.
    QPainter painter(this);
    surfaceImage.drawImage(&painter, QPoint(0, 0));
}

void ImageWidget::fillRect(int x, int y, int width, int height,
                          const QColor& color)
{
    // Round up the global work size so we can process the
    // rectangle in local work size units.  The kernel will
    // ignore pixels that are outside the rectangle limits.
    fillRectWithColor.setRoundedGlobalWorkSize(width, height);
    fillRectWithColor(surfaceImage, x, y, x + width, y + height, color);
}

void ImageWidget::drawImage(const QCLImage2D& image, int x, int y, float opacity)
{
    QRect srcRect = QRect(0, 0, image.width(), image.height());
    QRect dstRect = QRect(x, y, srcRect.width(), srcRect.height());

    // Clamp the draw to the surface extents.
    QRect dstRect2 = dstRect.intersect
        (QRect(0, 0, windowSize.width(), windowSize.height()));
    srcRect.setLeft(srcRect.left() + dstRect2.left() - dstRect.left());
    srcRect.setTop(srcRect.top() + dstRect2.top() - dstRect.top());
    srcRect.setRight(srcRect.right() + dstRect2.right() - dstRect.right());
    srcRect.setBottom(srcRect.bottom() + dstRect2.bottom() - dstRect.bottom());
    dstRect = dstRect2;
    if (srcRect.isEmpty() || dstRect.isEmpty())
        return;

    // Set the global work size to the destination size rounded up to local.
    drawImageKernel.setRoundedGlobalWorkSize(dstRect.size());

    // Draw the image.
    QVector4D src(srcRect.x(), srcRect.y(), srcRect.width(), srcRect.height());
    drawImageKernel
        (surfaceImage, surfaceImage, image,
         dstRect.x(), dstRect.y(), dstRect.width(), dstRect.height(),
         src, opacity);
}

void ImageWidget::drawScaledImage
    (const QCLImage2D& image, int x, int y, int width, int height,
     float opacity)
{
    QRectF srcRect = QRectF(0, 0, image.width(), image.height());
    QRect dstRect = QRect(x, y, width, height);

    // Clamp the draw to the surface extents.
    QRect dstRect2 = dstRect.intersect
        (QRect(0, 0, windowSize.width(), windowSize.height()));
    qreal scaleX = srcRect.width() / width;
    qreal scaleY = srcRect.height() / height;
    srcRect.setLeft(srcRect.left() + (dstRect2.left() - dstRect.left()) * scaleX);
    srcRect.setTop(srcRect.top() + (dstRect2.top() - dstRect.top()) * scaleY);
    srcRect.setRight(srcRect.right() + (dstRect2.right() - dstRect.right()) * scaleX);
    srcRect.setBottom(srcRect.bottom() + (dstRect2.bottom() - dstRect.bottom()) * scaleY);
    dstRect = dstRect2;
    if (srcRect.isEmpty() || dstRect.isEmpty())
        return;

    // Set the global work size to the destination size rounded up to local.
    drawImageKernel.setRoundedGlobalWorkSize(dstRect.size());

    // Draw the image.
    QVector4D src(srcRect.x(), srcRect.y(), srcRect.width(), srcRect.height());
    drawImageKernel
        (surfaceImage, surfaceImage, image,
         dstRect.x(), dstRect.y(), dstRect.width(), dstRect.height(),
         src, opacity);
}
