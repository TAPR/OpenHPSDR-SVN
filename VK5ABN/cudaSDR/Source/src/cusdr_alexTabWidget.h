/**
* @file  cusdr_alexTabWidget.h
* @brief Alex settings widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-08-23
*/

/*
 *   
 *   Copyright 2010-2012 Hermann von Hasseln, DL3HVH
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
 
#ifndef _CUSDR_ALEX_TABWIDGET_H
#define _CUSDR_ALEX_TABWIDGET_H

//#include <QWidget>
//#include <QTabWidget>
//#include <QGroupBox>
//#include <QLineEdit>

#include "Util/cusdr_buttons.h"
#include "cusdr_settings.h"
#include "cusdr_alexAntennaWidget.h"
#include "cusdr_alexFilterWidget.h"


class AlexTabWidget : public QTabWidget {

	Q_OBJECT

public:
	AlexTabWidget(QWidget *parent = 0);
	~AlexTabWidget();


public slots:
	QSize	sizeHint() const;
	QSize	minimumSizeHint() const;
	void	addNICChangedConnection();
	
private:
	Settings				*set;

	AlexAntennaWidget		*m_alexAntennaWidget;
	AlexFilterWidget		*m_alexFilterWidget;

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;

	void	setupConnections();

};

#endif // _CUSDR_ALEX_TABWIDGET_H
