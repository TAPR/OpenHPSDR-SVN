#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "frequencysender.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    int val;
    QString value;
    ui->setupUi(this);

    sender = new FrequencySender( this );

    connect(ui->horizontalSlider,SIGNAL(sliderMoved(int)),this,SLOT(updateFreq(int)));
    connect(ui->comboBox,SIGNAL(activated(QString)),this,SLOT(updateMode(QString)));

}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::updateFreq(int val)
{
   QString strFrequency;
   long freq = 14000000 + long(val * 1000);
   strFrequency.sprintf("%ld.%03ld.%03ld",freq/1000000,freq%1000000/1000,freq%1000);
   ui->label->setText(strFrequency);
   QByteArray message = QByteArray("frequency ");
   message.append(strFrequency);
   sender->broadcastDatagram( message );
}

void MainWindow::updateMode(QString value)
{
    qDebug() << value;
    QByteArray message = "mode ";
    message.append(value);

    sender->broadcastDatagram( message );
}
