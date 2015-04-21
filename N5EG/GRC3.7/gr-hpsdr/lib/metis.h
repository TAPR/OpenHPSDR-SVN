
// This version has been modified from the John Melton original 
// by Tom McDermott, N5EG for use with metis.cc and Gnuradio.
// Version - November 16, 2012

#ifndef METIS_H
#define METIS_H


enum {	RxStream_Off,		// Hermes Receiver Stream Controls
	RxStream_NB_On,		// Narrow Band (down converted)
	RxStream_WB_On,		// Wide Band (raw ADC samples)
	RxStream_NBWB_On	// Narrow Band and Wide Band both On
};	

typedef struct _METIS_CARD {
    char ip_address[16];
    char mac_address[18];
} METIS_CARD;

void metis_discover(const char* interface);
int metis_found();
char* metis_ip_address(int entry);
char* metis_mac_address(int entry);
void metis_receive_stream_control(unsigned char, unsigned int);
void metis_stop_receive_thread();

int metis_write(unsigned char ep,unsigned char* buffer,int length);
void* metis_receive_thread(void* arg);
void metis_send_buffer(unsigned char* buffer,int length);


#endif  // METIS_H


