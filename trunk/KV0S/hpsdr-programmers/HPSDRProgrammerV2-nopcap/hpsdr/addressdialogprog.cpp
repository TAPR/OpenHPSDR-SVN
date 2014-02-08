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
    connect(ui->dhcpButton,SIGNAL(clicked()),this, SLOT(dhcpIPwrite()));
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
    i1 = ip1.toInt();
    ui->IPLineEdit1->setText( ip1 );
    ip2 = m->toIPPartString( 2 );
    i2 = ip2.toInt();
    ui->IPLineEdit2->setText( ip2 );
    ip3 = m->toIPPartString( 3 );
    i3 = ip3.toInt();
    ui->IPLineEdit3->setText( ip3 );
    ip4 = m->toIPPartString( 4 );
    ui->IPLineEdit4->setText( ip4 );
    ui->writeButton->setDisabled(false);
}

void AddressDialog::getMACaddress(Board *m)
{
    qDebug() << "in getMACAddress" << m->toMACString();

}

/* Previous version
 *
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
    //else if (new_addr.sin_addr.s_addr >= class_e_addr.sin_addr.s_addr) {
    //    invalidIPAddress( QString("Class E address space restricted") );
     //   return;
    //}

    // Multicast address not allowed
    //else if (new_addr.sin_addr.s_addr >= multicast_addr.sin_addr.s_addr) {
    //    invalidIPAddress( QString("multicast address space restricted") );
    //    return;
    //}

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

*
*/

void AddressDialog::setupIPwrite()
{
    qDebug() << "in writeIPsetup";


    QString IPstring =  ui->IPLineEdit1->text() + '.' +
                        ui->IPLineEdit2->text() + '.' +
                        ui->IPLineEdit3->text() + '.' +
                        ui->IPLineEdit4->text() ;

    bool ok1, ok2, ok3, ok4;

    int o1 = (ui->IPLineEdit1->text().toInt(&ok1, 10));
    int o2 = (ui->IPLineEdit2->text().toInt(&ok2, 10));
    int o3 = (ui->IPLineEdit3->text().toInt(&ok3, 10));
    int o4 = (ui->IPLineEdit4->text().toInt(&ok4, 10));

    // Range test of interger values,  number between 0 and 255

    if ( ((ok1 != true) || o1 < 0 || o1 > 255) || ((ok2 != true) || o2 < 0 || o2 > 255) ||
         ((ok3 != true) || o3 < 0 || o3 > 255) || ((ok4 != true) || o4 < 0 || o4 > 255) )
    {
         invalidIPAddress( "Integer values out of range for IP4 address!" );
         return;
    }

    sockaddr_in new_addr;         // address type int to dialog
    sockaddr_in broadcast_addr;   // 255.255.255.255

    new_addr.sin_addr.s_addr       = ntohl (inet_addr( IPstring.toUtf8().constData() ));
    broadcast_addr.sin_addr.s_addr = ntohl (inet_addr("255.255.255.255"));

    // restrict fixed address in localhost space
    if (o1 == 127)
    {
        invalidIPAddress( QString("Localhost address space is reserved"));
        return;
    }

    // restrict fixed address in localhost space
    else if (o1 == 224)
    {
        invalidIPAddress( QString("Multicast address space is reserved"));
        return;
    }

    // restrict fixed address in adhoc address space
    else if ((o1 == 169) || ((o1 == 169) && (o2 == 254))) {
        invalidIPAddress( QString("Ad Hoc address space not suitable for fixed address") );
        return;
    }

    // restrict fixed address in restricted IPv4 address space
    // broadcast is allowed to start DHCP
    else if ((o1 >= 240) && !(new_addr.sin_addr.s_addr == broadcast_addr.sin_addr.s_addr) ) {
        invalidIPAddress( QString("This address space is restricted") );
        return;
    }

    // restrict fixed address starting with 0
    // address 0.0.0.0 is allowed to start DHCP
    else if ((o1 == 0) && !(new_addr.sin_addr.s_addr == 0) ) {
        invalidIPAddress( QString("This address space is restricted") );
        return;
    }


    // Check for empty address
    if (new_addr.sin_addr.s_addr == 0)
    {
        dhcpIPAddress( "New Address will set on DHCP");
    }

    // Check for broadcast address
    else if (new_addr.sin_addr.s_addr == broadcast_addr.sin_addr.s_addr )
    {
        dhcpIPAddress( "New Address will set on DHCP");
    }

    // Recommended fixed address is in current router address space
    else if( (o1 == i1) && (o2 == i2) && (o3 == i3) )
    {
        okIPAddress(QString("New Address is in current %0.%1.%2.x address space").arg(o1).arg(o2).arg(o3));
    }

    else if( (o1 == 192) && (o2 == 168) )
    {
        okIPAddress( "New Address is in the 192.168/16 address space");
    }

    else if( (o1 == 172) && ( o2 >= 0 && o2 <= 31 ) )
    {
        okIPAddress( "New Address is in the 172.16/12 address space");
    }

    // If using another address space restrict private address space addresses
    else if( (o1 == 10)  )
    {
         okIPAddress( "New Address is in the 10/8 address space");
    }

    // Drop through proposed address did not meet standard assumptions
    else{
        QString str = QString("Address will space changed to:\t%0\nYou will have to move the board to a router with this address.").arg(IPstring);
        int ret = QMessageBox::critical(this, tr("HPSDRBootloader"),QString("Potential ERROR: %0\n\nDo you want to used the IP address?").arg(str),  QMessageBox::Yes|QMessageBox::No, QMessageBox::No);
        if( ret == QMessageBox::Yes ){
            qDebug() << "answered Yes";
            return;
        }else{
            qDebug() << "answered No";
            return;
        }


    }

    emit writeIP();
    this->close();
    return;
}


void AddressDialog::dhcpIPwrite()
{
    qDebug() << "in dhcpIPsetup";

    ui->IPLineEdit1->setText("0");
    ui->IPLineEdit2->setText("0");
    ui->IPLineEdit3->setText("0");
    ui->IPLineEdit4->setText("0");

    dhcpIPAddress( "Switching to DHCP, devices may need a restart.");

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
    QMessageBox::warning(this, tr("HPSDRBootloader"),QString("WARNING: %0\n\nThe new IP address will uses DHCP").arg(str),  QMessageBox::Close);
}

void AddressDialog::okIPAddress(QString str)
{
    qDebug() << QString("Message: %0").arg(str);
    QMessageBox::warning(this, tr("HPSDRBootloader"),QString("Message: %0\n\nThe new IP address will be uses").arg(str),  QMessageBox::Close);
}
