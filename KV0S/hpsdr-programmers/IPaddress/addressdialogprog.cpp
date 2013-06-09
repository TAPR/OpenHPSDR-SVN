#include "addressdialogprog.h"
#include "ui_addressdialogprog.h"

AddressDialog::AddressDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::AddressDialog)
{
    ui->setupUi(this);

    ui->writeButton->setDisabled(true);

    connect(ui->closeButton,SIGNAL(clicked()),this,SLOT(close()));
    connect(ui->writeButton,SIGNAL(clicked()),this, SLOT(setupIPwrite()));

}

AddressDialog::~AddressDialog()
{
    delete ui;
}



void AddressDialog::getIPaddress(Board *m)
{
    QString ip1, ip2, ip3, ip4;
    qDebug() << "in getIPAddress"; //<< m->toIPString();
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

void AddressDialog::getMACaddress(Board *m)
{
    qDebug() << "in getMACAddress"; // << m->toMACString();

}

void AddressDialog::setupIPwrite()
{
    qDebug() << "in writeIPsetup";
    emit writeIP();
    this->close();
}


void AddressDialog::getNewIPAddress( QStringList *addr )
{
    qDebug() << "in writeIPsetup";
    addr->append(ui->IPLineEdit1->text());
    addr->append(ui->IPLineEdit2->text());
    addr->append(ui->IPLineEdit3->text());
    addr->append(ui->IPLineEdit4->text());

}

void AddressDialog::writeEnabled()
{
    ui->writeButton->setDisabled(false);
}


void AddressDialog::setMACaddress(QString mac)
{
    qDebug() << "in setMACAddress" << mac;

}

void AddressDialog::readMAC()
{
    qDebug() << "in readMAC";
    emit readMACAddress();
}

void AddressDialog::readIP()
{
    qDebug() << "in readIP";
    emit readIPAddress();
}




