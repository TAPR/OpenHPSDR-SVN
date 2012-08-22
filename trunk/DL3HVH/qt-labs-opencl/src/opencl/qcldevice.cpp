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

#include "qcldevice.h"
#include "qclext_p.h"
#include <QtCore/qvarlengtharray.h>
#include <QtCore/qdebug.h>

QT_BEGIN_NAMESPACE

/*!
    \class QCLDevice
    \brief The QCLDevice class represents an OpenCL device definition.
    \since 4.7
    \ingroup opencl

    A device is a collection of compute units that can be used by a
    QCLContext to build programs, execute kernels, and queue
    commands.

    Devices have an associated deviceType(), which is usually
    QCLDevice::CPU or QCLDevice::GPU.  CPU devices typically
    execute on the host CPU.  GPU devices typically execute on
    the same computational resource as the system's OpenGL
    implementation, and it may be possible to share OpenGL
    objects with OpenCL using the facilities of QCLContextGL.

    The devices() function can be used to find all devices of a
    specific type, optionally constrained by the QCLPlatform
    they belong to.

    The \l{Querying OpenCL Device Capabilities}{clinfo} utility
    program can be used to dump all of the devices that are
    supported by the system's OpenCL implementation.

    \sa QCLPlatform, QCLContext
*/

/*!
    \fn QCLDevice::QCLDevice()

    Constructs a default OpenCL device identifier.
*/

/*!
    \fn QCLDevice::QCLDevice(cl_device_id id)

    Constructs an OpenCL device identifier that corresponds to the
    native OpenCL value \a id.
*/

/*!
    \fn bool QCLDevice::isNull() const

    Returns true if this OpenCL device identifier is null.
*/

/*!
    \enum QCLDevice::DeviceType
    This enum defines the type of OpenCL device that is represented
    by a QCLDevice object.

    \value Default The default OpenCL device.
    \value CPU The host CPU within the OpenCL system.
    \value GPU An OpenCL device that is also an OpenGL GPU.
    \value Accelerator Dedicated OpenCL accelerator.
    \value All All OpenCL device types.
*/

static uint qt_cl_paramUInt(cl_device_id id, cl_device_info name)
{
    cl_uint value;
    if (!id || clGetDeviceInfo(id, name, sizeof(value), &value, 0)
            != CL_SUCCESS)
        return 0;
    else
        return uint(value);
}

static int qt_cl_paramInt(cl_device_id id, cl_device_info name)
{
    cl_int value;
    if (!id || clGetDeviceInfo(id, name, sizeof(value), &value, 0)
            != CL_SUCCESS)
        return 0;
    else
        return int(value);
}

static quint64 qt_cl_paramULong(cl_device_id id, cl_device_info name)
{
    cl_ulong value;
    if (!id || clGetDeviceInfo(id, name, sizeof(value), &value, 0)
            != CL_SUCCESS)
        return 0;
    else
        return quint64(value);
}

static size_t qt_cl_paramSize(cl_device_id id, cl_device_info name)
{
    size_t value;
    if (!id || clGetDeviceInfo(id, name, sizeof(value), &value, 0)
            != CL_SUCCESS)
        return 0;
    else
        return value;
}

static bool qt_cl_paramBool(cl_device_id id, cl_device_info name)
{
    cl_bool value;
    if (!id || clGetDeviceInfo(id, name, sizeof(value), &value, 0)
            != CL_SUCCESS)
        return false;
    else
        return value != 0;
}

static QString qt_cl_paramString(cl_device_id id, cl_device_info name)
{
    size_t size;
    if (!id || clGetDeviceInfo(id, name, 0, 0, &size) != CL_SUCCESS)
        return QString();
    QVarLengthArray<char> buf(size);
    clGetDeviceInfo(id, name, size, buf.data(), &size);
    return QString::fromLatin1(buf.data());
}

static bool qt_cl_isParamString
    (cl_device_id id, cl_device_info name, const char *str)
{
    size_t len = qstrlen(str);
    size_t size;
    if (!id || clGetDeviceInfo(id, name, 0, 0, &size) != CL_SUCCESS)
        return false;
    if (size <= len)
        return false;
    QVarLengthArray<char> buf(size);
    clGetDeviceInfo(id, name, size, buf.data(), &size);
    if (qstrncmp(buf.constData(), str, len) != 0)
        return false;
    return buf[len] == '\0';
}

/*!
    Returns the type of this device.  It is possible for a device
    to have more than one type.
*/
QCLDevice::DeviceTypes QCLDevice::deviceType() const
{
    cl_device_type type;
    if (!m_id || clGetDeviceInfo(m_id, CL_DEVICE_TYPE, sizeof(type), &type, 0)
            != CL_SUCCESS)
        return QCLDevice::DeviceTypes(0);
    else
        return QCLDevice::DeviceTypes(type);
}

/*!
    Returns the platform identifier for this device.
*/
QCLPlatform QCLDevice::platform() const
{
    cl_platform_id plat;
    if (!m_id || clGetDeviceInfo(m_id, CL_DEVICE_PLATFORM, sizeof(plat), &plat, 0)
            != CL_SUCCESS)
        return QCLPlatform();
    else
        return QCLPlatform(plat);
}

/*!
    Returns the vendor's identifier for this device.
*/
uint QCLDevice::vendorId() const
{
    return qt_cl_paramUInt(m_id, CL_DEVICE_VENDOR_ID);
}

/*!
    Returns true if this device is available; false otherwise.
*/
bool QCLDevice::isAvailable() const
{
    return qt_cl_paramBool(m_id, CL_DEVICE_AVAILABLE);
}

/*!
    Returns true if this device has a compiler available;
    false otherwise.
*/
bool QCLDevice::hasCompiler() const
{
    return qt_cl_paramBool(m_id, CL_DEVICE_COMPILER_AVAILABLE);
}

/*!
    Returns true if this device has support for executing
    native kernels; false otherwise.
*/
bool QCLDevice::hasNativeKernels() const
{
    cl_device_exec_capabilities caps;
    if (!m_id || clGetDeviceInfo(m_id, CL_DEVICE_EXECUTION_CAPABILITIES,
                        sizeof(caps), &caps, 0)
            != CL_SUCCESS)
        return false;
    else
        return (caps & CL_EXEC_NATIVE_KERNEL) != 0;
}

/*!
    Returns true if this device supports out of order execution
    of commands on a QCLCommandQueue; false otherwise.

    \sa QCLCommandQueue::isOutOfOrder()
*/
bool QCLDevice::hasOutOfOrderExecution() const
{
    cl_command_queue_properties props;
    if (!m_id || clGetDeviceInfo(m_id, CL_DEVICE_QUEUE_PROPERTIES,
                        sizeof(props), &props, 0)
            != CL_SUCCESS)
        return false;
    else
        return (props & CL_QUEUE_OUT_OF_ORDER_EXEC_MODE_ENABLE) != 0;
}

/*!
    Returns true if this device supports the \c{double} type
    via the \c{cl_khr_fp64} extension; false otherwise.
*/
bool QCLDevice::hasDouble() const
{
    return hasExtension("cl_khr_fp64");
}

/*!
    Returns true if this device supports operations on the
    \c{half} type via the \c{cl_khr_fp16} extension;
    false otherwise.

    Note: \c{half} is supported by the OpenCL 1.0 core specification
    for data storage even if this function returns false.
    However, kernels can only perform arithmetic operations on
    \c{half} values if this function returns true.
*/
bool QCLDevice::hasHalfFloat() const
{
    return hasExtension("cl_khr_fp16");
}

/*!
    Returns true if the device implements error correction on
    its memory areas; false otherwise.
*/
bool QCLDevice::hasErrorCorrectingMemory() const
{
    return qt_cl_paramBool(m_id, CL_DEVICE_ERROR_CORRECTION_SUPPORT);
}

/*!
    Returns true if the device and the host share a unified
    memory address space; false otherwise.

    Note: OpenCL 1.0 devices will usually report false from
    this function even if they have unified memory because
    the relevant device query is specific to OpenCL 1.1.
*/
bool QCLDevice::hasUnifiedMemory() const
{
    return qt_cl_paramBool(m_id, CL_DEVICE_HOST_UNIFIED_MEMORY);
}

/*!
    Returns the number of parallel compute units on the device.
*/
int QCLDevice::computeUnits() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_MAX_COMPUTE_UNITS);
}

/*!
    Returns the maximum clock frequency for this device in MHz.
*/
int QCLDevice::clockFrequency() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_MAX_CLOCK_FREQUENCY);
}

/*!
    Returns the number of address bits used by the device;
    usually 32 or 64.
*/
int QCLDevice::addressBits() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_ADDRESS_BITS);
}

/*!
    Returns the byte order of the device, indicating little
    endian or big endian.
*/
QSysInfo::Endian QCLDevice::byteOrder() const
{
    if (qt_cl_paramBool(m_id, CL_DEVICE_ENDIAN_LITTLE))
        return QSysInfo::LittleEndian;
    else
        return QSysInfo::BigEndian;
}

/*!
    Returns the maximum work size for this device.

    \sa maximumWorkItemsPerGroup()
*/
QCLWorkSize QCLDevice::maximumWorkItemSize() const
{
    size_t dims = 0;
    if (!m_id || clGetDeviceInfo(m_id, CL_DEVICE_MAX_WORK_ITEM_DIMENSIONS,
                        sizeof(dims), &dims, 0) != CL_SUCCESS || !dims)
        return QCLWorkSize(1, 1, 1);
    QVarLengthArray<size_t> buf(dims);
    clGetDeviceInfo(m_id, CL_DEVICE_MAX_WORK_ITEM_SIZES,
                    sizeof(size_t) * dims, buf.data(), 0);
    if (dims == 1)
        return QCLWorkSize(buf[0]);
    else if (dims == 2)
        return QCLWorkSize(buf[0], buf[1]);
    else
        return QCLWorkSize(buf[0], buf[1], buf[2]);
}

/*!
    Returns the maximum number of work items in a work group executing a
    kernel using data parallel execution.

    \sa maximumWorkItemSize()
*/
size_t QCLDevice::maximumWorkItemsPerGroup() const
{
    return qt_cl_paramSize(m_id, CL_DEVICE_MAX_WORK_GROUP_SIZE);
}

/*!
    Returns true if this device has 2D image support; false otherwise.
*/
bool QCLDevice::hasImage2D() const
{
    return qt_cl_paramBool(m_id, CL_DEVICE_IMAGE_SUPPORT);
}

/*!
    Returns true if this device has 3D image support; false otherwise.
*/
bool QCLDevice::hasImage3D() const
{
    if (!qt_cl_paramBool(m_id, CL_DEVICE_IMAGE_SUPPORT))
        return false;
    return qt_cl_paramSize(m_id, CL_DEVICE_IMAGE3D_MAX_WIDTH) != 0 ||
           qt_cl_paramSize(m_id, CL_DEVICE_IMAGE3D_MAX_HEIGHT) != 0 ||
           qt_cl_paramSize(m_id, CL_DEVICE_IMAGE3D_MAX_DEPTH) != 0;
}

/*!
    Returns true if this device supports writing to 3D images
    via the \c{cl_khr_3d_image_writes} extension; false otherwise.
*/
bool QCLDevice::hasWritableImage3D() const
{
    return hasExtension("cl_khr_3d_image_writes");
}

/*!
    Returns the maximum size of 2D images that are supported
    by this device; or an empty QSize if images are not supported.

    \sa maximumImage3DSize(), hasImage2D()
*/
QSize QCLDevice::maximumImage2DSize() const
{
    if (!qt_cl_paramBool(m_id, CL_DEVICE_IMAGE_SUPPORT))
        return QSize();
    return QSize(qt_cl_paramSize(m_id, CL_DEVICE_IMAGE2D_MAX_WIDTH),
                 qt_cl_paramSize(m_id, CL_DEVICE_IMAGE2D_MAX_HEIGHT));
}

/*!
    Returns the maximum size of 3D images that are supported
    by this device; or (0, 0, 0) if images are not supported.

    \sa maximumImage2DSize(), hasImage3D()
*/
QCLWorkSize QCLDevice::maximumImage3DSize() const
{
    if (!qt_cl_paramBool(m_id, CL_DEVICE_IMAGE_SUPPORT))
        return QCLWorkSize(0, 0, 0);
    return QCLWorkSize(qt_cl_paramSize(m_id, CL_DEVICE_IMAGE3D_MAX_WIDTH),
                       qt_cl_paramSize(m_id, CL_DEVICE_IMAGE3D_MAX_HEIGHT),
                       qt_cl_paramSize(m_id, CL_DEVICE_IMAGE3D_MAX_DEPTH));
}

/*!
    Returns the maximum number of image samplers that can be used
    in a kernel at one time; 0 if images are not supported.
*/
int QCLDevice::maximumSamplers() const
{
    if (!qt_cl_paramBool(m_id, CL_DEVICE_IMAGE_SUPPORT))
        return 0;
    return qt_cl_paramInt(m_id, CL_DEVICE_MAX_SAMPLERS);
}

/*!
    Returns the maximum number of image objects that can be
    read simultaneously by a kernel; 0 if images are not supported.

    \sa maximumWriteImages()
*/
int QCLDevice::maximumReadImages() const
{
    if (!qt_cl_paramBool(m_id, CL_DEVICE_IMAGE_SUPPORT))
        return 0;
    return qt_cl_paramInt(m_id, CL_DEVICE_MAX_READ_IMAGE_ARGS);
}

/*!
    Returns the maximum number of image objects that can be
    written simultaneously by a kernel; 0 if images are not supported.

    \sa maximumReadImages()
*/
int QCLDevice::maximumWriteImages() const
{
    if (!qt_cl_paramBool(m_id, CL_DEVICE_IMAGE_SUPPORT))
        return 0;
    return qt_cl_paramInt(m_id, CL_DEVICE_MAX_WRITE_IMAGE_ARGS);
}

/*!
    Returns the preferred size for vectors of type \c{char}
    in the device.  For example, 4 indicates that 4 \c{char}
    values can be packed into a vector and operated on as a
    unit for optimal performance.
*/
int QCLDevice::preferredCharVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_PREFERRED_VECTOR_WIDTH_CHAR);
}

/*!
    Returns the preferred size for vectors of type \c{short}
    in the device.  For example, 4 indicates that 4 \c{short}
    values can be packed into a vector and operated on as a
    unit for optimal performance.
*/
int QCLDevice::preferredShortVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_PREFERRED_VECTOR_WIDTH_SHORT);
}

/*!
    Returns the preferred size for vectors of type \c{int}
    in the device.  For example, 4 indicates that 4 \c{int}
    values can be packed into a vector and operated on as a
    unit for optimal performance.
*/
int QCLDevice::preferredIntVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_PREFERRED_VECTOR_WIDTH_INT);
}

/*!
    Returns the preferred size for vectors of type \c{long}
    in the device.  For example, 2 indicates that 2 \c{long}
    values can be packed into a vector and operated on as a
    unit for optimal performance.
*/
int QCLDevice::preferredLongVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_PREFERRED_VECTOR_WIDTH_LONG);
}

/*!
    Returns the preferred size for vectors of type \c{float}
    in the device.  For example, 4 indicates that 4 \c{float}
    values can be packed into a vector and operated on as a
    unit for optimal performance.
*/
int QCLDevice::preferredFloatVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_PREFERRED_VECTOR_WIDTH_FLOAT);
}

/*!
    Returns the preferred size for vectors of type \c{double}
    in the device.  For example, 2 indicates that 2 \c{double}
    values can be packed into a vector and operated on as a
    unit for optimal performance.

    Returns zero if the device does not support \c{double}.

    \sa hasDouble()
*/
int QCLDevice::preferredDoubleVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_PREFERRED_VECTOR_WIDTH_DOUBLE);
}

/*!
    Returns the preferred size for vectors of type \c{half}
    in the device.  For example, 2 indicates that 2 \c{half}
    values can be packed into a vector and operated on as a
    unit for optimal performance.

    Returns zero if the device does not support \c{half},
    or the device does not support OpenCL 1.1.

    \sa hasHalfFloat()
*/
int QCLDevice::preferredHalfFloatVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_PREFERRED_VECTOR_WIDTH_HALF);
}

/*!
    Returns the native size for vectors of type \c{char}
    in the device.  For example, 4 indicates that 4 \c{char}
    values can be packed into a vector and operated on as a
    unit for optimal performance.  Returns zero on OpenCL 1.0.
*/
int QCLDevice::nativeCharVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_NATIVE_VECTOR_WIDTH_CHAR);
}

/*!
    Returns the native size for vectors of type \c{short}
    in the device.  For example, 4 indicates that 4 \c{short}
    values can be packed into a vector and operated on as a
    unit for optimal performance.  Returns zero on OpenCL 1.0.
*/
int QCLDevice::nativeShortVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_NATIVE_VECTOR_WIDTH_SHORT);
}

/*!
    Returns the native size for vectors of type \c{int}
    in the device.  For example, 4 indicates that 4 \c{int}
    values can be packed into a vector and operated on as a
    unit for optimal performance.  Returns zero on OpenCL 1.0.
*/
int QCLDevice::nativeIntVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_NATIVE_VECTOR_WIDTH_INT);
}

/*!
    Returns the native size for vectors of type \c{long}
    in the device.  For example, 2 indicates that 2 \c{long}
    values can be packed into a vector and operated on as a
    unit for optimal performance.  Returns zero on OpenCL 1.0.
*/
int QCLDevice::nativeLongVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_NATIVE_VECTOR_WIDTH_LONG);
}

/*!
    Returns the native size for vectors of type \c{float}
    in the device.  For example, 4 indicates that 4 \c{float}
    values can be packed into a vector and operated on as a
    unit for optimal performance.  Returns zero on OpenCL 1.0.
*/
int QCLDevice::nativeFloatVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_NATIVE_VECTOR_WIDTH_FLOAT);
}

/*!
    Returns the native size for vectors of type \c{double}
    in the device.  For example, 2 indicates that 2 \c{double}
    values can be packed into a vector and operated on as a
    unit for optimal performance.  Returns zero on OpenCL 1.0,
    or if the device does not support \c{double}.

    \sa hasDouble()
*/
int QCLDevice::nativeDoubleVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_NATIVE_VECTOR_WIDTH_DOUBLE);
}

/*!
    Returns the native size for vectors of type \c{half}
    in the device.  For example, 2 indicates that 2 \c{half}
    values can be packed into a vector and operated on as a
    unit for optimal performance.  Returns zero on OpenCL 1.0,
    or if the device does not support \c{half}.

    \sa hasHalfFloat()
*/
int QCLDevice::nativeHalfFloatVectorSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_NATIVE_VECTOR_WIDTH_HALF);
}

/*!
    \enum QCLDevice::FloatCapability
    This enum defines the floating-point capabilities of the
    \c{float} or \c{double} type on an OpenCL device.

    \value NotSupported Returned to indicate that \c{double} is
           not supported on the device.
    \value Denorm Denorms are supported.
    \value InfinityNaN Infinity and quiet NaN's are supported.
    \value RoundNearest Round to nearest even rounding mode supported.
    \value RoundZero Round to zero rounding mode supported.
    \value RoundInfinity Round to infinity rounding mode supported.
    \value FusedMultiplyAdd IEEE754-2008 fused multiply-add
           is supported.
*/

/*!
    Returns a set of flags that describes the floating-point
    capabilities of the \c{float} type on this device.
*/
QCLDevice::FloatCapabilities QCLDevice::floatCapabilities() const
{
    cl_device_fp_config config;
    if (!m_id || clGetDeviceInfo(m_id, CL_DEVICE_SINGLE_FP_CONFIG,
                        sizeof(config), &config, 0)
            != CL_SUCCESS)
        return NotSupported;
    else
        return QCLDevice::FloatCapabilities(config);
}

/*!
    Returns a set of flags that describes the floating-point
    capabilities of the \c{double} type on this device.

    Returns QCLDevice::NotSupported if operations on \c{double}
    are not supported by the device.

    \sa hasDouble()
*/
QCLDevice::FloatCapabilities QCLDevice::doubleCapabilities() const
{
    cl_device_fp_config config;
    if (!m_id || clGetDeviceInfo(m_id, CL_DEVICE_DOUBLE_FP_CONFIG,
                        sizeof(config), &config, 0)
            != CL_SUCCESS)
        return NotSupported;
    else
        return QCLDevice::FloatCapabilities(config);
}

/*!
    Returns a set of flags that describes the floating-point
    capabilities of the \c{half} type on this device.

    Returns QCLDevice::NotSupported if operations on \c{half}
    are not supported by the device.

    \sa hasHalfFloat()
*/
QCLDevice::FloatCapabilities QCLDevice::halfFloatCapabilities() const
{
    cl_device_fp_config config;
    if (!m_id || clGetDeviceInfo(m_id, CL_DEVICE_HALF_FP_CONFIG,
                        sizeof(config), &config, 0)
            != CL_SUCCESS)
        return NotSupported;
    else
        return QCLDevice::FloatCapabilities(config);
}

/*!
    Returns the resolution of the device profiling timer in
    nanoseconds.

    \sa QCLEvent::finishTime()
*/
quint64 QCLDevice::profilingTimerResolution() const
{
    // Spec says size_t, even though actual times are cl_ulong.
    return qt_cl_paramSize(m_id, CL_DEVICE_PROFILING_TIMER_RESOLUTION);
}

/*!
    Returns the maximum memory allocation size in bytes.

    \sa globalMemorySize()
*/
quint64 QCLDevice::maximumAllocationSize() const
{
    return qt_cl_paramULong(m_id, CL_DEVICE_MAX_MEM_ALLOC_SIZE);
}

/*!
    Returns the number of bytes of global memory in the device.

    \sa globalMemoryCacheSize(), localMemorySize()
*/
quint64 QCLDevice::globalMemorySize() const
{
    return qt_cl_paramULong(m_id, CL_DEVICE_GLOBAL_MEM_SIZE);
}

/*!
    \enum QCLDevice::CacheType
    This enum defines the type of global memory cache that is
    supported by an OpenCL device.

    \value NoCache No global memory cache.
    \value ReadOnlyCache Read-only global memory cache.
    \value ReadWriteCache Read-write global memory cache.
*/

/*!
    Returns the type of global memory cache that is supported
    by the device.
*/
QCLDevice::CacheType QCLDevice::globalMemoryCacheType() const
{
    cl_device_mem_cache_type type;
    if (!m_id || clGetDeviceInfo(m_id, CL_DEVICE_GLOBAL_MEM_CACHE_TYPE,
                        sizeof(type), &type, 0)
            != CL_SUCCESS)
        return NoCache;
    else
        return QCLDevice::CacheType(type);
}

/*!
    Returns the size of the global memory cache in bytes.

    \sa globalMemorySize(), globalMemoryCacheLineSize()
*/
quint64 QCLDevice::globalMemoryCacheSize() const
{
    return qt_cl_paramULong(m_id, CL_DEVICE_GLOBAL_MEM_CACHE_SIZE);
}

/*!
    Returns the size of a single global memory cache line in bytes.

    \sa globalMemoryCacheSize()
*/
int QCLDevice::globalMemoryCacheLineSize() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_GLOBAL_MEM_CACHELINE_SIZE);
}

/*!
    Returns the number of bytes of local memory in the device.

    \sa globalMemorySize(), isLocalMemorySeparate()
*/
quint64 QCLDevice::localMemorySize() const
{
    return qt_cl_paramULong(m_id, CL_DEVICE_LOCAL_MEM_SIZE);
}

/*!
    Returns true if the local memory on this device is in a separate
    dedicated storage area from global memory; false if local memory
    is allocated from global memory.

    \sa localMemorySize()
*/
bool QCLDevice::isLocalMemorySeparate() const
{
    cl_device_local_mem_type type;
    if (!m_id || clGetDeviceInfo(m_id, CL_DEVICE_LOCAL_MEM_TYPE,
                        sizeof(type), &type, 0)
            != CL_SUCCESS)
        return false;
    else
        return type == CL_LOCAL;
}

/*!
    Returns the maximum size for a constant buffer allocation.

    \sa maximumConstantArguments()
*/
quint64 QCLDevice::maximumConstantBufferSize() const
{
    return qt_cl_paramULong(m_id, CL_DEVICE_MAX_CONSTANT_BUFFER_SIZE);
}

/*!
    Returns the maximum number of constant arguments that can
    be passed to a kernel.

    \sa maximumConstantBufferSize()
*/
int QCLDevice::maximumConstantArguments() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_MAX_CONSTANT_ARGS);
}

/*!
    Returns the default alignment for allocated memory in bytes.

    \sa minimumAlignment()
*/
int QCLDevice::defaultAlignment() const
{
    // OpenCL setting is in bits, but that is inconsistent with
    // every other alignment value, so return bytes instead.
    return qt_cl_paramInt(m_id, CL_DEVICE_MEM_BASE_ADDR_ALIGN) / 8;
}

/*!
    Returns the minimum alignment for any data type in bytes.

    \sa defaultAlignment()
*/
int QCLDevice::minimumAlignment() const
{
    return qt_cl_paramInt(m_id, CL_DEVICE_MIN_DATA_TYPE_ALIGN_SIZE);
}

/*!
    Returns the maximum number of parameter bytes that can be passed
    to a kernel.
*/
int QCLDevice::maximumParameterBytes() const
{
    return int(qt_cl_paramSize(m_id, CL_DEVICE_MAX_PARAMETER_SIZE));
}

/*!
    Returns true if profile() is \c FULL_PROFILE; false otherwise.

    \sa isEmbeddedProfile()
*/
bool QCLDevice::isFullProfile() const
{
    return qt_cl_isParamString(m_id, CL_DEVICE_PROFILE, "FULL_PROFILE");
}

/*!
    Returns true if profile() is \c EMBEDDED_PROFILE; false otherwise.

    \sa isFullProfile()
*/
bool QCLDevice::isEmbeddedProfile() const
{
    return qt_cl_isParamString(m_id, CL_DEVICE_PROFILE, "EMBEDDED_PROFILE");
}

/*!
    Returns the profile that is implemented by this OpenCL device,
    usually \c FULL_PROFILE or \c EMBEDDED_PROFILE.

    \sa isFullProfile(), isEmbeddedProfile()
*/
QString QCLDevice::profile() const
{
    return qt_cl_paramString(m_id, CL_DEVICE_PROFILE);
}

/*!
    Returns the OpenCL version that is implemented by this OpenCL device,
    usually something like \c{OpenCL 1.0}.

    The versionFlags() function parses the version into flag bits
    that are easier to test than the string returned by version().

    \sa versionFlags(), driverVersion()
*/
QString QCLDevice::version() const
{
    return qt_cl_paramString(m_id, CL_DEVICE_VERSION);
}

/*!
    Returns the driver version of this OpenCL device.

    \sa version()
*/
QString QCLDevice::driverVersion() const
{
    return qt_cl_paramString(m_id, CL_DRIVER_VERSION);
}

/*!
    Returns the name of this OpenCL device.
*/
QString QCLDevice::name() const
{
    return qt_cl_paramString(m_id, CL_DEVICE_NAME);
}

/*!
    Returns the vendor of this OpenCL device.
*/
QString QCLDevice::vendor() const
{
    return qt_cl_paramString(m_id, CL_DEVICE_VENDOR);
}

/*!
    Returns a list of the extensions supported by this OpenCL device.

    \sa hasExtension()
*/
QStringList QCLDevice::extensions() const
{
    if (!m_id)
        return QStringList();
    QString extns = qt_cl_paramString(m_id, CL_DEVICE_EXTENSIONS).simplified();
    if (!extns.isEmpty())
        return extns.split(QChar(' '));
    else
        return QStringList();
}

/*!
    Returns the highest version of the OpenCL language supported by
    this device's compiler.  For example, \c{OpenCL 1.1}.
*/
QString QCLDevice::languageVersion() const
{
    QString vers = qt_cl_paramString(m_id, CL_DEVICE_OPENCL_C_VERSION);

    // The define was introduced in OpenCL 1.1.  If the device is
    // only OpenCL 1.0 and doesn't respond to the query, then assume
    // that the device supports at least the OpenCL 1.0 language.
    if (vers.isEmpty() && !(versionFlags() & QCLPlatform::Version_1_1))
        vers = QLatin1String("OpenCL 1.0");

    return vers;
}

bool qt_cl_has_extension(const char *list, size_t listLen, const char *name)
{
    size_t nameLen = qstrlen(name);
    size_t tempLen;
    while (listLen > 0) {
        if (*list == '\0') {
            break;
        } else if (*list == ' ') {
            ++list;
            --listLen;
            continue;
        }
        tempLen = 0;
        while (tempLen < listLen &&
                    list[tempLen] != ' ' && list[tempLen] != '\0')
            ++tempLen;
        if (nameLen == tempLen && !qstrnicmp(list, name, nameLen))
            return true;
        list += tempLen;
        listLen -= tempLen;
    }
    return false;
}

/*!
    Returns true if this device has an extension called \a name;
    false otherwise.

    This function is more efficient than checking for \a name
    in the return value from extensions(), if the caller is only
    interested in a single extension.  Use extensions() to check
    for several extensions at once.

    \sa extensions()
*/
bool QCLDevice::hasExtension(const char *name) const
{
    size_t size;
    if (!m_id || clGetDeviceInfo(m_id, CL_DEVICE_EXTENSIONS, 0, 0, &size) != CL_SUCCESS)
        return false;
    QVarLengthArray<char> buf(size);
    clGetDeviceInfo(m_id, CL_DEVICE_EXTENSIONS, size, buf.data(), &size);
    return qt_cl_has_extension(buf.constData(), size, name);
}

int qt_cl_version_flags(const QString &version)
{
    if (!version.startsWith(QLatin1String("OpenCL ")))
        return 0;
    int index = 7;
    int major = 0;
    int minor = 0;
    while (index < version.length()) {
        int ch = version[index].unicode();
        if (ch < '0' || ch > '9')
            break;
        major = major * 10 + (ch - '0');
        ++index;
    }
    if (index < version.length() && version[index] == QChar('.')) {
        ++index;
        while (index < version.length()) {
            int ch = version[index].unicode();
            if (ch < '0' || ch > '9')
                break;
            minor = minor * 10 + (ch - '0');
            ++index;
        }
    }
    int flags = 0;
    if (major >= 1)
        flags |= QCLPlatform::Version_1_0;
    if ((major == 1 && minor >= 1) || major >= 2)
        flags |= QCLPlatform::Version_1_1;
    return flags;
}

/*!
    Returns the OpenCL versions supported by this device.

    \sa version(), QCLPlatform::versionFlags()
*/
QCLPlatform::VersionFlags QCLDevice::versionFlags() const
{
    if (!m_flags) {
        m_flags = qt_cl_version_flags
            (qt_cl_paramString(m_id, CL_DEVICE_VERSION));
    }
    return QCLPlatform::VersionFlags(m_flags);
}

/*!
    \fn cl_device_id QCLDevice::deviceId() const

    Returns the native OpenCL device identifier for this object.
*/

/*!
    Returns a list of all OpenCL devices on all platforms on this system.

    \sa devices()
*/
QList<QCLDevice> QCLDevice::allDevices()
{
    QList<QCLPlatform> platforms = QCLPlatform::platforms();
    QList<QCLDevice> devs;
    for (int plat = 0; plat < platforms.size(); ++plat) {
        cl_uint size;
        if (clGetDeviceIDs(platforms[plat].platformId(), CL_DEVICE_TYPE_ALL,
                           0, 0, &size) != CL_SUCCESS)
            continue;
        QVarLengthArray<cl_device_id> buf(size);
        clGetDeviceIDs(platforms[plat].platformId(), CL_DEVICE_TYPE_ALL,
                       size, buf.data(), &size);
        for (int index = 0; index < buf.size(); ++index)
            devs.append(QCLDevice(buf[index]));
    }
    return devs;
}

/*!
    Returns a list of all OpenCL devices that match \a types on
    \a platform on this system.  If \a platform is null, then
    the first platform that has devices matching \a types will
    be used.

    \sa allDevices()
*/
QList<QCLDevice> QCLDevice::devices
    (QCLDevice::DeviceTypes types, const QCLPlatform &platform)
{
    QList<QCLDevice> devs;
    QList<QCLPlatform> platforms;
    if (platform.isNull())
        platforms = QCLPlatform::platforms();
    else
        platforms.append(platform);
    for (int plat = 0; plat < platforms.size(); ++plat) {
        cl_uint size;
        if (clGetDeviceIDs(platforms[plat].platformId(), cl_device_type(types),
                           0, 0, &size) != CL_SUCCESS)
            continue;
        if (!size)
            continue;
        QVarLengthArray<cl_device_id> buf(size);
        clGetDeviceIDs(platforms[plat].platformId(), cl_device_type(types),
                       size, buf.data(), &size);
        for (int index = 0; index < buf.size(); ++index)
            devs.append(QCLDevice(buf[index]));
        break;
    }
    return devs;
}

/*!
    \fn bool QCLDevice::operator==(const QCLDevice &other) const

    Returns true if this OpenCL device identifier is the same
    as \a other; false otherwise.

    \sa operator!=()
*/

/*!
    \fn bool QCLDevice::operator!=(const QCLDevice &other) const

    Returns true if this OpenCL device identifier is not the
    same as \a other; false otherwise.

    \sa operator==()
*/

#ifndef QT_NO_DEBUG_STREAM

QDebug operator<<(QDebug dbg, const QCLDevice &device)
{
    dbg.nospace() << "QCLDevice(" << device.name() << ')';
    return dbg.space();
}

#endif

QT_END_NAMESPACE
