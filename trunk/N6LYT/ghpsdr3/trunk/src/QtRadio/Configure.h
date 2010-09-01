/* 
 * File:   Configure.h
 * Author: john
 *
 * Created on 16 August 2010, 20:03
 */

#ifndef _CONFIGURE_H
#define	_CONFIGURE_H

#include <QSettings>
#include <QDebug>
#include <QtMultimedia/QAudioFormat>
#include <QtMultimedia/QAudioDeviceInfo>

#include "ui_Configure.h"

#include "Audio.h"

class Configure : public QDialog {
    Q_OBJECT
public:
    Configure();
    virtual ~Configure();
    void initAudioDevices(Audio* audio);

    void loadSettings(QSettings* settings);
    void saveSettings(QSettings* settings);

    void connected(bool state);

    QString getHost();
    int getReceiver();
    
    int getSpectrumHigh();
    int getSpectrumLow();
    int getFps();

    void setSpectrumHigh(int high);
    void setSpectrumLow(int low);

    int getWaterfallHigh();
    int getWaterfallLow();

    void setWaterfallHigh(int high);
    void setWaterfallLow(int low);

    QAudioFormat::Endian getByteOrder();
    int getSampleRate();
    int getChannels();
    
signals:
    void hostChanged(QString host);
    void receiverChanged(int receiver);
    void spectrumHighChanged(int high);
    void spectrumLowChanged(int low);
    void fpsChanged(int fps);
    void waterfallHighChanged(int high);
    void waterfallLowChanged(int low);
    void audioDeviceChanged(QAudioDeviceInfo info,int rate,int channels,QAudioFormat::Endian order);
//    void sampleRateChanged(int rate);
//    void channelsChanged(int channels);
//    void byteOrderChanged(QAudioFormat::Endian order);

public slots:
    void slotHostChanged(int selection);
    void slotReceiverChanged(int receiver);
    void slotSpectrumHighChanged(int high);
    void slotSpectrumLowChanged(int low);
    void slotFpsChanged(int fps);
    void slotWaterfallHighChanged(int high);
    void slotWaterfallLowChanged(int low);
    void slotAudioDeviceChanged(int selection);
    void slotSampleRateChanged(int rate);
    void slotChannelsChanged(int channels);
    void slotByteOrderChanged(int selection);

private:
    Ui::Configure widget;
};

#endif	/* _CONFIGURE_H */
