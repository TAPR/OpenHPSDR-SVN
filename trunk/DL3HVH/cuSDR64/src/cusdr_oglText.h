/**
* @file  cusdr_oglText.h
* @brief OpenGL Text generation header file for cuSDR 
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-02-18
*/

/*
 *	 adapted from the MIFit project: http://code.google.com/p/mifit
 *
 *   Copyright 2012 adapted for cuSDR by Hermann von Hasseln, DL3HVH
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

#ifndef _CUSDR_OGL_TEXT_H
#define _CUSDR_OGL_TEXT_H

#include "cusdr_oglUtils.h"
#include "cusdr_oglInfo.h"
#include "cusdr_settings.h"

#include <QtGlobal>

class QChar;
class QFont;
class QFontMetrics;
class QString;

class OGLTextPrivate;

class OGLText {

public:
    OGLText(const QFont &f);
    virtual ~OGLText();

    QFont font() const;
    QFontMetrics fontMetrics() const;

    void renderText(float x, float y, const QString &text);
	void renderText(float x, float y, float z, const QString &text);

private:
    Q_DISABLE_COPY(OGLText)

    OGLTextPrivate *const d;
};

#endif // _CUSDR_OGL_TEXT_H