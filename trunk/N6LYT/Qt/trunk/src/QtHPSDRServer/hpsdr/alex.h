#ifndef ALEX_H
#define ALEX_H

#include <QSettings>
#include <QObject>

#include "../Common/Bands.h"

#define ANT1 0
#define ANT2 1
#define ANT3 2
#define RX1  3
#define RX2  4
#define XVRX 5

class Alex : public QObject
{
    Q_OBJECT

public:
    static Alex* getInstance();
    void configure(QSettings* mysettings);
    void set(int band,int rxant,int txant);
    void setRx(int band,int rxant);
    void setTx(int band,int txant);
    int getRx(int band);
    int getTx(int band);
    
    QString getRxAntenna(int band);
    QString getTxAntenna(int band);
    QString getBand(int band);

protected:
    Alex(QObject *parent = 0);

signals:
    
public slots:
    void save();

private:
    static Alex* instance;

    QSettings* settings;

    int rx[HAM_BAND_LAST+1];
    int tx[HAM_BAND_LAST+1];
};

#endif // ALEX_H
