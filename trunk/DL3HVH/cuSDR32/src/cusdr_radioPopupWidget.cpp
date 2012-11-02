/**
* @file cusdr_radioPopupWidget.cpp
* @brief Radio control popup widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-08-22
*/

/*
 *   Copyright 2010-2012 Hermann von Hasseln, DL3HVH
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
 
//#include <QtGui>
//#include <QDebug>

#include "cusdr_radioPopupWidget.h"

#define	btn_height		14
#define	btn_width		60
#define	btn_widthb		70
#define	btn_widths		34


RadioPopupWidget::RadioPopupWidget(QWidget *parent)
	: QWidget(parent)
	, set(Settings::instance())
	, m_currentRx(set->getCurrentReceiver())
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(set->getMinimumGroupBoxWidth())
{
	//setWindowFlags(Qt::Tool | Qt::FramelessWindowHint | Qt::WindowStaysOnTopHint);
	setWindowFlags(Qt::Tool | Qt::WindowStaysOnTopHint);
	setWindowTitle("Radio Control");
	setContentsMargins(4, 4, 4, 4);
	setWindowOpacity(0.8);
	setMouseTracking(true);
	
	setFixedWidth(206);
	//setMinimumHeight(100);
	setFixedHeight(116);

	setFocusPolicy(Qt::StrongFocus);

	QString	style = QString::fromUtf8(
			"border: 0px solid rgba(166, 196, 208, 255); "
			"border-right-width: 0px; "
			"color: rgb(166, 196, 208); "
			"background-color: rgba(40, 40, 40, 255); ");

	setStyleSheet(style);

	m_receiverDataList = set->getReceiverDataList();

	m_hamBand = m_receiverDataList.at(0).hamBand;
	//m_dspMode = m_receiverDataList.at(0).dspMode;
	m_dspModeList = m_receiverDataList.at(0).dspModeList;
	m_agcMode = m_receiverDataList.at(0).agcMode;
	m_filterMode = m_receiverDataList.at(0).defaultFilterMode;
	m_filterLo = m_receiverDataList.at(0).filterLo;
	m_filterHi = m_receiverDataList.at(0).filterHi;

	m_lastFrequencyList = m_receiverDataList.at(0).lastFrequencyList;

	createBandBtnGroup();
	createModeBtnGroup();
	createAgcBtnGroup();
	createFilterBtnWidgetA();
	createFilterBtnWidgetB();
	createFilterBtnWidgetC();


	m_filterStackedWidget = new QStackedWidget(this);//(this->layout());
	m_filterStackedWidget->setContentsMargins(0, 0, 0, 0);
	//m_filterStackedWidget->setSpacing(0);
	//m_filterStackedWidget->setAlignment(Qt::AlignTop);
	m_filterStackedWidget->addWidget(filterAWidget);
	m_filterStackedWidget->addWidget(filterBWidget);
	m_filterStackedWidget->addWidget(filterCWidget);

	QVBoxLayout *mainLayout = new QVBoxLayout;
	mainLayout->setSpacing(0);
	mainLayout->setMargin(0);
	mainLayout->addLayout(bandVBox);
	mainLayout->addSpacing(2);
	mainLayout->addLayout(modeVBox);
	mainLayout->addSpacing(2);
	//mainLayout->addLayout(m_filterStackedLayout);
	mainLayout->addWidget(m_filterStackedWidget);
	mainLayout->addSpacing(2);
	mainLayout->addLayout(agcVBox);

	setLayout(mainLayout);
	
	// setup values from settings.ini
	bandBtnList.at(m_hamBand)->setBtnState(AeroButton::ON);
	bandBtnList.at(m_hamBand)->update();

	dspModeChanged(this, 0, m_dspModeList.at(m_hamBand));
	agcModeChanged(this, 0, m_agcMode, false);
	filterChanged(this, 0, m_filterLo, m_filterHi);

	DSPMode dspMode = m_dspModeList.at(m_hamBand);
	if (dspMode == (DSPMode) LSB || dspMode == (DSPMode) USB || dspMode == (DSPMode) DIGU || dspMode == (DSPMode) DIGL) {

		//m_filterStackedLayout->setCurrentIndex(0);
		m_filterStackedWidget->setCurrentIndex(0);
	}
	else
	if (dspMode == (DSPMode) DSB || dspMode == (DSPMode) FMN || dspMode == (DSPMode) AM || dspMode == (DSPMode) SAM) {

		//m_filterStackedLayout->setCurrentIndex(1);
		m_filterStackedWidget->setCurrentIndex(1);
	}
	else
	if (dspMode == (DSPMode) CWL || dspMode == (DSPMode) CWU) {

		//m_filterStackedLayout->setCurrentIndex(2);
		m_filterStackedWidget->setCurrentIndex(2);
	}


	setupConnections();

	m_timerID = 0;

	m_closingTimer.start();
	m_timerID = startTimer(100);
}

RadioPopupWidget::~RadioPopupWidget() {

	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);

	if (m_timerID) {

		killTimer(m_timerID);
		m_timerID = 0;
	}
}

QSize RadioPopupWidget::sizeHint() const {
	
	return QSize(width(), height());
}

QSize RadioPopupWidget::minimumSizeHint() const {

	return QSize(width(), height());
}

void RadioPopupWidget::setupConnections() {

	CHECKED_CONNECT(
		set,
		SIGNAL(systemStateChanged(
					QObject *,
					QSDR::_Error,
					QSDR::_HWInterfaceMode,
					QSDR::_ServerMode,
					QSDR::_DataEngineState)),
		this,
		SLOT(systemStateChanged(
					QObject *,
					QSDR::_Error,
					QSDR::_HWInterfaceMode,
					QSDR::_ServerMode,
					QSDR::_DataEngineState)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(currentReceiverChanged(QObject *, int)),
		this, 
		SLOT(setCurrentReceiver(QObject *, int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(frequencyChanged(QObject *, bool, int, long)),
		this,
		SLOT(frequencyChanged(QObject *, bool, int, long)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(hamBandChanged(QObject *, int, bool, HamBand)),
		this,
		SLOT(bandChanged(QObject *, int, bool, HamBand)));

	CHECKED_CONNECT(
		set,
		SIGNAL(dspModeChanged(QObject *, int, DSPMode)), 
		this, 
		SLOT(dspModeChanged(QObject *, int, DSPMode)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(agcModeChanged(QObject *, int, AGCMode, bool)),
		this, 
		SLOT(agcModeChanged(QObject *, int, AGCMode, bool)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(filterFrequenciesChanged(QObject *, int, qreal, qreal)), 
		this, 
		SLOT(filterChanged(QObject *, int, qreal, qreal)));
}

void RadioPopupWidget::createBandBtnGroup() {

	band160mBtn = new AeroButton("160 m", this);
	band160mBtn->setRoundness(0);
	//band160mBtn->setGlass(false);
	band160mBtn->setFixedSize(btn_widths, btn_height);
	band160mBtn->setStyleSheet(set->getMiniButtonStyle());
	//band160mBtn->setTextColor(QColor(200, 200, 200));
	bandBtnList.append(band160mBtn);
	CHECKED_CONNECT(band160mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band80mBtn = new AeroButton("80 m", this);
	band80mBtn->setRoundness(0);
	//band80mBtn->setGlass(false);
	band80mBtn->setFixedSize(btn_widths, btn_height);
	band80mBtn->setStyleSheet(set->getMiniButtonStyle());
	//band80mBtn->setTextColor(QColor(200, 200, 200));
	bandBtnList.append(band80mBtn);
	CHECKED_CONNECT(band80mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band60mBtn = new AeroButton("60 m", this);
	band60mBtn->setRoundness(0);
	//band60mBtn->setGlass(false);
	band60mBtn->setFixedSize(btn_widths, btn_height);
	band60mBtn->setStyleSheet(set->getMiniButtonStyle());
	//band60mBtn->setTextColor(QColor(200, 200, 200));
	bandBtnList.append(band60mBtn);
	CHECKED_CONNECT(band60mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band40mBtn = new AeroButton("40 m", this);
	band40mBtn->setRoundness(0);
	//band40mBtn->setGlass(false);
	band40mBtn->setFixedSize(btn_widths, btn_height);
	band40mBtn->setStyleSheet(set->getMiniButtonStyle());
	//band40mBtn->setTextColor(QColor(200, 200, 200));
	bandBtnList.append(band40mBtn);
	CHECKED_CONNECT(band40mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band30mBtn = new AeroButton("30 m", this);
	band30mBtn->setRoundness(0);
	//band30mBtn->setGlass(false);
	band30mBtn->setFixedSize(btn_widths, btn_height);
	band30mBtn->setStyleSheet(set->getMiniButtonStyle());
	//band30mBtn->setTextColor(QColor(200, 200, 200));
	bandBtnList.append(band30mBtn);
	CHECKED_CONNECT(band30mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band20mBtn = new AeroButton("20 m", this);
	band20mBtn->setRoundness(0);
	//band20mBtn->setGlass(false);
	band20mBtn->setFixedSize(btn_widths, btn_height);
	band20mBtn->setStyleSheet(set->getMiniButtonStyle());
	//band20mBtn->setTextColor(QColor(200, 200, 200));
	bandBtnList.append(band20mBtn);
	CHECKED_CONNECT(band20mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band17mBtn = new AeroButton("17 m", this);
	band17mBtn->setRoundness(0);
	//band17mBtn->setGlass(false);
	band17mBtn->setFixedSize(btn_widths, btn_height);
	band17mBtn->setStyleSheet(set->getMiniButtonStyle());
	//band17mBtn->setTextColor(QColor(200, 200, 200));
	bandBtnList.append(band17mBtn);
	CHECKED_CONNECT(band17mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band15mBtn = new AeroButton("15 m", this);
	band15mBtn->setRoundness(0);
	//band15mBtn->setGlass(false);
	band15mBtn->setFixedSize(btn_widths, btn_height);
	band15mBtn->setStyleSheet(set->getMiniButtonStyle());
	//band15mBtn->setTextColor(QColor(200, 200, 200));
	bandBtnList.append(band15mBtn);
	CHECKED_CONNECT(band15mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band12mBtn = new AeroButton("12 m", this);
	band12mBtn->setRoundness(0);
	//band12mBtn->setGlass(false);
	band12mBtn->setFixedSize(btn_widths, btn_height);
	band12mBtn->setStyleSheet(set->getMiniButtonStyle());
	//band12mBtn->setTextColor(QColor(200, 200, 200));
	bandBtnList.append(band12mBtn);
	CHECKED_CONNECT(band12mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band10mBtn = new AeroButton("10 m", this);
	band10mBtn->setRoundness(0);
	//band10mBtn->setGlass(false);
	band10mBtn->setFixedSize(btn_widths, btn_height);
	band10mBtn->setStyleSheet(set->getMiniButtonStyle());
	//band10mBtn->setTextColor(QColor(200, 200, 200));
	bandBtnList.append(band10mBtn);
	CHECKED_CONNECT(band10mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band6mBtn = new AeroButton("6 m", this);
	band6mBtn->setRoundness(0);
	//band6mBtn->setGlass(false);
	band6mBtn->setFixedSize(btn_widths, btn_height);
	band6mBtn->setStyleSheet(set->getMiniButtonStyle());
	//band6mBtn->setTextColor(QColor(200, 200, 200));
	bandBtnList.append(band6mBtn);
	CHECKED_CONNECT(band6mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	bandGenBtn = new AeroButton("Gen", this);
	bandGenBtn->setRoundness(0);
	//bandGenBtn->setGlass(false);
	bandGenBtn->setFixedSize(btn_widths, btn_height);
	bandGenBtn->setStyleSheet(set->getMiniButtonStyle());
	//bandGenBtn->setTextColor(QColor(200, 200, 200));
	bandBtnList.append(bandGenBtn);
	CHECKED_CONNECT(bandGenBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	/*bandxxBtn = new AeroButton("", this);
	bandxxBtn->setRoundness(0);
	bandxxBtn->setGlass(false);
	bandxxBtn->setFixedSize(btn_widths, btn_height);
	bandxxBtn->setHighlight(QColor(90, 90, 90));
	bandxxBtn->setStyleSheet(set->getMiniButtonStyle());
	bandxxBtn->setTextColor(QColor(200, 200, 200));
	bandxxBtn->setEnabled(false);*/
	

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setContentsMargins(0, 0, 0, 0);
	hbox1->setSpacing(0);
	hbox1->addWidget(band160mBtn);
	hbox1->addWidget(band80mBtn);
	hbox1->addWidget(band60mBtn);
	hbox1->addWidget(band40mBtn);
	hbox1->addWidget(band30mBtn);
	hbox1->addWidget(band20mBtn);

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setContentsMargins(0, 0, 0, 0);
	hbox2->setSpacing(0);
	hbox2->addWidget(band17mBtn);
	hbox2->addWidget(band15mBtn);
	hbox2->addWidget(band12mBtn);
	hbox2->addWidget(band10mBtn);
	hbox2->addWidget(band6mBtn);
	hbox2->addWidget(bandGenBtn);

	bandVBox = new QVBoxLayout;
	bandVBox->setSpacing(1);
	bandVBox->addLayout(hbox1);
	bandVBox->addLayout(hbox2);
}

void RadioPopupWidget::createModeBtnGroup() {

	lsbBtn = new AeroButton("LSB", this);
	lsbBtn->setRoundness(0);
	//lsbBtn->setGlass(false);
	lsbBtn->setFixedSize(btn_widths, btn_height);
	lsbBtn->setStyleSheet(set->getMiniButtonStyle());
	//lsbBtn->setTextColor(QColor(200, 200, 200));
	dspModeBtnList.append(lsbBtn);
	CHECKED_CONNECT(lsbBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	usbBtn = new AeroButton("USB", this);
	usbBtn->setRoundness(0);
	//usbBtn->setGlass(false);
	usbBtn->setFixedSize(btn_widths, btn_height);
	usbBtn->setStyleSheet(set->getMiniButtonStyle());
	//usbBtn->setTextColor(QColor(200, 200, 200));
	dspModeBtnList.append(usbBtn);
	CHECKED_CONNECT(usbBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	dsbBtn = new AeroButton("DSB", this);
	dsbBtn->setRoundness(0);
	//dsbBtn->setGlass(false);
	dsbBtn->setFixedSize(btn_widths, btn_height);
	dsbBtn->setStyleSheet(set->getMiniButtonStyle());
	//dsbBtn->setTextColor(QColor(200, 200, 200));
	dspModeBtnList.append(dsbBtn);
	CHECKED_CONNECT(dsbBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	cwlBtn = new AeroButton("CWL", this);
	cwlBtn->setRoundness(0);
	//cwlBtn->setGlass(false);
	cwlBtn->setFixedSize(btn_widths, btn_height);
	cwlBtn->setStyleSheet(set->getMiniButtonStyle());
	//cwlBtn->setTextColor(QColor(200, 200, 200));
	dspModeBtnList.append(cwlBtn);
	CHECKED_CONNECT(cwlBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	cwuBtn = new AeroButton("CWU", this);
	cwuBtn->setRoundness(0);
	//cwuBtn->setGlass(false);
	cwuBtn->setFixedSize(btn_widths, btn_height);
	cwuBtn->setStyleSheet(set->getMiniButtonStyle());
	//cwuBtn->setTextColor(QColor(200, 200, 200));
	dspModeBtnList.append(cwuBtn);
	CHECKED_CONNECT(cwuBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	fmnBtn = new AeroButton("FMN", this);
	fmnBtn->setRoundness(0);
	//fmnBtn->setGlass(false);
	fmnBtn->setFixedSize(btn_widths, btn_height);
	fmnBtn->setStyleSheet(set->getMiniButtonStyle());
	//fmnBtn->setTextColor(QColor(200, 200, 200));
	dspModeBtnList.append(fmnBtn);
	CHECKED_CONNECT(fmnBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	amBtn = new AeroButton("AM", this);
	amBtn->setRoundness(0);
	//amBtn->setGlass(false);
	amBtn->setFixedSize(btn_widths, btn_height);
	amBtn->setStyleSheet(set->getMiniButtonStyle());
	//amBtn->setTextColor(QColor(200, 200, 200));
	dspModeBtnList.append(amBtn);
	CHECKED_CONNECT(amBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	diguBtn = new AeroButton("DIGU", this);
	diguBtn->setRoundness(0);
	//diguBtn->setGlass(false);
	diguBtn->setFixedSize(btn_widths, btn_height);
	diguBtn->setStyleSheet(set->getMiniButtonStyle());
	//diguBtn->setTextColor(QColor(200, 200, 200));
	dspModeBtnList.append(diguBtn);
	CHECKED_CONNECT(diguBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	diglBtn = new AeroButton("DIGL", this);
	diglBtn->setRoundness(0);
	//diglBtn->setGlass(false);
	diglBtn->setFixedSize(btn_widths, btn_height);
	diglBtn->setStyleSheet(set->getMiniButtonStyle());
	//diglBtn->setTextColor(QColor(200, 200, 200));
	dspModeBtnList.append(diglBtn);
	CHECKED_CONNECT(diglBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	specBtn = new AeroButton("SPEC", this);
	specBtn->setRoundness(0);
	//specBtn->setGlass(false);
	specBtn->setFixedSize(btn_widths, btn_height);
	specBtn->setStyleSheet(set->getMiniButtonStyle());
	//specBtn->setTextColor(QColor(200, 200, 200));
	dspModeBtnList.append(specBtn);
	CHECKED_CONNECT(specBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	samBtn = new AeroButton("SAM", this);
	samBtn->setRoundness(0);
	//samBtn->setGlass(false);
	samBtn->setFixedSize(btn_widths, btn_height);
	samBtn->setStyleSheet(set->getMiniButtonStyle());
	//samBtn->setTextColor(QColor(200, 200, 200));
	dspModeBtnList.append(samBtn);
	CHECKED_CONNECT(samBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	drmBtn = new AeroButton("DRM", this);
	drmBtn->setRoundness(0);
	//drmBtn->setGlass(false);
	drmBtn->setFixedSize(btn_widths, btn_height);
	drmBtn->setStyleSheet(set->getMiniButtonStyle());
	//drmBtn->setTextColor(QColor(200, 200, 200));
	dspModeBtnList.append(drmBtn);
	CHECKED_CONNECT(drmBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	/*foreach(AeroButton *btn, dspModeBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}
	m_dspMode = set->getCurrentDSPMode();
	dspModeBtnList.at(m_dspMode)->setBtnState(AeroButton::ON);*/

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setContentsMargins(0, 0, 0, 0);
	hbox1->setSpacing(0);
	hbox1->addWidget(lsbBtn);
	hbox1->addWidget(usbBtn);
	hbox1->addWidget(dsbBtn);
	hbox1->addWidget(cwlBtn);
	hbox1->addWidget(cwuBtn);
	hbox1->addWidget(fmnBtn);

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setContentsMargins(0, 0, 0, 0);
	hbox2->setSpacing(0);
	hbox2->addWidget(amBtn);
	hbox2->addWidget(diguBtn);
	hbox2->addWidget(specBtn);
	hbox2->addWidget(diglBtn);
	hbox2->addWidget(samBtn);
	hbox2->addWidget(drmBtn);

	modeVBox = new QVBoxLayout;
	modeVBox->setSpacing(1);
	modeVBox->addLayout(hbox1);
	modeVBox->addLayout(hbox2);
}

//void RadioPopupWidget::createAgcBtnGroup() {
//
//	AgcBtn = new AeroButton(getAGCModeString(m_agcMode));
//	AgcBtn->setRoundness(0);
//	//AgcBtn->setGlass(false);
//	AgcBtn->setFixedSize(90, btn_height);
//	AgcBtn->setStyleSheet(set->getMiniButtonStyle());
//	//AgcBtn->setTextColor(QColor(200, 200, 200));
//	AgcBtn->setEnabled(false);
//	
//	agc_left.addFile(QString::fromUtf8(":/img/left_arrow.png"), QSize(), QIcon::Normal, QIcon::Off);
//	agc_right.addFile(QString::fromUtf8(":/img/right_arrow.png"), QSize(), QIcon::Normal, QIcon::Off);
//
//	AgcLeftBtn = new AeroButton(agc_left, "", this);
//	AgcLeftBtn->setRoundness(0);
//	AgcLeftBtn->setGlass(false);
//	AgcLeftBtn->setFixedSize(17, btn_height);
//	AgcLeftBtn->setStyleSheet(set->getMiniButtonStyle());
//	CHECKED_CONNECT(AgcLeftBtn, SIGNAL(clicked()), this, SLOT(agcDownModeByBtn()));
//
//	AgcRightBtn = new AeroButton(agc_right, "", this);
//	AgcRightBtn->setRoundness(0);
//	AgcRightBtn->setGlass(false);
//	AgcRightBtn->setFixedSize(15, btn_height);
//	AgcRightBtn->setStyleSheet(set->getMiniButtonStyle());
//	CHECKED_CONNECT(AgcRightBtn, SIGNAL(clicked()), this, SLOT(agcUpModeByBtn()));
//
//	QGridLayout *layout = new QGridLayout();
//	layout->setVerticalSpacing(0);
//	layout->setHorizontalSpacing(0);
//	layout->setContentsMargins(0, 0, 0, 0);
//
//	/*QHBoxLayout *layout = new QHBoxLayout;
//	layout->setSpacing(0);
//	layout->setMargin(0);
//	layout->addSpacing(50);*/
//	layout->addWidget(AgcLeftBtn, 0, 0);
//	layout->addWidget(AgcBtn, 0, 1);
//	layout->addWidget(AgcRightBtn, 0, 2);
//	//layout->addStretch();
//
//	QVBoxLayout *vbox = new QVBoxLayout;
//	vbox->setSpacing(1);
//	vbox->addLayout(layout);
//
//	agcGroupBox = new QGroupBox(tr("AGC"), this);
//	agcGroupBox->setLayout(vbox);
//	agcGroupBox->setStyleSheet(set->getWidgetStyle());
//	agcGroupBox->setMinimumWidth(175);
//	agcGroupBox->setFont(QFont("Arial", 8));
//}

void RadioPopupWidget::createAgcBtnGroup() {

	agcOFF = new AeroButton("Off", this);
	agcOFF->setRoundness(0);
	agcOFF->setFixedSize(btn_widths, btn_height);
	agcOFF->setStyleSheet(set->getMiniButtonStyle());
	agcModeBtnList.append(agcOFF);
	CHECKED_CONNECT(agcOFF, SIGNAL(clicked()), this, SLOT(agcModeChangedByBtn()));

	agcLONG = new AeroButton("Long", this);
	agcLONG->setRoundness(0);
	agcLONG->setFixedSize(btn_widths, btn_height);
	agcLONG->setStyleSheet(set->getMiniButtonStyle());
	agcModeBtnList.append(agcLONG);
	CHECKED_CONNECT(agcLONG, SIGNAL(clicked()), this, SLOT(agcModeChangedByBtn()));

	agcSLOW = new AeroButton("Slow", this);
	agcSLOW->setRoundness(0);
	agcSLOW->setFixedSize(btn_widths, btn_height);
	agcSLOW->setStyleSheet(set->getMiniButtonStyle());
	agcModeBtnList.append(agcSLOW);
	CHECKED_CONNECT(agcSLOW, SIGNAL(clicked()), this, SLOT(agcModeChangedByBtn()));

	agcMED = new AeroButton("Med", this);
	agcMED->setRoundness(0);
	agcMED->setFixedSize(btn_widths, btn_height);
	agcMED->setStyleSheet(set->getMiniButtonStyle());
	agcModeBtnList.append(agcMED);
	CHECKED_CONNECT(agcMED, SIGNAL(clicked()), this, SLOT(agcModeChangedByBtn()));

	agcFAST = new AeroButton("Fast", this);
	agcFAST->setRoundness(0);
	agcFAST->setFixedSize(btn_widths, btn_height);
	agcFAST->setStyleSheet(set->getMiniButtonStyle());
	agcModeBtnList.append(agcFAST);
	CHECKED_CONNECT(agcFAST, SIGNAL(clicked()), this, SLOT(agcModeChangedByBtn()));
	
	agcEmpty = new AeroButton("", this);
	agcEmpty->setRoundness(0);
	agcEmpty->setFixedSize(btn_widths, btn_height);
	agcEmpty->setStyleSheet(set->getMiniButtonStyle());
	agcEmpty->setEnabled(false);

	
	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setContentsMargins(0, 0, 0, 0);
	hbox1->setSpacing(0);
	hbox1->addWidget(agcOFF);
	hbox1->addWidget(agcLONG);
	hbox1->addWidget(agcSLOW);
	hbox1->addWidget(agcMED);
	hbox1->addWidget(agcFAST);
	hbox1->addWidget(agcEmpty);

	agcVBox = new QVBoxLayout;
	agcVBox->setSpacing(1);
	agcVBox->addLayout(hbox1);
}

void RadioPopupWidget::createFilterBtnWidgetA() {

	filter1kBtnA = new AeroButton("1k", this);
	filter1kBtnA->setObjectName("1k");
	filter1kBtnA->setRoundness(0);
	//filter1kBtnA->setGlass(false);
	filter1kBtnA->setFixedSize(btn_widths, btn_height);
	filter1kBtnA->setStyleSheet(set->getMiniButtonStyle());
	//filter1kBtnA->setTextColor(QColor(200, 200, 200));
	filterBtnListA.append(filter1kBtnA);
	CHECKED_CONNECT(filter1kBtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter1k8BtnA = new AeroButton("1k8", this);
	filter1kBtnA->setObjectName("1k8");
	filter1k8BtnA->setRoundness(0);
	//filter1k8BtnA->setGlass(false);
	filter1k8BtnA->setFixedSize(btn_widths, btn_height);
	filter1k8BtnA->setStyleSheet(set->getMiniButtonStyle());
	//filter1k8BtnA->setTextColor(QColor(200, 200, 200));
	filterBtnListA.append(filter1k8BtnA);
	CHECKED_CONNECT(filter1k8BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter2k1BtnA = new AeroButton("2k1", this);
	filter1kBtnA->setObjectName("2k1");
	filter2k1BtnA->setRoundness(0);
	//filter2k1BtnA->setGlass(false);
	filter2k1BtnA->setFixedSize(btn_widths, btn_height);
	filter2k1BtnA->setStyleSheet(set->getMiniButtonStyle());
	//filter2k1BtnA->setTextColor(QColor(200, 200, 200));
	filterBtnListA.append(filter2k1BtnA);
	CHECKED_CONNECT(filter2k1BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter2k4BtnA = new AeroButton("2k4", this);
	filter1kBtnA->setObjectName("2k4");
	filter2k4BtnA->setRoundness(0);
	//filter2k4BtnA->setGlass(false);
	filter2k4BtnA->setFixedSize(btn_widths, btn_height);
	filter2k4BtnA->setStyleSheet(set->getMiniButtonStyle());
	//filter2k4BtnA->setTextColor(QColor(200, 200, 200));
	filterBtnListA.append(filter2k4BtnA);
	CHECKED_CONNECT(filter2k4BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter2k7BtnA = new AeroButton("2k7", this);
	filter1kBtnA->setObjectName("2k7");
	filter2k7BtnA->setRoundness(0);
	//filter2k7BtnA->setGlass(false);
	filter2k7BtnA->setFixedSize(btn_widths, btn_height);
	filter2k7BtnA->setStyleSheet(set->getMiniButtonStyle());
	//filter2k7BtnA->setTextColor(QColor(200, 200, 200));
	filterBtnListA.append(filter2k7BtnA);
	CHECKED_CONNECT(filter2k7BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter2k9BtnA = new AeroButton("2k9", this);
	filter1kBtnA->setObjectName("2k9");
	filter2k9BtnA->setRoundness(0);
	//filter2k9BtnA->setGlass(false);
	filter2k9BtnA->setFixedSize(btn_widths, btn_height);
	filter2k9BtnA->setStyleSheet(set->getMiniButtonStyle());
	//filter2k9BtnA->setTextColor(QColor(200, 200, 200));
	filterBtnListA.append(filter2k9BtnA);
	CHECKED_CONNECT(filter2k9BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter3k3BtnA = new AeroButton("3k3", this);
	filter1kBtnA->setObjectName("3k3");
	filter3k3BtnA->setRoundness(0);
	//filter3k3BtnA->setGlass(false);
	filter3k3BtnA->setFixedSize(btn_widths, btn_height);
	filter3k3BtnA->setStyleSheet(set->getMiniButtonStyle());
	//filter3k3BtnA->setTextColor(QColor(200, 200, 200));
	filterBtnListA.append(filter3k3BtnA);
	CHECKED_CONNECT(filter3k3BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter3k8BtnA = new AeroButton("3k8", this);
	filter1kBtnA->setObjectName("3k8");
	filter3k8BtnA->setRoundness(0);
	//filter3k8BtnA->setGlass(false);
	filter3k8BtnA->setFixedSize(btn_widths, btn_height);
	filter3k8BtnA->setStyleSheet(set->getMiniButtonStyle());
	//filter3k8BtnA->setTextColor(QColor(200, 200, 200));
	filterBtnListA.append(filter3k8BtnA);
	CHECKED_CONNECT(filter3k8BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter4k4BtnA = new AeroButton("4k4", this);
	filter1kBtnA->setObjectName("4k4");
	filter4k4BtnA->setRoundness(0);
	//filter4k4BtnA->setGlass(false);
	filter4k4BtnA->setFixedSize(btn_widths, btn_height);
	filter4k4BtnA->setStyleSheet(set->getMiniButtonStyle());
	//filter4k4BtnA->setTextColor(QColor(200, 200, 200));
	filterBtnListA.append(filter4k4BtnA);
	CHECKED_CONNECT(filter4k4BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter5kBtnA = new AeroButton("5k", this);
	filter1kBtnA->setObjectName("5k");
	filter5kBtnA->setRoundness(0);
	//filter5kBtnA->setGlass(false);
	filter5kBtnA->setFixedSize(btn_widths, btn_height);
	filter5kBtnA->setStyleSheet(set->getMiniButtonStyle());
	//filter5kBtnA->setTextColor(QColor(200, 200, 200));
	filterBtnListA.append(filter5kBtnA);
	CHECKED_CONNECT(filter5kBtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filterVar1BtnA = new AeroButton("Var1", this);
	filter1kBtnA->setObjectName("Var1");
	filterVar1BtnA->setRoundness(0);
	//filterVar1BtnA->setGlass(false);
	filterVar1BtnA->setFixedSize(btn_widths, btn_height);
	filterVar1BtnA->setStyleSheet(set->getMiniButtonStyle());
	//filterVar1BtnA->setTextColor(QColor(200, 200, 200));
	filterBtnListA.append(filterVar1BtnA);
	CHECKED_CONNECT(filterVar1BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filterVar2BtnA = new AeroButton("Var2", this);
	filter1kBtnA->setObjectName("Var2");
	filterVar2BtnA->setRoundness(0);
	//filterVar2BtnA->setGlass(false);
	filterVar2BtnA->setFixedSize(btn_widths, btn_height);
	filterVar2BtnA->setStyleSheet(set->getMiniButtonStyle());
	//filterVar2BtnA->setTextColor(QColor(200, 200, 200));
	filterBtnListA.append(filterVar2BtnA);
	CHECKED_CONNECT(filterVar2BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	foreach(AeroButton *btn, filterBtnListA) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}
	//filterBtnListA.at(set->getCurrentDSPMode())->setBtnState(AeroButton::ON);


	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setContentsMargins(0, 0, 0, 0);
	hbox1->setSpacing(0);
	hbox1->addWidget(filter5kBtnA);
	hbox1->addWidget(filter4k4BtnA);
	hbox1->addWidget(filter3k8BtnA);
	hbox1->addWidget(filter3k3BtnA);
	hbox1->addWidget(filter2k9BtnA);
	hbox1->addWidget(filter2k7BtnA);

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setContentsMargins(0, 0, 0, 0);
	hbox2->setSpacing(0);
	hbox2->addWidget(filter2k4BtnA);
	hbox2->addWidget(filter2k1BtnA);
	hbox2->addWidget(filter1k8BtnA);
	hbox2->addWidget(filter1kBtnA);
	hbox2->addWidget(filterVar1BtnA);
	hbox2->addWidget(filterVar2BtnA);

	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(0);
	vbox->setMargin(0);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);

	filterAWidget = new QWidget();
	filterAWidget->setContentsMargins(0, 0, 0, 0);
	filterAWidget->setLayout(vbox);
}

void RadioPopupWidget::createFilterBtnWidgetB() {

	filter16kBtnB = new AeroButton("16k", this);
	filter16kBtnB->setRoundness(0);
	//filter16kBtnB->setGlass(false);
	filter16kBtnB->setFixedSize(btn_widths, btn_height);
	filter16kBtnB->setStyleSheet(set->getMiniButtonStyle());
	//filter16kBtnB->setTextColor(QColor(200, 200, 200));
	filterBtnListB.append(filter16kBtnB);
	CHECKED_CONNECT(filter16kBtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter12kBtnB = new AeroButton("12k", this);
	filter12kBtnB->setRoundness(0);
	//filter12kBtnB->setGlass(false);
	filter12kBtnB->setFixedSize(btn_widths, btn_height);
	filter12kBtnB->setStyleSheet(set->getMiniButtonStyle());
	//filter12kBtnB->setTextColor(QColor(200, 200, 200));
	filterBtnListB.append(filter12kBtnB);
	CHECKED_CONNECT(filter12kBtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter10kBtnB = new AeroButton("10k", this);
	filter10kBtnB->setRoundness(0);
	//filter10kBtnB->setGlass(false);
	filter10kBtnB->setFixedSize(btn_widths, btn_height);
	filter10kBtnB->setStyleSheet(set->getMiniButtonStyle());
	//filter10kBtnB->setTextColor(QColor(200, 200, 200));
	filterBtnListB.append(filter10kBtnB);
	CHECKED_CONNECT(filter10kBtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter8kBtnB = new AeroButton("8k", this);
	filter8kBtnB->setRoundness(0);
	//filter8kBtnB->setGlass(false);
	filter8kBtnB->setFixedSize(btn_widths, btn_height);
	filter8kBtnB->setStyleSheet(set->getMiniButtonStyle());
	//filter8kBtnB->setTextColor(QColor(200, 200, 200));
	filterBtnListB.append(filter8kBtnB);
	CHECKED_CONNECT(filter8kBtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter6k6BtnB = new AeroButton("6k6", this);
	filter6k6BtnB->setRoundness(0);
	//filter6k6BtnB->setGlass(false);
	filter6k6BtnB->setFixedSize(btn_widths, btn_height);
	filter6k6BtnB->setStyleSheet(set->getMiniButtonStyle());
	//filter6k6BtnB->setTextColor(QColor(200, 200, 200));
	filterBtnListB.append(filter6k6BtnB);
	CHECKED_CONNECT(filter6k6BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter5k2BtnB = new AeroButton("5k2", this);
	filter5k2BtnB->setRoundness(0);
	//filter5k2BtnB->setGlass(false);
	filter5k2BtnB->setFixedSize(btn_widths, btn_height);
	filter5k2BtnB->setStyleSheet(set->getMiniButtonStyle());
	//filter5k2BtnB->setTextColor(QColor(200, 200, 200));
	filterBtnListB.append(filter5k2BtnB);
	CHECKED_CONNECT(filter5k2BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter4kBtnB = new AeroButton("4k", this);
	filter4kBtnB->setRoundness(0);
	//filter4kBtnB->setGlass(false);
	filter4kBtnB->setFixedSize(btn_widths, btn_height);
	filter4kBtnB->setStyleSheet(set->getMiniButtonStyle());
	//filter4kBtnB->setTextColor(QColor(200, 200, 200));
	filterBtnListB.append(filter4kBtnB);
	CHECKED_CONNECT(filter4kBtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter3k1BtnB = new AeroButton("3k1", this);
	filter3k1BtnB->setRoundness(0);
	//filter3k1BtnB->setGlass(false);
	filter3k1BtnB->setFixedSize(btn_widths, btn_height);
	filter3k1BtnB->setStyleSheet(set->getMiniButtonStyle());
	//filter3k1BtnB->setTextColor(QColor(200, 200, 200));
	filterBtnListB.append(filter3k1BtnB);
	CHECKED_CONNECT(filter3k1BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter2k9BtnB = new AeroButton("2k9", this);
	filter2k9BtnB->setRoundness(0);
	//filter2k9BtnB->setGlass(false);
	filter2k9BtnB->setFixedSize(btn_widths, btn_height);
	filter2k9BtnB->setStyleSheet(set->getMiniButtonStyle());
	//filter2k9BtnB->setTextColor(QColor(200, 200, 200));
	filterBtnListB.append(filter2k9BtnB);
	CHECKED_CONNECT(filter2k9BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter2k4BtnB = new AeroButton("2k4", this);
	filter2k4BtnB->setRoundness(0);
	//filter2k4BtnB->setGlass(false);
	filter2k4BtnB->setFixedSize(btn_widths, btn_height);
	filter2k4BtnB->setStyleSheet(set->getMiniButtonStyle());
	//filter2k4BtnB->setTextColor(QColor(200, 200, 200));
	filterBtnListB.append(filter2k4BtnB);
	CHECKED_CONNECT(filter2k4BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filterVar1BtnB = new AeroButton("Var1", this);
	filterVar1BtnB->setRoundness(0);
	//filterVar1BtnB->setGlass(false);
	filterVar1BtnB->setFixedSize(btn_widths, btn_height);
	filterVar1BtnB->setStyleSheet(set->getMiniButtonStyle());
	//filterVar1BtnB->setTextColor(QColor(200, 200, 200));
	filterBtnListB.append(filterVar1BtnB);
	CHECKED_CONNECT(filterVar1BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filterVar2BtnB = new AeroButton("Var2", this);
	filterVar2BtnB->setRoundness(0);
	//filterVar2BtnB->setGlass(false);
	filterVar2BtnB->setFixedSize(btn_widths, btn_height);
	filterVar2BtnB->setStyleSheet(set->getMiniButtonStyle());
	//filterVar2BtnB->setTextColor(QColor(200, 200, 200));
	filterBtnListB.append(filterVar2BtnB);
	CHECKED_CONNECT(filterVar2BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	foreach(AeroButton *btn, filterBtnListB) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}
	//filterBtnListA.at(set->getCurrentDSPMode())->setBtnState(AeroButton::ON);


	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setContentsMargins(0, 0, 0, 0);
	hbox1->setSpacing(0);
	hbox1->addWidget(filter16kBtnB);
	hbox1->addWidget(filter12kBtnB);
	hbox1->addWidget(filter10kBtnB);
	hbox1->addWidget(filter8kBtnB);
	hbox1->addWidget(filter6k6BtnB);
	hbox1->addWidget(filter5k2BtnB);

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setContentsMargins(0, 0, 0, 0);
	hbox2->setSpacing(0);
	hbox2->addWidget(filter4kBtnB);
	hbox2->addWidget(filter3k1BtnB);
	hbox2->addWidget(filter2k9BtnB);
	hbox2->addWidget(filter2k4BtnB);
	hbox2->addWidget(filterVar1BtnB);
	hbox2->addWidget(filterVar2BtnB);

	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(0);
	vbox->setMargin(0);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);

	filterBWidget = new QWidget();
	filterBWidget->setContentsMargins(0, 0, 0, 0);
	filterBWidget->setLayout(vbox);
}

void RadioPopupWidget::createFilterBtnWidgetC() {

	filter1kBtnC = new AeroButton("1k", this);
	filter1kBtnC->setRoundness(0);
	//filter1kBtnC->setGlass(false);
	filter1kBtnC->setFixedSize(btn_widths, btn_height);
	filter1kBtnC->setStyleSheet(set->getMiniButtonStyle());
	//filter1kBtnC->setTextColor(QColor(200, 200, 200));
	filterBtnListC.append(filter1kBtnC);
	CHECKED_CONNECT(filter1kBtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter800BtnC = new AeroButton("800", this);
	filter800BtnC->setRoundness(0);
	//filter800BtnC->setGlass(false);
	filter800BtnC->setFixedSize(btn_widths, btn_height);
	filter800BtnC->setStyleSheet(set->getMiniButtonStyle());
	//filter800BtnC->setTextColor(QColor(200, 200, 200));
	filterBtnListC.append(filter800BtnC);
	CHECKED_CONNECT(filter800BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter750BtnC = new AeroButton("750", this);
	filter750BtnC->setRoundness(0);
	//filter750BtnC->setGlass(false);
	filter750BtnC->setFixedSize(btn_widths, btn_height);
	filter750BtnC->setStyleSheet(set->getMiniButtonStyle());
	//filter750BtnC->setTextColor(QColor(200, 200, 200));
	filterBtnListC.append(filter750BtnC);
	CHECKED_CONNECT(filter750BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter600BtnC = new AeroButton("600", this);
	filter600BtnC->setRoundness(0);
	//filter600BtnC->setGlass(false);
	filter600BtnC->setFixedSize(btn_widths, btn_height);
	filter600BtnC->setStyleSheet(set->getMiniButtonStyle());
	//filter600BtnC->setTextColor(QColor(200, 200, 200));
	filterBtnListC.append(filter600BtnC);
	CHECKED_CONNECT(filter600BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter500BtnC = new AeroButton("500", this);
	filter500BtnC->setRoundness(0);
	//filter500BtnC->setGlass(false);
	filter500BtnC->setFixedSize(btn_widths, btn_height);
	filter500BtnC->setStyleSheet(set->getMiniButtonStyle());
	//filter500BtnC->setTextColor(QColor(200, 200, 200));
	filterBtnListC.append(filter500BtnC);
	CHECKED_CONNECT(filter500BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter400BtnC = new AeroButton("400", this);
	filter400BtnC->setRoundness(0);
	//filter400BtnC->setGlass(false);
	filter400BtnC->setFixedSize(btn_widths, btn_height);
	filter400BtnC->setStyleSheet(set->getMiniButtonStyle());
	//filter400BtnC->setTextColor(QColor(200, 200, 200));
	filterBtnListC.append(filter400BtnC);
	CHECKED_CONNECT(filter400BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter250BtnC = new AeroButton("250", this);
	filter250BtnC->setRoundness(0);
	//filter250BtnC->setGlass(false);
	filter250BtnC->setFixedSize(btn_widths, btn_height);
	filter250BtnC->setStyleSheet(set->getMiniButtonStyle());
	//filter250BtnC->setTextColor(QColor(200, 200, 200));
	filterBtnListC.append(filter250BtnC);
	CHECKED_CONNECT(filter250BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter100BtnC = new AeroButton("100", this);
	filter100BtnC->setRoundness(0);
	//filter100BtnC->setGlass(false);
	filter100BtnC->setFixedSize(btn_widths, btn_height);
	filter100BtnC->setStyleSheet(set->getMiniButtonStyle());
	//filter100BtnC->setTextColor(QColor(200, 200, 200));
	filterBtnListC.append(filter100BtnC);
	CHECKED_CONNECT(filter100BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter50BtnC = new AeroButton("50", this);
	filter50BtnC->setRoundness(0);
	//filter50BtnC->setGlass(false);
	filter50BtnC->setFixedSize(btn_widths, btn_height);
	filter50BtnC->setStyleSheet(set->getMiniButtonStyle());
	//filter50BtnC->setTextColor(QColor(200, 200, 200));
	filterBtnListC.append(filter50BtnC);
	CHECKED_CONNECT(filter50BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter25BtnC = new AeroButton("25", this);
	filter25BtnC->setRoundness(0);
	//filter25BtnC->setGlass(false);
	filter25BtnC->setFixedSize(btn_widths, btn_height);
	filter25BtnC->setStyleSheet(set->getMiniButtonStyle());
	//filter25BtnC->setTextColor(QColor(200, 200, 200));
	filterBtnListC.append(filter25BtnC);
	CHECKED_CONNECT(filter25BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filterVar1BtnC = new AeroButton("Var1", this);
	filterVar1BtnC->setRoundness(0);
	//filterVar1BtnC->setGlass(false);
	filterVar1BtnC->setFixedSize(btn_widths, btn_height);
	filterVar1BtnC->setStyleSheet(set->getMiniButtonStyle());
	//filterVar1BtnC->setTextColor(QColor(200, 200, 200));
	filterBtnListC.append(filterVar1BtnC);
	CHECKED_CONNECT(filterVar1BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filterVar2BtnC = new AeroButton("Var2", this);
	filterVar2BtnC->setRoundness(0);
	//filterVar2BtnC->setGlass(false);
	filterVar2BtnC->setFixedSize(btn_widths, btn_height);
	filterVar2BtnC->setStyleSheet(set->getMiniButtonStyle());
	//filterVar2BtnC->setTextColor(QColor(200, 200, 200));
	filterBtnListC.append(filterVar2BtnC);
	CHECKED_CONNECT(filterVar2BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	foreach(AeroButton *btn, filterBtnListC) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}
	//filterBtnListC.at(set)->getCurrentDSPMode())->setBtnState(AeroButton::ON);


	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setContentsMargins(0, 0, 0, 0);
	hbox1->setSpacing(0);
	hbox1->addWidget(filter1kBtnC);
	hbox1->addWidget(filter800BtnC);
	hbox1->addWidget(filter750BtnC);
	hbox1->addWidget(filter600BtnC);
	hbox1->addWidget(filter500BtnC);
	hbox1->addWidget(filter400BtnC);

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setContentsMargins(0, 0, 0, 0);
	hbox2->setSpacing(0);
	hbox2->addWidget(filter250BtnC);
	hbox2->addWidget(filter100BtnC);
	hbox2->addWidget(filter50BtnC);
	hbox2->addWidget(filter25BtnC);
	hbox2->addWidget(filterVar1BtnC);
	hbox2->addWidget(filterVar2BtnC);

	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(0);
	vbox->setMargin(0);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);

	filterCWidget = new QWidget();
	filterCWidget->setContentsMargins(0, 0, 0, 0);
	filterCWidget->setLayout(vbox);
}

//QLabel *RadioPopupWidget::createLabel(const QString &text) {
//
//    QLabel *label = new QLabel(text, this);
//    label->setFrameStyle(QFrame::Box | QFrame::Raised);
//    return label;
//}

 
//******************************************
void RadioPopupWidget::frequencyChanged(QObject* sender, bool value, int rx, long frequency) {

	Q_UNUSED(sender)
	Q_UNUSED(value)

	if (m_currentRx != rx) return;
	m_frequency = frequency;

	HamBand band = getBandFromFrequency(set->getBandFrequencyList(), frequency);
	m_lastFrequencyList[(int) band] = m_frequency;
}

void RadioPopupWidget::bandChangedByBtn() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btn = bandBtnList.indexOf(button);

	foreach(AeroButton *btn, bandBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	set->setHamBand(this, m_currentRx, true, (HamBand) btn);

	QString str = button->text();
	if (str == "160 m")
		set->setFrequency(this, true, m_currentRx, m_lastFrequencyList.at(0));
	else
	if (str == "80 m")
		set->setFrequency(this, true, m_currentRx, m_lastFrequencyList.at(1));
	else
	if (str == "60 m")
		set->setFrequency(this, true, m_currentRx, m_lastFrequencyList.at(2));
	else
	if (str == "40 m")
		set->setFrequency(this, true, m_currentRx, m_lastFrequencyList.at(3));
	else
	if (str == "30 m")
		set->setFrequency(this, true, m_currentRx, m_lastFrequencyList.at(4));
	else
	if (str == "20 m")
		set->setFrequency(this, true, m_currentRx, m_lastFrequencyList.at(5));
	else
	if (str == "17 m")
		set->setFrequency(this, true, m_currentRx, m_lastFrequencyList.at(6));
	else
	if (str == "15 m")
		set->setFrequency(this, true, m_currentRx, m_lastFrequencyList.at(7));
	else
	if (str == "12 m")
		set->setFrequency(this, true, m_currentRx, m_lastFrequencyList.at(8));
	else
	if (str == "10 m")
		set->setFrequency(this, true, m_currentRx, m_lastFrequencyList.at(9));
	else
	if (str == "6 m")
		set->setFrequency(this, true, m_currentRx, m_lastFrequencyList.at(10));
	else
	if (str == "Gen")
		set->setFrequency(this, true, m_currentRx, m_lastFrequencyList.at(11));

}

void RadioPopupWidget::bandChanged(QObject *sender, int rx, bool byButton, HamBand band) {

	Q_UNUSED (byButton)

	if (sender == this) return;

	if (m_currentRx != rx) return;
	m_hamBand = band;

	foreach(AeroButton *btn, bandBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	bandBtnList.at(band)->setBtnState(AeroButton::ON);
	bandBtnList.at(band)->update();
}

void RadioPopupWidget::dspModeChangedByBtn() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btn = dspModeBtnList.indexOf(button);
	
	foreach(AeroButton *btn, dspModeBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	set->setDSPMode(this, m_currentRx, (DSPMode) btn);
	m_dspModeList[m_hamBand] = (DSPMode) btn;
	filterChanged(this, m_currentRx, m_filterLo, m_filterHi);
	filterGroupChanged((DSPMode) btn);

	button->setBtnState(AeroButton::ON);
	button->update();
}

void RadioPopupWidget::dspModeChanged(QObject *sender, int rx, DSPMode mode) {

	Q_UNUSED(sender)
	
	if (m_currentRx != rx) return;
	m_dspModeList[m_hamBand] = mode;

	foreach(AeroButton *btn, dspModeBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	dspModeBtnList.at(mode)->setBtnState(AeroButton::ON);
	dspModeBtnList.at(mode)->update();
}

void RadioPopupWidget::filterGroupChanged(DSPMode mode) {

	if (mode == 0 || mode == 1 || mode == 7 || mode == 9) {

		//m_filterStackedLayout->setCurrentIndex(0);
		m_filterStackedWidget->setCurrentIndex(0);
	}
	else if (mode == 2 || mode == 5 || mode == 6 || mode == 10)	{

		//m_filterStackedLayout->setCurrentIndex(1);
		m_filterStackedWidget->setCurrentIndex(1);
	}
	else if (mode == 3 || mode == 4) {

		//m_filterStackedLayout->setCurrentIndex(2);
		m_filterStackedWidget->setCurrentIndex(2);
	}
}

void RadioPopupWidget::filterChangedByBtn()	 {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	
	QList<AeroButton *> btnList;
	int filterList = 0;

	if (filterBtnListA.contains(button)) {
		
		btnList = filterBtnListA;
		filterList = 0;
	}
	else if (filterBtnListB.contains(button)) {
		
		btnList = filterBtnListB;
		filterList = 1;
	}
	else if (filterBtnListC.contains(button)) {
		
		btnList = filterBtnListC;
		filterList = 2;
	}

	foreach(AeroButton *btn, btnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	QString str = button->text();
	DSPMode dspMode = m_dspModeList.at(m_hamBand);
	switch (filterList) {

		case 0: // filterBtnList A

			if (str == "1k") {

				switch (dspMode) {

					case (DSPMode) LSB:
						m_filterLo = -1150.0f;
						m_filterHi = -150.0f;
						break;

					case (DSPMode) USB:
						m_filterLo = 150.0f;
						m_filterHi = 1150.0f;
						break;

					default:
						break;

				}
			}
			else
			if (str == "1k8") {

				switch (dspMode) {

					case (DSPMode) LSB:
						m_filterLo = -1950.0f;
						m_filterHi = -150.0f;
						break;

					case (DSPMode) USB:
						m_filterLo = 150.0f;
						m_filterHi = 1950.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "2k1") {

				switch (dspMode) {

					case (DSPMode) LSB:
						m_filterLo = -2250.0f;
						m_filterHi = -150.0f;
						break;

					case (DSPMode)  USB:
						m_filterLo = 150.0f;
						m_filterHi = 2250.0f;
						break;

					default:
						break;

				}
			}
			else
			if (str == "2k4") {

				switch (dspMode) {

					case (DSPMode) LSB:
						m_filterLo = -2550.0f;
						m_filterHi = -150.0f;
						break;

					case (DSPMode) USB:
						m_filterLo = 150.0f;
						m_filterHi = 2550.0f;
						break;

					default:
						break;

				}
			}
			else
			if (str == "2k7") {

				switch (dspMode) {

					case (DSPMode) LSB:
						m_filterLo = -2850.0f;
						m_filterHi = -150.0f;
						break;

					case (DSPMode) USB:
						m_filterLo = 150.0f;
						m_filterHi = 2850.0f;
						break;

					default:
						break;

				}
			}
			else
			if (str == "2k9") {

				switch (dspMode) {

					case (DSPMode) LSB:
						m_filterLo = -3050.0f;
						m_filterHi = -150.0f;
						break;

					case (DSPMode) USB:
						m_filterLo = 150.0f;
						m_filterHi = 3050.0f;
						break;

					default:
						break;

				}
			}
			else
			if (str == "3k3") {

				switch (dspMode) {

					case (DSPMode) LSB:
						m_filterLo = -3450.0f;
						m_filterHi = -150.0f;
						break;

					case (DSPMode) USB:
						m_filterLo = 150.0f;
						m_filterHi = 3450.0f;
						break;

					default:
						break;

				}
			}
			else
			if (str == "3k8") {

				switch (dspMode) {

					case (DSPMode) LSB:
						m_filterLo = -3950.0f;
						m_filterHi = -150.0f;
						break;

					case (DSPMode) USB:
						m_filterLo = 150.0f;
						m_filterHi = 3950.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "4k4") {

				switch (dspMode) {

					case (DSPMode) LSB:
						m_filterLo = -4550.0f;
						m_filterHi = -150.0f;
						break;

					case (DSPMode) USB:
						m_filterLo = 150.0f;
						m_filterHi = 4550.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "5k") {

				switch (dspMode) {

					case (DSPMode) LSB:
						m_filterLo = -5150.0f;
						m_filterHi = -150.0f;
						break;

					case (DSPMode) USB:
						m_filterLo = 150.0f;
						m_filterHi = 5150.0f;
						break;

					default:
						break;
				}
			}
			break;

		case 1: // filterBtnList B

			if (str == "2k4") {

				switch (dspMode) {

					case (DSPMode) DSB:
					case (DSPMode) AM:
					case (DSPMode) SAM:
						m_filterLo = -1200.0f;
						m_filterHi =  1200.0f;
						break;

					case 5:
						m_filterLo = -2000.0f;
						m_filterHi =  2000.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "2k9") {

				switch (dspMode) {

					case (DSPMode) DSB:
					case (DSPMode) AM:
					case (DSPMode) SAM:
						m_filterLo = -1450.0f;
						m_filterHi =  1450.0f;
						break;

					case 5:
						m_filterLo = -2000.0f;
						m_filterHi =  2000.0f;
						break;

					/*case 6:
						m_filterLo = -4000.0f;
						m_filterHi =  4000.0f;
						break;*/

					default:
						break;
				}
			}
			else
			if (str == "3k1") {

				switch (dspMode) {

					case (DSPMode) DSB:
					case (DSPMode) AM:
					case (DSPMode) SAM:
						m_filterLo = -1550.0f;
						m_filterHi =  1550.0f;
						break;

					case 5:
						m_filterLo = -2000.0f;
						m_filterHi =  2000.0f;
						break;

					/*case 6:
						m_filterLo = -4000.0f;
						m_filterHi =  4000.0f;
						break;*/

					default:
						break;
				}
			}
			else
			if (str == "4k") {

				switch (dspMode) {

					case (DSPMode) DSB:
					case (DSPMode) AM:
					case (DSPMode) SAM:
						m_filterLo = -2000.0f;
						m_filterHi =  2000.0f;
						break;

					case 5:
						m_filterLo = -2000.0f;
						m_filterHi =  2000.0f;
						break;

					/*case 6:
						m_filterLo = -4000.0f;
						m_filterHi =  4000.0f;
						break;*/

					default:
						break;
				}
			}
			else
			if (str == "5k2") {

				switch (dspMode) {

					case (DSPMode) DSB:
					case (DSPMode) AM:
					case (DSPMode) SAM:
						m_filterLo = -2600.0f;
						m_filterHi =  2600.0f;
						break;

					case 5:
						m_filterLo = -2000.0f;
						m_filterHi =  2000.0f;
						break;

					/*case 6:
						m_filterLo = -4000.0f;
						m_filterHi =  4000.0f;
						break;*/

					default:
						break;
				}
			}
			else
			if (str == "6k6") {

				switch (dspMode) {

					case (DSPMode) DSB:
					case (DSPMode) AM:
					case (DSPMode) SAM:
						m_filterLo = -3300.0f;
						m_filterHi =  3300.0f;
						break;

					case (DSPMode) FMN:
						m_filterLo = -2000.0f;
						m_filterHi =  2000.0f;
						break;

					/*case 6:
						m_filterLo = -4000.0f;
						m_filterHi =  4000.0f;
						break;*/

					default:
						break;
				}
			}
			else
			if (str == "8k") {

				switch (dspMode) {

					case (DSPMode) DSB:
					case (DSPMode) AM:
					case (DSPMode) SAM:
						m_filterLo = -4000.0f;
						m_filterHi =  4000.0f;
						break;

					case (DSPMode) FMN:
						m_filterLo = -2000.0f;
						m_filterHi =  2000.0f;
						break;

					/*case 6:
						m_filterLo = -4000.0f;
						m_filterHi =  4000.0f;
						break;*/

					default:
						break;
				}
			}
			else
			if (str == "10k") {

				switch (dspMode) {

					case (DSPMode) DSB:
					case (DSPMode) AM:
					case (DSPMode) SAM:
						m_filterLo = -5000.0f;
						m_filterHi =  5000.0f;
						break;

					case (DSPMode) FMN:
						m_filterLo = -2000.0f;
						m_filterHi =  2000.0f;
						break;

					/*case 6:
						m_filterLo = -4000.0f;
						m_filterHi =  4000.0f;
						break;*/

					default:
						break;
				}
			}
			else
			if (str == "12k") {

				switch (dspMode) {

					case (DSPMode) DSB:
					case (DSPMode) AM:
					case (DSPMode) SAM:
						m_filterLo = -6000.0f;
						m_filterHi =  6000.0f;
						break;

					case (DSPMode) FMN:
						m_filterLo = -2000.0f;
						m_filterHi =  2000.0f;
						break;

					/*case 6:
						m_filterLo = -4000.0f;
						m_filterHi =  4000.0f;
						break;*/

					default:
						break;
				}
			}
			else
			if (str == "16k") {

				switch (dspMode) {

					case (DSPMode) DSB:
					case (DSPMode) AM:
					case (DSPMode) SAM:
						m_filterLo = -8000.0f;
						m_filterHi =  8000.0f;
						break;

					case (DSPMode) FMN:
						m_filterLo = -2000.0f;
						m_filterHi =  2000.0f;
						break;

					/*case 6:
						m_filterLo = -4000.0f;
						m_filterHi =  4000.0f;
						break;*/

					default:
						break;
				}
			}
			break;

		case 2: // filterBtnList C

			if (str == "25") {

				switch (dspMode) {

					case (DSPMode) CWL:
						m_filterLo = -125.0f;
						m_filterHi = -100.0f;
						break;

					case (DSPMode) CWU:
						m_filterLo = 100.0f;
						m_filterHi = 125.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "50") {

				switch (dspMode) {

					case (DSPMode) CWL:
						m_filterLo = -150.0f;
						m_filterHi = -100.0f;
						break;

					case (DSPMode) CWU:
						m_filterLo = 100.0f;
						m_filterHi = 150.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "100") {

				switch (dspMode) {

					case (DSPMode) CWL:
						m_filterLo = -200.0f;
						m_filterHi = -100.0f;
						break;

					case (DSPMode) CWU:
						m_filterLo = 100.0f;
						m_filterHi = 200.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "250") {

				switch (dspMode) {

					case (DSPMode) CWL:
						m_filterLo = -350.0f;
						m_filterHi = -100.0f;
						break;

					case (DSPMode) CWU:
						m_filterLo = 100.0f;
						m_filterHi = 350.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "400") {

				switch (dspMode) {

					case (DSPMode) CWL:
						m_filterLo = -500.0f;
						m_filterHi = -100.0f;
						break;

					case (DSPMode) CWU:
						m_filterLo = 100.0f;
						m_filterHi = 500.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "500") {

				switch (dspMode) {

					case (DSPMode) CWL:
						m_filterLo = -600.0f;
						m_filterHi = -100.0f;
						break;

					case (DSPMode) CWU:
						m_filterLo = 100.0f;
						m_filterHi = 600.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "600") {

				switch (dspMode) {

					case (DSPMode) CWL:
						m_filterLo = -700.0f;
						m_filterHi = -100.0f;
						break;

					case (DSPMode) CWU:
						m_filterLo = 100.0f;
						m_filterHi = 700.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "750") {

				switch (dspMode) {

					case (DSPMode) CWL:
						m_filterLo = -850.0f;
						m_filterHi = -100.0f;
						break;

					case (DSPMode) CWU:
						m_filterLo = 100.0f;
						m_filterHi = 850.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "800") {

				switch (dspMode) {

					case (DSPMode) CWL:
						m_filterLo = -900.0f;
						m_filterHi = -100.0f;
						break;

					case (DSPMode) CWU:
						m_filterLo = 100.0f;
						m_filterHi = 900.0f;
						break;

					default:
						break;
				}
			}
			else
			if (str == "1k") {

				switch (dspMode) {

					case (DSPMode) CWL:
						m_filterLo = -1100.0f;
						m_filterHi = -100.0f;
						break;

					case (DSPMode) CWU:
						m_filterLo = 100.0f;
						m_filterHi = 1100.0f;
						break;

					default:
						break;
				}
			}
			break;
	}

	set->setRXFilter(this, m_currentRx, m_filterLo, m_filterHi);	
}

void RadioPopupWidget::filterChanged(QObject *sender, int rx, qreal low, qreal high) {

	Q_UNUSED(sender)
	
	if (m_currentRx != rx) return;
	m_filterLo = low;
	m_filterHi = high;

	DSPMode dspMode = m_dspModeList.at(m_hamBand);
	if (dspMode == (DSPMode ) LSB || dspMode == (DSPMode ) USB || dspMode == (DSPMode ) DIGU || dspMode == (DSPMode ) DIGL) {

		foreach(AeroButton *btn, filterBtnListA) {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}

		if ((m_filterLo == -5150.0 && m_filterHi == -150.0) || (m_filterLo == 150.0 && m_filterHi == 5150.0)) {
			filter5kBtnA->setBtnState(AeroButton::ON);
			filter5kBtnA->update();
		}
		else 
		if ((m_filterLo == -4550.0 && m_filterHi == -150.0) || (m_filterLo == 150.0 && m_filterHi == 4550.0)) {
			filter4k4BtnA->setBtnState(AeroButton::ON);
			filter4k4BtnA->update();
		}
		else 
		if ((m_filterLo == -3950.0 && m_filterHi == -150.0) || (m_filterLo == 150.0 && m_filterHi == 3950.0)) {
			filter3k8BtnA->setBtnState(AeroButton::ON);
			filter3k8BtnA->update();
		}
		else 
		if ((m_filterLo == -3450.0 && m_filterHi == -150.0) || (m_filterLo == 150.0 && m_filterHi == 3450.0)) {
			filter3k3BtnA->setBtnState(AeroButton::ON);
			filter3k3BtnA->update();
		}
		else 
		if ((m_filterLo == -3050.0 && m_filterHi == -150.0) || (m_filterLo == 150.0 && m_filterHi == 3050.0)) {
			filter2k9BtnA->setBtnState(AeroButton::ON);
			filter2k9BtnA->update();
		}
		else 
		if ((m_filterLo == -2850.0 && m_filterHi == -150.0) || (m_filterLo == 150.0 && m_filterHi == 2850.0)) {
			filter2k7BtnA->setBtnState(AeroButton::ON);
			filter2k7BtnA->update();
		}
		else 
		if ((m_filterLo == -2550.0 && m_filterHi == -150.0) || (m_filterLo == 150.0 && m_filterHi == 2550.0)) {
			filter2k4BtnA->setBtnState(AeroButton::ON);
			filter2k4BtnA->update();
		}
		else 
		if ((m_filterLo == -2250.0 && m_filterHi == -150.0) || (m_filterLo == 150.0 && m_filterHi == 2250.0)) {
			filter2k1BtnA->setBtnState(AeroButton::ON);
			filter2k1BtnA->update();
		}
		else 
		if ((m_filterLo == -1950.0 && m_filterHi == -150.0) || (m_filterLo == 150.0 && m_filterHi == 1950.0)) {
			filter1k8BtnA->setBtnState(AeroButton::ON);
			filter1k8BtnA->update();
		}
		else 
		if ((m_filterLo == -1150.0 && m_filterHi == -150.0) || (m_filterLo == 150.0 && m_filterHi == 1150.0)) {
			filter1kBtnA->setBtnState(AeroButton::ON);
			filter1kBtnA->update();
		}
	}
	else if (dspMode == (DSPMode ) DSB || dspMode == (DSPMode ) FMN || dspMode == (DSPMode ) AM || dspMode == (DSPMode ) SAM)	{

		// filterGroupBBox
		foreach(AeroButton *btn, filterBtnListB) {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}

		if (m_filterLo == -8000.0 && m_filterHi == 8000.0) {
			filter16kBtnB->setBtnState(AeroButton::ON);
			filter16kBtnB->update();
		}
		else
		if (m_filterLo == -6000.0 && m_filterHi == 6000.0) {
			filter12kBtnB->setBtnState(AeroButton::ON);
			filter12kBtnB->update();
		}
		else
		if (m_filterLo == -5000.0 && m_filterHi == 5000.0) {
			filter10kBtnB->setBtnState(AeroButton::ON);
			filter10kBtnB->update();
		}
		else
		if (m_filterLo == -4000.0 && m_filterHi == 4000.0) {
			filter8kBtnB->setBtnState(AeroButton::ON);
			filter8kBtnB->update();
		}
		else
		if (m_filterLo == -3300.0 && m_filterHi == 3300.0) {
			filter6k6BtnB->setBtnState(AeroButton::ON);
			filter6k6BtnB->update();
		}
		else
		if (m_filterLo == -2600.0 && m_filterHi == 2600.0) {
			filter5k2BtnB->setBtnState(AeroButton::ON);
			filter5k2BtnB->update();
		}
		else
		if (m_filterLo == -2000.0 && m_filterHi == 2000.0) {
			filter4kBtnB->setBtnState(AeroButton::ON);
			filter4kBtnB->update();
		}
		else
		if (m_filterLo == -1550.0 && m_filterHi == 1550.0) {
			filter3k1BtnB->setBtnState(AeroButton::ON);
			filter3k1BtnB->update();
		}
		else
		if (m_filterLo == -1450.0 && m_filterHi == 1450.0) {
			filter2k9BtnB->setBtnState(AeroButton::ON);
			filter2k9BtnB->update();
		}
		else
		if (m_filterLo == -1200.0 && m_filterHi == 1200.0) {
			filter2k4BtnB->setBtnState(AeroButton::ON);
			filter2k4BtnB->update();
		}
	}
	else if (dspMode == (DSPMode ) CWL || dspMode == (DSPMode ) CWU) {

		// filterGroupCBox
		foreach(AeroButton *btn, filterBtnListC) {

			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}

		if ((m_filterLo == -1100.0 && m_filterHi == -100.0) || (m_filterLo == 100.0 && m_filterHi == 1100.0)) {
			filter1kBtnC->setBtnState(AeroButton::ON);
			filter1kBtnC->update();
		}
		else
		if ((m_filterLo == -900.0 && m_filterHi == -100.0) || (m_filterLo == 100.0 && m_filterHi == 900.0)) {
			filter800BtnC->setBtnState(AeroButton::ON);
			filter800BtnC->update();
		}
		else
		if ((m_filterLo == -850.0 && m_filterHi == -100.0) || (m_filterLo == 100.0 && m_filterHi == 850.0)) {
			filter750BtnC->setBtnState(AeroButton::ON);
			filter750BtnC->update();
		}
		else
		if ((m_filterLo == -700.0 && m_filterHi == -100.0) || (m_filterLo == 100.0 && m_filterHi == 700.0)) {
			filter600BtnC->setBtnState(AeroButton::ON);
			filter600BtnC->update();
		}
		else
		if ((m_filterLo == -600.0 && m_filterHi == -100.0) || (m_filterLo == 100.0 && m_filterHi == 600.0)) {
			filter500BtnC->setBtnState(AeroButton::ON);
			filter500BtnC->update();
		}
		else
		if ((m_filterLo == -500.0 && m_filterHi == -100.0) || (m_filterLo == 100.0 && m_filterHi == 500.0)) {
			filter400BtnC->setBtnState(AeroButton::ON);
			filter400BtnC->update();
		}
		else
		if ((m_filterLo == -350.0 && m_filterHi == -100.0) || (m_filterLo == 100.0 && m_filterHi == 350.0)) {
			filter250BtnC->setBtnState(AeroButton::ON);
			filter250BtnC->update();
		}
		else
		if ((m_filterLo == -200.0 && m_filterHi == -100.0) || (m_filterLo == 100.0 && m_filterHi == 200.0)) {
			filter100BtnC->setBtnState(AeroButton::ON);
			filter100BtnC->update();
		}
		else
		if ((m_filterLo == -150.0 && m_filterHi == -100.0) || (m_filterLo == 100.0 && m_filterHi == 150.0)) {
			filter50BtnC->setBtnState(AeroButton::ON);
			filter50BtnC->update();
		}
		else
		if ((m_filterLo == -125.0 && m_filterHi == -100.0) || (m_filterLo == 100.0 && m_filterHi == 125.0)) {
			filter25BtnC->setBtnState(AeroButton::ON);
			filter25BtnC->update();
		}	
	}
}

void RadioPopupWidget::agcModeChangedByBtn() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btn = agcModeBtnList.indexOf(button);
	
	foreach(AeroButton *btn, agcModeBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	set->setAGCMode(this, m_currentRx, (AGCMode) btn);
	m_agcMode = (AGCMode) btn;
	
	button->setBtnState(AeroButton::ON);
	button->update();
}

void RadioPopupWidget::agcModeChanged(QObject *sender, int rx, AGCMode mode, bool hang) {

	Q_UNUSED(sender)
	Q_UNUSED(hang)
	
	if (m_currentRx != rx) return;
	m_agcMode = mode;

	foreach(AeroButton *btn, agcModeBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	agcModeBtnList.at(mode)->setBtnState(AeroButton::ON);
	agcModeBtnList.at(mode)->update();
}

void RadioPopupWidget::setCurrentReceiver(QObject *sender, int value) {

	Q_UNUSED(sender)

	if (m_currentRx == value) return;
	m_currentRx = value;

	TReceiver rxData = set->getReceiverDataList().at(m_currentRx);

	if (m_hamBand != rxData.hamBand) {
		m_hamBand = rxData.hamBand;

		foreach(AeroButton *btn, bandBtnList) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
		AeroButton *button = bandBtnList.at(m_hamBand);
		button->setBtnState(AeroButton::ON);
		button->update();
		
	}

	DSPMode dspMode = m_dspModeList.at(m_hamBand);
	if (dspMode != rxData.dspModeList.at(m_hamBand)) {

		dspMode = rxData.dspModeList.at(m_hamBand);

		foreach(AeroButton *btn, dspModeBtnList) {
			
			btn->setBtnState(AeroButton::OFF);
			btn->update();
		}
		AeroButton *button = dspModeBtnList.at(dspMode);
		button->setBtnState(AeroButton::ON);
		button->update();

		filterGroupChanged(dspMode);
		filterChanged(this, m_currentRx, m_filterLo, m_filterHi);
	}

	if (m_agcMode != rxData.agcMode) {
		m_agcMode = rxData.agcMode;
	}

	if (m_filterLo != rxData.filterLo || m_filterHi != rxData.filterHi) {

		m_filterLo = rxData.filterLo;
		m_filterHi = rxData.filterHi;
		filterChanged(this, m_currentRx, m_filterLo, m_filterHi);
	}
}
 
// **********************
bool RadioPopupWidget::showPopupWidget(QObject *sender, QPoint position) {

	Q_UNUSED(sender)

	m_mouseOver = true;
	show();

	QDesktopWidget *desktop = QApplication::desktop();
	QRect desktopRect = desktop->availableGeometry(desktop->screenNumber());

	position.setX(position.x() - frameGeometry().width() / 2);
	position.setY(position.y() - frameGeometry().height() / 2);

	move(position);

	// stop us being lost off the edge of the screen
	if (frameGeometry().right() > desktopRect.right()) move(QPoint(desktopRect.right() - frameGeometry().width(), frameGeometry().top()));
	if (frameGeometry().bottom() > desktopRect.bottom()) move(QPoint(frameGeometry().left(), desktopRect.bottom() - frameGeometry().height()));
	if (frameGeometry().left() < desktopRect.left()) move(QPoint(desktopRect.left(), frameGeometry().top()));
	if (frameGeometry().top() < desktopRect.top()) move(QPoint(frameGeometry().left(), desktopRect.top()));

	setFocus();

	return true;
}

void RadioPopupWidget::systemStateChanged(
		QObject *sender, 
		QSDR::_Error err, 
		QSDR::_HWInterfaceMode hwmode, 
		QSDR::_ServerMode mode, 
		QSDR::_DataEngineState state
) {
	Q_UNUSED(sender)
	Q_UNUSED(err)
	Q_UNUSED(hwmode)
	Q_UNUSED(state)

}

void RadioPopupWidget::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void RadioPopupWidget::showEvent(QShowEvent *event) {

	m_closingTimer.restart();
	//emit showEvent(this);

	QWidget::showEvent(event);
}

void RadioPopupWidget::hideEvent(QHideEvent *event) {

	//emit hidingEvent(this);
	QWidget::hideEvent(event);
}

void RadioPopupWidget::paintEvent(QPaintEvent *event) {

	QWidget::paintEvent(event);
}

void RadioPopupWidget::resizeEvent(QResizeEvent *event) {

	//createBackground(event->size());

	setFixedSize(event->size());
	QWidget::resizeEvent(event);
}

void RadioPopupWidget::mousePressEvent(QMouseEvent *event) {

	m_closingTimer.restart();

	m_mouseDownPos = QCursor::pos();
	m_mouseDownWindowPos = pos();

	QWidget::mousePressEvent(event);
}

void RadioPopupWidget::mouseMoveEvent(QMouseEvent *event) {

	m_closingTimer.restart();

	if (event->buttons() == Qt::LeftButton) {

		QPoint delta = QCursor::pos() - m_mouseDownPos;
		QPoint new_pos = m_mouseDownWindowPos + delta;

		QDesktopWidget *desktop = QApplication::desktop();
//		QRect desktopRect = desktop->availableGeometry();
		QRect desktopRect = desktop->availableGeometry(desktop->screenNumber());
//		QRect desktopRect = desktop->screenGeometry(desktop->screenNumber());
//		QRect desktopRect = desktop->geometry();

		QRect new_rect(QPoint(0, 0), size());
		new_rect.moveTopLeft(new_pos);

		// stop us being lost off the edge of the screen
		if (new_rect.right() > desktopRect.right()) new_rect.moveLeft(desktopRect.right() - (new_rect.width() - 1));
		if (new_rect.bottom() > desktopRect.bottom()) new_rect.moveTop(desktopRect.bottom() - (new_rect.height() - 1));
		if (new_rect.left() < desktopRect.left()) new_rect.moveLeft(desktopRect.left());
		if (new_rect.top() < desktopRect.top()) new_rect.moveTop(desktopRect.top());

		move(new_rect.topLeft());
	}

	QWidget::mouseMoveEvent(event);
}

void RadioPopupWidget::mouseReleaseEvent(QMouseEvent *event) {

	m_closingTimer.restart();
	QWidget::mouseReleaseEvent(event);
}

void RadioPopupWidget::enterEvent(QEvent *event) {

	m_closingTimer.restart();
	m_mouseOver = true;
	QWidget::enterEvent(event);
}

void RadioPopupWidget::leaveEvent(QEvent *event) {

	m_closingTimer.restart();
	m_mouseOver = false;
	QWidget::leaveEvent(event);
}

void RadioPopupWidget::timerEvent(QTimerEvent *event) {

	int timerId = event->timerId();

	if (m_timerID > 0 && timerId == m_timerID) {

		if (!m_mouseOver && isVisible()) {

			if (m_closingTimer.elapsed() >= 500) close();
		}
		return;
	}

	QWidget::timerEvent(event);
}

bool RadioPopupWidget::event(QEvent *event) {

	switch (event->type())
	{

		case QEvent::NonClientAreaMouseButtonPress:
			// mouse press on the title bar or window edges
			m_closingTimer.restart();
			m_mouseOver = true;
			break;

		case QEvent::NonClientAreaMouseMove:
			// mouse move on the title bar or window edges
			m_closingTimer.restart();
//			mouse_over = true;
			break;

		case QEvent::NonClientAreaMouseButtonRelease:
			// mouse press on the title bar or window edges
			m_closingTimer.restart();
			m_mouseOver = false;
			break;

		default:
			break;
	}

	return QWidget::event(event);
}

