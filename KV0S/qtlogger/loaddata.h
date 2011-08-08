#ifndef LOADDATA_H
#define LOADDATA_H

#include <QObject>
#include <QFile>
#include <QTextStream>
#include <QDebug>
#include <QString>
#include <QFileDialog>
#include <QStandardItemModel>
#include "adif.h"
#include "loghandler.h"


class loadData : public QWidget
{
    Q_OBJECT

public:
    explicit loadData(QWidget *parent = 0);
    ~loadData();
    QString getFilename();
    void setFilename(QString fname);
    QStringList getHeaders();
    int getRows();
    int getColumns();


signals:
    void refresh();

public slots:
    QString* readData();
    void readADIFHeader( QString filename );
    void readXMLHeader( QString filename );
    void readADIFData( QStandardItemModel * model);
    void readXMLData( QStandardItemModel * model);
    void queryFilename();
    void updateHeaders();


private:
    QString filename;
    QStringList *hdr;
    QStringList *selectedhdr;
    int rows;
    int columns;
};

#endif // LOADDATA_H
