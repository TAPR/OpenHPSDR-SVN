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

#include "framerate.h"
#include <QtCore/qtimer.h>

FrameRate::FrameRate(QObject *parent)
    : QObject(parent)
{
    checkPointTimer = new QTimer(this);
    connect(checkPointTimer, SIGNAL(timeout()), this, SLOT(checkPoint()));
    start();
}

FrameRate::~FrameRate()
{
}

qreal FrameRate::fps() const
{
    int ms = frameBase.elapsed();
    qint64 count = frames;
    if (checkPoints[0] != -1) {
        // Adjust the reported value so that we get a running fps
        // over a 5 second period rather than a fps value measured
        // from the last time start() was called.  This can help
        // smooth out ripples in the fps count.
        count -= checkPoints[0];
        ms -= frameBase.msecsTo(checkPointTimes[0]);
    }
    if (active && ms >= 100)
        return count * 1000.0f / ms;
    else
        return 0.0f;
}

void FrameRate::start()
{
    active = true;
    frames = 0;
    frameBase.start();
    for (int index = 0; index < NumCheckPoints; ++index)
        checkPoints[index] = -1;
    checkPointTimer->start(1000);
}

void FrameRate::stop()
{
    active = false;
    checkPointTimer->stop();
}

void FrameRate::checkPoint()
{
    for (int index = 0; index < NumCheckPoints - 1; ++index) {
        checkPoints[index] = checkPoints[index + 1];
        checkPointTimes[index] = checkPointTimes[index + 1];
    }
    checkPoints[NumCheckPoints - 1] = frames;
    checkPointTimes[NumCheckPoints - 1] = QTime::currentTime();
}
