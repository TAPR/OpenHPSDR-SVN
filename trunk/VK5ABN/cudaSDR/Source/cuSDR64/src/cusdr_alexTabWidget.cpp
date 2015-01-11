/**
* @file cusdr_alexTabWidget.cpp
* @brief Alex settings widget class for cuSDR
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
 
#include <QtGui>
#include <QMenu>
#include <QFileDialog>
#include <QDebug>

#include "cusdr_alexTabWidget.h"


#define	btn_height		15
#define	btn_width		74
#define	btn_width2		52
#define	btn_widths		40


AlexTabWidget::AlexTabWidget(QWidget *parent)
	: QTabWidget(parent)
	, set(Settings::instance())
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(set->getMinimumGroupBoxWidth())
{
	setStyleSheet(set->getTabWidgetStyle());
	//setMinimumWidth(m_minimumWidgetWidth);
	//setMaximumWidth (247);
	setContentsMargins(4, 4, 4, 0);
	setMouseTracking(true);
	
	m_alexAntennaWidget = new AlexAntennaWidget(this);
	m_alexFilterWidget = new AlexFilterWidget(this);

	this->addTab(m_alexAntennaWidget, "Antenna");
	this->addTab(m_alexFilterWidget, "Filter");

	setupConnections();
}

AlexTabWidget::~AlexTabWidget() {

	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

QSize AlexTabWidget::sizeHint() const {
	
	return QSize(m_minimumWidgetWidth, height());
}

QSize AlexTabWidget::minimumSizeHint() const {

	return QSize(m_minimumWidgetWidth, height());
}

void AlexTabWidget::setupConnections() {

}

void AlexTabWidget::addNICChangedConnection() {

	//m_hpsdrWidget->addNICChangedConnection();
}
