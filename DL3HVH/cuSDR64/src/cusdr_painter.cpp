/**
* @file cusdr_painter.cpp
* @brief painter class for cuSDR
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
#include "cusdr_painter.h"
#include "cusdr_image.h"

QHPainter::QHPainter() 
	: QPainter()
{
	setRenderHint(QPainter::Antialiasing);
	setRenderHint(QPainter::TextAntialiasing);
}

QHPainter::QHPainter(QPaintDevice *device)
	: QPainter(device)
{
	setRenderHint(QPainter::Antialiasing);
	setRenderHint(QPainter::TextAntialiasing);
}

QHPainter::~QHPainter() {
}


QPainterPath QHPainter::roundRectangle (const QRectF& rect, qreal radius) {

	return(roundRectangle(rect, radius, radius, radius, radius));
}

QPainterPath QHPainter::roundRectangle (const QRectF& rect, qreal leftRadius, qreal rightRadius) {

	return(roundRectangle(rect, leftRadius, leftRadius, rightRadius, rightRadius));
}

QPainterPath QHPainter::topShadowRectangle (const QRectF& rect, qreal radius) {

	return(topShadowRectangle(rect, radius, radius, radius, radius));
}

QPainterPath QHPainter::topShadowRectangle (const QRectF& rect, qreal leftRadius, qreal rightRadius) {

	return(topShadowRectangle(rect, leftRadius, leftRadius, rightRadius, rightRadius));
}

QPainterPath QHPainter::roundRectangle (const QRectF& rect, 
										qreal leftTopRadius,
										qreal leftBottomRadius,
										qreal rightTopRadius,
										qreal rightBottomRadius)
{
	// Top Left Corner
	// Top Side
	// Top right corner
	// Right side
	// Bottom right corner
	// Bottom side
	// Bottom left corner

	QPainterPath path(QPoint(rect.left(), rect.top() + leftTopRadius));
	path.quadTo(rect.left(), rect.top(), rect.left() + leftTopRadius, rect.top());
	path.lineTo(rect.right() - rightTopRadius, rect.top());
	path.quadTo(rect.right(), rect.top(), rect.right(), rect.top() + rightTopRadius);
	path.lineTo(rect.right(), rect.bottom() - rightBottomRadius);
	path.quadTo(rect.right(), rect.bottom(), rect.right() - rightBottomRadius, rect.bottom());
	path.lineTo(rect.left() + leftBottomRadius, rect.bottom());
	path.quadTo(rect.left(), rect.bottom(), rect.left(), rect.bottom() - leftBottomRadius);
	path.closeSubpath();

	return(path);
}

QPainterPath QHPainter::topShadowRectangle (const QRectF& rect, 
											qreal leftTopRadius,
											qreal leftBottomRadius,
											qreal rightTopRadius,
											qreal rightBottomRadius)
{
	// Top Left Corner
	// Top Side
	// Top right corner
	// Right side
	// Bottom right corner
	// Bottom side
	// Bottom left corner
	Q_UNUSED(rightBottomRadius)
	
	QPainterPath path(QPoint(rect.left(), rect.bottom() - leftBottomRadius));
	path.lineTo(rect.left(), rect.top() + leftTopRadius);
	path.quadTo(rect.left(), rect.top(), rect.left() + leftTopRadius, rect.top());
	path.lineTo(rect.right() - rightTopRadius, rect.top());

	return(path);
}

void QHPainter::drawRoundRect (const QRectF& rect, qreal radius) {
	drawPath(roundRectangle(rect, radius, radius, radius, radius));
}

void QHPainter::drawRoundRect (const QRectF& rect, 
									qreal leftRadius,
									qreal rightRadius) 
{
	drawPath(roundRectangle(rect, leftRadius, leftRadius,
							rightRadius, rightRadius));
}

void QHPainter::drawRoundRect (	const QRectF& rect,
								qreal leftTopRadius,
								qreal leftBottomRadius,
								qreal rightTopRadius,
								qreal rightBottomRadius)
{
	drawPath(roundRectangle(rect, leftTopRadius, leftBottomRadius,
							rightTopRadius, rightBottomRadius));
}

void QHPainter::drawTopShadow (const QRectF& rect, qreal radius) {

	drawPath(topShadowRectangle(rect, radius, radius, radius, radius));
}

void QHPainter::drawTopShadow (const QRectF& rect, 
									qreal leftRadius,
									qreal rightRadius) 
{
	drawPath(topShadowRectangle(rect, leftRadius, leftRadius,
								rightRadius, rightRadius));
}

void QHPainter::drawTopShadow (	const QRectF& rect,
								qreal leftTopRadius,
								qreal leftBottomRadius,
								qreal rightTopRadius,
								qreal rightBottomRadius)
{
	drawPath(topShadowRectangle(rect, leftTopRadius, leftBottomRadius,
								rightTopRadius, rightBottomRadius));
}

void QHPainter::drawShadowText (qreal x, qreal y,
								const QString& text,
								const QColor& shadowColor,
								const QPointF& offset,
								qreal radius)
{
	QPainter p;

	// Draw Text
	QRect textRect = QFontMetrics(text).boundingRect(text);
	QImage textImage(textRect.size(), QImage::Format_ARGB32_Premultiplied);
	textImage.fill(Qt::transparent);
	p.begin(&textImage);
	p.setPen(pen());
	p.setFont(font());
	p.drawText(textImage.rect(), Qt::AlignLeft, text);
	p.end();

	// Draw Blurred Shadow
	QHImage shadowImage(textRect.size() + QSize(radius * 2, radius * 2),
						QImage::Format_ARGB32_Premultiplied);
	shadowImage.fill(Qt::transparent);
	p.begin(&shadowImage);
	p.drawImage(radius, radius, textImage);
	p.end();
	shadowImage.shadowBlur(radius, shadowColor);

	// Compose Text and Shadow
	int addSizeX = (offset.x() > radius) ? (abs(offset.x()) - radius) : 0;
	int addSizeY = (offset.y() > radius) ? (abs(offset.y()) - radius) : 0;
	QSize finalSize = shadowImage.size() + QSize(addSizeX, addSizeY);

	QPointF shadowTopLeft(QPointF((finalSize.width() - shadowImage.width()) / 2.0,
							(finalSize.height() - shadowImage.height()) / 2.0) +
							(offset / 2.0));
	QPointF textTopLeft(QPointF((finalSize.width() - textImage.width()) / 2.0,
						((finalSize.height() - textImage.height()) / 2.0)) - 
						(offset / 2.0));

	// Paint Text and Shadow
	save();
	translate(x, y);
	setCompositionMode(QPainter::CompositionMode_Xor);
	drawImage(shadowTopLeft, shadowImage);
	drawImage(textTopLeft, textImage);
	restore();
}


void QHPainter::fillRoundRect (	const QRectF& rect,
								qreal radius,
								const QBrush& brush)
{
	fillPath(roundRectangle(rect, radius, radius, radius, radius), brush);
}

void QHPainter::fillRoundRect (	const QRectF& rect,
								qreal leftRadius,
								qreal rightRadius,
								const QBrush& brush)
{
	fillPath(roundRectangle(rect, leftRadius, leftRadius, 
							rightRadius, rightRadius), brush);
}

void QHPainter::fillRoundRect (	const QRectF& rect,
								qreal leftTopRadius,
								qreal leftBottomRadius,
								qreal rightTopRadius,
								qreal rightBottomRadius,
								const QBrush& brush)
{
	fillPath(roundRectangle(rect, leftTopRadius, leftBottomRadius, 
							rightTopRadius, rightBottomRadius), brush);
}

