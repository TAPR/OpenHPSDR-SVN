/* 
 * File:   BandStackEntry.h
 * Author: john
 *
 * Created on 13 August 2010, 16:47
 */

#ifndef BANDSTACKENTRY_H
#define	BANDSTACKENTRY_H

#include <QObject>

class BandStackEntry : public QObject {
    Q_OBJECT
public:
    BandStackEntry();
    virtual ~BandStackEntry();

    void setFrequency(long long f);
    void setMode(int m);
    void setFilter(int f);
    void setStep(int s);
    void setSpectrumHigh(int high);
    void setSpectrumLow(int low);
    void setWaterfallHigh(int high);
    void setWaterfallLow(int low);

    long long getFrequency();
    int getMode();
    int getFilter();
    int getStep();
    int getSpectrumHigh();
    int getSpectrumLow();
    int getWaterfallHigh();
    int getWaterfallLow();

private:
    long long frequency;
    int mode;
    int filter;
    int step;
    int spectrumHigh;
    int spectrumLow;
    int spectrumStep;
    int waterfallHigh;
    int waterfallLow;
};

#endif	/* BANDSTACKENTRY_H */

