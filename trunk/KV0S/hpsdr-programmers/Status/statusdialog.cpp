/*
 * File:  statusdialog.cpp
 *
 * Revised on December 30, 2012
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

#include "statusdialog.h"
#include "ui_statusdialog.h"

/*! \brief status( QString text )
 *
 *  This function send a text string to the status bar abd the status window.
 */

StatusDialog::StatusDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::StatusDialog)
{
    ui->setupUi(this);

    connect(ui->closeButton,SIGNAL(clicked()),this,SLOT(close()));
}

/*! \brief status( QString text )
 *
 *  This function send a text string to the status bar abd the status window.
 */

StatusDialog::~StatusDialog()
{
    delete ui;
}

/*! \brief status( QString text )
 *
 *  This function send a text string to the status bar abd the status window.
 */

void StatusDialog::status( QString text )
{
    //qDebug()<<"status:"<<text;
    ui->statusListWidget->insertItem(ui->statusListWidget->count(),text);
    ui->statusListWidget->setCurrentRow(ui->statusListWidget->count());
    emit stbar(text);
}


/*! \brief status( QString text )
 *
 *  This function send a text string to the status bar abd the status window.
 */


void StatusDialog::clear()
{
    ui->statusListWidget->clear();
}
