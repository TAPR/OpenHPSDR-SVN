/*
 * File:   Band.cpp
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

#include "Band.h"
#include "Mode.h"
#include "../Common/Bands.h"

Band::Band(QString label, int id, int min,int max,QSettings *settings) {

    // setup defaults
    this->label=label;
    this->id=id;
    bandLimit.setMin(min);
    bandLimit.setMax(max);

    // update from settings
    settings->beginGroup("BAND_"+label);
    if(settings->contains("id")) this->id=settings->value("id").toInt();
    if(settings->contains("label")) this->label=settings->value("label").toString();
    if(settings->contains("minfrequency")) bandLimit.setMin(settings->value("minfrequency").toLongLong());
    if(settings->contains("maxfrequency")) bandLimit.setMax(settings->value("maxfrequency").toLongLong());

    // default bandstack entries
    for(int i=0;i<BANDSTACK_ENTRIES;i++) {
        bandstack[i].setFrequency(min);
        bandstack[i].setFilter(0);
        bandstack[i].setMode(0);
        bandstack[i].setSpectrumHigh(-40);
        bandstack[i].setSpectrumLow(-140);
        bandstack[i].setWaterfallHigh(-60);
        bandstack[i].setWaterfallLow(-125);
    }
    // default band specific bandstack entries
    switch(id) {
    case HAM_BAND_160:
        bandstack[0].setFrequency(1820000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(3);
        bandstack[1].setFrequency(1840000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(3);
        bandstack[2].setFrequency(1900000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(0);
        bandstack[3].setFrequency(1920000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(0);
        bandstack[4].setFrequency(1940000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(0);
        break;
    case HAM_BAND_80:
        bandstack[0].setFrequency(1820000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(3);
        bandstack[1].setFrequency(1840000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(3);
        bandstack[2].setFrequency(1900000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(0);
        bandstack[3].setFrequency(1920000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(0);
        bandstack[4].setFrequency(1940000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(0);
        break;
    case HAM_BAND_60:
        bandstack[0].setFrequency(3520000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(3);
        bandstack[1].setFrequency(3540000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(3);
        bandstack[2].setFrequency(3600000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(0);
        bandstack[3].setFrequency(3650000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(0);
        bandstack[4].setFrequency(3750000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(0);
        break;
    case HAM_BAND_40:
        bandstack[0].setFrequency(7010000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(3);
        bandstack[1].setFrequency(70400000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(3);
        bandstack[2].setFrequency(7050000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(0);
        bandstack[3].setFrequency(7100000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(0);
        bandstack[4].setFrequency(7150000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(0);
        break;
    case HAM_BAND_30:
        bandstack[0].setFrequency(10200000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(4);
        bandstack[1].setFrequency(10300000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(4);
        bandstack[2].setFrequency(10400000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(1);
        bandstack[3].setFrequency(10100000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(1);
        bandstack[4].setFrequency(10140000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(1);
        break;
    case HAM_BAND_20:
        bandstack[0].setFrequency(14010000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(4);
        bandstack[1].setFrequency(14020000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(4);
        bandstack[2].setFrequency(14050000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(1);
        bandstack[3].setFrequency(14150000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(1);
        bandstack[4].setFrequency(14250000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(1);
        break;
    case HAM_BAND_17:
        bandstack[0].setFrequency(18070000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(4);
        bandstack[1].setFrequency(18096000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(4);
        bandstack[2].setFrequency(18100000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(1);
        bandstack[3].setFrequency(18120000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(1);
        bandstack[4].setFrequency(18150000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(1);
        break;
    case HAM_BAND_15:
        bandstack[0].setFrequency(21030000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(4);
        bandstack[1].setFrequency(21070000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(4);
        bandstack[2].setFrequency(21100000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(1);
        bandstack[3].setFrequency(21200000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(1);
        bandstack[4].setFrequency(21300000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(1);
        break;
    case HAM_BAND_12:
        bandstack[0].setFrequency(24900000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(4);
        bandstack[1].setFrequency(24930000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(4);
        bandstack[2].setFrequency(24950000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(1);
        bandstack[3].setFrequency(24960000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(1);
        bandstack[4].setFrequency(24970000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(1);
        break;
    case HAM_BAND_10:
        bandstack[0].setFrequency(28030000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(4);
        bandstack[1].setFrequency(28070000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(4);
        bandstack[2].setFrequency(28100000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(1);
        bandstack[3].setFrequency(28500000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(1);
        bandstack[4].setFrequency(29000000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(1);
        break;
    case HAM_BAND_6:
        bandstack[0].setFrequency(50030000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(4);
        bandstack[1].setFrequency(50070000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(4);
        bandstack[2].setFrequency(50100000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(1);
        bandstack[3].setFrequency(50500000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(1);
        bandstack[4].setFrequency(51500000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(1);
        break;
    case HAM_BAND_4:
        bandstack[0].setFrequency(70030000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(4);
        bandstack[1].setFrequency(70070000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(4);
        bandstack[2].setFrequency(70100000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(1);
        bandstack[3].setFrequency(70200000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(1);
        bandstack[4].setFrequency(70300000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(1);
        break;
    case HAM_BAND_144:
        bandstack[0].setFrequency(144050000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(4);
        bandstack[1].setFrequency(144100000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(4);
        bandstack[2].setFrequency(144150000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(1);
        bandstack[3].setFrequency(144300000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(1);
        bandstack[4].setFrequency(145000000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(1);
        break;
    case HAM_BAND_432:
        bandstack[0].setFrequency(430050000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(4);
        bandstack[1].setFrequency(430100000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(4);
        bandstack[2].setFrequency(430300000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(1);
        bandstack[3].setFrequency(432150000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(1);
        bandstack[4].setFrequency(432800000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(1);
        break;
    case HAM_BAND_1240:
        bandstack[0].setFrequency(1240100000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(4);
        bandstack[1].setFrequency(1272000000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(4);
        bandstack[2].setFrequency(1296000000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(1);
        bandstack[3].setFrequency(1296600000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(1);
        bandstack[4].setFrequency(1299000000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(1);
        break;
    case HAM_BAND_2400:
        bandstack[0].setFrequency(2310100000);
        bandstack[0].setFilter(5);
        bandstack[0].setMode(4);
        bandstack[1].setFrequency(2310300000);
        bandstack[1].setFilter(5);
        bandstack[1].setMode(4);
        bandstack[2].setFrequency(2310500000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(1);
        bandstack[3].setFrequency(2320000000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(1);
        bandstack[4].setFrequency(2330000000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(1);
        break;
    case AIR_BAND:
        bandstack[0].setFrequency(121800000);
        bandstack[0].setFilter(3);
        bandstack[0].setMode(MODE_AM);
        bandstack[1].setFrequency(124225000);
        bandstack[1].setFilter(3);
        bandstack[1].setMode(MODE_AM);
        bandstack[2].setFrequency(126825000);
        bandstack[2].setFilter(3);
        bandstack[2].setMode(MODE_AM);
        bandstack[3].setFrequency(118000000);
        bandstack[3].setFilter(3);
        bandstack[3].setMode(MODE_AM);
        bandstack[4].setFrequency(127000000);
        bandstack[4].setFilter(3);
        bandstack[4].setMode(MODE_AM);
        break;
    default:
        for(int i=0;i<BANDSTACK_ENTRIES;i++) {
            bandstack[i].setFrequency(min);
            bandstack[i].setFilter(3);
            bandstack[i].setMode(MODE_AM);
            bandstack[i].setSpectrumHigh(-40);
            bandstack[i].setSpectrumLow(-140);
            bandstack[i].setWaterfallHigh(-60);
            bandstack[i].setWaterfallLow(-125);
        }
        break;
    }

    for(int i=0;i<BANDSTACK_ENTRIES;i++) {
        settings->beginGroup("BANDSTACK_"+QString::number(i));
        if(settings->contains("frequency")) bandstack[i].setFrequency(settings->value("frequency").toLongLong());
        if(settings->contains("filter")) bandstack[i].setFilter(settings->value("filter").toInt());
        if(settings->contains("mode")) bandstack[i].setMode(settings->value("mode").toInt());
        if(settings->contains("spectrumhigh")) bandstack[i].setSpectrumHigh(settings->value("spectrumhigh").toInt());
        if(settings->contains("spectrumlow")) bandstack[i].setSpectrumLow(settings->value("spectrumlow").toInt());
        if(settings->contains("waterfallhigh")) bandstack[i].setWaterfallHigh(settings->value("waterfallhigh").toInt());
        if(settings->contains("waterfalllow")) bandstack[i].setWaterfallLow(settings->value("waterfalllow").toInt());
        settings->endGroup();
    }

    currentStack=0;
    if(settings->contains("currentstack")) currentStack=settings->value("currentstack").toInt();
    settings->endGroup();
}

Band::~Band() {
    // save state
    
}

int Band::getId() {
    return id;
}



void Band::saveSettings(QSettings* settings) {
    
    settings->beginGroup("Band_"+label);
    settings->setValue("currentstack",currentStack);
    for(int i=0;i<BANDSTACK_ENTRIES;i++) {
        settings->beginGroup("BANDSTACK_"+QString::number(i));
        settings->setValue("frequency",bandstack[i].getFrequency());
        settings->setValue("filter",bandstack[i].getFilter());
        settings->setValue("mode",bandstack[i].getMode());
        settings->setValue("spectrumhigh",bandstack[i].getSpectrumHigh());
        settings->setValue("spectrumlow",bandstack[i].getSpectrumLow());
        settings->setValue("waterfallhigh",bandstack[i].getWaterfallHigh());
        settings->setValue("waterfalllow",bandstack[i].getWaterfallLow());
        settings->endGroup();
    }
    settings->endGroup();
}

void Band::nextBandStackEntry() {
    currentStack++;
    if(currentStack==BANDSTACK_ENTRIES) {
        currentStack=0;
    }
}

QString Band::getLabel() {
    qDebug()<<"Band::getLabel:"<<label;
    return label;
}

int Band::getBandStackEntry() {
    return currentStack;
}


quint64 Band::getFrequency() {
    return bandstack[currentStack].getFrequency();
}

int Band::getMode() {
    return bandstack[currentStack].getMode();
}

int Band::getFilter() {
    qDebug()<<"Band::getFilter: currentStack:"<<currentStack;
    return bandstack[currentStack].getFilter();
}

int Band::getStep() {
    return bandstack[currentStack].getStep();
}

int Band::getSpectrumHigh() {
    return bandstack[currentStack].getSpectrumHigh();
}

int Band::getSpectrumLow() {
    return bandstack[currentStack].getSpectrumLow();
}

int Band::getWaterfallHigh() {
    return bandstack[currentStack].getWaterfallHigh();
}

int Band::getWaterfallLow() {
    return bandstack[currentStack].getWaterfallLow();
}

BandLimit* Band::getBandLimits() {
    return &bandLimit;
}

void Band::setFrequency(quint64 f) {
    bandstack[currentStack].setFrequency(f);
}

void Band::setMode(int m) {
    bandstack[currentStack].setMode(m);
}

void Band::setFilter(int f) {
    bandstack[currentStack].setFilter(f);
}

void Band::setSpectrumHigh(int h) {
    bandstack[currentStack].setSpectrumHigh(h);
}

void Band::setSpectrumLow(int l) {
    bandstack[currentStack].setSpectrumLow(l);
}

void Band::setWaterfallHigh(int h) {
    bandstack[currentStack].setWaterfallHigh(h);
}

void Band::setWaterfallLow(int l) {
    bandstack[currentStack].setWaterfallLow(l);
}
