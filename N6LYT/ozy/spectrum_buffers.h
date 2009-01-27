/* 
 * File:   spectrum_buffers.h
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 12 January 2009, 14:17
 */

#ifndef _SPECTRUM_BUFFERS_H
#define	_SPECTRUM_BUFFERS_H

#ifdef	__cplusplus
extern "C" {
#endif
    #include <semaphore.h>

    #define SPECTRUM_BUFFER_SIZE 4096

    struct spectrum_buffer {
        struct spectrum_buffer* next;
        int sequence;
        int size;
        float buffer[SPECTRUM_BUFFER_SIZE];
    };

    void put_spectrum_input_buffer(struct spectrum_buffer* buffer);
    struct spectrum_buffer* get_spectrum_input_buffer(void);

    struct spectrum_buffer* new_spectrum_buffer();
    void free_spectrum_buffer(struct spectrum_buffer* buffer);

    void create_spectrum_buffers(int n);
    struct spectrum_buffer* get_spectrum_free_buffer(void);

    sem_t spectrum_input_buffer_sem;

#ifdef	__cplusplus
}
#endif

#endif	/* _SPECTRUM_BUFFERS_H */

