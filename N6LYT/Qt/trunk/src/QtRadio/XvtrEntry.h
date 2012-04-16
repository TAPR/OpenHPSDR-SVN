#ifndef XVTRENTRY_H
#define XVTRENTRY_H

#include <QString>

class XvtrEntry
{
public:
    XvtrEntry();
    XvtrEntry(int e,QString t,quint64 minF,quint64 maxF,quint64 ifF,quint64 freq,int m,int filt);
    int getEntry();
    QString getTitle();
    quint64 getMinFrequency();
    quint64 getMaxFrequency();
    quint64 getIFFrequency();
    quint64 getFrequency();
    int getMode();
    int getFilter();
    void setFrequency(quint64 f);
    void setMode(int m);
    void setFilter(int f);

private:
    int entry;
    QString title;
    quint64 minFrequency;
    quint64 maxFrequency;
    quint64 ifFrequency;

    quint64 frequency;
    int mode;
    int filter;
};

#endif // XVTRENTRY_H
