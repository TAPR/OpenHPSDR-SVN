/* 
 * File:   ozyio.h
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 09 December 2008, 16:42
 */

#ifndef _OZYIO_H
#define	_OZYIO_H

#ifdef	__cplusplus
extern "C" {
#endif

// Ozy IO Routines
struct OzyHandle {
        struct usb_device *devp;
        struct usb_dev_handle *h;
};


int IsOzyAttached(void);
struct OzyHandle *OzyOpen(void);
void OzyClose(struct OzyHandle *h);
extern int OzyBulkWrite(struct OzyHandle *h, int ep, void* buffer, int buffersize);
extern int OzyBulkRead(struct OzyHandle *h, int ep, void* buffer, int buffersize);


#ifdef	__cplusplus
}
#endif

#endif	/* _OZYIO_H */

