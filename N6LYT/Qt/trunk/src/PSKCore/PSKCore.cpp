//////////////////////////////////////////////////////////////////////
// PSKCore.cpp: implementation of the CPSKCore class.
//
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

#include <float.h>

#include "ErrorCodes.h"
#include "PSKCore.h"

#include <QDebug>

#include <math.h>

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

#define MAXTIMER 50		//wait for 5 seconds for thread to start

#define PERCENTMODE_OFF 0
#define PERCENTMODE_IN 1
#define PERCENTMODE_OUT 2

double DebugFP1=0.0;
double DebugFP2=0.0;
INT DebugINT = 0;
//#define USE_PERFORMANCE 1		//displays performance info
//#define USE_NOISEGEN 1		//internal additive noise


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CPSKCore::CPSKCore()
{
	m_DLLRevision = 121;	//DLL revision number.(100 = 1.00) AA6YQ
	m_fProcThreadQuit = FALSE;
	m_RXState = TRUE;
	m_DLLStatus = DLLSTAT_OFF;
	for(INT i=0; i<MAX_CHANNELS; i++)
		m_pPSKDet[i] = NULL;
    //m_pProcThread = NULL;
	m_TxMode = BPSK_MODE;
    //m_hComPort = INVALID_HANDLE_VALUE;
    //m_PTTPort = 0;
    //m_ComPortMode = 0;
    //m_InputCardNum = -1;
    //m_OutputCardNum = -1;  //AA6YQ 1.20
	m_MaxNumRXChannels = MAX_CHANNELS;
	m_AllChannelsActive = TRUE;
	m_ErrorMessage = _T("");
    //m_InWavePath = _T("");
    //m_OutWavePath = _T("");
	m_ThreadActive = FALSE;
    //m_hWnd = NULL;
	m_SatMode = FALSE;
	m_TXRunning = FALSE;
    m_IOMode = IOM_UDP;
    //m_InFilePercentage = 0;
    //m_OutFilePercentage = 0;
	m_InTotalSamples = 0;
	m_OutTotalSamples = 0;
    //m_WaveOutLimit = 0;
	m_ClockError = 1.0;
// Setup Soundcard values
    //m_WaveFormatEx.wFormatTag = WAVE_FORMAT_PCM;
    //m_WaveFormatEx.nChannels = 1;
    //m_WaveFormatEx.wBitsPerSample = 16;
    //m_WaveFormatEx.nSamplesPerSec = SAMP_RATE;
    //m_WaveFormatEx.nBlockAlign =
    //		m_WaveFormatEx.nChannels *(m_WaveFormatEx.wBitsPerSample/8);
    //m_WaveFormatEx.nAvgBytesPerSec =
    //		m_WaveFormatEx.nSamplesPerSec *
    //		m_WaveFormatEx.nBlockAlign;
    //m_WaveFormatEx.cbSize = 0;
    InitializeCriticalSection(/*&*/m_CriticalSection);
    m_PSKMod.InitPSKMod(SAMP_RATE, /*NULL,*/FALSE);

    hasConnection=FALSE;
    band=5;
    frequency=14070000;
}

CPSKCore::~CPSKCore()
{
	StopIO();
    /*
	if( m_hComPort != INVALID_HANDLE_VALUE )
	{
		EscapeCommFunction(m_hComPort, CLRRTS);
		EscapeCommFunction(m_hComPort, CLRDTR);
		CloseHandle( m_hComPort );
		m_hComPort = INVALID_HANDLE_VALUE;
	}
    */
	DeleteResources();
	for(INT i=0; i<MAX_CHANNELS; i++)
	{
		if(m_pPSKDet[i])
		{
			delete m_pPSKDet[i];
			m_pPSKDet[i] = NULL;
		}
	}
	DeleteCriticalSection(&m_CriticalSection);
}

/////////////////////////////////////////////////////////////////
// Control functions
/////////////////////////////////////////////////////////////////
long  CPSKCore::fnStartSoundCard(/*HWND h_Wnd,*/ long cardnum,
													long numRXchannels)
{
long error = 0;

        m_InputCardNum = cardnum;
        m_OutputCardNum = cardnum;  //AA6YQ 1.20
        error =  StartIO( (INT)numRXchannels, 0);

	return error;
}

long  CPSKCore::fnStartSoundCardEx(/*HWND h_Wnd,*/ long cardnum,
													long numRXchannels, long IOMode)
{
long error = 0;

        m_InputCardNum = cardnum;
        m_OutputCardNum = cardnum;  //AA6YQ 1.20
        error =  StartIO( (INT)numRXchannels,IOMode);

	return error;
}


//added by AA6YQ 1.20
#ifdef NOT_INCLUDED
long  CPSKCore::fnStartRXTXSoundCard(/*HWND h_Wnd,*/ long RXcardnum,long TXcardnum,
													long numRXchannels)
{
long error = 0;

        m_InputCardNum = RXcardnum;
        m_OutputCardNum = TXcardnum;
        error =  StartIO( (INT)numRXchannels, 0);

	return error;
}
#endif

void  CPSKCore::fnStopSoundCard(void)
{
        StopIO();
}

/////////////////////////////////////////////////////////////////
// RX Functions
/////////////////////////////////////////////////////////////////
void  CPSKCore::fnSetRXFrequency(long freq,
										long CaptureRange, long chan)
{
	if(freq>3500)
		freq = 3500;
	if(freq<100)
		freq = 100;
		if( ChkDetExists(chan) )
			SetRXFrequency( freq, CaptureRange, chan );
}

long  CPSKCore::fnEnableRXChannel(long chan, long enable)
{
long error = DLLERR_MEMORY;
		error = EnableRXChannel( chan, enable );
	return error;
}

long  CPSKCore::fnIsRXChannelActive(long chan)
{
BOOL ret = FALSE;
		ret = IsRxChannelActive(chan);
	return ret;
}

long  CPSKCore::fnGetNumActiveRXChannels()
{
int ret = 0;
		ret = GetNumActiveRXChannels();
	return ret;
}

void  CPSKCore::fnSetRXPSKMode (long mode, long chan)
{
		if( ChkDetExists(chan) )
			m_pPSKDet[chan]->SetRXPSKMode(mode&0x001F);  //AA6YQ 1.21 need 5 bits of mode
}

long  CPSKCore::fnGetRXFrequency(long chan)
{
INT f = 1000;
		if( IsRxChannelActive(chan) )
			f = m_pPSKDet[chan]->GetRXFrequency();
	if(f>3500)
		f = 3500;
	if(f<100)
		f = 100;
	return f;
}

void  CPSKCore::fnSetFFTMode(long ave, long maxscale, long type)
{
	if(ave>10)
		ave = 10;
	if(ave<1)
		ave = 1;
	if(type>99)
		type = 99;
	if(type<0)
		type = 0;
		SetFFTParams(  ave,( double)maxscale*0.10, type );
}

long  CPSKCore::fnGetFFTData(quint32* array, quint32 start, quint32 end)
{
long result = 0;
	if(start>1022)
		start = 1022;
	if(start<0)
		start = 0;
	if(end>1023)
		end = 1023;
	if(end<1)
		end = 1;
		result = GetFFTData(array, start, end);
	return result;
}

long  CPSKCore::fnGetClosestPeak(long Startf, long Stopf)
{
LONG freq = 1500;
		freq = m_Cfft.GetClosestPeak( Startf, Stopf);
	return freq;
}

void  CPSKCore::fnGetVectorData(qint32* vectorarray, long chan)
{
		if( IsRxChannelActive(chan) )
		{
			if(m_RXState || m_SatMode)
			{
				if(m_pPSKDet[chan])
					m_pPSKDet[chan]->GetVectorData(vectorarray);
			}
			else
			{
					m_PSKMod.GetVectorData(vectorarray);
			}
		}
		else
		{
			for(INT i=0; i<16; i++)
				vectorarray[i] = 0;
		}
}

void  CPSKCore::fnGetSyncData(long* syncarray, long chan)
{
		if( IsRxChannelActive(chan) )
		{
			m_pPSKDet[chan]->GetSyncData(syncarray);
		}
		else
		{
			for(INT i=0; i<16; i++)
				syncarray[i] = 0;
		}
}

long  CPSKCore::fnGetRawData(quint32* array, quint32 start, quint32 end)
{
LONG result = 0;
	if(start < end)
	{
		if(start>2046)
			start = 2046;
		if(start<0)
			start = 0;
		if(end>2047)
			end = 2047;
		if(end<1)
			end = 1;
			result = GetRawData(array, start, end);
	}
	return result;
}

void  CPSKCore::fnSetAFCLimit(long limit, long chan)
{
		if( ChkDetExists(chan) && (limit>=0) && (limit<=3500) )
			m_pPSKDet[chan]->SetAFCLimit(limit);
}

void  CPSKCore::fnSetSquelchThreshold(long thresh, long mode, long chan)
{
	if(thresh>99)
		thresh = 99;
	if(thresh<0)
		thresh = 0;
	if(mode>200)
		mode = 200;
	if(mode<0)
		mode = 0;
	if( (mode>1) && (mode<10) )
		mode = 10;
		if( ChkDetExists(chan) )
			m_pPSKDet[chan]->SetSquelchThresh(thresh,mode);
}

long  CPSKCore::fnGetSignalLevel(long chan)
{
LONG level = 0;
		if( IsRxChannelActive(chan) )
			level = m_pPSKDet[chan]->GetSignalLevel();
	if(level>99)
		level = 99;
	if(level<0)
		level = 0;
	return level;
}


/////////////////////////////////////////////////////////////////
// TX Functions
/////////////////////////////////////////////////////////////////
void  CPSKCore::fnStartTX(long mode)
{
		if(mode & (1<<7))
		{
			mode &= ~(1<<7);
			m_SatMode = TRUE;
		}
		else
		{
			m_SatMode = FALSE;
		}
		if(mode>18)	//AA6YQ 1.21
			mode = 18;
		if(mode<0)
			mode = 0;
			m_PSKMod.SetTXMode(mode);
			StartTX();
}

void  CPSKCore::fnStopTX()
{
		StopTX();
}

void  CPSKCore::fnAbortTX()
{
		AbortTX();
}

void  CPSKCore::fnSetTXFrequency(long freq)
{
	if(freq>3500)
		freq = 3500;
	if(freq<100)
		freq = 100;
		m_PSKMod.SetTXFreq(freq);
}

void  CPSKCore::fnSetCWIDString(char* lpszIDstrg)
{
		m_PSKMod.SetTXCWID(lpszIDstrg);
}

long  CPSKCore::fnSendTXCharacter( long txchar, bool cntrl)
{
LONG count = 0;
		m_PSKMod.PutTxQue(txchar, cntrl);
		count = m_PSKMod.GetTXCharsRemaining();
	return count;
}

long  CPSKCore::fnSendTXString(char* lpszTXStrg)
{
LONG ret = 0;
INT count = 0;
char ch;
		while( ((ch = lpszTXStrg[count++]) != 0 ) && (count < TX_BUF_SIZE) )
		{
			m_PSKMod.PutTxQue(ch, FALSE);
		}
		ret = m_PSKMod.GetTXCharsRemaining();
	return ret;
}

long  CPSKCore::fnGetTXCharsRemaining()
{
LONG count = 0;
		count = m_PSKMod.GetTXCharsRemaining();
	return count;
}

void  CPSKCore::fnClearTXBuffer()
{
		m_PSKMod.ClrQue();
}


void  CPSKCore::fnSetCWIDSpeed (long speed )
{
	if(speed>4)
		speed = 4;
	if(speed<1)
		speed = 1;
		m_PSKMod.SetCWIDSpeed(speed);
}

long  CPSKCore::fnSetComPort (long portnum, long mode )
{
LONG ret = 0;
/*
	if( portnum > 8 )
		portnum = 8;	//clamp number of ports 0 to 8
	if( portnum < 0 )
		portnum = 0;
		ret = SetComPort(portnum, mode);
*/
	return ret;
}

/////////////////////////////////////////////////////////////////
//  MISC Functions
/////////////////////////////////////////////////////////////////
void  CPSKCore::fnSetClockErrorAdjustment (long ppm)
{
	if(ppm>20000)
		ppm = 20000;
	if(ppm<-20000)
		ppm = -20000;
		SetClockErrorAdjustment(-ppm);
}

long  CPSKCore::fnGetDLLVersion ()
{	
long version = 0;
		version = m_DLLRevision;
	return version;
}

void  CPSKCore::fnGetErrorString(char* lpszError)
{
CString err = "";
		GetErrorMsg(err);
    strcpy( lpszError, (LPCTSTR)err);
}

long  CPSKCore::fnSetInputWavePath( char* sPath, 
								  long* pLengthTime, long Offset)
{
long error = DLLERR_UNKNOWN;
/*
		error = SetInputWavePath( sPath, (PINT)pLengthTime, (INT)Offset);
*/
	return error;
}

long  CPSKCore::fnSetOutputWavePath( char* sPath, long TimeLimit, long Append)
{
long error = DLLERR_UNKNOWN;
/*
		error = SetOutputWavePath( sPath, (INT)TimeLimit, (BOOL)Append);
*/
	return error;
}

long  CPSKCore::fnGetDebugData(double* val1, double* val2)
{
	*val1 = DebugFP1;
	*val2 = DebugFP2;
	return DebugINT;
}

/////////////////////////////////////////////////////////////////
//  Functions added after the initial release of the dll
/////////////////////////////////////////////////////////////////
void  CPSKCore::fnRewindInput (long Blocks )
{
	if(Blocks>99)
		Blocks = 99;
	if(Blocks<1)
		Blocks = 1;
		RewindInput( (INT)Blocks );
}


///////////////////////////////////////////////////////////////////////////
//  Stop process thread proceedure and wait for it to finish
//  Called by main program thread to signal data processing thread to stop
//	and then wait for it to stop before returning.
///////////////////////////////////////////////////////////////////////////
void CPSKCore::StopIO()
{
    //if( m_pProcThread != NULL )		//if thread is already running
    //{
		AbortTX();
		m_ThreadActive = FALSE;
		m_fProcThreadQuit = TRUE;	// stop it
        //::WaitForSingleObject(m_pProcThread->m_hThread,	2500);
        //delete m_pProcThread;
        //m_pProcThread = NULL;
		m_DLLStatus = DLLSTAT_OFF;

#ifdef USE_PERFORMANCE
	ReadPerformance();
#endif
    //}
}


///////////////////////////////////////////////////////////////////////////
//   Called to start the I/O thread running
//  maxchannels is the maximum number of RX channels that will be active at
// any one time.  If the caller has not specifically disabled any channels,
// they will all be active at the start.
///////////////////////////////////////////////////////////////////////////
INT CPSKCore::StartIO(/*HWND h_Wnd,*/ INT maxchannels, INT IOMode)
{
INT Timer;

qDebug()<<"CPSKCore::StartIO:"<<maxchannels<<","<<IOMode;
qDebug()<<"    m_AllChannelsActive:"<<m_AllChannelsActive;
    //if( m_pProcThread == NULL)
    //{
        //m_hWnd = h_Wnd;
		m_IOMode = IOMode;
		if(maxchannels>MAX_CHANNELS)
			return DLLERR_MEMORY;
		m_MaxNumRXChannels = maxchannels;
		if( m_AllChannelsActive )	//user has not modified active channel list
		{							//so create all and make all active
			for( INT i=0; i<m_MaxNumRXChannels; i++)
			{
				if( !ChkDetExists(i) )
					return DLLERR_MEMORY;
			}
			// delete any previously created channels greater than the new max
			for(int  i = m_MaxNumRXChannels; i<MAX_CHANNELS; i++)
			{
				if( m_pPSKDet[i] != NULL )
				{
					delete m_pPSKDet[i];
					m_pPSKDet[i] = NULL;
				}
			}
		}
        //m_PSKMod.m_hWnd = m_hWnd;
        //m_ThreadActive = FALSE;
		m_DLLStatus = DLLSTAT_OFF;
		m_fProcThreadQuit = FALSE;
        //m_pProcThread = AfxBeginThread(
        //	(AFX_THREADPROC)ThreadLauncher,	//thread function
        //	(LPVOID)this,					// ptr to this class
        //	THREAD_PRIORITY_NORMAL, 		//give worker thread priority
        //	0,								// same stack size
        //	CREATE_SUSPENDED,				// don't let it start yet
        //	NULL );							// same security attributes
        //m_pProcThread->m_bAutoDelete = FALSE;	// keep thread object around
        //m_pProcThread->ResumeThread ();	//let er rip
        //Timer = MAXTIMER;
        //while( (m_DLLStatus == DLLSTAT_OFF) && !m_fProcThreadQuit && (Timer-- > 0) )
        //	::Sleep(100);

        if(IOMode==IOM_UDP) {
            connect();
        } else {
            m_DLLStatus=DLLERR_SYS_NOTSUPPORTED;
        }
    //}
    //::Sleep(100);
	if(m_DLLStatus >= 0)
		return m_DLLStatus;
	else
		return DLLERR_NONE;
}

///////////////////////////////////////////////////////////////////////////
// Check to see if det object exists and create if not
//  returns FALSE if can't create object, TRUE if it now exists
///////////////////////////////////////////////////////////////////////////
BOOL CPSKCore::ChkDetExists(INT chan)
{
BOOL res = TRUE;
	if( (chan > (MAX_CHANNELS-1) ) || ( chan < 0) )
	{
		res = FALSE;		//error if channel num out of range
	}
	else
	{
		if( m_pPSKDet[chan] == NULL )	//if NULL then must create it
		{
            qDebug()<<"EnterCriticalSection";
            EnterCriticalSection(/*&*/m_CriticalSection);
            m_pPSKDet[chan] = new CPSKDet();	//create PSK Detector object
//			ASSERT( m_pPSKDet[chan]);
			if( m_pPSKDet[chan] == NULL )
			{
				res = FALSE;
			}
			else	//initialize the new detector object
			{
                m_pPSKDet[chan]->Init(SAMP_RATE, BUF_SIZE, /*NULL,*/ chan);
				m_pPSKDet[chan]->ResetDetector();
                //m_pPSKDet[chan]->m_hWnd = m_hWnd;	//give object user's msg handle

                QObject::connect(m_pPSKDet[chan],SIGNAL(pskCharReady(int,int)),this,SLOT(slotPskCharReady(int,int)));
			}
            qDebug()<<"LeaveCriticalSection";
            LeaveCriticalSection(/*&*/m_CriticalSection);
		}
	}
	return res;
}

///////////////////////////////////////////////////////////////////////////
// Call to Enable or disable a receiver channel either before or after the 
// worker thread is started. Returns FALSE if can't create/delete a channel
// TRUE if ok
///////////////////////////////////////////////////////////////////////////
BOOL CPSKCore::EnableRXChannel(INT chan, BOOL enable)
{
BOOL ret = TRUE;
	m_AllChannelsActive = FALSE;	//user is modifying active channel list
	if(enable)
	{
		ret = ChkDetExists(chan);	//create if doesn't already exist
	}
	else	//delete detector object if it exists
	{
        EnterCriticalSection(/*&*/m_CriticalSection);
		if( m_pPSKDet[chan] != NULL )
		{
			delete m_pPSKDet[chan];
			m_pPSKDet[chan] = NULL;
		}
        LeaveCriticalSection(/*&*/m_CriticalSection);
	}
	return ret;
}



///////////////////////////////////////////////////////////////////////////
// Call to see if te specified channel is Active.
// Returns TRUE if the channel is active
// FALSE if not active
///////////////////////////////////////////////////////////////////////////
BOOL CPSKCore::IsRxChannelActive(long chan)
{
	return( m_pPSKDet[chan] != NULL );
}

//(dec) 20-Dec-01 Added this back in after Moe dropped it in V1.14
///////////////////////////////////////////////////////////////////////////
// Call to retrieve the number of active receive channels
///////////////////////////////////////////////////////////////////////////
INT CPSKCore::GetNumActiveRXChannels()
{
INT num=0;
	for(INT i=0; i<m_MaxNumRXChannels; i++)
	{
		if( IsRxChannelActive(i) )
			num++;
	}
	return num;
}

///////////////////////////////////////////////////////////////////////////
// Starts the TX process running and stops the receive process
///////////////////////////////////////////////////////////////////////////
void CPSKCore::StartTX()
{
	if(	m_DLLStatus != DLLSTAT_OFF)
		m_RXState = FALSE;
}

///////////////////////////////////////////////////////////////////////////
// Stops the TX process after all characters have been sent and starts the
//      receive process.
///////////////////////////////////////////////////////////////////////////
void CPSKCore::StopTX()
{
	if(!m_RXState)
	{
		m_PSKMod.SetAutoShutoff(TRUE);
		StatusChange(DLLSTAT_TXFINISHING);
	}
}

///////////////////////////////////////////////////////////////////////////
// Aborts the TX process without waiting for all characters to be sent 
//    and starts the receive process.
//  If it is in the RX mode, the TX character buffer is cleared.
///////////////////////////////////////////////////////////////////////////
void CPSKCore::AbortTX()
{
	m_PSKMod.ClrQue();
	m_PSKMod.SetAutoShutoff(FALSE);
	m_PSKMod.SetAutoCWID(FALSE);
	if(!m_RXState )
		m_RXState = TRUE;
}

///////////////////////////////////////////////////////////////////////////
// Initializes the com port 
///////////////////////////////////////////////////////////////////////////
LONG CPSKCore::SetComPort(long portnum, long mode)
{
char szPort[10];
//HANDLE hComPort = INVALID_HANDLE_VALUE;
BOOL ret = FALSE;
#ifdef NOT_INCLUDED
	if( portnum == 0)
	{
		m_PTTPort = 0;
		m_ComPortMode = 0;
        EnterCriticalSection(/*&*/m_CriticalSection);
		if( m_hComPort != INVALID_HANDLE_VALUE )
		{
			EscapeCommFunction(m_hComPort, CLRRTS);
			EscapeCommFunction(m_hComPort, CLRDTR);
			CloseHandle( m_hComPort );
			m_hComPort = INVALID_HANDLE_VALUE;
		}
        LeaveCriticalSection(/*&*/m_CriticalSection);
	}
	else
	{
        EnterCriticalSection(/*&*/m_CriticalSection);
		if( m_hComPort != INVALID_HANDLE_VALUE )
		{
			EscapeCommFunction(m_hComPort, CLRRTS);
			EscapeCommFunction(m_hComPort, CLRDTR);
			CloseHandle( m_hComPort );
			m_hComPort = INVALID_HANDLE_VALUE;
		}
		wsprintf( szPort, "COM%d", portnum );
		hComPort = CreateFile( szPort, 0, 0, NULL,
							OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL );
		if( hComPort == INVALID_HANDLE_VALUE )
		{
			m_PTTPort = 0;
			m_ComPortMode = 0;
		}
		else
		{
			EscapeCommFunction(hComPort, CLRRTS);
			EscapeCommFunction(hComPort, CLRDTR);
			CloseHandle( hComPort );
			m_hComPort = INVALID_HANDLE_VALUE;
			m_ComPortMode = mode&0x0003;
			m_PTTPort = portnum;
			ret = TRUE;
		}
        LeaveCriticalSection(/*&*/m_CriticalSection);
	}
#endif
	return (LONG)ret;
}

///////////////////////////////////////////////////////////////////////////
// Initializes the FFT 
///////////////////////////////////////////////////////////////////////////
void CPSKCore::SetFFTParams(INT ave,double gain, INT type )
{
	m_Cfft.SetFFTParams( ave, gain, type, m_ClockError);
}

///////////////////////////////////////////////////////////////////////////
// calculates FFT on latest data and put's it in users array
///////////////////////////////////////////////////////////////////////////
BOOL CPSKCore::GetFFTData(LONG* pData, LONG start, LONG end)
{
BOOL overflo = FALSE;
	if( !m_fProcThreadQuit )
	{
        EnterCriticalSection(/*&*/m_CriticalSection);
		overflo = m_Cfft.GetFFTData( start, end, pData, m_RXState||m_SatMode);
        LeaveCriticalSection(/*&*/m_CriticalSection);
	}
	else
	{
		for(INT i=start; i<=end; i++)
			pData[i] = 0;
	}
	if(m_DLLStatus != DLLSTAT_RX)	//keep tx level from triggering
		return FALSE;				// an overflo
	else
		return overflo;
}

///////////////////////////////////////////////////////////////////////////
// Gets latest raw data and put's it in users array
///////////////////////////////////////////////////////////////////////////
BOOL CPSKCore::GetRawData(LONG* pData, LONG start, LONG end)
{
BOOL Ovrld = FALSE;
	if( !m_fProcThreadQuit )
	{
        EnterCriticalSection(/*&*/m_CriticalSection);
		for(INT i=start; i<=end;i++)
		{
			pData[i] = (INT)m_pBuffer[i];
			if( pData[i] > 29491 )	//flag overload if within 10% of max
				Ovrld = TRUE;
		}
        LeaveCriticalSection(/*&*/m_CriticalSection);
	}
	else
	{
		for(INT i=start; i<=end; i++)
			pData[i] = 0;
	}
	return Ovrld;
}

void CPSKCore::SetRXFrequency(long freq, long CaptureRange, long chan)
{
INT pkfreq = freq;
	if(CaptureRange>100)
		CaptureRange = 100;
	if(CaptureRange<0)
		CaptureRange = 0;
	if( (CaptureRange>10) && (m_DLLStatus != DLLSTAT_OFF) )
	{
        EnterCriticalSection(/*&*/m_CriticalSection);
		pkfreq = m_Cfft.FindPKfreq( freq, CaptureRange );
        LeaveCriticalSection(/*&*/m_CriticalSection);
	}
	if( m_pPSKDet[chan] != NULL )
		m_pPSKDet[chan]->SetRXFrequency(pkfreq);
}

void CPSKCore::SetClockErrorAdjustment(INT ppm)
{
	for( INT i=0; i<m_MaxNumRXChannels; i++)
	{
		if( m_pPSKDet[i] != NULL )
			m_pPSKDet[i]->SetSampleClkAdj(ppm);
	}
	m_PSKMod.SetSampleClkAdj(ppm);
	m_ClockError = 1.0 - (double)ppm/1000000.0;
}


void CPSKCore::GetErrorMsg(CString& err)
{
	err = m_ErrorMessage;
}


void CPSKCore::RewindInput(INT Blocks)
{
#ifdef NOT_INCLUDED
	if(m_RXState||m_SatMode)
	{
		if( m_SoundDev.m_InputOpen)
		{
			m_SoundDev.RewindInputData(Blocks);
		}
		else
		{
			if(m_InTotalSamples >= BUF_SIZE*Blocks)
			{
				if( m_WaveDev.RewindInputData(Blocks) )
					m_InTotalSamples = m_InTotalSamples - BUF_SIZE*Blocks;
			}
		}
	}
#endif
}

INT CPSKCore::SetInputWavePath(CString sPath, PINT pLengthTime, INT Offset)
{
INT error;
DWORD filesize;
#ifdef NOT_INCLUDED
WAVEFORMATEX WaveFormatEx;
	m_InWavePath = _T("");
	m_WaveInOffset = Offset*SAMP_RATE;
	error = m_WaveDev.InOpen(&sPath, &WaveFormatEx, 0, &filesize, 0);
	if( (error == 0) && (WaveFormatEx.nChannels == 1) &&
			(WaveFormatEx.nSamplesPerSec == SAMP_RATE) &&
			(WaveFormatEx.wBitsPerSample == 16) )
	{
		m_InWavePath = sPath;
		*pLengthTime = filesize/SAMP_RATE;
		if(m_WaveInOffset > (INT)filesize)
			m_WaveInOffset = filesize-1;
		return 0;
	}
	else
	{
		*pLengthTime = 0;
		ProcessError( error );
		return m_DLLStatus;
	}
#endif
    return 0;
}

INT CPSKCore::SetOutputWavePath(CString sPath, INT TimeLimit, INT Append)
{
INT ret=0;
#ifdef NOT_INCLUDED
	if(sPath != "")
	{
		m_OutWavePath = sPath;
		ret = 0;
	}
	else
	{
		ProcessError( WAVOUT_ERR_OPEN );
		ret = DLLERR_WAVOUT_OPEN;
	}
	m_WaveOutLimit = TimeLimit;
	m_WaveOutAppend = Append;
#endif
	return ret;
}

//*************************************************************************
//**********************  local routines  *********************************
//*************************************************************************

///////////////////////////////////////////////////////////////////////////
//  Main I/O thread processing loop
///////////////////////////////////////////////////////////////////////////
void CPSKCore::ProcessLoop()
{
#ifdef NOT_INCLUDED
INT i;
INT ErrorCode;
DWORD insize=0;
WAVEFORMATEX WaveFormatEx;

#ifdef USE_PERFORMANCE
	InitPerformance();
#endif

	m_InTotalSamples = 0;
	m_OutTotalSamples = 0;
	m_InFilePercentage = 0;
	m_OutFilePercentage = 0;
	if( m_IOMode & IOM_WAVEIN)
	{
		//Open wavefile for input if it is the source
		ErrorCode = m_WaveDev.InOpen( &m_InWavePath, &WaveFormatEx,
												BUF_SIZE, &insize, m_WaveInOffset);
		if( ErrorCode != NO_ERROR )
		{
			ProcessError( ErrorCode );
			StopThread();
		}
		if( (WaveFormatEx.nChannels != 1) ||
			(WaveFormatEx.nSamplesPerSec != SAMP_RATE) ||
			(WaveFormatEx.wBitsPerSample != 16) )
		{
			ProcessError( WAVIN_ERR_OPEN );
			StopThread();
		}
		m_InSampleLimit = insize;
		::PostMessage( m_hWnd, MSG_STATUSCHANGE, DLLSTAT_INWAVEFILESTATUS, 0);
	}
	if( m_IOMode & (IOM_WAVELOG|IOM_WAVETX) )
	{
		//Open wavefile for Output if it is specified
		ErrorCode = m_WaveDev.OutOpen( &m_OutWavePath, &m_WaveFormatEx, BUF_SIZE,
										m_WaveOutLimit*8000, !m_WaveOutAppend);
		if( ErrorCode != NO_ERROR )
		{
			ProcessError( ErrorCode );
			StopThread();
		}
		m_OutSampleLimit = m_WaveOutLimit*8000;
		::PostMessage( m_hWnd, MSG_STATUSCHANGE, DLLSTAT_OUTWAVEFILESTATUS, 0);
	}
	for( i=0; i<m_MaxNumRXChannels; i++)
	{
        EnterCriticalSection(/*&*/m_CriticalSection);
		if( m_pPSKDet[i] != NULL )
		{
			m_pPSKDet[i]->ResetDetector();
			m_pPSKDet[i]->m_hWnd = m_hWnd;
		}
        LeaveCriticalSection(/*&*/m_CriticalSection);
	}
	m_Cfft.ResetFFT();
	m_PSKMod.InitPSKMod(m_WaveFormatEx.nSamplesPerSec, m_hWnd, m_SatMode);
	m_ThreadActive = TRUE;
	StatusChange(DLLSTAT_RX);
	AbortTX();
	Sleep(50);
	PTTCntrl(FALSE);
	m_TXRunning = TRUE;
	m_RXState = TRUE;
//m_pPSKMod->SetTXMode(TUNE_MODE);
//m_pPSKMod->SetTXMode(BPSK_MODE);
//m_pPSKMod->SetTXMode(QPSKU_MODE);
//m_pPSKMod->InitPSKMod( SAMP_RATE, m_hWnd);

	while( !m_fProcThreadQuit )
	{
		if(m_SatMode)	// if full duplex operation
		{
			if( m_RXState )
			{
				if(m_TXRunning)	//if just finished TX mode or first time in
				{
					// Open soundcard if it is the source and not already open
					if( !(m_IOMode & IOM_WAVEIN) && !m_SoundDev.m_InputOpen)
					{
						// Open Soundcard for input
						ErrorCode = m_SoundDev.InOpen( &m_WaveFormatEx, BUF_SIZE,
																0, m_InputCardNum);
						if( ErrorCode != NO_ERROR )
						{
							ProcessError( ErrorCode );
							StopThread();
						}
					}
					if( m_SoundDev.m_OutputOpen && !(m_IOMode & IOM_ECHOINPUT))
					{
						m_SoundDev.OutClose();
					}
					StatusChange(DLLSTAT_RX);
					Sleep(50);	//delay to give xmitters time to switch
					PTTCntrl(FALSE);
					m_TXRunning = FALSE;
				}
			}
			else
			{
				if(!m_TXRunning)	//if just gone to TX mode
				{
					if( ((m_IOMode & IOM_NOTXSOUND)==0) && !m_SoundDev.m_OutputOpen)
					{
						// Open Soundcard for Output
						ErrorCode = m_SoundDev.OutOpen( &m_WaveFormatEx, BUF_SIZE,
																0, m_OutputCardNum); //AA6YQ 1.20
						if( ErrorCode != NO_ERROR )
						{
							ProcessError( ErrorCode );
							StopThread();
						}
					}
					m_PSKMod.InitPSKMod(m_WaveFormatEx.nSamplesPerSec, m_hWnd, m_SatMode);
					StatusChange(DLLSTAT_TX);
					PTTCntrl(TRUE);
					Sleep(50);	//delay to give xmitters time to switch
					m_TXRunning = TRUE;
				}
				ProcessTX();
			}
			ProcessRX();
		}
		else	// half duplex non-satellite operation
		{		
			if( m_RXState )
			{
				if(m_TXRunning)	//if just finished TX mode
				{
					for( i=0; i<m_MaxNumRXChannels; i++)
					{
                        EnterCriticalSection(/*&*/m_CriticalSection);
						if( m_pPSKDet[i] != NULL )
							m_pPSKDet[i]->ResetDetector();
                        LeaveCriticalSection(/*&*/m_CriticalSection);
					}
					m_Cfft.ResetFFT();
					StatusChange(DLLSTAT_RX);
					if( m_SoundDev.m_OutputOpen && !(m_IOMode & IOM_ECHOINPUT))
					{
						m_SoundDev.OutClose();
						Sleep(100);	//delay to give xmitters time to switch
						PTTCntrl(FALSE);
					}
					// Open soundcard if it is the source and not already open
					if( !(m_IOMode & IOM_WAVEIN) && !m_SoundDev.m_InputOpen)
					{
						// Open Soundcard for input
						ErrorCode = m_SoundDev.InOpen( &m_WaveFormatEx, BUF_SIZE,
																0, m_InputCardNum);
						if( ErrorCode != NO_ERROR )
						{
							ProcessError( ErrorCode );
							StopThread();
						}
					}
					if( m_IOMode & IOM_ECHOINPUT )	//if need to echo input to soundcard out
					{
						if( !m_SoundDev.m_OutputOpen)
						{	// Open Soundcard for Echo Output
							ErrorCode = m_SoundDev.OutOpen( &m_WaveFormatEx, BUF_SIZE,
															0, m_OutputCardNum); //AA6YQ 1.20
							if( ErrorCode != NO_ERROR )
							{
								ProcessError( ErrorCode );
								StopThread();
							}
						}
					}
					m_TXRunning = FALSE;
				}
				ProcessRX();
			}
			else
			{
				if(!m_TXRunning)	//if just finished Rx mode
				{
					if( m_SoundDev.m_InputOpen)
					{
						m_SoundDev.InClose();
					}
					if( ((m_IOMode & IOM_NOTXSOUND) ==0) && !m_SoundDev.m_OutputOpen)
					{
						// Open Soundcard for Output
						ErrorCode = m_SoundDev.OutOpen( &m_WaveFormatEx, BUF_SIZE,
																0, m_OutputCardNum); //AA6YQ 1.20
						if( ErrorCode != NO_ERROR )
						{
							ProcessError( ErrorCode );
							StopThread();
						}
						PTTCntrl(TRUE);
						Sleep(100);	//delay to give xmitters time to switch
					}
					m_PSKMod.InitPSKMod(m_WaveFormatEx.nSamplesPerSec, m_hWnd, m_SatMode);
					StatusChange(DLLSTAT_TX);
					m_TXRunning = TRUE;
				}
				ProcessTX();
			}
		}
	}
	m_RXState = TRUE;
	m_ThreadActive = FALSE;
	DeleteResources();		//kill all resources
	Sleep(50);
	PTTCntrl(FALSE);
#endif
}

////////////////////////////////////////////////////////////////////
// Process for Receiving audio data from soundcard
////////////////////////////////////////////////////////////////////
void CPSKCore::ProcessRX()
{
INT i;
INT result;
INT ErrorCode;

//qDebug()<<"CPSKCore::ProcessRX";
	if( m_IOMode & IOM_WAVEIN)	// Get data from wavefile
	{
#ifdef NOT_INCLUDED
		if( (ErrorCode = m_WaveDev.InRead( m_pBuffer, BUF_SIZE )) != BUF_SIZE)
		{
			if( ErrorCode < 0)
			{
				ErrorCode = m_WaveDev.GetError();
				ProcessError( ErrorCode );
			}
			StatusChange(DLLSTAT_INWAVEDONE);
			m_WaveDev.InClose();
			::PostMessage( m_hWnd, MSG_STATUSCHANGE, DLLSTAT_INWAVEFILESTATUS, 100);
			StopThread();
		}
		m_InTotalSamples += BUF_SIZE;
		if( (m_InSampleLimit > 0) )
		{
			INT Percent = (100*m_InTotalSamples)/m_InSampleLimit;
			if(Percent != m_InFilePercentage)
			{
				::PostMessage( m_hWnd, MSG_STATUSCHANGE, DLLSTAT_INWAVEFILESTATUS, Percent);
				m_InFilePercentage = Percent;
			}
		}
		Sleep(25);		// delay a bit so Windows msg system doesn't clog up
#endif
	}
	else	//Get data from soundcard
	{
#ifdef NOT_INCLUDED
		if( (result = m_SoundDev.InRead( m_pBuffer, BUF_SIZE )) != BUF_SIZE)
		{
			ErrorCode = m_SoundDev.GetError();
			if( (ErrorCode == SOUNDIN_ERR_OVERFLOW) ||
				(ErrorCode == SOUNDIN_ERR_TIMEOUT) )
			{								//cpu couldn't keep up
											// so try again
				m_SoundDev.InClose();
				StatusChange(DLLSTAT_CPUSLOW);
				ErrorCode = m_SoundDev.InOpen( &m_WaveFormatEx, BUF_SIZE, 0, m_InputCardNum);
			}
			if( ErrorCode != NO_ERRORS)	//must be some other soundcard error
			{
				m_SoundDev.InClose();
				ProcessError( ErrorCode );
				StopThread();
			}
		}
		Sleep(0);		// delay a bit so Windows msg system doesn't clog up
#endif
	}
//        m_pPSKMod->CalcPSK( m_pBuffer, BUF_SIZE );
//      AddGaussian(m_pBuffer, BUF_SIZE, 16384.0/5.0);

//////////  Loop through all the receiver channels and decode each one
#ifdef USE_PERFORMANCE
StartPerformance();
#endif
	for( i=0; i<m_MaxNumRXChannels; i++)
	{
        EnterCriticalSection(/*&*/m_CriticalSection);
		if( m_pPSKDet[i] != NULL )
			m_pPSKDet[i]->ProcPSKDet(m_pBuffer);
        LeaveCriticalSection(/*&*/m_CriticalSection);
	}
#ifdef USE_PERFORMANCE
StopPerformance();
#endif
    EnterCriticalSection(/*&*/m_CriticalSection);
	m_Cfft.CalcFFT( m_pBuffer);
	if( m_pPSKDet[0] != NULL )
	{
        LeaveCriticalSection(/*&*/m_CriticalSection);
        //::PostMessage( m_hWnd, MSG_DATARDY, m_pPSKDet[0]->GetRXFrequency(),
        //						m_pPSKDet[0]->GetSignalLevel());
        //qDebug()<<"emit dataReady";
        emit dataReady(m_pPSKDet[0]->GetRXFrequency(),
                       m_pPSKDet[0]->GetSignalLevel());
	}
	else
	{
        LeaveCriticalSection(/*&*/m_CriticalSection);
        //::PostMessage( m_hWnd, MSG_DATARDY, 1500, 0 );
        //qDebug()<<"emit dataReady";
        emit dataReady(1500,0);
	}
	if( (m_IOMode & IOM_WAVELOG) )	//if need to log rx audio to wave file
	{
#ifdef NOT_INCLUDED
		if( (ErrorCode = m_WaveDev.OutWrite( m_pBuffer, BUF_SIZE )) != BUF_SIZE)
		{
			if( ErrorCode < 0)
			{
				ErrorCode = m_WaveDev.GetError();
				ProcessError( ErrorCode );
			}
			StatusChange(DLLSTAT_OUTWAVEDONE);
			m_WaveDev.OutClose();	//time limit expired
			::PostMessage( m_hWnd, MSG_STATUSCHANGE, DLLSTAT_OUTWAVEFILESTATUS, 100);
			StopThread();
		}
		m_OutTotalSamples += BUF_SIZE;
		if( (m_OutSampleLimit > 0) )
		{
			INT Percent = (100*m_OutTotalSamples)/m_OutSampleLimit;
			if(Percent != m_OutFilePercentage)
			{
				::PostMessage( m_hWnd, MSG_STATUSCHANGE, DLLSTAT_OUTWAVEFILESTATUS, Percent);
				m_OutFilePercentage = Percent;
			}
		}
#endif
	}
	if( (m_IOMode & IOM_ECHOINPUT) && !m_TXRunning )	//if need to echo input to soundcard out
	{
#ifdef NOT_INCLUDED
		if( (result = m_SoundDev.OutWrite( m_pBuffer, BUF_SIZE )) != BUF_SIZE)
		{
			ErrorCode = m_SoundDev.GetError();
			if( (ErrorCode == SOUNDOUT_ERR_UNDERFLOW) ||
				(ErrorCode == SOUNDOUT_ERR_TIMEOUT) )
			{								//cpu couldn't keep up
											// so try again
				m_SoundDev.OutClose();
				StatusChange(DLLSTAT_CPUSLOW);
				ErrorCode = m_SoundDev.OutOpen( &m_WaveFormatEx, BUF_SIZE, 0, m_OutputCardNum);  //AA6YQ 1.20
			}
			if( ErrorCode != NO_ERRORS)	//must be some other soundcard error
			{
				m_SoundDev.OutClose();
				ProcessError( ErrorCode );
				StopThread();
			}
		}
#endif
	}
}

///////////////////////////////////////////////////////////////////////
// Processes transmit data while in TX mode
///////////////////////////////////////////////////////////////////////
void CPSKCore::ProcessTX()
{
INT result;
INT ErrorCode;
	m_PSKMod.CalcPSK( m_pBuffer, BUF_SIZE );	//generate the tx audio samples
#ifdef NOT_INCLUDED
	if( ((m_IOMode & IOM_NOTXSOUND) == 0) )		//send to sound card output?
	{
		if( (result = m_SoundDev.OutWrite( m_pBuffer, BUF_SIZE )) != BUF_SIZE)
		{
			ErrorCode = m_SoundDev.GetError();
			if( (ErrorCode == SOUNDOUT_ERR_UNDERFLOW) ||
				(ErrorCode == SOUNDOUT_ERR_TIMEOUT) )
			{								//cpu couldn't keep up
											// so try again
				m_SoundDev.OutClose();
				StatusChange(DLLSTAT_CPUSLOW);
				ErrorCode = m_SoundDev.OutOpen( &m_WaveFormatEx, BUF_SIZE, 0, m_OutputCardNum); //AA6YQ 1.20
			}
			if( ErrorCode != NO_ERRORS)	//must be some other soundcard error
			{
				m_SoundDev.OutClose();
				ProcessError( ErrorCode );
				StopThread();
			}
		}
	}
	else
		Sleep(25);		// delay a bit so Windows msg system doesn't clog up
	if( (m_IOMode & IOM_WAVETX) )	//if need to write Tx data to wave file
	{
		if( (ErrorCode = m_WaveDev.OutWrite( m_pBuffer, BUF_SIZE )) != BUF_SIZE)
		{
			if( ErrorCode < 0)
			{
				ErrorCode = m_WaveDev.GetError();
				ProcessError( ErrorCode );
			}
			StatusChange(DLLSTAT_OUTWAVEDONE);
			m_WaveDev.OutClose();	//time limit expired
			::PostMessage( m_hWnd, MSG_STATUSCHANGE, DLLSTAT_OUTWAVEFILESTATUS, 100);
			StopThread();
		}
		m_OutTotalSamples += BUF_SIZE;
		if( (m_OutSampleLimit > 0) )
		{
			INT Percent = (100*m_OutTotalSamples)/m_OutSampleLimit;
			if(Percent != m_OutFilePercentage)
			{
				::PostMessage( m_hWnd, MSG_STATUSCHANGE, DLLSTAT_OUTWAVEFILESTATUS, Percent);
				m_OutFilePercentage = Percent;
				Sleep(25);
			}
        }
	}
	else	// here if neither sound or wave file output just delay
		if( (m_IOMode & IOM_NOTXSOUND) && !(m_IOMode & IOM_WAVETX) )
			Sleep(225);

#endif
	if(!m_SatMode)	// if half duplex operation then create display fft of tx data
	{
        EnterCriticalSection(/*&*/m_CriticalSection);
		m_Cfft.CalcFFT( m_pBuffer );
		if( m_pPSKDet[0] != NULL )
		{
            LeaveCriticalSection(/*&*/m_CriticalSection);
            //::PostMessage( m_hWnd, MSG_DATARDY, m_pPSKDet[0]->GetRXFrequency(),
            //					m_pPSKDet[0]->GetSignalLevel());
            emit dataReady(m_pPSKDet[0]->GetRXFrequency(),
                           m_pPSKDet[0]->GetSignalLevel());
		}
		else
		{
            LeaveCriticalSection(/*&*/m_CriticalSection);
            //::PostMessage( m_hWnd, MSG_DATARDY, 1500, 0 );
            emit dataReady(1500,0);
		}
	}
	if( !m_PSKMod.GetTXState() )
	{
		m_RXState = TRUE;
	}
}

///////////////////////////////////////////////////////////////////////////
//  Free resources and exit process thread
//   Called by process thread itself to shut itself off if not signaled by
//  main program thread.
///////////////////////////////////////////////////////////////////////////
void CPSKCore::StopThread()
{
//  if any resources are open, kill em.
    //m_pProcThread->m_bAutoDelete = TRUE;	//make thread clean up after
    //m_pProcThread = NULL;  				// itself before it dies
	DeleteResources();
    //AfxEndThread(3);
}

///////////////////////////////////////////////////////////////////////////
//   Called to delete any allocated resources in this class
///////////////////////////////////////////////////////////////////////////
void CPSKCore::DeleteResources()
{
	m_ThreadActive = FALSE;
    m_IOMode = IOM_UDP;
    //m_WaveDev.InClose();
    //m_WaveDev.OutClose();
    //m_SoundDev.InClose();
    //m_SoundDev.OutClose();
}

//////////////////////////////////////////////////////////////////////////
// Simple minded error message display for various soundcard errors
// Should be put in string table.
//////////////////////////////////////////////////////////////////////////
void CPSKCore::ProcessError( INT ErrorCode )
{
	switch( ErrorCode )
	{
		case NO_ERRORS:
			m_ErrorMessage = _T("");
			break;
		case MEMORY_ERROR:
        //case MMSYSERR_NOMEM:
			m_ErrorMessage = _T("Memory Allocation error");
			ErrorCode = DLLERR_MEMORY;
			break;
		case SOUNDIN_ERR_NOTOPEN:
			m_ErrorMessage = _T("Tried to read and soundcard is not open");
			ErrorCode = DLLERR_IN_NOTOPEN;
			break;
		case SOUNDIN_ERR_OVERFLOW:
			m_ErrorMessage = _T("Input buffers overflowed");
			ErrorCode = DLLERR_IN_OVERFLOW;
			break;
		case SOUNDIN_ERR_TIMEOUT:
			m_ErrorMessage = _T("Timed out waiting for input buffers");
			ErrorCode = DLLERR_IN_TIMEOUT;
			break;
		case SOUNDOUT_ERR_NOTOPEN:
			m_ErrorMessage = _T("Tried to write and soundcard is not open");
			ErrorCode = DLLERR_OUT_NOTOPEN;
			break;
		case SOUNDOUT_ERR_UNDERFLOW:
			m_ErrorMessage = _T("Output buffers Underflowed");
			ErrorCode = DLLERR_OUT_UNDERFLOW;
			break;
		case SOUNDOUT_ERR_TIMEOUT:
			m_ErrorMessage = _T("Timed out waiting for output buffers");
			ErrorCode = DLLERR_OUT_TIMEOUT;
			break;

#ifdef NOT_INCLUDED
        case WAVERR_BADFORMAT:
            m_ErrorMessage = _T("Card doesn't support 16bit, 8000Hz, Mono format");
            ErrorCode = DLLERR_WAV_BADFORMAT;
            break;
        case WAVERR_STILLPLAYING:
            m_ErrorMessage = _T("still something playing");
            ErrorCode = DLLERR_WAV_STILLPLAYING;
            break;
        case WAVERR_UNPREPARED:
            m_ErrorMessage = _T("header not prepared ");
            ErrorCode = DLLERR_WAV_UNPREPARED;
            break;
        case WAVERR_SYNC:
            m_ErrorMessage = _T("device is synchronous");
            ErrorCode = DLLERR_WAV_SYNC;
            break;

		case WAVIN_ERR_OPEN:
			m_ErrorMessage = _T("Can't open wave file for input");
			ErrorCode = DLLERR_WAVIN_OPEN;
			break;
		case WAVIN_ERR_NOTWAVE:
			m_ErrorMessage = _T("File is not a RIFF wave type");
			ErrorCode = DLLERR_WAVIN_NOTWAVE;
			break;
		case WAVIN_ERR_INVALID:
			m_ErrorMessage = _T("Invalid wave file");
			ErrorCode = DLLERR_WAVIN_INVALID;
			break;
		case WAVIN_ERR_NODATA:
			m_ErrorMessage = _T("No data in file");
			ErrorCode = DLLERR_WAVIN_NODATA;
			break;
		case WAVIN_ERR_NOTSUPPORTED:
			m_ErrorMessage = _T("Not a supported data type");
			ErrorCode = DLLERR_WAVIN_NOTSUPPORTED;
			break;
		case WAVIN_ERR_READING:
			m_ErrorMessage = _T("Error reading data from file");
			ErrorCode = DLLERR_WAVIN_READING;
			break;
		case WAVIN_ERR_NOTOPEN:
			m_ErrorMessage = _T("Tried to read and file is not open");
			ErrorCode = DLLERR_WAVIN_NOTOPEN;
			break;

		case WAVOUT_ERR_OPEN:
			m_ErrorMessage = _T("Can't open wave file for output");
			ErrorCode = DLLERR_WAVOUT_OPEN;
			break;
		case WAVOUT_ERR_WRITING:
			m_ErrorMessage = _T("Error writing to wave file");
			ErrorCode = DLLERR_WAVOUT_WRITING;
			break;
		case WAVOUT_ERR_NOTOPEN:
			m_ErrorMessage = _T("Tried to write and file is not open");
			ErrorCode = DLLERR_WAVOUT_NOTOPEN;
			break;


        case MMSYSERR_BADDEVICEID:
            m_ErrorMessage = _T("Bad Device ID\nIs Soundcard Present?");
            ErrorCode = DLLERR_SYS_BADDEVICEID;
            break;
		case MMSYSERR_NOTENABLED:
			m_ErrorMessage = _T("Driver failed enable");
			ErrorCode = DLLERR_SYS_NOTENABLED;
			break;
		case MMSYSERR_ALLOCATED:
			m_ErrorMessage = _T("Device already allocated");
			ErrorCode = DLLERR_SYS_ALLOCATED;
			break;
		case MMSYSERR_INVALHANDLE:
			m_ErrorMessage = _T("Device handle is invalid");
			ErrorCode = DLLERR_SYS_INVALHANDLE;
			break;
		case MMSYSERR_NODRIVER:
			m_ErrorMessage = _T("No device driver present");
			ErrorCode = DLLERR_SYS_NODRIVER;
			break;
		case MMSYSERR_NOTSUPPORTED:
			m_ErrorMessage = _T("Function isn't supported");
			ErrorCode = DLLERR_SYS_NOTSUPPORTED;
			break;
		case MMSYSERR_BADERRNUM:
			m_ErrorMessage = _T("Error value out of range");
			ErrorCode = DLLERR_SYS_BADERRNUM;
			break;
		case MMSYSERR_INVALFLAG: 
			m_ErrorMessage = _T("Invalid flag passed");
			ErrorCode = DLLERR_SYS_INVALFLAG;
			break;
		case MMSYSERR_INVALPARAM: 
			m_ErrorMessage = _T("Invalid parameter passed");
			ErrorCode = DLLERR_SYS_INVALPARAM;
			break;
		case MMSYSERR_HANDLEBUSY:
			m_ErrorMessage = _T("Handle being used");
			ErrorCode = DLLERR_SYS_HANDLEBUSY;
			break;
		case MMSYSERR_NODRIVERCB:
			m_ErrorMessage = _T("driver does not call DriverCallback");
			ErrorCode = DLLERR_SYS_NODRIVERCB;
			break;
		case MMSYSERR_INVALIDALIAS:
		case MMSYSERR_BADDB:
		case MMSYSERR_KEYNOTFOUND:
		case MMSYSERR_READERROR:
		case MMSYSERR_WRITEERROR:
		case MMSYSERR_DELETEERROR:
		case MMSYSERR_VALNOTFOUND:
			m_ErrorMessage = _T("Registry error");
			ErrorCode = DLLERR_SYS_REGISTRY;
			break;
#endif
		default:
			m_ErrorMessage = _T("Unknown Error");
			ErrorCode = DLLERR_UNKNOWN;
			break;
	}
	m_DLLStatus = ErrorCode;
	if(	m_ThreadActive )
		StatusChange(ErrorCode);
}


void CPSKCore::StatusChange(INT code)
{
	if( code != m_DLLStatus )
	{
		m_DLLStatus = code;
        emit statusChange(code);
        //::PostMessage( m_hWnd, MSG_STATUSCHANGE, code, 0);
	}
}

//////////////////////////////////////////////////////////////////
//  Controls COM port signals DTR and RTS for Xmitter on/off control
//////////////////////////////////////////////////////////////////
void CPSKCore::PTTCntrl(BOOL Ptton)
{
	if( Ptton )
	{
        // send message
	}
	else
	{
        //send message
	}
}

//////////////////////////////////////////////////////////////////
// Adds n gaussian random doubles with 0 mean and std_dev = RMS = std
//    to the specified buffer
//////////////////////////////////////////////////////////////////
void CPSKCore::AddGaussian(double * pData, INT n, double std)
{
#ifdef USE_NOISEGEN
INT i = 0;
double rad;
double r;
double u1;
double u2;
	while( i<n )
	{
// Generate two uniform random numbers between -1 and +1
// that are inside the unit circle
		do {
			u1 = 1.0 - 2.0 * (double)rand()/(double)RAND_MAX ;
			u2 = 1.0 - 2.0 * (double)rand()/(double)RAND_MAX ;
			r = u1*u1 + u2*u2;
		} while(r >= 1.0 || r == 0.0);
		rad = sqrt(-2.0*log(r)/r);
		pData[i++] += (std*u1*rad);
		pData[i++] += (std*u2*rad);
	}
#endif
}

void CPSKCore::fnSetConnection(QString h,int r,int l) {
    host=h;
    remotePort=r;
    localPort=l;
}

void CPSKCore::fnSetBand(int b,long f) {
    band=b;
    frequency=f;
    if(hasConnection) {
        QString command;
        command.clear();
        command.append(QString("frequency "));
        command.append(QString::number(frequency));
        command.append(QString(" "));
        command.append(QString::number(band));
        sendCommand(command);
    }
}

void CPSKCore::connect() {
    qDebug()<<"CIOSntrl::connect:"<<host<<":"<<remotePort<<" "<<localPort;
    for( int i=0; i<m_MaxNumRXChannels; i++)
    {
        EnterCriticalSection(/*&*/m_CriticalSection);
        if( m_pPSKDet[i] != NULL )
        {
            m_pPSKDet[i]->ResetDetector();
            //m_pPSKDet[i]->m_hWnd = m_hWnd;
        } else {
            qDebug()<<"connect: m_pPSKDet["<<i<<"] is NULL";
        }
        LeaveCriticalSection(/*&*/m_CriticalSection);
    }
    m_Cfft.ResetFFT();
    m_PSKMod.InitPSKMod(SAMP_RATE, /*m_hWnd,*/ m_SatMode);
    //m_ThreadActive = TRUE;
    StatusChange(DLLSTAT_RX);
    PTTCntrl(FALSE);
    m_TXRunning = FALSE;
    m_RXState = TRUE;

    tcpSocket=new QTcpSocket();
    tcpSocket->connectToHost(host,remotePort);
    QObject::connect(tcpSocket, SIGNAL(connected()), this, SLOT(connected()));
    QObject::connect(tcpSocket, SIGNAL(readyRead()), this, SLOT(tcpReadyRead()));
    QObject::connect(tcpSocket,SIGNAL(error(QAbstractSocket::SocketError)),this,SLOT(tcpSocketError(QAbstractSocket::SocketError)));

    m_pBufferIndex=0;
    udpSocket=new QUdpSocket();
    udpSocket->bind(localPort, QUdpSocket::ShareAddress);
    QObject::connect(udpSocket,SIGNAL(readyRead()),this,SLOT(udpReadyRead()));
    QObject::connect(udpSocket,SIGNAL(error(QAbstractSocket::SocketError)),this,SLOT(udpSocketError(QAbstractSocket::SocketError)));

}

void CPSKCore::tcpSocketError(QAbstractSocket::SocketError error) {
    qDebug()<<"CPSKCore::tcpSocketError: "<<error;
}

void CPSKCore::udpSocketError(QAbstractSocket::SocketError error) {
    qDebug()<<"CPSKCore::udpSocketError: "<<error;
}

void CPSKCore::connected() {
    QString command;

    sequence=0;
    hasConnection=TRUE;
    qDebug()<<"CPSKCore::connected";;

    command.clear();
    command.append(QString("connect "));
    command.append(QString::number(localPort));
    command.append(QString(" "));
    command.append(QString::number(8000));
    command.append(QString(" "));
    command.append(QString::number(1));
    command.append(QString(" "));
    command.append(QString("PCM"));
    command.append(QString(" "));
    command.append(QString("QtPSK"));
    sendCommand(command);

    // send initial settings
    command.clear();
    command.append(QString("frequency "));
    command.append(QString::number(frequency));
    command.append(QString(" "));
    command.append(QString::number(band));
    sendCommand(command);

    command.clear();
    command.append(QString("mode 7")); // 1 USB, 7 DIGU, 8 SPEC
    sendCommand(command);

    command.clear();
    command.append(QString("filter 0 4000")); // 4KHz
    sendCommand(command);

}

void CPSKCore::sendCommand(QString command) {
    qDebug()<<"CPSKCore::sendCommand:"<<command;
    if(tcpSocket!=NULL) {
        tcpMutex.lock();
        tcpSocket->write(command.append("\n").toLocal8Bit());
        tcpSocket->flush();
        tcpMutex.unlock();
    }
}

void CPSKCore::tcpReadyRead() {
    // response to a command
}

void CPSKCore::udpReadyRead() {
    // an audio buffer
    // need to get BUF_SIZE samples
    int length;
    short sample;
    qint64 this_sequence;
    //qDebug()<<"CPSKCore::udpReadyRead";
    if(udpSocket!=NULL) {
        while(udpSocket->hasPendingDatagrams()) {
            length=udpSocket->readDatagram(udpBuffer,sizeof(udpBuffer));
            if(length==sizeof(udpBuffer)) {
                this_sequence=(udpBuffer[0]&0xFF)<<56;
                this_sequence+=(udpBuffer[1]&0xFF)<<48;
                this_sequence+=(udpBuffer[2]&0xFF)<<40;
                this_sequence+=(udpBuffer[3]&0xFF)<<32;
                this_sequence+=(udpBuffer[4]&0xFF)<<24;
                this_sequence+=(udpBuffer[5]&0xFF)<<16;
                this_sequence+=(udpBuffer[6]&0xFF)<<8;
                this_sequence+=(udpBuffer[7]&0xFF);
                if(this_sequence!=sequence) {
                    qDebug()<<"udpReadyRead: this_sequence:"<<this_sequence<<" expected:"<<sequence;
                }
                if(sequence%100==0) {
                    qDebug()<<sequence;
                }
                sequence=this_sequence+1;
                int i=8;
                while(i<length) {
                    sample=(udpBuffer[i++]&0xFF)<<8;
                    sample+=(udpBuffer[i++]&0xFF);
                    m_pBuffer[m_pBufferIndex++]=(double)sample*0.80;
                    if(m_pBufferIndex==BUF_SIZE) {
                        //ProcessRX();
                        for( i=0; i<m_MaxNumRXChannels; i++)
                        {
                            EnterCriticalSection(/*&*/m_CriticalSection);
                            if( m_pPSKDet[i] != NULL ) {
                                m_pPSKDet[i]->ProcPSKDet(m_pBuffer);
                            } else {
                                qDebug()<<"m_pPSKDet["<<i<<"] is NULL";
                            }
                            LeaveCriticalSection(/*&*/m_CriticalSection);
                        }

                        EnterCriticalSection(/*&*/m_CriticalSection);
                        m_Cfft.CalcFFT( m_pBuffer);
                        if( m_pPSKDet[0] != NULL )
                        {
                            LeaveCriticalSection(/*&*/m_CriticalSection);
                            emit dataReady(m_pPSKDet[0]->GetRXFrequency(),
                                           m_pPSKDet[0]->GetSignalLevel());
                        }
                        else
                        {
                            LeaveCriticalSection(/*&*/m_CriticalSection);
                            emit dataReady(1500,0);
                        }
                        m_pBufferIndex=0;
                    }
                }
            } else {
                qDebug()<<"CPSKCore::udpReadyRead: expected:"<<sizeof(udpBuffer)<<" got:"<<length;
            }
        }
    }
}

void CPSKCore::slotPskCharReady(int ch,int rx) {
    emit pskCharReady(ch,rx);
}
