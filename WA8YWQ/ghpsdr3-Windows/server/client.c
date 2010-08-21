/**
* @file client.c
* @brief Handle client connection
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

#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>

#ifdef __linux__
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <pthread.h>
#else
#include "pthread.h"
#endif

#include <string.h>

#include "client.h"
#include "ozy.h"
#include "receiver.h"
#include "messages.h"

extern RECEIVER receiver[MAX_RECEIVERS];

int MercuryAudioReceiver;		// the one receiver whose demodulated audio is to be sent to Mercury's headphone / speaker output

short audio_port=AUDIO_PORT;

char* parse_command(CLIENT* client,char* command);
void* audio_thread(void* arg);

/**
 * client_thread() is started by listener_thread when a TCP connection request is received;
 * It receives command strings from a dspserver, calls parse_command(), and sends a response
 * back to dspserver.
 */
void* client_thread(void* arg) {
    CLIENT* client=(CLIENT*)arg;
    char command[80];
    int bytes_read;
    char* response;

fprintf(stderr,"client connected: %s:%d\n",inet_ntoa(client->address.sin_addr),ntohs(client->address.sin_port));

    client->state=RECEIVER_DETACHED;

    while(1) {
        bytes_read=recv(client->socket,command,sizeof(command),0);
        if(bytes_read<=0) {
            break;
        }
        command[bytes_read]=0;
        response=parse_command(client,command);
        send(client->socket,response,strlen(response),0);

fprintf(stderr,"response(Rx%d): '%s'\n",client->receiver,response);
    }

    if(client->state==RECEIVER_ATTACHED) {
        receiver[client->receiver].client=(CLIENT*)NULL;
        client->state=RECEIVER_DETACHED;
    }

#ifdef __linux__
    close(client->socket);
#else
    closesocket(client->socket);
#endif

fprintf(stderr,"client disconnected: %s:%d\n",inet_ntoa(client->address.sin_addr),ntohs(client->address.sin_port));

    free(client);
	return (void *) NULL;
}

/**
 * Recognize the following command strings, sent from a dspserver:
 * - attach ReceiverNumber  (0, 1, 2, or 3)
 * - detach ReceiverNumber
 * - frequency f (f is an integer, Hz)
 * - start iq port
 * - start bandscope port
 * - stop iq n
 * - stop bandscope

 * \param client A pointer to struct client
 * \param command A null-terminated string to be scanned for one of the above commands
 * \return OK or INVALID_COMMAND
 */
char* parse_command(CLIENT* client,char* command) {
    int selectAudio;
    char* token;

fprintf(stderr,"parse_command(Rx%d): '%s'\n",client->receiver,command);

    token=strtok(command," \r\n");
    if(token!=NULL) 
	{	if(strcmp(token,"attach")==0) 
		{	// select receiver
            token=strtok(NULL," \r\n");
            if(token!=NULL) 
			{	int rx=atoi(token);
                return attach_receiver(rx,client);
            } else 
			{	return INVALID_COMMAND;
            }
        } else if(strcmp(token,"detach")==0) 
		{	// select receiver
            token=strtok(NULL," \r\n");
            if(token!=NULL) 
			{	int rx=atoi(token);
                return detach_receiver(rx,client);
            } else 
			{	return INVALID_COMMAND;
            }
        } else if(strcmp(token,"frequency")==0) 
		{	// set frequency
            token=strtok(NULL," \r\n");
            if(token!=NULL) 
			{	long f=atol(token);
				return set_frequency(client,f);
            } else 
			{	return INVALID_COMMAND;
            }
        } else if(strcmp(token,"start")==0) 
		{	token=strtok(NULL," \r\n");
            if(token!=NULL) 
			{	if(strcmp(token,"iq")==0) 
				{	token=strtok(NULL," \r\n");
					if(token!=NULL) 
					{	client->iq_port=atoi(token);
						// Remember the last receiver started, so that one will send demodulated data back to Ozy / Mercury.
						// Eventually, there'll be a command to select whose audio goes to the headphone / speaker output.
						MercuryAudioReceiver = client->receiver;

						// Start a thread to handle demodulated audio from this receiver.
						if(pthread_create(&receiver[client->receiver].audio_thread_id,NULL,audio_thread,&receiver[client->receiver])!=0) 
						{	fprintf(stderr,"failed to create audio thread for rx %d\n",client->receiver);
	                        exit(1);
						}
						return OK;
					} else 
					{	return INVALID_COMMAND;
				    }
				} else if(strcmp(token,"bandscope")==0) 
				{	token=strtok(NULL," \r\n");
                    if(token!=NULL) 
					{	client->bs_port=atoi(token);
						return OK;
                    } else 
					{	// invalid command string
			            return INVALID_COMMAND;
				    }
				} else 
				{	// invalid command string
					return INVALID_COMMAND;
				}
			}
			else return INVALID_COMMAND;
        } else if(strcmp(token,"stop")==0) 
		{	token=strtok(NULL," \r\n");
            if(token!=NULL) 
			{	if(strcmp(token,"iq")==0) 
				{	client->iq_port=-1;
						return OK;
				} else if(strcmp(token,"bandscope")==0) 
				{	client->bs_port=-1;
					return OK;
				}
                 else 
				{	// invalid command string
					return INVALID_COMMAND;
				}
			}
			else 
			{	// invalid command string
                return INVALID_COMMAND;
            }
		} else if (strcmp(token, "selectAudio") == 0) 
		{	// change selection of which receiver's audio goes to Mercury headphone output
			token = strtok(NULL, " \r\n");
			if (token != NULL) 
			{	selectAudio = atoi(token);
				if (   (selectAudio >= 0) 
					&& (selectAudio < ozy_get_receivers() ) 
					&& (receiver[selectAudio].client != NULL)
					)
				{	MercuryAudioReceiver = selectAudio;
					return OK;
				}
				else return RECEIVER_INVALID;
			}
			else return RECEIVER_INVALID;
        } else 
		{	// invalid command string
            return INVALID_COMMAND;
        }
    } else 
	{	// empty command string
        return INVALID_COMMAND;
    }
	return INVALID_COMMAND;
}

/**
 * Create a UDP socket receiving demodulated audio from dspserver and sending to Mercury headphone output;
 * Then read bytes from the socket and call process_ozy_output_buffer().
 * \param arg A pointer to a struct RECEIVER
 */
void* audio_thread(void* arg) {		// #### problem:  if dspserver for Rx 0 has not been started, there's no audio data coming
    RECEIVER *rx=(RECEIVER*)arg;	// #### to server.  The recvfrom() will block, and process_ozy_output_buffer() will not be
									// #### called.  As a result, write_ozy_output_buffer() will never be called, and no receiver
									// #### frequencies will be updated. #### I cannot find where audio data from dspserver is
									// #### blocked except for Rx 0, but I think I've seen explicit code to do that.  
									// #### It might be simply that dspserver for every receiver sends demodulated audio, on
									// #### ports 15000 + 2*Rx, but, because rx->id is not set here, every audio thread tries to
									// #### receive on port 15000.  If Rx 0 dspserver has not been started, nothing is ever sent
									// #### to Ozy / Mercury.
    struct sockaddr_in audio;
    int audio_length;
    int old_state, old_type;
    int bytes_read;
    int on=1;

fprintf(stderr,"audio_thread port=%d\n",audio_port+(rx->id*2));		// ####  rx->id is NEVER SET;  it's always zero

    pthread_setcancelstate(PTHREAD_CANCEL_ENABLE,&old_state);
    pthread_setcanceltype(PTHREAD_CANCEL_ASYNCHRONOUS,&old_type);

    rx->audio_socket=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(rx->audio_socket<0) {
        perror("create socket failed for server audio socket");
        exit(1);
    }

    setsockopt(rx->audio_socket, SOL_SOCKET, SO_REUSEADDR, &on, sizeof(on));

    audio_length=sizeof(audio);
    memset(&audio,0,audio_length);
    audio.sin_family=AF_INET;
    audio.sin_addr.s_addr=htonl(INADDR_ANY);
    audio.sin_port=htons(audio_port+(rx->id*2));

    if(bind(rx->audio_socket,(struct sockaddr*)&audio,audio_length)<0) {
        perror("bind socket failed for server audio socket");
        exit(1);
    }

    fprintf(stderr,"listening for rx %d audio on port %d\n",rx->id,audio_port+(rx->id*2));

    while(1) {
        // get audio from a client
        bytes_read=recvfrom(rx->audio_socket, (char *)rx->output_buffer,sizeof(rx->output_buffer),0,(struct sockaddr*)&audio,&audio_length);
        if(bytes_read<0) {
            perror("recvfrom socket failed for audio buffer");
            exit(1);
        }
		if (rx->id == MercuryAudioReceiver)		// if this receiver is selected, send the demodulated audio to Mercury's headphone output
			process_ozy_output_buffer(rx->output_buffer,&rx->output_buffer[BUFFER_SIZE]);

    }
}

