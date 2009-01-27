/* 
 * File:   command-thread.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 16 December 2008, 18:04
 */

#include <stdio.h>
#include <stdlib.h>

/*
 * 
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>

#include "command_thread.h"
#include "ozy.h"
#include "ozy_to_jack.h"
#include "global.h"



int command_port;

pthread_t CommandThreadID;

void setMOX(int state) {
    if(debug_commands) fprintf(stderr,"setMOX: %02X\n",state);
    control_out[0]=control_out[0]&0xFE;
    control_out[0]=control_out[0]|(state&0x01);
    if(debug) fprintf(stderr,"setMOX: %02X %02X %02X %02X %02X\n",
                        control_out[0],
                        control_out[1],
                        control_out[2],
                        control_out[3],
                        control_out[4]);

    mox_state_changed=1;
}

void setSpeed(int speed) {
    if(debug_commands) fprintf(stderr,"setSpeed: %02X\n",speed);
    control_out[1]=control_out[1]&0xFC;
    control_out[1]=control_out[1]|speed;
    if(speed==SPEED_48KHZ) {
        output_sample_increment=1;
    } else if(speed==SPEED_96KHZ) {
        output_sample_increment=2;
    } else if(speed==SPEED_192KHZ) {
        output_sample_increment=4;
    }
    if(debug_commands) fprintf(stderr,"C1: %02X\n",control_out[1]);
}

void set10MHzSource(int source) {
    if(debug_commands) fprintf(stderr,"set10MHZSource: %02X\n",source);
    control_out[1]=control_out[1]&0xF3;
    control_out[1]=control_out[1]|(source<<2);
    if(debug_commands) fprintf(stderr,"C1: %02X\n",control_out[1]);
}

void set122MHzSource(int source) {
    if(debug_commands) fprintf(stderr,"set122MHZSource: %02X\n",source);
    control_out[1]=control_out[1]&0xEF;
    control_out[1]=control_out[1]|(source<<4);
    if(debug_commands) fprintf(stderr,"C1: %02X\n",control_out[1]);
}

void setConfig(int config) {
    if(debug_commands) fprintf(stderr,"setConfig: %02X\n",config);
    control_out[1]=control_out[1]&0x9F;
    control_out[1]=control_out[1]|(config<<5);
    if(debug_commands) fprintf(stderr,"C1: %02X\n",control_out[1]);
}


void setMicSource(int source) {
    if(debug_commands) fprintf(stderr,"setMicSource: %02X\n",source);
    control_out[1]=control_out[1]&0x7F;
    control_out[1]=control_out[1]|(source<<7);
    if(debug_commands) fprintf(stderr,"C1: %02X\n",control_out[1]);
}

void setMode(int mode) {
    if(debug_commands) fprintf(stderr,"setMode: %02X\n",mode);
    control_out[2]=control_out[2]&0xFE;
    control_out[2]=control_out[2]|mode;
    if(debug_commands) fprintf(stderr,"C2: %02X\n",control_out[2]);
}

void setOCOutputs(int outputs) {
    if(debug_commands) fprintf(stderr,"setOCOutputs: %02X\n",outputs);
    control_out[2]=control_out[2]&0x01;
    control_out[2]=control_out[2]|(outputs<<1);
    if(debug_commands) fprintf(stderr,"C2: %02X\n",control_out[2]);
}

void setAlexAttenuator(int attenuator) {
    if(debug_commands) fprintf(stderr,"setAlexAttenuator: %02X\n",attenuator);
    control_out[3]=control_out[3]&0xFC;
    control_out[3]=control_out[3]|attenuator;
    if(debug_commands) fprintf(stderr,"C3: %02X\n",control_out[3]);
}

void setPreampGain(int gain) {
    if(debug_commands) fprintf(stderr,"setPreampGain: %02X\n",gain);
    control_out[3]=control_out[3]&0xFB;
    control_out[3]=control_out[3]|(gain<<2);
    if(debug_commands) fprintf(stderr,"C3: %02X\n",control_out[3]);
}

void setLT2208Dither(int dither) {
    if(debug_commands) fprintf(stderr,"setLT2208Dither: %02X\n",dither);
    control_out[3]=control_out[3]&0xF7;
    control_out[3]=control_out[3]|(dither<<3);
    if(debug_commands) fprintf(stderr,"C3: %02X\n",control_out[3]);
}

void setLT2208Random(int random) {
    if(debug_commands) fprintf(stderr,"setLT2208Random: %02X\n",random);
    control_out[3]=control_out[3]&0xEF;
    control_out[3]=control_out[3]|(random<<4);
    if(debug_commands) fprintf(stderr,"C3: %02X\n",control_out[3]);
}

void setFrequency(double f) {
    if(debug_commands) fprintf(stderr,"setFrequency: %f\n",f);
    frequency=(int)((f*1000000.0)+0.5);
    frequency_changed=1;
}



void process_commands() {

    int command_socket;
    struct sockaddr_in commands;
    int commands_length;
    char buffer[128];

    if(debug_commands) fprintf(stderr,"process_commands\n");

    commands_length=sizeof(commands);

    if ((command_socket = socket(PF_INET, SOCK_DGRAM, IPPROTO_UDP)) < 0) {
        perror("Failed to create UDP socket for commands");
        exit(1);
    }

    memset((char *) &commands, 0, commands_length);
    commands.sin_family = AF_INET;
    commands.sin_addr.s_addr = htonl(INADDR_ANY);
    commands.sin_port = htons(command_port);

    if (bind(command_socket, (struct sockaddr *) &commands, commands_length) < 0) {
        perror("Failed to bind update UDP socket");
        exit(1);
    }


    int length;
    char* token;
    char* response;
    while((length=recvfrom(command_socket,buffer,sizeof(buffer),0,(struct sockaddr *)&commands,(socklen_t *)&commands_length))>=0) {
        buffer[length]=0;
        if(debug_commands) fprintf(stderr,"process_commands: received command from %s: %s\n",inet_ntoa(commands.sin_addr),buffer);
        response="ok";
        token=strtok(buffer," \r\n\0");
        if(!strcmp(token,"setMOX")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                int i=atoi(token);
                setMOX(i);
            }
        } else if(!strcmp(token,"setSpeed")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                int i=atoi(token);
                setSpeed(i);
            }
        } else if(!strcmp(token,"set10MHzSource")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                int i=atoi(token);
                set10MHzSource(i);
            }
        } else if(!strcmp(token,"set122.88MHzSource")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                int i=atoi(token);
                set122MHzSource(i);
            }
        } else if(!strcmp(token,"setConfig")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                int i=atoi(token);
                setConfig(i);
            }
        } else if(!strcmp(token,"setMicSource")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                int i=atoi(token);
                setMicSource(i);
            }
        } else if(!strcmp(token,"setMode")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                int i=atoi(token);
                setMode(i);
            }
        } else if(!strcmp(token,"setOCOutputs")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                int i=atoi(token);
                setOCOutputs(i);
            }
        } else if(!strcmp(token,"setAlexAttenuator")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                int i=atoi(token);
                setAlexAttenuator(i);
            }
        } else if(!strcmp(token,"setPreampGain")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                int i=atoi(token);
                setPreampGain(i);
            }
        } else if(!strcmp(token,"setLT2208Dither")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                int i=atoi(token);
                setLT2208Dither(i);
            }
        } else if(!strcmp(token,"setLT2208Random")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                int i=atoi(token);
                setLT2208Random(i);
            }
        } else if(!strcmp(token,"setFrequency")) {
            token=strtok(NULL," \r\n\0");
            if(token) {
                double f=atof(token);
                setFrequency(f);
            }
        } else {
            response="error";
        }

        if(sendto(command_socket,response,strlen(response),0,(struct sockaddr *)&commands,commands_length)!=strlen(response)) {
            perror("Failed to send command response");
            exit(1);
        }
    }

}

void * command_thread(void * args) {
    if(debug_commands) fprintf(stderr,"command_thread\n");
    command_port=DEFAULT_COMMAND_PORT;

    process_commands();
}

int create_command_processor() {
    int rc;

    if(debug_commands) fprintf(stderr,"create_command_processor\n");
    rc = pthread_create(&CommandThreadID, NULL,  command_thread, NULL);

    return rc;
}
