/**
* @file  cusdr_oglWidget.h
* @brief OpenGL widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-01-13
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

#ifndef _CUSDR_OGLWIDGET_H
#define _CUSDR_OGLWIDGET_H

#include "cusdr_settings.h"
#include "cusdr_oglReceiverPanel.h"
#include "cusdr_oglWidebandPanel.h"
#include "cusdr_oglDistancePanel.h"
//#include "cusdr_oglUtils.h"
//#include "cusdr_oglInfo.h"

#include <QtGui>
#include <QtOpenGL/QGLWidget>

class OGLWidget : public QWidget
{
    Q_OBJECT

public:
    OGLWidget(QWidget* parent = 0);
    ~OGLWidget();

	QSize minimumSizeHint() const;
	QSize sizeHint() const;

protected:
	void resizeEvent(QResizeEvent *event);
	void paintEvent(QPaintEvent* event);

private:
	Settings*		m_settings;

	QSDR::_ServerMode			m_serverMode;
	QSDR::_HWInterfaceMode		m_hwInterface;
	QSDR::_DataEngineState		m_dataEngineState;

	QGLWidebandPanel*			m_wbDisplay;
	QGLDistancePanel*			m_distDisplay;
	QList<QGLReceiverPanel *>	m_rxWidgetList;
	QList<QHBoxLayout *>		m_rxBoxLayoutList;
	QHBoxLayout*				widebandBox;

	QSplitter*					m_topSplitter;
	QSplitter*					m_bottomSplitter;
	QSplitter*					m_widebandSplitter;

	QTime						m_displayTime;

	bool	m_widebandPanel;
	bool	m_wideBandStatus;

	int		m_numberOfReceivers;
	int		m_multiRxView;
	int		m_minWidebandDisplayHeight;
	
	void	setupConnections();
	void	initReceiverPanels(int rx);
	void	initReceiverDisplays(int rx, int view);
	void	initWidebandDisplay();
	void	initDistanceDisplay();
	void	initLayout(int view);

private slots:
	void	systemStateChanged(
					QObject* sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);

	void	setNumberOfReceivers(QObject* sender, int value);
	void	setMultiRxView(int view);
	void	setSpectrumBuffer(int rx, const float* buffer);
	void	setFrequency(QObject* sender, bool value, int rx, long freq);
	void	setWideBandStatus(bool value);
};

#endif // _CUSDR_OGLWIDGET_H