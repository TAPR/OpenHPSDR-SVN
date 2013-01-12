#ifndef ADDRESSDIALOG_H
#define ADDRESSDIALOG_H

#include <QDialog>
#include <QDebug>
#include "board.h"

namespace Ui {
class AddressDialog;
}

class AddressDialog : public QDialog
{
    Q_OBJECT
    
public:
    explicit AddressDialog(QWidget *parent = 0);
    ~AddressDialog();
    void setIPaddress(Board *m );
    void setIPWrite( int *addr );
    void setMACaddress(Board *m );
    void writeIPAddress(int *addr);

signals:
    void writeIP();

private:
    Ui::AddressDialog *ui;

};

#endif // ADDRESSDIALOG_H
