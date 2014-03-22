#include "XvtrEntry.h"

XvtrEntry::XvtrEntry() {
}

XvtrEntry::XvtrEntry(int e,QString t,quint64 minF,quint64 maxF,quint64 ifF,quint64 freq,int m,int filt) {
    entry=e;
    title=t;
    minFrequency=minF;
    maxFrequency=maxF;
    ifFrequency=ifF;
    frequency=freq;
    mode=m;
    filter=filt;
}

int XvtrEntry::getEntry() {
    return entry;
}

QString XvtrEntry::getTitle() {
    return title;
}

quint64 XvtrEntry::getMinFrequency() {
    return minFrequency;
}

quint64 XvtrEntry::getMaxFrequency() {
    return maxFrequency;
}

quint64 XvtrEntry::getIFFrequency() {
    return ifFrequency;
}

quint64 XvtrEntry::getFrequency() {
    return frequency;
}

int XvtrEntry::getMode() {
    return mode;
}

int XvtrEntry::getFilter() {
    return filter;
}

void XvtrEntry::setFrequency(quint64 f) {
    frequency=f;
}

void XvtrEntry::setMode(int m) {
    mode=m;
}

void XvtrEntry::setFilter(int f) {
    filter=f;
}
