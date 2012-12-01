/**
* @file  cusdr_alexAntennaWidget.h
* @brief Alexiares antenna settings widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-06-09
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
 
#ifndef _CUSDR_ALEX_ANTENNA_WIDGET_H
#define _CUSDR_ALEX_ANTENNA_WIDGET_H

//#include <QWidget>
//#include <QComboBox>
//#include <QGroupBox>
//#include <QSpinBox>
//#include <QLineEdit>

#include "Util/cusdr_buttons.h"
#include "cusdr_settings.h"
#include "cusdr_fonts.h"

#ifdef LOG_ANTENNA_WIDGET
#   define ANTENNA_WIDGET_DEBUG qDebug().nospace() << "AntennaWidget::\t"
#else
#   define ALEX_WIDGET_DEBUG nullDebug()
#endif


class AlexAntennaWidget : public QWidget {

	Q_OBJECT

public:
	AlexAntennaWidget(QWidget *parent = 0);
	~AlexAntennaWidget();

protected:
	//void resizeEvent(QResizeEvent *event);

public slots:
	
private:
	void	setupConnections();
	void	createAntennasGroup();
	
	void 	setAlexValues();

private:
	Settings				*set;

	QSDR::_ServerMode		m_serverMode;
	QSDR::_HWInterfaceMode	m_hwInterface;
	QSDR::_DataEngineState	m_dataEngineState;

	QGroupBox				*antennaGroup;

	QList<AeroButton *>		antenna1BtnList;
	QList<AeroButton *>		antenna2BtnList;
	QList<AeroButton *>		antenna3BtnList;

	QList<QList<AeroButton *> >	bandBtnMatrix;

	QList<AeroButton *>	rx1BtnList;
	QList<AeroButton *>	rx2BtnList;
	QList<AeroButton *>	xvBtnList;

	QList<QList<AeroButton *> >	bandBtnRxMatrix;

	QList<AeroButton *>	tx1BtnList;
	QList<AeroButton *>	tx2BtnList;
	QList<AeroButton *>	tx3BtnList;

	QList<QList<AeroButton *> >	bandBtnTxMatrix;

	QList<int>					m_alexStates;

	CFonts	*fonts;
	TFonts	m_fonts;

	quint16	m_alexConfig;

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;
	int		m_numberOfBands;

private slots:
	void	rxAntBtnClicked();
	void	rxAuxBtnClicked();
	void	txAntBtnClicked();

signals:
	void	showEvent(QObject* sender);
	void	closeEvent(QObject* sender);
	void	messageEvent(QString);
};

#endif // _CUSDR_ALEX_ANTENNA_WIDGET_H
