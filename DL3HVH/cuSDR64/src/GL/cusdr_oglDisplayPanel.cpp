/**
* @file  cusdr_oglDisplayPanel.cpp
* @brief Display panel class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-02-22
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
//#define LOG_DISPLAYPANEL

#include "cusdr_oglDisplayPanel.h"

//#include <QtGui>
//#include <QDebug>
//#include <QFileInfo>
//#include <QTimer>
//#include <QImage>
//#include <QString>
//#include <QGLFramebufferObject>

#ifndef GL_MULTISAMPLE
#define GL_MULTISAMPLE  0x809D
#endif


OGLDisplayPanel::OGLDisplayPanel(QWidget *parent)
	: QGLWidget(QGLFormat(QGL::SampleBuffers), parent)

	, set(Settings::instance())
	, m_serverMode(set->getCurrentServerMode())
	, m_hwInterface(set->getHWInterface())
	, m_dataEngineState(QSDR::DataEngineDown)
	, m_smeterUpdate(true)
	, m_smeterRenew(true)
	, m_sMeterAvg(true)
	, m_oldFreq(0)
	, m_height(120)//(120)
	, m_sMeterWidth(300)
	, m_rxRectWidth(500)
	, m_lowerRectY(12)
	, m_upperRectY(1)
	, m_syncStatus(0)
	, m_adcStatus(0)
	, m_packetLossStatus(0)
	, m_sendIQStatus(0)
	, m_recvAudioStatus(0)
	, m_receivers(set->getNumberOfReceivers())
	, m_sample_rate(set->getSampleRate()/1000)
	, m_dither(set->getMercuryDither())
	, m_random(set->getMercuryRandom())
	, m_currentReceiver(set->getCurrentReceiver())
	, m_sMeterDeform(15)
	, m_freqDigitsPosY(30)
	, m_sMeterPosY(45)
	, m_sMeterHoldTime(set->getSMeterHoldTime())
	, m_sMeterPrevHoldTimeMax(0)
	, m_sMeterPrevHoldTimeMin(0)
	, m_sMeterMeanValueCnt(0)
	, m_mouseWheelFreqStep(set->getMouseWheelFreqStep(m_currentReceiver))
	, m_dBmPanMin(-130.0f)
	, m_dBmPanMax(10.0f)
	, m_unit(1.0f)
	, m_smeterVertices(256.0f)
	, m_sMeterValue((float)(-156*ONEPI/256.0f))
	, m_sMeterMeanValue(0.0f)
	, m_sMeterMaxValueA((float)(-ONEPI/2.0f))
	, m_sMeterMinValueA((float)(ONEPI/2.0f))
	, m_sMeterMaxValueB(-1000.0f)
	, m_sMeterMinValueB(1000.0f)
{
	//QGL::setPreferredPaintEngine(QPaintEngine::OpenGL);

	setSizePolicy(QSizePolicy::Expanding, QSizePolicy::Fixed);

	setAutoBufferSwap(true);
	setAutoFillBackground(false);
	setMouseTracking(true);

	setupDisplayRegions(size());

	fonts = new CFonts(this);
	m_fonts = fonts->getFonts();

	m_oglTextTiny = new OGLText(m_fonts.tinyFont);
	m_oglTextSmall = new OGLText(m_fonts.smallFont);

	m_fonts.smallFont.setItalic(true);
	m_oglTextSmallItalic = new OGLText(m_fonts.smallFont);
	m_oglTextNormal = new OGLText(m_fonts.normalFont);
	m_oglTextBig = new OGLText(m_fonts.bigFont1);

	m_fonts.bigFont1.setItalic(true);
	m_oglTextBigItalic = new OGLText(m_fonts.bigFont1);
	m_oglTextFreq1 = new OGLText(m_fonts.freqFont1);
	m_oglTextFreq2 = new OGLText(m_fonts.freqFont2);
	m_oglTextImpact = new OGLText(m_fonts.impactFont);

	setupConnections();
	setupTextstrings();

	set10mhzSource(this, set->get10MHzSource());
	set122_88mhzSource(this, set->get122_8MHzSource());

	QList<long> fList = set->getFrequencies();
	
	for (int i = 0; i < MAX_RECEIVERS; i++) {

		TFrequency f;
		f.freqMHz = (int)(fList.at(i) / 1000);
		f.freqkHz = (int)(fList.at(i) % 1000);
		
		m_frequencyList << f;
	}

	QList<THamBandFrequencies> bandList = getHamBandFrequencies();
	HamBand band = getBandFromFrequency(bandList, fList.at(0));

	m_mercuryAttenuator = set->getMercuryAttenuators(0).at(band);


	m_smeterFBO = 0;

	m_digitColor = QColor(68, 68, 68);
	m_bkgColor1 = QColor(30, 30, 30);
	m_bkgColor2 = QColor(50, 50, 50);
	m_activeTextColor = QColor(166, 196, 208);
	m_inactiveTextColor = QColor(68, 68, 68);//Qt::white;//
	m_textBackgroundColor = QColor(66, 96, 208);

	m_sMeterTimer.start();
	m_sMeterMaxTimer.start();
	m_sMeterMinTimer.start();
	m_sMeterDisplayTime.start();
	
	m_SMeterA = false;
	//m_SMeterA = true;
}

OGLDisplayPanel::~OGLDisplayPanel() {
}

QSize OGLDisplayPanel::minimumSizeHint() const {

	return QSize(width(), m_height);
}

QSize OGLDisplayPanel::sizeHint() const {
	
	//return QSize(width(), height());
	return QSize(width(), m_height);
}

void OGLDisplayPanel::setupConnections() {

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
		SIGNAL(frequencyChanged(QObject *, bool, int, long)),
		this,
		SLOT(setFrequency(QObject *, bool, int, long)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(numberOfRXChanged(QObject *, int)), 
		this, 
		SLOT(setReceivers(QObject *, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(currentReceiverChanged(QObject *, int)),
		this, 
		SLOT(setCurrentReceiver(QObject *, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(mercuryAttenuatorChanged(QObject *, HamBand, int)),
		this, 
		SLOT(setMercuryAttenuator(QObject *, HamBand, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(ditherChanged(QObject *, int)), 
		this, 
		SLOT(setDither(QObject *, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(randomChanged(QObject *, int)), 
		this, 
		SLOT(setRandom(QObject *, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(sampleRateChanged(QObject *, int)), 
		this, 
		SLOT(setSampleRate(QObject *, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(mercuryPresenceChanged(bool)), 
		this, 
		SLOT(setMercuryPresence(bool)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(mercuryVersionChanged(int)), 
		this, 
		SLOT(setMercuryVersion(int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(penelopePresenceChanged(bool)), 
		this, 
		SLOT(setPenelopePresence(bool)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(pennyLanePresenceChanged(bool)),
		this,
		SLOT(setPennylanePresence(bool)));

	CHECKED_CONNECT(
		set,
		SIGNAL(metisVersionChanged(int)), 
		this, 
		SLOT(setMetisVersion(int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(penelopeVersionChanged(int)), 
		this, 
		SLOT(setPenelopeVersion(int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(penelopeVersionChanged(int)),
		this,
		SLOT(setPennylaneVersion(int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(hermesVersionChanged(int)), 
		this, 
		SLOT(setHermesVersion(int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(alexPresenceChanged(bool)), 
		this, 
		SLOT(setAlexPresence(bool)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(excaliburPresenceChanged(bool)), 
		this, 
		SLOT(setExcaliburPresence(bool)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(src10MhzChanged(QObject *, int)), 
		this, 
		SLOT(set10mhzSource(QObject *, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(src122_88MhzChanged(QObject *, int)), 
		this, 
		SLOT(set122_88mhzSource(QObject *, int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(protocolSyncChanged(int)),
		this,
		SLOT(setSyncStatus(int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(adcOverflowChanged(int)),
		this,
		SLOT(setADCStatus(int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(packetLossChanged(int)),
		this,
		SLOT(setPacketLossStatus(int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(sendIQSignalChanged(int)),
		this,
		SLOT(setSendIQStatus(int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(rcveIQSignalChanged(int)),
		this,
		SLOT(setRecvAudioStatus(int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(mouseWheelFreqStepChanged(QObject*, int, qreal)),
		this, 
		SLOT(setMouseWheelFreqStep(QObject*, int, qreal)));

	CHECKED_CONNECT(
		set,
		SIGNAL(sMeterValueChanged(int, float)),
		this,
		SLOT(setSMeterValue(int, float)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(sMeterHoldTimeChanged(int)), 
		this, 
		SLOT(setSMeterHoldTime(int)));
}

void OGLDisplayPanel::setupTextstrings() {

	m_SYNCString = QString("SYNC");
	m_syncWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(m_SYNCString).width();

	m_ADCString = QString("ADC");
	m_adcWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(m_ADCString).width();

	m_PacketLossString = QString("IP Packets");
	m_packetLossWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(m_PacketLossString).width();

	m_sendIQString = QString("send IQ");
	m_sendIQWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(m_sendIQString).width();

	m_recvAudioString = QString("recv Audio");
	m_recvAudioWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(m_recvAudioString).width();

	m_AttnString = QString("Attn:");
	m_AttnWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(m_AttnString).width();

	m_ditherString = QString("Dither");
	m_ditherWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(m_ditherString).width();

	m_randomString = QString("Random");
	m_randomWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(m_randomString).width();

	m_sampleRateString = QString("SampleRate:");
	m_sampleRateWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(m_sampleRateString).width();

	m_modusString = QString("Modus:");
	m_modusWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(m_modusString).width();

	m_10MHzString = QString("10 MHz:");
	m_10MHzWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(m_10MHzString).width();

	m_12288MHzString = QString("122.88 MHz:");
	m_12288MHzWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(m_12288MHzString).width();

	m_mercuryString = QString("Mercury ");
	m_pennyString = QString("Penny[Lane] ");
	m_penelopeString = QString("Penelope ");
	m_pennylaneString = QString("Pennylane ");
	m_excaliburString = QString("Excalibur ");
	m_metisString = QString("Metis ");
	m_alexString = QString("Alex ");
	m_hermesString = QString("Hermes ");

}

void OGLDisplayPanel::initializeGL() {

	if (!isValid()) return;
	
	//*****************************************************************
	// default initialization

	//glShadeModel(GL_FLAT);
	glShadeModel(GL_SMOOTH);
	glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
	//glPixelStorei(GL_UNPACK_ALIGNMENT, 4); // 4-byte pixel alignment
	//glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);
    //glHint(GL_POLYGON_SMOOTH_HINT, GL_NICEST);
	glDisable(GL_LINE_SMOOTH);
	glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);
	glDisable(GL_POINT_SMOOTH);
	glHint(GL_POINT_SMOOTH_HINT, GL_NICEST);
	glEnable(GL_POLYGON_SMOOTH);
	glHint(GL_POLYGON_SMOOTH_HINT, GL_NICEST);

	//glDepthFunc(GL_LESS);
    glEnable(GL_DEPTH_TEST);
	glDisable(GL_CULL_FACE);
	glEnable(GL_MULTISAMPLE);
}

void OGLDisplayPanel::resizeGL(int iWidth, int iHeight) {

	int width = (int)(iWidth/2) * 2;
	int height = iHeight;

	glFinish();

	//m_resizeTime.restart();
	setupDisplayRegions(QSize(width, height));
    glViewport(0, 0, (GLsizei)width, (GLsizei)height);

	setProjectionOrthographic(width, height);
}

void OGLDisplayPanel::paintGL() {

	glClearColor(0, 0, 0, 1.0);
	glClear(GL_DEPTH_BUFFER_BIT);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);

	//m_mutex.lock();
	paintUpperRegion();
	paintRxRegion();
	paintLowerRegion();
	paintSMeter();
	//m_mutex.unlock();
}

void OGLDisplayPanel::paintUpperRegion() {

	QString str;

	GLint x1 = m_rxRect.left();
	GLint y1 = m_rxRect.top();

	//QFontMetrics d_fm(m_smallFont);
	//QFontMetrics d_fm(m_fonts.smallFont);
	//int fontHeight = d_fm.tightBoundingRect("M").height();
	
	// sync status
	str = QString(m_SYNCString);
	//QRect rect = QRect(x1+2, y1 + m_upperRectY, d_fm.tightBoundingRect(str).width() + 5, fontHeight + 4);
	QRect rect = QRect(x1+2, y1 + m_upperRectY, m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 5, m_fonts.fontHeightSmallFont + 4);

	switch (m_syncStatus) {

		case 0:
			drawGLRect(rect, QColor(68, 68, 68), -2.0f);
			//qglColor(QColor(100, 100, 100));
			break;

		case 1:
			drawGLRect(rect, QColor(56, 242, 115), -2.0f);
			//qglColor(QColor(56, 242, 115));
			break;

		case 2:
			drawGLRect(rect, QColor(242, 56, 109), -2.0f);
			//qglColor(QColor(242, 56, 109));
			break;
	}
	qglColor(QColor(0, 0, 0));
	m_oglTextSmallItalic->renderText(x1 + 3, y1 + m_upperRectY, m_SYNCString);

	// ADC status
	str = QString(m_ADCString);
	//rect = QRect(x1+36, y1 + m_upperRectY, d_fm.tightBoundingRect(str).width() + 5, fontHeight + 4);
	rect = QRect(x1+36, y1 + m_upperRectY, m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 5, m_fonts.fontHeightSmallFont + 4);

	switch (m_adcStatus) {

		case 0:
			drawGLRect(rect, QColor(68, 68, 68), -2.0f);
			//qglColor(QColor(100, 100, 100));
			break;

		case 1:
			drawGLRect(rect, QColor(56, 242, 115), -2.0f);
			//qglColor(QColor(56, 242, 115));
			break;

		case 2:
			drawGLRect(rect, QColor(242, 56, 109), -2.0f);
			//qglColor(QColor(242, 56, 109));
			break;
	}
	qglColor(QColor(0, 0, 0));
	m_oglTextSmallItalic->renderText(m_syncWidth + 11, y1 + m_upperRectY, m_ADCString);

	// Packet loss status
	str = QString(m_PacketLossString);
	//rect = QRect(x1+63, y1 + m_upperRectY, d_fm.tightBoundingRect(str).width() + 5, fontHeight + 4);
	rect = QRect(x1+63, y1 + m_upperRectY,  m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 5, m_fonts.fontHeightSmallFont + 4);

	switch (m_packetLossStatus) {

		case 0:
			drawGLRect(rect, QColor(68, 68, 68), -2.0f);
			//qglColor(QColor(100, 100, 100));
			break;

		case 1:
			drawGLRect(rect, QColor(56, 242, 115), -2.0f);
			//qglColor(QColor(56, 242, 115));
			break;

		case 2:
			drawGLRect(rect, QColor(242, 56, 109), -2.0f);
			//qglColor(QColor(242, 56, 109));
			break;
	}
	qglColor(QColor(0, 0, 0));
	m_oglTextSmallItalic->renderText(m_syncWidth + m_adcWidth + 18, y1 + m_upperRectY, m_PacketLossString);

	// send IQ data status
	//str = QString(m_sendIQString);
	//rect = QRect(x1+63, y1 + m_upperRectY, d_fm.tightBoundingRect(str).width() + 6, fontHeight + 4);

	//switch (m_sendIQStatus) {

	//	case 0:
	//		drawGLRect(rect, QColor(68, 68, 68), -2.0f);
	//		//qglColor(QColor(100, 100, 100));
	//		break;

	//	case 1:
	//		drawGLRect(rect, QColor(56, 242, 115), -2.0f);
	//		//qglColor(QColor(56, 242, 115));
	//		break;

	//	case 2:
	//		drawGLRect(rect, QColor(242, 56, 109), -2.0f);
	//		//qglColor(QColor(242, 56, 109));
	//		break;
	//}
	//qglColor(QColor(0, 0, 0));
	//m_oglTextSmallItalic->renderText(m_syncWidth + m_adcWidth + 19, y1 + m_upperRectY, m_sendIQString);

	// receiver Audio status
	//str = QString(m_recvAudioString);
	//rect = QRect(x1+108, y1 + m_upperRectY, d_fm.tightBoundingRect(str).width() + 8, fontHeight + 4);

	//switch (m_recvAudioStatus) {

	//	case 0:
	//		drawGLRect(rect, QColor(68, 68, 68), -2.0f);
	//		//qglColor(QColor(100, 100, 100));
	//		break;

	//	case 1:
	//		drawGLRect(rect, QColor(56, 242, 115), -2.0f);
	//		//qglColor(QColor(56, 242, 115));
	//		break;

	//	case 2:
	//		drawGLRect(rect, QColor(242, 56, 109), -2.0f);
	//		//qglColor(QColor(242, 56, 109));
	//		break;
	//}
	//qglColor(QColor(0, 0, 0));
	//m_oglTextSmallItalic->renderText(m_syncWidth + m_adcWidth + m_sendIQWidth + 29, y1 + m_upperRectY, m_recvAudioString);

	//int d = m_syncWidth + m_adcWidth + m_sendIQWidth + m_recvAudioWidth + 40;
	int d = m_syncWidth + m_adcWidth + m_packetLossWidth + 40;

	// Metis status
	str = m_metisString;
	//rect = QRect(d-2, y1 + m_upperRectY, d_fm.tightBoundingRect(str).width() + 22, fontHeight + 4);
	rect = QRect(d-2, y1 + m_upperRectY, m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 22, m_fonts.fontHeightSmallFont + 4);
	
	if (m_hwInterface == QSDR::Metis) {

		drawGLRect(rect, m_textBackgroundColor, -2.0f);
		if (m_dataEngineState == QSDR::DataEngineUp) {

			str.append(m_metisVersion);
			qglColor(QColor(206, 236, 248));
		}
		else
			qglColor(QColor(0, 0, 0));
	}
	else {

		drawGLRect(rect, QColor(68, 68, 68), -2.0f);
		qglColor(QColor(0, 0, 0));
	}
		
	m_oglTextSmallItalic->renderText(d, y1 + m_upperRectY, 1.0f, str);

	// Mercury status
	str = m_mercuryString;
	rect = QRect(d+50, y1 + m_upperRectY, m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 22, m_fonts.fontHeightSmallFont + 4);

	if (set->getMercuryPresence() && m_hwInterface == QSDR::Metis) {

		drawGLRect(rect, m_textBackgroundColor, -2.0f);
		if (m_dataEngineState == QSDR::DataEngineUp) {

			str.append(m_mercuryVersion);
			qglColor(QColor(206, 236, 248));
		}
		else
			qglColor(QColor(0, 0, 0));
	}
	else {

		drawGLRect(rect, QColor(68, 68, 68), -2.0f);
		qglColor(QColor(0, 0, 0));
	}

	m_oglTextSmallItalic->renderText(d+52, y1 + m_upperRectY, 1.0f, str);

	// Penelope status
	str = m_penelopeString;
	rect = QRect(d+116, y1 + m_upperRectY, m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 22, m_fonts.fontHeightSmallFont + 4);

	if (set->getPenelopePresence() && m_hwInterface == QSDR::Metis) {

		str = m_penelopeString;
		drawGLRect(rect, m_textBackgroundColor, -2.0f);
		if (m_dataEngineState == QSDR::DataEngineUp) {

			str.append(m_penelopeVersion);
			qglColor(QColor(206, 236, 248));
		}
		else {

			//str = m_pennyString;
			qglColor(QColor(0, 0, 0));
		}
	}
	else if (set->getPennyLanePresence() && m_hwInterface == QSDR::Metis) {

		str = m_pennylaneString;
		drawGLRect(rect, m_textBackgroundColor, -2.0f);
		if (m_dataEngineState == QSDR::DataEngineUp) {

			str.append(m_pennylaneVersion);
			qglColor(QColor(206, 236, 248));
		}
		else {

			//str = m_pennyString;
			qglColor(QColor(0, 0, 0));
		}
	}
	else {

		drawGLRect(rect, QColor(68, 68, 68), -2.0f);
		qglColor(QColor(0, 0, 0));
	}
	
	m_oglTextSmallItalic->renderText(d+118, y1 + m_upperRectY, str);

	// Hermes status
	str = m_hermesString;
	rect = QRect(d+187, y1 + m_upperRectY,  m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 22, m_fonts.fontHeightSmallFont + 4);

	if (set->getHPSDRHardware() == 1) {

		drawGLRect(rect, m_textBackgroundColor, -2.0f);
		if (m_dataEngineState == QSDR::DataEngineUp) {

			str.append(m_hermesVersion);
			qglColor(QColor(206, 236, 248));
		}
		else
			qglColor(QColor(0, 0, 0));
	}
	else {

		drawGLRect(rect, QColor(68, 68, 68), -2.0f);
		qglColor(QColor(0, 0, 0));
	}
	
	m_oglTextSmallItalic->renderText(d+189, y1 + m_upperRectY, str);

	// Excalibur status
	str = m_excaliburString;
	//rect = QRect(d+250, y1 + m_upperRectY, d_fm.tightBoundingRect(str).width() + 22, fontHeight + 4);
	rect = QRect(d+250, y1 + m_upperRectY,  m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 2, m_fonts.fontHeightSmallFont + 4);

	if (set->getExcaliburPresence() && m_hwInterface == QSDR::Metis) {

		drawGLRect(rect, m_textBackgroundColor, -2.0f);
		if (m_dataEngineState == QSDR::DataEngineUp) {

			str.append(m_excaliburVersion);
			qglColor(QColor(206, 236, 248));
		}
		else
			qglColor(QColor(0, 0, 0));
	}
	else {

		drawGLRect(rect, QColor(68, 68, 68), -2.0f);
		qglColor(QColor(0, 0, 0));
	}
	
	m_oglTextSmallItalic->renderText(d+252, y1 + m_upperRectY, str);
	
	// Alex status
	str = m_alexString;
	//rect = QRect(d+322, y1 + m_upperRectY, d_fm.tightBoundingRect(str).width() + 22, fontHeight + 4);
	rect = QRect(d+302, y1 + m_upperRectY, m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 2, m_fonts.fontHeightSmallFont + 4);

	if (set->getAlexPresence()) {

		drawGLRect(rect, m_textBackgroundColor, -2.0f);
		if (m_dataEngineState == QSDR::DataEngineUp) {

			str.append(m_alexVersion);
			qglColor(QColor(206, 236, 248));
		}
		else
			qglColor(QColor(0, 0, 0));
	}
	else {

		drawGLRect(rect, QColor(68, 68, 68), -2.0f);
		qglColor(QColor(0, 0, 0));
	}
	
	//m_oglTextSmallItalic->renderText(d+324, y1 + m_upperRectY, str);
	m_oglTextSmallItalic->renderText(d+304, y1 + m_upperRectY, str);
}

void OGLDisplayPanel::paintLowerRegion() {

	QString str;

	GLint x1 = m_rxRect.left();
	//GLint y1 = m_rxRect.top();
	GLint y2 = m_rxRect.height();
	
	// Attenuator
	qglColor(QColor(106, 136, 148));
	m_oglTextSmallItalic->renderText(x1 + 3, y2 - m_lowerRectY, m_AttnString);

	if (m_mercuryAttenuator == 1)
		str = "0 dB";
	else
		str = "-20 dB";

	qglColor(m_activeTextColor);
	m_oglTextSmallItalic->renderText(m_AttnWidth + 8, y2 - m_lowerRectY, str);

	// Dither status
	if (m_dither == 1)
		qglColor(m_activeTextColor);
	else
		qglColor(QColor(68, 68, 68));

	m_oglTextSmallItalic->renderText(m_AttnWidth + 50, y2 - m_lowerRectY, m_ditherString);

	// Random status
	if (m_random == 1)
		qglColor(m_activeTextColor);
	else
		qglColor(QColor(68, 68, 68));

	m_oglTextSmallItalic->renderText(m_AttnWidth + m_ditherWidth + 60, y2 - m_lowerRectY, m_randomString);

	// Sample rate status
	str = "%1";
	qglColor(QColor(166, 196, 208));
	m_oglTextSmallItalic->renderText(m_AttnWidth + m_ditherWidth + m_randomWidth + 80, y2 - m_lowerRectY, str.arg(m_sample_rate, 3, 10, QLatin1Char(' ')));
	int samplerateWidth = m_oglTextSmall->fontMetrics().tightBoundingRect(str.arg(m_sample_rate, 3, 10, QLatin1Char(' '))).width();

	str = "kHz";
	m_oglTextSmallItalic->renderText(m_AttnWidth + m_ditherWidth + m_randomWidth + samplerateWidth + 85, y2 - m_lowerRectY, str);

	// server modus status
	switch (m_serverMode) {

		case QSDR::DemoMode:

			str = "Demo mode";
			break;

		case QSDR::NoServerMode:

			str = "No Server mode";
			break;

		case QSDR::SDRMode:
			
			str = "SDR Mode";
			break;

		case QSDR::ChirpWSPR:
				
			str = "Chirp Dec";
			break;

		case QSDR::ChirpWSPRFile:
				
			str = "Chirp Dec File";
			break;
	}
	qglColor(QColor(166, 196, 208));
	m_oglTextSmallItalic->renderText(m_AttnWidth + m_ditherWidth + m_randomWidth + 145, y2 - m_lowerRectY, str);

	if (m_hwInterface == QSDR::Metis) {

		// 10 MHz source status
		qglColor(QColor(106, 136, 148));
		m_oglTextSmallItalic->renderText(m_AttnWidth + m_ditherWidth + m_randomWidth + 200, y2 - m_lowerRectY, m_10MHzString);

		qglColor(QColor(166, 196, 208));
		m_oglTextSmallItalic->renderText(m_AttnWidth + m_ditherWidth + m_randomWidth + m_10MHzWidth + 205, y2 - m_lowerRectY, m_src10mhz);

		// 122.88 MHz source status
		qglColor(QColor(106, 136, 148));
		m_oglTextSmallItalic->renderText(m_AttnWidth + m_ditherWidth + m_randomWidth + m_10MHzWidth + 260, y2 - m_lowerRectY, m_12288MHzString);

		qglColor(QColor(166, 196, 208));
		m_oglTextSmallItalic->renderText(m_AttnWidth + m_ditherWidth + m_randomWidth + m_10MHzWidth + m_12288MHzWidth + 265, y2 - m_lowerRectY, m_src122_88mhz);
	}
}

void OGLDisplayPanel::paintRxRegion() {

	QString str;

	GLint x1 = m_rxRect.left();
	GLint y1 = m_rxRect.top();
	
	// draw background
	if (m_dataEngineState == QSDR::DataEngineUp) {

		//drawGLRect(m_rxRect, Qt::black, m_bkgColor2, -3.0f, false);
		drawGLRect(m_rect, Qt::black, m_bkgColor2, -3.0f, false);
		qglColor(m_activeTextColor);
	}
	else {

		//drawGLRect(m_rxRect, QColor(0, 0, 0, 255), -3.0f);
		drawGLRect(m_rect, QColor(0, 0, 0, 255), -3.0f);
		qglColor(QColor(68, 68, 68));
	}

	str = "%1.%2";
	int f1 = m_frequencyList[m_currentReceiver].freqMHz;
	int f2 = m_frequencyList[m_currentReceiver].freqkHz;

	int offset = x1;
	if (f1 / 1000 < 10)
		offset += 41;
	else
		offset += 21;

	//if (f1 / 1000 < 10)
		m_oglTextFreq1->renderText(offset, y1 + m_freqDigitsPosY, str.arg(f1/1000).arg(f1 - 1000 * (int)(f1/1000), 3, 10, QLatin1Char('0')));
	//else
	//	m_oglTextFreq1->renderText(x1 + 21, y1 + m_freqDigitsPosY, str.arg(f1/1000).arg(f1 - 1000 * (int)(f1/1000), 3, 10, QLatin1Char('0')));

	str = "%1";
	m_oglTextFreq2->renderText(x1 + 137, y1 + m_freqDigitsPosY + 1, str.arg(f2, 3, 10, QLatin1Char('0')));
	m_oglTextFreq1->renderText(x1 + 186, y1 + m_freqDigitsPosY - 1, "MHz");
	
	// current mouse wheel step size
	str = "step: %1";
	m_oglTextNormal->renderText(x1 + 280, y1 + m_freqDigitsPosY + 2, str.arg(set->getValue1000(m_mouseWheelFreqStep, 0, "Hz")));

	// current receiver
	str = "Rx: %1";
	m_oglTextBig->renderText(x1 + 280, y1 + m_freqDigitsPosY + 17, str.arg(m_currentReceiver + 1));

	// frequency info
	if (m_oldFreq != m_frequencyList[m_currentReceiver].frequency) {

		m_bandText = getHamBandTextString(set->getHamBandTextList(), false, m_frequencyList[m_currentReceiver].frequency);
		m_oldFreq = m_frequencyList[m_currentReceiver].frequency;
	}

	m_oglTextSmall->renderText(offset, y1 + m_freqDigitsPosY + 42, m_bandText);
}

void OGLDisplayPanel::paintSMeter() {

	GLint width = m_smeterRect.width();
	GLint height = m_smeterRect.height();
	GLint x1 = m_smeterRect.left();
	GLint x2 = x1 + width;
	GLint y1 = m_smeterRect.top();
	GLint y2 = y1 + height;
	
	//float X = m_smeterRect.left() + width/2.0f;
	//float Y = 370;
	
	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);
	glEnable(GL_LINE_SMOOTH);
	
	if (!m_smeterFBO || m_smeterUpdate || m_smeterRenew) {

		if (!m_smeterFBO || m_smeterRenew) {

			if (m_smeterFBO) {
			
				delete m_smeterFBO;
				m_smeterFBO = 0;
			}

			m_smeterFBO = new QGLFramebufferObject(m_sMeterWidth, height);
			//DISPLAYPANEL_DEBUG << "m_smeterFBO generated.";
		}

		m_smeterFBO->bind();
			renderSMeterScale();
			//renderSMeterB();
		m_smeterFBO->release();

		m_smeterUpdate = false;
		m_smeterRenew = false;
	}

	QRect rect = QRect(m_rxRect.right() + m_sMeterOffset, 0, m_sMeterWidth, height);
	renderTexture(rect, m_smeterFBO->texture(), -1.0f);
	
	// set a scissor box
	glScissor(x1, size().height() - y2, x2, height);
	glEnable(GL_SCISSOR_TEST);

	if (m_dataEngineState == QSDR::DataEngineUp) {

		// S-Meter hold value
		glLineWidth(1);
			
		int min = (int)(m_sMeterMinValueB * m_unit);
		int max = (int)(m_sMeterMaxValueB * m_unit);
		min += min%2;
		max += max%2;

		QRect bar = QRect(x1 + min, m_sMeterPosY + 4, max - min, 5);
		if (min > 0)
			drawGLRect(bar, QColor(255, 50, 50), QColor(255, 255, 50), true);

		glDisable(GL_LINE_SMOOTH);
		glDisable(GL_BLEND);
		glDisable(GL_MULTISAMPLE);

		/*if (min > 0) {

			for (int i = min; i <= max; i += 2) {

				glBegin(GL_LINES);
					glVertex3i(x1 + i, m_sMeterPosY + 3, 0.0);
					glVertex3i(x1 + i, m_sMeterPosY + 10, 0.0);
				glEnd();
			}
		}*/

		// S-Meter needle
		glLineWidth(2);
		qglColor(QColor(255, 255, 255));
		if (m_sMeterValue > 0) {

			glBegin(GL_LINES);
				glVertex3f(x1 + (int)(m_sMeterValue * m_unit), m_sMeterPosY - 15, 1.0f);
				glVertex3f(x1 + (int)(m_sMeterValue * m_unit), m_sMeterPosY + 28, 1.0f);
			glEnd();
		}

		// actual S-Meter value
		glEnable(GL_MULTISAMPLE);
		qglColor(m_activeTextColor);

		QString str = "%1";
		m_sMeterNumValueString = QString(str.arg(m_sMeterOrgValue, 0, 'f', 1));
		m_oglTextBig->renderText(x1 + m_sMeterWidth - 85, 80, 3.0, m_sMeterNumValueString);
		m_oglTextNormal->renderText(x1 + m_sMeterWidth - 28, 86, 3.0, "dBm");

		glEnable(GL_MULTISAMPLE);
	}

	// disable scissor box
	glDisable(GL_SCISSOR_TEST);
	
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);
	glDisable(GL_LINE_SMOOTH);
	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
}

void OGLDisplayPanel::renderSMeterA() {

	GLint width = m_smeterRect.width();
	GLint height = m_smeterRect.height();

	GLint x1 = m_smeterRect.left();
	GLint y1 = m_smeterRect.top();
	GLint x2 = x1 + width;
	GLint y2 = y1 + height;

	float X = (x2-x1)/2.0;
	float Y = 370;//355.0;

	GLint mid = m_smeterRect.width()/2;

	// draw background
	if (m_dataEngineState == QSDR::DataEngineUp)
		//drawGLRect(QRect(0, 0, x2-x1, y2-y1), QColor(0, 0, 0, 255), QColor(0, 0, 90, 255), false);
		drawGLRect(QRect(0, 0, x2-x1, y2-y1), Qt::black, m_bkgColor2, false);
		//drawGLRect(QRect(0, 0, x2-x1, y2-y1), QColor(0, 0, 0, 255), QColor(15, 55, 75, 255), false);
	else
		drawGLRect(QRect(0, 0, x2-x1, y2-y1), Qt::black);
	
	glDisable(GL_MULTISAMPLE);
	
	//***********************************************************
	// center of S-Meter circles

	// circle 0, first half
	float t = (float)(-156*ONEPI/256.0f);
	float R0 = 320.0;

	if (m_dataEngineState == QSDR::DataEngineUp)
		//qglColor(QColor(106, 136, 148));
		qglColor(QColor(180, 180, 180));
	else
		qglColor(QColor(68, 68, 68));
	
	glLineWidth(2);
	glBegin(GL_LINE_STRIP);
	for (int i = 0; i < m_smeterVertices; ++i) {
		
		glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
		t += TWOPI / m_smeterVertices;
		if ( t > -128*ONEPI/256.0f) break;
	}
	glEnd();

	// circle 0, second half
	R0 = 321.0;
	t = (float)(-126*ONEPI/256.0);
	if (m_dataEngineState == QSDR::DataEngineUp)
		qglColor(QColor(105, 110, 250));
	else
		qglColor(QColor(68, 68, 68));
	
	glLineWidth(3);
	glBegin(GL_LINE_STRIP);
	for (int i = 0; i < m_smeterVertices; ++i) {
		
		glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
		t += TWOPI / m_smeterVertices;
		if ( t > -99*ONEPI/256.0f) break;
	}
	glEnd();

	// circle 1
	if (m_dataEngineState == QSDR::DataEngineUp)
		//qglColor(QColor(106, 136, 148));
		qglColor(QColor(180, 180, 180));
	else
		qglColor(QColor(68, 68, 68));
	
	glLineWidth(2);
	float R1 = 312.0;
	t = (float)(-156*ONEPI/256.0f);
	glBegin(GL_LINE_STRIP);
	for (int i = 0; i < m_smeterVertices; ++i) {
		
		glVertex3f(X + R1 * qCos(t), Y + R1 * qSin(t), 0.0);
		t += TWOPI / m_smeterVertices;
		if ( t > -99*ONEPI/256.0f) break;
	}
	glEnd();


	// circle 2
	/*if (m_dataEngineState == QSDR::DataEngineUp)
		qglColor(QColor(106, 136, 148));
	else
		qglColor(QColor(68, 68, 68));
	
	glLineWidth(2);
	float R2 = 290.0;
	t = -156*ONEPI/256.0f;
	glBegin(GL_LINE_STRIP);
	for (int i = 0; i < numVertices; ++i) {
		
		glVertex3f(X + R2 * qCos(t), Y + R2 * qSin(t), 0.0);
		t += TWOPI / numVertices;
		if ( t > -99*ONEPI/256.0f) break;
	}
	glEnd();*/


	// circle 3
	/*if (m_dataEngineState == QSDR::DataEngineUp)
		qglColor(QColor(106, 136, 148));
	else
		qglColor(QColor(68, 68, 68));
	
	glLineWidth(2);
	float R3 = 270.0;
	t = -156*ONEPI/256.0f;
	glBegin(GL_LINE_STRIP);
	for (int i = 0; i < numVertices; ++i) {
		
		glVertex3f(X + R3 * qCos(t), Y + R3 * qSin(t), 0.0);
		t += TWOPI / numVertices;
		if ( t > -99*ONEPI/256.0f) break;
	}
	glEnd();*/

		
	// S-Meter ticks
	if (m_dataEngineState == QSDR::DataEngineUp)
		//qglColor(QColor(106, 136, 148));
		qglColor(QColor(180, 180, 180));
	else
		qglColor(QColor(68, 68, 68));

	QFontMetrics fm = m_oglTextNormal->fontMetrics();

	m_oglTextBigItalic->renderText(X - mid + 30, Y - R0, "S");

	t = (float)(-152*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+11) * qCos(t), Y + (R0+11) * qSin(t), 0.0);
	glEnd();

	int d = fm.width("1");
	m_oglTextNormal->renderText(X + (R0+28) * qCos(t) - d/2, Y + (R0+28) * qSin(t), "1");

	t = (float)(-149*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+7) * qCos(t), Y + (R0+7) * qSin(t), 0.0);
	glEnd();

	t = (float)(-146*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+11) * qCos(t), Y + (R0+11) * qSin(t), 0.0);
	glEnd();

	d = fm.width("3");
	m_oglTextNormal->renderText(X + (R0+28) * qCos(t) - d/2, Y + (R0+28) * qSin(t), "3");

	t = (float)(-143*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+7) * qCos(t), Y + (R0+7) * qSin(t), 0.0);
	glEnd();

	t = (float)(-140*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+11) * qCos(t), Y + (R0+11) * qSin(t), 0.0);
	glEnd();

	d = fm.width("5");
	m_oglTextNormal->renderText(X + (R0+28) * qCos(t) - d/2, Y + (R0+28) * qSin(t), "5");

	t = (float)(-137*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+7) * qCos(t), Y + (R0+7) * qSin(t), 0.0);
	glEnd();

	t = (float)(-134*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+11) * qCos(t), Y + (R0+11) * qSin(t), 0.0);
	glEnd();

	d = fm.width("7");
	m_oglTextNormal->renderText(X + (R0+28) * qCos(t) - d/2, Y + (R0+28) * qSin(t), "7");

	t = (float)(-131*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+7) * qCos(t), Y + (R0+7) * qSin(t), 0.0);
	glEnd();

	t = (float)(-ONEPI/2.0f);
	glBegin(GL_LINES);
	glVertex3f(X + (R0-1) * qCos(t), Y + (R0-1) * qSin(t), 0.0);
	glVertex3f(X + (R0+11) * qCos(t), Y + (R0+11) * qSin(t), 0.0);
	glEnd();

	d = fm.width("9");
	m_oglTextNormal->renderText(X + (R0+28) * qCos(t) - d/2, Y + (R0+28) * qSin(t), "9");

	if (m_dataEngineState == QSDR::DataEngineUp)
		qglColor(QColor(85, 90, 250));
	else
		qglColor(QColor(68, 68, 68));

	t = (float)(-123*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+7) * qCos(t), Y + (R0+7) * qSin(t), 0.0);
	glEnd();

	t = (float)(-118*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+11) * qCos(t), Y + (R0+11) * qSin(t), 0.0);
	glEnd();

	d = fm.width("+20");
	m_oglTextNormal->renderText(X + (R0+28) * qCos(t) - d/2, Y + (R0+28) * qSin(t), "+20");

	t = (float)(-113*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+7) * qCos(t), Y + (R0+7) * qSin(t), 0.0);
	glEnd();

	t = (float)(-108*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+11) * qCos(t), Y + (R0+11) * qSin(t), 0.0);
	glEnd();

	d = fm.width("+40");
	m_oglTextNormal->renderText(X + (R0+28) * qCos(t) - d/2, Y + (R0+28) * qSin(t), "+40");

	t = (float)(-103*ONEPI/256.0f);
	glBegin(GL_LINES);
	glVertex3f(X + R0 * qCos(t), Y + R0 * qSin(t), 0.0);
	glVertex3f(X + (R0+7) * qCos(t), Y + (R0+7) * qSin(t), 0.0);
	glEnd();

	m_oglTextBigItalic->renderText(X + mid - 35, Y - R0, "dB");

	glEnable(GL_MULTISAMPLE);
}

void OGLDisplayPanel::renderSMeterScale() {

	GLint width = m_sMeterWidth;
	GLint height = m_smeterRect.height();

	GLint x1 = m_smeterRect.left();
	GLint y1 = m_smeterRect.top();
	GLint x2 = x1 + width;
	GLint y2 = y1 + height;

	QFontMetrics fm = m_oglTextNormal->fontMetrics();

	//int fontHeight = fm.tightBoundingRect("S9").height();
	//int fontMaxWidth = fm.boundingRect("-000").width();

	qreal dBmRange = qAbs(m_dBmPanMax - m_dBmPanMin);
	m_unit = (qreal)(m_sMeterWidth / dBmRange);
	
	QRect rect = QRect(0, 0, x2-x1, y2-y1);
	
	// draw background
	if (m_dataEngineState == QSDR::DataEngineUp)
		drawGLRect(rect, Qt::black, m_bkgColor2, -3.0f, false);
	else
		drawGLRect(rect, Qt::black);

	glDisable(GL_MULTISAMPLE);
	glDisable(GL_LINE_SMOOTH);
	glLineWidth(1.0f);

	// draw horizontal lines
	if (m_dataEngineState == QSDR::DataEngineUp)
		qglColor(m_activeTextColor);
	else
		qglColor(m_inactiveTextColor);

	glBegin(GL_LINES);
		glVertex3f(0,		m_sMeterPosY, 0.0);
		glVertex3f(width-1,	m_sMeterPosY, 0.0);
		glVertex3f(0,		m_sMeterPosY + 12, 0.0);
		glVertex3f(width-1,	m_sMeterPosY + 12, 0.0);
	glEnd();

	// draw integer step scale
	if (m_dataEngineState == QSDR::DataEngineUp)
		qglColor(QColor(126, 156, 168));
	else
		qglColor(m_inactiveTextColor);

	int vertexArrayLength = m_sMeterWidth;
	vertexArrayLength += vertexArrayLength%2;
	TGL3float *vertexArray = new TGL3float[2*vertexArrayLength];

	for (int i = 0; i < vertexArrayLength; i++) {

		vertexArray[2*i].x = (GLfloat)(2.0f * i);
		vertexArray[2*i].y = (GLfloat)(m_sMeterPosY + 4);
		vertexArray[2*i].z = 0.0;

		vertexArray[2*i+1].x = (GLfloat)(2.0f * i);
		vertexArray[2*i+1].y = (GLfloat)(m_sMeterPosY + 9);
		vertexArray[2*i+1].z = 0.0;
	}

	glEnableClientState(GL_VERTEX_ARRAY);
				
	glVertexPointer(3, GL_FLOAT, 0, vertexArray);
	glDrawArrays(GL_LINES, 0, width);
	glDisableClientState(GL_VERTEX_ARRAY);

	delete[] vertexArray;

	// Draw the dbm items
	QString marker;

	if (m_dataEngineState == QSDR::DataEngineUp)
			qglColor(m_activeTextColor);
		else
			qglColor(m_inactiveTextColor);

	for (int i = 1, z = -120; z < 10; i++, z += 10) {
		
		// big ticks
		glBegin(GL_LINES);
			glVertex3f(10*i*m_unit, m_sMeterPosY - 4, 0.0);
			glVertex3f(10*i*m_unit, m_sMeterPosY, 0.0);
		glEnd();

		// small ticks
		glBegin(GL_LINES);
			glVertex3f((10*i - 5)*m_unit, m_sMeterPosY - 2, 0.0);
			glVertex3f((10*i - 5)*m_unit, m_sMeterPosY, 0.0);
		glEnd();
		
		marker = QString::number(z, 'f', 0);
		int d = fm.width(marker);

		if (z == -120 || z == -100 || z == -80 || z == -60 || z == -40 || z == -20) 
			m_oglTextNormal->renderText(10*i*m_unit-d/2-2, m_sMeterPosY - 18, marker);

		if (m_sMeterWidth > 500) {

			if (z == -110 || z == -90 || z == -70 || z == -50 || z == -30 || z == -10)
				m_oglTextNormal->renderText(10*i*m_unit-d/2-2, m_sMeterPosY - 18, marker);
		}

		if (m_sMeterWidth > 400)
			if (z == 0) m_oglTextNormal->renderText(10*i*m_unit-d/2, m_sMeterPosY - 18, marker);
	}

	m_oglTextSmallItalic->renderText(m_sMeterWidth - 25, m_sMeterPosY - 16, "dBm");

	// Draw the S1..S9 value items
	for (int i = 0; i < 17; i++) {

		if (i < 10) {
		
			glBegin(GL_LINES);
				glVertex3f((6*i + 3)*m_unit, m_sMeterPosY + 12, 0.0f);
				glVertex3f((6*i + 3)*m_unit, m_sMeterPosY + 17, 0.0f);
			glEnd();

			marker = "S1";
			int d = fm.width(marker);

			if (i == 1) {	
			
				m_oglTextNormal->renderText((6*(i+1) - d/2 + 1)*m_unit, m_sMeterPosY + 18, marker);
			}
			else if (i == 3) {
			
				marker = "S3";
				m_oglTextNormal->renderText((6*(i+1) - d/2 + 1)*m_unit, m_sMeterPosY + 18, marker);
			}
			else if (i == 5) {
			
				marker = "S5";
				m_oglTextNormal->renderText((6*(i+1) - d/2 + 1)*m_unit, m_sMeterPosY + 18, marker);
			}
			else if (i == 7) {
			
				marker = "S7";
				m_oglTextNormal->renderText((6*(i+1) - d/2 + 1)*m_unit, m_sMeterPosY + 18, marker);
			}
			else if (i == 9) {
			
				marker = "S9";
				m_oglTextNormal->renderText((6*(i+1) - d/2 + 1)*m_unit, m_sMeterPosY + 18, marker);
			}
		}
		else {

			if (m_dataEngineState == QSDR::DataEngineUp)
				qglColor(QColor(255, 80, 80));
				//qglColor(QColor(55, 180, 220));
			else
				qglColor(m_inactiveTextColor);

			int idx = (10*i - 33)*m_unit;
			glBegin(GL_LINES);
				glVertex3f(idx, m_sMeterPosY + 12, 0.0);
				glVertex3f(idx, m_sMeterPosY + 17, 0.0);
			glEnd();

			marker = "+20";
			int d = fm.width(marker);

			if (i == 11) {
				m_oglTextNormal->renderText(idx - d/2 - 2, m_sMeterPosY + 18, marker);
			}
			else if (i == 13) {
				
				marker = "+40";
				m_oglTextNormal->renderText(idx - d/2 - 2, m_sMeterPosY + 18, marker);
			}
			else if (i == 15) {
				
				marker = "+60";
				m_oglTextNormal->renderText(idx - d/2 - 2, m_sMeterPosY + 18, marker);
			}
			/*else if (i == 17) {
				
				marker = "+80";
				m_oglTextNormal->renderText(idx - d/2 - 2, m_sMeterPosY + 18, marker);
			}*/
		}
	}

	glBegin(GL_LINES);
		glVertex3f(57*m_unit+1, m_sMeterPosY+12, 0.0);
		glVertex3f(width-1,	  m_sMeterPosY+12, 0.0);
	glEnd();

	/*if (m_dataEngineState == QSDR::DataEngineUp)
		qglColor(m_activeTextColor);
	else
		qglColor(m_inactiveTextColor);*/
		
	glEnable(GL_LINE_SMOOTH);
	glEnable(GL_MULTISAMPLE);
}

void OGLDisplayPanel::renderSMeterB() {

	//GLint width = m_smeterRect.width();
	GLint width = m_sMeterWidth;
	GLint height = m_smeterRect.height();

	//GLint x1 = m_smeterRect.left();
	GLint x1 = m_smeterRect.left() + m_sMeterOffset;
	GLint y1 = m_smeterRect.top();
	GLint x2 = x1 + width;
	GLint y2 = y1 + height;

	QFontMetrics fm = m_oglTextNormal->fontMetrics();

	int vertexArrayLength = width/2;

	// draw background
	if (m_dataEngineState == QSDR::DataEngineUp)
		drawGLRect(QRect(0, 0, x2-x1, y2-y1), Qt::black, m_bkgColor2, -3.0f, false);
	else
		drawGLRect(QRect(0, 0, x2-x1, y2-y1), Qt::black);

	glDisable(GL_MULTISAMPLE);
	glDisable(GL_LINE_SMOOTH);
	glLineWidth(1.0f);

	// Draw horizontal lines
	if (m_dataEngineState == QSDR::DataEngineUp)
		qglColor(m_activeTextColor);
	else
		qglColor(m_inactiveTextColor);

	glBegin(GL_LINES);
		glVertex3f(0,		m_sMeterPosY, 0.0);
		glVertex3f(width-1,	m_sMeterPosY, 0.0);
		glVertex3f(0,		m_sMeterPosY + 12, 0.0);
		glVertex3f(width-1,	m_sMeterPosY + 12, 0.0);
	glEnd();

	if (m_dataEngineState == QSDR::DataEngineUp)
		qglColor(QColor(100, 100, 100));
	else
		qglColor(m_inactiveTextColor);

	TGL3float *vertexArray = new TGL3float[width];

	for (int i = 0; i < vertexArrayLength; i++) {

		vertexArray[2*i].x = (GLfloat)(2.0f * i);
		vertexArray[2*i].y = (GLfloat)(m_sMeterPosY + 3);
		vertexArray[2*i].z = 0.0;

		vertexArray[2*i+1].x = (GLfloat)(2.0f * i);
		vertexArray[2*i+1].y = (GLfloat)(m_sMeterPosY + 10);
		vertexArray[2*i+1].z = 0.0;
	}

	glEnableClientState(GL_VERTEX_ARRAY);
				
	glVertexPointer(3, GL_FLOAT, 0, vertexArray);
	glDrawArrays(GL_LINES, 0, width);
	glDisableClientState(GL_VERTEX_ARRAY);

	delete[] vertexArray;

	// Draw the S1..S9 value items
	//int d;
    //int markerSpacing = 24; // spacing for the S value items
	//int markerSpacing = m_sMeterWidth/12.5f; // spacing for the S value items
	float markerSpacing = qRound(m_sMeterWidth/12.5f); // spacing for the S value items
	//DISPLAYPANEL_DEBUG << "S markerSpacing:" << markerSpacing;

	QString marker;

	if (m_dataEngineState == QSDR::DataEngineUp)
		qglColor(m_activeTextColor);
	else
		qglColor(m_inactiveTextColor);

	//for (int x = 21, y = 9, z = 0; z < 5; x += markerSpacing, y += markerSpacing, z++) {
	for (int x = markerSpacing - 3, y = x - markerSpacing/2, z = 0; z < 5; x += markerSpacing, y += markerSpacing, z++) {
		
		if (z == 0) marker = "S1";
		else if (z == 1) marker = "S3";
		else if (z == 2) marker = "S5";
		else if (z == 3) marker = "S7";
		else if (z == 4) marker = "S9";
		
		// big ticks
		glBegin(GL_LINES);
			glVertex3f(x, m_sMeterPosY + 12, 0.0);
			glVertex3f(x, m_sMeterPosY + 16, 0.0);
		glEnd();
		
		// small ticks
		glBegin(GL_LINES);
			glVertex3f(y, m_sMeterPosY + 12, 0.0);
			glVertex3f(y, m_sMeterPosY + 15, 0.0);
		glEnd();

		// S strings
		//d = fm.width(marker);
		m_oglTextNormal->renderText(x-7, m_sMeterPosY + 18, marker);
	}

	// Draw the S+ value items
	//markerSpacing = 20;
	markerSpacing = qRound(m_sMeterWidth/15.0f);
	//DISPLAYPANEL_DEBUG << "S+ markerSpacing:" << markerSpacing;

	for (int x = 118 + markerSpacing, y = 128 + markerSpacing, z = 0; z < 8; x += markerSpacing, y += 2 * markerSpacing, z++) {
	//for (int x = next + markerSpacing, y = next + markerSpacing/2, z = 0; z < 8; x += markerSpacing, y += 2 * markerSpacing, z++) {
		
		if (m_dataEngineState == QSDR::DataEngineUp)
			qglColor(m_activeTextColor);
		else
			qglColor(m_inactiveTextColor);

		if (z == 0) marker = "+20";
		else if (z == 1) marker = "+40";
		else if (z == 2) marker = "+60";
		else if (z == 3) marker = "+80";
		
		// big ticks
		glBegin(GL_LINES);
			glVertex3f(x, m_sMeterPosY + 12, 0.0);
			glVertex3f(x, m_sMeterPosY + 16, 0.0);
		glEnd();
		
		if (m_dataEngineState == QSDR::DataEngineUp)
			qglColor(QColor(255, 80, 80));
		else
			qglColor(QColor(68, 68, 68));

		m_oglTextNormal->renderText(y, m_sMeterPosY + 18, marker);
	}

	// Draw the dbm items
	if (m_dataEngineState == QSDR::DataEngineUp)
			qglColor(m_activeTextColor);
		else
			qglColor(m_inactiveTextColor);

	//for (int x = 4, y = 14, z = -130; z < 10; x += markerSpacing, y += markerSpacing, z += 10) {
	for (int x = markerSpacing - 16, y = x + markerSpacing/2, z = -130; z < 10; x += markerSpacing, y += markerSpacing, z += 10) {
		
		marker = QString::number(z, 'f', 0);
		int d = fm.width(marker);
		
		// big ticks
		glBegin(GL_LINES);
			glVertex3f(x, m_sMeterPosY - 4, 0.0);
			glVertex3f(x, m_sMeterPosY, 0.0);
		glEnd();

		// small ticks
		glBegin(GL_LINES);
			glVertex3f(y, m_sMeterPosY - 2, 0.0);
			glVertex3f(y, m_sMeterPosY, 0.0);
		glEnd();
		
		if (z == -120 || z == -100 || z == -80 || z == -60 || z == -40 || z == -20) 
			m_oglTextNormal->renderText(x-d/2-2, m_sMeterPosY - 18, marker);

		if (z == 0) m_oglTextNormal->renderText(x-d/2-1, m_sMeterPosY - 18, marker);
	}

	m_oglTextSmallItalic->renderText(width - 25, m_sMeterPosY - 16, "dBm");

	glEnable(GL_LINE_SMOOTH);
	glEnable(GL_MULTISAMPLE);
}

//void OGLDisplayPanel::renderSMeterB() {
//
//	//GLint width = m_smeterRect.width();
//	GLint width = m_sMeterWidth;
//	GLint height = m_smeterRect.height();
//
//	GLint x1 = m_smeterRect.left();
//	GLint y1 = m_smeterRect.top();
//	GLint x2 = x1 + width;
//	GLint y2 = y1 + height;
//
//	QFontMetrics fm = m_oglTextNormal->fontMetrics();
//
//	int vertexArrayLength = width/2;
//
//	// draw background
//	if (m_dataEngineState == QSDR::DataEngineUp)
//		drawGLRect(QRect(0, 0, x2-x1, y2-y1), Qt::black, m_bkgColor2, -3.0f, false);
//	else
//		drawGLRect(QRect(0, 0, x2-x1, y2-y1), Qt::black);
//
//	glDisable(GL_MULTISAMPLE);
//	glDisable(GL_LINE_SMOOTH);
//	glLineWidth(1.0f);
//
//	// Draw horizontal lines
//	if (m_dataEngineState == QSDR::DataEngineUp)
//		qglColor(m_activeTextColor);
//	else
//		qglColor(QColor(68, 68, 68));
//
//	glBegin(GL_LINES);
//		glVertex3f(0,		m_sMeterPosY, 0.0);
//		glVertex3f(width-1,	m_sMeterPosY, 0.0);
//		glVertex3f(0,		m_sMeterPosY + 12, 0.0);
//		glVertex3f(width-1,	m_sMeterPosY + 12, 0.0);
//	glEnd();
//
//	if (m_dataEngineState == QSDR::DataEngineUp)
//		qglColor(QColor(100, 100, 100));
//	else
//		qglColor(QColor(68, 68, 68));
//
//	TGL3float *vertexArray = new TGL3float[width];
//
//	for (int i = 0; i < vertexArrayLength; i++) {
//
//		vertexArray[2*i].x = (GLfloat)(2.0f * i);
//		vertexArray[2*i].y = (GLfloat)(m_sMeterPosY + 3);
//		vertexArray[2*i].z = 0.0;
//
//		vertexArray[2*i+1].x = (GLfloat)(2.0f * i);
//		vertexArray[2*i+1].y = (GLfloat)(m_sMeterPosY + 10);
//		vertexArray[2*i+1].z = 0.0;
//	}
//
//	glEnableClientState(GL_VERTEX_ARRAY);
//				
//	glVertexPointer(3, GL_FLOAT, 0, vertexArray);
//	glDrawArrays(GL_LINES, 0, width);
//	glDisableClientState(GL_VERTEX_ARRAY);
//
//	delete[] vertexArray;
//
//	// Draw the S1..S9 value items
//	int d;
//    //int markerSpacing = 24; // spacing for the S value items
//	int markerSpacing = m_sMeterWidth/12.5f; // spacing for the S value items
//	QString marker;
//
//	if (m_dataEngineState == QSDR::DataEngineUp)
//		qglColor(m_activeTextColor);
//	else
//		qglColor(QColor(68, 68, 68));
//
//	for (int x = 21, y = 9, z = 0; z < 5; x += markerSpacing, y += markerSpacing, z++) {
//		
//		if (z == 0) marker = "S1";
//		else if (z == 1) marker = "S3";
//		else if (z == 2) marker = "S5";
//		else if (z == 3) marker = "S7";
//		else if (z == 4) marker = "S9";
//		
//		// big ticks
//		glBegin(GL_LINES);
//			glVertex3f(x, m_sMeterPosY + 12, 0.0);
//			glVertex3f(x, m_sMeterPosY + 16, 0.0);
//		glEnd();
//		
//		// small ticks
//		glBegin(GL_LINES);
//			glVertex3f(y, m_sMeterPosY + 12, 0.0);
//			glVertex3f(y, m_sMeterPosY + 15, 0.0);
//		glEnd();
//
//		// S strings
//		//d = fm.width(marker);
//		m_oglTextNormal->renderText(x-7, m_sMeterPosY + 18, marker);
//	}
//
//	// Draw the S+ value items
//	//markerSpacing = 20;
//	markerSpacing = m_sMeterWidth/15;
//
//	for (int x = 118 + markerSpacing, y = 128 + markerSpacing, z = 0; z < 8; x += markerSpacing, y += 2 * markerSpacing, z++) {
//		
//		if (m_dataEngineState == QSDR::DataEngineUp)
//			qglColor(m_activeTextColor);
//		else
//			qglColor(QColor(68, 68, 68));
//
//		if (z == 0) marker = "+20";
//		else if (z == 1) marker = "+40";
//		else if (z == 2) marker = "+60";
//		else if (z == 3) marker = "+80";
//		
//		// big ticks
//		glBegin(GL_LINES);
//			glVertex3f(x, m_sMeterPosY + 12, 0.0);
//			glVertex3f(x, m_sMeterPosY + 16, 0.0);
//		glEnd();
//		
//		// small ticks
//		/*glBegin(GL_LINES);
//			glVertex3f(y, 42, 0.0);
//			glVertex3f(y, 45, 0.0);
//		glEnd();*/
//
//		if (m_dataEngineState == QSDR::DataEngineUp)
//			qglColor(QColor(255, 80, 80));
//		else
//			qglColor(QColor(68, 68, 68));
//
//		m_oglTextNormal->renderText(y, m_sMeterPosY + 18, marker);
//	}
//
//	// Draw the dbm items
//	for (int x = 4, y = 14, z = -130; z < 10; x += markerSpacing, y += markerSpacing, z += 10)	{
//		
//		if (m_dataEngineState == QSDR::DataEngineUp)
//			qglColor(m_activeTextColor);
//		else
//			qglColor(QColor(68, 68, 68));
//
//		marker = QString::number(z, 'f', 0);
//		int d = fm.width(marker);
//		
//		// big ticks
//		glBegin(GL_LINES);
//			glVertex3f(x, m_sMeterPosY - 4, 0.0);
//			glVertex3f(x, m_sMeterPosY, 0.0);
//		glEnd();
//
//		// small ticks
//		glBegin(GL_LINES);
//			glVertex3f(y, m_sMeterPosY - 2, 0.0);
//			glVertex3f(y, m_sMeterPosY, 0.0);
//		glEnd();
//		
//		if (z == -120 || z == -100 || z == -80 || z == -60 || z == -40 || z == -20) 
//			m_oglTextNormal->renderText(x-d/2-2, m_sMeterPosY - 18, marker);
//
//		if (z == 0) m_oglTextNormal->renderText(x-d/2-1, m_sMeterPosY - 18, marker);
//	}
//
//	m_oglTextSmallItalic->renderText(width - 25, m_sMeterPosY - 16, "dBm");
//
//	glEnable(GL_MULTISAMPLE);
//}

//***********************************************
void OGLDisplayPanel::setSMeterValue(int rx, float value) {

	Q_UNUSED(rx)

	//qDebug() << "setSMeterValue = " << value;
	if (m_SMeterA) {
	
//		float tmp = (0.444f * value - 111.111f) * ONEPI/256.0f;
//
//		if (m_sMeterTimer.elapsed() > 40) {
//
//			if (tmp < m_sMeterMinValueA) m_sMeterMinValueA = tmp;
//
//			if (tmp > m_sMeterMaxValueA) m_sMeterMaxValueA = tmp;
//
//			int elapsedTimeMax = m_sMeterMaxTimer.elapsed();
//			if (elapsedTimeMax > m_sMeterHoldTime) {
//
//				if (m_sMeterPrevHoldTimeMax <= 0)
//					m_sMeterPrevHoldTimeMax = 1500;
//
//				m_sMeterMaxValueA -= (float)(elapsedTimeMax - m_sMeterPrevHoldTimeMax) / 40;
//				m_sMeterPrevHoldTimeMax = elapsedTimeMax;
//
//				if ((qRound(m_sMeterMaxValueA) <= qRound(tmp)) || (m_sMeterMaxValueA <= tmp)) {
//
//					m_sMeterMaxValueA = tmp;
//					m_sMeterMaxTimer.restart();
//					m_sMeterPrevHoldTimeMax = 0;
//				}
//			}
//
//			if (m_sMeterMinTimer.elapsed() > m_sMeterHoldTime) {
//
//				m_sMeterMinValueA = tmp;
//				m_sMeterMinTimer.restart();
//			}
//
//			m_sMeterValue = tmp * 0.13f + m_sMeterValue * 0.87f;
//
//			QString str = "%1";
//			//m_sMeterNumValueString = QString(str.arg(value, 0, 'g', 3));
//			//qDebug() << "m_sMeterValue" << value;
//
//			update();
//			m_sMeterTimer.restart();
//		}
	}
	else {

		//float tmp = (1.00423f * value + 93.3932f);
		float tmp;
		if (m_mercuryAttenuator)
			//tmp = (1.67f * value + 156.237f);
			tmp = (1.06962f * value + 99.1537f);
		else
			//tmp = (1.06962f * value + 99.1537f);
			tmp = (1.67f * value + 156.237f);

		//qDebug() << "S-Meter tmp = " << tmp;
		if (m_sMeterTimer.elapsed() > 40) {

			if (tmp < m_sMeterMinValueB) m_sMeterMinValueB = tmp;

			if (tmp > m_sMeterMaxValueB) m_sMeterMaxValueB = tmp;

			/*if (m_sMeterMaxTimer.elapsed() > m_sMeterHoldTime) {

				m_sMeterMaxValueB = tmp;
				m_sMeterMaxTimer.restart();
			}*/

			int elapsedTimeMax = m_sMeterMaxTimer.elapsed();
			if (elapsedTimeMax > m_sMeterHoldTime) {

				if (m_sMeterPrevHoldTimeMax <= 0) 
					m_sMeterPrevHoldTimeMax = m_sMeterHoldTime;
				
				// slowly reduce the peak hold level (taken from SDRMAX3 by (c) Cathy Moss)
				m_sMeterMaxValueB -= (float)(elapsedTimeMax - m_sMeterPrevHoldTimeMax) / 15;
				m_sMeterPrevHoldTimeMax = elapsedTimeMax;

				if ((qRound(m_sMeterMaxValueB) <= qRound(tmp)) || (m_sMeterMaxValueB <= tmp)) {
					
					m_sMeterMaxValueB = tmp;
					m_sMeterMaxTimer.restart();
					m_sMeterPrevHoldTimeMax = 0;
				}
			}

			int elapsedTimeMin = m_sMeterMinTimer.elapsed();
			if (elapsedTimeMin > m_sMeterHoldTime) {

				if (m_sMeterPrevHoldTimeMin <= 0) 
					m_sMeterPrevHoldTimeMin = m_sMeterHoldTime;
				
				// slowly increase the minimum hold level (taken from SDRMAX3 by (c) Cathy Moss)
				m_sMeterMinValueB += (float)(elapsedTimeMin - m_sMeterPrevHoldTimeMin) / 15;
				m_sMeterPrevHoldTimeMin = elapsedTimeMin;

				if ((qRound(m_sMeterMinValueB) >= qRound(tmp)) || (m_sMeterMinValueB >= tmp)) {
					
					m_sMeterMinValueB = tmp;
					m_sMeterMinTimer.restart();
					m_sMeterPrevHoldTimeMin = 0;
				}
			}

			/*if (m_sMeterMinTimer.elapsed() > m_sMeterHoldTime) {

				m_sMeterMinValueB = tmp;
				m_sMeterMinTimer.restart();
			}*/

			m_sMeterValue = tmp * 0.13f + m_sMeterValue * 0.87f;
			//qDebug() << "scaled m_sMeterValue " << m_sMeterValue;

			if (m_sMeterDisplayTime.elapsed() > 200) {
				
				if (m_mercuryAttenuator)
					//m_sMeterOrgValue = value - 17.7f;
					m_sMeterOrgValue = value - 37.7f;
				else
					//m_sMeterOrgValue = value - 37.7f;
					m_sMeterOrgValue = value - 17.7f;

				m_sMeterDisplayTime.restart();
			}
			//m_sMeterOrgValue = value - 37.7f;
			
			/*m_sMeterMeanValue+= value;
			m_sMeterMeanValueCnt++;

			if (m_sMeterMeanValueCnt > 100) {

				qDebug() << "m_sMeterMeanValue" << m_sMeterMeanValue/100;
				m_sMeterMeanValue = 0.0f;
				m_sMeterMeanValueCnt = 0;
			}*/
			//qDebug() << "          tmp" << tmp;

			update();
			m_sMeterTimer.restart();
		}
	}
}

void OGLDisplayPanel::setupDisplayRegions(QSize size) {

	int width  = size.width();
	int height = size.height();

	m_sMeterWidth = (int)(0.8f*(width - m_rxRectWidth));
	if (m_sMeterWidth < 300) m_sMeterWidth = 300;
	if (m_sMeterWidth > 600) m_sMeterWidth = 600;

	int d = 320 - m_sMeterWidth;
	if (d > 0)
		m_sMeterOffset = (int)(width - m_rxRectWidth - m_sMeterWidth)/2.0f;
	else
		m_sMeterOffset = width - m_rxRectWidth - m_sMeterWidth - 40;
		
	m_rect = QRect(0, 0, width, height);
	m_rxRect = QRect(0, 0, m_rxRectWidth, height);
	m_smeterRect = QRect(m_rxRect.right() + m_sMeterOffset, 0, width - m_rxRectWidth, height);

	//DISPLAYPANEL_DEBUG << "m_sMeterWidth:" << m_sMeterWidth;
	//DISPLAYPANEL_DEBUG << "            d:" << d;
	//DISPLAYPANEL_DEBUG << "m_smeterRectWidth:" << m_smeterRect.width();
	//DISPLAYPANEL_DEBUG << "m_sMeterOffset:" << m_sMeterOffset;
	
	m_freg1 		 = QRegion(QRect(166, 35, 8, m_fonts.fontHeightBigFont2));
	m_freg10 		 = QRegion(QRect(152, 35, 8, m_fonts.fontHeightBigFont2));
	m_freg100 		 = QRegion(QRect(138, 35, 8, m_fonts.fontHeightBigFont2));
	m_freg1000 		 = QRegion(QRect(112, 35, 12, m_fonts.fontHeightBigFont1));
	m_freg10000 	 = QRegion(QRect(93, 35, 12, m_fonts.fontHeightBigFont1));
	m_freg100000	 = QRegion(QRect(73, 35, 12, m_fonts.fontHeightBigFont1));
	m_freg1000000	 = QRegion(QRect(41, 35, 12, m_fonts.fontHeightBigFont1));
	m_freg10000000	 = QRegion(QRect(23, 35, 12, m_fonts.fontHeightBigFont1));

	m_smeterRenew = true;
}

void OGLDisplayPanel::getSelectedDigit(QPoint p) {

	if (m_freg1.contains(p))
		m_digitPosition = Freq1;
	else 
	if (m_freg10.contains(p))
		m_digitPosition = Freq10;
	else 
	if (m_freg100.contains(p))
		m_digitPosition = Freq100;
	else 
	if (m_freg1000.contains(p))
		m_digitPosition = Freq1000;
	else 
	if (m_freg10000.contains(p))
		m_digitPosition = Freq10000;
	else 
	if (m_freg100000.contains(p))
		m_digitPosition = Freq100000;
	else 
	if (m_freg1000000.contains(p))
		m_digitPosition = Freq1000000;
	else 
	if (m_freg10000000.contains(p))
		m_digitPosition = Freq10000000;
	else
		m_digitPosition = None;
}

//***********************************************
void OGLDisplayPanel::enterEvent(QEvent *event) {

	Q_UNUSED(event)
}

void OGLDisplayPanel::leaveEvent(QEvent *event) {

	Q_UNUSED(event)
}

void OGLDisplayPanel::mousePressEvent(QMouseEvent *event) {

	QPoint pos = event->pos();
	
	//if (m_serverMode != QSDR::ExternalDSP) {

		getSelectedDigit(pos);
		switch (m_digitPosition) {

			case Freq1:
				if (event->buttons() == Qt::LeftButton) {
					if (set->getMouseWheelFreqStep(m_currentReceiver) == 1.0)
						set->setMouseWheelFreqStep(this, m_currentReceiver, 5.0);
					else
						set->setMouseWheelFreqStep(this, m_currentReceiver, 1.0);
				}
				break;

			case Freq10:
				if (event->buttons() == Qt::LeftButton) {
					if (set->getMouseWheelFreqStep(m_currentReceiver) == 10.0)
						set->setMouseWheelFreqStep(this, m_currentReceiver, 50.0);
					else
						set->setMouseWheelFreqStep(this, m_currentReceiver, 10.0);
				}
				break;

			case Freq100:
				if (event->buttons() == Qt::LeftButton) {
					if (set->getMouseWheelFreqStep(m_currentReceiver) == 100.0)
						set->setMouseWheelFreqStep(this, m_currentReceiver, 500.0);
					else
						set->setMouseWheelFreqStep(this, m_currentReceiver, 100.0);
				}
				break;
	
			case Freq1000:
				if (event->buttons() == Qt::LeftButton) {
					if (set->getMouseWheelFreqStep(m_currentReceiver) == 1000.0)
						set->setMouseWheelFreqStep(this, m_currentReceiver, 5000.0);
					else
						set->setMouseWheelFreqStep(this, m_currentReceiver, 1000.0);
				}
				break;

			case Freq10000:
				if (event->buttons() == Qt::LeftButton) {
					if (set->getMouseWheelFreqStep(m_currentReceiver) == 10000.0)
						set->setMouseWheelFreqStep(this, m_currentReceiver, 50000.0);
					else
						set->setMouseWheelFreqStep(this, m_currentReceiver, 10000.0);
				}
				break;
	
			case Freq100000:
				if (event->buttons() == Qt::LeftButton) {
					if (set->getMouseWheelFreqStep(m_currentReceiver) == 100000.0)
						set->setMouseWheelFreqStep(this, m_currentReceiver, 500000.0);
					else
						set->setMouseWheelFreqStep(this, m_currentReceiver, 100000.0);
				}
				break;

			case Freq1000000:
				if (event->buttons() == Qt::LeftButton) {
					if (set->getMouseWheelFreqStep(m_currentReceiver) == 1000000.0)
						set->setMouseWheelFreqStep(this, m_currentReceiver, 5000000.0);
					else
						set->setMouseWheelFreqStep(this, m_currentReceiver, 1000000.0);
				}
				break;

			case Freq10000000:
			case None:
				return;
		}

	//}

	QWidget::mousePressEvent(event);
}

void OGLDisplayPanel::mouseReleaseEvent(QMouseEvent *event) {

	Q_UNUSED(event)
}

void OGLDisplayPanel::mouseMoveEvent(QMouseEvent *event) {

	QPoint pos = event->pos();

	QColor oldDigitColor = m_digitColor;

	//if (m_serverMode != QSDR::ExternalDSP) {

		getSelectedDigit(pos);
		switch (m_digitPosition) {

			case Freq1:
				setCursor(Qt::PointingHandCursor);
				if (m_dataEngineState == QSDR::DataEngineUp)
					m_digitColor = QColor(136, 166, 178);
				else
					m_digitColor = QColor(98, 98, 98);
				break;

			case Freq10:
				setCursor(Qt::PointingHandCursor);
				if (m_dataEngineState == QSDR::DataEngineUp)
					m_digitColor = QColor(136, 166, 178);
				else
					m_digitColor = QColor(98, 98, 98);
				break;

			case Freq100:
				setCursor(Qt::PointingHandCursor);
				if (m_dataEngineState == QSDR::DataEngineUp)
					m_digitColor = QColor(136, 166, 178);
				else
					m_digitColor = QColor(98, 98, 98);
				break;

			case Freq1000:
				setCursor(Qt::PointingHandCursor);
				if (m_dataEngineState == QSDR::DataEngineUp)
					m_digitColor = QColor(136, 166, 178);
				else
					m_digitColor = QColor(98, 98, 98);
				break;

			case Freq10000:
				setCursor(Qt::PointingHandCursor);
				if (m_dataEngineState == QSDR::DataEngineUp)
					m_digitColor = QColor(136, 166, 178);
				else
					m_digitColor = QColor(98, 98, 98);
				break;

			case Freq100000:
				setCursor(Qt::PointingHandCursor);
				if (m_dataEngineState == QSDR::DataEngineUp)
					m_digitColor = QColor(136, 166, 178);
				else
					m_digitColor = QColor(98, 98, 98);
				break;

			case Freq1000000:
				setCursor(Qt::PointingHandCursor);
				if (m_dataEngineState == QSDR::DataEngineUp)
					m_digitColor = QColor(136, 166, 178);
				else
					m_digitColor = QColor(98, 98, 98);
				break;

			case Freq10000000:
				setCursor(Qt::PointingHandCursor);
				if (m_dataEngineState == QSDR::DataEngineUp)
					m_digitColor = QColor(136, 166, 178);
				else
					m_digitColor = QColor(98, 98, 98);
				break;

			case None:
				setCursor(Qt::ArrowCursor);
				if (m_dataEngineState == QSDR::DataEngineUp)
					m_digitColor = QColor(106, 136, 148);
				else
					m_digitColor = QColor(68, 68, 68);
				
				break;
		}
	//}

	if (oldDigitColor != m_digitColor) update();

	QGLWidget::mouseMoveEvent(event);
}

void OGLDisplayPanel::wheelEvent(QWheelEvent * event) {

	//if (m_serverMode != QSDR::ExternalDSP) {

		long deltaF = 0;
		switch (m_digitPosition) {

			case Freq1:
				deltaF = 1;
				break;

			case Freq10:
				deltaF = 10;
				break;

			case Freq100:
				deltaF = 100;
				break;

			case Freq1000:
				deltaF = 1000;
				break;

			case Freq10000:
				deltaF = 10000;
				break;

			case Freq100000:
				deltaF = 100000;
				break;

			case Freq1000000:
				deltaF = 1000000;
				break;

			case Freq10000000:
				deltaF = 10000000;
				break;

			case None:
				return;
		}

		int numDegrees = event->delta() / 8;
		int numSteps = numDegrees / 15;

		int f1 = m_frequencyList[m_currentReceiver].freqMHz;
		int f2 = m_frequencyList[m_currentReceiver].freqkHz;

		long newFreq = -1;
		if (deltaF > 0)
			//newFreq = frequency1 * 1000 + frequency2 + numSteps * deltaF;
			newFreq = f1 * 1000 + f2 + numSteps * deltaF;

		if (newFreq < MAXFREQUENCY && newFreq >= 0)
			set->setFrequency(this, true, m_currentReceiver, newFreq);

		//qDebug() << "set frequency for rx" << m_currentReceiver;
			
	//}
	event->accept();
	QGLWidget::wheelEvent(event);
}

void OGLDisplayPanel::keyPressEvent(QKeyEvent* event) {

	Q_UNUSED(event)
}

void OGLDisplayPanel::closeEvent(QCloseEvent *event) {

	Q_UNUSED(event)
}

void OGLDisplayPanel::showEvent(QShowEvent *event) {

	Q_UNUSED(event)
}

void OGLDisplayPanel::timerEvent(QTimerEvent *event) {

	Q_UNUSED(event)
}

void OGLDisplayPanel::setSMeterHoldTime(int value) {

	m_sMeterHoldTime = value;
}

void OGLDisplayPanel::setSyncStatus(int value) {

	m_syncStatus = value;
	update();

	//QTimer::singleShot(50, this, SLOT(updateSyncStatus()));
}

void OGLDisplayPanel::updateSyncStatus() {

	if (m_dataEngineState == QSDR::DataEngineUp)
		m_syncStatus = 1;
	else
		m_syncStatus = 0;

	update();
}

void OGLDisplayPanel::setADCStatus(int value) {

	m_adcStatus = value;
	update();

	QTimer::singleShot(500, this, SLOT(updateADCStatus()));
}

void OGLDisplayPanel::updateADCStatus() {

	if (m_dataEngineState == QSDR::DataEngineUp)
		m_adcStatus = 1;
	else
		m_adcStatus = 0;

	update();
}

void OGLDisplayPanel::setPacketLossStatus(int value) {

	m_packetLossStatus = value;
	update();

	QTimer::singleShot(100, this, SLOT(updatePacketLossStatus()));
}

void OGLDisplayPanel::updatePacketLossStatus() {

	if (m_dataEngineState == QSDR::DataEngineUp)
		m_packetLossStatus = 1;
	else
		m_packetLossStatus = 0;

	update();
}

void OGLDisplayPanel::setSendIQStatus(int value) {

	m_sendIQStatus = value;
	update();
}

void OGLDisplayPanel::setRecvAudioStatus(int value) {

	m_recvAudioStatus = value;
	update();
}

void OGLDisplayPanel::setReceivers(QObject *sender, int value) {

	Q_UNUSED (sender)

	m_receivers = value;
	update();
}

void OGLDisplayPanel::setSampleRate(QObject *sender, int value) {

	Q_UNUSED (sender)

	m_sample_rate = value / 1000;
	update();
}

void OGLDisplayPanel::setMercuryAttenuator(QObject *sender, HamBand band, int value) {

	Q_UNUSED (sender)
	Q_UNUSED (band)

	m_mercuryAttenuator = value;
	update();
}

void OGLDisplayPanel::setDither(QObject *sender, int value) {

	Q_UNUSED (sender)

	m_dither = value;
	update();
}

void OGLDisplayPanel::setRandom(QObject *sender, int value) {

	Q_UNUSED (sender)

	m_random = value;
	update();
}

void OGLDisplayPanel::setCurrentReceiver(QObject *sender, int value) {

	Q_UNUSED(sender)

	m_currentReceiver = value;
	update();
}

void OGLDisplayPanel::setFrequency(QObject *sender, bool value, int rx, long freq) {

	Q_UNUSED (sender)
	Q_UNUSED (value)
	//Q_UNUSED (rx)

	//m_oldFreq = freq;

	TFrequency f;
	f.frequency = freq;
	f.freqMHz = (int)(freq / 1000);
	f.freqkHz = (int)(freq % 1000);

	//frequency1 = (int)(freq / 1000);
	//frequency2 = (int)(freq % 1000);

	/*if (m_frequencyList.count() < set->getCurrentReceivers()) {

		m_frequencyList << f;
	}
	else*/
		m_frequencyList[rx] = f;

	update();
}

void OGLDisplayPanel::set10mhzSource(QObject *sender, int value) {

	Q_UNUSED (sender)

	switch (value) {
		case 0:
			m_src10mhz = "Atlas";
			break;
		case 1:
			m_src10mhz = "Penelope";
			break;
		case 2:
			m_src10mhz = "Mercury";
			break;
	}
	update();
}

void OGLDisplayPanel::set122_88mhzSource(QObject *sender, int value) {

	Q_UNUSED (sender)

	switch (value) {
		case 0:
			m_src122_88mhz = "Penelope";
			break;
		case 1:
			m_src122_88mhz = "Mercury";
			break;
	}
	update();
}

void OGLDisplayPanel::setMercuryPresence(bool value) {

	m_mercury = value;
	update();
}

void OGLDisplayPanel::setPenelopePresence(bool value) {

	m_penelope = value;
	update();
}

void OGLDisplayPanel::setPennylanePresence(bool value) {

	m_pennylane = value;
	update();
}

void OGLDisplayPanel::setAlexPresence(bool value) {

	m_alex = value;
	update();
}

void OGLDisplayPanel::setExcaliburPresence(bool value) {

	m_excalibur = value;
	update();
}

void OGLDisplayPanel::setMercuryVersion(int value) {

	m_mercuryVersion.setNum(value/10);
	m_mercuryVersion.append(".");
	QString str;
	m_mercuryVersion.append(str.setNum(value%10));
}

void OGLDisplayPanel::setPenelopeVersion(int value) {

	m_penelopeVersion.setNum(value/10);
	m_penelopeVersion.append(".");
	QString str;
	m_penelopeVersion.append(str.setNum(value%10));
}

void OGLDisplayPanel::setPennylaneVersion(int value) {

	m_pennylaneVersion.setNum(value/10);
	m_pennylaneVersion.append(".");
	QString str;
	m_pennylaneVersion.append(str.setNum(value%10));
}

void OGLDisplayPanel::setHermesVersion(int value) {

	m_hermesVersion.setNum(value/10);
	m_hermesVersion.append(".");
	QString str;
	m_hermesVersion.append(str.setNum(value%10));
}

void OGLDisplayPanel::setMetisVersion(int value) {

	QString str;
	switch (m_hwInterface) {

		case QSDR::Metis:

			m_metisVersion.setNum(value/10);
			m_metisVersion.append(".");
			m_metisVersion.append(str.setNum(value%10));
			break;

		case QSDR::Hermes:
		case QSDR::NoInterfaceMode:

			break;
	}
	
}

void OGLDisplayPanel::setExcaliburVersion(QObject *sender, int value) {

	Q_UNUSED (sender)

	m_excaliburVersion.setNum(value/10);
	m_excaliburVersion.append(".");
	QString str;
	m_excaliburVersion.append(str.setNum(value%10));
}

void OGLDisplayPanel::setAlexVersion(QObject *sender, int value) {

	Q_UNUSED (sender)

	m_alexVersion.setNum(value/10);
	m_alexVersion.append(".");
	QString str;
	m_alexVersion.append(str.setNum(value%10));
}

void OGLDisplayPanel::setMouseWheelFreqStep(QObject *sender, int rx, qreal value) {

	Q_UNUSED (sender)

	if (rx == m_currentReceiver)
		m_mouseWheelFreqStep = value;

	update();
}

void OGLDisplayPanel::systemStateChanged(
	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	Q_UNUSED (sender)
	Q_UNUSED (err)

	//m_mutex.lock();
	if (m_serverMode != mode) {

		m_serverMode = mode;

	}

	if (m_hwInterface != hwmode)
		m_hwInterface = hwmode;

	if (m_dataEngineState != state)
		m_dataEngineState = state;

	if (state == QSDR::DataEngineDown) {

		m_sMeterMaxValueB = -1000.0f;
		m_sMeterMinValueB =  1000.0f;

		m_sMeterTimer.restart();
		m_sMeterDisplayTime.restart();
		m_sMeterMaxTimer.restart();
		m_sMeterMinTimer.restart();

		QTimer::singleShot(50, this, SLOT(updateADCStatus()));
		QTimer::singleShot(50, this, SLOT(updateSyncStatus()));
		QTimer::singleShot(50, this, SLOT(updatePacketLossStatus()));

		//resizeGL(width(), height());
	}
//	else if (state == QSDR::DataEngineUp) {
//
//		resizeGL(width(), height());
//	}

	m_smeterUpdate = true;
	m_smeterRenew = true;

	update();
	//paintGL();
}
