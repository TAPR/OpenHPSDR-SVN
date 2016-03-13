#include "cmcomm.h"

/************************************************************************************************
*																								*
*							  VERSION FOR COMPLEX DOUBLE-PRECISION								*
*																								*
************************************************************************************************/

ringbuffer_t *
ringbuffer_create (size_t sz)
{
	int power_of_two;
	ringbuffer_t *rb;

	if ((rb = (ringbuffer_t *) malloc (sizeof (ringbuffer_t))) == NULL) {
		return NULL;
	}

	for (power_of_two = 1; (unsigned int)(1 << power_of_two) < sz; power_of_two++);

	rb->size = 1 << power_of_two;
	rb->size_mask = rb->size;
	rb->size_mask -= 1;
	rb->write_ptr = 0;
	rb->read_ptr = 0;
	if ((rb->buf = (double *) malloc (rb->size * sizeof(double))) == NULL) {
		free (rb);
		return NULL;
	}

	return rb;
}

void
ringbuffer_free (ringbuffer_t * rb)
{
	free (rb->buf);
	free (rb);
}

void
ringbuffer_reset_size (ringbuffer_t * rb, size_t sz)
{
    rb->size = sz;
    rb->size_mask = rb->size;
    rb->size_mask -= 1;
    rb->read_ptr = 0;
    rb->write_ptr = 0;
}

void
ringbuffer_reset (ringbuffer_t * rb)
{
    rb->read_ptr = 0;
    rb->write_ptr = 0;
	memset(rb->buf, 0, rb->size);
}

void
ringbuffer_clear (ringbuffer_t * rb, size_t sz)
{
	double *zero = (double *) malloc (sz * sizeof(double));

	memset(zero, 0, sz * sizeof(double));
	ringbuffer_write(rb, zero, sz);
	free(zero);
}

void
ringbuffer_restart (ringbuffer_t * rb, size_t sz)
{
	ringbuffer_reset (rb);
    ringbuffer_clear(rb, sz);
}

size_t
ringbuffer_read_space (const ringbuffer_t * rb)
{
	size_t w, r;

	w = rb->write_ptr;
	r = rb->read_ptr;

	if (w > r) {
		return w - r;
	} else {
		return (w - r + rb->size) & rb->size_mask;
	}
}

size_t
ringbuffer_write_space (const ringbuffer_t * rb)
{
	size_t w, r;

	w = rb->write_ptr;
	r = rb->read_ptr;

	if (w > r) {
		return ((r - w + rb->size) & rb->size_mask) - 1;
	} else if (w < r) {
		return (r - w) - 1;
	} else {
		return rb->size - 1;
	}
}

size_t
ringbuffer_write (ringbuffer_t * rb, const double *src, size_t cnt)
{
	size_t free_cnt;
	size_t cnt2;
	size_t to_write;
	size_t n1, n2;

	if ((free_cnt = ringbuffer_write_space (rb)) == 0) {
		return 0;
	}

	to_write = cnt > free_cnt ? free_cnt : cnt;

	cnt2 = rb->write_ptr + to_write;

	if (cnt2 > rb->size) {
		n1 = rb->size - rb->write_ptr;
		n2 = cnt2 & rb->size_mask;
	} else {
		n1 = to_write;
		n2 = 0;
	}

    memcpy (&(rb->buf[rb->write_ptr]), src, n1 * sizeof(double));
	rb->write_ptr = (rb->write_ptr + n1) & rb->size_mask;

	if (n2) {
		memcpy (&(rb->buf[rb->write_ptr]), src + n1, n2 * sizeof(double));
		rb->write_ptr = (rb->write_ptr + n2) & rb->size_mask;
	}

	return to_write;
}

size_t
ringbuffer_read (ringbuffer_t * rb, double *dest, size_t cnt)
{
	size_t free_cnt;
	size_t cnt2;
	size_t to_read;
	size_t n1, n2;

	if ((free_cnt = ringbuffer_read_space (rb)) == 0) {
		return 0;
	}

	to_read = cnt > free_cnt ? free_cnt : cnt;
	cnt2 = rb->read_ptr + to_read;

	if (cnt2 > rb->size) {
		n1 = rb->size - rb->read_ptr;
		n2 = cnt2 & rb->size_mask;
	} else {
		n1 = to_read;
		n2 = 0;
	}

	memcpy (dest, &(rb->buf[rb->read_ptr]), n1 * sizeof(double));
	rb->read_ptr = (rb->read_ptr + n1) & rb->size_mask;

	if (n2) {
		memcpy (dest + n1, &(rb->buf[rb->read_ptr]), n2 * sizeof(double));
		rb->read_ptr = (rb->read_ptr + n2) & rb->size_mask;
	}

	return to_read;
}
