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

#ifndef PALETTE_H
#define PALETTE_H

#include <QtCore/qvector.h>
#include <QtGui/qcolor.h>

class Palette
{
public:
    Palette();
    ~Palette();

    enum StandardPalette
    {
        Red,
        Blue,
        EarthSky,
        HotCold,
        Fire
    };

    void setStandardPalette(StandardPalette palette);

    void clear();
    void addRgb(qreal start, qreal r, qreal g, qreal b);
    void addHsv(qreal start, qreal h, qreal s, qreal v);
    void setOffset(qreal value) { offset = value; }

    QVector<QRgb> createTable(int size) const;

private:
    struct Range
    {
        qreal start;
        union {
            struct {
                qreal r, g, b;
            };
            struct {
                qreal h, s, v;
            };
        };
    };
    QColor::Spec type;
    qreal offset;
    QVector<Range> ranges;
};

#endif
