/*
 * File:   UI.cpp
 * Author: john
 *
 * Created on 13 August 2010, 14:28
 */

#include <QDebug>
#include <QSettings>

#include "UI.h"
#include "Configure.h"
#include "Band.h"
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


UI::UI() {
    widget.setupUi(this);

    qTimer=NULL;

    widget.gridLayout->setContentsMargins(2,2,2,2);
    widget.gridLayout->setVerticalSpacing(0);
    
    // connect up all the components
    connect(widget.actionConnectToServer,SIGNAL(triggered()),this,SLOT(actionConnect()));
    connect(widget.actionDisconnectFromServer,SIGNAL(triggered()),this,SLOT(actionDisconnect()));

    connect(&connection,SIGNAL(isConnected()),this,SLOT(connected()));
    connect(&connection,SIGNAL(disconnected(QString)),this,SLOT(disconnected(QString)));
    connect(&connection,SIGNAL(header(char*)),this,SLOT(receivedHeader(char*)));

    connect(widget.actionConfig,SIGNAL(triggered()),this,SLOT(actionConfigure()));

    connect(widget.actionMuteMainRx,SIGNAL(triggered()),this,SLOT(actionMuteMainRx()));
    connect(widget.actionMuteSubRx,SIGNAL(triggered()),this,SLOT(actionMuteSubRx()));

    connect(widget.action160, SIGNAL(triggered()),this,SLOT(action160()));
    connect(widget.action80, SIGNAL(triggered()),this,SLOT(action80()));
    connect(widget.action60, SIGNAL(triggered()),this,SLOT(action60()));
    connect(widget.action40, SIGNAL(triggered()),this,SLOT(action40()));
    connect(widget.action30, SIGNAL(triggered()),this,SLOT(action30()));
    connect(widget.action20, SIGNAL(triggered()),this,SLOT(action20()));
    connect(widget.action17, SIGNAL(triggered()),this,SLOT(action17()));
    connect(widget.action15, SIGNAL(triggered()),this,SLOT(action15()));
    connect(widget.action12, SIGNAL(triggered()),this,SLOT(action12()));
    connect(widget.action10, SIGNAL(triggered()),this,SLOT(action10()));
    connect(widget.action6, SIGNAL(triggered()),this,SLOT(action6()));
    connect(widget.actionGen, SIGNAL(triggered()),this,SLOT(actionGen()));
    connect(widget.actionWWV, SIGNAL(triggered()),this,SLOT(actionWWV()));

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

    connect(widget.actionANF,SIGNAL(triggered()),this,SLOT(actionANF()));
    connect(widget.actionNR,SIGNAL(triggered()),this,SLOT(actionNR()));
    connect(widget.actionNB,SIGNAL(triggered()),this,SLOT(actionNB()));

    connect(widget.actionLong,SIGNAL(triggered()),this,SLOT(actionLong()));
    connect(widget.actionSlow,SIGNAL(triggered()),this,SLOT(actionSlow()));
    connect(widget.actionMedium,SIGNAL(triggered()),this,SLOT(actionMedium()));
    connect(widget.actionFast,SIGNAL(triggered()),this,SLOT(actionFast()));

    //connect(widget.afGainHorizontalSlider,SIGNAL(valueChanged(int)),this,SLOT(setGain(int)));

    //connect(widget.subRxAfGainHorizontalSlider,SIGNAL(valueChanged(int)),this,SLOT(setSubRxGain(int)));
    connect(widget.actionSubrx,SIGNAL(triggered()),this,SLOT(actionSubRx()));

    connect(&band,SIGNAL(bandChanged(int,int)),this,SLOT(bandChanged(int,int)));
    connect(&band,SIGNAL(frequencyChanged(long long)),this,SLOT(frequencyChanged(long long)));
    connect(&mode,SIGNAL(modeChanged(int,int)),this,SLOT(modeChanged(int,int)));
    connect(&filters,SIGNAL(filtersChanged(FiltersBase*,FiltersBase*)),this,SLOT(filtersChanged(FiltersBase*,FiltersBase*)));

    connect(&filters,SIGNAL(filterChanged(int,int)),this,SLOT(filterChanged(int,int)));

    //connect(&connection,SIGNAL(isConnected()),this,SLOT(connected()));
    //connect(&connection,SIGNAL(disconnected(QString)),this,SLOT(disconnected(QString)));


    connect(widget.spectrumFrame, SIGNAL(frequencyMoved(int,int)),
            this, SLOT(frequencyMoved(int,int)));
    connect(widget.spectrumFrame, SIGNAL(frequencyChanged(long long)),
            this, SLOT(frequencyChanged(long long)));

    connect(widget.spectrumFrame, SIGNAL(spectrumHighChanged(int)),
            this,SLOT(spectrumHighChanged(int)));
    connect(widget.spectrumFrame, SIGNAL(spectrumLowChanged(int)),
            this,SLOT(spectrumLowChanged(int)));

    connect(widget.spectrumFrame, SIGNAL(waterfallHighChanged(int)),
            this,SLOT(waterfallHighChanged(int)));
    connect(widget.spectrumFrame, SIGNAL(waterfallLowChanged(int)),
            this,SLOT(waterfallLowChanged(int)));

    fps=15;

    QSpinBox* spectrumHighSpinBox=configure.findChild<QSpinBox*>("spectrumHighSpinBox");
    if(spectrumHighSpinBox==NULL) {
        qDebug() << "spectrumHighSpinBox id NULL";
    } else {
        spectrumHighSpinBox->setValue(widget.spectrumFrame->getHigh());
        connect(spectrumHighSpinBox,SIGNAL(valueChanged(int)),this,SLOT(spectrumHighChanged(int)));
    }

    QSpinBox* spectrumLowSpinBox=configure.findChild<QSpinBox*>("spectrumLowSpinBox");
    if(spectrumLowSpinBox==NULL) {
        qDebug() << "spectrumLowSpinBox id NULL";
    } else {
        spectrumLowSpinBox->setValue(widget.spectrumFrame->getLow());
        connect(spectrumLowSpinBox,SIGNAL(valueChanged(int)),this,SLOT(spectrumLowChanged(int)));
    }

    QSpinBox* fpsSpinBox=configure.findChild<QSpinBox*>("fpsSpinBox");
    if(fpsSpinBox==NULL) {
        qDebug() << "fpsSpinBox id NULL";
    } else {
        fpsSpinBox->setValue(fps);
        connect(fpsSpinBox,SIGNAL(valueChanged(int)),this,SLOT(fpsChanged(int)));
    }

    QSpinBox* waterfallHighSpinBox=configure.findChild<QSpinBox*>("waterfallHighSpinBox");
    if(waterfallHighSpinBox==NULL) {
        qDebug() << "waterfallHighSpinBox id NULL";
    } else {
        waterfallHighSpinBox->setValue(widget.waterfallFrame->getHigh());
        connect(waterfallHighSpinBox,SIGNAL(valueChanged(int)),this,SLOT(waterfallHighChanged(int)));
    }

    QSpinBox* waterfallLowSpinBox=configure.findChild<QSpinBox*>("waterfallLowSpinBox");
    if(waterfallLowSpinBox==NULL) {
        qDebug() << "waterfallLowSpinBox id NULL";
    } else {
        waterfallLowSpinBox->setValue(widget.waterfallFrame->getLow());
        connect(waterfallLowSpinBox,SIGNAL(valueChanged(int)),this,SLOT(waterfallLowChanged(int)));
    }

    QComboBox* audioDeviceComboBox=configure.findChild<QComboBox*>("audioDeviceComboBox");
    if(audioDeviceComboBox==NULL) {
        qDebug() << "audioDeviceComboBox id NULL";
    } else {
        audio.get_audio_devices(audioDeviceComboBox);
        connect(audioDeviceComboBox, SIGNAL(currentIndexChanged(int)),
                this, SLOT(audioChanged(int)));
    }

    QComboBox* sampleRateComboBox=configure.findChild<QComboBox*>("sampleRateComboBox");
    if(sampleRateComboBox==NULL) {
        qDebug() << "sampleRateComboBox id NULL";
    } else {
        connect(sampleRateComboBox, SIGNAL(currentIndexChanged(int)),
                this, SLOT(sampleRateChanged(int)));
    }

    QComboBox* encodingComboBox=configure.findChild<QComboBox*>("encodingComboBox");
    if(encodingComboBox==NULL) {
        qDebug() << "encodingComboBox id NULL";
    } else {
        connect(encodingComboBox, SIGNAL(currentIndexChanged(int)),
                this, SLOT(encodingChanged(int)));
    }

    QSpinBox* audioChannelsSpinBox=configure.findChild<QSpinBox*>("audioChannelsSpinBox");
    if(audioChannelsSpinBox==NULL) {
        qDebug() << "audioChannelsSpinBox id NULL";
    } else {
        connect(audioChannelsSpinBox,SIGNAL(valueChanged(int)),this,SLOT(audioChannelsChanged(int)));
    }

    QComboBox* byteOrderComboBox=configure.findChild<QComboBox*>("byteOrderComboBox");
    if(byteOrderComboBox==NULL) {
        qDebug() << "byteOrderComboBox id NULL";
    } else {
        connect(byteOrderComboBox, SIGNAL(currentIndexChanged(int)),
                this, SLOT(byteOrderChanged(int)));
    }





    QComboBox* hostComboBox=configure.findChild<QComboBox*>("hostComboBox");
    if(hostComboBox==NULL) {
        qDebug() << "hostComboBox id NULL";
    } else {
        connect(hostComboBox, SIGNAL(currentIndexChanged(int)),
                this, SLOT(hostChanged(int)));
    }

    gain=30;

    subRx=FALSE;
    subRxGain=30;

    agc=AGC_SLOW;
    widget.actionSlow->setChecked(TRUE);

    cwPitch=600;

    audio_device=0;
    audio_sample_rate=configure.getSampleRate();
    audio_channels=configure.getChannels();
    audio_byte_order=configure.getByteOrder();
    audio.initialize_audio(AUDIO_BUFFER_SIZE);

    // load any saved settings
    loadSettings();

    widget.spectrumFrame->setHost(configure.getHost());
    widget.spectrumFrame->setReceiver(configure.getReceiver());

    widget.actionSubrx->setDisabled(TRUE);
    widget.actionMuteSubRx->setDisabled(TRUE);

    band.initBand(band.getBand());

    
}

UI::~UI() {
    if(qTimer!=NULL) {
        qTimer->stop();
        qTimer=NULL;
    }

    connection.disconnect();
}

void UI::loadSettings() {
    QSettings settings("G0ORX", "QtRadio");
    band.loadSettings(&settings);
    configure.loadSettings(&settings);
}

void UI::saveSettings() {
    QSettings settings("G0ORX","QtRadio");

    qDebug() << "closeEvent: " << settings.fileName();
    configure.saveSettings(&settings);
    band.saveSettings(&settings);
}

void UI::hostChanged(int choice) {
    widget.spectrumFrame->setHost(configure.getHost());
}

void UI::rxChanged(int rx) {
    widget.spectrumFrame->setReceiver(configure.getReceiver());
}

void UI::closeEvent(QCloseEvent* event) {
    saveSettings();
}

void UI::actionConfigure() {
    configure.show();
}

void UI::spectrumHighChanged(int high) {
    qDebug() << __FUNCTION__ << ": " << high;

    widget.spectrumFrame->setHigh(high);

    QSpinBox* spectrumHighSpinBox=configure.findChild<QSpinBox*>("spectrumHighSpinBox");
    spectrumHighSpinBox->setValue(high);
}

void UI::spectrumLowChanged(int low) {
    qDebug() << __FUNCTION__ << ": " << low;

    widget.spectrumFrame->setLow(low);

    QSpinBox* spectrumLowSpinBox=configure.findChild<QSpinBox*>("spectrumLowSpinBox");
    spectrumLowSpinBox->setValue(low);
}

void UI::fpsChanged(int f) {
    qDebug() << "fpsChanged:" << f;
    fps=f;
    if(qTimer!=NULL) {
        qTimer->stop();
        qTimer=NULL;
    }
    qTimer = new QTimer(this);
    connect(qTimer, SIGNAL(timeout()), this, SLOT(updateSpectrum()));
    qTimer->start(1000 / fps);

}

void UI::waterfallHighChanged(int high) {
    qDebug() << __LINE__ << __FUNCTION__ << ": " << high;

    widget.waterfallFrame->setHigh(high);

    QSpinBox* waterfallHighSpinBox=configure.findChild<QSpinBox*>("waterfallHighSpinBox");
    waterfallHighSpinBox->setValue(high);
}

void UI::waterfallLowChanged(int low) {
    qDebug() << __FUNCTION__ << ": " << low;

    widget.waterfallFrame->setLow(low);

    QSpinBox* waterfallLowSpinBox=configure.findChild<QSpinBox*>("waterfallLowSpinBox");
    waterfallLowSpinBox->setValue(low);
}

void UI::audioChanged(int choice) {
    qDebug() << "audioChanged " << choice;
    audio_device=choice;
    QComboBox* audioDeviceComboBox=configure.findChild<QComboBox*>("audioDeviceComboBox");
    if(audioDeviceComboBox==NULL) {
        qDebug() << "UI::audioChanged: audioDeviceComboBox is NULL";
        return;
    }
    QComboBox* sampleRateComboBox=configure.findChild<QComboBox*>("sampleRateComboBox");
    if(sampleRateComboBox==NULL) {
        qDebug() << "UI::audioChanged: sampleRateComboBox is NULL";
        return;
    }

    audio.select_audio(audioDeviceComboBox->itemData(audio_device).value<QAudioDeviceInfo >(),audio_sample_rate,audio_channels,audio_byte_order);
}

void UI::sampleRateChanged(int choice) {
    qDebug() << "sampleRateChanged " << choice;


    QComboBox* audioDeviceComboBox=configure.findChild<QComboBox*>("audioDeviceComboBox");
    if(audioDeviceComboBox==NULL) {
        qDebug() << "UI::audioChanged: audioDeviceComboBox is NULL";
        return;
    }
    QComboBox* sampleRateComboBox=configure.findChild<QComboBox*>("sampleRateComboBox");
    if(sampleRateComboBox==NULL) {
        qDebug() << "UI::audioChanged: sampleRateComboBox is NULL";
        return;
    }
    audio_sample_rate=sampleRateComboBox->itemText(choice).toInt();
    audio.select_audio(audioDeviceComboBox->itemData(audio_device).value<QAudioDeviceInfo>(),audio_sample_rate,audio_channels,audio_byte_order);
}

void UI::audioChannelsChanged(int channels) {
    QComboBox* audioDeviceComboBox=configure.findChild<QComboBox*>("audioDeviceComboBox");
    if(audioDeviceComboBox==NULL) {
        qDebug() << "UI::audioChanged: audioDeviceComboBox is NULL";
        return;
    }
    audio_channels=channels;
    audio.select_audio(audioDeviceComboBox->itemData(audio_device).value<QAudioDeviceInfo>(),audio_sample_rate,audio_channels,audio_byte_order);
}

void UI::encodingChanged(int choice) {

}

void UI::byteOrderChanged(int choice) {
    QComboBox* audioDeviceComboBox=configure.findChild<QComboBox*>("audioDeviceComboBox");
    if(audioDeviceComboBox==NULL) {
        qDebug() << "UI::audioChanged: audioDeviceComboBox is NULL";
        return;
    }
    audio_byte_order=configure.getByteOrder();
    qDebug() << "byteOrderChanged: " << audio_byte_order;
    audio.select_audio(audioDeviceComboBox->itemData(audio_device).value<QAudioDeviceInfo>(),audio_sample_rate,audio_channels,audio_byte_order);
}

void UI::actionConnect() {
    //qDebug() << "UI::actionConnect";
    QComboBox* hostComboBox=configure.findChild<QComboBox*>("hostComboBox");
    if(hostComboBox==NULL) {
        qDebug() << "hostComboBox id NULL";
        return;
    }

    QSpinBox* rxSpinBox=configure.findChild<QSpinBox*>("rxSpinBox");
    if(rxSpinBox==NULL) {
        qDebug() << "rxSpinBox id NULL";
        return;
    }

    connection.connect(hostComboBox->currentText(), rxSpinBox->value());
    widget.spectrumFrame->setHost(connection.getHost());
    widget.spectrumFrame->setReceiver(connection.getReceiver());


}

void UI::actionDisconnect() {
    //qDebug() << "UI::actionDisconnect";

    if(qTimer!=NULL) {
        qTimer->stop();
        qTimer=NULL;
    }

    connection.disconnect();
    widget.actionConnectToServer->setDisabled(FALSE);
    widget.actionDisconnectFromServer->setDisabled(TRUE);
    widget.actionSubrx->setDisabled(TRUE);
    widget.actionMuteSubRx->setDisabled(TRUE);


    configure.connected(FALSE);
}

void UI::connected() {
    qDebug() << "UI::connected";

    configure.connected(TRUE);

    // send initial settings
    frequency=band.getFrequency();
    command.clear(); QTextStream(&command) << "setFrequency " << frequency;
    connection.sendCommand(command);
    widget.spectrumFrame->setFrequency(frequency);
    command.clear(); QTextStream(&command) << "setMode " << band.getMode();
    connection.sendCommand(command);
    command.clear(); QTextStream(&command) << "setFilter " << filters.getLow() << " " << filters.getHigh();
    connection.sendCommand(command);
    widget.spectrumFrame->setFilter(filters.getLow(),filters.getHigh());

    widget.actionConnectToServer->setDisabled(TRUE);
    widget.actionDisconnectFromServer->setDisabled(FALSE);
    widget.actionSubrx->setDisabled(FALSE);
    widget.actionMuteSubRx->setDisabled(FALSE);

    // select the audio
    //audio.select_audio(widget.audioComboBox->itemData(audio_device).value<QAudioDeviceInfo >);

    

    // start the spectrum
    qDebug() << "starting spectrum timer";
    qTimer= new QTimer(this);
    connect(qTimer, SIGNAL(timeout()), this, SLOT(updateSpectrum()));
    qTimer->start(1000 / fps);


    // start the audio
    audio_buffers=0;
    command.clear(); QTextStream(&command) << "SetRXOutputGain " << gain;
    connection.sendCommand(command);

    if (!getenv("QT_RADIO_NO_LOCAL_AUDIO")) {
       command.clear(); QTextStream(&command) << "startAudioStream " << (400*(audio_sample_rate/8000)) << " " << audio_sample_rate << " " << audio_channels;
       connection.sendCommand(command);
    }
    //widget.serverConnectPushButton->setText("Disconnect");
    //widget.serverConnectPushButton->setDisabled(FALSE);
    //widget.audioComboBox->setDisabled(TRUE);

    command.clear(); QTextStream(&command) << "SetPan 0.5"; // center
    connection.sendCommand(command);

    command.clear(); QTextStream(&command) << "SetAGC " << agc;
    connection.sendCommand(command);

    command.clear(); QTextStream(&command) << "SetANF " << (widget.actionANF->isChecked()?"true":"false");
    connection.sendCommand(command);
    command.clear(); QTextStream(&command) << "SetNR " << (widget.actionNR->isChecked()?"true":"false");
    connection.sendCommand(command);
    command.clear(); QTextStream(&command) << "SetNB " << (widget.actionNB->isChecked()?"true":"false");
    connection.sendCommand(command);

}

void UI::disconnected(QString message) {
    qDebug() << "UI::disconnected: " << message;

    widget.statusbar->showMessage(message,5000);

    //widget.serverConnectPushButton->setText("Connect");
    //widget.serverConnectPushButton->setDisabled(FALSE);
    //widget.audioComboBox->setDisabled(FALSE);

    widget.actionConnectToServer->setDisabled(FALSE);
    widget.actionDisconnectFromServer->setDisabled(TRUE);
    widget.actionSubrx->setDisabled(TRUE);
    widget.actionMuteSubRx->setDisabled(TRUE);

    //widget.spectrumFrame->setHost("");
    //widget.spectrumFrame->setReceiver(0);

    if(qTimer!=NULL) {
        qTimer->stop();
        qTimer=NULL;
    }

    configure.connected(FALSE);
}

void UI::updateSpectrum() {
    //qDebug() << "UI::updateSpectrum width=" << widget.spectrumFrame->width();
    command.clear(); QTextStream(&command) << "getSpectrum " << widget.spectrumFrame->width();
    connection.sendCommand(command);
}

void UI::receivedHeader(char* header) {
    int length;
    char* samples;
  
    length = atoi(&header[26]);
    samples=(char*)malloc(length);
    
    //qDebug() << "UI::receivedHeader: " << (int)header[0] << " length=" << length;

    switch(header[0]) {
        case 0:
            // spectrum data
            connection.read(samples,length);
            //qDebug() << "read spectrum samples";
            sampleRate = atoi(&header[32]);
            widget.spectrumFrame->updateSpectrum(header,samples,length);
            widget.waterfallFrame->updateWaterfall(header,samples,length);
            
            break;
        case 1:
            // audio data
            //qDebug() << "read audio samples";

            if(audio_buffers==0) {
                first_audio_buffer=(char*)malloc(length);
                connection.read(first_audio_buffer,length);
                audio_buffers++;
            } else if(audio_buffers==1) {
                connection.read(samples,length);
                audio_buffers++;
                audio.process_audio(header,first_audio_buffer,length);
                audio.process_audio(header,samples,length);
            } else {
                connection.read(samples,length);
                audio.process_audio(header,samples,length);
            }
            break;
    }
    free(samples);
}

void UI::setGain(int g) {
    // remember the gain
    gain=g;
    command.clear(); QTextStream(&command) << "SetRXOutputGain " << gain;
    connection.sendCommand(command);

    qDebug() << command;
}

void UI::actionSubRx() {
    if(subRx) {
        // on, so turn off
        subRx=FALSE;
        widget.spectrumFrame->setSubRxState(FALSE);
        widget.actionMuteSubRx->setChecked(FALSE);
    } else {
        subRx=TRUE;

        // check frequency in range
        int samplerate = widget.spectrumFrame->samplerate();
        long long frequency=band.getFrequency();
        if ((subRxFrequency < (frequency - (samplerate / 2))) || (subRxFrequency > (frequency + (samplerate / 2)))) {
            subRxFrequency=band.getFrequency();
        }
        widget.spectrumFrame->setSubRxState(TRUE);
        command.clear(); QTextStream(&command) << "SetSubRXOutputGain " << subRxGain;
        connection.sendCommand(command);
        command.clear(); QTextStream(&command) << "SetSubRXFrequency " << frequency - subRxFrequency;
        connection.sendCommand(command);
        setSubRxPan();
    }

    //widget.actionSubrx.setChecked(subRx);
    command.clear(); QTextStream(&command) << "SetSubRX " << subRx;
    connection.sendCommand(command);

}

void UI::setSubRxGain(int gain) {
    subRxGain=gain;
    command.clear(); QTextStream(&command) << "SetSubRXOutputGain " << subRxGain;
    connection.sendCommand(command);

    qDebug() << command;
}

void UI::action160() {
    band.selectBand(BAND_160);
}

void UI::action80() {
    band.selectBand(BAND_80);
}

void UI::action60() {
    band.selectBand(BAND_60);
}

void UI::action40() {
    band.selectBand(BAND_40);
}

void UI::action30() {
    band.selectBand(BAND_30);
}

void UI::action20() {
    band.selectBand(BAND_20);
}

void UI::action17() {
    band.selectBand(BAND_17);
}

void UI::action15() {
    band.selectBand(BAND_15);
}

void UI::action12() {
    band.selectBand(BAND_12);
}

void UI::action10() {
    band.selectBand(BAND_10);
}

void UI::action6() {
    band.selectBand(BAND_6);
}

void UI::actionGen() {
    band.selectBand(BAND_GEN);
}

void UI::actionWWV() {
    band.selectBand(BAND_WWV);
}

void UI::bandChanged(int previousBand,int newBand) {
    qDebug() << "bandChanged: " << previousBand << "," << newBand;
    // uncheck previous band
    switch(previousBand) {
        case BAND_160:
            widget.action160->setChecked(FALSE);
            break;
        case BAND_80:
            widget.action80->setChecked(FALSE);
            break;
        case BAND_60:
            widget.action60->setChecked(FALSE);
            break;
        case BAND_40:
            widget.action40->setChecked(FALSE);
            break;
        case BAND_30:
            widget.action30->setChecked(FALSE);
            break;
        case BAND_20:
            widget.action20->setChecked(FALSE);
            break;
        case BAND_17:
            widget.action17->setChecked(FALSE);
            break;
        case BAND_15:
            widget.action15->setChecked(FALSE);
            break;
        case BAND_12:
            widget.action12->setChecked(FALSE);
            break;
        case BAND_10:
            widget.action10->setChecked(FALSE);
            break;
        case BAND_6:
            widget.action6->setChecked(FALSE);
            break;
        case BAND_GEN:
            widget.actionGen->setChecked(FALSE);
            break;
        case BAND_WWV:
            widget.actionWWV->setChecked(FALSE);
            break;
    }

    // check new band
    switch(newBand) {
        case BAND_160:
            widget.action160->setChecked(TRUE);
            break;
        case BAND_80:
            widget.action80->setChecked(TRUE);
            break;
        case BAND_60:
            widget.action60->setChecked(TRUE);
            break;
        case BAND_40:
            widget.action40->setChecked(TRUE);
            break;
        case BAND_30:
            widget.action30->setChecked(TRUE);
            break;
        case BAND_20:
            widget.action20->setChecked(TRUE);
            break;
        case BAND_17:
            widget.action17->setChecked(TRUE);
            break;
        case BAND_15:
            widget.action15->setChecked(TRUE);
            break;
        case BAND_12:
            widget.action12->setChecked(TRUE);
            break;
        case BAND_10:
            widget.action10->setChecked(TRUE);
            break;
        case BAND_6:
            widget.action6->setChecked(TRUE);
            break;
        case BAND_GEN:
            widget.actionGen->setChecked(TRUE);
            break;
        case BAND_WWV:
            widget.actionWWV->setChecked(TRUE);
            break;
    }

    // get the current mode
    mode.setMode(band.getMode());
    widget.spectrumFrame->setBand(band.getStringBand());

    if(subRx) {

        int samplerate = widget.spectrumFrame->samplerate();
        long long frequency=band.getFrequency();
        if ((subRxFrequency < (frequency - (samplerate / 2))) || (subRxFrequency > (frequency + (samplerate / 2)))) {
            subRxFrequency=band.getFrequency();
        }
    }
    
}

void UI::modeChanged(int previousMode,int newMode) {

    qDebug() << "modeChanged: " << previousMode << "," << newMode;
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
            break;
        case MODE_CWU:
            widget.actionCWU->setChecked(TRUE);
            filters.selectFilters(&cwuFilters);
            break;
        case MODE_LSB:
            widget.actionLSB->setChecked(TRUE);
            filters.selectFilters(&lsbFilters);
            break;
        case MODE_USB:
            widget.actionUSB->setChecked(TRUE);
            filters.selectFilters(&usbFilters);
            break;
        case MODE_DSB:
            widget.actionDSB->setChecked(TRUE);
            filters.selectFilters(&dsbFilters);
            break;
        case MODE_AM:
            widget.actionAM->setChecked(TRUE);
            filters.selectFilters(&amFilters);
            break;
        case MODE_SAM:
            widget.actionSAM->setChecked(TRUE);
            filters.selectFilters(&samFilters);
            break;
        case MODE_FMN:
            widget.actionFMN->setChecked(TRUE);
            filters.selectFilters(&fmnFilters);
            break;
        case MODE_DIGL:
            widget.actionDIGL->setChecked(TRUE);
            filters.selectFilters(&diglFilters);
            break;
        case MODE_DIGU:
            widget.actionDIGU->setChecked(TRUE);
            filters.selectFilters(&diguFilters);
            break;
    }

    widget.spectrumFrame->setMode(mode.getStringMode());
    command.clear(); QTextStream(&command) << "setMode " << mode.getMode();
    connection.sendCommand(command);
  
}

void UI::filtersChanged(FiltersBase* previousFilters,FiltersBase* newFilters) {

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

    widget.spectrumFrame->setFilter(filters.getText());
    filters.selectFilter(filters.getFilter());
    

}

void UI::actionCWL() {
    mode.setMode(MODE_CWL);
    filters.selectFilters(&cwlFilters);
}

void UI::actionCWU() {
    mode.setMode(MODE_CWU);
    filters.selectFilters(&cwuFilters);
}

void UI::actionLSB() {
    mode.setMode(MODE_LSB);
    filters.selectFilters(&lsbFilters);
}

void UI::actionUSB() {
    mode.setMode(MODE_USB);
    filters.selectFilters(&usbFilters);
}

void UI::actionDSB() {
    mode.setMode(MODE_DSB);
    filters.selectFilters(&dsbFilters);
}

void UI::actionAM() {
    mode.setMode(MODE_AM);
    filters.selectFilters(&amFilters);
}

void UI::actionSAM() {
    mode.setMode(MODE_SAM);
    filters.selectFilters(&samFilters);
}

void UI::actionFMN() {
    mode.setMode(MODE_FMN);
    filters.selectFilters(&fmnFilters);
}

void UI::actionDIGL() {
    mode.setMode(MODE_DIGL);
    filters.selectFilters(&diglFilters);
}

void UI::actionDIGU() {
    mode.setMode(MODE_DIGU);
    filters.selectFilters(&diguFilters);
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

    command.clear(); QTextStream(&command) << "setFilter " << low << " " << high;
    connection.sendCommand(command);
    widget.spectrumFrame->setFilter(low,high);
}

void UI::frequencyChanged(long long f) {
    //qDebug() << __FUNCTION__ << ": " << f;

    frequency=f;
    command.clear(); QTextStream(&command) << "setFrequency " << f;

    connection.sendCommand(command);
    widget.spectrumFrame->setFrequency(frequency);
}

void UI::frequencyMoved(int increment,int step) {
    //qDebug() << __FUNCTION__ << ": increment=" << increment << " step=" << step;

    long long f;

    if(subRx) {
        long long diff;
        long long frequency = band.getFrequency();
        f=subRxFrequency+(long long)(increment*step);
        int samplerate = widget.spectrumFrame->samplerate();
        if ((f >= (frequency - (samplerate / 2))) && (f <= (frequency + (samplerate / 2)))) {
            subRxFrequency = f;
        }
        diff = frequency - subRxFrequency;
        command.clear(); QTextStream(&command) << "SetSubRXFrequency " << diff;
        connection.sendCommand(command);
        //qDebug() << "frequencyMoved:subrx: " << increment << "," << subRxFrequency;
        widget.spectrumFrame->setSubRxFrequency(subRxFrequency);
        setSubRxPan();
        qDebug() << command;


    } else {
        band.setFrequency(band.getFrequency()-(long long)(increment*step));
        frequency=band.getFrequency();
        command.clear(); QTextStream(&command) << "setFrequency " << frequency;
        connection.sendCommand(command);
    }
}

void UI::actionANF() {
    command.clear(); QTextStream(&command) << "SetANF " << (widget.actionANF->isChecked()?"true":"false");
    connection.sendCommand(command);
}

void UI::actionNR() {
    command.clear(); QTextStream(&command) << "SetNR " << (widget.actionNR->isChecked()?"true":"false");
    connection.sendCommand(command);
}

void UI::actionNB() {
    command.clear(); QTextStream(&command) << "SetNB " << (widget.actionNB->isChecked()?"true":"false");
    connection.sendCommand(command);
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

    command.clear(); QTextStream(&command) << "SetAGC " << agc;
    connection.sendCommand(command);

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

    command.clear(); QTextStream(&command) << "SetAGC " << agc;
    connection.sendCommand(command);

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

    command.clear(); QTextStream(&command) << "SetAGC " << agc;
    connection.sendCommand(command);
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
    command.clear(); QTextStream(&command) << "SetAGC " << agc;
    connection.sendCommand(command);
}

void UI::setSubRxPan() {
    float pan;

    // set the pan relative to frequency position (only works when in stereo)
    pan=(float)(subRxFrequency-(frequency-(sampleRate/2)))/(float)sampleRate;

    if(pan<0.0) pan=0.0;
    if(pan>1.0) pan=1.0;
    command.clear(); QTextStream(&command) << "SetSubRXPan " << pan;
    connection.sendCommand(command);
}

void UI::actionMuteMainRx() {
    int g=gain;

    if(widget.actionMuteMainRx->isChecked()) {
        g=0;
    }

    command.clear(); QTextStream(&command) << "SetRXOutputGain " << g;
    connection.sendCommand(command);
}

void UI::actionMuteSubRx() {
    int g=subRxGain;

    if(widget.actionMuteSubRx->isChecked()) {
        g=0;
    }

    command.clear(); QTextStream(&command) << "SetSubRXOutputGain " << g;
    connection.sendCommand(command);

}
