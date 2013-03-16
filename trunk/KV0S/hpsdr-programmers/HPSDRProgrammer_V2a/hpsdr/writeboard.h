#ifndef CHANGEIPADDRESS_H
#define CHANGEIPADDRESS_H

#include <QThread>
#include <QUdpSocket>
#include <QHostInfo>
#include <QStringList>
#include <QMessageBox>
#include <QFile>

#include "../Programmer/board.h"

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



class writeBoard : public QObject
{
    Q_OBJECT
public:
    writeBoard(QUdpSocket *s, unsigned char *MACaddress);
    ~writeBoard();


public slots:
    //void readyRead();
    void changeIP(QStringList *saddr );
    void discovery();
    void writeRBF();
    void readyRead();
    void sendData();
    void flashProgram();
    void eraseData();
    int loadRBF(QString filename );

private:
    QUdpSocket* socket;
    QString ip;
    unsigned char* macaddr;
    unsigned char buffer[1024];
    qint16 port;
};

#endif // CHANGEIPADDRESS_H
