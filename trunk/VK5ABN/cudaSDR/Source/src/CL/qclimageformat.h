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

#ifndef QCLIMAGEFORMAT_H
#define QCLIMAGEFORMAT_H

#include "qclglobal.h"
#include <QtGui/qimage.h>

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(CL)

class QCLContext;

class Q_CL_EXPORT QCLImageFormat
{
public:
    enum ChannelOrder
    {
        Order_R                     = 0x10B0,
        Order_A                     = 0x10B1,
        Order_RG                    = 0x10B2,
        Order_RA                    = 0x10B3,
        Order_RGB                   = 0x10B4,
        Order_RGBA                  = 0x10B5,
        Order_BGRA                  = 0x10B6,
        Order_ARGB                  = 0x10B7,
        Order_Intensity             = 0x10B8,
        Order_Luminence             = 0x10B9,
        Order_Rx                    = 0x10BA,   // OpenCL 1.1
        Order_RGx                   = 0x10BB,   // OpenCL 1.1
        Order_RGBx                  = 0x10BC    // OpenCL 1.1
    };

    enum ChannelType
    {
        Type_Normalized_Int8        = 0x10D0,
        Type_Normalized_Int16       = 0x10D1,
        Type_Normalized_UInt8       = 0x10D2,
        Type_Normalized_UInt16      = 0x10D3,
        Type_Normalized_565         = 0x10D4,
        Type_Normalized_555         = 0x10D5,
        Type_Normalized_101010      = 0x10D6,
        Type_Unnormalized_Int8      = 0x10D7,
        Type_Unnormalized_Int16     = 0x10D8,
        Type_Unnormalized_Int32     = 0x10D9,
        Type_Unnormalized_UInt8     = 0x10DA,
        Type_Unnormalized_UInt16    = 0x10DB,
        Type_Unnormalized_UInt32    = 0x10DC,
        Type_Half_Float             = 0x10DD,
        Type_Float                  = 0x10DE
    };

    QCLImageFormat();
    QCLImageFormat(QCLImageFormat::ChannelOrder order,
                   QCLImageFormat::ChannelType type);
    QCLImageFormat(QImage::Format format);

    bool isNull() const;

    QCLImageFormat::ChannelOrder channelOrder() const;
    QCLImageFormat::ChannelType channelType() const;

    bool operator==(const QCLImageFormat &other);
    bool operator!=(const QCLImageFormat &other);

    QImage::Format toQImageFormat() const { return m_qformat; }

private:
    cl_image_format m_format;
    QImage::Format m_qformat;

    friend class QCLContext;
};

inline QCLImageFormat::QCLImageFormat()
{
    m_format.image_channel_order = 0;
    m_format.image_channel_data_type = 0;
    m_qformat = QImage::Format_Invalid;
}

inline bool QCLImageFormat::isNull() const
{
    return m_format.image_channel_order == 0 &&
           m_format.image_channel_data_type == 0 &&
           m_qformat == QImage::Format_Invalid;
}

inline QCLImageFormat::ChannelOrder QCLImageFormat::channelOrder() const
{
    return QCLImageFormat::ChannelOrder(m_format.image_channel_order);
}

inline QCLImageFormat::ChannelType QCLImageFormat::channelType() const
{
    return QCLImageFormat::ChannelType(m_format.image_channel_data_type);
}

inline bool QCLImageFormat::operator==(const QCLImageFormat &other)
{
    return m_format.image_channel_order ==
                other.m_format.image_channel_order &&
           m_format.image_channel_data_type ==
                other.m_format.image_channel_data_type &&
           m_qformat == other.m_qformat;
}

inline bool QCLImageFormat::operator!=(const QCLImageFormat &other)
{
    return m_format.image_channel_order !=
                other.m_format.image_channel_order ||
           m_format.image_channel_data_type !=
                other.m_format.image_channel_data_type ||
           m_qformat != other.m_qformat;
}

#ifndef QT_NO_DEBUG_STREAM
Q_CL_EXPORT QDebug operator<<(QDebug, const QCLImageFormat &);
#endif

QT_END_NAMESPACE

QT_END_HEADER

#endif
