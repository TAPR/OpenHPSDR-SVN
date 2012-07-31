/**
* @file  cusdr_DttSP.cpp
* @brief DttSP interface class
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
#define LOG_DTTSP_API

#include "cusdr_DttSP.h"
#include <windows.h>


QDttSP::QDttSP(QObject *parent, int thread)
	: QObject(parent)
	, m_thread(thread)
	, m_settings(Settings::instance())
	, m_dttspOn(false)
{
	// horrible hack to load the 7 different dll's for DttSP
	QString str = "DttSP%1.dll";
	str = str.arg(m_thread);
	QByteArray ba = str.toUtf8();
	m_cString = ba.constData();

	dttspSetupSDR();
}

QDttSP::~QDttSP() {

	FreeLibrary(m_hinstLib);
}

void QDttSP::dttspSetupSDR() {

	// load DttSP dll
	m_hinstLib = LoadLibraryA(m_cString);
	//m_hinstLib = LoadLibrary("DttSP.dll");
	
	if (m_hinstLib != NULL) {

        // query the procedure addresses
        Setup_SDR = (setupSDR_t) GetProcAddress(m_hinstLib, "Setup_SDR");
		Release_Update = (releaseUpdate_t) GetProcAddress(m_hinstLib, "Release_Update");
		Destroy_SDR = (destroySDR_t) GetProcAddress(m_hinstLib, "Destroy_SDR");
		process_samples_thread = (processSamplesThread_t) GetProcAddress(m_hinstLib, "process_samples_thread");
		SetThreadCom = (setThreadCom_t) GetProcAddress(m_hinstLib, "SetThreadCom");
		SetTRX = (setTRX_t) GetProcAddress(m_hinstLib, "SetTRX");
		SetDSPBuflen = (setDSPBuflen_t) GetProcAddress(m_hinstLib, "SetDSPBuflen");
		SetSubRXSt = (setSubRXSt_t) GetProcAddress(m_hinstLib, "SetSubRXSt");
		SetThreadProcessingMode = (setThreadProcessingMode_t) GetProcAddress(m_hinstLib, "SetThreadProcessingMode");
		Audio_Callback = (audioCallback_t) GetProcAddress(m_hinstLib, "Audio_Callback");
		Audio_Callback2 = (audioCallback2_t) GetProcAddress(m_hinstLib, "Audio_Callback2");
		Process_Spectrum = (processSpectrum_t) GetProcAddress(m_hinstLib, "Process_Spectrum");
		Process_Panadapter = (processPanadapter_t) GetProcAddress(m_hinstLib, "Process_Panadapter");
		Process_Phase = (processPhase_t) GetProcAddress(m_hinstLib, "Process_Phase");
		Process_Scope = (processScope_t) GetProcAddress(m_hinstLib, "Process_Scope");
		SetSpectrumPolyphase = (setSpectrumPolyphase_t) GetProcAddress(m_hinstLib, "SetSpectrumPolyphase");
		CalculateRXMeter = (calculateRXMeter_t) GetProcAddress(m_hinstLib, "CalculateRXMeter");
		SetSampleRate = (setSampleRate_t) GetProcAddress(m_hinstLib, "SetSampleRate");
		SetRXOsc = (setRXOsc_t) GetProcAddress(m_hinstLib, "SetRXOsc");
		SetRXOutputGain = (setRXOutputGain_t) GetProcAddress(m_hinstLib, "SetRXOutputGain");
		SetRXPan = (setRXPan_t) GetProcAddress(m_hinstLib, "SetRXPan");
		SetRingBufferOffset = (setRingBufferOffset_t) GetProcAddress(m_hinstLib, "SetRingBufferOffset");
		SetMode = (setMode_t) GetProcAddress(m_hinstLib, "SetMode");
		SetRXFilter = (setRXFilter_t) GetProcAddress(m_hinstLib, "SetRXFilter");
		SetRXAGC = (setRXAGC_t) GetProcAddress(m_hinstLib, "SetRXAGC");
		SetRXAGCTop = (setRXAGCTop_t) GetProcAddress(m_hinstLib, "SetRXAGCTop");
		SetFixedAGC = (setFixedAGC_t) GetProcAddress(m_hinstLib, "SetFixedAGC");
		SetNR = (setNR_t) GetProcAddress(m_hinstLib, "SetNR");
		SetNB = (setNB_t) GetProcAddress(m_hinstLib, "SetNB");
		SetANF = (setANF_t) GetProcAddress(m_hinstLib, "SetANF");
 
        // set up DttSP
		//if (process_samples_thread != NULL)
		//	(*process_samples_thread)(m_thread);
		//else
		//	DTTSP_API_DEBUG << "failure calling process_samples_thread.";

		if (Setup_SDR != NULL) 
			(*Setup_SDR)();
		else
			DTTSP_API_DEBUG << "failure calling Setup_SDR.";
			
		if (Release_Update != NULL) 
			(*Release_Update)();
		else
			DTTSP_API_DEBUG << "failure calling Release_Update.";

	}
	else
		DTTSP_API_DEBUG << "could not load dttsp.dll.";
}

void QDttSP::dttspReleaseUpdate() {

	if (Release_Update != NULL) 
		Release_Update();
	else
		DTTSP_API_DEBUG << "failure calling Release_Update.";
}

void QDttSP::dttspDestroySDR() {

	if (Destroy_SDR != NULL) 
		Destroy_SDR();
	else
		DTTSP_API_DEBUG << "failure calling Destroy_SDR.";
}

void QDttSP::dttspProcessSamplesThread(unsigned int proc_thread) {

	if (process_samples_thread != NULL) 
		(*process_samples_thread)(proc_thread);
	else
		DTTSP_API_DEBUG << "failure calling process_samples_thread.";
}

void QDttSP::dttspSetThreadCom(int thread) {

	if (SetThreadCom != NULL)
		(*SetThreadCom)(thread);
	else
		DTTSP_API_DEBUG << "failure calling SetThreadCom.";
}

void QDttSP::dttspSetTRX(uint thread, bool trx_on) {

	// thread 0 is for receive; 1st arg = thread; 2nd arg: 0 = Rx,  1 = Tx
	if (SetTRX != NULL)
		(*SetTRX)(thread, trx_on);
	else
		DTTSP_API_DEBUG << "failure calling SetTRX.";
}

void QDttSP::dttspSetDSPBuflen(unsigned int thread, int newBuffSize) {

	if (SetDSPBuflen != NULL)
		(*SetDSPBuflen)(thread, newBuffSize);
	else
		DTTSP_API_DEBUG << "failure calling SetDSPBuflen.";
}

void QDttSP::dttspSetSubRXSt(uint thread, uint subrx, bool setit) {

	// 1st arg = thread; 2nd arg = sub-receiver number; 3rd arg: 0 = inactive, 1 = active
	if (SetSubRXSt != NULL)
		(*SetSubRXSt)(thread, subrx, setit);
	else
		DTTSP_API_DEBUG << "failure calling SetSubRXSt.";
}

void QDttSP::dttspSetThreadProcessingMode(uint thread, int runmode) {

	// 1st arg = thread; 2nd arg = RunMode: 0 = RUN_MUTE, 1 = RUN_PASS, 2 = RUN_PLAY, 3 = RUN_SWCH
	if (SetThreadProcessingMode != NULL)
		(*SetThreadProcessingMode)(thread, runmode);
	else
		DTTSP_API_DEBUG << "failure calling SetThreadProcessingMode.";
}

void QDttSP::dttspSetRXAGC(uint thread, uint subrx, AGCMode setit) {

	if (SetRXAGC != NULL)
		(*SetRXAGC)(thread, subrx, setit);
	else
		DTTSP_API_DEBUG << "failure calling SetRXAGC.";
}

void QDttSP::dttspSetRXAGCTop(uint thread, uint subrx, double max_agc) {

	if (SetRXAGCTop != NULL)
		(*SetRXAGCTop)(thread, subrx, max_agc);
	else
		DTTSP_API_DEBUG << "failure calling SetRXAGCTop.";
}

void QDttSP::dttspSetFixedAGC(uint thread, uint subrx, double fixed_agc) {

	if (SetFixedAGC != NULL)
		(*SetFixedAGC)(thread, subrx, fixed_agc);
	else
		DTTSP_API_DEBUG << "failure calling SetFixedAGC.";
}

int	QDttSP::dttspSetSampleRate(double sampleRate) {

	if (SetSampleRate != NULL)
		return (*SetSampleRate)(sampleRate);

	else {
		DTTSP_API_DEBUG << "failure calling SetSampleRate.";
		return 0;
	}
}

int	QDttSP::dttspSetRXOsc(unsigned int thread, unsigned subrx, double freq) {

	if (SetRXOsc != NULL)
		return (*SetRXOsc)(thread, subrx, freq);

	else {
		DTTSP_API_DEBUG << "failure calling SetRXOsc.";
		return 0;
	}
}

int	QDttSP::dttspSetRXOutputGain(unsigned int thread, unsigned subrx, double gain) {

	// 1st arg = thread; 2nd arg = sub-receiver number; 3rd arg = audio gain from 0.0 to 1.0
	if (SetRXOutputGain != NULL)
		return (*SetRXOutputGain)(thread, subrx, gain);

	else {
		DTTSP_API_DEBUG << "failure calling SetRXOutputGain.";
		return 0;
	}
}

int	QDttSP::dttspSetMode(uint thread, uint subrx, DSPMode m) {

	if (SetMode != NULL)
		return (*SetMode)(thread, subrx, m);

	else {
		DTTSP_API_DEBUG << "failure calling SetMode.";
		return 0;
	}
}

int	QDttSP::dttspSetRXFilter(uint thread, uint subrx, double low, double high) {

	if (SetRXFilter != NULL)
		return (*SetRXFilter)(thread, subrx, low, high);

	else {
		DTTSP_API_DEBUG << "failure calling SetRXFilter.";
		return 0;
	}
}

int	QDttSP::dttspSetRingBufferOffset(unsigned int thread, int offset) {

	if (SetRingBufferOffset != NULL)
		return (*SetRingBufferOffset)(thread, offset);

	else {
		DTTSP_API_DEBUG << "failure calling SetRingBufferOffset.";
		return 0;
	}
}

void QDttSP::dttspAudioCallback(
		float *input_l, 
		float *input_r, 
		float *output_l,
		float *output_r, 
		unsigned int nframes, 
		int thread)
{
	if (Audio_Callback != NULL)
		(*Audio_Callback)(input_l, input_r, output_l, output_r, nframes, thread);
	else
		DTTSP_API_DEBUG << "failure calling Audio_Callback.";
}

void QDttSP::dttspAudioCallback2(
		float **input, 
		float **output, 
		unsigned int nframes)
{
	if (Audio_Callback2 != NULL)
		(*Audio_Callback2)(input, output, nframes);
	else
		DTTSP_API_DEBUG << "failure calling Audio_Callback2.";
}

void QDttSP::dttspProcessSpectrum(int thread, float *results) {

	if (Process_Spectrum != NULL)
		(*Process_Spectrum)(thread, results);
	else
		DTTSP_API_DEBUG << "failure calling Process_Spectrum.";
}

void QDttSP::dttspProcessPanadapter(int thread, float *results) {

	if (Process_Panadapter != NULL)
		(*Process_Panadapter)(thread, results);
	else
		DTTSP_API_DEBUG << "failure calling Process_Panadapter.";
}

void QDttSP::dttspProcessPhase(int thread, float *results, int numpoints) {

	if (Process_Phase != NULL)
		(*Process_Phase)(thread, results, numpoints);
	else
		DTTSP_API_DEBUG << "failure calling Process_Phase.";
}

void QDttSP::dttspProcessScope(int thread, float *results, int numpoints) {

	if (Process_Scope != NULL)
		(*Process_Scope)(thread, results, numpoints);
	else
		DTTSP_API_DEBUG << "failure calling Process_Scope.";
}

void QDttSP::dttspSetSpectrumPolyphase(unsigned int thread, bool setit) {

	if (SetSpectrumPolyphase != NULL)
		(*SetSpectrumPolyphase)(thread, setit);
	else
		DTTSP_API_DEBUG << "failure calling SetSpectrumPolyphase.";
}

void QDttSP::dttspSetSpectrumWindow(unsigned int thread, TWindowtype type) {

	if (SetWindow != NULL)
		(*SetWindow)(thread, type);
	else
		DTTSP_API_DEBUG << "failure calling SetWindow.";
}

float QDttSP::dttspCalculateRXMeter(int thread, unsigned int subrx, int mt) {

	if (CalculateRXMeter != NULL)
		return (*CalculateRXMeter)(thread, subrx, mt);

	else {
		DTTSP_API_DEBUG << "failure calling CalculateRXMeter.";
		return 0;
	}
}

void QDttSP::newMessage(QString msg) {

	msg.prepend("[DttSP]: ");

	emit messageEvent(msg);
}