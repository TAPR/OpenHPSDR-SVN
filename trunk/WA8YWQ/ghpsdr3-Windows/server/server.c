/**
* @file server.c
* @brief HPSDR server application
* @author John Melton, G0ORX/N6LYT, Windows code by Dave McQuate, WA8YWQ
* @version 0.1
* @date 2009-10-13, revised 2010-06-24
*/


/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#ifdef __linux__ 
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <pthread.h>
#include <getopt.h>
#else  // Windows
#include "pthread.h"
#include <winsock.h>
#include "getopt.h"
#endif

#include "client.h"
#include "listener.h"
#include "ozy.h"
#include "receiver.h"

/** command line options:  --option value
 */
static struct option long_options[] = {
    {"receivers",required_argument, 0, 0},
    {"samplerate",required_argument, 0, 1},
    {"dither",required_argument, 0, 2},
    {"random",required_argument, 0, 3},
    {"preamp",required_argument, 0, 4},
    {"10mhzsource",required_argument, 0, 5},
    {"122.88mhzsource",required_argument, 0, 6},
    {"micsource",required_argument, 0, 7},
    {"class",required_argument, 0, 8},
    {"timing",no_argument, 0, 9},
    {0,0, 0, 0},
};
static char* short_options="";
static int option_index;

void process_args(int argc,char* argv[]);

/**
 * Set default command line parameter values,
 * process command line,
 * start threads.
 */
int main(int argc,char* argv[]) 
{
#ifndef __linux__ 
	WORD wVersionRequested;
	WSADATA wsaData;
	int err;
#endif

// set default values
	ozy_set_receivers(4);
	ozy_set_sample_rate(96000);
	ozy_set_dither(0);
	ozy_set_random(0);
	ozy_set_preamp(0);
	ozy_set_10mhzsource(2);
	ozy_set_122_88mhzsource(1);
	ozy_set_micsource(1);
	ozy_set_class(0);


#ifndef __linux__  
	wVersionRequested = MAKEWORD(1, 1);
	err = WSAStartup(wVersionRequested, &wsaData);		// initialize Windows sockets
#endif

    process_args(argc,argv);	// process any command line arguments

    init_receivers();

    create_listener_thread();

    create_ozy_thread();

    while(1) {
#ifdef __linux__ 
	sleep(10);
#else   // Windows
        Sleep(10);
#endif
    }
}
/**
 * Call getopt_long() to process command line arguments and store values:
 * - receivers: 1, 2, 3, or 4,
 * - samplerate: 48000, 96000, or 192000, 
 * - preamp: on or off,
 * - dither: on or off,
 * - random: on or off,
 * - 10mhzsource: atlas, penelope, or mercury:  0, 1, 2
 * - 122.88mhzsource: penelope or mercury: 0, 1
 * - Micsource: janus or penelope: 0, 1
 * - class: other or E: 0, 1
 * - timing
 */
void process_args(int argc,char* argv[]) {
    int i;

    while((i=getopt_long(argc,argv,short_options,long_options,&option_index))!=EOF) {

        switch(i) {
            case 0: // receivers
                ozy_set_receivers(atoi(optarg));
                break;
                break;
            case 1: // sample rate
                ozy_set_sample_rate(atoi(optarg));
                break;
            case 2: // dither
                if(strcmp(optarg,"off")==0) {
                    ozy_set_dither(0);
                } else if(strcmp(optarg,"on")==0) {
                    ozy_set_dither(1);
                } else {
                    fprintf(stderr,"invalid dither option\n");
                }
                break;
            case 3: // random
                if(strcmp(optarg,"off")==0) {
                    ozy_set_random(0);
                } else if(strcmp(optarg,"on")==0) {
                    ozy_set_random(1);
                } else {
                    fprintf(stderr,"invalid random option\n");
                }
                break;
            case 4: // preamp
                if(strcmp(optarg,"off")==0) {
                    ozy_set_preamp(0);
                } else if(strcmp(optarg,"on")==0) {
                    ozy_set_preamp(1);
                } else {
                    fprintf(stderr,"invalid preamp option\n");
                }
                break;
            case 5: // 10 MHz clock source
                if(strcmp(optarg,"atlas")==0) {
                    ozy_set_10mhzsource(0);
                } else if(strcmp(optarg,"penelope")==0) {
                    ozy_set_10mhzsource(1);
                } else if(strcmp(optarg,"mercury")==0) {
                    ozy_set_10mhzsource(2);
                } else {
                    fprintf(stderr,"invalid 10 MHz clock option\n");
                }
                break;
            case 6: // 122.88 MHz clock source
                if(strcmp(optarg,"penelope")==0) {
                    ozy_set_122_88mhzsource(0);
                } else if(strcmp(optarg,"mercury")==0) {
                    ozy_set_122_88mhzsource(1);
                } else {
                    fprintf(stderr,"invalid 122.88 MHz clock option\n");
                }
                break;
            case 7: // mic source
                if(strcmp(optarg,"janus")==0) {
                    ozy_set_micsource(0);
                } else if(strcmp(optarg,"penelope")==0) {
                    ozy_set_micsource(1);
                } else {
                    fprintf(stderr,"invalid mic source option\n");
                }
                break;
            case 8: // class
                if(strcmp(optarg,"other")==0) {
                    ozy_set_class(0);
                } else if(strcmp(optarg,"E")==0) {
                    ozy_set_class(1);
                } else {
                    fprintf(stderr,"invalid class option\n");
                }
                break;

            case 9: // timing
                ozy_set_timing(1);
                break;

            default:
                fprintf(stderr,"Usage: \n");
                fprintf(stderr,"  server --receivers N --samplerate 48000|96000|192000\n");
                exit(1);
                break;
               
        }
    }
}
