/**
* @file  cusdr_alexWidget.h
* @brief Alexiares filter settings widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-06-09
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
 
#ifndef _CUSDR_ALEX_WIDGET_H
#define _CUSDR_ALEX_WIDGET_H

#include <QWidget>
#include <QComboBox>
#include <QGroupBox>
#include <QSpinBox>
#include <QLineEdit>

#include "cusdr_buttons.h"
#include "cusdr_settings.h"


class AlexWidget : public QWidget {

	Q_OBJECT

public:
	AlexWidget(QWidget *parent = 0);
	~AlexWidget();

protected:
	void resizeEvent(QResizeEvent *event);

public slots:
	
private:
	void	setupConnections();
	void	createReceiveGroup();
	void	createTransmitGroup();
	
private:
	Settings*				m_settings;

	QSDR::_ServerMode		m_serverMode;
	QSDR::_HWInterfaceMode	m_hwInterface;
	QSDR::_DataEngineState	m_dataEngineState;

	QGroupBox*	receiveGroup;
	QGroupBox*	transmitGroup;

	AeroButton*	band160mBtn1;
	AeroButton*	band80mBtn1;
	AeroButton*	band60mBtn1;
	AeroButton*	band40mBtn1;
	AeroButton*	band30mBtn1;
	AeroButton*	band20mBtn1;
	AeroButton*	band17mBtn1;
	AeroButton*	band15mBtn1;
	AeroButton*	band12mBtn1;
	AeroButton*	band10mBtn1;
	AeroButton*	band6mBtn1;

	AeroButton*	band160mBtn2;
	AeroButton*	band80mBtn2;
	AeroButton*	band60mBtn2;
	AeroButton*	band40mBtn2;
	AeroButton*	band30mBtn2;
	AeroButton*	band20mBtn2;
	AeroButton*	band17mBtn2;
	AeroButton*	band15mBtn2;
	AeroButton*	band12mBtn2;
	AeroButton*	band10mBtn2;
	AeroButton*	band6mBtn2;

	AeroButton*	band160mBtn3;
	AeroButton*	band80mBtn3;
	AeroButton*	band60mBtn3;
	AeroButton*	band40mBtn3;
	AeroButton*	band30mBtn3;
	AeroButton*	band20mBtn3;
	AeroButton*	band17mBtn3;
	AeroButton*	band15mBtn3;
	AeroButton*	band12mBtn3;
	AeroButton*	band10mBtn3;
	AeroButton*	band6mBtn3;

	QList<AeroButton *>	band160mBtnList;
	QList<AeroButton *>	band80mBtnList;
	QList<AeroButton *>	band60mBtnList;
	QList<AeroButton *>	band40mBtnList;
	QList<AeroButton *>	band30mBtnList;
	QList<AeroButton *>	band20mBtnList;
	QList<AeroButton *>	band17mBtnList;
	QList<AeroButton *>	band15mBtnList;
	QList<AeroButton *>	band12mBtnList;
	QList<AeroButton *>	band10mBtnList;
	QList<AeroButton *>	band6mBtnList;

	AeroButton*	band160mBtnRx1;
	AeroButton*	band80mBtnRx1;
	AeroButton*	band60mBtnRx1;
	AeroButton*	band40mBtnRx1;
	AeroButton*	band30mBtnRx1;
	AeroButton*	band20mBtnRx1;
	AeroButton*	band17mBtnRx1;
	AeroButton*	band15mBtnRx1;
	AeroButton*	band12mBtnRx1;
	AeroButton*	band10mBtnRx1;
	AeroButton*	band6mBtnRx1;

	AeroButton*	band160mBtnRx2;
	AeroButton*	band80mBtnRx2;
	AeroButton*	band60mBtnRx2;
	AeroButton*	band40mBtnRx2;
	AeroButton*	band30mBtnRx2;
	AeroButton*	band20mBtnRx2;
	AeroButton*	band17mBtnRx2;
	AeroButton*	band15mBtnRx2;
	AeroButton*	band12mBtnRx2;
	AeroButton*	band10mBtnRx2;
	AeroButton*	band6mBtnRx2;

	AeroButton*	band160mBtnRx3;
	AeroButton*	band80mBtnRx3;
	AeroButton*	band60mBtnRx3;
	AeroButton*	band40mBtnRx3;
	AeroButton*	band30mBtnRx3;
	AeroButton*	band20mBtnRx3;
	AeroButton*	band17mBtnRx3;
	AeroButton*	band15mBtnRx3;
	AeroButton*	band12mBtnRx3;
	AeroButton*	band10mBtnRx3;
	AeroButton*	band6mBtnRx3;

	QList<AeroButton *>	band160mBtnRxList;
	QList<AeroButton *>	band80mBtnRxList;
	QList<AeroButton *>	band60mBtnRxList;
	QList<AeroButton *>	band40mBtnRxList;
	QList<AeroButton *>	band30mBtnRxList;
	QList<AeroButton *>	band20mBtnRxList;
	QList<AeroButton *>	band17mBtnRxList;
	QList<AeroButton *>	band15mBtnRxList;
	QList<AeroButton *>	band12mBtnRxList;
	QList<AeroButton *>	band10mBtnRxList;
	QList<AeroButton *>	band6mBtnRxList;

	// Tx
	AeroButton*	band160mBtnTx1;
	AeroButton*	band80mBtnTx1;
	AeroButton*	band60mBtnTx1;
	AeroButton*	band40mBtnTx1;
	AeroButton*	band30mBtnTx1;
	AeroButton*	band20mBtnTx1;
	AeroButton*	band17mBtnTx1;
	AeroButton*	band15mBtnTx1;
	AeroButton*	band12mBtnTx1;
	AeroButton*	band10mBtnTx1;
	AeroButton*	band6mBtnTx1;

	AeroButton*	band160mBtnTx2;
	AeroButton*	band80mBtnTx2;
	AeroButton*	band60mBtnTx2;
	AeroButton*	band40mBtnTx2;
	AeroButton*	band30mBtnTx2;
	AeroButton*	band20mBtnTx2;
	AeroButton*	band17mBtnTx2;
	AeroButton*	band15mBtnTx2;
	AeroButton*	band12mBtnTx2;
	AeroButton*	band10mBtnTx2;
	AeroButton*	band6mBtnTx2;

	AeroButton*	band160mBtnTx3;
	AeroButton*	band80mBtnTx3;
	AeroButton*	band60mBtnTx3;
	AeroButton*	band40mBtnTx3;
	AeroButton*	band30mBtnTx3;
	AeroButton*	band20mBtnTx3;
	AeroButton*	band17mBtnTx3;
	AeroButton*	band15mBtnTx3;
	AeroButton*	band12mBtnTx3;
	AeroButton*	band10mBtnTx3;
	AeroButton*	band6mBtnTx3;

	QFont	m_tinyFont;
	QFont	m_smallFont;
	QFont	m_normalFont;

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;

private slots:
	void	band160mBtnClicked();
	void	band80mBtnClicked();
	void	band60mBtnClicked();
	void	band40mBtnClicked();
	void	band30mBtnClicked();
	void	band20mBtnClicked();
	void	band17mBtnClicked();
	void	band15mBtnClicked();
	void	band12mBtnClicked();
	void	band10mBtnClicked();
	void	band6mBtnClicked();

	void	band160mBtnRxClicked();
	void	band80mBtnRxClicked();
	void	band60mBtnRxClicked();
	void	band40mBtnRxClicked();
	void	band30mBtnRxClicked();
	void	band20mBtnRxClicked();
	void	band17mBtnRxClicked();
	void	band15mBtnRxClicked();
	void	band12mBtnRxClicked();
	void	band10mBtnRxClicked();
	void	band6mBtnRxClicked();
	
signals:
	void	showEvent(QObject* sender);
	void	closeEvent(QObject* sender);
	void	messageEvent(QString);
};

#endif // _CUSDR_ALEX_WIDGET_H
