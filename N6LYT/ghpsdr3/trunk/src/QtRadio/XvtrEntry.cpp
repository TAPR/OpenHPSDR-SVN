#include "XvtrEntry.h"

XvtrEntry::XvtrEntry() {
}

XvtrEntry::XvtrEntry(QString t,long long minF,long long maxF,long long ifF) {
    title=t;
    minFrequency=minF;
    maxFrequency=maxF;
    ifFrequency=ifF;
}

QString XvtrEntry::getTitle() {
    return title;
}

long long XvtrEntry::getMinFrequency() {
    return minFrequency;
}

long long XvtrEntry::getMaxFrequency() {
    return maxFrequency;
}

long long XvtrEntry::getIFFrequency() {
    return ifFrequency;
}
