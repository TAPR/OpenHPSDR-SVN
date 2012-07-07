/**
* @file  cusdr_emptyWidget.h
* @brief 
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-06-16
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
 
#ifndef _CUSDR_EMPTY_WIDGET_H
#define _CUSDR_EMPTY_WIDGET_H

#include <QWidget>
#include <QGroupBox>
#include <QLineEdit>

#include "cusdr_buttons.h"
#include "cusdr_settings.h"


class EmptyWidget : public QWidget {

	Q_OBJECT

public:
	EmptyWidget(QWidget *parent = 0);
	~EmptyWidget();


public slots:
	
protected:
	void	closeEvent(QCloseEvent *event);
	void	showEvent(QShowEvent *event);
	void	enterEvent(QEvent *event);
	void	leaveEvent(QEvent *event);
	void	mouseMoveEvent(QMouseEvent *event);
	void	mousePressEvent(QMouseEvent *event);
	void	mouseReleaseEvent(QMouseEvent *event);

private:
	Settings*	m_settings;

	QString		m_message;

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;
	
	void	setupConnections();
	
private slots:
	
signals:
	void	showEvent(QObject *sender);
	void	closeEvent(QObject *sender);
	void	messageEvent(QString message);
};

#endif // _CUSDR_EMPTY_WIDGET_H
