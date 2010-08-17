/* 
 * File:   UI.h
 * Author: john
 *
 * Created on 13 August 2010, 14:28
 */

#ifndef _UI_H
#define	_UI_H

#include "ui_UI.h"

#include <QTimer>

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
    void setGain(int gain);

    void actionSubRx();
    void setSubRxGain(int gain);

    void frequencyMoved(int increment);

    void spectrumHighChanged(int high);
    void spectrumLowChanged(int low);
    void fpsChanged(int f);
    void waterfallHighChanged(int high);
    void waterfallLowChanged(int low);

private:
    
    Ui::UI widget;
    Audio audio;
    int audio_device;
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

    int cwPitch;

    long long currentFrequency;

    int fps;

    QTimer* qTimer;

    QString command;

};

#endif	/* _UI_H */
