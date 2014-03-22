#ifndef SAVEDATA_H
#define SAVEDATA_H

#include <QObject>
#include <QStandardItemModel>
#include <QString>
#include <QDebug>
#include <QFileDialog>
#include <QTextStream>
#include <QDateTime>
#include <QRegExp>
#include "about.h"

class saveData : public QWidget
{
    Q_OBJECT
public:
    explicit saveData(QWidget *parent = 0);
    void setModel( QStandardItemModel * mdl);
    QDateTime * date;
    void setRows( int row );
    void setColumns( int col );
    int getRows();
    int getColumns();

signals:

public slots:
    void writeData();
    void writeXmlData( QString filename );
    void writeADIFData( QString filename );

private:
    QString filename;
    QStandardItemModel * model;
    int rows;
    int columns;

};

#endif // SAVEDATA_H
