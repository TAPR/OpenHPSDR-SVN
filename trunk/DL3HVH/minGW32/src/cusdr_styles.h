/**
* @file  cusdr_styles.h
* @brief style settings header file for cuSDR
* @author by Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-06-30
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
 
#ifndef CUSDR_STYLES_H
#define CUSDR_STYLES_H

QString sdrStyle = QString::fromUtf8(
		"background-color: rgba(0, 0, 0, 255);"
		);
	
QString dialogStyle = QString::fromUtf8(
		"background-color: rgba(60, 60, 60, 255);");

QString colorDialogStyle = QString::fromUtf8(
		"background-color: rgba(30, 30, 30, 255); "
		"color: rgb(166, 196, 208); ");

QString mainWindowStyle = QString::fromUtf8(
		"QMainWindow::separator {"
		"	background: rgba(166, 196, 208, 130);"
		"	width: 3px;"
		"	height: 3px;"
		"}"
		"QMainWindow::separator:hover {"
		"	background: rgba(166, 196, 208, 200);"
		"}"
		);

QString	widgetStyle = QString::fromUtf8(
		//"background-color: rgba(0, 0, 0, 255); "
		"QGroupBox { "
		"	background-color: rgba(0, 0, 0, 255); "
		//"	border: 1px solid rgba(45, 122, 148, 150); "
		"	border: 1px solid rgba(255, 255, 255, 80); "
		"	border-radius: 2px; margin-top: 2ex; } "
		"QGroupBox::title { "
		"	subcontrol-origin: margin; "
		"	padding: 0 0px; "
		"	color: rgba(170, 170, 170, 255); }"
		);

QString itemStyle = QString::fromUtf8(
		"color: rgba(200, 200, 200, 255); "
		"border: 0px solid; ");

QString	labelStyle = QString::fromUtf8(
		"color: rgba(166, 196, 208, 255);");

QString	tableStyle = QString::fromUtf8(
		"color: rgba(166, 196, 208, 255); "
		"border: 0px solid; ");

QString	messageBoxStyle = QString::fromUtf8(
		"border: 0px solid rgba(166, 196, 208, 255); "
		"border-right-width: 0px; "
		"color: rgb(166, 196, 208); "
		"background-color: rgba(0, 0, 0, 255); ");

QString	lineEditStyle = QString::fromUtf8(
		"border: 1px solid rgba(60, 60, 60, 255); "
		//"border-right-width: 0px; "
		"color: rgb(166, 196, 208); "
		"background-color: rgba(0, 0, 0, 255); ");

QString toolbarStyle = QString::fromUtf8(
		"QToolBar {"
		"	border: 0px solid rgba(0, 0, 0, 255); "
		//"border-right-width: 0px; "
		"	color: rgb(166, 196, 208); "
		"	background-color: rgba(0, 0, 0, 255); "
		"}"
		"QToolBar::handle {"
		//"	background-color: rgba(0, 0, 0, 255); "
		"}"
		);

QString statusbarStyle = QString::fromUtf8(
		"QStatusBar {"
		//"border: 0px solid rgba(60, 60, 60, 255); "
		//"border-right-width: 0px; "
		"	color: rgb(166, 196, 208); "
		"	background-color: rgba(0, 0, 0, 255); "
		"}"
		"QStatusBar::item {"
		"	border: 1px rgba(60, 60, 60, 255);"
		"	border-radius: 0px;"
		"}"
		);

QString	dockStyle = QString::fromUtf8(
		"QDockWidget {"
		"	color: rgba(166, 196, 208, 255);"
		"	border: 1px solid rgba(60, 60, 60, 255);"
		"}"
		//"background-color: rgba(0, 0, 0, 255); "
		"QDockWidget::title {"
		"	text-align: left;"
		//"	background: rgba(60, 60, 60, 255);"
		"	background: qlineargradient(y1:0, y2:1, stop: 0 #a0a0a0, stop: 0.5 #202020, stop: 1 #404040); "
		//"	border: 1px solid rgba(60, 60, 60, 255);"
		//"	border-radius: 2px;"
		"	padding-left: 5px;"
		"}"
		"QDockWidget::close-button, QDockWidget::float-button {"
		"	border: 1px solid transparent;"
		"	background: rgba(166, 196, 208, 255);"
		"	padding: 1px;"
		"}");

QString	comboBoxStyle = QString::fromUtf8(
		"QComboBox { "
		"	color: rgba(166, 196, 208, 255); "
		//"	border: 1px solid rgba(45, 122, 148, 150); "
		"	border: 1px solid rgba(255, 255, 255, 50); "
		"	border-radius: 2px; } "
		"QComboBox::drop-down:!editable { "
		"	color: rgba(166, 196, 208, 255); "
		"	background: qlineargradient(x1: 0, y1: 0, x2: 0, y2: 1, "
		"		stop: 0 #a1a1a1, stop: 0.4 #656565, "
		"		stop: 0.5 #383838, stop: 1.0 #232323); }"
		"QComboBox:on { "
		"	color: rgba(166, 196, 208, 255); "
		"	padding-top: 3px; "
		"	padding-left: 4px; }"
		"QComboBox::drop-down { "
		"	subcontrol-origin: padding; "
		"	subcontrol-position: top right; "
		"	width: 15px; "
		"	border-left-width: 0px; "
		"	border-left-color: darkgray; "
		"	border-left-style: solid; "
		"	border-top-right-radius: 2px;  "
		"	border-bottom-right-radius: 2px; }"
		"QComboBox::down-arrow { "
		"	image: url(:/img/down_arrow.png); "
		"	width: 10px; "
		"	height: 10px; }"
		"QComboBox::down-arrow:on { "
		"	top: 1px; "
		"	left: 1px; }"
		"QComboBox QAbstractItemView { "
		"	color: rgba(166, 196, 208, 255); "
		//"	border: 1px solid rgba(45, 122, 148, 150); "
		"	border: 1px solid rgba(255, 255, 255, 80); "
		"	selection-background-color: rgba(38, 38, 38, 255); }");

QString	spinBoxStyle = QString::fromUtf8(
		"QSpinBox::up-button { "
		"	subcontrol-origin: border; "
		"	subcontrol-position: top left; "
		"	width: 18px; "
		"	height: 10px; "
		"	border-width: 4px; } "
		"QSpinBox::down-button { "
		"	subcontrol-origin: border; "
		"	subcontrol-position: bottom left; "
		"	width: 18px; "
		"	height: 10px; "
		"	border-width: 4px; } "
		"QSpinBox::up-arrow { "
		"	image: url(:/img/up_arrow.png); "
		"	width: 8px; "
		"	height: 8px; } "
		"QSpinBox::down-arrow { "
		"	image: url(:/img/down_arrow.png); "
		"	width: 8px; "
		"	height: 8px; } "
		"QSpinBox::up-arrow:on { "
		"	top: 1px; "
		"	left: 1px; } "
		"QSpinBox::down-arrow:on { "
		"	top: 1px; "
		"	left: 1px; } "
		"QSpinBox { "
		"	color: rgba(166, 196, 208, 255); "
		//"	border: 1px solid rgba(45, 122, 148, 150); } "
		"	border: 1px solid rgba(255, 255, 255, 80); } "
	);

QString	menuStyle = QString::fromUtf8(
		"QMenu { "
		"	background-color: rgba(0, 0, 0, 255); "
		"	border: 1px solid rgba(45, 122, 148, 150); "
		"	color: rgba(220, 220, 220, 255);} "
		"QMenu::item { "
		"	background-color: transparent; } "
		"QMenu::item:selected {  "
		"	background: rgba(45, 122, 148, 200); } "
			/*border: 1px inset rgba(45, 122, 148, 200); \
			position: absolute; \
			top: 1px; \
			right: 1px; \
			bottom: 1px; \
			left: 8px; }"*/
	);

QString	miniButtonStyle = QString::fromUtf8(
		"font-size: 7pt;"
	);
	
QString	sliderLabelStyle = QString::fromUtf8(
		"QLabel { "
		"	color: rgba(126, 156, 168, 255); "
		"	background: qlineargradient(y1:0, y2:1, stop: 0 #808080, stop: 0.5 #202020, stop: 1 #404040); "
		"	border: qlineargradient(y1:0, y2:1, stop:0 #B1B1B1, stop:1 #000000); }"
		);

QString	volSliderStyle = QString::fromUtf8(
		"QSlider::groove:horizontal {"
		"	border: qlineargradient(y1:0, y2:1, stop:0 #B1B1B1, stop:1 #000000);"
		"	background-color: transparent;"
		"}"
		"QSlider::handle:horizontal {"
		"	background: qlineargradient(x1:0, y1:0, x2:0, y2:1, "
		"	stop: 0 #1592ba, stop: 0.5 #00527a, stop: 1 #0582aa);"
		"	border-radius: 2px;"
		"	width: 12px;"
		"} "
		"QSlider::handle:hover {"
		"	background: qlineargradient(x1:0, y1:0, x2:0, y2:1, "
		"	stop: 0 #55c2ea, stop: 0.5 #4092ba, stop: 1 #55c2ea);"
		//"	margin: -4px 0;"
		"	border: 1px solid #91ebff;"
		"	border-radius: 2px;"
		//"	height: 16px;"
		"}"
		"QSlider::handle:pressed {"
		"  background: qlineargradient(x1:0, y1:0, x2:0, y2:1, "
		"	stop: 0 #75e2fa, stop: 0.5 #60a2da, stop: 1 #75e2fa);"
		"  border: 1px solid #91ebff;"
		"  border-radius: 2px;"
		"}"
		"QSlider::add-page:horizontal {"
		"	background: qlineargradient(y1:0, y2:1, "
		"	stop: 0 #808080, stop: 0.5 #202020, stop: 1 #404040);"
		//"	background-color: transparent;"
		"}"
		"QSlider::sub-page:horizontal {"
		"	background: qlineargradient(y1:0, y2:1, "
		"		stop: 0 #1592ba, stop: 0.5 #00527a, stop: 1 #0582aa);"
		//"	background-color: transparent;"
		"}");

QString	splitterStyle = QString::fromUtf8(
		"QSplitter::handle:vertical { "
		"	height: 4px; "
		"	background: rgba(255, 255, 255, 0); } "
		/*"	background: qlineargradient(spread:pad, x1:0, y1:0, x2:1, y2:0, "
		"						stop:0 rgba(255, 255, 255, 0), "
		"						stop:0.3 rgba(85, 210, 250, 105), "
		"						stop:0.7 rgba(85, 210, 250, 105), "
		"						stop:1 rgba(255, 255, 255, 0)); } "*/
		
		"QSplitter::handle:horizontal { "
		"	width: 4px; "
		"	background: rgba(255, 255, 255, 0); } "
		/*"	background: qlineargradient(x1:0, y1:0, x2:1, y2:1, "
		"						stop:0 rgba(255, 255, 255, 0), "
		"						stop:0.3 rgba(85, 210, 250, 105), "
		"						stop:0.7 rgba(85, 210, 250, 105), "
		"						stop:1 rgba(255, 255, 255, 0)); } "*/
		"	border: 0px solid rgba(255, 0, 0, 255); "
		//"	width: 8px; "
		//"	margin-top: 1px; "
		//"	margin-bottom: 1px; "
		//"	border-radius: 1px; "
		//"	width: 3px; } "

		////"QSplitter::handle { "
		////"	image: url(:/img/left.png); } "
		//"QSplitter { "
		//"	padding: 0px; "
		////"	border: 1px solid rgba(60, 60, 60, 255); "
		////"	border-radius: 2px; } "
		//"QSplitter::handle:horizontal { "
		//"	width: 2px; } "
		//"QSplitter::handle:vertical { "
		//"    height: 2px; } ");
	);

QString	frameStyle = QString::fromUtf8(
		//"border: 1px solid rgba(45, 122, 148, 150); "
		"border: 1px solid rgba(255, 255, 255, 80); "
		"border-radius: 2px; margin-top: 2ex; "
	);

QString	tabWidgetStyle = QString::fromUtf8(
		"QTabWidget::pane {"
		"	margin-top: 4px;"
		"	margin-left: 4px;"
		//"	margin-right: 4px;"
		"	margin-bottom: 2px;"
		//"	border: 0px"
		//"	border: 1px solid rgba(255, 255, 255, 80); "
		"	border: 1px solid #565656; "
		"	border-radius: 2px;"
		"}"
		"QTabWidget::tab-bar {"
		"	left: 4px;"
		"	top: 5px;"
		"	right: 4px;"
		"}"
		"QTabBar::tab {"
		"	color: #EEEEEE;"
		"	background: qlineargradient(x1: 0, y1: 0, x2: 0, y2: 1,"
		"									stop: 0 #868686, stop: 0.3 #565656,"
		"									stop: 0.4 #000000, stop: 1.0 #000000);"
		"	border: 1px solid #565656;"
		//"	border-bottom-color: #91EBFF;"
		"	border-bottom-color: #565656;"
		"	border-left-color: #606060;"
		"	border-right-color: #000000;"
		"	border-top-left-radius: 1px;"
		"	border-top-right-radius: 1px;"
		"	min-width: 8ex;"
		"	max-height: 10px;"
		"	padding: 2px;"
		"}"
		"QTabBar::tab:selected {"
		/*"	background: qlineargradient(x1: 0, y1: 0, x2: 0, y2: 1,"
		"								stop: 0 #55D2FA, stop: 0.3 #505050,"
		"								stop: 0.4 #101010, stop: 1.0 #000000);"
		"}"*/
		"	background: qlineargradient(x1: 0, y1: 0, x2: 0, y2: 1,"
		"								stop: 0 #969696, stop: 0.3 #505050,"
		"								stop: 0.4 #101010, stop: 1.0 #000000);"
		"}"
		"QTabBar::tab:hover {"
		/*"	background: qlineargradient(x1: 0, y1: 0, x2: 0, y2: 1,"
		"								stop: 0 #91EBFF, stop: 0.4 #000000,"
		"								stop: 0.8 #000000, stop: 1.0 #91EBFF);"*/
		"	background: qlineargradient(x1: 0, y1: 0, x2: 0, y2: 1,"
		"								stop: 0 #C6C6C6, stop: 0.3 #505050,"
		"								stop: 0.4 #202020, stop: 1.0 #000000);"
		//"	border-top-color: #91EBFF;"
		//"	border-left-color: #61CBDF;"
		//"	border-right-color: #91EBFF;"
		//"	border-color: #91EBFF;"
		//"	border-color: #969696;"
		//"	border-bottom-color: #000000;"
		"}"
		"QTabBar::tab:selected {"
		//"	border-color: #91EBFF;"
		"	border-color: #565656;"
		"	border-right-color: #000000;"
		"	border-bottom-color: #000000;"
		"}"
		"QTabBar::tab:!selected {"
		"	margin-top: 2px;"
		"}"
	);

#endif // CUSDR_STYLES_H
