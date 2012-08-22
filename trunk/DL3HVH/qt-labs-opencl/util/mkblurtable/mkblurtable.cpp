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

#include <QtCore/qvector.h>
#include <QtCore/qmath.h>
#include <stdio.h>

static const qreal Q_2PI = qreal(6.28318530717958647693); // 2*pi

static inline qreal gaussian(qreal dx, qreal sigma)
{
    return exp(-dx * dx / (2 * sigma * sigma)) / (Q_2PI * sigma * sigma);
}

int main(int argc, char **argv)
{
    Q_UNUSED(argc);
    Q_UNUSED(argv);
    QVector<int> sizes;
    sizes.append(0);
    for (int radius = 1; radius <= 16; ++radius) {
        // Calculate the Gaussian blur weights and offsets.
        QVector<qreal> components;
        QVector<float> offsets;
        QVector<float> weights;
        qreal sigma = radius / 1.65;
        qreal sum = 0;
        for (int i = -radius; i <= radius; ++i) {
            qreal value = gaussian(i, sigma);
            components.append(value);
            sum += value;
        }
        for (int i = 0; i < components.size(); ++i)
            components[i] /= sum;
        for (int i = 0; i < components.size() - 1; i += 2) {
            qreal weight = components[i] + components[i + 1];
            qreal offset = i - radius + components[i + 1] / weight;
            offsets.append(offset);
            weights.append(weight);
        }
        // odd size ?
        if (components.size() & 1) {
            offsets.append(radius);
            weights.append(components[components.size() - 1]);
        }

        printf("static float const blurOffsets%d[] = {\n", radius);
        for (int index = 0; index < offsets.size(); ++index) {
            if (!index)
                printf("    ");
            else if ((index % 6) == 0)
                printf(",\n    ");
            else
                printf(", ");
            printf("%g", offsets[index]);
        }
        printf("\n};\n");
        printf("static float const blurWeights%d[] = {\n", radius);
        for (int index = 0; index < weights.size(); ++index) {
            if (!index)
                printf("    ");
            else if ((index % 6) == 0)
                printf(",\n    ");
            else
                printf(", ");
            printf("%g", weights[index]);
        }
        printf("\n};\n\n");
        sizes.append(weights.size());
    }

    printf("static const float * const blurOffsets[] = {\n");
    printf("    0,\n");
    for (int index = 1; index < 16; ++index)
        printf("    blurOffsets%d,\n", index);
    printf("    blurOffsets16\n};\n\n");

    printf("static const float * const blurWeights[] = {\n");
    printf("    0,\n");
    for (int index = 1; index < 16; ++index)
        printf("    blurWeights%d,\n", index);
    printf("    blurWeights16\n};\n\n");

    printf("static int const blurSizes[] = {\n    ");
    for (int index = 0; index < sizes.size() - 1; ++index)
        printf("%d, ", sizes[index]);
    printf("%d\n};\n\n", sizes[sizes.size() - 1]);

    return 0;
}
