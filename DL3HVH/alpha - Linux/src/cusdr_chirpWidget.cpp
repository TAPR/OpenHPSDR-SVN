/**
* @file  cusdr_chirpWidget.cpp
* @brief Chirp signal settings widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-05-21
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

#define LOG_CHIRP_WIDGET

//#include <QtGui>
//#include <QPen>
//#include <QDebug>
//#include <QScopedPointer>

#include "cusdr_chirpWidget.h"

#define	btn_height		15
#define	btn_width		80
#define	btn_width2		52

ChirpWidget::ChirpWidget(QWidget *parent) 
	: QWidget(parent)
	, set(Settings::instance())
	, m_serverMode(set->getCurrentServerMode())
	, m_hwInterface(set->getHWInterface())
	, m_dataEngineState(set->getDataEngineState())
	, m_timeScaleMax(2)
	, m_graphWidth(200)
	, m_graphHeight(100)
	, m_antialiased(true)
	, m_mouseOver(false)
	, m_showChirpFFT(false)
	, m_chirpUSB(false)
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(set->getMinimumGroupBoxWidth())
	, m_btnSpacing(5)
	, m_cnt(0)
{
	//setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 0, 4, 0);
	setMouseTracking(true);

	fonts = new CFonts(this);
	m_fonts = fonts->getFonts();

	m_stripes.resize(100);
	m_timeScale = m_graphWidth / m_timeScaleMax;
	stripeFreqRange();
    
	// ****************

	createChirpModeGroup();
	createPlayFileGroup();
	createChirpSignalGroup();
	createMatchedFilterControlGroup();
	
	set->setChirpReceiver(false);

	spectrumLabel = new QLabel(this);
	createChirpSpectrumGroup();
	
	//**************************************************************
	// set main layout
	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(0);
	hbox1->setMargin(0);
	hbox1->addStretch();
	hbox1->addWidget(chirpModeGroupBox);

	QHBoxLayout *hbox2 = new QHBoxLayout;
	hbox2->setSpacing(0);
	hbox2->setMargin(0);
	hbox2->addStretch();
	hbox2->addWidget(playFileGroupBox);

	QHBoxLayout *hbox3 = new QHBoxLayout;
	hbox3->setSpacing(0);
	hbox3->setMargin(0);
	hbox3->addStretch();
	hbox3->addWidget(chirpSignalGroupBox);

	QHBoxLayout *hbox4 = new QHBoxLayout;
	hbox4->setSpacing(0);
	hbox4->setMargin(0);
	hbox4->addStretch();
	hbox4->addWidget(matchedFilterControlGroupBox);
	

	QHBoxLayout *hbox5 = new QHBoxLayout;
	hbox5->setSpacing(0);
	hbox5->setMargin(0);
	hbox5->addStretch();
	hbox5->addWidget(chirpSpectrumGroupBox);

	mainLayout->addLayout(hbox1);
	mainLayout->addLayout(hbox2);
	mainLayout->addLayout(hbox3);
	mainLayout->addLayout(hbox4);
	mainLayout->addLayout(hbox5);
	mainLayout->addStretch();
		
	setLayout(mainLayout);

	setupConnections();
	initChirpDecoder();

	//createSpectrum();
}

ChirpWidget::~ChirpWidget() {

	// disconnect all signals
	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

QSize ChirpWidget::sizeHint() const {
	
	return QSize(m_minimumWidgetWidth, height());
}

QSize ChirpWidget::minimumSizeHint() const {

	return QSize(m_minimumWidgetWidth, height());
}

void ChirpWidget::setupConnections() {

	/*CHECKED_CONNECT(
		set,
		SIGNAL(audioBufferChanged(QObject *, qint64, qint64, const QByteArray)),
		this,
		SLOT(chirpBufferChanged(QObject *, qint64, qint64, const QByteArray)));*/

	CHECKED_CONNECT(
		set, 
		SIGNAL(lowerChirpFreqChanged(QObject *, int)), 
		this, 
		SLOT(setChirpLowerFrequency(QObject *, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(upperChirpFreqChanged(QObject *, int)), 
		this, 
		SLOT(setChirpUpperFrequency(QObject *, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(chirpBufferDurationUsChanged(QObject *, qint64)), 
		this, 
		SLOT(setChirpBufferDurationUs(QObject *, qint64)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(chirpRepetitionTimesChanged(QObject *, int)), 
		this, 
		SLOT(setChirpRepetitionTimes(QObject *, int)));

	/*CHECKED_CONNECT(
		set, 
		SIGNAL(chirpSpectrumChanged(qint64, qint64, const FrequencySpectrum &)),
        this, 
		SLOT(chirpSpectrumChanged(qint64, qint64, const FrequencySpectrum &)));*/

	CHECKED_CONNECT(
		set, 
		SIGNAL(chirpSpectrumListChanged(const QList<FrequencySpectrum> &)),
        this, 
		SLOT(chirpSpectrumListChanged(const QList<FrequencySpectrum> &)));
	
}

void ChirpWidget::createChirpModeGroup() {

	chirpModeBtn = new AeroButton("Decoding", this);
	chirpModeBtn->setRoundness(10);
	chirpModeBtn->setFixedSize(btn_width, btn_height);
	chirpModeBtn->setBtnState(AeroButton::ON);

	CHECKED_CONNECT(
		chirpModeBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(initChirpDecoder()));
	
	loadFileBtn = new AeroButton("Load File", this);
	loadFileBtn->setRoundness(10);
	loadFileBtn->setFixedSize(btn_width, btn_height);
	loadFileBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		loadFileBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(showFileDialog()));

	/*rcveBtn = new AeroButton("Receiver", this);
	rcveBtn->setRoundness(10);
	rcveBtn->setFixedSize(btn_width, btn_height);
	rcveBtn->setBtnState(AeroButton::OFF);*/


	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addWidget(chirpModeBtn);
	hbox1->addWidget(loadFileBtn);
	//hbox1->addStretch();

	/*QHBoxLayout *hbox2 = new QHBoxLayout;
	hbox2->setSpacing(4);
	hbox2->addStretch();
	hbox2->addWidget(rcveBtn);*/

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(4);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	//vbox->addLayout(hbox2);

	chirpModeGroupBox = new QGroupBox(tr("Chirp WSPR"), this);
	chirpModeGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	chirpModeGroupBox->setLayout(vbox);
	chirpModeGroupBox->setStyleSheet(set->getWidgetStyle());
	chirpModeGroupBox->setFont(QFont("Arial", 8));
}

void ChirpWidget::createPlayFileGroup() {

	pauseBtn = new AeroButton("Pause", this);
	pauseBtn->setRoundness(10);
	pauseBtn->setFixedSize(btn_width2, btn_height);
	pauseBtn->setBtnState(AeroButton::OFF);
	pauseBtn->setEnabled(false);
	CHECKED_CONNECT(
		pauseBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(suspend()));

	playBtn = new AeroButton("Play", this);
	playBtn->setRoundness(10);
	playBtn->setFixedSize(btn_width2, btn_height);
	playBtn->setBtnState(AeroButton::OFF);
	playBtn->setEnabled(false);
	CHECKED_CONNECT(
		playBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(startPlayback()));

	settingsBtn = new AeroButton("Settings", this);
	settingsBtn->setRoundness(10);
	settingsBtn->setFixedSize(btn_width2, btn_height);
	settingsBtn->setBtnState(AeroButton::OFF);
	settingsBtn->setEnabled(true);
	CHECKED_CONNECT(
		settingsBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(showSettingsDialog()));

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(m_btnSpacing);
	hbox1->addStretch();
	hbox1->addWidget(pauseBtn);
	hbox1->addWidget(playBtn);
	hbox1->addWidget(settingsBtn);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(m_btnSpacing);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);

	playFileGroupBox = new QGroupBox(tr("Sound File"), this);
	playFileGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	playFileGroupBox->setLayout(vbox);
	playFileGroupBox->setStyleSheet(set->getWidgetStyle());
	playFileGroupBox->setFont(QFont("Arial", 8));
}

void ChirpWidget::createChirpSignalGroup() {

	//QFontMetrics d_fm(m_titleFont);
	int fontMaxWidth = m_fonts.normalFontMetrics->boundingRect("00000").width();

	QLabel *minFLabel = new QLabel(tr("lo :"), this);
	minFLabel->setFont(m_fonts.normalFont);
	minFLabel->setStyleSheet(set->getLabelStyle());

	QLabel *maxFLabel = new QLabel(tr("hi :"), this);
	maxFLabel->setFont(m_fonts.normalFont);
	maxFLabel->setStyleSheet(set->getLabelStyle());

	QLabel *freqLabel = new QLabel(tr(" Hz"), this);
	freqLabel->setFont(m_fonts.normalFont);
	freqLabel->setStyleSheet(set->getLabelStyle());

	QLabel *timeLabel = new QLabel(tr("chirp length :"), this);
	timeLabel->setFont(m_fonts.normalFont);
	timeLabel->setStyleSheet(set->getLabelStyle());

	QLabel *msecLabel = new QLabel(tr(" ms"), this);
	msecLabel->setFont(m_fonts.normalFont);
	msecLabel->setStyleSheet(set->getLabelStyle());

	/*QLabel *repetitionLabel = new QLabel(tr("repetitions :"), this);
	repetitionLabel->setFont(m_titleFont);
	repetitionLabel->setStyleSheet(set->getLabelStyle());*/

	lowerFreqSpinBox = new QSpinBox(this);
	lowerFreqSpinBox->setStyleSheet(set->getSpinBoxStyle());
	lowerFreqSpinBox->setFixedWidth(fontMaxWidth + 30);
	lowerFreqSpinBox->setRange(0, 10000);
	lowerFreqSpinBox->setSingleStep(10);
	lowerFreqSpinBox->setValue(set->getLowerChirpFreq());

	CHECKED_CONNECT(
		lowerFreqSpinBox,
		SIGNAL(valueChanged(int)), 
		set, 
		SLOT(setLowerChirpFreq(int)));

	upperFreqSpinBox = new QSpinBox(this);
	upperFreqSpinBox->setStyleSheet(set->getSpinBoxStyle());
	upperFreqSpinBox->setFixedWidth(fontMaxWidth + 30);
	upperFreqSpinBox->setRange(50, 10000);
	upperFreqSpinBox->setSingleStep(10);
	upperFreqSpinBox->setValue(set->getUpperChirpFreq());

	CHECKED_CONNECT(
		upperFreqSpinBox,
		SIGNAL(valueChanged(int)), 
		set, 
		SLOT(setUpperChirpFreq(int)));

	timeSpinBox = new QSpinBox(this);
	timeSpinBox->setStyleSheet(set->getSpinBoxStyle());
	timeSpinBox->setFixedWidth(fontMaxWidth + 30);
	timeSpinBox->setRange(100, 10000);
	timeSpinBox->setSingleStep(1);
	timeSpinBox->setValue(set->getChirpBufferDurationUs()/1000);

	CHECKED_CONNECT(
		timeSpinBox,
		SIGNAL(valueChanged(int)), 
		set, 
		SLOT(setChirpBufferDurationUs(int)));

	CHECKED_CONNECT(
		timeSpinBox,
		SIGNAL(valueChanged(int)), 
		this,
		SLOT(stripeResize(int)));

	/*repetitionSpinBox = new QSpinBox(this);
	repetitionSpinBox->setStyleSheet(set->getSpinBoxStyle());
	repetitionSpinBox->setFixedWidth(fontMaxWidth + 30);
	repetitionSpinBox->setRange(1, 10);
	repetitionSpinBox->setSingleStep(1);
	repetitionSpinBox->setValue(set->getChirpRepetitionTimes());
	CHECKED_CONNECT(
		repetitionSpinBox,
		SIGNAL(valueChanged(int)), 
		set, 
		SLOT(setChirpRepetitionTimes(int)));*/

	playChirpBtn = new AeroButton("Play", this);
	playChirpBtn->setRoundness(10);
	playChirpBtn->setFixedSize(btn_width2, btn_height);
	playChirpBtn->setBtnState(AeroButton::OFF);
	CHECKED_CONNECT(
		playChirpBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(startPlayback()));

	/*chirpForwardBtn = new AeroButton("Forward", this);
	chirpForwardBtn->setRoundness(10);
	chirpForwardBtn->setFixedSize(btn_width2, btn_height);

	chirpBackwardBtn = new AeroButton("Backward", this);
	chirpBackwardBtn->setRoundness(10);
	chirpBackwardBtn->setFixedSize(btn_width2, btn_height);*/

	/*chirpLSBBtn = new AeroButton("LSB", this);
	chirpLSBBtn->setRoundness(10);
	chirpLSBBtn->setFixedSize(btn_width2, btn_height);
	chirpLSBBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		chirpLSBBtn,
		SIGNAL(clicked()), 
		this, 
		SLOT(switchSideband()));

	chirpUSBBtn = new AeroButton("USB", this);
	chirpUSBBtn->setRoundness(10);
	chirpUSBBtn->setFixedSize(btn_width2, btn_height);
	chirpUSBBtn->setBtnState(AeroButton::ON);

	CHECKED_CONNECT(
		chirpUSBBtn,
		SIGNAL(clicked()), 
		this, 
		SLOT(switchSideband()));*/

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(m_btnSpacing);
	hbox1->addStretch();
	hbox1->addWidget(minFLabel);
	hbox1->addWidget(lowerFreqSpinBox);
	hbox1->addSpacing(6);
	hbox1->addWidget(maxFLabel);
	hbox1->addWidget(upperFreqSpinBox);
	hbox1->addSpacing(1);
	hbox1->addWidget(freqLabel);
	//hbox1->addWidget(playChirpBtn);
	//hbox1->addStretch();

	QHBoxLayout *hbox2 = new QHBoxLayout;
	hbox2->setSpacing(m_btnSpacing);
	hbox2->addStretch();
	hbox2->addWidget(timeLabel);
	hbox2->addWidget(timeSpinBox);
	hbox2->addWidget(msecLabel);
	//hbox2->addStretch();

	/*QHBoxLayout *hbox3 = new QHBoxLayout;
	hbox3->setSpacing(m_btnSpacing);
	hbox3->addStretch();
	hbox3->addWidget(repetitionLabel);
	hbox3->addWidget(repetitionSpinBox);
	hbox3->addSpacing(d_fm.boundingRect("   ms").width());*/

	/*QHBoxLayout *hbox3 = new QHBoxLayout;
	hbox3->setSpacing(m_btnSpacing);
	hbox3->addStretch();
	hbox3->addWidget(chirpForwardBtn);
	hbox3->addWidget(chirpBackwardBtn);*/

	/*QHBoxLayout *hbox4 = new QHBoxLayout;
	hbox4->setSpacing(m_btnSpacing);
	hbox4->addStretch();
	hbox4->addWidget(chirpLSBBtn);
	hbox4->addWidget(chirpUSBBtn);*/
	
	QHBoxLayout *hbox5 = new QHBoxLayout;
	hbox5->setSpacing(m_btnSpacing);
	hbox5->addStretch();
	hbox5->addWidget(playChirpBtn);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(m_btnSpacing);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	vbox->addLayout(hbox2);
	vbox->addSpacing(6);
	//vbox->addLayout(hbox3);
	//vbox->addLayout(hbox4);
	vbox->addLayout(hbox5);

	chirpSignalGroupBox = new QGroupBox(tr("Chirp Signal"), this);
	chirpSignalGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	chirpSignalGroupBox->setLayout(vbox);
	chirpSignalGroupBox->setStyleSheet(set->getWidgetStyle());
	chirpSignalGroupBox->setFont(QFont("Arial", 8));
}

void ChirpWidget::createChirpSpectrumGroup() {
	
	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(m_btnSpacing);
	hbox1->addStretch();
	hbox1->addWidget(spectrumLabel);

	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(m_btnSpacing);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	
	chirpSpectrumGroupBox = new QGroupBox(tr("Chirp Signal Spectrum"), this);
	chirpSpectrumGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	chirpSpectrumGroupBox->setLayout(vbox);
	chirpSpectrumGroupBox->setStyleSheet(set->getWidgetStyle());
	chirpSpectrumGroupBox->setFont(QFont("Arial", 8));
}

void ChirpWidget::createMatchedFilterControlGroup() {

	//QFontMetrics d_fm(m_titleFont);
	int fontMaxWidth = m_fonts.normalFontMetrics->boundingRect("00000").width();

	/*avgBtn = new AeroButton("AVG", this);
	avgBtn->setRoundness(10);
	avgBtn->setFixedSize(btn_width2, btn_height);
	avgBtn->setBtnState(AeroButton::OFF);*/
	
	QLabel *avgLabel = new QLabel(tr("Avg:"), this);
	avgLabel->setFont(m_fonts.normalFont);
	avgLabel->setStyleSheet(set->getLabelStyle());

	QLabel *secLabel = new QLabel(tr("   s"), this);
	secLabel->setFont(m_fonts.normalFont);
	secLabel->setStyleSheet(set->getLabelStyle());

	avgSpinBox = new QSpinBox(this);
	avgSpinBox->setStyleSheet(set->getSpinBoxStyle());
	avgSpinBox->setFixedWidth(fontMaxWidth + 30);
	avgSpinBox->setRange(1, 120);
	avgSpinBox->setSingleStep(1);
	avgSpinBox->setValue(1);

	CHECKED_CONNECT(
		avgSpinBox,
		SIGNAL(valueChanged(int)), 
		set, 
		SLOT(setChirpAvgLength(int)));

	/*QLabel *filterLabel = new QLabel(tr("FIR Bandpass:"), this);
	filterLabel->setFont(m_titleFont);
	filterLabel->setStyleSheet(set->getLabelStyle());*/

	QLabel *loFilterLabel = new QLabel(tr("BPF   lo :"), this);
	loFilterLabel->setFont(m_fonts.normalFont);
	loFilterLabel->setStyleSheet(set->getLabelStyle());

	QLabel *upFilterLabel = new QLabel(tr("hi :"), this);
	upFilterLabel->setFont(m_fonts.normalFont);
	upFilterLabel->setStyleSheet(set->getLabelStyle());

	QLabel *freqFilterLabel = new QLabel(tr(" Hz"), this);
	freqFilterLabel->setFont(m_fonts.normalFont);
	freqFilterLabel->setStyleSheet(set->getLabelStyle());

	filterLowerFreqSpinBox = new QSpinBox(this);
	filterLowerFreqSpinBox->setStyleSheet(set->getSpinBoxStyle());
	filterLowerFreqSpinBox->setFixedWidth(fontMaxWidth + 30);
	filterLowerFreqSpinBox->setRange(0, 5000);
	filterLowerFreqSpinBox->setSingleStep(10);
	filterLowerFreqSpinBox->setValue(set->getChirpFilterLowerFrequency());

	CHECKED_CONNECT(
		filterLowerFreqSpinBox,
		SIGNAL(valueChanged(int)), 
		set, 
		SLOT(setChirpFilterLowerFrequency(int)));

	filterUpperFreqSpinBox = new QSpinBox(this);
	filterUpperFreqSpinBox->setStyleSheet(set->getSpinBoxStyle());
	filterUpperFreqSpinBox->setFixedWidth(fontMaxWidth + 30);
	filterUpperFreqSpinBox->setRange(50, 10000);
	filterUpperFreqSpinBox->setSingleStep(10);
	filterUpperFreqSpinBox->setValue(set->getChirpFilterUpperFrequency());

	CHECKED_CONNECT(
		filterUpperFreqSpinBox,
		SIGNAL(valueChanged(int)), 
		set, 
		SLOT(setChirpFilterUpperFrequency(int)));


	showChirpFFTBtn = new AeroButton("Show Rx spectrum", this);
	showChirpFFTBtn->setRoundness(10);
	showChirpFFTBtn->setFixedSize(100, btn_height);
	showChirpFFTBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		showChirpFFTBtn,
		SIGNAL(clicked()), 
		this, 
		SLOT(showChirpFFT()));

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(m_btnSpacing);
	hbox1->addStretch();
	hbox1->addWidget(avgLabel);
	hbox1->addWidget(avgSpinBox);
	hbox1->addSpacing(1);
	hbox1->addWidget(secLabel);

	/*QHBoxLayout *hbox2 = new QHBoxLayout;
	hbox2->setSpacing(m_btnSpacing);
	hbox2->addSpacing(6);
	hbox2->addWidget(filterLabel);*/
	//hbox2->addStretch();

	QHBoxLayout *hbox3 = new QHBoxLayout;
	hbox3->setSpacing(m_btnSpacing);
	hbox3->addStretch();
	hbox3->addWidget(loFilterLabel);
	hbox3->addWidget(filterLowerFreqSpinBox);
	hbox3->addSpacing(6);
	hbox3->addWidget(upFilterLabel);
	hbox3->addWidget(filterUpperFreqSpinBox);
	hbox3->addSpacing(1);
	hbox3->addWidget(freqFilterLabel);
	
	QHBoxLayout *hbox4 = new QHBoxLayout;
	hbox4->setSpacing(m_btnSpacing);
	hbox4->addStretch();
	hbox4->addWidget(showChirpFFTBtn);
	
	QVBoxLayout *vbox = new QVBoxLayout;
	vbox->setSpacing(m_btnSpacing);
	vbox->addSpacing(6);
	vbox->addLayout(hbox1);
	//vbox->addLayout(hbox2);
	vbox->addLayout(hbox3);
	vbox->addSpacing(6);
	vbox->addLayout(hbox4);
	
	matchedFilterControlGroupBox = new QGroupBox(tr("Matched Filter Control"), this);
	matchedFilterControlGroupBox->setMinimumWidth(m_minimumGroupBoxWidth);
	matchedFilterControlGroupBox->setLayout(vbox);
	matchedFilterControlGroupBox->setStyleSheet(set->getWidgetStyle());
	matchedFilterControlGroupBox->setFont(QFont("Arial", 8));
}

void ChirpWidget::showFileDialog() {
	
	if (chirpModeBtn->btnState() == AeroButton::ON) {

		chirpModeBtn->setBtnState(AeroButton::OFF);
		loadFileBtn->setBtnState(AeroButton::ON);
		chirpModeBtn->update();
	}

	const QString dir;
    const QStringList fileNames = QFileDialog::getOpenFileNames(this, tr("Open WAV file"), dir, "*.wav");

    if (fileNames.count()) {
		
		set->setSystemState(						
						this, 
						QSDR::NoError,
						//m_hwInterface,
						QSDR::NoInterfaceMode,
						QSDR::ChirpWSPRFile, 
						QSDR::DataEngineDown);
		emit messageEvent("[server]: switched to chirp decode file mode.");
        emit loadFileEvent(this, fileNames.front());
        //updateButtonStates();
		playBtn->setEnabled(true);
		pauseBtn->setEnabled(true);
    }
	else {

		playBtn->setEnabled(false);
		pauseBtn->setEnabled(false);
	}
}

void ChirpWidget::initChirpDecoder() {

	if (loadFileBtn->btnState() == AeroButton::ON) {

		chirpModeBtn->setBtnState(AeroButton::ON);
		loadFileBtn->setBtnState(AeroButton::OFF);

		playBtn->setEnabled(false);
		pauseBtn->setEnabled(false);
		loadFileBtn->update();

		set->setSystemState(						
						this, 
						QSDR::NoError,
						m_hwInterface, 
						QSDR::ChirpWSPR, 
						QSDR::DataEngineDown);
		emit messageEvent("[server]: switched to chirp decode mode.");
	}

	set->switchToChirpSignalMode(this);
}

void ChirpWidget::setReceiver() {

	if (rcveBtn->btnState() == AeroButton::OFF) {

		rcveBtn->setBtnState(AeroButton::ON);
		set->setChirpReceiver(true);
		
	}
	else
	if (rcveBtn->btnState() == AeroButton::ON) {

		rcveBtn->setBtnState(AeroButton::OFF);
		set->setChirpReceiver(false);
	}
}

void ChirpWidget::showChirpFFT() {

	if (m_showChirpFFT) {

		showChirpFFTBtn->setBtnState(AeroButton::OFF);
		set->setChirpFFTShow(false);
		m_showChirpFFT = false;
	}
	else {

		showChirpFFTBtn->setBtnState(AeroButton::ON);
		set->setChirpFFTShow(true);
		m_showChirpFFT = true;
	}

}

void ChirpWidget::switchSideband() {

	if (m_chirpUSB) {

		chirpLSBBtn->setBtnState(AeroButton::OFF);
		chirpUSBBtn->setBtnState(AeroButton::ON);
		chirpLSBBtn->update();
		chirpUSBBtn->update();
		set->setChirpUSB(true);
		m_chirpUSB = false;
	}
	else {

		chirpLSBBtn->setBtnState(AeroButton::ON);
		chirpUSBBtn->setBtnState(AeroButton::OFF);
		chirpLSBBtn->update();
		chirpUSBBtn->update();
		set->setChirpUSB(false);
		m_chirpUSB = true;
	}
}

//void ChirpWidget::chirpBufferChanged(QObject *sender, qint64 position, qint64 length, const QByteArray &buffer) {
//
//	Q_UNUSED (sender)
//
//    CHIRP_WIDGET_DEBUG	<< "audioBufferChanged"
//						<< "bufferPosition" << position
//						<< "bufferLength" << length;
//
//    m_chirpBufferPosition = position;
//    m_chirpBufferLength = length;
//	m_chirpBuffer = buffer;
//
//	
//}

void ChirpWidget::chirpSpectrumChanged(qint64 position, qint64 length, const FrequencySpectrum &spectrum) {
	
	CHIRP_WIDGET_DEBUG << "chirp spectrum changed!";

	m_chirpPosition = position;
	m_chirpLength = length;
	m_spectrum = spectrum;

	CHIRP_WIDGET_DEBUG << "pos =" << m_chirpPosition;

	//updateStripes();
}

void ChirpWidget::chirpSpectrumListChanged(const QList<FrequencySpectrum> &spectrumList) {

	//CHIRP_WIDGET_DEBUG << "chirp spectrumList changed!";

	m_spectrumList = spectrumList;
	updateStripes();
}

void ChirpWidget::resetSpectrum() {

    m_spectrum.reset();
    chirpSpectrumChanged(m_chirpPosition, m_chirpLength, m_spectrum);
}

void ChirpWidget::suspend() {

	emit suspendSignal(this);
}

void ChirpWidget::startPlayback() {

	emit startPlaybackSignal(this);
}

void ChirpWidget::showSettingsDialog() {

	emit showSettingsDialogSignal(this);
}

void ChirpWidget::setChirpLowerFrequency(QObject* sender, int lo) {

	Q_UNUSED(sender)

	if (lo >= upperFreqSpinBox->value())
		lowerFreqSpinBox->setValue(upperFreqSpinBox->value());
}

void ChirpWidget::setChirpUpperFrequency(QObject* sender, int hi) {

	Q_UNUSED(sender)

	if (hi <= lowerFreqSpinBox->value())
		upperFreqSpinBox->setValue(lowerFreqSpinBox->value());
}

void ChirpWidget::setChirpAmplitude(QObject* sender, qreal amp) {

	Q_UNUSED(sender)
	Q_UNUSED(amp)
	
}

void ChirpWidget::setChirpBufferDurationUs(QObject* sender, qint64 value) {

	Q_UNUSED(sender)
	Q_UNUSED(value)
}

void ChirpWidget::setChirpRepetitionTimes(QObject* sender, int value) {

	Q_UNUSED(sender)
	Q_UNUSED(value)
}

void ChirpWidget::setFilterLowerFrequency(QObject* sender, int value) {

	Q_UNUSED(sender)
	Q_UNUSED(value)
}

void ChirpWidget::setFilterUpperFrequency(QObject* sender, int value) {

	Q_UNUSED(sender)
	Q_UNUSED(value)
}

int ChirpWidget::stripeIndex(qreal frequency) const {

    Q_ASSERT(frequency >= m_lowFreq && frequency < m_highFreq);

    const qreal bandWidth = (m_highFreq - m_lowFreq) / m_stripes.count();
    const int index = (frequency - m_lowFreq) / bandWidth;
    
	if(index < 0 || index >= m_stripes.count())  Q_ASSERT(false);

    return index;
}

QPair<qreal, qreal> ChirpWidget::stripeRange(int index) const {

    Q_ASSERT(index >= 0 && index < m_stripes.count());

    const qreal bandWidth = (m_highFreq - m_lowFreq) / m_stripes.count();

    return QPair<qreal, qreal>(index * bandWidth, (index+1) * bandWidth);
}

void ChirpWidget::stripeFreqRange() {

	//int lo = set->getLowerChirpFreq();
	int hi = set->getUpperChirpFreq();

	//int unit = qRound((hi - lo) / 8);
	//m_lowFreq = 100;
	//m_highFreq = hi + unit;
	m_lowFreq = 0;

	if (hi < 5000)
		m_highFreq = 5000;
	else
		m_highFreq = 10000;
}

void ChirpWidget::updateStripes() {

	if (m_stripeList.count() > 0)
		m_stripeList.clear();

	stripeFreqRange();
	foreach(FrequencySpectrum spectrum, m_spectrumList) {
		
		m_stripes.fill(Stripe());
		FrequencySpectrum::const_iterator i = spectrum.begin();
		const FrequencySpectrum::const_iterator end = spectrum.end();
		
		for ( ; i != end; ++i) {
			
			const FrequencySpectrum::Element e = *i;
			if (e.frequency >= m_lowFreq && e.frequency < m_highFreq) {
				
				Stripe &stripe = m_stripes[stripeIndex(e.frequency)];
				stripe.value = qMax(stripe.value, e.amplitude);
				stripe.clipped |= e.clipped;
			}
		}
		m_stripeList.append(m_stripes);
	}

    createSpectrum();
}

void ChirpWidget::stripeResize(int scale) {

	qreal s = scale/1000.0;
	if (s <= 2.0)
		m_timeScaleMax = 2;
	else
	if (s <= 5.0)
		m_timeScaleMax = 5;
	else
		m_timeScaleMax = 10;

	m_timeScale = s * m_graphWidth / m_timeScaleMax;

	updateStripes();
}

//void ChirpWidget::selectStripe(int index) {
//    const QPair<qreal, qreal> frequencyRange = stripeRange(index);
//    const QString message = QString("%1 - %2 Hz")
//                                .arg(frequencyRange.first)
//                                .arg(frequencyRange.second);
//    emit infoMessage(message, StripeSelectionInterval);
//
//    if (NullTimerId != m_timerId)
//        killTimer(m_timerId);
//    m_timerId = startTimer(StripeSelectionInterval);
//
//    m_stripeSelected = index;
//    update();
//}

void ChirpWidget::createSpectrum() {

	m_resultImage = QImage(220, 150, QImage::Format_ARGB32_Premultiplied);

	QPainter painter(&m_resultImage);
	painter.setCompositionMode(QPainter::CompositionMode_Source);
	painter.fillRect(m_resultImage.rect(), Qt::transparent);
	painter.setRenderHints(QPainter::SmoothPixmapTransform | QPainter::TextAntialiasing, true);
	
	QRect rect(0, 0, m_graphWidth + 20, m_graphHeight + 20);
	QImage* stripeImage;
	stripeImage = createSpectrumImage(rect);

	if (stripeImage) {
		
		painter.drawImage(0, 0, *stripeImage);
		delete stripeImage;
	}

	painter.end();

	spectrumLabel->setPixmap(QPixmap::fromImage(m_resultImage));
}

QImage* ChirpWidget::createSpectrumImage(const QRect& rect) {

	QImage *image = new QImage(rect.size(), QImage::Format_ARGB32_Premultiplied);
	if (!image) return NULL;

	QPainter painter(image);
	painter.setFont(m_fonts.smallFont);
	//painter.fillRect(image->rect(), Qt::transparent);
	painter.fillRect(image->rect(), Qt::black);

	QFontMetrics sfm(m_fonts.smallFont);
	m_fontHeight = sfm.height();
	m_maxFontWidth = sfm.width("00000 Hz");

	int s = 0;
	int length = m_stripeList.count();
	
	if (length > 0)
		//m_scale = (qreal)(1.0f * m_graphWidth / length);
		m_scale = m_timeScale / length;
	else
		m_scale = 0;

	//CHIRP_WIDGET_DEBUG << "m_timeScale = " << m_timeScale;
	
	foreach(QVector<Stripe> stripe, m_stripeList) {

		int slength = stripe.count();
		for (int i = 0; i < slength; i++) {
			
			qreal v = stripe[i].value;// + 0.2f;
			
			//QColor color = QColor((int)(v * 55), (int)(v * 225), (int)(v * 255));
			//QColor color = QColor((int)(v * 255), (int)(v * 55), (int)(v * 255));
			QColor color = QColor((int)(v * 255), (int)(v * 55), (int)(v * 255));
			painter.setPen(QPen(color, 1));
			
			int x1 = s * m_scale + 15;
			int x2 = (s+1) * m_scale + 15;
			int y  = m_graphWidth/2 - (i+1);
			
			//if (x2 < m_graphWidth + 5)
				painter.drawLine(x1, y, x2, y);
		}
		s++;
	}

	QPen pen(QColor(45, 122, 148, 150), 1);
	painter.setPen(pen);
	
	painter.drawRect(15, 0, m_graphWidth-1, m_graphHeight-1);
	//painter.drawRect(0, 0, 10, m_graphHeight+9);

	pen.setColor(QColor(85, 162, 188, 255));
	painter.setPen(pen);
	painter.rotate(-90);

	int scaleTextWidth = sfm.width("Frequency (kHz)");
	painter.drawText(- (scaleTextWidth + m_graphHeight) / 2, m_fontHeight - 5, "Frequency (kHz)");
	
	painter.rotate(90);
	scaleTextWidth = sfm.width("Time (sec)");
	painter.drawText(15 + (m_graphWidth - scaleTextWidth) / 2, 10 + m_graphHeight, "Time (sec)");
	painter.drawText(10, 10 + m_graphHeight, "0.0");

	qreal t = 1.0f * m_timeScaleMax;
	QString str = QString::number(t, 'f', 1);
	painter.drawText(5 + m_graphWidth, 10 + m_graphHeight, str);

	//pen.setColor(QColor(225, 225, 225, 255));
	painter.setPen(pen);
	
	painter.drawText(
		18, 32 - m_fontHeight, 
		m_maxFontWidth, m_fontHeight, 
		Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, 
		"3.75");

	painter.drawText(
		18, 57 - m_fontHeight, 
		m_maxFontWidth, m_fontHeight, 
		Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, 
		"2.50");

	painter.drawText(
		18, 82 - m_fontHeight, 
		m_maxFontWidth, m_fontHeight, 
		Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, 
		"1.25");

	pen.setStyle(Qt::DashLine);
	pen.setColor(QColor(45, 122, 148, 110));
	painter.setPen(pen);

	painter.drawLine(42, 25, m_graphWidth + 15, 25);
	painter.drawLine(42, 50, m_graphWidth + 15, 50);
	painter.drawLine(42, 75, m_graphWidth + 15, 75);
	
	painter.drawLine(65, 0, 65, m_graphHeight);
	painter.drawLine(115, 0, 115, m_graphHeight);
	painter.drawLine(165, 0, 165, m_graphHeight);
	//painter.drawLine(0, m_graphHeight, m_graphWidth, m_graphHeight);	// bottom
	
	
	painter.end();

	return image;
}

void ChirpWidget::resizeEvent(QResizeEvent *event) {

	//Q_UNUSED(event);
	//QResizeEvent::oldSize()
	
	//QTimer::singleShot(10, this, SLOT(getRegion()));
	//m_resizeFrame = true;
	
	QWidget::resizeEvent(event);
}
