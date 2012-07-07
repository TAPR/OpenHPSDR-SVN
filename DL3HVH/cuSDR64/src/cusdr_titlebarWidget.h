/**
* @file  cusdr_titlebarWidget.h
* @brief hpsdr server titlebar widget
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

#ifndef CUSDR_TITLEBAR_WIDGET_H
#define CUSDR_TITLEBAR_WIDGET_H

#include <QBrush>
#include <QPen>
#include <QPixmap>
#include <QWidget>

#include "cusdr_buttons.h"

class TitleBarWidget : public QWidget
{
	Q_OBJECT

public:
	TitleBarWidget(QWidget *parent = 0);
	~TitleBarWidget();
	
	QSize minimumSizeHint() const;
	QSize sizeHint() const;

	QIcon	window;

	AeroButton*		closeBtn;
	AeroButton*		maxBtn;
	AeroButton*		hideBtn;

public slots:
	void setPen(const QPen &pen);
	void setBrush(const QBrush &brush);
	void setAntialiased(bool antialiased);
	
protected:
	void paintEvent(QPaintEvent *event);
	void resizeEvent(QResizeEvent *event);
	void enterEvent(QEvent *event);
	void leaveEvent(QEvent *event);
	void mouseMoveEvent(QMouseEvent *event);
	void mousePressEvent(QMouseEvent *event);
	void mouseReleaseEvent(QMouseEvent *event);	

private slots:
	void mainWindowClose();
	void maximizeMainWindow();
	void hideTitlebar();

private:
	QFont		m_appFont;
	QFont		m_titleFont;
	QString		m_appName;
	QString		m_appVersion;
	QString		m_callsign;

	QPen		pen;
	QBrush		brush;
	
	bool	antialiased;
	bool	transformed;

	QPixmap	m_icon;
	
	QIcon	m_quitIcon;
	QIcon	m_minimizeIcon;
	QIcon	maximize;
	QIcon	m_hideIcon;
	
	//QPoint	m_mouseDownPos;
	//QPoint	m_mouseDownWindowPos;
	bool	m_mouseOver;

	int		m_appNameFontWidth;
	int		m_appNameFontHeight;
	int		m_appVersionFontWidth;
	int		m_appVersionFontHeight;
	int		m_callsignFontWidth;
	int		m_titlebarHeight;

signals:
	void	mainClose();
	void	mainMaximize();
	void	mainMaximizeHideTitlebar();
};

#endif  // CUSDR_TITLEBAR_WIDGET_H
