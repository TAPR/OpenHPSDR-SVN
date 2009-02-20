/* 
 * File:   jack_ringbuffer.h
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 02 January 2009, 20:29
 */

#ifndef _JACK_RINGBUFFER_H
#define	_JACK_RINGBUFFER_H

#ifdef	__cplusplus
extern "C" {
#endif

    struct jack_ringbuffer {
        int size;
        int entries;
        float* buffer_1;
        float* buffer_2;
        float* buffer_3;
        float* buffer_4;
        int insert_index;
        int remove_index;
    };

    extern struct jack_ringbuffer* jack_output_buffer;

    extern int jack_ringbuffer_put(float f1,float f2,float f3,float f4);
    extern int jack_ringbuffer_get(float* f1,float* f2,float* f3,float* f4,int nframes,int buffer_count);


#ifdef	__cplusplus
}
#endif

#endif	/* _JACK_RINGBUFFER_H */

