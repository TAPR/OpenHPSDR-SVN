#include "lastcontact.h"
#include "ui_lastcontact.h"

lastContact::lastContact(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::lastContact)
{
    ui->setupUi(this);

    proxyModel = new QSortFilterProxyModel(this);

    proxyModel->setFilterKeyColumn(0);
    proxyModel->setDynamicSortFilter( true );
    ui->treeView->setModel( proxyModel );

    connect(ui->closeButton,SIGNAL(clicked()),this,SLOT(close()));

}

lastContact::~lastContact()
{
    delete ui;
}


void lastContact::setModel( QStandardItemModel *model )
{
    contactModel = model;
    proxyModel->setSourceModel( contactModel );

}

void lastContact::setCallLabel( QString call )
{
    ui->label->setText(QString("Search Call:%1").arg(call.remove("^")));
}
