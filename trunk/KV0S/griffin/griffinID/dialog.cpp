#include "dialog.h"
#include "ui_dialog.h"

Dialog::Dialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::Dialog)
{
    QString path;
    path = QApplication::applicationDirPath() + "/help/";
    ui->setupUi(this);
    ui->textBrowser->setSearchPaths(QStringList() << "./help/" << path );
    ui->textBrowser->setSource(QUrl("griffinid.html"));
    oldsource << QUrl("griffinid.html");

    connect(ui->quitButton,SIGNAL(clicked()),this,SLOT(close()));
    connect(ui->homeButton,SIGNAL(clicked()),this,SLOT(home()));
    connect(ui->backButton,SIGNAL(clicked()),this,SLOT(back()));
    connect(ui->textBrowser,SIGNAL(sourceChanged(QUrl)),this,SLOT(updatesource(QUrl)));

}

Dialog::~Dialog()
{
    delete ui;
}

void Dialog::home()
{
    ui->textBrowser->setSource(QUrl("griffinid.html"));
}

void Dialog::back()
{
    if( oldsource.isEmpty() ){
        ui->textBrowser->setSource(QUrl("griffinid.html"));
    }else{
        oldsource.removeLast();
        ui->textBrowser->setSource( oldsource.last());
    }

}

void Dialog::updatesource( QUrl newsource )
{
    oldsource << newsource;
}
