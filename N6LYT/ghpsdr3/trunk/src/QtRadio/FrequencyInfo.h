#ifndef FREQUENCYINFO_H
#define FREQUENCYINFO_H

#include <QObject>
#include <QString>

class FrequencyInfo {
public:
    FrequencyInfo();
    FrequencyInfo(long long min,long long max,QString descr,int b,bool t);
    QString getDescription();
    bool isFrequency(long long frequency);
    int getBand();
    bool canTransmit();
private:
    long long minFrequency;
    long long maxFrequency;
    QString description;
    int band;
    bool transmit;
};

#endif // FREQUENCYINFO_H
