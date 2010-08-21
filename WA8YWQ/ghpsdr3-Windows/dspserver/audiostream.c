/**
* @file audiostream.c
* @brief audio out put stream (for iPhone)
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-03-10
*/

/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
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

#include <stdio.h>
#include <string.h>
#ifdef __linux__
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <pthread.h>
#else
#include "pthread.h"
#endif

#include "audiostream.h"
#include "client.h"
#include "buffer.h"

extern int audio_buffer_size;
extern unsigned char* audio_buffer;
extern int send_audio;

static int sample_count=0;

static int audio_stream_buffer_insert=0;

static unsigned char encodetable[65536];

unsigned char alaw(short sample);

void init_alaw_tables();

void audio_stream_init(int receiver) {

    int rc;

    init_alaw_tables();

}

/* --------------------------------------------------------------------------*/
/**
* @brief put samples to the audio stream
*
* @return
*/

void audio_stream_reset() {
    audio_stream_buffer_insert=0;
}

/**
 * Every sixth time this function is called the left_sample and right_sample are averaged,
 * alaw-encoded, and inserted into audio_buffer[].  When the number of samples inserted reaches
 * audio_buffer_size, call client_send_audio().
 * \param left_sample
 * \param right_sample
 */
void audio_stream_put_samples(short left_sample,short right_sample) {

    // samples are delivered at 48K
    // output to stream at 8K (1 in 6)
    if(sample_count==0) {
        // use this sample and convert to a-law (mono)
            audio_buffer[audio_stream_buffer_insert+48]=alaw((left_sample+right_sample)/2);
            audio_stream_buffer_insert++;
            if(audio_stream_buffer_insert==audio_buffer_size) {
                audio_buffer[0]=AUDIO_BUFFER;
                client_send_audio();
                audio_stream_buffer_insert=0;
            }
    }
    sample_count++;
    if(sample_count==6) {
        sample_count=0;
    }
}

/**
 * Use parameter sample to index encodetable[], and return the byte found there.
 * \param sample
 */
unsigned char alaw(short sample) {
    return encodetable[sample&0xFFFF];
}

/**
 * Calculate values for alaw encodeing table, encodetable[].   The table is 65536 bytes long.
 */
void init_alaw_tables() {
    int i;

/*
    for (i = 0; i < 256; i++) {
        int input = i ^ 85;
        int mantissa = (input & 15) << 4;
        int segment = (input & 112) >> 4;
        int value = mantissa + 8;
        if (segment >= 1) value += 256;
        if (segment > 1) value <<= (segment - 1);
        if ((input & 128) == 0) value = -value;
        decodetable[i]=(short)value;
    }
*/
    for(i=0;i<65536;i++) 
	{
        int exponent=7;
        int expMask;
		int mantis;
		unsigned char alaw;
        short sample=(short)i;

        int sign=(sample&0x8000) >> 8;
        if(sign != 0)
		{
            sample=(short)-sample;
            sign=0x80;
        }

        if(sample > 32635) sample = 32635;

        for(expMask=0x4000;(sample&expMask)==0 && exponent>0; exponent--, expMask >>= 1) 
		{
        }
        mantis = (sample >> ((exponent == 0) ? 4 : (exponent + 3))) & 0x0f;
        alaw = (unsigned char)(sign | exponent << 4 | mantis);
        encodetable[i]=(unsigned char)(alaw^0xD5);
    }

}

