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

#include "viewgl.h"
#include "image.h"
#include "palette.h"
#include "zoom.h"
#include <QtGui/qpainter.h>
#include <QtGui/qevent.h>
#include <QtCore/qtimer.h>

#if defined(QT_OPENGL_ES_2)
#define VIEW_USE_SHADERS 1
#endif

#ifdef VIEW_USE_SHADERS
#include <QtOpenGL/qglshaderprogram.h>
#endif

ViewGL::ViewGL(QWidget *parent)
    : QGLWidget(parent)
{
    setAutoFillBackground(false);

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

    program = 0;
}

ViewGL::~ViewGL()
{
    delete palette;
    delete image;
    delete zoom;
}

void ViewGL::resizeGL(int width, int height)
{
    glViewport(0, 0, width, height);
}

void ViewGL::initializeGL()
{
    if (!image) {
        int wid = (width() + 15) & ~15;
        int ht = (height() + 15) & ~15;

        image = Image::createImage(wid, ht);
        textureId = image->textureId();
    }

#ifdef VIEW_USE_SHADERS
    static char const vertexShader[] =
        "attribute highp vec4 vertex;\n"
        "attribute highp vec4 texcoord;\n"
        "varying highp vec4 qTexCoord;\n"
        "void main(void)\n"
        "{\n"
        "    gl_Position = vertex;\n"
        "    qTexCoord = texcoord;\n"
        "}\n";
    static char const fragmentShader[] =
        "uniform sampler2D tex;\n"
        "varying highp vec4 qTexCoord;\n"
        "void main(void)\n"
        "{\n"
        "    gl_FragColor = texture2D(tex, qTexCoord.st);\n"
        "}\n";

    program = new QGLShaderProgram(this);
    program->addShaderFromSourceCode(QGLShader::Vertex, vertexShader);
    program->addShaderFromSourceCode(QGLShader::Fragment, fragmentShader);
    program->bindAttributeLocation("vertex", 0);
    program->bindAttributeLocation("texcoord", 1);
    program->link();
#endif
}

void ViewGL::paintGL()
{
    if (!image)
        return;

    static GLfloat const vertices[] = {
        -1, -1, 1, -1, 1, 1, -1, 1
    };
    static GLfloat const texCoords[] = {
        0, 1, 1, 1, 1, 0, 0, 0
    };

#if !defined(VIEW_USE_SHADERS)
    glBindTexture(GL_TEXTURE_2D, textureId);
    glEnable(GL_TEXTURE_2D);
    glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);

    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();

    glVertexPointer(2, GL_FLOAT, 0, vertices);
    glTexCoordPointer(2, GL_FLOAT, 0, texCoords);

    glEnableClientState(GL_VERTEX_ARRAY);
    glEnableClientState(GL_TEXTURE_COORD_ARRAY);

    glColor4f(0.0f, 0.0f, 1.0f, 1.0f);
    glDrawArrays(GL_TRIANGLE_FAN, 0, 4);

    glDisableClientState(GL_VERTEX_ARRAY);
    glDisableClientState(GL_TEXTURE_COORD_ARRAY);

    glDisable(GL_TEXTURE_2D);
    glBindTexture(GL_TEXTURE_2D, 0);
#else
    glBindTexture(GL_TEXTURE_2D, textureId);

    program->bind();
    program->enableAttributeArray(0);
    program->enableAttributeArray(1);
    program->setUniformValue("tex", 0);

    program->setAttributeArray(0, vertices, 2);
    program->setAttributeArray(1, texCoords, 2);

    glDrawArrays(GL_TRIANGLE_FAN, 0, 4);

    program->disableAttributeArray(0);
    program->disableAttributeArray(1);

    glBindTexture(GL_TEXTURE_2D, 0);
#endif

    if (timer->isActive()) {
        qreal fps = frameRate.fps();
        if (fps > 0.0f) {
            QPainter painter(this);
            QString str = QString::number(fps) + QLatin1String(" fps");
            painter.setPen(Qt::white);
            painter.drawText(rect(), str);
        }
    }
}

void ViewGL::keyPressEvent(QKeyEvent *event)
{
    if (event->key() == Qt::Key_Space) {
        if (timer->isActive()) {
            timer->stop();
            frameRate.stop();
        } else {
            timer->start();
            frameRate.start();
        }
        updateGL();
    }
    QGLWidget::keyPressEvent(event);
}

void ViewGL::resizeEvent(QResizeEvent *event)
{
    if (!firstResize)
        resizeTimer->start(100);
    QGLWidget::resizeEvent(event);
}

void ViewGL::animate()
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
    updateGL();
    frameRate.newFrame();
}

void ViewGL::performResize()
{
    firstResize = false;

    int wid = (width() + 15) & ~15;
    int ht = (height() + 15) & ~15;

    if (!image || image->size() != QSize(wid, ht)) {
        delete image;
        image = Image::createImage(wid, ht);
        textureId = image->textureId();

        frameRate.start();

        animate();
    }
}
