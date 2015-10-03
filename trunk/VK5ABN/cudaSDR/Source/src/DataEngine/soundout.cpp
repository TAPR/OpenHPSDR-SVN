/********************************************************************
 * soundout.cpp: implementation of the CSoundOut class.
 *
 *	This class implements a class to output data to a soundcard.
 * A fractional resampler is used to convert the users input rate to
 * the sound card rate and also perform frequency lock between the
 * two clock domains.
 *
 * History:
 *      2010-09-15  Initial creation MSW
 *      2011-03-27  Initial release
 *      2011-08-07  Changed some debug output
 *      2015-01-24  RRK, Minor mods, adding to cudaSDR
 *******************************************************************/

//==========================================================================================
// + + +   This Software is released under the "Simplified BSD License"  + + +
//Copyright 2010 Moe Wheatley. All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are
//permitted provided that the following conditions are met:
//
//   1. Redistributions of source code must retain the above copyright notice, this list of
//	  conditions and the following disclaimer.
//
//   2. Redistributions in binary form must reproduce the above copyright notice, this list
//	  of conditions and the following disclaimer in the documentation and/or other materials
//	  provided with the distribution.
//
//THIS SOFTWARE IS PROVIDED BY Moe Wheatley ``AS IS'' AND ANY EXPRESS OR IMPLIED
//WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL Moe Wheatley OR
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//The views and conclusions contained in the software and documentation are those of the
//authors and should not be interpreted as representing official policies, either expressed
//or implied, of Moe Wheatley.
//==========================================================================================
#include "soundout.h"
#include <QDebug>
#include <math.h>

#define SOUNDCARD_RATE 48000	//output soundcard sample rate
//#define SOUNDCARD_RATE 44100

#define FILTERQLEVEL_ALPHA 0.001
#define P_GAIN 2.38e-7		//Proportional gain

#define TRUE 1
#define FALSE 0

#define TEST_ERROR 1.0
//#define TEST_ERROR 1.001	//use to force fixed sample rate error for testing
//#define TEST_ERROR 0.999

/////////////////////////////////////////////////////////////////////
//   constructor/destructor
/////////////////////////////////////////////////////////////////////
CSoundOut::CSoundOut(QObject *parent) :
	QThread(parent)
{
	m_pParent = parent;
	m_pAudioOutput = NULL;
	m_pOutput = NULL;
	m_ThreadQuit = true;
	m_UserDataRate = SOUNDCARD_RATE;
	m_OutRatio = 1.0;
	//RRK m_OutAudioFormat.setFrequency(SOUNDCARD_RATE);
	m_OutAudioFormat.setSampleRate(SOUNDCARD_RATE);
	m_OutResampler.Init(8192);
	m_RateCorrection = 0.0;
	m_Gain = 1.0;
	m_Startup = true;
	m_BlockingMode = false;
}

CSoundOut::~CSoundOut()
{
	Stop();
}

void GetAlsaMasterVolume(long *volume)
{
    long min, max;
    snd_mixer_t *handle;
    snd_mixer_selem_id_t *sid;
    const char *card = "default";
    const char *selem_name = "Master";

    snd_mixer_open(&handle, 0);
    snd_mixer_attach(handle, card);
    snd_mixer_selem_register(handle, NULL, NULL);
    snd_mixer_load(handle);

    snd_mixer_selem_id_alloca(&sid);
    snd_mixer_selem_id_set_index(sid, 0);
    snd_mixer_selem_id_set_name(sid, selem_name);
    snd_mixer_elem_t* elem = snd_mixer_find_selem(handle, sid);

    snd_mixer_selem_get_playback_volume(elem, SND_MIXER_SCHN_FRONT_LEFT, volume);

    snd_mixer_close(handle);
}

void SetAlsaMasterVolume(long volume)
{
    long min, max;
    snd_mixer_t *handle;
    snd_mixer_selem_id_t *sid;
    const char *card = "default";
    const char *selem_name = "Master";

    snd_mixer_open(&handle, 0);
    snd_mixer_attach(handle, card);
    snd_mixer_selem_register(handle, NULL, NULL);
    snd_mixer_load(handle);

    snd_mixer_selem_id_alloca(&sid);
    snd_mixer_selem_id_set_index(sid, 0);
    snd_mixer_selem_id_set_name(sid, selem_name);
    snd_mixer_elem_t* elem = snd_mixer_find_selem(handle, sid);

    snd_mixer_selem_get_playback_volume_range(elem, &min, &max);
    snd_mixer_selem_set_playback_volume_all(elem, volume * max / 100);

    snd_mixer_close(handle);
}

/////////////////////////////////////////////////////////////////////
// Starts up soundcard output thread using soundcard at list OutDevIndx
/////////////////////////////////////////////////////////////////////
bool CSoundOut::Start(int OutDevIndx, bool StereoOut, double UsrDataRate, bool BlockingMode)
{
QAudioDeviceInfo  DeviceInfo;
	long mvolume;
	m_StereoOut = StereoOut;
	m_BlockingMode = BlockingMode;
	//Get required soundcard from list
	m_OutDevices = DeviceInfo.availableDevices(QAudio::AudioOutput);

	if (-1 == OutDevIndx) GetAlsaMasterVolume(&mvolume);
		qDebug()<<"Soundcard volume" << mvolume;

	if (-1 == OutDevIndx) m_OutDeviceInfo = QAudioDeviceInfo::defaultOutputDevice();
	else m_OutDeviceInfo = m_OutDevices.at(OutDevIndx);

#if 0 //RRK get a list of audio devices and the default
	foreach (const QAudioDeviceInfo &deviceInfo, QAudioDeviceInfo::availableDevices(QAudio::AudioOutput)) {
	     qDebug() << "l:" << deviceInfo.deviceName();
	}

	QAudioDeviceInfo info = QAudioDeviceInfo::defaultOutputDevice();
	qDebug() << "res:" << info.deviceName();
#endif

	//Setup fixed format for sound ouput
	m_OutAudioFormat.setCodec("audio/pcm");
	//m_OutAudioFormat.setFrequency(SOUNDCARD_RATE);
	m_OutAudioFormat.setSampleRate(SOUNDCARD_RATE);
	m_OutAudioFormat.setSampleSize(16);
	m_OutAudioFormat.setSampleType(QAudioFormat::SignedInt);
	m_OutAudioFormat.setByteOrder(QAudioFormat::LittleEndian);
	if(m_StereoOut)
		//RRK m_OutAudioFormat.setChannels(2);
		m_OutAudioFormat.setChannelCount(2);
	else
		m_OutAudioFormat.setChannelCount(1);

	m_pAudioOutput = new QAudioOutput(m_OutDeviceInfo, m_OutAudioFormat, this);
	if(!m_pAudioOutput)
	{
		qDebug()<<"Soundcard output error";
		return false;
	}
	if(QAudio::NoError == m_pAudioOutput->error() )
	{
		//initialize the data queue variables
		m_UserDataRate = 1;	//force user data rate to be changed
		ChangeUserDataRate(UsrDataRate);
		m_pOutput = m_pAudioOutput->start(); //start QT AudioOutput

		//RRK workaround for default, for some reason choosing default
		//sets the master volume to max!
		if (-1 == OutDevIndx) SetAlsaMasterVolume(50);

		//determine how long to sleep between low level reads based on samplerate and period size
		m_BlockTime = ( 250*m_pAudioOutput->periodSize() )/
						( SOUNDCARD_RATE*m_OutAudioFormat.channelCount() );
						//RRK ( SOUNDCARD_RATE*m_OutAudioFormat.channels() );
//qDebug()<<"periodSize "<<m_pAudioOutput->periodSize();
//qDebug()<<"BlockTime "<<m_BlockTime;
		m_ThreadQuit = FALSE;
		start(QThread::HighestPriority);	//start worker thread and set its priority
//		start(QThread::TimeCriticalPriority);	//start worker thread and set its priority
		return true;
	}
	else
	{
		qDebug()<<"Soundcard output error";
		return false;
	}
}

/////////////////////////////////////////////////////////////////////
// Closes down sound card output thread
/////////////////////////////////////////////////////////////////////
void CSoundOut::Stop()
{
	if(!m_ThreadQuit)
	{
		m_ThreadQuit = TRUE;
		m_pAudioOutput->stop();
		wait(500);
	}
	if(NULL != m_pAudioOutput)
	{
		delete m_pAudioOutput;
		m_pAudioOutput = NULL;
	}
}

/////////////////////////////////////////////////////////////////////
// Sets/changes user data input rate
/////////////////////////////////////////////////////////////////////
void CSoundOut::ChangeUserDataRate(double UsrDataRate)
{
	if(m_UserDataRate != UsrDataRate)
	{
		m_UserDataRate = UsrDataRate;
		for(int i=0; i<OUTQSIZE ;i++)	//zero buffer for data output
		{
			m_OutQueueMono[i] = 0;
			m_OutQueueStereo[i].re = 0;
			m_OutQueueStereo[i].im = 0;
		}
		m_OutRatio = m_UserDataRate/m_OutAudioFormat.sampleRate();
		m_OutQHead = 0;
		m_OutQTail = 0;
		m_OutQLevel = 0;
		m_AveOutQLevel = OUTQSIZE/2;
		m_Startup = true;
	}
qDebug()<<"SoundOutRatio  Rate"<<(1.0/m_OutRatio) << m_UserDataRate;
}

/////////////////////////////////////////////////////////////////////
// Sets/changes volume control gain  0 <= vol <= 99
//range scales to attenuation(gain) of -50dB to 0dB
/////////////////////////////////////////////////////////////////////
void CSoundOut::SetVolume(qint32 vol)
{
	m_Mutex.lock();
	if(0==vol)	//if zero make infinite attenuation
		m_Gain = 0.0;
	else if(vol<=99)
		m_Gain = pow(10.0, ((double)vol-99.0)/39.2 );
	m_Mutex.unlock();
//qDebug()<<"Volume "<<vol << m_Gain;
}

////////////////////////////////////////////////////////////////
//Called by application to put COMPLEX input into
// STEREO 2 channel soundcard output queue
////////////////////////////////////////////////////////////////
void CSoundOut::PutOutQueue(int numsamples, TYPECPX* pData )
{
TYPESTEREO16 RData[OUTQSIZE];	//buffer to hold resampled data
int i;
bool overflow = false;
	if(( 0==numsamples) || m_ThreadQuit)
		return;
	//Call Resampler to match sample rates between radio and sound card
	numsamples = m_OutResampler.Resample(numsamples, TEST_ERROR*m_OutRatio *(1.0+m_RateCorrection),
										 pData, RData, m_Gain);

	if(m_BlockingMode)	//if in Blocking Mode then wait for soundcard queue to be available
	{
		for( i=0; i<numsamples; i++)
		{
			while( ((m_OutQHead+1) & (OUTQSIZE-1)) == m_OutQTail)
				msleep(10);	//wait if Queue is full
			m_OutQueueStereo[m_OutQHead++] = RData[i];
			m_OutQHead &= (OUTQSIZE-1);
			m_OutQLevel++;
		}
	}
	else
	{	//here if non-blocking mode so need to deal with possible over/under flow
		m_Mutex.lock();
		for( i=0; i<numsamples; i++)
		{
			m_OutQueueStereo[m_OutQHead++] = RData[i];
			m_OutQHead &= (OUTQSIZE-1);
			m_OutQLevel++;
			if(m_OutQHead==m_OutQTail)	//if full
			{	//remove 1/4 a queue's worth of data
				m_OutQTail += OUTQSIZE/4;
				m_OutQTail &= (OUTQSIZE-1);
				m_OutQLevel -= OUTQSIZE/4;
				i = numsamples;		//force break out of for loop
				overflow = true;
			}
		}
		if(overflow)
		{
			qDebug()<<"Snd Overflow";
			m_AveOutQLevel = m_OutQLevel;
		}
		//calculate average Queue fill level
		m_AveOutQLevel = (1.0-FILTERQLEVEL_ALPHA)*m_AveOutQLevel + FILTERQLEVEL_ALPHA*(double)m_OutQLevel;
		m_Mutex.unlock();
	}
}

////////////////////////////////////////////////////////////////
//Called by application to put REAL soundcard output samples
//into MONO soundcard queue
////////////////////////////////////////////////////////////////
void CSoundOut::PutOutQueue(int numsamples, TYPEREAL* pData )
{
TYPEMONO16 RData[OUTQSIZE];	//buffer to hold resampled data
int i;
bool overflow = false;
	if(( 0==numsamples) || m_ThreadQuit)
		return;

	//Call Resampler to match sample rates between radio and sound card
	numsamples = m_OutResampler.Resample(numsamples, TEST_ERROR*m_OutRatio *(1.0+m_RateCorrection),
										 pData, RData, m_Gain);

	if(m_BlockingMode)	//if in Blocking Mode then wait for soundcard queue to be available
	{
		for( i=0; i<numsamples; i++)
		{
			while( ((m_OutQHead+1) & (OUTQSIZE-1)) == m_OutQTail)
				msleep(10);	//wait if Queue is full
			m_OutQueueMono[m_OutQHead++] = RData[i];
			m_OutQHead &= (OUTQSIZE-1);
			m_OutQLevel++;
		}
	}
	else
	{	//here if non-blocking mode so need to deal with possible over/under flow
		m_Mutex.lock();
		for( i=0; i<numsamples; i++)
		{
			m_OutQueueMono[m_OutQHead++] = RData[i];
			m_OutQHead &= (OUTQSIZE-1);
			m_OutQLevel++;
			if(m_OutQHead==m_OutQTail)	//if full
			{	//remove 1/4 a queue's worth of data
				m_OutQTail += OUTQSIZE/4;
				m_OutQTail &= (OUTQSIZE-1);
				m_OutQLevel -= OUTQSIZE/4;
				i = numsamples;		//force break out of for loop
				overflow = true;
			}
		}
		if(overflow)
		{
			qDebug()<<"Snd Overflow";
			m_AveOutQLevel = m_OutQLevel;
		}
		//calculate average Queue fill level
		m_AveOutQLevel = (1.0-FILTERQLEVEL_ALPHA)*m_AveOutQLevel + FILTERQLEVEL_ALPHA*(double)m_OutQLevel;
		m_Mutex.unlock();
	}
}

////////////////////////////////////////////////////////////////
//Called by CSoundOut worker thread to get new samples from queue
// This routine is called from a worker thread so must be careful.
//   MONO version
////////////////////////////////////////////////////////////////
void CSoundOut::GetOutQueue(int numsamples, TYPEMONO16* pData )
{
int i;
bool underflow = false;
	m_Mutex.lock();
	if(m_Startup)
	{	//if no data in queue yet just stuff in silence until something is put in queue
		for( i=0; i<numsamples; i++)
			pData[i] = 0;
		if(m_OutQLevel>OUTQSIZE/2)
		{
			m_Startup = false;
			m_RateUpdateCount = -5*SOUNDCARD_RATE;	//delay first error update to let settle
			m_PpmError = 0;
			m_AveOutQLevel = m_OutQLevel;
			m_UpdateToggle = true;
			}
		else
		{
			m_Mutex.unlock();
			return;
		}
	}

	for( i=0; i<numsamples; i++)
	{
		if(m_OutQHead!=m_OutQTail)
		{
			pData[i] = m_OutQueueMono[m_OutQTail++];
			m_OutQTail &= (OUTQSIZE-1);
			m_OutQLevel--;
		}
		else	//queue went empty
		{	//backup queue ptr and use previous data in queue
			m_OutQTail -= (OUTQSIZE/4);
			m_OutQTail &= (OUTQSIZE-1);
			pData[i] = m_OutQueueMono[m_OutQTail];
			m_OutQLevel += (OUTQSIZE/4);
			underflow = true;
		}
	}

	if(m_BlockingMode)
	{	//if in blocking mode just return
		m_Mutex.unlock();
		return;
	}

	//calculate average Queue fill level
	m_AveOutQLevel = (1.0-FILTERQLEVEL_ALPHA)*m_AveOutQLevel + FILTERQLEVEL_ALPHA*m_OutQLevel;
	if(underflow)
	{
		qDebug()<<"Snd Underflow";
		m_AveOutQLevel = m_OutQLevel;
	}

	// See if time to update rate error calculation routine
	m_RateUpdateCount += numsamples;
	if(m_RateUpdateCount >= SOUNDCARD_RATE)	//every second
	{
		CalcError();
		m_RateUpdateCount = 0;
	}
	m_Mutex.unlock();
}

////////////////////////////////////////////////////////////////
//Called by CSoundOut worker thread to get new samples from queue
// This routine is called from a worker thread so must be careful.
//   STEREO version
////////////////////////////////////////////////////////////////
void CSoundOut::GetOutQueue(int numsamples, TYPESTEREO16* pData )
{
int i;
bool underflow = false;
	m_Mutex.lock();
	if(m_Startup)
	{	//if no data in queue yet just stuff in silence until something is put in queue
		for( i=0; i<numsamples; i++)
		{
			pData[i].re = 0;
			pData[i].im = 0;
		}
		if(m_OutQLevel>OUTQSIZE/2)
		{
			m_Startup = false;
			m_RateUpdateCount = -5*SOUNDCARD_RATE;	//delay first error update to let settle
			m_PpmError = 0;
			m_AveOutQLevel = m_OutQLevel;
			m_UpdateToggle = true;
			}
		else
		{
			m_Mutex.unlock();
			return;
		}
	}

	for( i=0; i<numsamples; i++)
	{
		if(m_OutQHead!=m_OutQTail)
		{
			pData[i] = m_OutQueueStereo[m_OutQTail++];
			m_OutQTail &= (OUTQSIZE-1);
			m_OutQLevel--;
		}
		else	//queue went empty
		{	//backup queue ptr and use previous data in queue
			m_OutQTail -= (OUTQSIZE/4);
			m_OutQTail &= (OUTQSIZE-1);
			pData[i] = m_OutQueueStereo[m_OutQTail];
			m_OutQLevel += (OUTQSIZE/4);
			underflow = true;
		}
	}
	if(m_BlockingMode)
	{	//if in blocking mode just return
		m_Mutex.unlock();
		return;
	}
	//calculate average Queue fill level
	m_AveOutQLevel = (1.0-FILTERQLEVEL_ALPHA)*m_AveOutQLevel + FILTERQLEVEL_ALPHA*m_OutQLevel;

	if(underflow)
	{
		qDebug()<<"Snd Underflow";
		m_AveOutQLevel = m_OutQLevel;
	}
	// See if time to update rate error calculation routine
	m_RateUpdateCount += numsamples;
	if(m_RateUpdateCount >= SOUNDCARD_RATE)	//every second
	{
		CalcError();
		m_RateUpdateCount = 0;
	}
	m_Mutex.unlock();
}

////////////////////////////////////////////////////////////////
// Called alternately from the Get routines to update the
// error correction process
////////////////////////////////////////////////////////////////
void CSoundOut::CalcError()
{
double error;
	error = (double)(m_AveOutQLevel - OUTQSIZE/2 );	//neg==level is too low  pos == level is to high
	error = error * P_GAIN;
	m_RateCorrection = error;
	m_PpmError = (int)( m_RateCorrection*1e6 );
	if( abs(m_PpmError) > 500)
	{
//		qDebug()<<"SoundOut "<<m_PpmError << m_AveOutQLevel;
	}
}

//////////////////////////////////////////////////////////////////////////
// Worker thread polls QAudioOutput device to see if there is room
// to put more data into it and then calls GetOutQueue(..) to get more data.
// This thread was needed because the normal "pull" mechanism of Qt does
// not work very well since it depends on the main process signal-slot event
// queue and you get dropouts if the GUI gets busy.
//////////////////////////////////////////////////////////////////////////
void CSoundOut::run()
{
	while(!m_ThreadQuit )	//execute loop until quit flag set
	{
		if( (QAudio::IdleState == m_pAudioOutput->state() ) ||
			(QAudio::ActiveState == m_pAudioOutput->state() ) )
		{	//Process sound data while soundcard is active and no errors
			unsigned int len =  m_pAudioOutput->bytesFree();	//in bytes
			if( len>0 )
			{
				//limit size to SOUND_WRITEBUFSIZE
				if(len > SOUND_WRITEBUFSIZE)
					len = SOUND_WRITEBUFSIZE;
				if(m_StereoOut)
				{
					len &= ~(0x03);	//keep on 4 byte chunks
					GetOutQueue( len/4, (TYPESTEREO16*)m_pData );
				}
				else
				{
					len &= ~(0x01);	//keep on 2 byte chunks
					GetOutQueue( len/2, (TYPEMONO16*)m_pData );
				}
				m_pOutput->write((char*)m_pData,len);
			}
			else	//no room in sound card output buffer so wait
			{		//not good but no other wait or blocking mechanism available
				msleep(m_BlockTime);
			}
		}
		else
		{	//bail out if error occurs
			qDebug()<<"SoundOut Error";
#if 0 //RRK
			if(m_pParent)
				((CSdrInterface*)m_pParent)->SendIOStatus(CSdrInterface::ERROR);
#endif
			m_ThreadQuit = true;
		}
	}
	qDebug()<<"sound thread exit";
}
