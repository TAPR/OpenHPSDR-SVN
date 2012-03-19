#include <QtXmlPatterns/QXmlQuery>
#include "ctyxmlreader.h"

#include <QDebug>

CtyXmlReader::CtyXmlReader (QStandardItemModel *model) :
    myModel (model)
{
}

bool CtyXmlReader::read (QFile *file)
{
    qDebug() << "inside read";
    myFile = file;
    qDebug() << "inside after file";
    xml.setDevice (myFile);
    qDebug() << "inside setdevice" << myFile->fileName();

    qDebug() << "in next element" << xml.text();
    if (xml.readNextStartElement ())
    {
        qDebug() << "in next element" << xml.text();
        if (xml.name () == "resources")
            readResources (file->fileName());
        else
            xml.raiseError (QObject::tr ("The file is not a valid country file."));
    }

    return !xml.error ();
}

QString CtyXmlReader::errorString () const
{
    return QObject::tr("%1\nLine %2, column %3")
                 .arg(xml.errorString())
                 .arg(xml.lineNumber())
                 .arg(xml.columnNumber());
}

void CtyXmlReader::readResources (const QString filename)
{
    Q_ASSERT (xml.isStartElement () && xml.name () == "resources");
    qDebug() << "inside readResources";
    // create function item
    qDebug() << filename;

    while (xml.readNextStartElement ())
    {
        if (xml.name () == "country")
            readCountry ( );
    }

}


void CtyXmlReader::readCountry ()
{
    Q_ASSERT (xml.isStartElement () && xml.name () == "country");

    codestr = xml.attributes().value("code").toString();

    while (xml.readNext ())
    {
        if (xml.name () == "name" && xml.isStartElement ())
        {
            //qDebug() << "1" <<xml.name() << xml.readElementText() << codestr;
            country = QString( xml.readElementText() );


        }
        else if (xml.name () == "cont" && xml.isStartElement ())
        {
            qDebug() << "2" << xml.name() << xml.readElementText();
        }
        else if (xml.name () == "subdivision" && xml.isStartElement ())
        {
            readSubdivision ( );
            //qDebug() << "3" << xml.name() << xml.readElementText();
        }
        else if (xml.name () == "country" && xml.isEndElement ())
        {

            qDebug() << "readCountry" << country << codestr;

            return;
        }
        else
        {
            //qDebug() << "5" << xml.name() << xml.readElementText();
        }
    }
 }


void CtyXmlReader::readSubdivision ()
{
     Q_ASSERT (xml.isStartElement () && xml.name () == "subdivision");
     //QList <QStandardItem *> subdivisionList;
     QStringList prestr = QString(xml.attributes().value("prefix").toString().simplified()).split(",");

     qDebug() << "subdivision" ;

     while (xml.readNext ())
     {
         if (xml.name () == "name" && xml.isStartElement ())
         {
             subdivision = QString( xml.readElementText() );

             for (int i = 0; i < prestr.size(); ++i)
             {
                 countryList.clear();

                 QStandardItem* count = new QStandardItem(country);
                 QStandardItem* code = new QStandardItem(codestr);
                 QStandardItem* subdiv = new QStandardItem(subdivision);
                 QStandardItem* pref = new QStandardItem(prestr[i]);

                 countryList.append( count );
                 countryList.append( code );
                 countryList.append( subdiv );
                 countryList.append( pref );

                 myModel->appendRow( countryList );
                 countryList.clear();
             }
             qDebug() << "6" << xml.name() << xml.readElementText();

         }else if (xml.name () == "ITU" && xml.isStartElement ())
         {
             qDebug() << "3" << xml.name() << xml.readElementText();
         }
         else if (xml.name () == "CQ" && xml.isStartElement ())
         {
             qDebug() << "4" << xml.name() << xml.readElementText();
         }
         else if (xml.name () == "subdivision" && xml.isEndElement ())
         {
             return;
         }
         else
         {
             //qDebug() << "5" << xml.name() << xml.readElementText();
         }
      }
}
