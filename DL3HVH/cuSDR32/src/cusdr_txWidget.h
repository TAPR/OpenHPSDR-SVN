/**
* @file  cusdr_txWidget.h
* @brief transmit control widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-06-16
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
 
#ifndef _CUSDR_TX_WIDGET_H
#define _CUSDR_TX_WIDGET_H

#include <QWidget>
#include <QGroupBox>
#include <QLineEdit>

#include "cusdr_buttons.h"
#include "cusdr_settings.h"


class TxWidget : public QWidget {

	Q_OBJECT

public:
	TxWidget(QWidget *parent = 0);
	~TxWidget();


public slots:
	
protected:
	void	closeEvent(QCloseEvent *event);
	void	showEvent(QShowEvent *event);
	void	enterEvent(QEvent *event);
	void	leaveEvent(QEvent *event);
	void	mouseMoveEvent(QMouseEvent *event);
	void	mousePressEvent(QMouseEvent *event);
	void	mouseReleaseEvent(QMouseEvent *event);

private:
	void	setupConnections();
	void	createSourceGroup();
	void	createTransmitFilterGroup();
	void	createPTTOptionsGroup();

private:
	Settings*	m_settings;

	QString		m_message;

	QGroupBox*	sourceGroup;
	QGroupBox*	transmitFilterGroup;
	QGroupBox*	pttOptionsGroup;

	QSpinBox*	highFilterSpinBox;
	QSpinBox*	lowFilterSpinBox;
	QSpinBox*	micGainMaxSpinBox;
	QSpinBox*	micGainMinSpinBox;

	AeroButton*	micInputBtn;
	AeroButton*	lineInputBtn;
	AeroButton*	micBoostBtn;

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;
	
private slots:
	void	inputButtonClicked();
	void	boostButtonClicked();
	
signals:
	void	showEvent(QObject *sender);
	void	closeEvent(QObject *sender);
	void	messageEvent(QString message);
};

#endif // _CUSDR_TX_WIDGET_H
