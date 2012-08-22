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

#include "blurwidget.h"
#include "blurtable.h"
#include <QtCore/qvector.h>
#include <QtCore/qdebug.h>
#include <QtCore/qtimer.h>
#include <QtCore/qdatetime.h>
#include <QtCore/qmath.h>
#include <QtGui/qpainter.h>

BlurWidget::BlurWidget(QWidget *parent)
    : QWidget(parent)
    , radius(1)
    , step(1)
{
//! [1]
    if (!context.create())
        qFatal("Could not create OpenCL context");

    program = context.buildProgramFromSourceFile(QLatin1String(":/blur.cl"));
//! [1]

//! [2]
    QImage img(QLatin1String(":/qtlogo.png"));
    srcImageBuffer = context.createImage2DCopy(img, QCLMemoryObject::ReadOnly);
//! [2]

//! [3]
    tmpImageBuffer = context.createImage2DDevice(QImage::Format_ARGB32, img.size(), QCLMemoryObject::ReadWrite);

    dstImage = QImage(img.size(), QImage::Format_ARGB32);
    dstImageBuffer = context.createImage2DDevice(dstImage.format(), dstImage.size(), QCLMemoryObject::WriteOnly);
//! [3]

//! [4]
    weightsBuffer = context.createVector<float>(100);
    offsetsBuffer = context.createVector<float>(100);
//! [4]

//! [5]
    hgaussian = program.createKernel("hgaussian");
    hgaussian.setGlobalWorkSize(img.size());
    hgaussian.setLocalWorkSize(hgaussian.bestLocalWorkSizeImage2D());

    vgaussian = program.createKernel("vgaussian");
    vgaussian.setGlobalWorkSize(img.size());
    vgaussian.setLocalWorkSize(vgaussian.bestLocalWorkSizeImage2D());
//! [5]

    QTimer *timer = new QTimer(this);
    connect(timer, SIGNAL(timeout()), this, SLOT(animate()));
    timer->start(50);
}

BlurWidget::~BlurWidget()
{
}

void BlurWidget::paintEvent(QPaintEvent *)
{
    QTime time;
    time.start();

    // Upload the weights and offsets into OpenCL.
//! [6]
    offsetsBuffer.write(blurOffsets[radius], blurSizes[radius]);
    weightsBuffer.write(blurWeights[radius], blurSizes[radius]);
//! [6]

    // Execute the horizontal and vertical Gaussian kernels.
//! [7]
    hgaussian(srcImageBuffer, tmpImageBuffer, weightsBuffer, offsetsBuffer, blurSizes[radius]);
    vgaussian(tmpImageBuffer, dstImageBuffer, weightsBuffer, offsetsBuffer, blurSizes[radius]);
//! [7]

    // Read back the destination image into host memory and draw it.
//! [8]
    dstImageBuffer.read(&dstImage);
    QPainter painter(this);
    painter.drawImage(0, 0, dstImage);
//! [8]

    qWarning("Time to blur with radius %d: %d ms", radius, time.elapsed());
}

void BlurWidget::animate()
{
    radius += step;
    if (radius >= 16 || radius <= 1)
        step = -step;
    update();
}
