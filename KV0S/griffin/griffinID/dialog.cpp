#include "dialog.h"
#include "ui_dialog.h"

Dialog::Dialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::Dialog)
{
    QString path;
    path = QApplication::applicationDirPath() + "/help";
    ui->setupUi(this);
    ui->textBrowser->setSearchPaths(QStringList() << "./" << path );
    ui->textBrowser->setSource(QUrl("griffinid.html"));
}

Dialog::~Dialog()
{
    delete ui;
}
