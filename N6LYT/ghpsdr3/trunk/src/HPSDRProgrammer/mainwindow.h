/*
 * File:   mainwindow.h
 * Author: John Melton, G0ORX/N6LYT
 *
 * Created on 23 November 2010
 */

/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/

#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <sys/types.h>
#ifdef __WIN32
#include <winsock2.h>
#else
#include <sys/socket.h>
#include <sys/ioctl.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <net/if_arp.h>
#include <net/if.h>
#include <ifaddrs.h>
#endif
#include <pcap.h>

#include <QMainWindow>
#include "Interfaces.h"
#include "ReceiveThread.h"
#include "RawReceiveThread.h"
#include "DiscoveryThread.h"
#include "Metis.h"

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
#define MAX_ERASE_TIMEOUTS (2000) // 20 seconds

namespace Ui {
    class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

public slots:
    void interfaceSelected(int);
    void browse();
    void program();
    void erase();
    void getMAC();
    void getIP();
    void setIP();

    void discover();
    void discovery_timeout();

    // SLOTS for RawReceiveThread
    void eraseCompleted();
    void nextBuffer();
    void timeout();
    void macAddress(unsigned char*);
    void ipAddress(unsigned char*);
    void fpgaId(unsigned char*);

    void metis_found(unsigned char*,long);
    void metisSelected(int);

    void tabChanged(int);

    void jtagInterrogate();
    void jtagBrowse();
    void jtagProgram();
    void jtagFlashBrowse();
    void jtagFlashProgram();
    void nextJTAGBuffer();
    void startJTAGFlashErase();

private:
    Ui::MainWindow *ui;

    //void loadPOF(QString filename);
    int loadRBF(QString filename);
    void eraseData();
    void readMAC();
    void readIP();
    void writeIP();

    void sendRawCommand(unsigned char command);
    void sendCommand(unsigned char command);

    void sendRawData();
    void sendData();
    void sendJTAGData();
    void sendJTAGFlashData();

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

    Interfaces interfaces;
    long ip;
    QString hwAddress;
    unsigned char hw[6];

    long metisIP;

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

    QList<Metis*> metis;

    int state;

    int percent;
    int eraseTimeouts;

    int discovery_socket;
    struct sockaddr_in discovery_addr;
    int discovery_length;

    DiscoveryThread* discoveryThread;
    ReceiveThread* receiveThread;
    RawReceiveThread* rawReceiveThread;

    bool bootloader;

    long fpga_id;

};

#endif // MAINWINDOW_H
