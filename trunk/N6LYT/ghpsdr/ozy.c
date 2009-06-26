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

#include <gtk/gtk.h>

#include "ozy.h"
#include "ozy_buffers.h"
#include "ozy_ringbuffer.h"
#include "spectrum_buffers.h"
#include "dttsp.h"
#include "util.h"
#include "libusbio.h"
#include "filter.h"
#include "volume.h"
#include "mode.h"

/*
 *   ozy interface
 */

#define USB_TIMEOUT -7
//static struct OzyHandle* ozy;

static char ozy_firmware_version[9];
int mercury_software_version=0;
int penelope_software_version=0;
int ozy_software_version=0;

int forward_power=0;

static pthread_t ep6_ep2_io_thread_id;
static pthread_t ep4_io_thread_id;
static pthread_t ozy_input_buffer_thread_id;
static pthread_t ozy_spectrum_buffer_thread_id;

static int frequency=7056000;
static int frequency_changed=1;


static unsigned char control_in[5]={0x00,0x00,0x00,0x00,0x00};

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

int speed=0;           // default 48K
int class=0;           // default other
int lt2208Dither=1;    // default dither on
int lt2208Random=1;    // default random 0n
int alexAttenuation=0; // default alex attenuation 0Db
int micSource=1;       // default mic source Penelope
int clock10MHz=2;      // default 10 MHz clock source Mercury
int clock122_88MHz=1;  // default 122.88 MHz clock source Mercury
int preamp=0;          // default preamp off

int sampleRate=48000;  // default 48k
/* --------------------------------------------------------------------------*/
/** 
* @brief Process the ozy input buffer
* 
* @param buffer
*/
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

        if((control_in[0]&0x04)==0) {

            if(control_in[1]&0x01) {
                lt2208ADCOverflow=1;
            }

            if(mercury_software_version!=control_in[2]) {
                mercury_software_version=control_in[2];
                fprintf(stderr,"  Mercury Software version: %d (0x%0X)\n",mercury_software_version,mercury_software_version);
            }

        } else if((control_in[0]&0x04)==1) {
            forward_power=(control_in[1]<<8)+control_in[2];
        }

        if(penelope_software_version!=control_in[3]) {
            penelope_software_version=control_in[3];
            fprintf(stderr,"  Penelope Software version: %d (0x%0X)\n",penelope_software_version,penelope_software_version);
        }
        if(ozy_software_version!=control_in[4]) {
            ozy_software_version=control_in[4];
            fprintf(stderr,"  Ozy Software version: %d (0x%0X)\n",ozy_software_version,ozy_software_version);
        }

        // extract the 63 samples
        for(i=0;i<63;i++) {

            left_sample   = (int)((signed char) buffer[b++]) << 16;
            left_sample  += (int)((unsigned char)buffer[b++]) << 8;
            left_sample  += (int)((unsigned char)buffer[b++]);
            right_sample  = (int)((signed char) buffer[b++]) << 16;
            right_sample += (int)((unsigned char)buffer[b++]) << 8;
            right_sample += (int)((unsigned char)buffer[b++]);
            mic_sample    = (int)((signed char) buffer[b++]) << 8;
            mic_sample   += (int)((unsigned char)buffer[b++]);

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
//fprintf(stderr,"call dttsp: frame=%d buffer_size=%d\n",frames,buffer_size);
                Audio_Callback (left_input_buffer,right_input_buffer,
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
        dump_ozy_buffer("input buffer",buffer);
    }

}

/* --------------------------------------------------------------------------*/
/** 
* @brief Ozy input buffer thread
* 
* @param arg
* 
* @return 
*/
void* ozy_input_buffer_thread(void* arg) {
    struct ozy_buffer* buffer;

    while(1) {
        // wait for an ozy buffer
        sem_wait(ozy_input_buffer_sem);
        buffer=get_ozy_input_buffer();
        if(buffer==NULL) {
            fprintf(stderr,"ozy_input_buffer_thread: get_ozy_buffer returned NULL!\n");
        } else {
            process_ozy_input_buffer(buffer->buffer);
            free_ozy_buffer(buffer);
        }

    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Ozy spectrum buffer thread
* 
* @param arg
* 
* @return 
*/
void* ozy_spectrum_buffer_thread(void* arg) {
    struct spectrum_buffer* spectrum_buffer;
    while(1) {
        sem_wait(spectrum_input_buffer_sem);
        spectrum_buffer=get_spectrum_input_buffer();
        memcpy(spectrum_samples,spectrum_buffer->buffer,SPECTRUM_BUFFER_SIZE);
        free_spectrum_buffer(spectrum_buffer);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Thread for reading the ep6 and ep2 I2C IO 
* 
* @param arg
* 
* @return 
*/
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
                free_ozy_buffer(ozy_buffer);
            } else if (bytes != OZY_BUFFER_SIZE) {
                fprintf(stderr,"ozy_ep6_ep2_io_thread: OzyBulkRead only read %d bytes\n",bytes);
                free_ozy_buffer(ozy_buffer);
            } else {
                // process input buffer
                put_ozy_input_buffer(ozy_buffer);
                sem_post(ozy_input_buffer_sem);
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
            //dump_ozy_buffer("output buffer",output_buffer);

        }

    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Thread for reading the Ozy ep4 I2C IO
* 
* @param arg
* 
* @return 
*/
void* ozy_ep4_io_thread(void* arg) {
    struct spectrum_buffer* spectrum_buffer;
    int bytes;
    int i;

    while(1) {
        spectrum_buffer=get_spectrum_free_buffer();
        if(spectrum_buffer!=NULL) {
            bytes=libusb_read_ozy(0x84,(void*)(spectrum_buffer->buffer),SPECTRUM_BUFFER_SIZE);
            if (bytes < 0) {
                fprintf(stderr,"ozy_ep4_io_thread: OzyBulkRead failed %d bytes\n",bytes);
                free_spectrum_buffer(spectrum_buffer);
            } else if (bytes != SPECTRUM_BUFFER_SIZE) {
                fprintf(stderr,"ozy_ep4_io_thread: OzyBulkRead only read %d bytes\n",bytes);
                free_spectrum_buffer(spectrum_buffer);
            } else {

                // process input buffer
                put_spectrum_input_buffer(spectrum_buffer);
                sem_post(spectrum_input_buffer_sem);
            }
        } else {
            fprintf(stderr,"ozy_ep4_io_thread: get_spectrum_free_buffer returned NULL!");
        }
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Get the spectrum samples
* 
* @param samples
*/
void getSpectrumSamples(char *samples) {
    memcpy(samples,spectrum_samples,SPECTRUM_BUFFER_SIZE);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the MOX
* 
* @param state
*/
void setMOX(int state) {
    control_out[0]=control_out[0]&0xFE;
    control_out[0]=control_out[0]|(state&0x01);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the speed
* 
* @param speed
*/
void setSpeed(int s) {
    speed=s;
    control_out[1]=control_out[1]&0xFC;
    control_out[1]=control_out[1]|s;
    if(s==SPEED_48KHZ) {
        output_sample_increment=1;
        sampleRate=48000;
        SetSampleRate((double)sampleRate);
        SetRXOsc(0,0,0.0);
        setFilter(filter);
        setMode(mode);
        SetRXOutputGain(0,0,volume/100.0);
    } else if(s==SPEED_96KHZ) {
        output_sample_increment=2;
        sampleRate=96000;
        SetSampleRate((double)sampleRate);
        SetRXOsc(0,0,0.0);
        setFilter(filter);
        setMode(mode);
        SetRXOutputGain(0,0,volume/100.0);
    } else if(s==SPEED_192KHZ) {
        output_sample_increment=4;
        sampleRate=192000;
        SetSampleRate((double)sampleRate);
        SetRXOsc(0,0,0.0);
        setFilter(filter);
        setMode(mode);
        SetRXOutputGain(0,0,volume/100.0);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the 10 mHz source
* 
* @param source
*/
void set10MHzSource(int source) {
    clock10MHz=source;
    control_out[1]=control_out[1]&0xF3;
    control_out[1]=control_out[1]|(source<<2);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the 122 mHz source
* 
* @param source
*/
void set122MHzSource(int source) {
    clock122_88MHz=source;
    control_out[1]=control_out[1]&0xEF;
    control_out[1]=control_out[1]|(source<<4);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the configuration
* 
* @param config
*/
void setConfig(int config) {
    control_out[1]=control_out[1]&0x9F;
    control_out[1]=control_out[1]|(config<<5);
}


/* --------------------------------------------------------------------------*/
/** 
* @brief Set the mic source
* 
* @param source
*/
void setMicSource(int source) {
    micSource=source;
    control_out[1]=control_out[1]&0x7F;
    control_out[1]=control_out[1]|(source<<7);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the class
* 
* @param class
*/
void setClass(int c) {
    class=c;
    control_out[2]=control_out[2]&0xFE;
    control_out[2]=control_out[2]|c;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the OC outputs
* 
* @param outputs
*/
void setOCOutputs(int outputs) {
    control_out[2]=control_out[2]&0x01;
    control_out[2]=control_out[2]|(outputs<<1);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the Alex attenuation
* 
* @param attenuation
*/
void setAlexAttenuation(int attenuation) {
    alexAttenuation=attenuation;
    control_out[3]=control_out[3]&0xFC;
    control_out[3]=control_out[3]|attenuation;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the preamplifer gain
* 
* @param gain
*/
void setPreamp(int p) {
    preamp=p;
    control_out[3]=control_out[3]&0xFB;
    control_out[3]=control_out[3]|(p<<2);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the LT2208 dither
* 
* @param dither
*/
void setLT2208Dither(int dither) {
    lt2208Dither=dither;
    control_out[3]=control_out[3]&0xF7;
    control_out[3]=control_out[3]|(dither<<3);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the LT2208 random
* 
* @param random
*/
void setLT2208Random(int random) {
    lt2208Random=random;
    control_out[3]=control_out[3]&0xEF;
    control_out[3]=control_out[3]|(random<<4);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set Frequency
* 
* @param f
*/
void setFrequency(double f) {
    frequency=(int)((f*1000000.0)+0.5);
    frequency_changed=1;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Initialize Ozy
* 
* @param sample_rate
* 
* @return 
*/
int ozy_init() {
    int rc;

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

    // open ozy
    rc = libusb_open_ozy();
    if (rc != 0) {
        fprintf(stderr,"Cannot locate Ozy\n");
        return (-1);
    }

    rc=libusb_get_ozy_firmware_string(ozy_firmware_version,8);

    if(rc!=0) {
        fprintf(stderr,"Failed to get Ozy Firmware Version - Have you run initozy yet?\n");
        libusb_close_ozy();
        return (-2);
    }

    fprintf(stderr,"Ozy FX2 version: %s\n",ozy_firmware_version);
    
    switch(speed) {
        case 48000:
            output_sample_increment = 1;
            break;
        case 96000:
            output_sample_increment = 2;
            break;
        case 192000:
            output_sample_increment = 4;
            break;
    }


    // create buffers of ozy
    create_ozy_ringbuffer(68*512);
    create_ozy_buffers(68);
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


/* --------------------------------------------------------------------------*/
/** 
* @brief Get the ADC Overflow 
* 
* @return 
*/
int getADCOverflow() {
    int result=lt2208ADCOverflow;
    lt2208ADCOverflow=0;
    return result;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Get Ozy FX2 firmware version
* 
* @return 
*/
char* get_ozy_firmware_version() {
    return ozy_firmware_version;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Get Mercury software version
* 
* @return 
*/
int get_mercury_software_version() {
    return mercury_software_version;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Get Penelope software version
* 
* @return 
*/
int get_penelope_software_version() {
    return penelope_software_version;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Get Ozy software version
* 
* @return 
*/
int get_ozy_software_version() {
    return ozy_software_version;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief save Ozy state
* 
* @return 
*/
void ozySaveState() {
    char string[128];
    sprintf(string,"%d",clock10MHz);
    setProperty("10MHzClock",string);
    sprintf(string,"%d",clock122_88MHz);
    setProperty("122_88MHzClock",string);
    sprintf(string,"%d",micSource);
    setProperty("micSource",string);
    sprintf(string,"%d",class);
    setProperty("class",string);
    sprintf(string,"%d",lt2208Dither);
    setProperty("lt2208Dither",string);
    sprintf(string,"%d",lt2208Random);
    setProperty("lt2208Random",string);
    sprintf(string,"%d",alexAttenuation);
    setProperty("alexAttenuation",string);
    sprintf(string,"%d",preamp);
    setProperty("preamp",string);
    sprintf(string,"%d",speed);
    setProperty("speed",string);
    sprintf(string,"%d",sampleRate);
    setProperty("sampleRate",string);

}

/* --------------------------------------------------------------------------*/
/** 
* @brief resore Ozy state
* 
* @return 
*/
void ozyRestoreState() {
    char *value;

    value=getProperty("10MHzClock");
    if(value) {
        set10MHzSource(atoi(value));
    }
    value=getProperty("122_88MHzClock");
    if(value) {
        set122MHzSource(atoi(value));
    }
    value=getProperty("micSource");
    if(value) {
        setMicSource(atoi(value));
    }
    value=getProperty("class");
    if(value) {
        setMode(atoi(value));
    }
    value=getProperty("lt2208Dither");
    if(value) {
        setLT2208Dither(atoi(value));
    }
    value=getProperty("lt2208Random");
    if(value) {
        setLT2208Random(atoi(value));
    }
    value=getProperty("alexAttenuation");
    if(value) {
        setAlexAttenuation(atoi(value));
    }
    value=getProperty("preamp");
    if(value) {
        setPreamp(atoi(value));
    }
    value=getProperty("speed");
    if(value) {
        setSpeed(atoi(value));
    }
    value=getProperty("sampleRate");
    if(value) {
        sampleRate=atoi(value);
    }
}

