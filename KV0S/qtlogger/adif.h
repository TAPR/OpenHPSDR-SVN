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


#ifndef ADIF_H
#define ADIF_H

#include <QObject>
#include <QRegExp>
#include <QStringList>
#include <QStandardItem>
#include <QXmlInputSource>
#include <QXmlSimpleReader>
#include <QXmlDefaultHandler>
#include "adifhandler.h"

class Adif : public QObject
{
    Q_OBJECT
public:
    explicit Adif(QObject *parent = 0);
    void adifParse(QString *str);
    QString* adifParseRecords( QString *str, bool header);
    QStringList* adifParseToItem(QString *str);
    void adifParseItem(const QString &str);
    void adifParseItem(const QString &str, QStandardItem *item, QString *colname);
    QString *recordStr;
    bool newRecord;


signals:

public slots:

};

#endif // ADIF_H
