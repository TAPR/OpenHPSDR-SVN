/*
 * File:   Spectrum.h
 * Author: john
 *
 * Created on 16 August 2010, 10:03
 */

#ifndef SPECTRUM_H
#define	SPECTRUM_H

#include <QtCore>

#include <QFrame>
#include <QPainter>
#include <QMouseEvent>

class Spectrum: public QFrame {
    Q_OBJECT
public:
    Spectrum();
    Spectrum(QWidget*& widget);
    virtual ~Spectrum();
    void setObjectName(QString name);
    void setGeometry(QRect rect);
    void initialize();
    void setFrequency(long long f);
    void setSubRxFrequency(long long f);
    void setFilter(int low,int high);
    void updateSpectrum(char* header,char* buffer,int width);
    int samplerate();

    int getHigh();
    int getLow();
    void setHigh(int high);
    void setLow(int low);

    void setSubRxState(bool state);

    void setMode(QString m);
    void setBand(QString b);
    void setFilter(QString f);

    void setHost(QString h);
    void setReceiver(int r);

signals:
    void frequencyMoved(int step);

protected:
    void paintEvent(QPaintEvent*);

    void mousePressEvent(QMouseEvent* event);
    void mouseMoveEvent(QMouseEvent* event);
    void mouseReleaseEvent(QMouseEvent* event);

    void wheelEvent(QWheelEvent *event);

private:
    float* samples;
    int spectrumHigh;
    int spectrumLow;

    QString host;
    int receiver;
    QString band;
    QString mode;
    QString filter;

    int startX;
    int lastX;
    int moved;

    int sampleRate;
    int meter;
    int maxMeter;
    int meterCount;
    int subrx_meter;
    int subrx_maxMeter;
    int subrx_meterCount;

    int filterLow;
    int filterHigh;
    long long frequency;
    long long subRxFrequency;
    bool subRx;

    QVector <QPoint> plot;
};


#endif	/* SPECTRUM_H */

