/**
* \file loghandler.h
* \brief Header files for the logHandler functions as part of the QtLogger program
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

#ifndef LOGHANDLER_H
#define LOGHANDLER_H

#include <QXmlDefaultHandler>
#include <QStandardItem>
#include <QStandardItemModel>
#include <QList>


class logHandler : public QXmlDefaultHandler
{

public:
    explicit logHandler();
    explicit logHandler( QStandardItemModel *model, QStringList *hdr );
    bool startDocument();
    bool startElement(const QString &namespaceURI, const QString &localName, const QString &qName,  const QXmlAttributes &attrs);
    bool endElement(const QString &namespaceURI, const QString &localName, const QString &qName);
    bool fatalError (const QXmlParseException & exception);
    bool characters(const QString &ch);
    QStringList* getHeaders();
    void setModel( QStandardItemModel *model );
    void setHeader( QStringList* header );
    void readHeader( bool flag );
    QStandardItemModel* getModel();
    QStandardItemModel* localmodel;
    int getRows();

signals:

public slots:

private:
    bool inrecord;
    bool inlabel;
    bool invalue;
    bool headerflag;
    int tablerow;
    int tablecol;
    QString label;
    QString datavalue;
    QStringList *headerlist;
};

#endif // LOGHANDLER_H
