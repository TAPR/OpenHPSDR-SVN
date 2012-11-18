/**
* @file  cusdr_oglReceiverPanel.h
* @brief receiver panel header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-03-14
*/

/*
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

#ifndef _CUSDR_QGL_RECEIVERPANEL_H
#define _CUSDR_QGL_RECEIVERPANEL_H

#include "cusdr_oglUtils.h"
#include "cusdr_oglInfo.h"
#include "cusdr_settings.h"
#include "Util/cusdr_buttons.h"
#include "Util/cusdr_fonts.h"
#include "cusdr_oglText.h"
#include "QtDSP/qtdsp_dualModeAverager.h"

#include <QWheelEvent>
#include <QtOpenGL/QGLWidget>


#ifdef LOG_GRAPHICS
#   define GRAPHICS_DEBUG qDebug().nospace() << "Graphics::\t"
#else
#   define GRAPHICS_DEBUG nullDebug()
#endif


class QGLReceiverPanel : public QGLWidget {

    Q_OBJECT

public:
    QGLReceiverPanel(QWidget *parent = 0, int rx = 0);
	~QGLReceiverPanel();

public slots:
	QSize minimumSizeHint() const;
	QSize sizeHint() const;

	void setSpectrumBuffer(const float *buffer);
	//void setFrequency(QObject *sender, bool value, long freq);
	void	setFrequency(QObject *sender, bool value, int rx, long freq);

protected:
    void initializeGL();
    void resizeGL(int iWidth, int iHeight);
    void paintGL();
    
	void enterEvent(QEvent *event);
	void leaveEvent(QEvent *event);
	void mousePressEvent(QMouseEvent *event);
	void mouseDoubleClickEvent(QMouseEvent *event);
	void mouseReleaseEvent(QMouseEvent *event);
	void mouseMoveEvent(QMouseEvent *event);
	void wheelEvent(QWheelEvent * event );
	void keyPressEvent(QKeyEvent* event);

private:
	Settings	*set;

	QSDR::_ServerMode			m_serverMode;
	QSDR::_HWInterfaceMode		m_hwInterface;
	QSDR::_DataEngineState		m_dataEngineState;

	QSDRGraphics::_Panadapter	m_panadapterMode;
	QSDRGraphics::_WfScheme 	m_waterColorScheme;

	CFonts		*fonts;
	TFonts		m_fonts;

	AGCMode			m_agcMode;

	QTime			m_displayTime;
	QTime			m_resizeTime;
	QTime			freqChangeTimer;
	QTime			peakHoldTimer;
	
	QString			m_bandText;
	QString			m_agcModeString;

	//QGLWidget		*m_shareWidget;

	DualModeAverager*	averager;

	TScale			m_frequencyScale;
	TScale			m_dBmScale;
	TScale			m_secScale;
	
	QList<TReceiver>	m_rxDataList;
	
	QVector<qreal>					m_panadapterBins;
	QVector<qreal>					m_panPeakHoldBins;
	QVarLengthArray<TGL_ubyteRGBA>	m_waterfallPixel;

	QQueue<QVector<float> >			specAv_queue;

	QGLFramebufferObject			*m_frequencyScaleFBO;
	QGLFramebufferObject			*m_dBmScaleFBO;
	QGLFramebufferObject			*m_panadapterGridFBO;
	QGLFramebufferObject			*m_textureFBO;
	QGLFramebufferObject			*m_waterfallLineFBO;
	QGLFramebufferObject			*m_waterfallFBO;
	QGLFramebufferObject			*m_secScaleWaterfallFBO;

	QRect		m_panRect;
	QRect		m_dBmScalePanRect;
	QRect		m_freqScalePanRect;
	QRect		m_waterfallRect;
	QRect		m_secScaleWaterfallRect;
	QRect		m_filterRect;
	QRect		m_agcButtonRect;
	
	OGLText		*m_oglTextTiny;
	OGLText		*m_oglTextSmall;
	OGLText		*m_oglTextNormal;
	OGLText		*m_oglTextFreq1;
	OGLText		*m_oglTextFreq2;
	OGLText		*m_oglTextBig1;
	OGLText		*m_oglTextBig2;
	OGLText		*m_oglTextHuge;

	QPoint		m_mousePos;
	QPoint		m_oldMousePos;
	QPoint		m_mouseLastPos;
	QPoint		m_mouseDownPos;
	QPoint		m_rulerMouseDownPos;
	QPoint		m_cameraAngle;

	QColor		m_waterfallLoColor;
	QColor		m_waterfallHiColor;
	QColor		m_waterfallMidColor;
	QColor		m_gridColor;
	
	QMutex		mutex;
	QMutex		spectrumBufferMutex;

	enum Region {

		freqScalePanadapterRegion,
		panadapterRegion,
		dBmScalePanadapterRegion,
		waterfallRegion,
		filterRegion,
		filterRegionLow,
		filterRegionHigh,
		agcButtonRegion,
		agcThresholdLine,
		agcHangLine,
		agcFixedGainLine,
		elsewhere,
		out
	};
    
	GLint		m_panRectWidth;
	GLint		m_panSpectrumBinsLength;

	GLint		m_filterLeft;
	GLint		m_filterRight;
	GLint		m_filterTop;
	GLint		m_filterBottom;

	GLfloat		m_agcThresholdPixel;
	GLfloat		m_agcHangLevelPixel;
	GLfloat		m_agcFixedGainLevelPixel;

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

	GLfloat		m_redGrid;
	GLfloat		m_greenGrid;
	GLfloat		m_blueGrid;
	
	unsigned int timer;

	int			m_bigHeight;
	int			m_bigWidth;
	int			m_receiver;
	int			m_frequencyRxOnRx;
	int			m_spectrumSize;
	int			m_oldWidth;
	int			m_oldPanRectHeight;
	int			m_cnt;
	int			m_specAveragingCnt;
	int			m_currentReceiver;
	int			m_waterfallAlpha;
	int			m_waterfallOffsetLo;
	int			m_waterfallOffsetHi;
	int			m_waterfallColorRange;
	int			m_freqRulerDisplayWidth;
	int			m_oldWaterfallWidth;
	int			m_displayTop;
	int			m_dBmPanLogGain;
	int			m_panSpectrumMinimumHeight;
	int			m_mouseRegion;
	int			m_oldMouseRegion;
	int			m_oldMousePosX;
	int			m_snapMouse;
	int			m_panDisplayMode;
	int			m_sampleRate;
	int			m_downRate;
	int			m_mercuryAttenuator;
	int			m_haircrossOffsetRight;
	int			m_haircrossOffsetLeft;
	int			m_haircrossMaxRight;
	int			m_haircrossMinTop;
	int			m_displayCenterlineHeight;
	int			m_waterfallLineCnt;
	int			m_adcStatus;
	int			m_fps;

	long		m_frequency;
	long		m_otherFrequency;
	//long		m_oldFreq;

	bool		m_smallSize;
	bool		m_spectrumVertexColorUpdate;
	bool		m_dBmScalePanadapterRenew;
	bool		m_dBmScalePanadapterUpdate;
	bool		m_freqScalePanadapterRenew;
	bool		m_freqScalePanadapterUpdate;
	bool		m_secScaleWaterfallUpdate;
	bool		m_secScaleWaterfallRenew;
	bool		m_panGridRenew;
	bool		m_panGridUpdate;
	bool		m_waterfallUpdate;
	bool		m_waterfallDisplayUpdate;
	bool		m_distRulerUpdate;
	bool		m_newWidebandPanFreqRuler;
	bool		m_spectrumColorsChanged;
	bool		m_spectrumAveraging;
	bool		m_spectrumAveragingOld;
	bool		m_crossHairCursor;
	bool		m_panGrid;
	bool		m_peakHold;
	bool		m_filterChanged;
	bool		m_showFilterLeftBoundary;
	bool		m_showFilterRightBoundary;
	bool		m_highlightFilter;
	bool		m_peakHoldBufferResize;
	bool		m_showAGCLines;
	bool		m_agcHangEnabled;
	
	qreal		m_yScaleFactor;
	qreal		m_panFrequencyScale;
	qreal		m_freqScaleZoomFactor;
	qreal		m_dBmPanMin;
	qreal		m_dBmPanMax;
	qreal		m_dBmPanDelta;
	qreal		m_mouseWheelFreqStep;
	qreal		m_secWaterfallMin;
	qreal		m_secWaterfallMax;
	qreal		m_panScale;
	qreal		m_scaleMult;
	qreal		m_scaleMultOld;
	qreal		m_filterLowerFrequency;
	qreal		m_filterUpperFrequency;
	qreal		m_mouseDownFilterFrequencyLo;
	qreal		m_mouseDownFilterFrequencyHi;
	qreal		m_filterLo;
	qreal		m_filterHi;
	qreal		m_agcThresholdNew;
	qreal		m_agcThresholdOld;
	qreal		m_mouseDownAGCThreshold;
	qreal		m_agcHangLevelNew;
	qreal		m_agcHangLevelOld;
	qreal		m_mouseDownAGCHangLevel;
	qreal		m_agcFixedGain;
	qreal		m_mouseDownFixedGainLevel;

	float		m_scale;
	float		m_cameraDistance;
	float		m_freqRulerPosition;
	
	QVector<float>	m_tmp;
	

	//******************************************************************
	void	setupConnections();

	QColor	getWaterfallColorAtPixel(qreal value);

	void	saveGLState();
	void	restoreGLState();

	// drawing
	void	paintReceiverDisplay();
	void	paint3DPanadapterMode();

	void	drawPanadapter();
	void 	drawPanVerticalScale();
	void 	drawPanHorizontalScale();
	void 	drawPanadapterGrid();
	void 	drawPanFilter();
	void 	drawWaterfall();
	void 	drawWaterfallVerticalScale();
	void	drawCrossHair();
	void 	drawReceiverInfo();
	void	drawAGCControl();

	void 	renderPanVerticalScale();
	void 	renderPanHorizontalScale();
	void 	renderPanadapterGrid();
	void 	renderWaterfallVerticalScale();

	//void	computeDisplayBins(const float *panBuffer, const float *waterfallBuffer);
	void	computeDisplayBins(const QVector<float> &panBuffer, const float *waterfallBuffer);
	void 	showText(float x, float y, float z, const QString &text, bool smallText);

private slots:
	void	systemStateChanged(
					QObject *sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);

	void	graphicModeChanged(
					QObject *sender, 
					QSDRGraphics::_Panadapter panMode,
					QSDRGraphics::_WfScheme colorScheme);

	void	setSpectrumSize(QObject *sender, int value);
	void	setCurrentReceiver(QObject *sender, int value);
	void 	setHamBand(QObject *sender, int rx, bool byButton, HamBand band);
	void	setCoupledRx(QObject* sender, int value);
	void	setFilterFrequencies(QObject *sender, int rx, qreal lo, qreal hi);
	void	setMercuryAttenuator(QObject* sender, HamBand band, int value);
	void	setupDisplayRegions(QSize size);
	
	void	setSpectrumAveraging(bool value);
	void	setSpectrumAveragingCnt(int value);
	void	setPanGridStatus(bool value);
	void	setPeakHoldStatus(bool value);
	void	setPanadapterColors();
	void	getRegion(QPoint p);
	void	freqRulerPositionChanged(float pos, int rx);
	void	sampleRateChanged(QObject *sender, int value);
	void	setWaterfallOffesetLo(int rx, int value);
	void	setWaterfallOffesetHi(int rx, int value);
	void	setdBmScaleMin(int rx, qreal value);
	void	setdBmScaleMax(int rx, qreal value);
	void	setMouseWheelFreqStep(QObject *, int, qreal);

	void 	setADCStatus(int value);
	void 	updateADCStatus();
	void	setFramesPerSecond(QObject *sender, int rx, int value);
	void	setAGCMode(QObject *sender, int rx, AGCMode mode, bool hangEnabled);
	void 	setAGCLineLevels(QObject *sender, int rx, qreal thresh, qreal hang);
	void	setAGCLineFixedLevel(QObject *sender, int rx, qreal value);
	//void	setAGCHangEnabled(QObject *sender, int rx, bool hangEnabled);

signals:
	void	showEvent(QObject *sender);
	void	closeEvent(QObject *sender);
	void	messageEvent(QString msg);
	void	coordChanged(int x,int y);
};

#endif  // _CUSDR_QGL_RECEIVERPANEL_H
