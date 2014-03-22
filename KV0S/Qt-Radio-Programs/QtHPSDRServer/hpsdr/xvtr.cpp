#include "xvtr.h"

XVTR::XVTR(QObject *parent) :
    QObject(parent)
{
    label="";
    minFrequency=0;
    maxFrequency=0;
    LOFrequency=0;
    rxAnt=0;
    txAnt=0;
}

void XVTR::setLabel(QString l) {
    label=l;
}

void XVTR::setMinFrequency(qint64 f) {
    minFrequency=f;
}

void XVTR::setMaxFrequency(qint64 f) {
    maxFrequency=f;
}

void XVTR::setLOFrequency(qint64 f) {
    LOFrequency=f;
}

QString XVTR::getLabel() {
    return label;
}

qint64 XVTR::getMinFrequency() {
    return minFrequency;
}

qint64 XVTR::getMaxFrequency() {
    return maxFrequency;
}

qint64 XVTR::getLOFrequency() {
    return LOFrequency;
}

void XVTR::setRx(int ant) {
    rxAnt=ant;
}

void XVTR::setTx(int ant) {
     txAnt=ant;
}

int XVTR::getRx() {
    return rxAnt;
}

int XVTR::getTx() {
    return txAnt;
}

QString XVTR::getRxAntenna() {
    QString result="Unknown";
    switch(rxAnt) {
    case ANT1:
        result="ANT_1";
        break;
    case ANT2:
        result="ANT_2";
        break;
    case ANT3:
        result="ANT_3";
        break;
    case RX1:
        result="RX_1";
        break;
    case RX2:
        result="RX_1";
        break;
    case XVRX:
        result="XVRX";
        break;
    }
    return result;
}

QString XVTR::getTxAntenna() {
    QString result="Unknown";
    switch(txAnt) {
    case ANT1:
        result="ANT_1";
        break;
    case ANT2:
        result="ANT_2";
        break;
    case ANT3:
        result="ANT_3";
        break;
    case 3:
        result="RX_ONLY";
        break;
    }
    return result;
}
