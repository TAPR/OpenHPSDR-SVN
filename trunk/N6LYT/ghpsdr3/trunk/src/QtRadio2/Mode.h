/* 
 * File:   Mode.h
 * Author: john
 *
 * Created on 15 August 2010, 07:46
 */

#ifndef MODE_H
#define	MODE_H

#include <QObject>

#define MODE_LSB 0
#define MODE_USB 1
#define MODE_DSB 2
#define MODE_CWL 3
#define MODE_CWU 4
#define MODE_FMN 5
#define MODE_AM 6
#define MODE_DIGU 7
#define MODE_SPEC 8
#define MODE_DIGL 9
#define MODE_SAM 10
#define MODE_DRM 11


class Mode : public QObject {
    Q_OBJECT
public:
    Mode();
    virtual ~Mode();
    void setMode(int m);
    int getMode();
    QString getStringMode();

signals:
    void modeChanged(int oldMode, int newMode);

private:
    int currentMode;

};

#endif	/* MODE_H */

