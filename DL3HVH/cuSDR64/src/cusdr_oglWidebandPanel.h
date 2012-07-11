/**
* @file  cusdr_oglWidebandPanel.h
* @brief wide band spectrum panel header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-02-11
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

#ifndef _CUSDR_OGL_WIDEBANDPANEL_H
#define _CUSDR_OGL_WIDEBANDPANEL_H

#include "cusdr_oglUtils.h"
#include "cusdr_oglInfo.h"
#include "cusdr_settings.h"
#include "cusdr_oglText.h"

#include <QPixmap>
#include <QImage>
#include <QFontMetrics>
#include <QWheelEvent>
#include <QQueue>
#include <QDebug>
#include <QtOpenGL/QGLWidget>


class QGLWidebandPanel : public QGLWidget {

    Q_OBJECT

public:
    QGLWidebandPanel(QWidget *parent = 0);
	~QGLWidebandPanel();

public slots:
	//QSize minimumSizeHint() const;
	QSize sizeHint() const;

	//void setSpectrumBuffer(const float *buffer);
	//void setFrequency(QObject *sender, bool value, long freq);

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

	QSDR::_ServerMode					m_serverMode;
	QSDR::_HWInterfaceMode				m_hwInterface;
	QSDR::_DataEngineState				m_dataEngineState;

	QSDRGraphics::_Panadapter			m_panadapterMode;
	QSDRGraphics::_WaterfallColorScheme m_waterColorScheme;

	QVector<qreal>						m_widebandPanSpectrumBins;
	QQueue<QVector<float>>				specAv_queue;
	QList<TReceiver>					m_rxDataList;

	QGLFramebufferObject				*m_frequencyScaleFBO;
	QGLFramebufferObject				*m_dBmScaleFBO;
	QGLFramebufferObject				*m_gridFBO;

	QTime		m_panTimer;
	QTime		m_displayTime;
	QTime		m_resizeTime;
	
	QRect		m_panRect;
	QRect		m_freqScaleRect;
	QRect		m_dBmScaleRect;
	
	QMutex		mutex;
	
	QFont		m_tinyFont;
	QFont		m_smallFont;
	QFont		m_normalFont;

	OGLText*	m_oglTextTiny;
	OGLText*	m_oglTextSmall;
	OGLText*	m_oglTextNormal;

	QPoint		m_mousePos;
	QPoint		m_oldMousePos;
	QPoint		m_mouseLastPos;
	QPoint		m_mouseDownPos;
	QPoint		m_rulerMouseDownPos;
	QPoint		m_cameraAngle;

	TScale		m_frequencyScale;
	TScale		m_dBmScale;

	GLfloat		m_r;
	GLfloat		m_g;
	GLfloat		m_b;

	GLfloat		m_rf;
	GLfloat		m_gf;
	GLfloat		m_bf;

	GLfloat		m_redST;
	GLfloat		m_greenST;
	GLfloat		m_blueST;

	GLfloat		m_redSB;
	GLfloat		m_greenSB;
	GLfloat		m_blueSB;

	GLfloat		m_bkgRed;
	GLfloat		m_bkgGreen;
	GLfloat		m_bkgBlue;

	enum Region {

		panRegion,
		freqScaleRegion,
		dBmScaleRegion,
		elsewhere,
		out
	};

	long		m_frequency;

	qreal		m_dBmPanMin;
	qreal		m_dBmPanMax;

	qVectorFloat m_wbSpectrumBuffer;

	float		m_scale;
	float		m_distMax;

	bool		m_spectrumUpdate;
	bool		m_freqScaleUpdate;
	bool		m_freqScaleRenew;
	bool		m_dBmScaleUpdate;
	bool		m_dBmScaleRenew;
	bool		m_panGridUpdate;
	bool		m_panGridRenew;
	bool		m_spectrumColorsChanged;
	bool		m_spectrumVertexColorUpdate;
	bool		m_crossHairCursor;
	bool		m_panGrid;

	int			m_receiver;
	int			m_oldWidth;
	int			m_oldHeight;
	int			m_oldPanRectHeight;
	int			m_smallFontHeight;
	int			m_cnt;
	int			m_specAveragingCnt;
	int			m_Preamp;
	
	float		m_cameraDistance;

	unsigned int timer;
    
	GLint		m_panRectWidth;
	GLint		m_panSpectrumBinsLength;
	
	int			m_mouseRegion;
	int			m_oldMouseRegion;
	int			m_snapMouse;

	int			m_currentReceiver;
	int			m_freqScaleWidth;
	int			m_displayTop;
	int			m_dBmPanLogGain;
	int			m_panDisplayMode;
	int			m_sampleRate;
	int			m_downRate;
	
	float		m_freqScalePosition;
	float		m_freqScaleZoomFactor;
	
	double		m_displayDelta;

	qreal		m_dBmPanDelta;
	qreal		m_panScale;
	qreal		m_scaleMultOld;
	qreal		m_panFrequencyScale;
	qreal		m_frequencySpan;
	qreal		m_frequencyUnit;
	
	//******************************************************************
	void saveGLState();
	void restoreGLState();
	//void computeDisplayBins(const float* panBuffer);

	void drawSpectrum();
	void drawVerticalScale();
	void drawHorizontalScale();
	void drawGrid();
	void drawCrossHair();
	void drawHamBand(int lo, int hi, const QString &band);

	void renderVerticalScale();
	void renderHorizontalScale();
	void renderGrid();

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
					QSDRGraphics::_WaterfallColorScheme colorScheme);

	void	setupConnections();
	void	setCurrentReceiver(int value);
	void	setFrequency(QObject *sender, bool value, int rx, long freq);
	void	setupDisplayRegions(QSize size);
	void	setWidebandSpectrumBuffer(const qVectorFloat &buffer);
	void	resetWidebandSpectrumBuffer();
	//void	setSpectrumAveragingCnt(int value);
	void	setPanadapterColors();
	void	setPanGridStatus(bool value);
	void	setPreamp(QObject* sender, int value);

	void	getRegion(QPoint p);
	void	sampleRateChanged(QObject* sender, int value);
	void	freqScaleUpdate(bool value);
	void	freqScaleRenew(bool value);
	void	dBmScaleUpdate(bool value);
	void	dBmScaleRenew(bool value);
	void	panGridUpdate(bool value);
	void	panGridRenew(bool value);

signals:
	void showEvent(QObject* sender);
	void closeEvent(QObject* sender);
	void messageEvent(QString msg);
	void coordChanged(int x, int y);
};

#endif  // _CUSDR_OGL_WIDEBANDPANEL_H