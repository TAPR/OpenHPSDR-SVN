/*
 * File:   Spectrum.cpp
 * Author: John Melton, G0ORX/N6LYT
 *
 * Created on 16 August 2010, 10:03
 */

/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/

#include "Spectrum.h"

Spectrum::Spectrum() {
}

Spectrum::Spectrum(QWidget*& widget) {
    QFrame::setParent(widget);

    //qDebug() << "Spectrum::Spectrum " << width() << ":" << height();

    sMeterMain=new Meter("Main Rx");
    sMeterSub=new Meter("Sub Rx");
    sampleRate=96000;
    step=100;
    spectrumHigh=-40;
    spectrumLow=-160;
    filterLow=-3450;
    filterHigh=-150;
    mode="LSB";

    subRxFrequency=0LL;
    subRx=FALSE;

    band_min=0LL;
    band_max=0LL;

    samples=NULL;
    
    receiver=0;

    meter=-121;
    maxMeter=-121;
    meterCount=0;
    
    subrx_meter=-121;
    subrx_maxMeter=-121;
    subrx_meterCount=0;

    button=-1;
    showSquelchControl=false;
    settingSquelch=false;

    mox=false;

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

void Spectrum::setSampleRate(int r) {
    sampleRate=r;
    step=sampleRate*100/96000;

    qDebug()<<"Spectrum::setSampleRate:"<<sampleRate<<","<<step;
}

int Spectrum::samplerate() {
    return sampleRate;
}

void Spectrum::setBandLimits(qint64 min,qint64 max) {

    qDebug() << "Spectrum::setBandLimits: " << min << "," << max;
    band_min=min;
    band_max=max;
}

void Spectrum::setObjectName(QString name) {
    QFrame::setObjectName(name);
}

void Spectrum::setGeometry(QRect rect) {
    QFrame::setGeometry(rect);

    //qDebug() << "Spectrum:setGeometry: width=" << rect.width() << " height=" << rect.height();

    samples=(float*)malloc(rect.width()*sizeof(float));
}

void Spectrum::mousePressEvent(QMouseEvent* event) {

    //qDebug() << __FUNCTION__ << ": " << event->pos().x();

    //qDebug() << "mousePressEvent: event->button(): " << event->button();

    button=event->button();
    startX=lastX=event->pos().x();
    moved=0;

    if(squelch) {
        if((event->pos().y()>=squelchY-1)&&(event->pos().y()<=squelchY+1)) {
            settingSquelch=true;
        } else {
            settingSquelch=false;
        }
    } else {
        if((event->pos().x()>width()-45) && (event->pos().y()<40)) {
            // mox button pressed
        }
    }
}

void Spectrum::mouseMoveEvent(QMouseEvent* event){
    int move=event->pos().x()-lastX;
    lastX=event->pos().x();

    moved=1;

    if(button==-1) {
        if(squelch && ((event->pos().y()>=squelchY-1)&&(event->pos().y()<=squelchY+1))) {
            showSquelchControl=true;
            this->setCursor(Qt::SizeVerCursor);
        } else {
            showSquelchControl=false;
            this->setCursor(Qt::ArrowCursor);
        }
    } else {
        if(settingSquelch) {
            int delta=squelchY-event->pos().y();
            delta=int((float)delta*((float)(spectrumHigh-spectrumLow)/(float)height()));
            emit squelchValueChanged(delta);
        } else {
            if (!move==0) emit frequencyMoved(move,step);
        }
    }
}

void Spectrum::mouseReleaseEvent(QMouseEvent* event) {
    int move=event->pos().x()-lastX;
    lastX=event->pos().x();

    if(squelch && settingSquelch) {
        button=-1;
        //convert squelchY to value
        settingSquelch=false;

    } else {
        if(moved) {
            emit frequencyMoved(move,step);
            button=-1;
        } else {
            float hzPixel = sampleRate/width();  // spectrum resolution: Hz/pixel

            qint64 freqOffsetPixel;
            qint64 f = frequency - (sampleRate/2) + (event->pos().x()*hzPixel);
            if(subRx) {    
                freqOffsetPixel = (subRxFrequency-f)/hzPixel;
                if (button == Qt::LeftButton) {
                    // set frequency to center of filter
                    if(filterLow<0 && filterHigh<0) {
                        freqOffsetPixel+=(((filterLow-filterHigh)/2)+filterHigh)/hzPixel;
                    } else if(filterLow>0 && filterHigh>0){
                        freqOffsetPixel-=(((filterHigh-filterLow)/2)-filterHigh)/hzPixel;
                    } else {
                        // no adjustment
                    }
                }
            } else {
                freqOffsetPixel = (f-frequency)/hzPixel; // compute the offset from the central frequency, in pixel
                if (button == Qt::LeftButton) {
                    // set frequency to center of filter
                    if(filterLow<0 && filterHigh<0) {
                        freqOffsetPixel-=(((filterLow-filterHigh)/2)+filterHigh)/hzPixel;
                    } else if(filterLow>0 && filterHigh>0){
                        freqOffsetPixel+=(((filterHigh-filterLow)/2)-filterHigh)/hzPixel;
                    } else {
                        // no adjustment
                    }
                }
            }

            qDebug()<<"Spectrum::mouseReleaseEvent: hzPixel:"<<hzPixel;
            qDebug()<<"Spectrum::mouseReleaseEvent: sampleRate:"<<sampleRate;
            qDebug()<<"Spectrum::mouseReleaseEvent: lastX:"<<lastX;
            qDebug()<<"Spectrum::mouseReleaseEvent: f:"<<f;
            qDebug()<<"Spectrum::mouseReleaseEvent: frequency:"<<frequency;
            qDebug()<<"Spectrum::mouseReleaseEvent: freqOffsetPixel:"<<freqOffsetPixel;
            qDebug()<<"Spectrum::mouseReleaseEvent: "<<(-(qint64)(freqOffsetPixel*hzPixel)/step);
            emit frequencyMoved(-(qint64)(freqOffsetPixel*hzPixel)/step,step);
            button=-1;
        }
    }
}

void Spectrum::wheelEvent(QWheelEvent *event) {
    //qDebug() << __FUNCTION__ << "Delta: " << event->delta() << "y: " << event->pos().y() << " heigth:" << height();

    if(event->pos().x() < 50) {
        // wheel event on the left side, change the vertical axis values
        float shift =  (float)(event->delta()/8/15 * 5)                    // phy steps of wheel * 5
                     * ((float)(spectrumHigh - spectrumLow) / height());   // dBm / pixel on vertical axis

        if (event->buttons() == Qt::MidButton) {
           // change the vertical axis range
           //qDebug() << __FUNCTION__ << " change vertical axis scale: " << shift;
           emit spectrumHighChanged (spectrumHigh+(int)shift);
           emit spectrumLowChanged  (spectrumLow-(int)shift);
           emit waterfallHighChanged (spectrumHigh+(int)shift);
           emit waterfallLowChanged  (spectrumLow-(int)shift);

        } else {
          // if middle mouse button pressed shift the spectrum scale
          //qDebug() << __FUNCTION__ << " shift on vertical axis scale: " << shift;
          emit spectrumHighChanged (spectrumHigh+(int)shift);
          emit spectrumLowChanged  (spectrumLow+(int)shift);
          emit waterfallHighChanged (spectrumHigh+(int)shift);
          emit waterfallLowChanged  (spectrumLow+(int)shift);

       }
    } else if((event->pos().x()>(width()-50)) & squelch) {
        // wheel event on right side, change squelch
        int delta=event->delta()/8/15;
        //fprintf(stderr,"squelchValueChanged %d\n",delta);
        emit squelchValueChanged(delta);
    } else {
        // wheel event
        // change frequency
        float v= (float)event->pos().y() / (float)height();
        //qDebug() << "wheelEvent v: " << v;

        if (v > 0.75) {
            emit frequencyMoved(event->delta()/8/15,step);
        } else if (v > 0.50) {
            emit frequencyMoved(event->delta()/8/15,50);
        } else if (v > 0.25) {
            emit frequencyMoved(event->delta()/8/15,25);
        } else {
            emit frequencyMoved(event->delta()/8/15,10);
        }
    }
}

void Spectrum::paintEvent(QPaintEvent*) {
    QPainter painter(this);
    int filterLeft;
    int filterRight;
    QString text;

    QLinearGradient gradient(0, 0, 0,height());
    gradient.setColorAt(0, Qt::black);
    gradient.setColorAt(1, Qt::gray);
    painter.setBrush(gradient);
    painter.drawRect(0, 0, width(), height());

    if(sampleRate==0) {
        qDebug() << "sampleRate is 0";
        return;
    }

    // draw filter
    filterLeft = (filterLow - (-sampleRate / 2)) * width() / sampleRate;
    filterRight = (filterHigh - (-sampleRate / 2)) * width() / sampleRate;
    painter.setBrush(Qt::SolidPattern);
    painter.setOpacity(0.5);
    painter.fillRect(filterLeft,0,filterRight-filterLeft,height(),Qt::gray);

    // draw sub rx filter and cursor
    if(subRx) {
        int cursor=(subRxFrequency-(frequency-(sampleRate/2))) * width() / sampleRate;
        filterLeft = (filterLow - (-sampleRate / 2) + (subRxFrequency-frequency)) * width() / sampleRate;
        filterRight = (filterHigh - (-sampleRate / 2) + (subRxFrequency-frequency)) * width() / sampleRate;
        painter.setBrush(Qt::SolidPattern);
        painter.setOpacity(0.5);
        painter.fillRect(filterLeft, 0, filterRight - filterLeft, height(), Qt::lightGray);

        painter.setPen(QPen(Qt::red, 1));
        painter.drawLine(cursor,0,cursor,height());
    }

    // plot horizontal dBm lines
    int V = spectrumHigh - spectrumLow;
    int numSteps = V / 20;
    for (int i = 1; i < numSteps; i++) {
        int num = spectrumHigh - i * 20;
        int y = (int) floor((spectrumHigh - num) * height() / V);

        painter.setOpacity(0.5);
        painter.setPen(QPen(Qt::white, 1,Qt::DotLine));
        painter.drawLine(0, y, width(), y);

        painter.setOpacity(1.0);
        painter.setPen(QPen(Qt::green, 1));
        painter.setFont(QFont("Arial", 10));
        painter.drawText(3,y,QString::number(num)+" dBm");
    }
    
    // plot the vertical frequency lines
    float hzPerPixel=(float)sampleRate/(float)width();
    qint64 f=frequency-(sampleRate/2);

    for(int i=0;i<width();i++) {
        f=frequency-(sampleRate/2)+(qint64)(hzPerPixel*(float)i);
        if(f>0) {
            if((f%10000)<(qint64)hzPerPixel) {
                painter.setOpacity(0.5);
                painter.setPen(QPen(Qt::white, 1,Qt::DotLine));
                painter.drawLine(i, 0, i, height());

                painter.setOpacity(1.0);
                painter.setPen(QPen(Qt::black, 1));
                painter.setFont(QFont("Arial", 10));
                text.sprintf("%lld.%02lld",f/1000000,f%1000000/10000);
                painter.drawText(i,height(),text);
            }
        }
    }

    // draw the band limits
    qint64 min_display=frequency-(sampleRate/2);
    qint64 max_display=frequency+(sampleRate/2);
    if(band_min!=0LL && band_max!=0LL) {
        int i;
        painter.setPen(QPen(Qt::red, 2));
        if((min_display<band_min)&&(max_display>band_min)) {
            i=(band_min-min_display)/(qint64)hzPerPixel;
            painter.drawLine(i,0,i,height());
        }
        if((min_display<band_max)&&(max_display>band_max)) {
            i=(band_max-min_display)/(qint64)hzPerPixel;
            painter.drawLine(i+1,0,i+1,height());
        }
    }

    // draw cursor
    painter.setPen(QPen(Qt::red, 1));
    painter.drawLine(width()/2,0,width()/2,height());

    // draw the squelch
    if(settingSquelch || showSquelchControl) {
        squelchY=(int) floor(((float) spectrumHigh - squelchVal)*(float) height() / (float) (spectrumHigh - spectrumLow));
        painter.setPen(QPen(Qt::red, 1,Qt::DashLine));
        painter.drawLine(0,squelchY,width(),squelchY);
        painter.setFont(QFont("Arial", 10));
        text.sprintf("%s","Squelch");
        painter.drawText(width()-48,squelchY,text);
    } else if(squelch) {
        squelchY=(int) floor(((float) spectrumHigh - squelchVal)*(float) height() / (float) (spectrumHigh - spectrumLow));
        painter.setPen(QPen(Qt::red, 1));
        painter.drawLine(0,squelchY,width(),squelchY);
        painter.setFont(QFont("Arial", 10));
        text.sprintf("%s","Squelch");
        painter.drawText(width()-48,squelchY,text);

        //if(showSquelchControl) {
        //    painter.setPen(QPen(Qt::red, 1));
        //    painter.drawLine(width()-50,squelchY-10,width()-50,squelchY+10);
        //}
    }

    // show the frequency
    if(mox) {
        painter.setPen(QPen(Qt::red,1));
    } else {
        painter.setPen(QPen(Qt::green,1));
    }
    painter.setFont(QFont("Verdana", 30));
    painter.drawText(width()/2,30,strFrequency);

    // show the band and mode and filter
    painter.setPen(QPen(Qt::green,1));
    painter.setFont(QFont("Arial", 12));
    text=band+" "+mode+" "+filter+"Hz";
    painter.drawText((width()/2),50,text);

    // show the server and receiver
    text="Server:"+host+" Rx:"+QString::number(receiver);
    painter.drawText(5,15,text);

    // draw the analog meters
    painter.setOpacity(0.8);
    QImage image=sMeterMain->getImage(meter);
    painter.drawImage(width()-image.width()-45,0,image);

    if(subRx) {
        image=sMeterSub->getImage(subrx_meter);
        painter.drawImage(width()-image.width()-45,image.height()+5,image);
    }

    // plot Spectrum
    painter.setOpacity(0.9);
    painter.setPen(QPen(Qt::yellow, 1));
    if(plot.count()==width()) {
        painter.drawPolyline(plot.constData(),plot.count());
    }

    // show the subrx frequency
    if(subRx) {
        /*
        filterRight = (filterHigh - (-sampleRate / 2) + (subRxFrequency-frequency)) * width() / sampleRate;
        painter.setPen(QPen(Qt::black,1));
        painter.setFont(QFont("Arial", 12));
        painter.drawText(filterRight,height()-20,strSubRxFrequency);
        */
        // show the frequency
        painter.setPen(QPen(Qt::green,1));
        painter.setFont(QFont("Verdana", 30));
        painter.drawText(width()/2,image.height()+5+30,strSubRxFrequency);
    }
}

void Spectrum::setFrequency(qint64 f) {
    frequency=f;
    subRxFrequency=f;
    strFrequency.sprintf("%lld.%03lld.%03lld",f/1000000,f%1000000/1000,f%1000);
    strSubRxFrequency.sprintf("%lld.%03lld.%03lld",f/1000000,f%1000000/1000,f%1000);
}

void Spectrum::setSubRxFrequency(qint64 f) {
    subRxFrequency=f;
    strSubRxFrequency.sprintf("%lld.%03lld.%03lld",f/1000000,f%1000000/1000,f%1000);
}

void Spectrum::setSubRxState(bool state) {
    subRx=state;
}

void Spectrum::setFilter(int low, int high) {
    qDebug() << "Spectrum::setFilter " << low << "," << high;
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
    qDebug() << "Spectrum:setMode: " << m;
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

    meter=((header[5]&0xFF)<<8)+(header[6]&0xFF);
    subrx_meter=((header[7]&0xFF)<<8)+(header[8]&0xFF);
    sampleRate=((header[9]&0xFF)<<24)+((header[10]&0xFF)<<16)+((header[11]&0xFF)<<8)+(header[12]&0xFF);

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


void Spectrum::setSquelch(bool state) {
    squelch=state;
    QFrame::setMouseTracking(state);
}

void Spectrum::setSquelchVal(float val) {
    squelchVal=val;
    squelchY=(int) floor(((float) spectrumHigh - squelchVal)*(float) height() / (float) (spectrumHigh - spectrumLow));
    //qDebug()<<"Spectrum::setSquelchVal"<<val<<"squelchY="<<squelchY;
}

void Spectrum::setMox(bool state) {
    mox=state;
}
