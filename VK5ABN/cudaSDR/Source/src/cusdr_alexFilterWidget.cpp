/**
* @file  cusdr_alexFilterWidget.cpp
* @brief Alexiares filter settings widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-08-23
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

#include "cusdr_alexFilterWidget.h"

#define	btn_height		15
#define	btn_width		22
#define	btn_width2		24//28
#define	btn_width3		45


AlexFilterWidget::AlexFilterWidget(QWidget *parent)
	: QWidget(parent)
	, set(Settings::instance())
	, m_frequency(set->getVfoFrequencies().at(0))
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(0)
	, m_hpfFilters(6)
	, m_lpfFilters(7)
	, m_receiver(0)
	, bypassAll(false)
	, lowNoise6m(false)
	, hpf13MHz(false)
	, hpf20MHz(false)
	, hpf9_5MHz(false)
	, hpf6_5MHz(false)
	, hpf1_5MHz(false)
{
	setObjectName("AlexFilterWidget");
	setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 8, 4, 0);
	setMouseTracking(true);

	fonts = new CFonts(this);
	m_fonts = fonts->getFonts();

	btnOff = QColor(68, 68, 68, 255);
	btnOn = QColor(56, 242, 115, 255);

	manualFilterBtn = new AeroButton("Auto", this);
	manualFilterBtn->setRoundness(0);
	manualFilterBtn->setFixedSize (btn_width3, btn_height);
	manualFilterBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(manualFilterBtn, SIGNAL(clicked()), this, SLOT(manualFilterBtnClicked()));

	defaultValuesBtn = new AeroButton("Default", this);
	defaultValuesBtn->setRoundness(0);
	defaultValuesBtn->setFixedSize (btn_width3, btn_height);
	defaultValuesBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(defaultValuesBtn, SIGNAL(clicked()), this, SLOT(defaultValuesBtnClicked()));

	//**************************************************************
	// create groups
	setFilterValues();

	createHPFGroup();
	createLPFGroup();

	// set main layout
	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);
	mainLayout->setContentsMargins(1, 0, 4, 0);

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(0);
	hbox1->setContentsMargins(4, 0, 12, 0);
	hbox1->addStretch();
	hbox1->addWidget(manualFilterBtn);
	hbox1->addWidget(defaultValuesBtn);

	QHBoxLayout *hbox2 = new QHBoxLayout;
	hbox2->setSpacing(0);
	hbox2->setContentsMargins(2, 0, 10, 0);
	hbox2->addWidget(HPFGroup);

	QHBoxLayout *hbox3 = new QHBoxLayout;
	hbox3->setSpacing(0);
	hbox3->setContentsMargins(2, 0, 10, 0);
	hbox3->addWidget(LPFGroup);

	mainLayout->addLayout(hbox1);
	mainLayout->addLayout(hbox2);
	mainLayout->addLayout(hbox3);
	mainLayout->addStretch();

	setLayout(mainLayout);

	initAlexValues();
	setupConnections();

	m_HPFActiveBtnList.at(6)->setColors(btnOn, btnOn);

	setAlexConfiguration((double)(set->getVfoFrequencies().at(0)/1000.0));
}

AlexFilterWidget::~AlexFilterWidget() {

	// disconnect all signals
	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

void AlexFilterWidget::setupConnections() {

	CHECKED_CONNECT(
		set,
		SIGNAL(alexManualStateChanged(QObject *, bool)),
		this,
		SLOT(alexManualStateChanged(QObject *, bool)));

//	CHECKED_CONNECT(
//		set,
//		SIGNAL(currentReceiverChanged(QObject *,int)),
//		this,
//		SLOT(setCurrentReceiver(QObject *, int)));

	/*CHECKED_CONNECT(
		set,
		SIGNAL(vfoFrequencyChanged(QObject *, bool, int, long)),
		this,
		SLOT(setFrequency(QObject *, bool, int, long)));*/

	CHECKED_CONNECT(
		set,
		SIGNAL(vfoFrequencyChanged(QObject *, int, int, long)),
		this,
		SLOT(setFrequency(QObject *, int, int, long)));
}

void AlexFilterWidget::createHPFGroup() {

	QLabel *byPassLabel = new QLabel("byPass", this);
	byPassLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	byPassLabel->setStyleSheet(set->getLabelStyle());

	QLabel *emptyLabel = new QLabel(" ", this);
	emptyLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	emptyLabel->setStyleSheet(set->getLabelStyle());

	QLabel *hpfLabel = new QLabel("HPF (kHz)", this);
	hpfLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	hpfLabel->setStyleSheet(set->getLabelStyle());

	QLabel *mHz1_5Label = new QLabel("1.5 MHz", this);
	mHz1_5Label->setFrameStyle(QFrame::Box | QFrame::Raised);
	mHz1_5Label->setStyleSheet(set->getLabelStyle());
	m_HPFLabelList.append(mHz1_5Label);

	QLabel *mHz6_5Label = new QLabel("6.5 MHz", this);
	mHz6_5Label->setFrameStyle(QFrame::Box | QFrame::Raised);
	mHz6_5Label->setStyleSheet(set->getLabelStyle());
	m_HPFLabelList.append(mHz6_5Label);

	QLabel *mHz9_5Label = new QLabel("9.5 MHz", this);
	mHz9_5Label->setFrameStyle(QFrame::Box | QFrame::Raised);
	mHz9_5Label->setStyleSheet(set->getLabelStyle());
	m_HPFLabelList.append(mHz9_5Label);

	QLabel *mHz13Label = new QLabel("13 MHz", this);
	mHz13Label->setFrameStyle(QFrame::Box | QFrame::Raised);
	mHz13Label->setStyleSheet(set->getLabelStyle());
	m_HPFLabelList.append(mHz13Label);

	QLabel *mHz20Label = new QLabel("20 MHz", this);
	mHz20Label->setFrameStyle(QFrame::Box | QFrame::Raised);
	mHz20Label->setStyleSheet(set->getLabelStyle());
	m_HPFLabelList.append(mHz20Label);

	QLabel *m6BPF_LNALabel = new QLabel("6m LNA", this);
	m6BPF_LNALabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m6BPF_LNALabel->setStyleSheet(set->getLabelStyle());
	m_HPFLabelList.append(m6BPF_LNALabel);

	QLabel *byPassAllLabel = new QLabel("Bypass", this);
	byPassAllLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	byPassAllLabel->setStyleSheet(set->getLabelStyle());

	mhz55HPFLabel = new QLabel("      55 MHz HPF     ", this);
	mhz55HPFLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	mhz55HPFLabel->setStyleSheet(set->getLabelStyle());

	int fontMaxWidth = m_fonts.smallFontMetrics->boundingRect("000000.0").width() + 30;

	QList<long> values = set->getHPFLoFrequencies();

	// HPF lo spin boxes
	for (int i = 0; i < m_hpfFilters; i++) {

		QDoubleSpinBox *spinBox = new QDoubleSpinBox();
		spinBox->setStyleSheet(set->getDoubleSpinBoxStyle());
		spinBox->setDecimals(1);
		spinBox->setWrapping(true);
		spinBox->setMinimumWidth(fontMaxWidth);
		spinBox->setRange(m_HPFFrequencyRangeLoList.at(i).first, m_HPFFrequencyRangeLoList.at(i).second);
		spinBox->setSingleStep(0.5);
		spinBox->setValue((double)(values.at(i)/1000.0));

		CHECKED_CONNECT(spinBox, SIGNAL(valueChanged(double)), this, SLOT(hpfLoSpinBoxValueChanged(double)));

		m_HPFLoSpinBoxList << spinBox;
	}

	values = set->getHPFHiFrequencies();

	// HPF hi spin boxes
	for (int i = 0; i < m_hpfFilters; i++) {

		QDoubleSpinBox *spinBox = new QDoubleSpinBox();
		spinBox->setStyleSheet(set->getDoubleSpinBoxStyle());
		spinBox->setDecimals(1);
		spinBox->setWrapping(true);
		spinBox->setMinimumWidth(fontMaxWidth);
		spinBox->setRange(m_HPFFrequencyRangeHiList.at(i).first, m_HPFFrequencyRangeHiList.at(i).second);
		spinBox->setSingleStep(0.5);
		spinBox->setValue((double)(values.at(i)/1000.0));

		CHECKED_CONNECT(spinBox, SIGNAL(valueChanged(double)), this, SLOT(hpfHiSpinBoxValueChanged(double)));

		m_HPFHiSpinBoxList << spinBox;
	}

	// HPF active buttons
	for (int i = 0; i < m_hpfFilters+1; i++) {

		QHLed *btn = new QHLed("", this);
		btn->setFixedSize(4, btn_height);
		btn->setColors(btnOff, btnOff);

		m_HPFActiveBtnList << btn;
	}


	hpf1_5MHzBtn = new AeroButton("Off", this);
	hpf1_5MHzBtn->setRoundness(0);
	hpf1_5MHzBtn->setFixedSize(btn_width, btn_height);
	hpf1_5MHzBtn->setStyleSheet(set->getMiniButtonStyle());
	hpf1_5MHzBtn->setBtnState(AeroButton::OFF);
	m_HPFBtnList.append(hpf1_5MHzBtn);

	CHECKED_CONNECT(hpf1_5MHzBtn, SIGNAL(clicked()), this, SLOT(hpf1_5MHzBtnClicked()));

	hpf6_5MHzBtn = new AeroButton("Off", this);
	hpf6_5MHzBtn->setRoundness(0);
	hpf6_5MHzBtn->setFixedSize(btn_width, btn_height);
	hpf6_5MHzBtn->setStyleSheet(set->getMiniButtonStyle());
	hpf6_5MHzBtn->setBtnState(AeroButton::OFF);
	m_HPFBtnList.append(hpf6_5MHzBtn);

	CHECKED_CONNECT(hpf6_5MHzBtn, SIGNAL(clicked()), this, SLOT(hpf6_5MHzBtnClicked()));

	hpf9_5MHzBtn = new AeroButton("Off", this);
	hpf9_5MHzBtn->setRoundness(0);
	hpf9_5MHzBtn->setFixedSize(btn_width, btn_height);
	hpf9_5MHzBtn->setStyleSheet(set->getMiniButtonStyle());
	hpf9_5MHzBtn->setBtnState(AeroButton::OFF);
	m_HPFBtnList.append(hpf9_5MHzBtn);

	CHECKED_CONNECT(hpf9_5MHzBtn, SIGNAL(clicked()), this, SLOT(hpf9_5MHzBtnClicked()));

	hpf13MHzBtn = new AeroButton("Off", this);
	hpf13MHzBtn->setRoundness(0);
	hpf13MHzBtn->setFixedSize(btn_width, btn_height);
	hpf13MHzBtn->setStyleSheet(set->getMiniButtonStyle());
	hpf13MHzBtn->setBtnState(AeroButton::OFF);
	m_HPFBtnList.append(hpf13MHzBtn);

	CHECKED_CONNECT(hpf13MHzBtn, SIGNAL(clicked()), this, SLOT(hpf13MHzBtnClicked()));

	hpf20MHzBtn = new AeroButton("Off", this);
	hpf20MHzBtn->setRoundness(0);
	hpf20MHzBtn->setFixedSize(btn_width, btn_height);
	hpf20MHzBtn->setStyleSheet(set->getMiniButtonStyle());
	hpf20MHzBtn->setBtnState(AeroButton::OFF);
	m_HPFBtnList.append(hpf20MHzBtn);

	CHECKED_CONNECT(hpf20MHzBtn, SIGNAL(clicked()), this, SLOT(hpf20MHzBtnClicked()));

	lowNoise6mAmpBtn = new AeroButton("Off", this);
	lowNoise6mAmpBtn->setRoundness(0);
	lowNoise6mAmpBtn->setFixedSize(btn_width, btn_height);
	lowNoise6mAmpBtn->setStyleSheet(set->getMiniButtonStyle());
	lowNoise6mAmpBtn->setBtnState(AeroButton::OFF);
	m_HPFBtnList.append(lowNoise6mAmpBtn);

	CHECKED_CONNECT(lowNoise6mAmpBtn, SIGNAL(clicked()), this, SLOT(lowNoise6mAmpBtnClicked()));

	bypassAllHPFBtn = new AeroButton("Off", this);
	bypassAllHPFBtn->setRoundness(0);
	bypassAllHPFBtn->setFixedSize (btn_width, btn_height);
	bypassAllHPFBtn->setStyleSheet(set->getMiniButtonStyle());
	bypassAllHPFBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(bypassAllHPFBtn, SIGNAL(clicked()), this, SLOT(bypassAllHPFBtnClicked()));


	QGridLayout* grid = new QGridLayout(this);
	grid->setVerticalSpacing(1);
	grid->setHorizontalSpacing(1);
	grid->setContentsMargins(2, 12, 2, 7);

	grid->addWidget(byPassLabel, 	0, 0, 1, 2, Qt::AlignLeft);
	grid->addWidget(hpfLabel, 		0, 2, 1, 3, Qt::AlignCenter);

	for (int i = 0; i < m_hpfFilters; i++) {

		grid->addWidget(m_HPFBtnList.at(i), 		i+1, 0, 1, 1, Qt::AlignLeft);
		grid->addWidget(m_HPFLabelList.at(i), 		i+1, 1, 1, 1, Qt::AlignLeft);
		grid->addWidget(m_HPFLoSpinBoxList.at(i), 	i+1, 2, 1, 1, Qt::AlignCenter);
		grid->addWidget(m_HPFHiSpinBoxList.at(i), 	i+1, 3, 1, 1, Qt::AlignCenter);
		grid->addWidget(m_HPFActiveBtnList.at(i),	i+1, 4, 1, 1, Qt::AlignCenter);
	}

	grid->addWidget(bypassAllHPFBtn, 8, 0, 1, 1, Qt::AlignLeft);
	grid->addWidget(byPassAllLabel, 8, 1, 1, 1, Qt::AlignCenter);
	grid->addWidget(mhz55HPFLabel, 8, 2, 1, 2, Qt::AlignCenter);
	grid->addWidget(m_HPFActiveBtnList.at(m_hpfFilters),	8, 4, 1, 1, Qt::AlignCenter);

	HPFGroup = new QGroupBox(tr("HP Filters"), this);
	HPFGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	HPFGroup->setLayout(grid);
	HPFGroup->setStyleSheet(set->getWidgetStyle());
	HPFGroup->setFont(QFont("Arial", 8));
}

void AlexFilterWidget::createLPFGroup() {

	QLabel *lpfLabel = new QLabel("LPF (kHz)", this);
	lpfLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	lpfLabel->setStyleSheet(set->getLabelStyle());

	QLabel *emptyLabel = new QLabel(" ", this);
	emptyLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	emptyLabel->setStyleSheet(set->getLabelStyle());

	QLabel *m160Label = new QLabel("160m", this);
	m160Label->setFrameStyle(QFrame::Box | QFrame::Raised);
	m160Label->setStyleSheet(set->getLabelStyle());
	m_LPFLabelList.append(m160Label);

	QLabel *m80Label = new QLabel("80m", this);
	m80Label->setFrameStyle(QFrame::Box | QFrame::Raised);
	m80Label->setStyleSheet(set->getLabelStyle());
	m_LPFLabelList.append(m80Label);

	QLabel *m60_40Label = new QLabel("60/40m", this);
	m60_40Label->setFrameStyle(QFrame::Box | QFrame::Raised);
	m60_40Label->setStyleSheet(set->getLabelStyle());
	m_LPFLabelList.append(m60_40Label);

	QLabel *m30_20Label = new QLabel("30/20m", this);
	m30_20Label->setFrameStyle(QFrame::Box | QFrame::Raised);
	m30_20Label->setStyleSheet(set->getLabelStyle());
	m_LPFLabelList.append(m30_20Label);

	QLabel *m17_15Label = new QLabel("17/15m", this);
	m17_15Label->setFrameStyle(QFrame::Box | QFrame::Raised);
	m17_15Label->setStyleSheet(set->getLabelStyle());
	m_LPFLabelList.append(m17_15Label);

	QLabel *m12_10Label = new QLabel("12/10m", this);
	m12_10Label->setFrameStyle(QFrame::Box | QFrame::Raised);
	m12_10Label->setStyleSheet(set->getLabelStyle());
	m_LPFLabelList.append(m12_10Label);

	QLabel *m6Label = new QLabel("6m", this);
	m6Label->setFrameStyle(QFrame::Box | QFrame::Raised);
	m6Label->setStyleSheet(set->getLabelStyle());
	m_LPFLabelList.append(m6Label);

	int fontMaxWidth = m_fonts.smallFontMetrics->boundingRect("000000.0").width() + 30;

	QList<long> values = set->getLPFLoFrequencies();

	// LPF lo spin boxes
	for (int i = 0; i < m_lpfFilters; i++) {

		QDoubleSpinBox *spinBox = new QDoubleSpinBox();
		spinBox->setStyleSheet(set->getDoubleSpinBoxStyle());
		spinBox->setDecimals(1);
		spinBox->setWrapping(true);
		spinBox->setMinimumWidth(fontMaxWidth);
		spinBox->setRange(m_LPFFrequencyRangeLoList.at(i).first, m_LPFFrequencyRangeLoList.at(i).second);
		spinBox->setSingleStep(0.5);
		spinBox->setValue((double)(values.at(i)/1000.0));

		CHECKED_CONNECT(spinBox, SIGNAL(valueChanged(double)), this, SLOT(lpfLoSpinBoxValueChanged(double)));

		m_LPFLoSpinBoxList << spinBox;
	}

	values = set->getLPFHiFrequencies();

	// LPF hi spin boxes
	for (int i = 0; i < m_lpfFilters; i++) {

		QDoubleSpinBox *spinBox = new QDoubleSpinBox();
		spinBox->setStyleSheet(set->getDoubleSpinBoxStyle());
		spinBox->setDecimals(1);
		spinBox->setWrapping(true);
		spinBox->setMinimumWidth(fontMaxWidth);
		spinBox->setRange(m_LPFFrequencyRangeHiList.at(i).first, m_LPFFrequencyRangeHiList.at(i).second);
		spinBox->setSingleStep(0.5);
		spinBox->setValue((double)(values.at(i)/1000.0));

		CHECKED_CONNECT(spinBox, SIGNAL(valueChanged(double)), this, SLOT(lpfHiSpinBoxValueChanged(double)));

		m_LPFHiSpinBoxList << spinBox;
	}

	// LPF active buttons
	for (int i = 0; i < m_lpfFilters; i++) {

		QHLed *btn = new QHLed("", this);
		btn->setFixedSize(4, btn_height);
		btn->setColors(btnOff, btnOff);

		m_LPFActiveBtnList << btn;

		//CHECKED_CONNECT(spinBox, SIGNAL(valueChanged(double)), this, SLOT(hpfHiSpinBoxValueChanged(double)));
	}

	QGridLayout* grid = new QGridLayout(this);
	grid->setVerticalSpacing(1);
	grid->setHorizontalSpacing(1);
	grid->setContentsMargins(28, 12, 2, 7);

	grid->addWidget(emptyLabel, 0, 0, 1, 1, Qt::AlignCenter);
	grid->addWidget(lpfLabel, 	0, 1, 1, 3, Qt::AlignCenter);

	for (int i = 0; i < m_lpfFilters; i++) {

		grid->addWidget(m_LPFLabelList.at(i), 		i+1, 0, 1, 1, Qt::AlignLeft);
		grid->addWidget(m_LPFLoSpinBoxList.at(i),	i+1, 1, 1, 1, Qt::AlignCenter);
		grid->addWidget(m_LPFHiSpinBoxList.at(i), 	i+1, 2, 1, 1, Qt::AlignCenter);
		grid->addWidget(m_LPFActiveBtnList.at(i), 	i+1, 3, 1, 1, Qt::AlignCenter);
	}

	LPFGroup = new QGroupBox(tr("LP Filters"), this);
	LPFGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	LPFGroup->setLayout(grid);
	LPFGroup->setStyleSheet(set->getWidgetStyle());
	LPFGroup->setFont(QFont("Arial", 8));
}

//*****************

void AlexFilterWidget::setFilterValues() {

	m_HPFFrequencyRangeLoList << qMakePair(0.0, 2000.0);
	m_HPFFrequencyRangeLoList << qMakePair(6000.0, 8000.0);
	m_HPFFrequencyRangeLoList << qMakePair(9000.0, 11000.0);
	m_HPFFrequencyRangeLoList << qMakePair(12000.0, 15000.0);
	m_HPFFrequencyRangeLoList << qMakePair(18000.0, 25000.0);
	m_HPFFrequencyRangeLoList << qMakePair(49000.0, 52500.0);

	m_HPFFrequencyRangeHiList << qMakePair(1600.0, 6000.0);
	m_HPFFrequencyRangeHiList << qMakePair(7000.0, 9500.0);
	m_HPFFrequencyRangeHiList << qMakePair(10000.0, 13000.0);
	m_HPFFrequencyRangeHiList << qMakePair(13700.0, 19000.0);
	m_HPFFrequencyRangeHiList << qMakePair(25000.0, 32000.0);
	m_HPFFrequencyRangeHiList << qMakePair(52500.0, 55000.0);

	m_LPFFrequencyRangeLoList << qMakePair(0.0, 1900.0);
	m_LPFFrequencyRangeLoList << qMakePair(2000.0, 4000.0);
	m_LPFFrequencyRangeLoList << qMakePair(5000.0, 11000.0);
	m_LPFFrequencyRangeLoList << qMakePair(9000.0, 15000.0);
	m_LPFFrequencyRangeLoList << qMakePair(17000.0, 22000.0);
	m_LPFFrequencyRangeLoList << qMakePair(23000.0, 30000.0);
	m_LPFFrequencyRangeLoList << qMakePair(30000.0, 52000.0);

	m_LPFFrequencyRangeHiList << qMakePair(1000.0, 3000.0);
	m_LPFFrequencyRangeHiList << qMakePair(2000.0, 5000.0);
	m_LPFFrequencyRangeHiList << qMakePair(5000.0, 8000.0);
	m_LPFFrequencyRangeHiList << qMakePair(9000.0, 15000.0);
	m_LPFFrequencyRangeHiList << qMakePair(17000.0, 22000.0);
	m_LPFFrequencyRangeHiList << qMakePair(23000.0, 30000.0);
	m_LPFFrequencyRangeHiList << qMakePair(52000.0, 66000.0);


	m_HPFLoDefaultFrequencyList << 1500.0;
	m_HPFLoDefaultFrequencyList << 7000.0;
	m_HPFLoDefaultFrequencyList << 10100.0;
	m_HPFLoDefaultFrequencyList << 14000.0;
	m_HPFLoDefaultFrequencyList << 21000.0;
	m_HPFLoDefaultFrequencyList << 50000.0;

	m_HPFHiDefaultFrequencyList << 5500.0;
	m_HPFHiDefaultFrequencyList << 7300.0;
	m_HPFHiDefaultFrequencyList << 10150.0;
	m_HPFHiDefaultFrequencyList << 18168.0;
	m_HPFHiDefaultFrequencyList << 29700.0;
	m_HPFHiDefaultFrequencyList << 54000.0;

	m_LPFLoDefaultFrequencyList << 1800.0;
	m_LPFLoDefaultFrequencyList << 3500.0;
	m_LPFLoDefaultFrequencyList << 5330.0;
	m_LPFLoDefaultFrequencyList << 10100.0;
	m_LPFLoDefaultFrequencyList << 18068.0;
	m_LPFLoDefaultFrequencyList << 24890.0;
	m_LPFLoDefaultFrequencyList << 50000.0;

	m_LPFHiDefaultFrequencyList << 2000.0;
	m_LPFHiDefaultFrequencyList << 4000.0;
	m_LPFHiDefaultFrequencyList << 7300.0;
	m_LPFHiDefaultFrequencyList << 14350.0;
	m_LPFHiDefaultFrequencyList << 21450.0;
	m_LPFHiDefaultFrequencyList << 29700.0;
	m_LPFHiDefaultFrequencyList << 54000.0;
}

void AlexFilterWidget::hpfLoSpinBoxValueChanged(double value) {

	QDoubleSpinBox *spinBox = qobject_cast<QDoubleSpinBox *>(sender());

	int filter = m_HPFLoSpinBoxList.indexOf(spinBox);

	set->setAlexHPFLoFrequencies(filter, (long)(value * 1000));
}

void AlexFilterWidget::hpfHiSpinBoxValueChanged(double value) {

	QDoubleSpinBox *spinBox = qobject_cast<QDoubleSpinBox *>(sender());

	int filter = m_HPFHiSpinBoxList.indexOf(spinBox);

	set->setAlexHPFHiFrequencies(filter, (long)(value * 1000));
}

void AlexFilterWidget::lpfLoSpinBoxValueChanged(double value) {

	QDoubleSpinBox *spinBox = qobject_cast<QDoubleSpinBox *>(sender());

	int filter = m_LPFLoSpinBoxList.indexOf(spinBox);

	set->setAlexLPFLoFrequencies(filter, (long)(value * 1000));
}

void AlexFilterWidget::lpfHiSpinBoxValueChanged(double value) {

	QDoubleSpinBox *spinBox = qobject_cast<QDoubleSpinBox *>(sender());

	int filter = m_LPFHiSpinBoxList.indexOf(spinBox);

	set->setAlexLPFHiFrequencies(filter, (long)(value * 1000));
}

void AlexFilterWidget::manualFilterBtnClicked() {

	if (manualFilterBtn->btnState() == AeroButton::OFF) {

		manualFilterBtn->setText("Manual");
		manualFilterBtn->setBtnState(AeroButton::ON);

//		bypassAllHPFBtn->setEnabled(true);
//		lowNoise6mAmpBtn->setEnabled(true);
//		hpf13MHzBtn->setEnabled(true);
//		hpf20MHzBtn->setEnabled(true);
//		hpf9_5MHzBtn->setEnabled(true);
//		hpf6_5MHzBtn->setEnabled(true);
//		hpf1_5MHzBtn->setEnabled(true);
//		lpf30_20mBtn->setEnabled(true);
//		lpf60_40mBtn->setEnabled(true);
//		lpf80mBtn->setEnabled(true);
//		lpf160mBtn->setEnabled(true);
//		lpf6mBtn->setEnabled(true);
//		lpf12_10mBtn->setEnabled(true);
//		lpf17_15mBtn->setEnabled(true);

		m_alexConfig |= 0x01;
	}
	else {

		manualFilterBtn->setText("Auto");
		manualFilterBtn->setBtnState(AeroButton::OFF);

//		bypassAllHPFBtn->setEnabled(false);
//		lowNoise6mAmpBtn->setEnabled(false);
//		hpf13MHzBtn->setEnabled(false);
//		hpf20MHzBtn->setEnabled(false);
//		hpf9_5MHzBtn->setEnabled(false);
//		hpf6_5MHzBtn->setEnabled(false);
//		hpf1_5MHzBtn->setEnabled(false);
//		lpf30_20mBtn->setEnabled(false);
//		lpf60_40mBtn->setEnabled(false);
//		lpf80mBtn->setEnabled(false);
//		lpf160mBtn->setEnabled(false);
//		lpf6mBtn->setEnabled(false);
//		lpf12_10mBtn->setEnabled(false);
//		lpf17_15mBtn->setEnabled(false);

		m_alexConfig |= 0xFFFE;
	}
	manualFilterBtn->update();

	set->setAlexConfiguration(this, m_alexConfig);
}

void AlexFilterWidget::defaultValuesBtnClicked() {

	for (int i = 0; i < m_hpfFilters; i++) {

		m_HPFLoSpinBoxList[i]->setValue(m_HPFLoDefaultFrequencyList.at(i));
		m_HPFHiSpinBoxList[i]->setValue(m_HPFHiDefaultFrequencyList.at(i));
	}

	for (int i = 0; i < m_lpfFilters; i++) {

		m_LPFLoSpinBoxList[i]->setValue(m_LPFLoDefaultFrequencyList.at(i));
		m_LPFHiSpinBoxList[i]->setValue(m_LPFHiDefaultFrequencyList.at(i));
	}
}

void AlexFilterWidget::bypassAllHPFBtnClicked() {

	if (bypassAllHPFBtn->btnState() == AeroButton::OFF) {

		bypassAllHPFBtn->setText("On");
		bypassAllHPFBtn->setBtnState(AeroButton::ON);
		bypassAll = true;
	}
	else {

		bypassAllHPFBtn->setText("Off");
		bypassAllHPFBtn->setBtnState(AeroButton::OFF);
		bypassAll = false;
	}
	bypassAllHPFBtn->update();

	setFrequency(this, 0, m_receiver, set->getVfoFrequencies().at(m_receiver));
}

void AlexFilterWidget::lowNoise6mAmpBtnClicked() {

	if (lowNoise6mAmpBtn->btnState() == AeroButton::OFF) {

		lowNoise6mAmpBtn->setText("On");
		lowNoise6mAmpBtn->setBtnState(AeroButton::ON);
		lowNoise6m = true;
	}
	else {

		lowNoise6mAmpBtn->setText("Off");
		lowNoise6mAmpBtn->setBtnState(AeroButton::OFF);
		lowNoise6m = false;
	}
	lowNoise6mAmpBtn->update();

	setFrequency(this, 0, m_receiver, set->getVfoFrequencies().at(m_receiver));
}

void AlexFilterWidget::hpf1_5MHzBtnClicked() {

	if (hpf1_5MHzBtn->btnState() == AeroButton::OFF) {

		hpf1_5MHzBtn->setBtnState(AeroButton::ON);
		hpf1_5MHzBtn->setText("On");
		hpf1_5MHz = true;
	}
	else {

		hpf1_5MHzBtn->setBtnState(AeroButton::OFF);
		hpf1_5MHzBtn->setText("Off");
		hpf1_5MHz= false;
	}
	hpf1_5MHzBtn->update();

	setFrequency(this, 0, m_receiver, set->getVfoFrequencies().at(m_receiver));
}

void AlexFilterWidget::hpf6_5MHzBtnClicked() {

	if (hpf6_5MHzBtn->btnState() == AeroButton::OFF) {

		hpf6_5MHzBtn->setBtnState(AeroButton::ON);
		hpf6_5MHzBtn->setText("On");
		hpf6_5MHz = true;
	}
	else {

		hpf6_5MHzBtn->setBtnState(AeroButton::OFF);
		hpf6_5MHzBtn->setText("Off");
		hpf6_5MHz = false;
	}
	hpf6_5MHzBtn->update();

	setFrequency(this, 0, m_receiver, set->getVfoFrequencies().at(m_receiver));
}

void AlexFilterWidget::hpf9_5MHzBtnClicked() {

	if (hpf9_5MHzBtn->btnState() == AeroButton::OFF) {

		hpf9_5MHzBtn->setBtnState(AeroButton::ON);
		hpf9_5MHzBtn->setText("On");
		hpf9_5MHz = true;
	}
	else {

		hpf9_5MHzBtn->setBtnState(AeroButton::OFF);
		hpf9_5MHzBtn->setText("Off");
		hpf9_5MHz = false;
	}
	hpf9_5MHzBtn->update();

	setFrequency(this, 0, m_receiver, set->getVfoFrequencies().at(m_receiver));
}

void AlexFilterWidget::hpf13MHzBtnClicked() {

	if (hpf13MHzBtn->btnState() == AeroButton::OFF) {

		hpf13MHzBtn->setBtnState(AeroButton::ON);
		hpf13MHzBtn->setText("On");
		hpf13MHz = true;
	}
	else {

		hpf13MHzBtn->setBtnState(AeroButton::OFF);
		hpf13MHzBtn->setText("Off");
		hpf13MHz = false;
	}
	hpf13MHzBtn->update();

	setFrequency(this, 0, m_receiver, set->getVfoFrequencies().at(m_receiver));
}

void AlexFilterWidget::hpf20MHzBtnClicked() {

	if (hpf20MHzBtn->btnState() == AeroButton::OFF) {

		hpf20MHzBtn->setBtnState(AeroButton::ON);
		hpf20MHzBtn->setText("On");
		hpf20MHz = true;
	}
	else {

		hpf20MHzBtn->setBtnState(AeroButton::OFF);
		hpf20MHzBtn->setText("Off");
		hpf20MHz = false;
	}
	hpf20MHzBtn->update();

	setFrequency(this, 0, m_receiver, set->getVfoFrequencies().at(m_receiver));
}

void AlexFilterWidget::setFrequency(QObject * sender, int mode, int rx, long frequency) {

	Q_UNUSED(sender)
	Q_UNUSED(mode)

	m_receiver = rx;
	m_frequency = frequency;

	if ((m_alexConfig & 0x01) == 0) return;

	setAlexConfiguration((double)(frequency/1000.0));

	set->setAlexConfiguration(this, m_alexConfig);
}

void AlexFilterWidget::setAlexConfiguration(double freq) {

	foreach(QHLed *led, m_HPFActiveBtnList)
		led->setColors(btnOff, btnOff);

	// Alex configuration
	// m_alexConfig (quint16):
	//
	// 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
	//   | | | | | | | | | | | | | | |
	//   | | | | | | | | | | | | | | +-----Alex   - manual HPF/LPF filter select (0 = disable, 1 = enable)
	//   | | | | | | | | | | | | | +------ Alex   -	Bypass all HPFs   (0 = disable, 1 = enable)*
	//   | | | | | | | | | | | | +-------- Alex   -	6M low noise amplifier (0 = disable, 1 = enable)*
	//   | | | | | | | | | | | +---------- Alex   -	select 1.5MHz HPF (0 = disable, 1 = enable)*
	//   | | | | | | | | | | +------------ Alex   -	select 6.5MHz HPF (0 = disable, 1 = enable)*
	//   | | | | | | | | | +-------------- Alex   -	select 9.5MHz HPF (0 = disable, 1 = enable)*
	//   | | | | | | | | +---------------- Alex   -	select 13MHz  HPF (0 = disable, 1 = enable)*
	//   | | | | | | | +------------------ Alex   -	select 20MHz  HPF (0 = disable, 1 = enable)*
	//   | | | | | | +-------------------- Alex   - select 160m   LPF (0 = disable, 1 = enable)*
	//   | | | | | +---------------------- Alex   - select 80m    LPF (0 = disable, 1 = enable)*
	//   | | | | +------------------------ Alex   - select 60/40m LPF (0 = disable, 1 = enable)*
	//   | | | +-------------------------- Alex   - select 30/20m LPF (0 = disable, 1 = enable)*
	//   | | +---------------------------- Alex   - select 17/15m LPF (0 = disable, 1 = enable)*
	//   | +------------------------------ Alex   - select 12/10m LPF (0 = disable, 1 = enable)*
	//   +-------------------------------- Alex   - select 6m     LPF (0 = disable, 1 = enable)*

	// reset
	m_alexConfig &= 1;

	// HPF switching
	if (!bypassAll) {

		// HPF 1.5 MHz
		if (freq >= m_HPFLoSpinBoxList.at(0)->value() && freq <= m_HPFHiSpinBoxList.at(0)->value()) {

			if (!hpf1_5MHz) {

				m_HPFActiveBtnList.at(0)->setColors(btnOn, btnOn);
				m_alexConfig |= 0x08;
			}
			else {

				m_HPFActiveBtnList.at(6)->setColors(btnOn, btnOn);
				m_alexConfig |= 0x02;
			}
		}
		// HPF 6.5 MHz
		else if (freq >= m_HPFLoSpinBoxList.at(1)->value() && freq <= m_HPFHiSpinBoxList.at(1)->value()) {

			if (!hpf6_5MHz) {

				m_HPFActiveBtnList.at(1)->setColors(btnOn, btnOn);
				m_alexConfig |= 0x10;
			}
			else {

				m_HPFActiveBtnList.at(6)->setColors(btnOn, btnOn);
				m_alexConfig |= 0x02;
			}
		}
		// HPF 9.5 MHz
		else if (freq >= m_HPFLoSpinBoxList.at(2)->value() && freq <= m_HPFHiSpinBoxList.at(2)->value()) {

			if (!hpf9_5MHz) {

				m_HPFActiveBtnList.at(2)->setColors(btnOn, btnOn);
				m_alexConfig |= 0x20;
			}
			else {

				m_HPFActiveBtnList.at(6)->setColors(btnOn, btnOn);
				m_alexConfig |= 0x02;
			}
		}
		// HPF 13 MHz
		else if (freq >= m_HPFLoSpinBoxList.at(3)->value() && freq <= m_HPFHiSpinBoxList.at(3)->value()) {

			if (!hpf13MHz) {

				m_HPFActiveBtnList.at(3)->setColors(btnOn, btnOn);
				m_alexConfig |= 0x40;
			}
			else {

				m_HPFActiveBtnList.at(6)->setColors(btnOn, btnOn);
				m_alexConfig |= 0x02;
			}
		}
		// HPF 20 MHz
		else if (freq >= m_HPFLoSpinBoxList.at(4)->value() && freq <= m_HPFHiSpinBoxList.at(4)->value()) {

			if (!hpf20MHz) {

				m_HPFActiveBtnList.at(4)->setColors(btnOn, btnOn);
				m_alexConfig |= 0x80;
			}
			else {

				m_HPFActiveBtnList.at(6)->setColors(btnOn, btnOn);
				m_alexConfig |= 0x02;
			}
		}
		// 6m BPF/LNA
		else if (freq >= m_HPFLoSpinBoxList.at(5)->value() && freq <= m_HPFHiSpinBoxList.at(5)->value()) {

			if (!lowNoise6m) {

				m_HPFActiveBtnList.at(5)->setColors(btnOn, btnOn);
				m_alexConfig |= 0x04;
			}
			else {

				m_HPFActiveBtnList.at(6)->setColors(btnOn, btnOn);
				m_alexConfig |= 0x02;
			}
		}
		else {

			m_HPFActiveBtnList.at(6)->setColors(btnOn, btnOn);
			m_alexConfig |= 0x02;
		}
	}
	// by pass all
	else {

		m_HPFActiveBtnList.at(6)->setColors(btnOn, btnOn);
		m_alexConfig |= 0x02;
	}


	// LPF switching
	foreach(QHLed *led, m_LPFActiveBtnList)
		led->setColors(btnOff, btnOff);

	if (freq >= m_LPFLoSpinBoxList.at(0)->value() && freq <= m_LPFHiSpinBoxList.at(0)->value()) {

		m_LPFActiveBtnList.at(0)->setColors(btnOn, btnOn);
		m_alexConfig |= 0x100;
	}
	else if (freq >= m_LPFLoSpinBoxList.at(1)->value() && freq <= m_LPFHiSpinBoxList.at(1)->value()) {

		m_LPFActiveBtnList.at(1)->setColors(btnOn, btnOn);
		m_alexConfig |= 0x200;
	}
	else if (freq >= m_LPFLoSpinBoxList.at(2)->value() && freq <= m_LPFHiSpinBoxList.at(2)->value()) {

		m_LPFActiveBtnList.at(2)->setColors(btnOn, btnOn);
		m_alexConfig |= 0x400;
	}
	else if (freq >= m_LPFLoSpinBoxList.at(3)->value() && freq <= m_LPFHiSpinBoxList.at(3)->value()) {

		m_LPFActiveBtnList.at(3)->setColors(btnOn, btnOn);
		m_alexConfig |= 0x800;
	}
	else if (freq >= m_LPFLoSpinBoxList.at(4)->value() && freq <= m_LPFHiSpinBoxList.at(4)->value()) {

		m_LPFActiveBtnList.at(4)->setColors(btnOn, btnOn);
		m_alexConfig |= 0x1000;
	}
	else if (freq >= m_LPFLoSpinBoxList.at(5)->value() && freq <= m_LPFHiSpinBoxList.at(5)->value()) {

		m_LPFActiveBtnList.at(5)->setColors(btnOn, btnOn);
		m_alexConfig |= 0x2000;
	}
	else {

		m_LPFActiveBtnList.at(6)->setColors(btnOn, btnOn);
		m_alexConfig |= 0x4000;
	}
}

void AlexFilterWidget::setCurrentReceiver(QObject *sender, int rx) {

	Q_UNUSED(sender)

	m_receiver = rx;
}

void AlexFilterWidget::initAlexValues() {

	// Alex configuration:
	//
	// m_alexConfig (quint16)
	//
	// 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
	//   | | | | | | | | | | | | | | |
	//   | | | | | | | | | | | | | | +-----Alex   - manual HPF/LPF filter select (0 = disable, 1 = enable)
	//   | | | | | | | | | | | | | +------ Alex   -	Bypass all HPFs   (0 = disable, 1 = enable)*
	//   | | | | | | | | | | | | +-------- Alex   -	6M low noise amplifier (0 = disable, 1 = enable)*
	//   | | | | | | | | | | | +---------- Alex   -	select 1.5MHz HPF (0 = disable, 1 = enable)*
	//   | | | | | | | | | | +------------ Alex   -	select 6.5MHz HPF (0 = disable, 1 = enable)*
	//   | | | | | | | | | +-------------- Alex   -	select 9.5MHz HPF (0 = disable, 1 = enable)*
	//   | | | | | | | | +---------------- Alex   -	select 13MHz  HPF (0 = disable, 1 = enable)*
	//   | | | | | | | +------------------ Alex   -	select 20MHz  HPF (0 = disable, 1 = enable)*
	//   | | | | | | +-------------------- Alex   - select 160m   LPF (0 = disable, 1 = enable)*
	//   | | | | | +---------------------- Alex   - select 80m    LPF (0 = disable, 1 = enable)*
	//   | | | | +------------------------ Alex   - select 60/40m LPF (0 = disable, 1 = enable)*
	//   | | | +-------------------------- Alex   - select 30/20m LPF (0 = disable, 1 = enable)*
	//   | | +---------------------------- Alex   - select 17/15m LPF (0 = disable, 1 = enable)*
	//   | +------------------------------ Alex   - select 12/10m LPF (0 = disable, 1 = enable)*
	//   +-------------------------------- Alex   - select 6m     LPF (0 = disable, 1 = enable)*

	m_alexConfig = set->getAlexConfig();
	m_alexStates = set->getAlexStates();


	if (m_alexConfig & 0x01) {

		manualFilterBtn->setText("Manual");
		manualFilterBtn->setBtnState(AeroButton::ON);
	}
	else {

		manualFilterBtn->setText("Auto");
		manualFilterBtn->setBtnState(AeroButton::OFF);
	}
	manualFilterBtn->update();
}

void AlexFilterWidget::alexManualStateChanged(QObject *sender, bool value) {

	Q_UNUSED(sender)

	if (value) {

		m_alexConfig |= 0x01;
		m_receiver = set->getCurrentReceiver();
		setFrequency(this, true, m_receiver, set->getVfoFrequencies().at(m_receiver));

		manualFilterBtn->setText("Manual");
		manualFilterBtn->setBtnState(AeroButton::ON);
	}
	else {

		m_alexConfig &= 0xFFFE;

		foreach(QHLed *led, m_HPFActiveBtnList)
			led->setColors(btnOff, btnOff);

		m_HPFActiveBtnList.at(6)->setColors(btnOn, btnOn);

		manualFilterBtn->setText("Auto");
		manualFilterBtn->setBtnState(AeroButton::OFF);

		set->setAlexConfiguration(this, m_alexConfig);
	}
	manualFilterBtn->update();
}

