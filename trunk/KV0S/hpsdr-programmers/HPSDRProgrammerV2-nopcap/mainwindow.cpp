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

    this->setWindowTitle(QString("HPSDRProgrammer V2 nopcap %1").arg(QString("%0 %1").arg(VERSION).arg(RELEASE)));

    interfaces = Interfaces();
    eraseTimeouts = 0;

    ab = new AboutDialog(this);
    ab->setVersion( QString(VERSION), QString(RELEASE) );

    stat = new StatusDialog(this);
    add = new AddressDialog(this);

    socket = new QUdpSocket(this);

    discoveryDone = false;
    programComplete = false;

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
    connect(ui->discoverComboBox,SIGNAL(currentIndexChanged(int)),this,SLOT(boardSelected(int)));

    connect(ui->discoverButton,SIGNAL(clicked()),this,SLOT(discover()));

    connect(ui->programButton,SIGNAL(clicked()),this,SLOT(program()));
    connect(ui->browseButton,SIGNAL(clicked()),this,SLOT(browse()));

    connect(ui->actionStatus,SIGNAL(triggered()),stat,SLOT(show()));
    connect(ui->actionAddress,SIGNAL(triggered()),add,SLOT(show()));


    connect(stat,SIGNAL(stbar(QString)),this,SLOT(stbar(QString)));


    connect(add,SIGNAL(writeIP()),this,SLOT(setIP_UDP()));
    connect(this,SIGNAL(updateDiscovery()),this,SLOT(discover()));



    if(ui->interfaceComboBox->count()>0) {
       ui->interfaceComboBox->setCurrentIndex(0);
       interfaceSelected(0);
    } else {
       // dont allow discovery if no interface found
       ui->discoverButton->setEnabled(false);
    }


    /* qDebug()<<"interface:"<<server->getInterface();
    index=ui->comboBoxInterface->findText(server->getInterface());
    if(index!=-1) {
        ui->comboBoxInterface->setCurrentIndex(index);
    } else {
        // interface not found

    }

    qDebug()<<"after updateInterfaces interface:"<<server->getInterface();
    if(server->getInterface()!="") {
        server->bind();
        actionDiscover();
        index=ui->comboBoxMetis->findText(server->getMetisDetail());
        if(index!=-1) {
            ui->comboBoxMetis->setCurrentIndex(index);
        } else {
            server->setMetisDetail("");
        }
    } */


    wb = new WriteBoard( socket, stat);

    connect(wb,SIGNAL(discover()),this,SLOT(discover()));
    connect(wb,SIGNAL(discoveryBoxUpdate()),this,SLOT(discoveryUpdate()));
    connect(wb,SIGNAL(eraseCompleted()),this,SLOT(eraseCompleted()));
    connect(wb,SIGNAL(programmingCompleted()),this,SLOT(programmingCompleted()));
    connect(wb,SIGNAL(nextBuffer()),this,SLOT(nextBuffer()));
    connect(wb,SIGNAL(timeout()),this,SLOT(timeout()));
}

MainWindow::~MainWindow()
{
    delete ui;
}


void MainWindow::interfaceSelected(int id)
{
    qDebug( "in InterfaceSelected" );
    qDebug() << interfaces.getInterfaceNameAt(id);
    ui->IPInterfaceLabel->setText( interfaces.getInterfaceIPAddress( interfaces.getInterfaceNameAt(id) ) );
    ui->MACInterfaceLabel->setText( interfaces.getInterfaceHardwareAddress(id) );
    ui->discoverComboBox->clear();
#ifdef WIN32
    socket->close();
    socket->bind();
#else
    socket->close();
    qDebug() << "interface addresses " << interfaces.getInterfaceIPAddress( interfaces.getInterfaceNameAt(id));
    //socket->bind(QHostAddress( interfaces.getInterfaceIPAddress( interfaces.getInterfaceNameAt(id))),1024 );
    if(!socket->bind(QHostAddress(interfaces.getInterfaceIPAddress(interfaces.getInterfaceNameAt(id))),1024,QUdpSocket::ReuseAddressHint)) {
        qDebug()<<"Error: Server::bind bind failed "<<socket->errorString();
    }
    qDebug() << socket->localAddress();
#endif
}

void MainWindow::boardSelected(int id)
{
    qDebug( "in boardSelected" );
    if (id < 0 ){
        id = 0;
    }
    qDebug() << id;
    if( wb->boards.uniqueKeys().count() > 0 )
    {
       qDebug() << (wb->boards.keys())[id];
       wb->currentboard = (wb->boards.keys())[id];
    }
}

// private function to display message in the status window
void MainWindow::status(QString text) {
    //qDebug()<<"status:"<<text;
    ui->statusBar->showMessage( text );
    stat->status( text.trimmed() );
}

void MainWindow::stbar(QString text)
{
   ui->statusBar->showMessage( text );
}


void MainWindow::discover()
{
    qDebug() << "in MainWindow::discover";
    status(tr("Attempting to discover HPSDR boards. (May take upto %0 seconds)").arg(BOARD_DISCOVERY_TIMEOUT/1000));

    ui->discoverComboBox->clear();
    ui->fileLineEdit->clear();
    wb->boards.clear();
    discoveryDone = false;
    programComplete = false;
    wb->discovery();
    qDebug() << "in MainWindow::discover after broadcast";
}

void MainWindow::discoveryUpdate()
{
    QString text;
    qDebug() << "in MainWindow::discoverUpdate";
    qDebug() << wb->boards.uniqueKeys();

    if( discoveryDone == false ){
       if( wb->boards.uniqueKeys().count() > 0 )
       {

          ui->discoverComboBox->addItems( QStringList(wb->boards.uniqueKeys()) );
          wb->currentboard = ui->discoverComboBox->currentText();
          add->getIPaddress(wb->boards[wb->currentboard]);
          if( ui->discoverComboBox->count() < 2 )
          {
             text = QString("%0 board found.").arg(ui->discoverComboBox->count());
          }else{
             text = QString("%0 boards found.").arg(ui->discoverComboBox->count());
          }
          deviceIndicator->setIndent(0);
          deviceIndicator->setPixmap (QPixmap(":/icons/green16.png"));
          deviceIndicator->setToolTip (QString ("Device open %0").arg(wb->boards[wb->currentboard]->toIPString()));
          status( wb->currentboard );
          status( text );
          discoveryDone = true;
        }else{
          stat->status(tr("Discovery has timeout at %0 seconds\n No boards found, you may need to use HPSDRBootloader.").arg(BOARD_DISCOVERY_TIMEOUT/1000));
          QMessageBox::warning(this, tr("HPSDRProgrammer_V2"),
                                      tr("Discovery has failed\n"
                                         "If you have more that one interface, check you are using the correct one.  "
                                         "Check that the board is on and no jumper on J1 or J12.\n"
                                         "You may need to use HPSDRBootloader."),
                                       QMessageBox::Ok);
          return;
       }
    }

}

void MainWindow::browse()
{
    qDebug() << "in MainWindow::browse";
    QString dir = "";
    QString text;
    dir = settings.value("dir").toString();
    //qDebug() << "dir1" << dir;
    fileName = QFileDialog::getOpenFileName(this, tr("Open RBF"), dir, tr("RBF Files (*.rbf)"));
    ui->fileLineEdit->setText( fileName );
    QFileInfo fi( fileName );
    QDir dr = fi.dir();
    dir = dr.path();
    //qDebug() << "dir2" << dir;
    settings.setValue("dir", QVariant(dir) );
    QString fn = fi.baseName();
    bool brdtest= true;
    QString brd = wb->boards[wb->currentboard]->getBoardString();

    if (  brd.contains("metis") && !(fn.contains("metis") || fn.contains("Metis"))  ){
        brdtest = false;
    }else if ( brd.contains("hermes") && !(fn.contains("hermes") || fn.contains("Hermes")) ){
        brdtest = false;
    }else if ( brd.contains("angelia") && !(fn.contains("angelia") || fn.contains("Angelia")) ){
        brdtest = false;
    }else if ( brd.contains("orion") && !(fn.contains("orion") || fn.contains("Orion")) ){
        brdtest = false;
    }

    if ( brdtest == false ){
      stat->status(tr("Discovery shows you have a %0 board\n The filename %1 is not for a metis board.").arg(brd, fi.completeBaseName()));
      QMessageBox::warning(this, tr("HPSDRProgrammer_V2"),
                                        tr("Discovery shows you are loading a <b>%0</b> board! \n <br/><br/>"
                                           "The filename for the RBF file <b>\"%1\"</b> does not seem to match.").arg(brd, fi.completeBaseName()),
                                         QMessageBox::Ok);
      fileName.clear();
      ui->fileLineEdit->clear();
    }else{
      text = QString("Opening file: %0").arg(fileName);
      status( text );
    }
}

void MainWindow::program()
{
    qDebug() << "in MainWindow::program";
    // read RBF
    if (fileName.isEmpty() ){
        QMessageBox::warning(this, tr("HPSDRProgrammer_V2"),
             tr("The RBF file is missing! \n "
             "The filename for the RBF file \"%1\" does not seem exists.").arg( fileName),
                  QMessageBox::Ok);
    }else{
       wb->loadRBF( fileName );
       // erase device
       state=ERASING;
       wb->eraseData(wb->boards[wb->currentboard]);
       // program device
    }
}

void MainWindow::setIP_UDP()
{
    qDebug() << "in MainWindow::setIP_UDP";

    QStringList *saddr = new QStringList();
    add->getNewIPAddress(saddr);
    wb->changeIP(saddr, wb->boards[wb->currentboard]->getMACAddress());

    ui->discoverComboBox->clear();
    stat->status("Rediscovery.");
    QTimer *timer = new QTimer(this);
    timer->setSingleShot(true);
    connect(timer, SIGNAL(timeout()), this, SLOT(discover()));
    timer->start(BOARD_DISCOVERY_TIMEOUT);


}


void MainWindow::timeout() {
    QString text;
    qDebug()<<"MainWindow::timeout state="<<state;
    switch(state) {
    case ERASING:
    case ERASING_ONLY:


        if ( wb->boards[wb->currentboard]->getBoardString() == "metis" ){
            stat->status(tr("Erase has timeout at %0 seconds\nTry again.\n").arg(METIS_MAX_ERASE_TIMEOUTS/1000));
            QMessageBox::warning(this, tr("HPSDRProgrammer_V2"),
                                 tr("Erase has timeout at %0 seconds\n").arg(METIS_MAX_ERASE_TIMEOUTS/1000),
                                 QMessageBox::Retry, QMessageBox::Cancel);
        }else if ( wb->boards[wb->currentboard]->getBoardString() == "hermes" ){
            stat->status(tr("Erase has timeout at %0 seconds\nTry again.\n").arg(HERMES_MAX_ERASE_TIMEOUTS/1000));
            QMessageBox::warning(this, tr("HPSDRProgrammer_V2"),
                                 tr("Erase has timeout at %0 seconds\n").arg(HERMES_MAX_ERASE_TIMEOUTS/1000),
                                 QMessageBox::Retry, QMessageBox::Cancel);
        }else {  // Angelia
            stat->status(tr("Erase has timeout at %0 seconds\nTry again.\n").arg(ANGELIA_MAX_ERASE_TIMEOUTS/1000));
            QMessageBox::warning(this, tr("HPSDRProgrammer_V2"),
                                 tr("Erase has timeout at %0 seconds\n").arg(ANGELIA_MAX_ERASE_TIMEOUTS/1000),
                                 QMessageBox::Retry, QMessageBox::Cancel);
        }

        QApplication::restoreOverrideCursor();
        break;
    case PROGRAMMING:
        qDebug()<<"timeout";
        break;
    case READ_MAC:
        stat->status("Error: timeout reading MAC address!");
        stat->status("Check that the correct interface is selected.");
        text=QString("Check that there is a jumper");
        stat->status(text);
        //idle();
        break;
    case READ_IP:
        stat->status("Error: timeout reading IP address!");
        stat->status("Check that the correct interface is selected.");
        text=QString("Check that there is a jumper");
        stat->status(text);
        //idle();
        break;
    case WRITE_IP:
        // should not happen as there is no repsonse
        break;
   }
}

void MainWindow::programmingCompleted() {
    programComplete = true;
    state=IDLE;
    wb->state =IDLE;
    ui->discoverComboBox->clear();
    QTimer::singleShot(BOARD_DISCOVERY_DELAY, this, SLOT(discover()));

}


// SLOT - eraseCompleted
void MainWindow::eraseCompleted() {
    if( programComplete == false ){
      switch(state) {
      case ERASING:
          stat->status("Device erased successfully");
          state=PROGRAMMING;
          qDebug("Programming device ...");
          wb->sendData(wb->boards[wb->currentboard]);
          break;
      case ERASING_ONLY:
          stat->status("Device erased successfully");
          //status("Device erased successfully");
          //idle();
          QApplication::restoreOverrideCursor();
          break;
      case READ_MAC:
          qDebug()<<"received eraseCompleted when state is READ_MAC";
          break;
      case READ_IP:
          qDebug()<<"received eraseCompleted when state is READ_IP";
          break;
      case WRITE_IP:
          stat->status("IP address written successfully");
          //text=QString("IP address written successfully");
          //status(text);
          //idle();
          break;
      }
    }
}


void MainWindow::nextBuffer() {
    qDebug("in Mainwindow::nextbuffer");
    wb->incOffset();
    //wb->sendData(wb->boards[wb->currentboard]);
}
