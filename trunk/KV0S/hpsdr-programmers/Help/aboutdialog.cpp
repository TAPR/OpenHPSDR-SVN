/*
 * File:   aboutdialog.cpp
 *
 * Created on December 29, 2012
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

#include "aboutdialog.h"
#include "ui_aboutdialog.h"

/*! \brief AboutDialog(QWidget *parent)
 *
 *  This constructor function for AboutDialog class.
 */

AboutDialog::AboutDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::AboutDialog)
{
    ui->setupUi(this);

    ui->TitleLabel->setText("HPSDRBootloader");
    ui->logoLabel->setPixmap(QPixmap(":icons/hpsdr4sm.png"));

    connect(ui->OKButton,SIGNAL(clicked()),this,SLOT(close()));
}

/*! \brief ~AboutDialog()
 *
 *  This Deconstructor function AboutDialog class.
 */

AboutDialog::~AboutDialog()
{
    delete ui;
}

/*! \brief setVersion(QString version, QString release)
 *
 *  \param version is the current program version number.
 *  \param release is the relase date.
 *
 *  This function to set the version and release date in the about screen.
 */

void AboutDialog::setVersion(QString version, QString release)
{
    ui->versionLabel->setText(QString("%0 - %1").arg(version).arg(release));
}
