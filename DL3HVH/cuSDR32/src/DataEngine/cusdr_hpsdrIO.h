/**
* @file  cusdr_hpsdrIO.h
* @brief HPSDR device network IO header file
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

#ifndef _CUSDR_HPSDRIO_H
#define _CUSDR_HPSDRIO_H

//#include <QObject>
//#include <QComboBox>
//#include <QDialogButtonBox>
//#include <QLabel>
//#include <QPushButton>
//#include <QVBoxLayout>
//#include <QCheckBox>
//#include <QSlider>
//#include <QSpinBox>
//#include <QElapsedTimer>

#include "cusdr_settings.h"

#ifdef LOG_HPSDRIO
#   define HPSDRIO_DEBUG qDebug().nospace() << "HPSDR IO::\t"
#else
#   define HPSDRIO_DEBUG nullDebug()
#endif


class QHpsdrIO : public QObject {

    Q_OBJECT

public:
    QHpsdrIO(THPSDRParameter *ioData = 0);
    ~QHpsdrIO();

	int		findHPSDRDevices();
	void	clear();

public slots:
	void	initHPSDRDevice();
	
	
private slots:
	void	displayDiscoverySocketError(QAbstractSocket::SocketError error);
	//void	metisDeviceChanged(int index);
	//void	okBtnClicked();
    
private:
	Settings*			set;
	THPSDRParameter*	io;
	QTime				m_searchTime;
	
	QByteArray		m_findDatagram;
	QByteArray		m_deviceDatagram;

	//QString			m_deviceStr;

	TNetworkDevicecard			m_deviceCard;
	QList<TNetworkDevicecard>	m_deviceCards;

signals:

};

#endif // _CUSDR_METISDIALOG_H
