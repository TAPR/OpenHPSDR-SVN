#include "addressdialog.h"
#include "ui_addressdialog.h"

AddressDialog::AddressDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::AddressDialog)
{
    ui->setupUi(this);

    ui->writeButton->setDisabled(true);

    connect(ui->closeButton,SIGNAL(clicked()),this,SLOT(close()));
    connect(ui->writeButton,SIGNAL(clicked()),this, SLOT(writeIPaddress()));
}

AddressDialog::~AddressDialog()
{
    delete ui;
}

void AddressDialog::setIPaddress(Board *m)
{
    QString ip1, ip2, ip3, ip4;
    qDebug() << "in setIPAddress" << m->toIPString();
    ip1 = m->toIPPartString( 1 );
    ui->IPLineEdit1->setText( ip1 );
    ip2 = m->toIPPartString( 2 );
    ui->IPLineEdit2->setText( ip2 );
    ip3 = m->toIPPartString( 3 );
    ui->IPLineEdit3->setText( ip3 );
    ip4 = m->toIPPartString( 4 );
    ui->IPLineEdit4->setText( ip4 );
    ui->writeButton->setDisabled(false);
}

void AddressDialog::setMACaddress(Board *m)
{
    qDebug() << "in setMACAddress" << m->toMACString();
    ui->MACLabel->setText( m->toMACString() );
}

void AddressDialog::setIPWrite( int *addr )
{
    addr[0]=ui->IPLineEdit1->text().toInt();
    addr[1]=ui->IPLineEdit2->text().toInt();
    addr[2]=ui->IPLineEdit3->text().toInt();
    addr[3]=ui->IPLineEdit4->text().toInt();

    emit writeIP();

}
