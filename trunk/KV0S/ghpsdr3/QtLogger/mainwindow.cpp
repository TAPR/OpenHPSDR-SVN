/**
* \file mainwindow.cpp
* \brief Code files for the MainWindow functions as part of the QtLogger program
* \author David R. Larsen, KV0S
* \version 1.0.2
* \date August 21, 2011
*/



/* Copyright (C) 2011 - David R. Larsen, KV0S
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
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    this->setWindowTitle( tr("QT Logger") );

    version = "1.0.2";

    About *about = new About();
    about->setVersion( version );

    Help *help = new Help();

    data = new Data();
    data->setMinimumHeader();

    add = new addDialog();
    readDefinitions("../QtLogger/adif_codes_2.2.7.xml");

    add->loadmodeComboBox( modes );
    add->loadcountryComboBox( country );
    add->loadsubdivisionsComboBox( subdivisions );
    add->loadbandsData( bands );

    udpSocket = new QUdpSocket(this);
    udpSocket->bind( 45454, QUdpSocket::ShareAddress );

    ui->tableView->verticalScrollBar()->setValue(ui->tableView->verticalScrollBar()->maximum());

    connect(udpSocket, SIGNAL(readyRead()), this, SLOT(processPendingDatagrams()));
    connect(ui->actionQuit,SIGNAL(triggered()),this,SLOT(closeDown()));
    connect(ui->actionAbout,SIGNAL(triggered()),about,SLOT(aboutMessage()));
    connect(ui->actionHelp,SIGNAL(triggered()),help,SLOT(show()));
    connect(ui->actionOpen,SIGNAL(triggered()),data,SLOT(readData()));
    connect(ui->actionOpen,SIGNAL(triggered()),this,SLOT(updateStatus()));
    connect(ui->actionSave,SIGNAL(triggered()),this,SLOT(writeData()));
    connect(ui->actionAdd,SIGNAL(triggered()),add,SLOT(show()));
    connect(add,SIGNAL(newdata()),this,SLOT(update()));
    connect(data,SIGNAL(refresh()),this,SLOT(update()));
}


MainWindow::~MainWindow()
{
    delete ui;
    data->close();
}

void MainWindow::closeDown()
{
    writeData();
    close();
}

void MainWindow::updateStatus()
{
    ui->statusBar->showMessage(QString("Reading: \"%1\"").arg(data->getFilename()));
}

//void MainWindow::sortDateTime()
//{
//    ui->tableView->sortByColumn(3);
//}

void MainWindow::update()
{
    QStringList hdr = data->getHeaders();
    data->model->setHorizontalHeaderLabels(hdr);
    ui->tableView->setModel(data->model);
    ui->tableView->resizeRowsToContents();
    ui->tableView->resizeColumnsToContents();
    int idx = data->model->rowCount();
    if( ! (add->record.isEmpty()) )
    {
        //qDebug() << add->record["CONTACT"] << add->record["CALL"] << add->record["DATE"] << add->record["TIME"];
        QStandardItem *it1 = new QStandardItem( add->record["CALL"] );
        data->model->setItem(idx, hdr.indexOf("CALL"), it1 );
        QStandardItem *it2 = new QStandardItem( (add->record["DATE"]) );
        data->model->setItem(idx, hdr.indexOf("QSO_DATE"), it2 );
        QStandardItem *it3 = new QStandardItem( add->record["TIME"] );
        data->model->setItem(idx, hdr.indexOf("TIME_ON"), it3 );
        QStandardItem *it4 = new QStandardItem( add->record["FREQ"] );
        data->model->setItem(idx, hdr.indexOf("FREQ"), it4 );
        QStandardItem *it5 = new QStandardItem( add->record["TX_RST"] );
        data->model->setItem(idx, hdr.indexOf("TX_RST"), it5 );
        QStandardItem *it6 = new QStandardItem( add->record["RX_RST"] );
        data->model->setItem(idx, hdr.indexOf("RX_RST"), it6 );
        QStandardItem *it7 = new QStandardItem( add->record["NAME"] );
        data->model->setItem(idx, hdr.indexOf("NAME"), it7 );
        QStandardItem *it8 = new QStandardItem( add->record["QTH"] );
        data->model->setItem(idx, hdr.indexOf("QTH"), it8 );
        QStandardItem *it9 = new QStandardItem( add->record["MODE"] );
        data->model->setItem(idx, hdr.indexOf("MODE"), it9 );
        QStandardItem *it10 = new QStandardItem( add->record["COUNTRY"] );
        data->model->setItem(idx, hdr.indexOf("COUNTRY"), it10 );
        QStandardItem *it11 = new QStandardItem( add->record["STATE"] );
        data->model->setItem(idx, hdr.indexOf("STATE"), it11 );
        QStandardItem *it12 = new QStandardItem( add->record["EVENT"] );
        data->model->setItem(idx, hdr.indexOf("EVENT"), it12 );
        QStandardItem *it13 = new QStandardItem( add->record["CHECK"] );
        data->model->setItem(idx, hdr.indexOf("CHECK"), it13 );
        QStandardItem *it = new QStandardItem( add->record["CONTACT"] );
        data->model->setItem(idx, hdr.indexOf("CONTACT"), it );
        QStandardItem *it14 = new QStandardItem( add->record["BAND"] );
        data->model->setItem(idx, hdr.indexOf("BAND"), it14 );
    }
    ui->tableView->verticalScrollBar()->setValue(ui->tableView->verticalScrollBar()->maximum());
    add->callreset();
}



void MainWindow::writeData()
{
    QString filename =  QFileDialog::getSaveFileName(this, tr("Save File"),"",tr("Files (*.xml *.adif *.adi)"));
    data->setFilename( filename );


    if( filename.contains("xml"))
    {
        ui->statusBar->showMessage(QString("Writing: \"%1\"").arg(filename));
        data->writeXmlData( filename );
    }else{
        ui->statusBar->showMessage(QString("Writing: \"%1\"").arg(filename));
        data->writeADIFData( filename );
    }

}

void MainWindow::processPendingDatagrams()
{
      while (udpSocket->hasPendingDatagrams()) {
          QByteArray datagram;
          datagram.resize(udpSocket->pendingDatagramSize());
          udpSocket->readDatagram(datagram.data(), datagram.size());
          qDebug() << QString("%1").arg(datagram.data());
          if( add != 0 )
          {
              QString output = QString("%1").arg(datagram.data());
              if( output.contains("frequency")){
                 QString datastr = parseDatagrams(output);
                 qDebug() << datastr;
                 add->setFrequency( datastr );
              }else if( output.contains("mode")){
                 QString datastr = parseDatagrams(output);
                 qDebug() << datastr;
                 add->setMode( datastr );
                 data->modestr = datastr;
              }
          }

      }
}

QString MainWindow::parseDatagrams( QString data )
{
    QStringList output;
    output = data.split(' ');
    return output[1];
}

void MainWindow::readDefinitions( QString filename )
{
  ui->statusBar->showMessage( QString("Loading ADIF definitions from \"%1\"").arg(filename) );

  QFile *file = new QFile(filename);
  if( !file->open(QFile::ReadOnly) )
  {
      qDebug() << "Open failed." << endl;
  }else{
      qDebug() << "Open success." << endl;
  }
  QXmlSimpleReader xmlReader;
  QXmlInputSource *source = new QXmlInputSource( file );

  AdifHandler *handler = new AdifHandler();
  xmlReader.setContentHandler(handler);
  xmlReader.setErrorHandler(handler);

  bool ok = xmlReader.parse(source);

  if( !ok )
  {
      qDebug() << "Parsing failed." << endl;

  }
  modes = new QStringList( handler->mode );
  country = new QStringList( handler->country );
  subdivisions = new QStringList( handler->subdivisions );
  bands = QHash <QString, Range>( handler->band );
  //qDebug() << bands["160m"].lower;
}

