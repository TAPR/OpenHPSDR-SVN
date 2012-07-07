/**
* @file  cusdr_pennyWidget.h
* @brief Penelope/PennyLane J6-Pins settings widget header file for cuSDR
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

#include <QWidget>
#include <QComboBox>
#include <QGroupBox>
#include <QSpinBox>
#include <QLineEdit>

#include "cusdr_buttons.h"
#include "cusdr_settings.h"


class PennyWidget : public QWidget {

	Q_OBJECT

public:
	PennyWidget(QWidget *parent = 0);
	~PennyWidget();

protected:
	void resizeEvent(QResizeEvent *event);

public slots:
	
private:
	void	setupConnections();
	
private:
	Settings*				m_settings;

	QSDR::_ServerMode		m_serverMode;
	QSDR::_HWInterfaceMode	m_hwInterface;
	QSDR::_DataEngineState	m_dataEngineState;

	QGroupBox*	receivePinsGroup;
	QGroupBox*	transmitPinsGroup;	

	QFont	m_tinyFont;
	QFont	m_smallFont;
	QFont	m_normalFont;

	AeroButton*	enableBtn;

	// receive pins buttons
	QList<AeroButton *> receivePinsBtnList;
	QList<AeroButton *> transmitPinsBtnList;
	

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;

	void	createReceivePinsGroup();
	void	createTransmitPinsGroup();

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
