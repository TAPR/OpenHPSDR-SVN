/* 
 * File:   ozy_to_jack.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 22 December 2008, 17:09
 */

#include <stdio.h>
#include <stdlib.h>
#include <semaphore.h>

#include "global.h"
#include "jack_ringbuffer.h"
#include "ozy_buffers.h"
#include "ozy_to_jack.h"
#include "ozy_client.h"
#include "spectrum_buffers.h"

//#define USE_SYNC

/*
 * OZY sends 512 byte frames
 * 
 */

int ozy_state_in=SYNC_0;
int ozy_data_state_in=LEFT_HIGH;

pthread_t ozy_to_jack_thread_id;

struct jack_buffer* jack_buffer;
int jack_buffer_index;
int ozy_samples_in=0;
int ozy_total_samples_in=0;



int left_sample;
int right_sample;
int mic_sample;

float left_sample_float;
float right_sample_float;
float mic_sample_float;

int sync_point;

int min_left=8388607;
int max_left=-8388607;
int min_right=8388607;
int max_right=-8388607;

unsigned char ozy_previous_bytes[2]={0x00,0x00};


struct spectrum_buffer* spectrum_buffer;
int adc_spectrum_index=0;
int collect_adc_spectrum=0;

int frame=0;

void process_ozy_samples(char byte) {

    switch(ozy_data_state_in) {
        case LEFT_HIGH:
            left_sample=byte<<16;
            ozy_data_state_in=LEFT_MIDDLE;
            break;
        case LEFT_MIDDLE:
            left_sample=left_sample+((byte&0xFF)<<8);
            ozy_data_state_in=LEFT_LOW;
            break;
        case LEFT_LOW:
            left_sample=left_sample+(byte&0xFF);
            ozy_data_state_in=RIGHT_HIGH;
            break;
        case RIGHT_HIGH:
            right_sample=byte<<16;
            ozy_data_state_in=RIGHT_MIDDLE;
            break;
        case RIGHT_MIDDLE:
            right_sample=right_sample+((byte&0xFF)<<8);
            ozy_data_state_in=RIGHT_LOW;
            break;
        case RIGHT_LOW:
            right_sample=right_sample+(byte&0xFF);
            ozy_data_state_in=MIC_HIGH;
            break;
        case MIC_HIGH:
            mic_sample=byte<<8;
            ozy_data_state_in=MIC_LOW;
            break;
        case MIC_LOW:
            mic_sample=mic_sample+(byte&0xFF);

            // have set of samples - convert to float (range -1.0 to +1.0)and save in jack buffers
            left_sample_float=(float)left_sample/8388607.0; // 24 bit sample
            right_sample_float=(float)right_sample/8388607.0; // 24 bit sample
#ifdef USE_SYNC
            if(collect_adc_spectrum==1) {
#endif
                if(adc_spectrum_index<4096) {

                    //fprintf(stderr,"ADC %d=%d (%f)\n",adc_spectrum_index,mic_sample,(float)mic_sample/32767.0f);
                    
                    spectrum_buffer->buffer[adc_spectrum_index++]=(float)mic_sample/32767.0f; // 16 bit samples
                    if(adc_spectrum_index==4096) {
                        // spectrum_buffer->buffer[adc_spectrum_index++]=0.0f;  // pad the buffer
                        fprintf(stderr,"got 4096 spectrum samples frame=%d\n",frame);
                        // put in queue to be sent to a frienly client
                        put_spectrum_input_buffer(spectrum_buffer);
                        sem_post(&spectrum_input_buffer_sem);

                        spectrum_buffer=get_spectrum_free_buffer();
                        adc_spectrum_index=0;
                        if(spectrum_buffer==NULL) {
                            fprintf(stderr,"ozy_to_jack: failed to get_spectrum_free_buffer\n");
                            collect_adc_spectrum=0;
                        }
                    }
                }
                mic_sample_float=0.0;
#ifdef USE_SYNC
            } else {
                mic_sample_float=(float)mic_sample/32767.0; // 16 bit sample
            }
#endif
            if(jack_ringbuffer_put(left_sample_float,right_sample_float,mic_sample_float,mic_sample_float)==0) {
                // buffer overflow
            }
            
            if(debug_rx_samples) {
                if(left_sample<min_left) min_left=left_sample;
                if(left_sample>max_left) max_left=left_sample;
                if(right_sample<min_right) min_right=right_sample;
                if(right_sample>max_right) max_right=right_sample;

                fprintf(stderr,"ozy rx samples: left=%d(%f) right=%d(%f)\n",left_sample,left_sample_float,right_sample,right_sample_float);
                fprintf(stderr,"min_left=%d max_left=%d min_right=%d max_right=%d\n",min_left,max_left,min_right,max_right);
            }

            if(debug_mic_samples) fprintf(stderr,"ozy mic samples: mic=%d(%f)\n",mic_sample,mic_sample_float);
                              

            ozy_samples_in++;
            ozy_total_samples_in++;

#ifdef USE_SYNC
            if(ozy_samples_in>=OZY_SAMPLES_PER_BUFFER) {
                ozy_state_in=SYNC_0;
            } else {
                ozy_data_state_in=LEFT_HIGH;
            }
#else
            ozy_data_state_in=LEFT_HIGH;
#endif
            break;
    }
}

void process_ozy_byte(unsigned char byte) {
    //if(debug) fprintf(stderr,"%02X\n",byte);
    switch(ozy_state_in) {
        case SYNC_0:
            if(byte==SYNC) {
                ozy_state_in=SYNC_1;
            }
            break;
        case SYNC_1:
            if(byte==SYNC) {
                ozy_state_in=SYNC_2;
            } else {
                ozy_state_in=SYNC_0;
            }
            break;
        case SYNC_2:
            if(byte==SYNC) {
                ozy_state_in=CONTROL_0;
            } else {
                ozy_state_in=SYNC_0;
            }
            break;   
        case CONTROL_0:
            frame++;
            // extract out PTT/DOT and DASH
            if(byte!=0) {
                if(debug) fprintf(stderr,"control_in[0]=%02X\n",byte);
            }
            ozy_state_in=CONTROL_1;
            break;
        case CONTROL_1:
            // extract out LT2208 Overflow
            if((byte&0x01)==0x01) {
                fprintf(stderr,"LT2208 Overflow\n");
            }
            if((byte&0x02)==0x02) {
                // start of ADC Spectrum
                fprintf(stderr,"start of ADC frame=%d\n",frame);
                spectrum_buffer=get_spectrum_free_buffer();
                if(spectrum_buffer!=NULL) {
                    collect_adc_spectrum=1;
                    adc_spectrum_index=0;
                } else {
                    fprintf(stderr,"ozy_to_jack: failed to get_spectrum_free_buffer\n");
                }
            }
            ozy_state_in=CONTROL_2;
            break;
        case CONTROL_2:
            // reserved for future use
            if(byte!=0) {
                if(debug) fprintf(stderr,"RXoverrun\n");
            }
            ozy_state_in=CONTROL_3;
            break;
        case CONTROL_3:
            // extract out number of bytes in receive fifo
            fifo=(byte&0xFF)<<4;
            ozy_state_in=CONTROL_4;
            break;
        case CONTROL_4:
            // reserved for future use
            ozy_state_in=SAMPLES;
            ozy_data_state_in=LEFT_HIGH;
            ozy_samples_in=0;
            break;
        case SAMPLES:
            process_ozy_samples(byte);
            break;
    }    
}

void process_ozy_input_buffer(unsigned char* buffer) {
    int i;
    if(debug) fprintf(stderr,"process_ozy_input_buffer\n");
    for(i=0;i<OZY_BUFFER_SIZE;i++) {
        process_ozy_byte(buffer[i]);
    }
}

void* ozy_to_jack_thread(void* arg) {
    struct ozy_buffer* buffer;
    
#ifdef USE_SYNC
    ozy_state_in=SYNC_0;
#else
    ozy_state_in=SAMPLES;
    ozy_data_state_in=LEFT_HIGH;
    spectrum_buffer=get_spectrum_free_buffer();
    if(spectrum_buffer==NULL) {
        fprintf(stderr,"ozy_to_jack_thread: failed to get_spectrum_free_buffer\n");
    }
    adc_spectrum_index=0;
#endif
    while(1) {
        // wait for an ozy buffer
        sem_wait(&ozy_input_buffer_sem);
        if(debug_buffers) fprintf(stderr,"ozy_to_jack_thread: get_ozy_input_buffer\n");
        buffer=get_ozy_input_buffer();
        if(buffer==NULL) {
            fprintf(stderr,"ozy_to_jack_thread: get_ozy_buffer returned NULL!\n");
        } else {
            process_ozy_input_buffer(buffer->buffer);
            free_ozy_buffer(buffer);
        }

    }
}

int create_ozy_to_jack() {
    int rc;
    if(debug) fprintf(stderr,"create_ozy_to_jack\n");
    jack_buffer=NULL;
    rc=pthread_create(&ozy_to_jack_thread_id,NULL,ozy_to_jack_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on ozy_to_jack_thread: rc=%d\n", rc);
    }
}

