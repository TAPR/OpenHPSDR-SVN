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

#ifndef QCLDEVICE_H
#define QCLDEVICE_H

#include "qclplatform.h"
#include "qclworksize.h"

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(CL)

class Q_CL_EXPORT QCLDevice
{
public:
    QCLDevice() : m_id(0), m_flags(0) {}
    QCLDevice(cl_device_id id) : m_id(id), m_flags(0) {}

    enum DeviceType
    {
        Default             = 0x00000001,
        CPU                 = 0x00000002,
        GPU                 = 0x00000004,
        Accelerator         = 0x00000008,
        All                 = 0xFFFFFFFF
    };
    Q_DECLARE_FLAGS(DeviceTypes, DeviceType)

    bool isNull() const { return m_id == 0; }

    QCLDevice::DeviceTypes deviceType() const;
    QCLPlatform platform() const;
    uint vendorId() const;
    bool isAvailable() const;

    bool hasCompiler() const;
    bool hasNativeKernels() const;
    bool hasOutOfOrderExecution() const;
    bool hasDouble() const;
    bool hasHalfFloat() const;
    bool hasErrorCorrectingMemory() const;
    bool hasUnifiedMemory() const;

    int computeUnits() const;
    int clockFrequency() const;
    int addressBits() const;
    QSysInfo::Endian byteOrder() const;

    QCLWorkSize maximumWorkItemSize() const;
    size_t maximumWorkItemsPerGroup() const;

    bool hasImage2D() const;
    bool hasImage3D() const;
    bool hasWritableImage3D() const;
    QSize maximumImage2DSize() const;
    QCLWorkSize maximumImage3DSize() const;
    int maximumSamplers() const;
    int maximumReadImages() const;
    int maximumWriteImages() const;

    int preferredCharVectorSize() const;
    int preferredShortVectorSize() const;
    int preferredIntVectorSize() const;
    int preferredLongVectorSize() const;
    int preferredFloatVectorSize() const;
    int preferredDoubleVectorSize() const;
    int preferredHalfFloatVectorSize() const;

    int nativeCharVectorSize() const;
    int nativeShortVectorSize() const;
    int nativeIntVectorSize() const;
    int nativeLongVectorSize() const;
    int nativeFloatVectorSize() const;
    int nativeDoubleVectorSize() const;
    int nativeHalfFloatVectorSize() const;

    enum FloatCapability
    {
        NotSupported        = 0x0000,
        Denorm              = 0x0001,
        InfinityNaN         = 0x0002,
        RoundNearest        = 0x0004,
        RoundZero           = 0x0008,
        RoundInfinity       = 0x0010,
        FusedMultiplyAdd    = 0x0020
    };
    Q_DECLARE_FLAGS(FloatCapabilities, FloatCapability)

    QCLDevice::FloatCapabilities floatCapabilities() const;
    QCLDevice::FloatCapabilities doubleCapabilities() const;
    QCLDevice::FloatCapabilities halfFloatCapabilities() const;

    quint64 profilingTimerResolution() const;

    enum CacheType
    {
        NoCache             = 0,
        ReadOnlyCache       = 1,
        ReadWriteCache      = 2
    };

    quint64 maximumAllocationSize() const;
    quint64 globalMemorySize() const;
    QCLDevice::CacheType globalMemoryCacheType() const;
    quint64 globalMemoryCacheSize() const;
    int globalMemoryCacheLineSize() const;
    quint64 localMemorySize() const;
    bool isLocalMemorySeparate() const;
    quint64 maximumConstantBufferSize() const;
    int maximumConstantArguments() const;

    int defaultAlignment() const;
    int minimumAlignment() const;
    int maximumParameterBytes() const;

    bool isFullProfile() const;
    bool isEmbeddedProfile() const;

    QString profile() const;
    QString version() const;
    QString driverVersion() const;
    QString name() const;
    QString vendor() const;
    QStringList extensions() const;
    QString languageVersion() const;

    bool hasExtension(const char *name) const;

    QCLPlatform::VersionFlags versionFlags() const;

    cl_device_id deviceId() const { return m_id; }

    static QList<QCLDevice> allDevices();
    static QList<QCLDevice> devices
        (QCLDevice::DeviceTypes types,
         const QCLPlatform &platform = QCLPlatform());

    bool operator==(const QCLDevice &other) const;
    bool operator!=(const QCLDevice &other) const;

private:
    cl_device_id m_id;
    mutable int m_flags;
};

Q_DECLARE_OPERATORS_FOR_FLAGS(QCLDevice::DeviceTypes)
Q_DECLARE_OPERATORS_FOR_FLAGS(QCLDevice::FloatCapabilities)

inline bool QCLDevice::operator==(const QCLDevice &other) const
{
    return m_id == other.m_id;
}

inline bool QCLDevice::operator!=(const QCLDevice &other) const
{
    return m_id != other.m_id;
}

#ifndef QT_NO_DEBUG_STREAM
Q_CL_EXPORT QDebug operator<<(QDebug, const QCLDevice &);
#endif

QT_END_NAMESPACE

QT_END_HEADER

#endif
