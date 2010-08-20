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

#include <errno.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
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
#include "client.h"
#include "ozy.h"
#include "audiostream.h"
#include "main.h"
#include "soundcard.h"
#include "dttsp.h"
#include "buffer.h"

static pthread_t client_thread_id;

static int client_terminate=0;

#define BASE_PORT 8000

static int port=BASE_PORT;

static int serverSocket;
static int clientSocket;
static struct sockaddr_in server;
static struct sockaddr_in client;
static socklen_t addrlen;

#define SAMPLE_BUFFER_SIZE 4096
static float spectrumBuffer[SAMPLE_BUFFER_SIZE];

static float meter;
static float subrx_meter;

static sem_t network_semaphore;

void* client_thread(void* arg);

void client_send_samples(int size);
void client_set_samples(float* samples,int size);

static unsigned char* client_samples;


float getFilterSizeCalibrationOffset() {
    int size=1024; // dspBufferSize
    float i=log10((float)size);
    return 3.0f*(11.0f-i);
}


void client_init(int receiver) {
    int rc;

    sem_init(&network_semaphore,0,1);

    signal(SIGPIPE, SIG_IGN);

    audio_buffer=malloc((audio_buffer_size*audio_channels)+BUFFER_HEADER_SIZE);

fprintf(stderr,"client_init audio_buffer_size=%d audio_buffer=%ld\n",audio_buffer_size,audio_buffer);

    port=BASE_PORT+receiver;
    clientSocket=-1;
    rc=pthread_create(&client_thread_id,NULL,client_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on client_thread: rc=%d\n", rc);
    }

}

void* client_thread(void* arg) {
    int rc;
    char *token;
    int i;
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

fprintf(stderr,"client_thread: listening on port %d\n",port);
    if (listen(serverSocket, 5) == -1) {
        perror("client listen");
        exit(1);
    }

    while(1) {
        addrlen = sizeof(client); 
	if ((clientSocket = accept(serverSocket,(struct sockaddr *)&client,&addrlen)) == -1) {
		perror("client accept");
	} else {

            time_t tt;
            struct tm *tod;
            time(&tt);
            tod=localtime(&tt);
            //fprintf(stdout,"wget -O - http://api.hostip.info/get_html.php?ip=%s\n",inet_ntoa(client.sin_addr));
            fflush(stdout);
            fprintf(stderr,"%02d/%02d/%02d %02d:%02d:%02d RX%d: client connection from %s:%d\n",tod->tm_mday,tod->tm_mon+1,tod->tm_year+1900,tod->tm_hour,tod->tm_min,tod->tm_sec,receiver,inet_ntoa(client.sin_addr),ntohs(client.sin_port));


            // set timeout on receive
            struct timeval tv;
            tv.tv_sec=3;
            tv.tv_usec=0;
            rc=setsockopt(clientSocket, SOL_SOCKET, SO_RCVTIMEO,(char *)&tv,sizeof tv);

            client_terminate=0;
            while(client_terminate==0) {
                bytesRead=recv(clientSocket, message, sizeof(message), 0);
                if(bytesRead==0) {
                    break;
                }
         
                if(bytesRead<0) {
                    if(errno!=EWOULDBLOCK) {
                        continue;
                    }
                    break;
                }

                message[bytesRead]=0;
                token=strtok(message," ");
                    if(token!=NULL) {
                    i=0;
                    while(token[i]!=0) {
                       token[i]=tolower(token[i]);
                       i++;
                    }
                    
                    if(strcmp(token,"getspectrum")==0) {
                        int samples;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            samples=atoi(token);
                            Process_Panadapter(0,spectrumBuffer);
                            meter=CalculateRXMeter(0,0,0)+multimeterCalibrationOffset+getFilterSizeCalibrationOffset();
                            subrx_meter=CalculateRXMeter(0,1,0)+multimeterCalibrationOffset+getFilterSizeCalibrationOffset();

fprintf(stderr,"meter=%f subrx_meter=%f\n",meter,subrx_meter);
                            client_samples=malloc(BUFFER_HEADER_SIZE+samples);
                            client_set_samples(spectrumBuffer,samples);
                            client_send_samples(samples);
                            free(client_samples);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"setfrequency")==0) {
                        long long frequency;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            frequency=atoll(token);
                            ozySetFrequency(frequency);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"setmode")==0) {
                        int mode;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            mode=atoi(token);
                            SetMode(0,0,mode);
                            SetMode(0,1,mode);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"setfilter")==0) {
                        int low,high;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            low=atoi(token);
                            token=strtok(NULL," ");
                            if(token!=NULL) {
                              high=atoi(token);
                              SetRXFilter(0,0,(double)low,(double)high);
                              SetRXFilter(0,1,(double)low,(double)high);
                            } else {
                                fprintf(stderr,"Invalid command: '%s'\n",message);
                            }
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"setagc")==0) {
                        int agc;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            agc=atoi(token);
                            SetRXAGC(0,0,agc);
                            SetRXAGC(0,1,agc);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"setnr")==0) {
                        int nr;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            if(strcmp(token,"true")==0) {
                                nr=1;
                            } else {
                                nr=0;
                            }
                            SetNR(0,0,nr);
                            SetNR(0,1,nr);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"setnb")==0) {
                        int nb;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            if(strcmp(token,"true")==0) {
                                nb=1;
                            } else {
                                nb=0;
                            }
                            SetNB(0,0,nb);
                            SetNB(0,1,nb);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"setanf")==0) {
                        int anf;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            if(strcmp(token,"true")==0) {
                                anf=1;
                            } else {
                                anf=0;
                            }
                            SetANF(0,0,anf);
                            SetANF(0,1,anf);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"setrxoutputgain")==0) {
                        int gain;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            gain=atoi(token);
                            SetRXOutputGain(0,0,(double)gain/100.0);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"setsubrxoutputgain")==0) {
                        int gain;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            gain=atoi(token);
                            SetRXOutputGain(0,1,(double)gain/100.0);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"startaudiostream")==0) {
                        token=strtok(NULL," ");
                        if(token==NULL) {
                            audio_buffer_size=480;
                        } else {
                            audio_buffer_size=atoi(token);
                        }
                        token=strtok(NULL," ");
                        if(token==NULL) {
                            audio_sample_rate=8000;
                        } else {
                            audio_sample_rate=atoi(token);
                            if(audio_sample_rate!=8000 &&
                               audio_sample_rate!=48000) {
                                fprintf(stderr,"Invalid audio sample rate: %d\n",audio_sample_rate);
                                audio_sample_rate=8000;
                            }
                        }
                        token=strtok(NULL," ");
                        if(token==NULL) {
                            audio_channels=1;
                        } else {
                            audio_channels=atoi(token);
                            if(audio_channels!=1 &&
                               audio_channels!=2) {
                                fprintf(stderr,"Invalid audio channels: %d\n",audio_channels);
                                audio_channels=1;
                            }
                        }
                        fprintf(stderr,"starting audio stream at %d with %d channels and buffer size %d\n",audio_sample_rate,audio_channels,audio_buffer_size);
                        free(audio_buffer);
                        audio_buffer=malloc((audio_buffer_size*audio_channels)+BUFFER_HEADER_SIZE);
                        audio_stream_reset();
                        send_audio=1;
                    } else if(strcmp(token,"stopaudiostream")==0) {
                        send_audio=0;
                    } else if(strcmp(token,"setsubrx")==0) {
                        int state;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            state=atoi(token);
                            SetSubRXSt(0,1,state);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"setsubrxfrequency")==0) {
                        int offset;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            offset=atoi(token);
                            SetRXOsc(0,1,offset);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"setpan")==0) {
                        float pan;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            pan=atof(token);
                            SetRXPan(0,0,pan);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else if(strcmp(token,"setsubrxpan")==0) {
                        float pan;
                        token=strtok(NULL," ");
                        if(token!=NULL) {
                            pan=atof(token);
                            SetRXPan(0,1,pan);
                        } else {
                            fprintf(stderr,"Invalid command: '%s'\n",message);
                        }
                    } else {
                        fprintf(stderr,"Invalid command: '%s'\n",message);
                    }
                } else {
                    fprintf(stderr,"Invalid command: '%s'\n",message);
                }
            }

            close(clientSocket);
            time(&tt);
            tod=localtime(&tt);
            fprintf(stderr,"%02d/%02d/%02d %02d:%02d:%02d RX%d: client disconnected from %s:%d\n",tod->tm_mday,tod->tm_mon+1,tod->tm_year+1900,tod->tm_hour,tod->tm_min,tod->tm_sec,receiver,inet_ntoa(client.sin_addr),ntohs(client.sin_port));
        }
        send_audio=0;
        clientSocket=-1;
//fprintf(stderr,"client disconnected send_audio=%d\n",send_audio);

    }
}

void client_send_samples(int size) {
    int rc;
    if(clientSocket!=-1) {
        sem_wait(&network_semaphore);
            rc=send(clientSocket,client_samples,size+BUFFER_HEADER_SIZE,MSG_NOSIGNAL);
            if(rc<0) {
                // perror("client_send_samples failed");
            }
        sem_post(&network_semaphore);
    } else {
        fprintf(stderr,"client_send_samples: clientSocket==-1\n");
    }
}

void client_send_audio() {
    int rc;

        if(clientSocket!=-1) {
            sem_wait(&network_semaphore);
                if(send_audio && (clientSocket!=-1)) {
                    rc=send(clientSocket,audio_buffer,(audio_buffer_size*audio_channels)+BUFFER_HEADER_SIZE,MSG_NOSIGNAL);
                    if(rc!=((audio_buffer_size*audio_channels)+BUFFER_HEADER_SIZE)) {
                        fprintf(stderr,"client_send_audio sent %d bytes",rc);
                    }
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


    // next 6 bytes contain the main rx s meter
    sprintf(&client_samples[14],"%d",(int)meter);

    // next 6 bytes contain the subrx s meter
    sprintf(&client_samples[20],"%d",(int)subrx_meter);

    // next 6 bytes contain data length
    sprintf(&client_samples[26],"%d",size);

    // next 8 bytes contain the sample rate
    sprintf(&client_samples[32],"%d",sampleRate);

    // next 8 bytes contain the meter - for compatability
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
        client_samples[i+BUFFER_HEADER_SIZE]=(unsigned char)-(max+displayCalibrationOffset+preampOffset);
    }

}
