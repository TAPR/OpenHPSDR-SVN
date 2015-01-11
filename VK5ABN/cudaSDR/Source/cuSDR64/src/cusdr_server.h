/**
* @file cusdr_server.h
* @brief Server implementation header file for cuSDR
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

#ifndef CUSDR_SERVER_H
#define CUSDR_SERVER_H

#include <QObject>
#include <QMetaType>
#include <QtNetwork>
#include <QNetworkInterface>
#include <QTcpServer>
#include <QTcpSocket>
#include <QUdpSocket>
#include <QDebug>

#include "cusdr_receiver.h"


#ifdef LOG_SERVER
#   define SERVER_DEBUG qDebug().nospace() << "Server::\t"
#else
#   define SERVER_DEBUG nullDebug()
#endif


class HPSDRServer : public QTcpServer {

	Q_OBJECT

public:
	explicit HPSDRServer(QObject *parent = 0);
	~HPSDRServer();

	
public slots:
	bool	startServer();
	void	stopServer();
	void	serverStop();
	
	int		getAudioReceiver() { return audioReceiver; }

private:
	Settings*					set;

	QSDR::_ServerMode			m_serverMode;
	QSDR::_HWInterfaceMode		m_hwInterface;
	QSDR::_DataEngineState		m_dataEngineState;

	QList<Receiver *>			m_rxList;
	QList<QTcpSocket *>			m_clientConnections;

	int			m_receivers[MAX_RECEIVERS];
	
	QMutex		m_serverMutex;
	
	QString		m_message;
	QString		m_separators;

	qint64		m_bytesRead;

	char		command[80];
	//char*		seps;
	char*		token;
	char*		next_token;
	char*		response;

	int			socketDescriptor;
	int			audioReceiver;
	int			selectAudio;

private slots:
	void	setSystemState(
					QObject *sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);

	void 	masterSwitchChanged(QObject *sender, bool power);
	void	rxListChanged(QList<Receiver *> rxList);
	void	setupConnections();
	void 	handleNewConnection();
    void 	clientDisconnected();
	void 	newMessage(QString message);
	void 	sendMessageToAllClients();
	void	readCommand();
	
	char*	attachReceiver(int rx, int client);
	char*	detachReceiver(int rx, int client);
	char*	parseCommand(char *command, int client);
	char*	setFrequency(long frequency, int client);
	
signals:
	void	masterSwitchEvent();
	void	messageEvent(QString message);

	void	setIQPortEvent(int rx, int port);
	void 	socketErrorEvent(int socketErrorCode, QString socketErrorString, QString addr, quint16 port);
	void	setPeerAddressEvent(int rx, QHostAddress address);
	void	clientConnectedEvent(int rx);
	void	clientDisconnectedEvent(int client);
	void	frequencyChangedEvent(bool value, int rx, long frequency);
	void	newClientEvent(int rx);
	void	audioReceiverEvent(int rx);
};


#endif  // CUSDR_SERVER_H
