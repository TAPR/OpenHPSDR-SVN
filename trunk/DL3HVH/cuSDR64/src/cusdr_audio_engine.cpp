/**
* @file cusdr_audio_engine.cpp
* @brief cuSDR audio engine
* @author adaptation for cuSDR by Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-04-02
*/

/****************************************************************************
**
** Copyright (C) 2011 Nokia Corporation and/or its subsidiary(-ies).
** All rights reserved.
** Contact: Nokia Corporation (qt-info@nokia.com)
**
** This file is part of the examples of the Qt Toolkit.
**
** $QT_BEGIN_LICENSE:BSD$
** You may use this file under the terms of the BSD license as follows:
**
** "Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions are
** met:
**   * Redistributions of source code must retain the above copyright
**     notice, this list of conditions and the following disclaimer.
**   * Redistributions in binary form must reproduce the above copyright
**     notice, this list of conditions and the following disclaimer in
**     the documentation and/or other materials provided with the
**     distribution.
**   * Neither the name of Nokia Corporation and its Subsidiary(-ies) nor
**     the names of its contributors may be used to endorse or promote
**     products derived from this software without specific prior written
**     permission.
**
** THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
** "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
** LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
** A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
** OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
** SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
** LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
** OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE."
** $QT_END_LICENSE$
**
****************************************************************************/
//#define LOG_AUDIO_ENGINE

#include "cusdr_audio_engine.h"
#include "cusdr_audio_settingsdialog.h"
#include "cusdr_audio_utils.h"
#include "cusdr_settings.h"

//#include <math.h>

#include <QCoreApplication>
#include <QMetaObject>
#include <QSet>
#include <QtMultimedia/QAudioInput>
#include <QtMultimedia/QAudioOutput>
#include <QDebug>
#include <QThread>
#include <QFile>
#include <QTimer>


struct SWavHeader {
	
	char chunkID[4];
	unsigned int chunkSize;
	char format[4];
	
	unsigned char subchunk1id[4];
	unsigned int subchunk1size;
	unsigned short audioFormat;
	unsigned short nofChannels;
	unsigned int sampleRate;
	unsigned int byteRate;
	unsigned short blockAlign;
	unsigned short bitsPerSample;
	
	unsigned char subchunk2id[4];
	unsigned int subchunk2size;
};


//-----------------------------------------------------------------------------
// Constants
//-----------------------------------------------------------------------------

//const qint64 BufferDurationUs       = 10 * 1000000;
const int    NotifyIntervalMs       = 1;

// Size of the level calculation window in microseconds
const int    LevelWindowUs          = 0.1 * 1000000;

// Waveform window size in microseconds
//const qint64 WaveformWindowDuration = 500 * 1000;

//-----------------------------------------------------------------------------
// Helper functions
//-----------------------------------------------------------------------------

QDebug& operator<<(QDebug &debug, const QAudioFormat &format) {

    debug << format.frequency() << "Hz"
          << format.channels() << "channels";
    return debug;
}

//-----------------------------------------------------------------------------
// Constructor and destructor
//-----------------------------------------------------------------------------

AudioEngine::AudioEngine(QWidget *parent)
	: QWidget(parent)
	, m_settings(Settings::instance())
    , m_mode(QAudio::AudioInput)
    , m_state(QAudio::StoppedState)
    , m_generateTone(false)
	, m_generateLocalChirp(false)
    , m_file(0)
    , m_analysisFile(0)
    , m_availableAudioInputDevices(
			QAudioDeviceInfo::availableDevices(QAudio::AudioInput))
    , m_audioInputDevice(QAudioDeviceInfo::defaultInputDevice())
    , m_audioInput(0)
    , m_audioInputIODevice(0)
    , m_recordPosition(0)
    , m_availableAudioOutputDevices(
			QAudioDeviceInfo::availableDevices(QAudio::AudioOutput))
    , m_audioOutputDevice(QAudioDeviceInfo::defaultOutputDevice())
    , m_audioOutput(0)
    , m_playPosition(0)
    , m_bufferPosition(0)
    , m_bufferLength(0)
    , m_dataLength(0)
    , m_levelBufferLength(0)
    , m_rmsLevel(0.0)
    , m_peakLevel(0.0)
    , m_spectrumBufferLength(0)
    , m_spectrumAnalyser()
    , m_spectrumPosition(0)
    , m_count(0)
	, m_settingsDialog(new SettingsDialog(
			availableAudioInputDevices(),
            availableAudioOutputDevices(),
            this))
	, m_lowerChirpFreq(m_settings->getLowerChirpFreq())
	, m_upperChirpFreq(m_settings->getUpperChirpFreq())
	, m_chirpAmplitude(m_settings->getChirpAmplitude())
	, m_chirpBufferDurationUs(m_settings->getChirpBufferDurationUs())
	, m_chirpRepetition(1)
	, m_sampleRate(m_settings->getSampleRate())
	, m_downRate(m_settings->getChirpDownSampleRate())
	, m_audioFileBuffer(0)
{
    qRegisterMetaType<FrequencySpectrum>("FrequencySpectrum");
    qRegisterMetaType<WindowFunction>("WindowFunction");
    
	setupConnections();
    initializePCMS16LE();

	AUDIO_ENGINE_DEBUG	<< "defaultOutputDevice options:";
	AUDIO_ENGINE_DEBUG	<< "	defaultOutputDevice" << m_audioOutputDevice.defaultOutputDevice;
	AUDIO_ENGINE_DEBUG	<< "	preferredFormat" << m_audioOutputDevice.preferredFormat();
	AUDIO_ENGINE_DEBUG	<< "	supportedByteOrders" << m_audioOutputDevice.supportedByteOrders();
	AUDIO_ENGINE_DEBUG	<< "	supportedChannelCounts" << m_audioOutputDevice.supportedChannelCounts();
	AUDIO_ENGINE_DEBUG	<< "	supportedChannels" << m_audioOutputDevice.supportedChannels();
	AUDIO_ENGINE_DEBUG	<< "	supportedCodecs" << m_audioOutputDevice.supportedCodecs();
	AUDIO_ENGINE_DEBUG	<< "	supportedFrequencies" << m_audioOutputDevice.supportedFrequencies();
	AUDIO_ENGINE_DEBUG	<< "	supportedSampleRates" << m_audioOutputDevice.supportedSampleRates();
	AUDIO_ENGINE_DEBUG	<< "	supportedSampleSizes" << m_audioOutputDevice.supportedSampleSizes();
	AUDIO_ENGINE_DEBUG	<< "	supportedSampleTypes" << m_audioOutputDevice.supportedSampleTypes();


#ifdef DUMP_DATA
    createOutputDir();
#endif

#ifdef DUMP_SPECTRUM
    m_spectrumAnalyser.setOutputPath(outputPath());
#endif
}

AudioEngine::~AudioEngine() {
}

//-----------------------------------------------------------------------------
// Public functions
//-----------------------------------------------------------------------------

bool AudioEngine::loadFile(const QString &fileName) {

    reset();
    bool result = false;
    Q_ASSERT(!m_generateTone);
    Q_ASSERT(!m_file);
    Q_ASSERT(!fileName.isEmpty());
    m_file = new WavFile(this);

    if (m_file->open(fileName)) {

		QString formatStr = formatToString(m_file->fileFormat());
		AUDIO_ENGINE_DEBUG << "file audio format" << formatStr;


        if (isPCMS16LE(m_file->fileFormat())) {

            result = initializePCMS16LE();

			m_audioFileBuffer = new AudiofileBuffer();
			m_audioFileBuffer = AudiofileBuffer::loadWav(m_file->fileName());

			qint16 channels = m_audioFileBuffer->getNofChannels();

			AUDIO_ENGINE_DEBUG << "  data length:" << m_audioFileBuffer->getDataLength();
			AUDIO_ENGINE_DEBUG << "  bytes per sample:" << m_audioFileBuffer->getBytesPerSample();
			AUDIO_ENGINE_DEBUG << "  bits per sample:" << m_audioFileBuffer->getBitsPerSample();
			AUDIO_ENGINE_DEBUG << "  samples per second:" << m_audioFileBuffer->getSamplesPerSec();
			AUDIO_ENGINE_DEBUG << "  no of channels:" << channels;

			const qint16 *ptr = reinterpret_cast<const qint16*>(m_audioFileBuffer->getRawData());
			const int numSamples = m_audioFileBuffer->getDataLength() / (m_audioFileBuffer->getBytesPerSample() * channels);
			//const int numSamples = m_audioFileBuffer->getDataLength() / m_audioFileBuffer->getBytesPerSample();
			AUDIO_ENGINE_DEBUG << "  no of samples:" << numSamples;
			//float realSample;
			QList<qreal> audioBuffer;
			
			for (int i = 0; i < numSamples; i++) {
				for (int j = 0; j < m_audioFileBuffer->getNofChannels(); j++) {
					
					//realSample = pcmToReal(*ptr);
					//AUDIO_ENGINE_DEBUG << i << "\t" << *ptr << "\t" << realSample;
					audioBuffer << pcmToReal(*ptr);
					ptr++;// += m_audioFileBuffer->getNofChannels();
				}

				//realSample = pcmToReal(*ptr);
				////AUDIO_ENGINE_DEBUG << i << "\t" << *ptr << "\t" << realSample;
				//audioBuffer << pcmToReal(*ptr);
				//ptr++;// += m_audioFileBuffer->getNofChannels();
			}

			/*int lng = audioBuffer.length();
			for (int i = 0; i < lng/6; i++)
				qDebug()  << audioBuffer.at(i) << "\t" << audioBuffer.at(i+1) << "\t" << audioBuffer.at(i+2) << "\t"
						  << audioBuffer.at(i+3) << "\t" << audioBuffer.at(i+4) << "\t" << audioBuffer.at(i+5);*/

			emit audiofileBufferChanged(audioBuffer);
        }
		else
		if (isPCMS32LE(m_file->fileFormat())) {

			result = initializePCMS32LE();
		}
		else {

            m_message = "[audio engine]: audio format %1 not supported.";
			emit messageEvent(m_message.arg(formatToString(m_file->fileFormat())));
        }
    } 
	else {
        
		m_message = "[audio engine]: could not open %1.";
		emit messageEvent(m_message.arg(fileName));
    }

    if (result) {

        m_analysisFile = new WavFile(this);
        m_analysisFile->open(fileName);
    }

    return result;
}

bool AudioEngine::generateTone(const Tone &tone) {

    reset();
    Q_ASSERT(!m_generateTone);
    Q_ASSERT(!m_file);
    m_generateTone = true;
    m_tone = tone;
    AUDIO_ENGINE_DEBUG	<< "generateTone"
						<< "startFreq" << m_tone.startFreq
						<< "endFreq" << m_tone.endFreq
						<< "amp" << m_tone.amplitude;

    return initializePCMS16LE();
}

bool AudioEngine::generateSweptTone() {

    Q_ASSERT(!m_generateTone);
    Q_ASSERT(!m_file);
    m_generateTone = true;
    m_tone.startFreq = m_lowerChirpFreq;
    m_tone.endFreq = m_upperChirpFreq;
    m_tone.amplitude = m_chirpAmplitude;
	m_tone.duration = m_chirpBufferDurationUs;
    AUDIO_ENGINE_DEBUG	<< "generateSweptTone"
						<< "startFreq" << m_tone.startFreq
						<< "endFreq" << m_tone.endFreq
						<< "amp" << m_tone.amplitude
						<< "duration" << m_tone.duration;

	m_message = "[audio engine]: set chirp signal to: low = %1 Hz, high = %2 Hz, amplitude = %3, duration = %4 s";
	emit messageEvent(m_message.arg(m_lowerChirpFreq).arg(m_upperChirpFreq).arg(m_chirpAmplitude).arg(m_chirpBufferDurationUs/1E6));

    return initializePCMS16LE();
}

void AudioEngine::generateAudioChirpSignal(const SweptTone &tone, const QAudioFormat &format, QByteArray &buffer) {

    Q_ASSERT(isPCMS16LE(format));

    const int channelBytes = format.sampleSize() / 8;
    const int sampleBytes = format.channels() * channelBytes;
    int length = buffer.size();
    const int numSamples = buffer.size() / sampleBytes;

    Q_ASSERT(length % sampleBytes == 0);
    Q_UNUSED(sampleBytes) // suppress warning in release builds

    unsigned char *ptr = reinterpret_cast<unsigned char *>(buffer.data());

    qreal phase = 0.0;

	const qreal d = TWOPI / format.frequency();
	
    // We can't generate a zero-frequency sine wave
    const qreal startFreq = tone.startFreq ? tone.startFreq : 1.0;

    // Amount by which phase increases on each sample
    qreal dw = d * startFreq;
	
    // Amount by which phaseStep increases on each sample
    // If this is non-zero, the output is a frequency-swept tone
    qreal incFreq = m_chirpRepetition * d * (tone.endFreq - startFreq) / numSamples;//(numSamples - 4800);
	
	while (length) {

        const qreal x = tone.amplitude * qSin(phase);
        const qint16 value = realToPcm(x);
        for (int i = 0; i < format.channels(); ++i) {

            qToLittleEndian<qint16>(value, ptr);
            ptr += channelBytes;
            length -= channelBytes;
        }

		phase += dw;
		if (m_chirpRepetition > 1 && (length % (buffer.size() / m_chirpRepetition) == 0)) {

			dw = d * startFreq;
			//incFreq = 0;
		}

		dw += incFreq;
    }
}

bool AudioEngine::initializeRecord() {

    reset();
    AUDIO_ENGINE_DEBUG << "initializeRecord";
    Q_ASSERT(!m_generateTone);
    Q_ASSERT(!m_file);
    m_generateTone = false;
    m_tone = SweptTone();
    return initializePCMS16LE();
}

qint64 AudioEngine::bufferLength() const {

    return m_file ? m_file->size() : m_bufferLength;
}

void AudioEngine::setWindowFunction(WindowFunction type) {

    m_spectrumAnalyser.setWindowFunction(type);
}

void AudioEngine::sampleRateChanged(QObject *sender, int value) {

	Q_UNUSED(sender)

	m_sampleRate = value;
}

//-----------------------------------------------------------------------------
// Public slots
//-----------------------------------------------------------------------------

void AudioEngine::startRecording() {

    if (m_audioInput) {

        if (QAudio::AudioInput == m_mode &&
            QAudio::SuspendedState == m_state) {

            m_audioInput->resume();
        } 
		else {
            
			m_spectrumAnalyser.cancelCalculation();
            spectrumChanged(0, 0, FrequencySpectrum());

            m_buffer.fill(0);
            setRecordPosition(0, true);
            stopPlayback();
            m_mode = QAudio::AudioInput;

            CHECKED_CONNECT(
				m_audioInput, 
				SIGNAL(stateChanged(QAudio::State)),
                this, 
				SLOT(audioStateChanged(QAudio::State)));

            CHECKED_CONNECT(
				m_audioInput, 
				SIGNAL(notify()),
                this, 
				SLOT(audioNotify()));

            m_count = 0;
            m_dataLength = 0;
            emit dataLengthChanged(0);
            m_audioInputIODevice = m_audioInput->start();

            CHECKED_CONNECT(
				m_audioInputIODevice, 
				SIGNAL(readyRead()),
                this, 
				SLOT(audioDataReady()));
        }
    }
}

void AudioEngine::startPlayback() {

    if (m_audioOutput) {
		
		if (QAudio::AudioOutput == m_mode &&
			QAudio::SuspendedState == m_state) {

#ifdef Q_OS_WIN
            // The Windows backend seems to internally go back into ActiveState
            // while still returning SuspendedState, so to ensure that it doesn't
            // ignore the resume() call, we first re-suspend
            m_audioOutput->suspend();
#endif
            m_audioOutput->resume();
        } 
		else {
			
			m_spectrumAnalyser.cancelCalculation();
            spectrumChanged(0, 0, FrequencySpectrum());
            setPlayPosition(0, true);
            stopRecording();
            m_mode = QAudio::AudioOutput;
            
			CHECKED_CONNECT(
				m_audioOutput, 
				SIGNAL(stateChanged(QAudio::State)),
				this, 
				SLOT(audioStateChanged(QAudio::State)));

            CHECKED_CONNECT(
				m_audioOutput, 
				SIGNAL(notify()),
				this, 
				SLOT(audioNotify()));

            m_count = 0;

            if (m_file) {
				
				m_file->seek(0);
                m_bufferPosition = 0;
                m_dataLength = 0;
                m_audioOutput->start(m_file);
            } 
			else {
				
				m_audioOutputIODevice.close();
                m_audioOutputIODevice.setBuffer(&m_buffer);
                m_audioOutputIODevice.open(QIODevice::ReadOnly);
				
                m_audioOutput->start(&m_audioOutputIODevice);
            }
        }
    }
}

void AudioEngine::suspend() {

    if (QAudio::ActiveState == m_state ||
        QAudio::IdleState == m_state) {
        switch (m_mode) {
        case QAudio::AudioInput:
            m_audioInput->suspend();
            break;
        case QAudio::AudioOutput:
            m_audioOutput->suspend();
            break;
        }
    }
}

void AudioEngine::setAudioInputDevice(const QAudioDeviceInfo &device) {

    if (device.deviceName() != m_audioInputDevice.deviceName()) {
        m_audioInputDevice = device;
        initializePCMS16LE();
    }
}

void AudioEngine::setAudioOutputDevice(const QAudioDeviceInfo &device) {

    if (device.deviceName() != m_audioOutputDevice.deviceName()) {
        m_audioOutputDevice = device;
		AUDIO_ENGINE_DEBUG	<< "setAudioOutputDevice to:" 
							<< m_audioOutputDevice.deviceName();

		AUDIO_ENGINE_DEBUG	<< "	options:";
		AUDIO_ENGINE_DEBUG	<< "	defaultOutputDevice" << m_audioOutputDevice.defaultOutputDevice;
		AUDIO_ENGINE_DEBUG	<< "	preferredFormat" << m_audioOutputDevice.preferredFormat();
		AUDIO_ENGINE_DEBUG	<< "	supportedByteOrders" << m_audioOutputDevice.supportedByteOrders();
		AUDIO_ENGINE_DEBUG	<< "	supportedChannelCounts" << m_audioOutputDevice.supportedChannelCounts();
		AUDIO_ENGINE_DEBUG	<< "	supportedChannels" << m_audioOutputDevice.supportedChannels();
		AUDIO_ENGINE_DEBUG	<< "	supportedCodecs" << m_audioOutputDevice.supportedCodecs();
		AUDIO_ENGINE_DEBUG	<< "	supportedFrequencies" << m_audioOutputDevice.supportedFrequencies();
		AUDIO_ENGINE_DEBUG	<< "	supportedSampleRates" << m_audioOutputDevice.supportedSampleRates();
		AUDIO_ENGINE_DEBUG	<< "	supportedSampleSizes" << m_audioOutputDevice.supportedSampleSizes();
		AUDIO_ENGINE_DEBUG	<< "	supportedSampleTypes" << m_audioOutputDevice.supportedSampleTypes();

		initializePCMS16LE();
    }
}

void AudioEngine::showSettingsDialog() {

    m_settingsDialog->exec();
    if (m_settingsDialog->result() == QDialog::Accepted) {
        setAudioInputDevice(m_settingsDialog->inputDevice());
        setAudioOutputDevice(m_settingsDialog->outputDevice());
        //setWindowFunction(m_settingsDialog->windowFunction());
    }
}

void AudioEngine::setChirpSignalMode(QObject* sender) {

	Q_UNUSED(sender)

	reset();
	generateSweptTone();

}

void AudioEngine::setChirpLowerFrequency(QObject *sender, int lo) {

	Q_UNUSED(sender)

	m_lowerChirpFreq = lo;
	reset();
	generateSweptTone();
}

void AudioEngine::setChirpUpperFrequency(QObject *sender, int hi) {

	Q_UNUSED(sender)

	m_upperChirpFreq = hi;
	reset();
	generateSweptTone();
}

void AudioEngine::setChirpAmplitude(QObject *sender, qreal amp) {

	Q_UNUSED(sender)

	m_chirpAmplitude = amp;
	reset();
	generateSweptTone();
}

void AudioEngine::setChirpBufferDurationUs(QObject *sender, qint64 value) {

	Q_UNUSED(sender)

	m_chirpBufferDurationUs = value;
	reset();
	generateSweptTone();
}

void AudioEngine::setChirpRepetitionTimes(QObject *sender, int value) {

	Q_UNUSED(sender)

	m_chirpRepetition = value;
	reset();
	generateSweptTone();
}

 
//-----------------------------------------------------------------------------
// Private slots
//-----------------------------------------------------------------------------

void AudioEngine::audioNotify() {

    switch (m_mode) {
	
		case QAudio::AudioInput: {

            const qint64 recordPosition = qMin(m_bufferLength, audioLength(m_format, m_audioInput->processedUSecs()));
            setRecordPosition(recordPosition);
            const qint64 levelPosition = m_dataLength - m_levelBufferLength;

            if (levelPosition >= 0)
                calculateLevel(levelPosition, m_levelBufferLength);

            if (m_dataLength >= m_spectrumBufferLength) {
                const qint64 spectrumPosition = m_dataLength - m_spectrumBufferLength;
                calculateSpectrum(spectrumPosition);
            }
            emit bufferChanged(this, 0, m_dataLength, m_buffer);
        }
        break;
		
		case QAudio::AudioOutput: {

            const qint64 playPosition = audioLength(m_format, m_audioOutput->processedUSecs());
            setPlayPosition(qMin(bufferLength(), playPosition));
            const qint64 levelPosition = playPosition - m_levelBufferLength;
            const qint64 spectrumPosition = playPosition - m_spectrumBufferLength;
            
			if (m_file) {

                if (levelPosition > m_bufferPosition ||
                    spectrumPosition > m_bufferPosition ||
                    qMax(m_levelBufferLength, m_spectrumBufferLength) > m_dataLength) 
				{
                    m_bufferPosition = 0;
                    m_dataLength = 0;
                    // Data needs to be read into m_buffer in order to be analysed
                    const qint64 readPos = qMax(qint64(0), qMin(levelPosition, spectrumPosition));
                    const qint64 readEnd = qMin(m_analysisFile->size(), qMax(levelPosition + m_levelBufferLength, spectrumPosition + m_spectrumBufferLength));
                    const qint64 readLen = readEnd - readPos + audioLength(m_format, WaveformWindowDuration);
                    AUDIO_ENGINE_DEBUG	<< "audioNotify [1]"
										<< "analysisFileSize" << m_analysisFile->size()
										<< "readPos" << readPos
										<< "readLen" << readLen;
                    if (m_analysisFile->seek(readPos + m_analysisFile->headerLength())) {
                        m_buffer.resize(readLen);
                        m_bufferPosition = readPos;
                        m_dataLength = m_analysisFile->read(m_buffer.data(), readLen);
                        AUDIO_ENGINE_DEBUG << "audioNotify [2]" << "bufferPosition" << m_bufferPosition << "dataLength" << m_dataLength;
                    } 
					else {

                        AUDIO_ENGINE_DEBUG << "audioNotify [2]" << "file seek error";
                    }
                    emit bufferChanged(this, m_bufferPosition, m_dataLength, m_buffer);
                }
            } 
			else {

                if (playPosition >= m_dataLength) stopPlayback();
            }

            if (levelPosition >= 0 && levelPosition + m_levelBufferLength < m_bufferPosition + m_dataLength)
                calculateLevel(levelPosition, m_levelBufferLength);
            //if (spectrumPosition >= 0 && spectrumPosition + m_spectrumBufferLength < m_bufferPosition + m_dataLength)
            //    calculateSpectrum(spectrumPosition);
			//calculateTotalSpectrum();
        }
        break;
    }
}

void AudioEngine::audioStateChanged(QAudio::State state) {

    AUDIO_ENGINE_DEBUG	<< "audioStateChanged from" << m_state
						<< "to" << state;

    if (QAudio::IdleState == state && m_file && m_file->pos() == m_file->size()) {
        stopPlayback();
    } else {
        if (QAudio::StoppedState == state) {
            // Check error
            QAudio::Error error = QAudio::NoError;
            switch (m_mode) {
            case QAudio::AudioInput:
                error = m_audioInput->error();
                break;
            case QAudio::AudioOutput:
                error = m_audioOutput->error();
                break;
            }
            if (QAudio::NoError != error) {
                reset();
                return;
            }
        }
        setState(state);
    }
}

void AudioEngine::audioDataReady() {

    Q_ASSERT(0 == m_bufferPosition);
    const qint64 bytesReady = m_audioInput->bytesReady();
    const qint64 bytesSpace = m_buffer.size() - m_dataLength;
    const qint64 bytesToRead = qMin(bytesReady, bytesSpace);

    const qint64 bytesRead = m_audioInputIODevice->read(
                                       m_buffer.data() + m_dataLength,
                                       bytesToRead);

    if (bytesRead) {
        m_dataLength += bytesRead;
        emit dataLengthChanged(dataLength());
    }

    if (m_buffer.size() == m_dataLength)
        stopRecording();
}

void AudioEngine::spectrumChanged(const FrequencySpectrum &spectrum) {

    AUDIO_ENGINE_DEBUG << "spectrumChanged" << "pos" << m_spectrumPosition;
    //emit spectrumChanged(m_spectrumPosition, m_spectrumBufferLength, spectrum);
	m_settings->setChirpSpectrum(m_spectrumPosition, m_spectrumBufferLength, spectrum);
}

void AudioEngine::spectrumListChanged(const QList<FrequencySpectrum> &spectrumList) {

	AUDIO_ENGINE_DEBUG << "spectrumListChanged";

	m_settings->setChirpSpectrumList(spectrumList);
}
 
//-----------------------------------------------------------------------------
// Private functions
//-----------------------------------------------------------------------------

void AudioEngine::setupConnections() {

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(chirpSignalModeChanged(QObject *)), 
		this, 
		SLOT(setChirpSignalMode(QObject *)));

	CHECKED_CONNECT(
		&m_spectrumAnalyser,
		SIGNAL(spectrumChanged(FrequencySpectrum)),
		this,
		SLOT(spectrumChanged(FrequencySpectrum)));

	CHECKED_CONNECT(
		&m_spectrumAnalyser,
		SIGNAL(spectrumListChanged(const QList<FrequencySpectrum>)),
		this,
		SLOT(spectrumListChanged(const QList<FrequencySpectrum>)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(lowerChirpFreqChanged(QObject *, int)), 
		this, 
		SLOT(setChirpLowerFrequency(QObject *, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(upperChirpFreqChanged(QObject *, int)), 
		this, 
		SLOT(setChirpUpperFrequency(QObject *, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(chirpAmplitudeChanged(QObject *, qreal)), 
		this, 
		SLOT(setChirpAmplitude(QObject *, qreal)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(chirpBufferDurationUsChanged(QObject *, qint64)), 
		this, 
		SLOT(setChirpBufferDurationUs(QObject *, qint64)));

	CHECKED_CONNECT(
		m_settings,
		SIGNAL(chirpRepetitionTimesChanged(QObject *, int)),
		this,
		SLOT(setChirpRepetitionTimes(QObject *, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(sampleRateChanged(QObject *, int)), 
		this, 
		SLOT(sampleRateChanged(QObject *, int)));
}

void AudioEngine::resetAudioDevices() {

	delete m_audioInput;
    m_audioInput = 0;
    m_audioInputIODevice = 0;
    setRecordPosition(0);
    delete m_audioOutput;
    m_audioOutput = 0;
    setPlayPosition(0);
    m_spectrumPosition = 0;
    setLevel(0.0, 0.0, 0);
}

void AudioEngine::reset() {

    stopRecording();
    stopPlayback();
    setState(QAudio::AudioInput, QAudio::StoppedState);
    setFormat(QAudioFormat());
    m_generateTone = false;
    delete m_file;
    m_file = 0;
    delete m_analysisFile;
    m_analysisFile = 0;
    m_buffer.clear();
	m_bufferPosition = 0;
    m_bufferLength = 0;
	m_spectrumBuffer.clear();
	m_spectrumBufferLength = 0;
    m_dataLength = 0;
    emit dataLengthChanged(0);
    resetAudioDevices();
}

bool AudioEngine::initializePCMS16LE() {

    bool result = false;

    QAudioFormat format = m_format;

    if (selectFormat()) {
        if (m_format != format) {
			
			resetAudioDevices();

            if (m_file) {

				AUDIO_ENGINE_DEBUG << "m_file bufferLength" << bufferLength();
				AUDIO_ENGINE_DEBUG << "m_file dataLength" << dataLength();
                emit bufferLengthChanged(bufferLength());
                emit dataLengthChanged(dataLength());
                emit bufferChanged(this, 0, 0, m_buffer);
                setRecordPosition(bufferLength());

				result = true;
            } 
			else {

				m_bufferLength = audioLength(m_format, m_chirpRepetition * m_chirpBufferDurationUs);
                m_buffer.resize(m_bufferLength);
                m_buffer.fill(0);
                emit bufferLengthChanged(bufferLength());
                
				if (m_generateTone) {

                    if (0 == m_tone.endFreq) {

                        const qreal nyquist = nyquistFrequency(m_format);
                        m_tone.endFreq = qMin(qreal(SpectrumHighFreq), nyquist);
                    }
                    // Call function defined in utils.h, at global scope
                    //::generateTone(m_tone, m_format, m_buffer);
					generateAudioChirpSignal(m_tone, m_format, m_buffer);
					
                    m_dataLength = m_bufferLength;
					emit chirpSignalChanged();
                    emit dataLengthChanged(dataLength());
                    emit bufferChanged(this, 0, m_dataLength, m_buffer);
                    setRecordPosition(m_bufferLength);
                    result = true;

					calculateTotalSpectrum();
                } 
				else {
                    
					emit bufferChanged(this, 0, 0, m_buffer);
                    m_audioInput = new QAudioInput(m_audioInputDevice, m_format, this);
                    m_audioInput->setNotifyInterval(NotifyIntervalMs);
                    result = true;
                }
            }
            m_audioOutput = new QAudioOutput(m_audioOutputDevice, m_format, this);
            m_audioOutput->setNotifyInterval(NotifyIntervalMs);
		}
	} 
	else {
		if (m_file) {
            m_message = "[audio engine]: audio format not supported, %1 .";
			emit messageEvent(m_message.arg(formatToString(m_format)));
		}
        else if (m_generateTone)
            emit messageEvent("[audio engine]: no suitable format found.");
        else
            emit messageEvent("[audio engine]: no common input / output format found.");
    }

    AUDIO_ENGINE_DEBUG << "initialize" << "m_bufferLength" << m_bufferLength;
    AUDIO_ENGINE_DEBUG << "initialize" << "m_dataLength" << m_dataLength;
	AUDIO_ENGINE_DEBUG << "initialize" << "format" << m_format;
	m_message = "[audio engine]: file format = %1.";
	emit messageEvent(m_message.arg(formatToString(m_format)));

    return result;
}

bool AudioEngine::initializePCMS32LE() {

	bool result = false;

	QAudioFormat format = m_format;

    if (selectFormat()) {
        if (m_format != format) {
			
			resetAudioDevices();

            if (m_file) {

                emit bufferLengthChanged(bufferLength());
                emit dataLengthChanged(dataLength());
                emit bufferChanged(this, 0, 0, m_buffer);
                setRecordPosition(bufferLength());

				result = true;
            }
		}
	}
	else {
		if (m_file) {
            m_message = "[audio engine]: audio format not supported, %1 .";
			emit messageEvent(m_message.arg(formatToString(m_format)));
		}
    }

    AUDIO_ENGINE_DEBUG << "initialize" << "m_bufferLength" << m_bufferLength;
    AUDIO_ENGINE_DEBUG << "initialize" << "m_dataLength" << m_dataLength;
	AUDIO_ENGINE_DEBUG << "initialize" << "format" << m_format;
	m_message = "[audio engine]: file format = %1.";
	emit messageEvent(m_message.arg(formatToString(m_format)));

	return result;
}

bool AudioEngine::selectFormat() {

    bool foundSupportedFormat = false;

	if (m_file || QAudioFormat() != m_format) {
		
		QAudioFormat format = m_format;
		if (m_file)
			// Header is read from the WAV file; just need to check whether
			// it is supported by the audio output device
			format = m_file->fileFormat();
		if (m_audioOutputDevice.isFormatSupported(format)) {
			
			setFormat(format);
			foundSupportedFormat = true;
		}
		else {

			QAudioFormat nearestFormat = m_audioOutputDevice.nearestFormat(format);

			QString formatStr = formatToString(nearestFormat);
			AUDIO_ENGINE_DEBUG << "audio format not supported!";
			AUDIO_ENGINE_DEBUG << "nearest file audio format" << formatStr;
		}
    } 
	else {

        QList<int> frequenciesList;
    #ifdef Q_OS_WIN
        // The Windows audio backend does not correctly report format support
        // (see QTBUG-9100).  Furthermore, although the audio subsystem captures
        // at 11025Hz, the resulting audio is corrupted.
        frequenciesList += 8000;
		//frequenciesList += 48000;
    #endif

        if (!m_generateTone)
            frequenciesList += m_audioInputDevice.supportedFrequencies();

        frequenciesList += m_audioOutputDevice.supportedFrequencies();
        frequenciesList = frequenciesList.toSet().toList(); // remove duplicates
        qSort(frequenciesList);
        AUDIO_ENGINE_DEBUG << "initialize frequenciesList" << frequenciesList;

        QList<int> channelsList;
        channelsList += m_audioInputDevice.supportedChannels();
        channelsList += m_audioOutputDevice.supportedChannels();
        channelsList = channelsList.toSet().toList();
        qSort(channelsList);
        AUDIO_ENGINE_DEBUG << "initialize channelsList" << channelsList;

        QAudioFormat format;
        format.setByteOrder(QAudioFormat::LittleEndian);
        format.setCodec("audio/pcm");
        format.setSampleSize(16);
        format.setSampleType(QAudioFormat::SignedInt);

		format.setFrequency(m_settings->getChirpSamplingFreq());
		format.setChannels(m_settings->getChirpChannels());
		const bool inputSupport = m_generateTone || m_audioInputDevice.isFormatSupported(format);
		const bool outputSupport = m_audioOutputDevice.isFormatSupported(format);
		
		AUDIO_ENGINE_DEBUG	<< "initialize checking " << format
							<< "input:" << inputSupport << ";"
							<< "output:" << outputSupport;
			
		AUDIO_ENGINE_DEBUG	<< "checking audio length:";
		AUDIO_ENGINE_DEBUG	<< "   format.frequency() =" << format.frequency();
		AUDIO_ENGINE_DEBUG	<< "   format.channels() =" << format.channels();
		AUDIO_ENGINE_DEBUG	<< "   format.sampleSize() =" << format.sampleSize();

		if (inputSupport && outputSupport) {

			setFormat(format);
			foundSupportedFormat = true;
		}

		/*int frequency, channels;
        foreach (frequency, frequenciesList) {
            if (foundSupportedFormat)
                break;
            format.setFrequency(frequency);
            foreach (channels, channelsList) {
                format.setChannels(channels);
                const bool inputSupport = m_generateTone ||
                                          m_audioInputDevice.isFormatSupported(format);
                const bool outputSupport = m_audioOutputDevice.isFormatSupported(format);
                AUDIO_ENGINE_DEBUG	<< "initialize checking " << format
									<< "input" << inputSupport
									<< "output" << outputSupport;
                if (inputSupport && outputSupport) {
                    foundSupportedFormat = true;
                    break;
                }
            }
        }

        if (!foundSupportedFormat)
            format = QAudioFormat();

        setFormat(format);*/
    }

    return foundSupportedFormat;
}

void AudioEngine::stopRecording() {

    if (m_audioInput) {
        m_audioInput->stop();
        QCoreApplication::instance()->processEvents();
        m_audioInput->disconnect();
    }
    m_audioInputIODevice = 0;

#ifdef DUMP_AUDIO
    dumpData();
#endif
}

void AudioEngine::stopPlayback() {

    if (m_audioOutput) {
        m_audioOutput->stop();
        QCoreApplication::instance()->processEvents();
        m_audioOutput->disconnect();
        setPlayPosition(0);
    }
}

void AudioEngine::setState(QAudio::State state) {

    const bool changed = (m_state != state);
    m_state = state;
    if (changed)
        emit stateChanged(m_mode, m_state);
}

void AudioEngine::setState(QAudio::Mode mode, QAudio::State state) {

    const bool changed = (m_mode != mode || m_state != state);
    m_mode = mode;
    m_state = state;
    if (changed)
        emit stateChanged(m_mode, m_state);
}

void AudioEngine::setRecordPosition(qint64 position, bool forceEmit) {

    const bool changed = (m_recordPosition != position);
    m_recordPosition = position;
    if (changed || forceEmit)
        emit recordPositionChanged(m_recordPosition);
}

void AudioEngine::setPlayPosition(qint64 position, bool forceEmit) {

    const bool changed = (m_playPosition != position);
    m_playPosition = position;
    if (changed || forceEmit)
        emit playPositionChanged(this, m_playPosition);
}

void AudioEngine::calculateLevel(qint64 position, qint64 length) {

#ifdef DISABLE_LEVEL
    Q_UNUSED(position)
    Q_UNUSED(length)
#else
    Q_ASSERT(position + length <= m_bufferPosition + m_dataLength);

    qreal peakLevel = 0.0;

    qreal sum = 0.0;
    const char *ptr = m_buffer.constData() + position - m_bufferPosition;
    const char *const end = ptr + length;
    while (ptr < end) {
        const qint16 value = *reinterpret_cast<const qint16*>(ptr);
        const qreal fracValue = pcmToReal(value);
        peakLevel = qMax(peakLevel, fracValue);
        sum += fracValue * fracValue;
        ptr += 2;
    }
    const int numSamples = length / 2;
    qreal rmsLevel = sqrt(sum / numSamples);

    rmsLevel = qMax(qreal(0.0), rmsLevel);
    rmsLevel = qMin(qreal(1.0), rmsLevel);
    setLevel(rmsLevel, peakLevel, numSamples);

    AUDIO_ENGINE_DEBUG	<< "calculateLevel" << "pos" << position << "len" << length
						<< "rms" << rmsLevel << "peak" << peakLevel;
#endif
}

void AudioEngine::calculateSpectrum(qint64 position) {

#ifdef DISABLE_SPECTRUM
    Q_UNUSED(position)
#else
    Q_ASSERT(position + m_spectrumBufferLength <= m_bufferPosition + m_dataLength);
    Q_ASSERT(0 == m_spectrumBufferLength % 2); // constraint of FFT algorithm

    // QThread::currentThread is marked 'for internal use only', but
    // we're only using it for debug output here, so it's probably OK :)
    AUDIO_ENGINE_DEBUG	<< "calculateSpectrum" << QThread::currentThread()
						<< "count" << m_count << "pos" << position << "len" << m_spectrumBufferLength
						<< "spectrumAnalyser.isReady" << m_spectrumAnalyser.isReady();

	if(m_spectrumAnalyser.isReady()) {
        //m_spectrumBuffer = QByteArray::fromRawData(m_buffer.constData() + position - m_bufferPosition,
        //                                           m_spectrumBufferLength);
        m_spectrumPosition = position;
        //m_spectrumAnalyser.calculate(m_spectrumBuffer, m_format);
	}
    
#endif
}

void AudioEngine::calculateTotalSpectrum() {

	// QThread::currentThread is marked 'for internal use only', but
    // we're only using it for debug output here, so it's probably OK :)
    AUDIO_ENGINE_DEBUG	<< "calculateTotalSpectrum" << QThread::currentThread()
						//<< "count" << m_count << "pos" << position << "len" << m_spectrumBufferLength
						<< "spectrumAnalyser.isReady" << m_spectrumAnalyser.isReady();

	if(m_spectrumAnalyser.isReady()) {

        //m_spectrumBuffer = QByteArray::fromRawData(m_buffer.constData() + position - m_bufferPosition, m_spectrumBufferLength);
		m_spectrumBuffer = QByteArray::fromRawData(m_buffer.constData(), m_bufferLength);
        
        m_spectrumAnalyser.calculateTotal(0, m_dataLength, m_format, m_spectrumBuffer);
    }
}

void AudioEngine::setFormat(const QAudioFormat &format) {

    const bool changed = (format != m_format);
    m_format = format;
    m_levelBufferLength = audioLength(m_format, LevelWindowUs);
    m_spectrumBufferLength = SpectrumLengthSamples * (m_format.sampleSize() / 8) * m_format.channels();
    if (changed)
        emit formatChanged(this, m_format);
}

void AudioEngine::setLevel(qreal rmsLevel, qreal peakLevel, int numSamples) {

    m_rmsLevel = rmsLevel;
    m_peakLevel = peakLevel;
    emit levelChanged(m_rmsLevel, m_peakLevel, numSamples);
}

#ifdef DUMP_DATA
void AudioEngine::createOutputDir()
{
    m_outputDir.setPath("output");

    // Ensure output directory exists and is empty
    if (m_outputDir.exists()) {
        const QStringList files = m_outputDir.entryList(QDir::Files);
        QString file;
        foreach (file, files)
            m_outputDir.remove(file);
    } else {
        QDir::current().mkdir("output");
    }
}
#endif // DUMP_DATA

#ifdef DUMP_AUDIO
void AudioEngine::dumpData()
{
    const QString txtFileName = m_outputDir.filePath("data.txt");
    QFile txtFile(txtFileName);
    txtFile.open(QFile::WriteOnly | QFile::Text);
    QTextStream stream(&txtFile);
    const qint16 *ptr = reinterpret_cast<const qint16*>(m_buffer.constData());
    const int numSamples = m_dataLength / (2 * m_format.channels());
    for (int i=0; i<numSamples; ++i) {
        stream << i << "\t" << *ptr << "\n";
        ptr += m_format.channels();
    }

    const QString pcmFileName = m_outputDir.filePath("data.pcm");
    QFile pcmFile(pcmFileName);
    pcmFile.open(QFile::WriteOnly);
    pcmFile.write(m_buffer.constData(), m_dataLength);
}
#endif // DUMP_AUDIO


 
// *********************************************************************
// AudiofileBuffer

AudiofileBuffer::AudiofileBuffer() {
	
	m_data = 0;
	m_dataLength = 0;
	m_sampleFunction = 0;
};

AudiofileBuffer::~AudiofileBuffer() {
	
	reallocate(0);
}

void AudiofileBuffer::reallocate( int length ) {
	
	if (m_data) delete [] ((char*)m_data);
	
	m_dataLength = length;
	if (m_dataLength>0) {
		
		m_data = new char[ m_dataLength ];
	} 
	else 
		m_data = 0;
};


AudiofileBuffer *AudiofileBuffer::loadWav(QString fileName) {
	
	QFile *wavFile = new QFile(fileName);
	
	if (wavFile->open(QIODevice::ReadOnly)) {
		
		SWavHeader header;
		
		wavFile->read(header.chunkID, 4);
		if (header.chunkID[0]!='R' || header.chunkID[1]!='I' || 
			header.chunkID[2]!='F' || header.chunkID[3]!='F') 
			return 0;    //  incorrect header
		
		wavFile->read((char*)&header.chunkSize, 4);
		wavFile->read((char*)&header.format, 4);
		
		if (header.format[0]!='W' || header.format[1]!='A' || 
			header.format[2]!='V' || header.format[3]!='E') 
			return 0;    //  incorrect header
		
		wavFile->read((char*)&header.subchunk1id, 4);
		if (header.subchunk1id[0]!='f' || header.subchunk1id[1]!='m' || 
			header.subchunk1id[2]!='t' || header.subchunk1id[3]!=' ') 
			return 0;    //  incorrect header
		
		wavFile->read((char*)&header.subchunk1size, 4);
		wavFile->read((char*)&header.audioFormat, 2);
		wavFile->read((char*)&header.nofChannels, 2);
		wavFile->read((char*)&header.sampleRate, 4);
		wavFile->read((char*)&header.byteRate, 4);
		wavFile->read((char*)&header.blockAlign, 2);
		wavFile->read((char*)&header.bitsPerSample, 2);
		
		AUDIO_ENGINE_DEBUG << fileName << " opened.";
		
		while (1) {
			
			if (wavFile->read((char*)&header.subchunk2id, 4) != 4) return 0;
			if (wavFile->read((char*)&header.subchunk2size, 4) != 4) return 0;

            //int deb_size = header.subchunk2size;
            //char tes[4];
            //memcpy(tes, header.subchunk2id, 4 );
            //if (header.subchunk2id[0]!='d' || header.subchunk2id[1]!='a' || header.subchunk2id[2]!='t' || header.subchunk2id[3]!='a') return 0;    //  incorrect header
			
			if (header.subchunk2id[0]=='d' && header.subchunk2id[1]=='a' && 
				header.subchunk2id[2]=='t' && header.subchunk2id[3]=='a') 
				break;            // found the data, chunk
			
			// this was not the data-chunk. skip it
			if (header.subchunk2size < 1) return 0;           // error in file
			char *unused = new char[header.subchunk2size];
			wavFile->read(unused, header.subchunk2size );
			delete [] unused;
		}

         // the data follows.
         if (header.subchunk2size < 1) return 0;

         AudiofileBuffer *rval = new AudiofileBuffer;
		 rval->m_nofChannels = header.nofChannels;
		 rval->m_bitsPerSample = header.bitsPerSample;
         rval->m_samplesPerSec = header.sampleRate;
         rval->m_signedData = 0;        // where to know this?
         rval->reallocate(header.subchunk2size);
		 
		 wavFile->read((char*)rval->m_data, header.subchunk2size);
		 
		 // choose a good sampling function.
		 rval->m_sampleFunction = 0;
		 if (rval->m_nofChannels == 1) {

             if (rval->m_bitsPerSample == 8) 
				 rval->m_sampleFunction = sampleFunction8bitMono;
             if (rval->m_bitsPerSample == 16) 
				 rval->m_sampleFunction = sampleFunction16bitMono;

         } 
		 else {

             if (rval->m_bitsPerSample == 8) 
				 rval->m_sampleFunction = sampleFunction8bitStereo;
             if (rval->m_bitsPerSample == 16) 
				 rval->m_sampleFunction = sampleFunction16bitStereo;
         }

         return rval;

     } 
	else {

         AUDIO_ENGINE_DEBUG << fileName << " NOT opened";
         return 0;
     }

     delete wavFile;
 };


AUDIO_SAMPLE_TYPE AudiofileBuffer::sampleFunction8bitMono(AudiofileBuffer *abuffer, int pos, int channel) {

	Q_UNUSED(channel)
	return (AUDIO_SAMPLE_TYPE)(((unsigned char*)(abuffer->m_data))[pos]-128) << 8;
};

AUDIO_SAMPLE_TYPE AudiofileBuffer::sampleFunction16bitMono(AudiofileBuffer *abuffer, int pos, int channel) {

	Q_UNUSED(channel)
	return (AUDIO_SAMPLE_TYPE)(((short*)(abuffer->m_data))[pos]);
};

AUDIO_SAMPLE_TYPE AudiofileBuffer::sampleFunction8bitStereo(AudiofileBuffer *abuffer, int pos, int channel) {
	
	return ((AUDIO_SAMPLE_TYPE)(((char*)(abuffer->m_data))[pos*abuffer->m_nofChannels + channel]) << 8);
};

AUDIO_SAMPLE_TYPE AudiofileBuffer::sampleFunction16bitStereo(AudiofileBuffer *abuffer, int pos, int channel) {
	
	return (AUDIO_SAMPLE_TYPE)(((short*)(abuffer->m_data))[pos*abuffer->m_nofChannels + channel]);
};