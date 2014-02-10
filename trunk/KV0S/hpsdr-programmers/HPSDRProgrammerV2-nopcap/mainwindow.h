#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QLabel>
#include <QDebug>
#include <QTimer>
#include <QFileDialog>
#include <QFileInfo>
#include <QDir>
#include <QString>
#include <QMessageBox>
#include <QStringList>
#include <QSettings>
#include <QUdpSocket>


#include "../Help/aboutdialog.h"
#include "../Status/statusdialog.h"
#include "./hpsdr/addressdialogprog.h"
#include "./hpsdr/interfaces.h"
#include "./hpsdr/writeboard.h"
#include "./hpsdr/timeouts.h"
#include "version.h"


// states
#define IDLE 0
#define ERASING 1
#define PROGRAMMING 2
#define ERASING_ONLY 3
#define READ_MAC 4
#define READ_IP 5
#define WRITE_IP 6
#define JTAG_INTERROGATE 7
#define JTAG_PROGRAM 8
#define FLASH_ERASING 9
#define FLASH_PROGRAM 10


namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
    
public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    // private function to display message in the status window
    void status(QString text);


private:
    Ui::MainWindow *ui;
    QLabel *deviceIndicator;
    QSettings settings;
    AboutDialog *ab;
    StatusDialog *stat;
    AddressDialog *add;
    Interfaces interfaces;
    QUdpSocket *socket;
    WriteBoard *wb;
    QString fileName;
    int state;
    int eraseTimeouts;
    int offset;
    int start;
    int end;

signals:
    void updateDiscovery();

private slots:
    void interfaceSelected(int id);
    void boardSelected(int id);
    void discover();
    void discoveryUpdate();
    void browse();
    void program();
    void setIP_UDP();
    void timeout();
    void eraseCompleted();
    void programmingCompleted();
    void nextBuffer();
    void stbar(QString text);

private:
    bool discoveryDone;
    bool programComplete;
};

#endif // MAINWINDOW_H
