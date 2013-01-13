#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QLabel>
#include <QDebug>
#include <QTimer>
#include <QFileDialog>
#include <QMessageBox>

#include <pcap.h>

#include "interfaces.h"
#include "receivethread.h"
#include "rawreceivethread.h"
#include "discovery.h"
#include "board.h"
#include "aboutdialog.h"
#include "statusdialog.h"
#include "addressdialog.h"
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
    //void about();
    void interfaceSelected(int);
    //void metisSelected();
    void metisSelected(int index);
    //void hermesSelected();
    //void angeliaSelected();
    void browse();
    void program();
    //void erase();
    //void getMAC();
    //void getIP();
    void setIP();

    void discover();
    void discovery_timeout();

    // SLOTS for RawReceiveThread
    void erase_timeout();
    void eraseCompleted();
    void nextBuffer();
    void timeout();

    void readIPaddress();
    //void macAddress(unsigned char*);
    //void ipAddress(unsigned char*);
    //void fpgaId(unsigned char*);


    void board_found(Board*);

    /*
    void tabChanged(int);

    void jtagInterrogate();
    void jtagBrowse();
    void jtagProgram();
    void jtagFlashBrowse();
    void jtagFlashProgram();
    void nextJTAGBuffer();
    void startJTAGFlashErase();
    */
private:
    Ui::MainWindow *ui;
    QLabel *deviceIndicator;


    //void loadPOF(QString filename);
    int loadRBF(QString filename);

    void eraseData();
    void sendCommand(unsigned char command);
    void sendData();


    void readMAC();
    void readIP();
    void writeIP();
    /*
    void sendRawCommand(unsigned char command);


    void sendRawData();

    void sendJTAGData();
    */

    void sendJTAGFlashData();

    void idle();

    void status(QString text);

    //void bootloaderProgram();
    void flashProgram();
    //void bootloaderErase();
    void flashErase();

    /*
    int loadMercuryRBF(QString filename);
    int loadPenelopeRBF(QString filename);
    void jtagBootloaderProgram();
    void jtagEraseData();
    //void jtagFlashProgram();
  */

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

    /*
    bool bootloader;

    long fpga_id;
    */
    AboutDialog *ab;
    StatusDialog *stat;
    AddressDialog *add;
};

#endif // MAINWINDOW_H
