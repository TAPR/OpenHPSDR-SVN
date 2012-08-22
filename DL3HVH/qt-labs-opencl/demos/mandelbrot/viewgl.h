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

#ifndef VIEWGL_H
#define VIEWGL_H

#include <QtOpenGL/qgl.h>
#include "framerate.h"

class Image;
class Palette;
class Zoom;
class QTimer;
class QGLShaderProgram;

class ViewGL : public QGLWidget
{
    Q_OBJECT
public:
    ViewGL(QWidget *parent = 0);
    ~ViewGL();

private slots:
    void animate();
    void performResize();

protected:
    void resizeGL(int width, int height);
    void initializeGL();
    void paintGL();
    void keyPressEvent(QKeyEvent *);
    void resizeEvent(QResizeEvent *);

private:
    QTimer *timer;
    Image *image;
    Palette *palette;
    qreal offset;
    qreal step;
    GLuint textureId;
    Zoom *zoom;
    FrameRate frameRate;
    QGLShaderProgram *program;
    QTimer *resizeTimer;
    bool firstResize;
};

#endif
