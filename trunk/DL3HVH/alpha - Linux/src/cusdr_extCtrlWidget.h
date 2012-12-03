/**
* @file  cusdr_extCtrlWidget.h
* @brief J6-Pins settings widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-06-13
*/

/*
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
 
#ifndef _CUSDR_PENNY_WIDGET_H
#define _CUSDR_PENNY_WIDGET_H

//#include <QWidget>
//#include <QComboBox>
//#include <QGroupBox>
//#include <QSpinBox>
//#include <QLineEdit>

#include "Util/cusdr_buttons.h"
#include "cusdr_settings.h"


class ExtCtrlWidget : public QWidget {

	Q_OBJECT

public:
	ExtCtrlWidget(QWidget *parent = 0);
	~ExtCtrlWidget();

public slots:
	
private:
	void	setupConnections();
	
private:
	Settings*				set;

	QSDR::_ServerMode		m_serverMode;
	QSDR::_HWInterfaceMode	m_hwInterface;
	QSDR::_DataEngineState	m_dataEngineState;

	QGroupBox*	receivePinsGroup;
	QGroupBox*	transmitPinsGroup;	

	AeroButton*	enableBtn;
	
	QList<QList<AeroButton *> >	receivePinsBtnMatrix;
	QList<QList<AeroButton *> >	transmitPinsBtnMatrix;

	QList<int>	m_rxPins;
	QList<int>	m_txPins;

	bool	m_pennyOCEnabled;

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;

	void	createReceivePinsGroup();
	void	createTransmitPinsGroup();
	void	setValues();
	uchar	getMask(int value);

private slots:
	void	enable();

	void	receivePinsBtnClicked();
	void	transmitPinsBtnClicked();
	
signals:
	void	showEvent(QObject* sender);
	void	closeEvent(QObject* sender);
	void	messageEvent(QString);
};

#endif // _CUSDR_PENNY_WIDGET_H
