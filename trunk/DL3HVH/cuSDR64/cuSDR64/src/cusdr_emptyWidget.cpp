/**
* @file  cusdr_hpsdrWidget.cpp
* @brief HPSDR hardware settings widget
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
 
#include <QtGui>
#include <QMenu>
#include <QFileDialog>
#include <QDebug>

#include "cusdr_emptyWidget.h"


#define	btn_height		15
#define	btn_width		74
#define	btn_width2		52
#define	btn_widths		40


EmptyWidget::EmptyWidget(QWidget *parent) 
	: QWidget(parent)
	, m_settings(Settings::instance())
	, m_minimumWidgetWidth(m_settings->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(m_settings->getMinimumGroupBoxWidth())
{
	//setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(0, 8, 0, 0);
	setMouseTracking(true);
	
	QFont titleFont;
	titleFont.setStyleStrategy(QFont::PreferQuality);
	titleFont.setPointSizeF(8);
	titleFont.setFamily("Arial");

	
	setupConnections();
}

EmptyWidget::~EmptyWidget() {

	disconnect(m_settings, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

void EmptyWidget::setupConnections() {

}

// ************************************************************************


void EmptyWidget::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void EmptyWidget::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}

void EmptyWidget::enterEvent(QEvent *event) {

	Q_UNUSED(event)
}

void EmptyWidget::leaveEvent(QEvent *event) {

	Q_UNUSED(event)
}

void EmptyWidget::mouseMoveEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void EmptyWidget::mousePressEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void EmptyWidget::mouseReleaseEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}


