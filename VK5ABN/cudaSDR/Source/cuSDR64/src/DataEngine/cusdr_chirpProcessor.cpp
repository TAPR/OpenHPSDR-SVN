/**
* @file  cusdr_chirpProcessor.cpp
* @brief chirp data processor class
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-09-22
*/

/*   
 *   Copyright 2011 Hermann von Hasseln, DL3HVH
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

#define LOG_CHIRP_PROCESSOR

#include "cusdr_chirpProcessor.h"

ChirpProcessor::ChirpProcessor(THPSDRParameter *ioData)
	: QObject()
	, set(Settings::instance())
	, io(ioData)
	, m_stopped(false)
	, m_specMax(0.0f)
	, m_specMin(0.0f)
	, m_sampleRate(set->getSampleRate())
	, m_downRate(set->getChirpDownSampleRate())
	, m_specAvgLength(1)
	, m_filterLowerFrequency((float)set->getChirpFilterLowerFrequency())
	, m_filterUpperFrequency((float)set->getChirpFilterUpperFrequency())
	, m_switch(false)
	, m_showChirpFFT(false)
	, m_chirpBufferLength(set->getChirpBufferLength())
	, m_chirpSidebandFactor(1.0f)
{
	/*m_QCPXin = initQCPX(BUFFERSIZE);
	m_temp0  = initQCPX(BUFFERSIZE);
	m_temp2  = initQCPX(BUFFERSIZE/m_downRate);

	m_temp1  = initQCPX(FULL_BUFFERSIZE);
	m_temp3  = initQCPX(FULL_BUFFERSIZE);*/
	
	/*
	m_cpxIn			= mallocCPX(BUFFERSIZE);
	m_tmp0			= mallocCPX(BUFFERSIZE);
	m_tmp2			= mallocCPX(BUFFERSIZE/m_downRate);

	m_cpxInFilt		= mallocCPX(FULL_BUFFERSIZE);
	m_cpxRxFFT		= mallocCPX(FULL_BUFFERSIZE);
	m_tmp1			= mallocCPX(FULL_BUFFERSIZE);
	m_tmp3			= mallocCPX(FULL_BUFFERSIZE);
	m_cpxChirpIn	= mallocCPX(FULL_BUFFERSIZE);
	m_cpxChirpOut	= mallocCPX(FULL_BUFFERSIZE);
	m_cpxChirpTmp	= mallocCPX(FULL_BUFFERSIZE);
	m_cpxOut		= mallocCPX(FULL_BUFFERSIZE);
	*/
	m_cpxIn.resize(BUFFERSIZE);
	m_tmp0.resize(BUFFERSIZE);
	m_tmp2.resize(BUFFERSIZE/m_downRate);

	m_cpxInFilt.resize(FULL_BUFFERSIZE);
	m_cpxRxFFT.resize(FULL_BUFFERSIZE);
	m_tmp1.resize(FULL_BUFFERSIZE);
	m_tmp3.resize(FULL_BUFFERSIZE);
	m_cpxChirpIn.resize(FULL_BUFFERSIZE);
	m_cpxChirpOut.resize(FULL_BUFFERSIZE);
	m_cpxChirpTmp.resize(FULL_BUFFERSIZE);
	m_cpxOut.resize(FULL_BUFFERSIZE);


	/*
	memset(m_cpxIn, 0, BUFFERSIZE * sizeof(CPX));
	memset(m_tmp0, 0, BUFFERSIZE * sizeof(CPX));
	memset(m_tmp2, 0, (BUFFERSIZE/4) * sizeof(CPX));

	memset(m_cpxInFilt, 0, FULL_BUFFERSIZE * sizeof(CPX));
	memset(m_cpxRxFFT, 0, FULL_BUFFERSIZE * sizeof(CPX));
	memset(m_tmp1, 0, FULL_BUFFERSIZE * sizeof(CPX));
	memset(m_tmp3, 0, FULL_BUFFERSIZE * sizeof(CPX));
	memset(m_cpxOut, 0, FULL_BUFFERSIZE * sizeof(CPX));
	memset(m_cpxChirpIn, 0, FULL_BUFFERSIZE * sizeof(CPX));
	memset(m_cpxChirpOut, 0, FULL_BUFFERSIZE * sizeof(CPX));
	memset(m_cpxChirpTmp, 0, FULL_BUFFERSIZE * sizeof(CPX));
	*/

	// FFTs by fftw
	m_chirpFFT = new QFFT(FULL_BUFFERSIZE);
	m_matchedFFT = new QFFT(FULL_BUFFERSIZE);

	// FIR band pass filter
	//m_filter = new QFilter(this, FULL_BUFFERSIZE, 2);
	// 2 = FIR Bandpass, 5 = Bartlett window
	m_filter = new QFilter(this, BUFFERSIZE, 2, 2);

	// set the BPF 
	m_filter->setFilter(500.0f, 2500.0f);
	m_filter->setStreamMode(true);

	setupConnections();
}

ChirpProcessor::~ChirpProcessor() {

	while (average_queue.length() > 0)
		average_queue.dequeue();

	/*
	freeCPX(m_cpxIn);
	freeCPX(m_cpxOut);
	freeCPX(m_cpxInFilt);
	freeCPX(m_cpxRxFFT);
	freeCPX(m_cpxChirpIn);
	freeCPX(m_cpxChirpOut);
	freeCPX(m_tmp0);
	freeCPX(m_tmp1);
	*/

	/*
	delete m_cpxIn;
	delete m_cpxOut;
	delete m_cpxInFilt;
	delete m_cpxRxFFT;
	delete m_cpxChirpIn;
	delete m_cpxChirpOut;
	delete m_tmp0;
	delete m_tmp1;
	*/
	
	delete m_chirpFFT;
	delete m_matchedFFT;
}

void ChirpProcessor::stop() {

	m_stopped = true;
}

void ChirpProcessor::setupConnections() {

	CHECKED_CONNECT(
		set,
		SIGNAL(sampleRateChanged(QObject *, int)),
		this,
		SLOT(samplingRateChanged(QObject *, int)));

	CHECKED_CONNECT_OPT(
		set,
		SIGNAL(chirpFilterLowerFrequencyChanged(int)),
		this,
		SLOT(setFilterLowerFrequency(int)),
		Qt::DirectConnection);

	CHECKED_CONNECT_OPT(
		set,
		SIGNAL(chirpFilterUpperFrequencyChanged(int)),
		this,
		SLOT(setFilterUpperFrequency(int)),
		Qt::DirectConnection);

	CHECKED_CONNECT_OPT(
		set, 
		SIGNAL(chirpAvgLengthChanged(int)),
		this,
		SLOT(setDistSpectrumAvgLength(int)),
		Qt::DirectConnection);

	CHECKED_CONNECT_OPT(
		set, 
		SIGNAL(chirpFFTShowChanged(bool)),
		this,
		SLOT(setChirpFFTShow(bool)),
		Qt::DirectConnection);

	CHECKED_CONNECT_OPT(
		set, 
		SIGNAL(chirpSidebandChanged(bool)),
		this,
		SLOT(setChirpSideband(bool)),
		Qt::DirectConnection);
}

int ChirpProcessor::setSpectrumBufferSize(int size) {

	int i = 1;
	while (i < size) i *= 2;

	return i;
}

void ChirpProcessor::processChirpData() {

	forever {

		//matchedFilter(io->chirp_queue.dequeue());
		matchedFilterFIRFilter(io->chirp_queue.dequeue());
		
		m_mutex.lock();
		if (m_stopped) {
			m_stopped = false;
			m_mutex.unlock();
			break;
		}
		m_mutex.unlock();
	}
}

void ChirpProcessor::matchedFilterFIRFilter(QList<qreal> data) {

	int dataLength = data.length();
	int dataIdx = 0;
	int buffer = 0;
	int newsize = (int)(BUFFERSIZE / m_downRate);

	CHIRP_PROCESSOR_DEBUG << "start matched filter - samples:" << dataLength;

	// filtering to 2kHz by constant-overlap-add (COLA)
	while (dataIdx < dataLength) {
		
		dataIdx = 2 * buffer * BUFFERSIZE;
		for (int i = 0; i < 2 * BUFFERSIZE; i += 2) {
	
			if (dataIdx + i + 1 < dataLength) {
				
				m_cpxIn[i/2].re = data.at(dataIdx + i);
				m_cpxIn[i/2].im = data.at(dataIdx + i + 1);
			}
			else {

				dataIdx = dataLength;
				break;
			}
		}
		
		// BPF 2.5 kHz
		m_filter->ProcessFilter(m_cpxIn, m_tmp0, BUFFERSIZE);

		// decimate by 4
		decimate(m_tmp0, m_tmp2, BUFFERSIZE, m_downRate);

		for (int i = 0; i < newsize; i++) {

			m_cpxInFilt[buffer * newsize + i].re = m_tmp2[i].re;
			m_cpxInFilt[buffer * newsize + i].im = m_tmp2[i].im;
		}
		buffer++;
	}

	// Due to the COLA method the filter output gets shifet by BUFFERSIZE/2 bytes.
	// In order to have a buffer in full chirp length, we save the last newsize/2 bytes
	// (due to decimation by downrate) and add it to the beginning of the newbuffer.
	for (int i = 0; i < FULL_BUFFERSIZE - newsize/2; i++) {

		if (i < newsize/2) {

			m_cpxInFilt[i].re = m_tmp3[i].re;
			m_cpxInFilt[i].im = m_tmp3[i].im;
		}
		else {

			m_cpxInFilt[i].re = m_cpxInFilt[i + newsize/2].re;
			m_cpxInFilt[i].im = m_cpxInFilt[i + newsize/2].im;
		}
		
	}

	// save the last buffer
	for (int i = 0; i < newsize/2; i++) {

		m_tmp3[i].re = m_tmp2[i].re;
		m_tmp3[i].im = m_tmp2[i].im;
	}

	// map rx signal to the frequency domain
	m_matchedFFT->DoFFTWForward(m_cpxInFilt, m_cpxRxFFT, FULL_BUFFERSIZE);
	
	// multiply the chirp signal with the complex conjugate of the received signal
	for (int i = 0; i < FULL_BUFFERSIZE; i++) {
		
		// f * ~g
		m_tmp1[i].re = (m_cpxChirpOut[i].re * m_cpxRxFFT[i].re) + (m_cpxChirpOut[i].im * m_cpxRxFFT[i].im);
		m_tmp1[i].im = (m_cpxChirpOut[i].im * m_cpxRxFFT[i].re) - (m_cpxChirpOut[i].re * m_cpxRxFFT[i].im);

		// ~f * g
		//m_tmp1[i].re = (m_cpxRxFFT[i].re * m_cpxChirpOut[i].re) + (m_cpxRxFFT[i].im * m_cpxChirpOut[i].im);
		//m_tmp1[i].im = (m_cpxRxFFT[i].im * m_cpxChirpOut[i].re) - (m_cpxRxFFT[i].re * m_cpxChirpOut[i].im);

		// f * g
		//m_tmp1[i].re = (m_cpxChirpOut[i].re * m_cpxRxFFT[i].re) - (m_cpxChirpOut[i].im * m_cpxRxFFT[i].im);
		//m_tmp1[i].im = (m_cpxChirpOut[i].re * m_cpxRxFFT[i].im) + (m_cpxChirpOut[i].im * m_cpxRxFFT[i].re);
	}
	
	// map back to time domain
	m_matchedFFT->DoFFTWInverse(m_tmp1, m_cpxOut, FULL_BUFFERSIZE);
	
	float max = -1000;
	float min = 1000;
	float mean = 0.0f;
	float oneOverNorm = 1.0f / FULL_BUFFERSIZE;
	
	for (int i = 0; i < FULL_BUFFERSIZE; i++) {
	
		if (!m_showChirpFFT) {
			
			m_spectrumBufferFull[i] = (float)(10.0 * log10(MagCPX(ScaleCPX(m_cpxOut[i], oneOverNorm)) + 1.5E-45));
			
			if (m_spectrumBufferFull[i] > max) max = m_spectrumBufferFull[i];
			if (m_spectrumBufferFull[i] < min) min = m_spectrumBufferFull[i];
			mean += m_spectrumBufferFull[i];
		}
		else {
			//m_spectrumBufferFull[i] = (float)(10.0 * log10(SqrMagCPX(m_tmp1[i]) + 1.5E-45));
			//m_spectrumBufferFull[i] = (float)(10.0 * log10(SqrMagCPX(ScaleCPX(m_tmp1[i], oneOverNorm)) + 1.5E-45));
			m_spectrumBufferFull[i] = (float)(10.0 * log10(MagCPX(m_cpxRxFFT[i]) + 1.5E-45));
			//m_spectrumBufferFull[i] = (float)(10.0 * log10(SqrMagCPX(m_cpxChirpOut[i]) + 1.5E-45) + 50.0f);
			//m_spectrumBufferFull[i] = (float)(10.0 * log10(SqrMagCPX(SubCPX(m_cpxChirpOut[i], m_cpxRxFFT[i])) + 1.5E-45) + 50.0f);
			//m_spectrumBufferFull[i] = (float)(10.0 * log10(MagCPX(m_cpxInFilt[i]) + 1.5E-45));
			//m_spectrumBufferFull[i] = (float)(10.0 * log10(MagCPX(m_cpxIn[i]) + 1.5E-45));
		}
	}

	mean *= 1.0f/FULL_BUFFERSIZE;
	CHIRP_PROCESSOR_DEBUG << "dist min" << min << "max" << max << "mean" << mean;
	CHIRP_PROCESSOR_DEBUG << "dist delta" << max - mean;

	// we take the first half of the matched filter output to display
	memcpy(m_spectrumBuffer, m_spectrumBufferFull, (FULL_BUFFERSIZE/2) * sizeof(float));
	
	// we take the full length for the frequency spectrum, 
	// because we want to see positive as well as negative spectras
	if (m_showChirpFFT) {

		int topsize = FULL_BUFFERSIZE - 1;
		
		// reorder the RX FFT buffer
		for (int i = 0; i < FULL_BUFFERSIZE/2; i++) {
			
			m_fftSpectrumBuffer[topsize - i] = m_spectrumBufferFull[i + FULL_BUFFERSIZE/2];
			m_fftSpectrumBuffer[FULL_BUFFERSIZE/2 - i] = m_spectrumBufferFull[i];
		}
	}
	setSpectras(m_spectrumBuffer, m_fftSpectrumBuffer);
}

void ChirpProcessor::setSpectras(const float *distance, const float *chirpfft) {

	if (m_showChirpFFT)
		set->setChirpSpectrumBuffer(m_sampleRate/m_downRate, FULL_BUFFERSIZE, chirpfft);
		
	else {
		if (m_specAvgLength > 1)
			//spectrumAveraging(FULL_BUFFERSIZE, distance);
			spectrumAveraging(FULL_BUFFERSIZE/2, distance);
		else
			//set->setChirpSpectrumBuffer(m_sampleRate/m_downRate, FULL_BUFFERSIZE, distance);
			set->setChirpSpectrumBuffer(m_sampleRate/m_downRate, FULL_BUFFERSIZE/2, distance);
	}		
}

void ChirpProcessor::spectrumAveraging(qint64 length, const float *buffer) {

	QVector<float>	m_specBuf(length);

	m_mutex.lock();

	memcpy(
		(float *) m_specBuf.data(),
		(float *) &buffer[0],
		length * sizeof(float));

	average_queue.enqueue(m_specBuf);

	float specMax = 0.0f;
	float specMin = 0.0f;
	float specMean = 0.0f;
	if (average_queue.size() <= m_specAvgLength) {
	
		for (int i = 0; i < length; i++) {

			m_tmpBuf[i] += average_queue.last().data()[i];
			m_outBuf[i] = m_tmpBuf[i] * (1.0f/average_queue.size());
			specMean += m_outBuf[i];

			if (m_outBuf[i] > specMax) specMax = m_outBuf[i];
			if (m_outBuf[i] < specMin) specMin = m_outBuf[i];
		}

		m_mutex.unlock();

		specMean *= 1.0f/FULL_BUFFERSIZE;
		CHIRP_PROCESSOR_DEBUG << "distance spectrum averaging size" << average_queue.size();
		CHIRP_PROCESSOR_DEBUG << "specMin =" << specMin << "specMax =" << specMax;
		CHIRP_PROCESSOR_DEBUG << "distance spectrum mean value" << specMean;
		set->setChirpSpectrumBuffer(m_sampleRate, FULL_BUFFERSIZE, m_outBuf);
		return;
	}
	
	for (int i = 0; i < length; i++) {

		m_tmpBuf[i] -= average_queue.first().at(i);
		m_tmpBuf[i] += average_queue.last().at(i);
		m_outBuf[i] = m_tmpBuf[i] * m_scale;
		specMean += m_outBuf[i];
	}
	
	//set->setChirpSpectrumBuffer(m_sampleRate, FULL_BUFFERSIZE, m_outBuf);
	set->setChirpSpectrumBuffer(m_sampleRate, FULL_BUFFERSIZE/2, m_outBuf);
	average_queue.dequeue();
	
	m_mutex.unlock();
}

void ChirpProcessor::generateLocalChirp() {

	const int sampleRate = m_sampleRate / m_downRate;
    
	//memset(m_cpxChirpIn, 0, FULL_BUFFERSIZE * sizeof(CPX));
	//memset(m_cpxChirpOut, 0, FULL_BUFFERSIZE * sizeof(CPX));
	m_cpxChirpIn.resize(FULL_BUFFERSIZE);
	m_cpxChirpOut.resize(FULL_BUFFERSIZE);


	qreal time =  set->getChirpBufferDurationUs() / 1.0E6;
	qint64 length = (qint64)(sampleRate * time);
	
	qreal a = ONEPI * (set->getUpperChirpFreq() - set->getLowerChirpFreq()) / time;
	qreal b = TWOPI * set->getLowerChirpFreq();

	for (int i = 0; i < length; i++) {

		// forward chirp
		qreal t = (qreal)(1.0f * i/length);
		// backward chirp
		//qreal t = (qreal)(1.0f * (length-i)/length);
		
		// complex chirp signal
		m_cpxChirpIn[i].re = qSin(a * t * t + b * t);
		m_cpxChirpIn[i].im = qCos(a * t * t + b * t);

		//m_cpxChirpTmp[i].re = qSin(a * t * t + b * t);
		//m_cpxChirpTmp[i].im = qCos(a * t * t + b * t);

		//m_cpxChirpIn[i].re = qCos(a * t * t + b * t);
		//m_cpxChirpIn[i].im = qSin(a * t * t + b * t);

		//m_cpxChirpIn[i].re = qSin(a * t * t + b * t);
		//m_cpxChirpIn[i].im = -qCos(a * t * t + b * t);
	}

	//m_specBufferSize = setSpectrumBufferSize(length);
	
	// BPF 2.5 kHz
	//m_filter->ProcessFilter(m_cpxChirpTmp, m_cpxChirpIn, FULL_BUFFERSIZE);

	// transform chirp signal to frequency domain
	m_chirpFFT->DoFFTWForward(m_cpxChirpIn, m_cpxChirpOut, FULL_BUFFERSIZE);

	float dur = set->getChirpBufferDurationUs() / 1000.0f;

	CHIRP_PROCESSOR_DEBUG	<< "chirp buffer changed:";
	CHIRP_PROCESSOR_DEBUG	<< "  bufferLength" << length;
	//CHIRP_PROCESSOR_DEBUG	<< "  spectrum buffer size" << m_specBufferSize;
	CHIRP_PROCESSOR_DEBUG	<< "  start frequency (Hz)" << set->getLowerChirpFreq();
	CHIRP_PROCESSOR_DEBUG	<< "  end frequency (Hz)" << set->getUpperChirpFreq();
	CHIRP_PROCESSOR_DEBUG	<< "  duration (ms)" << dur;
}

//void ChirpProcessor::decimate(CPX *in, CPX *out, int size, int downrate) {
void ChirpProcessor::decimate(const CPX &in, CPX &out, int size, int downrate) {

	int newsize = size / downrate;

	//memset(out, 0, newsize * sizeof(CPX));
	out.resize(newsize);

	for (int j = 0; j < newsize; j++) {
		for (int k = 0; k < downrate; k++) {
		
			if (j * downrate + k < size) {

				/*out[j].re += m_tmpDec[j * downrate + k].re;
				out[j].im += m_tmpDec[j * downrate + k].im;*/
				out[j].re += in[j * downrate + k].re;
				out[j].im += in[j * downrate + k].im;
			}
		}
	}
}

//void ChirpProcessor::decimate(QList<qreal> data, CPX *out, int downrate) {
void ChirpProcessor::decimate(const QList<qreal> &data, CPX &out, int downrate) {

	//memset(out, 0, 16 * BUFFER_SIZE * sizeof(CPX));
	out.resize(16 * BUFFER_SIZE);
	
	for (int j = 0; j < data.length()/4; j += 2) {
		for (int k = 0; k < downrate; k++) {
		
			if (j * downrate + 2*k < data.length()) {

				out[j/2].re += data.at(j * downrate + 2*k);
				out[j/2].im += data.at(j * downrate + 2*k + 1);
			}
		}
	}
}

void ChirpProcessor::samplingRateChanged(QObject *sender, int value) {

	Q_UNUSED(sender)

	m_sampleRate = value;
}

void ChirpProcessor::setDistSpectrumAvgLength(int value) {

	m_mutex.lock();

		//for (int i = 0; i < SAMPLE_BUFFER_SIZE; i++) m_tmpBuf[i] = 0.0f;
		memset(m_tmpBuf, 0, FULL_BUFFERSIZE * sizeof(float));

		while (!average_queue.isEmpty())
			average_queue.dequeue();

		m_specAvgLength = value;

		if (m_specAvgLength > 0)
			m_scale = 1.0f / m_specAvgLength;
		else
			m_scale = 1.0f;

	m_mutex.unlock();
}

void ChirpProcessor::setChirpFFTShow(bool value) {

	m_showChirpFFT = value;

}

void ChirpProcessor::setChirpSideband(bool value) {

	if (value)
		m_chirpSidebandFactor = 1.0f;
	else
		m_chirpSidebandFactor = -1.0f;

	generateLocalChirp();
}

void ChirpProcessor::setFilterLowerFrequency(int value) {

	if ((int)m_filterLowerFrequency == value) return;
	m_filterLowerFrequency = 1.0f * value;

	m_filter->setFilter(m_filterLowerFrequency, m_filterUpperFrequency);
}

void ChirpProcessor::setFilterUpperFrequency(int value) {

	if ((int)m_filterUpperFrequency == value) return;
	m_filterUpperFrequency = 1.0f * value;

	m_filter->setFilter(m_filterLowerFrequency, m_filterUpperFrequency);
}
