#ifndef FREQUENCY_H
#define FREQUENCY_H

#include "FrequencyInfo.h"
#include <Band.h>

class Frequency
{
public:
    Frequency();
    FrequencyInfo getFrequencyInfo(long long frequency);
private:
    QVector <FrequencyInfo> info;
};

#endif // FREQUENCY_H
