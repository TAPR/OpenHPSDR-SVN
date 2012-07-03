/**
* @file  cusdr_oglWidebandPanel.h
* @brief wide band spectrum panel class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-02-11
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

#define LOG_WBGRAPHICS

#include "cusdr_oglWidebandPanel.h"

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

QGLWidebandPanel::QGLWidebandPanel(QWidget *parent)
		: QGLWidget(QGLFormat(QGL::SampleBuffers|QGL::AlphaChannel), parent)
		, m_settings(Settings::instance())
		, m_serverMode(m_settings->getCurrentServerMode())
		, m_hwInterface(m_settings->getHWInterface())
		, m_dataEngineState(m_settings->getDataEngineState())
		, m_currentReceiver(m_settings->getCurrentReceiver())
		, m_sampleRate(m_settings->getSampleRate())
		, m_panadapterMode(m_settings->getPanadapterMode())
		//, m_specAveragingCnt(m_settings->getSpectrumAveragingCnt())
		, m_mousePos(QPoint(-1, -1))
		, m_mouseDownPos(QPoint(-1, -1))
		, m_snapMouse(3)
		, m_panSpectrumBinsLength(0)
		, m_freqScaleZoomFactor(1.0)
		, m_displayDelta(0.0)
		, m_freqScaleUpdate(true)
		, m_freqScaleRenew(true)
		, m_dBmScaleUpdate(true)
		, m_dBmScaleRenew(true)
		, m_panGridRenew(true)
		, m_spectrumUpdate(false)
		, m_spectrumColorsChanged(true)
		, m_crossHairCursor(false)
		, m_panGrid(m_settings->getPanGridStatus())
{
	QGL::setPreferredPaintEngine(QPaintEngine::OpenGL);
	
	setAutoBufferSwap(true);
	setAutoFillBackground(false);
	
	setMouseTracking(true);
	//setFocusPolicy(Qt::StrongFocus);

	setupDisplayRegions(size());
	m_oldWidth = size().width();
	m_oldHeight = size().height();
	
	m_rxDataList = m_settings->getReceiverDataList();

	m_rxDataList = m_settings->getReceiverDataList();
	m_frequency = m_rxDataList.at(0).frequency;

	m_dBmPanMin = m_settings->getdBmWBScaleMin();
	m_dBmPanMax = m_settings->getdBmWBScaleMax();

	
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

	m_panTimer.start();
	m_displayTime.start();
	m_resizeTime.start();

	memset(m_wbSpectrumBuffer, -10000, 4 * BUFFER_SIZE * sizeof(float));
	
	m_dBmPanLogGain = 75;//69; // allow user to calibrate this value
	
	if (m_specAveragingCnt > 0)
		m_scale = 1.0f / m_specAveragingCnt;
	else
		m_scale = 1.0f;

	memset(m_tmpBuf, 0, SAMPLE_BUFFER_SIZE * sizeof(float));

	m_frequencyScaleFBO = 0;
	m_dBmScaleFBO = 0;
	m_gridFBO = 0;

	m_r = (GLfloat)(m_settings->getPanadapterColors().wideBandLineColor.red() / 256.0);
	m_g = (GLfloat)(m_settings->getPanadapterColors().wideBandLineColor.green() / 256.0);
	m_b = (GLfloat)(m_settings->getPanadapterColors().wideBandLineColor.blue() / 256.0);

	m_rf = (GLfloat)(m_settings->getPanadapterColors().wideBandFilledColor.red() / 256.0);
	m_gf = (GLfloat)(m_settings->getPanadapterColors().wideBandFilledColor.green() / 256.0);
	m_bf = (GLfloat)(m_settings->getPanadapterColors().wideBandFilledColor.blue() / 256.0);

	m_redST	  = (GLfloat)(m_settings->getPanadapterColors().panSolidTopColor.red() / 256.0);
	m_greenST = (GLfloat)(m_settings->getPanadapterColors().panSolidTopColor.green() / 256.0);
	m_blueST  = (GLfloat)(m_settings->getPanadapterColors().panSolidTopColor.blue() / 256.0);

	m_redSB   = (GLfloat)(m_settings->getPanadapterColors().panSolidBottomColor.red() / 256.0);
	m_greenSB = (GLfloat)(m_settings->getPanadapterColors().panSolidBottomColor.green() / 256.0);
	m_blueSB  = (GLfloat)(m_settings->getPanadapterColors().panSolidBottomColor.blue() / 256.0);

	m_bkgRed   = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.red() / 256.0);
	m_bkgGreen = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.green() / 256.0);
	m_bkgBlue  = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.blue() / 256.0);
}

QGLWidebandPanel::~QGLWidebandPanel() {

	disconnect(m_settings, 0, this, 0);
	
	makeCurrent();
	glFinish();

	while (!specAv_queue.isEmpty())
		specAv_queue.dequeue();

	if (m_frequencyScaleFBO) {

		delete m_frequencyScaleFBO;
		m_frequencyScaleFBO = 0;
	}

	if (m_dBmScaleFBO) {

		delete m_dBmScaleFBO;
		m_dBmScaleFBO = 0;
	}

	if (m_gridFBO) {

		delete m_gridFBO;
		m_gridFBO = 0;
	}
}

QSize QGLWidebandPanel::sizeHint() const {
	
	//return QSize(width(), height());
	return QSize(width(), 5);
}

void QGLWidebandPanel::setupConnections() {

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
		SIGNAL(widebandSpectrumBufferChanged(const float*)),
		this,
		SLOT(setWidebandSpectrumBuffer(const float*)));

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
		SIGNAL(panadapterColorChanged()), 
		this, 
		SLOT(setPanadapterColors()));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(panGridStatusChanged(bool)),
		this,
		SLOT(setPanGridStatus(bool)));

	//CHECKED_CONNECT(
	//	m_settings, 
	//	SIGNAL(spectrumAveragingCntChanged(int)), 
	//	this, 
	//	SLOT(setSpectrumAveragingCnt(int)));
}

void QGLWidebandPanel::initializeGL() {

	if (!isValid()) return;

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

void QGLWidebandPanel::paintGL() {

	switch (m_serverMode) {

		case QSDR::ExternalDSP:
			drawGLRect(QRect(0, 0, width(), height()), QColor(0, 0, 0));
			break;

		//case QSDR::InternalDSP:
		case QSDR::DttSP:
		case QSDR::QtDSP:
			
			drawSpectrum();
			drawHorizontalScale();
			drawVerticalScale();

			if (m_panGrid)
				drawGrid();

			// Ham band information
			if (m_dataEngineState == QSDR::DataEngineUp) {

				drawHamBand(1810000, 2000000, "160m");
				drawHamBand(3500000, 3800000, "80m");
				drawHamBand(5258500, 5403500, "60m");
				drawHamBand(7000000, 7300000, "40m");
				drawHamBand(10100000, 10150000, "30m");
				drawHamBand(14000000, 14350000, "20m");
				drawHamBand(18068000, 18168000, "17m");
				drawHamBand(21000000, 21450000, "15m");
				drawHamBand(24890000, 24990000, "12m");
				drawHamBand(28000000, 29700000, "10m");
				drawHamBand(50000000, 51990000, "6m");

				//qglColor(QColor(255, 255, 255, 130));
				//m_oglTextSmall->renderText(m_panRect.right() - 100, m_panRect.top(), 5.0f, "Region 1");
			}

			if (m_mouseRegion == panRegion && m_crossHairCursor)
				drawCrossHair();

			break;
	}
}
 
//************************************************************************
void QGLWidebandPanel::drawSpectrum() {

	GLint width  = m_panRect.width();
	GLint height = m_panRect.height();

	GLint x1 = m_panRect.left();
	GLint y1 = m_panRect.top();
	GLint x2 = x1 + width;
	GLint y2 = y1 + height;

	GLint vertexArrayLength = 0;

	// x scale
	int newBufferSize = 0;
	int idx = 0;
	int lIdx = 0;
	int rIdx = 0;
	int deltaIdx = 0;

	qreal scaleMult = 1.0;
	
	newBufferSize	= qFloor(2.0 * BUFFER_SIZE * m_freqScaleZoomFactor);
	deltaIdx		= qFloor(m_displayDelta * (2.0 * BUFFER_SIZE / MAXFREQUENCY));
	
	qreal frequencyScale = (qreal)(1.0f * newBufferSize / width);
	
	if (frequencyScale < 0.125)
		scaleMult = 0.0625;
	else if (frequencyScale < 0.25)
		scaleMult = 0.125;
	else if (frequencyScale < 0.5)
		scaleMult = 0.25;
	else if (frequencyScale < 1.0)
		scaleMult = 0.5;
	
	// y scale
	float yScale;
	float yScaleColor;
	float yTop;
	float localMax;

	qreal dBmRange = qAbs(m_dBmPanMax - m_dBmPanMin);

	yScale = height / dBmRange;
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
				glColor3f(0.2f * m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x1, y1, -4.0); // top left corner
				glColor3f(0.2f * m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x2, y1, -4.0); // top right corner
				glColor3f(0.8f * m_bkgRed, 0.8f * m_bkgGreen, 0.8f * m_bkgBlue); glVertex3f(x1, y2, -4.0); // bottom left corner
				glColor3f(       m_bkgRed,        m_bkgGreen,        m_bkgBlue); glVertex3f(x2, y2, -4.0); // bottom right corner
			glEnd();
		}
		else {

			glBegin(GL_TRIANGLE_STRIP);
				glColor3f(0.3f * m_bkgRed, 0.3f * m_bkgGreen, 0.3f * m_bkgBlue); glVertex3f(x1, y1, -4.0); // top left corner
				glColor3f(0.3f * m_bkgRed, 0.3f * m_bkgGreen, 0.3f * m_bkgBlue); glVertex3f(x2, y1, -4.0); // top right corner
				glColor3f(0.5f * m_bkgRed, 0.5f * m_bkgGreen, 0.5f * m_bkgBlue); glVertex3f(x1, y2, -4.0); // bottom left corner
				glColor3f(0.7f * m_bkgRed, 0.7f * m_bkgGreen, 0.7f * m_bkgBlue); glVertex3f(x2, y2, -4.0); // bottom right corner
			glEnd();
		}
	}
	else {

		drawGLRect(m_panRect, QColor(30, 30, 50, 155), -4.0f);
	}

	// set a scissor box
	glScissor(x1, size().height() - y2, x2, height);
	glEnable(GL_SCISSOR_TEST);

	// set up the vertex arrays
	vertexArrayLength = (GLint)(scaleMult * width);

	TGL3float *vertexArray = new TGL3float[vertexArrayLength];
	TGL3float *vertexColorArray = new TGL3float[vertexArrayLength];

	TGL3float *vertexArrayBg = new TGL3float[2*vertexArrayLength];
	TGL3float *vertexColorArrayBg = new TGL3float[2*vertexArrayLength];

	switch (m_panadapterMode) {

		case QSDRGraphics::FilledLine:
			
			for (int i = 0; i < vertexArrayLength; i++) {

				lIdx = (int)floor((qreal)(i * frequencyScale / scaleMult));
				rIdx = (int)floor((qreal)(i * frequencyScale / scaleMult) + frequencyScale / scaleMult);

				// max value; later we try mean value also!
				localMax = -10000.0F;
				for (int j = lIdx; j < rIdx; j++) {
					if (m_wbSpectrumBuffer[j] > localMax) {

						localMax = m_wbSpectrumBuffer[j];
						idx = j;
					}
				}
				idx += deltaIdx;

				vertexColorArrayBg[2*i].x = m_rf;
				vertexColorArrayBg[2*i].y = m_gf;
				vertexColorArrayBg[2*i].z = m_bf;

				vertexColorArrayBg[2*i+1].x = 0.3 * m_rf;
				vertexColorArrayBg[2*i+1].y = 0.3 * m_gf;
				vertexColorArrayBg[2*i+1].z = 0.3 * m_bf;

				vertexColorArray[i].x = m_r * (yScaleColor * (m_wbSpectrumBuffer[idx] - m_dBmPanMin));
				vertexColorArray[i].y = m_g * (yScaleColor * (m_wbSpectrumBuffer[idx] - m_dBmPanMin));
				vertexColorArray[i].z = m_b * (yScaleColor * (m_wbSpectrumBuffer[idx] - m_dBmPanMin));

				vertexArrayBg[2*i].x = (GLfloat)(i/scaleMult);
				vertexArrayBg[2*i].y = (GLfloat)(yTop - yScale * (m_wbSpectrumBuffer[idx] - m_dBmPanMin - m_dBmPanLogGain));
				vertexArrayBg[2*i].z = -2.5;

				vertexArrayBg[2*i+1].x = (GLfloat)(i/scaleMult);
				vertexArrayBg[2*i+1].y = (GLfloat)yTop;
				vertexArrayBg[2*i+1].z = -2.5;

				vertexArray[i].x = (GLfloat)(i/scaleMult);
				vertexArray[i].y = (GLfloat)(yTop - yScale * (m_wbSpectrumBuffer[idx] - m_dBmPanMin - m_dBmPanLogGain));
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

				lIdx = (int)floor((qreal)(i * frequencyScale / scaleMult));
				rIdx = (int)floor((qreal)(i * frequencyScale / scaleMult) + frequencyScale / scaleMult);

				// max value; later we try mean value also!
				localMax = -10000.0F;
				for (int j = lIdx; j < rIdx; j++) {
					if (m_wbSpectrumBuffer[j] > localMax) {

						localMax = m_wbSpectrumBuffer[j];
						idx = j;
					}
				}
				idx += deltaIdx;
	
				mutex.lock();
				vertexColorArray[i].x = m_r	* (yScaleColor * (m_wbSpectrumBuffer[idx] - m_dBmPanMin));
				vertexColorArray[i].y = m_g * (yScaleColor * (m_wbSpectrumBuffer[idx] - m_dBmPanMin));
				vertexColorArray[i].z = m_b * (yScaleColor * (m_wbSpectrumBuffer[idx] - m_dBmPanMin));
				mutex.unlock();
				
				vertexArray[i].x = (GLfloat)(i/scaleMult);
				vertexArray[i].y = (GLfloat)(yTop - yScale * (m_wbSpectrumBuffer[idx] - m_dBmPanMin - m_dBmPanLogGain));
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
			
				lIdx = (int)floor((qreal)(i * frequencyScale / scaleMult));
				rIdx = (int)floor((qreal)(i * frequencyScale / scaleMult) + frequencyScale / scaleMult);

				// max value; later we try mean value also!
				localMax = -10000.0F;
				for (int j = lIdx; j < rIdx; j++) {
					if (m_wbSpectrumBuffer[j] > localMax) {

						localMax = m_wbSpectrumBuffer[j];
						idx = j;
					}
				}
				idx += deltaIdx;

				mutex.lock();
				vertexColorArrayBg[2*i].x = m_redST;
				vertexColorArrayBg[2*i].y = m_greenST;
				vertexColorArrayBg[2*i].z = m_blueST;
								
				vertexColorArrayBg[2*i+1].x = m_redSB;
				vertexColorArrayBg[2*i+1].y = m_greenSB;
				vertexColorArrayBg[2*i+1].z = m_blueSB;
				mutex.unlock();

				vertexArrayBg[2*i].x = (GLfloat)(i/scaleMult);
				vertexArrayBg[2*i].y = (GLfloat)(yTop - yScale * (m_wbSpectrumBuffer[idx] - m_dBmPanMin - m_dBmPanLogGain));
				vertexArrayBg[2*i].z = -2.0f;

				vertexArrayBg[2*i+1].x = (GLfloat)(i/scaleMult);
				vertexArrayBg[2*i+1].y = (GLfloat)yTop;
				vertexArrayBg[2*i+1].z = -2.0f;
			}
			
			glEnableClientState(GL_VERTEX_ARRAY);
			glEnableClientState(GL_COLOR_ARRAY);
				
			glVertexPointer(3, GL_FLOAT, 0, vertexArrayBg);
			glColorPointer(3, GL_FLOAT, 0, vertexColorArrayBg);
			glDrawArrays(GL_LINES, 0, 2*vertexArrayLength);

			glDisableClientState(GL_VERTEX_ARRAY);
			glDisableClientState(GL_COLOR_ARRAY);

			delete[] vertexArray;
			delete[] vertexColorArray;
			delete[] vertexArrayBg;
			delete[] vertexColorArrayBg;

			break;
	}

	// draw a rectangle for the panadapter frequency region
	if (m_dataEngineState == QSDR::DataEngineUp) {

		y1 = m_panRect.top() + 15;
		y2 = m_panRect.height() - 15;
		
		int centerFreq = (int)(m_frequencyUnit * (m_frequency - m_displayDelta));
		int deltaF = (int)(m_frequencyUnit * (float)m_sampleRate / 2);
	
		x1 = (GLint)(centerFreq - deltaF);
		x2 = (GLint)(centerFreq + deltaF);
		if (x1 == x2) x2 = x1 + 1;
	
		QRect rect = QRect(x1, y1, x2-x1, y2);
		drawGLRect(rect, QColor(160, 235, 255, 120), 0.0f);
	}

	glDisable(GL_SCISSOR_TEST);
	glDisable(GL_BLEND);
	glDisable(GL_LINE_SMOOTH);
}

void QGLWidebandPanel::drawVerticalScale() {

	if (!m_dBmScaleRect.isValid()) return;

	int width = m_dBmScaleRect.width();
	int height = m_dBmScaleRect.height();

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	glColor3f(0.65, 0.76, 0.81);
	
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);

	if (!m_dBmScaleFBO || m_dBmScaleUpdate || m_dBmScaleRenew)	{

		if (!m_dBmScaleFBO || m_dBmScaleRenew) {

			if (m_dBmScaleFBO) {
			
				delete m_dBmScaleFBO;
				m_dBmScaleFBO = 0;
			}

			m_dBmScaleFBO = new QGLFramebufferObject(width, height);//, format);
			//WBGRAPHICS_DEBUG << "dBmScaleFBO generated.";
		}

		glPushAttrib(GL_VIEWPORT_BIT);
		glViewport(0, 0, width, height);
		setProjectionOrthographic(width, height);

		m_dBmScaleFBO->bind();
			renderVerticalScale();
		m_dBmScaleFBO->release();

		glPopAttrib();
		glViewport(0, 0, size().width(), size().height());
		setProjectionOrthographic(size().width(), size().height());
		
		//WBGRAPHICS_DEBUG << "dBm scale updated.";
		m_dBmScaleUpdate = false;
		m_dBmScaleRenew = false;
	}

	renderTexture(m_dBmScaleRect, m_dBmScaleFBO->texture(), 0.0f);

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
	glColor3f(0.65, 0.76, 0.81);
}

void QGLWidebandPanel::drawHorizontalScale() {

	if (!m_freqScaleRect.isValid()) return;

	int width = m_freqScaleRect.width();
	int height = m_freqScaleRect.height();

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	glColor3f(0.65, 0.76, 0.81);
	
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);

	if (!m_frequencyScaleFBO || m_freqScaleUpdate || m_freqScaleRenew) {

		if (!m_frequencyScaleFBO || m_freqScaleRenew) {

			if (m_frequencyScaleFBO) {
			
				delete m_frequencyScaleFBO;
				m_frequencyScaleFBO = 0;
			}

			m_frequencyScaleFBO = new QGLFramebufferObject(width, height);//, format);
			//WBGRAPHICS_DEBUG << "wb-frequencyScaleFBO generated.";
		}

		glPushAttrib(GL_VIEWPORT_BIT);
		glViewport(0, 0, width, height);
		setProjectionOrthographic(width, height);
		
		m_frequencyScaleFBO->bind();
			renderHorizontalScale();
		m_frequencyScaleFBO->release();

		glPopAttrib();
		glViewport(0, 0, size().width(), size().height());
		setProjectionOrthographic(size().width(), size().height());
		
		//WBGRAPHICS_DEBUG << "frequency scale updated.";
		m_freqScaleUpdate = false;
		m_freqScaleRenew = false;
	}
	
	renderTexture(m_freqScaleRect, m_frequencyScaleFBO->texture(), 0.0f);
	
	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
	glColor3f(0.65, 0.76, 0.81);
}

void QGLWidebandPanel::drawGrid() {

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
	
	if (!m_gridFBO || m_panGridUpdate || m_panGridRenew)	{

		if (!m_gridFBO || m_panGridRenew) {

			if (m_gridFBO) {
			
				delete m_gridFBO;
				m_gridFBO = 0;
			}

			m_gridFBO = new QGLFramebufferObject(width, height);//, format);
			//WBGRAPHICS_DEBUG << "gridFBO generated.";
		}

		glPushAttrib(GL_VIEWPORT_BIT);
		glViewport(0, 0, width, height);
		setProjectionOrthographic(width, height);
		
		m_gridFBO->bind();
			renderGrid();
		m_gridFBO->release();

		glPopAttrib();
		glViewport(0, 0, size().width(), size().height());
		setProjectionOrthographic(size().width(), size().height());
		
		//WBGRAPHICS_DEBUG << "grid updated.";
		m_panGridUpdate = false;
		m_panGridRenew = false;
	}

	renderTexture(m_panRect, m_gridFBO->texture(), -3.0f);
	
	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
	glColor3f(0.65, 0.76, 0.81);
	glEnable(GL_MULTISAMPLE);
}

void QGLWidebandPanel::drawCrossHair() {

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
		glVertex3f(rect.left() - 1, y, 4.0f);
		glVertex3f(m_dBmScaleRect.left() - 2, y, 4.0f);
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
	if (m_mouseRegion == panRegion) {
		
		QString str;
		qglColor(QColor(255, 255, 255, 255));

		qreal unit = (qreal)((MAXFREQUENCY * m_freqScaleZoomFactor) / m_panRect.width());
		qreal frequency = unit * x;

		str = frequencyString(frequency);
		if (x > m_panRect.width() - 85)
			m_oglTextSmall->renderText(x - 90, y - 18, 5.0f, str);
		else
			m_oglTextSmall->renderText(x + 4, y - 18, 5.0f, str);

		qreal dBm = m_dBmPanMax - ((m_dBmPanMax - m_dBmPanMin) * ((qreal)(y - m_panRect.top()) / m_panRect.height()));
		str = QString::number(dBm, 'f', 1) + " dBm";
		if (x > m_panRect.width() - 85)
			m_oglTextSmall->renderText(x - 90, y + 6, 5.0f, str);
		else
			m_oglTextSmall->renderText(x + 4, y + 6, 5.0f, str);
	}

	glDisable(GL_SCISSOR_TEST);
	glEnable(GL_MULTISAMPLE);
}

void QGLWidebandPanel::drawHamBand(
		int lo,
		int hi,
		const QString &band
) {
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	GLint x1 = (GLint)(m_frequencyUnit * (lo - m_displayDelta));
	GLint x2 = (GLint)(m_frequencyUnit * (hi - m_displayDelta));
	GLint y1 = m_panRect.top();// + 15;
	GLint y2 = m_panRect.bottom();
	
	QRect rect = QRect(x1, y1, x2-x1, y2);

	//glColor3f(0.6f * m_bkgRed, 0.6f * m_bkgGreen, 0.6f * m_bkgBlue);
	if (x2 < m_dBmScaleRect.left())
		//drawGLRect(rect, QColor(100, 100, 255, 70), QColor(40, 40, 200, 20), -2.0f, false);
		drawGLRect(rect, QColor((int)(255 * m_bkgRed), (int)(255 * m_bkgGreen), (int)(255 * m_bkgBlue), 70), QColor((int)(255 * m_bkgRed), (int)(255 * m_bkgGreen), (int)(255 * m_bkgBlue), 20), -2.0f, false);

	QFontMetrics d_fm(m_smallFont);
	int fontWidth = d_fm.boundingRect(band).width();

	qglColor(QColor(255, 255, 255, 180));
	m_oglTextSmall->renderText((x2 + x1 - fontWidth)/2.0f, y1, 5.0f, band);
}

//************************************************************************
// The algorithms of the scale functions are taken from SDRMAXIII 
// (c) Catherine Moss, with permission.

void QGLWidebandPanel::renderVerticalScale() {

	QString str;
	QFontMetrics d_fm(m_smallFont);
	int spacing = 6;
	int fontHeight = d_fm.tightBoundingRect(".0dBm").height() + spacing;
	int fontMaxWidth = d_fm.boundingRect("-000.0").width();

	GLint width = m_dBmScaleRect.width();
	GLint height = m_dBmScaleRect.height();

	qreal unit = (qreal)(m_dBmScaleRect.height() / qAbs(m_dBmPanMax - m_dBmPanMin));

	m_dBmScale = getYRuler2(m_dBmScaleRect, fontHeight, unit, m_dBmPanMin, m_dBmPanMax);

	glClear(GL_COLOR_BUFFER_BIT);
	
	QRect textRect(0, 0, fontMaxWidth, fontHeight);
	textRect.moveLeft(3);
	int yOld = -textRect.height();

	int len		= m_dBmScale.mainPointPositions.length();
	int sublen	= m_dBmScale.subPointPositions.length();
	
	// draw the scale background
	drawGLScaleBackground(QRect(0, 0, width, height), QColor(30, 30, 30, 180));
	
	if (len > 0) {

		glColor3f(0.65, 0.76, 0.81);
		glLineWidth(1);

		glBegin(GL_LINES);
		for (int i = 0; i < len; i++) {

			glVertex3f(0,     m_dBmScale.mainPointPositions.at(i), 0.0f);	// origin of the line
			glVertex3f(4, m_dBmScale.mainPointPositions.at(i), 0.0f);	// ending point of the line
		}
		glEnd();
		
		glColor3f(0.45, 0.56, 0.61);
		if (sublen > 0) {

			glBegin(GL_LINES);
			for (int i = 1; i < sublen; i++) {

				glVertex3f(0,     m_dBmScale.subPointPositions.at(i), 0.0f);	// origin of the line
				glVertex3f(2, m_dBmScale.subPointPositions.at(i), 0.0f);	// ending point of the line
			}
			glEnd();
		}

		glColor3f(0.75, 0.86, 0.91);
		for (int i = 0; i < len; i++) {

			textRect.moveBottom(m_dBmScale.mainPointPositions.at(i) + textRect.height()/2);
			
			//if (textRect.y() >= yOld && textRect.bottom() <= (m_dBmScaleRect.height() - textRect.height())) {
			if (textRect.y() > m_dBmScaleRect.top() + textRect.height() && textRect.bottom() <= (m_dBmScaleRect.height() - textRect.height()/2)) {
			
				str = QString::number(m_dBmScale.mainPoints.at(i), 'f', 1);
				m_oglTextSmall->renderText(textRect.x() + 10, textRect.y(), str);
				yOld = textRect.bottom();
			}
		}
	}

	textRect.moveTop(m_dBmScaleRect.top());
	glColor3f(0.94, 0.22, 0.43);
	
	str = QString("dBm");
	m_oglTextSmall->renderText(textRect.x() + 18, textRect.y(), str);
}

void QGLWidebandPanel::renderHorizontalScale() {

	if (m_freqScaleRect.isEmpty()) return;

	QFontMetrics d_fm(m_smallFont);
	int fontHeight = d_fm.tightBoundingRect(".0kMGHz").height();
	int fontMaxWidth = d_fm.boundingRect("000.000").width();

	m_frequencySpan = MAXFREQUENCY * m_freqScaleZoomFactor;
	
	double lowerFreq = m_displayDelta;
	if (lowerFreq < 0.0) lowerFreq = 0.0;

	double upperFreq = lowerFreq + m_frequencySpan;
	if (upperFreq > MAXFREQUENCY) {
		
		upperFreq = MAXFREQUENCY;
		lowerFreq = MAXFREQUENCY - m_frequencySpan;
	}
	
	m_frequencyUnit = (qreal)(m_freqScaleRect.width() / m_frequencySpan);
	m_frequencyScale = getXRuler(m_freqScaleRect, fontMaxWidth, m_frequencyUnit, lowerFreq, upperFreq);
	
	// draw the frequency scale
	int		offset_X		= -1;
	int		textOffset_y	= 5;
	double	freqScale		= 1;

	QString fstr = QString(" Hz ");
	if (upperFreq >= 1e6) { freqScale = 1e6; fstr = QString("  MHz "); }
	else
	if (upperFreq >= 1e3) { freqScale = 1e3; fstr = QString("  kHz "); }

	// draw the wide band scale background
	drawGLScaleBackground(QRect(0, 0, m_freqScaleRect.width(), m_freqScaleRect.height()), QColor(0, 0, 0, 255));

	QRect scaledTextRect(0, textOffset_y, 1, fontHeight);
	scaledTextRect.setWidth(d_fm.width(fstr));
	scaledTextRect.moveLeft(m_freqScaleRect.width() - scaledTextRect.width());// - menu_pull_right_rect.width());

	glColor3f((GLfloat)0.65f, (GLfloat)0.76f, (GLfloat)0.81f);
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

			if (freqScale == 1e3)
				while (str.endsWith('0')) str.remove(str.size() - 1, 1);

			if (str.endsWith('.')) str.remove(str.size() - 1, 1);

			int text_width = d_fm.width(str);
			QRect textRect(m_frequencyScale.mainPointPositions.at(i) + offset_X - (text_width / 2), textOffset_y, text_width, fontHeight);

			if (textRect.left() < 0 || textRect.right() >= scaledTextRect.left()) continue;
			
			m_oglTextSmall->renderText(textRect.x(), textRect.y(), str);
		}
	}

	len = m_frequencyScale.subPointPositions.length();
	if (len > 0) {

		glLineWidth(1);
		glBegin(GL_LINES);
		for (int i = 0; i < len; i++) {

			//glVertex3f(m_frequencyScale.subPointPositions.at(i), m_freqScaleRect.top() + 1, 0.0f); // origin of the line
			//glVertex3f(m_frequencyScale.subPointPositions.at(i), m_freqScaleRect.top() + 3, 0.0f); // ending point of the line
			glVertex3f(m_frequencyScale.subPointPositions.at(i), 1.0f, 0.0f);
			glVertex3f(m_frequencyScale.subPointPositions.at(i), 3.0f, 0.0f);
		}
		glEnd();
	}

	glColor3f(0.94, 0.22, 0.43);
	m_oglTextSmall->renderText(m_freqScaleRect.width() - 30, textOffset_y, fstr);
}

void QGLWidebandPanel::renderGrid() {

	glClear(GL_COLOR_BUFFER_BIT);
	glLineStipple(1, 0x9999);
	glEnable(GL_LINE_STIPPLE);
	glLineWidth(1.0f);

	// vertical lines
	int len = m_frequencyScale.mainPointPositions.length();
	if (len > 0) {

		GLint x1 = m_panRect.left();
		GLint x2 = 1;
		if (m_dBmScaleRect.isValid()) x2 += m_dBmScaleRect.width();

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
		point1.x = m_panRect.left();// + m_dBmScaleRect.width();
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

void QGLWidebandPanel::getRegion(QPoint p) {

	if (m_freqScaleRect.contains(p)) {

		m_mouseRegion = freqScaleRegion;
		
		if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}
	}
	else if (m_dBmScaleRect.contains(p)) {

		m_mouseRegion = dBmScaleRegion;

		if (m_displayTime.elapsed() >= 50) {
			
			m_displayTime.restart();
			update();
		}
	}
	else if (m_panRect.contains(p)) {

		m_mouseRegion = panRegion;

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

void QGLWidebandPanel::resizeGL(int iWidth, int iHeight) {

	int width = (int)(iWidth/2) * 2;
	int height = iHeight;

	if (width != m_oldWidth) {

		m_freqScaleRenew = true;
		m_panGridRenew = true;

		m_oldWidth = width;
	}

	if (height != m_oldHeight) {

		m_dBmScaleRenew = true;
		m_panGridRenew = true;

		m_oldHeight = height;
	}

	m_spectrumVertexColorUpdate = true;
	
	glFinish();

	m_resizeTime.restart();
	setupDisplayRegions(QSize(width, height));
    glViewport(0, 0, (GLsizei)width, (GLsizei)height);

	setProjectionOrthographic(width, height);
}

void QGLWidebandPanel::setupDisplayRegions(QSize size) {

	m_displayTop = 0;
	int freqScaleRectHeight = 20;
	int dBmScaleWidth = 45;

	//WBGRAPHICS_DEBUG << "WB display width:" << size.width();
	//WBGRAPHICS_DEBUG << "WB display height:" << size.height()/6.0;
	
	m_freqScaleRect = QRect(
				0, 
				size.height() - m_displayTop - freqScaleRectHeight,
				size.width(), 
				freqScaleRectHeight);

	m_panRect = QRect(
			0,
			m_displayTop, 
			size.width(),
			m_freqScaleRect.top() - m_displayTop);

	m_dBmScaleRect = QRect(
				m_panRect.right() - dBmScaleWidth,
				m_displayTop, 
				dBmScaleWidth, 
				m_panRect.height());

	m_panRectWidth = (GLint)m_panRect.width();
	m_freqScaleUpdate = true;
	m_dBmScaleUpdate = true;
	m_panGridUpdate = true;
	
	//***************************************************************************
	/*WBGRAPHICS_DEBUG << "***************************************************************************";
	WBGRAPHICS_DEBUG << "total size" << size.height();
	WBGRAPHICS_DEBUG << "sizes (top, bottom, height):";
	WBGRAPHICS_DEBUG << "m_wideBandPanRect" << m_panRect.top() << m_panRect.bottom() << m_panRect.height();
	WBGRAPHICS_DEBUG << "m_freqScaleWideBandRect" << m_freqScaleRect.top() << m_freqScaleRect.bottom() << m_freqScaleRect.height();
	WBGRAPHICS_DEBUG << "m_dBmScaleWideBandRect" << m_dBmScaleRect.top() << m_dBmScaleRect.bottom() << m_dBmScaleRect.height();
	WBGRAPHICS_DEBUG << "";*/
}
 
void QGLWidebandPanel::saveGLState() {

    glPushAttrib(GL_ALL_ATTRIB_BITS);
    glMatrixMode(GL_PROJECTION);
    glPushMatrix();
    glMatrixMode(GL_MODELVIEW);
    glPushMatrix();
}

void QGLWidebandPanel::restoreGLState() {

    glMatrixMode(GL_PROJECTION);
    glPopMatrix();
    glMatrixMode(GL_MODELVIEW);
    glPopMatrix();
    glPopAttrib();
}
 
//********************************************************************
// HMI control
 
void QGLWidebandPanel::enterEvent(QEvent *event) {

	setFocus(Qt::MouseFocusReason);

	m_mousePos = QPoint(-1, -1);
	m_mouseRegion = elsewhere;
	update();

	QGLWidget::enterEvent(event);
}

void QGLWidebandPanel::leaveEvent(QEvent *event) {

	m_mousePos = QPoint(-1, -1);
	m_mouseRegion = elsewhere;
	update();

	QGLWidget::leaveEvent(event);
}

void QGLWidebandPanel::wheelEvent(QWheelEvent* event) {
	
	//GRAPHICS_DEBUG << "wheelEvent";
	QPoint pos = event->pos();

	if (event->buttons() == Qt::NoButton) getRegion(pos);

	double freqStep = m_settings->getMouseWheelFreqStep();

	switch (m_mouseRegion) {

		case panRegion:

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

			//m_settings->setFrequency(this, true, m_currentReceiver, m_frequency);
			break;
	}

 	//updateGL();
	update();
}

void QGLWidebandPanel::mousePressEvent(QMouseEvent* event) {
	
	//GRAPHICS_DEBUG << "mousePressEvent";
	m_mousePos = event->pos();
	m_mouseDownPos = m_mousePos;

	getRegion(m_mousePos);

	if (m_mouseRegion == freqScaleRegion) {

		m_rulerMouseDownPos = m_freqScaleRect.topLeft();
		
		if (event->buttons() == Qt::RightButton) setCursor(Qt::SplitHCursor);
		update();

		return;
	}
	else if (m_mouseRegion == dBmScaleRegion) {

		m_rulerMouseDownPos = m_dBmScaleRect.topLeft();

		if (event->buttons() == Qt::RightButton) setCursor(Qt::SplitVCursor);
		update();

		return;
	}
	else if (m_mouseRegion == panRegion) {

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
		else if (event->buttons() == Qt::LeftButton) {
			
			float unit = (float)(m_panRect.width() / (MAXFREQUENCY * m_freqScaleZoomFactor));
			
			m_frequency = (long)(1000 * (int)(qRound(m_mousePos.x()/unit + m_displayDelta)/1000));
			m_settings->setFrequency(this, true, m_currentReceiver, m_frequency);
		}
		update();

		return;
	}
	
 	update();
}

void QGLWidebandPanel::mouseReleaseEvent(QMouseEvent *event) {

	//GRAPHICS_DEBUG << "mouseReleaseEvent";
	m_mousePos = event->pos();
	m_mouseDownPos = m_mousePos;

	getRegion(m_mousePos);

	if (m_mouseRegion == freqScaleRegion) {

		return;
	}
	else if (m_mouseRegion == panRegion) {

		return;
	}
	
 	update();
}

void QGLWidebandPanel::mouseMoveEvent(QMouseEvent* event) {
	
	QPoint pos = event->pos();
	m_mousePos = event->pos();

	if (event->buttons() == Qt::NoButton) getRegion(pos);

	switch (m_mouseRegion) {

		case panRegion:
			
			if (m_crossHairCursor)
				setCursor(Qt::BlankCursor);
			else
				setCursor(Qt::ArrowCursor);

			update();
			break;

		case dBmScaleRegion:
			
			if (event->buttons() == Qt::LeftButton) {

				QPoint dPos = m_mouseDownPos - pos;
				qreal unit = (qreal)(qAbs(m_dBmPanMax - m_dBmPanMin) / m_panRect.height());
				
				qreal newMin = m_dBmPanMin - unit * dPos.y();
				qreal newMax = m_dBmPanMax - unit * dPos.y();

				if (newMin > MINDBM && newMax < MAXDBM) {

					m_dBmPanMin = newMin;
					m_dBmPanMax = newMax;

					m_settings->setdBmWBScaleMin(m_dBmPanMin);
					m_settings->setdBmWBScaleMax(m_dBmPanMax);
				}
				
				m_mouseDownPos = pos;
				m_dBmScaleUpdate = true;
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

				m_settings->setdBmWBScaleMin(m_dBmPanMin);
				m_settings->setdBmWBScaleMax(m_dBmPanMax);

				m_mouseDownPos = pos;
				m_dBmScaleUpdate = true;
				m_panGridUpdate = true;
			}
			else
				setCursor(Qt::ArrowCursor);

			update();
			break;

		case freqScaleRegion:
			
			if (event->buttons() == Qt::LeftButton) {

				if (m_freqScaleZoomFactor < 1.0) {

					QPoint dPos = m_mouseDownPos - pos;
					if (dPos.x() > 0)
						m_displayDelta += 100000.0;
					else if (dPos.x() < 0)
						m_displayDelta -= 100000.0;

					if (m_displayDelta < 0)
						m_displayDelta = 0.0;

					if (m_displayDelta > MAXFREQUENCY * (1 - m_freqScaleZoomFactor))
						m_displayDelta -= 100000.0;

					m_mouseDownPos = pos;
					m_freqScaleUpdate = true;
					m_panGridUpdate = true;
				}
				else
					m_displayDelta = 0.0;
			}
			else if (event->buttons() == Qt::RightButton) {
				
				QPoint dPos = m_mouseDownPos - pos;
				if (dPos.x() > 0)
					m_freqScaleZoomFactor += 0.005f;
				else if (dPos.x() < 0)
					m_freqScaleZoomFactor -= 0.005f;

				if (m_freqScaleZoomFactor > 1.0) m_freqScaleZoomFactor = 1.0f;
				if (m_freqScaleZoomFactor < 0.3) m_freqScaleZoomFactor = 0.3f;

				m_mouseDownPos = pos;
				m_freqScaleUpdate = true;
				m_panGridUpdate = true;
			}
			else
				setCursor(Qt::ArrowCursor);

			update();
			break;

		case elsewhere:
			//GRAPHICS_DEBUG << "elsewhere";
			
			update();
			break;
	}

	//if (m_oldMouseRegion != m_mouseRegion) update();

	//m_oldMouseRegion = m_mouseRegion;
		
	//if (m_displayTime.elapsed() >= 50) {

	//	m_displayTime.restart();
	//	//updateGL();
	//	update();
	//}
	//setupDisplayRegions(size());
	//update();

	//locker.unlock();
}

void QGLWidebandPanel::keyPressEvent(QKeyEvent* event) {
	
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

void QGLWidebandPanel::timerEvent(QTimerEvent *) {
	
	update();
}
 
//********************************************************************
 
void QGLWidebandPanel::setFrequency(QObject *sender, bool value, int rx, long freq) {

	Q_UNUSED(sender)
	Q_UNUSED(value)
	Q_UNUSED(rx)
	
	m_frequency = freq;
	m_freqScaleUpdate = true;
	m_panGridUpdate = true;

	//updateGL();
	update();
}

void QGLWidebandPanel::setCurrentReceiver(int value) {

	m_currentReceiver = value;
	update();
}

void QGLWidebandPanel::setSpectrumBuffer(const float *buffer) {

	if (m_settings->getSpectrumAveraging()) {

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

void QGLWidebandPanel::computeDisplayBins(const float *panBuffer) {

	//int newSampleSize = 0;
	//int deltaSampleSize = 0;
	//int idx = 0;
	//int lIdx = 0;
	//int rIdx = 0;
	//qreal localMax;

	//if (m_serverMode == QSDR::ChirpWSPRFile) {
	//	
	//	newSampleSize = (int)floor(2 * BUFFER_SIZE * m_displayData.freqScaleZoomFactor);
	//	deltaSampleSize = 2 * BUFFER_SIZE - newSampleSize;
	//}
	//else {

	//	newSampleSize = (int)floor(4 * BUFFER_SIZE * m_displayData.freqScaleZoomFactor);
	//	deltaSampleSize = 4 * BUFFER_SIZE - newSampleSize;
	//}

	//if (deltaSampleSize%2 != 0) {
	//	deltaSampleSize += 1;
	//	newSampleSize -= 1;
	//}

	//m_panScale = (qreal)(1.0 * newSampleSize / m_panRectWidth);
	//m_scaleMultOld = m_displayData.scaleMult;
	//	
	//if (m_panScale < 0.125) {
	//	m_displayData.scaleMult = 0.0625;
	//}
	//else if (m_panScale < 0.25) {
	//	m_displayData.scaleMult = 0.125;
	//}
	//else if (m_panScale < 0.5) {
	//	m_displayData.scaleMult = 0.25;
	//}
	//else if (m_panScale < 1.0) {
	//	m_displayData.scaleMult = 0.5;
	//}
	//else {
	//	m_displayData.scaleMult = 1.0;
	//}

	//m_panSpectrumBinsLength = (GLint)(m_displayData.scaleMult * m_panRectWidth);

	//if (m_scaleMultOld != m_displayData.scaleMult) {

	//	m_displayData.waterfallUpdate = true;
	//}

	//m_displayData.waterfallPixel.clear();
	//m_displayData.waterfallPixel.resize(4 * m_panRectWidth);

	//m_displayData.panadapterBins.clear();
	//
	//for (int i = 0; i < m_panSpectrumBinsLength; i++) {
	//		
	//	idx = 0;
	//	lIdx = (int)floor((qreal)(i * m_panScale / m_displayData.scaleMult));
	//	rIdx = (int)floor((qreal)(i * m_panScale / m_displayData.scaleMult) + m_panScale / m_displayData.scaleMult);
	//				
	//	// max value; later we try mean value also!
	//	localMax = -10000.0F;
	//	for (int j = lIdx; j < rIdx; j++) {

	//		if (panBuffer[j] > localMax) {

	//			localMax = panBuffer[j];
	//			idx = j;
	//		}
	//	}
	//	idx += deltaSampleSize/2;
	//			
	//	m_displayData.panadapterBins << panBuffer[idx] - m_displayData.dBmPanMin - m_dBmPanLogGain;

	//update();
	////updateGL();
}

void QGLWidebandPanel::setWidebandSpectrumBuffer(const float *buffer) {

	mutex.lock();
		////memcpy(m_wbSpectrumBuffer, &buffer[4*BUFFER_SIZE], 4*BUFFER_SIZE * sizeof(float));
		memcpy(m_wbSpectrumBuffer, buffer, 2*BUFFER_SIZE * sizeof(float));
	mutex.unlock();

	update();
}

void QGLWidebandPanel::systemStateChanged(
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

		memset(m_wbSpectrumBuffer, -10000, 4 * BUFFER_SIZE * sizeof(float));
		m_serverMode = mode;
	}

	resizeGL(width(), height());
	m_displayTime.restart();
	
	update();
}

 
void QGLWidebandPanel::graphicModeChanged(
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

 
void QGLWidebandPanel::setSpectrumAveragingCnt(int value) {

	mutex.lock();

		memset(m_tmpBuf, 0, SAMPLE_BUFFER_SIZE * sizeof(float));

		while (!specAv_queue.isEmpty())
			specAv_queue.dequeue();

		m_specAveragingCnt = value;

		if (m_specAveragingCnt > 0)
			m_scale = 1.0f / m_specAveragingCnt;
		else
			m_scale = 1.0f;

	mutex.unlock();
}

void QGLWidebandPanel::setPanadapterColors() {

	m_spectrumColorsChanged = true;

	mutex.lock();
	m_bkgRed   = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.red() / 256.0);
	m_bkgGreen = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.green() / 256.0);
	m_bkgBlue  = (GLfloat)(m_settings->getPanadapterColors().panBackgroundColor.blue() / 256.0);

	m_r	= (GLfloat)(m_settings->getPanadapterColors().wideBandLineColor.red() / 256.0);
	m_g = (GLfloat)(m_settings->getPanadapterColors().wideBandLineColor.green() / 256.0);
	m_b	= (GLfloat)(m_settings->getPanadapterColors().wideBandLineColor.blue() / 256.0);

	m_rf = (GLfloat)(m_settings->getPanadapterColors().wideBandFilledColor.red() / 256.0);
	m_gf = (GLfloat)(m_settings->getPanadapterColors().wideBandFilledColor.green() / 256.0);
	m_bf = (GLfloat)(m_settings->getPanadapterColors().wideBandFilledColor.blue() / 256.0);

	m_redST	  = (GLfloat)(m_settings->getPanadapterColors().panSolidTopColor.red() / 256.0);
	m_greenST = (GLfloat)(m_settings->getPanadapterColors().panSolidTopColor.green() / 256.0);
	m_blueST  = (GLfloat)(m_settings->getPanadapterColors().panSolidTopColor.blue() / 256.0);

	m_redSB   = (GLfloat)(m_settings->getPanadapterColors().panSolidBottomColor.red() / 256.0);
	m_greenSB = (GLfloat)(m_settings->getPanadapterColors().panSolidBottomColor.green() / 256.0);
	m_blueSB  = (GLfloat)(m_settings->getPanadapterColors().panSolidBottomColor.blue() / 256.0);
	mutex.unlock();

	update();
}

void QGLWidebandPanel::setPanGridStatus(bool value) {

	mutex.lock();

	 if (m_panGrid == value) 
		 return;
	 else
		 m_panGrid = value;

	 mutex.unlock();
	 update();
}

void QGLWidebandPanel::sampleRateChanged(QObject *sender, int value) {

	Q_UNUSED(sender)
	
	m_sampleRate = value;
	update();
}

void QGLWidebandPanel::freqScaleUpdate(bool value) {

	m_freqScaleUpdate = value;
}

void QGLWidebandPanel::freqScaleRenew(bool value) {

	m_freqScaleRenew = value;
}

void QGLWidebandPanel::dBmScaleUpdate(bool value) {

	m_dBmScaleUpdate = value;
}

void QGLWidebandPanel::dBmScaleRenew(bool value) {

	m_dBmScaleRenew = value;
}

void QGLWidebandPanel::panGridUpdate(bool value) {

	m_panGridUpdate = value;
}

void QGLWidebandPanel::panGridRenew(bool value) {

	m_panGridRenew = value;
}

void QGLWidebandPanel::closeEvent(QCloseEvent *event) {

	emit closeEvent(this);
	QWidget::closeEvent(event);
}

void QGLWidebandPanel::showEvent(QShowEvent *event) {

	emit showEvent(this);
	QWidget::showEvent(event);
}
