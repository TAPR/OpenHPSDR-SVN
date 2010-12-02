/*
 * File:   MainWindow.cpp
 * Author: John Melton, G0ORX/N6LYT
 *
 * Created on 23 November 2010
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

#include "MainWindow.h"
#include "ui_MainWindow.h"

#include "QDebug"
#include "QFile"
#include "QFileDialog"


MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    int i;
    int nInterfaces;

    ui->setupUi(this);

    for(i=0,nInterfaces=0;i<interfaces.getInterfaces();i++) {
        hwAddress=interfaces.getInterfaceHardwareAddress(i);
        if((hwAddress==NULL) || (hwAddress=="00:00:00:00:00:00")) {
            // interface has no harwdare address so ignore it
        } else {
            ui->interfaceComboBox->addItem(interfaces.getInterfaceNameAt(i));
            nInterfaces++;
        }
    }

    if(nInterfaces>0) {
        ui->interfaceComboBox->setCurrentIndex(0);
        interfaceSelected(0);
    }

    QObject::connect(ui->interfaceComboBox,SIGNAL(currentIndexChanged(int)),this,SLOT(interfaceSelected(int)));
    QObject::connect(ui->browsePushButton,SIGNAL(clicked()),this,SLOT(browse()));
    QObject::connect(ui->programPushButton,SIGNAL(clicked()),this,SLOT(program()));
    QObject::connect(ui->erasePushButton,SIGNAL(clicked()),this,SLOT(erase()));
    QObject::connect(ui->readMACPushButton,SIGNAL(clicked()),this,SLOT(getMAC()));
    QObject::connect(ui->readIPPushButton,SIGNAL(clicked()),this,SLOT(getIP()));
    QObject::connect(ui->writeIPPushButton,SIGNAL(clicked()),this,SLOT(setIP()));
}

MainWindow::~MainWindow() {
    delete ui;
}

// SLOTS
void MainWindow::interfaceSelected(int index) {
    bool ok;
    ip=interfaces.getInterfaceIPAddress(index);
    hwAddress=interfaces.getInterfaceHardwareAddress(index);
    if(hwAddress==NULL) {
        ui->interfaceLabel->setText("");
        status("Inteface is not a valid network device");
    } else {
        QString text;
        text.sprintf("MAC=%s  IP=%ld.%ld.%ld.%ld",
                     hwAddress.toAscii().constData(),
                     (ip>>24)&0xFF,(ip>>16)&0xFF,(ip>>8)&0xFF,ip&0xFF);
        ui->interfaceLabel->setText(text);

        hw[0]=(unsigned char)hwAddress.mid(0,2).toInt(&ok,16);
        hw[1]=(unsigned char)hwAddress.mid(3,2).toInt(&ok,16);
        hw[2]=(unsigned char)hwAddress.mid(6,2).toInt(&ok,16);
        hw[3]=(unsigned char)hwAddress.mid(9,2).toInt(&ok,16);
        hw[4]=(unsigned char)hwAddress.mid(12,2).toInt(&ok,16);
        hw[5]=(unsigned char)hwAddress.mid(15,2).toInt(&ok,16);
    }
}

void MainWindow::browse() {
    QString fileName=QFileDialog::getOpenFileName(this,tr("Select File"),"",tr("pof Files (*.pof)"));
    ui->fileLineEdit->setText(fileName);
}

void MainWindow::program() {
    char errbuf[PCAP_ERRBUF_SIZE];
    int length;
    int ffCount;
    int i;

    ui->statusListWidget->clear();
    ui->statusListWidget->addItem("");
    percent=0;

    // check that an interface has been selected
    if(ui->interfaceComboBox->currentIndex()!=-1) {
        // check that a pof file has been selected
        if(ui->fileLineEdit->text().endsWith(".pof")) {

            // load the pof file and convert to rpd
            QFile pofFile(ui->fileLineEdit->text());
            pofFile.open(QIODevice::ReadOnly);
            QDataStream in(&pofFile);
            length=pofFile.size();
            data=(char*)malloc(length);

            status("reading file...");
            if(in.readRawData(data,length)!=length) {
                status("Error: could not read pof file");
            } else {
                pofFile.close();
                status("file read successfully");

                // trim
                start=0x95;
                if(length>0x200095) {
                    length=0x200095;
                }
                ffCount=0;
                for(i=length-1;i>=start;i--) {
                    if(data[i]==(char)0xFF) {
                        ffCount++;
                    } else {
                        break;
                    }
                }

                end=length-ffCount+36; // trim the FF's but keep at least 36
                end+=(256-((end-start)%256)); // must be multiple of 256 bytes

                qDebug() <<"start="<<start<<" end="<<end;

                handle=pcap_open_live(ui->interfaceComboBox->currentText().toAscii().constData(),1024,1,TIMEOUT,errbuf);
                if (handle == NULL) {
                    qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toAscii().constData()<<errbuf;
                    status("Error: cannot open interface (are you running as root)");
                } else {

                    rawReceiveThread=new RawReceiveThread(hw,handle);
                    rawReceiveThread->start();
                    QObject::connect(rawReceiveThread,SIGNAL(commandCompleted()),this,SLOT(commandCompleted()));
                    QObject::connect(rawReceiveThread,SIGNAL(nextBuffer()),this,SLOT(nextBuffer()));
                    QObject::connect(rawReceiveThread,SIGNAL(timeout()),this,SLOT(timeout()));

                    // start by erasing
                    state=ERASING;
                    eraseData();
                }

            }

        } else {
            status("Error: no file selected");
        }
    } else {
        status("Error: no interface selected");
    }
}

void MainWindow::erase() {
    char errbuf[PCAP_ERRBUF_SIZE];

    //qDebug()<<"erase";

    ui->statusListWidget->clear();
    ui->statusListWidget->addItem("");
    percent=0;

    // check that an interface has been selected
    if(ui->interfaceComboBox->currentIndex()!=-1) {

        handle=pcap_open_live(ui->interfaceComboBox->currentText().toAscii().constData(),1024,1,TIMEOUT,errbuf);
        if (handle == NULL) {
            qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toAscii().constData()<<errbuf;
            status("Error: cannot open interface (are you running as root)");
        } else {

            rawReceiveThread=new RawReceiveThread(hw,handle);
            rawReceiveThread->start();
            QObject::connect(rawReceiveThread,SIGNAL(commandCompleted()),this,SLOT(commandCompleted()));
            QObject::connect(rawReceiveThread,SIGNAL(timeout()),this,SLOT(timeout()));

            // start by erasing
            state=ERASING_ONLY;
            eraseData();
        }
    } else {
        status("Error: no interface selected");
    }
}

void MainWindow::getMAC() {
    char errbuf[PCAP_ERRBUF_SIZE];

    ui->statusListWidget->clear();
    ui->statusListWidget->addItem("");
    percent=0;

    // check that an interface has been selected
    if(ui->interfaceComboBox->currentIndex()!=-1) {
        //hw=interfaces.getInterfaceHardwareAddress(ui->interfaceComboBox->currentText());
        //ip=interfaces.getInterfaceIPAddress(ui->interfaceComboBox->currentText());

        handle=pcap_open_live(ui->interfaceComboBox->currentText().toAscii().constData(),1024,1,TIMEOUT,errbuf);
        if (handle == NULL) {
            qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toAscii().constData()<<errbuf;
            status("Error: cannot open interface (are you running as root)");
        } else {

            rawReceiveThread=new RawReceiveThread(hw,handle);
            rawReceiveThread->start();
            QObject::connect(rawReceiveThread,SIGNAL(macAddress(unsigned char*)),this,SLOT(macAddress(unsigned char*)));
            QObject::connect(rawReceiveThread,SIGNAL(timeout()),this,SLOT(timeout()));

            // start by erasing
            state=READ_MAC;
            readMAC();
        }
    } else {
        status("Error: no interface selected");
    }

}

void MainWindow::macAddress(unsigned char* mac) {
    QString text;
    text.sprintf("%02X:%02X:%02X:%02X:%02X:%02X",mac[0],mac[1],mac[2],mac[3],mac[4],mac[5]);
    ui->macLineEdit->setText(text);
    status("Metis MAC address read successfully");
    idle();
}

void MainWindow::getIP() {
    char errbuf[PCAP_ERRBUF_SIZE];

    //qDebug()<<"getIP";

    ui->statusListWidget->clear();
    ui->statusListWidget->addItem("");
    percent=0;

    // check that an interface has been selected
    if(ui->interfaceComboBox->currentIndex()!=-1) {

        handle=pcap_open_live(ui->interfaceComboBox->currentText().toAscii().constData(),1024,1,TIMEOUT,errbuf);
        if (handle == NULL) {
            qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toAscii().constData()<<errbuf;
            status("Error: cannot open interface (are you running as root)");
        } else {

            rawReceiveThread=new RawReceiveThread(hw,handle);
            rawReceiveThread->start();
            QObject::connect(rawReceiveThread,SIGNAL(ipAddress(unsigned char*)),this,SLOT(ipAddress(unsigned char*)));
            QObject::connect(rawReceiveThread,SIGNAL(timeout()),this,SLOT(timeout()));

            // start by erasing
            state=READ_IP;
            readIP();
        }
    } else {
        status("Error: no interface selected");
    }
}

void MainWindow::ipAddress(unsigned char* ip) {
    QString text;
    text.sprintf("%d",ip[0]);
    ui->ipALineEdit->setText(text);
    text.sprintf("%d",ip[1]);
    ui->ipBLineEdit->setText(text);
    text.sprintf("%d",ip[2]);
    ui->ipCLineEdit->setText(text);
    text.sprintf("%d",ip[3]);
    ui->ipDLineEdit->setText(text);
    status("Metis IP address read successfully");
    idle();
}

void MainWindow::setIP() {
    char errbuf[PCAP_ERRBUF_SIZE];
    unsigned char buffer[66];
    int addr[4];
    int i;

    ui->statusListWidget->clear();
    ui->statusListWidget->addItem("");

    addr[0]=ui->ipALineEdit->text().toInt();
    addr[1]=ui->ipBLineEdit->text().toInt();
    addr[2]=ui->ipCLineEdit->text().toInt();
    addr[3]=ui->ipDLineEdit->text().toInt();
    if((addr[0]<0 || addr[0]>255) || (addr[1]<0 || addr[1]>255) || (addr[2]<0 || addr[2]>255) || (addr[3]<0 || addr[3]>255)) {
        status("Error: invalid IP address");
    } else {

        handle=pcap_open_live(ui->interfaceComboBox->currentText().toAscii().constData(),1024,1,TIMEOUT,errbuf);
        if (handle == NULL) {
            qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toAscii().constData()<<errbuf;
            status("Error: cannot open interface (are you running as root)");
        } else {

            state=WRITE_IP;

            /*set the frame header*/
            buffer[0]=0x11; // dest address
            buffer[1]=0x22;
            buffer[2]=0x33;
            buffer[3]=0x44;
            buffer[4]=0x55;
            buffer[5]=0x66;

            buffer[6]=hw[0]; // src address
            buffer[7]=hw[1];
            buffer[8]=hw[2];
            buffer[9]=hw[3];
            buffer[10]=hw[4];
            buffer[11]=hw[5];

            buffer[12]=0xEF; // protocol
            buffer[13]=0xFE;

            buffer[14]=0x03; //
            buffer[15]=0x05; // write ip address

            /*fill the frame with 0x00*/
            buffer[16]=(unsigned char)addr[0];
            buffer[17]=(unsigned char)addr[1];
            buffer[18]=(unsigned char)addr[2];
            buffer[19]=(unsigned char)addr[3];

            for(i=0;i<46;i++) {
                buffer[i+20]=(unsigned char)0x00;
            }

            int bytes=0;
            bytes=pcap_inject(handle,buffer,66);
            if(bytes!=66) {
                qDebug() << "pcap_inject returned:" << bytes;
                status("send write ip command failed");
                idle();
            } else {
                status("Written Metis IP address");
            }
        }

        idle();
    }
}

void MainWindow::eraseData() {
    int i;
    unsigned char buffer[62];

    //qDebug()<<"eraseData";

    eraseTimeouts=0;

    if(handle!=NULL) {

        /*set the frame header*/
        buffer[0]=0x11; // dest address
        buffer[1]=0x22;
        buffer[2]=0x33;
        buffer[3]=0x44;
        buffer[4]=0x55;
        buffer[5]=0x66;

        buffer[6]=hw[0]; // src address
        buffer[7]=hw[1];
        buffer[8]=hw[2];
        buffer[9]=hw[3];
        buffer[10]=hw[4];
        buffer[11]=hw[5];

        buffer[12]=0xEF; // protocol
        buffer[13]=0xFE;

        buffer[14]=0x03; //
        buffer[15]=0x02; // erase

        /*fill the frame with 0x00*/
        for(i=0;i<46;i++) {
                buffer[i+16]=(unsigned char)0x00;
        }

        int bytes=0;
        bytes=pcap_inject(handle,buffer,62);
        if(bytes!=62) {
            qDebug() << "pcap_inject returned:" << bytes;
            status("send erase command failed");
            idle();
        } else {
            status("Erasing device ... (takes several seconds)");
        }
    }
}

void MainWindow::readMAC() {
    int i;
    unsigned char buffer[62];

    eraseTimeouts=0;
    if(handle!=NULL) {

        /*set the frame header*/
        buffer[0]=0x11; // dest address
        buffer[1]=0x22;
        buffer[2]=0x33;
        buffer[3]=0x44;
        buffer[4]=0x55;
        buffer[5]=0x66;

        buffer[6]=hw[0]; // src address
        buffer[7]=hw[1];
        buffer[8]=hw[2];
        buffer[9]=hw[3];
        buffer[10]=hw[4];
        buffer[11]=hw[5];

        buffer[12]=0xEF; // protocol
        buffer[13]=0xFE;

        buffer[14]=0x03; //
        buffer[15]=0x03; // read MAC

        /*fill the frame with 0x00*/
        for(i=0;i<46;i++) {
                buffer[i+16]=(unsigned char)0x00;
        }

        int bytes=0;
        bytes=pcap_inject(handle,buffer,62);
        if(bytes!=62) {
            qDebug() << "pcap_inject returned:" << bytes;
            status("send erase command failed");
            idle();
        } else {
            status("Reading Metis MAC Address ...");
        }
    }
}

void MainWindow::readIP() {
    int i;
    unsigned char buffer[62];

    //qDebug()<<"readIP";

    eraseTimeouts=0;
    if(handle!=NULL) {

        /*set the frame header*/
        buffer[0]=0x11; // dest address
        buffer[1]=0x22;
        buffer[2]=0x33;
        buffer[3]=0x44;
        buffer[4]=0x55;
        buffer[5]=0x66;

        buffer[6]=hw[0]; // src address
        buffer[7]=hw[1];
        buffer[8]=hw[2];
        buffer[9]=hw[3];
        buffer[10]=hw[4];
        buffer[11]=hw[5];

        buffer[12]=0xEF; // protocol
        buffer[13]=0xFE;

        buffer[14]=0x03; //
        buffer[15]=0x04; // read IP

        /*fill the frame with 0x00*/
        for(i=0;i<46;i++) {
                buffer[i+16]=(unsigned char)0x00;
        }

        if(handle!=NULL) {
            qDebug() << "pcap_inject";
            int bytes=0;
            bytes=pcap_inject(handle,buffer,62);
            if(bytes!=62) {
                qDebug() << "pcap_inject returned:" << bytes;
                status("send erase command failed");
                idle();
            } else {
                status("Reading Metis IP address ...");
            }
        }
    }
}

void MainWindow::sendData() {
    unsigned char buffer[272];
    int i;

    //qDebug()<<"sendData offset="<<offset<<"start="<<start<<"end="<<end;

    if(handle!=NULL) {
        /*set the frame header*/
        buffer[0]=0x11; // dest address
        buffer[1]=0x22;
        buffer[2]=0x33;
        buffer[3]=0x44;
        buffer[4]=0x55;
        buffer[5]=0x66;

        buffer[6]=hw[0]; // src address
        buffer[7]=hw[1];
        buffer[8]=hw[2];
        buffer[9]=hw[3];
        buffer[10]=hw[4];
        buffer[11]=hw[5];

        buffer[12]=0xEF; // protocol
        buffer[13]=0xFE;

        buffer[14]=0x03;
        buffer[15]=0x01;

        /*fill the frame with some data*/
        for(i=0;i<256;i++) {
                buffer[i+16]=(unsigned char)data[i+offset];
        }

        int bytes=0;
        bytes=pcap_inject(handle,buffer,272);
        if(bytes!=(272)) {
            qDebug() << "pcap_inject returned:" << bytes;
            status("send data command failed");
            idle();
        } else {
            QString text;
            int p=offset*100/(end-start);
            if(p!=percent) {
                if((p%20)==0) {
                    percent=p;
                    text.sprintf("Programming device %d%% written ...",percent);
                    status(text);
                }
            }
        }


    }
}

void MainWindow::commandCompleted() {
    switch(state) {
    case IDLE:
        // ignore
        break;
    case ERASING:
        status("Device erased successfully");
        state=PROGRAMMING;
        offset=start;
        status("Programming device ...");
        sendData();
        break;
    case ERASING_ONLY:
        status("Device erased successfully");
        pcap_close(handle);
        state=IDLE;
        break;
    case READ_MAC:
        break;
    case READ_IP:
        break;
    case WRITE_IP:
        status("Metis IP written successfully");
        pcap_close(handle);
        state=IDLE;
        break;
    }
}

void MainWindow::nextBuffer() {
    offset+=256;
    if(offset<end) {
        sendData();
    } else {
        status("Programming device completed successfully");
        status("Remember to remove JP1 when you power cycle");
        idle();
    }
}

void MainWindow::timeout() {
    //qDebug()<<"timeout";
    switch(state) {
    case IDLE:
        // ignore
        break;
    case ERASING:
    case ERASING_ONLY:
        eraseTimeouts++;
        if(eraseTimeouts==MAX_ERASE_TIMEOUTS) {
            status("Error: erase timeout - have you set the jumper at JP1 and power cycled?");
            idle();
        }
        break;
    case PROGRAMMING:
        //qDebug()<<"timeout";
        break;
    case READ_MAC:
        break;
    case READ_IP:
        break;
    case WRITE_IP:
        break;
    }
}

void MainWindow::idle() {
    //qDebug()<<"idle";
    if(rawReceiveThread!=NULL) {
        rawReceiveThread->stop();
        if(handle!=NULL) {
            pcap_close(handle);
        }
    }
    state=IDLE;
}

void MainWindow::status(QString text) {
    qDebug()<<"status:"<<text;
    ui->statusListWidget->insertItem(ui->statusListWidget->count()-1,text);
    ui->statusListWidget->setCurrentRow(ui->statusListWidget->count()-1);
}

