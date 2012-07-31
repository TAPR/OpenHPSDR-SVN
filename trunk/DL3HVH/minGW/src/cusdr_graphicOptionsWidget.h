/**
* @file  cusdr_graphicOptionsWidget.h
* @brief OpenGL graphics options widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-08-19
*/

/*
 *   
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
 
#ifndef _CUSDR_GRAPHIC_OPTIONS_WIDGET_H
#define _CUSDR_GRAPHIC_OPTIONS_WIDGET_H

#include <QWidget>
#include <QPainter>
#include <QComboBox>
#include <QGroupBox>
#include <QSpinBox>
#include <QLineEdit>

#include "cusdr_buttons.h"
#include "cusdr_settings.h"
#include "cusdr_colorTriangle.h"


class GraphicOptionsWidget : public QWidget {

	Q_OBJECT

public:
	GraphicOptionsWidget(QWidget *parent = 0);
	~GraphicOptionsWidget();

public slots:
	QSize	sizeHint() const;
	QSize	minimumSizeHint() const;

private:
	Settings*	m_settings;

	QSDR::_ServerMode						m_serverMode;
	QSDR::_HWInterfaceMode					m_hwInterface;
	QSDR::_DataEngineState					m_dataEngineState;
	QSDRGraphics::_Panadapter				m_panadapterMode;
	QSDRGraphics::_WaterfallColorScheme		m_waterColorScheme;

	QtColorTriangle*	m_colorTriangle;
	QList<TReceiver>	m_rxDataList;
	
	QString			m_menu_style;
	QFont			m_titleFont;
	QFont			m_smallFont;

	//QString			lineedit_style;
	QString			m_callSingText;

	QColor			m_currentColor;
	QColor			m_newColor;

	QGroupBox*		m_panSpectrumOptions;
	QGroupBox*		m_waterfallSpectrumOptions;
	QGroupBox*		m_wideBandSpectrumOptions;
	QGroupBox*		m_sMeterOptions;
	QGroupBox*		m_colorChooserWidget;
	QGroupBox*		m_callSignEditor;

	QLineEdit*		callSignLineEdit;
		
	//QComboBox*      m_inputDeviceComboBox;
    //QComboBox*      m_outputDeviceComboBox;

	QSpinBox*		m_averagingFilterSpinBox;
	QSpinBox*		m_framesPerSecondSpinBox;
	QSpinBox*		m_waterfallTimeSpinBox;
	QSpinBox*		m_waterfallLoOffsetSpinBox;
	QSpinBox*		m_waterfallHiOffsetSpinBox;
	QSpinBox*		m_sMeterHoldTimeSpinBox;

	QLabel*			m_framePerSecondsLabel;
	QLabel*			m_averagingFilterLabel;
	QLabel*			m_resolutionLabel;
	QLabel*			m_waterfallTimeLabel;
	QLabel*			m_waterfallLoOffsetLabel;
	QLabel*			m_waterfallHiOffsetLabel;
	QLabel*			m_sMeterHoldTimeLabel;

	AeroButton*		m_PanLineBtn;
	AeroButton*		m_PanFilledLineBtn;
	AeroButton*		m_PanSolidBtn;
	AeroButton*		m_resetBtn;
	AeroButton*		m_okBtn;

	AeroButton*		m_res1Btn;
	AeroButton*		m_res2Btn;
	AeroButton*		m_res4Btn;

	AeroButton*		m_avgBtn;

	AeroButton*		m_setPanBackground;
	AeroButton*		m_setPanCenterLine;
	AeroButton*		m_setPanLine;
	AeroButton*		m_setPanLineFilling;
	AeroButton*		m_setPanSolidTop;
	AeroButton*		m_setPanSolidBottom;
	AeroButton*		m_setWaterfall;
	AeroButton*		m_setWideBandLine;
	AeroButton*		m_setWideBandFilling;
	AeroButton*		m_setWideBandSolidTop;
	AeroButton*		m_setWideBandSolidBottom;
	AeroButton*		m_setDistanceLine;
	AeroButton*		m_setDistanceLineFilling;
	AeroButton*		m_setCallSignBtn;

	AeroButton*		m_waterfallSimpleBtn;
	AeroButton*		m_waterfallEnhancedBtn;
	AeroButton*		m_waterfallSpectranBtn;

	AeroButton*		m_sMeterA;
	AeroButton*		m_sMeterB;

	QList<AeroButton *>		m_panadapterBtnList;
	QList<AeroButton *>		m_changeColorBtnList;
	QList<AeroButton *>		m_changeResolutionBtnList;
	QList<AeroButton *>		m_waterfallSchemeBtnList;

	TPanadapterColors		m_panadapterColors;
	TPanadapterColors		m_oldPanadapterColors;
	
	int		m_fontHeight;
	int		m_maxFontWidth;

	bool	m_antialiased;
	bool	m_mouseOver;
	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;
	int		m_btnSpacing;

	int		m_currentReceiver;
	int		m_btnChooserHit;
	int		m_panStyle;
	int		m_framesPerSecond;
	int		m_graphicResolution;
	int		m_sampleRate;
	int		m_waterfallTime;
	int		m_sMeterHoldTime;

	void	setupConnections();
	void	createPanSpectrumOptions();
	void	createWaterfallSpectrumOptions();
	void	createSMeterOptions();
	void	createColorChooserWidget();
	void	createCallSignEditor();

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
					QSDRGraphics::_WaterfallColorScheme waterColorScheme);

	void	setCurrentReceiver(int rx);
	void	panModeChanged();
	void	waterfallSchemeChanged();
	void	sMeterChanged();
	void	resolutionModeChanged();
	void	setAveragingMode();
	void	colorChooserChanged();
	void	waterfallTimeChanged(int value);
	void	waterfallLoOffsetChanged(int value);
	void	waterfallHiOffsetChanged(int value);
	void	sMeterHoldTimeChanged(int value);
	void	resetColors();
	void	acceptColors();
	void	triangleColorChanged(QColor color);
	void 	fpsValueChanged(int value);
	void	averagingFilterCntChanged(int value);
	void	sampleRateChanged(QObject *sender, int value);
	void	callSignTextChanged(const QString &text);
	void	callSignChanged();
	
signals:
	void	colorChanged(const QColor &color);
	void	averagingModeChanged(bool value);
	void	showEvent(QObject *sender);
	void	closeEvent(QObject *sender);
	void	messageEvent(QString );
};

#endif // _CUSDR_GRAPHIC_OPTIONS_WIDGET_H
