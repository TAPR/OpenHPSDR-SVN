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

    receiveThread=NULL;
    rawReceiveThread=NULL;
    discovery=NULL;

    deviceIndicator->setPixmap (QPixmap(":/icons/red16.png"));
    deviceIndicator->setToolTip (QString ("Device port not open"));

    statusBar ()->addPermanentWidget (deviceIndicator);

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
    connect(ui->AngeliaButton,SIGNAL(clicked()),this,SLOT(hermesSelected()));
    connect(ui->MetisButton,SIGNAL(clicked()),this,SLOT(metisSelected()));
    //connect(ui->browseButton,SIGNAL(clicked()),this,SLOT(browse()));

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
    ui->statusListWidget->insertItem(ui->statusListWidget->count()-1,text);
    ui->statusListWidget->setCurrentRow(ui->statusListWidget->count()-1);
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
}

void MainWindow::hermesSelected() {
    isMetis=false;
}


// SLOT - browse - called when the "Browse ..." button on the Program tab is pressed.
//void MainWindow::browse() {
    //QString fileName=QFileDialog::getOpenFileName(this,tr("Select File"),"",tr("pof Files (*.pof)"));
//    QString fileName=QFileDialog::getOpenFileName(this,tr("Select File"),"",tr("rbf Files (*.rbf)"));
//    ui->fileLineEdit->setText(fileName);
//}
