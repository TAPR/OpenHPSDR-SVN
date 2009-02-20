/* 
 * File:   main.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 15 December 2008, 17:29
 */

#include <semaphore.h>
#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <malloc.h>

#include "global.h"
#include "ozy.h"
#include "jack_ringbuffer.h"
#include "jack_buffers.h"
#include "ozy_ringbuffer.h"
#include "ozy_buffers.h"
#include "spectrum_buffers.h"

int debug;
int debug_buffers;
int debug_commands;
int debug_mic_samples;
int debug_rx_samples;
int debug_spectrum;

int mox_state_changed;
int total_samples_in;
int total_samples_out;

int command_port=DEFAULT_COMMAND_PORT;

/*
 * 
 */

void start_threads() {
    int rc;

    // create transform threads
    rc=create_jack_to_ozy();
    rc=create_ozy_to_jack();

    // create client threads
    rc=create_ozy_client();
    rc=create_jack_client();
#ifdef SPECTRUM_THREAD
    rc=create_spectrum_thread();
#endif
    
    // create the command processor thread
    rc=create_command_processor();
}

int main(int argc, char** argv) {

    // TODO - add command line parsing to change defaults
    debug=0;
    debug_buffers=0;
    debug_commands=0;
    debug_mic_samples=0;
    debug_rx_samples=0;
    debug_spectrum=0;

    sem_init(&ozy_input_buffer_sem,0,0);
    sem_init(&jack_input_buffer_sem,0,0);
    sem_init(&spectrum_input_buffer_sem,0,0);
    
    
    create_jack_ringbuffer(4*1024);
    create_jack_buffers(8);
    
    create_ozy_ringbuffer(68*512);
    create_ozy_buffers(34);

    create_spectrum_buffers(4);

    start_threads();

    while(1) {
        if(debug) {
            sleep(1);
            fprintf(stderr,"malloc_stats\n");
            malloc_stats();
        } else {
            sleep(10);
        }
    }
}
