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

    // connect up all the components
    connect(widget.serverConnectPushButton,SIGNAL(pressed()),this,SLOT(actionConnect()));
    connect(&connection,SIGNAL(isConnected()),this,SLOT(connected()));
    connect(&connection,SIGNAL(disconnected(QString)),this,SLOT(disconnected(QString)));
    connect(&connection,SIGNAL(header(char*)),this,SLOT(receivedHeader(char*)));

    connect(widget.actionConfigure,SIGNAL(triggered()),this,SLOT(actionConfigure()));

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

    connect(widget.afGainHorizontalSlider,SIGNAL(valueChanged(int)),this,SLOT(setGain(int)));

    connect(widget.subRxAfGainHorizontalSlider,SIGNAL(valueChanged(int)),this,SLOT(setSubRxGain(int)));
    connect(widget.subRxPushButton,SIGNAL(pressed()),this,SLOT(actionSubRx()));

    connect(&band,SIGNAL(bandChanged(int,int)),this,SLOT(bandChanged(int,int)));
    connect(&band,SIGNAL(frequencyChanged(long long)),this,SLOT(frequencyChanged(long long)));
    connect(&mode,SIGNAL(modeChanged(int,int)),this,SLOT(modeChanged(int,int)));
    connect(&filters,SIGNAL(filtersChanged(FiltersBase*,FiltersBase*)),this,SLOT(filtersChanged(FiltersBase*,FiltersBase*)));

    connect(&filters,SIGNAL(filterChanged(int,int)),this,SLOT(filterChanged(int,int)));

    connect(&connection,SIGNAL(isConnected()),this,SLOT(connected()));
    connect(&connection,SIGNAL(disconnected(QString)),this,SLOT(disconnected(QString)));


    connect(widget.spectrumFrame, SIGNAL(frequencyMoved(int)),
            this, SLOT(frequencyMoved(int)));

    fps=15;

    audio_device=0;
    audio.initialize_audio(AUDIO_BUFFER_SIZE);
    audio.get_audio_devices(widget.audioComboBox);
    connect(widget.audioComboBox, SIGNAL(currentIndexChanged(int)),
            this, SLOT(audioChanged(int)));

    gain=30;

    subRx=FALSE;
    subRxGain=30;

    cwPitch=600;

    // load any saved settings
    loadSettings();

    band.initBand(band.getBand());

    
}

UI::~UI() {
}

void UI::loadSettings() {
    QSettings settings("G0ORX", "QtRadio");
    band.loadSettings(&settings);
}

void UI::saveSettings() {
    QSettings settings("G0ORX","QtRadio");

    qDebug() << "closeEvent: " << settings.fileName();
    band.saveSettings(&settings);
}


void UI::closeEvent(QCloseEvent* event) {
    saveSettings();
}

void UI::actionConfigure() {
    Configure* configure = new Configure();

    QSpinBox* spectrumHighSpinBox=configure->findChild<QSpinBox*>("spectrumHighSpinBox");
    if(spectrumHighSpinBox==NULL) {
        qDebug() << "spectrumHighSpinBox id NULL";
    } else {
        spectrumHighSpinBox->setValue(widget.spectrumFrame->getHigh());
        connect(spectrumHighSpinBox,SIGNAL(valueChanged(int)),this,SLOT(spectrumHighChanged(int)));
    }

    QSpinBox* spectrumLowSpinBox=configure->findChild<QSpinBox*>("spectrumLowSpinBox");
    if(spectrumLowSpinBox==NULL) {
        qDebug() << "spectrumLowSpinBox id NULL";
    } else {
        spectrumLowSpinBox->setValue(widget.spectrumFrame->getLow());
        connect(spectrumLowSpinBox,SIGNAL(valueChanged(int)),this,SLOT(spectrumLowChanged(int)));
    }

    QSpinBox* waterfallHighSpinBox=configure->findChild<QSpinBox*>("waterfallHighSpinBox");
    if(waterfallHighSpinBox==NULL) {
        qDebug() << "waterfallHighSpinBox id NULL";
    } else {
        waterfallHighSpinBox->setValue(widget.waterfallFrame->getHigh());
        connect(waterfallHighSpinBox,SIGNAL(valueChanged(int)),this,SLOT(waterfallHighChanged(int)));
    }

    QSpinBox* waterfallLowSpinBox=configure->findChild<QSpinBox*>("waterfallLowSpinBox");
    if(waterfallLowSpinBox==NULL) {
        qDebug() << "waterfallLowSpinBox id NULL";
    } else {
        waterfallLowSpinBox->setValue(widget.waterfallFrame->getLow());
        connect(waterfallLowSpinBox,SIGNAL(valueChanged(int)),this,SLOT(waterfallLowChanged(int)));
    }
    configure->show();
}

void UI::spectrumHighChanged(int high) {
    widget.spectrumFrame->setHigh(high);
}

void UI::spectrumLowChanged(int low) {
    widget.spectrumFrame->setLow(low);
}

void UI::waterfallHighChanged(int high) {
    widget.waterfallFrame->setHigh(high);
}

void UI::waterfallLowChanged(int low) {
    widget.waterfallFrame->setLow(low);
}

void UI::audioChanged(int choice) {
    qDebug() << "audioChanged " << choice;
    audio_device=choice;
    //audio.select_audio(widget.audioComboBox->itemData(choice).value<QAudioDeviceInfo > ());
}

void UI::actionConnect() {
    connection.connect(widget.serverHostComboBox->currentText(), widget.serverRxSpinBox->value());
    widget.serverConnectPushButton->setDisabled(TRUE);
    widget.audioComboBox->setDisabled(TRUE);
}

void UI::connected() {
    qDebug() << "connected";

    // send initial settings
    connection.sendCommand(command.sprintf("setFrequency %lld",band.getFrequency()));
    widget.spectrumFrame->setFrequency(band.getFrequency());
    connection.sendCommand(command.sprintf("setMode %d",band.getMode()));
    connection.sendCommand(command.sprintf("setFilter %d %d",filters.getLow(),filters.getHigh()));
    widget.spectrumFrame->setFilter(filters.getLow(),filters.getHigh());

    // select the audio
    audio.select_audio(widget.audioComboBox->itemData(audio_device).value<QAudioDeviceInfo > ());

    // start the audio
    connection.sendCommand(command.sprintf("SetRXOutputGain %d", gain));
    connection.sendCommand(command.sprintf("startAudioStream %d", 480));

    // start the spectrum
    qTimer= new QTimer(this);
    connect(qTimer, SIGNAL(timeout()), this, SLOT(update()));
    qTimer->start(1000 / fps);


    widget.serverConnectPushButton->setText("Disconnect");
    widget.serverConnectPushButton->setDisabled(FALSE);
    widget.audioComboBox->setDisabled(TRUE);

}

void UI::disconnected(QString message) {
    qDebug() << message;

    widget.statusbar->showMessage(message,5000);

    widget.serverConnectPushButton->setText("Connect");
    widget.serverConnectPushButton->setDisabled(FALSE);
    widget.audioComboBox->setDisabled(FALSE);
}

void UI::update() {
    connection.sendCommand(command.sprintf("getSpectrum %d", widget.spectrumFrame->width()));
}

void UI::receivedHeader(char* header) {
    char spectrumSamples[widget.spectrumFrame->width()];
    char audioSamples[AUDIO_BUFFER_SIZE];
    
    //qDebug() << "UI::receivedHeader: " << (int)header[0];

    switch(header[0]) {
        case 0:
            // spectrum data
            connection.read(&spectrumSamples[0],widget.spectrumFrame->width());
            //qDebug() << "read spectrum samples";
            widget.spectrumFrame->updateSpectrum(header,&spectrumSamples[0]);
            widget.waterfallFrame->updateWaterfall(header,&spectrumSamples[0]);
            break;
        case 1:
            // audio data
            connection.read(&audioSamples[0],AUDIO_BUFFER_SIZE);
            //qDebug() << "read audio samples";
            audio.process_audio(header,&audioSamples[0]);

            break;
    }
}

void UI::setGain(int g) {
    // remember the gain
    gain=g;
    connection.sendCommand(command.sprintf("SetRXOutputGain %d",gain));

    qDebug() << command;
}

void UI::actionSubRx() {
    if(subRx) {
        // on, so turn off
        subRx=FALSE;
        widget.subRxPushButton->setText("Switch On");
        widget.spectrumFrame->setSubRxFrequency(0LL);
    } else {
        subRx=TRUE;
        subRxFrequency=band.getFrequency();
        widget.subRxPushButton->setText("Switch Off");
        connection.sendCommand(command.sprintf("SetSubRXOutputGain %d", subRxGain));
        connection.sendCommand(command.sprintf("SetSubRXFrequency 0"));
    }
    connection.sendCommand(command.sprintf("SetSubRX %d",subRx));

}

void UI::setSubRxGain(int gain) {
    subRxGain=gain;
    connection.sendCommand(command.sprintf("SetSubRXOutputGain %d",gain));

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

    connection.sendCommand(command.sprintf("setMode %d",mode.getMode()));
  
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

    connection.sendCommand(command.sprintf("setFilter %d %d",low,high));
    widget.spectrumFrame->setFilter(low,high);
}

void UI::frequencyChanged(long long frequency) {
    connection.sendCommand(command.sprintf("SetFrequency %lld",frequency));
    widget.spectrumFrame->setFrequency(frequency);
}

void UI::frequencyMoved(int increment) {

    qDebug() << "frequencyMoved " << increment;

    long long f;


    if(subRx) {
        long long diff;
        long long frequency = band.getFrequency();
        f=subRxFrequency+((long long)increment*100LL);
        int samplerate = widget.spectrumFrame->samplerate();
        if ((f >= (frequency - (samplerate / 2))) && (f <= (frequency + (samplerate / 2)))) {
            subRxFrequency = f;
        }
        diff = frequency - subRxFrequency;
        connection.sendCommand(command.sprintf("SetSubRXFrequency %lld",diff));
        qDebug() << "frequencyMoved:subrx: " << increment << "," << subRxFrequency;
        widget.spectrumFrame->setSubRxFrequency(subRxFrequency);
        qDebug() << command;


    } else {
        band.setFrequency(band.getFrequency()-((long long)increment*100LL));
        connection.sendCommand(command.sprintf("setFrequency %lld",band.getFrequency()));
    }
}
