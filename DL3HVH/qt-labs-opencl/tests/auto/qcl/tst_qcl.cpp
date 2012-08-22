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
#include <QtGui/qvector2d.h>
#include <QtGui/qvector3d.h>
#include <QtGui/qvector4d.h>
#include <QtGui/qmatrix4x4.h>
#include <QtCore/qpoint.h>

class tst_QCL : public QObject
{
    Q_OBJECT
public:
    tst_QCL() {}
    virtual ~tst_QCL() {}

private slots:
    void initTestCase();
    void checkProfile();
    void platform();
    void device();
    void buildProgram();
    void argumentPassing();
    void vectorBuffer();
    void eventProfiling();
    void sampler();
    void workSize();
    void roundWorkSize_data();
    void roundWorkSize();
    void imageFormat();
    void qimageFormat_data();
    void qimageFormat();
    void eventList();
    void concurrent();

private:
    QCLContext context;
    QCLProgram program;
};

void tst_QCL::initTestCase()
{
    if (!context.create())
        QFAIL("OpenCL implementation unusable - cannot create default context");
}

// Checks that the OpenCL implementation supports either
// FULL_PROFILE or EMBEDDED_PROFILE and that the device and
// platform are consistent.
void tst_QCL::checkProfile()
{
    QCLDevice device = context.defaultDevice();
    QCLPlatform platform = device.platform();
    QVERIFY((device.isFullProfile() && !device.isEmbeddedProfile()) ||
            (!device.isFullProfile() && device.isEmbeddedProfile()));
    QVERIFY((platform.isFullProfile() && !platform.isEmbeddedProfile()) ||
            (!platform.isFullProfile() && platform.isEmbeddedProfile()));
    QVERIFY(device.isFullProfile() == platform.isFullProfile());
    QVERIFY(device.isEmbeddedProfile() == platform.isEmbeddedProfile());
}

// Test the QCLPlatform class.
void tst_QCL::platform()
{
    // Check the null platform identifier.
    QCLPlatform platform;
    QVERIFY(platform.isNull());
    QVERIFY(platform.platformId() == 0);
    QVERIFY(platform == platform);
    QVERIFY(!(platform != platform));
    QVERIFY(!platform.isFullProfile());
    QVERIFY(!platform.isEmbeddedProfile());
    QVERIFY(platform.profile().isEmpty());
    QVERIFY(platform.version().isEmpty());
    QVERIFY(platform.name().isEmpty());
    QVERIFY(platform.vendor().isEmpty());
    QVERIFY(platform.extensions().isEmpty());
    QVERIFY(!platform.hasExtension("cl_khr_byte_addressable_store"));

    // Must have at least 1 platform on the system.
    QList<QCLPlatform> platforms = QCLPlatform::platforms();
    QVERIFY(platforms.size() >= 1);
    foreach (QCLPlatform platform2, platforms) {
        // Should be non-null and not equal to a null platform.
        QVERIFY(!platform2.isNull());
        QVERIFY(platform2.platformId() != 0);
        QVERIFY(platform != platform2);
        QVERIFY(!(platform == platform2));

        // Must be either full or embedded.
        QVERIFY(platform2.isFullProfile() || platform2.isEmbeddedProfile());

        // Version should start with "OpenCL ".
        QVERIFY(platform2.version().startsWith(QLatin1String("OpenCL ")));

        // profile() should be consistent with isFull/EmbeddedProfile().
        QString profile = platform2.profile();
        if (platform2.isFullProfile())
            QCOMPARE(profile, QLatin1String("FULL_PROFILE"));
        if (platform2.isEmbeddedProfile())
            QCOMPARE(profile, QLatin1String("EMBEDDED_PROFILE"));

        // Check the extension list.
        QStringList extensions = platform2.extensions();
        foreach (QString extn, extensions) {
            QVERIFY(platform2.hasExtension(extn.toLatin1().constData()));
            QVERIFY(platform2.hasExtension(extn.toUpper().toLatin1().constData()));
        }
        QVERIFY(!platform2.hasExtension("unknown_extension"));
        QVERIFY(!platform2.hasExtension("cl")); // Prefix match.
    }
}

static bool sameDeviceList(const QList<QCLDevice> &list1,
                           const QList<QCLDevice> &list2)
{
    foreach (QCLDevice dev, list1) {
        if (!list2.contains(dev))
            return false;
    }
    foreach (QCLDevice dev, list2) {
        if (!list1.contains(dev))
            return false;
    }
    return true;
}

// Test the QCLDevice class.
void tst_QCL::device()
{
    // Check the null device identifier.
    QCLDevice device;
    QVERIFY(device.isNull());
    QVERIFY(device.deviceId() == 0);
    QVERIFY(device.deviceType() == 0);
    QVERIFY(device.platform().isNull());
    QVERIFY(device == device);
    QVERIFY(!(device != device));
    QVERIFY(!device.isFullProfile());
    QVERIFY(!device.isEmbeddedProfile());
    QVERIFY(device.vendorId() == 0);
    QVERIFY(device.profile().isEmpty());
    QVERIFY(device.version().isEmpty());
    QVERIFY(device.driverVersion().isEmpty());
    QVERIFY(device.name().isEmpty());
    QVERIFY(device.vendor().isEmpty());
    QVERIFY(device.extensions().isEmpty());
    QVERIFY(!device.hasExtension("cl_khr_byte_addressable_store"));

    // Must have at least 1 device on the system.
    QList<QCLDevice> devices = QCLDevice::allDevices();
    QList<QCLDevice> gpus;
    QList<QCLDevice> cpus;
    QVERIFY(devices.size() >= 1);
    foreach (QCLDevice device2, devices) {
        // Should be non-null and not equal to a null device.
        QVERIFY(!device2.isNull());
        QVERIFY(device2.deviceId() != 0);
        QVERIFY(device2.deviceType() != 0);
        QVERIFY(device != device2);
        QVERIFY(!(device == device2));

        // Must be either full or embedded.
        QVERIFY(device2.isFullProfile() || device2.isEmbeddedProfile());

        // Version should start with "OpenCL ".
        QVERIFY(device2.version().startsWith(QLatin1String("OpenCL ")));

        // profile() should be consistent with isFull/EmbeddedProfile().
        QString profile = device2.profile();
        if (device2.isFullProfile())
            QCOMPARE(profile, QLatin1String("FULL_PROFILE"));
        if (device2.isEmbeddedProfile())
            QCOMPARE(profile, QLatin1String("EMBEDDED_PROFILE"));

        // Check the extension list.
        QStringList extensions = device2.extensions();
        foreach (QString extn, extensions) {
            QVERIFY(device2.hasExtension(extn.toLatin1().constData()));
            QVERIFY(device2.hasExtension(extn.toUpper().toLatin1().constData()));
        }
        QVERIFY(!device2.hasExtension("unknown_extension"));
        QVERIFY(!device2.hasExtension("cl")); // Prefix match.

        // Warn for device properties that may be of concern.
        // The device may still be usable, but buggy.
        if (!device2.isAvailable())
            qWarning() << device2.name() << "is not available";
        if (!device2.hasCompiler())
            qWarning() << device2.name() << "does not have a compiler";
        if (!device2.computeUnits())
            qWarning() << device2.name() << "does not have any compute units";
        if (!device2.hasImage2D()) {
            qWarning() << device2.name() << "does not support 2D images";
        } else {
            QSize size = device2.maximumImage2DSize();
            if (size.width() < 1 || size.height() < 1)
                qWarning() << device2.name() << "has 2D images, but max size is" << size;
        }
        if (!device2.hasImage3D()) {
            // It is OK for the embedded profile to not have 3D images.
            if (!device2.isEmbeddedProfile())
                qWarning() << device2.name() << "does not support 3D images";
        } else {
            QCLWorkSize size = device2.maximumImage3DSize();
            if (size.width() < 1 || size.height() < 1 || size.depth() < 1)
                qWarning() << device2.name() << "has 3D images, but max size is" << size.width() << size.height() << size.depth();
        }

        // Split the devices based on type.
        if ((device2.deviceType() & QCLDevice::GPU) != 0)
            gpus.append(device2);
        if ((device2.deviceType() & QCLDevice::CPU) != 0)
            cpus.append(device2);
    }

    // Check that the type-specific lists are accurate.
    QVERIFY(sameDeviceList(gpus, QCLDevice::devices(QCLDevice::GPU)));
    QVERIFY(sameDeviceList(cpus, QCLDevice::devices(QCLDevice::CPU)));
}

// Build the OpenCL program code we will be using for the rest of the tests.
void tst_QCL::buildProgram()
{
    program = context.buildProgramFromSourceFile(QLatin1String(":/tst_qcl.cl"));
    if (program.isNull()) {
        if (context.defaultDevice().hasCompiler())
            QFAIL("could not compile the OpenCL test program");
        else
            QFAIL("OpenCL implementation does not have a compiler");
    }

    // Don't need the compiler any more.
    QCLProgram::unloadCompiler();
}

// Tests that passing various argument types like int, float, QVector3D,
// QPointF, QMatrix4x4, etc to an OpenCL kernel does the right thing.
void tst_QCL::argumentPassing()
{
    QCLBuffer buffer = context.createBufferDevice
        (sizeof(float) * 16, QCLMemoryObject::WriteOnly);
    float buf[16];
    cl_int bufi[2];
    cl_uint bufui;
    cl_long buflong;
    cl_ulong bufulong;

    QCLKernel storeFloat = program.createKernel("storeFloat");
    storeFloat(buffer, 5.0f);
    buffer.read(buf, sizeof(float));
    QCOMPARE(buf[0], 5.0f);

    QCLKernel storeInt = program.createKernel("storeInt");
    storeInt(buffer, 7);
    buffer.read(bufi, sizeof(cl_int));
    QCOMPARE(bufi[0], cl_int(7));

    QCLKernel storeUInt = program.createKernel("storeUInt");
    storeUInt(buffer, cl_uint(9));
    buffer.read(&bufui, sizeof(cl_uint));
    QCOMPARE(bufui, cl_uint(9));

    QCLKernel storeLong = program.createKernel("storeLong");
    storeLong(buffer, cl_long(-9));
    buffer.read(&buflong, sizeof(cl_long));
    QCOMPARE(buflong, cl_long(-9));

    QCLKernel storeULong = program.createKernel("storeULong");
    storeULong(buffer, cl_ulong(9));
    buffer.read(&bufulong, sizeof(cl_ulong));
    QCOMPARE(bufulong, cl_ulong(9));

    QCLKernel storeVec2 = program.createKernel("storeVec2");
    storeVec2(buffer, QVector2D(1.0f, 2.0f));
    buffer.read(buf, sizeof(float) * 2);
    QCOMPARE(buf[0], 1.0f);
    QCOMPARE(buf[1], 2.0f);
    storeVec2(buffer, QPointF(3.0f, 4.0f));
    buffer.read(buf, sizeof(float) * 2);
    QCOMPARE(buf[0], 3.0f);
    QCOMPARE(buf[1], 4.0f);

    QCLKernel storeVec2i = program.createKernel("storeVec2i");
    storeVec2i(buffer, QPoint(6, -7));
    buffer.read(bufi, sizeof(cl_int) * 2);
    QCOMPARE(bufi[0], cl_int(6));
    QCOMPARE(bufi[1], cl_int(-7));

    QCLKernel storeVec3 = program.createKernel("storeVec3");
    storeVec3(buffer, QVector3D(-1.0f, -2.0f, -3.0f));
    buffer.read(buf, sizeof(float) * 4);
    QCOMPARE(buf[0], -1.0f);
    QCOMPARE(buf[1], -2.0f);
    QCOMPARE(buf[2], -3.0f);
    QCOMPARE(buf[3], 1.0f);

    QCLKernel storeVec4 = program.createKernel("storeVec4");
    storeVec4(buffer, QVector4D(1.0f, 2.0f, 3.0f, 4.0f));
    buffer.read(buf, sizeof(float) * 4);
    QCOMPARE(buf[0], 1.0f);
    QCOMPARE(buf[1], 2.0f);
    QCOMPARE(buf[2], 3.0f);
    QCOMPARE(buf[3], 4.0f);

    QMatrix4x4 m(1.0f, 2.0f, 3.0f, 4.0f,    // row-major input
                 5.0f, 6.0f, 7.0f, 8.0f,
                 9.0f, 10.0f, 11.0f, 12.0f,
                 13.0f, 14.0f, 15.0f, 16.0f);
    QCLKernel storeMat4 = program.createKernel("storeMat4");
    storeMat4(buffer, m);
    buffer.read(buf, sizeof(float) * 16);
    QCOMPARE(buf[0], 1.0f);                 // column-major output
    QCOMPARE(buf[1], 5.0f);
    QCOMPARE(buf[2], 9.0f);
    QCOMPARE(buf[3], 13.0f);
    QCOMPARE(buf[4], 2.0f);
    QCOMPARE(buf[5], 6.0f);
    QCOMPARE(buf[6], 10.0f);
    QCOMPARE(buf[7], 14.0f);
    QCOMPARE(buf[8], 3.0f);
    QCOMPARE(buf[9], 7.0f);
    QCOMPARE(buf[10], 11.0f);
    QCOMPARE(buf[11], 15.0f);
    QCOMPARE(buf[12], 4.0f);
    QCOMPARE(buf[13], 8.0f);
    QCOMPARE(buf[14], 12.0f);
    QCOMPARE(buf[15], 16.0f);
}

static float constVectorAt(const QCLVector<float>& vector, int index)
{
    return vector[index];
}

// Test the QCLVector<T> class.
void tst_QCL::vectorBuffer()
{
    QCLVector<float> vector1;
    QVERIFY(vector1.isNull());
    QVERIFY(vector1.isEmpty());
    QCOMPARE(vector1.size(), 0);
    QVERIFY(vector1.toBuffer().memoryId() == 0);
    QVERIFY(vector1.context() == 0);

    vector1 = context.createVector<float>(100);
    QVERIFY(!vector1.isNull());
    QVERIFY(!vector1.isEmpty());
    QCOMPARE(vector1.size(), 100);
    QVERIFY(vector1.toBuffer().memoryId() != 0);
    QVERIFY(vector1.context() == &context);

    for (int index = 0; index < 100; ++index)
        vector1[index] = float(index);

    for (int index = 0; index < 100; ++index)
        QCOMPARE(vector1[index], float(index));

    QCLKernel addToVector = program.createKernel("addToVector");
    addToVector.setGlobalWorkSize(vector1.size());
    addToVector(vector1, 42.0f);

    for (int index = 0; index < 100; ++index) {
        QCOMPARE(constVectorAt(vector1, index), float(index + 42));
        QCOMPARE(vector1[index], float(index + 42));
    }

    vector1.release();
    QVERIFY(vector1.isNull());
    QVERIFY(vector1.isEmpty());
    QCOMPARE(vector1.size(), 0);
    QVERIFY(vector1.toBuffer().memoryId() == 0);
    QVERIFY(vector1.context() == 0);
}

void tst_QCL::eventProfiling()
{
    QVERIFY(!context.defaultCommandQueue().isProfilingEnabled());

    QCLCommandQueue queue =
        context.createCommandQueue(CL_QUEUE_PROFILING_ENABLE);
    QVERIFY(queue.isProfilingEnabled());
    context.setCommandQueue(queue);

    QCLVector<float> vector1 = context.createVector<float>(20000);
    for (int index = 0; index < vector1.size(); ++index)
        vector1[index] = float(index);

    QCLKernel addToVector = program.createKernel("addToVector");
    addToVector.setGlobalWorkSize(vector1.size());
    QCLEvent event = addToVector(vector1, 42.0f);

    event.waitForFinished();

    // The following tests assume that device time is monotonicly increasing.
    QVERIFY(event.finishTime() != 0);
    QVERIFY(event.submitTime() >= event.queueTime());
    QVERIFY(event.runTime() >= event.submitTime());
    QVERIFY(event.finishTime() >= event.runTime());

    context.setCommandQueue(context.defaultCommandQueue());
}

// Test QCLSampler.
void tst_QCL::sampler()
{
    QCLSampler sampler;

    // Check the default conditions.
    QVERIFY(sampler.isNull());
    QVERIFY(!sampler.normalizedCoordinates());
    QVERIFY(sampler.addressingMode() == QCLSampler::ClampToEdge);
    QVERIFY(sampler.filterMode() == QCLSampler::Linear);
    QVERIFY(sampler.samplerId() == 0);
    QVERIFY(sampler.context() == 0);
    QVERIFY(sampler == sampler);
    QVERIFY(!(sampler != sampler));

    // Create a new sampler with specific values.
    sampler = context.createSampler
        (true, QCLSampler::None, QCLSampler::Nearest);
    QVERIFY(sampler.normalizedCoordinates());
    QVERIFY(sampler.addressingMode() == QCLSampler::None);
    QVERIFY(sampler.filterMode() == QCLSampler::Nearest);
    QVERIFY(!sampler.isNull());
    QVERIFY(sampler.samplerId() != 0);
    QVERIFY(sampler.context() == &context);

    // Check that the sampler is not the same as the default.
    QCLSampler sampler2;
    QVERIFY(sampler != sampler2);
    QVERIFY(!(sampler == sampler2));

    // Copy and check.
    QCLSampler sampler3(sampler);
    QVERIFY(sampler3.normalizedCoordinates());
    QVERIFY(sampler3.addressingMode() == QCLSampler::None);
    QVERIFY(sampler3.filterMode() == QCLSampler::Nearest);
    sampler2 = sampler;
    QVERIFY(sampler2.normalizedCoordinates());
    QVERIFY(sampler2.addressingMode() == QCLSampler::None);
    QVERIFY(sampler2.filterMode() == QCLSampler::Nearest);
    QVERIFY(sampler3 == sampler);
    QVERIFY(!(sampler3 != sampler));
    QVERIFY(sampler2 == sampler);
    QVERIFY(!(sampler2 != sampler));

    // Create a sampler from a raw identifier.
    clRetainSampler(sampler3.samplerId());
    QCLSampler sampler4(sampler3.context(), sampler3.samplerId());
    QVERIFY(sampler3.samplerId() == sampler4.samplerId());
    QVERIFY(sampler3.context() == sampler4.context());
    QVERIFY(sampler4.normalizedCoordinates());
    QVERIFY(sampler4.addressingMode() == QCLSampler::None);
    QVERIFY(sampler4.filterMode() == QCLSampler::Nearest);
}

// Test QCLWorkSize.
void tst_QCL::workSize()
{
    QCLWorkSize size;
    QVERIFY(size.dimensions() == 1);
    QVERIFY(size.width() == 1);
    QVERIFY(size.height() == 1);
    QVERIFY(size.depth() == 1);

    QCLWorkSize size1(42);
    QVERIFY(size1.dimensions() == 1);
    QVERIFY(size1.width() == 42);
    QVERIFY(size1.height() == 1);
    QVERIFY(size1.depth() == 1);

    QCLWorkSize size2(42, 63);
    QVERIFY(size2.dimensions() == 2);
    QVERIFY(size2.width() == 42);
    QVERIFY(size2.height() == 63);
    QVERIFY(size2.depth() == 1);

    QCLWorkSize size2b(QSize(63, 42));
    QVERIFY(size2b.dimensions() == 2);
    QVERIFY(size2b.width() == 63);
    QVERIFY(size2b.height() == 42);
    QVERIFY(size2b.depth() == 1);

    QCLWorkSize size3(42, 63, 12);
    QVERIFY(size3.dimensions() == 3);
    QVERIFY(size3.width() == 42);
    QVERIFY(size3.height() == 63);
    QVERIFY(size3.depth() == 12);

    QVERIFY(size != size1);
    QVERIFY(size1 != size2);
    QVERIFY(size2 != size3);
    QVERIFY(!(size == size1));
    QVERIFY(!(size1 == size2));
    QVERIFY(!(size2 == size3));

    QCLWorkSize size4(size3);
    QVERIFY(size4.dimensions() == 3);
    QVERIFY(size4.width() == 42);
    QVERIFY(size4.height() == 63);
    QVERIFY(size4.depth() == 12);
    QVERIFY(size3 == size4);
    QVERIFY(!(size3 != size4));

    size4 = size2;
    QVERIFY(size4.dimensions() == 2);
    QVERIFY(size4.width() == 42);
    QVERIFY(size4.height() == 63);
    QVERIFY(size4.depth() == 1);

    QVERIFY(size4.width() == size4.sizes()[0]);
    QVERIFY(size4.height() == size4.sizes()[1]);
    QVERIFY(size4.depth() == size4.sizes()[2]);

    QCLWorkSize size5;
    size5 = QCLWorkSize::fromString(QLatin1String(""));
    QVERIFY(size5.dimensions() == 1);
    QVERIFY(size5.width() == 1);
    QVERIFY(size5.height() == 1);
    QVERIFY(size5.depth() == 1);
    QCOMPARE(size5.toString(), QLatin1String("1"));

    size5 = QCLWorkSize::fromString(QLatin1String("23"));
    QVERIFY(size5.dimensions() == 1);
    QVERIFY(size5.width() == 23);
    QVERIFY(size5.height() == 1);
    QVERIFY(size5.depth() == 1);
    QCOMPARE(size5.toString(), QLatin1String("23"));

    size5 = QCLWorkSize::fromString(QLatin1String("23x6"));
    QVERIFY(size5.dimensions() == 2);
    QVERIFY(size5.width() == 23);
    QVERIFY(size5.height() == 6);
    QVERIFY(size5.depth() == 1);
    QCOMPARE(size5.toString(), QLatin1String("23x6"));

    size5 = QCLWorkSize::fromString(QLatin1String("23 x 6 x 43"));
    QVERIFY(size5.dimensions() == 3);
    QVERIFY(size5.width() == 23);
    QVERIFY(size5.height() == 6);
    QVERIFY(size5.depth() == 43);
    QCOMPARE(size5.toString(), QLatin1String("23x6x43"));

    size5 = QCLWorkSize::fromString(QLatin1String("23x6x43x1"));
    QVERIFY(size5.dimensions() == 3);
    QVERIFY(size5.width() == 23);
    QVERIFY(size5.height() == 6);
    QVERIFY(size5.depth() == 43);
    QCOMPARE(size5.toString(), QLatin1String("23x6x43"));

    QByteArray array;
    QDataStream stream(&array, QIODevice::WriteOnly);
    stream << QCLWorkSize(23);
    stream << QCLWorkSize(42, 12);
    stream << QCLWorkSize(2, 3, 1);
    stream << QCLWorkSize();

    QDataStream stream2(array);
    stream2 >> size1;
    stream2 >> size2;
    stream2 >> size3;
    stream2 >> size4;
    QVERIFY(size1 == QCLWorkSize(23));
    QVERIFY(size2 == QCLWorkSize(42, 12));
    QVERIFY(size3 == QCLWorkSize(2, 3, 1));
    QVERIFY(size4 == QCLWorkSize());
}

// Test QCLWorkSize::roundTo().
void tst_QCL::roundWorkSize_data()
{
    QTest::addColumn<int>("value");
    QTest::addColumn<int>("multiple");
    QTest::addColumn<int>("result");

    QTest::newRow("0-20") << 0 << 20 << 0;
    QTest::newRow("23-20") << 23 << 20 << 40;
    QTest::newRow("23-2") << 23 << 2 << 24;
    QTest::newRow("23-1") << 23 << 1 << 23;
    QTest::newRow("23-0") << 23 << 0 << 23;
}
void tst_QCL::roundWorkSize()
{
    QFETCH(int, value);
    QFETCH(int, multiple);
    QFETCH(int, result);

    size_t svalue = size_t(value);
    size_t smultiple = size_t(multiple);
    size_t sresult = size_t(result);

    QCLWorkSize size1(svalue);
    QCLWorkSize size2 = size1.roundTo(smultiple);
    QCOMPARE(size2.width(), sresult);
    QCOMPARE(size2.height(), size_t(1));
    QCOMPARE(size2.depth(), size_t(1));
    QCOMPARE(size2.dimensions(), size_t(1));

    QCLWorkSize size3(svalue, 1);
    QCLWorkSize size4 = size3.roundTo(QCLWorkSize(smultiple, smultiple));
    QCOMPARE(size4.width(), sresult);
    QCOMPARE(size4.height(), smultiple ? smultiple : size_t(1));
    QCOMPARE(size4.depth(), size_t(1));
    QCOMPARE(size4.dimensions(), size_t(2));

    QCLWorkSize size5(1, svalue);
    QCLWorkSize size6 = size5.roundTo(QCLWorkSize(smultiple, smultiple));
    QCOMPARE(size6.width(), smultiple ? smultiple : size_t(1));
    QCOMPARE(size6.height(), sresult);
    QCOMPARE(size6.depth(), size_t(1));
    QCOMPARE(size6.dimensions(), size_t(2));

    QCLWorkSize size7(svalue, 1, 1);
    QCLWorkSize size8 = size7.roundTo(QCLWorkSize(smultiple, smultiple, smultiple));
    QCOMPARE(size8.width(), sresult);
    QCOMPARE(size8.height(), smultiple ? smultiple : size_t(1));
    QCOMPARE(size8.depth(), smultiple ? smultiple : size_t(1));
    QCOMPARE(size8.dimensions(), size_t(3));

    QCLWorkSize size9(1, svalue, 1);
    QCLWorkSize size10 = size9.roundTo(QCLWorkSize(smultiple, smultiple, smultiple));
    QCOMPARE(size10.width(), smultiple ? smultiple : size_t(1));
    QCOMPARE(size10.height(), sresult);
    QCOMPARE(size10.depth(), smultiple ? smultiple : size_t(1));
    QCOMPARE(size10.dimensions(), size_t(3));

    QCLWorkSize size11(1, 1, svalue);
    QCLWorkSize size12 = size11.roundTo(QCLWorkSize(smultiple, smultiple, smultiple));
    QCOMPARE(size12.width(), smultiple ? smultiple : size_t(1));
    QCOMPARE(size12.height(), smultiple ? smultiple : size_t(1));
    QCOMPARE(size12.depth(), sresult);
    QCOMPARE(size12.dimensions(), size_t(3));
}

// Test QCLImageFormat.
void tst_QCL::imageFormat()
{
    QCLImageFormat format1;
    QVERIFY(format1.isNull());
    QVERIFY(format1.channelOrder() == 0);
    QVERIFY(format1.channelType() == 0);
    QVERIFY(format1.toQImageFormat() == QImage::Format_Invalid);

    QCLImageFormat format2(QCLImageFormat::Order_RGBA,
                           QCLImageFormat::Type_Normalized_UInt16);
    QVERIFY(!format2.isNull());
    QVERIFY(format2.channelOrder() == QCLImageFormat::Order_RGBA);
    QVERIFY(format2.channelType() == QCLImageFormat::Type_Normalized_UInt16);
    QVERIFY(format2.toQImageFormat() == QImage::Format_Invalid);
    QVERIFY(format1 != format2);
    QVERIFY(!(format1 == format2));

    QCLImageFormat format3(format2);
    QVERIFY(format3.channelOrder() == QCLImageFormat::Order_RGBA);
    QVERIFY(format3.channelType() == QCLImageFormat::Type_Normalized_UInt16);
    QVERIFY(format3.toQImageFormat() == QImage::Format_Invalid);
    QVERIFY(format3 == format2);
    QVERIFY(!(format3 != format2));
}

void tst_QCL::qimageFormat_data()
{
    QTest::addColumn<int>("qformat");
    QTest::addColumn<int>("order");
    QTest::addColumn<int>("type");
    QTest::addColumn<int>("resultQformat");
    QTest::addColumn<int>("reverseQformat");

    QCLImageFormat::ChannelOrder hostOrder;
    if (QSysInfo::ByteOrder == QSysInfo::LittleEndian)
        hostOrder = QCLImageFormat::Order_BGRA;
    else
        hostOrder = QCLImageFormat::Order_ARGB;

    QTest::newRow("Indexed8")
        << int(QImage::Format_Indexed8)
        << int(QCLImageFormat::Order_A)
        << int(QCLImageFormat::Type_Normalized_UInt8)
        << int(QImage::Format_Indexed8)
        << int(QImage::Format_Indexed8);

    QTest::newRow("RGB32")
        << int(QImage::Format_RGB32)
        << int(hostOrder)
        << int(QCLImageFormat::Type_Normalized_UInt8)
        << int(QImage::Format_RGB32)
        << int(QImage::Format_ARGB32);

    QTest::newRow("ARGB32")
        << int(QImage::Format_ARGB32)
        << int(hostOrder)
        << int(QCLImageFormat::Type_Normalized_UInt8)
        << int(QImage::Format_ARGB32)
        << int(QImage::Format_ARGB32);

    QTest::newRow("ARGB32_Premultiplied")
        << int(QImage::Format_ARGB32_Premultiplied)
        << int(hostOrder)
        << int(QCLImageFormat::Type_Normalized_UInt8)
        << int(QImage::Format_ARGB32_Premultiplied)
        << int(QImage::Format_ARGB32);

    QTest::newRow("RGB16")
        << int(QImage::Format_RGB16)
        << int(QCLImageFormat::Order_RGB)
        << int(QCLImageFormat::Type_Normalized_565)
        << int(QImage::Format_RGB16)
        << int(QImage::Format_RGB16);

    QTest::newRow("RGB555")
        << int(QImage::Format_RGB555)
        << int(QCLImageFormat::Order_RGB)
        << int(QCLImageFormat::Type_Normalized_555)
        << int(QImage::Format_RGB555)
        << int(QImage::Format_RGB555);

    QTest::newRow("RGB888")
        << int(QImage::Format_RGB888)
        << int(QCLImageFormat::Order_RGB)
        << int(QCLImageFormat::Type_Normalized_UInt8)
        << int(QImage::Format_RGB888)
        << int(QImage::Format_RGB888);

    QTest::newRow("RGB444")
        << int(QImage::Format_RGB444)
        << int(0)
        << int(0)
        << int(QImage::Format_Invalid)
        << int(QImage::Format_Invalid);

    QTest::newRow("!Indexed8")
        << int(QImage::Format_Invalid)
        << int(QCLImageFormat::Order_A)
        << int(QCLImageFormat::Type_Normalized_UInt16)
        << int(QImage::Format_Invalid)
        << int(QImage::Format_Invalid);
}

void tst_QCL::qimageFormat()
{
    QFETCH(int, qformat);
    QFETCH(int, order);
    QFETCH(int, type);
    QFETCH(int, resultQformat);
    QFETCH(int, reverseQformat);

    if (qformat != int(QImage::Format_Invalid)) {
        QCLImageFormat format1((QImage::Format)qformat);
        QVERIFY(format1.channelOrder() == QCLImageFormat::ChannelOrder(order));
        QVERIFY(format1.channelType() == QCLImageFormat::ChannelType(type));
        QVERIFY(format1.toQImageFormat() == QImage::Format(resultQformat));
    }

    QCLImageFormat format2((QCLImageFormat::ChannelOrder)order,
                           (QCLImageFormat::ChannelType)type);
    QVERIFY(format2.channelOrder() == QCLImageFormat::ChannelOrder(order));
    QVERIFY(format2.channelType() == QCLImageFormat::ChannelType(type));
    QVERIFY(format2.toQImageFormat() == QImage::Format(reverseQformat));
}

// Test QCLEventList.
void tst_QCL::eventList()
{
    QCLBuffer buffer = context.createBufferDevice
        (sizeof(float) * 16, QCLMemoryObject::WriteOnly);

    QCLKernel storeFloat = program.createKernel("storeFloat");
    QCLEvent event1 = storeFloat(buffer, 5.0f);

    QCLKernel storeInt = program.createKernel("storeInt");
    QCLEvent event2 = storeInt(buffer, 7);

    QCLEventList list;
    QVERIFY(list.isEmpty());
    QCOMPARE(list.size(), 0);

    list.append(event1);
    QVERIFY(!list.isEmpty());
    QCOMPARE(list.size(), 1);
    QVERIFY(list.at(-1).isNull());
    QVERIFY(list.at(0) == event1);
    QVERIFY(list.at(1).isNull());
    QVERIFY(list.contains(event1));
    QVERIFY(!list.contains(event2));
    QVERIFY(!list.contains(QCLEvent()));

    list.append(event2);
    QVERIFY(!list.isEmpty());
    QCOMPARE(list.size(), 2);
    QVERIFY(list.at(0) == event1);
    QVERIFY(list.at(1) == event2);
    QVERIFY(list.contains(event1));
    QVERIFY(list.contains(event2));
    QVERIFY(!list.contains(QCLEvent()));

    list.append(QCLEvent());    // Should be ignored.
    QVERIFY(!list.isEmpty());
    QCOMPARE(list.size(), 2);
    QVERIFY(list.at(0) == event1);
    QVERIFY(list.at(1) == event2);
    QVERIFY(list.contains(event1));
    QVERIFY(list.contains(event2));
    QVERIFY(!list.contains(QCLEvent()));

    // We should be able to wait as many times as we'd like.
    list.waitForFinished();
    list.waitForFinished();

    list.remove(event1);
    QVERIFY(!list.isEmpty());
    QCOMPARE(list.size(), 1);
    QVERIFY(list.at(0) == event2);
    QVERIFY(list.contains(event2));
    QVERIFY(!list.contains(event1));
    QVERIFY(!list.contains(QCLEvent()));

    QCLEventList list2;
    list2.append(event1);
    list.append(list2);
    QVERIFY(!list.isEmpty());
    QCOMPARE(list.size(), 2);
    QVERIFY(list.at(0) == event2);
    QVERIFY(list.at(1) == event1);
    QVERIFY(list.contains(event1));
    QVERIFY(list.contains(event2));
    QVERIFY(!list.contains(QCLEvent()));

    list.append(event2);
    QVERIFY(!list.isEmpty());
    QCOMPARE(list.size(), 3);
    QVERIFY(list.at(0) == event2);
    QVERIFY(list.at(1) == event1);
    QVERIFY(list.at(2) == event2);
    QVERIFY(list.contains(event1));
    QVERIFY(list.contains(event2));
    QVERIFY(!list.contains(QCLEvent()));

    // Wait on a list with multiple copies of the same event.
    list.waitForFinished();

    list.remove(event2);
    QVERIFY(!list.isEmpty());
    QCOMPARE(list.size(), 1);
    QVERIFY(list.at(0) == event1);
    QVERIFY(list.contains(event1));
    QVERIFY(!list.contains(event2));
    QVERIFY(!list.contains(QCLEvent()));
}

#ifndef QT_NO_CONCURRENT

// Regular QtConcurrent function that checks that normal usage of
// QtConcurrent is not inhibited by the definitions in qclkernel.h.
static void runStoreFloat(QCLKernel &kernel, const QCLBuffer &buffer, float v)
{
    kernel(buffer, v).waitForFinished();
}

#endif

void tst_QCL::concurrent()
{
#ifndef QT_NO_CONCURRENT
    QCLBuffer buffer = context.createBufferDevice
        (sizeof(float) * 16, QCLMemoryObject::WriteOnly);
    float buf[16];

    QCLKernel storeFloat = program.createKernel("storeFloat");
    QFuture<void> future = QtConcurrent::run(storeFloat, buffer, 5.0f);
    future.waitForFinished();
    buffer.read(buf, sizeof(float));
    QCOMPARE(buf[0], 5.0f);

    future = QtConcurrent::run(runStoreFloat, storeFloat, buffer, 7.0f);
    future.waitForFinished();
    buffer.read(buf, sizeof(float));
    QCOMPARE(buf[0], 7.0f);
#endif
}

QTEST_MAIN(tst_QCL)

#include "tst_qcl.moc"
