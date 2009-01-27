/* 
 * File:   spectrum_thread.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 12 January 2009, 14:33
 */

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <semaphore.h>
#include <string.h>

#include "global.h"
#include "spectrum_buffers.h"
#include "spectrum_thread.h"

pthread_t spectrum_thread_id;

int spectrum_port=19602;

void* spectrum_thread(void* arg) {
    struct spectrum_buffer* buffer;

    struct sockaddr_in clnt;
    int sock, clnt_len;

    if ((sock = socket(PF_INET, SOCK_DGRAM, IPPROTO_UDP)) < 0) {
        perror("Failed to create UDP socket for spectrum");
        return;
    }

    clnt_len = sizeof(clnt);
    memset((char *) &clnt, 0, clnt_len);
    clnt.sin_family = AF_INET;
    clnt.sin_addr.s_addr = htonl(INADDR_ANY);
    clnt.sin_port = htons(spectrum_port);

    while(1) {
        // wait for a spectrum buffer
        sem_wait(&spectrum_input_buffer_sem);
        if(debug_spectrum) fprintf(stderr,"spectrum_thread: get_spectrum_input_buffer\n");
        buffer=get_spectrum_input_buffer();
        if(buffer==NULL) {
            fprintf(stderr,"spectrum_thread: get_spectrum_buffer returned NULL!\n");
        } else {
            // write to spectrum port
            if (sendto(sock,buffer->buffer,SPECTRUM_BUFFER_SIZE*sizeof(float),0,(struct sockaddr *)&clnt,clnt_len)!=SPECTRUM_BUFFER_SIZE*sizeof(float)) {
                perror("Failed to send spectrum");
            }
            free_spectrum_buffer(buffer);
        }

    }
}

int create_spectrum_thread() {
    int rc;
    if(debug_spectrum) fprintf(stderr,"create_spectrum_thread\n");
    rc=pthread_create(&spectrum_thread_id,NULL,spectrum_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on spectrum_thread: rc=%d\n", rc);
    }
}
