#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QLabel>
#include <QDebug>
#include <QTimer>
#include <QFileDialog>
#include <QFileInfo>
#include <QDir>
#include <QMessageBox>
#include <QStringList>
#include <QSettings>

#include "../Programmer/interfaces.h"
#include "../Programmer/receivethread.h"
#include "../Programmer/rawreceivethread.h"
#include "../Programmer/discovery.h"
#include "../Programmer/board.h"
#include "../IPaddress/changeipaddress.h"
#include "../Help/aboutdialog.h"
#include "../Status/statusdialog.h"
#include "../IPaddress/addressdialogprog.h"
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


#define TIMEOUT 10 // ms
#define MAX_ERASE_TIMEOUTS (9000) // 90 seconds


namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
    
public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
#ifdef Q_WS_MAC
    void setPath(char* path);
#endif


public slots:
    void quit();

    void interfaceSelected(int);

    void metisSelected(int index);

    void browse();
    void program();

    //void setIP();
    void setIP_UDP();


    void clearDiscovery();
    void discover();
    void discovery_timeout();

    // SLOTS for RawReceiveThread
    void erase_timeout();
    void eraseCompleted();
    void nextBuffer();
    void timeout();

    void readIPaddress();


    void board_found(Board*);


private:
    Ui::MainWindow *ui;
    QLabel *deviceIndicator;
    int currentBoardIndex;
    QString currentboard;
    QSettings settings;
    QMessageBox msg;

    int loadRBF(QString filename);

    void eraseData();
    void sendCommand(unsigned char command);
    void sendData();


    void readMAC();
    void readIP();
    void writeIP();


    void sendFlashData();
    int testSubnet( QStringList *saddr );


    void idle();

    void status(QString text);


    void flashProgram();

    void flashErase();

    void loadFlash();

#ifdef Q_WS_MAC
    char* myPath;
#endif

    QString text;

    bool isMetis;


    Interfaces interfaces;
    long ip;
    QString interfaceName;
    QString hwAddress;
    unsigned char hw[6];

    long selectedBoardIP;
    QString selectedBoardHostAddress;
    unsigned char* selectedBoardMAC;


    int s;

    char* data;
    int offset;
    int start;
    int end;
    int blocks;

    int size; //depending on how and what we are programming
              // 0 if programming flash on metis in raw mode
              // blocks if programming flash on metis in command mode
              // bytes (blocks*256) if programming flash on JTAG (Mercury or Penelope)
    unsigned char data_command;


    pcap_t *handle;

    QList<Board*> bd;

    int state;

    int percent;
    int eraseTimeouts;


    QUdpSocket socket;
    Discovery* discovery;
    ReceiveThread* receiveThread;
    RawReceiveThread* rawReceiveThread;


    AboutDialog *ab;
    StatusDialog *stat;
    AddressDialog *add;
};

#endif // MAINWINDOW_H
