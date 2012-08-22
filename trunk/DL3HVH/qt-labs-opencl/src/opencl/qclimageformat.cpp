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

#include "qclimageformat.h"
#include <QtCore/qdebug.h>

QT_BEGIN_NAMESPACE

/*!
    \class QCLImageFormat
    \brief The QCLImageFormat class represents the format of a QCLImage2D or QCLImage3D.
    \since 4.7
    \ingroup opencl

    QImage formats are mapped to OpenCL image formats as follows:

    \table
    \header \o QImage format \o Channel order \o Channel type
    \row \o QImage::Format_Indexed8
         \o Order_A \o Type_Normalized_UInt8
    \row \o QImage::Format_RGB32
         \o Order_BGRA or Order_ARGB \o Type_Normalized_UInt8
    \row \o QImage::Format_ARGB32
         \o Order_BGRA or Order_ARGB \o Type_Normalized_UInt8
    \row \o QImage::Format_ARGB32_Premultiplied
         \o Order_BGRA or Order_ARGB \o Type_Normalized_UInt8
    \row \o QImage::Format_RGB16
         \o Order_RGB \o Type_Normalized_565
    \row \o QImage::Format_RGB555
         \o Order_RGB \o Type_Normalized_555
    \row \o QImage::Format_RGB888
         \o Order_RGB \o Type_Normalized_UInt8
    \endtable

    Order_BGRA is used on little-endian systems for 32-bit
    pixel formats, and Order_ARGB on big-endian systems.
    QImage::Format_Indexed8 is mapped to an alpha-only image
    format to make it easier to convert font glyphs into
    OpenCL images.  OpenCL does not otherwise support
    indexed palettes.  All other formats result in a null
    QCLImageFormat object.

    It isn't possible to distinguish between QImage::Format_RGB32,
    QImage::Format_ARGB32, and QImage::Format_ARGB32_Premultiplied
    based on the OpenCL format parameters.  It is up the OpenCL
    kernels that operate on the image data to interpret the
    data appropriately.  If the QCLImageFormat was constructed
    from a QImage format, then the object will remember whether
    the format was QImage::Format_RGB32, QImage::Format_ARGB32,
    or QImage::Format_ARGB32_Premultiplied.
*/

/*!
    \enum QCLImageFormat::ChannelOrder
    This enum defines the order of R, G, B, and A channels in an image.

    \value Order_R Single red channel.
    \value Order_A Single alpha channel.
    \value Order_RG Red then green channel.
    \value Order_RA Red then alpha channel.
    \value Order_RGB Red, green, and blue channels.
    \value Order_RGBA Red, green, blue, then alpha channels.
    \value Order_BGRA Blue, green, red, then alpha channels.
    \value Order_ARGB Alpha, red, green, then blue channels.
    \value Order_Intensity Single intensity channel.
    \value Order_Luminence Single luminence channel.
    \value Order_Rx Red channel plus an ignored channel.
        This value is an optional channel order in OpenCL 1.1,
        and is not supported by OpenCL 1.0.
    \value Order_RGx Red and green channels plus an ignored channel.
        This value is an optional channel order in OpenCL 1.1,
        and is not supported by OpenCL 1.0.
    \value Order_RGBx Red, green, and blue channels, plus an
        ignored channel.  This value is an optional channel
        order in OpenCL 1.1, and is not supported by OpenCL 1.0.
*/

/*!
    \enum QCLImageFormat::ChannelType
    This enum defines the representation type for an image channel.

    \value Type_Normalized_Int8 Signed 8-bit integer, normalized to
           the range -1.0 to 1.0.
    \value Type_Normalized_Int16 Signed 8-bit integer, normalized to
           the range -1.0 to 1.0.
    \value Type_Normalized_UInt8 Unsigned 8-bit integer, normalized
           to the range 0.0 to 1.0.
    \value Type_Normalized_UInt16 Unsigned 16-bit integer, normalized
           to the range 0.0 to 1.0.
    \value Type_Normalized_565 RGB565 representation.
    \value Type_Normalized_555 RGB555 representation.
    \value Type_Normalized_101010 RGB-10-10-10 representation with
           10 bits of precision for each channel.
    \value Type_Unnormalized_Int8 Unnormalized signed 8-bit integer.
    \value Type_Unnormalized_Int16 Unnormalized signed 16-bit integer.
    \value Type_Unnormalized_Int32 Unnormalized signed 32-bit integer.
    \value Type_Unnormalized_UInt8 Unnormalized unsigned 8-bit integer.
    \value Type_Unnormalized_UInt16 Unnormalized unsigned 16-bit integer.
    \value Type_Unnormalized_UInt32 Unnormalized unsigned 32-bit integer.
    \value Type_Half_Float Half-precision floating-point value.
    \value Type_Float Single-precision floating-point value.
*/

/*!
    \fn QCLImageFormat::QCLImageFormat()

    Constructs a null OpenCL image format descriptor.

    \sa isNull()
*/

/*!
    Constructs an OpenCL image format descriptor from \a order and \a type.

    This constructor will infer the closest matching QImage format
    to return from toQImageFormat(), which may be QImage::Format_Invalid
    if \a order and \a type do not correspond to a QImage format.
*/
QCLImageFormat::QCLImageFormat
    (QCLImageFormat::ChannelOrder order, QCLImageFormat::ChannelType type)
{
    m_format.image_channel_order = order;
    m_format.image_channel_data_type = type;
    m_qformat = QImage::Format_Invalid;
    if (order == Order_RGB) {
        if (type == Type_Normalized_565)
            m_qformat = QImage::Format_RGB16;
        else if (type == Type_Normalized_555)
            m_qformat = QImage::Format_RGB555;
        else if (type == Type_Normalized_UInt8)
            m_qformat = QImage::Format_RGB888;
    } else if (order == Order_BGRA && type == Type_Normalized_UInt8) {
        if (QSysInfo::ByteOrder == QSysInfo::LittleEndian)
            m_qformat = QImage::Format_ARGB32;
    } else if (order == Order_ARGB && type == Type_Normalized_UInt8) {
        if (QSysInfo::ByteOrder != QSysInfo::LittleEndian)
            m_qformat = QImage::Format_ARGB32;
    } else if (order == Order_A && type == Type_Normalized_UInt8) {
        // We assume that Indexed8 images are alpha maps for font glyphs.
        m_qformat = QImage::Format_Indexed8;
    }
}


/*!
    Constructs an OpenCL image format descriptor that is equivalent to
    the specified QImage \a format.  If the \a format does not have
    an OpenCL equivalent, the descriptor will be set to null.

    \sa isNull(), toQImageFormat()
*/
QCLImageFormat::QCLImageFormat(QImage::Format format)
{
    m_qformat = format;

    switch (format) {
    case QImage::Format_Indexed8:
        // We assume that Indexed8 images are alpha maps for font glyphs.
        m_format.image_channel_order = Order_A;
        m_format.image_channel_data_type = Type_Normalized_UInt8;
        break;

    case QImage::Format_RGB32:
    case QImage::Format_ARGB32:
    case QImage::Format_ARGB32_Premultiplied:
        if (QSysInfo::ByteOrder == QSysInfo::LittleEndian) {
            m_format.image_channel_order = Order_BGRA;
            m_format.image_channel_data_type = Type_Normalized_UInt8;
        } else {
            m_format.image_channel_order = Order_ARGB;
            m_format.image_channel_data_type = Type_Normalized_UInt8;
        }
        break;

    case QImage::Format_RGB16:
        m_format.image_channel_order = Order_RGB;
        m_format.image_channel_data_type = Type_Normalized_565;
        break;

    case QImage::Format_RGB555:
        m_format.image_channel_order = Order_RGB;
        m_format.image_channel_data_type = Type_Normalized_555;
        break;

    case QImage::Format_RGB888:
        m_format.image_channel_order = Order_RGB;
        m_format.image_channel_data_type = Type_Normalized_UInt8;
        break;

    default:
        // Everything else is null.
        m_format.image_channel_order = 0;
        m_format.image_channel_data_type = 0;
        m_qformat = QImage::Format_Invalid;
        break;
    }
}

/*!
    \fn bool QCLImageFormat::isNull() const

    Returns true if this OpenCL image format descriptor is null.
*/

/*!
    \fn QCLImageFormat::ChannelOrder QCLImageFormat::channelOrder() const

    Returns the order of channels in this OpenCL image format.

    \sa channelType()
*/

/*!
    \fn QCLImageFormat::ChannelType QCLImageFormat::channelType() const

    Returns the representation type for channels in this OpenCL image format.

    \sa channelOrder()
*/

/*!
    \fn bool QCLImageFormat::operator==(const QCLImageFormat &other)

    Returns true if this image format is the same as \a other;
    false otherwise.

    \sa operator!=()
*/

/*!
    \fn bool QCLImageFormat::operator!=(const QCLImageFormat &other)

    Returns true if this image format is not the same as \a other;
    false otherwise.

    \sa operator==()
*/

/*!
    \fn QImage::Format QCLImageFormat::toQImageFormat() const

    Returns the nearest QImage format to this OpenCL image format;
    QImage::Format_Invalid if there is no corresponding QImage format.
*/

#ifndef QT_NO_DEBUG_STREAM

QDebug operator<<(QDebug dbg, const QCLImageFormat &format)
{
    if (format.isNull()) {
        dbg << "QCLImageFormat()";
        return dbg;
    }
    dbg.nospace() << "QCLImageFormat(";
    switch (int(format.channelOrder())) {
    case QCLImageFormat::Order_R: dbg << "R,"; break;
    case QCLImageFormat::Order_A: dbg << "A,"; break;
    case QCLImageFormat::Order_RG: dbg << "RG,"; break;
    case QCLImageFormat::Order_RA: dbg << "RA,"; break;
    case QCLImageFormat::Order_RGB: dbg << "RGB,"; break;
    case QCLImageFormat::Order_RGBA: dbg << "RGBA,"; break;
    case QCLImageFormat::Order_BGRA: dbg << "BGRA,"; break;
    case QCLImageFormat::Order_ARGB: dbg << "ARGB,"; break;
    case QCLImageFormat::Order_Intensity: dbg << "Intensity,"; break;
    case QCLImageFormat::Order_Luminence: dbg << "Luminence,"; break;
    case QCLImageFormat::Order_Rx: dbg << "Rx,"; break;
    case QCLImageFormat::Order_RGx: dbg << "RGx,"; break;
    case QCLImageFormat::Order_RGBx: dbg << "RGBx,"; break;
    default: dbg << int(format.channelOrder()) << ','; break;
    }
    switch (int(format.channelType())) {
    case QCLImageFormat::Type_Normalized_Int8:
        dbg << "int8"; break;
    case QCLImageFormat::Type_Normalized_Int16:
        dbg << "int16"; break;
    case QCLImageFormat::Type_Normalized_UInt8:
        dbg << "uint8"; break;
    case QCLImageFormat::Type_Normalized_UInt16:
        dbg << "uint16"; break;
    case QCLImageFormat::Type_Normalized_565:
        dbg << "565"; break;
    case QCLImageFormat::Type_Normalized_555:
        dbg << "555"; break;
    case QCLImageFormat::Type_Normalized_101010:
        dbg << "101010"; break;
    case QCLImageFormat::Type_Unnormalized_Int8:
        dbg << "U_int8"; break;
    case QCLImageFormat::Type_Unnormalized_Int16:
        dbg << "U_int16"; break;
    case QCLImageFormat::Type_Unnormalized_Int32:
        dbg << "U_int32"; break;
    case QCLImageFormat::Type_Unnormalized_UInt8:
        dbg << "U_uint8"; break;
    case QCLImageFormat::Type_Unnormalized_UInt16:
        dbg << "U_uint16"; break;
    case QCLImageFormat::Type_Unnormalized_UInt32:
        dbg << "U_uint32"; break;
    case QCLImageFormat::Type_Half_Float:
        dbg << "half"; break;
    case QCLImageFormat::Type_Float:
        dbg << "float"; break;
    default: dbg << int(format.channelType()); break;
    }
    QImage::Format qformat = format.toQImageFormat();
    if (qformat != QImage::Format_Invalid)
        dbg << ",QImage=" << int(qformat);
    dbg << ')';
    return dbg.space();
}

#endif

QT_END_NAMESPACE
