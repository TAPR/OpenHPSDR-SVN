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

#include <QDebug>
#include "adif.h"
#include "adifhandler.h"

Adif::Adif(QObject *parent) : QObject(parent)
{
    qDebug() << "in Adif";

    recordStr = new QString("");
    newRecord = false;

}

QString* Adif::adifParseRecords( QString *str, bool header=false)
{
    QString *nullStr = new QString("");
    if( newRecord == true )
    {
        recordStr->clear();
        newRecord = false;
    }
    if( str->contains("<eor>") || str->contains("<EOR>"))
    {
        recordStr->append(str);
        newRecord = true;
        return recordStr;
    }else if( str->contains("<eoh>") || str->contains("<EOH>"))
    {
        recordStr->append(str);
        newRecord = true;
        if( header == true )
        {
            return recordStr;
        }else{
            return nullStr;
        }

    }else{
        recordStr->append(str);
        return nullStr;
    }
}


QStringList* Adif::adifParseToItem(QString *str)
{
    QStringList *list = new QStringList();
    //qDebug() << *str;
    QRegExp rx("(<\\w+:\\d+(:\\w)?>[\\w ]*)");
    int pos = 0;
    while ((pos = rx.indexIn(*str, pos)) != -1) {
        list->append(rx.cap(1));
        //qDebug() << list[cnt];

        pos += rx.matchedLength();
        //qDebug()  << pos;
    }
    return (list);
 }

void Adif::adifParseItem(const QString &str )
{
    QString name, size, type, value;
    //qDebug() << str;
    QRegExp rx("<(\\w+):(\\d+)>([\\w ]+)");
    QRegExp rx2("<(\\w+):(\\d+):(\\w+)>([\\w ]+)");
    if (rx.indexIn(str) != -1) {
         name = rx.cap(1);
         size = rx.cap(2);
         value = rx.cap(3);
    }else if (rx2.indexIn(str) != -1) {
         name = rx2.cap(1);
         size = rx2.cap(2);
         type = rx2.cap(3);
         value = rx2.cap(4);
    }
    //qDebug() << name << size << type << value << endl;
}


void Adif::adifParseItem(const QString &str, QStandardItem *item, QString *colname)
{
    QString name, size, type, value;
    //qDebug() << str;
    QRegExp rx("<(\\w+):(\\d+)>([\\w ]+)");
    QRegExp rx2("<(\\w+):(\\d+):(\\w+)>([\\w ]+)");
    if (rx.indexIn(str) != -1) {
         name = rx.cap(1);
         size = rx.cap(2);
         value = rx.cap(3);
    }else if (rx2.indexIn(str) != -1) {
         name = rx2.cap(1);
         size = rx2.cap(2);
         type = rx2.cap(3);
         value = rx2.cap(4);
    }
    item->setText(value);
    *colname = name;
    //qDebug() << name << size << type << value << endl;
}


