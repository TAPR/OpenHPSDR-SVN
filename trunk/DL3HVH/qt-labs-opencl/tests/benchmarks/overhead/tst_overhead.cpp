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
#include "qclcontext.h"

// Test the overhead of QtOpenCL operations compared to performing
// them directly with raw OpenCL C API calls.
class tst_OpenCLOverhead : public QObject
{
    Q_OBJECT
public:
    tst_OpenCLOverhead() {}
    virtual ~tst_OpenCLOverhead() {}

private slots:
    void initTestCase();

    // Test the overhead of reference count retain/release operations.
    void bufferRefCount();
    void bufferRefCountRaw();
    void commandQueueRefCount();
    void commandQueueRefCountRaw();

    // Test the overhead of kernel execution.
    void kernelExec();
    void kernelExecRaw();

private:
    QCLContext context;
    QCLProgram program;
    QCLKernel storeVec4;
};

void tst_OpenCLOverhead::initTestCase()
{
    QVERIFY(context.create());

    program = context.buildProgramFromSourceFile(QLatin1String(":/overhead.cl"));
    if (program.isNull()) {
        if (context.defaultDevice().hasCompiler())
            QFAIL("could not compile the OpenCL test program");
        else
            QFAIL("OpenCL implementation does not have a compiler");
    }
    storeVec4 = program.createKernel("storeVec4");
}

void tst_OpenCLOverhead::bufferRefCount()
{
    QCLBuffer buffer, buffer2, buffer3;

    buffer = context.createBufferDevice(1024, QCLMemoryObject::ReadWrite);
    buffer2 = context.createBufferDevice(1024, QCLMemoryObject::ReadWrite);

    QBENCHMARK {
        buffer3 = buffer;
        buffer3 = buffer2;
    }

    buffer3.write("abcdefg", 8);
}

void tst_OpenCLOverhead::bufferRefCountRaw()
{
    QCLBuffer buffer, buffer2;

    buffer = context.createBufferDevice(1024, QCLMemoryObject::ReadWrite);
    buffer2 = context.createBufferDevice(1024, QCLMemoryObject::ReadWrite);

    cl_mem bufId = buffer.memoryId();
    cl_mem bufId2 = buffer2.memoryId();

    QBENCHMARK {
        clRetainMemObject(bufId);
        clRetainMemObject(bufId2);
        clReleaseMemObject(bufId);
        clReleaseMemObject(bufId2);
    }
}

void tst_OpenCLOverhead::commandQueueRefCount()
{
    QCLCommandQueue queue = context.createCommandQueue(0);
    QCLCommandQueue queue2 = context.createCommandQueue(0);
    QCLCommandQueue queue3;

    QBENCHMARK {
        queue3 = queue;
        queue3 = queue2;
    }

    queue3.isOutOfOrder();
}

void tst_OpenCLOverhead::commandQueueRefCountRaw()
{
    QCLCommandQueue queue = context.createCommandQueue(0);
    QCLCommandQueue queue2 = context.createCommandQueue(0);

    cl_command_queue queueId = queue.queueId();
    cl_command_queue queueId2 = queue2.queueId();

    QBENCHMARK {
        clRetainCommandQueue(queueId);
        clRetainCommandQueue(queueId2);
        clReleaseCommandQueue(queueId);
        clReleaseCommandQueue(queueId2);
    }
}

void tst_OpenCLOverhead::kernelExec()
{
    float args[] = {1.0f, 2.0f, -5.0f, 10.0f};

    QCLBuffer buffer;
    buffer = context.createBufferDevice(1024, QCLMemoryObject::ReadWrite);

    QBENCHMARK {
        storeVec4(buffer, args[0], args[1], args[2], args[3]).waitForFinished();
    }
}

void tst_OpenCLOverhead::kernelExecRaw()
{
    float args[] = {1.0f, 2.0f, -5.0f, 10.0f};

    QCLBuffer buffer;
    buffer = context.createBufferDevice(1024, QCLMemoryObject::ReadWrite);

    cl_kernel kernel = storeVec4.kernelId();
    cl_mem bufId = buffer.memoryId();
    cl_command_queue queue = context.commandQueue().queueId();
    size_t globalWorkSize[3] = {1, 1, 1};

    QBENCHMARK {
        clSetKernelArg(kernel, 0, sizeof(bufId), &bufId);
        clSetKernelArg(kernel, 1, sizeof(float), args + 0);
        clSetKernelArg(kernel, 2, sizeof(float), args + 1);
        clSetKernelArg(kernel, 3, sizeof(float), args + 2);
        clSetKernelArg(kernel, 4, sizeof(float), args + 3);
        cl_event event;
        clEnqueueNDRangeKernel
            (queue, kernel, 1, 0, globalWorkSize, 0, 0, 0, &event);
        clWaitForEvents(1, &event);
    }
}

QTEST_MAIN(tst_OpenCLOverhead)

#include "tst_overhead.moc"
