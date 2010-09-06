#ifndef METER_H
#define METER_H

#include <QDebug>
#include <QImage>
#include <QPainter>

class Meter {
public:
    Meter(QString title);
    void calculateLine(int dbm,double minRadius,double maxRadius);
    int getX();
    int getY();
    QImage getImage(int dbm);

private:
    QImage* image;
    int x,y;
    int dxmin,dymin,dxmax,dymax;

};

#endif // METER_H
