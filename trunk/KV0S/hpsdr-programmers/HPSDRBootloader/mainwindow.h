#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QSettings>
#include <QMessageBox>
#include <QLabel>
#include <QFile>
#include <QFileInfo>
#include <QFileDialog>
#include <QTimer>
#include <QStringList>

#include <pcap.h>

#include "version.h"
#include "../Help/aboutdialog.h"
#include "../Status/statusdialog.h"
#include "../IPaddress/addressdialogboot.h"
#include "../Programmer/board.h"
#include "../Programmer/discovery.h"
#include "../Programmer/interfaces.h"
#include "../Programmer/receivethread.h"
#include "../Programmer/rawreceivethread.h"

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
#define METIS_MAX_ERASE_TIMEOUTS (20000) // 20 seconds
#define HERMES_MAX_ERASE_TIMEOUTS (40000) // 40 seconds
#define ANGELIA_MAX_ERASE_TIMEOUTS (90000) // 90 seconds



namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
    
public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

signals:
    void retryProgram();


public slots:
    void quit();

    void interfaceSelected(int);
    void browse();
    void program();
    void erase();
    void getMAC();
    void getIP();
    void setIP();


    // SLOTS for RawReceiveThread
    void erase_timeout();
    void eraseCompleted();
    void nextBuffer();
    void timeout();
    void macAddress(unsigned char*);
    void ipAddress(unsigned char*);
    void fpgaId(unsigned char*);


    void metisSelected(int);

    void jtagInterrogate();
    void jtagBrowse();
    void jtagProgram();
    void jtagFlashBrowse();
    void jtagFlashProgram();
    void nextJTAGBuffer();
    void startJTAGFlashErase();
    void readIP();
    void readMAC();
    
private:
    Ui::MainWindow *ui;
    AboutDialog *ab;
    StatusDialog *stat;
    AddressDialog *add;
    int currentBoardIndex;
    QString currentboard;
    QString rbfstr;
    QSettings settings;
    QMessageBox msg;
    QLabel *deviceIndicator;

    int loadRBF(QString filename);
    void eraseData();

    void writeIP();

    void sendRawCommand(unsigned char command);
    void sendCommand(unsigned char command);

    void sendRawData();
    void sendData();
    void sendJTAGData();
    void sendJTAGFlashData();

    long selectedBoardIP;
    QString selectedBoardHostAddress;
    unsigned char* selectedBoardMAC;

    void idle();
    void status(QString text);

    void bootloaderProgram();
    void flashProgram();
    void bootloaderErase();
    void flashErase();

    int loadMercuryRBF(QString filename);
    int loadPenelopeRBF(QString filename);
    void jtagBootloaderProgram();
    void jtagEraseData();
    //void jtagFlashProgram();
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

    long metisIP;
    QString metisHostAddress;

    int s;

    char* data;
    int offset;
    int start;
    int end;
    int blocks;

    int size; //depending on how and what we are programming
              // 0 if programming flash on metis in raw modfe
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

    bool bootloader;

    long fpga_id;


};

#endif // MAINWINDOW_H
