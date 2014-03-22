/**
* \file data.h
* \brief Header files for the Data functions as part of the QtLogger program
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



#ifndef DATA_H
#define DATA_H

#include <QObject>
#include <QString>
#include <QStringList>
#include <QDateTime>
#include <QFileDialog>
#include <QFile>
#include <QStandardItemModel>
#include <QXmlSimpleReader>
#include <QRegExp>
#include <QRegExpValidator>
#include <QDebug>
#include <QSettings>

#include "loghandler.h"
#include "adif.h"

class Data : public QWidget
{
    Q_OBJECT
public:
    explicit Data(QWidget *parent = 0);
    ~Data();
    void setMinimumHeader();
    QStringList getHeaders();
    QDateTime * date;
    void setModel( QStandardItemModel* mod );
    QStandardItemModel* getModel();
    void updateRowColumns();
    QStandardItemModel *model;
    void setFilename( QString fname );
    void setDirname( QString dname );
    QString getFilename();
    QString getDirname();
    int getRows();
    int getColumns();
    QString freqstr;
    QString modestr;
    QSettings settings;
    QString operatorstr;
    QString qthstr;
    QString gridstr;
    QString stationstr;


signals:
    void refresh();

public slots:
    void writeXmlData( QString filename );
    void writeADIFData( QString filename );
    QString* readData();
    void readADIFHeader( QString filename );
    void readXMLHeader( QString filename );
    void readADIFData();
    void readXMLData();
    void queryFilename();
    void updateHeaders();
    void removeRow( QModelIndex idx );

private:
    QStringList *hdr;
    QStringList *selectedhdr;
    QString filename;
    QString dirname;
    int rows;
    int columns;



};

#endif // DATA_H
