/* 
 * File:   SAMFilters.cpp
 * Author: john
 * 
 * Created on 14 August 2010, 10:22
 */


#include "SAMFilters.h"
#include "Filter.h"
#include "Filters.h"

SAMFilters::SAMFilters() {
    filters[0].init("16k",-8000,8000);
    filters[1].init("12k",-6000,6000);
    filters[2].init("10k",-5000,5000);
    filters[3].init("8k",-4000,4000);
    filters[4].init("6.6k",-3300,3300);
    filters[5].init("5.2k",-2600,2600);
    filters[6].init("4.0k",-2000,2000);
    filters[7].init("3.1k",-1550,1550);
    filters[8].init("2.9k",-1450,1450);
    filters[9].init("2.4k",-1200,1200);

    selectFilter(3);
}

SAMFilters::~SAMFilters() {
}

