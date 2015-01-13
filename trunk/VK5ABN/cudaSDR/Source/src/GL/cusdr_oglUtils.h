/**
* @file  cusdr_oglUtils.h
* @brief Utils header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-11-17
*/

/*
 *   Copyright 2011 Hermann von Hasseln, DL3HVH
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

#ifndef _CUSDR_OPENGLTYPES_H
#define _CUSDR_OPENGLTYPES_H

#include <QGLWidget>
//#include <QList>
//#include <QRect>
//#include <QColor>
//#include <QVarLengthArray>
#include <QGLFramebufferObject>
#include <QtCore/qmath.h>

#define GL_CLAMP_TO_EDGE	0x812F


typedef struct _gl2i {

	GLint x;
	GLint y;

} TGL2int;

typedef struct _gl3i {

	GLint x;
	GLint y;
	GLint z;

} TGL3int;

typedef struct _gl2f {

	GLfloat x;
	GLfloat y;

} TGL2float;

typedef struct _gl3f {

	GLfloat x;
	GLfloat y;
	GLfloat z;

} TGL3float;

typedef struct _ucharRGBA {

	uchar	red;
	uchar	green;
	uchar	blue;
	uchar	alpha;

} ucharRGBA;

typedef struct _glubyteRGBA {

	GLubyte	red;
	GLubyte	green;
	GLubyte	blue;
	GLubyte	alpha;

} TGL_ubyteRGBA;

typedef struct _scaleSteps {

	double smallStep;
	double bigStep;

} TScaleSteps;

typedef struct _scale {

	QList<int> mainPointPositions;
	QList<int> subPointPositions;

	QList<qreal> mainPoints;
	QList<qreal> subPoints;

} TScale;


struct s_glRGBA_float {

	GLfloat r, g, b, a;

	s_glRGBA_float() : r(0), g(0), b(0), a(0) {}

	s_glRGBA_float(GLfloat red, GLfloat grn, GLfloat blu, GLfloat alpha) : r(red), g(grn), b(blu), a(alpha) {}
};

struct s_glRGBA_uByte {

	GLubyte r, g, b, a;

	s_glRGBA_uByte() : r(0), g(0), b(0), a(0) {}

	s_glRGBA_uByte(GLubyte red, GLubyte grn, GLubyte blu, GLubyte alpha) : r(red), g(grn), b(blu), a(alpha) {}
};

typedef struct _widebandDisplayData {

	QSize		size;

	QRect		widebandPanRect;
	QRect		freqScaleWidebandPanRect;
	QRect		dBmScaleWidebandPanRect;

	QVector<qreal>	widebandPanBins;
	
	qreal		dBmPanMin;
	qreal		dBmPanMax;
	qreal		scaleMult;
	qreal		freqScaleZoomFactor;

	long		frequency;

	bool		freqScaleWidebandUpdate;
	bool		freqScaleWidebandRenew;
	bool		dBmScaleWidebandUpdate;
	bool		dBmScaleWidebandRenew;
	bool		widebandPanGridUpdate;
	bool		widebandPanGridRenew;

} TWideBandDisplayData;
 
//**************************************************************
inline QString frequencyString(double frequency, bool addPlusSign = false) {

	QString str("");

	double f = qAbs(frequency);

	if (f >= 1e9) {

		str = QString::number(f / 1e9, 'f', 6);
		str.insert(str.size() - 3, '.');
		str += " GHz";
	}
	else
	if (f >= 1e6) {

		str = QString::number(f / 1e6, 'f', 6 + 1);
		str.insert(str.size() - 4, '.');
		str.insert(str.size() - 1, '.');
		str += " MHz";
	}
	else
	if (f >= 1e3) {

		str = QString::number(f / 1e3, 'f', 3 + 1);
		str.insert(str.size() - 1, '.');
		str += " kHz";
	}
	else {

		str = QString::number(f, 'f', 1) + "Hz";
	}

	if (frequency < 0) str = '-' + str;
	else
	if (frequency > 0 && addPlusSign) str = '+' + str;

	return str;
}

inline int nextPowerOfTwo(int value) {

	value--;
	value |= value >> 1;
	value |= value >> 2;
	value |= value >> 4;
	value |= value >> 8;
	value |= value >> 16;
	++value;
	return value;
}

inline TScaleSteps getXScale(double size) {

	TScaleSteps s;

	qint64 base = 1;
	int mult = 1;
	while (size > 10.0f) {
        size /= 10;
		base *= 10;
    }

	if (size < 2)		mult = 2;
	else if (size < 5)	mult = 5;
	else				mult = 10;

	s.bigStep = base * mult;
	switch (mult) {

		case 1:	 s.smallStep = s.bigStep / 5; break;
		case 2:	 s.smallStep = s.bigStep / 2; break;
		case 5:	 s.smallStep = s.bigStep / 5; break;
		case 10: s.smallStep = s.bigStep / 5; break;
	}
	return s;
}

inline TScaleSteps getXScale(double size, float scale) {

	TScaleSteps s;

	qint64 base = 1;
	int mult = 1;
	while (size > scale) {
        size /= scale;
		base *= scale;
    }

	if (size < 2)		mult = 2;
	else if (size < 5)	mult = 5;
	else				mult = 10;

	s.bigStep = base * mult;
	switch (mult) {

		case 1:	 s.smallStep = s.bigStep / 5; break;
		case 2:	 s.smallStep = s.bigStep / 2; break;
		case 5:	 s.smallStep = s.bigStep / 5; break;
		case 10: s.smallStep = s.bigStep / 5; break;
	}
	return s;
}

inline TScaleSteps getYScale(double size, float scale) {

	TScaleSteps s;

	qint64 base = 1;
	int mult = 1;
	while (size > scale) {
        size /= scale;
		base *= scale;
    }

	if (size < 2)		mult = 2;
	else if (size < 5)	mult = 5;
	else				mult = 10;

	s.bigStep = base * mult;
	switch (mult) {

		case 1:	 s.smallStep = s.bigStep / 5; break;
		case 2:	 s.smallStep = s.bigStep / 2; break;
		case 5:	 s.smallStep = s.bigStep / 5; break;
		case 10: s.smallStep = s.bigStep / 2; break;
	}
	return s;
}

inline TScale getXRuler(const QRect &rect, int fontMaxWidth, qreal unit, qreal lo, qreal hi) {

	TScale ruler;
	
	TScaleSteps scale = getXScale(fontMaxWidth / unit);
	//qreal value = floor(lo / scale.bigStep) * scale.bigStep;
	qreal value = qFloor(lo / scale.bigStep) * scale.bigStep;

	while (value < hi) {

		int x = qRound(unit * (value - lo));

		if (x >= rect.width()) break;
		if (x > 0) {

			ruler.mainPoints << value;
			ruler.mainPointPositions << x;
		}

		if (scale.smallStep > 0) {

			qreal smallValue = value + scale.smallStep;
			qreal smallUpperValue = value + scale.bigStep;

			while (smallValue < smallUpperValue && smallValue < hi) {

				int x = qRound(unit * (smallValue - lo));
				if (x >= rect.width()) break;
				if (x > 0) ruler.subPointPositions << x;

				smallValue += scale.smallStep;
			}
		}
		value += scale.bigStep;
	}

	return ruler;
}

inline TScale getXRuler(const QRect &rect, int fontMaxWidth, qreal unit, qreal lo, qreal hi, float s) {

	TScale ruler;
	
	TScaleSteps scale = getXScale(fontMaxWidth / unit, s);
	//qreal value = floor(lo / scale.bigStep) * scale.bigStep;
	qreal value = qFloor(lo / scale.bigStep) * scale.bigStep;

	while (value < hi) {

		int x = qRound(unit * (value - lo));

		if (x >= rect.width()) break;
		if (x > 0) {

			ruler.mainPoints << value;
			ruler.mainPointPositions << x;
		}

		if (scale.smallStep > 0) {

			qreal smallValue = value + scale.smallStep;
			qreal smallUpperValue = value + scale.bigStep;

			while (smallValue < smallUpperValue && smallValue < hi) {

				int x = qRound(unit * (smallValue - lo));
				if (x >= rect.width()) break;
				if (x > 0) ruler.subPointPositions << x;

				smallValue += scale.smallStep;
			}
		}
		value += scale.bigStep;
	}

	return ruler;
}

inline TScale getYRuler(const QRect &rect, int fontHeight, qreal unit, qreal lo, qreal hi) {

	TScale ruler;
	
	TScaleSteps scale = getYScale(fontHeight / unit, 10.0f);
	//qreal value = ceil(hi / scale.bigStep) * scale.bigStep;
	qreal value = qCeil(hi / scale.bigStep) * scale.bigStep;

	while (value >= lo) {

		int y = qRound(unit * -(value - hi));
		if (y > 0 && y < rect.height()) {

			if (ruler.mainPointPositions.length() < 100) {

				ruler.mainPoints << value;
				ruler.mainPointPositions << rect.top() + y;
			}
		}

		if (scale.smallStep > 0) {

			qreal smallValue = value - scale.smallStep;
			qreal smallEndValue = value - scale.bigStep;
			while (smallValue > smallEndValue && smallValue > lo) {

				int y = qRound(unit * -(smallValue - hi));
				if (y > 0 && y < rect.height())	{

					if (ruler.subPointPositions.length() < 200)
						ruler.subPointPositions << rect.top() + y;
				}
				smallValue -= scale.smallStep;
			}
		}
		value -= scale.bigStep;
	}

	return ruler;
}

inline TScale getYRuler2(const QRect &rect, int fontHeight, qreal unit, qreal lo, qreal hi) {

	TScale ruler;
	
	TScaleSteps scale = getYScale(fontHeight / unit, 10.0f);
	qreal value = qCeil(hi / scale.bigStep) * scale.bigStep;

	while (value >= lo) {

		int y = qRound(unit * -(value - hi));
		if (y > 0 && y < rect.height()) {

			if (ruler.mainPointPositions.length() < 100) {

				ruler.mainPoints << value;
				//ruler.mainPointPositions << rect.top() + y;
				ruler.mainPointPositions << y;
			}
		}

		if (scale.smallStep > 0) {

			qreal smallValue = value - scale.smallStep;
			qreal smallEndValue = value - scale.bigStep;
			while (smallValue > smallEndValue && smallValue > lo) {

				int y = qRound(unit * -(smallValue - hi));
				if (y > 0 && y < rect.height())	{

					if (ruler.subPointPositions.length() < 200)
						ruler.subPointPositions << y;
						//ruler.subPointPositions << rect.top() + y;
				}
				smallValue -= scale.smallStep;
			}
		}
		value -= scale.bigStep;
	}

	return ruler;
}

inline TScale getYRuler3(const QRect &rect, int fontHeight, qreal unit, qreal lo, qreal hi, float v) {

	TScale ruler;

	TScaleSteps scale = getYScale(fontHeight / unit, v);
	qreal value = qCeil(hi / scale.bigStep) * scale.bigStep;

	while (value >= lo) {

		int y = qRound(unit * -(value - hi));
		if (y > 0 && y < rect.height()) {

			if (ruler.mainPointPositions.length() < 100) {

				ruler.mainPoints << value;
				//ruler.mainPointPositions << rect.top() + y;
				ruler.mainPointPositions << y;
			}
		}

		if (scale.smallStep > 0) {

			qreal smallValue = value - scale.smallStep;
			qreal smallEndValue = value - scale.bigStep;
			while (smallValue > smallEndValue && smallValue > lo) {

				int y = qRound(unit * -(smallValue - hi));
				if (y > 0 && y < rect.height())	{

					if (ruler.subPointPositions.length() < 200)
						ruler.subPointPositions << y;
						//ruler.subPointPositions << rect.top() + y;
				}
				smallValue -= scale.smallStep;
			}
		}
		value -= scale.bigStep;
	}

	return ruler;
}

inline GLfloat dBmToGLPixel(const QRect &rect, qreal dBmMax, qreal dBmMin, qreal value) {

	GLfloat y;

	qreal yScale = rect.height() / qAbs(dBmMax - dBmMin);
	y = (GLfloat)(yScale * (dBmMax - value) + (qreal)rect.top());

	return y;
}

inline qreal glPixelTodBm(const QRect &rect, qreal dBmMax, qreal dBmMin, int position) {

	qreal dBm;

	float yScale = rect.height() / qAbs(dBmMax - dBmMin);
	dBm = dBmMax - (qreal)(position - rect.top())/yScale;
	//qreal dBm = m_dBmPanMax - ((m_dBmPanMax - m_dBmPanMin) * ((qreal)(position - rect.top()) / rect.height()));

	return dBm;
}

//**************************************************************

inline void setProjectionOrthographic(int width, int height) {

    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    
	//glOrtho(-1.0, +1.0, -1.0, +1.0, -90.0, +90.0);
	glOrtho(0.0, width, height, 0, -5.0, 5.0);
	//glOrtho(0.0, width, height, 0, -1.0, 1.0);
	//glOrtho(0.0, width, 0, height, -1.0, 1.0);

    glMatrixMode(GL_MODELVIEW);
}

inline void setProjectionPerspective(int width, int height) {

	Q_UNUSED(width)
	Q_UNUSED(height)

	glMatrixMode(GL_PROJECTION);
    glLoadIdentity();

    //glFrustum(-aspect, +aspect, -1.0, +1.0, 4.0, 15.0);
	//gluPerspective(60.0f, (float)(width)/height, 1.0f, 10.0f);

    glMatrixMode(GL_MODELVIEW);
}

inline GLuint emptyTexture1D(int width) {

	GLuint txtnumber;
	unsigned int* data;
	
	data = (unsigned int*)new GLuint[(width * 4 * sizeof(unsigned int))];

#if defined(Q_OS_WIN32)
	ZeroMemory(data,(width * 4 * sizeof(unsigned int)));
#elif defined(Q_OS_LINUX)
	memset(data, 0, width * 4 * sizeof(unsigned int));
#endif

	glGenTextures(1, &txtnumber);
	glBindTexture(GL_TEXTURE_1D, txtnumber);
	glTexImage1D(GL_TEXTURE_1D, 0, 4, width, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
	glTexParameteri(GL_TEXTURE_1D, GL_TEXTURE_MIN_FILTER, GL_LINEAR); 
	glTexParameteri(GL_TEXTURE_1D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
	
	delete[] data;
	return txtnumber;
}

inline GLuint emptyTexture2D(int width, int height) {

	GLuint txtnumber;
	unsigned int* data;
	
	data = (unsigned int*)new GLuint[((width * height)* 4 * sizeof(unsigned int))];

#if defined(Q_OS_WIN32)
	ZeroMemory(data,((width * height)* 4 * sizeof(unsigned int)));
#elif defined(Q_OS_LINUX)
	memset(data, 0, (width * height)* 4 * sizeof(unsigned int));
#endif
	
	glGenTextures(1, &txtnumber);
	glBindTexture(GL_TEXTURE_2D, txtnumber);
	glTexImage2D(GL_TEXTURE_2D, 0, 4, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	//glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
	//glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
	
	delete [] data;
	return txtnumber;
}

inline void drawQuad2Di(int x, int y, int width, int height) {

	glEnable(GL_TEXTURE_2D);

	glBegin(GL_QUADS);
	glTexCoord2f(0, 1); glVertex2i(x - width/2, y - height/2);
	glTexCoord2f(1, 1); glVertex2i(x + width/2, y - height/2);
	glTexCoord2f(1, 0); glVertex2i(x + width/2, y + height/2);
	glTexCoord2f(0, 0); glVertex2i(x - width/2, y + height/2);
	glEnd();

	glDisable(GL_TEXTURE_2D);
}

inline void drawQuad3Df(float x, float y, float z, float width, float height) {

	glEnable(GL_TEXTURE_2D);

	glBegin(GL_QUADS);
	glTexCoord2f(0, 1); glVertex3f(x - width/2, y - height/2, -z);
	glTexCoord2f(1, 1); glVertex3f(x + width/2, y - height/2, -z);
	glTexCoord2f(1, 0); glVertex3f(x + width/2, y + height/2, -z);
	glTexCoord2f(0, 0); glVertex3f(x - width/2, y + height/2, -z);
	glEnd();

	glDisable(GL_TEXTURE_2D);
}

inline void drawGLRect(const QRect &rect, const QColor &color) {

	if (rect.isEmpty()) return;

	// draw a rectangle using 2 triangles
	GLint x1 = rect.left();
	GLint y1 = rect.top();
	GLint x2 = x1 + rect.width();//rect.right() + 1;
	GLint y2 = y1 + rect.height();//rect.bottom() + 1;

	TGL2int vertexArray[4] = {{x1, y1}, {x2, y1}, {x1, y2}, {x2, y2}};
	
	glColor4ub(color.red(), color.green(), color.blue(), color.alpha());

	glEnableClientState(GL_VERTEX_ARRAY);
	glVertexPointer(2, GL_INT, 0, vertexArray);
	glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);
	glDisableClientState(GL_VERTEX_ARRAY);
}

inline void drawGLRect(const QRect &rect, const QColor &color, float z) {

	if (rect.isEmpty()) return;

	// draw a rectangle using 2 triangles
	GLint x1 = rect.left();
	GLint y1 = rect.top();
	GLint x2 = x1 + rect.width();
	GLint y2 = y1 + rect.height();

	TGL3float vertexArray[4] = 
		{
			{(GLfloat)x1, (GLfloat)y1, z}, 
			{(GLfloat)x2, (GLfloat)y1, z}, 
			{(GLfloat)x1, (GLfloat)y2, z}, 
			{(GLfloat)x2, (GLfloat)y2, z}
		};

	glColor4ub(color.red(), color.green(), color.blue(), color.alpha());

	glEnableClientState(GL_VERTEX_ARRAY);
	glVertexPointer(3, GL_FLOAT, 0, vertexArray);
	glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);
	glDisableClientState(GL_VERTEX_ARRAY);
}

inline void drawGLRect(const QRect &rect, const QColor &color1, const QColor &color2, bool colorLeftToRight) {

	// draw a rectangle using 2 triangles

	GLint x1 = rect.left();
	GLint y1 = rect.top();
	GLint x2 = rect.right() + 1;
	GLint y2 = rect.bottom() + 1;

	TGL2int vertexArray[4] = {{x1, y1}, {x2, y1}, {x1, y2}, {x2, y2}};

	s_glRGBA_uByte gl_color1;
	gl_color1.r = color1.red();
	gl_color1.g = color1.green();
	gl_color1.b = color1.blue();
	gl_color1.a = color1.alpha();

	s_glRGBA_uByte gl_color2;
	gl_color2.r = color2.red();
	gl_color2.g = color2.green();
	gl_color2.b = color2.blue();
	gl_color2.a = color2.alpha();

	s_glRGBA_uByte vertexColors[4];
	if (!colorLeftToRight)	{
		
		// top to bottom
		vertexColors[0] = gl_color1;	// top left
		vertexColors[1] = gl_color1;	// top right
		vertexColors[2] = gl_color2;	// bottom left
		vertexColors[3] = gl_color2;	// bottom right
	}
	else {
		
		// left to right
		vertexColors[0] = gl_color1;	// top left
		vertexColors[1] = gl_color2;	// top right
		vertexColors[2] = gl_color1;	// bottom left
		vertexColors[3] = gl_color2;	// bottom right
	}

	glEnableClientState(GL_VERTEX_ARRAY);
	glVertexPointer(2, GL_INT, 0, vertexArray);
	glEnableClientState(GL_COLOR_ARRAY);
	glColorPointer(4, GL_UNSIGNED_BYTE, 0, vertexColors);
	glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);
	glDisableClientState(GL_COLOR_ARRAY);
	glDisableClientState(GL_VERTEX_ARRAY);
}

inline void drawGLRect(const QRect &rect, const QColor &color1, const QColor &color2, float z, bool colorLeftToRight) {

	// draw a rectangle using 2 triangles

	GLint x1 = rect.left();
	GLint y1 = rect.top();
	GLint x2 = rect.right() + 1;
	GLint y2 = rect.bottom() + 1;

	TGL3float vertexArray[4] = 
		{
			{(GLfloat)x1, (GLfloat)y1, z}, 
			{(GLfloat)x2, (GLfloat)y1, z}, 
			{(GLfloat)x1, (GLfloat)y2, z}, 
			{(GLfloat)x2, (GLfloat)y2, z}
		};

	s_glRGBA_uByte gl_color1;
	gl_color1.r = color1.red();
	gl_color1.g = color1.green();
	gl_color1.b = color1.blue();
	gl_color1.a = color1.alpha();

	s_glRGBA_uByte gl_color2;
	gl_color2.r = color2.red();
	gl_color2.g = color2.green();
	gl_color2.b = color2.blue();
	gl_color2.a = color2.alpha();

	s_glRGBA_uByte vertexColors[4];

	if (!colorLeftToRight)	{
		
		// top to bottom
		vertexColors[0] = gl_color1;	// top left
		vertexColors[1] = gl_color1;	// top right
		vertexColors[2] = gl_color2;	// bottom left
		vertexColors[3] = gl_color2;	// bottom right
	}
	else {
		
		// left to right
		vertexColors[0] = gl_color1;	// top left
		vertexColors[1] = gl_color2;	// top right
		vertexColors[2] = gl_color1;	// bottom left
		vertexColors[3] = gl_color2;	// bottom right
	}

	glEnableClientState(GL_VERTEX_ARRAY);
	//glVertexPointer(2, GL_INT, 0, vertexArray);
	glVertexPointer(3, GL_FLOAT, 0, vertexArray);
	glEnableClientState(GL_COLOR_ARRAY);
	glColorPointer(4, GL_UNSIGNED_BYTE, 0, vertexColors);
	glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);
	glDisableClientState(GL_COLOR_ARRAY);
	glDisableClientState(GL_VERTEX_ARRAY);
}

inline void drawGLTriangleLeft(const QRect &rect, const QColor &color, float z) {

	if (rect.isEmpty()) return;

	GLint x1 = rect.left();
	GLint x2 = rect.left() + rect.width();
	GLint y1 = rect.top() + rect.height()/2;
	GLint y2 = rect.top();
	GLint y3 = rect.top() + rect.height();

	TGL3float vertexArray[3] =
		{
			{(GLfloat)x1, (GLfloat)y1, z},
			{(GLfloat)x2, (GLfloat)y2, z},
			{(GLfloat)x2, (GLfloat)y3, z}
		};

	glColor4ub(color.red(), color.green(), color.blue(), color.alpha());

	glEnableClientState(GL_VERTEX_ARRAY);
	glVertexPointer(3, GL_FLOAT, 0, vertexArray);
	glDrawArrays(GL_TRIANGLES, 0, 3);
	glDisableClientState(GL_VERTEX_ARRAY);
}

inline void drawGLTriangleRight(const QRect &rect, const QColor &color, float z) {

	if (rect.isEmpty()) return;

	GLint x1 = rect.left();
	GLint x2 = rect.left() + rect.width();
	GLint y1 = rect.top();
	GLint y2 = y1 + rect.height();
	GLint y3 = y1 + rect.height()/2;

	TGL3float vertexArray[3] =
		{
			{(GLfloat)x1, (GLfloat)y1, z},
			{(GLfloat)x2, (GLfloat)y3, z},
			{(GLfloat)x1, (GLfloat)y2, z}
		};

	glColor4ub(color.red(), color.green(), color.blue(), color.alpha());

	glEnableClientState(GL_VERTEX_ARRAY);
	glVertexPointer(3, GL_FLOAT, 0, vertexArray);
	glDrawArrays(GL_TRIANGLES, 0, 3);
	glDisableClientState(GL_VERTEX_ARRAY);
}

inline void drawGLBackground(const QRect &rect) {

	if (rect.isEmpty()) return;

	GLint x1 = rect.left();
	GLint y1 = rect.top();
	GLint x2 = x1 + rect.width();
	GLint y2 = y1 + rect.height();

	//glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);

	glBegin(GL_TRIANGLE_STRIP);
	  glColor3f((GLfloat)0.13, (GLfloat)0.13, (GLfloat)0.13); glVertex3f(x1, y1, 0.0); // top left corner
	  glColor3f((GLfloat)0.13, (GLfloat)0.13, (GLfloat)0.13); glVertex3f(x2, y1, 0.0); // top right corner
	  glColor3f((GLfloat)0.18, (GLfloat)0.18, (GLfloat)0.18); glVertex3f(x1, y2, 0.0); // bottom left corner
	  glColor3f((GLfloat)0.31, (GLfloat)0.31, (GLfloat)0.31); glVertex3f(x2, y2, 0.0); // bottom right corner
   glEnd();
   //glFlush();
}

inline void drawGLScaleBackground(const QRect &rect, const QColor &color) {

	if (rect.isEmpty()) return;

	GLint x1 = rect.left();
	GLint y1 = rect.top();
	GLint x2 = x1 + rect.width();
	GLint y2 = y1 + rect.height();

	//const bool GL_TEXTURE_2D_wasEnabled = glIsEnabled(GL_TEXTURE_2D);
	glColor4ub(color.red(), color.green(), color.blue(), color.alpha());
	glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);

	glBegin(GL_POLYGON);
	  glVertex3f(x1, y1, 0.0); // top left corner
	  glVertex3f(x2, y1, 0.0); // top middle corner
	  glVertex3f(x2, y2, 0.0); // bottom middle corner
	  glVertex3f(x1, y2, 0.0); // bottom left corner
	glEnd();
	glFlush();
}

inline void renderTexture(
		const QRect &rect, 
		const GLuint texId,
		float z)
{
	if (rect.isEmpty()) return;
	if (!texId) return;

	const bool GL_TEXTURE_2D_enabled = glIsEnabled(GL_TEXTURE_2D);
	GLint oldTex; 
	glGetIntegerv(GL_TEXTURE_BINDING_2D, &oldTex);

	GLint x1 = rect.left();
	GLint y1 = rect.top();
	GLint x2 = x1 + rect.width();
	GLint y2 = y1 + rect.height();

	if (!GL_TEXTURE_2D_enabled)	glEnable(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, texId);

	glBegin(GL_QUADS);
		glTexCoord2f(0, 1); glVertex3f(x1, y1, z);	// top left corner
		glTexCoord2f(1, 1); glVertex3f(x2, y1, z);	// top right corner
		glTexCoord2f(1, 0); glVertex3f(x2, y2, z);	// bottom right corner
		glTexCoord2f(0, 0); glVertex3f(x1, y2, z);	// bottom left corner
	glEnd();

	glBindTexture(GL_TEXTURE_2D, oldTex);
	if (!GL_TEXTURE_2D_enabled) glDisable(GL_TEXTURE_2D);
}

//inline void writeBitmapString(void *font, char *string) {
//
//	char *c;
//	for (c = string; *c != '\0'; c++) glutBitmapCharacter(font, *c);
//}
//
//inline void writeStrokeString(void *font, char *string) {
//
//	char *c;
//	for (c = string; *c != '\0'; c++) glutStrokeCharacter(font, *c);
//}

#endif // _CUSDR_OPENGLTYPES_H
