/*
 * File:   Configure.cpp
 * Author: john
 *
 * Created on 16 August 2010, 20:03
 */

#include <QSettings>
#include <QComboBox>

#include "Configure.h"

Configure::Configure() {
    widget.setupUi(this);

    widget.sampleRateComboBox->addItem("8000");
    widget.sampleRateComboBox->addItem("48000");
    widget.hostComboBox->addItem("192.168.1.82");
    widget.hostComboBox->addItem("127.0.0.1");
    widget.spectrumHighSpinBox->setValue(-40);
    widget.spectrumLowSpinBox->setValue(-160);
    widget.waterfallHighSpinBox->setValue(-60);
    widget.waterfallLowSpinBox->setValue(-120);
    widget.fpsSpinBox->setValue(15);

}

Configure::~Configure() {
}

void Configure::loadSettings(QSettings* settings) {
    int i;
    settings->beginGroup("Servers");
    if(settings->contains("entries")) {
        int entries=settings->value("entries").toInt();
        for(i=0;i<entries;i++) {
            widget.hostComboBox->addItem(settings->value(QString::number(i)).toString());
        }
        widget.hostComboBox->setCurrentIndex(settings->value("selected").toInt());
    }
    if(settings->contains("rx")) widget.rxSpinBox->setValue(settings->value("rx").toInt());
    settings->endGroup();
    settings->beginGroup("Display");
    if(settings->contains("spectrumHigh"))widget.spectrumHighSpinBox->setValue(settings->value("spectrumHigh").toInt());
    if(settings->contains("spectrumLow"))widget.spectrumLowSpinBox->setValue(settings->value("spectrumLow").toInt());
    if(settings->contains("fps"))widget.fpsSpinBox->setValue(settings->value("fps").toInt());
    if(settings->contains("waterfallHigh"))widget.waterfallHighSpinBox->setValue(settings->value("waterfallHigh").toInt());
    if(settings->contains("waterfallLow"))widget.waterfallLowSpinBox->setValue(settings->value("waterfallLow").toInt());
    settings->endGroup();
}

void Configure::saveSettings(QSettings* settings) {
    int i;
    settings->beginGroup("Servers");
    for(i=0;i<widget.hostComboBox->count();i++) {
        settings->setValue(QString::number(i),widget.hostComboBox->itemText(i));
    }
    settings->setValue("selected",widget.hostComboBox->currentIndex());
    settings->setValue("rx",widget.rxSpinBox->value());
    settings->endGroup();
    settings->beginGroup("Display");
    settings->setValue("spectrumHigh",widget.spectrumHighSpinBox->value());
    settings->setValue("spectrumLow",widget.spectrumLowSpinBox->value());
    settings->setValue("fps",widget.fpsSpinBox->value());
    settings->setValue("waterfallHigh",widget.waterfallHighSpinBox->value());
    settings->setValue("waterfallLow",widget.waterfallLowSpinBox->value());
    settings->endGroup();
}

QString Configure::getHost() {
    return widget.hostComboBox->currentText();
}

int Configure::getReceiver() {
    return widget.rxSpinBox->value();
}

int Configure::getSpectrumHigh() {
    return widget.spectrumHighSpinBox->value();
}

int Configure::getSpectrumLow() {
    return widget.spectrumLowSpinBox->value();
}

int Configure::getFps() {
    return widget.fpsSpinBox->value();
}

int Configure::getWaterfallHigh() {
    return widget.waterfallHighSpinBox->value();
}

int Configure::getWaterfallLow() {
    return widget.waterfallLowSpinBox->value();
}
