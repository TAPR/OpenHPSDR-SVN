/* 
 * File:   jack_ringbuffer.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 02 January 2009, 20:28
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "global.h"
#include "jack_ringbuffer.h"

/*
 * 
 */

struct jack_ringbuffer* jack_output_buffer;

pthread_mutex_t jack_output_buffer_mutex;

int jack_put_bytes=0;
int jack_get_bytes=0;

struct jack_ringbuffer* new_jack_ringbuffer(int n) {
    struct jack_ringbuffer* buffer;
    buffer=malloc(sizeof(struct jack_ringbuffer));
    if(buffer!=NULL) {
        buffer->size=n;
        buffer->entries=0;
        buffer->buffer_1=malloc(sizeof(float)*n);
        buffer->buffer_2=malloc(sizeof(float)*n);
        buffer->buffer_3=malloc(sizeof(float)*n);
        buffer->buffer_4=malloc(sizeof(float)*n);
        buffer->insert_index=0;
        buffer->remove_index=0;
    }
    return buffer;
}

int jack_ringbuffer_space(struct jack_ringbuffer* buffer) {
    return buffer->size-buffer->entries;
}

int jack_ringbuffer_entries(struct jack_ringbuffer* buffer) {
    return buffer->entries;
}

int jack_ringbuffer_put(float f1,float f2,float f3,float f4) {
    int i;

    pthread_mutex_lock(&jack_output_buffer_mutex);
    jack_put_bytes+=sizeof(float);
    if(jack_ringbuffer_space(jack_output_buffer)>1) {
        jack_output_buffer->buffer_1[jack_output_buffer->insert_index]=f1;
        jack_output_buffer->buffer_2[jack_output_buffer->insert_index]=f2;
        jack_output_buffer->buffer_3[jack_output_buffer->insert_index]=f3;
        jack_output_buffer->buffer_4[jack_output_buffer->insert_index]=f4;
        jack_output_buffer->entries++;
        jack_output_buffer->insert_index++;
        if(jack_output_buffer->insert_index>=jack_output_buffer->size) {
            jack_output_buffer->insert_index=0;
        }
        i=4;
    } else {
        i=0;
    }
    pthread_mutex_unlock(&jack_output_buffer_mutex);

    if(debug_buffers) fprintf(stderr,"jack_ringbuffer_put: insert_index=%d remove_index=%d entries=%d space=%d\n",jack_output_buffer->insert_index,jack_output_buffer->remove_index,jack_ringbuffer_entries(jack_output_buffer),jack_ringbuffer_space(jack_output_buffer));
    return i;
}

int jack_ringbuffer_get(float* f1,float* f2,float* f3,float*f4,int nframes,int buffer_count) {
    int entries;

    pthread_mutex_lock(&jack_output_buffer_mutex);
    entries=nframes;
    if(jack_output_buffer->entries<nframes) {
        entries=jack_output_buffer->entries;
        fprintf(stderr,"jack_ringbuffer_get: buffer=%d wanted %d got %d\n",buffer_count,nframes,entries);
    }
    
    jack_get_bytes+=entries*sizeof(float);

    if(debug_buffers) fprintf(stderr,"jack_ring_buffer_get space=%d entries=%d total=%d\n",jack_ringbuffer_space(jack_output_buffer),jack_ringbuffer_entries(jack_output_buffer),jack_get_bytes);
    if((jack_output_buffer->remove_index+entries)<=jack_output_buffer->size) {
        // all together in buffer
        memcpy(f1,&jack_output_buffer->buffer_1[jack_output_buffer->remove_index],entries*sizeof(float));
        memcpy(f2,&jack_output_buffer->buffer_2[jack_output_buffer->remove_index],entries*sizeof(float));
        memcpy(f3,&jack_output_buffer->buffer_3[jack_output_buffer->remove_index],entries*sizeof(float));
        memcpy(f4,&jack_output_buffer->buffer_4[jack_output_buffer->remove_index],entries*sizeof(float));
    } else {
        // split across of buffer
        memcpy(f1,&jack_output_buffer->buffer_1[jack_output_buffer->remove_index],(jack_output_buffer->size-jack_output_buffer->remove_index)*sizeof(float));
        memcpy(f2,&jack_output_buffer->buffer_2[jack_output_buffer->remove_index],(jack_output_buffer->size-jack_output_buffer->remove_index)*sizeof(float));
        memcpy(f3,&jack_output_buffer->buffer_3[jack_output_buffer->remove_index],(jack_output_buffer->size-jack_output_buffer->remove_index)*sizeof(float));
        memcpy(f4,&jack_output_buffer->buffer_4[jack_output_buffer->remove_index],(jack_output_buffer->size-jack_output_buffer->remove_index)*sizeof(float));
        memcpy(&f1[jack_output_buffer->size-jack_output_buffer->remove_index],jack_output_buffer->buffer_1,(entries-(jack_output_buffer->size-jack_output_buffer->remove_index))*sizeof(float));
        memcpy(&f2[jack_output_buffer->size-jack_output_buffer->remove_index],jack_output_buffer->buffer_2,(entries-(jack_output_buffer->size-jack_output_buffer->remove_index))*sizeof(float));
        memcpy(&f3[jack_output_buffer->size-jack_output_buffer->remove_index],jack_output_buffer->buffer_3,(entries-(jack_output_buffer->size-jack_output_buffer->remove_index))*sizeof(float));
        memcpy(&f4[jack_output_buffer->size-jack_output_buffer->remove_index],jack_output_buffer->buffer_4,(entries-(jack_output_buffer->size-jack_output_buffer->remove_index))*sizeof(float));
    }

    jack_output_buffer->entries-=entries;
    jack_output_buffer->remove_index+=entries;
    if(jack_output_buffer->remove_index>=jack_output_buffer->size) {
        jack_output_buffer->remove_index-=jack_output_buffer->size;
    }
    pthread_mutex_unlock(&jack_output_buffer_mutex);
    return entries;
}

int create_jack_ringbuffer(int n) {
    if(debug) fprintf(stderr,"create_jack_ringbuffer: entry: %d\n",n);
    pthread_mutex_init(&jack_output_buffer_mutex, NULL);
    jack_output_buffer=new_jack_ringbuffer(n);
    if(debug) fprintf(stderr,"create_jack_ringbuffer: exit\n");
}
