/**
* @file  cusdr_serverWidget.cpp
* @brief hpsdr server settings widget
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
#define LOG_SERVER_WIDGET

//#include <QtGui>
//#include <QDebug>
//#include <QScopedPointer>

#include "cusdr_serverWidget.h"

#define	btn_height		15
#define	btn_width		80
#define	btn_width2		52

ServerWidget::ServerWidget(QWidget *parent) 
	: QWidget(parent)
	, set(Settings::instance())
	, m_serverMode(Settings::instance()->getCurrentServerMode())
	, m_minimumWidgetWidth(Settings::instance()->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(Settings::instance()->getMinimumGroupBoxWidth())
	, m_btnSpacing(5)
{
	//setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 0, 4, 0);
	setMouseTracking(true);

	createServerNIGroup();
	
	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(0);
	hbox1->setMargin(0);
	hbox1->addStretch();
	hbox1->addWidget(serverNIGroupBox);

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(0);
	hbox2->setMargin(0);
	hbox2->addStretch();
	hbox2->addWidget(portAddressesGroup());

	mainLayout->addLayout(hbox1);
	mainLayout->addLayout(hbox2);
	mainLayout->addStretch();
		
	setLayout(mainLayout);

	setupConnections();
	setPorts();
}

ServerWidget::~ServerWidget() {

	// disconnect all signals
	disconnect(Settings::instance(), 0, this, 0);
	disconnect(this, 0, 0, 0);
}

QSize ServerWidget::sizeHint() const {
	
	return QSize(m_minimumWidgetWidth, height());
}

QSize ServerWidget::minimumSizeHint() const {

	return QSize(m_minimumWidgetWidth, height());
}

void ServerWidget::setupConnections() {

	CHECKED_CONNECT(
		set, 
		SIGNAL(newServerNetworkInterface(QString, QString)), 
		this, 
		SLOT(addServerNIEntry(QString, QString)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(serverNICChanged(int)), 
		this, 
		SLOT(setServerNIC(int)));
}

void ServerWidget::addNICChangedConnection() {

	CHECKED_CONNECT(
		serverNetworkInterfaces, 
		SIGNAL(currentIndexChanged(int)), 
		set,
		SLOT(setServerNetworkInterface(int)));
}

void ServerWidget::createServerNIGroup() {

	serverNetworkInterfaces = new QComboBox();

	serverNetworkInterfaces->setStyleSheet(Settings::instance()->getComboBoxStyle());
	serverNetworkInterfaces->setMinimumContentsLength(22);

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(1);
	hbox1->addWidget(serverNetworkInterfaces);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(5);
	vbox->addSpacing(5);
	vbox->addLayout(hbox1);
	vbox->addSpacing(5);
	//vbox->addLayout(hbox2);
	
	serverNIGroupBox = new QGroupBox(tr("Server network interface"));
	serverNIGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	serverNIGroupBox->setLayout(vbox);
	serverNIGroupBox->setStyleSheet(Settings::instance()->getWidgetStyle());
	serverNIGroupBox->setFont(QFont("Arial", 8));
}

void ServerWidget::addServerNIEntry(QString niName, QString ipAddress) {

	//QString item = niName;
	QString item = ipAddress;
	//item.append(" (");
	//item.append(ipAddress);
	//item.append(")");
	serverNetworkInterfaces->addItem(item);
}

QGroupBox *ServerWidget::portAddressesGroup() {

	portGridBox = new QGridLayout;
	portGridBox->setVerticalSpacing(3);
	//portGridBox->setHorizontalSpacing(43);

	labelServerPortLabel = new QLabel("Command Server Port:");
    labelServerPortLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	labelServerPortLabel->setStyleSheet(Settings::instance()->getLabelStyle());
	portGridBox->addWidget(labelServerPortLabel, 0, 0);

	labelServerPortText = new QLabel("");
    labelServerPortText->setFrameStyle(QFrame::Box | QFrame::Raised);
	labelServerPortText->setStyleSheet(Settings::instance()->getLabelStyle());
	portGridBox->addWidget(labelServerPortText, 0, 1);

	labelListenerPortLabel = new QLabel("Listener Port:");
    labelListenerPortLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	labelListenerPortLabel->setStyleSheet(Settings::instance()->getLabelStyle());
	portGridBox->addWidget(labelListenerPortLabel, 1, 0);

	labelListenerPortText = new QLabel("");
    labelListenerPortText->setFrameStyle(QFrame::Box | QFrame::Raised);
	labelListenerPortText->setStyleSheet(Settings::instance()->getLabelStyle());
	portGridBox->addWidget(labelListenerPortText, 1, 1);

	labelAudioPortLabel = new QLabel("Audio Port:");
    labelAudioPortLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	labelAudioPortLabel->setStyleSheet(Settings::instance()->getLabelStyle());
	portGridBox->addWidget(labelAudioPortLabel, 2, 0);

	labelAudioPortText = new QLabel("");
    labelAudioPortText->setFrameStyle(QFrame::Box | QFrame::Raised);
	labelAudioPortText->setStyleSheet(Settings::instance()->getLabelStyle());
	portGridBox->addWidget(labelAudioPortText, 2, 1);

	/*le_server_port = new QLineEdit(QString::number(Settings::instance()->serverPort()), this);
	le_server_port->setFont(font());
	le_server_port->setFixedSize(50, QFontMetrics(le_server_port->font()).height() + 4);
	le_server_port->setInputMask("00000;");
	le_server_port->setMaxLength(7);
	le_server_port->setStyleSheet(lineedit_style);*/
	
	/*QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(1);
	hbox1->addWidget(le_server_port);
	hbox1->addStretch();*/

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(1);
	vbox->addLayout(portGridBox);

	QGroupBox *groupBox = new QGroupBox(tr("Port Addresses"));
	groupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	groupBox->setLayout(vbox);
	groupBox->setStyleSheet(Settings::instance()->getWidgetStyle());
	groupBox->setFont(QFont("Arial", 8));

	return groupBox;
}

QGroupBox *ServerWidget::serverPortAddressGroup() {

	le_server_port = new QLineEdit(QString::number(Settings::instance()->getServerPort()), this);
	le_server_port->setFont(font());
	le_server_port->setFixedSize(50, QFontMetrics(le_server_port->font()).height() + 4);
	le_server_port->setInputMask("00000;");
	le_server_port->setMaxLength(7);
	le_server_port->setStyleSheet(lineedit_style);
	
	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(1);
	hbox1->addWidget(le_server_port);
	hbox1->addStretch();

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(1);
	vbox->addLayout(hbox1);

	QGroupBox *groupBox = new QGroupBox(tr("Command Server port"));
	groupBox->setLayout(vbox);
	groupBox->setStyleSheet(Settings::instance()->getWidgetStyle());
	groupBox->setFont(QFont("Arial", 8));

	return groupBox;
}

QGroupBox *ServerWidget::listenerPortAddressGroup() {

	le_listener_port = new QLineEdit(QString::number(Settings::instance()->getListenPort()), this);
	le_listener_port->setFont(font());
	le_listener_port->setFixedSize(50, le_server_port->height());
	le_listener_port->setInputMask("00000;");
	le_listener_port->setMaxLength(7);
	le_listener_port->setStyleSheet(lineedit_style);
	
	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(1);
	hbox1->addWidget(le_listener_port);
	hbox1->addStretch();

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(1);
	vbox->addLayout(hbox1);

	QGroupBox *groupBox = new QGroupBox(tr("Listener port"));
	groupBox->setLayout(vbox);
	groupBox->setStyleSheet(Settings::instance()->getWidgetStyle());
	groupBox->setFont(QFont("Arial", 8));

	return groupBox;
}

QGroupBox *ServerWidget::audioPortAddressGroup() {

	le_audio_port = new QLineEdit(QString::number(Settings::instance()->getAudioPort()), this);
	le_audio_port->setFont(font());
	le_audio_port->setFixedSize(50, le_server_port->height());
	le_audio_port->setInputMask("00000;");
	le_audio_port->setMaxLength(7);
	le_audio_port->setStyleSheet(lineedit_style);
	
	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(1);
	hbox1->addWidget(le_audio_port);
	hbox1->addStretch();

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(1);
	vbox->addLayout(hbox1);

	QGroupBox *groupBox = new QGroupBox(tr("Audio port"));
	groupBox->setLayout(vbox);
	groupBox->setStyleSheet(Settings::instance()->getWidgetStyle());
	groupBox->setFont(QFont("Arial", 8));

	return groupBox;
}


 
// ***************************************************************

void ServerWidget::setServerNIC(int index) {

	serverNetworkInterfaces->setCurrentIndex(index);
}

void ServerWidget::setPorts() {

	labelServerPortText->setText(QString::number(Settings::instance()->getServerPort()));
	labelListenerPortText->setText(QString::number(Settings::instance()->getListenPort()));
	labelAudioPortText->setText(QString::number(Settings::instance()->getAudioPort()));
}

void ServerWidget::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void ServerWidget::showEvent(QShowEvent *event) {

	//emit showEvent(this);
	
	QWidget::showEvent(event);
}

void ServerWidget::portChanged(const QString &text) {

	bool ok = false;
	int port = text.toInt(&ok);
	if (!ok || port < 0 || port >= 65536) return;
	//Settings::instance()->setServerPort(port_le, port);
}

