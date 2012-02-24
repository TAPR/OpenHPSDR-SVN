// PSKCore.h: interface for the CPSKCore class.
//
//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
// PSK31Core Library for transmission and reception of PSK31 signals
//    using a PC soundcard  or .wav files.
//	               Copyright 2000, Moe Wheatley, AE4JY
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// http://www.gnu.org/copyleft/lesser.html
//////////////////////////////////////////////////////////////////////
//

#if !defined(AFX_IOCNTRL_H__ED8BF853_5C8C_11D3_A248_00A0C996E7F5__INCLUDED_)
#define AFX_IOCNTRL_H__ED8BF853_5C8C_11D3_A248_00A0C996E7F5__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "types.h"

#include <QtNetwork/QTcpSocket>
#include <QtNetwork/QUdpSocket>
#include <QMutex>

#include "fft.h"
#include "PSKMod.h"
#include "PskDet.h"
#include "PSKCore.h"

#define BUF_SIZE 2048	//size of data chunks to process at a time from
						// the soundcard

#define SAMP_RATE 8000  //soundcard samplerate^M
#define MAX_CHANNELS 50
#define TX_BUF_SIZE 32000


#define DLLSTAT_OFF -1			//DLL status/error codes
#define DLLSTAT_RX 0
#define DLLSTAT_TX 1
#define DLLSTAT_CPUSLOW 2
#define DLLSTAT_TXFINISHING 3
#define DLLSTAT_INWAVEDONE 4
#define DLLSTAT_OUTWAVEDONE 5
#define DLLSTAT_INWAVEFILESTATUS 6
#define DLLSTAT_OUTWAVEFILESTATUS 7

#define DLLERR_NONE 0 
#define DLLERR_MEMORY 10 

#define DLLERR_IN_NOTOPEN 11
#define DLLERR_IN_OVERFLOW 12
#define DLLERR_IN_TIMEOUT 13
#define DLLERR_OUT_NOTOPEN 14
#define DLLERR_OUT_UNDERFLOW 15
#define DLLERR_OUT_TIMEOUT 16

#define DLLERR_WAV_BADFORMAT 17
#define DLLERR_WAV_STILLPLAYING 18
#define DLLERR_WAV_UNPREPARED 19
#define DLLERR_WAV_SYNC 20

#define DLLERR_SYS_BADDEVICEID 21
#define DLLERR_SYS_NOTENABLED 22
#define DLLERR_SYS_ALLOCATED 23
#define DLLERR_SYS_INVALHANDLE 24
#define DLLERR_SYS_NODRIVER 25
#define DLLERR_SYS_NOTSUPPORTED 26
#define DLLERR_SYS_BADERRNUM 27
#define DLLERR_SYS_INVALFLAG 28 
#define DLLERR_SYS_INVALPARAM 29
#define DLLERR_SYS_HANDLEBUSY 30
#define DLLERR_SYS_REGISTRY 31
#define DLLERR_SYS_NODRIVERCB 32

#define DLLERR_UNKNOWN 33

// input wave file error codes
#define DLLERR_WAVIN_OPEN 34			// can't open wave file for input
#define DLLERR_WAVIN_NOTWAVE 35			// file is not a RIFF wave type
#define DLLERR_WAVIN_INVALID 36			// Invalid wave file
#define DLLERR_WAVIN_NODATA 37			// no data in file
#define DLLERR_WAVIN_NOTSUPPORTED 38	// not a supported data type
#define DLLERR_WAVIN_READING 39			// Error reading data from file
#define DLLERR_WAVIN_NOTOPEN 40			// tried to read and file is not open

// output wave file error codes
#define DLLERR_WAVOUT_OPEN 41			// can't open wave file for output
#define DLLERR_WAVOUT_WRITING 42		// error writing to wave file
#define DLLERR_WAVOUT_NOTOPEN 43		// tried to write and file is not open


// IO modes
#define IOM_NORMAL 0		//Use soundcard only
#define IOM_WAVEIN (1<<0)	//1==use input wave file, 0==use soundcard
#define IOM_WAVELOG (1<<1)	//1==copy soundcard in to outwave file, 0==don't
#define IOM_WAVETX (1<<2)	//1==Save Tx to outwave file, 0==don't
#define IOM_NOTXSOUND (1<<3)//1==No Tx to soundcard, 0==Tx Output to soundcard
#define IOM_ECHOINPUT (1<<4)//1==Echo input to SoundcarOut 0== No Echo
#define IOM_UDP (1<<5)      //1==Audio input from QtDSPServer

#include <QObject>

class CPSKCore : public QObject
{
    Q_OBJECT

public:

	CPSKCore();
    virtual ~CPSKCore();


//  Initialization/Shutdown Functions
    BOOL fnInitPSKLib(void);
    void fnTermPSKLib(void);

//  Initialization/Shutdown Functions
    long   fnStartSoundCard(/*HWND h_Wnd,*/ long cardnum, long numRXchannels);
    void   fnStopSoundCard(void);

//  RX Functions
    void   fnSetRXFrequency(long freq, long CaptureRange, long chan);
    void   fnSetRXPSKMode (long mode, long chan);
    long   fnGetRXFrequency(long chan);
    void   fnSetFFTMode(long ave, long maxscale, long log);
    long   fnGetFFTData(quint32* datarray, quint32 start, quint32 end);
    long   fnGetClosestPeak( long Startf, long Stopf);
    void   fnGetVectorData(qint32* vectorarray, long chan);
    void   fnGetSyncData(long* syncarray, long chan);
    long   fnGetRawData(quint32* array, quint32 start, quint32 end);
    void   fnSetAFCLimit(long limit, long chan);
    void   fnSetSquelchThreshold(long thresh, long mode, long chan);
    long   fnGetSignalLevel(long chan);
//  TX Functions
    void   fnStartTX(long mode);
    void   fnStopTX();
    void   fnAbortTX();
    void   fnSetTXFrequency(long freq);
    void   fnSetCWIDString(char* lpszIDstrg);
    long   fnSendTXCharacter( long txchar, bool cntrl);
    long   fnSendTXString(char* lpszTXStrg);
    long   fnGetTXCharsRemaining();
    void   fnClearTXBuffer();
    void   fnSetCWIDSpeed (long speed );
    long   fnSetComPort (long portnum, long mode );

//  MISC Functions
    void   fnSetClockErrorAdjustment (long ppm);
    long   fnGetDLLVersion ();
    void   fnGetErrorString(char* lpszError);
    long   fnGetDebugData(double* val1, double* val2);

//  New Functions added after the initial release
    void   fnRewindInput (long Blocks );
    long   fnSetInputWavePath(char* path, long* pLengthTime, long Offset);
    long   fnSetOutputWavePath(char* path, long TimeLimit, long Append);
    long   fnStartSoundCardEx(/*HWND h_Wnd,*/ long cardnum, long numRXchannels, long IOMode);
    long   fnEnableRXChannel(long chan, long enable);
    long   fnIsRXChannelActive(long chan);
    long   fnGetNumActiveRXChannels();

// New Functions for HPSDR
    void fnSetConnection(QString host,int remoteport,int localport);
    void fnSetBand(int band,long frequency);
    void sendCommand(QString command);

signals:
    void statusChange(int);
    void dataReady(int,int);
    void pskCharReady(int,int);

public slots:
    void connected();
    void tcpReadyRead();
    void udpReadyRead();
    void tcpSocketError(QAbstractSocket::SocketError);
    void udpSocketError(QAbstractSocket::SocketError);
    void slotPskCharReady(int,int);

private:
	double m_ClockError;

//public Exposed Functions to CPSKCoreApp
	void RewindInput( INT Blocks);
	void GetErrorMsg( CString& err);
	void SetClockErrorAdjustment(INT adj);
	void SetRXFrequency(long freq, long range, long chan);
	void SetFFTParams( INT ave, double gain, INT type);
	void StartTX();
	void StopTX();
	void AbortTX();
	void StopIO();
	BOOL IsRxChannelActive(long chan);
	BOOL m_SatMode;
	BOOL ChkDetExists( INT chan );
	BOOL GetFFTData(LONG* pData, LONG start, LONG end);
	BOOL GetRawData(LONG* pData, LONG start, LONG end);
    LONG SetComPort( long portnum, long mode);
	INT GetNumActiveRXChannels();
    BOOL EnableRXChannel(INT chan, BOOL enable);
    INT SetInputWavePath( CString sPath, PINT pLengthTime, INT Offset);
    INT SetOutputWavePath(CString sPath, INT TimeLimit, INT Append);
    INT StartIO(/*HWND h_Wnd,*/ INT maxchannels, INT IOMode);

	void ProcessLoop();		//called externally by worker thread

//Publicly exposed Variables
	CPSKMod m_PSKMod;
	CPSKDet* m_pPSKDet[MAX_CHANNELS];
	Cfft m_Cfft;
	long m_MaxNumRXChannels;
	long m_DLLRevision;
    INT m_InputCardNum;
    INT m_OutputCardNum;   //AA6YQ 1.20
	BOOL m_RXState;

//Functions
	void ProcessRX();
	void ProcessTX();
	void StatusChange(INT code);
	void DeleteResources();
	void StopThread( );
	void ProcessError( INT ErrorCode );
	void AddGaussian(double * pData, INT n, double std);
	void PTTCntrl( BOOL Ptton );

    void connect();

//Variables
	BOOL m_AllChannelsActive;
	BOOL m_TXRunning;
	BOOL m_WaveOutAppend;
    BOOL m_fProcThreadQuit;			// signals to tnsigned shorthread to quit
	BOOL m_ThreadActive;
	INT m_InSampleLimit;
    INT m_OutSampleLimit;
	INT m_InTotalSamples;
    INT m_OutTotalSamples;
	INT m_TxMode;
    INT m_DLLStatus;
	INT m_IOMode;
	double m_pBuffer[BUF_SIZE];
	CString m_ErrorMessage;
    CString m_InWavePath;
    CString m_OutWavePath;
	CRITICAL_SECTION m_CriticalSection;	//for keeping threads from stomping on each other

    QString host;
    int localPort;
    int remotePort;
    QTcpSocket* tcpSocket;
    QMutex tcpMutex;
    QUdpSocket* udpSocket;
    int m_pBufferIndex;
    char udpBuffer[8+((8000*1*2)/100)]; // (sampleRate*audio_channels*2)/50

    bool hasConnection;
    int band;
    long frequency;

    qint64 sequence;

};

#endif // !defined(AFX_IOCNTRL_H__ED8BF853_5C8C_11D3_A248_00A0C996E7F5__INCLUDED_)
