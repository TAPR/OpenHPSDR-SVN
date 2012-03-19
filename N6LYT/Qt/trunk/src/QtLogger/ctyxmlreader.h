#ifndef CTYXMLREADER_H
#define CTYXMLREADER_H

#include <QStandardItemModel>
#include <QFile>
#include <QXmlStreamReader>
#include <QStandardItem>

class CtyXmlReader
{
public:
    CtyXmlReader(QStandardItemModel *model);
    bool read (QFile *file);
    QString errorString () const;
    //QStandardItemModel *myModel;

private:
    void readResources (const QString filename);
    void readCountry ();
    void readSubdivision ();
    QList <QStandardItem *> countryList;
    QXmlStreamReader xml;
    QStandardItemModel *myModel;
    QFile *myFile;
    QString countrycode;
    QStringList prefix;
    QString codestr;
    QString country;
    QString subdivision;
};

#endif // CTYXMLREADER_H
