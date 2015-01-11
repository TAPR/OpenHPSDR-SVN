/**
* @file cusdr_transmitTabWidget.cpp
* @brief Transmit settings tab widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-08-28
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

#include "cusdr_transmitTabWidget.h"


#define	btn_height		15
#define	btn_width		74
#define	btn_width2		52
#define	btn_widths		40


TransmitTabWidget::TransmitTabWidget(QWidget *parent)
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
	
	m_transmitOptionsWidget = new TransmitOptionsWidget(this);
	m_transmitPAWidget = new TransmitPAWidget(this);

	this->addTab(m_transmitOptionsWidget, "Options");
	this->addTab(m_transmitPAWidget, "PA Settings");

	if (!set->getPenelopePresence() && !set->getPennyLanePresence() && !QSDR::Hermes) {

		setTabEnabled(1, false);
//		setTabEnabled(2, false);
//		setTabEnabled(3, false);
	}

//	if (!set->getAlexPresence())
//		setTabEnabled(4, false);

	setupConnections();
}

TransmitTabWidget::~TransmitTabWidget() {

	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

QSize TransmitTabWidget::sizeHint() const {
	
	return QSize(m_minimumWidgetWidth, height());
}

QSize TransmitTabWidget::minimumSizeHint() const {

	return QSize(m_minimumWidgetWidth, height());
}

void TransmitTabWidget::setupConnections() {

//	CHECKED_CONNECT(
//		set,
//		SIGNAL(alexPresenceChanged(bool)),
//		this,
//		SLOT(setAlexPresence(bool)));

//	CHECKED_CONNECT(
//		set,
//		SIGNAL(penelopePresenceChanged(bool)),
//		this,
//		SLOT(setPennyPresence(bool)));
//
//	CHECKED_CONNECT(
//		set,
//		SIGNAL(pennyLanePresenceChanged(bool)),
//		this,
//		SLOT(setPennyPresence(bool)));
}

void TransmitTabWidget::setPennyPresence(bool value) {

	setTabEnabled(1, value);
//	setTabEnabled(2, value);
//	setTabEnabled(3, value);
}

//void TransmitTabWidget::setAlexPresence(bool value) {
//
//	setTabEnabled(4, value);
//}

void TransmitTabWidget::addNICChangedConnection() {

	//m_hpsdrWidget->addNICChangedConnection();
}

void TransmitTabWidget::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void TransmitTabWidget::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}

void TransmitTabWidget::enterEvent(QEvent *event) {

	Q_UNUSED(event)
}

void TransmitTabWidget::leaveEvent(QEvent *event) {

	Q_UNUSED(event)
}

void TransmitTabWidget::mouseMoveEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void TransmitTabWidget::mousePressEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void TransmitTabWidget::mouseReleaseEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}


