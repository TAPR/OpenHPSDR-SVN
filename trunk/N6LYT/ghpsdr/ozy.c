/* 
 * File:   ozy.c
 * Author: jm57878
 *
 * Created on 10 March 2009, 20:26
 */

#include <math.h>
#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <semaphore.h>
#include <time.h>
#include <sys/timeb.h>

#include <gtk/gtk.h>

#include "ozy.h"
#include "ozy_buffers.h"
#include "ozy_ringbuffer.h"
#include "property.h"
#include "spectrum_buffers.h"
#include "util.h"
#include "libusbio.h"
#include "filter.h"
#include "volume.h"
#include "mode.h"
#include "audiostream.h"
#include "transmit.h"
#include "main.h"
#include "sinewave.h"
#include "vfo.h"
#include "metis.h"
#include "cw.h"
#include "wdsp.h"
#include "subrx.h"
#include "channel.h"

//#define OZY_BUFFERS

/*
 *   ozy interface
 */

#define USB_TIMEOUT -7
//static struct OzyHandle* ozy;

static char ozy_firmware_version[9];
int mercury_software_version=0;
int penelope_software_version=0;
int ozy_software_version=0;

int penelopeForwardPower=0;
int alexForwardPower=0;
int alexReversePower=0;
int AIN3=0;
int AIN4=0;
int AIN6=0;
int IO1=1; // 1 is inactive
int IO2=1;
int IO3=1;

static pthread_t ep6_ep2_io_thread_id;
static pthread_t ep4_io_thread_id;
static pthread_t ozy_input_buffer_thread_id;
static pthread_t ozy_spectrum_buffer_thread_id;
int running;

static long rxFrequency=7056000;
static int rxFrequency_changed=1;
static long txFrequency=7056000;
static int txFrequency_changed=1;

int command=0;
int freqcommand=0;

unsigned char output_buffer[OZY_BUFFER_SIZE];
int output_buffer_index=8;

unsigned char bandscope_buffer[8192];
int bandscope_buffer_index=0;

static int force_write=0;

static unsigned char control_in[5]={0x00,0x00,0x00,0x00,0x00};

unsigned char control_out[5]={0x00,0x00,0x00,0x00,0x00};

int buffer_size=BUFFER_SIZE;
int fft_size=FFT_SIZE;
int output_buffer_size=OUTPUT_BUFFER_SIZE;

float left_input_buffer[BUFFER_SIZE];
float right_input_buffer[BUFFER_SIZE];

float mic_left_buffer[BUFFER_SIZE];
float mic_right_buffer[BUFFER_SIZE];

float left_output_buffer[OUTPUT_BUFFER_SIZE];
float right_output_buffer[OUTPUT_BUFFER_SIZE];

float left_subrx_output_buffer[OUTPUT_BUFFER_SIZE];
float right_subrx_output_buffer[OUTPUT_BUFFER_SIZE];

float left_tx_buffer[OUTPUT_BUFFER_SIZE];
float right_tx_buffer[OUTPUT_BUFFER_SIZE];

float bandscope_left_buffer[BANDSCOPE_BUFFER_SIZE];
float bandscope_right_buffer[BANDSCOPE_BUFFER_SIZE];

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

int sampleRate=48000;
int dspRate=48000;
int outputRate=48000;
int outputSamples=OUTPUT_BUFFER_SIZE;

int mox=0;
int ptt=0;
int dot=0;
int dash=0;

int xmit=0;  // not transmitting

int pennyLane=1;       // default is Pennylane
int driveLevel=255;    // default is max
int driveLevelChanged=0; // force drive level to be sent

int timing=0;
static struct timeb start_time;
static struct timeb end_time;
static int sample_count=0;

static int metis=1;
//static char interface[128];

int alexRxAntenna=NONE;
int alexTxAntenna=ANT1;

float vswr=0.0;

int debug_control1=0;

void ozy_set_usb() {
    metis=0;
}

int ozy_use_metis() {
    return metis;
}

//void ozy_set_interface(char* iface) {
//    strcpy(interface,iface);
//}

void setPennyLane(int state) {
    pennyLane=state;
}

void setDriveLevelChanged(int level) {
    driveLevel=level;
    driveLevelChanged=1;
}

//char* ozy_get_interface() {
//    return interface;
//}

void ozy_send_buffer() {
                        output_buffer[0]=SYNC;
                        output_buffer[1]=SYNC;
                        output_buffer[2]=SYNC;

			switch(command) {
                            case 0:
                                output_buffer[3]=control_out[0];
                                output_buffer[4]=control_out[1];
                                output_buffer[5]=control_out[2];
                                output_buffer[6]=control_out[3];
                                output_buffer[7]=control_out[4];
                                break;
                            case 2:
                                output_buffer[3]=0x12;
                                output_buffer[4]=driveLevel;
                                output_buffer[5]=control_out[2];
                                output_buffer[6]=control_out[3];
                                output_buffer[7]=control_out[4];
                                break;
                            case 4:
                                // need to add orion tip/ring and bias configuration
                                output_buffer[3]=0x14;
                                output_buffer[4]=0x00;
                                output_buffer[5]=0x00;
                                output_buffer[6]=0x00;
                                output_buffer[7]=0x00;
                                break;
                            case 6:
                                // need to add rx attenuation and cw configuration
                                output_buffer[3]=0x16;
                                output_buffer[4]=0x00;
                                output_buffer[5]=0x00;
                                if(cwkeysreversed!=0) {
                                    output_buffer[5]|=0x40;
                                }
                                output_buffer[6]=cwkeyerspeed | (cwkeyermode<<6);
                                output_buffer[7]=cwkeyerweight | (cwkeyerspacing<<7);
                                break;
                            case 8:
                                // need to add tx attenuation and rx ADC selection
                                output_buffer[3]=0x1C;
                                output_buffer[4]=0x00;
                                output_buffer[5]=0x00;
                                output_buffer[6]=0x00;
                                output_buffer[7]=0x00;
                                break;
                            case 10:
                                // need to add cw configuration
                                output_buffer[3]=0x1E;
                                if(cwinternal==1) {
                                    if(xmit || tuning || (mode!=modeCWU && mode!=modeCWL)) {
                                        output_buffer[4]=0x00;
                                    } else {
                                        output_buffer[4]=0x01;
                                    }
                                } else {
                                    output_buffer[4]=0x00;
                                }
                                output_buffer[5]=cwsidetonevolume;
                                output_buffer[6]=cwpttdelay;
                                output_buffer[7]=0x00;
                                break;
                            case 12:
                                // need to add cw configuration
                                output_buffer[3]=0x20;
                                output_buffer[4]=cwhangtime;
                                output_buffer[5]=cwhangtime>>8;
                                output_buffer[6]=cwsidetonefrequency;
                                output_buffer[7]=cwsidetonefrequency>>8;
                                break;
                            default:
                                switch(freqcommand) {
                                    case 0:
                                        // send rx frequency
                                        output_buffer[3]=control_out[0]|0x04;
                                        output_buffer[4]=ddsAFrequency>>24;
                                        output_buffer[5]=ddsAFrequency>>16;
                                        output_buffer[6]=ddsAFrequency>>8;
                                        output_buffer[7]=ddsAFrequency;
                                        freqcommand++;
                                        break;
                                    case 1:
                                        // send tx frequency
                                        output_buffer[3]=control_out[0]|0x02;
                                        if(bSplit) {
                                            output_buffer[3]=control_out[0]|0x02; // Penelope
                                            output_buffer[4]=ddsBFrequency>>24;
                                            output_buffer[5]=ddsBFrequency>>16;
                                            output_buffer[6]=ddsBFrequency>>8;
                                            output_buffer[7]=ddsBFrequency;
                                        } else {
                                            output_buffer[4]=ddsAFrequency>>24;
                                            output_buffer[5]=ddsAFrequency>>16;
                                            output_buffer[6]=ddsAFrequency>>8;
                                            output_buffer[7]=ddsAFrequency;
                                        }
                                        freqcommand=0;
                                        break;
                                }
                                break;
                        }
                        command++;
                        if(command>=14) {
                            command=0;
                        }
                        // set mox
                        output_buffer[3]|=(xmit&0x01);

/*
                        // set mox
                        control_out[0]=control_out[0]&0xFE;
                        control_out[0]=control_out[0]|(xmit&0x01);

if(control_out[1]!=debug_control1) {
    fprintf(stderr,"control_out[1]=%02X\n",control_out[1]);
    debug_control1=control_out[1];
}

                        if(splitChanged) {
                            output_buffer[3]=control_out[0];
                            output_buffer[4]=control_out[1];
                            output_buffer[5]=control_out[2];
                            output_buffer[6]=control_out[3];
                            if(bSplit) {
                                output_buffer[7]=control_out[4]|0x04;
                            } else {
                                output_buffer[7]=control_out[4];
                            }
                            splitChanged=0;
                        } else if(frequencyAChanged) {
                            if(bSplit) {
                                output_buffer[3]=control_out[0]|0x04; // Mercury (1)
                            } else {
                                output_buffer[3]=control_out[0]|0x02; // Mercury and Penelope
                            }
                            output_buffer[4]=ddsAFrequency>>24;
                            output_buffer[5]=ddsAFrequency>>16;
                            output_buffer[6]=ddsAFrequency>>8;
                            output_buffer[7]=ddsAFrequency;
                            frequencyAChanged=0;
                        } else if(frequencyBChanged) {
                            if(bSplit) {
                                output_buffer[3]=control_out[0]|0x02; // Penelope
                                output_buffer[4]=ddsBFrequency>>24;
                                output_buffer[5]=ddsBFrequency>>16;
                                output_buffer[6]=ddsBFrequency>>8;
                                output_buffer[7]=ddsBFrequency;
                            }
                            frequencyBChanged=0;
                        } else if(driveLevelChanged && pennyLane) { 
                            output_buffer[3]=0x12|(xmit&0x01);
                            output_buffer[4]=driveLevel;
                            output_buffer[5]=0;
                            output_buffer[6]=0;
                            output_buffer[7]=0;
                            driveLevelChanged=0;
                        } else {
                            output_buffer[3]=control_out[0];
                            output_buffer[4]=control_out[1];
                            output_buffer[5]=control_out[2];
                            output_buffer[6]=control_out[3];
                            if(bSplit) {
                                output_buffer[7]=control_out[4]|0x04;
                            } else {
                                output_buffer[7]=control_out[4];
                            }
                        }

                        */

                        if(metis) {
                            metis_write(0x02,output_buffer,OZY_BUFFER_SIZE);
                        } else {
                            int bytes;
                            bytes=libusb_write_ozy(0x02,(void*)(output_buffer),OZY_BUFFER_SIZE);
                            if(bytes!=OZY_BUFFER_SIZE) {
                                perror("OzyBulkWrite failed");
                            }
                        }
}

void process_bandscope_buffer(char* buffer) {
    short sample;
    int i;
    for (i = 0; i < 512; i += 2) {
        sample = (short) ((buffer[i + 1] << 8) + (buffer[i] & 0xFF));
        bandscope_left_buffer[bandscope_buffer_index++]=(float)sample/32767.0F;
        bandscope_right_buffer[bandscope_buffer_index++]=0.0F;
        if (bandscope_buffer_index == BANDSCOPE_BUFFER_SIZE) {
            Spectrum(CHANNEL_BS, 0, 0, bandscope_left_buffer, bandscope_right_buffer);
            bandscope_buffer_index = 0;
        }
    }
}


/* --------------------------------------------------------------------------*/
/** 
* @brief Process the ozy input buffer
* 
* @param buffer
*/
void process_ozy_input_buffer(char* buffer) {
    int i,j;
    int b=0;
    unsigned char ozy_samples[8*8];
    int bytes;

    int last_ptt;
    int last_dot;
    int last_dash;
    int last_xmit;

    double gain;


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

        last_xmit=xmit;
        xmit=mox|ptt|dot|dash;

        int *vfoState=malloc(sizeof(int));
        *vfoState=xmit;
        g_idle_add(vfoTransmit,(gpointer)vfoState);

//if(xmit!=last_xmit) {
//    fprintf(stderr,"ozy: xmit=%d mox=%d ptt=%d dot=%d dash=%d\n", xmit,mox,ptt,dot,dash);
//    fprintf(stderr,"ozy: testing=%d tuning=%d\n", testing,tuning);
//}
        switch((control_in[0]>>3)&0x1F) {

        case 0:
            lt2208ADCOverflow=control_in[1]&0x01;
            IO1=(control_in[1]&0x02)?0:1;
            IO2=(control_in[1]&0x04)?0:1;
            IO3=(control_in[1]&0x08)?0:1;
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
            break;
        case 1:
            penelopeForwardPower=(control_in[1]<<8)+control_in[2]; // from Penelope or Hermes
            
            alexForwardPower=(control_in[3]<<8)+control_in[4]; // from Alex or Apollo
            break;
        case 2:
            alexReversePower=(control_in[1]<<8)+control_in[2]; // from Alex or Apollo
            AIN3=(control_in[3]<<8)+control_in[4]; // from Pennelope or Hermes
            break;
        case 3:
            AIN4=(control_in[1]<<8)+control_in[2]; // from Pennelope or Hermes
            AIN6=(control_in[3]<<8)+control_in[4]; // from Pennelope or Hermes
            break;
        }

if(xmit) {
    float fwd=(float)alexForwardPower/100.0F;
    float rev=(float)alexReversePower/100.0F;

    float gamma=sqrt(rev/fwd);
    vswr=(1.0F+gamma)/(1.0F-gamma);
    //fprintf(stderr,"fwd=%f rev=%f vswr=%f\n",fwd,rev,vswr);
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
            mic_sample_float=((float)mic_sample/32767.0f)*micGain; // 16 bit sample

            // add to buffer
            if(xmit) { // mute the input
                left_input_buffer[samples]=0.0;
                right_input_buffer[samples]=0.0;
            } else {
                left_input_buffer[samples]=left_sample_float;
                right_input_buffer[samples]=right_sample_float;
            }

            if(testing) {
                mic_left_buffer[samples]=0.0f;
                mic_right_buffer[samples]=0.0f;
            } else {
                mic_left_buffer[samples]=mic_sample_float;
                mic_right_buffer[samples]=0.0f;
            }
            samples++;

            if(timing) {
                sample_count++;
                if(sample_count==sampleRate) {
                    ftime(&end_time);
                    fprintf(stderr,"%d samples in %ld ms\n",sample_count,((end_time.time*1000)+end_time.millitm)-((start_time.time*1000)+start_time.millitm));
                    sample_count=0;
                    ftime(&start_time);
                }
            }

            // when we have enough samples give them to DttSP and get the results
            if(samples==buffer_size) {
                // process the input
                int error;
		fexchange2(CHANNEL_RX0, left_input_buffer, right_input_buffer, left_output_buffer, right_output_buffer, &error);
                if(error!=0) {
                    fprintf(stderr,"fexchange2 (CHANNEL_RX0) returned error: %d\n", error);
                }
                if(subrx) {
		    fexchange2(CHANNEL_SUBRX, left_input_buffer, right_input_buffer, left_subrx_output_buffer, right_subrx_output_buffer, &error);
                    if(error!=0) {
                        fprintf(stderr,"fexchange2 (CHANNEL_SUBRX) returned error: %d\n", error);
                    }
                }
                Spectrum(CHANNEL_RX0, 0, 0, right_input_buffer, left_input_buffer);

                // transmit
                if(xmit) {
                    if(tuning) {
                        sineWave(mic_left_buffer,buffer_size,tuningPhase,(double)cwPitch);
                        tuningPhase=sineWave(mic_right_buffer,buffer_size,tuningPhase,(double)cwPitch);
                    } else if(testing) {
                        // leave alone
                    } else if(mode==modeCWU || mode==modeCWL) {
                    }

                    // process the output
	            fexchange2(CHANNEL_TX, mic_left_buffer, mic_right_buffer, left_tx_buffer, right_tx_buffer, &error);
                    if(error!=0) {
                        fprintf(stderr,"fexchange2 (CHANNEL_TX) returned error: %d\n", error);
                    }
                    Spectrum(CHANNEL_TX, 0, 0, right_tx_buffer, left_tx_buffer);
                }

                if(pennyLane) {
                    gain=1.0;
                } else {
                    //gain=rfGain/255.0;
                    gain=rfGain;
                }

                for(j=0;j<output_buffer_size;j++) {
                    left_rx_sample=(short)(left_output_buffer[j]*32767.0*(volume/100.0));
                    right_rx_sample=(short)(right_output_buffer[j]*32767.0*(volume/100.0));
                    if(subrx) {
                        left_rx_sample+=(short)(left_subrx_output_buffer[j]*32767.0*(subrxGain/100.0));
                        right_rx_sample+=(short)(right_subrx_output_buffer[j]*32767.0*(subrxGain/100.0));
                        //left_rx_sample/=2;
                        //right_rx_sample/=2;
                    }

                    if(xmit) {
                        left_tx_sample=(short)(left_tx_buffer[j]*32767.0*gain);
                        right_tx_sample=(short)(right_tx_buffer[j]*32767.0*gain);
                    } else {
                        left_tx_sample=0;
                        right_tx_sample=0;
                    }

                    output_buffer[output_buffer_index++]=left_rx_sample>>8;
                    output_buffer[output_buffer_index++]=left_rx_sample;
                    output_buffer[output_buffer_index++]=right_rx_sample>>8;
                    output_buffer[output_buffer_index++]=right_rx_sample;
                    output_buffer[output_buffer_index++]=left_tx_sample>>8;
                    output_buffer[output_buffer_index++]=left_tx_sample;
                    output_buffer[output_buffer_index++]=right_tx_sample>>8;
                    output_buffer[output_buffer_index++]=right_tx_sample;

                    if(output_buffer_index>=OZY_BUFFER_SIZE) {
                        ozy_send_buffer();
                        output_buffer_index=8;
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
        exit(1);
    }

}

void ozy_prime() {
    int i;
    int bytes;

    for(i=8;i<OZY_BUFFER_SIZE;i++) {
        output_buffer[i]=0;
    }

    do {
        ozy_send_buffer();
    } while (command!=0);
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
    unsigned char input_buffer[OZY_BUFFER_SIZE];
    int bytes;

    while(running) {
        // read an input buffer (blocks until all bytes read)
#ifdef OZY_BUFFERS
        ozy_buffer=get_ozy_free_buffer();
        if(ozy_buffer!=NULL) {
            bytes=libusb_read_ozy(0x86,(void*)(ozy_buffer->buffer),OZY_BUFFER_SIZE);
#else
            bytes=libusb_read_ozy(0x86,(void*)(input_buffer),OZY_BUFFER_SIZE);
#endif
            if (bytes < 0) {
                fprintf(stderr,"ozy_ep6_ep2_io_thread: OzyBulkRead read failed %d\n",bytes);
#ifdef OZY_BUFFERS
                free_ozy_buffer(ozy_buffer);
#endif
            } else if (bytes != OZY_BUFFER_SIZE) {
                fprintf(stderr,"ozy_ep6_ep2_io_thread: OzyBulkRead only read %d bytes\n",bytes);
#ifdef OZY_BUFFERS
                free_ozy_buffer(ozy_buffer);
#endif
            } else {
                // process input buffer
#ifdef OZY_BUFFERS
                put_ozy_input_buffer(ozy_buffer);
                sem_post(ozy_input_buffer_sem);
#else
                process_ozy_input_buffer(input_buffer);
#endif
            }

#ifdef OZY_BUFFERS
        }
#endif

#ifdef OZY_BUFFERS
        // see if we have enough to send a buffer or force the write if we have a timeout
        if((bytes==USB_TIMEOUT) || ozy_ringbuffer_entries(ozy_output_buffer)>=(OZY_BUFFER_SIZE-8) || force_write || frequencyAChanged || frequencyBChanged) {
#else
        if(bytes==USB_TIMEOUT) {
#endif
            
            force_write=0;

            output_buffer[0]=SYNC;
            output_buffer[1]=SYNC;
            output_buffer[2]=SYNC;

            // set if transmitting
            control_out[0]=control_out[0]&0xFE;
            control_out[0]=control_out[0]|(xmit&0x01);

            if(splitChanged) {
                output_buffer[3]=control_out[0];
                output_buffer[4]=control_out[1];
                output_buffer[5]=control_out[2];
                output_buffer[6]=control_out[3];
                if(bSplit) {
                    output_buffer[7]=control_out[4]|0x04;
                } else {
                    output_buffer[7]=control_out[4];
                }
                splitChanged=0;
            } else if(frequencyAChanged) {
                if(bSplit) {
                    output_buffer[3]=control_out[0]|0x04; // Mercury (1)
                } else {
                    output_buffer[3]=control_out[0]|0x02; // Mercury and Penelope
                }
                output_buffer[4]=ddsAFrequency>>24;
                output_buffer[5]=ddsAFrequency>>16;
                output_buffer[6]=ddsAFrequency>>8;
                output_buffer[7]=ddsAFrequency;
                frequencyAChanged=0;
            } else if(frequencyBChanged) {
                if(bSplit) {
                    output_buffer[3]=control_out[0]|0x02; // Penelope
                    output_buffer[4]=ddsBFrequency>>24;
                    output_buffer[5]=ddsBFrequency>>16;
                    output_buffer[6]=ddsBFrequency>>8;
                    output_buffer[7]=ddsBFrequency;
                }
                frequencyBChanged=0;
            } else {
                output_buffer[3]=control_out[0];
                output_buffer[4]=control_out[1];
                output_buffer[5]=control_out[2];
                output_buffer[6]=control_out[3];
                if(bSplit) {
                    output_buffer[7]=control_out[4]|0x04;
                } else {
                    output_buffer[7]=control_out[4];
                }
            }

#ifdef OZY_BUFFERS
            if(ozy_ringbuffer_entries(ozy_output_buffer)>=(OZY_BUFFER_SIZE-8)) {
                bytes=ozy_ringbuffer_get(ozy_output_buffer,&output_buffer[8],OZY_BUFFER_SIZE-8);
                if(bytes!=OZY_BUFFER_SIZE-8) {
                    fprintf(stderr,"OOPS - thought there was enough for usb output but only %d\n",bytes);
                }
            }
#endif

            bytes=libusb_write_ozy(0x02,(void*)(output_buffer),OZY_BUFFER_SIZE);
            if(bytes!=OZY_BUFFER_SIZE) {
                perror("OzyBulkWrite failed");
            }
            //dump_ozy_buffer("output buffer",output_buffer);

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
    unsigned char input_buffer[SPECTRUM_BUFFER_SIZE];

    while(running) {
#ifdef OZY_BUFFERS
        spectrum_buffer=get_spectrum_free_buffer();
        if(spectrum_buffer!=NULL) {
            bytes=libusb_read_ozy(0x84,(void*)(spectrum_buffer->buffer),SPECTRUM_BUFFER_SIZE);
#else
            bytes=libusb_read_ozy(0x84,(void*)(input_buffer),SPECTRUM_BUFFER_SIZE);
#endif
            if (bytes < 0) {
                fprintf(stderr,"ozy_ep4_io_thread: OzyBulkRead failed %d bytes\n",bytes);
#ifdef OZY_BUFFERS
                free_spectrum_buffer(spectrum_buffer);
#endif
            } else if (bytes != SPECTRUM_BUFFER_SIZE) {
                fprintf(stderr,"ozy_ep4_io_thread: OzyBulkRead only read %d bytes\n",bytes);
#ifdef OZY_BUFFERS
                free_spectrum_buffer(spectrum_buffer);
#endif
            } else {
                // process input buffer
#ifdef OZY_BUFFERS
                put_spectrum_input_buffer(spectrum_buffer);
                sem_post(spectrum_input_buffer_sem);
#else
                for(i=0;i<SPECTRUM_BUFFER_SIZE;i+=OZY_BUFFER_SIZE) {
                    process_bandscope_buffer(&input_buffer[i]);
                }
#endif
            }
#ifdef OZY_BUFFERS
        } else {
            fprintf(stderr,"ozy_ep4_io_thread: get_spectrum_free_buffer returned NULL!");
        }
#endif
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
        //output_sample_increment=1;
        sampleRate=48000;
        output_buffer_size=OUTPUT_BUFFER_SIZE;
    } else if(s==SPEED_96KHZ) {
        //output_sample_increment=2;
        sampleRate=96000;
        output_buffer_size=OUTPUT_BUFFER_SIZE/2;
    } else if(s==SPEED_192KHZ) {
        //output_sample_increment=4;
        sampleRate=192000;
        output_buffer_size=OUTPUT_BUFFER_SIZE/4;
    } else if(s==SPEED_384KHZ) {
        //output_sample_increment=4;
        sampleRate=384000;
        output_buffer_size=OUTPUT_BUFFER_SIZE/8;
    }

fprintf(stderr,"setSpeed s=%d sampleRate=%d\n",s,sampleRate);

    SetAllRates(CHANNEL_RX0,sampleRate,dspRate,outputRate);
    SetAllRates(CHANNEL_SUBRX,sampleRate,dspRate,outputRate);
    SetAllRates(CHANNEL_TX,sampleRate,dspRate,outputRate);
    SetEERSamplerate(0, 48000);
    setFilter(filter);
    setMode(mode);
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
    control_out[1]=control_out[1]|(clock10MHz<<2);
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
* @brief Initialize Ozy
* 
* @param sample_rate
* 
* @return 
*/
int ozy_init() {
    int rc;

    fprintf(stderr,"ozy_init\n");

    // setup defaults
    control_out[0] = MOX_DISABLED;
    control_out[1] = CONFIG_BOTH
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

    //ozyRestoreState();
    setSpeed(speed);


    tuningPhase=0.0;

    if(!metis) {
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
    }
    
    force_write=0;

#ifdef OZY_BUFFERS
    // create buffers of ozy
    create_ozy_ringbuffer(128*512);
    create_ozy_buffers(128);
    create_spectrum_buffers(8);
#endif
    

    running=1;

    if(metis) { 
        metis_start_receive_thread();
    } else {
#ifdef OZY_BUFFERS
        // create a thread to process EP6 input buffers
        rc=pthread_create(&ozy_input_buffer_thread_id,NULL,ozy_input_buffer_thread,NULL);
        if(rc != 0) {
            fprintf(stderr,"pthread_create failed on ozy_input_buffer_thread: rc=%d\n", rc);
        }

        // create a thread to process EP4 input buffers
        rc=pthread_create(&ozy_spectrum_buffer_thread_id,NULL,ozy_spectrum_buffer_thread,NULL);
        if(rc != 0) {
            fprintf(stderr,"pthread_create failed on ozy_spectrum_buffer_thread: rc=%d\n", rc);
        }
#endif

        // create a thread to read/write to EP6/EP2
        rc=pthread_create(&ep6_ep2_io_thread_id,NULL,ozy_ep6_ep2_io_thread,NULL);
        if(rc != 0) {
            fprintf(stderr,"pthread_create failed on ozy_ep6_io_thread: rc=%d\n", rc);
        }

        // create a thread to read from EP4
        rc=pthread_create(&ep4_io_thread_id,NULL,ozy_ep4_io_thread,NULL);
        if(rc != 0) {
            fprintf(stderr,"pthread_create failed on ozy_ep4_io_thread: rc=%d\n", rc);
        }
    }


    return rc;
}

void ozy_stop() {
    if(metis) {
        metis_stop();
    } else {
        running=0;
    }
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
    sprintf(string,"%d",pennyLane);
    setProperty("pennyLane",string);

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
        //setMode(atoi(value));
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
    value=getProperty("pennyLane");
    if(value) {
        pennyLane=atoi(value);
    }
}

void setAlexRxAntenna(int a) {
    alexRxAntenna=a;
    //if(!xmit) {
    //    control_out[4]=control_out[4]&0xFC;
    //    control_out[4]=control_out[4]|a;
    //}
}

void setAlexTxAntenna(int a) {
    alexTxAntenna=a;
    //if(xmit) {
    //    control_out[4]=control_out[4]&0xFC;
    //    control_out[4]=control_out[4]|a;
    //}
}
