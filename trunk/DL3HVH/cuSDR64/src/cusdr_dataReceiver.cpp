/**
* @file  cusdr_dataReceiver.cpp
* @brief Data receiver class
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
#define LOG_DATA_RECEIVER

#include "cusdr_dataReceiver.h"

DataReceiver::DataReceiver(THPSDRParameter *ioData)
	: QObject()
	, io(ioData)
	, m_settings(Settings::instance())
	, m_sequence(0)
	, m_oldSequence(-1)
	, m_sequenceWideBand(0)
	, m_oldSequenceWideBand(-1)
	, m_wbCount(0)
	, m_stopped(false)
	, m_sendEP4(false)
	, m_dataReceiverSocketOn(false)
	, m_socketBufferSize(m_settings->getSocketBufferSize())
	, m_manualBufferSize(m_settings->getManualSocketBufferSize())
{
	m_metisGetDataSignature.resize(3);
	m_metisGetDataSignature[0] = (char)0xEF;
	m_metisGetDataSignature[1] = (char)0xFE;
	m_metisGetDataSignature[2] = (char)0x01;
	//m_metisGetDataSignature[3] = (char)0x06;

	m_datagram.resize(1032);
	m_wbDatagram.resize(0);

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(manualSocketBufferChanged(QObject*, bool)), 
		this, 
		SLOT(setManualSocketBufferSize(QObject*, bool)));
}

DataReceiver::~DataReceiver() {

	if (m_dataReceiverSocketOn) {
		m_dataReceiverSocket->close();
		delete m_dataReceiverSocket;
		m_dataReceiverSocket = NULL;
	}
}

void DataReceiver::stop() {

	m_mutex.lock();
		m_stopped = true;
	m_mutex.unlock();
}

void DataReceiver::initDataReceiverSocket() {

	m_dataReceiverSocket = new QUdpSocket();

	int newBufferSize;

	if (m_manualBufferSize) {

		newBufferSize = m_socketBufferSize * 1032;
		DATA_RECEIVER_DEBUG << "initDataReceiverSocket socket buffer size set to " << m_socketBufferSize << " kB.";
	}
	else {

		if (io->samplerate == 192000) {

			newBufferSize = 64 * 1032;
			DATA_RECEIVER_DEBUG << "initDataReceiverSocket socket buffer size set to 64 kB.";
		}
		else if (io->samplerate == 96000) {

			newBufferSize = 32 * 1032;
			DATA_RECEIVER_DEBUG << "initDataReceiverSocket socket buffer size set to 32 kB.";
		}
		else if (io->samplerate == 48000) {

			newBufferSize = 16 * 1032;
			DATA_RECEIVER_DEBUG << "initDataReceiverSocket socket buffer size set to 16 kB.";
		}
	}

	if (m_dataReceiverSocket->bind(QHostAddress(m_settings->getHPSDRDeviceLocalAddr()), 
								   m_settings->metisPort(),
								   QUdpSocket::ReuseAddressHint | QUdpSocket::ShareAddress))
	{
		if (::setsockopt(m_dataReceiverSocket->socketDescriptor(), SOL_SOCKET,
                     SO_RCVBUF, (char *)&newBufferSize, sizeof(newBufferSize)) == -1) {
						 
						 DATA_RECEIVER_DEBUG << "initDataReceiverSocket error!";
		}

		CHECKED_CONNECT(
			m_dataReceiverSocket, 
			SIGNAL(error(QAbstractSocket::SocketError)), 
			this, 
			SLOT(displayDataReceiverSocketError(QAbstractSocket::SocketError)));

		CHECKED_CONNECT_OPT(
			m_dataReceiverSocket, 
			SIGNAL(readyRead()), 
			this, 
			SLOT(readMetisData()),
			Qt::DirectConnection);

		//m_IQRcvrSocket->setSocketOption(QAbstractSocket::LowDelayOption, 1);
		//m_IQRcvrSocket->setSocketOption(QAbstractSocket::KeepAliveOption, 1);

		DATA_RECEIVER_DEBUG << "data receiver socket bound successful to local port" << m_dataReceiverSocket->localPort();

		m_dataReceiverSocketOn = true;
	}
	else {
		
		DATA_RECEIVER_DEBUG << "data receiver socket binding failed.";

		m_dataReceiverSocketOn = false;
	}
}

void DataReceiver::readMetisData() {

	/*if (m_settings->getMercuryVersion() > 32 || m_settings->getHermesVersion() > 16)
		m_wbDatagram.resize(2*BIGWIDEBANDSIZE);
	else
		m_wbDatagram.resize(0);*/

	// if we have 4096 * 16 bit = 8 * 1024 raw consecutive ADC samples, m_wbBuffers = 8
	// we have 16384 * 16 bit = 32 * 1024 raw consecutive ADC samples, m_wbBuffers = 32
	if (m_settings->getMercuryVersion() > 32 || m_settings->getHermesVersion() > 16)
		m_wbBuffers = BIGWIDEBANDSIZE / 512;
	else
		m_wbBuffers = SMALLWIDEBANDSIZE / 512;

	while (m_dataReceiverSocket->hasPendingDatagrams()) {

		if (m_dataReceiverSocket->readDatagram(m_datagram.data(), m_datagram.size()) == METIS_DATA_SIZE) {
			
			if (m_datagram.left(3) == m_metisGetDataSignature) {

				if (m_datagram[3] == (char)0x06) {

					m_sequence  = (m_datagram[4] & 0xFF) << 24;
					m_sequence += (m_datagram[5] & 0xFF) << 16;
					m_sequence += (m_datagram[6] & 0xFF) << 8;
					m_sequence += (m_datagram[7] & 0xFF);

					if (m_sequence != m_oldSequence + 1)
						DATA_RECEIVER_DEBUG	<< "readData missed " << m_sequence - m_oldSequence 
											<< " packages.";// error:" << m_dataReceiverSocket->errorString();
				
					m_oldSequence = m_sequence;

					// enqueue first half of the HPSDR frame from Metis
					io->iq_queue.enqueue(m_datagram.mid(METIS_HEADER_SIZE, BUFFER_SIZE/2));
					// enqueue second half of the HPSDR frame from Metis
					io->iq_queue.enqueue(m_datagram.right(BUFFER_SIZE/2));
				}
				else if (m_datagram[3] == (char)0x04) { // wide band data

					//qDebug() << "wideband data received!";
					m_sequenceWideBand  = (m_datagram[4] & 0xFF) << 24;
					m_sequenceWideBand += (m_datagram[5] & 0xFF) << 16;
					m_sequenceWideBand += (m_datagram[6] & 0xFF) << 8;
					m_sequenceWideBand += (m_datagram[7] & 0xFF);

					if (m_sequenceWideBand != m_oldSequenceWideBand + 1)
						DATA_RECEIVER_DEBUG	<< "wideband readData missed " << m_sequenceWideBand - m_oldSequenceWideBand 
											<< " packages.";// error:" << m_dataReceiverSocket->errorString();
				
					m_oldSequenceWideBand = m_sequenceWideBand;

					// check for the last 3 bits of m_sequenceWideBand being 0, then copy over the payload
					if ((0x07 & m_datagram[7]) == 0) m_sendEP4 = true;

					if (m_sendEP4) {
						
						m_wbDatagram.append(m_datagram.mid(METIS_HEADER_SIZE, BUFFER_SIZE));
						//m_wbDatagram.replace(m_wbCount * BUFFER_SIZE, BUFFER_SIZE, m_datagram.mid(METIS_HEADER_SIZE));
						m_wbCount++;
					}
					
					if (m_wbCount == m_wbBuffers) {

						// enqueue
						io->wb_queue.enqueue(m_wbDatagram);
						m_wbDatagram.resize(0);
						m_wbCount = 0;
					}

					//// enqueue
					//io->wb_queue.enqueue(m_wbDatagram);
					////m_wbDatagram.resize(0);
					//if (m_wbCount == m_wbBuffers) m_wbCount = 0;
				}
			}
		}
	}
	//DATA_RECEIVER_DEBUG << "no more pending datagrams.";
}

void DataReceiver::readData() {

	qint64 length = io->inputBuffer.length();
	
	//int buffers = qRound(length/(2*BUFFER_SIZE));
	int buffers = qRound(length/128);

	DATA_RECEIVER_DEBUG << "input buffer length " << length << " buffers " << buffers;

	while (!m_stopped) {
	
		for (int i = 0; i < buffers; i++) {

			//io->data_queue.enqueue(io->inputBuffer.mid(i*2*BUFFER_SIZE, 2*BUFFER_SIZE));
			io->data_queue.enqueue(io->inputBuffer.mid(i*128, 128));
			if (m_stopped) break;
		}
	}
	m_stopped = false;
}

void DataReceiver::displayDataReceiverSocketError(QAbstractSocket::SocketError error) {

	DATA_RECEIVER_DEBUG << "displayDataReceiverSocketError data receiver socket error: " << error;
}

void DataReceiver::setManualSocketBufferSize(QObject *sender, bool value) {

	Q_UNUSED (sender)

	m_manualBufferSize = value;
	m_socketBufferSize = m_settings->getSocketBufferSize();
}