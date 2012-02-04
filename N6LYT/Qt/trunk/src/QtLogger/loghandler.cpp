/**
* \file loghandler.cpp
* \brief Scource files for the loghandler functions as part of the QtLogger program
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
#include "loghandler.h"
#include <QDebug>

logHandler::logHandler() : QXmlDefaultHandler()
{
    headerlist = new QStringList();
    inrecord = false;
    headerflag = true;
    tablerow = 0;
}

logHandler::logHandler( QStandardItemModel *model, QStringList* hdr ) : QXmlDefaultHandler()
{

    headerlist = new QStringList( *hdr);
    localmodel = model;
    inrecord = false;
    headerflag = false;
    tablerow = -1;
}


bool logHandler::startDocument()
{
    //qDebug() << "in startDocument";
    return true;
}

bool logHandler::startElement( const QString&, const QString&, const QString &name, const QXmlAttributes &attrs )
{
    //qDebug() << "in startElement " << name;
    if( name == "record" )
    {
        inrecord = true;
        tablerow++;

    }
    label = name;

    return true;
}

bool logHandler::endElement(const QString &namespaceURI, const QString &localName, const QString &qName)
{

    if( inrecord )
    {
        if( qName == "record" )
        {
            inrecord = false;

        }else{
            if( headerflag )
            {
               headerlist->append( QString( qName.toUpper()) );
            }else{
               //qDebug() << "in endElement " << qName << datavalue << tablerow;
               QStandardItem *it = new QStandardItem( datavalue );
               //qDebug() << it->text();
               //qDebug() << *headerlist;
               tablecol = headerlist->indexOf(qName.toUpper());
               //qDebug() << tablerow << tablecol << it->text();
               localmodel->setItem( tablerow, tablecol, it );
            }
        }
        label.clear();
    }
    return true;
}

bool logHandler::characters(const QString &ch)
{
    if( inrecord && (label != "record") && !headerflag )
    {
        //qDebug() << "in characters " << ch;
        datavalue = ch;
    }
    //qDebug() << "in characters " << ch;
    return true;
}



bool logHandler::fatalError (const QXmlParseException & exception)
{
    qWarning() << "Fatal error on line" << exception.lineNumber()
               << ", column" << exception.columnNumber() << ":"
               << exception.message();

    return false;
}

QStringList* logHandler::getHeaders()
{
    return( headerlist );
}

void logHandler::setHeader( QStringList* header )
{
    headerlist = header;
}

void logHandler::setModel(QStandardItemModel *model)
{
   localmodel = model;
}

QStandardItemModel* logHandler::getModel()
{
    //qDebug() << "in Loghandler ";
    //qDebug() << "in Loghandler " << localmodel->item(1,1)->text();
    return( localmodel );
}

void logHandler::readHeader( bool flag )
{
   headerflag = flag;
}

int logHandler::getRows()
{
    return( tablerow );
}
