/*
 * File:   Band.h
 * Author: John Melton, G0ORX/N6LYT
 *
 * Created on 13 August 2010, 14:52
 */

/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/

#ifndef BAND_H
#define	BAND_H

#include <QAction>
#include <QObject>
#include <QSettings>
#include <QDebug>

#include "BandStackEntry.h"
#include "BandLimit.h"

#define BANDSTACK_ENTRIES 5

class Band : public QObject {
    Q_OBJECT
public:
    Band(QString label,int id, int min,int max,QSettings* settings);
    virtual ~Band();

    int getId();
    QString getLabel();

    //void initBand(int b);
    //void selectBand(int b);
    //quint64 bandSelected(int b,quint64 currentFrequency);
    void nextBandStackEntry();
    int getBandStackEntry();
    void setFrequency(quint64 f);
    quint64 getFrequency();
    int getBand();
    QString getStringBand();
    QString getStringBand(int band);
    int getMode();
    int getFilter();
    int getStep();
    int getSpectrumHigh();
    int getSpectrumLow();
    int getWaterfallHigh();
    int getWaterfallLow();
    void setMode(int m);
    void setFilter(int f);
    void setSpectrumHigh(int h);
    void setSpectrumLow(int l);
    void setWaterfallHigh(int h);
    void setWaterfallLow(int l);
    void loadSettings(QSettings* settings);
    void saveSettings(QSettings* settings);
    BandLimit* getBandLimits();

    void setupBands(long min,long max);

signals:
    void bandChanged(int previousBand,int newBand);

private:
    QString label;
    int id;
    int stackEntry;
    quint64 minFrequency;
    quint64 maxFrequency;

    BandStackEntry bandstack[BANDSTACK_ENTRIES];
    int currentStack;

    BandLimit bandLimit;
    /*
    int currentBand;
    int stack[BAND_LAST];
    BandStackEntry bandstack[BAND_LAST][BANDSTACK_ENTRIES];
    QVector <BandLimit> limits;
    */
};

#endif	/* BAND_H */

