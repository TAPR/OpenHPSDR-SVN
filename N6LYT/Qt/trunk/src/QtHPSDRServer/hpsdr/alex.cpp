#include "alex.h"

#include <QDebug>


Alex* Alex::instance = NULL;

Alex* Alex::getInstance(){
  if(instance == NULL){
    instance = new Alex();
  }
  return instance;
}

Alex::Alex(QObject *parent) :
    QObject(parent)
{
    // default all to ANT1
    for(int i=0;i<BANDS;i++) {
        rx[i]=ANT1;
        tx[i]=ANT1;
    }
}

void Alex::configure(QSettings* mysettings) {
    settings=mysettings;
    set(BAND_160,settings->value("160rx",QString("0")).toInt(),settings->value("160tx",QString("0")).toInt());
    set(BAND_80,settings->value("80rx",QString("0")).toInt(),settings->value("80tx",QString("0")).toInt());
    set(BAND_60,settings->value("60rx",QString("0")).toInt(),settings->value("60tx",QString("0")).toInt());
    set(BAND_40,settings->value("40rx",QString("0")).toInt(),settings->value("40tx",QString("0")).toInt());
    set(BAND_30,settings->value("30rx",QString("0")).toInt(),settings->value("30tx",QString("0")).toInt());
    set(BAND_20,settings->value("20rx",QString("0")).toInt(),settings->value("20tx",QString("0")).toInt());
    set(BAND_17,settings->value("17rx",QString("0")).toInt(),settings->value("17tx",QString("0")).toInt());
    set(BAND_15,settings->value("15rx",QString("0")).toInt(),settings->value("15tx",QString("0")).toInt());
    set(BAND_12,settings->value("12rx",QString("0")).toInt(),settings->value("12tx",QString("0")).toInt());
    set(BAND_10,settings->value("10rx",QString("0")).toInt(),settings->value("10tx",QString("0")).toInt());
    set(BAND_6,settings->value("6rx",QString("0")).toInt(),settings->value("6tx",QString("0")).toInt());
    set(BAND_GEN,settings->value("genrx",QString("0")).toInt(),settings->value("gentx",QString("0")).toInt());
}

void Alex::save() {
    settings->setValue("160rx",getRx(BAND_160));
    settings->setValue("160tx",getTx(BAND_160));
    settings->setValue("80rx",getRx(BAND_80));
    settings->setValue("80tx",getTx(BAND_80));
    settings->setValue("60rx",getRx(BAND_60));
    settings->setValue("60tx",getTx(BAND_60));
    settings->setValue("40rx",getRx(BAND_40));
    settings->setValue("40tx",getTx(BAND_40));
    settings->setValue("30rx",getRx(BAND_30));
    settings->setValue("30tx",getTx(BAND_30));
    settings->setValue("20rx",getRx(BAND_20));
    settings->setValue("20tx",getTx(BAND_20));
    settings->setValue("17rx",getRx(BAND_17));
    settings->setValue("17tx",getTx(BAND_17));
    settings->setValue("15rx",getRx(BAND_15));
    settings->setValue("15tx",getTx(BAND_15));
    settings->setValue("12rx",getRx(BAND_12));
    settings->setValue("12tx",getTx(BAND_12));
    settings->setValue("10rx",getRx(BAND_10));
    settings->setValue("10tx",getTx(BAND_10));
    settings->setValue("6rx",getRx(BAND_6));
    settings->setValue("6tx",getTx(BAND_6));
    settings->setValue("genrx",getRx(BAND_GEN));
    settings->setValue("gentx",getTx(BAND_GEN));
}

void Alex::set(int band,int rxant,int txant) {
    if(band<BANDS) {
        rx[band]=rxant;
        tx[band]=txant;
    } else {
        qDebug()<<"Alex::set: invalid band:"<<band;
    }
}

void Alex::setRx(int band,int rxant) {
    if(band<BANDS) {
        rx[band]=rxant;
    } else {
        qDebug()<<"Alex::set: invalid band:"<<band;
    }
}

void Alex::setTx(int band,int txant) {
    if(band<BANDS) {
        tx[band]=txant;
    } else {
        qDebug()<<"Alex::set: invalid band:"<<band;
    }
}

int Alex::getRx(int band) {
    if(band<BANDS) {
        return rx[band];
    }
    qDebug()<<"Alex::getRx: invalid band:"<<band;
    return 0;
}

int Alex::getTx(int band) {
    if(band<BANDS) {
        return tx[band];
    }
    qDebug()<<"Alex::getTx: invalid band:"<<band;
    return 0;
}

QString Alex::getRxAntenna(int band) {
    QString result="Unknown_"+QString::number(band);
    switch(getRx(band)) {
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

QString Alex::getTxAntenna(int band) {
    QString result="Unknown_"+QString::number(band);
    switch(getTx(band)) {
    case ANT1:
        result="ANT_1";
        break;
    case ANT2:
        result="ANT_2";
        break;
    case ANT3:
        result="ANT_3";
        break;
    }
    return result;
}

QString Alex::getBand(int band) {
    QString result="Unknown_"+QString::number(band);
    switch(band) {
    case BAND_160:
        result="BAND_160";
        break;
    case BAND_80:
        result="BAND_80";
        break;
    case BAND_60:
        result="BAND_60";
        break;
    case BAND_40:
        result="BAND_40";
        break;
    case BAND_30:
        result="BAND_30";
        break;
    case BAND_20:
        result="BAND_20";
        break;
    case BAND_17:
        result="BAND_17";
        break;
    case BAND_15:
        result="BAND_15";
        break;
    case BAND_12:
        result="BAND_12";
        break;
    case BAND_10:
        result="BAND_10";
        break;
    case BAND_6:
        result="BAND_6";
        break;
    case BAND_GEN:
        result="BAND_GEN";
        break;
    }
    return result;
}
