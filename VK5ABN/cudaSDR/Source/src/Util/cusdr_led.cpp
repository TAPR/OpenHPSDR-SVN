/**
* @file  cusdr_led.cpp
* @brief LED objects
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-09-21
*/

/*
 *   
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

//#include <QtGui>
#include <QDebug>
#include <QPaintEvent>

#include "cusdr_led.h"
//#include "cusdr_settings.h"


QHLed::QHLed(const QString& text, QWidget *parent) : QAbstractButton (parent) {

	setText(text);
	this->setEnabled(false);
}

QHLed::~QHLed() {

}

void QHLed::setColors(const QColor bgr, const QColor pen) {

	penColor = pen;

	linearGrad = QLinearGradient(QPointF(0, 0), QPointF(0, 1));
	linearGrad.setCoordinateMode(QGradient::ObjectBoundingMode);
	linearGrad.setSpread(QGradient::PadSpread);
	linearGrad.setColorAt(0, bgr);
	//linearGrad.setColorAt(0.8, QColor(0x31, 0x8b, 0xda));
	linearGrad.setColorAt(1, bgr);
	update();
}

//void QHLed::changeColor (QColor color) {
//}

void QHLed::setLEDText(const QString& text) {

	QHLed::setText(text);
}

QSize QHLed::minimumSizeHint(void) const {

	QFontMetrics fontMetrics(QFont("Arial", 8, QFont::Bold));
	int width = fontMetrics.width(text()) + 12;
	return(QSize(width, 13));
}

void QHLed::paintEvent(QPaintEvent *event) {

	int height = event->rect().height();
	int width = event->rect().width();
	
	p = new QHPainter(this);

	p->setPen (QPen(penColor, 1));

	p->fillRoundRect(QRect(0, 0, width, height), 1, 1, 1, 1, QBrush(linearGrad));

	p->setFont (QFont("Arial", 8, -1, true));
	//p->setPen(QPen(m_btnPenColor, 1));
	//p->drawText(event->rect(), Qt::AlignCenter, text());
	p->drawText(event->rect(), Qt::AlignLeft, text());

	p->end();
}
