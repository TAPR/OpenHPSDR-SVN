/*
 * File:   Spectrum.cpp
 * Author: john
 *
 * Created on 16 August 2010, 10:03
 */


#include "Spectrum.h"

Spectrum::Spectrum() {
}

Spectrum::Spectrum(QWidget*& widget) {
    QFrame::setParent(widget);

    qDebug() << "Spectrum::Spectrum " << width() << ":" << height();

    sampleRate=96000;
    spectrumHigh=-40;
    spectrumLow=-160;
    filterLow=-3450;
    filterHigh=-150;
    mode="LSB";

    subRxFrequency=0LL;
    subRx=FALSE;

    samples=NULL;
    
    receiver=0;
    
    plot.clear();
}

Spectrum::~Spectrum() {
}

void Spectrum::setHigh(int high) {
    spectrumHigh=high;
    repaint();
}

void Spectrum::setLow(int low) {
    spectrumLow=low;
    repaint();
}

int Spectrum::getHigh() {
    return spectrumHigh;
}

int Spectrum::getLow() {
    return spectrumLow;
}


void Spectrum::initialize() {
    QFrame::setVisible(true);
}

int Spectrum::samplerate() {
    return sampleRate;
}

void Spectrum::setObjectName(QString name) {
    QFrame::setObjectName(name);
}

void Spectrum::setGeometry(QRect rect) {
    QFrame::setGeometry(rect);

    qDebug() << "Spectrum:setGeometry: width=" << rect.width() << " height=" << rect.height();

    samples=(float*)malloc(rect.width()*sizeof(float));
}

void Spectrum::mousePressEvent(QMouseEvent* event) {

    //qDebug() << "mousePressEvent " << event->pos().x();

    startX=lastX=event->pos().x();
    moved=0;
}

void Spectrum::mouseMoveEvent(QMouseEvent* event){
    int move=event->pos().x()-lastX;
    lastX=event->pos().x();
    //qDebug() << "mouseMoveEvent " << event->pos().x() << " move:" << move;

    emit frequencyMoved(move);

}

void Spectrum::mouseReleaseEvent(QMouseEvent* event) {
    int move=event->pos().x()-lastX;
    lastX=event->pos().x();
    //qDebug() << "mouseReleaseEvent " << event->pos().x() << " move:" << move;

    if(moved) {
        emit frequencyMoved(move);
    } else {

    }
}

void Spectrum::wheelEvent(QWheelEvent *event) {
    emit frequencyMoved(event->delta()/8/15);
}

void Spectrum::paintEvent(QPaintEvent*) {
    QPainter painter(this);
    int filterLeft;
    int filterRight;


    QLinearGradient gradient(0, 0, 0,height());
    gradient.setColorAt(0, Qt::black);
    gradient.setColorAt(1, Qt::gray);
    painter.setBrush(gradient);
    painter.drawRect(0, 0, width(), height());

    // draw sub rx filter
    if(subRx) {
        filterLeft = (filterLow - (-sampleRate / 2) + (subRxFrequency-frequency)) * width() / sampleRate;
        filterRight = (filterHigh - (-sampleRate / 2) + (subRxFrequency-frequency)) * width() / sampleRate;
        painter.setBrush(Qt::SolidPattern);
        painter.setOpacity(0.5);
        painter.fillRect(filterLeft, 0, filterRight - filterLeft, height(), Qt::lightGray);
    }

    // draw filter
    filterLeft = (filterLow - (-sampleRate / 2)) * width() / sampleRate;
    filterRight = (filterHigh - (-sampleRate / 2)) * width() / sampleRate;
    painter.setBrush(Qt::SolidPattern);
    painter.setOpacity(0.5);
    painter.fillRect(filterLeft,0,filterRight-filterLeft,height(),Qt::gray);

    painter.setOpacity(1.0);

    // plot horizontal grid
    int V = spectrumHigh - spectrumLow;
    int numSteps = V / 20;
    for (int i = 1; i < numSteps; i++) {
        int num = spectrumHigh - i * 20;
        int y = (int) floor((spectrumHigh - num) * height() / V);

        painter.setPen(QPen(Qt::white, 1,Qt::DotLine));
        painter.drawLine(0, y, width(), y);

        painter.setPen(QPen(Qt::green, 1));
        painter.setFont(QFont("Arial", 10));
        painter.drawText(3,y,QString::number(num)+" dBm");

    }

    // draw cursor
    painter.setPen(QPen(Qt::red, 1));
    painter.drawLine(width()/2,0,width()/2,height());

    // show the frequency
    painter.setPen(QPen(Qt::green,1));
    painter.setFont(QFont("Arial", 30));
    painter.drawText(width()/2,30,QString::number(frequency));

    // show the band and mode and filter
    painter.setFont(QFont("Arial", 12));
    QString text=band+" "+mode+" "+filter;
    painter.drawText((width()/2)+200,30,text);
    text=host+" Rx"+QString::number(receiver);
    painter.drawText(5,15,text);

    // show the subrx frequency
    if(subRx) {
        filterLeft = (filterLow - (-sampleRate / 2) + (subRxFrequency-frequency)) * width() / sampleRate;
        filterRight = (filterHigh - (-sampleRate / 2) + (subRxFrequency-frequency)) * width() / sampleRate;
        painter.setPen(QPen(Qt::green,1));
        painter.setFont(QFont("Arial", 12));
        painter.drawText(filterRight,height()-20,QString::number(subRxFrequency));
    }

    // plot Spectrum
    painter.setPen(QPen(Qt::yellow, 1));
    if(plot.count()==width()) {
        painter.drawPolyline(plot.constData(),plot.count());
    }
}

void Spectrum::setFrequency(long long f) {
    frequency=f;
    subRxFrequency=f;

    qDebug() << "Spectrum:setFrequency: " << f;
}

void Spectrum::setSubRxFrequency(long long f) {
    subRxFrequency=f;
    qDebug() << "Spectrum:setSubRxFrequency: " << f;
}

void Spectrum::setSubRxState(bool state) {
    subRx=state;
}

void Spectrum::setFilter(int low, int high) {
    filterLow=low;
    filterHigh=high;
}

void Spectrum::setHost(QString h) {
    host=h;
    repaint();
}

void Spectrum::setReceiver(int r) {
    receiver=r;
    repaint();
}

void Spectrum::setMode(QString m) {
    mode=m;
    repaint();
}

void Spectrum::setBand(QString b) {
    band=b;
    repaint();
}

void Spectrum::setFilter(QString f) {
    filter=f;
    repaint();
}

void Spectrum::updateSpectrum(char* header,char* buffer,int width) {
    int i;


    //qDebug() << "updateSpectrum: width=" << width() << " height=" << height();
    
    sampleRate = atoi(&header[32]);

    //qDebug() << "updateSpectrum: samplerate=" << sampleRate;

    if(samples!=NULL) {
        free(samples);
    }
    samples = (float*) malloc(width * sizeof (float));

    for(i=0;i<width;i++) {
        samples[i] = -(buffer[i] & 0xFF);
    }

    //qDebug() << "updateSpectrum: create plot points";
    plot.clear();
    for (i = 0; i < width; i++) {

        plot << QPoint(i, (int) floor(((float) spectrumHigh - samples[i])*(float) height() / (float) (spectrumHigh - spectrumLow)));
    }

    //qDebug() << "updateSpectrum: repaint";
    this->repaint();
}


