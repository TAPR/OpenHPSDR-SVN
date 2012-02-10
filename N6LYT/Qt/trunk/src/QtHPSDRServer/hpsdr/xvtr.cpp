#include "xvtr.h"

XVTR::XVTR(QObject *parent) :
    QObject(parent)
{
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
