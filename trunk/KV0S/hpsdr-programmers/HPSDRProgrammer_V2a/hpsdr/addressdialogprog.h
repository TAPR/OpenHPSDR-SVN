#ifndef ADDRESSDIALOG_H
#define ADDRESSDIALOG_H

#include <QDialog>
#include <QDebug>
#include <QStringList>
#include "../Programmer/board.h"

namespace Ui {
class AddressDialog;
}

class AddressDialog : public QDialog
{
    Q_OBJECT
    
public:
    explicit AddressDialog(QWidget *parent = 0);
    ~AddressDialog();

    void getIPaddress(Board *m );
    void getMACaddress(Board *m );
    void getNewIPAddress(QStringList *addr );
    Ui::AddressDialog *ui;
    void writeEnabled();
    void setMACaddress(QString mac);

signals:
    void writeIP();
    void readIPAddress();
    void readMACAddress();

private:

private slots:
    void setupIPwrite();
    void readIP();
    void readMAC();
};

#endif // ADDRESSDIALOG_H
