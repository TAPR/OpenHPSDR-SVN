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
* 2012, 2013 - Dave Larsen, KV0S
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

    ui->setupUi(this);

    this->setWindowTitle(QString("HPSDRProgrammer V2 %1").arg(QString("%0 %1").arg(VERSION).arg(RELEASE)));

    interfaces = Interfaces();
    eraseTimeouts = 0;

    ab = new AboutDialog(this);
    ab->setVersion( QString(VERSION), QString(RELEASE) );

    stat = new StatusDialog(this);
    add = new AddressDialog(this);

    socket = new QUdpSocket(this);

    QCoreApplication::setOrganizationName("HPSDR");
    QCoreApplication::setOrganizationDomain("openhpsdr.org");
    QCoreApplication::setApplicationName("HPSDRProgrammer_V2a");


    deviceIndicator->setIndent(0);
    deviceIndicator->setPixmap (QPixmap(":/icons/red16.png"));
    deviceIndicator->setToolTip (QString ("Device port not open"));

    statusBar()->addPermanentWidget (deviceIndicator);


    for (int i = 0; i < interfaces.count(); ++i)
    {
        ui->interfaceComboBox->addItem(interfaces.getInterfaceNameAt(i));
    }


    connect(ui->actionAbout,SIGNAL(triggered()),ab,SLOT(show()));
    connect(ui->actionQuit,SIGNAL(triggered()),this,SLOT(close()));
    connect(ui->interfaceComboBox,SIGNAL(currentIndexChanged(int)),this,SLOT(interfaceSelected(int)));
    //connect(ui->discoverComboBox,SIGNAL(currentIndexChanged(int)),this,SLOT(metisSelected(int)));

    connect(ui->discoverButton,SIGNAL(clicked()),this,SLOT(discover()));

    connect(ui->programButton,SIGNAL(clicked()),this,SLOT(program()));
    connect(ui->browseButton,SIGNAL(clicked()),this,SLOT(browse()));

    connect(ui->actionStatus,SIGNAL(triggered()),stat,SLOT(show()));
    connect(ui->actionAddress,SIGNAL(triggered()),add,SLOT(show()));


    //connect(wb,SIGNAL(eraseCompleted()),this,SLOT(eraseCompleted()));
    //connect(wb,SIGNAL(nextBuffer()),this,SLOT(nextBuffer()));
    //connect(wb,SIGNAL(timeout()),this,SLOT(timeout()));

    //connect(add,SIGNAL(writeIP()),this,SLOT(setIP_UDP()));



    if(ui->interfaceComboBox->count()>0) {
       ui->interfaceComboBox->setCurrentIndex(0);
       interfaceSelected(0);
    } else {
       // dont allow discovery if no interface found
       ui->discoverButton->setEnabled(false);
    }

    wb = new WriteBoard( socket, (unsigned char*)interfaces.getInterfaceHardwareAddress(ui->interfaceComboBox->currentIndex()).toStdString().c_str());


    connect(wb,SIGNAL(discoveryBoxUpdate()),this,SLOT(discoveryUpdate()));
}

MainWindow::~MainWindow()
{
    delete ui;
}


void MainWindow::interfaceSelected(int id)
{
    qDebug() << "in InterfaceSelected";
    ui->IPInterfaceLabel->setText( interfaces.getInterfaceIPAddress(interfaces.getInterfaceNameAt(id)) );
    ui->MACInterfaceLabel->setText( interfaces.getInterfaceHardwareAddress(id) );
}

// private function to display message in the status window
void MainWindow::status(QString text) {
    qDebug()<<"status:"<<text;
    ui->statusBar->showMessage( text );
    stat->status( text.trimmed() );
}

void MainWindow::discover()
{
    qDebug() << "in MainWindow::discover";
    wb->discovery();
    qDebug() << "in MainWindow::discover after broadcast";
}

void MainWindow::discoveryUpdate()
{
    qDebug() << "in MainWindow::discoverUpdate";
    qDebug() << wb->boards.keys();
    ui->discoverComboBox->addItems( QStringList(wb->boards.keys()) );
    wb->currentboard = ui->discoverComboBox->currentText();
    //add->getIPaddress(wb->boards[wb->currentboard]);
}

void MainWindow::browse()
{
    qDebug() << "in MainWindow::browse";
    QString dir = "";
    dir = settings.value("dir").toString();
    //qDebug() << "dir1" << dir;
    fileName = QFileDialog::getOpenFileName(this, tr("Open RBF"), dir, tr("RBF Files (*.rbf)"));
    ui->fileLineEdit->setText( fileName );
    QFileInfo fi( fileName );
    QDir dr = fi.dir();
    dir = dr.path();
    //qDebug() << "dir2" << dir;
    settings.setValue("dir", QVariant(dir) );

}

void MainWindow::program()
{
    qDebug() << "in MainWindow::program";
    // read RBF
    wb->loadRBF( fileName );
    // erase device
    wb->eraseData(wb->boards[wb->currentboard]);
    // program device
}

void MainWindow::setIP_UDP()
{
    qDebug() << "in MainWindow::setIP_UDP";
}

void MainWindow::timeout() {
    QString text;
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
        text=QString("Check that there is a jumper");
        status(text);
        idle();
        break;
    case READ_IP:
        status("Error: timeout reading IP address!");
        status("Check that the correct interface is selected.");
        text=QString("Check that there is a jumper");
        status(text);
        idle();
        break;
    case WRITE_IP:
        // should not happen as there is no repsonse
        break;
    case JTAG_INTERROGATE:
        status("Error: timeout reading interrogating JTAG chain!");
        status("Check that the correct interface is selected.");
        text=QString("Check that there is a jumper" );
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
}

// SLOT - eraseCompleted
void MainWindow::eraseCompleted() {
    QString text;
    switch(state) {
    case IDLE:
        qDebug()<<"received eraseCompleted when state is IDLE";
        break;
    case ERASING:
        status("Device erased successfully");
        state=PROGRAMMING;
        offset=start;
        status("Programming device ...");
        wb->sendData();
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
        text=QString("IP address written successfully");
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
        //loadFlash();
        break;
    case FLASH_PROGRAM:
        qDebug()<<"received eraseCompleted when state is FLASH_PROGRAM";
        break;
    }
}



