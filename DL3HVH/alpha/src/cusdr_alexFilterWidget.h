/**
* @file  cusdr_alexFilterWidget.h
* @brief Alexiares filter settings widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-08-23
*/

/*
 *   
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
 
#ifndef _CUSDR_ALEX_FILTER_WIDGET_H
#define _CUSDR_ALEX_FILTER_WIDGET_H

//#include <QWidget>
//#include <QComboBox>
//#include <QGroupBox>
//#include <QSpinBox>
//#include <QLineEdit>

#include "Util/cusdr_buttons.h"
#include "Util/cusdr_led.h"
#include "Util/cusdr_fonts.h"
#include "cusdr_settings.h"


class AlexFilterWidget : public QWidget {

	Q_OBJECT

public:
	AlexFilterWidget(QWidget *parent = 0);
	~AlexFilterWidget();

public slots:
	
private:
	void	setupConnections();
	void	createHPFGroup();
	void	createLPFGroup();
	
	void 	initAlexValues();
	void	setFilterValues();

private:
	Settings				*set;

	QSDR::_ServerMode		m_serverMode;
	QSDR::_HWInterfaceMode	m_hwInterface;
	QSDR::_DataEngineState	m_dataEngineState;

	QGroupBox*	HPFGroup;
	QGroupBox*	LPFGroup;

	QList<QDoubleSpinBox *>		m_HPFLoSpinBoxList;
	QList<QDoubleSpinBox *>		m_HPFHiSpinBoxList;
	QList<QDoubleSpinBox *>		m_LPFLoSpinBoxList;
	QList<QDoubleSpinBox *>		m_LPFHiSpinBoxList;

	QList<QHLed *>				m_HPFActiveBtnList;
	QList<QHLed *>				m_LPFActiveBtnList;

	QList<QLabel *>			m_HPFLabelList;
	QList<QLabel *>			m_LPFLabelList;
	QList<AeroButton *>		m_HPFBtnList;

	QList<int>					m_alexStates;

	QList<QPair<qreal, qreal> >	m_HPFFrequencyRangeLoList;
	QList<QPair<qreal, qreal> >	m_HPFFrequencyRangeHiList;
	QList<QPair<qreal, qreal> >	m_LPFFrequencyRangeLoList;
	QList<QPair<qreal, qreal> >	m_LPFFrequencyRangeHiList;

	QList<qreal>			m_HPFLoDefaultFrequencyList;
	QList<qreal>			m_HPFHiDefaultFrequencyList;
	QList<qreal>			m_LPFLoDefaultFrequencyList;
	QList<qreal>			m_LPFHiDefaultFrequencyList;

	CFonts		*fonts;
	TFonts		m_fonts;

	QLabel 		*mhz55HPFLabel;

	AeroButton 	*manualFilterBtn;
	AeroButton 	*defaultValuesBtn;
	AeroButton 	*bypassAllHPFBtn;
	AeroButton 	*lowNoise6mAmpBtn;
	AeroButton 	*hpf13MHzBtn;
	AeroButton 	*hpf20MHzBtn;
	AeroButton 	*hpf9_5MHzBtn;
	AeroButton 	*hpf6_5MHzBtn;
	AeroButton 	*hpf1_5MHzBtn;

	QColor 		btnOff;
	QColor 		btnOn;

	quint16	m_alexConfig;

	long	m_frequency;
	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;
	int		m_hpfFilters;
	int		m_lpfFilters;
	int		m_receiver;

	bool	bypassAll;
	bool 	lowNoise6m;
	bool 	hpf13MHz;
	bool 	hpf20MHz;
	bool 	hpf9_5MHz;
	bool 	hpf6_5MHz;
	bool 	hpf1_5MHz;

private slots:
	void alexManualStateChanged(QObject *sender, bool value);

	void hpfLoSpinBoxValueChanged(double value);
	void hpfHiSpinBoxValueChanged(double value);
	void lpfLoSpinBoxValueChanged(double value);
	void lpfHiSpinBoxValueChanged(double value);

	void setFrequency(QObject * sender, bool value, int rx, long frequency);
	void setCurrentReceiver(QObject *sender, int rx);
	void setAlexConfiguration(double frequency);
	void manualFilterBtnClicked();
	void defaultValuesBtnClicked();
	void bypassAllHPFBtnClicked();
	void lowNoise6mAmpBtnClicked();
	void hpf13MHzBtnClicked();
	void hpf20MHzBtnClicked();
	void hpf9_5MHzBtnClicked();
	void hpf6_5MHzBtnClicked();
	void hpf1_5MHzBtnClicked();

signals:
	void	showEvent(QObject* sender);
	void	closeEvent(QObject* sender);
	void	messageEvent(QString);
};

#endif // _CUSDR_ALEX_FILTER_WIDGET_H
