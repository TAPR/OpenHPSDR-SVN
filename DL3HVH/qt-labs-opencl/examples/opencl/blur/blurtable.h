/****************************************************************************
**
** Copyright (C) 2010 Nokia Corporation and/or its subsidiary(-ies).
** All rights reserved.
** Contact: Nokia Corporation (qt-info@nokia.com)
**
** This file is part of the examples of the Qt Toolkit.
**
** $QT_BEGIN_LICENSE:BSD$
** You may use this file under the terms of the BSD license as follows:
**
** "Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions are
** met:
**   * Redistributions of source code must retain the above copyright
**     notice, this list of conditions and the following disclaimer.
**   * Redistributions in binary form must reproduce the above copyright
**     notice, this list of conditions and the following disclaimer in
**     the documentation and/or other materials provided with the
**     distribution.
**   * Neither the name of Nokia Corporation and its Subsidiary(-ies) nor
**     the names of its contributors may be used to endorse or promote
**     products derived from this software without specific prior written
**     permission.
**
** THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
** "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
** LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
** A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
** OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
** SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
** LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
** OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE."
** $QT_END_LICENSE$
**
****************************************************************************/

#ifndef BLURTABLE_H
#define BLURTABLE_H

// Pre-computed Gaussian blur offsets and weights for radii 1..16.

static float const blurOffsets1[] = {
    -0.204037, 1
};
static float const blurWeights1[] = {
    0.830539, 0.169461
};

static float const blurOffsets2[] = {
    -1.26484, 0.415734, 2
};
static float const blurWeights2[] = {
    0.329687, 0.582997, 0.087316
};

static float const blurOffsets3[] = {
    -2.31946, -0.462259, 1.38847, 3
};
static float const blurWeights3[] = {
    0.185568, 0.430063, 0.325087, 0.0592818
};

static float const blurOffsets4[] = {
    -3.35536, -1.43654, 0.478743, 2.39523, 4
};
static float const blurWeights4[] = {
    0.126493, 0.285829, 0.336411, 0.206316, 0.0449509
};

static float const blurOffsets5[] = {
    -4.37988, -2.43235, -0.486391, 1.45925, 3.40585, 5
};
static float const blurWeights5[] = {
    0.0953448, 0.200199, 0.275104, 0.24745, 0.145682, 0.0362198
};

static float const blurOffsets6[] = {
    -5.39749, -3.43421, -1.47167, 0.490548, 2.45287, 4.41573,
    6
};
static float const blurWeights6[] = {
    0.0763185, 0.148829, 0.215681, 0.232293, 0.185936, 0.110606,
    0.0303358
};

static float const blurOffsets7[] = {
    -6.41068, -4.43782, -2.46533, -0.493055, 1.47918, 3.45154,
    5.42419, 7
};
static float const blurWeights7[] = {
    0.0635512, 0.116117, 0.170399, 0.200841, 0.190133, 0.144571,
    0.0882898, 0.0260993
};

static float const blurOffsets8[] = {
    -7.42091, -5.44177, -3.46285, -1.48405, 0.494683, 2.47344,
    4.45229, 6.43131, 8
};
static float const blurWeights8[] = {
    0.0544121, 0.0940428, 0.137352, 0.169521, 0.176808, 0.155836,
    0.11607, 0.0730552, 0.0229025
};

static float const blurOffsets9[] = {
    -8.42906, -6.4456, -4.46226, -2.47901, -0.495799, 1.4874,
    3.47062, 5.45392, 7.43731, 9
};
static float const blurWeights9[] = {
    0.0475557, 0.0784024, 0.113123, 0.142848, 0.157869, 0.152695,
    0.129256, 0.0957588, 0.0620875, 0.0204042
};

static float const blurOffsets10[] = {
    -9.4357, -7.44913, -5.46264, -3.4762, -1.48979, 0.496597,
    2.48299, 4.46941, 6.45587, 8.4424, 10
};
static float const blurWeights10[] = {
    0.0422257, 0.0668679, 0.0950346, 0.121219, 0.138767, 0.142571,
    0.131462, 0.108792, 0.0808019, 0.0538604, 0.0183977
};

static float const blurOffsets11[] = {
    -10.4412, -8.45233, -6.4635, -4.47471, -2.48594, -0.497188,
    1.49156, 3.48032, 5.4691, 7.45791, 9.44676, 11
};
static float const blurWeights11[] = {
    0.0379654, 0.0580776, 0.0812383, 0.103907, 0.121524, 0.129961,
    0.127085, 0.113635, 0.0929098, 0.0694614, 0.0474851, 0.0167508
};

static float const blurOffsets12[] = {
    -11.4459, -9.45522, -7.46461, -5.47403, -3.48346, -1.49291,
    0.497637, 2.48819, 4.47874, 6.46932, 8.45991, 10.4505,
    12
};
static float const blurWeights12[] = {
    0.0344833, 0.0511946, 0.0704938, 0.0900303, 0.106644, 0.117165,
    0.119391, 0.112838, 0.098912, 0.0804185, 0.0606421, 0.0424134,
    0.0153746
};

static float const blurOffsets13[] = {
    -12.4498, -10.4578, -8.46582, -6.47385, -4.48188, -2.48993,
    -0.497986, 1.49396, 3.48591, 5.47786, 7.46983, 9.46181,
    11.4538, 13
};
static float const blurWeights13[] = {
    0.0315844, 0.0456815, 0.0619634, 0.0788237, 0.0940385, 0.105216,
    0.110404, 0.108647, 0.100272, 0.0867892, 0.0704499, 0.0536318,
    0.0382905, 0.0142075
};

static float const blurOffsets14[] = {
    -13.4533, -11.4601, -9.46706, -7.47398, -5.48091, -3.48785,
    -1.49479, 0.498264, 2.49132, 4.48438, 6.47744, 8.47052,
    10.4636, 12.4567, 14
};
static float const blurWeights14[] = {
    0.0291341, 0.0411804, 0.0550723, 0.0696835, 0.0834219, 0.0944896,
    0.101261, 0.102672, 0.098496, 0.0894001, 0.0767734, 0.0623789,
    0.0479532, 0.034878, 0.0132052
};

static float const blurOffsets15[] = {
    -14.4562, -12.4623, -10.4683, -8.47431, -6.48035, -4.48639,
    -2.49244, -0.498488, 1.49546, 3.48941, 5.48337, 7.47733,
    9.47129, 11.4653, 13.4593, 15
};
static float const blurWeights15[] = {
    0.0270357, 0.037445, 0.0494189, 0.0621492, 0.0744769, 0.0850456,
    0.0925391, 0.0959494, 0.0947989, 0.08925, 0.0800675, 0.068446,
    0.055755, 0.0432776, 0.03201, 0.012335
};

static float const blurOffsets16[] = {
    -15.4589, -13.4642, -11.4695, -9.47476, -7.48007, -5.48538,
    -3.4907, -1.49601, 0.498671, 2.49335, 4.48804, 6.48273,
    8.47742, 10.4721, 12.4668, 14.4615, 16
};
static float const blurWeights16[] = {
    0.0252188, 0.0343012, 0.0447166, 0.055873, 0.0669129, 0.0768055,
    0.0844986, 0.0891007, 0.0900508, 0.0872305, 0.0809887, 0.07207,
    0.0614694, 0.0502503, 0.0393724, 0.0295679, 0.0115725
};

static const float * const blurOffsets[] = {
    0,
    blurOffsets1,
    blurOffsets2,
    blurOffsets3,
    blurOffsets4,
    blurOffsets5,
    blurOffsets6,
    blurOffsets7,
    blurOffsets8,
    blurOffsets9,
    blurOffsets10,
    blurOffsets11,
    blurOffsets12,
    blurOffsets13,
    blurOffsets14,
    blurOffsets15,
    blurOffsets16
};

static const float * const blurWeights[] = {
    0,
    blurWeights1,
    blurWeights2,
    blurWeights3,
    blurWeights4,
    blurWeights5,
    blurWeights6,
    blurWeights7,
    blurWeights8,
    blurWeights9,
    blurWeights10,
    blurWeights11,
    blurWeights12,
    blurWeights13,
    blurWeights14,
    blurWeights15,
    blurWeights16
};

static int const blurSizes[] = {
    0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17
};

#endif
