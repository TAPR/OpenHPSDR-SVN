/**
* @file  audioReceiver.h
* @brief audio receiver header file
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

#ifndef _CUSDR_AUDIORECEIVER_H
#define _CUSDR_AUDIORECEIVER_H

#include <QObject>
#include <QMutex>
#include <QByteArray>
#include <QBuffer>
#include <QVector>
#include <QList>
#include <QWaitCondition>
#include <QThread>

#include "cusdr_settings.h"

class AudioReceiver : public QObject {

    Q_OBJECT

public:
	AudioReceiver(THPSDRParameter *ioData = 0);
	~AudioReceiver();

	int			id;

public slots:
	void	initClient();
	
private:
	Settings*		m_settings;
	QMutex			m_mutex;

	QList<QUdpSocket *> clientConnections;
	QString			m_message;
	QByteArray		m_datagram;

	THPSDRParameter	*io;
	
	int				m_client;
	
private slots:
	void	displayAudioRcvrSocketError(QAbstractSocket::SocketError error);
	void	readPendingAudioRcvrData();

signals:
	void 	messageEvent(QString message);
	void	rcveIQEvent(QObject *sender, int value);
	void	outputBufferEvent(unsigned char* outbuffer);
	void	clientConnectedEvent(bool value);
	void	newData();
	void	newAudioData();
};

#endif // _CUSDR_AUDIORECEIVER_H