/* 
 * File:   Filter.h
 * Author: john
 *
 * Created on 14 August 2010, 10:08
 */

#ifndef FILTER_H
#define	FILTER_H

#include <QObject>
#include <QSettings>
#include <QDebug>

class Filter : QObject {
    Q_OBJECT
public:
    Filter();
    Filter(QString t,int l,int h);
    virtual ~Filter();
    void init(QString t,int l,int h);
    void setText(QString t);
    void setLow(int l);
    void setHigh(int h);
    QString getText();
    int getLow();
    int getHigh();
private:
    QString text;
    int low;
    int high;
};

#endif	/* FILTER_H */

