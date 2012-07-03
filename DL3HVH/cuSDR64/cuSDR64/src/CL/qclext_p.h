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

#ifndef QCLEXT_P_H
#define QCLEXT_P_H

#include "qclglobal.h"

// This file provides standard and extension definitions
// that we cannot rely upon being present in the system headers.

// OpenCL 1.1
#ifndef CL_MISALIGNED_SUB_BUFFER_OFFSET
#define CL_MISALIGNED_SUB_BUFFER_OFFSET -13
#endif
#ifndef CL_EXEC_STATUS_ERROR_FOR_EVENTS_IN_WAIT_LIST
#define CL_EXEC_STATUS_ERROR_FOR_EVENTS_IN_WAIT_LIST -14
#endif
#ifndef CL_DEVICE_PREFERRED_VECTOR_WIDTH_HALF
#define CL_DEVICE_PREFERRED_VECTOR_WIDTH_HALF 0x1034
#endif
#ifndef CL_DEVICE_HOST_UNIFIED_MEMORY
#define CL_DEVICE_HOST_UNIFIED_MEMORY 0x1035
#endif
#ifndef CL_DEVICE_NATIVE_VECTOR_WIDTH_CHAR
#define CL_DEVICE_NATIVE_VECTOR_WIDTH_CHAR 0x1036
#define CL_DEVICE_NATIVE_VECTOR_WIDTH_SHORT 0x1037
#define CL_DEVICE_NATIVE_VECTOR_WIDTH_INT 0x1038
#define CL_DEVICE_NATIVE_VECTOR_WIDTH_LONG 0x1039
#define CL_DEVICE_NATIVE_VECTOR_WIDTH_FLOAT 0x103A
#define CL_DEVICE_NATIVE_VECTOR_WIDTH_DOUBLE 0x103B
#define CL_DEVICE_NATIVE_VECTOR_WIDTH_HALF 0x103C
#endif
#ifndef CL_DEVICE_OPENCL_C_VERSION
#define CL_DEVICE_OPENCL_C_VERSION 0x103D
#endif
#ifndef CL_COMMAND_READ_BUFFER_RECT
#define CL_COMMAND_READ_BUFFER_RECT 0x1201
#endif
#ifndef CL_COMMAND_WRITE_BUFFER_RECT
#define CL_COMMAND_WRITE_BUFFER_RECT 0x1202
#endif
#ifndef CL_COMMAND_COPY_BUFFER_RECT
#define CL_COMMAND_COPY_BUFFER_RECT 0x1203
#endif
#ifndef CL_COMMAND_USER
#define CL_COMMAND_USER 0x1204
#endif
#ifndef CL_MEM_ASSOCIATED_MEMOBJECT
#define CL_MEM_ASSOCIATED_MEMOBJECT 0x1107
#endif
#ifndef CL_MEM_OFFSET
#define CL_MEM_OFFSET 0x1108
#endif
#ifndef CL_KERNEL_PREFERRED_WORK_GROUP_SIZE_MULTIPLE
#define CL_KERNEL_PREFERRED_WORK_GROUP_SIZE_MULTIPLE 0x11B3
#endif

// OpenCL-OpenGL sharing.
#ifndef CL_INVALID_CL_SHAREGROUP_REFERENCE_KHR
#define CL_INVALID_CL_SHAREGROUP_REFERENCE_KHR -1000
#endif

// cl_khr_fp64
#ifndef CL_DEVICE_DOUBLE_FP_CONFIG
#define CL_DEVICE_DOUBLE_FP_CONFIG 0x1032
#endif

// cl_khr_fp16
#ifndef CL_DEVICE_HALF_FP_CONFIG
#define CL_DEVICE_HALF_FP_CONFIG 0x1033
#endif

// cl_khr_icd
#ifndef CL_PLATFORM_ICD_SUFFIX_KHR
#define CL_PLATFORM_ICD_SUFFIX_KHR 0x0920
#endif
#ifndef CL_PLATFORM_NOT_FOUND_KHR
#define CL_PLATFORM_NOT_FOUND_KHR -1001
#endif

// cl_ext_device_fission
#ifndef CL_DEVICE_PARTITION_FAILED_EXT
#define CL_DEVICE_PARTITION_FAILED_EXT -1057
#endif
#ifndef CL_INVALID_PARTITION_COUNT_EXT
#define CL_INVALID_PARTITION_COUNT_EXT -1058
#endif
#ifndef CL_INVALID_PARTITION_NAME_EXT
#define CL_INVALID_PARTITION_NAME_EXT -1059
#endif

#endif
