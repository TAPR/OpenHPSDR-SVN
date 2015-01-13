/**
* @file  audioReceiver.cpp
* @brief audio receiver class
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-10-02
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

#define LOG_AUDIO_RECEIVER

#include "cusdr_audioReceiver.h"


AudioReceiver::AudioReceiver(THPSDRParameter *ioData)
	: QObject()
	, set(Settings::instance())
	, io(ioData)
	, m_client(0)
{
}

AudioReceiver::~AudioReceiver() {

}

void AudioReceiver::displayAudioRcvrSocketError(QAbstractSocket::SocketError error) {

	AUDIO_RECEIVER << "audio client socket error:" << error;
}

void AudioReceiver::initClient() {

	quint16 port = (quint16) (set->getAudioPort() + (io->audio_rx * 2));

	QUdpSocket *socket = new QUdpSocket();
	socket->setSocketOption(QAbstractSocket::LowDelayOption, 1);

	if (socket->bind(port, QUdpSocket::ReuseAddressHint | QUdpSocket::ShareAddress)) {
	
		CHECKED_CONNECT(
			socket, 
			SIGNAL(error(QAbstractSocket::SocketError)), 
			this, 
			SLOT(displayAudioRcvrSocketError(QAbstractSocket::SocketError)));

		CHECKED_CONNECT(
			socket,
			SIGNAL(readyRead()), 
			this, 
			SLOT(readPendingAudioRcvrData()));

		clientConnections.append(socket);

		AUDIO_RECEIVER << "client socket binding successful.";
		m_message = tr("[server]: listening for rx %1 audio on port %2.");
		emit messageEvent(m_message.arg(io->audio_rx).arg(port));
		
		//m_dataEngine->clientConnected = true;
		// need to implement connection in dataEngine !!!!
		emit clientConnectedEvent(true);
		//rcveIQ_toggle = false;
	}
	else {

		m_message = tr("[server]: bind socket failed for socket on port %1.");
		emit messageEvent(m_message.arg(port));
	}
}

void AudioReceiver::readPendingAudioRcvrData() {

	QUdpSocket *socket = qobject_cast<QUdpSocket *>(sender());
	
	while (socket->hasPendingDatagrams()) {
	
		m_datagram.resize(socket->pendingDatagramSize());
			
		if (socket->readDatagram(m_datagram.data(), m_datagram.size()) < 0) {
		
			AUDIO_RECEIVER << "read client" << m_client << "socket failed.";
			if (io->rcveIQ_toggle) {  // toggles the rcveIQ signal
			
				emit rcveIQEvent(this, 2);
				io->rcveIQ_toggle = false;
			}
		}
		else {

			io->au_queue.enqueue(m_datagram);
				
			if (!io->rcveIQ_toggle) {  // toggles the rcveIQ signal

				emit rcveIQEvent(this, 1);
				io->rcveIQ_toggle = true;
			}
		}
	}
}
