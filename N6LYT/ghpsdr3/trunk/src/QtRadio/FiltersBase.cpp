/* 
 * File:   FiltersBase.cpp
 * Author: john
 * 
 * Created on 14 August 2010, 10:15
 */

#include "FiltersBase.h"

FiltersBase::FiltersBase() {
}

FiltersBase::FiltersBase(const FiltersBase& orig) {
}

FiltersBase::~FiltersBase() {
}

int FiltersBase::getSelected() {
    return currentFilter;
}

void FiltersBase::selectFilter(int f) {
    currentFilter=f;
}

QString FiltersBase::getText(int f) {
    return filters[f].getText();
}

QString FiltersBase::getText() {
    return filters[currentFilter].getText();
}

int FiltersBase::getLow() {
    return filters[currentFilter].getLow();
}

int FiltersBase::getHigh() {
    return filters[currentFilter].getHigh();
}

