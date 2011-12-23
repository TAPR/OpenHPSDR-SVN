/**
* \file adifhandler.h
* \brief Header files for the AdifHandler functions as part of the QtLogger program
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

#ifndef ADIFHANDLER_H
#define ADIFHANDLER_H

#include <QXmlDefaultHandler>
#include <QHash>

#include "range.h"


class AdifHandler : public QXmlDefaultHandler
{

public:
    explicit AdifHandler();
    bool startDocument();
    bool startElement(const QString &namespaceURI, const QString &localName, const QString &qName,  const QXmlAttributes &attrs);
    bool endElement(const QString &namespaceURI, const QString &localName, const QString &qName);
    bool fatalError (const QXmlParseException & exception);
    bool characters(const QString &ch);
    QString abbr;
    QString atName;
    QString dataString;
    QStringList mode;
    QStringList country;
    QHash <QString, QString> ARRLSections;
    QStringList subdivisions;
    QHash <QString, Range> band;


signals:

public slots:

private:
    bool inchoice;
    bool initem;
    bool inlabel;
    bool invalue;
    bool inlower;
    bool inupper;
    QString ename;
    QString iname;
    QString label;
    QString value;
    QString upper;
    QString lower;
    QHash <QString,QString> attr;
};

#endif // ADIFHANDLER_H
