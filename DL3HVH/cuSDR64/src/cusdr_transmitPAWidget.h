/**
* @file  cusdr_transmitPAWidget.h
* @brief transmit PA control widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-08-28
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
 
#ifndef _CUSDR_TRANSMIT_PA_WIDGET_H
#define _CUSDR_TRANSMIT_PA_WIDGET_H

//#include <QWidget>
//#include <QGroupBox>
//#include <QLineEdit>

#include "Util/cusdr_buttons.h"
#include "cusdr_settings.h"


class TransmitPAWidget : public QWidget {

	Q_OBJECT

public:
	TransmitPAWidget(QWidget *parent = 0);
	~TransmitPAWidget();


public slots:
	
protected:
	void	closeEvent(QCloseEvent *event);
	void	showEvent(QShowEvent *event);
	void	enterEvent(QEvent *event);
	void	leaveEvent(QEvent *event);
	void	mouseMoveEvent(QMouseEvent *event);
	void	mousePressEvent(QMouseEvent *event);
	void	mouseReleaseEvent(QMouseEvent *event);

private:
	void	setupConnections();
	void	createGainGroup();

private:
	Settings*	set;

	QString		m_message;

	QGroupBox	*gainGroup;

	QList<QDoubleSpinBox *>	gainSpinBoxList;

//	AeroButton*	micInputBtn;

	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;
	
private slots:
//	void	inputButtonClicked();
//	void	boostButtonClicked();
	
signals:
	void	showEvent(QObject *sender);
	void	closeEvent(QObject *sender);
	void	messageEvent(QString message);
};

#endif // _CUSDR_TRANSMIT_PS_WIDGET_H
