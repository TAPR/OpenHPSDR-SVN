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

#include "cusdr_oglReceiverPanel.h"

#include <QtGui>
#include <QDebug>
#include <QFileInfo>
#include <QTimer>
#include <QImage>
#include <QString>
#include <QGLFramebufferObject>

#ifndef GL_MULTISAMPLE
#define GL_MULTISAMPLE  0x809D
#endif

QGLReceiverPanel::QGLReceiverPanel(QWidget *parent, int rx)
		: QGLWidget(QGLFormat(QGL::SampleBuffers|QGL::AlphaChannel), parent)

		, m_settings(Settings::instance())
		, m_receiver(rx)
		, m_spectrumSize(m_settings->getSpectrumSize())
		, m_serverMode(m_settings->getCurrentServerMode())
		, m_hwInterface(m_settings->getHWInterface())
		, m_dataEngineState(QSDR::DataEngineDown)
		, m_currentReceiver(m_settings->getCurrentReceiver())
		, m_frequencyRxOnRx(0)
		, m_sampleRate(m_settings->getSampleRate())
		, m_downRate(m_settings->getChirpDownSampleRate())
		, m_spectrumAveraging(m_settings->getSpectrumAveraging())
		, m_spectrumAveragingOld(m_spectrumAveraging)
		, m_specAveragingCnt(m_settings->getSpectrumAveragingCnt())
		, m_mousePos(QPoint(-1, -1))
		, m_mouseDownPos(QPoint(-1, -1))
		, m_snapMouse(3)
		, m_spectrumVertexColorUpdate(false)
		, m_spectrumColorsChanged(true)
		, m_freqScaleZoomFactor(1.0f)
		, m_freqRulerPosition(0.5)
		, m_freqRulerDisplayWidth(0)
		, m_scaleMult(1.0f)
		, m_filterLowerFrequency(-3050.0)
		, m_filterUpperFrequency(-150.0)
		, m_panSpectrumBinsLength(0)	
		, m_panSpectrumMinimumHeight(0)
		, m_panadapterMode(m_settings->getPanadapterMode())
		, m_waterColorScheme(m_settings->getWaterfallColorScheme())
		, m_waterfallAlpha(255)
		, m_oldWaterfallWidth(0)
		, m_crossHairCursor(false)
		, m_panGrid(m_settings->getPanGridStatus())
		, m_filterLeft(0)
		, m_filterRight(0)
		, m_filterTop(0)
		, m_filterBottom(0)
		, m_filterChanged(true)
		, m_highlightFilter(false)
		, m_showFilterLeftBoundary(false)
		, m_showFilterRightBoundary(false)
{
	QGL::setPreferredPaintEngine(QPaintEngine::OpenGL);

	setAutoBufferSwap(true);
	setAutoFillBackground(false);
	
	setMouseTracking(true);
	setFocusPolicy(Qt::StrongFocus);

	GRAPHICS_DEBUG << "set spectrum buffer size to:" << m_spectrumSize;

	setupDisplayRegions(size());
	m_oldWidth = size().width();
	
	m_rxDataList = m_settings->getReceiverDataList();
	m_frequency = m_rxDataList.at(m_receiver).frequency;

	m_dBmScalePanadapterRenew = true;
	m_dBmScalePanadapterUpdate = true;
	m_freqScalePanadapterRenew = true;
	m_freqScalePanadapterUpdate = true;
	m_panGridRenew = true;
	m_panGridUpdate = true;
	m_waterfallUpdate = true;

	m_dBmPanMin = m_rxDataList.at(m_receiver).dBmPanScaleMin;
	m_dBmPanMax = m_rxDataList.at(m_receiver).dBmPanScaleMax;

	m_waterfallTime = m_rxDataList.at(m_receiver).waterfallTime;
	m_waterfallOffsetLo = m_rxDataList.at(m_receiver).waterfallOffsetLo;
	m_waterfallOffsetHi = m_rxDataList.at(m_receiver).waterfallOffsetHi;
		

	m_filterLowerFrequency = m_rxDataList.at(m_receiver).filterLo;
	m_filterUpperFrequency = m_rxDataList.at(m_receiver).filterHi;

	m_tinyFont.setStyleStrategy(QFont::PreferAntialias);
	m_tinyFont.setFixedPitch(true);
	#ifdef Q_OS_MAC
		m_tinyFont.setPixelSize(8);
		m_tinyFont.setFamily("Arial");
	#else if Q_OS_WIN
		m_tinyFont.setPixelSize(8);
		m_tinyFont.setFamily("Arial");
	#endif

	m_smallFont.setStyleStrategy(QFont::PreferAntialias);
	//m_smallFont.setStyleHint(QFont::SansSerif, QFont::PreferAntialias);
	m_smallFont.setBold(true);
	m_smallFont.setFixedPitch(true);
	#ifdef Q_OS_MAC
		m_smallFont.setPixelSize(10);
		m_smallFont.setFamily("Arial");
	#else if Q_OS_WIN
		m_smallFont.setPixelSize(10);
		m_smallFont.setFamily("Arial");
	#endif

	m_normalFont.setStyleStrategy(QFont::PreferAntialias);
	//m_normalFont.setStyleHint(QFont::SansSerif, QFont::PreferAntialias);
	m_normalFont.setFixedPitch(true);
	#ifdef Q_OS_MAC
		m_normalFont.setPixelSize(11);
		m_normalFont.setFamily("Arial");
	#else if Q_OS_WIN
		m_normalFont.setPixelSize(11);
		m_normalFont.setFamily("Arial");
	#endif

	m_freqFont1.setStyleStrategy(QFont::PreferAntialias);
	//m_freqFont1.setStyleHint(QFont::SansSerif, QFont::PreferAntialias);
	m_freqFont1.setBold(true);
	m_freqFont1.setFixedPitch(true);
	#ifdef Q_OS_MAC
		m_freqFont1.setPixelSize(18);
		m_freqFont1.setFamily("Arial");
	#else if Q_OS_WIN
		m_freqFont1.setPixelSize(12);
		m_freqFont1.setFamily("Arial");
	#endif

	m_freqFont2.setStyleStrategy(QFont::PreferAntialias);
	//m_freqFont2.setStyleHint(QFont::SansSerif, QFont::PreferAntialias);
	m_freqFont2.setBold(true);
	m_freqFont2.setFixedPitch(true);
	#ifdef Q_OS_MAC
		m_freqFont2.setPixelSize(12);
		m_freqFont2.setFamily("Arial");
	#else if Q_OS_WIN
		m_freqFont2.setPixelSize(10);
		m_freqFont2.setFamily("Arial");
	#endif

	m_oglTextTiny = new OGLText(m_tinyFont);
	m_oglTextSmall = new OGLText(m_smallFont);
	m_oglTextNormal = new OGLText(m_normalFont);
	m_oglTextFreq1 = new OGLText(m_freqFont1);
	m_oglTextFreq2 = new OGLText(m_freqFont2);

	timer = 0;

	setupConnections();

	m_displayTime.start();
	m_resizeTime.start();
	waterfallTimer.start();
	//freqChangeTimer.start();
	
	memset(m_spectrumBuffer, -10000, m_spectrumSize * sizeof(float));
	
	m_dBmPanLogGain = 47;//49;//69 // allow user to calibrate this value

	m_cameraDistance = 0;
	m_cameraAngle = QPoint(0, 0);
	
	m_bkgRed   = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.red() / 256.0);
	m_bkgGreen = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.green() / 256.0);
	m_bkgBlue  = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.blue() / 256.0);

	m_red	= (GLfloat)(m_settings->getPanadapterColors().panLineColor.red() / 256.0);
	m_green = (GLfloat)(m_settings->getPanadapterColors().panLineColor.green() / 256.0);
	m_blue	= (GLfloat)(m_settings->getPanadapterColors().panLineColor.blue() / 256.0);

	m_redF	 = (GLfloat)(m_settings->getPanadapterColors().panLineFilledColor.red() / 256.0);
	m_greenF = (GLfloat)(m_settings->getPanadapterColors().panLineFilledColor.green() / 256.0);
	m_blueF  = (GLfloat)(m_settings->getPanadapterColors().panLineFilledColor.blue() / 256.0);

	m_redST	  = (GLfloat)(m_settings->getPanadapterColors().panSolidTopColor.red() / 256.0);
	m_greenST = (GLfloat)(m_settings->getPanadapterColors().panSolidTopColor.green() / 256.0);
	m_blueST  = (GLfloat)(m_settings->getPanadapterColors().panSolidTopColor.blue() / 256.0);

	m_redSB   = (GLfloat)(m_settings->getPanadapterColors().panSolidBottomColor.red() / 256.0);
	m_greenSB = (GLfloat)(m_settings->getPanadapterColors().panSolidBottomColor.green() / 256.0);
	m_blueSB  = (GLfloat)(m_settings->getPanadapterColors().panSolidBottomColor.blue() / 256.0);

	m_waterfallLoColor = QColor(0, 0, 0, m_waterfallAlpha);
	m_waterfallHiColor = QColor(192, 124, 255, m_waterfallAlpha);
	m_waterfallMidColor = m_settings->getPanadapterColors().waterfallColor.toRgb();
	m_waterfallColorRange = (int)(m_dBmPanMax - m_dBmPanMin);

	m_frequencyScaleFBO = 0;
	m_dBmScaleFBO = 0;
	m_panadapterGridFBO = 0;
	m_textureFBO = 0;
	m_waterfallLineFBO = 0;
	m_waterfallFBO = 0;

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

	disconnect(m_settings, 0, this, 0);
	
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

	while (!specAv_queue.isEmpty())
		specAv_queue.dequeue();
}

QSize QGLReceiverPanel::sizeHint() const {
	
	return QSize(width(), height());
}

void QGLReceiverPanel::setupConnections() {

	CHECKED_CONNECT(
		m_settings, 
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
		m_settings,
		SIGNAL(graphicModeChanged(
					QObject *,
					QSDRGraphics::_Panadapter,
					QSDRGraphics::_WaterfallColorScheme)), 
		this, 
		SLOT(graphicModeChanged(
					QObject *,
					QSDRGraphics::_Panadapter,
					QSDRGraphics::_WaterfallColorScheme)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(freqRulerPositionChanged(float, int)), 
		this, 
		SLOT(freqRulerPositionChanged(float, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(frequencyChanged(QObject *, bool, int, long)), 
		this, 
		SLOT(setFrequency(QObject *, bool, int, long)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(currentReceiverChanged(int)), 
		this, 
		SLOT(setCurrentReceiver(int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(sampleRateChanged(QObject *, int)), 
		this, 
		SLOT(sampleRateChanged(QObject *, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(filterFrequenciesChanged(QObject *, int, qreal, qreal)), 
		this, 
		SLOT(setFilterFrequencies(QObject *, int, qreal, qreal)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(waterfallTimeChanged(int, int)),
		this,
		SLOT(setWaterfallTime(int, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(waterfallOffesetLoChanged(int, int)),
		this,
		SLOT(setWaterfallOffesetLo(int, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(waterfallOffesetHiChanged(int, int)),
		this,
		SLOT(setWaterfallOffesetHi(int, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(spectrumAveragingChanged(bool)), 
		this, 
		SLOT(setSpectrumAveraging(bool)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(spectrumAveragingCntChanged(int)), 
		this, 
		SLOT(setSpectrumAveragingCnt(int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(panGridStatusChanged(bool)),
		this,
		SLOT(setPanGridStatus(bool)));

	/*CHECKED_CONNECT(
		m_settings, 
		SIGNAL(panadapterColorChanged()),
		this,
		SLOT(setSimpleWaterfallColor()));*/

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(panadapterColorChanged()),
		this,
		SLOT(setPanadapterColors()));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(preampChanged(QObject *, int)), 
		this, 
		SLOT(setPreamp(QObject *, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(coupledRxChanged(QObject *, int)), 
		this, 
		SLOT(setCoupledRx(QObject *, int)));
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
	glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
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

		case QSDR::ExternalDSP:
			drawGLRect(QRect(0, 0, width(), height()), QColor(0, 0, 0));
			break;

		//case QSDR::InternalDSP:
		case QSDR::DttSP:
		case QSDR::QtDSP:

			//if (freqChangeTimer.elapsed() > 50)  m_spectrumAveraging = m_spectrumAveragingOld;

			if (m_resizeTime.elapsed() > 100 || m_dataEngineState == QSDR::DataEngineDown)
				paintReceiverDisplay();
			
			break;
	}
}
 
//****************************************************
// painting modes
void QGLReceiverPanel::paintReceiverDisplay() {
 
	QRect mouse_rect(0, 0, 100, 100);
	mouse_rect.moveCenter(m_mousePos);

	if (m_filterChanged) {

		m_filterLo = m_filterLowerFrequency / m_sampleRate;
		m_filterHi = m_filterUpperFrequency / m_sampleRate;
		m_filterChanged = false;
	}

	drawPanadapter();
	drawPanHorizontalScale();
	drawPanVerticalScale();

	if (m_panGrid)
		drawPanadapterGrid();

	drawPanFilter();
	
	//if (m_settings->getNumberOfReceivers() > 1) 
		drawReceiverInfo();

	if (waterfallTimer.elapsed() >= m_waterfallTime && m_waterfallRect.height() > 10) {
		
		drawWaterfall();
		waterfallTimer.restart();
	}

	if ((m_mouseRegion == panadapterRegion || m_mouseRegion == waterfallRegion) && m_crossHairCursor)
		drawCrossHair();

	m_oldFreq = m_frequency;
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
	float yScaleColor;
	float yTop;
	
	qreal dBmRange = qAbs(m_dBmPanMax - m_dBmPanMin);

	yScale = m_panRect.height() / dBmRange;
	//yScaleColor = 4.0f / dBmRange;
	yScaleColor = 30.0f / dBmRange;
	yTop = (float) y2;
	
	if (m_dataEngineState == QSDR::DataEngineUp)
		glClear(GL_DEPTH_BUFFER_BIT);
	else
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glEnable(GL_MULTISAMPLE);
	glEnable(GL_LINE_SMOOTH);

	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);
	glLineWidth(1);

	// draw background
	if (m_dataEngineState == QSDR::DataEngineUp) {

		if (m_panGrid) {

			if (m_receiver == m_currentReceiver) {

				glBegin(GL_TRIANGLE_STRIP);
					glColor3f(0.2f * m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x1, y1, -3.0); // top left corner
					glColor3f(0.2f * m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x2, y1, -3.0); // top right corner
					glColor3f(0.8f * m_bkgRed, 0.8f * m_bkgGreen, 0.8f * m_bkgBlue); glVertex3f(x1, y2, -3.0); // bottom left corner
					glColor3f(       m_bkgRed,        m_bkgGreen,        m_bkgBlue); glVertex3f(x2, y2, -3.0); // bottom right corner
				glEnd();
			}
			else {

				glBegin(GL_TRIANGLE_STRIP);
					glColor3f(0.4f * m_bkgRed, 0.4f * m_bkgGreen, 0.4f * m_bkgBlue); glVertex3f(x1, y1, -3.0); // top left corner
					glColor3f(0.4f * m_bkgRed, 0.4f * m_bkgGreen, 0.4f * m_bkgBlue); glVertex3f(x2, y1, -3.0); // top right corner
					glColor3f(0.4f * m_bkgRed, 0.4f * m_bkgGreen, 0.4f * m_bkgBlue); glVertex3f(x1, y2, -3.0); // bottom left corner
					glColor3f(0.4f * m_bkgRed, 0.4f * m_bkgGreen, 0.4f * m_bkgBlue); glVertex3f(x2, y2, -3.0); // bottom right corner
				glEnd();
			}
		}
		else {

			if (m_receiver == m_settings->getCurrentReceiver()) {

				glBegin(GL_TRIANGLE_STRIP);
					glColor3f(0.2f * m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x1, y1, -3.0); // top left corner
					glColor3f(0.2f * m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x2, y1, -3.0); // top right corner
					glColor3f(0.5f * m_bkgRed, 0.5f * m_bkgGreen, 0.5f * m_bkgBlue); glVertex3f(x1, y2, -3.0); // bottom left corner
					glColor3f(0.7f * m_bkgRed, 0.7f * m_bkgGreen, 0.7f * m_bkgBlue); glVertex3f(x2, y2, -3.0); // bottom right corner
				glEnd();
			}
			else {

				glBegin(GL_TRIANGLE_STRIP);
					glColor3f(0.1f * m_bkgRed, 0.1f * m_bkgGreen, 0.1f * m_bkgBlue); glVertex3f(x1, y1, -3.0); // top left corner
					glColor3f(0.1f * m_bkgRed, 0.1f * m_bkgGreen, 0.1f * m_bkgBlue); glVertex3f(x2, y1, -3.0); // top right corner
					glColor3f(0.1f * m_bkgRed, 0.1f * m_bkgGreen, 0.1f * m_bkgBlue); glVertex3f(x1, y2, -3.0); // bottom left corner
					glColor3f(0.1f * m_bkgRed, 0.1f * m_bkgGreen, 0.1f * m_bkgBlue); glVertex3f(x2, y2, -3.0); // bottom right corner
				glEnd();
			}
		}
	}
	else {

		drawGLRect(m_panRect, QColor(30, 30, 50, 155), -3.0f);
	}

	//glBegin(GL_TRIANGLE_STRIP);
	//glColor3f(m_bkgRed,       m_bkgGreen,       m_bkgBlue);			glVertex3f(x1, y1, -3.0); // top left corner
	//glColor3f(m_bkgRed,       m_bkgGreen,       m_bkgBlue);			glVertex3f(x2, y1, -3.0); // top right corner
	//glColor3f(m_bkgRed * 1.2, m_bkgGreen * 1.2, m_bkgBlue * 1.2);	glVertex3f(x1, y2, -3.0); // bottom left corner
	//glColor3f(m_bkgRed * 1.6, m_bkgGreen * 1.6, m_bkgBlue * 1.6);	glVertex3f(x2, y2, -3.0); // bottom right corner
	//glEnd();

	// set a scissor box
	glScissor(x1, size().height() - y2, x2, height);
	glEnable(GL_SCISSOR_TEST);

	TGL3float *vertexArray = new TGL3float[vertexArrayLength];
	TGL3float *vertexColorArray = new TGL3float[vertexArrayLength];

	TGL3float *vertexArrayBg = new TGL3float[2*vertexArrayLength];
	TGL3float *vertexColorArrayBg = new TGL3float[2*vertexArrayLength];
	
	switch (m_panadapterMode) {

		case QSDRGraphics::FilledLine:

			for (int i = 0; i < vertexArrayLength; i++) {
			
				mutex.lock();
				vertexColorArrayBg[2*i].x = m_redF;
				vertexColorArrayBg[2*i].y = m_greenF;
				vertexColorArrayBg[2*i].z = m_blueF;
								
				vertexColorArrayBg[2*i+1].x = 0.3 * m_redF;
				vertexColorArrayBg[2*i+1].y = 0.3 * m_greenF;
				vertexColorArrayBg[2*i+1].z = 0.3 * m_blueF;
				
				vertexColorArray[i].x = m_red   * (yScaleColor * m_panadapterBins.at(i));
				vertexColorArray[i].y = m_green * (yScaleColor * m_panadapterBins.at(i));
				vertexColorArray[i].z = m_blue  * (yScaleColor * m_panadapterBins.at(i));
				mutex.unlock();

				vertexArrayBg[2*i].x = (GLfloat)(i/m_scaleMult);
				vertexArrayBg[2*i].y = (GLfloat)(yTop - yScale * m_panadapterBins.at(i));
				vertexArrayBg[2*i].z = -2.5;

				vertexArrayBg[2*i+1].x = (GLfloat)(i/m_scaleMult);
				vertexArrayBg[2*i+1].y = (GLfloat)yTop;
				vertexArrayBg[2*i+1].z = -2.5;
	
				vertexArray[i].x = (GLfloat)(i/m_scaleMult);
				vertexArray[i].y = (GLfloat)(yTop - yScale * m_panadapterBins.at(i));
				vertexArray[i].z = -1.0;
			}
			
			glEnableClientState(GL_VERTEX_ARRAY);
			glEnableClientState(GL_COLOR_ARRAY);
				
			glVertexPointer(3, GL_FLOAT, 0, vertexArrayBg);
			glColorPointer(3, GL_FLOAT, 0, vertexColorArrayBg);
			glDrawArrays(GL_QUAD_STRIP, 0, 2*vertexArrayLength);

			glVertexPointer(3, GL_FLOAT, 0, vertexArray);
			glColorPointer(3, GL_FLOAT, 0, vertexColorArray);
			glDrawArrays(GL_LINE_STRIP, 0, vertexArrayLength);

			glDisableClientState(GL_VERTEX_ARRAY);
			glDisableClientState(GL_COLOR_ARRAY); 

			delete[] vertexArray;
			delete[] vertexColorArray;
			delete[] vertexArrayBg;
			delete[] vertexColorArrayBg;

			break;

		case QSDRGraphics::Line:

			for (int i = 0; i < vertexArrayLength; i++) {
	
				mutex.lock();
				vertexColorArray[i].x = m_red;
				vertexColorArray[i].y = m_green;
				vertexColorArray[i].z = m_blue;
				mutex.unlock();
				
				vertexArray[i].x = (GLfloat)(i/m_scaleMult);
				vertexArray[i].y = (GLfloat)(yTop - yScale * m_panadapterBins.at(i));
				vertexArray[i].z = -1.0;
			}
		
			glEnableClientState(GL_VERTEX_ARRAY);
			glEnableClientState(GL_COLOR_ARRAY);
				
			glVertexPointer(3, GL_FLOAT, 0, vertexArray);
			glColorPointer(3, GL_FLOAT, 0, vertexColorArray);
			glDrawArrays(GL_LINE_STRIP, 0, vertexArrayLength);
			
			glDisableClientState(GL_VERTEX_ARRAY);
			glDisableClientState(GL_COLOR_ARRAY);

			delete[] vertexArray;
			delete[] vertexColorArray;
			delete[] vertexArrayBg;
			delete[] vertexColorArrayBg;

			break;

		case QSDRGraphics::Solid:

			glDisable(GL_MULTISAMPLE);
			glDisable(GL_LINE_SMOOTH);

			for (int i = 0; i < vertexArrayLength; i++) {
			
				mutex.lock();
				vertexColorArrayBg[2*i].x = m_redST;
				vertexColorArrayBg[2*i].y = m_greenST;
				vertexColorArrayBg[2*i].z = m_blueST;
								
				vertexColorArrayBg[2*i+1].x = m_redSB;
				vertexColorArrayBg[2*i+1].y = m_greenSB;
				vertexColorArrayBg[2*i+1].z = m_blueSB;
				mutex.unlock();

				vertexArrayBg[2*i].x = (GLfloat)(i/m_scaleMult);
				vertexArrayBg[2*i].y = (GLfloat)(yTop - yScale * m_panadapterBins.at(i));
				vertexArrayBg[2*i].z = -2.0f;

				vertexArrayBg[2*i+1].x = (GLfloat)(i/m_scaleMult);
				vertexArrayBg[2*i+1].y = (GLfloat)yTop;
				vertexArrayBg[2*i+1].z = -2.0f;
			}
			
			glEnableClientState(GL_VERTEX_ARRAY);
			glEnableClientState(GL_COLOR_ARRAY);
				
			glVertexPointer(3, GL_FLOAT, 0, vertexArrayBg);
			glColorPointer(3, GL_FLOAT, 0, vertexColorArrayBg);
			//glDrawArrays(GL_QUAD_STRIP, 0, 2*vertexArrayLength);
			glDrawArrays(GL_LINES, 0, 2*vertexArrayLength);

			glDisableClientState(GL_VERTEX_ARRAY);
			glDisableClientState(GL_COLOR_ARRAY);

			//glEnable(GL_MULTISAMPLE);
			//glEnable(GL_LINE_SMOOTH);

			delete[] vertexArray;
			delete[] vertexColorArray;
			delete[] vertexArrayBg;
			delete[] vertexColorArrayBg;

			break;
	}

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
	glColor3f(0.65, 0.76, 0.81);
	
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);
	
	if (!m_dBmScaleFBO || m_dBmScalePanadapterUpdate || m_dBmScalePanadapterRenew)	{

		if (!m_dBmScaleFBO || m_dBmScalePanadapterRenew) {

			if (m_dBmScaleFBO) {
			
				delete m_dBmScaleFBO;
				m_dBmScaleFBO = 0;
			}
			m_dBmScaleFBO = new QGLFramebufferObject(width, height);
			if (m_dBmScaleFBO)
				GRAPHICS_DEBUG << "m_dBmScaleFBO generated.";
			
		}

		m_dBmScaleFBO->bind();
			renderPanVerticalScale();
		m_dBmScaleFBO->release();
		
		m_dBmScalePanadapterUpdate = false;
		m_dBmScalePanadapterRenew = false;
	}
		
	renderTexture(m_dBmScalePanRect, m_dBmScaleFBO->texture(), 0.0f);

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
	glColor3f(0.65, 0.76, 0.81);
}

void QGLReceiverPanel::drawPanHorizontalScale() {

	if (!m_freqScalePanRect.isValid()) return;

	int width = m_freqScalePanRect.width();
	int height = m_freqScalePanRect.height();

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	glColor3f(0.65, 0.76, 0.81);
	
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);

	if (!m_frequencyScaleFBO || m_freqScalePanadapterUpdate || m_freqScalePanadapterRenew) {

		if (!m_frequencyScaleFBO || m_freqScalePanadapterRenew) {

			if (m_frequencyScaleFBO) {
			
				delete m_frequencyScaleFBO;
				m_frequencyScaleFBO = 0;
			}

			m_frequencyScaleFBO = new QGLFramebufferObject(width, height);
			if (m_frequencyScaleFBO)
				GRAPHICS_DEBUG << "m_frequencyScaleFBO generated.";
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
	glColor3f(0.65, 0.76, 0.81);
}

void QGLReceiverPanel::drawPanadapterGrid() {

	if (!m_panRect.isValid()) return;

	int width = m_panRect.width();
	int height = m_panRect.height();

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	//glColor4f(0.35, 0.46, 0.51, 0.7);
	//glColor4f(0.45, 0.56, 0.61, 1.0);
	if (m_receiver == m_currentReceiver)
		glColor4f(0.45, 0.56, 0.61, 0.8);
	else
		glColor4f(0.45, 0.56, 0.61, 0.6);
	
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
			if (m_panadapterGridFBO)
				GRAPHICS_DEBUG << "m_panadapterGridFBO generated.";
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

	//renderTexture(m_panRect, m_panadapterGridFBO->texture(), -2.0f);
	//renderTexture(m_panRect, m_panadapterGridFBO->texture(), -1.5f);
	renderTexture(m_panRect, m_panadapterGridFBO->texture(), -2.0f);
	
	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
	glColor3f(0.65, 0.76, 0.81);
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
		m_oglTextSmall->renderText(m_filterLeft + 5, m_filterTop + 44, 4.0f, str1);
		m_oglTextSmall->renderText(m_filterLeft + 5, m_filterTop + 64, 4.0f, str2);

		qglColor(QColor(255, 255, 255, 255));
		m_oglTextSmall->renderText(m_filterLeft + 3, m_filterTop + 42, 5.0f, str1);
		m_oglTextSmall->renderText(m_filterLeft + 3, m_filterTop + 62, 5.0f, str2);
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
		m_oglTextSmall->renderText(m_filterRight + 5, m_filterTop + 44, 4.0f, str1);
		m_oglTextSmall->renderText(m_filterRight + 5, m_filterTop + 64, 4.0f, str2);

		qglColor(QColor(255, 255, 255, 255));
		m_oglTextSmall->renderText(m_filterRight + 3, m_filterTop + 42, 5.0f, str1);
		m_oglTextSmall->renderText(m_filterRight + 3, m_filterTop + 62, 5.0f, str2);
	}

	// draw a line for the display center
	GLint y1 = m_panRect.top() + 3;
	GLint y2 = m_displayCenterlineHeight;
	
	if (y2 > y1 + 3) {

		GLint x = m_panRect.width()/2;
			
		color = m_settings->getPanadapterColors().panCenterLineColor;

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
	if (m_settings->getFBOPresence()) {
	
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
				if (m_waterfallLineFBO)
				GRAPHICS_DEBUG << "m_waterfallLineFBO generated.";

				m_waterfallFBO = new QGLFramebufferObject(width, height);
				if (m_waterfallFBO)
				GRAPHICS_DEBUG << "m_waterfallFBO generated.";

				m_textureFBO = new QGLFramebufferObject(width, height);
				if (m_textureFBO)
				GRAPHICS_DEBUG << "m_textureFBO generated.";
				
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

void QGLReceiverPanel::drawCrossHair() {

	QRect rect(0, m_panRect.top(), width(), height() - m_panRect.top());

	int x = m_mousePos.x();
	int y = m_mousePos.y();

	glDisable(GL_MULTISAMPLE);
	glDisable(GL_BLEND);
	//glEnable(GL_BLEND);
	//glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	glDisable(GL_LINE_SMOOTH);
	glLineWidth(1.0f);

	//qglColor(QColor(255, 255, 255, 80));
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
	//qglColor(QColor(255, 255, 255, 180));
	qglColor(QColor(255, 255, 255, 255));
	glBegin(GL_LINES);
		glVertex3f(x     , y - 20, 5.0f);
		glVertex3f(x     , y + 20, 5.0f);
		glVertex3f(x - 20, y, 5.0f);
		glVertex3f(x + 20, y, 5.0f);
	glEnd();

	// text only on panadapter
	//if (m_mouseRegion == panadapterRegion) {
		
	QString str;
	qglColor(QColor(255, 55, 55, 255));

	int dx = m_panRect.width()/2 - x;
	qreal unit = (qreal)((m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width());
	qreal df = unit * dx;
	qreal frequency = m_frequency - df;
	
	str = frequencyString(-df, true);
	if (x > m_panRect.width() - m_haircrossMaxRight) {

		if (y > m_haircrossMinTop)
			m_oglTextSmall->renderText(x - m_haircrossOffsetLeft, y - 42, 5.0f, str);
		else
			m_oglTextSmall->renderText(x - m_haircrossOffsetLeft, y + 6, 5.0f, str);
	}
	else {

		if (y > m_haircrossMinTop)
			m_oglTextSmall->renderText(x + m_haircrossOffsetRight, y - 42, 5.0f, str);
		else
			m_oglTextSmall->renderText(x + m_haircrossOffsetRight, y + 6, 5.0f, str);
	}

	qglColor(QColor(255, 255, 255, 255));

	str = frequencyString(frequency);
	if (x > m_panRect.width() - m_haircrossMaxRight) {

		if (y > m_haircrossMinTop)
			m_oglTextSmall->renderText(x - m_haircrossOffsetLeft, y - 30, 5.0f, str);
		else
			m_oglTextSmall->renderText(x - m_haircrossOffsetLeft, y + 18, 5.0f, str);
	}
	else {

		if (y > m_haircrossMinTop)
			m_oglTextSmall->renderText(x + m_haircrossOffsetRight, y - 30, 5.0f, str);
		else
			m_oglTextSmall->renderText(x + m_haircrossOffsetRight, y + 18, 5.0f, str);
	}

	if (m_mouseRegion == panadapterRegion) {

		qreal dBm = m_dBmPanMax - ((m_dBmPanMax - m_dBmPanMin) * ((qreal)(y - m_panRect.top()) / m_panRect.height()));
		str = QString::number(dBm, 'f', 1) + " dBm";

		if (x > m_panRect.width() - m_haircrossMaxRight) {

			if (y > m_haircrossMinTop)
				m_oglTextSmall->renderText(x - m_haircrossOffsetLeft, y - 18, 5.0f, str);
			else
				m_oglTextSmall->renderText(x - m_haircrossOffsetLeft, y + 30, 5.0f, str);
		}
		else {

			if (y > m_haircrossMinTop)
				m_oglTextSmall->renderText(x + m_haircrossOffsetRight, y - 18, 5.0f, str);
			else
				m_oglTextSmall->renderText(x + m_haircrossOffsetRight, y + 30, 5.0f, str);
		}
	}

	glDisable(GL_SCISSOR_TEST);
	glEnable(GL_MULTISAMPLE);

	//glEnable(GL_BLEND);
	//glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
}

void QGLReceiverPanel::drawReceiverInfo() {

	/*if (m_oldFreq != m_frequency) {

		m_bandText = getHamBandTextString(m_settings->getHamBandTextList(), m_frequency);
	}
	QByteArray ba = m_bandText.toLatin1();
	char *bstr = ba.data();*/

	// draw a line for the display center
	//y1 = m_panRect.top() + 3;
	//y2 = m_displayCenterlineHeight;
	//
	//if (y2 > y1 + 3) {

	//	GLint x = m_panRect.width()/2;
	//		
	//	QColor color = m_settings->getPanadapterColors().panCenterLineColor;

	//	//glDisable(GL_LINE_SMOOTH);
	//	glDisable(GL_MULTISAMPLE);
	//	glLineWidth(1);
	//	glColor4ub(color.red(), color.green(), color.blue(), color.alpha());
	//	glBegin(GL_LINES);
	//		/*glVertex2i(x, y1);
	//		glVertex2i(x, y2);*/
	//		glVertex3f(x, y1, 4.0f);
	//		glVertex3f(x, y2, 4.0f);
	//	glEnd();
	//	glEnable(GL_MULTISAMPLE);
	//}

	QString str;
	if (m_settings->getNumberOfReceivers() > 1) {
	
		if (m_receiver == m_settings->getCurrentReceiver())
			glColor3f(0.94, 0.82, 0.43);
		else
			glColor3f(0.54, 0.42, 0.13);
	
		str = QString("Rx %1").arg(m_receiver + 1);
		m_oglTextSmall->renderText(m_panRect.width() - 30, 5, str);
	}

	// main frequency display
	if (m_panRect.height() > 15) {

	int alpha;
	if (m_receiver == m_settings->getCurrentReceiver())
		alpha = 255;
	else
		alpha = 155;

	TFrequency f;
	f.freqMHz = (int)(m_frequency / 1000);
	f.freqkHz = (int)(m_frequency % 1000);

	str = "%1.%2";
	int f1 = f.freqMHz;
	int f2 = f.freqkHz;

	GLint x = m_panRect.width()/2 - 65;
	
	qglColor(QColor(0, 0, 0, alpha));
	m_oglTextFreq1->renderText(x+2, 7, 4.0f, str.arg(f1/1000).arg(f1 - 1000 * (int)(f1/1000), 3, 10, QLatin1Char('0')));
	qglColor(QColor(255, 255, 255, alpha));
	m_oglTextFreq1->renderText(x, 5, 5.0f, str.arg(f1/1000).arg(f1 - 1000 * (int)(f1/1000), 3, 10, QLatin1Char('0')));
	
	str = "%1";
	if (f1 / 1000 < 10) {

		qglColor(QColor(0, 0, 0, alpha));
		m_oglTextFreq2->renderText(x + 36, 7, 4.0f, str.arg(f2, 3, 10, QLatin1Char('0')));
		m_oglTextFreq1->renderText(x + 60, 7, 4.0f, "MHz");

		qglColor(QColor(255, 255, 255, alpha));
		m_oglTextFreq2->renderText(x + 34, 5, 5.0f, str.arg(f2, 3, 10, QLatin1Char('0')));
		m_oglTextFreq1->renderText(x + 58, 5, 5.0f, "MHz");
	}
	else {

		qglColor(QColor(0, 0, 0, alpha));
		m_oglTextFreq2->renderText(x + 39, 7, 4.0f, str.arg(f2, 3, 10, QLatin1Char('0')));
		m_oglTextFreq1->renderText(x + 65, 7, 4.0f, "MHz");

		qglColor(QColor(255, 255, 255, alpha));
		m_oglTextFreq2->renderText(x + 41, 5, 5.0f, str.arg(f2, 3, 10, QLatin1Char('0')));
		m_oglTextFreq1->renderText(x + 63, 5, 5.0f, "MHz");
	}
	}
	//m_oglTextFreq1->renderText(x + 126, 15 - 1, "MHz");
}

void QGLReceiverPanel::drawAGCControl() {
}
 
//**********************************************************************************************
// The algorithms of the scale functions renderPanVerticalScale() and renderPanHorizontalScale() 
// are taken from SDRMAXIII (c) Catherine Moss, with permission.

void QGLReceiverPanel::renderPanVerticalScale() {

	QString str;
	QFontMetrics d_fm(m_smallFont);
	//QFontMetrics d_fm(m_normalFont);
	int spacing = 7;
	int fontHeight = d_fm.tightBoundingRect(".0dBm").height() + spacing;
	int fontMaxWidth = d_fm.boundingRect("-000.0").width();

	GLint width = m_dBmScalePanRect.width();
	GLint height = m_dBmScalePanRect.height();

	qreal unit = (qreal)(m_dBmScalePanRect.height() / qAbs(m_dBmPanMax - m_dBmPanMin));

	m_dBmScale = getYRuler2(m_dBmScalePanRect, fontHeight, unit, m_dBmPanMin, m_dBmPanMax);

	glClear(GL_COLOR_BUFFER_BIT);
	
	QRect textRect(0, 0, fontMaxWidth, fontHeight);
	textRect.moveLeft(3);
	int yOld = -textRect.height();

	int len		= m_dBmScale.mainPointPositions.length();
	int sublen	= m_dBmScale.subPointPositions.length();
	
	glViewport(0, 0, width, height);
	setProjectionOrthographic(width, height);

	// draw the scale background
	drawGLScaleBackground(QRect(0, 0, width, height), QColor(30, 30, 30, 180));
	
	if (len > 0) {

		glColor3f(0.65, 0.76, 0.81);
		glLineWidth(1);

		glBegin(GL_LINES);
		for (int i = 0; i < len; i++) {

			glVertex3f(width,     m_dBmScale.mainPointPositions.at(i), 0.0f);	// origin of the line
			glVertex3f(width - 4, m_dBmScale.mainPointPositions.at(i), 0.0f);	// ending point of the line
		}
		glEnd();
		
		glColor3f(0.45, 0.56, 0.61);
		if (sublen > 0) {

			glBegin(GL_LINES);
			for (int i = 0; i < sublen; i++) {

				glVertex3f(width,     m_dBmScale.subPointPositions.at(i), 0.0f);	// origin of the line
				glVertex3f(width - 2, m_dBmScale.subPointPositions.at(i), 0.0f);	// ending point of the line
			}
			glEnd();
		}

		glColor3f(0.75, 0.86, 0.91);
		for (int i = 0; i < len; i++) {

			textRect.moveBottom(m_dBmScale.mainPointPositions.at(i) + textRect.height()/2);
			
			if (textRect.y() >= yOld && textRect.bottom() <= (m_dBmScalePanRect.height() - textRect.height())) {
			
				str = QString::number(m_dBmScale.mainPoints.at(i), 'f', 1);
				m_oglTextSmall->renderText(textRect.x() + fontMaxWidth - d_fm.tightBoundingRect(str).width(), textRect.y(), str);
				yOld = textRect.bottom();
			}
		}
	}

	textRect.moveTop(m_dBmScalePanRect.height() - textRect.height());
	glColor3f(0.94, 0.22, 0.43);
	
	str = QString("dBm");
	m_oglTextSmall->renderText(textRect.x(), textRect.y(), str);

	glViewport(0, 0, size().width(), size().height());
	setProjectionOrthographic(size().width(), size().height());
}

void QGLReceiverPanel::renderPanHorizontalScale() {

	//GRAPHICS_DEBUG << "render frequency scale";
	QFontMetrics d_fm(m_smallFont);
	int fontHeight = d_fm.tightBoundingRect(".0kMGHz").height();
	int fontMaxWidth = d_fm.boundingRect("000.000.0").width();

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
	scaledTextRect.setWidth(d_fm.width(fstr));
	scaledTextRect.moveLeft(m_freqScalePanRect.width() - scaledTextRect.width());

	glColor3f(0.65, 0.76, 0.81);
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

			int text_width = d_fm.width(str);
			QRect textRect(m_frequencyScale.mainPointPositions.at(i) + offset_X - (text_width / 2), textOffset_y, text_width, fontHeight);

			if (textRect.left() < 0 || textRect.right() >= scaledTextRect.left()) continue;
			
			m_oglTextSmall->renderText(textRect.x(), textRect.y(), str);
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

	glColor3f(0.94, 0.22, 0.43);
	m_oglTextSmall->renderText(m_freqScalePanRect.width() - 30, textOffset_y, fstr);
}

void QGLReceiverPanel::renderPanadapterGrid() {

	//GRAPHICS_DEBUG << "render panadapter grid";
	//glLineStipple(1, 0xCCCC);
	glClear(GL_COLOR_BUFFER_BIT);
	glLineStipple(1, 0x9999);
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
 

//********************************************************************

void QGLReceiverPanel::getRegion(QPoint p) {

	QRect mouse_rect(0, 0, 100, 100);
	mouse_rect.moveCenter(p);

	if (m_freqScalePanRect.contains(p)) {

		m_mouseRegion = freqScalePanadapterRegion;
		
		if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}
	}
	else if (m_dBmScalePanRect.contains(p)) {

		m_mouseRegion = dBmScalePanadapterRegion;

		if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}
	}
	else if (abs(p.x() - m_filterRect.left()) < m_snapMouse &&
			 m_panRect.contains(p)
	) {
		m_mouseRegion = filterRegionLow;
		m_mouseDownFilterFrequencyLo = m_filterLowerFrequency;
	}
	else if (abs(p.x() - m_filterRect.right()) < m_snapMouse &&
			 m_panRect.contains(p)
	) {
		m_mouseRegion = filterRegionHigh;
		m_mouseDownFilterFrequencyHi = m_filterUpperFrequency;
	}
	else if (m_filterRect.contains(p)) {

		m_mouseRegion = filterRegion;

		if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}
	}
	else if (m_panRect.contains(p)) {

		m_mouseRegion = panadapterRegion;

		if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}
	}
	else if (m_waterfallRect.contains(p)) {

		m_mouseRegion = waterfallRegion;

		if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}
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
	
	glFinish();

	m_resizeTime.restart();
	setupDisplayRegions(QSize(width, height));
    glViewport(0, 0, (GLsizei)width, (GLsizei)height);

	setProjectionOrthographic(width, height);
}

void QGLReceiverPanel::setupDisplayRegions(QSize size) {

	m_displayTop = 0;
	int freqScaleRectHeight = 20;
	int dBmScaleWidth = 45;
	
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
	
	m_dBmScalePanRect = QRect(
						m_panRect.left(), 
						m_displayTop, 
						45, 
						m_panRect.height());

	m_freqScalePanadapterUpdate = true;
	m_dBmScalePanadapterUpdate = true;
	m_panGridUpdate = true;
	
	/*GRAPHICS_DEBUG << "***************************************************************************";
	GRAPHICS_DEBUG << "receiver:" << m_receiver;
	GRAPHICS_DEBUG << "total size" << size.height();
	GRAPHICS_DEBUG << "sizes (top, bottom, height):";
	GRAPHICS_DEBUG << "panRect" << m_panRect.top() << m_panRect.bottom() << m_panRect.height();
	GRAPHICS_DEBUG << "waterfallRect" << m_waterfallRect.top() << m_waterfallRect.bottom() << m_waterfallRect.height();
	GRAPHICS_DEBUG << "freqScalePanRect" << m_freqScalePanRect.top() << m_freqScalePanRect.bottom() << m_freqScalePanRect.height();
	GRAPHICS_DEBUG << "dBmScalePanRect" << m_dBmScalePanRect.top() << m_dBmScalePanRect.bottom() << m_dBmScalePanRect.height();
	GRAPHICS_DEBUG << "";*/
	
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

	double freqStep = m_settings->getMouseWheelFreqStep();

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

			m_settings->setFrequency(this, true, m_receiver, m_frequency);

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

	if (m_mouseRegion == panadapterRegion) {

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

			double freqStep = m_settings->getMouseWheelFreqStep();
			int dx = m_panRect.width()/2 - m_mousePos.x();
			qreal unit = (qreal)((m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width());
			m_frequency -= unit * dx;

			m_frequency = (long)(qRound(m_frequency / qAbs(freqStep)) * qAbs(freqStep));
				
			m_settings->setFrequency(this, true, m_receiver, m_frequency);
			update();
		}
		else if (event->buttons() == Qt::LeftButton) {

			if (m_receiver != m_settings->getCurrentReceiver())
				m_settings->setReceiver(this, m_receiver);
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

			double freqStep = m_settings->getMouseWheelFreqStep();
			int dx = m_panRect.width()/2 - m_mousePos.x();
			qreal unit = (qreal)((m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width());
			m_frequency -= unit * dx;

			m_frequency = (long)(qRound(m_frequency / qAbs(freqStep)) * qAbs(freqStep));
				
			m_settings->setFrequency(this, true, m_receiver, m_frequency);
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

void QGLReceiverPanel::mouseMoveEvent(QMouseEvent* event) {
	
	//GRAPHICS_DEBUG << "mouseMoveEvent";
	QPoint pos = event->pos();
	m_mousePos = event->pos();

	if (event->buttons() == Qt::NoButton) getRegion(pos);

	switch (m_mouseRegion) {

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

				m_settings->setFrequency(this, true, m_receiver, m_frequency);
				m_mouseDownPos = pos;
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

					m_settings->setdBmPanScaleMin(m_receiver, m_dBmPanMin);
					m_settings->setdBmPanScaleMax(m_receiver, m_dBmPanMax);
				}
				
				m_mouseDownPos = pos;
				m_dBmScalePanadapterUpdate = true;
				m_panGridUpdate = true;
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

				m_settings->setdBmPanScaleMin(m_receiver, m_dBmPanMin);
				m_settings->setdBmPanScaleMax(m_receiver, m_dBmPanMax);

				m_mouseDownPos = pos;
				m_dBmScalePanadapterUpdate = true;
				m_panGridUpdate = true;
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

				m_settings->setdBmPanScaleMin(m_receiver, m_dBmPanMin);
				m_settings->setdBmPanScaleMax(m_receiver, m_dBmPanMax);

				m_mouseDownPos = pos;
				m_dBmScalePanadapterUpdate = true;
				m_panGridUpdate = true;
			}
			else
				setCursor(Qt::ArrowCursor);
			
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
				m_settings->setFreqRulerPosition(m_freqRulerPosition, m_receiver);
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
				m_settings->setRXFilter(this, m_receiver, m_filterLowerFrequency, m_filterUpperFrequency);
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
				m_settings->setRXFilter(this, m_receiver, m_filterLowerFrequency, m_filterUpperFrequency);
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
				m_settings->setRXFilter(this, m_receiver, m_filterLowerFrequency, m_filterUpperFrequency);
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

void QGLReceiverPanel::timerEvent(QTimerEvent *) {
	
	update();
}
 
//********************************************************************
 
void QGLReceiverPanel::setFrequency(QObject *sender, bool value, int rx, long freq) {

	Q_UNUSED(sender)
	Q_UNUSED(value)
	
	for (int i = 0; i < m_settings->getNumberOfReceivers(); i++) {

		m_rxDataList[i].frequency = freq;
	}

	if (rx == m_receiver) {
		
		m_frequency = freq;
		m_freqScalePanadapterUpdate = true;
		m_panGridUpdate = true;
	}
	/*else {

		m_otherFrequency = freq;
		GRAPHICS_DEBUG << "freq from Rx " << rx << " : " << m_otherFrequency;
	}*/

	update();
}

void QGLReceiverPanel::setFilterFrequencies(QObject *sender, int rx, qreal lo, qreal hi) {

	Q_UNUSED(sender)

	if (m_receiver == rx) {
		
		m_filterLowerFrequency = lo;
		m_filterUpperFrequency = hi;
		m_filterChanged = true;
	}

	update();
}

void QGLReceiverPanel::setCurrentReceiver(int value) {

	m_currentReceiver = value;
	if (m_receiver != m_currentReceiver) m_crossHairCursor = false;

	m_panGridUpdate = true;
	update();
}

void QGLReceiverPanel::setCoupledRx(QObject* sender, int value) {

	Q_UNUSED(sender)

	m_frequencyRxOnRx = value;
	GRAPHICS_DEBUG << "showing frequency of Rx " << m_frequencyRxOnRx/10 << " on Rx " << m_frequencyRxOnRx%10;
}

void QGLReceiverPanel::freqRulerPositionChanged(float pos, int rx) {

	if (rx == m_receiver) {
		
		m_freqRulerPosition = pos;

		setupDisplayRegions(size());
		update();
	}
}

void QGLReceiverPanel::setSpectrumBuffer(const float *buffer) {

	if (m_spectrumAveraging) {
	
		QVector<float> specBuf(m_spectrumSize);
		QVector<float> avg;

		//spectrumBufferMutex.lock();

		memcpy(
			(float *) specBuf.data(),
			(float *) &buffer[0],
			m_spectrumSize * sizeof(float));

		specAv_queue.enqueue(specBuf);
		if (specAv_queue.size() <= m_specAveragingCnt) {
	
			for (int i = 0; i < m_spectrumSize; i++) {
				
				if (m_tmp.size() < m_spectrumSize)
					m_tmp << specAv_queue.last().data()[i];
				else
					m_tmp[i] += specAv_queue.last().data()[i];
			}
			//spectrumBufferMutex.unlock();
			return;
		}
	
		for (int i = 0; i < m_spectrumSize; i++) {

			m_tmp[i] -= specAv_queue.first().at(i);
			m_tmp[i] += specAv_queue.last().at(i);
			avg << m_tmp.at(i) * m_scale;
		}

		computeDisplayBins(avg, buffer);
		specAv_queue.dequeue();
	
		avg.clear();
		//spectrumBufferMutex.unlock();
	}
	else {

		QVector<float> specBuf(m_spectrumSize);

		memcpy(
			(float *) specBuf.data(),
			(float *) &buffer[0],
			m_spectrumSize * sizeof(float));

		computeDisplayBins(specBuf, buffer);
	}
}

void QGLReceiverPanel::computeDisplayBins(const QVector<float> &panBuffer, const float *waterfallBuffer) {
//void QGLReceiverPanel::computeDisplayBins(const float *panBuffer, const float *waterfallBuffer) {

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

		/*newSampleSize = (int)floor(4 * BUFFER_SIZE * m_freqScaleZoomFactor);
		deltaSampleSize = 4 * BUFFER_SIZE - newSampleSize;*/
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
	
	for (int i = 0; i < m_panSpectrumBinsLength; i++) {
			
		idx = 0;
		lIdx = (int)floor((qreal)(i * m_panScale / m_scaleMult));
		rIdx = (int)floor((qreal)(i * m_panScale / m_scaleMult) + m_panScale / m_scaleMult);
					
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
		if (m_Preamp) {

			m_panadapterBins << panBuffer[idx] - m_dBmPanMin - m_dBmPanLogGain - 20.0f;
			pColor = getWaterfallColorAtPixel(waterfallBuffer[idx] - m_dBmPanLogGain - 20.0f);
		}
		else {

			m_panadapterBins << panBuffer[idx] - m_dBmPanMin - m_dBmPanLogGain;
			pColor = getWaterfallColorAtPixel(waterfallBuffer[idx] - m_dBmPanLogGain);
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

void QGLReceiverPanel::systemStateChanged(
	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	Q_UNUSED (sender)
	Q_UNUSED (err)
	Q_UNUSED (hwmode)
	Q_UNUSED (state)

	//bool change = false;

	if (m_dataEngineState != state) {
		
		m_dataEngineState = state;
		//change = true;
	}

	if (m_serverMode == mode) 
		return;
	else {

		memset(m_spectrumBuffer, -10000, 4 * BUFFER_SIZE * sizeof(float));
		m_serverMode = mode;
	}

	resizeGL(width(), height());
	m_displayTime.restart();
	
	update();
}

 
void QGLReceiverPanel::graphicModeChanged(
	QObject *sender,
	QSDRGraphics::_Panadapter panMode,
	QSDRGraphics::_WaterfallColorScheme colorScheme)
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

void QGLReceiverPanel::sampleRateChanged(QObject *sender, int value) {

	Q_UNUSED(sender)
	
	m_sampleRate = value;
	m_filterChanged = true;

	update();
}

void QGLReceiverPanel::setPreamp(QObject* sender, int value) {

	Q_UNUSED(sender)
	
	m_Preamp = value;
	GRAPHICS_DEBUG << "m_Preamp" << m_Preamp;
	update();
}

void QGLReceiverPanel::setPanadapterColors() {

	m_spectrumColorsChanged = true;

	mutex.lock();
	m_bkgRed   = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.red() / 256.0);
	m_bkgGreen = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.green() / 256.0);
	m_bkgBlue  = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.blue() / 256.0);

	m_red	= (GLfloat)(m_settings->getPanadapterColors().panLineColor.red() / 256.0);
	m_green = (GLfloat)(m_settings->getPanadapterColors().panLineColor.green() / 256.0);
	m_blue	= (GLfloat)(m_settings->getPanadapterColors().panLineColor.blue() / 256.0);

	m_redF	 = (GLfloat)(m_settings->getPanadapterColors().panLineFilledColor.red() / 256.0);
	m_greenF = (GLfloat)(m_settings->getPanadapterColors().panLineFilledColor.green() / 256.0);
	m_blueF  = (GLfloat)(m_settings->getPanadapterColors().panLineFilledColor.blue() / 256.0);

	m_redST	  = (GLfloat)(m_settings->getPanadapterColors().panSolidTopColor.red() / 256.0);
	m_greenST = (GLfloat)(m_settings->getPanadapterColors().panSolidTopColor.green() / 256.0);
	m_blueST  = (GLfloat)(m_settings->getPanadapterColors().panSolidTopColor.blue() / 256.0);

	m_redSB   = (GLfloat)(m_settings->getPanadapterColors().panSolidBottomColor.red() / 256.0);
	m_greenSB = (GLfloat)(m_settings->getPanadapterColors().panSolidBottomColor.green() / 256.0);
	m_blueSB  = (GLfloat)(m_settings->getPanadapterColors().panSolidBottomColor.blue() / 256.0);

	m_waterfallMidColor = m_settings->getPanadapterColors().waterfallColor.toRgb() ;
	mutex.unlock();

	update();
}

void QGLReceiverPanel::setWaterfallTime(int rx, int value) {

	if (m_receiver == rx)
		m_waterfallTime = value;
}

void QGLReceiverPanel::setWaterfallOffesetLo(int rx, int value) {

	if (m_receiver == rx)
		m_waterfallOffsetLo = value;
	
	update();
}

void QGLReceiverPanel::setWaterfallOffesetHi(int rx, int value) {

	if (m_receiver == rx)
		m_waterfallOffsetHi = value;
	
	update();
}

void QGLReceiverPanel::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void QGLReceiverPanel::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}
