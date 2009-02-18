/* 
 * File:   libusbio.h
 * Author: jm57878
 *
 * Created on 18 February 2009, 21:55
 */

#ifndef _LIBUSBIO_H
#define	_LIBUSBIO_H

#ifdef	__cplusplus
extern "C" {
#endif

int libusb_open_ozy(void);
int libusb_write_ozy(int ep,void* buffer,int buffersize);
int libusb_read_ozy(int ep,void* buffer,int buffersize);


#ifdef	__cplusplus
}
#endif

#endif	/* _LIBUSBIO_H */

