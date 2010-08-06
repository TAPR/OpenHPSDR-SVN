/* 
 * File:   spectrum.h
 * Author: john
 *
 * Created on 05 August 2010, 16:12
 */

#ifndef SPECTRUM_H
#define	SPECTRUM_H

#include <QtCore>

#include <QFrame>
#include <QPainter>


class spectrum: public QFrame {
    Q_OBJECT
public:
    spectrum();
    spectrum(QWidget*& widget);
    virtual ~spectrum();
    void updateSpectrum(char* buffer);
    void setObjectName(QString name);
    void setGeometry(QRect rect);
    void setVisible(bool visible);
    
protected:
    void paintEvent(QPaintEvent*);

private:
    float samples[480];
    int   X[480];
    int   Y[480];
    int spectrumHigh;
    int spectrumLow;
};

static QVector <QPoint> plot(480);


#endif	/* SPECTRUM_H */

