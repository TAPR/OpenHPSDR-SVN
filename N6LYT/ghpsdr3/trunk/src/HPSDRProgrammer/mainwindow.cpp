/*
 * File:   mainwindow.cpp
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

#include "mainwindow.h"
#include "ui_mainwindow.h"

#include "QDebug"
#include "QFile"
#include "QFileDialog"
#include "QRect"
#include "QTimer"

#include "pcap.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    int i;
    int nInterfaces;

    ui->setupUi(this);

    receiveThread=NULL;
    rawReceiveThread=NULL;
    discoveryThread=NULL;

#ifdef __WIN32
    ui->privilegesLabel->setText("You must be running with Administrator privileges to be able to read/write raw ethernet frames.");
    QRect rect=ui->interfaceComboBox->geometry();
    rect.setWidth(ui->interfaceLabel->width());
    ui->interfaceComboBox->setGeometry(rect);
#else
    ui->privilegesLabel->setText("You must be running as root to be able to read/write raw ethernet frames.");
#endif

    for (i = 0; i < interfaces.getInterfaces(); ++i)
    {   ui->interfaceComboBox->addItem(interfaces.getInterfaceNameAt(i));
        ++nInterfaces;
    }

    connect(ui->interfaceComboBox,SIGNAL(currentIndexChanged(int)),this,SLOT(interfaceSelected(int)));
    connect(ui->browsePushButton,SIGNAL(clicked()),this,SLOT(browse()));
    connect(ui->programPushButton,SIGNAL(clicked()),this,SLOT(program()));
    connect(ui->erasePushButton,SIGNAL(clicked()),this,SLOT(erase()));
    connect(ui->readMACPushButton,SIGNAL(clicked()),this,SLOT(getMAC()));
    connect(ui->readIPPushButton,SIGNAL(clicked()),this,SLOT(getIP()));
    connect(ui->writeIPPushButton,SIGNAL(clicked()),this,SLOT(setIP()));

    connect(ui->discoverPushButton,SIGNAL(clicked()),this,SLOT(discover()));
    connect(ui->metisComboBox,SIGNAL(currentIndexChanged(int)),this,SLOT(metisSelected(int)));

    connect(ui->tabWidget,SIGNAL(currentChanged(int)),this,SLOT(tabChanged(int)));

    if(ui->interfaceComboBox->count()>0) {
        ui->interfaceComboBox->setCurrentIndex(0);
        interfaceSelected(0);
    }

    bootloader=false;

    ui->tabWidget_2->setTabEnabled(2,false);
    ui->tabWidget_2->setTabEnabled(3,false);

}

MainWindow::~MainWindow() {
    delete ui;
}

// SLOT - interfaceSelected - called when the interface selection is changed
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

// SLOT - browse - called when the "Browse ..." button on the Program tab is pressed.
void MainWindow::browse() {
    QString fileName=QFileDialog::getOpenFileName(this,tr("Select File"),"",tr("pof Files (*.pof)"));
    ui->fileLineEdit->setText(fileName);
}

// SLOT - program - called when the "Program" button on the Program tab is pressed.
void MainWindow::program() {
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
                blocks=(end-start)/256;

                qDebug() <<"start="<<start<<" end="<<end<<" blocks="<<blocks;

                if(bootloader) {
                    bootloaderProgram();
                } else {
                    flashProgram();
                }

            }
        } else {
            status("Error: no file selected");
        }
    } else {
        status("Error: no interface selected");
    }
}

void MainWindow::bootloaderProgram() {
    char errbuf[PCAP_ERRBUF_SIZE];

    qDebug()<<"MainWindow::bootloaderProgram";
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

void MainWindow::flashProgram() {

    // start a thread to listen for replies
    receiveThread=new ReceiveThread(metisIP);
    receiveThread->setIPAddress(ip);
    receiveThread->start();

    QObject::connect(receiveThread,SIGNAL(commandCompleted()),this,SLOT(commandCompleted()));
    QObject::connect(receiveThread,SIGNAL(nextBuffer()),this,SLOT(nextBuffer()));

    state=ERASING;
    eraseData();

}

// SLOT - erase - called when the "Erase" button on the Erase taqb is pressed
void MainWindow::erase() {

    qDebug()<<"erase";

    ui->statusListWidget->clear();
    ui->statusListWidget->addItem("");
    percent=0;

    // check that an interface has been selected
    if(ui->interfaceComboBox->currentIndex()!=-1) {
        if(bootloader) {
            bootloaderErase();
        } else {
            flashErase();
        }
    } else {
        status("Error: no interface selected");
    }
}

void MainWindow::bootloaderErase() {
    char errbuf[PCAP_ERRBUF_SIZE];

    handle=pcap_open_live(ui->interfaceComboBox->currentText().toAscii().constData(),1024,1,TIMEOUT,errbuf);
    if (handle == NULL) {
        qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toAscii().constData()<<errbuf;
        status("Error: cannot open interface (are you running as root)");
    } else {
        rawReceiveThread=new RawReceiveThread(hw,handle);
        QObject::connect(rawReceiveThread,SIGNAL(commandCompleted()),this,SLOT(commandCompleted()));
        QObject::connect(rawReceiveThread,SIGNAL(timeout()),this,SLOT(timeout()));
        rawReceiveThread->start();

        // start by erasing
        state=ERASING_ONLY;
        eraseData();
    }
}

void MainWindow::flashErase() {
    receiveThread=new ReceiveThread(metisIP);
    receiveThread->setIPAddress(ip);
    QObject::connect(receiveThread,SIGNAL(commandCompleted()),this,SLOT(commandCompleted()));
    QObject::connect(receiveThread,SIGNAL(timeout()),this,SLOT(timeout()));
    receiveThread->start();

    // start erasing
    state=ERASING_ONLY;
    eraseData();
}

// SLOT - getMac - called when the "Read" button on the MAC Address tab is pressed.
void MainWindow::getMAC() {
    char errbuf[PCAP_ERRBUF_SIZE];

    qDebug()<<"getMAC";
    ui->statusListWidget->clear();
    ui->statusListWidget->addItem("");
    percent=0;

    // check that an interface has been selected
    if(ui->interfaceComboBox->currentIndex()!=-1) {
        //hw=interfaces.getInterfaceHardwareAddress(ui->interfaceComboBox->currentText());
        //ip=interfaces.getInterfaceIPAddress(ui->interfaceComboBox->currentText());
//    QString x = ui->interfaceComboBox->currentText().toAscii().constData();
    QString x = ui->interfaceComboBox->currentText();
        handle=pcap_open_live(x.toAscii(),1024,1,TIMEOUT,errbuf);
        if (handle == NULL) {
            qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toAscii().constData()<<errbuf;
            status("Error: cannot open interface (are you running as root)");
        } else {

            rawReceiveThread=new RawReceiveThread(hw,handle);
            rawReceiveThread->start();
            QObject::connect(rawReceiveThread,SIGNAL(macAddress(unsigned char*)),this,SLOT(macAddress(unsigned char*)));
            QObject::connect(rawReceiveThread,SIGNAL(timeout()),this,SLOT(timeout()));


            state=READ_MAC;
            readMAC();
        }
    } else {
        status("Error: no interface selected");
    }

}

// SLOT - macAddress - called when the reply packet is received containg the Metis MAC address.
void MainWindow::macAddress(unsigned char* mac) {
    QString text;
    text.sprintf("%02X:%02X:%02X:%02X:%02X:%02X",mac[0],mac[1],mac[2],mac[3],mac[4],mac[5]);
    ui->macLineEdit->setText(text);
    status("Metis MAC address read successfully");
    idle();
}

// SLOT - getIP - called when the "Read" button on the IP Address tab is pressed.
void MainWindow::getIP() {
    char errbuf[PCAP_ERRBUF_SIZE];

    qDebug()<<"getIP";

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

            state=READ_IP;
            readIP();
        }
    } else {
        status("Error: no interface selected");
    }
}

// SLOT - ipAddress - called when the reply packet is received containing Metis IP address.
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

// SLOT - setIP - called when the "Write" button on the IP Address tab is pressed.
void MainWindow::setIP() {
    char errbuf[PCAP_ERRBUF_SIZE];
    unsigned char buffer[66];
    int addr[4];
    int i;

    qDebug()<<"setIP";
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

           if(pcap_sendpacket(handle,buffer,62)!=0) {
                qDebug()<<"pcap_sendpacket failed";
                status("send write ip command failed");
                idle();
            } else {
                status("Written Metis IP address");
            }
        }

        idle();
    }
}

// private function to send the command to erase
void MainWindow::eraseData() {
    eraseTimeouts=0;
    status("Erasing device ... (takes several seconds)");
    if(bootloader) {
        sendRawCommand(0x02);
    } else {
        sendCommand(0x02);
    }
}

// private function to send command to read MAC address from Metis
void MainWindow::readMAC() {
    eraseTimeouts=0;
    status("Reading Metis MAC Address ...");
    sendRawCommand(0x03);
}

// private function to read the IP address from Metis.
void MainWindow::readIP() {
    eraseTimeouts=0;
    status("Reading Metis IP address ...");
    sendRawCommand(0x04);
}

// private function to send a command.
void MainWindow::sendCommand(unsigned char command) {
    unsigned char buffer[64];
    int i;

    //qDebug()<<"sendCommand "<<command;

    buffer[0]=0xEF; // protocol
    buffer[1]=0xFE;

    buffer[2]=0x03;
    buffer[3]=command;

    /*fill the frame with 0x00*/
    for(i=0;i<60;i++) {
        buffer[i+4]=(unsigned char)0x00;
    }

    //
    /*
    int s;
    */
    int rc;
    int on=1;



    s=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(s<0) {
        perror("create socket failed for send_command\n");
        exit(1);
    }

    setsockopt(s, SOL_SOCKET, SO_REUSEADDR, (const char*)&on, sizeof(on));

    // bind to the selected interface
    struct sockaddr_in name={0,0,{0},{0}};
    name.sin_family = AF_INET;
    name.sin_addr.s_addr = htonl(ip);
    name.sin_port = htons(1024);
    rc=bind(s,(struct sockaddr*)&name,sizeof(name));
    if(rc != 0) {
        fprintf(stderr,"cannot bind to interface: rc=%d\n", rc);
        exit(1);
    }

    struct sockaddr_in addr={0,0,{0},{0}};
    addr.sin_family = AF_INET;
    addr.sin_addr.s_addr = metisIP;
    addr.sin_port = htons(1024);

    if(sendto(s,(char*)buffer,sizeof(buffer),0,(struct sockaddr*)&addr,sizeof(addr))<0) {
        perror("sendto socket failed for sendCommand\n");
        exit(1);
    }

}

// private function to send a command.
void MainWindow::sendRawCommand(unsigned char command) {
    unsigned char buffer[62];
    int i;

    //qDebug()<<"sendCommand "<<command;

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
        buffer[15]=command;

        /*fill the frame with 0x00*/
        for(i=0;i<46;i++) {
                buffer[i+16]=(unsigned char)0x00;
        }

        if(handle!=NULL) {
            //qDebug() << "pcap_sendpacket";
            if(pcap_sendpacket(handle,buffer,62)!=0) {
                qDebug()<<"pcap_sendpacket failed";
                status("send command failed");
                idle();
            }
        }
    }
}

// private function to send 256 byte block of the pof file.
void MainWindow::sendData() {
    unsigned char buffer[264];

    qDebug()<<"sendData offset="<<offset;

    buffer[0]=0xEF;
    buffer[1]=0xFE;
    buffer[2]=0x03;
    buffer[3]=0x01;
    buffer[4]=(blocks>>24)&0xFF;
    buffer[5]=(blocks>>16)&0xFF;
    buffer[6]=(blocks>>8)&0xFF;
    buffer[7]=blocks&0xFF;

    /*fill the frame with some data*/
    for(int i=0;i<256;i++) {
        buffer[i+8]=(unsigned char)data[i+offset];
    }

    //
    struct sockaddr_in addr;
    int length;

    length=sizeof(addr);
    memset(&addr,0,length);
    addr.sin_family=AF_INET;
    addr.sin_port=htons(1024);
    addr.sin_addr.s_addr=metisIP;

    qDebug("sendto");
    if(sendto(s,(char*)&buffer[0],sizeof(buffer),0,(struct sockaddr*)&addr,length)<0) {
        perror("sendto socket failed for sendCommand\n");
        exit(1);
    }

    QString text;
    int p=(offset+256)*100/(end-start);
    if(p!=percent) {
        if((p%20)==0) {
            percent=p;
            text.sprintf("Programming device %d%% written ...",percent);
            status(text);
        }
    }

}

// private function to send 256 byte block of the pof file.
void MainWindow::sendRawData() {
    unsigned char buffer[272];

    qDebug()<<"sendRawData offset="<<offset;

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
        for(int i=0;i<256;i++) {
                buffer[i+16]=(unsigned char)data[i+offset];
        }

        if(pcap_sendpacket(handle,buffer,272)!=0) {
            qDebug()<<"pcap_sendpacket failed";
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

// SLOT - commandCompleted - called when a command completed reply packet received
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
        if(bootloader) {
            sendRawData();
        } else {
            sendData();
        }
        break;
    case ERASING_ONLY:
        status("Device erased successfully");
        idle();
        break;
    case READ_MAC:
        break;
    case READ_IP:
        break;
    case WRITE_IP:
        status("Metis IP written successfully");
        idle();
        break;
    }
}

// SLOT - called when a ready for next buffer reply packet is received.
void MainWindow::nextBuffer() {
    offset+=256;
    if(offset<end) {
        if(bootloader) {
            sendRawData();
        } else {
            sendData();
        }
    } else {
        status("Programming device completed successfully");
        if(bootloader) {
            status("Remember to remove JP1 when you power cycle");
        }
        idle();
    }
}

// SLOT - called when a raw packet read times out (especially when erasing)
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
        status("Error: timeout reading MAC address!");
        idle();
        break;
    case READ_IP:
        status("Error: timeout reading IP address!");
        idle();
        break;
    case WRITE_IP:
        // should not happen as there is no repsonse
        break;
    }
}

// private function to set state to idle
void MainWindow::idle() {
    //qDebug()<<"idle";
    if(rawReceiveThread!=NULL) {
        rawReceiveThread->stop();
        rawReceiveThread=NULL;
    }
    if(receiveThread!=NULL) {
        receiveThread->stop();
        receiveThread=NULL;
    }
    state=IDLE;
}

// private function to display message in the status window
void MainWindow::status(QString text) {
    qDebug()<<"status:"<<text;
    ui->statusListWidget->insertItem(ui->statusListWidget->count()-1,text);
    ui->statusListWidget->setCurrentRow(ui->statusListWidget->count()-1);
}

void MainWindow::discover() {
    // using raw sockets as unable to get Qt UDP sockets to bind to a specific interface

    ui->statusListWidget->clear();
    status("");
    status("Metis Discovery");

    ui->metisComboBox->clear();
    metis.clear();

    // send the discovery packet
    int discovery_socket;
    struct sockaddr_in discovery_addr;
    int discovery_length;
    unsigned char buffer[17];
    int rc;
    int on=1;

    // start a thread to listen for discovery responses
    discoveryThread=new DiscoveryThread();
    discoveryThread->setIPAddress(ip);
    discoveryThread->start();

    QObject::connect(discoveryThread,SIGNAL(metis_found(unsigned char*,long)),this,SLOT(metis_found(unsigned char*,long)));

    discovery_socket=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(discovery_socket<0) {
        perror("create socket failed for discovery_socket\n");
        exit(1);
    }

    setsockopt(discovery_socket, SOL_SOCKET, SO_REUSEADDR, (char*)&on, sizeof(on));

    // bind to the selected interface
    struct sockaddr_in name={0,0,{0},{0}};
    name.sin_family = AF_INET;
    name.sin_addr.s_addr = htonl(ip);
    name.sin_port = htons(1024);
    rc=bind(discovery_socket,(struct sockaddr*)&name,sizeof(name));
    if(rc != 0) {
        fprintf(stderr,"cannot bind to interface: rc=%d\n", rc);
        exit(1);
    }

    rc=setsockopt(discovery_socket, SOL_SOCKET, SO_BROADCAST, (char*)&on, sizeof(on));
    if(rc != 0) {
        fprintf(stderr,"cannot set SO_BROADCAST: rc=%d\n", rc);
        exit(1);
    }

    buffer[0]=(char)0xEF; //header
    buffer[1]=(char)0XFE;
    buffer[2]=(char)0x02;
    buffer[3]=(char)ip&0xFF; //my ip
    buffer[4]=(char)(ip>>8)&0xFF;
    buffer[5]=(char)(ip>>16)&0xFF;
    buffer[6]=(char)(ip>>24)&0xFF;
    buffer[7]=(char)hw[0]; //my mac
    buffer[8]=(char)hw[1];
    buffer[9]=(char)hw[2];
    buffer[10]=(char)hw[3];
    buffer[11]=(char)hw[4];
    buffer[12]=(char)hw[5];
    buffer[13]=(char)0x0A;// metis ip
    buffer[14]=(char)0x01;
    buffer[15]=(char)0x01;
    buffer[16]=(char)0x02;

    discovery_length=sizeof(discovery_addr);
    memset(&discovery_addr,0,discovery_length);
    discovery_addr.sin_family=AF_INET;
    discovery_addr.sin_port=htons(1024);
    discovery_addr.sin_addr.s_addr=htonl(INADDR_BROADCAST);

    if(sendto(discovery_socket,(char*)buffer,sizeof(buffer),0,(struct sockaddr*)&discovery_addr,discovery_length)<0) {
        perror("sendto socket failed for discovery_socket\n");
        exit(1);
    }

    // disable the Discovery button
    ui->discoverPushButton->setDisabled(true);

    // wait 2 seconds to allow replys
    QTimer::singleShot(2000,this,SLOT(discovery_timeout()));
}

void MainWindow::discovery_timeout() {

    // stop the discovery listening thread
    discoveryThread->stop();

    if(ui->metisComboBox->count()>0) {
        ui->metisComboBox->setCurrentIndex(0);
        metisSelected(0);
    }

    // enable the Discovery button
    ui->discoverPushButton->setDisabled(false);

    QString text;
    text.sprintf("Discovery found %d Metis card(s)",ui->metisComboBox->count());
    status(text);
}

void MainWindow::metis_found(unsigned char* hw,long ip) {
    Metis* m;

    m=new Metis(ip,hw);

    //qDebug() << "metis_found";
    metis.append(m);
    ui->metisComboBox->addItem(m->toString());
    status(m->toString());
}

void MainWindow::metisSelected(int index) {
    if(index>=0) {
        metisIP=metis.at(index)->getIpAddress();
    }
}

void MainWindow::tabChanged(int index) {
    bootloader=(index==1);
    ui->tabWidget_2->setTabEnabled(2,bootloader);
    ui->tabWidget_2->setTabEnabled(3,bootloader);

}
