#include "savedata.h"

saveData::saveData(QWidget *parent) :
    QWidget(parent)
{
    date = new QDateTime();
}

void saveData::setModel( QStandardItemModel * mdl )
{
    model = mdl;
}

void saveData::setRows( int row )
{
    rows = row;
}

void saveData::setColumns( int col )
{
    columns = col;
}

void saveData::writeData()
{
    filename =  QFileDialog::getSaveFileName(this, tr("Save File"),"",tr("Files (*.xml *.adif *.adi)"));
    qDebug() << filename << " returned " << endl;

    if( filename.contains("xml"))
    {
        writeXmlData( filename );
    }else{
        writeADIFData( filename );
    }
}

void saveData::writeADIFData( QString filename )
{

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

      int rows = getRows();
      int cols = getColumns();
      for (int i = 0; i < rows; ++i)
      {
          for (int j = 0; j < cols; ++j)
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

void saveData::writeXmlData( QString filename )
{
    QFile data(filename);
    if (data.open(QIODevice::WriteOnly)) {
      QTextStream stream(&data);
      stream << QString("<?xml version=\"1.0\" encoding=\"UTF-8\"?>") << endl;
      stream << QString("<?xml-stylesheet type=\"text/xsl\" href=\"qtlogger.xsl\" ?>") << endl;
      stream << QString("<AmateurRadioLog>") << endl;
      stream << QString("  <header>") << endl;
      stream << QString("    <file>") << endl;
      stream << QString("      <author>Dave Larsen KV0S</author>") << endl;
      stream << QString("      <info>For more information see: http://openhpsdr.org</info>") << endl;
      stream << QString("      <created>");
      stream << (date->currentDateTimeUtc()).toString("yyyyMMdd-hhmm");
      stream << QString(" UTC</created>") << endl;
      stream << QString("      <program>QtLogger</program>") << endl;
      stream << QString("      <programversion>%1</programversion>").arg( "1.0.1" ) << endl;  // fix to mainWindow version
      stream << QString("    </file>") << endl;
      stream << QString("  </header>") << endl;
      stream << QString("  <contacts>") << endl;
      int rows = getRows();
      int cols = getColumns();
      for (int i = 0; i < rows; ++i)
      {
          stream << QString("   <record>") << endl;
          for (int j = 0; j < cols; ++j)
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

int saveData::getRows()
{
    qDebug() << rows;
    return ( rows );
}

int saveData::getColumns()
{
    qDebug() << columns;
    return ( columns );
}
