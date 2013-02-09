/**
* @file  cusdr_mainWidget.cpp
* @brief main window widget class for cuSDR
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
//#define LOG_NETWORKDIALOG

// use: MAIN_DEBUG
// use: NETWORKDIALOG_DEBUG

//#include <QtGui>
//#include <QHBoxLayout>
//#include <QVBoxLayout>
//#include <QtNetwork>
//#include <QTimer>

#include "cusdr_mainWidget.h"


#define window_height1		600
#define window_height2		750
#define window_width1		800
#define window_width2		1030
#define btn_width1			56//65
#define btn_width2			54
#define btn_width3			48
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
	: QMainWindow(parent)
	, set(Settings::instance())
	, m_serverMode(set->getCurrentServerMode())
	, m_hwInterface(set->getHWInterface())
	, m_dataEngineState(QSDR::DataEngineDown)
	, m_cudaPresence(false)
	, m_mover(false)
	, m_resizePosition(0)
{	
	QPalette palette;
	QColor color = Qt::black;
	color.setAlpha(255);
	palette.setColor(QPalette::Window, color);
	setPalette(palette);

	setAutoFillBackground(true);
	setMouseTracking(true);
	setContentsMargins(0, 0, 0, 0);
	
	m_fullScreen = false;

	// save and reload the windows size and state
	m_windowsSettingsFilename = "windowsSettings.ini";
	QSettings settings(QCoreApplication::applicationDirPath() +  "/" + m_windowsSettingsFilename, QSettings::IniFormat);
	restoreGeometry(settings.value("geometry").toByteArray());
	restoreState(settings.value("windowState").toByteArray());

	// Dock windows options
	setDockOptions(QMainWindow::AnimatedDocks | QMainWindow::AllowNestedDocks);
	setMinimumSize(QSize(window_width1, window_height1));
	setStyleSheet(set->getSDRStyle());

	m_oldSampleRate = set->getSampleRate();
	m_numberOfReceivers = set->getNumberOfReceivers();
	m_alexConfig = set->getAlexConfig();
	m_alexStates = set->getAlexStates();
	m_mercuryAttn = set->getMercuryAttenuators(0);
	m_currentHamBand = set->getCurrentHamBand(0);

	m_alexAttnState = 0x03 & (m_alexStates[m_currentHamBand] >> 7);
	m_mercuryAttnState = m_mercuryAttn.at(m_currentHamBand);

	fonts = new CFonts(this);
	m_fonts = fonts->getFonts();

	// the SDR data engine
	m_dataEngine = new DataEngine(this);

	// control widgets
	m_serverWidget = new ServerWidget(this);
	m_chirpWidget = new ChirpWidget(this);
	m_hpsdrTabWidget = new HPSDRTabWidget(this);
	m_radioTabWidget = new RadioTabWidget(this);
	m_displayTabWidget = new DisplayTabWidget(this);

	m_wbDisplay = 0;

	m_serverWidget->hide();
	m_chirpWidget->hide();
	m_hpsdrTabWidget->hide();
	m_radioTabWidget->hide();
	m_displayTabWidget->hide();

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

	CHECKED_CONNECT_OPT(
		set,
		SIGNAL(systemMessageEvent(const QString&, int)),
		this,
		SLOT(showStatusBarMessage(const QString &, int)),
		Qt::DirectConnection);

	CHECKED_CONNECT(
		m_dataEngine,
		SIGNAL(clearSystemMessageEvent()),
		this,
		SLOT(clearStatusBarMessage()));

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
		set,
		SIGNAL(cpuLoadChanged(short)), 
		this, 
		SLOT(updateStatusBar(short)));

	CHECKED_CONNECT(
		set,
		SIGNAL(masterSwitchChanged(QObject *, bool)), 
		this, 
		SLOT(masterSwitchChanged(QObject *, bool)));

	CHECKED_CONNECT(
		set,
		SIGNAL(numberOfRXChanged(QObject *, int)), 
		this, 
		SLOT(setNumberOfReceivers(QObject *, int)));

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
		SIGNAL(currentReceiverChanged(QObject *, int)),
		this,
		SLOT(setCurrentReceiver(QObject *, int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(callsignChanged()),
		this,
		SLOT(updateTitle()));

	CHECKED_CONNECT(
		set,
		SIGNAL(showNetworkIO()),
		this,
		SLOT(showNetworkIODialog()));

	CHECKED_CONNECT(
		set,
		SIGNAL(showWarning(const QString &)),
		this,
		SLOT(showWarningDialog(const QString &)));

	CHECKED_CONNECT(
		set,
		SIGNAL(networkIOComboBoxEntryAdded(QString)),
		this,
		SLOT(addNetworkIOComboBoxEntry(QString)));

	CHECKED_CONNECT(
		set,
		SIGNAL(clearNetworkIOComboBoxEntrySignal()),
		this,
		SLOT(clearNetworkIOComboBoxEntry()));
		
	CHECKED_CONNECT(
		set,
		SIGNAL(showRadioPopupChanged(bool)),
		this,
		SLOT(showRadioPopup(bool)));

	CHECKED_CONNECT(
		set,
		SIGNAL(txAllowedChanged(QObject *, bool)),
		this,
		SLOT(setTxAllowed(QObject *, bool)));

	CHECKED_CONNECT(
		set,
		SIGNAL(agcModeChanged(QObject *, int, AGCMode, bool)),
		this,
		SLOT(setAGCMode(QObject *, int, AGCMode, bool)));

	CHECKED_CONNECT(
		set,
		SIGNAL(agcMaximumGainChanged_dB(QObject *, int, qreal)),
		this,
		SLOT(setAGCGain(QObject *, int, qreal)));

	CHECKED_CONNECT(
		set,
		SIGNAL(agcFixedGainChanged_dB(QObject *, int, qreal)),
		this,
		SLOT(setAGCGain(QObject *, int, qreal)));

	CHECKED_CONNECT(
		set,
		SIGNAL(mercuryAttenuatorChanged(QObject *, HamBand, int)),
		this,
		SLOT(mercuryAttenuatorChanged(QObject *, HamBand, int)));

	CHECKED_CONNECT(
		set,
		SIGNAL(alexPresenceChanged(bool)),
		this,
		SLOT(alexPresenceChanged(bool)));

	CHECKED_CONNECT(
		set,
		SIGNAL(alexConfigurationChanged(quint16)),
		this,
		SLOT(alexConfigurationChanged(quint16)));

	CHECKED_CONNECT(
		set,
		SIGNAL(alexStateChanged(HamBand, const QList<int> &)),
		this,
		SLOT(alexStateChanged(HamBand, const QList<int> &)));
}

/*!
	\brief setup the main window (called in main.cpp):
	- create main button group
	- create dock widgets
	- create mode menus
	- setup main layout
	- setup connections
	- setup dialog windows
	- get network interfaces
*/
void MainWindow::setup() {
	
	//runFFTWWisdom();

	// create the big display panel at the top of the application.	
	createDisplayPanelToolBar();

	// create the main buttons tool bar
	createMainBtnToolBar();

	// create the status tool bar
	createStatusToolBar();

	// create the mode menu
	createModeMenu();

	// create the view menu
	createViewMenu();

	// create the attenuator menu
	createAttenuatorMenu();
	
	// the wideband display
	m_wbDisplay = new QGLWidebandPanel(this);

	// create the receiver panels
	createReceiverPanels(MAX_RECEIVERS);
	
	// setup the layout for the control widgets, the wideband panel and the receiver panels
	setupLayout();

	// set the main window title
	updateTitle();

	// show the wideband data panel as specified in the settings.ini
	if (set->getWidebandData()) {

		wideBandBtn->setEnabled(true);

		if (set->getWidebandStatus()) {
			wideBandBtnClickedEvent();
		}
	}
	
	// get the network interfaces
	getNetworkInterfaces();

	// init network IO dialog to HPSDR components
	m_netIODialog = new NetworkIODialog();

	// init warning dialog
	m_warningDialog = new WarningDialog();

	// setup connection for the NIC lists of the server and hpsdr widgets.
	// We need to add these connections after detecting the network interfaces.
	m_serverWidget->addNICChangedConnection();
	m_hpsdrTabWidget->addNICChangedConnection();
	
	// experimental:
	// check for OpenCL devices
	//QList<QCLDevice> clDevices = QCLDevice::allDevices();
	//if (clDevices.length() == 0)
	//	showMessage("[main]: no OpenCL devices found.");

	//else {

	//	m_message = "[main]: found %1 OpenCL device(s).";
	//	showStatusBarMessage(m_message.arg(clDevices.length()), 5000);
	//	//QString clNo = QString::number(m_clDevices.length());
	//}
	//set->setOpenCLDevices(clDevices);

	// set the centralwidget as the central widget of the main window,
	// i.e., we have a second QMainWindow as the central widget.
	setCentralWidget(centralwidget);

	// update the display panel
	m_oglDisplayPanel->updateGL();

	// set the Alex configuration
	alexConfigurationChanged(m_alexConfig);

	// set the value of the attenuator(s)
	setAttenuatorButton();

	m_agcMode = set->getAGCMode(0);

	// initialize all Signal/Slot connections
	setupConnections();

	updateFromSettings();
}

//void MainWindow::runFFTWWisdom() {
//
//	QString directory = QDir::currentPath();
//	
//	m_currentDir = QDir(directory);
//	qDebug() << m_currentDir;
//	
//	if (m_currentDir.exists("wisdom")) {
//	
//		qDebug() << "wisdom exists !";
//		return;
//	}
//	else {
//	
//		qDebug() << "wisdom does not exist - running fftw-wisdom.exe ...";
//		QProcess process;
//		process.start("fftwf-wisdom.exe");
//		process.waitForFinished();
//	}
//}
 
/*!
	\brief updates the OpenGL widget.
*/
void MainWindow::update() {

	//if (m_oglWidget)
	//	m_oglWidget->update();
}

/*!
	\brief set up the main layout.
*/
void MainWindow::setupLayout() {

	centralwidget = new QMainWindow(this);
	centralwidget->setWindowFlags(Qt::Widget);
	centralwidget->setDockOptions(QMainWindow::AnimatedDocks | QMainWindow::AllowNestedDocks);
	centralwidget->setStyleSheet(set->getMainWindowStyle());
	centralwidget->setContextMenuPolicy(Qt::NoContextMenu);  //setStyleSheet(set->getMenuStyle());

	// radio control widget
	QDockWidget *dock = new QDockWidget(tr("Radio Ctrl"), this);
	dock->setObjectName("RadioCtrl");
    dock->setAllowedAreas(Qt::LeftDockWidgetArea | Qt::RightDockWidgetArea);
	dock->setFeatures(QDockWidget::NoDockWidgetFeatures);
	//dock->setFeatures(QDockWidget::DockWidgetFloatable);
	dock->setStyleSheet(set->getDockStyle());
	dock->setMaximumWidth(245);
	dock->setMinimumWidth(245);
	dock->setWidget(m_radioTabWidget);
	dockWidgetList.append(dock);

    addDockWidget(Qt::RightDockWidgetArea, dock);
	dock->hide();

	// server control widget
	dock = new QDockWidget(tr("Server Ctrl"), this);
	dock->setObjectName("ServerCtrl");
    dock->setAllowedAreas(Qt::LeftDockWidgetArea | Qt::RightDockWidgetArea);
	dock->setFeatures(QDockWidget::NoDockWidgetFeatures);
	//dock->setFeatures(QDockWidget::DockWidgetFloatable);
	dock->setStyleSheet(set->getDockStyle());
	dock->setMaximumWidth(245);
	dock->setMinimumWidth(245);
	dock->setWidget(m_serverWidget);
	dockWidgetList.append(dock);

    addDockWidget(Qt::RightDockWidgetArea, dock);
	dock->hide();

	// HPSDR hardware control widget
	dock = new QDockWidget(tr("HPSDR Ctrl"), this);
	dock->setObjectName("HPSDRCtrl");
    dock->setAllowedAreas(Qt::LeftDockWidgetArea | Qt::RightDockWidgetArea);
	dock->setFeatures(QDockWidget::NoDockWidgetFeatures);
	//dock->setFeatures(QDockWidget::DockWidgetFloatable);
	dock->setStyleSheet(set->getDockStyle());
	dock->setMaximumWidth(245);
	dock->setMinimumWidth(245);
	dock->setWidget(m_hpsdrTabWidget);
	dockWidgetList.append(dock);

    addDockWidget(Qt::RightDockWidgetArea, dock);
	dock->hide();

	// chirp mode control widget
	dock = new QDockWidget(tr("Chirp Ctrl"), this);
	dock->setObjectName("ChirpCtrl");
    dock->setAllowedAreas(Qt::LeftDockWidgetArea | Qt::RightDockWidgetArea);
	dock->setFeatures(QDockWidget::NoDockWidgetFeatures);
	dock->setStyleSheet(set->getDockStyle());
	dock->setMaximumWidth(245);
	dock->setMinimumWidth(245);
	dock->setWidget(m_chirpWidget);
	dockWidgetList.append(dock);

    addDockWidget(Qt::RightDockWidgetArea, dock);
	dock->hide();

	// display control widget
	dock = new QDockWidget(tr("Display Ctrl"), this);
	dock->setObjectName("DisplayCtrl");
    dock->setAllowedAreas(Qt::LeftDockWidgetArea | Qt::RightDockWidgetArea);
	dock->setFeatures(QDockWidget::NoDockWidgetFeatures);
	dock->setStyleSheet(set->getDockStyle());
	dock->setMaximumWidth(245);
	dock->setMinimumWidth(245);
	dock->setWidget(m_displayTabWidget);
	dockWidgetList.append(dock);

    addDockWidget(Qt::RightDockWidgetArea, dock);
	dock->hide();

	// receiver and wideband panel docks;
	// set receiver 0 as the main receiver
	centralwidget->setCentralWidget(rxWidgetList.at(0));

	// wideband panel dock window
	widebandDock = new QDockWidget(tr("Wideband"), this);
	widebandDock->setObjectName("Wideband");
	widebandDock->setAllowedAreas(Qt::TopDockWidgetArea | Qt::BottomDockWidgetArea);
    widebandDock->setFeatures(QDockWidget::DockWidgetClosable | QDockWidget::DockWidgetFloatable | QDockWidget::DockWidgetMovable);
	widebandDock->setStyleSheet(set->getDockStyle());
    widebandDock->setWidget(m_wbDisplay);
	
	centralwidget->addDockWidget(Qt::TopDockWidgetArea, widebandDock);
	widebandDock->hide();

	CHECKED_CONNECT(
		widebandDock,
		SIGNAL(visibilityChanged(bool)),
		this,
		SLOT(widebandVisibilityChanged(bool)));

	// receiver dock windows
	for (int i = 1; i < MAX_RECEIVERS; i++) {

		QString str = "Receiver ";
		QString num = QString::number(i+1);
		str.append(num);
		dock = new QDockWidget(str, this);
		widebandDock->setObjectName(str);
		dock->setStyleSheet(set->getDockStyle());
		dock->setWidget(rxWidgetList.at(i));
		rxDockWidgetList.append(dock);

		centralwidget->addDockWidget(Qt::BottomDockWidgetArea, dock);
		dock->hide();

		rxVolumeList << 0.0f;
		//viewMenu->addAction(dock->toggleViewAction());
	}

	// the outline of the receiver panels
	for (int i = 0; i < (int)(MAX_RECEIVERS-1)/2; i++) {

		centralwidget->splitDockWidget(rxDockWidgetList.at(i), rxDockWidgetList.at(i+3), Qt::Vertical);
	}

	//viewMenu->addAction(dock->toggleViewAction());
}

/*!
	\brief create the receiver panels.
*/
void MainWindow::createReceiverPanels(int rx) {

	rxWidgetList.clear();
	
	for (int i = 0; i < rx; i++) {
	
		QGLReceiverPanel* rxPanel = new QGLReceiverPanel(this, i);
		rxWidgetList.append(rxPanel);
    }
}

/*!
	\brief create the status tool bar.
*/
void MainWindow::createStatusToolBar() {

	QDateTime dateTime = QDateTime::currentDateTime();
	m_dateTimeString = dateTime.toString();
	m_dateTimeString.append(" (loc)");

	m_cpuLoadString = "CPU load:     ";
	m_cpuLoadLabel = new QLabel(m_cpuLoadString, this);
	m_cpuLoadLabel->setStyleSheet(set->getLabelStyle());

	m_dateTimeLabel = new QLabel(m_dateTimeString, this);
	m_dateTimeLabel->setStyleSheet(set->getLabelStyle());

	statusBar()->setStyleSheet(set->getStatusbarStyle());
	statusBar()->addPermanentWidget(m_cpuLoadLabel);
	statusBar()->insertPermanentWidget(1, m_dateTimeLabel, 0);
}

/*!
	\brief update the staus tool bar content with
	the CPU load and the local date & time.
*/
void MainWindow::updateStatusBar(short load) {

	QString str = "CPU load: %1 % \t";
	m_cpuLoadLabel->setText(str.arg(load));

	QDateTime dateTime = QDateTime::currentDateTime();
	m_dateTimeString = dateTime.toString();
	m_dateTimeString.append(" (loc)");
	m_dateTimeLabel->setText(m_dateTimeString);

	statusBar()->update();
}

/*!
	\brief create the display panel tool bar.
*/
void MainWindow::createDisplayPanelToolBar() {

	displayPanelToolBar = new QToolBar(tr("Display Panel"), this);
	displayPanelToolBar->setObjectName("DisplayPanel");
	displayPanelToolBar->setAllowedAreas(Qt::TopToolBarArea);
	displayPanelToolBar->setMovable(false);
	displayPanelToolBar->setStyleSheet(set->getDisplayToolbarStyle());

	// The display panel has the displayPanelToolBar as parent.
	m_oglDisplayPanel = new OGLDisplayPanel(displayPanelToolBar);

	displayPanelToolBar->addWidget(m_oglDisplayPanel);

	// add displayPanelToolBar to the main window.
	addToolBar(displayPanelToolBar);
	addToolBarBreak(Qt::TopToolBarArea);
}

/*!
	\brief create the main button tool bar.
*/
void MainWindow::createMainBtnToolBar() {

	mainBtnToolBar = new QToolBar(tr("Main Buttons"), this);
	mainBtnToolBar->setObjectName("MainButtons");
	//mainBtnToolBar->setAllowedAreas(Qt::TopToolBarArea);
	mainBtnToolBar->setMovable(false);
	mainBtnToolBar->setStyleSheet(set->getMainBtnToolbarStyle());
	
	m_buttonWidget = new QWidget(this);

    QColor btnCol = QColor(230, 230, 230);
//	QHBoxLayout *firstBtnLayout = new QHBoxLayout;
//	firstBtnLayout->setSpacing(0);
//	firstBtnLayout->setMargin(0);
	
	startBtn = new AeroButton("Start", this);
	startBtn->setRoundness(10);
    startBtn->setFont(m_fonts.normalFont);
    startBtn->setTextColor(btnCol);
	startBtn->setFixedSize(btn_width1, btn_height1);

	CHECKED_CONNECT(
		startBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(startButtonClickedEvent()));

	/*ctrlDisplayBtn = new AeroButton("Radio", this);
	ctrlDisplayBtn->setRoundness(10);
	ctrlDisplayBtn->setFixedSize(btn_width1, btn_height1);
	ctrlDisplayBtn->setBtnState(AeroButton::ON);

	CHECKED_CONNECT(
		ctrlDisplayBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(ctrlDisplayBtnClickedEvent()));*/

	rxCtrlBtn = new AeroButton("Radio Ctrl", this);
	rxCtrlBtn->setRoundness(10);
    rxCtrlBtn->setFont(m_fonts.normalFont);
    rxCtrlBtn->setTextColor(btnCol);
	rxCtrlBtn->setFixedSize(btn_width1, btn_height1);
	mainBtnList.append(rxCtrlBtn);

	//if (m_serverMode == QSDR::ExternalDSP)
	//	rxCtrlBtn->setEnabled(false);

	CHECKED_CONNECT(
		rxCtrlBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(widgetBtnClickedEvent()));

	serverBtn = new AeroButton("Server", this);
	serverBtn->setRoundness(10);
    serverBtn->setFont(m_fonts.normalFont);
    serverBtn->setTextColor(btnCol);
	serverBtn->setFixedSize(btn_width1, btn_height1);
	mainBtnList.append(serverBtn);

	CHECKED_CONNECT(
		serverBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(widgetBtnClickedEvent()));

	hpsdrBtn = new AeroButton("HPSDR", this);
	hpsdrBtn->setRoundness(10);
    hpsdrBtn->setFont(m_fonts.normalFont);
    hpsdrBtn->setTextColor(btnCol);
	hpsdrBtn->setFixedSize(btn_width1, btn_height1);
	mainBtnList.append(hpsdrBtn);

	CHECKED_CONNECT(
		hpsdrBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(widgetBtnClickedEvent()));

	chirpBtn = new AeroButton("Chirp", this);
	chirpBtn->setRoundness(10);
    chirpBtn->setFont(m_fonts.normalFont);
    chirpBtn->setTextColor(btnCol);
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
    wideBandBtn->setFont(m_fonts.normalFont);
    wideBandBtn->setTextColor(btnCol);
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

	//	//if (set->getCudaDevices() == 0)
	//}
	//else {

	//	cudaBtn->setEnabled(false);
	//}
	
	//if (set->getCudaDevices() == 0) {

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
    displayBtn->setFont(m_fonts.normalFont);
    displayBtn->setTextColor(btnCol);
	displayBtn->setFixedSize(btn_width1, btn_height1);
	mainBtnList.append(displayBtn);

	CHECKED_CONNECT(
		displayBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(widgetBtnClickedEvent()));

	QColor col = QColor(90, 90, 90);

	nullBtn = new AeroButton(this);
	nullBtn->setRoundness(0);
	nullBtn->setFixedHeight(btn_height1);
	nullBtn->setHighlight(col);
	nullBtn->setEnabled(false);

	/*plusRxBtn = new AeroButton("+Rx", this);
	plusRxBtn->setRoundness(10);
    plusRxBtn->setFont(m_fonts.normalFont);
    plusRxBtn->setTextColor(btnCol);
	plusRxBtn->setFixedSize(btn_width1, btn_height1);

	CHECKED_CONNECT(
		plusRxBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(addReceiver()));*/

	viewBtn = new AeroButton("View Rx", this);
	viewBtn->setRoundness(10);
    viewBtn->setFont(m_fonts.normalFont);
    viewBtn->setTextColor(btnCol);
	viewBtn->setFixedSize(btn_width1, btn_height1);

	modeBtn = new AeroButton("Mode", this);
	modeBtn->setRoundness(10);
    modeBtn->setFont(m_fonts.normalFont);
    modeBtn->setTextColor(btnCol);
	modeBtn->setFixedSize(btn_width1, btn_height1);

	quitBtn = new AeroButton("Quit", this);
	quitBtn->setRoundness(10);
    quitBtn->setFont(m_fonts.normalFont);
    quitBtn->setTextColor(btnCol);
	quitBtn->setFixedSize(btn_width3, btn_height1);

	CHECKED_CONNECT(
		quitBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(closeMainWindow()));
			
//	firstBtnLayout->addWidget(startBtn);
//	//firstBtnLayout->addWidget(serverLogBtn);
//	firstBtnLayout->addWidget(rxCtrlBtn);
//	firstBtnLayout->addWidget(serverBtn);
//	firstBtnLayout->addWidget(hpsdrBtn);
//	firstBtnLayout->addWidget(chirpBtn);
//	firstBtnLayout->addWidget(wideBandBtn);
//	//firstBtnLayout->addWidget(openclBtn);
//	firstBtnLayout->addWidget(displayBtn);
//	firstBtnLayout->addWidget(nullBtn);
//	//firstBtnLayout->addStretch();
//	firstBtnLayout->addWidget(viewBtn);
//	firstBtnLayout->addWidget(modeBtn);
//	firstBtnLayout->addWidget(quitBtn);
	
//	QHBoxLayout *firstBtnLayout = new QHBoxLayout;
//	firstBtnLayout->setSpacing(0);
//	firstBtnLayout->setMargin(0);
//
//	firstBtnLayout->addWidget(startBtn);
//	//firstBtnLayout->addWidget(serverLogBtn);
//	firstBtnLayout->addWidget(rxCtrlBtn);
//	firstBtnLayout->addWidget(serverBtn);
//	firstBtnLayout->addWidget(hpsdrBtn);
//	firstBtnLayout->addWidget(chirpBtn);
//	firstBtnLayout->addWidget(wideBandBtn);
//	//firstBtnLayout->addWidget(openclBtn);
//	firstBtnLayout->addWidget(displayBtn);
//	firstBtnLayout->addWidget(nullBtn);
//	//firstBtnLayout->addStretch();
//	firstBtnLayout->addWidget(viewBtn);
//	firstBtnLayout->addWidget(modeBtn);
//	firstBtnLayout->addWidget(quitBtn);

//	QHBoxLayout *secondBtnLayout = new QHBoxLayout;
//	secondBtnLayout->setSpacing(0);
//	secondBtnLayout->setMargin(0);

	int fontMaxWidth = m_fonts.smallFontMetrics->boundingRect("100 % ").width();
	int vol = (int)(set->getMainVolume(0) * 100);

	m_volumeSlider = new QSlider(Qt::Horizontal, this);
	m_volumeSlider->setTickPosition(QSlider::NoTicks);
	m_volumeSlider->setFixedSize(100, 14);
	m_volumeSlider->setSingleStep(1);
	m_volumeSlider->setRange(0, 100);
	m_volumeSlider->setValue(vol);
	m_volumeSlider->setStyleSheet(set->getVolSliderStyle());

	CHECKED_CONNECT(
		m_volumeSlider, 
		SIGNAL(valueChanged(int)), 
		this, 
		SLOT(setMainVolume(int)));

	m_volumeLabel = new QLabel("Vol:", this);
    m_volumeLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_volumeLabel->setStyleSheet(set->getLabelStyle());

	QString str = "%1 %";
	m_volLevelLabel = new QLabel(str.arg(vol, 2, 10, QLatin1Char(' ')), this);
	m_volLevelLabel->setFont(m_fonts.smallFont);
	m_volLevelLabel->setFixedSize(fontMaxWidth, 14);
    m_volLevelLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_volLevelLabel->setStyleSheet(set->getSliderLabelStyle());

	
	int agcMaxGain = (int) set->getAGCMaximumGain_dB(0);
	//MAIN_DEBUG << "agcMaximumGain = " << agcMaxGain;
	m_agcGainSlider = new QSlider(Qt::Horizontal, this);
	m_agcGainSlider->setTickPosition(QSlider::NoTicks);
	m_agcGainSlider->setFixedSize(100, 14);
	m_agcGainSlider->setSingleStep(1);
	m_agcGainSlider->setRange(-20, 120);
	m_agcGainSlider->setValue(agcMaxGain);
	m_agcGainSlider->setStyleSheet(set->getVolSliderStyle());
	
	CHECKED_CONNECT(
		m_agcGainSlider, 
		SIGNAL(valueChanged(int)), 
		this, 
		SLOT(setAGCGain(int)));

	m_agcGainLabel = new QLabel("", this);
    m_agcGainLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_agcGainLabel->setStyleSheet(set->getLabelStyle());
	if(m_agcMode == (AGCMode) agcOFF)
		m_agcGainLabel->setText("AGC-F:");
	else
		m_agcGainLabel->setText("AGC-G:");

	fontMaxWidth = m_fonts.smallFontMetrics->boundingRect(" 120 dB").width();
	str = " %1 dB";
	m_agcGainLevelLabel = new QLabel(str.arg(agcMaxGain + 0, 2, 10, QLatin1Char(' ')), this);
	m_agcGainLevelLabel->setFont(m_fonts.smallFont);
	m_agcGainLevelLabel->setFixedSize(fontMaxWidth, 14);
    m_agcGainLevelLabel->setFrameStyle(QFrame::Box | QFrame::Raised);
	m_agcGainLevelLabel->setStyleSheet(set->getSliderLabelStyle());
	

	moxBtn = new AeroButton("MOX", this);
	moxBtn->setRoundness(10);
    moxBtn->setFont(m_fonts.normalFont);
    moxBtn->setTextColor(btnCol);
	moxBtn->setFixedSize(btn_width1, btn_height3);
	moxBtn->setEnabled(false);
	col = QColor(200, 100, 100);
	moxBtn->setColor(col);
	col = QColor(200, 200, 100);
	moxBtn->setHighlight(col);
	col = QColor(250, 100, 100);
	moxBtn->setColorOn(col);
	moxBtn->setBtnState(AeroButton::OFF);

	tunBtn = new AeroButton("Tune", this);
	tunBtn->setRoundness(10);
    tunBtn->setFont(m_fonts.normalFont);
    tunBtn->setTextColor(btnCol);
	tunBtn->setFixedSize(btn_width1, btn_height3);
	tunBtn->setEnabled(false);
	col = QColor(200, 100, 100);
	tunBtn->setColor(col);
	col = QColor(200, 200, 100);
	tunBtn->setHighlight(col);
	col = QColor(250, 100, 100);
	tunBtn->setColorOn(col);
	tunBtn->setBtnState(AeroButton::OFF);

	if (set->getPenelopePresence() || set->getPennyLanePresence() || (m_hwInterface == QSDR::Hermes)) {

		moxBtn->setEnabled(true);
		tunBtn->setEnabled(true);
	}
	else {

		moxBtn->setEnabled(false);
		tunBtn->setEnabled(false);
	}

	alexBtn = new AeroButton("Alex Auto", this);
	alexBtn->setRoundness(10);
    alexBtn->setFont(m_fonts.normalFont);
    alexBtn->setTextColor(btnCol);
	alexBtn->setFixedSize(btn_width1, btn_height3);
	alexBtn->setBtnState(AeroButton::ON);
	if (set->getAlexPresence())
		alexBtn->setEnabled(true);
	else
		alexBtn->setEnabled(false);

	CHECKED_CONNECT(
		alexBtn,
		SIGNAL(clicked()),
		this,
		SLOT(alexBtnClickedEvent()));

	attenuatorBtn = new AeroButton("Attenuator", this);
	attenuatorBtn->setRoundness(10);
    attenuatorBtn->setFont(m_fonts.normalFont);
    attenuatorBtn->setTextColor(btnCol);
	attenuatorBtn->setFixedSize(btn_width1, btn_height1);

	muteBtn = new AeroButton("Mute", this);
	muteBtn->setRoundness(10);
    muteBtn->setFont(m_fonts.normalFont);
    muteBtn->setTextColor(btnCol);
	muteBtn->setFixedSize(btn_width3, btn_height1);

	CHECKED_CONNECT(
		muteBtn,
		SIGNAL(clicked()),
		this,
		SLOT(muteBtnClickedEvent()));

//	lastFreqBtn = new AeroButton(" ", this);
//	lastFreqBtn->setRoundness(10);
//	lastFreqBtn->setFixedSize(btn_width1, btn_height3);
//	lastFreqBtn->setBtnState(AeroButton::OFF);
//
//	CHECKED_CONNECT(
//		lastFreqBtn,
//		SIGNAL(clicked()),
//		this,
//		SLOT(getLastFrequency()));

	QHBoxLayout *firstBtnLayout = new QHBoxLayout;
	firstBtnLayout->setSpacing(0);
	firstBtnLayout->setMargin(0);

	firstBtnLayout->addWidget(startBtn);
	//firstBtnLayout->addWidget(serverLogBtn);
	firstBtnLayout->addWidget(rxCtrlBtn);
	firstBtnLayout->addWidget(serverBtn);
	firstBtnLayout->addWidget(hpsdrBtn);
	firstBtnLayout->addWidget(chirpBtn);
	firstBtnLayout->addWidget(wideBandBtn);
	//firstBtnLayout->addWidget(openclBtn);
	firstBtnLayout->addWidget(displayBtn);
	firstBtnLayout->addWidget(nullBtn);
	//firstBtnLayout->addWidget(plusRxBtn);
	firstBtnLayout->addWidget(viewBtn);
	firstBtnLayout->addWidget(modeBtn);
	firstBtnLayout->addWidget(quitBtn);


	QHBoxLayout* secondBtnLayout = new QHBoxLayout;
	secondBtnLayout->setSpacing(0);
	secondBtnLayout->setMargin(0);

	secondBtnLayout->addWidget(moxBtn);
	secondBtnLayout->addWidget(tunBtn);
	secondBtnLayout->addStretch();
	secondBtnLayout->addWidget(alexBtn);
	secondBtnLayout->addWidget(attenuatorBtn);
	secondBtnLayout->addSpacing(5);
	secondBtnLayout->addWidget(m_agcGainLabel);
	secondBtnLayout->addWidget(m_agcGainSlider);
	secondBtnLayout->addWidget(m_agcGainLevelLabel);
	secondBtnLayout->addSpacing(10);
	secondBtnLayout->addWidget(m_volumeLabel);
	secondBtnLayout->addWidget(m_volumeSlider);
	secondBtnLayout->addWidget(m_volLevelLabel);
	secondBtnLayout->addSpacing(2);
	secondBtnLayout->addWidget(muteBtn);
	//secondBtnLayout->addWidget(lastFreqBtn);
	
	/*QHBoxLayout *thirdBtnLayout = new QHBoxLayout;
	thirdBtnLayout->setSpacing(0);
	thirdBtnLayout->setMargin(0);

	thirdBtnLayout->addWidget(lockPanBtn);
	thirdBtnLayout->addStretch();*/

	QVBoxLayout* btnLayout = new QVBoxLayout;
	btnLayout->setSpacing(0);
	btnLayout->setMargin(0);
	btnLayout->addLayout(firstBtnLayout);
	btnLayout->addLayout(secondBtnLayout);
	//btnLayout->addLayout(thirdBtnLayout);

	m_buttonWidget->setLayout(btnLayout);

	mainBtnToolBar->addWidget(m_buttonWidget);

	addToolBar(mainBtnToolBar);
}

/*!
	\brief create the Mode menu.
*/
void MainWindow::createModeMenu() {

	modeMenu = new QMenu(this);
	modeMenu->setStyleSheet(set->getMenuStyle());
    modeBtn->setMenu(modeMenu);

	modeActionGroup = new QActionGroup(this);
    modeActionGroup->setExclusive(true);

//    noServerModeAction = modeActionGroup->addAction(tr("QtDSP"));
//    noServerModeAction->setCheckable(true);
//    noServerModeAction->setChecked(m_serverMode == QSDR::QtDSP);

    sdrModeAction = modeActionGroup->addAction(tr("SDR"));
    sdrModeAction->setCheckable(true);
    sdrModeAction->setChecked(m_serverMode == QSDR::SDRMode);

    chirpWSPRAction = modeActionGroup->addAction(tr("ChirpWSPR"));
    chirpWSPRAction->setCheckable(false);
    chirpWSPRAction->setChecked(m_serverMode == QSDR::ChirpWSPR);
	
    modeMenu->addActions(modeActionGroup->actions());

    if (sdrModeAction->isCheckable()) {
		
		CHECKED_CONNECT(
			sdrModeAction,
			SIGNAL(triggered(bool)), 
			this, 
			SLOT(setSDRMode(bool)));
	}

	if (chirpWSPRAction->isCheckable()) {
		
		CHECKED_CONNECT(
			chirpWSPRAction,
			SIGNAL(triggered(bool)), 
			this, 
			SLOT(setChirpWSPRMode(bool)));
	}

}
 
/*!
	\brief create the receiver's dock windows view menu.
*/
void MainWindow::createViewMenu() {

	viewMenu = new QMenu(this);
	viewMenu->setStyleSheet(set->getMenuStyle());
    viewBtn->setMenu(viewMenu);
}

/*!
	\brief create the attenuator menu.
*/
void MainWindow::createAttenuatorMenu() {

	attenuatorMenu = new QMenu(this);
	attenuatorMenu->setStyleSheet(set->getMenuStyle());
	attenuatorBtn->setMenu(attenuatorMenu);

	mercuryAttn_0dBAction = attenuatorMenu->addAction(tr("Mercury Attn 0 dB"));
	mercuryAttn_0dBAction->setCheckable(true);
	mercuryAttnActionList.append(mercuryAttn_0dBAction);

	mercuryAttn_20dBAction = attenuatorMenu->addAction(tr("Mercury Attn -20 dB"));
	mercuryAttn_20dBAction->setCheckable(true);
	mercuryAttnActionList.append(mercuryAttn_20dBAction);

	attenuatorMenu->addSeparator();
	alexAttn_0dBAction = attenuatorMenu->addAction(tr("Alex Attn 0 dB"));
	alexAttn_0dBAction->setCheckable(true);
	alexAttnActionList.append(alexAttn_0dBAction);

	alexAttn_10dBAction = attenuatorMenu->addAction(tr("Alex Attn -10 dB"));
	alexAttn_10dBAction->setCheckable(true);
	alexAttnActionList.append(alexAttn_10dBAction);

	alexAttn_20dBAction = attenuatorMenu->addAction(tr("Alex Attn -20 dB"));
	alexAttn_20dBAction->setCheckable(true);
	alexAttnActionList.append(alexAttn_20dBAction);

	alexAttn_30dBAction = attenuatorMenu->addAction(tr("Alex Attn -30 dB"));
	alexAttn_30dBAction->setCheckable(true);
	alexAttnActionList.append(alexAttn_30dBAction);

    if (mercuryAttn_0dBAction->isCheckable()) {

		CHECKED_CONNECT(
			mercuryAttn_0dBAction,
			SIGNAL(triggered(bool)),
			this,
			SLOT(setAttenuator()));
	}

    if (mercuryAttn_20dBAction->isCheckable()) {

		CHECKED_CONNECT(
			mercuryAttn_20dBAction,
			SIGNAL(triggered(bool)),
			this,
			SLOT(setAttenuator()));
	}

    if (alexAttn_0dBAction->isCheckable()) {

		CHECKED_CONNECT(
			alexAttn_0dBAction,
			SIGNAL(triggered(bool)),
			this,
			SLOT(setAttenuator()));
	}

    if (alexAttn_10dBAction->isCheckable()) {

		CHECKED_CONNECT(
			alexAttn_10dBAction,
			SIGNAL(triggered(bool)),
			this,
			SLOT(setAttenuator()));
	}

    if (alexAttn_20dBAction->isCheckable()) {

		CHECKED_CONNECT(
			alexAttn_20dBAction,
			SIGNAL(triggered(bool)),
			this,
			SLOT(setAttenuator()));
	}

    if (alexAttn_30dBAction->isCheckable()) {

		CHECKED_CONNECT(
			alexAttn_30dBAction,
			SIGNAL(triggered(bool)),
			this,
			SLOT(setAttenuator()));
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

		if (m_dataEngine->initDataEngine()) { // start data engine
		
			//if (m_serverMode == QSDR::ExternalDSP && !m_hpsdrServer->startServer())
			//	m_hpsdrServer->stopServer();
			return;
		}
		else {

			set->setMainPower(this, false);
			startButtonClickedEvent();
			return;
		}
	}
	else {

		m_dataEngine->stop();

		//if (m_serverMode == QSDR::ExternalDSP)
		//	m_hpsdrServer->stopServer();
		set->setMainPower(this, false);
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

	//if (sender == this) return;

	if (m_hwInterface != hwmode)
		m_hwInterface = hwmode;

	if (m_serverMode != mode)
		m_serverMode = mode;

	if (m_dataEngineState != state)
		m_dataEngineState = state;

	//if (!modeBtn->isEnabled() && m_dataEngineState == QSDR::DataEngineDown)
	//	modeBtn->setEnabled(true);
	modeBtn->setEnabled(m_dataEngineState == QSDR::DataEngineDown);
	moxBtn->setEnabled(m_hwInterface == QSDR::Hermes);
	tunBtn->setEnabled(m_hwInterface == QSDR::Hermes);

	/*
	if (state == QSDR::DataEngineUp) {

		m_dataEngineState = QSDR::DataEngineUp;
		modeBtn->setEnabled(false);
	}
	else
	if (state == QSDR::DataEngineDown) {

		m_dataEngineState = QSDR::DataEngineDown;
		modeBtn->setEnabled(true);
		//setCurrentReceiver(0);
	}
	*/
}

void MainWindow::setSystemState(
			QSDR::_Error err,
			QSDR::_HWInterfaceMode hwmode,
			QSDR::_ServerMode mode,
			QSDR::_DataEngineState state)
{
	m_dataEngine->io.networkIOMutex.lock();
	set->setSystemState(this, err, hwmode, mode, state);
	m_dataEngine->io.networkIOMutex.unlock();
}

/*!
	\brief show a temporary message on the status bar.
*/
void MainWindow::showStatusBarMessage(const QString &msg, int time) {

	statusBar()->showMessage(msg, time);
}

/*!
	\brief clear the temporary status message
*/
void MainWindow::clearStatusBarMessage() {

	statusBar()->clearMessage();
}

/*!
	\brief closes the application and shut down all engines.
*/
void MainWindow::closeMainWindow() {

	if (set->getMainPower())
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
 
void MainWindow::showRadioPopup(bool value) {

	Q_UNUSED (value)

	//m_radioPopupWidget->showPopupWidget(this, QCursor::pos());
}

/*!
	\brief updates entries from the ini-file.
*/
void MainWindow::updateFromSettings() {

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

	//foreach(QWidget *widget, widgetList) {
	foreach(QDockWidget *dockWidget, dockWidgetList) {

		//int off = widgetList.indexOf(widget);
		int off = dockWidgetList.indexOf(dockWidget);

		if (dockWidget->isVisible()) {
				
			dockWidget->hide();
			mainBtnList.at(off)->setBtnState(AeroButton::OFF);
			mainBtnList.at(off)->update();
		}
		else if (on == off) {

			button->setBtnState(AeroButton::ON);
			button->update();
			dockWidgetList.at(on)->show();
		}
	}
}

/*!
	\brief implements the Start/Stop functionality of the Start/Stop button.
*/
void MainWindow::startButtonClickedEvent() {

	if (startBtn->btnState() == AeroButton::OFF) {

		startBtn->setBtnState(AeroButton::ON);

		QColor col = QColor(180, 0, 0);
		startBtn->setColorOn(col);

		col = QColor(250, 0, 0);
		startBtn->setHighlight(col);
		startBtn->setText("Stop");
		set->setMainPower(this, true);
	}
	else if (startBtn->btnState() == AeroButton::ON) {

		startBtn->setBtnState(AeroButton::OFF);

		QColor col = QColor(0x91, 0xeb, 0xff);
		startBtn->setHighlight(col);
		startBtn->setText("Start");
		set->saveSettings();
		set->setMainPower(this, false);

		if (muteBtn->btnState() == AeroButton::ON) {

			muteBtn->setBtnState(AeroButton::OFF);
			muteBtn->update();

			m_volumeSlider->setEnabled(true);
			for (int i = 0; i < set->getNumberOfReceivers(); i++)
				set->setMainVolume(this, i, rxVolumeList.at(i));
		}
	}
}

/*!
	\brief implements the Show/Hide functionality of the wide band data widget.
*/
void MainWindow::wideBandBtnClickedEvent() {

	if (wideBandBtn->btnState() == AeroButton::OFF) {

		wideBandBtn->setBtnState(AeroButton::ON);
		set->setWidebandStatus(this, true);
		widebandDock->show();
		//showMessage("[server]: wide band data on.");
	}
	else if (wideBandBtn->btnState() == AeroButton::ON) {

		wideBandBtn->setBtnState(AeroButton::OFF);
		set->setWidebandStatus(this, false);
		widebandDock->hide();
		//showMessage("[server]: wide band data off.");
	}
}

void MainWindow::widebandVisibilityChanged(bool value) {

	if (value)
		wideBandBtn->setBtnState(AeroButton::ON);
	else
		wideBandBtn->setBtnState(AeroButton::OFF);

	wideBandBtn->update();
}

void MainWindow::alexBtnClickedEvent() {

	if (alexBtn->btnState() == AeroButton::OFF) {

		alexBtn->setBtnState(AeroButton::ON);
		alexBtn->setText("Alex Auto");

		//m_alexConfiguration[0].value = false;
		m_alexConfig &= 0xFFFE;

		set->setAlexToManual(this, false);
	}
	else if (alexBtn->btnState() == AeroButton::ON) {

		alexBtn->setBtnState(AeroButton::OFF);
		alexBtn->setText("Alex Man");

		//m_alexConfiguration[0].value = true;
		m_alexConfig |= 0x01;

		set->setAlexToManual(this, true);
	}
	//set->setAlexConfiguration(this, m_alexConfiguration);
}

void MainWindow::alexStateChanged(HamBand band, const QList<int> &states) {

        Q_UNUSED(band)

	m_currentHamBand = set->getCurrentHamBand(0);
	m_alexStates[m_currentHamBand] = states.at(m_currentHamBand);
	m_alexAttnState = 0x03 & (m_alexStates[m_currentHamBand] >> 7);

	setAttenuatorButton();
}

void MainWindow::alexConfigurationChanged(quint16 conf) {

	if (conf & 0x01) {

		alexBtn->setBtnState(AeroButton::OFF);
		alexBtn->setText("Alex Man");
	}
	else {

		alexBtn->setBtnState(AeroButton::ON);
		alexBtn->setText("Alex Auto");
	}

	alexBtn->update();
}

void MainWindow::alexPresenceChanged(bool value) {

	if (value) {

		//if (m_alexConfiguration[0].value)
		if (m_alexConfig & 0x01)
			alexBtn->setText(tr("Alex Man"));
		else
			alexBtn->setText(tr("Alex Auto"));
	}
	else
		alexBtn->setText(tr("Alex"));

	alexBtn->setEnabled(value);

	alexAttn_0dBAction->setCheckable(value);
	alexAttn_10dBAction->setCheckable(value);
	alexAttn_20dBAction->setCheckable(value);
	alexAttn_30dBAction->setCheckable(value);

	alexBtn->update();
}

void MainWindow::addReceiver() {
}

/*!
	\brief tune to last frequency.
*/
void MainWindow::getLastFrequency() {
}

/*!
	\brief updates the main window title.
*/
void MainWindow::updateTitle() {

	setWindowTitle(
		QApplication::applicationName() + "  " + \
		QApplication::applicationVersion() + "         " + \
		set->getCallsign());
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
 
void MainWindow::setCurrentReceiver(QObject *sender, int rx) {

	Q_UNUSED(sender)

	MAIN_DEBUG << "setCurrentReceiver: " << rx;
	//set->setCurrentReceiver(this, rx);
	//m_dataEngine->io.currentReceiver = rx;
	m_volumeSlider->setValue((int)(set->getMainVolume(rx) * 100));
	m_agcGainSlider->setValue(set->getAGCMaximumGain_dB(rx));
}

/*!
	\brief enable the receiver buttons according to the actual number of available receivers.
*/
void MainWindow::setNumberOfReceivers(
		QObject *sender,			/*!<[in] the sender of the event. */
		int value					/*!<[in] the number of receivers. */
) {
	Q_UNUSED(sender)

	viewMenu->clear();
	for (int i = 0; i < value-1; i++) {
		
		viewMenu->addAction(rxDockWidgetList.at(i)->toggleViewAction());
		if (!rxDockWidgetList.at(i)->isVisible())
			rxDockWidgetList.at(i)->show();
	}

	for (int i = value-1; i < MAX_RECEIVERS-1; i++) {
		
		if (rxDockWidgetList.at(i)->isVisible())
			rxDockWidgetList.at(i)->hide();
	}
}

/*!
	\brief set the main volume.
*/
void MainWindow::setMainVolume(int value) {

	if (value < 0 ) value = 0;
	if (value > 100 ) value = 100;

	QString str = "%1 %";
	m_volLevelLabel->setText(str.arg(value, 2, 10, QLatin1Char(' ')));

	set->setMainVolume(this, set->getCurrentReceiver(), value / 100.0f);
}

/*!
	\brief mute Volume.
*/
void MainWindow::muteBtnClickedEvent() {

	int rcvr = set->getNumberOfReceivers();
	if (muteBtn->btnState() == AeroButton::OFF) {

		muteBtn->setBtnState(AeroButton::ON);
		m_volumeSlider->setEnabled(false);

		for (int i = 0; i < rcvr; i++) {

			rxVolumeList[i] = set->getMainVolume(i);
			set->setMainVolume(this, i, 0.0f);
		}
	}
	else if (muteBtn->btnState() == AeroButton::ON) {

		muteBtn->setBtnState(AeroButton::OFF);
		m_volumeSlider->setEnabled(true);

		for (int i = 0; i < rcvr; i++) {

			set->setMainVolume(this, i, rxVolumeList.at(i));
		}
	}
}

void MainWindow::setTxAllowed(QObject *sender, bool value) {

	Q_UNUSED(sender)

	if (!value) {

		moxBtn->setEnabled(false);
		tunBtn->setEnabled(false);
	}
	else if (set->getPenelopePresence() || set->getPennyLanePresence() || (m_hwInterface == QSDR::Hermes)) {

		moxBtn->setEnabled(true);
		tunBtn->setEnabled(true);
	}
}

void MainWindow::setAGCMode(QObject *sender, int rx, AGCMode mode, bool hang) {

	Q_UNUSED(sender)
	Q_UNUSED(hang)

	m_agcMode = mode;

	if(m_agcMode == (AGCMode) agcOFF) {

		int gain = (int) set->getAGCFixedGain_dB(rx);
		m_agcGainLabel->setText("AGC-F:");
		m_agcGainSlider->blockSignals(true);
		m_agcGainSlider->setValue(gain);
		m_agcGainSlider->blockSignals(false);

		QString str = " %1 dB";
		m_agcGainLevelLabel->setText(str.arg(gain, 2, 10, QLatin1Char(' ')));
	}
	else {

		int gain = (int) set->getAGCMaximumGain_dB(rx);
		m_agcGainLabel->setText("AGC-G:");
		m_agcGainSlider->blockSignals(true);
		m_agcGainSlider->setValue(gain);
		m_agcGainSlider->blockSignals(false);

		QString str = " %1 dB";
		m_agcGainLevelLabel->setText(str.arg(gain, 2, 10, QLatin1Char(' ')));
	}
}

void MainWindow::setAGCGain(int value) {

	QString str = " %1 dB";
	m_agcGainLevelLabel->setText(str.arg(value + 0, 2, 10, QLatin1Char(' ')));

	int rx = set->getCurrentReceiver();
	if (m_agcMode == (AGCMode) agcOFF)
		set->setAGCFixedGain_dB(this, rx, (qreal) value);
	else
		set->setAGCMaximumGain_dB(this, rx, (qreal) value);
}

void MainWindow::setAGCGain(QObject *sender, int rx, qreal value) {

	Q_UNUSED(sender)
	Q_UNUSED(rx)

	m_agcGainSlider->blockSignals(true);
	m_agcGainSlider->setValue((int) value);

	QString str = " %1 dB";
	m_agcGainLevelLabel->setText(str.arg((int) value + 0, 2, 10, QLatin1Char(' ')));

	m_agcGainSlider->blockSignals(false);
}

void MainWindow::setSDRMode(bool) {

	MAIN_DEBUG << "setSDRMode";
	setServerMode(QSDR::SDRMode);

	//if (m_oldSampleRate == 48000 || m_oldSampleRate == 96000 || m_oldSampleRate == 192000)
	//	set->setSampleRate(this, m_oldSampleRate);
	
	//showMessage("[server]: switched to SDR mode.");
}

/*!
	\brief set \a QSDR::_ServerMode to \a QSDR::ChirpWSPR.
*/
void MainWindow::setChirpWSPRMode(bool value) {

        Q_UNUSED(value)

	setServerMode(QSDR::ChirpWSPR);

	m_oldSampleRate = set->getSampleRate();
	setNumberOfReceivers(this, 1);
	set->setCurrentReceiver(this, 1);
	set->setSampleRate(this, 48000);
	//m_dataEngine->createChirpDataProcessor();

	MAIN_DEBUG << "switched to chirp decode mode";
}
 
void MainWindow::setAttenuator() {

	QAction *action = qobject_cast<QAction *>(sender());

	int mercuryPos = mercuryAttnActionList.indexOf(action);
	int alexPos = alexAttnActionList.indexOf(action);

	if (mercuryPos > -1) {

		foreach(QAction *act, mercuryAttnActionList) {

			act->setChecked(false);
		}
		mercuryAttnActionList.at(mercuryPos)->setChecked(true);

		switch (mercuryPos) {

			case 0:

				//attenuatorBtn->setText(tr("Attn 0 dB"));
				//attenuatorBtn->setBtnState(AeroButton::OFF);

				set->setMercuryAttenuator(this, 1);
				break;

			case 1:

				//attenuatorBtn->setText(tr("Attn -20 dB"));
				//attenuatorBtn->setBtnState(AeroButton::ON);

				set->setMercuryAttenuator(this, 0);
				break;
		}
	}

	if (alexPos > -1) {

		foreach(QAction *act, alexAttnActionList) {
			act->setChecked(false);
		}
		alexAttnActionList.at(alexPos)->setChecked(true);

		int state = 0;
		state &= 0x7F; // 0 0 1 1 1 1 1 1 1
		state |= alexPos << 7;

		set->setAlexState(this, state);
	}

	if (mercuryAttnActionList.at(0)->isChecked()) {

		if (alexAttnActionList.at(0)->isChecked()) {

			attenuatorBtn->setText(tr("Attn 0 dB"));
			attenuatorBtn->setBtnState(AeroButton::OFF);
		}
		else if (alexAttnActionList.at(1)->isChecked()) {

			attenuatorBtn->setText(tr("Attn -10 dB"));
			attenuatorBtn->setBtnState(AeroButton::ON);
		}
		else if (alexAttnActionList.at(2)->isChecked()) {

			attenuatorBtn->setText(tr("Attn -20 dB"));
			attenuatorBtn->setBtnState(AeroButton::ON);
		}
		else if (alexAttnActionList.at(3)->isChecked()) {

			attenuatorBtn->setText(tr("Attn -30 dB"));
			attenuatorBtn->setBtnState(AeroButton::ON);
		}
	}
	else if (mercuryAttnActionList.at(1)->isChecked()) {

		if (alexAttnActionList.at(0)->isChecked()) {

			attenuatorBtn->setText(tr("Attn -20 dB"));
		}
		else if (alexAttnActionList.at(1)->isChecked()) {

			attenuatorBtn->setText(tr("Attn -30 dB"));
		}
		else if (alexAttnActionList.at(2)->isChecked()) {

			attenuatorBtn->setText(tr("Attn -40 dB"));
		}
		else if (alexAttnActionList.at(3)->isChecked()) {

			attenuatorBtn->setText(tr("Attn -50 dB"));
		}
		attenuatorBtn->setBtnState(AeroButton::ON);
	}
}

void MainWindow::setAttenuatorButton() {

	foreach(QAction *act, mercuryAttnActionList) act->setChecked(false);
	foreach(QAction *act, alexAttnActionList) act->setChecked(false);

	if (m_mercuryAttnState == 1) {

		mercuryAttnActionList.at(0)->setChecked(true);

		switch (m_alexAttnState) {

			case 0:

				attenuatorBtn->setText(tr("Attn 0 dB"));
				attenuatorBtn->setBtnState(AeroButton::OFF);
				break;

			case 1:

				attenuatorBtn->setText(tr("Attn -10 dB"));
				attenuatorBtn->setBtnState(AeroButton::ON);
				break;

			case 2:

				attenuatorBtn->setText(tr("Attn -20 dB"));
				attenuatorBtn->setBtnState(AeroButton::ON);
				break;

			case 3:

				attenuatorBtn->setText(tr("Attn -30 dB"));
				attenuatorBtn->setBtnState(AeroButton::ON);
				break;
		}
	}
	else if (m_mercuryAttnState == 0) {

		mercuryAttnActionList.at(1)->setChecked(true);
		switch (m_alexAttnState) {

			case 0:

				attenuatorBtn->setText(tr("Attn -20 dB"));
				break;

			case 1:

				attenuatorBtn->setText(tr("Attn -30 dB"));
				break;

			case 2:

				attenuatorBtn->setText(tr("Attn -40 dB"));
				break;

			case 3:

				attenuatorBtn->setText(tr("Attn -50 dB"));
				break;
		}
		attenuatorBtn->setBtnState(AeroButton::ON);
	}

	if (m_alexAttnState > -1)
		alexAttnActionList.at(m_alexAttnState)->setChecked(true);
}

void MainWindow::mercuryAttenuatorChanged(QObject *sender, HamBand band, int value) {

	Q_UNUSED(sender)
    Q_UNUSED(value)
    Q_UNUSED(band)

	m_currentHamBand = set->getCurrentHamBand(0);
	m_mercuryAttn[m_currentHamBand] = value;
	m_mercuryAttnState = m_mercuryAttn.at(m_currentHamBand);

	setAttenuatorButton();
}

/*!
	\brief set \a QSDR::_ServerMode to \a mode.
*/
void MainWindow::setServerMode(
		QSDR::_ServerMode mode			/*!<[in] server mode. */
) {
	/*
	if (mode != QSDR::ChirpWSPR && chirpBtn->btnState() == AeroButton::ON)
		m_chirpWidget->hide();

	if (mode == QSDR::ChirpWSPR && wideBandBtn->btnState() == AeroButton::ON) {

		//set->setWideBandStatus(false);
		wideBandBtn->setBtnState(AeroButton::OFF);
		wideBandBtn->setEnabled(false);
	}

	chirpBtn->setEnabled(QSDR::ChirpWSPR == mode);
	*/

	setSystemState(QSDR::NoError, m_hwInterface, mode, m_dataEngineState);
}

/*!
	\brief get all network interfaces and IP addresses.
*/
void MainWindow::getNetworkInterfaces() {

	// find out which IP to connect to
	QString localIP = QHostAddress(QHostAddress::LocalHost).toString();

	// ip addresses from ini file
	QString serverIpAddress = set->getServerAddr();
	QString hpsdrDeviceLocalIpAddress = set->getHPSDRDeviceLocalAddr();

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
		//		set->m_ipAddressesList.append(entry.ip());
		//		set->m_networkInterfaces.append(ni);
		//
		//		set->addServerNetworkInterface(ni.humanReadableName(), entry.ip().toString());
		//		set->addHPSDRDeviceNIC(ni.humanReadableName(), entry.ip().toString());
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
				set->m_ipAddressesList.append(entry.ip());
				set->m_networkInterfaces.append(ni);
				
				set->addServerNetworkInterface(ni.humanReadableName(), entry.ip().toString());
				set->addHPSDRDeviceNIC(ni.humanReadableName(), entry.ip().toString());
			}
		}
	}

	set->setNumberOfNetworkInterfaces(nics.size());
	MAIN_DEBUG << "found " << nics.size() << " valid ip addresses.";

	//emit serverWidgetEvent();
	//emit metisWidgetEvent();

	int serverIdx = -1;
	int metisIdx = -1;

	if (ipAddresses.isEmpty()) {

		// if we did not find one, use IPv4 localhost
		set->setServerAddr(this, localIP);
		set->setHPSDRDeviceLocalAddr(this, localIP);
	}
	else {

		//int serverIdx = -1;
		//int metisIdx = -1;
		
		for (int i = 0; i < nics.size(); ++i) {
			if (broadcastAddresses.at(i) != QHostAddress::Null) {

				if (ipAddresses.at(i).toString() == serverIpAddress) {

					set->setServerWidgetNIC(i);
					serverIdx = i;
				}
				if (ipAddresses.at(i).toString() == hpsdrDeviceLocalIpAddress) {

					set->setHPSDRWidgetNIC(i);
					metisIdx = i;
				}
			}
		}
	}

	QString message;
	for (int i = 0; i < nics.size(); ++i) {
		if (broadcastAddresses.at(i) != QHostAddress::Null) {
				
			if (serverIdx < 0) {
				set->setServerWidgetNIC(i);
				set->setServerAddr(this, ipAddresses.at(i).toString());
			}
			else {

				/*message = "[server]: network interface set to: %1 (%2).";
				showMessage(
					message.arg(
						nics.at(serverIdx).humanReadableName(),
						ipAddresses.at(serverIdx).toString() ));*/
			}

			if (metisIdx < 0) {

				set->setHPSDRWidgetNIC(i);
				set->setHPSDRDeviceLocalAddr(this, ipAddresses.at(i).toString());

				MAIN_DEBUG 	<< "HPSDR network device interface set to: "
							<< nics.at(i).humanReadableName()
							<< "(" << ipAddresses.at(i).toString() << ")";
			}
			else {

				MAIN_DEBUG 	<< "HPSDR network device interface set to: "
							<< nics.at(metisIdx).humanReadableName()
							<< "(" << ipAddresses.at(metisIdx).toString() << ")";
			}
		}

		/*for (int i = 0; i < nics.size(); ++i) {
			if (serverIdx != i) {
				set->setServerWidgetNIC(i);
				break;
			}
		}

		for (int i = 0; i < nics.size(); ++i) {
			if (metisIdx != i) {
				set->setHPSDRWidgetNIC(i);
				break;
			}
		
		}*/
	}

	//emit serverWidgetEvent();
	//emit metisWidgetEvent();

	//set->setServerWidgetNIC(1);
	MAIN_DEBUG << "using ip address " << qPrintable(set->getServerAddr()) << " for the server.";
	MAIN_DEBUG << "using ip address " << qPrintable(set->getHPSDRDeviceLocalAddr()) << " for connecting to a HPSDR device.";

	
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

void MainWindow::showWarningDialog(const QString &str) {

	m_warningDialog->setWarningMessage(str);
	m_warningDialog->exec();
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
//void MainWindow::initialMessage() {
//
//	QString str = set->versionStr();
//	str.prepend("cuSDR ");
//	str.append(". \nOpenGL enabled HPSDR Front end \n(C) 2010-2012 Hermann von Hasseln, DL3HVH.\n");
//	m_msgBrowser->appendPlainText(str);
//	m_msgBrowser->appendPlainText("");
//}
 
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
	\brief mouse wheel event implementation.
*/
void MainWindow::wheelEvent(
		QWheelEvent *event			/*!<[in] event */
) {
	event->accept();
	QWidget::wheelEvent(event);
}
 
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
	//displayPanelToolBar->updateGeometry();
	m_oglDisplayPanel->update();
	QWidget::resizeEvent(event);
}
 
/*!
	\brief close event implementation.
*/
void MainWindow::closeEvent(
		QCloseEvent *event			/*!<[in] event */
) {
	if (set->getMainPower())
		startButtonClickedEvent();

	QSettings settings(QCoreApplication::applicationDirPath() +  "/" + m_windowsSettingsFilename, QSettings::IniFormat);
    settings.setValue("geometry", saveGeometry());
    settings.setValue("windowState", saveState());

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

	if (m_radioTabWidget) {
		
		disconnect(m_radioTabWidget, 0, 0, 0);
		delete m_radioTabWidget;
		m_radioTabWidget = NULL;
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

	if (m_displayTabWidget) {
		
		disconnect(m_displayTabWidget, 0, 0, 0);
		delete m_displayTabWidget;
		m_displayTabWidget = NULL;
	}

	/*if (m_hpsdrServer) {

		disconnect(m_hpsdrServer, 0, 0, 0);
		delete m_hpsdrServer;
		m_hpsdrServer = NULL;
	}*/
	QMainWindow::closeEvent(event);
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


//***************************************************************************
// NetworkIODialog class

NetworkIODialog::NetworkIODialog(QWidget *parent)
    :   QDialog(parent)
	,	set(Settings::instance())
{
	int btnWidth = 74;
	int btnHeight = 18;

	m_deviceCards = set->getMetisCardsList();

	setWindowModality(Qt::NonModal);
	setWindowOpacity(0.9);
	setStyleSheet(set->getDialogStyle());

	setMouseTracking(true);

	m_titleFont.setStyleStrategy(QFont::PreferAntialias);
	m_titleFont.setFixedPitch(true);
	m_titleFont.setPixelSize(13);
	m_titleFont.setFamily("Arial");
	m_titleFont.setBold(true);
	

	QVBoxLayout *dialogLayout = new QVBoxLayout(this);

	m_deviceComboBox = new QComboBox(this);
	m_deviceComboBox->setStyleSheet(set->getComboBoxStyle());
	m_deviceComboBox->setMinimumContentsLength(30);
	
	QScopedPointer<QHBoxLayout> titleLayout(new QHBoxLayout);
	QLabel *titleLabel = new QLabel(tr("found more than one device:"), this);
	titleLabel->setFont(m_titleFont);
	titleLabel->setStyleSheet(set->getLabelStyle());
	titleLayout->addWidget(titleLabel);
	dialogLayout->addLayout(titleLayout.data());
	titleLayout.take(); // ownership transferred to dialogLayout

	QScopedPointer<QHBoxLayout> metisDeviceLayout(new QHBoxLayout);
	QLabel *ipAddressLabel = new QLabel(tr("Device (IP Addr):"), this);
	ipAddressLabel->setStyleSheet(set->getLabelStyle());
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
	okBtn->setFixedSize(btnWidth, btnHeight);
	CHECKED_CONNECT(
		okBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(okBtnClicked()));

	AeroButton* cancelBtn = new AeroButton("Cancel", this);
	cancelBtn->setRoundness(10);
	cancelBtn->setFixedSize(btnWidth, btnHeight);
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
		
		set->setCurrentHPSDRDevice(m_deviceCards.at(m_deviceComboBox->currentIndex()));
		NETWORKDIALOG_DEBUG << "Network device at: " << m_deviceCards.at(m_deviceComboBox->currentIndex()).ip_address.toString() << " selected.";
		accept();
	}
	else
		NETWORKDIALOG_DEBUG << "HPSDR network device list length:" << m_deviceCards.length();
}

void NetworkIODialog::addDeviceComboBoxItem(QString str) {

	m_deviceComboBox->addItem(str);
	m_deviceCards = set->getMetisCardsList();
}

void NetworkIODialog::clearDeviceComboBoxItem() {

	m_deviceComboBox->clear();
}


//***************************************************************************
// WarningDialog class

WarningDialog::WarningDialog(QWidget *parent)
    :   QDialog(parent)
	,	set(Settings::instance())
	,	m_btnWidth(74)
	,	m_btnHeight(18)
{
	setWindowModality(Qt::NonModal);
	setWindowTitle("Warning");
	setWindowOpacity(0.9);
	setStyleSheet(set->getDialogStyle());

	setMouseTracking(true);

	m_titleFont.setStyleStrategy(QFont::PreferAntialias);
	m_titleFont.setFixedPitch(true);
	m_titleFont.setBold(true);
	m_titleFont.setPixelSize(13);
	m_titleFont.setFamily("Arial");
	m_titleFont.setBold(true);
	
	//m_warningIcon.QPixmap::fromImage(QImage(QLatin1String(":/img/warning.png")), Qt::ColorOnly);

	m_warningLabel = new QLabel("", this);
		
	okBtn = new AeroButton("Ok", this);
	okBtn->setRoundness(10);
	okBtn->setFixedSize(m_btnWidth, m_btnHeight);

	CHECKED_CONNECT(
		okBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(okBtnClicked()));
}

WarningDialog::~WarningDialog() {
}

void WarningDialog::paintEvent(QPaintEvent *) {

	QPainter p(this);
	p.setRenderHints(QPainter::SmoothPixmapTransform | QPainter::Antialiasing | QPainter::TextAntialiasing, true);

	//QRect titlebar_rect(0, 0, width(), height());

	/*QLinearGradient titlebarGrad(0, 0, 0, 1);
	titlebarGrad.setCoordinateMode(QGradient::ObjectBoundingMode);
	titlebarGrad.setSpread(QGradient::PadSpread);
	titlebarGrad.setColorAt(0, QColor(110, 110, 110));
	titlebarGrad.setColorAt(0.45, QColor(80, 80, 80));
	titlebarGrad.setColorAt(0.55, QColor(56, 56, 65));
	titlebarGrad.setColorAt(1, QColor(40, 40, 40));*/

	// draw background rect
	/*p.setPen(Qt::NoPen);
	p.setBrush(QBrush(titlebarGrad));
	p.drawRect(titlebar_rect);
	p.setPen(QColor(255, 255, 255, 140));
	p.drawLine(1, titlebar_rect.top(), width() - 2, titlebar_rect.top());
	p.setPen(QColor(255, 255, 255, 30));
	p.drawLine(1, titlebar_rect.bottom() - 2, width() - 2, titlebar_rect.bottom() - 2);
	p.setPen(QColor(0, 0, 0, 255));
	p.drawLine(0, titlebar_rect.bottom(), width(), titlebar_rect.bottom());*/

	QPixmap warningIcon = QPixmap::fromImage(QImage(QLatin1String(":/img/warning.png")), Qt::ColorOnly);
	if (!warningIcon.isNull()) p.drawPixmap(13, 5, 16, 16, warningIcon);
		
	// draw text
	p.setFont(m_titleFont);
	p.setPen(QColor(95, 95, 95, 255));

	// warning msg
	p.drawText(
		40, 6, 
		m_msgFontWidth, m_msgFontHeight, 
		Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, 
		m_message);

	p.setPen(QColor(235, 235, 235, 255));
	p.drawText(
		39, 5, 
		m_msgFontWidth, m_msgFontHeight, 
		Qt::TextSingleLine | Qt::TextDontClip | Qt::AlignVCenter | Qt::AlignLeft, 
		m_message);

	okBtn->move((width() - m_btnWidth)/2, 30);

	p.end();
}

void WarningDialog::setWarningMessage(const QString &msg) {

	m_message = msg;
	
	QFontMetrics tfm(m_titleFont);
	m_msgFontWidth = tfm.width(m_message);
	m_msgFontHeight = tfm.height();

	this->setFixedWidth(m_msgFontWidth + 60);
	this->setFixedHeight(60);
}

void WarningDialog::okBtnClicked() {

	accept();
}
