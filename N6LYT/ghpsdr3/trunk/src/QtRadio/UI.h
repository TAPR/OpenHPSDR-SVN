/* 
 * File:   UI.h
 * Author: john
 *
 * Created on 13 August 2010, 14:28
 */

#ifndef _UI_H
#define	_UI_H

#include "ui_UI.h"

#include <QDebug>
#include <QSettings>
#include <QTimer>
#include <QtMultimedia/QAudioFormat>

#include "Configure.h"
#include "Audio.h"
#include "Connection.h"
#include "Spectrum.h"
#include "Band.h"
#include "Mode.h"
#include "Filters.h"
#include "CWLFilters.h"
#include "CWUFilters.h"
#include "LSBFilters.h"
#include "USBFilters.h"
#include "DSBFilters.h"
#include "AMFilters.h"
#include "SAMFilters.h"
#include "FMNFilters.h"
#include "DIGLFilters.h"
#include "DIGUFilters.h"

#define AGC_LONG 1
#define AGC_SLOW 2
#define AGC_MEDIUM 3
#define AGC_FAST 4

class UI : public QMainWindow {
    Q_OBJECT
public:
    UI();
    virtual ~UI();
    void loadSettings();
    void saveSettings();
    void closeEvent(QCloseEvent* event);

    void sendCommand(QString command);

public slots:
    void actionConfigure();

    void actionConnect();
    void actionDisconnect();
    void actionSubRx();

    void actionMuteMainRx();
    void actionMuteSubRx();

    void action160();
    void action80();
    void action60();
    void action40();
    void action30();
    void action20();
    void action17();
    void action15();
    void action12();
    void action10();
    void action6();
    void actionGen();
    void actionWWV();

    void actionCWL();
    void actionCWU();
    void actionLSB();
    void actionUSB();
    void actionDSB();
    void actionAM();
    void actionSAM();
    void actionFMN();
    void actionDIGL();
    void actionDIGU();

    void actionFilter0();
    void actionFilter1();
    void actionFilter2();
    void actionFilter3();
    void actionFilter4();
    void actionFilter5();
    void actionFilter6();
    void actionFilter7();
    void actionFilter8();
    void actionFilter9();

    void actionANF();
    void actionNR();
    void actionNB();

    void actionSlow();
    void actionMedium();
    void actionFast();
    void actionLong();

    void connected();
    void disconnected(QString message);
    void receivedHeader(char* header);

    void bandChanged(int previousBand,int newBand);
    void modeChanged(int previousMode,int newMode);
    void filtersChanged(FiltersBase* previousFilters,FiltersBase* newFilters);
    void filterChanged(int previousFilter,int newFilter);
    void frequencyChanged(long long frequency);

    void updateSpectrum();

    void audioChanged(int choice);
    void sampleRateChanged(int choice);
    void audioChannelsChanged(int channels);
    void encodingChanged(int choice);
    void byteOrderChanged(int choice);
    void setGain(int gain);


    void setSubRxGain(int gain);

    void frequencyMoved(int increment,int step);

    void spectrumHighChanged(int high);
    void spectrumLowChanged(int low);
    void fpsChanged(int f);
    void waterfallHighChanged(int high);
    void waterfallLowChanged(int low);

    void hostChanged(int choice);
    void rxChanged(int rx);

signals:
    void subRxStateChanged(bool state);

private:
    void setSubRxPan();

    Ui::UI widget;
    Audio audio;
    int audio_device;
    int audio_sample_rate;
    int audio_channels;
    int audio_buffers;
    QAudioFormat::Endian audio_byte_order;
    char* first_audio_buffer;
    int gain;
    int subRxGain;
    bool subRx;
    long long subRxFrequency;
    Connection connection;
    Band band;
    Mode mode;
    Filters filters;
    CWLFilters cwlFilters;
    CWUFilters cwuFilters;
    LSBFilters lsbFilters;
    USBFilters usbFilters;
    DSBFilters dsbFilters;
    AMFilters amFilters;
    SAMFilters samFilters;
    FMNFilters fmnFilters;
    DIGUFilters diguFilters;
    DIGLFilters diglFilters;

    int agc;

    int cwPitch;

    long long frequency;

    int fps;

    QTimer* qTimer;

    QString command;

    Configure configure;
    int sampleRate;

};

#endif	/* _UI_H */
