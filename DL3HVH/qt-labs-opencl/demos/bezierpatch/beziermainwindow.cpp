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

#include "beziermainwindow.h"

BezierMainWindow::BezierMainWindow(QWidget *parent)
    : QWidget(parent)
{
    setupUi(this);

    useOpenCL->setChecked(true);
    localWorkSize8x8->setChecked(true);

    connect(useOpenCL, SIGNAL(toggled(bool)),
            bezierWidget, SLOT(setUseOpenCL(bool)));
    connect(useOpenCL, SIGNAL(toggled(bool)),
            localWorkSize, SLOT(setEnabled(bool)));

    connect(localWorkSizeDefault, SIGNAL(toggled(bool)),
            this, SLOT(selectLocalWorkSizeDefault(bool)));
    connect(localWorkSize1x1, SIGNAL(toggled(bool)),
            this, SLOT(selectLocalWorkSize1x1(bool)));
    connect(localWorkSize1x8, SIGNAL(toggled(bool)),
            this, SLOT(selectLocalWorkSize1x8(bool)));
    connect(localWorkSize8x1, SIGNAL(toggled(bool)),
            this, SLOT(selectLocalWorkSize8x1(bool)));
    connect(localWorkSize8x8, SIGNAL(toggled(bool)),
            this, SLOT(selectLocalWorkSize8x8(bool)));
    connect(localWorkSize16x16, SIGNAL(toggled(bool)),
            this, SLOT(selectLocalWorkSize16x16(bool)));
}

BezierMainWindow::~BezierMainWindow()
{
}
