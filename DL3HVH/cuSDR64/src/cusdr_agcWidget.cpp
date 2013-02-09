/**
* @file  cusdr_agcWidget.h
* @brief AGC options widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-09-29
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

//#define LOG_GRAPHIC_OPTIONS_WIDGET

//#include <QtGui>
//#include <QPen>
//#include <QDebug>
//#include <QDialog>
//#include <QColorDialog>

#include "cusdr_agcWidget.h"

#define	btn_height		15
#define	btn_width		70
#define	btn_widths		32
#define	btn_width2		52
#define	btn_width3		60

AGCOptionsWidget::AGCOptionsWidget(QWidget *parent)
	: QWidget(parent)
	, set(Settings::instance())
	, m_serverMode(set->getCurrentServerMode())
	, m_hwInterface(set->getHWInterface())
	, m_dataEngineState(set->getDataEngineState())
	//, m_panadapterMode(set->getPanadapterMode())
	//, m_waterColorScheme(set->getWaterfallColorScheme())
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(set->getMinimumGroupBoxWidth())
	, m_btnSpacing(5)
	, m_currentReceiver(set->getCurrentReceiver())
	, m_sampleRate(set->getSampleRate())
	, m_mouseOver(false)
{
	//setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 0, 4, 0);
	setMouseTracking(true);

	m_rxDataList = set->getReceiverDataList();
	
	m_agcMode = m_rxDataList.at(0).agcMode;

	fonts = new CFonts(this);
	m_fonts = fonts->getFonts();

	createAgcModeBtnGroup();
	createAgcOptionsGroup();

	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(0);
	hbox1->setMargin(0);
	hbox1->addStretch();
	hbox1->addWidget(agcModeGroupBox);

	QHBoxLayout *hbox2 = new QHBoxLayout;
	hbox2->setSpacing(0);
	hbox2->setMargin(0);
	hbox2->addStretch();
	hbox2->addWidget(agcOptionsGroupBox);

	mainLayout->addLayout(hbox1);
	mainLayout->addLayout(hbox2);
	mainLayout->addStretch();
	setLayout(mainLayout);

	agcModeChanged(this, 0, m_agcMode, false);

	setupConnections();
}

AGCOptionsWidget::~AGCOptionsWidget() {

	// disconnect all signals
	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

QSize AGCOptionsWidget::sizeHint() const {
	
	return QSize(m_minimumWidgetWidth, height());
}

QSize AGCOptionsWidget::minimumSizeHint() const {

	return QSize(m_minimumWidgetWidth, height());
}

void AGCOptionsWidget::setupConnections() {

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

//	CHECKED_CONNECT(
//		set,
//		SIGNAL(graphicModeChanged(
//					QObject *,
//					QSDRGraphics::_Panadapter,
//					QSDRGraphics::_WaterfallColorScheme)),
//		this,
//		SLOT(graphicModeChanged(
//					QObject *,
//					QSDRGraphics::_Panadapter,
//					QSDRGraphics::_WaterfallColorScheme)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(currentReceiverChanged(QObject *, int)),
		this, 
		SLOT(setCurrentReceiver(QObject *, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(sampleRateChanged(QObject *, int)), 
		this, 
		SLOT(sampleRateChanged(QObject *, int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(agcModeChanged(QObject *, int, AGCMode, bool)),
		this,
		SLOT(agcModeChanged(QObject *, int, AGCMode, bool)));

	CHECKED_CONNECT(
		set,
		SIGNAL(agcHangThresholdSliderChanged(QObject *, int, qreal)),
		this,
		SLOT(setAGCHangThresholdSlider(QObject *, int, qreal)));

	CHECKED_CONNECT(
		set,
		SIGNAL(agcMaximumGainChanged_dB(QObject *, int, qreal)),
		this,
		SLOT(setAGCMaximumGain_dB(QObject *, int, qreal)));

	CHECKED_CONNECT(
		set,
		SIGNAL(agcFixedGainChanged_dB(QObject *, int, qreal)),
		this,
		SLOT(setAGCFixedGain_dB(QObject *, int, qreal)));
}

void AGCOptionsWidget::createAgcModeBtnGroup() {

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

	QGridLayout *layout = new QGridLayout();
	layout->setVerticalSpacing(1);
	layout->setHorizontalSpacing(1);
	layout->setContentsMargins(0, 0, 0, 0);
	layout->addWidget(agcOFF,  0, 0);
	layout->addWidget(agcLONG, 0, 1);
	layout->addWidget(agcSLOW, 0, 2);
	layout->addWidget(agcMED,  0, 3);
	layout->addWidget(agcFAST, 0, 4);
	layout->addWidget(agcUSER, 0, 5);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addLayout(layout);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(1);
	vbox->addLayout(hbox1);

	agcModeGroupBox = new QGroupBox(tr("AGC Mode"), this);
	agcModeGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	agcModeGroupBox->setLayout(vbox);
	agcModeGroupBox->setStyleSheet(set->getWidgetStyle());
	//agcModeGroupBox->setMinimumWidth(175);
	agcModeGroupBox->setFont(QFont("Arial", 8));
}

void AGCOptionsWidget::createAgcOptionsGroup() {

	m_slopeSpinBox = new QSpinBox(this);
	m_slopeSpinBox->setMinimum(0);
	m_slopeSpinBox->setMaximum(20);
	m_slopeSpinBox->setSingleStep(1);
	m_slopeSpinBox->setStyleSheet(set->getSpinBoxStyle());
	m_slopeSpinBox->setMinimumWidth(60);
	m_slopeSpinBox->setValue((int) m_rxDataList.at(m_currentReceiver).agcVariableGain);

	CHECKED_CONNECT(
		m_slopeSpinBox,
		SIGNAL(valueChanged(int)),
		this,
		SLOT(slopeChanged(int)));

	m_slopeLabel = new QLabel("Slope (dB):", this);
	m_slopeLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_slopeLabel->setStyleSheet(set->getLabelStyle());


	m_maxGainSpinBox = new QSpinBox(this);
	m_maxGainSpinBox->setMinimum(-60);
	m_maxGainSpinBox->setMaximum(120);
	m_maxGainSpinBox->setSingleStep(1);
	m_maxGainSpinBox->setStyleSheet(set->getSpinBoxStyle());
	m_maxGainSpinBox->setMinimumWidth(60);
	m_maxGainSpinBox->setValue((int) m_rxDataList.at(m_currentReceiver).agcMaximumGain_dB);

	CHECKED_CONNECT(
		m_maxGainSpinBox,
		SIGNAL(valueChanged(int)),
		this,
		SLOT(maxGainChanged(int)));

	m_maxGainLabel = new QLabel("Max Gain (dB):", this);
	m_maxGainLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_maxGainLabel->setStyleSheet(set->getLabelStyle());


	m_attackTimeSpinBox = new QSpinBox(this);
	m_attackTimeSpinBox->setMinimum(1);
	m_attackTimeSpinBox->setMaximum(10);
	m_attackTimeSpinBox->setSingleStep(1);
	m_attackTimeSpinBox->setStyleSheet(set->getSpinBoxStyle());
	m_attackTimeSpinBox->setMinimumWidth(60);
	m_attackTimeSpinBox->setValue((int) m_rxDataList.at(m_currentReceiver).agcAttackTime * 1000);

	CHECKED_CONNECT(
		m_attackTimeSpinBox,
		SIGNAL(valueChanged(int)),
		this,
		SLOT(attackTimeChanged(int)));

	m_attackTimeLabel = new QLabel("Attack Time (ms):", this);
	m_attackTimeLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_attackTimeLabel->setStyleSheet(set->getLabelStyle());


	m_decayTimeSpinBox = new QSpinBox(this);
	m_decayTimeSpinBox->setMinimum(10);
	m_decayTimeSpinBox->setMaximum(5000);
	m_decayTimeSpinBox->setSingleStep(1);
	m_decayTimeSpinBox->setStyleSheet(set->getSpinBoxStyle());
	m_decayTimeSpinBox->setMinimumWidth(60);
	m_decayTimeSpinBox->setValue((int) (m_rxDataList.at(m_currentReceiver).agcDecayTime * 1000));

	CHECKED_CONNECT(
		m_decayTimeSpinBox,
		SIGNAL(valueChanged(int)),
		this,
		SLOT(decayTimeChanged(int)));

	m_decayTimeLabel = new QLabel("Decay Time (ms):", this);
	m_decayTimeLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_decayTimeLabel->setStyleSheet(set->getLabelStyle());


	m_hangTimeSpinBox = new QSpinBox(this);
	m_hangTimeSpinBox->setMinimum(10);
	m_hangTimeSpinBox->setMaximum(5000);
	m_hangTimeSpinBox->setSingleStep(1);
	m_hangTimeSpinBox->setStyleSheet(set->getSpinBoxStyle());
	m_hangTimeSpinBox->setMinimumWidth(60);
	m_hangTimeSpinBox->setValue((int) (m_rxDataList.at(m_currentReceiver).agcHangTime * 1000));

	CHECKED_CONNECT(
		m_hangTimeSpinBox,
		SIGNAL(valueChanged(int)),
		this,
		SLOT(hangTimeChanged(int)));

	m_hangTimeLabel = new QLabel("Hang Time (ms):", this);
	m_hangTimeLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_hangTimeLabel->setStyleSheet(set->getLabelStyle());

	m_hangThresholdSlider = new QSlider(Qt::Horizontal, this);
	m_hangThresholdSlider->setTickPosition(QSlider::NoTicks);
	m_hangThresholdSlider->setFixedSize(190, 12);
	m_hangThresholdSlider->setSingleStep(1);
	m_hangThresholdSlider->setRange(0, 100);
	m_hangThresholdSlider->setValue(0);
	m_hangThresholdSlider->setStyleSheet(set->getVolSliderStyle());

	CHECKED_CONNECT(
		m_hangThresholdSlider,
		SIGNAL(valueChanged(int)),
		this,
		SLOT(hangThresholdValueChanged(int)));

	int fontMaxWidth = m_fonts.smallFontMetrics->boundingRect(" 100 ").width();

	QString str = "%1 ";
	m_hangThresholdValueLabel = new QLabel(str.arg(0, 2, 10, QLatin1Char(' ')), this);
	m_hangThresholdValueLabel->setFont(m_fonts.smallFont);
	m_hangThresholdValueLabel->setFixedSize(fontMaxWidth, 12);
	m_hangThresholdValueLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_hangThresholdValueLabel->setStyleSheet(set->getSliderLabelStyle());

	m_hangThresholdLabel = new QLabel("Hang Threshold:", this);
	m_hangThresholdLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_hangThresholdLabel->setStyleSheet(set->getLabelStyle());


	m_fixedGainSpinBox = new QSpinBox(this);
	m_fixedGainSpinBox->setMinimum(-20);
	m_fixedGainSpinBox->setMaximum(120);
	m_fixedGainSpinBox->setSingleStep(1);
	m_fixedGainSpinBox->setStyleSheet(set->getSpinBoxStyle());
	m_fixedGainSpinBox->setMinimumWidth(60);
	m_fixedGainSpinBox->setValue((int) m_rxDataList.at(m_currentReceiver).agcFixedGain_dB);

	CHECKED_CONNECT(
		m_fixedGainSpinBox,
		SIGNAL(valueChanged(int)),
		this,
		SLOT(fixedGainChanged(int)));

	m_fixedGainLabel = new QLabel("Fixed Gain (dB):", this);
	m_fixedGainLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_fixedGainLabel->setStyleSheet(set->getLabelStyle());


	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addWidget(m_slopeLabel);
	hbox1->addStretch();
	hbox1->addWidget(m_slopeSpinBox);

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(4);
	hbox2->addWidget(m_maxGainLabel);
	hbox2->addStretch();
	hbox2->addWidget(m_maxGainSpinBox);

	QHBoxLayout *hbox3 = new QHBoxLayout();
	hbox3->setSpacing(4);
	hbox3->addWidget(m_attackTimeLabel);
	hbox3->addStretch();
	hbox3->addWidget(m_attackTimeSpinBox);

	QHBoxLayout *hbox4 = new QHBoxLayout();
	hbox4->setSpacing(4);
	hbox4->addWidget(m_decayTimeLabel);
	hbox4->addStretch();
	hbox4->addWidget(m_decayTimeSpinBox);

	QHBoxLayout *hbox5 = new QHBoxLayout();
	hbox5->setSpacing(4);
	hbox5->addWidget(m_hangTimeLabel);
	hbox5->addStretch();
	hbox5->addWidget(m_hangTimeSpinBox);

	QHBoxLayout *hbox6 = new QHBoxLayout();
	hbox6->setSpacing(4);
	hbox6->addWidget(m_fixedGainLabel);
	hbox6->addStretch();
	hbox6->addWidget(m_fixedGainSpinBox);

	QHBoxLayout *hbox7 = new QHBoxLayout();
	hbox7->setSpacing(4);
	hbox7->addWidget(m_hangThresholdLabel);
	hbox7->addStretch();
	//hbox7->addWidget(m_fixedGainSpinBox);

	QHBoxLayout *hbox8 = new QHBoxLayout;
	hbox8->setSpacing(0);
	hbox8->setMargin(0);
	hbox8->addStretch();
	hbox8->addWidget(m_hangThresholdSlider);
	hbox8->addWidget(m_hangThresholdValueLabel);
	hbox8->addStretch();


	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(1);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);
	vbox->addLayout(hbox3);
	vbox->addLayout(hbox4);
	vbox->addLayout(hbox5);
	vbox->addLayout(hbox6);
	vbox->addSpacing(20);
	vbox->addLayout(hbox7);
	vbox->addSpacing(4);
	vbox->addLayout(hbox8);

	agcOptionsGroupBox = new QGroupBox(tr("AGC Options"), this);
	agcOptionsGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	agcOptionsGroupBox->setLayout(vbox);
	agcOptionsGroupBox->setStyleSheet(set->getWidgetStyle());
	//agcModeGroupBox->setMinimumWidth(175);
	agcOptionsGroupBox->setFont(QFont("Arial", 8));
}

void AGCOptionsWidget::systemStateChanged(
	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	Q_UNUSED (sender)
	Q_UNUSED (err)

	if (m_hwInterface != hwmode)
		m_hwInterface = hwmode;
		

	if (m_serverMode != mode)
		m_serverMode = mode;

	if (m_dataEngineState != state)
		m_dataEngineState = state;

	update();
}

void AGCOptionsWidget::agcModeChangedByBtn() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btn = agcModeBtnList.indexOf(button);

	foreach(AeroButton *btn, agcModeBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	set->setAGCMode(this, m_currentReceiver, (AGCMode) btn);
	m_agcMode = (AGCMode) btn;

	button->setBtnState(AeroButton::ON);
	button->update();
}

void AGCOptionsWidget::agcModeChanged(QObject *sender, int rx, AGCMode mode, bool hang) {

	Q_UNUSED(sender)
	Q_UNUSED(hang)

	if (m_currentReceiver != rx) return;
	m_agcMode = mode;

	foreach(AeroButton *btn, agcModeBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	agcModeBtnList.at(mode)->setBtnState(AeroButton::ON);
	agcModeBtnList.at(mode)->update();

	if (mode == (AGCMode) agcUser) {

		m_attackTimeSpinBox->setEnabled(true);
		m_decayTimeSpinBox->setEnabled(true);
		m_hangTimeSpinBox->setEnabled(true);
	}
	else {

		m_attackTimeSpinBox->setEnabled(false);
		m_decayTimeSpinBox->setEnabled(false);
		m_hangTimeSpinBox->setEnabled(false);
	}
}

void AGCOptionsWidget::slopeChanged(int value) {

	set->setAGCVariableGain_dB(this, set->getCurrentReceiver(), (qreal)value);
}

void AGCOptionsWidget::maxGainChanged(int value) {

	int rx = set->getCurrentReceiver();
	set->setAGCMaximumGain_dB(this, rx, (qreal) value);
}

void AGCOptionsWidget::setAGCMaximumGain_dB(QObject *sender, int rx, qreal value) {

	Q_UNUSED(sender)
	Q_UNUSED(rx)

	m_maxGainSpinBox->blockSignals(true);
	m_maxGainSpinBox->setValue((int) value);
	m_maxGainSpinBox->blockSignals(false);
}

void AGCOptionsWidget::fixedGainChanged(int value) {

	int rx = set->getCurrentReceiver();
	set->setAGCFixedGain_dB(this, rx, (qreal) value);
}

void AGCOptionsWidget::setAGCFixedGain_dB(QObject *sender, int rx, qreal value) {

	Q_UNUSED(sender)
	Q_UNUSED(rx)

	m_fixedGainSpinBox->blockSignals(true);
	m_fixedGainSpinBox->setValue((int) value);
	m_fixedGainSpinBox->blockSignals(false);
}

void AGCOptionsWidget::attackTimeChanged(int value) {

	set->setAGCAttackTime(this, set->getCurrentReceiver(), value/1000.0);
}

void AGCOptionsWidget::decayTimeChanged(int value) {

	set->setAGCDecayTime(this, set->getCurrentReceiver(), value/1000.0);
}

void AGCOptionsWidget::hangTimeChanged(int value) {

	set->setAGCHangTime(this, set->getCurrentReceiver(), value/1000.0);
}

void AGCOptionsWidget::hangThresholdValueChanged(int value) {

	QString str = " %1 ";
	m_hangThresholdValueLabel->setText(str.arg(value, 2, 10, QLatin1Char(' ')));

	set->setAGCHangThreshold(this, set->getCurrentReceiver(), value);
}

void AGCOptionsWidget::setAGCHangThresholdSlider(QObject *sender, int rx, qreal value) {

	Q_UNUSED(sender)
	Q_UNUSED(rx)

	m_hangThresholdSlider->blockSignals(true);
	m_hangThresholdSlider->setValue((int) value);
	m_hangThresholdSlider->blockSignals(false);

	QString str = " %1 ";
	m_hangThresholdValueLabel->setText(str.arg((int)value, 2, 10, QLatin1Char(' ')));
}

void AGCOptionsWidget::sampleRateChanged(QObject *sender, int value) {

	Q_UNUSED(sender)

	m_sampleRate = value;
	//int currentValue = m_framesPerSecondSpinBox->value();

	//m_framesPerSecondSpinBox->setMaximum(m_sampleRate/1000);

	//if (currentValue > m_sampleRate/1000) {

	//	m_framesPerSecondSpinBox->setValue(m_sampleRate/1000);
	//	set->setFramesPerSecond(value);
	//}
}

void AGCOptionsWidget::setCurrentReceiver(QObject *sender, int rx) {

	Q_UNUSED(sender)

	if (m_currentReceiver == rx) return;
	m_currentReceiver = rx;

	if (m_agcMode != m_rxDataList.at(rx).agcMode) {
		m_agcMode = m_rxDataList.at(rx).agcMode;
	}
}
