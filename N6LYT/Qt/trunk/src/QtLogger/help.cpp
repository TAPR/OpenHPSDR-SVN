/**
* \file help.cpp
* \brief Code files for the Help functions as part of the QtLogger program
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




#include "help.h"
#include "ui_help.h"

Help::Help(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::Help)
{
    QString path;
    path = QApplication::applicationDirPath() + "/help/";
    ui->setupUi(this);
    ui->textBrowser->setSearchPaths(QStringList() << ":/help/" << path );
    ui->textBrowser->setSource(QUrl("QtLogger.html"));
    oldsource << QUrl("QtLogger.html");

    connect(ui->quitButton,SIGNAL(clicked()),this,SLOT(close()));
    connect(ui->homeButton,SIGNAL(clicked()),this,SLOT(home()));
    connect(ui->backButton,SIGNAL(clicked()),this,SLOT(back()));
    connect(ui->textBrowser,SIGNAL(sourceChanged(QUrl)),this,SLOT(updatesource(QUrl)));

}

Help::~Help()
{
    delete ui;
}

void Help::home()
{
    ui->textBrowser->setSource(QUrl("QtLogger.html"));
}

void Help::back()
{
    if( oldsource.isEmpty() ){
        ui->textBrowser->setSource(QUrl("QtLogger.html"));
    }else{
        oldsource.removeLast();
        ui->textBrowser->setSource( oldsource.last());
    }

}

void Help::updatesource( QUrl newsource )
{
    oldsource << newsource;
}
