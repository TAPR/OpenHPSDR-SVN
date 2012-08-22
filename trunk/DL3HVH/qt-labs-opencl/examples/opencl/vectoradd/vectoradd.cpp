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

#include <stdio.h>
#include "qclcontext.h"

int main(int, char **)
{
//! [1]
    QCLContext context;
    if (!context.create()) {
        fprintf(stderr, "Could not create OpenCL context for the GPU\n");
        return 1;
    }
//! [1]

//! [2]
    QCLVector<int> input1 = context.createVector<int>(2048);
    QCLVector<int> input2 = context.createVector<int>(2048);
    for (int index = 0; index < 2048; ++index) {
        input1[index] = index;
        input2[index] = 2048 - index;
    }
//! [2]
//! [3]
    QCLVector<int> output = context.createVector<int>(2048);
//! [3]

//! [4]
    QCLProgram program = context.buildProgramFromSourceFile(":/vectoradd.cl");
    QCLKernel kernel = program.createKernel("vectorAdd");
//! [4]

//! [5]
    kernel.setGlobalWorkSize(2048);
//! [5]
//! [6]
    kernel(input1, input2, output);
//! [6]

//! [7]
    for (int index = 0; index < 2048; ++index) {
        if (output[index] != 2048) {
            fprintf(stderr, "Answer at index %d is %d, should be %d\n",
                    index, output[index], 2048);
            return 1;
        }
    }
    printf("Answer is correct: %d\n", 2048);
//! [7]

    return 0;
}
