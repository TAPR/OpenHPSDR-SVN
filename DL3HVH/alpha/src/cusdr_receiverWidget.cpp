/**
* @file  cusdr_receiverWidget.cpp
* @brief receiver widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-11-22
*/

/*
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
 
//#include <QtGui>
//#include <QDebug>

#include "cusdr_receiverWidget.h"
#include "cusdr_settings.h"

#define	btn_height		15
#define	btn_width		60
#define	btn_widths		40


ReceiverWidget::ReceiverWidget() 
	: QWidget()
{
	setObjectName("ReceiverWidget");

	setMouseTracking(true);
	//mouse_over = false;

	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(1);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);
	//mainLayout->addWidget(dspBtnGroup());
	//mainLayout->addSpacing(5);
	//mainLayout->addWidget(fftwExtGroup());
	mainLayout->addStretch();
		
	setLayout(mainLayout);

	setContentsMargins(5, 5, 5, 5);
}

ReceiverWidget::~ReceiverWidget() {

	disconnect(Settings::instance(), 0, this, 0);
	disconnect(this, 0, 0, 0);
}



// **********************

void ReceiverWidget::closeEvent(QCloseEvent *event) {

	//settings::instance()->serverFormRect = QRect(pos(), size());

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void ReceiverWidget::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}

//***************************************************

MiniReceiverWidget::MiniReceiverWidget() 
	: QWidget()
{
	setObjectName("MiniReceiverWidget");

	setMouseTracking(true);
	//mouse_over = false;

	QFont titleFont;
	titleFont.setStyleStrategy(QFont::PreferQuality);
	titleFont.setPointSizeF(8);
	titleFont.setFamily("Arial");

	setContentsMargins(5, 5, 5, 5);
}

MiniReceiverWidget::~MiniReceiverWidget() {

	disconnect(Settings::instance(), 0, this, 0);
	disconnect(this, 0, 0, 0);
}



// **********************

void MiniReceiverWidget::closeEvent(QCloseEvent *event) {

	//settings::instance()->serverFormRect = QRect(pos(), size());

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void MiniReceiverWidget::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}
