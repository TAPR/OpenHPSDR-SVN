/**
* @file  cusdr_oglWidget.cpp
* @brief OpenGL widget class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-01-13
*/

/*
 *   Copyright 2011 Hermann von Hasseln, DL3HVH
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
#define LOG_GRAPHICS

#include "cusdr_oglWidget.h"

#include <QtGui>
#include <QDebug>
#include <QTimer>
#include <QString>

OGLWidget::OGLWidget(QWidget *parent)
		: QWidget(parent)

		, m_settings(Settings::instance())
		, m_serverMode(m_settings->getCurrentServerMode())
		, m_dataEngineState(QSDR::DataEngineDown)
		, m_numberOfReceivers(m_settings->getNumberOfReceivers())
		, m_multiRxView(m_settings->getMultiRxView())
		, m_widebandPanel(false)
		, m_wideBandStatus(m_settings->getWideBandStatus())
		, m_minWidebandDisplayHeight(120)
{
	setAutoFillBackground(true);

	QPalette palette;
	QColor color = Qt::black;
	color.setAlpha(255);
	palette.setColor(QPalette::Window, color);
	setPalette(palette);

	m_wbDisplay = 0;
	m_distDisplay = 0;

	if (m_serverMode == QSDR::DttSP || m_serverMode == QSDR::ChirpWSPR)
		m_settings->setSpectrumSize(4096);
	else if (m_serverMode == QSDR::QtDSP)
		m_settings->setSpectrumSize(2048);

	if (m_settings->getWideBandData())
		initWidebandDisplay();

	initReceiverPanels(m_numberOfReceivers);
	initReceiverDisplays(m_numberOfReceivers, m_multiRxView);

	setupConnections();
	initLayout(m_multiRxView);
}

OGLWidget::~OGLWidget() {

	if (m_wbDisplay) {
					
		delete m_wbDisplay;
		m_wbDisplay = 0;
	}

	if (m_distDisplay) {

		delete m_distDisplay;
		m_distDisplay = 0;
	}

	for (int i = 0; i < m_numberOfReceivers; i++) {

		delete m_rxWidgetList.at(i);
		delete m_rxBoxLayoutList.at(i);
	}
	m_rxWidgetList.clear();
	m_rxBoxLayoutList.clear();
}

QSize OGLWidget::minimumSizeHint() const {
	
	return QSize(1000, 1024);
}

QSize OGLWidget::sizeHint() const {
	
	return QSize(width(), height());
}

void OGLWidget::setupConnections() {

	CHECKED_CONNECT(
		m_settings, 
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
		m_settings, 
		SIGNAL(numberOfRXChanged(QObject *, int)), 
		this, 
		SLOT(setNumberOfReceivers(QObject *, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(multiRxViewChanged(int)), 
		this, 
		SLOT(setMultiRxView(int)));

	CHECKED_CONNECT(
		m_settings,
		SIGNAL(spectrumBufferChanged(int, const float*)),
		this,
		SLOT(setSpectrumBuffer(int, const float*)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(frequencyChanged(QObject *, bool, int, long)), 
		this, 
		SLOT(setFrequency(QObject *, bool, int, long)));

	CHECKED_CONNECT(
		m_settings,
		SIGNAL(wideBandStausChanged(bool)),
		this,
		SLOT(setWideBandStatus(bool)));
}

void OGLWidget::systemStateChanged(
	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	Q_UNUSED (sender)
	Q_UNUSED (err)
	Q_UNUSED (hwmode)
	Q_UNUSED (state)

	if (m_dataEngineState != state) {
		
		m_dataEngineState = state;
	}

	if (m_serverMode != mode) {

		switch (mode) {

			//case QSDR::InternalDSP:
			case QSDR::DttSP:
			case QSDR::QtDSP:

				if (m_wbDisplay) {
					
					delete m_wbDisplay;
					m_wbDisplay = 0;
				}

				if (m_distDisplay) {

					delete m_distDisplay;
					m_distDisplay = 0;
				}

				if (m_settings->getWideBandData())
					initWidebandDisplay();
				
				initReceiverPanels(m_numberOfReceivers);
				initReceiverDisplays(m_numberOfReceivers, m_multiRxView);
				initLayout(m_multiRxView);

				break;

			case QSDR::ChirpWSPR:

				if (m_wbDisplay) {
					
					delete m_wbDisplay;
					m_wbDisplay = 0;
				}

				if (m_distDisplay) {

					delete m_distDisplay;
					m_distDisplay = 0;
				}

				for (int i = 0; i < m_numberOfReceivers; i++) {

					delete m_rxWidgetList.at(i);
					delete m_rxBoxLayoutList.at(i);
				}
				m_rxWidgetList.clear();
				m_rxBoxLayoutList.clear();

				initDistanceDisplay();
				initLayout(0);

				break;

			case QSDR::NoServerMode:
				break;
		}
		m_serverMode = mode;
	}
	update();
}

void OGLWidget::initWidebandDisplay() {

	if (m_wbDisplay) {

		delete m_wbDisplay;
		m_wbDisplay = 0;
	}
	
	m_wbDisplay = new QGLWidebandPanel(this);

	QHBoxLayout *gbox = new QHBoxLayout();
	gbox->setSpacing(0);
	gbox->setMargin(2);
	gbox->addWidget(m_wbDisplay);

	QFrame *frame = new QFrame(this);
	frame->setStyleSheet(m_settings->getFrameStyle());
	frame->setLayout(gbox);

	m_widebandSplitter = new QSplitter(Qt::Horizontal, this);
	m_widebandSplitter->setOpaqueResize(false);
	m_widebandSplitter->setMinimumHeight(m_minWidebandDisplayHeight);
	m_widebandSplitter->setStyleSheet(m_settings->getSplitterStyle());
	m_widebandSplitter->addWidget(frame);

	m_widebandPanel = true;
}

void OGLWidget::initDistanceDisplay() {

	if (m_distDisplay) {

		delete m_distDisplay;
		m_distDisplay = 0;
	}

	QFrame *frame = new QFrame(this);
	frame->setFrameShape(QFrame::NoFrame);
	
	m_distDisplay = new QGLDistancePanel(this);
	
	QHBoxLayout *gbox = new QHBoxLayout();
	gbox->setSpacing(0);
	gbox->setMargin(2);
	gbox->addWidget(m_distDisplay);

	m_topSplitter = new QSplitter(Qt::Vertical, this);
	m_topSplitter->setOpaqueResize(false);
	m_topSplitter->setStyleSheet(m_settings->getSplitterStyle());

	//QFrame *frame = new QFrame(this);
	//frame->setFrameShape(QFrame::NoFrame);
	frame->setLayout(gbox);

	m_topSplitter->addWidget(frame);
}

void OGLWidget::initReceiverPanels(int rx) {

	m_rxWidgetList.clear();
	m_rxBoxLayoutList.clear();

	for (int i = 0; i < rx; i++) {
	
		QGLReceiverPanel *rxPanel = new QGLReceiverPanel(this, i);
		m_rxWidgetList.append(rxPanel);

		QHBoxLayout *gbox = new QHBoxLayout();
		gbox->setSpacing(0);
		gbox->setMargin(2);
		gbox->addWidget(rxPanel);
		m_rxBoxLayoutList.append(gbox);
    }
}

void OGLWidget::initReceiverDisplays(int rx, int view) {

	switch (view) {

		case 0:

			m_topSplitter = new QSplitter(Qt::Vertical, this);
			m_topSplitter->setOpaqueResize(false);
			m_topSplitter->setStyleSheet(m_settings->getSplitterStyle());
			
			if (rx > 1) {
		
				m_bottomSplitter = new QSplitter(Qt::Horizontal, this);
				m_bottomSplitter->setOpaqueResize(false);
				m_bottomSplitter->setStyleSheet(m_settings->getSplitterStyle());
				
				for (int i = 1; i < rx; i++) {

					QFrame *frame = new QFrame(this);
					frame->setStyleSheet(m_settings->getFrameStyle());
					frame->setLayout(m_rxBoxLayoutList.at(i));

					m_bottomSplitter->addWidget(frame);
				}

				QFrame *frame = new QFrame(this);
				frame->setStyleSheet(m_settings->getFrameStyle());
				frame->setLayout(m_rxBoxLayoutList.at(0));

				if (m_widebandPanel && m_wideBandStatus)
					m_topSplitter->addWidget(m_widebandSplitter);

				m_topSplitter->addWidget(frame);
				m_topSplitter->addWidget(m_bottomSplitter);
			}
			else {

				QFrame *frame = new QFrame(this);
				frame->setFrameShape(QFrame::NoFrame);
				frame->setLayout(m_rxBoxLayoutList.at(rx-1));

				if (m_widebandPanel && m_wideBandStatus) {

					frame->setStyleSheet(m_settings->getFrameStyle());
					m_topSplitter->addWidget(m_widebandSplitter);
				}
				m_topSplitter->addWidget(frame);
			}
			break;

		case 1:

			m_bottomSplitter = new QSplitter(Qt::Vertical, this);
			m_bottomSplitter->setOpaqueResize(false);
			m_bottomSplitter->setStyleSheet(m_settings->getSplitterStyle());

			if (rx > 1) {
		
				m_topSplitter = new QSplitter(Qt::Horizontal, this);
				m_topSplitter->setOpaqueResize(false);
				m_topSplitter->setStyleSheet(m_settings->getSplitterStyle());

				for (int i = 0; i < rx - 1; i++) {

					QFrame *frame = new QFrame(this);
					frame->setStyleSheet(m_settings->getFrameStyle());
					frame->setLayout(m_rxBoxLayoutList.at(i));

					m_topSplitter->addWidget(frame);
				}

				QFrame *frame = new QFrame(this);
				//frame->setFrameShape(QFrame::NoFrame);
				frame->setStyleSheet(m_settings->getFrameStyle());
				frame->setLayout(m_rxBoxLayoutList.at(rx-1));

				if (m_widebandPanel && m_wideBandStatus)
					m_bottomSplitter->addWidget(m_widebandSplitter);

				m_bottomSplitter->addWidget(m_topSplitter);
				m_bottomSplitter->addWidget(frame);
			}
			else {

				QFrame *frame = new QFrame(this);
				frame->setFrameShape(QFrame::NoFrame);
				frame->setLayout(m_rxBoxLayoutList.at(rx-1));

				if (m_widebandPanel && m_wideBandStatus) {

					frame->setStyleSheet(m_settings->getFrameStyle());
					//m_bottomSplitter->addWidget(m_widebandSplitter);
					m_topSplitter->addWidget(m_widebandSplitter);
				}
				m_bottomSplitter->addWidget(frame);
			}
			break;

		case 2:

			break;
	}
}

//void OGLWidget::initReceiverDisplaysDefault(int rx) {
//
//	m_rxWidgetList.clear();
//	m_rxBoxLayoutList.clear();
//
//	for (int i = 0; i < rx; i++) {
//	
//		QGLReceiverPanel *rxPanel = new QGLReceiverPanel(this, i);
//		m_rxWidgetList.append(rxPanel);
//
//		QHBoxLayout *gbox = new QHBoxLayout();
//		gbox->setSpacing(0);
//		gbox->setMargin(2);
//		gbox->addWidget(rxPanel);
//		m_rxBoxLayoutList.append(gbox);
//    }
//
//	m_topSplitter = new QSplitter(Qt::Vertical, this);
//	m_topSplitter->setOpaqueResize(false);
//	m_topSplitter->setStyleSheet(m_settings->getSplitterStyle());
//	
//	if (rx > 1) {
//		
//		m_bottomSplitter = new QSplitter(Qt::Horizontal, this);
//		m_bottomSplitter->setOpaqueResize(false);
//		m_bottomSplitter->setStyleSheet(m_settings->getSplitterStyle());
//
//		for (int i = 1; i < rx; i++) {
//
//			QFrame *frame = new QFrame(this);
//			//frame->setFrameShape(QFrame::StyledPanel);
//			frame->setStyleSheet(m_settings->getFrameStyle());
//			frame->setLayout(m_rxBoxLayoutList.at(i));
//
//			m_bottomSplitter->addWidget(frame);
//		}
//
//		QFrame *frame = new QFrame(this);
//		//frame->setFrameShape(QFrame::StyledPanel);
//		frame->setStyleSheet(m_settings->getFrameStyle());
//		frame->setLayout(m_rxBoxLayoutList.at(0));
//
//		m_topSplitter->addWidget(frame);
//		m_topSplitter->addWidget(m_bottomSplitter);
//		
//	}
//	else {
//
//		QFrame *frame = new QFrame(this);
//		frame->setFrameShape(QFrame::NoFrame);
//		frame->setLayout(m_rxBoxLayoutList.at(rx-1));
//
//		m_topSplitter->addWidget(frame);
//	}
//}

void OGLWidget::initLayout(int view) {

	delete layout();

	QHBoxLayout *hbox = new QHBoxLayout(this);
	hbox->setSpacing(0);
	hbox->setMargin(0);

	if (view == 0)
		hbox->addWidget(m_topSplitter);
	else
		hbox->addWidget(m_bottomSplitter);
	
	setLayout(hbox);
}

void OGLWidget::setNumberOfReceivers(QObject* sender, int value) {

	Q_UNUSED (sender)

	delete layout();

	if (m_wbDisplay) {
		
		delete m_wbDisplay;
		m_wbDisplay = 0;
	}

	if (m_widebandSplitter != 0)
		delete m_widebandSplitter;

	if (m_settings->getWideBandData())
		initWidebandDisplay();

	for (int i = 0; i < m_numberOfReceivers; i++) {

		delete m_rxWidgetList.at(i);
		delete m_rxBoxLayoutList.at(i);
	}
	m_rxWidgetList.clear();
	m_rxBoxLayoutList.clear();

	if (m_numberOfReceivers == value) return;
	m_numberOfReceivers = value;

	initReceiverPanels(m_numberOfReceivers);
	initReceiverDisplays(m_numberOfReceivers, m_multiRxView);
	initLayout(m_multiRxView);
}

void OGLWidget::setMultiRxView(int view) {

	m_multiRxView = view;

	setNumberOfReceivers(this, m_numberOfReceivers);
	initReceiverPanels(m_numberOfReceivers);
	initReceiverDisplays(m_numberOfReceivers, m_multiRxView);
	initLayout(m_multiRxView);
	
}

void OGLWidget::setSpectrumBuffer(int rx, const float *buffer) {

	//if (m_serverMode == QSDR::InternalDSP && !m_rxWidgetList.empty())
	if ((m_serverMode == QSDR::DttSP || m_serverMode == QSDR::QtDSP) && !m_rxWidgetList.empty())
		m_rxWidgetList.at(rx)->setSpectrumBuffer(buffer);
	else if ((m_serverMode == QSDR::ChirpWSPR || m_serverMode == QSDR::ChirpWSPRFile) && m_distDisplay != 0)
		m_distDisplay->setSpectrumBuffer(buffer);
}

void OGLWidget::setFrequency(QObject* sender, bool value, int rx, long freq) {

	Q_UNUSED(sender)
	Q_UNUSED(value)
	
	//if (m_serverMode == QSDR::InternalDSP && !m_rxWidgetList.empty())
	if ((m_serverMode == QSDR::DttSP || m_serverMode == QSDR::QtDSP) && !m_rxWidgetList.empty())
		//m_rxWidgetList.at(rx)->setFrequency(this, true, freq);
		m_rxWidgetList.at(rx)->setFrequency(this, true, rx, freq);
	//else if (m_serverMode == QSDR::InternalDSP && m_distDisplay != 0)
	else if ((m_serverMode == QSDR::DttSP || m_serverMode == QSDR::QtDSP) && m_distDisplay != 0)
		m_distDisplay->setFrequency(this, true, freq);
}

void OGLWidget::setWideBandStatus(bool value) {

	m_wideBandStatus = value;

	if (m_wideBandStatus)
		initWidebandDisplay();

	initReceiverDisplays(m_numberOfReceivers, m_multiRxView);
	initLayout(m_multiRxView);
}

void OGLWidget::resizeEvent(QResizeEvent *event) {

	QWidget::resizeEvent(event);
}

void OGLWidget::paintEvent(QPaintEvent* event) {

	//Q_UNUSED(event)

	/*setAutoFillBackground(true);

	QPalette palette;
	QColor color = Qt::black;
	color.setAlpha(255);
	palette.setColor(QPalette::Window, color);
	setPalette(palette);*/

	QWidget::paintEvent(event);
	/*QPainter p(this);
	p.end();

	if (m_displayTime.elapsed() >= 100) {
		QWidget::paintEvent(event);
	}*/
}