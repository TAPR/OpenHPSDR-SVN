/**
* @file  cusdr_displayWidget.h
* @brief OpenGL display options widget header file for cuSDR
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
 
#ifndef _CUSDR_DISPLAY_OPTIONS_WIDGET_H
#define _CUSDR_DISPLAY_OPTIONS_WIDGET_H

//#include <QWidget>
//#include <QPainter>
//#include <QComboBox>
//#include <QGroupBox>
//#include <QSpinBox>
//#include <QLineEdit>

#include "Util/cusdr_buttons.h"
#include "cusdr_settings.h"
#include "cusdr_fonts.h"


#ifdef LOG_DISPLAYOPTIONS_WIDGET
#   define DISPLAYOPTIONS_DEBUG qDebug().nospace() << "DisplayWidget::\t"
#else
#   define DISPLAYOPTIONS_DEBUG nullDebug()
#endif


class DisplayOptionsWidget : public QWidget {

	Q_OBJECT

public:
	DisplayOptionsWidget(QWidget* parent = 0);
	~DisplayOptionsWidget();

public slots:
	QSize	sizeHint() const;
	QSize	minimumSizeHint() const;

private:
	Settings*					set;

	QSDR::_ServerMode			m_serverMode;
	QSDR::_HWInterfaceMode		m_hwInterface;
	QSDR::_DataEngineState		m_dataEngineState;

	PanGraphicsMode				m_panadapterMode;
	PanGraphicsMode				m_wbPanadapterMode;
	WaterfallColorMode			m_waterColorMode;

	QList<TReceiver>		m_rxDataList;
	TWideband				m_widebandOptions;
	
	QString					m_menu_style;
	QString					m_callSingText;

	CFonts*					fonts;
	TFonts					m_fonts;

	QGroupBox*				m_fpsGroupBox;
	QGroupBox*				m_panSpectrumOptions;
	QGroupBox*				m_widebandPanOptions;
	QGroupBox*				m_waterfallSpectrumOptions;
	QGroupBox*				m_wideBandSpectrumOptions;
	QGroupBox*				m_sMeterOptions;
	QGroupBox*				m_callSignEditor;

	QLineEdit*				callSignLineEdit;

	QSlider*				m_fpsSlider;
	QSlider*				m_avgSlider;
	QSlider*				m_wbAvgSlider;

	QSpinBox*				m_waterfallLoOffsetSpinBox;
	QSpinBox*				m_waterfallHiOffsetSpinBox;
	QSpinBox*				m_sMeterHoldTimeSpinBox;

	QLabel*					m_fpsLabel;
	QLabel*					m_fpsLevelLabel;
	QLabel*					m_avgLabel;
	QLabel*					m_wbAvgLabel;
	QLabel*					m_avgLevelLabel;
	QLabel*					m_wbAvgLevelLabel;
	QLabel*					m_resolutionLabel;
	QLabel*					m_waterfallTimeLabel;
	QLabel*					m_waterfallLoOffsetLabel;
	QLabel*					m_waterfallHiOffsetLabel;
	QLabel*					m_sMeterHoldTimeLabel;

	AeroButton*				m_PanLineBtn;
	AeroButton*				m_PanFilledLineBtn;
	AeroButton*				m_PanSolidBtn;
	AeroButton*				m_wbPanLineBtn;
	AeroButton*				m_wbPanFilledLineBtn;
	AeroButton*				m_wbPanSolidBtn;
	AeroButton*				m_setCallSignBtn;
	AeroButton*				m_waterfallSimpleBtn;
	AeroButton*				m_waterfallEnhancedBtn;
	AeroButton*				m_waterfallSpectranBtn;

	QList<AeroButton* >		m_panadapterBtnList;
	QList<AeroButton* >		m_wbpanadapterBtnList;
	QList<AeroButton* >		m_waterfallColorBtnList;
	
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
	int		m_avgValue;
	int		m_wbAvgValue;
	int		m_sampleRate;
	int		m_waterfallTime;
	int		m_sMeterHoldTime;

	void	setupConnections();
	void	createFPSGroupBox();
	void	createPanSpectrumOptions();
	void	createWidebandPanOptions();
	void	createWaterfallSpectrumOptions();
	void	createSMeterOptions();
	void	createCallSignEditor();

	void	setPanadapterMode(int rx);
	void	setWaterfallColorMode(int rx);

private slots:
	void	systemStateChanged(
					QObject *sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);

	void	graphicModeChanged(
					QObject *sender,
					int rx,
					PanGraphicsMode panMode,
					WaterfallColorMode waterfallColorMode);

	void	setCurrentReceiver(QObject *sender, int rx);
	void	setFramesPerSecond(QObject *sender, int rx, int value);
	void	panModeChanged();
	void	wbPanModeChanged();
	void	waterfallColorChanged();
	void	sMeterChanged();
	void	waterfallTimeChanged(int value);
	void	waterfallLoOffsetChanged(int value);
	void	waterfallHiOffsetChanged(int value);
	void	sMeterHoldTimeChanged(int value);
	void 	fpsValueChanged(int value);
	void	averagingFilterCntChanged(int value);
	void	setWidebandAveragingCnt(int value);
	void	sampleRateChanged(QObject *sender, int value);
	void	callSignTextChanged(const QString &text);
	void	callSignChanged();
	
signals:
	void	averagingModeChanged(bool value);
	//void	showEvent(QObject *sender);
	//void	closeEvent(QObject *sender);
};

#endif // _CUSDR_DISPLAY_OPTIONS_WIDGET_H
