/* 
 * File:   Mode.cpp
 * Author: john
 * 
 * Created on 15 August 2010, 07:46
 */


#include "Mode.h"

Mode::Mode() {
    currentMode=MODE_LSB;
}

Mode::~Mode() {
}

void Mode::setMode(int m) {
    int previousMode=currentMode;
    currentMode=m;
    emit modeChanged(previousMode,currentMode);
}

int Mode::getMode() {
    return currentMode;
}

QString Mode::getStringMode() {
    QString m="INV";

    switch(currentMode) {
        case MODE_LSB:
            m="LSB";
            break;
        case MODE_USB:
            m="USB";
            break;
        case MODE_DSB:
            m="DSB";
            break;
        case MODE_CWL:
            m="CWL";
            break;
        case MODE_CWU:
            m="CWU";
            break;
        case MODE_FMN:
            m="FMN";
            break;
        case MODE_AM:
            m="AM";
            break;
        case MODE_DIGU:
            m="DIGU";
            break;
        case MODE_SPEC:
            m="SPEC";
            break;
        case MODE_DIGL:
            m="DIGL";
            break;
        case MODE_SAM:
            m="SAM";
            break;
        case MODE_DRM:
            m="DRM";
            break;
    }

    return m;
}