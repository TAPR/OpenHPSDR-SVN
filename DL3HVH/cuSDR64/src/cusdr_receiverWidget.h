/**
* @file  cusdr_receiverWidget.h
* @brief receiver widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-11-22
*/

/*
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
 
#ifndef CUSDR_RECEIVER_WIDGET_H
#define CUSDR_RECEIVER_WIDGET_H

#include <QWidget>
#include <QGroupBox>
#include <QLineEdit>

#include "cusdr_settings.h"
#include "cusdr_buttons.h"

class MiniReceiverWidget;


class ReceiverWidget : public QWidget {

	Q_OBJECT

public:
	ReceiverWidget();
	~ReceiverWidget();

public slots:
	//void dspServerChanged();
	//void setClientConnected(QObject *sender, bool value);

protected:
	void closeEvent(QCloseEvent *event);
	void showEvent(QShowEvent *event);

private:
	QList<MiniReceiverWidget *> miniRxList;
	MiniReceiverWidget	*miniRx;

	//QGroupBox	*dspBtnGroup();
	//QGroupBox	*fftwIntGroup();
	//QGroupBox	*fftwExtGroup();

	//QHButton	*cudaBtn;
	//QHButton	*fftwIntBtn;
	//QHButton	*fftwExtBtn;
	//QHButton	*launchDSPBtn;

signals:
	void showEvent(QObject *sender);
	void closeEvent(QObject *sender);
};


class MiniReceiverWidget : public QWidget {

	Q_OBJECT

public:
	MiniReceiverWidget();
	~MiniReceiverWidget();

public slots:
	//void dspServerChanged();
	//void setClientConnected(QObject *sender, bool value);

protected:
	void closeEvent(QCloseEvent *event);
	void showEvent(QShowEvent *event);

private:
	
signals:
	void showEvent(QObject *sender);
	void closeEvent(QObject *sender);
};

#endif  // CUSDR_RECEIVER_WIDGET_H
