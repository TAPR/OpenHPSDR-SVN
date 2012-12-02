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
 * This is an adapatation of Jack support routines to C for 
 * use on Linux and Bill Tracy's janus ozy utilities.  Without Bill's 
 * support I would not have made it through the project.
 * 
 * Author: Bob Campbell (joan-bob@bigpond.net.au)
 */ 

#include <stdio.h>
#include <errno.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

#include <jack/jack.h>

extern x,y,z; /* debug variables */

jack_port_t *input_port_lp;
jack_port_t *input_port_rp;
jack_port_t *mic_port_lp;
jack_port_t *mic_port_rp;

jack_port_t *output_port_lp;
jack_port_t *output_port_rp;
jack_port_t *mon_port_lp;
jack_port_t *mon_port_rp;

jack_client_t *client;

const char **ports;
const char *client_name;
const char *server_name = NULL;
jack_options_t options = JackNullOption;
jack_status_t status;

/* Flags for sequencing Ozy & Jack */
enum ozy_state{
	OZY,
	JACK
	}

extern  loop_state;


/* a simple state machine for this client */
enum state {
	Init,
	Run,
	Exit
	} client_state = Init;

/**
 * Check all is well and if state is exit clean up
 * if state run just return
 */
int
checkjack() {

	int ret = 0;
	int i;
//fprintf(stderr," jack client state %d \n",client_state);

	if ( client_state == Init)
		{
		x=0;
		fprintf (stderr, " Init State x=%d\n",x);
		  ret= startclient();
		  return(ret);
		}
	if ( client_state == Run)
		{
//		if (x <= 5) {
//		for ( i=0;i<=20;i++) {
//			fprintf(stderr,"%f ",miclp[i]);
//				}
//		fprintf(stderr,"**** X=%d \n",x);
//			}
//			x++;
		return(1);

		}


	if (client_state == Exit) {
		fprintf (stderr, "Exit State - Jack Client stopping \n");
		jack_client_close (client);
		return(Exit);
		}

	fprintf (stderr, " Should never get here..\n");

//	ret=startclient();
	return(ret);
	}

/**
 * The process callback for this JACK application is called in a
 * special realtime thread once for each audio cycle.
 *
 * This client follows a simple rule: when the JACK transport is
 * running, copy the input port to the output.  When it stops, exit.
 */

int
process (jack_nframes_t nframes, void *arg)
{
extern  float Buf_inlp[]; 
extern  float Buf_inrp[]; 
extern  float Buf_miclp[];
extern  float Buf_micrp[]; 
extern  float Buf_outlp[]; 
extern  float Buf_outrp[]; 
extern  float Buf_monlp[]; 
extern  float Buf_monrp[]; 
extern  int Ozbufferlength;

int ret;
int i=0;
float *outlp, *inlp, *outrp, *inrp; /* Temp pointers */
//++z; /*debug Var */

	jack_default_audio_sample_t *in, *out;
	jack_transport_state_t ts = jack_transport_query(client, NULL);

	if ((ts == JackTransportRolling)&&(loop_state==JACK)) {

 		++x; /*debug Var */


// more to be done here bob.c

		outlp = Buf_miclp;
		out = (jack_port_get_buffer (mic_port_lp, nframes));
		memcpy (out, outlp,
			 sizeof (jack_default_audio_sample_t) * nframes);

		inlp = Buf_monlp;
		in = jack_port_get_buffer (mon_port_lp, nframes);
		memcpy (inlp, in,
			 sizeof (jack_default_audio_sample_t) * nframes);

		outrp = Buf_micrp;
		out = (jack_port_get_buffer (mic_port_rp, nframes));
		memcpy (out, outrp,
			 sizeof (jack_default_audio_sample_t) * nframes);

		inrp = Buf_monrp;
		in = jack_port_get_buffer (mon_port_rp, nframes);
		memcpy (inrp, in,
			 sizeof (jack_default_audio_sample_t) * nframes);
//.............
		inlp = Buf_outlp;
		in = (jack_port_get_buffer (output_port_lp, nframes));
		memcpy (inlp, in,
			 sizeof (jack_default_audio_sample_t) * nframes);

		outlp = Buf_inlp;
		out = jack_port_get_buffer (input_port_lp, nframes);
		memcpy (out, outlp,
			 sizeof (jack_default_audio_sample_t) * nframes);

		inrp = Buf_outrp;
		in = (jack_port_get_buffer (output_port_rp, nframes));
		memcpy (inrp, in,
			 sizeof (jack_default_audio_sample_t) * nframes);

		outrp = Buf_inrp;
		out = jack_port_get_buffer (input_port_rp, nframes);
		memcpy (out, outrp,
			 sizeof (jack_default_audio_sample_t) * nframes);

		ret=0;
		}

	if (ts == JackTransportStopped) {
		fprintf (stderr, "\n Process_Jack_Fail...\n");
		client_state = Exit;
//		for (i=0; i <= 10; i++){  /* dump a buffer on fail */
//			fprintf(stderr,"%f ",Buf_monlp[i]);
//			}
		fprintf(stderr," \n");
		fprintf(stderr,"Ozbufferlength=%d \n",Ozbufferlength);
		fprintf(stderr,"jack_default_audio_sample_t=%d \n",(sizeof (jack_default_audio_sample_t)));
		fprintf(stderr,"nframes=%d \n",nframes);
		in = (float *) jack_port_get_buffer (mic_port_lp, nframes);
		ret=1;
		}

	loop_state=OZY;


	return(ret);      

}


/**
 * JACK calls this shutdown_callback if the server ever shuts down or
 * decides to disconnect the client.
 */
void
jack_shutdown ()
{
   jack_client_close (client);

	return;
}

int
startclient()
{ 

	int ret = 0;
	int fs;

	client_name = "OzyJanus";

	/* open a client connection to the JACK server */

	client = jack_client_open (client_name, options, &status, server_name);
	if (client == NULL) {
		fprintf (stderr, "jack_client_open() failed, "
			 "status = 0x%2.0x\n", status);
		if (status & JackServerFailed) {
			fprintf (stderr, "Unable to connect to JACK server\n");
		}
		client_state=Exit;
		return(ret);

	}

	if (status & JackServerStarted) {
		fprintf (stderr, "JACK server started\n");
		ret = 1;
	}
	if (status & JackNameNotUnique) {
		client_name = jack_get_client_name(client);
		fprintf (stderr, "unique name `%s' assigned\n", client_name);
	}


	/* tell the JACK server to call `process()' whenever
	   there is work to be done.
	*/

	jack_set_process_callback (client, process, 0);

	/* tell the JACK server to call `jack_shutdown()' if
	   it ever shuts down, either entirely, or if it
	   just decides to stop calling us.
	*/

	jack_on_shutdown (client, jack_shutdown, 0);

	/* display the current sample rate. 
	 */

	printf ("Jack engine sample rate: %" PRIu32 "\n",
		jack_get_sample_rate (client));

	/* create ports */

	input_port_lp = jack_port_register (client, "OJ_inputlp",
					 JACK_DEFAULT_AUDIO_TYPE,
					 JackPortIsOutput, 0);
	input_port_rp = jack_port_register (client, "OJ_inputrp",
					 JACK_DEFAULT_AUDIO_TYPE,
					 JackPortIsOutput, 0);
	mic_port_lp = jack_port_register (client, "OJ_miclp",
					 JACK_DEFAULT_AUDIO_TYPE,
					 JackPortIsOutput, 0);
	mic_port_rp = jack_port_register (client, "OJ_micrp",
					 JACK_DEFAULT_AUDIO_TYPE,
					 JackPortIsOutput, 0);

	output_port_lp = jack_port_register (client, "OJ_outputlp",
					  JACK_DEFAULT_AUDIO_TYPE,
					  JackPortIsInput, 0);
	output_port_rp = jack_port_register (client, "OJ_outputrp",
					  JACK_DEFAULT_AUDIO_TYPE,
					  JackPortIsInput, 0);
	mon_port_lp = jack_port_register (client, "OJ_monlp",
					  JACK_DEFAULT_AUDIO_TYPE,
					  JackPortIsInput, 0);
	mon_port_rp = jack_port_register (client, "OJ_monrp",
					  JACK_DEFAULT_AUDIO_TYPE,
					  JackPortIsInput, 0);

// by testing the last one registered in each group is ok
	if ((mic_port_rp == NULL) || (mon_port_rp == NULL)) {
		fprintf(stderr, "not enough JACK ports available\n");
		ret=0;
		client_state=Exit;
		return(ret);


	}

	/* Tell the JACK server that we are ready to roll.  Our
	 * process() callback will start running now. */

	if (jack_activate (client)) {
		fprintf (stderr, "cannot activate client");
		ret=0;
		client_state=Exit;
		return(ret);

	}

	/* Connect the ports.  You can't do this before the client is
	 * activated, because we can't make connections to clients
	 * that aren't running.  Note the confusing (but necessary)
	 * orientation of the driver backend ports: playback ports are
	 * "input" to the backend, and capture ports are "output" from
	 * it.
	 */

	ports = jack_get_ports (client, NULL, NULL,
				JackPortIsPhysical|JackPortIsOutput);
	if (ports == NULL) {
		fprintf(stderr, "no physical capture ports\n");
		ret=0;
		client_state=Exit;
		return(ret);

	}
#if 0
//   This does connects at the end of the init function.

	if (jack_connect (client, ports[0], jack_port_name (mon_port_lp))) {
		fprintf (stderr, "cannot connect input ports\n");
	}

	free (ports);
	
	ports = jack_get_ports (client, NULL, NULL,
				JackPortIsPhysical|JackPortIsInput);
	if (ports == NULL) {
		fprintf(stderr, "no physical playback ports\n");
		ret=0;
		client_state=Exit;
		return(ret);

	}
// more to be done here bob.c
	if (jack_connect (client, jack_port_name (mic_port_lp), ports[0])) {
		fprintf (stderr, "cannot connect output ports\n");
		ret=0;
		client_state=Exit;
		return(ret);

	}
#endif
	free (ports);

	client_state = Run;
	ret=1;
	return (ret);	/* All OK */
}

