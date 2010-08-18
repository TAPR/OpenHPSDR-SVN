/* 
 * File:   Band.cpp
 * Author: john
 * 
 * Created on 13 August 2010, 14:52
 */

#include "Band.h"
#include "Mode.h"

Band::Band() {
    int i;

    currentBand=BAND_40;
    currentStack=0;
    for(i=0;i<BAND_LAST;i++) {
        stack[i]=0;
    }

    // bandstack entries max of BANDSTACK_ENTRIES or frequency 0
    bandstack[BAND_160][0].setFrequency(1810000LL);
    bandstack[BAND_160][0].setMode(MODE_CWL);
    bandstack[BAND_160][0].setFilter(4);
    bandstack[BAND_160][1].setFrequency(1835000LL);
    bandstack[BAND_160][1].setMode(MODE_LSB);
    bandstack[BAND_160][1].setFilter(4);
    bandstack[BAND_160][2].setFrequency(1845000LL);
    bandstack[BAND_160][2].setMode(MODE_LSB);
    bandstack[BAND_160][2].setFilter(3);
    bandstack[BAND_160][3].setFrequency(0LL);

    bandstack[BAND_80][0].setFrequency(3501000LL);
    bandstack[BAND_80][0].setMode(MODE_CWL);
    bandstack[BAND_80][0].setFilter(4);
    bandstack[BAND_80][1].setFrequency(3751000LL);
    bandstack[BAND_80][1].setMode(MODE_LSB);
    bandstack[BAND_80][1].setFilter(3);
    bandstack[BAND_80][2].setFrequency(3850000LL);
    bandstack[BAND_80][2].setMode(MODE_LSB);
    bandstack[BAND_80][2].setFilter(3);
    bandstack[BAND_80][3].setFrequency(0LL);

    bandstack[BAND_60][0].setFrequency(5330500LL);
    bandstack[BAND_60][0].setMode(MODE_CWL);
    bandstack[BAND_60][0].setFilter(3);
    bandstack[BAND_60][1].setFrequency(5346500LL);
    bandstack[BAND_60][1].setMode(MODE_LSB);
    bandstack[BAND_60][1].setFilter(3);
    bandstack[BAND_60][2].setFrequency(5366500LL);
    bandstack[BAND_60][2].setMode(MODE_LSB);
    bandstack[BAND_60][2].setFilter(3);
    bandstack[BAND_60][3].setFrequency(5371500LL);
    bandstack[BAND_60][3].setMode(MODE_LSB);
    bandstack[BAND_60][3].setFilter(3);
    bandstack[BAND_60][4].setFrequency(5403500LL);
    bandstack[BAND_60][4].setMode(MODE_LSB);
    bandstack[BAND_60][4].setFilter(3);

    bandstack[BAND_40][0].setFrequency(7001000LL);
    bandstack[BAND_40][0].setMode(MODE_CWL);
    bandstack[BAND_40][0].setFilter(4);
    bandstack[BAND_40][1].setFrequency(7056000LL);
    bandstack[BAND_40][1].setMode(MODE_LSB);
    bandstack[BAND_40][1].setFilter(3);
    bandstack[BAND_40][2].setFrequency(7120000LL);
    bandstack[BAND_40][2].setMode(MODE_LSB);
    bandstack[BAND_40][2].setFilter(3);
    bandstack[BAND_40][3].setFrequency(0LL);

    bandstack[BAND_30][0].setFrequency(10120000LL);
    bandstack[BAND_30][0].setMode(MODE_CWU);
    bandstack[BAND_30][0].setFilter(4);
    bandstack[BAND_30][1].setFrequency(10130000LL);
    bandstack[BAND_30][1].setMode(MODE_CWU);
    bandstack[BAND_30][1].setFilter(4);
    bandstack[BAND_30][2].setFrequency(10140000LL);
    bandstack[BAND_30][2].setMode(MODE_CWU);
    bandstack[BAND_30][2].setFilter(4);
    bandstack[BAND_30][3].setFrequency(0LL);

    bandstack[BAND_20][0].setFrequency(14010000LL);
    bandstack[BAND_20][0].setMode(MODE_CWU);
    bandstack[BAND_20][0].setFilter(4);
    bandstack[BAND_20][1].setFrequency(14230000LL);
    bandstack[BAND_20][1].setMode(MODE_USB);
    bandstack[BAND_20][1].setFilter(3);
    bandstack[BAND_20][2].setFrequency(14336000LL);
    bandstack[BAND_20][2].setMode(MODE_USB);
    bandstack[BAND_20][2].setFilter(3);
    bandstack[BAND_20][3].setFrequency(0LL);

    bandstack[BAND_17][0].setFrequency(18068600LL);
    bandstack[BAND_17][0].setMode(MODE_CWU);
    bandstack[BAND_17][0].setFilter(4);
    bandstack[BAND_17][1].setFrequency(18125000LL);
    bandstack[BAND_17][1].setMode(MODE_USB);
    bandstack[BAND_17][1].setFilter(3);
    bandstack[BAND_17][2].setFrequency(18140000LL);
    bandstack[BAND_17][2].setMode(MODE_USB);
    bandstack[BAND_17][2].setFilter(3);
    bandstack[BAND_17][3].setFrequency(0LL);

    bandstack[BAND_15][0].setFrequency(21001000LL);
    bandstack[BAND_15][0].setMode(MODE_CWU);
    bandstack[BAND_15][0].setFilter(4);
    bandstack[BAND_15][1].setFrequency(21255000LL);
    bandstack[BAND_15][1].setMode(MODE_USB);
    bandstack[BAND_15][1].setFilter(3);
    bandstack[BAND_15][2].setFrequency(21300000LL);
    bandstack[BAND_15][2].setMode(MODE_USB);
    bandstack[BAND_15][2].setFilter(3);
    bandstack[BAND_15][3].setFrequency(0LL);

    bandstack[BAND_12][0].setFrequency(24895000LL);
    bandstack[BAND_12][0].setMode(MODE_CWU);
    bandstack[BAND_12][0].setFilter(4);
    bandstack[BAND_12][1].setFrequency(24900000LL);
    bandstack[BAND_12][1].setMode(MODE_CWU);
    bandstack[BAND_12][1].setFilter(4);
    bandstack[BAND_12][2].setFrequency(24910000LL);
    bandstack[BAND_12][2].setMode(MODE_CWU);
    bandstack[BAND_12][2].setFilter(4);
    bandstack[BAND_12][3].setFrequency(0LL);

    bandstack[BAND_10][0].setFrequency(28010000LL);
    bandstack[BAND_10][0].setMode(MODE_CWU);
    bandstack[BAND_10][0].setFilter(4);
    bandstack[BAND_10][1].setFrequency(28300000LL);
    bandstack[BAND_10][1].setMode(MODE_USB);
    bandstack[BAND_10][1].setFilter(3);
    bandstack[BAND_10][2].setFrequency(28400000LL);
    bandstack[BAND_10][2].setMode(MODE_USB);
    bandstack[BAND_10][2].setFilter(3);
    bandstack[BAND_10][3].setFrequency(0LL);

    bandstack[BAND_6][0].setFrequency(50010000LL);
    bandstack[BAND_6][0].setMode(MODE_CWU);
    bandstack[BAND_6][0].setFilter(4);
    bandstack[BAND_6][1].setFrequency(50125000LL);
    bandstack[BAND_6][1].setMode(MODE_USB);
    bandstack[BAND_6][1].setFilter(3);
    bandstack[BAND_6][2].setFrequency(50200000LL);
    bandstack[BAND_6][2].setMode(MODE_USB);
    bandstack[BAND_6][2].setFilter(3);
    bandstack[BAND_6][3].setFrequency(0LL);

    bandstack[BAND_GEN][0].setFrequency(909000LL);
    bandstack[BAND_GEN][0].setMode(MODE_AM);
    bandstack[BAND_GEN][0].setFilter(3);
    bandstack[BAND_GEN][1].setFrequency(6145000LL);
    bandstack[BAND_GEN][1].setMode(MODE_AM);
    bandstack[BAND_GEN][1].setFilter(3);
    bandstack[BAND_GEN][2].setFrequency(11765000LL);
    bandstack[BAND_GEN][2].setMode(MODE_AM);
    bandstack[BAND_GEN][2].setFilter(3);
    bandstack[BAND_GEN][3].setFrequency(15400000LL);
    bandstack[BAND_GEN][3].setMode(MODE_AM);
    bandstack[BAND_GEN][3].setFilter(3);
    bandstack[BAND_GEN][4].setFrequency(17795000LL);
    bandstack[BAND_GEN][4].setMode(MODE_AM);
    bandstack[BAND_GEN][4].setFilter(3);

    bandstack[BAND_WWV][0].setFrequency(2500000LL);
    bandstack[BAND_WWV][0].setMode(MODE_AM);
    bandstack[BAND_WWV][0].setFilter(3);
    bandstack[BAND_WWV][1].setFrequency(5000000LL);
    bandstack[BAND_WWV][1].setMode(MODE_AM);
    bandstack[BAND_WWV][1].setFilter(3);
    bandstack[BAND_WWV][2].setFrequency(10000000LL);
    bandstack[BAND_WWV][2].setMode(MODE_AM);
    bandstack[BAND_WWV][2].setFilter(3);
    bandstack[BAND_WWV][3].setFrequency(15000000LL);
    bandstack[BAND_WWV][3].setMode(MODE_AM);
    bandstack[BAND_WWV][3].setFilter(3);
    bandstack[BAND_WWV][4].setFrequency(20000000LL);
    bandstack[BAND_WWV][4].setMode(MODE_AM);
    bandstack[BAND_WWV][4].setFilter(3);
}

Band::~Band() {
    // save state
    
}

void Band::loadSettings(QSettings* settings) {
    settings->beginGroup("Band");
    currentBand=settings->value("currentBand").toInt();
    currentStack=settings->value("currentStack").toInt();
    settings->endGroup();
}

void Band::saveSettings(QSettings* settings) {
    int i,j;
    QString s;
    
    settings->beginGroup("Band");
    settings->setValue("currentBand",currentBand);
    settings->setValue("currentStack",currentStack);

    for(i=0;i<BAND_LAST;i++) {
        s.sprintf("%d",i);
        settings->beginGroup(s);
        for(j=0;j<BANDSTACK_ENTRIES;j++) {
            if(bandstack[i][j].getFrequency()!=0LL) {
                s.sprintf("%d",j);
                settings->beginGroup(s);
                settings->setValue("frequency",bandstack[i][j].getFrequency());
                settings->setValue("filter",bandstack[currentBand][currentStack].getFilter());
                settings->setValue("mode",bandstack[currentBand][currentStack].getMode());
    //            settings->setValue("step",bandstack[currentBand][currentStack].getStep());
    //            settings->setValue("spectrumLow",bandstack[currentBand][currentStack].getSpectrumLow());
    //            settings->setValue("spectrumHigh",bandstack[currentBand][currentStack].getSpectrumHigh());
    //            settings->setValue("waterfallHigh",bandstack[currentBand][currentStack].getWaterfallHigh());
    //            settings->setValue("waterfallLow",bandstack[currentBand][currentStack].getWaterfallLow());
                settings->endGroup();
            }
        }
        settings->endGroup();

    }
    
    settings->endGroup();
}

void Band::initBand(int b) {
    currentBand=b;
    emit bandChanged(currentBand, currentBand);
    emit frequencyChanged(bandstack[currentBand][currentStack].getFrequency());
}

void Band::selectBand(int b) {
    int previousBand=currentBand;
    currentBand=b;

    if(previousBand==currentBand) {
        // step through band stack
        currentStack++;
        if (currentStack == BANDSTACK_ENTRIES) {
            currentStack = 0;
        } else if (bandstack[currentBand][currentStack].getFrequency() == 0LL) {
            currentStack = 0;
        }
    } else {
        // new band
        currentStack=stack[currentBand];
    }

    qDebug() << "selectBand " << currentBand << ":" << currentStack;
    
    emit bandChanged(previousBand,currentBand);
    emit frequencyChanged(bandstack[currentBand][currentStack].getFrequency());
}

int Band::getBand() {
    return currentBand;
}

QString Band::getStringBand() {
    QString b="Gen";

    switch(currentBand) {
        case BAND_160:
            b="160 Mtrs";
            break;
        case BAND_80:
            b="80 Mtrs";
            break;
        case BAND_60:
            b="60 Mtrs";
            break;
        case BAND_40:
            b="40 Mtrs";
            break;
        case BAND_30:
            b="30 Mtrs";
            break;
        case BAND_20:
            b="20 Mtrs";
            break;
        case BAND_17:
            b="17 Mtrs";
            break;
        case BAND_15:
            b="15 Mtrs";
            break;
        case BAND_12:
            b="12 Mtrs";
            break;
        case BAND_10:
            b="10 Mtrs";
            break;
        case BAND_6:
            b="6 Mtrs";
            break;
        case BAND_GEN:
            b="Gen";
            break;
        case BAND_WWV:
            b="WWV";
            break;
    }

    return b;
}

long long Band::bandSelected(int b,long long currentFrequency) {
    long long f;

    // save the current frequency in the current bandstack entry
    bandstack[currentBand][currentStack].setFrequency(currentFrequency);

    if(currentBand==b) {
        // step through band stack
        currentStack++;
        if(currentStack==BANDSTACK_ENTRIES) {
            currentStack=0;
        } else if(bandstack[currentBand][currentStack].getFrequency()==0LL) {
            currentStack=0;
        }
        

        qDebug() << "same band currentStack " << currentStack;

    } else {
        // save the current stack
        stack[currentBand]=currentStack;

        // change the band
        currentBand=b;
        // get the last stack entry used
        currentStack=stack[currentBand];

        qDebug() << "change band currentStack " << currentStack;
        
    }
    
    f = bandstack[currentBand][currentStack].getFrequency();

    return f;
}

int Band::getBandStackEntry() {
    return currentStack;
}

void Band::setFrequency(long long f) {
    bandstack[currentBand][currentStack].setFrequency(f);
    emit frequencyChanged(bandstack[currentBand][currentStack].getFrequency());
}

long long Band::getFrequency() {
    return bandstack[currentBand][currentStack].getFrequency();
}

int Band::getMode() {
    return bandstack[currentBand][currentStack].getMode();
}

int Band::getFilter() {
    return bandstack[currentBand][currentStack].getFilter();
}

int Band::getStep() {
    return bandstack[currentBand][currentStack].getStep();
}

int Band::getSpectrumHigh() {
    return bandstack[currentBand][currentStack].getSpectrumHigh();
}

int Band::getSpectrumLow() {
    return bandstack[currentBand][currentStack].getSpectrumLow();
}

int Band::getWaterfallHigh() {
    return bandstack[currentBand][currentStack].getWaterfallHigh();
}

int Band::getWaterfallLow() {
    return bandstack[currentBand][currentStack].getWaterfallLow();
}