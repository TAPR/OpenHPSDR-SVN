#ifndef ADDRESSDIALOG_H
#define ADDRESSDIALOG_H

#include <QDialog>
#include <QDebug>
#include <QStringList>
#include <QString>
#include <QMessageBox>
#include "../Programmer/board.h"

#include <sys/types.h>
#ifndef __WIN32
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#endif



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
    void setIPaddress(QStringList ipaddress);
    Ui::AddressDialog *ui;
    void writeEnabled();
    void setMACaddress(QString mac);
    void invalidIPAddress(QString str);

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
