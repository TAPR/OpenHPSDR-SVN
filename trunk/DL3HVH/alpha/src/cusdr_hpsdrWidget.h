/**
* @file cusdr_hpsdrWidget.h
* @brief Hardware settings widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-09-21
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
 
#ifndef _CUSDR_HPSDR_WIDGET_H
#define _CUSDR_HPSDR_WIDGET_H

//#include <QWidget>
//#include <QGroupBox>
//#include <QSpinBox>
//#include <QLineEdit>

#include "Util/cusdr_buttons.h"
#include "cusdr_settings.h"

#ifdef LOG_HPSDR_WIDGET
#   define HPSDR_WIDGET_DEBUG qDebug().nospace() << "HPSDRWidget::\t"
#else
#   define HPSDR_WIDGET_DEBUG nullDebug()
#endif


class HPSDRWidget : public QWidget {

	Q_OBJECT

public:
	HPSDRWidget(QWidget *parent = 0);
	~HPSDRWidget();


public slots:
	void	hpsdrHardwareChanged();
	void	penelopePresenceChanged();
	void	pennyPresenceChanged();
	void	mercuryPresenceChanged();
	void	alexPresenceChanged();
	void	excaliburPresenceChanged();
	void	source122_88MhzChanged();

private:
	Settings	*set;

	QGroupBox	*hpsdrHardwareBtnGroup();
	QGroupBox	*receiversExclusiveBtnGroup();
	QGroupBox	*source10MhzExclusiveGroup;
	QGroupBox	*source122_88MhzExclusiveGroup;
	QGroupBox	*sampleRateExclusiveGroup();
	QGroupBox	*numberOfReceiversGroup();
	
	QComboBox	*m_receiverComboBox;

	QLabel		*m_receiversLabel;
	
	QString		m_message;

	AeroButton	*hermesPresenceBtn;
	AeroButton	*modulesPresenceBtn;
	AeroButton	*penelopePresenceBtn;
	AeroButton	*pennyPresenceBtn;
	AeroButton	*mercuryPresenceBtn;
	AeroButton	*alexPresenceBtn;
	AeroButton	*excaliburPresenceBtn;
	AeroButton	*atlasBtn;
	AeroButton	*penelopeBtn;
	AeroButton	*mercuryBtn;
	AeroButton	*penelope2Btn;
	AeroButton	*mercury2Btn;
	AeroButton	*rx1to2Btn;
	AeroButton	*samplerate48Btn;
	AeroButton	*samplerate96Btn;
	AeroButton	*samplerate192Btn;


	QList<AeroButton *>	hardwareBtnList;
	QList<AeroButton *>	source10MhzBtnList;
	QList<AeroButton *>	viewBtnList;
	QList<QString> sources10Mhz;
	QList<AeroButton *>	rxBtnList;

	QSDR::_ServerMode		m_serverMode;
	QSDR::_HWInterfaceMode	m_hwInterface;
	QSDR::_HWInterfaceMode	m_hwInterfaceTemp;
	QSDR::_DataEngineState	m_dataEngineState;

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;
	int		m_numberOfReceivers;
	int		m_hpsdrHardware;
	int		m_socketBufferSize;

	void	setupConnections();
	void	createSource10MhzExclusiveGroup();
	void	createSource122_88MhzExclusiveGroup();

private slots:
	void	systemStateChanged(
					QObject *sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);

	void	setHPSDRHardware();
	void	source10MhzChanged();
	void	setNumberOfReceivers(int value);
	void	setShow1on2();
	void	disableButtons();
	void	enableButtons();
	void 	sampleRateChangedTo48();
	void 	sampleRateChangedTo96();
	void 	sampleRateChangedTo192();

	
signals:
	void	messageEvent(QString message);
};

#endif // _CUSDR_HPSDR_WIDGET_H
