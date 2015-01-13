/**
* @file cusdr_buttons.cpp
* @brief Button implementation class for cuSDR
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

#include "cusdr_buttons.h"

AeroButton::AeroButton(QWidget *parent) 
	: QPushButton(parent)
	,m_state(OFF)
	,m_hovered(false)
	,m_pressed(false)
	,m_color(QColor(150, 150, 150))
	,m_color_on(QColor(85, 210, 250))
	//,m_color_on(QColor(105, 105, 250))
	,m_highlight(QColor(0x91, 0xeb, 0xff))
	,m_shadow(Qt::black)
	,m_textcolor(QColor(255, 255, 255))
	,m_opacity(1.0)
	,m_glass(true)
	,m_roundness(0){}

AeroButton::AeroButton(const QString &text, QWidget *parent) 
	: QPushButton(text, parent)
	,m_state(OFF)
	,m_hovered(false)
	,m_pressed(false)
	,m_color(QColor(90, 90, 90))
	,m_color_on(QColor(85, 210, 250))
	//,m_color_on(QColor(105, 105, 250))
	,m_highlight(QColor(0x91, 0xeb, 0xff))
	,m_shadow(Qt::black)
	,m_textcolor(QColor(255, 255, 255))
	,m_opacity(1.0)
	,m_glass(true)
	,m_roundness(0){}

AeroButton::AeroButton(const QIcon &icon, const QString &text, QWidget *parent) 
	: QPushButton(icon, text, parent)
	,m_state(OFF)
	,m_hovered(false)
	,m_pressed(false)
	,m_color(QColor(150, 150, 150))
	,m_color_on(QColor(85, 210, 250))
	//,m_color_on(QColor(105, 105, 250))
	,m_highlight(QColor(0x91, 0xeb, 0xff))
	,m_shadow(Qt::black)
	,m_textcolor(QColor(255, 255, 255))
	,m_icon(icon)
	,m_opacity(1.0)
	,m_glass(true)
	,m_roundness(0){}


AeroButton::~AeroButton(){}

void AeroButton::paintEvent(QPaintEvent * pe)
{
	Q_UNUSED(pe);

	QPainter painter(this);  
	painter.setRenderHint(QPainter::Antialiasing);

	//test for state changes
	QColor button_color;
	if(this->isEnabled())
	{
		if (m_state == ON) {
			
			m_hovered ? button_color = m_highlight : button_color = m_color_on;
		}
		else if (m_state == OFF) {

			m_hovered ? button_color = m_highlight : button_color = m_color;
		}

		if(m_pressed)
		{
			button_color = m_highlight.darker(250);
		}
	}
	else
	{
		button_color = QColor(50, 50, 50);
	}

	QRect button_rect = this->geometry();

	//outline
	painter.setPen(QPen(QBrush(Qt::black), 2.0));
	QPainterPath outline;
	outline.addRoundRect(0, 0, button_rect.width(), button_rect.height(), m_roundness, m_roundness);
	painter.setOpacity(m_opacity);
	painter.drawPath(outline);

	//gradient
	QLinearGradient gradient(0, 0, 0, button_rect.height());
	gradient.setSpread(QGradient::ReflectSpread);
	gradient.setColorAt(0.0, button_color);
	gradient.setColorAt(0.4, m_shadow);
	gradient.setColorAt(0.6, m_shadow);
	gradient.setColorAt(1.0, button_color);

	QBrush brush(gradient);
	painter.setBrush(brush); 
	painter.setPen(QPen(QBrush(button_color), 2.0));

	//main button
	QPainterPath painter_path;
	painter_path.addRoundRect(1, 1, button_rect.width() - 2, button_rect.height() - 2, m_roundness, m_roundness);
	painter.setClipPath(painter_path);

	painter.setOpacity(m_opacity);
	painter.drawRoundRect(1, 1, button_rect.width() - 2, button_rect.height() - 2, m_roundness, m_roundness);

	//glass highlight
	painter.setBrush(QBrush(Qt::white));
	painter.setPen(QPen(QBrush(Qt::white), 0.01));
	painter.setOpacity(0.30);
	if (m_glass)
		painter.drawRect(1, 1, button_rect.width() - 2, (button_rect.height() / 2) - 2);

	//text
	QString text = this->text();
	if(!text.isNull())
	{
		QFont font = this->font();
		painter.setFont(font);
		//painter.setPen(Qt::white);
		painter.setPen(m_textcolor);
		painter.setOpacity(1.0);
		painter.drawText(0, 0, button_rect.width(), button_rect.height(), Qt::AlignCenter, text);
	}

	//icon
	//QIcon icon = this->icon();
	QIcon icon = m_icon;
	if(!icon.isNull())
	{
		QSize icon_size = this->iconSize();
		QRect icon_position = this->calculateIconPosition(button_rect, icon_size);
		//painter.setOpacity(1.0);
		m_hovered ? painter.setOpacity(1.0) : painter.setOpacity(0.7);
		painter.drawPixmap(icon_position, QPixmap(icon.pixmap(icon_size)));
		//painter.drawPixmap(QRect(0, 0, icon_size.width(), icon_size.height()), QPixmap(icon.pixmap(icon_size)));
	}
}

//void AeroButton::setBtnState( BtnState state ) { m_btnState = state; }

AeroButton::BtnState AeroButton::btnState() const { return m_state; }

void AeroButton::enterEvent(QEvent * e)
{
	m_hovered = true;
	this->repaint();

	QPushButton::enterEvent(e);
}

void AeroButton::leaveEvent(QEvent * e)
{
	m_hovered = false;
	this->repaint();

	QPushButton::leaveEvent(e);
}

void AeroButton::mousePressEvent(QMouseEvent * e)
{
	m_pressed = true;
	this->repaint();

	QPushButton::mousePressEvent(e);
}

void AeroButton::mouseReleaseEvent(QMouseEvent * e)
{
	m_pressed = false;
	this->repaint();

	QPushButton::mouseReleaseEvent(e);
}

QRect AeroButton::calculateIconPosition(QRect button_rect, QSize icon_size)
{
	int x = (button_rect.width() / 2) - (icon_size.width() / 2);
	int y = (button_rect.height() / 2) - (icon_size.height() / 2);
	int width = icon_size.width(); 
	int height = icon_size.height();

	QRect icon_position;
	icon_position.setX(x);
	icon_position.setY(y);
	icon_position.setWidth(width);
	icon_position.setHeight(height);

	return icon_position;
}
