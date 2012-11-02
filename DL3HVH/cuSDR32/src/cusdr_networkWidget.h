/**
* @file cusdr_networkWidget.h
* @brief Network settings widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-10-24
*/

/*
 *   
 *   Copyright 2010 - 2012 Hermann von Hasseln, DL3HVH
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
 
#ifndef _CUSDR_NETWORK_WIDGET_H
#define _CUSDR_NETWORK_WIDGET_H

//#include <QWidget>
//#include <QGroupBox>
//#include <QSpinBox>
//#include <QLineEdit>

#include "Util/cusdr_buttons.h"
#include "cusdr_settings.h"

#ifdef LOG_NETWORK_WIDGET
#   define NETWORK_WIDGET_DEBUG qDebug().nospace() << "NetworkWidget::\t"
#else
#   define NETWORK_WIDGET_DEBUG nullDebug()
#endif


class NetworkWidget : public QWidget {

	Q_OBJECT

public:
	NetworkWidget(QWidget *parent = 0);
	~NetworkWidget();


public slots:
	void	addDeviceNICEntry(QString niName, QString ipAddress);
	void	addNICChangedConnection();
	void	setSocketBufSize(QObject *sender, int size);
	void	hwInterfaceChanged();

private:
	Settings	*set;

	QString		m_message;

	QList<TNetworkDevicecard>	m_deviceCards;

	QGroupBox	*hpsdrInterfaceExclusiveBtnGroup();
	QGroupBox	*receiversExclusiveBtnGroup();
	QGroupBox	*source10MhzExclusiveGroup;
	QGroupBox	*source122_88MhzExclusiveGroup;
	QGroupBox	*deviceNIGroupBox;
	QGroupBox	*searchNetworkDeviceGroupBox;
	QGroupBox	*socketBufferSizeGroupBox;
	
	QComboBox	*networkDeviceInterfaces;
	QComboBox	*networkDeviceIPAdresses;
	QComboBox	*socketBufferSizes;
	QComboBox	*m_receiverComboBox;

	QLabel		*socketBufferSizeLabel;
	
	AeroButton	*networkPresenceBtn;
	AeroButton	*noHWBtn;

	AeroButton	*searchNetworkDeviceBtn;
	AeroButton	*socketBufSizeBtn;

	QSDR::_ServerMode		m_serverMode;
	QSDR::_HWInterfaceMode	m_hwInterface;
	QSDR::_HWInterfaceMode	m_hwInterfaceTemp;
	QSDR::_DataEngineState	m_dataEngineState;

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;
	int		m_numberOfReceivers;
	int		m_hpsdrHardware;
	int		m_socketBufferSize;

	void	setupConnections();
	void	createDeviceNetworkInterfaceGroup();
	void	createDeviceSearchGroup();

private slots:
	void	systemStateChanged(
					QObject *sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);

	void	interfaceBtnClicked();
	void	searchHPSDRDeviceBtnClicked();
	void	socketBufSizeBtnClicked();
	void	setSocketBufferSize(int value);
	void	setDeviceNIC(int index);
	void	setNetworkDeviceList(QList<TNetworkDevicecard> list);
	void	setCurrentNetworkDevice(TNetworkDevicecard card);
	void	disableButtons();
	void	enableButtons();
	
signals:
	void	messageEvent(QString message);
};

#endif // _CUSDR_NETWORK_WIDGET_H
