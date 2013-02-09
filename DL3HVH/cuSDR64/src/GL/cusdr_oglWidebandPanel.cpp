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

// use: WBGRAPHICS_DEBUG

#include "cusdr_oglWidebandPanel.h"

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

QGLWidebandPanel::QGLWidebandPanel(QWidget *parent)
		: QGLWidget(QGLFormat(QGL::SampleBuffers|QGL::AlphaChannel), parent)
		, set(Settings::instance())
		, m_serverMode(set->getCurrentServerMode())
		, m_hwInterface(set->getHWInterface())
		, m_dataEngineState(set->getDataEngineState())
		, m_panMode(set->getPanadapterMode(0))
		//, m_specAveragingCnt(set->getSpectrumAveragingCnt())
		, m_mousePos(QPoint(-1, -1))
		, m_mouseDownPos(QPoint(-1, -1))
		, m_spectrumUpdate(false)
		, m_freqScaleUpdate(true)
		, m_freqScaleRenew(true)
		, m_dBmScaleUpdate(true)
		, m_dBmScaleRenew(true)
		, m_panGridRenew(true)
		, m_spectrumColorsChanged(true)
		, m_crossHairCursor(false)
		//, m_panGrid(set->getPanGridStatus(0))
		, m_panGrid(true)
		, m_calibrate(false)
		, m_panSpectrumBinsLength(0)
		, m_snapMouse(3)
		, m_currentReceiver(set->getCurrentReceiver())
		, m_sampleRate(set->getSampleRate())
		, m_freqScaleZoomFactor(1.0)
		, m_dBmScaleOffset(0.0)
{
	QGL::setPreferredPaintEngine(QPaintEngine::OpenGL);
	
	setSizePolicy(QSizePolicy::Expanding, QSizePolicy::Expanding);

	setAutoBufferSwap(true);
	setAutoFillBackground(false);
	
	setMouseTracking(true);
	//setFocusPolicy(Qt::StrongFocus);

	setupDisplayRegions(size());
	m_oldWidth = size().width();
	m_oldHeight = size().height();
	
	m_rxDataList = set->getReceiverDataList();
	m_widebandOptions = set->getWidebandOptions();
	m_panMode = m_widebandOptions.panMode;

	m_frequency = m_rxDataList.at(0).vfoFrequency;

	m_lowerFrequency = 0.0;
	m_upperFrequency = (qreal) MAXFREQUENCY;

	m_dBmPanMin = set->getWidebanddBmScaleMin();
	m_dBmPanMax = set->getWidebanddBmScaleMax();

	fonts = new CFonts(this);
	m_fonts = fonts->getFonts();
	
	m_fonts.smallFont.setBold(true);

	m_oglTextTiny = new OGLText(m_fonts.tinyFont);
	m_oglTextSmall = new OGLText(m_fonts.smallFont);
	m_oglTextNormal = new OGLText(m_fonts.normalFont);
	

	timer = 0;

	setupConnections();

	m_panTimer.start();
	m_displayTime.start();
	m_resizeTime.start();

	m_wbSpectrumBuffer.resize(BIGWIDEBANDSIZE / 2);
	m_wbSpectrumBuffer.fill(-1000.0f);
	m_wbSpectrumBufferLength = m_wbSpectrumBuffer.size();

	m_dBmPanLogGain = 75;//69; // allow user to calibrate this value
	
	if (m_specAveragingCnt > 0)
		m_scale = 1.0f / m_specAveragingCnt;
	else
		m_scale = 1.0f;

	m_frequencyScaleFBO = 0;
	m_dBmScaleFBO = 0;
	m_gridFBO = 0;


	m_gridColor = set->getPanadapterColors().gridLineColor;

	m_redGrid   = (GLfloat)(m_gridColor.red()/256.0);
	m_greenGrid = (GLfloat)(m_gridColor.green()/256.0);
	m_blueGrid  = (GLfloat)(m_gridColor.blue()/256.0);

	m_r = (GLfloat)(set->getPanadapterColors().wideBandLineColor.red() / 256.0);
	m_g = (GLfloat)(set->getPanadapterColors().wideBandLineColor.green() / 256.0);
	m_b = (GLfloat)(set->getPanadapterColors().wideBandLineColor.blue() / 256.0);

	m_rf = (GLfloat)(set->getPanadapterColors().wideBandFilledColor.red() / 256.0);
	m_gf = (GLfloat)(set->getPanadapterColors().wideBandFilledColor.green() / 256.0);
	m_bf = (GLfloat)(set->getPanadapterColors().wideBandFilledColor.blue() / 256.0);

	m_redST	  = (GLfloat)(set->getPanadapterColors().panSolidTopColor.red() / 256.0);
	m_greenST = (GLfloat)(set->getPanadapterColors().panSolidTopColor.green() / 256.0);
	m_blueST  = (GLfloat)(set->getPanadapterColors().panSolidTopColor.blue() / 256.0);

	m_redSB   = (GLfloat)(set->getPanadapterColors().panSolidBottomColor.red() / 256.0);
	m_greenSB = (GLfloat)(set->getPanadapterColors().panSolidBottomColor.green() / 256.0);
	m_blueSB  = (GLfloat)(set->getPanadapterColors().panSolidBottomColor.blue() / 256.0);

	m_bkgRed   = (GLfloat)(set->getPanadapterColors().panBackgroundColor.red() / 256.0);
	m_bkgGreen = (GLfloat)(set->getPanadapterColors().panBackgroundColor.green() / 256.0);
	m_bkgBlue  = (GLfloat)(set->getPanadapterColors().panBackgroundColor.blue() / 256.0);
}

QGLWidebandPanel::~QGLWidebandPanel() {

	disconnect(set, 0, this, 0);
	
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

QSize QGLWidebandPanel::minimumSizeHint() const {
	
	return QSize(width(), 50);
	//return QSize(width(), height());
}

QSize QGLWidebandPanel::sizeHint() const {
	
	//return QSize(width(), height());
	return QSize(width(), 120);
}

void QGLWidebandPanel::setupConnections() {

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
					QSDR::_DataEngineState))
	);

	CHECKED_CONNECT(
		set, 
		SIGNAL(graphicModeChanged(
					QObject *,
					int,
					PanGraphicsMode,
					WaterfallColorMode)),
		this, 
		SLOT(graphicModeChanged(
					QObject *,
					int,
					PanGraphicsMode,
					WaterfallColorMode)));

	CHECKED_CONNECT(
		set,
		SIGNAL(widebandSpectrumBufferChanged(const qVectorFloat &)),
		this,
		SLOT(setWidebandSpectrumBuffer(const qVectorFloat &)));

	CHECKED_CONNECT(
		set,
		SIGNAL(widebandSpectrumBufferReset()),
		this,
		SLOT(resetWidebandSpectrumBuffer()));

	CHECKED_CONNECT(
		set,
		SIGNAL(vfoFrequencyChanged(QObject *, int, int, long)),
		this,
		SLOT(setFrequency(QObject *, int, int, long)));

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
		SIGNAL(panadapterColorChanged()), 
		this, 
		SLOT(setPanadapterColors()));

	/*CHECKED_CONNECT(
		set, 
		SIGNAL(panGridStatusChanged(bool, int)),
		this,
		SLOT(setPanGridStatus(bool, int)));*/

	CHECKED_CONNECT(
		set, 
		SIGNAL(mercuryAttenuatorChanged(QObject *, HamBand, int)),
		this, 
		SLOT(setMercuryAttenuator(QObject *, HamBand, int)));

	//CHECKED_CONNECT(
	//	set, 
	//	SIGNAL(spectrumAveragingCntChanged(int)), 
	//	this, 
	//	SLOT(setSpectrumAveragingCnt(int)));
}

void QGLWidebandPanel::initializeGL() {

	if (!isValid()) return;

	glShadeModel(GL_SMOOTH);
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

void QGLWidebandPanel::paintGL() {

	switch (m_serverMode) {

		case QSDR::NoServerMode:
		case QSDR::ChirpWSPR:
		case QSDR::ChirpWSPRFile:
		case QSDR::DemoMode:

			drawGLRect(QRect(0, 0, width(), height()), QColor(0, 0, 0));
			break;

		case QSDR::SDRMode:
			
			if (m_resizeTime.elapsed() > 200 || m_dataEngineState == QSDR::DataEngineDown) {
			
				drawSpectrum();
				drawHorizontalScale();
				drawVerticalScale();

				if (m_panGrid)
					drawGrid();

				// Ham band information
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

				if (m_mouseRegion == panRegion && m_crossHairCursor)
					drawCrossHair();
			}

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
	int idx = 0;
	int lIdx = 0;
	int rIdx = 0;
	int deltaIdx = 0;

	qreal scaleMult = 1.0;

	m_scaledBufferSize = qFloor(m_wbSpectrumBufferLength * m_freqScaleZoomFactor);
	deltaIdx = qFloor((qreal)(m_wbSpectrumBufferLength * (m_lowerFrequency / MAXFREQUENCY)));
	
	qreal frequencyScale = (qreal)(1.0f * m_scaledBufferSize / width);
	
	if (frequencyScale < 0.125)
		scaleMult = 0.0625;
	else if (frequencyScale < 0.25)
		scaleMult = 0.125;
	else if (frequencyScale < 0.5)
		scaleMult = 0.25;
	else if (frequencyScale < 1.0)
		scaleMult = 0.5;
	
	qreal scale = frequencyScale / scaleMult;

	// y scale
	float yScale;
	float yScaleColor;
	float yTop;
	float localMax;

	//qreal dBmRange = qAbs(m_dBmPanMax - m_dBmPanMin);
	qreal dBmRange;

	if (m_calibrate)
		dBmRange = qAbs(m_dBmPanMaxOld - m_dBmPanMinOld);
	else
		dBmRange = qAbs(m_dBmPanMax - m_dBmPanMin);

	yScale = height / dBmRange;
	yScaleColor = 1.0f / dBmRange;
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

//		glBegin(GL_TRIANGLE_STRIP);
//			glColor3f(0.2f * m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x1, y1, -4.0); // top left corner
//			glColor3f(0.2f * m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x2, y1, -4.0); // top right corner
//			glColor3f(0.8f * m_bkgRed, 0.8f * m_bkgGreen, 0.8f * m_bkgBlue); glVertex3f(x1, y2, -4.0); // bottom left corner
//			glColor3f(       m_bkgRed,        m_bkgGreen,        m_bkgBlue); glVertex3f(x2, y2, -4.0); // bottom right corner
//		glEnd();
		glBegin(GL_TRIANGLE_STRIP);
			glColor3f(0.8f * m_bkgRed, 0.8f * m_bkgGreen, 0.8f * m_bkgBlue); glVertex3f(x1, y1, -4.0); // top left corner
			glColor3f(0.6f * m_bkgRed, 0.6f * m_bkgGreen, 0.6f * m_bkgBlue); glVertex3f(x2, y1, -4.0); // top right corner
			glColor3f(0.4f * m_bkgRed, 0.4f * m_bkgGreen, 0.4f * m_bkgBlue); glVertex3f(x1, y2, -4.0); // bottom left corner
			glColor3f(0.2f * m_bkgRed, 0.2f * m_bkgGreen, 0.2f * m_bkgBlue); glVertex3f(x2, y2, -4.0); // bottom right corner
		glEnd();
	}
	else {

		drawGLRect(m_panRect, QColor(30, 30, 50, 155), -4.0f);
	}

	// set a scissor box
	glScissor(x1, size().height() - y2, x2, height);
	glEnable(GL_SCISSOR_TEST);

	// set up the vertex arrays
	vertexArrayLength = (GLint)(scaleMult * width);
	//WBGRAPHICS_DEBUG << "vertexArrayLength: " << vertexArrayLength;

	TGL3float *vertexArray = new TGL3float[vertexArrayLength];
	TGL3float *vertexColorArray = new TGL3float[vertexArrayLength];

	TGL3float *vertexArrayBg = new TGL3float[2*vertexArrayLength];
	TGL3float *vertexColorArrayBg = new TGL3float[2*vertexArrayLength];

	switch (m_panMode) {

		case (PanGraphicsMode) FilledLine:
			
			for (int i = 0; i < vertexArrayLength; i++) {

				idx = 0;
				lIdx = (int)floor((qreal)(i * scale));
				rIdx = (int)floor((qreal)(i * scale) + scale);

				// max value; later we try mean value also!
				localMax = -10000.0F;
				for (int j = lIdx; j < rIdx; j++) {
					if (m_wbSpectrumBuffer.at(j) > localMax) {

						localMax = m_wbSpectrumBuffer.at(j);
						idx = j;
					}
				}
				idx += deltaIdx;

				mutex.lock();

				vertexColorArrayBg[2*i].x = m_rf;
				vertexColorArrayBg[2*i].y = m_gf;
				vertexColorArrayBg[2*i].z = m_bf;

				vertexColorArrayBg[2*i+1].x = 0.3 * m_rf;
				vertexColorArrayBg[2*i+1].y = 0.3 * m_gf;
				vertexColorArrayBg[2*i+1].z = 0.3 * m_bf;

				qreal yvalue = 0;
				if (idx < m_wbSpectrumBufferLength)
					yvalue = m_wbSpectrumBuffer.at(idx) - m_dBmPanMin;

				vertexColorArray[i].x = m_r * (yScaleColor * yvalue);
				vertexColorArray[i].y = m_g * (yScaleColor * yvalue);
				vertexColorArray[i].z = m_b * (yScaleColor * yvalue);
				
				if (idx < m_wbSpectrumBufferLength)
					yvalue = m_wbSpectrumBuffer.at(idx) - m_dBmPanMin - m_dBmPanLogGain;

				if (m_mercuryAttenuator)
					yvalue -= 20.0f;

				vertexArrayBg[2*i].x = (GLfloat)(i/scaleMult);
				vertexArrayBg[2*i].y = (GLfloat)(yTop - yScale * yvalue);
				vertexArrayBg[2*i].z = -2.5;

				vertexArrayBg[2*i+1].x = (GLfloat)(i/scaleMult);
				vertexArrayBg[2*i+1].y = (GLfloat)yTop;
				vertexArrayBg[2*i+1].z = -2.5;

				vertexArray[i].x = (GLfloat)(i/scaleMult);
				vertexArray[i].y = (GLfloat)(yTop - yScale * yvalue);
				vertexArray[i].z = -1.0;

				mutex.unlock();
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

		case (PanGraphicsMode) Line:

			for (int i = 0; i < vertexArrayLength; i++) {

				lIdx = (int)floor((qreal)(i * scale));
				rIdx = (int)floor((qreal)(i * scale) + scale);

				// max value; later we try mean value also!
				localMax = -10000.0F;
				for (int j = lIdx; j < rIdx; j++) {
					if (m_wbSpectrumBuffer.at(j) > localMax) {

						localMax = m_wbSpectrumBuffer.at(j);
						idx = j;
					}
				}
				idx += deltaIdx;

				mutex.lock();

				qreal yvalue = 0;
				if (idx < m_wbSpectrumBufferLength)
					yvalue = m_wbSpectrumBuffer.at(idx) - m_dBmPanMin;

				vertexColorArray[i].x = m_r	* (yScaleColor * yvalue);
				vertexColorArray[i].y = m_g * (yScaleColor * yvalue);
				vertexColorArray[i].z = m_b * (yScaleColor * yvalue);

				if (idx < m_wbSpectrumBufferLength) {

					if (m_calibrate)
						yvalue = m_wbSpectrumBuffer.at(idx) - m_dBmPanMinOld - m_dBmPanLogGain;
					else
						yvalue = m_wbSpectrumBuffer.at(idx) - m_dBmPanMin - m_dBmPanLogGain - m_dBmScaleOffset;
				}

				if (m_mercuryAttenuator) yvalue -= 20.0f;

				vertexArray[i].x = (GLfloat)(i/scaleMult);
				vertexArray[i].y = (GLfloat)(yTop - yScale * yvalue);
				vertexArray[i].z = -1.0;

				mutex.unlock();
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


		case (PanGraphicsMode) Solid:

			glDisable(GL_MULTISAMPLE);
			glDisable(GL_LINE_SMOOTH);

			mutex.lock();
			for (int i = 0; i < vertexArrayLength; i++) {
			
				lIdx = qFloor((qreal)(i * scale));
				rIdx = qFloor((qreal)(i * scale) + scale);

				// max value
				localMax = -10000.0F;
				for (int j = lIdx; j < rIdx; j++) {

					if (m_wbSpectrumBuffer.at(j) > localMax) {

						localMax = m_wbSpectrumBuffer.at(j);
						idx = j;
					}
				}
				//qreal mean = m_wbSpectrumBuffer.at(lIdx + deltaIdx);
				//int len = rIdx - lIdx;
				//WBGRAPHICS_DEBUG << "leftIdx = " << lIdx << " rightIdx = " << rIdx << " length = " << len;
				//for (int j = lIdx+1; j < len; j++) {

				//	mean += m_wbSpectrumBuffer.at(j);
				//}
				//if (len > 0) mean /= len;

				idx += deltaIdx;

				//mutex.lock();
				vertexColorArrayBg[2*i].x = m_redST;
				vertexColorArrayBg[2*i].y = m_greenST;
				vertexColorArrayBg[2*i].z = m_blueST;
								
				vertexColorArrayBg[2*i+1].x = m_redSB;
				vertexColorArrayBg[2*i+1].y = m_greenSB;
				vertexColorArrayBg[2*i+1].z = m_blueSB;

				qreal yvalue = 0;
				if (idx < m_wbSpectrumBufferLength)
					yvalue = m_wbSpectrumBuffer.at(idx) - m_dBmPanMin - m_dBmPanLogGain;
					//yvalue = mean - m_dBmPanMin - m_dBmPanLogGain;

				if (m_mercuryAttenuator)
					yvalue -= 20.0;

				vertexArrayBg[2*i].x = (GLfloat)(i/scaleMult);
				vertexArrayBg[2*i].y = (GLfloat)(yTop - yScale * yvalue);
				vertexArrayBg[2*i].z = -2.0f;

				vertexArrayBg[2*i+1].x = (GLfloat)(i/scaleMult);
				vertexArrayBg[2*i+1].y = (GLfloat)yTop;
				vertexArrayBg[2*i+1].z = -2.0f;

				//mutex.unlock();
			}
			mutex.unlock();

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
	glDisable(GL_SCISSOR_TEST);

	// draw a rectangle for the panadapter frequency region
	if (m_dataEngineState == QSDR::DataEngineUp && !m_calibrate) {

		y1 = m_panRect.top() + 15;
		y2 = m_panRect.height() - 15;
		
		//m_frequencySpan = (m_upperFrequency - m_lowerFrequency) * m_freqScaleZoomFactor;
		//m_frequencyUnit = (qreal)(m_freqScaleRect.width() / m_frequencySpan);

		//int centerFreq = (int)(m_frequencyUnit * (m_frequency - m_lowerFrequency));
		//int deltaF = (int)(m_frequencyUnit * (float)m_sampleRate/2);
		//int linePos = centerFreq;

		int centerFreq = (int)(m_frequencyUnit * (m_frequency - m_lowerFrequency));
		int deltaF = (int)(m_frequencyUnit * (float)m_sampleRate/2);
		//int linePos = centerFreq;

	
		x1 = (GLint)(centerFreq - deltaF);
		x2 = (GLint)(centerFreq + deltaF);

//		int dist = qAbs(x2-x1);
//		if (dist < 2) {
//
//			x2 = x1 + 1;
//			linePos = x1;
//			glLineWidth(2);
//		}
//		else if (dist%2 == 0) {
//
//			glLineWidth(1);
//		}
//		else if (dist%2 == 1) {
//
//			linePos -= 1;
//			glLineWidth(2);
//		}
	
		QRect rect = QRect(x1, y1, x2-x1, y2);
		drawGLRect(rect, QColor(160, 235, 255, 80), 0.0f);

		// small vertical line
//		qglColor(QColor(255, 0, 0, 255));
//		glBegin(GL_LINES);
//			glVertex3f(centerFreq, y2 + 15, 4.0f);
//			glVertex3f(centerFreq, y2 + 30, 4.0f);
//		glEnd();

		glLineWidth(1);
	}

	//glDisable(GL_SCISSOR_TEST);
	glDisable(GL_BLEND);
	glDisable(GL_LINE_SMOOTH);
}

void QGLWidebandPanel::drawVerticalScale() {

	if (!m_dBmScaleRect.isValid()) return;

	int width = m_dBmScaleRect.width();
	int height = m_dBmScaleRect.height();

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	glColor3f(0.65f, 0.76f, 0.81f);
	
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
	glColor3f(0.65f, 0.76f, 0.81f);
}

void QGLWidebandPanel::drawHorizontalScale() {

	if (!m_freqScaleRect.isValid()) return;

	int width = m_freqScaleRect.width();
	int height = m_freqScaleRect.height();

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	glColor3f(0.65f, 0.76f, 0.81f);
	
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
	glColor3f(0.65f, 0.76f, 0.81f);
}

void QGLWidebandPanel::drawGrid() {

	if (!m_panRect.isValid()) return;

	int width = m_panRect.width();
	int height = m_panRect.height();

	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
	glColor4f(m_redGrid, m_greenGrid, m_blueGrid, 1.0);
	
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
	glColor3f(0.65f, 0.76f, 0.81f);
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
		qreal frequency = (unit * x) + m_lowerFrequency;

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

	GLint x1 = (GLint)(m_frequencyUnit * (lo - m_lowerFrequency));
	GLint x2 = (GLint)(m_frequencyUnit * (hi - m_lowerFrequency));

	GLint y1 = m_panRect.top();// + 15;
	GLint y2 = m_panRect.bottom();
	
	QRect rect = QRect(x1, y1, x2-x1, y2);

	if (x2 < m_dBmScaleRect.left()) {

		drawGLRect(
				rect,
				QColor((int)(255 * m_bkgRed), (int)(255 * m_bkgGreen), (int)(255 * m_bkgBlue), 180),
				QColor((int)(255 * m_bkgRed), (int)(255 * m_bkgGreen), (int)(255 * m_bkgBlue), 80),
				-2.0f,
				false);
	}

	//QFontMetrics d_fm(m_smallFont);
	int fontWidth = m_fonts.smallFontMetrics->boundingRect(band).width();

	qglColor(QColor(255, 255, 255, 180));
	m_oglTextSmall->renderText((x2 + x1 - fontWidth)/2.0f, y1, 5.0f, band);
}

//************************************************************************
// The algorithms of the scale functions are taken from SDRMAXIII 
// (c) Catherine Moss, with permission.

void QGLWidebandPanel::renderVerticalScale() {

	QString str;
	//QFontMetrics d_fm(m_smallFont);
	int spacing = 6;
	int fontHeight = m_fonts.smallFontMetrics->tightBoundingRect(".0dBm").height() + spacing;
	int fontMaxWidth = m_fonts.smallFontMetrics->boundingRect("-000.0").width();

	GLint width = m_dBmScaleRect.width();
	GLint height = m_dBmScaleRect.height();

	qreal unit = (qreal)(m_dBmScaleRect.height() / qAbs(m_dBmPanMax - m_dBmPanMin));

	m_dBmScale = getYRuler2(m_dBmScaleRect, fontHeight, unit, m_dBmPanMin, m_dBmPanMax);

	glClear(GL_COLOR_BUFFER_BIT);
	
	QRect textRect(0, 0, fontMaxWidth, fontHeight);
	textRect.moveLeft(3);
	m_dBmScaleTextPos = -textRect.height();

	int len		= m_dBmScale.mainPointPositions.length();
	int sublen	= m_dBmScale.subPointPositions.length();
	
	// draw the scale background
	drawGLScaleBackground(QRect(0, 0, width, height), QColor(30, 30, 30, 180));
	
	if (len > 0) {

		glColor3f(0.65f, 0.76f, 0.81f);
		glLineWidth(1);

		glBegin(GL_LINES);
		for (int i = 0; i < len; i++) {

			glVertex3f(0,     m_dBmScale.mainPointPositions.at(i), 0.0f);	// origin of the line
			glVertex3f(4, m_dBmScale.mainPointPositions.at(i), 0.0f);	// ending point of the line
		}
		glEnd();
		
		glColor3f(0.45f, 0.56f, 0.61f);
		if (sublen > 0) {

			glBegin(GL_LINES);
			for (int i = 1; i < sublen; i++) {

				glVertex3f(0.0f, (float)m_dBmScale.subPointPositions.at(i), 0.0f);	// origin of the line
				glVertex3f(2.0f, (float)m_dBmScale.subPointPositions.at(i), 0.0f);	// ending point of the line
			}
			glEnd();
		}

		glColor3f(0.75f, 0.86f, 0.91f);
		for (int i = 0; i < len; i++) {

			textRect.moveBottom((int)m_dBmScale.mainPointPositions.at(i) + textRect.height()/2);
			
			//if (textRect.y() >= yOld && textRect.bottom() <= (m_dBmScaleRect.height() - textRect.height())) {
			if (textRect.y() > m_dBmScaleRect.top() + textRect.height() && textRect.bottom() <= (m_dBmScaleRect.height() - textRect.height()/2)) {
			
				str = QString::number((qreal)m_dBmScale.mainPoints.at(i), 'f', 1);
				m_oglTextSmall->renderText(textRect.x() + 10, textRect.y(), str);
				m_dBmScaleTextPos = textRect.bottom();
			}
		}
	}

	textRect.moveTop(m_dBmScaleRect.top());
	glColor3f(0.94f, 0.22f, 0.43f);
	
	str = QString("dBm");
	m_oglTextSmall->renderText(textRect.x() + 18, textRect.y(), str);
}

void QGLWidebandPanel::renderHorizontalScale() {

	if (m_freqScaleRect.isEmpty()) return;

	//QFontMetrics d_fm(m_smallFont);
	int fontHeight = m_fonts.smallFontMetrics->tightBoundingRect(".0kMGHz").height();
	int fontMaxWidth = m_fonts.smallFontMetrics->boundingRect("000.000").width();

	m_frequencySpan = MAXFREQUENCY * m_freqScaleZoomFactor;
	m_frequencyUnit = (qreal)(m_freqScaleRect.width() / m_frequencySpan);
	m_frequencyScale = getXRuler(m_freqScaleRect, fontMaxWidth, m_frequencyUnit, m_lowerFrequency, m_upperFrequency);
	
	// draw the frequency scale
	int		offset_X		= -1;
	int		textOffset_y	= 5;
	double	freqScale		= 1;

	QString fstr = QString(" Hz ");
	if (m_upperFrequency >= 1e6) { freqScale = 1e6; fstr = QString("  MHz "); }
	else
	if (m_upperFrequency >= 1e3) { freqScale = 1e3; fstr = QString("  kHz "); }

	// draw the wide band scale background
	drawGLScaleBackground(QRect(0, 0, m_freqScaleRect.width(), m_freqScaleRect.height()), QColor(0, 0, 0, 255));

	QRect scaledTextRect(0, textOffset_y, 1, fontHeight);
	scaledTextRect.setWidth(m_fonts.smallFontMetrics->width(fstr));
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

			int text_width = m_fonts.smallFontMetrics->width(str);
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

			glVertex3f(m_frequencyScale.subPointPositions.at(i), 1.0f, 0.0f);
			glVertex3f(m_frequencyScale.subPointPositions.at(i), 3.0f, 0.0f);
		}
		glEnd();
	}

	glColor3f(0.94f, 0.22f, 0.43f);
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

	//if (event->buttons() == Qt::NoButton) getRegion(pos);

	//double freqStep = set->getMouseWheelFreqStep(m_currentReceiver);

	//switch (m_mouseRegion) {

	//	case panRegion:

	//		double delta = 0;
	//		if (event->delta() < 0) delta = -freqStep;
	//		else
	//		if (event->delta() > 0) delta =  freqStep;

	//		if (m_frequency + delta > MAXFREQUENCY)
	//			m_frequency = MAXFREQUENCY;
	//		else
	//		if (m_frequency + delta < 0)
	//			m_frequency = 0;
	//		else
	//			// snap to the frequency step
	//			m_frequency = (long)(qRound((m_frequency + delta) / qAbs(freqStep)) * qAbs(freqStep));

	//		set->setFrequency(this, true, m_currentReceiver, m_frequency);
	//		break;
	//}

 //	//updateGL();
	//update();
}

void QGLWidebandPanel::mousePressEvent(QMouseEvent* event) {
	
	//GRAPHICS_DEBUG << "mousePressEvent";
	m_mousePos = event->pos();
	m_mouseDownPos = m_mousePos;

	getRegion(m_mousePos);

	if (m_mouseRegion == freqScaleRegion) {

		m_yScaleMouseDownPos = m_freqScaleRect.topLeft();
		
		if (event->buttons() == Qt::RightButton) setCursor(Qt::SplitHCursor);
		update();

		return;
	}
	else if (m_mouseRegion == dBmScaleRegion) {

		//m_yScaleMouseDownPos = m_dBmScaleRect.topLeft();

		if (event->buttons() == Qt::RightButton) {

			setCursor(Qt::SplitVCursor);
			m_yScaleMouseDownPos = m_dBmScaleRect.topLeft();
		}

		if (event->buttons() == Qt::LeftButton &&
			event->modifiers() == Qt::ControlModifier) {

			setCursor(Qt::SplitVCursor);

			m_dBmPanMinOld = m_dBmPanMin;
			m_dBmPanMaxOld = m_dBmPanMax;

			m_dBmScaleOffset = 0.0;
			m_calibrate = true;
		}

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
			
			m_frequency = (long)(1000 * (int)(qRound(m_mousePos.x()/unit + m_lowerFrequency)/1000));
			set->setVFOFrequency(this, 1, m_currentReceiver, m_frequency);
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
	
	if (m_calibrate) m_calibrate = false;

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
			
			if (event->buttons() == Qt::LeftButton &&
				event->modifiers() == Qt::ControlModifier) {

				QPoint dPos = m_mouseDownPos - pos;
				qreal unit = (qreal)(qAbs(m_dBmPanMax - m_dBmPanMin) / m_panRect.height());
				qreal delta =  - unit * dPos.y();

				qreal newMin = m_dBmPanMin + delta;
				qreal newMax = m_dBmPanMax + delta;

				m_dBmScaleOffset -= delta / unit;
				WBGRAPHICS_DEBUG << "m_dBmScaleOffset: " << m_dBmScaleOffset;

				if (newMin > MINDBM && newMax < MAXDBM) {

					m_dBmPanMin = newMin;
					m_dBmPanMax = newMax;

					//set->setWidebanddBmScaleMin(this, m_dBmPanMin);
					//set->setWidebanddBmScaleMax(this, m_dBmPanMax);
				}

				m_mouseDownPos = pos;
				m_dBmScaleUpdate = true;
				m_panGridUpdate = true;
			}
			else if (event->buttons() == Qt::LeftButton) {

				QPoint dPos = m_mouseDownPos - pos;
				qreal unit = (qreal)(qAbs(m_dBmPanMax - m_dBmPanMin) / m_panRect.height());
				
				qreal newMin = m_dBmPanMin - unit * dPos.y();
				qreal newMax = m_dBmPanMax - unit * dPos.y();

				if (newMin > MINDBM && newMax < MAXDBM) {
				
					m_dBmPanMin = newMin;
					m_dBmPanMax = newMax;

					set->setWidebanddBmScaleMin(this, m_dBmPanMin);
					set->setWidebanddBmScaleMax(this, m_dBmPanMax);
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
				
				//m_dBmPanMin += m_dBmPanDelta;
				m_dBmPanMax -= m_dBmPanDelta;

				if (qAbs(m_dBmPanMax - m_dBmPanMin) < 10) {

					m_dBmPanMin -= m_dBmPanDelta;
					m_dBmPanMax += m_dBmPanDelta;
				}
				if (m_dBmPanMin < MINDBM) m_dBmPanMin = MINDBM;
				if (m_dBmPanMax > MAXDBM) m_dBmPanMax = MAXDBM;

				set->setWidebanddBmScaleMin(this, m_dBmPanMin);
				set->setWidebanddBmScaleMax(this, m_dBmPanMax);

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

					m_frequencySpan = MAXFREQUENCY * m_freqScaleZoomFactor;

					qreal unit = (qreal)((MAXFREQUENCY * m_freqScaleZoomFactor) / m_freqScaleRect.width());

					m_lowerFrequency += unit * dPos.x();
					m_upperFrequency = m_lowerFrequency + m_frequencySpan;

					if (m_lowerFrequency < 0.0) m_lowerFrequency = 0.0;
					if (m_upperFrequency > (qreal) MAXFREQUENCY) {

						m_upperFrequency = (qreal)MAXFREQUENCY;
						m_lowerFrequency = (qreal)(MAXFREQUENCY - m_frequencySpan);
					}

					m_mouseDownPos = pos;
					m_freqScaleUpdate = true;
					m_panGridUpdate = true;
				}
				else {

					m_lowerFrequency = 0.0;
					m_upperFrequency = (qreal) MAXFREQUENCY;
				}
			}
			else if (event->buttons() == Qt::RightButton) {
				
				QPoint dPos = m_mouseDownPos - pos;

				if (dPos.x() > 0) {

					m_freqScaleZoomFactor += 0.005f;
				}
				else if (dPos.x() < 0)
					m_freqScaleZoomFactor -= 0.005f;

				if (m_freqScaleZoomFactor > 1.0) m_freqScaleZoomFactor = 1.0f;
				//if (m_freqScaleZoomFactor < 0.24) m_freqScaleZoomFactor = 0.24f;
				if (m_freqScaleZoomFactor < 0.15) m_freqScaleZoomFactor = 0.15f;

				qreal unit = (qreal)((MAXFREQUENCY * m_freqScaleZoomFactor) / m_freqScaleRect.width());
				m_lowerFrequency -= unit * dPos.x();
				m_upperFrequency = m_lowerFrequency + m_frequencySpan;

				if (m_lowerFrequency < 0.0) m_lowerFrequency = 0.0;
				if (m_upperFrequency > (qreal) MAXFREQUENCY) {

					m_upperFrequency = (qreal)MAXFREQUENCY;
					m_lowerFrequency = (qreal)(MAXFREQUENCY - m_frequencySpan);
				}

				m_mouseDownPos = pos;
				m_freqScaleUpdate = true;
				m_panGridUpdate = true;
			}
			else
				setCursor(Qt::ArrowCursor);

			update();
			break;

		case elsewhere:
			//WBGRAPHICS_DEBUG << "elsewhere";
			
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
 
void QGLWidebandPanel::setFrequency(QObject *sender, int mode, int rx, long freq) {

	Q_UNUSED (sender)
	Q_UNUSED (mode)
	Q_UNUSED (rx)
	
	m_frequency = freq;
	m_freqScaleUpdate = true;
	m_panGridUpdate = true;

	//updateGL();
	update();
}

void QGLWidebandPanel::setCurrentReceiver(QObject *sender, int value) {

	Q_UNUSED(sender)

	m_currentReceiver = value;
	update();
}

//void QGLWidebandPanel::computeDisplayBins(const float *panBuffer) {
//
//	//int newSampleSize = 0;
//	//int deltaSampleSize = 0;
//	//int idx = 0;
//	//int lIdx = 0;
//	//int rIdx = 0;
//	//qreal localMax;
//
//
//	//m_panScale = (qreal)(1.0 * newSampleSize / m_panRectWidth);
//	//m_scaleMultOld = m_displayData.scaleMult;
//	//	
//	//if (m_panScale < 0.125) {
//	//	m_displayData.scaleMult = 0.0625;
//	//}
//	//else if (m_panScale < 0.25) {
//	//	m_displayData.scaleMult = 0.125;
//	//}
//	//else if (m_panScale < 0.5) {
//	//	m_displayData.scaleMult = 0.25;
//	//}
//	//else if (m_panScale < 1.0) {
//	//	m_displayData.scaleMult = 0.5;
//	//}
//	//else {
//	//	m_displayData.scaleMult = 1.0;
//	//}
//
//	//m_panSpectrumBinsLength = (GLint)(m_displayData.scaleMult * m_panRectWidth);
//
//	//if (m_scaleMultOld != m_displayData.scaleMult) {
//
//	//	m_displayData.waterfallUpdate = true;
//	//}
//
//	//m_displayData.waterfallPixel.clear();
//	//m_displayData.waterfallPixel.resize(4 * m_panRectWidth);
//
//	//m_displayData.panadapterBins.clear();
//	//
//	//for (int i = 0; i < m_panSpectrumBinsLength; i++) {
//	//		
//	//	idx = 0;
//	//	lIdx = (int)floor((qreal)(i * m_panScale / m_displayData.scaleMult));
//	//	rIdx = (int)floor((qreal)(i * m_panScale / m_displayData.scaleMult) + m_panScale / m_displayData.scaleMult);
//	//				
//	//	// max value; later we try mean value also!
//	//	localMax = -10000.0F;
//	//	for (int j = lIdx; j < rIdx; j++) {
//
//	//		if (panBuffer[j] > localMax) {
//
//	//			localMax = panBuffer[j];
//	//			idx = j;
//	//		}
//	//	}
//	//	idx += deltaSampleSize/2;
//	//			
//	//	m_displayData.panadapterBins << panBuffer[idx] - m_displayData.dBmPanMin - m_dBmPanLogGain;
//
//	//update();
//	////updateGL();
//}

void QGLWidebandPanel::setWidebandSpectrumBuffer(const qVectorFloat &buffer) {

	//int deltaIdx;
	//qreal frequencyScale;
	//qreal scaleMult = 1.0;

	m_wbSpectrumBufferLength = buffer.size();

	mutex.lock();
	m_wbSpectrumBuffer.resize(m_wbSpectrumBufferLength);
	m_wbSpectrumBuffer = buffer;

	//m_scaledBufferSize = qFloor(m_wbSpectrumBufferLength * m_freqScaleZoomFactor);
	mutex.unlock();

//	deltaIdx = qFloor((qreal)(m_wbSpectrumBufferLength * (m_lowerFrequency / MAXFREQUENCY)));
//	frequencyScale = (qreal)(1.0f * m_scaledBufferSize / width);
//
//	if (frequencyScale < 0.125)
//		scaleMult = 0.0625;
//	else if (frequencyScale < 0.25)
//		scaleMult = 0.125;
//	else if (frequencyScale < 0.5)
//		scaleMult = 0.25;
//	else if (frequencyScale < 1.0)
//		scaleMult = 0.5;

	update();
}

void QGLWidebandPanel::resetWidebandSpectrumBuffer() {

	m_wbSpectrumBuffer.resize(BIGWIDEBANDSIZE / 2);
	m_wbSpectrumBuffer.fill(-1000.0f);
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

	//if (m_serverMode != mode)
	//	m_serverMode = mode;

	if (m_serverMode == mode)
		return;
	else {

		m_wbSpectrumBuffer.fill(-10000.0);
		//memset(&m_wbSpectrumBuffer, -10000, 4 * BUFFER_SIZE * sizeof(float));
		m_serverMode = mode;
	}

	//resizeGL(width(), height());
	m_displayTime.restart();
	
	update();
}

void QGLWidebandPanel::graphicModeChanged(
	QObject *sender,
	int rx,
	PanGraphicsMode panMode,
	WaterfallColorMode colorScheme)
{
	Q_UNUSED (sender)
	//Q_UNUSED (rx)

	if (rx != -1) return;

	bool change = false;

	if (m_panMode != panMode) {
		
		m_panMode = panMode;
		change = true;
	}

	if (m_waterfallMode != colorScheme) {

		m_waterfallMode = colorScheme;
		change = true;
	}

	if (!change) return;

	update();
}

 
//void QGLWidebandPanel::setSpectrumAveragingCnt(int value) {
//
//	mutex.lock();
//
//		memset(m_tmpBuf, 0, SAMPLE_BUFFER_SIZE * sizeof(float));
//
//		while (!specAv_queue.isEmpty())
//			specAv_queue.dequeue();
//
//		m_specAveragingCnt = value;
//
//		if (m_specAveragingCnt > 0)
//			m_scale = 1.0f / m_specAveragingCnt;
//		else
//			m_scale = 1.0f;
//
//	mutex.unlock();
//}

void QGLWidebandPanel::setMercuryAttenuator(QObject* sender, HamBand band, int value) {

	Q_UNUSED(sender)
	Q_UNUSED(band)
	
		m_mercuryAttenuator = value;
	update();
}

void QGLWidebandPanel::setPanadapterColors() {

	m_spectrumColorsChanged = true;

	mutex.lock();
	m_bkgRed   = (GLfloat)(set->getPanadapterColors().panBackgroundColor.red() / 256.0);
	m_bkgGreen = (GLfloat)(set->getPanadapterColors().panBackgroundColor.green() / 256.0);
	m_bkgBlue  = (GLfloat)(set->getPanadapterColors().panBackgroundColor.blue() / 256.0);

	m_r	= (GLfloat)(set->getPanadapterColors().wideBandLineColor.red() / 256.0);
	m_g = (GLfloat)(set->getPanadapterColors().wideBandLineColor.green() / 256.0);
	m_b	= (GLfloat)(set->getPanadapterColors().wideBandLineColor.blue() / 256.0);

	m_rf = (GLfloat)(set->getPanadapterColors().wideBandFilledColor.red() / 256.0);
	m_gf = (GLfloat)(set->getPanadapterColors().wideBandFilledColor.green() / 256.0);
	m_bf = (GLfloat)(set->getPanadapterColors().wideBandFilledColor.blue() / 256.0);

	m_redST	  = (GLfloat)(set->getPanadapterColors().panSolidTopColor.red() / 256.0);
	m_greenST = (GLfloat)(set->getPanadapterColors().panSolidTopColor.green() / 256.0);
	m_blueST  = (GLfloat)(set->getPanadapterColors().panSolidTopColor.blue() / 256.0);

	m_redSB   = (GLfloat)(set->getPanadapterColors().panSolidBottomColor.red() / 256.0);
	m_greenSB = (GLfloat)(set->getPanadapterColors().panSolidBottomColor.green() / 256.0);
	m_blueSB  = (GLfloat)(set->getPanadapterColors().panSolidBottomColor.blue() / 256.0);

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

void QGLWidebandPanel::setPanGridStatus(bool value, int rx) {

	Q_UNUSED (rx)

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
