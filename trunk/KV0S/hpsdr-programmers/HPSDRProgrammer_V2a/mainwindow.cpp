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

    ab = new AboutDialog(this);
    ab->setVersion( QString(VERSION), QString(RELEASE) );

    stat = new StatusDialog(this);
    add = new AddressDialog(this);

    QCoreApplication::setOrganizationName("HPSDR");
    QCoreApplication::setOrganizationDomain("openhpsdr.org");
    QCoreApplication::setApplicationName("HPSDRProgrammer_V2");

    //settings.setValue("dir", "");



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

    //connect(ui->programButton,SIGNAL(clicked()),this,SLOT(program()));
    connect(ui->browseButton,SIGNAL(clicked()),this,SLOT(browse()));

    connect(ui->actionStatus,SIGNAL(triggered()),stat,SLOT(show()));
    connect(ui->actionAddress,SIGNAL(triggered()),add,SLOT(show()));


    //connect(add,SIGNAL(writeIP()),this,SLOT(setIP_UDP()));



    if(ui->interfaceComboBox->count()>0) {
       ui->interfaceComboBox->setCurrentIndex(0);
       interfaceSelected(0);
    } else {
       // dont allow discovery if no interface found
       ui->discoverButton->setEnabled(false);
    }


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
}


void MainWindow::browse()
{
    qDebug() << "in MainWindow::browse";
}
