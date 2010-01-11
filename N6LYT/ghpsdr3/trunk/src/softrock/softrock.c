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
    int rc;
    int i,j;

    while(1) {

        // read an input buffer (blocks until all bytes read)
        rc=softrock_read(receiver[current_receiver].input_buffer,&receiver[current_receiver].input_buffer[BUFFER_SIZE]);
        if(rc==0) {
            // process input buffer
            rx_frame++;
            input_buffers++;
            send_IQ_buffer(current_receiver);
        } else {
            fprintf(stderr,"softrock_read returned %d\n",rc);
        }
        current_receiver++;

        if(current_receiver==receivers) {
            current_receiver=0;
        }
    }
}

void process_softrock_output_buffer(float* left_output_buffer,float* right_output_buffer) {
    softrock_write(left_output_buffer,right_output_buffer);
    
}
