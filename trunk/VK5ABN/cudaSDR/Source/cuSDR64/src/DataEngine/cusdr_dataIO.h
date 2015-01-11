/**
* @file  cusdr_dataIO.h
* @brief Data IO header file
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

#ifndef _CUSDR_DATAIO_H
#define _CUSDR_DATAIO_H

//#include <QObject>
//#include <QMutex>
//#include <QByteArray>
//#include <QBuffer>
//#include <QVector>
//#include <QList>
//#include <QWaitCondition>
//#include <QThread>

#include "cusdr_settings.h"

#ifdef LOG_DATAIO
#   define DATAIO_DEBUG qDebug().nospace() << "DataIO::\t"
#else
#   define DATAIO_DEBUG nullDebug()
#endif


class DataIO : public QObject {

    Q_OBJECT

public:
    DataIO(THPSDRParameter *ioData = 0);
	~DataIO();

public slots:
	void	stop();
	void	initDataReceiverSocket();
	void	readData();
	void 	writeData();
	void	sendInitFramesToNetworkDevice(int rx);
	void	networkDeviceStartStop(char value);
	//void	setWidebandBuffers(int value);
	
private slots:
	void setSampleRate(QObject *sender, int value);
	void displayDataReceiverSocketError(QAbstractSocket::SocketError error);
	void setManualSocketBufferSize(QObject *sender, bool value);
	void setSocketBufferSize(QObject *sender, int value);
	void readDeviceData();
	
private:
	Settings*		set;
	QUdpSocket*		m_dataIOSocket;
	//QMutex			m_mutex;
	QByteArray		m_commandDatagram;
	QByteArray		m_datagram;
	QByteArray		m_wbDatagram;
	QByteArray		m_twoFramesDatagram;
	QByteArray		m_metisGetDataSignature;
	QByteArray		m_outDatagram;
	QByteArray		m_deviceSendDataSignature;
	QString			m_message;

	QTime			m_packetLossTime;

	THPSDRParameter*	io;
	//TNetworkDevicecard 	netDevice;

	bool	m_dataIOSocketOn;
	bool	m_networkDeviceRunning;
	bool	m_setNetworkDeviceHeader;

	long	m_sequence;
	long	m_oldSequence;
	long	m_sequenceWideBand;
	long	m_oldSequenceWideBand;
	long	m_sendSequence;
	long	m_oldSendSequence;


	int		m_wbBuffers;
	int		m_wbCount;
	int		m_socketBufferSize;

	bool	m_sendEP4;
	bool	m_manualBufferSize;
	bool	m_packetsToggle;
	bool	m_firstFrame;
	
	volatile bool	m_stopped;

signals:
	void	messageEvent(QString message);
};

#endif // _CUSDR_DATAIO_H
