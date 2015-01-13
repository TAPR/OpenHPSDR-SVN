/**
* @file  cusdr_led.h
* @brief header file for LED objects
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
 
#ifndef CUSDR_LED_H
#define CUSDR_LED_H

#include <QAbstractButton>

#include "cusdr_painter.h"


class QHLed : public QAbstractButton {

	Q_OBJECT

public:	
	QHLed(const QString& text, QWidget *parent = 0);
	~QHLed();

	QSize minimumSizeHint (void) const;

public slots:
	void setColors(const QColor bgColor, const QColor penColor);
	void setLEDText(const QString& text);

protected:
	void paintEvent(QPaintEvent *event);


private:
	QHPainter *p;

	QColor bgColor;
	QColor penColor;

	QLinearGradient linearGrad;
};

#endif // CUSDR_LED_H
