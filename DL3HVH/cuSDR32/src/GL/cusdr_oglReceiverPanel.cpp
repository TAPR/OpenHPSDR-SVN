/**
* @file  cusdr_oglReceiverPanel.cpp
* @brief Receiver panel class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-03-14
*/

/*
 *   Copyright 2011 Hermann von Hasseln, DL3HVH
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

#define LOG_GRAPHICS

// use: GRAPHICS_DEBUG

#include "cusdr_oglReceiverPanel.h"

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

#define	btn_height		14
#define	btn_width		60
#define	btn_widthb		70
#define	btn_widths		34

QGLReceiverPanel::QGLReceiverPanel(QWidget *parent, int rx)
	: QGLWidget(QGLFormat(QGL::SampleBuffers|QGL::AlphaChannel), parent)

	, set(Settings::instance())
	, m_serverMode(set->getCurrentServerMode())
	, m_hwInterface(set->getHWInterface())
	, m_dataEngineState(QSDR::DataEngineDown)
	, m_panadapterMode(set->getPanadapterMode())
	, m_waterColorScheme(set->getWaterfallColorScheme())
	, m_mousePos(QPoint(-1, -1))
	, m_mouseDownPos(QPoint(-1, -1))
	, m_panSpectrumBinsLength(0)
	, m_filterLeft(0)
	, m_filterRight(0)
	, m_filterTop(0)
	, m_filterBottom(0)
	, m_receiver(rx)
	, m_frequencyRxOnRx(0)
	, m_spectrumSize(set->getSpectrumSize())
	, m_specAveragingCnt(set->getSpectrumAveragingCnt())
	, m_currentReceiver(set->getCurrentReceiver())
	, m_waterfallAlpha(255)
	, m_freqRulerDisplayWidth(0)
	, m_oldWaterfallWidth(0)
	, m_panSpectrumMinimumHeight(0)
	, m_snapMouse(3)
	, m_sampleRate(set->getSampleRate())
	, m_downRate(set->getChirpDownSampleRate())
	, m_adcStatus(0)
	, m_smallSize(true)
	, m_spectrumVertexColorUpdate(false)
	, m_spectrumColorsChanged(true)
	, m_spectrumAveraging(set->getSpectrumAveraging())
	, m_spectrumAveragingOld(m_spectrumAveraging)
	, m_crossHairCursor(false)
	, m_panGrid(set->getPanGridStatus())
	, m_peakHold(false)
	, m_filterChanged(true)
	, m_showFilterLeftBoundary(false)
	, m_showFilterRightBoundary(false)
	, m_highlightFilter(false)
	, m_peakHoldBufferResize(true)
	, m_freqScaleZoomFactor(1.0f)
	, m_scaleMult(1.0f)
	, m_filterLowerFrequency(-3050.0)
	, m_filterUpperFrequency(-150.0)
	, m_freqRulerPosition(0.5)
{
	QGL::setPreferredPaintEngine(QPaintEngine::OpenGL);

	setSizePolicy(QSizePolicy::Expanding, QSizePolicy::Expanding);

	setAutoBufferSwap(true);
	setAutoFillBackground(false);
	
	setMouseTracking(true);
	setFocusPolicy(Qt::StrongFocus);

	//GRAPHICS_DEBUG << "set spectrum buffer size to: " << m_spectrumSize;

	m_bigHeight = 600;
	m_bigWidth = 1000;

	setupDisplayRegions(size());
	m_oldWidth = size().width();
	
	m_rxDataList = set->getReceiverDataList();
	m_frequency = m_rxDataList.at(m_receiver).frequency;

	m_dBmScalePanadapterRenew = true;
	m_dBmScalePanadapterUpdate = true;
	m_freqScalePanadapterRenew = true;
	m_freqScalePanadapterUpdate = true;
	m_panGridRenew = true;
	m_panGridUpdate = true;
	m_waterfallUpdate = true;
	m_secScaleWaterfallUpdate = true;
	m_secScaleWaterfallRenew = true;
	m_waterfallDisplayUpdate = true;

	HamBand band = m_rxDataList.at(m_receiver).hamBand;

	m_dBmPanMin = m_rxDataList.at(m_receiver).dBmPanScaleMinList.at(band);
	m_dBmPanMax = m_rxDataList.at(m_receiver).dBmPanScaleMaxList.at(band);

	m_waterfallOffsetLo = m_rxDataList.at(m_receiver).waterfallOffsetLo;
	m_waterfallOffsetHi = m_rxDataList.at(m_receiver).waterfallOffsetHi;

	m_secWaterfallMin = 0.0;
	m_secWaterfallMax = 0.0;

	m_filterLowerFrequency = m_rxDataList.at(m_receiver).filterLo;
	m_filterUpperFrequency = m_rxDataList.at(m_receiver).filterHi;

	m_mouseWheelFreqStep = m_rxDataList.at(m_receiver).mouseWheelFreqStep;

	m_agcMode = m_rxDataList.at(m_receiver).agcMode;
	m_agcModeString = set->getAGCModeString(m_receiver);
	m_agcFixedGain = m_rxDataList.at(m_receiver).agcFixedGain_dB;
//	m_agcThresholdOld = m_rxDataList.at(m_receiver).acgThreshold;
//	m_agcThresholdNew = m_agcThresholdOld;
//	m_agcHangLevelOld = m_rxDataList.at(m_receiver).agcHangLevel;
//	m_agcHangLevelNew = m_agcHangLevelOld;

	m_agcHangEnabled = m_rxDataList.at(m_receiver).hangEnabled;
	m_showAGCLines = m_rxDataList.at(m_receiver).agcLines;

	averager = new DualModeAverager(this, m_spectrumSize);

	fonts = new CFonts(this);
	m_fonts = fonts->getFonts();

	m_fonts.smallFont.setBold(true);
	m_oglTextTiny = new OGLText(m_fonts.tinyFont);
	m_oglTextSmall = new OGLText(m_fonts.smallFont);
	m_oglTextNormal = new OGLText(m_fonts.normalFont);
	m_oglTextFreq1 = new OGLText(m_fonts.freqFont1);
	m_oglTextFreq2 = new OGLText(m_fonts.freqFont2);
	m_oglTextBig1 = new OGLText(m_fonts.bigFont1);
	m_oglTextBig2 = new OGLText(m_fonts.bigFont2);
	m_oglTextHuge = new OGLText(m_fonts.hugeFont);

	timer = 0;

	setupConnections();

	m_displayTime.start();
	m_resizeTime.start();
	peakHoldTimer.start();
	//freqChangeTimer.start();
	
	m_fps = set->getFramesPerSecond(m_receiver);
	m_secWaterfallMin = -(1.0/m_fps) * m_secScaleWaterfallRect.height();

	
	m_dBmPanLogGain = 47;//49;//69 // allow user to calibrate this value

	m_cameraDistance = 0;
	m_cameraAngle = QPoint(0, 0);
	
	m_gridColor = set->getPanadapterColors().gridLineColor;

	m_redGrid   = (GLfloat)(m_gridColor.red()/256.0);
	m_greenGrid = (GLfloat)(m_gridColor.green()/256.0);
	m_blueGrid  = (GLfloat)(m_gridColor.blue()/256.0);

	m_bkgRed   = (GLfloat)(set->getPanadapterColors().panBackgroundColor.red() / 256.0);
	m_bkgGreen = (GLfloat)(set->getPanadapterColors().panBackgroundColor.green() / 256.0);
	m_bkgBlue  = (GLfloat)(set->getPanadapterColors().panBackgroundColor.blue() / 256.0);

	m_red	= (GLfloat)(set->getPanadapterColors().panLineColor.red() / 256.0);
	m_green = (GLfloat)(set->getPanadapterColors().panLineColor.green() / 256.0);
	m_blue	= (GLfloat)(set->getPanadapterColors().panLineColor.blue() / 256.0);

	m_redF	 = (GLfloat)(set->getPanadapterColors().panLineFilledColor.red() / 256.0);
	m_greenF = (GLfloat)(set->getPanadapterColors().panLineFilledColor.green() / 256.0);
	m_blueF  = (GLfloat)(set->getPanadapterColors().panLineFilledColor.blue() / 256.0);

	m_redST	  = (GLfloat)(set->getPanadapterColors().panSolidTopColor.red() / 256.0);
	m_greenST = (GLfloat)(set->getPanadapterColors().panSolidTopColor.green() / 256.0);
	m_blueST  = (GLfloat)(set->getPanadapterColors().panSolidTopColor.blue() / 256.0);

	m_redSB   = (GLfloat)(set->getPanadapterColors().panSolidBottomColor.red() / 256.0);
	m_greenSB = (GLfloat)(set->getPanadapterColors().panSolidBottomColor.green() / 256.0);
	m_blueSB  = (GLfloat)(set->getPanadapterColors().panSolidBottomColor.blue() / 256.0);

	m_waterfallLoColor = QColor(0, 0, 0, m_waterfallAlpha);
	m_waterfallHiColor = QColor(192, 124, 255, m_waterfallAlpha);
	m_waterfallMidColor = set->getPanadapterColors().waterfallColor.toRgb();
	m_waterfallColorRange = (int)(m_dBmPanMax - m_dBmPanMin);

	m_frequencyScaleFBO = 0;
	m_dBmScaleFBO = 0;
	m_panadapterGridFBO = 0;
	m_textureFBO = 0;
	m_waterfallLineFBO = 0;
	m_waterfallFBO = 0;
	m_secScaleWaterfallFBO = 0;

	m_waterfallLineCnt = 0;

	m_haircrossOffsetRight = 30;
	m_haircrossOffsetLeft = 116;
	m_haircrossMaxRight = 110;
	m_haircrossMinTop = 40;

	if (m_specAveragingCnt > 0)
		m_scale = 1.0f / m_specAveragingCnt;
	else
		m_scale = 1.0f;
}

QGLReceiverPanel::~QGLReceiverPanel() {

	disconnect(set, 0, this, 0);
	
	makeCurrent();
	glFinish();

	if (m_frequencyScaleFBO) {

		delete m_frequencyScaleFBO;
		m_frequencyScaleFBO = 0;
	}

	if (m_waterfallLineFBO) {

		delete m_waterfallLineFBO;
		m_waterfallLineFBO = 0;
	}
		
	if (m_waterfallFBO) {

		delete m_waterfallFBO;
		m_waterfallFBO = 0;
	}

	if (m_textureFBO) {

		delete m_textureFBO;
		m_textureFBO = 0;
	}

	if (m_dBmScaleFBO) {

		delete m_dBmScaleFBO;
		m_dBmScaleFBO = 0;
	}

	if (m_panadapterGridFBO) {

		delete m_panadapterGridFBO;
		m_panadapterGridFBO = 0;
	}

	if (m_secScaleWaterfallFBO) {

		delete m_secScaleWaterfallFBO;
		m_secScaleWaterfallFBO = 0;
	}

	if (averager)
		delete averager;

	while (!specAv_queue.isEmpty())
		specAv_queue.dequeue();
}

QSize QGLReceiverPanel::minimumSizeHint() const {
	
	if (m_receiver == 0)
		return QSize(width(), 250);
	else
		return QSize(250, 120);
	//return QSize(width(), height());
}

QSize QGLReceiverPanel::sizeHint() const {
	
	return QSize(width(), height());
}

void QGLReceiverPanel::setupConnections() {

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

//	CHECKED_CONNECT(
//		set,
//		SIGNAL(spectrumSizeChanged(QObject *, int)),
//		this,
//		SLOT(setSpectrumSize(QObject *, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(freqRulerPositionChanged(float, int)), 
		this, 
		SLOT(freqRulerPositionChanged(float, int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(frequencyChanged(QObject *, bool, int, long)),
		this,
		SLOT(setFrequency(QObject *, bool, int, long)));

	CHECKED_CONNECT(
		set,
		SIGNAL(hamBandChanged(QObject *, int, bool, HamBand)),
		this,
		SLOT(setHamBand(QObject *, int, bool, HamBand)));

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
		SIGNAL(filterFrequenciesChanged(QObject *, int, qreal, qreal)), 
		this, 
		SLOT(setFilterFrequencies(QObject *, int, qreal, qreal)));

//	CHECKED_CONNECT(
//		set,
//		SIGNAL(waterfallTimeChanged(int, int)),
//		this,
//		SLOT(setWaterfallTime(int, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(waterfallOffesetLoChanged(int, int)),
		this,
		SLOT(setWaterfallOffesetLo(int, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(waterfallOffesetHiChanged(int, int)),
		this,
		SLOT(setWaterfallOffesetHi(int, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(spectrumAveragingChanged(bool)), 
		this, 
		SLOT(setSpectrumAveraging(bool)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(spectrumAveragingCntChanged(int)), 
		this, 
		SLOT(setSpectrumAveragingCnt(int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(panGridStatusChanged(bool)),
		this,
		SLOT(setPanGridStatus(bool)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(peakHoldStatusChanged(bool)),
		this,
		SLOT(setPeakHoldStatus(bool)));

	/*CHECKED_CONNECT(
		set, 
		SIGNAL(panadapterColorChanged()),
		this,
		SLOT(setSimpleWaterfallColor()));*/

	CHECKED_CONNECT(
		set, 
		SIGNAL(panadapterColorChanged()),
		this,
		SLOT(setPanadapterColors()));

	CHECKED_CONNECT(
		set, 
		SIGNAL(mercuryAttenuatorChanged(QObject *, HamBand, int)),
		this, 
		SLOT(setMercuryAttenuator(QObject *, HamBand, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(coupledRxChanged(QObject *, int)), 
		this, 
		SLOT(setCoupledRx(QObject *, int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(adcOverflowChanged(int)),
		this,
		SLOT(setADCStatus(int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(framesPerSecondChanged(QObject*, int, int)),
		this,
		SLOT(setFramesPerSecond(QObject*, int, int)));

//	CHECKED_CONNECT(
//		set,
//		SIGNAL(agcThresholdLine_dBmChanged(QObject *, int, qreal)),
//		this,
//		SLOT(setAGCThresholdLine_dBm(QObject *, int, qreal)));

	CHECKED_CONNECT(
		set,
		SIGNAL(agcFixedGainChanged_dB(QObject *, int, qreal)),
		this,
		SLOT(setAGCLineFixedLevel(QObject *, int, qreal)));

	CHECKED_CONNECT(
		set,
		SIGNAL(agcLineLevelsChanged(QObject *, int, qreal, qreal)),
		this,
		SLOT(setAGCLineLevels(QObject *, int, qreal, qreal)));

	CHECKED_CONNECT(
		set,
		SIGNAL(agcModeChanged(QObject *, int, AGCMode, bool)),
		this,
		SLOT(setAGCMode(QObject *, int, AGCMode, bool)));

//	CHECKED_CONNECT(
//		set,
//		SIGNAL(agcHangEnabledChanged(QObject *, int, bool)),
//		this,
//		SLOT(setAGCHangEnabled(QObject *, int, bool)));

	CHECKED_CONNECT(
		set,
		SIGNAL(mouseWheelFreqStepChanged(QObject *, int, qreal)),
		this,
		SLOT(setMouseWheelFreqStep(QObject *, int, qreal)));

}

void QGLReceiverPanel::initializeGL() {

	if (!isValid()) return;
	

	/*QOpenGLInfo glInfo;
	glInfo.getInfo();
	glInfo.printSelf();*/

	//*****************************************************************
	// default initialization

	//glShadeModel(GL_FLAT);
	glShadeModel(GL_SMOOTH);
	//glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
	glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	glPixelStorei(GL_UNPACK_ALIGNMENT, 4); // 4-byte pixel alignment
	//glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);
    //glHint(GL_POLYGON_SMOOTH_HINT, GL_NICEST);

	glDepthFunc(GL_LESS);
    glEnable(GL_DEPTH_TEST);
	glDisable(GL_CULL_FACE);
    
	m_cnt = 0;

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
}

void QGLReceiverPanel::paintGL() {

	switch (m_serverMode) {

		case QSDR::ChirpWSPR:
		case QSDR::ChirpWSPRFile:
		case QSDR::NoServerMode:
		case QSDR::DemoMode:

			drawGLRect(QRect(0, 0, width(), height()), QColor(0, 0, 0));
			break;

		case QSDR::SDRMode:

			//if (freqChangeTimer.elapsed() > 50)  m_spectrumAveraging = m_spectrumAveragingOld;

			if (m_resizeTime.elapsed() > 100 || m_dataEngineState == QSDR::DataEngineDown)
				paintReceiverDisplay();
			
			break;
	}
}
 
void QGLReceiverPanel::paintReceiverDisplay() {
 
	QRect mouse_rect(0, 0, 100, 100);
	mouse_rect.moveCenter(m_mousePos);

	if (m_filterChanged) {

		m_filterLo = m_filterLowerFrequency / m_sampleRate;
		m_filterHi = m_filterUpperFrequency / m_sampleRate;
		m_filterChanged = false;
	}
	//m_displayTime.restart();

	drawPanadapter();
	drawPanHorizontalScale();
	drawPanVerticalScale();
	drawPanadapterGrid();
	drawPanFilter();

	if (m_dataEngineState == QSDR::DataEngineUp && m_showAGCLines && (m_receiver == m_currentReceiver))
		drawAGCControl();

	//if (m_dataEngineState == QSDR::DataEngineUp)
		drawReceiverInfo();

	if (m_waterfallDisplayUpdate && m_waterfallRect.height() > 10) {

		drawWaterfall();
		drawWaterfallVerticalScale();
		m_waterfallDisplayUpdate = false;
	}

	if ((m_mouseRegion == panadapterRegion || m_mouseRegion == waterfallRegion) && m_crossHairCursor)
		drawCrossHair();
 }

void QGLReceiverPanel::paint3DPanadapterMode() {
}
  
void QGLReceiverPanel::drawPanadapter() {

	GLint vertexArrayLength = (GLint)m_panadapterBins.size();

	GLint height = m_panRect.height();
	GLint x1 = m_panRect.left();
	GLint y1 = m_panRect.top();
	GLint x2 = x1 + m_panRect.width();
	GLint y2 = y1 + m_panRect.height();

	// y scale
	float yScale;
	//float yScaleColor;
	float yTop;
	
	qreal dBmRange = qAbs(m_dBmPanMax - m_dBmPanMin);

	yScale = m_panRect.height() / dBmRange;
	//yScaleColor = 1.0f / dBmRange;
	//yScaleColor = 0.05f * yScale;
	yTop = (float) y2;
	
	if (m_dataEngineState == QSDR::DataEngineUp)
		glClear(GL_DEPTH_BUFFER_BIT);
	else
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glEnable(GL_MULTISAMPLE);
	glEnable(GL_LINE_SMOOTH);

	//glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_BLEND);

	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);
	glLineWidth(1);

	// draw background
	if (m_dataEngineState == QSDR::DataEngineUp) {

		if (m_receiver == m_currentReceiver) {

//			glBegin(GL_TRIANGLE_STRIP);
//				glColor3f(0.2f * m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x1, y1, -4.0); // top left corner
//				glColor3f(0.2f * m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x2, y1, -4.0); // top right corner
//				glColor3f(0.8f * m_bkgRed, 0.8f * m_bkgGreen, 0.8f * m_bkgBlue); glVertex3f(x1, y2, -4.0); // bottom left corner
//				glColor3f(       m_bkgRed,        m_bkgGreen,        m_bkgBlue); glVertex3f(x2, y2, -4.0); // bottom right corner
//			glEnd();
			glBegin(GL_TRIANGLE_STRIP);
				glColor3f(0.8f * m_bkgRed, 0.8f * m_bkgGreen, 0.8f * m_bkgBlue); glVertex3f(x1, y1, -4.0); // top left corner
				glColor3f(0.6f * m_bkgRed, 0.6f * m_bkgGreen, 0.6f * m_bkgBlue); glVertex3f(x2, y1, -4.0); // top right corner
				glColor3f(0.4f * m_bkgRed, 0.4f * m_bkgGreen, 0.4f * m_bkgBlue); glVertex3f(x1, y2, -4.0); // bottom left corner
				glColor3f(0.2f * m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x2, y2, -4.0); // bottom right corner
			glEnd();
		}
		else {

			glBegin(GL_TRIANGLE_STRIP);
				glColor3f(0.4f * m_bkgRed, 0.4f * m_bkgGreen, 0.4f * m_bkgBlue); glVertex3f(x1, y1, -4.0); // top left corner
				glColor3f(0.4f * m_bkgRed, 0.4f * m_bkgGreen, 0.4f * m_bkgBlue); glVertex3f(x2, y1, -4.0); // top right corner
				glColor3f(0.4f * m_bkgRed, 0.4f * m_bkgGreen, 0.4f * m_bkgBlue); glVertex3f(x1, y2, -4.0); // bottom left corner
				glColor3f(0.4f * m_bkgRed, 0.4f * m_bkgGreen, 0.4f * m_bkgBlue); glVertex3f(x2, y2, -4.0); // bottom right corner
			glEnd();
		}

		if (m_adcStatus == 2) {

			glBegin(GL_TRIANGLE_STRIP);
				glColor3f(m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x1, y1, -4.0); // top left corner
				glColor3f(m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x2, y1, -4.0); // top right corner
				glColor3f(m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x1, y2, -4.0); // bottom left corner
				glColor3f(m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x2, y2, -4.0); // bottom right corner
			glEnd();

			QString str = "ADC  Overload";
			int rectWidth = m_fonts.hugeFontMetrics->boundingRect(str).width();

			qglColor(QColor(255, 0, 0, 155));
			m_oglTextHuge->renderText((x2-rectWidth)/2, y1+30, -3.5f, str);
		}
	}
	else {

		drawGLRect(m_panRect, QColor(30, 30, 50, 155), -4.0f);
	}

	// set a scissor box
	glScissor(x1, size().height() - y2, x2, height);
	glEnable(GL_SCISSOR_TEST);

	//spectrumBufferMutex.lock();

	TGL3float *vertexArray = new TGL3float[vertexArrayLength];
	TGL3float *vertexColorArray = new TGL3float[vertexArrayLength];
	
	TGL3float *vertexArrayBg = new TGL3float[2*vertexArrayLength];
	TGL3float *vertexColorArrayBg = new TGL3float[2*vertexArrayLength];

	TGL3float *vertexArrayBgPH = new TGL3float[2*vertexArrayLength];
	TGL3float *vertexColorArrayBgPH = new TGL3float[2*vertexArrayLength];

	TGL3float *vertexArrayPH = new TGL3float[vertexArrayLength];
	TGL3float *vertexColorArrayPH = new TGL3float[vertexArrayLength];
	
	switch (m_panadapterMode) {

		case QSDRGraphics::FilledLine:

			for (int i = 0; i < vertexArrayLength; i++) {
			
				mutex.lock();
				vertexColorArrayBg[2*i].x = 0.7 * m_redF;
				vertexColorArrayBg[2*i].y = 0.7 * m_greenF;
				vertexColorArrayBg[2*i].z = 0.7 * m_blueF;
								
				vertexColorArrayBg[2*i+1].x = 0.3 * m_redF;
				vertexColorArrayBg[2*i+1].y = 0.3 * m_greenF;
				vertexColorArrayBg[2*i+1].z = 0.3 * m_blueF;
				
				if (!m_peakHold) {
				
					vertexColorArray[i].x = m_red;
					vertexColorArray[i].y = m_green;
					vertexColorArray[i].z = m_blue;

					/*vertexColorArray[i].x = m_red   * (yScaleColor * (m_panadapterBins.at(i) - m_dBmPanMin));
					vertexColorArray[i].y = m_green * (yScaleColor * (m_panadapterBins.at(i) - m_dBmPanMin));
					vertexColorArray[i].z = m_blue  * (yScaleColor * (m_panadapterBins.at(i) - m_dBmPanMin));*/
				}
				else {
					
					vertexColorArray[i].x = 0.7f;
					vertexColorArray[i].y = 0.7f;
					vertexColorArray[i].z = 0.7f;

					vertexColorArrayPH[i].x = m_red;
					vertexColorArrayPH[i].y = m_green;
					vertexColorArrayPH[i].z = m_blue;

					/*vertexColorArray[i].x = m_red   * (yScaleColor * (m_panadapterBins.at(i) - m_dBmPanMin));
					vertexColorArray[i].y = m_green * (yScaleColor * (m_panadapterBins.at(i) - m_dBmPanMin));
					vertexColorArray[i].z = m_blue  * (yScaleColor * (m_panadapterBins.at(i) - m_dBmPanMin));*/
				}
				mutex.unlock();

				vertexArrayBg[2*i].x = (GLfloat)(i/m_scaleMult);
				vertexArrayBg[2*i].y = (GLfloat)(yTop - yScale * m_panadapterBins.at(i));
				vertexArrayBg[2*i].z = -1.5;

				vertexArrayBg[2*i+1].x = (GLfloat)(i/m_scaleMult);
				vertexArrayBg[2*i+1].y = (GLfloat)yTop;
				vertexArrayBg[2*i+1].z = -1.5;
	
				/*vertexArray[i].x = (GLfloat)(i/m_scaleMult);
				vertexArray[i].y = (GLfloat)(yTop - yScale * m_panadapterBins.at(i));
				vertexArray[i].z = -1.0;*/

				if (m_peakHold) {
					
					vertexArrayPH[i].x = (GLfloat)(i/m_scaleMult);
					vertexArrayPH[i].y = (GLfloat)(yTop - yScale * m_panPeakHoldBins.at(i));
					vertexArrayPH[i].z = -0.5;
				}
			}
			
			glEnableClientState(GL_VERTEX_ARRAY);
			glEnableClientState(GL_COLOR_ARRAY);
				
			glVertexPointer(3, GL_FLOAT, 0, vertexArrayBg);
			glColorPointer(3, GL_FLOAT, 0, vertexColorArrayBg);
			glDrawArrays(GL_TRIANGLE_STRIP, 0, 2*vertexArrayLength);

			if (m_peakHold) {
				
				glVertexPointer(3, GL_FLOAT, 0, vertexArrayPH);
				glColorPointer(3, GL_FLOAT, 0, vertexColorArrayPH);
				glDrawArrays(GL_LINE_STRIP, 0, vertexArrayLength);
			}
			else {

				glVertexPointer(3, GL_FLOAT, 6*sizeof(float), vertexArrayBg);
				glColorPointer(3, GL_FLOAT, 0, vertexColorArray);
				glDrawArrays(GL_LINE_STRIP, 0, vertexArrayLength);
			}

			glDisableClientState(GL_VERTEX_ARRAY);
			glDisableClientState(GL_COLOR_ARRAY); 

			break;

		case QSDRGraphics::Line:

			for (int i = 0; i < vertexArrayLength; i++) {
	
				mutex.lock();
				if (!m_peakHold) {
				
					vertexColorArray[i].x = m_red;
					vertexColorArray[i].y = m_green;
					vertexColorArray[i].z = m_blue;

					/*vertexColorArray[i].x = m_red   * (yScaleColor * (m_panadapterBins.at(i) - m_dBmPanMin));
					vertexColorArray[i].y = m_green * (yScaleColor * (m_panadapterBins.at(i) - m_dBmPanMin));
					vertexColorArray[i].z = m_blue  * (yScaleColor * (m_panadapterBins.at(i) - m_dBmPanMin));*/

					/*vertexColorArray[i].x = m_red   * (yScaleColor * m_panadapterBins.at(i));
					vertexColorArray[i].y = m_green * (yScaleColor * m_panadapterBins.at(i));
					vertexColorArray[i].z = m_blue  * (yScaleColor * m_panadapterBins.at(i));*/
				}
				else {
					
					vertexColorArray[i].x = 0.6f;
					vertexColorArray[i].y = 0.6f;
					vertexColorArray[i].z = 0.6f;

					vertexColorArrayPH[i].x = m_red;
					vertexColorArrayPH[i].y = m_green;
					vertexColorArrayPH[i].z = m_blue;
				}
				mutex.unlock();
				
				vertexArray[i].x = (GLfloat)(i/m_scaleMult);
				vertexArray[i].y = (GLfloat)(yTop - yScale * m_panadapterBins.at(i));
				vertexArray[i].z = -1.0;
				
				if (m_peakHold) {
					
					vertexArrayPH[i].x = (GLfloat)(i/m_scaleMult);
					vertexArrayPH[i].y = (GLfloat)(yTop - yScale * m_panPeakHoldBins.at(i));
					vertexArrayPH[i].z = -0.5;
				}
			}
		
			glEnableClientState(GL_VERTEX_ARRAY);
			glEnableClientState(GL_COLOR_ARRAY);
			
			glVertexPointer(3, GL_FLOAT, 0, vertexArray);
			glColorPointer(3, GL_FLOAT, 0, vertexColorArray);
			glDrawArrays(GL_LINE_STRIP, 0, vertexArrayLength);
			
			if (m_peakHold) {
				
				glVertexPointer(3, GL_FLOAT, 0, vertexArrayPH);
				glColorPointer(3, GL_FLOAT, 0, vertexColorArrayPH);
				glDrawArrays(GL_LINE_STRIP, 0, vertexArrayLength);
			}
			
			glDisableClientState(GL_VERTEX_ARRAY);
			glDisableClientState(GL_COLOR_ARRAY);

			break;

		case QSDRGraphics::Solid:

			glDisable(GL_MULTISAMPLE);
			glDisable(GL_LINE_SMOOTH);

			for (int i = 0; i < vertexArrayLength; i++) {
			
				mutex.lock();
				if (!m_peakHold) {
					
					vertexColorArrayBg[2*i].x = m_redST;
					vertexColorArrayBg[2*i].y = m_greenST;
					vertexColorArrayBg[2*i].z = m_blueST;
								
					vertexColorArrayBg[2*i+1].x = m_redSB;
					vertexColorArrayBg[2*i+1].y = m_greenSB;
					vertexColorArrayBg[2*i+1].z = m_blueSB;
				}
				else {

					vertexColorArrayBg[2*i].x = m_redSB;
					vertexColorArrayBg[2*i].y = m_greenSB;
					vertexColorArrayBg[2*i].z = m_blueSB;
								
					vertexColorArrayBg[2*i+1].x = m_redSB;
					vertexColorArrayBg[2*i+1].y = m_greenSB;
					vertexColorArrayBg[2*i+1].z = m_blueSB;

					vertexColorArrayBgPH[2*i].x = 0.9f;//m_redST;
					vertexColorArrayBgPH[2*i].y = 0.9f;//m_greenST;
					vertexColorArrayBgPH[2*i].z = 0.9f;//m_blueST;
								
					vertexColorArrayBgPH[2*i+1].x = 0.9f;// * m_redSB;
					vertexColorArrayBgPH[2*i+1].y = 0.9f;// * m_greenSB;
					vertexColorArrayBgPH[2*i+1].z = 0.9f;// * m_blueSB;
				}
				mutex.unlock();

				vertexArrayBg[2*i].x = (GLfloat)(i/m_scaleMult);
				vertexArrayBg[2*i].y = (GLfloat)(yTop - yScale * m_panadapterBins.at(i));
				vertexArrayBg[2*i].z = -1.0f;

				vertexArrayBg[2*i+1].x = (GLfloat)(i/m_scaleMult);
				vertexArrayBg[2*i+1].y = (GLfloat)yTop;
				vertexArrayBg[2*i+1].z = -1.0f;

				if (m_peakHold) {
					
					vertexArrayBgPH[2*i].x = (GLfloat)(i/m_scaleMult);
					vertexArrayBgPH[2*i].y = (GLfloat)(yTop - yScale * m_panPeakHoldBins.at(i));
					vertexArrayBgPH[2*i].z = -2.0f;

					vertexArrayBgPH[2*i+1].x = (GLfloat)(i/m_scaleMult);
					vertexArrayBgPH[2*i+1].y = (GLfloat)yTop;
					vertexArrayBgPH[2*i+1].z = -2.0f;
				}
			}
			
			glEnableClientState(GL_VERTEX_ARRAY);
			glEnableClientState(GL_COLOR_ARRAY);
				
			glVertexPointer(3, GL_FLOAT, 0, vertexArrayBg);
			glColorPointer(3, GL_FLOAT, 0, vertexColorArrayBg);
			//glDrawArrays(GL_QUAD_STRIP, 0, 2*vertexArrayLength);
			glDrawArrays(GL_LINES, 0, 2*vertexArrayLength);

			if (m_peakHold) {

				glVertexPointer(3, GL_FLOAT, 0, vertexArrayBgPH);
				glColorPointer(3, GL_FLOAT, 0, vertexColorArrayBgPH);
				glDrawArrays(GL_LINES, 0, 2*vertexArrayLength);
			}

			glDisableClientState(GL_VERTEX_ARRAY);
			glDisableClientState(GL_COLOR_ARRAY);

			break;
	}
	//spectrumBufferMutex.unlock();

	delete[] vertexArray;
	delete[] vertexColorArray;
	delete[] vertexArrayBg;
	delete[] vertexColorArrayBg;
	delete[] vertexArrayBgPH;
	delete[] vertexColorArrayBgPH;
	delete[] vertexArrayPH;
	delete[] vertexColorArrayPH;

	//glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);

	glDisable(GL_MULTISAMPLE);
	glDisable(GL_LINE_SMOOTH);

	// disable scissor box
	glDisable(GL_SCISSOR_TEST);
} 

void QGLReceiverPanel::drawPanVerticalScale() {

	if (!m_dBmScalePanRect.isValid()) return;

	int width = m_dBmScalePanRect.width();
	int height = m_dBmScalePanRect.height();

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	glColor3f(0.65f, 0.76f, 0.81f);
	
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);
	
	if (!m_dBmScaleFBO || m_dBmScalePanadapterUpdate || m_dBmScalePanadapterRenew)	{

		if (!m_dBmScaleFBO || m_dBmScalePanadapterRenew) {

			if (m_dBmScaleFBO) {
			
				delete m_dBmScaleFBO;
				m_dBmScaleFBO = 0;
			}
			m_dBmScaleFBO = new QGLFramebufferObject(width, height);
			//if (m_dBmScaleFBO)
			//	GRAPHICS_DEBUG << "m_dBmScaleFBO generated.";
			
		}

		m_dBmScaleFBO->bind();
			renderPanVerticalScale();
		m_dBmScaleFBO->release();
		
		m_dBmScalePanadapterUpdate = false;
		m_dBmScalePanadapterRenew = false;
	}
		
	renderTexture(m_dBmScalePanRect, m_dBmScaleFBO->texture(), 0.0f);

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
	glColor3f(0.65f, 0.76f, 0.81f);
}

void QGLReceiverPanel::drawPanHorizontalScale() {

	if (!m_freqScalePanRect.isValid()) return;

	int width = m_freqScalePanRect.width();
	int height = m_freqScalePanRect.height();

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	glColor3f(0.65f, 0.76f, 0.81f);
	
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);

	if (!m_frequencyScaleFBO || m_freqScalePanadapterUpdate || m_freqScalePanadapterRenew) {

		if (!m_frequencyScaleFBO || m_freqScalePanadapterRenew) {

			if (m_frequencyScaleFBO) {
			
				delete m_frequencyScaleFBO;
				m_frequencyScaleFBO = 0;
			}

			m_frequencyScaleFBO = new QGLFramebufferObject(width, height);
			//if (m_frequencyScaleFBO)
			//	GRAPHICS_DEBUG << "m_frequencyScaleFBO generated.";
		}

		glPushAttrib(GL_VIEWPORT_BIT);
		glViewport(0, 0, width, height);
		setProjectionOrthographic(width, height);
		
		m_frequencyScaleFBO->bind();
			renderPanHorizontalScale();
		m_frequencyScaleFBO->release();

		glPopAttrib();
		glViewport(0, 0, size().width(), size().height());
		setProjectionOrthographic(size().width(), size().height());
		
		m_freqScalePanadapterUpdate = false;
		m_freqScalePanadapterRenew = false;
	}

	renderTexture(m_freqScalePanRect, m_frequencyScaleFBO->texture(), 0.0f);

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
	glColor3f(0.65f, 0.76f, 0.81f);
}

void QGLReceiverPanel::drawPanadapterGrid() {

	if (!m_panGrid) return;

	int width = m_panRect.width();
	int height = m_panRect.height();

	//glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_BLEND);
	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	
	if (m_receiver == m_currentReceiver)
		glColor4f(m_redGrid, m_greenGrid, m_blueGrid, 1.0f);
	else
		glColor4f(m_redGrid, m_greenGrid, m_blueGrid, 0.8f);

	
	glDisable(GL_MULTISAMPLE);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);
	
	if (!m_panadapterGridFBO || m_panGridUpdate || m_panGridRenew)	{

		if (!m_panadapterGridFBO || m_panGridRenew) {

			if (m_panadapterGridFBO) {
			
				delete m_panadapterGridFBO;
				m_panadapterGridFBO = 0;
			}

			m_panadapterGridFBO = new QGLFramebufferObject(width, height);
			//if (m_panadapterGridFBO)
			//	GRAPHICS_DEBUG << "m_panadapterGridFBO generated.";
		}

		glPushAttrib(GL_VIEWPORT_BIT);
		glViewport(0, 0, width, height);
		setProjectionOrthographic(width, height);
		
		m_panadapterGridFBO->bind();
			renderPanadapterGrid();
		m_panadapterGridFBO->release();

		glPopAttrib();
		glViewport(0, 0, size().width(), size().height());
		setProjectionOrthographic(size().width(), size().height());
		
		m_panGridUpdate = false;
		m_panGridRenew = false;
	}

	renderTexture(m_panRect, m_panadapterGridFBO->texture(), -3.0f);
	
	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
	glColor3f(0.65f, 0.76f, 0.81f);
	glEnable(GL_MULTISAMPLE);
}

void QGLReceiverPanel::drawPanFilter() {

	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);
	
	QColor color;
	//QString str;

	if (m_highlightFilter)
		color = QColor(150, 150, 150, 140);
	else
		color = QColor(150, 150, 150, 100);

	m_filterLeft = m_panRect.left() + qRound((qreal)(m_panRect.width()/2.0f) + m_filterLo * m_panRect.width() / m_freqScaleZoomFactor);
	m_filterRight = m_panRect.left() + qRound((qreal)(m_panRect.width()/2.0f) + m_filterHi * m_panRect.width() / m_freqScaleZoomFactor);
	m_filterTop = m_panRect.top() + 1;
	m_filterBottom = m_panRect.top() + m_panRect.height() - 1;
	
	m_filterRect = QRect(m_filterLeft, m_filterTop, m_filterRight - m_filterLeft, m_filterBottom - m_filterTop);

	if ((m_filterLeft >= m_panRect.left() && m_filterLeft <= m_panRect.right()) ||
		(m_filterRight >= m_panRect.left() && m_filterRight <= m_panRect.right()) ||
		(m_filterLeft < m_panRect.left() && m_filterRight > m_panRect.right()))
	{
		if (m_filterRect.height() > 5) drawGLRect(m_filterRect, color, 0.0);
	}

	// filter boundaries
	if (m_showFilterLeftBoundary) {

		color = QColor(150, 150, 150, 230);

		glDisable(GL_MULTISAMPLE);
		glLineWidth(1);
		qglColor(color);
		glBegin(GL_LINES);
			glVertex3f(m_filterLeft, m_filterTop, 4.0f);
			glVertex3f(m_filterLeft, m_filterBottom, 4.0f);
		glEnd();
		glEnable(GL_MULTISAMPLE);

		// text
		QString str1 = QString("Filter Lo");
		QString str2 = frequencyString(m_filterLowerFrequency, true);

		qglColor(QColor(0, 0, 0, 255));
		if (m_smallSize) {

			m_oglTextSmall->renderText(m_filterLeft + 5, m_filterTop + 44, 4.0f, str1);
			m_oglTextSmall->renderText(m_filterLeft + 5, m_filterTop + 64, 4.0f, str2);
		}
		else {

			m_oglTextBig1->renderText(m_filterLeft + 5, m_filterTop + 44, 4.0f, str1);
			m_oglTextBig1->renderText(m_filterLeft + 5, m_filterTop + 64, 4.0f, str2);
		}

		qglColor(QColor(255, 255, 255, 255));
		if (m_smallSize) {

			m_oglTextSmall->renderText(m_filterLeft + 3, m_filterTop + 42, 5.0f, str1);
			m_oglTextSmall->renderText(m_filterLeft + 3, m_filterTop + 62, 5.0f, str2);
		}
		else {

			m_oglTextBig1->renderText(m_filterLeft + 3, m_filterTop + 42, 5.0f, str1);
			m_oglTextBig1->renderText(m_filterLeft + 3, m_filterTop + 62, 5.0f, str2);
		}
	}

	if (m_showFilterRightBoundary) {

		color = QColor(150, 150, 150, 230);

		glDisable(GL_MULTISAMPLE);
		glLineWidth(1);
		qglColor(color);
		glBegin(GL_LINES);
			glVertex3f(m_filterRight, m_filterTop, 4.0f);
			glVertex3f(m_filterRight, m_filterBottom, 4.0f);
		glEnd();
		glEnable(GL_MULTISAMPLE);

		// text
		QString str1 = QString("Filter Hi");
		QString str2 = frequencyString(m_filterUpperFrequency, true);

		qglColor(QColor(0, 0, 0, 255));
		if (m_smallSize) {

			m_oglTextSmall->renderText(m_filterRight + 5, m_filterTop + 44, 4.0f, str1);
			m_oglTextSmall->renderText(m_filterRight + 5, m_filterTop + 64, 4.0f, str2);
		}
		else {

			m_oglTextBig1->renderText(m_filterRight + 5, m_filterTop + 44, 4.0f, str1);
			m_oglTextBig1->renderText(m_filterRight + 5, m_filterTop + 64, 4.0f, str2);
		}

		qglColor(QColor(255, 255, 255, 255));
		if (m_smallSize) {

			m_oglTextSmall->renderText(m_filterRight + 3, m_filterTop + 42, 5.0f, str1);
			m_oglTextSmall->renderText(m_filterRight + 3, m_filterTop + 62, 5.0f, str2);
		}
		else {

			m_oglTextBig1->renderText(m_filterRight + 3, m_filterTop + 42, 5.0f, str1);
			m_oglTextBig1->renderText(m_filterRight + 3, m_filterTop + 62, 5.0f, str2);

		}
	}

	// draw a line for the display center
	GLint y1 = m_panRect.top() + 3;
	GLint y2 = m_displayCenterlineHeight;
	
	if (y2 > y1 + 3) {

		GLint x = m_panRect.width()/2;
			
		color = set->getPanadapterColors().panCenterLineColor;

		//glDisable(GL_LINE_SMOOTH);
		glDisable(GL_MULTISAMPLE);
		glLineWidth(1);
		glColor4ub(color.red(), color.green(), color.blue(), color.alpha());
		glBegin(GL_LINES);
			glVertex3f(x, m_filterTop, 4.0f);
			glVertex3f(x, m_filterBottom, 4.0f);
		glEnd();
		glEnable(GL_MULTISAMPLE);
	}

	// draw a frequency line from a different receiver
	if (m_frequencyRxOnRx != 0) {
		
		y1 = m_waterfallRect.top();
		y2 = m_waterfallRect.bottom();
	
		if (y2 > y1 + 3) {

			qreal unit = (qreal)((m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width());
			qreal df = m_frequency - m_rxDataList.at(m_frequencyRxOnRx%10 - 1).frequency;

			GLint dx = (GLint)(df/unit);
			GLint x = m_panRect.width()/2 - dx;
			
			color = QColor(255, 255, 0, 255);

			glDisable(GL_MULTISAMPLE);
			glLineWidth(2);
			glColor4ub(color.red(), color.green(), color.blue(), color.alpha());
			glBegin(GL_LINES);
				glVertex3f(x-1, y1, 4.0f);
				glVertex3f(x-1, y2, 4.0f);
			glEnd();
			glEnable(GL_MULTISAMPLE);
		}
	}
}

void QGLReceiverPanel::drawWaterfall() {

	if (m_waterfallRect.isEmpty()) return;
	
	int top = m_waterfallRect.top();
	int left = m_waterfallRect.left();
	int width = m_waterfallRect.width();
	int height = m_waterfallRect.height();
	//int height = this->size().height();


	glColor4f(1.0, 1.0, 1.0, 1.0);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);

	// check for framebuffer objects
	if (set->getFBOPresence()) {
	
		// create the FBOs if not exist
		if (!m_textureFBO || !m_waterfallLineFBO || !m_waterfallFBO || m_waterfallUpdate) {
	
			if (m_waterfallLineFBO) {

				delete m_waterfallLineFBO;
				m_waterfallLineFBO = 0;
			}
		
			if (m_waterfallFBO) {

				delete m_waterfallFBO;
				m_waterfallFBO = 0;
			}

			if (m_textureFBO) {

				delete m_textureFBO;
				m_textureFBO = 0;
			}

			if (QGLFramebufferObject::hasOpenGLFramebufferBlit()) {
			
				//QGLFramebufferObjectFormat format;
				//format.setSamples(2);
				//format.setAttachment(QGLFramebufferObject::CombinedDepthStencil);

				m_waterfallLineFBO = new QGLFramebufferObject(width, 1);
				//if (m_waterfallLineFBO)
				//GRAPHICS_DEBUG << "m_waterfallLineFBO generated.";

				m_waterfallFBO = new QGLFramebufferObject(width, height);
				//if (m_waterfallFBO)
				//GRAPHICS_DEBUG << "m_waterfallFBO generated.";

				m_textureFBO = new QGLFramebufferObject(width, height);
				//if (m_textureFBO)
				//GRAPHICS_DEBUG << "m_textureFBO generated.";
				
			}	
			else {
			
				GRAPHICS_DEBUG << "has no OpenGL FramebufferBlit";
			}

			m_waterfallUpdate = false;
			drawGLRect(m_waterfallRect, Qt::black);

			m_waterfallLineCnt = 0;
						
			if (width > 0) {

				m_waterfallPixel.clear();
				m_waterfallPixel.resize(width);
		
				TGL_ubyteRGBA col;
				col.red = 0; col.green = 0;	col.blue = 0; col.alpha = 255;
		
				for (int i = 0; i < width; i++) 
					m_waterfallPixel[i] = col;
			}
		}

		if (m_dataEngineState == QSDR::DataEngineUp) {

			GLint oldTex;
			glGetIntegerv(GL_TEXTURE_BINDING_2D, &oldTex);

			m_waterfallLineFBO->bind();
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, 1, 0, GL_RGBA, GL_UNSIGNED_BYTE, m_waterfallPixel.data());
			m_waterfallLineFBO->release();

			m_waterfallLineCnt++;
			if (m_waterfallLineCnt > height) m_waterfallLineCnt = height;

			// draw the waterfall
			glEnable(GL_TEXTURE_2D);
			glBindTexture(GL_TEXTURE_2D, m_waterfallFBO->texture());
	
			glBegin(GL_QUADS);
				glTexCoord2f(0, 1); glVertex2i(left,			top);			// top left corner
				glTexCoord2f(1, 1); glVertex2i(left + width,	top);			// top right corner
				glTexCoord2f(1, 0); glVertex2i(left + width,	top + height);	// bottom right corner
				glTexCoord2f(0, 0); glVertex2i(left,			top + height);	// bottom left corner
			glEnd();
			glBindTexture(GL_TEXTURE_2D, oldTex);
			glDisable(GL_TEXTURE_2D);

			glPushAttrib(GL_VIEWPORT_BIT);
			glViewport(0, 0, width, height);
			setProjectionOrthographic(width, height);

			// render to the next waterfall texture
			m_textureFBO->bind();

			glEnable(GL_TEXTURE_2D);
			glBindTexture(GL_TEXTURE_2D, m_waterfallFBO->texture());

			// the waterfall FBO
			glBegin(GL_QUADS);
				glTexCoord2f(0, 1); glVertex2i(0,	  1);			// top left corner
				glTexCoord2f(1, 1); glVertex2i(width, 1);			// top right corner
				glTexCoord2f(1, 0); glVertex2i(width, 1 + height);	// bottom right corner
				glTexCoord2f(0, 0); glVertex2i(0,	  1 + height);	// bottom left corner
			glEnd();

			// the new waterfall line
			glBindTexture(GL_TEXTURE_2D, m_waterfallLineFBO->texture());
			
			glBegin(GL_QUADS);
				glTexCoord2f(0, 1); glVertex2i(0,		0);	// top left corner
				glTexCoord2f(1, 1); glVertex2i(width,	0);	// top right corner
				glTexCoord2f(1, 0); glVertex2i(width,	1);	// bottom right corner
				glTexCoord2f(0, 0); glVertex2i(0,		1);	// bottom left corner
			glEnd();
			
			m_textureFBO->release();

			glDisable(GL_TEXTURE_2D);
			glPopAttrib();
			glViewport(0, 0, size().width(), size().height());
			setProjectionOrthographic(size().width(), size().height());

			if (m_waterfallLineCnt < height) {

				QRect rect(0, top + m_waterfallLineCnt, width, height - m_waterfallLineCnt);
				drawGLRect(rect, QColor(0, 0, 0, 255), 3.0f);
			}

			// copy the next waterfall image to the waterfall FBO
			QRect copyRect(0, 0, width, height);
			QGLFramebufferObject::blitFramebuffer(m_waterfallFBO, copyRect, m_textureFBO, copyRect);
		}
		else {

			drawGLRect(m_waterfallRect, Qt::black);
		}
	}
	else {
	}
}

void QGLReceiverPanel::drawWaterfallVerticalScale() {

	if (!m_secScaleWaterfallRect.isValid()) return;

	int width = m_secScaleWaterfallRect.width();
	int height = m_secScaleWaterfallRect.height();

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	glColor3f(0.65f, 0.76f, 0.81f);

	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);

	if (!m_secScaleWaterfallFBO || m_secScaleWaterfallUpdate || m_secScaleWaterfallRenew)	{

		if (!m_secScaleWaterfallFBO || m_secScaleWaterfallRenew) {

			if (m_secScaleWaterfallFBO) {

				delete m_secScaleWaterfallFBO;
				m_secScaleWaterfallFBO = 0;
			}
			m_secScaleWaterfallFBO = new QGLFramebufferObject(width, height);
			//if (m_secScaleWaterfallFBO)
			//	GRAPHICS_DEBUG << "m_secScaleWaterfallFBO generated.";
		}

		m_secScaleWaterfallFBO->bind();
			renderWaterfallVerticalScale();
		m_secScaleWaterfallFBO->release();

		m_secScaleWaterfallUpdate = false;
		m_secScaleWaterfallRenew = false;
	}

	renderTexture(m_secScaleWaterfallRect, m_secScaleWaterfallFBO->texture(), 4.0f);

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
	glColor3f(0.65f, 0.76f, 0.81f);
}

void QGLReceiverPanel::drawCrossHair() {

	QRect rect(0, m_panRect.top(), width(), height() - m_panRect.top());

	int x = m_mousePos.x();
	int y = m_mousePos.y();

	int textOffset = 20;
	int spacing = 6;

	glDisable(GL_MULTISAMPLE);
	glDisable(GL_BLEND);
	glDisable(GL_LINE_SMOOTH);
	glLineWidth(1.0f);

	qglColor(QColor(125, 125, 125, 255));

	// set a scissor box
	glScissor(rect.left(), rect.top(), rect.width() - 1, rect.height());
	glEnable(GL_SCISSOR_TEST);

	// horizontal line
	glBegin(GL_LINES);
		glVertex3f(m_dBmScalePanRect.right() - 2, y, 4.0f);
		glVertex3f(rect.right() - 1, y, 4.0f);
	glEnd();

	// vertical line
	glBegin(GL_LINES);
		glVertex3f(x, rect.top() + 1, 4.0f);
		glVertex3f(x, rect.bottom() - 1, 4.0f);
	glEnd();

	// cross hair
	qglColor(QColor(255, 255, 255, 255));
	glBegin(GL_LINES);
		glVertex3f(x     , y - 20, 5.0f);
		glVertex3f(x     , y + 20, 5.0f);
		glVertex3f(x - 20, y, 5.0f);
		glVertex3f(x + 20, y, 5.0f);
	glEnd();

	// text	
	QString dFstr;
	QString fstr;
	QString dBstr;

	qglColor(QColor(255, 55, 55, 255));

	int dx = m_panRect.width()/2 - x;
	qreal unit = (qreal)((m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width());
	qreal df = unit * dx;
	qreal frequency = m_frequency - df;
	
	dFstr = frequencyString(-df, true);
	fstr = frequencyString(frequency);

	qreal dBm = glPixelTodBm(m_panRect, m_dBmPanMax, m_dBmPanMin, y);
	dBstr = QString::number(dBm, 'f', 1) + " dBm";

	int rectWidth;
	int fontHeight;
	if (m_smallSize) {

		rectWidth = m_fonts.smallFontMetrics->boundingRect(fstr).width();
		fontHeight = m_fonts.smallFontMetrics->tightBoundingRect("0").height() + spacing;
	}
	else {

		rectWidth = m_fonts.bigFont1Metrics->width(fstr);
		fontHeight = m_fonts.bigFont1Metrics->tightBoundingRect("0").height() + spacing;
	}

	m_haircrossMaxRight = rectWidth + textOffset;
	m_smallSize ? m_haircrossMinTop = 40 : m_haircrossMinTop = 60;


	int tx, ty;
	if (x > m_panRect.width() - m_haircrossMaxRight) {

		tx = x - m_haircrossMaxRight;
		if (y > m_haircrossMinTop)
			m_smallSize ? ty = y - 42 : ty = y - 62;
		else
			m_smallSize ? ty = y + 10 : ty = y + 30;
	}
	else {

		tx = x + textOffset;
		if (y > m_haircrossMinTop)
			m_smallSize ? ty = y - 42 : ty = y - 62;
		else
			m_smallSize ? ty = y + 10 : ty = y + 30;
	}

	// delta frequency and frequency
	if (m_smallSize) {

		m_oglTextSmall->renderText(tx, ty, 5.0f, dFstr);
		qglColor(QColor(255, 255, 255, 255));
		m_oglTextSmall->renderText(tx, ty + fontHeight, 5.0f, fstr);
	}
	else {

		m_oglTextBig1->renderText(tx, ty, 5.0f, dFstr);
		qglColor(QColor(255, 255, 255, 255));
		m_oglTextBig1->renderText(tx, ty + fontHeight, 5.0f, fstr);
	}


	// dBm value
	if (m_mouseRegion == panadapterRegion) {

		if (m_smallSize)
			m_oglTextSmall->renderText(tx, ty + 2*fontHeight, 5.0f, dBstr);
		else
			m_oglTextBig1->renderText(tx, ty + 2*fontHeight, 5.0f, dBstr);
	}

	// Ham band text
	if (m_oldMousePosX != m_mousePos.x()) {

		m_bandText = getHamBandTextString(set->getHamBandTextList(), true, frequency);
		m_oldMousePosX = m_mousePos.x();
	}

	glColor3f(0.94f, 0.82f, 0.43f);
	if (m_smallSize)
		m_oglTextSmall->renderText(tx, ty + 4*fontHeight, 5.0f, m_bandText);
	else
		m_oglTextBig1->renderText(tx, ty + 5*fontHeight, 5.0f, m_bandText);


	glDisable(GL_SCISSOR_TEST);
	glEnable(GL_MULTISAMPLE);
}

//void QGLReceiverPanel::drawReceiverInfo() {
//
//	// receiver number
//	QColor col;
//	if (m_dataEngineState == QSDR::DataEngineUp) {
//
//		if (m_receiver == m_currentReceiver)
//			qglColor(QColor(240, 210, 110));
//		else
//			qglColor(QColor(120, 88, 13));
//	}
//	else
//		qglColor(QColor(68, 68, 68));
//
//	QString str;
//	str = QString("Rx %1").arg(m_receiver + 1);
//
//	int x1 = m_panRect.width() - 30;
//	int y1 = 3;
//
//	m_oglTextSmall->renderText(x1+3, y1-1, str);
//
//
//	// DSP core
//	if (m_dataEngineState == QSDR::DataEngineUp) {
//
//		if (m_receiver == m_currentReceiver)
//			qglColor(QColor(140, 210, 110));
//		else
//			qglColor(QColor(20, 88, 13));
//	}
//	else
//		qglColor(QColor(68, 68, 68));
//
//
//	x1 -= smallFontMetrics->tightBoundingRect(str).width() + 8;
//	y1 = 3;
//	m_oglTextSmall->renderText(x1+3, y1-1, str);
//
//
//	// mouse wheel freq step size
//	if (m_dataEngineState == QSDR::DataEngineUp) {
//
//		if (m_receiver == m_currentReceiver)
//			qglColor(QColor(1, 190, 180));
//		else
//			qglColor(QColor(1, 150, 140));
//	}
//	else
//		qglColor(QColor(68, 68, 68));
//
//	str = "%1";
//	str = str.arg(set->getValue1000(m_rxDataList.at(m_receiver).mouseWheelFreqStep, 0, "Hz"));
//
//	x1 -= smallFontMetrics->tightBoundingRect(str).width() + 8;
//	y1 = 3;
//
//	m_oglTextSmall->renderText(x1+3, y1-1, str);
//
//
//	// main frequency display
//	if (m_panRect.height() > 15) {
//
//		int alpha;
//		if (m_dataEngineState == QSDR::DataEngineUp) {
//
//			if (m_receiver == set->getCurrentReceiver())
//				alpha = 255;
//			else
//				alpha = 155;
//		}
//		else
//			alpha = 100;
//
//		TFrequency f;
//		f.freqMHz = (int)(m_frequency / 1000);
//		f.freqkHz = (int)(m_frequency % 1000);
//
//		str = "%1.%2";
//		int f1 = f.freqMHz;
//		int f2 = f.freqkHz;
//
//		GLint x = m_panRect.width()/2 - 65;
//
//		qglColor(QColor(0, 0, 0, alpha));
//		m_oglTextFreq1->renderText(x+2, 7, 4.0f, str.arg(f1/1000).arg(f1 - 1000 * (int)(f1/1000), 3, 10, QLatin1Char('0')));
//		//m_oglTextFreq1->renderText(x+2, 22, 4.0f, str.arg(f1/1000).arg(f1 - 1000 * (int)(f1/1000), 3, 10, QLatin1Char('0')));
//		qglColor(QColor(255, 255, 255, alpha));
//		m_oglTextFreq1->renderText(x, 5, 5.0f, str.arg(f1/1000).arg(f1 - 1000 * (int)(f1/1000), 3, 10, QLatin1Char('0')));
//		//m_oglTextFreq1->renderText(x, 20, 5.0f, str.arg(f1/1000).arg(f1 - 1000 * (int)(f1/1000), 3, 10, QLatin1Char('0')));
//
//		str = "%1";
//		if (f1 / 1000 < 10) {
//
//			qglColor(QColor(0, 0, 0, alpha));
//			m_oglTextFreq2->renderText(x + 36, 7, 4.0f, str.arg(f2, 3, 10, QLatin1Char('0')));
//			m_oglTextFreq1->renderText(x + 60, 7, 4.0f, "MHz");
//
//			qglColor(QColor(255, 255, 255, alpha));
//			m_oglTextFreq2->renderText(x + 34, 5, 5.0f, str.arg(f2, 3, 10, QLatin1Char('0')));
//			m_oglTextFreq1->renderText(x + 58, 5, 5.0f, "MHz");
//		}
//		else {
//
//			qglColor(QColor(0, 0, 0, alpha));
//			m_oglTextFreq2->renderText(x + 39, 7, 4.0f, str.arg(f2, 3, 10, QLatin1Char('0')));
//			m_oglTextFreq1->renderText(x + 65, 7, 4.0f, "MHz");
//
//			qglColor(QColor(255, 255, 255, alpha));
//			m_oglTextFreq2->renderText(x + 41, 5, 5.0f, str.arg(f2, 3, 10, QLatin1Char('0')));
//			m_oglTextFreq1->renderText(x + 63, 5, 5.0f, "MHz");
//		}
//	}
//}

void QGLReceiverPanel::drawReceiverInfo() {

	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);

	// mouse wheel freq step size
	QColor col;
	if (m_dataEngineState == QSDR::DataEngineUp) {

		if (m_receiver == m_currentReceiver)
			col = QColor(1, 190, 180);
		else
			col = QColor(1, 100, 90);
	}
	else
		col = QColor(48, 48, 48);

	QString str = "%1";
	str = str.arg(set->getValue1000(m_mouseWheelFreqStep, 0, "Hz"));

	int x1 = m_panRect.width() - (m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 9);
	int y1 = 3;

	QRect rect = QRect(x1+2, y1, m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 5, m_fonts.fontHeightSmallFont + 4);
	drawGLRect(rect, col, -2.0f);
	qglColor(QColor(0, 0, 0));
	m_oglTextSmall->renderText(x1+3, y1-1, str);


	// AGC mode
	if (m_dataEngineState == QSDR::DataEngineUp) {

		if (m_receiver == m_currentReceiver) {

			if (m_showAGCLines)
				col = QColor(255, 170, 90);
			else
				col = QColor(215, 130, 50);
		}
		else
			col = QColor(165, 80, 1);
	}
	else
		col = QColor(48, 48, 48);

	str = "%1";
	//str = str.arg(set->getAGCModeString(m_receiver));
	str = str.arg(m_agcModeString);

	x1 -= m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 7;
	y1 = 3;

	//rect = QRect(x1+2, y1, smallFontMetrics->tightBoundingRect(str).width() + 5, m_fontHeightSmallFont + 4);
	m_agcButtonRect = QRect(x1+2, y1, m_fonts.smallFontMetrics->tightBoundingRect(str).width() + 5, m_fonts.fontHeightSmallFont + 4);
	drawGLRect(m_agcButtonRect, col, -2.0f);
	qglColor(QColor(0, 0, 0));
	m_oglTextSmall->renderText(x1+3, y1-1, str);


	// main frequency display
	if (m_panRect.height() > 15) {

		int alpha;
		if (m_dataEngineState == QSDR::DataEngineUp) {

			if (m_receiver == set->getCurrentReceiver())
				alpha = 255;
			else
				alpha = 155;
		}
		else
			alpha = 100;

		TFrequency f;
		f.freqMHz = (int)(m_frequency / 1000);
		f.freqkHz = (int)(m_frequency % 1000);

		str = "%1.%2";
		int f1 = f.freqMHz;
		int f2 = f.freqkHz;

		GLint x = m_panRect.width()/2 - 65;

		qglColor(QColor(0, 0, 0, alpha));
		m_oglTextBig1->renderText(x+2, 7, 4.0f, str.arg(f1/1000).arg(f1 - 1000 * (int)(f1/1000), 3, 10, QLatin1Char('0')));
		//m_oglTextFreq1->renderText(x+2, 22, 4.0f, str.arg(f1/1000).arg(f1 - 1000 * (int)(f1/1000), 3, 10, QLatin1Char('0')));
		qglColor(QColor(255, 255, 255, alpha));
		m_oglTextBig1->renderText(x, 5, 5.0f, str.arg(f1/1000).arg(f1 - 1000 * (int)(f1/1000), 3, 10, QLatin1Char('0')));
		//m_oglTextFreq1->renderText(x, 20, 5.0f, str.arg(f1/1000).arg(f1 - 1000 * (int)(f1/1000), 3, 10, QLatin1Char('0')));

		str = "%1";
		if (f1 / 1000 < 10) {

			qglColor(QColor(0, 0, 0, alpha));
			m_oglTextBig2->renderText(x + 36, 7, 4.0f, str.arg(f2, 3, 10, QLatin1Char('0')));
			m_oglTextBig1->renderText(x + 60, 7, 4.0f, "MHz");

			qglColor(QColor(255, 255, 255, alpha));
			m_oglTextBig2->renderText(x + 34, 5, 5.0f, str.arg(f2, 3, 10, QLatin1Char('0')));
			m_oglTextBig1->renderText(x + 58, 5, 5.0f, "MHz");
		}
		else {

			qglColor(QColor(0, 0, 0, alpha));
			m_oglTextBig2->renderText(x + 39, 7, 4.0f, str.arg(f2, 3, 10, QLatin1Char('0')));
			m_oglTextBig1->renderText(x + 65, 7, 4.0f, "MHz");

			qglColor(QColor(255, 255, 255, alpha));
			m_oglTextBig2->renderText(x + 41, 5, 5.0f, str.arg(f2, 3, 10, QLatin1Char('0')));
			m_oglTextBig1->renderText(x + 63, 5, 5.0f, "MHz");
		}
	}
}

void QGLReceiverPanel::drawAGCControl() {

	glDisable(GL_MULTISAMPLE);
	glLineStipple(1, 0x0C0C);
	glEnable(GL_LINE_STIPPLE);
	glLineWidth(1.0f);

	glScissor(m_panRect.left(), size().height() - (m_panRect.top() + m_panRect.height()), m_panRect.left() + m_panRect.width(), m_panRect.height());
	glEnable(GL_SCISSOR_TEST);

	if (m_agcMode == (AGCMode) agcOFF) {

		m_agcFixedGainLevelPixel = dBmToGLPixel(m_panRect, m_dBmPanMax, m_dBmPanMin, -m_agcFixedGain);
		//GRAPHICS_DEBUG << "m_agcFixedGainLevelPixel = " << m_agcFixedGainLevelPixel;

		QString str = "AGC-F";
		qglColor(QColor(0, 0, 0, 255));
		m_oglTextSmall->renderText(m_panRect.right() - 32, m_agcFixedGainLevelPixel - 13, 4.0f, str);
		qglColor(QColor(225, 125, 225, 255));
		m_oglTextSmall->renderText(m_panRect.right() - 34, m_agcFixedGainLevelPixel - 15, 5.0f, str);

		// AGC fixed gain line
		glBegin(GL_LINES);
			qglColor(QColor(0, 0, 0, 255));
			glVertex3f(m_dBmScalePanRect.right() - 1, m_agcFixedGainLevelPixel + 2, 4.0f);
			glVertex3f(m_panRect.right() - 1, m_agcFixedGainLevelPixel, 4.0f);
			qglColor(QColor(225, 125, 225, 255));
			glVertex3f(m_dBmScalePanRect.right() - 3, m_agcFixedGainLevelPixel, 5.0f);
			glVertex3f(m_panRect.right() - 1, m_agcFixedGainLevelPixel, 4.0f);
		glEnd();

	}
	else {

		m_agcThresholdPixel = dBmToGLPixel(m_panRect, m_dBmPanMax, m_dBmPanMin, m_agcThresholdOld);

		if (m_agcHangEnabled)
			m_agcHangLevelPixel = dBmToGLPixel(m_panRect, m_dBmPanMax, m_dBmPanMin, m_agcHangLevelOld);

		QString str = "AGC-T";
		qglColor(QColor(0, 0, 0, 255));
		m_oglTextSmall->renderText(m_panRect.right() - 32, m_agcThresholdPixel - 13, 4.0f, str);
		qglColor(QColor(225, 125, 125, 255));
		m_oglTextSmall->renderText(m_panRect.right() - 34, m_agcThresholdPixel - 15, 5.0f, str);

		// AGC threshold line
		glBegin(GL_LINES);
			qglColor(QColor(0, 0, 0, 255));
			glVertex3f(m_dBmScalePanRect.right() - 1, m_agcThresholdPixel + 2, 4.0f);
			glVertex3f(m_panRect.right() - 1, m_agcThresholdPixel, 4.0f);
			qglColor(QColor(225, 125, 125, 255));
			glVertex3f(m_dBmScalePanRect.right() - 3, m_agcThresholdPixel, 5.0f);
			glVertex3f(m_panRect.right() - 1, m_agcThresholdPixel, 4.0f);
		glEnd();

		// AGC hang threshold line
		if (m_agcHangEnabled) {

			str = "AGC-H";
			qglColor(QColor(0, 0, 0, 255));
			m_oglTextSmall->renderText(m_panRect.right() - 32, m_agcHangLevelPixel - 13, 4.0f, str);
			qglColor(QColor(125, 225, 125, 255));
			m_oglTextSmall->renderText(m_panRect.right() - 34, m_agcHangLevelPixel - 15, 5.0f, str);

			glBegin(GL_LINES);
				qglColor(QColor(0, 0, 0, 255));
				glVertex3f(m_dBmScalePanRect.right() - 1, m_agcHangLevelPixel + 2, 4.0f);
				glVertex3f(m_panRect.right() - 1, m_agcHangLevelPixel, 4.0f);
				qglColor(QColor(125, 225, 125, 255));
				glVertex3f(m_dBmScalePanRect.right() - 3, m_agcHangLevelPixel, 5.0f);
				glVertex3f(m_panRect.right() - 1, m_agcHangLevelPixel, 4.0f);
			glEnd();
		}
	}

	glDisable(GL_SCISSOR_TEST);
	glDisable(GL_LINE_STIPPLE);
	glEnable(GL_MULTISAMPLE);
}
 
//**********************************************************************************************
// The algorithms of the scale functions renderPanVerticalScale() and renderPanHorizontalScale() 
// are taken from SDRMAXIII (c) Catherine Moss, with permission.

void QGLReceiverPanel::renderPanVerticalScale() {

	QString str;

	int spacing = 7;
	int fontHeight;
	int fontMaxWidth;
	if (m_smallSize) {

		fontHeight = m_fonts.smallFontMetrics->tightBoundingRect(".0dBm").height() + spacing;
		fontMaxWidth = m_fonts.smallFontMetrics->boundingRect("-000.0").width();
	}
	else {

		fontHeight = m_fonts.bigFont1Metrics->tightBoundingRect(".0dBm").height() + spacing;
		fontMaxWidth = m_fonts.bigFont1Metrics->boundingRect("-000.0").width();
	}


	GLint width = m_dBmScalePanRect.width();
	GLint height = m_dBmScalePanRect.height();

	qreal unit = (qreal)(height / qAbs(m_dBmPanMax - m_dBmPanMin));

	m_dBmScale = getYRuler2(m_dBmScalePanRect, fontHeight, unit, m_dBmPanMin, m_dBmPanMax);

	glClear(GL_COLOR_BUFFER_BIT);
	
	QRect textRect(0, 0, fontMaxWidth, fontHeight);
	if (m_smallSize)
		textRect.moveLeft(3);
	else
		textRect.moveLeft(-1);

	int yOld = -textRect.height();

	int len		= m_dBmScale.mainPointPositions.length();
	int sublen	= m_dBmScale.subPointPositions.length();
	
	glViewport(0, 0, width, height);
	setProjectionOrthographic(width, height);

	// draw the scale background
	drawGLScaleBackground(QRect(0, 0, width, height), QColor(30, 30, 30, 180));
	
	if (len > 0) {

		glColor3f(0.65f, 0.76f, 0.81f);
		glLineWidth(1);

		glBegin(GL_LINES);
		for (int i = 0; i < len; i++) {

			glVertex3f(width,     m_dBmScale.mainPointPositions.at(i), 0.0f);	// origin of the line
			glVertex3f(width - 4, m_dBmScale.mainPointPositions.at(i), 0.0f);	// ending point of the line
		}
		glEnd();
		
		glColor3f(0.45f, 0.56f, 0.61f);
		if (sublen > 0) {

			glBegin(GL_LINES);
			for (int i = 0; i < sublen; i++) {

				glVertex3f(width,     m_dBmScale.subPointPositions.at(i), 0.0f);	// origin of the line
				glVertex3f(width - 2, m_dBmScale.subPointPositions.at(i), 0.0f);	// ending point of the line
			}
			glEnd();
		}

		glColor3f(0.75f, 0.86f, 0.91f);
		for (int i = 0; i < len; i++) {

			textRect.moveBottom(m_dBmScale.mainPointPositions.at(i) + textRect.height()/2);
			
			if (textRect.y() >= yOld && textRect.bottom() <= (m_dBmScalePanRect.height() - textRect.height())) {
			
				str = QString::number(m_dBmScale.mainPoints.at(i), 'f', 1);
				if (m_smallSize)
					m_oglTextSmall->renderText(textRect.x() + fontMaxWidth - m_fonts.smallFontMetrics->tightBoundingRect(str).width(), textRect.y(), str);
				else
					m_oglTextBig1->renderText(textRect.x() + fontMaxWidth - m_fonts.bigFont1Metrics->tightBoundingRect(str).width(), textRect.y(), str);
				yOld = textRect.bottom();
			}
		}
	}

	textRect.moveTop(m_dBmScalePanRect.height() - textRect.height());
	glColor3f(0.94f, 0.22f, 0.43f);
	
	str = QString("dBm");
	if (m_smallSize)
		m_oglTextSmall->renderText(textRect.x(), textRect.y(), str);
	else
		m_oglTextBig1->renderText(textRect.x() + 10, textRect.y(), str);

	glViewport(0, 0, size().width(), size().height());
	setProjectionOrthographic(size().width(), size().height());
}

void QGLReceiverPanel::renderPanHorizontalScale() {

	//GRAPHICS_DEBUG << "render frequency scale";
	int fontHeight;
	int fontMaxWidth;
	if (m_smallSize) {

		fontHeight = m_fonts.smallFontMetrics->tightBoundingRect(".0kMGHz").height();
		fontMaxWidth = m_fonts.smallFontMetrics->boundingRect("000.000.0").width();
	}
	else {

		fontHeight = m_fonts.bigFont1Metrics->tightBoundingRect(".0kMGHz").height();
		fontMaxWidth = m_fonts.bigFont1Metrics->boundingRect("000.000.0").width();
	}

	qreal freqSpan = (qreal)(m_sampleRate * m_freqScaleZoomFactor);
	qreal lowerFreq = (qreal)m_frequency - freqSpan / 2;
	qreal upperFreq = (qreal)m_frequency + freqSpan / 2;
	qreal unit = (qreal)(m_freqScalePanRect.width() / freqSpan);

	m_frequencyScale = getXRuler(m_freqScalePanRect, fontMaxWidth, unit, lowerFreq, upperFreq);

	// draw the frequency scale
	int		offset_X		= -1;
	int		textOffset_y	= 5;
	double	freqScale		= 1;

	QString fstr = QString(" Hz ");
	if (upperFreq >= 1e6) { freqScale = 1e6; fstr = QString("  MHz "); }
	else
	if (upperFreq >= 1e3) { freqScale = 1e3; fstr = QString("  kHz "); }

	// draw the scale background
	drawGLScaleBackground(QRect(0, 0, m_freqScalePanRect.width(), m_freqScalePanRect.height()), QColor(0, 0, 0, 255));
	
	QRect scaledTextRect(0, textOffset_y, 1, m_freqScalePanRect.height());

	if (m_smallSize)
		scaledTextRect.setWidth(m_fonts.smallFontMetrics->width(fstr));
	else
		scaledTextRect.setWidth(m_fonts.bigFont1Metrics->width(fstr));

	scaledTextRect.moveLeft(m_freqScalePanRect.width() - scaledTextRect.width());

	glColor3f(0.65f, 0.76f, 0.81f);
	int len = m_frequencyScale.mainPointPositions.length();
	if (len > 0) {

		glLineWidth(3);
		glBegin(GL_LINES);
		for (int i = 0; i < len; i++) {

			glVertex3f(m_frequencyScale.mainPointPositions.at(i), 1.0f, 0.0f);
			glVertex3f(m_frequencyScale.mainPointPositions.at(i), 4.0f, 0.0f);
		}
		glEnd();
		
		for (int i = 0; i < len; i++) {
		
			QString str = QString::number(m_frequencyScale.mainPoints.at(i) / freqScale, 'f', 3);
			
			if (i > 0) {

				double delta = m_frequencyScale.mainPoints.at(i) - m_frequencyScale.mainPoints.at(i-1);
				if (delta < 1000.0)
					str = QString::number(m_frequencyScale.mainPoints.at(i) / freqScale, 'f', 4);
				else
				if (freqScale == 1e3)
					while (str.endsWith('0')) str.remove(str.size() - 1, 1);
			}
			if (str.endsWith('.')) str.remove(str.size() - 1, 1);

			int textWidth;
			if (m_smallSize)
				textWidth = m_fonts.smallFontMetrics->width(str);
			else
				textWidth = m_fonts.bigFont1Metrics->width(str);
			QRect textRect(m_frequencyScale.mainPointPositions.at(i) + offset_X - (textWidth / 2), textOffset_y, textWidth, fontHeight);

			if (textRect.left() < 0 || textRect.right() >= scaledTextRect.left()) continue;
			
			if (m_smallSize)
				m_oglTextSmall->renderText(textRect.x(), textRect.y(), str);
			else
				m_oglTextBig1->renderText(textRect.x(), textRect.y(), str);
		}
	}

	if (m_frequencyScale.subPointPositions.length() > 0) {

		glLineWidth(1);
		glBegin(GL_LINES);
		for (int i = 0; i < m_frequencyScale.subPointPositions.length(); i++) {

			glVertex3f(m_frequencyScale.subPointPositions.at(i), 1.0f, 0.0f);
			glVertex3f(m_frequencyScale.subPointPositions.at(i), 3.0f, 0.0f);
		}
		glEnd();
	}

	glColor3f(0.94f, 0.22f, 0.43f);

	if (m_smallSize)
		m_oglTextSmall->renderText(m_freqScalePanRect.width() - 30, textOffset_y, fstr);
	else
		m_oglTextBig1->renderText(m_freqScalePanRect.width() - 33, textOffset_y, fstr);
}

void QGLReceiverPanel::renderPanadapterGrid() {

	//GRAPHICS_DEBUG << "render panadapter grid";
	glClear(GL_COLOR_BUFFER_BIT);
	glLineStipple(1, 0x9999);
	//glLineStipple(1, 0xCCCC);
	glEnable(GL_LINE_STIPPLE);
	glLineWidth(1.0f);

	// vertical lines
	int len = m_frequencyScale.mainPointPositions.length();
	if (len > 0) {

		GLint x1 = m_panRect.left();
		GLint x2 = 1;
		if (m_dBmScalePanRect.isValid()) x2 += m_dBmScalePanRect.width();

		//GLint y1 = rect.top() + 1;
		GLint y1 = 1;
		GLint y2 = m_panRect.bottom() - 1;

		TGL2int *vertexArray = new TGL2int[len * 2];
		int vertexArrayLen = 0;

		TGL2int point1, point2;
		point1.y = y1;
		point2.y = y2;
		for (int i = 0; i < len; i++) {

			GLint x = m_frequencyScale.mainPointPositions.at(i);
			if (x < x2) continue;
			x += x1;
			point1.x = x;
			point2.x = x;
			vertexArray[vertexArrayLen++] = point1;
			vertexArray[vertexArrayLen++] = point2;
		}

		glEnableClientState(GL_VERTEX_ARRAY);
		glVertexPointer(2, GL_INT, 0, vertexArray);
		glDrawArrays(GL_LINES, 0, vertexArrayLen);
		glDisableClientState(GL_VERTEX_ARRAY);

		delete[] vertexArray;
	}

	// horizontal lines
	len = m_dBmScale.mainPointPositions.length();
	if (len > 0) {

		TGL2float *vertexArray = new TGL2float[len * 2];
		int vertexArrayLen = 0;

		TGL2float point1, point2;
		point1.x = m_panRect.left() + m_dBmScalePanRect.width();
		point2.x = m_panRect.right();
		
		for (int i = 0; i < len; i++) {

			GLfloat y = m_dBmScale.mainPointPositions.at(i);
			
			point1.y = y;
			point2.y = y;
			
			vertexArray[vertexArrayLen++] = point1;
			vertexArray[vertexArrayLen++] = point2;
		}

		glEnableClientState(GL_VERTEX_ARRAY);
		glVertexPointer(2, GL_FLOAT, 0, vertexArray);
		glDrawArrays(GL_LINES, 0, vertexArrayLen);
		glDisableClientState(GL_VERTEX_ARRAY);

		delete[] vertexArray;

	}
	glDisable(GL_LINE_STIPPLE);
}
 
void QGLReceiverPanel::renderWaterfallVerticalScale() {

	QString str;

	int spacing = 7;
	int fontHeight;
	int fontMaxWidth;
	if (m_smallSize) {

		fontHeight = m_fonts.smallFontMetrics->tightBoundingRect(".0s").height() + spacing;
		fontMaxWidth = m_fonts.smallFontMetrics->boundingRect("000.0").width();
	}
	else {

		fontHeight = m_fonts.bigFont1Metrics->tightBoundingRect(".0s").height() + spacing;
		fontMaxWidth = m_fonts.bigFont1Metrics->boundingRect("000.0").width();
	}

	GLint width = m_secScaleWaterfallRect.width();
	GLint height = m_secScaleWaterfallRect.height();

	qreal unit = (qreal)(height / qAbs(m_secWaterfallMax - m_secWaterfallMin));

	m_secScale = getYRuler2(m_secScaleWaterfallRect, fontHeight, unit, m_secWaterfallMin, m_secWaterfallMax);
	//m_secScale = getYRuler3(m_secScaleWaterfallRect, fontHeight, unit, m_secWaterfallMin, m_secWaterfallMax, 10.0f);

	glClear(GL_COLOR_BUFFER_BIT);

	QRect textRect(0, 0, fontMaxWidth, fontHeight);
	if (m_smallSize)
		textRect.moveLeft(3);
	else
		textRect.moveLeft(-1);

	int yOld = -textRect.height();

	int len		= m_secScale.mainPointPositions.length();
	int sublen	= m_secScale.subPointPositions.length();

	glViewport(0, 0, width, height);
	setProjectionOrthographic(width, height);

	// draw the scale background
	drawGLScaleBackground(QRect(0, 0, width, height), QColor(40, 40, 40, 180));

	if (len > 0) {

		glColor3f(0.65f, 0.76f, 0.81f);
		glLineWidth(1);

		glBegin(GL_LINES);
		for (int i = 0; i < len; i++) {

			glVertex3f(width,     m_secScale.mainPointPositions.at(i), 0.0f);	// origin of the line
			glVertex3f(width - 4, m_secScale.mainPointPositions.at(i), 0.0f);	// ending point of the line
		}
		glEnd();

		glColor3f(0.45f, 0.56f, 0.61f);
		if (sublen > 0) {

			glBegin(GL_LINES);
			for (int i = 0; i < sublen; i++) {

				glVertex3f(width,     m_secScale.subPointPositions.at(i), 0.0f);	// origin of the line
				glVertex3f(width - 2, m_secScale.subPointPositions.at(i), 0.0f);	// ending point of the line
			}
			glEnd();
		}

		glColor3f(0.95f, 0.96f, 0.91f);
		for (int i = 0; i < len; i++) {

			textRect.moveBottom(m_secScale.mainPointPositions.at(i) + textRect.height()/2);

			if (textRect.y() >= yOld && textRect.bottom() <= (height - textRect.height())) {

				str = QString::number(m_secScale.mainPoints.at(i), 'f', 1);
				if (m_smallSize)
					m_oglTextSmall->renderText(textRect.x() + fontMaxWidth - m_fonts.smallFontMetrics->tightBoundingRect(str).width(), textRect.y(), str);
				else
					m_oglTextBig1->renderText(textRect.x() + fontMaxWidth - m_fonts.bigFont1Metrics->tightBoundingRect(str).width(), textRect.y(), str);
				yOld = textRect.bottom();
			}
		}
	}

	textRect.moveTop(height - textRect.height());
	glColor3f(0.94f, 0.22f, 0.43f);

	str = QString("sec");
	if (m_smallSize)
		m_oglTextSmall->renderText(textRect.x(), textRect.y(), str);
	else
		m_oglTextBig1->renderText(textRect.x() + 10, textRect.y(), str);

	glViewport(0, 0, size().width(), size().height());
	setProjectionOrthographic(size().width(), size().height());
}

//********************************************************************

void QGLReceiverPanel::getRegion(QPoint p) {

	//QRect mouse_rect(0, 0, 100, 100);
	//mouse_rect.moveCenter(p);

	if (m_agcButtonRect.contains(p)) {

		m_mouseRegion = agcButtonRegion;

		/*if (m_displayTime.elapsed() >= 50) {

			m_displayTime.restart();
			update();
		}*/
	}
	else if (m_freqScalePanRect.contains(p)) {

		m_mouseRegion = freqScalePanadapterRegion;
		
		/*if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}*/
	}
	else if (m_dBmScalePanRect.contains(p)) {

		m_mouseRegion = dBmScalePanadapterRegion;

		/*if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}*/
	}
	else if (qAbs(p.x() - m_filterRect.left()) < m_snapMouse &&
			 m_panRect.contains(p)
	) {
		m_mouseRegion = filterRegionLow;
		m_mouseDownFilterFrequencyLo = m_filterLowerFrequency;
	}
	else if (qAbs(p.x() - m_filterRect.right()) < m_snapMouse &&
			 m_panRect.contains(p)
	) {
		m_mouseRegion = filterRegionHigh;
		m_mouseDownFilterFrequencyHi = m_filterUpperFrequency;
	}
	else if (m_filterRect.contains(p)) {

		m_mouseRegion = filterRegion;

		/*if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}*/
	}
	else if (qAbs(p.y() - m_agcThresholdPixel) < m_snapMouse && !m_crossHairCursor) {

		m_mouseRegion = agcThresholdLine;
		m_mouseDownAGCThreshold = m_agcThresholdOld;
	}
	else if (qAbs(p.y() - m_agcHangLevelPixel) < m_snapMouse && !m_crossHairCursor) {

		m_mouseRegion = agcHangLine;
		m_mouseDownAGCHangLevel = m_agcHangLevelOld;
	}
	else if (qAbs(p.y() - m_agcFixedGainLevelPixel) < m_snapMouse && !m_crossHairCursor) {

		m_mouseRegion = agcFixedGainLine;
		m_mouseDownFixedGainLevel = -m_agcFixedGain;
	}
	else if (m_panRect.contains(p)) {

		m_mouseRegion = panadapterRegion;

		/*if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}*/
	}
	else if (m_waterfallRect.contains(p)) {

		m_mouseRegion = waterfallRegion;

		/*if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}*/
	}
	else
		m_mouseRegion = elsewhere;

	//GRAPHICS_DEBUG << "region" << m_mouseRegion;
}

void QGLReceiverPanel::resizeGL(int iWidth, int iHeight) {

	int width = (int)(iWidth/2) * 2;
	int height = iHeight;

	if (width != m_oldWidth) {

		m_freqScalePanadapterRenew = true;
		m_panGridRenew = true;

		m_oldWidth = width;
	}

	m_spectrumVertexColorUpdate = true;
	m_waterfallUpdate = true;
	m_peakHoldBufferResize = true;
	
	glFinish();

	m_resizeTime.restart();
	setupDisplayRegions(QSize(width, height));
    glViewport(0, 0, (GLsizei)width, (GLsizei)height);

	setProjectionOrthographic(width, height);
}

void QGLReceiverPanel::setupDisplayRegions(QSize size) {

	m_displayTop = 0;
	int freqScaleRectHeight = 20;
	//int dBmScaleWidth = 45;
	
	m_freqScalePanRect = QRect(
			0, 
			m_displayTop + qRound((size.height() - m_displayTop - freqScaleRectHeight) * m_freqRulerPosition), 
			size.width(), 
			freqScaleRectHeight);

	m_panRect = QRect(
			0,
			m_displayTop, 
			size.width(),
			m_freqScalePanRect.top() - m_displayTop);


	if (m_panRect.height() != m_oldPanRectHeight) {

		m_dBmScalePanadapterRenew = true;
		m_panGridRenew = true;
		m_secScaleWaterfallRenew = true;
	}

	m_oldPanRectHeight = m_panRect.height();
	m_panRectWidth = (GLint)m_panRect.width();
	m_displayCenterlineHeight = m_panRect.top() + (m_panRect.height() - 3);
	
	m_waterfallRect = QRect(
			m_freqScalePanRect.left(), 
			m_freqScalePanRect.top() + m_freqScalePanRect.height(),
			//m_freqScalePanRect.top(),
			m_freqScalePanRect.width(),
			//size.height() - m_displayTop - m_freqScalePanRect.top() - m_freqScalePanRect.height());
			size.height() - m_freqScalePanRect.top() - m_freqScalePanRect.height());
			//size.height() - m_freqScalePanRect.top());	
			
	m_waterfallUpdate = true;

	if ((m_panRect.height() + m_waterfallRect.height()) > m_bigHeight && m_panRect.width() > m_bigWidth)
		m_smallSize = false;
	else
		m_smallSize = true;
	
	m_dBmScalePanRect = QRect(
						m_panRect.left(), 
						m_displayTop, 
						45, 
						m_panRect.height());

	m_secScaleWaterfallRect = QRect(
								m_waterfallRect.left(),
								m_freqScalePanRect.top() + m_freqScalePanRect.height(),
								45,
								m_waterfallRect.height());

	m_secWaterfallMin = -(1.0/m_fps) * m_secScaleWaterfallRect.height();

	m_freqScalePanadapterUpdate = true;
	m_dBmScalePanadapterUpdate = true;
	m_secScaleWaterfallUpdate = true;
	m_panGridUpdate = true;
	
//	GRAPHICS_DEBUG << "***************************************************************************";
//	GRAPHICS_DEBUG << "receiver:" << m_receiver;
//	GRAPHICS_DEBUG << "total size" << size.height();
//	GRAPHICS_DEBUG << "sizes (top, bottom, height):";
//	GRAPHICS_DEBUG << "panRect" << m_panRect.top() << m_panRect.bottom() << m_panRect.height();
//	//GRAPHICS_DEBUG << "panRect (H/W): " << m_panRect.height() << ": " << m_panRect.width();
//	GRAPHICS_DEBUG << "waterfallRect" << m_waterfallRect.top() << m_waterfallRect.bottom() << m_waterfallRect.height();
//	GRAPHICS_DEBUG << "freqScalePanRect" << m_freqScalePanRect.top() << m_freqScalePanRect.bottom() << m_freqScalePanRect.height();
//	GRAPHICS_DEBUG << "dBmScalePanRect" << m_dBmScalePanRect.top() << m_dBmScalePanRect.bottom() << m_dBmScalePanRect.height();
//	GRAPHICS_DEBUG << "";
	
}

void QGLReceiverPanel::saveGLState() {

    glPushAttrib(GL_ALL_ATTRIB_BITS);
    glMatrixMode(GL_PROJECTION);
    glPushMatrix();
    glMatrixMode(GL_MODELVIEW);
    glPushMatrix();
}

void QGLReceiverPanel::restoreGLState() {

    glMatrixMode(GL_PROJECTION);
    glPopMatrix();
    glMatrixMode(GL_MODELVIEW);
    glPopMatrix();
    glPopAttrib();
}
 
void QGLReceiverPanel::showText(float x, float y, float z = 0.0f, const QString &text = "", bool smallText = true) {

	if (smallText)
		m_oglTextSmall->renderText(x, y, z, text);
	else
		m_oglTextBig1->renderText(x, y, z, text);
}

//********************************************************************
// HMI control
 
void QGLReceiverPanel::enterEvent(QEvent *event) {

	setFocus(Qt::MouseFocusReason);

	m_mousePos = QPoint(-1, -1);
	m_mouseRegion = elsewhere;
	update();

	QGLWidget::enterEvent(event);
}

void QGLReceiverPanel::leaveEvent(QEvent *event) {

	m_mousePos = QPoint(-1, -1);
	m_mouseRegion = elsewhere;
	update();

	QGLWidget::leaveEvent(event);
}

void QGLReceiverPanel::wheelEvent(QWheelEvent* event) {
	
	//GRAPHICS_DEBUG << "wheelEvent";
	QPoint pos = event->pos();

	if (event->buttons() == Qt::NoButton) getRegion(pos);

	double freqStep = set->getMouseWheelFreqStep(m_currentReceiver);

	switch (m_mouseRegion) {

		case panadapterRegion:
		case waterfallRegion:

			double delta = 0;
			if (event->delta() < 0) delta = -freqStep;
			else
			if (event->delta() > 0) delta =  freqStep;

			if (m_frequency + delta > MAXFREQUENCY)
				m_frequency = MAXFREQUENCY;
			else
			if (m_frequency + delta < 0)
				m_frequency = 0;
			else
				// snap to the frequency step
				m_frequency = (long)(qRound((m_frequency + delta) / qAbs(freqStep)) * qAbs(freqStep));

			set->setFrequency(this, true, m_receiver, m_frequency);

			/*if (m_spectrumAveragingOld) {

				m_spectrumAveragingOld = true;
				m_spectrumAveraging = false;

				freqChangeTimer.restart();
			}*/
			break;
	}

 	//updateGL();
	update();
}

void QGLReceiverPanel::mousePressEvent(QMouseEvent* event) {
	
	//GRAPHICS_DEBUG << "mousePressEvent";
	m_mousePos = event->pos();
	m_mouseDownPos = m_mousePos;

	getRegion(m_mousePos);

	if (m_mouseRegion == agcButtonRegion) {

		if (event->buttons() == Qt::LeftButton) {

			if (m_showAGCLines) {

				m_showAGCLines = false;
				set->setAGCShowLines(this, m_receiver, false);
			}
			else {

				m_showAGCLines = true;
				set->setAGCShowLines(this, m_receiver, true);
			}
		}
	}
	else if (m_mouseRegion == panadapterRegion) {

		if (event->buttons() == Qt::RightButton) {

			if (m_crossHairCursor) {

				m_crossHairCursor = false;
				setCursor(Qt::ArrowCursor);
			}
			else {

				m_crossHairCursor = true;
				setCursor(Qt::BlankCursor);
			}	
		}
		else if (event->buttons() == Qt::LeftButton && m_crossHairCursor) {

			double freqStep = set->getMouseWheelFreqStep(m_currentReceiver);
			int dx = m_panRect.width()/2 - m_mousePos.x();
			qreal unit = (qreal)((m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width());
			m_frequency -= unit * dx;

			m_frequency = (long)(qRound(m_frequency / qAbs(freqStep)) * qAbs(freqStep));
				
			set->setFrequency(this, true, m_receiver, m_frequency);
			update();
		}
		else if (event->buttons() == Qt::LeftButton) {

			if (m_receiver != set->getCurrentReceiver())
				//set->setReceiver(this, m_receiver);
				set->setCurrentReceiver(this, m_receiver);
		}
	}
	else if (m_mouseRegion == filterRegion) {

		//setCursor(Qt::ArrowCursor);
		if (event->buttons() == Qt::LeftButton)
			m_highlightFilter = true;
	}
	else if (m_mouseRegion == freqScalePanadapterRegion) {

		m_rulerMouseDownPos = m_freqScalePanRect.topLeft();
		
		if (event->buttons() == Qt::RightButton) setCursor(Qt::SplitHCursor);
		update();

		return;
	}
	else if (m_mouseRegion == dBmScalePanadapterRegion) {

		m_rulerMouseDownPos = m_dBmScalePanRect.topLeft();

		if (event->buttons() == Qt::RightButton) 
			setCursor(Qt::SplitVCursor);
		update();

		return;
	}
	else if (m_mouseRegion == waterfallRegion) {

		m_rulerMouseDownPos = m_waterfallRect.topLeft();

		if (event->buttons() == Qt::LeftButton && m_crossHairCursor) {

			double freqStep = set->getMouseWheelFreqStep(m_currentReceiver);
			int dx = m_panRect.width()/2 - m_mousePos.x();
			qreal unit = (qreal)((m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width());
			m_frequency -= unit * dx;

			m_frequency = (long)(qRound(m_frequency / qAbs(freqStep)) * qAbs(freqStep));
				
			set->setFrequency(this, true, m_receiver, m_frequency);
			update();
		}		
		update();

		return;
	}
 	update();
}

void QGLReceiverPanel::mouseReleaseEvent(QMouseEvent *event) {

	//GRAPHICS_DEBUG << "mouseReleaseEvent";
	m_mousePos = event->pos();
	m_mouseDownPos = m_mousePos;

	getRegion(m_mousePos);

	if (m_mouseRegion == freqScalePanadapterRegion) {

		return;
	}
	update();
}

void QGLReceiverPanel::mouseDoubleClickEvent(QMouseEvent *event) {

	//GRAPHICS_DEBUG << "mouseDoubleClickEvent";
	m_mousePos = event->pos();
	m_mouseDownPos = m_mousePos;

	getRegion(m_mousePos);

	if (m_mouseRegion == panadapterRegion) {

		if (event->buttons() == Qt::LeftButton) {

			set->showRadioPopupWidget();
//			if (!band160mBtn->isVisible())
//				band160mBtn->show();
//			else
//				band160mBtn->hide();
		}
	}
}

void QGLReceiverPanel::mouseMoveEvent(QMouseEvent* event) {
	
	//GRAPHICS_DEBUG << "mouseMoveEvent";
	QPoint pos = event->pos();
	m_mousePos = event->pos();

	if (event->buttons() == Qt::NoButton) getRegion(pos);

	switch (m_mouseRegion) {

		case agcThresholdLine:

			if (!m_showAGCLines || (m_agcMode == (AGCMode) agcOFF))
				break;

			setCursor(Qt::SizeVerCursor);

			if (event->buttons() == Qt::LeftButton) {

				QPoint dPos = m_mouseDownPos - pos;

				qreal unit = qAbs(m_dBmPanMax - m_dBmPanMin) / m_panRect.height();
				qreal dAGCThreshold =  dPos.y() * unit;

				m_agcThresholdNew = m_mouseDownAGCThreshold + dAGCThreshold;
				if (m_agcThresholdNew > m_dBmPanMax-2)
					m_agcThresholdNew = m_dBmPanMax-2;

				if (m_agcThresholdNew < m_dBmPanMin+2)
					m_agcThresholdNew = m_dBmPanMin+2;

				set->setAGCThreshold_dB(this, m_receiver, m_agcThresholdNew);
			}
			break;

		case agcHangLine:

			if (!m_showAGCLines || (m_agcMode == (AGCMode) agcOFF))
				break;

			setCursor(Qt::SizeVerCursor);

			if (event->buttons() == Qt::LeftButton) {

				QPoint dPos = m_mouseDownPos - pos;

				qreal unit = qAbs(m_dBmPanMax - m_dBmPanMin) / m_panRect.height();
				qreal dAGCThreshold =  dPos.y() * unit;

				m_agcHangLevelNew = m_mouseDownAGCHangLevel + dAGCThreshold;

				if (m_agcHangLevelNew > m_dBmPanMax-2)
					m_agcHangLevelNew = m_dBmPanMax-2;

				if (m_agcHangLevelNew < m_dBmPanMin+2)
					m_agcHangLevelNew = m_dBmPanMin+2;

				set->setAGCHangLevel_dB(this, m_receiver, m_agcHangLevelNew);
				//GRAPHICS_DEBUG << "set m_agcHangLevelNew = " << m_agcHangLevelNew;
			}
			break;

		case agcFixedGainLine:

			if (!m_showAGCLines || (m_agcMode != (AGCMode) agcOFF))
				break;

			setCursor(Qt::SizeVerCursor);

			if (event->buttons() == Qt::LeftButton) {

				QPoint dPos = m_mouseDownPos - pos;

				qreal unit = qAbs(m_dBmPanMax - m_dBmPanMin) / m_panRect.height();
				qreal dAGCFixedGain =  dPos.y() * unit;

				qreal agcFixedGain = m_mouseDownFixedGainLevel + dAGCFixedGain;

				if (agcFixedGain > m_dBmPanMax-2)
					agcFixedGain = m_dBmPanMax-2;

				if (agcFixedGain < m_dBmPanMin+2)
					agcFixedGain = m_dBmPanMin+2;

				set->setAGCFixedGain_dB(this, m_receiver, -agcFixedGain);
			}
			break;

		case panadapterRegion:
			//GRAPHICS_DEBUG << "panadapterRegion Rx:" << m_receiver;
			if (m_crossHairCursor)
				setCursor(Qt::BlankCursor);
			else
				setCursor(Qt::ArrowCursor);

			if (event->buttons() == Qt::LeftButton) {

				QPoint dPos = m_mouseDownPos - pos;
				
				qreal unit = (qreal)((m_sampleRate * m_freqScaleZoomFactor) / m_freqScalePanRect.width());
				qreal deltaFreq = unit * dPos.x();
				
				long newFrequency = m_frequency + deltaFreq;
				if (newFrequency > MAXFREQUENCY)
					newFrequency = MAXFREQUENCY;
				else
				if (newFrequency + deltaFreq < 0)
					newFrequency = 0;
				else
					m_frequency += deltaFreq;

				set->setFrequency(this, true, m_receiver, m_frequency);
				m_mouseDownPos = pos;

				m_displayCenterlineHeight = m_panRect.top() + (m_panRect.height() - 3);

				m_showFilterLeftBoundary = false;
				m_showFilterRightBoundary = false;
				m_highlightFilter = false;

//				if (m_displayTime.elapsed() >= 50) {
//
//					m_displayTime.restart();
//					update();
//				}
			}
			m_displayCenterlineHeight = m_panRect.top() + (m_panRect.height() - 3);

			m_showFilterLeftBoundary = false;
			m_showFilterRightBoundary = false;
			m_highlightFilter = false;
			break;

		case waterfallRegion:
			//GRAPHICS_DEBUG << "waterfallRegion Rx:" << m_receiver;
			if (m_crossHairCursor)
				setCursor(Qt::BlankCursor);
			else
				setCursor(Qt::ArrowCursor);

			/*if (event->buttons() == Qt::LeftButton) {

				m_cameraAngle += (pos - m_mouseDownPos);
				m_mouseDownPos = pos;
			}
			else
			if (event->buttons() == Qt::RightButton) {

				m_cameraDistance += (pos.y() - m_mouseDownPos.y()) * 0.2f;
				m_mouseDownPos = pos;
			}
			setCursor(Qt::ArrowCursor);*/
			//update();
			break;

		case dBmScalePanadapterRegion:
			//GRAPHICS_DEBUG << "dBmScalePanadapterRegion";
			if (event->buttons() == Qt::LeftButton) {

				QPoint dPos = m_mouseDownPos - pos;
				qreal unit = (qreal)(qAbs(m_dBmPanMax - m_dBmPanMin) / m_panRect.height());
				
				qreal newMin = m_dBmPanMin - unit * dPos.y();
				qreal newMax = m_dBmPanMax - unit * dPos.y();

				if (newMin > MINDBM && newMax < MAXDBM) {

					m_dBmPanMin = newMin;
					m_dBmPanMax = newMax;

					set->setdBmPanScaleMin(m_receiver, m_dBmPanMin);
					set->setdBmPanScaleMax(m_receiver, m_dBmPanMax);
				}
				
				m_mouseDownPos = pos;
				m_dBmScalePanadapterUpdate = true;
				m_panGridUpdate = true;
				m_peakHoldBufferResize = true;

//				if (m_displayTime.elapsed() >= 50) {
//
//					m_displayTime.restart();
//					update();
//				}
			}
			else
			if (event->buttons() == Qt::RightButton &&
				event->modifiers() == Qt::ControlModifier) {

				QPoint dPos = m_mouseDownPos - pos;
				if (dPos.y() > 0)
					m_dBmPanDelta = 0.5f;
				else if (dPos.y() < 0)
					m_dBmPanDelta = -0.5f;
					
				m_dBmPanMin += m_dBmPanDelta;
				m_dBmPanMax -= m_dBmPanDelta;

				if (qAbs(m_dBmPanMax - m_dBmPanMin) < 10) {

					m_dBmPanMin -= m_dBmPanDelta;
					m_dBmPanMax += m_dBmPanDelta;
				}
				if (m_dBmPanMin < MINDBM) m_dBmPanMin = MINDBM;
				if (m_dBmPanMax > MAXDBM) m_dBmPanMax = MAXDBM;

				set->setdBmPanScaleMin(m_receiver, m_dBmPanMin);
				set->setdBmPanScaleMax(m_receiver, m_dBmPanMax);

				m_mouseDownPos = pos;
				m_dBmScalePanadapterUpdate = true;
				m_panGridUpdate = true;
				m_peakHoldBufferResize = true;

//				if (m_displayTime.elapsed() >= 50) {
//
//					m_displayTime.restart();
//					update();
//				}
			}
			if (event->buttons() == Qt::RightButton) {

				QPoint dPos = m_mouseDownPos - pos;
				if (dPos.y() > 0)
					m_dBmPanDelta = 0.5f;
				else if (dPos.y() < 0)
					m_dBmPanDelta = -0.5f;
					
				m_dBmPanMax -= m_dBmPanDelta;

				if (qAbs(m_dBmPanMax - m_dBmPanMin) < 10) {

					m_dBmPanMin -= m_dBmPanDelta;
					m_dBmPanMax += m_dBmPanDelta;
				}
				if (m_dBmPanMin < MINDBM) m_dBmPanMin = MINDBM;
				if (m_dBmPanMax > MAXDBM) m_dBmPanMax = MAXDBM;

				set->setdBmPanScaleMin(m_receiver, m_dBmPanMin);
				set->setdBmPanScaleMax(m_receiver, m_dBmPanMax);

				m_mouseDownPos = pos;
				m_dBmScalePanadapterUpdate = true;
				m_panGridUpdate = true;
				m_peakHoldBufferResize = true;

//				if (m_displayTime.elapsed() >= 50) {
//
//					m_displayTime.restart();
//					update();
//				}
			}
			else {

				setCursor(Qt::ArrowCursor);

//				if (m_displayTime.elapsed() >= 50) {
//
//					m_displayTime.restart();
//					update();
//				}
			}
			break;

		case freqScalePanadapterRegion:
			//GRAPHICS_DEBUG << "freqScalePanadapterRegion Rx" << m_receiver;
			if (event->buttons() == Qt::LeftButton) {
				
				QPoint dPos = m_mouseDownPos - pos;
				int bottom_y = height() - m_freqScalePanRect.height();
				int new_y = m_rulerMouseDownPos.y() - dPos.y();
				
				if (new_y < m_panRect.top() + m_panSpectrumMinimumHeight) 
					new_y = m_panRect.top() + m_panSpectrumMinimumHeight;
				if (new_y > bottom_y) 
					new_y = bottom_y;
				
				m_freqRulerPosition = (float)(new_y - m_panRect.top()) / (bottom_y - m_panRect.top());
				set->setFreqRulerPosition(m_freqRulerPosition, m_receiver);

//				if (m_displayTime.elapsed() >= 50) {
//
//					m_displayTime.restart();
//					update();
//				}
			}
			else
			if (event->buttons() == Qt::RightButton) {

				QPoint dPos = m_mouseDownPos - pos;
				if (dPos.x() > 0)
					m_freqScaleZoomFactor += 0.01;
				else if (dPos.x() < 0)
					m_freqScaleZoomFactor -= 0.01;

				if (m_freqScaleZoomFactor > 1.0) m_freqScaleZoomFactor = 1.0;
				//if (m_freqScaleZoomFactor < 0.05) m_freqScaleZoomFactor = 0.05;
				if (m_freqScaleZoomFactor < 0.15) m_freqScaleZoomFactor = 0.15;

				m_mouseDownPos = pos;
				m_freqScalePanadapterUpdate = true;
				m_panGridUpdate = true;
				m_peakHoldBufferResize = true;

//				if (m_displayTime.elapsed() >= 50) {
//
//					m_displayTime.restart();
//					update();
//				}
			}
			else
				setCursor(Qt::ArrowCursor);

			m_showFilterLeftBoundary = false;
			m_showFilterRightBoundary = false;
			m_highlightFilter = false;
			break;

		case filterRegionLow:

			setCursor(Qt::SizeHorCursor);
			m_showFilterLeftBoundary = true;
			if (event->buttons() == Qt::LeftButton) {

				QPoint dPos = m_mouseDownPos - pos;
				qreal dFreq = (qreal)(dPos.x() * m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width();

				m_filterLowerFrequency = qRound(m_mouseDownFilterFrequencyLo - dFreq);
				set->setRXFilter(this, m_receiver, m_filterLowerFrequency, m_filterUpperFrequency);

//				if (m_displayTime.elapsed() >= 50) {
//
//					m_displayTime.restart();
//					update();
//				}
			}

			m_highlightFilter = false;
			break;

		case filterRegionHigh:

			setCursor(Qt::SizeHorCursor);
			m_showFilterRightBoundary = true;
			if (event->buttons() == Qt::LeftButton) {

				QPoint dPos = m_mouseDownPos - pos;
				qreal dFreq = (qreal)(dPos.x() * m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width();

				m_filterUpperFrequency = qRound(m_mouseDownFilterFrequencyHi - dFreq);
				set->setRXFilter(this, m_receiver, m_filterLowerFrequency, m_filterUpperFrequency);

//				if (m_displayTime.elapsed() >= 50) {
//
//					m_displayTime.restart();
//					update();
//				}
			}

			m_highlightFilter = false;
			break;

		case filterRegion:

			setCursor(Qt::ArrowCursor);
			m_displayCenterlineHeight = m_panRect.top() + (size().height() - 3);
			
			if (event->buttons() == Qt::LeftButton) {

				m_highlightFilter = true;
				QPoint dPos = m_mouseDownPos - pos;
				qreal dFreq = (qreal)(dPos.x() * m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width();

				m_filterUpperFrequency = qRound(m_mouseDownFilterFrequencyHi - dFreq);
				m_filterLowerFrequency = qRound(m_mouseDownFilterFrequencyLo - dFreq);
				set->setRXFilter(this, m_receiver, m_filterLowerFrequency, m_filterUpperFrequency);

//				if (m_displayTime.elapsed() >= 50) {
//
//					m_displayTime.restart();
//					update();
//				}
			}
			m_showFilterLeftBoundary = false;
			m_showFilterRightBoundary = false;
			break;

		case elsewhere:
			//GRAPHICS_DEBUG << "elsewhere";
			break;
	}

	if (m_displayTime.elapsed() >= 50) {

		m_displayTime.restart();
		update();
	}
}

void QGLReceiverPanel::keyPressEvent(QKeyEvent* event) {
	
	//GRAPHICS_DEBUG << "keyPressEvent";
	if (event->key() == Qt::Key_Control) {
		
		//m_keyCTRLpressed = true;
		//GRAPHICS_DEBUG << "m_keyCTRLpressed =" << m_keyCTRLpressed;
		//printf("Ry %f\n",ry);
		//ry+=10.0f;
 	}
	else if (event->key() == Qt::Key_T) {
			
			//printf("Ry %f\n",ry);
			//ry-=10.0f;
	}
 	else if (event->key() == Qt::Key_P) {
		
		//drawTeapot = !drawTeapot;
	}
 	else if (event->key() == Qt::Key_W) {
		
		//approach -= 0.1f;
	}
 	else if (event->key() == Qt::Key_S) {
		
		//approach += 0.1f;
	}
	else {
		
		//m_keyCTRLpressed = false;
		//GRAPHICS_DEBUG << "m_keyCTRLpressed =" << m_keyCTRLpressed;
		//event->ignore();
 	}

	QWidget::keyPressEvent(event);
 	//updateGL();
	update();
}

//void QGLReceiverPanel::timerEvent(QTimerEvent *) {
//
//	update();
//}
 
//********************************************************************

void QGLReceiverPanel::setSpectrumSize(QObject *sender, int value) {

	Q_UNUSED(sender)

	if (m_dataEngineState == QSDR::DataEngineDown) {

		GRAPHICS_DEBUG << "set spectrum size to: " << value;
		m_spectrumSize = value;
		update();
	}
}

void QGLReceiverPanel::setFrequency(QObject *sender, bool value, int rx, long freq) {

	Q_UNUSED(sender)
	Q_UNUSED(value)
	
	if (m_receiver != rx) return;

	if (m_peakHold) {
		
		//spectrumBufferMutex.lock();
		m_panPeakHoldBins.clear();
		m_panPeakHoldBins.resize(m_panSpectrumBinsLength);
		m_panPeakHoldBins.fill(-500.0);
		//spectrumBufferMutex.unlock();
	}

	for (int i = 0; i < set->getNumberOfReceivers(); i++) {

		m_rxDataList[i].frequency = freq;
	}

	m_frequency = freq;
	m_freqScalePanadapterUpdate = true;
	m_panGridUpdate = true;
	update();
}

void QGLReceiverPanel::setFilterFrequencies(QObject *sender, int rx, qreal lo, qreal hi) {

	Q_UNUSED(sender)

	if (m_receiver != rx) return;
		
	m_filterLowerFrequency = lo;
	m_filterUpperFrequency = hi;
	m_filterChanged = true;
	update();
}

void QGLReceiverPanel::setCurrentReceiver(QObject *sender, int value) {

	Q_UNUSED(sender)

	m_currentReceiver = value;
	if (m_receiver != m_currentReceiver) m_crossHairCursor = false;

	m_panGridUpdate = true;
	update();
}

void QGLReceiverPanel::setCoupledRx(QObject* sender, int value) {

	Q_UNUSED(sender)

	m_frequencyRxOnRx = value;
	//GRAPHICS_DEBUG << "showing frequency of Rx " << m_frequencyRxOnRx/10 << " on Rx " << m_frequencyRxOnRx%10;
}

void QGLReceiverPanel::freqRulerPositionChanged(float pos, int rx) {

	if (rx == m_receiver) {
		
		m_freqRulerPosition = pos;

		setupDisplayRegions(size());
		update();
	}
}

//void QGLReceiverPanel::setSpectrumBuffer(const float *buffer) {
//
//	if (m_spectrumAveraging) {
//	
//		QVector<float> specBuf(m_spectrumSize);
//		QVector<float> avg;
//
//		//spectrumBufferMutex.lock();
//
//		memcpy(
//			(float *) specBuf.data(),
//			(float *) &buffer[0],
//			m_spectrumSize * sizeof(float));
//
//		specAv_queue.enqueue(specBuf);
//		if (specAv_queue.size() <= m_specAveragingCnt) {
//	
//			for (int i = 0; i < m_spectrumSize; i++) {
//				
//				if (m_tmp.size() < m_spectrumSize)
//					m_tmp << specAv_queue.last().data()[i];
//				else
//					m_tmp[i] += specAv_queue.last().data()[i];
//			}
//			//spectrumBufferMutex.unlock();
//			return;
//		}
//	
//		for (int i = 0; i < m_spectrumSize; i++) {
//
//			m_tmp[i] -= specAv_queue.first().at(i);
//			m_tmp[i] += specAv_queue.last().at(i);
//			avg << m_tmp.at(i) * m_scale;
//		}
//
//		computeDisplayBins(avg, buffer);
//		specAv_queue.dequeue();
//	
//		avg.clear();
//		//spectrumBufferMutex.unlock();
//	}
//	else {
//
//		QVector<float> specBuf(m_spectrumSize);
//
//		memcpy(
//			(float *) specBuf.data(),
//			(float *) &buffer[0],
//			m_spectrumSize * sizeof(float));
//
//		computeDisplayBins(specBuf, buffer);
//	}
//}

void QGLReceiverPanel::setSpectrumBuffer(const float *buffer) {

	QVector<float> specBuf(m_spectrumSize);

	if (m_spectrumAveraging) {
	
		//QVector<float> specBuf(m_spectrumSize);
		QVector<float> avg;

		//spectrumBufferMutex.lock();

		memcpy(
			(float *) specBuf.data(),
			(float *) &buffer[0],
			m_spectrumSize * sizeof(float));

		if (m_dataEngineState == QSDR::DataEngineUp) {

			averager->ProcessDBAverager(specBuf, specBuf);
			computeDisplayBins(specBuf, buffer);
		}

		//spectrumBufferMutex.unlock();
	}
	else {

		//QVector<float> specBuf(m_spectrumSize);

		memcpy(
			(float *) specBuf.data(),
			(float *) &buffer[0],
			m_spectrumSize * sizeof(float));

		if (m_dataEngineState == QSDR::DataEngineUp)
			computeDisplayBins(specBuf, buffer);
	}
}

void QGLReceiverPanel::computeDisplayBins(const QVector<float> &panBuffer, const float *waterfallBuffer) {

	int newSampleSize = 0;
	int deltaSampleSize = 0;
	int idx = 0;
	int lIdx = 0;
	int rIdx = 0;
	qreal localMax;

	if (m_serverMode == QSDR::ChirpWSPRFile) {
		
		newSampleSize = (int)floor(2 * BUFFER_SIZE * m_freqScaleZoomFactor);
		deltaSampleSize = 2 * BUFFER_SIZE - newSampleSize;
	}
	else {

		newSampleSize = (int)floor(m_spectrumSize * m_freqScaleZoomFactor);
		deltaSampleSize = m_spectrumSize - newSampleSize;
	}

	/*if (deltaSampleSize%2 != 0) {
		deltaSampleSize += 1;
		newSampleSize -= 1;
	}*/

	m_panScale = (qreal)(1.0 * newSampleSize / m_panRectWidth);
	m_scaleMultOld = m_scaleMult;
		
	if (m_panScale < 0.125) {
		m_scaleMult = 0.0625;
	}
	else if (m_panScale < 0.25) {
		m_scaleMult = 0.125;
	}
	else if (m_panScale < 0.5) {
		m_scaleMult = 0.25;
	}
	else if (m_panScale < 1.0) {
		m_scaleMult = 0.5;
	}
	else {
		m_scaleMult = 1.0;
	}

	m_panSpectrumBinsLength = (GLint)(m_scaleMult * m_panRectWidth);

	/*if (bins != binsOld) {

		GRAPHICS_DEBUG << "newSampleSize" << newSampleSize;
		GRAPHICS_DEBUG << "m_panScale" << m_panScale;
		GRAPHICS_DEBUG << "bins:" << bins;
	}*/

	if (m_scaleMultOld != m_scaleMult) {

		m_waterfallUpdate = true;
	}

	m_waterfallPixel.clear();
	m_waterfallPixel.resize(4 * m_panRectWidth);

	m_panadapterBins.clear();

	/*if (m_peakHold && peakHoldTimer.elapsed() > 10000) {
		
		spectrumBufferMutex.lock();
		m_panPeakHoldBins.clear();
		m_panPeakHoldBins.resize(m_panSpectrumBinsLength);
		m_panPeakHoldBins.fill(-500.0);
		spectrumBufferMutex.unlock();

		peakHoldTimer.restart();
	}*/

	if (m_peakHoldBufferResize) {
		
		//spectrumBufferMutex.lock();
		m_panPeakHoldBins.clear();
		m_panPeakHoldBins.resize(m_panSpectrumBinsLength);
		m_panPeakHoldBins.fill(-500.0);
		//spectrumBufferMutex.unlock();

		m_peakHoldBufferResize = false;
	}
	
	for (int i = 0; i < m_panSpectrumBinsLength; i++) {
		
		//qreal max;
		idx = 0;
		lIdx = (int)qFloor((qreal)(i * m_panScale / m_scaleMult));
		rIdx = (int)qFloor((qreal)(i * m_panScale / m_scaleMult) + m_panScale / m_scaleMult);
					
		// max value; later we try mean value also!
		localMax = -10000.0F;
		for (int j = lIdx; j < rIdx; j++) {

			if (panBuffer[j] > localMax) {

				localMax = panBuffer[j];
				idx = j;
			}
		}
		idx += deltaSampleSize/2;
		
		QColor pColor;
		if (m_mercuryAttenuator) {

			m_panadapterBins << panBuffer[idx] - m_dBmPanMin - m_dBmPanLogGain - 20.0f;
			pColor = getWaterfallColorAtPixel(waterfallBuffer[idx] - m_dBmPanLogGain - 20.0f);
		}
		else {

			m_panadapterBins << panBuffer[idx] - m_dBmPanMin - m_dBmPanLogGain;
			pColor = getWaterfallColorAtPixel(waterfallBuffer[idx] - m_dBmPanLogGain);
		}

		if (m_peakHold && (m_panadapterBins.at(i) > m_panPeakHoldBins.at(i))) {

			m_panPeakHoldBins[i] = m_panadapterBins.at(i);
		}
				
		
		TGL_ubyteRGBA color;
		color.red   = (uchar)(pColor.red());
		color.green = (uchar)(pColor.green());
		color.blue  = (uchar)(pColor.blue());
		color.alpha = 255;
		
		for (int j = 0; j < (int)(1/m_scaleMult); j++) {

			m_waterfallPixel[(int)(i / m_scaleMult) + j] = color;
		}
	}
	m_waterfallDisplayUpdate = true;
	update();
}

// get waterfall colors - taken from PowerSDR/KISS Konsole
QColor QGLReceiverPanel::getWaterfallColorAtPixel(qreal value) {

	QColor color;
	//int r = 0; int g = 0; int b = 0;
	int r, g, b;
	int lowerThreshold = (int)m_dBmPanMin - m_waterfallOffsetLo;
	int upperThreshold = (int)m_dBmPanMax + m_waterfallOffsetHi;

	float offset;
	float globalRange;
	float localRange;
	float percent;
	
	switch (m_waterColorScheme) {

		case QSDRGraphics::simple:

			if (value <= lowerThreshold)
				color = m_waterfallLoColor;
			else 
			if (value >= upperThreshold)
					color = QColor(255, 255, 255);//m_waterfallHiColor;
			else {

				percent = (value - lowerThreshold) / (upperThreshold - lowerThreshold);
				if (percent <= 0.5)	{ // use a gradient between low and mid colors
				
					percent *= 2;

					r = (int)((1 - percent) * m_waterfallLoColor.red()   + percent * m_waterfallMidColor.red());
					g = (int)((1 - percent) * m_waterfallLoColor.green() + percent * m_waterfallMidColor.green());
					b = (int)((1 - percent) * m_waterfallLoColor.blue()  + percent * m_waterfallMidColor.blue());
				}
				else {	// use a gradient between mid and high colors

					percent = (float)(percent - 0.5) * 2;

					r = (int)((1 - percent) * m_waterfallMidColor.red()   + percent * 255);//m_waterfallHiColor.red());
					g = (int)((1 - percent) * m_waterfallMidColor.green() + percent * 255);//m_waterfallHiColor.green());
					b = (int)((1 - percent) * m_waterfallMidColor.blue()  + percent * 255);//m_waterfallHiColor.blue());
				}

				if (r > 255) r = 255;
				if (g > 255) g = 255;
				if (b > 255) b = 255;
				color = QColor(r, g, b, m_waterfallAlpha);
			}

			break;

		case QSDRGraphics::enhanced:

			if (value <= lowerThreshold)
				color = m_waterfallLoColor;
			else 
			if (value >= upperThreshold)
					color = m_waterfallHiColor;
			else {

				offset = value - lowerThreshold;
				globalRange = offset / m_waterfallColorRange; // value from 0.0 to 1.0 where 1.0 is high and 0.0 is low.

				if (globalRange < (float)2/9) { // background to blue

					localRange = globalRange / ((float)2/9);
					r = (int)((1.0 - localRange) * m_waterfallLoColor.red());
					g = (int)((1.0 - localRange) * m_waterfallLoColor.green());
					b = (int)(m_waterfallLoColor.blue() + localRange * (255 - m_waterfallLoColor.blue()));
				}
				else 
				if (globalRange < (float)3/9) { // blue to blue-green

					localRange = (globalRange - (float)2/9) / ((float)1/9);
					r = 0;
					g = (int)(localRange * 255);
					b = 255;
				}
				else 
				if (globalRange < (float)4/9) { // blue-green to green

					localRange = (globalRange - (float)3/9) / ((float)1/9);
					r = 0;
					g = 255;
					b = (int)((1.0 - localRange) * 255);
				}
				else 
				if (globalRange < (float)5/9) { // green to red-green

					localRange = (globalRange - (float)4/9) / ((float)1/9);
					r = (int)(localRange * 255);
					g = 255;
					b = 0;
				}
				else 
				if (globalRange < (float)7/9) { // red-green to red

					localRange = (globalRange - (float)5/9) / ((float)2/9);
					r = 255;
					g = (int)((1.0 - localRange) * 255);
					b = 0;
				}
				else 
				if (globalRange < (float)8/9) { // red to red-blue

					localRange = (globalRange - (float)7/9) / ((float)1/9);
					r = 255;
					g = 0;
					b = (int)(localRange * 255);
				}
				else { // red-blue to purple end

					localRange = (globalRange - (float)8/9) / ((float)1/9);
					r = (int)((0.75 + 0.25 * (1.0 - localRange)) * 255);
					g = (int)(localRange * 255 * 0.5);
					b = 255;
				}
				if (r > 255) r = 255;
				if (g > 255) g = 255;
				if (b > 255) b = 255;
				if (r < 0) r = 0;
				if (g < 0) g = 0;
				if (b < 0) b = 0;
				color = QColor(r, g, b, m_waterfallAlpha);
			}

			break;

		case QSDRGraphics::spectran:

			break;
	}
	
	return color;
}

void QGLReceiverPanel::setFramesPerSecond(QObject *sender, int rx, int value) {

	Q_UNUSED(sender)

	if (m_receiver != rx) return;

	m_fps = value;

	m_secWaterfallMin = -(1.0/m_fps) * m_secScaleWaterfallRect.height();
	m_secScaleWaterfallRenew = true;
	m_secScaleWaterfallUpdate = true;
	update();
}

void QGLReceiverPanel::systemStateChanged(
	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	//Q_UNUSED (sender)
	Q_UNUSED (err)
	Q_UNUSED (hwmode)
	Q_UNUSED (state)

	if (m_dataEngineState != state)
		m_dataEngineState = state;

	//if (state == QSDR::DataEngineDown)
	//	m_panadapterBins.clear();

	if (state == QSDR::DataEngineDown)
		averager->clearBuffer();

	if (m_serverMode != mode)
		m_serverMode = mode;

//	if (m_serverMode == mode)
//		return;
//	else
//		m_serverMode = mode;

	//resizeGL(width(), height());
	m_displayTime.restart();
	
	update();
}

void QGLReceiverPanel::graphicModeChanged(
	QObject *sender,
	QSDRGraphics::_Panadapter panMode,
	QSDRGraphics::_WfScheme colorScheme)
{
	Q_UNUSED (sender)

	bool change = false;

	if (m_panadapterMode != panMode) {
		
		m_panadapterMode = panMode;
		change = true;
	}

	if (m_waterColorScheme != colorScheme) {

		m_waterColorScheme = colorScheme;
		change = true;
	}

	if (!change) return;

	update();
}

 void QGLReceiverPanel::setSpectrumAveraging(bool value) {

	 spectrumBufferMutex.lock();

	 if (m_spectrumAveraging == value) 
		 return;
	 else
		 m_spectrumAveraging = value;

	 spectrumBufferMutex.unlock();
 }

void QGLReceiverPanel::setSpectrumAveragingCnt(int value) {

	spectrumBufferMutex.lock();

		m_tmp.clear();

		while (!specAv_queue.isEmpty())
			specAv_queue.dequeue();

		m_specAveragingCnt = value;

		if (m_specAveragingCnt > 0)
			m_scale = 1.0f / m_specAveragingCnt;
		else
			m_scale = 1.0f;

	spectrumBufferMutex.unlock();
}

void QGLReceiverPanel::setPanGridStatus(bool value) {

	spectrumBufferMutex.lock();

	 if (m_panGrid == value) 
		 return;
	 else
		 m_panGrid = value;

	 spectrumBufferMutex.unlock();

	 update();
}

void QGLReceiverPanel::setPeakHoldStatus(bool value) {

	spectrumBufferMutex.lock();

	if (m_peakHold == value) 
		return;
	else
		m_peakHold = value;
	
	m_panPeakHoldBins.clear();
	m_panPeakHoldBins.resize(m_panSpectrumBinsLength);
	m_panPeakHoldBins.fill(-500.0);
	spectrumBufferMutex.unlock();
}

void QGLReceiverPanel::sampleRateChanged(QObject *sender, int value) {

	Q_UNUSED(sender)
	
	m_sampleRate = value;
	m_freqScalePanadapterUpdate = true;
	m_panGridUpdate = true;
	m_peakHoldBufferResize = true;
	m_filterChanged = true;

	update();
}

void QGLReceiverPanel::setMercuryAttenuator(QObject* sender, HamBand band, int value) {

	Q_UNUSED(sender)
	Q_UNUSED(band)
	
	m_mercuryAttenuator = value;
	update();
}

void QGLReceiverPanel::setPanadapterColors() {

	m_spectrumColorsChanged = true;

	mutex.lock();
	m_bkgRed   = (GLfloat)(set->getPanadapterColors().panBackgroundColor.red() / 256.0);
	m_bkgGreen = (GLfloat)(set->getPanadapterColors().panBackgroundColor.green() / 256.0);
	m_bkgBlue  = (GLfloat)(set->getPanadapterColors().panBackgroundColor.blue() / 256.0);

	m_red	= (GLfloat)(set->getPanadapterColors().panLineColor.red() / 256.0);
	m_green = (GLfloat)(set->getPanadapterColors().panLineColor.green() / 256.0);
	m_blue	= (GLfloat)(set->getPanadapterColors().panLineColor.blue() / 256.0);

	m_redF	 = (GLfloat)(set->getPanadapterColors().panLineFilledColor.red() / 256.0);
	m_greenF = (GLfloat)(set->getPanadapterColors().panLineFilledColor.green() / 256.0);
	m_blueF  = (GLfloat)(set->getPanadapterColors().panLineFilledColor.blue() / 256.0);

	m_redST	  = (GLfloat)(set->getPanadapterColors().panSolidTopColor.red() / 256.0);
	m_greenST = (GLfloat)(set->getPanadapterColors().panSolidTopColor.green() / 256.0);
	m_blueST  = (GLfloat)(set->getPanadapterColors().panSolidTopColor.blue() / 256.0);

	m_redSB   = (GLfloat)(set->getPanadapterColors().panSolidBottomColor.red() / 256.0);
	m_greenSB = (GLfloat)(set->getPanadapterColors().panSolidBottomColor.green() / 256.0);
	m_blueSB  = (GLfloat)(set->getPanadapterColors().panSolidBottomColor.blue() / 256.0);

	m_waterfallMidColor = set->getPanadapterColors().waterfallColor.toRgb() ;

	QColor gridColor = m_gridColor;
	m_gridColor = set->getPanadapterColors().gridLineColor;

	if (gridColor != m_gridColor) {

		m_redGrid   = (GLfloat)(m_gridColor.red()/256.0);
		m_greenGrid = (GLfloat)(m_gridColor.green()/256.0);
		m_blueGrid  = (GLfloat)(m_gridColor.blue()/256.0);

		m_panGridUpdate = true;
	}
	mutex.unlock();

	update();
}

void QGLReceiverPanel::setWaterfallOffesetLo(int rx, int value) {

	if (m_receiver != rx) return;

	m_waterfallOffsetLo = value;
	update();
}

void QGLReceiverPanel::setWaterfallOffesetHi(int rx, int value) {

	if (m_receiver != rx) return;

	m_waterfallOffsetHi = value;
	update();
}

void QGLReceiverPanel::setdBmScaleMin(int rx, qreal value) {

	if (m_receiver != rx) return;

	m_dBmPanMin = value;

	m_dBmScalePanadapterUpdate = true;
	m_panGridUpdate = true;
	m_peakHoldBufferResize = true;

	update();
}

void QGLReceiverPanel::setdBmScaleMax(int rx, qreal value) {

	if (m_receiver != rx) return;

	m_dBmPanMax = value;

	m_dBmScalePanadapterUpdate = true;
	m_panGridUpdate = true;
	m_peakHoldBufferResize = true;

	update();
}

void QGLReceiverPanel::setMouseWheelFreqStep(QObject *sender, int rx, qreal step) {

	if (m_receiver != rx) return;

	m_mouseWheelFreqStep = step;
	update();
}

void QGLReceiverPanel::setHamBand(QObject *sender, int rx, bool byButton, HamBand band) {

	if (m_receiver != rx || !byButton) return;
	// we change the dBm-scale only, if the frequency is changed by changing the band by button.
	// That is, if we change band by changing the frequency, the dBm scale stays constant.

	m_rxDataList[m_receiver].hamBand = band;

	m_dBmPanMin = m_rxDataList.at(m_receiver).dBmPanScaleMinList.at(band);
	m_dBmPanMax = m_rxDataList.at(m_receiver).dBmPanScaleMaxList.at(band);

	m_dBmScalePanadapterUpdate = true;
	m_panGridUpdate = true;
	m_peakHoldBufferResize = true;

	update();
}

void QGLReceiverPanel::setADCStatus(int value) {

	m_adcStatus = value;
	update();

	QTimer::singleShot(50, this, SLOT(updateADCStatus()));
}

void QGLReceiverPanel::updateADCStatus() {

	if (m_dataEngineState == QSDR::DataEngineUp)
		m_adcStatus = 1;
	else
		m_adcStatus = 0;

	//qDebug() << "m_adcStatus: " << m_adcStatus;
	update();
}

void QGLReceiverPanel::setAGCLineLevels(QObject *sender, int rx, qreal thresh, qreal hang) {

	Q_UNUSED(sender)

	if (m_receiver != rx) return;
	if (m_agcThresholdOld == thresh && m_agcHangLevelOld == hang) return;

	m_agcThresholdOld = thresh;
	m_agcHangLevelOld = hang;
	//GRAPHICS_DEBUG << "m_agcThresholdOld = " << m_agcThresholdOld;
	//GRAPHICS_DEBUG << "m_agcHangLevelOld = " << m_agcHangLevelOld;
}

void QGLReceiverPanel::setAGCLineFixedLevel(QObject *sender, int rx, qreal value) {

	Q_UNUSED(sender)

	if (m_receiver != rx) return;
	if (m_agcFixedGain == value) return;

	m_agcFixedGain = value;
	//GRAPHICS_DEBUG << "m_agcFixedGain = " << m_agcFixedGain;
}

void QGLReceiverPanel::setAGCMode(QObject *sender, int rx, AGCMode mode, bool hangEnabled) {

	Q_UNUSED(sender)

	if (m_receiver != rx) return;

	if (m_agcHangEnabled == hangEnabled && m_agcMode == mode) return;

	m_agcMode = mode;
	m_agcModeString = set->getAGCModeString(m_receiver);
	m_agcHangEnabled = hangEnabled;
	//GRAPHICS_DEBUG << "m_agcMode = " << m_agcModeString;
	//GRAPHICS_DEBUG << "m_agcHangEnabled = " << m_agcHangEnabled;

	update();
}

//void QGLReceiverPanel::setAGCHangEnabled(QObject *sender, int rx, bool hangEnabled) {
//
//	Q_UNUSED(sender)
//
//	if (m_receiver != rx) return;
//
//	if (m_agcHangEnabled == hangEnabled) return;
//	m_agcHangEnabled = hangEnabled;
//	GRAPHICS_DEBUG << "m_agcHangEnabled = " << m_agcHangEnabled;
//
//	update();
//}
