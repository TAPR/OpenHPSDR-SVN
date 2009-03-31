/* 
 * File:   ozy.c
 * Author: jm57878
 *
 * Created on 10 March 2009, 20:26
 */

#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <semaphore.h>
#include <time.h>

#include "ozy.h"
#include "ozy_buffers.h"
#include "ozy_ringbuffer.h"
#include "spectrum_buffers.h"
#include "dttsp.h"
#include "util.h"

/*
 *   ozy interface
 */

#define USB_TIMEOUT -7
struct OzyHandle* ozy;

pthread_t ep6_ep2_io_thread_id;
pthread_t ep4_io_thread_id;
pthread_t ozy_input_buffer_thread_id;
pthread_t ozy_spectrum_buffer_thread_id;

int frequency=7056000;
int frequency_changed=1;


unsigned char control_in[5]={0x00,0x00,0x00,0x00,0x00};

unsigned char control_out[5]={0x00,0x00,0x00,0x00,0x00};

int output_sample_increment=1; // 1=48000 2=96000 4=192000

int buffer_size=1024;

float left_input_buffer[1024];
float right_input_buffer[1024];

float left_output_buffer[1024];
float right_output_buffer[1024];

int samples=0;

int left_sample;
int right_sample;
int mic_sample;

float left_sample_float;
float right_sample_float;
float mic_sample_float;

short left_rx_sample;
short right_rx_sample;
short left_tx_sample;
short right_tx_sample;

int frames=0;
int usb_output_buffers=0;

int show_software_serial_numbers=1;

unsigned char spectrum_samples[SPECTRUM_BUFFER_SIZE];

int lt2208ADCOverflow=0;

void process_ozy_input_buffer(char* buffer) {
    int i,j;
    int b=0;
    int c=0;
    unsigned char ozy_samples[8*8];

    if(buffer[b++]==SYNC && buffer[b++]==SYNC && buffer[b++]==SYNC) {
        // extract control bytes
        control_in[0]=buffer[b++];
        control_in[1]=buffer[b++];
        control_in[2]=buffer[b++];
        control_in[3]=buffer[b++];
        control_in[4]=buffer[b++];

if(control_in[0]&0x01) {
    // PPT/DOT
    fprintf(stderr,"PTT/DOT\n");
} else if(control_in[0]&0x02) {
    // DASH
    fprintf(stderr,"DASH\n");
}

if(control_in[1]&0x01) {
    lt2208ADCOverflow=1;
}

if(show_software_serial_numbers) {
    fprintf(stderr,"Software Serial Numbers:\n");
    fprintf(stderr,"  Mercury: %d\n",control_in[2]);
    fprintf(stderr,"  Penelope: %d\n",control_in[3]);
    fprintf(stderr,"  Ozy: %d\n",control_in[4]);
    show_software_serial_numbers=0;
}

        // extract the 63 samples
        for(i=0;i<63;i++) {
            left_sample = buffer[b++] << 16;
            left_sample = left_sample + ((buffer[b++] & 0xFF) << 8);
            left_sample = left_sample + (buffer[b++] & 0xFF);
            right_sample = buffer[b++] << 16;
            right_sample = right_sample + ((buffer[b++] & 0xFF) << 8);
            right_sample = right_sample + (buffer[b++] & 0xFF);
            mic_sample=buffer[b++]<<8;
            mic_sample=mic_sample+(buffer[b++]&0xFF);

            left_sample_float=(float)left_sample/8388607.0; // 24 bit sample
            right_sample_float=(float)right_sample/8388607.0; // 24 bit sample
            mic_sample_float=(float)mic_sample/32767.0; // 16 bit sample

            // add to buffer
            left_input_buffer[samples]=left_sample_float;
            right_input_buffer[samples]=right_sample_float;
            samples++;

            // when we have enough samples give them to DttSP and get the results
            if(samples==buffer_size) {
                // give to DttSP and get any output
                //fprintf(stderr,"call dttsp: frame=%d\n",frames);
                audio_callback (left_input_buffer,right_input_buffer,
                                left_output_buffer,right_output_buffer, buffer_size);

                // process the output
                for(j=0,c=0;j<buffer_size;j+=output_sample_increment) {
                    left_rx_sample=(short)(left_output_buffer[j]*32767.0);
                    right_rx_sample=(short)(right_output_buffer[j]*32767.0);
                    left_tx_sample=0/*(short)(buffer->buffer_3[j]*32767.0)*/;
                    right_tx_sample=0/*(short)(buffer->buffer_4[j]*32767.0)*/;

                    ozy_samples[c]=left_rx_sample>>8;
                    ozy_samples[c+1]=left_rx_sample;
                    ozy_samples[c+2]=right_rx_sample>>8;
                    ozy_samples[c+3]=right_rx_sample;
                    ozy_samples[c+4]=left_tx_sample>>8;
                    ozy_samples[c+5]=left_tx_sample;
                    ozy_samples[c+6]=right_tx_sample>>8;
                    ozy_samples[c+7]=right_tx_sample;
                    c+=8;
                    if(c==64) {
                        if(ozy_ringbuffer_put(ozy_output_buffer,ozy_samples,c)!=c) {
                            // ozy output buffer overflow
                        }
                        c=0;
                    }
                }
                
                
                samples=0;
                frames++;
            }
        }
    } else {
        time_t t;
        struct tm* gmt;
        time(&t);
        gmt=gmtime(&t);

        fprintf(stderr,"%s: process_ozy_input_buffer: did not find sync\n",
                asctime(gmt));
        dump_ozy_buffer("buffer",buffer);
    }

}

void* ozy_input_buffer_thread(void* arg) {
    struct ozy_buffer* buffer;
    while(1) {
        // wait for an ozy buffer
        sem_wait(&ozy_input_buffer_sem);
        buffer=get_ozy_input_buffer();
        if(buffer==NULL) {
            fprintf(stderr,"ozy_input_buffer_thread: get_ozy_buffer returned NULL!\n");
        } else {
            process_ozy_input_buffer(buffer->buffer);
            free_ozy_buffer(buffer);
        }

    }
}

void* ozy_spectrum_buffer_thread(void* arg) {
    struct spectrum_buffer* spectrum_buffer;
    while(1) {
        sem_wait(&spectrum_input_buffer_sem);
        spectrum_buffer=get_spectrum_input_buffer();
        memcpy(spectrum_samples,spectrum_buffer->buffer,SPECTRUM_BUFFER_SIZE);
        free_spectrum_buffer(spectrum_buffer);
    }
}

void* ozy_ep6_ep2_io_thread(void* arg) {
    struct ozy_buffer* ozy_buffer;
    unsigned char output_buffer[OZY_BUFFER_SIZE];
    int bytes;

    while(1) {

        // read an input buffer (blocks until all bytes read)
        ozy_buffer=get_ozy_free_buffer();
        if(ozy_buffer!=NULL) {
            bytes=libusb_read_ozy(0x86,(void*)(ozy_buffer->buffer),OZY_BUFFER_SIZE);
            if (bytes < 0) {
                fprintf(stderr,"ozy_ep6_ep2_io_thread: OzyBulkRead read failed %d\n",bytes);
            } else if (bytes != OZY_BUFFER_SIZE) {
                fprintf(stderr,"ozy_ep6_ep2_io_thread: OzyBulkRead only read %d bytes\n",bytes);
            } else {
                // process input buffer
                put_ozy_input_buffer(ozy_buffer);
                sem_post(&ozy_input_buffer_sem);
            }

        }

        // see if we have enough to send a buffer or force the write if we have a timeout
        if((bytes==USB_TIMEOUT) || ozy_ringbuffer_entries(ozy_output_buffer)>=(OZY_BUFFER_SIZE-8)) {
            
            output_buffer[0]=SYNC;
            output_buffer[1]=SYNC;
            output_buffer[2]=SYNC;

            if(frequency_changed) {
                output_buffer[3]=control_out[0]|0x02;
                output_buffer[4]=frequency>>24;
                output_buffer[5]=frequency>>16;
                output_buffer[6]=frequency>>8;
                output_buffer[7]=frequency;
                frequency_changed=0;
            } else {
                output_buffer[3]=control_out[0];
                output_buffer[4]=control_out[1];
                output_buffer[5]=control_out[2];
                output_buffer[6]=control_out[3];
                output_buffer[7]=control_out[4];
            }

            if(ozy_ringbuffer_entries(ozy_output_buffer)>=(OZY_BUFFER_SIZE-8)) {
                bytes=ozy_ringbuffer_get(ozy_output_buffer,&output_buffer[8],OZY_BUFFER_SIZE-8);
                if(bytes!=OZY_BUFFER_SIZE-8) {
                    fprintf(stderr,"OOPS - thought there was enough for usb output but only %d\n",bytes);
                }
            }

            bytes=libusb_write_ozy(0x02,(void*)(output_buffer),OZY_BUFFER_SIZE);
            if(bytes!=OZY_BUFFER_SIZE) {
                perror("OzyBulkWrite failed");
            }

        }

    }
}

void* ozy_ep4_io_thread(void* arg) {
    struct spectrum_buffer* spectrum_buffer;
    int bytes;
    int i;

    while(1) {
        spectrum_buffer=get_spectrum_free_buffer();
        if(spectrum_buffer!=NULL) {
            bytes=libusb_read_ozy(0x84,(void*)(spectrum_buffer->buffer),SPECTRUM_BUFFER_SIZE);
            if (bytes < 0) {
                perror("ozy_ep4_io_thread: OzyBulkRead failed");
            } else if (bytes != SPECTRUM_BUFFER_SIZE) {
                fprintf(stderr,"ozy_ep4_io_thread: OzyBulkRead only read %d bytes\n",bytes);
            }

            // process input buffer
            put_spectrum_input_buffer(spectrum_buffer);
            sem_post(&spectrum_input_buffer_sem);
        } else {
            fprintf(stderr,"ozy_ep4_io_thread: get_spectrum_free_buffer returned NULL!");
        }
    }
}

void getSpectrumSamples(char *samples) {
    memcpy(samples,spectrum_samples,SPECTRUM_BUFFER_SIZE);
}

void setMOX(int state) {
    control_out[0]=control_out[0]&0xFE;
    control_out[0]=control_out[0]|(state&0x01);
}

void setSpeed(int speed) {
    control_out[1]=control_out[1]&0xFC;
    control_out[1]=control_out[1]|speed;
    if(speed==SPEED_48KHZ) {
        output_sample_increment=1;
    } else if(speed==SPEED_96KHZ) {
        output_sample_increment=2;
    } else if(speed==SPEED_192KHZ) {
        output_sample_increment=4;
    }
}

void set10MHzSource(int source) {
    control_out[1]=control_out[1]&0xF3;
    control_out[1]=control_out[1]|(source<<2);
}

void set122MHzSource(int source) {
    control_out[1]=control_out[1]&0xEF;
    control_out[1]=control_out[1]|(source<<4);
}

void setConfig(int config) {
    control_out[1]=control_out[1]&0x9F;
    control_out[1]=control_out[1]|(config<<5);
}


void setMicSource(int source) {
    control_out[1]=control_out[1]&0x7F;
    control_out[1]=control_out[1]|(source<<7);
}

void setMode(int mode) {
    control_out[2]=control_out[2]&0xFE;
    control_out[2]=control_out[2]|mode;
}

void setOCOutputs(int outputs) {
    control_out[2]=control_out[2]&0x01;
    control_out[2]=control_out[2]|(outputs<<1);
}

void setAlexAttenuator(int attenuator) {
    control_out[3]=control_out[3]&0xFC;
    control_out[3]=control_out[3]|attenuator;
}

void setPreampGain(int gain) {
    control_out[3]=control_out[3]&0xFB;
    control_out[3]=control_out[3]|(gain<<2);
}

void setLT2208Dither(int dither) {
    control_out[3]=control_out[3]&0xF7;
    control_out[3]=control_out[3]|(dither<<3);
}

void setLT2208Random(int random) {
    control_out[3]=control_out[3]&0xEF;
    control_out[3]=control_out[3]|(random<<4);
}

void setFrequency(double f) {
    frequency=(int)((f*1000000.0)+0.5);
    frequency_changed=1;
}

int ozy_init(int sample_rate) {
    int rc;

    // open ozy
    rc = libusb_open_ozy();
    if (rc != 0) {
        perror("Cannot locate Ozy");
        return (EXIT_FAILURE);
    }
    
    // setup ozy defaults
    int speed = SPEED_96KHZ;

    switch (sample_rate) {
        case 48000:
            speed = SPEED_48KHZ;
            output_sample_increment = 1;
            break;
        case 96000:
            speed = SPEED_96KHZ;
            output_sample_increment = 2;
            break;
        case 192000:
            speed = SPEED_192KHZ;
            output_sample_increment = 4;
            break;
    }

    // setup defaults
    control_out[0] = MOX_DISABLED;
    control_out[1] = CONFIG_MERCURY
            | MERCURY_122_88MHZ_SOURCE
            | MERCURY_10MHZ_SOURCE
            | speed
            | MIC_SOURCE_PENELOPE;
    control_out[2] = MODE_OTHERS;
    control_out[3] = ALEX_ATTENUATION_0DB
            | LT2208_GAIN_OFF
            | LT2208_DITHER_ON
            | LT2208_RANDOM_ON;
    control_out[4] = 0;

    // create buffers of ozy
    create_ozy_ringbuffer(68*512);
    create_ozy_buffers(34);
    create_spectrum_buffers(8);
    
    // create a thread to process EP6 input buffers
    rc=pthread_create(&ozy_input_buffer_thread_id,NULL,ozy_input_buffer_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on ozy_input_buffer_thread: rc=%d\n", rc);
    }

    // create a thread to read/write to EP6/EP2
    rc=pthread_create(&ep6_ep2_io_thread_id,NULL,ozy_ep6_ep2_io_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on ozy_ep6_io_thread: rc=%d\n", rc);
    }

    // create a thread to process EP4 input buffers
    rc=pthread_create(&ozy_spectrum_buffer_thread_id,NULL,ozy_spectrum_buffer_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on ozy_spectrum_buffer_thread: rc=%d\n", rc);
    }

    // create a thread to read from EP4
    rc=pthread_create(&ep4_io_thread_id,NULL,ozy_ep4_io_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on ozy_ep4_io_thread: rc=%d\n", rc);
    }


    return rc;
}


int getADCOverflow() {
    int result=lt2208ADCOverflow;
    lt2208ADCOverflow=0;
    return result;
}


