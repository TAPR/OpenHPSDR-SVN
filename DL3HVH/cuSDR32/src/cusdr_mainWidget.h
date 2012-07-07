/**
* @file  cusdr_main.h
* @brief main window header file for cuSDR
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

#include <QtGui>
#include <QWidget>
#include <QTimer>
#include <QNetworkInterface>
#include <QSlider>

#include "cusdr_settings.h"
#include "cusdr_buttons.h"
#include "cusdr_dataEngine.h"
#include "cusdr_hpsdrTabWidget.h"
#include "cusdr_serverWidget.h"
#include "cusdr_chirpWidget.h"
#include "cusdr_alexWidget.h"
#include "cusdr_pennyWidget.h"
#include "cusdr_radioWidget.h"
#include "cusdr_oglWidget.h"
#include "cusdr_oglDisplayPanel.h"
#include "cusdr_graphicOptionsWidget.h"
#include "cusdr_server.h"

class NetworkIODialog;

class MainWindow : public QWidget {

    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

	void		setup();

public slots:
	void		update();
	void		masterSwitchChanged(QObject *sender, bool power);
	void		startButtonClickedEvent();
	void		messagesBtnClickedEvent();
	void		widgetBtnClickedEvent();
	void		wideBandBtnClickedEvent();
	void		avgBtnClickedEvent();
	void		gridBtnClickedEvent();
	void		resizeWidget();
	
	void		showWidgetEvent(QObject *sender);
	void		closeWidgetEvent(QObject *sender);

	void		loadWavFile(QObject *sender, const QString &fileName);
	void		suspendSignal(QObject *sender);
	void		startPlaybackSignal(QObject *sender);
	void		showSettingsDialogSignal(QObject *sender);

private:
	void		setupConnections();
	void		setupLayout();
	void		createModeMenu();
	void		createMainBtnGroup();
	void		createConsoleWidget();
	void		createSecondBtnGroup();
	void		updateFromSettings();

private:
	Settings*	m_settings;

	QSDR::_ServerMode			m_serverMode;
	QSDR::_HWInterfaceMode		m_hwInterface;
	QSDR::_DataEngineState		m_dataEngineState;

	QList<QHostAddress>			m_ipList;
	QList<QNetworkInterface>	m_niList;

	QSize		m_oldSize;
	QPoint		m_oldPosition;
	
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

	int			m_oldSampleRate;

	QPixmap		m_originalPixmap;
	QPixmap		m_widgetMask;

	QTimer*		m_resizeTimer;

	QPoint		m_dragPosition;
	QPoint		m_pos;
    QRect		m_rect;
	QString		m_message;
	
	QGridLayout*		m_contentLayout;
	
	QFont				m_smallFont;
	QLabel*				m_volumeLabel;
	QLabel*				m_volLevelLabel;
	QLabel*				m_rxLabel;
	QLabel*				m_agcGainLabel;
	QLabel*				m_agcGainLevelLabel;
	
	QWidget*			m_buttonWidget;
	QWidget*			m_secondButtonWidget;
	QPlainTextEdit*		m_msgBrowser;
	QSlider*			m_volumeSlider;
	QSlider*			m_agcGainSlider;

	DataEngine*			m_dataEngine;
	HPSDRServer*		m_hpsdrServer;

	RadioWidget*		m_radioWidget;
	ServerWidget*		m_serverWidget;
	ChirpWidget*		m_chirpWidget;
	//HPSDRWidget*		m_hpsdrWidget;
	HPSDRTabWidget*		m_hpsdrTabWidget;
	AlexWidget*			m_alexWidget;
	PennyWidget*		m_pennyWidget;
	GraphicOptionsWidget*	m_graphicOptionsWidget;
	OGLDisplayPanel*	m_oglDisplayPanel;
	OGLWidget*			m_oglWidget;
	//CudaInfoWidget*		m_cudaInfoWidget;

	NetworkIODialog*	m_netIODialog;

	//QList<QCLDevice>		m_clDevices;
	
	AeroButton			*startBtn;
	AeroButton			*serverBtn;
	AeroButton			*hpsdrBtn;
	AeroButton			*alexBtn;
	AeroButton			*pennyBtn;
	AeroButton			*serverLogBtn;
	AeroButton			*modeBtn;
	AeroButton			*chirpBtn;
	AeroButton			*openclBtn;
	//AeroButton			*cudaGraphBtn;
	AeroButton			*rxCtrlBtn;
	AeroButton			*wideBandBtn;
	AeroButton			*ctrlDisplayBtn;
	AeroButton			*displayBtn;
	AeroButton			*quitBtn;
	
	AeroButton			*avgBtn;
	AeroButton			*gridBtn;
	AeroButton			*lastFreqBtn;

	AeroButton			*rx1Btn;
	AeroButton			*rx2Btn;
	AeroButton			*rx3Btn;
	AeroButton			*rx4Btn;

	AeroButton			*nullBtn;
	
	QList<AeroButton *>	mainBtnList;
	QList<AeroButton *>	rxBtnList;
	QList<QWidget *>	widgetList;

	QMenu*		m_modeMenu;
	QAction*	m_internalDSPModeAction;
	QAction*	m_dttspModeAction;
	QAction*	m_qtdspModeAction;
    QAction*	m_externalDSPModeAction;
    QAction*	m_chirpWSPRAction;

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
	void initialMessage();
	void showMessage(QString message);
	void setFullScreen();
	void getRegion();

	void setServerMode(QSDR::_ServerMode mode);
	void setReceiver();
	void setReceiver(int rx);
	void setNumberOfReceivers(QObject *sender, int value);
	void setInternalDSPMode();
	void setDttSPMode();
	void setQtDSPMode();
    void setExternalDSPMode();
    void setChirpWSPRMode();
	void setCudaDSPMode();
	//void setAlexPresence(bool value);
	//void setPennyPresence(bool value);
	void getNetworkInterfaces();
	void setMainVolume(int value);
	//void newMainVolumeLevelEvent(QObject* sender, float value);
	void setAGCGain(int value);
	void getLastFrequency();

	void showNetworkIODialog();
	void addNetworkIOComboBoxEntry(QString str);
	void clearNetworkIOComboBoxEntry();

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
	//void	changeNoOfReceivers(QObject *sender, int value);
	//void	setNetworkInterfaceEvent(QString niName, QString ipAddress);
	void	messageEvent(QString message);
	//void	serverWidgetEvent();
	//void	metisWidgetEvent();
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
	Settings*		m_settings;

	QFont			m_titleFont;
    QComboBox*      m_deviceComboBox;

	TNetworkDevicecard			m_deviceCard;
	QList<TNetworkDevicecard>	m_deviceCards;

private slots:
	void	okBtnClicked();
};


#endif // CUSDR_MAIN_H
