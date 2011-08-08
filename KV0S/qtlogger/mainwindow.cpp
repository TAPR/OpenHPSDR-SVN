/** 
* \file 
* \brief 
* \author David R. Larsen 
* \version 1.0
* \date 
*/



/* Copyright (C) 2010 - David R. Larsen 
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
#include <QDebug>
#include <QFile>



MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    this->setWindowTitle("QtLogger");
    version = "1.0.1";
    About * ab = new About();

    model = new QStandardItemModel();

    readDefinitions("../qtlogger/adif_codes_2.2.7.xml");

    dta = new loadData();
    save = new saveData();
    save->setModel( model );

    udpSocket = new QUdpSocket(this);

    udpSocket->bind( 45454, QUdpSocket::ShareAddress );



    connect(udpSocket, SIGNAL(readyRead()), this, SLOT(processPendingDatagrams()));
    connect(ui->actionQuit,SIGNAL(triggered()),this,SLOT(close()));
    connect(ui->actionAbout,SIGNAL(triggered()),ab,SLOT(aboutMessage()));
    connect(ui->actionOpen,SIGNAL(triggered()),this,SLOT(readFile()));
    connect(ui->actionSave_As,SIGNAL(triggered()),save,SLOT(writeData()));
    connect(ui->actionAdd,SIGNAL(triggered()),this,SLOT(addItem()));
}

MainWindow::~MainWindow()
{
    delete ui;
    delete dta;
}

void MainWindow::update()
{
    qDebug() << "in update()";
    //qDebug() << dta->getHeaders();

    model->setHorizontalHeaderLabels(dta->getHeaders());
    ui->tableView->setModel(model);
    //ui->tableView->hideColumn(2);
    ui->tableView->resizeRowsToContents();
    ui->tableView->resizeColumnsToContents();

}

void MainWindow::readFile()
{
    type = dta->readData();
    ui->statusBar->showMessage( QString("Loading log file from \"%1\"").arg(dta->getFilename()) );
    if( *type == "XML" )
    {
      dta->readXMLData( model );
    }else{
      dta->readADIFData( model );
    }
    update();
    save->setRows(dta->getRows());
    save->setColumns(dta->getColumns());
}

void MainWindow::newFile()
{
    dta->readData();
    dta->readADIFData( model );
    update();
    save->setRows(dta->getRows());
    save->setColumns(dta->getColumns());
}

void MainWindow::addItem()
{
    add = new addDialog();
    add->setHeader( &(dta->getHeaders() ));
    add->setModel( model );
    add->loadmodeComboBox(modes);
    add->loadcountryComboBox(country);

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
  //qDebug() << handler->mode;
  //qDebug() << handler->ARRLSections["MO"];
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
                 QString data = parseDatagrams(output);
                 qDebug() << data;
                 add->setFrequency(&data);
              }else if( output.contains("mode")){
                 QString data = parseDatagrams(output);
                 qDebug() << data;
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

QString MainWindow::getVersion()
{
    return version;
}
