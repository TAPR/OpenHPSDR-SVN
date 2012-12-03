/**
* @file cusdr_image.cpp
* @brief image definitions class for cuSDR
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

#include "cusdr_imageblur.h"
#include "cusdr_painter.h"
#include "cusdr_image.h"

QHImage::QHImage (const QSize& size, Format format)
	: QImage(size, format)
{
}

QHImage::QHImage (int width, int height, Format format)
	: QImage(width, height, format)
{
}

QHImage::QHImage (uchar *data, int width, int height, Format format)
	: QImage(data, width, height, format)
{
}

QHImage::QHImage (const uchar *data, int width, int height, Format format)
	: QImage(data, width, height, format)
{
}

QHImage::QHImage (uchar *data, int width, int height, int bytesPerLine, Format format)
	: QImage(data, width, height, bytesPerLine, format)
{
}

QHImage::QHImage (const uchar *data, int width, int height, int bytesPerLine, Format format)
	: QImage(data, width, height, bytesPerLine, format)
{
}

QHImage::QHImage (const QString& fileName, const char *format)
	: QImage(fileName, format)
{
}

QHImage::QHImage (const char *fileName, const char *format)
	: QImage(fileName, format)
{
}

QHImage::QHImage (const QImage& image)
	: QImage(image)
{
}

QHImage::~QHImage() {
}


void QHImage::expblur(int aprec, int zprec, int radius) {
	ImageBlur::expblur(this, aprec, zprec, radius);
}
#include <QCoreApplication>

void QHImage::shadowBlur (int radius, const QColor& color) {
	ImageBlur::expblur(this, 16, 7, radius);

	QHPainter p(this);
	p.setCompositionMode(QPainter::CompositionMode_SourceIn);
	p.fillRect(rect(), color);
	p.end();
}


