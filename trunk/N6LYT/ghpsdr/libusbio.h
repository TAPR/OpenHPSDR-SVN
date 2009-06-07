/** 
* @file libusbio.h
* @brief Header file for the USB I/O functions, interface to libusb1.0
* @author 
* @version 0.1
* @date 2009-05-18
*/

#ifndef _LIBUSBIO_H
#define _LIBUSBIO_H

extern int libusb_open_ozy(void);
extern int libusb_write_ozy(int ep,void* buffer,int buffersize);
extern int libusb_read_ozy(int ep,void* buffer,int buffersize);

#endif /* _LIBUSBIO_H */

