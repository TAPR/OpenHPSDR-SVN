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

#include "view.h"
#include "image.h"
#include "palette.h"
#include "zoom.h"
#include <QtGui/qpainter.h>
#include <QtGui/qevent.h>
#include <QtCore/qtimer.h>

View::View(QWidget *parent)
    : QWidget(parent)
{
    setAttribute(Qt::WA_OpaquePaintEvent);

    palette = new Palette();
    palette->setStandardPalette(Palette::EarthSky);
    offset = 0.0f;
    step = 0.005f;

    zoom = new GoldenGradientZoom();

    image = 0;

    timer = new QTimer(this);
    connect(timer, SIGNAL(timeout()), this, SLOT(animate()));
    timer->start(0);

    resizeTimer = new QTimer(this);
    resizeTimer->setSingleShot(true);
    connect(resizeTimer, SIGNAL(timeout()), this, SLOT(performResize()));
    resizeTimer->start(0);
    firstResize = true;
}

View::~View()
{
    delete palette;
    delete image;
    delete zoom;
}

void View::paintEvent(QPaintEvent *)
{
    if (!image)
        return;
    QPainter painter(this);
    QSize imgSize = image->size();
    int x = (width() - imgSize.width()) / 2;
    int y = (height() - imgSize.height()) / 2;
    image->paint(&painter, QPoint(x, y));
    if (timer->isActive()) {
        qreal fps = frameRate.fps();
        if (fps > 0.0f) {
            QString str = QString::number(fps) + QLatin1String(" fps");
            painter.setPen(Qt::white);
            painter.drawText(rect(), str);
        }
    }
}

void View::keyPressEvent(QKeyEvent *event)
{
    if (event->key() == Qt::Key_Space) {
        if (timer->isActive()) {
            timer->stop();
            frameRate.stop();
        } else {
            timer->start();
            frameRate.start();
        }
        update();
    }
    QWidget::keyPressEvent(event);
}

void View::resizeEvent(QResizeEvent *event)
{
    if (!firstResize)
        resizeTimer->start(100);
    QWidget::resizeEvent(event);
}

void View::animate()
{
    if (!image)
        return;
    if (step > 0) {
        offset += step;
        if (offset >= 1.0f) {
            offset = 1.0f;
            step = -step;
        }
    } else {
        offset += step;
        if (offset <= 0.0f) {
            offset = 0.0f;
            step = -step;
        }
    }
    zoom->generate(image, offset, *palette);
    update();
    frameRate.newFrame();
}

void View::performResize()
{
    firstResize = false;

    int wid = (width() + 15) & ~15;
    int ht = (height() + 15) & ~15;

    if (!image || image->size() != QSize(wid, ht)) {
        delete image;
        image = Image::createImage(wid, ht);
        image->initialize();

        frameRate.start();

        animate();
    }
}
