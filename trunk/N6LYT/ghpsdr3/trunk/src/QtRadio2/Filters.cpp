/* 
 * File:   Filters.cpp
 * Author: john
 * 
 * Created on 14 August 2010, 10:15
 */

#include "Filters.h"

Filters::Filters() {
    currentFilters=NULL;
}

Filters::Filters(const Filters& orig) {
}

Filters::~Filters() {
}

void Filters::selectFilters(FiltersBase* filters) {
    FiltersBase* oldFilters=currentFilters;
    currentFilters=filters;
    emit filtersChanged(oldFilters,currentFilters);
}

void Filters::selectFilter(int f) {
    int previousFilter=currentFilters->getSelected();
    currentFilters->selectFilter(f);
    emit filterChanged(previousFilter,f);
}

int Filters:: getFilter() {
    return currentFilters->getSelected();
}

int Filters::getLow() {
    return currentFilters->getLow();
}

int Filters::getHigh() {
    return currentFilters->getHigh();
}

QString Filters::getText() {
    return currentFilters->getText();
}