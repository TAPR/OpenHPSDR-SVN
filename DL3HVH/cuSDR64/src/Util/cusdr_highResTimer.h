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

#ifndef CUSDR_HRES_TIMER_H
#define CUSDR_HRES_TIMER_H

#ifdef WIN32   // Windows system specific
#include <windows.h>
#else          // Unix based system specific
#include <sys/time.h>
#endif


class HResTimer {

public:
	HResTimer();                                    // default constructor
   	~HResTimer();                                   // default destructor

   	void   start();                             // start timer
   	void   stop();                              // stop the timer
   	double getElapsedTime();                    // get elapsed time in second
   	double getElapsedTimeInSec();               // get elapsed time in second (same as getElapsedTime)
   	double getElapsedTimeInMilliSec();          // get elapsed time in milli-second
   	double getElapsedTimeInMicroSec();          // get elapsed time in micro-second

protected:

private:
    double startTimeInMicroSec;                 // starting time in micro-second
    double endTimeInMicroSec;                   // ending time in micro-second
    int    stopped;                             // stop flag

#ifdef WIN32
    LARGE_INTEGER frequency;                    // ticks per second
    LARGE_INTEGER startCount;                   //
    LARGE_INTEGER endCount;                     //
#else
    timeval startCount;                         //
    timeval endCount;                           //
#endif
};

#endif // CUSDR_HRES_TIMER_H
