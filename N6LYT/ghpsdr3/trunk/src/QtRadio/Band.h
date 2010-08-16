/* 
 * File:   Band.h
 * Author: john
 *
 * Created on 13 August 2010, 14:52
 */

#ifndef BAND_H
#define	BAND_H

#include <QObject>
#include <QSettings>
#include <QDebug>

#include "BandStackEntry.h"


#define BAND_160 0
#define BAND_80  1
#define BAND_60  2
#define BAND_40  3
#define BAND_30  4
#define BAND_20  5
#define BAND_17  6
#define BAND_15  7
#define BAND_12  8
#define BAND_10  9
#define BAND_6   10
#define BAND_GEN 11
#define BAND_WWV 12
#define BAND_LAST 13

#define BANDSTACK_ENTRIES 5

class Band : public QObject {
    Q_OBJECT
public:
    Band();
    virtual ~Band();
    void initBand(int b);
    void selectBand(int b);
    long long bandSelected(int b,long long currentFrequency);
    int getBandStackEntry();
    void setFrequency(long long f);
    long long getFrequency();
    int getBand();
    int getMode();
    int getFilter();
    int getStep();
    int getSpectrumHigh();
    int getSpectrumLow();
    int getWaterfallHigh();
    int getWaterfallLow();
    void loadSettings(QSettings* settings);
    void saveSettings(QSettings* settings);

signals:
    void bandChanged(int previousBand,int newBand);
    void frequencyChanged(long long newFrequency);

private:
    int currentBand;
    int currentStack;
    int stack[BAND_LAST];
    BandStackEntry bandstack[BAND_LAST][BANDSTACK_ENTRIES];
};

#endif	/* BAND_H */

