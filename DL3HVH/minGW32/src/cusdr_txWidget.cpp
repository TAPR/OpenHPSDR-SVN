/**
* @file  cusdr_txWidget.h
* @brief transmit control widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-06-16
*/

/*
 *   
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
 
#include <QtGui>
#include <QMenu>
#include <QFileDialog>
#include <QDebug>

#include "cusdr_txWidget.h"


#define	btn_height		15
#define	btn_width		74
#define	btn_width2		52
#define	btn_widths		40


TxWidget::TxWidget(QWidget *parent) 
	: QWidget(parent)
	, m_settings(Settings::instance())
	, m_minimumWidgetWidth(m_settings->getMinimumWidgetWidth())
	//, m_minimumGroupBoxWidth(m_settings->getMinimumGroupBoxWidth())
	, m_minimumGroupBoxWidth(0)
{
	setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 8, 4, 0);
	setMouseTracking(true);
	
	QFont titleFont;
	titleFont.setStyleStrategy(QFont::PreferQuality);
	titleFont.setPointSizeF(8);
	titleFont.setFamily("Arial");

	// create groups
	createSourceGroup();
	createTransmitFilterGroup();
	createPTTOptionsGroup();


	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(0);
	hbox1->setContentsMargins(4, 0, 4, 0);
	hbox1->addWidget(sourceGroup);

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(0);
	hbox2->setContentsMargins(4, 0, 4, 0);
	hbox2->addWidget(transmitFilterGroup);

	QHBoxLayout *hbox3 = new QHBoxLayout();
	hbox3->setSpacing(0);
	hbox3->setContentsMargins(4, 0, 4, 0);
	hbox3->addWidget(pttOptionsGroup);

	/*QHBoxLayout *hbox4 = new QHBoxLayout();
	hbox4->setSpacing(0);
	hbox4->setContentsMargins(4, 0, 4, 0);
	hbox4->addWidget(searchNetworkDeviceGroupBox);

	if (m_hwInterface == QSDR::NoInterfaceMode) {
		
		deviceNIGroupBox->hide();
		searchNetworkDeviceGroupBox->hide();
	}

	QHBoxLayout *hbox5 = new QHBoxLayout();
	hbox5->setSpacing(0);
	hbox5->setContentsMargins(4, 0, 4, 0);
	hbox5->addWidget(source10MhzExclusiveGroup);

	QHBoxLayout *hbox6 = new QHBoxLayout();
	hbox6->setSpacing(0);
	hbox6->setContentsMargins(4, 0, 4, 0);
	hbox6->addWidget(source122_88MhzExclusiveGroup);

	QHBoxLayout *hbox7 = new QHBoxLayout();
	hbox7->setSpacing(0);
	hbox7->setContentsMargins(4, 0, 4, 0);
	hbox7->addWidget(numberOfReceiversGroup());*/

	mainLayout->addLayout(hbox1);
	mainLayout->addLayout(hbox2);
	mainLayout->addLayout(hbox3);
	/*mainLayout->addLayout(hbox4);
	mainLayout->addLayout(hbox5);
	mainLayout->addLayout(hbox6);
	mainLayout->addLayout(hbox7);*/
	mainLayout->addStretch();
	setLayout(mainLayout);

	setupConnections();
}

TxWidget::~TxWidget() {

	disconnect(m_settings, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

void TxWidget::setupConnections() {

}

void TxWidget::createSourceGroup() {

	QLabel* sourceLabel = new QLabel("Source:", this);
    sourceLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	sourceLabel->setStyleSheet(m_settings->getLabelStyle());

	micInputBtn = new AeroButton("Mic Input", this);
	micInputBtn->setRoundness(0);
	micInputBtn->setFixedSize(btn_width, btn_height);
	micInputBtn->setBtnState(AeroButton::ON);

	CHECKED_CONNECT(
		micInputBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(inputButtonClicked()));

	lineInputBtn = new AeroButton("Line Input", this);
	lineInputBtn->setRoundness(0);
	lineInputBtn->setFixedSize(btn_width, btn_height);
	lineInputBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		lineInputBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(inputButtonClicked()));


	QLabel* maxLabel = new QLabel("Max Gain (dB):", this);
    maxLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	maxLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* minLabel = new QLabel("Min Gain (dB):", this);
    minLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	minLabel->setStyleSheet(m_settings->getLabelStyle());

	micGainMaxSpinBox = new QSpinBox(this);
	micGainMaxSpinBox->setMinimum(1);
	micGainMaxSpinBox->setMaximum(70);
	micGainMaxSpinBox->setStyleSheet(m_settings->getSpinBoxStyle());
	micGainMaxSpinBox->setValue(10);

	micGainMinSpinBox = new QSpinBox(this);
	micGainMinSpinBox->setMinimum(-96);
	micGainMinSpinBox->setMaximum(0);
	micGainMinSpinBox->setStyleSheet(m_settings->getSpinBoxStyle());
	micGainMinSpinBox->setValue(-40);

	QLabel* boostLabel = new QLabel("20 dB Mic Boost:", this);
    boostLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	boostLabel->setStyleSheet(m_settings->getLabelStyle());

	micBoostBtn = new AeroButton(" Off ", this);
	micBoostBtn->setRoundness(0);
	micBoostBtn->setFixedSize(btn_width, btn_height);
	micBoostBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		micBoostBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(boostButtonClicked()));

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addWidget(sourceLabel);
	hbox1->addStretch();
	hbox1->addWidget(micInputBtn);
	hbox1->addWidget(lineInputBtn);

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(4);
	hbox2->addWidget(boostLabel);
	hbox2->addStretch();
	hbox2->addWidget(micBoostBtn);

	QHBoxLayout *hbox3 = new QHBoxLayout();
	hbox3->setSpacing(4);
	hbox3->addWidget(maxLabel);
	hbox3->addStretch();
	hbox3->addWidget(micGainMaxSpinBox);

	QHBoxLayout *hbox4 = new QHBoxLayout();
	hbox4->setSpacing(4);
	hbox4->addWidget(minLabel);
	hbox4->addStretch();
	hbox4->addWidget(micGainMinSpinBox);
	
	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);
	vbox->addSpacing(12);
	vbox->addLayout(hbox3);
	vbox->addLayout(hbox4);
	
	sourceGroup = new QGroupBox(tr("Mic / Line Options"), this);
	sourceGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	sourceGroup->setLayout(vbox);
	sourceGroup->setStyleSheet(m_settings->getWidgetStyle());
	sourceGroup->setFont(QFont("Arial", 8));
}

void TxWidget::createTransmitFilterGroup() {

	QLabel* highLabel = new QLabel("High (Hz):", this);
    highLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	highLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* lowLabel = new QLabel("Low (Hz):", this);
    lowLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	lowLabel->setStyleSheet(m_settings->getLabelStyle());

	highFilterSpinBox = new QSpinBox(this);
	highFilterSpinBox->setMinimum(1000);
	highFilterSpinBox->setMaximum(5000);
	highFilterSpinBox->setStyleSheet(m_settings->getSpinBoxStyle());
	highFilterSpinBox->setValue(3100);

	lowFilterSpinBox = new QSpinBox(this);
	lowFilterSpinBox->setMinimum(0);
	lowFilterSpinBox->setMaximum(1000);
	lowFilterSpinBox->setStyleSheet(m_settings->getSpinBoxStyle());
	lowFilterSpinBox->setValue(200);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addWidget(highLabel);
	hbox1->addStretch();
	hbox1->addWidget(highFilterSpinBox);
	
	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(4);
	hbox2->addWidget(lowLabel);
	hbox2->addStretch();
	hbox2->addWidget(lowFilterSpinBox);
	
	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);
	
	transmitFilterGroup = new QGroupBox(tr("Transmit Filter"), this);
	transmitFilterGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	transmitFilterGroup->setLayout(vbox);
	transmitFilterGroup->setStyleSheet(m_settings->getWidgetStyle());
	transmitFilterGroup->setFont(QFont("Arial", 8));
}

void TxWidget::createPTTOptionsGroup() {

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	//hbox1->addWidget(penelopeBtn);
	//hbox1->addWidget(mercuryBtn);
	
	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	
	pttOptionsGroup = new QGroupBox(tr("PTT Options"), this);
	pttOptionsGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	pttOptionsGroup->setLayout(vbox);
	pttOptionsGroup->setStyleSheet(m_settings->getWidgetStyle());
	pttOptionsGroup->setFont(QFont("Arial", 8));
}


// ************************************************************************

void TxWidget::inputButtonClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	
	micInputBtn->setBtnState(AeroButton::OFF);
	micInputBtn->update();
	lineInputBtn->setBtnState(AeroButton::OFF);
	lineInputBtn->update();

	button->setBtnState(AeroButton::ON);
	button->update();
}

void TxWidget::boostButtonClicked() {

	if (micBoostBtn->btnState() == AeroButton::OFF) {

		micBoostBtn->setBtnState(AeroButton::ON);
		micBoostBtn->setText(" On ");
	}
	else {

		micBoostBtn->setBtnState(AeroButton::OFF);
		micBoostBtn->setText(" Off ");
	}
}

void TxWidget::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void TxWidget::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}

void TxWidget::enterEvent(QEvent *event) {

	Q_UNUSED(event)
}

void TxWidget::leaveEvent(QEvent *event) {

	Q_UNUSED(event)
}

void TxWidget::mouseMoveEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void TxWidget::mousePressEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void TxWidget::mouseReleaseEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}


