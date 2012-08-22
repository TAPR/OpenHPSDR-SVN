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

#include "zoom.h"
#include "image.h"

Zoom::Zoom()
{
}

Zoom::~Zoom()
{
}

void Zoom::addStep(double centerx, double centery,
                   double hdiameter, int iterations)
{
    ZoomStep step;
    step.centerx = centerx;
    step.centery = centery;
    step.hdiameter = hdiameter;
    step.iterations = iterations;
    steps.append(step);
}

static inline double interpolate(double x, double y, double amt)
{
    return x + (y - x) * amt;
}

static inline int interpolate(int x, int y, double amt)
{
    return qRound(x + (y - x) * amt);
}

void Zoom::generate(Image *image, qreal value, const Palette& palette)
{
    Q_ASSERT(steps.size() >= 2);
    int posn, iterations;
    if (value <= 0.0f) {
        image->setRegion(steps[0].centerx, steps[0].centery,
                         steps[0].hdiameter);
        iterations = steps[0].iterations;
    } else if (value >= 1.0f) {
        posn = steps.size() - 1;
        image->setRegion(steps[posn].centerx, steps[posn].centery,
                         steps[posn].hdiameter);
        iterations = steps[posn].iterations;
    } else {
        int groups = steps.size() - 1;
        posn = int(value * groups);
        double lower = double(posn) / groups;
        double upper = double(posn + 1) / groups;
        double amt = (value - lower) / (upper - lower);
        image->setRegion
            (interpolate(steps[posn].centerx, steps[posn + 1].centerx, amt),
             interpolate(steps[posn].centery, steps[posn + 1].centery, amt),
             interpolate(steps[posn].hdiameter, steps[posn + 1].hdiameter, amt));
        iterations = interpolate(steps[posn].iterations,
                                 steps[posn + 1].iterations, amt);
    }
    image->generate(iterations, palette);
}

WikipediaZoom::WikipediaZoom()
{
    // This table defines the steps in the 14-step Mandelbrot zoom at
    // http://commons.wikimedia.org/wiki/Mandelbrot_set
    //
    // With float precision, step 7 is roughly at the rounding error limit.
    addStep(-.7, 0, 3.0769, 200);              // Starting point
    addStep(-.87591, .20464, .53184, 200);     // Step 1
    addStep(-.759856, .125547, .051579, 500);
    addStep(-.743030, .126433, .016110, 700);
    addStep(-.7435669, .1314023, .0022878, 700);
    addStep(-.74364990, .13188204, .00073801, 2000);
    addStep(-.74364085, .13182733, .00012068, 2000);
    addStep(-.743643135, .131825963, .000014628, 2000); // Step 7
#if 0
    addStep(-.7436447860, .1318252536, .0000029336, 2000);
    addStep(-.74364409961, .13182604688, .00000066208, 2000);
    addStep(-.74364386269, .13182590271, .00000013526, 2000);
    addStep(-.743643900055, .131825890901, .000000049304, 2000);
    addStep(-.7436438885706, .1318259043124, .0000000041493, 2000);
    addStep(-.74364388717342, .13182590425182, .00000000059849, 2000);
    addStep(-.743643887037151, .131825904205330, .000000000051299, 2000); // Step 14
#endif
}

GoldenGradientZoom::GoldenGradientZoom()
{
    // Golden Gradient zoom from:
    // http://commons.wikimedia.org/wiki/File:Mandelbrot_sequence_new.gif
    double centerx = -0.743643887037158704752191506114774;
    double centery = 0.131825904205311970493132056385139;
    double diameter = 4.0;
    addStep(centerx, centery, diameter, 200);
    addStep(centerx, centery, diameter / 2, 200);
    addStep(centerx, centery, diameter / 4, 500);
    addStep(centerx, centery, diameter / 8, 300);
    addStep(centerx, centery, diameter / 16, 300);
    addStep(centerx, centery, diameter / 32, 500);
    addStep(centerx, centery, diameter / 64, 1000);
    addStep(centerx, centery, diameter / 128, 1000);
    addStep(centerx, centery, diameter / 256, 1000);
    addStep(centerx, centery, diameter / 512, 1000);
    addStep(centerx, centery, diameter / 1024, 1000);
    addStep(centerx, centery, diameter / 2048, 2000);
    addStep(centerx, centery, diameter / 4096, 2000);
    addStep(centerx, centery, diameter / 8192, 2000);
    addStep(centerx, centery, diameter / 16384, 2000);
    addStep(centerx, centery, diameter / 32768, 2000);
    addStep(centerx, centery, diameter / 65536, 2000);
    addStep(centerx, centery, diameter / 131072, 2000);
}
