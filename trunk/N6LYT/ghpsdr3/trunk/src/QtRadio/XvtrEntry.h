#ifndef XVTRENTRY_H
#define XVTRENTRY_H

#include <QString>

class XvtrEntry
{
public:
    XvtrEntry();
    XvtrEntry(QString t,long long minF,long long maxF,long long ifF);
    QString getTitle();
    long long getMinFrequency();
    long long getMaxFrequency();
    long long getIFFrequency();
private:
    QString title;
    long long minFrequency;
    long long maxFrequency;
    long long ifFrequency;
};

#endif // XVTRENTRY_H
