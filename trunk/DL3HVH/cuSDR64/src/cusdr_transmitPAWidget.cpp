/**
* @file  cusdr_transmitPAWidget.cpp
* @brief transmit PA control widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-08-28
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
 
//#include <QtGui>
//#include <QMenu>
//#include <QFileDialog>
//#include <QDebug>

#include "cusdr_transmitPAWidget.h"


#define	btn_height		15
#define	btn_width		74
#define	btn_width2		52
#define	btn_widths		40


TransmitPAWidget::TransmitPAWidget(QWidget *parent)
	: QWidget(parent)
	, set(Settings::instance())
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(0)
{
	setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 8, 4, 0);
	setMouseTracking(true);
	
	createGainGroup();
	//createTransmitFilterGroup();
	//createPTTOptionsGroup();


	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(0);
	hbox1->setContentsMargins(4, 0, 4, 0);
	hbox1->addWidget(gainGroup);

//	QHBoxLayout *hbox2 = new QHBoxLayout();
//	hbox2->setSpacing(0);
//	hbox2->setContentsMargins(4, 0, 4, 0);
//	hbox2->addWidget(transmitFilterGroup);
//
//	QHBoxLayout *hbox3 = new QHBoxLayout();
//	hbox3->setSpacing(0);
//	hbox3->setContentsMargins(4, 0, 4, 0);
//	hbox3->addWidget(pttOptionsGroup);

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
	//mainLayout->addLayout(hbox2);
	//mainLayout->addLayout(hbox3);
	/*mainLayout->addLayout(hbox4);
	mainLayout->addLayout(hbox5);
	mainLayout->addLayout(hbox6);
	mainLayout->addLayout(hbox7);*/
	mainLayout->addStretch();
	setLayout(mainLayout);

	setupConnections();
}

TransmitPAWidget::~TransmitPAWidget() {

	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

void TransmitPAWidget::setupConnections() {

}

void TransmitPAWidget::createGainGroup() {

//	QLabel *dBmLabel = new QLabel("dBm", this);
//	dBmLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
//	dBmLabel->setStyleSheet(set->getLabelStyle());

	// band label
	QStringList bandNames;
	bandNames << "160 m" << "80 m" << "60 m" << "40 m" << "30 m" << "20 m" << "17 m" << "15 m" << "12 m" << "10 m" << "6 m" << "gen";

	QList<QLabel *> bandLabelList;

	for (int i = 0; i < MAX_BANDS-1; i++) {

		QLabel *label = new QLabel(bandNames.at(i), this);
		label->setFrameStyle(QFrame::Box | QFrame::Raised);
		label->setStyleSheet(set->getLabelStyle());

		bandLabelList << label;
	}

	// gain spinBox list
	for (int i = 0; i < MAX_BANDS-1; i++) {

		QDoubleSpinBox *spinBox = new QDoubleSpinBox();
		spinBox->setStyleSheet(set->getComboBoxStyle());
		spinBox->setMinimum(38.8);
		spinBox->setMaximum(100.0);
		spinBox->setSingleStep(0.1);
		spinBox->setStyleSheet(set->getDoubleSpinBoxStyle());


		//CHECKED_CONNECT(comboBox, SIGNAL(currentIndexChanged(int)), this, SLOT(setAttenuatorValue(int)));

		gainSpinBoxList << spinBox;
	}

	QGridLayout* grid = new QGridLayout(this);
	grid->setVerticalSpacing(1);
	grid->setHorizontalSpacing(1);
	grid->setContentsMargins(7, 7, 5, 7);

	//grid->addWidget(attnLabel, 0, 0, 1, 2, Qt::AlignCenter);
	//grid->addWidget(emptyLabel1, 0, 2, 1, 1, Qt::AlignCenter);

	for (int i = 0; i < MAX_BANDS-1; i++) {

		grid->addWidget(bandLabelList.at(i), i+1, 0, 1, 1, Qt::AlignLeft);
		grid->addWidget(gainSpinBoxList.at(i), i+1, 1, 1, 1, Qt::AlignCenter);
		//grid->addWidget(dBmLabel, i+1, 3, 1, 1, Qt::AlignLeft);
	}


	gainGroup = new QGroupBox(tr("Gain by Band (dB)"), this);
	gainGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	gainGroup->setLayout(grid);
	gainGroup->setStyleSheet(set->getWidgetStyle());
	gainGroup->setFont(QFont("Arial", 8));
}


// ************************************************************************

void TransmitPAWidget::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void TransmitPAWidget::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}

void TransmitPAWidget::enterEvent(QEvent *event) {

	Q_UNUSED(event)
}

void TransmitPAWidget::leaveEvent(QEvent *event) {

	Q_UNUSED(event)
}

void TransmitPAWidget::mouseMoveEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void TransmitPAWidget::mousePressEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void TransmitPAWidget::mouseReleaseEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}


