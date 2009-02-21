/* 
 * File:   ozy_ringbuffer.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 05 January 2009, 09:07
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <semaphore.h>
#include <pthread.h>

#include "global.h"
#include "ozy_ringbuffer.h"

/*
 * 
 */

struct ozy_ringbuffer* ozy_output_buffer;

 
pthread_mutex_t ozy_output_buffer_mutex; 

int ozy_put_bytes=0;
int ozy_get_bytes=0;

struct ozy_ringbuffer* new_ozy_ringbuffer(int n) {
    struct ozy_ringbuffer* buffer;

    /*if(debug)*/ fprintf(stderr,"new_ozy_ringbuffer: %d\n",n);

    buffer=malloc(sizeof(struct ozy_ringbuffer));
    if(buffer!=NULL) {
        buffer->size=n;
        buffer->entries=0;
        buffer->buffer=malloc(sizeof(char)*n);
        buffer->insert_index=0;
        buffer->remove_index=0;
    }
    return buffer;
}

int ozy_ringbuffer_space(struct ozy_ringbuffer* buffer) {
    return buffer->size-buffer->entries;
}

int ozy_ringbuffer_entries(struct ozy_ringbuffer* buffer) {
    return buffer->entries;
}

int ozy_ringbuffer_put(struct ozy_ringbuffer* buffer,char* f,int n) {
    int i;
    ozy_put_bytes+=n;

    if(debug_buffers) fprintf(stderr,"ozy_ring_buffer_put n=%d\n",n);

        pthread_mutex_lock(&ozy_output_buffer_mutex);
        if(ozy_ringbuffer_space(buffer)<=n) {
            
            // flush the buffer
            buffer->remove_index=buffer->insert_index;
            buffer->entries=0;
        }

        if(ozy_ringbuffer_space(buffer)>n) {
            memcpy(&buffer->buffer[buffer->insert_index],f,n);
            buffer->entries+=n;
            buffer->insert_index+=n;
            if(buffer->insert_index>=buffer->size) {
                buffer->insert_index=0;
            }
            i=n;
        } else {
            fprintf(stderr,"ozy_ringbuffer_put: overflow space=%d entries=%d\n",ozy_ringbuffer_space(buffer),ozy_ringbuffer_entries(buffer));
            i=0;
        }
        pthread_mutex_unlock(&ozy_output_buffer_mutex);

        if(debug_buffers) fprintf(stderr,"ozy_ring_buffer_put space=%d entries=%d total=%d\n",ozy_ringbuffer_space(buffer),ozy_ringbuffer_entries(buffer),ozy_put_bytes);
        return i;
}

int ozy_ringbuffer_get(struct ozy_ringbuffer* buffer,char* f,int n) {
    int entries;

    pthread_mutex_lock(&ozy_output_buffer_mutex);
    entries=n;
    if(buffer->entries<n) entries=buffer->entries;
    
    ozy_get_bytes+=entries;

    if(debug_buffers) fprintf(stderr,"ozy_ring_buffer_get n=%d\n",n);
    if((buffer->remove_index+entries)<=buffer->size) {
        // all together
        memcpy(f,&buffer->buffer[buffer->remove_index],entries);
    } else {
        memcpy(f,&buffer->buffer[buffer->remove_index],buffer->size-buffer->remove_index);
        memcpy(&f[buffer->size-buffer->remove_index],buffer->buffer,entries-(buffer->size-buffer->remove_index));
    }
    
    buffer->entries-=entries;
    buffer->remove_index+=entries;
    if(buffer->remove_index>=buffer->size) {
        buffer->remove_index-=buffer->size;
    }
    pthread_mutex_unlock(&ozy_output_buffer_mutex);

    if(debug_buffers) fprintf(stderr,"ozy_ring_buffer_get space=%d entries=%d total=%d\n",ozy_ringbuffer_space(buffer),ozy_ringbuffer_entries(buffer),ozy_get_bytes);
    return entries;
}

int create_ozy_ringbuffer(int n) {
    pthread_mutex_init(&ozy_output_buffer_mutex, NULL);

    ozy_output_buffer=new_ozy_ringbuffer(n);
}
