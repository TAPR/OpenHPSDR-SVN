/**
* @file  cusdr_highResTimer.h
* @brief HPSDR high resolution timer header file
* @author Song Ho Ahn (song.ahn@gmail.com)
* @version 
* @date 2006-01-13
*/

/*
 * 	 High Resolution Timer.
 *   This timer is able to measure the elapsed time with 1 micro-second accuracy
 *   in both Windows, Linux and Unix system 
 *   
 *	 Copyright 2006 Song Ho Ahn (song.ahn@gmail.com)
 *   Copyright 2012 adapted for cuSDR by Hermann von Hasseln, DL3HVH
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Library General Public License version 2 as
 *   published by the Free Software Foundation
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details
 *
 *   You should have received a copy of the GNU Library General Public
 *   License along with this program; if not, write to the
 *   Free Software Foundation, Inc.,
 *   51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

#include "cusdr_highResTimer.h"
#include <stdlib.h>

HResTimer::HResTimer() {

#ifdef WIN32
    QueryPerformanceFrequency(&frequency);
    startCount.QuadPart = 0;
    endCount.QuadPart = 0;
#else
    startCount.tv_sec = startCount.tv_usec = 0;
    endCount.tv_sec = endCount.tv_usec = 0;
#endif

    stopped = 0;
    startTimeInMicroSec = 0;
    endTimeInMicroSec = 0;
}

HResTimer::~HResTimer() {

}

void HResTimer::start() {

    stopped = 0; // reset stop flag

#ifdef WIN32
    QueryPerformanceCounter(&startCount);
#else
    gettimeofday(&startCount, NULL);
#endif
}

void HResTimer::stop() {

    stopped = 1; // set timer stopped flag

#ifdef WIN32
    QueryPerformanceCounter(&endCount);
#else
    gettimeofday(&endCount, NULL);
#endif
}

double HResTimer::getElapsedTimeInMicroSec() {

#ifdef WIN32
    if(!stopped)
        QueryPerformanceCounter(&endCount);

    startTimeInMicroSec = startCount.QuadPart * (1000000.0 / frequency.QuadPart);
    endTimeInMicroSec = endCount.QuadPart * (1000000.0 / frequency.QuadPart);
#else
    if(!stopped)
        gettimeofday(&endCount, NULL);

    startTimeInMicroSec = (startCount.tv_sec * 1000000.0) + startCount.tv_usec;
    endTimeInMicroSec = (endCount.tv_sec * 1000000.0) + endCount.tv_usec;
#endif

    return endTimeInMicroSec - startTimeInMicroSec;
}

double HResTimer::getElapsedTimeInMilliSec() {

    return this->getElapsedTimeInMicroSec() * 0.001;
}

double HResTimer::getElapsedTimeInSec() {

    return this->getElapsedTimeInMicroSec() * 0.000001;
}

double HResTimer::getElapsedTime() {

    return this->getElapsedTimeInSec();
}
