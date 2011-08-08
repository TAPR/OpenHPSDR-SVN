#include "loaddata.h"

loadData::loadData(QWidget *parent) :
    QWidget(parent)
{
    filename = "";
    hdr = new QStringList();
    selectedhdr = new QStringList();
}

loadData::~loadData()
{
    delete hdr;
}

QString* loadData::readData()
{
    QString *filetype = new QString();
    filename =  QFileDialog::getOpenFileName(this, tr("Open File"),"",tr("Files (*.xml *.adif *.adi)"));
     //qDebug() << filename << " returned " << endl;
    if( filename.contains("xml"))
    {
        readXMLHeader( filename );
        *filetype = "XML";
     }else{
        readADIFHeader( filename );
        *filetype = "ADIF";
    }
    return filetype;
}

void loadData::readXMLHeader( QString filename )
{


   QFile *file = new QFile(filename);
   if( !file->open(QFile::ReadOnly) )
   {
       qDebug() << "Open failed." << endl;
   }else{
       qDebug() << "Open success." << endl;
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
       qDebug() << "Parsing failed." << endl;
   }
   hdr = loghandler->getHeaders();
   hdr->removeDuplicates();
   qDebug() << hdr->size() << *hdr << endl;
   columns = hdr->size();
   *selectedhdr = QStringList(*hdr);
   qDebug() << *selectedhdr << endl;
   file->close();
}


void loadData::readADIFHeader( QString filename )
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
       *selectedhdr = QStringList(*hdr);
       //qDebug() << *selectedhdr;
       data.close();
    }

}

void loadData::readADIFData( QStandardItemModel *model)
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
                model->setItem(row,selectedhdr->indexOf(*name),it);
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
         qDebug() << " Could not open file " << filename << endl;
  }
  data.close();
}

void loadData::readXMLData( QStandardItemModel *model)
{
    QFile *file = new QFile(filename);
    if( !file->open(QFile::ReadOnly) )
    {
        qDebug() << "Open failed." << endl;
    }else{
        qDebug() << "Open success." << endl;
    }
    QXmlSimpleReader logxmlReader;
    QXmlInputSource *source = new QXmlInputSource( file );

    logHandler *loghandler = new logHandler( model );
    loghandler->readHeader( false );
    loghandler->setHeader( selectedhdr );
    logxmlReader.setContentHandler(loghandler);
    logxmlReader.setErrorHandler(loghandler);


    bool ok = logxmlReader.parse(source);

    if( !ok )
    {
        qDebug() << "Parsing failed." << endl;
    }
    //qDebug() << (model->item(1,1))->text();
    model = (loghandler->getModel() );
    qDebug() << model->item(1,1);
    rows = loghandler->getRows();
    emit refresh();
    file->close();
}

void loadData::queryFilename()
{
    filename =  QFileDialog::getOpenFileName(this, tr("Open File"),"",tr("Files (*.adif *.adi)"));
    //qDebug() << filename << " returned " << endl;
}

void loadData::setFilename( QString fname )
{
    filename = fname;
}

QString loadData::getFilename()
{
    return filename;
}

QStringList loadData::getHeaders()
{
    //qDebug() << "in getHeader";
    //qDebug() << *selectedhdr;
    return QStringList(*selectedhdr);
}

void loadData::updateHeaders()
{
//    *selectedhdr = QStringList(coldat->getResult());
}

int loadData::getRows()
{
    qDebug() << rows;
    return rows;
}

int loadData::getColumns()
{
    qDebug() << columns;
    return columns;
}
