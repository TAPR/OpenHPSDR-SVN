/**
* @file  cusdr_dataEngine.h
* @brief cuSDR data engine header file
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-02-02
*/

/*
 *   
 *   Copyright 2010 Hermann von Hasseln, DL3HVH
 *
 *	 using original C code by John Melton, G0ORX/N6LYT and Dave McMcQuate, WA8YWQ
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

#ifndef _CUSDR_DATA_ENGINE_H
#define _CUSDR_DATA_ENGINE_H


#include <QObject>
#include <QThread>
#include <QMetaType>
#include <QtNetwork>
#include <QHostAddress>
#include <QMutexLocker>
#include <QMutex>
#include <QWaitCondition>
#include <QVariant>
#include <QElapsedTimer>
//#include <QFuture>
//#include <qtconcurrentrun.h>

#include "cusdr_settings.h"
#include "cusdr_dataReceiver.h"
#include "cusdr_fft.h"
#include "cusdr_filter.h"
#include "cusdr_dspEngine.h"
#include "cusdr_receiver.h"
#include "cusdr_DttSP.h"
#include "cusdr_audio_engine.h"
#include "cusdr_chirpProcessor.h"
#include "cusdr_audioReceiver.h"
#include "cusdr_hpsdrIO.h"

//using namespace QtConcurrent;

class DataProcessor;
class WideBandDataProcessor;
class AudioProcessor;


Q_DECLARE_METATYPE (QAbstractSocket::SocketError)

//static QList<HPSDRReceiver *>	rxList;
//static QList<QDttSP *>			dttSPList;


// *********************************************************************
// data engine class

class DataEngine : public QObject {

	Q_OBJECT

public:
	DataEngine(QObject *parent = 0);
	~DataEngine();

	Settings*				settings;
	THPSDRParameter			io;

    QList<HPSDRReceiver *>	rxList;
	QList<QDttSP *>			dttSPList;
	QList<QDSPEngine *>		qtdspList;
	QList<qreal>			chirpData;

	bool 	audioProcessorRunning() { return m_audioProcessorRunning; }
	bool	clientConnected;
	bool	syncToggle;
	bool	adcToggle;
	
public slots:
	void	newMessage(QString msg);

	bool	startDataEngine();
	void	stopDataEngine();

	// set Server parameter
	void	setRxPeerAddress(int rx, QHostAddress address);
	void	setRxClient(int rx, int client);
	void	setRx(int rx);
	void	setRxSocketState(int rx, const char* prop, QString);
	
	//void	setSendIQSignal(QObject *sender, int value);
	void	setRcveIQSignal(QObject *sender, int value);
	void	setAudioReceiver(QObject *sender, int rx);
	void	setAudioProcessorRunning(bool value);
	void	setIQPort(int rx, int port);
	void	setRxConnectedStatus(QObject* sender, int rx, bool value);
	void	setClientConnected(QObject* sender, int rx);
	void	setClientConnected(bool value);
	//void	setClientNoConnected(QObject* sender, int client);
	void	setClientDisconnected(int client);
	void	setFramesPerSecond(QObject *sender, int value);
	void	createChirpDataProcessor();

	// DSP processing
	void	processInputBuffer(const QByteArray &buffer);
	void	processWideBandInputBuffer(const QByteArray &buffer);
	void	processFileBuffer(const QList<qreal> data);
	void	processOutputBuffer(float *left, float *right);
	void	processOutputBuffer(CPX* buffer);
	void	writeControlBytes();
	void	dttSPDspProcessing(int rx);
	//void	dttspReadyAt(int rx);

	// DttSP interface
	void	setDttspFrequency(QObject *sender, bool value, int rx, long frequency);
	void	setDttspRXFilter(QObject *sender, int rx, qreal low, qreal high);
	void	setDttspDspMode(QObject *sender, int rx, DSPMode mode);
	void	setDttspAgcMode(QObject *sender, int rx, AGCMode mode);
	void	setDttspAgcGain(QObject *sender, int rx, int value);

	// change HPSDR hardware settings
	void	setPenelopeVersion(QObject *sender, int version);
	void	setHwIOVersion(QObject *sender, int version);
	void	setNumberOfRx(QObject *sender, int value);
	void	setSampleRate(QObject *sender, int value);
	void	setPreamp(QObject *sender, int value);
	void	setDither(QObject *sender, int value);
	void	setRandom(QObject *sender, int value);
	void	setTimeStamp(QObject *sender, bool value);
	void	set10MhzSource(QObject *sender, int source);
	void	set122_88MhzSource(QObject *sender, int source);
	void	setMicSource(QObject *sender, int source);
	void	setMercuryClass(QObject *sender, int value);
	void	setMercuryTiming(QObject* sender, int value);
	void	setFrequency(QObject* sender, bool value, int rx, long frequency);
	void	setRXFilter(QObject *sender, int rx, qreal low, qreal high);
	void	setAgcMode(QObject *sender, int rx, AGCMode mode);
	void	setAgcGain(QObject *sender, int rx, int value);

	void	loadWavFile(const QString &fileName);
	void	suspend();
	void	startPlayback();
	void	showSettingsDialog();

	void	displayDiscoverySocketError(QAbstractSocket::SocketError error);

	//void	spectrumBufferEvent(const float *buffer);
	void	spectrumBufferEvent(int rx, const float *buffer);

private:	
	void	setupConnections();
	void	initReceivers();
	
	// network devices
	void	networkDeviceStartStop(char value);
	void	sendInitFramesToNetworkDevice(int rx);
	
	bool	startDataEngineWithoutConnection();
	bool	startDataEngineWithConnection();
	bool	startDttSPInterface(int rx);
	void	initDttSPInterface(int rx);
	void	initDttSPInterfaceS();
	void	initDSPEngine(int rx);

	void	connectDTTSP();
	void	connectQTDSP();
	void	disconnectDTTSP();
	void	disconnectQTDSP();

	void	createHpsdrIO();
	void	createDataReceiver();
	void	createDataProcessor();
	void	createWideBandDataProcessor();
	//void	createChirpDataProcessor();
	//void	createAudioReceiver(int rx);
	//void	createAudioProcessor(int rx);
	void	createAudioReceiver();
	void	createAudioProcessor();
	
	bool	startHpsdrIO(QThread::Priority prio);
	bool	startDataReceiver(QThread::Priority prio);
	bool	startDataProcessor(QThread::Priority prio);
	bool	startWideBandDataProcessor(QThread::Priority prio);
	bool	startChirpDataProcessor(QThread::Priority prio);
	bool	startAudioProcessor(QThread::Priority prio, QSDR::_ServerMode mode);

	void	stopHpsdrIO();
	void	stopDataReceiver();
	void	stopDataProcessor();
	void	stopAudioProcessor();
	void	stopWideBandDataProcessor();
	void	stopChirpDataProcessor();
	
	//uchar	setHPSDRConfig();
	void	setHPSDRConfig();
	//void	dumpIQDatagram(int r);
	//void	dumpIQDatagramCPX(int r);

private:
	DataReceiver*				m_dataReceiver;
	DataProcessor*				m_dataProcessor;
	WideBandDataProcessor*		m_wbDataProcessor;
	//QDSPEngine*					m_wbDspEngine;
	QDSPEngine*					m_chirpDspEngine;
	AudioReceiver*				m_audioReceiver;
	AudioProcessor*				m_audioProcessor;
	AudioEngine*				m_audioEngine;
	ChirpProcessor*				m_chirpProcessor;
	QHpsdrIO*					m_hpsdrIO;

	QFFT*						m_wbFFT;
	
	QThreadEx					*m_netIOThread;
	QThreadEx					*m_dataRcvrThread;
	QThreadEx					*m_dataProcThread;
	QThreadEx					*m_wbDataProcThread;
	QThreadEx					*m_chirpDataProcThread;
	QThreadEx					*m_AudioRcvrThread;
	QThreadEx					*m_audioProcThread;

	QMutex		m_mutex;

	QString		m_message;

	QByteArray	m_commandDatagram;
	QByteArray	m_datagram;

	QElapsedTimer	m_timer;

	QSDR::_ServerMode		m_serverMode;
	QSDR::_HWInterfaceMode	m_hwInterface;
	QSDR::_DataEngineState	m_dataEngineState;

	QQueue<QVector<float>>	m_specAv_queue;

	TMeterType		m_meterType;

	
	//bool	timer;
	//qint64	oldtime;
	//qint64	newtime;

	bool	m_restart;
	bool	m_networkDeviceRunning;
	bool	m_soundFileLoaded;
	bool	m_clientConnect;
	bool	m_audioProcessorRunning;
	bool	m_chirpInititalized;
	bool	m_netIOThreadRunning;
	bool	m_dataRcvrThreadRunning;
	bool	m_wbDataRcvrThreadRunning;
	bool	m_chirpDataProcThreadRunning;
	bool	m_dataProcThreadRunning;
	bool	m_audioRcvrThreadRunning;
	bool	m_audioProcThreadRunning;
	bool	m_frequencyChange;
	bool	m_wbSpectrumAveraging;
	bool	m_chirpThreadStopped;
	bool	m_chirpGateBit;
	bool	m_chirpBit;
	bool	m_chirpStart;
	int		m_chirpStartSample;

	float	m_mainVolume;

	int		m_hpsdrDevices;
	int		m_configure;
	int		m_timeout;
	int		m_txFrame;
	int		m_bytes;
	int		m_remainingTime;
	int		m_found;
	int		m_idx;
	int		m_RxFrequencyChange;
	
	int		m_forwardPower;
	int		m_maxSamples;
	int		m_offset;

	int		m_rxSamples;
	int		m_chirpSamples;

	int		m_leftSample;
	int		m_rightSample;
	int		m_micSample;

	int		m_spectrumSize;

	float	m_lsample;
	float	m_rsample;

	int		m_framesPerSecond;
	int		m_framesCnt;

	float	m_scale;
	float	m_sMeterValue;
	float	m_sMeterCalibrationOffset;
	float	m_micSample_float;
	float	m_spectrumBuffer[SAMPLE_BUFFER_SIZE];
	float	m_wbSpectrumBuffer[2*BUFFER_SIZE];
	float	m_tmpBuf[2*BUFFER_SIZE];
	float	m_avgBuf[2*BUFFER_SIZE];

	int		m_spectrumCounter;
	QList<int>	m_specCntList;
	int		m_specAveragingCnt;

	qint64		m_audioFileBufferPosition;
    qint64		m_audioFileBufferLength;
	QByteArray	m_audioFileBuffer;

	QVector<int>	m_rx;

	float	getFilterSizeCalibrationOffset();

private slots:
	void	setSystemState(
					QObject *sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);

	void	sendMessage(QString msg);
	void	sendReceiverMessage(QString msg);

	void	setHPSDRDeviceNumber(int value);
	void	rxListChanged(QList<HPSDRReceiver *> rxList);
	void	searchHpsdrNetworkDevices();
	void	setCurrentReceiver(int rx);
	void	setWbSpectrumAveraging(bool value);
	void	setWbSpectrumAveragingCnt(int value);

	void	dttspProcessing();
	void	qtdspProcessing();
	//void	dspProcessing(int rx);
	//void	dspProcessing();
	//void	dttspReadyAt(int rx);
	//void	dspProcessing(const HPSDRReceiver &rx);

	void	setDttSPMainVolume(QObject *sender, int rx, float value);
	void	setQtDSPMainVolume(QObject *sender, int rx, float value);

	void	setAudioFileFormat(QObject *sender, const QAudioFormat &format);
	void	setAudioFilePosition(QObject *sender, qint64 position);
	void	setAudioFileBuffer(QObject *sender, qint64 position, qint64 length, const QByteArray &buffer);

	void	setAudioFileBuffer(const QList<qreal> &buffer);

signals:
	void	error(QUdpSocket::SocketError error);
	void	masterSwitchEvent(QObject *sender, bool power);
	void	systemStateChanged(
					QObject *sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);
	
	void	messageEvent(QString message);
	void	penelopeVersionInfoEvent(QObject *sender, int version);
	void	hwIOVersionInfoEvent(QObject *sender, int version);
	void	sendIQEvent(QObject *sender, int sendIQ);
	void	rcveIQEvent(QObject *sender, int value);
	void	iqDataReady(int rx);
	void	chirpDataReady(int samples);
	void	audioDataReady();
	void	clientConnectedEvent(int rx);
	void	audioRxEvent(int rx);
	void	outMultiplierEvent(int value);
	//void	closeNetworkDeviceDataSocket();
	//void	stopMetisEvent();
	void	DataProcessorReadyEvent();
	void	audioSenderReadyEvent(bool value);

	void	spectrumBufferChanged(const float *buffer);
	void	wbSpectrumBufferChanged(const float *buffer);
};


 
// *********************************************************************
// Data processor class

class DataProcessor : public QObject {

    Q_OBJECT

public:
	DataProcessor(DataEngine* de = 0, QSDR::_ServerMode serverMode = QSDR::NoServerMode);
	~DataProcessor();

public slots:
	void	stop();
	void	processData();
	void	processDeviceData();
	void	externalDspProcessing(int rx);
	void	externalDspProcessingBig(int rx);
	
private slots:
	void	initDataProcessorSocket();
	void	displayDataProcessorSocketError(QAbstractSocket::SocketError error);
	
private:
	DataEngine*		m_dataEngine;
	QUdpSocket*		m_dataProcessorSocket;
	
	QMutex			m_mutex;
	QMutex			m_spectrumMutex;
	QByteArray		m_IQDatagram;
	QString			m_message;

	QSDR::_ServerMode		m_serverMode;

	bool			m_socketConnected;
	int				m_bytes;
	unsigned long	m_IQSequence;
	unsigned long	m_sequenceHi;
	unsigned short	m_offset;
	unsigned short	m_length;
	volatile bool	m_stopped;

	unsigned char	m_ibuffer[IO_BUFFER_SIZE * IO_BUFFERS];

signals:
	void	messageEvent(QString message);
	void	connectingEvent(QString addr, quint16 port);
	void	connectedEvent(QString addr, quint16 port);
	void	disconnectedEvent();
	void	serverVersionEvent(QString version);
	//void	metisVersionEvent(QObject *sender, int version);
	void	newData();
	void	newIQData(int rx);
	void	newAudioDataEvent(float *lBuf, float *rBuf);
};


 
// *********************************************************************
// Wide band data processor class

class WideBandDataProcessor : public QObject {

    Q_OBJECT

public:
	WideBandDataProcessor(DataEngine* de = 0, QSDR::_ServerMode serverMode = QSDR::NoServerMode);
	~WideBandDataProcessor();

public slots:
	void	stop();
	void	processWideBandData();
	
private slots:
	//void	initDataProcessorSocket();
	//void	displayDataProcessorSocketError(QAbstractSocket::SocketError error);
	
private:
	DataEngine*		m_dataEngine;
	
	QMutex			m_mutex;
	QByteArray		m_WBDatagram;
	QString			m_message;

	QSDR::_ServerMode		m_serverMode;

	int				m_bytes;
	volatile bool	m_stopped;

	unsigned char	m_ibuffer[IO_BUFFER_SIZE * IO_BUFFERS];

signals:
	void	messageEvent(QString message);
	/*void	connectingEvent(QString addr, quint16 port);
	void	connectedEvent(QString addr, quint16 port);
	void	disconnectedEvent();
	void	serverVersionEvent(QString version);
	void	metisVersionEvent(QObject *sender, int version);*/
	void	newWBData();
	//void	newIQData(int rx);
	//void	newAudioDataEvent(float *lBuf, float *rBuf);
};

 
// *********************************************************************
// Audio processor class
 
class AudioProcessor : public QObject {

    Q_OBJECT

public:
	AudioProcessor(DataEngine *de = 0);
	~AudioProcessor();

	int			id;

public slots:
	void		stop();
	void		clientConnected(int rx);
	void		audioReceiverChanged(int rx);
	void		initAudioProcessorSocket();
	void		processAudioData();
	void		deviceWriteBuffer();

private:
	DataEngine*		m_dataEngine;
	QUdpSocket*		m_audioProcessorSocket;
	QMutex			m_mutex;
	QByteArray		m_deviceSendDataSignature;
	QByteArray		m_outBuffer;
	QByteArray		m_outDatagram;

	bool			m_setNetworkDeviceHeader;
	long			m_sendSequence;
	long			m_oldSendSequence;

	volatile bool	m_stopped;

	float			m_left[BUFFER_SIZE];
	float			m_right[BUFFER_SIZE];

	int				m_client;
	int				m_audioRx;

private slots:
	void		displayAudioProcessorSocketError(QAbstractSocket::SocketError error);
	
signals:
	void 		messageEvent(QString message);
	void		rcveIQEvent(QObject *sender, int value);
	void		outputBufferEvent(unsigned char* outbuffer);
	void		newData();
	void		newAudioData();
};

#endif  // _CUSDR_DATA_ENGINE_H
