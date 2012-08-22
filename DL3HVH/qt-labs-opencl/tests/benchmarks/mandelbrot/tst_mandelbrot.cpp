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

#include <QtTest/QtTest>
#include <QtGui/qimage.h>
#include <QtGui/qcolor.h>
#include <QtCore/qvarlengtharray.h>
#include "qclcontext.h"

const int Test_Width = 1024;
const int Test_Height = 1024;
const int Test_MaxIterations = 512;

class tst_Mandelbrot : public QObject
{
    Q_OBJECT
public:
    tst_Mandelbrot() {}
    virtual ~tst_Mandelbrot() {}

private slots:
    void initTestCase();
    void plain_data();
    void plain();
    void buildProgram();
    void openclPerPixel_data();
    void openclPerPixel();
    void openclPerPixelHost_data();
    void openclPerPixelHost();
    void openclPerGroup_data();
    void openclPerGroup();
    void openclImage_data();
    void openclImage();

private:
    QCLContext context;
    QCLProgram program;
    QCLKernel perPixelKernel;
    QCLKernel perGroupKernel;
    QCLKernel imageKernel;
    QVarLengthArray<QRgb> rgbColors;
    QVarLengthArray<float> floatColors;
};

void tst_Mandelbrot::initTestCase()
{
    QVERIFY(context.create());

    // Create a color table to use when generating the pixel values.
    for (int index = 0; index < Test_MaxIterations; ++index) {
        qreal amt = qreal(index) / (Test_MaxIterations - 1);
        QColor color = QColor::fromHsvF(amt, 1.0f, 1.0f);
        rgbColors.append(color.rgb());
        floatColors.append(color.redF());
        floatColors.append(color.greenF());
        floatColors.append(color.blueF());
        floatColors.append(color.alphaF());
    }
}

// Generate Mandelbrot iteration data using plain C++ and no acceleration.
void tst_Mandelbrot::plain_data()
{
    QTest::addColumn<qreal>("centerx");
    QTest::addColumn<qreal>("centery");
    QTest::addColumn<qreal>("diameter");
    QTest::addColumn<int>("workSize");

    // Defines some of the steps in the 14-step Mandelbrot zoom at
    // http://commons.wikimedia.org/wiki/Mandelbrot_set
    QTest::newRow("full")
        << qreal(-0.7f) << qreal(0.0f) << qreal(3.0769f) << 0;
    QTest::newRow("zoom1")
        << qreal(-0.87591f) << qreal(0.20464f) << qreal(0.53184f) << 0;
    QTest::newRow("zoom2")
        << qreal(-0.759856f) << qreal(0.125547f) << qreal(0.051579f) << 0;
    QTest::newRow("full_8x8")
        << qreal(-0.7f) << qreal(0.0f) << qreal(3.0769f) << 8;
    QTest::newRow("zoom1_8x8")
        << qreal(-0.87591f) << qreal(0.20464f) << qreal(0.53184f) << 8;
    QTest::newRow("zoom2_8x8")
        << qreal(-0.759856f) << qreal(0.125547f) << qreal(0.051579f) << 8;
}
void tst_Mandelbrot::plain()
{
    QFETCH(qreal, centerx);
    QFETCH(qreal, centery);
    QFETCH(qreal, diameter);

    QRectF region(centerx - diameter / 2, centery - diameter / 2,
                  diameter, diameter);

    QImage data(Test_Width, Test_Height, QImage::Format_RGB32);
    QBENCHMARK {
        int width = data.width();
        int height = data.height();
        float xstep = region.width() / width;
        float ystep = region.height() / height;
        float yin = region.y();
        uint *line = reinterpret_cast<uint *>(data.bits());
        int stride = data.bytesPerLine() / sizeof(uint);
        for (int ypos = 0; ypos < height; ++ypos, yin += ystep) {
            float xin = region.x();
            for (int xpos = 0; xpos < width; ++xpos, xin += xstep) {
                int iteration = 0;
                float x = 0;
                float y = 0;
                while (iteration < Test_MaxIterations) {
                    float x2 = x * x;
                    float y2 = y * y;
                    if ((x2 + y2) > (2.0f * 2.0f))
                        break;
                    float xtemp = x2 - y2 + xin;
                    y = 2 * x * y + yin;
                    x = xtemp;
                    ++iteration;
                }
                line[xpos] = iteration;
            }
            line += stride;
        }
    }
}

void tst_Mandelbrot::buildProgram()
{
    QBENCHMARK_ONCE {
        program = context.buildProgramFromSourceFile
            (QLatin1String(":/mandelbrot.cl"));
        perPixelKernel = program.createKernel("mandelbrot_per_pixel");
        perGroupKernel = program.createKernel("mandelbrot_per_group");
        imageKernel = program.createKernel("mandelbrot_image2d");
    }
}

// Generate Mandelbrot iteration data using a per-pixel OpenCL kernel.
void tst_Mandelbrot::openclPerPixel_data()
{
    plain_data();
}
void tst_Mandelbrot::openclPerPixel()
{
    QFETCH(qreal, centerx);
    QFETCH(qreal, centery);
    QFETCH(qreal, diameter);
    QFETCH(int, workSize);

    QVERIFY(!perPixelKernel.isNull());

    QRectF region(centerx - diameter / 2, centery - diameter / 2,
                  diameter, diameter);

    QCLBuffer data = context.createBufferDevice
        (Test_Width * Test_Height * sizeof(int), QCLMemoryObject::WriteOnly);
    perPixelKernel.setGlobalWorkSize(Test_Width, Test_Height);
    perPixelKernel.setLocalWorkSize(workSize, workSize);

    QCLBuffer colors = context.createBufferCopy
        (rgbColors.constData(), sizeof(QRgb) * Test_MaxIterations,
         QCLMemoryObject::ReadOnly);

    QBENCHMARK {
        QCLEvent event = perPixelKernel
            (data, colors, float(region.x()), float(region.y()),
             float(region.width()), float(region.height()),
             Test_Width, Test_Height, Test_MaxIterations);
        event.waitForFinished();
    }
}

// Generate Mandelbrot iteration data using a per-pixel OpenCL kernel,
// but write into host-accessible memory.
void tst_Mandelbrot::openclPerPixelHost_data()
{
    plain_data();
}
void tst_Mandelbrot::openclPerPixelHost()
{
    QFETCH(qreal, centerx);
    QFETCH(qreal, centery);
    QFETCH(qreal, diameter);
    QFETCH(int, workSize);

    QVERIFY(!perPixelKernel.isNull());

    QRectF region(centerx - diameter / 2, centery - diameter / 2,
                  diameter, diameter);

    QCLBuffer data = context.createBufferHost
        (0, Test_Width * Test_Height * sizeof(int), QCLMemoryObject::WriteOnly);
    perPixelKernel.setGlobalWorkSize(Test_Width, Test_Height);
    perPixelKernel.setLocalWorkSize(workSize, workSize);

    QCLBuffer colors = context.createBufferCopy
        (rgbColors.constData(), sizeof(QRgb) * Test_MaxIterations,
         QCLMemoryObject::ReadOnly);

    QBENCHMARK {
        QCLEvent event = perPixelKernel
            (data, colors, float(region.x()), float(region.y()),
             float(region.width()), float(region.height()),
             Test_Width, Test_Height, Test_MaxIterations);
        event.waitForFinished();

        // Force a copy-back to host memory by mapping it.
        void *ptr = data.map(QCLMemoryObject::WriteOnly);
        data.unmap(ptr);
    }
}

// Generate Mandelbrot iteration data using a 16x16 per-group OpenCL kernel.
// This tests whether doing more pixels per kernel call helps or hinders.
void tst_Mandelbrot::openclPerGroup_data()
{
    plain_data();
}
void tst_Mandelbrot::openclPerGroup()
{
    QFETCH(qreal, centerx);
    QFETCH(qreal, centery);
    QFETCH(qreal, diameter);
    QFETCH(int, workSize);

    QVERIFY(!perGroupKernel.isNull());

    QRectF region(centerx - diameter / 2, centery - diameter / 2,
                  diameter, diameter);

    QCLBuffer data = context.createBufferDevice
        (Test_Width * Test_Height * sizeof(int), QCLMemoryObject::WriteOnly);
    perGroupKernel.setGlobalWorkSize(Test_Width / 16, Test_Height / 16);
    perGroupKernel.setLocalWorkSize(workSize, workSize);

    QCLBuffer colors = context.createBufferCopy
        (rgbColors.constData(), sizeof(QRgb) * Test_MaxIterations,
         QCLMemoryObject::ReadOnly);

    QBENCHMARK {
        QCLEvent event = perGroupKernel
            (data, colors, float(region.x()), float(region.y()),
             float(region.width()), float(region.height()),
             Test_Width, Test_Height, Test_MaxIterations, 16);
        event.waitForFinished();
    }
}

// Generate Mandelbrot iteration data using a per-pixel OpenCL kernel.
// The data is written to a 2D OpenCL image object rather than a buffer.
// This test uses a float color table.
void tst_Mandelbrot::openclImage_data()
{
    plain_data();
}
void tst_Mandelbrot::openclImage()
{
    QFETCH(qreal, centerx);
    QFETCH(qreal, centery);
    QFETCH(qreal, diameter);
    QFETCH(int, workSize);

    QVERIFY(!imageKernel.isNull());

    QRectF region(centerx - diameter / 2, centery - diameter / 2,
                  diameter, diameter);

    QCLImageFormat format(QCLImageFormat::Order_RGBA,
                          QCLImageFormat::Type_Normalized_UInt8);
    QCLImage2D data = context.createImage2DDevice
        (format, QSize(Test_Width, Test_Height), QCLMemoryObject::WriteOnly);
    imageKernel.setGlobalWorkSize(Test_Width, Test_Height);
    imageKernel.setLocalWorkSize(workSize, workSize);

    QCLBuffer colorBuffer = context.createBufferCopy
        (floatColors.constData(), sizeof(float) * 4 * Test_MaxIterations,
         QCLMemoryObject::ReadOnly);

    QBENCHMARK {
        QCLEvent event = imageKernel
            (data, colorBuffer, float(region.x()), float(region.y()),
             float(region.width()), float(region.height()),
             Test_Width, Test_Height, Test_MaxIterations);
        event.waitForFinished();
    }
}

QTEST_MAIN(tst_Mandelbrot)

#include "tst_mandelbrot.moc"
