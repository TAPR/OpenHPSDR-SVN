/**
* @file  cusdr_chirpProcessor.h
* @brief chirp processor header file
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

#ifndef _CUSDR_CHIRPPROCESSOR_H
#define _CUSDR_CHIRPPROCESSOR_H

#include <QObject>
#include <QMutex>
#include <QByteArray>
#include <QBuffer>
#include <QVector>
#include <QList>
#include <QWaitCondition>
#include <QThread>

#include "cusdr_settings.h"
#include "cusdr_complex.h"
#include "cusdr_filter.h"
#include "cusdr_fft.h"

#define FULL_BUFFERSIZE		16384//65536
#define HALF_BUFFERSIZE		32768
#define BUFFERSIZE			2048

class ChirpProcessor : public QObject {

    Q_OBJECT

public:
	ChirpProcessor(THPSDRParameter *ioData = 0);
	~ChirpProcessor();

public slots:
	void	stop();
	void	processChirpData();
	void	generateLocalChirp();
	
private slots:
	void	matchedFilterFIRFilter(QList<qreal> data);
	void	samplingRateChanged(QObject *sender, int value);
	void	setSpectras(const float *distance, const float *chirpfft);
	void	setDistSpectrumAvgLength(int value);
	void	setChirpFFTShow(bool value);
	void	setChirpSideband(bool value);
	void	setFilterLowerFrequency(int value);
	void	setFilterUpperFrequency(int value);
		
private:
	Settings*	m_settings;

	QMutex		m_mutex;
	QString		m_message;

	QFFT*		m_chirpFFT;
	QFFT*		m_matchedFFT;
	QFilter*	m_filter;

	CPX*		m_tmp0;
	CPX*		m_tmp1;
	CPX*		m_tmp2;
	CPX*		m_tmp3;
	CPX*		m_tmpDec;

	CPX*		m_cpxRxFFT;
	CPX*		m_cpxChirpIn;
	CPX*		m_cpxChirpTmp;
	CPX*		m_cpxChirpOut;
	CPX*		m_cpxIn;
	CPX*		m_cpxInFilt;
	CPX*		m_cpxOut;
	
	THPSDRParameter	*io;

	QQueue<QVector<float> >  average_queue;

	float			m_tmpBuf[FULL_BUFFERSIZE];
	float			m_outBuf[FULL_BUFFERSIZE];
	float			m_spectrumBuffer[FULL_BUFFERSIZE];
	float			m_fftSpectrumBuffer[FULL_BUFFERSIZE];
	float			m_spectrumBufferFull[FULL_BUFFERSIZE];
	float			*m_window;

	volatile bool	m_stopped;

	float		m_specMax;
	float		m_specMin;
	float		m_scale;

	int			m_sampleRate;
	int			m_downSampleRate;
	int			m_downRate;
	int			m_specBufferSize;
	int			m_specAvgLength;
	float		m_filterLowerFrequency;
	float		m_filterUpperFrequency;
	
	bool		m_switch;
	bool		m_showChirpFFT;


	qint64		m_chirpBufferLength;
	qreal		m_chirpSidebandFactor;

	void		setupConnections();
	void		decimate(CPX *in, CPX *out, int size, int downrate);
	void		decimate(QList<qreal> data, CPX *out, int downrate);
	void		spectrumAveraging(qint64 length, const float *buffer);

	int			setSpectrumBufferSize(int size);

signals:
	void	messageEvent(QString message);
};

#endif // _CUSDR_CHIRPPROCESSOR_H