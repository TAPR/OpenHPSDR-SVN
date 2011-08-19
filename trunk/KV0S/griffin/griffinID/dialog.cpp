#include "dialog.h"
#include "ui_dialog.h"

Dialog::Dialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::Dialog)
{
    ui->setupUi(this);
    ui->textBrowser->setSearchPaths(QStringList("./" "./help/"));
    ui->textBrowser->setSource(QUrl("griffinid.html"));
}

Dialog::~Dialog()
{
    delete ui;
}
