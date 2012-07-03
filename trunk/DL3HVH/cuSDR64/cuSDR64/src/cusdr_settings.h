/**
* @file  cusdr_settings.h
* @brief settings header file for cuSDR
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
 
#ifndef CUSDR_SETTINGS_H
#define CUSDR_SETTINGS_H

#include <QtGlobal>
#include <QObject>
#include <QSettings>
#include <QDateTime>
#include <QErrorMessage>
#include <QFile>
#include <QFont>
#include <QMutex>
#include <QString>
#include <QTextStream>
#include <QtNetwork>
#include <QtMultimedia/qaudio.h>
#include <QtMultimedia/QAudioFormat>
#include <QtCore/QVector>

//#define WIN32_LEAN_AND_MEAN
//#define _WIN32_WINNT 0x0601  // Win 7
//#define _WIN32_WINNT 0x0501  // WinXP

//#define LOG_ALL


#include "cusdr_hamDatabase.h"
#include "cusdr_complex.h"

// test for OpenCL
#include "qclcontext.h"

//// **************************************
//// logging
//#ifdef LOG_ALL
//#define LOG_SETTINGS
//#define LOG_MAIN
//#define LOG_SERVER
//#define LOG_DATA_ENGINE
//#define LOG_DATA_RECEIVER
//#define LOG_DATA_PROCESSOR
//#define LOG_NETWORKDIALOG
//#define LOG_HPSDRIO
//#define LOG_AUDIO_ENGINE
//#define LOG_AUDIO_WAVFILE
//#define LOG_AUDIO_RECEIVER
//#define LOG_AUDIO_PROCESSOR
//#define LOG_CHIRP_PROCESSOR
//#define LOG_DTTSP_API
//#define LOG_SPECTRUMANALYSER
//#define LOG_WAVEFORM
//#define LOG_PAINT_EVENT
//#define LOG_GRAPHICS
//#define LOG_WBGRAPHICS
//#define LOG_DISPLAYPANEL
//#define LOG_CUDAGRAPH
//#define LOG_SERVER_WIDGET
//#define LOG_CHIRP_WIDGET
//#define LOG_RECEIVER
//#define LOG_FUTUREWATCHER
//#define LOG_ALEX_WIDGET
//#endif

// **************************************
// messages

#define BANDSCOPE_IN_USE "Error: bandscope in use"
#define BANDSCOPE_NOT_OWNER "Error: Not owner of bandscope"

#define RECEIVER_INVALID "Error: Invalid Receiver"
#define RECEIVER_IN_USE "Error: Receiver in use"
#define RECEIVER_NOT_OWNER "Error: Not owner of receiver"

#define CLIENT_ATTACHED "Error: Client is already attached to receiver"
#define CLIENT_DETACHED "Error: Client is not attached to receiver"

#define INVALID_COMMAND "Error: Invalid Command"

#define OK "OK"

// **************************************
// constants

#define ONEPI 3.14159265358979323846264338328
#define TWOPI 6.28318530717958647692528676656

#define MAXFREQUENCY 61440000
#define MINDBM -180
#define MAXDBM 10
#define MINDISTDBM -150
#define MAXDISTDBM 150

// **************************************
// receiver settings

#define MAX_RECEIVERS				4
#define BUFFER_SIZE					1024
#define SAMPLE_BUFFER_SIZE			4096

#define								SMALL_PACKETS


// **************************************
// IO buffer, command & control settings

#define IO_BUFFERS					16
#define IO_BUFFER_SIZE				512
#define IO_HEADER_SIZE				8
#define IO_AUDIOBUFFER_SIZE			8192

#define SYNC						0x7F

#define IQ_DATAGRAM_BUFFERS			512
#define AUDIO_DATAGRAM_BUFFERS		512

#define METIS_HEADER_SIZE			8
#define METIS_DATA_SIZE				1032

// uncomment to compile code that allows for SYNC error recovery
#define RESYNC

#define MOX_DISABLED				0x00
#define MOX_ENABLED					0x01

#define ATLAS_10MHZ_SOURCE			0x00
#define PENELOPE_10MHZ_SOURCE		0x04
#define MERCURY_10MHZ_SOURCE		0x08
#define PENELOPE_122_88MHZ_SOURCE	0x00
#define MERCURY_122_88MHZ_SOURCE	0x10
#define PENELOPE_PRESENT			0x20
#define MERCURY_PRESENT				0x40
#define MIC_SOURCE_PENELOPE			0x80

//#define MIC_SOURCE_JANUS			0x00
//#define CONFIG_NONE					0x00
//#define CONFIG_BOTH					0x60


//#define SPEED_48KHZ					0x00
//#define SPEED_96KHZ					0x01
//#define SPEED_192KHZ				0x02

#define MODE_CLASS_E				0x01
#define MODE_OTHERS					0x00

#define ALEX_ATTENUATION_0DB		0x00
#define ALEX_ATTENUATION_10DB		0x01
#define ALEX_ATTENUATION_20DB		0x02
#define ALEX_ATTENUATION_30DB		0x03
#define LT2208_GAIN_OFF				0x00
#define LT2208_GAIN_ON				0x04
#define LT2208_DITHER_OFF			0x00
#define LT2208_DITHER_ON			0x08
#define LT2208_RANDOM_OFF			0x00
#define LT2208_RANDOM_ON			0x10

#define SIMPLEX						0x00
#define DUPLEX						0x04

#define BANDSCOPE_BUFFER_SIZE		4096

#ifdef RESYNC

	#define			SYNC_0			0
	#define			SYNC_1			1
	#define			SYNC_2			2
	#define			CONTROL_0		3
	#define			CONTROL_1		4
	#define			CONTROL_2		5
	#define			CONTROL_3		6
	#define			CONTROL_4		7
	#define			LEFT_SAMPLE_0	8
	#define			LEFT_SAMPLE_1	9
	#define			LEFT_SAMPLE_2	10
	#define			RIGHT_SAMPLE_0	11
	#define			RIGHT_SAMPLE_1	12
	#define			RIGHT_SAMPLE_2	13
	#define			MIC_SAMPLE_0	14
	#define			MIC_SAMPLE_1	15

	static int		decode_state;

#endif

// **************************************
// Metis definitions

#define MAX_METIS_CARDS 10
#define DISCOVER_IDLE 0
#define DISCOVER_SENT 1
#define METIS_PORT 1024
//#define DATA_PORT 8886

// **************************************
// Audio definitions

#define	WAVEFORM_WINDOW_DURATION 250000
#define WAVEFORM_TILE_LENGTH 4096


#include "cusdr_fspectrum.h"
#include "cusdr_queue.h"


// **************************************
// Server modes

namespace QSDR {

	enum _Error { 
		
		NoError,
		NotImplemented,
		HwIOError,
		ServerModeError,
		OpenError, 
		DataProcessThreadError,
		WideBandDataProcessThreadError,
		AudioThreadError,
		UnderrunError, 
		FatalError
	};

	enum _DataEngineState {

		DataEngineDown,
		DataEngineUp
	};

	enum _ServerMode {

		NoServerMode,
        //InternalDSP,
		DttSP,
		QtDSP,
        //CudaDSP,
		ChirpWSPR,
		ChirpWSPRFile,
		ExternalDSP,
		DemoMode
	};

	enum _HWInterfaceMode { 
		
		NoInterfaceMode, 
		Metis,
		Hermes
	};
}

Q_DECLARE_METATYPE(QSDR::_Error)
Q_DECLARE_METATYPE(QSDR::_DataEngineState)
Q_DECLARE_METATYPE(QSDR::_ServerMode)
Q_DECLARE_METATYPE(QSDR::_HWInterfaceMode)

// **************************************
// Graphic modes

namespace QSDRGraphics {

	enum _Panadapter { 
		
		Line,
		FilledLine,
		Solid
	};

	enum _WaterfallColorScheme { 
		
		simple,
		enhanced,
		spectran
	};

	/*enum _Colors { 
		
		PanCenterLineColor,
		PanLine,
		PanLineFilled,
		PanSolidTop,
		PanSolidBottom,
		WideBandLine,
		WideBandFilled,
		WideBandSolidTop,
		WideBandSolidBottom
	};*/
}

Q_DECLARE_METATYPE(QSDRGraphics::_Panadapter)
Q_DECLARE_METATYPE(QSDRGraphics::_WaterfallColorScheme)
//Q_DECLARE_METATYPE(QSDRGraphics::_Colors)


// **************************************
// receiver modes

//namespace QRadio {
//
//	typedef struct _metisCard {
//
//		QHostAddress	ip_address;
//		char			mac_address[18];
//
//	} TMetiscard;
//
//	QList<TMetiscard>	TMetiscards;
//}
//
//Q_DECLARE_METATYPE (QRadio::TMetiscard)
//Q_DECLARE_METATYPE (QRadio::TMetiscards)

enum {
	prefixNothing = 0,	/*!< No prefix. */

	/* SI units. */
	prefixKilo = 1,		/*!< Kilo prefix 1000^1 = 10^3. */
	prefixMega = 2,		/*!< Mega prefix 1000^2 = 10^6. */
	prefixGiga = 3,		/*!< Giga prefix 1000^3 = 10^9. */
	prefixTera = 4,		/*!< Tera prefix 1000^4 = 10^12. */
	prefixPeta = 5,		/*!< Peta prefix 1000^5 = 10^15. */
	prefixExa = 6,		/*!< Exa prefix 1000^6 = 10^18. */
	prefixZetta = 7,	/*!< Zetta prefix 1000^7 = 10^21. */
	prefixYotta = 8,	/*!< Yotta prefix 1000^8 = 10^24. */
	prefixSiMax = prefixYotta,

	/* IEC 60027 units. */
	prefixKibi = 1,		/*!< Kibi prefix 1024^1 = 2^10. */
	prefixMebi = 2,		/*!< Mebi prefix 1024^2 = 2^20. */
	prefixGibi = 3,		/*!< Gibi prefix 1024^3 = 2^30. */
	prefixTebi = 4,		/*!< Tebi prefix 1024^4 = 2^40. */
	prefixPebi = 5,		/*!< Pebi prefix 1024^5 = 2^50. */
	prefixExbi = 6,		/*!< Exbi prefix 1024^6 = 2^60. */
	prefixZebi = 7,		/*!< Zebi prefix 1024^7 = 2^70. */
	prefixYobi = 8,		/*!< Yobi prefix 1024^8 = 2^80. */
	prefixIecMax = prefixYobi,
};
  

// **************************************
// type definitions

typedef struct _frequency {

	int	freqMHz;
	int	freqkHz;

} TFrequency;

typedef struct _ccParameterRx {

	uchar	roundRobin;	// roundRobin is varied in a round-robin fashion in order to decode
						// all values which are sent in sequence. 

	bool	ptt;		// PTT  (1 = active, 0 = inactive), GPIO[23]= Ozy J8-8, Hermes J16-1
	bool	dash;		// DASH (1 = active, 0 = inactive), GPIO[21]= Ozy J8-6, Hermes J6-2
	bool	dot;		// DOT  (1 = active, 0 = inactive), GPIO[22]= Ozy J8-7, Hermes J6-3
	bool	lt2208;		// LT2208 Overflow (1 = active, 0 = inactive)
	bool	hermesI01;	// Hermes I01 (0 = active, 1 = inactive)
	bool	hermesI02;	// Hermes I02 (0 = active, 1 = inactive)
	bool	hermesI03;	// Hermes I03 (0 = active, 1 = inactive)
	bool	hermesI04;	// Hermes I04 (0 = active, 1 = inactive)
	bool	cyclopsPLL;	// Cyclops PLL locked (0 = unlocked, 1 = locked)
	bool	cyclops;	// Cyclops - Mercury frequency changed, bit toggles 

	int		mercuryFirmwareVersion;			// Mercury firmware version
	int		penelopeFirmwareVersion;		// Penelope firmware version
	int		networkDeviceFirmwareVersion;	// Metis/Hermes firmware version

	quint16	ain1;		// Forward Power from Alex or Apollo
	quint16	ain2;		// Reverse Power from Alex or Apollo
	quint16	ain3;		// AIN3 from Penny or Hermes
	quint16	ain4;		// AIN4 from Penny or Hermes
	quint16	ain5;		// Forward Power from Penelope or Hermes
	quint16	ain6;		// AIN6,13.8v supply on Hermes

	bool	mercury1_LT2208;	//Mercury 1 LT2208 Overflow (1 = active, 0 = inactive)
	bool	mercury2_LT2208;	//Mercury 2 LT2208 Overflow (1 = active, 0 = inactive)
	bool	mercury3_LT2208;	//Mercury 3 LT2208 Overflow (1 = active, 0 = inactive)
	bool	mercury4_LT2208;	//Mercury 4 LT2208 Overflow (1 = active, 0 = inactive)


} TCCParameterRx;

typedef struct _ccParameterTx {

	bool	mox;
	bool	lineIn;
	bool	micGain20dB;

	int		hpsdr10MhzSource;
	int		hpsdr122_88MhzSource;
	int		hpsdrConfig;

} TCCParameterTx;

typedef struct _hpsdrParameter {

	uchar	control_in[5];
	uchar	control_out[5];

	uchar	output_buffer[IO_BUFFER_SIZE];
	//float			in_buffer[2*BUFFER_SIZE];
	float			out_buffer[2*BUFFER_SIZE];

	float			wbWindow[4*BUFFER_SIZE];

	CPX*	cpxIn;
	CPX*	cpxOut;
	CPX*	cpxWBIn;
	CPX*	cpxWBOut;
	CPX*	cpxTmp;

	QByteArray	audioDatagram;
	
	QHQueue<QByteArray>		iq_queue;
	QHQueue<QByteArray>		au_queue;
	QHQueue<QByteArray>		wb_queue;
	QHQueue<QList<qreal>>	chirp_queue;
	QHQueue<QList<qreal>>	data_queue;

	QList<qreal> inputBuffer;

	QList<int> clientList;
	
	QMutex mutex;
	QMutex networkIOMutex;

	QWaitCondition devicefound;
	//QMutex iqMutex;

	QHostAddress	metisIPAddress;

	TCCParameterRx	ccRx;
	TCCParameterTx	ccTx;

	int		samplerate;
	int		speed;
	int		outputMultiplier;

	int		clients;
	int		current_client;
	int		receivers;
	int		currentReceiver;
	int		audio_rx;
	int		timing;
	
	int		currentMetisCard;

	int		preamp;
	int		dither;
	int		random;

	//int		hpsdr_10MhzSource;
	//int		hpsdr_122_88MhzSource;
	int		mic_source;
	int		rxClass;
	int		rx_freq_change;
	
	float	mic_gain;
	float	mic_left_buffer[BUFFER_SIZE];
	float	mic_right_buffer[BUFFER_SIZE];

	qreal	penelopeForwardVolts;
	qreal	penelopeForwardPower;
	qreal	alexForwardVolts;
	qreal	alexForwardPower;
	qreal	alexReverseVolts;
	qreal	alexReversePower;
	qreal	ain3Volts;
	qreal	ain4Volts;
	qreal	supplyVolts;

	bool	rcveIQ_toggle;
	bool	sendIQ_toggle;
	bool	timeStamp;
	bool	mute;

	qint16		audiofileChannels;

} THPSDRParameter;

//Q_DECLARE_METATYPE (THPSDRParameter)

typedef struct _networkDeviceCard {

		QHostAddress	ip_address;
		char			mac_address[18];
		int				boardID;
		QString			boardName;

} TNetworkDevicecard;

Q_DECLARE_METATYPE (TNetworkDevicecard)
Q_DECLARE_METATYPE (QList<TNetworkDevicecard>)

typedef struct _receiver {

	HamBand				hamBand;
	DSPMode				dspMode;
	AGCMode				agcMode;
	TDefaultFilterMode	defaultFilterMode;
	QList<long>			lastFrequencyList;

	long	frequency;

	float	audioVolume;

	qreal	filterLo;
	qreal	filterHi;
	qreal	dBmPanScaleMin;
	qreal	dBmPanScaleMax;

	int		sampleRate;
	int		acgGain;
	int		waterfallTime;
	int		waterfallOffsetLo;
	int		waterfallOffsetHi;

} TReceiver;

typedef struct t_panadapterColors {

	QColor		panBackgroundColor;
	QColor		waterfallColor;
	QColor		panLineColor;
	QColor		panLineFilledColor;
	QColor		panSolidTopColor;
	QColor		panSolidBottomColor;
	QColor		wideBandLineColor;
	QColor		wideBandFilledColor;
	QColor		wideBandSolidTopColor;
	QColor		wideBandSolidBottomColor;
	QColor		distanceLineColor;
	QColor		distanceLineFilledColor;
	QColor		panCenterLineColor;

} TPanadapterColors;

typedef enum {

	SIGNAL_STRENGTH,
	AVG_SIGNAL_STRENGTH,
	ADC_REAL,
	ADC_IMAG,
	AGC_GAIN,
	MIC,
	PWR,
	ALC,
	EQtap,
	LEVELER,
	COMP,
	CPDR,
	ALC_G,
	LVL_G,
	MIC_PK,
	ALC_PK,
	EQ_PK,
	LEVELER_PK,
	COMP_PK,
	CPDR_PK

} TMeterType;

typedef enum _windowtype {

  RECTANGULAR_WINDOW,
  HANNING_WINDOW,
  WELCH_WINDOW,
  PARZEN_WINDOW,
  BARTLETT_WINDOW,
  HAMMING_WINDOW,
  BLACKMAN2_WINDOW,
  BLACKMAN3_WINDOW,
  BLACKMAN4_WINDOW,
  EXPONENTIAL_WINDOW,
  RIEMANN_WINDOW,
  BLACKMANHARRIS_WINDOW,
  NUTTALL_WINDOW

} TWindowtype;

static int	currentRx;

class HPSDRReceiver;

// *********************************************************************
// thread class

class QThreadEx : public QThread {

protected:
    void run() { exec(); }

};

// **************************************
// Settings class

class Settings : public QObject {

	Q_OBJECT

public:
	static Settings *instance(QObject *parent = 0) {

		if (Settings::m_instance)
			return Settings::m_instance;

		Settings::m_instance = new Settings(parent);
		
		return Settings::m_instance;
	}

	static void delete_instance() {
	
		if (Settings::m_instance) {
		
			disconnect(Settings::m_instance, 0, 0, 0);
			delete Settings::m_instance;
			Settings::m_instance = 0;
		}
	}

	virtual ~Settings();

private:
	Settings(QObject *parent = 0);

	static Settings		*m_instance;

	QSettings			*settings;
	QSettings			*debugLog;
	QErrorMessage		*error;

signals:
	void messageEvent(QString message);

	void masterSwitchChanged(QObject *sender, bool power);

	void systemStateChanged(
		QObject *sender, 
		QSDR::_Error err, 
		QSDR::_HWInterfaceMode hwmode, 
		QSDR::_ServerMode mode, 
		QSDR::_DataEngineState state);

	void graphicModeChanged(
		QObject *sender,
		QSDRGraphics::_Panadapter panMode,
		QSDRGraphics::_WaterfallColorScheme waterColorScheme);
		//QSDRGraphics::_Colors colorItem);

	void multiRxViewChanged(int view);
	void sMeterValueChanged(int rx, float value);
	void spectrumBufferChanged(int rx, const float* buffer);
	void postSpectrumBufferChanged(int rx, const float* buffer);
	void widebandSpectrumBufferChanged(const float* buffer);
	void rxListChanged(QList<HPSDRReceiver *> rxList);
	void clientConnectedChanged(QObject* sender, bool connect);
	void clientNoConnectedChanged(QObject* sender, int client);
	void audioRxChanged(QObject* sender, int rx);
	void receiverChanged(int value);
	void currentReceiverChanged(int rx);
	void connectedChanged(QObject *sender, bool connect);

	void clientConnectedEvent(int client);
	void clientDisconnectedEvent(int client);
	void rxConnectedStatusChanged(QObject* sender, int rx, bool value);
	void framesPerSecondChanged(QObject* sender, int value);
	void graphicResolutionChanged(QObject* sender, int value); 
	
	void settingsFilenameChanged(QString filename);
	void settingsLoadedChanged(bool loaded);

	void newServerNetworkInterface(QString nicName, QString ipAddress);
	void newHPSDRDeviceNIC(QString nicName, QString ipAddress);
	void serverNICChanged(int);
	void hpsdrDeviceNICChanged(int);
	//void metisCardListChanged(QList<TMetiscard> list);
	void metisCardListChanged(const QList<TNetworkDevicecard> &list);
	void metisChanged(TNetworkDevicecard card);
	void networkDeviceNumberChanged(int value);
	void networkIOComboBoxEntryAdded(QString str);
	void clearNetworkIOComboBoxEntrySignal();
	void searchMetisSignal();
	void serverAddrChanged(QObject *sender, QString addr);
	void hpsdrDeviceLocalAddrChanged(QObject *sender, QString addr);
	void serverPortChanged(QObject *sender, quint16 port);
	void listenPortChanged(QObject *sender, quint16 port);
	void audioPortChanged(QObject *sender, quint16 port);
	void metisPortChanged(QObject *sender, quint16 port);
	void showNetworkIO();

	void callsignChanged(QString callsign );

	void mouseWheelFreqStepChanged(QObject *sender, double value);
	void mainVolumeChanged(QObject *sender, int rx, float volume );

	//void hermesPresenceChanged(bool value);
	void hpsdrHardwareChanged(int value);
	void hermesVersionChanged(int value);
	void mercuryPresenceChanged(bool value);
	void mercuryVersionChanged(int value);
	void penelopePresenceChanged(bool value);
	void penelopeVersionChanged(int value);
	void pennyLanePresenceChanged(bool value);
	void pennyLaneVersionChanged(int value);
	void alexPresenceChanged(bool value);
	void excaliburPresenceChanged(bool value);
	void hwInterfaceVersionChanged(int value);

	void protocolSyncChanged(int value);
	void adcOverflowChanged(int value);
	void sendIQSignalChanged(int value);
	void rcveIQSignalChanged(int value);

	void numberOfRXChanged(QObject *sender, int value);
	void sampleRateChanged(QObject *sender, int value);
	void preampChanged(QObject *sender, int value);
	void ditherChanged(QObject *sender, int value);
	void randomChanged(QObject *sender, int value);
	void src10MhzChanged(QObject *sender, int source);
	void src122_88MhzChanged(QObject *sender, int source);
	void micSourceChanged(QObject *sender, int source);
	void classChanged(QObject *sender, int value);
	void timingChanged(QObject *sender, int value);
	void controlBytesOutChanged(QObject *sender, unsigned char *values);
	void frequencyChanged(QObject* sender, bool value, int rx, long frequency);

	void wideBandStausChanged(bool value);
	void wideBandDataChanged(bool value);

	void iqPortChanged(QObject* sender, int rx, int port);

	void hamBandChanged(QObject *sender, int rx, HamBand band);
	void dspModeChanged(QObject *sender, int rx, DSPMode mode);
	void agcModeChanged(QObject *sender, int rx, AGCMode mode);
	void agcGainChanged(QObject *sender, int rx, int value);
	void filterFrequenciesChanged(QObject *sender, int rx, qreal low, qreal high);
	
	void cudaDevicesChanged(QObject *sender, int value);
	void cudaDriverChanged(QObject *sender, int value);
	void cudaRuntimeChanged(QObject *sender, int value);
	void cudaCurrentDeviceChanged(QObject *sender, int value);
	void cudaLastDeviceChanged(QObject *sender, int value);

	void freqRulerPositionChanged(float position, int rx);
	void wideBandRulerPositionChanged(float position);

	void audioFormatChanged(QObject *sender, const QAudioFormat &format);
	void audioPositionChanged(QObject *sender, qint64 position);
	void audioBufferChanged(QObject *sender, qint64 position, qint64 length, const QByteArray &buffer);

	void chirpSignalModeChanged(QObject *sender);
	void lowerChirpFreqChanged(QObject *sender, int value);
	void upperChirpFreqChanged(QObject *sender, int value);
	void chirpAmplitudeChanged(QObject *sender, qreal value);
	void chirpSamplingFreqChanged(QObject *sender, int value);
	void chirpBufferDurationUsChanged(QObject *sender, qint64 value);
	void chirpRepetitionTimesChanged(QObject *sender, int value);
	void chirpReceiverChanged(QObject *sender, int value);
	void chirpBufferChanged(qint64 length, const QList<qreal> &buffer);
	void chirpAvgLengthChanged(int length);
	void chirpFFTShowChanged(bool value);
	void chirpSidebandChanged(bool value);
	void chirpFilterLowerFrequencyChanged(int value);
	void chirpFilterUpperFrequencyChanged(int value);
	void chirpSpectrumBufferChanged(int sampleRate, qint64 length, const float *buffer);
	void chirpSpectrumChanged(qint64 position, qint64 length, const FrequencySpectrum &spectrum);
	void chirpSpectrumListChanged(const QList<FrequencySpectrum> &spectrumList);

	void displayWidgetHeightChanged(int value);
	void spectrumSizeChanged(int value);
	void panadapterColorChanged();
	void panGridStatusChanged(bool value);

	void spectrumAveragingChanged(bool value);
	void spectrumAveragingCntChanged(int value);

	void waterfallTimeChanged(int rx, int value);
	void waterfallOffesetLoChanged(int rx, int value);
	void waterfallOffesetHiChanged(int rx, int value);

	void sMeterHoldTimeChanged(int value);
	void dBmScaleMinChanged(int rx, qreal value);
	void dBmScaleMaxChanged(int rx, qreal value);
	void dBmScaleWBMinChanged(qreal value);
	void dBmScaleWBMaxChanged(qreal value);

public:
	int 	loadSettings();
	int 	saveSettings();

	QSDR::_ServerMode					getCurrentServerMode()		{ return m_serverMode; }
	QSDR::_HWInterfaceMode				getHWInterface()			{ return m_hwInterface; }
	QSDR::_DataEngineState				getDataEngineState()		{ return m_dataEngineState; }
	QSDRGraphics::_Panadapter			getPanadapterMode()			{ return m_panadapterMode; }
	QSDRGraphics::_WaterfallColorScheme	getWaterfallColorScheme()	{ return m_waterfallColorScheme; }
	//QSDRGraphics::_Colors				getColorItem()				{ return m_colorItem; }

	QString	getServerModeString(QSDR::_ServerMode mode);
	QString	getHWInterfaceModeString(QSDR::_HWInterfaceMode mode);
	QString	getHDataEngineStateString(QSDR::_DataEngineState mode);
	QString	getErrorString(QSDR::_Error err);

	QString getValue1000(double value, int valuePrefix, QString unitBase);
	QString getValue1024(double value, int valuePrefix, QString unitBase);

	bool mainPower()				{ return m_mainPower; }	
	bool defaultSkin()				{ return m_defaultSkin; }

	bool getPBOPresence()			{ return m_pboFound; }
	bool getFBOPresence()			{ return m_fboFound; }

	int getMinimumWidgetWidth()		{ return m_minimumWidgetWidth; }
	int getMinimumGroupBoxWidth()	{ return m_minimumGroupBoxWidth; }
	int getFramesPerSecond()		{ return m_framesPerSecond; }
	int getGraphicResolution()		{ return m_graphicResolution; }
	int getMultiRxView()			{ return m_multiRxView; }
	
	bool settingsLoaded()			{ return m_settingsLoaded; }
	bool connected()				{ return m_connected; }
	bool clientConnected()			{ return m_clientConnected; }

	QString titleStr()				{ return m_titleString; }	
	QString versionStr()			{ return m_versionString; }
	QString getSettingsFilename()	{ return settingsFilename; }
	QString callsign()				{ return m_callsignString; }

	QString getSDRStyle();
	QString getWidgetStyle();
	QString getMessageBoxStyle();
	QString getDialogStyle();
	QString getColorDialogStyle();
	QString getItemStyle();
	QString getLabelStyle();
	QString getSliderLabelStyle();
	QString getTableStyle();
	QString getComboBoxStyle();
	QString getSpinBoxStyle();
	QString getMenuStyle();
	QString getMiniButtonStyle();
	QString getVolSliderStyle();
	QString getSplitterStyle();
	QString getFrameStyle();
	QString getTabWidgetStyle();
	//QString getNewSliderStyle();

	QString getServerAddr()				{ return m_serverAddress; }
	QString getHPSDRDeviceLocalAddr()	{ return m_hpsdrDeviceLocalAddr; }
	quint16 serverPort()				{ return m_serverPort; }
	quint16 listenPort()				{ return m_listenerPort; }
	quint16 audioPort()					{ return m_audioPort; }
	quint16	metisPort()					{ return m_metisPort; }

	TNetworkDevicecard			getCurrentMetisCard()		{ return m_currentMetisCard; }
	QList<TNetworkDevicecard>	getMetisCardsList()			{ return m_metisCards; }
	QList<TReceiver>			getReceiverDataList()		{ return m_receiverDataList; }
	QList<THamBandFrequencies>	getBandFrequencyList()		{ return m_bandList; }
	QList<THamBandText>			getHamBandTextList()		{ return m_bandTextList; }
	QList<TDefaultFilter>		getDefaultFilterList()		{ return m_defaultFilterList; }
	TDefaultFilterMode			getCurrentFilterMode()		{ return m_filterMode; }
	QString						getDSPModeString(int mode);

	int	 getHpsdrNetworkDevices()	{ return m_hpsdrNetworkDevices; }
	int	 getNetworkInterfacesNo()	{ return m_NetworkInterfacesNo; }
	bool getMercuryPresence()		{ return m_mercuryPresence; }
	int	 getMercuryVersion()		{ return m_mercuryVersion; }
	bool getPenelopePresence()		{ return m_penelopePresence; }
	int  getPenelopeVersion()		{ return m_penelopeVersion; }
	bool getPennyLanePresence()		{ return m_pennyLanePresence; }
	int  getPennyLaneVersion()		{ return m_pennyLaneVersion; }
	//bool getHermesPresence()		{ return m_hermesPresence; }
	int  getHermesVersion()			{ return m_hermesVersion; }
	int	 getHPSDRHardware()			{ return m_hpsdrHardware; }
	bool getAlexPresence()			{ return m_alexPresence; }
	bool getExcaliburPresence()		{ return m_excaliburPresence; }
	bool getJanusPresence()			{ return m_janusPresence; }
	bool getMetisPresence()			{ return m_metisPresence; }
	int  getHWInterfaceVersion()	{ return m_hwInterfaceVersion; }

	bool getWideBandStatus()		{ return m_wideBandStatus; }
	bool getWideBandData()			{ return m_wideBandData; }

	bool getPanGridStatus()			{ return m_panGrid; }

	int getMercurySpeed()			{ return m_mercurySpeed; }
	int	getOutputSampleIncrement()	{ return m_outputSampleIncrement; }
	int getNumberOfReceivers()		{ return m_mercuryReceivers; }
	//int	getCurrentReceivers()		{ return m_mercuryReceivers; }
	int getCurrentReceiver()		{ return m_currentReceiver; }
	int getSampleRate()				{ return m_sampleRate; }

	int getMercuryPreamp()			{ return m_mercuryPreamp; }
	int getMercuryDither()			{ return m_mercuryDither; }
	int getMercuryRandom()			{ return m_mercuryRandom; }
	int get10MHzSource()			{ return m_10MHzSource; }
	int get122_8MHzSource()			{ return m_122_8MHzSource; }
	int getMicSource()				{ return m_micSource; }
	int getRxClass()				{ return m_RxClass; }
	int	getRxTiming()				{ return m_RxTiming; }
	
	qreal	getMainVolume(int rx);
	int		getAGCGain(int rx);

	double	getMouseWheelFreqStep() { return m_mouseWheelFreqStep; }
	
	int		getLowerChirpFreq()				{ return m_lowerChirpFreq; }
	int		getUpperChirpFreq()				{ return m_upperChirpFreq; }
	qreal	getChirpAmplitude()				{ return m_chirpAmplitude; }
	int		getChirpSamplingFreq()			{ return m_chirpSamplingFreq; }
	qint64	getChirpBufferDurationUs()		{ return m_chirpBufferDurationUs; }
	qint64	getChirpBufferLength()			{ return m_chirpBufferLength; }
	int		getChirpChannels()				{ return m_chirpChannels; }
	int		getChirpRepetitionTimes()		{ return m_chirpRepetitionTimes; }
	int		getChirpDownSampleRate()		{ return m_chirpDownSampleRate; }
	int		getChirpAvgLength()				{ return m_chirpAvgLength; }
	int		getChirpFilterLowerFrequency()	{ return m_chirpFilterLowerFrequency; }
	int		getChirpFilterUpperFrequency()	{ return m_chirpFilterUpperFrequency; }
	bool	getChirpReceiver()				{ return m_chirpReceiverOn; }
	bool	getChirpFFTShow()				{ return m_showChirpFFT; }
	bool	getChirpSideband()				{ return m_chirpUSB; }
	
	int		getSpectrumSize()			{ return m_spectrumSize; }
	qreal	getdBmWBScaleMin()			{ return m_dBmWBScaleMin; }
	qreal	getdBmWBScaleMax()			{ return m_dBmWBScaleMax; }
	qreal	getdBmDistScaleMin()		{ return m_dBmDistScaleMin; }
	qreal	getdBmDistScaleMax()		{ return m_dBmDistScaleMax; }

	int		getSMeterHoldTime()			{ return m_sMeterHoldTime; }

	qreal	getFilterFrequencyLow()		{ return m_filterFrequencyLow; }
	qreal	getFilterFrequencyHigh()	{ return m_filterFrequencyHigh; }

	QList<QHostAddress>				m_ipAddressesList;
	QList<QNetworkInterface>		m_networkInterfaces;
	
	// audio
	QAudio::Mode mode() const			{ return m_audioMode; }
    QAudio::State state() const			{ return m_audioState; }
	QAudioFormat getAudioFormat() const { return m_format; }

	// colors
	TPanadapterColors getPanadapterColors()	{ return m_panadapterColors; }

	bool getSpectrumAveraging()			{ return m_specAveraging; }
	int getSpectrumAveragingCnt()		{ return m_specAveragingCnt; }

public slots:
	void	setMainPower(QObject *sender, bool power);
	void	setDefaultSkin(QObject *sender, bool value);
	void	setSettingsFilename(QString filename);
	void	setSettingsLoaded(bool loaded);
	void	setCallsign(QString callsign);

	void	setPBOPresence(bool value);
	void	setFBOPresence(bool value);

	void	setMainVolume(QObject *sender, int rx, float volume);
	void	setSystemState(
				QObject *sender, 
				QSDR::_Error err, 
				QSDR::_HWInterfaceMode hwmode, 
				QSDR::_ServerMode mode, 
				QSDR::_DataEngineState state);

	void	setGraphicsState(
				QObject *sender, 
				QSDRGraphics::_Panadapter panMode,
				QSDRGraphics::_WaterfallColorScheme);
				//QSDRGraphics::_Colors colorItem);

	void setMultiRxView(int view);
	void setSMeterValue(int rx, float value);
	void setSpectrumBuffer(int rx, const float*);
	void setPostSpectrumBuffer(int rx, const float*);
	void setWidebandSpectrumBuffer(const float*);
	void setRxList(QList<HPSDRReceiver *> rxList);
	void setMetisCardList(QList<TNetworkDevicecard> list);
	void searchHpsdrNetworkDevices();
	void clearMetisCardList();
	void setHPSDRDeviceNumber(int value);
	void setCurrentMetisCard(TNetworkDevicecard card);
	void addNetworkIOComboBoxEntry(QString str);
	void clearNetworkIOComboBoxEntry();
	void addServerNetworkInterface(QString nicName, QString ipAddress);
	void addHPSDRDeviceNIC(QString nicName, QString ipAddress);
	void setNumberOfNetworkInterfaces(int value);
	void setServerNetworkInterface(int index);
	void setHPSDRDeviceNIC(int index);
	void setServerWidgetNIC(int index);
	void setHPSDRWidgetNIC(int index);
	void setServerAddr(QObject *sender, QString addr);
	void setHPSDRDeviceLocalAddr(QObject *sender, QString addr);
	void setServerPort(QObject *sender, quint16 port);
	void setListenPort(QObject *sender, quint16 port);
	void setAudioPort(QObject *sender, quint16 port);
	void setMetisPort(QObject *sender, quint16 port);
	void setClientConnected(QObject *sender, bool value);
	void setClientNoConnected(QObject* sender, int client);
	void setRxConnectedStatus(QObject* sender, int rx, bool value);
	void setAudioRx(QObject* sender, int rx);
	void setConnected(QObject *sender, bool value);

	//void setHermesPresence(bool value);
	void setHermesVersion(int value);
	void setHPSDRHardware(int value);
	void setMercuryPresence(bool value);
	void setMercuryVersion(int value);
	void setPenelopePresence(bool value);
	void setPenelopeVersion(int value);
	void setPennyLanePresence(bool value);
	void setPennyLaneVersion(int value);
	void setAlexPresence(bool value);
	void setExcaliburPresence(bool value);
	void setHWInterfaceVersion(int value);

	void setIQPort(QObject *sender, int rx, int port);

	void setProtocolSync(int value);
	void setADCOverflow(int value);
	void setSendIQ(int value);
	void setRcveIQ(int value);

	void setReceivers(QObject *sender, int value);
	void setReceiver(QObject *sender, int value);
	void setCurrentReceiver(int value);
	void setSampleRate(QObject *sender, int value);
	void setPreamp(QObject *sender, int value);
	void setDither(QObject *sender, int value);
	void setRandom(QObject *sender, int value);
	void set10MhzSource(QObject *sender, int source);
	void set122_88MhzSource(QObject *sender, int source);
	void setMicSource(QObject *sender, int source);
	void setClass(QObject *sender, int value);
	void setTiming(QObject *sender, int value);
	void setFrequency(QObject *sender, bool value, int rx, long frequency);
	void setFrequency(int rx, long frequency);
	void clientDisconnected(int client);
	void setFramesPerSecond(QObject *sender, int value);
	void setMouseWheelFreqStep(QObject *sender, double value);

	void setSpectrumSize(int value);
	void setdBmPanScaleMin(int rx, qreal value);
	void setdBmPanScaleMax(int rx, qreal value);

	void setdBmWBScaleMin(qreal value);
	void setdBmWBScaleMax(qreal value);
	void setdBmDistScaleMin(qreal value);
	void setdBmDistScaleMax(qreal value);

	void setWideBandStatus(bool value);
	void setWideBandData(bool value);

	void setHamBand(QObject *sender, int rx, HamBand band);
	void setDSPMode(QObject *sender, int rx, DSPMode mode);
	void setAGCMode(QObject *sender, int rx, AGCMode mode);
	void setAGCGain(QObject *sender, int rx, int value);
	void setRXFilter(QObject *sender, int rx, qreal low, qreal high);
	
	void setOpenCLDevices(QList<QCLDevice>	dev);

	/*void setCudaPresence(bool value);
	void setCudaDevices(int value);
	void setCudaLastDevice(QObject *sender, int vlaue);
	void setCudaDriver(QObject *sender, int value);
	void setCudaRuntime(QObject *sender, int value);
	void setCurrentCudaDevice(QObject *sender, int value);*/

	void setFreqRulerPosition(float pos, int rx);
	//void setRulerPosition(QObject *sender, float pos);
	void setWideBandRulerPosition(float pos);

	void setAudioFormat(QObject *sender, const QAudioFormat &format);
	void setAudioPosition(QObject *sender, qint64 position);
	void setAudioBuffer(QObject *sender, qint64 position, qint64 length, const QByteArray &buffer);

	void switchToChirpSignalMode(QObject *sender);
	void setLowerChirpFreq(int value);
	void setUpperChirpFreq(int value);
	void setChirpAmplitude(qreal value);
	void setChirpSamplingFreq(int value);
	void setChirpBufferDurationUs(int value);
	void setChirpRepetitionTimes(int value);
	void setChirpReceiver(bool value);
	void setChirpAvgLength(int value);
	void setChirpFFTShow(bool value);
	void setChirpUSB(bool value);
	void setChirpFilterLowerFrequency(int value);
	void setChirpFilterUpperFrequency(int value);
	//void setChirpDownSampleRate(int value);
	//void setChirpBufferLength(qint64 length);
	void setChirpBuffer(qint64 length, const QList<qreal> &buffer);
	//void setChirpSpectrumBuffer(const QList<qreal> &buffer);
	void setChirpSpectrumBuffer(int sampleRate, qint64 length, const float *buffer);

	//void setSpectrumBuffer(const float *buffer);
	void setChirpSpectrum(qint64 position, qint64 length, const FrequencySpectrum &spectrum);
	void setChirpSpectrumList(const QList<FrequencySpectrum> &spectrumList);

	void moveDisplayWidget(int value);

	void setPanadapterColors(TPanadapterColors type);
	void setPanGrid(bool value);
	void setFramesPerSecond(int value);
	void setGraphicResolution(int value);

	void setSpectrumAveraging(bool value);
	void setSpectrumAveragingCnt(int value);

	void setWaterfallTime(int rx, int value);
	void setWaterfallOffesetLo(int rx, int value);
	void setWaterfallOffesetHi(int rx, int value);

	void setSMeterHoldTime(int value);

	void showNetworkIODialog();

	QList<long> getFrequencies();
	
private:
	QMutex mutex;

	QString			m_titleString;
	QString			m_versionString;
	QString			m_serverAddress;
	QString			m_hpsdrDeviceLocalAddr;
	QString			m_callsignString;
	QString			settingsFilename;

	QDateTime		startTime;
	QDateTime		now;
		
	QHostAddress	m_hostAddress;

	quint16			m_serverPort;
	quint16			m_listenerPort;
	quint16			m_audioPort;
	quint16			m_metisPort;

	QAudio::Mode	m_audioMode;
    QAudio::State	m_audioState;
	QAudioFormat    m_format;

	QSDR::_Error				m_systemError;
	QSDR::_ServerMode			m_serverMode;
	QSDR::_HWInterfaceMode		m_hwInterface;
	QSDR::_DataEngineState		m_dataEngineState;

	QSDRGraphics::_Panadapter				m_panadapterMode;
	QSDRGraphics::_WaterfallColorScheme		m_waterfallColorScheme;
	//QSDRGraphics::_Colors			m_colorItem;

	TNetworkDevicecard			m_currentMetisCard;
	QList<TNetworkDevicecard>	m_metisCards;
	QList<TReceiver>			m_receiverDataList;
	QList<THamBandFrequencies>	m_bandList;
	QList<THamBandText>			m_bandTextList;
	QList<TDefaultFilter>		m_defaultFilterList;
	QList<QString>				m_rxStringList;

	TDefaultFilterMode			m_filterMode;
	TPanadapterColors			m_panadapterColors;

	QList<QCLDevice>			m_clDevices;

	bool	m_mainPower;
	bool	m_defaultSkin;
	bool	m_settingsLoaded;
	bool	m_connected;
	bool	m_clientConnected;
	bool	m_wideBandStatus;
	bool	m_wideBandData;
	bool	m_pboFound;
	bool	m_fboFound;

	bool	m_mercuryPresence;
	bool	m_penelopePresence;
	bool	m_pennyLanePresence;
	bool	m_alexPresence;
	bool	m_excaliburPresence;
	bool	m_janusPresence;
	bool	m_metisPresence;
	
	//bool	main_mute;
	bool	m_specAveraging;
	bool	m_panGrid;

	int		m_hpsdrHardware;
	int		m_hpsdrNetworkDevices;
	int		m_NetworkInterfacesNo;
	int		m_mercuryVersion;
	int		m_penelopeVersion;
	int		m_pennyLaneVersion;
	int		m_hermesVersion;
	int		m_hwInterfaceVersion;

	int		m_clientNoConnected;
	int		m_minimumWidgetWidth;
	int		m_minimumGroupBoxWidth;

	int		m_mercuryReceivers;
	int		m_currentReceiver;
	int		m_sampleRate;
	int		m_mercurySpeed;

	int		m_mercuryPreamp;
	int		m_mercuryDither;
	int		m_mercuryRandom;

	int		m_outputSampleIncrement;
	int		m_10MHzSource;
	int		m_122_8MHzSource;
	int		m_micSource;
	int		m_RxClass;
	int		m_RxTiming;

	int		m_framesPerSecond;
	int		m_graphicResolution;
	int		m_multiRxView;

	int		m_spectrumSize;
	int		m_specAveragingCnt;
	int		m_sMeterHoldTime;

	double	m_mouseWheelFreqStep;

	long freq1;
	
	float m_mainVolume;

	int control_register;

	bool connect_at_startup;

	qreal	m_dBmWBScaleMin;
	qreal	m_dBmWBScaleMax;
	qreal	m_dBmDistScaleMin;
	qreal	m_dBmDistScaleMax;

	qreal	m_filterFrequencyLow;
	qreal	m_filterFrequencyHigh;

	int		m_lowerChirpFreq;
	int		m_upperChirpFreq;
	qreal	m_chirpAmplitude;
	int		m_chirpSamplingFreq;
	qint64	m_chirpBufferDurationUs;
	qint64	m_chirpBufferLength;
	int		m_chirpChannels;
	int		m_chirpRepetitionTimes;
	int		m_chirpDownSampleRate;
	int		m_chirpAvgLength;
	int		m_chirpFilterLowerFrequency;
	int		m_chirpFilterUpperFrequency;
	bool	m_chirpReceiverOn;
	bool	m_showChirpFFT;
	bool	m_chirpUSB;

	/*bool	m_cudaPresence;
	int		m_cuda_devices;
	int		m_cudaLastDevice;
	int		m_cuda_driver_version;
	int		m_cuda_runtime_version;
	int		m_current_cuda_device;*/
};

//******************************************************
// Macros

/****************************************************************************
**
** Copyright (C) 2011 Nokia Corporation and/or its subsidiary(-ies).
** All rights reserved.
** Contact: Nokia Corporation (qt-info@nokia.com)
**
** The following Macro "CHECKED_CONNECT" is part of the examples of the Qt Toolkit.
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
// Macro which connects a signal to a slot, and which causes application to
// abort if the connection fails.  This is intended to catch programming errors
// such as mis-typing a signal or slot name.  It is necessary to write our own
// macro to do this - the following idiom
//     Q_ASSERT(connect(source, signal, receiver, slot));
// will not work because Q_ASSERT compiles to a no-op in release builds.

#define CHECKED_CONNECT(source, signal, receiver, slot) \
    if(!connect(source, signal, receiver, slot)) \
        qt_assert_x(Q_FUNC_INFO, "CHECKED_CONNECT failed", __FILE__, __LINE__);

#define CHECKED_CONNECT_OPT(source, signal, receiver, slot, opt) \
    if(!connect(source, signal, receiver, slot, opt)) \
        qt_assert_x(Q_FUNC_INFO, "CHECKED_CONNECT failed", __FILE__, __LINE__);


//******************************************************
// Debug output

class NullDebug {

public:
    template <typename T>
    NullDebug& operator<<(const T&) { return *this; }
};

inline NullDebug nullDebug() { return NullDebug(); }


#ifdef LOG_SETTINGS
#   define SETTINGS_DEBUG qDebug().nospace() << "Settings::\t"
#else
#   define SETTINGS_DEBUG nullDebug()
#endif

#ifdef LOG_MAIN
#   define MAIN_DEBUG qDebug().nospace() << "Main::\t"
#else
#   define MAIN_DEBUG nullDebug()
#endif

#ifdef LOG_SERVER
#   define SERVER_DEBUG qDebug().nospace() << "Server::\t"
#else
#   define SERVER_DEBUG nullDebug()
#endif

#ifdef LOG_DATA_ENGINE
#   define DATA_ENGINE_DEBUG qDebug().nospace() << "DataEngine::\t"
#else
#   define DATA_ENGINE_DEBUG nullDebug()
#endif

#ifdef LOG_DATA_RECEIVER
#   define DATA_RECEIVER_DEBUG qDebug().nospace() << "DataReceiver::\t"
#else
#   define DATA_RECEIVER_DEBUG nullDebug()
#endif

#ifdef LOG_DATA_PROCESSOR
#   define DATA_PROCESSOR_DEBUG qDebug().nospace() << "DataProcessor::\t"
#else
#   define DATA_PROCESSOR_DEBUG nullDebug()
#endif

#ifdef LOG_NETWORKDIALOG
#   define NETWORKDIALOG_DEBUG qDebug().nospace() << "NetworkDialog::\t"
#else
#   define NETWORKDIALOG_DEBUG nullDebug()
#endif

#ifdef LOG_HPSDRIO
#   define HPSDRIO_DEBUG qDebug().nospace() << "HPSDR IO::\t"
#else
#   define HPSDRIO_DEBUG nullDebug()
#endif

#ifdef LOG_AUDIO_ENGINE
#   define AUDIO_ENGINE_DEBUG qDebug().nospace() << "AudioEngine::\t"
#else
#   define AUDIO_ENGINE_DEBUG nullDebug()
#endif

#ifdef LOG_AUDIO_WAVFILE
#   define AUDIO_WAVFILE_DEBUG qDebug().nospace() << "AudioWavFile::\t"
#else
#   define AUDIO_WAVFILE_DEBUG nullDebug()
#endif

#ifdef LOG_AUDIO_RECEIVER
#   define AUDIO_RECEIVER qDebug().nospace() << "AudioReceiver::\t"
#else
#   define AUDIO_RECEIVER nullDebug()
#endif

#ifdef LOG_AUDIO_PROCESSOR
#   define AUDIO_PROCESSOR qDebug().nospace() << "AudioProcessor::\t"
#else
#   define AUDIO_PROCESSOR nullDebug()
#endif

#ifdef LOG_CHIRP_PROCESSOR
#   define CHIRP_PROCESSOR_DEBUG qDebug().nospace() << "ChirpProcessor::\t"
#else
#   define CHIRP_PROCESSOR_DEBUG nullDebug()
#endif

#ifdef LOG_DTTSP_API
#   define DTTSP_API_DEBUG qDebug().nospace() << "DttSP API::\t"
#else
#   define DTTSP_API_DEBUG nullDebug()
#endif

#ifdef LOG_SPECTRUMANALYSER
#   define SPECTRUMANALYSER_DEBUG qDebug().nospace() << "SpectrumAnalyzer::\t"
#else
#   define SPECTRUMANALYSER_DEBUG nullDebug()
#endif

#ifdef LOG_WAVEFORM
#   define WAVEFORM_DEBUG qDebug().nospace() << "WaveForm::\t"
#else
#   define WAVEFORM_DEBUG nullDebug()
#endif

#ifdef LOG_PAINT_EVENT
#   define WAVEFORM_PAINT_DEBUG qDebug().nospace() << "WaveFormPaint::\t"
#else
#   define WAVEFORM_PAINT_DEBUG nullDebug()
#endif

#ifdef LOG_GRAPHICS
#   define GRAPHICS_DEBUG qDebug().nospace() << "Graphics::\t"
#else
#   define GRAPHICS_DEBUG nullDebug()
#endif

#ifdef LOG_WBGRAPHICS
#   define WBGRAPHICS_DEBUG qDebug().nospace() << "WB-Graphics::\t"
#else
#   define WBGRAPHICS_DEBUG nullDebug()
#endif

#ifdef LOG_DISPLAYPANEL
#   define DISPLAYPANEL_DEBUG qDebug().nospace() << "DisplayPanel::\t"
#else
#   define DISPLAYPANEL_DEBUG nullDebug()
#endif

#ifdef LOG_CUDAGRAPH
#   define CUDAGRAPH_DEBUG qDebug().nospace() << "CudaGraph::\t"
#else
#   define CUDAGRAPH_DEBUG nullDebug()
#endif

#ifdef LOG_SERVER_WIDGET
#   define SERVER_WIDGET_DEBUG qDebug().nospace() << "ServerWidget::\t"
#else
#   define SERVER_WIDGET_DEBUG nullDebug()
#endif

#ifdef LOG_CHIRP_WIDGET
#   define CHIRP_WIDGET_DEBUG qDebug().nospace() << "ChirpWidget::\t"
#else
#   define CHIRP_WIDGET_DEBUG nullDebug()
#endif

#ifdef LOG_RECEIVER
#   define RECEIVER_DEBUG qDebug().nospace() << "Receiver::\t"
#else
#   define RECEIVER_DEBUG nullDebug()
#endif

#ifdef LOG_FUTUREWATCHER
#   define FUTUREWATCHER_DEBUG qDebug().nospace() << "FutureWatcher::\t"
#else
#   define FUTUREWATCHER_DEBUG nullDebug()
#endif

#ifdef LOG_ALEX_WIDGET
#   define ALEX_WIDGET_DEBUG qDebug().nospace() << "AlexWidget::\t"
#else
#   define ALEX_WIDGET_DEBUG nullDebug()
#endif

//******************************************************
// sleeper function

class SleeperThread : public QThread {
	
	public:
		static void msleep(unsigned long msecs) {QThread::msleep(msecs);}
		static void usleep(unsigned long usecs) {QThread::usleep(usecs);}
};

#endif  // CUSDR_SETTINGS_H
