/* HPSDR-USB - High Performance Software Defined Radio, Ozy Janus Jack support 
 *  
 * Copyright (C) 2008 Bill Tracey, KD5TFD  
 * Copyright (C) 2008 Bob Campbell, VK4XV 
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */


/* 
 * This is an adapatation of the Bill Tracey's (kd5tfg) loopBack.c routines  for 
 * use on Linux and Bill Tracy's janus ozy utilities.  Without Bill's 
 * support I would not have made it through the project.  I have also used
 * bits of code from the dttsp package by Bob McGwire (ab2kt) for the resampleing
 * necessary when using 96000 or 192000 with Ozy Janus boards.
 * 
 * Author: Bob Campbell (joan-bob@bigpond.net.au)
 */ 


char *VERSION = "0.0.1";

#include <stdio.h> 
#include <stdlib.h>

/* Diognostic vairables */
int x=0;
int y=0;
int z=0;

#define BUFSIZE  4096

/* Global Buffers between usb stuff and jackclient */

  float Buf_inlp[BUFSIZE]; 
  float Buf_inrp[BUFSIZE]; 
  float Buf_miclp[BUFSIZE]; 
  float Buf_micrp[BUFSIZE]; 
  float Buf_outlp[BUFSIZE]; 
  float Buf_outrp[BUFSIZE]; 
  float Buf_monlp[BUFSIZE]; 
  float Buf_monrp[BUFSIZE]; 
  int Ozbufferlength;

/* state machine for sequencing usb stuff and jackclient 
	the two callbacks get to go in turn  */
enum ozy_state{
	OZY,
	JACK
	} loop_state=OZY;

//
// this is called with audio from the Janus IO thread -- 
// we copy the data to and from the global buffers ready for jackclient
//
//   on input, inp points to 4 pointers to arrays of floats for input (inL, inR, micL, micR) 
//             outp points to 4 pointers to arrsys of floats for output (outL, outR, monL, monR) 
// 
int AudioCallback(void *inp, void *outp, int framecount, void *timep, int flags, void *userp) { 
  int i;
  float **in_floats; 
  float **out_floats; 
  float *inlp; 
  float *inrp; 
  float *miclp; 
  float *micrp; 
  float *outlp; 
  float *outrp; 
  float *monlp; 
  float *monrp; 
  
if (loop_state==OZY){

	++y; /*debug Var */



  in_floats = (float * *)inp; 
  out_floats = (float * *)outp; 

  // printf("callback frameCount: %d\n", framecount); 
  inlp = in_floats[0]; 
  inrp = in_floats[1]; 
  miclp = in_floats[2]; 
  micrp = in_floats[3]; 

  outlp = out_floats[0]; 
  outrp = out_floats[1]; 
  monlp = out_floats[2]; 
  monrp = out_floats[3]; 

  // copy everything from ozy to global buffers
 
  for ( i = 0; i < framecount; ++i ) { 

	monlp[i] = Buf_monlp[i];
	monrp[i] = Buf_monrp[i];
	Buf_miclp[i] = miclp[i];
	Buf_micrp[i] = micrp[i];

	outlp[i] = Buf_outlp[i];
	outrp[i] = Buf_outrp[i];
	Buf_inlp[i] = inlp[i];
	Buf_inrp[i] = inrp[i];
  
 	}

  Ozbufferlength = framecount;
#if 0
	fprintf(stderr,"Loop:\tMic=%f  ",miclp[0]);
	fprintf(stderr,"\t\tMon=%f  \r",monlp[0]);
#endif
	loop_state=JACK; 
	}
  return 0; 
} 

void usage() {
	fprintf(stderr,"Code Version = %s\n",VERSION);
	fprintf(stderr,"Ozyjack built @: %s %s\n", __DATE__, __TIME__);
	fprintf(stderr,"Ozyjack is HPSDR driver for Ozy Janus boards\n");
	fprintf(stderr,"Useage ozyjack [SAMPLE_RATE] [BUFFER_SIZE]\n");
	exit(3);
	}

int cleanup_exit(int exrc) {
	fprintf(stderr,"Shutting Jack\n");
	jack_shutdown();
	fprintf(stderr,"Shutting Audio\n");
	StopAudio();
	fprintf(stderr,"X ProcessCB=%d\tY AudioCB=%d\tZ Idle loop cnt=%d\n",x,y,z);
	fprintf(stderr,"Stopped. exrc= %d\n",exrc);
	exit(exrc);
	}



int main(int argc, char *argv[])

  { 
  char ibuf[100]; 
  int rc; 
  int OJ_SAMPLE_RATE = 48000; /* Default */
  int OJ_BUFFER_SIZE = 2048;

  if (argc >= 4) {
	usage();
	}

  if (argc == 3) {
	OJ_SAMPLE_RATE = atoi(argv[1]);
	OJ_BUFFER_SIZE = atoi(argv[2]);
	}

  if (argc==2) {
	OJ_SAMPLE_RATE = atoi(argv[1]); /* Buffer size default */
	}

switch(OJ_SAMPLE_RATE) {
	case 48000:
		break;
	case 96000:
		break;
	case 192000:
		break;
	default:
		fprintf(stderr, "Not a valid sample rate 48000 96000 192000 only\n");
		usage();
	}
fprintf(stderr,"SAMPLE RATE = %d  BUFFER SIZE = %d\n",OJ_SAMPLE_RATE,OJ_BUFFER_SIZE);

  rc = IsOzyAttached();
	if (rc==0) {
		fprintf(stderr,"OZY Not Found rc= %d\n",rc);
		exit(4);
		}
	fprintf(stderr,"Found OZY OK.....\n");

#if 0	
//    printf("Press enter to start..."); 
//    fgets(ibuf, sizeof(ibuf), stdin); 
	exit(99);
#endif

  rc = StartAudioNative(OJ_SAMPLE_RATE, OJ_BUFFER_SIZE, AudioCallback, 24, 0); 
	rc=checkjack();
	if (rc==0) {
		fprintf(stderr,"Jackd failure rc= %d\n",rc);
		cleanup_exit(87);
		}

	fprintf(stderr,"Jack client started with rc=%d\n",rc);

 
	printf("Running ....\n");

	while (( rc==1)) {

	fprintf(stderr,"\r");
	rc=checkjack();
	z++;
#if 0
	fprintf(stderr,"Size=%d  ",z);
	fprintf(stderr,"\tBufMic=%f  ",Buf_miclp[0]);
	fprintf(stderr,"\t\tBufMon=%f ",Buf_monlp[0]);
	fprintf(stderr,"\tBufinl=%f  ",Buf_inlp[0]);
	fprintf(stderr,"\t\tBufinr=%f ",Buf_inrp[0]);
	fprintf(stderr,"\t\tBuflen=%d  \r",Ozbufferlength);
#endif

/* if any diag counters get too big zero then all togeather */
	if ((x>= 10000000)|(y>= 10000000)|(z>= 10000000)){
		x=0; y=0; z=0;
	}
 
	if (rc==2) {
		printf("LoopBack Rec Exit State\n");
		}
	}
    fprintf(stderr,"Shutdown  rc=%d.\n", rc); 

    cleanup_exit(rc);
    exit(0);  
} 




