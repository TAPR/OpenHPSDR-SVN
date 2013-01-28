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

signals:
    void writeIP();

private:

private slots:
    void setupIPwrite();
};

#endif // ADDRESSDIALOG_H
