/**
* @file  cusdr_settings.cpp
* @brief settings class for cuSDR
* @author by Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-11-18
*/

/*   
 *   Copyright 2010, 2011, 2012 Hermann von Hasseln, DL3HVH
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


#define LOG_SETTINGS

#include "cusdr_settings.h"
#include "Util/cusdr_styles.h"

Settings *Settings::m_instance = NULL;		/*!< set m_instance to NULL. */

/*!
	\class Settings
	\brief Settings class implements application specific and user defined variables for the application.
*/

Settings::Settings(QObject *parent)
	:QObject(parent)
	, m_dataEngineState(QSDR::DataEngineDown)
	, setLoaded(false)
	, m_mainPower(false)
	, m_manualSocketBufferSize(false)
	, m_peakHold(false)
	, m_packetsToggle(true)
	, m_radioPopupVisible(false)
	, m_hpsdrNetworkDevices(0)
	, m_mercuryReceivers(1)
	, m_currentReceiver(0)
{
	// temporarily
	//m_pennyLanePresence = false;
	m_devices.mercuryFWVersion = 0;

	qRegisterMetaType<TNetworkDevicecard>();
	qRegisterMetaType<QList<TNetworkDevicecard> >();
	qRegisterMetaType<qVectorFloat>("qVectorFloat");
	//qRegisterMetaType<CPX>("CPX");

	startTime = QDateTime::currentDateTime();

	qDebug() << "************************************************************************";
	SETTINGS_DEBUG << "start at: " << qPrintable(startTime.toString());

	settingsFilename = "settings.ini";
	settings = new QSettings(QCoreApplication::applicationDirPath() +  "/" + settingsFilename, QSettings::IniFormat);

	m_titleString = "cuSDR64 BETA ";

	#ifdef DEBUG
		m_titleString = "cuSDR64 Debug BETA ";
	#endif

	m_versionString = "v0.3.2.2";
	
	qDebug() << qPrintable(m_titleString);

	for (int i = 0; i < MAX_RECEIVERS; i++) {

		TReceiver receiverData;
		m_receiverDataList.append(receiverData);

		QString str = "receiver";
		QString num;
		num.setNum(i);
		str.append(num);

		m_rxStringList << str;

		m_receiverDataList[i].agcHangThreshold = 0.0;

		for (int j = 0; j < MAX_BANDS; j++) {

			m_receiverDataList[i].mercuryAttenuators << 0;
			m_receiverDataList[i].dBmPanScaleMinList << 0.0;
			m_receiverDataList[i].dBmPanScaleMaxList << 0.0;
			m_receiverDataList[i].dspModeList << (DSPMode) LSB;
		}
	}

	// Alex parameter configurations
	m_alexConfig = 0;

	for (int i = 0; i < 6; i++) {

		m_HPFLoFrequencyList.append((long)0);
		m_HPFHiFrequencyList.append((long)0);
	}

	for (int i = 0; i < 7; i++) {

		m_LPFLoFrequencyList.append((long)0);
		m_LPFHiFrequencyList.append((long)0);
	}

	// init alex states
	for (int i = 0; i < MAX_BANDS; i++) {

		m_alexStates << 0;
	}

	// Rx, Tx J6 pins list
	// Bands: 160m, 80m, 60m, 40m, 30m, 20m, 17m, 15m, 12m, 10m, 6m
	// Values: no pin, pins 1 to 7 (0..7)
	//
	// 0 0 0 0 0 0 0 0
	// | | | | | | |
	// | | | | | | |
	// | | | | | | +-------------- pin 1 (0 = deactivated, 1 = activated)
	// | | | | | +---------------- pin 2 (0 = deactivated, 1 = activated)
	// | | | | +------------------ pin 3 (0 = deactivated, 1 = activated)
	// | | | +-------------------- pin 4 (0 = deactivated, 1 = activated)
	// | | +---------------------- pin 5 (0 = deactivated, 1 = activated)
	// | +------------------------ pin 6 (0 = deactivated, 1 = activated)
	// +-------------------------- pin 7 (0 = deactivated, 1 = activated)

	for (int i = 0; i < MAX_BANDS-1; i++) {

		m_rxJ6pinList << 0;
		m_txJ6pinList << 0;
	}


	m_bandList = getHamBandFrequencies();
	m_bandTextList = getHamBandText();
	m_defaultFilterList = getDefaultFilterFrequencies();

	m_transmitter.txAllowed = false;

	//m_packetLossTimer = new QTimer(this);
	//m_packetLossTimer->setSingleShot(true);
	//m_packetLossTime.start();

	//// ****************************
	//// check for OpenCL devices
	//m_clDevices = QCLDevice::allDevices();
	//if (m_clDevices.length() == 0) {
	//
	//	SETTINGS_DEBUG << "Init:: no OpenCL devices found!\n";
	//}
	//else {

	//	SETTINGS_DEBUG << "found" << m_clDevices.length() << "OpenCL device(s).";
	//	QString clNo = QString::number(m_clDevices.length());
	//}

	/*
	CPX x, y, z;

	x.resize(2);
	y.resize(2);
	z.resize(2);

	x[0].re = 1.0f; x[0].im = 2.0f;
	x[1].re = 0.0f; x[1].im = 1.0f;
	y[0].re = 2.0f; y[0].im = 2.0f;
	y[1].re = 2.0f; y[1].im = 3.0f;

	qDebug() << "x1 = (" << x.at(0).re << "," << x.at(0).im << ")";
	qDebug() << "x2 = (" << x.at(1).re << "," << x.at(1).im << ")";
	qDebug() << "y1 = (" << y.at(0).re << "," << y.at(0).im << ")";
	qDebug() << "y2 = (" << y.at(1).re << "," << y.at(1).im << ")";

	PlusCPX(x, y, z);

	qDebug() << "z1 = (" << z.at(0).re << "," << z.at(0).im << ")";
	qDebug() << "z2 = (" << z.at(1).re << "," << z.at(1).im << ")";

	PlusCPX(z, x, y);

	qDebug() << "y1 = (" << y.at(0).re << "," << y.at(0).im << ")";
	qDebug() << "y2 = (" << y.at(1).re << "," << y.at(1).im << ")";
	*/
}

Settings::~Settings() {

	//m_clDevices.clear();
}

int Settings::loadSettings() {

	QString str;
	int value;

	//QList<QString> bandList = HamBandStrings();

	// user's call sign
	str = settings->value("user/callSign", "Your Call sign").toString();
	//while (str.startsWith('\"')) str = str.right(str.count() - 1).trimmed();
	//while (str.endsWith('\"')) str = str.left(str.count() - 1).trimmed();

	m_callsignString = str;

	// Window settings
	value = settings->value("window/minimumWidgetWidth", 235).toInt();
	if (value < 235 || value > 300) m_minimumWidgetWidth = 235;
	m_minimumWidgetWidth = value;

	value = settings->value("window/minimumGroupBoxWidth", 230).toInt();
	if (value < 230 || value > 295 || value > m_minimumWidgetWidth - 5) m_minimumGroupBoxWidth = 230;
	m_minimumGroupBoxWidth = value;

	value = settings->value("window/multiRxView", 0).toInt();
	if (value < 0 || value > 2) m_multiRxView = 0;
	m_multiRxView = value;


	// network settings
	str = settings->value("network/server_ipAddress", "127.0.0.1").toString();
	while (str.startsWith('\"')) str = str.right(str.count() - 1).trimmed();
	while (str.endsWith('\"')) str = str.left(str.count() - 1).trimmed();
	m_serverAddress = str;

	str = settings->value("network/hpsdr_local_ipAddress", "127.0.0.1").toString();
	while (str.startsWith('\"')) str = str.right(str.count() - 1).trimmed();
	while (str.endsWith('\"')) str = str.left(str.count() - 1).trimmed();
	m_hpsdrDeviceLocalAddr = str;

	value = settings->value("network/server_port", 52685).toInt();
	if (value < 0 || value > 65535) value = 52685;
	m_serverPort = value;

	value = settings->value("network/listen_port", 11000).toInt();
	if (value < 0 || value > 65535) value = 11000;
	m_listenerPort = value;

	value = settings->value("network/audio_port", 15000).toInt();
	if (value < 0 || value > 65535) value = 15000;
	m_audioPort = value;

	value = settings->value("network/metis_port", 1024).toInt();
	if (value < 0 || value > 65535) value = 1024;
	m_metisPort = value;

	value = settings->value("network/socketBufferSize", 32).toInt();
	if (value != 16 && value != 32 && value != 64 && value != 128 && value != 256) value = 32;
	m_socketBufferSize = value;


	// HPSDR hardware
	value = settings->value("hpsdr/hardware", 0).toInt();
	if (value < 0 || value > 2) value = 0;
	m_hpsdrHardware = value;

	str = settings->value("hpsdr/mercury", "true").toString();
	if (str == "true")
		m_devices.mercuryPresence = true;
	else
		m_devices.mercuryPresence = false;

	str = settings->value("hpsdr/penelope", "false").toString();
	if (str == "true")
		m_devices.penelopePresence = true;
	else
		m_devices.penelopePresence = false;

	str = settings->value("hpsdr/pennylane", "false").toString();
	if (str == "true")
		m_devices.pennylanePresence = true;
	else
		m_devices.pennylanePresence = false;

	str = settings->value("hpsdr/excalibur", "false").toString();
	if (str == "true")
		m_devices.excaliburPresence = true;
	else
		m_devices.excaliburPresence = false;

	str = settings->value("hpsdr/alex", "false").toString();
	if (str == "true")
		m_devices.alexPresence = true;
	else
		m_devices.alexPresence = false;

//	str = settings->value("hpsdr/hermes", "false").toString();
//	if (str == "true")
//		m_devices.hermesPresence = true;
//	else
//		m_devices.hermesPresence = false;

	if (m_hpsdrHardware == 0) {

		str = settings->value("hpsdr/interface", "metis").toString();
			if (str == "metis")
				m_hwInterface = QSDR::Metis;
			else
				m_hwInterface = QSDR::NoInterfaceMode;
	}
	else if (m_hpsdrHardware == 1) {

		m_hwInterface = QSDR::Hermes;
	}

	//checkHPSDRDevices();

	value = settings->value("server/sample_rate", 48000).toInt();
	if ((value != 48000) & (value != 96000) & (value != 192000)) value = 48000;
	setSampleRate(this, value);

	str = settings->value("server/dither", "off").toString();
	if (str.toLower() == "on")
		m_mercuryDither = 1;
	else
		m_mercuryDither = 0;

	str = settings->value("server/random", "off").toString();
	if (str.toLower() == "on")
		m_mercuryRandom = 1;
	else
		m_mercuryRandom = 0;

	str = settings->value("server/widebandData", "on").toString();
	if (str.toLower() == "on")
		m_wideBandData = true;
	else if (str.toLower() == "off")
		m_wideBandData = false;
	else
		m_wideBandData = true;

	str = settings->value("server/widebandDisplay", "off").toString();
	if (str.toLower() == "on")
		m_wideBandDisplayStatus = true;
	else
		m_wideBandDisplayStatus = false;

	if (!m_wideBandData) m_wideBandDisplayStatus = false;

	str = settings->value("server/10mhzsource", "mercury").toString();
	if (str == "atlas")
		m_10MHzSource = 0;
	else if (str == "penelope")
		m_10MHzSource = 1;
	else if (str == "mercury")
		m_10MHzSource = 2;
	else if (str == "none")
		m_10MHzSource = 3;
	else
		m_10MHzSource = 2;

	str = settings->value("server/122_88mhzsource", "mercury").toString();
	if (str == "penelope")
		m_122_8MHzSource = 0;
	else
		m_122_8MHzSource = 1;

	str = settings->value("server/mic_source", "penelope").toString();
	if (str == "janus")
		m_micSource = 0;
	else
		m_micSource = 1;

	str = settings->value("server/class", 0).toString();
	m_RxClass = (str.toLower() == "E");
	if (m_RxClass)
		m_RxClass = 1;
	else
		m_RxClass = 0;

	value = settings->value("server/timing", 0).toInt();
	if (value < 0 || value > 1) value = 0;
	m_RxTiming = value;

	/*value = settings->value("server/mainVolume", 10).toInt();
	if (value < 0) value = 0;
	if (value > 100) value = 100;
	m_mainVolume = value / 100.0f;*/

	str = settings->value("server/mode", "sdr").toString();
	if (str == "sdr") {

		m_serverMode = QSDR::SDRMode;
		setSpectrumSize(this, 4096);
	}
	else if (str == "chirpWSPR") {

		m_serverMode = QSDR::ChirpWSPR;
		setSpectrumSize(this, 4096);
	}
	else if (str == "chirpWSPRFile") {

		m_serverMode = QSDR::ChirpWSPRFile;
		setSpectrumSize(this, 4096);
	}
	else {

		m_serverMode = QSDR::SDRMode;
		setSpectrumSize(this, 4096);
	}


//	value = settings->value("server/mouseWheelFreqStep", 1000).toInt();
//	if ((value != 1) & (value != 10) & (value != 100) & (value != 1000) & (value != 10000) & (value != 100000) &
//		(value != 5) & (value != 50) & (value != 500) & (value != 5000) & (value != 50000) & (value != 500000))
//		value = 100;
//	m_mouseWheelFreqStep = (double)value;

	//******************************************************************
	// Alexiares data settings
	// m_alexConfig (qint16):
	//
	// 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
	//   | | | | | | | | | | | | | | |
	//   | | | | | | | | | | | | | | +-----Alex   - manual HPF/LPF filter select (0 = disable, 1 = enable)
	//   | | | | | | | | | | | | | +------ Alex   -	Bypass all HPFs   (0 = disable, 1 = enable)*
	//   | | | | | | | | | | | | +-------- Alex   -	6M low noise amplifier (0 = disable, 1 = enable)*
	//   | | | | | | | | | | | +---------- Alex   -	select 1.5MHz HPF (0 = disable, 1 = enable)*
	//   | | | | | | | | | | +------------ Alex   -	select 6.5MHz HPF (0 = disable, 1 = enable)*
	//   | | | | | | | | | +-------------- Alex   -	select 9.5MHz HPF (0 = disable, 1 = enable)*
	//   | | | | | | | | +---------------- Alex   -	select 13MHz  HPF (0 = disable, 1 = enable)*
	//   | | | | | | | +------------------ Alex   -	select 20MHz  HPF (0 = disable, 1 = enable)*
	//   | | | | | | +-------------------- Alex   - select 160m   LPF (0 = disable, 1 = enable)*
	//   | | | | | +---------------------- Alex   - select 80m    LPF (0 = disable, 1 = enable)*
	//   | | | | +------------------------ Alex   - select 60/40m LPF (0 = disable, 1 = enable)*
	//   | | | +-------------------------- Alex   - select 30/20m LPF (0 = disable, 1 = enable)*
	//   | | +---------------------------- Alex   - select 17/15m LPF (0 = disable, 1 = enable)*
	//   | +------------------------------ Alex   - select 12/10m LPF (0 = disable, 1 = enable)*
	//   +-------------------------------- Alex   - select 6m     LPF (0 = disable, 1 = enable)*

	m_alexConfig = 0;
	double fLo;
	double fHi;

	str = settings->value("alex/manual", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x01;

	str = settings->value("alex/bypassAll", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x02;

	str = settings->value("alex/amp6m", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x04;

	fLo = settings->value("alex/amp6mLo", 50000000).toDouble();
	if ((fLo < 49000000) || (fLo > 52500000)) fLo = 50000000;

	fHi = settings->value("alex/amp6mHi", 54000000).toDouble();
	if ((fHi < 52500000) || (fHi > 55000000)) fHi = 54000000;

	m_HPFLoFrequencyList[5] = (long)fLo;
	m_HPFHiFrequencyList[5] = (long)fHi;


	str = settings->value("alex/hpf1_5MHz", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x08;

	fLo = settings->value("alex/hpf1_5MHzLo", 1500000).toDouble();
	if ((fLo < 0) || (fLo > 2000000)) fLo = 1500000;

	fHi = settings->value("alex/hpf1_5MHzHi", 5500000).toDouble();
	if ((fHi < 1600000) || (fHi > 6000000)) fHi = 5500000;

	m_HPFLoFrequencyList[0] = (long)fLo;
	m_HPFHiFrequencyList[0] = (long)fHi;


	str = settings->value("alex/hpf6_5MHz", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x10;

	fLo = settings->value("alex/hpf6_5MHzLo", 7000000).toDouble();
	if ((fLo < 6000000) || (fLo > 8000000)) fLo = 7000000;

	fHi = settings->value("alex/hpf6_5MHzHi", 7300000).toDouble();
	if ((fHi < 7000000) || (fHi > 9500000)) fHi = 7300000;

	m_HPFLoFrequencyList[1] = (long)fLo;
	m_HPFHiFrequencyList[1] = (long)fHi;


	str = settings->value("alex/hpf9_5MHz", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x20;

	fLo = settings->value("alex/hpf9_5MHzLo", 10100000).toDouble();
	if ((fLo < 9000000) || (fLo > 11000000)) fLo = 10100000;

	fHi = settings->value("alex/hpf9_5MHzHi", 10150000).toDouble();
	if ((fHi < 10000000) || (fHi > 13000000)) fHi = 10150000;

	m_HPFLoFrequencyList[2] = (long)fLo;
	m_HPFHiFrequencyList[2] = (long)fHi;


	str = settings->value("alex/hpf13MHz", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x40;

	fLo = settings->value("alex/hpf13MHzLo", 14000000).toDouble();
	if ((fLo < 12000000) || (fLo > 15000000)) fLo = 14000000;

	fHi = settings->value("alex/hpf13MHzHi", 18168000).toDouble();
	if ((fHi < 13700000) || (fHi > 19000000)) fHi = 18168000;

	m_HPFLoFrequencyList[3] = (long)fLo;
	m_HPFHiFrequencyList[3] = (long)fHi;


	str = settings->value("alex/hpf20MHz", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x80;

	fLo = settings->value("alex/hpf20MHzLo", 21000000).toDouble();
	if ((fLo < 18000000) || (fLo > 25000000)) fLo = 21000000;

	fHi = settings->value("alex/hpf20MHzHi", 29700000).toDouble();
	if ((fHi < 25000000) || (fHi > 32000000)) fHi = 29700000;

	m_HPFLoFrequencyList[4] = (long)fLo;
	m_HPFHiFrequencyList[4] = (long)fHi;


	str = settings->value("alex/lpf160m", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x100;

	fLo = settings->value("alex/lpf160mLo", 1800000).toDouble();
	if ((fLo < 0) || (fLo > 1900000)) fLo = 1800000;

	fHi = settings->value("alex/lpf160mHi", 2000000).toDouble();
	if ((fHi < 1000000) || (fHi > 3000000)) fHi = 2000000;

	m_LPFLoFrequencyList[0] = (long)fLo;
	m_LPFHiFrequencyList[0] = (long)fHi;


	str = settings->value("alex/lpf80m", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x200;

	fLo = settings->value("alex/lpf80mLo", 3500000).toDouble();
	if ((fLo < 2000000) || (fLo > 4000000)) fLo = 3500000;

	fHi = settings->value("alex/lpf80mHi", 4000000).toDouble();
	if ((fHi < 2000000) || (fHi > 5000000)) fHi = 4000000;

	m_LPFLoFrequencyList[1] = (long)fLo;
	m_LPFHiFrequencyList[1] = (long)fHi;


	str = settings->value("alex/lpf60_40m", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x400;

	fLo = settings->value("alex/lpf60_40mLo", 5330000).toDouble();
	if ((fLo < 5000000) || (fLo > 11000000)) fLo = 5330000;

	fHi = settings->value("alex/lpf60_40mHi", 7300000).toDouble();
	if ((fHi < 5000000) || (fHi > 8000000)) fHi = 7300000;

	m_LPFLoFrequencyList[2] = (long)fLo;
	m_LPFHiFrequencyList[2] = (long)fHi;


	str = settings->value("alex/lpf30_20m", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x800;

	fLo = settings->value("alex/lpf30_20mLo", 10100000).toDouble();
	if ((fLo < 9000000) || (fLo > 15000000)) fLo = 10100000;

	fHi = settings->value("alex/lpf30_20mHi", 14350000).toDouble();
	if ((fHi < 9000000) || (fHi > 15000000)) fHi = 14350000;

	m_LPFLoFrequencyList[3] = (long)fLo;
	m_LPFHiFrequencyList[3] = (long)fHi;


	str = settings->value("alex/lpf17_15m", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x1000;

	fLo = settings->value("alex/lpf17_15mLo", 18068000).toDouble();
	if ((fLo < 17000000) || (fLo > 22000000)) fLo = 18068000;

	fHi = settings->value("alex/lpf17_15mHi", 21450000).toDouble();
	if ((fHi < 17000000) || (fHi > 22000000)) fHi = 21450000;

	m_LPFLoFrequencyList[4] = (long)fLo;
	m_LPFHiFrequencyList[4] = (long)fHi;


	str = settings->value("alex/lpf12_10m", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x2000;

	fLo = settings->value("alex/lpf12_10mLo", 24890000).toDouble();
	if ((fLo < 23000000) || (fLo > 30000000)) fLo = 24890000;

	fHi = settings->value("alex/lpf12_10mHi", 29700000).toDouble();
	if ((fHi < 23000000) || (fHi > 30000000)) fHi = 29700000;

	m_LPFLoFrequencyList[5] = (long)fLo;
	m_LPFHiFrequencyList[5] = (long)fHi;


	str = settings->value("alex/lpf6m", "off").toString();
	if (str.toLower() == "on")
		m_alexConfig |= 0x4000;

	fLo = settings->value("alex/lpf6mLo", 50000000).toDouble();
	if ((fLo < 30000000) || (fLo > 52000000)) fLo = 50000000;

	fHi = settings->value("alex/lpf6mHi", 54000000).toDouble();
	if ((fHi < 52000000) || (fHi > 66000000)) fHi = 54000000;

	m_LPFLoFrequencyList[6] = (long)fLo;
	m_LPFHiFrequencyList[6] = (long)fHi;


	SETTINGS_DEBUG << "Alex config: " << m_alexConfig;

	if (m_alexStates.length() == MAX_BANDS && m_bandList.length() == MAX_BANDS) {

		for (int i = 0; i < MAX_BANDS; i++) {

			str = "alex/state";
			str.append(m_bandList.at(i).bandString);

			value = settings->value(str, 33).toInt();
			setAlexState(this, i, value);
		}
	}

	//******************************************************************
	// Penny open collector settings

	str = settings->value("penny/OCenabled", "off").toString();
	if (str.toLower() == "on")
		m_pennyOCEnabled = true;
	else
		m_pennyOCEnabled = false;

	if (m_rxJ6pinList.length() == MAX_BANDS-1 && m_txJ6pinList.length() == MAX_BANDS-1 && m_bandList.length() == MAX_BANDS) {

		for (int i = 0; i < MAX_BANDS-1; i++) {

			str = "penny/rxState";
			str.append(m_bandList.at(i).bandString);

			value = settings->value(str, 0).toInt();
			if (value < 0 || value > 255) value = 0;
			setRxJ6Pin(this, (HamBand) i, value);
		}

		for (int i = 0; i < MAX_BANDS-1; i++) {

			str = "penny/txState";
			str.append(m_bandList.at(i).bandString);

			value = settings->value(str, 0).toInt();
			if (value < 0 || value > 255) value = 0;
			setTxJ6Pin(this, (HamBand) i, value);
		}
	}
	else {

		qWarning() << "rxJ6pinList or txJ6pinList or bandList have wrong length!";
	}

	//******************************************************************
	// receiver data settings

	for (int i = 0; i < MAX_RECEIVERS; i++) {

		QString cstr = m_rxStringList.at(i);
		cstr.append("/dspCore");

		str = settings->value(cstr, "qtdsp").toString();
		if (str == "qtdsp") {

			m_receiverDataList[i].dspCore = QSDR::QtDSP;
			setSpectrumSize(this, 4096);
			//SETTINGS_DEBUG << "DSP core for rx " << i << " is QtDSP.";
		}

		cstr = m_rxStringList.at(i);
		cstr.append("/audioVolume");

		value = settings->value(cstr, 10).toInt();
		if (value < 0) value = 0;
		if (value > 100) value = 100;
		m_receiverDataList[i].audioVolume = value/100.0f;

		cstr = m_rxStringList.at(i);
		cstr.append("/mouseWheelFreqStep");

		value = settings->value(cstr, 100).toInt();
		if ((value != 1) & (value != 10) & (value != 100) & (value != 1000) & (value != 10000) & (value != 100000) &
			(value != 5) & (value != 50) & (value != 500) & (value != 5000) & (value != 50000) & (value != 500000))
			value = 100;
		m_receiverDataList[i].mouseWheelFreqStep = (qreal)value;

		cstr = m_rxStringList.at(i);
		cstr.append("/agcGain");

		value = settings->value(cstr, 100).toInt();
		if (value < -20) value = -20;
		if (value > 120) value = 120;
		m_receiverDataList[i].acgGain = value;

		cstr = m_rxStringList.at(i);
		cstr.append("/agcFixedGain");

		value = settings->value(cstr, 30).toInt();
		if (value < -20) value = -20;
		if (value > 50) value = 50;
		m_receiverDataList[i].agcFixedGain_dB = value;

		cstr = m_rxStringList.at(i);
		cstr.append("/agcMaximumGain");

		value = settings->value(cstr, 30).toInt();
		if (value < -20) value = -20;
		if (value > 150) value = 150;
		m_receiverDataList[i].agcMaximumGain_dB = value - 0;

		cstr = m_rxStringList.at(i);
		cstr.append("/agcSlope");

		value = settings->value(cstr, 0).toInt();
		if (value < 0) value = 0;
		if (value > 20) value = 20;
		m_receiverDataList[i].agcVariableGain = (qreal)value;

		cstr = m_rxStringList.at(i);
		cstr.append("/agcAttacktime");

		value = settings->value(cstr, 1).toInt();
		if (value < 1) value = 1;
		if (value > 10) value = 10;
		m_receiverDataList[i].agcAttackTime = value/1000.0;

		cstr = m_rxStringList.at(i);
		cstr.append("/agcDecaytime");

		value = settings->value(cstr, 250).toInt();
		if (value < 10) value = 10;
		if (value > 5000) value = 5000;
		m_receiverDataList[i].agcDecayTime = value/1000.0;

		cstr = m_rxStringList.at(i);
		cstr.append("/agcHangtime");

		value = settings->value(cstr, 100).toInt();
		if (value < 10) value = 10;
		if (value > 5000) value = 5000;
		m_receiverDataList[i].agcHangTime = value/1000.0;

		cstr = m_rxStringList.at(i);
		cstr.append("/agcLines");

		str = settings->value(cstr, "on").toString();
		if (str.toLower() == "on")
			m_receiverDataList[i].agcLines = true;
		else
			m_receiverDataList[i].agcLines = false;

		cstr = m_rxStringList.at(i);
		cstr.append("/agcMode");

		str = settings->value(cstr, "MED").toString();
		if (str == "LONG")
			m_receiverDataList[i].agcMode = agcLONG;
		else if (str == "SLOW")
			m_receiverDataList[i].agcMode = agcSLOW;
		else if (str == "MED")
			m_receiverDataList[i].agcMode = agcMED;
		else if (str == "FAST")
			m_receiverDataList[i].agcMode = agcFAST;
		else
			m_receiverDataList[i].agcMode = agcOFF;

		if (str == "MED" || str == "FAST")
			m_receiverDataList[i].hangEnabled = false;
		else
			m_receiverDataList[i].hangEnabled = true;

//		cstr = m_rxStringList.at(i);
//		cstr.append("/waterfallTime");
//		value = settings->value(cstr, 50).toInt();
//		if ((value < 20) || (value > 250)) value = 50;
//		m_receiverDataList[i].waterfallTime = value;

		cstr = m_rxStringList.at(i);
		cstr.append("/framesPerSecond");
		value = settings->value(cstr, 25).toInt();
		if (value < 0 || value > 200) value = 25;
		m_receiverDataList[i].framesPerSecond = value;

		cstr = m_rxStringList.at(i);
		cstr.append("/waterfallOffsetLo");
		value = settings->value(cstr, -5).toInt();
		if ((value < -50) || (value > 50)) value = -5;
		m_receiverDataList[i].waterfallOffsetLo = value;

		cstr = m_rxStringList.at(i);
		cstr.append("/waterfallOffsetHi");
		value = settings->value(cstr, 20).toInt();
		if ((value < -50) || (value > 50)) value = 20;
		m_receiverDataList[i].waterfallOffsetHi = value;

		cstr = m_rxStringList.at(i);
		cstr.append("/filterHi");
		value = settings->value(cstr, -150).toInt();
		if (value < -20000 || value > 20000) value = -150;
		m_receiverDataList[i].filterHi = (qreal)(1.0f * value);

		cstr = m_rxStringList.at(i);
		cstr.append("/filterLo");
		value = settings->value(cstr, -3050).toInt();
		if (value < -20000 || value > 20000) value = -3050;
		m_receiverDataList[i].filterLo = (qreal)(1.0f * value);

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency160m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) || (value > 50000000)) value = 1800000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency80m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) || (value > 50000000)) value = 3779000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency60m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) || (value > 50000000)) value = 5260000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency40m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) || (value > 50000000)) value = 7000000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency30m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) || (value > 50000000)) value = 10100000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency20m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) || (value > 50000000)) value = 14000000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency17m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) || (value > 50000000)) value = 18068000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency15m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) || (value > 50000000)) value = 21000000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency12m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) || (value > 50000000)) value = 24890000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency10m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) || (value > 50000000)) value = 28000000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency6m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) || (value > 50000000)) value = 50000000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequencyGen");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 0) || (value > 50000000)) value = 3500000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/frequency");
		value = settings->value(cstr, 3672000).toDouble();
		if ((value < 0) || (value > 50000000)) value = 3600000;
		m_receiverDataList[i].frequency = value;

		setFrequency(i, value);

		if (m_receiverDataList[i].dspModeList.length() == MAX_BANDS && m_bandList.length() == MAX_BANDS) {

			for (int j = 0; j < MAX_BANDS; j++) {

				cstr = m_rxStringList.at(i);
				cstr.append("/dspMode");
				cstr.append(m_bandList.at(j).bandString);

				str = settings->value(cstr, "LSB").toString();
				if (str == "USB")
					m_receiverDataList[i].dspModeList[j] = USB;
				else if (str == "DSB")
					m_receiverDataList[i].dspModeList[j] = DSB;
				else if (str == "CWL")
					m_receiverDataList[i].dspModeList[j] = CWL;
				else if (str == "CWU")
					m_receiverDataList[i].dspModeList[j] = CWU;
				else if (str == "FMN")
					m_receiverDataList[i].dspModeList[j] = FMN;
				else if (str == "AM")
					m_receiverDataList[i].dspModeList[j] = AM;
				else if (str == "DIGU")
					m_receiverDataList[i].dspModeList[j] = DIGU;
				else if (str == "SPEC")
					m_receiverDataList[i].dspModeList[j] = SPEC;
				else if (str == "DIGL")
					m_receiverDataList[i].dspModeList[j] = DIGL;
				else if (str == "SAM")
					m_receiverDataList[i].dspModeList[j] = SAM;
				else if (str == "DRM")
					m_receiverDataList[i].dspModeList[j] = DRM;
				else
					m_receiverDataList[i].dspModeList[j] = LSB;
			}
		}

		if (m_receiverDataList[i].mercuryAttenuators.length() == MAX_BANDS && m_bandList.length() == MAX_BANDS) {

			for (int j = 0; j < MAX_BANDS; j++) {

				cstr = m_rxStringList.at(i);
				cstr.append("/attenuator");
				cstr.append(m_bandList.at(j).bandString);

				str = settings->value(cstr, "off").toString();
				if (str.toLower() == "on")
					m_receiverDataList[i].mercuryAttenuators[j] = 0; // which is 'Preamp' off
				else
					m_receiverDataList[i].mercuryAttenuators[j] = 1; // which is 'Preamp' on
			}
		}

		int valueMin, valueMax;
		if (m_receiverDataList[i].dBmPanScaleMinList.length() == MAX_BANDS &&
			m_receiverDataList[i].dBmPanScaleMaxList.length() == MAX_BANDS &&
			m_bandList.length() == MAX_BANDS
		) {
			for (int j = 0; j < MAX_BANDS; j++) {

				cstr = m_rxStringList.at(i);
				cstr.append("/dBmPanScaleMin");
				cstr.append(m_bandList.at(j).bandString);

				valueMin = settings->value(cstr, -120).toInt();
				if ((valueMin < -200) || (valueMin > 0)) valueMin = -120;

				cstr = m_rxStringList.at(i);
				cstr.append("/dBmPanScaleMax");
				cstr.append(m_bandList.at(j).bandString);

				valueMax = settings->value(cstr, -10).toInt();
				if ((valueMax < -200) || (valueMax > 0)) valueMax = -10;

				if (valueMax <= valueMin) {

					valueMin = -120;
					valueMax = -10;
				}

				m_receiverDataList[i].dBmPanScaleMinList[j] = (qreal)(1.0 * valueMin);
				m_receiverDataList[i].dBmPanScaleMaxList[j] = (qreal)(1.0 * valueMax);
			}
		}
	}

	//******************************************************************
	// Chirp WSPR settings
	value = settings->value("ChirpWSPR/lowerChirpFrequency", 500).toInt();
	if (value < 1 || value > 10000) value = 500;
	m_lowerChirpFreq = value;

	value = settings->value("ChirpWSPR/upperChirpFrequency", 2500).toInt();
	if (value < 2 || value < m_lowerChirpFreq + 1 || value > 50000) value = 2500;
	m_upperChirpFreq = value;

	value = settings->value("ChirpWSPR/chirpAmplitude", 75).toInt();
	if (value < 0 || value > 100) return -1;
	m_chirpAmplitude = qreal(value / 100.0);

	value = settings->value("ChirpWSPR/chirpSamplingFrequency", 48000).toInt();
	if ((value != 8000) & (value != 48000) & (value != 96000)) value = 48000;
	m_chirpSamplingFreq	= value;

	value = settings->value("ChirpWSPR/chirpBufferDurationUs", 10000000).toInt();
	if (value < 0 || value > 100000000) value = 10000000;
	m_chirpBufferDurationUs	= (qint64)value;

	value = settings->value("ChirpWSPR/chirpChannels", 1).toInt();
	if ((value != 1) & (value != 2)) value = 1;
	m_chirpChannels	= value;

	value = settings->value("ChirpWSPR/chirpBufferLength", 4096).toInt();
	if ((value != 4096) & (value != 16384)) value = 4096;
	m_chirpBufferLength = (qint64)value;

	value = settings->value("ChirpWSPR/chirpFilterLowerFrequency", 500).toInt();
	if (value < 10000 || value > 10000) value = 500;
	m_chirpFilterLowerFrequency = value;

	value = settings->value("ChirpWSPR/chirpFilterUpperFrequency", 2500).toInt();
	if (value < 10000 || value > 10000) value = 2500;
	m_chirpFilterUpperFrequency = value;

	/*value = settings->value("ChirpWSPR/chirpDownSampleRate", 4).toInt();
	if ((value != 4) & (value != 8)) return -1;
	m_chirpDownSampleRate = value;*/


	//******************************************************************
	// graphics settings
	str = settings->value("graphics/panadapter", "filledline").toString();
	if (str == "filledline")
		m_panadapterMode = QSDRGraphics::FilledLine;
	else
	if (str == "line")
		m_panadapterMode = QSDRGraphics::Line;
	else
	if (str == "solid")
		m_panadapterMode = QSDRGraphics::Solid;

	str = settings->value("graphics/averaging", "on").toString();
	if (str.toLower() == "on")
		m_specAveraging = true;
	else
		m_specAveraging = false;

	value = settings->value("graphics/averagingCnt", 5).toInt();
	if ((value < 1) || (value > 100)) value = 5;
	m_specAveragingCnt = value;

	str = settings->value("graphics/grid", "on").toString();
	if (str.toLower() == "on")
		m_panGrid = true;
	else
		m_panGrid = false;

	value = settings->value("graphics/resolution", 2).toInt();
	if ((value != 1) & (value != 2) & (value != 4)) value = 2;
	m_graphicResolution = value;

	value = settings->value("graphics/dBmWideBandScaleMin", -140).toInt();
	if ((value < -200) || (value > 0)) value = -140;
	m_dBmWBScaleMin = (qreal)(1.0 * value);

	value = settings->value("graphics/dBmWideBandScaleMax", -10).toInt();
	if ((value < -100) || (value > 0)) value = -10;
	m_dBmWBScaleMax = (qreal)(1.0 * value);

	value = settings->value("graphics/dBmDistScaleMin", -20).toInt();
	if ((value < -200) || (value > 0)) value = -20;
	m_dBmDistScaleMin = (qreal)(1.0 * value);

	value = settings->value("graphics/dBmDistScaleMax", 100).toInt();
	if ((value < -100) || (value > 200)) value = 100;
	m_dBmDistScaleMax = (qreal)(1.0 * value);

	str = settings->value("graphics/waterfall", "enhanced").toString();
	if (str == "simple")
		m_waterfallColorScheme = QSDRGraphics::simple;
	else
	if (str == "enhanced")
		m_waterfallColorScheme = QSDRGraphics::enhanced;
	else
		m_waterfallColorScheme = QSDRGraphics::spectran;

	value = settings->value("graphics/sMeterHoldTime", 2000).toInt();
	if ((value < 0) || (value > 10000)) value = 2000;
	m_sMeterHoldTime = value;


	//******************************************************************
	// color settings
	QColor color;

	color = settings->value("colors/panBackground", QColor(102, 69, 8)).value<QColor>();
	if (!color.isValid()) color = QColor(102, 69, 8);
	m_panadapterColors.panBackgroundColor = color;

	color = settings->value("colors/waterfall", QColor(246, 146, 6)).value<QColor>();
	if (!color.isValid()) color = QColor(246, 146, 6);
	m_panadapterColors.waterfallColor = color;

	color = settings->value("colors/panLine", QColor(246, 164, 76)).value<QColor>();
	if (!color.isValid()) color = QColor(246, 164, 76);
	m_panadapterColors.panLineColor = color;

	color = settings->value("colors/panLineFilled", QColor(246, 159, 7)).value<QColor>();
	if (!color.isValid()) color = QColor(246, 159, 7);
	m_panadapterColors.panLineFilledColor = color;

	color = settings->value("colors/panSolidTop", QColor(230, 246, 204)).value<QColor>();
	if (!color.isValid()) color = QColor(230, 246, 204);
	m_panadapterColors.panSolidTopColor = color;

	color = settings->value("colors/panSolidBottom", QColor(102, 96, 8)).value<QColor>();
	if (!color.isValid()) color = QColor(102, 96, 8);
	m_panadapterColors.panSolidBottomColor = color;

	color = settings->value("colors/panWideBandLine", QColor(73, 111, 7)).value<QColor>();
	if (!color.isValid()) color = QColor(73, 111, 7);
	m_panadapterColors.wideBandLineColor = color;

	color = settings->value("colors/panWideBandFilled", QColor(137, 172, 62)).value<QColor>();
	if (!color.isValid()) color = QColor(137, 172, 62);
	m_panadapterColors.wideBandFilledColor = color;

	color = settings->value("colors/panWideBandSolidTop", QColor(236, 38, 16)).value<QColor>();
	if (!color.isValid()) color = QColor(236, 38, 16);
	m_panadapterColors.wideBandSolidTopColor = color;

	color = settings->value("colors/panWideBandSolidBottom", QColor(232, 134, 29)).value<QColor>();
	if (!color.isValid()) color = QColor(232, 134, 29);
	m_panadapterColors.wideBandSolidBottomColor = color;

	color = settings->value("colors/distanceLine", QColor(246, 27, 45)).value<QColor>();
	if (!color.isValid()) color = QColor(246, 27, 45);
	m_panadapterColors.distanceLineColor = color;

	color = settings->value("colors/distanceLineFilled", QColor(232, 29, 86)).value<QColor>();
	if (!color.isValid()) color = QColor(232, 29, 86);
	m_panadapterColors.distanceLineFilledColor = color;

	color = settings->value("colors/panCenterLine", QColor(246, 7, 19)).value<QColor>();
	if (!color.isValid()) color = QColor(246, 7, 19);
	m_panadapterColors.panCenterLineColor = color;

	color = settings->value("colors/gridLine", QColor(7, 96, 96)).value<QColor>();
	if (!color.isValid()) color = QColor(7, 96, 96);
	m_panadapterColors.gridLineColor = color;

	SETTINGS_DEBUG << "reading done.";

	return 0;
}

int Settings::saveSettings() {

	QString str;
	//QList<QString> bandList = HamBandStrings();

	settings->setValue(getTitleStr(), getVersionStr());
	settings->setValue("saved", QDateTime::currentDateTime().toString("dddd dd MMMM yyyy hh:mm:ss"));// << " local time\n\n");
	settings->setValue("user/callSign", m_callsignString);

	// window settings
	settings->setValue("window/minimumWidgetWidth", m_minimumWidgetWidth);
	settings->setValue("window/minimumGroupBoxWidth", m_minimumGroupBoxWidth);
	settings->setValue("window/multiRxView", m_multiRxView);
	
	// network settings
	settings->setValue("network/server_ipAddress", m_serverAddress);
	settings->setValue("network/hpsdr_local_ipAddress", m_hpsdrDeviceLocalAddr);
	settings->setValue("network/server_port", m_serverPort);
	settings->setValue("network/listen_port", m_listenerPort);
	settings->setValue("network/audio_port", m_audioPort);
	settings->setValue("network/metis_port", m_metisPort);
	settings->setValue("network/socketBufferSize", m_socketBufferSize);

	
	// HPSDR hardware
	settings->setValue("hpsdr/hardware", m_hpsdrHardware);

	switch (m_hpsdrHardware) {

		// Mercury/Penelope, PennyLane
		case 0:

			if (m_devices.mercuryPresence)
				settings->setValue("hpsdr/mercury", "true");
			else
				settings->setValue("hpsdr/mercury", "false");

			if (m_devices.penelopePresence)
				settings->setValue("hpsdr/penelope", "true");
			else
				settings->setValue("hpsdr/penelope", "false");

			if (m_devices.pennylanePresence)
				settings->setValue("hpsdr/pennylane", "true");
			else
				settings->setValue("hpsdr/pennylane", "false");

			if (m_devices.excaliburPresence)
				settings->setValue("hpsdr/excalibur", "true");
			else
				settings->setValue("hpsdr/excalibur", "false");
			break;

		// Hermes
		case 1:

			if (m_devices.mercuryPresence)
				settings->setValue("hpsdr/mercury", "true");
			else
				settings->setValue("hpsdr/mercury", "false");

			if (m_devices.penelopePresence)
				settings->setValue("hpsdr/penelope", "true");
			else
				settings->setValue("hpsdr/penelope", "false");

			if (m_devices.pennylanePresence)
				settings->setValue("hpsdr/pennylane", "true");
			else
				settings->setValue("hpsdr/pennylane", "false");

			if (m_devices.excaliburPresence)
				settings->setValue("hpsdr/excalibur", "true");
			else
				settings->setValue("hpsdr/excalibur", "false");
			break;

		// Cyclops
		case 2:
			break;
	}
	
	if (m_devices.alexPresence)
		settings->setValue("hpsdr/alex", "true");
	else
		settings->setValue("hpsdr/alex", "false");

//	if (m_devices.hermesPresence)
//		settings->setValue("hpsdr/hermes", "true");
//	else
//		settings->setValue("hpsdr/hermes", "false");

	switch (m_hpsdrHardware) {

		// Mercury/Penelope
		case 0:

			if (m_hwInterface == QSDR::Metis)
				settings->setValue("hpsdr/interface", "metis");
			else if (m_hwInterface == QSDR::NoInterfaceMode)
				settings->setValue("hpsdr/interface", "noInterface");
			break;

		// Hermes
		case 1:

			if (m_hwInterface == QSDR::Hermes)
				settings->setValue("hpsdr/interface", "hermes");
			break;

		// Cyclops
		case 2:
			break;
	}

	// server settings
	settings->setValue("server/sample_rate", getSampleRate());

	if (m_mercuryDither == 1)
		settings->setValue("server/dither", "on");
	else
		settings->setValue("server/dither", "off");

	if (m_mercuryRandom == 1)
		settings->setValue("server/random", "on");
	else
		settings->setValue("server/random", "off");

	if (m_wideBandData)
		settings->setValue("server/widebandData", "on");
	else
		settings->setValue("server/widebandData", "off");

	if (!m_wideBandData) m_wideBandDisplayStatus = false;

	if (m_wideBandDisplayStatus)
		settings->setValue("server/widebandDisplay", "on");
	else
		settings->setValue("server/widebandDisplay", "off");

	if (m_10MHzSource == 0)
		settings->setValue("server/10mhzsource", "atlas");
	else if (m_10MHzSource == 1)
		settings->setValue("server/10mhzsource", "penelope");
	else if (m_10MHzSource == 2)
		settings->setValue("server/10mhzsource", "mercury");
	else if (m_10MHzSource == 3)
		settings->setValue("server/10mhzsource", "none");
	else
		settings->setValue("server/10mhzsource", "mercury");

	if (m_122_8MHzSource == 0)
		settings->setValue("server/122_88mhzsource", "penelope");
	else if (m_122_8MHzSource == 1)
		settings->setValue("server/122_88mhzsource", "mercury");
	
	if (m_micSource == 0)
		settings->setValue("server/mic_source", "janus");
	else if (m_micSource == 1)
		settings->setValue("server/mic_source", "penelope");

	settings->setValue("server/class", m_RxClass);
	settings->setValue("server/timing", m_RxTiming);
	//settings->setValue("server/mainVolume", (int)(m_mainVolume * 100));

	//if (m_serverMode == QSDR::SDRMode)
		settings->setValue("server/mode", "sdr");

	//settings->setValue("server/mouseWheelFreqStep", m_mouseWheelFreqStep);

	//******************************************************************
	// Alexiares data settings

	// m_alexConfig (qint16)
	//
	// 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
	//   | | | | | | | | | | | | | | |
	//   | | | | | | | | | | | | | | +-----Alex   - manual HPF/LPF filter select (0 = disable, 1 = enable)
	//   | | | | | | | | | | | | | +------ Alex   -	Bypass all HPFs   (0 = disable, 1 = enable)*
	//   | | | | | | | | | | | | +-------- Alex   -	6M low noise amplifier (0 = disable, 1 = enable)*
	//   | | | | | | | | | | | +---------- Alex   -	select 1.5MHz HPF (0 = disable, 1 = enable)*
	//   | | | | | | | | | | +------------ Alex   -	select 6.5MHz HPF (0 = disable, 1 = enable)*
	//   | | | | | | | | | +-------------- Alex   -	select 9.5MHz HPF (0 = disable, 1 = enable)*
	//   | | | | | | | | +---------------- Alex   -	select 13MHz  HPF (0 = disable, 1 = enable)*
	//   | | | | | | | +------------------ Alex   -	select 20MHz  HPF (0 = disable, 1 = enable)*
	//   | | | | | | +-------------------- Alex   - select 160m   LPF (0 = disable, 1 = enable)*
	//   | | | | | +---------------------- Alex   - select 80m    LPF (0 = disable, 1 = enable)*
	//   | | | | +------------------------ Alex   - select 60/40m LPF (0 = disable, 1 = enable)*
	//   | | | +-------------------------- Alex   - select 30/20m LPF (0 = disable, 1 = enable)*
	//   | | +---------------------------- Alex   - select 17/15m LPF (0 = disable, 1 = enable)*
	//   | +------------------------------ Alex   - select 12/10m LPF (0 = disable, 1 = enable)*
	//   +-------------------------------- Alex   - select 6m     LPF (0 = disable, 1 = enable)*

	if (m_alexConfig & 0x01)
		settings->setValue("alex/manual", "on");
	else
		settings->setValue("alex/manual", "off");

	if (m_alexConfig & 0x02)
		settings->setValue("alex/bypassAll", "on");
	else
		settings->setValue("alex/bypassAll", "off");

	if (m_alexConfig & 0x04)
		settings->setValue("alex/amp6m", "on");
	else
		settings->setValue("alex/amp6m", "off");

	settings->setValue("alex/amp6mLo", (int)m_HPFLoFrequencyList.at(5));
	settings->setValue("alex/amp6mHi", (int)m_HPFHiFrequencyList.at(5));

	if (m_alexConfig & 0x08)
		settings->setValue("alex/hpf1_5MHz", "on");
	else
		settings->setValue("alex/hpf1_5MHz", "off");

	settings->setValue("alex/hpf1_5MHzLo", (int)m_HPFLoFrequencyList.at(0));
	settings->setValue("alex/hpf1_5MHzHi", (int)m_HPFHiFrequencyList.at(0));

	if (m_alexConfig & 0x10)
		settings->setValue("alex/hpf6_5MHz", "on");
	else
		settings->setValue("alex/hpf6_5MHz", "off");

	settings->setValue("alex/hpf6_5MHzLo", (int)m_HPFLoFrequencyList.at(1));
	settings->setValue("alex/hpf6_5MHzHi", (int)m_HPFHiFrequencyList.at(1));

	if (m_alexConfig & 0x20)
		settings->setValue("alex/hpf9_5MHz", "on");
	else
		settings->setValue("alex/hpf9_5MHz", "off");

	settings->setValue("alex/hpf9_5MHzLo", (int)m_HPFLoFrequencyList.at(2));
	settings->setValue("alex/hpf9_5MHzHi", (int)m_HPFHiFrequencyList.at(2));

	if (m_alexConfig & 0x40)
		settings->setValue("alex/hpf13MHz", "on");
	else
		settings->setValue("alex/hpf13MHz", "off");

	settings->setValue("alex/hpf13MHzLo", (int)m_HPFLoFrequencyList.at(3));
	settings->setValue("alex/hpf13MHzHi", (int)m_HPFHiFrequencyList.at(3));

	if (m_alexConfig & 0x80)
		settings->setValue("alex/hpf20MHz", "on");
	else
		settings->setValue("alex/hpf20MHz", "off");

	settings->setValue("alex/hpf20MHzLo", (int)m_HPFLoFrequencyList.at(4));
	settings->setValue("alex/hpf20MHzHi", (int)m_HPFHiFrequencyList.at(4));

	if (m_alexConfig & 0x100)
		settings->setValue("alex/lpf160m", "on");
	else
		settings->setValue("alex/lpf160m", "off");

	settings->setValue("alex/lpf160mLo", (int)m_LPFLoFrequencyList.at(0));
	settings->setValue("alex/lpf160mHi", (int)m_LPFHiFrequencyList.at(0));

	if (m_alexConfig & 0x200)
		settings->setValue("alex/lpf80m", "on");
	else
		settings->setValue("alex/lpf80m", "off");

	settings->setValue("alex/lpf80mLo", (int)m_LPFLoFrequencyList.at(1));
	settings->setValue("alex/lpf80mHi", (int)m_LPFHiFrequencyList.at(1));

	if (m_alexConfig & 0x400)
		settings->setValue("alex/lpf60_40m", "on");
	else
		settings->setValue("alex/lpf60_40m", "off");

	settings->setValue("alex/lpf60_40mLo", (int)m_LPFLoFrequencyList.at(2));
	settings->setValue("alex/lpf60_40mHi", (int)m_LPFHiFrequencyList.at(2));

	if (m_alexConfig & 0x800)
		settings->setValue("alex/lpf30_20m", "on");
	else
		settings->setValue("alex/lpf30_20m", "off");

	settings->setValue("alex/lpf30_20mLo", (int)m_LPFLoFrequencyList.at(3));
	settings->setValue("alex/lpf30_20mHi", (int)m_LPFHiFrequencyList.at(3));

	if (m_alexConfig & 0x1000)
		settings->setValue("alex/lpf17_15m", "on");
	else
		settings->setValue("alex/lpf17_15m", "off");

	settings->setValue("alex/lpf17_15mLo", (int)m_LPFLoFrequencyList.at(4));
	settings->setValue("alex/lpf17_15mHi", (int)m_LPFHiFrequencyList.at(4));

	if (m_alexConfig & 0x2000)
		settings->setValue("alex/lpf12_10m", "on");
	else
		settings->setValue("alex/lpf12_10m", "off");

	settings->setValue("alex/lpf12_10mLo", (int)m_LPFLoFrequencyList.at(5));
	settings->setValue("alex/lpf12_10mHi", (int)m_LPFHiFrequencyList.at(5));

	if (m_alexConfig & 0x4000)
		settings->setValue("alex/lpf6m", "on");
	else
		settings->setValue("alex/lpf6m", "off");

	settings->setValue("alex/lpf6mLo", (int)m_LPFLoFrequencyList.at(6));
	settings->setValue("alex/lpf6mHi", (int)m_LPFHiFrequencyList.at(6));


	//***********************************************************************
	for (int i = 0; i < MAX_BANDS; i++) {

		str = "alex/state";
		str.append(m_bandList.at(i).bandString);

		settings->setValue(str, m_alexStates.at(i));
	}

	if (m_alexStates.length() == MAX_BANDS && m_bandList.length() == MAX_BANDS) {

		for (int i = 0; i < MAX_BANDS-1; i++) {

			str = "alex/state";
			str.append(m_bandList.at(i).bandString);

			settings->setValue(str, m_alexStates.at(i));
		}
	}


	//******************************************************************
	// Penny open collector settings

	if (m_pennyOCEnabled)
		settings->setValue("penny/OCenabled", "on");
	else
		settings->setValue("penny/OCenabled", "off");

	if (m_rxJ6pinList.length() == MAX_BANDS-1 && m_txJ6pinList.length() == MAX_BANDS-1 && m_bandList.length() == MAX_BANDS) {

		for (int i = 0; i < MAX_BANDS-1; i++) {

			str = "penny/rxState";
			str.append(m_bandList.at(i).bandString);

			settings->setValue(str, m_rxJ6pinList.at(i));
		}

		for (int i = 0; i < MAX_BANDS-1; i++) {

			str = "penny/txState";
			str.append(m_bandList.at(i).bandString);

			settings->setValue(str, m_txJ6pinList.at(i));
		}
	}


	//******************************************************************
	// receiver data settings

	for (int i = 0; i < MAX_RECEIVERS; i++) {

		QString str = m_rxStringList.at(i);
		str.append("/dspCore");

		if (m_receiverDataList[i].dspCore == QSDR::QtDSP)
			settings->setValue(str, "qtdsp");
		//else
		//	settings->setValue(str, "dttsp");

		str = m_rxStringList.at(i);
		str.append("/audioVolume");
		settings->setValue(str, (int)(m_receiverDataList[i].audioVolume * 100));

		str = m_rxStringList.at(i);
		str.append("/mouseWheelFreqStep");
		settings->setValue(str, (int)(m_receiverDataList[i].mouseWheelFreqStep));

		str = m_rxStringList.at(i);
		str.append("/agcGain");
		settings->setValue(str, m_receiverDataList[i].acgGain);

		str = m_rxStringList.at(i);
		str.append("/agcFixedGain");
		settings->setValue(str, (int) m_receiverDataList[i].agcFixedGain_dB);

		str = m_rxStringList.at(i);
		str.append("/agcMaximumGain");
		settings->setValue(str, (int) m_receiverDataList[i].agcMaximumGain_dB);

		str = m_rxStringList.at(i);
		str.append("/agcSlope");
		settings->setValue(str, (int)m_receiverDataList[i].agcVariableGain);

		str = m_rxStringList.at(i);
		str.append("/agcAttacktime");
		settings->setValue(str, (int)(m_receiverDataList[i].agcAttackTime * 1000));

		str = m_rxStringList.at(i);
		str.append("/agcDecaytime");
		settings->setValue(str, (int)(m_receiverDataList[i].agcDecayTime * 1000));

		str = m_rxStringList.at(i);
		str.append("/agcHangTime");
		settings->setValue(str, (int)(m_receiverDataList[i].agcHangTime * 1000));

		str = m_rxStringList.at(i);
		str.append("/agcLines");

		if (m_receiverDataList[i].agcLines)
			settings->setValue(str, "on");
		else
			settings->setValue(str, "off");

//		str = m_rxStringList.at(i);
//		str.append("/attenuator");
//		str.append(m_bandList.at(j).bandString);
//
//		if (m_receiverDataList.at(i).mercuryAttenuators.at(j))
//			settings->setValue(str, "off");
//		else
//			settings->setValue(str, "on");

		str = m_rxStringList.at(i);
		str.append("/agcMode");

		if (m_receiverDataList[i].agcMode == agcOFF)
			settings->setValue(str, "OFF");
		else if (m_receiverDataList[i].agcMode == agcLONG)
			settings->setValue(str, "LONG");
		else if (m_receiverDataList[i].agcMode == agcSLOW)
			settings->setValue(str, "SLOW");
		else if (m_receiverDataList[i].agcMode == agcMED)
			settings->setValue(str, "MED");
		else if (m_receiverDataList[i].agcMode == agcFAST)
			settings->setValue(str, "FAST");


		//str = m_rxStringList.at(i);
		//str.append("/waterfallTime");
		//settings->setValue(str, m_receiverDataList[i].waterfallTime);

		str = m_rxStringList.at(i);
		str.append("/framesPerSecond");
		settings->setValue(str, m_receiverDataList[i].framesPerSecond);

		str = m_rxStringList.at(i);
		str.append("/waterfallOffsetLo");
		settings->setValue(str, m_receiverDataList[i].waterfallOffsetLo);

		str = m_rxStringList.at(i);
		str.append("/waterfallOffsetHi");
		settings->setValue(str, m_receiverDataList[i].waterfallOffsetHi);

		str = m_rxStringList.at(i);
		str.append("/filterHi");
		settings->setValue(str, m_receiverDataList[i].filterHi);

		str = m_rxStringList.at(i);
		str.append("/filterLo");
		settings->setValue(str, m_receiverDataList[i].filterLo);

		// frequencies
		for (int j = 0; j < MAX_BANDS; j++) {

			str = m_rxStringList.at(i);
			str.append("/lastFrequency");
			str.append(m_bandList.at(j).bandString);

			settings->setValue(str, (int)m_receiverDataList[i].lastFrequencyList.at(j));
		}

		str = m_rxStringList.at(i);
		str.append("/frequency");
		settings->setValue(str, (int)m_receiverDataList[i].frequency);

		for (int j = 0; j < MAX_BANDS; j++) {

			str = m_rxStringList.at(i);
			str.append("/dspmode");
			str.append(m_bandList.at(j).bandString);

			DSPMode mode = m_receiverDataList.at(i).dspModeList.at(j);
			if (mode == LSB)
				settings->setValue(str, "LSB");
			else if (mode == USB)
				settings->setValue(str, "USB");
			else if (mode == DSB)
				settings->setValue(str, "DSB");
			else if (mode == CWL)
				settings->setValue(str, "CWL");
			else if (mode == CWU)
				settings->setValue(str, "CWU");
			else if (mode == FMN)
				settings->setValue(str, "FMN");
			else if (mode == AM)
				settings->setValue(str, "AM");
			else if (mode == DIGU)
				settings->setValue(str, "DIGU");
			else if (mode == SPEC)
				settings->setValue(str, "SPEC");
			else if (mode == DIGL)
				settings->setValue(str, "DIGL");
			else if (mode == SAM)
				settings->setValue(str, "SAM");
			else if (mode == DRM)
				settings->setValue(str, "DRM");
		}

		for (int j = 0; j < MAX_BANDS; j++) {

			str = m_rxStringList.at(i);
			str.append("/attenuator");
			str.append(m_bandList.at(j).bandString);

			if (m_receiverDataList.at(i).mercuryAttenuators.at(j))
				settings->setValue(str, "off");
			else
				settings->setValue(str, "on");
		}

		for (int j = 0; j < MAX_BANDS; j++) {

			str = m_rxStringList.at(i);
			str.append("/dBmPanScaleMin");
			str.append(m_bandList.at(j).bandString);

			settings->setValue(str, (int)m_receiverDataList[i].dBmPanScaleMinList[j]);

			str = m_rxStringList.at(i);
			str.append("/dBmPanScaleMax");
			str.append(m_bandList.at(j).bandString);

			settings->setValue(str, (int)m_receiverDataList[i].dBmPanScaleMaxList[j]);
		}
	}


	//******************************************************************
	// Cuda settings
	//settings->setValue("Cuda/lastDevice", m_cudaLastDevice);


	//******************************************************************
	// Chirp WSPR settings
	settings->setValue("ChirpWSPR/lowerChirpFrequency", m_lowerChirpFreq);
	settings->setValue("ChirpWSPR/upperChirpFrequency", m_upperChirpFreq);
	settings->setValue("ChirpWSPR/chirpAmplitude", int(m_chirpAmplitude * 100));
	settings->setValue("ChirpWSPR/chirpSamplingFrequency", m_chirpSamplingFreq);
	settings->setValue("ChirpWSPR/chirpBufferDurationUs", m_chirpBufferDurationUs);
	settings->setValue("ChirpWSPR/chirpChannels", m_chirpChannels);
	settings->setValue("ChirpWSPR/chirpBufferLength", m_chirpBufferLength);
	settings->setValue("ChirpWSPR/chirpFilterLowerFrequency", m_chirpFilterLowerFrequency);
	settings->setValue("ChirpWSPR/chirpFilterUpperFrequency", m_chirpFilterUpperFrequency);


	//settings->setValue("ChirpWSPR/chirpDownSampleRate", m_chirpDownSampleRate);


	//******************************************************************
	// Graphics settings
	if (m_panadapterMode == QSDRGraphics::FilledLine)
		settings->setValue("graphics/panadapter", "filledline");
	else if (m_panadapterMode == QSDRGraphics::Line)
		settings->setValue("graphics/panadapter", "line");
	else if (m_panadapterMode == QSDRGraphics::Solid)
		settings->setValue("graphics/panadapter", "solid");

	if (m_specAveraging)
		settings->setValue("graphics/averaging", "on");
	else
		settings->setValue("graphics/averaging", "off");

	settings->setValue("graphics/averagingCnt", m_specAveragingCnt);

	if (m_panGrid)
		settings->setValue("graphics/grid", "on");
	else
		settings->setValue("graphics/grid", "off");

	//settings->setValue("graphics/framesPerSecond", m_framesPerSecond);
	settings->setValue("graphics/resolution", m_graphicResolution);

	settings->setValue("graphics/dBmWideBandScaleMin", (int)m_dBmWBScaleMin);
	settings->setValue("graphics/dBmWideBandScaleMax", (int)m_dBmWBScaleMax);
	settings->setValue("graphics/dBmDistScaleMin", (int)m_dBmDistScaleMin);
	settings->setValue("graphics/dBmDistScaleMax", (int)m_dBmDistScaleMax);

	if (m_waterfallColorScheme == QSDRGraphics::simple)
		settings->setValue("graphics/waterfall", "simple");
	else
	if (m_waterfallColorScheme == QSDRGraphics::enhanced)
		settings->setValue("graphics/waterfall", "enhanced");
	else
	if (m_waterfallColorScheme == QSDRGraphics::spectran)
		settings->setValue("graphics/waterfall", "spectran");

	settings->setValue("graphics/sMeterHoldTime", m_sMeterHoldTime);


	// Colors
	settings->setValue("colors/panBackground", QVariant(m_panadapterColors.panBackgroundColor).toString());
	settings->setValue("colors/waterfall", QVariant(m_panadapterColors.waterfallColor).toString());
	settings->setValue("colors/panLine", QVariant(m_panadapterColors.panLineColor).toString());
	settings->setValue("colors/panLineFilled", QVariant(m_panadapterColors.panLineFilledColor).toString());
	settings->setValue("colors/panSolidTop", QVariant(m_panadapterColors.panSolidTopColor).toString());
	settings->setValue("colors/panSolidBottom", QVariant(m_panadapterColors.panSolidBottomColor).toString());
	settings->setValue("colors/panWideBandLine", QVariant(m_panadapterColors.wideBandLineColor).toString());
	settings->setValue("colors/panWideBandFilled", QVariant(m_panadapterColors.wideBandFilledColor).toString());
	settings->setValue("colors/panWideBandSolidTop", QVariant(m_panadapterColors.wideBandSolidTopColor).toString());
	settings->setValue("colors/panWideBandSolidBottom", QVariant(m_panadapterColors.wideBandSolidBottomColor).toString());
	settings->setValue("colors/distanceLine", QVariant(m_panadapterColors.distanceLineColor).toString());
	settings->setValue("colors/distanceLineFilled", QVariant(m_panadapterColors.distanceLineFilledColor).toString());
	settings->setValue("colors/panCenterLine", QVariant(m_panadapterColors.panCenterLineColor).toString());
	settings->setValue("colors/gridLine", QVariant(m_panadapterColors.gridLineColor).toString());

	SETTINGS_DEBUG << "save settings done.";
	return 0;
}

//*******************************************************

QList<long> Settings::getFrequencies() {

	QList<long> frequencies;

	for (int i = 0; i < MAX_RECEIVERS; i++)
		frequencies << m_receiverDataList[i].frequency;

	return frequencies;
}

QString Settings::getDSPModeString(int mode) {

	switch (mode) {

		case 0:
			return QString("LSB");

		case 1:
			return QString("USB");

		case 2:
			return QString("DSB");

		case 3:
			return QString("CWL");

		case 4:
			return QString("CWU");

		case 5:
			return QString("FMN");

		case 6:
			return QString("AM");

		case 7:
			return QString("DIGU");

		case 8:
			return QString("SPEC");

		case 9:
			return QString("DIGL");

		case 10:
			return QString("SAM");

		case 11:
			return QString("DRM");

		default:
			return QString("unknown mode");
	}
}

//*******************************************************

QString Settings::getTitleStr() {

	return m_titleString;
}

QString Settings::getVersionStr() {

	return m_versionString;
}

QString Settings::getSettingsFilename()	{

	return settingsFilename;
}

QString Settings::getCallsign() {

	return m_callsignString;
}

QString Settings::getValue1000(

	double value,			/*!<[in] Value to print. */
	int valuePrefix,		/*!<[in] Value current prefix. */
	QString unitBase)		/*!<[in] Unit base string. */
{
	const int prefixBase = 1000;
	int resPrefix = valuePrefix;

	static const char *prefixTab[prefixSiMax + 1] = {
		"",	/* prefixNothing */
		"k",	/* prefixKilo */
		"M",	/* prefixMega */
		"G",	/* prefixGiga */
		"T",	/* prefixTera */
		"P",	/* prefixPeta */
		"E",	/* prefixExa */
		"Z",	/* prefixZetta */
		"Y",	/* prefixYotta */
	};

	/*while((value > (10 * prefixBase)) && (resPrefix < prefixSiMax)) {
		value /= prefixBase;
		resPrefix++;
	}*/
	while((value > (prefixBase/10)) && (resPrefix < prefixSiMax)) {
		value /= prefixBase;
		resPrefix++;
	}

	return QString("%1 %2%3").arg(value).arg(prefixTab[resPrefix]).arg(unitBase);
}

QString Settings::getValue1024(

	double value,			/*!<[in] Value to print. */
	int valuePrefix,		/*!<[in] Value current prefix. */
	QString unitBase)		/*!<[in] Unit base string. */
{
	const int prefixBase = 1024;
	int resPrefix = valuePrefix;

	static const char *prefixTab[prefixIecMax + 1] = {
		"",	/* prefixNothing */
		"Ki",	/* prefixKibi */
		"Mi",	/* prefixMebi */
		"Gi",	/* prefixGibi */
		"Ti",	/* prefixTebi */
		"Pi",	/* prefixPebi */
		"Ei",	/* prefixExbi */
		"Zi",	/* prefixZebi */
		"Yi",	/* prefixYobi */
	};

	while((value > (10 * prefixBase)) && (resPrefix < prefixIecMax)) {
		value /= prefixBase;
		resPrefix++;
	}

	return QString("%1 %2%3").arg(value).arg(prefixTab[resPrefix]).arg(unitBase);
}

int Settings::getMinimumWidgetWidth() {

	return m_minimumWidgetWidth;
}

int Settings::getMinimumGroupBoxWidth()	{

	return m_minimumGroupBoxWidth;
}

void Settings::debugSystemState() {

	qDebug() << " ";
	SETTINGS_DEBUG << "**********************************************************";
	SETTINGS_DEBUG << "Error:\t\t\t" << qPrintable(getErrorString(m_systemError));
	SETTINGS_DEBUG << "HW Interface:\t\t" << qPrintable(getHWInterfaceModeString(m_hwInterface));
	SETTINGS_DEBUG << "Server Mode:\t\t" << qPrintable(getServerModeString(m_serverMode));
	SETTINGS_DEBUG << "DataEngine State:\t" << qPrintable(getHDataEngineStateString(m_dataEngineState));
	qDebug() << " ";
}

QString Settings::getSDRStyle() {

	return sdrStyle;
}

QString Settings::getWidgetStyle() {

	return widgetStyle;
}

QString Settings::getMainWindowStyle() {

	return mainWindowStyle;
}

QString Settings::getDockStyle() {

	return dockStyle;
}

QString Settings::getDisplayToolbarStyle() {

	return displayToolbarStyle;
}

QString Settings::getMainBtnToolbarStyle() {

	return mainBtnToolbarStyle;
}

QString Settings::getStatusbarStyle() {

	return statusbarStyle;
}

QString Settings::getMessageBoxStyle() {

	return messageBoxStyle;
}

QString Settings::getLineEditStyle() {

	return lineEditStyle;
}

QString Settings::getDialogStyle() {

	return dialogStyle;
}

QString Settings::getColorDialogStyle() {

	return colorDialogStyle;
}

QString Settings::getItemStyle() {

	return itemStyle;
}

QString Settings::getLabelStyle() {

	return labelStyle;
}

QString Settings::getSliderLabelStyle() {

	return sliderLabelStyle;
}

QString Settings::getTableStyle() {

	return tableStyle;
}

QString Settings::getComboBoxStyle() {

	return comboBoxStyle;
}

QString Settings::getSpinBoxStyle() {

	return spinBoxStyle;
}

QString Settings::getDoubleSpinBoxStyle() {

	return dSpinBoxStyle;
}

QString Settings::getMenuStyle() {

	return menuStyle;
}

QString Settings::getMiniButtonStyle() {

	return miniButtonStyle;
}

QString Settings::getVolSliderStyle() {

	return volSliderStyle;
}

QString Settings::getSplitterStyle() {

	return splitterStyle;
}

QString Settings::getFrameStyle() {

	return frameStyle;
}

QString Settings::getTabWidgetStyle() {

	return tabWidgetStyle;
}

//QString Settings::getNewSliderStyle() {
//
//	return m_newSliderStyle;
//}


//*******************************************************

void Settings::setMainPower(QObject *sender, bool power) {

	if (m_mainPower == power) return;

	m_mainPower = power;
	if (power)
		m_mainPower = true;
	else
		m_mainPower = false;

	emit masterSwitchChanged(sender, m_mainPower);
}

bool Settings::getMainPower() {

	return m_mainPower;
}

void Settings::setSystemState(

	QObject *sender,
	QSDR::_Error err,
	QSDR::_HWInterfaceMode hwmode,
	QSDR::_ServerMode mode,
	QSDR::_DataEngineState state)
{
	//Q_UNUSED (sender)

	//QMutexLocker locker(&settingsMutex);

	if (m_systemError != err)
		m_systemError = err;

	if (m_hwInterface != hwmode)
		m_hwInterface = hwmode;

	if (m_serverMode != mode) {

		m_serverMode = mode;

		if (m_serverMode == QSDR::ChirpWSPR)
			setWideBandStatus(false);
	}

	if (m_dataEngineState != state)
		m_dataEngineState = state;

	if (m_dataEngineState == QSDR::DataEngineDown)
		setCurrentReceiver(this, 0);
		//m_currentReceiver = 0;

	//locker.unlock();

	debugSystemState();
	emit systemStateChanged(this, m_systemError, m_hwInterface, m_serverMode, m_dataEngineState);
}

QSDR::_ServerMode Settings::getCurrentServerMode() {

	return m_serverMode;
}

QSDR::_HWInterfaceMode Settings::getHWInterface() {

	return m_hwInterface;
}

QSDR::_DataEngineState Settings::getDataEngineState() {

	return m_dataEngineState;
}

QString	Settings::getErrorString(QSDR::_Error err) {

	QString str;
	switch (err) {

		case 0:
			str = "No error";
			break;

		case 1:
			str = "Not implemented";
			break;

		case 2:
			str = "Hardware IO error";
			break;

		case 3:
			str = "Server mode error";
			break;

		case 4:
			str = "open device error";
			break;

		case 5:
			str = "dataReceiverThread error";
			break;

		case 6:
			str = "dataProcessThread error";
			break;

		case 7:
			str = "widebandDataProcessThread error";
			break;

		case 8:
			str = "audioThread error";
			break;

		case 9:
			str = "ChirpDataProcessThread error";
			break;

		case 10:
			str = "underrun error";
			break;

		case 11:
			str = "firmware error";
			break;

		case 12:
			str = "fatal error";
			break;
	}
	return str;
}

QString	Settings::getHDataEngineStateString(QSDR::_DataEngineState mode) {

	QString str;
	switch (mode) {

		case 0:
			str = "down";
			break;

		case 1:
			str = "up";
			break;
	}
	return str;
}

QString	Settings::getServerModeString(QSDR::_ServerMode mode) {

	QString str;
	switch (mode) {

		case 0:
			str = "no server mode";
			break;

		case 1:
			str = "SDR mode";
			break;

		case 2:
			str = "ChirpWSPR";
			break;

		case 3:
			str = "ChirpWSPRFile";
			break;

		case 4:
			str = "demo";
			break;
	}
	return str;
}

QString	Settings::getHWInterfaceModeString(QSDR::_HWInterfaceMode mode) {

	QString str;
	switch (mode) {

		case 0:
			str = "no interface";
			break;

		case 1:
			str = "Metis";
			break;

		case 2:
			str = "Hermes";
			break;
	}
	return str;
}

void Settings::setTxAllowed(QObject *sender, bool value) {

	if (m_devices.penelopePresence || m_devices.pennylanePresence || (m_hwInterface == QSDR::Hermes))
		m_transmitter.txAllowed = value;
	else
		m_transmitter.txAllowed = false;

	emit txAllowedChanged(sender, m_transmitter.txAllowed);
}

bool Settings::getTxAllowed() {

	return m_transmitter.txAllowed;
}

void Settings::setGraphicsState(

	QObject *sender, 
	QSDRGraphics::_Panadapter panMode,
	QSDRGraphics::_WfScheme waterColorScheme)
{
	Q_UNUSED (sender)

	bool change = false;
	//QMutexLocker locker(&settingsMutex);

	if (m_panadapterMode != panMode) {

		m_panadapterMode = panMode;
		change = true;
	}

	if (m_waterfallColorScheme != waterColorScheme) {

		m_waterfallColorScheme = waterColorScheme;
		change = true;
	}

	if (!change) return;

	//locker.unlock();

	SETTINGS_DEBUG << "graphics mode:" << m_panadapterMode << m_waterfallColorScheme;
	emit graphicModeChanged(this, m_panadapterMode, m_waterfallColorScheme);
}

QSDRGraphics::_Panadapter Settings::getPanadapterMode()	{

	return m_panadapterMode;
}

QSDRGraphics::_WfScheme	Settings::getWaterfallColorScheme()	{

	return m_waterfallColorScheme;
}

//QSDRGraphics::_Colors Settings::getColorItem() {
//
//	return m_colorItem;
//}

void Settings::setDefaultSkin(QObject *sender, bool value) {

	Q_UNUSED (sender)

	m_defaultSkin = value;
}

bool Settings::getDefaultSkin() {

	return m_defaultSkin;
}

void Settings::setSettingsFilename(QString filename) {

	filename = filename.trimmed();

	//QMutexLocker locker(&settingsMutex);

	settingsFilename = filename;
	//locker.unlock();

	emit settingsFilenameChanged(filename);
}

void Settings::setSettingsLoaded(bool value) {

	QMutexLocker locker(&settingsMutex);

	setLoaded = value;

	locker.unlock();

	emit settingsLoadedChanged(setLoaded);
}

bool Settings::getSettingsLoaded() {

	return setLoaded;
}

void Settings::setCPULoad(short load) {

	emit cpuLoadChanged(load);
}

void Settings::setCallsign(const QString &callsign) {

	QString cs = callsign.trimmed();

	QMutexLocker locker(&settingsMutex);

	if (m_callsignString == cs)	return;

	m_callsignString = cs;

	locker.unlock();
	emit callsignChanged();
}

void Settings::setRxList(QList<Receiver *> rxList) {

	emit rxListChanged(rxList);
}

void Settings::setMultiRxView(int view) {

	QMutexLocker locker(&settingsMutex);

	if (m_multiRxView == view) return;
	m_multiRxView = view;

	locker.unlock();
	emit multiRxViewChanged(m_multiRxView);
}

int Settings::getMultiRxView() {

	return m_multiRxView;
}

void Settings::setMetisCardList(QList<TNetworkDevicecard> list) {

	QMutexLocker locker(&settingsMutex);

	m_metisCards = list;

	locker.unlock();
	emit metisCardListChanged(m_metisCards);
}

void Settings::searchHpsdrNetworkDevices() {

	emit searchMetisSignal();
}

void Settings::clearMetisCardList() {

	m_metisCards.clear();

	//emit metisCardListChanged(m_metisCards);
}

void Settings::setCurrentHPSDRDevice(TNetworkDevicecard card) {

	m_currentHPSDRDevice = card;

	emit hpsdrNetworkDeviceChanged(m_currentHPSDRDevice);
}

void Settings::setHPSDRDeviceNumber(int value) {

	m_hpsdrNetworkDevices = value;
	emit networkDeviceNumberChanged(value);
}

void Settings::showNetworkIODialog() {

	emit showNetworkIO();
}

void Settings::showWarningDialog(const QString &msg) {

	emit showWarning(msg);
}

void Settings::addNetworkIOComboBoxEntry(QString str) {

	emit networkIOComboBoxEntryAdded(str);
}

void Settings::clearNetworkIOComboBoxEntry() {

	emit clearNetworkIOComboBoxEntrySignal();
}

void Settings::setPBOPresence(bool value) {

	m_pboFound = value;
}

bool Settings::getPBOPresence()	{

	return m_pboFound;
}

void Settings::setFBOPresence(bool value) {

	m_fboFound = value;
}

bool Settings::getFBOPresence()	{

	return m_fboFound;
}

//*******************************
// Network settings

void Settings::setNumberOfNetworkInterfaces(int value) {

	m_NetworkInterfacesNo = value;
}

void Settings::addServerNetworkInterface(QString nicName, QString ipAddress) {

	emit newServerNetworkInterface(nicName, ipAddress);
}

void Settings::addHPSDRDeviceNIC(QString nicName, QString ipAddress) {

	emit newHPSDRDeviceNIC(nicName, ipAddress);
}

void Settings::setServerNetworkInterface(int index) {

	setServerAddr(this, this->m_ipAddressesList.at(index).toString());

	//qDebug() << "m_networkInterfaces.at(index).humanReadableName():" << m_networkInterfaces.at(index).humanReadableName();
	//qDebug() << "m_ipAddressesList.at(index).toString():" << m_ipAddressesList.at(index).toString();
	
	QString message = "[settings]: network interface set to: %1 (%2)."; 
	emit messageEvent(
		message.arg(
			m_networkInterfaces.at(index).humanReadableName(),
			m_ipAddressesList.at(index).toString() ));
	
}

void Settings::setHPSDRDeviceNIC(int index) {

	setHPSDRDeviceLocalAddr(this, this->m_ipAddressesList.at(index).toString());

	QString message = "[settings]: HPSDR device network interface set to: %1 (%2)."; 
	emit messageEvent(
		message.arg(
			m_networkInterfaces.at(index).humanReadableName(),
			m_ipAddressesList.at(index).toString() ));
	
}

void Settings::setServerWidgetNIC(int index) {

	/*QString message = "[server]: network interface set to: %1 (%2)."; 
	emit messageEvent(
		message.arg(
			m_networkInterfaces.at(index).humanReadableName(),
			m_ipAddressesList.at(index).toString() ));*/

	emit serverNICChanged(index);
}

void Settings::setHPSDRWidgetNIC(int index) {

	/*QString message = "[server]: HPSDR device network interface set to: %1 (%2)."; 
	emit messageEvent(
		message.arg(
			m_networkInterfaces.at(index).humanReadableName(),
			m_ipAddressesList.at(index).toString() ));*/

	emit hpsdrDeviceNICChanged(index);
}

void Settings::setServerAddr(QObject *sender, QString addr) {

	QMutexLocker locker(&settingsMutex);

	m_serverAddress = addr;

	locker.unlock();
	emit serverAddrChanged(sender, m_serverAddress);
}

QString Settings::getServerAddr() {

	return m_serverAddress;
}

void Settings::setHPSDRDeviceLocalAddr(QObject *sender, QString addr) {

	QMutexLocker locker(&settingsMutex);

	m_hpsdrDeviceLocalAddr = addr;

	locker.unlock();
	emit hpsdrDeviceLocalAddrChanged(sender, m_hpsdrDeviceLocalAddr);
}

QString Settings::getHPSDRDeviceLocalAddr()	{

	return m_hpsdrDeviceLocalAddr;
}

void Settings::setServerPort(QObject *sender, quint16 port) {

	QMutexLocker locker(&settingsMutex);

	m_serverPort = port;

	locker.unlock();
	emit serverPortChanged(sender, m_serverPort);
}

quint16 Settings::getServerPort() {

	return m_serverPort;
}

void Settings::setListenPort(QObject *sender, quint16 port) {

	QMutexLocker locker(&settingsMutex);

	m_listenerPort = port;

	locker.unlock();
	emit listenPortChanged(sender, m_listenerPort);
}

quint16 Settings::getListenPort() {

	return m_listenerPort;
}

void Settings::setAudioPort(QObject *sender, quint16 port) {

	QMutexLocker locker(&settingsMutex);
	m_audioPort = port;
	locker.unlock();

	emit audioPortChanged(sender, m_audioPort);
}

quint16 Settings::getAudioPort() {

	return m_audioPort;
}

void Settings::setMetisPort(QObject *sender, quint16 port) {

	QMutexLocker locker(&settingsMutex);
	m_metisPort = port;
	locker.unlock();

	emit metisPortChanged(sender, m_metisPort);
}

quint16	Settings::getMetisPort() {

	return m_metisPort;
}

void Settings::setClientConnected(QObject *sender, bool value) {

	QMutexLocker locker(&settingsMutex);
	m_clientConnected = value;
	locker.unlock();

	emit clientConnectedChanged(sender, m_clientConnected);
}

bool Settings::getClientConnected() {

	return m_clientConnected;
}

void Settings::setClientNoConnected(QObject* sender, int client) {

	QMutexLocker locker(&settingsMutex);
	m_clientNoConnected = client;
	locker.unlock();

	emit clientNoConnectedChanged(sender, m_clientNoConnected);
}

void Settings::setAudioRx(QObject* sender, int rx) {

	emit audioRxChanged(sender, rx);
}

void Settings::setConnected(QObject *sender, bool value) {

	QMutexLocker locker(&settingsMutex);
	m_connected = value;
	locker.unlock();

	emit connectedChanged(sender, m_connected);
}

bool Settings::getConnected() {

	return m_connected;
}

void Settings::clientDisconnected(int client) {

	emit clientDisconnectedEvent(client);
}

void Settings::setRxConnectedStatus(QObject* sender, int rx, bool value) {

	emit rxConnectedStatusChanged(sender, rx, value);
}

void Settings::setSocketBufferSize(QObject *sender, int value) {

	m_socketBufferSize = value;
	emit socketBufferSizeChanged(sender, value);
}

void Settings::setManualSocketBufferSize(QObject *sender, bool value) {

	m_manualSocketBufferSize = value;
	emit manualSocketBufferChanged(sender, m_manualSocketBufferSize);
}
 
 
//*******************************
// HPSDR hardware presence and firmware versions

THPSDRDevices Settings::getHPSDRDevices() {

	return m_devices;
}

void Settings::setHPSDRDevices(QObject *sender, THPSDRDevices devices) {

}

void Settings::checkHPSDRDevices() {

	SETTINGS_DEBUG << "mercuryPresence: " << m_devices.mercuryPresence;
	SETTINGS_DEBUG << "penelopePresence: " << m_devices.penelopePresence;
	SETTINGS_DEBUG << "pennylanePresence: " << m_devices.pennylanePresence;
	SETTINGS_DEBUG << "excaliburPresence: " << m_devices.excaliburPresence;
	SETTINGS_DEBUG << "alexPresence: " << m_devices.alexPresence;
	SETTINGS_DEBUG << "hermesPresence: " << m_devices.hermesPresence;

	if (m_hpsdrHardware == 0) { // 0 = Mercury/Penelope

		if (m_devices.penelopePresence && m_devices.pennylanePresence) {

			m_devices.pennylanePresence = false;
			m_devices.penelopePresence = true;
			SETTINGS_DEBUG << "settings specifies both Penelope and Pennylane - choosing Penelope !";
		}

		if (m_devices.hermesPresence) {

			m_devices.hermesPresence = false;
			SETTINGS_DEBUG << "settings specifies also Hermes - choosing Mercury/Penelope/Pennylane !";
		}
	}
	else if (m_hpsdrHardware == 1) { // 1 = Hermes

		if (m_devices.mercuryPresence   ||
			m_devices.penelopePresence  ||
			m_devices.pennylanePresence ||
			m_devices.excaliburPresence)
		{
			m_devices.mercuryPresence = false;
			m_devices.penelopePresence = false;
			m_devices.pennylanePresence = false;
			m_devices.excaliburPresence = false;
			SETTINGS_DEBUG << "settings specifies HPSDR Modules - choosing Hermes !";
		}
	}
}


void Settings::setHPSDRHardware(int value) {

	m_hpsdrHardware = value; // 0 = Mercury/Penelope, 1 = Hermes, 2 = Cyclops

	emit hpsdrHardwareChanged(m_hpsdrHardware);
}

void Settings::setHermesVersion(int value) {

	QMutexLocker locker(&settingsMutex);
	m_devices.hermesFWVersion = value;
	locker.unlock();

	emit hermesVersionChanged(m_devices.hermesFWVersion);
}

void Settings::setMercuryPresence(bool value) {

	m_devices.mercuryPresence = value;

	emit mercuryPresenceChanged(m_devices.mercuryPresence);
}

void Settings::setMercuryVersion(int value) {

	QMutexLocker locker(&settingsMutex);
	m_devices.mercuryFWVersion = value;
	locker.unlock();

	emit mercuryVersionChanged(m_devices.mercuryFWVersion);
}

void Settings::setPenelopePresence(bool value) {

	m_devices.penelopePresence = value;
	setTxAllowed(this, value);

	emit penelopePresenceChanged(m_devices.penelopePresence);
}

void Settings::setPenelopeVersion(int value) {

	QMutexLocker locker(&settingsMutex);
	m_devices.penelopeFWVersion = value;
	locker.unlock();

	emit penelopeVersionChanged(m_devices.penelopeFWVersion);
}

void Settings::setPennyLanePresence(bool value) {

	m_devices.pennylanePresence = value;
	setTxAllowed(this, value);

	emit pennyLanePresenceChanged(m_devices.pennylanePresence);
}

void Settings::setPennyLaneVersion(int value) {

	QMutexLocker locker(&settingsMutex);
	m_devices.pennylaneFWVersion = value;
	locker.unlock();

	emit pennyLaneVersionChanged(m_devices.pennylaneFWVersion);
}

void Settings::setAlexPresence(bool value) {

	m_devices.alexPresence = value;

	emit alexPresenceChanged(m_devices.alexPresence);
}

void Settings::setExcaliburPresence(bool value) {

	m_devices.excaliburPresence = value;

	emit excaliburPresenceChanged(m_devices.excaliburPresence);
}


void Settings::setMetisVersion(int value) {

	QMutexLocker locker(&settingsMutex);
	m_devices.metisFWVersion = value;
	locker.unlock();

	emit metisVersionChanged(m_devices.metisFWVersion);
}

void Settings::setProtocolSync(int value) {

	emit protocolSyncChanged(value);
}

void Settings::setADCOverflow(int value) {

	emit adcOverflowChanged(value);
}

void Settings::setPacketLoss(int value) {

	emit packetLossChanged(value);
}

void Settings::setSendIQ(int value) {

	emit sendIQSignalChanged(value);
}

void Settings::setRcveIQ(int value) {

	emit rcveIQSignalChanged(value);
}

/**
 * Set the number of receivers to be supported by this server
 * \param r The number of receivers: 0 to 6
 * This value is embedded into the command & control bytes that are sent to Mercury.
 * Thus it determines how the I & Q samples read from EP6 are placed in the data stream to dspservers.
 */
void Settings::setReceivers(QObject *sender, int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_mercuryReceivers == value) return;
	if (value > MAX_RECEIVERS) value = MAX_RECEIVERS;

	m_mercuryReceivers = value;
	locker.unlock();

	SETTINGS_DEBUG << "set number of receivers to: " << m_mercuryReceivers;
	emit numberOfRXChanged(sender, value);
}

void Settings::setCoupledReceivers(QObject *sender, int value) {

	if (value == 0)
		m_frequencyRx1onRx2 = false;
	else
	if (value == 12)
		m_frequencyRx1onRx2 = true;

	emit coupledRxChanged(sender, value);
}

//void Settings::setReceiver(QObject *sender, int value) {
//
//	QMutexLocker locker(&settingsMutex);
//
//	if (m_currentReceiver == value) return;
//	if (value > MAX_RECEIVERS) value = MAX_RECEIVERS;
//
//	m_currentReceiver = value;
//	locker.unlock();
//
//	SETTINGS_DEBUG << "switch to receiver: " << m_currentReceiver;
//	emit receiverChanged(value);
//	emit frequencyChanged(this, true, value, m_receiverDataList[value].frequency);
//}

void Settings::setCurrentReceiver(QObject *sender, int value) {

	//SETTINGS_DEBUG << "sender: " << sender;
	QMutexLocker locker(&settingsMutex);

	if (value > MAX_RECEIVERS) {

		SETTINGS_DEBUG << "receiver number > MAX_RECEIVERS; setting to MAX_RECEIVERS.";
		value = MAX_RECEIVERS;
	}

	m_currentReceiver = value;

	HamBand band = m_receiverDataList.at(m_currentReceiver).hamBand;
	DSPMode mode = m_receiverDataList.at(m_currentReceiver).dspModeList[band];
	locker.unlock();

	setMercuryAttenuator(this, m_receiverDataList.at(m_currentReceiver).mercuryAttenuators.at(band));
	setFramesPerSecond(this, m_currentReceiver, m_receiverDataList.at(m_currentReceiver).framesPerSecond);

	SETTINGS_DEBUG << "switch to receiver: " << m_currentReceiver;
	emit currentReceiverChanged(sender, value);
	emit frequencyChanged(sender, true, value, m_receiverDataList.at(m_currentReceiver).frequency);
	emit hamBandChanged(sender, m_currentReceiver, false, band);
	emit dspModeChanged(sender, m_currentReceiver, mode);
	emit mouseWheelFreqStepChanged(sender, m_currentReceiver, m_receiverDataList.at(m_currentReceiver).mouseWheelFreqStep);
}

void Settings::setSampleRate(QObject *sender, int value) {

	QMutexLocker locker(&settingsMutex);
	
	switch (value)	{
	
		case 48000:
			m_sampleRate = value;
			m_mercurySpeed = 0;
			m_outputSampleIncrement = 1;
			m_chirpDownSampleRate = 4;
			break;
			
		case 96000:
			m_sampleRate = value;
			m_mercurySpeed = 1;
			m_outputSampleIncrement = 2;
			m_chirpDownSampleRate = 8;
			break;
			
		case 192000:
			m_sampleRate = value;
			m_mercurySpeed = 2;
			m_outputSampleIncrement = 4;
			m_chirpDownSampleRate = 16;
			break;
			
		default:
			SETTINGS_DEBUG << "Invalid sample rate (48000,96000,192000)!\n";
			break;
	}

	for (int i = 0; i < MAX_RECEIVERS; i++)
		m_receiverDataList[i].sampleRate = m_sampleRate;
	
	emit sampleRateChanged(sender, value);
}

void Settings::setMercuryAttenuator(QObject *sender, int value) {

	//QMutexLocker locker(&settingsMutex);

	if (m_receiverDataList.at(m_currentReceiver).mercuryAttenuators.length() != MAX_BANDS)
		return;

	HamBand band = m_receiverDataList[m_currentReceiver].hamBand;
	m_receiverDataList[m_currentReceiver].mercuryAttenuators[band] = value;

	emit mercuryAttenuatorChanged(sender, band, value);
}

QList<int> Settings::getMercuryAttenuators(int rx) {

	return m_receiverDataList[rx].mercuryAttenuators;
}

void Settings::setDither(QObject *sender, int value) {

	QMutexLocker locker(&settingsMutex);
	m_mercuryDither = value;

	emit ditherChanged(sender, value);
}

void Settings::setRandom(QObject *sender, int value) {

	QMutexLocker locker(&settingsMutex);
	m_mercuryRandom = value;

	emit randomChanged(sender, value);
}

void Settings::set10MhzSource(QObject *sender, int source) {

	QMutexLocker locker(&settingsMutex);

	m_10MHzSource = source;
	emit src10MhzChanged(sender, source);
}

void Settings::set122_88MhzSource(QObject *sender, int source) {

	QMutexLocker locker(&settingsMutex);

	m_122_8MHzSource = source;
	emit src122_88MhzChanged(sender, source);
}

void Settings::setMicSource(QObject *sender, int source) {

	QMutexLocker locker(&settingsMutex);

	m_micSource = source;
	emit micSourceChanged(sender, source);
}

void Settings::setClass(QObject *sender, int value) {

	QMutexLocker locker(&settingsMutex);

	m_RxClass = value;
	emit classChanged(sender, value);
}

void Settings::setTiming(QObject *sender, int value) {

	QMutexLocker locker(&settingsMutex);

	m_RxTiming = value;
	emit timingChanged(sender, value);
}

void Settings::setMouseWheelFreqStep(QObject *sender, int rx, qreal value) {

	QMutexLocker locker(&settingsMutex);

	//m_mouseWheelFreqStep = value;
	m_receiverDataList[rx].mouseWheelFreqStep = value;
	emit mouseWheelFreqStepChanged(sender, rx, value);
}

double Settings::getMouseWheelFreqStep(int rx) {

	return m_receiverDataList[rx].mouseWheelFreqStep;
}

qreal Settings::getMainVolume(int rx) {

	return m_receiverDataList[rx].audioVolume;
}

void Settings::setMainVolume(QObject *sender, int rx, float volume) {

	if (volume < 0) volume = 0.0f;
	if (volume > 1) volume = 1.0f;

	QMutexLocker locker(&settingsMutex);

	//if (m_receiverDataList[rx].audioVolume == volume) return;
	m_receiverDataList[rx].audioVolume = volume;

	emit mainVolumeChanged(sender, rx, volume);
}

void Settings::setMainVolumeMute(QObject *sender, bool value) {

}

void Settings::setFrequency(int rx, long frequency) {

	QMutexLocker locker(&settingsMutex);

	HamBand band = getBandFromFrequency(m_bandList, frequency);

	m_receiverDataList[rx].frequency = frequency;
	m_receiverDataList[rx].hamBand = band;
	m_receiverDataList[rx].lastFrequencyList[(int) band] = frequency;
}

void Settings::setFrequency(QObject *sender, bool value, int rx, long frequency) {

	//SETTINGS_DEBUG << "sender: " << sender;
	QMutexLocker locker(&settingsMutex);

	if (m_receiverDataList.at(rx).frequency == frequency) return;
	m_receiverDataList[rx].frequency = frequency;

	HamBand band = getBandFromFrequency(m_bandList, frequency);
	m_receiverDataList[rx].lastFrequencyList[(int) band] = frequency;

	locker.unlock();
	if (m_receiverDataList.at(rx).hamBand != band)
		setHamBand(this, rx, false, band);

	//m_receiverDataList[rx].lastFrequencyList[(int) band] = frequency;

	emit frequencyChanged(sender, value, rx, frequency);
}

void Settings::setHamBand(QObject *sender, int rx, bool byButton, HamBand band) {

	//SETTINGS_DEBUG << "sender: " << sender;
	QMutexLocker locker(&settingsMutex);
	if (m_receiverDataList[rx].hamBand == band && sender != this)
		return;

	m_receiverDataList[rx].hamBand = band;

	if (m_receiverDataList[rx].hamBand == (HamBand) gen)
		setTxAllowed(this, false);
	else
		setTxAllowed(this, true);

	locker.unlock();

	setDSPMode(this, rx, m_receiverDataList.at(rx).dspModeList.at(band));
	setMercuryAttenuator(this, m_receiverDataList[rx].mercuryAttenuators[band]);

	emit hamBandChanged(sender, rx, byButton, band);
}

HamBand Settings::getCurrentHamBand(int rx) {

	return m_receiverDataList[rx].hamBand;
}

void Settings::setDSPMode(QObject *sender, int rx, DSPMode mode) {

	//SETTINGS_DEBUG << "sender: " << sender;
	HamBand band = m_receiverDataList[m_currentReceiver].hamBand;

	m_receiverDataList[rx].dspModeList[band] = mode;
	setRXFilter(this, rx, m_defaultFilterList.at((int) mode).filterLo, m_defaultFilterList.at((int) mode).filterHi);

	emit dspModeChanged(sender, rx, mode);
}

AGCMode Settings::getAGCMode(int rx) {

	return m_receiverDataList.at(rx).agcMode;
}

QString Settings::getAGCModeString(int rx) {

	AGCMode mode = getAGCMode(rx);
	QString str;
	switch (mode) {

		case (AGCMode) agcOFF:
			str = "Off";
			break;

		case (AGCMode) agcLONG:
			str = "Long";
			break;

		case (AGCMode) agcSLOW:
			str = "Slow";
			break;

		case (AGCMode) agcMED:
			str = "Med";
			break;

		case (AGCMode) agcFAST:
			str = "Fast";
			break;

		case (AGCMode) agcUser:
			str = "User";
			break;
	}
	return str;
}

void Settings::setAGCMode(QObject *sender, int rx, AGCMode mode) {

	QMutexLocker locker(&settingsMutex);

	if (m_receiverDataList[rx].agcMode == mode) return;
	m_receiverDataList[rx].agcMode = mode;

	bool hang;
	if (mode == (AGCMode) agcOFF || mode == (AGCMode) agcMED || mode == (AGCMode) agcFAST) {

		m_receiverDataList[rx].hangEnabled = false;
		hang = false;
		if (mode == (AGCMode) agcOFF)
			emit agcFixedGainChanged_dB(sender, rx, m_receiverDataList[rx].agcFixedGain_dB);
	}
	else {

		m_receiverDataList[rx].hangEnabled = true;
		hang = true;
	}

	emit agcModeChanged(sender, rx, mode, hang);
	emit agcHangEnabledChanged(sender, rx, hang);
}

void Settings::setAGCShowLines(QObject *sender, int rx, bool value) {

	Q_UNUSED(sender)

		m_receiverDataList[rx].agcLines = value;
}

int Settings::getAGCGain(int rx) {

	return m_receiverDataList[rx].acgGain;
}

void Settings::setAGCGain(QObject *sender, int rx, int value) {

	//QMutexLocker locker(&settingsMutex);

	if (m_receiverDataList[rx].acgGain == value) return;
	m_receiverDataList[rx].acgGain = value;
	//SETTINGS_DEBUG << "acgGain " << value;
	emit agcGainChanged(sender, rx, value);
}

void Settings::setAGCMaximumGain_dB(QObject *sender, int rx, qreal value) {

	//QMutexLocker locker(&settingsMutex);

	if (m_receiverDataList[rx].agcMaximumGain_dB == value) return;
	m_receiverDataList[rx].agcMaximumGain_dB = value;

	//SETTINGS_DEBUG << "agcMaximumGain_dB = " << m_receiverDataList[rx].agcMaximumGain_dB << " (sender: " << sender << ")";
	emit agcMaximumGainChanged_dB(sender, rx, value);
}

qreal Settings::getAGCMaximumGain_dB(int rx) {

	return m_receiverDataList[rx].agcMaximumGain_dB;
}

void Settings::setAGCFixedGain_dB(QObject *sender, int rx, qreal value) {

	//QMutexLocker locker(&settingsMutex);

	if (m_receiverDataList[rx].agcFixedGain_dB == value) return;
	m_receiverDataList[rx].agcFixedGain_dB = value;

	//SETTINGS_DEBUG << "m_receiverDataList[rx].agcFixedGain_dB = " << m_receiverDataList[rx].agcFixedGain_dB;
	emit agcFixedGainChanged_dB(sender, rx, value);
}

qreal Settings::getAGCFixedGain_dB(int rx) {

	return m_receiverDataList[rx].agcFixedGain_dB;
}

void Settings::setAGCThreshold_dB(QObject *sender, int rx, qreal value) {

	//QMutexLocker locker(&settingsMutex);

	if (m_receiverDataList[rx].acgThreshold_dB == value) return;
	m_receiverDataList[rx].acgThreshold_dB = value;

	//SETTINGS_DEBUG << "acgThreshold = " << m_receiverDataList[rx].acgThreshold;
	emit agcThresholdChanged_dB(sender, rx, value);
}

void Settings::setAGCHangThresholdSlider(QObject *sender, int rx, qreal value) {

	emit agcHangThresholdSliderChanged(sender, rx, value);
}

int Settings::getAGCHangThreshold(int rx) {

	return m_receiverDataList[rx].agcHangThreshold;
}

void Settings::setAGCHangThreshold(QObject *sender, int rx, int value) {

	//QMutexLocker locker(&settingsMutex);

	if (m_receiverDataList[rx].agcHangThreshold == value) return;
	m_receiverDataList[rx].agcHangThreshold = value;

	//SETTINGS_DEBUG << "agcHangThreshold = " << m_receiverDataList[rx].agcHangThreshold;
	emit agcHangThresholdChanged(sender, rx, value);
}

int Settings::getAGCHangLeveldB(int rx) {

	return m_receiverDataList[rx].agcHangThreshold;
}

void Settings::setAGCHangLevel_dB(QObject *sender, int rx, qreal value) {

	//QMutexLocker locker(&settingsMutex);

	if (m_receiverDataList[rx].agcHangLevel == value) return;
	m_receiverDataList[rx].agcHangLevel = value;

	//SETTINGS_DEBUG << "agcHangLevel = " << m_receiverDataList[rx].agcHangLevel;
	emit agcHangLevelChanged_dB(sender, rx, value);
}

void Settings::setAGCLineLevels(QObject *sender, int rx, qreal thresh, qreal hang) {

	if (m_currentReceiver != rx) return;

	m_receiverDataList[rx].agcHangLevel = hang;
	//SETTINGS_DEBUG << "agcHangLevel = " << m_receiverDataList[rx].agcHangLevel;

	emit agcLineLevelsChanged(sender, rx, thresh, hang);
}

void Settings::setAGCVariableGain_dB(QObject *sender, int rx, qreal value) {

	if (m_currentReceiver != rx) return;

	if (m_receiverDataList[rx].agcVariableGain == value) return;
	m_receiverDataList[rx].agcVariableGain = value;

	SETTINGS_DEBUG << "agcVariableGain = " << m_receiverDataList[rx].agcVariableGain;
	emit agcVariableGainChanged_dB(sender, rx, value);
}

void Settings::setAGCAttackTime(QObject *sender, int rx, qreal value) {

	if (m_currentReceiver != rx) return;

	if (m_receiverDataList[rx].agcAttackTime == value) return;
	m_receiverDataList[rx].agcAttackTime = value;

	SETTINGS_DEBUG << "agcAttackTime = " << m_receiverDataList[rx].agcAttackTime;
	emit agcAttackTimeChanged(sender, rx, value);
}

void Settings::setAGCDecayTime(QObject *sender, int rx, qreal value) {

	if (m_currentReceiver != rx) return;

	if (m_receiverDataList[rx].agcDecayTime == value) return;
	m_receiverDataList[rx].agcDecayTime = value;

	SETTINGS_DEBUG << "agcDecayTime = " << m_receiverDataList[rx].agcDecayTime;
	emit agcDecayTimeChanged(sender, rx, value);
}

void Settings::setAGCHangTime(QObject *sender, int rx, qreal value) {

	if (m_currentReceiver != rx) return;

	if (m_receiverDataList[rx].agcHangTime == value) return;
	m_receiverDataList[rx].agcHangTime = value;

	SETTINGS_DEBUG << "agcHangTime = " << m_receiverDataList[rx].agcHangTime;
	emit agcHangTimeChanged(sender, rx, value);
}

void Settings::setRXFilter(QObject *sender, int rx, qreal low, qreal high) {

	QMutexLocker locker(&settingsMutex);

	if (m_filterFrequencyLow == low && m_filterFrequencyHigh == high) return;

	m_filterFrequencyLow = low;
	m_filterFrequencyHigh = high;
	m_receiverDataList[rx].filterLo = low;
	m_receiverDataList[rx].filterHi = high;

	emit filterFrequenciesChanged(sender, rx, m_filterFrequencyLow, m_filterFrequencyHigh);
}

void Settings::setIQPort(QObject *sender, int rx, int port) {

	emit iqPortChanged(sender, rx, port);
}

void Settings::setWideBandStatus(bool value) {

	QMutexLocker locker(&settingsMutex);

	m_wideBandDisplayStatus = value;
	emit wideBandStausChanged(m_wideBandDisplayStatus);
}

void Settings::setWideBandData(bool value) {

	QMutexLocker locker(&settingsMutex);

	m_wideBandData = value;
	emit wideBandDataChanged(value);
}

void Settings::setWidebandBuffers(QObject *sender, int value) {

	Q_UNUSED(sender)

	QMutexLocker locker(&settingsMutex);
	m_wbBuffers = value;
}
 
void Settings::setSpectrumBuffer(int rx, const float* buffer) {

	emit spectrumBufferChanged(rx, buffer);
}

void Settings::setPostSpectrumBuffer(int rx, const float* buffer) {

	emit postSpectrumBufferChanged(rx, buffer);
}

void Settings::setSMeterValue(int rx, float value) {

	emit sMeterValueChanged(rx, value);
}

void Settings::setWidebandSpectrumBuffer(const qVectorFloat &buffer) {

	emit widebandSpectrumBufferChanged(buffer);
}

void Settings::resetWidebandSpectrumBuffer() {

	emit widebandSpectrumBufferReset();
}

// Alex configuration:
//
// manual 		  0
// bypassAll 	  1
// amp6m		  2
// hpf1_5MHz	  3
// hpf6_5MHz	  4
// hpf9_5MHz	  5
// hpf13MHz		  6
// hpf20MHz		  7
// lpf160m		  8
// lpf80m		  9
// lpf60_40m	 10
// lpf30_20m	 11
// lpf17_15m	 12
// lpf12_10m	 13
// lpf6m		 14

// m_alexConfig (qint16)
//
// 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
//   | | | | | | | | | | | | | | |
//   | | | | | | | | | | | | | | +-----Alex   - manual HPF/LPF filter select (0 = disable, 1 = enable)
//   | | | | | | | | | | | | | +------ Alex   -	Bypass all HPFs   (0 = disable, 1 = enable)*
//   | | | | | | | | | | | | +-------- Alex   -	6M low noise amplifier (0 = disable, 1 = enable)*
//   | | | | | | | | | | | +---------- Alex   -	select 1.5MHz HPF (0 = disable, 1 = enable)*
//   | | | | | | | | | | +------------ Alex   -	select 6.5MHz HPF (0 = disable, 1 = enable)*
//   | | | | | | | | | +-------------- Alex   -	select 9.5MHz HPF (0 = disable, 1 = enable)*
//   | | | | | | | | +---------------- Alex   -	select 13MHz  HPF (0 = disable, 1 = enable)*
//   | | | | | | | +------------------ Alex   -	select 20MHz  HPF (0 = disable, 1 = enable)*
//   | | | | | | +-------------------- Alex   - select 160m   LPF (0 = disable, 1 = enable)*
//   | | | | | +---------------------- Alex   - select 80m    LPF (0 = disable, 1 = enable)*
//   | | | | +------------------------ Alex   - select 60/40m LPF (0 = disable, 1 = enable)*
//   | | | +-------------------------- Alex   - select 30/20m LPF (0 = disable, 1 = enable)*
//   | | +---------------------------- Alex   - select 17/15m LPF (0 = disable, 1 = enable)*
//   | +------------------------------ Alex   - select 12/10m LPF (0 = disable, 1 = enable)*
//   +-------------------------------- Alex   - select 6m     LPF (0 = disable, 1 = enable)*

void Settings::setAlexConfiguration(QObject *sender, quint16 conf) {

	Q_UNUSED (sender)

	QMutexLocker locker(&settingsMutex);
	m_alexConfig = conf;

	emit alexConfigurationChanged(m_alexConfig);
}

void Settings::setAlexHPFLoFrequencies(int filter, long value) {

	m_HPFLoFrequencyList[filter] = value;
}

void Settings::setAlexHPFHiFrequencies(int filter, long value) {

	m_HPFHiFrequencyList[filter] = value;
}

void Settings::setAlexLPFLoFrequencies(int filter, long value) {

	m_LPFLoFrequencyList[filter] = value;
}

void Settings::setAlexLPFHiFrequencies(int filter, long value) {

	m_LPFHiFrequencyList[filter] = value;
}

/*
 * Alex state encoding
 * We use the same encoding as in the KISS Konsole by George Byrkit, K9TRV
 * AA.TT.BBB.RR
 * AA is RX Attenuator
 * TT is TX antenna selection (0 is NOT valid!)
 * BBB is RX special selection
 * RR is TX antenna to use for RX (0 is NOT valid!)
 *
 */
void Settings::setAlexState(QObject *sender, int pos, int value) {

	Q_UNUSED (sender)

	if (m_alexStates.length() != MAX_BANDS)
		return;
	else {

		if (m_alexStates.at(pos) == value)
			return;

		int state = checkAlexState(value);
		//qDebug() << "alex state at pos: " << pos <<" = " << state;

		m_alexStates.replace(pos, state);

		emit alexStateChanged((HamBand) pos, m_alexStates);
	}
}

void Settings::setAlexState(QObject *sender, int value) {

	HamBand band = m_receiverDataList[m_currentReceiver].hamBand;

	setAlexState(this, band, value);
}

void Settings::setAlexStates(QObject *sender, const QList<int> &states) {

	Q_UNUSED (sender)

	if (m_alexStates == states) return;

	m_alexStates = states;

	emit alexStatesChanged(m_alexStates);
}

// check Alex state - adapted from KISS Konsole, (c) George Byrkit, K9TRV
int Settings::checkAlexState(int state) {

	if ((state & 0x3) == 0) {

		// if rx antenna selector is 0, set it to 1
		state |= 1;
	}

	if (((state >> 5) & 0x3) == 0) {

		// if tx antenna selector is 0, set it to 1
		state |= 33;
	}
	return state;
}

void Settings::setAlexToManual(QObject *sender, bool value) {

	//QMutexLocker locker(&settingsMutex);

	emit alexManualStateChanged(sender, value);
}

void Settings::setRxJ6Pin(QObject *sender, HamBand band, int value) {

	Q_UNUSED (sender)

	if (m_rxJ6pinList.length() != MAX_BANDS-1) return;
	if (m_rxJ6pinList[band] == value) return;

	m_rxJ6pinList[band] = value;

	emit rxJ6PinsChanged(m_rxJ6pinList);
}

void Settings::setRxJ6Pins(QObject * sender, const QList<int> &states) {

	Q_UNUSED (sender)

	//if (m_rxJ6pinList == states) return;

	m_rxJ6pinList = states;

	emit rxJ6PinsChanged(m_rxJ6pinList);
}

void Settings::setTxJ6Pin(QObject *sender, HamBand band, int value) {

	Q_UNUSED (sender)

	if (m_txJ6pinList.length() != MAX_BANDS-1) return;
	if (m_txJ6pinList[band] == value) return;

	m_txJ6pinList[band] = value;

	emit txJ6PinsChanged(m_txJ6pinList);
}

void Settings::setTxJ6Pins(QObject * sender, const QList<int> &states) {

	Q_UNUSED (sender)

	//if (m_txJ6pinList == states) return;

	m_txJ6pinList = states;

	emit txJ6PinsChanged(m_txJ6pinList);
}

void Settings::setPennyOCEnabled(QObject *sender, bool value) {

	Q_UNUSED (sender)

	if (m_pennyOCEnabled == value) return;

	m_pennyOCEnabled = value;

	emit pennyOCEnabledChanged(m_pennyOCEnabled);
}

//**************************************
//**************************************
// OpenCL stuff

void Settings::setOpenCLDevices(QList<QCLDevice> devices) {

	m_clDevices = devices;
}

//void Settings::setCudaPresence(bool value) {
//
//	m_cudaPresence = value;
//}
//
//void Settings::setCudaDevices(int value) {
//
//	QMutexLocker locker(&mutex);
//		if (m_cuda_devices == value) return;
//		m_cuda_devices = value;
//	locker.unlock();
//
//	emit cudaDevicesChanged(this, value);
//}
//
//void Settings::setCudaDriver(QObject *sender, int value) {
//
//	QMutexLocker locker(&mutex);
//		if (m_cuda_driver_version == value) return;
//		m_cuda_driver_version = value;
//	locker.unlock();
//
//	emit cudaDriverChanged(sender, value);
//}
//
//void Settings::setCudaRuntime(QObject *sender, int value) {
//
//	QMutexLocker locker(&mutex);
//		if (m_cuda_runtime_version == value) return;
//		m_cuda_runtime_version = value;
//	locker.unlock();
//
//	emit cudaRuntimeChanged(sender, value);
//}
//
//void Settings::setCurrentCudaDevice(QObject *sender, int value) {
//
//	QMutexLocker locker(&mutex);
//		if (m_current_cuda_device == value) return;
//		m_current_cuda_device = value;
//	locker.unlock();
//
//	emit cudaCurrentDeviceChanged(sender, value);
//}
//
//void Settings::setCudaLastDevice(QObject *sender, int value) {
//
//	QMutexLocker locker(&mutex);
//		if (m_cudaLastDevice == value) return;
//		m_cudaLastDevice = value;
//	locker.unlock();
//
//	emit cudaLastDeviceChanged(sender, value);
//}

void Settings::setFreqRulerPosition(float position, int rx) {

	if (position < 0) position = 0;
	if (position > 1) position = 1;

	emit freqRulerPositionChanged(position, rx);
}

void Settings::setWideBandRulerPosition(float position) {

	if (position < 0) position = 0;
	if (position > 1) position = 1;

	emit wideBandRulerPositionChanged(position);
}

//**********************************************************************************
// audio settings
 
void Settings::setAudioFormat(QObject *sender, const QAudioFormat &format) {

	Q_UNUSED (sender)

	QMutexLocker locker(&settingsMutex);

	//if (m_format == format) return;
	m_format = format;

	emit audioFormatChanged(sender, m_format);
}

void Settings::setAudioPosition(QObject *sender, qint64 position) {

	emit audioPositionChanged(sender, position);
}

void Settings::setAudioBuffer(QObject *sender, qint64 position, qint64 length, const QByteArray &buffer) {

	emit audioBufferChanged(sender, position, length, buffer);
}

//**********************************************************************************
// chirp signal settings
 
void Settings::switchToChirpSignalMode(QObject *sender) {

	emit chirpSignalModeChanged(sender);
}

void Settings::setLowerChirpFreq(int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_lowerChirpFreq == value) return;
	if (m_lowerChirpFreq >= m_upperChirpFreq)
		m_lowerChirpFreq = m_upperChirpFreq;
	else
		m_lowerChirpFreq = value;

	emit lowerChirpFreqChanged(this, m_lowerChirpFreq);
}

void Settings::setUpperChirpFreq(int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_upperChirpFreq == value) return;
	if (m_lowerChirpFreq >= m_upperChirpFreq)
		m_upperChirpFreq = m_lowerChirpFreq;
	else
		m_upperChirpFreq = value;

	emit upperChirpFreqChanged(this, m_upperChirpFreq);
}

void Settings::setChirpAmplitude(qreal value) {

	QMutexLocker locker(&settingsMutex);

	if (m_chirpAmplitude == value) return;
	m_chirpAmplitude = value;

	emit chirpAmplitudeChanged(this, m_chirpAmplitude);
}

void Settings::setChirpSamplingFreq(int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_chirpSamplingFreq == value) return;
	m_chirpSamplingFreq = value;

	emit chirpSamplingFreqChanged(this, m_chirpSamplingFreq);
}

void Settings::setChirpBufferDurationUs(int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_chirpBufferDurationUs == (qint64)value) return;
	m_chirpBufferDurationUs = (qint64)(1000 * value);

	emit chirpBufferDurationUsChanged(this, m_chirpBufferDurationUs);
}

void Settings::setChirpRepetitionTimes(int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_chirpRepetitionTimes == value) return;
	m_chirpRepetitionTimes = value;

	emit chirpRepetitionTimesChanged(this, m_chirpRepetitionTimes);
}

void Settings::setChirpReceiver(bool value) {

	QMutexLocker locker(&settingsMutex);

	if (m_chirpReceiverOn == value) return;
	m_chirpReceiverOn = value;

	emit chirpReceiverChanged(this, m_chirpReceiverOn);
}

void Settings::setChirpAvgLength(int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_chirpAvgLength == value) return;
	m_chirpAvgLength = value;

	emit chirpAvgLengthChanged(m_chirpAvgLength);
}

void Settings::setChirpFFTShow(bool value) {

	QMutexLocker locker(&settingsMutex);

	if (m_showChirpFFT == value) return;
	m_showChirpFFT = value;

	emit chirpFFTShowChanged(m_showChirpFFT);
}

void Settings::setChirpUSB(bool value) {

	QMutexLocker locker(&settingsMutex);

	if (m_chirpUSB == value) return;
	m_chirpUSB = value;

	emit chirpSidebandChanged(m_chirpUSB);
}

//void Settings::setChirpDownSampleRate(int value) {
//
//	if (m_chirpDownSampleRate == value) return;
//	m_chirpDownSampleRate = value;
//}

void Settings::setChirpBuffer(qint64 length, const QList<qreal> &buffer) {

	emit chirpBufferChanged(length, buffer);
}

void Settings::setChirpSpectrumBuffer(int sampleRate, qint64 length, const float *buffer) {

	emit chirpSpectrumBufferChanged(sampleRate, length, buffer);
}

void Settings::setChirpSpectrum(qint64 position, qint64 length, const FrequencySpectrum &spectrum) {
	
	emit chirpSpectrumChanged(position, length, spectrum);
}

void Settings::setChirpSpectrumList(const QList<FrequencySpectrum> &spectrumList) {

	emit chirpSpectrumListChanged(spectrumList);
}

void Settings::setChirpFilterLowerFrequency(int value) {

	if (m_chirpFilterLowerFrequency == value) return;
	m_chirpFilterLowerFrequency = value;

	emit chirpFilterLowerFrequencyChanged(m_chirpFilterLowerFrequency);
}

void Settings::setChirpFilterUpperFrequency(int value) {

	if (m_chirpFilterUpperFrequency == value) return;
	m_chirpFilterUpperFrequency = value;

	emit chirpFilterUpperFrequencyChanged(m_chirpFilterUpperFrequency);
}

//********************************

void Settings::setSpectrumSize(QObject *sender, int value) {

	if (m_spectrumSize == value) return;

	m_spectrumSize = value;
	emit spectrumSizeChanged(sender, m_spectrumSize);
}

void Settings::moveDisplayWidget(int value) {

	emit displayWidgetHeightChanged(value);
}


//*********************************
// color stuff

void Settings::setPanadapterColors(TPanadapterColors type) {

	if (type.panBackgroundColor != m_panadapterColors.panBackgroundColor)
		m_panadapterColors.panBackgroundColor = type.panBackgroundColor;

	if (type.waterfallColor != m_panadapterColors.waterfallColor)
		m_panadapterColors.waterfallColor = type.waterfallColor;
	
	if (type.panLineColor != m_panadapterColors.panLineColor)
		m_panadapterColors.panLineColor = type.panLineColor;
	
	if (type.panLineFilledColor != m_panadapterColors.panLineFilledColor)
		m_panadapterColors.panLineFilledColor = type.panLineFilledColor;
	
	if (type.panSolidTopColor != m_panadapterColors.panSolidTopColor)
		m_panadapterColors.panSolidTopColor = type.panSolidTopColor;
	
	if (type.panSolidBottomColor != m_panadapterColors.panSolidBottomColor)
		m_panadapterColors.panSolidBottomColor = type.panSolidBottomColor;
	
	if (type.wideBandLineColor != m_panadapterColors.wideBandLineColor)
		m_panadapterColors.wideBandLineColor = type.wideBandLineColor;
	
	if (type.wideBandFilledColor != m_panadapterColors.wideBandFilledColor)
		m_panadapterColors.wideBandFilledColor = type.wideBandFilledColor;
	
	if (type.wideBandSolidBottomColor != m_panadapterColors.wideBandSolidTopColor)
		m_panadapterColors.wideBandSolidTopColor = type.wideBandSolidTopColor;
	
	if (type.wideBandSolidBottomColor != m_panadapterColors.wideBandSolidBottomColor)
		m_panadapterColors.wideBandSolidBottomColor = type.wideBandSolidBottomColor;

	if (type.distanceLineColor != m_panadapterColors.distanceLineColor)
		m_panadapterColors.distanceLineColor = type.distanceLineColor;

	if (type.distanceLineFilledColor != m_panadapterColors.distanceLineFilledColor)
		m_panadapterColors.distanceLineFilledColor = type.distanceLineFilledColor;

	if (type.panCenterLineColor != m_panadapterColors.panCenterLineColor)
		m_panadapterColors.panCenterLineColor = type.panCenterLineColor;

	if (type.gridLineColor != m_panadapterColors.gridLineColor)
		m_panadapterColors.gridLineColor = type.gridLineColor;

	emit panadapterColorChanged();
}

void Settings::setFramesPerSecond(QObject *sender, int rx, int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_receiverDataList.at(rx).framesPerSecond != value)
		m_receiverDataList[rx].framesPerSecond = value;

	emit framesPerSecondChanged(this, rx, m_receiverDataList[rx].framesPerSecond);
}

int	Settings::getFramesPerSecond(int rx) {

	return m_receiverDataList.at(rx).framesPerSecond;
}

void Settings::setGraphicResolution(int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_graphicResolution == value) return;
	m_graphicResolution = value;

	emit graphicResolutionChanged(this, m_graphicResolution);
}

int Settings::getGraphicResolution() {

	return m_graphicResolution;
}

void Settings::setSpectrumAveraging(bool value) {

	QMutexLocker locker(&settingsMutex);

	if (m_specAveraging == value) return;
	m_specAveraging = value;

	emit spectrumAveragingChanged(m_specAveraging);
}

void Settings::setSpectrumAveragingCnt(int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_specAveragingCnt == value) return;
	m_specAveragingCnt = value;

	emit spectrumAveragingCntChanged(m_specAveragingCnt);
}

void Settings::setPanGrid(bool value) {

	QMutexLocker locker(&settingsMutex);

	if (m_panGrid == value) return;
	m_panGrid = value;

	emit panGridStatusChanged(m_panGrid);
}

void Settings::setPeakHold(bool value) {

	QMutexLocker locker(&settingsMutex);

	if (m_peakHold == value) return;
	m_peakHold = value;

	emit peakHoldStatusChanged(m_peakHold);
}

void Settings::setWaterfallTime(int rx, int value) {

	QMutexLocker locker(&settingsMutex);

	//if (m_receiverDataList[rx].waterfallTime == value) return;
	//m_receiverDataList[rx].waterfallTime = value;

	//emit waterfallTimeChanged(rx, value);
}

void Settings::setWaterfallOffesetLo(int rx, int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_receiverDataList[rx].waterfallOffsetLo == value) return;
	m_receiverDataList[rx].waterfallOffsetLo = value;

	emit waterfallOffesetLoChanged(rx, value);
}

void Settings::setWaterfallOffesetHi(int rx, int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_receiverDataList[rx].waterfallOffsetHi == value) return;
	m_receiverDataList[rx].waterfallOffsetHi = value;

	emit waterfallOffesetHiChanged(rx, value);
}

void Settings::setSMeterHoldTime(int value) {

	QMutexLocker locker(&settingsMutex);

	if (m_sMeterHoldTime == value) return;
	m_sMeterHoldTime = value;

	emit sMeterHoldTimeChanged(m_sMeterHoldTime);
}

void Settings::setdBmPanScaleMin(int rx, qreal value) {

	QMutexLocker locker(&settingsMutex);

	HamBand band = m_receiverDataList.at(m_currentReceiver).hamBand;
	m_receiverDataList[rx].dBmPanScaleMinList[band] = value;

	emit dBmScaleMinChanged(rx, value);
}

void Settings::setdBmPanScaleMax(int rx, qreal value) {

	QMutexLocker locker(&settingsMutex);

	HamBand band = m_receiverDataList.at(m_currentReceiver).hamBand;
	m_receiverDataList[rx].dBmPanScaleMaxList[band] = value;

	emit dBmScaleMaxChanged(rx, value);
}

void Settings::setdBmWBScaleMin(qreal value) {

	QMutexLocker locker(&settingsMutex);

	if (m_dBmWBScaleMin == value) return;
	m_dBmWBScaleMin = value;

	locker.unlock();
	emit dBmScaleWBMinChanged(value);
}

void Settings::setdBmWBScaleMax(qreal value) {

	QMutexLocker locker(&settingsMutex);

	if (m_dBmWBScaleMax == value) return;
	m_dBmWBScaleMax = value;

	locker.unlock();
	emit dBmScaleWBMaxChanged(value);
}

void Settings::setdBmDistScaleMin(qreal value) {

	Q_UNUSED(value)
}

void Settings::setdBmDistScaleMax(qreal value) {

	Q_UNUSED(value)
}

// **********************************************************************

void Settings::showRadioPopupWidget() {

	if (m_radioPopupVisible)
		m_radioPopupVisible = false;
	else
		m_radioPopupVisible = true;

	emit showRadioPopupChanged(m_radioPopupVisible);
}
