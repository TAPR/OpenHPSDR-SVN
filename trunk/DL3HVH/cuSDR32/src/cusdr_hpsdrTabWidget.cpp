/**
* @file cusdr_hpsdrTabWidget.cpp
* @brief Hardware settings widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-09-21
*/

/*
 *   
 *   Copyright 2010, 2011 Hermann von Hasseln, DL3HVH
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

#include "cusdr_hpsdrTabWidget.h"


#define	btn_height		15
#define	btn_width		74
#define	btn_width2		52
#define	btn_widths		40


HPSDRTabWidget::HPSDRTabWidget(QWidget *parent) 
	: QTabWidget(parent)
	, set(Settings::instance())
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(set->getMinimumGroupBoxWidth())
{
	setStyleSheet(set->getTabWidgetStyle());
	setContentsMargins(4, 4, 4, 0);
	setMouseTracking(true);
	
	m_hpsdrWidget = new HPSDRWidget(this);
	m_networkWidget = new NetworkWidget(this);
	//m_transmitTabWidget = new TransmitTabWidget(this);
	m_alexTabWidget = new AlexTabWidget(this);
	m_extCtrlWidget = new ExtCtrlWidget(this);

	this->addTab(m_hpsdrWidget, " Config ");
	this->addTab(m_networkWidget, " Network ");
	//this->addTab(m_transmitTabWidget, " Transmit ");
	this->addTab(m_extCtrlWidget, " Ext Ctrl ");
	this->addTab(m_alexTabWidget, " Alex ");

	setTabEnabled(1, true);

	if (!set->getPenelopePresence() && !set->getPennyLanePresence() && (set->getHWInterface() != QSDR::Hermes)) {

		setTabEnabled(2, false);
		//setTabEnabled(3, false);
	}

	if (!set->getAlexPresence())
		setTabEnabled(3, false);

	setupConnections();
}

HPSDRTabWidget::~HPSDRTabWidget() {

	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

QSize HPSDRTabWidget::sizeHint() const {
	
	return QSize(m_minimumWidgetWidth, height());
}

QSize HPSDRTabWidget::minimumSizeHint() const {

	return QSize(m_minimumWidgetWidth, height());
}

void HPSDRTabWidget::setupConnections() {

	CHECKED_CONNECT(
		set,
		SIGNAL(systemStateChanged(
					QObject *,
					QSDR::_Error,
					QSDR::_HWInterfaceMode,
					QSDR::_ServerMode,
					QSDR::_DataEngineState)),
		this,
		SLOT(systemStateChanged(
					QObject *,
					QSDR::_Error,
					QSDR::_HWInterfaceMode,
					QSDR::_ServerMode,
					QSDR::_DataEngineState)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(alexPresenceChanged(bool)),
		this,
		SLOT(setAlexPresence(bool)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(penelopePresenceChanged(bool)),
		this,
		SLOT(setPennyPresence(bool)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(pennyLanePresenceChanged(bool)),
		this,
		SLOT(setPennyPresence(bool)));
}

void HPSDRTabWidget::systemStateChanged(
	QObject *sender,					/*!<[in] the sender of the signal. */
	QSDR::_Error err,					/*!<[in] error state. */
	QSDR::_HWInterfaceMode hwmode,		/*!<[in] HPSDR interface (Metis, Hermes, none). */
	QSDR::_ServerMode mode,				/*!<[in] server mode. */
	QSDR::_DataEngineState state		/*!<[in] data engine state. */
) {
	Q_UNUSED (sender)
	Q_UNUSED (err)

	//if (sender == this) return;

	if (m_hwInterface != hwmode)
		m_hwInterface = hwmode;

	if (m_hwInterface == QSDR::Hermes)
		setTabEnabled(2, true);
	else
		setTabEnabled(2, false);

	if (m_serverMode != mode)
		m_serverMode = mode;

	if (m_dataEngineState != state)
		m_dataEngineState = state;
}

void HPSDRTabWidget::setPennyPresence(bool value) {

	//setTabEnabled(1, value);
	setTabEnabled(2, value);
	//setTabEnabled(3, value);
}

void HPSDRTabWidget::setAlexPresence(bool value) {

	setTabEnabled(3, value);
}

void HPSDRTabWidget::addNICChangedConnection() {

	m_networkWidget->addNICChangedConnection();
}

void HPSDRTabWidget::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void HPSDRTabWidget::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}

void HPSDRTabWidget::enterEvent(QEvent *event) {

	Q_UNUSED(event)
}

void HPSDRTabWidget::leaveEvent(QEvent *event) {

	Q_UNUSED(event)
}

void HPSDRTabWidget::mouseMoveEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void HPSDRTabWidget::mousePressEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void HPSDRTabWidget::mouseReleaseEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}


