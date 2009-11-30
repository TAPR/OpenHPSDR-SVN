/**
* @file softrock.c
* @brief Softrock audio implementation
* @author John Melton, G0ORX/N6LYT
* @version 0.1
* @date 2009-10-13
*/


/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/

#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <arpa/inet.h>
#include <netdb.h>

#include "client.h"
#include "softrockio.h"
#include "receiver.h"
#include "util.h"

static pthread_t softrock_io_thread_id;

static int rx_frame=0;
static int tx_frame=0;
static int receivers=1;
static int current_receiver=0;

static int speed=0;
static int sample_rate=48000;

static int samples=0;

static int input_buffers;

static struct sockaddr_in client;
static int client_length;

static int iq_socket;
static struct sockaddr_in iq_address;
static int iq_address_length;

char device[80];

void* softrock_io_thread(void* arg);

void process_softrock_input_buffer(char* buffer);

int create_softrock_thread() {
    int rc;

    softrock_init();

    // create a thread to read from the audio deice
    rc=pthread_create(&softrock_io_thread_id,NULL,softrock_io_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on softrock_io_thread: rc=%d\n", rc);
        exit(1);
    }

    return 0;
}

void softrock_set_device(char* d) {
fprintf(stderr,"softrock_set_device %s\n",d);
    strcpy(device,d);
}

char* softrock_get_device() {
fprintf(stderr,"softrock_get_device %s\n",device);
    return device;
}

void softrock_set_receivers(int r) {
    if(r>MAX_RECEIVERS) {
        fprintf(stderr,"MAX Receivers is 8!\n");
        exit(1);
    }
    receivers=r;
}

int softrock_get_receivers() {
    return receivers;
}

void softrock_set_sample_rate(int r) {
fprintf(stderr,"softrock_set_sample_rate %d\n",r);
    switch(r) {
        case 48000:
            sample_rate=r;
            speed=0;
            break;
        case 96000:
            sample_rate=r;
            speed=1;
            break;
        case 192000:
            sample_rate=r;
            speed=2;
            break;
        default:
            fprintf(stderr,"Invalid sample rate (48000,96000,192000)!\n");
            exit(1);
            break;
    }
}

int softrock_get_sample_rate() {
fprintf(stderr,"softrock_get_sample_rate %d\n",sample_rate);
    return sample_rate;
}

int softrock_init() {
    int rc;
    int i;

    iq_socket=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(iq_socket<0) {
        perror("create socket failed for iq samples");
        exit(1);
    }

    iq_address_length=sizeof(iq_address);
    memset(&iq_address,0,iq_address_length);
    iq_address.sin_family=AF_INET;
    iq_address.sin_addr.s_addr=htonl(INADDR_ANY);
    iq_address.sin_port=htons(0);

    if(bind(iq_socket,(struct sockaddr*)&iq_address,iq_address_length)<0) {
        perror("bind socket failed for iq socket");
        exit(1);
    }

    // open softrock audio
    rc = softrock_open();
    if (rc != 0) {
        fprintf(stderr,"Cannot open softrock\n");
        return (-1);
    }

    for(i=0;i<receivers;i++) {
        receiver[i].frequency=7056000L;
        receiver[i].frequency_changed=1;
    }

fprintf(stderr,"server configured for %d receivers at %d\n",receivers,sample_rate);
    return rc;
}

void* softrock_io_thread(void* arg) {
    unsigned char input_buffer[BUFFER_SIZE*6]; // samples * 3 * 2
    int bytes;
    int i,j;

    while(1) {

        // read an input buffer (blocks until all bytes read)
        bytes=softrock_read(input_buffer,sizeof(input_buffer));
        if (bytes < 0) {
            fprintf(stderr,"softrock_io_thread: read failed %d\n",bytes);
        } else if (bytes != sizeof(input_buffer)) {
            fprintf(stderr,"sfoftrock_io_thread: only read %d bytes\n",bytes);
        } else {
            // process input buffer
            rx_frame++;
            process_softrock_input_buffer(input_buffer);
        }
        input_buffers++;

        current_receiver++;

        if(current_receiver==receivers) {
            current_receiver=0;
        }


    }
}

void process_softrock_input_buffer(char* buffer) {
    int b=0;
    int r;
    int left_sample,right_sample;
    float left_sample_float,right_sample_float;
    int rc;

        // extract the samples
    while(b<(BUFFER_SIZE*6)) {
        // extract each of the receivers
        for(r=0;r<receivers;r++) {
            left_sample   = (int)((unsigned char)buffer[b++]);
            left_sample  += (int)((unsigned char)buffer[b++])<<8;
            left_sample  += (int)((signed char)buffer[b++])<<16;
            right_sample  = (int)((unsigned char)buffer[b++]);
            right_sample += (int)((unsigned char)buffer[b++])<<8;
            right_sample += (int)((signed char)buffer[b++])<<16;
/*
            left_sample_float=(float)left_sample/32767.0; // 16 bit sample
            right_sample_float=(float)right_sample/32767.0; // 16 bit sample
*/
            left_sample_float=(float)left_sample/8388607.0; // 24 bit sample
            right_sample_float=(float)right_sample/8388607.0; // 24 bit sample
            receiver[r].input_buffer[samples]=left_sample_float;
            receiver[r].input_buffer[samples+BUFFER_SIZE]=right_sample_float;
        }
        samples++;

        // when we have enough samples send them to the clients
        if(samples==BUFFER_SIZE) {
            // send I/Q data to clients
            for(r=0;r<receivers;r++) {
                send_IQ_buffer(r);
            }
            samples=0;
        }
    }

}

void process_softrock_output_buffer(float* left_output_buffer,float* right_output_buffer) {
    int i;
    unsigned char output_buffer[BUFFER_SIZE*3*2];
    int left_sample,right_sample;
    int b=0;

    for(i=0;i<BUFFER_SIZE;i++) {
/*
        left_sample=(int)(left_output_buffer[i]*32767.0F);
        right_sample=(int)(right_output_buffer[i]*32767.0F);
*/
        left_sample=(int)(left_output_buffer[i]*8388607.0F);
        right_sample=(int)(right_output_buffer[i]*8388607.0F);
        output_buffer[b++]=left_sample&0xFF;
        output_buffer[b++]=(left_sample>>8)&0xFF;
        output_buffer[b++]=(left_sample>>16)&0xFF;
        output_buffer[b++]=right_sample&0xFF;
        output_buffer[b++]=(right_sample>>8)&0xFF;
        output_buffer[b++]=(right_sample>>16)&0xFF;
    }

    softrock_write(output_buffer,sizeof(output_buffer));
    
}
