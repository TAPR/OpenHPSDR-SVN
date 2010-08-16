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