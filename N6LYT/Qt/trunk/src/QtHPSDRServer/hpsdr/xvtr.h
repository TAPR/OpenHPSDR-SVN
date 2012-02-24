#ifndef XVTR_H
#define XVTR_H

#include <QObject>

#include "alex.h"

class XVTR : public QObject
{
    Q_OBJECT
public:
    explicit XVTR(QObject *parent = 0);

    void setLabel(QString l);
    void setMinFrequency(qint64 f);
    void setMaxFrequency(qint64 f);
    void setLOFrequency(qint64 f);
    void setRx(int ant);
    void setTx(int ant);


    QString getLabel();
    qint64 getMinFrequency();
    qint64 getMaxFrequency();
    qint64 getLOFrequency();
    int getRx();
    int getTx();
    QString getRxAntenna();
    QString getTxAntenna();

signals:
    
public slots:
    
private:
    QString label;
    qint64 minFrequency;
    qint64 maxFrequency;
    qint64 LOFrequency;
    int rxAnt;
    int txAnt;
};

#endif // XVTR_H
