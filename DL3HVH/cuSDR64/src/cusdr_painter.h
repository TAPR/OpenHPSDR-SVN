/**
* @file cusdr_painter.h
* @brief painter header file for cuSDR
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

#ifndef CUSDR_PAINTER_H
#define CUSDR_PAINTER_H

#include <QPainter>

class QHPainter : public QPainter {

public:
	QHPainter();
	QHPainter(QPaintDevice *device);
	~QHPainter();

	// STATIC Methods
	static QPainterPath roundRectangle (const QRectF& rect, qreal radius);
	static QPainterPath roundRectangle (const QRectF& rect, 
										qreal leftRadius,
										qreal rightRadius);
	static QPainterPath roundRectangle (const QRectF& rect, 
										qreal leftTopRadius,
										qreal leftBottomRadius,
										qreal rightTopRadius,
										qreal rightBottomRadius);
	static QPainterPath topShadowRectangle (const QRectF& rect, qreal radius);
	static QPainterPath topShadowRectangle (const QRectF& rect, 
											qreal leftRadius,
											qreal rightRadius);
	static QPainterPath topShadowRectangle (const QRectF& rect, 
											qreal leftTopRadius,
											qreal leftBottomRadius,
											qreal rightTopRadius,
											qreal rightBottomRadius);
	
	// Methods
	void drawShadowText (	qreal x, qreal y,
							const QString& text,
							const QColor& shadowColor,
							const QPointF& offset,
							qreal radius);

	void drawRoundRect (const QRectF& rect,
						qreal radius);
	void drawRoundRect (const QRectF& rect,
						qreal leftRadius,
						qreal rightRadius);
	void drawRoundRect (const QRectF& rect,
						qreal leftTopRadius,
						qreal leftBottomRadius,
						qreal rightTopRadius,
						qreal rightBottomRadius);

	void drawTopShadow (const QRectF& rect,
						qreal radius);
	void drawTopShadow (const QRectF& rect,
						qreal leftRadius,
						qreal rightRadius);
	void drawTopShadow (const QRectF& rect,
						qreal leftTopRadius,
						qreal leftBottomRadius,
						qreal rightTopRadius,
						qreal rightBottomRadius);
	
	void fillRoundRect (const QRectF& rect,
						qreal radius,
						const QBrush& brush);
	void fillRoundRect (const QRectF& rect,
						qreal leftRadius,
						qreal rightRadius,
						const QBrush& brush);
	void fillRoundRect (const QRectF& rect,
						qreal leftTopRadius,
						qreal leftBottomRadius,
						qreal rightTopRadius,
						qreal rightBottomRadius,
						const QBrush& brush);
};

#endif // CUSDR_PAINTER_H