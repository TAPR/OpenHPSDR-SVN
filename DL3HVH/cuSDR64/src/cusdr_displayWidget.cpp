/**
* @file  cusdr_displayWidget.cpp
* @brief OpenGL display options widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-08-19
*/

/*
 *   
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

#define LOG_DISPLAYOPTIONS_WIDGET

// use: DISPLAYOPTIONS_DEBUG

//#include <QtGui>
//#include <QPen>
//#include <QDebug>
//#include <QDialog>
//#include <QColorDialog>

#include "cusdr_displayWidget.h"

#define	btn_height		15
#define	btn_width		70
#define	btn_widths		38
#define	btn_width2		52
#define	btn_width3		60

DisplayOptionsWidget::DisplayOptionsWidget(QWidget *parent)
	: QWidget(parent)
	, set(Settings::instance())
	, m_serverMode(set->getCurrentServerMode())
	, m_hwInterface(set->getHWInterface())
	, m_dataEngineState(set->getDataEngineState())
	, m_panadapterMode(set->getPanadapterMode())
	, m_waterColorScheme(set->getWaterfallColorScheme())
	, m_antialiased(true)
	, m_mouseOver(false)
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(set->getMinimumGroupBoxWidth())
	, m_btnSpacing(5)
	, m_currentReceiver(set->getCurrentReceiver())
	, m_btnChooserHit(0)
	, m_framesPerSecond(set->getFramesPerSecond(m_currentReceiver))
	, m_graphicResolution(set->getGraphicResolution())
	, m_sampleRate(set->getSampleRate())
	, m_sMeterHoldTime(set->getSMeterHoldTime())
{
	//setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 0, 4, 0);
	setMouseTracking(true);

	m_rxDataList = set->getReceiverDataList();
	
	fonts = new CFonts(this);
	m_fonts = fonts->getFonts();

	createFPSGroupBox();
	createPanSpectrumOptions();
	createWaterfallSpectrumOptions();
	createSMeterOptions();
	createCallSignEditor();

	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(0);
	hbox1->setMargin(0);
	hbox1->addStretch();
	hbox1->addWidget(m_fpsGroupBox);

	QHBoxLayout *hbox2 = new QHBoxLayout;
	hbox2->setSpacing(0);
	hbox2->setMargin(0);
	hbox2->addStretch();
	hbox2->addWidget(m_panSpectrumOptions);

	QHBoxLayout *hbox3 = new QHBoxLayout;
	hbox3->setSpacing(0);
	hbox3->setMargin(0);
	hbox3->addStretch();
	hbox3->addWidget(m_waterfallSpectrumOptions);

	QHBoxLayout *hbox4 = new QHBoxLayout;
	hbox4->setSpacing(0);
	hbox4->setMargin(0);
	hbox4->addStretch();
	hbox4->addWidget(m_sMeterOptions);

	QHBoxLayout *hbox5 = new QHBoxLayout;
	hbox5->setSpacing(0);
	hbox5->setMargin(0);
	hbox5->addStretch();
	hbox5->addWidget(m_callSignEditor);
	hbox5->addStretch();

	mainLayout->addLayout(hbox1);
	mainLayout->addLayout(hbox2);
	mainLayout->addLayout(hbox3);
	mainLayout->addLayout(hbox4);
	mainLayout->addLayout(hbox5);
	mainLayout->addStretch();
	setLayout(mainLayout);

	setupConnections();
}

DisplayOptionsWidget::~DisplayOptionsWidget() {

	// disconnect all signals
	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

QSize DisplayOptionsWidget::sizeHint() const {
	
	return QSize(m_minimumWidgetWidth, height());
}

QSize DisplayOptionsWidget::minimumSizeHint() const {

	return QSize(m_minimumWidgetWidth, height());
}

void DisplayOptionsWidget::setupConnections() {

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
					QSDRGraphics::_Panadapter,
					QSDRGraphics::_WfScheme)),
		this, 
		SLOT(graphicModeChanged(
					QObject *,
					QSDRGraphics::_Panadapter,
					QSDRGraphics::_WfScheme)));

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
		SIGNAL(framesPerSecondChanged(QObject*, int, int)),
		this,
		SLOT(setFramesPerSecond(QObject*, int, int)));
}

void DisplayOptionsWidget::systemStateChanged(
	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	Q_UNUSED (sender)
	Q_UNUSED (err)

	bool change = false;

	if (m_hwInterface != hwmode) {
		
		m_hwInterface = hwmode;
		//hwInterfaceChanged();
		change = true;
	}

	if (m_serverMode != mode) {
		
		m_serverMode = mode;
		change = true;
	}
		
	if (m_dataEngineState != state) {

		m_dataEngineState = state;

		if (m_dataEngineState == QSDR::DataEngineUp) {
		
			
		}
		else {

			
		}
		change = true;
	}
	if (!change) return;

	update();
}

void DisplayOptionsWidget::graphicModeChanged(
	QObject *sender,
	QSDRGraphics::_Panadapter panMode,
	QSDRGraphics::_WfScheme waterColorScheme)
{
	Q_UNUSED (sender)

	bool change = false;

	if (m_panadapterMode != panMode) {
		
		m_panadapterMode = panMode;
		change = true;
	}

	if (m_waterColorScheme != waterColorScheme) {
		
		m_waterColorScheme = waterColorScheme;
		change = true;
	}

	if (!change) return;

	update();
}

void DisplayOptionsWidget::createFPSGroupBox() {

	int fontMaxWidth = m_fonts.smallFontMetrics->boundingRect(" 200 ").width();

	m_fpsSlider = new QSlider(Qt::Horizontal, this);
	m_fpsSlider->setTickPosition(QSlider::NoTicks);
	m_fpsSlider->setFixedSize(190, 12);
	m_fpsSlider->setSingleStep(1);
	m_fpsSlider->setRange(1, 200);
	m_fpsSlider->setValue(m_framesPerSecond);
	m_fpsSlider->setStyleSheet(set->getVolSliderStyle());

	CHECKED_CONNECT(m_fpsSlider, SIGNAL(valueChanged(int)), this, SLOT(fpsValueChanged(int)));

	QString str = "%1 ";
	m_fpsLevelLabel = new QLabel(str.arg(m_framesPerSecond, 2, 10, QLatin1Char(' ')), this);
	m_fpsLevelLabel->setFont(m_fonts.smallFont);
	m_fpsLevelLabel->setFixedSize(fontMaxWidth, 12);
	m_fpsLevelLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_fpsLevelLabel->setStyleSheet(set->getSliderLabelStyle());

	QHBoxLayout *hbox = new QHBoxLayout;
	hbox->setSpacing(0);
	hbox->setMargin(0);
	hbox->addStretch();
	hbox->addWidget(m_fpsSlider);
	hbox->addWidget(m_fpsLevelLabel);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(6);
	vbox->addSpacing(6);
	vbox->addLayout(hbox);

	m_fpsGroupBox = new QGroupBox(tr("Frames per Second"), this);
	m_fpsGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	m_fpsGroupBox->setLayout(vbox);
	m_fpsGroupBox->setStyleSheet(set->getWidgetStyle());
	m_fpsGroupBox->setFont(QFont("Arial", 8));
}

void DisplayOptionsWidget::createPanSpectrumOptions() {

	m_PanLineBtn = new AeroButton("Line", this);
	m_PanLineBtn->setRoundness(0);
	m_PanLineBtn->setFixedSize(btn_width, btn_height);
	m_panadapterBtnList.append(m_PanLineBtn);

	CHECKED_CONNECT(
		m_PanLineBtn,
		SIGNAL(clicked()),
		this,
		SLOT(panModeChanged()));

	m_PanFilledLineBtn = new AeroButton("Filled Line", this);
	m_PanFilledLineBtn->setRoundness(0);
	m_PanFilledLineBtn->setFixedSize(btn_width, btn_height);
	m_panadapterBtnList.append(m_PanFilledLineBtn);

	CHECKED_CONNECT(
		m_PanFilledLineBtn,
		SIGNAL(clicked()),
		this,
		SLOT(panModeChanged()));

	m_PanSolidBtn = new AeroButton("Solid", this);
	m_PanSolidBtn->setRoundness(0);
	m_PanSolidBtn->setFixedSize (btn_width, btn_height);
	m_panadapterBtnList.append(m_PanSolidBtn);

	CHECKED_CONNECT(
		m_PanSolidBtn,
		SIGNAL(clicked()),
		this,
		SLOT(panModeChanged()));

	switch (m_panadapterMode) {

		case QSDRGraphics::Line:

			m_PanLineBtn->setBtnState(AeroButton::ON);
			m_PanFilledLineBtn->setBtnState(AeroButton::OFF);
			m_PanSolidBtn->setBtnState(AeroButton::OFF);
			break;

		case QSDRGraphics::FilledLine:

			m_PanFilledLineBtn->setBtnState(AeroButton::ON);
			m_PanLineBtn->setBtnState(AeroButton::OFF);
			m_PanSolidBtn->setBtnState(AeroButton::OFF);
			break;

		case QSDRGraphics::Solid:

			m_PanSolidBtn->setBtnState(AeroButton::ON);
			m_PanLineBtn->setBtnState(AeroButton::OFF);
			m_PanFilledLineBtn->setBtnState(AeroButton::OFF);
			break;
	}

	int fontMaxWidth = m_fonts.smallFontMetrics->boundingRect(" 200 ").width();

	m_avgValue = set->getSpectrumAveragingCnt();

	m_avgSlider = new QSlider(Qt::Horizontal, this);
	m_avgSlider->setTickPosition(QSlider::NoTicks);
	m_avgSlider->setFixedSize(130, 12);
	m_avgSlider->setSingleStep(1);
	m_avgSlider->setRange(1, 100);
	m_avgSlider->setValue(m_avgValue);
	m_avgSlider->setStyleSheet(set->getVolSliderStyle());

	CHECKED_CONNECT(m_avgSlider, SIGNAL(valueChanged(int)), this, SLOT(averagingFilterCntChanged(int)));

	QString str = "%1 ";
	m_avgLevelLabel = new QLabel(str.arg(m_avgValue, 2, 10, QLatin1Char(' ')), this);
	m_avgLevelLabel->setFont(m_fonts.smallFont);
	m_avgLevelLabel->setFixedSize(fontMaxWidth, 12);
	m_avgLevelLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_avgLevelLabel->setStyleSheet(set->getSliderLabelStyle());

	m_avgLabel = new QLabel("Avg Filter:", this);
	m_avgLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_avgLabel->setStyleSheet(set->getLabelStyle());

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addWidget(m_PanLineBtn);
	hbox1->addWidget(m_PanFilledLineBtn);
	hbox1->addWidget(m_PanSolidBtn);

	QHBoxLayout *hbox2 = new QHBoxLayout;
	hbox2->setSpacing(0);
	hbox2->setMargin(0);
	hbox2->addWidget(m_avgLabel);
	hbox2->addStretch();
	hbox2->addWidget(m_avgSlider);
	hbox2->addWidget(m_avgLevelLabel);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(6);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);

	m_panSpectrumOptions = new QGroupBox(tr("Panadapter Spectrum"), this);
	m_panSpectrumOptions->setMinimumWidth(m_minimumGroupBoxWidth);
	m_panSpectrumOptions->setLayout(vbox);
	m_panSpectrumOptions->setStyleSheet(set->getWidgetStyle());
	m_panSpectrumOptions->setFont(QFont("Arial", 8));
}

void DisplayOptionsWidget::createWaterfallSpectrumOptions() {

	m_waterfallSimpleBtn = new AeroButton("Simple", this);
	m_waterfallSimpleBtn->setRoundness(0);
	m_waterfallSimpleBtn->setFixedSize(btn_width, btn_height);
	m_waterfallSchemeBtnList.append(m_waterfallSimpleBtn);

	CHECKED_CONNECT(
		m_waterfallSimpleBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(waterfallSchemeChanged()));

	m_waterfallEnhancedBtn = new AeroButton("Enhanced", this);
	m_waterfallEnhancedBtn->setRoundness(0);
	m_waterfallEnhancedBtn->setFixedSize(btn_width, btn_height);
	m_waterfallSchemeBtnList.append(m_waterfallEnhancedBtn);

	CHECKED_CONNECT(
		m_waterfallEnhancedBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(waterfallSchemeChanged()));

	m_waterfallSpectranBtn = new AeroButton("Spectran", this);
	m_waterfallSpectranBtn->setRoundness(0);
	m_waterfallSpectranBtn->setFixedSize(btn_width, btn_height);
	m_waterfallSchemeBtnList.append(m_waterfallSpectranBtn);

	CHECKED_CONNECT(
		m_waterfallSpectranBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(waterfallSchemeChanged()));

	switch (m_waterColorScheme) {

		case QSDRGraphics::simple:
			
			m_waterfallSimpleBtn->setBtnState(AeroButton::ON);
			m_waterfallEnhancedBtn->setBtnState(AeroButton::OFF);
			m_waterfallSpectranBtn->setBtnState(AeroButton::OFF);
			break;

		case QSDRGraphics::enhanced:
			
			m_waterfallEnhancedBtn->setBtnState(AeroButton::ON);
			m_waterfallSimpleBtn->setBtnState(AeroButton::OFF);
			m_waterfallSpectranBtn->setBtnState(AeroButton::OFF);
			break;

		case QSDRGraphics::spectran:
			
			m_waterfallSpectranBtn->setBtnState(AeroButton::ON);
			m_waterfallSimpleBtn->setBtnState(AeroButton::OFF);
			m_waterfallEnhancedBtn->setBtnState(AeroButton::OFF);
			break;
	}

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(4);
	hbox1->addStretch();
	//hbox1->addSpacing(77);
	hbox1->addWidget(m_waterfallSimpleBtn);
	hbox1->addWidget(m_waterfallEnhancedBtn);
	hbox1->addWidget(m_waterfallSpectranBtn);

	/*m_waterfallTimeSpinBox = new QSpinBox(this);
	m_waterfallTimeSpinBox->setMinimum(5);
	m_waterfallTimeSpinBox->setMaximum(250);
	m_waterfallTimeSpinBox->setSingleStep(5);
	m_waterfallTimeSpinBox->setStyleSheet(set->getSpinBoxStyle());
	m_waterfallTimeSpinBox->setValue(m_rxDataList.at(m_currentReceiver).waterfallTime);

	CHECKED_CONNECT(
		m_waterfallTimeSpinBox,
		SIGNAL(valueChanged(int)),
		this,
		SLOT(waterfallTimeChanged(int)));*/

	m_waterfallLoOffsetSpinBox = new QSpinBox(this);
	m_waterfallLoOffsetSpinBox->setMinimum(-200);
	m_waterfallLoOffsetSpinBox->setMaximum(200);
	m_waterfallLoOffsetSpinBox->setSingleStep(1);
	m_waterfallLoOffsetSpinBox->setStyleSheet(set->getSpinBoxStyle());
	m_waterfallLoOffsetSpinBox->setValue(m_rxDataList.at(m_currentReceiver).waterfallOffsetLo);

	CHECKED_CONNECT(
		m_waterfallLoOffsetSpinBox,
		SIGNAL(valueChanged(int)),
		this,
		SLOT(waterfallLoOffsetChanged(int)));

	m_waterfallHiOffsetSpinBox = new QSpinBox(this);
	m_waterfallHiOffsetSpinBox->setMinimum(-200);
	m_waterfallHiOffsetSpinBox->setMaximum(200);
	m_waterfallHiOffsetSpinBox->setSingleStep(1);
	m_waterfallHiOffsetSpinBox->setStyleSheet(set->getSpinBoxStyle());
	m_waterfallHiOffsetSpinBox->setValue(m_rxDataList.at(m_currentReceiver).waterfallOffsetHi);

	CHECKED_CONNECT(
		m_waterfallHiOffsetSpinBox,
		SIGNAL(valueChanged(int)),
		this,
		SLOT(waterfallHiOffsetChanged(int)));

	//m_waterfallTimeLabel = new QLabel("Timing (ms):", this);
    //m_waterfallTimeLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	//m_waterfallTimeLabel->setStyleSheet(set->getLabelStyle());

	m_waterfallLoOffsetLabel = new QLabel("Offset Low (dBm):", this);
    m_waterfallLoOffsetLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_waterfallLoOffsetLabel->setStyleSheet(set->getLabelStyle());

	m_waterfallHiOffsetLabel = new QLabel("Offset High (dBm):", this);
    m_waterfallHiOffsetLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_waterfallHiOffsetLabel->setStyleSheet(set->getLabelStyle());

	/*QHBoxLayout *hbox2 = new QHBoxLayout;
	hbox2->setSpacing(4);
	hbox2->addWidget(m_waterfallTimeLabel);
	hbox2->addStretch();
	hbox2->addWidget(m_waterfallTimeSpinBox);*/

	QHBoxLayout *hbox3 = new QHBoxLayout;
	hbox3->setSpacing(4);
	hbox3->addWidget(m_waterfallLoOffsetLabel);
	hbox3->addStretch();
	hbox3->addWidget(m_waterfallLoOffsetSpinBox);

	QHBoxLayout *hbox4 = new QHBoxLayout;
	hbox4->setSpacing(4);
	hbox4->addWidget(m_waterfallHiOffsetLabel);
	hbox4->addStretch();
	hbox4->addWidget(m_waterfallHiOffsetSpinBox);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(6);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	//vbox->addLayout(hbox2);
	vbox->addLayout(hbox3);
	vbox->addLayout(hbox4);

	m_waterfallSpectrumOptions = new QGroupBox(tr("Waterfall Spectrum"), this);
	m_waterfallSpectrumOptions->setMinimumWidth(m_minimumGroupBoxWidth);
	m_waterfallSpectrumOptions->setLayout(vbox);
	m_waterfallSpectrumOptions->setStyleSheet(set->getWidgetStyle());
	m_waterfallSpectrumOptions->setFont(QFont("Arial", 8));
}

void DisplayOptionsWidget::createSMeterOptions() {

	/*m_sMeterA = new AeroButton("S-Meter A", this);
	m_sMeterA->setRoundness(0);
	m_sMeterA->setFixedSize (btn_width, btn_height);

	CHECKED_CONNECT(
		m_sMeterA, 
		SIGNAL(clicked()), 
		this, 
		SLOT(sMeterChanged()));

	m_sMeterB = new AeroButton("S-Meter B", this);
	m_sMeterB->setRoundness(0);
	m_sMeterB->setFixedSize (btn_width, btn_height);

	CHECKED_CONNECT(
		m_sMeterA, 
		SIGNAL(clicked()), 
		this, 
		SLOT(sMeterChanged()));*/
	
	m_sMeterHoldTimeSpinBox = new QSpinBox(this);
	m_sMeterHoldTimeSpinBox->setMinimum(500);
	m_sMeterHoldTimeSpinBox->setMaximum(10000);
	m_sMeterHoldTimeSpinBox->setSingleStep(500);
	m_sMeterHoldTimeSpinBox->setStyleSheet(set->getSpinBoxStyle());
	m_sMeterHoldTimeSpinBox->setValue(m_sMeterHoldTime);

	CHECKED_CONNECT(
		m_sMeterHoldTimeSpinBox,
		SIGNAL(valueChanged(int)),
		this,
		SLOT(sMeterHoldTimeChanged(int)));

	m_sMeterHoldTimeLabel = new QLabel("Hold Time (ms):", this);
    m_sMeterHoldTimeLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_sMeterHoldTimeLabel->setStyleSheet(set->getLabelStyle());

	/*QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addWidget(m_sMeterA);
	hbox1->addWidget(m_sMeterB);*/

	QHBoxLayout *hbox2 = new QHBoxLayout;
	hbox2->setSpacing(4);
	//hbox2->addSpacing(77);
	hbox2->addWidget(m_sMeterHoldTimeLabel);
	hbox2->addStretch();
	hbox2->addWidget(m_sMeterHoldTimeSpinBox);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(6);
	vbox->addSpacing(6);
	//vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);
	//vbox->addLayout(hbox3);
	//vbox->addLayout(hbox4);

	m_sMeterOptions = new QGroupBox(tr("S-Meter"), this);
	m_sMeterOptions->setMinimumWidth(m_minimumGroupBoxWidth);
	m_sMeterOptions->setLayout(vbox);
	m_sMeterOptions->setStyleSheet(set->getWidgetStyle());
	m_sMeterOptions->setFont(QFont("Arial", 8));
}

void DisplayOptionsWidget::createCallSignEditor() {

	callSignLineEdit = new QLineEdit(this);
	callSignLineEdit->setStyleSheet(set->getLineEditStyle());
	callSignLineEdit->setText(set->getCallsign());

	CHECKED_CONNECT(
		callSignLineEdit, 
		SIGNAL(textEdited(const QString &)), 
		this, 
		SLOT(callSignTextChanged(const QString &)));

	m_setCallSignBtn = new AeroButton("Set", this);
	m_setCallSignBtn->setRoundness(0);
	m_setCallSignBtn->setFixedSize(btn_width, btn_height);
	
	CHECKED_CONNECT(
		m_setCallSignBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(callSignChanged()));

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(4);
	//hbox1->addStretch();
	hbox1->addWidget(callSignLineEdit);
	hbox1->addStretch();
	hbox1->addWidget(m_setCallSignBtn);
	//hbox1->addWidget(m_sMeterB);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(6);
	vbox->addLayout(hbox1);
	//vbox->addLayout(hbox2);

	m_callSignEditor = new QGroupBox(tr("Call Sign Editor"), this);
	m_callSignEditor->setMinimumWidth(m_minimumGroupBoxWidth);
	m_callSignEditor->setLayout(vbox);
	m_callSignEditor->setStyleSheet(set->getWidgetStyle());
	m_callSignEditor->setFont(QFont("Arial", 8));
}

void DisplayOptionsWidget::panModeChanged() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = m_panadapterBtnList.indexOf(button);

	foreach(AeroButton *btn, m_panadapterBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			m_panadapterMode = QSDRGraphics::Line;
			DISPLAYOPTIONS_DEBUG << "set panadapter to line.";
			break;
			
		case 1:
			m_panadapterMode = QSDRGraphics::FilledLine;
			DISPLAYOPTIONS_DEBUG << "set panadapter to filled line.";
			break;

		case 2:
			m_panadapterMode = QSDRGraphics::Solid;
			DISPLAYOPTIONS_DEBUG << "set panadapter to solid.";
			break;
	}

	set->setGraphicsState(this, m_panadapterMode, m_waterColorScheme);
}

void DisplayOptionsWidget::sMeterChanged() {
}

void DisplayOptionsWidget::waterfallSchemeChanged() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = m_waterfallSchemeBtnList.indexOf(button);

	foreach(AeroButton *btn, m_waterfallSchemeBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			m_waterColorScheme = QSDRGraphics::simple;
			DISPLAYOPTIONS_DEBUG << "set waterfall scheme to simple.";
			break;
			
		case 1:
			m_waterColorScheme = QSDRGraphics::enhanced;
			DISPLAYOPTIONS_DEBUG << "set waterfall scheme to enhanced.";
			break;

		case 2:
			m_waterColorScheme = QSDRGraphics::spectran;
			DISPLAYOPTIONS_DEBUG << "set waterfall scheme to spectran.";
			break;
	}

	set->setGraphicsState(this, m_panadapterMode, m_waterColorScheme);
}

void DisplayOptionsWidget::fpsValueChanged(int value) {

	m_framesPerSecond = value;

	QString str = "%1 ";
	m_fpsLevelLabel->setText(str.arg(m_framesPerSecond, 2, 10, QLatin1Char(' ')));
	set->setFramesPerSecond(this, m_currentReceiver, value);
}

void DisplayOptionsWidget::setFramesPerSecond(QObject *sender, int rx, int value) {

	Q_UNUSED(sender)

	disconnect(m_fpsSlider, SIGNAL(valueChanged(int)), this, SLOT(fpsValueChanged(int)));
	m_fpsSlider->setValue(value);
	m_fpsSlider->update();
	connect(m_fpsSlider, SIGNAL(valueChanged(int)), this, SLOT(fpsValueChanged(int)));

	QString str = "%1 ";
	m_fpsLevelLabel->setText(str.arg(value, 2, 10, QLatin1Char(' ')));
}

void DisplayOptionsWidget::averagingFilterCntChanged(int value) {

	m_avgValue = value;

	QString str = "%1 ";
	m_avgLevelLabel->setText(str.arg(m_avgValue, 2, 10, QLatin1Char(' ')));

	set->setSpectrumAveragingCnt(value);
}

void DisplayOptionsWidget::sampleRateChanged(QObject *sender, int value) {

	Q_UNUSED(sender)

	m_sampleRate = value;
	//int currentValue = m_framesPerSecondSpinBox->value();

	//m_framesPerSecondSpinBox->setMaximum(m_sampleRate/1000);

	//if (currentValue > m_sampleRate/1000) {

	//	m_framesPerSecondSpinBox->setValue(m_sampleRate/1000);
	//	set->setFramesPerSecond(value);
	//}
}

void DisplayOptionsWidget::waterfallTimeChanged(int value) {

	set->setWaterfallTime(m_currentReceiver, value);
}

void DisplayOptionsWidget::waterfallLoOffsetChanged(int value) {

	set->setWaterfallOffesetLo(m_currentReceiver, value);
}

void DisplayOptionsWidget::waterfallHiOffsetChanged(int value) {

	set->setWaterfallOffesetHi(m_currentReceiver, value);
}

void DisplayOptionsWidget::sMeterHoldTimeChanged(int value) {

	set->setSMeterHoldTime(value);
}

void DisplayOptionsWidget::setCurrentReceiver(QObject *sender, int rx) {

	Q_UNUSED(sender)

	if (m_currentReceiver == rx) return;
	m_currentReceiver = rx;
}

void DisplayOptionsWidget::callSignTextChanged(const QString& text) {

	m_callSingText = text;
}

void DisplayOptionsWidget::callSignChanged() {

	set->setCallsign(m_callSingText);
}
