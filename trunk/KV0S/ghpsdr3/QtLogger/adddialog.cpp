/**
* \file adddialog.cpp
* \brief Source files for the adddialog functions as part of the QtLogger program
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

#include "adddialog.h"
#include "ui_adddialog.h"

addDialog::addDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::addDialog)
{
    ui->setupUi(this);
    this->setWindowTitle( "Add Contact" );

    date = new QDate();
    time = new QTime();

    QTimer *timer = new QTimer(this);
    connect(timer, SIGNAL(timeout()), this, SLOT(updateTime()));
    timer->start(1000);


    QRegExp rx("^[A-z]{1,2}[0-9]{1}[A-z]{1,3}[/]{1}[A-z0-9]+");
    QRegExpValidator  *valid = new QRegExpValidator(rx,this);
    ui->callEdit->setValidator( valid );

    connect(ui->cancelButton,SIGNAL(clicked()),this,SLOT(close()));
    connect(ui->resetButton,SIGNAL(clicked()),this,SLOT(reset()));
    connect(ui->addButton,SIGNAL(clicked()),this,SLOT(addContact()));
    connect(ui->callEdit,SIGNAL(textChanged(QString)),this,SLOT(updateCall()));
}

addDialog::~addDialog()
{
    delete ui;
}

void addDialog::reset()
{
    // This function should reset all the field in the add Dialog
    ui->dateEdit->setText(date->currentDate().toString("yyyy:MM:dd"));
    ui->timeEdit->setText(time->currentTime().toString());
    ui->callEdit->clear();
    ui->nameEdit->clear();
    ui->countryEdit->clear();
    ui->subdivisionEdit->clear();
    ui->qthEdit->clear();
    ui->checkEdit->clear();

}

void addDialog::callreset()
{
    reset();
}

void addDialog::setFrequency( QString freq )
{
    ui->freqEdit->setText( freq );
}

void addDialog::setMode( QString modestr)
{
    ui->modeEdit->setText( modestr );
}

void addDialog::updateTime()
{
    ui->dateEdit->setText(date->currentDate().toString("yyyy:MM:dd"));
    ui->timeEdit->setText(time->currentTime().toString());
}

void addDialog::addContact()
{
    record.clear();
    int contact = ui->contactBox->value();
    record["CONTACT"] = QString("%1").arg(contact);
    record["CALL"] = ui->callEdit->text();
    record["DATE"] = ui->dateEdit->text();
    record["TIME"] = ui->timeEdit->text();
    record["FREQ"] = ui->freqEdit->text() ;
    record["RX_RST"] = ui->rxrstBox->currentText();
    record["TX_RST"] = ui->txrstBox->currentText();
    record["NAME"] = ui->nameEdit->text();
    record["QTH"] = ui->qthEdit->text();
    record["MODE"] = ui->modeEdit->text();
    record["COUNTRY"] = ui->countryEdit->text();
    record["STATE"] = ui->subdivisionEdit->text();
    record["EVENT"] = ui->eventBox->currentText();
    record["CHECK"] = ui->checkEdit->text();
    record["BAND"] = getBand(ui->freqEdit->text());

    //qDebug() << getBand(ui->freqEdit->text()) << ui->freqEdit->text();
    contact++;
    ui->contactBox->setValue( contact );
    //reset();
    emit newdata();
}

QString addDialog::getBand( QString fstr )
{
    QString bandstr;
    fstr = ui->freqEdit->text().mid(0,5);
    double f = fstr.toDouble();
    //qDebug() << f << bandData["160m"].lower;
    QHashIterator<QString, Range> i(bandData);
    while (i.hasNext()) {
        i.next();
        //qDebug() << i.value().lower << f << i.value().upper;
        if( (f > i.value().lower.toDouble()) && (f < i.value().upper.toDouble()))
        {
            bandstr = i.key();
        }
    }
    return( bandstr );
}

void addDialog::updateCall()
{
    QString call = ui->callEdit->text();
    ui->callEdit->setText( call.toUpper() );
}

void addDialog::loadmodeComboBox(QStringList *modes)
{
    QCompleter *modeCompleter = new QCompleter(*modes, this);
    modeCompleter->setCaseSensitivity(Qt::CaseInsensitive);
    ui->modeEdit->setCompleter(modeCompleter);
}

void addDialog::loadcountryComboBox(QStringList *country)
{
    QCompleter *countryCompleter = new QCompleter( *country, this);
    countryCompleter->setCaseSensitivity(Qt::CaseInsensitive);
    ui->countryEdit->setCompleter(countryCompleter);
}

void addDialog::loadsubdivisionsComboBox(QStringList *subdivisions)
{
    QCompleter *subdivisionsCompleter = new QCompleter( *subdivisions, this);
    subdivisionsCompleter->setCaseSensitivity(Qt::CaseInsensitive);
    ui->subdivisionEdit->setCompleter(subdivisionsCompleter);
}

void addDialog::loadbandsData( QHash <QString, Range> band )
{
    bandData = QHash <QString, Range>( band );
}
