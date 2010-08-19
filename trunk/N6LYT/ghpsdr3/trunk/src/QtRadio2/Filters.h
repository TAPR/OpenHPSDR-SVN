/* 
 * File:   LSBFilter.h
 * Author: john
 *
 * Created on 14 August 2010, 10:15
 */

#ifndef FILTERS_H
#define	FILTERS_H

#include <QObject>
#include <QDebug>

#include "FiltersBase.h"

#define MAX_FILTERS 10

class Filters : public QObject {
    Q_OBJECT
public:
    Filters();
    Filters(const Filters& orig);
    virtual ~Filters();
    void selectFilters(FiltersBase* f);
    void selectFilter(int f);
    int getFilter();
    int getLow();
    int getHigh();
    QString getText();

signals:
    void filtersChanged(FiltersBase* previousFilters, FiltersBase* newFilters);
    void filterChanged(int previousFitler,int newFilter);

private:
    FiltersBase* currentFilters;
    
};

#endif	/* FILTERS_H */

