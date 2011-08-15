/**
* \file mainwindow.h
* \brief Header files for the MainWindow functions as part of the Griffin ID program
* \author David R. Larsen, KV0S
* \version 1.0.0
* \date August 14, 2011
*/



/* Copyright (C) 2010 - David R. Larsen, KV0S
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



#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QFile>
#include <QFileDialog>
#include <QSettings>
#include <QByteArray>
#include <QHelpEngine>
#include "about.h"
#include "qrss.h"
#include "wspr.h"

namespace Ui {
    class MainWindow;
}
/*!
 *  \brief the MainWindow class for the griffinID program
 *  this class deals with the GUI
 */


class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    QSettings settings;
    void loadGridBox();
    void loadPowerBox();
    WSPR *wspr;
    QRSS *qrss;
    int wsprcallchar( QChar ch );
    int wsprlocchar( QChar ch );
    long encodeWSPR( QStringList message );


private:
    Ui::MainWindow *ui;
    QString WSPRfile;
    QString QRSSfile;


private slots:
    void gridLocationUpdate();
    void latLocationUpdate( double latval );
    void lonLocationUpdate( double lonval );
    void outputWSPRfile();
    void outputQRSSfile();
};

#endif // MAINWINDOW_H
