/* 
 * File:   jack_buffers.h
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 05 January 2009, 16:25
 */

#ifndef _JACK_BUFFERS_H
#define	_JACK_BUFFERS_H

#ifdef	__cplusplus
extern "C" {
#endif

    #include <semaphore.h>

    #define JACK_SAMPLES_PER_BUFFER 1024

    struct jack_buffer {
        struct jack_buffer* next;
        int sequence;
        int nframes;
        float buffer_1[JACK_SAMPLES_PER_BUFFER];
        float buffer_2[JACK_SAMPLES_PER_BUFFER];
        float buffer_3[JACK_SAMPLES_PER_BUFFER];
        float buffer_4[JACK_SAMPLES_PER_BUFFER];
    };

    void put_jack_input_buffer(struct jack_buffer* buffer);
    struct jack_buffer* get_jack_input_buffer(void);

    struct jack_buffer* new_jack_buffer();
    void free_jack_buffer(struct jack_buffer* buffer);

    struct jack_buffer* get_jack_free_buffer();

    void create_jack_buffers(int n);

    sem_t jack_input_buffer_sem;

#ifdef	__cplusplus
}
#endif

#endif	/* _JACK_BUFFERS_H */

