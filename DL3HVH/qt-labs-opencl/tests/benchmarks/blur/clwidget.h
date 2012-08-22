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

#ifndef CLWIDGET_H
#define CLWIDGET_H

#include <QWidget>
#include <QEventLoop>
#include "qclcontext.h"

class CLWidget : public QWidget
{
public:
    CLWidget(QWidget *parent = 0);

    bool contextCreated();
    void setEventLoop(QEventLoop *loop) { eventLoop = loop; }
    void setup(int radius);
    void startBlur(int radius);

protected:
    void paintEvent(QPaintEvent *);

    void populateSourceImages();
    void adjustSourceImageSizes(QSize newSize);

private:
    QEventLoop *eventLoop;

    QCLContext context;
    QCLProgram program;
    QCLKernel horizontalGaussianKernel;
    QCLKernel verticalGaussianKernel;

    QImage srcImages[9];
    QImage dstImages[9 * 15];
    QCLImage2D srcImageBuffers[9];
    QCLImage2D tmpImageBuffers[9];
    QCLImage2D dstImageBuffers[9];

    QVector<float> offsets;
    QVector<float> weights;
    QCLVector<float> weightsBuffer;
    QCLVector<float> offsetsBuffer;

    QSize largestSourceImageSize;

    // old variables
//    QImage dstImage;
//    QCLImage2D srcImageBuffer;
//    QCLImage2D dstImageBuffer;
//    QCLImage2D tmpImageBuffer;
};

#endif // CLWIDGET_H
