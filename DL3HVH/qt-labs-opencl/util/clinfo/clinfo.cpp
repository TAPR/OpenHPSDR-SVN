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

#include <stdio.h>
#include "qclplatform.h"
#include "qcldevice.h"

static void printFloatCapabilities(QCLDevice::FloatCapabilities caps);
static void printMemorySize(quint64 size);

int main(int argc, char *argv[])
{
    Q_UNUSED(argc);
    Q_UNUSED(argv);
    printf("OpenCL Platforms:\n");
    QList<QCLPlatform> platforms = QCLPlatform::platforms();
    foreach (QCLPlatform platform, platforms) {
        printf("    Platform ID       : %ld\n", long(platform.platformId()));
        printf("    Profile           : %s\n", platform.profile().toLatin1().constData());
        printf("    Version           : %s\n", platform.version().toLatin1().constData());
        printf("    Name              : %s\n", platform.name().toLatin1().constData());
        printf("    Vendor            : %s\n", platform.vendor().toLatin1().constData());
        printf("    Extension Suffix  : %s\n", platform.extensionSuffix().toLatin1().constData());
        printf("    Extensions        :\n");
        QStringList extns = platform.extensions();
        foreach (QString ext, extns)
            printf("        %s\n", ext.toLatin1().constData());
        printf("\n");
    }

    printf("OpenCL Devices:\n");
    QList<QCLDevice> devices = QCLDevice::allDevices();
    foreach (QCLDevice dev, devices) {
        printf("    Device ID         : %ld\n", long(dev.deviceId()));
        printf("    Platform ID       : %ld\n", long(dev.platform().platformId()));
        printf("    Vendor ID         : %u\n", dev.vendorId());
        printf("    Type              :");
        QCLDevice::DeviceTypes type = dev.deviceType();
        if (type & QCLDevice::Default)
            printf(" Default");
        if (type & QCLDevice::CPU)
            printf(" CPU");
        if (type & QCLDevice::GPU)
            printf(" GPU");
        if (type & QCLDevice::Accelerator)
            printf(" Accelerator");
        printf("\n");
        printf("    Profile           : %s\n", dev.profile().toLatin1().constData());
        printf("    Version           : %s\n", dev.version().toLatin1().constData());
        printf("    Driver Version    : %s\n", dev.driverVersion().toLatin1().constData());
        printf("    Language Version  : %s\n", dev.languageVersion().toLatin1().constData());
        printf("    Name              : %s\n", dev.name().toLatin1().constData());
        printf("    Vendor            : %s\n", dev.vendor().toLatin1().constData());
        printf("    Available         : %s\n",
               dev.isAvailable() ? "true" : "false");
        printf("    Compute Units     : %d\n", dev.computeUnits());
        printf("    Clock Frequency   : %d MHz\n", dev.clockFrequency());
        printf("    Address Bits      : %d\n", dev.addressBits());
        printf("    Byte Order        : %s\n",
               (dev.byteOrder() == QSysInfo::LittleEndian
                    ? "Little Endian" : "Big Endian"));
        QCLWorkSize size = dev.maximumWorkItemSize();
        printf("    Max Work Size     : %ux%ux%u\n",
               uint(size.width()), uint(size.height()),
               uint(size.depth()));
        printf("    Max Items/Group   : %u\n",
               uint(dev.maximumWorkItemsPerGroup()));
        printf("    Local Memory      : ");
        printMemorySize(dev.localMemorySize());
        printf("    Global Memory     : ");
        printMemorySize(dev.globalMemorySize());
        QCLDevice::CacheType cacheType = dev.globalMemoryCacheType();
        printf("    Global Cache Type : ");
        if (cacheType == QCLDevice::ReadOnlyCache)
            printf("ReadOnly\n");
        else if (cacheType == QCLDevice::ReadWriteCache)
            printf("ReadWrite\n");
        else
            printf("None\n");
        printf("    Global Cache Size : ");
        printMemorySize(dev.globalMemoryCacheSize());
        printf("    Cache Line Size   : %d\n",
               dev.globalMemoryCacheLineSize());
        printf("    Max Alloc Size    : ");
        printMemorySize(dev.maximumAllocationSize());
        printf("    Max Constant Size : ");
        printMemorySize(dev.maximumConstantBufferSize());
        printf("    Max Constant Args : %d\n",
               dev.maximumConstantArguments());
        printf("    Separate Local    : %s\n",
               dev.isLocalMemorySeparate() ? "true" : "false");
        printf("    Alignment         : %d (min %d)\n",
               dev.defaultAlignment(), dev.minimumAlignment());
        printf("    Max Param Bytes   : %d\n",
               dev.maximumParameterBytes());
        printf("    Error Correction  : %s\n",
               dev.hasErrorCorrectingMemory() ? "true" : "false");
        printf("    Unified Memory    : %s\n",
               dev.hasUnifiedMemory() ? "true" : "false");
        printf("    Float Support     :");
        printFloatCapabilities(dev.floatCapabilities());
        printf("    Double Support    :");
        printFloatCapabilities(dev.doubleCapabilities());
        printf("    Half Float Support:");
        printFloatCapabilities(dev.halfFloatCapabilities());
        if (dev.hasImage2D()) {
            if (dev.hasImage3D())
                printf("    Image Support     : 2D, 3D\n");
            else
                printf("    Image Support     : 2D\n");
        } else {
            printf("    Image Support     : No\n");
        }
        if (dev.hasImage2D()) {
            QSize size2d = dev.maximumImage2DSize();
            printf("    Max 2D Image Size : %dx%d\n",
                   size2d.width(), size2d.height());
            printf("    Max Samplers      : %d\n", dev.maximumSamplers());
            printf("    Max Read Images   : %d\n", dev.maximumReadImages());
            printf("    Max Write Images  : %d\n", dev.maximumWriteImages());
        }
        if (dev.hasImage3D()) {
            QCLWorkSize size3d = dev.maximumImage3DSize();
            printf("    Max 3D Image      : %ux%ux%u\n",
                   uint(size3d.width()), uint(size3d.height()),
                   uint(size3d.depth()));
            printf("    Writable 3D Images: %s\n",
                   dev.hasWritableImage3D() ? "true" : "false");
        }
        printf("    Has Compiler      : %s\n",
               dev.hasCompiler() ? "true" : "false");
        printf("    Native Kernels    : %s\n",
               dev.hasNativeKernels() ? "true" : "false");
        printf("    Out of Order Exec : %s\n",
               dev.hasOutOfOrderExecution() ? "true" : "false");
        printf("    Preferred Vector Sizes:\n");
        printf("        char%d, short%d, int%d, long%d, float%d",
               dev.preferredCharVectorSize(),
               dev.preferredShortVectorSize(),
               dev.preferredIntVectorSize(),
               dev.preferredLongVectorSize(),
               dev.preferredFloatVectorSize());
        int dsize = dev.preferredDoubleVectorSize();
        if (dsize)
            printf(", double%d", dsize);
        int hsize = dev.preferredHalfFloatVectorSize();
        if (hsize)
            printf(", half%d", hsize);
        printf("\n");
        if (dev.nativeCharVectorSize()) {
            printf("    Native Vector Sizes:\n");
            printf("        char%d, short%d, int%d, long%d, float%d",
                   dev.nativeCharVectorSize(),
                   dev.nativeShortVectorSize(),
                   dev.nativeIntVectorSize(),
                   dev.nativeLongVectorSize(),
                   dev.nativeFloatVectorSize());
            dsize = dev.nativeDoubleVectorSize();
            if (dsize)
                printf(", double%d", dsize);
            hsize = dev.nativeHalfFloatVectorSize();
            if (hsize)
                printf(", half%d", hsize);
            printf("\n");
        }
        printf("    Extensions        :\n");
        QStringList extns = dev.extensions();
        foreach (QString ext, extns)
            printf("        %s\n", ext.toLatin1().constData());
        printf("\n");
    }

    return 0;
}

void printFloatCapabilities(QCLDevice::FloatCapabilities caps)
{
    if (caps == QCLDevice::NotSupported) {
        printf(" No\n");
        return;
    }
    if ((caps & QCLDevice::Denorm) != 0)
        printf(" Denom");
    if ((caps & QCLDevice::InfinityNaN) != 0)
        printf(" InfinityNaN");
    if ((caps & QCLDevice::RoundNearest) != 0)
        printf(" RoundNearest");
    if ((caps & QCLDevice::RoundZero) != 0)
        printf(" RoundZero");
    if ((caps & QCLDevice::RoundInfinity) != 0)
        printf(" RoundInfinity");
    if ((caps & QCLDevice::FusedMultiplyAdd) != 0)
        printf(" FusedMultiplyAdd");
    printf("\n");
}

static void printMemorySize(quint64 size)
{
    if (size >= 1024 * 1024)
        printf("%d MB\n", (int)(size / (1024 * 1024)));
    else
        printf("%d kB\n", (int)(size / 1024));
}
