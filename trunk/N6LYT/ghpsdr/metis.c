/**
* @file metis.c
* @brief Metis protocol implementation
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
#ifdef __linux
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/ioctl.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <net/if_arp.h>
#include <net/if.h>
#include <ifaddrs.h>
#include <pthread.h>
#else
#include "pthread.h"
#endif

#include <string.h>
#include <errno.h>

#include "discovery.h"
#include "metis.h"
#include "ozy.h"

#define PORT_BASE 1024
#define DATA_PORT PORT_BASE

static unsigned char hw_address[6];
static long ip_address;

static int data_socket;
static struct sockaddr_in data_addr;
static int data_addr_length;

static unsigned char buffer[70];

static pthread_t receive_thread_id;
static int found=0;

int running;

int ep;
long sequence=-1;

void metis_stop(void);

void* metis_receive_thread(void* arg);
void metis_send_buffer(char* buffer,int length);

#define inaddrr(x) (*(struct in_addr *) &ifr->x[sizeof sa.sin_port])
/*
static int get_addr(int sock, char * ifname) {

  struct ifreq *ifr;
  struct ifreq ifrr;
  struct sockaddr_in sa;
  unsigned char      *u;
  int i;

  ifr = &ifrr;
  ifrr.ifr_addr.sa_family = AF_INET;
  strncpy(ifrr.ifr_name, ifname, sizeof(ifrr.ifr_name));

  if (ioctl(sock, SIOCGIFADDR, ifr) < 0) {
    printf("No %s interface.\n", ifname);
    return -1;
  }

  ip_address=inaddrr(ifr_addr.sa_data).s_addr;

  if (ioctl(sock, SIOCGIFHWADDR, ifr) < 0) {
    printf("No %s interface.\n", ifname);
    return -1;
  }

  u = (unsigned char *) &ifr->ifr_addr.sa_data;

  for(i=0;i<6;i++)
      hw_address[i]=u[i];


  return 0;
}
*/

void metis_stop() {
    int i;

    // send a packet to stop the stream
    buffer[0]=0xEF;
    buffer[1]=0xFE;
    buffer[2]=0x04;    // data send state
    buffer[3]=0x00;    // 0x00=stop

    for(i=0;i<60;i++) {
        buffer[i+4]=0x00;
    }

    if(sendto(data_socket,buffer,64,0,(struct sockaddr*)&data_addr,data_addr_length)<0) {
        perror("sendto socket failed for stop\n");
        exit(1);
    }

    running=0;
    close(data_socket);
}

void metis_start_receive_thread() {
    int i;
    int rc;
    struct hostent *h;

    fprintf(stderr,"Metis starting receive thread\n");

    DISCOVERED* d=&discovered[selected_device];

    data_socket=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(data_socket<0) {
        perror("metis: create socket failed for data_socket\n");
        exit(-1);
    }

    // bind to the interface
    if(bind(data_socket,(struct sockaddr*)&d->interface_address,d->interface_length)<0) {
        perror("metis: bind socket failed for data_socket\n");
        exit(-1);
    }

    memcpy(&data_addr,&d->address,d->address_length);
    data_addr_length=d->address_length;
    data_addr.sin_port=htons(DATA_PORT);


    rc=pthread_create(&receive_thread_id,NULL,metis_receive_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on metis_receive_thread: rc=%d\n", rc);
        exit(-1);
    }

    
    ozy_prime();
    
    // send a packet to start the stream
    buffer[0]=0xEF;
    buffer[1]=0xFE;
    buffer[2]=0x04;    // data send state
    buffer[3]=0x03;    // send EP6 and EP4 data (0x00=stop)

    for(i=0;i<60;i++) {
        buffer[i+4]=0x00;
    }

    if(sendto(data_socket,buffer,64,0,(struct sockaddr*)&data_addr,data_addr_length)<0) {
        perror("sendto socket failed for start\n");
        exit(1);
    }

}

void* metis_receive_thread(void* arg) {
    struct sockaddr_in addr;
    int length;
    unsigned char buffer[2048];
    int bytes_read;

    running=1;

    length=sizeof(addr);
    while(running) {
   	bytes_read=recvfrom(data_socket,buffer,sizeof(buffer),0,(struct sockaddr*)&addr,&length);
        if(bytes_read<0) {
            perror("recvfrom socket failed for metis_receive_thread");
            exit(1);
        }

        if(buffer[0]==0xEF && buffer[1]==0xFE) {
            switch(buffer[2]) {
                case 1:
                    // get the end point
                    ep=buffer[3]&0xFF;

                    // get the sequence number
                    sequence=((buffer[4]&0xFF)<<24)+((buffer[5]&0xFF)<<16)+((buffer[6]&0xFF)<<8)+(buffer[7]&0xFF);
                    //fprintf(stderr,"received data ep=%d sequence=%ld\n",ep,sequence);

                    switch(ep) {
                        case 6: // EP6
                            // process the data
                            process_ozy_input_buffer(&buffer[8]);
                            process_ozy_input_buffer(&buffer[520]);
                            break;
                        case 4: // EP4
                            process_bandscope_buffer(&buffer[8]);
                            process_bandscope_buffer(&buffer[520]);
                            break;
                        default:
                            fprintf(stderr,"unexpected EP %d length=%d\n",ep,bytes_read);
                            break;
                    }
                    break;
                case 2:  // response to a discovery packet
                    fprintf(stderr,"unexepected discovery response when not in discovery mode\n");
                    break;
                default:
                    fprintf(stderr,"unexpected packet type: 0x%02X\n",buffer[2]);
                    break;
            }
        } else {
            fprintf(stderr,"received bad header bytes on data port %02X,%02X\n",buffer[0],buffer[1]);
        }

    }
    
}

static unsigned char output_buffer[1032];
static long send_sequence=-1;
static int offset=8;

int metis_write(unsigned char ep,char* buffer,int length) {
    int i;

    if(offset==8) {

        send_sequence++;
        output_buffer[0]=0xEF;
        output_buffer[1]=0xFE;
        output_buffer[2]=0x01;
        output_buffer[3]=ep;
        output_buffer[4]=(send_sequence>>24)&0xFF;
        output_buffer[5]=(send_sequence>>16)&0xFF;
        output_buffer[6]=(send_sequence>>8)&0xFF;
        output_buffer[7]=(send_sequence)&0xFF;

        // copy the buffer over
        for(i=0;i<512;i++) {
            output_buffer[i+offset]=buffer[i];
        }
        offset=520;
    } else {
        // copy the buffer over
        for(i=0;i<512;i++) {
            output_buffer[i+offset]=buffer[i];
        }
        offset=8;

        // send the buffer
        metis_send_buffer(&output_buffer[0],1032);

    }

    return length;
}

void metis_send_buffer(char* buffer,int length) {
//fprintf(stderr,"metis_send_buffer\n");
    if(sendto(data_socket,buffer,length,0,(struct sockaddr*)&data_addr,data_addr_length)!=length) {
        perror("sendto socket failed for metis_send_data\n");
        //exit(1);
    }
}
