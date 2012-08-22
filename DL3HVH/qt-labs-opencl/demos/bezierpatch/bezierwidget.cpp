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

#include "bezierwidget.h"
#include <QtCore/qtimer.h>
#include <QtCore/qdatetime.h>
#include <QtGui/qpainter.h>
#if QT_VERSION >= 0x040700
#define USE_VBOS 1
#include <QtOpenGL/qglbuffer.h>
#endif

/*
    The Bezier patch equation is:

    x(s, t) = S . M . Gx . transpose(M) . transpose(T)
    y(s, t) = S . M . Gy . transpose(M) . transpose(T)
    z(s, t) = S . M . Gz . transpose(M) . transpose(T)

    where:

    S  = [s^3 s^2 s 1]
    T  = [t^3 t^2 t 1]
    M  = |-1  3 -3  1|
         | 3 -6  3  0]
         |-3  3  0  0|
         | 1  0  0  0|
    Gx = | x11 x12 x13 x14 |  i.e. x components of the control points
         | x21 x22 x23 x24 |
         | x31 x32 x33 x34 |
         | x41 x42 x43 x44 |
    Gy, Gz are similar

    Note: transpose(M) = M

    From "Computer Graphics: Principles and Practice", Foley, van Dam,
    et al, Second Edition, Addison Wesley, 1996.
*/

BezierWidget::BezierWidget(QWidget *parent)
    : QGLWidget(parent)
    , useOpenCL(true)
    , matrixM(-1,  3, -3,  1,
               3, -6,  3,  0,
              -3,  3,  0,  0,
               1,  0,  0,  0)
    , positions(0)
    , texCoords(0)
    , indices(0)
    , numVertices(0)
    , numIndices(0)
#ifdef QT_OPENGL_ES
    // We need a value that is a multiple of 16, but less than
    // sqrt(65535 / 6) = 104.5 because we cannot use more than
    // 65535 indices under OpenGL/ES.
    , subdivisionSize(96)
#else
    , subdivisionSize(512)
#endif
    , lastSubdivisionSize(-1)
    , textureId(0)
    , vertexBuffer(0)
    , texBuffer(0)
    , indexBuffer(0)
{
    setAutoFillBackground(false);

    // Patch is in the X-Z plane from x = 0, z = 0, to x = 3, z = 3.
    // The height of the patch at each control point is adjusted in Y.
    cp[0]  = QVector3D(0, 0, 0);
    cp[1]  = QVector3D(1, 1, 0);
    cp[2]  = QVector3D(2, -1, 0);
    cp[3]  = QVector3D(3, 0, 0);
    cp[4]  = QVector3D(0, 0, 1);
    cp[5]  = QVector3D(1, 1, 1);
    cp[6]  = QVector3D(2, -1, 1);
    cp[7]  = QVector3D(3, 0, 1);
    cp[8]  = QVector3D(0, 0, 2);
    cp[9]  = QVector3D(1, 1, 2);
    cp[10] = QVector3D(2, -1, 2);
    cp[11] = QVector3D(3, 0, 2);
    cp[12] = QVector3D(0, 0, 3);
    cp[13] = QVector3D(1, 1, 3);
    cp[14] = QVector3D(2, -1, 3);
    cp[15] = QVector3D(3, 0, 3);

    camera.setCenter(QVector3D(1.5f, 0.0f, 1.5f));
    camera.setEye(QVector3D(-6.0f, 5.0f, 6.0f));

    QTimer *timer = new QTimer(this);
    connect(timer, SIGNAL(timeout()), this, SLOT(animate()));
    timer->start(0);

    qsrand(QDateTime::currentDateTime().toTime_t());
}

BezierWidget::~BezierWidget()
{
    freeVertices();
}

void BezierWidget::resizeGL(int width, int height)
{
    glViewport(0, 0, width, height);
}

void BezierWidget::initializeGL()
{
    if (!context.create())
        qFatal("Could not create OpenCL context");

    program = context.buildProgramFromSourceFile(QLatin1String(":/bezierpatch.cl"));
    evaluateBezier = program.createKernel("evaluateBezier");
    if (evaluateBezier.bestLocalWorkSizeImage2D().width() == 8)
        evaluateBezier.setLocalWorkSize(8, 8);

    glEnable(GL_DEPTH_TEST);

    textureImage = QImage(QLatin1String(":/qtlogo.png"));
    textureId = bindTexture(textureImage);
}

#if !defined(QT_OPENGL_ES_2)

static void setGLMatrix(GLenum type, const QMatrix4x4& m)
{
    glMatrixMode(type);
    if (sizeof(qreal) == sizeof(GLfloat)) {
        glLoadMatrixf(reinterpret_cast<const GLfloat *>(m.constData()));
    } else {
        GLfloat mat[16];
        const qreal *data = m.constData();
        for (int index = 0; index < 16; ++index)
            mat[index] = data[index];
        glLoadMatrixf(mat);
    }
}

#endif

void BezierWidget::paintGL()
{
#if !defined(QT_OPENGL_ES_2)
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

    if (useOpenCL)
        computeVerticesCL();
    else
        computeVerticesNative();

    QMatrix4x4 proj = camera.projectionMatrix(qreal(width()) / height());
    QMatrix4x4 mv = camera.modelViewMatrix();

    setGLMatrix(GL_PROJECTION, proj);
    setGLMatrix(GL_MODELVIEW, mv);

#ifdef USE_VBOS
    if (vertexBuffer) {
        vertexBuffer->bind();
        glVertexPointer(4, GL_FLOAT, 0, 0);
        vertexBuffer->release();
        texBuffer->bind();
        glTexCoordPointer(2, GL_FLOAT, 0, 0);
        texBuffer->release();
    } else {
        glVertexPointer(4, GL_FLOAT, 0, positions);
        glTexCoordPointer(2, GL_FLOAT, 0, texCoords);
    }
#else
    glVertexPointer(4, GL_FLOAT, 0, positions);
    glTexCoordPointer(2, GL_FLOAT, 0, texCoords);
#endif

    glEnableClientState(GL_VERTEX_ARRAY);
    glEnableClientState(GL_TEXTURE_COORD_ARRAY);

    glBindTexture(GL_TEXTURE_2D, textureId);
    glEnable(GL_TEXTURE_2D);

#ifdef USE_VBOS
    if (indexBuffer) {
        indexBuffer->bind();
        glDrawElements(GL_TRIANGLES, numIndices, IndexTypeEnum, 0);
        indexBuffer->release();
    } else {
        glDrawElements(GL_TRIANGLES, numIndices, IndexTypeEnum, indices);
    }
#else
    glDrawElements(GL_TRIANGLES, numIndices, IndexTypeEnum, indices);
#endif

    glDisable(GL_TEXTURE_2D);
#endif

    qreal fps = frameRate.fps();
    if (fps > 0.0f) {
        QPainter painter(this);
        QString str = QString::number(numIndices / 3) + QLatin1String(" triangles, ");
        str += QString::number(fps) + QLatin1String(" fps");
        painter.setPen(Qt::white);
        painter.drawText(rect(), str);
    }
}

void BezierWidget::setUseOpenCL(bool value)
{
    useOpenCL = value;
    freeVertices();
    frameRate.start();
}

void BezierWidget::setLocalWorkSize(int x, int y)
{
    evaluateBezier.setLocalWorkSize(x, y);
    frameRate.start();
}

static inline qreal clamp(qreal value, qreal lower, qreal upper)
{
    if (value < lower)
        return lower;
    else if (value > upper)
        return upper;
    else
        return value;
}

void BezierWidget::animate()
{
    // Move several of the non-corner control points up or down.
    static int mapPoints[12] = {1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 13, 14};
    for (int move = 0; move < 4; ++move) {
        int index = mapPoints[qrand() % 12];
        qreal amt = (qrand() & 1) ? 0.2f : -0.2f;
        qreal y = clamp(cp[index].y() + amt, -3.0f, 3.0f);
        cp[index].setY(y);
    }
    frameRate.newFrame();
    updateGL();
}

void BezierWidget::computeMatrices()
{
    QMatrix4x4 xm(cp[0].x(), cp[1].x(), cp[2].x(), cp[3].x(),
                  cp[4].x(), cp[5].x(), cp[6].x(), cp[7].x(),
                  cp[8].x(), cp[9].x(), cp[10].x(), cp[11].x(),
                  cp[12].x(), cp[13].x(), cp[14].x(), cp[15].x());
    QMatrix4x4 ym(cp[0].y(), cp[1].y(), cp[2].y(), cp[3].y(),
                  cp[4].y(), cp[5].y(), cp[6].y(), cp[7].y(),
                  cp[8].y(), cp[9].y(), cp[10].y(), cp[11].y(),
                  cp[12].y(), cp[13].y(), cp[14].y(), cp[15].y());
    QMatrix4x4 zm(cp[0].z(), cp[1].z(), cp[2].z(), cp[3].z(),
                  cp[4].z(), cp[5].z(), cp[6].z(), cp[7].z(),
                  cp[8].z(), cp[9].z(), cp[10].z(), cp[11].z(),
                  cp[12].z(), cp[13].z(), cp[14].z(), cp[15].z());

    matrixX = matrixM * xm * matrixM;
    matrixY = matrixM * ym * matrixM;
    matrixZ = matrixM * zm * matrixM;
}

void BezierWidget::computeVerticesNative()
{
    computeMatrices();
    allocVertices();

    QVector4D *posns;
    QVector2D *texcs;

#ifdef USE_VBOS
    if (vertexBuffer) {
        vertexBuffer->bind();
        void *mapped = vertexBuffer->map(QGLBuffer::WriteOnly);
        posns = (QVector4D *)mapped;
    } else {
        posns = positions;
    }
#else
    posns = positions;
#endif

    // Compute the positions.
    for (int sint = 0; sint < subdivisionSize; ++sint) {
        qreal s = qreal(sint) / (subdivisionSize - 1);
        QVector4D svec = QVector4D(s * s * s, s * s, s, 1);
        for (int tint = 0; tint < subdivisionSize; ++tint) {
            qreal t = qreal(tint) / (subdivisionSize - 1);
            QVector4D tvec = QVector4D(t * t * t, t * t, t, 1);
            qreal x = QVector4D::dotProduct(svec * matrixX, tvec);
            qreal y = QVector4D::dotProduct(svec * matrixY, tvec);
            qreal z = QVector4D::dotProduct(svec * matrixZ, tvec);
            int offset = sint + tint * subdivisionSize;
            posns[offset] = QVector4D(x, y, z, 1);
        }
    }

#ifdef USE_VBOS
    if (vertexBuffer) {
        vertexBuffer->unmap();
        vertexBuffer->release();
        texBuffer->bind();
        void *mapped = texBuffer->map(QGLBuffer::WriteOnly);
        texcs = (QVector2D *)mapped;
    } else {
        texcs = texCoords;
    }
#else
    texcs = texCoords;
#endif

    // Compute the texture co-ordinates.
    for (int sint = 0; sint < subdivisionSize; ++sint) {
        qreal s = qreal(sint) / (subdivisionSize - 1);
        for (int tint = 0; tint < subdivisionSize; ++tint) {
            qreal t = qreal(tint) / (subdivisionSize - 1);
            int offset = sint + tint * subdivisionSize;
            texcs[offset] = QVector2D(s, t);
        }
    }

#ifdef USE_VBOS
    if (texBuffer) {
        texBuffer->unmap();
        texBuffer->release();
    }
#endif
}

void BezierWidget::computeVerticesCL()
{
    computeMatrices();
    allocVertices();

#ifdef USE_VBOS
    if (vertexBuffer && context.supportsObjectSharing()) {
        // Acquire the vertex buffers from OpenGL.
        context.acquire(positionBuffer);
        context.acquire(texCoordBuffer);
    }
#endif

    evaluateBezier.setGlobalWorkSize(subdivisionSize, subdivisionSize);
    evaluateBezier(positionBuffer, texCoordBuffer,
                   matrixX, matrixY, matrixZ, subdivisionSize);

#ifdef USE_VBOS
    if (vertexBuffer && context.supportsObjectSharing()) {
        // Release the vertex buffers from OpenCL back to OpenGL.
        context.release(positionBuffer);
        context.release(texCoordBuffer).waitForFinished();
    } else if (vertexBuffer) {
        // Read the results directly into the vertex buffers.
        vertexBuffer->bind();
        GLfloat *mapped = (GLfloat *)vertexBuffer->map(QGLBuffer::WriteOnly);
        positionBuffer.read(mapped, numVertices * sizeof(GLfloat) * 4);
        vertexBuffer->unmap();
        vertexBuffer->release();
        texBuffer->bind();
        mapped = (GLfloat *)texBuffer->map(QGLBuffer::WriteOnly);
        texCoordBuffer.read(mapped, numVertices * sizeof(GLfloat) * 2);
        texBuffer->unmap();
        texBuffer->release();
    } else {
        positionBuffer.read(positions, numVertices * sizeof(GLfloat) * 4);
        texCoordBuffer.read(texCoords, numVertices * sizeof(GLfloat) * 2);
    }
#else
    positionBuffer.read(positions, numVertices * sizeof(GLfloat) * 4);
    texCoordBuffer.read(texCoords, numVertices * sizeof(GLfloat) * 2);
#endif
}

void BezierWidget::allocVertices()
{
    if (subdivisionSize == lastSubdivisionSize)
        return;
    lastSubdivisionSize = subdivisionSize;

    // Expand the vertex and index arrays.
    int num = subdivisionSize * subdivisionSize;
    if (num > numVertices) {
        positions = (QVector4D *)qRealloc(positions, num * sizeof(GLfloat) * 4);
        texCoords = (QVector2D *)qRealloc(texCoords, num * sizeof(GLfloat) * 2);

        int maxIndices = num * 6;
        indices = (IndexType *)qRealloc(indices, maxIndices * sizeof(IndexType));
    }
    numVertices = num;
#ifdef USE_VBOS
    // Create a vertex buffer in the server.
    if (!vertexBuffer) {
        vertexBuffer = new QGLBuffer(QGLBuffer::VertexBuffer);
        vertexBuffer->setUsagePattern(QGLBuffer::DynamicDraw);
        texBuffer = new QGLBuffer(QGLBuffer::VertexBuffer);
        texBuffer->setUsagePattern(QGLBuffer::DynamicDraw);
        if (!vertexBuffer->create() || !texBuffer->create()) {
            delete vertexBuffer;
            vertexBuffer = 0;
            delete texBuffer;
            texBuffer = 0;
        }
    }
    if (vertexBuffer) {
        vertexBuffer->bind();
        vertexBuffer->allocate(sizeof(GLfloat) * 4 * num);
        if (!vertexBuffer->map(QGLBuffer::WriteOnly)) {
            // No point using a vertex buffer if we cannot map it.
            vertexBuffer->unmap();
            vertexBuffer->release();
            delete vertexBuffer;
            vertexBuffer = 0;
            delete texBuffer;
            texBuffer = 0;
        } else {
            vertexBuffer->unmap();
            vertexBuffer->release();
        }
    }
    if (texBuffer) {
        texBuffer->bind();
        texBuffer->allocate(sizeof(GLfloat) * 2 * num);
        if (!texBuffer->map(QGLBuffer::WriteOnly)) {
            // No point using a vertex buffer if we cannot map it.
            texBuffer->unmap();
            texBuffer->release();
            delete vertexBuffer;
            vertexBuffer = 0;
            delete texBuffer;
            texBuffer = 0;
        } else {
            texBuffer->unmap();
            texBuffer->release();
        }
    }
#endif

    // Generate a new index array for drawing the triangles.
    numIndices = 0;
    for (int sint = 0; sint < (subdivisionSize - 1); ++sint) {
        for (int tint = 0; tint < (subdivisionSize - 1); ++tint) {
            int corner1 = sint + tint * subdivisionSize;
            int corner2 = sint + (tint + 1) * subdivisionSize;
            int corner3 = (sint + 1) + (tint + 1) * subdivisionSize;
            int corner4 = (sint + 1) + tint * subdivisionSize;
            indices[numIndices++] = corner1;
            indices[numIndices++] = corner2;
            indices[numIndices++] = corner3;
            indices[numIndices++] = corner1;
            indices[numIndices++] = corner3;
            indices[numIndices++] = corner4;
        }
    }
#ifdef USE_VBOS
    // Upload the indices into a server-side buffer.
    if (!indexBuffer) {
        indexBuffer = new QGLBuffer(QGLBuffer::IndexBuffer);
        if (!indexBuffer->create()) {
            delete indexBuffer;
            indexBuffer = 0;
        }
    }
    if (indexBuffer) {
        indexBuffer->bind();
        indexBuffer->allocate(indices, sizeof(IndexType) * numIndices);
        indexBuffer->release();
    }
#endif

    // Allocate OpenCL buffers of the right size, directly on top of
    // the OpenGL vertex buffers if possible.
#ifdef USE_VBOS
    if (context.supportsObjectSharing()) {
        positionBuffer = context.createGLBuffer
            (vertexBuffer, QCLMemoryObject::WriteOnly);
        texCoordBuffer = context.createGLBuffer
            (texBuffer, QCLMemoryObject::WriteOnly);
    } else
#endif
    {
        positionBuffer = context.createBufferDevice
            (num * sizeof(GLfloat) * 4, QCLMemoryObject::WriteOnly);
        texCoordBuffer = context.createBufferDevice
            (num * sizeof(GLfloat) * 2, QCLMemoryObject::WriteOnly);
    }
}

void BezierWidget::freeVertices()
{
    positionBuffer = QCLBuffer();
    texCoordBuffer = QCLBuffer();
    if (positions) {
        qFree(positions);
        positions = 0;
    }
    if (texCoords) {
        qFree(texCoords);
        texCoords = 0;
    }
    if (indices) {
        qFree(indices);
        indices = 0;
    }
#ifdef USE_VBOS
    delete vertexBuffer;
    delete texBuffer;
    delete indexBuffer;
    vertexBuffer = 0;
    texBuffer = 0;
    indexBuffer = 0;
#endif
    lastSubdivisionSize = -1;
    numVertices = 0;
    numIndices = 0;
}
