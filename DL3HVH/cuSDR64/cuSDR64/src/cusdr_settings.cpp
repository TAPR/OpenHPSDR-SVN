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

//#define LOG_SETTINGS
#define LOG_ALL

// **************************************
// logging
#ifdef LOG_ALL
#define LOG_SETTINGS
#define LOG_MAIN
#define LOG_SERVER
#define LOG_DATA_ENGINE
#define LOG_DATA_RECEIVER
#define LOG_DATA_PROCESSOR
#define LOG_NETWORKDIALOG
#define LOG_HPSDRIO
#define LOG_AUDIO_ENGINE
#define LOG_AUDIO_WAVFILE
#define LOG_AUDIO_RECEIVER
#define LOG_AUDIO_PROCESSOR
#define LOG_CHIRP_PROCESSOR
#define LOG_DTTSP_API
#define LOG_SPECTRUMANALYSER
#define LOG_WAVEFORM
#define LOG_PAINT_EVENT
#define LOG_GRAPHICS
#define LOG_WBGRAPHICS
#define LOG_DISPLAYPANEL
#define LOG_CUDAGRAPH
#define LOG_SERVER_WIDGET
#define LOG_CHIRP_WIDGET
#define LOG_RECEIVER
#define LOG_FUTUREWATCHER
#define LOG_ALEX_WIDGET
#endif

#include "cusdr_settings.h"
#include "cusdr_styles.h"

#include <QMessageBox>


Settings *Settings::m_instance = NULL;		/*!< set m_instance to NULL. */

/*!
	\class Settings
	\brief Settings class implements application specific and user defined variables for the application.
*/

Settings::Settings(QObject *parent)
	:QObject(parent)
	, m_mainPower(false)
	, m_settingsLoaded(false)
	, m_dataEngineState(QSDR::DataEngineDown)
	, m_mercuryVersion(0)
	, m_mercuryReceivers(1)
	, m_currentReceiver(0)
	, m_hpsdrNetworkDevices(0)
{
	// temporarily
	m_pennyLanePresence = false;

	//qRegisterMetaType<THPSDRParameter>();
	qRegisterMetaType<TNetworkDevicecard>();
	qRegisterMetaType<QList<TNetworkDevicecard>>();

	startTime = QDateTime::currentDateTime();

	qDebug() << "************************************************************************";
	SETTINGS_DEBUG << "start at: " << qPrintable(startTime.toString());

	settingsFilename = "settings.ini";
	settings = new QSettings(QCoreApplication::applicationDirPath() +  "/" + settingsFilename, QSettings::IniFormat);

	m_titleString = "cuSDR";
	m_versionString = "v0.2.1";

	// get styles
	//m_sdrStyle = sdrStyle;
	/*m_dialogStyle = dialogStyle;
	m_colorDialogStyle = colorDialogStyle;
	m_widgetStyle = widgetStyle;
	m_itemStyle = itemStyle;
	m_labelStyle = labelStyle;
	m_tableStyle = tableStyle;
	m_messageBoxStyle = messageBoxStyle;
	m_comboBoxStyle = comboBoxStyle;
	m_spinBoxStyle = spinBoxStyle;
	m_menuStyle = menuStyle;
	m_miniButtonStyle = miniButtonStyle;
	m_sliderLabelStyle = sliderLabelStyle;
	m_volSliderStyle = volSliderStyle;
	m_splitterStyle = splitterStyle;
	m_frameStyle = frameStyle;
	m_tabWidgetStyle = tabWidgetStyle;*/
	
	for (int i = 0; i < MAX_RECEIVERS; i++) {

		TReceiver receiverData;
		m_receiverDataList.append(receiverData);
	}

	m_rxStringList << "receiver0" << "receiver1" << "receiver2" << "receiver3";
	m_bandList = getHamBandFrequencies();
	m_bandTextList = getHamBandText();
	m_defaultFilterList = getDefaultFilterFrequencies();

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
}

Settings::~Settings() {

	//m_clDevices.clear();
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
 
QString	Settings::getServerModeString(QSDR::_ServerMode mode) {

	QString str;
	switch (mode) {

		case 0:
			str = "no server mode";
			break;

		case 1:
			str = "DttSP";
			break;

		case 2:
			str = "QtDSP";
			break;

		case 3:
			str = "ChirpWSPR";
			break;

		case 4:
			str = "ChirpWSPRFile";
			break;

		case 5:
			str = "external DSP";
			break;

		case 6:
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

QString	Settings::getErrorString(QSDR::_Error err) {

	QString str;
	switch (err) {

		case 0:
			str = "no error";
			break;

		case 1:
			str = "not implemented";
			break;

		case 2:
			str = "hardware IO error";
			break;

		case 3:
			str = "server mode error";
			break;

		case 4:
			str = "open device error";
			break;

		case 5:
			str = "dataProcessThread error";
			break;

		case 6:
			str = "widebandDataProcessThread error";
			break;

		case 7:
			str = "audioThread error";
			break;

		case 8:
			str = "underrun error";
			break;

		case 9:
			str = "fatal error";
			break;
	}
	return str;
}

QString Settings::getSDRStyle()				{ return sdrStyle; }
QString Settings::getWidgetStyle()			{ return widgetStyle; }
QString Settings::getMessageBoxStyle()		{ return messageBoxStyle; }
QString Settings::getDialogStyle()			{ return dialogStyle; }
QString Settings::getColorDialogStyle()		{ return colorDialogStyle; }
QString Settings::getItemStyle()			{ return itemStyle; }
QString Settings::getLabelStyle()			{ return labelStyle; }
QString Settings::getSliderLabelStyle()		{ return sliderLabelStyle; }
QString Settings::getTableStyle()			{ return tableStyle; }
QString Settings::getComboBoxStyle()		{ return comboBoxStyle; }
QString Settings::getSpinBoxStyle()			{ return spinBoxStyle; }
QString Settings::getMenuStyle()			{ return menuStyle; }
QString Settings::getMiniButtonStyle()		{ return miniButtonStyle; }
QString Settings::getVolSliderStyle()		{ return volSliderStyle; }
QString Settings::getSplitterStyle()		{ return splitterStyle; }
QString Settings::getFrameStyle()			{ return frameStyle; }
QString Settings::getTabWidgetStyle()		{ return tabWidgetStyle; }
//QString Settings::getNewSliderStyle()		{ return m_newSliderStyle; }

//********************************************
// load/save settings

int Settings::loadSettings() {

	QString str;
	int value;

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
	

	// HPSDR hardware
	value = settings->value("hpsdr/hardware", 0).toInt();
	if (value < 0 || value > 2) value = 0;
	m_hpsdrHardware = value;

	str = settings->value("hpsdr/mercury", "true").toString();
	if (str == "true")
		m_mercuryPresence = true;
	else
		m_mercuryPresence = false;

	str = settings->value("hpsdr/penelope", "false").toString();
	if (str == "true")
		m_penelopePresence = true;
	else
		m_penelopePresence = false;

	str = settings->value("hpsdr/excalibur", "false").toString();
	if (str == "true")
		m_excaliburPresence = true;
	else
		m_excaliburPresence = false;

	str = settings->value("hpsdr/alex", "false").toString();
	if (str == "true")
		m_alexPresence = true;
	else
		m_alexPresence = false;

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

	
	/*value = settings->value("server/receivers", 1).toInt();
	if (value < 1 || value > 4) value = 1;
	m_mercuryReceivers = value;*/

	value = settings->value("server/sample_rate", 48000).toInt();
	if ((value != 48000) & (value != 96000) & (value != 192000)) value = 48000;
	setSampleRate(this, value);
	
	str = settings->value("server/preamp", "off").toString();
	if (str.toLower() == "on")
		m_mercuryPreamp = 1;
	else
		m_mercuryPreamp = 0;

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

	str = settings->value("server/wideband", "off").toString();
	if (str.toLower() == "on")
		m_wideBandStatus = true;
	else
		m_wideBandStatus = false;

	str = settings->value("server/widebandData", "on").toString();
	if (str.toLower() == "on")
		m_wideBandData = true;
	else
		m_wideBandData = false;

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

	//str = settings->value("server/mode", "internal").toString();
	str = settings->value("server/mode", "dttsp").toString();
	if (str == "dttsp") {

		m_serverMode = QSDR::DttSP;
		setSpectrumSize(4096);
	}
	else if (str == "qtdsp") {

		m_serverMode = QSDR::QtDSP;
		setSpectrumSize(2048);
	}
	if (str == "externalDSP") {

		m_serverMode = QSDR::ExternalDSP;
	}
	else if (str == "chirpWSPR") {

		m_serverMode = QSDR::ChirpWSPR;
		setSpectrumSize(4096);
	}
	else if (str == "chirpWSPRFile") {

		m_serverMode = QSDR::ChirpWSPRFile;
		setSpectrumSize(4096);
	}
	/*else
		m_serverMode = QSDR::InternalDSP;*/

	value = settings->value("server/mouseWheelFreqStep", 1000).toInt();
	if ((value != 1) & (value != 10) & (value != 100) & (value != 1000) & (value != 10000) & (value != 100000) &
		(value != 5) & (value != 50) & (value != 500) & (value != 5000) & (value != 50000) & (value != 500000))
		value = 100;
	m_mouseWheelFreqStep = (double)value;

	//******************************************************************
	// receiver data settings

	for (int i = 0; i < MAX_RECEIVERS; i++) {

		QString cstr = m_rxStringList.at(i);
		cstr.append("/audioVolume");

		value = settings->value(cstr, 10).toInt();
		if (value < 0) value = 0;
		if (value > 100) value = 100;
		m_receiverDataList[i].audioVolume = value/100.0f;

		cstr = m_rxStringList.at(i);
		cstr.append("/agcGain");

		value = settings->value(cstr, 75).toInt();
		if (value < -20) value = -20;
		if (value > 120) value = 120;
		m_receiverDataList[i].acgGain = value;

		cstr = m_rxStringList.at(i);
		cstr.append("/dspmode");

		str = settings->value(cstr, "LSB").toString();
		if (str == "USB")
			m_receiverDataList[i].dspMode = USB;
		else if (str == "DSB")
			m_receiverDataList[i].dspMode = DSB;
		else if (str == "CWL")
			m_receiverDataList[i].dspMode = CWL;
		else if (str == "CWU")
			m_receiverDataList[i].dspMode = CWU;
		else if (str == "FMN")
			m_receiverDataList[i].dspMode = FMN;
		else if (str == "AM")
			m_receiverDataList[i].dspMode = AM;
		else if (str == "DIGU")
			m_receiverDataList[i].dspMode = DIGU;
		else if (str == "SPEC")
			m_receiverDataList[i].dspMode = SPEC;
		else if (str == "DIGL")
			m_receiverDataList[i].dspMode = DIGL;
		else if (str == "SAM")
			m_receiverDataList[i].dspMode = SAM;
		else if (str == "DRM")
			m_receiverDataList[i].dspMode = DRM;
		else
			m_receiverDataList[i].dspMode = LSB;

		cstr = m_rxStringList.at(i);
		cstr.append("/agcmode");

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

		cstr = m_rxStringList.at(i);
		cstr.append("/dBmPanScaleMin");
		value = settings->value(cstr, -140).toInt();
		if ((value < -200) || (value > 0)) value = -140;
		m_receiverDataList[i].dBmPanScaleMin = (qreal)(1.0 * value);

		cstr = m_rxStringList.at(i);
		cstr.append("/dBmPanScaleMax");
		value = settings->value(cstr, 0).toInt();
		if ((value < -100) || (value > 0)) value = 0;
		m_receiverDataList[i].dBmPanScaleMax = (qreal)(1.0 * value);

		cstr = m_rxStringList.at(i);
		cstr.append("/waterfallTime");
		value = settings->value(cstr, 50).toInt();
		if ((value < 20) || (value > 250)) value = 50;
		m_receiverDataList[i].waterfallTime = value;
		
		cstr = m_rxStringList.at(i);
		cstr.append("/waterfallOffsetLo");
		value = settings->value(cstr, 0).toInt();
		if ((value < -50) || (value > 50)) value = 0;
		m_receiverDataList[i].waterfallOffsetLo = value;

		cstr = m_rxStringList.at(i);
		cstr.append("/waterfallOffsetHi");
		value = settings->value(cstr, 0).toInt();
		if ((value < -50) || (value > 50)) value = 0;
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
		if ((value < 1800000) & (value > 50000000)) value = 1800000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency80m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) & (value > 50000000)) value = 3779000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency60m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) & (value > 50000000)) value = 5260000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency40m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) & (value > 50000000)) value = 7000000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency30m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) & (value > 50000000)) value = 10100000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency20m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) & (value > 50000000)) value = 14000000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency17m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) & (value > 50000000)) value = 18068000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency15m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) & (value > 50000000)) value = 21000000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency12m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) & (value > 50000000)) value = 24890000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency10m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) & (value > 50000000)) value = 28000000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequency6m");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) & (value > 50000000)) value = 50000000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/lastFrequencyGen");
		value = settings->value(cstr, 1800000).toDouble();
		if ((value < 1800000) & (value > 50000000)) value = 1000;
		m_receiverDataList[i].lastFrequencyList << value;

		cstr = m_rxStringList.at(i);
		cstr.append("/frequency");
		value = settings->value(cstr, 3672000).toDouble();
		if ((value < 0) & (value > 50000000)) value = 3600000;
		m_receiverDataList[i].frequency = value;

		setFrequency(i, value);
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
	
	str = settings->value("graphics/averaging", "off").toString();
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

	value = settings->value("graphics/framesPerSecond", 24).toInt();
	if (value < 0 || value > 192) value = 24;
	m_framesPerSecond = value;

	value = settings->value("graphics/resolution", 2).toInt();
	if ((value != 1) & (value != 2) & (value != 4)) value = 2;
	m_graphicResolution = value;

	value = settings->value("graphics/dBmWideBandScaleMin", -140).toInt();
	if ((value < -200) || (value > 0)) value = -140;
	m_dBmWBScaleMin = (qreal)(1.0 * value);

	value = settings->value("graphics/dBmWideBandScaleMax", 0).toInt();
	if ((value < -100) || (value > 0)) value = 0;
	m_dBmWBScaleMax = (qreal)(1.0 * value);

	value = settings->value("graphics/dBmDistScaleMin", -20).toInt();
	if ((value < -200) || (value > 0)) value = -20;
	m_dBmDistScaleMin = (qreal)(1.0 * value);

	value = settings->value("graphics/dBmDistScaleMax", 100).toInt();
	if ((value < -100) || (value > 200)) value = 100;
	m_dBmDistScaleMax = (qreal)(1.0 * value);

	str = settings->value("graphics/waterfall", "simple").toString();
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

	color = settings->value("colors/panBackground", QColor(30, 30, 30)).value<QColor>();
	if (!color.isValid()) color = QColor(30, 30, 30);
	m_panadapterColors.panBackgroundColor = color;

	color = settings->value("colors/waterfall", QColor(100, 110, 130)).value<QColor>();
	if (!color.isValid()) color = QColor(100, 110, 130);
	m_panadapterColors.waterfallColor = color;

	color = settings->value("colors/panLine", QColor(100, 110, 130)).value<QColor>();
	if (!color.isValid()) color = QColor(100, 110, 130);
	m_panadapterColors.panLineColor = color;

	color = settings->value("colors/panLineFilled", QColor(100, 110, 130)).value<QColor>();
	if (!color.isValid()) color = QColor(100, 110, 130);
	m_panadapterColors.panLineFilledColor = color;

	color = settings->value("colors/panSolidTop", QColor(100, 110, 130)).value<QColor>();
	if (!color.isValid()) color = QColor(100, 110, 130);
	m_panadapterColors.panSolidTopColor = color;

	color = settings->value("colors/panSolidBottom", QColor(100, 110, 130)).value<QColor>();
	if (!color.isValid()) color = QColor(100, 110, 130);
	m_panadapterColors.panSolidBottomColor = color;

	color = settings->value("colors/panWideBandLine", QColor(100, 110, 130)).value<QColor>();
	if (!color.isValid()) color = QColor(100, 110, 130);
	m_panadapterColors.wideBandLineColor = color;

	color = settings->value("colors/panWideBandFilled", QColor(100, 110, 130)).value<QColor>();
	if (!color.isValid()) color = QColor(100, 110, 130);
	m_panadapterColors.wideBandFilledColor = color;

	color = settings->value("colors/panWideBandSolidTop", QColor(100, 110, 130)).value<QColor>();
	if (!color.isValid()) color = QColor(100, 110, 130);
	m_panadapterColors.wideBandSolidTopColor = color;

	color = settings->value("colors/panWideBandSolidBottom", QColor(100, 110, 130)).value<QColor>();
	if (!color.isValid()) color = QColor(100, 110, 130);
	m_panadapterColors.wideBandSolidBottomColor = color;

	color = settings->value("colors/distanceLine", QColor(100, 110, 130)).value<QColor>();
	if (!color.isValid()) color = QColor(100, 110, 130);
	m_panadapterColors.distanceLineColor = color;

	color = settings->value("colors/distanceLineFilled", QColor(100, 110, 130)).value<QColor>();
	if (!color.isValid()) color = QColor(100, 110, 130);
	m_panadapterColors.distanceLineFilledColor = color;

	color = settings->value("colors/panCenterLine", QColor(100, 110, 130)).value<QColor>();
	if (!color.isValid()) color = QColor(100, 110, 130);
	m_panadapterColors.panCenterLineColor = color;
	
	SETTINGS_DEBUG << "reading done.";

	return 0;
}

int Settings::saveSettings() {

	settings->setValue(titleStr(), versionStr());
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

	
	// HPSDR hardware
	settings->setValue("hpsdr/hardware", m_hpsdrHardware);

	switch (m_hpsdrHardware) {

		// Mercury/Penelope
		case 0:

			if (m_mercuryPresence)
				settings->setValue("hpsdr/mercury", "true");
			else
				settings->setValue("hpsdr/mercury", "false");

			if (m_penelopePresence)
				settings->setValue("hpsdr/penelope", "true");
			else
				settings->setValue("hpsdr/penelope", "false");

			if (m_excaliburPresence)
				settings->setValue("hpsdr/excalibur", "true");
			else
				settings->setValue("hpsdr/excalibur", "false");
			break;

		// Hermes
		case 1:

			if (m_mercuryPresence)
				settings->setValue("hpsdr/mercury", "true");
			else
				settings->setValue("hpsdr/mercury", "false");

			if (m_penelopePresence)
				settings->setValue("hpsdr/penelope", "true");
			else
				settings->setValue("hpsdr/penelope", "false");

			if (m_excaliburPresence)
				settings->setValue("hpsdr/excalibur", "true");
			else
				settings->setValue("hpsdr/excalibur", "false");
			break;

		// Cyclops
		case 2:
			break;
	}
	
	if (m_alexPresence)
		settings->setValue("hpsdr/alex", "true");
	else
		settings->setValue("hpsdr/alex", "false");

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
	
	if (m_mercuryPreamp == 1)
		settings->setValue("server/preamp", "on");
	else
		settings->setValue("server/preamp", "off");

	if (m_mercuryDither == 1)
		settings->setValue("server/dither", "on");
	else
		settings->setValue("server/dither", "off");

	if (m_mercuryRandom == 1)
		settings->setValue("server/random", "on");
	else
		settings->setValue("server/random", "off");

	if (m_wideBandStatus)
		settings->setValue("server/wideband", "on");
	else
		settings->setValue("server/wideband", "off");

	if (m_wideBandData)
		settings->setValue("server/widebandData", "on");
	else
		settings->setValue("server/widebandData", "off");

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

	if (m_serverMode == QSDR::QtDSP)
		settings->setValue("server/mode", "qtdsp");
	else
		settings->setValue("server/mode", "dttsp");

	settings->setValue("server/mouseWheelFreqStep", m_mouseWheelFreqStep);

	//******************************************************************
	// receiver data settings

	for (int i = 0; i < MAX_RECEIVERS; i++) {
	
		QString str = m_rxStringList.at(i);
		str.append("/audioVolume");
		settings->setValue(str, (int)(m_receiverDataList[i].audioVolume * 100));

		str = m_rxStringList.at(i);
		str.append("/agcGain");
		settings->setValue(str, m_receiverDataList[i].acgGain);
		
		str = m_rxStringList.at(i);
		str.append("/dspmode");

		if (m_receiverDataList[i].dspMode == LSB)
			settings->setValue(str, "LSB");
		else if (m_receiverDataList[i].dspMode == USB)
			settings->setValue(str, "USB");
		else if (m_receiverDataList[i].dspMode == DSB)
			settings->setValue(str, "DSB");
		else if (m_receiverDataList[i].dspMode == CWL)
			settings->setValue(str, "CWL");
		else if (m_receiverDataList[i].dspMode == CWU)
			settings->setValue(str, "CWU");
		else if (m_receiverDataList[i].dspMode == FMN)
			settings->setValue(str, "FMN");
		else if (m_receiverDataList[i].dspMode == AM)
			settings->setValue(str, "AM");
		else if (m_receiverDataList[i].dspMode == DIGU)
			settings->setValue(str, "DIGU");
		else if (m_receiverDataList[i].dspMode == SPEC)
			settings->setValue(str, "SPEC");
		else if (m_receiverDataList[i].dspMode == DIGL)
			settings->setValue(str, "DIGL");
		else if (m_receiverDataList[i].dspMode == SAM)
			settings->setValue(str, "SAM");
		else if (m_receiverDataList[i].dspMode == DRM)
			settings->setValue(str, "DRM");

		str = m_rxStringList.at(i);
		str.append("/agcmode");

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

		str = m_rxStringList.at(i);
		str.append("/dBmPanScaleMin");
		settings->setValue(str, (int)m_receiverDataList[i].dBmPanScaleMin);

		str = m_rxStringList.at(i);
		str.append("/dBmPanScaleMax");
		settings->setValue(str, (int)m_receiverDataList[i].dBmPanScaleMax);

		str = m_rxStringList.at(i);
		str.append("/waterfallTime");
		settings->setValue(str, m_receiverDataList[i].waterfallTime);

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

		str = m_rxStringList.at(i);
		str.append("/lastFrequency160m");
		settings->setValue(str, m_receiverDataList.at(i).lastFrequencyList.at(0));

		str = m_rxStringList.at(i);
		str.append("/lastFrequency80m");
		settings->setValue(str, m_receiverDataList.at(i).lastFrequencyList.at(1));

		str = m_rxStringList.at(i);
		str.append("/lastFrequency60m");
		settings->setValue(str, m_receiverDataList.at(i).lastFrequencyList.at(2));

		str = m_rxStringList.at(i);
		str.append("/lastFrequency40m");
		settings->setValue(str, m_receiverDataList.at(i).lastFrequencyList.at(3));

		str = m_rxStringList.at(i);
		str.append("/lastFrequency30m");
		settings->setValue(str, m_receiverDataList.at(i).lastFrequencyList.at(4));

		str = m_rxStringList.at(i);
		str.append("/lastFrequency20m");
		settings->setValue(str, m_receiverDataList.at(i).lastFrequencyList.at(5));

		str = m_rxStringList.at(i);
		str.append("/lastFrequency17m");
		settings->setValue(str, m_receiverDataList.at(i).lastFrequencyList.at(6));

		str = m_rxStringList.at(i);
		str.append("/lastFrequency15m");
		settings->setValue(str, m_receiverDataList.at(i).lastFrequencyList.at(7));

		str = m_rxStringList.at(i);
		str.append("/lastFrequency12m");
		settings->setValue(str, m_receiverDataList.at(i).lastFrequencyList.at(8));

		str = m_rxStringList.at(i);
		str.append("/lastFrequency10m");
		settings->setValue(str, m_receiverDataList.at(i).lastFrequencyList.at(9));

		str = m_rxStringList.at(i);
		str.append("/lastFrequency6m");
		settings->setValue(str, m_receiverDataList.at(i).lastFrequencyList.at(10));

		str = m_rxStringList.at(i);
		str.append("/lastFrequencyGen");
		settings->setValue(str, m_receiverDataList.at(i).lastFrequencyList.at(11));

		str = m_rxStringList.at(i);
		str.append("/frequency");
		settings->setValue(str, m_receiverDataList[i].frequency);
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
	
	settings->setValue("graphics/framesPerSecond", m_framesPerSecond);
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
	settings->setValue("colors/panBackground", m_panadapterColors.panBackgroundColor);
	settings->setValue("colors/waterfall", m_panadapterColors.waterfallColor);
	settings->setValue("colors/panLine", m_panadapterColors.panLineColor);
	settings->setValue("colors/panLineFilled", m_panadapterColors.panLineFilledColor);
	settings->setValue("colors/panSolidTop", m_panadapterColors.panSolidTopColor);
	settings->setValue("colors/panSolidBottom", m_panadapterColors.panSolidBottomColor);
	settings->setValue("colors/panWideBandLine", m_panadapterColors.wideBandLineColor);
	settings->setValue("colors/panWideBandFilled", m_panadapterColors.wideBandFilledColor);
	settings->setValue("colors/panWideBandSolidTop", m_panadapterColors.wideBandSolidTopColor);
	settings->setValue("colors/panWideBandSolidBottom", m_panadapterColors.wideBandSolidBottomColor);
	settings->setValue("colors/distanceLine", m_panadapterColors.distanceLineColor);
	settings->setValue("colors/distanceLineFilled", m_panadapterColors.distanceLineFilledColor);
	settings->setValue("colors/panCenterLine", m_panadapterColors.panCenterLineColor);

	SETTINGS_DEBUG << "saveSettings done.";
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

void Settings::setMainPower(QObject *sender, bool power) {

	QMutexLocker locker(&mutex);
		if (m_mainPower == power) return;
		m_mainPower = power;
		if (power) 
			m_mainPower = true;
		else
			m_mainPower = false;
	locker.unlock();

	emit masterSwitchChanged(sender, m_mainPower);
}

void Settings::setSystemState(

	QObject *sender, 
	QSDR::_Error err, 
	QSDR::_HWInterfaceMode hwmode, 
	QSDR::_ServerMode mode, 
	QSDR::_DataEngineState state)
{
	Q_UNUSED (sender)

	bool change = false;
	QMutexLocker locker(&mutex);

	SETTINGS_DEBUG << "system state error: " << qPrintable(getErrorString(err));
	if (m_systemError != err) {
		
		m_systemError = err;
		change = true;
	}

	if (m_hwInterface != hwmode) {
		
		m_hwInterface = hwmode;
		change = true;
	}

	if (m_serverMode != mode) {
		
		m_serverMode = mode;

		if (m_serverMode == QSDR::ChirpWSPR)
			setWideBandStatus(false);

		change = true;
	}

	if (m_dataEngineState != state) {
		
		m_dataEngineState = state;
		change = true;
	}

	if (!change) return;

	locker.unlock();

	SETTINGS_DEBUG	<< "hwInterface: " << qPrintable(getHWInterfaceModeString(m_hwInterface));
	SETTINGS_DEBUG	<< "serverMode: " << qPrintable(getServerModeString(m_serverMode));
	SETTINGS_DEBUG	<< "dataEngineState: " << qPrintable(getHDataEngineStateString(m_dataEngineState));

	emit systemStateChanged(this, m_systemError, m_hwInterface, m_serverMode, m_dataEngineState);
}

 
void Settings::setGraphicsState(

	QObject *sender, 
	QSDRGraphics::_Panadapter panMode,
	QSDRGraphics::_WaterfallColorScheme waterColorScheme)
{
	Q_UNUSED (sender)

	bool change = false;
	QMutexLocker locker(&mutex);

	if (m_panadapterMode != panMode) {

		m_panadapterMode = panMode;
		change = true;
	}

	if (m_waterfallColorScheme != waterColorScheme) {

		m_waterfallColorScheme = waterColorScheme;
		change = true;
	}

	if (!change) return;

	SETTINGS_DEBUG << "graphics mode:" << m_panadapterMode << m_waterfallColorScheme;
	emit graphicModeChanged(this, m_panadapterMode, m_waterfallColorScheme);

	locker.unlock();
}

 
void Settings::setDefaultSkin(QObject *sender, bool value) {

	Q_UNUSED (sender)

	m_defaultSkin = value;
}

void Settings::setSettingsFilename(QString filename) {

	filename = filename.trimmed();

	QMutexLocker locker(&mutex);
		settingsFilename = filename;
	locker.unlock();

	emit settingsFilenameChanged(filename);
}

void Settings::setSettingsLoaded(bool value) {

	QMutexLocker locker(&mutex);
		m_settingsLoaded = value;
	locker.unlock();

	emit settingsLoadedChanged(m_settingsLoaded);
}

void Settings::setCallsign(QString callsign) {

	callsign = callsign.trimmed();

	QMutexLocker locker(&mutex);
	if (m_callsignString == callsign.trimmed()) 
		return;
	m_callsignString = callsign.trimmed();
	locker.unlock();

	emit callsignChanged(callsign);
}

void Settings::setRxList(QList<HPSDRReceiver *> rxList) {

	emit rxListChanged(rxList);
}

void Settings::setMultiRxView(int view) {

	if (m_multiRxView == view) return;
	m_multiRxView = view;

	emit multiRxViewChanged(m_multiRxView);
}

void Settings::setMetisCardList(QList<TNetworkDevicecard> list) {

	m_metisCards = list;

	emit metisCardListChanged(m_metisCards);
}

void Settings::searchHpsdrNetworkDevices() {

	emit searchMetisSignal();
}

void Settings::clearMetisCardList() {

	m_metisCards.clear();

	//emit metisCardListChanged(m_metisCards);
}

void Settings::setCurrentMetisCard(TNetworkDevicecard card) {

	m_currentMetisCard = card;

	emit metisChanged(m_currentMetisCard);
}

void Settings::setHPSDRDeviceNumber(int value) {

	m_hpsdrNetworkDevices = value;
	emit networkDeviceNumberChanged(value);
}

void Settings::showNetworkIODialog() {

	emit showNetworkIO();
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

void Settings::setFBOPresence(bool value) {

	m_fboFound = value;
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

	QMutexLocker locker(&mutex);
	m_serverAddress = addr;
	locker.unlock();

	emit serverAddrChanged(sender, m_serverAddress);
}

void Settings::setHPSDRDeviceLocalAddr(QObject *sender, QString addr) {

	QMutexLocker locker(&mutex);
	m_hpsdrDeviceLocalAddr = addr;
	locker.unlock();

	emit hpsdrDeviceLocalAddrChanged(sender, m_hpsdrDeviceLocalAddr);
}

void Settings::setServerPort(QObject *sender, quint16 port) {

	QMutexLocker locker(&mutex);
	m_serverPort = port;
	locker.unlock();

	emit serverPortChanged(sender, m_serverPort);
}

void Settings::setListenPort(QObject *sender, quint16 port) {

	QMutexLocker locker(&mutex);
	m_listenerPort = port;
	locker.unlock();

	emit listenPortChanged(sender, m_listenerPort);
}

void Settings::setAudioPort(QObject *sender, quint16 port) {

	QMutexLocker locker(&mutex);
	m_audioPort = port;
	locker.unlock();

	emit audioPortChanged(sender, m_audioPort);
}

void Settings::setMetisPort(QObject *sender, quint16 port) {

	QMutexLocker locker(&mutex);
	m_metisPort = port;
	locker.unlock();

	emit metisPortChanged(sender, m_metisPort);
}

void Settings::setClientConnected(QObject *sender, bool value) {

	QMutexLocker locker(&mutex);
	m_clientConnected = value;
	locker.unlock();

	emit clientConnectedChanged(sender, m_clientConnected);
}

void Settings::setClientNoConnected(QObject* sender, int client) {

	QMutexLocker locker(&mutex);
	m_clientNoConnected = client;
	locker.unlock();

	emit clientNoConnectedChanged(sender, m_clientNoConnected);
}

void Settings::setAudioRx(QObject* sender, int rx) {

	emit audioRxChanged(sender, rx);
}

void Settings::setConnected(QObject *sender, bool value) {

	QMutexLocker locker(&mutex);
	m_connected = value;
	locker.unlock();

	emit connectedChanged(sender, m_connected);
}

void Settings::clientDisconnected(int client) {

	emit clientDisconnectedEvent(client);
}

void Settings::setRxConnectedStatus(QObject* sender, int rx, bool value) {

	emit rxConnectedStatusChanged(sender, rx, value);
}
 
 
//*******************************
// HPSDR hardware presence and firmware versions

//void Settings::setHermesPresence(bool value) {
//
//	QMutexLocker locker(&mutex);
//	m_hermesPresence = value;
//	locker.unlock();
//
//	emit hermesPresenceChanged(m_hermesPresence);
//}

void Settings::setHPSDRHardware(int value) {

	QMutexLocker locker(&mutex);
	m_hpsdrHardware = value; // 0 = Mercury/Penelope, 1 = Hermes, 2 = Cyclops
	locker.unlock();

	emit hpsdrHardwareChanged(m_hpsdrHardware);
}

void Settings::setHermesVersion(int value) {

	QMutexLocker locker(&mutex);
	m_hermesVersion = value;
	locker.unlock();

	emit hermesVersionChanged(m_hermesVersion);
}

void Settings::setMercuryPresence(bool value) {

	QMutexLocker locker(&mutex);
	m_mercuryPresence = value;
	locker.unlock();

	emit mercuryPresenceChanged(m_mercuryPresence);
}

void Settings::setMercuryVersion(int value) {

	QMutexLocker locker(&mutex);
	m_mercuryVersion = value;
	locker.unlock();

	emit mercuryVersionChanged(m_mercuryVersion);
}

void Settings::setPenelopePresence(bool value) {

	QMutexLocker locker(&mutex);
	m_penelopePresence = value;
	locker.unlock();

	emit penelopePresenceChanged(m_penelopePresence);
}

void Settings::setPenelopeVersion(int value) {

	QMutexLocker locker(&mutex);
	m_penelopeVersion = value;
	locker.unlock();

	emit penelopeVersionChanged(m_penelopeVersion);
}

void Settings::setPennyLanePresence(bool value) {

	QMutexLocker locker(&mutex);
	m_pennyLanePresence = value;
	locker.unlock();

	emit pennyLanePresenceChanged(m_pennyLanePresence);
}

void Settings::setPennyLaneVersion(int value) {

	QMutexLocker locker(&mutex);
	m_pennyLaneVersion = value;
	locker.unlock();

	emit pennyLaneVersionChanged(m_pennyLaneVersion);
}

void Settings::setAlexPresence(bool value) {

	QMutexLocker locker(&mutex);
	m_alexPresence = value;
	locker.unlock();

	emit alexPresenceChanged(m_alexPresence);
}

void Settings::setExcaliburPresence(bool value) {

	QMutexLocker locker(&mutex);
	m_excaliburPresence = value;
	locker.unlock();

	emit excaliburPresenceChanged(m_excaliburPresence);
}

void Settings::setHWInterfaceVersion(int value) {

	QMutexLocker locker(&mutex);
	m_hwInterfaceVersion = value;
	locker.unlock();

	emit hwInterfaceVersionChanged(m_hwInterfaceVersion);
}

void Settings::setProtocolSync(int value) {

	emit protocolSyncChanged(value);
}

void Settings::setADCOverflow(int value) {

	emit adcOverflowChanged(value);
}

void Settings::setSendIQ(int value) {

	emit sendIQSignalChanged(value);
}

void Settings::setRcveIQ(int value) {

	emit rcveIQSignalChanged(value);
}

/**
 * Set the number of receivers to be supported by this server
 * \param r The number of receivers: 1, 2, 3, or 4
 * This value (less one) is embedded into the command & control bytes that are sent to Mercury.
 * Thus it determines how the I & Q samples read from EP6 are placed in the data stream to dspservers.
 */
void Settings::setReceivers(QObject *sender, int value) {

	QMutexLocker locker(&mutex);

		if (m_mercuryReceivers == value) return;
		if (value > MAX_RECEIVERS) value = MAX_RECEIVERS;

		m_mercuryReceivers = value;
		
	locker.unlock();

	emit numberOfRXChanged(sender, value);
}

void Settings::setReceiver(QObject *sender, int value) {

	QMutexLocker locker(&mutex);

		if (m_currentReceiver == value) return;
		if (value > MAX_RECEIVERS) value = MAX_RECEIVERS;

		m_currentReceiver = value;
		
	locker.unlock();

	emit receiverChanged(value);
	emit frequencyChanged(this, true, value, m_receiverDataList[value].frequency);
}

void Settings::setCurrentReceiver(int value) {

	QMutexLocker locker(&mutex);

		//if (m_currentReceiver == value) return;
		if (value > MAX_RECEIVERS) value = MAX_RECEIVERS;

		m_currentReceiver = value;
		
	locker.unlock();

	emit currentReceiverChanged(value);
}

void Settings::setSampleRate(QObject *sender, int value) {

	QMutexLocker locker(&mutex);
	
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
				fprintf(stderr,"Invalid sample rate (48000,96000,192000)!\n");
				exit(1);
				break;
		}
		
	locker.unlock();
	
	emit sampleRateChanged(sender, value);
}

void Settings::setPreamp(QObject *sender, int value) {

	m_mercuryPreamp = value;
	emit preampChanged(sender, value);
}

void Settings::setDither(QObject *sender, int value) {

	m_mercuryDither = value;
	emit ditherChanged(sender, value);
}

void Settings::setRandom(QObject *sender, int value) {

	m_mercuryRandom = value;
	emit randomChanged(sender, value);
}

void Settings::set10MhzSource(QObject *sender, int source) {

	m_10MHzSource = source;
	emit src10MhzChanged(sender, source);
}

void Settings::set122_88MhzSource(QObject *sender, int source) {

	m_122_8MHzSource = source;
	emit src122_88MhzChanged(sender, source);
}

void Settings::setMicSource(QObject *sender, int source) {

	m_micSource = source;
	emit micSourceChanged(sender, source);
}

void Settings::setClass(QObject *sender, int value) {

	m_RxClass = value;
	emit classChanged(sender, value);
}

void Settings::setTiming(QObject *sender, int value) {

	m_RxTiming = value;
	emit timingChanged(sender, value);
}

void Settings::setMouseWheelFreqStep(QObject *sender, double value) {

	m_mouseWheelFreqStep = value;
	emit mouseWheelFreqStepChanged(sender, value);
}

qreal Settings::getMainVolume(int rx) {

	return m_receiverDataList[rx].audioVolume;
}

void Settings::setMainVolume(QObject *sender, int rx, float volume) {

	if (volume < 0) volume = 0.0f;
	if (volume > 1) volume = 1.0f;

	QMutexLocker locker(&mutex);
		if (m_receiverDataList[rx].audioVolume == volume) return;
		m_receiverDataList[rx].audioVolume = volume;
	locker.unlock();

	emit mainVolumeChanged(sender, rx, volume);
}

void Settings::setFramesPerSecond(QObject *sender, int value) {

	if (m_framesPerSecond == value) return;
	m_framesPerSecond = value;

	framesPerSecondChanged(sender, value);
}

void Settings::setFrequency(int rx, long frequency) {

	//HamBand band = getBandFromFrequency(getBandFrequencyList(), frequency);
	HamBand band = getBandFromFrequency(m_bandList, frequency);

	m_receiverDataList[rx].frequency = frequency;
	m_receiverDataList[rx].hamBand = band;
	m_receiverDataList[rx].lastFrequencyList[(int) band] = frequency;
}

void Settings::setFrequency(QObject *sender, bool value, int rx, long frequency) {

	//SETTINGS_DEBUG << "change frequency for rx" << rx;
	QMutexLocker locker(&mutex);
		if (m_receiverDataList[rx].frequency == frequency) return;
		//m_centerFrequencies[rx] = frequency;
		m_receiverDataList[rx].frequency = frequency;
		HamBand band = getBandFromFrequency(m_bandList, frequency);
		m_receiverDataList[rx].hamBand = band;
		m_receiverDataList[rx].lastFrequencyList[(int) band] = frequency;
	locker.unlock();

	emit frequencyChanged(sender, value, rx, frequency);
}

void Settings::setIQPort(QObject *sender, int rx, int port) {

	emit iqPortChanged(sender, rx, port);
}

void Settings::setWideBandStatus(bool value) {

	m_wideBandStatus = value;

	emit wideBandStausChanged(value);
}

void Settings::setWideBandData(bool value) {

	m_wideBandData = value;

	emit wideBandDataChanged(value);
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

void Settings::setWidebandSpectrumBuffer(const float* buffer) {

	emit widebandSpectrumBufferChanged(buffer);
}
 
//**************************************

void Settings::setHamBand(QObject *sender, int rx, HamBand band) {

	QMutexLocker locker(&mutex);
		if (m_receiverDataList[rx].hamBand == band) return;
		m_receiverDataList[rx].hamBand = band;
	locker.unlock();

	emit hamBandChanged(sender, rx, band);
}

void Settings::setDSPMode(QObject *sender, int rx, DSPMode mode) {

	QMutexLocker locker(&mutex);
		if (m_receiverDataList[rx].dspMode == mode) return;
		m_receiverDataList[rx].dspMode = mode;
		setRXFilter(this, rx, m_defaultFilterList.at((int) mode).filterLo, m_defaultFilterList.at((int) mode).filterHi);
	locker.unlock();

	emit dspModeChanged(sender, rx, mode);
}

void Settings::setAGCMode(QObject *sender, int rx, AGCMode mode) {

	QMutexLocker locker(&mutex);
		if (m_receiverDataList[rx].agcMode == mode) return;
		m_receiverDataList[rx].agcMode = mode;
	locker.unlock();

	emit agcModeChanged(sender, rx, mode);
}

int Settings::getAGCGain(int rx) {

	return m_receiverDataList[rx].acgGain;
}

void Settings::setAGCGain(QObject *sender, int rx, int value) {

	QMutexLocker locker(&mutex);
		if (m_receiverDataList[rx].acgGain == value) return;
		m_receiverDataList[rx].acgGain = value;
	locker.unlock();

	emit agcGainChanged(sender, rx, value);
}

void Settings::setRXFilter(QObject *sender, int rx, qreal low, qreal high) {

	if (m_filterFrequencyLow == low && m_filterFrequencyHigh == high) return;
	m_filterFrequencyLow = low;
	m_filterFrequencyHigh = high;
	m_receiverDataList[rx].filterLo = low;
	m_receiverDataList[rx].filterHi = high;

	emit filterFrequenciesChanged(sender, rx, m_filterFrequencyLow, m_filterFrequencyHigh);
}

 
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

	QMutexLocker locker(&mutex);
		//if (m_format == format) return;
		m_format = format;
	locker.unlock();

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

	QMutexLocker locker(&mutex);
		if (m_lowerChirpFreq == value) return;
		if (m_lowerChirpFreq >= m_upperChirpFreq)
			m_lowerChirpFreq = m_upperChirpFreq;
		else
			m_lowerChirpFreq = value;
	locker.unlock();

	emit lowerChirpFreqChanged(this, m_lowerChirpFreq);
}

void Settings::setUpperChirpFreq(int value) {

	QMutexLocker locker(&mutex);
		if (m_upperChirpFreq == value) return;
		if (m_lowerChirpFreq >= m_upperChirpFreq)
			m_upperChirpFreq = m_lowerChirpFreq;
		else
			m_upperChirpFreq = value;
	locker.unlock();

	emit upperChirpFreqChanged(this, m_upperChirpFreq);
}

void Settings::setChirpAmplitude(qreal value) {

	QMutexLocker locker(&mutex);
		if (m_chirpAmplitude == value) return;
		m_chirpAmplitude = value;
	locker.unlock();

	emit chirpAmplitudeChanged(this, m_chirpAmplitude);
}

void Settings::setChirpSamplingFreq(int value) {

	QMutexLocker locker(&mutex);
		if (m_chirpSamplingFreq == value) return;
		m_chirpSamplingFreq = value;
	locker.unlock();

	emit chirpSamplingFreqChanged(this, m_chirpSamplingFreq);
}

void Settings::setChirpBufferDurationUs(int value) {

	QMutexLocker locker(&mutex);
		if (m_chirpBufferDurationUs == (qint64)value) return;
		m_chirpBufferDurationUs = (qint64)(1000 * value);
	locker.unlock();

	emit chirpBufferDurationUsChanged(this, m_chirpBufferDurationUs);
}

void Settings::setChirpRepetitionTimes(int value) {

	QMutexLocker locker(&mutex);
		if (m_chirpRepetitionTimes == value) return;
		m_chirpRepetitionTimes = value;
	locker.unlock();

	emit chirpRepetitionTimesChanged(this, m_chirpRepetitionTimes);
}

void Settings::setChirpReceiver(bool value) {

	QMutexLocker locker(&mutex);
		if (m_chirpReceiverOn == value) return;
		m_chirpReceiverOn = value;
	locker.unlock();

	emit chirpReceiverChanged(this, m_chirpReceiverOn);
}

void Settings::setChirpAvgLength(int value) {

	QMutexLocker locker(&mutex);
		if (m_chirpAvgLength == value) return;
		m_chirpAvgLength = value;
	locker.unlock();

	emit chirpAvgLengthChanged(m_chirpAvgLength);
}

void Settings::setChirpFFTShow(bool value) {

	QMutexLocker locker(&mutex);
		if (m_showChirpFFT == value) return;
		m_showChirpFFT = value;
	locker.unlock();

	emit chirpFFTShowChanged(m_showChirpFFT);
	
}

void Settings::setChirpUSB(bool value) {

	QMutexLocker locker(&mutex);
		if (m_chirpUSB == value) return;
		m_chirpUSB = value;
	locker.unlock();

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

void Settings::setSpectrumSize(int value) {

	if (m_spectrumSize == value) return;

	m_spectrumSize = value;
	emit spectrumSizeChanged(m_spectrumSize);
}

void Settings::moveDisplayWidget(int value) {

	emit displayWidgetHeightChanged(value);
}


//*********************************
// color stuff

void Settings::setPanadapterColors(TPanadapterColors type) {

	if (type.panBackgroundColor != m_panadapterColors.panBackgroundColor)
		m_panadapterColors.panBackgroundColor = type.panBackgroundColor;

	if (type.panCenterLineColor != m_panadapterColors.waterfallColor)
		m_panadapterColors.waterfallColor = type.waterfallColor;
	
	if (type.panCenterLineColor != m_panadapterColors.panLineColor)
		m_panadapterColors.panLineColor = type.panLineColor;
	
	if (type.panCenterLineColor != m_panadapterColors.panLineFilledColor)
		m_panadapterColors.panLineFilledColor = type.panLineFilledColor;
	
	if (type.panCenterLineColor != m_panadapterColors.panSolidTopColor)
		m_panadapterColors.panSolidTopColor = type.panSolidTopColor;
	
	if (type.panCenterLineColor != m_panadapterColors.panSolidBottomColor)
		m_panadapterColors.panSolidBottomColor = type.panSolidBottomColor;
	
	if (type.panCenterLineColor != m_panadapterColors.wideBandLineColor)
		m_panadapterColors.wideBandLineColor = type.wideBandLineColor;
	
	if (type.panCenterLineColor != m_panadapterColors.wideBandFilledColor)
		m_panadapterColors.wideBandFilledColor = type.wideBandFilledColor;
	
	if (type.panCenterLineColor != m_panadapterColors.wideBandSolidTopColor)
		m_panadapterColors.wideBandSolidTopColor = type.wideBandSolidTopColor;
	
	if (type.panCenterLineColor != m_panadapterColors.wideBandSolidBottomColor)
		m_panadapterColors.wideBandSolidBottomColor = type.wideBandSolidBottomColor;

	if (type.distanceLineColor != m_panadapterColors.distanceLineColor)
		m_panadapterColors.distanceLineColor = type.distanceLineColor;

	if (type.distanceLineFilledColor != m_panadapterColors.distanceLineFilledColor)
		m_panadapterColors.distanceLineFilledColor = type.distanceLineFilledColor;

	if (type.panCenterLineColor != m_panadapterColors.panCenterLineColor)
		m_panadapterColors.panCenterLineColor = type.panCenterLineColor;

	emit panadapterColorChanged();
}

void Settings::setFramesPerSecond(int value) {

	QMutexLocker locker(&mutex);
		if (m_framesPerSecond == value) return;
		m_framesPerSecond = value;
	locker.unlock();

	emit framesPerSecondChanged(this, m_framesPerSecond);
	
}

void Settings::setGraphicResolution(int value) {

	QMutexLocker locker(&mutex);
		if (m_graphicResolution == value) return;
		m_graphicResolution = value;
	locker.unlock();

	emit graphicResolutionChanged(this, m_graphicResolution);
	
}

void Settings::setSpectrumAveraging(bool value) {

	QMutexLocker locker(&mutex);
		if (m_specAveraging == value) return;
		m_specAveraging = value;
	locker.unlock();

	emit spectrumAveragingChanged(m_specAveraging);
}

void Settings::setSpectrumAveragingCnt(int value) {

	QMutexLocker locker(&mutex);
		if (m_specAveragingCnt == value) return;
		m_specAveragingCnt = value;
	locker.unlock();

	emit spectrumAveragingCntChanged(m_specAveragingCnt);
}

void Settings::setPanGrid(bool value) {

	QMutexLocker locker(&mutex);
		if (m_panGrid == value) return;
		m_panGrid = value;
	locker.unlock();

	emit panGridStatusChanged(m_panGrid);
}

void Settings::setWaterfallTime(int rx, int value) {

	QMutexLocker locker(&mutex);
		if (m_receiverDataList[rx].waterfallTime == value) return;
		m_receiverDataList[rx].waterfallTime = value;
	locker.unlock();

	emit waterfallTimeChanged(rx, value);
}

void Settings::setWaterfallOffesetLo(int rx, int value) {

		QMutexLocker locker(&mutex);
		if (m_receiverDataList[rx].waterfallOffsetLo == value) return;
		m_receiverDataList[rx].waterfallOffsetLo = value;
	locker.unlock();

	emit waterfallOffesetLoChanged(rx, value);
}

void Settings::setWaterfallOffesetHi(int rx, int value) {

		QMutexLocker locker(&mutex);
		if (m_receiverDataList[rx].waterfallOffsetHi == value) return;
		m_receiverDataList[rx].waterfallOffsetHi = value;
	locker.unlock();

	emit waterfallOffesetHiChanged(rx, value);
}

void Settings::setSMeterHoldTime(int value) {

	QMutexLocker locker(&mutex);
		if (m_sMeterHoldTime == value) return;
		m_sMeterHoldTime = value;
	locker.unlock();

	emit sMeterHoldTimeChanged(m_sMeterHoldTime);
}

void Settings::setdBmPanScaleMin(int rx, qreal value) {

	QMutexLocker locker(&mutex);
		if (m_receiverDataList[rx].dBmPanScaleMin == value) return;
		m_receiverDataList[rx].dBmPanScaleMin = value;
	locker.unlock();

	emit dBmScaleMinChanged(rx, value);
}

void Settings::setdBmPanScaleMax(int rx, qreal value) {

	QMutexLocker locker(&mutex);
		if (m_receiverDataList[rx].dBmPanScaleMax == value) return;
		m_receiverDataList[rx].dBmPanScaleMax = value;
	locker.unlock();

	emit dBmScaleMaxChanged(rx, value);
}

void Settings::setdBmWBScaleMin(qreal value) {

	QMutexLocker locker(&mutex);
		if (m_dBmWBScaleMin == value) return;
		m_dBmWBScaleMin = value;
	locker.unlock();

	emit dBmScaleWBMinChanged(value);
}

void Settings::setdBmWBScaleMax(qreal value) {

	QMutexLocker locker(&mutex);
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