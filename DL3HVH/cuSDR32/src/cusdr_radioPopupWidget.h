/**
* @file cusdr_radioPopupWidget.h
* @brief Radio control popup widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-08-22
*/

/*
 *   Copyright 2010-2012 Hermann von Hasseln, DL3HVH
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
 
#ifndef _CUSDR_RADIO_POPUP_WIDGET_H
#define _CUSDR_RADIO_POPUP_WIDGET_H

#ifdef LOG_RADIOPOPUP
#define RADIOPOPUP_DEBUG qDebug().nospace() << "RadioPopup::\t"
#else
#define RADIOPOPUP_DEBUG nullDebug()
#endif

//#include <QWidget>
//#include <QGroupBox>
//#include <QLineEdit>

#include "Util/cusdr_buttons.h"
#include "cusdr_settings.h"
#include "cusdr_fonts.h"

class RadioPopupWidget : public QWidget {

	Q_OBJECT

public:
	RadioPopupWidget(QWidget *parent = 0, int rx = 0);
	~RadioPopupWidget();

	bool getSpectrumAveraging() { return m_spectrumAveraging; }
	bool getPanGridStatus()		{ return m_panGrid; }
	bool getPeakHoldStatus()	{ return m_peakHold; }

public slots:
	//QSize	sizeHint() const;
	QSize	minimumSizeHint() const;

	void systemStateChanged(
			QObject *sender, 
			QSDR::_Error err, 
			QSDR::_HWInterfaceMode hwmode, 
			QSDR::_ServerMode mode, 
			QSDR::_DataEngineState state);

	bool showPopupWidget(QObject *sender, QPoint position);
		
protected:
	void showEvent(QShowEvent *event);
	void hideEvent(QHideEvent *event);
	void closeEvent(QCloseEvent *event);
	void paintEvent(QPaintEvent *event);
	void resizeEvent(QResizeEvent *event);
	void mousePressEvent(QMouseEvent *event);
	void mouseMoveEvent(QMouseEvent *event);
	void mouseReleaseEvent(QMouseEvent *event);
	void enterEvent(QEvent *event);
	void leaveEvent(QEvent *event);
	void timerEvent(QTimerEvent *event);
	bool event(QEvent *event);

private:
	Settings*				set;

	QTime					m_closingTimer;

	CFonts*					fonts;
	TFonts					m_fonts;

	PanGraphicsMode			m_panadapterMode;
	WaterfallColorMode		m_waterfallColorMode;

	QVBoxLayout*	optionsVBox;
	QVBoxLayout*	bandVBox;
	QVBoxLayout*	modeVBox;
	QVBoxLayout*	agcVBox;
	//QVBoxLayout*	mercuryBtnVBox();

	QWidget*		filterAWidget;
	QWidget*		filterBWidget;
	QWidget*		filterCWidget;

	QStackedLayout*	m_filterStackedLayout;
	QStackedWidget*	m_filterStackedWidget;

	QList<AeroButton *>	filterBtnListA;
	QList<AeroButton *>	filterBtnListB;
	QList<AeroButton *>	filterBtnListC;
	QList<AeroButton *>	panadapterBtnList;
	QList<AeroButton *>	waterfallBtnList;
	
	QLabel*			m_optionsLabel;

	AeroButton*		stickyBtn;

	AeroButton*		lockPanBtn;
	AeroButton*		clickVfoBtn;
	AeroButton*		showCrossBtn;
	AeroButton*		midToVfoBtn;
	AeroButton*		vfoToMidBtn;
	AeroButton*		avgBtn;
	AeroButton*		peakHoldBtn;
	AeroButton*		gridBtn;

	AeroButton*		m_PanLineBtn;
	AeroButton*		m_PanFilledLineBtn;
	AeroButton*		m_PanSolidBtn;
	AeroButton*		m_WaterfallSimpleBtn;
	AeroButton*		m_WaterfallEnhancedBtn;

	AeroButton*		band160mBtn;
	AeroButton*		band80mBtn;
	AeroButton*		band60mBtn;
	AeroButton*		band40mBtn;
	AeroButton*		band30mBtn;
	AeroButton*		band20mBtn;
	AeroButton*		band17mBtn;
	AeroButton*		band15mBtn;
	AeroButton*		band12mBtn;
	AeroButton*		band10mBtn;
	AeroButton*		band6mBtn;
	AeroButton*		bandGenBtn;
	//AeroButton*	bandxxBtn;

	QList<AeroButton *>	bandBtnList;

	AeroButton*		lsbBtn;
	AeroButton*		usbBtn;
	AeroButton*		dsbBtn;
	AeroButton*		cwlBtn;
	AeroButton*		cwuBtn;
	AeroButton*		fmnBtn;
	AeroButton*		amBtn;
	AeroButton*		diguBtn;
	AeroButton*		specBtn;
	AeroButton*		diglBtn;
	AeroButton*		samBtn;
	AeroButton*		drmBtn;

	QList<AeroButton *>	dspModeBtnList;

	AeroButton*		showAGCLines;
	AeroButton*		agcOFF;
	AeroButton*		agcLONG;
	AeroButton*		agcSLOW;
	AeroButton*		agcMED;
	AeroButton*		agcFAST;
	AeroButton*		agcUSER;

	QList<AeroButton *>	agcModeBtnList;
	
	AeroButton*		filter1kBtnA;
	AeroButton*		filter1k8BtnA;
	AeroButton*		filter2k1BtnA;
	AeroButton*		filter2k4BtnA;
	AeroButton*		filter2k7BtnA;
	AeroButton*		filter2k9BtnA;
	AeroButton*		filter3k3BtnA;
	AeroButton*		filter3k8BtnA;
	AeroButton*		filter4k4BtnA;
	AeroButton*		filter5kBtnA;
	AeroButton*		filterVar1BtnA;
	AeroButton*		filterVar2BtnA;

	AeroButton*		filter2k4BtnB;
	AeroButton*		filter2k9BtnB;
	AeroButton*		filter3k1BtnB;
	AeroButton*		filter4kBtnB;
	AeroButton*		filter5k2BtnB;
	AeroButton*		filter6k6BtnB;
	AeroButton*		filter8kBtnB;
	AeroButton*		filter10kBtnB;
	AeroButton*		filter12kBtnB;
	AeroButton*		filter16kBtnB;
	AeroButton*		filterVar1BtnB;
	AeroButton*		filterVar2BtnB;

	AeroButton*		filter25BtnC;
	AeroButton*		filter50BtnC;
	AeroButton*		filter100BtnC;
	AeroButton*		filter250BtnC;
	AeroButton*		filter400BtnC;
	AeroButton*		filter500BtnC;
	AeroButton*		filter600BtnC;
	AeroButton*		filter750BtnC;
	AeroButton*		filter800BtnC;
	AeroButton*		filter1kBtnC;
	AeroButton*		filterVar1BtnC;
	AeroButton*		filterVar2BtnC;
	
	QLabel*			m_rxLabel;

	//QIcon	agc_left;
	//QIcon	agc_right;

	QList<TReceiver>	m_receiverDataList;
	QList<DSPMode>		m_dspModeList;

	HamBand				m_hamBand;
	//DSPMode				m_dspMode;
	AGCMode				m_agcMode;
	TDefaultFilterMode	m_filterMode;

	QPoint				m_mouseDownPos;
	QPoint				m_mouseDownWindowPos;

	bool				m_mouseOver;

	//QList<long>			m_lastFrequencyList;
	QList<long>			m_lastCtrFrequencyList;
	QList<long>			m_lastVfoFrequencyList;

	long	m_ctrFrequency;
	long	m_vfoFrequency;

	bool	m_sticky;
	bool	m_spectrumAveraging;
	bool	m_panGrid;
	bool	m_peakHold;
	bool	m_panLocked;
	bool	m_clickVFO;
	bool	m_showCross;

	qreal	m_filterLo;
	qreal	m_filterHi;

	int		m_timerID;
	int		m_receiver;
	int		m_currentRx;
	int		current_band;
	int		current_dsp_mode;
	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;

	void setupConnections();
	void createBackground(QSize size);

private slots:
	void	graphicModeChanged(
					QObject* sender,
					int rx,
					PanGraphicsMode panMode,
					WaterfallColorMode waterfallColorMode);

	void setSticky();
	void createOptionsBtnGroup();
	void createBandBtnGroup();
	void createModeBtnGroup();
	void createAgcBtnGroup();
	void createFilterBtnWidgetA();
	void createFilterBtnWidgetB();
	void createFilterBtnWidgetC();

	//QLabel *createLabel(const QString &text);

	void avgBtnClicked();
	void gridBtnClicked();
	void peakHoldBtnClicked();
	void panLockedBtnClicked();
	void clickVfoBtnClicked();
	void hairCrossBtnClicked();
	void midToVfoBtnClicked();
	void vfoToMidBtnClicked();
	void panModeChanged();
	void waterfallModeChanged();

	void setCurrentReceiver(QObject *sender, int value);
	void ctrFrequencyChanged(QObject* sender, int mode, int rx, long frequency);
	void vfoFrequencyChanged(QObject* sender, int mode, int rx, long frequency);
	void bandChangedByBtn();
	void bandChanged(QObject *sender, int rx, bool byButton, HamBand band);
	void dspModeChangedByBtn();
	void dspModeChanged(QObject *sender, int rx, DSPMode mode);
	void agcModeChangedByBtn();
	void agcModeChanged(QObject *sender, int rx, AGCMode mode, bool hang);
	void agcShowLinesChanged();
	void filterChangedByBtn();
	void filterChanged(QObject *sender, int rx, qreal low, qreal high);
	void filterGroupChanged(DSPMode mode);
	
signals:
	void showEvent(QObject *sender);
	void hideEvent(QObject *sender);
	void closeEvent(QObject *sender);
	void newMessage(QString msg);
	void midToVfoBtnEvent();
	void vfoToMidBtnEvent();
};

#endif // _CUSDR_RADIO_POPUP_WIDGET_H
