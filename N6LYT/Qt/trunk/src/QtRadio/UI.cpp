/*
 * File:   UI.cpp
 * Author: John Melton, G0ORX/N6LYT
 *
 * Created on 13 August 2010, 14:28
 */

/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/

#include <QDebug>
#include <QSettings>
#include <QtNetwork/QHostAddress>

#include "UI.h"
#include "about.h"
#include "Configure.h"
#include "bands.h"
#include "Mode.h"
#include "FiltersBase.h"
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
#include "Xvtr.h"
#include "XvtrEntry.h"

UI::UI() {
    widget.setupUi(this);
    configFileName=getConfigDir()+"/"+QCoreApplication::applicationName()+".ini";
    settings=new QSettings(configFileName,QSettings::IniFormat);

    ab = new About(this);
    ab->setVersion("1.0.2");

    widget.gridLayout->setContentsMargins(2,2,2,2);
    widget.gridLayout->setVerticalSpacing(0);

    freqsender = new FreqSender();

    // connect up all the menus
    connect(widget.actionAbout,SIGNAL(triggered()),this,SLOT(actionAbout()));
    connect(widget.actionConnectToServer,SIGNAL(triggered()),this,SLOT(actionConnect()));
    connect(widget.actionDisconnectFromServer,SIGNAL(triggered()),this,SLOT(actionDisconnect()));

    connect(widget.actionSubrx,SIGNAL(triggered()),this,SLOT(actionSubRx()));
    connect(widget.actionBandscope,SIGNAL(triggered()),this,SLOT(actionBandscope()));
    connect(widget.actionRecord,SIGNAL(triggered()),this,SLOT(actionRecord()));

    connect(Connection::getInstance(),SIGNAL(isConnected()),this,SLOT(connected()));
    connect(Connection::getInstance(),SIGNAL(disconnected(QString)),this,SLOT(disconnected(QString)));
    connect(Connection::getInstance(),SIGNAL(spectrumBuffer(char*,char*)),this,SLOT(spectrumBuffer(char*,char*)));
    connect(Connection::getInstance(),SIGNAL(spectrum(QByteArray)),this,SLOT(spectrumBuffer(QByteArray)));
    connect(Connection::getInstance(),SIGNAL(connected()),this,SLOT(connected()));

    connect(Connection::getInstance(),SIGNAL(configBuffer(char*,char*)),this,SLOT(configBuffer(char*,char*)));

    connect(widget.actionConfig,SIGNAL(triggered()),this,SLOT(actionConfigure()));

    connect(widget.actionMuteMainRx,SIGNAL(triggered()),this,SLOT(actionMuteMainRx()));
    connect(widget.actionMuteSubRx,SIGNAL(triggered()),this,SLOT(actionMuteSubRx()));

    connect(widget.actionGain_10,SIGNAL(triggered()),this,SLOT(actionGain_10()));
    connect(widget.actionGain_20,SIGNAL(triggered()),this,SLOT(actionGain_20()));
    connect(widget.actionGain_30,SIGNAL(triggered()),this,SLOT(actionGain_30()));
    connect(widget.actionGain_40,SIGNAL(triggered()),this,SLOT(actionGain_40()));
    connect(widget.actionGain_50,SIGNAL(triggered()),this,SLOT(actionGain_50()));
    connect(widget.actionGain_60,SIGNAL(triggered()),this,SLOT(actionGain_60()));
    connect(widget.actionGain_70,SIGNAL(triggered()),this,SLOT(actionGain_70()));
    connect(widget.actionGain_80,SIGNAL(triggered()),this,SLOT(actionGain_80()));
    connect(widget.actionGain_90,SIGNAL(triggered()),this,SLOT(actionGain_90()));
    connect(widget.actionGain_100,SIGNAL(triggered()),this,SLOT(actionGain_100()));

    connect(widget.actionSquelchEnable,SIGNAL(triggered()),this,SLOT(actionSquelch()));
    connect(widget.actionSquelchReset,SIGNAL(triggered()),this,SLOT(actionSquelchReset()));

    connect(widget.actionKeypad, SIGNAL(triggered()),this,SLOT(actionKeypad()));
    connect(&keypad,SIGNAL(setKeypadFrequency(quint64)),this,SLOT(setKeypadFrequency(quint64)));

    connect(widget.actionCWL,SIGNAL(triggered()),this,SLOT(actionCWL()));
    connect(widget.actionCWU,SIGNAL(triggered()),this,SLOT(actionCWU()));
    connect(widget.actionLSB,SIGNAL(triggered()),this,SLOT(actionLSB()));
    connect(widget.actionUSB,SIGNAL(triggered()),this,SLOT(actionUSB()));
    connect(widget.actionDSB,SIGNAL(triggered()),this,SLOT(actionDSB()));
    connect(widget.actionAM,SIGNAL(triggered()),this,SLOT(actionAM()));
    connect(widget.actionSAM,SIGNAL(triggered()),this,SLOT(actionSAM()));
    connect(widget.actionFMN,SIGNAL(triggered()),this,SLOT(actionFMN()));
    connect(widget.actionDIGL,SIGNAL(triggered()),this,SLOT(actionDIGL()));
    connect(widget.actionDIGU,SIGNAL(triggered()),this,SLOT(actionDIGU()));

    connect(widget.actionFilter_0,SIGNAL(triggered()),this,SLOT(actionFilter0()));
    connect(widget.actionFilter_1,SIGNAL(triggered()),this,SLOT(actionFilter1()));
    connect(widget.actionFilter_2,SIGNAL(triggered()),this,SLOT(actionFilter2()));
    connect(widget.actionFilter_3,SIGNAL(triggered()),this,SLOT(actionFilter3()));
    connect(widget.actionFilter_4,SIGNAL(triggered()),this,SLOT(actionFilter4()));
    connect(widget.actionFilter_5,SIGNAL(triggered()),this,SLOT(actionFilter5()));
    connect(widget.actionFilter_6,SIGNAL(triggered()),this,SLOT(actionFilter6()));
    connect(widget.actionFilter_7,SIGNAL(triggered()),this,SLOT(actionFilter7()));
    connect(widget.actionFilter_8,SIGNAL(triggered()),this,SLOT(actionFilter8()));
    connect(widget.actionFilter_9,SIGNAL(triggered()),this,SLOT(actionFilter9()));

    connect(widget.actionANF,SIGNAL(triggered()),this,SLOT(actionANF()));
    connect(widget.actionNR,SIGNAL(triggered()),this,SLOT(actionNR()));
    connect(widget.actionNB,SIGNAL(triggered()),this,SLOT(actionNB()));
    connect(widget.actionSDROM,SIGNAL(triggered()),this,SLOT(actionSDROM()));

    connect(widget.actionPolyphase,SIGNAL(triggered()),this,SLOT(actionPolyphase()));

    connect(widget.actionLong,SIGNAL(triggered()),this,SLOT(actionLong()));
    connect(widget.actionSlow,SIGNAL(triggered()),this,SLOT(actionSlow()));
    connect(widget.actionMedium,SIGNAL(triggered()),this,SLOT(actionMedium()));
    connect(widget.actionFast,SIGNAL(triggered()),this,SLOT(actionFast()));


    connect(widget.actionPreamp,SIGNAL(triggered()),this,SLOT(actionPreamp()));

    connect(widget.actionBookmarkThisFrequency,SIGNAL(triggered()),this,SLOT(actionBookmark()));
    connect(widget.actionEditBookmarks,SIGNAL(triggered()),this,SLOT(editBookmarks()));

    connect(widget.actionMOX,SIGNAL(triggered()),this,SLOT(actionMox()));
    connect(widget.pushButtonMox,SIGNAL(clicked()),this,SLOT(actionMox()));
    connect(widget.actionRemoteMic,SIGNAL(triggered()),this,SLOT(actionRemoteMic()));

    // connect up band and frequency changes
    connect(&bands,SIGNAL(bandSelected(int)),this,SLOT(bandChanged(int)));
//    connect(&band,SIGNAL(frequencyChanged(quint64)),this,SLOT(frequencyChanged(quint64)));

    // connect up mode changes
    connect(&mode,SIGNAL(modeChanged(int,int)),this,SLOT(modeChanged(int,int)));

    // connect up filter changes
    connect(&filters,SIGNAL(filtersChanged(FiltersBase*,FiltersBase*)),this,SLOT(filtersChanged(FiltersBase*,FiltersBase*)));
    connect(&filters,SIGNAL(filterChanged(int,int)),this,SLOT(filterChanged(int,int)));

    // connect up spectrum frame
    connect(widget.spectrumFrame, SIGNAL(frequencyMoved(int,int)),
            this, SLOT(frequencyMoved(int,int)));
    connect(widget.spectrumFrame, SIGNAL(frequencyChanged(quint64)),
            this, SLOT(frequencyChanged(quint64)));
    connect(widget.spectrumFrame, SIGNAL(spectrumHighChanged(int)),
            this,SLOT(spectrumHighChanged(int)));
    connect(widget.spectrumFrame, SIGNAL(spectrumLowChanged(int)),
            this,SLOT(spectrumLowChanged(int)));
    connect(widget.spectrumFrame, SIGNAL(waterfallHighChanged(int)),
            this,SLOT(waterfallHighChanged(int)));
    connect(widget.spectrumFrame, SIGNAL(waterfallLowChanged(int)),
            this,SLOT(waterfallLowChanged(int)));

    connect(widget.spectrumFrame, SIGNAL(squelchValueChanged(int)),
            this,SLOT(squelchValueChanged(int)));


    // connect up waterfall frame
    connect(widget.waterfallFrame, SIGNAL(frequencyMoved(int,int)),
            this, SLOT(frequencyMoved(int,int)));

    // connect up configuration changes
    connect(&configure,SIGNAL(spectrumHighChanged(int)),this,SLOT(spectrumHighChanged(int)));
    connect(&configure,SIGNAL(spectrumLowChanged(int)),this,SLOT(spectrumLowChanged(int)));
    connect(&configure,SIGNAL(fpsChanged(int)),this,SLOT(fpsChanged(int)));
    connect(&configure,SIGNAL(waterfallHighChanged(int)),this,SLOT(waterfallHighChanged(int)));
    connect(&configure,SIGNAL(waterfallLowChanged(int)),this,SLOT(waterfallLowChanged(int)));
    connect(&configure,SIGNAL(waterfallAutomaticChanged(bool)),this,SLOT(waterfallAutomaticChanged(bool)));

    audio=new Audio();
    configure.initAudioDevices(audio);

    audio->init(10900);

    connect(&configure,SIGNAL(audioDeviceChanged(QAudioDeviceInfo,int,int,QAudioFormat::Endian,QString)),this,SLOT(audioDeviceChanged(QAudioDeviceInfo,int,int,QAudioFormat::Endian,QString)));

    connect(&configure,SIGNAL(hostChanged(QString)),this,SLOT(hostChanged(QString)));
    connect(&configure,SIGNAL(receiverChanged(int)),this,SLOT(receiverChanged(int)));

    connect(&configure,SIGNAL(nrValuesChanged(int,int,double,double)),this,SLOT(nrValuesChanged(int,int,double,double)));
    connect(&configure,SIGNAL(anfValuesChanged(int,int,double,double)),this,SLOT(anfValuesChanged(int,int,double,double)));
    connect(&configure,SIGNAL(nbThresholdChanged(double)),this,SLOT(nbThresholdChanged(double)));
    connect(&configure,SIGNAL(sdromThresholdChanged(double)),this,SLOT(sdromThresholdChanged(double)));

    connect(&bookmarks,SIGNAL(bookmarkSelected(QAction*)),this,SLOT(selectBookmark(QAction*)));
    connect(&bookmarkDialog,SIGNAL(accepted()),this,SLOT(addBookmark()));

    connect(&configure,SIGNAL(addXVTR(QString,quint64,quint64,quint64,quint64,int,int)),this,SLOT(addXVTR(QString,quint64,quint64,quint64,quint64,int,int)));
    connect(&configure,SIGNAL(deleteXVTR(int)),this,SLOT(deleteXVTR(int)));


    connect(&xvtr,SIGNAL(xvtrSelected(QAction*)),this,SLOT(selectXVTR(QAction*)));

    connect(&spectrumTimer,SIGNAL(timeout()),this,SLOT(updateSpectrum()));

    widget.actionConnectToServer->setEnabled(TRUE);
    widget.actionDisconnectFromServer->setDisabled(TRUE);

    bandscope=NULL;

    fps=15;
    gain=100;
    subRx=FALSE;
    subRxGain=100;
    agc=AGC_SLOW;
    cwPitch=600;
    squelch=false;
    squelchValue=-100;
    mox=false;
    ifFrequency=0LL;

    audio_device=0;
    audio_sample_rate=configure.getAudioSampleRate();
    audio_channels=configure.getChannels();
    audio_byte_order=configure.getByteOrder();

    audio->initialize_audio();

    // load any saved settings
    loadSettings();
    switch(agc) {
        case AGC_SLOW:
            widget.actionSlow->setChecked(TRUE);
            break;
        case AGC_MEDIUM:
            widget.actionMedium->setChecked(TRUE);
            break;
        case AGC_FAST:
            widget.actionFast->setChecked(TRUE);
            break;
        case AGC_LONG:
            widget.actionLong->setChecked(TRUE);
            break;
    }

    isConnected=false;

    fps=configure.getFps();

    widget.spectrumFrame->setHost(configure.getHost());
    widget.spectrumFrame->setReceiver(configure.getReceiver());

    widget.actionSubrx->setDisabled(TRUE);
    widget.actionMuteSubRx->setDisabled(TRUE);

    mode.setMode(MODE_USB);

}

UI::~UI() {
    //connection.disconnect();
}

void UI::resizeEvent(QResizeEvent* event) {
    int width=event->size().width();
    widget.pushButtonMox->move(width-widget.pushButtonMox->width()-5,
                               widget.pushButtonMox->pos().y());
}

void UI::actionAbout() {
    ab->aboutMessage();
}

void UI::loadSettings() {
    bands.loadSettings(settings);
    xvtr.loadSettings(settings);
    configure.loadSettings(settings);
    configure.updateXvtrList(&xvtr);
    bookmarks.loadSettings(settings);
    bookmarks.buildMenu(widget.menuView_Bookmarks,&bands);

    settings->beginGroup("UI");
    if(settings->contains("gain")) gain=subRxGain=settings->value("gain").toInt();
    if(settings->contains("agc")) agc=settings->value("agc").toInt();
    if(settings->contains("squelch")) squelchValue=settings->value("squelch").toInt();
    settings->endGroup();
}

void UI::saveSettings() {
    QString s;
    Bookmark* bookmark;

    settings->clear();

    configure.saveSettings(settings);
    bands.saveSettings(settings);
    xvtr.saveSettings(settings);
    bookmarks.saveSettings(settings);

    settings->beginGroup("UI");
    settings->setValue("gain",gain);
    settings->setValue("subRxGain",subRxGain);
    settings->setValue("agc",agc);
    settings->setValue("squelch",squelchValue);
    settings->endGroup();

}

void UI::hostChanged(QString host) {
    widget.spectrumFrame->setHost(host);
}

void UI::receiverChanged(int rx) {
    widget.spectrumFrame->setReceiver(rx);
}

void UI::closeEvent(QCloseEvent* event) {
    saveSettings();
}

void UI::actionConfigure() {
    configure.show();
}

void UI::spectrumHighChanged(int high) {
    //qDebug() << __FUNCTION__ << ": " << high;

    widget.spectrumFrame->setHigh(high);
    configure.setSpectrumHigh(high);
    bands.setSpectrumHigh(high);
}

void UI::spectrumLowChanged(int low) {
    //qDebug() << __FUNCTION__ << ": " << low;

    widget.spectrumFrame->setLow(low);
    configure.setSpectrumLow(low);
    bands.setSpectrumLow(low);
}

void UI::fpsChanged(int f) {
    //qDebug() << "fpsChanged:" << f;
    fps=f;
    if(spectrumTimer.isActive()) {
        spectrumTimer.setInterval(1000/fps);
    }

}

void UI::waterfallHighChanged(int high) {
    //qDebug() << __LINE__ << __FUNCTION__ << ": " << high;

    widget.waterfallFrame->setHigh(high);
    configure.setWaterfallHigh(high);
    bands.setWaterfallHigh(high);
}

void UI::waterfallLowChanged(int low) {
    //qDebug() << __FUNCTION__ << ": " << low;

    widget.waterfallFrame->setLow(low);
    configure.setWaterfallLow(low);
    bands.setWaterfallLow(low);
}

void UI::waterfallAutomaticChanged(bool state) {
    widget.waterfallFrame->setAutomatic(state);
}

void UI::audioDeviceChanged(QAudioDeviceInfo info,int rate,int channels,QAudioFormat::Endian byteOrder,QString encoding) {
    audio->select_audio(info,rate,channels,byteOrder,encoding);
}

void UI::encodingChanged(int choice) {
    // not supported yet
}

void UI::actionConnect() {
    qDebug() << "UI::actionConnect";
    widget.statusbar->clearMessage();
    Connection::getInstance()->connect(configure.getHost(), DSPSERVER_BASE_PORT+configure.getReceiver());

    qDebug()<<"disable connect";
    widget.actionConnectToServer->setDisabled(TRUE);
    widget.actionDisconnectFromServer->setEnabled(TRUE);
    widget.spectrumFrame->setHost(configure.getHost());
    widget.spectrumFrame->setReceiver(configure.getReceiver());

    // when we get the response a signal is sent to connected()

}

void UI::actionDisconnect() {
    qDebug() << "UI::actionDisconnect";

    Connection::getInstance()->disconnect();

    widget.actionConnectToServer->setEnabled(TRUE);
    widget.actionDisconnectFromServer->setDisabled(FALSE);
    widget.actionSubrx->setDisabled(TRUE);
    widget.actionMuteSubRx->setDisabled(TRUE);

    configure.connected(FALSE);
}

void UI::connected() {

    qDebug() << "UI::connected";

    QString command;

    configure.connected(TRUE);

    audio->reset();

    command.clear();
    command.append(QString("connect "));
    command.append(QString::number(10900+configure.getReceiver()));
    command.append(QString(" "));
    command.append(QString::number(audio->get_sample_rate()));
    command.append(QString(" "));
    command.append(QString::number(audio->get_channels()));
    command.append(QString(" "));
    switch(audio->get_encoding()) {
    case ENCODING_G711A:
        command.append(QString("G711a"));
        break;
    case ENCODING_G711U:
        command.append(QString("G711u"));
        break;
    case ENCODING_PCM:
        command.append(QString("PCM"));
        break;
    case ENCODING_G721:
        command.append(QString("G721"));
        break;
    }
    command.append(QString(" "));
    command.append(QString("QtRadio"));

    qDebug()<<"UI::connected: "<<command;
    Connection::getInstance()->sendCommand(command);



}

void UI::disconnected(QString message) {
    qDebug() << "UI::disconnected: " << message;

    widget.statusbar->showMessage(message,0);

    widget.actionConnectToServer->setEnabled(TRUE);
    widget.actionDisconnectFromServer->setDisabled(TRUE);
    widget.actionSubrx->setDisabled(TRUE);
    widget.actionMuteSubRx->setDisabled(TRUE);

    spectrumTimer.stop();

    configure.connected(FALSE);
}

void UI::updateSpectrum() {
    command.clear();
    command.append(QString("getspectrum "));
    command.append(QString::number(widget.spectrumFrame->width()));
    Connection::getInstance()->sendCommand(command);
}

void UI::spectrumBuffer(char* header,char* buffer) {
    int length=((header[3]&0xFF)<<8)+(header[4]&0xFF);
    sampleRate=((header[9]&0xFF)<<24)+((header[10]&0xFF)<<16)+((header[11]&0xFF)<<8)+(header[12]&0xFF);
    widget.spectrumFrame->updateSpectrum(header,buffer,length);
    widget.waterfallFrame->updateWaterfall(header,buffer,length);
}

void UI::configBuffer(char *header, char *buffer) {

    int length=((header[3]&0xFF)<<8)+(header[4]&0xFF);
    QByteArray config(buffer,length);

    qDebug()<<"configBuffer:"<<config;

    QDomDocument* configuration=new QDomDocument();
    configuration->setContent(config);

    configOptions(configuration);

    configure.setHardwareConfiguration(configuration);
    widget.spectrumFrame->setSampleRate(configure.getSampleRate());
    widget.waterfallFrame->setSampleRate(configure.getSampleRate());

    // setup bands based on frequency range
    bands.setupBands(configure.getMinFrequency(),configure.getMaxFrequency());
    bands.buildMenu(widget.menuHam,widget.menuBroadcast);

    xvtr.configure(configuration);
    xvtr.buildMenu(widget.menuXvtr);

    bookmarks.buildMenu(widget.menuView_Bookmarks,&bands);

    // send initial settings
    quint64 frequency=bands.getFrequency();
    QString command;
    command.clear();
    command.append(QString("frequency "));
    command.append(QString::number(frequency));
    command.append(QString(" "));
    command.append(QString::number(bands.getBand()));
    Connection::getInstance()->sendCommand(command);
    widget.spectrumFrame->setFrequency(frequency);
    widget.waterfallFrame->setFrequency(frequency);

    command.clear();
    command.append(QString("mode "));
    command.append(QString::number(bands.getMode()));
    Connection::getInstance()->sendCommand(command);

    int low,high;
    if(mode.getMode()==MODE_CWL) {
        low=-cwPitch-filters.getLow();
        high=-cwPitch+filters.getHigh();
    } else if(mode.getMode()==MODE_CWU) {
        low=cwPitch-filters.getLow();
        high=cwPitch+filters.getHigh();
    } else {
        low=filters.getLow();
        high=filters.getHigh();
    }
    command.clear();
    command.append(QString("filter "));
    command.append(QString::number(low));
    command.append(QString(" "));
    command.append(QString::number(high));
    Connection::getInstance()->sendCommand(command);

    qDebug() << "connected calling widget.spectrumFrame.setFilter";

    widget.spectrumFrame->setFilter(low,high);
    widget.waterfallFrame->setFilter(low,high);

    widget.actionSubrx->setEnabled(TRUE);
    widget.actionMuteSubRx->setDisabled(TRUE);


    audio_buffers=0;
    actionGain(gain);
    command.clear();
    command.append(QString("rxoutputgain "));
    command.append(QString::number(gain));
    Connection::getInstance()->sendCommand(command);

    command.clear();
    command.append(QString("pan "));
    command.append(QString::number(0.5));
    Connection::getInstance()->sendCommand(command);

    command.clear();
    command.append(QString("agc "));
    command.append(QString::number(agc));
    Connection::getInstance()->sendCommand(command);

    command.clear();
    command.append(QString("anfvals "));
    command.append(QString::number(configure.getAnfTaps()));
    command.append(QString(" "));
    command.append(QString::number(configure.getAnfDelay()));
    command.append(QString(" "));
    command.append(QString::number(configure.getAnfGain()));
    command.append(QString(" "));
    command.append(QString::number(configure.getAnfLeak()));
    Connection::getInstance()->sendCommand(command);

    command.clear();
    command.append(QString("nrvals "));
    command.append(QString::number(configure.getNrTaps()));
    command.append(QString(" "));
    command.append(QString::number(configure.getNrDelay()));
    command.append(QString(" "));
    command.append(QString::number(configure.getNrGain()));
    command.append(QString(" "));
    command.append(QString::number(configure.getNrLeak()));
    Connection::getInstance()->sendCommand(command);


    command.clear();
    command.append(QString("nbvals "));
    command.append(QString::number(configure.getNbThreshold()));
    Connection::getInstance()->sendCommand(command);

    command.clear();
    command.append(QString("anf "));
    command.append(boolToString(widget.actionANF->isChecked()));
    Connection::getInstance()->sendCommand(command);

    command.clear();
    command.append(QString("nr "));
    command.append(boolToString(widget.actionNR->isChecked()));
    Connection::getInstance()->sendCommand(command);

    command.clear();
    command.append(QString("nb "));
    command.append(boolToString(widget.actionNB->isChecked()));
    Connection::getInstance()->sendCommand(command);

    command.clear();
    command.append(QString("squelchstate "));
    command.append(QString("0"));
    Connection::getInstance()->sendCommand(command);

    qDebug()<<"UI::connected: calling updateSpectrum";
    spectrumTimer.start(1000/fps);
    updateSpectrum();

}

void UI::actionSubRx() {
    if(subRx) {
        // on, so turn off
        subRx=FALSE;
        widget.spectrumFrame->setSubRxState(FALSE);
        widget.waterfallFrame->setSubRxState(FALSE);
        widget.actionMuteSubRx->setChecked(FALSE);
        widget.actionMuteSubRx->setDisabled(TRUE);
    } else {
        subRx=TRUE;

        // check frequency in range
        int samplerate = widget.spectrumFrame->samplerate();
        quint64 frequency=bands.getFrequency();
        if ((subRxFrequency < (frequency - (samplerate / 2))) || (subRxFrequency > (frequency + (samplerate / 2)))) {
            subRxFrequency=bands.getFrequency();
        }
        widget.spectrumFrame->setSubRxState(TRUE);
        widget.waterfallFrame->setSubRxState(TRUE);
        command.clear();
        command.append(QString("subrxfrequency "));
        command.append(QString::number(frequency - subRxFrequency));
        Connection::getInstance()->sendCommand(command);

        setSubRxPan();

        widget.actionMuteSubRx->setEnabled(TRUE);
    }

    command.clear();
    command.append(QString("subrx "));
    command.append(boolToString(subRx));
    Connection::getInstance()->sendCommand(command);
}

void UI::setSubRxGain(int gain) {
    subRxGain=gain;
    command.clear();
    command.append(QString("subrxoutputgain "));
    command.append(QString::number(subRxGain));
    Connection::getInstance()->sendCommand(command);
}

void UI::actionKeypad() {
    keypad.clear();
    keypad.show();
}

void UI::setKeypadFrequency(quint64 f) {
    frequencyChanged(f);

    // clear the band for manually set frequency
    widget.spectrumFrame->setBand("");
}

void UI::bandChanged(int band) {
    qDebug() << "UI::bandChanged: " << band;

    // get the band setting
    mode.setMode(bands.getMode());
    frequency=bands.getFrequency();
    int samplerate = widget.spectrumFrame->samplerate();
    if(subRx) {
        if ((subRxFrequency < (frequency - (samplerate / 2))) || (subRxFrequency > (frequency + (samplerate / 2)))) {
            subRxFrequency=frequency;
        }
    }

    command.clear();
    command.append(QString("frequency "));
    command.append(QString::number(frequency));
    command.append(QString(" "));
    command.append(QString::number(band));
    Connection::getInstance()->sendCommand(command);

    widget.spectrumFrame->setFrequency(frequency);
    widget.spectrumFrame->setSubRxFrequency(subRxFrequency);
    widget.spectrumFrame->setHigh(bands.getSpectrumHigh());
    widget.spectrumFrame->setLow(bands.getSpectrumLow());
    widget.waterfallFrame->setFrequency(frequency);
    widget.waterfallFrame->setSubRxFrequency(subRxFrequency);
    widget.waterfallFrame->setHigh(bands.getWaterfallHigh());
    widget.waterfallFrame->setLow(bands.getWaterfallLow());


    widget.spectrumFrame->setBand(bands.getLabel()+" ("+QString::number(bands.getBandStackEntry())+")");
    BandLimit* limits=bands.getBandLimits(bands.getFrequency()-(samplerate/2),bands.getFrequency()+(samplerate/2));
    widget.spectrumFrame->setBandLimits(limits->getMin(),limits->getMax());

    ifFrequency=0LL;
}

void UI::modeChanged(int previousMode,int newMode) {

    qDebug() << "UI::modeChanged: " << previousMode << "," << newMode;
    // uncheck previous mode
    switch(previousMode) {
        case MODE_CWL:
            widget.actionCWL->setChecked(FALSE);
            break;
        case MODE_CWU:
            widget.actionCWU->setChecked(FALSE);
            break;
        case MODE_LSB:
            widget.actionLSB->setChecked(FALSE);
            break;
        case MODE_USB:
            widget.actionUSB->setChecked(FALSE);
            break;
        case MODE_DSB:
            widget.actionDSB->setChecked(FALSE);
            break;
        case MODE_AM:
            widget.actionAM->setChecked(FALSE);
            break;
        case MODE_SAM:
            widget.actionSAM->setChecked(FALSE);
            break;
        case MODE_FMN:
            widget.actionFMN->setChecked(FALSE);
            break;
        case MODE_DIGL:
            widget.actionDIGL->setChecked(FALSE);
            break;
        case MODE_DIGU:
            widget.actionDIGU->setChecked(FALSE);
            break;
    }

    // check the new mode and set the filters
    switch(newMode) {
        case MODE_CWL:
            widget.actionCWL->setChecked(TRUE);
            filters.selectFilters(&cwlFilters);
            freqsender->broadcastDatagram(QByteArray().append("mode CWL"));
            break;
        case MODE_CWU:
            widget.actionCWU->setChecked(TRUE);
            filters.selectFilters(&cwuFilters);
            freqsender->broadcastDatagram(QByteArray().append("mode CWU"));
            break;
        case MODE_LSB:
            widget.actionLSB->setChecked(TRUE);
            filters.selectFilters(&lsbFilters);
            freqsender->broadcastDatagram(QByteArray().append("mode LSB"));
            break;
        case MODE_USB:
            widget.actionUSB->setChecked(TRUE);
            filters.selectFilters(&usbFilters);
            freqsender->broadcastDatagram(QByteArray().append("mode USB"));
            break;
        case MODE_DSB:
            widget.actionDSB->setChecked(TRUE);
            filters.selectFilters(&dsbFilters);
            freqsender->broadcastDatagram(QByteArray().append("mode DSB"));
            break;
        case MODE_AM:
            widget.actionAM->setChecked(TRUE);
            filters.selectFilters(&amFilters);
            freqsender->broadcastDatagram(QByteArray().append("mode AM"));
            break;
        case MODE_SAM:
            widget.actionSAM->setChecked(TRUE);
            filters.selectFilters(&samFilters);
            freqsender->broadcastDatagram(QByteArray().append("mode SAM"));
            break;
        case MODE_FMN:
            widget.actionFMN->setChecked(TRUE);
            filters.selectFilters(&fmnFilters);
            freqsender->broadcastDatagram(QByteArray().append("mode FMN"));
            break;
        case MODE_DIGL:
            widget.actionDIGL->setChecked(TRUE);
            filters.selectFilters(&diglFilters);
            freqsender->broadcastDatagram(QByteArray().append("mode DIGL"));
            break;
        case MODE_DIGU:
            widget.actionDIGU->setChecked(TRUE);
            filters.selectFilters(&diguFilters);
            freqsender->broadcastDatagram(QByteArray().append("mode DIGU"));
            break;
    }

    widget.spectrumFrame->setMode(mode.getStringMode());
    command.clear();
    command.append(QString("mode "));
    command.append(QString::number(mode.getMode()));
    Connection::getInstance()->sendCommand(command);
  
}

void UI::filtersChanged(FiltersBase* previousFilters,FiltersBase* newFilters) {

    qDebug() << "UI::filtersChanged to " << newFilters->getText();
    // uncheck old filter
    if(previousFilters!=NULL) {
        switch (previousFilters->getSelected()) {
            case 0:
                widget.actionFilter_0->setChecked(FALSE);
                break;
            case 1:
                widget.actionFilter_1->setChecked(FALSE);
                break;
            case 2:
                widget.actionFilter_2->setChecked(FALSE);
                break;
            case 3:
                widget.actionFilter_3->setChecked(FALSE);
                break;
            case 4:
                widget.actionFilter_4->setChecked(FALSE);
                break;
            case 5:
                widget.actionFilter_5->setChecked(FALSE);
                break;
            case 6:
                widget.actionFilter_6->setChecked(FALSE);
                break;
            case 7:
                widget.actionFilter_7->setChecked(FALSE);
                break;
            case 8:
                widget.actionFilter_8->setChecked(FALSE);
                break;
            case 9:
                widget.actionFilter_9->setChecked(FALSE);
                break;
        }
    }

    // set the filter menu text
    widget.actionFilter_0->setText(newFilters->getText(0));
    widget.actionFilter_1->setText(newFilters->getText(1));
    widget.actionFilter_2->setText(newFilters->getText(2));
    widget.actionFilter_3->setText(newFilters->getText(3));
    widget.actionFilter_4->setText(newFilters->getText(4));
    widget.actionFilter_5->setText(newFilters->getText(5));
    widget.actionFilter_6->setText(newFilters->getText(6));
    widget.actionFilter_7->setText(newFilters->getText(7));
    widget.actionFilter_8->setText(newFilters->getText(8));
    widget.actionFilter_9->setText(newFilters->getText(9));

    // check new filter
    if(newFilters!=NULL) {
        switch (newFilters->getSelected()) {
            case 0:
                widget.actionFilter_0->setChecked(TRUE);
                break;
            case 1:
                widget.actionFilter_1->setChecked(TRUE);
                break;
            case 2:
                widget.actionFilter_2->setChecked(TRUE);
                break;
            case 3:
                widget.actionFilter_3->setChecked(TRUE);
                break;
            case 4:
                widget.actionFilter_4->setChecked(TRUE);
                break;
            case 5:
                widget.actionFilter_5->setChecked(TRUE);
                break;
            case 6:
                widget.actionFilter_6->setChecked(TRUE);
                break;
            case 7:
                widget.actionFilter_7->setChecked(TRUE);
                break;
            case 8:
                widget.actionFilter_8->setChecked(TRUE);
                break;
            case 9:
                widget.actionFilter_9->setChecked(TRUE);
                break;
        }
    }

    filters.selectFilter(filters.getFilter());
    widget.spectrumFrame->setFilter(filters.getText());

}

void UI::actionCWL() {
    mode.setMode(MODE_CWL);
    filters.selectFilters(&cwlFilters);
    bands.setMode(MODE_CWL);
}

void UI::actionCWU() {
    mode.setMode(MODE_CWU);
    filters.selectFilters(&cwuFilters);
    bands.setMode(MODE_CWU);
}

void UI::actionLSB() {
    mode.setMode(MODE_LSB);
    filters.selectFilters(&lsbFilters);
    bands.setMode(MODE_LSB);
}

void UI::actionUSB() {
    mode.setMode(MODE_USB);
    filters.selectFilters(&usbFilters);
    bands.setMode(MODE_USB);
}

void UI::actionDSB() {
    mode.setMode(MODE_DSB);
    filters.selectFilters(&dsbFilters);
    bands.setMode(MODE_DSB);
}

void UI::actionAM() {
    mode.setMode(MODE_AM);
    filters.selectFilters(&amFilters);
    bands.setMode(MODE_AM);
}

void UI::actionSAM() {
    mode.setMode(MODE_SAM);
    filters.selectFilters(&samFilters);
    bands.setMode(MODE_SAM);
}

void UI::actionFMN() {
    mode.setMode(MODE_FMN);
    filters.selectFilters(&fmnFilters);
    bands.setMode(MODE_FMN);
}

void UI::actionDIGL() {
    mode.setMode(MODE_DIGL);
    filters.selectFilters(&diglFilters);
    bands.setMode(MODE_DIGL);
}

void UI::actionDIGU() {
    mode.setMode(MODE_DIGU);
    filters.selectFilters(&diguFilters);
    bands.setMode(MODE_DIGU);
}

void UI::actionFilter0() {
    filters.selectFilter(0);
}

void UI::actionFilter1() {
    filters.selectFilter(1);
}

void UI::actionFilter2() {
    filters.selectFilter(2);
}

void UI::actionFilter3() {
    filters.selectFilter(3);
}

void UI::actionFilter4() {
    filters.selectFilter(4);
}

void UI::actionFilter5() {
    filters.selectFilter(5);
}

void UI::actionFilter6() {
    filters.selectFilter(6);
}

void UI::actionFilter7() {
    filters.selectFilter(7);
}

void UI::actionFilter8() {
    filters.selectFilter(8);
}

void UI::actionFilter9() {
    filters.selectFilter(9);
}

void UI::filterChanged(int previousFilter,int newFilter) {

    qDebug() << "UI::filterChanged: " << previousFilter << ":" << newFilter;

    int low,high;
    switch(previousFilter) {
        case 0:
            widget.actionFilter_0->setChecked(FALSE);
            break;
        case 1:
            widget.actionFilter_1->setChecked(FALSE);
            break;
        case 2:
            widget.actionFilter_2->setChecked(FALSE);
            break;
        case 3:
            widget.actionFilter_3->setChecked(FALSE);
            break;
        case 4:
            widget.actionFilter_4->setChecked(FALSE);
            break;
        case 5:
            widget.actionFilter_5->setChecked(FALSE);
            break;
        case 6:
            widget.actionFilter_6->setChecked(FALSE);
            break;
        case 7:
            widget.actionFilter_7->setChecked(FALSE);
            break;
        case 8:
            widget.actionFilter_8->setChecked(FALSE);
            break;
        case 9:
            widget.actionFilter_9->setChecked(FALSE);
            break;
    }

    switch(newFilter) {
        case 0:
            widget.actionFilter_0->setChecked(TRUE);
            break;
        case 1:
            widget.actionFilter_1->setChecked(TRUE);
            break;
        case 2:
            widget.actionFilter_2->setChecked(TRUE);
            break;
        case 3:
            widget.actionFilter_3->setChecked(TRUE);
            break;
        case 4:
            widget.actionFilter_4->setChecked(TRUE);
            break;
        case 5:
            widget.actionFilter_5->setChecked(TRUE);
            break;
        case 6:
            widget.actionFilter_6->setChecked(TRUE);
            break;
        case 7:
            widget.actionFilter_7->setChecked(TRUE);
            break;
        case 8:
            widget.actionFilter_8->setChecked(TRUE);
            break;
        case 9:
            widget.actionFilter_9->setChecked(TRUE);
            break;
    }

    if(mode.getMode()==MODE_CWL) {
        low=-cwPitch-filters.getLow();
        high=-cwPitch+filters.getHigh();
    } else if(mode.getMode()==MODE_CWU) {
        low=cwPitch-filters.getLow();
        high=cwPitch+filters.getHigh();
    } else {
        low=filters.getLow();
        high=filters.getHigh();
    }

    command.clear();
    command.append(QString("filter "));
    command.append(QString::number(low));
    command.append(QString(" "));
    command.append(QString::number(high) );
    Connection::getInstance()->sendCommand(command);
    widget.spectrumFrame->setFilter(low,high);
    widget.spectrumFrame->setFilter(filters.getText());
    widget.waterfallFrame->setFilter(low,high);
    bands.setFilter(newFilter);
}

void UI::frequencyChanged(quint64 f) {
    //qDebug() << __FUNCTION__ << ": " << f;

    frequency=f;
    bands.setFrequency(f);
    command.clear();
    command.append(QString("frequency "));
    command.append(QString::number(frequency));
    freqsender->broadcastDatagram(QByteArray().append(command));
    Connection::getInstance()->sendCommand(command);
    widget.spectrumFrame->setFrequency(f);
    widget.waterfallFrame->setFrequency(f);
}

void UI::frequencyMoved(int increment,int step) {
    qint64 f;
    if(subRx) {
        qint64 diff;
        qint64 frequency = bands.getFrequency();
        f=subRxFrequency+(qint64)(increment*step);
        int samplerate = widget.spectrumFrame->samplerate();
        if ((f >= (frequency - (samplerate / 2))) && (f <= (frequency + (samplerate / 2)))) {
            subRxFrequency = f;
        }
        diff = frequency - subRxFrequency;

        command.clear();
        command.append(QString("subrxfrequency "));
        command.append(QString::number(diff));
        Connection::getInstance()->sendCommand(command);
        widget.spectrumFrame->setSubRxFrequency(subRxFrequency);
        widget.waterfallFrame->setSubRxFrequency(subRxFrequency);
        setSubRxPan();

    } else {
        if(ifFrequency==0LL) {
            bands.setFrequency(bands.getFrequency()-(quint64)(increment*step));
            frequency=bands.getFrequency();
        } else {
            xvtr.setFrequency(xvtr.getFrequency()-(quint64)(increment*step));
            frequency=xvtr.getFrequency();
        }
        command.clear();
        command.append(QString("frequency "));
        command.append(QString::number(frequency));
        freqsender->broadcastDatagram(QByteArray().append(command));
        Connection::getInstance()->sendCommand(command);
        widget.spectrumFrame->setFrequency(frequency);
        widget.waterfallFrame->setFrequency(frequency);
    }
}

void UI::actionANF() {
    command.clear();
    command.append(QString("anf "));
    command.append(boolToString(widget.actionANF->isChecked()));
    Connection::getInstance()->sendCommand(command);
}

void UI::actionNR() {
    command.clear();
    command.append(QString("nr "));
    command.append(boolToString(widget.actionNR->isChecked()));
    Connection::getInstance()->sendCommand(command);
}

void UI::actionNB() {
    command.clear();
    command.append(QString("nb "));
    command.append(boolToString(widget.actionNB->isChecked()));
    Connection::getInstance()->sendCommand(command);
}

void UI::actionSDROM() {
    command.clear();
    command.append(QString("sdrom "));
    command.append(boolToString(widget.actionSDROM->isChecked()));
    Connection::getInstance()->sendCommand(command);
}

void UI::actionPolyphase() {
    command.clear();
    command.append(QString("polyphase "));
    command.append(boolToString(widget.actionPolyphase->isChecked()));
    Connection::getInstance()->sendCommand(command);
}

void UI::actionSlow() {
    // reset the current selection
    switch(agc) {
    case AGC_LONG:
        widget.actionLong->setChecked(FALSE);
        break;
    case AGC_SLOW:
        widget.actionSlow->setChecked(FALSE);
        break;
    case AGC_MEDIUM:
        widget.actionMedium->setChecked(FALSE);
        break;
    case AGC_FAST:
        widget.actionFast->setChecked(FALSE);
        break;
    }
    agc=AGC_SLOW;

    command.clear();
    command.append(QString("agc "));
    command.append(QString::number(agc));
    Connection::getInstance()->sendCommand(command);

}

void UI::actionMedium() {

    // reset the current selection
    switch(agc) {
    case AGC_LONG:
        widget.actionLong->setChecked(FALSE);
        break;
    case AGC_SLOW:
        widget.actionSlow->setChecked(FALSE);
        break;
    case AGC_MEDIUM:
        widget.actionMedium->setChecked(FALSE);
        break;
    case AGC_FAST:
        widget.actionFast->setChecked(FALSE);
        break;
    }
    agc=AGC_MEDIUM;

    command.clear();
    command.append(QString("agc "));
    command.append(QString::number(agc));
    Connection::getInstance()->sendCommand(command);

}

void UI::actionFast() {
    // reset the current selection
    switch(agc) {
    case AGC_LONG:
        widget.actionLong->setChecked(FALSE);
        break;
    case AGC_SLOW:
        widget.actionSlow->setChecked(FALSE);
        break;
    case AGC_MEDIUM:
        widget.actionMedium->setChecked(FALSE);
        break;
    case AGC_FAST:
        widget.actionFast->setChecked(FALSE);
        break;
    }
    agc=AGC_FAST;

    command.clear();
    command.append(QString("agc "));
    command.append(QString::number(agc));
    Connection::getInstance()->sendCommand(command);
}

void UI::actionLong() {
    // reset the current selection
    switch(agc) {
    case AGC_LONG:
        widget.actionLong->setChecked(FALSE);
        break;
    case AGC_SLOW:
        widget.actionSlow->setChecked(FALSE);
        break;
    case AGC_MEDIUM:
        widget.actionMedium->setChecked(FALSE);
        break;
    case AGC_FAST:
        widget.actionFast->setChecked(FALSE);
        break;
    }
    agc=AGC_LONG;

    command.clear();
    command.append(QString("agc "));
    command.append(QString::number(agc));
    Connection::getInstance()->sendCommand(command);
}

void UI::setSubRxPan() {
    float pan;

    // set the pan relative to frequency position (only works when in stereo)
    pan=(float)(subRxFrequency-(frequency-(sampleRate/2)))/(float)sampleRate;

    if(pan<0.0) pan=0.0;
    if(pan>1.0) pan=1.0;
    command.clear();
    command.append(QString("subrxpan "));
    command.append(QString::number(pan));
    Connection::getInstance()->sendCommand(command);
}

void UI::actionMuteMainRx() {
    int g=gain;

    if(widget.actionMuteMainRx->isChecked()) {
        g=0;
    }

    command.clear();
    command.append(QString("rxoutputgain"));
    command.append(QString::number(g));
    Connection::getInstance()->sendCommand(command);
}

void UI::actionMuteSubRx() {
    int g=subRxGain;

    if(widget.actionMuteSubRx->isChecked()) {
        g=0;
    }

    command.clear();
    command.append(QString("rxoutputgain "));
    command.append(QString::number(g));
    Connection::getInstance()->sendCommand(command);

}

void UI::actionPreamp() {
    command.clear();
    command.append(QString("preamp "));
    command.append(boolToString(widget.actionPreamp->isChecked()));
    Connection::getInstance()->sendCommand(command);
}

void UI::actionBandscope() {
    if(widget.actionBandscope->isChecked()) {
        if(bandscope==NULL) bandscope=new Bandscope();
        bandscope->setWindowTitle("QtRadio Bandscope");
        bandscope->show();
        bandscope->connect(configure.getHost());
    } else {
        if(bandscope!=NULL) {
            bandscope->setVisible(FALSE);
            bandscope->disconnect();
        }
    }
}

void UI::actionRecord() {
}

void UI::actionGain_10() {
    actionGain(10);
}

void UI::actionGain_20() {
    actionGain(20);
}

void UI::actionGain_30() {
    actionGain(30);
}

void UI::actionGain_40() {
    actionGain(40);
}

void UI::actionGain_50() {
    actionGain(50);
}

void UI::actionGain_60() {
    actionGain(60);
}

void UI::actionGain_70() {
    actionGain(70);
}

void UI::actionGain_80() {
    actionGain(80);
}

void UI::actionGain_90() {
    actionGain(90);
}

void UI::actionGain_100() {
    actionGain(100);
}

void UI::actionGain(int g) {
    setGain(false);
    gain=g;
    subRxGain=g;
    setGain(true);
    command.clear();
    command.append(QString("rxoutputgain "));
    command.append(QString::number(gain));
    Connection::getInstance()->sendCommand(command);
    command.clear();
    command.append(QString("subrxoutputgain "));
    command.append(QString::number(subRxGain));
    Connection::getInstance()->sendCommand(command);
}

void UI::setGain(bool state) {
    switch(gain) {
    case 10:
        widget.actionGain_10->setChecked(state);
        break;
    case 20:
        widget.actionGain_20->setChecked(state);
        break;
    case 30:
        widget.actionGain_30->setChecked(state);
        break;
    case 40:
        widget.actionGain_40->setChecked(state);
        break;
    case 50:
        widget.actionGain_50->setChecked(state);
        break;
    case 60:
        widget.actionGain_60->setChecked(state);
        break;
    case 70:
        widget.actionGain_70->setChecked(state);
        break;
    case 80:
        widget.actionGain_80->setChecked(state);
        break;
    case 90:
        widget.actionGain_90->setChecked(state);
        break;
    case 100:
        widget.actionGain_100->setChecked(state);
        break;
    }
}

void UI::nrValuesChanged(int taps,int delay,double gain,double leakage) {
    command.clear();
    command.append(QString("nrvals "));
    command.append(QString::number(taps));
    command.append(QString(" "));
    command.append(QString::number(delay));
    command.append(QString(" "));
    command.append(QString::number(gain));
    command.append(QString(" "));
    command.append(QString::number(leakage));
    Connection::getInstance()->sendCommand(command);
}

void UI::anfValuesChanged(int taps,int delay,double gain,double leakage) {
    command.clear();
    command.append(QString("anfvals "));
    command.append(QString::number(taps));
    command.append(QString(" "));
    command.append(QString::number(delay));
    command.append(QString(" "));
    command.append(QString::number(gain));
    command.append(QString(" "));
    command.append(QString::number(leakage));
    Connection::getInstance()->sendCommand(command);
}

void UI::nbThresholdChanged(double threshold) {
    command.clear();
    command.append(QString("nbvals "));
    command.append(QString::number(threshold));
    Connection::getInstance()->sendCommand(command);
}

void UI::sdromThresholdChanged(double threshold) {
    command.clear();
    command.append(QString("sdromvals "));
    command.append(QString::number(threshold));
    Connection::getInstance()->sendCommand(command);
}

void UI::actionBookmark() {
    QString strFrequency=stringFrequency(frequency);
    bookmarkDialog.setTitle(strFrequency);
    bookmarkDialog.setBand(bands.getLabel());
    bookmarkDialog.setFrequency(strFrequency);
    bookmarkDialog.setMode(mode.getStringMode());
    bookmarkDialog.setFilter(filters.getText());
    bookmarkDialog.show();
}

void UI::addBookmark() {
    qDebug() << "addBookmark";
    Bookmark* bookmark=new Bookmark();
    bookmark->setTitle(bookmarkDialog.getTitle());
    bookmark->setBand(bands.getBand());
    bookmark->setFrequency(bands.getFrequency());
    bookmark->setMode(mode.getMode());
    bookmark->setFilter(filters.getFilter());
    bookmarks.add(bookmark);
    bookmarks.buildMenu(widget.menuView_Bookmarks,&bands);
}

void UI::selectBookmark(QAction* action) {

    bookmarks.select(action);

    bands.selectBand(bookmarks.getBand());

    frequency=bookmarks.getFrequency();
    bands.setFrequency(frequency);
    command.clear();
    command.append(QString("frequency "));
    command.append(QString::number(frequency));
    command.append(QString(" "));
    command.append(QString::number(bookmarks.getBand()));
    Connection::getInstance()->sendCommand(command);

    widget.spectrumFrame->setFrequency(frequency);
    widget.waterfallFrame->setFrequency(frequency);

    mode.setMode(bookmarks.getMode());

    filters.selectFilter(bookmarks.getFilter());

}

void UI::selectABookmark() {
/*
    int entry=bookmarksDialog->getSelected();
    if(entry>=0 && entry<bookmarks.count()) {
        selectBookmark(entry);
    }
*/
}

void UI::editBookmarks() {
    bookmarksEditDialog=new BookmarksEditDialog(this,&bookmarks);
    bookmarksEditDialog->setVisible(true);
    connect(bookmarksEditDialog,SIGNAL(bookmarkDeleted(int)),this,SLOT(bookmarkDeleted(int)));
    connect(bookmarksEditDialog,SIGNAL(bookmarkUpdated(int,QString)),this,SLOT(bookmarkUpdated(int,QString)));
    connect(bookmarksEditDialog,SIGNAL(bookmarkSelected(int)),this,SLOT(bookmarkSelected(int)));
}

void UI::bookmarkDeleted(int entry) {
    //qDebug() << "UI::bookmarkDeleted: " << entry;
    bookmarks.remove(entry);
    bookmarks.buildMenu(widget.menuView_Bookmarks,&bands);
}

void UI::bookmarkUpdated(int entry,QString title) {
    if(entry>=0 && entry<bookmarks.count()) {
        Bookmark* bookmark=bookmarks.at(entry);
        bookmark->setTitle(title);
    }
}

void UI::bookmarkSelected(int entry) {

    //qDebug() << "UI::bookmarkSelected " << entry;
    if(entry>=0 && entry<bookmarks.count()) {
        Bookmark* bookmark=bookmarks.at(entry);
        FiltersBase* filters;

        bookmarksEditDialog->setTitle(bookmark->getTitle());
        bookmarksEditDialog->setBand(bands.getLabel(bookmark->getBand()));
        bookmarksEditDialog->setFrequency(stringFrequency(bookmark->getFrequency()));
        bookmarksEditDialog->setMode(mode.getStringMode(bookmark->getMode()));

        switch(bookmark->getMode()) {
        case MODE_CWL:
            filters=&cwlFilters;
            break;
        case MODE_CWU:
            filters=&cwuFilters;
            break;
        case MODE_LSB:
            filters=&lsbFilters;
            break;
        case MODE_USB:
            filters=&usbFilters;
            break;
        case MODE_DSB:
            filters=&dsbFilters;
            break;
        case MODE_AM:
            filters=&amFilters;
            break;
        case MODE_SAM:
            filters=&samFilters;
            break;
        case MODE_FMN:
            filters=&fmnFilters;
            break;
        case MODE_DIGL:
            filters=&diglFilters;
            break;
        case MODE_DIGU:
            filters=&diguFilters;
            break;
        }
        bookmarksEditDialog->setFilter(filters->getText(bookmark->getFilter()));
    } else {
        bookmarksEditDialog->setTitle("");
        bookmarksEditDialog->setBand("");
        bookmarksEditDialog->setFrequency("");
        bookmarksEditDialog->setMode("");
        bookmarksEditDialog->setFilter("");
    }
}

QString UI::stringFrequency(quint64 frequency) {
    QString strFrequency;
    strFrequency.sprintf("%lld.%03lld.%03lld",frequency/1000000,frequency%1000000/1000,frequency%1000);
    return strFrequency;
}

void UI::addXVTR(QString title,quint64 minFrequency,quint64 maxFrequency,quint64 ifFrequency,quint64 freq,int m,int filt) {

    qDebug()<<"UI::addXVTR"<<title;
    /*
    xvtr.add(title,minFrequency,maxFrequency,ifFrequency,freq,m,filt);

    // update the menu
    xvtr.buildMenu(widget.menuXVTR);
    configure.updateXvtrList(&xvtr);
    */
}

void UI::deleteXVTR(int index) {
    xvtr.del(index);

    // update the menu
    //xvtr.buildMenu(widget.menuXVTR);
    //configure.updateXvtrList(&xvtr);
}

void UI::selectXVTR(QAction* action) {

    xvtr.select(action);

    ifFrequency=xvtr.getIFFrequency();
    frequency=xvtr.getFrequency();
    int samplerate = widget.spectrumFrame->samplerate();
    if(subRx) {
        if ((subRxFrequency < (frequency - (samplerate / 2))) || (subRxFrequency > (frequency + (samplerate / 2)))) {
            subRxFrequency=frequency;
        }
    }

    command.clear();
    command.append(QString("frequency "));
    command.append(QString::number(frequency));
    command.append(QString(" "));
    command.append(QString::number(xvtr.getEntry()));  /// BANDS+xvtr
    Connection::getInstance()->sendCommand(command);

    widget.spectrumFrame->setFrequency(frequency);
    widget.spectrumFrame->setSubRxFrequency(subRxFrequency);
    widget.spectrumFrame->setHigh(bands.getSpectrumHigh());
    widget.spectrumFrame->setLow(bands.getSpectrumLow());
    widget.waterfallFrame->setFrequency(frequency);
    widget.waterfallFrame->setSubRxFrequency(subRxFrequency);
    widget.waterfallFrame->setHigh(bands.getWaterfallHigh());
    widget.waterfallFrame->setLow(bands.getWaterfallLow());

    widget.spectrumFrame->setBand(xvtr.getTitle());
    BandLimit* limits=bands.getBandLimits(xvtr.getFrequency()-(samplerate/2),xvtr.getFrequency()+(samplerate/2));
    widget.spectrumFrame->setBandLimits(limits->getMin(),limits->getMax());

}

void UI::actionSquelch() {
    if(squelch) {
        squelch=false;
        command.clear();
        command.append(QString("squelchstate 0"));
        Connection::getInstance()->sendCommand(command);
        widget.spectrumFrame->setSquelch(false);
        widget.actionSquelchEnable->setChecked(false);
    } else {
        squelch=true;
        command.clear();
        command.append(QString("squelch "));
        command.append(QString::number(squelchValue));
        Connection::getInstance()->sendCommand(command);
        command.clear();
        command.append(QString("squelchstate 1"));
        Connection::getInstance()->sendCommand(command);
        widget.spectrumFrame->setSquelch(true);
        widget.spectrumFrame->setSquelchVal(squelchValue);
        widget.actionSquelchEnable->setChecked(true);
    }
}

void UI::actionSquelchReset() {
    squelchValue=-100;
    if(squelch) {
        command.clear();
        command.append(QString("squelch "));
        command.append(QString::number(squelchValue));
        Connection::getInstance()->sendCommand(command);
        widget.spectrumFrame->setSquelchVal(squelchValue);
    }
}

void UI::squelchValueChanged(int val) {
    squelchValue=squelchValue+val;
    if(squelch) {
        command.clear();
        command.append(QString("squelch "));
        command.append(QString::number(squelchValue));
        Connection::getInstance()->sendCommand(command);
        widget.spectrumFrame->setSquelchVal(squelchValue);
    }
}

void UI::actionMox() {
    mox=!mox;
    audio->mox(mox);
    widget.spectrumFrame->setMox(mox);
    command.clear();
    command.append(QString("mox "));
    command.append(QString::number(mox));
    Connection::getInstance()->sendCommand(command);

    qDebug()<<command;

}

void UI::actionRemoteMic() {
    command.clear();
    command.append(QString("remotemic "));
    command.append(boolToString(widget.actionRemoteMic->isChecked()));
    Connection::getInstance()->sendCommand(command);

    qDebug()<<command;
}

QString UI::boolToString(bool b) {
    QString result;
    if(b)
        result="1";
    else
        result="0";
    return result;
}

QString UI::getConfigDir() {
    if (!configDir.isNull()) {
        return configDir;
    }
    // Search config file

    QString binDir=QCoreApplication::applicationDirPath();
    QString organization=QCoreApplication::organizationName();
    QString configFileName=QCoreApplication::applicationName()+".ini";

    QStringList searchList;
    searchList.append(QDir::cleanPath(binDir));
    searchList.append(QDir::cleanPath(binDir+"/../etc"));
    searchList.append(QDir::cleanPath(binDir+"/../../etc")); // for development under windows
    searchList.append(QDir::rootPath()+"etc/xdg/"+organization);
    searchList.append(QDir::rootPath()+"etc/opt");
    searchList.append(QDir::rootPath()+"etc");

    foreach (QString dir, searchList) {
        QFile file(dir+"/"+configFileName);
        if (file.exists()) {
            // found
            configDir=dir;
            qDebug("Using config file %s",qPrintable(file.fileName()));
            return configDir;
        }
    }

    // not found
    foreach (QString dir, searchList) {
        qWarning("%s/%s not found",qPrintable(dir),qPrintable(configFileName));
    }
    qDebug("Cannot find config file %s",qPrintable(configFileName));
    configDir=binDir;
    return configDir;
}

void UI::configOptions(QDomDocument* configuration) {

    QString id;
    QString state;
    QString choice;
    QVector<QString> choices;

    // walk through the DOM tree and extract the hardware config options
    QDomElement element=configuration->documentElement();
    QDomNode n=element.firstChild();
    while(!n.isNull()) {
        if(n.nodeName()=="control") {
            choices.clear();
            QDomNode controlNode=n.firstChild();
            QDomNode controlElement;
            while(!controlNode.isNull()) {
                if(controlNode.nodeName()=="id") {
                    controlElement=controlNode.firstChild();
                    if(!controlElement.isNull()) {
                        if(controlElement.isText()) {
                            id=controlElement.nodeValue();
                        }
                    }
                } else if(controlNode.nodeName()=="state") {
                    controlElement=controlNode.firstChild();
                    if(!controlElement.isNull()) {
                        if(controlElement.isText()) {
                            state=controlElement.nodeValue();
                        }
                    }
                } else if(controlNode.nodeName()=="choice") {
                    controlElement=controlNode.firstChild();
                    if(!controlElement.isNull()) {
                        if(controlElement.isText()) {
                            choices.append(controlElement.nodeValue());
                        }
                    }
                }
                controlNode=controlNode.nextSibling();
            }
            if(choices.isEmpty()) {
                qDebug()<<"control: "<<id<<":"<<state;
                QAction* action;
                action=new QAction(id,this);
                action->setCheckable(true);
                if(state=="true") action->setChecked(true);
                widget.menuHardware->addAction(action);
                connect(action,SIGNAL(triggered()),this,SLOT(hardwareActionTriggered()));
            } else {
                qDebug()<<"control: "<<id;
                QMenu* menu;
                menu=new QMenu(id);
                QActionGroup* actionGroup=new QActionGroup(menu);
                for(int i=0;i<choices.size();i++) {
                    qDebug()<<"    "<<choices.at(i);
                    QAction* action;
                    action=new QAction(choices.at(i),this);
                    action->setCheckable(true);
                    action->setActionGroup(actionGroup);
                    menu->addAction(action);
                    connect(action,SIGNAL(triggered()),this,SLOT(hardwareChoiceTriggered()));
                }
                widget.menuHardware->addMenu(menu);

            }
        }
        n=n.nextSibling();
    }
}

void UI::hardwareActionTriggered() {
    QAction *action = qobject_cast<QAction *>(sender());
    qDebug()<<"hardwareActionTriggered:"<<action->text()<<":"<<action->isChecked();
    command.clear();
    command.append("hardware ");
    command.append(action->text());
    command.append(" ");
    command.append(action->isChecked()?"on":"off");
    Connection::getInstance()->sendCommand(command);
}

void UI::hardwareChoiceTriggered() {
    QAction *action = qobject_cast<QAction *>(sender());
    QMenu *menu=(QMenu*)action->actionGroup()->parent();
    qDebug()<<"hardwareChoiceTriggered:"<<action->text()<<":"<<menu->title();
    command.clear();
    command.append("hardware ");
    command.append(menu->title());
    command.append(" ");
    command.append(action->text());
    Connection::getInstance()->sendCommand(command);
}
