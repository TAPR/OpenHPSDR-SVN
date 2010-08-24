/* 
 * File:   Configure.h
 * Author: john
 *
 * Created on 16 August 2010, 20:03
 */

#ifndef _CONFIGURE_H
#define	_CONFIGURE_H

#include <QSettings>
#include <QDebug>

#include "ui_Configure.h"

class Configure : public QDialog {
    Q_OBJECT
public:
    Configure();
    virtual ~Configure();

    void loadSettings(QSettings* settings);
    void saveSettings(QSettings* settings);

    void connected(bool state);

    QString getHost();
    int getReceiver();
    
    int getSpectrumHigh();
    int getSpectrumLow();
    int getFps();

    int getWaterfallHigh();
    int getWaterfallLow();
    

private:
    Ui::Configure widget;
};

#endif	/* _CONFIGURE_H */
