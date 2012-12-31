/*
 * File:   mainwindow.cpp
 * Author: John Melton, G0ORX/N6LYT
 *
 * Created on 23 November 2010
 *
 * Revised on December 29, 2012
 * Author: Dave Larsen, KV0S
 */

/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* 2012 - Dave Larsen, KV0S
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

    this->setWindowTitle(QString("HPSDRProgrammer V2 %1").arg(QString("%0 %1").arg(VERSION).arg(RELEASE)));

    ab = new AboutDialog();
    ab->setVersion( QString(VERSION), QString(RELEASE) );

    stat = new StatusDialog();


    receiveThread=NULL;
    rawReceiveThread=NULL;
    discovery=NULL;

    deviceIndicator->setIndent(0);
    deviceIndicator->setPixmap (QPixmap(":/icons/red16.png"));
    deviceIndicator->setToolTip (QString ("Device port not open"));

    statusBar()->addPermanentWidget (deviceIndicator);

#ifdef __WIN32
    //ui->privilegesLabel->setText("You must be running with Administrator privileges to be able to read/write raw ethernet frames.");
    QRect rect=ui->interfaceComboBox->geometry();
    rect.setWidth(ui->interfaceLabel->width());
    ui->interfaceComboBox->setGeometry(rect);
#else
    //ui->privilegesLabel->setText("You must be running as root to be able to read/write raw ethernet frames.");
#endif

    for (int i = 0; i < interfaces.getInterfaces(); ++i)
    {   ui->   interfaceComboBox->addItem(interfaces.getInterfaceNameAt(i));
        ++nInterfaces;
    }

    connect(ui->actionAbout,SIGNAL(triggered()),ab,SLOT(show()));
    connect(ui->actionQuit,SIGNAL(triggered()),this,SLOT(quit()));
    connect(ui->interfaceComboBox,SIGNAL(currentIndexChanged(int)),this,SLOT(interfaceSelected(int)));
    connect(ui->HermesButton,SIGNAL(clicked()),this,SLOT(hermesSelected()));
    connect(ui->AngeliaButton,SIGNAL(clicked()),this,SLOT(angeliaSelected()));
    connect(ui->MetisButton,SIGNAL(clicked()),this,SLOT(metisSelected()));

    connect(ui->discoverButton,SIGNAL(clicked()),this,SLOT(discover()));

    connect(ui->programButton,SIGNAL(clicked()),this,SLOT(program()));
    connect(ui->browseButton,SIGNAL(clicked()),this,SLOT(browse()));

    connect(ui->actionStatus,SIGNAL(triggered()),stat,SLOT(show()));


    if(ui->interfaceComboBox->count()>0) {
       ui->interfaceComboBox->setCurrentIndex(0);
       interfaceSelected(0);
    } else {
       // dont allow discovery if no interface found
       //ui->discoverPushButton->setEnabled(false);
    }
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
        ui->IPInterfaceLabel->setText("0.0.0.0");
        ui->MACInterfaceLabel->setText("00:00:00:00:00:00");
        status("Interface is not a valid network device");
    } else {
        text.sprintf("MAC=%s  IP=%ld.%ld.%ld.%ld",
                     hwAddress.toUtf8().constData(),
                     (ip>>24)&0xFF,(ip>>16)&0xFF,(ip>>8)&0xFF,ip&0xFF);
        QString MACstr = QString("%0").arg(hwAddress.toUtf8().constData());
        ui->MACInterfaceLabel->setText(MACstr);
        QString IPstr = QString("%0.%1.%2.%3").arg((ip>>24)&0xFF).arg((ip>>16)&0xFF).arg((ip>>8)&0xFF).arg(ip&0xFF);
        qDebug() << MACstr << IPstr;
        ui->IPInterfaceLabel->setText(IPstr);

        hw[0]=(unsigned char)hwAddress.mid(0,2).toInt(&ok,16);
        hw[1]=(unsigned char)hwAddress.mid(3,2).toInt(&ok,16);
        hw[2]=(unsigned char)hwAddress.mid(6,2).toInt(&ok,16);
        hw[3]=(unsigned char)hwAddress.mid(9,2).toInt(&ok,16);
        hw[4]=(unsigned char)hwAddress.mid(12,2).toInt(&ok,16);
        hw[5]=(unsigned char)hwAddress.mid(15,2).toInt(&ok,16);

    }
}

void MainWindow::metisSelected() {
    isMetis=true;
    board = metis;
    qDebug() << "Metis selected";
    status( "Metis selected");
}

void MainWindow::metisSelected(int index) {
    if(index>=0) {
        metisIP=bd.at(index)->getIpAddress();
        metisHostAddress=bd.at(index)->getHostAddress();

        if(isMetis) {
            if(bd.at(index)->getBoard()==1) {
                status("Warning: you have Metis selected but board selected is Hermes!");
            }
        } else {
            if(bd.at(index)->getBoard()==0) {
                status("Warning: you have Hermes selected but board selected is Metis!");
            }

        }
    }
}


void MainWindow::hermesSelected() {
    isMetis=false;
    board = hermes;
    qDebug() << "Hermes selected";
    status( "Hermes selected");
}

void MainWindow::angeliaSelected()
{
    isMetis=false;
    board = angelia;
    qDebug() << "Angelia selected";
    status( "Angelia selected");
}



// SLOT - browse - called when the "Browse ..." button on the Program tab is pressed.
void MainWindow::browse()
{
    //QString fileName=QFileDialog::getOpenFileName(this,tr("Select File"),"",tr("pof Files (*.pof)"));
    QString fileName=QFileDialog::getOpenFileName(this,tr("Select File"),"",tr("rbf Files (*.rbf)"));
    ui->fileLineEdit->setText(fileName);
    status( QString("Reading rbf file: %0").arg(fileName) );
}


void MainWindow::discover() {
    stat->clear();
    status("");
    if( board == metis )
    {
        text = QString("%0 Discovery").arg("Metis");
    }else if( board == hermes){
        text = QString("%0 Discovery").arg("Hermes");
    }else{
        text = QString("%0 Discovery").arg("Angelia");
    }
    status(text);

    ui->discoverComboBox->clear();
    bd.clear();



    QString myip=interfaces.getInterfaceIPAddress(interfaceName);

    QApplication::setOverrideCursor(QCursor(Qt::BusyCursor));

    socket.close();
    if(!socket.bind(QHostAddress(ip),0,QUdpSocket::ReuseAddressHint)) {
        qDebug()<<"Error: Discovery: bind failed "<<socket.errorString();
        return;
    }

    discovery=new Discovery(&socket,myip);
    connect(discovery,SIGNAL(board_found(Board*)),this,SLOT(board_found(Board*)));
    discovery->discover();
    // disable the Discovery button
    ui->discoverButton->setDisabled(true);

    // wait 9 seconds to allow replys
    QTimer::singleShot(9000,this,SLOT(discovery_timeout()));
}

void MainWindow::discovery_timeout() {

    discovery->stop();
    if(ui->discoverComboBox->count()>0) {
        ui->discoverComboBox->setCurrentIndex(0);
        metisSelected(0);
    }

    // enable the Discovery button
    ui->discoverButton->setDisabled(false);

    text.sprintf("Discovery found %d card(s)",ui->discoverComboBox->count());
    status(text);
    if(ui->discoverComboBox->count()==0) {
        status("Make sure the correct interface is selected.");
        text.sprintf("Make sure that there is no jumper on %s.",isMetis?"JP1":"J12");
        deviceIndicator->setPixmap (QPixmap(":/icons/red16.png"));
        deviceIndicator->setToolTip (QString ("Device port not open"));
    }else{
        deviceIndicator->setPixmap (QPixmap(":/icons/green16.png"));
        deviceIndicator->setToolTip (QString ("Device port open"));
    }
    QApplication::restoreOverrideCursor();
}

void MainWindow::board_found(Board* m) {

    if(htonl(m->getIpAddress())!=ip) {
        bd.append(m);
        ui->discoverComboBox->addItem(m->toString());
        status(m->toString());
        if(isMetis) {
            if(m->getBoard()!=0) {
                status("Warning: you have Metis selected but board is Hermes!");
            }
        } else if(m->getBoard()!=1) {
            status("Warning: you have Hermes selected but board is Metis!");
        }

    }
}


// SLOT - program - called when the "Program" button on the Program tab is pressed.
void MainWindow::program() {

    stat->clear();
    status("");
    percent=0;

    // check that an interface has been selected
    if(ui->interfaceComboBox->currentIndex()!=-1) {
        // check that a file has been selected
        if(ui->fileLineEdit->text().endsWith(".rbf")) {
        //if(ui->fileLineEdit->text().endsWith(".pof")) {


            QApplication::setOverrideCursor(QCursor(Qt::BusyCursor));

            // load thefile
            //loadPOF(ui->fileLineEdit->text());
            if(loadRBF(ui->fileLineEdit->text())==0) {
                //if(bootloader) {
                //    bootloaderProgram();
                //} else {
                    flashProgram();
                //}
            }
        } else {
            status("Error: no file selected");
        }
    } else {
        status("Error: no interface selected");
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

void MainWindow::flashProgram() {

    size=blocks;
    data_command=PROGRAM_METIS_FLASH;

    // start a thread to listen for replies
    QString myip=interfaces.getInterfaceIPAddress(interfaceName);
    receiveThread=new ReceiveThread(&socket,myip,metisHostAddress);

    connect(receiveThread,SIGNAL(eraseCompleted()),this,SLOT(eraseCompleted()));
    connect(receiveThread,SIGNAL(nextBuffer()),this,SLOT(nextBuffer()));
    connect(receiveThread,SIGNAL(timeout()),this,SLOT(timeout()));

    state=ERASING;
    eraseData();

}

// private function to send the command to erase
void MainWindow::eraseData() {
    eraseTimeouts=0;
    status("Erasing device ... (takes several seconds)");
    //if(bootloader) {
    //    sendRawCommand(ERASE_METIS_FLASH);
    //} else {
        sendCommand(ERASE_METIS_FLASH);
        // wait 20 seconds to allow replys
        QTimer::singleShot(20000,this,SLOT(erase_timeout()));
    //}
}

// slot for erase timout
void MainWindow::erase_timeout() {
    qDebug()<<"MainWindow::erase_timeout";
    if(state==ERASING || state==ERASING_ONLY) {
        status("Error: erase timeout.");
        status("Power cycle and try again.");
        idle();
        QApplication::restoreOverrideCursor();
    }
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


// SLOT - called when a ready for next buffer reply packet is received.
void MainWindow::nextBuffer() {
    offset+=256;
    if(offset<end) {
        //if(bootloader) {
        //    sendRawData();
        //} else {
            sendData();
        //}
    } else {
        status("Programming device completed successfully.");
        //if(bootloader) {
        //    text.sprintf("Remember to remove %s when you power cycle.",isMetis?"JP1":"J12");
        //    status(text);
        //} else {
            ui->discoverComboBox->clear();
            bd.clear();
            text.sprintf("Please wait for %s to restart.",isMetis?"Metis":"Hermes or Angelia");
            status(text);
            status("If using DHCP this can take up to 5 seconds.");
            status("To use other functions you will need to run Discovery again.");
        //}
        idle();
        QApplication::restoreOverrideCursor();
    }
}

// SLOT - called when a raw packet read times out (especially when erasing)
void MainWindow::timeout() {
    qDebug()<<"MainWindow::timeout state="<<state;
    switch(state) {
    case IDLE:
        // ignore
        break;
    case ERASING:
    case ERASING_ONLY:
        //if(bootloader) {
        //    eraseTimeouts++;
           //qDebug()<<"eraseTimeouts="<<eraseTimeouts;
       //     if(eraseTimeouts==MAX_ERASE_TIMEOUTS) {
       //         status("Error: erase timeout.");
       //         text.sprintf("Have you set the jumper at %s on %s and power cycled?",isMetis?"JP1":"J12",isMetis?"Metis":"Hermes or Angelia");
       //         status(text);
       //         idle();
       //         QApplication::restoreOverrideCursor();
       //     }
       // } else {
            status("Error: erase timeout.");
            status("Power cycle and try again.");
            idle();
            QApplication::restoreOverrideCursor();
        //}
        break;
    case PROGRAMMING:
        qDebug()<<"timeout";
        break;
    case READ_MAC:
        status("Error: timeout reading MAC address!");
        status("Check that the correct interface is selected.");
        text.sprintf("Check that there is a jumper at %s on %s.",isMetis?"JP1":"J12",isMetis?"Metis":"Hermes or Angelia");
        status(text);
        idle();
        break;
    case READ_IP:
        status("Error: timeout reading IP address!");
        status("Check that the correct interface is selected.");
        text.sprintf("Check that there is a jumper at %s on %s.",isMetis?"JP1":"J12",isMetis?"Metis":"Hermes or Angelia");
        status(text);
        idle();
        break;
    case WRITE_IP:
        // should not happen as there is no repsonse
        break;
    case JTAG_INTERROGATE:
        status("Error: timeout reading interrogating JTAG chain!");
        status("Check that the correct interface is selected.");
        text.sprintf("Check that there is a jumper at %s on %s.",isMetis?"JP1":"J12",isMetis?"Metis":"Hermes or Angelia");
        status(text);
        idle();
        break;
    case JTAG_PROGRAM:
        //qDebug() << "timeout while state is JTAG_PROGRAM";
        break;
    case FLASH_ERASING:
        eraseTimeouts++;
        if(eraseTimeouts==MAX_ERASE_TIMEOUTS) {
            status("Error: erase timeout - power cycle and try again?");
            idle();
            QApplication::restoreOverrideCursor();
        }
        break;
    case FLASH_PROGRAM:
        qDebug() << "timeout while state is FLASH_PROGRAM";
        break;
    }
}

// private function to set state to idle
void MainWindow::idle() {
    qDebug()<<"idle";
    state=IDLE;
    //if(rawReceiveThread!=NULL) {
    //    rawReceiveThread->stop();
    //    rawReceiveThread=NULL;
    //}
    if(receiveThread!=NULL) {
        receiveThread->stop();
        receiveThread=NULL;
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
        //if(bootloader) {
        //    sendRawData();
        //} else {
            sendData();
        //}
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
        text.sprintf("%s IP address written successfully",isMetis?"Metis":"Hermes or Angelia");
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

void MainWindow::loadFlash() {
    qDebug()<<"MainWindow::loadFlash";
    if(loadRBF(ui->fileLineEdit->text())!=0) {
        status("Error: Failed to load Flash Program file.");
        return;
    }
    data_command=PROGRAM_MERCURY;
    state=FLASH_PROGRAM;
    offset=start;
    sendJTAGFlashData();
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
