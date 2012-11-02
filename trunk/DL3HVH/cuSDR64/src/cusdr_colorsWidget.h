/**
* @file  cusdr_colorsWidget.h
* @brief OpenGL color options widget header file for cuSDR
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
 
#ifndef _CUSDR_COLOR_OPTIONS_WIDGET_H
#define _CUSDR_COLOR_OPTIONS_WIDGET_H

//#include <QWidget>
//#include <QPainter>
//#include <QComboBox>
//#include <QGroupBox>
//#include <QSpinBox>
//#include <QLineEdit>

#include "Util/cusdr_buttons.h"
#include "Util/cusdr_colorTriangle.h"
#include "cusdr_settings.h"

#ifdef LOG_COLOROPTIONS_WIDGET
#   define COLOROPTIONS_DEBUG qDebug().nospace() << "ColorWidget::\t"
#else
#   define COLOROPTIONS_DEBUG nullDebug()
#endif


class ColorOptionsWidget : public QWidget {

	Q_OBJECT

public:
	ColorOptionsWidget(QWidget *parent = 0);
	~ColorOptionsWidget();

public slots:
	QSize	sizeHint() const;
	QSize	minimumSizeHint() const;

private:
	Settings					*set;

	QSDR::_ServerMode			m_serverMode;
	QSDR::_HWInterfaceMode		m_hwInterface;
	QSDR::_DataEngineState		m_dataEngineState;
	QSDRGraphics::_Panadapter	m_panadapterMode;
	QSDRGraphics::_WfScheme		m_waterColorScheme;

	QtColorTriangle				*m_colorTriangle;
	QList<TReceiver>			m_rxDataList;
	
	QString						m_menu_style;

	QColor			m_currentColor;
	QColor			m_newColor;

	QGroupBox		*m_colorChooserWidget;

	AeroButton		*m_resetBtn;
	AeroButton		*m_okBtn;
	AeroButton		*m_setPanBackground;
	AeroButton		*m_setPanCenterLine;
	AeroButton		*m_setPanLine;
	AeroButton		*m_setPanLineFilling;
	AeroButton		*m_setPanSolidTop;
	AeroButton		*m_setPanSolidBottom;
	AeroButton		*m_setWaterfall;
	AeroButton		*m_setWideBandLine;
	AeroButton		*m_setWideBandFilling;
	AeroButton		*m_setWideBandSolidTop;
	AeroButton		*m_setWideBandSolidBottom;
	AeroButton		*m_setDistanceLine;
	AeroButton		*m_setDistanceLineFilling;
	AeroButton		*m_setGridLine;

	QList<AeroButton *>		m_changeColorBtnList;

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
	int		m_sampleRate;

	void	setupConnections();
	void	createColorChooserWidget();

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
					QSDRGraphics::_WfScheme waterColorScheme);

	void	setCurrentReceiver(QObject *sender, int rx);
	void	colorChooserChanged();
	void	resetColors();
	void	acceptColors();
	void	triangleColorChanged(QColor color);
	void	sampleRateChanged(QObject *sender, int value);
	
signals:
	void	colorChanged(const QColor &color);
};

#endif // _CUSDR_COLOR_OPTIONS_WIDGET_H
