/**
* @file cusdr_hpsdrWidget.cpp
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

#include "cusdr_hpsdrWidget.h"


#define	btn_height		15
#define	btn_width		74
#define	btn_width2		52
#define	btn_widths		40


HPSDRWidget::HPSDRWidget(QWidget *parent) 
	: QWidget(parent)
	, m_settings(Settings::instance())
	, m_minimumWidgetWidth(m_settings->getMinimumWidgetWidth())
	//, m_minimumGroupBoxWidth(m_settings->getMinimumGroupBoxWidth())
	, m_minimumGroupBoxWidth(0)
	, m_serverMode(m_settings->getCurrentServerMode())
	, m_hwInterface(m_settings->getHWInterface())
	, m_hwInterfaceTemp(m_settings->getHWInterface())
	, m_hpsdrHardware(m_settings->getHPSDRHardware())
	, m_dataEngineState(QSDR::DataEngineDown)
	, m_numberOfReceivers(1)
	//, m_socketBufferSize(16)
{
	setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 8, 4, 0);
	setMouseTracking(true);
	
	QFont titleFont;
	titleFont.setStyleStrategy(QFont::PreferQuality);
	titleFont.setPointSizeF(8);
	titleFont.setFamily("Arial");

	CHECKED_CONNECT(
		m_settings, 
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

	m_deviceCards = m_settings->getMetisCardsList();

	createDeviceNetworkInterfaceGroup();
	createDeviceSearchGroup();
	createsocketBufferSizeGroup();
	createSource10MhzExclusiveGroup();
	createSource122_88MhzExclusiveGroup();

	setSocketBufSize(this, m_settings->getSocketBufferSize());

	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(0);
	hbox1->setContentsMargins(4, 0, 4, 0);
	hbox1->addWidget(hpsdrHardwareBtnGroup());

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(0);
	hbox2->setContentsMargins(4, 0, 4, 0);
	hbox2->addWidget(hpsdrInterfaceExclusiveBtnGroup());

	QHBoxLayout *hbox3 = new QHBoxLayout();
	hbox3->setSpacing(0);
	hbox3->setContentsMargins(4, 0, 4, 0);
	hbox3->addWidget(deviceNIGroupBox);

	QHBoxLayout *hbox4 = new QHBoxLayout();
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
	hbox5->addWidget(socketBufferSizeGroupBox);

	QHBoxLayout *hbox6 = new QHBoxLayout();
	hbox6->setSpacing(0);
	hbox6->setContentsMargins(4, 0, 4, 0);
	hbox6->addWidget(source10MhzExclusiveGroup);

	QHBoxLayout *hbox7 = new QHBoxLayout();
	hbox7->setSpacing(0);
	hbox7->setContentsMargins(4, 0, 4, 0);
	hbox7->addWidget(source122_88MhzExclusiveGroup);

	QHBoxLayout *hbox8 = new QHBoxLayout();
	hbox8->setSpacing(0);
	hbox8->setContentsMargins(4, 0, 4, 0);
	hbox8->addWidget(numberOfReceiversGroup());

	mainLayout->addLayout(hbox1);
	mainLayout->addLayout(hbox2);
	mainLayout->addLayout(hbox3);
	mainLayout->addLayout(hbox4);
	mainLayout->addLayout(hbox5);
	mainLayout->addLayout(hbox6);
	mainLayout->addLayout(hbox7);
	mainLayout->addLayout(hbox8);
	mainLayout->addStretch();
	setLayout(mainLayout);

	setupConnections();
	setHPSDRHardware();
}

HPSDRWidget::~HPSDRWidget() {

	disconnect(m_settings, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

void HPSDRWidget::setupConnections() {

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(newHPSDRDeviceNIC(QString, QString)), 
		this, 
		SLOT(addDeviceNICEntry(QString, QString)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(hpsdrDeviceNICChanged(int)), 
		this, 
		SLOT(setDeviceNIC(int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(metisCardListChanged(QList<TNetworkDevicecard>)), 
		this, 
		SLOT(setNetworkDeviceList(QList<TNetworkDevicecard>)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(metisChanged(TNetworkDevicecard)), 
		this, 
		SLOT(setCurrentNetworkDevice(TNetworkDevicecard)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(socketBufferSizeChanged(QObject*, int)), 
		this, 
		SLOT(setSocketBufSize(QObject*, int)));

	/*CHECKED_CONNECT(
		m_settings, 
		SIGNAL(numberOfRXChanged(QObject *, int)), 
		this, 
		SLOT(numberOfReceiversChanged(QObject *, int)));*/
}

void HPSDRWidget::addNICChangedConnection() {

	CHECKED_CONNECT(
		networkDeviceInterfaces, 
		SIGNAL(currentIndexChanged(int)), 
		m_settings, 
		SLOT(setHPSDRDeviceNIC(int)));
}

void HPSDRWidget::setDeviceNIC(int index) {

	networkDeviceInterfaces->setCurrentIndex(index);
}

QGroupBox* HPSDRWidget::hpsdrInterfaceExclusiveBtnGroup() {

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
	groupBox->setStyleSheet(m_settings->getWidgetStyle());
	groupBox->setFont(QFont("Arial", 8));

	return groupBox;
}

QGroupBox* HPSDRWidget::hpsdrHardwareBtnGroup() {

	modulesPresenceBtn = new AeroButton("Modules", this);
	modulesPresenceBtn->setRoundness(0);
	modulesPresenceBtn->setFixedSize(btn_width, btn_height);
	modulesPresenceBtn->setBtnState(AeroButton::OFF);
	hardwareBtnList.append(modulesPresenceBtn);
	
	CHECKED_CONNECT(
		modulesPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(hpsdrHardwareChanged()));

	hermesPresenceBtn = new AeroButton("Hermes", this);
	hermesPresenceBtn->setRoundness(0);
	hermesPresenceBtn->setFixedSize(btn_width, btn_height);
	hermesPresenceBtn->setBtnState(AeroButton::OFF);
	hardwareBtnList.append(hermesPresenceBtn);
	
	CHECKED_CONNECT(
		hermesPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(hpsdrHardwareChanged()));

	penelopePresenceBtn = new AeroButton("Penelope", this);
	penelopePresenceBtn->setRoundness(0);
	penelopePresenceBtn->setFixedSize(btn_width, btn_height);
	penelopePresenceBtn->setBtnState(AeroButton::OFF);
	
	CHECKED_CONNECT(
		penelopePresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(penelopePresenceChanged()));

	pennyPresenceBtn = new AeroButton("PennyLane", this);
	pennyPresenceBtn->setRoundness(0);
	pennyPresenceBtn->setFixedSize(btn_width, btn_height);
	pennyPresenceBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		pennyPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(pennyPresenceChanged()));
	
	mercuryPresenceBtn = new AeroButton("Mercury", this);
	mercuryPresenceBtn->setRoundness(0);
	mercuryPresenceBtn->setFixedSize(btn_width, btn_height);
	mercuryPresenceBtn->setBtnState(AeroButton::OFF);
	
	CHECKED_CONNECT(
		mercuryPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(mercuryPresenceChanged()));

	alexPresenceBtn = new AeroButton("Alex", this);
	alexPresenceBtn->setRoundness(0);
	alexPresenceBtn->setFixedSize(btn_width, btn_height);
	alexPresenceBtn->setBtnState(AeroButton::OFF);
	
	CHECKED_CONNECT(
		alexPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(alexPresenceChanged()));
	
	excaliburPresenceBtn = new AeroButton("Excalibur", this);
	excaliburPresenceBtn->setRoundness(0);
	excaliburPresenceBtn->setFixedSize(btn_width, btn_height);
	excaliburPresenceBtn->setBtnState(AeroButton::OFF);
	
	CHECKED_CONNECT(
		excaliburPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(excaliburPresenceChanged()));
	
	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addWidget(modulesPresenceBtn);
	hbox1->addWidget(hermesPresenceBtn);
	
	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(4);
	hbox2->addStretch();
	hbox2->addWidget(penelopePresenceBtn);
	hbox2->addWidget(pennyPresenceBtn);
	
	
	QHBoxLayout *hbox3 = new QHBoxLayout();
	hbox3->setSpacing(4);
	hbox3->addStretch();
	hbox3->addWidget(mercuryPresenceBtn);
	hbox3->addWidget(excaliburPresenceBtn);

	QHBoxLayout *hbox4 = new QHBoxLayout();
	hbox4->setSpacing(4);
	hbox4->addStretch();
	hbox4->addWidget(alexPresenceBtn);
	
	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);
	vbox->addLayout(hbox3);
	vbox->addLayout(hbox4);
	
	QGroupBox *groupBox = new QGroupBox(tr("Hardware selection"), this);
	groupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	groupBox->setLayout(vbox);
	groupBox->setStyleSheet(m_settings->getWidgetStyle());
	groupBox->setFont(QFont("Arial", 8));

	return groupBox;
}

void HPSDRWidget::createSource10MhzExclusiveGroup() {

	atlasBtn = new AeroButton("Atlas", this);
	atlasBtn->setRoundness(0);
	atlasBtn->setFixedSize(btn_width, btn_height);
	source10MhzBtnList.append(atlasBtn);

	CHECKED_CONNECT(
		atlasBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(source10MhzChanged()));
	
	penelopeBtn = new AeroButton("Penelope", this);
	penelopeBtn->setRoundness(0);
	penelopeBtn->setFixedSize(btn_width, btn_height);
	source10MhzBtnList.append(penelopeBtn);

	CHECKED_CONNECT(
		penelopeBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(source10MhzChanged()));
	
	mercuryBtn = new AeroButton("Mercury", this);
	mercuryBtn->setRoundness(0);
	mercuryBtn->setFixedSize(btn_width, btn_height);
	source10MhzBtnList.append(mercuryBtn);

	CHECKED_CONNECT(
		mercuryBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(source10MhzChanged()));
	
	
	sources10Mhz << "Atlas" << "Penelope" << "Mercury";

	switch(m_settings->get10MHzSource()) {

		case 0:
			atlasBtn->setBtnState(AeroButton::ON);
			penelopeBtn->setBtnState(AeroButton::OFF);
			mercuryBtn->setBtnState(AeroButton::OFF);
			break;
			
		case 1:
			penelopeBtn->setBtnState(AeroButton::ON);
			atlasBtn->setBtnState(AeroButton::OFF);
			mercuryBtn->setBtnState(AeroButton::OFF);
			break;

		case 2:
			mercuryBtn->setBtnState(AeroButton::ON);
			atlasBtn->setBtnState(AeroButton::OFF);
			penelopeBtn->setBtnState(AeroButton::OFF);
			break;

		case 3:
			mercuryBtn->setBtnState(AeroButton::OFF);
			atlasBtn->setBtnState(AeroButton::OFF);
			penelopeBtn->setBtnState(AeroButton::OFF);
			break;
	}
	
	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addWidget(penelopeBtn);
	hbox1->addWidget(mercuryBtn);
	
	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(4);
	hbox2->addStretch();
	hbox2->addWidget(atlasBtn);
	
	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);
	
	source10MhzExclusiveGroup = new QGroupBox(tr("10 MHz Clock"), this);
	source10MhzExclusiveGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	source10MhzExclusiveGroup->setLayout(vbox);
	source10MhzExclusiveGroup->setStyleSheet(m_settings->getWidgetStyle());
	source10MhzExclusiveGroup->setFont(QFont("Arial", 8));
}

void HPSDRWidget::createSource122_88MhzExclusiveGroup() {

	penelope2Btn = new AeroButton("Penelope", this);
	penelope2Btn->setRoundness(0);
	penelope2Btn->setFixedSize(btn_width, btn_height);

	CHECKED_CONNECT(
		penelope2Btn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(source122_88MhzChanged()));

	mercury2Btn = new AeroButton("Mercury", this);
	mercury2Btn->setRoundness(0);
	mercury2Btn->setFixedSize(btn_width, btn_height);

	CHECKED_CONNECT(
		mercury2Btn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(source122_88MhzChanged()));

	switch(m_settings->get122_8MHzSource()) {

		case 0:
			penelope2Btn->setBtnState(AeroButton::ON);
			mercury2Btn->setBtnState(AeroButton::OFF);
			break;

		case 1:
			mercury2Btn->setBtnState(AeroButton::ON);
			penelope2Btn->setBtnState(AeroButton::OFF);
			break;
	}
	
	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addWidget(penelope2Btn);
	hbox1->addWidget(mercury2Btn);
	
	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	
	source122_88MhzExclusiveGroup = new QGroupBox(tr("122.8 MHz Clock"), this);
	source122_88MhzExclusiveGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	source122_88MhzExclusiveGroup->setLayout(vbox);
	source122_88MhzExclusiveGroup->setStyleSheet(m_settings->getWidgetStyle());
	source122_88MhzExclusiveGroup->setFont(QFont("Arial", 8));
}

QGroupBox *HPSDRWidget::numberOfReceiversGroup() {

	rx1Btn = new AeroButton("1", this);
	rx1Btn->setRoundness(10);
	rx1Btn->setFixedSize(24, btn_height);
	rx1Btn->setBtnState(AeroButton::ON);
	rxBtnList.append(rx1Btn);
	
	CHECKED_CONNECT(
		rx1Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setNumberOfReceivers()));

	rx2Btn = new AeroButton("2", this);
	rx2Btn->setRoundness(10);
	rx2Btn->setFixedSize(24, btn_height);
	rx2Btn->setBtnState(AeroButton::OFF);
	rxBtnList.append(rx2Btn);

	CHECKED_CONNECT(
		rx2Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setNumberOfReceivers()));

	rx3Btn = new AeroButton("3", this);
	rx3Btn->setRoundness(10);
	rx3Btn->setFixedSize(24, btn_height);
	rx3Btn->setBtnState(AeroButton::OFF);
	rxBtnList.append(rx3Btn);

	CHECKED_CONNECT(
		rx3Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setNumberOfReceivers()));

	rx4Btn = new AeroButton("4", this);
	rx4Btn->setRoundness(10);
	rx4Btn->setFixedSize(24, btn_height);
	rx4Btn->setBtnState(AeroButton::OFF);
	rxBtnList.append(rx4Btn);

	CHECKED_CONNECT(
		rx4Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setNumberOfReceivers()));

	rx1to2Btn = new AeroButton("Link 1+2", this);
	rx1to2Btn->setRoundness(10);
	rx1to2Btn->setFixedSize(btn_width2, btn_height);
	rx1to2Btn->setBtnState(AeroButton::OFF);
	rx1to2Btn->setEnabled(false);
	//rxBtnList.append(rx1Btn);
	
	CHECKED_CONNECT(
		rx1to2Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setShow1on2()));

	view1Btn = new AeroButton("View 1", this);
	view1Btn->setRoundness(10);
	view1Btn->setFixedSize(btn_width2, btn_height);
	view1Btn->setBtnState(AeroButton::OFF);
	view1Btn->setEnabled(false);
	viewBtnList.append(view1Btn);

	CHECKED_CONNECT(
		view1Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setView()));

	view2Btn = new AeroButton("View 2", this);
	view2Btn->setRoundness(10);
	view2Btn->setFixedSize(btn_width2, btn_height);
	view2Btn->setBtnState(AeroButton::OFF);
	view2Btn->setEnabled(false);
	viewBtnList.append(view2Btn);

	CHECKED_CONNECT(
		view2Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setView()));

	/*view3Btn = new AeroButton("View 3", this);
	view3Btn->setRoundness(10);
	view3Btn->setFixedSize(btn_width2, btn_height);
	view3Btn->setBtnState(AeroButton::OFF);
	view3Btn->setEnabled(false);
	viewBtnList.append(view3Btn);

	CHECKED_CONNECT(
		view3Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setView()));*/


	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(5);
	hbox1->addStretch();
	hbox1->addWidget(rx1Btn);
	hbox1->addWidget(rx2Btn);
	hbox1->addWidget(rx3Btn);
	hbox1->addWidget(rx4Btn);
	hbox1->addSpacing(6);
	hbox1->addWidget(rx1to2Btn);

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(6);
	hbox2->addStretch();
	hbox2->addWidget(view1Btn);
	hbox2->addWidget(view2Btn);
	//hbox2->addWidget(view3Btn);
	hbox2->addSpacing(63);
	
	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);
	
	QGroupBox *groupBox = new QGroupBox(tr("Number of Receivers"), this);
	groupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	groupBox->setLayout(vbox);
	groupBox->setStyleSheet(m_settings->getWidgetStyle());
	groupBox->setFont(QFont("Arial", 8));

	return groupBox;
}

void HPSDRWidget::createDeviceNetworkInterfaceGroup() {

	networkDeviceInterfaces = new QComboBox();

	networkDeviceInterfaces->setStyleSheet(m_settings->getComboBoxStyle());
	networkDeviceInterfaces->setMinimumContentsLength(22);
	
	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(1);
	hbox1->addStretch();
	hbox1->addWidget(networkDeviceInterfaces);
	
	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(3);
	vbox->addSpacing(5);
	vbox->addLayout(hbox1);
	vbox->addSpacing(5);
	
	deviceNIGroupBox = new QGroupBox(tr("Local network interface"), this);
	deviceNIGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	deviceNIGroupBox->setLayout(vbox);
	deviceNIGroupBox->setStyleSheet(m_settings->getWidgetStyle());
	deviceNIGroupBox->setFont(QFont("Arial", 8));
}

void HPSDRWidget::createDeviceSearchGroup() {

	searchNetworkDeviceBtn = new AeroButton("search", this);
	searchNetworkDeviceBtn->setRoundness(10);
	searchNetworkDeviceBtn->setFixedSize(btn_width2, btn_height);
	
	CHECKED_CONNECT(
		searchNetworkDeviceBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(searchHPSDRDeviceBtnClicked()));

	networkDeviceIPAdresses = new QComboBox();

	networkDeviceIPAdresses->setStyleSheet(m_settings->getComboBoxStyle());
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
	searchNetworkDeviceGroupBox->setStyleSheet(m_settings->getWidgetStyle());
	searchNetworkDeviceGroupBox->setFont(QFont("Arial", 8));
}

void HPSDRWidget::createsocketBufferSizeGroup() {

	socketBufSizeBtn = new AeroButton("Enable", this);
	socketBufSizeBtn->setRoundness(10);
	socketBufSizeBtn->setFixedSize(btn_width2, btn_height);
	socketBufSizeBtn->setBtnState(AeroButton::OFF);
	
	CHECKED_CONNECT(
		socketBufSizeBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(socketBufSizeBtnClicked()));

	socketBufferSizes = new QComboBox();

	socketBufferSizes->setStyleSheet(m_settings->getComboBoxStyle());
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
	 

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(1);
	//hbox1->addStretch();
	hbox1->addWidget(socketBufSizeBtn);
	hbox1->addSpacing(3);
	hbox1->addStretch();
	hbox1->addWidget(socketBufferSizes);

	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(3);
	vbox->addSpacing(5);
	vbox->addLayout(hbox1);
	vbox->addSpacing(5);

	socketBufferSizeGroupBox = new QGroupBox(tr("Socket Buffer Size"), this);
	socketBufferSizeGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	socketBufferSizeGroupBox->setLayout(vbox);
	socketBufferSizeGroupBox->setStyleSheet(m_settings->getWidgetStyle());
	socketBufferSizeGroupBox->setFont(QFont("Arial", 8));
}


// ************************************************************************

void HPSDRWidget::setHPSDRHardware() {

	switch (m_hpsdrHardware) {

		case 0:
			// HPSDR modules
			modulesPresenceBtn->setBtnState(AeroButton::ON);
			hermesPresenceBtn->setBtnState(AeroButton::OFF);

			mercuryPresenceBtn->setEnabled(true);
			if (m_settings->getMercuryPresence())
				mercuryPresenceBtn->setBtnState(AeroButton::ON);
			else
				mercuryPresenceBtn->setBtnState(AeroButton::OFF);

			penelopePresenceBtn->setEnabled(true);
			if (m_settings->getPenelopePresence())
				penelopePresenceBtn->setBtnState(AeroButton::ON);
			else
				penelopePresenceBtn->setBtnState(AeroButton::OFF);

			excaliburPresenceBtn->setEnabled(true);
			if (m_settings->getExcaliburPresence()) {

				m_settings->set10MhzSource(this, 3); // none
				mercuryBtn->setBtnState(AeroButton::OFF);
				atlasBtn->setBtnState(AeroButton::OFF);
				penelopeBtn->setBtnState(AeroButton::OFF);
				mercuryBtn->setEnabled(false);
				penelopeBtn->setEnabled(false);
				atlasBtn->setEnabled(false);

				excaliburPresenceBtn->setBtnState(AeroButton::ON);
			}
			else {

				switch(m_settings->get10MHzSource()) {

					case 0:
						atlasBtn->setBtnState(AeroButton::ON);
						penelopeBtn->setBtnState(AeroButton::OFF);
						mercuryBtn->setBtnState(AeroButton::OFF);
						break;
			
					case 1:
						penelopeBtn->setBtnState(AeroButton::ON);
						atlasBtn->setBtnState(AeroButton::OFF);
						mercuryBtn->setBtnState(AeroButton::OFF);
						break;

					case 2:
						mercuryBtn->setBtnState(AeroButton::ON);
						atlasBtn->setBtnState(AeroButton::OFF);
						penelopeBtn->setBtnState(AeroButton::OFF);
						break;

					case 3:
						mercuryBtn->setBtnState(AeroButton::OFF);
						atlasBtn->setBtnState(AeroButton::OFF);
						penelopeBtn->setBtnState(AeroButton::OFF);
						break;
				}

				excaliburPresenceBtn->setBtnState(AeroButton::OFF);
			}

			break;

		case 1:
			
			// Hermes
			modulesPresenceBtn->setBtnState(AeroButton::OFF);
			hermesPresenceBtn->setBtnState(AeroButton::ON);
		
			penelopePresenceBtn->setBtnState(AeroButton::OFF);
			penelopePresenceBtn->setEnabled(false);

			mercuryPresenceBtn->setBtnState(AeroButton::OFF);
			mercuryPresenceBtn->setEnabled(false);

			excaliburPresenceBtn->setBtnState(AeroButton::OFF);
			excaliburPresenceBtn->setEnabled(false);

			deviceNIGroupBox->show();
			searchNetworkDeviceGroupBox->show();

			m_settings->set10MhzSource(this, 2); // none
			source10MhzExclusiveGroup->hide();
			source122_88MhzExclusiveGroup->hide();

			break;

		case 2:

			// Cyclops
			break;
	}

	if (m_settings->getAlexPresence())
		alexPresenceBtn->setBtnState(AeroButton::ON);
}

void HPSDRWidget::addDeviceNICEntry(QString niName, QString ipAddress) {

	//QString item = niName;
	QString item = ipAddress;
	//item.append(" (");
	//item.append(ipAddress);
	//item.append(")");
	networkDeviceInterfaces->addItem(item);
	//this->repaint();
}

void HPSDRWidget::systemStateChanged(
	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	Q_UNUSED (sender)
	Q_UNUSED (err)

	//bool change = false;

	if (m_hwInterface != hwmode) {
		
		m_hwInterface = hwmode;
		hwInterfaceChanged();
		//change = true;
	}

	if (m_serverMode != mode) {
		
		m_serverMode = mode;

		if (m_serverMode == QSDR::ChirpWSPR) {

			numberOfReceiversChanged(0);
			disableButtons();
		}
		else
			enableButtons();
		
		//change = true;
	}
		
	if (m_dataEngineState != state) {

		m_dataEngineState = state;

		if (m_dataEngineState == QSDR::DataEngineUp)
			disableButtons();
		else if (m_serverMode == QSDR::DttSP || m_serverMode == QSDR::QtDSP)
			enableButtons();

		//change = true;
	}

	//if (!change) return;
	update();
}

void HPSDRWidget::setConnected(QObject *sender, bool value) {

	Q_UNUSED (sender)

	if (value) {

		networkPresenceBtn->setEnabled(false);
		penelopePresenceBtn->setEnabled(false);
		mercuryPresenceBtn->setEnabled(false);
		alexPresenceBtn->setEnabled(false);
		excaliburPresenceBtn->setEnabled(false);
		atlasBtn->setEnabled(false);
		penelopeBtn->setEnabled(false);
		mercuryBtn->setEnabled(false);
		penelope2Btn->setEnabled(false);
		mercury2Btn->setEnabled(false);
	}
	else {

		networkPresenceBtn->setEnabled(true);
		penelopePresenceBtn->setEnabled(true);
		mercuryPresenceBtn->setEnabled(true);
		alexPresenceBtn->setEnabled(true);
		excaliburPresenceBtn->setEnabled(true);
		atlasBtn->setEnabled(true);
		penelopeBtn->setEnabled(true);
		mercuryBtn->setEnabled(true);
		penelope2Btn->setEnabled(true);
		mercury2Btn->setEnabled(true);
	}
}

void HPSDRWidget::setClientConnected(QObject *sender, bool value) {

	Q_UNUSED (sender)
	Q_UNUSED (value)

	//if (m_settings->dspMode() == 0) {
	//	if (value) {

	//		samplerate48Btn->setEnabled(false);
	//		samplerate96Btn->setEnabled(false);
	//		samplerate192Btn->setEnabled(false);
	//	}
	//	else {

	//		samplerate48Btn->setEnabled(true);
	//		samplerate96Btn->setEnabled(true);
	//		samplerate192Btn->setEnabled(true);
	//	}
	//}
}

void HPSDRWidget::hwInterfaceChanged() {

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

void HPSDRWidget::interfaceBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());

	if (button == networkPresenceBtn && m_hpsdrHardware == 0) {

		noHWBtn->setBtnState(AeroButton::OFF);
		noHWBtn->update();
		networkPresenceBtn->setBtnState(AeroButton::ON);

		m_hwInterface = QSDR::Metis;
		emit messageEvent("[hpsdr]: HW interface changed to network.");
		deviceNIGroupBox->show();
		searchNetworkDeviceGroupBox->show();
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
	}

	m_settings->setSystemState(
					this,
					QSDR::NoError,
					m_hwInterface,
					m_serverMode,
					m_dataEngineState);
}

void HPSDRWidget::searchHPSDRDeviceBtnClicked() {

	m_settings->searchHpsdrNetworkDevices();
}

void HPSDRWidget::socketBufSizeBtnClicked() {

	if (socketBufSizeBtn->btnState() == AeroButton::OFF) {

		socketBufferSizes->setEnabled(true);
		setSocketBufSize(this, m_settings->getSocketBufferSize());
		m_settings->setManualSocketBufferSize(this, true);
		socketBufSizeBtn->setText("Disable");
		socketBufSizeBtn->setBtnState(AeroButton::ON);
	}
	else {

		socketBufferSizes->setEnabled(false);
		m_settings->setManualSocketBufferSize(this, false);
		socketBufSizeBtn->setText("Enable");
		socketBufSizeBtn->setBtnState(AeroButton::OFF);
	}

	socketBufSizeBtn->update();
}

void HPSDRWidget::hpsdrHardwareChanged() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btn = hardwareBtnList.indexOf(button);

	foreach(AeroButton *btn, hardwareBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	m_hpsdrHardware = btn;
	m_settings->setHPSDRHardware(m_hpsdrHardware);

	setHPSDRHardware();

	switch (m_hpsdrHardware) {

		case 0:
			// HPSDR modules
			source10MhzExclusiveGroup->show();
			source122_88MhzExclusiveGroup->show();

			m_hwInterface = QSDR::Metis;
			noHWBtn->setEnabled(true);

			emit messageEvent("[hpsdr]: HPSDR modules chosen.");
			break;

		case 1:
			// Hermes
			source10MhzExclusiveGroup->hide();
			source122_88MhzExclusiveGroup->hide();

			m_hwInterface = QSDR::Hermes;

			noHWBtn->setBtnState(AeroButton::OFF);
			networkPresenceBtn->setBtnState(AeroButton::ON);
			noHWBtn->setEnabled(false);

			emit messageEvent("[hpsdr]: Hermes chosen.");
			break;

		case 2:
			break;
	}

	m_settings->setSystemState(
					this,
					QSDR::NoError,
					m_hwInterface,
					m_serverMode,
					m_dataEngineState);
}

void HPSDRWidget::penelopePresenceChanged() {

	if (penelopePresenceBtn->btnState() == AeroButton::OFF) {
		
		if (pennyPresenceBtn->btnState() == AeroButton::ON) {

			m_settings->setPennyLanePresence(false);
			pennyPresenceBtn->setBtnState(AeroButton::OFF);
			pennyPresenceBtn->update();

			emit messageEvent("[hpsdr]: PennyLane removed");
		}
		m_settings->setPenelopePresence(true);
		penelopePresenceBtn->setBtnState(AeroButton::ON);

		emit messageEvent("[hpsdr]: Penelope added");

	} else {

		m_settings->setPenelopePresence(false);
		penelopePresenceBtn->setBtnState(AeroButton::OFF);
		emit messageEvent("[hpsdr]: Penelope removed.");
	}
}

void HPSDRWidget::pennyPresenceChanged() {

	if (pennyPresenceBtn->btnState() == AeroButton::OFF) {
		
		if (penelopePresenceBtn->btnState() == AeroButton::ON) {

			m_settings->setPenelopePresence(false);
			penelopePresenceBtn->setBtnState(AeroButton::OFF);
			penelopePresenceBtn->update();

			emit messageEvent("[hpsdr]: Penelope removed");
		}
		m_settings->setPennyLanePresence(true);
		pennyPresenceBtn->setBtnState(AeroButton::ON);

		emit messageEvent("[hpsdr]: PennyLane added");

	} else {

		m_settings->setPennyLanePresence(false);
		pennyPresenceBtn->setBtnState(AeroButton::OFF);
		emit messageEvent("[hpsdr]: PennyLane removed.");
	}
}

void HPSDRWidget::mercuryPresenceChanged() {

	if (mercuryPresenceBtn->btnState() == AeroButton::OFF) {
		
		m_settings->setMercuryPresence(true);
		mercuryPresenceBtn->setBtnState(AeroButton::ON);
		emit messageEvent("[hpsdr]: Mercury added.");

	} else {

		m_settings->setMercuryPresence(false);
		mercuryPresenceBtn->setBtnState(AeroButton::OFF);
		emit messageEvent("[hpsdr]: Mercury removed.");
	}
}

void HPSDRWidget::alexPresenceChanged() {

	if (alexPresenceBtn->btnState() == AeroButton::OFF) {
		
		m_settings->setAlexPresence(true);
		alexPresenceBtn->setBtnState(AeroButton::ON);
		emit messageEvent("[hpsdr]: Alex added.");

	} else {

		m_settings->setAlexPresence(false);
		alexPresenceBtn->setBtnState(AeroButton::OFF);
		emit messageEvent("[hpsdr]: Alex removed.");
	}
}

void HPSDRWidget::excaliburPresenceChanged() {

	if (excaliburPresenceBtn->btnState() == AeroButton::OFF) {
		
		m_settings->set10MhzSource(this, 3); // None
		
		mercuryBtn->setBtnState(AeroButton::OFF);
		atlasBtn->setBtnState(AeroButton::OFF);
		penelopeBtn->setBtnState(AeroButton::OFF);
		
		mercuryBtn->setEnabled(false);
		penelopeBtn->setEnabled(false);
		atlasBtn->setEnabled(false);

		m_settings->setExcaliburPresence(true);
		excaliburPresenceBtn->setBtnState(AeroButton::ON);
		emit messageEvent("[hpsdr]: Excalibur added.");

	} else {

		m_settings->set10MhzSource(this, 2); // Mercury
		
		mercuryBtn->setEnabled(true);
		penelopeBtn->setEnabled(true);
		atlasBtn->setEnabled(true);

		mercuryBtn->setBtnState(AeroButton::ON);
		mercuryBtn->update();
		atlasBtn->setBtnState(AeroButton::OFF);
		penelopeBtn->setBtnState(AeroButton::OFF);

		m_settings->setExcaliburPresence(false);
		excaliburPresenceBtn->setBtnState(AeroButton::OFF);
		emit messageEvent("[hpsdr]: Excalibur removed.");
	}
}

void HPSDRWidget::source10MhzChanged() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btn = source10MhzBtnList.indexOf(button);
	
	foreach(AeroButton *btn, source10MhzBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	m_settings->set10MhzSource(this, btn);
	button->setBtnState(AeroButton::ON);
	button->update();

	QString msg = "[hpsdr]: 10 MHz source changed to %1";
	emit messageEvent(msg.arg(sources10Mhz.at(btn)));
}

void HPSDRWidget::source122_88MhzChanged() {
	
	switch (m_settings->get122_8MHzSource()) {

		// penelope 0, mercury 1
		case 0:
			penelope2Btn->setBtnState(AeroButton::OFF);
			penelope2Btn->update();

			m_settings->set122_88MhzSource(this, 1);
			emit messageEvent("[hpsdr]: 122.88 MHz source changed to Mercury.");
			mercury2Btn->setBtnState(AeroButton::ON);
			break;
			
		case 1:
			mercury2Btn->setBtnState(AeroButton::OFF);
			mercury2Btn->update();

			m_settings->set122_88MhzSource(this, 0);
			emit messageEvent("[hpsdr]: 122.88 MHz source changed to Penelope.");
			penelope2Btn->setBtnState(AeroButton::ON);
			break;
	}
}

void HPSDRWidget::setNumberOfReceivers() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int rx = rxBtnList.indexOf(button);

	foreach (AeroButton *btn, rxBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	m_numberOfReceivers = rx + 1;

	foreach (AeroButton *btn, viewBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	if (m_numberOfReceivers == 1) {

		view1Btn->setEnabled(false);
		view2Btn->setEnabled(false);
		//view3Btn->setEnabled(false);

		rx1to2Btn->setEnabled(false);
	}
	else {
		
		m_settings->setMultiRxView(0);
		view1Btn->setBtnState(AeroButton::ON);
		view1Btn->update();

		view1Btn->setEnabled(true);
		view2Btn->setEnabled(true);

		if (m_numberOfReceivers == 2)
			rx1to2Btn->setEnabled(true);
		else {

			m_settings->setCoupledReceivers(this, 0);
			rx1to2Btn->setBtnState(AeroButton::OFF);
			rx1to2Btn->setEnabled(true);
		}
	}

	m_settings->setReceivers(this, m_numberOfReceivers);
}

void HPSDRWidget::setShow1on2() {

	if (rx1to2Btn->btnState() == AeroButton::OFF) {

		m_settings->setCoupledReceivers(this, 12);
		rx1to2Btn->setBtnState(AeroButton::ON);
	}
	else {

		m_settings->setCoupledReceivers(this, 0);
		rx1to2Btn->setBtnState(AeroButton::OFF);
	}

	rx1to2Btn->update();
}

void HPSDRWidget::numberOfReceiversChanged(int value) {

	foreach (AeroButton *btn, rxBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	rxBtnList.at(value)->setBtnState(AeroButton::ON);
	rxBtnList.at(value)->update();

	m_numberOfReceivers = value + 1;

	foreach (AeroButton *btn, viewBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	if (m_numberOfReceivers == 1) {

		view1Btn->setEnabled(false);
		view2Btn->setEnabled(false);
	}
	else {
		
		m_settings->setMultiRxView(0);
		view1Btn->setBtnState(AeroButton::ON);
		view1Btn->update();

		view1Btn->setEnabled(true);
		view2Btn->setEnabled(true);
	}
}

void HPSDRWidget::disableButtons() {

	disconnect(
		networkPresenceBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(interfaceBtnClicked()));

	disconnect(
		noHWBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(interfaceBtnClicked()));

	disconnect(
		modulesPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(hpsdrHardwareChanged()));

	disconnect(
		hermesPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(hpsdrHardwareChanged()));

	disconnect(
		penelopePresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(penelopePresenceChanged()));

	disconnect(
		mercuryPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(mercuryPresenceChanged()));

	disconnect(
		alexPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(alexPresenceChanged()));

	disconnect(
		excaliburPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(excaliburPresenceChanged()));

	disconnect(
		atlasBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(source10MhzChanged()));

	disconnect(
		penelopeBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(source10MhzChanged()));

	disconnect(
		mercuryBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(source10MhzChanged()));

	disconnect(
		penelope2Btn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(source122_88MhzChanged()));

	disconnect(
		mercury2Btn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(source122_88MhzChanged()));

	disconnect(
		rx1Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setNumberOfReceivers()));

	disconnect(
		rx2Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setNumberOfReceivers()));

	disconnect(
		rx3Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setNumberOfReceivers()));

	disconnect(
		rx4Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setNumberOfReceivers()));

	disconnect(
		view1Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setView()));

	disconnect(
		view2Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setView()));

	disconnect(
		searchNetworkDeviceBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(searchHPSDRDeviceBtnClicked()));
}

void HPSDRWidget::enableButtons() {

	CHECKED_CONNECT(
		networkPresenceBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(interfaceBtnClicked()));

	CHECKED_CONNECT(
		noHWBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(interfaceBtnClicked()));

	CHECKED_CONNECT(
		modulesPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(hpsdrHardwareChanged()));

	CHECKED_CONNECT(
		hermesPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(hpsdrHardwareChanged()));

	CHECKED_CONNECT(
		penelopePresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(penelopePresenceChanged()));

	CHECKED_CONNECT(
		mercuryPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(mercuryPresenceChanged()));

	CHECKED_CONNECT(
		alexPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(alexPresenceChanged()));

	CHECKED_CONNECT(
		excaliburPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(excaliburPresenceChanged()));

	CHECKED_CONNECT(
		atlasBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(source10MhzChanged()));

	CHECKED_CONNECT(
		penelopeBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(source10MhzChanged()));

	CHECKED_CONNECT(
		mercuryBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(source10MhzChanged()));

	CHECKED_CONNECT(
		penelope2Btn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(source122_88MhzChanged()));

	CHECKED_CONNECT(
		mercury2Btn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(source122_88MhzChanged()));

	CHECKED_CONNECT(
		rx1Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setNumberOfReceivers()));

	CHECKED_CONNECT(
		rx2Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setNumberOfReceivers()));

	CHECKED_CONNECT(
		rx3Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setNumberOfReceivers()));

	CHECKED_CONNECT(
		rx4Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setNumberOfReceivers()));

	CHECKED_CONNECT(
		view1Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setView()));

	CHECKED_CONNECT(
		view2Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setView()));

	CHECKED_CONNECT(
		searchNetworkDeviceBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(searchHPSDRDeviceBtnClicked()));
}

void HPSDRWidget::setView() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int view = viewBtnList.indexOf(button);

	foreach (AeroButton *btn, viewBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	m_settings->setMultiRxView(view);
	button->setBtnState(AeroButton::ON);
	button->update();
}

void HPSDRWidget::setNetworkDeviceList(QList<TNetworkDevicecard> list) {

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

void HPSDRWidget::setCurrentNetworkDevice(TNetworkDevicecard card) {

	int i = 0;
	foreach (TNetworkDevicecard dev, m_deviceCards) {

		if (dev.ip_address == card.ip_address) networkDeviceIPAdresses->setCurrentIndex(i);
		i++;
	}	
}

void HPSDRWidget::setSocketBufSize(QObject *sender, int size) {

	//Q_UNUSED (sender)
	if (sender == this) return;

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

void HPSDRWidget::setSocketBufferSize(int value) {

	switch (value) {

		case 0:
			m_settings->setSocketBufferSize(this, 8);
			break;

		case 1:
			m_settings->setSocketBufferSize(this, 16);
			break;

		case 2:
			m_settings->setSocketBufferSize(this, 32);
			break;

		case 3:
			m_settings->setSocketBufferSize(this, 64);
			break;

		case 4:
			m_settings->setSocketBufferSize(this, 128);
			break;

		case 5:
			m_settings->setSocketBufferSize(this, 256);
			break;
	}
}

// **********************

void HPSDRWidget::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void HPSDRWidget::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}

void HPSDRWidget::enterEvent(QEvent *event) {

	Q_UNUSED(event)
}

void HPSDRWidget::leaveEvent(QEvent *event) {

	Q_UNUSED(event)
}

void HPSDRWidget::mouseMoveEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void HPSDRWidget::mousePressEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void HPSDRWidget::mouseReleaseEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}


