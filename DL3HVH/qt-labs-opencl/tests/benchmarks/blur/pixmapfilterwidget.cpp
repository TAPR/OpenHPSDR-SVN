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

#include "pixmapfilterwidget.h"
#include <QPainter>
#include <QDebug>

PixmapFilterWidget::PixmapFilterWidget(QWidget *parent)
    : QWidget(parent), filter(0), eventLoop(0), pixmapOffset(-1),
      useOpenVG(false)
{
#ifdef OPENVG_RAW_TEST
    for (int index = 0; index < 17; ++index)
        tempImage[index] = VG_INVALID_HANDLE;
#endif

    pixmaps[0] = QPixmap(QLatin1String(":/images/accessories-calculator.png"));
    pixmaps[1] = QPixmap(QLatin1String(":/images/accessories-text-editor.png"));
    pixmaps[2] = QPixmap(QLatin1String(":/images/help-browser.png"));
    pixmaps[3] = QPixmap(QLatin1String(":/images/internet-group-chat.png"));
    pixmaps[4] = QPixmap(QLatin1String(":/images/internet-mail.png"));
    pixmaps[5] = QPixmap(QLatin1String(":/images/internet-web-browser.png"));
    pixmaps[6] = QPixmap(QLatin1String(":/images/office-calendar.png"));
    pixmaps[7] = QPixmap(QLatin1String(":/images/system-users.png"));
    pixmaps[8] = QPixmap(QLatin1String(":/images/qt-logo.png"));

    // Create pre-expanded versions of the pixmaps for each blur radius size.
    for (int radius = 1; radius <= 16; ++radius) {
        QPixmapBlurFilter filter;
        filter.setRadius(radius);
        for (int index = 0; index < 9; ++index) {
            QRectF bounds = filter.boundingRectFor(pixmaps[index].rect());
            QPixmap expanded(bounds.size().toSize());
            expanded.fill(QColor(0, 0, 0, 0));
            QPainter p(&expanded);
            p.drawPixmap(-bounds.topLeft(), pixmaps[index]);
            p.end();
            pixmaps[index + radius * 9] = expanded;
        }
    }
}


void PixmapFilterWidget::paintEvent(QPaintEvent *)
{
    if (!filter && !eventLoop)
    {
        return;
    }

    QPainter painter;
    painter.begin(this);
#ifdef OPENVG_RAW_TEST
    bool rawvg = (useOpenVG && filter &&
            painter.paintEngine()->type() == QPaintEngine::OpenVG &&
            pixmapOffset > 0);
    QPointF start(0.0f, 0.0f);
    if (rawvg) {
        painter.beginNativePainting();
        vgSeti(VG_MATRIX_MODE, VG_MATRIX_IMAGE_USER_TO_SURFACE);
        vgSeti(VG_IMAGE_MODE, VG_DRAW_IMAGE_NORMAL);
    }
#endif
    for (int y = 0; y < 3; ++y) {
        for (int x = 0; x < 3; ++x) {
            QPixmap pm(pixmaps[y * 3 + x + (pixmapOffset < 0 ? 0 : pixmapOffset)]);
            QPointF dest(x * 100 + (100 - pm.width()) / 2,
                         y * 100 + (100 - pm.height()) / 2);
#ifdef OPENVG_RAW_TEST
            if (rawvg) {
                // Blur the pixmap and draw it with raw OpenVG calls.
                // This helps to determine the theoretical maximum speed.
                VGImage image = qPixmapToVGImage(pm);
                VGImage blurred = tempImage[pixmapOffset / 9];
                if (blurred == VG_INVALID_HANDLE) {
                    blurred = vgCreateImage
                        (VG_sARGB_8888_PRE, pm.width(), pm.height(),
                         VG_IMAGE_QUALITY_FASTER);
                    tempImage[pixmapOffset / 9] = blurred;
                }
                VGfloat radius = VGfloat(pixmapOffset / 9) / 2.0f;
                vgGaussianBlur(blurred, image, radius, radius, VG_TILE_PAD);
                vgTranslate(dest.x() - start.x(), dest.y() - start.y());
                vgDrawImage(blurred);
                start = dest;
            } else
#endif
            if (filter && pixmapOffset >= 0) {
                // Use a pixmap that has been pre-rendered to a larger size.
                filter->draw(&painter, dest, pm, pm.rect());
            } else if (filter) {
                // Render the pixmap into a larger one for the bounding rect.
                QRectF bounds = filter->boundingRectFor(pm.rect());
                if (pm.width() != bounds.width() ||
                        pm.height() != bounds.height()) {
                    dest += bounds.topLeft();
                    QPixmap pm2(bounds.size().toSize());
                    pm2.fill(QColor(0, 0, 0, 0));
                    QPainter p(&pm2);
                    p.drawPixmap(-bounds.topLeft(), pm);
                    p.end();
                    pm = pm2;
                }
                filter->draw(&painter, dest, pm, pm.rect());
            } else {
                painter.drawPixmap(dest, pm, pm.rect());
            }
        }
    }
#ifdef OPENVG_RAW_TEST
    if (rawvg)
        painter.endNativePainting();
#endif
    painter.end();

    eventLoop->quit();

    filter = 0;
    eventLoop = 0;
};
