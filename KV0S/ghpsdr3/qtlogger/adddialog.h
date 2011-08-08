#ifndef ADDDIALOG_H
#define ADDDIALOG_H

#include <QDialog>
#include <QTime>
#include <QDate>
#include <QTimer>
#include <QDebug>
#include <QStandardItemModel>
#include <QStringList>
#include <QString>


namespace Ui {
    class addDialog;
}

class addDialog : public QDialog
{
    Q_OBJECT

public:
    explicit addDialog(QWidget *parent = 0);
    ~addDialog();
    void loadmodeComboBox( QStringList *mode);
    void loadcountryComboBox( QStringList *country);
    QDate *date;
    QTime *time;
    Ui::addDialog *ui;
    void setFrequency( QString *freq );
    void setHeader( QStringList *hdr );
    void setModel( QStandardItemModel *model );
    QStandardItemModel* localmodel;
    QStringList* selectedhdr;

private slots:
    void reset();
    void updateTime();
    void addContact();

};

#endif // ADDDIALOG_H
