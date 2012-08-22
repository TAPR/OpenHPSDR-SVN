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

#include "qclsampler.h"
#include "qclcontext.h"

QT_BEGIN_NAMESPACE

/*!
    \class QCLSampler
    \brief The QCLSampler class represents an OpenCL sampler object.
    \since 4.7
    \ingroup opencl

    Sampler objects define how to derive pixel values when an
    image is read by an OpenCL kernel.

    The filterMode() defines how the pixel color should be derived.
    For example, reading the value at co-ordinates (3.2, 6.7) will
    read the nearest pixel value at (3, 7) if the filterMode()
    is \l{QCLSampler::Nearest}{Nearest}, or will interpolate an
    intermediate color if the filterMode() is \l{QCLSampler::Linear}{Linear}.

    The addressingMode() defines how to handle out of bounds
    pixel accesses, by clamping to the range, repeating the image's
    pattern, etc.

    Samplers are created using QCLContext::createSampler(), as follows:

    \code
    QCLSampler sampler = context.createSampler
        (false, QCLSampler::ClampToEdge, QCLSampler::Linear);
    \endcode

    Samplers can also be defined as literals in the OpenCL kernel
    source code, which avoids the need to create an explicit
    QCLSampler value:

    \code
    const sampler_t samp = CLK_ADDRESS_CLAMP_TO_EDGE |
                           CLK_FILTER_LINEAR;
    \endcode

    The main advantage of QCLSampler over literal sampler values
    is that QCLSampler allows the pixel derivation strategy to be
    modified at runtime.
*/

/*!
    \fn QCLSampler::QCLSampler()

    Constructs a null OpenCL sampler object.
*/

/*!
    \fn QCLSampler::QCLSampler(QCLContext *context, cl_sampler id)

    Constructs an OpenCL sampler object from the native identifier \a id.
    This class takes over ownership of \a id and will release it in
    the destructor.  The sampler \a id will be associated with \a context.
*/

/*!
    \fn QCLSampler::QCLSampler(const QCLSampler &other)

    Constructs a copy of \a other.  The \c{clRetainSampler()} function
    will be called to update the reference count on samplerId().
*/

/*!
    \fn QCLSampler::~QCLSampler()

    Releases this OpenCL sampler object by calling \c{clReleaseSampler()}.
*/

/*!
    \fn QCLSampler &QCLSampler::operator=(const QCLSampler &other)

    Assigns \a other to this OpenCL sampler object.  The current samplerId()
    will be released with \c{clReleaseSampler()}, and the new samplerId()
    will be retained with \c{clRetainSampler()}.
*/

/*!
    \enum QCLSampler::AddressingMode
    This enum specifies how to handle out-of-range image co-ordinates
    when reading from an image in OpenCL.

    \value None No special handling of out-of-range co-ordinates.
    \value ClampToEdge Out-of-range requests clamp to the edge pixel value.
    \value Clamp Out-of-range requests clamp to the image extents.
    \value Repeat Repeats the image in a cycle.
*/

/*!
    \enum QCLSampler::FilterMode
    This enum defines the type of filter to apply when reading from
    an image in OpenCL.

    \value Nearest Use the color of the nearest pixel.
    \value Linear Interpolate linearly between pixel colors to generate
    intermediate pixel colors.
*/

/*!
    \fn bool QCLSampler::isNull() const

    Returns true if this OpenCL sampler object is null; false otherwise.
*/

/*!
    Returns true if this sampler is using normalized co-ordinates;
    false otherwise.

    \sa addressingMode(), filterMode()
*/
bool QCLSampler::normalizedCoordinates() const
{
    if (m_id) {
        cl_bool normalized = CL_FALSE;
        clGetSamplerInfo(m_id, CL_SAMPLER_NORMALIZED_COORDS,
                         sizeof(normalized), &normalized, 0);
        return normalized != CL_FALSE;
    } else {
        return false;
    }
}

/*!
    Returns the addressing mode for out-of-range co-ordinates
    when reading from an image in OpenCL.

    \sa normalizedCoordinates(), filterMode()
*/
QCLSampler::AddressingMode QCLSampler::addressingMode() const
{
    if (m_id) {
        cl_addressing_mode addressing = CL_ADDRESS_CLAMP_TO_EDGE;
        clGetSamplerInfo(m_id, CL_SAMPLER_ADDRESSING_MODE,
                         sizeof(addressing), &addressing, 0);
        return QCLSampler::AddressingMode(addressing);
    } else {
        return ClampToEdge;
    }
}

/*!
    Returns the type of filter to apply when reading from an image
    in OpenCL.

    \sa normalizedCoordinates(), addressingMode()
*/
QCLSampler::FilterMode QCLSampler::filterMode() const
{
    if (m_id) {
        cl_filter_mode filter = CL_FILTER_LINEAR;
        clGetSamplerInfo(m_id, CL_SAMPLER_FILTER_MODE,
                         sizeof(filter), &filter, 0);
        return QCLSampler::FilterMode(filter);
    } else {
        return Linear;
    }
}

/*!
    \fn cl_sampler QCLSampler::samplerId() const

    Returns the native OpenCL identifier for this sampler; or 0 if
    the sampler is null.
*/

/*!
    \fn QCLContext *QCLSampler::context() const

    Returns the OpenCL context that this sampler was created for;
    null if not yet created within a context.
*/

/*!
    \fn bool QCLSampler::operator==(const QCLSampler &other) const

    Returns true if this OpenCL sampler object is the same as \a other;
    false otherwise.

    \sa operator!=()
*/

/*!
    \fn bool QCLSampler::operator!=(const QCLSampler &other) const

    Returns true if this OpenCL sampler object is not the same as \a other;
    false otherwise.

    \sa operator==()
*/

QT_END_NAMESPACE
