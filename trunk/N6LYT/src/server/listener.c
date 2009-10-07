#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>


#include "debug.h"
#include "listener.h"
#include "ozy.h"
#include "receiver.h"

unsigned short command_port=11000;
unsigned short audio_port=15000;

#define BUFFER_SIZE 1024

static int listening=1;

static struct sockaddr_in client;
static socklen_t client_length=sizeof(client);
static int bytes_read;

char response[128];

void cleanup_receiver(int rx) {
    int rc;

fprintf(stderr,"cleanup_receiver: %d\n",rx);
    // terminate
    if(receiver[rx].watchdog_thread_id!=-1) {
fprintf(stderr,"cancel watchdog thread\n");
        rc=pthread_cancel(receiver[rx].watchdog_thread_id);
        if(rc!=0) {
            fprintf(stderr,"cleanup_receiver: pthread_cancel failed for watchdog_thread: %d\n",rc);
        }
        receiver[rx].watchdog_thread_id=-1;
    }
    if(receiver[rx].audio_thread_id!=-1) {
fprintf(stderr,"cancel audio thread\n");
        rc=pthread_cancel(receiver[rx].audio_thread_id);
        if(rc!=0) {
            fprintf(stderr,"cleanup_receiver: pthread_cancel failed for audio_thread: %d\n",rc);
        }
        receiver[rx].audio_thread_id=-1;
    }
fprintf(stderr,"close audio socket\n");
    rc=close(receiver[rx].audio_socket);
    if(rc!=0) {
        fprintf(stderr,"cleanup_receiver: close failed for audio_socket: %d\n",rc);
    }
    receiver[rx].audio_socket=-1;

    receiver[rx].address=-1L;
    receiver[rx].port=-1;
    receiver[rx].output_buffer_valid=0;
}

void* watchdog_thread(void* arg) {
    RECEIVER *rx=(RECEIVER*)arg;
    int old_state, old_type;

    pthread_setcancelstate(PTHREAD_CANCEL_ENABLE,&old_state);
    pthread_setcanceltype(PTHREAD_CANCEL_ASYNCHRONOUS,&old_type);

    while(1) {
        sleep(10);
        if(rx->keepalive>0) {
            rx->keepalive=0;
        } else {
            break;
        }
    }

    
    // we get here when there was no keepalive
    fprintf(stderr,"no keepalive for rx %d\n", rx->id);

    rx->watchdog_thread_id=-1;
    cleanup_receiver(rx->id);
}

void* audio_thread(void* arg) {
    RECEIVER *rx=(RECEIVER*)arg;
    struct sockaddr_in audio;
    int audio_length;
    int old_state, old_type;

fprintf(stderr,"audio_thread port=%d\n",audio_port+(rx->id*2));

    pthread_setcancelstate(PTHREAD_CANCEL_ENABLE,&old_state);
    pthread_setcanceltype(PTHREAD_CANCEL_ASYNCHRONOUS,&old_type);


    rx->audio_socket=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(rx->audio_socket<0) {
        perror("create socket failed for server audio socket");
        exit(1);
    }

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
        bytes_read=recvfrom(rx->audio_socket,rx->output_buffer,sizeof(rx->output_buffer),0,(struct sockaddr*)&audio,&audio_length);
        if(bytes_read<0) {
            perror("recvfrom socket failed for audio buffer");
            exit(1);
        }
        rx->output_buffer_valid=1;

//fprintf(stderr,"audio bytes read=%d expected=%d\n",bytes_read,sizeof(rx->output_buffer));
        //process_ozy_output_buffer(rx->output_buffer,&rx->output_buffer[BUFFER_SIZE]);

    }
}

/*
 *    Command:                     Response:
 *
 *    connect receiver port        -- OK sampleRate
 *                                 -- ERROR message
 *    
 *    disconnect                   -- OK
 *                                 -- ERROR message
 *
 *    set frequency                -- OK setfrequency session
 *                                 -- ERROR setfrequency message
 *
 *    keepalive                    -- OK keepalive
 *                                 -- ERROR keepalive message
 *
 */

void parse_connect() {
    char *token;
    int rx;
    int spectrum_port;
    int rc;

    token=strtok(NULL," ");
    if(token!=NULL) {
        rx=atoi(token);
        token=strtok(NULL," ");
        if(token!=NULL) {
            spectrum_port=atoi(token);
            if(rx>=0 && rx<receivers) {
                if(receiver[rx].address==-1L) {
                    // initialize receiver
                    receiver[rx].address=client.sin_addr.s_addr;
                    receiver[rx].port=client.sin_port;
                    receiver[rx].spectrum_port=spectrum_port;

                    // start a thread to receive audio
                    rc=pthread_create(&receiver[rx].audio_thread_id,NULL,audio_thread,&receiver[rx]);
                    if(rc != 0) {
                        fprintf(stderr,"failed to create audio thread for rx %d\n",rx);
                    }

               

                    // start a thread to monitor keepalive
                    receiver[rx].keepalive=0;
                    rc=pthread_create(&receiver[rx].watchdog_thread_id,NULL,watchdog_thread,&receiver[rx]);
                    if(rc != 0) {
                        fprintf(stderr,"failed to create watchdog thread for rx %d\n",rx);
                    }

                    sprintf(response,"OK %d",sampleRate);

fprintf(stderr,"new client connected %s:%d to rx %d\n",inet_ntoa(client.sin_addr),client.sin_port,rx);

                } else if(receiver[rx].address==client.sin_addr.s_addr && receiver[rx].port==client.sin_port) {
                    sprintf(response,"OK %d",sampleRate);
fprintf(stderr,"existing client connected to rx %d\n",rx);
                } else {
                    sprintf(response,"%s","ERROR rx in use");
                }
            } else {
                sprintf(response,"%s","ERROR invalid rx");
            }
        } else {
            sprintf(response,"%s","ERROR expected spectrum_port");
        }
    } else {
        sprintf(response,"%s","ERROR expected rx");
    }
}

void parse_disconnect() {
    char *token;
    int rx;
    long f;

    token=strtok(NULL," ");
    if(token!=NULL) {
        rx=atoi(token);
        if(rx>=0 && rx<receivers) {
            if(receiver[rx].address==client.sin_addr.s_addr && receiver[rx].port==client.sin_port) {
               // clean up receiver
               cleanup_receiver(rx);
               sprintf(response,"OK");
fprintf(stderr,"client disconnected from rx %d\n",rx);
            } else {
                sprintf(response,"%s","ERROR not owner");
            }
        } else {
            sprintf(response,"%s","ERROR invalid rx");
        }
    } else {
        sprintf(response,"%s","ERROR expected rx");
    }
}

void parse_getfrequency() {
}

void parse_getreceivers() {
}

void parse_getsamplerate() {
}

void parse_getpreamp() {
}

void parse_get() {
}

void parse_setfrequency() {
    char *token;
    int rx;
    long f;

    token=strtok(NULL," ");
    if(token!=NULL) {
        rx=atoi(token);
        if(rx>=0 && rx<receivers) {
            if(receiver[rx].address==client.sin_addr.s_addr && receiver[rx].port==client.sin_port) {
                token=strtok(NULL," ");
                if(token!=NULL) {
                    f=atol(token);
                    receiver[rx].frequency=f;
                    receiver[rx].frequency_changed=1;
                    sprintf(response,"OK");
                } else {
                    sprintf(response,"%s","ERROR expected frequency");
                }
            } else {
                sprintf(response,"%s","ERROR not owner");
            }
        } else {
            sprintf(response,"%s","ERROR invalid receiver");
        }
    } else {
        sprintf(response,"%s","ERROR expected receiver");
    }
}

void parse_set() {
    char *token;
    token=strtok(NULL," ");
    if(token!=NULL) {
        if(strcmp(token,"frequency")==0) {
            parse_setfrequency();
        } else {
            sprintf(response,"ERROR invalid set command: %s",token);
        }
    } else {
        sprintf(response,"ERROR empty set command string");
    }
}

void parse_keepalive() {
    char *token;
    int rx;

    token=strtok(NULL," ");
    if(token!=NULL) {
        rx=atoi(token);
        if(rx>=0 && rx<receivers) {
            if(receiver[rx].address==client.sin_addr.s_addr && receiver[rx].port==client.sin_port) {
                receiver[rx].keepalive++;
            } else {
                sprintf(response,"ERROR not owner");
            }
        } else {
            sprintf(response,"ERROR empty invalid receiver");
        }
    } else {
        sprintf(response,"ERROR empty keepalive command string");
    }
}

void parse_command(char *command) {
    char *token;
    int result;

    if(debug) {
        fprintf(stderr,"parse_command: '%s'\n",command);
    }
    result=0;
    token=strtok(command," ");
    if(token) {
        if(strcmp(token,"connect")==0) {
            parse_connect();
        } else if(strcmp(token,"disconnect")==0) {
            parse_disconnect();
        } else if(strcmp(token,"get")==0) {
            parse_get();
        } else if(strcmp(token,"set")==0) {
            parse_set();
        } else if(strcmp(token,"keepalive")==0) {
            parse_keepalive();
        } else {
            sprintf(response,"ERROR invalid command: %s",token);
        }
    } else {
        sprintf(response,"ERROR empty command string");
    }
}

void listener() {
    int s;
    struct sockaddr_in command;
    int command_length;
    char buffer[BUFFER_SIZE];
    int i;

    for(i=0;i<receivers;i++) {
        receiver[i].id=i;
        receiver[i].address=-1L;
        receiver[i].port=-1;
        receiver[i].output_buffer_valid=0;
    }

    s=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(s<0) {
        perror("create socket failed for server command socket");
        exit(1);
    }

    command_length=sizeof(command);
    memset(&command,0,command_length);
    command.sin_family=AF_INET;
    command.sin_addr.s_addr=htonl(INADDR_ANY);
    command.sin_port=htons(command_port);

    if(bind(s,(struct sockaddr*)&command,command_length)<0) {
        perror("bind socket failed for server command socket");
        exit(1);
    }

    fprintf(stderr,"listening for commands on port %d\n",command_port);

    while(listening) {

        // get a message from a client
        bytes_read=recvfrom(s,buffer,BUFFER_SIZE-1,0,(struct sockaddr*)&client,&client_length);
        if(bytes_read<0) {
            perror("recvfrom socket failed for command buffer");
            exit(1);
        }
        buffer[bytes_read]=0;

//        if(debug) {
            fprintf(stderr,"message received from %s:%d '%s'\n",inet_ntoa(client.sin_addr),client.sin_port,buffer);
//        }

        // process the message
        parse_command(buffer);

//        if(debug) {
            fprintf(stderr,"response '%s'\n",response);
//        }

        // send the response
//fprintf(stderr,"response sendto %d\n",ntohs(client.sin_port));
        if(sendto(s,response,strlen(response),0,(struct sockaddr *)&client,sizeof(client))!=strlen(response)) {
            perror("sednto failed sending command response\n");
            exit(1);
        }
    }
}

