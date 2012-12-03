/**
* @file cusdr_networkWidget.cpp
* @brief Network settings widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-10-24
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

#define LOG_NETWORK_WIDGET

// use: NETWORK_WIDGET_DEBUG

//#include <QtGui>
//#include <QMenu>
//#include <QFileDialog>
//#include <QDebug>

#include "cusdr_networkWidget.h"


#define	btn_height		15
#define	btn_width		74
#define	btn_width2		52
#define	btn_widths		42


NetworkWidget::NetworkWidget(QWidget *parent)
	: QWidget(parent)
	, set(Settings::instance())
	, m_serverMode(set->getCurrentServerMode())
	, m_hwInterface(set->getHWInterface())
	, m_hwInterfaceTemp(set->getHWInterface())
	, m_dataEngineState(QSDR::DataEngineDown)
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(0)
	, m_numberOfReceivers(1)
	, m_hpsdrHardware(set->getHPSDRHardware())
{
	setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 8, 4, 0);
	setMouseTracking(true);
	
	m_deviceCards = set->getMetisCardsList();

	createDeviceNetworkInterfaceGroup();
	createDeviceSearchGroup();

	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(0);
	hbox1->setContentsMargins(4, 0, 4, 0);
	hbox1->addWidget(hpsdrInterfaceExclusiveBtnGroup());

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(0);
	hbox2->setContentsMargins(4, 0, 4, 0);
	hbox2->addWidget(deviceNIGroupBox);

	QHBoxLayout *hbox3 = new QHBoxLayout();
	hbox3->setSpacing(0);
	hbox3->setContentsMargins(4, 0, 4, 0);
	hbox3->addWidget(searchNetworkDeviceGroupBox);

	if (m_hwInterface == QSDR::NoInterfaceMode) {
		
		deviceNIGroupBox->hide();
		searchNetworkDeviceGroupBox->hide();
	}

	mainLayout->addLayout(hbox1);
	mainLayout->addLayout(hbox2);
	mainLayout->addLayout(hbox3);
	mainLayout->addStretch();
	setLayout(mainLayout);

	setupConnections();
	setSocketBufSize(this, set->getSocketBufferSize());
}

NetworkWidget::~NetworkWidget() {

	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

void NetworkWidget::setupConnections() {

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
		SIGNAL(newHPSDRDeviceNIC(QString, QString)), 
		this, 
		SLOT(addDeviceNICEntry(QString, QString)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(hpsdrDeviceNICChanged(int)), 
		this, 
		SLOT(setDeviceNIC(int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(metisCardListChanged(QList<TNetworkDevicecard>)), 
		this, 
		SLOT(setNetworkDeviceList(QList<TNetworkDevicecard>)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(hpsdrNetworkDeviceChanged(TNetworkDevicecard)),
		this, 
		SLOT(setCurrentNetworkDevice(TNetworkDevicecard)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(socketBufferSizeChanged(QObject*, int)), 
		this, 
		SLOT(setSocketBufSize(QObject*, int)));
}

void NetworkWidget::addNICChangedConnection() {

	CHECKED_CONNECT(
		networkDeviceInterfaces, 
		SIGNAL(currentIndexChanged(int)), 
		set, 
		SLOT(setHPSDRDeviceNIC(int)));
}

void NetworkWidget::setDeviceNIC(int index) {

	networkDeviceInterfaces->setCurrentIndex(index);
}

QGroupBox* NetworkWidget::hpsdrInterfaceExclusiveBtnGroup() {

	networkPresenceBtn = new AeroButton("Network", this);
	networkPresenceBtn->setRoundness(0);
	networkPresenceBtn->setFixedSize (btn_width, btn_height);

	CHECKED_CONNECT(
		networkPresenceBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(interfaceBtnClicked()));
	
	noHWBtn = new AeroButton("None", this);
	noHWBtn->setRoundness(0);
	noHWBtn->setFixedSize (btn_width, btn_height);
	
	CHECKED_CONNECT(
		noHWBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(interfaceBtnClicked()));

	hwInterfaceChanged();

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addWidget(noHWBtn);
	hbox1->addWidget(networkPresenceBtn);

	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	
	QGroupBox *groupBox = new QGroupBox(tr("Hardware Interface"), this);
	groupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	groupBox->setLayout(vbox);
	groupBox->setStyleSheet(set->getWidgetStyle());
	groupBox->setFont(QFont("Arial", 8));

	return groupBox;
}

void NetworkWidget::createDeviceNetworkInterfaceGroup() {

	networkDeviceInterfaces = new QComboBox();

	networkDeviceInterfaces->setStyleSheet(set->getComboBoxStyle());
	networkDeviceInterfaces->setMinimumContentsLength(22);
	
	socketBufSizeBtn = new AeroButton("Enable", this);
	socketBufSizeBtn->setRoundness(10);
	socketBufSizeBtn->setFixedSize(btn_widths, btn_height);
	socketBufSizeBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		socketBufSizeBtn,
		SIGNAL(clicked()),
		this,
		SLOT(socketBufSizeBtnClicked()));

	socketBufferSizes = new QComboBox();
	socketBufferSizes->setStyleSheet(set->getComboBoxStyle());
	socketBufferSizes->addItem("8 kB");
	socketBufferSizes->addItem("16 kB");
	socketBufferSizes->addItem("32 kB");
	socketBufferSizes->addItem("64 kB");
	socketBufferSizes->addItem("128 kB");
	socketBufferSizes->addItem("256 kB");
	socketBufferSizes->setEnabled(false);

	CHECKED_CONNECT(
		socketBufferSizes,
		SIGNAL(currentIndexChanged(int)),
		this,
		SLOT(setSocketBufferSize(int)));

	socketBufferSizeLabel = new QLabel("Socket Buffer Size:", this);
	socketBufferSizeLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	socketBufferSizeLabel->setStyleSheet(set->getLabelStyle());

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(1);
	hbox1->addStretch();
	hbox1->addWidget(networkDeviceInterfaces);
	
	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(1);
	//hbox2->addStretch();
	hbox2->addWidget(socketBufferSizeLabel);
	hbox2->addWidget(socketBufSizeBtn);
	hbox2->addSpacing(3);
	hbox2->addStretch();
	hbox2->addWidget(socketBufferSizes);

	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(3);
	vbox->addSpacing(5);
	vbox->addLayout(hbox1);
	vbox->addSpacing(5);
	vbox->addLayout(hbox2);
	
	deviceNIGroupBox = new QGroupBox(tr("Local network interface"), this);
	deviceNIGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	deviceNIGroupBox->setLayout(vbox);
	deviceNIGroupBox->setStyleSheet(set->getWidgetStyle());
	deviceNIGroupBox->setFont(QFont("Arial", 8));
}

void NetworkWidget::createDeviceSearchGroup() {

	searchNetworkDeviceBtn = new AeroButton("search", this);
	searchNetworkDeviceBtn->setRoundness(10);
	searchNetworkDeviceBtn->setFixedSize(btn_width2, btn_height);
	
	CHECKED_CONNECT(
		searchNetworkDeviceBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(searchHPSDRDeviceBtnClicked()));

	networkDeviceIPAdresses = new QComboBox();

	networkDeviceIPAdresses->setStyleSheet(set->getComboBoxStyle());
	networkDeviceIPAdresses->setMinimumContentsLength(22);
	
	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(1);
	hbox1->addStretch();
	hbox1->addWidget(searchNetworkDeviceBtn);
	hbox1->addSpacing(3);
	hbox1->addWidget(networkDeviceIPAdresses);

	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(3);
	vbox->addSpacing(5);
	vbox->addLayout(hbox1);
	vbox->addSpacing(5);

	searchNetworkDeviceGroupBox = new QGroupBox(tr("HPSDR device IP address"), this);
	searchNetworkDeviceGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	searchNetworkDeviceGroupBox->setLayout(vbox);
	searchNetworkDeviceGroupBox->setStyleSheet(set->getWidgetStyle());
	searchNetworkDeviceGroupBox->setFont(QFont("Arial", 8));
}

// ************************************************************************

void NetworkWidget::addDeviceNICEntry(QString niName, QString ipAddress) {

	//QString item = niName;
	QString item = ipAddress;
	//item.append(" (");
	//item.append(ipAddress);
	//item.append(")");
	networkDeviceInterfaces->addItem(item);
	//this->repaint();
}

void NetworkWidget::systemStateChanged(
	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	Q_UNUSED (sender)
	Q_UNUSED (err)

	if (m_hwInterface != hwmode) {
		
		m_hwInterface = hwmode;
		hwInterfaceChanged();
	}

	//m_oldServerMode = m_serverMode;
	if (m_serverMode != mode) {

		if (mode == QSDR::ChirpWSPR)
			disableButtons();

		if (m_serverMode == QSDR::ChirpWSPR)
			enableButtons();
		
		m_serverMode = mode;
	}
		
	if (m_dataEngineState != state) {

		if (state == QSDR::DataEngineUp)
			disableButtons();
		else
			enableButtons();

		m_dataEngineState = state;
	}

	//if (!change) return;
	update();
}

void NetworkWidget::hwInterfaceChanged() {

	switch (m_hwInterface) {

		case QSDR::NoInterfaceMode:

			noHWBtn->setBtnState(AeroButton::ON);
			networkPresenceBtn->setBtnState(AeroButton::OFF);
			break;

		case QSDR::Metis:
		case QSDR::Hermes:
			
			noHWBtn->setBtnState(AeroButton::OFF);
			networkPresenceBtn->setBtnState(AeroButton::ON);
			break;
	}

	m_hwInterfaceTemp = m_hwInterface;
}

void NetworkWidget::interfaceBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());

	if (button == networkPresenceBtn && m_hpsdrHardware == 0) { // HPSDR modules

		noHWBtn->setBtnState(AeroButton::OFF);
		noHWBtn->update();
		networkPresenceBtn->setBtnState(AeroButton::ON);

		if (m_hpsdrHardware == 0) {

			m_hwInterface = QSDR::Metis;
			NETWORK_WIDGET_DEBUG << "HW interface changed to Metis.";
		}
		else if (m_hpsdrHardware == 1) {

			m_hwInterface = QSDR::Hermes;
			NETWORK_WIDGET_DEBUG << "HW interface changed to Hermes.";
		}

		deviceNIGroupBox->show();
		searchNetworkDeviceGroupBox->show();
		source10MhzExclusiveGroup->show();
		source122_88MhzExclusiveGroup->show();
		socketBufferSizeGroupBox->show();
	}
	else
	if (button == noHWBtn) {

		networkPresenceBtn->setBtnState(AeroButton::OFF);
		networkPresenceBtn->update();
		noHWBtn->setBtnState(AeroButton::ON);
		
		m_hwInterface = QSDR::NoInterfaceMode;
		emit messageEvent("[hpsdr]: changed to no-interface mode.");
		deviceNIGroupBox->hide();
		searchNetworkDeviceGroupBox->hide();
		source10MhzExclusiveGroup->hide();
		source122_88MhzExclusiveGroup->hide();
		socketBufferSizeGroupBox->hide();
	}

	qDebug() << "HPSDRWidget::  setSystemState.";
	set->setSystemState(
					this,
					QSDR::NoError,
					m_hwInterface,
					m_serverMode,
					m_dataEngineState);
}

void NetworkWidget::searchHPSDRDeviceBtnClicked() {

	set->searchHpsdrNetworkDevices();
}

void NetworkWidget::socketBufSizeBtnClicked() {

	if (socketBufSizeBtn->btnState() == AeroButton::OFF) {

		socketBufferSizes->setEnabled(true);
		socketBufferSizes->blockSignals(true);
		setSocketBufSize(this, set->getSocketBufferSize());
		socketBufferSizes->blockSignals(false);
		//NETWORK_WIDGET_DEBUG << "getSocketBufferSize() :" << set->getSocketBufferSize();
		set->setManualSocketBufferSize(this, true);
		socketBufSizeBtn->setText("Disable");
		socketBufSizeBtn->setBtnState(AeroButton::ON);
	}
	else {

		socketBufferSizes->setEnabled(false);
		set->setManualSocketBufferSize(this, false);
		socketBufSizeBtn->setText("Enable");
		socketBufSizeBtn->setBtnState(AeroButton::OFF);
	}

	socketBufSizeBtn->update();
}

void NetworkWidget::disableButtons() {

	noHWBtn->setEnabled(false);
	networkPresenceBtn->setEnabled(false);
}

void NetworkWidget::enableButtons() {

	noHWBtn->setEnabled(true);
	networkPresenceBtn->setEnabled(true);
}

void NetworkWidget::setNetworkDeviceList(QList<TNetworkDevicecard> list) {

	m_deviceCards = list;

	if (list.length() == 0 ) {

		emit messageEvent("[hpsdr]: Metis card list empty.");
		return;
	}

	networkDeviceIPAdresses->clear();
	foreach (TNetworkDevicecard device, list) {

		networkDeviceIPAdresses->addItem(device.ip_address.toString());
		networkDeviceIPAdresses->update();
	}
}

void NetworkWidget::setCurrentNetworkDevice(TNetworkDevicecard card) {

	int i = 0;
	foreach (TNetworkDevicecard dev, m_deviceCards) {

		if (dev.ip_address == card.ip_address) networkDeviceIPAdresses->setCurrentIndex(i);
		i++;
	}	
}

void NetworkWidget::setSocketBufSize(QObject *sender, int size) {

	Q_UNUSED (sender)
	//if (sender == this) return;

	m_socketBufferSize = size;

	switch (m_socketBufferSize) {

		case 8:
			socketBufferSizes->setCurrentIndex(0);
			break;

		case 16:
			socketBufferSizes->setCurrentIndex(1);
			break;

		case 32:
			socketBufferSizes->setCurrentIndex(2);
			break;

		case 64:
			socketBufferSizes->setCurrentIndex(3);
			break;

		case 128:
			socketBufferSizes->setCurrentIndex(4);
			break;

		case 256:
			socketBufferSizes->setCurrentIndex(5);
			break;
	}
	
}

void NetworkWidget::setSocketBufferSize(int value) {

	switch (value) {

		case 0:
			set->setSocketBufferSize(this, 8);
			break;

		case 1:
			set->setSocketBufferSize(this, 16);
			break;

		case 2:
			set->setSocketBufferSize(this, 32);
			break;

		case 3:
			set->setSocketBufferSize(this, 64);
			break;

		case 4:
			set->setSocketBufferSize(this, 128);
			break;

		case 5:
			set->setSocketBufferSize(this, 256);
			break;
	}
}
