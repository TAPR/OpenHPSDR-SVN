/*
 * File:   mainwindow.cpp
 * Author: John Melton, G0ORX/N6LYT
 * Author: Dave Larsen, KV0S (Version 2)
 *
 * Created on 23 November 2010
 */

/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* 2013 - Dave Larsen, KV0S
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

MainWindow::MainWindow(QWidget *parent) :
    deviceIndicator( new QLabel ),
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    int nInterfaces;

    ui->setupUi(this);
    this->setWindowTitle(QString("HPSDRBootloader %1").arg(QString("%0 %1").arg(VERSION).arg(RELEASE)));

    ab = new AboutDialog(this);
    ab->setVersion( QString(VERSION), QString(RELEASE) );
    ab->setWindowTitle(QString("HPSDRBootloader %1").arg(VERSION));

    stat = new StatusDialog(this);
    stat->setWindowTitle(QString("HPSDRBootloader %1").arg(VERSION));
    add = new AddressDialog(this);
    add->writeEnabled();

    QCoreApplication::setOrganizationName("HPSDR");
    QCoreApplication::setOrganizationDomain("openhpsdr.org");
    QCoreApplication::setApplicationName("HPSDRProgrammer_V2");

    receiveThread=NULL;
    rawReceiveThread=NULL;
    discovery=NULL;
    currentboard="";

    deviceIndicator->setIndent(0);
    deviceIndicator->setPixmap (QPixmap(":/icons/green16.png"));
    deviceIndicator->setToolTip (QString ("Device port open"));

    statusBar()->addPermanentWidget (deviceIndicator);

    for (int i = 0; i < interfaces.getInterfaces(); ++i)
    {   ui->interfaceComboBox->addItem(interfaces.getInterfaceNameAt(i));
            ++nInterfaces;
    }


    connect(ui->actionAbout,SIGNAL(triggered()),ab,SLOT(show()));
    connect(ui->actionQuit,SIGNAL(triggered()),this,SLOT(quit()));
    connect(ui->actionIP_Address,SIGNAL(triggered()),add,SLOT(show()));
    connect(ui->actionStatus,SIGNAL(triggered()),stat,SLOT(show()));
    connect(ui->interfaceComboBox,SIGNAL(currentIndexChanged(int)),this,SLOT(interfaceSelected(int)));

    // Programmer Buttons
    connect(ui->fileBrowseButton,SIGNAL(clicked()),this,SLOT(browse()));
    connect(ui->fileProgramButton,SIGNAL(clicked()),this,SLOT(program()));
    connect(this,SIGNAL(retryProgram()),this,SLOT(program()));

    //JTAG programmer buttons
    connect(ui->interogateButton,SIGNAL(clicked()),this,SLOT(jtagInterrogate()));
    //connect(ui->jtagHelperButton,SIGNAL(clicked()),this,SLOT(jtagBrowse()));
    connect(ui->firmwareButton,SIGNAL(clicked()),this,SLOT(jtagFlashBrowse()));
    connect(ui->jtagProgramButton,SIGNAL(clicked()),this,SLOT(jtagFlashProgram()));
    connect(ui->boardTest,SIGNAL(clicked()),this,SLOT(getMAC()));

    connect(add,SIGNAL(writeIP()),this,SLOT(setIP()));
    connect(add,SIGNAL(readMACAddress()),this,SLOT(getMAC()));
    connect(add,SIGNAL(readIPAddress()),this,SLOT(getIP()));

    if(ui->interfaceComboBox->count()>0) {
       ui->interfaceComboBox->setCurrentIndex(0);
       interfaceSelected(0);
    } else {

    }

    bootloader=true;
}



MainWindow::~MainWindow()
{
    delete ui;
}

#ifdef Q_WS_MAC
void MainWindow::setPath(char* path) {
    myPath=path;
}
#endif

void MainWindow::quit() {
    exit(0);
}

// private function to display message in the status window
void MainWindow::status(QString text) {
    qDebug()<<"status:"<<text;
    ui->statusBar->showMessage( text );
    stat->status( text.trimmed() );
}

// SLOT - interfaceSelected - called when the interface selection is changed
void MainWindow::interfaceSelected(int index) {
    bool ok;
    interfaceName=interfaces.getInterfaceNameAt(index);
    ip=interfaces.getInterfaceIPAddress(index);
    hwAddress=interfaces.getInterfaceHardwareAddress(index);
    if(hwAddress==NULL) {
        ui->IPAddressLabel->setText("0.0.0.0");
        ui->MACAddressLabel->setText("00:00:00:00:00:00");
        status("Interface is not a valid network device");
    } else {
        text.sprintf("MAC=%s  IP=%ld.%ld.%ld.%ld",
                     hwAddress.toUtf8().constData(),
                     (ip>>24)&0xFF,(ip>>16)&0xFF,(ip>>8)&0xFF,ip&0xFF);
        QString MACstr = QString("%0").arg(hwAddress.toUtf8().constData());
        ui->MACAddressLabel->setText(MACstr);
        QString IPstr = QString("%0.%1.%2.%3").arg((ip>>24)&0xFF).arg((ip>>16)&0xFF).arg((ip>>8)&0xFF).arg(ip&0xFF);
        qDebug() << MACstr << IPstr;
        ui->IPAddressLabel->setText(IPstr);

        hw[0]=(unsigned char)hwAddress.mid(0,2).toInt(&ok,16);
        hw[1]=(unsigned char)hwAddress.mid(3,2).toInt(&ok,16);
        hw[2]=(unsigned char)hwAddress.mid(6,2).toInt(&ok,16);
        hw[3]=(unsigned char)hwAddress.mid(9,2).toInt(&ok,16);
        hw[4]=(unsigned char)hwAddress.mid(12,2).toInt(&ok,16);
        hw[5]=(unsigned char)hwAddress.mid(15,2).toInt(&ok,16);

    }
}

void MainWindow::metisSelected(int index) {

    currentBoardIndex = index;
    if(index>=0) {
        selectedBoardIP=bd.at(index)->getIpAddress();
        selectedBoardHostAddress=bd.at(index)->getHostAddress();
        selectedBoardMAC=bd[currentBoardIndex]->getMACAddress();
    }
}


// SLOT - browse - called when the "Browse ..." button on the Program tab is pressed.
void MainWindow::browse()
{
    // Clear other lineedits
    ui->firmwareLineEdit->clear();
    ui->interogateLineEdit->clear();
    //ui->jtagLineEdit->clear();
    QString dd = settings.value("dir").toString();
    QString fileName=QFileDialog::getOpenFileName(this,tr("Select File"),dd,tr("rbf Files (*.rbf)"));
    if( fileName.contains("metis") || fileName.contains("Metis") || fileName.contains("hermes") || fileName.contains("Hermes") || fileName.contains("griffin") || fileName.contains("Griffin") || fileName.contains("angelia") || fileName.contains("Angelia") ){
      QFileInfo *fileif = new QFileInfo(fileName);
      qDebug() << fileif->filePath();
      settings.setValue("dir", fileif->filePath());
      ui->fileLineEdit->setText(fileName);
      status( QString("Reading rbf file: %0").arg(fileName) );
    }else{
      QMessageBox::information(this, tr("HPSDRBootloader"),
              QString("HPSDRBootloader Programmer will only write to boards with and ethernet connection! \n\n Use HPSDRBootloader as a JTAG programmer for %0").arg(fileName), QMessageBox::Close);
      status( QString("HPSDRBootloader Programmer will only write to boards with and ethernet connection!") );
      status( QString("Use HPSDRBootloader as a JTAG programmer for %0").arg(fileName));
    }

}


// private load an rbf file
int MainWindow::loadRBF(QString filename) {
    int length;
    int i;
    int rc;

    QFile rbfFile(filename);
    rbfFile.open(QIODevice::ReadOnly);
    QDataStream in(&rbfFile);
    length=((rbfFile.size()+255)/256)*256;
    data=(char*)malloc(length);


    qDebug() << "file size=" << rbfFile.size() << "length=" << length;
    status(filename);
    status("reading file...");
    if(in.readRawData(data,rbfFile.size())!=rbfFile.size()) {
        status("Error: could not read rbf file");
        rbfFile.close();
        QApplication::restoreOverrideCursor();
        blocks=0;
        rc=1;
    } else {
        status("file read successfully");

        // pad out to mod 256 with 0xFF
        for(i=rbfFile.size();i<length;i++) {
            data[i]=0xFF;
        }
        rbfFile.close();

        start=0;
        end=length;
        blocks=length/256;

        qDebug() <<"start="<<start<<" end="<<end<<" blocks="<<blocks;
        rc=0;
    }
    return rc;
}

// SLOT - program - called when the "Program" button on the Program tab is pressed.
void MainWindow::program() {

    stat->clear();
    status("");
    percent=0;

    // check that an interface has been selected
    if(ui->interfaceComboBox->currentIndex()!=-1) {
        qDebug() << "Hardware address" << hwAddress << hw;
        // check that a file has been selected
        if(ui->fileLineEdit->text().endsWith(".rbf")) {
        //if(ui->fileLineEdit->text().endsWith(".pof")) {


            QApplication::setOverrideCursor(QCursor(Qt::BusyCursor));

            // load thefile
            //loadPOF(ui->fileLineEdit->text());
            if(loadRBF(ui->fileLineEdit->text())==0) {
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

    size=0;
    data_command=PROGRAM_FLASH;
    qDebug()<<"MainWindow::bootloaderProgram";
    handle=pcap_open_live(interfaces.getPcapName(ui->interfaceComboBox->currentText().toUtf8().constData()),1024,1,TIMEOUT,errbuf);
    if (handle == NULL) {
        qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toUtf8().constData()<<errbuf;
        QMessageBox::information(this, tr("HPSDRBootloader"),
              QString("Error: cannot open interface (are you running as root)"), QMessageBox::Close);
        status("Error: cannot open interface (are you running as root)");
    } else {
        rawReceiveThread=new RawReceiveThread(hw,handle);
        rawReceiveThread->start();
        QObject::connect(rawReceiveThread,SIGNAL(eraseCompleted()),this,SLOT(eraseCompleted()));
        QObject::connect(rawReceiveThread,SIGNAL(nextBuffer()),this,SLOT(nextBuffer()));
        QObject::connect(rawReceiveThread,SIGNAL(timeout()),this,SLOT(timeout()));

        // start by erasing
        state=ERASING;
        eraseData();
    }
}

void MainWindow::flashProgram() {

    size=blocks;
    data_command=PROGRAM_METIS_FLASH;

    // start a thread to listen for replies
    QString myip=interfaces.getInterfaceIPAddress(interfaceName);
    receiveThread=new ReceiveThread(&socket,myip,metisHostAddress);

    QObject::connect(receiveThread,SIGNAL(eraseCompleted()),this,SLOT(eraseCompleted()));
    QObject::connect(receiveThread,SIGNAL(nextBuffer()),this,SLOT(nextBuffer()));
    QObject::connect(receiveThread,SIGNAL(timeout()),this,SLOT(timeout()));

    state=ERASING;
    eraseData();

}

// SLOT - erase - called when the "Erase" button on the Erase taqb is pressed
void MainWindow::erase() {

    stat->clear();
    status("");

    qDebug()<<"erase";

    stat->clear();
    status("");
    percent=0;

    // check that an interface has been selected
    if(ui->interfaceComboBox->currentIndex()!=-1) {
        QApplication::setOverrideCursor(QCursor(Qt::BusyCursor));
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

    handle=pcap_open_live(interfaces.getPcapName(ui->interfaceComboBox->currentText().toUtf8().constData()),1024,1,TIMEOUT,errbuf);
    if (handle == NULL) {
        qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toUtf8().constData()<<errbuf;
        QMessageBox::information(this, tr("HPSDRBootloader"),
              QString("Error: cannot open interface (are you running as root)"), QMessageBox::Close);
        status("Error: cannot open interface (are you running as root)");
    } else {
        rawReceiveThread=new RawReceiveThread(hw,handle);
        QObject::connect(rawReceiveThread,SIGNAL(eraseCompleted()),this,SLOT(eraseCompleted()));
        QObject::connect(rawReceiveThread,SIGNAL(timeout()),this,SLOT(timeout()));
        rawReceiveThread->start();

        // start by erasing
        state=ERASING_ONLY;
        eraseData();
    }
}

void MainWindow::flashErase() {

    qDebug()<<"MainWindow::flashErase";

    QString myip=interfaces.getInterfaceIPAddress(interfaceName);
    receiveThread=new ReceiveThread(&socket,myip,metisHostAddress);

    // start erasing
    state=ERASING_ONLY;
    eraseData();
}

// SLOT - getMac - called when the "Read" button on the MAC Address tab is pressed.
void MainWindow::getMAC() {
    char errbuf[PCAP_ERRBUF_SIZE];


    qDebug()<<"getMAC";

    stat->clear();
    status("");

    //ui->macLineEdit->setText("");
    percent=0;

    // check that an interface has been selected
    if(ui->interfaceComboBox->currentIndex()!=-1) {
        //hw=interfaces.getInterfaceHardwareAddress(ui->interfaceComboBox->currentText());
        //ip=interfaces.getInterfaceIPAddress(ui->interfaceComboBox->currentText());

        handle=pcap_open_live(interfaces.getPcapName(ui->interfaceComboBox->currentText().toUtf8().constData()),1024,1,TIMEOUT,errbuf);
        if (handle == NULL) {
            qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toUtf8().constData()<<errbuf;
            ui->testStatement->setText(QString("No Board with Bootloader Found"));
            QMessageBox::information(this, tr("HPSDRBootloader"),
                  QString("Error: cannot open interface (are you running as root)"), QMessageBox::Close);
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
    text.sprintf("%02X:%02X:%02X:%02X:%02X:%02X",mac[0],mac[1],mac[2],mac[3],mac[4],mac[5]);
    //ui->programMACLabel->setText(text);
    add->setMACaddress(text);
    ui->boardMACaddress->setText(text);
    ui->testStatement->setText(QString("Board with Bootloader Found"));
    text.sprintf("MAC address read successfully");
    idle();
}

// SLOT - getIP - called when the "Read" button on the IP Address tab is pressed.
void MainWindow::getIP() {
    char errbuf[PCAP_ERRBUF_SIZE];

    qDebug()<<"getIP: "<<interfaceName;

    stat->clear();
    status("");

    //add->IPLineEdit1->setText("");
    //add->IPLineEdit2->setText("");
    //add->IPLineEdit3->setText("");
    //add->IPLineEdit4->setText("");
    percent=0;

    // check that an interface has been selected
    if(ui->interfaceComboBox->currentIndex()!=-1) {

        handle=pcap_open_live(interfaces.getPcapName(ui->interfaceComboBox->currentText().toUtf8().constData()),1024,1,TIMEOUT,errbuf);
        if (handle == NULL) {
            qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toUtf8().constData()<<errbuf;
            QMessageBox::information(this, tr("HPSDRBootloader"),
                  QString("Error: cannot open interface (are you running as root)"), QMessageBox::Close);
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
    QStringList saddr = QStringList();
    text.sprintf("%d",ip[0]);
    saddr.append(QString("").sprintf("%d",ip[0]));
    text.sprintf("%d",ip[1]);
    saddr.append(QString("").sprintf("%d",ip[1]));
    text.sprintf("%d",ip[2]);
    saddr.append(QString("").sprintf("%d",ip[2]));
    text.sprintf("%d",ip[3]);
    saddr.append(QString("").sprintf("%d",ip[3]));

    add->setIPaddress( saddr );

    QString ipstr;


    text.sprintf("HPSDR Board IP address read successfully");
    status(text);
    idle();
}

// SLOT - setIP - called when the "Write" button on the IP Address tab is pressed.
void MainWindow::setIP() {
    char errbuf[PCAP_ERRBUF_SIZE];
    unsigned char buffer[66];
    int addr[4];
    int i;

    qDebug()<<"setIP";

    stat->clear();
    status("");


    QStringList *saddr = new QStringList();
    add->getNewIPAddress( saddr );

    addr[0] = saddr->at(0).toInt();
    addr[1] = saddr->at(1).toInt();
    addr[2] = saddr->at(2).toInt();
    addr[3] = saddr->at(3).toInt();

    QString ipstr;
    ipstr = QString("%0.%1.%2.%3").arg(saddr->at(0)).arg(saddr->at(1)).arg(saddr->at(2)).arg(saddr->at(3));
    qDebug() << "new address" << ipstr;

    handle=pcap_open_live(interfaces.getPcapName(ui->interfaceComboBox->currentText().toUtf8().constData()),1024,1,TIMEOUT,errbuf);
    if (handle == NULL) {
            qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toUtf8().constData()<<errbuf;
            QMessageBox::information(this, tr("HPSDRBootloader"),
                  QString("Error: cannot open interface (are you running as root)"), QMessageBox::Close);
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
            buffer[15]=WRITE_METIS_IP;

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
                text.sprintf("Written IP address");
                status(text);
            }

    }


    QMessageBox::information(this, tr("HPSDRBootloader"),QString("The HPSDR board has been changed to IP address: %1.\n").arg(ipstr),QMessageBox::Close);

    idle();

}

// private function to send the command to erase
void MainWindow::eraseData() {
    eraseTimeouts=0;

    status(QString("Erasing device ... (takes up to %0 seconds)").arg(ANGELIA_MAX_ERASE_TIMEOUTS/1000));


    if(bootloader) {
        sendRawCommand(ERASE_METIS_FLASH);
    } else {
        sendCommand(ERASE_METIS_FLASH);
        // wait 20 seconds to allow replys

        QTimer::singleShot(ANGELIA_MAX_ERASE_TIMEOUTS,this,SLOT(erase_timeout()));


    }
}


// slot for erase timout
void MainWindow::erase_timeout() {
    qDebug()<<"MainWindow::erase_timeout";
    if(state==ERASING || state==ERASING_ONLY) {
        status("Error: erase timeout.");
        status("Try again.");
        idle();
        QApplication::restoreOverrideCursor();
    }
}

// private function to send command to read MAC address from Metis
void MainWindow::readMAC() {
    eraseTimeouts=0;
    text.sprintf("Reading MAC address ...");
    status(text);
    sendRawCommand(READ_METIS_MAC);
}

// private function to read the IP address from Metis.
void MainWindow::readIP() {
    eraseTimeouts=0;
    text.sprintf("Reading IP address ...");
    status(text);
    sendRawCommand(READ_METIS_IP);
}

// private function to send a command.
void MainWindow::sendCommand(unsigned char command) {
    unsigned char buffer[64];
    int i;

    qDebug()<<"sendCommand "<<command;

    buffer[0]=0xEF; // protocol
    buffer[1]=0xFE;

    buffer[2]=0x03;
    buffer[3]=command;

    /*fill the frame with 0x00*/
    for(i=0;i<60;i++) {
        buffer[i+4]=(unsigned char)0x00;
    }

    receiveThread->send((const char*)buffer,sizeof(buffer));

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
    buffer[3]=PROGRAM_METIS_FLASH;
    buffer[4]=(blocks>>24)&0xFF;
    buffer[5]=(blocks>>16)&0xFF;
    buffer[6]=(blocks>>8)&0xFF;
    buffer[7]=blocks&0xFF;

    /*fill the frame with some data*/
    for(int i=0;i<256;i++) {
        buffer[i+8]=(unsigned char)data[i+offset];
    }

    receiveThread->send((const char*)buffer,sizeof(buffer));

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
    unsigned char buffer[276];

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
        buffer[15]=PROGRAM_METIS_FLASH;

        buffer[16]=0x00;
        buffer[17]=0x00;
        buffer[18]=0x00;
        buffer[19]=0x00;

        /*fill the frame with some data*/
        for(int i=0;i<256;i++) {
                buffer[i+20]=(unsigned char)data[i+offset];
        }

        if(pcap_sendpacket(handle,buffer,276)!=0) {
            qDebug()<<"pcap_sendpacket failed";
            status("send data command failed");
            idle();
        } else {
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

// private function to send 256 byte block of the pof file.
void MainWindow::sendJTAGData() {
    unsigned char buffer[276];
    int length=blocks*256;

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
        buffer[15]=data_command;


        buffer[16]=(length>>24)&0xFF;
        buffer[17]=(length>>16)&0xFF;
        buffer[18]=(length>>8)&0xFF;
        buffer[19]=length&0xFF;

        /*fill the frame with some data*/
        for(int i=0;i<256;i++) {
                buffer[i+20]=(unsigned char)data[i+offset];
        }

        if(pcap_sendpacket(handle,buffer,276)!=0) {
            qDebug()<<"pcap_sendpacket failed";
            status("send data command failed");
            idle();
        } else {
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

// private function to send 256 byte block of the pof file.
void MainWindow::sendJTAGFlashData() {
    unsigned char buffer[276];

    qDebug()<<"sendJTAGFlashData offset="<<offset;

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
        buffer[15]=PROGRAM_FLASH;

        buffer[16]=(blocks>>24)&0xFF;
        buffer[17]=(blocks>>16)&0xFF;
        buffer[18]=(blocks>>8)&0xFF;
        buffer[19]=blocks&0xFF;

        /*fill the frame with some data*/
        for(int i=0;i<256;i++) {
                buffer[i+20]=(unsigned char)data[i+offset];
        }

        if(pcap_sendpacket(handle,buffer,276)!=0) {
            qDebug()<<"pcap_sendpacket failed";
            status("send data command failed");
            idle();
        } else {
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

// SLOT - eraseCompleted
void MainWindow::eraseCompleted() {
    switch(state) {
    case IDLE:
        qDebug()<<"received eraseCompleted when state is IDLE";
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
        QApplication::restoreOverrideCursor();
        break;
    case READ_MAC:
        qDebug()<<"received eraseCompleted when state is READ_MAC";
        break;
    case READ_IP:
        qDebug()<<"received eraseCompleted when state is READ_IP";
        break;
    case WRITE_IP:
        text.sprintf("HPSDR Board IP address written successfully");
        status(text);
        idle();
        break;
    case JTAG_INTERROGATE:
        qDebug()<<"received eraseCompleted when state is JTAG_INTERROGATE";
        break;
    case JTAG_PROGRAM:
        qDebug()<<"received eraseCompleted when state is JTAG_PROGRAM";
        break;
    case FLASH_ERASING:
        status("Flash erased successfully");
        // now load the flash
        loadFlash();
        break;
    case FLASH_PROGRAM:
        qDebug()<<"received eraseCompleted when state is FLASH_PROGRAM";
        break;
    }
}

// SLOT - fpgaId
void MainWindow::fpgaId(unsigned char* data) {
    switch(state) {
    case JTAG_INTERROGATE:
        fpga_id=((data[0]&0xFF)<<16)+((data[1]&0xFF)<<8)+(data[2]&0xFF);
        if(fpga_id==0) {
            status("No Mercury or Penelope board found");
            status("Make sure that Metis is in the slot farthest away from the power connector.");
            status("The target board should be in the next slot adjacent to Metis.");
            status("The target board has the 'Last JTAG' jumper installed.");
            status("There are no other boards on the Atlas Bus.");
        } else if(fpga_id==0x020F30) {

            status("found Mercury, Select Source");
            int ret = QMessageBox::information(this, tr("HPSDRBootloader"),
                  QString("Found a Mercury, is it a TAPR board?"), QMessageBox::Yes, QMessageBox::No);
            if( ret == QMessageBox::Yes ){
               ui->interogateLineEdit->setText("TAPR-Mercury - 0x020F30");
               rbfstr = QString(":/rbf/Mercury_JTAG.rbf");
            }else if( ret == QMessageBox::No ){
               ui->interogateLineEdit->setText("EU-Mercury - 0x020F30");
               rbfstr = QString(":/rbf/MercuryEURev1_JTAG.rbf");
            }

        } else if(fpga_id==0x020B20) {
            status("found Penelope");
            ui->interogateLineEdit->setText("Penelope - 0x020B20");
            rbfstr = QString(":/rbf/Penelope_JTAG.rbf");
            //ui->jtagLineEdit->setText(rbfstr);
        } else {
            status("unknown FPGA id");
            fpga_id=0;
        }
        idle();
        break;
    default:
        qDebug()<<"received fpgaId when state is not JTAG_INTERROGATE";
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
        status("Programming device completed successfully.");
        if(bootloader) {
            text.sprintf("Remember to remove jumper on (J1 or J12). Power cycle.");
            status(text);

        }
        idle();
        QApplication::restoreOverrideCursor();
    }
}

// SLOT - called when a raw packet read times out (especially when erasing)
void MainWindow::timeout() {
    //qDebug()<<"MainWindow::timeout state="<<state;
    switch(state) {
    case IDLE:
        // ignore
        break;
    case ERASING:
    case ERASING_ONLY:
        if(bootloader) {
            eraseTimeouts++;
            //qDebug()<<"eraseTimeouts="<<eraseTimeouts;
            if(eraseTimeouts==ANGELIA_MAX_ERASE_TIMEOUTS) {
                status("Error: erase timeout.");
                //QMessageBox::StandardButton reply;
                //reply = QMessageBox::question(this, tr("HPSDRBootloader"),
                //      QString("Erase timed out, try again?"), QMessageBox::No|QMessageBox::Yes);
                //if (reply == QMessageBox::Yes) {
                //    qDebug() << "Yes was clicked";
                //    emit retryProgram();
                //}


                text.sprintf("Remember to remove jumper on (J1 or J12). Try program again.");
                status(text);
                idle();
                QApplication::restoreOverrideCursor();
            }
        } else {
            status("Error: erase timeout.");
            status("Try program again.");
            idle();
            QApplication::restoreOverrideCursor();
        }
        break;
    case PROGRAMMING:
        //qDebug()<<"timeout";
        break;
    case READ_MAC:
        status("Error: timeout reading MAC address!");
        status("Check that the correct interface is selected.");
        text.sprintf("Check that there is a jumper on J1 for Metis");
        status(text);
        status(text);
        idle();
        break;
    case READ_IP:
        status("Error: timeout reading IP address!");
        status("Check that the correct interface is selected.");
        text.sprintf("Check that there is a jumper on J1 for Metis");

        status(text);
        status(text);
        idle();
        break;
    case WRITE_IP:
        // should not happen as there is no repsonse
        break;
    case JTAG_INTERROGATE:
        status("Error: timeout reading interrogating JTAG chain!");
        status("Check that the correct interface is selected.");
        text.sprintf("Check that there is a jumper on J1 for Metis");
        status(text);
        status(text);
        idle();
        break;
    case JTAG_PROGRAM:
        //qDebug() << "timeout while state is JTAG_PROGRAM";
        break;
    case FLASH_ERASING:
        eraseTimeouts++;
        if(eraseTimeouts==ANGELIA_MAX_ERASE_TIMEOUTS) {
            status("Error: erase timeout - try again?");
            idle();
            QApplication::restoreOverrideCursor();
        }
        break;
    case FLASH_PROGRAM:
        //qDebug() << "timeout while state is FLASH_PROGRAM";
        break;
    }
}

// private function to set state to idle
void MainWindow::idle() {
    qDebug()<<"idle";
    state=IDLE;
    if(rawReceiveThread!=NULL) {
        rawReceiveThread->stop();
        rawReceiveThread=NULL;
    }
    if(receiveThread!=NULL) {
        receiveThread->stop();
        receiveThread=NULL;
    }

}



void MainWindow::jtagInterrogate() {
    char errbuf[PCAP_ERRBUF_SIZE];

    //Clear Program lineedit
    ui->fileLineEdit->clear();
    ui->firmwareLineEdit->clear();

    //ui->jtagLineEdit->clear();

    stat->clear();
    status("");

    handle=pcap_open_live(interfaces.getPcapName(ui->interfaceComboBox->currentText().toUtf8().constData()),1024,1,TIMEOUT,errbuf);
    if (handle == NULL) {
        qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toUtf8().constData()<<errbuf;
        QMessageBox::information(this, tr("HPSDRBootloader"),
              QString("Error: cannot open interface (are you running as root)"), QMessageBox::Close);
        status("Error: cannot open interface (are you running as root)");
    } else {
        rawReceiveThread=new RawReceiveThread(hw,handle);
        QObject::connect(rawReceiveThread,SIGNAL(fpgaId(unsigned char*)),this,SLOT(fpgaId(unsigned char*)));
        QObject::connect(rawReceiveThread,SIGNAL(timeout()),this,SLOT(timeout()));
        rawReceiveThread->start();

        state=JTAG_INTERROGATE;

        eraseTimeouts=0;

        status("Interrogating JTAG chain");
        sendRawCommand(GET_JTAG_DEVICE_ID);

    }
}

void MainWindow::jtagBrowse() {
    QString dd = settings.value("dir").toString();
    QString fileName=QFileDialog::getOpenFileName(this,tr("Select File"),dd,tr("rbf Files (*.rbf)"));
    if( fileName.contains("mercury") || fileName.contains("Mercury") || fileName.contains("penelope") || fileName.contains("Penelope") ){
      QFileInfo *fileif = new QFileInfo(fileName);
      qDebug() << fileif->filePath();
      settings.setValue("firmwaredir", fileif->filePath());
      //ui->jtagLineEdit->setText(fileName);
    }else{
      QMessageBox::information(this, tr("HPSDRBootloader"),
              QString("HPSDRBootloader JTAG Programmer will only write to Mercury and Penelope/Pennylane boards! \n\n Use HPSDRBootloader Programmer for %0").arg(fileName), QMessageBox::Close);
      status( QString("HPSDRBootloader JTAG Programmer will only write to Mercury and Penelope/Pennylane boards!") );
      status( QString("Use HPSDRBootloader Programmer for %0").arg(fileName));
    }
}

void MainWindow::jtagProgram() {

    qDebug()<<"MainWIndow::jtagProgram";

    stat->clear();
    status("");

    if(fpga_id==0x020F30 || fpga_id==0x020B20) {
        if(rbfstr.endsWith(".rbf")) {
            if(fpga_id==0x020F30) {
                // Mercury
                data_command=PROGRAM_MERCURY;
            } else {
                // Penelope
                data_command=PROGRAM_PENELOPE;
            }
            loadRBF(rbfstr);
            if(blocks>0) {
                if(bootloader) {
                    jtagBootloaderProgram();
                } else {
                    //jtagFlashProgram();
                }
            }
        } else {
            status("No file selected.");
        }
    } else {
        status("No target defined");
    }
}

int MainWindow::loadMercuryRBF(QString filename) {
    int length;
    int i;
    int rc;

    qDebug()<<"MainWindow::loadMercuryRBF "<<filename;
    QFile rbfFile(filename);
    rbfFile.open(QIODevice::ReadOnly);
    QDataStream in(&rbfFile);
    length=((rbfFile.size()+16+255)/256)*256;
    data=(char*)malloc(length);


    qDebug() << "file size=" << rbfFile.size() << "length=" << length;
    status(filename);
    status("reading file...");
    if(in.readRawData(data,rbfFile.size())!=rbfFile.size()) {
        status("Error: could not read rbf file");
        rbfFile.close();
        QApplication::restoreOverrideCursor();
        blocks=0;
        rc=1;
    } else {
        status("file read successfully");

        // pad out to mod 256 with 0xFF
        for(i=rbfFile.size();i<length;i++) {
            data[i]=0xFF;
        }
        rbfFile.close();

        start=0;
        end=length;
        blocks=length/256;

        qDebug() <<"start="<<start<<" end="<<end<<" blocks="<<blocks;

        rc=0;
    }
    return rc;
}

int MainWindow::loadPenelopeRBF(QString filename) {
    int length;
    int i;
    int rc;

    qDebug()<<"MainWindow::loadPenelopeRBF "<<filename;

    QFile rbfFile(filename);
    rbfFile.open(QIODevice::ReadOnly);
    QDataStream in(&rbfFile);
    length=((rbfFile.size()-44+255)/256)*256;
    data=(char*)malloc(length+44);


    qDebug() << "file size=" << rbfFile.size() << "length=" << length;
    status(filename);
    status("reading file...");
    if(in.readRawData(data,rbfFile.size())!=rbfFile.size()) {
        status("Error: could not read rbf file");
        rbfFile.close();
        QApplication::restoreOverrideCursor();
        blocks=0;
        rc=1;
    } else {
        status("file read successfully");

        // pad out to mod 256 with 0xFF
        for(i=rbfFile.size();i<length;i++) {
            data[i]=0xFF;
        }
        rbfFile.close();

        start=44;
        end=length;
        blocks=length/256;

        qDebug() <<"start="<<start<<" end="<<end<<" blocks="<<blocks;

        rc=0;
    }
    return rc;
}

void MainWindow::jtagBootloaderProgram() {
    char errbuf[PCAP_ERRBUF_SIZE];

    qDebug()<<"MainWindow::jtagBootloaderProgram";

    stat->clear();
    status("");

    handle=pcap_open_live(interfaces.getPcapName(ui->interfaceComboBox->currentText().toUtf8().constData()),1024,1,TIMEOUT,errbuf);
    if (handle == NULL) {
        qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toUtf8().constData()<<errbuf;
        QMessageBox::information(this, tr("HPSDRBootloader"),
              QString("Error: cannot open interface (are you running as root)"), QMessageBox::Close);
        status("Error: cannot open interface (are you running as root)");
    } else {
        rawReceiveThread=new RawReceiveThread(hw,handle);
        rawReceiveThread->start();
        QObject::connect(rawReceiveThread,SIGNAL(commandCompleted()),this,SLOT(commandCompleted()));
        QObject::connect(rawReceiveThread,SIGNAL(nextBuffer()),this,SLOT(nextBuffer()));
        QObject::connect(rawReceiveThread,SIGNAL(timeout()),this,SLOT(timeout()));

        state=JTAG_PROGRAM;
        sendRawData();
    }
}

// private function to send the command to erase
void MainWindow::jtagEraseData() {
    eraseTimeouts=0;
    sendRawCommand(JTAG_ERASE_FLASH);
}

void MainWindow::jtagFlashBrowse() {

    QString dd = settings.value("dir").toString();
    QString fileName=QFileDialog::getOpenFileName(this,tr("Select File"),dd,tr("rbf Files (*.rbf)"));
    if(ui->interogateLineEdit->text().contains("Mercury") && (fileName.contains("mercury") || fileName.contains("Mercury") )){
       QFileInfo *fileif = new QFileInfo(fileName);
       qDebug() << fileif->filePath();
       settings.setValue("firmwaredir", fileif->filePath());
       ui->firmwareLineEdit->setText(fileName);
    }  else if( ui->interogateLineEdit->text().contains("Penelope") && (fileName.contains("penelope") || fileName.contains("Penelope") )){
       QFileInfo *fileif = new QFileInfo(fileName);
       qDebug() << fileif->filePath();
       settings.setValue("firmwaredir", fileif->filePath());
       ui->firmwareLineEdit->setText(fileName);
    }else{
       QMessageBox::information(this, tr("HPSDRBootloader"),
             QString("%0 has been found! This rbf does not match: %1").arg(ui->interogateLineEdit->text()).arg(fileName), QMessageBox::Close);
       status( QString("HPSDRBootloader JTAG Programmer will only write to Mercury and Penelope/Pennylane boards!") );
       status( QString("Penelope has been found! This rbf does not match: %0").arg(fileName));
    }

}

void MainWindow::jtagFlashProgram() {
    char errbuf[PCAP_ERRBUF_SIZE];

    qDebug()<<"MainWIndow::jtagFlashProgram";
    stat->clear();
    status("");

    // validate file selection
    if(!rbfstr.endsWith(".rbf")) {
        status("Error: No JTAG Program file selected.");
        return;
    }
    if(!ui->firmwareLineEdit->text().endsWith(".rbf")) {
        status("Error: No Flash Program file selected");
        return;
    }

    // try to load the JTAG.rbf file
    if(fpga_id==0x020F30) {
        // Mercury
        if(loadMercuryRBF(rbfstr)!=0) {
            status("Error: Failed to load Mercury_JTAG Program file.");
            return;
        }
        data_command=PROGRAM_MERCURY;
    } else if(fpga_id==0x020B20) {
        // Penelope
        if(loadPenelopeRBF(rbfstr)!=0) {
            status("Error: Failed to load Penelope_JTAG Program file.");
            return;
        }
        data_command=PROGRAM_PENELOPE;
    } else {
        status("Error: Undefined FPGA ID.");
        return;
    }

    handle=pcap_open_live(interfaces.getPcapName(ui->interfaceComboBox->currentText().toUtf8().constData()),1024,1,TIMEOUT,errbuf);
    if (handle == NULL) {
        qDebug()<<"Couldn't open device "<<ui->interfaceComboBox->currentText().toUtf8().constData()<<errbuf;
        QMessageBox::information(this, tr("HPSDRBootloader"),
              QString("Error: cannot open interface (are you running as root)"), QMessageBox::Close);
        status("Error: cannot open interface (are you running as root)");
    }

    rawReceiveThread=new RawReceiveThread(hw,handle);
    rawReceiveThread->start();
    QObject::connect(rawReceiveThread,SIGNAL(eraseCompleted()),this,SLOT(eraseCompleted()));
    QObject::connect(rawReceiveThread,SIGNAL(nextBuffer()),this,SLOT(nextJTAGBuffer()));
    QObject::connect(rawReceiveThread,SIGNAL(timeout()),this,SLOT(timeout()));

    // send the JTAG.rbf file
    state=JTAG_PROGRAM;
    offset=start;

    sendJTAGData();
}

void MainWindow::nextJTAGBuffer() {
    qDebug()<<"MainWIndow::nextJTABBuffer state="<<state<<" offset="<<offset;
    if(state==JTAG_PROGRAM) {
        offset+=256;
        if(offset<end) {
            sendJTAGData();
        } else {
            status("Loaded successfully.");

            // wait 2 seconds to start erase
            QTimer::singleShot(2000,this,SLOT(startJTAGFlashErase()));

        }
    } else { // FLASH Programming
        offset+=256;
        if(offset<end) {
            sendJTAGFlashData();
        } else {
            status("Loaded Flash successfully.");
            text.sprintf("Remember to remove jumper on J1 on Metis. Then power cycle.");
            status(text);
            idle();
        }
    }
}

void MainWindow::startJTAGFlashErase() {
    state=FLASH_ERASING;
    status("Erasing Flash ... (takes several seconds)");
    jtagEraseData();
}

void MainWindow::loadFlash() {
    qDebug()<<"MainWindow::loadFlash";
    if(loadRBF(ui->firmwareLineEdit->text())!=0) {
        status("Error: Failed to load Flash Program file.");
        return;
    }
    data_command=PROGRAM_MERCURY;
    state=FLASH_PROGRAM;
    offset=start;
    sendJTAGFlashData();
}


