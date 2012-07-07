/**
* @file cusdr_audio_spectrumanalyser.h
* @brief cuSDR audio engine spectrumanalyser header file
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

#ifndef _CUSDR_AUDIO_SPECTRUMANALYSER_H
#define _CUSDR_AUDIO_SPECTRUMANALYSER_H

#include <QByteArray>
#include <QObject>
#include <QVector>

#ifdef DUMP_SPECTRUMANALYSER
#include <QDir>
#include <QFile>
#include <QTextStream>
#endif

//#define SPECTRUM_ANALYSER_SEPARATE_THREAD

//#include "frequencyspectrum.h"
#include "cusdr_audio_spectrum.h"
#include "cusdr_settings.h"
#include "cusdr_fft.h"
#include "cusdr_complex.h"
//#include "cusdr_filter.h"

QT_FORWARD_DECLARE_CLASS(QAudioFormat)
QT_FORWARD_DECLARE_CLASS(QThread)

//class SpectrumAnalyserThreadPrivate;

/**
 * Implementation of the spectrum analysis which can be run in a
 * separate thread.
 */
class SpectrumAnalyserThread : public QObject
{
    Q_OBJECT
public:
    SpectrumAnalyserThread(QObject *parent);
    ~SpectrumAnalyserThread();

public slots:
    void setWindowFunction(WindowFunction type);

    void calculateSpectrum(
			const QByteArray &buffer,
			int inputFrequency,
			int bytesPerSample);

	void calculateTotalSpectrum(
			const QByteArray &buffer,
			int inputFrequency,
			int bytesPerSample);

signals:
    void calculationComplete(const FrequencySpectrum &spectrum);
	void calculationTotalComplete(const QList<FrequencySpectrum> &spectrumList);

private:
    void calculateWindow();

private:
	Settings*			m_settings;
    int					m_numSamples;

    WindowFunction		m_windowFunction;

    //typedef qreal		DataType;
	typedef float		DataType;

    QVector<DataType>	m_window;

    QVector<DataType>	m_input;
    QVector<DataType>	m_output;

	CPX					*m_cpxInput;
	CPX					*m_cpxOutput;

	QFFT				*m_fft;

	QByteArray			m_tmp;

    FrequencySpectrum   m_spectrum;

	QList<FrequencySpectrum>	m_spectrumList;

#ifdef SPECTRUM_ANALYSER_SEPARATE_THREAD
    QThread*			m_thread;
#endif
};

/**
 * Class which performs frequency spectrum analysis on a window of
 * audio samples, provided to it by the Engine.
 */
class SpectrumAnalyser : public QObject
{
    Q_OBJECT
public:
    SpectrumAnalyser(QObject *parent = 0);
    ~SpectrumAnalyser();

#ifdef DUMP_SPECTRUMANALYSER
    void setOutputPath(const QString &outputPath);
#endif

public:
    /*
     * Set the windowing function which is applied before calculating the FFT
     */
    void setWindowFunction(WindowFunction type);

    /*
     * Calculate a frequency spectrum
     *
     * \param buffer       Audio data
     * \param format       Format of audio data
     *
     * Frequency spectrum is calculated asynchronously.  The result is returned
     * via the spectrumChanged signal.
     *
     * An ongoing calculation can be cancelled by calling cancelCalculation().
     *
     */
    void calculate(const QByteArray &buffer, const QAudioFormat &format);

	void calculateTotal(
			qint64 position, 
			qint64 length,
			const QAudioFormat &format,
			const QByteArray &buffer);

    /*
     * Check whether the object is ready to perform another calculation
     */
    bool isReady() const;

    /*
     * Cancel an ongoing calculation
     *
     * Note that cancelling is asynchronous.
     */
    void cancelCalculation();

signals:
    void spectrumChanged(const FrequencySpectrum &spectrum);
	void spectrumListChanged(const QList<FrequencySpectrum> &spectrumList);

private slots:
    void calculationComplete(const FrequencySpectrum &spectrum);
	void calculationTotalComplete(const QList<FrequencySpectrum> &m_spectrumList);

private:
    void calculateWindow();

private:

    SpectrumAnalyserThread*    m_thread;

    enum State {
        Idle,
        Busy,
        Cancelled
    };

    State              m_state;

#ifdef DUMP_SPECTRUMANALYSER
    QDir                m_outputDir;
    int                 m_count;
    QFile               m_textFile;
    QTextStream         m_textStream;
#endif
};

#endif // _CUSDR_AUDIO_SPECTRUMANALYSER_H

