/**
* @file cusdr_receiver.cpp
* @brief cuSDR receiver class
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-11-12
*/

/* Copyright (C)
*
* 2010 - Hermann von Hasseln, DL3HVH
*
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/
#define LOG_RECEIVER

#include "cusdr_receiver.h"

HPSDRReceiver::HPSDRReceiver(QObject *parent)
	: QObject(parent)
	, m_settings(Settings::instance())
	, m_filterMode(m_settings->getCurrentFilterMode())
{
	setupConnections();
}

HPSDRReceiver::~HPSDRReceiver() {
}

void HPSDRReceiver::setupConnections() {

	CHECKED_CONNECT(
		m_settings, 
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
		m_settings, 
		SIGNAL(mainVolumeChanged(QObject *, int, float)), 
		this, 
		SLOT(setAudioVolume(QObject *, int, float)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(dspModeChanged(QObject *, int, DSPMode)), 
		this, 
		SLOT(setDspMode(QObject *, int, DSPMode)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(hamBandChanged(QObject *, int, HamBand)), 
		this, 
		SLOT(setHamBand(QObject *, int, HamBand)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(agcModeChanged(QObject *, int, AGCMode)), 
		this, 
		SLOT(setAGCMode(QObject *, int, AGCMode)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(agcGainChanged(QObject *, int, int)), 
		this, 
		SLOT(setAGCGain(QObject *, int, int)));

	CHECKED_CONNECT(
		m_settings, 
		SIGNAL(filterFrequenciesChanged(QObject *, int, qreal, qreal)), 
		this, 
		SLOT(setFilterFrequencies(QObject *, int, qreal, qreal)));
}

void HPSDRReceiver::setSocketState(SocketState state) { 
	
	m_socketState = state;
}

HPSDRReceiver::SocketState HPSDRReceiver::socketState() const { 
	
	return m_socketState;
}

void HPSDRReceiver::setSystemState(
	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	Q_UNUSED (sender)
	Q_UNUSED (err)

	bool change = false;

	if (m_hwInterface != hwmode) {
		
		m_hwInterface = hwmode;
		change = true;
	}

	if (m_serverMode != mode) {
		
		m_serverMode = mode;
		change = true;
	}

	if (m_dataEngineState != state) {
		
		m_dataEngineState = state;
		change = true;
	}
}

void HPSDRReceiver::setPeerAddress(QHostAddress addr) {

	m_peerAddress = addr;
}

void HPSDRReceiver::setSocketDescriptor(int value) {

	m_socketDescriptor = value;
}

void HPSDRReceiver::setReceiver(int value) {

	m_receiver = value;
}

void HPSDRReceiver::setClient(int value) {

	m_client = value;
}

void HPSDRReceiver::setIQPort(int value) {

	m_iqPort = value;
}

void HPSDRReceiver::setBSPort(int value) {

	m_bsPort = value;
}

void HPSDRReceiver::setConnectedStatus(bool value) {

	m_connected = value;
}

void HPSDRReceiver::setID(int value) {

	m_receiverID = value;
}

void HPSDRReceiver::setSampleRate(int value) {

	m_sampleRate = value;
}

void HPSDRReceiver::setHamBand(QObject *sender, int rx, HamBand band) {

	Q_UNUSED(sender)

	if (m_receiver == rx) {
		
		if (m_hamBand == band) return;
		m_hamBand = band;
	}
}

void HPSDRReceiver::setDspMode(QObject *sender, int rx, DSPMode mode) {

	Q_UNUSED(sender)

	if (m_receiver == rx) {

		if (m_dspMode == mode) return;
		m_dspMode = mode;
	}

	QString msg = "[receiver]: set mode for receiver %1 to %2";
	emit messageEvent(msg.arg(rx).arg(m_settings->getDSPModeString(m_dspMode)));
}

void HPSDRReceiver::setAGCMode(QObject *sender, int rx, AGCMode mode) {

	Q_UNUSED(sender)
	
	if (m_receiver == rx) {

		if (m_agcMode == mode) return;
		m_agcMode = mode;
	}
}

void HPSDRReceiver::setAGCGain(QObject *sender, int rx, int value) {

	Q_UNUSED(sender)
	
	if (m_receiver == rx) {

		if (m_agcGain == value) return;
		m_agcGain = value;
	}
}

void HPSDRReceiver::setAudioVolume(QObject *sender, int rx, float value) {

	Q_UNUSED(sender)
	
	if (m_receiver == rx) {

		if (m_audioVolume == value) return;
		m_audioVolume = value;
	}
}

void HPSDRReceiver::setFilterFrequencies(QObject *sender, int rx, double low, double high) {

	Q_UNUSED(sender)
	
	if (m_receiver == rx) {

		if (m_filterLo == low && m_filterHi == high) return;
		m_filterLo = low;
		m_filterHi = high;
	}
}

void HPSDRReceiver::setFrequency(long frequency) {

	if (m_frequency == frequency) return;
	m_frequency = frequency;

	HamBand band = getBandFromFrequency(m_settings->getBandFrequencyList(), frequency);
	m_lastFrequencyList[(int) band] = m_frequency;

}

void HPSDRReceiver::setLastFrequencyList(const QList<long> fList) {

	m_lastFrequencyList = fList;
}

void HPSDRReceiver::setdBmPanScaleMin(qreal value) {

	if (m_dBmPanScaleMin == value) return;
	m_dBmPanScaleMin = value;
}

void HPSDRReceiver::setdBmPanScaleMax(qreal value) {

	if (m_dBmPanScaleMax == value) return;
	m_dBmPanScaleMax = value;
}

