/**
* @file  cusdr_DttSP.h
* @brief DttSP interface header file
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-11-06
*/

/*
 *   
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

#ifndef CUSDR_DTTSP_H
#define CUSDR_DTTSP_H

#include <QtGlobal>
#include <QObject>
#include <QMetaType>

#include "cusdr_settings.h"

//typedef enum _windowtype {
//
//  RECTANGULAR_WINDOW,
//  HANNING_WINDOW,
//  WELCH_WINDOW,
//  PARZEN_WINDOW,
//  BARTLETT_WINDOW,
//  HAMMING_WINDOW,
//  BLACKMAN2_WINDOW,
//  BLACKMAN3_WINDOW,
//  BLACKMAN4_WINDOW,
//  EXPONENTIAL_WINDOW,
//  RIEMANN_WINDOW,
//  BLACKMANHARRIS_WINDOW,
//  NUTTALL_WINDOW
//
//} Windowtype;

typedef void (*setupSDR_t) (void) ;
typedef void (*releaseUpdate_t) (void);
typedef void (*destroySDR_t) (void);
typedef void (*processSamplesThread_t) (unsigned int);
typedef void (*setThreadCom_t) (int);
typedef void (*audioCallback_t) (float *, float *, float *, float *, unsigned int, int);
typedef void (*audioCallback2_t) (float **, float **, unsigned int);
typedef	void (*processSpectrum_t) (int, float *);
typedef	void (*processPanadapter_t) (int, float *);
typedef void (*processPhase_t) (int, float *, int);
typedef	void (*processScope_t) (int, float *, int);
typedef	void (*setSpectrumPolyphase_t) (unsigned int, bool);
typedef	float (*calculateRXMeter_t) (int, unsigned int, int);
typedef	int (*setSampleRate_t) (double);
typedef	int (*setRXOsc_t) (unsigned int, unsigned, double);
typedef	int (*setRXOutputGain_t) (unsigned int, unsigned, double);
typedef	int (*setRXPan_t) (unsigned int, unsigned, float);
typedef	int (*setRingBufferOffset_t) (unsigned int, int);
typedef	int (*setMode_t) (uint, uint, DSPMode);
typedef	int (*setRXFilter_t) (uint, uint, double, double);
typedef	void (*setRXAGC_t) (uint, uint, AGCMode);
typedef	void (*setRXAGCTop_t) (uint, uint, double);
typedef void (*setFixedAGC_t) (uint, uint, double);
typedef	void (*setNR_t) (uint, uint, bool);
typedef	void (*setNB_t) (uint, uint, bool);
typedef	void (*setANF_t) (uint, uint, bool);
typedef	void (*setTRX_t) (uint, bool);
typedef	void (*setThreadProcessingMode_t) (uint, int);
typedef	void (*setSubRXSt_t) (uint, uint, bool);
typedef	void (*setDSPBuflen_t) (unsigned int, int);
typedef void (*setWindow_t) (unsigned int, TWindowtype type);

typedef enum _trxmode { 
	
	RX, 
	TX 

} TRXMODE;

class QDttSP : public QObject {

	Q_OBJECT

public:
	QDttSP(QObject *parent = 0, int thread = 0);
	~QDttSP();

private:
	Settings					*m_settings;

	HINSTANCE					m_hinstLib;

	processSamplesThread_t		process_samples_thread;
	setupSDR_t					Setup_SDR;
	releaseUpdate_t				Release_Update;
	destroySDR_t				Destroy_SDR;

	audioCallback_t				Audio_Callback;
	audioCallback2_t			Audio_Callback2;
	processSpectrum_t			Process_Spectrum;
	processPanadapter_t			Process_Panadapter;
	processPhase_t				Process_Phase;
	processScope_t				Process_Scope;
	setSpectrumPolyphase_t		SetSpectrumPolyphase;
	calculateRXMeter_t			CalculateRXMeter;

	setThreadCom_t				SetThreadCom;
	setSampleRate_t				SetSampleRate;
	setRXOsc_t					SetRXOsc;
	setRXOutputGain_t			SetRXOutputGain;
	setRXPan_t					SetRXPan;
	setRingBufferOffset_t		SetRingBufferOffset;
	setMode_t					SetMode;
	setRXFilter_t				SetRXFilter;
	setRXAGC_t					SetRXAGC;
	setRXAGCTop_t				SetRXAGCTop;
	setFixedAGC_t				SetFixedAGC;
	setNR_t						SetNR;
	setNB_t						SetNB;
	setANF_t					SetANF;
	setTRX_t					SetTRX;
	setThreadProcessingMode_t	SetThreadProcessingMode;
	setSubRXSt_t				SetSubRXSt;
	setDSPBuflen_t				SetDSPBuflen;
	setWindow_t					SetWindow;

	QString			m_dllString;
	const char*		m_cString;
	bool			m_dttspOn;

	int				m_thread;

private slots:
	void		newMessage(QString msg);

public slots:
	void		setDttSPStatus(bool value) { m_dttspOn = value; }
	bool		getDttSPStatus() { return m_dttspOn; }

	void		dttspSetupSDR();
	void		dttspReleaseUpdate();
	void		dttspDestroySDR();
	void		dttspSetThreadCom(int thread);
	void		dttspSetTRX(uint thread, bool trx_on);
	void		dttspSetSubRXSt(uint thread, uint subrx, bool setit);
	void		dttspSetDSPBuflen(unsigned int thread, int newBuffSize);
	void		dttspSetThreadProcessingMode(uint thread, int runmode);

	int			dttspSetSampleRate(double sampleRate);
	int			dttspSetRXOsc(unsigned int thread, unsigned subrx, double freq);
	int			dttspSetRXOutputGain(unsigned int thread, unsigned subrx, double gain);
	int			dttspSetMode(uint thread, uint subrx, DSPMode m);
	int			dttspSetRXFilter(uint thread, uint subrx, double low, double high);
	int			dttspSetRingBufferOffset(unsigned int thread, int offset);

	void		dttspSetRXAGC(uint thread, uint subrx, AGCMode setit);
	void		dttspSetRXAGCTop(uint thread, uint subrx, double max_agc);
	void		dttspSetFixedAGC(uint thread, uint subrx, double fixed_agc);

	void		dttspAudioCallback(
							float *input_l, 
							float *input_r, 
							float *output_l,
							float *output_r, 
							unsigned int nframes, 
							int thread);

	void		dttspAudioCallback2(
							float **input, 
							float **output, 
							unsigned int nframes);

	void		dttspProcessSpectrum(int thread, float *results);
	void		dttspProcessPanadapter(int thread, float *results);

	void		dttspProcessPhase(int thread, float *results, int numpoints);
	void		dttspProcessScope(int thread, float *results, int numpoints);

	void		dttspSetSpectrumPolyphase(unsigned int thread, bool setit);
	void		dttspProcessSamplesThread (unsigned int proc_thread);
	void		dttspSetSpectrumWindow(unsigned int thread, TWindowtype type);
	
	float		dttspCalculateRXMeter(int thread, unsigned int subrx, int mt);


signals:
	void		messageEvent(QString message);
};

#endif  //  CUSDR_DTTSP_H