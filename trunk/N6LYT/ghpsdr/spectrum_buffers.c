/* 
 * File:   spectrum_buffers.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 12 January 2009, 14:20
 */

#include <stdio.h>
#include <stdlib.h>
#include <semaphore.h>

#include "spectrum_buffers.h"
/*
 *
 */
//#define MAX_BUFFERS 32
//#define MAX_OUTPUT_BUFFERS 34

struct spectrum_buffer* spectrum_input_buffers_head;
struct spectrum_buffer* spectrum_input_buffers_tail;
sem_t* spectrum_input_buffer_sem;
int spectrum_input_sequence=0;
int spectrum_input_buffers=0;

pthread_mutex_t spectrum_input_buffer_mutex;

struct spectrum_buffer* spectrum_free_buffers_head;
struct spectrum_buffer* spectrum_free_buffers_tail;

pthread_mutex_t spectrum_free_buffer_mutex;

void free_spectrum_buffer(struct spectrum_buffer* buffer);

void put_spectrum_free_buffer(struct spectrum_buffer* buffer) {

    pthread_mutex_lock(&spectrum_free_buffer_mutex);
    if(spectrum_free_buffers_tail==NULL) {
        spectrum_free_buffers_head=spectrum_free_buffers_tail=buffer;
    } else {
        spectrum_free_buffers_tail->next=buffer;
        spectrum_free_buffers_tail=buffer;
    }
    pthread_mutex_unlock(&spectrum_free_buffer_mutex);
}

struct spectrum_buffer* get_spectrum_free_buffer(void) {
    struct spectrum_buffer* buffer;
    if(spectrum_free_buffers_head==NULL) {
        fprintf(stderr,"get_spectrum_free_buffer: NULL\n");
        return NULL;
    }
    pthread_mutex_lock(&spectrum_free_buffer_mutex);
    buffer=spectrum_free_buffers_head;
    spectrum_free_buffers_head=buffer->next;
    if(spectrum_free_buffers_head==NULL) {
        spectrum_free_buffers_tail=NULL;
    }
    buffer->size=SPECTRUM_BUFFER_SIZE;
    pthread_mutex_unlock(&spectrum_free_buffer_mutex);
    return buffer;
}

void put_spectrum_input_buffer(struct spectrum_buffer* buffer) {

    pthread_mutex_lock(&spectrum_input_buffer_mutex);
    buffer->sequence=spectrum_input_sequence++;
    if(spectrum_input_buffers_tail==NULL) {
        spectrum_input_buffers_head=spectrum_input_buffers_tail=buffer;
    } else {
        spectrum_input_buffers_tail->next=buffer;
        spectrum_input_buffers_tail=buffer;
    }
    pthread_mutex_unlock(&spectrum_input_buffer_mutex);
}

struct spectrum_buffer* get_spectrum_input_buffer(void) {
    struct spectrum_buffer* buffer;
    if(spectrum_input_buffers_head==NULL) {
        fprintf(stderr,"get_spectrum_input_buffer: NULL\n");
        return NULL;
    }
    pthread_mutex_lock(&spectrum_input_buffer_mutex);
    buffer=spectrum_input_buffers_head;
    spectrum_input_buffers_head=buffer->next;
    if(spectrum_input_buffers_head==NULL) {
        spectrum_input_buffers_tail=NULL;
    }
    pthread_mutex_unlock(&spectrum_input_buffer_mutex);
    return buffer;
}

struct spectrum_buffer* new_spectrum_buffer() {
    struct spectrum_buffer* buffer;
    buffer=malloc(sizeof(struct spectrum_buffer));
    buffer->next=NULL;
    buffer->size=SPECTRUM_BUFFER_SIZE;
    return buffer;
}

void create_spectrum_buffers(int n) {
    struct spectrum_buffer* buffer;
    int i;
    char name[64];
    
fprintf(stderr,"create_spectrum_buffers: %d\n",n);
    pthread_mutex_init(&spectrum_input_buffer_mutex, NULL);
    pthread_mutex_init(&spectrum_free_buffer_mutex, NULL);
    sprintf(name,"spectrum_sem.%d",getpid());
    spectrum_input_buffer_sem=sem_open(name,O_CREAT|O_EXCL,0600,0);
    if(spectrum_input_buffer_sem==SEM_FAILED) {
        perror(name);
        exit(1);
    }
fprintf(stderr,"%s\n",name);

    for(i=0;i<n;i++) {
        buffer=new_spectrum_buffer();
        put_spectrum_free_buffer(buffer);
    }

}

void free_spectrum_buffer(struct spectrum_buffer* buffer) {
    put_spectrum_free_buffer(buffer);
}



