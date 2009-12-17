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
#include <netdb.h>
#include <netinet/in.h>
#include <arpa/inet.h>

#include <gtk/gtk.h>

#include "ozy.h"
#include "property.h"
#include "spectrum_buffers.h"
#include "dttsp.h"
#include "filter.h"
#include "volume.h"
#include "mode.h"
#include "audiostream.h"
#include "transmit.h"
#include "main.h"
#include "sinewave.h"
#include "vfo.h"
#include "local_audio.h"

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

static pthread_t spectrum_thread_id;
static pthread_t keepalive_thread_id;

static long rxFrequency=7056000;
static int rxFrequency_changed=1;
static long txFrequency=7056000;
static int txFrequency_changed=1;


static unsigned char control_in[5]={0x00,0x00,0x00,0x00,0x00};

unsigned char control_out[5]={0x00,0x00,0x00,0x00,0x00};

int output_sample_increment=1; // 1=48000 2=96000 4=192000

#define BUFFER_SIZE 1024
int buffer_size=BUFFER_SIZE;

float input_buffer[BUFFER_SIZE*2];
float output_buffer[BUFFER_SIZE*2];

//float left_input_buffer[BUFFER_SIZE];
//float right_input_buffer[BUFFER_SIZE];

float mic_left_buffer[BUFFER_SIZE];
float mic_right_buffer[BUFFER_SIZE];

//float left_output_buffer[BUFFER_SIZE];
//float right_output_buffer[BUFFER_SIZE];

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

#define COMMAND_PORT 12000
#define SPECTRUM_PORT 13000
#define AUDIO_PORT 15000

int command_socket;
int command_port;
static struct sockaddr_in command_addr;
static socklen_t command_length=sizeof(command_addr);

int audio_socket;
int audio_port;
static struct sockaddr_in audio_addr;
static socklen_t audio_length=sizeof(audio_addr);

static struct sockaddr_in server_addr;
static socklen_t server_length=sizeof(server_addr);

#define BUFFER_SIZE 1024

char server_address[64];
short server_port;
char command[64];
char response[64];

int session;


static int local_audio=0;

void* spectrum_thread(void* arg) {
    int bytes_read;
    int bytes_written;
    int spectrum_socket;
    struct sockaddr_in spectrum_addr;
    int spectrum_length;
    struct sockaddr_in client_addr;
    int client_length;
    int i,j;
    short sample;
    unsigned char audio_buffer[BUFFER_SIZE*2*2];

    spectrum_length=sizeof(spectrum_addr);
    client_length=sizeof(client_addr);

    // create a socket to receive spectrum from the server
    spectrum_socket=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(spectrum_socket<0) {
        perror("spectrum_thread: create spectrum socket failed");
        exit(1);
    }


    memset(&spectrum_addr,0,spectrum_length);

    spectrum_addr.sin_family=AF_INET;
    spectrum_addr.sin_addr.s_addr=htonl(INADDR_ANY);
    spectrum_addr.sin_port=htons(SPECTRUM_PORT+(receiver*2));

    if(bind(spectrum_socket,(struct sockaddr*)&spectrum_addr,spectrum_length)<0) {
        perror("spectrum_thread: bind socket failed for spectrum socket");
        exit(1);
    }

    fprintf(stderr,"ozy_init: spectrum bound to port %d socket=%d\n",spectrum_addr.sin_port,spectrum_socket);

fprintf(stderr,"spectrum_thread: socket %d\n",spectrum_socket);

    if(local_audio) {
        open_local_audio();
    }


fprintf(stderr,"output_sample_increment=%d\n",output_sample_increment);
    
    while(1) {
        bytes_read=recvfrom(spectrum_socket,(char*)input_buffer,BUFFER_SIZE*2*4,0,(struct sockaddr*)&client_addr,&client_length);
        if(bytes_read<0) {
            perror("recvfrom socket failed for spectrum buffer");
            exit(1);
        }

        Audio_Callback (input_buffer,&input_buffer[BUFFER_SIZE],
                                output_buffer,&output_buffer[BUFFER_SIZE], buffer_size, 0);

        if(local_audio) {
            // play the audio back locally
            write_local_audio(&output_buffer,&output_buffer[BUFFER_SIZE],BUFFER_SIZE,output_sample_increment);
        } else {
            // send the audio back to the server
            bytes_written=sendto(audio_socket,output_buffer,sizeof(output_buffer),0,(struct sockaddr *)&audio_addr,audio_length);
            if(bytes_written<0) {
                fprintf(stderr,"sendto audio failed: %d\n",bytes_written);
                exit(1);
            }
        }

    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief send a command
* 
* @param command
*/
void send_command(char* command) {
    int rc;

fprintf(stderr,"send_command: command='%s'\n",command);

    rc=send(command_socket,command,strlen(command),0);
    if(rc<0) {
        fprintf(stderr,"send command failed: %d\n",rc);
        exit(1);
    }

    rc=recv(command_socket,response,sizeof(response),0);
    if(rc<0) {
        fprintf(stderr,"read response failed: %d\n",rc);
    }
    response[rc]=0;

fprintf(stderr,"send_command: response='%s'\n",response);
}

void* keepalive_thread(void* arg) {
    char command[128];
    sprintf(command,"keepalive %d",receiver);
    while(1) {
        sleep(5);
        send_command(command);
    }
}

int make_connection() {
    char *token;
    int result;

    result=0;
    sprintf(command,"attach %d",receiver);
    //sprintf(command,"connect %d %d",receiver,SPECTRUM_PORT+(receiver*2));
    send_command(command);

    token=strtok(response," ");
    if(token!=NULL) {
        if(strcmp(token,"OK")==0) {
            token=strtok(NULL," ");
            if(token!=NULL) {
                result=0;
                sampleRate=atoi(token);
fprintf(stderr,"connect: sampleRate=%d\n",sampleRate);
                switch(sampleRate) {
                    case 48000:
                        setSpeed(SPEED_48KHZ);
                        break;
                    case 96000:
                        setSpeed(SPEED_96KHZ);
                        break;
                    case 192000:
                        setSpeed(SPEED_192KHZ);
                        break;
                }
            } else {
                fprintf(stderr,"invalid response to connect: %s\n",response);
                result=1;
            }
        } else if (strcmp(token,"ERROR")==0) {
            result=1;
        } else {
            fprintf(stderr,"invalid response to connect: %s\n",response);
            result=1;
        }
    }

    sprintf(command,"start iq %d",SPECTRUM_PORT+(receiver*2));
    send_command(command);

    return result;
}

void ozyDisconnect() {
    char command[128];
    sprintf(command,"detach %d",receiver);
    send_command(command);

    close(command_socket);
    close(audio_socket);
}

int set_frequency() {
    char *token;
    int result;

    result=0;
    sprintf(command,"frequency %lld",ddsAFrequency);
    send_command(command);
    token=strtok(response," ");
    if(token!=NULL) {
        if(strcmp(token,"OK")==0) {
            result=0;
        } else if (strcmp(token,"ERROR")==0) {
            result=1;
        } else {
            fprintf(stderr,"invalid response to set frequency: %s\n",response);
            result=1;
        }
    }

    return result;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Process the ozy input buffer
* 
* @param buffer
*/
void process_ozy_input_buffer(char* buffer) {
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
* @brief Set the speed
* 
* @param speed
*/
void setSpeed(int s) {
fprintf(stderr,"setSpeed %d\n",s);
    speed=s;
    control_out[1]=control_out[1]&0xFC;
    control_out[1]=control_out[1]|s;
    if(s==SPEED_48KHZ) {
        sampleRate=48000;
        output_sample_increment=1;
        SetSampleRate((double)sampleRate);
        SetRXOsc(0,0,0.0);
        setFilter(filter);
        setMode(mode);
        SetRXOutputGain(0,0,volume/100.0);
    } else if(s==SPEED_96KHZ) {
        sampleRate=96000;
        output_sample_increment=2;
        SetSampleRate((double)sampleRate);
        SetRXOsc(0,0,0.0);
        setFilter(filter);
        setMode(mode);
        SetRXOutputGain(0,0,volume/100.0);
    } else if(s==SPEED_192KHZ) {
        sampleRate=192000;
        output_sample_increment=4;
        SetSampleRate((double)sampleRate);
        SetRXOsc(0,0,0.0);
        setFilter(filter);
        setMode(mode);
        SetRXOutputGain(0,0,volume/100.0);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Initialize Ozy Server
* 
* @return 
*/
int ozy_init() {
    int rc;
    struct hostent *h;
    char command[64];
    int on=1;

    h=gethostbyname(server_address);
    if(h==NULL) {
        fprintf(stderr,"ozy_init: unknown host %s\n",server_address);
        exit(1);
    }

    server_port=11000;

    // create a socket to send commands to the server
    command_socket=socket(AF_INET,SOCK_STREAM,0);
    if(command_socket<0) {
        perror("ozy_init: create command socket failed");
        exit(1);
    }
   
    setsockopt(command_socket, SOL_SOCKET, SO_REUSEADDR, &on, sizeof(on));

    memset(&command_addr,0,command_length);

    command_addr.sin_family=AF_INET;
    command_addr.sin_addr.s_addr=htonl(INADDR_ANY);
    command_addr.sin_port=htons(COMMAND_PORT+(receiver*2));

    if(bind(command_socket,(struct sockaddr*)&command_addr,command_length)<0) {
        perror("ozy_init: bind socket failed for command socket");
        exit(1);
    }

    fprintf(stderr,"ozy_init: command bound to port %d socket %d\n",ntohs(command_addr.sin_port),command_socket);

    

    // create a socket to send audio to the server
    audio_socket=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(audio_socket<0) {
        perror("ozy_init: create audio socket failed");
        exit(1);
    }

    memset(&audio_addr,0,audio_length);

    audio_addr.sin_family=AF_INET;
    audio_addr.sin_family=h->h_addrtype;
    memcpy((char *)&audio_addr.sin_addr.s_addr,h->h_addr_list[0],h->h_length);
    audio_addr.sin_port=htons(AUDIO_PORT+(receiver*2));

/*
    if(bind(audio_socket,(struct sockaddr*)&audio_addr,audio_length)<0) {
        perror("ozy_init: bind socket failed for audio socket");
        exit(1);
    }

    fprintf(stderr,"ozy_init: audio bound to port %d socket %d\n",audio_addr.sin_port,audio_socket);
*/

    // setup the server address

    memset(&server_addr,0,server_length);
    server_addr.sin_family=h->h_addrtype;
    memcpy((char *)&server_addr.sin_addr.s_addr,h->h_addr_list[0],h->h_length);
    server_addr.sin_port=htons(server_port);


    fprintf(stderr,"ozy_init: server %s\n",server_address);

    // connect
    rc=connect(command_socket,(struct sockaddr*)&server_addr,server_length);
    if(rc<0) {
        perror("ozy_init: connect failed");
        exit(1);
    }


    if(make_connection()) {
        fprintf(stderr,"connect failed\n");
        exit(1);
    }

    // create a thread to send keepalive messages
/*
    rc=pthread_create(&keepalive_thread_id,NULL,keepalive_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on keepalive_thread: rc=%d\n", rc);
    }
*/

    // create a thread to listen for spectrum frames
    rc=pthread_create(&spectrum_thread_id,NULL,spectrum_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on spectrum_thread: rc=%d\n", rc);
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
    int result=0;
    return result;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief save Ozy state
* 
* @return 
*/
void ozySaveState() {
    char string[128];
}

/* --------------------------------------------------------------------------*/
/** 
* @brief resore Ozy state
* 
* @return 
*/
void ozyRestoreState() {
    char *value;
}


void ozy_set_local_audio(int state) {
    local_audio=state;
}
