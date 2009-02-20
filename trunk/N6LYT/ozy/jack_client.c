/* 
 * File:   jack_client.c
 * Author: John Melton G0ORX/N6LYT
 *
 * on each process callback:
 *
 *   transfer (and translate) the output data from dttsp and put into
 *   a output buffer ready for ozy.
 *
 *   take out out data from ozy and put into the input buffer for dttsp.
 *
 * Created on 15 December 2008, 15:33
 */

#include <stdio.h>
#include <stdlib.h>

#include <stdio.h>
#include <errno.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>
#include <semaphore.h>

#include <jack/jack.h>

#include "global.h"
#include "ozy_client.h"
#include "jack_buffers.h"
#include "jack_ringbuffer.h"

jack_port_t *rx_in_l;
jack_port_t *rx_in_r;
jack_port_t *mic_in_l;
jack_port_t *mic_in_r;
jack_port_t *mon_out_l;
jack_port_t *mon_out_r;
jack_port_t *tx_out_l;
jack_port_t *tx_out_r;

char * client_name="ozy";
jack_client_t *client;

jack_default_audio_sample_t *samples_rx_in_l,
                            *samples_rx_in_r,
                            *samples_mic_in_l,
                            *samples_mic_in_r;

jack_default_audio_sample_t *samples_mon_out_l,
                            *samples_mon_out_r,
                            *samples_tx_out_l,
                            *samples_tx_out_r;

int buffer_count=0;


int jack_callback(jack_nframes_t nframes, void *arg) {
    
    struct jack_buffer* jack_buffer;

    int i;

    jack_time_t start_time;
    jack_time_t end_time;

    start_time=jack_get_time();

    if(debug) fprintf(stderr,"jack_callback entry time=%llu\n",start_time);
    // put the output samples
    samples_rx_in_l=jack_port_get_buffer(rx_in_l,nframes);
    samples_rx_in_r=jack_port_get_buffer(rx_in_r,nframes);
    samples_mic_in_l=jack_port_get_buffer(mic_in_l,nframes);
    samples_mic_in_r=jack_port_get_buffer(mic_in_r,nframes);

    jack_ringbuffer_get(samples_rx_in_l,samples_rx_in_r,samples_mic_in_l,samples_mic_in_r,nframes,buffer_count);

    if(debug_rx_samples) {
        // first 8 samples
        fprintf(stderr,"rx_samples: %f %f %f %f %f %f %f %f\n",
                samples_rx_in_l[0],
                samples_rx_in_l[1],
                samples_rx_in_l[2],
                samples_rx_in_l[3],
                samples_rx_in_l[4],
                samples_rx_in_l[5],
                samples_rx_in_l[6],
                samples_rx_in_l[7]
                );
    }

    // get the input samples
    samples_mon_out_l=jack_port_get_buffer(mon_out_l,nframes);
    samples_mon_out_r=jack_port_get_buffer(mon_out_r,nframes);
    samples_tx_out_l=jack_port_get_buffer(tx_out_l,nframes);
    samples_tx_out_r=jack_port_get_buffer(tx_out_r,nframes);

    jack_buffer=get_jack_free_buffer();
    if(jack_buffer!=NULL) {
        jack_buffer->nframes=nframes;
        memcpy(jack_buffer->buffer_1,samples_mon_out_l,sizeof(jack_default_audio_sample_t)*nframes);
        memcpy(jack_buffer->buffer_2,samples_mon_out_r,sizeof(jack_default_audio_sample_t)*nframes);
        memcpy(jack_buffer->buffer_3,samples_tx_out_l,sizeof(jack_default_audio_sample_t)*nframes);
        memcpy(jack_buffer->buffer_4,samples_tx_out_r,sizeof(jack_default_audio_sample_t)*nframes);

        if(debug) fprintf(stderr,"jack_callback: put_jack_input_buffer\n");
        put_jack_input_buffer(jack_buffer);
        sem_post(&jack_input_buffer_sem);
    } else {
        fprintf(stderr,"jack_callback: get_jack_free_buffer returned NULL\n");
    }

    
    
    end_time=jack_get_time();
    buffer_count++;
    if(debug) fprintf(stderr,"jack_callback duration %llu\n",end_time-start_time);
    return 0;
}

void jack_shutdown() {
    if(debug) fprintf(stderr,"jack_shutdown called\n");
    exit(1);
}


int open_jack_client() {
    const char *server_name = NULL;
    jack_options_t options = JackNullOption;
    jack_status_t status;

    client = jack_client_open (client_name, options, &status, server_name);
    if (client == NULL) {
            fprintf (stderr, "jack_client_open() failed, "
                     "status = 0x%2.0x\n", status);
            if (status & JackServerFailed) {
                    fprintf (stderr, "Unable to connect to JACK server\n");
            }
            exit (1);
    }
    if (status & JackServerStarted) {
            if(debug) fprintf (stderr, "JACK server started\n");
    }
    if (status & JackNameNotUnique) {
            client_name = jack_get_client_name(client);
            fprintf (stderr, "unique name `%s' assigned\n", client_name);
    }

    // setup the callback function to process data
    jack_set_process_callback (client, jack_callback, 0);

    /* tell the JACK server to call `jack_shutdown()' if
       it ever shuts down, either entirely, or if it
       just decides to stop calling us.
    */

    // setup the callback function if jack shuts down
    jack_on_shutdown (client, jack_shutdown, 0);
}

void create_jack_ports() {

    // Ouput port for data received from ozy
    rx_in_l = jack_port_register (client, "rx_in_l",
                                         JACK_DEFAULT_AUDIO_TYPE,
                                         JackPortIsOutput, 0);
    if(rx_in_l == NULL) {
         fprintf(stderr, "failed to register rx_in_l\n");
         exit (1);
    }
    rx_in_r = jack_port_register (client, "rx_in_r",
                                         JACK_DEFAULT_AUDIO_TYPE,
                                         JackPortIsOutput, 0);
    if(rx_in_r == NULL) {
         fprintf(stderr, "failed to register rx_in_r\n");
         exit (1);
    }
    mic_in_l = jack_port_register (client, "mic_in_l",
                                         JACK_DEFAULT_AUDIO_TYPE,
                                         JackPortIsOutput, 0);
    if(mic_in_l == NULL) {
         fprintf(stderr, "failed to register mic_in_l\n");
         exit (1);
    }
    mic_in_r = jack_port_register (client, "mic_in_r",
                                         JACK_DEFAULT_AUDIO_TYPE,
                                         JackPortIsOutput, 0);
    if(mic_in_r == NULL) {
         fprintf(stderr, "failed to register mic_in_r\n");
         exit (1);
    }

    // input ports for data to ozy
    mon_out_l = jack_port_register (client, "mon_out_l",
                                          JACK_DEFAULT_AUDIO_TYPE,
                                          JackPortIsInput, 0);
    if(mon_out_l == NULL) {
         fprintf(stderr, "failed to register mon_out_l\n");
         exit (1);
    }
    mon_out_r = jack_port_register (client, "mon_out_r",
                                          JACK_DEFAULT_AUDIO_TYPE,
                                          JackPortIsInput, 0);
    if(mon_out_r == NULL) {
         fprintf(stderr, "failed to register mon_out_r\n");
         exit (1);
    }
    tx_out_l = jack_port_register (client, "tx_out_l",
                                          JACK_DEFAULT_AUDIO_TYPE,
                                          JackPortIsInput, 0);
    if(tx_out_l == NULL) {
         fprintf(stderr, "failed to register tx_out_l\n");
         exit (1);
    }
    tx_out_r = jack_port_register (client, "tx_out_r",
                                          JACK_DEFAULT_AUDIO_TYPE,
                                          JackPortIsInput, 0);
    if(tx_out_r == NULL) {
         fprintf(stderr, "failed to register tx_out_r\n");
         exit (1);
    }
}

int create_jack_client() {

    if(debug) fprintf(stderr,"create_jack_client\n");

    open_jack_client();
    create_jack_ports();
    
    if(debug) fprintf(stderr, "jack_activate\n");
    if (jack_activate (client)) {
        fprintf (stderr, "cannot activate client\n");
        exit (1);
    }

    return (EXIT_SUCCESS);
    
}
