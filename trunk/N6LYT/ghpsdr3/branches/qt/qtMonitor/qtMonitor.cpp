/*
 * File:   qtMonitor.cpp
 * Author: john
 *
 * Created on 05 August 2010, 12:20
 */

#include "qtMonitor.h"

#define BUFFER_SIZE 480
#define HEADER_SIZE 48



qtMonitor::qtMonitor() {

    sem.release(1);
    
    widget.setupUi(this);
    connect(widget.band_160_pushButton, SIGNAL(pressed()),
            this, SLOT(band_160_buttonPressed()));
    connect(widget.band_80_pushButton, SIGNAL(pressed()),
            this, SLOT(band_80_buttonPressed()));
    connect(widget.band_60_pushButton, SIGNAL(pressed()),
            this, SLOT(band_60_buttonPressed()));
    connect(widget.band_40_pushButton, SIGNAL(pressed()),
            this, SLOT(band_40_buttonPressed()));
    connect(widget.band_30_pushButton, SIGNAL(pressed()),
            this, SLOT(band_30_buttonPressed()));
    connect(widget.band_20_pushButton, SIGNAL(pressed()),
            this, SLOT(band_20_buttonPressed()));
    connect(widget.band_17_pushButton, SIGNAL(pressed()),
            this, SLOT(band_17_buttonPressed()));
    connect(widget.band_15_pushButton, SIGNAL(pressed()),
            this, SLOT(band_15_buttonPressed()));
    connect(widget.band_12_pushButton, SIGNAL(pressed()),
            this, SLOT(band_12_buttonPressed()));
    connect(widget.band_10_pushButton, SIGNAL(pressed()),
            this, SLOT(band_10_buttonPressed()));
    connect(widget.band_6_pushButton, SIGNAL(pressed()),
            this, SLOT(band_6_buttonPressed()));
    connect(widget.band_gen_pushButton, SIGNAL(pressed()),
            this, SLOT(band_gen_buttonPressed()));

    connect(widget.mode_lsb_pushButton, SIGNAL(pressed()),
            this, SLOT(mode_lsb_buttonPressed()));
    connect(widget.mode_usb_pushButton, SIGNAL(pressed()),
            this, SLOT(mode_usb_buttonPressed()));
    connect(widget.mode_dsb_pushButton, SIGNAL(pressed()),
            this, SLOT(mode_dsb_buttonPressed()));
    connect(widget.mode_cwl_pushButton, SIGNAL(pressed()),
            this, SLOT(mode_cwl_buttonPressed()));
    connect(widget.mode_cwu_pushButton, SIGNAL(pressed()),
            this, SLOT(mode_cwu_buttonPressed()));
    connect(widget.mode_am_pushButton, SIGNAL(pressed()),
            this, SLOT(mode_am_buttonPressed()));

    connect(widget.connect_pushButton, SIGNAL(pressed()),
            this, SLOT(connect_buttonPressed()));

    widget.spectrumFrame->setVisible(TRUE);

    audio_device.initialize_audio();

}

qtMonitor::~qtMonitor() {
}

void qtMonitor::band_160_buttonPressed() {
    widget.vfo_lcdNumber->display(1.845);
    sendCommand("setFrequency 1845000");
    sendCommand("setFilter -3440 -150");
    sendCommand("setMode 0");
}

void qtMonitor::band_80_buttonPressed() {
    widget.vfo_lcdNumber->display(3.75);
    sendCommand("setFrequency 3750000");
    sendCommand("setFilter -3440 -150");
    sendCommand("setMode 0");
}

void qtMonitor::band_60_buttonPressed() {
    widget.vfo_lcdNumber->display(5.3305);
    sendCommand("setFrequency 5330500");
    sendCommand("setFilter -3440 -150");
    sendCommand("setMode 0");
}

void qtMonitor::band_40_buttonPressed() {
    widget.vfo_lcdNumber->display(7.056);
    sendCommand("setFrequency 7056000");
    sendCommand("setFilter -3440 -150");
    sendCommand("setMode 0");
}

void qtMonitor::band_30_buttonPressed() {
    widget.vfo_lcdNumber->display(10.120);
    sendCommand("setFrequency 10120000");
    sendCommand("setFilter 150 3440");
    sendCommand("setMode 1");
}

void qtMonitor::band_20_buttonPressed() {
    widget.vfo_lcdNumber->display(14.230);
    sendCommand("setFrequency 14230000");
    sendCommand("setFilter 150 3440");
    sendCommand("setMode 1");
}

void qtMonitor::band_17_buttonPressed() {
    widget.vfo_lcdNumber->display(18.0686);
    sendCommand("setFrequency 18068600");
    sendCommand("setFilter 150 3440");
    sendCommand("setMode 1");
}

void qtMonitor::band_15_buttonPressed() {
    widget.vfo_lcdNumber->display(21.255);
    sendCommand("setFrequency 21255000");
    sendCommand("setFilter 150 3440");
    sendCommand("setMode 1");
}

void qtMonitor::band_12_buttonPressed() {
    widget.vfo_lcdNumber->display(24.895);
    sendCommand("setFrequency 24895000");
    sendCommand("setFilter 150 3440");
    sendCommand("setMode 1");
}

void qtMonitor::band_10_buttonPressed() {
    widget.vfo_lcdNumber->display(28.500);
    sendCommand("setFrequency 28500000");
    sendCommand("setFilter 150 3440");
    sendCommand("setMode 1");
}

void qtMonitor::band_6_buttonPressed() {
    widget.vfo_lcdNumber->display(50.125);
    sendCommand("setFrequency 50125000");
    sendCommand("setFilter 150 3440");
    sendCommand("setMode 1");
}

void qtMonitor::band_gen_buttonPressed() {
    widget.vfo_lcdNumber->display(0.909);
    sendCommand("setFrequency 909000");
    sendCommand("setFilter -4000 4000");
    sendCommand("setMode 6");
}

void qtMonitor::mode_lsb_buttonPressed() {
    sendCommand("setMode 0");
    sendCommand("setFilter -3440 -150");
}

void qtMonitor::mode_usb_buttonPressed() {
    sendCommand("setMode 1");
    sendCommand("setFilter 150 3440");
}

void qtMonitor::mode_dsb_buttonPressed() {
    sendCommand("setMode 2");
    sendCommand("setFilter -3440 3440");
}

void qtMonitor::mode_cwl_buttonPressed() {
    sendCommand("setMode 3");
    sendCommand("setFilter  -800 -400");
}

void qtMonitor::mode_cwu_buttonPressed() {
    sendCommand("setMode 4");
    sendCommand("setFilter 400 800");
}

void qtMonitor::mode_am_buttonPressed() {
    sendCommand("setMode 6");
    sendCommand("setFilter -4000 4000");
}

void qtMonitor::socketError(QAbstractSocket::SocketError socketError) {
    switch (socketError) {
        case QAbstractSocket::RemoteHostClosedError:
            qDebug() << "Remote closed connection";
            break;
        case QAbstractSocket::HostNotFoundError:
            qDebug() << "Host not found";
            break;
        case QAbstractSocket::ConnectionRefusedError:
            qDebug() << "Remote host refused connection";
            break;
        default:
            qDebug() << "Socket Error: " << tcpSocket->errorString();
    }

}

void qtMonitor::connected() {

    qDebug() << "Connected";

    sendCommand("startAudioStream 480");

    QTimer* qTimer=new QTimer(this);
    connect(qTimer, SIGNAL(timeout()), this, SLOT(update()));
    qTimer->start(1000/15); /* 15 fps */

}

void qtMonitor::connect_buttonPressed() {
    qDebug() << "connect_buttonPressed";

    tcpSocket=new QTcpSocket(this);

    connect(tcpSocket, SIGNAL(error(QAbstractSocket::SocketError)),
            this, SLOT(socketError(QAbstractSocket::SocketError)));

    connect(tcpSocket, SIGNAL(connected()),
            this, SLOT(connected()));

    connect(tcpSocket, SIGNAL(readyRead()),
            this, SLOT(socketData()));

    tcpSocket->connectToHost(widget.server_lineEdit->text(),widget.rx_spinBox->value()+8000);
    
}

void qtMonitor::sendCommand(char* command) {
    char buffer[32];

    sem.acquire(1);
    //qDebug() << "sendCommand:" << command;
    strcpy(buffer,command);
    //qDebug() << "sendCommand: buffer:" << buffer;
    tcpSocket->write(buffer,32);
    tcpSocket->flush();
    sem.release(1);
}

void qtMonitor::update() {
    sendCommand("getSpectrum 480");
}

void qtMonitor::socketData() {
    char buffer[HEADER_SIZE+BUFFER_SIZE];

    //if(tcpSocket->bytesAvailable()>=HEADER_SIZE+BUFFER_SIZE) {
        tcpSocket->read(buffer,HEADER_SIZE+BUFFER_SIZE);
        
        if(buffer[0]==0) {
            // spectrum data
            widget.spectrumFrame->updateSpectrum(buffer);
        } else if(buffer[0]==1) {
            // audio data
        } else {
            qDebug() << "unknown data: " << buffer[0];
        }
    //}

}