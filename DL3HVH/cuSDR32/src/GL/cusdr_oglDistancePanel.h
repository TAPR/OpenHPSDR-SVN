/**
* @file  cusdr_oglDistancePanel.h
* @brief distance panel header file for cuSDR
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

#ifndef _CUSDR_QGL_DISTANCEPANEL_H
#define _CUSDR_QGL_DISTANCEPANEL_H

#include "cusdr_oglUtils.h"
#include "cusdr_oglInfo.h"
#include "cusdr_settings.h"
#include "cusdr_fonts.h"
#include "cusdr_oglText.h"

//#include <QtOpenGL/QGLWidget>
//#include <QImage>
//#include <QFontMetrics>
#include <QWheelEvent>
//#include <QQueue>
//#include <QDebug>



class QGLDistancePanel : public QGLWidget {

    Q_OBJECT

public:
    QGLDistancePanel(QWidget *parent = 0);
	~QGLDistancePanel();

public slots:
	//QSize minimumSizeHint() const;
	QSize sizeHint() const;

	void setSpectrumBuffer(const float *buffer);
	void distanceSpectrumBufferChanged(int sampleRate, qint64 length, const float *buffer);
	void setFrequency(QObject *sender, bool value, long freq);

protected:
    void initializeGL();
    void resizeGL(int iWidth, int iHeight);
    void paintGL();
    
	void enterEvent(QEvent *event);
	void leaveEvent(QEvent *event);
	void mousePressEvent(QMouseEvent *event);
	void mouseReleaseEvent(QMouseEvent *event);
	void mouseMoveEvent(QMouseEvent *event);
	void wheelEvent(QWheelEvent * event );
	void keyPressEvent(QKeyEvent* event);
	void closeEvent(QCloseEvent *event);
	void showEvent(QShowEvent *event);
	void timerEvent(QTimerEvent *);

private:
	Settings*	set;

	QSDR::_ServerMode			m_serverMode;
	QSDR::_HWInterfaceMode		m_hwInterface;
	QSDR::_DataEngineState		m_dataEngineState;

	QSDRGraphics::_Panadapter	m_panadapterMode;
	
	QTime			m_displayTime;
	QTime			m_resizeTime;
	QTime			freqChangeTimer;
	
	CFonts			*fonts;

	TScale			m_frequencyScale;
	TScale			m_dBmScale;
	TScale			m_distanceScale;
	TScale			m_dBmDistScale;
	TFonts			m_fonts;
	
	QList<TReceiver>	m_rxDataList;
	
	QVector<qreal>					m_panadapterBins;
	QQueue<QVector<float> >			specAv_queue;

	QGLFramebufferObject*			m_frequencyScaleFBO;
	QGLFramebufferObject*			m_dBmScaleFBO;
	QGLFramebufferObject*			m_panadapterGridFBO;
	QGLFramebufferObject*			m_textureFBO;
	
	QRect		m_panRect;
	QRect		m_dBmScalePanRect;
	QRect		m_freqScalePanRect;
	QRect		m_distancePanRect;
	QRect		m_freqScaleDistancePanRect;
	QRect		m_dBmScaleDistancePanRect;
	QRect		m_filterRect;
	QRect		m_freqScaleWideBandPanRect;
	QRect		m_dBmScaleWideBandPanRect;
	QRect		m_distanceSpectrumRect;

	OGLText*	m_oglTextTiny;
	OGLText*	m_oglTextSmall;
	OGLText*	m_oglTextNormal;

	QPoint		m_mousePos;
	QPoint		m_oldMousePos;
	QPoint		m_mouseLastPos;
	QPoint		m_mouseDownPos;
	QPoint		m_rulerMouseDownPos;
	
	enum Region {

		freqScalePanadapterRegion,
		freqScaleDistancePanRegion,
		panadapterRegion,
		dBmScalePanadapterRegion,
		dBmScaleDistancePanRegion,
		distancePanRegion,
		filterRegion,
		filterRegionLow,
		filterRegionHigh,
		elsewhere,
		out
	};

	long		m_frequency;
	long		m_oldFreq;

	float		m_spectrumBuffer[4*BUFFER_SIZE];
	float		m_distanceSpectrumBuffer[16*BUFFER_SIZE];

	float		m_scale;
	float		m_tmpBuf[SAMPLE_BUFFER_SIZE];
	float		m_avgBuf[SAMPLE_BUFFER_SIZE];
	
	float		m_distMax;

	QMutex		mutex;
	QMutex		spectrumBufferMutex;
	QMutex		distanceSpectrumBufferMutex;

	int			m_oldWidth;
	int			m_oldPanRectHeight;
	int			m_smallFontHeight;
	int			m_cnt;
	int			m_specAveragingCnt;
	int			m_freqRulerDisplayWidth;
	int			m_displayTop;
	int			m_dBmPanLogGain;
	int			m_panSpectrumMinimumHeight;
	
	unsigned int timer;
    
	GLint		m_panRectWidth;
	GLint		m_panSpectrumBinsLength;

	bool		m_spectrumUpdate;
	bool		m_showZerodBmLine;
	bool		m_spectrumVertexColorUpdate;
	bool		m_dBmScalePanadapterRenew;
	bool		m_dBmScalePanadapterUpdate;
	bool		m_freqScalePanadapterRenew;
	bool		m_freqScalePanadapterUpdate;
	bool		m_panGridRenew;
	bool		m_panGridUpdate;
	bool		m_distRulerUpdate;
	bool		m_newWidebandPanFreqRuler;
	bool		m_spectrumColorsChanged;
	bool		m_showChirpFFT;
	bool		m_spectrumAveraging;
	bool		m_spectrumAveragingOld;
	bool		m_crossHairCursor;
	bool		m_panGrid;
	
	float		m_freqRulerPosition;
	float		m_wideBandZoomFactor;
	double		m_distRulerKilometerSpan;
	double		m_distRulerDisplayDelta;
	double		m_distRulerDisplayDeltaStep;
	double		m_distRulerMaxDist;

	qreal		m_panFrequencyScale;
	qreal		m_freqScaleZoomFactor;
	qreal		m_distScaleZoomFactor;
	qreal		m_dBmPanMin;
	qreal		m_dBmPanMax;
	qreal		m_dBmPanDelta;
	qreal		m_panScale;
	qreal		m_scaleMult;
	qreal		m_scaleMultOld;
	qreal		m_filterLowerFrequency;
	qreal		m_filterUpperFrequency;
	qreal		m_mouseDownFilterFrequencyLo;
	qreal		m_mouseDownFilterFrequencyHi;
	qreal		m_dBmDistScaleMin;
	qreal		m_dBmDistScaleMax;
	qreal		m_dBmDistMin;
	qreal		m_dBmDistMax;
	qreal		m_dBmDistDelta;

	int			m_mouseRegion;
	int			m_oldMouseRegion;
	int			m_snapMouse;
	int			m_panDisplayMode;
	int			m_sampleRate;
	int			m_downRate;
	
	
	double		m_kilometersPerGate;
	qint64		m_chirpBufferLength;

	GLfloat		m_bkgRed;
	GLfloat		m_bkgGreen;
	GLfloat		m_bkgBlue;

	GLfloat		m_red;
	GLfloat		m_green;
	GLfloat		m_blue;

	GLfloat		m_redF;
	GLfloat		m_greenF;
	GLfloat		m_blueF;


	GLfloat		m_redST;
	GLfloat		m_greenST;
	GLfloat		m_blueST;

	GLfloat		m_redSB;
	GLfloat		m_greenSB;
	GLfloat		m_blueSB;

	GLfloat		m_redD;
	GLfloat		m_greenD;
	GLfloat		m_blueD;

	//******************************************************************
	//QColor	getWaterfallColorAtPixel(qreal value);

	void	saveGLState();
	void	restoreGLState();

	// drawing
	void	paintReceiverDisplay();
	void	paintChirpWSPRDisplay();

	void	drawPanadapter();
	void 	drawPanVerticalScale();
	void 	drawPanHorizontalScale();
	void 	drawPanadapterGrid();
	void 	drawPanFilter();
	void	drawCrossHair();

	void	drawDistanceSpectrum();
	void	drawDistHorizontalScale();
	void	drawDistVerticalScale();

	void 	renderPanVerticalScale();
	void 	renderPanHorizontalScale();
	void 	renderPanadapterGrid();
	
	void	computeDisplayBins(const float *panBuffer);

private slots:

	void	systemStateChanged(
					QObject* sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);

	void	graphicModeChanged(
					QObject* sender, 
					QSDRGraphics::_Panadapter panMode,
					QSDRGraphics::_WfScheme colorScheme);

	void	setupConnections();
	void	setFilterFrequencies(QObject *sender, int rx, qreal lo, qreal hi);
	void	setupDisplayRegions(QSize size);
	
	void	setDistanceSpectrumBuffer(int sampleRate, qint64 length, const float *buffer);
	void	setSpectrumAveraging(bool value);
	void	setSpectrumAveragingCnt(int value);
	void	setPanGridStatus(bool value);
	void	setPanadapterColors();
	void	getRegion(QPoint p);
	void	freqRulerPositionChanged(float pos, int rx);
	void	sampleRateChanged(QObject *sender, int value);
	void	setChirpFFTShow(bool value);

signals:
	void showEvent(QObject* sender);
	void closeEvent(QObject* sender);
	void messageEvent(QString msg);
	void coordChanged(int x,int y);
};

#endif  // _CUSDR_QGL_DISTANCEPANEL_H
