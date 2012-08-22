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

#include "graphicseffectview.h"

GraphicsEffectView::GraphicsEffectView(QWidget *parent)
    : QGraphicsView(parent), eventLoop(0)
{
    setHorizontalScrollBarPolicy(Qt::ScrollBarAlwaysOff);
    setVerticalScrollBarPolicy(Qt::ScrollBarAlwaysOff);
    setViewportUpdateMode(FullViewportUpdate);
    setRenderHints(QPainter::Antialiasing | QPainter::SmoothPixmapTransform
                   | QPainter::TextAntialiasing);

    scene = new QGraphicsScene(this);
    setScene(scene);

    pixmaps[0] = new QGraphicsPixmapItem
        (QPixmap(QLatin1String(":/images/accessories-calculator.png")));
    pixmaps[1] = new QGraphicsPixmapItem
        (QPixmap(QLatin1String(":/images/accessories-text-editor.png")));
    pixmaps[2] = new QGraphicsPixmapItem
        (QPixmap(QLatin1String(":/images/help-browser.png")));
    pixmaps[3] = new QGraphicsPixmapItem
        (QPixmap(QLatin1String(":/images/internet-group-chat.png")));
    pixmaps[4] = new QGraphicsPixmapItem
        (QPixmap(QLatin1String(":/images/internet-mail.png")));
    pixmaps[5] = new QGraphicsPixmapItem
        (QPixmap(QLatin1String(":/images/internet-web-browser.png")));
    pixmaps[6] = new QGraphicsPixmapItem
        (QPixmap(QLatin1String(":/images/office-calendar.png")));
    pixmaps[7] = new QGraphicsPixmapItem
        (QPixmap(QLatin1String(":/images/system-users.png")));
    pixmaps[8] = new QGraphicsPixmapItem
        (QPixmap(QLatin1String(":/images/qt-logo.png")));

    for (int y = 0; y < 3; ++y) {
        for (int x = 0; x < 3; ++x) {
            int index = y * 3 + x;
            pixmaps[index]->setPos(x * 100 + 50, y * 100 + 50);
            scene->addItem(pixmaps[index]);
        }
    }
}

GraphicsEffectView::~GraphicsEffectView()
{
}

void GraphicsEffectView::paintEvent(QPaintEvent *event)
{
    QGraphicsView::paintEvent(event);
    if (eventLoop) {
        eventLoop->quit();
        eventLoop = 0;
    }
}
