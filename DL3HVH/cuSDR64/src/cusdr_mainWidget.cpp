/**
* @file  cusdr_main.cpp
* @brief main window class for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-01-06
*/

/*   
 *   Copyright 2010, 2011, 2012 Hermann von Hasseln, DL3HVH
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

#define LOG_MAIN
#define LOG_NETWORKDIALOG

#include <QtGui>
#include <QHBoxLayout>
#include <QVBoxLayout>
#include <QtNetwork>
#include <QTimer>

#include "cusdr_mainWidget.h"


#pragma comment(lib, "User32.lib")

#ifdef Q_OS_X11
#include <private/qt_x11_p.h>
#else ifdef Q_OS_WIN
#include "qt_windows.h"
#endif
#ifdef Q_OS_MAC
#include <private/qt_mac_p.h>
#endif

#define window_height1		56
#define window_height2		600
#define window_width		800
#define btn_width1			65
#define btn_width2			54
#define btn_height1			17
#define btn_height2			13
#define btn_height3			16



/*!
	\class MainWindow
	\brief This class implements main window of the application.
*/
/*!
	\brief Creates a new #MainWindow with the given \a parent.
	This function does following steps:
	- set up GUI.
	- set up connections.
	- find network connections.
*/
MainWindow::MainWindow(QWidget *parent)
	: QWidget(parent)
	, m_settings(Settings::instance())
	, m_serverMode(m_settings->getCurrentServerMode())
	, m_hwInterface(m_settings->getHWInterface())
	, m_dataEngineState(QSDR::DataEngineDown)
	, m_fullScreen(false)
	, m_modeMenu(new QMenu(this))
	, m_cudaPresence(false)
	, m_mover(false)
	, m_resizePosition(0)
	, m_oldSampleRate(m_settings->getSampleRate())
{	
	QPalette palette;
	QColor color = Qt::black;
	color.setAlpha(255);
	palette.setColor(QPalette::Window, color);
	setPalette(palette);

	m_smallFont.setStyleStrategy(QFont::PreferAntialias);
	m_smallFont.setBold(true);
	m_smallFont.setFixedPitch(true);
	#ifdef Q_OS_MAC
		m_smallFont.setPixelSize(10);
		m_smallFont.setFamily("Arial");
	#else if Q_OS_WIN
		m_smallFont.setPixelSize(10);
		m_smallFont.setFamily("Arial");
	#endif

	m_dataEngine = new DataEngine(this);
	m_hpsdrServer = new HPSDRServer(this);
	m_oglDisplayPanel = new OGLDisplayPanel(this);
	m_graphicOptionsWidget = new GraphicOptionsWidget(this);
	m_radioWidget = new RadioWidget(this);
	m_serverWidget = new ServerWidget(this);
	m_chirpWidget = new ChirpWidget(this);
	//m_hpsdrWidget = new HPSDRWidget(this);
	m_hpsdrTabWidget = new HPSDRTabWidget(this);
	//m_alexWidget = new AlexWidget(this);
	//m_pennyWidget = new PennyWidget();
	
	setAutoFillBackground(true);
	setMouseTracking(true);

	setContentsMargins(0, 0, 0, 0);

	m_radioWidget->hide();
	m_serverWidget->hide();
	m_chirpWidget->hide();
	m_hpsdrTabWidget->hide();
	m_graphicOptionsWidget->hide();

	m_resizeTimer = new QTimer(this);
	
	m_oglWidget = new OGLWidget(this);

	// we collect the widgets in the widgetList
	widgetList.append(m_radioWidget);	
	widgetList.append(m_serverWidget);
	widgetList.append(m_hpsdrTabWidget);
	widgetList.append(m_chirpWidget);
	widgetList.append(m_graphicOptionsWidget);
	updateTitle();

	MAIN_DEBUG << "main window init done";
}

/*!
	\brief MainWindow Destructor
*/
MainWindow::~MainWindow() {
}

 
/*!
	\brief set up connections.
*/
void MainWindow::setupConnections() {

	CHECKED_CONNECT(
		m_dataEngine, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(showMessage(QString)));

	//if (m_graphicsWidget) {

	//	CHECKED_CONNECT(
	//		m_dataEngine,
	//		SIGNAL(spectrumBufferChanged(const float*)),
	//		m_graphicsWidget,
	//		SLOT(setSpectrumBuffer(const float*)));

	//	CHECKED_CONNECT(
	//		m_dataEngine,
	//		SIGNAL(wbSpectrumBufferChanged(const float*)),
	//	m_graphicsWidget,
	//	SLOT(wbSpectrumBufferChanged(const float*)));
	//	//Qt::DirectConnection);
	//}

	//*******************************************

	CHECKED_CONNECT(
		m_hpsdrServer, 
		SIGNAL(masterSwitchEvent()), 
		this, 
		SLOT(startButtonClickedEvent()));

	CHECKED_CONNECT(
		m_hpsdrServer, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(showMessage(QString)));

	CHECKED_CONNECT(
		m_radioWidget, 
		SIGNAL(showEvent(QObject *)), 
		this, 
		SLOT(showWidgetEvent(QObject *)));

	CHECKED_CONNECT(
		m_radioWidget, 
		SIGNAL(closeEvent(QObject *)), 
		this, 
		SLOT(closeWidgetEvent(QObject *)));

	CHECKED_CONNECT(
		m_serverWidget, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(showMessage(QString)));

	CHECKED_CONNECT(
		m_serverWidget, 
		SIGNAL(showEvent(QObject *)), 
		this, 
		SLOT(showWidgetEvent(QObject *)));

	CHECKED_CONNECT(
		m_serverWidget, 
		SIGNAL(closeEvent(QObject *)), 
		this, 
		SLOT(closeWidgetEvent(QObject *)));

	CHECKED_CONNECT(
		m_chirpWidget, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(showMessage(QString)));

	CHECKED_CONNECT(
		m_chirpWidget, 
		SIGNAL(loadFileEvent(QObject *, const QString)), 
		this, 
		SLOT(loadWavFile(QObject *, const QString)));

	CHECKED_CONNECT(
		m_chirpWidget, 
		SIGNAL(suspendSignal(QObject *)), 
		this, 
		SLOT(suspendSignal(QObject *)));

	CHECKED_CONNECT(
		m_chirpWidget, 
		SIGNAL(startPlaybackSignal(QObject *)), 
		this, 
		SLOT(startPlaybackSignal(QObject *)));

	CHECKED_CONNECT(
		m_chirpWidget, 
		SIGNAL(showSettingsDialogSignal(QObject *)), 
		this, 
		SLOT(showSettingsDialogSignal(QObject *)));

	/*CHECKED_CONNECT(
		m_hpsdrWidget, 
		SIGNAL(showEvent(QObject *)), 
		this, 
		SLOT(showWidgetEvent(QObject *)));*/

	CHECKED_CONNECT(
		m_hpsdrTabWidget, 
		SIGNAL(showEvent(QObject *)), 
		this, 
		SLOT(showWidgetEvent(QObject *)));

	/*CHECKED_CONNECT(
		m_hpsdrWidget, 
		SIGNAL(closeEvent(QObject *)), 
		this, 
		SLOT(closeWidgetEvent(QObject *)));

	CHECKED_CONNECT(
		m_hpsdrWidget, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(showMessage(QString)));*/

	CHECKED_CONNECT(
		m_hpsdrTabWidget, 
		SIGNAL(closeEvent(QObject *)), 
		this, 
		SLOT(closeWidgetEvent(QObject *)));

	CHECKED_CONNECT(
		m_hpsdrTabWidget, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(showMessage(QString)));

	/*if (m_cudaPresence) {
	
		CHECKED_CONNECT(
			m_cudaInfoWidget, 
			SIGNAL(showEvent(QObject *)), 
			this, 
			SLOT(showWidgetEvent(QObject *)));

		CHECKED_CONNECT(
			m_cudaInfoWidget, 
			SIGNAL(closeEvent(QObject *)), 
			this, 
			SLOT(closeWidgetEvent(QObject *)));
	}*/
	
	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(showMessage(QString)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(masterSwitchChanged(QObject *, bool)), 
		this, 
		SLOT(masterSwitchChanged(QObject *, bool)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(numberOfRXChanged(QObject *, int)), 
		this, 
		SLOT(setNumberOfReceivers(QObject *, int)));

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
		SIGNAL(receiverChanged(int)),
		this,
		SLOT(setReceiver(int)));

	/*CHECKED_CONNECT(
		m_settings, 
		SIGNAL(alexPresenceChanged(bool)),
		this,
		SLOT(setAlexPresence(bool)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(penelopePresenceChanged(bool)),
		this,
		SLOT(setPennyPresence(bool)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(pennyLanePresenceChanged(bool)),
		this,
		SLOT(setPennyPresence(bool)));*/

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(showNetworkIO()),
		this,
		SLOT(showNetworkIODialog()));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(networkIOComboBoxEntryAdded(QString)),
		this,
		SLOT(addNetworkIOComboBoxEntry(QString)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(clearNetworkIOComboBoxEntrySignal()),
		this,
		SLOT(clearNetworkIOComboBoxEntry()));

	CHECKED_CONNECT(
		m_graphicOptionsWidget, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(showMessage(QString)));
}

 
/*!
	\brief setup the main window:
	- create main button group
	- create console widget
	- set up main layout
	- create mode menu
	- set up connections
	- get network interfaces
*/
void MainWindow::setup() {

	this->setMinimumSize(QSize(window_width, window_height2));
	
	//QIcon icon;
	//icon.addFile(QString::fromUtf8("cuSDR_big.ico"), QSize(), QIcon::Normal, QIcon::Off);
	//this->setWindowIcon(icon);
	this->setStyleSheet(m_settings->getSDRStyle());
	
	//m_alpha = 255;
	//m_widgetMask = QPixmap(this->width(), this->height());

	// create button groups
	createMainBtnGroup();
	createSecondBtnGroup();
	
	// create console widget
	createConsoleWidget();

	// setup main layout
	setupLayout();

	// create server mode menu
	createModeMenu();

	initialMessage();
	messagesBtnClickedEvent();

	if (m_settings->getWideBandStatus())
		wideBandBtnClickedEvent();

	m_quitHighBotton = false;
	m_resizeFrame = true;

	setupConnections();
	updateFromSettings();

	// get network interfaces
	getNetworkInterfaces();

	// init network IO dialog to HPSDR components
	m_netIODialog = new NetworkIODialog();

	// setup connection for the NIC lists of the server and hpsdr widgets.
	// We need to add these connections after detecting the network interfaces.
	m_serverWidget->addNICChangedConnection();
	m_hpsdrTabWidget->addNICChangedConnection();
	
	// check for OpenCL devices
	//QList<QCLDevice> clDevices = QCLDevice::allDevices();
	//if (clDevices.length() == 0)
	//	showMessage("[main]: no OpenCL devices found.");

	//else {

	//	m_message = "[main]: found %1 OpenCL device(s).";
	//	showMessage(m_message.arg(clDevices.length()));
	//	//QString clNo = QString::number(m_clDevices.length());
	//}
	//m_settings->setOpenCLDevices(clDevices);

	
	//m_tlw = this;
	setNumberOfReceivers(this, 1);

	m_oglDisplayPanel->updateGL();
}

 
/*!
	\brief updates the OpenGL widget.
*/
void MainWindow::update() {

	if (m_oglWidget)
		m_oglWidget->update();
}

 
/*!
	\brief set up the main layout with borders.
*/
void MainWindow::setupLayout() {

	/*m_topBorder = new FrameWidget(this);
	m_topBorder->setEdge(FrameWidget::Top);
	
	m_leftBorder = new FrameWidget(this);
	m_leftBorder->setEdge(FrameWidget::Left);
	
	m_rightBorder = new FrameWidget(this);
	m_rightBorder->setEdge(FrameWidget::Right);

	m_leftTopAngleBorder = new FrameWidget(this);
	m_leftTopAngleBorder->setEdge(FrameWidget::TopLeft);
	
	m_leftBottomAngleBorder = new FrameWidget(this);
	m_leftBottomAngleBorder->setEdge(FrameWidget::BottomLeft);
	
	m_rightTopAngleBorder  = new FrameWidget(this);
	m_rightTopAngleBorder->setEdge(FrameWidget::TopRight);
	
	m_rightBottomAngleBorder = new FrameWidget(this);
	m_rightBottomAngleBorder->setEdge(FrameWidget::BottomRight);
	
	m_bottomBorder = new FrameWidget(this);
	m_bottomBorder->setEdge(FrameWidget::Bottom);*/
	
	// main content
	m_contentLayout = new QGridLayout(this);
	//m_contentLayout->setVerticalSpacing(3);
	m_contentLayout->setVerticalSpacing(0);
	m_contentLayout->setHorizontalSpacing(0);
	//setContentsMargins(0, 0, 4, 0);
	//m_contentLayout->setContentsMargins(1, 1, 1, 1);
	m_contentLayout->setContentsMargins(2, 0, 2, 0);
	m_contentLayout->addWidget(m_oglDisplayPanel, 0, 0, 1, 2, Qt::AlignTop);
	m_contentLayout->addWidget(m_buttonWidget, 1, 0, 1, 2, Qt::AlignTop);
	m_contentLayout->addWidget(m_secondButtonWidget, 2, 0, 1, 2, Qt::AlignTop);
	//setContentsMargins(0, 0, 6, 0);
	m_contentLayout->addWidget(m_oglWidget, 3, 0, 1, 1);
	m_contentLayout->addWidget(m_radioWidget, 3, 1, 2, 1);
	m_contentLayout->addWidget(m_serverWidget, 3, 1, 2, 1);
	m_contentLayout->addWidget(m_hpsdrTabWidget, 3, 1, 2, 1);
	m_contentLayout->addWidget(m_chirpWidget, 3, 1, 2, 1);
	//if (m_cudaPresence)
	//	m_contentLayout->addWidget(m_cudaInfoWidget, 3, 1, 2, 1);
	m_contentLayout->addWidget(m_graphicOptionsWidget, 3, 1, 2, 1);
	m_contentLayout->addWidget(m_msgBrowser, 4, 0, 1, 1);
	m_contentLayout->setColumnStretch(0, 2);

	//m_innerWidget->setContentsMargins(0, 0, 0, 0);
	//m_innerWidget->setLayout(m_contentLayout);

	/*QGridLayout *mainLayout = new QGridLayout(this);
	mainLayout->setVerticalSpacing(0);
	mainLayout->setHorizontalSpacing(0);
	mainLayout->setContentsMargins (0, 0, 0, 0);
	mainLayout->setRowMinimumHeight(3, 20);
	mainLayout->setRowStretch(2, 3);
	mainLayout->setColumnStretch(1, 3);

	mainLayout->addWidget(m_titlebar, 0, 0, 1, 3);
	mainLayout->addWidget(m_leftTopAngleBorder, 1, 0);
	mainLayout->addWidget(m_topBorder, 1, 1);
	mainLayout->addWidget(m_rightTopAngleBorder, 1, 2);

	mainLayout->addWidget(m_leftBorder, 2, 0);
	mainLayout->addWidget(m_innerWidget, 2, 1);
	mainLayout->addWidget(m_dummyLabel, 2, 1);
	mainLayout->addWidget(m_rightBorder, 2, 2);
	mainLayout->addWidget(m_leftBottomAngleBorder, 3, 0);
	mainLayout->addWidget(m_bottomBorder, 3, 1);
	mainLayout->addWidget(m_rightBottomAngleBorder, 3, 2);
	
	setLayout(mainLayout);*/

	setLayout(m_contentLayout);
}

 
/*!
	\brief create the console widget.
*/
void MainWindow::createConsoleWidget() {

	QFont browserFont;
	//browserFont.setStyleStrategy(QFont::PreferAntialias);
	browserFont.setFixedPitch(true);
	browserFont.setPixelSize(11);
	browserFont.setFamily("Arial");

	m_msgBrowser = new QPlainTextEdit(this);
	m_msgBrowser->setReadOnly(true);
	m_msgBrowser->setLineWrapMode(QPlainTextEdit::NoWrap);
	m_msgBrowser->setFont(browserFont);
	m_msgBrowser->setStyleSheet(m_settings->getMessageBoxStyle());
}

 
/*!
	\brief create the main button group.
*/
void MainWindow::createMainBtnGroup() {

	m_buttonWidget = new QWidget(this);

	QHBoxLayout *mainBtnLayout = new QHBoxLayout(this);
	mainBtnLayout->setSpacing(0);
	mainBtnLayout->setMargin(0);
	
	startBtn = new AeroButton("Start", this);
	startBtn->setRoundness(10);
	startBtn->setFixedSize(btn_width1, btn_height1);

	CHECKED_CONNECT(
		startBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(startButtonClickedEvent()));

	ctrlDisplayBtn = new AeroButton("Radio", this);
	ctrlDisplayBtn->setRoundness(10);
	ctrlDisplayBtn->setFixedSize(btn_width1, btn_height1);
	ctrlDisplayBtn->setBtnState(AeroButton::ON);

	CHECKED_CONNECT(
		ctrlDisplayBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(ctrlDisplayBtnClickedEvent()));

	serverLogBtn = new AeroButton("Log", this);
	serverLogBtn->setRoundness(10);
	serverLogBtn->setFixedSize(btn_width1, btn_height1);

	CHECKED_CONNECT(
		serverLogBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(messagesBtnClickedEvent()));

	rxCtrlBtn = new AeroButton("Rx Ctrl", this);
	rxCtrlBtn->setRoundness(10);
	rxCtrlBtn->setFixedSize(btn_width1, btn_height1);
	mainBtnList.append(rxCtrlBtn);

	if (m_serverMode == QSDR::ExternalDSP)
		rxCtrlBtn->setEnabled(false);

	CHECKED_CONNECT(
		rxCtrlBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(widgetBtnClickedEvent()));

	serverBtn = new AeroButton("Server", this);
	serverBtn->setRoundness(10);
	serverBtn->setFixedSize(btn_width1, btn_height1);
	mainBtnList.append(serverBtn);

	CHECKED_CONNECT(
		serverBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(widgetBtnClickedEvent()));

	hpsdrBtn = new AeroButton("HPSDR", this);
	hpsdrBtn->setRoundness(10);
	hpsdrBtn->setFixedSize(btn_width1, btn_height1);
	mainBtnList.append(hpsdrBtn);

	CHECKED_CONNECT(
		hpsdrBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(widgetBtnClickedEvent()));

	/*alexBtn = new AeroButton("Alex", this);
	alexBtn->setRoundness(10);
	alexBtn->setFixedSize(btn_width1, btn_height1);
	mainBtnList.append(alexBtn);

	if (!m_settings->getAlexPresence()) {

		alexBtn->setEnabled(false);
		alexBtn->hide();
	}

	CHECKED_CONNECT(
		alexBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(widgetBtnClickedEvent()));*/

	/*pennyBtn = new AeroButton("Penelope", this);
	pennyBtn->setRoundness(10);
	pennyBtn->setFixedSize(btn_width1, btn_height1);
	mainBtnList.append(pennyBtn);

	if (!m_settings->getPenelopePresence() && !m_settings->getPennyLanePresence()) {

		pennyBtn->setEnabled(false);
		pennyBtn->hide();
	}

	CHECKED_CONNECT(
		pennyBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(widgetBtnClickedEvent()));*/

	chirpBtn = new AeroButton("Chirp", this);
	chirpBtn->setRoundness(10);
	chirpBtn->setFixedSize(btn_width1, btn_height1);
	mainBtnList.append(chirpBtn);

	if (m_serverMode != QSDR::ChirpWSPR)
		chirpBtn->setEnabled(false);

	CHECKED_CONNECT(
		chirpBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(widgetBtnClickedEvent()));

	wideBandBtn = new AeroButton("Wideband", this);
	wideBandBtn->setRoundness(10);
	wideBandBtn->setFixedSize(btn_width1, btn_height1);
	wideBandBtn->setEnabled(false);
	//mainBtnList.append(wideBandBtn);

	if (m_serverMode == QSDR::ChirpWSPR)
		wideBandBtn->setEnabled(false);

	CHECKED_CONNECT(
		wideBandBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(wideBandBtnClickedEvent()));

	//openclBtn = new AeroButton("OpenCL", this);
	//openclBtn->setRoundness(10);
	//openclBtn->setFixedSize(btn_width1, btn_height1);
	//mainBtnList.append(openclBtn);

	//if (m_cudaPresence) {
	//	
	//	mainBtnList.append(cudaBtn);

	//	CHECKED_CONNECT(
	//	cudaBtn, 
	//	SIGNAL(clicked()), 
	//	this, 
	//	SLOT(widgetBtnClickedEvent()));

	//	//if (m_settings->getCudaDevices() == 0)
	//}
	//else {

	//	cudaBtn->setEnabled(false);
	//}
	
	//if (m_settings->getCudaDevices() == 0) {

	//	cudaGraphBtn->setEnabled(false);

	//	/*CHECKED_CONNECT(
	//			cudaGraphBtn, 
	//			SIGNAL(clicked()), 
	//			this, 
	//			SLOT(widgetBtnClickedEvent()));
	//	CHECKED_CONNECT(cudaTestBtn, 
	//			SIGNAL(clicked()), 
	//			this, 
	//			SLOT(cudaTestBtnClickedEvent()));*/
	//}
	
	displayBtn = new AeroButton("Display", this);
	displayBtn->setRoundness(10);
	displayBtn->setFixedSize(btn_width1, btn_height1);
	mainBtnList.append(displayBtn);

	CHECKED_CONNECT(
		displayBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(widgetBtnClickedEvent()));

	modeBtn = new AeroButton("Mode", this);
	modeBtn->setRoundness(10);
	modeBtn->setFixedSize(btn_width1, btn_height1);

	quitBtn = new AeroButton("Quit", this);
	quitBtn->setRoundness(10);
	quitBtn->setFixedSize(btn_width1, btn_height1);

	CHECKED_CONNECT(
		quitBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(closeMainWindow()));

	nullBtn = new AeroButton(this);
	nullBtn->setRoundness(0);
	nullBtn->setFixedHeight(btn_height1);
	nullBtn->setHighlight(QColor(90, 90, 90));
	nullBtn->setEnabled(false);
		
	mainBtnLayout->addWidget(startBtn);
	mainBtnLayout->addWidget(ctrlDisplayBtn);
	mainBtnLayout->addWidget(serverLogBtn);
	mainBtnLayout->addWidget(rxCtrlBtn);
	mainBtnLayout->addWidget(serverBtn);
	mainBtnLayout->addWidget(hpsdrBtn);
	//mainBtnLayout->addWidget(alexBtn);
	//mainBtnLayout->addWidget(pennyBtn);
	mainBtnLayout->addWidget(chirpBtn);
	mainBtnLayout->addWidget(wideBandBtn);
	//mainBtnLayout->addWidget(openclBtn);
	mainBtnLayout->addWidget(displayBtn);
	mainBtnLayout->addWidget(nullBtn);
	mainBtnLayout->addWidget(modeBtn);
	mainBtnLayout->addWidget(quitBtn);
	
	m_buttonWidget->setLayout(mainBtnLayout);
}

 
/*!
	\brief create a second button group.
*/
void MainWindow::createSecondBtnGroup() {

	m_secondButtonWidget = new QWidget(this);

	QHBoxLayout *secondBtnLayout = new QHBoxLayout(this);
	secondBtnLayout->setSpacing(0);
	secondBtnLayout->setMargin(0);

	QFontMetrics fm(m_smallFont);
	int fontMaxWidth = fm.boundingRect("100 % ").width();
	int vol = (int)(m_settings->getMainVolume(0) * 100);

	m_volumeSlider = new QSlider(Qt::Horizontal, this);
	m_volumeSlider->setTickPosition(QSlider::NoTicks);
	m_volumeSlider->setFixedSize(100, 14);
	m_volumeSlider->setSingleStep(1);
	m_volumeSlider->setRange(0, 100);
	m_volumeSlider->setValue(vol);
	m_volumeSlider->setStyleSheet(m_settings->getVolSliderStyle());

	CHECKED_CONNECT(
		m_volumeSlider, 
		SIGNAL(valueChanged(int)), 
		this, 
		SLOT(setMainVolume(int)));

	m_volumeLabel = new QLabel("Vol:", this);
    m_volumeLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_volumeLabel->setStyleSheet(m_settings->getLabelStyle());

	QString str = "%1 %";
	m_volLevelLabel = new QLabel(str.arg(vol, 2, 10, QLatin1Char(' ')), this);
	m_volLevelLabel->setFont(m_smallFont);
	m_volLevelLabel->setFixedSize(fontMaxWidth, 14);
    m_volLevelLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_volLevelLabel->setStyleSheet(m_settings->getSliderLabelStyle());

	
	int gain = m_settings->getAGCGain(0);
	m_agcGainSlider = new QSlider(Qt::Horizontal, this);
	m_agcGainSlider->setTickPosition(QSlider::NoTicks);
	m_agcGainSlider->setFixedSize(100, 14);
	m_agcGainSlider->setSingleStep(1);
	m_agcGainSlider->setRange(-20, 120);
	m_agcGainSlider->setValue(gain);
	m_agcGainSlider->setStyleSheet(m_settings->getVolSliderStyle());
	
	CHECKED_CONNECT(
		m_agcGainSlider, 
		SIGNAL(valueChanged(int)), 
		this, 
		SLOT(setAGCGain(int)));

	m_agcGainLabel = new QLabel("", this);
    m_agcGainLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_agcGainLabel->setStyleSheet(m_settings->getLabelStyle());
	if (m_serverMode == QSDR::DttSP)
		m_agcGainLabel->setText("AGC-T:");
	else if (m_serverMode == QSDR::QtDSP)
		m_agcGainLabel->setText("AGC Gain:");

	fontMaxWidth = fm.boundingRect("120 dB ").width();
	str = "%1 dB";
	m_agcGainLevelLabel = new QLabel(str.arg(gain, 2, 10, QLatin1Char(' ')), this);
	m_agcGainLevelLabel->setFont(m_smallFont);
	m_agcGainLevelLabel->setFixedSize(fontMaxWidth, 14);
    m_agcGainLevelLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_agcGainLevelLabel->setStyleSheet(m_settings->getSliderLabelStyle());
	
	avgBtn = new AeroButton("Pan Avg", this);
	avgBtn->setRoundness(10);
	avgBtn->setFixedSize(btn_width1, btn_height3);

	if (m_settings->getSpectrumAveraging())
		avgBtn->setBtnState(AeroButton::ON);
	else
		avgBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		avgBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(avgBtnClickedEvent()));

	gridBtn = new AeroButton("Pan Grid", this);
	gridBtn->setRoundness(10);
	gridBtn->setFixedSize(btn_width1, btn_height3);

	if (m_settings->getPanGridStatus())
		gridBtn->setBtnState(AeroButton::ON);
	else
		gridBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		gridBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(gridBtnClickedEvent()));
	
	lastFreqBtn = new AeroButton("last Freq", this);
	lastFreqBtn->setRoundness(10);
	lastFreqBtn->setFixedSize(btn_width1, btn_height3);
	lastFreqBtn->setBtnState(AeroButton::OFF);

	CHECKED_CONNECT(
		lastFreqBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(getLastFrequency()));

	// rx Buttons
	/*rxBtn = new AeroButton("Rx", this);
	rxBtn->setRoundness(0);
	rxBtn->setFixedSize(22, btn_height3);
	rxBtn->setHighlight(QColor(90, 90, 90));
	rxBtn->setEnabled(false);*/

	m_rxLabel = new QLabel("Rx:", this);
    m_rxLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_rxLabel->setStyleSheet(m_settings->getLabelStyle());

	rx1Btn = new AeroButton("1", this);
	rx1Btn->setRoundness(10);
	rx1Btn->setFixedSize(22, btn_height3);
	rx1Btn->setBtnState(AeroButton::ON);
	rxBtnList.append(rx1Btn);
	
	CHECKED_CONNECT(
		rx1Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setReceiver()));

	rx2Btn = new AeroButton("2", this);
	rx2Btn->setRoundness(10);
	rx2Btn->setFixedSize(22, btn_height3);
	rx2Btn->setBtnState(AeroButton::OFF);
	rxBtnList.append(rx2Btn);

	CHECKED_CONNECT(
		rx2Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setReceiver()));

	rx3Btn = new AeroButton("3", this);
	rx3Btn->setRoundness(10);
	rx3Btn->setFixedSize(22, btn_height3);
	rx3Btn->setBtnState(AeroButton::OFF);
	rxBtnList.append(rx3Btn);

	CHECKED_CONNECT(
		rx3Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setReceiver()));

	rx4Btn = new AeroButton("4", this);
	rx4Btn->setRoundness(10);
	rx4Btn->setFixedSize(22, btn_height3);
	rx4Btn->setBtnState(AeroButton::OFF);
	rxBtnList.append(rx4Btn);

	CHECKED_CONNECT(
		rx4Btn, 
		SIGNAL(released()), 
		this, 
		SLOT(setReceiver()));

	/*secondBtnLayout->addWidget(avgBtn);
	secondBtnLayout->addWidget(gridBtn);
	secondBtnLayout->addStretch();
	secondBtnLayout->addWidget(m_agcGainLabel);
	secondBtnLayout->addWidget(m_agcGainSlider);
	secondBtnLayout->addSpacing(-50);
	secondBtnLayout->addWidget(m_agcGainLevelLabel);
	secondBtnLayout->addSpacing(40);
	secondBtnLayout->addWidget(m_volumeLabel);
	secondBtnLayout->addWidget(m_volumeSlider);
	secondBtnLayout->addSpacing(-50);
	secondBtnLayout->addWidget(m_volLevelLabel);
	secondBtnLayout->addSpacing(40);
	secondBtnLayout->addSpacing(10);
	secondBtnLayout->addWidget(lastFreqBtn);
	secondBtnLayout->addWidget(m_rxLabel);
	secondBtnLayout->addWidget(rx1Btn);
	secondBtnLayout->addWidget(rx2Btn);
	secondBtnLayout->addWidget(rx3Btn);
	secondBtnLayout->addWidget(rx4Btn);*/

	secondBtnLayout->addWidget(avgBtn);
	secondBtnLayout->addWidget(gridBtn);
	secondBtnLayout->addStretch();
	secondBtnLayout->addWidget(m_agcGainLabel);
	secondBtnLayout->addWidget(m_agcGainSlider);
	secondBtnLayout->addWidget(m_agcGainLevelLabel);
	secondBtnLayout->addSpacing(10);
	secondBtnLayout->addWidget(m_volumeLabel);
	secondBtnLayout->addWidget(m_volumeSlider);
	secondBtnLayout->addWidget(m_volLevelLabel);
	secondBtnLayout->addSpacing(10);
	secondBtnLayout->addWidget(lastFreqBtn);
	secondBtnLayout->addWidget(m_rxLabel);
	secondBtnLayout->addWidget(rx1Btn);
	secondBtnLayout->addWidget(rx2Btn);
	secondBtnLayout->addWidget(rx3Btn);
	secondBtnLayout->addWidget(rx4Btn);
	
	m_secondButtonWidget->setLayout(secondBtnLayout);
}

/*!
	\brief create the Mode menu.
*/
void MainWindow::createModeMenu() {

	m_modeMenu->setStyleSheet(m_settings->getMenuStyle());
    modeBtn->setMenu(m_modeMenu);

    //m_internalDSPModeAction = m_modeMenu->addAction(tr("internal DSP"));
	m_qtdspModeAction = m_modeMenu->addAction(tr("QtDSP"));
	m_dttspModeAction = m_modeMenu->addAction(tr("DttSP"));
    m_externalDSPModeAction = m_modeMenu->addAction(tr("extDSP"));
    m_chirpWSPRAction = m_modeMenu->addAction(tr("ChirpWSPR"));

    //m_internalDSPModeAction->setCheckable(true);
	m_qtdspModeAction->setCheckable(false);
	m_dttspModeAction->setCheckable(true);
    m_externalDSPModeAction->setCheckable(false);
    m_chirpWSPRAction->setCheckable(false);
	
	setServerMode(m_serverMode);

    /*CHECKED_CONNECT(
		m_internalDSPModeAction, 
		SIGNAL(triggered(bool)), 
		this, 
		SLOT(setInternalDSPMode()));*/

	if (m_qtdspModeAction->isCheckable()) {
		
		CHECKED_CONNECT(
			m_qtdspModeAction, 
			SIGNAL(triggered(bool)), 
			this, 
			SLOT(setQtDSPMode()));
	}

	if (m_dttspModeAction->isCheckable()) {
		
		CHECKED_CONNECT(
			m_dttspModeAction, 
			SIGNAL(triggered(bool)), 
			this, 
			SLOT(setDttSPMode()));
	}

	if (m_externalDSPModeAction->isCheckable()) {

	    CHECKED_CONNECT(
			m_externalDSPModeAction, 
			SIGNAL(triggered(bool)), 
			this, 
			SLOT(setExternalDSPMode()));
	}

	if (m_chirpWSPRAction->isCheckable()) {
		
		CHECKED_CONNECT(
			m_chirpWSPRAction, 
			SIGNAL(triggered(bool)), 
			this, 
			SLOT(setChirpWSPRMode()));
	}
}
 
//*******************************************************************************
 
/*!
	\brief the master switch functionality.
	- starts/stops the data engine.
	- starts/stops the server if in \a QSDR::ExternalDSP mode.
*/
void MainWindow::masterSwitchChanged(
		QObject *sender,				/*!<[in] the sender of the signal. */
		bool power						/*!<[in] power on or off*/
) {
	Q_UNUSED(sender)
	
	if (power) {

		if (m_dataEngine->startDataEngine()) {	// start hpsdr hardware interface
		
			if (m_serverMode == QSDR::ExternalDSP && !m_hpsdrServer->startServer())
				m_hpsdrServer->stopServer();
			
			return;
		}
		else {

			m_settings->setMainPower(this, false);
			startButtonClickedEvent();
			return;
		}
	}
	else {

		m_dataEngine->stopDataEngine();

		if (m_serverMode == QSDR::ExternalDSP)
			m_hpsdrServer->stopServer();

		m_settings->setMainPower(this, false);
	}
}

 
/*!
	\brief set the system state according to
	- \a QSDR::_ServerMode,
	- \a QSDR::_HWInterfaceMode,
	- \a QSDR::_DataEngineState.
*/
void MainWindow::systemStateChanged(
	QObject *sender,					/*!<[in] the sender of the signal. */
	QSDR::_Error err,					/*!<[in] error state. */
	QSDR::_HWInterfaceMode hwmode,		/*!<[in] HPSDR interface (Metis, Hermes, none). */
	QSDR::_ServerMode mode,				/*!<[in] server mode. */
	QSDR::_DataEngineState state		/*!<[in] data engine state. */
) {
	Q_UNUSED (sender)
	Q_UNUSED (err)
	Q_UNUSED (hwmode)
	//Q_UNUSED (state)

	bool change = false;

	if (m_serverMode != mode) {
		
		m_serverMode = mode;
		change = true;
	}

	if (state == QSDR::DataEngineUp) {

		m_dataEngineState = QSDR::DataEngineUp;
		modeBtn->setEnabled(false);
	}
	else
	if (state == QSDR::DataEngineDown) {

		m_dataEngineState = QSDR::DataEngineDown;
		modeBtn->setEnabled(true);
		setReceiver(0);
	}

	if (!change) return;
}
 
/*!
	\brief closes the application and shuts down all engines.
*/
void MainWindow::closeMainWindow() {

	if (m_settings->mainPower())
		startButtonClickedEvent();
	close();
}
 
/*!
	\brief maximizes/minimizes the main window.
*/
void MainWindow::maximizeMainWindow() {

	if (isMaximized()) {

		//m_titlebar->maxBtn->setIcon(QIcon(QString::fromUtf8(":/img/maximize_high.png")));//, QSize(), QIcon::Normal, QIcon::Off);
		//m_titlebar->maxBtn->update();
		showNormal();
	}
	else {

		//m_titlebar->maxBtn->setIcon(QIcon(QString::fromUtf8(":/img/minimize_high.png")));//, QSize(), QIcon::Normal, QIcon::Off);
		//m_titlebar->maxBtn->update();
		showMaximized();
	}
}
 
/*!
	\brief maximizes/minimizes the main window and hide/show titlebar.
*/
void MainWindow::setMainWindowGeometry() {

	if (isMaximized()) {

		setWindowFlags(Qt::Widget);
		//this->move(m_oldPosition);
		this->setGeometry(m_oldPosition.x(), m_oldPosition.y(), m_oldSize.width(), m_oldSize.height());
		showNormal();
	}
	else {

		setWindowFlags(Qt::FramelessWindowHint);

		m_oldSize = this->size();
		m_oldPosition = this->pos();
		QPoint pos = m_oldPosition;

		if (pos.x() < 0) 
			pos.setX(0);
		if (pos.y() < 0) 
			pos.setY(0);

		this->move(pos);
		this->show();
		showMaximized();
	}
}
 
/*!
	\brief sets full screen operation.
*/
void MainWindow::setFullScreen() {

	if (m_fullScreen)
		this->showNormal();
	else
		this->showFullScreen();
}
 
/*!
	\brief updates entries from the ini-file.
*/
void MainWindow::updateFromSettings() {

}
 
/*!
	\brief implements the Log button functionality which opens/closes the logging widget.
*/
void MainWindow::messagesBtnClickedEvent() {

	if (m_msgBrowser->isVisible()) {
	
		m_msgBrowser->setVisible(false);
		m_contentLayout->setRowMinimumHeight(3, 0);
		serverLogBtn->setBtnState(AeroButton::OFF);
	} 
	else {

		m_msgBrowser->setVisible(true);
		m_contentLayout->setRowMinimumHeight(3, 150);
		serverLogBtn->setBtnState(AeroButton::ON);
	}
}
 
/*!
	\brief implements the Radio-display button functionality which opens/closes the radio control widget.
*/
void MainWindow::ctrlDisplayBtnClickedEvent() {

	if (m_oglDisplayPanel->isVisible()) {
	
		m_oglDisplayPanel->setVisible(false);
		ctrlDisplayBtn->setBtnState(AeroButton::OFF);
	} 
	else {

		m_oglDisplayPanel->setVisible(true);
		ctrlDisplayBtn->setBtnState(AeroButton::ON);
	}
}
 
/*!
	\brief implements various Button functionalities.
*/
void MainWindow::widgetBtnClickedEvent() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int on = mainBtnList.indexOf(button);

	foreach(QWidget *widget, widgetList) {

		int off = widgetList.indexOf(widget);

		if (widget->isVisible()) {
				
			widget->hide();
			mainBtnList.at(off)->setBtnState(AeroButton::OFF);
			mainBtnList.at(off)->update();
		}
		else if (on == off) {

			button->setBtnState(AeroButton::ON);
			button->update();
			widgetList.at(on)->show();
		}
	}
}

 
/*!
	\brief implements the Start/Stop functionality of the Start/Stop button.
*/
void MainWindow::startButtonClickedEvent() {

	if (startBtn->btnState() == AeroButton::OFF) {

		startBtn->setBtnState(AeroButton::ON);
		startBtn->setColorOn(QColor(180, 0, 0));
		startBtn->setHighlight(QColor(250, 0, 0));
		startBtn->setText("Stop");
		m_settings->setMainPower(this, true);
	}
	else if (startBtn->btnState() == AeroButton::ON) {

		startBtn->setBtnState(AeroButton::OFF);
		startBtn->setHighlight(QColor(0x91, 0xeb, 0xff));
		startBtn->setText("Start");
		m_settings->saveSettings();
		m_settings->setMainPower(this, false);
	}
}

 
/*!
	\brief implements the Show/Hide functionality of the wide band data widget.
*/
void MainWindow::wideBandBtnClickedEvent() {

	if (wideBandBtn->btnState() == AeroButton::OFF) {

		wideBandBtn->setBtnState(AeroButton::ON);
		m_settings->setWideBandStatus(true);
		showMessage("[server]: wide band data on.");
	}
	else if (wideBandBtn->btnState() == AeroButton::ON) {

		wideBandBtn->setBtnState(AeroButton::OFF);
		m_settings->setWideBandStatus(false);
		showMessage("[server]: wide band data off.");
	}
}

 
/*!
	\brief switch Panadapter Spectrum Averaging Filter on/off.
*/
void MainWindow::avgBtnClickedEvent() {

	if (avgBtn->btnState() == AeroButton::OFF) {

		avgBtn->setBtnState(AeroButton::ON);
		m_settings->setSpectrumAveraging(true);
		//showMessage("[server]: wide band data on.");
	}
	else if (avgBtn->btnState() == AeroButton::ON) {

		avgBtn->setBtnState(AeroButton::OFF);
		m_settings->setSpectrumAveraging(false);
		//showMessage("[server]: wide band data off.");
	}
}

/*!
	\brief switch Panadapter Grid on/off.
*/
void MainWindow::gridBtnClickedEvent() {

	if (gridBtn->btnState() == AeroButton::OFF) {

		gridBtn->setBtnState(AeroButton::ON);
		m_settings->setPanGrid(true);
	}
	else if (gridBtn->btnState() == AeroButton::ON) {

		gridBtn->setBtnState(AeroButton::OFF);
		m_settings->setPanGrid(false);
	}
}

/*!
	\brief tune to last frequency.
*/
void MainWindow::getLastFrequency() {
}

/*!
	\brief shows a system message in the Log widget.
*/
void MainWindow::showMessage(QString message) {

	m_msgBrowser->appendPlainText(message);
}

 
/*!
	\brief updates the main window title.
*/
void MainWindow::updateTitle() {

	setWindowTitle(
		QApplication::applicationName() + "  " + \
		QApplication::applicationVersion() + "         " + \
		m_settings->callsign());
}
 
/*!
	\brief show widget event.
*/
void MainWindow::showWidgetEvent(
		QObject *sender				/*!<[in] the sender of the event. */
) {
	if (!sender) return;
}

 
/*!
	\brief hide widget event.
*/
void MainWindow::closeWidgetEvent(
		QObject *sender				/*!<[in] the sender of the event. */
)
{
	if (!sender) return;
}

 
void MainWindow::setReceiver() {

	AeroButton *button = qobject_cast<AeroButton *>(sender());
	int rx = rxBtnList.indexOf(button);

	foreach(AeroButton *btn, rxBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	rxBtnList[rx]->setBtnState(AeroButton::ON);
	rxBtnList[rx]->update();
	m_settings->setCurrentReceiver(rx);
	m_dataEngine->io.currentReceiver = rx;
	m_volumeSlider->setValue((int)(m_settings->getMainVolume(rx) * 100));
	m_agcGainSlider->setValue((int)(m_settings->getAGCGain(rx) * 100));
}

 
void MainWindow::setReceiver(int rx) {

	foreach(AeroButton *btn, rxBtnList) {

		btn->setBtnState(AeroButton::OFF);
		btn->update();
	}

	rxBtnList[rx]->setBtnState(AeroButton::ON);
	rxBtnList[rx]->update();
	m_settings->setCurrentReceiver(rx);
	m_dataEngine->io.currentReceiver = rx;
	m_volumeSlider->setValue((int)(m_settings->getMainVolume(rx) * 100));
	m_agcGainSlider->setValue(m_settings->getAGCGain(rx));
}

 
/*!
	\brief enable the receiver buttons according to the actual number of available receivers.
*/
void MainWindow::setNumberOfReceivers(
		QObject *sender,			/*!<[in] the sender of the event. */
		int value					/*!<[in] the number of receivers. */
) {
	Q_UNUSED(sender)

	switch(value) {
	
		case 1:
			rx1Btn->setEnabled(true);
			rx2Btn->setEnabled(false);
			rx3Btn->setEnabled(false);
			rx4Btn->setEnabled(false);
			break;
			
		case 2:
			rx1Btn->setEnabled(true);
			rx2Btn->setEnabled(true);
			rx3Btn->setEnabled(false);
			rx4Btn->setEnabled(false);
			break;

		case 3:
			rx1Btn->setEnabled(true);
			rx2Btn->setEnabled(true);
			rx3Btn->setEnabled(true);
			rx4Btn->setEnabled(false);
			break;

		case 4:
			rx1Btn->setEnabled(true);
			rx2Btn->setEnabled(true);
			rx3Btn->setEnabled(true);
			rx4Btn->setEnabled(true);
			break;
	}
}

 

//void MainWindow::noOfReceiversChanged() {
//

//
//	if (rx1Btn->btnState() == AeroButton::ON) {
//
//		m_settings->setReceivers(this, 1);
//		emit changeNoOfReceivers(this, 1);
//	}
//
//	if (rx2Btn->btnState() == AeroButton::ON) {
//
//		m_settings->setReceivers(this, 2);
//		emit changeNoOfReceivers(this, 2);
//	}
//
//	if (rx3Btn->btnState() == AeroButton::ON) {
//
//		m_settings->setReceivers(this, 3);
//		emit changeNoOfReceivers(this, 3);
//	}
//
//	if (rx4Btn->btnState() == AeroButton::ON) {
//
//		m_settings->setReceivers(this, 4);
//		emit changeNoOfReceivers(this, 4);
//	}
//}
 
/*!
	\brief set the main volume.
*/
void MainWindow::setMainVolume(int value) {

	if (value < 0 ) value = 0;
	if (value > 100 ) value = 100;

	QString str = "%1 %";
	m_volLevelLabel->setText(str.arg(value, 2, 10, QLatin1Char(' ')));
	m_settings->setMainVolume(this, m_settings->getCurrentReceiver(), value / 100.0f);
}

void MainWindow::setAGCGain(int value) {

	if (value < -20 ) value = -20;
	if (value > 120 ) value = 120;

	QString str = "%1 dB";
	m_agcGainLevelLabel->setText(str.arg(value, 2, 10, QLatin1Char(' ')));
	m_settings->setAGCGain(this, m_settings->getCurrentReceiver(), value);
}
 
/*!
	\brief set \a QSDR::_ServerMode to \a QSDR::InternalDSP.
*/
void MainWindow::setInternalDSPMode() {

	/*setServerMode(QSDR::InternalDSP);

	if (m_oldSampleRate == 48000 || m_oldSampleRate == 96000 || m_oldSampleRate == 192000)
		m_settings->setSampleRate(this, m_oldSampleRate);
	
	showMessage("[server]: switched to internal DSP mode.");*/
}

void MainWindow::setDttSPMode() {

	m_settings->setSpectrumSize(4096);
	m_agcGainLabel->setText("AGC-T:");
	setServerMode(QSDR::DttSP);

	if (m_oldSampleRate == 48000 || m_oldSampleRate == 96000 || m_oldSampleRate == 192000)
		m_settings->setSampleRate(this, m_oldSampleRate);
	
	showMessage("[server]: switched to DttSP mode.");
}

void MainWindow::setQtDSPMode() {

	m_settings->setSpectrumSize(2048);
	m_agcGainLabel->setText("AGC Gain:");
	setServerMode(QSDR::QtDSP);

	if (m_oldSampleRate == 48000 || m_oldSampleRate == 96000 || m_oldSampleRate == 192000)
		m_settings->setSampleRate(this, m_oldSampleRate);
	
	showMessage("[server]: switched to internal QtDSP mode.");
}

/*!
	\brief set \a QSDR::_ServerMode to \a QSDR::ExternalDSP.
*/
void MainWindow::setExternalDSPMode() {

	//setServerMode(QSDR::ExternalDSP);
	
	//showMessage("[server]: switched to external DSP mode.");
}
 
/*!
	\brief set \a QSDR::_ServerMode to \a QSDR::ChirpWSPR.
*/
void MainWindow::setChirpWSPRMode() {

	setServerMode(QSDR::ChirpWSPR);

	m_oldSampleRate = m_settings->getSampleRate();
	setNumberOfReceivers(this, 1);
	m_settings->setReceivers(this, 1);
	m_settings->setSampleRate(this, 48000);
	//m_dataEngine->createChirpDataProcessor();

	showMessage("[server]: switched to chirp decode mode.");
}
 
/*!
	\brief set \a QSDR::_ServerMode to \a QSDR::CudaDSP.
*/
void MainWindow::setCudaDSPMode() {

	//setServerMode(QSDR::CudaDSP);
	
	//showMessage("[server]: switched to Cuda DSP mode.");
}
 
/*!
	\brief set \a QSDR::_ServerMode to \a mode.
*/
void MainWindow::setServerMode(
		QSDR::_ServerMode mode			/*!<[in] server mode. */
) {
	//rxCtrlBtn->setEnabled(QSDR::InternalDSP == mode);
	//wideBandBtn->setEnabled(QSDR::InternalDSP == mode);
	wideBandBtn->setEnabled(QSDR::DttSP == mode || QSDR::QtDSP == mode);

	if (mode != QSDR::ChirpWSPR && chirpBtn->btnState() == AeroButton::ON)
		m_chirpWidget->hide();

	if (mode == QSDR::ChirpWSPR && wideBandBtn->btnState() == AeroButton::ON) {

		//m_settings->setWideBandStatus(false);
		wideBandBtn->setBtnState(AeroButton::OFF);
	}

	chirpBtn->setEnabled(QSDR::ChirpWSPR == mode);

	m_qtdspModeAction->setChecked(QSDR::QtDSP == mode);
	m_dttspModeAction->setChecked(QSDR::DttSP == mode);
    //m_internalDSPModeAction->setChecked(QSDR::InternalDSP == mode);
    m_externalDSPModeAction->setChecked(QSDR::ExternalDSP == mode);
    //m_chirpWSPRAction->setChecked(QSDR::ChirpWSPR == mode);
	
	m_settings->setSystemState(
		this,
		QSDR::NoError,
		m_settings->getHWInterface(),
		mode,
		m_settings->getDataEngineState());
}

//void MainWindow::setPennyPresence(bool value) {
//
//	pennyBtn->setEnabled(value);
//	if (value)
//		pennyBtn->show();
//	else
//		pennyBtn->hide();
//}
//
//void MainWindow::setAlexPresence(bool value) {
//
//	alexBtn->setEnabled(value);
//	if (value)
//		alexBtn->show();
//	else
//		alexBtn->hide();
//}
 
/*!
	\brief get all network interfaces and IP addresses.
*/
void MainWindow::getNetworkInterfaces() {

	// find out which IP to connect to
	QString localIP = QHostAddress(QHostAddress::LocalHost).toString();

	// ip addresses from ini file
	QString serverIpAddress = m_settings->getServerAddr();
	QString hpsdrDeviceLocalIpAddress = m_settings->getHPSDRDeviceLocalAddr();

	MAIN_DEBUG << "server ip from ini-file: " << serverIpAddress;
	MAIN_DEBUG << "HPSDR device local ip from ini-file: " << hpsdrDeviceLocalIpAddress;
	
	QList<QHostAddress> broadcastAddresses;
	QList<QHostAddress> ipAddresses;
	QList<QNetworkInterface> nics;

	m_ipList = QNetworkInterface::allAddresses();
	m_niList = QNetworkInterface::allInterfaces();
	
	foreach (QNetworkInterface ni, m_niList) {

		//MAIN_DEBUG << "network interface " << ni.humanReadableName() << " :";
		//foreach (QNetworkAddressEntry entry, ni.addressEntries()) {

		//	QHostAddress broadcastAddress = entry.broadcast();
		//	MAIN_DEBUG << "  -> broadcast address: " << qPrintable(broadcastAddress.toString());
		//	//if (broadcastAddress != QHostAddress::Null && entry.ip() != QHostAddress::LocalHost) {
		//	if (entry.ip().toIPv4Address() && entry.ip() != QHostAddress::LocalHost) {
		//	//if (entry.ip() != QHostAddress::LocalHost) {
		//		broadcastAddresses << broadcastAddress;
		//		ipAddresses << entry.ip();
		//		nics << ni;
		//		MAIN_DEBUG << "  -> ip address: " << qPrintable(entry.ip().toString());
		//		m_settings->m_ipAddressesList.append(entry.ip());
		//		m_settings->m_networkInterfaces.append(ni);
		//
		//		m_settings->addServerNetworkInterface(ni.humanReadableName(), entry.ip().toString());
		//		m_settings->addHPSDRDeviceNIC(ni.humanReadableName(), entry.ip().toString());
		//	}
		//}

		foreach (QNetworkAddressEntry entry, ni.addressEntries()) {

			QHostAddress broadcastAddress = entry.broadcast();
			
			//if (broadcastAddress != QHostAddress::Null && entry.ip() != QHostAddress::LocalHost) {
			if (entry.ip().toIPv4Address() && entry.ip() != QHostAddress::LocalHost) {
			//if (entry.ip() != QHostAddress::LocalHost) {

				MAIN_DEBUG << "network interface " << ni.humanReadableName() << " :";
				MAIN_DEBUG << "  -> broadcast address: " << qPrintable(broadcastAddress.toString());
				broadcastAddresses << broadcastAddress;
				ipAddresses << entry.ip();
				nics << ni;
				MAIN_DEBUG << "  -> ip address: " << qPrintable(entry.ip().toString());
				m_settings->m_ipAddressesList.append(entry.ip());
				m_settings->m_networkInterfaces.append(ni);
				
				m_settings->addServerNetworkInterface(ni.humanReadableName(), entry.ip().toString());
				m_settings->addHPSDRDeviceNIC(ni.humanReadableName(), entry.ip().toString());
			}
		}
	}

	m_settings->setNumberOfNetworkInterfaces(nics.size());
	MAIN_DEBUG << "found " << nics.size() << " valid ip addresses.";

	//emit serverWidgetEvent();
	//emit metisWidgetEvent();

	int serverIdx = -1;
	int metisIdx = -1;

	if (ipAddresses.isEmpty()) {

		// if we did not find one, use IPv4 localhost
		m_settings->setServerAddr(this, localIP);
		m_settings->setHPSDRDeviceLocalAddr(this, localIP);
	}
	else {

		//int serverIdx = -1;
		//int metisIdx = -1;
		
		for (int i = 0; i < nics.size(); ++i) {
			if (broadcastAddresses.at(i) != QHostAddress::Null) {

				if (ipAddresses.at(i).toString() == serverIpAddress) {

					m_settings->setServerWidgetNIC(i);
					serverIdx = i;
				}
				if (ipAddresses.at(i).toString() == hpsdrDeviceLocalIpAddress) {

					m_settings->setHPSDRWidgetNIC(i);
					metisIdx = i;
				}
			}
		}
	}

	QString message;
	for (int i = 0; i < nics.size(); ++i) {
		if (broadcastAddresses.at(i) != QHostAddress::Null) {
				
			if (serverIdx < 0) {
				m_settings->setServerWidgetNIC(i);
				m_settings->setServerAddr(this, ipAddresses.at(i).toString());

				/*message = "[server]: network interface set to: %1 (%2).";
				showMessage(
					message.arg(
							nics.at(i).humanReadableName(),
							ipAddresses.at(i).toString()));*/
			}
			else {

				/*message = "[server]: network interface set to: %1 (%2).";
				showMessage(
					message.arg(
						nics.at(serverIdx).humanReadableName(),
						ipAddresses.at(serverIdx).toString() ));*/
			}

			if (metisIdx < 0) {

				m_settings->setHPSDRWidgetNIC(i);
				m_settings->setHPSDRDeviceLocalAddr(this, ipAddresses.at(i).toString());

				message = "[server]: HPSDR network device interface set to: %1 (%2).";
				showMessage(
					message.arg(
							nics.at(i).humanReadableName(),
							ipAddresses.at(i).toString()));
			}
			else {
			
				message = "[server]: HPSDR network device interface set to: %1 (%2).";
				showMessage(
					message.arg(
							nics.at(metisIdx).humanReadableName(),
							ipAddresses.at(metisIdx).toString()));
			}
		}

		/*for (int i = 0; i < nics.size(); ++i) {
			if (serverIdx != i) {
				m_settings->setServerWidgetNIC(i);
				break;
			}
		}

		for (int i = 0; i < nics.size(); ++i) {
			if (metisIdx != i) {
				m_settings->setHPSDRWidgetNIC(i);
				break;
			}
		
		}*/
	}

	//emit serverWidgetEvent();
	//emit metisWidgetEvent();

	//m_settings->setServerWidgetNIC(1);
	MAIN_DEBUG << "using ip address " << qPrintable(m_settings->getServerAddr()) << " for the server.";
	MAIN_DEBUG << "using ip address " << qPrintable(m_settings->getHPSDRDeviceLocalAddr()) << " for connecting to a HPSDR device.";

	
}

 
/*!
	\brief load a wav-file.
*/
void MainWindow::loadWavFile(
		QObject *sender,				/*!<[in] the sender of the event. */
		const QString &fileName			/*!<[in] file name of the *.wav file. */
) {
	Q_UNUSED(sender)

	m_dataEngine->loadWavFile(fileName);
}

 
/*!
	\brief suspend playing wav-file.
*/
void MainWindow::suspendSignal(
		QObject *sender					/*!<[in] the sender of the event. */
) {
	Q_UNUSED(sender)

	m_dataEngine->suspend();
}

 
/*!
	\brief start playback.
*/
void MainWindow::startPlaybackSignal(
		QObject *sender					/*!<[in] the sender of the event. */
) {
	Q_UNUSED(sender)

	m_dataEngine->startPlayback();
}

 
/*!
	\brief show the audio settings dialog.
*/
void MainWindow::showSettingsDialogSignal(
		QObject *sender					/*!<[in] the sender of the event. */
) {
	Q_UNUSED(sender)

	m_dataEngine->showSettingsDialog();
}

void MainWindow::showNetworkIODialog() {

	m_netIODialog->exec();
}

void MainWindow::addNetworkIOComboBoxEntry(QString str) {

	m_netIODialog->addDeviceComboBoxItem(str);
}

void MainWindow::clearNetworkIOComboBoxEntry() {

	m_netIODialog->clearDeviceComboBoxItem();
}
 
/*!
	\brief generates an initial message for the logging widget-
*/
void MainWindow::initialMessage() {

	QString str = m_settings->versionStr();
	str.prepend("cuSDR ");
	//str.append(". OpenCL enabled HPSDR Front end (C) 2010-2012 Hermann von Hasseln, DL3HVH.\n");
	str.append(". OpenGL enabled HPSDR Front end (C) 2010-2012 Hermann von Hasseln, DL3HVH.\n");
	m_msgBrowser->appendPlainText(str);
	m_msgBrowser->appendPlainText("");
}
 
//*******************************************************************************
// Application's window stuff

/*!
	\brief get the QRegion of the window borders.
*/
void MainWindow::getRegion() {

	/*m_topBorderFrame    = QRegion(QRect(8, 0, width() - 16, 2));
	m_topFrame 			= QRegion(QRect(8, 2, width() - 16, 3));
	m_leftFrame 		= QRegion(QRect(0, 8, 8, height() - 16));
	m_rightFrame 		= QRegion(QRect(width() - 8, 8, 8, height() - 16));
	m_bottomFrame 		= QRegion(QRect(8, height() - 8, width() - 16, 8));
	m_topLeftFrame 		= QRegion(QRect(0, 0, 8, 8));
	m_bottomLeftFrame	= QRegion(QRect(0, height() - 8, 8, 8));
	m_topRightFrame 	= QRegion(QRect(width() - 8, 0, 8, 8));
	m_bottomRightFrame	= QRegion(QRect(width() - 8, height() - 8, 8, 8));*/
}

 
/*!
	\brief selects part of the frame for resizing the main window.
*/
void MainWindow::getSelectedFrame(
		QPoint p					/*!<[in] QPoint p selected */
) {
	/*if (m_leftFrame.contains(p))		m_resizePosition = Left;
	else 
	if (m_topFrame.contains(p))			m_resizePosition = Top;
	else 
	if (m_rightFrame.contains(p))		m_resizePosition = Right;
	else 
	if (m_bottomFrame.contains(p))		m_resizePosition = Bottom;
	else 
	if (m_topLeftFrame.contains(p))		m_resizePosition = TopLeft;
	else 
	if (m_bottomLeftFrame.contains(p))	m_resizePosition = BottomLeft;
	else
	if (m_topRightFrame.contains(p))	m_resizePosition = TopRight;
	else 
	if (m_bottomRightFrame.contains(p))	m_resizePosition = BottomRight;
	else
										m_resizePosition = None;*/
		
}

/*!
	\brief resize the main window (not used currently).
*/
void MainWindow::resizeWidget() {

	//if(m_resizePosition != 0) m_tlw->setGeometry(m_newRect);
}
 
/*!
	\brief mouse release event implementation.
*/
//void MainWindow::mouseReleaseEvent(
//		QMouseEvent *event			/*!<[in] event */
//) {
//
//	if (event->button() == Qt::LeftButton) {
//        
//        m_mousePressed = false;
//		m_pos = QPoint();
//		m_resizePosition = 0;
//		
//		m_dummyLabel->hide();
//		m_innerWidget->show();
//    } 
//	else {
//
//        QWidget::mouseReleaseEvent(event);
//    }
//}

 
/*!
	\brief mouse wheel event implementation.
*/
void MainWindow::wheelEvent(
		QWheelEvent *event			/*!<[in] event */
) {
	event->accept();
	QWidget::wheelEvent(event);
}

 
/*!
	\brief mouse press event implementation.
*/
//void MainWindow::mousePressEvent(
//		QMouseEvent *event			/*!<[in] event */
//) {
//	QPoint pos = event->pos();
//	QRect rect = this->rect();
//
//    if (event->button() == Qt::LeftButton) {
//		
//        m_dragPosition = event->globalPos() - frameGeometry().topLeft();
//		
//        event->accept();
//    }
//
//	//if (pos.x() < width() - 5)
//	if (pos.x() < width())
//
//		// Resizing
//		getSelectedFrame(pos);
//		
//		if (event->button() != Qt::LeftButton) {
//			
//			QWidget::mousePressEvent(event);
//			return;
//		}
//
//		QWidget *tlw = this;
//		m_pos = event->globalPos();
//		m_mousePressed = true;
//		m_rect = tlw->geometry();
//
//	    // Find available desktop/workspace geometry.
//		bool hasVerticalSizeConstraint = true;
//		bool hasHorizontalSizeConstraint = true;
//		
//		if (tlw->isWindow())
//			m_availableGeometry = QApplication::desktop()->availableGeometry(tlw);
//		else {
//
//			const QWidget *tlwParent = tlw->parentWidget();
//			// Check if tlw is inside QAbstractScrollArea/QScrollArea.
//			// If that's the case tlw->parentWidget() will return the viewport
//			// and tlw->parentWidget()->parentWidget() will return the scroll area.
//			m_availableGeometry = tlwParent->contentsRect();
//		}
//		
//		// Find frame geometries, title bar height, and decoration sizes.
//		const QRect frameGeometry = tlw->frameGeometry();
//		const int titleBarHeight = qMax(tlw->geometry().y() - frameGeometry.y(), 0);
//		const int bottomDecoration = qMax(frameGeometry.height() - tlw->height() - titleBarHeight, 0);
//		const int leftRightDecoration = qMax((frameGeometry.width() - tlw->width()) / 2, 0);
//
//		// Create a pixmap from a desktop screenshot. This screenshot
//		// is shown during resizing. We need to take a desktop screenshot
//		// and compute our application's position from this, because we try
//		// to grab a layered window, which doesn't work on Win7, see here:
//		// http://doc.qt.nokia.com/latest/qpixmap.html#grabWidget
//		if (m_resizePosition != None) {
//
//			/*m_originalPixmap = QPixmap::grabWindow(
//										QApplication::desktop()->winId(),
//										QApplication::activeWindow()->pos().x() + 8,
//										QApplication::activeWindow()->pos().y() + m_titlebar->height() + 8, 
//										m_innerWidget->width(), m_innerWidget->height());*/
//
//			m_innerWidget->hide();
//			m_dummyLabel->show();
//		}
//		// Determine m_deltaY_max depending on whether the sizegrip is at the bottom
//		// of the widget or not.
//		switch (m_resizePosition) {
//
//			case Top:
//				if (hasVerticalSizeConstraint)
//					m_deltaY_max = m_availableGeometry.y() - m_rect.y() - bottomDecoration;
//				else
//					m_deltaY_max = -INT_MAX;
//			
//			case Bottom:
//				if (hasVerticalSizeConstraint)
//					m_deltaY_max = m_availableGeometry.bottom() - m_rect.bottom() - bottomDecoration;
//				else
//					m_deltaY_max = INT_MAX;
//				break;
//			
//			case Left:
//				if (hasHorizontalSizeConstraint)
//					m_deltaX_max = m_availableGeometry.x() - m_rect.x() + leftRightDecoration;
//				else
//					m_deltaX_max = -INT_MAX;
//				break;
//			
//			case Right:
//				if (hasHorizontalSizeConstraint)
//					m_deltaX_max = m_availableGeometry.right() - m_rect.right() - leftRightDecoration;
//				else
//					m_deltaX_max = INT_MAX;
//				break;
//			
//			case TopLeft:
//				if (hasVerticalSizeConstraint)
//					m_deltaY_max = m_availableGeometry.y() - m_rect.y() - bottomDecoration;
//				else
//					m_deltaY_max = -INT_MAX;
//				
//				if (hasHorizontalSizeConstraint)
//					m_deltaX_max = m_availableGeometry.x() - m_rect.x() + leftRightDecoration;
//				else
//					m_deltaX_max = -INT_MAX;
//				break;
//			
//			case BottomLeft:
//				if (hasVerticalSizeConstraint)
//					m_deltaY_max = m_availableGeometry.bottom() - m_rect.bottom() - bottomDecoration;
//				else
//					m_deltaY_max = INT_MAX;
//				
//				if (hasHorizontalSizeConstraint)
//					m_deltaX_max = m_availableGeometry.x() - m_rect.x() + leftRightDecoration;
//				else
//					m_deltaX_max = -INT_MAX;
//				break;
//			
//			case TopRight:
//				if (hasVerticalSizeConstraint)
//					m_deltaY_max = m_availableGeometry.y() - m_rect.y() - bottomDecoration;
//				else
//					m_deltaY_max = -INT_MAX;
//				
//				if (hasHorizontalSizeConstraint)
//					m_deltaX_max = m_availableGeometry.right() - m_rect.right() - leftRightDecoration;
//				else
//					m_deltaX_max = INT_MAX;
//				break;
//			
//			case BottomRight:
//				if (hasVerticalSizeConstraint)
//					m_deltaY_max = m_availableGeometry.bottom() - m_rect.bottom() - bottomDecoration;
//				else
//					m_deltaY_max = INT_MAX;
//				
//				if (hasHorizontalSizeConstraint)
//					m_deltaX_max = m_availableGeometry.right() - m_rect.right() - leftRightDecoration;
//				else
//					m_deltaX_max = INT_MAX;
//				break;
//		}
//}

 
/*!
	\brief mouse move event implementation.
*/
//void MainWindow::mouseMoveEvent(
//		QMouseEvent *event			/*!<[in] event */
//) {
//	m_mover = false;
//
//    if (event->buttons() & Qt::LeftButton) {
//
//		// find new position on screen when moving
//		if (//m_dragPosition.y() < m_titlebar->height() &&
//			m_dragPosition.y() > 5 && m_dragPosition.x() > 5 &&
//			m_dragPosition.x() < width() - 5)
//		{
//			QPoint newPos = event->globalPos() - m_dragPosition;
//
//			// snap application's window to screen borders
//			if (qAbs(newPos.x()) < 15)
//				newPos = QPoint(0, newPos.y());
//			if (qAbs(newPos.y()) < 15)
//				newPos = QPoint(newPos.x(), 0);
//
//			/*if (qAbs(m_availableGeometry.width() - newPos.x() - m_tlw->frameGeometry().width()) < 15)
//				newPos = QPoint(qAbs(m_availableGeometry.width() - m_tlw->frameGeometry().width()), newPos.y());
//			if (qAbs(m_availableGeometry.height() - newPos.y() - m_tlw->frameGeometry().height()) < 15)
//				newPos = QPoint(newPos.x(), qAbs(m_availableGeometry.height() - m_tlw->frameGeometry().height()));*/
//
//			if (qAbs(m_availableGeometry.width() - newPos.x() - this->frameGeometry().width()) < 15)
//				newPos = QPoint(qAbs(m_availableGeometry.width() - this->frameGeometry().width()), newPos.y());
//			if (qAbs(m_availableGeometry.height() - newPos.y() - this->frameGeometry().height()) < 15)
//				newPos = QPoint(newPos.x(), qAbs(m_availableGeometry.height() - this->frameGeometry().height()));
//
//			move(newPos);
//			m_mover = true;
//		}
//		
//        event->accept();
//    }
//	
//	if (!m_mover) {
//
//		//if (!m_mousePressed || m_tlw->testAttribute(Qt::WA_WState_ConfigPending))
//	    if (!m_mousePressed || this->testAttribute(Qt::WA_WState_ConfigPending))
//	        return;
//
//		/*if (m_tlw->isWindow() && ::GetSystemMenu(m_tlw->winId(), FALSE) != 0) {
//	        MSG msg;
//	        while(PeekMessage(&msg, winId(), WM_MOUSEMOVE, WM_MOUSEMOVE, PM_REMOVE));
//	        return;
//	    }*/
//		if (this->isWindow() && ::GetSystemMenu(this->winId(), FALSE) != 0) {
//	        MSG msg;
//	        while(PeekMessage(&msg, winId(), WM_MOUSEMOVE, WM_MOUSEMOVE, PM_REMOVE));
//	        return;
//	    }
//
//		QPoint np(event->globalPos());
//
//	    // Don't extend beyond the available geometry; bound to m_deltaY_max and m_deltaX_max.
//		
//	    QSize ns;
//	    switch(m_resizePosition) {
//			case Top:
//				ns.rheight() = m_rect.height() - qMin(np.y() - m_pos.y(), m_deltaY_max);
//				ns.rwidth() = m_rect.width();
//			break;
//			case Bottom:
//				ns.rheight() = m_rect.height() + qMin(np.y() - m_pos.y(), m_deltaY_max);
//				ns.rwidth() = m_rect.width();
//			break;
//			case Left:
//				ns.rwidth() = m_rect.width() - qMax(np.x() - m_pos.x(), m_deltaX_max);
//				ns.rheight() = m_rect.height();
//			break;
//			case Right:
//				ns.rwidth() = m_rect.width() + qMin(np.x() - m_pos.x(), m_deltaX_max);
//				ns.rheight() = m_rect.height();
//			break;
//			case TopLeft:
//				ns.rwidth() = m_rect.width() - qMax(np.x() - m_pos.x(), m_deltaX_max);
//				ns.rheight() = m_rect.height() - qMax(np.y() - m_pos.y(), m_deltaY_max);
//			break;
//			case BottomLeft:
//				ns.rwidth() = m_rect.width() - qMax(np.x() - m_pos.x(), m_deltaX_max);
//				ns.rheight() = m_rect.height() + qMin(np.y() - m_pos.y(), m_deltaY_max);
//			break;
//			case TopRight:
//				ns.rheight() = m_rect.height() - qMax(np.y() - m_pos.y(), m_deltaY_max);
//				ns.rwidth() = m_rect.width() + qMin(np.x() - m_pos.x(), m_deltaX_max);
//			break;
//			case BottomRight:
//				ns.rwidth() = m_rect.width() + qMin(np.x() - m_pos.x(), m_deltaX_max);
//				ns.rheight() = m_rect.height() + qMin(np.y() - m_pos.y(), m_deltaY_max);
//			break;
//			default:
//				ns.rheight() = m_rect.height();
//				ns.rwidth() = m_rect.width();
//			break;
//		} 
//		ns = ns.expandedTo(QSize(100,32));
//		QPoint p;
//		
//		m_newRect = QRect(p, ns);
//		switch(m_resizePosition) {
//
//			case Bottom:
//				m_newRect.moveTopLeft(m_rect.topLeft());
//				break;
//			case Right:
//				m_newRect.moveTopLeft(m_rect.topLeft());
//				break;
//			case Left:
//				m_newRect.moveTopRight(m_rect.topRight());
//				break;
//			case Top:
//				m_newRect.moveBottomLeft(m_rect.bottomLeft());
//				break;
//			case TopLeft:
//				m_newRect.moveBottomRight(m_rect.bottomRight());
//				break;
//			case BottomLeft:
//				m_newRect.moveTopRight(m_rect.topRight());
//				break;
//			case TopRight:
//				m_newRect.moveBottomLeft(m_rect.bottomLeft());
//				break;
//			case BottomRight:
//				m_newRect.moveTopLeft(m_rect.topLeft());
//				break;
//			
//		} 
//		//if(m_resizePosition != 0) m_tlw->setGeometry(m_newRect);
//		if(m_resizePosition != 0) this->setGeometry(m_newRect);
//	}
//}

 
/*!
	\brief enter event implementation.
*/
void MainWindow::enterEvent(
		QEvent *event				/*!<[in] event */
) {

	Q_UNUSED(event);
}

 
/*!
	\brief leave event implementation.
*/
void MainWindow::leaveEvent(
		QEvent *event				/*!<[in] event */
) {
	Q_UNUSED(event);
}
 
/*!
	\brief resize event implementation.
*/
void MainWindow::resizeEvent(
		QResizeEvent *event			/*!<[in] event */
) {
	//Q_UNUSED(event);
	
	//QTimer::singleShot(10, this, SLOT(getRegion()));
	//m_resizeFrame = true;
	
	QWidget::resizeEvent(event);
}
 
/*!
	\brief close event implementation.
*/
void MainWindow::closeEvent(
		QCloseEvent *event			/*!<[in] event */
) {
	Q_UNUSED(event);

	if (m_settings->mainPower())
		startButtonClickedEvent();

	foreach(QWidget *widget, widgetList) {

		int off = widgetList.indexOf(widget);

		if (widget->isVisible()) {
				
			widget->hide();
			mainBtnList.at(off)->setBtnState(AeroButton::OFF);
			mainBtnList.at(off)->update();
		}
	}
	widgetList.clear();
	mainBtnList.clear();

	if (m_serverWidget) {

		disconnect(m_serverWidget, 0, 0, 0);
		delete m_serverWidget;
		m_serverWidget = NULL;
	}

	if (m_dataEngine) {

		disconnect(m_dataEngine, 0, 0, 0);
		delete m_dataEngine;
		m_dataEngine = NULL;
	}

	/*if (m_cudaInfoWidget && m_cudaPresence) {

		disconnect(m_cudaInfoWidget, 0, 0, 0);
		delete m_cudaInfoWidget;
		m_cudaInfoWidget = NULL;
	}*/

	if (m_oglDisplayPanel) {
		
		disconnect(m_oglDisplayPanel, 0, 0, 0);
		delete m_oglDisplayPanel;
		m_oglDisplayPanel = NULL;
	}

	if (m_radioWidget) {
		
		disconnect(m_radioWidget, 0, 0, 0);
		delete m_radioWidget;
		m_radioWidget = NULL;
	}

	if (m_chirpWidget) {
		
		disconnect(m_chirpWidget, 0, 0, 0);
		delete m_chirpWidget;
		m_chirpWidget = NULL;
	}

	/*if (m_hpsdrWidget) {
		
		disconnect(m_hpsdrWidget, 0, 0, 0);
		delete m_hpsdrWidget;
		m_hpsdrWidget = NULL;
	}*/

	if (m_hpsdrTabWidget) {
		
		disconnect(m_hpsdrTabWidget, 0, 0, 0);
		delete m_hpsdrTabWidget;
		m_hpsdrTabWidget = NULL;
	}

	if (m_graphicOptionsWidget) {
		
		disconnect(m_graphicOptionsWidget, 0, 0, 0);
		delete m_graphicOptionsWidget;
		m_graphicOptionsWidget = NULL;
	}

	if (m_hpsdrServer) {

		disconnect(m_hpsdrServer, 0, 0, 0);
		delete m_hpsdrServer;
		m_hpsdrServer = NULL;
	}

	if (m_msgBrowser) {

		delete m_msgBrowser;
		m_msgBrowser = NULL;
	}

	
}
 
/*!
	\brief show event implementation.
*/
void MainWindow::showEvent(
		QShowEvent *event			/*!<[in] event */
) {
	QWidget::showEvent(event);
}

 
/*!
	\brief paint event implementation.
*/
//void MainWindow::paintEvent(
//		QPaintEvent *event			/*!<[in] event */
//) {
//	/*QPainter p(this);
//
//	p.setRenderHints(QPainter::SmoothPixmapTransform | QPainter::Antialiasing | QPainter::TextAntialiasing, true);
//	if (m_drawShadowFrame) {
//
//		m_dummyLabel->setPixmap(m_originalPixmap);
//	}
//	else {
//		
//		QRect rect = this->geometry();
//
//		QPixmap pixmap = QPixmap(rect.width(), rect.height());
//		pixmap.fill(Qt::black);
//
//		m_dummyLabel->setPixmap(pixmap);
//	}
//	p.end();*/
//
//	QWidget::paintEvent(event);
//}
 
/*!
	\brief key pressed event implementation.
*/
void MainWindow::keyPressEvent(
		QKeyEvent *event			/*!<[in] key event */
) {
	switch (event->key()) {

		case Qt::Key_Escape:

			setMainWindowGeometry();
			return;

		case Qt::Key_1:

			return;
    }
    
    QWidget::keyPressEvent(event);
}

void MainWindow::mousePressEvent(QMouseEvent *event) {

	QWidget::mousePressEvent(event);
}


//***************************************************************************
// NetworkIODialog class

NetworkIODialog::NetworkIODialog(QWidget *parent)
    :   QDialog(parent)
	,	m_settings(Settings::instance())
{
	int btn_width = 74;
	int btn_height = 18;

	m_deviceCards = m_settings->getMetisCardsList();

	setWindowModality(Qt::NonModal);
	setWindowOpacity(0.9);
	setStyleSheet(m_settings->getDialogStyle());

	setMouseTracking(true);

	m_titleFont.setStyleStrategy(QFont::PreferAntialias);
	m_titleFont.setFixedPitch(true);
	m_titleFont.setPixelSize(13);
	m_titleFont.setFamily("Arial");
	m_titleFont.setBold(true);
	

	QVBoxLayout *dialogLayout = new QVBoxLayout(this);

	m_deviceComboBox = new QComboBox(this);
	m_deviceComboBox->setStyleSheet(m_settings->getComboBoxStyle());
	m_deviceComboBox->setMinimumContentsLength(30);
	
	QScopedPointer<QHBoxLayout> titleLayout(new QHBoxLayout);
	QLabel *titleLabel = new QLabel(tr("found more than one device:"), this);
	titleLabel->setFont(m_titleFont);
	titleLabel->setStyleSheet(m_settings->getLabelStyle());
	titleLayout->addWidget(titleLabel);
	dialogLayout->addLayout(titleLayout.data());
	titleLayout.take(); // ownership transferred to dialogLayout

	QScopedPointer<QHBoxLayout> metisDeviceLayout(new QHBoxLayout);
	QLabel *ipAddressLabel = new QLabel(tr("Device (IP Addr):"), this);
	ipAddressLabel->setStyleSheet(m_settings->getLabelStyle());
	metisDeviceLayout->addWidget(ipAddressLabel);
	metisDeviceLayout->addWidget(m_deviceComboBox);
	dialogLayout->addLayout(metisDeviceLayout.data());
	metisDeviceLayout.take(); // ownership transferred to dialogLayout

	/*CHECKED_CONNECT(
		m_metisDeviceComboBox, 
		SIGNAL(activated(int)),
		this, 
		SLOT(metisDeviceChanged(int)));*/

	AeroButton* okBtn = new AeroButton("Ok", this);
	okBtn->setRoundness(10);
	okBtn->setFixedSize(btn_width, btn_height);
	CHECKED_CONNECT(
		okBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(okBtnClicked()));

	AeroButton* cancelBtn = new AeroButton("Cancel", this);
	cancelBtn->setRoundness(10);
	cancelBtn->setFixedSize(btn_width, btn_height);
	CHECKED_CONNECT(
		cancelBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(reject()));

	QHBoxLayout *hbox = new QHBoxLayout;
	hbox->setSpacing(1);
	hbox->addWidget(okBtn);
	hbox->addWidget(cancelBtn);

	dialogLayout->addLayout(hbox);
    
    setLayout(dialogLayout);
}

NetworkIODialog::~NetworkIODialog() {
}

void NetworkIODialog::okBtnClicked() {

	if (m_deviceCards.length() > 0) {
		
		m_settings->setCurrentMetisCard(m_deviceCards.at(m_deviceComboBox->currentIndex()));
		NETWORKDIALOG_DEBUG << "Network device at: " << m_deviceCards.at(m_deviceComboBox->currentIndex()).ip_address.toString() << " selected.";
		accept();
	}
	else
		NETWORKDIALOG_DEBUG << "HPSDR network device list length:" << m_deviceCards.length();
}

void NetworkIODialog::addDeviceComboBoxItem(QString str) {

	m_deviceComboBox->addItem(str);
	m_deviceCards = m_settings->getMetisCardsList();
}

void NetworkIODialog::clearDeviceComboBoxItem() {

	m_deviceComboBox->clear();
}
