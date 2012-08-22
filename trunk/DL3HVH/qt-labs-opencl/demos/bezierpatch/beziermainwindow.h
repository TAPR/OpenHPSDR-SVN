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

#ifndef BEZIERMAINWINDOW_H
#define BEZIERMAINWINDOW_H

#include <QtGui/qwidget.h>
#include "ui_beziermainwindow.h"

class BezierMainWindow : public QWidget, public Ui_BezierMainWindow
{
    Q_OBJECT
public:
    BezierMainWindow(QWidget *parent = 0);
    ~BezierMainWindow();

private slots:
    void selectLocalWorkSizeDefault(bool checked)
        { if (checked) bezierWidget->setLocalWorkSize(0, 0); }
    void selectLocalWorkSize1x1(bool checked)
        { if (checked) bezierWidget->setLocalWorkSize(1, 1); }
    void selectLocalWorkSize1x8(bool checked)
        { if (checked) bezierWidget->setLocalWorkSize(1, 8); }
    void selectLocalWorkSize8x1(bool checked)
        { if (checked) bezierWidget->setLocalWorkSize(8, 1); }
    void selectLocalWorkSize8x8(bool checked)
        { if (checked) bezierWidget->setLocalWorkSize(8, 8); }
    void selectLocalWorkSize16x16(bool checked)
        { if (checked) bezierWidget->setLocalWorkSize(16, 16); }
};

#endif
