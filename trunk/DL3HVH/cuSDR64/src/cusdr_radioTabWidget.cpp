/**
* @file  cusdr_radioTabWidget.cpp
* @brief Radio settings tab widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-09-29
*/

/*
 *   
 *   Copyright 2010 - 2012 Hermann von Hasseln, DL3HVH
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

#include "cusdr_radioTabWidget.h"


#define	btn_height		15
#define	btn_width		74
#define	btn_width2		52
#define	btn_widths		40


RadioTabWidget::RadioTabWidget(QWidget *parent)
	: QTabWidget(parent)
	, set(Settings::instance())
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(set->getMinimumGroupBoxWidth())
{
	setStyleSheet(set->getTabWidgetStyle());
	setContentsMargins(4, 4, 4, 0);
	setMouseTracking(true);
	
	m_radioWidget = new RadioWidget(this);
	m_agcWidget = new AGCOptionsWidget(this);
	m_transmitTabWidget = new TransmitTabWidget(this);

	this->addTab(m_radioWidget, "  General ");
	this->addTab(m_agcWidget, " AGC ");
	this->addTab(m_transmitTabWidget, " Tx Ctrl ");

	if (!set->getPenelopePresence() && !set->getPennyLanePresence() && (set->getHWInterface() != QSDR::Hermes)) {

		setTabEnabled(2, false);
	}

	setupConnections();
}

RadioTabWidget::~RadioTabWidget() {

	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

QSize RadioTabWidget::sizeHint() const {
	
	return QSize(m_minimumWidgetWidth, height());
}

QSize RadioTabWidget::minimumSizeHint() const {

	return QSize(m_minimumWidgetWidth, height());
}

void RadioTabWidget::setupConnections() {

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
		SIGNAL(penelopePresenceChanged(bool)),
		this,
		SLOT(setPennyPresence(bool)));

	CHECKED_CONNECT(
		set,
		SIGNAL(pennyLanePresenceChanged(bool)),
		this,
		SLOT(setPennyPresence(bool)));
}

void RadioTabWidget::systemStateChanged(
	QObject *sender,					/*!<[in] the sender of the signal. */
	QSDR::_Error err,					/*!<[in] error state. */
	QSDR::_HWInterfaceMode hwmode,		/*!<[in] HPSDR interface (Metis, Hermes, none). */
	QSDR::_ServerMode mode,				/*!<[in] server mode. */
	QSDR::_DataEngineState state		/*!<[in] data engine state. */
) {
	Q_UNUSED (sender)
	Q_UNUSED (err)

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

void RadioTabWidget::setPennyPresence(bool value) {

	setTabEnabled(2, value);
}

void RadioTabWidget::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void RadioTabWidget::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}

void RadioTabWidget::enterEvent(QEvent *event) {

	Q_UNUSED(event)
}

void RadioTabWidget::leaveEvent(QEvent *event) {

	Q_UNUSED(event)
}

void RadioTabWidget::mouseMoveEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void RadioTabWidget::mousePressEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void RadioTabWidget::mouseReleaseEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}


