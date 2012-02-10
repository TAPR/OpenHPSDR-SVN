#ifndef XVTR_H
#define XVTR_H

#include <QObject>

class XVTR : public QObject
{
    Q_OBJECT
public:
    explicit XVTR(QObject *parent = 0);

    void setLabel(QString l);
    void setMinFrequency(qint64 f);
    void setMaxFrequency(qint64 f);
    void setLOFrequency(qint64 f);

    QString getLabel();
    qint64 getMinFrequency();
    qint64 getMaxFrequency();
    qint64 getLOFrequency();
    
signals:
    
public slots:
    
private:
    QString label;
    qint64 minFrequency;
    qint64 maxFrequency;
    qint64 LOFrequency;
};

#endif // XVTR_H
