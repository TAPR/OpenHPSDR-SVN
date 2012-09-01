/**
* @file cusdr_hpsdrTabWidget.h
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
	, m_settings(Settings::instance())
	, m_minimumWidgetWidth(m_settings->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(m_settings->getMinimumGroupBoxWidth())
{
	setStyleSheet(m_settings->getTabWidgetStyle());
	//setMinimumWidth(m_minimumWidgetWidth);
	//setMaximumWidth (247);
	setContentsMargins(4, 4, 4, 0);
	setMouseTracking(true);
	
	QFont titleFont;
	titleFont.setStyleStrategy(QFont::PreferQuality);
	titleFont.setPointSizeF(8);
	titleFont.setFamily("Arial");
		
	m_hpsdrWidget = new HPSDRWidget(this);
	m_alexWidget = new AlexWidget(this);
	m_pennyWidget = new PennyWidget(this);
	m_emptyWidget = new EmptyWidget(this);
	m_txWidget = new TxWidget(this);

	this->addTab(m_hpsdrWidget, " Config ");
	this->addTab(m_txWidget, " Transmit ");
	this->addTab(m_pennyWidget, " Ext Ctrl ");
	this->addTab(m_emptyWidget, " PA ");
	this->addTab(m_alexWidget, " Alex ");

	if (!m_settings->getPenelopePresence() && !m_settings->getPennyLanePresence() && !QSDR::Hermes) {

		setTabEnabled(1, false);
		setTabEnabled(2, false);
		setTabEnabled(3, false);
	}

	if (!m_settings->getAlexPresence())
		setTabEnabled(4, false);

	setupConnections();
}

HPSDRTabWidget::~HPSDRTabWidget() {

	disconnect(m_settings, 0, this, 0);
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
		m_settings, 
		SIGNAL(alexPresenceChanged(bool)),
		this,
		SLOT(setAlexPresence(bool)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(penelopePresenceChanged(bool)),
		this,
		SLOT(setPennyPresence(bool)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(pennyLanePresenceChanged(bool)),
		this,
		SLOT(setPennyPresence(bool)));
}

void HPSDRTabWidget::setPennyPresence(bool value) {

	setTabEnabled(1, value);
	setTabEnabled(2, value);
	setTabEnabled(3, value);
}

void HPSDRTabWidget::setAlexPresence(bool value) {

	setTabEnabled(4, value);
}

void HPSDRTabWidget::addNICChangedConnection() {

	m_hpsdrWidget->addNICChangedConnection();
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


