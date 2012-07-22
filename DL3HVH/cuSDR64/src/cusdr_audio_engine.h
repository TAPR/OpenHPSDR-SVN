/**
* @file cusdr_audio_engine.h
* @brief cuSDR audio engine header file
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

#ifndef _CUSDR_AUDIOENGINE_H
#define _CUSDR_AUDIOENGINE_H

#include "cusdr_audio_spectrum.h"
#include "cusdr_audio_spectrumanalyser.h"
#include "cusdr_audio_wavfile.h"
#include "cusdr_audio_settingsdialog.h"

#include <QObject>
//#include <QWidget>
#include <QByteArray>
#include <QBuffer>
#include <QVector>
#include <QtMultimedia/QAudioDeviceInfo>
#include <QtMultimedia/QAudioFormat>
#include <QFile>


#ifdef DUMP_CAPTURED_AUDIO
#define DUMP_DATA
#endif

#ifdef DUMP_SPECTRUM
#define DUMP_DATA
#endif

#ifdef DUMP_DATA
#include <QDir>
#endif

#define AUDIO_SAMPLE_TYPE short

//class SettingsDialog;
class FrequencySpectrum;

QT_FORWARD_DECLARE_CLASS(QAudioInput)
QT_FORWARD_DECLARE_CLASS(QAudioOutput)
QT_FORWARD_DECLARE_CLASS(QFile)


class AudiofileBuffer;

typedef AUDIO_SAMPLE_TYPE(*SAMPLE_FUNCTION_TYPE)(AudiofileBuffer *abuffer, int pos, int channel);


/**
 * This class interfaces with the QtMultimedia audio classes.  Its role is 
 * to manage the capture and playback of audio data.
 */

class AudioEngine : public QWidget {
    Q_OBJECT

public:
    //AudioEngine(QObject *parent = 0);
	AudioEngine(QWidget *parent = 0);
	~AudioEngine();

    const QList<QAudioDeviceInfo>& availableAudioInputDevices() const
                                    { return m_availableAudioInputDevices; }

    const QList<QAudioDeviceInfo>& availableAudioOutputDevices() const
                                    { return m_availableAudioOutputDevices; }

    QAudio::Mode mode() const       { return m_mode; }
    QAudio::State state() const     { return m_state; }

    /**
     * \return Current audio format
     * \note May be QAudioFormat() if engine is not initialized
     */
    const QAudioFormat& format() const  { return m_format; }

    /**
     * Stop any ongoing recording or playback, and reset to ground state.
     */
    void reset();

    /**
     * Load data from WAV file
     */
    bool loadFile(const QString &fileName);

    /**
     * Generate tone
     */
    bool generateTone(const Tone &tone);

    /**
     * Generate tone
     */
    //bool generateSweptTone(qreal amplitude);
	bool generateSweptTone();	

	/**
     * Generate local chirp signal
     */
    bool generateLocalChirp();

    /**
     * Initialize for recording
     */
    bool initializeRecord();

    /**
     * Position of the audio input device.
     * \return Position in bytes.
     */
    qint64 recordPosition() const   { return m_recordPosition; }

    /**
     * RMS level of the most recently processed set of audio samples.
     * \return Level in range (0.0, 1.0)
     */
    qreal rmsLevel() const          { return m_rmsLevel; }

    /**
     * Peak level of the most recently processed set of audio samples.
     * \return Level in range (0.0, 1.0)
     */
    qreal peakLevel() const         { return m_peakLevel; }

    /**
     * Position of the audio output device.
     * \return Position in bytes.
     */
    qint64 playPosition() const     { return m_playPosition; }

    /**
     * Length of the internal engine buffer.
     * \return Buffer length in bytes.
     */
    qint64 bufferLength() const;

    /**
     * Amount of data held in the buffer.
     * \return Data length in bytes.
     */
    qint64 dataLength() const       { return m_dataLength; }

    /**
     * Set window function applied to audio data before spectral analysis.
     */
    void setWindowFunction(WindowFunction type);

public slots:
    void startRecording();
    void startPlayback();
	void showSettingsDialog();
    void suspend();
    void setAudioInputDevice(const QAudioDeviceInfo &device);
    void setAudioOutputDevice(const QAudioDeviceInfo &device);

	/**
     * set Chirp tone parameters
     */
	void setChirpSignalMode(QObject *);
	//void generateChirpSignal(const SweptTone &tone);
	void generateAudioChirpSignal(const SweptTone &tone, const QAudioFormat &format, QByteArray &buffer);
	void setChirpLowerFrequency(QObject *sender, int lo);
	void setChirpUpperFrequency(QObject *sender, int lo);
	void setChirpAmplitude(QObject *sender, qreal amp);
	void setChirpBufferDurationUs(QObject *sender, qint64 value);
	void setChirpRepetitionTimes(QObject *sender, int value);
	void sampleRateChanged(QObject *sender, int value);

signals:
    void stateChanged(QAudio::Mode mode, QAudio::State state);

    /**
     * Informational message for non-modal display
     */
	void messageEvent(QString msg);
    void infoMessage(const QString &message, int durationMs);

    /**
     * Error message for modal display
     */
    void errorMessage(const QString &heading, const QString &detail);

    /**
     * Format of audio data has changed
     */
    void formatChanged(QObject *sender, const QAudioFormat &format);

    /**
     * Length of buffer has changed.
     * \param duration Duration in microseconds
     */
    void bufferLengthChanged(qint64 duration);

    /**
     * Amount of data in buffer has changed.
     * \param Length of data in bytes
     */
    void dataLengthChanged(qint64 duration);

    /**
     * Position of the audio input device has changed.
     * \param position Position in bytes
     */
    void recordPositionChanged(qint64 position);

    /**
     * Position of the audio output device has changed.
     * \param position Position in bytes
     */
    void playPositionChanged(QObject *sender, qint64 position);

    /**
     * Level changed
     * \param rmsLevel RMS level in range 0.0 - 1.0
     * \param peakLevel Peak level in range 0.0 - 1.0
     * \param numSamples Number of audio samples analyzed
     */
    void levelChanged(qreal rmsLevel, qreal peakLevel, int numSamples);

    /**
     * Spectrum has changed.
     * \param position Position of start of window in bytes
     * \param length   Length of window in bytes
     * \param spectrum Resulting frequency spectrum
     */
    void spectrumChanged(qint64 position, qint64 length, const FrequencySpectrum &spectrum);

    /**
     * Buffer containing audio data has changed.
     * \param position Position of start of buffer in bytes
     * \param buffer   Buffer
     */
    void bufferChanged(QObject *sender, qint64 position, qint64 length, const QByteArray &buffer);
	//void chirpBufferChanged(qint64 length, const QList<qreal> &buffer);

	void chirpSignalChanged();
	void audiofileBufferChanged(const QList<qreal> &buffer);

private slots:
    void audioNotify();
    void audioStateChanged(QAudio::State state);
    void audioDataReady();
    void spectrumChanged(const FrequencySpectrum &spectrum);
	void spectrumListChanged(const QList<FrequencySpectrum> &spectrumList);

private:
	void setupConnections();
    void resetAudioDevices();
    bool initializePCMS16LE();
	bool initializePCMS32LE();
    bool selectFormat();
    void stopRecording();
    void stopPlayback();
    void setState(QAudio::State state);
    void setState(QAudio::Mode mode, QAudio::State state);
    void setFormat(const QAudioFormat &format);
    void setRecordPosition(qint64 position, bool forceEmit = false);
    void setPlayPosition(qint64 position, bool forceEmit = false);
    void calculateLevel(qint64 position, qint64 length);
    void calculateSpectrum(qint64 position);
	void calculateTotalSpectrum();
    void setLevel(qreal rmsLevel, qreal peakLevel, int numSamples);

#ifdef DUMP_DATA
    void createOutputDir();
    QString outputPath() const { return m_outputDir.path(); }
#endif

#ifdef DUMP_CAPTURED_AUDIO
    void dumpData();
#endif

private:
	Settings*			m_settings;

	QString				m_message;
    QAudio::Mode        m_mode;
    QAudio::State       m_state;

    bool                m_generateTone;
	bool				m_generateLocalChirp;
    SweptTone           m_tone;
	int					m_lowerChirpFreq;
	int					m_upperChirpFreq;
	qreal				m_chirpAmplitude;
	int					m_chirpSamplingFreq;
	int					m_downRate;
	qint64				m_chirpBufferDurationUs;
	int					m_chirpChannels;
	int					m_chirpRepetition;

    WavFile*            m_file;
    // We need a second file handle via which to read data into m_buffer
    // for analysis
    WavFile*            m_analysisFile;

    QAudioFormat        m_format;

    const QList<QAudioDeviceInfo> m_availableAudioInputDevices;
    QAudioDeviceInfo    m_audioInputDevice;
    QAudioInput*        m_audioInput;
    QIODevice*          m_audioInputIODevice;
    qint64              m_recordPosition;

    const QList<QAudioDeviceInfo> m_availableAudioOutputDevices;
    QAudioDeviceInfo    m_audioOutputDevice;
    QAudioOutput*		m_audioOutput;
    qint64              m_playPosition;
    QBuffer             m_audioOutputIODevice;

    QByteArray          m_buffer;
    qint64              m_bufferPosition;
    qint64              m_bufferLength;
    qint64              m_dataLength;

    int                 m_levelBufferLength;
    qreal               m_rmsLevel;
    qreal               m_peakLevel;

    int                 m_spectrumBufferLength;
    QByteArray          m_spectrumBuffer;
    SpectrumAnalyser    m_spectrumAnalyser;
    qint64              m_spectrumPosition;

    int                 m_count;

	SettingsDialog*		m_settingsDialog;

	int					m_sampleRate;

	AudiofileBuffer		*m_audioFileBuffer;

#ifdef DUMP_DATA
    QDir                m_outputDir;
#endif

};

 
// *********************************************************************
// AudiofileBuffer Class

class AudiofileBuffer {

public:
	AudiofileBuffer();
	virtual ~AudiofileBuffer();
	
	static AudiofileBuffer *loadWav(QString fileName);
	//static AudiofileBuffer *loadWav( FILE *wavFile );	// support for stdio
	
	void reallocate( int length );
	
	inline void *getRawData()		{ return m_data; }
	inline int   getDataLength()	{ return m_dataLength; }
	
	inline int getBytesPerSample()	{ return (m_bitsPerSample >> 3); }
	inline int getBitsPerSample()	{ return m_bitsPerSample; }
	inline int getSamplesPerSec()	{ return m_samplesPerSec; }
	inline short getNofChannels()	{ return m_nofChannels; }
	
	inline SAMPLE_FUNCTION_TYPE getSampleFunction() { return m_sampleFunction; }

	// static implementations of sample functions
	static AUDIO_SAMPLE_TYPE sampleFunction8bitMono(AudiofileBuffer *abuffer, int pos, int channel);
	static AUDIO_SAMPLE_TYPE sampleFunction16bitMono(AudiofileBuffer *abuffer, int pos, int channel);
	static AUDIO_SAMPLE_TYPE sampleFunction8bitStereo(AudiofileBuffer *abuffer, int pos, int channel);
	static AUDIO_SAMPLE_TYPE sampleFunction16bitStereo(AudiofileBuffer *abuffer, int pos, int channel);

protected:
	SAMPLE_FUNCTION_TYPE m_sampleFunction;
	
	short	m_nofChannels;
	void	*m_data;
	int		m_dataLength;                // in bytes
	short	m_bitsPerSample;
	bool	m_signedData;
	int		m_samplesPerSec;
};

#endif // _CUSDR_AUDIOENGINE_H
