/**
* @file  cusdr_colorsWidget.cpp
* @brief OpenGL color options widget class for cuSDR
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

#define LOG_COLOROPTIONS_WIDGET

// use: COLOROPTIONS_DEBUG

//#include <QtGui>
//#include <QPen>
//#include <QDebug>
//#include <QDialog>
//#include <QColorDialog>

#include "cusdr_colorsWidget.h"

#define	btn_height		15
#define	btn_width		70
#define	btn_widths		38
#define	btn_width2		52
#define	btn_width3		60

ColorOptionsWidget::ColorOptionsWidget(QWidget *parent)
	: QWidget(parent)
	, set(Settings::instance())
	, m_serverMode(set->getCurrentServerMode())
	, m_hwInterface(set->getHWInterface())
	, m_dataEngineState(set->getDataEngineState())
	, m_panadapterMode(set->getPanadapterMode())
	, m_waterColorScheme(set->getWaterfallColorScheme())
	, m_colorTriangle(new QtColorTriangle(this))
	, m_antialiased(true)
	, m_mouseOver(false)
	, m_minimumWidgetWidth(set->getMinimumWidgetWidth())
	, m_minimumGroupBoxWidth(set->getMinimumGroupBoxWidth())
	, m_btnSpacing(5)
	, m_currentReceiver(set->getCurrentReceiver())
	, m_btnChooserHit(0)
	, m_sampleRate(set->getSampleRate())
{
	//setMinimumWidth(m_minimumWidgetWidth);
	setContentsMargins(4, 0, 4, 0);
	setMouseTracking(true);

	m_rxDataList = set->getReceiverDataList();
	
	createColorChooserWidget();

	QBoxLayout *mainLayout = new QBoxLayout(QBoxLayout::TopToBottom, this);
	mainLayout->setSpacing(5);
	mainLayout->setMargin(0);
	mainLayout->addSpacing(8);

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(0);
	hbox1->setMargin(0);
	hbox1->addStretch();
	hbox1->addWidget(m_colorChooserWidget);
	hbox1->addStretch();

	mainLayout->addLayout(hbox1);
	mainLayout->addStretch();
	setLayout(mainLayout);

	m_panadapterColors = set->getPanadapterColors();
	m_oldPanadapterColors = m_panadapterColors;

	setupConnections();
}

ColorOptionsWidget::~ColorOptionsWidget() {

	// disconnect all signals
	disconnect(set, 0, this, 0);
	disconnect(this, 0, 0, 0);
}

QSize ColorOptionsWidget::sizeHint() const {
	
	return QSize(m_minimumWidgetWidth, height());
}

QSize ColorOptionsWidget::minimumSizeHint() const {

	return QSize(m_minimumWidgetWidth, height());
}

void ColorOptionsWidget::setupConnections() {

	CHECKED_CONNECT(
		set,
		SIGNAL(systemStateChanged(
					QObject *,
					QSDR::_Error,
					QSDR::_HWInterfaceMode,
					QSDR::_ServerMode,
					QSDR::_DataEngineState)),
		this,
		SLOT(systemStateChanged(
					QObject *,
					QSDR::_Error,
					QSDR::_HWInterfaceMode,
					QSDR::_ServerMode,
					QSDR::_DataEngineState)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(graphicModeChanged(
					QObject *,
					QSDRGraphics::_Panadapter,
					QSDRGraphics::_WfScheme)),
		this, 
		SLOT(graphicModeChanged(
					QObject *,
					QSDRGraphics::_Panadapter,
					QSDRGraphics::_WfScheme)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(currentReceiverChanged(QObject *, int)),
		this, 
		SLOT(setCurrentReceiver(QObject *, int)));

	CHECKED_CONNECT(
		set, 
		SIGNAL(sampleRateChanged(QObject *, int)), 
		this, 
		SLOT(sampleRateChanged(QObject *, int)));

	CHECKED_CONNECT(
		m_colorTriangle, 
		SIGNAL(colorChanged(QColor)), 
		this, 
		SLOT(triangleColorChanged(QColor)));
}

void ColorOptionsWidget::systemStateChanged(
	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	Q_UNUSED (sender)
	Q_UNUSED (err)

	bool change = false;

	if (m_hwInterface != hwmode) {
		
		m_hwInterface = hwmode;
		//hwInterfaceChanged();
		change = true;
	}

	if (m_serverMode != mode) {
		
		m_serverMode = mode;
		change = true;
	}
		
	if (m_dataEngineState != state) {

		m_dataEngineState = state;

		if (m_dataEngineState == QSDR::DataEngineUp) {
		
			
		}
		else {

			
		}
		change = true;
	}
	if (!change) return;

	update();
}

void ColorOptionsWidget::graphicModeChanged(
	QObject *sender,
	QSDRGraphics::_Panadapter panMode,
	QSDRGraphics::_WfScheme waterColorScheme)
{
	Q_UNUSED (sender)

	bool change = false;

	if (m_panadapterMode != panMode) {
		
		m_panadapterMode = panMode;
		change = true;
	}

	if (m_waterColorScheme != waterColorScheme) {
		
		m_waterColorScheme = waterColorScheme;
		change = true;
	}

	if (!change) return;

	update();
}

void ColorOptionsWidget::createColorChooserWidget() {

	QColor col = set->getPanadapterColors().panCenterLineColor;

	m_setPanBackground = new AeroButton("Background", this);
	m_setPanBackground->setRoundness(0);
	m_setPanBackground->setFixedSize (btn_width3, btn_height);
	m_setPanBackground->setStyleSheet(set->getMiniButtonStyle());
	m_setPanBackground->setColorOn(col);
	m_changeColorBtnList.append(m_setPanBackground);

	CHECKED_CONNECT(
		m_setPanBackground, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().waterfallColor;

	m_setWaterfall = new AeroButton("Waterfall", this);
	m_setWaterfall->setRoundness(0);
	m_setWaterfall->setFixedSize (btn_width3, btn_height);
	m_setWaterfall->setStyleSheet(set->getMiniButtonStyle());
	m_setWaterfall->setColorOn(col);
	m_changeColorBtnList.append(m_setWaterfall);

	CHECKED_CONNECT(
		m_setWaterfall, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().panLineColor;

	m_setPanLine = new AeroButton("Pan Line", this);
	m_setPanLine->setRoundness(0);
	m_setPanLine->setFixedSize (btn_width3, btn_height);
	m_setPanLine->setStyleSheet(set->getMiniButtonStyle());
	m_setPanLine->setColorOn(col);
	m_changeColorBtnList.append(m_setPanLine);

	CHECKED_CONNECT(
		m_setPanLine, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().panLineFilledColor;

	m_setPanLineFilling = new AeroButton("Pan Filling", this);
	m_setPanLineFilling->setRoundness(0);
	m_setPanLineFilling->setFixedSize (btn_width3, btn_height);
	m_setPanLineFilling->setStyleSheet(set->getMiniButtonStyle());
	m_setPanLineFilling->setColorOn(col);
	m_changeColorBtnList.append(m_setPanLineFilling);

	CHECKED_CONNECT(
		m_setPanLineFilling, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().panSolidTopColor;

	m_setPanSolidTop = new AeroButton("Solid T", this);
	m_setPanSolidTop->setRoundness(0);
	m_setPanSolidTop->setFixedSize (btn_width3, btn_height);
	m_setPanSolidTop->setStyleSheet(set->getMiniButtonStyle());
	m_setPanSolidTop->setColorOn(col);
	m_changeColorBtnList.append(m_setPanSolidTop);

	CHECKED_CONNECT(
		m_setPanSolidTop, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().panSolidBottomColor;

	m_setPanSolidBottom = new AeroButton("Solid B", this);
	m_setPanSolidBottom->setRoundness(0);
	m_setPanSolidBottom->setFixedSize (btn_width3, btn_height);
	m_setPanSolidBottom->setStyleSheet(set->getMiniButtonStyle());
	m_setPanSolidBottom->setColorOn(col);
	m_changeColorBtnList.append(m_setPanSolidBottom);

	CHECKED_CONNECT(
		m_setPanSolidBottom, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().wideBandLineColor;

	m_setWideBandLine = new AeroButton("Wideband", this);
	m_setWideBandLine->setRoundness(0);
	m_setWideBandLine->setFixedSize (btn_width3, btn_height);
	m_setWideBandLine->setStyleSheet(set->getMiniButtonStyle());
	m_setWideBandLine->setColorOn(col);
	m_changeColorBtnList.append(m_setWideBandLine);

	CHECKED_CONNECT(
		m_setWideBandLine, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().wideBandFilledColor;

	m_setWideBandFilling = new AeroButton("WB Filling", this);
	m_setWideBandFilling->setRoundness(0);
	m_setWideBandFilling->setFixedSize (btn_width3, btn_height);
	m_setWideBandFilling->setStyleSheet(set->getMiniButtonStyle());
	m_setWideBandFilling->setColorOn(col);
	m_changeColorBtnList.append(m_setWideBandFilling);

	CHECKED_CONNECT(
		m_setWideBandFilling, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().wideBandSolidTopColor;

	m_setWideBandSolidTop = new AeroButton("WB Solid T", this);
	m_setWideBandSolidTop->setRoundness(0);
	m_setWideBandSolidTop->setFixedSize (btn_width3, btn_height);
	m_setWideBandSolidTop->setStyleSheet(set->getMiniButtonStyle());
	m_setWideBandSolidTop->setColorOn(col);
	m_changeColorBtnList.append(m_setWideBandSolidTop);

	CHECKED_CONNECT(
		m_setWideBandSolidTop, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().wideBandSolidBottomColor;

	m_setWideBandSolidBottom = new AeroButton("WB Solid B", this);
	m_setWideBandSolidBottom->setRoundness(0);
	m_setWideBandSolidBottom->setFixedSize (btn_width3, btn_height);
	m_setWideBandSolidBottom->setStyleSheet(set->getMiniButtonStyle());
	m_setWideBandSolidBottom->setColorOn(col);
	m_changeColorBtnList.append(m_setWideBandSolidBottom);

	CHECKED_CONNECT(
		m_setWideBandSolidBottom, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().wideBandSolidBottomColor;

	m_setDistanceLine = new AeroButton("Dist. Line", this);
	m_setDistanceLine->setRoundness(0);
	m_setDistanceLine->setFixedSize (btn_width3, btn_height);
	m_setDistanceLine->setStyleSheet(set->getMiniButtonStyle());
	m_setDistanceLine->setColorOn(col);
	m_changeColorBtnList.append(m_setDistanceLine);

	CHECKED_CONNECT(
		m_setDistanceLine, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().wideBandSolidBottomColor;

	m_setDistanceLineFilling = new AeroButton("Dist. Filling", this);
	m_setDistanceLineFilling->setRoundness(0);
	m_setDistanceLineFilling->setFixedSize (btn_width3, btn_height);
	m_setDistanceLineFilling->setStyleSheet(set->getMiniButtonStyle());
	m_setDistanceLineFilling->setColorOn(col);
	m_changeColorBtnList.append(m_setDistanceLineFilling);

	CHECKED_CONNECT(
		m_setDistanceLineFilling, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().panCenterLineColor;

	m_setPanCenterLine = new AeroButton("Center", this);
	m_setPanCenterLine->setRoundness(0);
	m_setPanCenterLine->setFixedSize (btn_width3, btn_height);
	m_setPanCenterLine->setStyleSheet(set->getMiniButtonStyle());
	m_setPanCenterLine->setColorOn(col);
	m_changeColorBtnList.append(m_setPanCenterLine);

	CHECKED_CONNECT(
		m_setPanCenterLine, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));

	col = set->getPanadapterColors().gridLineColor;

	m_setGridLine = new AeroButton("Grid", this);
	m_setGridLine->setRoundness(0);
	m_setGridLine->setFixedSize (btn_width3, btn_height);
	m_setGridLine->setStyleSheet(set->getMiniButtonStyle());
	m_setGridLine->setColorOn(col);
	m_changeColorBtnList.append(m_setGridLine);

	CHECKED_CONNECT(
		m_setGridLine, 
		SIGNAL(clicked()), 
		this, 
		SLOT(colorChooserChanged()));


	m_setPanBackground->setBtnState(AeroButton::OFF);
	m_setWaterfall->setBtnState(AeroButton::OFF);
	m_setPanLine->setBtnState(AeroButton::OFF);
	m_setPanLineFilling->setBtnState(AeroButton::OFF);
	m_setPanSolidTop->setBtnState(AeroButton::OFF);
	m_setPanSolidBottom->setBtnState(AeroButton::OFF);
	m_setWideBandLine->setBtnState(AeroButton::OFF);
	m_setWideBandFilling->setBtnState(AeroButton::OFF);
	m_setWideBandSolidTop->setBtnState(AeroButton::OFF);
	m_setWideBandSolidBottom->setBtnState(AeroButton::OFF);
	m_setDistanceLine->setBtnState(AeroButton::OFF);
	m_setDistanceLineFilling->setBtnState(AeroButton::OFF);
	m_setPanCenterLine->setBtnState(AeroButton::OFF);
	m_setGridLine->setBtnState(AeroButton::OFF);

	m_resetBtn = new AeroButton("Reset", this);
	m_resetBtn->setRoundness(0);
	//m_resetBtn->setFixedSize (btn_widths, btn_height);
	m_resetBtn->setFixedSize (btn_width3, btn_height);
	
	CHECKED_CONNECT(
		m_resetBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(resetColors()));

	m_okBtn = new AeroButton("OK", this);
	m_okBtn->setRoundness(0);
	//m_okBtn->setFixedSize (btn_widths, btn_height);
	m_okBtn->setFixedSize (btn_width3, btn_height);
	
	CHECKED_CONNECT(
		m_okBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(acceptColors()));

	
	QGridLayout *grid1 = new QGridLayout;
	grid1->setSpacing(1);
	grid1->setHorizontalSpacing(1);
	grid1->setContentsMargins(0, 0, 0, 0);
	grid1->addWidget(m_colorTriangle, 0, 3, 5, 3);
	grid1->addWidget(m_setPanBackground, 0, 0);
	grid1->addWidget(m_setWaterfall, 0, 1);
	grid1->addWidget(m_setPanLine, 1, 0);
	grid1->addWidget(m_setPanLineFilling, 1, 1);
	grid1->addWidget(m_setPanSolidTop, 2, 0);
	grid1->addWidget(m_setPanSolidBottom, 2, 1);
	grid1->addWidget(m_setWideBandLine, 3, 0);
	grid1->addWidget(m_setWideBandFilling, 3, 1);
	grid1->addWidget(m_setWideBandSolidTop, 4, 0);
	grid1->addWidget(m_setWideBandSolidBottom, 4, 1);
	grid1->addWidget(m_setDistanceLine, 5, 0);
	grid1->addWidget(m_setDistanceLineFilling, 5, 1);
	grid1->addWidget(m_setPanCenterLine, 6, 0);
	grid1->addWidget(m_setGridLine, 6, 1);
	grid1->addWidget(m_resetBtn, 8, 0);
	grid1->addWidget(m_okBtn, 8, 1);
	//grid1->addWidget(m_resetBtn, 7, 3);
	//grid1->addWidget(m_okBtn, 7, 4);

	QHBoxLayout *hbox1 = new QHBoxLayout;
	hbox1->setSpacing(4);
	hbox1->addStretch();
	hbox1->addWidget(m_resetBtn);
	hbox1->addWidget(m_okBtn);
	
	QVBoxLayout *vbox1 = new QVBoxLayout;
	vbox1->setSpacing(4);
	//vbox1->addStretch();
	vbox1->addWidget(m_colorTriangle);
	vbox1->addLayout(hbox1);
	
	QHBoxLayout *hbox = new QHBoxLayout;
	hbox->setSpacing(4);
	hbox->addSpacing(6);
	hbox->addLayout(grid1);
	//hbox->addLayout(vbox1);

	m_colorChooserWidget = new QGroupBox(tr("Color Chooser"), this);
	m_colorChooserWidget->setMinimumWidth(m_minimumGroupBoxWidth);
	m_colorChooserWidget->setLayout(hbox);
	m_colorChooserWidget->setStyleSheet(set->getWidgetStyle());
	m_colorChooserWidget->setFont(QFont("Arial", 8));
}

void ColorOptionsWidget::colorChooserChanged() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	m_btnChooserHit = m_changeColorBtnList.indexOf(button);

	foreach(AeroButton *btn, m_changeColorBtnList) {

		btn->setBtnState(AeroButton::OFF);
		//btn->setColor(QColor(90, 90, 90));
		btn->update();
	}

	button->setBtnState(AeroButton::ON);
	button->update();

	switch (m_btnChooserHit) {

		case 0:
			m_colorTriangle->setColor(set->getPanadapterColors().panBackgroundColor);
			break;

		case 1:
			m_colorTriangle->setColor(set->getPanadapterColors().waterfallColor);
			break;
			
		case 2:
			m_colorTriangle->setColor(set->getPanadapterColors().panLineColor);
			break;

		case 3:
			m_colorTriangle->setColor(set->getPanadapterColors().panLineFilledColor);
			break;

		case 4:
			m_colorTriangle->setColor(set->getPanadapterColors().panSolidTopColor);
			break;

		case 5:
			m_colorTriangle->setColor(set->getPanadapterColors().panSolidBottomColor);
			break;

		case 6:
			m_colorTriangle->setColor(set->getPanadapterColors().wideBandLineColor);
			break;

		case 7:
			m_colorTriangle->setColor(set->getPanadapterColors().wideBandFilledColor);
			break;

		case 8:
			m_colorTriangle->setColor(set->getPanadapterColors().wideBandSolidTopColor);
			break;

		case 9:
			m_colorTriangle->setColor(set->getPanadapterColors().wideBandSolidBottomColor);
			break;

		case 10:
			m_colorTriangle->setColor(set->getPanadapterColors().distanceLineColor);
			break;

		case 11:
			m_colorTriangle->setColor(set->getPanadapterColors().distanceLineFilledColor);
			break;

		case 12:
			m_colorTriangle->setColor(set->getPanadapterColors().panCenterLineColor);
			break;

		case 13:
			m_colorTriangle->setColor(set->getPanadapterColors().gridLineColor);
			break;
	}
}

void ColorOptionsWidget::resetColors() {

	m_panadapterColors = m_oldPanadapterColors;

	foreach(AeroButton *btn, m_changeColorBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}
	set->setPanadapterColors(m_panadapterColors);
}

void ColorOptionsWidget::acceptColors() {

	m_oldPanadapterColors = m_panadapterColors;
	set->setPanadapterColors(m_panadapterColors);
}

void ColorOptionsWidget::triangleColorChanged(QColor color) {

	m_currentColor = color;
	m_changeColorBtnList.at(m_btnChooserHit)->setColorOn(color);
	m_changeColorBtnList.at(m_btnChooserHit)->update();

	switch (m_btnChooserHit) {

		case 0:
			m_panadapterColors.panBackgroundColor = color;
			break;

		case 1:
			m_panadapterColors.waterfallColor = color;
			break;
			
		case 2:
			m_panadapterColors.panLineColor = color;
			break;

		case 3:
			m_panadapterColors.panLineFilledColor = color;
			break;

		case 4:
			m_panadapterColors.panSolidTopColor = color;
			break;

		case 5:
			m_panadapterColors.panSolidBottomColor = color;
			break;

		case 6:
			m_panadapterColors.wideBandLineColor = color;
			break;

		case 7:
			m_panadapterColors.wideBandFilledColor = color;
			break;

		case 8:
			m_panadapterColors.wideBandSolidTopColor = color;
			break;

		case 9:
			m_panadapterColors.wideBandSolidBottomColor = color;
			break;

		case 10:
			m_panadapterColors.distanceLineColor = color;
			break;

		case 11:
			m_panadapterColors.distanceLineFilledColor = color;
			break;

		case 12:
			m_panadapterColors.panCenterLineColor = color;
			break;

		case 13:
			m_panadapterColors.gridLineColor = color;
			break;
	}
	set->setPanadapterColors(m_panadapterColors);
}

void ColorOptionsWidget::sampleRateChanged(QObject *sender, int value) {

	Q_UNUSED(sender)

	m_sampleRate = value;
}

void ColorOptionsWidget::setCurrentReceiver(QObject *sender, int rx) {

	Q_UNUSED(sender)

	if (m_currentReceiver == rx) return;
	m_currentReceiver = rx;
}
