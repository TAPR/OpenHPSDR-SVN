/**
* @file  cusdr_oglInfo.h
* @brief OpenGL info header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-11-10
*/

/*
 *	 adapted from glInfo.h of Song Ho Ahn (song.ahn@gmail.com)
 *   Copyright (c) 2005 Song Ho Ahn
 *
 *	 (C) 2011 adapted for cuSDR by Hermann von Hasseln, DL3HVH
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Library General Public License version 2 as
 *   published by the Free Software Foundation
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details
 *
 *   You should have received a copy of the GNU Library General Public
 *   License along with this program; if not, write to the
 *   Free Software Foundation, Inc.,
 *   51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

#ifndef _CUSDR_OGLINFO_H
#define _CUSDR_OGLINFO_H

//#include <QObject>
//#include <QString>
//#include <QStringList>

#include "cusdr_settings.h"

// struct variable to store OpenGL info
typedef struct _glinfo {

	QString vendor;
    QString renderer;
    QString version;
    QStringList extensions;

    int redBits;
    int greenBits;
    int blueBits;
    int alphaBits;
    int depthBits;
    int stencilBits;
    int maxTextureSize;
    int maxLights;
    int maxAttribStacks;
    int maxModelViewStacks;
    int maxProjectionStacks;
    int maxClipPlanes;
    int maxTextureStacks;

} t_glinfo;


class QOpenGLInfo : public QObject {

	Q_OBJECT

public:
	QOpenGLInfo(QObject *parent = 0);
	~QOpenGLInfo();

	bool getInfo();
    void printSelf();
    bool isExtensionSupported(const QString &extension);

private:
	Settings*	set;

	t_glinfo	m_glInfo;
};

#endif // _CUSDR_OGLINFO_H
