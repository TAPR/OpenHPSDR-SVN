#ifndef ADDRESSDIALOG_H
#define ADDRESSDIALOG_H

#include <QDialog>

namespace Ui {
class AddressDialog;
}

class AddressDialog : public QDialog
{
    Q_OBJECT
    
public:
    explicit AddressDialog(QWidget *parent = 0);
    ~AddressDialog();
    
private:
    Ui::AddressDialog *ui;
};

#endif // ADDRESSDIALOG_H
