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

#ifndef QGLCAMERA_H
#define QGLCAMERA_H

#include <QtCore/qobject.h>
#include <QtCore/qsize.h>
#include <QtGui/qvector3d.h>
#include <QtGui/qmatrix4x4.h>
#include <QtGui/qquaternion.h>

QT_BEGIN_HEADER

class QGLCameraPrivate;

class QGLCamera : public QObject
{
    Q_OBJECT
    Q_ENUMS(ProjectionType)
    Q_PROPERTY(ProjectionType projectionType READ projectionType WRITE setProjectionType NOTIFY projectionChanged)
    Q_PROPERTY(qreal fieldOfView READ fieldOfView WRITE setFieldOfView NOTIFY projectionChanged)
    Q_PROPERTY(qreal nearPlane READ nearPlane WRITE setNearPlane NOTIFY projectionChanged)
    Q_PROPERTY(qreal farPlane READ farPlane WRITE setFarPlane NOTIFY projectionChanged)
    Q_PROPERTY(QSizeF viewSize READ viewSize WRITE setViewSize NOTIFY projectionChanged)
    Q_PROPERTY(QSizeF minViewSize READ minViewSize WRITE setMinViewSize NOTIFY projectionChanged)
    Q_PROPERTY(int screenRotation READ screenRotation WRITE setScreenRotation NOTIFY projectionChanged)
    Q_PROPERTY(qreal xEye READ xEye WRITE setXEye NOTIFY viewChanged)
    Q_PROPERTY(qreal yEye READ yEye WRITE setYEye NOTIFY viewChanged)
    Q_PROPERTY(qreal zEye READ zEye WRITE setZEye NOTIFY viewChanged)
    Q_PROPERTY(QVector3D eye READ eye WRITE setEye NOTIFY viewChanged)
    Q_PROPERTY(QVector3D upVector READ upVector WRITE setUpVector NOTIFY viewChanged)
    Q_PROPERTY(qreal xCentre READ xCentre WRITE setXCentre NOTIFY viewChanged)
    Q_PROPERTY(qreal yCentre READ yCentre WRITE setYCentre NOTIFY viewChanged)
    Q_PROPERTY(qreal zCentre READ zCentre WRITE setZCentre NOTIFY viewChanged)
    Q_PROPERTY(QVector3D center READ center WRITE setCenter NOTIFY viewChanged)
    Q_PROPERTY(qreal eyeSeparation READ eyeSeparation WRITE setEyeSeparation NOTIFY viewChanged)
    Q_PROPERTY(QVector3D motionAdjustment READ motionAdjustment WRITE setMotionAdjustment DESIGNABLE false NOTIFY viewChanged)
public:
    explicit QGLCamera(QObject *parent = 0);
    ~QGLCamera();

    enum ProjectionType
    {
        Perspective,
        Orthographic
    };

    QGLCamera::ProjectionType projectionType() const;
    void setProjectionType(QGLCamera::ProjectionType value);

    qreal fieldOfView() const;
    void setFieldOfView(qreal angle);

    qreal nearPlane() const;
    void setNearPlane(qreal value);

    qreal farPlane() const;
    void setFarPlane(qreal value);

    QSizeF viewSize() const;
    void setViewSize(const QSizeF& size);

    QSizeF minViewSize() const;
    void setMinViewSize(const QSizeF& size);

    int screenRotation() const;
    void setScreenRotation(int angle);

    qreal xEye() const;
    void setXEye(qreal value);
    qreal yEye() const;
    void setYEye(qreal value);
    qreal zEye() const;
    void setZEye(qreal value);

    QVector3D eye() const;
    void setEye(const QVector3D& vertex);

    QVector3D upVector() const;
    void setUpVector(const QVector3D& vector);

    qreal xCentre() const;
    void setXCentre(qreal value);
    qreal yCentre() const;
    void setYCentre(qreal value);
    qreal zCentre() const;
    void setZCentre(qreal value);

    QVector3D center() const;
    void setCenter(const QVector3D& vertex);

    qreal eyeSeparation() const;
    void setEyeSeparation(qreal value);

    QVector3D motionAdjustment() const;
    void setMotionAdjustment(const QVector3D& vector);

    QQuaternion tilt(qreal angle) const;
    QQuaternion pan(qreal angle) const;
    QQuaternion roll(qreal angle) const;

    void rotateEye(const QQuaternion& q);
    void rotateCenter(const QQuaternion& q);

    QVector3D translation(qreal x, qreal y, qreal z) const;

    void translateEye(const QVector3D& vector);
    void translateCenter(const QVector3D& vector);

    QMatrix4x4 projectionMatrix(qreal aspectRatio) const;
    QMatrix4x4 modelViewMatrix() const;

Q_SIGNALS:
    void projectionChanged();
    void viewChanged();

private:
    QGLCameraPrivate *d_ptr;

    QGLCameraPrivate *d_func() const { return d_ptr; }

    Q_DISABLE_COPY(QGLCamera)
};

QT_END_HEADER

#endif
