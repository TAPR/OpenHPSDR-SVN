/**
* @file  cusdr_pennyExtCtrlWidget.cpp
* @brief Penelope/PennyLane J6-Pins settings widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-06-13
*/

/*
 *   Copyright 2012 Hermann von Hasseln, DL3HVH
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

#define LOG_PENNY_WIDGET


#include <QtGui>
#include <QPen>
#include <QDebug>
#include <QScopedPointer>

#include "cusdr_pennyWidget.h"

#define	btn_height		15
#define	btn_width		20
#define	btn_width2		28
#define	btn_width3		32

PennyWidget::PennyWidget(QWidget *parent) 
	: QWidget(parent)
	, m_settings(Settings::instance())
	, m_minimumWidgetWidth(m_settings->getMinimumWidgetWidth())
	//, m_minimumGroupBoxWidth(m_settings->getMinimumGroupBoxWidth())
	, m_minimumGroupBoxWidth(0)
{
	setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 8, 4, 0);
	setMouseTracking(true);

	//QFont titleFont;
	m_normalFont.setStyleStrategy(QFont::PreferQuality);
	m_normalFont.setFixedPitch(true);
	#ifdef Q_OS_MAC
		m_titleFont.setPixelSize(11);
		m_titleFont.setFamily("Arial");
		//m_smallFont.setBold(true);
	#else if Q_OS_WIN
		m_normalFont.setPixelSize(11);
		m_normalFont.setFamily("Arial");
		//m_smallFont.setBold(true);
		//m_smallFont.setItalic(true);
	#endif

	m_smallFont.setStyleStrategy(QFont::PreferAntialias);
	m_smallFont.setFixedPitch(true);
	#ifdef Q_OS_MAC
		m_smallFont.setPixelSize(9);
		m_smallFont.setFamily("Arial");
		//m_smallFont.setBold(true);
	#else if Q_OS_WIN
		m_smallFont.setPixelSize(9);
		m_smallFont.setFamily("Arial");
		//m_smallFont.setBold(true);
		//m_smallFont.setItalic(true);
	#endif

	//**************************************************************
	// create groups
	createReceivePinsGroup();
	createTransmitPinsGroup();

	enableBtn = new AeroButton("Enable", this);
	enableBtn->setRoundness(0);
	enableBtn->setFixedSize (65, btn_height);
	enableBtn->setBtnState(AeroButton::OFF);
	
	CHECKED_CONNECT(
		enableBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(enable()));

	// disable all buttons
	foreach(AeroButton *btn, receivePinsBtnList) {

		btn->setEnabled(false);
		btn->update();
	}

	foreach(AeroButton *btn, transmitPinsBtnList) {

		btn->setEnabled(false);
		btn->update();
	}

	// set main layout
	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(0);
	hbox1->setContentsMargins(4, 0, 4, 0);
	hbox1->addWidget(enableBtn);
	hbox1->addStretch();

	QHBoxLayout *hbox2 = new QHBoxLayout;
	hbox2->setSpacing(0);
	hbox2->setContentsMargins(4, 0, 4, 0);
	hbox2->addWidget(receivePinsGroup);

	QHBoxLayout *hbox3 = new QHBoxLayout;
	hbox3->setSpacing(0);
	hbox3->setContentsMargins(4, 0, 4, 0);
	hbox3->addWidget(transmitPinsGroup);

	mainLayout->addLayout(hbox1);
	mainLayout->addLayout(hbox2);
	mainLayout->addLayout(hbox3);
	mainLayout->addStretch();
		
	setLayout(mainLayout);

	setupConnections();
}

PennyWidget::~PennyWidget() {

	// disconnect all signals
	disconnect(m_settings, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

void PennyWidget::setupConnections() {
}

void PennyWidget::createReceivePinsGroup() {

	QLabel* emptyLabel = new QLabel(" ", this);
    emptyLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	emptyLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band160mLabel = new QLabel("160m", this);
    band160mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band160mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band80mLabel = new QLabel("80m", this);
    band80mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band80mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band60mLabel = new QLabel("60m", this);
    band60mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band60mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band40mLabel = new QLabel("40m", this);
    band40mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band40mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band30mLabel = new QLabel("30m", this);
    band30mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band30mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band20mLabel = new QLabel("20m", this);
    band20mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band20mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band17mLabel = new QLabel("17m", this);
    band17mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band17mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band15mLabel = new QLabel("15m", this);
    band15mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band15mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band12mLabel = new QLabel("12m", this);
    band12mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band12mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band10mLabel = new QLabel("10m", this);
    band10mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band10mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band6mLabel = new QLabel("6m", this);
    band6mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band6mLabel->setStyleSheet(m_settings->getLabelStyle());

	// buttons
	QString str = "%1";
	for (int i = 0; i < 11; i++) { // bands

		for (int j = 0; j < 7; j++) { // pins

			AeroButton* btn = new AeroButton(str.arg(j+1), this);
			btn->setRoundness(0);
			btn->setFixedSize (btn_width, btn_height);
			btn->setStyleSheet(m_settings->getMiniButtonStyle());
			receivePinsBtnList.append(btn);

			CHECKED_CONNECT(
				btn,
				SIGNAL(clicked()), 
				this, 
				SLOT(receivePinsBtnClicked()));
		}
	}

	QGridLayout* grid = new QGridLayout(this);
	grid->setVerticalSpacing(2);
	grid->setHorizontalSpacing(0);
	grid->setContentsMargins(5, 10, 20, 10);

	grid->addWidget(emptyLabel, 0, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 0, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(emptyLabel, 0, 2, 1, 7, Qt::AlignCenter);
	
	grid->addWidget(band160mLabel, 1, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 1, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(0), 1, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(1), 1, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(2), 1, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(3), 1, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(4), 1, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(5), 1, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(6), 1, 8, 1, 1, Qt::AlignCenter);

	grid->addWidget(band80mLabel, 2, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 2, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(7), 2, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(8), 2, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(9), 2, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(10), 2, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(11), 2, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(12), 2, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(13), 2, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band60mLabel, 3, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 3, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(14), 3, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(15), 3, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(16), 3, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(17), 3, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(18), 3, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(19), 3, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(20), 3, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band40mLabel, 4, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 4, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(21), 4, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(22), 4, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(23), 4, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(24), 4, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(25), 4, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(26), 4, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(27), 4, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band30mLabel, 5, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 5, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(28), 5, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(29), 5, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(30), 5, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(31), 5, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(32), 5, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(33), 5, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(34), 5, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band20mLabel, 6, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 6, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(35), 6, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(36), 6, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(37), 6, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(38), 6, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(39), 6, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(40), 6, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(41), 6, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band17mLabel, 7, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 7, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(42), 7, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(43), 7, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(44), 7, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(45), 7, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(46), 7, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(47), 7, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(48), 7, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band15mLabel, 8, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 8, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(49), 8, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(50), 8, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(51), 8, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(52), 8, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(53), 8, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(54), 8, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(55), 8, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band12mLabel, 9, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 9, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(56), 9, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(57), 9, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(58), 9, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(59), 9, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(60), 9, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(61), 9, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(62), 9, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band10mLabel, 10, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 10, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(63), 10, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(64), 10, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(65), 10, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(66), 10, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(67), 10, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(68), 10, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(69), 10, 8, 1, 1, Qt::AlignCenter);

	grid->addWidget(band6mLabel, 11, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 11, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(70), 11, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(71), 11, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(72), 11, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(73), 11, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(74), 11, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(75), 11, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(receivePinsBtnList.at(76), 11, 8, 1, 1, Qt::AlignCenter);

	receivePinsGroup = new QGroupBox(tr("Receive Pins"), this);
	receivePinsGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	receivePinsGroup->setLayout(grid);
	receivePinsGroup->setStyleSheet(m_settings->getWidgetStyle());
	receivePinsGroup->setFont(QFont("Arial", 8));
}

void PennyWidget::createTransmitPinsGroup() {

	QLabel* emptyLabel = new QLabel(" ", this);
    emptyLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	emptyLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band160mLabel = new QLabel("160m", this);
    band160mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band160mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band80mLabel = new QLabel("80m", this);
    band80mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band80mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band60mLabel = new QLabel("60m", this);
    band60mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band60mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band40mLabel = new QLabel("40m", this);
    band40mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band40mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band30mLabel = new QLabel("30m", this);
    band30mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band30mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band20mLabel = new QLabel("20m", this);
    band20mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band20mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band17mLabel = new QLabel("17m", this);
    band17mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band17mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band15mLabel = new QLabel("15m", this);
    band15mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band15mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band12mLabel = new QLabel("12m", this);
    band12mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band12mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band10mLabel = new QLabel("10m", this);
    band10mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band10mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band6mLabel = new QLabel("6m", this);
    band6mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band6mLabel->setStyleSheet(m_settings->getLabelStyle());

	QString str = "%1";
	for (int i = 0; i < 11; i++) { // pins

		for (int j = 0; j < 7; j++) { // bands

			AeroButton* btn = new AeroButton(str.arg(j+1), this);
			btn->setRoundness(0);
			btn->setFixedSize (btn_width, btn_height);
			btn->setStyleSheet(m_settings->getMiniButtonStyle());
			transmitPinsBtnList.append(btn);

			CHECKED_CONNECT(
				btn,
				SIGNAL(clicked()), 
				this, 
				SLOT(transmitPinsBtnClicked()));
		}
	}

	QGridLayout* grid = new QGridLayout(this);
	grid->setVerticalSpacing(2);
	grid->setHorizontalSpacing(0);
	grid->setContentsMargins(5, 10, 20, 10);

	grid->addWidget(emptyLabel, 0, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 0, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(emptyLabel, 0, 2, 1, 7, Qt::AlignCenter);
	
	grid->addWidget(band160mLabel, 1, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 1, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(0), 1, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(1), 1, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(2), 1, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(3), 1, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(4), 1, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(5), 1, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(6), 1, 8, 1, 1, Qt::AlignCenter);

	grid->addWidget(band80mLabel, 2, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 2, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(7), 2, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(8), 2, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(9), 2, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(10), 2, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(11), 2, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(12), 2, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(13), 2, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band60mLabel, 3, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 3, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(14), 3, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(15), 3, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(16), 3, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(17), 3, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(18), 3, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(19), 3, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(20), 3, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band40mLabel, 4, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 4, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(21), 4, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(22), 4, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(23), 4, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(24), 4, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(25), 4, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(26), 4, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(27), 4, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band30mLabel, 5, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 5, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(28), 5, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(29), 5, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(30), 5, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(31), 5, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(32), 5, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(33), 5, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(34), 5, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band20mLabel, 6, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 6, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(35), 6, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(36), 6, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(37), 6, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(38), 6, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(39), 6, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(40), 6, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(41), 6, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band17mLabel, 7, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 7, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(42), 7, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(43), 7, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(44), 7, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(45), 7, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(46), 7, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(47), 7, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(48), 7, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band15mLabel, 8, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 8, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(49), 8, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(50), 8, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(51), 8, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(52), 8, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(53), 8, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(54), 8, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(55), 8, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band12mLabel, 9, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 9, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(56), 9, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(57), 9, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(58), 9, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(59), 9, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(60), 9, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(61), 9, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(62), 9, 8, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band10mLabel, 10, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 10, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(63), 10, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(64), 10, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(65), 10, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(66), 10, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(67), 10, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(68), 10, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(69), 10, 8, 1, 1, Qt::AlignCenter);

	grid->addWidget(band6mLabel, 11, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 11, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(70), 11, 2, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(71), 11, 3, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(72), 11, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(73), 11, 5, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(74), 11, 6, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(75), 11, 7, 1, 1, Qt::AlignCenter);
	grid->addWidget(transmitPinsBtnList.at(76), 11, 8, 1, 1, Qt::AlignCenter);

	transmitPinsGroup = new QGroupBox(tr("Transmit Pins"), this);
	transmitPinsGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	transmitPinsGroup->setLayout(grid);
	transmitPinsGroup->setStyleSheet(m_settings->getWidgetStyle());
	transmitPinsGroup->setFont(QFont("Arial", 8));
}


//**************
void PennyWidget::enable() {

	if (enableBtn->btnState() == AeroButton::OFF) {

		enableBtn->setBtnState(AeroButton::ON);

		foreach(AeroButton *btn, receivePinsBtnList) {

			btn->setEnabled(true);
			btn->update();
		}

		foreach(AeroButton *btn, transmitPinsBtnList) {

			btn->setEnabled(true);
			btn->update();
		}
	}
	else {

		enableBtn->setBtnState(AeroButton::OFF);

		foreach(AeroButton *btn, receivePinsBtnList) {

			btn->setEnabled(false);
			btn->update();
		}

		foreach(AeroButton *btn, transmitPinsBtnList) {

			btn->setEnabled(false);
			btn->update();
		}
	}
}

void PennyWidget::receivePinsBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = receivePinsBtnList.indexOf(button);

	if (button->btnState() == AeroButton::ON) {
			
			button->setBtnState(AeroButton::OFF);
			button->update();
	}
	else {
		
		button->setBtnState(AeroButton::ON);
		button->update();
	}
	
	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void PennyWidget::transmitPinsBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = transmitPinsBtnList.indexOf(button);

	if (button->btnState() == AeroButton::ON) {
			
			button->setBtnState(AeroButton::OFF);
			button->update();
	}
	else {
		
		button->setBtnState(AeroButton::ON);
		button->update();
	}
	
	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}


//*******************
void PennyWidget::resizeEvent(QResizeEvent *event) {

	//Q_UNUSED(event);
	//QResizeEvent::oldSize()
	
	//QTimer::singleShot(10, this, SLOT(getRegion()));
	//m_resizeFrame = true;
	
	QWidget::resizeEvent(event);
}