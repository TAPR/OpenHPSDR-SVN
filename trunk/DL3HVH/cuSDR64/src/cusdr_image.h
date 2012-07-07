/**
* @file cusdr_image.h
* @brief image definitions header file for cuSDR
*/

/*
 *   This is an adaptation of Matteo Bertozzi's "Black Bar and Buttons" code.
 *
 *   see: http://qt-apps.org/content/show.php/Black+Bar+and+Buttons?content=100399
 *
 *   Copyright 2009 Matteo Bertozzi
 *   Copyright 2010 Hermann von Hasseln, DL3HVH
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
#ifndef CUSDR_IMAGE_H
#define CUSDR_IMAGE_H

#include <QImage>

class QHImage : public QImage {
	public:
		QHImage (const QSize& size, Format format);
		QHImage (int width, int height, Format format);
		QHImage (uchar *data, int width, int height, Format format);
		QHImage (const uchar *data, int width, int height, Format format);
		QHImage (uchar *data, int width, int height, int bytesPerLine, Format format);
		QHImage (const uchar *data, int width, int height, int bytesPerLine, Format format);
		QHImage (const QString& fileName, const char *format = 0);
		QHImage (const char *fileName, const char *format = 0);
		QHImage (const QImage& image);
		~QHImage();

	public:
		void expblur(int aprec, int zprec, int radius);
		void shadowBlur (int radius, const QColor& color);
};

#endif // CUSDR_IMAGE_H

