/**
* @file cusdr_audio_spectrumanalyser.cpp
* @brief cuSDR audio engine spectrumanalyser class
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
//#define LOG_SPECTRUMANALYSER
//#define DUMP_SPECTRUMANALYSER

#include "cusdr_audio_spectrumanalyser.h"
#include "cusdr_audio_utils.h"
#include "cusdr_settings.h"

#include <QtCore/qmath.h>
#include <QtCore/qmetatype.h>
#include <QtMultimedia/QAudioFormat>
#include <QThread>



SpectrumAnalyserThread::SpectrumAnalyserThread(QObject *parent)
    :   QObject(parent)
	,	m_settings(Settings::instance())
    ,   m_numSamples(SpectrumLengthSamples)
    ,   m_windowFunction(DefaultWindowFunction)
    ,   m_window(SpectrumLengthSamples, 0.0)
    ,   m_input(SpectrumLengthSamples, 0.0)
    ,   m_output(SpectrumLengthSamples, 0.0)
    ,   m_spectrum(SpectrumLengthSamples)
#ifdef SPECTRUM_ANALYSER_SEPARATE_THREAD
    ,   m_thread(new QThread(this))
#endif
{
#ifdef SPECTRUM_ANALYSER_SEPARATE_THREAD
    // moveToThread() cannot be called on a QObject with a parent
    setParent(0);
    moveToThread(m_thread);
    m_thread->start();
#endif

	m_cpxInput = mallocCPX(SpectrumLengthSamples);
	m_cpxOutput = mallocCPX(SpectrumLengthSamples);

	m_fft = new QFFT(this, SpectrumLengthSamples);

	memset(m_cpxInput, 0, SpectrumLengthSamples * sizeof(CPX));
	memset(m_cpxOutput, 0, SpectrumLengthSamples * sizeof(CPX));

    calculateWindow();
}

SpectrumAnalyserThread::~SpectrumAnalyserThread() {
	
	delete m_fft;
}

void SpectrumAnalyserThread::setWindowFunction(WindowFunction type) {

    m_windowFunction = type;
    calculateWindow();
}

void SpectrumAnalyserThread::calculateWindow() {

    for (int i = 0; i < m_numSamples; ++i) {
		
		DataType x = 0.0;

        switch (m_windowFunction) {
			
			case NoWindow:
				x = 1.0;
				break;

			case HannWindow:
				x = 0.5 * (1 - qCos((TWOPI * i) / (m_numSamples - 1)));
				break;

			case BlackmanHarrisWindow: {
				float 
                        a0 = 0.35875F,
                        a1 = 0.48829F,
                        a2 = 0.14128F,
                        a3 = 0.01168F;


                x = a0 - a1* cos(TWOPI * (i + 0.5) / m_numSamples) 
                       + a2* cos(2.0 * TWOPI * (i + 0.5) / m_numSamples)
                       - a3* cos(3.0 * TWOPI * (i + 0.5) / m_numSamples);
				}
				break;
        
			default:
            Q_ASSERT(false);
        }

        m_window[i] = x;
    }
}

void SpectrumAnalyserThread::calculateTotalSpectrum(const QByteArray &buffer, int inputFrequency, int bytesPerSample) {

	if (m_spectrumList.count() > 0)
		m_spectrumList.clear();

	int samples = bytesPerSample * m_numSamples;
	int buffers = qRound(buffer.size() / samples);

	// cycle over all buffers
	for (int i = 0; i < buffers; i++) {
		
		//if (i == buffers-1 && overhead > 0)
		m_tmp = QByteArray::fromRawData(buffer.constData() + i * samples, samples);
		
		//Q_ASSERT(m_tmp.size() == m_numSamples * bytesPerSample);

		// Initialize data array
		const char *ptr = m_tmp.constData();
		
		for (int j = 0; j < m_numSamples; ++j) {

			const qint16 pcmSample = *reinterpret_cast<const qint16*>(ptr);
			// Scale down to range [-1.0, 1.0]
			const DataType realSample = pcmToReal(pcmSample);
			const DataType windowedSample = realSample * m_window[j];
	
			m_cpxInput[j].re = windowedSample;
			m_cpxInput[j].im = 0.0f;
			
			ptr += bytesPerSample;
		}

		// calculate the FFT
		m_fft->DoFFTWForward(m_cpxInput, m_cpxOutput, SpectrumLengthSamples);

		/*for (int i = 0; i < BUFFER_SIZE; i += 32) {
			qDebug() << "m_cpxOutput.re =" << m_cpxOutput[i].re << "m_cpxOutput.im =" << m_cpxOutput[i].im;
		}*/

		// Analyze output to obtain amplitude and phase for each frequency
		for (int i = 2; i <= m_numSamples / 2; ++i) {
			
			// Calculate frequency of this complex sample
			m_spectrum[i].frequency = qreal(i * inputFrequency) / (m_numSamples);
			
			//const qreal real = m_output[i];
			const qreal real = m_cpxOutput[i].re;
			qreal imag = 0.0;
			
			if (i > 0 && i < m_numSamples / 2)				
				imag = m_cpxOutput[m_numSamples/2 + i].re;

			const qreal magnitude = sqrt(real*real + imag*imag);
			qreal amplitude = SpectrumAnalyserMultiplier * log(magnitude);
			
			// Bound amplitude to [0.0, 1.0]
			m_spectrum[i].clipped = (amplitude > 1.0);
			amplitude = qMax(qreal(0.0), amplitude);
			amplitude = qMin(qreal(1.0), amplitude);
			m_spectrum[i].amplitude = amplitude;
		}
		m_spectrumList.append(m_spectrum);
	}
	emit calculationTotalComplete(m_spectrumList);
}

void SpectrumAnalyserThread::calculateSpectrum(const QByteArray &buffer, int inputFrequency, int bytesPerSample) {

    Q_ASSERT(buffer.size() == m_numSamples * bytesPerSample);

    // Initialize data array
    const char *ptr = buffer.constData();
    for (int i = 0; i < m_numSamples; ++i) {
		
		const qint16 pcmSample = *reinterpret_cast<const qint16*>(ptr);
        // Scale down to range [-1.0, 1.0]
        const DataType realSample = pcmToReal(pcmSample);
        const DataType windowedSample = realSample * m_window[i];
        m_input[i] = windowedSample;
		m_cpxInput[i].re = windowedSample;
		m_cpxInput[i].im = 0.0f;
        ptr += bytesPerSample;
    }

    // Calculate the FFT
    m_fft->DoFFTWForward(m_cpxInput, m_cpxOutput, SpectrumLengthSamples);

    // Analyze output to obtain amplitude and phase for each frequency
    for (int i = 2; i <= m_numSamples / 2; ++i) {
        // Calculate frequency of this complex sample
        m_spectrum[i].frequency = qreal(i * inputFrequency) / (m_numSamples);

        //const qreal real = m_output[i];
		const qreal real = m_cpxOutput[i].re;
        qreal imag = 0.0;
        if (i > 0 && i < m_numSamples / 2)
            //imag = m_output[m_numSamples/2 + i];
			imag = m_cpxOutput[m_numSamples/2 + i].re;

        const qreal magnitude = sqrt(real*real + imag*imag);
        qreal amplitude = SpectrumAnalyserMultiplier * log(magnitude);

        // Bound amplitude to [0.0, 1.0]
        m_spectrum[i].clipped = (amplitude > 1.0);
        amplitude = qMax(qreal(0.0), amplitude);
        amplitude = qMin(qreal(1.0), amplitude);
        m_spectrum[i].amplitude = amplitude;
    }

    emit calculationComplete(m_spectrum);
}



//=============================================================================
// SpectrumAnalyser
//=============================================================================

SpectrumAnalyser::SpectrumAnalyser(QObject *parent)
    :   QObject(parent)
    ,   m_thread(new SpectrumAnalyserThread(this))
    ,   m_state(Idle)
#ifdef DUMP_SPECTRUMANALYSER
    ,   m_count(0)
#endif
{
    CHECKED_CONNECT(
		m_thread, 
		SIGNAL(calculationComplete(FrequencySpectrum)),
		this, 
		SLOT(calculationComplete(FrequencySpectrum)));

	CHECKED_CONNECT(
		m_thread, 
		SIGNAL(calculationTotalComplete(QList<FrequencySpectrum>)),
		this, 
		SLOT(calculationTotalComplete(QList<FrequencySpectrum>)));
}

SpectrumAnalyser::~SpectrumAnalyser() {
}

#ifdef DUMP_SPECTRUMANALYSER
void SpectrumAnalyser::setOutputPath(const QString &outputDir)
{
    m_outputDir.setPath(outputDir);
    m_textFile.setFileName(m_outputDir.filePath("spectrum.txt"));
    m_textFile.open(QIODevice::WriteOnly | QIODevice::Text);
    m_textStream.setDevice(&m_textFile);
}
#endif

//-----------------------------------------------------------------------------
// Public functions
//-----------------------------------------------------------------------------

void SpectrumAnalyser::setWindowFunction(WindowFunction type) {

    const bool b = QMetaObject::invokeMethod(m_thread, "setWindowFunction",
                              Qt::AutoConnection,
                              Q_ARG(WindowFunction, type));
    Q_ASSERT(b);
    Q_UNUSED(b) // suppress warnings in release builds
}

void SpectrumAnalyser::calculateTotal(
			qint64 position, 
			qint64 length, 
			const QAudioFormat &format, 
			const QByteArray &buffer
) {
	Q_UNUSED(position)
	Q_UNUSED(length)

	SPECTRUMANALYSER_DEBUG << "SpectrumAnalyser::calculateTotal"
                           << QThread::currentThread()
                           << "state" << m_state;

	SPECTRUMANALYSER_DEBUG << "buffer size =" << buffer.size();

	if (isReady()) {
        Q_ASSERT(isPCMS16LE(format));

        const int bytesPerSample = format.sampleSize() * format.channels() / 8;

        m_state = Busy;

        // Invoke SpectrumAnalyserThread::calculateTotalSpectrum using QMetaObject.
        // If m_thread is in a different thread from the current thread, the
        // calculation will be done in the child thread.
        // Once the calculation is finished, a calculationChanged signal will be
        // emitted by m_thread.
        const bool b = QMetaObject::invokeMethod(m_thread, "calculateTotalSpectrum",
                                  Qt::AutoConnection,
                                  Q_ARG(QByteArray, buffer),
                                  Q_ARG(int, format.frequency()),
                                  Q_ARG(int, bytesPerSample));
        Q_ASSERT(b);
        Q_UNUSED(b) // suppress warnings in release builds
    }
}

void SpectrumAnalyser::calculate(const QByteArray &buffer,
                         const QAudioFormat &format)
{
    // QThread::currentThread is marked 'for internal use only', but
    // we're only using it for debug output here, so it's probably OK :)
    SPECTRUMANALYSER_DEBUG << "SpectrumAnalyser::calculate"
                           << QThread::currentThread()
                           << "state" << m_state;

	SPECTRUMANALYSER_DEBUG << "buffer size =" << buffer.size();

    if (isReady()) {
        Q_ASSERT(isPCMS16LE(format));

        const int bytesPerSample = format.sampleSize() * format.channels() / 8;

        m_state = Busy;

        // Invoke SpectrumAnalyserThread::calculateSpectrum using QMetaObject.  If
        // m_thread is in a different thread from the current thread, the
        // calculation will be done in the child thread.
        // Once the calculation is finished, a calculationChanged signal will be
        // emitted by m_thread.
        const bool b = QMetaObject::invokeMethod(m_thread, "calculateSpectrum",
                                  Qt::AutoConnection,
                                  Q_ARG(QByteArray, buffer),
                                  Q_ARG(int, format.frequency()),
                                  Q_ARG(int, bytesPerSample));
        Q_ASSERT(b);
        Q_UNUSED(b) // suppress warnings in release builds

#ifdef DUMP_SPECTRUMANALYSER
        m_textStream << "FrequencySpectrum " << m_count << "\n";
        FrequencySpectrum::const_iterator x = m_spectrum.begin();
        for (int i=0; i<m_numSamples; ++i, ++x)
            m_textStream << i << "\t"
                         << x->frequency << "\t"
                         << x->amplitude<< "\t"
                         << x->phase << "\n";
#endif
    }
}

bool SpectrumAnalyser::isReady() const {

    return (Idle == m_state);
}

void SpectrumAnalyser::cancelCalculation() {

    if (Busy == m_state)
        m_state = Cancelled;
}


//-----------------------------------------------------------------------------
// Private slots
//-----------------------------------------------------------------------------

void SpectrumAnalyser::calculationComplete(const FrequencySpectrum &spectrum) {

    Q_ASSERT(Idle != m_state);
    if (Busy == m_state)
        emit spectrumChanged(spectrum);
    m_state = Idle;
}

void SpectrumAnalyser::calculationTotalComplete(const QList<FrequencySpectrum> &m_spectrumList) {

	Q_ASSERT(Idle != m_state);
    if (Busy == m_state)
        emit spectrumListChanged(m_spectrumList);
    m_state = Idle;
}




