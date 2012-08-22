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

#include "palette.h"

Palette::Palette()
    : type(QColor::Invalid), offset(0.0f)
{
    setStandardPalette(Fire);
}

Palette::~Palette()
{
}

void Palette::setStandardPalette(StandardPalette palette)
{
    clear();
    switch (palette) {
    default: case Red:
        addHsv(0.0f, 0.0f, 1.0f, 1.0f);
        addHsv(1.0f, 1.0f, 1.0f, 1.0f);
        break;
    case Blue:
        addHsv(0.0f, 0.65f, 1.0f, 0.5f);
        addHsv(1.0f, 0.05f, 1.0f, 1.0f);
        break;

    // The following palettes are from xMandelbrotViewer.
    // http://math.hws.edu/xJava/MB/
    case EarthSky:
        addRgb(0.0f, 1.0f, 1.0f, 1.0f);
        addRgb(0.15f, 1.0f, 0.8f, 0.0f);
        addRgb(0.33f, 0.53f, 0.12f, 0.075f);
        addRgb(0.67f, 0.0f, 0.0f, 0.6f);
        addRgb(0.85f, 0.0f, 0.4f, 1.0f);
        addRgb(1.0f, 1.0f, 1.0f, 1.0f);
        setOffset(0.85f);
        break;
    case HotCold:
        addRgb(0.0f, 1.0f, 1.0f, 1.0f);
        addRgb(0.16f, 0.0f, 0.4f, 1.0f);
        addRgb(0.5f, 0.2f, 0.2f, 0.2f);
        addRgb(0.84f, 1.0f, 0.0f, 0.8f);
        addRgb(1.0f, 1.0f, 1.0f, 1.0f);
        break;
    case Fire:
        addRgb(0.0f, 0.0f, 0.0f, 0.0f);
        addRgb(0.17f, 1.0f, 0.0f, 0.0f);
        addRgb(0.83f, 1.0f, 1.0f, 0.0f);
        addRgb(1.0f, 1.0f, 1.0f, 1.0f);
        break;
    }
}

void Palette::clear()
{
    type = QColor::Invalid;
    offset = 0.0f;
    ranges.clear();
}

void Palette::addRgb(qreal start, qreal r, qreal g, qreal b)
{
    type = QColor::Rgb;
    Range range;
    range.start = start;
    range.r = r;
    range.g = g;
    range.b = b;
    ranges.append(range);
}

void Palette::addHsv(qreal start, qreal h, qreal s, qreal v)
{
    type = QColor::Hsv;
    Range range;
    range.start = start;
    range.h = h;
    range.s = s;
    range.v = v;
    ranges.append(range);
}

QVector<QRgb> Palette::createTable(int size) const
{
    Q_ASSERT(ranges.size() >= 2);
    Q_ASSERT(ranges[ranges.size() - 1].start == 1.0f);
    QVector<QRgb> colors;
    colors.resize(size);
    int ioffset = int(offset * size);
    for (int index = 0; index < size; ++index) {
        qreal amt = qreal((index + ioffset) % size) / (size - 1);
        int posn = 1;
        while (ranges[posn].start < 1.0f && ranges[posn].start < amt)
            ++posn;
        amt = (amt - ranges[posn - 1].start) /
              (ranges[posn].start - ranges[posn - 1].start);
        if (type == QColor::Rgb) {
            qreal r = ranges[posn - 1].r +
                (ranges[posn].r - ranges[posn - 1].r) * amt;
            qreal g = ranges[posn - 1].g +
                (ranges[posn].g - ranges[posn - 1].g) * amt;
            qreal b = ranges[posn - 1].b +
                (ranges[posn].b - ranges[posn - 1].b) * amt;
            colors[index] = QColor::fromRgbF(r, g, b).rgb();
        } else {
            qreal h = ranges[posn - 1].h +
                (ranges[posn].h - ranges[posn - 1].h) * amt;
            qreal s = ranges[posn - 1].s +
                (ranges[posn].s - ranges[posn - 1].s) * amt;
            qreal v = ranges[posn - 1].v +
                (ranges[posn].v - ranges[posn - 1].v) * amt;
            colors[index] = QColor::fromHsvF(h, s, v).rgb();
        }
    }
    return colors;
}
