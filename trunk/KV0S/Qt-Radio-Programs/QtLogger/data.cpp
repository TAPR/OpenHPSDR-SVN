/**
* \file data.cpp
* \brief Code files for the Data functions as part of the QtLogger program
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




#include "data.h"

Data::Data(QWidget *parent) :
    QWidget(parent)
{
    model = new QStandardItemModel();
    hdr = new QStringList();
}

Data::~Data()
{
    delete model;
    delete hdr;
}

void Data::setMinimumHeader()
{
    *hdr << "CALL" << "NAME" << "QSO_DATE" << "TIME_ON";
    *hdr << "FREQ" << "MODE" << "BAND";
    *hdr << "TX_RST"<< "RX_RST" << "COUNTRY";
    *hdr << "QTH" << "GRIDSQUARE" << "STATE" << "CNTY";
    *hdr << "COMMENT" << "EVENT";
    *hdr << "CONTACT" << "CHECK";
    *hdr << "OPERATOR" << "HOME_QTH" << "HOME_GRID" << "STATION_CALL";
    *hdr << "QSL_SENT" <<"QSL_SENT_VIA";
    *hdr << "QSL_RCVD" << "QSL_RCVD_VIA";
    //qDebug() << *hdr;
}

QStringList Data::getHeaders()
{
    //qDebug() << "in getHeader";
    //qDebug() << *selectedhdr;
    return QStringList(*hdr);
}

QStandardItemModel* Data::getModel()
{
    return( model );
}

void Data::setModel(QStandardItemModel *mod)
{
    qDebug() << "in setModel before " << model->item(1,1) << mod->item(1,1);
    model = mod;
    qDebug() << "in setModel after " << model->item(1,1) << mod->item(1,1);
}

void Data::updateRowColumns()
{
    rows = model->rowCount();
    columns = model->columnCount();
}


void Data::writeXmlData( QString filename )
{
    updateRowColumns();
    QFile data(filename);
    if (data.open(QIODevice::WriteOnly)) {
      QTextStream stream(&data);
      stream << QString("<?xml version=\"1.0\" encoding=\"UTF-8\"?>") << endl;
      stream << QString("<?xml-stylesheet type=\"text/xsl\" href=\"qtlogger.xsl\" ?>") << endl;
      stream << QString("<AmateurRadioLog>") << endl;
      stream << QString("  <header>") << endl;
      stream << QString("    <program>") << endl;
      stream << QString("      <author>Dave Larsen KV0S</author>") << endl;
      stream << QString("      <info>For more information see: http://openhpsdr.org</info>") << endl;
      stream << QString("      <created>");
      stream << (date->currentDateTimeUtc()).toString("yyyyMMdd-hhmm");
      stream << QString(" UTC</created>") << endl;
      stream << QString("      <program>QtLogger</program>") << endl;
      stream << QString("      <programversion>%1</programversion>").arg( "1.0.1" ) << endl;  // fix to mainWindow version
      stream << QString("    </program>") << endl;
      stream << QString("  </header>") << endl;
      stream << QString("  <contacts>") << endl;

      for (int i = 0; i < rows; ++i)
      {
          stream << QString("   <record>") << endl;
          for (int j = 0; j < columns; ++j)
          {
              QStandardItem * it;
              QString value;
              QString name;
              //int size = 0;
              it = model->item(i,j);
              if( !(it == 0) )
              {
                 name = (model->horizontalHeaderItem(j))->text();
                 value = (model->item(i,j))->text();
                 value.remove(QRegExp(" $"));
                 //size = value.length();
                 stream << QString("     <");
                 stream << name.toLower();
                 stream << QString(">");
                 stream << value.simplified();
                 stream << QString("</");
                 stream << name.toLower();
                 stream << QString(">") << endl;
              }
          }
          stream << QString("   </record>") << endl;
      }

      stream << QString("  </contacts>") << endl;
      stream << QString("</AmateurRadioLog>") << endl;
    }
    data.close();

}
QString* Data::readData()
{
    QString *filetype = new QString();
    QString fname = settings.value("filename").toString();
    filename =  QFileDialog::getOpenFileName(this, tr("Open File"),fname,tr("Files (*.xml *.adif *.adi)"));
    //qDebug() << "in data::readData" << filename << " returned " << endl;
    settings.setValue("filename", filename );
    if( filename.contains("xml"))
    {
        readXMLHeader( filename );
        *filetype = "XML";
        readXMLData();
     }else{
        readADIFHeader( filename );
        *filetype = "ADIF";
        readADIFData();
    }
    return filetype;
}

void Data::readXMLHeader( QString filename )
{


   QFile *file = new QFile(filename);
   if( !file->open(QFile::ReadOnly) )
   {
       qDebug() << "Open XML Header file failed.";
   }else{
       qDebug() << "Open XML Header file success.";
   }
   QXmlSimpleReader logxmlReader;
   QXmlInputSource *source = new QXmlInputSource( file );

   logHandler *loghandler = new logHandler();
   loghandler->readHeader( true );
   logxmlReader.setContentHandler(loghandler);
   logxmlReader.setErrorHandler(loghandler);


   bool ok = logxmlReader.parse(source);

   if( !ok )
   {
       qDebug() << "Parsing XML Data file failed.";
   }
   hdr = loghandler->getHeaders();
   hdr->removeDuplicates();
   //qDebug() << hdr->size() << *hdr << endl;
   columns = hdr->size();
   //*selectedhdr = QStringList(*hdr);
   //qDebug() << *selectedhdr << endl;
   file->close();
}

void Data::readXMLData()
{
    QFile *file = new QFile(filename);
    if( !file->open(QFile::ReadOnly) )
    {
        qDebug() << "Open XML Data file failed.";
    }else{
        qDebug() << "Open XML Data file success.";
    }
    QXmlSimpleReader logxmlReader;
    QXmlInputSource *source = new QXmlInputSource( file );

    logHandler *loghandler = new logHandler( model, hdr );
    loghandler->readHeader( false );
    loghandler->setHeader( hdr );
    logxmlReader.setContentHandler(loghandler);
    logxmlReader.setErrorHandler(loghandler);


    bool ok = logxmlReader.parse(source);

    if( !ok )
    {
        qDebug() << "Parsing XML Data file failed." << endl;
    }
    //qDebug() << (model->item(1,1))->text();
    //qDebug() << "from Loghandler " << loghandler->getModel()->item(1,1);
    //qDebug() << "in readXMLData " << model->item(1,1);
    rows = loghandler->getRows();
    emit refresh();
    file->close();
}

/**
 * \brief ADIF code elements
 *
 */

void Data::writeADIFData( QString filename )
{
    updateRowColumns();
    QFile data(filename);
    if (data.open(QIODevice::WriteOnly)) {
      QTextStream stream(&data);
      stream << QString("ADIF v2.2.6 Exported from QtLogger-0.1") << endl;
      stream << QString("For more information see: http://openhpsdr.org") << endl;
      stream << QString("Log File saved: ");
      stream << (date->currentDateTimeUtc()).toString("yyyyMMdd-hhmm");
      stream << QString(" UTC") << endl;
      stream << QString("<PROGRAMID:8>QtLogger<PROGRAMVERSION:3>") << QString("0.1") << endl;
      stream << QString("<EOH>") << endl << endl;



      for (int i = 0; i < rows; ++i)
      {
          for (int j = 0; j < columns; ++j)
          {
              QStandardItem * it;
              QString value;
              QString name;
              int size;
              it = model->item(i,j);

              if( !(it == 0) )
              {
                 name = (model->horizontalHeaderItem(j))->text();
                 value = (model->item(i,j))->text();
                 value.remove(QRegExp(" $"));
                 size = value.length();
                 stream << QString("<");
                 stream << name;
                 stream << QString(":%1").arg(size);
                 stream << QString(">");
                 stream << value;
              }
          }
          stream << QString("<EOR>") << endl;
      }
    }else{
        qDebug() << "Could not open file " << filename ;
    }
    data.close();
}

void Data::readADIFHeader( QString filename )
{

     Adif *log = new Adif(this);

     QFile data(filename);
     //parent->ui->statusBar->showMessage(QString("Opening: \"%1\"").arg(filename));
     if (data.open(QFile::ReadOnly)) {
       QTextStream stream(&data);
       QString line;

       QString *record;
       QStringList *list;

       do {
           line = stream.readLine();
           //qDebug() << line << endl;
           record = log->adifParseRecords(&line, false);
           list = log->adifParseToItem(record);
           //qDebug() << list->size() << *list << endl;
           for(int i = 0; i < list->size(); i++){
             QStandardItem *it = new QStandardItem();
             QString *name = new QString();
             log->adifParseItem(list->at(i), it, name);
             *hdr << *name;
           }
           hdr->removeDuplicates();
       }while(!line.isNull());
       //qDebug() << "in loadHeader";
       //qDebug() << hdr->size() << *hdr << endl;
       columns = hdr->size();
       //*selectedhdr = QStringList(*hdr);
       //qDebug() << *selectedhdr;
       data.close();
    }

}

void Data::readADIFData()
{

     Adif *log = new Adif(this);

     QFile data(filename);
     //parent->ui->statusBar->showMessage(QString("Opening: \"%1\"").arg(filename));
     if (data.open(QFile::ReadOnly)) {
       QTextStream stream(&data);
       QString line;

       QString *record;
       QStringList *list;
       int row = 0;
       int col = 0;

       do {
           line = stream.readLine();
           //qDebug() << line << endl;
           if ( line.contains("CALL") )
           {
              record = log->adifParseRecords(&line, false);
              list = log->adifParseToItem(record);
              //qDebug() << list->size() << *list << endl;
              for(int i = 0; i < list->size(); i++){
                QStandardItem *it = new QStandardItem();
                QString *name = new QString();
                log->adifParseItem(list->at(i), it, name);
                model->setItem(row,hdr->indexOf(*name),it);
                col++;
              }
              row++;
              col = 0;
           }
       }while(!line.isNull());
       rows = row;
       //qDebug() << hdr->size() << *hdr << endl;
       emit refresh();

  }else{
         qDebug() << " Could not open ADIF file " << filename << endl;
  }
  data.close();
}



void Data::queryFilename()
{
    QString dname = settings.value("directory").toString();
    filename =  QFileDialog::getOpenFileName(this, tr("Open File"),dname,tr("Files (*.adif *.adi)"));
    //qDebug() << filename << " returned " << endl;
}

void Data::setFilename( QString fname )
{
    filename = fname;
}

QString Data::getFilename()
{
    return filename;
}
void Data::setDirname( QString dname )
{
    dirname = dname;
}

QString Data::getDirname()
{
    return dirname;
}


void Data::updateHeaders()
{
//    *selectedhdr = QStringList(coldat->getResult());
}

int Data::getRows()
{
    qDebug() << rows;
    return rows;
}

int Data::getColumns()
{
    qDebug() << columns;
    return columns;
}

void Data::removeRow( QModelIndex idx )
{
    int row = idx.row();
    model->removeRow(row, idx );

}


