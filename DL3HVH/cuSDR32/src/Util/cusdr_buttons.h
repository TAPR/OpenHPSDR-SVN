/**
* @file cusdr_buttons.h
* @brief Button implementation header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-11-28
*/

/*
 *	 adapted from: http://www.qtcentre.org/wiki/index.php?title=AeroButton
 *	 Copyright (C) 2008 Jim Daniel
 *
 *	 (C) 2010, 2011 adapted for cuSDR by Hermann von Hasseln, DL3HVH
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

#ifndef CUSDR_BUTTON
#define CUSDR_BUTTON

//#include <QtCore>
#include <QtGui>

class AeroButton : public QPushButton
{
	Q_OBJECT

public:
	AeroButton(QWidget* parent = 0);
	AeroButton(const QString &text, QWidget* parent = 0);
	AeroButton(const QIcon &icon, const QString &text, QWidget* parent = 0);

	~AeroButton();

	enum BtnState { OFF, ON };

	void setBtnState(BtnState state)		{ m_state = state; }
	BtnState btnState() const;

	void setColor(QColor &color)			{ m_color = color; }
	void setColorOn(QColor &color)			{ m_color_on = color; }
	void setTextColor(QColor &color)		{ m_textcolor = color; }
	void setHighlight(QColor &highlight)	{ m_highlight = highlight; }
	void setShadow(QColor &shadow)			{ m_shadow = shadow; }
	void setGlass(bool glass)				{ m_glass = glass; }

	//Range: 0.0 [invisible] - 1.0 [opaque]
	void setOpacity(qreal opacity)			{ m_opacity = opacity; }

	//Range: 0 [rectangle] - 99 [oval]
	void setRoundness(int roundness)		{ m_roundness = roundness; }
	void setIcon(QIcon icon)				{ m_icon = icon; }

protected:
	void paintEvent(QPaintEvent *pe);

	void enterEvent(QEvent *e);
	void leaveEvent(QEvent *e);

	void mousePressEvent(QMouseEvent *e);
	void mouseReleaseEvent(QMouseEvent *e);

private:
	QRect calculateIconPosition(QRect button_rect, QSize icon_size);

private:
	BtnState m_state;

	bool m_hovered;
	bool m_pressed;

	QColor m_color;
	QColor m_color_on;
	QColor m_highlight;
	QColor m_shadow;
	QColor m_textcolor;

	QIcon  m_icon;

	qreal m_opacity;

	bool m_glass;
	int m_roundness;
};

#endif // CUSDR_BUTTON