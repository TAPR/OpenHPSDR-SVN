/* 
 * File:   ozy_buffers.h
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 05 January 2009, 16:27
 */

#ifndef _OZY_BUFFERS_H
#define	_OZY_BUFFERS_H

#ifdef	__cplusplus
extern "C" {
#endif

    #include <semaphore.h>

    #define OZY_BUFFER_SIZE 512

    struct ozy_buffer {
        struct ozy_buffer* next;
        int sequence;
        int size;
        unsigned char buffer[OZY_BUFFER_SIZE];
    };

    void put_ozy_input_buffer(struct ozy_buffer* buffer);
    struct ozy_buffer* get_ozy_input_buffer(void);

    struct ozy_buffer* new_ozy_buffer();
    void free_ozy_buffer(struct ozy_buffer* buffer);

    void create_ozy_buffers(int n);
    struct ozy_buffer* get_ozy_free_buffer(void);

    sem_t* ozy_input_buffer_sem;


#ifdef	__cplusplus
}
#endif

#endif	/* _OZY_BUFFERS_H */

