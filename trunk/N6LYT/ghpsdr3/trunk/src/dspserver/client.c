/** 
* @file client.c
* @brief client network interface
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-12
*/
// client.c

/* Copyright (C) 
* 2009 - John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
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
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <signal.h>
#include <string.h>
#include <pthread.h>
#include <semaphore.h>
#include <math.h>
#include <time.h>
#include "ozy.h"
#include "audiostream.h"
#include "main.h"
#include "soundcard.h"
#include "dttsp.h"
#include "buffer.h"

static pthread_t client_thread_id;

#define BASE_PORT 8000

int port=BASE_PORT;

int serverSocket;
int clientSocket;
struct sockaddr_in server;
struct sockaddr_in client;
socklen_t addrlen;

#define SAMPLE_BUFFER_SIZE 4096
float spectrumBuffer[SAMPLE_BUFFER_SIZE];

float meter;
int smeter;

static sem_t network_semaphore;

void* client_thread(void* arg);
void client_send_samples(int size);
void client_set_samples(float* samples,int size);

#define PREFIX 48
unsigned char* client_samples;

int rejectAddress(char* address) {
    int result=0;

    // ports scanners from China
    if(strcmp(address,"222.208.183.218")==0) result=1;
    if(strcmp(address,"221.195.73.68")==0) result=1;
    if(strcmp(address,"61.183.15.9")==0) result=1;

    return result;
}

float getFilterSizeCalibrationOffset() {
    int size=1024; // dspBufferSize
    float i=log10((float)size);
    return 3.0f*(11.0f-i);
}


void client_init(int receiver) {
    int rc;

    sem_init(&network_semaphore,0,1);

    signal(SIGPIPE, SIG_IGN);

    port=BASE_PORT+(receiver*2);
    clientSocket=-1;
    rc=pthread_create(&client_thread_id,NULL,client_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on client_thread: rc=%d\n", rc);
    }

}

void* client_thread(void* arg) {

    int bytesRead;
    char message[32];
    int on=1;

fprintf(stderr,"client_thread\n");

    serverSocket=socket(AF_INET,SOCK_STREAM,0);
    if(serverSocket==-1) {
        perror("client socket");
        return NULL;
    }

    setsockopt(serverSocket, SOL_SOCKET, SO_REUSEADDR, &on, sizeof(on));

    memset(&server,0,sizeof(server));
    server.sin_family=AF_INET;
    server.sin_addr.s_addr=INADDR_ANY;
    server.sin_port=htons(port);

    if(bind(serverSocket,(struct sockaddr *)&server,sizeof(server))<0) {
        perror("client bind");
        return NULL;
    }

    while(1) {
        if (listen(serverSocket, 5) == -1) {
            perror("client listen");
            break;
        }

        addrlen = sizeof(client); 
	if ((clientSocket = accept(serverSocket,(struct sockaddr *)&client,&addrlen)) == -1) {
		perror("client accept");
	} else {

            time_t tt;
            struct tm *tod;
            time(&tt);
            tod=localtime(&tt);
            fprintf(stdout,"wget -O - http://api.hostip.info/get_html.php?ip=%s\n",inet_ntoa(client.sin_addr));
            fflush(stdout);
            fprintf(stderr,"%02d/%02d/%02d %02d:%02d:%02d client connection from %s:%d\n",tod->tm_mday,tod->tm_mon+1,tod->tm_year+1900,tod->tm_hour,tod->tm_min,tod->tm_sec,inet_ntoa(client.sin_addr),ntohs(client.sin_port));

            if(rejectAddress(inet_ntoa(client.sin_addr))) {
                fprintf(stderr,"connection rejected!\n");
            } else {
                char *token;
                while(1) {
                    bytesRead=recv(clientSocket, message, sizeof(message), 0);
                    if(bytesRead<=0) {
                        //perror("client recv");
                        break;
                    }
                    message[bytesRead]=0;
//fprintf(stderr,"client message: %s\n",message);

                    token=strtok(message," ");
                    if(token!=NULL) {
                        if(strcmp(token,"getSpectrum")==0) {
                            int samples;
                            token=strtok(NULL," ");
                            samples=atoi(token);
                            Process_Panadapter(0,spectrumBuffer);
                            meter=CalculateRXMeter(0,0,0)+multimeterCalibrationOffset+getFilterSizeCalibrationOffset();
                            client_samples=malloc(PREFIX+samples);
                            client_set_samples(spectrumBuffer,samples);
                            client_send_samples(samples);
                            free(client_samples);
                        } else if(strcmp(token,"setFrequency")==0) {
                            long long frequency;
                            token=strtok(NULL," ");
                            frequency=atoll(token);
                            ozySetFrequency(frequency);
                        } else if(strcmp(token,"setMode")==0) {
                            int mode;
                            token=strtok(NULL," ");
                            mode=atoi(token);
                            SetMode(0,0,mode);
                        } else if(strcmp(token,"setFilter")==0) {
                            int low,high;
                            token=strtok(NULL," ");
                            low=atoi(token);
                            token=strtok(NULL," ");
                            high=atoi(token);
                            SetRXFilter(0,0,(double)low,(double)high);
                        } else {
                        }
                    } else {
                    }
                }
            }

            close(clientSocket);
            time(&tt);
            tod=localtime(&tt);
            fprintf(stderr,"%02d/%02d/%02d %02d:%02d:%02d client disconnected from %s:%d\n",tod->tm_mday,tod->tm_mon+1,tod->tm_year+1900,tod->tm_hour,tod->tm_min,tod->tm_sec,inet_ntoa(client.sin_addr),ntohs(client.sin_port));
        }
        clientSocket=-1;
    }
}

void client_send_samples(int size) {
    int rc;
    if(clientSocket!=-1) {
        sem_wait(&network_semaphore);
            rc=send(clientSocket,client_samples,size+PREFIX,0);
            if(rc<0) {
                // perror("client_send_samples failed");
            }
        sem_post(&network_semaphore);
    } else {
        fprintf(stderr,"client_send_samples: clientSocket==-1\n");
    }
}

void client_send_audio(int size) {
    int rc;
    if(clientSocket!=-1) {
        sem_wait(&network_semaphore);
            rc=send(clientSocket,audio_stream_buffer,size+PREFIX,0);
            if(rc<0) {
                //perror("client_send_audio failed");
            }
        sem_post(&network_semaphore);
    } else {
        //fprintf(stderr,"client_send_audio: clientSocket==-1\n");
    }
}

void client_set_samples(float* samples,int size) {
    int i,j;
    float slope;
    float max;
    int lindex,rindex;

    // first byte is the buffer type
    client_samples[0]=SPECTRUM_BUFFER;

    // first 14 bytes contain the frequency
    //sprintf(client_samples,"% 4lld.%03lld.%03lld",frequencyA/1000000LL,(frequencyA%1000000LL)/1000LL,frequencyA%1000LL);

    // next 6 bytes contain the filter low
    //sprintf(&client_samples[14],"%d",filterLow);

    // next 6 bytes contain the filter high
    //sprintf(&client_samples[20],"%d",filterHigh);

    // next 6 bytes contain the mode
    //sprintf(&client_samples[26],"%s",modeToString());

    // next 8 bytes contain the sample rate
    sprintf(&client_samples[32],"%d",sampleRate);

    // next 8 bytes contain the meter
    sprintf(&client_samples[40],"%d",(int)meter);

    slope=(float)SAMPLE_BUFFER_SIZE/(float)size;
    for(i=0;i<size;i++) {
        max=-10000.0F;
        lindex=(int)floor((float)i*slope);
        rindex=(int)floor(((float)i*slope)+slope);
        if(rindex>SAMPLE_BUFFER_SIZE) rindex=SAMPLE_BUFFER_SIZE;
        for(j=lindex;j<rindex;j++) {
            if(samples[j]>max) max=samples[j];
        }
        client_samples[i+PREFIX]=(unsigned char)-(max+displayCalibrationOffset+preampOffset);
    }

}
