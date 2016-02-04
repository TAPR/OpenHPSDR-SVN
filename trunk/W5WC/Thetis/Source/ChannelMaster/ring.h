#ifndef _ringbuffer_h
#define _ringbuffer_h

typedef struct _ringbuffer {
    double	*buf;
    volatile size_t write_ptr;
    volatile size_t read_ptr;
    size_t	 size;
    size_t  size_mask;
}
ringbuffer_t ;

extern ringbuffer_t *ringbuffer_create(size_t sz);
extern size_t ringbuffer_read_space(const ringbuffer_t *rb);
extern size_t ringbuffer_write_space(const ringbuffer_t *rb);
extern void ringbuffer_free(ringbuffer_t *rb);
extern size_t ringbuffer_read(ringbuffer_t *rb, double *dest, size_t cnt);
extern size_t ringbuffer_write(ringbuffer_t *rb, const double *src, size_t cnt);
extern void ringbuffer_reset_size (ringbuffer_t * rb, size_t sz);
extern void ringbuffer_restart (ringbuffer_t * rb, size_t sz);
extern void ringbuffer_free(ringbuffer_t *rb);
extern void ringbuffer_reset(ringbuffer_t *rb);


#endif
