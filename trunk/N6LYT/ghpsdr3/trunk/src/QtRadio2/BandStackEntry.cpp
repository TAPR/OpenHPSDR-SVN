/* 
 * File:   BandStackEntry.cpp
 * Author: john
 * 
 * Created on 13 August 2010, 16:47
 */

#include "BandStackEntry.h"

BandStackEntry::BandStackEntry() {
}

BandStackEntry::~BandStackEntry() {
}

void BandStackEntry::setFrequency(long long f) {
    frequency=f;
}

void BandStackEntry::setMode(int m) {
    mode=m;
}

void BandStackEntry::setFilter(int f) {
    filter=f;
}

void BandStackEntry::setStep(int s) {
    step=s;
}

void BandStackEntry::setSpectrumHigh(int high) {
    spectrumHigh=high;
}

void BandStackEntry::setSpectrumLow(int low) {
    spectrumLow=low;
}

void BandStackEntry::setWaterfallHigh(int high) {
    waterfallHigh=high;
}

void BandStackEntry::setWaterfallLow(int low) {
    waterfallLow=low;
}


long long BandStackEntry::getFrequency() {
    return frequency;
}

int BandStackEntry::getMode() {
    return mode;
}

int BandStackEntry::getFilter() {
    return filter;
}

int BandStackEntry::getStep() {
    return step;
}

int BandStackEntry::getSpectrumHigh() {
    return spectrumHigh;
}

int BandStackEntry::getSpectrumLow() {
    return spectrumLow;
}

int BandStackEntry::getWaterfallHigh() {
    return waterfallHigh;
}

int BandStackEntry::getWaterfallLow() {
    return waterfallLow;
}
