/**
* @file  cusdr_mainWidget.h
* @brief main window widget header file for cuSDR
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

#ifndef CUSDR_MAIN_H
#define CUSDR_MAIN_H

#include <QProcess>
//#include <QMainWindow>
//#include <QAction>
#include <QActionGroup>
//#include <QStyleOptionToolBar>
//#include <QWidget>
//#include <QListWidget>
//#include <QTimer>
//#include <QNetworkInterface>
//#include <QSlider>
//#include <QMessageBox>

#include "cusdr_settings.h"
#include "cusdr_fonts.h"
#include "Util/cusdr_buttons.h"
#include "DataEngine/cusdr_dataEngine.h"
#include "cusdr_hpsdrTabWidget.h"
#include "cusdr_radioTabWidget.h"
#include "cusdr_displayTabWidget.h"
#include "cusdr_serverWidget.h"
#include "cusdr_chirpWidget.h"
//#include "cusdr_alexTabWidget.h"
//#include "cusdr_extCtrlWidget.h"
//#include "cusdr_radioWidget.h"
#include "cusdr_radioPopupWidget.h"
#include "GL/cusdr_oglWidebandPanel.h"
#include "GL/cusdr_oglReceiverPanel.h"
#include "GL/cusdr_oglDisplayPanel.h"
//#include "cusdr_graphicOptionsWidget.h"
//#include "cusdr_server.h"


#ifdef LOG_MAIN
#   define MAIN_DEBUG qDebug().nospace() << "Main::\t"
#else
#   define MAIN_DEBUG nullDebug()
#endif

#ifdef LOG_NETWORKDIALOG
#   define NETWORKDIALOG_DEBUG qDebug().nospace() << "NetworkDialog::\t"
#else
#   define NETWORKDIALOG_DEBUG nullDebug()
#endif


class NetworkIODialog;
class WarningDialog;


class MainWindow : public QMainWindow {

    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

	void	setup();

public slots:
	void	update();
	void	masterSwitchChanged(QObject *sender, bool power);
	void	startButtonClickedEvent();
	void	widgetBtnClickedEvent();
	void	wideBandBtnClickedEvent();
	void	avgBtnClickedEvent();
	void	gridBtnClickedEvent();
	void	peakHoldBtnClickedEvent();
	void	alexBtnClickedEvent();
	void	muteBtnClickedEvent();
	void	resizeWidget();
	
	void	showWidgetEvent(QObject *sender);
	void	closeWidgetEvent(QObject *sender);

	void	loadWavFile(QObject *sender, const QString &fileName);
	void	suspendSignal(QObject *sender);
	void	startPlaybackSignal(QObject *sender);
	void	showSettingsDialogSignal(QObject *sender);

private:
	void	setSystemState(
				QSDR::_Error err,
				QSDR::_HWInterfaceMode hwmode,
				QSDR::_ServerMode mode,
				QSDR::_DataEngineState state);

#if defined(Q_OS_WIN32)
	void	getCPULoadThread(LPVOID lpParam);
#endif

	void	setupConnections();
	void	setupLayout();
	void	createModeMenu();
	void	createViewMenu();
	void	createAttenuatorMenu();
	void	createMainBtnGroup();
	void	createDisplayPanel();
	void	createStatusBar();
	void	initWidebandDisplay();
	void	initReceiverPanels(int rx);
	void	updateFromSettings();
	void	runFFTWWisdom();
	void	setAttenuatorButton();

private:
	Settings					*set;

	QSDR::_Error				m_error;
	QSDR::_ServerMode			m_serverMode;
	QSDR::_HWInterfaceMode		m_hwInterface;
	QSDR::_DataEngineState		m_dataEngineState;

	QMutex						m_mutex;

	QDir 						m_currentDir;
	QToolBar					*mainBtnToolBar;
	QToolBar					*displayPanelToolBar;
	
	QMainWindow					*centralwidget;

	QList<QGLReceiverPanel *>	rxWidgetList;
	QVector<float>				rxVolumeList;

	QDockWidget					*widebandDock;
	QDockWidget					*rx1Dock;
	QList<QDockWidget*>			dockWidgetList;
	QList<QDockWidget*>			rxDockWidgetList;
	
	QList<QHostAddress>			m_ipList;
	QList<QNetworkInterface>	m_niList;

	//QThreadEx					*cpuLoadThread;
	CFonts		*fonts;
	TFonts		m_fonts;

	QSize		m_oldSize;
	QPoint		m_oldPosition;

	QPixmap		m_originalPixmap;
	QPixmap		m_widgetMask;

	QTimer		*m_resizeTimer;

	QPoint		m_dragPosition;
	QPoint		m_pos;
    QRect		m_rect;
	QString		m_message;
	
	QGridLayout				*m_contentLayout;

	QLabel					*m_volumeLabel;
	QLabel					*m_volLevelLabel;
	QLabel					*m_agcGainLabel;
	QLabel					*m_agcGainLevelLabel;
	QLabel					*m_cpuLoadLabel;
	QLabel					*m_dateTimeLabel;
	QLabel					*m_statusBarMessage;

	QString					m_cpuLoadString;
	QString					m_dateTimeString;
	QString					m_statusBarMessageString;

	QWidget					*m_buttonWidget;
	QWidget					*m_secondButtonWidget;
	
	QSlider					*m_volumeSlider;
	QSlider					*m_agcGainSlider;
	AGCMode					m_agcMode;

	DataEngine				*m_dataEngine;
	//RadioWidget				*m_radioWidget;
	RadioPopupWidget		*m_radioPopupWidget;
	ServerWidget			*m_serverWidget;
	ChirpWidget				*m_chirpWidget;
	HPSDRTabWidget			*m_hpsdrTabWidget;
	RadioTabWidget			*m_radioTabWidget;
	DisplayTabWidget		*m_displayTabWidget;
	OGLDisplayPanel			*m_oglDisplayPanel;
	//CudaInfoWidget		*m_cudaInfoWidget;

	QGLWidebandPanel		*m_wbDisplay;

	NetworkIODialog			*m_netIODialog;
	WarningDialog			*m_warningDialog;

	HamBand					m_currentHamBand;

	quint16					m_alexConfig;
	QList<int>				m_alexStates;
	QList<int>				m_mercuryAttn;
	//QList<QCLDevice>		m_clDevices;
	
	AeroButton		*startBtn;
	AeroButton		*serverBtn;
	AeroButton		*hpsdrBtn;
	AeroButton		*modeBtn;
	AeroButton		*viewBtn;
	AeroButton		*chirpBtn;
	AeroButton		*openclBtn;
	AeroButton		*rxCtrlBtn;
	AeroButton		*wideBandBtn;
	AeroButton		*ctrlDisplayBtn;
	AeroButton		*displayBtn;
	AeroButton		*quitBtn;
	AeroButton		*nullBtn;
	
	AeroButton		*avgBtn;
	AeroButton		*peakHoldBtn;
	AeroButton		*gridBtn;
	AeroButton		*moxBtn;
	AeroButton		*tunBtn;
	AeroButton		*alexBtn;
	AeroButton		*lastFreqBtn;
	AeroButton		*attenuatorBtn;
	AeroButton		*muteBtn;
		
	QList<AeroButton *>	mainBtnList;

	QMenu			*modeMenu;
	QMenu			*viewMenu;
	QMenu			*attenuatorMenu;

	QActionGroup	*modeActionGroup;
	QAction			*noServerModeAction;
	QAction			*sdrModeAction;
    QAction			*chirpWSPRAction;

    QActionGroup	*mercuryAttnActionGroup;
    QActionGroup	*alexAttnActionGroup;
    QAction			*mercuryAttn_0dBAction;
    QAction			*mercuryAttn_20dBAction;
    QAction			*alexAttn_0dBAction;
    QAction			*alexAttn_10dBAction;
    QAction			*alexAttn_20dBAction;
    QAction			*alexAttn_30dBAction;

    QList<QAction *> mercuryAttnActionList;
    QList<QAction *> alexAttnActionList;

	bool		m_resizeFrame;
	bool		m_mousePressed;
	bool		m_quitHighBotton;
	bool		m_fullScreen;
	bool		m_cudaPresence;
	bool		m_mover;
	bool		m_msgBrowserVisible;

	double		m_alpha;

	int			m_deltaX_max;
    int			m_deltaY_max;
	int			m_resizePosition;
	int			m_numberOfReceivers;

	int			m_alexAttnState;
	int			m_mercuryAttnState;

	int			m_oldSampleRate;

	//qreal		m_agcMaxGain;

private slots:
	void systemStateChanged(
			QObject *sender, 
			QSDR::_Error err, 
			QSDR::_HWInterfaceMode hwmode, 
			QSDR::_ServerMode mode, 
			QSDR::_DataEngineState state);

	void ctrlDisplayBtnClickedEvent();
	void closeMainWindow();
	void maximizeMainWindow();
	void setMainWindowGeometry();
	void updateTitle();
	void updateStatusBar(short load);
	void setFullScreen();
	void getRegion();

	void setServerMode(QSDR::_ServerMode mode);
	//void setReceiver();
	void setTxAllowed(QObject *sender, bool value);
	void setCurrentReceiver(QObject *sender, int rx);
	void setNumberOfReceivers(QObject *sender, int value);
	//void setInternalDSPMode(bool value);
	void setSDRMode(bool);
    void setChirpWSPRMode(bool value);
	void setPeakHoldStatus(bool);
	//void setAlexPresence(bool value);
	//void setPennyPresence(bool value);
	void getNetworkInterfaces();
	void setMainVolume(int value);
	//void setHamBand(QObject *sender, int rx, bool byButton, HamBand band);
	void setAGCMode(QObject *sender, int rx, AGCMode mode, bool hang);
	void setAGCGain(int value);
	//void setAGCGain(QObject *sender, int rx, int value);
	void setAGCGain(QObject *sender, int rx, qreal value);
	void getLastFrequency();

	void alexPresenceChanged(bool value);
	void alexConfigurationChanged(quint16 conf);
	void alexStateChanged(HamBand band, const QList<int> &states);
	void setAttenuator();
	void mercuryAttenuatorChanged(QObject *sender, HamBand band, int value);

	void showStatusBarMessage(const QString &msg, int time);
	void clearStatusBarMessage();
	void showNetworkIODialog();
	void showWarningDialog(const QString &msg);

	void setSpectrumBuffer(int rx, const float* buffer);

	void addNetworkIOComboBoxEntry(QString str);
	void clearNetworkIOComboBoxEntry();

	void widebandVisibilityChanged(bool value);
	void showRadioPopup(bool value);

protected:
	void getSelectedFrame(QPoint p);
	//void paintEvent(QPaintEvent *event);
	void closeEvent(QCloseEvent *event);
	void keyPressEvent(QKeyEvent *event);
	void showEvent(QShowEvent *event);
    //void focusInEvent(QFocusEvent *event);
    //void focusOutEvent(QFocusEvent *event);
	//void moveEvent(QMoveEvent *event); 
	void resizeEvent(QResizeEvent *event);
	//void changeEvent(QEvent *event); 
	void mousePressEvent(QMouseEvent *event);
	//void mouseMoveEvent(QMouseEvent *event);
	//void mouseReleaseEvent(QMouseEvent *event);
	void wheelEvent(QWheelEvent *event);
	void enterEvent(QEvent *event);
	void leaveEvent(QEvent *event);

signals:
	void setAGCSliderValue(int value);
};

//***************************************************************************
// NetworkIODialog class

class NetworkIODialog : public QDialog {

    Q_OBJECT

public:
    NetworkIODialog(QWidget *parent = 0);
    ~NetworkIODialog();

public slots:
	void addDeviceComboBoxItem(QString str);
	void clearDeviceComboBoxItem();

private:
	Settings*		set;

	QFont			m_titleFont;
    QComboBox*      m_deviceComboBox;

	TNetworkDevicecard			m_deviceCard;
	QList<TNetworkDevicecard>	m_deviceCards;

private slots:
	void	okBtnClicked();
};


//***************************************************************************
// WarningDialog class

class WarningDialog : public QDialog {

    Q_OBJECT

public:
    WarningDialog(QWidget *parent = 0);
    ~WarningDialog();

public slots:
	void setWarningMessage(const QString &msg);
	
protected:
	void paintEvent(QPaintEvent *event);

private:
	Settings*		set;

	QFont			m_titleFont;
	//QPixmap			m_warningIcon;

	QLabel*			m_warningLabel;

	AeroButton*		okBtn;

	QString			m_message;

	int		m_btnWidth;
	int		m_btnHeight;
	int		m_msgFontWidth;
	int		m_msgFontHeight;
    
private slots:
	void	okBtnClicked();
};

#endif // CUSDR_MAIN_H
