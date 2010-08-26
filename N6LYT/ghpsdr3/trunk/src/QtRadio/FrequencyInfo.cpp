#include "FrequencyInfo.h"

FrequencyInfo::FrequencyInfo() {
}

FrequencyInfo::FrequencyInfo(long long min,long long max,QString descr,int b,bool t) {
    minFrequency=min;
    maxFrequency=max;
    description=descr;
    band=b;
    transmit=t;
}

QString FrequencyInfo::getDescription() {
    return description;
}

int FrequencyInfo::getBand() {
    return band;
}

bool FrequencyInfo::canTransmit() {
    return transmit;
}

bool FrequencyInfo::isFrequency(long long frequency) {
    return frequency>=minFrequency && frequency<=maxFrequency;
}
