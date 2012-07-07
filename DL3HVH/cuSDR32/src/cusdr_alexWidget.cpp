/**
* @file  cusdr_alexWidget.cpp
* @brief Alexiares filter settings widget class for cuSDR
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

#define LOG_ALEX_WIDGET


#include <QtGui>
#include <QPen>
#include <QDebug>
#include <QScopedPointer>

#include "cusdr_alexWidget.h"

#define	btn_height		15
#define	btn_width		20
#define	btn_width2		28
#define	btn_width3		32

AlexWidget::AlexWidget(QWidget *parent) 
	: QWidget(parent)
	, m_settings(Settings::instance())
	, m_minimumWidgetWidth(m_settings->getMinimumWidgetWidth())
	//, m_minimumGroupBoxWidth(m_settings->getMinimumGroupBoxWidth())
	, m_minimumGroupBoxWidth(0)
{
	setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 8, 4, 0);
	setMouseTracking(true);

	//QFont titleFont;
	m_normalFont.setStyleStrategy(QFont::PreferQuality);
	m_normalFont.setFixedPitch(true);
	#ifdef Q_OS_MAC
		m_titleFont.setPixelSize(11);
		m_titleFont.setFamily("Arial");
		//m_smallFont.setBold(true);
	#else if Q_OS_WIN
		m_normalFont.setPixelSize(11);
		m_normalFont.setFamily("Arial");
		//m_smallFont.setBold(true);
		//m_smallFont.setItalic(true);
	#endif

	m_smallFont.setStyleStrategy(QFont::PreferAntialias);
	m_smallFont.setFixedPitch(true);
	#ifdef Q_OS_MAC
		m_smallFont.setPixelSize(9);
		m_smallFont.setFamily("Arial");
		//m_smallFont.setBold(true);
	#else if Q_OS_WIN
		m_smallFont.setPixelSize(9);
		m_smallFont.setFamily("Arial");
		//m_smallFont.setBold(true);
		//m_smallFont.setItalic(true);
	#endif

	//**************************************************************
	// create groups
	createReceiveGroup();
	createTransmitGroup();

	// set main layout
	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(0);
	hbox1->setContentsMargins(4, 0, 4, 0);
	hbox1->addWidget(receiveGroup);

	QHBoxLayout *hbox2 = new QHBoxLayout;
	hbox2->setSpacing(0);
	hbox2->setContentsMargins(4, 0, 4, 0);
	hbox2->addWidget(transmitGroup);

	mainLayout->addLayout(hbox1);
	mainLayout->addLayout(hbox2);
	mainLayout->addStretch();
		
	setLayout(mainLayout);

	setupConnections();
}

AlexWidget::~AlexWidget() {

	// disconnect all signals
	disconnect(m_settings, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

void AlexWidget::setupConnections() {

	/*CHECKED_CONNECT(
		m_settings,
		SIGNAL(audioBufferChanged(QObject *, qint64, qint64, const QByteArray)),
		this,
		SLOT(chirpBufferChanged(QObject *, qint64, qint64, const QByteArray)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(lowerChirpFreqChanged(QObject *, int)), 
		this, 
		SLOT(setChirpLowerFrequency(QObject *, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(upperChirpFreqChanged(QObject *, int)), 
		this, 
		SLOT(setChirpUpperFrequency(QObject *, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(chirpBufferDurationUsChanged(QObject *, qint64)), 
		this, 
		SLOT(setChirpBufferDurationUs(QObject *, qint64)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(chirpRepetitionTimesChanged(QObject *, int)), 
		this, 
		SLOT(setChirpRepetitionTimes(QObject *, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(chirpSpectrumChanged(qint64, qint64, const FrequencySpectrum &)),
        this, 
		SLOT(chirpSpectrumChanged(qint64, qint64, const FrequencySpectrum &)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(chirpSpectrumListChanged(const QList<FrequencySpectrum> &)),
        this, 
		SLOT(chirpSpectrumListChanged(const QList<FrequencySpectrum> &)));*/	
}

void AlexWidget::createReceiveGroup() {

	QLabel* emptyLabel = new QLabel(" ", this);
    emptyLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	emptyLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* antLabel = new QLabel("Ant", this);
    antLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	antLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* rxAuxLabel = new QLabel("Rx Aux", this);
    rxAuxLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	rxAuxLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band160mLabel = new QLabel("160m", this);
    band160mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band160mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band80mLabel = new QLabel("80m", this);
    band80mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band80mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band60mLabel = new QLabel("60m", this);
    band60mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band60mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band40mLabel = new QLabel("40m", this);
    band40mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band40mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band30mLabel = new QLabel("30m", this);
    band30mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band30mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band20mLabel = new QLabel("20m", this);
    band20mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band20mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band17mLabel = new QLabel("17m", this);
    band17mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band17mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band15mLabel = new QLabel("15m", this);
    band15mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band15mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band12mLabel = new QLabel("12m", this);
    band12mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band12mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band10mLabel = new QLabel("10m", this);
    band10mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band10mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band6mLabel = new QLabel("6m", this);
    band6mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band6mLabel->setStyleSheet(m_settings->getLabelStyle());

	// Ant1
	band160mBtn1 = new AeroButton("1", this);
	band160mBtn1->setRoundness(0);
	band160mBtn1->setFixedSize (btn_width, btn_height);
	band160mBtn1->setStyleSheet(m_settings->getMiniButtonStyle());
	band160mBtnList.append(band160mBtn1);

	CHECKED_CONNECT(
		band160mBtn1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band160mBtnClicked()));

	band80mBtn1 = new AeroButton("1", this);
	band80mBtn1->setRoundness(0);
	band80mBtn1->setFixedSize (btn_width, btn_height);
	band80mBtn1->setStyleSheet(m_settings->getMiniButtonStyle());
	band80mBtnList.append(band80mBtn1);

	CHECKED_CONNECT(
		band80mBtn1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band80mBtnClicked()));

	band60mBtn1 = new AeroButton("1", this);
	band60mBtn1->setRoundness(0);
	band60mBtn1->setFixedSize (btn_width, btn_height);
	band60mBtn1->setStyleSheet(m_settings->getMiniButtonStyle());
	band60mBtnList.append(band60mBtn1);

	CHECKED_CONNECT(
		band60mBtn1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band60mBtnClicked()));

	band40mBtn1 = new AeroButton("1", this);
	band40mBtn1->setRoundness(0);
	band40mBtn1->setFixedSize (btn_width, btn_height);
	band40mBtn1->setStyleSheet(m_settings->getMiniButtonStyle());
	band40mBtnList.append(band40mBtn1);

	CHECKED_CONNECT(
		band40mBtn1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band40mBtnClicked()));

	band30mBtn1 = new AeroButton("1", this);
	band30mBtn1->setRoundness(0);
	band30mBtn1->setFixedSize (btn_width, btn_height);
	band30mBtn1->setStyleSheet(m_settings->getMiniButtonStyle());
	band30mBtnList.append(band30mBtn1);

	CHECKED_CONNECT(
		band30mBtn1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band30mBtnClicked()));

	band20mBtn1 = new AeroButton("1", this);
	band20mBtn1->setRoundness(0);
	band20mBtn1->setFixedSize (btn_width, btn_height);
	band20mBtn1->setStyleSheet(m_settings->getMiniButtonStyle());
	band20mBtnList.append(band20mBtn1);

	CHECKED_CONNECT(
		band20mBtn1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band20mBtnClicked()));

	band17mBtn1 = new AeroButton("1", this);
	band17mBtn1->setRoundness(0);
	band17mBtn1->setFixedSize (btn_width, btn_height);
	band17mBtn1->setStyleSheet(m_settings->getMiniButtonStyle());
	band17mBtnList.append(band17mBtn1);

	CHECKED_CONNECT(
		band17mBtn1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band17mBtnClicked()));

	band15mBtn1 = new AeroButton("1", this);
	band15mBtn1->setRoundness(0);
	band15mBtn1->setFixedSize (btn_width, btn_height);
	band15mBtn1->setStyleSheet(m_settings->getMiniButtonStyle());
	band15mBtnList.append(band15mBtn1);

	CHECKED_CONNECT(
		band15mBtn1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band15mBtnClicked()));

	band12mBtn1 = new AeroButton("1", this);
	band12mBtn1->setRoundness(0);
	band12mBtn1->setFixedSize (btn_width, btn_height);
	band12mBtn1->setStyleSheet(m_settings->getMiniButtonStyle());
	band12mBtnList.append(band12mBtn1);

	CHECKED_CONNECT(
		band12mBtn1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band12mBtnClicked()));

	band10mBtn1 = new AeroButton("1", this);
	band10mBtn1->setRoundness(0);
	band10mBtn1->setFixedSize (btn_width, btn_height);
	band10mBtn1->setStyleSheet(m_settings->getMiniButtonStyle());
	band10mBtnList.append(band10mBtn1);

	CHECKED_CONNECT(
		band10mBtn1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band10mBtnClicked()));

	band6mBtn1 = new AeroButton("1", this);
	band6mBtn1->setRoundness(0);
	band6mBtn1->setFixedSize (btn_width, btn_height);
	band6mBtn1->setStyleSheet(m_settings->getMiniButtonStyle());
	band6mBtnList.append(band6mBtn1);

	CHECKED_CONNECT(
		band6mBtn1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band6mBtnClicked()));

	// Ant2
	band160mBtn2 = new AeroButton("2", this);
	band160mBtn2->setRoundness(0);
	band160mBtn2->setFixedSize (btn_width, btn_height);
	band160mBtn2->setStyleSheet(m_settings->getMiniButtonStyle());
	band160mBtnList.append(band160mBtn2);

	CHECKED_CONNECT(
		band160mBtn2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band160mBtnClicked()));

	band80mBtn2 = new AeroButton("2", this);
	band80mBtn2->setRoundness(0);
	band80mBtn2->setFixedSize (btn_width, btn_height);
	band80mBtn2->setStyleSheet(m_settings->getMiniButtonStyle());
	band80mBtnList.append(band80mBtn2);

	CHECKED_CONNECT(
		band80mBtn2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band80mBtnClicked()));

	band60mBtn2 = new AeroButton("2", this);
	band60mBtn2->setRoundness(0);
	band60mBtn2->setFixedSize (btn_width, btn_height);
	band60mBtn2->setStyleSheet(m_settings->getMiniButtonStyle());
	band60mBtnList.append(band60mBtn2);

	CHECKED_CONNECT(
		band60mBtn2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band60mBtnClicked()));

	band40mBtn2 = new AeroButton("2", this);
	band40mBtn2->setRoundness(0);
	band40mBtn2->setFixedSize (btn_width, btn_height);
	band40mBtn2->setStyleSheet(m_settings->getMiniButtonStyle());
	band40mBtnList.append(band40mBtn2);

	CHECKED_CONNECT(
		band40mBtn2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band40mBtnClicked()));

	band30mBtn2 = new AeroButton("2", this);
	band30mBtn2->setRoundness(0);
	band30mBtn2->setFixedSize (btn_width, btn_height);
	band30mBtn2->setStyleSheet(m_settings->getMiniButtonStyle());
	band30mBtnList.append(band30mBtn2);

	CHECKED_CONNECT(
		band30mBtn2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band30mBtnClicked()));

	band20mBtn2 = new AeroButton("2", this);
	band20mBtn2->setRoundness(0);
	band20mBtn2->setFixedSize (btn_width, btn_height);
	band20mBtn2->setStyleSheet(m_settings->getMiniButtonStyle());
	band20mBtnList.append(band20mBtn2);

	CHECKED_CONNECT(
		band20mBtn2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band20mBtnClicked()));

	band17mBtn2 = new AeroButton("2", this);
	band17mBtn2->setRoundness(0);
	band17mBtn2->setFixedSize (btn_width, btn_height);
	band17mBtn2->setStyleSheet(m_settings->getMiniButtonStyle());
	band17mBtnList.append(band17mBtn2);

	CHECKED_CONNECT(
		band17mBtn2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band17mBtnClicked()));

	band15mBtn2 = new AeroButton("2", this);
	band15mBtn2->setRoundness(0);
	band15mBtn2->setFixedSize (btn_width, btn_height);
	band15mBtn2->setStyleSheet(m_settings->getMiniButtonStyle());
	band15mBtnList.append(band15mBtn2);

	CHECKED_CONNECT(
		band15mBtn2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band15mBtnClicked()));

	band12mBtn2 = new AeroButton("2", this);
	band12mBtn2->setRoundness(0);
	band12mBtn2->setFixedSize (btn_width, btn_height);
	band12mBtn2->setStyleSheet(m_settings->getMiniButtonStyle());
	band12mBtnList.append(band12mBtn2);

	CHECKED_CONNECT(
		band12mBtn2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band12mBtnClicked()));

	band10mBtn2 = new AeroButton("2", this);
	band10mBtn2->setRoundness(0);
	band10mBtn2->setFixedSize (btn_width, btn_height);
	band10mBtn2->setStyleSheet(m_settings->getMiniButtonStyle());
	band10mBtnList.append(band10mBtn2);

	CHECKED_CONNECT(
		band10mBtn2,
		SIGNAL(clicked()), 
		this, 
		SLOT(band10mBtnClicked()));

	band6mBtn2 = new AeroButton("2", this);
	band6mBtn2->setRoundness(0);
	band6mBtn2->setFixedSize (btn_width, btn_height);
	band6mBtn2->setStyleSheet(m_settings->getMiniButtonStyle());
	band6mBtnList.append(band6mBtn2);

	CHECKED_CONNECT(
		band6mBtn2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band6mBtnClicked()));

	// Ant3
	band160mBtn3 = new AeroButton("3", this);
	band160mBtn3->setRoundness(0);
	band160mBtn3->setFixedSize (btn_width, btn_height);
	band160mBtn3->setStyleSheet(m_settings->getMiniButtonStyle());
	band160mBtnList.append(band160mBtn3);
	band160mBtnList.append(band160mBtn3);

	CHECKED_CONNECT(
		band160mBtn3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band160mBtnClicked()));

	band80mBtn3 = new AeroButton("3", this);
	band80mBtn3->setRoundness(0);
	band80mBtn3->setFixedSize (btn_width, btn_height);
	band80mBtn3->setStyleSheet(m_settings->getMiniButtonStyle());
	band80mBtnList.append(band80mBtn3);

	CHECKED_CONNECT(
		band80mBtn3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band80mBtnClicked()));

	band60mBtn3 = new AeroButton("3", this);
	band60mBtn3->setRoundness(0);
	band60mBtn3->setFixedSize (btn_width, btn_height);
	band60mBtn3->setStyleSheet(m_settings->getMiniButtonStyle());
	band60mBtnList.append(band60mBtn3);

	CHECKED_CONNECT(
		band60mBtn3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band60mBtnClicked()));

	band40mBtn3 = new AeroButton("3", this);
	band40mBtn3->setRoundness(0);
	band40mBtn3->setFixedSize (btn_width, btn_height);
	band40mBtn3->setStyleSheet(m_settings->getMiniButtonStyle());
	band40mBtnList.append(band40mBtn3);

	CHECKED_CONNECT(
		band40mBtn3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band40mBtnClicked()));

	band30mBtn3 = new AeroButton("3", this);
	band30mBtn3->setRoundness(0);
	band30mBtn3->setFixedSize (btn_width, btn_height);
	band30mBtn3->setStyleSheet(m_settings->getMiniButtonStyle());
	band30mBtnList.append(band30mBtn3);

	CHECKED_CONNECT(
		band30mBtn3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band30mBtnClicked()));

	band20mBtn3 = new AeroButton("3", this);
	band20mBtn3->setRoundness(0);
	band20mBtn3->setFixedSize (btn_width, btn_height);
	band20mBtn3->setStyleSheet(m_settings->getMiniButtonStyle());
	band20mBtnList.append(band20mBtn3);

	CHECKED_CONNECT(
		band20mBtn3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band20mBtnClicked()));

	band17mBtn3 = new AeroButton("3", this);
	band17mBtn3->setRoundness(0);
	band17mBtn3->setFixedSize (btn_width, btn_height);
	band17mBtn3->setStyleSheet(m_settings->getMiniButtonStyle());
	band17mBtnList.append(band17mBtn3);

	CHECKED_CONNECT(
		band17mBtn3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band17mBtnClicked()));

	band15mBtn3 = new AeroButton("3", this);
	band15mBtn3->setRoundness(0);
	band15mBtn3->setFixedSize (btn_width, btn_height);
	band15mBtn3->setStyleSheet(m_settings->getMiniButtonStyle());
	band15mBtnList.append(band15mBtn3);

	CHECKED_CONNECT(
		band15mBtn3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band15mBtnClicked()));

	band12mBtn3 = new AeroButton("3", this);
	band12mBtn3->setRoundness(0);
	band12mBtn3->setFixedSize (btn_width, btn_height);
	band12mBtn3->setStyleSheet(m_settings->getMiniButtonStyle());
	band12mBtnList.append(band12mBtn3);

	CHECKED_CONNECT(
		band12mBtn3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band12mBtnClicked()));

	band10mBtn3 = new AeroButton("3", this);
	band10mBtn3->setRoundness(0);
	band10mBtn3->setFixedSize (btn_width, btn_height);
	band10mBtn3->setStyleSheet(m_settings->getMiniButtonStyle());
	band10mBtnList.append(band10mBtn3);

	CHECKED_CONNECT(
		band10mBtn3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band10mBtnClicked()));

	band6mBtn3 = new AeroButton("3", this);
	band6mBtn3->setRoundness(0);
	band6mBtn3->setFixedSize (btn_width, btn_height);
	band6mBtn3->setStyleSheet(m_settings->getMiniButtonStyle());
	band6mBtnList.append(band6mBtn3);

	CHECKED_CONNECT(
		band6mBtn3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band6mBtnClicked()));


	// Rx1
	band160mBtnRx1 = new AeroButton("Rx 1", this);
	band160mBtnRx1->setRoundness(0);
	band160mBtnRx1->setFixedSize (btn_width2, btn_height);
	band160mBtnRx1->setStyleSheet(m_settings->getMiniButtonStyle());
	band160mBtnRxList.append(band160mBtnRx1);

	CHECKED_CONNECT(
		band160mBtnRx1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band160mBtnRxClicked()));

	band80mBtnRx1 = new AeroButton("Rx 1", this);
	band80mBtnRx1->setRoundness(0);
	band80mBtnRx1->setFixedSize (btn_width2, btn_height);
	band80mBtnRx1->setStyleSheet(m_settings->getMiniButtonStyle());
	band80mBtnRxList.append(band80mBtnRx1);

	CHECKED_CONNECT(
		band80mBtnRx1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band80mBtnRxClicked()));

	band60mBtnRx1 = new AeroButton("Rx 1", this);
	band60mBtnRx1->setRoundness(0);
	band60mBtnRx1->setFixedSize (btn_width2, btn_height);
	band60mBtnRx1->setStyleSheet(m_settings->getMiniButtonStyle());
	band60mBtnRxList.append(band60mBtnRx1);

	CHECKED_CONNECT(
		band60mBtnRx1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band60mBtnRxClicked()));

	band40mBtnRx1 = new AeroButton("Rx 1", this);
	band40mBtnRx1->setRoundness(0);
	band40mBtnRx1->setFixedSize (btn_width2, btn_height);
	band40mBtnRx1->setStyleSheet(m_settings->getMiniButtonStyle());
	band40mBtnRxList.append(band40mBtnRx1);

	CHECKED_CONNECT(
		band40mBtnRx1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band40mBtnRxClicked()));

	band30mBtnRx1 = new AeroButton("Rx 1", this);
	band30mBtnRx1->setRoundness(0);
	band30mBtnRx1->setFixedSize (btn_width2, btn_height);
	band30mBtnRx1->setStyleSheet(m_settings->getMiniButtonStyle());
	band30mBtnRxList.append(band30mBtnRx1);

	CHECKED_CONNECT(
		band30mBtnRx1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band30mBtnRxClicked()));

	band20mBtnRx1 = new AeroButton("Rx 1", this);
	band20mBtnRx1->setRoundness(0);
	band20mBtnRx1->setFixedSize (btn_width2, btn_height);
	band20mBtnRx1->setStyleSheet(m_settings->getMiniButtonStyle());
	band20mBtnRxList.append(band20mBtnRx1);

	CHECKED_CONNECT(
		band20mBtnRx1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band20mBtnRxClicked()));

	band17mBtnRx1 = new AeroButton("Rx 1", this);
	band17mBtnRx1->setRoundness(0);
	band17mBtnRx1->setFixedSize (btn_width2, btn_height);
	band17mBtnRx1->setStyleSheet(m_settings->getMiniButtonStyle());
	band17mBtnRxList.append(band17mBtnRx1);

	CHECKED_CONNECT(
		band17mBtnRx1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band17mBtnRxClicked()));

	band15mBtnRx1 = new AeroButton("Rx 1", this);
	band15mBtnRx1->setRoundness(0);
	band15mBtnRx1->setFixedSize (btn_width2, btn_height);
	band15mBtnRx1->setStyleSheet(m_settings->getMiniButtonStyle());
	band15mBtnRxList.append(band15mBtnRx1);

	CHECKED_CONNECT(
		band15mBtnRx1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band15mBtnRxClicked()));

	band12mBtnRx1 = new AeroButton("Rx 1", this);
	band12mBtnRx1->setRoundness(0);
	band12mBtnRx1->setFixedSize (btn_width2, btn_height);
	band12mBtnRx1->setStyleSheet(m_settings->getMiniButtonStyle());
	band12mBtnRxList.append(band12mBtnRx1);

	CHECKED_CONNECT(
		band12mBtnRx1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band12mBtnRxClicked()));

	band10mBtnRx1 = new AeroButton("Rx 1", this);
	band10mBtnRx1->setRoundness(0);
	band10mBtnRx1->setFixedSize (btn_width2, btn_height);
	band10mBtnRx1->setStyleSheet(m_settings->getMiniButtonStyle());
	band10mBtnRxList.append(band10mBtnRx1);

	CHECKED_CONNECT(
		band10mBtnRx1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band10mBtnRxClicked()));

	band6mBtnRx1 = new AeroButton("Rx 1", this);
	band6mBtnRx1->setRoundness(0);
	band6mBtnRx1->setFixedSize (btn_width2, btn_height);
	band6mBtnRx1->setStyleSheet(m_settings->getMiniButtonStyle());
	band6mBtnRxList.append(band6mBtnRx1);

	CHECKED_CONNECT(
		band6mBtnRx1, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band6mBtnRxClicked()));

	// Rx2
	band160mBtnRx2 = new AeroButton("Rx 2", this);
	band160mBtnRx2->setRoundness(0);
	band160mBtnRx2->setFixedSize (btn_width2, btn_height);
	band160mBtnRx2->setStyleSheet(m_settings->getMiniButtonStyle());
	band160mBtnRxList.append(band160mBtnRx2);

	CHECKED_CONNECT(
		band160mBtnRx2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band160mBtnRxClicked()));

	band80mBtnRx2 = new AeroButton("Rx 2", this);
	band80mBtnRx2->setRoundness(0);
	band80mBtnRx2->setFixedSize (btn_width2, btn_height);
	band80mBtnRx2->setStyleSheet(m_settings->getMiniButtonStyle());
	band80mBtnRxList.append(band80mBtnRx2);

	CHECKED_CONNECT(
		band80mBtnRx2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band80mBtnRxClicked()));

	band60mBtnRx2 = new AeroButton("Rx 2", this);
	band60mBtnRx2->setRoundness(0);
	band60mBtnRx2->setFixedSize (btn_width2, btn_height);
	band60mBtnRx2->setStyleSheet(m_settings->getMiniButtonStyle());
	band60mBtnRxList.append(band60mBtnRx2);

	CHECKED_CONNECT(
		band60mBtnRx2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band60mBtnRxClicked()));

	band40mBtnRx2 = new AeroButton("Rx 2", this);
	band40mBtnRx2->setRoundness(0);
	band40mBtnRx2->setFixedSize (btn_width2, btn_height);
	band40mBtnRx2->setStyleSheet(m_settings->getMiniButtonStyle());
	band40mBtnRxList.append(band40mBtnRx2);

	CHECKED_CONNECT(
		band40mBtnRx2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band40mBtnRxClicked()));

	band30mBtnRx2 = new AeroButton("Rx 2", this);
	band30mBtnRx2->setRoundness(0);
	band30mBtnRx2->setFixedSize (btn_width2, btn_height);
	band30mBtnRx2->setStyleSheet(m_settings->getMiniButtonStyle());
	band30mBtnRxList.append(band30mBtnRx2);

	CHECKED_CONNECT(
		band30mBtnRx2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band30mBtnRxClicked()));

	band20mBtnRx2 = new AeroButton("Rx 2", this);
	band20mBtnRx2->setRoundness(0);
	band20mBtnRx2->setFixedSize (btn_width2, btn_height);
	band20mBtnRx2->setStyleSheet(m_settings->getMiniButtonStyle());
	band20mBtnRxList.append(band20mBtnRx2);

	CHECKED_CONNECT(
		band20mBtnRx2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band20mBtnRxClicked()));

	band17mBtnRx2 = new AeroButton("Rx 2", this);
	band17mBtnRx2->setRoundness(0);
	band17mBtnRx2->setFixedSize (btn_width2, btn_height);
	band17mBtnRx2->setStyleSheet(m_settings->getMiniButtonStyle());
	band17mBtnRxList.append(band17mBtnRx2);

	CHECKED_CONNECT(
		band17mBtnRx2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band17mBtnRxClicked()));

	band15mBtnRx2 = new AeroButton("Rx 2", this);
	band15mBtnRx2->setRoundness(0);
	band15mBtnRx2->setFixedSize (btn_width2, btn_height);
	band15mBtnRx2->setStyleSheet(m_settings->getMiniButtonStyle());
	band15mBtnRxList.append(band15mBtnRx2);

	CHECKED_CONNECT(
		band15mBtnRx2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band15mBtnRxClicked()));

	band12mBtnRx2 = new AeroButton("Rx 2", this);
	band12mBtnRx2->setRoundness(0);
	band12mBtnRx2->setFixedSize (btn_width2, btn_height);
	band12mBtnRx2->setStyleSheet(m_settings->getMiniButtonStyle());
	band12mBtnRxList.append(band12mBtnRx2);

	CHECKED_CONNECT(
		band12mBtnRx2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band12mBtnRxClicked()));

	band10mBtnRx2 = new AeroButton("Rx 2", this);
	band10mBtnRx2->setRoundness(0);
	band10mBtnRx2->setFixedSize (btn_width2, btn_height);
	band10mBtnRx2->setStyleSheet(m_settings->getMiniButtonStyle());
	band10mBtnRxList.append(band10mBtnRx2);

	CHECKED_CONNECT(
		band10mBtnRx2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band10mBtnRxClicked()));

	band6mBtnRx2 = new AeroButton("Rx 2", this);
	band6mBtnRx2->setRoundness(0);
	band6mBtnRx2->setFixedSize (btn_width2, btn_height);
	band6mBtnRx2->setStyleSheet(m_settings->getMiniButtonStyle());
	band6mBtnRxList.append(band6mBtnRx2);

	CHECKED_CONNECT(
		band6mBtnRx2, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band6mBtnRxClicked()));

	// XVTR
	band160mBtnRx3 = new AeroButton("XVTR", this);
	band160mBtnRx3->setRoundness(0);
	band160mBtnRx3->setFixedSize (btn_width3, btn_height);
	band160mBtnRx3->setStyleSheet(m_settings->getMiniButtonStyle());
	band160mBtnRxList.append(band160mBtnRx3);

	CHECKED_CONNECT(
		band160mBtnRx3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band160mBtnRxClicked()));

	band80mBtnRx3 = new AeroButton("XVTR", this);
	band80mBtnRx3->setRoundness(0);
	band80mBtnRx3->setFixedSize (btn_width3, btn_height);
	band80mBtnRx3->setStyleSheet(m_settings->getMiniButtonStyle());
	band80mBtnRxList.append(band80mBtnRx3);

	CHECKED_CONNECT(
		band80mBtnRx3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band80mBtnRxClicked()));

	band60mBtnRx3 = new AeroButton("XVTR", this);
	band60mBtnRx3->setRoundness(0);
	band60mBtnRx3->setFixedSize (btn_width3, btn_height);
	band60mBtnRx3->setStyleSheet(m_settings->getMiniButtonStyle());
	band60mBtnRxList.append(band60mBtnRx3);

	CHECKED_CONNECT(
		band60mBtnRx3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band60mBtnRxClicked()));

	band40mBtnRx3 = new AeroButton("XVTR", this);
	band40mBtnRx3->setRoundness(0);
	band40mBtnRx3->setFixedSize (btn_width3, btn_height);
	band40mBtnRx3->setStyleSheet(m_settings->getMiniButtonStyle());
	band40mBtnRxList.append(band40mBtnRx3);

	CHECKED_CONNECT(
		band40mBtnRx3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band40mBtnRxClicked()));

	band30mBtnRx3 = new AeroButton("XVTR", this);
	band30mBtnRx3->setRoundness(0);
	band30mBtnRx3->setFixedSize (btn_width3, btn_height);
	band30mBtnRx3->setStyleSheet(m_settings->getMiniButtonStyle());
	band30mBtnRxList.append(band30mBtnRx3);

	CHECKED_CONNECT(
		band30mBtnRx3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band30mBtnRxClicked()));

	band20mBtnRx3 = new AeroButton("XVTR", this);
	band20mBtnRx3->setRoundness(0);
	band20mBtnRx3->setFixedSize (btn_width3, btn_height);
	band20mBtnRx3->setStyleSheet(m_settings->getMiniButtonStyle());
	band20mBtnRxList.append(band20mBtnRx3);

	CHECKED_CONNECT(
		band20mBtnRx3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band20mBtnRxClicked()));

	band17mBtnRx3 = new AeroButton("XVTR", this);
	band17mBtnRx3->setRoundness(0);
	band17mBtnRx3->setFixedSize (btn_width3, btn_height);
	band17mBtnRx3->setStyleSheet(m_settings->getMiniButtonStyle());
	band17mBtnRxList.append(band17mBtnRx3);

	CHECKED_CONNECT(
		band17mBtnRx3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band17mBtnRxClicked()));

	band15mBtnRx3 = new AeroButton("XVTR", this);
	band15mBtnRx3->setRoundness(0);
	band15mBtnRx3->setFixedSize (btn_width3, btn_height);
	band15mBtnRx3->setStyleSheet(m_settings->getMiniButtonStyle());
	band15mBtnRxList.append(band15mBtnRx3);

	CHECKED_CONNECT(
		band15mBtnRx3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band15mBtnRxClicked()));

	band12mBtnRx3 = new AeroButton("XVTR", this);
	band12mBtnRx3->setRoundness(0);
	band12mBtnRx3->setFixedSize (btn_width3, btn_height);
	band12mBtnRx3->setStyleSheet(m_settings->getMiniButtonStyle());
	band12mBtnRxList.append(band12mBtnRx3);

	CHECKED_CONNECT(
		band12mBtnRx3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band12mBtnRxClicked()));

	band10mBtnRx3 = new AeroButton("XVTR", this);
	band10mBtnRx3->setRoundness(0);
	band10mBtnRx3->setFixedSize (btn_width3, btn_height);
	band10mBtnRx3->setStyleSheet(m_settings->getMiniButtonStyle());
	band10mBtnRxList.append(band10mBtnRx3);

	CHECKED_CONNECT(
		band10mBtnRx3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band10mBtnRxClicked()));

	band6mBtnRx3 = new AeroButton("XVTR", this);
	band6mBtnRx3->setRoundness(0);
	band6mBtnRx3->setFixedSize (btn_width3, btn_height);
	band6mBtnRx3->setStyleSheet(m_settings->getMiniButtonStyle());
	band6mBtnRxList.append(band6mBtnRx3);

	CHECKED_CONNECT(
		band6mBtnRx3, 
		SIGNAL(clicked()), 
		this, 
		SLOT(band6mBtnRxClicked()));


	QGridLayout* grid = new QGridLayout(this);
	grid->setVerticalSpacing(2);
	grid->setHorizontalSpacing(1);
	grid->setContentsMargins(15, 10, 15, 10);

	grid->addWidget(emptyLabel, 0, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(antLabel, 0, 1, 1, 3, Qt::AlignCenter);
	grid->addWidget(emptyLabel, 0, 4, 1, 1, Qt::AlignCenter);
	grid->addWidget(rxAuxLabel, 0, 5, 1, 3, Qt::AlignCenter);
	
	grid->addWidget(band160mLabel, 1, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(band160mBtn1, 1, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band160mBtn2, 1, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band160mBtn3, 1, 3, 1, 1, Qt::AlignTop);
	grid->addWidget(emptyLabel, 1, 4, 1, 1, Qt::AlignTop);
	grid->addWidget(band160mBtnRx1, 1, 5, 1, 1, Qt::AlignTop);
	grid->addWidget(band160mBtnRx2, 1, 6, 1, 1, Qt::AlignTop);
	grid->addWidget(band160mBtnRx3, 1, 7, 1, 1, Qt::AlignTop);

	grid->addWidget(band80mLabel, 2, 0, 1, 1, Qt::AlignTop);
	grid->addWidget(band80mBtn1, 2, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band80mBtn2, 2, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band80mBtn3, 2, 3, 1, 1, Qt::AlignTop);
	grid->addWidget(emptyLabel, 2, 4, 1, 1, Qt::AlignTop);
	grid->addWidget(band80mBtnRx1, 2, 5, 1, 1, Qt::AlignTop);
	grid->addWidget(band80mBtnRx2, 2, 6, 1, 1, Qt::AlignTop);
	grid->addWidget(band80mBtnRx3, 2, 7, 1, 1, Qt::AlignTop);

	grid->addWidget(band60mLabel, 3, 0, 1, 1, Qt::AlignTop);
	grid->addWidget(band60mBtn1, 3, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band60mBtn2, 3, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band60mBtn3, 3, 3, 1, 1, Qt::AlignTop);
	grid->addWidget(emptyLabel, 3, 4, 1, 1, Qt::AlignTop);
	grid->addWidget(band60mBtnRx1, 3, 5, 1, 1, Qt::AlignTop);
	grid->addWidget(band60mBtnRx2, 3, 6, 1, 1, Qt::AlignTop);
	grid->addWidget(band60mBtnRx3, 3, 7, 1, 1, Qt::AlignTop);

	grid->addWidget(band40mLabel, 4, 0, 1, 1, Qt::AlignTop);
	grid->addWidget(band40mBtn1, 4, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band40mBtn2, 4, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band40mBtn3, 4, 3, 1, 1, Qt::AlignTop);
	grid->addWidget(emptyLabel, 4, 4, 1, 1, Qt::AlignTop);
	grid->addWidget(band40mBtnRx1, 4, 5, 1, 1, Qt::AlignTop);
	grid->addWidget(band40mBtnRx2, 4, 6, 1, 1, Qt::AlignTop);
	grid->addWidget(band40mBtnRx3, 4, 7, 1, 1, Qt::AlignTop);

	grid->addWidget(band30mLabel, 5, 0, 1, 1, Qt::AlignTop);
	grid->addWidget(band30mBtn1, 5, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band30mBtn2, 5, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band30mBtn3, 5, 3, 1, 1, Qt::AlignTop);
	grid->addWidget(emptyLabel, 5, 4, 1, 1, Qt::AlignTop);
	grid->addWidget(band30mBtnRx1, 5, 5, 1, 1, Qt::AlignTop);
	grid->addWidget(band30mBtnRx2, 5, 6, 1, 1, Qt::AlignTop);
	grid->addWidget(band30mBtnRx3, 5, 7, 1, 1, Qt::AlignTop);

	grid->addWidget(band20mLabel, 6, 0, 1, 1, Qt::AlignTop);
	grid->addWidget(band20mBtn1, 6, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band20mBtn2, 6, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band20mBtn3, 6, 3, 1, 1, Qt::AlignTop);
	grid->addWidget(emptyLabel, 6, 4, 1, 1, Qt::AlignTop);
	grid->addWidget(band20mBtnRx1, 6, 5, 1, 1, Qt::AlignTop);
	grid->addWidget(band20mBtnRx2, 6, 6, 1, 1, Qt::AlignTop);
	grid->addWidget(band20mBtnRx3, 6, 7, 1, 1, Qt::AlignTop);

	grid->addWidget(band17mLabel, 7, 0, 1, 1, Qt::AlignTop);
	grid->addWidget(band17mBtn1, 7, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band17mBtn2, 7, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band17mBtn3, 7, 3, 1, 1, Qt::AlignTop);
	grid->addWidget(emptyLabel, 7, 4, 1, 1, Qt::AlignTop);
	grid->addWidget(band17mBtnRx1, 7, 5, 1, 1, Qt::AlignTop);
	grid->addWidget(band17mBtnRx2, 7, 6, 1, 1, Qt::AlignTop);
	grid->addWidget(band17mBtnRx3, 7, 7, 1, 1, Qt::AlignTop);

	grid->addWidget(band15mLabel, 8, 0, 1, 1, Qt::AlignTop);
	grid->addWidget(band15mBtn1, 8, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band15mBtn2, 8, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band15mBtn3, 8, 3, 1, 1, Qt::AlignTop);
	grid->addWidget(emptyLabel, 8, 4, 1, 1, Qt::AlignTop);
	grid->addWidget(band15mBtnRx1, 8, 5, 1, 1, Qt::AlignTop);
	grid->addWidget(band15mBtnRx2, 8, 6, 1, 1, Qt::AlignTop);
	grid->addWidget(band15mBtnRx3, 8, 7, 1, 1, Qt::AlignTop);

	grid->addWidget(band12mLabel, 9, 0, 1, 1, Qt::AlignTop);
	grid->addWidget(band12mBtn1, 9, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band12mBtn2, 9, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band12mBtn3, 9, 3, 1, 1, Qt::AlignTop);
	grid->addWidget(emptyLabel, 9, 4, 1, 1, Qt::AlignTop);
	grid->addWidget(band12mBtnRx1, 9, 5, 1, 1, Qt::AlignTop);
	grid->addWidget(band12mBtnRx2, 9, 6, 1, 1, Qt::AlignTop);
	grid->addWidget(band12mBtnRx3, 9, 7, 1, 1, Qt::AlignTop);

	grid->addWidget(band10mLabel, 10, 0, 1, 1, Qt::AlignTop);
	grid->addWidget(band10mBtn1, 10, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band10mBtn2, 10, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band10mBtn3, 10, 3, 1, 1, Qt::AlignTop);
	grid->addWidget(emptyLabel, 10, 4, 1, 1, Qt::AlignTop);
	grid->addWidget(band10mBtnRx1, 10, 5, 1, 1, Qt::AlignTop);
	grid->addWidget(band10mBtnRx2, 10, 6, 1, 1, Qt::AlignTop);
	grid->addWidget(band10mBtnRx3, 10, 7, 1, 1, Qt::AlignTop);

	grid->addWidget(band6mLabel, 11, 0, 1, 1, Qt::AlignTop);
	grid->addWidget(band6mBtn1, 11, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band6mBtn2, 11, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band6mBtn3, 11, 3, 1, 1, Qt::AlignTop);
	grid->addWidget(emptyLabel, 11, 4, 1, 1, Qt::AlignTop);
	grid->addWidget(band6mBtnRx1, 11, 5, 1, 1, Qt::AlignTop);
	grid->addWidget(band6mBtnRx2, 11, 6, 1, 1, Qt::AlignTop);
	grid->addWidget(band6mBtnRx3, 11, 7, 1, 1, Qt::AlignTop);

	receiveGroup = new QGroupBox(tr("Receive"), this);
	receiveGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	receiveGroup->setLayout(grid);
	receiveGroup->setStyleSheet(m_settings->getWidgetStyle());
	receiveGroup->setFont(QFont("Arial", 8));
}

void AlexWidget::createTransmitGroup() {

	QLabel* emptyLabel = new QLabel(" ", this);
    emptyLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	emptyLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* antLabel = new QLabel("Ant", this);
    antLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	antLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band160mLabel = new QLabel("160m", this);
    band160mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band160mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band80mLabel = new QLabel("80m", this);
    band80mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band80mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band60mLabel = new QLabel("60m", this);
    band60mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band60mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band40mLabel = new QLabel("40m", this);
    band40mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band40mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band30mLabel = new QLabel("30m", this);
    band30mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band30mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band20mLabel = new QLabel("20m", this);
    band20mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band20mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band17mLabel = new QLabel("17m", this);
    band17mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band17mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band15mLabel = new QLabel("15m", this);
    band15mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band15mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band12mLabel = new QLabel("12m", this);
    band12mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band12mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band10mLabel = new QLabel("10m", this);
    band10mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band10mLabel->setStyleSheet(m_settings->getLabelStyle());

	QLabel* band6mLabel = new QLabel("6m", this);
    band6mLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	band6mLabel->setStyleSheet(m_settings->getLabelStyle());

	// Ant1
	band160mBtnTx1 = new AeroButton("1", this);
	band160mBtnTx1->setRoundness(0);
	band160mBtnTx1->setFixedSize (btn_width, btn_height);
	band160mBtnTx1->setStyleSheet(m_settings->getMiniButtonStyle());
	//band160mBtnTx1->setColorOn(m_settings->getPanadapterColors().panCenterLineColor);

	band80mBtnTx1 = new AeroButton("1", this);
	band80mBtnTx1->setRoundness(0);
	band80mBtnTx1->setFixedSize (btn_width, btn_height);
	band80mBtnTx1->setStyleSheet(m_settings->getMiniButtonStyle());

	band60mBtnTx1 = new AeroButton("1", this);
	band60mBtnTx1->setRoundness(0);
	band60mBtnTx1->setFixedSize (btn_width, btn_height);
	band60mBtnTx1->setStyleSheet(m_settings->getMiniButtonStyle());

	band40mBtnTx1 = new AeroButton("1", this);
	band40mBtnTx1->setRoundness(0);
	band40mBtnTx1->setFixedSize (btn_width, btn_height);
	band40mBtnTx1->setStyleSheet(m_settings->getMiniButtonStyle());

	band30mBtnTx1 = new AeroButton("1", this);
	band30mBtnTx1->setRoundness(0);
	band30mBtnTx1->setFixedSize (btn_width, btn_height);
	band30mBtnTx1->setStyleSheet(m_settings->getMiniButtonStyle());

	band20mBtnTx1 = new AeroButton("1", this);
	band20mBtnTx1->setRoundness(0);
	band20mBtnTx1->setFixedSize (btn_width, btn_height);
	band20mBtnTx1->setStyleSheet(m_settings->getMiniButtonStyle());

	band17mBtnTx1 = new AeroButton("1", this);
	band17mBtnTx1->setRoundness(0);
	band17mBtnTx1->setFixedSize (btn_width, btn_height);
	band17mBtnTx1->setStyleSheet(m_settings->getMiniButtonStyle());

	band15mBtnTx1 = new AeroButton("1", this);
	band15mBtnTx1->setRoundness(0);
	band15mBtnTx1->setFixedSize (btn_width, btn_height);
	band15mBtnTx1->setStyleSheet(m_settings->getMiniButtonStyle());

	band12mBtnTx1 = new AeroButton("1", this);
	band12mBtnTx1->setRoundness(0);
	band12mBtnTx1->setFixedSize (btn_width, btn_height);
	band12mBtnTx1->setStyleSheet(m_settings->getMiniButtonStyle());

	band10mBtnTx1 = new AeroButton("1", this);
	band10mBtnTx1->setRoundness(0);
	band10mBtnTx1->setFixedSize (btn_width, btn_height);
	band10mBtnTx1->setStyleSheet(m_settings->getMiniButtonStyle());

	band6mBtnTx1 = new AeroButton("1", this);
	band6mBtnTx1->setRoundness(0);
	band6mBtnTx1->setFixedSize (btn_width, btn_height);
	band6mBtnTx1->setStyleSheet(m_settings->getMiniButtonStyle());

	// Ant2
	band160mBtnTx2 = new AeroButton("2", this);
	band160mBtnTx2->setRoundness(0);
	band160mBtnTx2->setFixedSize (btn_width, btn_height);
	band160mBtnTx2->setStyleSheet(m_settings->getMiniButtonStyle());
	//band160mBtnTx2->setColorOn(m_settings->getPanadapterColors().panCenterLineColor);

	band80mBtnTx2 = new AeroButton("2", this);
	band80mBtnTx2->setRoundness(0);
	band80mBtnTx2->setFixedSize (btn_width, btn_height);
	band80mBtnTx2->setStyleSheet(m_settings->getMiniButtonStyle());

	band60mBtnTx2 = new AeroButton("2", this);
	band60mBtnTx2->setRoundness(0);
	band60mBtnTx2->setFixedSize (btn_width, btn_height);
	band60mBtnTx2->setStyleSheet(m_settings->getMiniButtonStyle());

	band40mBtnTx2 = new AeroButton("2", this);
	band40mBtnTx2->setRoundness(0);
	band40mBtnTx2->setFixedSize (btn_width, btn_height);
	band40mBtnTx2->setStyleSheet(m_settings->getMiniButtonStyle());

	band30mBtnTx2 = new AeroButton("2", this);
	band30mBtnTx2->setRoundness(0);
	band30mBtnTx2->setFixedSize (btn_width, btn_height);
	band30mBtnTx2->setStyleSheet(m_settings->getMiniButtonStyle());

	band20mBtnTx2 = new AeroButton("2", this);
	band20mBtnTx2->setRoundness(0);
	band20mBtnTx2->setFixedSize (btn_width, btn_height);
	band20mBtnTx2->setStyleSheet(m_settings->getMiniButtonStyle());

	band17mBtnTx2 = new AeroButton("2", this);
	band17mBtnTx2->setRoundness(0);
	band17mBtnTx2->setFixedSize (btn_width, btn_height);
	band17mBtnTx2->setStyleSheet(m_settings->getMiniButtonStyle());

	band15mBtnTx2 = new AeroButton("2", this);
	band15mBtnTx2->setRoundness(0);
	band15mBtnTx2->setFixedSize (btn_width, btn_height);
	band15mBtnTx2->setStyleSheet(m_settings->getMiniButtonStyle());

	band12mBtnTx2 = new AeroButton("2", this);
	band12mBtnTx2->setRoundness(0);
	band12mBtnTx2->setFixedSize (btn_width, btn_height);
	band12mBtnTx2->setStyleSheet(m_settings->getMiniButtonStyle());

	band10mBtnTx2 = new AeroButton("2", this);
	band10mBtnTx2->setRoundness(0);
	band10mBtnTx2->setFixedSize (btn_width, btn_height);
	band10mBtnTx2->setStyleSheet(m_settings->getMiniButtonStyle());

	band6mBtnTx2 = new AeroButton("2", this);
	band6mBtnTx2->setRoundness(0);
	band6mBtnTx2->setFixedSize (btn_width, btn_height);
	band6mBtnTx2->setStyleSheet(m_settings->getMiniButtonStyle());

	// Ant3
	band160mBtnTx3 = new AeroButton("3", this);
	band160mBtnTx3->setRoundness(0);
	band160mBtnTx3->setFixedSize (btn_width, btn_height);
	band160mBtnTx3->setStyleSheet(m_settings->getMiniButtonStyle());
	//band160mBtnTx3->setColorOn(m_settings->getPanadapterColors().panCenterLineColor);

	band80mBtnTx3 = new AeroButton("3", this);
	band80mBtnTx3->setRoundness(0);
	band80mBtnTx3->setFixedSize (btn_width, btn_height);
	band80mBtnTx3->setStyleSheet(m_settings->getMiniButtonStyle());

	band60mBtnTx3 = new AeroButton("3", this);
	band60mBtnTx3->setRoundness(0);
	band60mBtnTx3->setFixedSize (btn_width, btn_height);
	band60mBtnTx3->setStyleSheet(m_settings->getMiniButtonStyle());

	band40mBtnTx3 = new AeroButton("3", this);
	band40mBtnTx3->setRoundness(0);
	band40mBtnTx3->setFixedSize (btn_width, btn_height);
	band40mBtnTx3->setStyleSheet(m_settings->getMiniButtonStyle());

	band30mBtnTx3 = new AeroButton("3", this);
	band30mBtnTx3->setRoundness(0);
	band30mBtnTx3->setFixedSize (btn_width, btn_height);
	band30mBtnTx3->setStyleSheet(m_settings->getMiniButtonStyle());

	band20mBtnTx3 = new AeroButton("3", this);
	band20mBtnTx3->setRoundness(0);
	band20mBtnTx3->setFixedSize (btn_width, btn_height);
	band20mBtnTx3->setStyleSheet(m_settings->getMiniButtonStyle());

	band17mBtnTx3 = new AeroButton("3", this);
	band17mBtnTx3->setRoundness(0);
	band17mBtnTx3->setFixedSize (btn_width, btn_height);
	band17mBtnTx3->setStyleSheet(m_settings->getMiniButtonStyle());

	band15mBtnTx3 = new AeroButton("3", this);
	band15mBtnTx3->setRoundness(0);
	band15mBtnTx3->setFixedSize (btn_width, btn_height);
	band15mBtnTx3->setStyleSheet(m_settings->getMiniButtonStyle());

	band12mBtnTx3 = new AeroButton("3", this);
	band12mBtnTx3->setRoundness(0);
	band12mBtnTx3->setFixedSize (btn_width, btn_height);
	band12mBtnTx3->setStyleSheet(m_settings->getMiniButtonStyle());

	band10mBtnTx3 = new AeroButton("3", this);
	band10mBtnTx3->setRoundness(0);
	band10mBtnTx3->setFixedSize (btn_width, btn_height);
	band10mBtnTx3->setStyleSheet(m_settings->getMiniButtonStyle());

	band6mBtnTx3 = new AeroButton("3", this);
	band6mBtnTx3->setRoundness(0);
	band6mBtnTx3->setFixedSize (btn_width, btn_height);
	band6mBtnTx3->setStyleSheet(m_settings->getMiniButtonStyle());

	QGridLayout* grid = new QGridLayout(this);
	grid->setVerticalSpacing(2);
	grid->setHorizontalSpacing(1);
	grid->setContentsMargins(15, 10, 90, 10);

	grid->addWidget(emptyLabel, 0, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(antLabel, 0, 1, 1, 3, Qt::AlignCenter);
	grid->addWidget(emptyLabel, 0, 4, 1, 1, Qt::AlignCenter);
	
	grid->addWidget(band160mLabel, 1, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(band160mBtnTx1, 1, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band160mBtnTx2, 1, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band160mBtnTx3, 1, 3, 1, 1, Qt::AlignTop);

	grid->addWidget(band80mLabel, 2, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(band80mBtnTx1, 2, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band80mBtnTx2, 2, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band80mBtnTx3, 2, 3, 1, 1, Qt::AlignTop);

	grid->addWidget(band60mLabel, 3, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(band60mBtnTx1, 3, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band60mBtnTx2, 3, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band60mBtnTx3, 3, 3, 1, 1, Qt::AlignTop);

	grid->addWidget(band40mLabel, 4, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(band40mBtnTx1, 4, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band40mBtnTx2, 4, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band40mBtnTx3, 4, 3, 1, 1, Qt::AlignTop);

	grid->addWidget(band30mLabel, 5, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(band30mBtnTx1, 5, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band30mBtnTx2, 5, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band30mBtnTx3, 5, 3, 1, 1, Qt::AlignTop);

	grid->addWidget(band20mLabel, 6, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(band20mBtnTx1, 6, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band20mBtnTx2, 6, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band20mBtnTx3, 6, 3, 1, 1, Qt::AlignTop);

	grid->addWidget(band17mLabel, 7, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(band17mBtnTx1, 7, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band17mBtnTx2, 7, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band17mBtnTx3, 7, 3, 1, 1, Qt::AlignTop);

	grid->addWidget(band15mLabel, 8, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(band15mBtnTx1, 8, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band15mBtnTx2, 8, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band15mBtnTx3, 8, 3, 1, 1, Qt::AlignTop);

	grid->addWidget(band12mLabel, 9, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(band12mBtnTx1, 9, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band12mBtnTx2, 9, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band12mBtnTx3, 9, 3, 1, 1, Qt::AlignTop);

	grid->addWidget(band10mLabel, 10, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(band10mBtnTx1, 10, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band10mBtnTx2, 10, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band10mBtnTx3, 10, 3, 1, 1, Qt::AlignTop);

	grid->addWidget(band6mLabel, 11, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(band6mBtnTx1, 11, 1, 1, 1, Qt::AlignTop);
	grid->addWidget(band6mBtnTx2, 11, 2, 1, 1, Qt::AlignTop);
	grid->addWidget(band6mBtnTx3, 11, 3, 1, 1, Qt::AlignTop);

	transmitGroup = new QGroupBox(tr("Transmit"), this);
	transmitGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	transmitGroup->setLayout(grid);
	transmitGroup->setStyleSheet(m_settings->getWidgetStyle());
	transmitGroup->setFont(QFont("Arial", 8));
}

//*****************
void AlexWidget::band160mBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band160mBtnList.indexOf(button);

	foreach(AeroButton *btn, band160mBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band80mBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band80mBtnList.indexOf(button);

	foreach(AeroButton *btn, band80mBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band60mBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band60mBtnList.indexOf(button);

	foreach(AeroButton *btn, band60mBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band40mBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band40mBtnList.indexOf(button);

	foreach(AeroButton *btn, band40mBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band30mBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band30mBtnList.indexOf(button);

	foreach(AeroButton *btn, band30mBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band20mBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band20mBtnList.indexOf(button);

	foreach(AeroButton *btn, band20mBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band17mBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band17mBtnList.indexOf(button);

	foreach(AeroButton *btn, band17mBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band15mBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band15mBtnList.indexOf(button);

	foreach(AeroButton *btn, band15mBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band12mBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band12mBtnList.indexOf(button);

	foreach(AeroButton *btn, band12mBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band10mBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band10mBtnList.indexOf(button);

	foreach(AeroButton *btn, band10mBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band6mBtnClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band6mBtnList.indexOf(button);

	foreach(AeroButton *btn, band6mBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

//**************
void AlexWidget::band160mBtnRxClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band160mBtnRxList.indexOf(button);

	foreach(AeroButton *btn, band160mBtnRxList) {

		if (btn->btnState() == AeroButton::ON && btn == button) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
			return;
		}
		else {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}	
}

void AlexWidget::band80mBtnRxClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band80mBtnRxList.indexOf(button);

	foreach(AeroButton *btn, band80mBtnRxList) {

		if (btn->btnState() == AeroButton::ON && btn == button) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
			return;
		}
		else {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band60mBtnRxClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band60mBtnRxList.indexOf(button);

	foreach(AeroButton *btn, band60mBtnRxList) {

		if (btn->btnState() == AeroButton::ON && btn == button) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
			return;
		}
		else {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band40mBtnRxClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band40mBtnRxList.indexOf(button);

	foreach(AeroButton *btn, band40mBtnRxList) {

		if (btn->btnState() == AeroButton::ON && btn == button) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
			return;
		}
		else {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band30mBtnRxClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band30mBtnRxList.indexOf(button);

	foreach(AeroButton *btn, band30mBtnRxList) {

		if (btn->btnState() == AeroButton::ON && btn == button) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
			return;
		}
		else {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band20mBtnRxClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band20mBtnRxList.indexOf(button);

	foreach(AeroButton *btn, band20mBtnRxList) {

		if (btn->btnState() == AeroButton::ON && btn == button) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
			return;
		}
		else {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band17mBtnRxClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band17mBtnRxList.indexOf(button);

	foreach(AeroButton *btn, band17mBtnRxList) {

		if (btn->btnState() == AeroButton::ON && btn == button) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
			return;
		}
		else {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band15mBtnRxClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band15mBtnRxList.indexOf(button);

	foreach(AeroButton *btn, band15mBtnRxList) {

		if (btn->btnState() == AeroButton::ON && btn == button) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
			return;
		}
		else {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band12mBtnRxClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band12mBtnRxList.indexOf(button);

	foreach(AeroButton *btn, band12mBtnRxList) {

		if (btn->btnState() == AeroButton::ON && btn == button) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
			return;
		}
		else {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band10mBtnRxClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band10mBtnRxList.indexOf(button);

	foreach(AeroButton *btn, band10mBtnRxList) {

		if (btn->btnState() == AeroButton::ON && btn == button) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
			return;
		}
		else {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

void AlexWidget::band6mBtnRxClicked() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = band6mBtnRxList.indexOf(button);

	foreach(AeroButton *btn, band6mBtnRxList) {

		if (btn->btnState() == AeroButton::ON && btn == button) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
			return;
		}
		else {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			break;
			
		case 1:
			break;

		case 2:
			break;
	}
}

//*******************
void AlexWidget::resizeEvent(QResizeEvent *event) {

	//Q_UNUSED(event);
	//QResizeEvent::oldSize()
	
	//QTimer::singleShot(10, this, SLOT(getRegion()));
	//m_resizeFrame = true;
	
	QWidget::resizeEvent(event);
}