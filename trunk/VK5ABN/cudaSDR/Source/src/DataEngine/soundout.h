/*********************************************************************
 * soundout.h: interface for the CSoundOut class.
 *
 * History:
 *	2010-09-15  Initial creation MSW
 *	2011-03-27  Initial release
 ********************************************************************/
#ifndef SOUNDOUT_H
#define SOUNDOUT_H

#include <QObject>
#include <QThread>
#include <QList>
#include <QMutex>
#include <QAudioOutput>
#include "fractresampler.h"
#include <alsa/asoundlib.h>
#include <alsa/mixer.h>

#define OUTQSIZE 16384	//max samples (keep power of 2 for ptr wrap around)
#define SOUND_WRITEBUFSIZE 8192

class CSoundOut : public QThread
{
	Q_OBJECT
public:
	explicit CSoundOut(QObject *parent = 0);
	virtual ~CSoundOut();

	//Exposed functions
	bool Start(int OutDevIndx, bool StereoOut, double UsrDataRate, bool BlockingMode);	//starts soundcard output
	void Stop();	//stops soundcard output
	void PutOutQueue(int numsamples, TYPEREAL* pData );
	void PutOutQueue(int numsamples, TYPECPX* pData );
	void ChangeUserDataRate(double UsrDataRate);
	void SetVolume(qint32 vol);
	int GetRateError(){return (int)m_PpmError;}

protected:
	void run();		//implements worker thread loop
	int m_BlockTime;

private:
	void GetOutQueue(int numsamples, TYPEMONO16* pData );
	void GetOutQueue(int numsamples, TYPESTEREO16* pData );
	void CalcError();

	QList<QAudioDeviceInfo> m_OutDevices;
	QAudioDeviceInfo  m_OutDeviceInfo;
	QAudioFormat m_OutAudioFormat;
	QAudioOutput* m_pAudioOutput;
	QIODevice* m_pOutput; // ptr to internal soundout IODevice
	QObject* m_pParent;
	QMutex m_Mutex;
	CFractResampler m_OutResampler;

	TYPEMONO16 m_OutQueueMono[OUTQSIZE];
	TYPESTEREO16 m_OutQueueStereo[OUTQSIZE];
	bool m_BlockingMode;
	bool m_ThreadQuit;
	bool m_Startup;
	bool m_StereoOut;
	bool m_UpdateToggle;
	quint32 m_OutQHead;
	quint32 m_OutQTail;
	char m_pData[SOUND_WRITEBUFSIZE];
	int m_RateUpdateCount;
	int m_OutQLevel;
	int m_PpmError;
	double m_Gain;
	double m_UserDataRate;
	double m_OutRatio;
	double m_RateCorrection;
	double m_AveOutQLevel;
};
#endif // SOUNDOUT_H
