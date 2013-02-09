/**
* @file  cusdr_dataIO.cpp
* @brief Data IO class
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-10-01
*/

/*   
 *   Copyright 2011 Hermann von Hasseln, DL3HVH
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

#define LOG_DATAIO

#include "cusdr_dataIO.h"

#if defined(Q_OS_WIN32)
#include <winsock2.h>
#endif


DataIO::DataIO(THPSDRParameter *ioData)
	: QObject()
	, set(Settings::instance())
	, io(ioData)
	, m_dataIOSocketOn(false)
	, m_setNetworkDeviceHeader(true)
	, m_sequence(0)
	, m_oldSequence(-1)
	, m_sequenceWideBand(0)
	, m_oldSequenceWideBand(-1)
	, m_wbBuffers(set->getWidebandBuffers() - 1)
	, m_wbCount(0)
	, m_socketBufferSize(set->getSocketBufferSize())
	, m_sendEP4(false)
	, m_manualBufferSize(set->getManualSocketBufferSize())
	, m_packetsToggle(true)
	, m_firstFrame(true)
	, m_stopped(false)
{
	m_dataIOSocket = 0;

	m_metisGetDataSignature.resize(3);
	m_metisGetDataSignature[0] = (char)0xEF;
	m_metisGetDataSignature[1] = (char)0xFE;
	m_metisGetDataSignature[2] = (char)0x01;
	//m_metisGetDataSignature[3] = (char)0x06;

	m_datagram.resize(1032);
	m_wbDatagram.resize(0);
	m_twoFramesDatagram.resize(0);

	m_sendSequence = 0L;
	m_oldSendSequence = 0L;

	m_deviceSendDataSignature.resize(4);
	m_deviceSendDataSignature[0] = (char)0xEF;
	m_deviceSendDataSignature[1] = (char)0xFE;
	m_deviceSendDataSignature[2] = (char)0x01;
	m_deviceSendDataSignature[3] = (char)0x02;

	m_packetLossTime.start();

	CHECKED_CONNECT(
		set,
		SIGNAL(sampleRateChanged(QObject *, int)), 
		this, 
		SLOT(setSampleRate(QObject *, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(manualSocketBufferChanged(QObject*, bool)), 
		this, 
		SLOT(setManualSocketBufferSize(QObject*, bool)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(socketBufferSizeChanged(QObject*, int)), 
		this, 
		SLOT(setSocketBufferSize(QObject*, int)));

	m_message = "m_sendSequence = %1, bytes sent: %2";
}

DataIO::~DataIO() {

	if (m_dataIOSocketOn) {
		m_dataIOSocket->close();
		delete m_dataIOSocket;
		m_dataIOSocket = 0;
	}
}

void DataIO::stop() {

	io->networkIOMutex.lock();
		m_stopped = true;
	io->networkIOMutex.unlock();
}

void DataIO::initDataReceiverSocket() {

	m_dataIOSocket = new QUdpSocket();

	int newBufferSize;

	if (m_manualBufferSize) {

		newBufferSize = m_socketBufferSize * 1024;//m_socketBufferSize * 1032;
		io->networkIOMutex.lock();
		DATAIO_DEBUG << "initDataReceiverSocket socket buffer size set to " << m_socketBufferSize << " kB.";
		io->networkIOMutex.unlock();
	}
	else {

		if (io->samplerate == 384000) {

			newBufferSize = 128*1024;//128 * 1032;
			io->networkIOMutex.lock();
			DATAIO_DEBUG << "socket buffer size set to 128 kB.";
			io->networkIOMutex.unlock();
		}
		else if (io->samplerate == 192000) {

			newBufferSize = 64*1024;//64 * 1032;
			io->networkIOMutex.lock();
			DATAIO_DEBUG << "socket buffer size set to 64 kB.";
			io->networkIOMutex.unlock();
		}
		else if (io->samplerate == 96000) {

			newBufferSize = 32*1024;//32 * 1032;
			io->networkIOMutex.lock();
			DATAIO_DEBUG << "socket buffer size set to 32 kB.";
			io->networkIOMutex.unlock();
		}
		else if (io->samplerate == 48000) {

			newBufferSize = 16*1024;//16 * 1032;
			io->networkIOMutex.lock();
			DATAIO_DEBUG << "socket buffer size set to 16 kB.";
			io->networkIOMutex.unlock();
		}
	}

	if (m_dataIOSocket->bind(QHostAddress(set->getHPSDRDeviceLocalAddr()),
							 set->getMetisPort(),
							 QUdpSocket::DontShareAddress))
							 //QUdpSocket::ReuseAddressHint | QUdpSocket::ShareAddress))
	{

#if defined(Q_OS_WIN32)
		if (::setsockopt(m_dataIOSocket->socketDescriptor(), SOL_SOCKET,
                     SO_RCVBUF, (char *)&newBufferSize, sizeof(newBufferSize)) == -1) {

			io->networkIOMutex.lock();
			DATAIO_DEBUG << "dataIOSocket error!";
			io->networkIOMutex.unlock();
		}
#endif

		
		//m_dataIOSocket->setSocketOption(QAbstractSocket::LowDelayOption, 1);
		//m_dataIOSocket->setSocketOption(QAbstractSocket::KeepAliveOption, 1);

		CHECKED_CONNECT(
			m_dataIOSocket,
			SIGNAL(error(QAbstractSocket::SocketError)), 
			this, 
			SLOT(displayDataReceiverSocketError(QAbstractSocket::SocketError)));

		/*CHECKED_CONNECT_OPT(
			m_dataIOSocket,
			SIGNAL(readyRead()), 
			this, 
			SLOT(readDeviceData()),
			Qt::DirectConnection);*/

		CHECKED_CONNECT(
			m_dataIOSocket,
			SIGNAL(readyRead()), 
			this, 
			SLOT(readDeviceData()));


		io->networkIOMutex.lock();
		DATAIO_DEBUG << "data receiver socket bound successful to local port " << m_dataIOSocket->localPort();
		io->networkIOMutex.unlock();

		m_dataIOSocketOn = true;
		set->setPacketLoss(1);
	}
	else {
		
		io->networkIOMutex.lock();
		DATAIO_DEBUG << "data receiver socket binding failed.";
		io->networkIOMutex.unlock();

		m_dataIOSocketOn = false;
	}
}

void DataIO::readDeviceData() {

	while (m_dataIOSocket->hasPendingDatagrams()) {

		QMutexLocker locker(&io->networkIOMutex);
		if (m_dataIOSocket->readDatagram(m_datagram.data(), m_datagram.size()) == METIS_DATA_SIZE) {
			
			if (m_datagram.left(3) == m_metisGetDataSignature) {

				if (m_datagram[3] == (char)0x06) {

					m_sequence  = (m_datagram[4] & 0xFF) << 24;
					m_sequence += (m_datagram[5] & 0xFF) << 16;
					m_sequence += (m_datagram[6] & 0xFF) << 8;
					m_sequence += (m_datagram[7] & 0xFF);

					if (m_sequence != m_oldSequence + 1) {

						//DATAIO_DEBUG << "readData missed " << m_sequence - m_oldSequence << " packages.";

						if (m_packetLossTime.elapsed() > 100) {
							
							set->setPacketLoss(2);
							m_packetLossTime.restart();
						}
					}

					m_oldSequence = m_sequence;

					//// enqueue first half of the HPSDR frame from the HPSDR device
					//io->iq_queue.enqueue(m_datagram.mid(METIS_HEADER_SIZE, BUFFER_SIZE/2));
					//// enqueue second half of the HPSDR frame from the HPSDR device
					//io->iq_queue.enqueue(m_datagram.right(BUFFER_SIZE/2));

					// enqueue one frame from the HPSDR device
					if (!io->iq_queue.isFull()) {
						io->iq_queue.enqueue(m_datagram.mid(METIS_HEADER_SIZE, BUFFER_SIZE));
					}

					// collect two HPSDR frames
					//if (m_firstFrame) {

					//	m_twoFramesDatagram += m_datagram.mid(METIS_HEADER_SIZE, BUFFER_SIZE);
					//	m_firstFrame = false;
					//}
					//else {

					//	m_twoFramesDatagram += m_datagram.mid(METIS_HEADER_SIZE, BUFFER_SIZE);

					//	//enqueue the two frames
					//	io->iq_queue.enqueue(m_twoFramesDatagram);
					//	m_firstFrame = true;

					//	m_twoFramesDatagram.resize(0);
					//}
				}
				else if (m_datagram[3] == (char)0x04) { // wide band data

					//qDebug() << "wideband data received!";
					m_sequenceWideBand  = (m_datagram[4] & 0xFF) << 24;
					m_sequenceWideBand += (m_datagram[5] & 0xFF) << 16;
					m_sequenceWideBand += (m_datagram[6] & 0xFF) << 8;
					m_sequenceWideBand += (m_datagram[7] & 0xFF);

					if (m_sequenceWideBand != m_oldSequenceWideBand + 1) {

						DATAIO_DEBUG << "wideband readData missed " << m_sequenceWideBand - m_oldSequenceWideBand << " packages.";

						if (m_packetLossTime.elapsed() > 100) {
							
							set->setPacketLoss(2);
							m_packetLossTime.restart();
						}
					}
					
					m_oldSequenceWideBand = m_sequenceWideBand;

					// three 'if's from KISS Konsole
					if ((m_wbBuffers & m_datagram[7]) == 0)
					{						
						m_sendEP4 = true;
						m_wbCount = 0;
					}

					if (m_sendEP4)
					{
						m_wbDatagram.append(m_datagram.mid(METIS_HEADER_SIZE, BUFFER_SIZE));
					}
						
					if (m_wbCount++ == m_wbBuffers)
					{
						// enqueue
						m_sendEP4 = false;
						io->wb_queue.enqueue(m_wbDatagram);
						m_wbDatagram.resize(0);
					}
				}
			}
			//DATA_RECEIVER_DEBUG << "got wrong HPSDR device signature!";
		}
		//DATA_RECEIVER_DEBUG << "got wrong HPSDR device data size!";
	}
	//DATA_RECEIVER_DEBUG << "no more pending datagrams.";
}

void DataIO::readData() {

	qint64 length = io->inputBuffer.length();
	
	//int buffers = qRound(length/(2*BUFFER_SIZE));
	int buffers = qRound(length/128);

	DATAIO_DEBUG << "input buffer length " << length << " buffers " << buffers;

	while (!m_stopped) {
	
		for (int i = 0; i < buffers; i++) {

			//io->data_queue.enqueue(io->inputBuffer.mid(i*2*BUFFER_SIZE, 2*BUFFER_SIZE));
			io->data_queue.enqueue(io->inputBuffer.mid(i*128, 128));
			if (m_stopped) break;
		}
	}
	m_stopped = false;
}

void DataIO::sendInitFramesToNetworkDevice(int rx) {

	QByteArray initDatagram;
	initDatagram.resize(1032);

	initDatagram[0] = (char)0xEF;
	initDatagram[1] = (char)0xFE;
	initDatagram[2] = (char)0x01;
	initDatagram[3] = (char)0x02;
	initDatagram[4] = (char)0x00;
	initDatagram[5] = (char)0x00;
	initDatagram[6] = (char)0x00;
	initDatagram[7] = (char)0x00;

	initDatagram[8] = SYNC;
    initDatagram[9] = SYNC;
    initDatagram[10] = SYNC;

	for (int i = 0; i < 5; i++) {

		initDatagram[i + 11]  = io->control_out[i];
	}

	for (int i = 16; i < 520; i++) {

		initDatagram[i]  = 0x00;
	}

	initDatagram[520] = SYNC;
    initDatagram[521] = SYNC;
    initDatagram[522] = SYNC;

	initDatagram[523] = io->control_out[0] | ((rx + 2) << 1);
	initDatagram[524] = set->getCtrFrequencies().at(rx) >> 24;
	initDatagram[525] = set->getCtrFrequencies().at(rx) >> 16;
	initDatagram[526] = set->getCtrFrequencies().at(rx) >> 8;
	initDatagram[527] = set->getCtrFrequencies().at(rx) ;


	for (int i = 528; i < 1032; i++) initDatagram[i]  = 0x00;

//	for (int i = 0; i < 5; i++) {
//
//		if (m_dataIOSocket->writeDatagram(initDatagram.data(), initDatagram.size(), io->hpsdrDeviceIPAddress, DEVICE_PORT) < 0) {
//
//			io->networkIOMutex.lock();
//			DATAIO_DEBUG << "error sending init data to device: " << qPrintable(m_dataIOSocket->errorString());
//			io->networkIOMutex.unlock();
//		}
//		else {
//
//			if (i == 0) {
//
//				io->networkIOMutex.lock();
//				DATAIO_DEBUG << "init frames sent to network device.";
//				io->networkIOMutex.unlock();
//			}
//		}
//	}

	if (m_dataIOSocket->writeDatagram(initDatagram.data(), initDatagram.size(), io->hpsdrDeviceIPAddress, DEVICE_PORT) < 0) {

		io->networkIOMutex.lock();
		DATAIO_DEBUG << "error sending init data to device: " << qPrintable(m_dataIOSocket->errorString());
		io->networkIOMutex.unlock();
	}
	else {

		io->networkIOMutex.lock();
		DATAIO_DEBUG << "init frames sent to network device.";
		io->networkIOMutex.unlock();
	}

	SleeperThread::msleep(20);

	if (m_dataIOSocket->writeDatagram(initDatagram.data(), initDatagram.size(), io->hpsdrDeviceIPAddress, DEVICE_PORT) < 0) {

		io->networkIOMutex.lock();
		DATAIO_DEBUG << "error sending init data to device: " << qPrintable(m_dataIOSocket->errorString());
		io->networkIOMutex.unlock();
	}
	else {

		io->networkIOMutex.lock();
		DATAIO_DEBUG << "init frames sent to network device.";
		io->networkIOMutex.unlock();
	}
}

void DataIO::networkDeviceStartStop(char value) {

	TNetworkDevicecard metis = set->getCurrentMetisCard();
	//QUdpSocket socket;

//	if (socket.bind(QHostAddress(set->getHPSDRDeviceLocalAddr()),
//				set->getMetisPort(),
//				//QUdpSocket::DefaultForPlatform))
//				QUdpSocket::ReuseAddressHint | QUdpSocket::ShareAddress))
//	{
		//DATAIO_DEBUG << "device start/stop: socket bound successful to local port " << set->getMetisPort();

		m_commandDatagram.resize(64);
		m_commandDatagram[0] = (char)0xEF;
		m_commandDatagram[1] = (char)0xFE;
		m_commandDatagram[2] = (char)0x04;
		m_commandDatagram[3] = (char)value;

		for (int i = 4; i < 64; i++) m_commandDatagram[i] = 0x00;

		if (m_dataIOSocket->writeDatagram(m_commandDatagram, metis.ip_address, DEVICE_PORT) == 64) {

			//if (value == 1) {
			if (value != 0) {

				io->networkIOMutex.lock();
				DATAIO_DEBUG << "sent start command to device at: "<< qPrintable(metis.ip_address.toString());
				io->networkIOMutex.unlock();
				m_networkDeviceRunning = true;
			}
			else {

				//DATA_ENGINE_DEBUG << "sent stop command to Metis at"<< m_metisCards[0].ip_address.toString();
				io->networkIOMutex.lock();
				DATAIO_DEBUG << "sent stop command to device at: "<< qPrintable(metis.ip_address.toString());
				io->networkIOMutex.unlock();
				m_networkDeviceRunning = false;
			}
		}
		else
			DATAIO_DEBUG << "device start/stop: sending command to device failed.";

		//socket.close();
//	}
//	else {
//
//		DATAIO_DEBUG << "device start/stop: socket binding failed.";
//	}

//	socket.close();
//	DATAIO_DEBUG << "device start/stop: socket closed.";
}

void DataIO::writeData() {

	if (m_setNetworkDeviceHeader) {

		m_outDatagram.resize(0);
        m_outDatagram += m_deviceSendDataSignature;

		QByteArray seq(reinterpret_cast<const char*>(&m_sendSequence), sizeof(m_sendSequence));

		m_outDatagram += seq;
		m_outDatagram += io->audioDatagram;

		m_sendSequence++;
        m_setNetworkDeviceHeader = false;
    }
	else {

		m_outDatagram += io->audioDatagram;
		
		if (m_dataIOSocket->writeDatagram(m_outDatagram, set->getCurrentMetisCard().ip_address, DEVICE_PORT) < 0) {
			DATAIO_DEBUG << "error sending data to device: " << m_dataIOSocket->errorString();
		}

		//if (m_sendSequence%100 == 0)
		//	DATAIO_DEBUG << m_sendSequence;

		if (m_sendSequence != m_oldSendSequence + 1) {
			DATAIO_DEBUG << "output sequence error: old = " << m_oldSendSequence << "; new =" << m_sendSequence;
		}

		m_oldSendSequence = m_sendSequence;
		m_setNetworkDeviceHeader = true;
    }
}

void DataIO::displayDataReceiverSocketError(QAbstractSocket::SocketError error) {

	io->networkIOMutex.lock();
	DATAIO_DEBUG << "data IO socket error: " << error;
	io->networkIOMutex.unlock();
}

void DataIO::setManualSocketBufferSize(QObject *sender, bool value) {

	Q_UNUSED (sender)

	m_manualBufferSize = value;
	DATAIO_DEBUG << "m_manualBufferSize to change = " << m_manualBufferSize;
	int socketBufferSize = 1032 * set->getSocketBufferSize();

	io->networkIOMutex.lock();

		if (m_manualBufferSize) {

			DATAIO_DEBUG << "set data IO socket BufferSize to " << m_socketBufferSize;
#if defined(Q_OS_WIN32)
			if (::setsockopt(m_dataIOSocket->socketDescriptor(), SOL_SOCKET,
                     SO_RCVBUF, (char *)&socketBufferSize, sizeof(socketBufferSize)) == -1) {

				io->networkIOMutex.lock();
				DATAIO_DEBUG << "dataIOSocket error!";
				io->networkIOMutex.unlock();
			}
#endif
		}
		else {

			DATAIO_DEBUG << "set data IO socket BufferSize to 32 kB.";
			socketBufferSize = 1032 * 32;
#if defined(Q_OS_WIN32)
			if (::setsockopt(m_dataIOSocket->socketDescriptor(), SOL_SOCKET,
                     SO_RCVBUF, (char *)&socketBufferSize, sizeof(socketBufferSize)) == -1) {

				io->networkIOMutex.lock();
				DATAIO_DEBUG << "dataIOSocket error!";
				io->networkIOMutex.unlock();
			}
#endif
		}
	io->networkIOMutex.unlock();
}

void DataIO::setSocketBufferSize(QObject *sender, int value) {

	Q_UNUSED (sender)

	int socketBufferSize = value * 1024;
	DATAIO_DEBUG << "m_socketBufferSize = " << value;

	io->networkIOMutex.lock();
#if defined(Q_OS_WIN32)
		if (::setsockopt(m_dataIOSocket->socketDescriptor(), SOL_SOCKET,
                     SO_RCVBUF, (char *)&socketBufferSize, sizeof(socketBufferSize)) == -1) {

			io->networkIOMutex.lock();
			DATAIO_DEBUG << "dataIOSocket error!";
			io->networkIOMutex.unlock();
		}
#endif
	io->networkIOMutex.unlock();
}

void DataIO::setSampleRate(QObject *sender, int value) {

	Q_UNUSED(sender)

	int bufferSize;
	io->networkIOMutex.lock();
	switch (value) {
	
		case 48000:
			bufferSize = 16*1024;//128 * 1032
			DATAIO_DEBUG << "socket buffer size set to 16 kB.";
			break;
			
		case 96000:
			bufferSize = 32*1024;//128 * 1032
			DATAIO_DEBUG << "socket buffer size set to 32 kB.";
			break;
			
		case 192000:
			bufferSize = 64*1024;//128 * 1032
			DATAIO_DEBUG << "socket buffer size set to 64 kB.";
			break;
			
		case 384000:
			bufferSize = 128*1024;//128 * 1032
			DATAIO_DEBUG << "socket buffer size set to 128 kB.";
			break;

		default:
			DATAIO_DEBUG << "invalid sample rate !\n";
			break;
	}

#if defined(Q_OS_WIN32)
		if (::setsockopt(m_dataIOSocket->socketDescriptor(), SOL_SOCKET,
                     SO_RCVBUF, (char *)&bufferSize, sizeof(bufferSize)) == -1) {

			DATAIO_DEBUG << "dataIOSocket error!";
		}
#endif

	io->networkIOMutex.unlock();
}