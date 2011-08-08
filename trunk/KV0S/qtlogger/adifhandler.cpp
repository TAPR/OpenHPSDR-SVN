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

        initem = false;
        iname = "";
        value = "";
        label = "";
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

    //qDebug() << ename << iname << ch;
    return true;
}
