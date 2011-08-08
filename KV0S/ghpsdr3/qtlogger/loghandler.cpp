
#include "loghandler.h"
#include <QDebug>

logHandler::logHandler() : QXmlDefaultHandler()
{
    headerlist = new QStringList();
    inrecord = false;
    headerflag = true;
    tablerow = 0;
}

logHandler::logHandler( QStandardItemModel* model ) : QXmlDefaultHandler()
{
    headerlist = new QStringList();
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
               qDebug() << "in endElement " << qName << datavalue << tablerow;
               QStandardItem *it = new QStandardItem( datavalue );
               qDebug() << it->text();
               qDebug() << *headerlist;
               tablecol = headerlist->indexOf(qName.toUpper());
               qDebug() << tablerow << tablecol << it->text();
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
