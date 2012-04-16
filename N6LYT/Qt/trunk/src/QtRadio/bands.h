#ifndef BANDS_H
#define BANDS_H

#include <QObject>
#include <QAction>
#include <QMenu>

#include "Band.h"

class Bands : public QObject
{
    Q_OBJECT
public:

    Bands();
    void loadSettings(QSettings* settings);
    void saveSettings(QSettings* settings);
    void setupBands(long min,long max);
    void selectBand(int id);
    void setFrequency(quint64 f);
    void setSpectrumLow(int value);
    void setSpectrumHigh(int value);
    void setWaterfallLow(int value);
    void setWaterfallHigh(int value);
    void setMode(int value);
    void setFilter(int value);

    bool exists(int id);

    QString getLabel();
    QString getLabel(int id);
    quint64 getFrequency();
    qint32 getBand();
    int getBandStackEntry();
    int getMode();
    int getFilter();
    int getSpectrumLow();
    int getSpectrumHigh();
    int getWaterfallLow();
    int getWaterfallHigh();

    int getCurrentBand();

    BandLimit* getBandLimits(quint64 low,quint64 high);
    void buildMenu(QMenu* hamMenu,QMenu* broadcastMenu);

signals:
    void bandSelected(int band);
    
public slots:
    void actionBandSelected();
    
private:
    QList<Band*> allBands;
    QList<Band*> bands;
    int currentBand;
    QAction* currentAction;
};

#endif // BANDS_H
