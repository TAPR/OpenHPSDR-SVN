#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "sender.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    int val;
    QString value;
    ui->setupUi(this);

    sender = new Sender;

    connect(ui->horizontalSlider,SIGNAL(sliderMoved(int)),this,SLOT(updateFreq(int)));
    connect(ui->comboBox,SIGNAL(activated(QString)),this,SLOT(updateMode(QString)));

}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::updateFreq(int val)
{
   float value = (float)val;
   float freq = 14.0 + ((value/100) * 0.350);
   ui->label->setText(QString("%1").number(freq,'f',6));
   QByteArray message = "frequency "+QByteArray::number(freq);
   sender->broadcastDatagram( message );
}

void MainWindow::updateMode(QString value)
{
    qDebug() << value;
    QByteArray message = "mode ";
    message.append(value);

    sender->broadcastDatagram( message );
}
