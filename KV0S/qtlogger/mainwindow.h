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


#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QTextStream>

#include "adif.h"
#include "about.h"
#include "savedata.h"
#include "loaddata.h"
#include "adddialog.h"
#include <QUdpSocket>

namespace Ui {
    class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    QStandardItemModel *model;
    loadData *dta;
    saveData *save;
    QString getVersion();
    QString version;
    QStringList *modes;
    QStringList *country;
    void readDefinitions( QString filename );

private slots:
    void update();
    void newFile();
    void readFile();
    void addItem();
    void processPendingDatagrams();

private:
    Ui::MainWindow *ui;
    addDialog *add;
    QString *type;
    QUdpSocket *udpSocket;
    QString parseDatagrams( QString data );
};

#endif // MAINWINDOW_H
