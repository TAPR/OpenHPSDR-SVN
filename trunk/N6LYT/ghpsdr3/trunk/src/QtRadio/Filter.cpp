/* 
 * File:   Filter.cpp
 * Author: john
 * 
 * Created on 14 August 2010, 10:08
 */

#include "Filter.h"

Filter::Filter() {
}

Filter::Filter(QString t,int l,int h) {
    text=t;
    low=l;
    high=h;
}

Filter::~Filter() {
}

void Filter::init(QString t,int l,int h) {
    text=t;
    low=l;
    high=h;
}

void Filter::setText(QString t) {
    text=t;
}

void Filter::setLow(int l) {
    low=l;
}

void Filter::setHigh(int h) {
    high=h;
}

QString Filter::getText() {
    return text;
}

int Filter::getLow() {
    return low;
}

int Filter::getHigh() {
    return high;
}

