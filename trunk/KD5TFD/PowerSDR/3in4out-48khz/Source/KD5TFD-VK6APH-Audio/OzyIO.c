// Copyright (C) Bill Tracey (KD5TFD) 2006 
// email: bill@ewjt.com 


#include <stdio.h> 
#include "private.h"
#include <usb.h> 

#define OZY_PID (0x0007) 
#define OZY_VID (0xfffe) 

/* IO Timeout - in msecs */ 
#define OZY_IO_TIMEOUT (1000) 

// returns non zero if devp match vid and pid passed 
int doesDevMatchVIDPID(struct usb_device *devp, int vid, int pid) { 
	if ( devp->descriptor.bDescriptorType == USB_DT_DEVICE  && devp->descriptor.idVendor == vid &&  devp->descriptor.idProduct == pid )  {	
		return 1; 
	}
	return 0; 
} 


struct usb_device *findOzyFromDevice(struct usb_device *devp) { 
	int i; 
	struct usb_device *dp;
	if ( doesDevMatchVIDPID(devp, OZY_VID, OZY_PID) ) { 
		return devp; 
	}
	if ( devp->num_children != 0 ) { 
		for ( i = 0; i < devp->num_children; i++ ) { 
			dp = findOzyFromDevice(devp->children[i]); 
			if ( dp != NULL ) { 
				return dp; 
			}
		} 
	} 
	return NULL; 
} 


struct usb_device *findOzyFromDevices(struct usb_device *devp) { 
	struct usb_device *dp; 
	while ( devp != NULL ) { 
		dp = findOzyFromDevice(devp); 
		if ( dp != NULL ) { 
			return dp; 
		} 
		devp = devp->next; 
	} 
	return NULL; 
}



struct usb_device *findOzy(struct usb_bus *busp) { 
	struct usb_bus *bp; 
	struct usb_device *devp; 
	bp = busp; 
	while ( bp != NULL )  {
		if ( bp->root_dev != NULL ) { 
			if ( (devp = findOzyFromDevices(bp->root_dev)) != NULL ) { 
				return devp; 
			} 			
		}
		bp = bp->next; 
	} 
	return NULL; 
} 

struct OzyHandle *OzyOpen(void)
{
	struct OzyHandle *ozyh = NULL; 
	struct usb_bus *busp; 
	struct usb_device *ozydevp; 
	struct usb_dev_handle *ozyusbh; 
	int rc; 
	
	ozyh = malloc(sizeof(struct OzyHandle)); 
	if ( ozyh == NULL ) { 
		return NULL; 
	} 
	ozyh->devp = NULL; 
	ozyh->h = NULL; 


	usb_init(); 
	usb_find_busses(); 
	usb_find_devices(); 
	busp = usb_get_busses(); 

	ozydevp = findOzy(busp); 
	if ( ozydevp == NULL ) { 
		fprintf(stderr, "findOzy didn't!\n"); 
		free(ozyh); 
		return NULL; 
	} 
	
	/* let's see if we can open the device */ 
	ozyusbh = usb_open(ozydevp); 
	if ( ozyusbh == NULL ) { 
		fprintf(stderr, "open on ozy failed\n"); 
		free(ozyh); 
		return NULL; 
	} 

	/* claim the out interface */ 
	rc = usb_claim_interface(ozyusbh, 0); 
	if ( rc != 0 ) { 
		fprintf(stderr, "usb_claim_interface failed: rc=%d\n", rc); 
		free(ozyh); 
		return NULL; 		
	} 
	/* if we get here all is well - device is open and claimed */ 
	ozyh->devp = ozydevp; 
	ozyh->h = ozyusbh; 
	return ozyh; 
}

void OzyClose(struct OzyHandle *h)
{
	usb_release_interface(h->h, 0x0); 
	usb_close(h->h); 
	free(h); 
	return; 

}

///////////////////////////////////////////////////
// USB functions to send and receive bulk packets
int OzyBulkWrite(struct OzyHandle *h, int ep, void* buffer, int buffersize)
{
	int rc; 
	rc = usb_bulk_write(h->h, ep, (char *)buffer, buffersize, OZY_IO_TIMEOUT); 
	return rc; 	
}


int  OzyBulkRead(struct OzyHandle *h, int ep, void* buffer, int buffersize)
{ 
	int rc; 
	rc = usb_bulk_read(h->h, ep, (char *)buffer,  buffersize, OZY_IO_TIMEOUT);
	return rc; 
}
