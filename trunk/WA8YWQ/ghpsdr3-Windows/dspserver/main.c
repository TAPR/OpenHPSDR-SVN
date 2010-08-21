/** 
* \file main.c
* \brief Main file for the GHPSDR3 Software Defined Radio dspserver. 
* \author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S and Dave McQuate WA8YWQ
* \version 0.1
* \date 2009-04-11
*
*
* \mainpage GHPSDR 
*  \image html ../ghpsdr.png
*  \image latex ../ghpsdr.png "Screen shot of GHPSDR" width=10cm
*
* \section Separate processes:  server communicates with the hardware via USB; dspserver provides digital signal processing
* \author John Melton, G0ORX/N6LYT
* \version 0.1
* \date 2009-04-11
* 
* \author Dave Larsen, KV0S, Doxygen comments
* \author Dave McQuate, WA8YWQ, porting to Windows, and Doxygen comments
*
* These files are design to build a simple 
* high performance  interface under the Linux  operating system.  
*
* This is still very much an Alpha version. It does still have problems and not everything is 
* completed.
*
* To build the application there is a simple Makefile.
*
* Tuning can be accomplished by left mouse clicking in the Panadapter/Waterfall window to 
* move the selected frequency to the center of the current filter. A right mouse click will 
* move the selected frequency to the cursor. You can also use the left mouse button to drag 
* the frequency by holding it down while dragging. If you have a scroll wheel, moving the 
* scroll wheel will increment/decrement the frequency by the current step amount.
*
* You can also left mouse click on the bandscope display and it will move to the selected frequency.
* 
* There are some problems when running at other than 48000. Sometimes the audio output will 
* stop although the Panadapter/Waterfall and bandscope continue to function. It usually 
* requires intiozy to be run again to get the audio back.
*
*
* Development of the system is documented at 
* http://javaguifordttsp.blogspot.com/
*
* This code is available at 
* svn://206.216.146.154/svn/repos_sdr_hpsdr/trunk/N6LYT/ghpsdr
*
* More information on the HPSDR project is availble at 
* http://openhpsdr.info
*
*/

/* Copyright (C) 
* 2009 - John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
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



// main.c

#include <fcntl.h>
#include <sys/types.h>

#ifdef __linux__
#include <unistd.h>
#endif

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

#ifdef __linux__
#include <getopt.h>
#else
#include <winsock.h>
#include "getopt.h"
#endif

#include "client.h"
#include "dttsp.h"
#include "pthread.h"
#include "audiostream.h"
#include "soundcard.h"
#include "ozy.h"
#include "version.h"

char propertyPath[128];

struct option longOptions[] = {
    {"soundcard",required_argument, 0, 0},
    {"receiver",required_argument, 0, 1},
    {"server",required_argument, 0, 2},
    {"offset",required_argument, 0, 3},
};

char* shortOptions="";

int optionIndex;

/* --------------------------------------------------------------------------*/
/** 
* @brief Process program arguments 
* Command line arguments are--
* - --soundcard
* - --receiver, an integer from 0 to 4
* - --server, the IP address of the server
* - --offset
* @param argc
* @param argv
*/
/* ----------------------------------------------------------------------------*/
void processCommands(int argc,char** argv) {
    int c;
    while((c=getopt_long(argc,argv,shortOptions,longOptions,&optionIndex)!=EOF)) {
        switch(optionIndex) {
            case 0:
                strcpy(soundCardName,optarg);
                break;
            case 1:
                receiver=atoi(optarg);
                break;
            case 2:
                strcpy(server_address,optarg);
                break;
            case 3:
                offset=atoi(optarg);
                break;
        }
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief  Main - it all starts here
* - On Windows only, initialize sockets
* - Set default values:  soundCardName = HPSDR,  server = 127.0.0.1, receiver = 0, offset = 0
* - Process command line arguments
* - Initialize DttSP
* - call client_init()
* - call audio_stream_init()
* - call ozy_init()
* - wait until terminated
* @param argc
* @param argv[]
* 
* @return 
*/
/* ----------------------------------------------------------------------------*/


int main(int argc,char* argv[]) {
    int i;

#ifndef __linux__
	WORD wVersionRequested;
	WSADATA wsaData;
	int err;

	wVersionRequested = MAKEWORD(1, 1);
	err = WSAStartup(wVersionRequested, &wsaData);		// initialize Windows sockets
#endif

    strcpy(soundCardName, "HPSDR");
    strcpy(server_address, "127.0.0.1"); // localhost
	receiver = 0;
	offset = 0;

    processCommands(argc,argv);

    fprintf(stderr,"gHPSDR rx %d (Version %s)\n",receiver,VERSION);

    setSoundcard(getSoundcardId(soundCardName));

    // initialize DttSP
    Setup_SDR();
    Release_Update();
    SetTRX(0,0);			// thread 0 is for receive; 1st arg = thread; 2nd arg: 0 = Rx,  1 = Tx
    SetTRX(1,1);			// thread 1 is for transmit
    SetRingBufferOffset(0,offset);
    SetThreadProcessingMode(0,2);	// 1st arg = thread; 2nd arg = RunMode: 0 = RUN_MUTE, 1 = RUN_PASS, 2 = RUN_PLAY, 3 = RUN_SWCH
    SetThreadProcessingMode(1,2);
    SetSubRXSt(0,0,1);			// 1st arg = thread; 2nd arg = sub-receiver number; 3rd arg: 0 = inactive, 1 = active
	SetRXOutputGain(0, 0, 0.9);	// 1st arg = thread; 2nd arg = sub-receiver number; 3rd arg = audio gain from 0.0 to 1.0
	SetSampleRate(96000.0);
    SetDSPBuflen(0,1024);
    SetDSPBuflen(1,1024);

    client_init(receiver);
    audio_stream_init(receiver);

    // initialize ozy
    ozy_init();

    while(1) {
#ifdef __linux__
		sleep(1000);
#else
        Sleep(1000);
#endif
    }

    return 0;
}
