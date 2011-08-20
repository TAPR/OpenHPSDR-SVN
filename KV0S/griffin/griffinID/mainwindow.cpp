/**
* \file mainwindow.cpp
* \brief Code files for the MainWindow functions as part of the Griffin ID program
* \author David R. Larsen, KV0S
* \version 1.0.0
* \date August 14, 2011
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
#include <QDebug>

/**
 *  \brief the MainWindow class for the griffinID program
 *
 *
 */

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);




    this->setWindowTitle("Griffin ID program");
    ui->call_lineEdit->setInputMask(">NN9NNN");

    About *abd = new About();
    abd->setVersion( QString( "1.0.1" ) );

    Dialog *dlg = new Dialog();

    loadGridBox();
    loadPowerBox();

    ui->call_lineEdit->setText( settings.value("call").toString() );
    ui->grid_comboBox->setCurrentIndex( settings.value("grid").toInt() );
    ui->lat_doubleSpinBox->setValue( settings.value("lat").toDouble());
    ui->lon_doubleSpinBox->setValue( settings.value("lon").toDouble());
    ui->power_comboBox->setCurrentIndex( settings.value("power").toInt() );

    wspr = new WSPR();
    qrss = new QRSS();


    // test->packing("KV0S  ", "EM38", 20);
    // test->convolution();
    // test->interleave();

    /*for( int i= 0; i < 11; i++ )
    {
        qDebug() << test->packed[i];
    }
    char* str = test->getSymbol();
    for( int i= 0; i < 162; i++ )
    {
        qDebug() << QString("%1").arg(str[i],1,2);
    }
    char* str2 = test->symbol2;
    for( int i= 0; i < 162; i++ )
    {
        qDebug() << QString("%1").arg(str2[i],1,10 );
    }
    */

    connect( ui->actionQuit,SIGNAL(triggered()),this,SLOT(close()));
    //connect(ui->actionSave_as,SIGNAL(triggered()),this,SLOT(writeString()));
    connect(ui->actionAbout,SIGNAL(triggered()),abd,SLOT(aboutMessage()));
    connect(ui->grid_comboBox,SIGNAL(currentIndexChanged(int)),this,SLOT(gridLocationUpdate()));
    connect(ui->lat_doubleSpinBox,SIGNAL(valueChanged(double)),this,SLOT(latLocationUpdate(double)));
    connect(ui->lon_doubleSpinBox,SIGNAL(valueChanged(double)),this,SLOT(lonLocationUpdate(double)));
    connect(ui->actionSave_WSPR,SIGNAL(triggered()),this,SLOT(outputWSPRfile()));
    connect(ui->actionSave_QRSS,SIGNAL(triggered()),this,SLOT(outputQRSSfile()));
    connect(ui->actionHelp,SIGNAL(triggered()),dlg,SLOT(show()));


}

/**
 *  \brief the MainWindow decontructor
 *
 *
 */

MainWindow::~MainWindow()
{
    settings.setValue("call", ui->call_lineEdit->text());
    settings.setValue("grid", ui->grid_comboBox->currentIndex());
    settings.setValue("lat", ui->lat_doubleSpinBox->value());
    settings.setValue("lon", ui->lon_doubleSpinBox->value());
    settings.setValue("power", ui->power_comboBox->currentIndex());
    delete ui;
}

/**
 *  \brief gridLocationUpdate
 *  slot for updating the grid location update
 *
 *
 */

void MainWindow::gridLocationUpdate()
{
    int idx = ui->grid_comboBox->currentIndex();
    QString value = ui->grid_comboBox->itemText( idx );
    wspr->grid2deg( value.toLocal8Bit().data(), 4);
    ui->lat_doubleSpinBox->setValue(wspr->getLat());
    ui->lon_doubleSpinBox->setValue(wspr->getLong());
}

/**
 *  \brief latLocationUpdate function
 *  slot for updating latitude location update
 *
 */

void MainWindow::latLocationUpdate( double latval )
{
   double lonval = ui->lon_doubleSpinBox->value();
   wspr->deg2grid( latval, lonval, 4 );
   QString gridsq = QString( wspr->getGrid() );
   gridsq = gridsq.mid(0,4);
   //qDebug() << QString( gridsq );
   int idx = ui->grid_comboBox->findText( gridsq );
   //qDebug() << idx;
   ui->grid_comboBox->setCurrentIndex( idx );
}

/**
 *  \brief lonLocationUpdate function
 *   slot for updating longitude location update
 *
 */

void MainWindow::lonLocationUpdate( double lonval )
{
   double latval = ui->lat_doubleSpinBox->value();
   wspr->deg2grid( latval, lonval, 4 );
   //qDebug() << "char* " << wspr->getGrid();
   QString gridsq = QString( wspr->getGrid() );
   //qDebug() << "QString" << gridsq.mid(0,4);
   gridsq = gridsq.mid(0,4);
   int idx = ui->grid_comboBox->findText( gridsq );
   //qDebug() << idx;
   ui->grid_comboBox->setCurrentIndex( idx );
}

/**
 *  \brief loadGridBox function
 *
 *
 */

void MainWindow::loadGridBox()
{
    QStringList grid;
    QString gridcode;
    for( int i=0; i<18; i++ )
    {
        int code = 65 + i;
        gridcode = QString( "####" );
        gridcode.replace( 0, 1, QChar(code) );
        for( int j=0; j<18; j++ )
        {
            int code1 = 65 + j;
            gridcode.replace( 1, 1, QChar(code1) );
            for( int k=0; k<10; k++ )
            {
                int code2 = 48 + k;
                gridcode.replace( 2, 1, QChar(code2) );
                for( int l=0; l<10; l++ )
                {
                    int code3 = 48 + l;
                    gridcode.replace( 3, 1, QChar(code3) );
                    grid << gridcode;
                }
            }
        }
    }
    ui->grid_comboBox->addItems( grid );

}

/**
 *  \brief loadPowerBox function
 *
 *
 */

void MainWindow::loadPowerBox()
{
    QStringList power;
    QString powercode;
    powercode = QString( "##" );
    for( int i=0; i<7; i++ )
    {
        int code = 48 + i;
        powercode.replace( 0, 1, QChar(code) );
        for( int l=0; l<10; l++ )
        {
            if( l == 0 || l == 3 || l == 7)
            {
               int code1 = 48 + l;
               powercode.replace( 1, 1, QChar(code1) );
               power << powercode;
            }
        }
    }
    ui->power_comboBox->addItems( power );
}

/**
 *  \brief outputWSPRfile function
 *
 *
 */

void MainWindow::outputWSPRfile( )
{
    QString call = ui->call_lineEdit->text();
    QString loc = ui->grid_comboBox->currentText();
    QString dbm = ui->power_comboBox->currentText();
    wspr->packing( QString("%1").arg(call,6).toLocal8Bit().data(), loc.toLocal8Bit().data(), dbm.toInt());
    wspr->convolution();
    wspr->interleave();
    char* symb = wspr->getSymbol();
    QString message = QString("%1 %2 %3").arg(call,6).arg(loc).arg(dbm);
    WSPRfile =  QFileDialog::getSaveFileName(this, tr("WSPR Output File"), "", tr("WSPR Files (*.mif)"));
    QFile file( WSPRfile );
    if (!file.open(QIODevice::WriteOnly | QIODevice::Text))
             return;

    QTextStream out(&file);
    out << "-- WSPR message file for openhpsdr.org : griffin board" << endl;
    out << "-- Encode call sign is "<< message << endl;
    // TODO: show Binary, Hex and Display
    out << "WIDTH=2;" << endl;
    out << "DEPTH=162;" << endl;
    out << "ADDRESS_RADIX=UNS;" << endl;
    out << "DATA_RADIX=UNS;" << endl;
    out << "CONTENT BEGIN" << endl;
    for( int i = 0; i < 162; i++ ){
        //TODO: add real data
        out << i << " : " << QString("%1").arg(int(symb[i]),1,10) << ";" << endl;
    }
    out << "END;" << endl;
    file.close();
 }

/**
 *  \brief outputQRSSfile function
 *
 *
 */

void MainWindow::outputQRSSfile( )
{
    QString call = ui->call_lineEdit->text();
    QString message = QString("%1").arg(call,6);
    QString str = qrss->encodeCW( call.mid(0,6) );
    QRSSfile =  QFileDialog::getSaveFileName(this, tr("QRSS Output File"), "", tr("QRSS Files (*.mif)"));
    QFile file( QRSSfile );
    if (!file.open(QIODevice::WriteOnly | QIODevice::Text))
             return;

    QTextStream out(&file);
    out << "-- QRSS call sign file for openhpsdr.org : griffin board" << endl;
    out << "-- Encode call sign is "<< message << endl;
    out << "-- " << str << endl;
    out << "-- " << qrss->displayCW( str ) << endl;
    // TODO: show Binary, Hex and Display
    out << "WIDTH=8;" << endl;
    out << "DEPTH=16;" << endl;
    out << "ADDRESS_RADIX=UNS;" << endl;
    out << "DATA_RADIX=HEX;" << endl;
    out << "CONTENT BEGIN" << endl;
    for( int i = 0; i< (str.size() / 8); i++ )
    {
        out << i << " : " << (QString("%1").arg(wspr->binaryByteLookup( str.mid(i*8,8) ),2,16)).toUpper() << ";" << endl;
    }
    out << "END;" << endl;
    file.close();
 }

