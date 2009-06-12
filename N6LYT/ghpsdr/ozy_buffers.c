/** 
* @file ozy_buffers.c
* @brief Ozy buffer functions
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-01-05
*/

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

#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
#include <semaphore.h>
#include <pthread.h>
#include <sys/types.h>
#include <unistd.h>

#include "ozy_buffers.h"

/*
 * 
 */
//#define MAX_BUFFERS 32
//#define MAX_OUTPUT_BUFFERS 34

struct ozy_buffer* ozy_input_buffers_head = NULL;
struct ozy_buffer* ozy_input_buffers_tail = NULL;
sem_t* ozy_input_buffer_sem;
int ozy_input_sequence=0;
int ozy_input_buffers=0;

pthread_mutex_t ozy_input_buffer_mutex;

struct ozy_buffer* ozy_free_buffers_head = NULL;
struct ozy_buffer* ozy_free_buffers_tail = NULL;

pthread_mutex_t ozy_free_buffer_mutex;

void free_ozy_buffer(struct ozy_buffer* buffer);

/* --------------------------------------------------------------------------*/
/** 
* @brief Put Ozy free buffer
*/
void put_ozy_free_buffer(struct ozy_buffer* buffer) {

    pthread_mutex_lock(&ozy_free_buffer_mutex);
    if(ozy_free_buffers_tail==NULL) {
        ozy_free_buffers_head=ozy_free_buffers_tail=buffer;
    } else {
        ozy_free_buffers_tail->next=buffer;
        ozy_free_buffers_tail=buffer;
    }
    pthread_mutex_unlock(&ozy_free_buffer_mutex);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Get Ozy free buffer
*/
struct ozy_buffer* get_ozy_free_buffer(void) {
    struct ozy_buffer* buffer;
    if(ozy_free_buffers_head==NULL) {
        fprintf(stderr,"get_ozy_free_buffer: NULL\n");
        return NULL;
    }
    pthread_mutex_lock(&ozy_free_buffer_mutex);
    buffer=ozy_free_buffers_head;
    ozy_free_buffers_head=buffer->next;
    if(ozy_free_buffers_head==NULL) {
        ozy_free_buffers_tail=NULL;
    }
    buffer->size=OZY_BUFFER_SIZE;
    buffer->next = NULL;
    pthread_mutex_unlock(&ozy_free_buffer_mutex);
    return buffer;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Put Ozy input buffer
*/
void put_ozy_input_buffer(struct ozy_buffer* buffer) {

    pthread_mutex_lock(&ozy_input_buffer_mutex);
    buffer->sequence=ozy_input_sequence++;
    if(ozy_input_buffers_tail==NULL) {
        ozy_input_buffers_head=ozy_input_buffers_tail=buffer;
    } else {
        ozy_input_buffers_tail->next=buffer;
        ozy_input_buffers_tail=buffer;
    }
    pthread_mutex_unlock(&ozy_input_buffer_mutex);

}

/* --------------------------------------------------------------------------*/
/** 
* @brief Get Ozy input buffer
*/
struct ozy_buffer* get_ozy_input_buffer(void) {
    struct ozy_buffer* buffer;
    if(ozy_input_buffers_head==NULL) {
        fprintf(stderr,"get_ozy_input_buffer: NULL\n");
        return NULL;
    }
    pthread_mutex_lock(&ozy_input_buffer_mutex);
    buffer=ozy_input_buffers_head;
    ozy_input_buffers_head=buffer->next;
    if(ozy_input_buffers_head==NULL) {
        ozy_input_buffers_tail=NULL;
    }
    buffer->next = NULL;
    pthread_mutex_unlock(&ozy_input_buffer_mutex);
    return buffer;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief New Ozy Buffer
*/
struct ozy_buffer* new_ozy_buffer() {
    struct ozy_buffer* buffer;
    buffer=calloc(1,sizeof(struct ozy_buffer));
    buffer->next=NULL;
    buffer->size=OZY_BUFFER_SIZE;
    return buffer;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Create Ozy buffer
* 
* @param n
*/
void create_ozy_buffers(int n) {
    struct ozy_buffer* buffer;
    int i;
    char name[64];

//fprintf(stderr,"create_ozy_buffers: %d\n",n);
    pthread_mutex_init(&ozy_input_buffer_mutex, NULL);
    pthread_mutex_init(&ozy_free_buffer_mutex, NULL);
    sprintf(name,"input_sem.%d",getpid());
    ozy_input_buffer_sem=sem_open(name,O_CREAT|O_EXCL,0600,0);
    if(ozy_input_buffer_sem==SEM_FAILED) {
        perror(name);
        exit(1);
    }
   
//fprintf(stderr,"%s\n",name);
    for(i=0;i<n;i++) {
        buffer=new_ozy_buffer();
        put_ozy_free_buffer(buffer);
    }

}

/* --------------------------------------------------------------------------*/
/** 
* @brief Free Ozy buffer
*/
void free_ozy_buffer(struct ozy_buffer* buffer) {
    put_ozy_free_buffer(buffer);
}


