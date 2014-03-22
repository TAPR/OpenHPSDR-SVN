/**
* \file mainwindow.h
* \brief Header files for the MainWindow functions as part of the QtLogger program
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



#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QString>
#include <QUdpSocket>
#include <QScrollBar>
#include <QLabel>
#include <QStandardItemModel>

#include "about.h"
#include "help.h"
#include "data.h"
#include "adddialog.h"
#include "adifhandler.h"
#include "range.h"
#include "preferences.h"
#include "lastcontact.h"
#include "ctyxmlreader.h"


namespace Ui {
    class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    QString version;
    void readDefinitions( QString filename );
    QStandardItemModel *model;
    CtyXmlReader *ctyreader;


private slots:
    void update();
    void writeData();
    void processPendingDatagrams();
    void updateStatus();
    void updateOwner();
    void updateView();
    void closeDown();
    void removeTableRow(QModelIndex idx);
    void setDeleteFlag( bool flg );
    void setOwner( QString );
    void setQTH( QString );
    void setGrid( QString );
    void setStation( QString );
    void setLogDirectory();
    void setSupportDirectory();


private:
    Ui::MainWindow *ui;
    Data *data;
    addDialog *add;
    Preferences * pref;
    lastContact * last;
    QUdpSocket *udpSocket;
    QString parseDatagrams( QString data );
    QStringList *modes;
    QStringList *country;
    QStringList *subdivisions;
    QHash <QString, Range> bands;
    QSettings settings;
    QLabel *logfileIndicator;
    bool deleteFlag;





};

#endif // MAINWINDOW_H
