/**
* @file  cusdr_extCtrlWidget.cpp
* @brief J6-Pins settings widget header file for cuSDR
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


//#include <QtGui>
//#include <QPen>
//#include <QDebug>
//#include <QScopedPointer>

#include "cusdr_extCtrlWidget.h"

#define	btn_height		15
#define	btn_width		20
#define	btn_width2		28
#define	btn_width3		32

ExtCtrlWidget::ExtCtrlWidget(QWidget *parent)
	: QWidget(parent)
	, set(Settings::instance())
	, m_pennyOCEnabled(false)
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(0)
{
	setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 8, 4, 0);
	setMouseTracking(true);

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
	foreach(QList<AeroButton *> btnList, receivePinsBtnMatrix) {

		foreach(AeroButton *btn, btnList) {

			btn->setEnabled(false);
			btn->update();
		}
	}

	foreach(QList<AeroButton *> btnList, transmitPinsBtnMatrix) {

		foreach(AeroButton *btn, btnList) {

			btn->setEnabled(false);
			btn->update();
		}
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

	setValues();
	setupConnections();
}

ExtCtrlWidget::~ExtCtrlWidget() {

	// disconnect all signals
	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

void ExtCtrlWidget::setupConnections() {
}

void ExtCtrlWidget::createReceivePinsGroup() {

	QLabel* emptyLabel = new QLabel(" ", this);
    emptyLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	emptyLabel->setStyleSheet(set->getLabelStyle());

	// band label
	QStringList bandNames;
	bandNames << "160 m" << "80 m" << "60 m" << "40 m" << "30 m" << "20 m" << "17 m" << "15 m" << "12 m" << "10 m" << "6 m";

	QList<QLabel *> bandLabelList;

	for (int i = 0; i < MAX_BANDS-1; i++) {

		QLabel *label = new QLabel(bandNames.at(i), this);
		label->setFrameStyle(QFrame::Box | QFrame::Raised);
		label->setStyleSheet(set->getLabelStyle());

		bandLabelList << label;
	}


	// buttons
	QString str = "%1";
	for (int i = 0; i < MAX_BANDS-1; i++) { // bands

		QList<AeroButton *> btnList;
		for (int j = 0; j < 7; j++) { // pins

			AeroButton* btn = new AeroButton(str.arg(j+1), this);
			btn->setRoundness(0);
			btn->setFixedSize (btn_width, btn_height);
			btn->setStyleSheet(set->getMiniButtonStyle());
			//receivePinsBtnList.append(btn);
			btnList.append(btn);

			CHECKED_CONNECT(
				btn,
				SIGNAL(clicked()), 
				this, 
				SLOT(receivePinsBtnClicked()));
		}
		receivePinsBtnMatrix.append(btnList);
	}


	QGridLayout* grid = new QGridLayout(this);
	grid->setVerticalSpacing(2);
	grid->setHorizontalSpacing(0);
	grid->setContentsMargins(5, 10, 20, 10);

	grid->addWidget(emptyLabel, 0, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 0, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(emptyLabel, 0, 2, 1, 7, Qt::AlignCenter);
	
	for (int i = 0; i < MAX_BANDS-1; i++) { // bands

		grid->addWidget(bandLabelList.at(i), i+1, 0, 1, 1, Qt::AlignLeft);
		grid->addWidget(emptyLabel, i+1, 1, 1, 1, Qt::AlignCenter);

		for (int j = 0; j < 7; j++) { // pins

			grid->addWidget(receivePinsBtnMatrix.at(i).at(j), i+1, j+2, 1, 1, Qt::AlignCenter);
		}
	}


	receivePinsGroup = new QGroupBox(tr("Receive Pins"), this);
	receivePinsGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	receivePinsGroup->setLayout(grid);
	receivePinsGroup->setStyleSheet(set->getWidgetStyle());
	receivePinsGroup->setFont(QFont("Arial", 8));
}

void ExtCtrlWidget::createTransmitPinsGroup() {

	QLabel* emptyLabel = new QLabel(" ", this);
    emptyLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	emptyLabel->setStyleSheet(set->getLabelStyle());

	// band label
	QStringList bandNames;
	bandNames << "160 m" << "80 m" << "60 m" << "40 m" << "30 m" << "20 m" << "17 m" << "15 m" << "12 m" << "10 m" << "6 m";

	QList<QLabel *> bandLabelList;

	for (int i = 0; i < MAX_BANDS-1; i++) {

		QLabel *label = new QLabel(bandNames.at(i), this);
		label->setFrameStyle(QFrame::Box | QFrame::Raised);
		label->setStyleSheet(set->getLabelStyle());

		bandLabelList << label;
	}


	QString str = "%1";
	for (int i = 0; i < MAX_BANDS-1; i++) { // bands

		QList<AeroButton *> btnList;
		for (int j = 0; j < 7; j++) { // pins

			AeroButton* btn = new AeroButton(str.arg(j+1), this);
			btn->setRoundness(0);
			btn->setFixedSize (btn_width, btn_height);
			btn->setStyleSheet(set->getMiniButtonStyle());
			btnList.append(btn);

			CHECKED_CONNECT(
				btn,
				SIGNAL(clicked()), 
				this, 
				SLOT(transmitPinsBtnClicked()));
		}
		transmitPinsBtnMatrix.append(btnList);
	}

	QGridLayout* grid = new QGridLayout(this);
	grid->setVerticalSpacing(2);
	grid->setHorizontalSpacing(0);
	grid->setContentsMargins(5, 10, 20, 10);

	grid->addWidget(emptyLabel, 0, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(emptyLabel, 0, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(emptyLabel, 0, 2, 1, 7, Qt::AlignCenter);
	
	for (int i = 0; i < MAX_BANDS-1; i++) { // bands

		grid->addWidget(bandLabelList.at(i), i+1, 0, 1, 1, Qt::AlignLeft);
		grid->addWidget(emptyLabel, i+1, 1, 1, 1, Qt::AlignCenter);

		for (int j = 0; j < 7; j++) { // pins

			grid->addWidget(transmitPinsBtnMatrix.at(i).at(j), i+1, j+2, 1, 1, Qt::AlignCenter);
		}
	}

	transmitPinsGroup = new QGroupBox(tr("Transmit Pins"), this);
	transmitPinsGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	transmitPinsGroup->setLayout(grid);
	transmitPinsGroup->setStyleSheet(set->getWidgetStyle());
	transmitPinsGroup->setFont(QFont("Arial", 8));
}


//**************
void ExtCtrlWidget::enable() {

	if (enableBtn->btnState() == AeroButton::OFF) {

		enableBtn->setBtnState(AeroButton::ON);

		foreach(QList<AeroButton *> btnList, receivePinsBtnMatrix) {

			foreach(AeroButton *btn, btnList) {

				btn->setEnabled(true);
				btn->update();
			}
		}

		foreach(QList<AeroButton *> btnList, transmitPinsBtnMatrix) {

			foreach(AeroButton *btn, btnList) {

				btn->setEnabled(true);
				btn->update();
			}
		}

		m_pennyOCEnabled = true;
	}
	else {

		enableBtn->setBtnState(AeroButton::OFF);

		foreach(QList<AeroButton *> btnList, receivePinsBtnMatrix) {

			foreach(AeroButton *btn, btnList) {

				btn->setEnabled(false);
				btn->update();
			}
		}

		foreach(QList<AeroButton *> btnList, transmitPinsBtnMatrix) {

			foreach(AeroButton *btn, btnList) {

				btn->setEnabled(false);
				btn->update();
			}
		}
		m_pennyOCEnabled = false;
	}
	set->setPennyOCEnabled(this, m_pennyOCEnabled);
}

void ExtCtrlWidget::receivePinsBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());

	int band = -1;
	int pin = -1;
	for (int i = 0; i < MAX_BANDS-1; i++) { // bands

		pin = receivePinsBtnMatrix.at(i).indexOf(button);
		if (pin < 0)
			continue;
		else {

			band = i;
			break;
		}
	}

	if (button->btnState() == AeroButton::ON) {
			
		button->setBtnState(AeroButton::OFF);
		button->update();

		if (pin >= 0)
			m_rxPins[band] &= getMask(pin);
	}
	else {
		
		button->setBtnState(AeroButton::ON);
		button->update();

		if (pin >= 0) {

			m_rxPins[band] &= getMask(pin);
			m_rxPins[band] |= (1 << (pin+1));
		}
	}

	set->setRxJ6Pins(this, m_rxPins);
}

void ExtCtrlWidget::transmitPinsBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());

	int band = -1;
	int pin = -1;
	for (int i = 0; i < MAX_BANDS-1; i++) { // bands

		pin = transmitPinsBtnMatrix.at(i).indexOf(button);
		if (pin < 0)
			continue;
		else {

			band = i;
			break;
		}
	}

	if (button->btnState() == AeroButton::ON) {
			
		button->setBtnState(AeroButton::OFF);
		button->update();

		if (pin >= 0)
			m_txPins[band] &= getMask(pin);
	}
	else {
		
		button->setBtnState(AeroButton::ON);
		button->update();

		if (pin >= 0) {

			m_txPins[band] &= getMask(pin);//mask;
			m_txPins[band] |= (1 << (pin+1));
		}
	}
	
	set->setTxJ6Pins(this, m_txPins);
}

void ExtCtrlWidget::setValues() {

	m_pennyOCEnabled = set->getPennyOCEnabled();

	if (m_pennyOCEnabled) enable();

	m_rxPins = set->getRxJ6Pins();
	m_txPins = set->getTxJ6Pins();

	for (int i = 0; i < 11; i++) { // bands

		for (int j = 0; j < 7; j++) { // pins

			if (0x1 & (m_rxPins.at(i) >> (j+1)))
				receivePinsBtnMatrix.at(i).at(j)->setBtnState(AeroButton::ON);

			if (0x1 & (m_txPins.at(i) >> (j+1)))
				transmitPinsBtnMatrix.at(i).at(j)->setBtnState(AeroButton::ON);
		}
	}
}

uchar ExtCtrlWidget::getMask(int value) {

	uchar mask = 0x0;
	switch (value) {

		case 0:
			mask = 0xFD;
			break;

		case 1:
			mask = 0xFB;
			break;

		case 2:
			mask = 0xF7;
			break;

		case 3:
			mask = 0xEF;
			break;

		case 4:
			mask = 0xDF;
			break;

		case 5:
			mask = 0xBF;
			break;

		case 6:
			mask = 0x7F;
			break;

		default:
			mask = 0x0;
			break;
	}
	return mask;
}

