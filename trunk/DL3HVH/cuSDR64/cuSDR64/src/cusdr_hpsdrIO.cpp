/**
* @file  cusdr_hpsdrIO.cpp
* @brief HPSDR device network IO class
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-05-19
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

#define LOG_HPSDRIO

#include "cusdr_hpsdrIO.h"
#include "cusdr_buttons.h"


#include <QComboBox>
#include <QDialogButtonBox>
#include <QLabel>
#include <QPushButton>
#include <QVBoxLayout>
#include <QCheckBox>
#include <QSlider>
#include <QSpinBox>

Q_DECLARE_METATYPE (QAbstractSocket::SocketError)

//#define	btn_height		18
//#define	btn_width		74

QHpsdrIO::QHpsdrIO(THPSDRParameter *ioData)
    :   QObject()
	, m_io(ioData)
	, m_settings(Settings::instance())
{
	m_deviceCards = m_settings->getMetisCardsList();
}

QHpsdrIO::~QHpsdrIO() {
}

void QHpsdrIO::initHPSDRDevice() {

	m_searchTime.start();

	int deviceNo = 0;
	while (deviceNo == 0) {

		deviceNo = findHPSDRDevices();

		if (deviceNo > 1) {
			
			m_settings->setHPSDRDeviceNumber(deviceNo);
			break;
		}

		if (deviceNo > 0) {

			m_settings->setHPSDRDeviceNumber(deviceNo);
			break;
		}

		if (m_searchTime.elapsed() > 5000) {

			m_settings->setHPSDRDeviceNumber(0);
			break;
		}

		//sendMessage("no Metis found - trying again...");
		HPSDRIO_DEBUG << "no Metis found - trying again...";
	}

	m_io->networkIOMutex.lock();
	m_io->devicefound.wakeAll();
	m_io->networkIOMutex.unlock();
}

int QHpsdrIO::findHPSDRDevices() {

	int devicesFound = 0;

	m_findDatagram.resize(63);
    m_findDatagram[0] = (char)0xEF;
    m_findDatagram[1] = (char)0xFE;
    m_findDatagram[2] = (char)0x02;
	for (int i = 3; i < 63; i++)
		m_findDatagram[i] = (char)0x00;

	QUdpSocket socket;
	HPSDRIO_DEBUG << "using " << qPrintable(QHostAddress(m_settings->getHPSDRDeviceLocalAddr()).toString()) << " for discovery.";

	// clear comboBox entries in the network dialogue
	m_settings->clearNetworkIOComboBoxEntry();

	if (socket.bind(
				QHostAddress(m_settings->getHPSDRDeviceLocalAddr()), 
				QUdpSocket::ReuseAddressHint | QUdpSocket::ShareAddress))
				//QUdpSocket::ShareAddress))
				//QUdpSocket::DontShareAddress))
	{
		CHECKED_CONNECT(
			&socket, 
			SIGNAL(error(QAbstractSocket::SocketError)), 
			this, 
			SLOT(displayDiscoverySocketError(QAbstractSocket::SocketError)));

		m_settings->setMetisPort(this, socket.localPort());
		HPSDRIO_DEBUG << "discovery_socket bound successfully to port " << socket.localPort();
	}
	else {
		
		HPSDRIO_DEBUG << "discovery_socket bind failed.";
	}

	if (socket.writeDatagram(m_findDatagram, QHostAddress::Broadcast, METIS_PORT) == 63)
		HPSDRIO_DEBUG << "discovery data sent.";
	else
		HPSDRIO_DEBUG << "discovery data not sent.";


	// wait a little
	SleeperThread::msleep(30);

	//if (socket.waitForReadyRead(500)) {
	while (socket.hasPendingDatagrams()) {

		TNetworkDevicecard mc;
		quint16 port;
				
		m_deviceDatagram.resize(socket.pendingDatagramSize());
		socket.readDatagram(m_deviceDatagram.data(), m_deviceDatagram.size(), &mc.ip_address, &port);

		if (m_deviceDatagram[0] == (char)0xEF && m_deviceDatagram[1] == (char)0xFE) {
			
			if (m_deviceDatagram[2] == (char)0x02) {
				
				sprintf(mc.mac_address, "%02X:%02X:%02X:%02X:%02X:%02X",
					m_deviceDatagram[3] & 0xFF, m_deviceDatagram[4] & 0xFF, m_deviceDatagram[5] & 0xFF,
					m_deviceDatagram[6] & 0xFF, m_deviceDatagram[7] & 0xFF, m_deviceDatagram[8] & 0xFF);
				
				HPSDRIO_DEBUG << "Device found at " << qPrintable(mc.ip_address.toString()) << ":" << port << "; Mac addr: [" << mc.mac_address << "]";
				HPSDRIO_DEBUG << "Device code version: " << qPrintable(QString::number(m_deviceDatagram.at(9), 16));

				int no = m_deviceDatagram.at(10);
				QString str;
				if (no == 0)
					str = "Metis";
				else if (no == 1)
					str = "Hermes";
				else if (no == 2)
					str = "Griffin";

				mc.boardID = no;
				mc.boardName = str;
				HPSDRIO_DEBUG << "Device board ID: " <<  no; 
				HPSDRIO_DEBUG << "Device is: " << qPrintable(str);
								
				m_deviceCards.append(mc);

				/*str = mc.ip_address.toString();
				str.prepend(": ");
				str.prepend(mc.boardName);*/
				str += " (";
				str += mc.ip_address.toString();
				str += ")";
				
				//m_settings->addNetworkIOComboBoxEntry(mc.ip_address.toString());
				m_settings->addNetworkIOComboBoxEntry(str);
				devicesFound++;
			}
			else if (m_deviceDatagram[2] == (char)0x03) {

				HPSDRIO_DEBUG << "Device already sending data!";
			}
		}
		
	}
	m_settings->setMetisCardList(m_deviceCards);

	if (devicesFound == 1) {

		m_settings->setCurrentMetisCard(m_deviceCards.at(0));
		HPSDRIO_DEBUG << "Device selected: " << qPrintable(m_deviceCards.at(0).ip_address.toString());
	}
	return devicesFound;
}

void QHpsdrIO::displayDiscoverySocketError(QAbstractSocket::SocketError error) {

	HPSDRIO_DEBUG << "discovery socket error: " << error;
}

//void NetworkIO::showNetworkIODialog() {

	//m_metisDialog->exec();
	//m_networkIO->exec();
//}

//void NetworkIO::metisDeviceChanged(int index) {
//
//    //m_settings->setCurrentMetisCard(m_metisCards.at(index));
//}

//void NetworkIO::okBtnClicked() {
//
//	m_settings->setCurrentMetisCard(m_metisCards.at(m_metisDeviceComboBox->currentIndex()));
//	NETWORKDIALOG_DEBUG << "Metis selected: " << m_metisCards.at(m_metisDeviceComboBox->currentIndex()).ip_address.toString();
//	accept();
//}

void QHpsdrIO::clear() {

	//m_metisDeviceComboBox->clear();
	m_deviceCards.clear();
}

// *********************************************************************
// Network IO Dialog

//NetworkIODialog::NetworkIODialog(QWidget *parent)
//    :   QDialog(parent)
//	,	m_settings(Settings::instance())
//{
//	setWindowModality(Qt::NonModal);
//	setWindowOpacity(0.9);
//	setStyleSheet(m_settings->getDialogStyle());
//
//	setMouseTracking(true);
//
//	m_titleFont.setStyleStrategy(QFont::PreferAntialias);
//	m_titleFont.setFixedPitch(true);
//	m_titleFont.setPixelSize(13);
//	m_titleFont.setFamily("Arial");
//	m_titleFont.setBold(true);
//	
//
//	QVBoxLayout *dialogLayout = new QVBoxLayout(this);
//
//	m_metisDeviceComboBox = new QComboBox(this);
//	m_metisDeviceComboBox->setStyleSheet(m_settings->getComboBoxStyle());
//	m_metisDeviceComboBox->setMinimumContentsLength(30);
//	
//	QScopedPointer<QHBoxLayout> titleLayout(new QHBoxLayout);
//	QLabel *titleLabel = new QLabel(tr("found more than one Metis:"), this);
//	titleLabel->setFont(m_titleFont);
//	titleLabel->setStyleSheet(m_settings->getLabelStyle());
//	titleLayout->addWidget(titleLabel);
//	dialogLayout->addLayout(titleLayout.data());
//	titleLayout.take(); // ownership transferred to dialogLayout
//
//	QScopedPointer<QHBoxLayout> metisDeviceLayout(new QHBoxLayout);
//	QLabel *ipAddressLabel = new QLabel(tr("IP address"), this);
//	ipAddressLabel->setStyleSheet(m_settings->getLabelStyle());
//	metisDeviceLayout->addWidget(ipAddressLabel);
//	metisDeviceLayout->addWidget(m_metisDeviceComboBox);
//	dialogLayout->addLayout(metisDeviceLayout.data());
//	metisDeviceLayout.take(); // ownership transferred to dialogLayout
//
//	/*CHECKED_CONNECT(
//		m_metisDeviceComboBox, 
//		SIGNAL(activated(int)),
//		this, 
//		SLOT(metisDeviceChanged(int)));*/
//
//	AeroButton* okBtn = new AeroButton("Ok", this);
//	okBtn->setRoundness(10);
//	okBtn->setFixedSize(btn_width, btn_height);
//	CHECKED_CONNECT(
//		okBtn, 
//		SIGNAL(clicked()), 
//		this, 
//		SLOT(okBtnClicked()));
//
//	AeroButton* cancelBtn = new AeroButton("Cancel", this);
//	cancelBtn->setRoundness(10);
//	cancelBtn->setFixedSize(btn_width, btn_height);
//	CHECKED_CONNECT(
//		cancelBtn, 
//		SIGNAL(clicked()), 
//		this, 
//		SLOT(reject()));
//
//	QHBoxLayout *hbox = new QHBoxLayout;
//	hbox->setSpacing(1);
//	hbox->addWidget(okBtn);
//	hbox->addWidget(cancelBtn);
//
//	dialogLayout->addLayout(hbox);
//    
//    setLayout(dialogLayout);
//}
//
//NetworkIODialog::~NetworkIODialog() {
//}
//
//void NetworkIODialog::okBtnClicked() {
//
//	//m_settings->setCurrentMetisCard(m_metisCards.at(m_metisDeviceComboBox->currentIndex()));
//	NETWORKDIALOG_DEBUG << "Metis selected: " << m_metisCards.at(m_metisDeviceComboBox->currentIndex()).ip_address.toString();
//	accept();
//}