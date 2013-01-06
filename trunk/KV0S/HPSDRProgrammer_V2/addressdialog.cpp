#include "addressdialog.h"
#include "ui_addressdialog.h"

AddressDialog::AddressDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::AddressDialog)
{
    ui->setupUi(this);

    connect(ui->closeButton,SIGNAL(clicked()),this,SLOT(close()));
}

AddressDialog::~AddressDialog()
{
    delete ui;
}
