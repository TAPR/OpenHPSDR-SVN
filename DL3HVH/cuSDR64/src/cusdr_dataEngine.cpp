/**
* @file  cusdr_dataEngine.cpp
* @brief cuSDR data engine class
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-02-02
*/

/*
 *   
 *   Copyright 2010 Hermann von Hasseln, DL3HVH
 *
 *	 using original C code by John Melton, G0ORX/N6LYT and Dave McQuate, WA8YWQ
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
#define LOG_DATA_ENGINE
#define LOG_DATA_PROCESSOR
#define LOG_FUTUREWATCHER

#include "cusdr_dataEngine.h"

/*!
	\class DataEngine
	\brief The DataEngine class implements the main SDR functionality.
*/
/*!
	\brief Implements interfaces to the HPSDR hardware and various Server and DSP functionality.
	- set up HW interfaces to Metis or other resp.
	- initializes Metis.
	- set up parameters for HPSDR hardware.
	- implements the data receiver thread.
	- implements the data processor thread.
	- implements the wide band data processor thread.
	- implements the audio receiver thread.
	- implements the audio processor thread.
	- implements the interface to the Chirp WSPR decoding functionality.
*/
DataEngine::DataEngine(QObject *parent)
	: QObject(parent)
	, settings(Settings::instance())
	, m_restart(false)
	, m_hwInterface(settings->getHWInterface())
	, m_serverMode(settings->getCurrentServerMode())
	, m_dataEngineState(QSDR::DataEngineDown)
	, m_spectrumSize(settings->getSpectrumSize())
	, m_hpsdrDevices(0)
	, m_metisFW(0)
	, m_mercuryFW(0)
	, m_hermesFW(0)
	, m_networkDeviceRunning(false)
	, m_soundFileLoaded(false)
	, m_audioProcessorRunning(false)
	, m_chirpInititalized(false)
	, m_netIOThreadRunning(false)
	, m_dataRcvrThreadRunning(false)
	, m_dataProcThreadRunning(false)
	, m_chirpDataProcThreadRunning(false)
	, m_audioRcvrThreadRunning(false)
	, m_audioProcThreadRunning(false)
	, m_RxFrequencyChange(0)
	, m_frequencyChange(false)
	, m_audioEngine(new AudioEngine())
	, m_configure(9)
	, m_rxSamples(0)
	, m_chirpSamples(0)
	, m_forwardPower(0)
	, m_idx(IO_HEADER_SIZE)
	, m_remainingTime(0)
	, m_timeout(5000)
	, m_framesPerSecond(settings->getFramesPerSecond())
	, m_spectrumCounter(0)
	, m_wbSpectrumAveraging(settings->getSpectrumAveraging())
	, m_specAveragingCnt(settings->getSpectrumAveragingCnt())
	, m_chirpThreadStopped(true)
	, m_chirpBit(false)
	, m_chirpGateBit(true)
	, m_chirpStart(false)
	, m_chirpStartSample(0)
	, clientConnected(false)
	, syncToggle(true)
	, adcToggle(false)
	, m_sMeterCalibrationOffset(0.0f)//(35.0f)
	, m_meterType(SIGNAL_STRENGTH)
	//, m_meterType(AVG_SIGNAL_STRENGTH)
{
	qRegisterMetaType<QAbstractSocket::SocketError>();

	if (m_specAveragingCnt > 0)
		m_scale = 1.0f / m_specAveragingCnt;
	else
		m_scale = 1.0f;

	currentRx = 0;
	m_hpsdrIO = 0;
	m_dataReceiver = 0;
	m_dataProcessor = 0;
	m_wbDataProcessor = 0;
	m_audioReceiver = 0;
	m_audioProcessor = 0;
	m_chirpProcessor = 0;

	settings->setMercuryVersion(0);
	settings->setPenelopeVersion(0);
	settings->setMetisVersion(0);
	settings->setHermesVersion(0);
	
	setupConnections();
}

DataEngine::~DataEngine() {
}

void DataEngine::setupConnections() {

	CHECKED_CONNECT(
		settings, 
		SIGNAL(systemStateChanged(
					QObject *, 
					QSDR::_Error, 
					QSDR::_HWInterfaceMode, 
					QSDR::_ServerMode, 
					QSDR::_DataEngineState)), 
		this, 
		SLOT(setSystemState(
					QObject *, 
					QSDR::_Error, 
					QSDR::_HWInterfaceMode, 
					QSDR::_ServerMode, 
					QSDR::_DataEngineState)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(rxListChanged(QList<HPSDRReceiver *>)),
		this,
		SLOT(rxListChanged(QList<HPSDRReceiver *>)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(numberOfRXChanged(QObject *, int)), 
		this, 
		SLOT(setNumberOfRx(QObject *, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(currentReceiverChanged(int)), 
		this, 
		SLOT(setCurrentReceiver(int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(sampleRateChanged(QObject *, int)), 
		this, 
		SLOT(setSampleRate(QObject *, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(preampChanged(QObject *, int)), 
		this, 
		SLOT(setPreamp(QObject *, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(ditherChanged(QObject *, int)), 
		this, 
		SLOT(setDither(QObject *, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(randomChanged(QObject *, int)), 
		this, 
		SLOT(setRandom(QObject *, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(src10MhzChanged(QObject *, int)), 
		this, 
		SLOT(set10MhzSource(QObject *, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(src122_88MhzChanged(QObject *, int)), 
		this, 
		SLOT(set122_88MhzSource(QObject *, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(micSourceChanged(QObject *, int)), 
		this, 
		SLOT(setMicSource(QObject *, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(classChanged(QObject *, int)), 
		this, 
		SLOT(setMercuryClass(QObject *, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(timingChanged(QObject *, int)), 
		this, 
		SLOT(setMercuryTiming(QObject *, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(clientDisconnectedEvent(int)), 
		this, 
		SLOT(setClientDisconnected(int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(clientNoConnectedChanged(QObject*, int)), 
		this, 
		SLOT(setClientConnected(QObject*, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(rxConnectedStatusChanged(QObject*, int, bool)), 
		this, 
		SLOT(setRxConnectedStatus(QObject*, int, bool)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(audioRxChanged(QObject*, int)), 
		this, 
		SLOT(setAudioReceiver(QObject*, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(framesPerSecondChanged(QObject*, int)), 
		this, 
		SLOT(setFramesPerSecond(QObject*, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(searchMetisSignal()), 
		this, 
		SLOT(searchHpsdrNetworkDevices()));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(spectrumAveragingChanged(bool)), 
		this, 
		SLOT(setWbSpectrumAveraging(bool)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(networkDeviceNumberChanged(int)), 
		this, 
		SLOT(setHPSDRDeviceNumber(int)));

	CHECKED_CONNECT(
		m_audioEngine,
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(newMessage(QString)));

	CHECKED_CONNECT(
		m_audioEngine,
		SIGNAL(formatChanged(QObject *, const QAudioFormat )), 
		settings, 
		SLOT(setAudioFormat(QObject *, const QAudioFormat )));

	CHECKED_CONNECT(
		m_audioEngine,
		SIGNAL(formatChanged(QObject *, const QAudioFormat )), 
		this, 
		SLOT(setAudioFileFormat(QObject *, const QAudioFormat )));

	CHECKED_CONNECT(
		m_audioEngine, 
		SIGNAL(playPositionChanged(QObject *, qint64)),
		settings,
		SLOT(setAudioPosition(QObject *, qint64)));

	CHECKED_CONNECT(
		m_audioEngine, 
		SIGNAL(playPositionChanged(QObject *, qint64)),
		this,
		SLOT(setAudioFilePosition(QObject *, qint64)));

	CHECKED_CONNECT(
		m_audioEngine, 
		SIGNAL(bufferChanged(QObject *, qint64, qint64, const QByteArray)),
		settings,
		SLOT(setAudioBuffer(QObject *, qint64, qint64, const QByteArray)));

	CHECKED_CONNECT(
		m_audioEngine, 
		SIGNAL(bufferChanged(QObject *, qint64, qint64, const QByteArray)),
		this,
		SLOT(setAudioFileBuffer(QObject *, qint64, qint64, const QByteArray)));

	CHECKED_CONNECT(
		m_audioEngine, 
		SIGNAL(audiofileBufferChanged(const QList<qreal>)),
		this,
		SLOT(setAudioFileBuffer(const QList<qreal>)));

	/*CHECKED_CONNECT(
		m_metisDialog,
		SIGNAL(),
		this,
		SLOT());*/
}
 
//********************************************************
// start/stop data engine
bool DataEngine::startDataEngineWithoutConnection() {

	sendMessage("no HPSDR-HW interface.");

	if (io.inputBuffer.length() > 0) {

		initReceivers();
		if (!m_dataReceiver)	createDataReceiver();
		if (!m_dataProcessor)	createDataProcessor();
		
		// data receiver thread
		if (!startDataReceiver(QThread::NormalPriority)) {

			DATA_ENGINE_DEBUG << "data receiver thread could not be started.";
			return false;
		}

		switch (m_serverMode) {

			case QSDR::ExternalDSP:
			//case QSDR::InternalDSP:
			case QSDR::DttSP:
			case QSDR::QtDSP:
			case QSDR::ChirpWSPR:
				return false;

			case QSDR::ChirpWSPRFile:			

				if (!m_chirpInititalized) createChirpDataProcessor();				

				m_chirpProcessor->generateLocalChirp();
				
				if (!startChirpDataProcessor(QThread::NormalPriority)) {
					
					DATA_ENGINE_DEBUG << "data processor thread could not be started.";
					return false;
				}
		
				io.cpxIn  = mallocCPX(2*BUFFER_SIZE);
				io.cpxOut = mallocCPX(2*BUFFER_SIZE);
		
				m_chirpDspEngine = new QDSPEngine(this, 0, 2*BUFFER_SIZE);
				//m_fft = new QFFT(this, 2*BUFFER_SIZE);

				memset(io.cpxIn, 0, 2*BUFFER_SIZE * sizeof(CPX));
				memset(io.cpxOut, 0, 2*BUFFER_SIZE * sizeof(CPX));

				rxList[0]->setConnectedStatus(true);
				settings->setRxList(rxList);

				m_rxSamples = 0;
				m_chirpSamples = 0;

				break;
		}

		// IQ data processing thread
		if (!startDataProcessor(QThread::NormalPriority)) {

			DATA_ENGINE_DEBUG << "data processor thread could not be started.";
			return false;
		}

		settings->setSystemState(
							this, 
							QSDR::NoError, 
							m_hwInterface, 
							m_serverMode, 
							QSDR::DataEngineUp);
		return true;
	}
	else {

		sendMessage("no data available - data file loaded?.");
		return false;
	}
}

bool DataEngine::findHPSDRDevices() {

	if (!m_hpsdrIO) createHpsdrIO();

	// HPSDR network IO thread
	if (!startHpsdrIO(QThread::NormalPriority)) {

		DATA_ENGINE_DEBUG << "HPSDR network IO thread could not be started.";
		return false;
	}

	io.networkIOMutex.lock();
	io.devicefound.wait(&io.networkIOMutex);

	m_hpsdrDevices = settings->getHpsdrNetworkDevices();
	if (m_hpsdrDevices == 0) {

		io.networkIOMutex.unlock();
		stopHpsdrIO();
		sendMessage("no device found. HPSDR hardware powered? Network connection established?");
		settings->setSystemState(
						this, 
						QSDR::HwIOError, 
						m_hwInterface, 
						m_serverMode,
						QSDR::DataEngineDown);
	}
	else {

		if (m_hpsdrDevices > 1)
			settings->showNetworkIODialog();

		QList<TNetworkDevicecard> metisList = settings->getMetisCardsList();
		m_message = "found %1 network device(s).";
		sendMessage(m_message.arg(metisList.count()));
				
		for (int i = 0; i < metisList.count(); i++) {					
			m_message = "Device %1 @ %2 [%3].";
			sendMessage(m_message.arg(i).arg(metisList.at(i).ip_address.toString()).arg((char *) &metisList.at(i).mac_address));
		}

		io.metisIPAddress = settings->getCurrentMetisCard().ip_address;
		DATA_ENGINE_DEBUG << "using HPSDR network device at " << qPrintable(io.metisIPAddress.toString());
		m_message = "using device @ %2.";
		sendMessage(m_message.arg(io.metisIPAddress.toString()));
		Sleep(100);

		// stop the discovery thread
		io.networkIOMutex.unlock();
		stopHpsdrIO();

		if (getFirmwareVersions()) return true;
	}

	return false;
}

bool DataEngine::getFirmwareVersions() {

	// as it says..
	initReceivers();

	if (!m_dataReceiver) createDataReceiver();
		
	if (!m_dataProcessor) createDataProcessor();

	switch (m_serverMode) {

		case QSDR::DttSP:
			
			for (int i = 0; i < settings->getNumberOfReceivers(); i++)
				initDttSPInterface(i);
				
			// turn time stamping off
			setTimeStamp(this, false);

			for (int i = 0; i < io.receivers; i++) {

				rxList[i]->setConnectedStatus(true);
				setDttspFrequency(this, true, i, settings->getFrequencies().at(i));
			}

			settings->setRxList(rxList);
			connectDTTSP();
			//dttspProcessing = new QFutureWatcher<int>(this);

			break;

		case QSDR::QtDSP:

			for (int i = 0; i < settings->getNumberOfReceivers(); i++)
				initDSPEngine(i);

			// turn time stamping off
			setTimeStamp(this, false);

			for (int i = 0; i < io.receivers; i++) {

				rxList[i]->setConnectedStatus(true);
				setFrequency(this, true, i, settings->getFrequencies().at(i));
			}

			settings->setRxList(rxList);
			connectQTDSP();

			break;

		default:

			sendMessage("no valid server mode.");
						
			settings->setSystemState(
							this, 
							QSDR::ServerModeError, 
							m_hwInterface, 
							m_serverMode, 
							QSDR::DataEngineDown);

			return false;
	}	// end switch (m_serverMode)

	// data receiver thread
	if (!startDataReceiver(QThread::HighPriority)) {//  ::NormalPriority)) {

		DATA_ENGINE_DEBUG << "data receiver thread could not be started.";
		return false;
	}

	// IQ data processing thread
	if (!startDataProcessor(QThread::NormalPriority)) {

		DATA_ENGINE_DEBUG << "data processor thread could not be started.";
		return false;
	}

	setSampleRate(this, settings->getSampleRate());

	// pre-conditioning
	for (int i = 0; i < io.receivers; i++)
		sendInitFramesToNetworkDevice(i);
				
	if ((m_serverMode == QSDR::DttSP || m_serverMode == QSDR::QtDSP))
		networkDeviceStartStop(0x01); // 0x01 for starting Metis without wide band data
		
	m_networkDeviceRunning = true;

	settings->setSystemState(
					this, 
					QSDR::NoError, 
					m_hwInterface, 
					m_serverMode, 
					QSDR::DataEngineUp);

	// just give it a little time to get the firmware versions
	Sleep(100);

	m_metisFW = settings->getMetisVersion();
	m_mercuryFW = settings->getMercuryVersion();
	m_hermesFW = settings->getHermesVersion();

	return true;
}

bool DataEngine::start() {

	// as it says..
	initReceivers();

	if (!m_dataReceiver) createDataReceiver();
		
	if (!m_dataProcessor) createDataProcessor();
		
	if ((m_serverMode == QSDR::DttSP || m_serverMode == QSDR::QtDSP) && !m_wbDataProcessor)
		createWideBandDataProcessor();
		
	if (!m_audioProcessor) 
		createAudioProcessor();

	if ((m_serverMode == QSDR::ChirpWSPR) && !m_chirpProcessor)
		createChirpDataProcessor();
		
		switch (m_serverMode) {

			case QSDR::ExternalDSP:
		
				//CHECKED_CONNECT(
				//	settings,
				//	SIGNAL(frequencyChanged(QObject*, bool, int, long)),
				//	this,
				//	SLOT(setFrequency(QObject*, bool, int, long)));

				//if (!m_audioProcessorRunning) {

				//	//if (!m_audioProcessor)	createAudioProcessor();
				//	if (!m_audioReceiver)	createAudioReceiver();

				//	m_audioProcThread->start();
				//	if (m_audioProcThread->isRunning()) {
				//				
				//		m_audioProcThreadRunning = true;
				//		DATA_ENGINE_DEBUG << "Audio processor process started.";
				//	}
				//	else {

				//		m_audioProcThreadRunning = false;
				//		settings->setSystemState(
				//						this, 
				//						QSDR::AudioThreadError, 
				//						m_hwInterface, 
				//						m_serverMode,
				//						QSDR::DataEngineDown);
				//		return false;
				//	}
				//			
				//	io.audio_rx = 0;
				//	io.clientList.append(0);

				//	m_audioProcessorRunning = true;
				//	settings->setSystemState(
				//						this, 
				//						QSDR::NoError, 
				//						m_hwInterface, 
				//						m_serverMode,
				//						QSDR::DataEngineUp);
				//}
				return false;

			//case QSDR::InternalDSP:
			case QSDR::DttSP:
			
				for (int i = 0; i < settings->getNumberOfReceivers(); i++)
					initDttSPInterface(i);
				
				// turn time stamping off
				setTimeStamp(this, false);

				for (int i = 0; i < io.receivers; i++) {

					rxList[i]->setConnectedStatus(true);
					setDttspFrequency(this, true, i, settings->getFrequencies().at(i));
				}

				settings->setRxList(rxList);
				connectDTTSP();
				//dttspProcessing = new QFutureWatcher<int>(this);

				break;

			case QSDR::QtDSP:

				for (int i = 0; i < settings->getNumberOfReceivers(); i++)
					initDSPEngine(i);

				// turn time stamping off
				setTimeStamp(this, false);

				for (int i = 0; i < io.receivers; i++) {

					rxList[i]->setConnectedStatus(true);
					setFrequency(this, true, i, settings->getFrequencies().at(i));
				}

				settings->setRxList(rxList);
				connectQTDSP();

				break;

			case QSDR::ChirpWSPR:
			//case QSDR::ChirpWSPRFile:

				initDttSPInterface(0);

				// turn time stamping on
				setTimeStamp(this, true);

				if (!startChirpDataProcessor(QThread::NormalPriority)) {
					
					DATA_ENGINE_DEBUG << "data processor thread could not be started.";
					return false;
				}

				rxList[0]->setConnectedStatus(true);
				settings->setRxList(rxList);

				setDttspFrequency(this, true, 0, settings->getFrequencies().at(0));

				CHECKED_CONNECT(
							settings,
							SIGNAL(frequencyChanged(QObject *, bool, int, long)),
							this,
							SLOT(setDttspFrequency(QObject *, bool, int, long)));

				break;

			default:

				sendMessage("no valid server mode.");
						
				settings->setSystemState(
								this, 
								QSDR::ServerModeError, 
								m_hwInterface, 
								m_serverMode, 
								QSDR::DataEngineDown);

				return false;
		}	// end switch (m_serverMode)

		// Wide band data processing thread
		if (m_serverMode != QSDR::ChirpWSPR && !startWideBandDataProcessor(QThread::NormalPriority)) {

			DATA_ENGINE_DEBUG << "wide band data processor thread could not be started.";
			return false;
		}

		// data receiver thread
		if (!startDataReceiver(QThread::HighPriority)) {//  ::NormalPriority)) {

			DATA_ENGINE_DEBUG << "data receiver thread could not be started.";
			return false;
		}

		// IQ data processing thread
		if (!startDataProcessor(QThread::NormalPriority)) {

			DATA_ENGINE_DEBUG << "data processor thread could not be started.";
			return false;
		}

		// audio processing thread
		if (!startAudioProcessor(QThread::NormalPriority, m_serverMode)) {

			DATA_ENGINE_DEBUG << "audio processor thread could not be started.";
			return false;
		}

		// just give them a little time..
		Sleep(100);

		setSampleRate(this, settings->getSampleRate());

		// pre-conditioning
		for (int i = 0; i < io.receivers; i++)
			sendInitFramesToNetworkDevice(i);
				
		//if (m_serverMode == QSDR::InternalDSP && settings->getWideBandData())
		if ((m_serverMode == QSDR::DttSP || m_serverMode == QSDR::QtDSP) && settings->getWideBandData())
			networkDeviceStartStop(0x03); // 0x03 for starting Metis with wide band data
		else
			networkDeviceStartStop(0x01); // 0x01 for starting Metis without wide band data
		
		m_networkDeviceRunning = true;

		settings->setSystemState(
			this, 
			QSDR::NoError, 
			m_hwInterface, 
			m_serverMode, 
			QSDR::DataEngineUp);

		return true;
	//} // if (m_hpsdrDevices == 0)
}

void DataEngine::stop() {

	if (m_dataEngineState == QSDR::DataEngineUp) {
		
		switch (m_hwInterface) {

			case QSDR::Metis:
			case QSDR::Hermes:
				
				// turn time stamping off
				setTimeStamp(this, false);
				Sleep(10);

				networkDeviceStartStop(0);
				m_networkDeviceRunning = false;

				sendMessage("Metis stopped.");

				stopAudioProcessor();
				stopDataReceiver();
				stopDataProcessor();
				stopChirpDataProcessor();
				if (m_wbDataProcessor)
					stopWideBandDataProcessor();
				
				//Sleep(100);
				settings->clearMetisCardList();
				//m_metisDialog->clear();
				//m_networkIO->clear();
				DATA_ENGINE_DEBUG << "device cards list cleared.";
				break;

			case QSDR::NoInterfaceMode:

				stopDataReceiver();
				
				DATA_ENGINE_DEBUG << "data queue count: " << io.data_queue.count();
				DATA_ENGINE_DEBUG << "chirp queue count: " << io.chirp_queue.count();

				stopDataProcessor();
				stopChirpDataProcessor();
		}

		while (!io.au_queue.isEmpty())
			io.au_queue.dequeue();

		QCoreApplication::processEvents();

		// save receiver settings and clear receiver list
		foreach (HPSDRReceiver *rx, rxList) {
		

			rx->setConnectedStatus(false);
			rx->setProperty("socketState", "RECEIVER_DETACHED");
		}

		rxList.clear();
		settings->setRxList(rxList);
		DATA_ENGINE_DEBUG << "receiver list cleared.";

		if (m_serverMode == QSDR::DttSP || m_serverMode == QSDR::ChirpWSPR) { // clear DttSP list
			
			disconnectDTTSP();
			foreach (QDttSP *dttsp, dttSPList) {

				dttsp->setDttSPStatus(false);
				dttsp->dttspReleaseUpdate();
				dttsp->dttspDestroySDR();
			}
			dttSPList.clear();
			DATA_ENGINE_DEBUG << "DttSP list deleted.";
		}
		else if (m_serverMode == QSDR::QtDSP) { // clear QtDSP list

			disconnectQTDSP();
			foreach (QDSPEngine *qtdsp, qtdspList) {

				qtdsp->setQtDSPStatus(false);
			}
			qtdspList.clear();
			DATA_ENGINE_DEBUG << "QtDSP list deleted.";
		}

		settings->setSystemState(
			this, 
			QSDR::NoError, 
			m_hwInterface, 
			m_serverMode, 
			QSDR::DataEngineDown);
	}

	m_rxSamples = 0;
	m_chirpSamples = 0;
	m_restart = true;
	m_found = 0;
	m_hpsdrDevices = 0;

	settings->setMercuryVersion(0);
	settings->setPenelopeVersion(0);
	settings->setMetisVersion(0);
	settings->setHermesVersion(0);

	settings->resetWidebandSpectrumBuffer();

	/*disconnect(
		SIGNAL(iqDataReady(int)),
		this,
		SLOT(dttSPDspProcessing(int)));*/

	disconnect(
		settings, 
		SIGNAL(frequencyChanged(QObject*, bool, int, long)), 
		this, 
		SLOT(setFrequency(QObject*, bool, int, long)));

	DATA_ENGINE_DEBUG << "shut down done.";
}

bool DataEngine::initDataEngine() {

	if (m_hwInterface == QSDR::NoInterfaceMode) {
		
		return startDataEngineWithoutConnection();
	}
	else {
			
		if (findHPSDRDevices()) {
		
			if (m_mercuryFW > 0 || m_hermesFW > 0) {

				stop();

				DATA_ENGINE_DEBUG << "got firmware versions:";
				DATA_ENGINE_DEBUG << "	Metis firmware:  " << m_metisFW;
				DATA_ENGINE_DEBUG << "	Mercury firmware:  " << m_mercuryFW;
				DATA_ENGINE_DEBUG << "	Hermes firmware: " << m_hermesFW;
				DATA_ENGINE_DEBUG << "stopping and restarting data engine.";

				return start();
			}
			else
				DATA_ENGINE_DEBUG << "did not get firmware versions!";
		}
	}
	return false;
}


 
//**********************************************************
// init receivers, dsp engines

void DataEngine::initReceivers() {

	for (int i = 0; i < settings->getNumberOfReceivers(); i++) {
	
		HPSDRReceiver *rcvr = new HPSDRReceiver(this);
		rcvr->setID(i);
		rcvr->setReceiver(i);
		rcvr->setConnectedStatus(false);
		rcvr->setProperty("socketState", "RECEIVER_DETACHED");
		//rcvr->in_cpx  = mallocCPX(BUFFER_SIZE);
		//rcvr->out_cpx = mallocCPX(BUFFER_SIZE);
		rcvr->inBuf  = mallocCPX(BUFFER_SIZE);
		rcvr->outBuf = mallocCPX(BUFFER_SIZE);

		memset(rcvr->inBuf, 0, BUFFER_SIZE * sizeof(CPX));
		memset(rcvr->outBuf, 0, BUFFER_SIZE * sizeof(CPX));

		TReceiver rxData = settings->getReceiverDataList().at(i);
		rcvr->setAudioVolume(this, i, rxData.audioVolume);
		rcvr->setHamBand(this, i, rxData.hamBand);
		rcvr->setDspMode(this, i, rxData.dspMode);
		rcvr->setAGCMode(this, i, rxData.agcMode);
		rcvr->setAGCGain(this, i, rxData.acgGain);
		rcvr->setFilterFrequencies(this, i, rxData.filterLo, rxData.filterHi);
		rcvr->setLastFrequencyList(rxData.lastFrequencyList);

		CHECKED_CONNECT(
			rcvr, 
			SIGNAL(messageEvent(QString)), 
			this, 
			SLOT(sendReceiverMessage(QString)));

		rxList.append(rcvr);

		m_specCntList << 0;
    }

	settings->setRxList(rxList);

	m_txFrame = 0;
	
	//iodata.current_receiver = m_settings->getCurrentReceivers();
	io.currentReceiver = 0;
	io.receivers = settings->getNumberOfReceivers();
	for (int i = 0; i < io.receivers; ++i) m_rx.append(i);

	io.timing = 0;
	m_configure = io.receivers + 1;

	// init cc Rc and Tx parameters
	io.ccRx.mercuryFirmwareVersion = 0;
	io.ccRx.penelopeFirmwareVersion = 0;
	io.ccRx.networkDeviceFirmwareVersion = 0;

	io.ccRx.ptt    = false;
	io.ccRx.dash   = false;
	io.ccRx.dot    = false;
	io.ccRx.lt2208 = false;
	io.ccRx.ain1   = 0;
	io.ccRx.ain2   = 0;
	io.ccRx.ain3   = 0;
	io.ccRx.ain4   = 0;
	io.ccRx.ain5   = 0;
	io.ccRx.ain6   = 0;
	io.ccRx.hermesI01 = false;
	io.ccRx.hermesI02 = false;
	io.ccRx.hermesI03 = false;
	io.ccRx.hermesI04 = false;
	io.ccRx.mercury1_LT2208 = false;
	io.ccRx.mercury2_LT2208 = false;
	io.ccRx.mercury3_LT2208 = false;
	io.ccRx.mercury4_LT2208 = false;

	io.alexForwardVolts = 0.0;
	io.alexReverseVolts = 0.0;
	io.alexForwardPower = 0.0;
	io.alexReversePower = 0.0;
	io.penelopeForwardVolts = 0.0;
	io.penelopeForwardPower = 0.0;
	io.ain3Volts = 0.0;
	io.ain4Volts = 0.0;
	io.supplyVolts = 0.0f;


	//*****************************
	// C&C bytes
	for (int i = 0; i < 5; i++) {

		io.control_in[i] = 0x00;
		io.control_out[i] = 0x00;
	}

	io.control_out[0] = MOX_DISABLED;

	// set C1, Bits 1,0
	setSampleRate(this, settings->getSampleRate());

	// set C1 Bits 7,..,2
	setHPSDRConfig();
	
	
	//****
	//io.control_out[1] = CONFIG_MERCURY | MERCURY_122_88MHZ_SOURCE | MERCURY_10MHZ_SOURCE | MIC_SOURCE_PENELOPE | SPEED_192KHZ;
	//io.control_out[1] = 0x40 | 0x10 | 0x08 | 0x80 | 0x02;
	io.control_out[2] = MODE_OTHERS;
	io.control_out[3] = ALEX_ATTENUATION_0DB | LT2208_GAIN_ON | LT2208_DITHER_ON | LT2208_RANDOM_ON;
	io.control_out[4] = DUPLEX | ((io.receivers - 1) << 3);
	
	io.control_out[4] &= 0xC7;
	io.control_out[4] |= (io.receivers - 1) << 3;

	io.preamp = settings->getMercuryPreamp();
	io.control_out[3] = io.control_out[3] & 0xFB;
	io.control_out[3] = io.control_out[3] | (io.preamp << 2);

	io.dither = settings->getMercuryDither();
	io.control_out[3] = io.control_out[3] & 0xF7;
	io.control_out[3] = io.control_out[3] | (io.dither << 3);

	io.random = settings->getMercuryRandom();
	io.control_out[3] = io.control_out[3] & 0xEF;
	io.control_out[3] = io.control_out[3] | (io.random << 4);

	io.rxClass = settings->getRxClass();
	io.control_out[2] = io.control_out[2] & 0xFE;
	io.control_out[2] = io.control_out[2] | io.rxClass;
		
	io.mic_gain = 0.26F;
	io.rx_freq_change = -1;

	io.clients = 0;
	io.sendIQ_toggle = true;
	io.rcveIQ_toggle = false;
}

void DataEngine::setHPSDRConfig() {

	uchar byte = 0x00;

	if ((settings->getPenelopePresence()   || settings->getPennyLanePresence()) && 
		((settings->get10MHzSource() == 0) || settings->getExcaliburPresence())) {

		byte = MIC_SOURCE_PENELOPE | MERCURY_PRESENT | PENELOPE_PRESENT | MERCURY_122_88MHZ_SOURCE | ATLAS_10MHZ_SOURCE;
	}
	else if ((settings->getPenelopePresence() || settings->getPennyLanePresence()) && (settings->get10MHzSource() == 1)) {
		
		byte = MIC_SOURCE_PENELOPE | MERCURY_PRESENT | PENELOPE_PRESENT | MERCURY_122_88MHZ_SOURCE | PENELOPE_10MHZ_SOURCE;
	}
	else if ((settings->getPenelopePresence() || settings->getPennyLanePresence()) && (settings->get10MHzSource() == 2)) {
		
		byte = MIC_SOURCE_PENELOPE | MERCURY_PRESENT | PENELOPE_PRESENT | MERCURY_122_88MHZ_SOURCE | MERCURY_10MHZ_SOURCE;
	}
	else if ((settings->get10MHzSource() == 0) || settings->getExcaliburPresence()) {
		
		byte = MERCURY_PRESENT | MERCURY_122_88MHZ_SOURCE | ATLAS_10MHZ_SOURCE;
	}
	else {
		
		byte = MERCURY_PRESENT | MERCURY_122_88MHZ_SOURCE | MERCURY_10MHZ_SOURCE;
	}

	//return byte;
	io.control_out[1] &= 0x03; // 0x03 = 00000011
	io.control_out[1] |= byte;
}

//void DataEngine::initReceivers() {
//
//	for (int i = 0; i < settings->getNumberOfReceivers(); i++) {
//	
//		HPSDRReceiver *rcvr = new HPSDRReceiver(this);
//		rcvr->setID(i);
//		rcvr->setReceiver(i);
//		rcvr->setConnectedStatus(false);
//		rcvr->setProperty("socketState", "RECEIVER_DETACHED");
//		//rcvr->in_cpx  = mallocCPX(BUFFER_SIZE);
//		//rcvr->out_cpx = mallocCPX(BUFFER_SIZE);
//		rcvr->inBuf  = mallocCPX(BUFFER_SIZE);
//		rcvr->outBuf = mallocCPX(BUFFER_SIZE);
//
//		memset(rcvr->inBuf, 0, BUFFER_SIZE * sizeof(CPX));
//		memset(rcvr->outBuf, 0, BUFFER_SIZE * sizeof(CPX));
//
//		TReceiver rxData = settings->getReceiverDataList().at(i);
//		rcvr->setAudioVolume(this, i, rxData.audioVolume);
//		rcvr->setHamBand(this, i, rxData.hamBand);
//		rcvr->setDspMode(this, i, rxData.dspMode);
//		rcvr->setAGCMode(this, i, rxData.agcMode);
//		rcvr->setAGCGain(this, i, rxData.acgGain);
//		rcvr->setFilterFrequencies(this, i, rxData.filterLo, rxData.filterHi);
//		rcvr->setLastFrequencyList(rxData.lastFrequencyList);
//
//		CHECKED_CONNECT(
//			rcvr, 
//			SIGNAL(messageEvent(QString)), 
//			this, 
//			SLOT(sendReceiverMessage(QString)));
//
//		rxList.append(rcvr);
//
//		m_specCntList << 0;
//    }
//
//	settings->setRxList(rxList);
//
//	m_txFrame = 0;
//	
//	//iodata.current_receiver = m_settings->getCurrentReceivers();
//	io.currentReceiver = 0;
//	io.receivers = settings->getNumberOfReceivers();
//	for (int i = 0; i < io.receivers; ++i) m_rx.append(i);
//
//	io.timing = 0;
//	m_configure = io.receivers + 1;
//
//	// init cc Rc and Tx parameters
//	io.ccRx.mercuryFirmwareVersion = 0;
//	io.ccRx.penelopeFirmwareVersion = 0;
//	io.ccRx.networkDeviceFirmwareVersion = 0;
//
//	io.ccRx.ptt    = false;
//	io.ccRx.dash   = false;
//	io.ccRx.dot    = false;
//	io.ccRx.lt2208 = false;
//	io.ccRx.ain1   = 0;
//	io.ccRx.ain2   = 0;
//	io.ccRx.ain3   = 0;
//	io.ccRx.ain4   = 0;
//	io.ccRx.ain5   = 0;
//	io.ccRx.ain6   = 0;
//	io.ccRx.hermesI01 = false;
//	io.ccRx.hermesI02 = false;
//	io.ccRx.hermesI03 = false;
//	io.ccRx.hermesI04 = false;
//	io.ccRx.mercury1_LT2208 = false;
//	io.ccRx.mercury2_LT2208 = false;
//	io.ccRx.mercury3_LT2208 = false;
//	io.ccRx.mercury4_LT2208 = false;
//
//	io.alexForwardVolts = 0.0;
//	io.alexReverseVolts = 0.0;
//	io.alexForwardPower = 0.0;
//	io.alexReversePower = 0.0;
//	io.penelopeForwardVolts = 0.0;
//	io.penelopeForwardPower = 0.0;
//	io.ain3Volts = 0.0;
//	io.ain4Volts = 0.0;
//	io.supplyVolts = 0.0f;
//
//	for (int i = 0; i < 5; i++) {
//
//		io.control_in[i] = 0x00;
//		io.control_out[i] = 0x00;
//	}
//
//	// default config
//	io.control_out[0] = MOX_DISABLED;
//
//	uchar config;
//	io.control_out[1] = CONFIG_MERCURY | MERCURY_122_88MHZ_SOURCE | MERCURY_10MHZ_SOURCE | MIC_SOURCE_PENELOPE | SPEED_192KHZ;
//	io.control_out[2] = MODE_OTHERS;
//	io.control_out[3] = ALEX_ATTENUATION_0DB | LT2208_GAIN_ON | LT2208_DITHER_ON | LT2208_RANDOM_ON;
//	io.control_out[4] = DUPLEX | ((io.receivers - 1) << 3);
//	
//	io.control_out[4] &= 0xC7;
//	io.control_out[4] |= (io.receivers - 1) << 3;
//
//	io.preamp = settings->getMercuryPreamp();
//	io.control_out[3] = io.control_out[3] & 0xFB;
//	io.control_out[3] = io.control_out[3] | (io.preamp << 2);
//
//	io.dither = settings->getMercuryDither();
//	io.control_out[3] = io.control_out[3] & 0xF7;
//	io.control_out[3] = io.control_out[3] | (io.dither << 3);
//
//	io.random = settings->getMercuryRandom();
//	io.control_out[3] = io.control_out[3] & 0xEF;
//	io.control_out[3] = io.control_out[3] | (io.random << 4);
//
//	io.ccTx.hpsdr10MhzSource = settings->get10MHzSource();
//	io.control_out[1] = io.control_out[1] & 0xF3;
//	io.control_out[1] = io.control_out[1] | (io.ccTx.hpsdr10MhzSource << 2);
//
//	io.ccTx.hpsdr122_88MhzSource = settings->get122_8MHzSource();
//	io.control_out[1] = io.control_out[1] & 0xEF;
//	io.control_out[1] = io.control_out[1] | (io.ccTx.hpsdr122_88MhzSource << 4);
//
//	io.mic_source = settings->getMicSource();
//	io.control_out[1] = io.control_out[1] & 0x7F;
//	io.control_out[1] = io.control_out[1] | (io.mic_source << 7);
//
//	io.rxClass = settings->getRxClass();
//	io.control_out[2] = io.control_out[2] & 0xFE;
//	io.control_out[2] = io.control_out[2] | io.rxClass;
//		
//	io.mic_gain = 0.26F;
//	io.rx_freq_change = -1;
//
//	io.clients = 0;
//	io.sendIQ_toggle = true;
//	io.rcveIQ_toggle = false;
//
//	setSampleRate(this, settings->getSampleRate());
//}

void DataEngine::initDttSPInterface(int rx) {

	QDttSP *dttsp = new QDttSP(this, rx);
	dttSPList.append(dttsp);
	
	if (dttSPList[rx])
		dttSPList[rx]->setDttSPStatus(true);
	else
		DATA_ENGINE_DEBUG << "could not start DttSP for receiver: " << rx;


	dttSPList[rx]->dttspSetTRX(0, 0); // thread 0 is for receive; 1st arg = thread; 2nd arg: 0 = Rx,  1 = Tx
	
	m_offset = 0;
	dttSPList[rx]->dttspSetRingBufferOffset(0, m_offset);
	// 1st arg = thread; 2nd arg = RunMode: 0 = RUN_MUTE, 1 = RUN_PASS, 2 = RUN_PLAY, 3 = RUN_SWCH
	dttSPList[rx]->dttspSetThreadProcessingMode(0, 2);
	
	// 1st arg = thread; 2nd arg = sub-receiver number; 3rd arg: 0 = inactive, 1 = active
	dttSPList[rx]->dttspSetSubRXSt(0, 0, 1);

	 // 1st arg = thread; 2nd arg = sub-receiver number; 3rd arg = audio gain from 0.0 to 1.0
	dttSPList[rx]->dttspSetRXOutputGain(0, 0, settings->getMainVolume(rx));
	dttSPList[rx]->dttspSetDSPBuflen(0, BUFFER_SIZE);
	
	//dttSPList[rx]->setDttSPStatus(true);

	//dttSPList[rx]->dttspSetSpectrumPolyphase(0, true);
	//dttSPList[rx]->dttspSetSpectrumWindow(0, HANNING_WINDOW);

	dttSPList[rx]->dttspSetSampleRate((double)io.samplerate);
	dttSPList[rx]->dttspSetRXOsc(0, 0, 0.0);
	//dttSPList[rx]->dttspSetRXOutputGain(0, 0, settings->getMainVolume(rx));
	
	setDttSPMainVolume(this, rx, rxList.at(rx)->getAudioVolume());
	setDttspDspMode(this, rx, rxList.at(rx)->getDSPMode());
	setDttspAgcMode(this, rx, rxList.at(rx)->getAGCMode());
	setDttspAgcGain(this, rx, rxList.at(rx)->getAGCGain());

	m_spectrumCounter = 0;
	m_specCntList[rx] = 0;

	DATA_ENGINE_DEBUG << "DttSP for receiver: " << rx << " started.";
}

void DataEngine::initDSPEngine(int rx) {

	QDSPEngine *dspEngine = new QDSPEngine(this, rx, BUFFER_SIZE);
	qtdspList.append(dspEngine);

	if (qtdspList[rx])
		qtdspList[rx]->setQtDSPStatus(true);
	else
		DATA_ENGINE_DEBUG << "could not start QtDSP for receiver: " << rx;

	//qtdspList[rx]->setVolume(settings->getMainVolume(rx));
	setQtDSPMainVolume(this, rx, rxList.at(rx)->getAudioVolume());

	m_spectrumCounter = 0;
	m_specCntList[rx] = 0;

	DATA_ENGINE_DEBUG << "QtDSP for receiver: " << rx << " started.";
}

void DataEngine::connectDTTSP() {

	/*CHECKED_CONNECT(
		dttspProcessing,
		SIGNAL(resultReadyAt(int)),
		this,
		SLOT(dttspReadyAt(int)));*/

	CHECKED_CONNECT(
		settings, 
		SIGNAL(dspModeChanged(QObject *, int, DSPMode)), 
		this, 
		SLOT(setDttspDspMode(QObject *, int, DSPMode)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(agcModeChanged(QObject *, int, AGCMode)), 
		this, 
		SLOT(setDttspAgcMode(QObject *, int, AGCMode)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(agcGainChanged(QObject *, int, int)), 
		this, 
		SLOT(setDttspAgcGain(QObject *, int, int)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(filterFrequenciesChanged(QObject *, int, qreal, qreal)),
		this,
		SLOT(setDttspRXFilter(QObject *, int, qreal, qreal)));

	CHECKED_CONNECT(
		settings,
		SIGNAL(frequencyChanged(QObject *, bool, int, long)),
		this,
		SLOT(setDttspFrequency(QObject *, bool, int, long)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(mainVolumeChanged(QObject *, int, float)), 
		this, 
		SLOT(setDttSPMainVolume(QObject *, int, float)));
}

void DataEngine::connectQTDSP() {

	CHECKED_CONNECT(
		settings, 
		SIGNAL(filterFrequenciesChanged(QObject *, int, qreal, qreal)),
		this,
		SLOT(setRXFilter(QObject *, int, qreal, qreal)));

	CHECKED_CONNECT(
		settings,
		SIGNAL(frequencyChanged(QObject *, bool, int, long)),
		this,
		SLOT(setFrequency(QObject *, bool, int, long)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(mainVolumeChanged(QObject *, int, float)), 
		this, 
		SLOT(setQtDSPMainVolume(QObject *, int, float)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(agcModeChanged(QObject *, int, AGCMode)), 
		this, 
		SLOT(setAgcMode(QObject *, int, AGCMode)));

	CHECKED_CONNECT(
		settings, 
		SIGNAL(agcGainChanged(QObject *, int, int)), 
		this, 
		SLOT(setAgcGain(QObject *, int, int)));
}

void DataEngine::disconnectDTTSP() {

	disconnect(
		settings,
		SIGNAL(frequencyChanged(QObject *, bool, int, long)),
		this,
		SLOT(setDttspFrequency(QObject *, bool, int, long)));

	disconnect(
		settings, 
		SIGNAL(dspModeChanged(QObject *, int, DSPMode)), 
		this, 
		SLOT(setDttspDspMode(QObject *, int, DSPMode)));

	disconnect(
		settings, 
		SIGNAL(agcModeChanged(QObject *, int, AGCMode)), 
		this, 
		SLOT(setDttspAgcMode(QObject *, int, AGCMode)));

	disconnect(
		settings, 
		SIGNAL(agcGainChanged(QObject *, int, int)), 
		this, 
		SLOT(setDttspAgcGain(QObject *, int, int)));

	disconnect(
		settings, 
		SIGNAL(filterFrequenciesChanged(QObject *, int, qreal, qreal)),
		this,
		SLOT(setDttspRXFilter(QObject *, int, qreal, qreal)));

	disconnect(
		settings, 
		SIGNAL(mainVolumeChanged(QObject *, int, float)), 
		this, 
		SLOT(setDttSPMainVolume(QObject *, int, float)));
}

void DataEngine::disconnectQTDSP() {

	disconnect(
		settings, 
		SIGNAL(mainVolumeChanged(QObject *, int, float)), 
		this, 
		SLOT(setQtDSPMainVolume(QObject *, int, float)));

	disconnect(
		settings, 
		SIGNAL(agcModeChanged(QObject *, int, AGCMode)), 
		this, 
		SLOT(setAgcMode(QObject *, int, AGCMode)));

	disconnect(
		settings, 
		SIGNAL(agcGainChanged(QObject *, int, int)), 
		this, 
		SLOT(setAgcGain(QObject *, int, int)));

	disconnect(
		settings, 
		SIGNAL(filterFrequenciesChanged(QObject *, int, qreal, qreal)),
		this,
		SLOT(setRXFilter(QObject *, int, qreal, qreal)));
}

void DataEngine::sendInitFramesToNetworkDevice(int rx) {

	QByteArray initDatagram;
	initDatagram.resize(1032);
	
	initDatagram[0] = (char)0xEF;
	initDatagram[1] = (char)0xFE;
	initDatagram[2] = (char)0x01;
	initDatagram[3] = (char)0x02;
	initDatagram[4] = (char)0x00;
	initDatagram[5] = (char)0x00;
	initDatagram[6] = (char)0x00;
	initDatagram[7] = (char)0x00;

	initDatagram[8] = SYNC;
    initDatagram[9] = SYNC;
    initDatagram[10] = SYNC;

	for (int i = 0; i < 5; i++) {
		
		initDatagram[i + 11]  = io.control_out[i];
	}

	for (int i = 16; i < 520; i++) {
		
		initDatagram[i]  = 0x00;
	}

	initDatagram[520] = SYNC;
    initDatagram[521] = SYNC;
    initDatagram[522] = SYNC;
	
	initDatagram[523] = io.control_out[0] | ((rx + 2) << 1);
	initDatagram[524] = rxList[rx]->getFrequency() >> 24;
	initDatagram[525] = rxList[rx]->getFrequency() >> 16;
	initDatagram[526] = rxList[rx]->getFrequency() >> 8;
	initDatagram[527] = rxList[rx]->getFrequency();

	/*io.output_buffer[3] = io.control_out[0] | ((rx + 2) << 1);
	io.output_buffer[4] = rxList[rx]->getFrequency() >> 24;
	io.output_buffer[5] = rxList[rx]->getFrequency() >> 16;
	io.output_buffer[6] = rxList[rx]->getFrequency() >> 8;
	io.output_buffer[7] = rxList[rx]->getFrequency();*/
	
	/*initDatagram[523] = io.control_out[0] | (2 << 1);
	initDatagram[524] = io.initialFrequency[0] >> 24;
	initDatagram[525] = io.initialFrequency[0] >> 16;
	initDatagram[526] = io.initialFrequency[0] >> 8;
	initDatagram[527] = io.initialFrequency[0];*/

	for (int i = 528; i < 1032; i++) {
		
		initDatagram[i]  = 0x00;
	}

	QUdpSocket socket;
	socket.bind(QHostAddress(settings->getHPSDRDeviceLocalAddr()), 
				settings->metisPort(),
				QUdpSocket::ReuseAddressHint | QUdpSocket::ShareAddress);
				
	for (int i = 0; i < 1; i++) {
		
		if (socket.writeDatagram(initDatagram.data(), initDatagram.size(), io.metisIPAddress, METIS_PORT) < 0)
			DATA_ENGINE_DEBUG << "error sending init data to device: " << qPrintable(socket.errorString());
		else {

			if (i == 0) DATA_ENGINE_DEBUG << "init frames sent to network device.";
		}
	}
	socket.close();
}
 
//********************************************************
// create, start/stop Metis network IO

void DataEngine::createHpsdrIO() {

	m_hpsdrIO = new QHpsdrIO(&io);

	m_netIOThread = new QThreadEx();
	m_hpsdrIO->moveToThread(m_netIOThread);

	m_hpsdrIO->connect(
					m_netIOThread, 
					SIGNAL(started()), 
					SLOT(initHPSDRDevice()));
}

bool DataEngine::startHpsdrIO(QThread::Priority prio) {

	m_netIOThread->start(prio);

	if (m_netIOThread->isRunning()) {
					
		m_netIOThreadRunning = true;
		DATA_ENGINE_DEBUG << "HPSDR network IO thread started.";

		return true;
	}
	else {

		m_netIOThreadRunning = false;
		return false;
	}
}

void DataEngine::stopHpsdrIO() {

	if (m_netIOThread->isRunning()) {
		
		m_netIOThread->quit();
		m_netIOThread->wait(1000);
		delete m_netIOThread;
		delete m_hpsdrIO;
		m_hpsdrIO = 0;

		m_netIOThreadRunning = false;

		DATA_ENGINE_DEBUG << "HPSDR discovery thread stopped and deleted.";
	}
	else
		DATA_ENGINE_DEBUG << "network IO thread wasn't started.";
}

void DataEngine::networkDeviceStartStop(char value) {

	TNetworkDevicecard metis = settings->getCurrentMetisCard();
	QUdpSocket socket;
	socket.bind(QHostAddress(settings->getHPSDRDeviceLocalAddr()), 
				settings->metisPort(),
				QUdpSocket::ReuseAddressHint | QUdpSocket::ShareAddress);
				
	m_commandDatagram.resize(64);
	m_commandDatagram[0] = (char)0xEF;
	m_commandDatagram[1] = (char)0xFE;
	m_commandDatagram[2] = (char)0x04;
	m_commandDatagram[3] = (char)value;

	for (int i = 4; i < 64; i++) m_commandDatagram[i] = 0x00;

	//if (socket.writeDatagram(m_commandDatagram, m_metisCards[0].ip_address, METIS_PORT) == 64) {
	if (socket.writeDatagram(m_commandDatagram, metis.ip_address, METIS_PORT) == 64) {
	
		//if (value == 1) {
		if (value != 0) {

			DATA_ENGINE_DEBUG << "sent start command to device at: "<< qPrintable(metis.ip_address.toString());
			m_networkDeviceRunning = true;
		}
		else {

			//DATA_ENGINE_DEBUG << "sent stop command to Metis at"<< m_metisCards[0].ip_address.toString();
			DATA_ENGINE_DEBUG << "sent stop command to device at: "<< qPrintable(metis.ip_address.toString());
			m_networkDeviceRunning = false;
		}
	}
	else
		DATA_ENGINE_DEBUG << "sending command to device failed.";
			
	socket.close();
}

//********************************************************
// create, start/stop data receiver

void DataEngine::createDataReceiver() {

	m_dataReceiver = new DataReceiver(&io);

	CHECKED_CONNECT(
		m_dataReceiver, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(newMessage(QString)));

	switch (m_serverMode) {
		
		case QSDR::ExternalDSP:
			break;

		//case QSDR::InternalDSP:
		case QSDR::DttSP:
		case QSDR::QtDSP:
			m_message = "configured for %1 receiver(s) at %2 kHz sample rate.";	
			sendMessage(
				m_message.arg( 
					QString::number(settings->getNumberOfReceivers()), 
					QString::number(settings->getSampleRate()/1000)));
			break;

		case QSDR::ChirpWSPR:
		case QSDR::ChirpWSPRFile:
			break;
	}

	m_dataRcvrThread = new QThreadEx();
	m_dataReceiver->moveToThread(m_dataRcvrThread);

	switch (m_hwInterface) {

		case QSDR::NoInterfaceMode:

			m_dataReceiver->connect(
						m_dataRcvrThread, 
						SIGNAL(started()), 
						SLOT(readData()));
			break;
			
		case QSDR::Metis:
		case QSDR::Hermes:

			m_dataReceiver->connect(
						m_dataRcvrThread, 
						SIGNAL(started()), 
						SLOT(initDataReceiverSocket()));
			break;
	}
}

bool DataEngine::startDataReceiver(QThread::Priority prio) {

	m_dataRcvrThread->start(prio);

	if (m_dataRcvrThread->isRunning()) {
					
		m_dataRcvrThreadRunning = true;
		DATA_ENGINE_DEBUG << "data receiver thread started.";

		return true;
	}
	else {

		m_dataRcvrThreadRunning = false;
		settings->setSystemState(
						this, 
						QSDR::DataProcessThreadError, 
						m_hwInterface, 
						m_serverMode,
						QSDR::DataEngineDown);
		return false;
	}
}

void DataEngine::stopDataReceiver() {

	if (m_dataRcvrThread->isRunning()) {
					
		m_dataReceiver->stop();

		/*if (m_serverMode == QSDR::InternalDSP || m_serverMode == QSDR::ChirpWSPR) {

			if (io.iq_queue.isEmpty())
				io.iq_queue.enqueue(m_datagram);
		}*/

		m_dataRcvrThread->quit();
		m_dataRcvrThread->wait(1000);
		delete m_dataRcvrThread;
		delete m_dataReceiver;
		m_dataReceiver = 0;

		if (m_serverMode == QSDR::ChirpWSPRFile) {

			while (!io.chirp_queue.isEmpty())
				io.chirp_queue.dequeue();
		}

		m_dataRcvrThreadRunning = false;

		DATA_ENGINE_DEBUG << "data receiver thread deleted.";
	}
	else
		DATA_ENGINE_DEBUG << "data receiver thread wasn't started.";
}

 
//********************************************************
// create, start/stop data processor

void DataEngine::createDataProcessor() {

	m_dataProcessor = new DataProcessor(this, m_serverMode);

	CHECKED_CONNECT(
		m_dataProcessor, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(newMessage(QString)));

	switch (m_serverMode) {
		
		// The signal iqDataReady is generated by the function
		// processInputBuffer when a block of input data are
		// decoded.
		case QSDR::ExternalDSP:
			
			CHECKED_CONNECT_OPT(
				this,
				SIGNAL(iqDataReady(int)),
				m_dataProcessor,
				SLOT(externalDspProcessing(int)),
				Qt::DirectConnection);

			break;

		//case QSDR::InternalDSP:
		case QSDR::DttSP:
		case QSDR::QtDSP:
		case QSDR::ChirpWSPR:
		case QSDR::ChirpWSPRFile:

			/*connect(
				this,
				SIGNAL(iqDataReady(int)),
				SLOT(dttSPDspProcessing(int)),
				Qt::DirectConnection);*/
			
			break;

		/*case QSDR::ChirpWSPR:
		case QSDR::ChirpWSPRFile:
			break;*/
	}

	m_dataProcThread = new QThreadEx();
	m_dataProcessor->moveToThread(m_dataProcThread);

	switch (m_hwInterface) {

		case QSDR::NoInterfaceMode:
			m_dataProcessor->connect(
						m_dataProcThread, 
						SIGNAL(started()), 
						SLOT(processData()));
			break;
			
		case QSDR::Metis:
		case QSDR::Hermes:
			m_dataProcessor->connect(
						m_dataProcThread, 
						SIGNAL(started()), 
						SLOT(processDeviceData()));

			break;
	}
	
	//m_dataProcessor->connect(m_dataProcThread, SIGNAL(started()), SLOT(initDataProcessorSocket()));

}

bool DataEngine::startDataProcessor(QThread::Priority prio) {

	m_dataProcThread->start(prio);
				
	if (m_dataProcThread->isRunning()) {
					
		m_dataProcThreadRunning = true;
		DATA_ENGINE_DEBUG << "data processor thread started.";

		return true;
	}
	else {

		m_dataProcThreadRunning = false;
		settings->setSystemState(
						this, 
						QSDR::DataProcessThreadError, 
						m_hwInterface, 
						m_serverMode,
						QSDR::DataEngineDown);
		return false;
	}
}

void DataEngine::stopDataProcessor() {

	if (m_dataProcThread->isRunning()) {
					
		m_dataProcessor->stop();
		
		//if (m_serverMode == QSDR::InternalDSP || m_serverMode == QSDR::ChirpWSPR) {
		if (m_serverMode == QSDR::DttSP || m_serverMode == QSDR::QtDSP || m_serverMode == QSDR::ChirpWSPR) {
			
			if (io.iq_queue.isEmpty())
				io.iq_queue.enqueue(QByteArray(BUFFER_SIZE, 0x0));
		}
		else if (m_serverMode == QSDR::ChirpWSPRFile) {

			if (io.data_queue.isEmpty()) {
				
				QList<qreal> buf;
				for (int i = 0; i < 128; i++) buf << 0.0f;
				io.data_queue.enqueue(buf);
			}
		}
				
		m_dataProcThread->quit();
		m_dataProcThread->wait();
		delete m_dataProcThread;
		delete m_dataProcessor;
		m_dataProcessor = 0;

		//if (m_serverMode == QSDR::InternalDSP || m_serverMode == QSDR::ChirpWSPR) {
		if (m_serverMode == QSDR::DttSP || m_serverMode == QSDR::QtDSP || m_serverMode == QSDR::ChirpWSPR) {

			while (!io.iq_queue.isEmpty())
				io.iq_queue.dequeue();

			DATA_ENGINE_DEBUG << "iq_queue empty.";
		}
		else if (m_serverMode == QSDR::ChirpWSPRFile) {
			
			while (!io.data_queue.isEmpty())
				io.data_queue.dequeue();

			DATA_ENGINE_DEBUG << "data_queue empty.";
			chirpData.clear();
		}

		m_dataProcThreadRunning = false;

		DATA_ENGINE_DEBUG << "data processor thread deleted.";
	}
	else
		DATA_ENGINE_DEBUG << "data processor thread wasn't started.";
}

 
//********************************************************
// create, start/stop winde band data processor

void DataEngine::createWideBandDataProcessor() {

	int size;

	if (m_mercuryFW > 32 || m_hermesFW > 16)
		size = BIGWIDEBANDSIZE;
	else
		size = SMALLWIDEBANDSIZE;
	
	m_wbDataProcessor = new WideBandDataProcessor(this);

	CHECKED_CONNECT(
		m_wbDataProcessor, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(newMessage(QString)));

	switch (m_serverMode) {
		
		// The signal iqDataReady is generated by the function
		// processInputBuffer when a block of input data are
		// decoded.
		//case QSDR::InternalDSP:
		case QSDR::DttSP:
		case QSDR::QtDSP:

			m_wbFFT = new QFFT(this, size);
			
			// wide band data complex buffer
			io.cpxWBIn	= mallocCPX(size);
			io.cpxWBOut	= mallocCPX(size);
			io.cpxWBTmp	= mallocCPX(size);

			io.wbWindow.resize(size);
			io.wbWindow.fill(0.0f);

			memset(io.cpxWBIn, 0, size * sizeof(CPX));
			memset(io.cpxWBOut, 0, size * sizeof(CPX));
			memset(io.cpxWBTmp, 0, size * sizeof(CPX));
			//memset(io.wbWindow, 0, size * sizeof(float));
			
			QFilter::MakeWindow(12, size, (float *)io.wbWindow.data()); // 12 = BLACKMANHARRIS_WINDOW

			m_wbAverager = new DualModeAverager(this, size/2);

			break;

		case QSDR::ExternalDSP:
		case QSDR::ChirpWSPR:
		case QSDR::ChirpWSPRFile:

			break;
	}

	m_wbDataProcThread = new QThreadEx();
	m_wbDataProcessor->moveToThread(m_wbDataProcThread);
	m_wbDataProcessor->connect(
						m_wbDataProcThread, 
						SIGNAL(started()), 
						SLOT(processWideBandData()));
}

bool DataEngine::startWideBandDataProcessor(QThread::Priority prio) {

	m_wbDataProcThread->start(prio);//(QThread::TimeCriticalPriority);//(QThread::HighPriority);//(QThread::LowPriority);

	if (m_wbDataProcThread->isRunning()) {
					
		m_wbDataRcvrThreadRunning = true;
		DATA_ENGINE_DEBUG << "wide band data processor thread started.";

		return true;
	}
	else {

		m_wbDataRcvrThreadRunning = false;
		settings->setSystemState(
						this, 
						QSDR::WideBandDataProcessThreadError, 
						m_hwInterface, 
						m_serverMode,
						QSDR::DataEngineDown);
		return false;
	}
}

void DataEngine::stopWideBandDataProcessor() {

	if (m_wbDataProcThread->isRunning()) {
					
		m_wbDataProcessor->stop();
		if (io.wb_queue.isEmpty())
			io.wb_queue.enqueue(m_datagram);

		m_wbDataProcThread->quit();
		m_wbDataProcThread->wait();
		delete m_wbDataProcThread;
		delete m_wbDataProcessor;
		m_wbDataProcessor = 0;

		m_wbDataRcvrThreadRunning = false;

		delete m_wbFFT;
		freeCPX(io.cpxWBIn);
		freeCPX(io.cpxWBOut);
		
		DATA_ENGINE_DEBUG << "wide band data processor thread deleted.";
	}
	else
		DATA_ENGINE_DEBUG << "wide band data processor thread wasn't started.";
}

 
//********************************************************
// create, start/stop chirp processor
void DataEngine::createChirpDataProcessor() {

	m_chirpProcessor = new ChirpProcessor(&io);
	sendMessage("chirp decoder initialized.");
	
	CHECKED_CONNECT(
		m_chirpProcessor, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(newMessage(QString)));

	CHECKED_CONNECT_OPT(
		m_audioEngine, 
		SIGNAL(chirpSignalChanged()),
		m_chirpProcessor,
		SLOT(generateLocalChirp()),
		Qt::DirectConnection);

	m_audioEngine->reset();
	if (m_audioEngine->generateSweptTone())
		sendMessage("audio chirp signal initialized.");
	else
		sendMessage("audio chirp signal initialization failed.");


	m_chirpDataProcThread = new QThreadEx();
	m_chirpProcessor->moveToThread(m_chirpDataProcThread);
	m_chirpProcessor->connect(
						m_chirpDataProcThread, 
						SIGNAL(started()),
						m_chirpProcessor,
						SLOT(processChirpData()));

	m_chirpInititalized = true;
}

bool DataEngine::startChirpDataProcessor(QThread::Priority prio) {

	m_chirpDataProcThread->start(prio);//(QThread::TimeCriticalPriority);//(QThread::HighPriority);//(QThread::LowPriority);
				
	if (m_chirpDataProcThread->isRunning()) {
					
		m_chirpDataProcThreadRunning = true;
		DATA_ENGINE_DEBUG << "chirp data processor thread started.";

		return true;
	}
	else {

		m_chirpDataProcThreadRunning = false;
		settings->setSystemState(
						this, 
						QSDR::DataProcessThreadError, 
						m_hwInterface, 
						m_serverMode,
						QSDR::DataEngineDown);
		return false;
	}
}

void DataEngine::stopChirpDataProcessor() {

	if (m_chirpInititalized) {

		m_chirpProcessor->stop();
		if (io.chirp_queue.isEmpty()) {
				
			QList<qreal> buf;
			for (int i = 0; i < 128; i++) buf << 0.0f;
				io.chirp_queue.enqueue(buf);
			}

			m_chirpDataProcThread->quit();
			m_chirpDataProcThread->wait();
			delete m_chirpDataProcThread;
			delete m_chirpProcessor;
			m_chirpProcessor = 0;

			if (m_hwInterface == QSDR::NoInterfaceMode) {

				freeCPX(io.cpxIn);
				freeCPX(io.cpxOut);
				delete m_chirpDspEngine;

				while (!io.chirp_queue.isEmpty())
					io.chirp_queue.dequeue();

				DATA_ENGINE_DEBUG << "io.cpxIn, io.cpxOut, fft deleted, io.chirp_queue empty.";
			}

			m_chirpInititalized = false;

			DATA_ENGINE_DEBUG << "chirp data processor thread deleted.";
	}
	else
		DATA_ENGINE_DEBUG << "chirp data processor thread wasn't started.";
}

 
//********************************************************
// create, start/stop audio receiver

void DataEngine::createAudioReceiver() {

	m_audioReceiver = new AudioReceiver(&io);

	CHECKED_CONNECT(
		m_audioReceiver, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(newMessage(QString)));

	CHECKED_CONNECT(
		m_audioReceiver, 
		SIGNAL(rcveIQEvent(QObject *, int)), 
		this, 
		SLOT(setRcveIQSignal(QObject *, int)));

	CHECKED_CONNECT(
		m_audioReceiver, 
		SIGNAL(clientConnectedEvent(bool)), 
		this, 
		SLOT(setClientConnected(bool)));

	
	m_AudioRcvrThread = new QThreadEx();
	m_audioReceiver->moveToThread(m_AudioRcvrThread);

	m_audioReceiver->connect(
						m_AudioRcvrThread, 
						SIGNAL(started()), 
						SLOT(initClient()));
}

void DataEngine::createAudioProcessor() {

	m_audioProcessor = new AudioProcessor(this);
	
	CHECKED_CONNECT(
		m_audioProcessor, 
		SIGNAL(messageEvent(QString)), 
		this, 
		SLOT(newMessage(QString)));

	/*CHECKED_CONNECT(
		m_audioProcessor, 
		SIGNAL(AudioProcessorRunningEvent(bool)), 
		this, 
		SLOT(setAudioProcessorRunning(bool)));*/

	CHECKED_CONNECT_OPT(
		this, 
		SIGNAL(clientConnectedEvent(int)), 
		m_audioProcessor, 
		SLOT(clientConnected(int)), 
		Qt::DirectConnection);

	CHECKED_CONNECT_OPT(
		this, 
		SIGNAL(audioRxEvent(int)), 
		m_audioProcessor, 
		SLOT(audioReceiverChanged(int)), 
		Qt::DirectConnection);

	CHECKED_CONNECT_OPT(
		this,
		SIGNAL(audioDataReady()),
		m_audioProcessor,
		SLOT(deviceWriteBuffer()),
		Qt::DirectConnection);

	switch (m_serverMode) {

		case QSDR::ExternalDSP:
			
			m_audioProcThread = new QThreadEx();
			m_audioProcessor->moveToThread(m_audioProcThread);
			m_audioProcessor->connect(
								m_audioProcThread, 
								SIGNAL(started()), 
								SLOT(processAudioData()));
			break;

		//case QSDR::InternalDSP:
		case QSDR::DttSP:
		case QSDR::QtDSP:

			break;
	}

	/*m_audioProcThread = new QThreadEx();
	m_audioProcessor->moveToThread(m_udioProcThread);
	m_audioProcessor->connect(m_audioProcThread, SIGNAL(started()), SLOT(processAudioData()));*/

	//setSampleRate(this, settings->getSampleRate());
}

bool DataEngine::startAudioProcessor(QThread::Priority prio, QSDR::_ServerMode mode) {

	if (!m_audioProcessorRunning) {

		switch (mode) {

			//case QSDR::InternalDSP:
			case QSDR::DttSP:
			case QSDR::QtDSP:
			case QSDR::ChirpWSPR:

				m_audioProcessor->initAudioProcessorSocket();
				m_audioProcessorRunning = true;

				return true;
		}

		return false;
	}
	else
		return false;
}

void DataEngine::stopAudioProcessor() {

	if (m_audioProcessorRunning) {
				
		m_audioProcessor->stop();
		io.au_queue.enqueue(m_datagram);

		if (m_audioProcThreadRunning) {
			m_audioProcThread->quit();
			m_audioProcThread->wait();
		}
		delete m_audioProcessor;
		m_audioProcessor = 0;

		m_audioProcessorRunning = false;

		DATA_ENGINE_DEBUG << "audio processor thread deleted.";
	}
	else
		DATA_ENGINE_DEBUG << "audio processor thread wasn't started.";
}
 
void DataEngine::displayDiscoverySocketError(QAbstractSocket::SocketError error) {

	DATA_ENGINE_DEBUG << "discovery socket error:" << error;
}

 
//*****************************************************************************
// HPSDR data processing functions

void DataEngine::processInputBuffer(const QByteArray &buffer) {

	int s = 0;

	if (buffer.at(s++) == SYNC && buffer.at(s++) == SYNC && buffer.at(s++) == SYNC) {

		if (syncToggle) { // toggle sync signal

			settings->setProtocolSync(1);
			syncToggle = false;
		}

        // extract control bytes
        io.control_in[0] = buffer.at(s++);
        io.control_in[1] = buffer.at(s++);
        io.control_in[2] = buffer.at(s++);
        io.control_in[3] = buffer.at(s++);
        io.control_in[4] = buffer.at(s++);

        io.ccRx.ptt    = (bool)((io.control_in[0] & 0x01) == 0x01);
		io.ccRx.dash   = (bool)((io.control_in[0] & 0x02) == 0x02);
		io.ccRx.dot    = (bool)((io.control_in[0] & 0x04) == 0x04);
		io.ccRx.lt2208 = (bool)((io.control_in[1] & 0x01) == 0x01);

		//if (io.ccRx.lt2208) { // toggle ADC signal

		//	if (adcToggle) {
		//			settings->setADCOverflow(2);
		//			adcToggle = false;
		//	}
		//}
		//else {
		//	
		//	if (!adcToggle) {
		//		settings->setADCOverflow(1);
		//		adcToggle = true;
		//	}
		//}

		io.ccRx.roundRobin = (uchar)(io.control_in[0] >> 3);
        switch (io.ccRx.roundRobin) { // cycle through C0

			case 0:

				if (io.ccRx.lt2208) { // toggle ADC signal
					if (adcToggle) {
						settings->setADCOverflow(2);
						adcToggle = false;
					}
				}
				else {
					if (!adcToggle) {
						settings->setADCOverflow(1);
						adcToggle = true;
					}
				}
				//qDebug() << "CC: " << io.ccRx.roundRobin;
				if (m_hwInterface == QSDR::Hermes) {

					io.ccRx.hermesI01 = (bool)((io.control_in[1] & 0x02) == 0x02);
					io.ccRx.hermesI02 = (bool)((io.control_in[1] & 0x04) == 0x04);
					io.ccRx.hermesI03 = (bool)((io.control_in[1] & 0x08) == 0x08);
					io.ccRx.hermesI04 = (bool)((io.control_in[1] & 0x10) == 0x10);
					//qDebug() << "Hermes IO 1: " << io.ccRx.hermesI01 << "2: " << io.ccRx.hermesI02 << "3: " << io.ccRx.hermesI03 << "4: " << io.ccRx.hermesI04;
				}

				if (m_hwInterface == QSDR::Metis) {
				
					if (io.ccRx.mercuryFirmwareVersion != io.control_in[2]) {
						io.ccRx.mercuryFirmwareVersion = io.control_in[2];
						settings->setMercuryVersion(io.ccRx.mercuryFirmwareVersion);
						m_message = "Mercury firmware version: %1.";
						sendMessage(m_message.arg(QString::number(io.control_in[2])));
					}
			
					if (io.ccRx.penelopeFirmwareVersion != io.control_in[3]) {
						io.ccRx.penelopeFirmwareVersion = io.control_in[3];
						settings->setPenelopeVersion(io.ccRx.penelopeFirmwareVersion);
						m_message = "Penelope firmware version: %1.";
						sendMessage(m_message.arg(QString::number(io.control_in[3])));
					}
			
					if (io.ccRx.networkDeviceFirmwareVersion != io.control_in[4]) {
						io.ccRx.networkDeviceFirmwareVersion = io.control_in[4];
						settings->setMetisVersion(io.ccRx.networkDeviceFirmwareVersion);
						m_message = "Metis firmware version: %1.";

						sendMessage(m_message.arg(QString::number(io.control_in[4])));
					}
				}
				else if (m_hwInterface == QSDR::Hermes) {

					if (io.ccRx.networkDeviceFirmwareVersion != io.control_in[4]) {
						io.ccRx.networkDeviceFirmwareVersion = io.control_in[4];
						settings->setHermesVersion(io.ccRx.networkDeviceFirmwareVersion);
						m_message = "Hermes firmware version: %1.";

						sendMessage(m_message.arg(QString::number(io.control_in[4])));
					}
				}
				break;

			case 1:

				//qDebug() << "CC: " << io.ccRx.roundRobin;
				// forward power
				if (settings->getPenelopePresence() || (m_hwInterface == QSDR::Hermes)) { // || settings->getPennyLanePresence()
					
					io.ccRx.ain5 = (quint16)((quint16)(io.control_in[1] << 8) + (quint16)io.control_in[2]);
					
					io.penelopeForwardVolts = (qreal)(3.3 * (qreal)io.ccRx.ain5 / 4095.0);
					io.penelopeForwardPower = (qreal)(io.penelopeForwardVolts * io.penelopeForwardVolts / 0.09);
				}
				//qDebug() << "penelopeForwardVolts: " << io.penelopeForwardVolts << "penelopeForwardPower" << io.penelopeForwardPower;
				
				if (settings->getAlexPresence()) { //|| settings->getApolloPresence()) {
					
					io.ccRx.ain1 = (quint16)((quint16)(io.control_in[3] << 8) + (quint16)io.control_in[4]);
					
					io.alexForwardVolts = (qreal)(3.3 * (qreal)io.ccRx.ain1 / 4095.0);
					io.alexForwardPower = (qreal)(io.alexForwardVolts * io.alexForwardVolts / 0.09);
				}
				//qDebug() << "alexForwardVolts: " << io.alexForwardVolts << "alexForwardPower" << io.alexForwardPower;
                break;

			case 2:

				//qDebug() << "CC: " << io.ccRx.roundRobin;
				// reverse power
				if (settings->getAlexPresence()) { //|| settings->getApolloPresence()) {
					
					io.ccRx.ain2 = (quint16)((quint16)(io.control_in[1] << 8) + (quint16)io.control_in[2]);
					
					io.alexReverseVolts = (qreal)(3.3 * (qreal)io.ccRx.ain2 / 4095.0);
					io.alexReversePower = (qreal)(io.alexReverseVolts * io.alexReverseVolts / 0.09);
				}
				//qDebug() << "alexReverseVolts: " << io.alexReverseVolts << "alexReversePower" << io.alexReversePower;
				
				if (settings->getPenelopePresence() || (m_hwInterface == QSDR::Hermes)) { // || settings->getPennyLanePresence() {
					
					io.ccRx.ain3 = (quint16)((quint16)(io.control_in[3] << 8) + (quint16)io.control_in[4]);
					io.ain3Volts = (qreal)(3.3 * (double)io.ccRx.ain3 / 4095.0);
				}
				//qDebug() << "ain3Volts: " << io.ain3Volts;
				break;

			case 3:

				//qDebug() << "CC: " << io.ccRx.roundRobin;
				
				if (settings->getPenelopePresence() || (m_hwInterface == QSDR::Hermes)) { // || settings->getPennyLanePresence() {
					
					io.ccRx.ain4 = (quint16)((quint16)(io.control_in[1] << 8) + (quint16)io.control_in[2]);
					io.ccRx.ain6 = (quint16)((quint16)(io.control_in[3] << 8) + (quint16)io.control_in[4]);
					
					io.ain4Volts = (qreal)(3.3 * (qreal)io.ccRx.ain4 / 4095.0);
					
					if (m_hwInterface == QSDR::Hermes) // read supply volts applied to board
						io.supplyVolts = (qreal)((qreal)io.ccRx.ain6 / 186.0f);
				}
				//qDebug() << "ain4Volts: " << io.ain4Volts << "supplyVolts" << io.supplyVolts;
				break;

			//case 4:

				// more than 1 Mercury module (currently not usable)
				//qDebug() << "CC: " << io.ccRx.roundRobin;
				//switch (io.receivers) {

				//	case 1:
				//		io.ccRx.mercury1_LT2208 = (bool)((io.control_in[1] & 0x02) == 0x02);
				//		//qDebug() << "mercury1_LT2208: " << io.ccRx.mercury1_LT2208;
				//		break;

				//	case 2:
				//		io.ccRx.mercury1_LT2208 = (bool)((io.control_in[1] & 0x02) == 0x02);
				//		io.ccRx.mercury2_LT2208 = (bool)((io.control_in[2] & 0x02) == 0x02);
				//		//qDebug() << "mercury1_LT2208: " << io.ccRx.mercury1_LT2208 << "mercury2_LT2208" << io.ccRx.mercury2_LT2208;
				//		break;

				//	case 3:
				//		io.ccRx.mercury1_LT2208 = (bool)((io.control_in[1] & 0x02) == 0x02);
				//		io.ccRx.mercury2_LT2208 = (bool)((io.control_in[2] & 0x02) == 0x02);
				//		io.ccRx.mercury3_LT2208 = (bool)((io.control_in[3] & 0x02) == 0x02);
				//		//qDebug() << "mercury1_LT2208: " << io.ccRx.mercury1_LT2208 << "mercury2_LT2208" << io.ccRx.mercury2_LT2208;
				//		//qDebug() << "mercury3_LT2208: " << io.ccRx.mercury3_LT2208;
				//		break;

				//	case 4:
				//		io.ccRx.mercury1_LT2208 = (bool)((io.control_in[1] & 0x02) == 0x02);
				//		io.ccRx.mercury2_LT2208 = (bool)((io.control_in[2] & 0x02) == 0x02);
				//		io.ccRx.mercury3_LT2208 = (bool)((io.control_in[3] & 0x02) == 0x02);
				//		io.ccRx.mercury4_LT2208 = (bool)((io.control_in[4] & 0x02) == 0x02);
				//		//qDebug() << "mercury1_LT2208: " << io.ccRx.mercury1_LT2208 << "mercury2_LT2208" << io.ccRx.mercury2_LT2208;
				//		//qDebug() << "mercury3_LT2208: " << io.ccRx.mercury3_LT2208 << "mercury4_LT2208" << io.ccRx.mercury4_LT2208;
				//		break;
				//}
				//break;
		}

        switch (io.receivers) {

            case 1: m_maxSamples = 512-0;  break;
            case 2: m_maxSamples = 512-0;  break;
            case 3: m_maxSamples = 512-4;  break;
            case 4: m_maxSamples = 512-10; break;
            case 5: m_maxSamples = 512-24; break;
            case 6: m_maxSamples = 512-10; break;
            case 7: m_maxSamples = 512-20; break;
            case 8: m_maxSamples = 512-4;  break;
        }

        // extract the samples
        while (s < m_maxSamples) {

            // extract each of the receivers
            for (int r = 0; r < io.receivers; r++) {

                m_leftSample   = (int)((  signed char) buffer.at(s++)) << 16;
                m_leftSample  += (int)((unsigned char) buffer.at(s++)) << 8;
                m_leftSample  += (int)((unsigned char) buffer.at(s++));
                m_rightSample  = (int)((  signed char) buffer.at(s++)) << 16;
                m_rightSample += (int)((unsigned char) buffer.at(s++)) << 8;
                m_rightSample += (int)((unsigned char) buffer.at(s++));
				
				m_lsample = (float)(m_leftSample / 8388607.0);
				m_rsample = (float)(m_rightSample / 8388607.0);

				if (m_serverMode == QSDR::ChirpWSPR && 
					m_chirpInititalized && 
					m_chirpSamples < io.samplerate
				) {
					chirpData << m_lsample;
					chirpData << m_rsample;
				}

				if (m_serverMode == QSDR::DttSP) {
					
					rxList[r]->in[m_rxSamples]				 = m_lsample; // 24 bit sample
					rxList[r]->in[m_rxSamples + BUFFER_SIZE] = m_rsample; // 24 bit sample
				}
				else if (m_serverMode == QSDR::QtDSP) {
					
					rxList[r]->inBuf[m_rxSamples].re = m_lsample; // 24 bit sample
					rxList[r]->inBuf[m_rxSamples].im = m_rsample; // 24 bit sample
				}
            }

            m_micSample = (int)((signed char) buffer.at(s++)) << 8;

			// extract chirp signal time stamp
			m_chirpBit = (buffer.at(s) & 0x01);// == 0x01;
			
			m_micSample += (int)((unsigned char) buffer.at(s++));
    		m_micSample_float = (float) m_micSample / 32767.0 * io.mic_gain; // 16 bit sample

            // add to buffer
            io.mic_left_buffer[m_rxSamples]  = m_micSample_float;
            io.mic_right_buffer[m_rxSamples] = 0.0f;

			//m_chirpSamples++;

			if (m_serverMode == QSDR::ChirpWSPR && m_chirpInititalized) {
				if (m_chirpBit) {
					if (m_chirpGateBit) {
					
						// we've found the rising edge of the GPS 1PPS signal, so we set the samples 
						// counter back to zero in order to have a simple and precise synchronisation 
						// with the local chirp.
						DATA_ENGINE_DEBUG << "GPS 1 PPS";

						// remove the last sample (real and imag) and enqueue the buffer
						chirpData.removeLast();
						chirpData.removeLast();
						io.chirp_queue.enqueue(chirpData);

						// empty the buffer and add the last sample, which is the starting point of the chirp
						m_chirpSamples = 0;
						chirpData.clear();

						chirpData << m_lsample;
						chirpData << m_rsample;

						m_chirpStart = true;
						m_chirpStartSample = m_rxSamples;
						m_chirpGateBit = false;
					}
				}
				else
					m_chirpGateBit = true;
			}
			m_rxSamples++;
			m_chirpSamples++;

			// when we have enough rx samples we start the DSP processing.
            if (m_rxSamples == BUFFER_SIZE) {
				
				// version 1
				/*for (int r = 0; r < io.receivers; r++) {

					QFuture<void> dspResult = run(spin, r);
					dttspReadyAt(r);
				}*/

				// version 2
				//dttspProcessing->setFuture(QtConcurrent::mapped(m_rx, spin));
				

				// classic version 1
				/*for (int r = 0; r < io.receivers; r++)
					dspProcessing(r);*/

				// classic version 2
				if (m_serverMode == QSDR::DttSP)
					dttspProcessing();
				else if (m_serverMode == QSDR::QtDSP)
					qtdspProcessing();

				m_rxSamples = 0;
            }
        }
    } 
	else {

		if (!syncToggle) {
			
			settings->setProtocolSync(2);
			syncToggle = true;
		}
	}
}

void DataEngine::processWideBandInputBuffer(const QByteArray &buffer) {

	int size;

	if (m_mercuryFW > 32 || m_hermesFW > 16)
		size = 2 * BIGWIDEBANDSIZE;
	else
		size = 2 * SMALLWIDEBANDSIZE;
	
	qint64 length = buffer.length();
	if (buffer.length() != size) {

		DATA_PROCESSOR_DEBUG << "wrong wide band buffer length: " << length;
		return;
	}

	int s;
	float sample;
	float norm = 1.0f / (4 * size);
	float mean = 0.0f;
	
	for (int i = 0; i < length; i += 2) {

		//sample = (float)((qint16)((buffer.at(i+1) << 8) | (buffer.at(i))) * norm);
		//mean += sample;

		s =  (int)((  signed char) buffer.at(i+1)) << 8;
        s += (int)((unsigned char) buffer.at(i));
		sample = (float)(s * norm);
		mean += sample;

		io.cpxWBIn[i/2].re = sample * io.wbWindow[i/2];
		io.cpxWBIn[i/2].im = io.cpxWBIn[i/2].re;//sample * io.wbWindow[i/2];
	}

	m_wbFFT->DoFFTWForward(io.cpxWBIn, io.cpxWBOut, size/2);
	
	// averaging
	QVector<float>	specBuf(size/4);

	wbMutex.lock();
	if (m_wbSpectrumAveraging) {
		
		for (int i = 0; i < size/4; i++)
			specBuf[i] = (float)(10.0 * log10(MagCPX(io.cpxWBOut[i]) + 1.5E-45));

		m_wbAverager->ProcessDBAverager(specBuf, specBuf);
		wbMutex.unlock();
	}
	else {

		for (int i = 0; i < size/4; i++)
			specBuf[i] = (float)(10.0 * log10(MagCPX(io.cpxWBOut[i]) + 1.5E-45));

		wbMutex.unlock();
	}

	settings->setWidebandSpectrumBuffer(specBuf);
}

void DataEngine::processFileBuffer(const QList<qreal> buffer) {

	
	int topsize = 2*BUFFER_SIZE - 1;
	//float specMax = -100.0f;
	//float specMin = 0.0f;

	Q_ASSERT(buffer.length() == 128);

	for (int i = 0; i < 64; i++) {
		
		io.cpxIn[i + m_rxSamples].re = buffer.at(2*i);
		io.cpxIn[i + m_rxSamples].im = buffer.at(2*i+1);

		chirpData << buffer.at(2*i);
		chirpData << buffer.at(2*i+1);

		m_chirpSamples++;
		if (m_chirpSamples == io.samplerate) {

			io.chirp_queue.enqueue(chirpData);
			chirpData.clear();
			m_chirpSamples = 0;
		}
	}
	m_rxSamples += 64;
		
	if (m_rxSamples == 2*BUFFER_SIZE) {
			
		m_chirpDspEngine->fft->DoFFTWForward(io.cpxIn, io.cpxOut, 2*BUFFER_SIZE);
			
		// reorder the spectrum buffer
		for (int i = 0; i < BUFFER_SIZE; i++) {
			
			m_spectrumBuffer[topsize - i] =
				(float)(10.0 * log10(MagCPX(io.cpxOut[i+BUFFER_SIZE]) + 1.5E-45));
			m_spectrumBuffer[BUFFER_SIZE - i] =
				(float)(10.0 * log10(MagCPX(io.cpxOut[i]) + 1.5E-45));
		}

		/*float specMean = 0.0f;
		for (int i = BUFFER_SIZE+20; i < BUFFER_SIZE+105; i++) {

			specMean += m_spectrumBuffer[i];
			if (m_spectrumBuffer[i] > specMax) specMax = m_spectrumBuffer[i];
			if (m_spectrumBuffer[i] < specMin) specMin = m_spectrumBuffer[i];
		}*/
		//specMean *= 1.0f/BUFFER_SIZE;
		//DATA_PROCESSOR_DEBUG << "pan min" << specMin << "max" << specMax << "mean" << specMean;

		SleeperThread::usleep(42667);
	
		//emit spectrumBufferChanged(m_spectrumBuffer);
		//settings->setSpectrumBuffer(m_spectrumBuffer);
		settings->setSpectrumBuffer(0, m_spectrumBuffer);
		
		m_rxSamples = 0;
	}
}

void DataEngine::processOutputBuffer(float *left, float *right) {

	qint16 leftRXSample;
    qint16 rightRXSample;
    qint16 leftTXSample;
    qint16 rightTXSample;

	// process the output
	for (int j = 0; j < BUFFER_SIZE; j += io.outputMultiplier) {

		leftRXSample  = (qint16)(left[j] * 32767.0f);
        rightRXSample = (qint16)(right[j] * 32767.0f);

		leftTXSample = 0;
        rightTXSample = 0;

		io.output_buffer[m_idx++] = leftRXSample  >> 8;
        io.output_buffer[m_idx++] = leftRXSample;
        io.output_buffer[m_idx++] = rightRXSample >> 8;
        io.output_buffer[m_idx++] = rightRXSample;
        io.output_buffer[m_idx++] = leftTXSample  >> 8;
        io.output_buffer[m_idx++] = leftTXSample;
        io.output_buffer[m_idx++] = rightTXSample >> 8;
        io.output_buffer[m_idx++] = rightTXSample;
		
		if (m_idx == IO_BUFFER_SIZE) {

			writeControlBytes();
			m_idx = IO_HEADER_SIZE;
		}
    }
}

void DataEngine::processOutputBuffer(CPX* buffer) {

	qint16 leftRXSample;
    qint16 rightRXSample;
    qint16 leftTXSample;
    qint16 rightTXSample;

	// process the output
	for (int j = 0; j < BUFFER_SIZE; j += io.outputMultiplier) {

		leftRXSample  = (qint16)(buffer[j].re * 32767.0f);
		rightRXSample = leftRXSample;//(qint16)(buffer[j].im * 32767.0f);

		leftTXSample = 0;
        rightTXSample = 0;

		io.output_buffer[m_idx++] = leftRXSample  >> 8;
        io.output_buffer[m_idx++] = leftRXSample;
        io.output_buffer[m_idx++] = rightRXSample >> 8;
        io.output_buffer[m_idx++] = rightRXSample;
        io.output_buffer[m_idx++] = leftTXSample  >> 8;
        io.output_buffer[m_idx++] = leftTXSample;
        io.output_buffer[m_idx++] = rightTXSample >> 8;
        io.output_buffer[m_idx++] = rightTXSample;
		
		if (m_idx == IO_BUFFER_SIZE) {

			writeControlBytes();
			m_idx = IO_HEADER_SIZE;
		}
	}
}

void DataEngine::writeControlBytes() {

	io.output_buffer[0] = SYNC;
    io.output_buffer[1] = SYNC;
    io.output_buffer[2] = SYNC;
	
	if (m_configure > 0) {

		io.output_buffer[3] = io.control_out[0];
        io.output_buffer[4] = io.control_out[1];
        io.output_buffer[5] = io.control_out[2];
        io.output_buffer[6] = io.control_out[3];
        io.output_buffer[7] = io.control_out[4];

		if (m_configure <= io.receivers) {

			int rx = io.receivers - m_configure;
			//DATA_ENGINE_DEBUG << "initializing frequency for rx:" << rx;
			io.mutex.lock();
			io.output_buffer[3] = io.control_out[0] | ((rx + 2) << 1);
			io.output_buffer[4] = rxList[rx]->getFrequency() >> 24;
			io.output_buffer[5] = rxList[rx]->getFrequency() >> 16;
			io.output_buffer[6] = rxList[rx]->getFrequency() >> 8;
			io.output_buffer[7] = rxList[rx]->getFrequency();
			io.mutex.unlock();
		}
		m_configure--;
	}
	else if (io.rx_freq_change < 0) {
	
		io.output_buffer[3] = io.control_out[0];
		io.output_buffer[4] = io.control_out[1];
        io.output_buffer[5] = io.control_out[2];
        io.output_buffer[6] = io.control_out[3];
        io.output_buffer[7] = io.control_out[4];
    }
	else {

		//DATA_ENGINE_DEBUG << "changing frequency for rx:" << io.rx_freq_change;
        io.mutex.lock();
		io.output_buffer[3] = io.control_out[0] | ((io.rx_freq_change + 2) << 1);
        io.output_buffer[4] = rxList[io.rx_freq_change]->getFrequency() >> 24;
        io.output_buffer[5] = rxList[io.rx_freq_change]->getFrequency() >> 16;
		io.output_buffer[6] = rxList[io.rx_freq_change]->getFrequency() >> 8;
        io.output_buffer[7] = rxList[io.rx_freq_change]->getFrequency();
		io.mutex.unlock();

        io.rx_freq_change = -1;
    }
	/*else if (m_frequencyChange) {
	
		io.mutex.lock();
		io.output_buffer[3] = io.control_out[0] | ((m_RxFrequencyChange + 2) << 1);
        io.output_buffer[4] = rxList[m_RxFrequencyChange]->frequency >> 24;
        io.output_buffer[5] = rxList[m_RxFrequencyChange]->frequency >> 16;
		io.output_buffer[6] = rxList[m_RxFrequencyChange]->frequency >> 8;
        io.output_buffer[7] = rxList[m_RxFrequencyChange]->frequency;
		io.mutex.unlock();

        m_frequencyChange = false;
    }
	else {

        io.output_buffer[3] = io.control_out[0];
		io.output_buffer[4] = io.control_out[1];
        io.output_buffer[5] = io.control_out[2];
        io.output_buffer[6] = io.control_out[3];
        io.output_buffer[7] = io.control_out[4];

		m_frequencyChange = true;
    }*/

	switch (m_hwInterface) {

		case QSDR::Metis:
		case QSDR::Hermes:

			io.audioDatagram.resize(IO_BUFFER_SIZE);
			//io.audioDatagram = QByteArray(reinterpret_cast<const char*>(&io.output_buffer), sizeof(io.output_buffer));
			io.audioDatagram = QByteArray::fromRawData((const char *)&io.output_buffer, IO_BUFFER_SIZE);
			
			//m_audioProcessor->deviceWriteBuffer();
			emit audioDataReady();
			break;
	}
	
	/*if (m_frequencyChange) {
		
		m_RxFrequencyChange++;
		if (m_RxFrequencyChange == io.receivers)
			m_RxFrequencyChange = 0;
	}*/
}

void DataEngine::dttspProcessing() {

	for (int r = 0; r < io.receivers; r++) {
		
		if (rxList.at(r)->getConnectedStatus()) {

			io.mutex.lock();

			dttSPList.at(r)->dttspAudioCallback(
								rxList.at(r)->in, &rxList.at(r)->in[BUFFER_SIZE],
								rxList.at(r)->out, &rxList.at(r)->out[BUFFER_SIZE], 
								BUFFER_SIZE, 0);

			dttSPList.at(r)->dttspProcessPanadapter(0, rxList.at(r)->spectrum);

			//if (r == io.currentReceiver)
			//	dttSPList.at(r)->dttspProcessSpectrum(0, rxList.at(r)->postSpectrum);
			
			m_spectrumCounter++;
			m_specCntList[r] += 1;
			if (m_specCntList.at(r) == m_framesCnt) {

				settings->setSpectrumBuffer(r, rxList.at(r)->spectrum);
				
				//if (r == io.currentReceiver)
				//	settings->setPostSpectrumBuffer(r, rxList.at(r)->postSpectrum);

				m_spectrumCounter = 0;
				m_specCntList[r] = 0;
			}
			
			io.mutex.unlock();

			if (r == io.currentReceiver) {

				m_sMeterValue = dttSPList.at(r)->dttspCalculateRXMeter(0, 0, m_meterType);
				settings->setSMeterValue(r, m_sMeterValue);
				//DATA_ENGINE_DEBUG << "S-Meter value:" << m_sMeterValue;

				processOutputBuffer(rxList.at(r)->out, &rxList.at(r)->out[BUFFER_SIZE]);
				//processOutputBuffer(rxList.at(r)->in, &rxList.at(r)->in[BUFFER_SIZE]);
			}
		}
	}
}

void DataEngine::qtdspProcessing() {

	for (int r = 0; r < io.receivers; r++) {
		
		if (rxList.at(r)->getConnectedStatus()) {

			io.mutex.lock();

			// do the funky DSP
			qtdspList.at(r)->processDSP(rxList.at(r)->inBuf, rxList.at(r)->outBuf, BUFFER_SIZE);

			// spectrum
			qtdspList.at(r)->getSpectrum(rxList.at(r)->spectrum);

			m_spectrumCounter++;
			m_specCntList[r] += 1;
			if (m_specCntList.at(r) == m_framesCnt) {

				settings->setSpectrumBuffer(r, rxList.at(r)->spectrum);
				
				//if (r == io.currentReceiver)
				//	settings->setPostSpectrumBuffer(r, rxList.at(r)->postSpectrum);

				m_spectrumCounter = 0;
				m_specCntList[r] = 0;
			}
			
			io.mutex.unlock();

			if (r == io.currentReceiver) {

				//m_sMeterValue = dttSPList.at(r)->dttspCalculateRXMeter(0, 0, m_meterType);
				//settings->setSMeterValue(r, m_sMeterValue);
				//DATA_ENGINE_DEBUG << "S-Meter value:" << m_sMeterValue;

				processOutputBuffer(rxList.at(r)->outBuf);
				//processOutputBuffer(rxList.at(r)->inBuf);
			}
		}
	}
}

//void DataEngine::dspProcessing(int r) {
//
//	//for (int r = 0; r < io.receivers; r++) {
//		
//		if (rxList.at(r)->getConnectedStatus()) {
//
//			dttSPList.at(r)->dttspAudioCallback(
//				rxList.at(r)->in, &rxList.at(r)->in[BUFFER_SIZE],
//				rxList.at(r)->out, &rxList.at(r)->out[BUFFER_SIZE], BUFFER_SIZE, 0);
//
//			if (r == io.currentReceiver) {
//
//				dttSPList[r]->dttspProcessPanadapter(0, rxList.at(r)->temp);
//				for (int i = 0; i < 4 * BUFFER_SIZE; i++)
//					rxList.at(r)->spectrum[i] = rxList.at(r)->temp[i] * 0.1 + rxList.at(r)->spectrum[i] * 0.9;
//			
//				settings->setSpectrumBuffer(r, rxList.at(r)->spectrum);
//			}
//
//			if (r == io.currentReceiver)
//				processOutputBuffer(rxList.at(r)->out, &rxList.at(r)->out[BUFFER_SIZE]);
//		}
//	//}
//}

//void DataEngine::dttspReadyAt(int r) {
//
//	if (r == io.currentReceiver) {
//		
//		processOutputBuffer(rxList.at(r)->out, &rxList.at(r)->out[BUFFER_SIZE]);
//		//emit spectrumBufferChanged(rxList.at(r)->spectrum);
//		//settings->setSpectrumBuffer(rxList.at(r)->spectrum);
//		settings->setSpectrumBuffer(r, rxList.at(r)->spectrum);
//	}
//}

void DataEngine::dttSPDspProcessing(int rx) {

	//DATA_ENGINE_DEBUG << "working rx" << rx;
	/*m_dttsp->dttspAudioCallback(
			rxList[rx]->in, &rxList[rx]->in[BUFFER_SIZE],
			io.out_buffer, &io.out_buffer[BUFFER_SIZE], BUFFER_SIZE, 0);*/

	/*dttSPList[rx]->dttspAudioCallback(
			rxList[rx]->in, &rxList[rx]->in[BUFFER_SIZE],
			io.out_buffer, &io.out_buffer[BUFFER_SIZE], BUFFER_SIZE, 0);*/

	//if (rx == io.currentReceiver)
		dttSPList[rx]->dttspAudioCallback(
			rxList[rx]->in, &rxList[rx]->in[BUFFER_SIZE],
			rxList[rx]->out, &rxList[rx]->out[BUFFER_SIZE], BUFFER_SIZE, 0);

	// process spectrum data (pre filter)
	//io.mutex.lock();
	//if (rx == io.currentReceiver)
		//dttSPList[rx]->dttspProcessPanadapter(0, rxList[rx]->spectrum);
		dttSPList[rx]->dttspProcessPanadapter(0, rxList[rx]->temp);
	//io.mutex.unlock();
	
	// process spectrum data (post filter)
	/*io.mutex.lock();
		dttSPList[rx]->dttspProcessSpectrum(0, rxList[rx]->spectrum);
	io.mutex.unlock();*/

	/*if (rx == io.currentReceiver) {
		m_spectrumCounter++;
		if (m_spectrumCounter == m_framesCnt) {
		
			if (settings->getSpectrumAveraging())
				m_spectrumAveragingFilter->doAveraging(rxList[rx]->spectrum);
			else
				//emit spectrumBufferChanged(rxList[rx]->spectrum);
				settings->setSpectrumBuffer(rxList.at(r)->spectrum);

			m_spectrumCounter = 0;
		}
	}*/
	//if (rx == io.currentReceiver) {
		for (int i = 0; i < 4 * BUFFER_SIZE; i++) {
	
			rxList[rx]->spectrum[i] = rxList[rx]->temp[i] * 0.1 + rxList[rx]->spectrum[i] * 0.9;
		}
		//emit spectrumBufferChanged(rxList[rx]->spectrum);
		//settings->setSpectrumBuffer(rxList.at(rx)->spectrum);
		settings->setSpectrumBuffer(rx, rxList.at(rx)->spectrum);
	//}

	if (rx == io.currentReceiver)
		//processOutputBuffer(io.out_buffer, &io.out_buffer[BUFFER_SIZE]);
		processOutputBuffer(rxList[rx]->out, &rxList[rx]->out[BUFFER_SIZE]);
}
   
/*!
	\brief event created when spectrum averaging filter is active.
*/
void DataEngine::spectrumBufferEvent(
		int rx,
		const float *buffer				/*!<[in] buffer from spectrum averaging filter. */
) {
	//emit spectrumBufferChanged(buffer);
	//settings->setSpectrumBuffer(buffer);
	settings->setSpectrumBuffer(rx, buffer);
}
 
 
//*****************************************************************************
//

void DataEngine::setSystemState(
	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	Q_UNUSED (sender)
	Q_UNUSED (err)

	io.mutex.lock();
	if (m_hwInterface != hwmode)
		m_hwInterface = hwmode;
		
	if (m_serverMode != mode)
		m_serverMode = mode;
		
	if (m_dataEngineState != state)
		m_dataEngineState = state;

	io.mutex.unlock();
}

float DataEngine::getFilterSizeCalibrationOffset() {

    //int size=1024; // dspBufferSize
    float i = log10((qreal) BUFFER_SIZE);
    return 3.0f*(11.0f - i);
}

void DataEngine::sendMessage(QString msg) {

	msg.prepend("[data engine]: ");

	emit messageEvent(msg);
}

void DataEngine::sendReceiverMessage(QString msg) {

	emit messageEvent(msg);
}

void DataEngine::newMessage(QString msg) {

	emit messageEvent(msg);
}

void DataEngine::searchHpsdrNetworkDevices() {

	if (!m_hpsdrIO) createHpsdrIO();

	// HPSDR network IO thread
	if (!startHpsdrIO(QThread::NormalPriority)) {

		DATA_ENGINE_DEBUG << "HPSDR network discovery thread could not be started.";
		sendMessage("HPSDR network discover thread could not be started.");
		return;
	}

	io.networkIOMutex.lock();
	io.devicefound.wait(&io.networkIOMutex);

	//m_hpsdrIO->findHPSDRDevices();

	// stop the discovery thread
	io.networkIOMutex.unlock();
	stopHpsdrIO();
}

void DataEngine::setHPSDRDeviceNumber(int value) {

	m_hpsdrDevices = value;
}

void DataEngine::rxListChanged(QList<HPSDRReceiver *> list) {

	io.mutex.lock();
		rxList = list;
	io.mutex.unlock();
}

void DataEngine::setCurrentReceiver(int rx) {

	io.mutex.lock();
		currentRx = rx;
	io.mutex.unlock();
}

void DataEngine::setFramesPerSecond(QObject *sender, int value) {

	Q_UNUSED(sender)

	io.mutex.lock();
		switch (io.samplerate) {

			case 48000:
				m_framesCnt = (int)(48/value);
				break;

			case 96000:
				m_framesCnt = (int)(96/value);
				break;

			case 192000:
				m_framesCnt = (int)(192/value);
				break;
		}

		//m_framesCnt = (int)(192/value);
		m_spectrumCounter = 0;
		for (int i = 0; i < io.receivers; i++)
			m_specCntList[i] = 0;

	io.mutex.unlock();
}

void DataEngine::setSampleRate(QObject *sender, int value) {

	Q_UNUSED(sender)

	io.mutex.lock();
	switch (value) {
	
		case 48000:
			io.samplerate = value;
			io.speed = 0;
			io.outputMultiplier = 1;

			m_framesCnt = (int)(48/m_framesPerSecond);
			break;
			
		case 96000:
			io.samplerate = value;
			io.speed = 1;
			io.outputMultiplier = 2;

			m_framesCnt = (int)(96/m_framesPerSecond);
			break;
			
		case 192000:
			io.samplerate = value;
			io.speed = 2;
			io.outputMultiplier = 4;

			m_framesCnt = (int)(192/m_framesPerSecond);
			break;
			
		default:
			sendMessage("invalid sample rate (48000, 96000, 192000)!\n");
			exit(1);
			break;
	}
		
	io.control_out[1] &= 0xFC;
	io.control_out[1] |= io.speed;

	if ((m_serverMode == QSDR::DttSP || 
		 m_serverMode == QSDR::ChirpWSPR) && 
		 m_dataEngineState == QSDR::DataEngineUp)
	{	
		if (dttSPList[io.currentReceiver]->getDttSPStatus()) {
			
			dttSPList[io.currentReceiver]->dttspSetSampleRate((double)io.samplerate);
			dttSPList[io.currentReceiver]->dttspSetRXOsc(0, 0, 0.0);
			dttSPList[io.currentReceiver]->dttspSetRXOutputGain(0, 0, settings->getMainVolume(io.currentReceiver));
		
			setDttspDspMode(this, io.currentReceiver, rxList.at(io.currentReceiver)->getDSPMode());
			setDttspAgcMode(this, io.currentReceiver, rxList.at(io.currentReceiver)->getAGCMode());
			setDttspFrequency(this, true, io.currentReceiver, rxList.at(io.currentReceiver)->getFrequency());

			m_spectrumCounter = 0;
			for (int i = 0; i < io.receivers; i++)
				m_specCntList[i] = 0;
		}
	}
	io.mutex.unlock();

	emit outMultiplierEvent(io.outputMultiplier);
}

void DataEngine::setPreamp(QObject *sender, int value) {

	Q_UNUSED(sender)

	io.mutex.lock();
	io.control_out[3] = io.control_out[3] & 0xFB;
	io.control_out[3] = io.control_out[3] | ( value << 2 );
	io.mutex.unlock();
}

void DataEngine::setDither(QObject *sender, int value) {

	Q_UNUSED(sender)

	io.mutex.lock();
	io.control_out[3] = io.control_out[3] & 0xF7;
	io.control_out[3] = io.control_out[3] | (value << 3);
	io.mutex.unlock();
}

void DataEngine::setRandom(QObject *sender, int value) {

	Q_UNUSED(sender)

	io.mutex.lock();
	io.control_out[3] = io.control_out[3] & 0xEF;
	io.control_out[3] = io.control_out[3] | (value << 4);
	io.mutex.unlock();
}

void DataEngine::set10MhzSource(QObject *sender, int source) {

	Q_UNUSED(sender)

	io.mutex.lock();
	io.control_out[1] = io.control_out[1] & 0xF3;
	io.control_out[1] = io.control_out[1] | (source << 2);
	io.mutex.unlock();
}

void DataEngine::set122_88MhzSource(QObject *sender, int source) {

	Q_UNUSED(sender)

	io.mutex.lock();
	io.control_out[1] = io.control_out[1] & 0xEF;
	io.control_out[1] = io.control_out[1] | (source << 4);
	io.mutex.unlock();
}

void DataEngine::setMicSource(QObject *sender, int source) {

	Q_UNUSED(sender)

	io.mutex.lock();
	io.control_out[1] = io.control_out[1] & 0x7F;
	io.control_out[1] = io.control_out[1] | (source << 7);
	io.mutex.unlock();
}

void DataEngine::setMercuryClass(QObject *sender, int value) {

	Q_UNUSED(sender)

	io.mutex.lock();
	io.control_out[2] = io.control_out[2] & 0xFE;
	io.control_out[2] = io.control_out[2] | value;
	io.mutex.unlock();
}

void DataEngine::setMercuryTiming(QObject *sender, int value) {

	Q_UNUSED(sender)

	io.timing = value;
}

void DataEngine::setRcveIQSignal(QObject *sender, int value) {

	emit rcveIQEvent(sender, value);
}

void DataEngine::setPenelopeVersion(QObject *sender, int version) {

	emit penelopeVersionInfoEvent(sender, version);
}

void DataEngine::setHwIOVersion(QObject *sender, int version) {

	emit hwIOVersionInfoEvent(sender, version);
}

void DataEngine::setNumberOfRx(QObject *sender, int value) {

	Q_UNUSED(sender)

	if (io.receivers == value) return;
	
	io.receivers = value;
		
	io.control_out[4] &= 0xc7;
	io.control_out[4] |= (value - 1) << 3;

	m_message = "number of receivers set to %1.";
	sendMessage(m_message.arg(QString::number(value)));
}

void DataEngine::setTimeStamp(QObject *sender, bool value) {

	Q_UNUSED(sender)

	if (io.timeStamp == value) return;

	io.timeStamp = value;

	//io.control_out[4] &= 0xc7;
	io.control_out[4] |= value << 6;

	if (value)
		m_message = "set time stamp on.";
	else
		m_message = "set time stamp off.";
	sendMessage(m_message);
}

void DataEngine::setRxSocketState(int rx, const char* prop, QString str) {

	rxList[rx]->setProperty(prop, str);
	settings->setRxList(rxList);
}

void DataEngine::setRxPeerAddress(int rx, QHostAddress address) {

	rxList[rx]->setPeerAddress(address);
	settings->setRxList(rxList);
}

void DataEngine::setRx(int rx) {

	io.mutex.lock();
	rxList[rx]->setReceiver(rx);
	settings->setRxList(rxList);
	io.mutex.unlock();
}

void DataEngine::setRxClient(int rx, int client) {

	io.mutex.lock();
	rxList[rx]->setClient(client);
	settings->setRxList(rxList);
	io.mutex.unlock();
}

void DataEngine::setClientConnected(QObject* sender, int rx) {

	Q_UNUSED(sender)

	if (!io.clientList.contains(rx)) {

		io.clientList.append(rx);
		io.audio_rx = rx;

		m_AudioRcvrThread->quit();
		m_AudioRcvrThread->wait();
		m_AudioRcvrThread->start();
	}
	else {

		io.sendIQ_toggle = true;
		io.rcveIQ_toggle = false;
		m_AudioRcvrThread->start();
	}
}

void DataEngine::setClientConnected(bool value) {

	clientConnected = value;
}

void DataEngine::setClientDisconnected(int client) {

	Q_UNUSED(client)
	/*if (clientConnected) {

		m_AudioRcvrThread->quit();
		m_AudioRcvrThread->wait();
		if (!m_AudioRcvrThread->isRunning())
			DATA_ENGINE_DEBUG << "audio receiver thread stopped.";

		clientConnected = false;		
	}
	sync_toggle = true;
	adc_toggle = false;*/
}

void DataEngine::setAudioProcessorRunning(bool value) {

	m_audioProcessorRunning = value;
}

void DataEngine::setAudioReceiver(QObject *sender, int rx) {

	Q_UNUSED(sender)

	io.mutex.lock();
	emit audioRxEvent(rx);
	io.mutex.unlock();
}

void DataEngine::setIQPort(int rx, int port) {

	io.mutex.lock();
	rxList[rx]->setIQPort(port);
	settings->setRxList(rxList);
	io.mutex.unlock();
}

void DataEngine::setRxConnectedStatus(QObject* sender, int rx, bool value) {

	Q_UNUSED(sender)

	io.mutex.lock();
	rxList[rx]->setConnectedStatus(value);
	settings->setRxList(rxList);
	io.mutex.unlock();
}

void DataEngine::setFrequency(QObject* sender, bool value, int rx, long frequency) {

	Q_UNUSED(sender)
	//Q_UNUSED(value)

	rxList[rx]->frequency_changed = value;
	rxList[rx]->setFrequency(frequency);
	io.rx_freq_change = rx;
}

void DataEngine::setRXFilter(QObject *sender, int rx, qreal low, qreal high) {

	Q_UNUSED(sender)

	io.mutex.lock();
	qtdspList[rx]->filter->setFilter((float)low, (float)high);
	io.mutex.unlock();
}

void DataEngine::setAgcMode(QObject *sender, int rx, AGCMode mode) {

	Q_UNUSED(sender)

	rxList[rx]->setAGCMode(this, rx, mode);
	qtdspList[rx]->setAGCMode(mode);
}

void DataEngine::setAgcGain(QObject *sender, int rx, int value) {

	Q_UNUSED(sender)

	rxList[rx]->setAGCGain(this, rx, value);
	//qtdspList[rx]->
}

void DataEngine::setWbSpectrumAveraging(bool value) {

	wbMutex.lock();
	m_wbSpectrumAveraging = value;
	wbMutex.unlock();
}

 
//**************************************************
// DttSP control
void DataEngine::setDttSPMainVolume(QObject *sender, int rx, float value) {

	Q_UNUSED(sender)

	switch (m_serverMode) {

		//case QSDR::InternalDSP:
		case QSDR::DttSP:
		case QSDR::ChirpWSPR:

			if (dttSPList[rx])
				dttSPList[rx]->dttspSetRXOutputGain(0, 0, value);
			break;
	}
}

void DataEngine::setDttspFrequency(QObject *sender, bool value, int rx, long frequency) {

	Q_UNUSED(sender)
	Q_UNUSED(value)

	rxList[rx]->frequency_changed = true;
	rxList[rx]->setFrequency(frequency);
	io.rx_freq_change = rx;
}

void DataEngine::setDttspRXFilter(QObject *sender, int rx, qreal low, qreal high) {

	Q_UNUSED(sender)

	//settings->setRXFilter(this, rx, low, high);
	if (dttSPList[rx]->getDttSPStatus())
		dttSPList[rx]->dttspSetRXFilter(0, 0, (double) low, (double) high);
}

void DataEngine::setDttspDspMode(QObject *sender, int rx, DSPMode mode) {

	Q_UNUSED(sender)

	if (dttSPList[rx]->getDttSPStatus()) {

		dttSPList[rx]->dttspSetMode(0, 0, mode);
		setDttspRXFilter(this, rx,
			getFilterFromDSPMode(settings->getDefaultFilterList(), mode).filterLo,
			getFilterFromDSPMode(settings->getDefaultFilterList(), mode).filterHi);		
	}
}

void DataEngine::setDttspAgcMode(QObject *sender, int rx, AGCMode mode) {

	Q_UNUSED(sender)

	if (dttSPList[rx]->getDttSPStatus())
		dttSPList[rx]->dttspSetRXAGC(0, 0, mode);
}

void DataEngine::setDttspAgcGain(QObject *sender, int rx, int value) {

	Q_UNUSED(sender)

	if (dttSPList[rx]->getDttSPStatus())
		//dttSPList[rx]->dttspSetRXAGCTop(0, 0, (double) (value * 100.0));
		dttSPList[rx]->dttspSetRXAGCTop(0, 0, (double) (value * 1.0));
}

//**************************************************
// QtDSP control
void DataEngine::setQtDSPMainVolume(QObject *sender, int rx, float value) {

	//if (m_dataEngineState == QSDR::DataEngineUp)
	if (qtdspList[rx]->getQtDSPStatus())
		qtdspList[rx]->setVolume(value);
}

//***************************************************

void DataEngine::loadWavFile(const QString &fileName) {

	if (m_audioEngine->loadFile(fileName))
		m_soundFileLoaded = true;
	else
		m_soundFileLoaded = false;
}

void DataEngine::suspend() {

	m_audioEngine->suspend();
}

void DataEngine::startPlayback() {

	m_audioEngine->startPlayback();
}

void DataEngine::showSettingsDialog() {

	m_audioEngine->showSettingsDialog();
}

void DataEngine::setAudioFileFormat(QObject *sender, const QAudioFormat &format) {

	Q_UNUSED (sender)
	Q_UNUSED (format)
}

void DataEngine::setAudioFilePosition(QObject *sender, qint64 position) {

	Q_UNUSED (sender)
	Q_UNUSED (position)
}

void DataEngine::setAudioFileBuffer(QObject *sender, qint64 position, qint64 length, const QByteArray &buffer) {

	Q_UNUSED (sender)

    m_audioFileBufferPosition = position;
    m_audioFileBufferLength = length;
	m_audioFileBuffer = buffer;

	//DATA_ENGINE_DEBUG << "audio file length" << m_audioFileBufferLength;
}

void DataEngine::setAudioFileBuffer(const QList<qreal> &buffer) {

	io.inputBuffer = buffer;
	
	/*for (int i = 0; i < buffer.length(); i++) {

		DATA_ENGINE_DEBUG << "i" << i << "audioBuffer" << io.inputBuffer.at(i);
	}*/
}
 
// *********************************************************************
// Data processor

DataProcessor::DataProcessor(DataEngine *de, QSDR::_ServerMode serverMode)
	: QObject()
	, m_dataEngine(de)
	, m_serverMode(serverMode)
	, m_stopped(false)
	, m_IQSequence(0L)
	, m_socketConnected(false)
{
	m_dataEngine->syncToggle = true;
	m_dataEngine->adcToggle = false;

	m_IQDatagram.resize(0);
	
	/*m_cpxIn	= mallocCPX(2*BUFFER_SIZE);
	m_cpxOut = mallocCPX(2*BUFFER_SIZE);
		
	m_fft = new QFFT(this, 2*BUFFER_SIZE);

	memset(m_cpxIn, 0, 2*BUFFER_SIZE * sizeof(CPX));
	memset(m_cpxOut, 0, 2*BUFFER_SIZE * sizeof(CPX));*/
}

DataProcessor::~DataProcessor() {
}

void DataProcessor::stop() {

	m_stopped = true;
}

void DataProcessor::initDataProcessorSocket() {

	m_dataProcessorSocket = new QUdpSocket();

	/*m_dataProcessorSocket->bind(QHostAddress(m_settings->getHPSDRDeviceLocalAddr()),
								  23000, 
								  QUdpSocket::ReuseAddressHint | QUdpSocket::ShareAddress);

	int newBufferSize = 64 * 1024;

	if (::setsockopt(m_dataProcessorSocket->socketDescriptor(), SOL_SOCKET,
                     SO_RCVBUF, (char *)&newBufferSize, sizeof(newBufferSize)) == -1) {
						 
						 DATA_ENGINE_DEBUG << "initDataProcessorSocket error setting m_dataProcessorSocket buffer size.";
	}*/

	//m_dataProcessorSocket->setSocketOption(QAbstractSocket::LowDelayOption, 1);
	//m_dataProcessorSocket->setSocketOption(QAbstractSocket::KeepAliveOption, 1);

	CHECKED_CONNECT(
		m_dataProcessorSocket, 
		SIGNAL(error(QAbstractSocket::SocketError)), 
		this, 
		SLOT(displayDataProcessorSocketError(QAbstractSocket::SocketError)));
}

void DataProcessor::displayDataProcessorSocketError(QAbstractSocket::SocketError error) {

	DATA_PROCESSOR_DEBUG << "displayDataProcessorSocketError data processor socket error:" << error;
}

void DataProcessor::processDeviceData() {

	if (m_serverMode == QSDR::ExternalDSP) 
		initDataProcessorSocket();

	forever {

		m_dataEngine->processInputBuffer(m_dataEngine->io.iq_queue.dequeue());
		//DATA_ENGINE_DEBUG << "IQ queue length:" << m_dataEngine->io.iq_queue.count();
		//DATA_ENGINE_DEBUG << "iq_queue length:" << m_dataEngine->io.iq_queue.dequeue().length();
		
		m_mutex.lock();
		if (m_stopped) {
			m_stopped = false;
			m_mutex.unlock();
			break;
		}
		m_mutex.unlock();
	}

	if (m_serverMode == QSDR::ExternalDSP) {

		disconnect(this);
		m_dataProcessorSocket->close();
		delete m_dataProcessorSocket;
		m_dataProcessorSocket = NULL;

		m_socketConnected = false;
	}
}

void DataProcessor::processData() {

	forever {

		m_dataEngine->processFileBuffer(m_dataEngine->io.data_queue.dequeue());

		m_mutex.lock();
		if (m_stopped) {
			m_stopped = false;
			m_mutex.unlock();
			break;
		}
		m_mutex.unlock();
	}
}

void DataProcessor::externalDspProcessing(int rx) {

	// keep UDP packets < 512 bytes 
	// 8 bytes sequency number, 2 bytes offset, 2 bytes length, 500 bytes data

	if (!m_socketConnected) {

		m_dataProcessorSocket->connectToHost(m_dataEngine->rxList[rx]->getPeerAddress(), m_dataEngine->rxList[rx]->getIQPort());
		//int newBufferSize = 64 * 1024;
		int newBufferSize = 16 * 1024;

		if (::setsockopt(m_dataProcessorSocket->socketDescriptor(), SOL_SOCKET,
                     SO_RCVBUF, (char *)&newBufferSize, sizeof(newBufferSize)) == -1) {
						 
						 DATA_PROCESSOR_DEBUG << "externalDspProcessing error setting m_dataProcessorSocket buffer size.";
		}
		m_socketConnected = true;
	}
	
#ifndef __linux__
	m_sequenceHi = 0L;
#endif
	
	/*QUdpSocket socket;
	CHECKED_CONNECT(&socket, 
			SIGNAL(error(QAbstractSocket::SocketError)), 
			this, 
			SLOT(displayDataProcessorSocketError(QAbstractSocket::SocketError)));*/

	m_offset = 0;
	//m_IQDatagram.append(reinterpret_cast<const char*>(&m_dataEngine->rxList[rx]->input_buffer), sizeof(m_dataEngine->rxList[rx]->input_buffer));
	m_IQDatagram.append(
		reinterpret_cast<const char*>(&m_dataEngine->rxList[rx]->in_cpx->re), 
		sizeof(m_dataEngine->rxList[rx]->in_cpx->re));

	m_IQDatagram.append(
		reinterpret_cast<const char*>(&m_dataEngine->rxList[rx]->in_cpx->im), 
		sizeof(m_dataEngine->rxList[rx]->in_cpx->im));
		
	while (m_offset < m_IQDatagram.size()) {
	
		m_length = m_IQDatagram.size() - m_offset;
		
		if (m_length > 500)  
			m_length = 500;

		QByteArray datagram;
		datagram += QByteArray(reinterpret_cast<const char*>(&m_IQSequence), sizeof(m_IQSequence));
		datagram += QByteArray(reinterpret_cast<const char*>(&m_sequenceHi), sizeof(m_sequenceHi));
		datagram += QByteArray(reinterpret_cast<const char*>(&m_offset), sizeof(m_offset));
		datagram += QByteArray(reinterpret_cast<const char*>(&m_length), sizeof(m_length));
		datagram += m_IQDatagram.mid(m_offset, m_length);
		
		if (m_dataProcessorSocket->write(datagram) < 0)
		/*if (m_dataProcessorSocket->writeDatagram(datagram,
											m_dataEngine->rxList[rx]->getPeerAddress(),
											m_dataEngine->rxList[rx]->getIQPort()) < 0)*/
		//if (socket.writeDatagram(datagram,
		//						 m_dataEngine->rxList[rx]->getPeerAddress(),
		//						 m_dataEngine->rxList[rx]->getIQPort()) < 0)
		{
			if (!m_dataEngine->io.sendIQ_toggle) {  // toggles the sendIQ signal

				m_dataEngine->settings->setSendIQ(2);
				m_dataEngine->io.sendIQ_toggle = true;
			}

			DATA_ENGINE_DEBUG	<< "externalDspProcessing error sending data to client:" 
								<< m_dataProcessorSocket->errorString();
		}
		else {
		
			//socket.flush();
			if (m_dataEngine->io.sendIQ_toggle) { // toggles the sendIQ signal
				
				m_dataEngine->settings->setSendIQ(1);
				m_dataEngine->io.sendIQ_toggle = false;
			}
		}
		m_offset += m_length;
	}
	m_IQDatagram.resize(0);
	m_IQSequence++;
}

void DataProcessor::externalDspProcessingBig(int rx) {
	
	m_IQDatagram.append(
		reinterpret_cast<const char*>(&m_dataEngine->rxList[rx]->in), sizeof(m_dataEngine->rxList[rx]->in));
		
	if (m_dataProcessorSocket->writeDatagram(m_IQDatagram.data(), 
										m_IQDatagram.size(), 
										m_dataEngine->rxList[rx]->getPeerAddress(), 
										m_dataEngine->rxList[rx]->getIQPort()) < 0)
										
	{		
		if (!m_dataEngine->io.sendIQ_toggle) {  // toggles the sendIQ signal

			m_dataEngine->settings->setSendIQ(2);
			m_dataEngine->io.sendIQ_toggle = true;
		}

		DATA_PROCESSOR_DEBUG << "error sending data to client:" << m_dataProcessorSocket->errorString();
	}
	else {
	
		m_dataProcessorSocket->flush();
		if (m_dataEngine->io.sendIQ_toggle) { // toggles the sendIQ signal
				
			m_dataEngine->settings->setSendIQ(1);
			m_dataEngine->io.sendIQ_toggle = false;
		}
	}
	m_IQDatagram.resize(0);
}

 
// *********************************************************************
// Wide band data processor
 
WideBandDataProcessor::WideBandDataProcessor(DataEngine *de, QSDR::_ServerMode serverMode)
	: QObject()
	, m_dataEngine(de)
	, m_serverMode(serverMode)
	, m_stopped(false)
{
	m_WBDatagram.resize(0);
}

WideBandDataProcessor::~WideBandDataProcessor() {

}

void WideBandDataProcessor::stop() {

	//mutex.lock();
	m_stopped = true;
	//mutex.unlock();
}

void WideBandDataProcessor::processWideBandData() {

	forever {

		m_dataEngine->processWideBandInputBuffer(m_dataEngine->io.wb_queue.dequeue());
		
		m_mutex.lock();
		if (m_stopped) {
			m_stopped = false;
			m_mutex.unlock();
			break;
		}
		m_mutex.unlock();
	}
}

 
// *********************************************************************
// Audio processor

AudioProcessor::AudioProcessor(DataEngine *de)
	: QObject()
	, m_dataEngine(de)
	, m_stopped(false)
	, m_sendSequence(0L)
	, m_oldSendSequence(0L)
	, m_setNetworkDeviceHeader(true)
{
	m_deviceSendDataSignature.resize(4);
	m_deviceSendDataSignature[0] = (char)0xEF;
	m_deviceSendDataSignature[1] = (char)0xFE;
	m_deviceSendDataSignature[2] = (char)0x01;
	m_deviceSendDataSignature[3] = (char)0x02;
}

AudioProcessor::~AudioProcessor() {
}

void AudioProcessor::stop() {

	m_stopped = true;
}

void AudioProcessor::initAudioProcessorSocket() {

	m_audioProcessorSocket = new QUdpSocket();
	m_audioProcessorSocket->connectToHost(m_dataEngine->io.metisIPAddress, METIS_PORT);

	//m_audioProcessorSocket->setSocketOption(QAbstractSocket::LowDelayOption, 1);
	//m_audioProcessorSocket->setSocketOption(QAbstractSocket::KeepAliveOption, 1);

	CHECKED_CONNECT(
		m_audioProcessorSocket, 
		SIGNAL(error(QAbstractSocket::SocketError)), 
		this, 
		SLOT(displayAudioProcessorSocketError(QAbstractSocket::SocketError)));

	m_dataEngine->setAudioProcessorRunning(true);
}

void AudioProcessor::audioReceiverChanged(int rx) {

	m_audioRx = rx;
}

void AudioProcessor::clientConnected(int rx) {

	m_mutex.lock();
	m_client = rx;
	m_mutex.unlock();
}

void AudioProcessor::displayAudioProcessorSocketError(QAbstractSocket::SocketError error) {

	AUDIO_PROCESSOR << "audio processor socket error:" << error;
}

void AudioProcessor::processAudioData() {

	initAudioProcessorSocket();

	forever {
		
		//DATA_ENGINE_DEBUG << "audioQueue length = " << m_dataEngine->io.au_queue.length();
		m_outBuffer = m_dataEngine->io.au_queue.dequeue();
		
		m_mutex.lock();
		if (m_stopped) {
			m_stopped = false;
			m_mutex.unlock();
			break;
		}
		m_mutex.unlock();

		memcpy(
			(float *) &m_left[0], 
			(float *) m_outBuffer.left(IO_AUDIOBUFFER_SIZE/2).data(), 
			BUFFER_SIZE * sizeof(float));

		memcpy(
			(float *) &m_right[0], 
			(float *) m_outBuffer.right(IO_AUDIOBUFFER_SIZE/2).data(), 
			BUFFER_SIZE * sizeof(float));
		
		m_dataEngine->processOutputBuffer(m_left, m_right);
	}


	disconnect(this);
	m_audioProcessorSocket->close();
	delete m_audioProcessorSocket;
	m_audioProcessorSocket = NULL;
}

void AudioProcessor::deviceWriteBuffer() {

	if (m_setNetworkDeviceHeader) {

		m_outDatagram.resize(0);
        m_outDatagram += m_deviceSendDataSignature;

		QByteArray seq(reinterpret_cast<const char*>(&m_sendSequence), sizeof(m_sendSequence));

		m_outDatagram += seq;
		m_outDatagram += m_dataEngine->io.audioDatagram;

		m_sendSequence++;
        m_setNetworkDeviceHeader = false;
    } 
	else {

		m_outDatagram += m_dataEngine->io.audioDatagram;

		//if (m_audioProcessorSocket->writeDatagram(outDatagram.data(), outDatagram.size(), m_dataEngine->metisIPAddress, m_dataEngine->metisDataPort) < 0)
		if (m_audioProcessorSocket->write(m_outDatagram) < 0)
			AUDIO_PROCESSOR << "error sending data to Metis:" << m_audioProcessorSocket->errorString();

		if (m_sendSequence != m_oldSendSequence + 1)
			AUDIO_PROCESSOR << "output sequence error: old =" << m_oldSendSequence << "; new =" << m_sendSequence; 

		m_oldSendSequence = m_sendSequence;
		m_setNetworkDeviceHeader = true;
    }
}
 