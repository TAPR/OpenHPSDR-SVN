#ifndef ADDDIALOG_H
#define ADDDIALOG_H

#include <QDialog>
#include <QTime>
#include <QDate>
#include <QTimer>
#include <QHash>
#include <QCompleter>
#include <QHashIterator>

#include "data.h"
#include "range.h"


namespace Ui {
    class addDialog;
}

class addDialog : public QDialog
{
    Q_OBJECT

public:
    explicit addDialog(QWidget *parent = 0 );
    ~addDialog();
    QDate *date;
    QTime *time;
    QHash <QString, QString> record;
    void setFrequency( QString freq );
    void setMode( QString modestr );
    QString getBand( QString fstr );
    void loadmodeComboBox(QStringList *modes);
    void loadcountryComboBox(QStringList *country);
    void loadsubdivisionsComboBox(QStringList *subdivisions);
    void loadbandsData( QHash <QString, Range> band );
    QHash <QString, Range> bandData;
    void callreset();

private:
    Ui::addDialog *ui;

signals:
    void newdata();

private slots:
    void reset();
    void addContact();
    void updateTime();
    void updateCall();
};

#endif // ADDDIALOG_H
