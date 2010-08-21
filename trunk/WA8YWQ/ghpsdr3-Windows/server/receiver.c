/**
* @file receiver.c
* @brief manage client attachment to receivers
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

#ifdef __linux__
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#endif

#include <string.h>

#include "client.h"
#include "receiver.h"
#include "messages.h"
#include "ozy.h"
#include "util.h"

#define SMALL_PACKETS

RECEIVER receiver[MAX_RECEIVERS];
static int iq_socket;
static struct sockaddr_in iq_addr;
static int iq_length;

static char response[80];

static unsigned long sequence=0L;

/**
 * - Set the client pointer to NULL for each receiver in the array receiver[].
 * - Open one UDP socket that will be used to send I & Q receive samples to each dspserver.
 *   At this point port 11002 is used, but the port number will be dynamically changed as
 *   samples are sent to each dspserver.
 */
void init_receivers() {
    int i;
    for(i=0;i<MAX_RECEIVERS;i++) {
        receiver[i].client=(CLIENT*)NULL;
		receiver[i].id = i;
    }

    iq_socket=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(iq_socket<0) {
        perror("create socket failed for iq_socket\n");
        exit(1);
    }

    iq_length=sizeof(iq_addr);
    memset(&iq_addr,0,iq_length);
    iq_addr.sin_family=AF_INET;
    iq_addr.sin_addr.s_addr=htonl(INADDR_ANY);
    iq_addr.sin_port=htons(11002);

    if(bind(iq_socket,(struct sockaddr*)&iq_addr,iq_length)<0) {
        perror("bind socket failed for iq socket");
        exit(1);
    }

}

/**
 * \param rx Integer = receiver number, 0, 1, 2, or 3
 * Store pointer to struct client in receiver array.
 * \param client Pointer to struct client
 * Set receiver number in struct client.
 */
char* attach_receiver(int rx,CLIENT* client) {

    if(client->state==RECEIVER_ATTACHED) {
        return CLIENT_ATTACHED;
    }

    if(rx>=ozy_get_receivers()) {
        return RECEIVER_INVALID;
    }

    if(receiver[rx].client!=(CLIENT *)NULL) {
        return RECEIVER_IN_USE;
    }
    
    client->state=RECEIVER_ATTACHED;
    receiver[rx].client=client;
    client->receiver=rx;

    sprintf(response,"%s %d",OK,ozy_get_sample_rate());

    return response;
}

char* detach_receiver(int rx,CLIENT* client) {
    if(client->state==RECEIVER_DETACHED) {
        return CLIENT_DETACHED;
    }

    if(rx>=ozy_get_receivers()) {
        return RECEIVER_INVALID;
    }

    if(receiver[rx].client!=client) {
        return RECEIVER_NOT_OWNER;
    }

    client->state=RECEIVER_DETACHED;
    receiver[rx].client=(CLIENT*)NULL;

    return OK;
}

/**
 * Store the new frequency for this receiver in its struct client, and set the corresponding "frequency has changed" flag
 * \param client Pointer to struct client
 * \param frequency Long integer = frequency to which this receiver is tuned, in Hz
 */
char* set_frequency(CLIENT* client,long frequency) {
    if(client->state==RECEIVER_DETACHED) {
        return CLIENT_DETACHED;
    }

    if(client->receiver<0) {
        return RECEIVER_INVALID;
    }

    receiver[client->receiver].frequency=frequency;
    receiver[client->receiver].frequency_changed=1;

    return OK;
}

/**
 * - Place the IQ port number into a struct sockaddr_in, confusingly named "client", which will be passed to sendto().
 * - Copy data from this receiver's buffer into a temporary buffer & send the samples to dspserver using sendto().
 * If SMALL_PACKETS is defined, the UDP packets are limited to a maximum of 512 bytes.
  * \param rx Integer = receiver number, 0, 1, 2, or 3
*/
void send_IQ_buffer(int rx) {
    struct sockaddr_in client;
    int client_length;
    unsigned short offset;
    unsigned short length;
    BUFFER buffer;
    int rc;

    if(rx>=ozy_get_receivers()) {
        fprintf(stderr,"send_IQ_buffer: invalid rx: %d\n",rx);
        return;
    }

    if(receiver[rx].client!=(CLIENT*)NULL) {
        if(receiver[rx].client->iq_port!=-1) {
            // send the IQ buffer

            client_length=sizeof(client);
            memset((char*)&client,0,client_length);
            client.sin_family=AF_INET;
            client.sin_addr.s_addr=receiver[rx].client->address.sin_addr.s_addr;
            client.sin_port=htons(receiver[rx].client->iq_port);

#ifdef SMALL_PACKETS
            // keep UDP packets to 512 bytes or less
            //     8 bytes sequency number
            //     2 byte offset
            //     2 byte length
            offset=0;
			
            while(offset<sizeof(receiver[rx].input_buffer)) {
                buffer.sequence=sequence;
#ifndef __linux__
				buffer.sequenceHi = 0L;
#endif
                buffer.offset=offset;
                buffer.length=sizeof(receiver[rx].input_buffer)-offset;
                if(buffer.length>500) buffer.length=500;
                memcpy((char*)&buffer.data[0],(char*)&receiver[rx].input_buffer[offset/4],buffer.length);
//fprintf(stderr,"send_IQ: rx:%d: %lld:%d:%d\n",rx,buffer.sequence,buffer.offset,buffer.length);
                rc=sendto(iq_socket,(char*)&buffer,sizeof(buffer),0,(struct sockaddr*)&client,client_length);
                if(rc<=0) {
                    perror("sendto failed for iq data");
                    exit(1);
                }
                offset+=buffer.length;
            }
            sequence++;

#else
            rc=sendto(iq_socket,receiver[rx].input_buffer,sizeof(receiver[rx].input_buffer),0,(struct sockaddr*)&client,client_length);
            if(rc<=0) {
                perror("sendto failed for iq data");
                exit(1);
            }
#endif
 
        }
    }
}
