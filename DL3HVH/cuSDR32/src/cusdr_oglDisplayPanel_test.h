/**
* @file  cusdr_oglDisplayPanel.h
* @brief header file for display panel
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

#ifndef _CUSDR_QGL_DISPLAYPANEL_H
#define _CUSDR_QGL_DISPLAYPANEL_H

#include "cusdr_oglUtils.h"
#include "cusdr_oglInfo.h"
#include "cusdr_settings.h"
#include "cusdr_oglText.h"

//#include <QPixmap>
//#include <QImage>
#include <QFontMetrics>
#include <QWheelEvent>
//#include <QQueue>
#include <QDebug>
#include <QtOpenGL/QGLWidget>

class OGLDisplayPanel : public QGLWidget {

    Q_OBJECT

public:
    OGLDisplayPanel(QWidget *parent = 0);
	~OGLDisplayPanel();

public slots:
	QSize minimumSizeHint() const;
	QSize sizeHint() const;

	void setSampleRate(QObject *sender, int value);
	void setFrequency(QObject *sender, bool value, int rx, long freq);

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

	QGLFramebufferObject		*m_smeterFBO;

	QList<TFrequency>	m_frequencyList;

	QFont		m_tinyFont;
	QFont		m_smallFont;
	QFont		m_normalFont;
	QFont		m_bigFont;
	QFont		m_freqFont1;
	QFont		m_freqFont2;
	QFont		m_impactFont;

	OGLText		*m_oglTextTiny;
	OGLText		*m_oglTextSmall;
	OGLText		*m_oglTextSmallItalic;
	OGLText		*m_oglTextNormal;
	OGLText		*m_oglTextBig;
	OGLText		*m_oglTextBigItalic;
	OGLText		*m_oglTextFreq1;
	OGLText		*m_oglTextFreq2;
	OGLText		*m_oglTextImpact;

	QRect		m_rxRect;
	QRect		m_smeterRect;
	
	QString		m_src10mhz;
	QString		m_src122_88mhz;
	QString		m_mercuryVersion;
	QString		m_penelopeVersion;
	QString		m_hermesVersion;
	QString		m_excaliburVersion;
	QString		m_metisVersion;
	QString		m_alexVersion;
	QString		m_mercuryString;
	QString		m_penelopeString;
	QString		m_excaliburString;
	QString		m_metisString;
	QString		m_alexString;
	QString		m_hermesString;

	QString		m_SYNCString;
	QString		m_ADCString;
	QString		m_sendIQString;
	QString		m_recvAudioString;
	QString		m_AttnString;
	QString		m_ditherString;
	QString		m_randomString;
	QString		m_sampleRateString;
	QString		m_modusString;
	QString		m_10MHzString;
	QString		m_12288MHzString;
	QString		m_sMeterNumValueString;

	QRegion		m_freg1;
	QRegion		m_freg10;
	QRegion		m_freg100;
	QRegion		m_freg1000;
	QRegion		m_freg10000;
	QRegion		m_freg100000;
	QRegion		m_freg1000000;
	QRegion		m_freg10000000;

	QColor		m_digitColor;
	QColor		m_bkgColor1;
	QColor		m_bkgColor2;
	QColor		m_activeTextColor;
	QColor		m_textBackgroundColor;

	QTime		m_sMeterTimer;
	QTime		m_sMeterMaxTimer;
	QTime		m_sMeterMinTimer;
	QTime		m_sMeterDisplayTime;

	enum Region {

		upperRegion,
		lowerRegion,
		rxRegion,
		smeterRegion,
		hpsdrRegion,
		elsewhere,
		out
	};

	enum FreqDigit {

		Freq1,
		Freq10,
		Freq100,
		Freq1000,
		Freq10000,
		Freq100000,
		Freq1000000,
		Freq10000000,
		None
	};

	GLuint	m_sMeterTex;

	bool	m_mercury;
	bool	m_penelope;
	bool	m_excalibur;
	bool	m_metis;
	bool	m_alex;
	bool	m_smeterUpdate;
	bool	m_smeterRenew;
	bool	m_SMeterA;
	bool	m_sMeterAvg;

	int		m_height;
	int		m_sMeterWidth;
	int		m_sMeterOffset;
	int		m_rxRectWidth;
	int		m_lowerRectY;
	int		m_upperRectY;
	int		m_digitPosition;
	int		m_syncStatus;
	int		m_adcStatus;
	int		m_sendIQStatus;
	int		m_recvAudioStatus;
	int		m_receivers;
	int		m_sample_rate;
	int		m_preamp;
	int		m_dither;
	int		m_random;
	int		m_currentReceiver;

	int		m_syncWidth;
	int		m_adcWidth;
	int		m_sendIQWidth;
	int		m_recvAudioWidth;
	int		m_AttnWidth;
	int		m_ditherWidth;
	int		m_randomWidth;
	int		m_sampleRateWidth;
	int		m_modusWidth;
	int		m_10MHzWidth;
	int		m_sMeterDeform;
	int		m_12288MHzWidth;
	int		m_freqDigitsPosY;
	int		m_sMeterPosY;
	int		m_sMeterHoldTime;
	
	qreal	m_mouseWheelFreqStep;
	
	float	m_smeterVertices;
	float	m_sMeterValue;
	float	m_sMeterOrgValue;
	float	m_sMeterMaxValueA;
	float	m_sMeterMinValueA;
	float	m_sMeterMaxValueB;
	float	m_sMeterMinValueB;

	//*************************
	void	setupConnections();
	void	setupTextstrings();
	void	paintUpperRegion();
	void	paintLowerRegion();
	void	paintRxRegion();
	
	void	paintSMeter();
	void	renderSMeterA();
	void	renderSMeterB();
	
	void	getSelectedDigit(QPoint p);
	
private slots:
	void	systemStateChanged(
				QObject *sender, 
				QSDR::_Error err, 
				QSDR::_HWInterfaceMode hwmode, 
				QSDR::_ServerMode mode, 
				QSDR::_DataEngineState state);

	void	setupDisplayRegions(QSize size);

	void	setSyncStatus(int value);
	void	setADCStatus(int value);
	void	setSendIQStatus(int value);
	void	setRecvAudioStatus(int value);
	void	setCurrentReceiver(int value);
	void	setReceivers(QObject *sender, int value);
	void	setPreamp(QObject *sender, int value);
	void	setDither(QObject *sender, int value);
	void	setRandom(QObject *sender, int value);
	void	set10mhzSource(QObject *sender, int value);
	void	set122_88mhzSource(QObject *sender, int value);

	void	setMercuryPresence(bool value);
	void	setPenelopePresence(bool value);
	void	setAlexPresence(bool value);
	void	setExcaliburPresence(bool value);
	void	setMercuryVersion(int value);
	void	setPenelopeVersion(int value);
	void	setHWInterfaceVersion(int value);
	void	setExcaliburVersion(QObject *sender, int value);
	void	setAlexVersion(QObject *sender, int value);

	void	setMouseWheelFreqStep(QObject *sender, double value);

	void	setSMeterValue(int rx, float value);
	void	setSMeterHoldTime(int value);

signals:
	void showEvent(QObject *sender);
	void closeEvent(QObject *sender);
	void messageEvent(QString msg);
};


#endif // _CUSDR_QGL_DISPLAYPANEL_H