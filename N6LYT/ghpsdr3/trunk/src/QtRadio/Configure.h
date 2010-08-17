/* 
 * File:   Configure.h
 * Author: john
 *
 * Created on 16 August 2010, 20:03
 */

#ifndef _CONFIGURE_H
#define	_CONFIGURE_H

#include "ui_Configure.h"

class Configure : public QDialog {
    Q_OBJECT
public:
    Configure();
    virtual ~Configure();
private:
    Ui::Configure widget;
};

#endif	/* _CONFIGURE_H */
