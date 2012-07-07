/**
* @file  cusdr_oglDistancePanel.cpp
* @brief distance panel class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-02-14
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

#define LOG_GRAPHICS

#include "cusdr_oglDistancePanel.h"

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

QGLDistancePanel::QGLDistancePanel(QWidget *parent)
		: QGLWidget(QGLFormat(QGL::SampleBuffers|QGL::AlphaChannel), parent)

		, m_settings(Settings::instance())
		, m_serverMode(m_settings->getCurrentServerMode())
		, m_hwInterface(m_settings->getHWInterface())
		, m_dataEngineState(QSDR::DataEngineDown)
		, m_sampleRate(m_settings->getSampleRate())
		, m_downRate(m_settings->getChirpDownSampleRate())
		, m_spectrumAveraging(m_settings->getSpectrumAveraging())
		, m_spectrumAveragingOld(m_spectrumAveraging)
		, m_specAveragingCnt(m_settings->getSpectrumAveragingCnt())
		, m_mousePos(QPoint(-1, -1))
		, m_mouseDownPos(QPoint(-1, -1))
		, m_snapMouse(3)
		, m_spectrumUpdate(false)
		, m_spectrumVertexColorUpdate(false)
		, m_spectrumColorsChanged(true)
		, m_freqScaleZoomFactor(1.0f)
		, m_freqRulerPosition(0.5)
		, m_freqRulerDisplayWidth(0)
		, m_scaleMult(1.0f)
		, m_filterLowerFrequency(-3050.0)
		, m_filterUpperFrequency(-150.0)
		, m_distScaleZoomFactor(1.0) // 0.2 .. 1.0
		, m_distRulerMaxDist(150000.0)
		, m_distRulerDisplayDelta(0.0)
		, m_distRulerDisplayDeltaStep(500.0)
		, m_panSpectrumMinimumHeight(70)
		, m_panadapterMode(m_settings->getPanadapterMode())
		, m_chirpBufferLength(m_settings->getChirpBufferLength())
		, m_showChirpFFT(false)
		, m_showZerodBmLine(false)
		, m_crossHairCursor(false)
		, m_panGrid(m_settings->getPanGridStatus())
{
	QGL::setPreferredPaintEngine(QPaintEngine::OpenGL);

	setAutoBufferSwap(true);
	setAutoFillBackground(false);
	
	setMouseTracking(true);
	setFocusPolicy(Qt::StrongFocus);

	setupDisplayRegions(size());
	m_oldWidth = size().width();
	
	m_rxDataList = m_settings->getReceiverDataList();
	m_frequency = m_rxDataList.at(0).frequency;

	m_dBmScalePanadapterRenew = true;
	m_dBmScalePanadapterUpdate = true;
	m_freqScalePanadapterRenew = true;
	m_freqScalePanadapterUpdate = true;
	m_panGridRenew = true;
	m_panGridUpdate = true;

	m_dBmPanMin = m_rxDataList.at(0).dBmPanScaleMin;
	m_dBmPanMax = m_rxDataList.at(0).dBmPanScaleMax;

	m_filterLowerFrequency = m_rxDataList.at(0).filterLo;
	m_filterUpperFrequency = m_rxDataList.at(0).filterHi;

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

	m_oglTextTiny = new OGLText(m_tinyFont);
	m_oglTextSmall = new OGLText(m_smallFont);
	m_oglTextNormal = new OGLText(m_normalFont);

	timer = 0;

	setupConnections();

	m_displayTime.start();
	m_resizeTime.start();
	//freqChangeTimer.start();
	
	memset(m_spectrumBuffer, -10000, 4 * BUFFER_SIZE * sizeof(float));
	memset(m_distanceSpectrumBuffer, -10000, 16 * BUFFER_SIZE * sizeof(float));

	m_kilometersPerGate = 0.5f * 3E5 / (m_sampleRate/m_downRate);

	m_dBmPanLogGain = 69; // allow user to calibrate this value

	m_dBmDistMin = m_settings->getdBmDistScaleMin();
	m_dBmDistMax = m_settings->getdBmDistScaleMax();
	m_dBmDistScaleMin = m_dBmDistMin;
	m_dBmDistScaleMax = m_dBmDistMax;

	m_panSpectrumBinsLength = 0;	
	
	/*m_bkgRed   = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.red() / 256.0);
	m_bkgGreen = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.green() / 256.0);
	m_bkgBlue  = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.blue() / 256.0);*/

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

	m_frequencyScaleFBO = 0;
	m_dBmScaleFBO = 0;
	m_panadapterGridFBO = 0;
	m_textureFBO = 0;
	
	if (m_specAveragingCnt > 0)
		m_scale = 1.0f / m_specAveragingCnt;
	else
		m_scale = 1.0f;

	memset(m_tmpBuf, 0, SAMPLE_BUFFER_SIZE * sizeof(float));

	updateGL();
}

QGLDistancePanel::~QGLDistancePanel() {

	disconnect(m_settings, 0, this, 0);
	
	makeCurrent();
	glFinish();

	if (m_frequencyScaleFBO) {

		delete m_frequencyScaleFBO;
		m_frequencyScaleFBO = 0;
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

//QSize QGLDistancePanel::minimumSizeHint() const {
//	
//	//return QSize(1000, 1024);
//	return QSize(width(), height());
//}

QSize QGLDistancePanel::sizeHint() const {
	
	return QSize(width(), height());
}

void QGLDistancePanel::setupConnections() {

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
		SIGNAL(sampleRateChanged(QObject *, int)), 
		this, 
		SLOT(sampleRateChanged(QObject *, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(filterFrequenciesChanged(QObject *, int, qreal, qreal)), 
		this, 
		SLOT(setFilterFrequencies(QObject *, int, qreal, qreal)));

	CHECKED_CONNECT_OPT(
		m_settings, 
		SIGNAL(chirpSpectrumBufferChanged(int, qint64, const float *)),
		this,
		SLOT(distanceSpectrumBufferChanged(int, qint64, const float *)),
		Qt::DirectConnection);

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(chirpFFTShowChanged(bool)),
		this,
		SLOT(setChirpFFTShow(bool)));

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

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(panadapterColorChanged()),
		this,
		SLOT(setPanadapterColors()));
}

void QGLDistancePanel::initializeGL() {

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

void QGLDistancePanel::paintGL() {

	switch (m_serverMode) {

		case QSDR::DttSP:
		case QSDR::QtDSP:
		case QSDR::ExternalDSP:
		//case QSDR::InternalDSP:

			drawGLRect(QRect(0, 0, width(), height()), QColor(0, 0, 0));
			break;

		case QSDR::ChirpWSPR:
		case QSDR::ChirpWSPRFile:

			//if (freqChangeTimer.elapsed() > 50)  m_spectrumAveraging = m_spectrumAveragingOld;

			if (m_resizeTime.elapsed() > 100 || m_dataEngineState == QSDR::DataEngineDown) {

				paintReceiverDisplay();
				paintChirpWSPRDisplay();
			}
			
			break;
	}
}
 
//****************************************************
// painting modes
void QGLDistancePanel::paintReceiverDisplay() {
 
	QRect mouse_rect(0, 0, 100, 100);
	mouse_rect.moveCenter(m_mousePos);

	drawPanadapter();
	drawPanHorizontalScale();
	drawPanVerticalScale();

	if (m_panGrid)
		drawPanadapterGrid();

	drawPanFilter();
	
	if (m_mouseRegion == panadapterRegion && m_crossHairCursor)
		drawCrossHair();

	m_oldFreq = m_frequency;
 }

void QGLDistancePanel::paintChirpWSPRDisplay() {

	//drawGLBackground(m_distanceSpectrumRect);

	distanceSpectrumBufferMutex.lock();
	drawDistanceSpectrum();
	distanceSpectrumBufferMutex.unlock();

	drawDistHorizontalScale();
	drawDistVerticalScale();
	
	//// chirp distance spectrum
	//m_distancePanadapter->drawGLDistanceRuler(
	//				m_freqScaleDistancePanRect,
	//				m_freqScalePanadapterRect,
	//				m_mouseRegion,
	//				m_distRulerZoomFactor,
	//				m_distRulerDisplayDelta,
	//				m_kilometersPerGate,
	//				m_showChirpFFT,
	//				m_chirpBufferLength);

	//m_distancePanadapter->drawGLdBmScale(
	//				m_dBmScaleDistancePanRect,
	//				m_distanceSpectrumRect,
	//				m_mouseRegion,
	//				m_dBmDistScaleMin,
	//				m_dBmDistScaleMax,
	//				m_showZerodBmLine);

	//m_distancePanadapter->drawGLGrid(m_distanceSpectrumRect, m_dBmScaleDistancePanRect);
}

void QGLDistancePanel::drawPanadapter() {

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
	yScaleColor = 10.0f / dBmRange;
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

			glBegin(GL_TRIANGLE_STRIP);
				glColor3f(0.15f, 0.15f, 0.3f);	glVertex3f(x1, y1, -3.0); // top left corner
				glColor3f(0.15f, 0.15f, 0.3f);	glVertex3f(x2, y1, -3.0); // top right corner
				glColor3f(0.15f, 0.15f, 0.51f);	glVertex3f(x1, y2, -3.0); // bottom left corner
				glColor3f(0.15f, 0.15f, 0.61f);	glVertex3f(x2, y2, -3.0); // bottom right corner
			glEnd();
		}
		else {

			glBegin(GL_TRIANGLE_STRIP);
				glColor3f(0.05f, 0.05f, 0.2f);	glVertex3f(x1, y1, -3.0); // top left corner
				glColor3f(0.05f, 0.05f, 0.2f);	glVertex3f(x2, y1, -3.0); // top right corner
				glColor3f(0.05f, 0.05f, 0.31f);	glVertex3f(x1, y2, -3.0); // bottom left corner
				glColor3f(0.05f, 0.05f, 0.41f);	glVertex3f(x2, y2, -3.0); // bottom right corner
			glEnd();
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
				vertexColorArray[i].x = m_red	* (yScaleColor * m_panadapterBins.at(i));
				vertexColorArray[i].y = m_green * (yScaleColor * m_panadapterBins.at(i));
				vertexColorArray[i].z = m_blue  * (yScaleColor * m_panadapterBins.at(i));
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

void QGLDistancePanel::drawPanVerticalScale() {

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

void QGLDistancePanel::drawPanHorizontalScale() {

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

void QGLDistancePanel::drawPanadapterGrid() {

	if (!m_panRect.isValid()) return;

	int width = m_panRect.width();
	int height = m_panRect.height();

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	//glColor4f(0.35, 0.46, 0.51, 0.7);
	//glColor4f(0.45, 0.56, 0.61, 1.0);
	glColor4f(0.45, 0.56, 0.61, 0.8);
	
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

void QGLDistancePanel::drawPanFilter() {

	qreal freqLo = m_filterLowerFrequency / m_sampleRate;
	qreal freqHi = m_filterUpperFrequency / m_sampleRate;

	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);
	//glColor4f(0.65, 0.65, 0.65, 0.25);
	QColor color = QColor(150, 150, 150, 100);

	GLint x1 = m_panRect.left() + qRound((qreal)(m_panRect.width()/2.0f) + freqLo * m_panRect.width() / m_freqScaleZoomFactor);
	GLint x2 = m_panRect.left() + qRound((qreal)(m_panRect.width()/2.0f) + freqHi * m_panRect.width() / m_freqScaleZoomFactor);
	GLint y1 = m_panRect.top() + 1;
	GLint y2 = m_panRect.top() + m_panRect.height() - 1;

	QRect filterRect = QRect(x1, y1, x2 - x1, y2 - y1);

	if ((x1 >= m_panRect.left() && x1 <= m_panRect.right()) ||
		(x2 >= m_panRect.left() && x2 <= m_panRect.right()) ||
		(x1 < m_panRect.left() && x2 > m_panRect.right()))
	{
		if (filterRect.height() > 5) 
			drawGLRect(filterRect, color, 3.0);
	}

	// draw a line for the display center
	y1 = m_panRect.top() + 3;
	y2 = m_panRect.top() + (m_panRect.height() - 3);

	if (y2 > y1 + 3) {

		GLint x = m_panRect.width()/2;
			
		color = m_settings->getPanadapterColors().panCenterLineColor;

		//glDisable(GL_LINE_SMOOTH);
		glDisable(GL_MULTISAMPLE);
		glLineWidth(1);
		glColor4ub(color.red(), color.green(), color.blue(), color.alpha());
		glBegin(GL_LINES);
			/*glVertex2i(x, y1);
			glVertex2i(x, y2);*/
			glVertex3f(x, y1, 4.0f);
			glVertex3f(x, y2, 4.0f);
		glEnd();
		glEnable(GL_MULTISAMPLE);
	}
}

void QGLDistancePanel::drawCrossHair() {

	QRect rect(0, m_panRect.top(), width(), height() - m_panRect.top());

	int x = m_mousePos.x();
	int y = m_mousePos.y();

	glDisable(GL_MULTISAMPLE);
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	glDisable(GL_LINE_SMOOTH);
	glLineWidth(1.0f);

	qglColor(QColor(255, 255, 255, 80));

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
	qglColor(QColor(255, 255, 255, 180));
	glBegin(GL_LINES);
		glVertex3f(x     , y - 20, 5.0f);
		glVertex3f(x     , y + 20, 5.0f);
		glVertex3f(x - 20, y, 5.0f);
		glVertex3f(x + 20, y, 5.0f);
	glEnd();

	// text only on panadapter
	//if (m_mouseRegion == panadapterRegion) {
		
	QString str;
	qglColor(QColor(255, 255, 255, 255));

	int dx = m_panRect.width()/2 - x;
	qreal unit = (qreal)((m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width());
	qreal df = unit * dx;
	qreal frequency = m_frequency - df;
	
	str = frequencyString(-df, true);
	if (x > m_panRect.width() - 85)
		m_oglTextSmall->renderText(x -90, y - 30, 5.0f, str);
	else
		m_oglTextSmall->renderText(x + 4, y - 30, 5.0f, str);

	str = frequencyString(frequency);
	if (x > m_panRect.width() - 85)
		m_oglTextSmall->renderText(x - 90, y - 18, 5.0f, str);
	else
		m_oglTextSmall->renderText(x + 4, y - 18, 5.0f, str);

	if (m_mouseRegion == panadapterRegion) {

		qreal dBm = m_dBmPanMax - ((m_dBmPanMax - m_dBmPanMin) * ((qreal)(y - m_panRect.top()) / m_panRect.height()));
		str = QString::number(dBm, 'f', 1) + " dBm";
		if (x > m_panRect.width() - 85)
			m_oglTextSmall->renderText(x - 90, y + 6, 5.0f, str);
		else
			m_oglTextSmall->renderText(x + 4, y + 6, 5.0f, str);
	}

	// disable scissor box
	glDisable(GL_SCISSOR_TEST);

	glEnable(GL_MULTISAMPLE);
}

//************
void QGLDistancePanel::drawDistanceSpectrum() {

	float yScale;
	float yScaleColor;
	float yTop;
	float localMax;

	GLint displayWidth = (GLint)m_distanceSpectrumRect.width();
	
	double distScale = 0;
	//freqScale = (float)m_sampleRate / rect.width();
	//distScale = 1024.0f / rect.width();
	
	distScale = (double)(1.0f * qRound(m_chirpBufferLength * m_distScaleZoomFactor) / m_distanceSpectrumRect.width());
	if (distScale < 1.0) distScale = 1.0;
	
	if (distScale < 0)	{

		drawGLRect(m_distanceSpectrumRect, Qt::black);
		GRAPHICS_DEBUG << "drawDistanceSpectrum bad distance scale:" << distScale;
		return;
	}
	
	//qreal dBmRange = qAbs(dBmMax - dBmMin);
	qreal dBmRange = (m_dBmDistMax - m_dBmDistMin) * m_distScaleZoomFactor;

	yScale = m_distanceSpectrumRect.height() / dBmRange;
	yScaleColor = 2.0f / dBmRange;
	yTop = m_distanceSpectrumRect.top() + m_distanceSpectrumRect.height();
	
	int idx = 0;
	int lIdx = 0;
	int rIdx = 0;

	// set a scissor box
	glScissor(m_distanceSpectrumRect.left(), size().height() - m_distanceSpectrumRect.bottom() - 1, m_distanceSpectrumRect.left() + m_distanceSpectrumRect.width(), m_distanceSpectrumRect.height());
	glEnable(GL_SCISSOR_TEST);

	GLint vertexArrayLength = 0;
	
	/*if (m_vertexArray) {

		delete [] m_vertexArray;
		m_vertexArray = 0;
	}

	if (m_vertexColorArray) {

		delete [] m_vertexColorArray;
		m_vertexColorArray = 0;
	}*/

	glLineWidth(1);
	glShadeModel(GL_SMOOTH);
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	vertexArrayLength = displayWidth;
	TGL2float *vertexArray = new TGL2float[vertexArrayLength];
	TGL3float *vertexColorArray = new TGL3float[vertexArrayLength];

	//TGL3float *vertexArrayBg = new TGL3float[2*vertexArrayLength];
	//TGL3float *vertexColorArrayBg = new TGL3float[2*vertexArrayLength];

	//m_vertexArray = new TGL2float[vertexArrayLength];
	//m_vertexColorArray = new TGL3float[vertexArrayLength];

	m_redD = (GLfloat)(m_settings->getPanadapterColors().distanceLineColor.red() / 256.0);
	m_greenD = (GLfloat)(m_settings->getPanadapterColors().distanceLineColor.green() / 256.0);
	m_blueD = (GLfloat)(m_settings->getPanadapterColors().distanceLineColor.blue() / 256.0);

	/*m_rf = (GLfloat)(m_settings->getPanadapterColors().panLineFilledColor.red() / 256.0);
	m_gf = (GLfloat)(m_settings->getPanadapterColors().panLineFilledColor.green() / 256.0);
	m_bf = (GLfloat)(m_settings->getPanadapterColors().panLineFilledColor.blue() / 256.0);*/

	for (int i = 0; i < vertexArrayLength; i++) {
	
		lIdx = (int)floor((qreal)(i * distScale));
		rIdx = (int)floor((qreal)(i * distScale) + distScale);

		// max value; later we try mean value also!
		localMax = -10000.0F;
		for (int j = lIdx; j < rIdx; j++) {
			if (m_distanceSpectrumBuffer[j] > localMax) {

				localMax = m_distanceSpectrumBuffer[j];
				idx = j;
			}
		}

		/*m_spectrumVertexColorArrayBg[2*i].x = 0.10;
		m_spectrumVertexColorArrayBg[2*i].y = 0.20;
		m_spectrumVertexColorArrayBg[2*i].z = 0.25;*/

		vertexColorArray[i].x = m_redD;// * (yScaleColor * (m_distanceSpectrumBuffer[idx] - dBmMin));
		vertexColorArray[i].y = m_greenD;// * (yScaleColor * (m_distanceSpectrumBuffer[idx] - dBmMin));
		vertexColorArray[i].z = m_blueD;// * (yScaleColor * (m_distanceSpectrumBuffer[idx] - dBmMin));
		
		vertexArray[i].x = i;
		vertexArray[i].y = yTop - yScale * (m_distanceSpectrumBuffer[idx] - m_dBmDistMin);
	}
	
	glEnableClientState(GL_VERTEX_ARRAY);
	glEnableClientState(GL_COLOR_ARRAY);
				
	glVertexPointer(2, GL_FLOAT, 0, vertexArray);
	glColorPointer(3, GL_FLOAT, 0, vertexColorArray);
	glDrawArrays(GL_LINE_STRIP, 0, vertexArrayLength);

	glDisableClientState(GL_VERTEX_ARRAY);
	glDisableClientState(GL_COLOR_ARRAY);

	delete[] vertexArray;
	delete[] vertexColorArray;
	
	// disable scissor box
	glDisable(GL_SCISSOR_TEST);
}

void QGLDistancePanel::drawDistHorizontalScale() {

	if (m_freqScaleDistancePanRect.isEmpty()) return;

	QFontMetrics d_fm(m_smallFont);

	int rulerFontHeight;
	int rulerFontMaxWidth;
	double maxSpanDist;

	if (m_showChirpFFT) {

		rulerFontHeight = d_fm.tightBoundingRect(".0Hz").height();
		rulerFontMaxWidth = d_fm.boundingRect("000000").width();

		//maxSpanDist = m_sampleRate / 2;
		maxSpanDist = m_sampleRate / 4;
		//maxSpanDist = m_sampleRate;
	}
	else {

		rulerFontHeight = d_fm.tightBoundingRect(".0kmM").height();
		rulerFontMaxWidth = d_fm.boundingRect("0000000").width();

		maxSpanDist = m_chirpBufferLength * m_kilometersPerGate;
	}

	qreal kilometerSpan = maxSpanDist * m_distScaleZoomFactor;
	
	qreal lowerDist = 0;
	qreal upperDist = 0;

	if (m_showChirpFFT)
		lowerDist = -maxSpanDist/2;
	else {

		lowerDist = m_dBmDistDelta;
		if (lowerDist < 0.0) lowerDist = 0.0;
	}
	
	upperDist = lowerDist + kilometerSpan;
	if (upperDist > maxSpanDist) {
		
		upperDist = maxSpanDist;
		lowerDist = maxSpanDist - kilometerSpan;
	}

	qreal unit = (float)(m_freqScaleDistancePanRect.width() / kilometerSpan);
	
	m_distanceScale = getXRuler(m_freqScaleDistancePanRect, rulerFontMaxWidth, unit, lowerDist, upperDist);

	// draw the scale background
	drawGLScaleBackground(m_freqScaleDistancePanRect, Qt::black);

	// draw the distance scale
	int		offset_X		= -1;
	int		textOffset_y	= 17;
	double	distScale		= 1;

	char *str;
	if (m_showChirpFFT)
		str = "  Hz ";
	else
		str = "  km ";
	
	QRect scaledTextRect(0, textOffset_y, 1, rulerFontHeight);
	scaledTextRect.setWidth(d_fm.width(str));
	scaledTextRect.moveLeft(m_freqScaleDistancePanRect.width() - scaledTextRect.width());// - menu_pull_right_rect.width());
	
	glColor3f(0.94, 0.22, 0.43);
	glRasterPos3f(m_freqScaleDistancePanRect.width() - 30, m_freqScaleDistancePanRect.top() + textOffset_y, 0.0);
	//writeBitmapString(GLUT_BITMAP_HELVETICA_10, str);

	if (m_mouseRegion == freqScaleDistancePanRegion)
		glColor3f(0.8, 0.92, 0.97);
	else
		glColor3f(0.65, 0.76, 0.81);

	int len = m_distanceScale.mainPointPositions.length();
	if (len > 0) {

		glLineWidth(3);
		glBegin(GL_LINES);
		for (int i = 0; i < len; i++) {

			glVertex3f(m_distanceScale.mainPointPositions.at(i), m_freqScaleDistancePanRect.top() + 1, 0.0f); // origin of the line
			glVertex3f(m_distanceScale.mainPointPositions.at(i), m_freqScaleDistancePanRect.top() + 4, 0.0f); // ending point of the line
		}
		glEnd();

		for (int i = 0; i < len; i++) {
		
			QString str = QString::number(m_distanceScale.mainPoints.at(i) / distScale, 'f', 0);

			int textWidth = d_fm.width(str);
			QRect textRect(m_distanceScale.mainPointPositions.at(i) + offset_X - (textWidth / 2), textOffset_y, textWidth, rulerFontHeight);

			QByteArray ba = str.toLatin1();
			char *cstr = ba.data();

			if (textRect.left() < 0 || textRect.right() >= scaledTextRect.left()) continue;

			glRasterPos3f(m_distanceScale.mainPointPositions.at(i) + offset_X - (textWidth / 2), m_freqScaleDistancePanRect.top() + textOffset_y, 0.0);
			m_oglTextSmall->renderText(textRect.x(), textRect.y(), cstr);
			//writeBitmapString(GLUT_BITMAP_HELVETICA_10, cstr);
		}
	}

	len = m_distanceScale.subPointPositions.length();
	if (len > 0) {

		glLineWidth(1);
		glBegin(GL_LINES);
		for (int i = 0; i < len; i++) {

			glVertex3f(m_distanceScale.subPointPositions.at(i), m_freqScaleDistancePanRect.top() + 1, 0.0f); // origin of the line
			glVertex3f(m_distanceScale.subPointPositions.at(i), m_freqScaleDistancePanRect.top() + 3, 0.0f); // ending point of the line
		}
		glEnd();
	}
}

void QGLDistancePanel::drawDistVerticalScale() {
		/*const QRect &rect,
		const QRect &distRect,
		int region,
		qreal dBmMin,
		qreal dBmMax,
		bool showZerodBmLine*/

	if (m_dBmScaleDistancePanRect.isEmpty()) return;

	QFontMetrics d_fm(m_smallFont);
	int spacing = 5;
	int fontHeight = d_fm.tightBoundingRect(".0dBm").height() + spacing;
	int fontMaxWidth = d_fm.boundingRect("-000.0").width();

	qreal dBmRange = qAbs(m_dBmDistScaleMax - m_dBmDistScaleMin);
	//if (dBmRange <= 0) return;

	qreal unit = (float)(m_dBmScaleDistancePanRect.height() / dBmRange);

	m_dBmScale = getYRuler(m_dBmScaleDistancePanRect, fontHeight, unit, m_dBmDistScaleMin, m_dBmDistScaleMax);
	
	// draw the scale background
	drawGLScaleBackground(m_dBmScaleDistancePanRect, QColor(60, 60, 60, 80));

	QRect textRect(0, 0, fontMaxWidth, fontHeight);
	textRect.moveRight(14);
	int yOld = -textRect.height();
	
	if (m_mouseRegion == dBmScaleDistancePanRegion)
		glColor3f(0.8, 0.92, 0.97);
	else
		glColor3f(0.65, 0.76, 0.81);


	glLineWidth(1);
	int len = m_dBmScale.mainPointPositions.length();
	
	if (len > 0) {

		glBegin(GL_LINES);
		for (int i = 0; i < len; i++) {

			glVertex3f(m_dBmScaleDistancePanRect.left(),     m_dBmScale.mainPointPositions.at(i), 0.0f);	// origin of the line
			glVertex3f(m_dBmScaleDistancePanRect.left() + 4, m_dBmScale.mainPointPositions.at(i), 0.0f);	// ending point of the line
		}
		glEnd();

		for (int i = 0; i < len; i++) {

			textRect.moveTop(m_dBmScale.mainPointPositions.at(i) + textRect.height()/3);

			QString str;
			if (textRect.y() >= yOld && 
				textRect.bottom() <= (m_dBmScaleDistancePanRect.top() + m_dBmScaleDistancePanRect.height() - textRect.height()) &&
				m_dBmScale.mainPointPositions.at(i) > 10 + m_dBmScaleDistancePanRect.top())
			{
				str = QString::number(m_dBmScale.mainPoints.at(i), 'f', 1);
				QByteArray ba = str.toLatin1();
				char *cstr = ba.data();
				glRasterPos3f(textRect.right() + m_dBmScaleDistancePanRect.left(), textRect.y(), 0.0);
				//writeBitmapString(GLUT_BITMAP_TIMES_ROMAN_10, cstr);
				yOld = textRect.bottom();
			}
		
			if (qRound(m_dBmScale.mainPoints.at(i)) == 0 && m_showZerodBmLine) {
		
				int zerodBmLine = m_dBmScale.mainPointPositions.at(i);
				if (zerodBmLine > m_dBmScaleDistancePanRect.top() && zerodBmLine < m_dBmScaleDistancePanRect.bottom()) {
		
					glColor3f(0.2, 0.87, 0.87);
					glBegin(GL_LINES);
					glVertex3f(m_distanceSpectrumRect.left(), zerodBmLine, 0.0f);						// origin of the line
					glVertex3f(m_distanceSpectrumRect.width() - m_dBmScaleDistancePanRect.width() + 4, zerodBmLine, 0.0f);	// ending point of the line
					glEnd();

					if (m_mouseRegion == dBmScaleDistancePanRegion)
						glColor3f(0.8, 0.92, 0.97);
					else
						glColor3f(0.65, 0.76, 0.81);
				}
			}
		}
	}

	if (m_mouseRegion == dBmScaleDistancePanRegion)
		glColor3f(0.5, 0.62, 0.67);
	else
		glColor3f(0.35, 0.46, 0.51);

	if (m_dBmScale.subPointPositions.length() > 0) {

		glBegin(GL_LINES);
		for (int i = 0; i < m_dBmScale.subPointPositions.length(); i++) {

			glVertex3f(m_dBmScaleDistancePanRect.left(),     m_dBmScale.subPointPositions.at(i), 0.0f);	// origin of the line
			glVertex3f(m_dBmScaleDistancePanRect.left() + 2, m_dBmScale.subPointPositions.at(i), 0.0f);	// ending point of the line
		}
		glEnd();
	}

	char* s = "dBm";
	textRect.moveTop(m_dBmScaleDistancePanRect.top() + m_dBmScaleDistancePanRect.height() - textRect.height());
	glColor3f(0.94, 0.22, 0.43);
	//glRasterPos3f(textRect.left(), textRect.center().y(), 0.0);
	//glRasterPos3f(textRect.right() + right, textRect.center().y(), 0.0);
	glRasterPos3f(textRect.right() + m_dBmScaleDistancePanRect.left(), textRect.center().y(), 0.0);
	//writeBitmapString(GLUT_BITMAP_HELVETICA_10, s);
	
	//glPushMatrix();
	//	glTranslatef(0.0, 0.0, 0.0);
	//	//glRotatef(45.0, 0.0, 0.0, 1.0);
	//	glScalef(0.25, 0.25, 0.25);
	//	writeStrokeString(GLUT_STROKE_ROMAN, s);
	//glPopMatrix();
}

//**********************************************************************************************
// The algorithms of the scale functions renderPanVerticalScale() and renderPanHorizontalScale() 
// are taken from SDRMAXIII (c) Catherine Moss, with permission.

void QGLDistancePanel::renderPanVerticalScale() {

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

void QGLDistancePanel::renderPanHorizontalScale() {

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

void QGLDistancePanel::renderPanadapterGrid() {

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

void QGLDistancePanel::getRegion(QPoint p) {

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

	else if (m_panRect.contains(p)) {

		m_mouseRegion = panadapterRegion;

		if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}
	}
	
	else if (m_freqScaleDistancePanRect.contains(p)) {		
		
		m_mouseRegion = freqScaleDistancePanRegion;
		m_distRulerUpdate = true;

		if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}
	}

	else if (m_dBmScaleDistancePanRect.contains(p)) {

		m_mouseRegion = dBmScaleDistancePanRegion;

		if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}
	}
	else if (m_distanceSpectrumRect.contains(p)) {

		m_mouseRegion = distancePanRegion;

		if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}
	}
	else
		m_mouseRegion = elsewhere;

	/*if (m_displayTime.elapsed() >= 50) {

		m_displayTime.restart();
		updateGL();
		update();
	}*/
	//GRAPHICS_DEBUG << "region" << m_mouseRegion;
}

void QGLDistancePanel::resizeGL(int iWidth, int iHeight) {

	int width = (int)(iWidth/2) * 2;
	int height = iHeight;

	if (width != m_oldWidth) {

		m_freqScalePanadapterRenew = true;
		m_panGridRenew = true;

		m_oldWidth = width;
	}

	m_spectrumVertexColorUpdate = true;
	//m_displayData.size = QSize(width, height);

	glFinish();

	m_resizeTime.restart();
	setupDisplayRegions(QSize(width, height));
    glViewport(0, 0, (GLsizei)width, (GLsizei)height);

	setProjectionOrthographic(width, height);
}

void QGLDistancePanel::setupDisplayRegions(QSize size) {

	m_displayTop = 0;
	int freqScaleRectHeight = 20;
	int dBmScaleWidth = 45;
	int distScaleRectHeight = 20;

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
	
	m_freqScaleDistancePanRect = QRect(
									0,
									size.height() - m_displayTop - distScaleRectHeight + 1, 
									size.width(),
									distScaleRectHeight);

	m_distanceSpectrumRect = QRect(
								m_freqScalePanRect.left(),
								m_freqScalePanRect.bottom(),
								m_freqScalePanRect.width(),
								size.height() - m_displayTop - m_freqScalePanRect.bottom() - distScaleRectHeight + 1);

	m_distancePanRect = QRect(
							m_freqScalePanRect.left(),
							m_freqScalePanRect.bottom(),
							m_freqScalePanRect.width(),
							size.height() - m_displayTop - m_freqScalePanRect.bottom() - distScaleRectHeight + 1);
					
	m_dBmScaleDistancePanRect = QRect(
									m_distanceSpectrumRect.right() - dBmScaleWidth, 
									m_freqScalePanRect.bottom(), 
									dBmScaleWidth, 
									size.height() - m_displayTop - m_freqScalePanRect.bottom() - distScaleRectHeight + 1);

	m_dBmScalePanRect = QRect(
						m_panRect.left(), 
						m_displayTop, 
						45, 
						m_panRect.height());

	m_freqScalePanadapterUpdate = true;
	m_dBmScalePanadapterUpdate = true;
	m_panGridUpdate = true;
	
	//***************************************************************************
	/*GRAPHICS_DEBUG << "***************************************************************************";
	GRAPHICS_DEBUG << "total size" << size.height();
	GRAPHICS_DEBUG << "sizes (top, bottom, height):";
	GRAPHICS_DEBUG << "panRect: " << m_panRect.top() << " " << m_panRect.bottom() << " " << m_panRect.height();
	GRAPHICS_DEBUG << "freqScalePanRect: " << m_freqScalePanRect.top() << " " << m_freqScalePanRect.bottom() << " " << m_freqScalePanRect.height();
	GRAPHICS_DEBUG << "dBmScalePanRect: " << m_dBmScalePanRect.top() << " " << m_dBmScalePanRect.bottom() << " " << m_dBmScalePanRect.height();
	GRAPHICS_DEBUG << "m_distanceSpectrumRect: " << m_distanceSpectrumRect.top() << " " << m_distanceSpectrumRect.bottom() << " " << m_distanceSpectrumRect.height();
	GRAPHICS_DEBUG << "m_freqScaleDistancePanRect: " << m_freqScaleDistancePanRect.top() << " " << m_freqScaleDistancePanRect.bottom() << " " << m_freqScaleDistancePanRect.height();
	GRAPHICS_DEBUG << "m_dBmScaleDistancePanRect: " << m_dBmScaleDistancePanRect.top() << " " << m_dBmScaleDistancePanRect.bottom() << " " << m_dBmScaleDistancePanRect.height();
	GRAPHICS_DEBUG << "";*/
}

void QGLDistancePanel::saveGLState() {

    glPushAttrib(GL_ALL_ATTRIB_BITS);
    glMatrixMode(GL_PROJECTION);
    glPushMatrix();
    glMatrixMode(GL_MODELVIEW);
    glPushMatrix();
}

void QGLDistancePanel::restoreGLState() {

    glMatrixMode(GL_PROJECTION);
    glPopMatrix();
    glMatrixMode(GL_MODELVIEW);
    glPopMatrix();
    glPopAttrib();
}
 
//********************************************************************
// HMI control
 
void QGLDistancePanel::enterEvent(QEvent *event) {

	setFocus(Qt::MouseFocusReason);

	m_mousePos = QPoint(-1, -1);
	m_mouseRegion = elsewhere;
	update();

	QGLWidget::enterEvent(event);
}

void QGLDistancePanel::leaveEvent(QEvent *event) {

	m_mousePos = QPoint(-1, -1);
	m_mouseRegion = elsewhere;
	update();

	QGLWidget::leaveEvent(event);
}

void QGLDistancePanel::wheelEvent(QWheelEvent* event) {
	
	//GRAPHICS_DEBUG << "wheelEvent";
	QPoint pos = event->pos();

	if (event->buttons() == Qt::NoButton) getRegion(pos);

	double freqStep = m_settings->getMouseWheelFreqStep();

	switch (m_mouseRegion) {

		case panadapterRegion:

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

			m_settings->setFrequency(this, true, 0, m_frequency);

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

void QGLDistancePanel::mousePressEvent(QMouseEvent* event) {
	
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
				
			m_settings->setFrequency(this, true, 0, m_frequency);
			update();
		}
	}
	else if (m_mouseRegion == freqScalePanadapterRegion) {

		m_rulerMouseDownPos = m_freqScalePanRect.topLeft();
		
		if (event->buttons() == Qt::RightButton) setCursor(Qt::SplitHCursor);
		update();

		return;
	}
	else if (m_mouseRegion == freqScaleDistancePanRegion) {

		m_rulerMouseDownPos = m_freqScaleDistancePanRect.topLeft();
		
		if (event->buttons() == Qt::RightButton) setCursor(Qt::SplitHCursor);
		update();

		return;
	}
	else if (m_mouseRegion == dBmScalePanadapterRegion) {

		m_rulerMouseDownPos = m_dBmScaleDistancePanRect.topLeft();

		if (event->buttons() == Qt::RightButton) setCursor(Qt::SplitVCursor);
		update();

		return;
	}
	else if (m_mouseRegion == dBmScaleDistancePanRegion) {

		m_rulerMouseDownPos = m_dBmScaleDistancePanRect.topLeft();

		if (event->buttons() == Qt::RightButton) setCursor(Qt::SplitVCursor);
		update();

		return;
	}
	
 	update();
}

void QGLDistancePanel::mouseReleaseEvent(QMouseEvent *event) {

	//GRAPHICS_DEBUG << "mouseReleaseEvent";
	m_mousePos = event->pos();
	m_mouseDownPos = m_mousePos;

	getRegion(m_mousePos);

	if (m_mouseRegion == freqScalePanadapterRegion) {

		return;
	}
	else if (m_mouseRegion == freqScaleDistancePanRegion) {

		return;
	}
	else if (m_mouseRegion == dBmScaleDistancePanRegion) {

		if (event->button() == Qt::LeftButton && m_showZerodBmLine) {

			m_showZerodBmLine = false;
		}
		else if (event->button() == Qt::RightButton) {
		}
		update();
		return;
	}
 	update();
}

void QGLDistancePanel::mouseMoveEvent(QMouseEvent* event) {
	
	//GRAPHICS_DEBUG << "mouseMoveEvent";
	QPoint pos = event->pos();
	m_mousePos = event->pos();

	if (event->buttons() == Qt::NoButton) getRegion(pos);

	switch (m_mouseRegion) {

		case distancePanRegion:
			//GRAPHICS_DEBUG << "distancePanRegion";
			setCursor(Qt::ArrowCursor);
			//update();
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

				m_settings->setFrequency(this, true, 0, m_frequency);
				m_mouseDownPos = pos;
			}
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

					m_settings->setdBmPanScaleMin(0, m_dBmPanMin);
					m_settings->setdBmPanScaleMax(0, m_dBmPanMax);
				}
				
				m_mouseDownPos = pos;
				m_dBmScalePanadapterUpdate = true;
				m_panGridUpdate = true;
			}
			else
			if (event->buttons() == Qt::RightButton) {

				QPoint dPos = m_mouseDownPos - pos;
				if (dPos.y() > 0)
					m_dBmPanDelta = 1.0;
				else if (dPos.y() < 0)
					m_dBmPanDelta = -1.0f;
				
				m_dBmPanMin += m_dBmPanDelta;
				m_dBmPanMax -= m_dBmPanDelta;

				if (qAbs(m_dBmPanMax - m_dBmPanMin) < 10) {

					m_dBmPanMin -= m_dBmPanDelta;
					m_dBmPanMax += m_dBmPanDelta;
				}
				if (m_dBmPanMin < MINDBM) m_dBmPanMin = MINDBM;
				if (m_dBmPanMax > MAXDBM) m_dBmPanMax = MAXDBM;

				m_settings->setdBmPanScaleMin(0, m_dBmPanMin);
				m_settings->setdBmPanScaleMax(0, m_dBmPanMax);

				m_mouseDownPos = pos;
				m_dBmScalePanadapterUpdate = true;
				m_panGridUpdate = true;
			}
			else
				setCursor(Qt::ArrowCursor);

			//update();
			break;

		case dBmScaleDistancePanRegion:
			//GRAPHICS_DEBUG << "dBmScaleDistancePanRegion";
			if (event->buttons() == Qt::LeftButton &&
				event->modifiers() == Qt::ControlModifier) {

				m_showZerodBmLine = true;
				QPoint dPos = m_mouseDownPos - pos;

				qreal unit = (qreal)(qAbs(m_dBmDistScaleMax - m_dBmDistScaleMin) / m_distanceSpectrumRect.height());
				
				qreal newMin = m_dBmDistScaleMin - unit * dPos.y();
				qreal newMax = m_dBmDistScaleMax - unit * dPos.y();

				if (newMin > MINDISTDBM && newMax < MAXDISTDBM) {

					m_dBmDistScaleMin = newMin;
					m_dBmDistScaleMax = newMax;
				}
				m_mouseDownPos = pos;
			}
			else
			if (event->buttons() == Qt::LeftButton) {

				QPoint dPos = m_mouseDownPos - pos;

				qreal unit = (qreal)(qAbs(m_dBmDistMax - m_dBmDistMin) / m_distanceSpectrumRect.height());
				
				qreal newMin = m_dBmDistMin - unit * dPos.y();
				qreal newMax = m_dBmDistMax - unit * dPos.y();

				if (newMin > MINDISTDBM && newMax < MAXDISTDBM) {

					m_dBmDistMin = newMin;
					m_dBmDistMax = newMax;
				}
				m_mouseDownPos = pos;

				m_dBmDistScaleMin = m_dBmDistMin;
				m_dBmDistScaleMax = m_dBmDistMax;
			}
			else
			if (event->buttons() == Qt::RightButton) {

				QPoint dPos = m_mouseDownPos - pos;
				if (dPos.y() > 0)
					m_dBmDistDelta = 1.0;
				else if (dPos.y() < 0)
					m_dBmDistDelta = -1.0f;
				
				m_dBmDistMin += m_dBmDistDelta;
				m_dBmDistMax -= m_dBmDistDelta;

				if (qAbs(m_dBmDistMax - m_dBmDistMin) < 10) {

					m_dBmDistMin -= m_dBmDistDelta;
					m_dBmDistMax += m_dBmDistDelta;
				}
				if (m_dBmDistMin < MINDISTDBM) m_dBmDistMin = MINDISTDBM;
				if (m_dBmDistMax > MAXDISTDBM)  m_dBmDistMax =  MAXDISTDBM;

				m_dBmDistScaleMin = m_dBmDistMin;
				m_dBmDistScaleMax = m_dBmDistMax;
				
				m_mouseDownPos = pos;
			}
			else
				setCursor(Qt::ArrowCursor);
			//update();
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
				m_settings->setFreqRulerPosition(m_freqRulerPosition, 0);
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

			//update();
			break;

		case freqScaleDistancePanRegion:
			//GRAPHICS_DEBUG << "freqScaleDistancePanRegion";
			if (event->buttons() == Qt::LeftButton) {

				if (m_distScaleZoomFactor < 1.0) {
					QPoint dPos = m_mouseDownPos - pos;
					if (dPos.x() > 0)
						m_distRulerDisplayDelta += m_distRulerDisplayDeltaStep;
					else if (dPos.x() < 0)
						m_distRulerDisplayDelta -= m_distRulerDisplayDeltaStep;

					if (m_distRulerDisplayDelta < 0)
						m_distRulerDisplayDelta = 0.0;

					if (m_distRulerDisplayDelta > m_chirpBufferLength * m_kilometersPerGate)
						m_distRulerDisplayDelta -= m_distRulerDisplayDeltaStep;

					m_mouseDownPos = pos;
				}
				else
					m_distRulerDisplayDelta = 0.0;
			}
			else if (event->buttons() == Qt::RightButton) {
				
				QPoint dPos = m_mouseDownPos - pos;
				if (dPos.x() > 0)
					m_distScaleZoomFactor += 0.005;
				else if (dPos.x() < 0)
					m_distScaleZoomFactor -= 0.005;

				if (m_distScaleZoomFactor > 1.0) m_distScaleZoomFactor = 1.0;
				if (m_distScaleZoomFactor < 0.1) m_distScaleZoomFactor = 0.1;

				m_mouseDownPos = pos;
			}
			else
				setCursor(Qt::ArrowCursor);

			//update();
			break;

		case filterRegionLow:

			setCursor(Qt::SizeHorCursor);
			if (event->buttons() == Qt::LeftButton) {

				QPoint dPos = m_mouseDownPos - pos;
				qreal dFreq = (qreal)(dPos.x() * m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width();

				m_filterLowerFrequency = qRound(m_mouseDownFilterFrequencyLo - dFreq);
				m_settings->setRXFilter(this, 0, m_filterLowerFrequency, m_filterUpperFrequency);
			}
			break;

		case filterRegionHigh:

			setCursor(Qt::SizeHorCursor);
			if (event->buttons() == Qt::LeftButton) {

				QPoint dPos = m_mouseDownPos - pos;
				qreal dFreq = (qreal)(dPos.x() * m_sampleRate * m_freqScaleZoomFactor) / m_panRect.width();

				m_filterUpperFrequency = qRound(m_mouseDownFilterFrequencyHi - dFreq);
				m_settings->setRXFilter(this, 0, m_filterLowerFrequency, m_filterUpperFrequency);
			}
			break;

		case elsewhere:
			//GRAPHICS_DEBUG << "elsewhere";
			
			//update();
			break;
	}

	if (m_displayTime.elapsed() >= 50) {

		m_displayTime.restart();
		update();
	}
	//setupDisplayRegions(size());
	//update();

	//locker.unlock();
}

void QGLDistancePanel::keyPressEvent(QKeyEvent* event) {
	
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

void QGLDistancePanel::timerEvent(QTimerEvent *) {
	
	update();
}
 
//********************************************************************
 
void QGLDistancePanel::setFrequency(QObject *sender, bool value, long freq) {

	Q_UNUSED(sender)
	Q_UNUSED(value)
	
	m_frequency = freq;
	//m_displayData.frequency = freq;
	m_freqScalePanadapterUpdate = true;
	m_panGridUpdate = true;

	//updateGL();
	update();
}

void QGLDistancePanel::setFilterFrequencies(QObject *sender, int rx, qreal lo, qreal hi) {

	Q_UNUSED(sender)

	if (rx == 0) {
		
		m_filterLowerFrequency = lo;
		m_filterUpperFrequency = hi;
	}

	update();
}

void QGLDistancePanel::freqRulerPositionChanged(float pos, int rx) {

	if (rx == 0) {
		
		m_freqRulerPosition = pos;

		setupDisplayRegions(size());
		update();
	}
}

void QGLDistancePanel::setSpectrumBuffer(const float *buffer) {

	if (m_spectrumAveraging) {
	
		QVector<float>	m_specBuf(SAMPLE_BUFFER_SIZE);

		//spectrumBufferMutex.lock();

		memcpy(
			(float *) m_specBuf.data(),
			(float *) &buffer[0],
			SAMPLE_BUFFER_SIZE * sizeof(float));

		specAv_queue.enqueue(m_specBuf);
		if (specAv_queue.size() <= m_specAveragingCnt) {
	
			for (int i = 0; i < SAMPLE_BUFFER_SIZE; i++)
				m_tmpBuf[i] += specAv_queue.last().data()[i];

			//spectrumBufferMutex.unlock();
			return;
		}
	
		for (int i = 0; i < SAMPLE_BUFFER_SIZE; i++) {

				m_tmpBuf[i] -= specAv_queue.first().at(i);
				m_tmpBuf[i] += specAv_queue.last().at(i);
				m_avgBuf[i] = m_tmpBuf[i] * m_scale;
		}

		computeDisplayBins(m_avgBuf);
		specAv_queue.dequeue();
	
		//spectrumBufferMutex.unlock();
	}
	else
		computeDisplayBins(buffer);
}

void QGLDistancePanel::computeDisplayBins(const float *panBuffer) {

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

		newSampleSize = (int)floor(4 * BUFFER_SIZE * m_freqScaleZoomFactor);
		deltaSampleSize = 4 * BUFFER_SIZE - newSampleSize;
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
				
		m_panadapterBins << panBuffer[idx] - m_dBmPanMin - m_dBmPanLogGain;
	}
	update();	
}

void QGLDistancePanel::setDistanceSpectrumBuffer(int sampleRate, qint64 length, const float *buffer) {
}

void QGLDistancePanel::distanceSpectrumBufferChanged(int sampleRate, qint64 length, const float *buffer) {

	Q_UNUSED(sampleRate)

	distanceSpectrumBufferMutex.lock();
		m_kilometersPerGate = 1.0f * 3E5 / sampleRate;
		//m_kilometersPerGate = 0.5f * 3E5 / 12000;
		m_chirpBufferLength = length;
		memcpy(m_distanceSpectrumBuffer, buffer, m_chirpBufferLength * sizeof(float));
	distanceSpectrumBufferMutex.unlock();

	update();
}

// get waterfall colors - taken from PowerSDR/KISS Konsole
//QColor QGLDistancePanel::getWaterfallColorAtPixel(qreal value) {
//
//	QColor color;
//	//int r = 0; int g = 0; int b = 0;
//	int r, g, b;
//	int lowerThreshold = (int)m_dBmPanMin - m_waterfallOffsetLo;
//	int upperThreshold = (int)m_dBmPanMax + m_waterfallOffsetHi;
//
//	float offset;
//	float globalRange;
//	float localRange;
//	float percent;
//	
//	switch (m_waterColorScheme) {
//
//		case QSDRGraphics::simple:
//
//			if (value <= lowerThreshold)
//				color = m_waterfallLoColor;
//			else 
//			if (value >= upperThreshold)
//					color = QColor(255, 255, 255);//m_waterfallHiColor;
//			else {
//
//				percent = (value - lowerThreshold) / (upperThreshold - lowerThreshold);
//				if (percent <= 0.5)	{ // use a gradient between low and mid colors
//				
//					percent *= 2;
//
//					r = (int)((1 - percent) * m_waterfallLoColor.red()   + percent * m_waterfallMidColor.red());
//					g = (int)((1 - percent) * m_waterfallLoColor.green() + percent * m_waterfallMidColor.green());
//					b = (int)((1 - percent) * m_waterfallLoColor.blue()  + percent * m_waterfallMidColor.blue());
//				}
//				else {	// use a gradient between mid and high colors
//
//					percent = (float)(percent - 0.5) * 2;
//
//					r = (int)((1 - percent) * m_waterfallMidColor.red()   + percent * 255);//m_waterfallHiColor.red());
//					g = (int)((1 - percent) * m_waterfallMidColor.green() + percent * 255);//m_waterfallHiColor.green());
//					b = (int)((1 - percent) * m_waterfallMidColor.blue()  + percent * 255);//m_waterfallHiColor.blue());
//				}
//
//				if (r > 255) r = 255;
//				if (g > 255) g = 255;
//				if (b > 255) b = 255;
//				color = QColor(r, g, b, m_waterfallAlpha);
//			}
//
//			break;
//
//		case QSDRGraphics::enhanced:
//
//			if (value <= lowerThreshold)
//				color = m_waterfallLoColor;
//			else 
//			if (value >= upperThreshold)
//					color = m_waterfallHiColor;
//			else {
//
//				offset = value - lowerThreshold;
//				globalRange = offset / m_waterfallColorRange; // value from 0.0 to 1.0 where 1.0 is high and 0.0 is low.
//
//				if (globalRange < (float)2/9) { // background to blue
//
//					localRange = globalRange / ((float)2/9);
//					r = (int)((1.0 - localRange) * m_waterfallLoColor.red());
//					g = (int)((1.0 - localRange) * m_waterfallLoColor.green());
//					b = (int)(m_waterfallLoColor.blue() + localRange * (255 - m_waterfallLoColor.blue()));
//				}
//				else 
//				if (globalRange < (float)3/9) { // blue to blue-green
//
//					localRange = (globalRange - (float)2/9) / ((float)1/9);
//					r = 0;
//					g = (int)(localRange * 255);
//					b = 255;
//				}
//				else 
//				if (globalRange < (float)4/9) { // blue-green to green
//
//					localRange = (globalRange - (float)3/9) / ((float)1/9);
//					r = 0;
//					g = 255;
//					b = (int)((1.0 - localRange) * 255);
//				}
//				else 
//				if (globalRange < (float)5/9) { // green to red-green
//
//					localRange = (globalRange - (float)4/9) / ((float)1/9);
//					r = (int)(localRange * 255);
//					g = 255;
//					b = 0;
//				}
//				else 
//				if (globalRange < (float)7/9) { // red-green to red
//
//					localRange = (globalRange - (float)5/9) / ((float)2/9);
//					r = 255;
//					g = (int)((1.0 - localRange) * 255);
//					b = 0;
//				}
//				else 
//				if (globalRange < (float)8/9) { // red to red-blue
//
//					localRange = (globalRange - (float)7/9) / ((float)1/9);
//					r = 255;
//					g = 0;
//					b = (int)(localRange * 255);
//				}
//				else { // red-blue to purple end
//
//					localRange = (globalRange - (float)8/9) / ((float)1/9);
//					r = (int)((0.75 + 0.25 * (1.0 - localRange)) * 255);
//					g = (int)(localRange * 255 * 0.5);
//					b = 255;
//				}
//				if (r > 255) r = 255;
//				if (g > 255) g = 255;
//				if (b > 255) b = 255;
//				if (r < 0) r = 0;
//				if (g < 0) g = 0;
//				if (b < 0) b = 0;
//				color = QColor(r, g, b, m_waterfallAlpha);
//			}
//
//			break;
//
//		case QSDRGraphics::spectran:
//
//			break;
//	}
//	
//	return color;
//}

void QGLDistancePanel::setChirpFFTShow(bool value) {

	distanceSpectrumBufferMutex.lock();
		m_showChirpFFT = value;
	distanceSpectrumBufferMutex.unlock();

	update();
}
 
void QGLDistancePanel::systemStateChanged(
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

	if (m_dataEngineState != state)
		m_dataEngineState = state;

	if (m_serverMode == mode) 
		return;
	else {

		//memset(m_wbSpectrumBuffer, -10000, 4 * BUFFER_SIZE * sizeof(float));
		memset(m_spectrumBuffer, -10000, 4 * BUFFER_SIZE * sizeof(float));
		memset(m_distanceSpectrumBuffer, -10000, 16 * BUFFER_SIZE * sizeof(float));
		m_serverMode = mode;
	}

	resizeGL(width(), height());
	m_displayTime.restart();
	
	update();
}

 
void QGLDistancePanel::graphicModeChanged(
	QObject *sender,
	QSDRGraphics::_Panadapter panMode,
	QSDRGraphics::_WaterfallColorScheme colorScheme)
{
	Q_UNUSED (sender)

	//bool change = false;

	if (m_panadapterMode != panMode) {
		
		m_panadapterMode = panMode;
		//change = true;
	}

	/*if (m_waterColorScheme != colorScheme) {

		m_waterColorScheme = colorScheme;
		change = true;
	}*/

	//if (!change) return;

	update();
}

 void QGLDistancePanel::setSpectrumAveraging(bool value) {

	 spectrumBufferMutex.lock();

	 if (m_spectrumAveraging == value) 
		 return;
	 else
		 m_spectrumAveraging = value;

	 spectrumBufferMutex.unlock();
 }

void QGLDistancePanel::setSpectrumAveragingCnt(int value) {

	spectrumBufferMutex.lock();

		memset(m_tmpBuf, 0, SAMPLE_BUFFER_SIZE * sizeof(float));

		while (!specAv_queue.isEmpty())
			specAv_queue.dequeue();

		m_specAveragingCnt = value;

		if (m_specAveragingCnt > 0)
			m_scale = 1.0f / m_specAveragingCnt;
		else
			m_scale = 1.0f;

	spectrumBufferMutex.unlock();
}

void QGLDistancePanel::setPanGridStatus(bool value) {

	spectrumBufferMutex.lock();

	 if (m_panGrid == value) 
		 return;
	 else
		 m_panGrid = value;

	 spectrumBufferMutex.unlock();

	 update();
}

void QGLDistancePanel::sampleRateChanged(QObject *sender, int value) {

	Q_UNUSED(sender)
	
	m_sampleRate = value;
	update();
}

void QGLDistancePanel::setPanadapterColors() {

	m_spectrumColorsChanged = true;

	mutex.lock();
	/*m_bkgRed   = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.red() / 256.0);
	m_bkgGreen = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.green() / 256.0);
	m_bkgBlue  = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.blue() / 256.0);*/

	/*if (m_bkgRed * 1.2 > 1.0) m_bkgRed = 1.0f;
	if (m_bkgGreen * 1.2 > 1.0) m_bkgGreen = 1.0f;
	if (m_bkgBlue * 1.2 > 1.0) m_bkgBlue = 1.0f;

	if (m_bkgRed * 1.6 > 1.0) m_bkgRed = 1.0f;
	if (m_bkgGreen * 1.6 > 1.0) m_bkgGreen = 1.0f;
	if (m_bkgBlue * 1.6 > 1.0) m_bkgBlue = 1.0f;*/

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

	mutex.unlock();

	update();
}

//void QGLDistancePanel::setWaterfallTime(int rx, int value) {
//
//	if (rx == 0)
//		m_waterfallTime = value;
//}

//void QGLDistancePanel::setWaterfallOffesetLo(int rx, int value) {
//
//	if (rx == 0)
//		m_waterfallOffsetLo = value;
//	
//	update();
//}
//
//void QGLDistancePanel::setWaterfallOffesetHi(int rx, int value) {
//
//	if (rx = 0)
//		m_waterfallOffsetHi = value;
//	
//	update();
//}

void QGLDistancePanel::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void QGLDistancePanel::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}
