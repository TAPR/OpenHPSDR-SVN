/**
* @file  cusdr_agcWidget.h
* @brief AGC options widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-09-29
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
 
#ifndef _CUSDR_AGC_OPTIONS_WIDGET_H
#define _CUSDR_AGC_OPTIONS_WIDGET_H

//#include <QWidget>
//#include <QPainter>
//#include <QComboBox>
//#include <QGroupBox>
//#include <QSpinBox>
//#include <QLineEdit>

#include "Util/cusdr_buttons.h"
#include "cusdr_settings.h"
#include "cusdr_fonts.h"


class AGCOptionsWidget : public QWidget {

	Q_OBJECT

public:
	AGCOptionsWidget(QWidget *parent = 0);
	~AGCOptionsWidget();

public slots:
	QSize	sizeHint() const;
	QSize	minimumSizeHint() const;

private:
	Settings	*set;

	QSDR::_ServerMode			m_serverMode;
	QSDR::_HWInterfaceMode		m_hwInterface;
	QSDR::_DataEngineState		m_dataEngineState;
	//QSDRGraphics::_Panadapter	m_panadapterMode;
	//QSDRGraphics::_WfScheme		m_waterColorScheme;

	AGCMode				m_agcMode;

	QList<TReceiver>	m_rxDataList;

	CFonts		*fonts;
	TFonts		m_fonts;

	QString			m_menu_style;
	QString			m_callSingText;

	QGroupBox	*agcModeGroupBox;
	QGroupBox	*agcOptionsGroupBox;

	AeroButton	*agcOFF;
	AeroButton	*agcLONG;
	AeroButton	*agcSLOW;
	AeroButton	*agcMED;
	AeroButton	*agcFAST;
	AeroButton	*agcUSER;

	QList<AeroButton *>	agcModeBtnList;

	QLineEdit		*callSignLineEdit;

	QSlider			*m_hangThresholdSlider;

	QSpinBox		*m_slopeSpinBox;
	QSpinBox		*m_maxGainSpinBox;
	QSpinBox		*m_attackTimeSpinBox;
	QSpinBox		*m_decayTimeSpinBox;
	QSpinBox		*m_hangTimeSpinBox;
	QSpinBox		*m_fixedGainSpinBox;

	QLabel			*m_slopeLabel;
	QLabel			*m_maxGainLabel;
	QLabel			*m_attackTimeLabel;
	QLabel			*m_decayTimeLabel;
	QLabel			*m_hangTimeLabel;
	QLabel			*m_fixedGainLabel;
	QLabel			*m_hangThresholdLabel;
	QLabel			*m_hangThresholdValueLabel;

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;
	int		m_btnSpacing;
	int		m_fontHeight;
	int		m_maxFontWidth;
	int		m_currentReceiver;
	int		m_sampleRate;
	int		m_agcHangThreshold;

	bool	m_mouseOver;

	//qreal	m_agcSlope;
	qreal	m_agcMaxGain;
	qreal	m_agcAttackTime;
	qreal	m_agcDecayTime;
	qreal	m_agcHangTime;
	qreal	m_agcFixedGain;



	void	setupConnections();
	void 	createAgcModeBtnGroup();
	void 	createAgcOptionsGroup();
//	void	createFPSGroupBox();
//	void	createPanSpectrumOptions();
//	void	createWaterfallSpectrumOptions();
//	void	createSMeterOptions();
//	void	createColorChooserWidget();
//	void	createCallSignEditor();

private slots:
	void	systemStateChanged(
					QObject *sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);

	void	setCurrentReceiver(QObject *sender, int rx);
	void	sampleRateChanged(QObject *sender, int value);
	void 	agcModeChanged(QObject *sender, int rx, AGCMode mode, bool value);
	void 	agcModeChangedByBtn();
	void	slopeChanged(int value);
	void	maxGainChanged(int value);
	void	attackTimeChanged(int value);
	void	decayTimeChanged(int value);
	void	hangTimeChanged(int value);
	void	fixedGainChanged(int value);
	void	hangThresholdValueChanged(int value);
	void	setAGCHangThresholdSlider(QObject *sender, int rx, qreal value);
	void	setAGCMaximumGain_dB(QObject *sender, int rx, qreal value);
	void	setAGCFixedGain_dB(QObject *sender, int rx, qreal value);
	
signals:
	void	showEvent(QObject *sender);
	void	closeEvent(QObject *sender);
	void	messageEvent(QString );
};

#endif // _CUSDR_AGC_OPTIONS_WIDGET_H
