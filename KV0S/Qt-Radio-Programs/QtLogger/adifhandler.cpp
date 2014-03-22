/**
* \file adifhandler.cpp
* \brief Source files for the adifhandler functions as part of the QtLogger program
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

#include "adifhandler.h"
#include <QDebug>

AdifHandler::AdifHandler() : QXmlDefaultHandler()
{

}

bool AdifHandler::startDocument()
{
    inchoice = false;
    initem = false;
    return true;
}


bool AdifHandler::startElement( const QString&, const QString&, const QString &name, const QXmlAttributes &attrs )
{

    //qDebug() << "in startElement " << name;

    if( inchoice && initem && name == "label")
    {
        inlabel = true;
    }
    else if( inchoice && initem && name == "value")
    {
        invalue = true;
    }
    else if( inchoice && initem && name == "lower")
    {
        inlower = true;
    }
    else if( inchoice && initem && name == "upper")
    {
        inupper = true;
    }
    else if( inchoice && name == "item")
    {
        initem = true;
    }
    else if( name == "choices")
    {
        inchoice = true;
        for( int i=0; i<attrs.count(); i++ )
        {
            if( attrs.localName(i) == "id"){
                ename = attrs.value( i );
            }
            //qDebug() << "in startElement " << ename;
            //qDebug() << "in startElement hash   " << attr.key(attrs.value( i )) << attr[attrs.localName(i)];
        }
    }
    return true;
}


bool AdifHandler::endElement(const QString &namespaceURI, const QString &localName, const QString &qName)
{
    if( qName == "label" )
    {
        inlabel = false;
    }
    else if ( qName == "value" )
    {
        invalue = false;
    }
    else if ( qName == "lower" )
    {
        inlower = false;
    }
    else if ( qName == "upper" )
    {
        inupper = false;
    }

    else if( qName == "item")
    {
        //qDebug() << "in endElement " << ename << label << value;
        if ( ename == "modes" )
        {
            mode.append( label );
        }
        else if( ename == "country")
        {
            country.append( value );
        }
        else if( ename == "subdivisions")
        {
            subdivisions.append( value );
        }
        else if( ename == "band")
        {
            band[value] = Range( lower, upper ) ;
            //qDebug() << label << value << lower << upper;
        }

        initem = false;
        iname = "";
        value = "";
        label = "";
        lower = "";
        upper = "";
    }
    else if ( qName == "choices")
    {
        inchoice = false;
        ename = "";
    }

    return true;
}


bool AdifHandler::fatalError (const QXmlParseException & exception)
 {
     qWarning() << "Fatal error on line" << exception.lineNumber()
                << ", column" << exception.columnNumber() << ":"
                << exception.message();

     return false;
 }

bool AdifHandler::characters(const QString &ch)
{
    if( inchoice && initem && inlabel )
    {
        label = ch;
    }
    else if( inchoice && initem && invalue )
    {
        value = ch;
    }
    else if( inchoice && initem && inlower )
    {
        lower = ch;
    }
    else if( inchoice && initem && inupper)
    {
        upper = ch;
    }

    //qDebug() << ename << iname << ch;
    return true;
}
