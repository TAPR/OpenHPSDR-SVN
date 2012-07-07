/**
* @file  cusdr_oglInfo.cpp
* @brief OpenGL info class for cuSDR
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

//#include <GL/gl.h>
#include "cusdr_oglUtils.h"
#include "cusdr_oglInfo.h"

QOpenGLInfo::QOpenGLInfo(QObject *parent)
	: QObject(parent)
	, m_settings(Settings::instance())
{
	m_glInfo.redBits = 0;
	m_glInfo.greenBits = 0;
	m_glInfo.blueBits = 0;
	m_glInfo.alphaBits = 0;
	m_glInfo.depthBits = 0;
	m_glInfo.stencilBits = 0;
	m_glInfo.maxTextureSize = 0;
	m_glInfo.maxLights = 0;
	m_glInfo.maxAttribStacks = 0;
	m_glInfo.maxModelViewStacks = 0;
	m_glInfo.maxClipPlanes = 0;
	m_glInfo.maxTextureStacks = 0;
}

QOpenGLInfo::~QOpenGLInfo() {
}

bool QOpenGLInfo::getInfo() {

    QString str;

    // get vendor string
	str = QLatin1String(reinterpret_cast<const char *>(glGetString(GL_EXTENSIONS)));
    if(!str.isNull())
		m_glInfo.vendor = str;
    else 
		return false;

    // get renderer string
	str = QLatin1String(reinterpret_cast<const char *>(glGetString(GL_RENDERER)));
    //str = (char*)glGetString(GL_RENDERER);
    if(!str.isNull())
		m_glInfo.renderer = str;
    else 
		return false;

    // get version string
	str = QLatin1String(reinterpret_cast<const char *>(glGetString(GL_VERSION)));
    str = (char*)glGetString(GL_VERSION);
	if(!str.isNull())
		m_glInfo.version = str;
    else 
		return false;
    
    // get all extensions as a string
	str = QLatin1String(reinterpret_cast<const char *>(glGetString(GL_EXTENSIONS)));
    //str = (char*)glGetString(GL_EXTENSIONS);

    // split extensions
	if(!str.isNull()) {

		m_glInfo.extensions = str.split(' ');
		m_glInfo.extensions.sort();
	}
	else 
		return false;

    // get number of color bits
    glGetIntegerv(GL_RED_BITS, &m_glInfo.redBits);
    glGetIntegerv(GL_GREEN_BITS, &m_glInfo.greenBits);
    glGetIntegerv(GL_BLUE_BITS, &m_glInfo.blueBits);
    glGetIntegerv(GL_ALPHA_BITS, &m_glInfo.alphaBits);

    // get depth bits
    glGetIntegerv(GL_DEPTH_BITS, &m_glInfo.depthBits);

    // get stecil bits
    glGetIntegerv(GL_STENCIL_BITS, &m_glInfo.stencilBits);

    // get max number of lights allowed
    glGetIntegerv(GL_MAX_LIGHTS, &m_glInfo.maxLights);

    // get max texture resolution
    glGetIntegerv(GL_MAX_TEXTURE_SIZE, &m_glInfo.maxTextureSize);

    // get max number of clipping planes
    glGetIntegerv(GL_MAX_CLIP_PLANES, &m_glInfo.maxClipPlanes);

    // get max modelview and projection matrix stacks
    glGetIntegerv(GL_MAX_MODELVIEW_STACK_DEPTH, &m_glInfo.maxModelViewStacks);
    glGetIntegerv(GL_MAX_PROJECTION_STACK_DEPTH, &m_glInfo.maxProjectionStacks);
    glGetIntegerv(GL_MAX_ATTRIB_STACK_DEPTH, &m_glInfo.maxAttribStacks);

    // get max texture stacks
    glGetIntegerv(GL_MAX_TEXTURE_STACK_DEPTH, &m_glInfo.maxTextureStacks);

    return true;
}

bool QOpenGLInfo::isExtensionSupported(const QString &extension) {

    for (int i = 0; i < m_glInfo.extensions.size(); ++i) {

		if (extension == m_glInfo.extensions.at(i))
			return true;
	}
    return false;
}

void QOpenGLInfo::printSelf() {

    //std::stringstream ss;

    qDebug() << "";
    qDebug() << "OpenGL Driver Info";
    qDebug() << "==================";
    qDebug() << "Vendor: " << m_glInfo.vendor;
    qDebug() << "Version: " << m_glInfo.version;
    qDebug() << "Renderer: " << m_glInfo.renderer;

    qDebug() << "";
    qDebug() << "Color Bits(R,G,B,A): (" 
			 << m_glInfo.redBits 
			 << ", " << m_glInfo.greenBits
			 << ", " << m_glInfo.blueBits 
			 << ", " << m_glInfo.alphaBits
			 << ")\n";

    qDebug() << "Depth Bits: " << m_glInfo.depthBits;
    qDebug() << "Stencil Bits: " << m_glInfo.stencilBits;

    qDebug() << "";
    qDebug() << "Max Texture Size: " 
			 << m_glInfo.maxTextureSize 
			 << "x" 
			 << m_glInfo.maxTextureSize;

    qDebug() << "Max Lights: " << m_glInfo.maxLights;
    qDebug() << "Max Clip Planes: " << m_glInfo.maxClipPlanes;
    qDebug() << "Max Modelview Matrix Stacks: " << m_glInfo.maxModelViewStacks;
    qDebug() << "Max Projection Matrix Stacks: " << m_glInfo.maxProjectionStacks;
    qDebug() << "Max Attribute Stacks: " << m_glInfo.maxAttribStacks;
    qDebug() << "Max Texture Stacks: " << m_glInfo.maxTextureStacks;

    qDebug() << "";
    qDebug() << "Total Number of Extensions: " << m_glInfo.extensions.size();
    qDebug() << "==============================";
    
	for(int i = 0; i < m_glInfo.extensions.size(); ++i)
        qDebug() << m_glInfo.extensions.at(i);

    qDebug() << "======================================================================";
}
