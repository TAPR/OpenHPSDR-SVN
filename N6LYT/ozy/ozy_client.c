/* 
 * File:   ozy_client.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 15 December 2008, 17:29
 */

#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <sys/timeb.h>
#include <semaphore.h>

#include "global.h"
#include "ozy.h"
#include "ozyio.h"
#include "ozy_buffers.h"
#include "ozy_ringbuffer.h"
#include "ozy_client.h"
#include "ozy_to_jack.h"

#define USE_EP6
#define USE_EP4

/*
 * 
 */

struct OzyHandle* ozy;

pthread_t io_thread_id;

int frequency=7056000;
int frequency_changed=1;


unsigned char control_in[5]={0x00,0x00,0x00,0x00,0x00};
int state_in=SYNC_0;
int data_state_in=LEFT_HIGH;
int samples_in=0;
int total_samples_in=0;

unsigned char control_out[5]={0x00,0x00,0x00,0x00,0x00};
int state_out=SYNC_0;
int data_state_out=LEFT_RX_HIGH;
int samples_out=0;
int total_samples_out=0;

int output_sample_increment=1; // 1=48000 2=96000 4=19200

int sample;
int left_sample;
int right_sample;
int mic_sample;

int left_rx_sample;
int right_rx_sample;
int left_tx_sample;
int left_sample;
int right_sample;
int mic_sample;

int left_rx_sample;
int right_rx_sample;
int left_tx_sample;
int right_tx_sample;

float left_sample_float;
float right_sample_float;
float mic_sample_float;

float left_rx_sample_float;
float right_rx_sample_float;
float left_tx_sample_float;
float right_tx_sample_float;

int fifo=0;

int sync_point=0;

int mox_state_changed=0;

void* ozy_io_thread(void* arg) {
    struct ozy_buffer* ozy_buffer;
    char buffer[512];
    int i;
    int bytes;
    struct timeb tb;

    
    if(debug) fprintf(stderr,"ozy_io_thread\n");
    while(1) {

        // read an input buffer (blocks until all bytes read)
        if(debug_buffers) {
            ftime(&tb);
            fprintf(stderr,"OzyBulkRead: %ld.%d\n",tb.time,tb.millitm);
        }
        ozy_buffer=get_ozy_free_buffer();
        if(ozy_buffer!=NULL) {

#ifdef USE_EP6
            bytes=OzyBulkRead(ozy,0x86,(void*)(ozy_buffer->buffer),OZY_BUFFER_SIZE);
            if (bytes < 0) {
                perror("OzyBulkRead failed ep=0x86");
            } else if (bytes != OZY_BUFFER_SIZE) {
                fprintf(stderr,"OzyBulkRead read (ep=0x86) only got %d bytes\n",bytes);
            } else {
                /*if(debug_buffers)*/ fprintf(stderr,"OzyBulkRead read (ep=0x86) got %d bytes\n",bytes);
            }

/*
            if(debug_buffers) {
*/
                if(bytes==OZY_BUFFER_SIZE) {
                    for(i=0;i<4;i++) { // just the first 4 rows (of 32)
                        fprintf(stderr,"ozy ep=0x86 < %04X: %02X%02X%02X%02X%02X%02X%02X%02X %02X%02X%02X%02X%02X%02X%02X%02X\n",
                                i*16,
                                ozy_buffer->buffer[(i*16)],
                                ozy_buffer->buffer[(i*16)+1],
                                ozy_buffer->buffer[(i*16)+2],
                                ozy_buffer->buffer[(i*16)+3],
                                ozy_buffer->buffer[(i*16)+4],
                                ozy_buffer->buffer[(i*16)+5],
                                ozy_buffer->buffer[(i*16)+6],
                                ozy_buffer->buffer[(i*16)+7],
                                ozy_buffer->buffer[(i*16)+8],
                                ozy_buffer->buffer[(i*16)+9],
                                ozy_buffer->buffer[(i*16)+10],
                                ozy_buffer->buffer[(i*16)+11],
                                ozy_buffer->buffer[(i*16)+12],
                                ozy_buffer->buffer[(i*16)+13],
                                ozy_buffer->buffer[(i*16)+14],
                                ozy_buffer->buffer[(i*16)+15]
                                );
                    }
                }
/*
            }
*/
            // just ignore end point 6 for now
#endif


#ifdef USE_EP4
            bytes=OzyBulkRead(ozy,0x84,(void*)(ozy_buffer->buffer),OZY_BUFFER_SIZE);
            if (bytes < 0) {
                perror("OzyBulkRead failed ep=0x84");
            } else if (bytes != OZY_BUFFER_SIZE) {
                fprintf(stderr,"OzyBulkRead read (ep=0x84) only got %d bytes\n",bytes);
            } else {
                /*if(debug_buffers)*/ fprintf(stderr,"OzyBulkRead read (ep=0x84) got %d bytes\n",bytes);
            }


/*
            if(debug_buffers) {
*/
                if(bytes==OZY_BUFFER_SIZE) {
                    for(i=0;i<4/*32*/;i++) { // just the first 4 rows
                        fprintf(stderr,"ozy ep=0x84 < %04X: %02X%02X%02X%02X%02X%02X%02X%02X %02X%02X%02X%02X%02X%02X%02X%02X\n",
                                i*16,
                                ozy_buffer->buffer[(i*16)],
                                ozy_buffer->buffer[(i*16)+1],
                                ozy_buffer->buffer[(i*16)+2],
                                ozy_buffer->buffer[(i*16)+3],
                                ozy_buffer->buffer[(i*16)+4],
                                ozy_buffer->buffer[(i*16)+5],
                                ozy_buffer->buffer[(i*16)+6],
                                ozy_buffer->buffer[(i*16)+7],
                                ozy_buffer->buffer[(i*16)+8],
                                ozy_buffer->buffer[(i*16)+9],
                                ozy_buffer->buffer[(i*16)+10],
                                ozy_buffer->buffer[(i*16)+11],
                                ozy_buffer->buffer[(i*16)+12],
                                ozy_buffer->buffer[(i*16)+13],
                                ozy_buffer->buffer[(i*16)+14],
                                ozy_buffer->buffer[(i*16)+15]
                                );
                    }
                }
/*
            }
*/
#endif

            // process input buffer
            put_ozy_input_buffer(ozy_buffer);
            sem_post(&ozy_input_buffer_sem);

        }
        
        // create an output buffer
        if(fifo>=OZY_BUFFER_SIZE && ozy_ringbuffer_entries(ozy_output_buffer)>=63) {
            buffer[0]=SYNC;
            buffer[1]=SYNC;
            buffer[2]=SYNC;
            if(frequency_changed) {
                buffer[3]=control_out[0]|0x02;
                buffer[4]=frequency>>24;
                buffer[5]=frequency>>16;
                buffer[6]=frequency>>8;
                buffer[7]=frequency;
                frequency_changed=0;
            } else {
                buffer[3]=control_out[0];
                buffer[4]=control_out[1];
                buffer[5]=control_out[2];
                buffer[6]=control_out[3];
                buffer[7]=control_out[4];
            }

            bytes=ozy_ringbuffer_get(ozy_output_buffer,&buffer[8],OZY_BUFFER_SIZE-8);
            if(bytes!=(OZY_BUFFER_SIZE-8)) {
                if(debug_buffers) fprintf(stderr,"ozy output buffer underrun got %d bytes\n",bytes);
            }
            if(debug_buffers) {
                for(i=0;i<4/*32*/;i++) { // just the first 4 rows
                    fprintf(stderr,"ozy> %04X: %02X%02X%02X%02X%02X%02X%02X%02X %02X%02X%02X%02X%02X%02X%02X%02X\n",
                            i,
                            buffer[(i*16)],
                            buffer[(i*16)+1],
                            buffer[(i*16)+2],
                            buffer[(i*16)+3],
                            buffer[(i*16)+4],
                            buffer[(i*16)+5],
                            buffer[(i*16)+6],
                            buffer[(i*16)+7],
                            buffer[(i*16)+8],
                            buffer[(i*16)+9],
                            buffer[(i*16)+10],
                            buffer[(i*16)+11],
                            buffer[(i*16)+12],
                            buffer[(i*16)+13],
                            buffer[(i*16)+14],
                            buffer[(i*16)+15]
                            );
                }
            }

            bytes=OzyBulkWrite(ozy,0x02,(void*)(buffer),OZY_BUFFER_SIZE);
            if(bytes!=OZY_BUFFER_SIZE) {
                perror("OzyBulkWrite failed");
            } else {
                if(debug_buffers) fprintf(stderr,"OzyBulkWrite %d bytes\n",bytes);
            }
            fifo=fifo-bytes;
        }
    }
}

int start_ozy_io_thread() {
    int rc;
    
    if(debug) fprintf(stderr,"start_ozy_client\n");

    // start the client thread
    rc=pthread_create(&io_thread_id,NULL,ozy_io_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on ozy_io_thread: rc=%d\n", rc);
    }
    
    return rc;
}

int create_ozy_client() {
    int rc;

    // setup defaults
    control_out[0]=MOX_DISABLED;
    control_out[1]=CONFIG_MERCURY
                  |MERCURY_122_88MHZ_SOURCE
                  |MERCURY_10MHZ_SOURCE
                  |SPEED_48KHZ
                  |MIC_SOURCE_JANUS/*MIC_SOURCE_PENELOPE*/;
    control_out[2]=MODE_OTHERS;
    control_out[3]=ALEX_ATTENUATION_0DB
                  |LT2208_GAIN_OFF
                  |LT2208_DITHER_OFF
                  |LT2208_RANDOM_OFF;
    control_out[4]=0;

    if(debug) fprintf(stderr,"create_ozy_client\n");

    // start a thread to read/write to the USB ports

    rc = IsOzyAttached();
    if (rc == 0) {
        perror("Cannot locate Ozy");
        return (EXIT_FAILURE);
    }

    ozy = OzyOpen();
    if (ozy == NULL) {
        perror("OzyOpen failed\n");
        return (EXIT_FAILURE);
    }

    rc=start_ozy_io_thread();

    return rc;
}

