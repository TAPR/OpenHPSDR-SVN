/**
* @file cusdr_hpsdrWidget.cpp
* @brief Hardware settings widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-09-21
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
#define LOG_HPSDR_WIDGET

// use: HPSDR_WIDGET_DEBUG

//#include <QtGui>
//#include <QMenu>
//#include <QFileDialog>
//#include <QDebug>

#include "cusdr_hpsdrWidget.h"


#define	btn_height		15
#define	btn_width		74
#define	btn_width2		52
#define	btn_widths		42


HPSDRWidget::HPSDRWidget(QWidget *parent) 
	: QWidget(parent)
	, set(Settings::instance())
	, m_serverMode(set->getCurrentServerMode())
	, m_hwInterface(set->getHWInterface())
	, m_hwInterfaceTemp(set->getHWInterface())
	, m_dataEngineState(QSDR::DataEngineDown)
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(0)
	, m_numberOfReceivers(1)
	, m_hpsdrHardware(set->getHPSDRHardware())
{
	setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 8, 4, 0);
	setMouseTracking(true);
	
	m_firmwareCheck = set->getFirmwareVersionCheck();

	createSource10MhzExclusiveGroup();
	createSource122_88MhzExclusiveGroup();

	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(0);
	hbox1->setContentsMargins(4, 0, 4, 0);
	hbox1->addWidget(hpsdrHardwareBtnGroup());

	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(0);
	hbox2->setContentsMargins(4, 0, 4, 0);
	hbox2->addWidget(source10MhzExclusiveGroup);

	QHBoxLayout *hbox3 = new QHBoxLayout();
	hbox3->setSpacing(0);
	hbox3->setContentsMargins(4, 0, 4, 0);
	hbox3->addWidget(source122_88MhzExclusiveGroup);

	QHBoxLayout *hbox4 = new QHBoxLayout();
	hbox4->setSpacing(0);
	//hbox4->setMargin(0);
	//hbox4->addStretch();
	hbox4->setContentsMargins(4, 0, 4, 0);
	hbox4->addWidget(sampleRateExclusiveGroup());

	QHBoxLayout *hbox5 = new QHBoxLayout();
	hbox5->setSpacing(0);
	hbox5->setContentsMargins(4, 0, 4, 0);
	hbox5->addWidget(numberOfReceiversGroup());

	mainLayout->addLayout(hbox1);
	mainLayout->addLayout(hbox2);
	mainLayout->addLayout(hbox3);
	mainLayout->addLayout(hbox4);
	mainLayout->addLayout(hbox5);
	mainLayout->addStretch();
	setLayout(mainLayout);

	setupConnections();
	setHPSDRHardware();
}

HPSDRWidget::~HPSDRWidget() {

	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

void HPSDRWidget::setupConnections() {

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
}

QGroupBox* HPSDRWidget::hpsdrHardwareBtnGroup() {

	modulesPresenceBtn = new AeroButton("Modules", this);
	modulesPresenceBtn->setRoundness(0);
	modulesPresenceBtn->setFixedSize(btn_width, btn_height);
	modulesPresenceBtn->setBtnState(AeroButton::OFF);
	hardwareBtnList.append(modulesPresenceBtn);
	
	CHECKED_CONNECT(
		modulesPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(hpsdrHardwareChanged()));

	hermesPresenceBtn = new AeroButton("Hermes", this);
	hermesPresenceBtn->setRoundness(0);
	hermesPresenceBtn->setFixedSize(btn_width, btn_height);
	hermesPresenceBtn->setBtnState(AeroButton::OFF);
	hardwareBtnList.append(hermesPresenceBtn);
	
	CHECKED_CONNECT(
		hermesPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(hpsdrHardwareChanged()));

	penelopePresenceBtn = new AeroButton("Penelope", this);
	penelopePresenceBtn->setRoundness(0);
	penelopePresenceBtn->setFixedSize(btn_width, btn_height);
	penelopePresenceBtn->setBtnState(AeroButton::OFF);
	
	CHECKED_CONNECT(
		penelopePresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(penelopePresenceChanged()));

	pennyPresenceBtn = new AeroButton("Pennylane", this);
	pennyPresenceBtn->setRoundness(0);
	pennyPresenceBtn->setFixedSize(btn_width, btn_height);
	pennyPresenceBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		pennyPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(pennyPresenceChanged()));
	
	mercuryPresenceBtn = new AeroButton("Mercury", this);
	mercuryPresenceBtn->setRoundness(0);
	mercuryPresenceBtn->setFixedSize(btn_width, btn_height);
	mercuryPresenceBtn->setBtnState(AeroButton::OFF);
	
	CHECKED_CONNECT(
		mercuryPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(mercuryPresenceChanged()));

	alexPresenceBtn = new AeroButton("Alex", this);
	alexPresenceBtn->setRoundness(0);
	alexPresenceBtn->setFixedSize(btn_width, btn_height);
	alexPresenceBtn->setBtnState(AeroButton::OFF);
	
	CHECKED_CONNECT(
		alexPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(alexPresenceChanged()));
	
	excaliburPresenceBtn = new AeroButton("Excalibur", this);
	excaliburPresenceBtn->setRoundness(0);
	excaliburPresenceBtn->setFixedSize(btn_width, btn_height);
	excaliburPresenceBtn->setBtnState(AeroButton::OFF);
	
	CHECKED_CONNECT(
		excaliburPresenceBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(excaliburPresenceChanged()));
	
	firmwareCheckBtn = new AeroButton("On", this);
	firmwareCheckBtn->setRoundness(0);
	firmwareCheckBtn->setFixedSize(btn_widths, btn_height);

	if (m_firmwareCheck) {

		firmwareCheckBtn->setBtnState(AeroButton::ON);
		firmwareCheckBtn->setText("On");
	}
	else {

		firmwareCheckBtn->setBtnState(AeroButton::OFF);
		firmwareCheckBtn->setText("Off");
	}

	CHECKED_CONNECT(
		firmwareCheckBtn,
		SIGNAL(released()),
		this,
		SLOT(firmwareCheckChanged()));

	m_fwCheckLabel = new QLabel("Firmware Check:", this);
	m_fwCheckLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_fwCheckLabel->setStyleSheet(set->getLabelStyle());

	QHBoxLayout *hbox0 = new QHBoxLayout();
	hbox0->setSpacing(4);
	hbox0->addStretch();
	hbox0->addWidget(m_fwCheckLabel);
	hbox0->addWidget(firmwareCheckBtn);

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addWidget(modulesPresenceBtn);
	hbox1->addWidget(hermesPresenceBtn);
	
	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(4);
	hbox2->addStretch();
	hbox2->addWidget(penelopePresenceBtn);
	hbox2->addWidget(pennyPresenceBtn);

	QHBoxLayout *hbox3 = new QHBoxLayout();
	hbox3->setSpacing(4);
	hbox3->addStretch();
	hbox3->addWidget(mercuryPresenceBtn);
	hbox3->addWidget(excaliburPresenceBtn);

	QHBoxLayout *hbox4 = new QHBoxLayout();
	hbox4->setSpacing(4);
	hbox4->addStretch();
	hbox4->addWidget(alexPresenceBtn);
	
	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox0);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);
	vbox->addLayout(hbox3);
	vbox->addLayout(hbox4);
	
	QGroupBox *groupBox = new QGroupBox(tr("Hardware selection"), this);
	groupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	groupBox->setLayout(vbox);
	groupBox->setStyleSheet(set->getWidgetStyle());
	groupBox->setFont(QFont("Arial", 8));

	return groupBox;
}

void HPSDRWidget::createSource10MhzExclusiveGroup() {

	atlasBtn = new AeroButton("Atlas", this);
	atlasBtn->setRoundness(0);
	atlasBtn->setFixedSize(btn_width, btn_height);
	source10MhzBtnList.append(atlasBtn);

	CHECKED_CONNECT(
		atlasBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(source10MhzChanged()));
	
	penelopeBtn = new AeroButton("Penny[Lane]", this);
	penelopeBtn->setRoundness(0);
	penelopeBtn->setFixedSize(btn_width, btn_height);
	source10MhzBtnList.append(penelopeBtn);

	CHECKED_CONNECT(
		penelopeBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(source10MhzChanged()));
	
	mercuryBtn = new AeroButton("Mercury", this);
	mercuryBtn->setRoundness(0);
	mercuryBtn->setFixedSize(btn_width, btn_height);
	source10MhzBtnList.append(mercuryBtn);

	CHECKED_CONNECT(
		mercuryBtn, 
		SIGNAL(released()), 
		this, 
		SLOT(source10MhzChanged()));
	
	
	sources10Mhz << "Atlas" << "Penelope" << "Mercury";

	switch(set->get10MHzSource()) {

		case 0:
			atlasBtn->setBtnState(AeroButton::ON);
			penelopeBtn->setBtnState(AeroButton::OFF);
			mercuryBtn->setBtnState(AeroButton::OFF);
			break;
			
		case 1:
			penelopeBtn->setBtnState(AeroButton::ON);
			atlasBtn->setBtnState(AeroButton::OFF);
			mercuryBtn->setBtnState(AeroButton::OFF);
			break;

		case 2:
			mercuryBtn->setBtnState(AeroButton::ON);
			atlasBtn->setBtnState(AeroButton::OFF);
			penelopeBtn->setBtnState(AeroButton::OFF);
			break;

		case 3:
			mercuryBtn->setBtnState(AeroButton::OFF);
			atlasBtn->setBtnState(AeroButton::OFF);
			penelopeBtn->setBtnState(AeroButton::OFF);
			break;
	}
	
	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addWidget(penelopeBtn);
	hbox1->addWidget(mercuryBtn);
	
	QHBoxLayout *hbox2 = new QHBoxLayout();
	hbox2->setSpacing(4);
	hbox2->addStretch();
	hbox2->addWidget(atlasBtn);
	
	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);
	
	source10MhzExclusiveGroup = new QGroupBox(tr("10 MHz Clock"), this);
	source10MhzExclusiveGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	source10MhzExclusiveGroup->setLayout(vbox);
	source10MhzExclusiveGroup->setStyleSheet(set->getWidgetStyle());
	source10MhzExclusiveGroup->setFont(QFont("Arial", 8));
}

void HPSDRWidget::createSource122_88MhzExclusiveGroup() {

	penelope2Btn = new AeroButton("Penny[Lane]", this);
	penelope2Btn->setRoundness(0);
	penelope2Btn->setFixedSize(btn_width, btn_height);

	CHECKED_CONNECT(
		penelope2Btn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(source122_88MhzChanged()));

	mercury2Btn = new AeroButton("Mercury", this);
	mercury2Btn->setRoundness(0);
	mercury2Btn->setFixedSize(btn_width, btn_height);

	CHECKED_CONNECT(
		mercury2Btn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(source122_88MhzChanged()));

	switch(set->get122_8MHzSource()) {

		case 0:
			penelope2Btn->setBtnState(AeroButton::ON);
			mercury2Btn->setBtnState(AeroButton::OFF);
			break;

		case 1:
			mercury2Btn->setBtnState(AeroButton::ON);
			penelope2Btn->setBtnState(AeroButton::OFF);
			break;
	}
	
	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addWidget(penelope2Btn);
	hbox1->addWidget(mercury2Btn);
	
	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	
	source122_88MhzExclusiveGroup = new QGroupBox(tr("122.8 MHz Clock"), this);
	source122_88MhzExclusiveGroup->setMinimumWidth(m_minimumGroupBoxWidth);
	source122_88MhzExclusiveGroup->setLayout(vbox);
	source122_88MhzExclusiveGroup->setStyleSheet(set->getWidgetStyle());
	source122_88MhzExclusiveGroup->setFont(QFont("Arial", 8));
}

QGroupBox *HPSDRWidget::sampleRateExclusiveGroup() {

	samplerate48Btn = new AeroButton("48 kHz", this);
	samplerate48Btn->setRoundness(0);
	samplerate48Btn->setFixedSize (50, btn_height);
	samplerate48Btn->setStyleSheet(set->getMiniButtonStyle());
	samplerateBtnList.append(samplerate48Btn);
	CHECKED_CONNECT(samplerate48Btn, SIGNAL(released()), this, SLOT(sampleRateChanged()));

	samplerate96Btn = new AeroButton("96 kHz", this);
	samplerate96Btn->setRoundness(0);
	samplerate96Btn->setFixedSize (50, btn_height);
	samplerate96Btn->setStyleSheet(set->getMiniButtonStyle());
	samplerateBtnList.append(samplerate96Btn);
	CHECKED_CONNECT(samplerate96Btn, SIGNAL(released()), this, SLOT(sampleRateChanged()));

	samplerate192Btn = new AeroButton("192 kHz", this);
	samplerate192Btn->setRoundness(0);
	samplerate192Btn->setFixedSize (50, btn_height);
	samplerate192Btn->setStyleSheet(set->getMiniButtonStyle());
	samplerateBtnList.append(samplerate192Btn);
	CHECKED_CONNECT(samplerate192Btn, SIGNAL(released()), this, SLOT(sampleRateChanged()));

	samplerate384Btn = new AeroButton("384 kHz", this);
	samplerate384Btn->setRoundness(0);
	samplerate384Btn->setFixedSize (50, btn_height);
	samplerate384Btn->setStyleSheet(set->getMiniButtonStyle());
	samplerate384Btn->setEnabled(false);
	samplerateBtnList.append(samplerate384Btn);
	CHECKED_CONNECT(samplerate384Btn, SIGNAL(released()), this, SLOT(sampleRateChanged()));

	switch(set->getSampleRate()) {

		case 48000:
			samplerate48Btn->setBtnState(AeroButton::ON);
			samplerate96Btn->setBtnState(AeroButton::OFF);
			samplerate192Btn->setBtnState(AeroButton::OFF);
			samplerate384Btn->setBtnState(AeroButton::OFF);
			break;

		case 96000:
			samplerate48Btn->setBtnState(AeroButton::OFF);
			samplerate96Btn->setBtnState(AeroButton::ON);
			samplerate192Btn->setBtnState(AeroButton::OFF);
			samplerate384Btn->setBtnState(AeroButton::OFF);
			break;

		case 192000:
			samplerate48Btn->setBtnState(AeroButton::OFF);
			samplerate96Btn->setBtnState(AeroButton::OFF);
			samplerate192Btn->setBtnState(AeroButton::ON);
			samplerate384Btn->setBtnState(AeroButton::OFF);
			break;

		case 384000:
			samplerate48Btn->setBtnState(AeroButton::OFF);
			samplerate96Btn->setBtnState(AeroButton::OFF);
			samplerate192Btn->setBtnState(AeroButton::OFF);
			samplerate384Btn->setBtnState(AeroButton::ON);
			break;
	}

	QHBoxLayout *hbox = new QHBoxLayout();
	hbox->setSpacing(4);
	hbox->addStretch();
	hbox->addWidget(samplerate48Btn);
	hbox->addWidget(samplerate96Btn);
	hbox->addWidget(samplerate192Btn);
	hbox->addWidget(samplerate384Btn);

	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox);

	QGroupBox *groupBox = new QGroupBox(tr("Sample Rate"), this);
	groupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	groupBox->setLayout(vbox);
	groupBox->setStyleSheet(set->getWidgetStyle());
	groupBox->setFont(QFont("Arial", 8));

	return groupBox;
}

QGroupBox *HPSDRWidget::numberOfReceiversGroup() {

	m_receiverComboBox = new QComboBox(this);
	m_receiverComboBox->setStyleSheet(set->getComboBoxStyle());
	m_receiverComboBox->setMinimumContentsLength(4);

	QString str = "%1";
	for (int i = 0; i < MAX_RECEIVERS; i++)
		m_receiverComboBox->addItem(str.arg(i+1));

	CHECKED_CONNECT(
		m_receiverComboBox,
		SIGNAL(currentIndexChanged(int)),
		this,
		SLOT(setNumberOfReceivers(int)));

	m_receiversLabel = new QLabel("Receivers:", this);
    m_receiversLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_receiversLabel->setStyleSheet(set->getLabelStyle());
	

	QHBoxLayout *hbox1 = new QHBoxLayout();
	hbox1->setSpacing(5);
	//hbox1->addStretch();
	hbox1->addWidget(m_receiversLabel);
	hbox1->addStretch();
	hbox1->addWidget(m_receiverComboBox);
	
	QVBoxLayout *vbox = new QVBoxLayout();
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	
	QGroupBox *groupBox = new QGroupBox(tr("Number of Receivers"), this);
	groupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	groupBox->setLayout(vbox);
	groupBox->setStyleSheet(set->getWidgetStyle());
	groupBox->setFont(QFont("Arial", 8));

	return groupBox;
}

// ************************************************************************

void HPSDRWidget::setHPSDRHardware() {

	switch (m_hpsdrHardware) {

		case 0:
			// HPSDR modules
			modulesPresenceBtn->setBtnState(AeroButton::ON);
			hermesPresenceBtn->setBtnState(AeroButton::OFF);

			mercuryPresenceBtn->setEnabled(true);
			if (set->getMercuryPresence())
				mercuryPresenceBtn->setBtnState(AeroButton::ON);
			else
				mercuryPresenceBtn->setBtnState(AeroButton::OFF);

			penelopePresenceBtn->setEnabled(true);
			pennyPresenceBtn->setEnabled(true);

			if (set->getPenelopePresence()) {

				penelopePresenceBtn->setBtnState(AeroButton::ON);
				pennyPresenceBtn->setBtnState(AeroButton::OFF);
			}
			else if (set->getPennyLanePresence()) {

				penelopePresenceBtn->setBtnState(AeroButton::OFF);
				pennyPresenceBtn->setBtnState(AeroButton::ON);
			}
			else {

				penelopePresenceBtn->setBtnState(AeroButton::OFF);
				pennyPresenceBtn->setBtnState(AeroButton::OFF);
			}

			excaliburPresenceBtn->setEnabled(true);
			if (set->getExcaliburPresence()) {

				set->set10MhzSource(this, 3); // none
				mercuryBtn->setBtnState(AeroButton::OFF);
				atlasBtn->setBtnState(AeroButton::OFF);
				penelopeBtn->setBtnState(AeroButton::OFF);
				mercuryBtn->setEnabled(false);
				penelopeBtn->setEnabled(false);
				atlasBtn->setEnabled(false);

				excaliburPresenceBtn->setBtnState(AeroButton::ON);
			}
			else {

				switch(set->get10MHzSource()) {

					case 0:
						atlasBtn->setBtnState(AeroButton::ON);
						penelopeBtn->setBtnState(AeroButton::OFF);
						mercuryBtn->setBtnState(AeroButton::OFF);
						break;
			
					case 1:
						penelopeBtn->setBtnState(AeroButton::ON);
						atlasBtn->setBtnState(AeroButton::OFF);
						mercuryBtn->setBtnState(AeroButton::OFF);
						break;

					case 2:
						mercuryBtn->setBtnState(AeroButton::ON);
						atlasBtn->setBtnState(AeroButton::OFF);
						penelopeBtn->setBtnState(AeroButton::OFF);
						break;

					case 3:
						mercuryBtn->setBtnState(AeroButton::OFF);
						atlasBtn->setBtnState(AeroButton::OFF);
						penelopeBtn->setBtnState(AeroButton::OFF);
						break;
				}

				excaliburPresenceBtn->setBtnState(AeroButton::OFF);
			}

			break;

		case 1:
			
			// Hermes
			modulesPresenceBtn->setBtnState(AeroButton::OFF);
			hermesPresenceBtn->setBtnState(AeroButton::ON);
		
			penelopePresenceBtn->setBtnState(AeroButton::OFF);
			penelopePresenceBtn->setEnabled(false);

			pennyPresenceBtn->setBtnState(AeroButton::OFF);
			pennyPresenceBtn->setEnabled(false);

			mercuryPresenceBtn->setBtnState(AeroButton::OFF);
			mercuryPresenceBtn->setEnabled(false);

			excaliburPresenceBtn->setBtnState(AeroButton::OFF);
			excaliburPresenceBtn->setEnabled(false);

			set->set10MhzSource(this, 2); // none
			source10MhzExclusiveGroup->hide();
			source122_88MhzExclusiveGroup->hide();

			break;

		case 2:

			// Cyclops
			break;
	}

	if (set->getAlexPresence())
		alexPresenceBtn->setBtnState(AeroButton::ON);
}

void HPSDRWidget::systemStateChanged(
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

	//m_oldServerMode = m_serverMode;
	if (m_serverMode != mode) {

		if (mode == QSDR::ChirpWSPR)
			disableButtons();

		if (m_serverMode == QSDR::ChirpWSPR)
			enableButtons();
		
		m_serverMode = mode;
	}
		
	if (m_dataEngineState != state) {

		if (state == QSDR::DataEngineUp)
			disableButtons();
		else
			enableButtons();

		m_dataEngineState = state;
	}
	update();
}

void HPSDRWidget::hpsdrHardwareChanged() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btn = hardwareBtnList.indexOf(button);

	foreach(AeroButton *btn, hardwareBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	m_hpsdrHardware = btn;
	set->setHPSDRHardware(m_hpsdrHardware);

	setHPSDRHardware();

	switch (m_hpsdrHardware) {

		case 0:
			// HPSDR modules
			source10MhzExclusiveGroup->show();
			source122_88MhzExclusiveGroup->show();

			m_hwInterface = QSDR::Metis;

			emit messageEvent("[hpsdr]: HPSDR modules chosen.");
			break;

		case 1:
			// Hermes
			source10MhzExclusiveGroup->hide();
			source122_88MhzExclusiveGroup->hide();

			m_hwInterface = QSDR::Hermes;
			emit messageEvent("[hpsdr]: Hermes chosen.");
			break;

		case 2:
			break;
	}

	set->setSystemState(
					this,
					QSDR::NoError,
					m_hwInterface,
					m_serverMode,
					m_dataEngineState);
}

void HPSDRWidget::penelopePresenceChanged() {

	if (penelopePresenceBtn->btnState() == AeroButton::OFF) {
		
		if (pennyPresenceBtn->btnState() == AeroButton::ON) {

			set->setPennyLanePresence(false);
			pennyPresenceBtn->setBtnState(AeroButton::OFF);
			pennyPresenceBtn->update();

			emit messageEvent("[hpsdr]: PennyLane removed");
		}
		set->setPenelopePresence(true);
		penelopePresenceBtn->setBtnState(AeroButton::ON);

		emit messageEvent("[hpsdr]: Penelope added");

	} else {

		set->setPenelopePresence(false);
		penelopePresenceBtn->setBtnState(AeroButton::OFF);
		emit messageEvent("[hpsdr]: Penelope removed.");
	}
}

void HPSDRWidget::pennyPresenceChanged() {

	if (pennyPresenceBtn->btnState() == AeroButton::OFF) {
		
		if (penelopePresenceBtn->btnState() == AeroButton::ON) {

			set->setPenelopePresence(false);
			penelopePresenceBtn->setBtnState(AeroButton::OFF);
			penelopePresenceBtn->update();

			emit messageEvent("[hpsdr]: Penelope removed");
		}
		set->setPennyLanePresence(true);
		pennyPresenceBtn->setBtnState(AeroButton::ON);

		emit messageEvent("[hpsdr]: PennyLane added");

	} else {

		set->setPennyLanePresence(false);
		pennyPresenceBtn->setBtnState(AeroButton::OFF);
		emit messageEvent("[hpsdr]: PennyLane removed.");
	}
}

void HPSDRWidget::mercuryPresenceChanged() {

	if (mercuryPresenceBtn->btnState() == AeroButton::OFF) {
		
		set->setMercuryPresence(true);
		mercuryPresenceBtn->setBtnState(AeroButton::ON);
		emit messageEvent("[hpsdr]: Mercury added.");

	} else {

		set->setMercuryPresence(false);
		mercuryPresenceBtn->setBtnState(AeroButton::OFF);
		emit messageEvent("[hpsdr]: Mercury removed.");
	}
}

void HPSDRWidget::alexPresenceChanged() {

	if (alexPresenceBtn->btnState() == AeroButton::OFF) {
		
		set->setAlexPresence(true);
		alexPresenceBtn->setBtnState(AeroButton::ON);
		emit messageEvent("[hpsdr]: Alex added.");

	} else {

		set->setAlexPresence(false);
		alexPresenceBtn->setBtnState(AeroButton::OFF);
		emit messageEvent("[hpsdr]: Alex removed.");
	}
}

void HPSDRWidget::excaliburPresenceChanged() {

	if (excaliburPresenceBtn->btnState() == AeroButton::OFF) {
		
		set->set10MhzSource(this, 3); // None
		
		mercuryBtn->setBtnState(AeroButton::OFF);
		atlasBtn->setBtnState(AeroButton::OFF);
		penelopeBtn->setBtnState(AeroButton::OFF);
		
		mercuryBtn->setEnabled(false);
		penelopeBtn->setEnabled(false);
		atlasBtn->setEnabled(false);

		set->setExcaliburPresence(true);
		excaliburPresenceBtn->setBtnState(AeroButton::ON);
		emit messageEvent("[hpsdr]: Excalibur added.");

	} else {

		set->set10MhzSource(this, 2); // Mercury
		
		mercuryBtn->setEnabled(true);
		penelopeBtn->setEnabled(true);
		atlasBtn->setEnabled(true);

		mercuryBtn->setBtnState(AeroButton::ON);
		mercuryBtn->update();
		atlasBtn->setBtnState(AeroButton::OFF);
		penelopeBtn->setBtnState(AeroButton::OFF);

		set->setExcaliburPresence(false);
		excaliburPresenceBtn->setBtnState(AeroButton::OFF);
		emit messageEvent("[hpsdr]: Excalibur removed.");
	}
}

void HPSDRWidget::firmwareCheckChanged() {

	if (firmwareCheckBtn->btnState() == AeroButton::OFF) {

		set->setCheckFirmwareVersion(this, true);
		firmwareCheckBtn->setBtnState(AeroButton::ON);
		firmwareCheckBtn->setText("On");

	} else {

		set->setCheckFirmwareVersion(this, false);
		firmwareCheckBtn->setBtnState(AeroButton::OFF);
		firmwareCheckBtn->setText("Off");
	}
}

void HPSDRWidget::source10MhzChanged() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btn = source10MhzBtnList.indexOf(button);
	
	foreach(AeroButton *btn, source10MhzBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	set->set10MhzSource(this, btn);
	button->setBtnState(AeroButton::ON);
	button->update();

	QString msg = "[hpsdr]: 10 MHz source changed to %1";
	emit messageEvent(msg.arg(sources10Mhz.at(btn)));
}

void HPSDRWidget::source122_88MhzChanged() {
	
	switch (set->get122_8MHzSource()) {

		// penelope 0, mercury 1
		case 0:
			penelope2Btn->setBtnState(AeroButton::OFF);
			penelope2Btn->update();

			set->set122_88MhzSource(this, 1);
			emit messageEvent("[hpsdr]: 122.88 MHz source changed to Mercury.");
			mercury2Btn->setBtnState(AeroButton::ON);
			break;
			
		case 1:
			mercury2Btn->setBtnState(AeroButton::OFF);
			mercury2Btn->update();

			set->set122_88MhzSource(this, 0);
			emit messageEvent("[hpsdr]: 122.88 MHz source changed to Penelope.");
			penelope2Btn->setBtnState(AeroButton::ON);
			break;
	}
}

void HPSDRWidget::sampleRateChanged() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int btnHit = samplerateBtnList.indexOf(button);

	foreach(AeroButton *btn, samplerateBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (btnHit) {

		case 0:
			set->setSampleRate(this, 48000);
			HPSDR_WIDGET_DEBUG << "set sample rate to 48 kHz.";
			break;
			
		case 1:
			set->setSampleRate(this, 96000);
			HPSDR_WIDGET_DEBUG << "set sample rate to 96 kHz.";
			break;

		case 2:
			set->setSampleRate(this, 192000);
			HPSDR_WIDGET_DEBUG << "set sample rate to 192 kHz.";
			break;

		case 3:
			set->setSampleRate(this, 384000);
			HPSDR_WIDGET_DEBUG << "set sample rate to 384 kHz.";
			break;
	}
}

void HPSDRWidget::setNumberOfReceivers(int receivers) {

	m_numberOfReceivers = receivers+1;
	set->setReceivers(this, m_numberOfReceivers);
}

void HPSDRWidget::disableButtons() {

	modulesPresenceBtn->setEnabled(false);
	hermesPresenceBtn->setEnabled(false);

	penelopePresenceBtn->setEnabled(false);
	pennyPresenceBtn->setEnabled(false);
	mercuryPresenceBtn->setEnabled(false);
	excaliburPresenceBtn->setEnabled(false);
	alexPresenceBtn->setEnabled(false);
	atlasBtn->setEnabled(false);
	penelopeBtn->setEnabled(false);
	mercuryBtn->setEnabled(false);
	penelope2Btn->setEnabled(false);
	mercury2Btn->setEnabled(false);

	m_receiverComboBox->setEnabled(false);
}

void HPSDRWidget::enableButtons() {

	modulesPresenceBtn->setEnabled(true);
	hermesPresenceBtn->setEnabled(true);

	penelopePresenceBtn->setEnabled(true);
	pennyPresenceBtn->setEnabled(true);
	mercuryPresenceBtn->setEnabled(true);
	excaliburPresenceBtn->setEnabled(true);
	alexPresenceBtn->setEnabled(true);
	atlasBtn->setEnabled(true);
	penelopeBtn->setEnabled(true);
	mercuryBtn->setEnabled(true);
	penelope2Btn->setEnabled(true);
	mercury2Btn->setEnabled(true);

	m_receiverComboBox->setEnabled(true);
}
