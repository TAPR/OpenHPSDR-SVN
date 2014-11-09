#ifndef CHANGEIPADDRESS_H
#define CHANGEIPADDRESS_H

#include <QThread>
#include <QUdpSocket>
#include <QHostInfo>
#include <QStringList>
#include <QMessageBox>
#include <../Status/statusdialog.h>
#include <QFile>
#include <QTimer>

#include "./hpsdr/board.h"
#include "./hpsdr/timeouts.h"

// command codes
#define PROGRAM_METIS_FLASH 0x01
#define ERASE_METIS_FLASH   0x02
#define READ_METIS_MAC      0x03
#define READ_METIS_IP       0x04
#define WRITE_METIS_IP      0x05
#define GET_JTAG_DEVICE_ID  0x06
#define PROGRAM_MERCURY     0x07
#define PROGRAM_PENELOPE    0x08
#define JTAG_ERASE_FLASH    0x09
#define PROGRAM_FLASH       0x0A

// reply codes
#define INVALID_COMMAND  0x00
#define ERASE_DONE       0x01
#define SEND_MORE        0x02
#define HAVE_MAC_ADDRESS 0x03
#define HAVE_IP_ADDRESS  0x04
#define FPGA_ID          0x05

// states
#define IDLE 0
#define ERASING 1
#define PROGRAMMING 2
#define ERASING_ONLY 3
#define READ_MAC 4
#define READ_IP 5
#define WRITE_IP 6




class WriteBoard : public QObject
{
    Q_OBJECT
public:
    WriteBoard(QUdpSocket *s, StatusDialog *st);
    ~WriteBoard();
    QHash<QString, Board*> boards;
    QString currentboard;
    StatusDialog *stat;
    int state;


signals:
    void discover();
    void discoveryBoxUpdate();
    void eraseCompleted();
    void programmingCompleted();
    void nextBuffer();
    void readyRead();
    void timeout();

public slots:
    //void readyRead();
    void changeIP(QStringList *saddr , unsigned char *macaddr);
    void discovery();
    void update_discovery();
    void update_command();
    void writeRBF();
    void readPending();
    void sendData(Board *bd);
    void eraseData(Board *bd);
    void erase_timeout();
    int loadRBF(QString filename );
    void sendCommand(unsigned char command, Board *bd);
    void incOffset();

private:
    QUdpSocket* socket;
    QString ip;
    unsigned char* macaddr;
    unsigned char buffer[1024];
    qint16 port;
    int offset;
    int start;
    int end;
    int blocks;
    char* data;
    int percent;



};

#endif // CHANGEIPADDRESS_H
