/* 
 * File:   FiltersBase.h
 * Author: john
 *
 * Created on 14 August 2010, 10:15
 */

#ifndef FILTERSBASE_H
#define	FILTERSBASE_H

#include <QObject>
#include <QDebug>

#include "Filter.h"

#define MAX_FILTERS 10

class FiltersBase : public QObject {
    Q_OBJECT
public:
    FiltersBase();
    FiltersBase(const FiltersBase& orig);
    virtual ~FiltersBase();
    QString getText(int f);
    int getSelected();
    void selectFilter(int f);
    QString getText();
    int getLow();
    int getHigh();


    Filter filters[MAX_FILTERS];
private:
    int currentFilter;
    
};

#endif	/* FILTERSBASE_H */

