#include "aboutdialog.h"
#include "ui_aboutdialog.h"

AboutDialog::AboutDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::AboutDialog)
{
    ui->setupUi(this);

    connect(ui->OKButton,SIGNAL(clicked()),this,SLOT(close()));
}

AboutDialog::~AboutDialog()
{
    delete ui;
}

void AboutDialog::setVersion(QString version, QString release)
{
    ui->versionLabel->setText(QString("%0 - %1").arg(version).arg(release));
}
