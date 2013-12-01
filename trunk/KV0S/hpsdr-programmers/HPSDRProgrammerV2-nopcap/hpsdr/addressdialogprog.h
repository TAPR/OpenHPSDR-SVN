#ifndef ADDRESSDIALOG_H
#define ADDRESSDIALOG_H

#include <QDialog>
#include <QDebug>
#include <QStringList>
#include <QString>
#include <QMessageBox>
#include "../Programmer/board.h"


#include <sys/types.h>
#ifdef __WIN32
#include <winsock2.h>
#else
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
    Ui::AddressDialog *ui;
    void writeEnabled();
    void setMACaddress(QString mac);
    void invalidIPAddress(QString str);
    void dhcpIPAddress(QString str);
    void okIPAddress(QString str);

signals:
    void writeIP();
    void readIPAddress();
    void readMACAddress();

private:
    int i1;
    int i2;
    int i3;

private slots:
    void setupIPwrite();
    void dhcpIPwrite();
    void readIP();
    void readMAC();

};

#endif // ADDRESSDIALOG_H
