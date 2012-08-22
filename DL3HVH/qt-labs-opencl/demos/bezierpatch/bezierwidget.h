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

#ifndef BEZIERWIDGET_H
#define BEZIERWIDGET_H

#include <QtOpenGL/qgl.h>
#include <QtGui/qmatrix4x4.h>
#include <QtGui/qvector2d.h>
#include <QtGui/qvector3d.h>
#include <QtGui/qvector4d.h>
#include "qclcontextgl.h"
#include "qglcamera.h"
#include "framerate.h"

class QGLBuffer;

class BezierWidget : public QGLWidget
{
    Q_OBJECT
public:
    BezierWidget(QWidget *parent = 0);
    ~BezierWidget();

protected:
    void resizeGL(int width, int height);
    void initializeGL();
    void paintGL();

public slots:
    void setUseOpenCL(bool value);
    void setLocalWorkSize(int x, int y);

private slots:
    void animate();

private:
    bool useOpenCL;

    QCLContextGL context;

    QCLProgram program;
    QCLKernel evaluateBezier;
    QCLBuffer positionBuffer;
    QCLBuffer texCoordBuffer;

    QGLCamera camera;
    FrameRate frameRate;

    QMatrix4x4 matrixM;
    QMatrix4x4 matrixX;
    QMatrix4x4 matrixY;
    QMatrix4x4 matrixZ;
    QVector3D cp[16];

    void *vertices;
    QVector4D *positions;
    QVector2D *texCoords;
#ifdef QT_OPENGL_ES
    typedef ushort IndexType;
    static const GLenum IndexTypeEnum = GL_UNSIGNED_SHORT;
#else
    typedef uint IndexType;
    static const GLenum IndexTypeEnum = GL_UNSIGNED_INT;
#endif
    IndexType *indices;
    int numVertices;
    int numIndices;
    int subdivisionSize;
    int lastSubdivisionSize;

    QImage textureImage;
    GLuint textureId;

    QGLBuffer *vertexBuffer;
    QGLBuffer *texBuffer;
    QGLBuffer *indexBuffer;

    void computeMatrices();
    void computeVerticesNative();
    void computeVerticesCL();

    void allocVertices();
    void freeVertices();
};

#endif
