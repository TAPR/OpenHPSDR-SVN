/**
* @file cusdr_radioWidget.h
* @brief Radio control widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-02-10
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
 
#ifndef _CUSDR_RADIO_WIDGET_H
#define _CUSDR_RADIO_WIDGET_H

//#include <QWidget>
//#include <QGroupBox>
//#include <QLineEdit>

#include "Util/cusdr_buttons.h"
#include "cusdr_settings.h"


class RadioWidget : public QWidget {

	Q_OBJECT

public:
	RadioWidget(QWidget *parent = 0);
	~RadioWidget();

public slots:
	QSize	sizeHint() const;
	QSize	minimumSizeHint() const;
	void systemStateChanged(
			QObject *sender, 
			QSDR::_Error err, 
			QSDR::_HWInterfaceMode hwmode, 
			QSDR::_ServerMode mode, 
			QSDR::_DataEngineState state);
		
protected:
	void closeEvent(QCloseEvent *event);
	void showEvent(QShowEvent *event);

private:
	Settings	*set;

	QGroupBox	*bandGroupBox;
	QGroupBox	*modeGroupBox;
	QGroupBox	*filterGroupABox;
	QGroupBox	*filterGroupBBox;
	QGroupBox	*filterGroupCBox;
	QGroupBox	*mercuryBtnGroup();

	QList<AeroButton *>	filterBtnListA;
	QList<AeroButton *>	filterBtnListB;
	QList<AeroButton *>	filterBtnListC;
	
	AeroButton	*band160mBtn;
	AeroButton	*band80mBtn;
	AeroButton	*band60mBtn;
	AeroButton	*band40mBtn;
	AeroButton	*band30mBtn;
	AeroButton	*band20mBtn;
	AeroButton	*band17mBtn;
	AeroButton	*band15mBtn;
	AeroButton	*band12mBtn;
	AeroButton	*band10mBtn;
	AeroButton	*band6mBtn;
	AeroButton	*bandGenBtn;
	//AeroButton	*bandxxBtn;

	QList<AeroButton *>	bandBtnList;
	QList<AeroButton *>	dspModeBtnList;

	AeroButton	*lsbBtn;
	AeroButton	*usbBtn;
	AeroButton	*dsbBtn;
	AeroButton	*cwlBtn;
	AeroButton	*cwuBtn;
	AeroButton	*fmnBtn;
	AeroButton	*amBtn;
	AeroButton	*diguBtn;
	AeroButton	*specBtn;
	AeroButton	*diglBtn;
	AeroButton	*samBtn;
	AeroButton	*drmBtn;

	AeroButton	*filter1kBtnA;
	AeroButton	*filter1k8BtnA;
	AeroButton	*filter2k1BtnA;
	AeroButton	*filter2k4BtnA;
	AeroButton	*filter2k7BtnA;
	AeroButton	*filter2k9BtnA;
	AeroButton	*filter3k3BtnA;
	AeroButton	*filter3k8BtnA;
	AeroButton	*filter4k4BtnA;
	AeroButton	*filter5kBtnA;
	AeroButton	*filterVar1BtnA;
	AeroButton	*filterVar2BtnA;

	AeroButton	*filter2k4BtnB;
	AeroButton	*filter2k9BtnB;
	AeroButton	*filter3k1BtnB;
	AeroButton	*filter4kBtnB;
	AeroButton	*filter5k2BtnB;
	AeroButton	*filter6k6BtnB;
	AeroButton	*filter8kBtnB;
	AeroButton	*filter10kBtnB;
	AeroButton	*filter12kBtnB;
	AeroButton	*filter16kBtnB;
	AeroButton	*filterVar1BtnB;
	AeroButton	*filterVar2BtnB;

	AeroButton	*filter25BtnC;
	AeroButton	*filter50BtnC;
	AeroButton	*filter100BtnC;
	AeroButton	*filter250BtnC;
	AeroButton	*filter400BtnC;
	AeroButton	*filter500BtnC;
	AeroButton	*filter600BtnC;
	AeroButton	*filter750BtnC;
	AeroButton	*filter800BtnC;
	AeroButton	*filter1kBtnC;
	AeroButton	*filterVar1BtnC;
	AeroButton	*filterVar2BtnC;
	
	AeroButton	*attenuatorBtn;
	AeroButton	*ditherBtn;
	AeroButton	*randomBtn;

	QIcon	agc_left;
	QIcon	agc_right;

	QList<TReceiver>	m_receiverDataList;
	QList<DSPMode>		m_dspModeList;
	HamBand				m_hamBand;
	//DSPMode				m_dspMode;
	AGCMode				m_agcMode;
	TDefaultFilterMode	m_filterMode;

	QList<long>			m_lastFrequencyList;
	long	m_frequency;

	qreal	m_filterLo;
	qreal	m_filterHi;

	int		m_currentRx;
	int		current_band;
	int		current_dsp_mode;
	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;

	void setupConnections();

private slots:
	void createBandBtnGroup();
	void createModeBtnGroup();
	void createFilterBtnGroupA();
	void createFilterBtnGroupB();
	void createFilterBtnGroupC();

	QLabel *createLabel(const QString &text);

	void setCurrentReceiver(QObject *sender, int value);
	void frequencyChanged(QObject* sender, bool value, int rx, long frequency);
	void bandChangedByBtn();
	void bandChanged(QObject *sender, int rx, bool byButton, HamBand band);
	void dspModeChangedByBtn();
	void dspModeChanged(QObject *sender, int rx, DSPMode mode);
	void filterChangedByBtn();
	void filterChanged(QObject *sender, int rx, qreal low, qreal high);
	void filterGroupChanged(DSPMode mode);

	void attenuatorChanged();
	void setMercuryAttenuator(QObject *sender, HamBand band, int value);
	void ditherChanged();
	void randomChanged();
	
signals:
	void showEvent(QObject *sender);
	void closeEvent(QObject *sender);
	void newMessage(QString msg);
};

#endif // _CUSDR_RADIO_WIDGET_H
