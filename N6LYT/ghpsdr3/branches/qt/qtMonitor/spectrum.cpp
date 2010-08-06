/* 
 * File:   spectrum.cpp
 * Author: john
 * 
 * Created on 05 August 2010, 16:12
 */

#include "spectrum.h"

spectrum::spectrum() {
}

spectrum::spectrum(QWidget*& widget) {
    QFrame::setParent(widget);
    spectrumHigh=-40;
    spectrumLow=-160;
}

spectrum::~spectrum() {
}


void spectrum::setVisible(bool visible) {
    QFrame::setVisible(visible);
}

void spectrum::setObjectName(QString name) {
    QFrame::setObjectName(name);
}

void spectrum::setGeometry(QRect rect) {
    QFrame::setGeometry(rect);
}

void spectrum::paintEvent(QPaintEvent*) {
    QPainter painter(this);

    // plot horizontal grid
    int V = spectrumHigh - spectrumLow;
    int numSteps = V / 20;
    int pixelStepSize = 180 / numSteps;
    for (int i = 1; i < numSteps; i++) {
        int num = spectrumHigh - i * 20;
        int y = (int) floor((spectrumHigh - num) * 180 / V);

        painter.setPen(QPen(Qt::yellow, 1));
        painter.drawLine(0, y, 480, y);

        painter.setPen(QPen(Qt::green, 1));
        painter.drawText(3,y+2,QString::number(num));

    }

    // draw cursor
    painter.setPen(QPen(Qt::red, 1));
    painter.drawLine(240,0,240,180);

    // plot spectrum
    painter.setPen(QPen(Qt::black, 1));
    painter.drawPolyline(plot.constData(),480);

}

void spectrum::updateSpectrum(char* buffer) {
    int i;

    for(i=0;i<480;i++) {
        samples[i] = -(buffer[i + 48] & 0xFF) - 30;
    }

    for (i = 0; i < 480; i++) {
        plot[i].setX(i);
        plot[i].setY((int) floor(((float) spectrumHigh - samples[i])*(float) 180 / (float) (spectrumHigh - spectrumLow)));
    }

    this->repaint();
}