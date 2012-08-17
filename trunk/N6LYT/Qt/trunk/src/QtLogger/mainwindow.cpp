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
    logfileIndicator (new QLabel ( this)),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    this->setWindowTitle( tr("QT Logger") );

    version = "1.1.2";


    this->setMouseTracking(true);
    this->setBackgroundRole(QPalette::Base);

    QCoreApplication::setOrganizationName("openhpsdr");
    QCoreApplication::setOrganizationDomain("openhpsdr.org");
    QCoreApplication::setApplicationName("QtLogger");

    //logfileIndicator->setStyleSheet (QString ("QLabel {color:red}"));
    logfileIndicator->setPixmap(QPixmap(":/icons/filefoldergrey16.svg"));
    logfileIndicator->setToolTip(QString("Logfile: No file")  );

    About *about = new About();
    about->setVersion( version );


    Help *help = new Help();

    data = new Data();
    data->setMinimumHeader();

    add = new addDialog( this );


    readDefinitions(":/xml/adif_codes_2.2.7.xml");


    add->loadmodeComboBox( modes );
    add->loadcountryComboBox( country );
    add->loadsubdivisionsComboBox( subdivisions );
    add->loadbandsData( bands );

    last = new lastContact( this );

    pref = new Preferences( this );
    pref->setPreferences( QString("radio"), settings.value("preference_radio").toString() );
    pref->setPreferences( QString("location"), settings.value("preference_location").toString() );
    pref->setPreferences( QString("event"), settings.value("preference_event").toString() );
    pref->setPreferences( QString("owner"), settings.value("preference_owner").toString() );
    pref->setPreferences( QString("qsl"), settings.value("preference_qsl").toString() );
    pref->setPreferences( QString("callfilter"), settings.value("callsign_filter").toString() );
    add->callsign_filter = pref->getPreferences("callfilter");


    udpSocket = new QUdpSocket(this);
    udpSocket->bind( 11500, QUdpSocket::ShareAddress );

    statusBar ()->addPermanentWidget (logfileIndicator);

    ui->tableView->verticalScrollBar()->setValue(ui->tableView->verticalScrollBar()->maximum());

    deleteFlag = false;

    connect(udpSocket, SIGNAL(readyRead()), this, SLOT(processPendingDatagrams()));
    connect(ui->actionQuit,SIGNAL(triggered()),this,SLOT(closeDown()));
    connect(ui->actionAbout,SIGNAL(triggered()),about,SLOT(aboutMessage()));
    connect(ui->actionHelp,SIGNAL(triggered()),help,SLOT(show()));
    connect(ui->actionPreferences,SIGNAL(triggered()),pref,SLOT(show()));
    connect(pref,SIGNAL(preferencesChanged()),this,SLOT(updateView()));
    connect(ui->actionOpen,SIGNAL(triggered()),data,SLOT(readData()));
    connect(ui->actionLast_Contact,SIGNAL(triggered()),last,SLOT(show()));
    connect(ui->actionOpen,SIGNAL(triggered()),this,SLOT(updateStatus()));
    connect(ui->actionSave,SIGNAL(triggered()),this,SLOT(writeData()));
    connect(ui->actionSupport_Directory,SIGNAL(triggered()),this,SLOT(setSupportDirectory()));
    connect(ui->actionLog_Directory,SIGNAL(triggered()),this,SLOT(setLogDirectory()));
    connect(ui->tableView,SIGNAL(clicked(QModelIndex)),this,SLOT(removeTableRow(QModelIndex)));
    connect(ui->tableView,SIGNAL(activated(QModelIndex)),ui->tableView,SLOT(resizeRowsToContents()));
    connect(ui->actionAdd,SIGNAL(triggered()),add,SLOT(show()));
    connect(add,SIGNAL(newdata()),this,SLOT(update()));
    connect(data,SIGNAL(refresh()),this,SLOT(update()));
    connect(ui->actionDelete,SIGNAL(toggled(bool)),this,SLOT(setDeleteFlag(bool)));
    connect(add,SIGNAL(ownerChanged()),this,SLOT(updateOwner()));
    connect(add,SIGNAL(prefixChanged(QString)),last->proxyModel,SLOT(setFilterRegExp(QString)));
    connect(add,SIGNAL(prefixChanged(QString)),last,SLOT(setCallLabel(QString)));
    connect(add,SIGNAL(prefixChanged(QString)),add->proxyModel,SLOT(setFilterRegExp(QString)));
}


MainWindow::~MainWindow()
{
    delete ui;
    data->close();
    add->close();
}

void MainWindow::closeDown()
{
    add->close();
    writeData();
    close();
}

void MainWindow::updateOwner()
{
    qDebug() << add->record["OPERATOR"] << data->operatorstr;
    setOwner(add->record["OPERATOR"]);
    setQTH(add->record["HOME_QTH"]);
    setGrid(add->record["HOME_GRID"]);
    setStation(add->record["STATION_CALL"]);
}

void MainWindow::updateStatus()
{
    ui->statusBar->showMessage(QString("Current Log: \"%1\"").arg(data->getFilename()));
    logfileIndicator->setPixmap(QPixmap(":/icons/filefolder16.svg"));
    //logfileIndicator->setStyleSheet (QString ("QLabel {color:green}"));
    logfileIndicator->setToolTip(QString("Logfile:" + data->getFilename()) );
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
    last->setModel( data->model );
    ui->tableView->resizeRowsToContents();
    ui->tableView->resizeColumnsToContents();
    int idx = data->model->rowCount();
    if( ! (add->record.isEmpty()) )
    {
        //qDebug() << add->record["CONTACT"] << add->record["CALL"] << add->record["DATE"] << add->record["TIME"];
        QStandardItem *it1 = new QStandardItem( add->record["CALL"] );
        data->model->setItem(idx, hdr.indexOf("CALL"), it1 );
        QStandardItem *it7 = new QStandardItem( add->record["NAME"] );
        data->model->setItem(idx, hdr.indexOf("NAME"), it7 );
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
        QStandardItem *it15 = new QStandardItem( add->record["COMMENT"] );
        data->model->setItem(idx, hdr.indexOf("COMMENT"), it15 );
        QStandardItem *it16 = new QStandardItem( add->record["OPERATOR"] );
        data->model->setItem(idx, hdr.indexOf("OPERATOR"), it16 );
        QStandardItem *it17 = new QStandardItem( add->record["HOME_QTH"] );
        data->model->setItem(idx, hdr.indexOf("HOME_QTH"), it17 );
        QStandardItem *it18 = new QStandardItem( add->record["HOME_GRID"] );
        data->model->setItem(idx, hdr.indexOf("HOME_GRID"), it18 );
        QStandardItem *it19 = new QStandardItem( add->record["STATION_CALL"] );
        data->model->setItem(idx, hdr.indexOf("STATION_CALL"), it19 );
        QStandardItem *it20 = new QStandardItem( add->record["QSL_SENT"] );
        data->model->setItem(idx, hdr.indexOf("QSL_SENT"), it20 );
        QStandardItem *it21 = new QStandardItem( add->record["QSL_SENT_VIA"] );
        data->model->setItem(idx, hdr.indexOf("QSL_SENT_VIA"), it21 );
        QStandardItem *it22 = new QStandardItem( add->record["QSL_RCVD"] );
        data->model->setItem(idx, hdr.indexOf("QSL_RCVD"), it22 );
        QStandardItem *it23 = new QStandardItem( add->record["QSL_RCVD_VIA"] );
        data->model->setItem(idx, hdr.indexOf("QSL_RCVD_VIA"), it23 );
        QStandardItem *it24 = new QStandardItem( add->record["GRIDSQUARE"] );
        data->model->setItem(idx, hdr.indexOf("GRIDSQUARE"), it24 );
        QStandardItem *it25 = new QStandardItem( add->record["CNTY"] );
        data->model->setItem(idx, hdr.indexOf("CNTY"), it25 );
    }
    ui->tableView->verticalScrollBar()->setValue(ui->tableView->verticalScrollBar()->maximum());
    add->callreset();
}

void MainWindow::updateView()
{
    QStringList hdr = data->getHeaders();
    qDebug() << "in updateView";
    add->callreset();
    for ( int i = 0; i<=hdr.length(); i++ )
    {
        ui->tableView->hideColumn(i);
    }
    ui->tableView->showColumn(hdr.indexOf("CALL"));
    ui->tableView->showColumn(hdr.indexOf("NAME"));
    ui->tableView->showColumn(hdr.indexOf("DATE"));
    ui->tableView->showColumn(hdr.indexOf("TIME"));
    if( pref->getPreferences("radio"))
    {
        settings.setValue("preference_radio", true );
        ui->tableView->showColumn(hdr.indexOf("FREQ"));
        ui->tableView->showColumn(hdr.indexOf("MODE"));
        ui->tableView->showColumn(hdr.indexOf("BAND"));
        ui->tableView->showColumn(hdr.indexOf("TX_RST"));
        ui->tableView->showColumn(hdr.indexOf("RX_RST"));
    }else{
        settings.setValue("preference_radio", false );
    }

    if( pref->getPreferences("location"))
    {
        settings.setValue("preference_location", true );
        ui->tableView->showColumn(hdr.indexOf("COUNTRY"));
        ui->tableView->showColumn(hdr.indexOf("NAME"));
        ui->tableView->showColumn(hdr.indexOf("QTH"));
        ui->tableView->showColumn(hdr.indexOf("STATE"));
        ui->tableView->showColumn(hdr.indexOf("COUNTY"));
        ui->tableView->showColumn(hdr.indexOf("GRID"));
    }else{
        settings.setValue("preference_location", false );
    }

    if( pref->getPreferences("event"))
    {
        settings.setValue("preference_event", true );
        ui->tableView->showColumn(hdr.indexOf("EVENT"));
        ui->tableView->showColumn(hdr.indexOf("CONTACT"));
        ui->tableView->showColumn(hdr.indexOf("CHECK"));
        ui->tableView->showColumn(hdr.indexOf("COMMENT"));
    }else{
        settings.setValue("preference_event", false );
    }

    if( pref->getPreferences("owner"))
    {
        settings.setValue("preference_owner", true );
        ui->tableView->showColumn(hdr.indexOf("OPERATOR"));
        ui->tableView->showColumn(hdr.indexOf("HOME_QTH"));
        ui->tableView->showColumn(hdr.indexOf("HOME_GRID"));
        ui->tableView->showColumn(hdr.indexOf("STATION_CALL"));
    }else{
        settings.setValue("preference_owner", false );
    }

    if( pref->getPreferences("qsl"))
    {
        settings.setValue("preference_qsl", true );
        ui->tableView->showColumn(hdr.indexOf("QSL_SENT"));
        ui->tableView->showColumn(hdr.indexOf("QSL_SENT_VIA"));
        ui->tableView->showColumn(hdr.indexOf("QSL_RCVD"));
        ui->tableView->showColumn(hdr.indexOf("QSL_RCVD_VIA"));
    }else{
        settings.setValue("preference_qsl", false );
    }

    ui->tableView->resizeRowsToContents();
    ui->tableView->resizeColumnsToContents();

}

void MainWindow::writeData()
{
    QString dname = settings.value("LDirectory").toString();
    QString filename =  QFileDialog::getSaveFileName(this, tr("Save File"),dname,tr("Files (*.xml *.adif *.adi)"));
    data->setFilename( filename );
    settings.setValue("filename", filename);


    if( filename.contains("xml"))
    {
        ui->statusBar->showMessage(QString("Writing: \"%1\"").arg(filename));
        data->writeXmlData( filename );
    }else{
        ui->statusBar->showMessage(QString("Writing: \"%1\"").arg(filename));
        data->writeADIFData( filename );
    }
    updateStatus();

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
                 qDebug() << (datastr.toDouble())/1000000.0;
                 add->setFrequency( QString("%1").arg((datastr.toDouble()/1000000.0),0,'f',6) );
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
      qDebug() << "Open Definition file failed." << endl;
  }else{
      qDebug() << "Open Definition file success." << endl;
  }
  QXmlSimpleReader xmlReader;
  QXmlInputSource *source = new QXmlInputSource( file );

  AdifHandler *handler = new AdifHandler();
  xmlReader.setContentHandler(handler);
  xmlReader.setErrorHandler(handler);

  bool ok = xmlReader.parse(source);

  if( !ok )
  {
      qDebug() << "Parsing Definition file failed." << endl;
  }

  ui->statusBar->showMessage( QString("Loading Country definitions from \"%1\"").arg("DXCCcountries.xml") );

  modes = new QStringList( handler->mode );
  country = new QStringList( );
  subdivisions = new QStringList( );
  bands = QHash <QString, Range>( handler->band );
  //qDebug() << bands["160m"].lower;
}

void MainWindow::setDeleteFlag( bool flg )
{
    deleteFlag = flg;

    QString deleteStatus;
    if( deleteFlag == true )
    {
        deleteStatus = "ON";
    }else{
        deleteStatus = "OFF";
    }
    QString message = QString("<h2>Delete Mode is now %1!</h2>\n").arg(deleteStatus);
            message.append( "When the delete mode is <b>ON</b>");
            message.append(" any record that you click on will be deleted!<br/><br/>");
            message.append( "When the delete mode is <b>OFF</b>");
            message.append(" any item that you click on can be edited!");
    QMessageBox::warning(this,"Delete Mode",message );
}

void MainWindow::removeTableRow(QModelIndex idx)
{

    qDebug() <<  idx.row() << deleteFlag;
    if( deleteFlag == true){
        data->model->removeRows( idx.row(), 1, QModelIndex() );
    }
}


void MainWindow::setOwner(QString operatorstrn)
{
    data->operatorstr = QString(operatorstrn);
}

void MainWindow::setQTH(QString qthstrn)
{
    data->qthstr =  QString(qthstrn);
}

void MainWindow::setGrid(QString gridstrn)
{
    data->gridstr =  QString(gridstrn);
}

void MainWindow::setStation(QString stationstrn)
{
    data->stationstr = QString(stationstrn);
}

void MainWindow::setLogDirectory()
{
    QString dname = settings.value("LDirectory").toString();
    QString dirname =  QFileDialog::getExistingDirectory(this, tr("Log Directory"),dname);
    data->setDirname( dirname );
    settings.setValue("LDirectory", dirname);
    ui->statusBar->showMessage( QString("Log Directory set to \"%1\"").arg(dirname) );
}

void MainWindow::setSupportDirectory()
{
    QString dname = settings.value("SDdirectory").toString();
    QString dirname =  QFileDialog::getExistingDirectory(this, tr("Support Directory"),dname);
    data->setDirname( dirname );
    settings.setValue("SDirectory", dirname);
    ui->actionSupport_Directory->setToolTip(QString("Support Directory %1").arg(dirname));
    ui->statusBar->showMessage( QString("Support Directory set to \"%1\"").arg(dirname) );
}


