/* 
 * File:   LSBFilters.cpp
 * Author: john
 * 
 * Created on 14 August 2010, 10:22
 */


#include "LSBFilters.h"
#include "Filter.h"
#include "Filters.h"

LSBFilters::LSBFilters() {

    filters[0].init("5.0k",-5150,-150);
    filters[1].init("4.4k",-4550,-150);
    filters[2].init("3.8k",-3950,-150);
    filters[3].init("3.3k",-3450,-150);
    filters[4].init("2.9k",-3050,-150);
    filters[5].init("2.7k",-2850,-150);
    filters[6].init("2.4k",-2550,-150);
    filters[7].init("2.1k",-2250,-150);
    filters[8].init("1.8k",-1950,-150);
    filters[9].init("1.0k",-1150,-150);

    selectFilter(3);
}

LSBFilters::~LSBFilters() {
}

