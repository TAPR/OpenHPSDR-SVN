/**
* @file  cusdr_discoverer.cpp
* @brief HPSDR device discoverer class
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

#define LOG_DISCOVERER

#include "cusdr_discoverer.h"
#include "Util/cusdr_buttons.h"


//#include <QComboBox>
//#include <QDialogButtonBox>
//#include <QLabel>
//#include <QPushButton>
//#include <QVBoxLayout>
//#include <QCheckBox>
//#include <QSlider>
//#include <QSpinBox>

Q_DECLARE_METATYPE (QAbstractSocket::SocketError)

//#define	btn_height		18
//#define	btn_width		74

Discoverer::Discoverer(THPSDRParameter *ioData)
    : QObject()
	, set(Settings::instance())
	, io(ioData)
{
	m_deviceCards = set->getMetisCardsList();
}

Discoverer::~Discoverer() {
}

void Discoverer::initHPSDRDevice() {

	m_searchTime.start();

	int deviceNo = 0;
	while (deviceNo == 0) {

		deviceNo = findHPSDRDevices();

		if (deviceNo > 1) {
			
			set->setHPSDRDeviceNumber(deviceNo);
			break;
		}

		if (deviceNo > 0) {

			set->setHPSDRDeviceNumber(deviceNo);
			break;
		}

		if (m_searchTime.elapsed() > 1000) {

			set->setHPSDRDeviceNumber(0);
			break;
		}

		io->networkIOMutex.lock();
		DISCOVERER_DEBUG << "no Metis found - trying again...";
		io->networkIOMutex.unlock();
	}

	io->networkIOMutex.lock();
	io->devicefound.wakeAll();
	io->networkIOMutex.unlock();
}

int Discoverer::findHPSDRDevices() {

	int devicesFound = 0;

	m_findDatagram.resize(63);
    m_findDatagram[0] = (char)0xEF;
    m_findDatagram[1] = (char)0xFE;
    m_findDatagram[2] = (char)0x02;
	for (int i = 3; i < 63; i++)
		m_findDatagram[i] = (char)0x00;

	QUdpSocket socket;

	CHECKED_CONNECT(
		&socket,
		SIGNAL(error(QAbstractSocket::SocketError)),
		this,
		SLOT(displayDiscoverySocketError(QAbstractSocket::SocketError)));

	io->networkIOMutex.lock();
	DISCOVERER_DEBUG << "using " << qPrintable(QHostAddress(set->getHPSDRDeviceLocalAddr()).toString()) << " for discovery.";
	io->networkIOMutex.unlock();

	// clear comboBox entries in the network dialogue
	set->clearNetworkIOComboBoxEntry();

#if defined(Q_OS_WIN32)

	if (socket.bind(
				QHostAddress(set->getHPSDRDeviceLocalAddr()), 0,
				QUdpSocket::ReuseAddressHint | QUdpSocket::ShareAddress))
				//QUdpSocket::ReuseAddressHint))
	{
		set->setMetisPort(this, socket.localPort());
		io->networkIOMutex.lock();
		DISCOVERER_DEBUG << "discovery_socket bound successfully to port " << socket.localPort();
		io->networkIOMutex.unlock();
	}
	else {

		io->networkIOMutex.lock();
		DISCOVERER_DEBUG << "discovery_socket bind failed.";
		io->networkIOMutex.unlock();

		socket.close();
		return 0;
	}
#elif defined(Q_OS_LINUX)

	if (socket.bind(
				QHostAddress(set->getHPSDRDeviceLocalAddr()),
				QUdpSocket::DefaultForPlatform))
	{
		CHECKED_CONNECT(
			&socket, 
			SIGNAL(error(QAbstractSocket::SocketError)), 
			this, 
			SLOT(displayDiscoverySocketError(QAbstractSocket::SocketError)));

		set->setMetisPort(this, socket.localPort());
		io->networkIOMutex.lock();
		DISCOVERER_DEBUG << "discovery_socket bound successfully to port " << socket.localPort();
		io->networkIOMutex.unlock();
	}
	else {
		
		io->networkIOMutex.lock();
		DISCOVERER_DEBUG << "discovery_socket bind failed.";
		io->networkIOMutex.unlock();

		socket.close();
		return 0;
	}
#endif

	if (socket.writeDatagram(m_findDatagram, QHostAddress::Broadcast, METIS_PORT) == 63) {

		io->networkIOMutex.lock();
		DISCOVERER_DEBUG << "discovery data sent.";
		io->networkIOMutex.unlock();
	}
	else {

		io->networkIOMutex.lock();
		DISCOVERER_DEBUG << "discovery data not sent.";
		io->networkIOMutex.unlock();
	}


	// wait a little
	//SleeperThread::msleep(30);
	SleeperThread::msleep(500);

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
				
				io->networkIOMutex.lock();
				DISCOVERER_DEBUG << "Device found at " << qPrintable(mc.ip_address.toString()) << ":" << port << "; Mac addr: [" << mc.mac_address << "]";
				DISCOVERER_DEBUG << "Device code version: " << qPrintable(QString::number(m_deviceDatagram.at(9), 16));
				io->networkIOMutex.unlock();

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
				io->networkIOMutex.lock();
				DISCOVERER_DEBUG << "Device board ID: " <<  no;
				DISCOVERER_DEBUG << "Device is: " << qPrintable(str);
				io->networkIOMutex.unlock();

				m_deviceCards.append(mc);

				str += " (";
				str += mc.ip_address.toString();
				str += ")";
				
				set->addNetworkIOComboBoxEntry(str);
				devicesFound++;
			}
			else if (m_deviceDatagram[2] == (char)0x03) {

				io->networkIOMutex.lock();
				DISCOVERER_DEBUG << "Device already sending data!";
				io->networkIOMutex.unlock();
			}
		}
		
	}
	set->setMetisCardList(m_deviceCards);

	if (devicesFound == 1) {

		set->setCurrentHPSDRDevice(m_deviceCards.at(0));
		io->networkIOMutex.lock();
		DISCOVERER_DEBUG << "Device selected: " << qPrintable(m_deviceCards.at(0).ip_address.toString());
		io->networkIOMutex.unlock();
	}

	socket.close();
	return devicesFound;
}

void Discoverer::displayDiscoverySocketError(QAbstractSocket::SocketError error) {

	io->networkIOMutex.lock();
	DISCOVERER_DEBUG << "discovery socket error: " << error;
	io->networkIOMutex.unlock();
}

void Discoverer::clear() {

	//m_metisDeviceComboBox->clear();
	m_deviceCards.clear();
}
