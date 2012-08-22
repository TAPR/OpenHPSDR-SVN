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
#include <QtGui/qpainter.h>

#include <QGraphicsView>
#include <QGraphicsItem>
#ifdef OPENVG_RAW_TEST
#include <QtOpenVG/qvg.h>

VGImage qPixmapToVGImage(const QPixmap& pixmap);
#endif
#include "pixmapfilterwidget.h"
#include "graphicseffectview.h"
#include "clwidget.h"

class PixmapFilterWidget;
class GraphicsEffectView;

enum blurAlgorithm
{
    AlgorithmPixmapFilter,
    AlgorithmGraphicsEffect,
    AlgorithmOpenCL
};

Q_DECLARE_METATYPE(blurAlgorithm);

class tst_Blur : public QObject
{
    Q_OBJECT
public:
    tst_Blur()
    {
        view = 0;
        pixmapFilterWidget = 0;
        clwidget = 0;
    }
    virtual ~tst_Blur()
    {
        delete view;
        delete pixmapFilterWidget;
        delete clwidget;
    }

private slots:
    void initTestCase();

    void blur_data();
    void blur();

    void openCLBlurAnimated();

    void animatedGraphicsEffectBlur_data();
    void animatedGraphicsEffectBlur();

    void pixmapFilterAnimatedBlur_data();
    void pixmapFilterAnimatedBlur();

private:
    void qPixmapFilterBlurFilter(int hint, int radius);
    void qGraphicsEffectBlur(int hint, int radius);
    void openCLBlur(int hint, int radius);

    PixmapFilterWidget *pixmapFilterWidget;
    GraphicsEffectView *view;
    CLWidget* clwidget;
};

// setup test widgets
void tst_Blur::initTestCase()
{
    // Pixmap Filter setup
    pixmapFilterWidget = new PixmapFilterWidget();
    pixmapFilterWidget->setFixedSize(300, 300);
    pixmapFilterWidget->show();
#ifdef Q_WS_X11
    qt_x11_wait_for_window_manager(pixmapFilterWidget);
#endif

    // GraphicsView Setup
    view = new GraphicsEffectView();
    view->setFixedSize(300, 300);
    view->show();
#ifdef Q_WS_X11
    qt_x11_wait_for_window_manager(view);
#endif

    // OpenCL blur setup
    clwidget = new CLWidget;
    clwidget->setFixedSize(300,300);
    clwidget->show();
#ifdef Q_WS_X11
    qt_x11_wait_for_window_manager(clwidget);
#endif

    QTest::qWait(200);
}

// Generate iteration data for all blur benchmarks
void tst_Blur::blur_data()
{
    QTest::addColumn<int>("hint");
    QTest::addColumn<int>("radius");
    QTest::addColumn<blurAlgorithm>("algorithm");

    // Add the openCL benchmark
    for(int i = 0; i <= 16; i++)
    {
        QTest::newRow("openCL--" + QString::number(i).toAscii())
                << 0 << i << AlgorithmOpenCL;
    }

    QTest::newRow("pixmapFilterPerformance--0") << int(QGraphicsBlurEffect::PerformanceHint) << 0 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--1") << int(QGraphicsBlurEffect::PerformanceHint) << 1 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--2") << int(QGraphicsBlurEffect::PerformanceHint) << 2 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--3") << int(QGraphicsBlurEffect::PerformanceHint) << 3 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--4") << int(QGraphicsBlurEffect::PerformanceHint) << 4 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--5") << int(QGraphicsBlurEffect::PerformanceHint) << 5 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--6") << int(QGraphicsBlurEffect::PerformanceHint) << 6 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--7") << int(QGraphicsBlurEffect::PerformanceHint) << 7 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--8") << int(QGraphicsBlurEffect::PerformanceHint) << 8 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--9") << int(QGraphicsBlurEffect::PerformanceHint) << 9 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--10") << int(QGraphicsBlurEffect::PerformanceHint) << 10 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--11") << int(QGraphicsBlurEffect::PerformanceHint) << 11 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--12") << int(QGraphicsBlurEffect::PerformanceHint) << 12 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--13") << int(QGraphicsBlurEffect::PerformanceHint) << 13 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--14") << int(QGraphicsBlurEffect::PerformanceHint) << 14 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--15") << int(QGraphicsBlurEffect::PerformanceHint) << 15 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterPerformance--16") << int(QGraphicsBlurEffect::PerformanceHint) << 16 << AlgorithmPixmapFilter;

    QTest::newRow("pixmapFilterQuality--0") << int(QGraphicsBlurEffect::QualityHint) << 0 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--1") << int(QGraphicsBlurEffect::QualityHint) << 1 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--2") << int(QGraphicsBlurEffect::QualityHint) << 2 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--3") << int(QGraphicsBlurEffect::QualityHint) << 3 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--4") << int(QGraphicsBlurEffect::QualityHint) << 4 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--5") << int(QGraphicsBlurEffect::QualityHint) << 5 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--6") << int(QGraphicsBlurEffect::QualityHint) << 6 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--7") << int(QGraphicsBlurEffect::QualityHint) << 7 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--8") << int(QGraphicsBlurEffect::QualityHint) << 8 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--9") << int(QGraphicsBlurEffect::QualityHint) << 9 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--10") << int(QGraphicsBlurEffect::QualityHint) << 10 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--11") << int(QGraphicsBlurEffect::QualityHint) << 11 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--12") << int(QGraphicsBlurEffect::QualityHint) << 12 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--13") << int(QGraphicsBlurEffect::QualityHint) << 13 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--14") << int(QGraphicsBlurEffect::QualityHint) << 14 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--15") << int(QGraphicsBlurEffect::QualityHint) << 15 << AlgorithmPixmapFilter;
    QTest::newRow("pixmapFilterQuality--16") << int(QGraphicsBlurEffect::QualityHint) << 16 << AlgorithmPixmapFilter;

    QTest::newRow("qGraphicsViewPerformance--0") << int(QGraphicsBlurEffect::PerformanceHint) << 0 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--1") << int(QGraphicsBlurEffect::PerformanceHint) << 1 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--2") << int(QGraphicsBlurEffect::PerformanceHint) << 2 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--3") << int(QGraphicsBlurEffect::PerformanceHint) << 3 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--4") << int(QGraphicsBlurEffect::PerformanceHint) << 4 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--5") << int(QGraphicsBlurEffect::PerformanceHint) << 5 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--6") << int(QGraphicsBlurEffect::PerformanceHint) << 6 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--7") << int(QGraphicsBlurEffect::PerformanceHint) << 7 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--8") << int(QGraphicsBlurEffect::PerformanceHint) << 8 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--9") << int(QGraphicsBlurEffect::PerformanceHint) << 9 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--10") << int(QGraphicsBlurEffect::PerformanceHint) << 10 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--11") << int(QGraphicsBlurEffect::PerformanceHint) << 11 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--12") << int(QGraphicsBlurEffect::PerformanceHint) << 12 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--13") << int(QGraphicsBlurEffect::PerformanceHint) << 13 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--14") << int(QGraphicsBlurEffect::PerformanceHint) << 14 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--15") << int(QGraphicsBlurEffect::PerformanceHint) << 15 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewPerformance--16") << int(QGraphicsBlurEffect::PerformanceHint) << 16 << AlgorithmGraphicsEffect;

    QTest::newRow("qGraphicsViewQuality--0") << int(QGraphicsBlurEffect::QualityHint) << 0 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--1") << int(QGraphicsBlurEffect::QualityHint) << 1 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--2") << int(QGraphicsBlurEffect::QualityHint) << 2 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--3") << int(QGraphicsBlurEffect::QualityHint) << 3 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--4") << int(QGraphicsBlurEffect::QualityHint) << 4 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--5") << int(QGraphicsBlurEffect::QualityHint) << 5 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--6") << int(QGraphicsBlurEffect::QualityHint) << 6 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--7") << int(QGraphicsBlurEffect::QualityHint) << 7 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--8") << int(QGraphicsBlurEffect::QualityHint) << 8 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--9") << int(QGraphicsBlurEffect::QualityHint) << 9 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--10") << int(QGraphicsBlurEffect::QualityHint) << 10 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--11") << int(QGraphicsBlurEffect::QualityHint) << 11 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--12") << int(QGraphicsBlurEffect::QualityHint) << 12 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--13") << int(QGraphicsBlurEffect::QualityHint) << 13 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--14") << int(QGraphicsBlurEffect::QualityHint) << 14 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--15") << int(QGraphicsBlurEffect::QualityHint) << 15 << AlgorithmGraphicsEffect;
    QTest::newRow("qGraphicsViewQuality--16") << int(QGraphicsBlurEffect::QualityHint) << 16 << AlgorithmGraphicsEffect;
}

void tst_Blur::blur()
{    
    QFETCH(int, hint);
    QFETCH(int, radius);
    QFETCH(blurAlgorithm, algorithm);

    switch(algorithm)

    {
    case AlgorithmPixmapFilter:
        qPixmapFilterBlurFilter(hint, radius);
        break;

    case AlgorithmGraphicsEffect:
        qGraphicsEffectBlur(hint, radius);
        break;

    case AlgorithmOpenCL:
        openCLBlur(hint, radius);
        break;
    }
}

// Test the performance of the pixmap filter blur filter.
void tst_Blur::qPixmapFilterBlurFilter(int hint, int radius)
{
    QPixmapBlurFilter filter;
    filter.setBlurHints(QGraphicsBlurEffect::BlurHints(hint));
    filter.setRadius(radius);

    pixmapFilterWidget->setPixmapOffset(-1);

    QBENCHMARK {
        QEventLoop eventLoop;
        pixmapFilterWidget->setFilter(&filter);
        pixmapFilterWidget->setEventLoop(&eventLoop);
        pixmapFilterWidget->update();
        eventLoop.exec();
    }

    pixmapFilterWidget->setPixmapOffset(-1);
    pixmapFilterWidget->setFilter(0);

}

// Test the performance of the graphics effect blur.
void tst_Blur::qGraphicsEffectBlur(int hint, int radius)
{
    for (int index = 0; index < 9; ++index) {
        QGraphicsBlurEffect *effect = new QGraphicsBlurEffect();
        effect->setBlurHints(QGraphicsBlurEffect::BlurHints(hint));
        effect->setBlurRadius(radius);
        view->setItemEffect(index, effect);
    }

    QBENCHMARK {
        QEventLoop eventLoop;
        view->setEventLoop(&eventLoop);
        view->updateScene();
        eventLoop.exec();
    }
}

// Test the performance of the opencl blur.
void tst_Blur::openCLBlur(int hint, int radius)
{
    Q_UNUSED(hint);
    clwidget->setup(radius);

    QBENCHMARK {
        QEventLoop eventLoop;
        clwidget->setEventLoop(&eventLoop);
        clwidget->startBlur(radius);
        clwidget->update();
        eventLoop.exec();
    }
}

void tst_Blur::openCLBlurAnimated()
{
    int startRadius = 0;
    int finishRadius = 16;
    bool animateUnblur = true;
    clwidget->setup(qMax(startRadius, finishRadius));

    int d = startRadius < finishRadius? 1 : -1;
    QBENCHMARK {
        for(int i = startRadius; i != finishRadius + d; i += d)
        {
            QEventLoop eventLoop;
            clwidget->setEventLoop(&eventLoop);
            clwidget->startBlur(i);
            clwidget->update();
            eventLoop.exec();
        };
        if(animateUnblur)
        {
            d = -d;
            for(int i = finishRadius + d; i != startRadius + d; i += d)
            {
                QEventLoop eventLoop;
                clwidget->setEventLoop(&eventLoop);
                clwidget->startBlur(i);
                clwidget->update();
                eventLoop.exec();
            };
        }
    }
    qWarning() << "divide by 33";
}

void tst_Blur::animatedGraphicsEffectBlur_data()
{
    QTest::addColumn<int>("hint");
    QTest::newRow("performance") << int(QGraphicsBlurEffect::PerformanceHint);
    QTest::newRow("quality") << int(QGraphicsBlurEffect::QualityHint);
}

// Test the performance of animating a blur from 0 to 16 and back.
void tst_Blur::animatedGraphicsEffectBlur()
{
    QFETCH(int, hint);

    for (int index = 0; index < 9; ++index) {
        QGraphicsBlurEffect *effect = new QGraphicsBlurEffect();
        effect->setBlurHints(QGraphicsBlurEffect::BlurHints(hint));
        view->setItemEffect(index, effect);
    }

    // Note: divide reported msec value by 33 to get per-frame msec value.
    QBENCHMARK {
        for (int radius = 0; radius <= 16; ++radius) {
            QEventLoop eventLoop;
            for (int index = 0; index < 9; ++index) {
                QGraphicsBlurEffect *blur =
                    qobject_cast<QGraphicsBlurEffect *>
                        (view->itemEffect(index));
                blur->setBlurRadius(radius);
            }
            view->setEventLoop(&eventLoop);
            view->updateScene();
            eventLoop.exec();
        }
        for (int radius = 15; radius >= 0; --radius) {
            QEventLoop eventLoop;
            for (int index = 0; index < 9; ++index) {
                QGraphicsBlurEffect *blur =
                    qobject_cast<QGraphicsBlurEffect *>
                        (view->itemEffect(index));
                blur->setBlurRadius(radius);
            }
            view->setEventLoop(&eventLoop);
            view->updateScene();
            eventLoop.exec();
        }
    }
    qWarning() << "divide by 33";
}

void tst_Blur::pixmapFilterAnimatedBlur_data()
{
    QTest::addColumn<int>("hint");
    QTest::newRow("performance") << int(QGraphicsBlurEffect::PerformanceHint |
                                        QGraphicsBlurEffect::AnimationHint);
    QTest::newRow("quality") << int(QGraphicsBlurEffect::QualityHint |
                                    QGraphicsBlurEffect::AnimationHint);
}

// Test the performance of animating a blur from 0 to 16 and back.
void tst_Blur::pixmapFilterAnimatedBlur()
{
    QFETCH(int, hint);

    QPixmapBlurFilter filter;
    filter.setBlurHints(QGraphicsBlurEffect::BlurHints(hint));

    // Note: divide reported msec value by 33 to get per-frame msec value.
    QBENCHMARK {
        for (int radius = 0; radius <= 16; ++radius) {
            QEventLoop eventLoop;
            filter.setRadius(radius);
            pixmapFilterWidget->setFilter(&filter);
            pixmapFilterWidget->setEventLoop(&eventLoop);
            pixmapFilterWidget->update();
            eventLoop.exec();
        }
        for (int radius = 15; radius >= 0; --radius) {
            QEventLoop eventLoop;
            filter.setRadius(radius);
            pixmapFilterWidget->setFilter(&filter);
            pixmapFilterWidget->setEventLoop(&eventLoop);
            pixmapFilterWidget->update();
            eventLoop.exec();
        }
    }
    qWarning() << "divide by 33";
}
QTEST_MAIN(tst_Blur)

#include "tst_blur.moc"
