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
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>


#include "ozy.h"
#include "ozy_buffers.h"
#include "ozy_ringbuffer.h"
#include "spectrum_buffers.h"
#include "util.h"
#include "libusbio.h"
#include "property.h"
#include "receiver.h"

/*
 *   ozy interface
 */

#define USB_TIMEOUT -7
//static struct OzyHandle* ozy;

static char ozy_firmware_version[9];
int mercury_software_version=0;
int penelope_software_version=0;
int ozy_software_version=0;

int forwardPower=0;

static pthread_t ep6_ep2_io_thread_id;
static pthread_t ep4_io_thread_id;
static pthread_t ozy_input_buffer_thread_id;
static pthread_t ozy_spectrum_buffer_thread_id;


//RECEIVER receiver[MAX_RECEIVERS];
int receivers=1;
static int current_receiver=0;

static long txFrequency=7056000;
static int txFrequency_changed=1;


static unsigned char control_in[5]={0x00,0x00,0x00,0x00,0x00};

unsigned char control_out[5]={0x00,0x00,0x00,0x00,0x00};

int output_sample_increment=1; // 1=48000 2=96000 4=192000

int buffer_size=BUFFER_SIZE;

//float left_input_buffer[BUFFER_SIZE];
//float right_input_buffer[BUFFER_SIZE];

float mic_left_buffer[BUFFER_SIZE];
float mic_right_buffer[BUFFER_SIZE];

float left_output_buffer[BUFFER_SIZE];
float right_output_buffer[BUFFER_SIZE];

float left_tx_buffer[BUFFER_SIZE];
float right_tx_buffer[BUFFER_SIZE];

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

int mox=0;             // default not transmitting

int ptt=0;
int dot=0;
int dash=0;

int configure=2;

static float micGain=0.26F;
static float rfGain=0.1F;

static long sequence=0;

static struct sockaddr_in client;
static int client_length;

static int spectrum_socket;
static struct sockaddr_in spectrum_address;
static int spectrum_address_length;

static int rx_frame=0;
static int tx_frame=0;

/* --------------------------------------------------------------------------*/
/** 
* @brief Process the ozy input buffer
* 
* @param buffer
*/
void process_ozy_input_buffer(char* buffer) {
    int i,j;
    int b=0;
    int b_max=512;
    int r;
    int c=0;
    unsigned char ozy_samples[8*8];
    int rc;

    int last_ptt;
    int last_dot;
    int last_dash;


    if(buffer[b++]==SYNC && buffer[b++]==SYNC && buffer[b++]==SYNC) {

        // extract control bytes
        control_in[0]=buffer[b++];
        control_in[1]=buffer[b++];
        control_in[2]=buffer[b++];
        control_in[3]=buffer[b++];
        control_in[4]=buffer[b++];

        last_ptt=ptt;
        last_dot=dot;
        last_dash=dash;
        ptt=(control_in[0]&0x01)==0x01;
        dash=(control_in[0]&0x02)==0x02;
        dot=(control_in[0]&0x04)==0x04;

        if(ptt!=last_ptt || dot!=last_dot || dash!=last_dash) {
            //int *vfoState=malloc(sizeof(int));
            //*vfoState=ptt|dot|dash;
            //g_idle_add(vfoTransmit,(gpointer)vfoState);
        }

        if((control_in[0]&0x08)==0) {
            if(control_in[1]&0x01) {
                lt2208ADCOverflow=1;
            }
            if(mercury_software_version!=control_in[2]) {
                mercury_software_version=control_in[2];
                fprintf(stderr,"  Mercury Software version: %d (0x%0X)\n",mercury_software_version,mercury_software_version);
            }
            if(penelope_software_version!=control_in[3]) {
                penelope_software_version=control_in[3];
                fprintf(stderr,"  Penelope Software version: %d (0x%0X)\n",penelope_software_version,penelope_software_version);
            }
            if(ozy_software_version!=control_in[4]) {
                ozy_software_version=control_in[4];
                fprintf(stderr,"  Ozy Software version: %d (0x%0X)\n",ozy_software_version,ozy_software_version);
            }
        } else if(control_in[0]&0x08) {
            forwardPower=(control_in[1]<<8)+control_in[2];
        }

        switch(receivers) {
            case 1: b_max=512-0; break;
            case 2: b_max=512-0; break;
            case 3: b_max=512-4; break;
            case 4: b_max=512-10; break;
            case 5: b_max=512-24; break;
            case 6: b_max=512-10; break;
            case 7: b_max=512-20; break;
            case 8: b_max=512-4; break;
        }

        // extract the samples
        while(b<b_max) {

            // extract each of the receivers
            for(r=0;r<receivers;r++) {
                left_sample   = (int)((signed char) buffer[b++]) << 16;
                left_sample  += (int)((unsigned char)buffer[b++]) << 8;
                left_sample  += (int)((unsigned char)buffer[b++]);
                right_sample  = (int)((signed char) buffer[b++]) << 16;
                right_sample += (int)((unsigned char)buffer[b++]) << 8;
                right_sample += (int)((unsigned char)buffer[b++]);
                left_sample_float=(float)left_sample/8388607.0; // 24 bit sample
                right_sample_float=(float)right_sample/8388607.0; // 24 bit sample
                receiver[r].input_buffer[samples]=left_sample_float;
                receiver[r].input_buffer[samples+BUFFER_SIZE]=right_sample_float;
            }
            mic_sample    = (int)((signed char) buffer[b++]) << 8;
            mic_sample   += (int)((unsigned char)buffer[b++]);
            mic_sample_float=(float)mic_sample/32767.0*micGain; // 16 bit sample

            // add to buffer
            mic_left_buffer[samples]=mic_sample_float;
            mic_right_buffer[samples]=0.0f;
            samples++;

            // when we have enough samples send them to the clients
            if(samples==buffer_size) {

                // send I/Q data to clients

                for(r=0;r<receivers;r++) {
                    if(receiver[r].address!=-1) {
                        client_length=sizeof(client);
                        memset((char *) &client, 0, client_length);
                        client.sin_family = AF_INET;
                        client.sin_addr.s_addr = receiver[r].address;
                        client.sin_port = htons(receiver[r].spectrum_port);

//fprintf(stderr,"buffer sendto rx=%d port=%d\n",r,ntohs(client.sin_port));
                        rc=sendto(spectrum_socket,receiver[r].input_buffer,BUFFER_SIZE*2*4,0,(struct sockaddr *)&client,client_length);

                        if(rc!=BUFFER_SIZE*4*2) {
                            fprintf(stderr,"sendto failed: %d\n",rc);
                        }
                    }

                }
                sequence++;
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
        dump_ozy_header("input buffer",rx_frame,buffer);
    }

}

void process_ozy_output_buffer(float *left_output_buffer,float *right_output_buffer) {
    unsigned char ozy_samples[8*8];
    int j,c;

//fprintf(stderr,"process_ozy_output_buffer\n");
                // process any output
                for(j=0,c=0;j<buffer_size;j+=output_sample_increment) {
                    left_rx_sample=(short)(left_output_buffer[j]*32767.0);
                    right_rx_sample=(short)(right_output_buffer[j]*32767.0);

                    if(mox || ptt || dot || dash ) {
                        left_tx_sample=(short)(left_tx_buffer[j]*32767.0*rfGain);
                        right_tx_sample=(short)(right_tx_buffer[j]*32767.0*rfGain);
                    } else {
                        left_tx_sample=0;
                        right_tx_sample=0;
                    }

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
    float mixed_output_buffer[BUFFER_SIZE*2];
    unsigned char output_buffer[OZY_BUFFER_SIZE];
    int bytes;
    int i,j;
    int valid_output;

    ozy_buffer=get_ozy_free_buffer();

    while(1) {

        // read an input buffer (blocks until all bytes read)
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
                rx_frame++;
//dump_ozy_buffer("received from Ozy:",rx_frame,ozy_buffer->buffer);
                process_ozy_input_buffer(ozy_buffer->buffer);
            }
        }

        if(ozy_ringbuffer_entries(ozy_output_buffer)<(OZY_BUFFER_SIZE-8)) {
            for(i=0;i<BUFFER_SIZE*2;i++) {
                mixed_output_buffer[i]=0.0F;
            }
            // see if there are any valid output buffers
            valid_output=0;
            for(i=0;i<receivers;i++) {
                if(receiver[i].output_buffer_valid) {
                    valid_output++;
                    receiver[i].output_buffer_valid=0;
                    for(j=0;j<BUFFER_SIZE*2;j++) {
                        mixed_output_buffer[j]+=receiver[i].output_buffer[j];
                    }
                }
            }

            if(valid_output>0) {
                process_ozy_output_buffer(mixed_output_buffer,&mixed_output_buffer[BUFFER_SIZE]);
            }
        }

        // see if we have enough to send a buffer
        // force a write if we are just starting up
        if(configure>0 || ozy_ringbuffer_entries(ozy_output_buffer)>=(OZY_BUFFER_SIZE-8)) {
            
            output_buffer[0]=SYNC;
            output_buffer[1]=SYNC;
            output_buffer[2]=SYNC;

            if(configure>0) {
//fprintf(stderr,"rx %d configure %d\n",current_receiver,configure);
                configure--;
                output_buffer[3]=control_out[0];
                output_buffer[4]=control_out[1];
                output_buffer[5]=control_out[2];
                output_buffer[6]=control_out[3];
                output_buffer[7]=control_out[4];
            } else if(receiver[current_receiver].frequency_changed) {
//fprintf(stderr,"rx %d frequency changed to %ld\n",current_receiver,receiver[current_receiver].frequency);
                output_buffer[3]=control_out[0]|((current_receiver+2)<<1);
                output_buffer[4]=receiver[current_receiver].frequency>>24;
                output_buffer[5]=receiver[current_receiver].frequency>>16;
                output_buffer[6]=receiver[current_receiver].frequency>>8;
                output_buffer[7]=receiver[current_receiver].frequency;
                receiver[current_receiver].frequency_changed=0;
            } else {
//fprintf(stderr,"rx %d entries=%d\n",current_receiver,ozy_ringbuffer_entries(ozy_output_buffer));
                output_buffer[3]=control_out[0];
                output_buffer[4]=control_out[1];
                output_buffer[5]=control_out[2];
                output_buffer[6]=control_out[3];
                output_buffer[7]=control_out[4];
            }

            current_receiver++;
            if(current_receiver==receivers) {
                current_receiver=0;
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

//dump_ozy_buffer("sent to Ozy:",tx_frame,output_buffer);
            tx_frame++;


            //if(mox) {
            //    dump_ozy_buffer("output buffer",output_buffer);
            //}
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
    mox=state;
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
//        SetSampleRate((double)sampleRate);
//        SetRXOsc(0,0,0.0);
//        setFilter(filter);
//        setMode(mode);
//        SetRXOutputGain(0,0,volume/100.0);
    } else if(s==SPEED_96KHZ) {
        output_sample_increment=2;
        sampleRate=96000;
//        SetSampleRate((double)sampleRate);
//        SetRXOsc(0,0,0.0);
//        setFilter(filter);
//        setMode(mode);
//        SetRXOutputGain(0,0,volume/100.0);
    } else if(s==SPEED_192KHZ) {
        output_sample_increment=4;
        sampleRate=192000;
//        SetSampleRate((double)sampleRate);
//        SetRXOsc(0,0,0.0);
//        setFilter(filter);
//        setMode(mode);
//        SetRXOutputGain(0,0,volume/100.0);
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
* @brief Set the number of receivers
* 
* @param receivers
*/
void setReceivers(int r) {
    receivers=r;
    control_out[4]=control_out[4]&0xC7;
    control_out[4]=control_out[4]|((r-1)<<3);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the receiver frequency
* 
* @param receiver
* @param frequency
*/
void setReceiverFrequency(int r,long f) {
    receiver[r].frequency=f;
    receiver[r].frequency_changed=1;
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
    int i;

    spectrum_socket=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(spectrum_socket<0) {
        perror("create socket failed for spectrum socket");
        exit(1);
    }

    spectrum_address_length=sizeof(spectrum_address);
    memset(&spectrum_address,0,spectrum_address_length);
    spectrum_address.sin_family=AF_INET;
    spectrum_address.sin_addr.s_addr=htonl(INADDR_ANY);
    spectrum_address.sin_port=htons(0);

    if(bind(spectrum_socket,(struct sockaddr*)&spectrum_address,spectrum_address_length)<0) {
        perror("bind socket failed for spectrum socket");
        exit(1);
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
    control_out[4] = DUPLEX | ((receivers-1)<<3);

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
        case SPEED_48KHZ:
            sampleRate=48000;
            output_sample_increment = 1;
            break;
        case SPEED_96KHZ:
            sampleRate=96000;
            output_sample_increment = 2;
            break;
        case SPEED_192KHZ:
            sampleRate=192000;
            output_sample_increment = 4;
            break;
    }

    for(i=0;i<receivers;i++) {
        receiver[i].frequency=7056000L;
        receiver[i].frequency_changed=1;
    }


    // create buffers of ozy
    create_ozy_ringbuffer(68*512);
    create_ozy_buffers(68);
    create_spectrum_buffers(8);

    return rc;
}

int ozy_start() {
    int rc;

    fprintf(stderr,"Ozy configured at %d for %d receivers\n",sampleRate,receivers);
    
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
    sprintf(string,"%d",receivers);
    setProperty("receivers",string);
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
        setClass(atoi(value));
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
    value=getProperty("receivers");
    if(value) {
        receivers=atoi(value);
    }
}

