#include "addressdialogboot.h"
#include "ui_addressdialogboot.h"

AddressDialog::AddressDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::AddressDialog)
{
    ui->setupUi(this);

    ui->writeButton->setDisabled(true);

    connect(ui->closeButton,SIGNAL(clicked()),this,SLOT(close()));
    connect(ui->writeButton,SIGNAL(clicked()),this, SLOT(setupIPwrite()));
    connect(ui->readMACButton,SIGNAL(clicked()),this,SLOT(readMAC()));
    connect(ui->readIPButton,SIGNAL(clicked()),this,SLOT(readIP()));
}

AddressDialog::~AddressDialog()
{
    delete ui;
}



void AddressDialog::getIPaddress(Board *m)
{
    QString ip1, ip2, ip3, ip4;
    qDebug() << "in getIPAddress" << m->toIPString();
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
    qDebug() << "in getMACAddress" << m->toMACString();
    ui->MACLabel->setText( m->toMACString() );
}

void AddressDialog::setupIPwrite()
{
    qDebug() << "in writeIPsetup";

    // IP error range test code suggested by Andrea Montefusco

    QString errorMessage;
    QString IPstring =  ui->IPLineEdit1->text() + '.' +
                        ui->IPLineEdit2->text() + '.' +
                        ui->IPLineEdit3->text() + '.' +
                        ui->IPLineEdit4->text() ;
    bool ok1, ok2, ok3, ok4;

    int o1 = (ui->IPLineEdit1->text().toInt(&ok1, 10));
    int o2 = (ui->IPLineEdit2->text().toInt(&ok2, 10));
    int o3 = (ui->IPLineEdit3->text().toInt(&ok3, 10));
    int o4 = (ui->IPLineEdit4->text().toInt(&ok4, 10));

    // Range test of interger values

    if ( ((ok1 != true) || o1 < 0 || o1 > 255) || ((ok2 != true) || o2 < 0 || o2 > 255) ||
         ((ok3 != true) || o3 < 0 || o3 > 255) || ((ok4 != true) || o4 < 0 || o4 > 255) )
    {
         errorMessage = "Integer values out of range for IP4 address!";
         invalidIPAddress( errorMessage );
         return;
    }

    sockaddr_in new_addr;
    sockaddr_in class_e_addr;
    sockaddr_in multicast_addr;
    sockaddr_in broadcast_addr;

    // convert in binary form for further checks
    new_addr.sin_addr.s_addr       = ntohl (inet_addr( IPstring.toUtf8().constData() ));
    class_e_addr.sin_addr.s_addr   = ntohl (inet_addr("240.0.0.0"));
    multicast_addr.sin_addr.s_addr = ntohl (inet_addr("224.0.0.0"));
    broadcast_addr.sin_addr.s_addr = ntohl (inet_addr("255.255.255.255"));



    // Check for localhost address
    if (o1 == 127)
    {
        invalidIPAddress( QString("localhost address reserved"));
        return;
    }

    // Class E address space not allowed
    else if (new_addr.sin_addr.s_addr >= class_e_addr.sin_addr.s_addr) {
        invalidIPAddress( QString("Class E address space restricted") );
        return;
    }

    // Multicast address not allowed
    else if (new_addr.sin_addr.s_addr >= multicast_addr.sin_addr.s_addr) {
        invalidIPAddress( QString("multicast address space restricted") );
        return;
    }

    // Check for empty address
    if (new_addr.sin_addr.s_addr == 0)
    {
        dhcpIPAddress( "New Address empty");
    }

    // Check for broadcast address
    else if (new_addr.sin_addr.s_addr == broadcast_addr.sin_addr.s_addr )
    {
        dhcpIPAddress( "New Address is a broadcast address");
    }

    emit writeIP();
    this->close();
    return;
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
    ui->MACLabel->setText( mac );
}

void AddressDialog::setIPaddress(QStringList ipaddress)
{
    qDebug() << "in setIPAddress" << ipaddress;
    ui->IPLineEdit1->setText( ipaddress[0] );
    ui->IPLineEdit2->setText( ipaddress[1] );
    ui->IPLineEdit3->setText( ipaddress[2] );
    ui->IPLineEdit4->setText( ipaddress[3] );
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

void AddressDialog::invalidIPAddress(QString str)
{
    qDebug() << QString("ERROR: %0").arg(str);
    QMessageBox::warning(this, tr("HPSDRBootloader"),QString("ERROR: %0\n\nThe new IP address will not be used").arg(str),  QMessageBox::Close);
}

void AddressDialog::dhcpIPAddress(QString str)
{
    qDebug() << QString("WARNING: %0").arg(str);
    QMessageBox::warning(this, tr("HPSDRBootloader"),QString("WARNING: %0\n\nThe new IP address will use DHCP").arg(str),  QMessageBox::Close);
}



