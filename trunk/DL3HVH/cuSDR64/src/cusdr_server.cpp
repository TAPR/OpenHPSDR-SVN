/**
* @file cusdr_server.cpp
* @brief Server implementation class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-10-28
*/

/* 
 *   Copyright (C) John Melton, G0ORX/N6LYT and Dave McMcQuate, WA8YWQ
 *
 *   Copyright 2010 adaptation for cuSDR by  Hermann von Hasseln, DL3HVH
 *
 *   This program is free software; you can redistribute it and/or
 *   modify it under the terms of the GNU General Public License
 *   as published by the Free Software Foundation; either version 2
 *   of the License, or (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program; if not, write to the Free Software
 *   Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */

//#define LOG_SERVER

#include "cusdr_server.h"
#include "cusdr_settings.h"



HPSDRServer::HPSDRServer(QObject *parent)
	: QTcpServer(parent)
	, m_settings(Settings::instance())
	, m_hwInterface(m_settings->getHWInterface())
	, m_serverMode(m_settings->getCurrentServerMode())
	, m_dataEngineState(QSDR::DataEngineDown)
{
	setupConnections();

	for (int i = 0; i < MAX_RECEIVERS; i++) {

		m_receivers[i] = -1;
	}

	seps = " ,\r\n";
}

HPSDRServer::~HPSDRServer() {

	serverStop();
	m_rxList.clear();
	//m_ipList.clear();
	//m_niList.clear();
	qDeleteAll(m_clientConnections);
}

void HPSDRServer::setupConnections() {

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(masterSwitchChanged(QObject *, bool)), 
		this, 
		SLOT(masterSwitchChanged(QObject *, bool)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(systemStateChanged(
					QObject *, 
					QSDR::_Error, 
					QSDR::_HWInterfaceMode, 
					QSDR::_ServerMode, 
					QSDR::_DataEngineState)), 
		this, 
		SLOT(setSystemState(
					QObject *, 
					QSDR::_Error, 
					QSDR::_HWInterfaceMode, 
					QSDR::_ServerMode, 
					QSDR::_DataEngineState)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(rxListChanged(QList<HPSDRReceiver *>)),
		this,
		SLOT(rxListChanged(QList<HPSDRReceiver *>)));

	CHECKED_CONNECT(
		this, 
		SIGNAL(clientDisconnectedEvent(int)), 
		m_settings, 
		SLOT(clientDisconnected(int)));

	CHECKED_CONNECT(
		this, 
		SIGNAL(newConnection()), 
		this, 
		SLOT(handleNewConnection()));
}

void HPSDRServer::setSystemState(
	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	Q_UNUSED (sender)
	Q_UNUSED (err)

	if (m_hwInterface != hwmode) m_hwInterface = hwmode;

	if (m_serverMode != mode) m_serverMode = mode;
		
	if (m_dataEngineState == state) return;
	m_dataEngineState = state;
}

bool HPSDRServer::startServer() {

	// listen for new connections
	if (!listen(QHostAddress::Any, m_settings->listenPort())) {

		m_message = tr("[server]: unable to start the listening service: '%1'.");
		emit messageEvent(m_message.arg(errorString()));

		return false;
	}

	m_message = tr("[server]: is running on '%1:%2'.");
	emit messageEvent(m_message.arg(m_settings->getServerAddr()).arg(serverPort()));

	return true;
}

void HPSDRServer::stopServer() {

	if (isListening()) {  // was the listener started?

		// shut down output_thread(s)
		emit messageEvent("[server]: shutting down the server...");
		
		// clear TCP/IP connections list
		foreach(QTcpSocket *socket, m_clientConnections) {

			socket->close();
		}
		m_clientConnections.clear();
		SERVER_DEBUG << "masterSwitchChanged TCP client socket(s) closed.";
		
		// shutdown TCP/IP command server
		serverStop();
		SERVER_DEBUG <<"masterSwitchChanged TCP server closed.";
	}
}

void HPSDRServer::rxListChanged(QList<HPSDRReceiver *> rxList) {

	QMutexLocker locker(&m_serverMutex);
		m_rxList = rxList;
	locker.unlock();
}

void HPSDRServer::masterSwitchChanged(QObject *sender, bool power) {

	Q_UNUSED(sender)
	Q_UNUSED(power)
}

void HPSDRServer::serverStop() {

	//disconnect(this, 0, 0, 0);
	close();
}

void HPSDRServer::handleNewConnection() {

	while (hasPendingConnections()) {
	
		QTcpSocket *socket = nextPendingConnection();
		
        CHECKED_CONNECT(
			socket, 
			SIGNAL(disconnected()), 
			this, 
			SLOT(clientDisconnected()));

		CHECKED_CONNECT(
			socket, 
			SIGNAL(readyRead()), 
			this, 
			SLOT(readCommand()));

		QString message = tr("[server]: client connected from %1 on port %2.");
		emit messageEvent(message.arg(socket->peerAddress().toString()).arg(socket->peerPort()));
		
        m_clientConnections.append(socket);
    }
}

void HPSDRServer::clientDisconnected() {

    QTcpSocket *socket = qobject_cast<QTcpSocket *>(sender());
 
    if (!socket) return;

	int client = m_clientConnections.indexOf(socket);

	m_clientConnections.removeAll(socket);
    socket->deleteLater();

	detachReceiver(client, client);

	m_message = tr("[server]: client %1 disconnected.");
	emit messageEvent(m_message.arg(client));

	emit clientDisconnectedEvent(client);
}

void HPSDRServer::sendMessageToAllClients() {

    m_message = QString("This is the cuSDR server!");
 
    foreach(QTcpSocket *client, m_clientConnections) {
	
        client->write(m_message.toLatin1());
    }
}

void HPSDRServer::readCommand() {

	QTcpSocket *socket = qobject_cast<QTcpSocket *>(sender());

	m_bytesRead = socket->read(command, sizeof(command));

	command[m_bytesRead] = 0;
	
	int client = m_clientConnections.indexOf(socket);

	response = parseCommand(command, client);
	socket->write(response);
	
	//QString message = tr("[server]: response(rx %1): '%2'.");
	//emit messageEvent(message.arg(receivers[client]).arg(response));
}

char *HPSDRServer::attachReceiver(int rx, int client) {

	if (m_rxList[rx]->property("socketState") == HPSDRReceiver::RECEIVER_ATTACHED)
		return CLIENT_ATTACHED;

	if (rx >= m_rxList.size())
		return RECEIVER_INVALID;

	if (m_rxList[rx]->getConnectedStatus())
		return RECEIVER_IN_USE;
    
	m_rxList[rx]->setProperty("socketState", "RECEIVER_ATTACHED");
	m_rxList[rx]->setReceiver(rx);
	m_rxList[rx]->setClient(client);
	m_rxList[rx]->setPeerAddress(m_clientConnections[client]->peerAddress());
	
	m_rxList[rx]->setIQPort(-1);
	m_settings->setIQPort(this, rx, -1);

	SERVER_DEBUG	<< "attachReceiver client" 
					<< m_rxList[rx]->getClient() 
					<< "connected to receiver" 
					<< m_rxList[rx]->getReceiver();

	m_receivers[client] = rx;
	
	m_settings->setClientConnected(this, true);
	m_settings->setClientNoConnected(this, client);
	
	static char resp[80];
    //sprintf_s(resp, "%s %d", "OK", m_settings->getSampleRate());
	
	m_settings->setRxList(m_rxList);

	return resp;
}

char *HPSDRServer::detachReceiver(int rx, int client) {

	//if (m_rxList[rx]->property("socketState") == HPSDRReceiver::RECEIVER_ATTACHED)
	//	return CLIENT_DETACHED;

    if (rx >= m_settings->getNumberOfReceivers())
		return RECEIVER_INVALID;

    //if (m_rxList[rx]->client != (HPSDRClient *) NULL)
	//	return RECEIVER_NOT_OWNER;

	m_serverMutex.lock();
    m_rxList[rx]->setProperty("socketState", "RECEIVER_DETACHED");
	m_rxList[rx]->setConnectedStatus(false);
	m_serverMutex.unlock();

	SleeperThread::msleep(200);
	
	m_settings->setRcveIQ(0);
	m_settings->setSendIQ(0);

	m_receivers[client] = -1;

	m_settings->setRxList(m_rxList);
    
    return OK;
}

char *HPSDRServer::setFrequency(long frequency, int rx) {

	if (m_rxList[rx]->property("socketState") == HPSDRReceiver::RECEIVER_DETACHED)
		return CLIENT_DETACHED;

	if (m_rxList[rx]->getReceiver() < 0)
		return RECEIVER_INVALID;

	//emit frequencyChangedEvent(true, rx, frequency);

	m_settings->setFrequency(this, true, rx, frequency);

    return OK;
}

char *HPSDRServer::parseCommand(char *command, int client) {

    m_message = tr("[client %1]: command '%2'.");
	
    //token = strtok_s(command, seps, &next_token);

    if (token != NULL) {	
		
		if (strcmp(token, "attach") == 0) {
			
			// select receiver
            //token = strtok_s(NULL, seps, &next_token);

            if (token != NULL) {
				
				//int rx = atoi(token);
				emit messageEvent(m_message.arg(client).arg(command));
				return attachReceiver(atoi(token), client);
            } 
			else
				return INVALID_COMMAND;

        } else if (strcmp(token, "detach") == 0) {
			
			// select receiver
            //token = strtok_s(NULL, seps, &next_token);

            if (token != NULL) {

				//int rx = atoi(token);
				emit messageEvent(m_message.arg(client).arg(command));
				return detachReceiver(atoi(token), client);

            } 
			else
				return INVALID_COMMAND;

        } else if (strcmp(token, "frequency") == 0)	{
			
			// set frequency
            //token = strtok_s(NULL, seps, &next_token);

            if (token != NULL) {
				
				//long f = atol(token);
				return setFrequency(atol(token), m_receivers[client]);
            } 
			else
				return INVALID_COMMAND;

        } else if (strcmp(token, "start") == 0) {
			
			//token = strtok_s(NULL, seps, &next_token);

            if (token != NULL) {
				
				if (strcmp(token, "iq") == 0) {
					
					//token = strtok_s(NULL, seps, &next_token);

					if (token != NULL) {
						
						emit setIQPortEvent(m_receivers[client], atoi(token));
						//emit setConnectedStatusEvent(receivers[client], true);
						m_settings->setRxConnectedStatus(this, m_receivers[client], true);
						
						// Remember the last receiver started, so that one will send demodulated data back to Mercury.
						audioReceiver = m_receivers[client];

						//emit newClientEvent(receivers[client]);
						m_settings->setClientNoConnected(this, m_receivers[client]);
						//emit audioReceiverEvent(audioReceiver);
						m_settings->setAudioRx(this, audioReceiver);
						
						return OK;
					} 
					else
						return INVALID_COMMAND;

				} else if (strcmp(token, "bandscope") == 0) {
					
					//token = strtok_s(NULL, seps, &next_token);

                    if (token != NULL) {
						
						m_rxList[client]->setBSPort(atoi(token));
						emit messageEvent(m_message.arg(client).arg(command));
						m_settings->setRxList(m_rxList);
						return OK;

                    } else {
						// invalid command string
			            return INVALID_COMMAND;
				    }
				} 
				else
					return INVALID_COMMAND;
			}
			else 
				return INVALID_COMMAND;

        } else if (strcmp(token, "stop" ) == 0) {
			
			//token = strtok_s(NULL, seps, &next_token);

            if (token != NULL) {
				
				if (strcmp(token, "iq") == 0) {
					
					m_rxList[client]->setIQPort(-1);
					m_settings->setRxList(m_rxList);
					return OK;

				} else if (strcmp(token, "bandscope") == 0) {
					
					m_rxList[client]->setBSPort(-1);
					m_settings->setRxList(m_rxList);
					return OK;

				} 
				else
					return INVALID_COMMAND;
			} 
			else
				return INVALID_COMMAND;

		} 
		else if (strcmp(token, "selectAudio") == 0) {
			
			// change selection of which receiver's audio goes to Mercury headphone output
			//token = strtok_s(NULL, seps, &next_token);
			if (token != NULL) {
				
				selectAudio = atoi(token);
				if (   (selectAudio >= 0) 
						&& (selectAudio < m_settings->getNumberOfReceivers()) 
						&& (m_rxList[selectAudio]->getConnectedStatus())
					) {	
						//m_dataEngine->AudioReceiverChanged(selectAudio);
						emit messageEvent(m_message.arg(client).arg(command));
						//emit audioReceiverEvent(selectAudio);
						m_settings->setAudioRx(this, selectAudio);
						return OK;

				} 
				else 
					return RECEIVER_INVALID;
			}
			else 
				return RECEIVER_INVALID;

        } 
		else
			return INVALID_COMMAND;

    } 
	else
		return INVALID_COMMAND;

	return INVALID_COMMAND;
}

void HPSDRServer::newMessage(QString message) {

	emit messageEvent(message);
}
