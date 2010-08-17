/* 
 * File:   Waterfall.h
 * Author: john
 *
 * Created on 16 August 2010, 10:35
 */

#ifndef WATERFALL_H
#define	WATERFALL_H

#include <QtCore>

#include <QFrame>
#include <QPainter>
#include <QMouseEvent>

class Waterfall: public QFrame {
    Q_OBJECT
public:
    Waterfall();
    Waterfall(QWidget*& widget);
    virtual ~Waterfall();
    void setObjectName(QString name);
    void setGeometry(QRect rect);
    void initialize();
    void setFrequency(long long f);
    void setFilter(int low,int high);
    void updateWaterfall(char* header,char* buffer,int width);

    void setLow(int low);
    void setHigh(int high);
    int getLow();
    int getHigh();

signals:
    void frequencyMoved(int step);

protected:
    void paintEvent(QPaintEvent*);

    void mousePressEvent(QMouseEvent* event);
    void mouseMoveEvent(QMouseEvent* event);
    void mouseReleaseEvent(QMouseEvent* event);

    void wheelEvent(QWheelEvent *event);

private:
    uint calculatePixel(int sample);

    float* samples;
    int waterfallHigh;
    int waterfallLow;

    int startX;
    int lastX;
    int moved;

    int colorLowR;
    int colorLowG;
    int colorLowB;
    int colorMidR;
    int colorMidG;
    int colorMidB;
    int colorHighR;
    int colorHighG;
    int colorHighB;
    QImage image;
};


#endif	/* WATERFALL_H */


