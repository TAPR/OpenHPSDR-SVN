/* 
 * File:   CWUFilters.cpp
 * Author: john
 * 
 * Created on 14 August 2010, 10:22
 */


#include "CWUFilters.h"
#include "Filter.h"
#include "Filters.h"

CWUFilters::CWUFilters() {
    filters[0].init("1.0k",500,500);
    filters[1].init("800",400,400);
    filters[2].init("750",375,375);
    filters[3].init("600",300,300);
    filters[4].init("500",250,250);
    filters[5].init("400",200,200);
    filters[6].init("250",125,125);
    filters[7].init("100",50,50);
    filters[8].init("50",25,25);
    filters[9].init("25",13,13);

    selectFilter(5);
}

CWUFilters::~CWUFilters() {
}

