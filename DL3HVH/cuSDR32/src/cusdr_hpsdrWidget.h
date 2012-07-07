/**
* @file cusdr_hpsdrWidget.h
* @brief Hardware settings widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-09-21
*/

/*
 *   
 *   Copyright 2010, 2011 Hermann von Hasseln, DL3HVH
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
 
#ifndef _CUSDR_HPSDR_WIDGET_H
#define _CUSDR_HPSDR_WIDGET_H

#include <QWidget>
#include <QGroupBox>
#include <QLineEdit>

#include "cusdr_buttons.h"
#include "cusdr_settings.h"


class HPSDRWidget : public QWidget {

	Q_OBJECT

public:
	HPSDRWidget(QWidget *parent = 0);
	~HPSDRWidget();


public slots:
	void	addDeviceNICEntry(QString niName, QString ipAddress);
	void	addNICChangedConnection();
	void	setSocketBufSize(QObject *sender, int size);
	void	hpsdrHardwareChanged();
	void	hwInterfaceChanged();
	void	penelopePresenceChanged();
	void	pennyPresenceChanged();
	void	mercuryPresenceChanged();
	void	alexPresenceChanged();
	void	excaliburPresenceChanged();
	void	source122_88MhzChanged();
	
protected:
	void	closeEvent(QCloseEvent *event);
	void	showEvent(QShowEvent *event);
	void	enterEvent(QEvent *event);
	void	leaveEvent(QEvent *event);
	void	mouseMoveEvent(QMouseEvent *event);
	void	mousePressEvent(QMouseEvent *event);
	void	mouseReleaseEvent(QMouseEvent *event);

private:
	Settings*	m_settings;

	QString		m_message;

	QGroupBox*	hpsdrHardwareBtnGroup();
	QGroupBox*	hpsdrInterfaceExclusiveBtnGroup();
	QGroupBox*	receiversExclusiveBtnGroup();
	QGroupBox*	source10MhzExclusiveGroup;
	QGroupBox*	source122_88MhzExclusiveGroup;
	QGroupBox*	deviceNIGroupBox;
	QGroupBox*	searchNetworkDeviceGroupBox;
	QGroupBox*	socketBufferSizeGroupBox;
	QGroupBox*	numberOfReceiversGroup();
	
	QComboBox*	networkDeviceInterfaces;
	QComboBox*	networkDeviceIPAdresses;
	QComboBox*	socketBufferSizes;
	
	AeroButton*	networkPresenceBtn;
	AeroButton*	noHWBtn;

	AeroButton*	hermesPresenceBtn;
	AeroButton*	modulesPresenceBtn;
	AeroButton*	penelopePresenceBtn;
	AeroButton*	pennyPresenceBtn;
	AeroButton*	mercuryPresenceBtn;
	AeroButton*	alexPresenceBtn;
	AeroButton*	excaliburPresenceBtn;
	

	AeroButton*	searchNetworkDeviceBtn;
	
	AeroButton*	atlasBtn;
	AeroButton*	penelopeBtn;
	AeroButton*	mercuryBtn;
	AeroButton*	penelope2Btn;
	AeroButton*	mercury2Btn;

	AeroButton*	rx1Btn;
	AeroButton*	rx2Btn;
	AeroButton*	rx3Btn;
	AeroButton*	rx4Btn;

	AeroButton*	view1Btn;
	AeroButton*	view2Btn;
	AeroButton*	view3Btn;

	AeroButton*	socketBufSizeBtn;

	QList<AeroButton *>	hardwareBtnList;
	QList<AeroButton *>	source10MhzBtnList;
	QList<AeroButton *>	viewBtnList;
	QList<QString> sources10Mhz;
	QList<AeroButton *>	rxBtnList;

	QSDR::_ServerMode		m_serverMode;
	QSDR::_HWInterfaceMode	m_hwInterface;
	QSDR::_HWInterfaceMode	m_hwInterfaceTemp;
	QSDR::_DataEngineState	m_dataEngineState;

	QList<TNetworkDevicecard>	m_deviceCards;

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;
	int		m_numberOfReceivers;
	int		m_hpsdrHardware;
	int		m_socketBufferSize;

	void	setupConnections();
	void	createDeviceNetworkInterfaceGroup();
	void	createDeviceSearchGroup();
	void	createsocketBufferSizeGroup();
	void	createSource10MhzExclusiveGroup();
	void	createSource122_88MhzExclusiveGroup();

private slots:
	void	systemStateChanged(
					QObject *sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);

	void	setHPSDRHardware();
	void	source10MhzChanged();
	void	interfaceBtnClicked();
	void	searchHPSDRDeviceBtnClicked();
	void	socketBufSizeBtnClicked();
	void	setSocketBufferSize(int value);
	void	setClientConnected(QObject *sender, bool value);
	void	setConnected(QObject *sender, bool value);
	void	setDeviceNIC(int index);
	void	setNetworkDeviceList(QList<TNetworkDevicecard> list);
	void	setCurrentNetworkDevice(TNetworkDevicecard card);
	void	setNumberOfReceivers();
	void	numberOfReceiversChanged(int value);
	void	disableButtons();
	void	enableButtons();
	void	setView();
	
signals:
	void	showEvent(QObject *sender);
	void	closeEvent(QObject *sender);
	void	messageEvent(QString message);
};

#endif // _CUSDR_HPSDR_WIDGET_H
