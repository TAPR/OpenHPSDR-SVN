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

//#include <QObject>
//#include <QThread>
//#include <QMetaType>
//#include <QtNetwork>
//#include <QHostAddress>
//#include <QMutexLocker>
//#include <QMutex>
//#include <QWaitCondition>
//#include <QVariant>
//#include <QElapsedTimer>
//#include <QFuture>
//#include <qtconcurrentrun.h>

#include "cusdr_settings.h"
#include "cusdr_dataIO.h"
#include "cusdr_receiver.h"
#include "cusdr_chirpProcessor.h"
#include "cusdr_audioReceiver.h"
#include "cusdr_discoverer.h"
#include "Util/qcircularbuffer.h"
#include "QtDSP/qtdsp_fft.h"
#include "QtDSP/qtdsp_filter.h"
#include "QtDSP/qtdsp_dualModeAverager.h"
#include "AudioEngine/cusdr_audio_engine.h"


#ifdef LOG_DATA_ENGINE
#   define DATA_ENGINE_DEBUG qDebug().nospace() << "DataEngine::\t"
#else
#   define DATA_ENGINE_DEBUG nullDebug()
#endif

#ifdef LOG_DATA_PROCESSOR
#   define DATA_PROCESSOR_DEBUG qDebug().nospace() << "DataProcessor::\t"
#else
#   define DATA_PROCESSOR_DEBUG nullDebug()
#endif

#ifdef LOG_WIDEBAND_PROCESSOR
#   define WIDEBAND_PROCESSOR_DEBUG qDebug().nospace() << "WidebandProcessor::\t"
#else
#   define WIDEBAND_PROCESSOR_DEBUG nullDebug()
#endif


class DataProcessor;
class AudioOutProcessor;
class WideBandDataProcessor;


Q_DECLARE_METATYPE (QAbstractSocket::SocketError)


// *********************************************************************
// data engine class

class DataEngine : public QObject {

	Q_OBJECT

public:
	DataEngine(QObject* parent = 0);
	~DataEngine();

	Settings*			set;
	THPSDRParameter		io;

    QList<Receiver *>	RX;
	QList<qreal>		chirpData;

	QUdpSocket*			sendSocket;
	
public slots:
	bool	initDataEngine();
	void	stop();

	// set Server parameter
	void	setRxPeerAddress(int rx, QHostAddress address);
	void	setRxClient(int rx, int client);
	void	setRx(int rx);
	void	setRxSocketState(int rx, const char* prop, QString);
	
	//void	setSendIQSignal(QObject *sender, int value);
	void	setRcveIQSignal(QObject *sender, int value);
	void	setAudioReceiver(QObject *sender, int rx);
	//void	setAudioInProcessorRunning(bool value);
	void	setIQPort(int rx, int port);
	void	setRxConnectedStatus(QObject* sender, int rx, bool value);
	void	setClientConnected(QObject* sender, int rx);
	void	setClientConnected(bool value);
	void	setClientDisconnected(int client);
	void	setFramesPerSecond(QObject *sender, int rx, int value);
	void	createChirpDataProcessor();

	// DSP processing
	void	processFileBuffer(const QList<qreal> data);
	
	// change HPSDR hardware settings
	void	setPenelopeVersion(QObject *sender, int version);
	void	setHwIOVersion(QObject *sender, int version);
	void	setNumberOfRx(QObject *sender, int value);
	void	setSampleRate(QObject *sender, int value);
	void	setMercuryAttenuator(QObject *sender, HamBand band, int value);
	void	setDither(QObject *sender, int value);
	void	setRandom(QObject *sender, int value);
	void	setTimeStamp(QObject *sender, bool value);
	void	set10MhzSource(QObject *sender, int source);
	void	set122_88MhzSource(QObject *sender, int source);
	void	setMicSource(QObject *sender, int source);
	void	setMercuryClass(QObject *sender, int value);
	void	setMercuryTiming(QObject* sender, int value);
	void	setHamBand(QObject *sender, int rx, bool byBtn, HamBand band);
	void	setFrequency(QObject* sender, int mode, int rx, long frequency);

	void	loadWavFile(const QString &fileName);
	void	suspend();
	void	startPlayback();
	void	showSettingsDialog();

private:
	void	setSystemState(
				QSDR::_Error err,
				QSDR::_HWInterfaceMode hwmode,
				QSDR::_ServerMode mode,
				QSDR::_DataEngineState state);

	void	initAudioEngine();
	void	setupConnections();
	void	connectDSPSlots();
	void	disconnectDSPSlots();
	void	createDiscoverer();
	void	createDataIO();
	void	createDataProcessor();
	void	createAudioOutProcessor();
	void	createWideBandDataProcessor();
	//void	createChirpDataProcessor();
	//void	createAudioReceiver(int rx);
	void	createAudioReceiver();

	bool	initReceivers(int rx);
	bool	start();
	bool	startDataEngineWithoutConnection();
	bool	findHPSDRDevices();
	bool	getFirmwareVersions();
	bool	checkFirmwareVersions();
	bool	startDiscoverer(QThread::Priority prio);
	bool	startDataIO(QThread::Priority prio);
	bool	startDataProcessor(QThread::Priority prio);
	void	startAudioOutProcessor(QThread::Priority prio);
	bool	startWideBandDataProcessor(QThread::Priority prio);
	bool	startChirpDataProcessor(QThread::Priority prio);

	void	stopDiscoverer();
	void	stopDataIO();
	void	stopDataProcessor();
	void	stopAudioOutProcessor();
	void	stopWideBandDataProcessor();
	void	stopChirpDataProcessor();
	void	setHPSDRConfig();

private:
	DataIO*					m_dataIO;
	DataProcessor*			m_dataProcessor;
	WideBandDataProcessor*	m_wbDataProcessor;
	QDSPEngine*				m_chirpDspEngine;
	AudioReceiver*			m_audioReceiver;
	AudioEngine*			m_audioEngine;
	AudioOutProcessor*		m_audioOutProcessor;
	ChirpProcessor*			m_chirpProcessor;
	Discoverer*				m_discoverer;
	
	QThreadEx*				m_discoveryThread;
	QThreadEx*				m_dataIOThread;
	QThreadEx*				m_dataProcThread;
	QThreadEx*				m_wbDataProcThread;
	QThreadEx*				m_chirpDataProcThread;
	QThreadEx*				m_AudioRcvrThread;
	QThreadEx*				m_audioInProcThread;
	QThreadEx*				m_audioOutProcThread;

	QList<QThreadEx* >		m_dspThreadList;

	QMutex					m_mutex;

	QString					m_message;
	QString					m_HPSDRDevice;

	QByteArray				m_commandDatagram;
	QByteArray				m_datagram;

	QSDR::_Error			m_error;
	QSDR::_ServerMode		m_serverMode;
	QSDR::_HWInterfaceMode	m_hwInterface;
	QSDR::_DataEngineState	m_dataEngineState;

	QCircularBuffer<int>	audioringbuffer;

	TMeterType				m_meterType;

	CPX		cpxIn;
	CPX		cpxOut;

	bool	m_restart;
	bool	m_networkDeviceRunning;
	bool	m_soundFileLoaded;
	bool	m_clientConnect;
	//bool	m_audioProcessorRunning;
	bool	m_chirpInititalized;
	bool	m_discoveryThreadRunning;
	bool	m_dataIOThreadRunning;
	bool	m_wbDataRcvrThreadRunning;
	bool	m_chirpDataProcThreadRunning;
	bool	m_dataProcThreadRunning;
	bool	m_audioRcvrThreadRunning;
	bool	m_audioInProcThreadRunning;
	bool	m_audioOutProcThreadRunning;
	bool	m_frequencyChange;
	bool	m_hamBandChanged;
	bool	m_chirpThreadStopped;
	bool	m_clientConnected;

	float	m_mainVolume;

	int		m_hpsdrDevices;
	int		m_fwCount;
	int		m_configure;
	int		m_timeout;
	int		m_txFrame;
	int		m_bytes;
	int		m_remainingTime;
	int		m_found;
	int		m_RxFrequencyChange;
	int		m_counter;
	
	int		m_forwardPower;
	int		m_maxSamples;
	int		m_offset;

	int		m_rxSamples;
	int		m_chirpSamples;

	int		m_leftSample;
	int		m_rightSample;
	int		m_micSample;

	int		m_spectrumSize;
	int		m_sendState;

	float	m_lsample;
	float	m_rsample;
	float	m_scale;
	float	m_sMeterValue;
	float	m_sMeterCalibrationOffset;
	float	m_micSample_float;
	float	m_spectrumBuffer[SAMPLE_BUFFER_SIZE];

	qint64		m_audioFileBufferPosition;
    qint64		m_audioFileBufferLength;
	QByteArray	m_audioFileBuffer;

	float	getFilterSizeCalibrationOffset();

private slots:
	void	systemStateChanged(
					QObject *sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);

	//void	setCurrentNetworkDevice(TNetworkDevicecard card);
	void	setHPSDRDeviceNumber(int value);
	void	rxListChanged(QList<Receiver *> rxList);
	void	searchHpsdrNetworkDevices();
	void	setCurrentReceiver(QObject* sender, int rx);
	
	void	setMercuryAttenuators(QObject *sender, QList<int> attn);
	void 	setAlexConfiguration(quint16 conf);
	void 	setAlexStates(HamBand band, const QList<int> &states);
	void	setPennyOCEnabled(bool value);
	void	setRxJ6Pins(const QList<int> &list);
	void	setTxJ6Pins(const QList<int> &list);

	void	setAudioFileFormat(QObject *sender, const QAudioFormat &format);
	void	setAudioFilePosition(QObject *sender, qint64 position);
	void	setAudioFileBuffer(QObject *sender, qint64 position, qint64 length, const QByteArray &buffer);

	void	setAudioFileBuffer(const QList<qreal> &buffer);
	
signals:
	void	error(QUdpSocket::SocketError error);
	void	masterSwitchEvent(QObject *sender, bool power);
	//void	messageEvent(QString message);
	void	penelopeVersionInfoEvent(QObject *sender, int version);
	void	hwIOVersionInfoEvent(QObject *sender, int version);
	void	sendIQEvent(QObject *sender, int sendIQ);
	void	rcveIQEvent(QObject *sender, int value);
	//void	iqDataReady(int rx);
	void	chirpDataReady(int samples);
	void	audioDataReady();
	void	clientConnectedEvent(int rx);
	void	audioRxEvent(int rx);
	void	outMultiplierEvent(int value);
	void	systemMessageEvent(const QString &str, int time);
	void	clearSystemMessageEvent();
	void	DataProcessorReadyEvent();
	void	audioSenderReadyEvent(bool value);
};


 
// *********************************************************************
// Data processor class

class DataProcessor : public QObject {

    Q_OBJECT

public:
	DataProcessor(
		DataEngine* de = 0, 
		QSDR::_ServerMode serverMode = QSDR::NoServerMode,
		QSDR::_HWInterfaceMode hwMode = QSDR::NoInterfaceMode);

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
	void	processInputBuffer(const QByteArray &buffer);
	void	processOutputBuffer(const CPX &buffer);
	void	decodeCCBytes(const QByteArray &buffer);
	void	encodeCCBytes();
	void	setOutputBuffer(int rx, const CPX &buffer);
	void	writeData();
	
private:
	DataEngine*		de;
	Settings*		set;
	QUdpSocket*		m_dataProcessorSocket;

	QSDR::_Error			m_error;
	QSDR::_ServerMode		m_serverMode;
	QSDR::_HWInterfaceMode	m_hwInterface;
	QSDR::_DataEngineState	m_dataEngineState;

	QHostAddress	m_deviceAddress;
	QMutex			m_mutex;
	QMutex			m_spectrumMutex;
	QByteArray		m_IQDatagram;
	QByteArray		m_outDatagram;
	QByteArray		m_deviceSendDataSignature;
	QString			m_message;

	QTime			m_SyncChangedTime;
	QTime			m_ADCChangedTime;

	bool			m_socketConnected;
	bool			m_setNetworkDeviceHeader;
	bool			m_chirpGateBit;
	bool			m_chirpBit;
	bool			m_chirpStart;

	int				m_leftSample;
	int				m_rightSample;
	int				m_micSample;
	int				m_bytes;
	int				m_maxSamples;
	int				m_rxSamples;
	int				m_chirpSamples;
	int				m_fwCount;
	int				m_idx;
	int				m_sendState;
	int				m_chirpStartSample;

	float			m_lsample;
	float			m_rsample;
	float			m_micSample_float;

	unsigned long	m_IQSequence;
	unsigned long	m_sequenceHi;
	unsigned short	m_offset;
	unsigned short	m_length;

	long			m_sendSequence;
	long			m_oldSendSequence;

	volatile bool	m_stopped;

	uchar	m_ibuffer[IO_BUFFER_SIZE * IO_BUFFERS];
	
signals:
	void	messageEvent(QString message);
	void	connectingEvent(QString addr, quint16 port);
	void	connectedEvent(QString addr, quint16 port);
	void	disconnectedEvent();
	void	serverVersionEvent(QString version);
};


// *********************************************************************
// Audio out processor class

class AudioOutProcessor : public QObject {

    Q_OBJECT

public:
	AudioOutProcessor(DataEngine* de = 0, QSDR::_ServerMode serverMode = QSDR::NoServerMode);
	~AudioOutProcessor();

public slots:
	void	stop();
	void	processData();
	void	processDeviceData();
	
private slots:
	
private:
	DataEngine*		m_dataEngine;
	
	QMutex			m_mutex;
	QByteArray		m_IQDatagram;
	QString			m_message;

	QSDR::_ServerMode		m_serverMode;

	/*int				m_bytes;
	unsigned long	m_IQSequence;
	unsigned long	m_sequenceHi;
	unsigned short	m_offset;
	unsigned short	m_length;*/
	volatile bool	m_stopped;

signals:
	//void	connectingEvent(QString addr, quint16 port);
	//void	connectedEvent(QString addr, quint16 port);
	//void	disconnectedEvent();
	//void	serverVersionEvent(QString version);
	////void	metisVersionEvent(QObject *sender, int version);
	//void	newData();
	//void	newIQData(int rx);
	//void	newAudioDataEvent(float *lBuf, float *rBuf);
};
 
// *********************************************************************
// Wide band data processor class

class WideBandDataProcessor : public QObject {

    Q_OBJECT

public:
	WideBandDataProcessor(THPSDRParameter *ioData = 0, QSDR::_ServerMode serverMode = QSDR::NoServerMode, int size = 0);
	~WideBandDataProcessor();

public slots:
	void	stop();
	void	processWideBandData();
	void	setWbSpectrumAveraging(QObject* sender, int rx, bool value);
	
private slots:
	//void	initDataProcessorSocket();
	//void	displayDataProcessorSocketError(QAbstractSocket::SocketError error);
	void	processWideBandInputBuffer(const QByteArray &buffer);
	
private:
	THPSDRParameter*	io;
	Settings*			set;

	QFFT*				wbFFT;
	DualModeAverager*	wbAverager;
	
	CPX					cpxWBIn;
	CPX					cpxWBOut;

	QMutex				m_mutex;
	QByteArray			m_WBDatagram;
	QString				m_message;

	QSDR::_ServerMode		m_serverMode;

	int				m_size;
	int				m_bytes;

	bool			m_wbSpectrumAveraging;
	volatile bool	m_stopped;

	unsigned char	m_ibuffer[IO_BUFFER_SIZE * IO_BUFFERS];

signals:
	void	messageEvent(QString message);
	void	wbSpectrumBufferChanged(const qVectorFloat &buffer);
};
 

#endif  // _CUSDR_DATA_ENGINE_H
