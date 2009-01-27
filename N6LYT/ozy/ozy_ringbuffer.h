/* 
 * File:   ozy_ringbuffer.h
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 05 January 2009, 09:04
 */

#ifndef _OZY_RINGBUFFER_H
#define	_OZY_RINGBUFFER_H

#ifdef	__cplusplus
extern "C" {
#endif

struct ozy_ringbuffer {
        int size;
        int entries;
        char* buffer;
        int insert_index;
        int remove_index;
    };

    extern struct ozy_ringbuffer* ozy_output_buffer;

    extern struct ozy_ringbuffer* new_ozy_ringbuffer(int n);
    extern int ozy_ringbuffer_put(struct ozy_ringbuffer* buffer,char* c,int n);
    extern int ozy_ringbuffer_get(struct ozy_ringbuffer* buffer,char* c,int n);


#ifdef	__cplusplus
}
#endif

#endif	/* _OZY_RINGBUFFER_H */

