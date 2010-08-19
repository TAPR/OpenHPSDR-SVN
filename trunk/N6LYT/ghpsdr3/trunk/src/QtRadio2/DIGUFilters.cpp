/* 
 * File:   DIGUFilters.cpp
 * Author: john
 * 
 * Created on 14 August 2010, 10:22
 */


#include "DIGUFilters.h"
#include "Filter.h"
#include "Filters.h"

DIGUFilters::DIGUFilters() {
    filters[0].init("5.0k",150,5150);
    filters[1].init("4.4k",150,4550);
    filters[2].init("3.8k",150,3950);
    filters[3].init("3.3k",150,3450);
    filters[4].init("2.9k",150,3050);
    filters[5].init("2.7k",150,2850);
    filters[6].init("2.4k",150,2550);
    filters[7].init("2.1k",150,2250);
    filters[8].init("1.8k",150,1950);
    filters[9].init("1.0k",150,1150);

    selectFilter(3);
}

DIGUFilters::~DIGUFilters() {
}

