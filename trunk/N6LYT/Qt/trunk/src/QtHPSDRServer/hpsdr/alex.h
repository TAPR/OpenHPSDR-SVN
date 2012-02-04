#ifndef ALEX_H
#define ALEX_H

#include <QSettings>
#include <QObject>

#define BAND_160 0
#define BAND_80 1
#define BAND_60 2
#define BAND_40 3
#define BAND_30 4
#define BAND_20 5
#define BAND_17 6
#define BAND_15 7
#define BAND_12 8
#define BAND_10 9
#define BAND_6 10
#define BAND_GEN 11

#define BANDS 12


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
    
protected:
    Alex(QObject *parent = 0);

signals:
    
public slots:
    void save();

private:
    static Alex* instance;

    QSettings* settings;

    int rx[BANDS];
    int tx[BANDS];
};

#endif // ALEX_H
