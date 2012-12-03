/**
* @file cusdr_radioWidget.cpp
* @brief Radio control widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-02-10
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
 
#include <QtGui>
#include <QDebug>

#include "cusdr_radioWidget.h"

#define	btn_height		14
#define	btn_width		60
#define	btn_widthb		70
#define	btn_widths		34


RadioWidget::RadioWidget(QWidget *parent) 
	: QWidget(parent)
	, set(Settings::instance())
	, m_currentRx(set->getCurrentReceiver())
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(set->getMinimumGroupBoxWidth())
{
	//setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 0, 4, 0);
	
	m_receiverDataList = set->getReceiverDataList();

	m_hamBand = m_receiverDataList.at(0).hamBand;
	//m_dspMode = m_receiverDataList.at(0).dspMode;
	m_dspModeList = m_receiverDataList.at(0).dspModeList;
	//m_agcMode = m_receiverDataList.at(0).agcMode;
	m_filterMode = m_receiverDataList.at(0).defaultFilterMode;
	m_filterLo = m_receiverDataList.at(0).filterLo;
	m_filterHi = m_receiverDataList.at(0).filterHi;

	m_lastFrequencyList = m_receiverDataList.at(0).lastFrequencyList;

	createBandBtnGroup();
	createModeBtnGroup();
	createFilterBtnGroupA();
	createFilterBtnGroupB();
	createFilterBtnGroupC();

	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setContentsMargins(0, 0, 0, 0);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(0);
	hbox1->setMargin(0);
	hbox1->addStretch();
	hbox1->addWidget(bandGroupBox);

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(0);
	hbox2->setMargin(0);
	hbox2->addStretch();
	hbox2->addWidget(modeGroupBox);

	QHBoxLayout *hbox3 = new QHBoxLayout();
	hbox3->setSpacing(0);
	hbox3->setMargin(0);
	hbox3->addStretch();
	hbox3->addWidget(filterGroupABox);

	QHBoxLayout *hbox4 = new QHBoxLayout();
	hbox4->setSpacing(0);
	hbox4->setMargin(0);
	hbox4->addStretch();
	hbox4->addWidget(filterGroupBBox);

	QHBoxLayout *hbox5 = new QHBoxLayout();
	hbox5->setSpacing(0);
	hbox5->setMargin(0);
	hbox5->addStretch();
	hbox5->addWidget(filterGroupCBox);

	QHBoxLayout *hbox6 = new QHBoxLayout();
	hbox6->setSpacing(0);
	hbox6->setMargin(0);
	hbox6->addStretch();
	hbox6->addWidget(mercuryBtnGroup());

	mainLayout->addLayout(hbox1);
	mainLayout->addLayout(hbox2);
	mainLayout->addLayout(hbox3);
	mainLayout->addLayout(hbox4);
	mainLayout->addLayout(hbox5);
	mainLayout->addLayout(hbox6);
	mainLayout->addStretch();
	
	// setup values from settings.ini
	bandBtnList.at(m_hamBand)->setBtnState(AeroButton::ON);
	bandBtnList.at(m_hamBand)->update();

	dspModeChanged(this, 0, m_dspModeList.at(m_hamBand));
	filterChanged(this, 0, m_filterLo, m_filterHi);

	DSPMode dspMode = m_dspModeList.at(m_hamBand);

	if (dspMode == (DSPMode) LSB || dspMode == (DSPMode) USB || dspMode == (DSPMode) DIGU || dspMode == (DSPMode) DIGL) {

		filterGroupBBox->hide();
		filterGroupCBox->hide();
	}
	else 
	if (dspMode == (DSPMode) DSB || dspMode == (DSPMode) FMN || dspMode == (DSPMode) AM || dspMode == (DSPMode) SAM) {

		filterGroupABox->hide();
		filterGroupCBox->hide();
	}
	else 
	if (dspMode == (DSPMode) CWL || dspMode == (DSPMode) CWU) {

		filterGroupABox->hide();
		filterGroupBBox->hide();
	}

	setLayout(mainLayout);
	setupConnections();
}

RadioWidget::~RadioWidget() {

	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

QSize RadioWidget::sizeHint() const {
	
	return QSize(m_minimumWidgetWidth, height());
}

QSize RadioWidget::minimumSizeHint() const {

	return QSize(m_minimumWidgetWidth, height());
}

void RadioWidget::setupConnections() {

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

//	CHECKED_CONNECT(
//		set,
//		SIGNAL(agcModeChanged(QObject *, int, AGCMode)),
//		this,
//		SLOT(agcModeChanged(QObject *, int, AGCMode)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(filterFrequenciesChanged(QObject *, int, qreal, qreal)), 
		this, 
		SLOT(filterChanged(QObject *, int, qreal, qreal)));

	CHECKED_CONNECT(
		set,
		SIGNAL(mercuryAttenuatorChanged(QObject *, HamBand, int)),
		this,
		SLOT(setMercuryAttenuator(QObject *, HamBand, int)));
}

void RadioWidget::createBandBtnGroup() {

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
	
	// only for testing
	//current_band = 1; // 80 m
	//band80mBtn->setBtnState(AeroButton::ON);

	QGridLayout *layout = new QGridLayout();
	layout->setVerticalSpacing(1);
	layout->setHorizontalSpacing(1);
	layout->setContentsMargins(0, 0, 0, 0);
	layout->addWidget(band160mBtn, 0, 0);
	layout->addWidget(band80mBtn, 0, 1);
	layout->addWidget(band60mBtn, 0, 2);
	layout->addWidget(band40mBtn, 0, 3);
	layout->addWidget(band30mBtn, 0, 4);
	layout->addWidget(band20mBtn, 0, 5);
	layout->addWidget(band17mBtn, 1, 0);
	layout->addWidget(band15mBtn, 1, 1);
	layout->addWidget(band12mBtn, 1, 2);
	layout->addWidget(band10mBtn, 1, 3);
	layout->addWidget(band6mBtn,  1, 4);
	layout->addWidget(bandGenBtn, 1, 5);
	//layout->addWidget(bandxxBtn, 2, 3);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addLayout(layout);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(1);
	//vbox->addLayout(layout);
	vbox->addLayout(hbox1);

	bandGroupBox = new QGroupBox(tr("Band"), this);
	bandGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	bandGroupBox->setLayout(vbox);
	bandGroupBox->setStyleSheet(set->getWidgetStyle());
	//bandGroupBox->setMinimumWidth(100);
	bandGroupBox->setFont(QFont("Arial", 8));
}

void RadioWidget::createModeBtnGroup() {

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

	QGridLayout *layout = new QGridLayout();
	layout->setVerticalSpacing(1);
	layout->setHorizontalSpacing(1);
	layout->setContentsMargins(0, 0, 0, 0);
	layout->addWidget(lsbBtn, 0, 0);
	layout->addWidget(usbBtn, 0, 1);
	layout->addWidget(dsbBtn, 0, 2);
	layout->addWidget(cwlBtn, 0, 3);
	layout->addWidget(cwuBtn, 0, 4);
	layout->addWidget(fmnBtn, 0, 5);
	layout->addWidget(amBtn,  1, 0);
	layout->addWidget(diguBtn, 1, 1);
	layout->addWidget(specBtn, 1, 2);
	layout->addWidget(diglBtn, 1, 3);
	layout->addWidget(samBtn,  1, 4);
	layout->addWidget(drmBtn,  1, 5);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addLayout(layout);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(1);
	vbox->addLayout(hbox1);

	modeGroupBox = new QGroupBox(tr("Mode"), this);
	modeGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	modeGroupBox->setLayout(vbox);
	modeGroupBox->setStyleSheet(set->getWidgetStyle());
	//modeGroupBox->setMinimumWidth(100);
	modeGroupBox->setFont(QFont("Arial", 8));
}

void RadioWidget::createFilterBtnGroupA() {

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

	QGridLayout *layout = new QGridLayout;
	layout->setVerticalSpacing(1);
	layout->setHorizontalSpacing(1);
	layout->setContentsMargins(0, 0, 0, 0);
	layout->addWidget(filter5kBtnA,  0, 0);
	layout->addWidget(filter4k4BtnA, 0, 1);
	layout->addWidget(filter3k8BtnA, 0, 2);
	layout->addWidget(filter3k3BtnA, 0, 3);
	layout->addWidget(filter2k9BtnA, 0, 4);
	layout->addWidget(filter2k7BtnA, 0, 5);
	layout->addWidget(filter2k4BtnA, 1, 0);
	layout->addWidget(filter2k1BtnA, 1, 1);
	layout->addWidget(filter1k8BtnA, 1, 2);
	layout->addWidget(filter1kBtnA,  1, 3);	
	layout->addWidget(filterVar1BtnA, 1, 4);
	layout->addWidget(filterVar2BtnA, 1, 5);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addLayout(layout);

	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(1);
	vbox->addLayout(hbox1);

	filterGroupABox = new QGroupBox(tr("Filter"), this);
	filterGroupABox->setMinimumWidth(m_minimumGroupBoxWidth);
	filterGroupABox->setLayout(vbox);
	filterGroupABox->setStyleSheet(set->getWidgetStyle());
	filterGroupABox->setFont(QFont("Arial", 8));
}

void RadioWidget::createFilterBtnGroupB() {

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

	QGridLayout *layout = new QGridLayout;
	layout->setVerticalSpacing(1);
	layout->setHorizontalSpacing(1);
	layout->setContentsMargins(0, 0, 0, 0);
	layout->addWidget(filter16kBtnB, 0, 0);
	layout->addWidget(filter12kBtnB, 0, 1);
	layout->addWidget(filter10kBtnB, 0, 2);
	layout->addWidget(filter8kBtnB,  0, 3);
	layout->addWidget(filter6k6BtnB, 0, 4);
	layout->addWidget(filter5k2BtnB, 0, 5);
	layout->addWidget(filter4kBtnB,  1, 0);
	layout->addWidget(filter3k1BtnB, 1, 1);
	layout->addWidget(filter2k9BtnB, 1, 2);
	layout->addWidget(filter2k4BtnB, 1, 3);	
	layout->addWidget(filterVar1BtnB, 1, 4);
	layout->addWidget(filterVar2BtnB, 1, 5);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addLayout(layout);

	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(1);
	vbox->addLayout(hbox1);

	filterGroupBBox = new QGroupBox(tr("Filter"), this);
	filterGroupBBox->setMinimumWidth(m_minimumGroupBoxWidth);
	filterGroupBBox->setLayout(vbox);
	filterGroupBBox->setStyleSheet(set->getWidgetStyle());
	filterGroupBBox->setFont(QFont("Arial", 8));
}

void RadioWidget::createFilterBtnGroupC() {

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

	QGridLayout *layout = new QGridLayout();
	layout->setVerticalSpacing(1);
	layout->setHorizontalSpacing(1);
	layout->setContentsMargins(0, 0, 0, 0);
	layout->addWidget(filter1kBtnC,  0, 0);
	layout->addWidget(filter800BtnC, 0, 1);
	layout->addWidget(filter750BtnC, 0, 2);
	layout->addWidget(filter600BtnC, 0, 3);
	layout->addWidget(filter500BtnC, 0, 4);
	layout->addWidget(filter400BtnC, 0, 5);
	layout->addWidget(filter250BtnC, 1, 0);
	layout->addWidget(filter100BtnC, 1, 1);
	layout->addWidget(filter50BtnC,  1, 2);
	layout->addWidget(filter25BtnC,  1, 3);	
	layout->addWidget(filterVar1BtnC, 1, 4);
	layout->addWidget(filterVar2BtnC, 1, 5);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addLayout(layout);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(1);
	vbox->addLayout(hbox1);

	filterGroupCBox = new QGroupBox(tr("Filter"), this);
	filterGroupCBox->setMinimumWidth(m_minimumGroupBoxWidth);
	filterGroupCBox->setLayout(vbox);
	filterGroupCBox->setStyleSheet(set->getWidgetStyle());
	filterGroupCBox->setFont(QFont("Arial", 8));
}

QGroupBox *RadioWidget::mercuryBtnGroup() {

	attenuatorBtn = new AeroButton("Attn.", this);
	attenuatorBtn->setRoundness(0);
	attenuatorBtn->setFixedSize (50, btn_height);
	attenuatorBtn->setStyleSheet(set->getMiniButtonStyle());
	connect(attenuatorBtn, SIGNAL(released()), this, SLOT(attenuatorChanged()));

	ditherBtn = new AeroButton("Dither", this);
	ditherBtn->setRoundness(0);
	//ditherBtn->setGlass(false);
	ditherBtn->setFixedSize (50, btn_height);
	ditherBtn->setStyleSheet(set->getMiniButtonStyle());
	//ditherBtn->setTextColor(QColor(200, 200, 200));
	connect(ditherBtn, SIGNAL(released()), this, SLOT(ditherChanged()));

	randomBtn = new AeroButton("Rand", this);
	randomBtn->setRoundness(0);
	//randomBtn->setGlass(false);
	randomBtn->setFixedSize(50, btn_height);
	randomBtn->setStyleSheet(set->getMiniButtonStyle());
	//randomBtn->setTextColor(QColor(200, 200, 200));
	CHECKED_CONNECT(randomBtn, SIGNAL(released()), this, SLOT(randomChanged()));
	
	HamBand band = m_receiverDataList.at(0).hamBand;

	if (m_receiverDataList.at(0).mercuryAttenuators.at(band)) {

		attenuatorBtn->setBtnState(AeroButton::OFF);
		attenuatorBtn->setText("Att 0dB");
	}
	else {

		attenuatorBtn->setBtnState(AeroButton::ON);
		attenuatorBtn->setText("Att -20dB");
	}

	if(set->getMercuryDither())
		ditherBtn->setBtnState(AeroButton::ON);

	if(set->getMercuryRandom())
		randomBtn->setBtnState(AeroButton::ON);
		
	QGridLayout *layout = new QGridLayout();
	layout->setVerticalSpacing(1);
	layout->setHorizontalSpacing(1);
	layout->setContentsMargins(0, 0, 0, 0);
	layout->addWidget(attenuatorBtn, 0, 0);
	layout->addWidget(ditherBtn, 0, 1);
	layout->addWidget(randomBtn, 0, 2);
	
	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addLayout(layout);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(1);
	vbox->addLayout(hbox1);
	
	QGroupBox *groupBox = new QGroupBox(tr("Mercury"), this);
	groupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	groupBox->setLayout(vbox);
	groupBox->setStyleSheet(set->getWidgetStyle());
	//groupBox->setMinimumWidth(175);
	groupBox->setFont(QFont("Arial", 8));

	return groupBox;
}

QLabel *RadioWidget::createLabel(const QString &text) {

    QLabel *label = new QLabel(text, this);
    label->setFrameStyle(QFrame::Box | QFrame::Raised);
    return label;
}

 
//******************************************
void RadioWidget::frequencyChanged(QObject* sender, bool value, int rx, long frequency) {

	Q_UNUSED(sender)
	Q_UNUSED(value)

	if (m_currentRx != rx) return;
	m_frequency = frequency;

	HamBand band = getBandFromFrequency(set->getBandFrequencyList(), frequency);
	m_lastFrequencyList[(int) band] = m_frequency;
}

void RadioWidget::bandChangedByBtn() {

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

void RadioWidget::bandChanged(QObject *sender, int rx, bool byButton, HamBand band) {

	Q_UNUSED (byButton)

	if (sender == this) return;

	if (m_currentRx != rx) return;
	m_hamBand = band;

	foreach(AeroButton *btn, bandBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	//m_lastFrequencyList[(int) band] = m_frequency;
	bandBtnList.at(band)->setBtnState(AeroButton::ON);
	bandBtnList.at(band)->update();
}

void RadioWidget::dspModeChangedByBtn() {

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

void RadioWidget::dspModeChanged(QObject *sender, int rx, DSPMode mode) {

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

void RadioWidget::filterGroupChanged(DSPMode mode) {

	if (mode == 0 || mode == 1 || mode == 7 || mode == 9) {

		filterGroupBBox->hide();
		filterGroupCBox->hide();
		filterGroupABox->show();
	}
	else if (mode == 2 || mode == 5 || mode == 6 || mode == 10)	{

		filterGroupABox->hide();
		filterGroupCBox->hide();
		filterGroupBBox->show();
	}
	else if (mode == 3 || mode == 4) {

		filterGroupABox->hide();
		filterGroupBBox->hide();
		filterGroupCBox->show();
	}
	/*else if (mode == 8 || mode == 11)
	{
		filterGroupA->hide();
		filterGroupB->hide();
		filterGroupC->hide();
	}*/
}

void RadioWidget::filterChangedByBtn()	 {

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

	DSPMode mode = m_dspModeList.at(m_hamBand);
	switch (filterList) {

		case 0: // filterBtnList A

			if (str == "1k") {

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

				switch (mode) {

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

void RadioWidget::filterChanged(QObject *sender, int rx, qreal low, qreal high) {

	Q_UNUSED(sender)
	
	if (m_currentRx != rx) return;
	m_filterLo = low;
	m_filterHi = high;

	DSPMode mode = m_dspModeList.at(m_hamBand);
	if (mode == (DSPMode ) LSB || mode == (DSPMode ) USB || mode == (DSPMode ) DIGU || mode == (DSPMode ) DIGL) {

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
	else if (mode == (DSPMode ) DSB || mode == (DSPMode ) FMN || mode == (DSPMode ) AM || mode == (DSPMode ) SAM)	{

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
	else if (mode == (DSPMode ) CWL || mode == (DSPMode ) CWU) {

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

void RadioWidget::attenuatorChanged() {

	if (attenuatorBtn->btnState() == AeroButton::OFF) {
		
		set->setMercuryAttenuator(this, 0);
		attenuatorBtn->setBtnState(AeroButton::ON);
		attenuatorBtn->setText("Att -20dB");
		emit newMessage("[hpsdr]: attenuator -20 dB.");
	}
	else {

		set->setMercuryAttenuator(this, 1);
		attenuatorBtn->setBtnState(AeroButton::OFF);
		attenuatorBtn->setText("Att 0dB");
		emit newMessage("[hpsdr]: attenuator 0 dB.");
	}
}

void RadioWidget::setMercuryAttenuator(QObject *sender, HamBand band, int value) {

	Q_UNUSED(sender)
	Q_UNUSED(band)

	if (value) {

		attenuatorBtn->setBtnState(AeroButton::OFF);
		attenuatorBtn->setText("Att 0dB");
		emit newMessage("[hpsdr]: attenuator 0 dB.");
	}
	else {

		attenuatorBtn->setBtnState(AeroButton::ON);
		attenuatorBtn->setText("Att -20dB");
		emit newMessage("[hpsdr]: attenuator -20 dB.");
	}
}

void RadioWidget::ditherChanged() {

	if (ditherBtn->btnState() == AeroButton::OFF) {
		
		set->setDither(this, true);
		ditherBtn->setBtnState(AeroButton::ON);
		emit newMessage("[hpsdr]: dither on.");

	} else {

		set->setDither(this, false);
		ditherBtn->setBtnState(AeroButton::OFF);
		emit newMessage("[hpsdr]: dither off.");
	}
}

void RadioWidget::randomChanged() {

	if (randomBtn->btnState() == AeroButton::OFF) {
		
		set->setRandom(this, true);
		randomBtn->setBtnState(AeroButton::ON);
		emit newMessage("[hpsdr]: random on.");

	} else {

		set->setRandom(this, false);
		randomBtn->setBtnState(AeroButton::OFF);
		emit newMessage("[hpsdr]: random off.");
	}
}

void RadioWidget::setCurrentReceiver(QObject *sender, int value) {

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

//	DSPMode mode = m_dspModeList.at(m_hamBand);
//	if (mode != rxData.dspModeList.at(m_hamBand)) {
//
//		m_dspModeList[m_hamBand] = rxData.dspModeList.at(m_hamBand);
//
//		foreach(AeroButton *btn, dspModeBtnList) {
//
//			btn->setBtnState(AeroButton::OFF);
//			btn->update();
//		}
//		qDebug() << "***********************    RadioWidget: DSPmode changed by changed receiver!";
//		AeroButton *button = dspModeBtnList.at(mode);
//		button->setBtnState(AeroButton::ON);
//		button->update();
//
//		filterGroupChanged(mode);
//		filterChanged(this, m_currentRx, m_filterLo, m_filterHi);
//	}

//	if (m_agcMode != rxData.agcMode) {
//		m_agcMode = rxData.agcMode;
//	}

	if (m_filterLo != rxData.filterLo || m_filterHi != rxData.filterHi) {

		m_filterLo = rxData.filterLo;
		m_filterHi = rxData.filterHi;
		filterChanged(this, m_currentRx, m_filterLo, m_filterHi);
	}
}
 
// **********************

void RadioWidget::systemStateChanged(
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

void RadioWidget::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void RadioWidget::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}


