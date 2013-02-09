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

#define LOG_RADIOPOPUP

// use: RADIOPOPUP_DEBUG

#include "cusdr_radioPopupWidget.h"

#define	btn_height		14
#define	btn_height1		16
#define	btn_width		40
#define	btn_widthb		66
#define	btn_widths		34


RadioPopupWidget::RadioPopupWidget(QWidget *parent, int rx)
	: QWidget(parent)
	, set(Settings::instance())
	, m_sticky(false)
	, m_receiver(rx)
	, m_currentRx(set->getCurrentReceiver())
	, m_minimumWidgetWidth(210)
	, m_minimumGroupBoxWidth(set->getMinimumGroupBoxWidth())
{
	setWindowFlags(Qt::Tool | Qt::FramelessWindowHint | Qt::WindowStaysOnTopHint);
	
	setMouseTracking(true);
	setContentsMargins(4, 4, 4, 4);
	setWindowOpacity(0.9);

		
	//setFixedWidth(206);
	//setFixedWidth(210);
	//setMinimumHeight(100);
	//setFixedHeight(240);

	setFocusPolicy(Qt::StrongFocus);

	QString	style = QString::fromUtf8(
			"border: 0px solid rgba(166, 196, 208, 255);"
			//"border: 1px solid;"// rgba(166, 196, 208, 255);"
			//"border-left-color: rgba(220, 120, 120, 255);"
			//"border-top-color: rgba(220, 120, 120, 255);"
			//"border-right-color: rgba(0, 0, 0, 255);"
			//"border-bottom-color: rgba(0, 0, 0, 255);"
			"color: rgb(166, 196, 208); "
			//"background-color: rgba(40, 40, 40, 255); ");
			"background-color: rgba(25, 25, 25, 255); ");

	setStyleSheet(style);

	fonts = new CFonts(this);
	m_fonts = fonts->getFonts();

	m_receiverDataList = set->getReceiverDataList();

	m_hamBand = m_receiverDataList.at(m_receiver).hamBand;
	//m_dspMode = m_receiverDataList.at(m_receiver).dspMode;
	m_dspModeList = m_receiverDataList.at(m_receiver).dspModeList;
	m_agcMode = m_receiverDataList.at(m_receiver).agcMode;
	m_filterMode = m_receiverDataList.at(m_receiver).defaultFilterMode;
	m_filterLo = m_receiverDataList.at(m_receiver).filterLo;
	m_filterHi = m_receiverDataList.at(m_receiver).filterHi;

	m_spectrumAveraging = m_receiverDataList.at(m_receiver).spectrumAveraging;
	m_panGrid = m_receiverDataList.at(m_receiver).panGrid;
	m_peakHold = m_receiverDataList.at(m_receiver).peakHold;
	m_panLocked = m_receiverDataList.at(m_receiver).panLocked;
	m_clickVFO = m_receiverDataList.at(m_receiver).clickVFO;
	m_showCross = m_receiverDataList.at(m_receiver).hairCross;

	m_panadapterMode = m_receiverDataList.at(m_receiver).panMode;
	m_waterfallColorMode = m_receiverDataList.at(m_receiver).waterfallMode;

	m_lastCtrFrequencyList = m_receiverDataList.at(m_receiver).lastCenterFrequencyList;
	m_lastVfoFrequencyList = m_receiverDataList.at(m_receiver).lastVfoFrequencyList;

	stickyBtn = new AeroButton("Lock", this);
	stickyBtn->setRoundness(0);
	stickyBtn->setGlass(false);
	QColor col = QColor(35, 35, 35);
	stickyBtn->setColor(col);
	col = QColor(120, 120, 120);
	stickyBtn->setHighlight(col);
	col = QColor(160, 100, 100);
	stickyBtn->setColorOn(col);
	stickyBtn->setFixedSize(btn_width, btn_height1);
	//stickyBtn->move(size().width() - btn_width - 2, 2);
	stickyBtn->setBtnState(AeroButton::OFF);
	CHECKED_CONNECT(stickyBtn, SIGNAL(clicked()), this, SLOT(setSticky()));

	createOptionsBtnGroup();
	createBandBtnGroup();
	createModeBtnGroup();
	createAgcBtnGroup();
	createFilterBtnWidgetA();
	createFilterBtnWidgetB();
	createFilterBtnWidgetC();

	m_filterStackedWidget = new QStackedWidget(this);
	m_filterStackedWidget->setContentsMargins(0, 0, 0, 0);
	m_filterStackedWidget->setFixedHeight(30);
	m_filterStackedWidget->addWidget(filterAWidget);
	m_filterStackedWidget->addWidget(filterBWidget);
	m_filterStackedWidget->addWidget(filterCWidget);
	
	
	QString rxStr = tr(" Rx %1 ");
	QLabel* rxLabel = new QLabel(rxStr.arg(m_receiver + 1));
	rxLabel->setStyleSheet("background-color: rgba(40, 40, 40, 255);");
	
	QHBoxLayout* title = new QHBoxLayout();
	title->setContentsMargins(0, 0, 0, 0);
	title->setSpacing(0);
	title->addWidget(rxLabel);
	title->addStretch();
	title->addWidget(stickyBtn);

	QVBoxLayout *mainLayout = new QVBoxLayout;
	mainLayout->setSpacing(0);
	mainLayout->setMargin(0);
	mainLayout->addLayout(title);
	mainLayout->addSpacing(16);
	//mainLayout->addStretch();
	mainLayout->addLayout(optionsVBox);
	mainLayout->addSpacing(16);
	mainLayout->addLayout(bandVBox);
	mainLayout->addSpacing(8);
	mainLayout->addLayout(modeVBox);
	mainLayout->addSpacing(8);
	mainLayout->addWidget(m_filterStackedWidget);
	mainLayout->addSpacing(16);
	mainLayout->addLayout(agcVBox);
	mainLayout->addStretch();

	setLayout(mainLayout);
	
	// setup values from settings.ini
	bandBtnList.at(m_hamBand)->setBtnState(AeroButton::ON);
	bandBtnList.at(m_hamBand)->update();

	dspModeChanged(this, 0, m_dspModeList.at(m_hamBand));
	agcModeChanged(this, 0, m_agcMode, false);
	filterChanged(this, 0, m_filterLo, m_filterHi);

	DSPMode dspMode = m_dspModeList.at(m_hamBand);
	if (dspMode == (DSPMode) LSB || dspMode == (DSPMode) USB || dspMode == (DSPMode) DIGU || dspMode == (DSPMode) DIGL) {

		m_filterStackedWidget->setCurrentIndex(0);
	}
	else
	if (dspMode == (DSPMode) DSB || dspMode == (DSPMode) FMN || dspMode == (DSPMode) AM || dspMode == (DSPMode) SAM) {

		m_filterStackedWidget->setCurrentIndex(1);
	}
	else
	if (dspMode == (DSPMode) CWL || dspMode == (DSPMode) CWU) {

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

//QSize RadioPopupWidget::sizeHint() const {
//	
//	return QSize(m_minimumWidgetWidth, height());
//}

QSize RadioPopupWidget::minimumSizeHint() const {

	return QSize(m_minimumWidgetWidth, height());
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
		SIGNAL(graphicModeChanged(
					QObject *,
					int,
					PanGraphicsMode,
					WaterfallColorMode)),
		this, 
		SLOT(graphicModeChanged(
					QObject *,
					int,
					PanGraphicsMode,
					WaterfallColorMode)));

	CHECKED_CONNECT(
		set,
		SIGNAL(vfoFrequencyChanged(QObject *, int, int, long)),
		this,
		SLOT(vfoFrequencyChanged(QObject *, int, int, long)));

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

void RadioPopupWidget::createOptionsBtnGroup() {

	//m_optionsLabel = new QLabel("Options:", this);
    //m_optionsLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	//m_optionsLabel->setStyleSheet(set->getLabelStyle());

	avgBtn = new AeroButton("Pan Avg", this);
	avgBtn->setRoundness(10);
	avgBtn->setFont(m_fonts.smallFont);
	
	if (m_spectrumAveraging)
		avgBtn->setBtnState(AeroButton::ON);
	else
		avgBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		avgBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(avgBtnClicked()));

	gridBtn = new AeroButton("Pan Grid", this);
	gridBtn->setRoundness(10);
	gridBtn->setFont(m_fonts.smallFont);
	
	if (m_panGrid)
		gridBtn->setBtnState(AeroButton::ON);
	else
		gridBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		gridBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(gridBtnClicked()));

	peakHoldBtn = new AeroButton("Peak Hold", this);
	peakHoldBtn->setRoundness(10);
	peakHoldBtn->setFont(m_fonts.smallFont);
	peakHoldBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		peakHoldBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(peakHoldBtnClicked()));

	lockPanBtn = new AeroButton("Lock Pan", this);
	lockPanBtn->setRoundness(10);
	lockPanBtn->setFont(m_fonts.smallFont);
	
	if (m_panLocked)
		lockPanBtn->setBtnState(AeroButton::ON);
	else
		lockPanBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		lockPanBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(panLockedBtnClicked()));

	clickVfoBtn = new AeroButton("Click VFO", this);
	clickVfoBtn->setRoundness(10);
	clickVfoBtn->setFont(m_fonts.smallFont);
	
	if (m_clickVFO)
		clickVfoBtn->setBtnState(AeroButton::ON);
	else
		clickVfoBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		clickVfoBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(clickVfoBtnClicked()));

	showCrossBtn = new AeroButton("Hair Cross", this);
	showCrossBtn->setRoundness(10);
	showCrossBtn->setFont(m_fonts.smallFont);
	
	if (m_showCross)
		showCrossBtn->setBtnState(AeroButton::ON);
	else
		showCrossBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		showCrossBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(hairCrossBtnClicked()));

	midToVfoBtn = new AeroButton("Mid = VFO", this);
	midToVfoBtn->setRoundness(10);
    midToVfoBtn->setFont(m_fonts.smallFont);
	
	CHECKED_CONNECT(
		midToVfoBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(midToVfoBtnClicked()));

	vfoToMidBtn = new AeroButton("VFO = Mid", this);
	vfoToMidBtn->setRoundness(10);
    vfoToMidBtn->setFont(m_fonts.smallFont);
	
	CHECKED_CONNECT(
		vfoToMidBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(vfoToMidBtnClicked()));

	m_PanLineBtn = new AeroButton("Line", this);
	m_PanLineBtn->setRoundness(10);
    m_PanLineBtn->setFont(m_fonts.smallFont);
	panadapterBtnList.append(m_PanLineBtn);

	CHECKED_CONNECT(
		m_PanLineBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(panModeChanged()));

	m_PanFilledLineBtn = new AeroButton("Filled Line", this);
	m_PanFilledLineBtn->setRoundness(10);
    m_PanFilledLineBtn->setFont(m_fonts.smallFont);
	panadapterBtnList.append(m_PanFilledLineBtn);

	CHECKED_CONNECT(
		m_PanFilledLineBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(panModeChanged()));

	m_PanSolidBtn = new AeroButton("Solid", this);
	m_PanSolidBtn->setRoundness(10);
    m_PanSolidBtn->setFont(m_fonts.smallFont);
	panadapterBtnList.append(m_PanSolidBtn);

	CHECKED_CONNECT(
		m_PanSolidBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(panModeChanged()));

	switch (m_panadapterMode) {

		case (PanGraphicsMode) Line:

			m_PanLineBtn->setBtnState(AeroButton::ON);
			m_PanFilledLineBtn->setBtnState(AeroButton::OFF);
			m_PanSolidBtn->setBtnState(AeroButton::OFF);
			break;

		case (PanGraphicsMode) FilledLine:

			m_PanFilledLineBtn->setBtnState(AeroButton::ON);
			m_PanLineBtn->setBtnState(AeroButton::OFF);
			m_PanSolidBtn->setBtnState(AeroButton::OFF);
			break;

		case (PanGraphicsMode) Solid:

			m_PanSolidBtn->setBtnState(AeroButton::ON);
			m_PanLineBtn->setBtnState(AeroButton::OFF);
			m_PanFilledLineBtn->setBtnState(AeroButton::OFF);
			break;
	}


	m_WaterfallSimpleBtn = new AeroButton("Simple", this);
	m_WaterfallSimpleBtn->setRoundness(10);
    m_WaterfallSimpleBtn->setFont(m_fonts.smallFont);
	waterfallBtnList.append(m_WaterfallSimpleBtn);

	CHECKED_CONNECT(
		m_WaterfallSimpleBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(waterfallModeChanged()));

	m_WaterfallEnhancedBtn = new AeroButton("Enhanced", this);
	m_WaterfallEnhancedBtn->setRoundness(10);
    m_WaterfallEnhancedBtn->setFont(m_fonts.smallFont);
	waterfallBtnList.append(m_WaterfallEnhancedBtn);

	CHECKED_CONNECT(
		m_WaterfallEnhancedBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(waterfallModeChanged()));

	switch (m_waterfallColorMode) {

		case (WaterfallColorMode) Simple:

			m_WaterfallSimpleBtn->setBtnState(AeroButton::ON);
			m_WaterfallEnhancedBtn->setBtnState(AeroButton::OFF);
			break;

		case (WaterfallColorMode) Enhanced:

			m_WaterfallSimpleBtn->setBtnState(AeroButton::OFF);
			m_WaterfallEnhancedBtn->setBtnState(AeroButton::ON);
			break;
	}

	/*QHBoxLayout* hbox0 = new QHBoxLayout();
	hbox0->setContentsMargins(0, 0, 0, 0);
	hbox0->setSpacing(0);
	hbox0->addWidget(m_optionsLabel);
	hbox0->addStretch();*/
	
	QHBoxLayout* hbox1 = new QHBoxLayout();
	hbox1->setContentsMargins(0, 0, 0, 0);
	hbox1->setSpacing(0);
	hbox1->addWidget(avgBtn);
	hbox1->addWidget(gridBtn);
	hbox1->addWidget(peakHoldBtn);
	
	QHBoxLayout* hbox2 = new QHBoxLayout();
	hbox2->setContentsMargins(0, 0, 0, 0);
	hbox2->setSpacing(0);
	hbox2->addWidget(lockPanBtn);
	hbox2->addWidget(clickVfoBtn);
	hbox2->addWidget(showCrossBtn);
	
	QHBoxLayout* hbox3 = new QHBoxLayout();
	hbox3->setContentsMargins(0, 0, 0, 0);
	hbox3->setSpacing(0);
	hbox3->addWidget(midToVfoBtn);
	hbox3->addWidget(vfoToMidBtn);
	
	QHBoxLayout* hbox4 = new QHBoxLayout();
	hbox4->setContentsMargins(0, 0, 0, 0);
	hbox4->setSpacing(0);
	hbox4->addWidget(m_PanLineBtn);
	hbox4->addWidget(m_PanFilledLineBtn);
	hbox4->addWidget(m_PanSolidBtn);
	
	QHBoxLayout* hbox5 = new QHBoxLayout();
	hbox5->setContentsMargins(0, 0, 0, 0);
	hbox5->setSpacing(0);
	hbox5->addWidget(m_WaterfallSimpleBtn);
	hbox5->addWidget(m_WaterfallEnhancedBtn);
	
	optionsVBox = new QVBoxLayout;
	optionsVBox->setSpacing(1);
	//optionsVBox->addLayout(hbox0);
	optionsVBox->addLayout(hbox1);
	optionsVBox->addLayout(hbox2);
	optionsVBox->addLayout(hbox3);
	optionsVBox->addSpacing(4);
	optionsVBox->addLayout(hbox4);
	optionsVBox->addLayout(hbox5);
}

void RadioPopupWidget::createBandBtnGroup() {

	band160mBtn = new AeroButton("160 m", this);
	bandBtnList.append(band160mBtn);
	CHECKED_CONNECT(band160mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band80mBtn = new AeroButton("80 m", this);
	bandBtnList.append(band80mBtn);
	CHECKED_CONNECT(band80mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band60mBtn = new AeroButton("60 m", this);
	bandBtnList.append(band60mBtn);
	CHECKED_CONNECT(band60mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band40mBtn = new AeroButton("40 m", this);
	bandBtnList.append(band40mBtn);
	CHECKED_CONNECT(band40mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band30mBtn = new AeroButton("30 m", this);
	bandBtnList.append(band30mBtn);
	CHECKED_CONNECT(band30mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band20mBtn = new AeroButton("20 m", this);
	bandBtnList.append(band20mBtn);
	CHECKED_CONNECT(band20mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band17mBtn = new AeroButton("17 m", this);
	bandBtnList.append(band17mBtn);
	CHECKED_CONNECT(band17mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band15mBtn = new AeroButton("15 m", this);
	bandBtnList.append(band15mBtn);
	CHECKED_CONNECT(band15mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band12mBtn = new AeroButton("12 m", this);
	bandBtnList.append(band12mBtn);
	CHECKED_CONNECT(band12mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band10mBtn = new AeroButton("10 m", this);
	bandBtnList.append(band10mBtn);
	CHECKED_CONNECT(band10mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	band6mBtn = new AeroButton("6 m", this);
	bandBtnList.append(band6mBtn);
	CHECKED_CONNECT(band6mBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));

	bandGenBtn = new AeroButton("Gen", this);
	bandBtnList.append(bandGenBtn);
	CHECKED_CONNECT(bandGenBtn, SIGNAL(clicked()), this, SLOT(bandChangedByBtn()));
	
	foreach (AeroButton *btn, bandBtnList) {

		btn->setRoundness(0);
		btn->setFixedHeight(btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());
		btn->update();
	}

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
	dspModeBtnList.append(lsbBtn);
	CHECKED_CONNECT(lsbBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	usbBtn = new AeroButton("USB", this);
	dspModeBtnList.append(usbBtn);
	CHECKED_CONNECT(usbBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	dsbBtn = new AeroButton("DSB", this);
	dspModeBtnList.append(dsbBtn);
	CHECKED_CONNECT(dsbBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	cwlBtn = new AeroButton("CWL", this);
	dspModeBtnList.append(cwlBtn);
	CHECKED_CONNECT(cwlBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	cwuBtn = new AeroButton("CWU", this);
	dspModeBtnList.append(cwuBtn);
	CHECKED_CONNECT(cwuBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	fmnBtn = new AeroButton("FMN", this);
	dspModeBtnList.append(fmnBtn);
	CHECKED_CONNECT(fmnBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	amBtn = new AeroButton("AM", this);
	dspModeBtnList.append(amBtn);
	CHECKED_CONNECT(amBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	diguBtn = new AeroButton("DIGU", this);
	dspModeBtnList.append(diguBtn);
	CHECKED_CONNECT(diguBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	diglBtn = new AeroButton("DIGL", this);
	dspModeBtnList.append(diglBtn);
	CHECKED_CONNECT(diglBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	specBtn = new AeroButton("SPEC", this);
	dspModeBtnList.append(specBtn);
	CHECKED_CONNECT(specBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	samBtn = new AeroButton("SAM", this);
	dspModeBtnList.append(samBtn);
	CHECKED_CONNECT(samBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	drmBtn = new AeroButton("DRM", this);
	dspModeBtnList.append(drmBtn);
	CHECKED_CONNECT(drmBtn, SIGNAL(clicked()), this, SLOT(dspModeChangedByBtn()));

	foreach (AeroButton *btn, dspModeBtnList) {

		btn->setRoundness(0);
		btn->setFixedHeight(btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());
		btn->update();
	}

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
	
	agcUSER = new AeroButton("User", this);
	agcUSER->setRoundness(0);
	agcUSER->setFixedSize(btn_widths, btn_height);
	agcUSER->setStyleSheet(set->getMiniButtonStyle());
	agcModeBtnList.append(agcUSER);
	CHECKED_CONNECT(agcUSER, SIGNAL(clicked()), this, SLOT(agcModeChangedByBtn()));

	showAGCLines = new AeroButton("Show Lines", this);
	showAGCLines->setRoundness(0);
	showAGCLines->setFixedSize(btn_widthb, btn_height);
	showAGCLines->setStyleSheet(set->getMiniButtonStyle());
	CHECKED_CONNECT(showAGCLines, SIGNAL(clicked()), this, SLOT(agcShowLinesChanged()));

	if (m_receiverDataList.at(m_receiver).agcLines)
		showAGCLines->setBtnState(AeroButton::ON);
	else
		showAGCLines->setBtnState(AeroButton::OFF);

	QHBoxLayout* hbox1 = new QHBoxLayout();
	hbox1->setContentsMargins(0, 0, 0, 0);
	hbox1->setSpacing(0);
	hbox1->addWidget(agcOFF);
	hbox1->addWidget(agcLONG);
	hbox1->addWidget(agcSLOW);
	hbox1->addWidget(agcMED);
	hbox1->addWidget(agcFAST);
	hbox1->addWidget(agcUSER);

	QHBoxLayout* hbox2 = new QHBoxLayout();
	hbox2->setContentsMargins(0, 0, 0, 0);
	hbox2->setSpacing(0);
	hbox2->addWidget(showAGCLines);
	hbox2->addStretch();

	agcVBox = new QVBoxLayout;
	agcVBox->setSpacing(1);
	agcVBox->addLayout(hbox1);
	agcVBox->addLayout(hbox2);
}

void RadioPopupWidget::createFilterBtnWidgetA() {

	filter1kBtnA = new AeroButton("1k", this);
	filter1kBtnA->setObjectName("1k");
	filterBtnListA.append(filter1kBtnA);
	CHECKED_CONNECT(filter1kBtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter1k8BtnA = new AeroButton("1k8", this);
	filter1k8BtnA->setObjectName("1k8");
	filterBtnListA.append(filter1k8BtnA);
	CHECKED_CONNECT(filter1k8BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter2k1BtnA = new AeroButton("2k1", this);
	filter2k1BtnA->setObjectName("2k1");
	filterBtnListA.append(filter2k1BtnA);
	CHECKED_CONNECT(filter2k1BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter2k4BtnA = new AeroButton("2k4", this);
	filter2k4BtnA->setObjectName("2k4");
	filterBtnListA.append(filter2k4BtnA);
	CHECKED_CONNECT(filter2k4BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter2k7BtnA = new AeroButton("2k7", this);
	filter2k7BtnA->setObjectName("2k7");
	filterBtnListA.append(filter2k7BtnA);
	CHECKED_CONNECT(filter2k7BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter2k9BtnA = new AeroButton("2k9", this);
	filter2k9BtnA->setObjectName("2k9");
	filterBtnListA.append(filter2k9BtnA);
	CHECKED_CONNECT(filter2k9BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter3k3BtnA = new AeroButton("3k3", this);
	filter3k3BtnA->setObjectName("3k3");
	filterBtnListA.append(filter3k3BtnA);
	CHECKED_CONNECT(filter3k3BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter3k8BtnA = new AeroButton("3k8", this);
	filter3k8BtnA->setObjectName("3k8");
	filterBtnListA.append(filter3k8BtnA);
	CHECKED_CONNECT(filter3k8BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter4k4BtnA = new AeroButton("4k4", this);
	filter4k4BtnA->setObjectName("4k4");
	filterBtnListA.append(filter4k4BtnA);
	CHECKED_CONNECT(filter4k4BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter5kBtnA = new AeroButton("5k", this);
	filter5kBtnA->setObjectName("5k");
	filterBtnListA.append(filter5kBtnA);
	CHECKED_CONNECT(filter5kBtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filterVar1BtnA = new AeroButton("Var1", this);
	filterVar1BtnA->setObjectName("Var1");
	filterBtnListA.append(filterVar1BtnA);
	CHECKED_CONNECT(filterVar1BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filterVar2BtnA = new AeroButton("Var2", this);
	filterVar2BtnA->setObjectName("Var2");
	filterBtnListA.append(filterVar2BtnA);
	CHECKED_CONNECT(filterVar2BtnA, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	foreach(AeroButton *btn, filterBtnListA) {

		btn->setRoundness(0);
		btn->setFixedHeight(btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());
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
	filterBtnListB.append(filter16kBtnB);
	CHECKED_CONNECT(filter16kBtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter12kBtnB = new AeroButton("12k", this);
	filterBtnListB.append(filter12kBtnB);
	CHECKED_CONNECT(filter12kBtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter10kBtnB = new AeroButton("10k", this);
	filterBtnListB.append(filter10kBtnB);
	CHECKED_CONNECT(filter10kBtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter8kBtnB = new AeroButton("8k", this);
	filterBtnListB.append(filter8kBtnB);
	CHECKED_CONNECT(filter8kBtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter6k6BtnB = new AeroButton("6k6", this);
	filterBtnListB.append(filter6k6BtnB);
	CHECKED_CONNECT(filter6k6BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter5k2BtnB = new AeroButton("5k2", this);
	filterBtnListB.append(filter5k2BtnB);
	CHECKED_CONNECT(filter5k2BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter4kBtnB = new AeroButton("4k", this);
	filterBtnListB.append(filter4kBtnB);
	CHECKED_CONNECT(filter4kBtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter3k1BtnB = new AeroButton("3k1", this);
	filterBtnListB.append(filter3k1BtnB);
	CHECKED_CONNECT(filter3k1BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter2k9BtnB = new AeroButton("2k9", this);
	filterBtnListB.append(filter2k9BtnB);
	CHECKED_CONNECT(filter2k9BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter2k4BtnB = new AeroButton("2k4", this);
	filterBtnListB.append(filter2k4BtnB);
	CHECKED_CONNECT(filter2k4BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filterVar1BtnB = new AeroButton("Var1", this);
	filterBtnListB.append(filterVar1BtnB);
	CHECKED_CONNECT(filterVar1BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filterVar2BtnB = new AeroButton("Var2", this);
	filterBtnListB.append(filterVar2BtnB);
	CHECKED_CONNECT(filterVar2BtnB, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	foreach(AeroButton *btn, filterBtnListB) {

		btn->setRoundness(0);
		btn->setFixedHeight(btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());
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
	filterBtnListC.append(filter1kBtnC);
	CHECKED_CONNECT(filter1kBtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter800BtnC = new AeroButton("800", this);
	filterBtnListC.append(filter800BtnC);
	CHECKED_CONNECT(filter800BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter750BtnC = new AeroButton("750", this);
	filterBtnListC.append(filter750BtnC);
	CHECKED_CONNECT(filter750BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter600BtnC = new AeroButton("600", this);
	filterBtnListC.append(filter600BtnC);
	CHECKED_CONNECT(filter600BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter500BtnC = new AeroButton("500", this);
	filterBtnListC.append(filter500BtnC);
	CHECKED_CONNECT(filter500BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter400BtnC = new AeroButton("400", this);
	filterBtnListC.append(filter400BtnC);
	CHECKED_CONNECT(filter400BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter250BtnC = new AeroButton("250", this);
	filterBtnListC.append(filter250BtnC);
	CHECKED_CONNECT(filter250BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter100BtnC = new AeroButton("100", this);
	filterBtnListC.append(filter100BtnC);
	CHECKED_CONNECT(filter100BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter50BtnC = new AeroButton("50", this);
	filterBtnListC.append(filter50BtnC);
	CHECKED_CONNECT(filter50BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filter25BtnC = new AeroButton("25", this);
	filterBtnListC.append(filter25BtnC);
	CHECKED_CONNECT(filter25BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filterVar1BtnC = new AeroButton("Var1", this);
	filterBtnListC.append(filterVar1BtnC);
	CHECKED_CONNECT(filterVar1BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	filterVar2BtnC = new AeroButton("Var2", this);
	filterBtnListC.append(filterVar2BtnC);
	CHECKED_CONNECT(filterVar2BtnC, SIGNAL(clicked()), this, SLOT(filterChangedByBtn()));

	foreach(AeroButton *btn, filterBtnListC) {

		btn->setRoundness(0);
		btn->setFixedHeight(btn_height);
		btn->setStyleSheet(set->getMiniButtonStyle());
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


void RadioPopupWidget::ctrFrequencyChanged(QObject* sender, int mode, int rx, long frequency) {

	Q_UNUSED (sender)
	Q_UNUSED (mode)

	if (m_receiver != rx) return;
	m_ctrFrequency = frequency;

	HamBand band = getBandFromFrequency(set->getBandFrequencyList(), frequency);
	m_lastCtrFrequencyList[(int) band] = m_ctrFrequency;
}

void RadioPopupWidget::vfoFrequencyChanged(QObject* sender, int mode, int rx, long frequency) {

	Q_UNUSED (sender)
	Q_UNUSED (mode)

	if (m_receiver != rx) return;
	m_vfoFrequency = frequency;

	HamBand band = getBandFromFrequency(set->getBandFrequencyList(), frequency);
	m_lastVfoFrequencyList[(int) band] = m_vfoFrequency;
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

	set->setHamBand(this, m_receiver, true, (HamBand) btn);

	QString str = button->text();
	if (str == "160 m")
		set->setVFOFrequency(this, 2, m_receiver, m_lastVfoFrequencyList.at(0));
	else
	if (str == "80 m")
		set->setVFOFrequency(this, 2, m_receiver, m_lastVfoFrequencyList.at(1));
	else
	if (str == "60 m")
		set->setVFOFrequency(this, 2, m_receiver, m_lastVfoFrequencyList.at(2));
	else
	if (str == "40 m")
		set->setVFOFrequency(this, 2, m_receiver, m_lastVfoFrequencyList.at(3));
	else
	if (str == "30 m")
		set->setVFOFrequency(this, 2, m_receiver, m_lastVfoFrequencyList.at(4));
	else
	if (str == "20 m")
		set->setVFOFrequency(this, 2, m_receiver, m_lastVfoFrequencyList.at(5));
	else
	if (str == "17 m")
		set->setVFOFrequency(this, 2, m_receiver, m_lastVfoFrequencyList.at(6));
	else
	if (str == "15 m")
		set->setVFOFrequency(this, 2, m_receiver, m_lastVfoFrequencyList.at(7));
	else
	if (str == "12 m")
		set->setVFOFrequency(this, 2, m_receiver, m_lastVfoFrequencyList.at(8));
	else
	if (str == "10 m")
		set->setVFOFrequency(this, 2, m_receiver, m_lastVfoFrequencyList.at(9));
	else
	if (str == "6 m")
		set->setVFOFrequency(this, 2, m_receiver, m_lastVfoFrequencyList.at(10));
	else
	if (str == "Gen")
		set->setVFOFrequency(this, 2, m_receiver, m_lastVfoFrequencyList.at(11));
}

void RadioPopupWidget::bandChanged(QObject *sender, int rx, bool byButton, HamBand band) {

	Q_UNUSED (byButton)

	if (sender == this) return;

	if (m_receiver != rx) return;
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

	set->setDSPMode(this, m_receiver, (DSPMode) btn);
	m_dspModeList[m_hamBand] = (DSPMode) btn;
	filterChanged(this, m_receiver, m_filterLo, m_filterHi);
	filterGroupChanged((DSPMode) btn);

	button->setBtnState(AeroButton::ON);
	button->update();
}

void RadioPopupWidget::dspModeChanged(QObject *sender, int rx, DSPMode mode) {

	Q_UNUSED(sender)
	
	if (m_receiver != rx) return;
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

	set->setRXFilter(this, m_receiver, m_filterLo, m_filterHi);	
}

void RadioPopupWidget::filterChanged(QObject *sender, int rx, qreal low, qreal high) {

	Q_UNUSED(sender)
	
	if (m_receiver != rx) return;
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

	set->setAGCMode(this, m_receiver, (AGCMode) btn);
	m_agcMode = (AGCMode) btn;
	
	button->setBtnState(AeroButton::ON);
	button->update();
}

void RadioPopupWidget::agcModeChanged(QObject *sender, int rx, AGCMode mode, bool hang) {

	Q_UNUSED(sender)
	Q_UNUSED(hang)
	
	if (m_receiver != rx) return;
	m_agcMode = mode;

	foreach(AeroButton *btn, agcModeBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	agcModeBtnList.at(mode)->setBtnState(AeroButton::ON);
	agcModeBtnList.at(mode)->update();
}

void RadioPopupWidget::agcShowLinesChanged() {

	if (showAGCLines->btnState() == AeroButton::OFF) {

		showAGCLines->setBtnState(AeroButton::ON);
		set->setAGCShowLines(this, m_receiver, true);
	}
	else {

		showAGCLines->setBtnState(AeroButton::OFF);
		set->setAGCShowLines(this, m_receiver, false);
	}
}

void RadioPopupWidget::avgBtnClicked() {

	if (avgBtn->btnState() == AeroButton::OFF) {

		avgBtn->setBtnState(AeroButton::ON);
		m_spectrumAveraging = true;
		set->setSpectrumAveraging(this, m_receiver, true);
	}
	else {

		avgBtn->setBtnState(AeroButton::OFF);
		m_spectrumAveraging = false;
		set->setSpectrumAveraging(this, m_receiver, false);
	}
}

void RadioPopupWidget::gridBtnClicked() {

	if (gridBtn->btnState() == AeroButton::OFF) {

		gridBtn->setBtnState(AeroButton::ON);
		m_panGrid = true;
		set->setPanGrid(true, m_receiver);
	}
	else {

		gridBtn->setBtnState(AeroButton::OFF);
		m_panGrid = false;
		set->setPanGrid(false, m_receiver);
	}
}

void RadioPopupWidget::peakHoldBtnClicked() {

	if (peakHoldBtn->btnState() == AeroButton::OFF) {

		peakHoldBtn->setBtnState(AeroButton::ON);
		m_peakHold = true;
		set->setPeakHold(true, m_receiver);
	}
	else {

		peakHoldBtn->setBtnState(AeroButton::OFF);
		m_peakHold = false;
		set->setPeakHold(false, m_receiver);
	}
}

void RadioPopupWidget::panLockedBtnClicked() {

	if (lockPanBtn->btnState() == AeroButton::OFF) {

		lockPanBtn->setBtnState(AeroButton::ON);
		m_panLocked = true;
		set->setPanLocked(true, m_receiver);
	}
	else {

		lockPanBtn->setBtnState(AeroButton::OFF);
		m_panLocked = false;
		set->setPanLocked(false, m_receiver);
	}
}

void RadioPopupWidget::clickVfoBtnClicked() {

	if (clickVfoBtn->btnState() == AeroButton::OFF) {

		clickVfoBtn->setBtnState(AeroButton::ON);
		m_clickVFO = true;
		set->setClickVFO(true, m_receiver);
	}
	else {

		clickVfoBtn->setBtnState(AeroButton::OFF);
		m_clickVFO = false;
		set->setClickVFO(false, m_receiver);
	}
}

void RadioPopupWidget::hairCrossBtnClicked() {

	if (showCrossBtn->btnState() == AeroButton::OFF) {

		showCrossBtn->setBtnState(AeroButton::ON);
		m_showCross = true;
		set->setHairCross(true, m_receiver);
	}
	else {

		showCrossBtn->setBtnState(AeroButton::OFF);
		m_showCross = false;
		set->setHairCross(false, m_receiver);
	}
}

void RadioPopupWidget::midToVfoBtnClicked() {

	emit midToVfoBtnEvent();
}

void RadioPopupWidget::vfoToMidBtnClicked() {

	emit vfoToMidBtnEvent();
}

void RadioPopupWidget::setCurrentReceiver(QObject *sender, int value) {

	Q_UNUSED(sender)

	if (m_receiver == value) return;
	m_receiver = value;

	TReceiver rxData = set->getReceiverDataList().at(m_receiver);

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
		filterChanged(this, m_receiver, m_filterLo, m_filterHi);
	}

	if (m_agcMode != rxData.agcMode) {
		m_agcMode = rxData.agcMode;
	}

	if (m_filterLo != rxData.filterLo || m_filterHi != rxData.filterHi) {

		m_filterLo = rxData.filterLo;
		m_filterHi = rxData.filterHi;
		filterChanged(this, m_receiver, m_filterLo, m_filterHi);
	}
}

void RadioPopupWidget::setSticky() {

	if (stickyBtn->btnState() == AeroButton::OFF) {

		stickyBtn->setBtnState(AeroButton::ON);
		stickyBtn->setText("Unlock");
		m_sticky = true;
	}
	else {

		stickyBtn->setBtnState(AeroButton::OFF);
		stickyBtn->setText("Lock");
		m_sticky = false;
	}
}

void RadioPopupWidget::panModeChanged() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = panadapterBtnList.indexOf(button);

	foreach(AeroButton *btn, panadapterBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			m_panadapterMode = (PanGraphicsMode) Line;
			break;
			
		case 1:
			m_panadapterMode = (PanGraphicsMode) FilledLine;
			break;

		case 2:
			m_panadapterMode = (PanGraphicsMode) Solid;
			break;
	}

	set->setGraphicsState(this, m_receiver, m_panadapterMode, m_waterfallColorMode);
}

void RadioPopupWidget::waterfallModeChanged() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = waterfallBtnList.indexOf(button);

	foreach(AeroButton *btn, waterfallBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			m_waterfallColorMode = (WaterfallColorMode) Simple;
			break;
			
		case 1:
			m_waterfallColorMode = (WaterfallColorMode) Enhanced;
			break;
	}

	set->setGraphicsState(this, m_receiver, m_panadapterMode, m_waterfallColorMode);
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
	Q_UNUSED(mode)
	Q_UNUSED(state)

}

void RadioPopupWidget::graphicModeChanged(
	QObject *sender,
	int rx,
	PanGraphicsMode panMode,
	WaterfallColorMode waterfallMode)
{
	Q_UNUSED (sender)
	Q_UNUSED (rx)

	bool change = false;

	if (m_panadapterMode != panMode) {
		
		m_panadapterMode = panMode;
		change = true;
	}

	if (m_waterfallColorMode != waterfallMode) {
		
		m_waterfallColorMode = waterfallMode;
		change = true;
	}

	if (!change) return;

	update();
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

	createBackground(event->size());
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

	if (!m_sticky) {
		
		m_closingTimer.restart();
		m_mouseOver = false;
	}
	QWidget::leaveEvent(event);
}

void RadioPopupWidget::timerEvent(QTimerEvent *event) {

	int timerId = event->timerId();

	if (m_timerID > 0 && timerId == m_timerID) {

		if (!m_mouseOver && isVisible()) {

			if (m_closingTimer.elapsed() >= 100) close();
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

void RadioPopupWidget::createBackground(QSize size) {

	if (size.width() <= 0 || size.height() <= 0) return;

	QFont font(this->font());
	font.setPixelSize(12);

	QFontMetrics fm(font);
	int font_height = fm.height();

	QImage image = QImage(size, QImage::Format_ARGB32_Premultiplied);
	if (image.isNull()) return;

	image.fill(QColor(0, 0, 0, 0).rgba());

	QPainter painter(&image);
	painter.setRenderHints(QPainter::SmoothPixmapTransform | QPainter::Antialiasing | QPainter::TextAntialiasing, true);
	painter.setFont(font);

	painter.fillRect(image.rect(), QColor(35, 35, 35));
	//painter.fillRect(image.rect(), QColor(0, 60, 129));
	
	QString titleStr = windowTitle();
	QRect title_bar_rect(0, 0, image.width(), font_height + 6);

	QLinearGradient title_bar_gradient(0, 0, 0, 1);
	title_bar_gradient.setCoordinateMode(QGradient::ObjectBoundingMode);
	title_bar_gradient.setSpread(QGradient::PadSpread);
	title_bar_gradient.setColorAt(0.0, QColor(100, 110, 128));
	title_bar_gradient.setColorAt(0.4, QColor(74, 80, 90));
	title_bar_gradient.setColorAt(0.6, QColor(56, 62, 70));
	title_bar_gradient.setColorAt(1.0, QColor(48, 56, 64));
	painter.setPen(Qt::NoPen);
	painter.setBrush(QBrush(title_bar_gradient));

	title_bar_rect.adjust(1, 1, 1, 1);
	painter.setPen(Qt::black);
	painter.drawText(title_bar_rect, Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, titleStr);
	title_bar_rect.adjust(-1, -1, -1, -1);
	painter.setPen(Qt::white);
	painter.drawText(title_bar_rect, Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, titleStr);
	
	painter.setPen(QColor(255, 255, 255, 64));
	//painter.setPen(QColor(0, 255, 255, 255));
	painter.drawLine(0, 0, image.width() - 1, 0);											// top line
	painter.drawLine(0, 0, 0, image.height() - 1);											// left line
	painter.setPen(QColor(0, 0, 0, 64));
	painter.drawLine(0, image.height() - 1, image.width() - 1, image.height() - 1);			// bottom line
	painter.drawLine(image.width() - 1, 0, image.width() - 1, image.height() - 1);			// right line
	
	painter.end();

	QPalette palette;
	palette.setBrush(backgroundRole(), QBrush(image));
	setPalette(palette);
	setAutoFillBackground(true);
}
