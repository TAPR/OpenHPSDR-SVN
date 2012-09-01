/**
* @file  cusdr_serverWidget.h
* @brief header file for hpsdr server settings widget
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-09-21
*/

/*
 *   
 *   Copyright 2010 Hermann von Hasseln, DL3HVH
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
 
#ifndef _CUSDR_SERVER_WIDGET_H
#define _CUSDR_SERVER_WIDGET_H

#include <QWidget>
#include <QComboBox>
#include <QGroupBox>
#include <QSpinBox>
#include <QLineEdit>

#include "cusdr_buttons.h"
#include "cusdr_settings.h"


class ServerWidget : public QWidget {

	Q_OBJECT

public:
	ServerWidget(QWidget *parent = 0);
	~ServerWidget();


public slots:
	QSize	sizeHint() const;
	QSize	minimumSizeHint() const;
	void	addServerNIEntry(QString niName, QString ipAddress);
	void	addNICChangedConnection();
	
protected:
	void	closeEvent(QCloseEvent *event);
	void	showEvent(QShowEvent *event);

private:
	Settings*		m_settings;

	QStringList		niList;
	QTableWidget*	serverNITable;

	QGroupBox*		portAddressesGroup();
	QGroupBox*		serverPortAddressGroup();
	QGroupBox*		listenerPortAddressGroup();
	QGroupBox*		audioPortAddressGroup();
	QGroupBox*		serverNIGroupBox;

	QGridLayout*	portGridBox;
	QComboBox*		serverNetworkInterfaces;
	
	QLabel*			labelServerPortLabel;
	QLabel*			labelServerPortText;
	QLabel*			labelListenerPortLabel;
	QLabel*			labelListenerPortText;
	QLabel*			labelAudioPortLabel;
	QLabel*			labelAudioPortText;

	QLineEdit*		le_server_address;
	QLineEdit*		le_server_port;
	QLineEdit*		le_listener_port;
	QLineEdit*		le_audio_port;

	QString			lineedit_style;

	QSDR::_ServerMode		m_serverMode;
	
	QFont	m_titleFont;

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;
	int		m_btnSpacing;

	void	createServerNIGroup();

private slots:
	void	setPorts();
	void	setupConnections();
	void	portChanged(const QString &text);
	void	setServerNIC(int index);
	
signals:
	void	showEvent(QObject *sender);
	void	closeEvent(QObject *sender);
	void	messageEvent(QString );
};

#endif // _CUSDR_SERVER_WIDGET_H
