/**
* @file ozy.c
* @brief Ozy protocol implementation
* @author John Melton, G0ORX/N6LYT
* @version 0.1
* @date 2009-10-13
*/


/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/

#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#ifdef __linux__
#include <arpa/inet.h>
#include <netdb.h>
#include <sys/timeb.h>
#include <pthread.h>
#include "ozyio.h"
#else
#include "pthread.h"
#endif

#include "client.h"
#include "ozy.h"
#include "bandscope.h"
#include "receiver.h"
#include "util.h"

#define THREAD_STACK 32768

#define OZY_BUFFERS 16
#define OZY_BUFFER_SIZE 512
#define OZY_HEADER_SIZE 8

#define SYNC 0x7F

// uncomment to compile code that allows for SYNC error recovery
#define RESYNC

// ozy command and control
#define MOX_DISABLED    0x00
#define MOX_ENABLED     0x01

#define MIC_SOURCE_JANUS 0x00
#define MIC_SOURCE_PENELOPE 0x80
#define CONFIG_NONE     0x00
#define CONFIG_PENELOPE 0x20
#define CONFIG_MERCURY  0x40
#define CONFIG_BOTH     0x60
#define PENELOPE_122_88MHZ_SOURCE 0x00
#define MERCURY_122_88MHZ_SOURCE  0x10
#define ATLAS_10MHZ_SOURCE        0x00
#define PENELOPE_10MHZ_SOURCE     0x04
#define MERCURY_10MHZ_SOURCE      0x08
#define SPEED_48KHZ               0x00
#define SPEED_96KHZ               0x01
#define SPEED_192KHZ              0x02

#define MODE_CLASS_E              0x01
#define MODE_OTHERS               0x00

#define ALEX_ATTENUATION_0DB      0x00
#define ALEX_ATTENUATION_10DB     0x01
#define ALEX_ATTENUATION_20DB     0x02
#define ALEX_ATTENUATION_30DB     0x03
#define LT2208_GAIN_OFF           0x00
#define LT2208_GAIN_ON            0x04
#define LT2208_DITHER_OFF         0x00
#define LT2208_DITHER_ON          0x08
#define LT2208_RANDOM_OFF         0x00
#define LT2208_RANDOM_ON          0x10

#define DUPLEX                    0x04

static pthread_t ep6_ep2_io_thread_id;
static pthread_t ep4_io_thread_id;

static int configure=6;
static int rx_frame=0;
static int tx_frame=0;
static int receivers=2;
static int current_receiver=0;

extern RECEIVER receiver[MAX_RECEIVERS];

static int speed=1;
static int sample_rate=96000;
static int output_sample_increment=2;

static int timing=0;
static struct timeb start_time;
static struct timeb end_time;
static int sample_count=0;

static unsigned char control_in[5]={0x00,0x00,0x00,0x00,0x00};
static unsigned char control_out[5] =
{
  MOX_DISABLED,
  CONFIG_MERCURY | MERCURY_122_88MHZ_SOURCE | MERCURY_10MHZ_SOURCE | MIC_SOURCE_PENELOPE | SPEED_96KHZ,
  MODE_OTHERS,
  ALEX_ATTENUATION_0DB | LT2208_GAIN_OFF | LT2208_DITHER_ON | LT2208_RANDOM_ON,
  DUPLEX
};

static int ptt=0;
static int dot=0;
static int dash=0;
static int lt2208ADCOverflow=0;

static char ozy_firmware_version[9];
static int mercury_software_version=0;
static int penelope_software_version=0;
static int ozy_software_version=0;

static int forward_power=0;

static int samples=0;

static float mic_gain=0.26F;
static float mic_left_buffer[BUFFER_SIZE];
static float mic_right_buffer[BUFFER_SIZE];

static struct sockaddr_in client;
static int client_length;

static int iq_socket;
static struct sockaddr_in iq_address;
static int iq_address_length;

static char ozy_firmware[64];
static char ozy_fpga[64];

static unsigned char ozy_output_buffer[OZY_BUFFER_SIZE];
static int ozy_output_buffer_index=OZY_HEADER_SIZE;

#ifdef RESYNC

static int left_sample,right_sample,mic_sample;
static float left_sample_float,right_sample_float,mic_sample_float;
static int rx,buffer_samples;

#define SYNC_0 0
#define SYNC_1 1
#define SYNC_2 2
#define CONTROL_0 3
#define CONTROL_1 4
#define CONTROL_2 5
#define CONTROL_3 6
#define CONTROL_4 7
#define LEFT_SAMPLE_0 8
#define LEFT_SAMPLE_1 9
#define LEFT_SAMPLE_2 10
#define RIGHT_SAMPLE_0 11
#define RIGHT_SAMPLE_1 12
#define RIGHT_SAMPLE_2 13
#define MIC_SAMPLE_0 14
#define MIC_SAMPLE_1 15

static int decode_state=SYNC_0;

void process_ozy_byte(char byte);
#endif

void* ozy_ep6_ep2_io_thread(void* arg);
void* ozy_ep4_io_thread(void* arg);

void process_ozy_input_buffer(char* buffer);
void write_ozy_output_buffer();
void process_bandscope_buffer(char* buffer);

#ifndef __linux__
#define bool int
bool init_hpsdr();
#endif

/**
 * Create threads--
 * - to read from EP6 and write to EP2,  ep6_ep2_io_thread
 * - to read from EP4 (currently, EP4 (wideband spectrum) data is not used, so this thread is not started)

 */
int create_ozy_thread() {
    int rc;

#ifndef __linux__
	if (init_hpsdr() == 0) exit(9);
#endif

    ozy_init();

    ftime(&start_time);

    // create a thread to read/write to EP6/EP2
    rc=pthread_create(&ep6_ep2_io_thread_id,NULL,ozy_ep6_ep2_io_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on ozy_ep6_io_thread: rc=%d\n", rc);
        exit(1);
    }

	//  For the moment, ignore EP4 
#if 0
    // create a thread to read from EP4
    rc=pthread_create(&ep4_io_thread_id,NULL,ozy_ep4_io_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on ozy_ep4_io_thread: rc=%d\n", rc);
        exit(1);
    }
#endif

    return 0;
}

/**
 * Set the number of receivers to be supported by this server
 * \param r The number of receivers: 1, 2, 3, or 4
 * This value (less one) is embedded into the command & control bytes that are sent to Mercury.
 * Thus it determines how the I & Q samples read from EP6 are placed in the data stream to dspservers.
 */
void ozy_set_receivers(int r) 
{
    if (r > MAX_RECEIVERS) 
	{
        fprintf(stderr,"MAX Receivers is 8!\n");
        exit(1);
    }
    receivers = r;
    control_out[4] &= 0xc7;
    control_out[4] |= (r - 1) << 3;
}

int ozy_get_receivers() 
{
    return receivers;
}

/**
 * Set the sample rate for I & Q data to be sent from Mercury to the PC via USB EP6
 * \param r One of 48000, 96000, or 192000
 * The value is used to set bits in the command & control bytes sent to Mercury.
 * These bits determine the decimation factor in Mercury's FPGA
 */
void ozy_set_sample_rate(int r) 
{
    switch(r) 
	{
        case 48000:
            sample_rate=r;
            speed=0;
            output_sample_increment = 1;
            break;
        case 96000:
            sample_rate=r;
            speed=1;
            output_sample_increment = 2;
            break;
        case 192000:
            sample_rate=r;
            speed=2;
            output_sample_increment = 4;
            break;
        default:
            fprintf(stderr,"Invalid sample rate (48000,96000,192000)!\n");
            exit(1);
            break;
    }
    control_out[1] &= 0xfc;
    control_out[1] |= speed;
}

int ozy_get_sample_rate() 
{
    return sample_rate;
}

/**
 * Initialize Ozy and set up the UDP socket thru which I & Q data will be sent to dspserver(s)
 * On Windows, first call init_hpsdr.
 * On Linux, all initialization happens here.
 * Several USB frames are sent to Ozy to ensure that it has all the correct command & control values
 * Then each receiver is given an initial frequency (currently all are set to 7.056 MHz),
 * and USB frames are again sent so that Ozy / Mercury have this frequency for the digital downconversion
 * performed in Mercury's FPGA.
 */
int ozy_init() 
{
    int rc = 0;
    int i;
	long int InitialFrequency[] = {3850000L, 7056000L, 14235000L, 21220000L};


    strcpy(ozy_firmware,"ozyfw-sdr1k.hex");
    strcpy(ozy_fpga,"Ozy_Janus.rbf");

    iq_socket=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(iq_socket<0) {
        perror("create socket failed for iq samples");
        exit(1);
    }

    iq_address_length=sizeof(iq_address);
    memset(&iq_address,0,iq_address_length);
    iq_address.sin_family=AF_INET;
    iq_address.sin_addr.s_addr=htonl(INADDR_ANY);
    iq_address.sin_port=htons(0);

    if(bind(iq_socket,(struct sockaddr*)&iq_address,iq_address_length)<0) {
        perror("bind socket failed for iq socket");
        exit(1);
    }

    // setup defaults
/*
    control_out[0] = MOX_DISABLED;
    control_out[1] = CONFIG_MERCURY | MERCURY_122_88MHZ_SOURCE | MERCURY_10MHZ_SOURCE | speed | MIC_SOURCE_PENELOPE;
    control_out[2] = MODE_OTHERS;
    control_out[3] = ALEX_ATTENUATION_0DB | LT2208_GAIN_OFF | LT2208_DITHER_ON | LT2208_RANDOM_ON;
    control_out[4] = DUPLEX | ((receivers-1)<<3);
*/

	// On Windows, the following is replaced by init_hpsdr() in OzyInit.c
#ifdef __linux__
    // open ozy
    rc = ozy_open();
    if (rc != 0) {
        fprintf(stderr,"Cannot locate Ozy\n");
        exit(1);
    }

    // load Ozy FW
    ozy_reset_cpu(1);
    ozy_load_firmware(ozy_firmware);
    ozy_reset_cpu(0);
ozy_close();
    sleep(5);
ozy_open();
    ozy_set_led(1,1);
    ozy_load_fpga(ozy_fpga);
    ozy_set_led(1,0);
    ozy_close();

    ozy_open();

    rc=ozy_get_firmware_string(ozy_firmware_version,8);
    fprintf(stderr,"Ozy FX2 version: %s\n",ozy_firmware_version);
#endif

    memset((char *)&ozy_output_buffer, 0, OZY_BUFFER_SIZE);
    while(configure > 0) 
	{
        write_ozy_output_buffer();
    }

    for(i = 0; i < receivers; i++) 
	{
        receiver[i].frequency = InitialFrequency[i]; //7056000L;
        receiver[i].frequency_changed = 1;
    }

    for(i = 0; i < receivers; i++) 
	{
        current_receiver=i;
        write_ozy_output_buffer();
    }

    current_receiver=0;

fprintf(stderr,"server configured for %d receivers at %d\n",receivers,sample_rate);
    return rc;
}

/**
 * "Forever" read blocks from Ozy's EP6 and call process_ozy_input_buffer().
 * OZY_BUFFERS determines how many (nominally 512 byte) buffers are to be read at once.
 * \param arg Void pointer to nothing, not used.
 */
void* ozy_ep6_ep2_io_thread(void* arg) 
{
    unsigned char input_buffer[OZY_BUFFER_SIZE*OZY_BUFFERS];
    int bytes;
    int i;

    while(1) 
	{
        // read an input buffer (blocks until all bytes read)
        bytes=ozy_read(0x86, input_buffer, OZY_BUFFER_SIZE*OZY_BUFFERS);
        if (bytes < 0) 
		{	// -22 EINVAL invalid argument
			// -116 ETIMEDOUT request timed out
			// -12 ENOMEM memory allocation error
			// -5 EIO general I/O error
			// -2 ENOENT no such file or directory

            fprintf(stderr, "ozy_ep6_ep2_io_thread: OzyBulkRead read failed %d\n", bytes);
        } else if (bytes != OZY_BUFFER_SIZE*OZY_BUFFERS) 
		{
            fprintf(stderr, "ozy_ep6_ep2_io_thread: OzyBulkRead only read %d bytes\n", bytes);
        } else 
		{
            // process input buffers
            for(i = 0; i < OZY_BUFFERS; i++) 
			{
                rx_frame++;
                process_ozy_input_buffer(&input_buffer[i*OZY_BUFFER_SIZE]);
            }
        }

        current_receiver++;

        if (current_receiver == receivers) 
		{
            current_receiver = 0;
        }
    }
}

/**
 * Copy command & control bytes to an output buffer & send to USB EP2.
 * Sample data headed to Ozy / Mercury is put into the buffer before calling this function.
 */
void write_ozy_output_buffer() {
    int bytes;

    ozy_output_buffer[0]=SYNC;
    ozy_output_buffer[1]=SYNC;
    ozy_output_buffer[2]=SYNC;

    if(configure>0) {
        configure--;
        ozy_output_buffer[3]=control_out[0];
        ozy_output_buffer[4]=control_out[1];
        ozy_output_buffer[5]=control_out[2];
        ozy_output_buffer[6]=control_out[3];
        ozy_output_buffer[7]=control_out[4];
    } else if(receiver[current_receiver].frequency_changed) {
        ozy_output_buffer[3]=control_out[0]|((current_receiver+2)<<1);
        ozy_output_buffer[4]=receiver[current_receiver].frequency>>24;
        ozy_output_buffer[5]=receiver[current_receiver].frequency>>16;
        ozy_output_buffer[6]=receiver[current_receiver].frequency>>8;
        ozy_output_buffer[7]=receiver[current_receiver].frequency;
        receiver[current_receiver].frequency_changed=0;
    } else {
        ozy_output_buffer[3]=control_out[0];
        ozy_output_buffer[4]=control_out[1];
        ozy_output_buffer[5]=control_out[2];
        ozy_output_buffer[6]=control_out[3];
        ozy_output_buffer[7]=control_out[4];
    }


    bytes=ozy_write(0x02,ozy_output_buffer,OZY_BUFFER_SIZE);
    if(bytes!=OZY_BUFFER_SIZE) {
        perror("OzyBulkWrite failed");
    }

if(tx_frame<10) {
    dump_ozy_buffer("sent to Ozy:",tx_frame,ozy_output_buffer);
}
    tx_frame++;

}

#ifdef RESYNC

/**
 * \param buffer Pointer to the data that was read from EP6.
 * Call process_ozy_byte() for each & every byte in the buffer.
 */
void process_ozy_input_buffer(char* buffer) {
    int i;
    for(i=0;i<OZY_BUFFER_SIZE;i++) {
        process_ozy_byte(buffer[i]);
    }
}

/**
 * A state machine that recognizes the SYNC SYNC SYNC signature indicating the logical beginning of
 * a block from Ozy.  
 * - Command & control bytes are saved in control_in[]
 * - 24-bit samples, alternating left & right, (I & Q) are converted to float and stored in buffers,
 *   one for each receiver.  The left samples are placed as a block at the beginning of the buffer,
 *   followed by the right samples.
 * - 16-bit microphone data (from janus or penelope ADC) are also converted to float
 *   and identical values are stored in both left_mic_buffer[] and right_mic_buffer[] -- but are not used.
 * - When the number of I & Q samples stored reaches BUFFER_SIZE, send_IQ_buffer() is called for each
 *   receiver.
 *
 */
void process_ozy_byte(char byte) {

    switch(decode_state) {
        case SYNC_0:
            if(byte==SYNC) decode_state=SYNC_1;
            break;
        case SYNC_1:
            if(byte==SYNC)
                decode_state=SYNC_2;
            else 
                decode_state=SYNC_0;
            break;
        case SYNC_2:
            if(byte==SYNC)
                decode_state=CONTROL_0;
            else 
                decode_state=SYNC_0;
            break;
        case CONTROL_0:
            rx=0;
            buffer_samples=0;
            control_in[0]=byte;
            decode_state=CONTROL_1;
            break;
        case CONTROL_1:
            control_in[1]=byte;
            decode_state=CONTROL_2;
            break;
        case CONTROL_2:
            control_in[2]=byte;
            decode_state=CONTROL_3;
            break;
        case CONTROL_3:
            control_in[3]=byte;
            decode_state=CONTROL_4;
            break;
        case CONTROL_4:
            control_in[4]=byte;

            // decode control bytes
            ptt=(control_in[0]&0x01)==0x01;
            dash=(control_in[0]&0x02)==0x02;
            dot=(control_in[0]&0x04)==0x04;
            if((control_in[0]&0x08)==0) {
                if(control_in[1]&0x01) {
                    lt2208ADCOverflow=1;
                }
                if(mercury_software_version!=control_in[2]) {
                    mercury_software_version=control_in[2];
                    fprintf(stderr,"Mercury Software version: %d (0x%0X)\n",mercury_software_version,mercury_software_version);
                }
                if(penelope_software_version!=control_in[3]) {
                    penelope_software_version=control_in[3];
                    fprintf(stderr,"Penelope Software version: %d (0x%0X)\n",penelope_software_version,penelope_software_version);
                }
                if(ozy_software_version!=control_in[4]) {
                    ozy_software_version=control_in[4];
                    fprintf(stderr,"Ozy Software version: %d (0x%0X)\n",ozy_software_version,ozy_software_version);
                }
            } else if(control_in[0]&0x08) {
                forward_power=(control_in[1]<<8)+control_in[2];
            }

            decode_state=LEFT_SAMPLE_0;
            break;
        case LEFT_SAMPLE_0:
            left_sample   = (int)byte << 16;
            decode_state=LEFT_SAMPLE_1;
            break;
        case LEFT_SAMPLE_1:
            left_sample  += (int)(unsigned char)byte << 8;
            decode_state=LEFT_SAMPLE_2;
            break;
        case LEFT_SAMPLE_2:
            left_sample  += (int)(unsigned char)byte;
            decode_state=RIGHT_SAMPLE_0;
            break;
        case RIGHT_SAMPLE_0:
            right_sample   = (int)byte << 16;
            decode_state=RIGHT_SAMPLE_1;
            break;
        case RIGHT_SAMPLE_1:
            right_sample  += (int)(unsigned char)byte << 8;
            decode_state=RIGHT_SAMPLE_2;
            break;
        case RIGHT_SAMPLE_2:
            right_sample  += (int)(unsigned char)byte;
            left_sample_float=(float)left_sample/8388607.0; // 24 bit sample
            right_sample_float=(float)right_sample/8388607.0; // 24 bit sample
            receiver[rx].input_buffer[samples]=left_sample_float;
            receiver[rx].input_buffer[samples+BUFFER_SIZE]=right_sample_float;
            rx++;
            if(rx==receivers) {
                rx=0;
                decode_state=MIC_SAMPLE_0;
            } else {
                decode_state=LEFT_SAMPLE_0;
            }
            break;
        case MIC_SAMPLE_0:
            mic_sample  = (int)byte << 8;
            decode_state=MIC_SAMPLE_1;
            break;
        case MIC_SAMPLE_1:
            mic_sample  += (int)(unsigned char)byte;
            mic_sample_float=(float)mic_sample/32767.0*mic_gain; // 16 bit sample
            mic_left_buffer[samples]=mic_sample_float;
            mic_right_buffer[samples]=0.0f;

            if(timing) {
                sample_count++;
                if(sample_count==sample_rate) {
                    ftime(&end_time);
                    fprintf(stderr,"%d samples in %ld ms\n",sample_count,((end_time.time*1000)+end_time.millitm)-((start_time.time*1000)+start_time.millitm));
                    sample_count=0;
                    ftime(&start_time);
                }
            }

            samples++;
            if(samples==BUFFER_SIZE) {
                int r;
                // send I/Q data to clients
                for(r=0;r<receivers;r++) {
                    send_IQ_buffer(r);
                }
                samples=0;
            }

            buffer_samples++;
            switch(receivers) {
                case 1:
                    if(buffer_samples==63) {
                        decode_state=SYNC_0;
                    } else {
                        decode_state=LEFT_SAMPLE_0;
                    }
                    break;
                case 2:
                    if(buffer_samples==36) {
                        decode_state=SYNC_0;
                    } else {
                        decode_state=LEFT_SAMPLE_0;
                    }
                    break;
                case 3:
                    if(buffer_samples==25) {
                        decode_state=SYNC_0;
                    } else {
                        decode_state=LEFT_SAMPLE_0;
                    }
                    break;
                case 4:
                    if(buffer_samples==19) {
                        decode_state=SYNC_0;
                    } else {
                        decode_state=LEFT_SAMPLE_0;
                    }
                    break;
                case 5:
                    if(buffer_samples==15) {
                        decode_state=SYNC_0;
                    } else {
                        decode_state=LEFT_SAMPLE_0;
                    }
                    break;
                case 6:
                    if(buffer_samples==13) {
                        decode_state=SYNC_0;
                    } else {
                        decode_state=LEFT_SAMPLE_0;
                    }
                    break;
                case 7:
                    if(buffer_samples==11) {
                        decode_state=SYNC_0;
                    } else {
                        decode_state=LEFT_SAMPLE_0;
                    }
                    break;
                case 8:
                    if(buffer_samples==10) {
                        decode_state=SYNC_0;
                    } else {
                        decode_state=LEFT_SAMPLE_0;
                    }
                    break;
            }
            break;
    }
}

#else
/**
 * Do the same processing as above, but without using a state machine.
 * This function requires that the first three bytes in the buffer are SYNC bytes.
 * If this is not the case, the buffer is printed, and the process is terminated.
 */
void process_ozy_input_buffer(char* buffer) {
    int b=0;
    int b_max;
    int r;
    int left_sample,right_sample,mic_sample;
    float left_sample_float,right_sample_float,mic_sample_float;
    int rc;
    int i;
    int bytes;

if(rx_frame<10) {
    dump_ozy_buffer("received from Ozy:",rx_frame,buffer);
}

    if(buffer[b++]==SYNC && buffer[b++]==SYNC && buffer[b++]==SYNC) {

        // extract control bytes
        control_in[0]=buffer[b++];
        control_in[1]=buffer[b++];
        control_in[2]=buffer[b++];
        control_in[3]=buffer[b++];
        control_in[4]=buffer[b++];

        // extract PTT, DOT and DASH
        ptt=(control_in[0]&0x01)==0x01;
        dash=(control_in[0]&0x02)==0x02;
        dot=(control_in[0]&0x04)==0x04;


        if((control_in[0]&0x08)==0) {
            if(control_in[1]&0x01) {
                lt2208ADCOverflow=1;
            }
            if(mercury_software_version!=control_in[2]) {
                mercury_software_version=control_in[2];
                fprintf(stderr,"Mercury Software version: %d (0x%0X)\n",mercury_software_version,mercury_software_version);
            }
            if(penelope_software_version!=control_in[3]) {
                penelope_software_version=control_in[3];
                fprintf(stderr,"Penelope Software version: %d (0x%0X)\n",penelope_software_version,penelope_software_version);
            }
            if(ozy_software_version!=control_in[4]) {
                ozy_software_version=control_in[4];
                fprintf(stderr,"Ozy Software version: %d (0x%0X)\n",ozy_software_version,ozy_software_version);
            }
        } else if(control_in[0]&0x08) {
            forward_power=(control_in[1]<<8)+control_in[2];
        }

        switch(receivers) {
            case 1: b_max=512-0; break;
            case 2: b_max=512-0; break;
            case 3: b_max=512-4; break;
            case 4: b_max=512-10; break;
            case 5: b_max=512-24; break;
            case 6: b_max=512-10; break;
            case 7: b_max=512-20; break;
            case 8: b_max=512-4; break;
        }

        // extract the samples
        while(b<b_max) {
            // extract each of the receivers
            for(r=0;r<receivers;r++) {
                left_sample   = (int)((signed char)buffer[b++]) << 16;
                left_sample  += (int)((unsigned char)buffer[b++]) << 8;
                left_sample  += (int)((unsigned char)buffer[b++]);
                right_sample  = (int)((signed char)buffer[b++]) << 16;
                right_sample += (int)((unsigned char)buffer[b++]) << 8;
                right_sample += (int)((unsigned char)buffer[b++]);
                left_sample_float=(float)left_sample/8388607.0; // 24 bit sample
                right_sample_float=(float)right_sample/8388607.0; // 24 bit sample
                receiver[r].input_buffer[samples]=left_sample_float;
                receiver[r].input_buffer[samples+BUFFER_SIZE]=right_sample_float;
            }
            mic_sample    = (int)((signed char) buffer[b++]) << 8;
            mic_sample   += (int)((unsigned char)buffer[b++]);
            mic_sample_float=(float)mic_sample/32767.0*mic_gain; // 16 bit sample

            // add to buffer
            mic_left_buffer[samples]=mic_sample_float;
            mic_right_buffer[samples]=0.0f;
            samples++;

            if(timing) {
                sample_count++;
                if(sample_count==sample_rate) {
                    ftime(&end_time);
                    fprintf(stderr,"%d samples in %ld ms\n",sample_count,((end_time.time*1000)+end_time.millitm)-((start_time.time*1000)+start_time.millitm));
                    sample_count=0;
                    ftime(&start_time);
                }
            }

            // when we have enough samples send them to the clients
            if(samples==BUFFER_SIZE) {
                // send I/Q data to clients
                for(r=0;r<receivers;r++) {
                    send_IQ_buffer(r);
                }
                samples=0;
            }
        }

    } else {
        fprintf(stderr,"SYNC error\n");
        dump_ozy_buffer("SYNC ERROR",rx_frame,buffer);
        exit(1);
    }
}

#endif

/**
 * Read from USB EP4 and send the block of full-sampling-speed samples to process_bandscope_buffer().
 *
 */
void* ozy_ep4_io_thread(void* arg) {
    unsigned char buffer[BANDSCOPE_BUFFER_SIZE*2];
    int bytes;

    while(1) {
        bytes=ozy_read(0x84,(void*)(bandscope.buffer),sizeof(buffer));
        if (bytes < 0) {
            fprintf(stderr,"ozy_ep4_io_thread: OzyBulkRead failed %d bytes\n",bytes);
            exit(1);
        } else if (bytes != BANDSCOPE_BUFFER_SIZE*2) {
            fprintf(stderr,"ozy_ep4_io_thread: OzyBulkRead only read %d bytes\n",bytes);
            exit(1);
        } else {
            // process the buffer
            process_bandscope_buffer(buffer);
        }
    }
}

/**
 * \param buffer A pointer to a buffer of 16-bit samples
 * The samples are converted to float and stored in bandscope.buffer[].
 * When all have been processed, call send_bandscope_buffer().
 */
void process_bandscope_buffer(char* buffer) 
{
    int b=0;
    int i=0;
    int sample;
    float sample_float;

    for (i = 0; i < BANDSCOPE_BUFFER_SIZE; i++) 
	{
        sample    = (int)((signed char) buffer[b++]) << 8;
        sample   += (int)((unsigned char)buffer[b++]);
        sample_float = (float)sample / 32767.0; // 16 bit sample
        bandscope.buffer[i] = sample_float;
    }

    send_bandscope_buffer();
}

/**
 * Convert float values (demodulated audio send by dspserver) to 16-bit integers,
 * and place into ozy_output_buffer[]
 * ozy_output_buffer[] also has designated places for transmit audio samples, which
 * are, at present, all simply to zero.  I.E. transmit is disabled.
 * When the output buffer has been filled, call write_ozy_output_buffer().
 * \param left_output_buffer A pointer to a buffer of floats.
 * \param right_output_buffer A pointer to a buffer of floats.
 */
void process_ozy_output_buffer(float *left_output_buffer,float *right_output_buffer) 
{
    unsigned char ozy_samples[1024*8];
    int j;
    short left_rx_sample;
    short right_rx_sample;
    short left_tx_sample;
    short right_tx_sample;

    // process the output
    for(j = 0; j < BUFFER_SIZE; j += output_sample_increment) 
	{
        left_rx_sample = (short)(left_output_buffer[j] * 32767.0);
        right_rx_sample = (short)(right_output_buffer[j] * 32767.0);

        left_tx_sample = 0;
        right_tx_sample = 0;

        ozy_output_buffer[ozy_output_buffer_index++]=left_rx_sample>>8;
        ozy_output_buffer[ozy_output_buffer_index++]=left_rx_sample;
        ozy_output_buffer[ozy_output_buffer_index++]=right_rx_sample>>8;
        ozy_output_buffer[ozy_output_buffer_index++]=right_rx_sample;
        ozy_output_buffer[ozy_output_buffer_index++]=left_tx_sample>>8;
        ozy_output_buffer[ozy_output_buffer_index++]=left_tx_sample;
        ozy_output_buffer[ozy_output_buffer_index++]=right_tx_sample>>8;
        ozy_output_buffer[ozy_output_buffer_index++]=right_tx_sample;

        if(ozy_output_buffer_index==OZY_BUFFER_SIZE) 
		{
            write_ozy_output_buffer();
            ozy_output_buffer_index=OZY_HEADER_SIZE;
        }
    }
}

void ozy_set_preamp(int p) {
    control_out[3]=control_out[3]&0xFB;
    control_out[3]=control_out[3]|(p<<2);
}

void ozy_set_dither(int dither) {
    control_out[3]=control_out[3]&0xF7;
    control_out[3]=control_out[3]|(dither<<3);
}

void ozy_set_random(int random) {
    control_out[3]=control_out[3]&0xEF;
    control_out[3]=control_out[3]|(random<<4);
}

void ozy_set_10mhzsource(int source) {
    control_out[1]=control_out[1]&0xF3;
    control_out[1]=control_out[1]|(source<<2);
}

void ozy_set_122_88mhzsource(int source) {
    control_out[1]=control_out[1]&0xEF;
    control_out[1]=control_out[1]|(source<<4);
}

void ozy_set_micsource(int source) {
    control_out[1]=control_out[1]&0x7F;
    control_out[1]=control_out[1]|(source<<7);
}

void ozy_set_class(int c) {
    control_out[2]=control_out[2]&0xFE;
    control_out[2]=control_out[2]|c;
}

void ozy_set_timing(int t) {
    timing=t;
}

