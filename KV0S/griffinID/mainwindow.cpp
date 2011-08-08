#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QDebug>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);


    QCoreApplication::setOrganizationName("openhpsdr" );
    QCoreApplication::setOrganizationDomain("openhpsdr.org" );
    QCoreApplication::setApplicationName("griffinID");



    this->setWindowTitle("Griffin ID program");
    ui->call_lineEdit->setInputMask(">NN9NNN");

    About *abd = new About();
    abd->setVersion( QString( "0.0.1" ) );

    loadGridBox();
    loadPowerBox();

    ui->call_lineEdit->setText( settings.value("call").toString() );
    ui->grid_comboBox->setCurrentIndex( settings.value("grid").toInt() );
    ui->power_comboBox->setCurrentIndex( settings.value("power").toInt() );

    connect( ui->actionQuit,SIGNAL(triggered()),this,SLOT(close()));
    connect(ui->actionSave_as,SIGNAL(triggered()),this,SLOT(writeString()));
    connect(ui->actionAbout,SIGNAL(triggered()),abd,SLOT(aboutMessage()));
}

MainWindow::~MainWindow()
{
    settings.setValue("call", ui->call_lineEdit->text());
    settings.setValue("grid", ui->grid_comboBox->currentIndex());
    settings.setValue("power", ui->power_comboBox->currentIndex());
    delete ui;
}

void MainWindow::loadGridBox()
{
    QStringList grid;
    QString gridcode;
    for( int i=0; i<18; i++ )
    {
        int code = 65 + i;
        gridcode = QString( "####" );
        gridcode.replace( 0, 1, QChar(code) );
        for( int j=0; j<18; j++ )
        {
            int code1 = 65 + j;
            gridcode.replace( 1, 1, QChar(code1) );
            for( int k=0; k<10; k++ )
            {
                int code2 = 48 + k;
                gridcode.replace( 2, 1, QChar(code2) );
                for( int l=0; l<10; l++ )
                {
                    int code3 = 48 + l;
                    gridcode.replace( 3, 1, QChar(code3) );
                    grid << gridcode;
                }
            }
        }
    }
    ui->grid_comboBox->addItems( grid );

}

void MainWindow::loadPowerBox()
{
    QStringList power;
    QString powercode;
    powercode = QString( "##" );
    for( int i=0; i<7; i++ )
    {
        int code = 48 + i;
        powercode.replace( 0, 1, QChar(code) );
        for( int l=0; l<10; l++ )
        {
            int code1 = 48 + l;
            powercode.replace( 1, 1, QChar(code1) );
            power << powercode;

        }
    }
    ui->power_comboBox->addItems( power );
}

QString MainWindow::morsechar( QChar ch )
{
    //ch.toUpper();
    QHash <QChar, QString> morse;
    morse['A'] = "10111";
    morse['B'] = "111010101";
    morse['C'] = "11101011101";
    morse['D'] = "1110101";
    morse['E'] = "1";
    morse['F'] = "101011101";
    morse['G'] = "111011101";
    morse['H'] = "1010101";
    morse['I'] = "101";
    morse['J'] = "1011101110111";
    morse['K'] = "111010111";
    morse['L'] = "101110101";
    morse['M'] = "1110111";
    morse['N'] = "11101";
    morse['O'] = "11101110111";
    morse['P'] = "10111011101";
    morse['Q'] = "1110111010111";
    morse['R'] = "1011101";
    morse['S'] = "10101";
    morse['T'] = "111";
    morse['U'] = "1010111";
    morse['V'] = "101010111";
    morse['W'] = "101110111";
    morse['X'] = "11101010111";
    morse['Y'] = "11101110101";
    morse['Z'] = "11101110101";
    morse['1'] = "10111011101110111";
    morse['2'] = "101011101110111";
    morse['3'] = "1010101110111";
    morse['4'] = "10101010111";
    morse['5'] = "101010101";
    morse['6'] = "11101010101";
    morse['7'] = "1110111010101";
    morse['8'] = "111011101110101";
    morse['9'] = "11101110111011101";
    morse['0'] = "1110111011101110111";
    morse[' '] = "000000";


    return( morse[ch]);
}

QString MainWindow::encodeCW( QStringList message )
{
    QString output;
    output = "";
    for (int i = 0; i < message.size(); ++i)
    {
        //qDebug() << message.at(i) << endl;
        QString value = (message.at(i)).toUpper();
        //qDebug() << value;
        for( int j = 0; j < value.size(); ++j )
        {
            //qDebug() << value[j];
            output.append( morsechar( value[j] ));
            output.append( "000" );
        }
        output.append( morsechar( ' ' ) );
    }

    //qDebug() << "in encodeCW";
    //foreach( QChar c, output ){
    //    qDebug() << c << "=" << c.unicode() << "=" << QString( "%1" ).arg( c.unicode(), 0 , 2 );
    //}
    return(  output );
}

QString MainWindow::displayCW( QString message )
{
    QString display;
    for( int i = 0; i < message.size(); ++i )
    {
        if( message.at(i) == '1' )
        {
            display.append('#');
        }else{
            display.append(' ');
        }
    }
    return( display );
}

void MainWindow::writeString()
{
    QString filename;
    QStringList message;
    filename =  QFileDialog::getSaveFileName(this, tr("Save File as"),"",tr("Files (*.qrss *.wspr)"));
     //qDebug() << filename << " returned " << endl;
    ui->statusBar->showMessage(QString(" Writing to file %1").arg(filename));
    QFile *file = new QFile(filename);
    if( !file->open(QFile::WriteOnly) )
    {
        qDebug() << "Open failed." << endl;
    }else{
        qDebug() << "Open success." << endl;
    }
    if( filename.contains("qrss"))
    {
        message << ui->call_lineEdit->text();
        QString binary = encodeCW( message );
        QString display = displayCW( encodeCW( message ) );
        file->write(binary.toLocal8Bit().constData());
        file->write("\n");
        file->write(display.toLocal8Bit().constData());
        file->close();
     }else{
        message << (ui->call_lineEdit->text().leftJustified(6,' ')) << ui->grid_comboBox->currentText() << ui->power_comboBox->currentText();
        long value = encodeWSPR( message );
        qDebug() << message << value;
    }

}

int MainWindow::wsprcallchar( QChar ch )
{
    //ch.toUpper();
    QHash <QChar, int> wspr;
    wspr['A'] = 10;
    wspr['B'] = 11;
    wspr['C'] = 12;
    wspr['D'] = 13;
    wspr['E'] = 14;
    wspr['F'] = 15;
    wspr['G'] = 16;
    wspr['H'] = 17;
    wspr['I'] = 18;
    wspr['J'] = 19;
    wspr['K'] = 20;
    wspr['L'] = 21;
    wspr['M'] = 22;
    wspr['N'] = 23;
    wspr['O'] = 24;
    wspr['P'] = 25;
    wspr['Q'] = 26;
    wspr['R'] = 27;
    wspr['S'] = 28;
    wspr['T'] = 29;
    wspr['U'] = 30;
    wspr['V'] = 31;
    wspr['W'] = 32;
    wspr['X'] = 33;
    wspr['Y'] = 34;
    wspr['Z'] = 35;
    wspr['1'] = 1;
    wspr['2'] = 2;
    wspr['3'] = 3;
    wspr['4'] = 4;
    wspr['5'] = 5;
    wspr['6'] = 6;
    wspr['7'] = 7;
    wspr['8'] = 8;
    wspr['9'] = 9;
    wspr['0'] = 0;
    wspr[' '] = 36;

    return( wspr[ch]);
}
int MainWindow::wsprlocchar( QChar ch )
{
    //ch.toUpper();
    QHash <QChar, int> wspr;
    wspr['A'] = 0;
    wspr['B'] = 1;
    wspr['C'] = 2;
    wspr['D'] = 3;
    wspr['E'] = 4;
    wspr['F'] = 5;
    wspr['G'] = 6;
    wspr['H'] = 7;
    wspr['I'] = 8;
    wspr['J'] = 9;
    wspr['K'] = 10;
    wspr['L'] = 11;
    wspr['M'] = 12;
    wspr['N'] = 13;
    wspr['O'] = 14;
    wspr['P'] = 15;
    wspr['Q'] = 16;
    wspr['R'] = 17;

    return( wspr[ch]);
}



long MainWindow::encodeWSPR( QStringList message )
{
    long output;
    long output1;
    long output2;
    QString msgNumbers;
    for (int i = 0; i < message.size(); ++i)
    {
        //qDebug() << message.at(i) << endl;
        QString value;
        value = (message.at(i)).toUpper();
        //value.leftJustified(6, ' ', false);
        qDebug() << value;
        if( i == 0 )
        {
          for( int j = 0; j < value.size(); ++j )
          {
              //qDebug() << wsprcallchar( value[j] );
              msgNumbers.append( QString("%1,").arg(wsprcallchar(value[j])));
              if( j == 0 )
              {
                 output = wsprcallchar( value[j] );
              }else if( j == 1 ){
                 output = output * 36 + wsprcallchar( value[j] );
              }else if ( j == 2 ){
                 output = output * 10 + wsprcallchar( value[j] );
              }else if ( j == 3 ){
                 output = output * 27 + (wsprcallchar( value[j] ) - 10);
              }else if ( j == 4 ){
                 output = output * 27 + (wsprcallchar( value[j] ) - 10);
              }else if ( j == 5 ){
                 output = output * 27 + (wsprcallchar( value[j] ) - 10);
              }else{

             }
          }
        }else if( i == 1){
            if( value.size() == 4 )
            {
                //qDebug() << wsprlocchar( value[0] );
                //qDebug() << wsprlocchar( value[1] );
                //qDebug() << wsprlocchar( value[2] );
                //qDebug() << wsprlocchar( value[3] );
                msgNumbers.append( QString("%1,").arg(wsprlocchar(value[0])));
                msgNumbers.append( QString("%1,").arg(wsprlocchar(value[1])));
                msgNumbers.append( QString("%1,").arg(wsprcallchar(value[2])));
                msgNumbers.append( QString("%1,").arg(wsprcallchar(value[3])));
                output1 = (179 - 10 * wsprlocchar( value[0] ) - wsprcallchar( value[2] )) * 180 + 10 * wsprlocchar( value[1]) + wsprcallchar( value[3] );
            }
        }else if( i == 2 ){
                //qDebug() << value.toInt();
                msgNumbers.append( QString("%1,").arg(value.toInt()));
                output2 = output1 * 128 + value.toInt() + 64;
        }else{
            qDebug() << "ERROR: only 3 words allowed! ";
        }
        msgNumbers.append( QString("%1,").arg(wsprcallchar( ' ' )));

    }
    qDebug() << msgNumbers << endl;
    qDebug() << "in encodeWSPR" << output << output1 << output2;
    qDebug() << QString("%1").arg(output,28,2) << QString("%1").arg(output2,15,2) << endl;
    QString data;
    bool ok;
    data.append(QString("%1").arg(output,28,2));
    data.append(QString("%1").arg(output2,15,2));
    qDebug() << data;
    QByteArray test = QCString(QString("10010110") );
    QByteArray KEY = QByteArray::fromHex( QByteArray( "30 81 9F 30 0D 06 09 2A 86 48 86 F7 0D 01 01 01 05 00 03 81 8D 00" ).replace( ' ', "" ) );
    //uint8_t array[] = { 0x30, 0x81, 0x9f, 0x8d, 0 };
    //qDebug() << QString("%1").arg(KEY,100,10);
    qDebug() << test;
    return(  output );
}
