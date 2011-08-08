#include "adddialog.h"
#include "ui_addDialog.h"

addDialog::addDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::addDialog)
{
    ui->setupUi(this);
    date = new QDate();
    time = new QTime();

    //localmodel = parent->model;
    selectedhdr = new QStringList();

    reset();

    QTimer *timer = new QTimer(this);
    connect(timer, SIGNAL(timeout()), this, SLOT(updateTime()));
    timer->start(1000);

    ui->callsignLineEdit->setInputMask(">NNNNNN; ");

    connect(ui->cancel_pushButton,SIGNAL(clicked()),this,SLOT(close()));
    connect(ui->reset_pushButton,SIGNAL(clicked()),this,SLOT(reset()));
    connect(ui->add_pushButton,SIGNAL(clicked()),this,SLOT(addContact()));

    this->show();
}

addDialog::~addDialog()
{
    delete ui;
}

void addDialog::reset()
{
    // This function should reset all the field in the add Dialog
    ui->date_lineEdit->setText(date->currentDate().toString());
    ui->time_lineEdit->setText(time->currentTime().toString());
    ui->callsignLineEdit->clear();
    ui->name_lineEdit->clear();
    ui->qth_lineEdit->clear();

}

void addDialog::addContact()
{
    qDebug() << "in addContact";
    QString contact = ui->contact_lineEdit->text();
    int numb = contact.toInt() + 1;
    ui->contact_lineEdit->setText( QString("%1").number(numb) );
    QStandardItem *it = new QStandardItem(ui->callsignLineEdit->text());
    qDebug() << it->text();
    qDebug() << *selectedhdr;
    *selectedhdr << "CALL";
    localmodel->setItem( contact.toInt(), selectedhdr->indexOf("CALL"), it );

    this->close();

}

void addDialog::updateTime()
{
    ui->time_lineEdit->setText(time->currentTime().toString());
}

void addDialog::loadmodeComboBox(QStringList *modes)
{
    ui->modeComboBox->addItems(*modes);
}

void addDialog::loadcountryComboBox(QStringList *country)
{
    ui->countryComboBox->addItems(*country);
}

void addDialog::setFrequency(QString *frequency)
{
    ui->freqLineEdit->setText(*frequency);
}

void addDialog::setHeader(QStringList *hdr)
{
    selectedhdr = hdr;
}

void addDialog::setModel(QStandardItemModel *model)
{
    localmodel = model;
}

