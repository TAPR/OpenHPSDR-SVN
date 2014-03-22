#include "bands.h"
#include "../Common/Bands.h"

Bands::Bands() {
    currentBand=0;
    currentAction=NULL;
}

void Bands::loadSettings(QSettings *settings) {
    // ham bands
    allBands.append(new Band(QString("160"),HAM_BAND_160,1800000,2000000,settings));
    allBands.append(new Band(QString("80"),HAM_BAND_80,3500000,3800000,settings));
    allBands.append(new Band(QString("60"),HAM_BAND_60,5258500,5450000,settings));
    allBands.append(new Band(QString("40"),HAM_BAND_40,7000000,7300000,settings));
    allBands.append(new Band(QString("30"),HAM_BAND_30,10100000,10150000,settings));
    allBands.append(new Band(QString("20"),HAM_BAND_20,14000000,14350000,settings));
    allBands.append(new Band(QString("17"),HAM_BAND_17,18068000,18168000,settings));
    allBands.append(new Band(QString("15"),HAM_BAND_15,21000000,21450000,settings));
    allBands.append(new Band(QString("12"),HAM_BAND_12,24890000,24990000,settings));
    allBands.append(new Band(QString("10"),HAM_BAND_10,28000000l,29700000,settings));
    allBands.append(new Band(QString("6"),HAM_BAND_6,50000000,54000000,settings));
    allBands.append(new Band(QString("4"),HAM_BAND_4,70000000,75000000,settings));
    allBands.append(new Band(QString("144"),HAM_BAND_144,144000000,148000000,settings));
    allBands.append(new Band(QString("432"),HAM_BAND_432,420000000,450000000,settings));
    allBands.append(new Band(QString("1240"),HAM_BAND_1240,1240000000,1300000000,settings));
    allBands.append(new Band(QString("2400"),HAM_BAND_2400,2300000000,2450000000,settings));

    // short wave broadcast band
    allBands.append(new Band(QString("120"),BC_BAND_120,2300000,2405000,settings));
    allBands.append(new Band(QString("90"),BC_BAND_90,3200000,3400000,settings));
    allBands.append(new Band(QString("75"),BC_BAND_75,3900000,4000000,settings));
    allBands.append(new Band(QString("60"),BC_BAND_60,4750000,5060000,settings));
    allBands.append(new Band(QString("49"),BC_BAND_49,5900000,6200000,settings));
    allBands.append(new Band(QString("41"),BC_BAND_41,7200000,7450000,settings));
    allBands.append(new Band(QString("31"),BC_BAND_31,9400000,9900000,settings));
    allBands.append(new Band(QString("25"),BC_BAND_25,11600000,12100000,settings));
    allBands.append(new Band(QString("22"),BC_BAND_22,12570000,13870000,settings));
    allBands.append(new Band(QString("19"),BC_BAND_19,15100000,15800000,settings));
    allBands.append(new Band(QString("16"),BC_BAND_16,17480000,17900000,settings));
    allBands.append(new Band(QString("15"),BC_BAND_15,18900000,19020000,settings));
    allBands.append(new Band(QString("13"),BC_BAND_13,21450000,21850000,settings));
    allBands.append(new Band(QString("11"),BC_BAND_11,25600000,26100000,settings));

    allBands.append(new Band(QString("Air"),AIR_BAND,118000000,138000000,settings));

    currentBand=settings->value("currentband",0).toInt();



}

void Bands::saveSettings(QSettings *settings) {
    settings->setValue("currentband",currentBand);
    for(int i=0;i<allBands.length();i++) {
        allBands.at(i)->saveSettings(settings);
    }
}

void Bands::setupBands(long min,long max) {
    bands.clear();
    for(int i=0;i<allBands.length();i++) {
        BandLimit* limits=allBands.at(i)->getBandLimits();
        if(limits->getMin()>=min && limits->getMax()<=max) {
            bands.append(allBands.at(i));
        }
    }
}

void Bands::selectBand(int id) {

    qDebug()<<"Bands::selectBand:"<<id<<" currentBand:"<<currentBand<<" stack:"<<allBands.at(id)->getBandStackEntry();
    if(currentBand==id) {
        allBands.at(id)->nextBandStackEntry();
    } else {
        currentBand=id;
    }
}

QString Bands::getLabel() {
    return allBands.at(currentBand)->getLabel();
}

QString Bands::getLabel(int id) {
    return allBands.at(id)->getLabel();
}

void Bands::setFrequency(quint64 f) {
    allBands.at(currentBand)->setFrequency(f);
}

void Bands::setMode(int value) {
     allBands.at(currentBand)->setMode(value);
}

void Bands::setFilter(int value) {
     allBands.at(currentBand)->setFilter(value);
}

void Bands::setSpectrumLow(int value) {
    allBands.at(currentBand)->setSpectrumLow(value);
}

void Bands::setSpectrumHigh(int value) {
    allBands.at(currentBand)->setSpectrumHigh(value);
}

void Bands::setWaterfallLow(int value) {
    allBands.at(currentBand)->setWaterfallLow(value);
}

void Bands::setWaterfallHigh(int value) {
    allBands.at(currentBand)->setWaterfallHigh(value);
}

quint64 Bands::getFrequency() {
    allBands.at(currentBand)->getFrequency();
}

int Bands::getBand() {
    allBands.at(currentBand)->getId();
}

int Bands::getBandStackEntry() {
    allBands.at(currentBand)->getBandStackEntry();
}

int Bands::getMode() {
    allBands.at(currentBand)->getMode();
}

int Bands::getFilter() {
    allBands.at(currentBand)->getFilter();
}

int Bands::getSpectrumLow() {
    allBands.at(currentBand)->getSpectrumLow();
}

int Bands::getSpectrumHigh() {
    allBands.at(currentBand)->getSpectrumHigh();
}

int Bands::getWaterfallLow() {
    allBands.at(currentBand)->getWaterfallLow();
}

int Bands::getWaterfallHigh() {
    allBands.at(currentBand)->getWaterfallHigh();
}

int Bands::getCurrentBand() {
    return currentBand;
}

BandLimit* Bands::getBandLimits(quint64 minDisplay, quint64 maxDisplay) {

    qDebug() << "Band::getBandLimits: " << minDisplay << "," << maxDisplay;

    BandLimit* limit=new BandLimit();
    for(int i=0;i<bands.length();i++) {
        limit=bands.at(i)->getBandLimits();

        if((limit->getMin()>=minDisplay&&limit->getMin()<=maxDisplay) || // band min within the display
           (limit->getMax()<=maxDisplay&&limit->getMax()>=minDisplay) || // band max within the display
           (minDisplay>=limit->getMin()&&maxDisplay<=limit->getMax())) { // display within a band
            break;
        }
    }

    return limit;
}

void Bands::buildMenu(QMenu* hamMenu, QMenu *broadcastMenu) {
    bool addSeparator=true;
    QAction* action;
    bool bandFound=false;
    int firstBand;
    QAction* firstAction;
    hamMenu->clear();
    broadcastMenu->clear();
    for(int i=0;i<bands.length();i++) {
        action=new QAction(bands.at(i)->getLabel(),this);
        action->setData(QVariant(bands.at(i)->getId()));
        action->setCheckable(true);
        connect(action,SIGNAL(triggered()),this,SLOT(actionBandSelected()));
        if(bands.at(i)->getId()<=HAM_BAND_LAST) {
            hamMenu->addAction(action);
        } else {
            broadcastMenu->addAction(action);
        }

        if(currentBand==bands.at(i)->getId()) {
            bandFound=true;
            currentAction=action;
            action->setChecked(true);
        } else {
            if(i==0) {
                firstBand=bands.at(i)->getId();
                firstAction=action;
            }
        }
    }
    if(!bandFound) {
        currentBand=firstBand;
        currentAction=firstAction;
        firstAction->setChecked(true);
    }
}

bool Bands::exists(int id) {
    bool found=false;
    for(int i=0;i<bands.length();i++) {
        if(bands.at(i)->getId()==id) {
            found=true;
            break;
        }
    }
    return found;
}

void Bands::actionBandSelected() {
    currentAction->setChecked(false);
    currentAction = qobject_cast<QAction *>(sender());
    currentAction->setChecked(true);
    int id=currentAction->data().toInt();

    qDebug()<<"Bands::actionBandSelected:"<<id<<" currentBand:"<<currentBand<<" stack:"<<allBands.at(id)->getBandStackEntry();
    if(currentBand==id) {
        allBands.at(id)->nextBandStackEntry();
    } else {
        currentBand=id;
    }
    emit bandSelected(id);
}
