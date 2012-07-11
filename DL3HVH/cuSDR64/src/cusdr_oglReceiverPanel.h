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
//#include "cusdr_oglTextRenderer.h"
#include "cusdr_oglText.h"

#include <QPixmap>
#include <QImage>
#include <QFontMetrics>
#include <QWheelEvent>
#include <QQueue>
#include <QDebug>
#include <QtOpenGL/QGLWidget>


class QGLReceiverPanel : public QGLWidget {

    Q_OBJECT

public:
    QGLReceiverPanel(QWidget *parent = 0, int rx = 0);
	~QGLReceiverPanel();

public slots:
	//QSize minimumSizeHint() const;
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
	void mouseReleaseEvent(QMouseEvent *event);
	void mouseMoveEvent(QMouseEvent *event);
	void wheelEvent(QWheelEvent * event );
	void keyPressEvent(QKeyEvent* event);
	void closeEvent(QCloseEvent *event);
	void showEvent(QShowEvent *event);
	void timerEvent(QTimerEvent *);

private:
	Settings*	m_settings;

	QSDR::_ServerMode			m_serverMode;
	QSDR::_HWInterfaceMode		m_hwInterface;
	QSDR::_DataEngineState		m_dataEngineState;

	QSDRGraphics::_Panadapter			m_panadapterMode;
	QSDRGraphics::_WaterfallColorScheme m_waterColorScheme;

	QTime			m_displayTime;
	QTime			m_resizeTime;
	QTime			waterfallTimer;
	QTime			freqChangeTimer;
	
	//QGLWidget		*m_shareWidget;

	TScale			m_frequencyScale;
	TScale			m_dBmScale;
	
	QList<TReceiver>	m_rxDataList;
	
	QVector<qreal>					m_panadapterBins;
	QVarLengthArray<TGL_ubyteRGBA>	m_waterfallPixel;

	QQueue<QVector<float>>			specAv_queue;

	QGLFramebufferObject			*m_frequencyScaleFBO;
	QGLFramebufferObject			*m_dBmScaleFBO;
	QGLFramebufferObject			*m_panadapterGridFBO;
	QGLFramebufferObject			*m_textureFBO;
	QGLFramebufferObject			*m_waterfallLineFBO;
	QGLFramebufferObject			*m_waterfallFBO;

	QRect		m_panRect;
	QRect		m_dBmScalePanRect;
	QRect		m_freqScalePanRect;
	QRect		m_waterfallRect;
	QRect		m_filterRect;
	
	QFont		m_tinyFont;
	QFont		m_smallFont;
	QFont		m_normalFont;
	QFont		m_freqFont1;
	QFont		m_freqFont2;

	OGLText		*m_oglTextTiny;
	OGLText		*m_oglTextSmall;
	OGLText		*m_oglTextNormal;
	OGLText		*m_oglTextFreq1;
	OGLText		*m_oglTextFreq2;

	QPoint		m_mousePos;
	QPoint		m_oldMousePos;
	QPoint		m_mouseLastPos;
	QPoint		m_mouseDownPos;
	QPoint		m_rulerMouseDownPos;
	QPoint		m_cameraAngle;

	QColor		m_waterfallLoColor;
	QColor		m_waterfallHiColor;
	QColor		m_waterfallMidColor;

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
		elsewhere,
		out
	};
    
	GLint		m_panRectWidth;
	GLint		m_panSpectrumBinsLength;

	GLint		m_filterLeft;
	GLint		m_filterRight;
	GLint		m_filterTop;
	GLint		m_filterBottom;

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
	
	unsigned int timer;

	int			m_receiver;
	int			m_frequencyRxOnRx;
	int			m_spectrumSize;
	int			m_oldWidth;
	int			m_oldPanRectHeight;
	int			m_smallFontHeight;
	int			m_cnt;
	int			m_specAveragingCnt;
	int			m_waterfallTime;
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
	int			m_snapMouse;
	int			m_panDisplayMode;
	int			m_sampleRate;
	int			m_downRate;
	int			m_Preamp;
	int			m_haircrossOffsetRight;
	int			m_haircrossOffsetLeft;
	int			m_haircrossMaxRight;
	int			m_haircrossMinTop;
	int			m_displayCenterlineHeight;
	int			m_waterfallLineCnt;

	long		m_frequency;
	long		m_otherFrequency;
	long		m_oldFreq;

	bool		m_spectrumVertexColorUpdate;
	bool		m_dBmScalePanadapterRenew;
	bool		m_dBmScalePanadapterUpdate;
	bool		m_freqScalePanadapterRenew;
	bool		m_freqScalePanadapterUpdate;
	bool		m_panGridRenew;
	bool		m_panGridUpdate;
	bool		m_waterfallUpdate;
	bool		m_distRulerUpdate;
	bool		m_newWidebandPanFreqRuler;
	bool		m_spectrumColorsChanged;
	bool		m_spectrumAveraging;
	bool		m_spectrumAveragingOld;
	bool		m_crossHairCursor;
	bool		m_panGrid;
	bool		m_filterChanged;
	bool		m_showFilterLeftBoundary;
	bool		m_showFilterRightBoundary;
	bool		m_highlightFilter;
	
	qreal		m_panFrequencyScale;
	qreal		m_freqScaleZoomFactor;
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
	qreal		m_filterLo;
	qreal		m_filterHi;

	float		m_scale;
	float		m_cameraDistance;
	float		m_freqRulerPosition;
	float		m_spectrumBuffer[4*BUFFER_SIZE];
	
	QVector<float>	m_tmp;
	

	//******************************************************************
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
	void	drawCrossHair();
	void 	drawReceiverInfo();
	void	drawAGCControl();

	void 	renderPanVerticalScale();
	void 	renderPanHorizontalScale();
	void 	renderPanadapterGrid();

	//void	computeDisplayBins(const float *panBuffer, const float *waterfallBuffer);
	void	computeDisplayBins(const QVector<float> &panBuffer, const float *waterfallBuffer);

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
					QSDRGraphics::_WaterfallColorScheme colorScheme);

	void	setupConnections();
	void	setCurrentReceiver(int value);
	void	setCoupledRx(QObject* sender, int value);
	void	setFilterFrequencies(QObject *sender, int rx, qreal lo, qreal hi);
	void	setPreamp(QObject* sender, int value);
	void	setupDisplayRegions(QSize size);
	
	void	setSpectrumAveraging(bool value);
	void	setSpectrumAveragingCnt(int value);
	void	setPanGridStatus(bool value);
	void	setPanadapterColors();
	void	getRegion(QPoint p);
	void	freqRulerPositionChanged(float pos, int rx);
	void	sampleRateChanged(QObject *sender, int value);
	void	setWaterfallTime(int rx, int value);
	void	setWaterfallOffesetLo(int rx, int value);
	void	setWaterfallOffesetHi(int rx, int value);

signals:
	void	showEvent(QObject *sender);
	void	closeEvent(QObject *sender);
	void	messageEvent(QString msg);
	void	coordChanged(int x,int y);
};

#endif  // _CUSDR_QGL_RECEIVERPANEL_H