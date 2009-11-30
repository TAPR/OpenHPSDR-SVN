/**
* @file ozyio.c
* @brief USB I/O with Ozy
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


#include <unistd.h>
#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/ioctl.h>
#include <linux/soundcard.h>

#include "softrock.h"
#include "softrockio.h"

#define RATE 96000   /* the sampling rate */
#define SIZE 24      /* sample size: 8 or 16 bits or 24 bits */
#define CHANNELS 2  /* 1 = mono 2 = stereo */

static int fd;

int softrock_open(void) {
    int arg;
    int status;
    int rc;

fprintf(stderr,"softrock_open: %s\n",softrock_get_device());
    /* open sound device */
    fd = open(softrock_get_device(), O_RDWR);
    if (fd < 0) {
        perror("open of audio device failed");
        exit(1);
    }

    /* set sampling parameters */
    arg = SIZE;      /* sample size */
    status = ioctl(fd, SOUND_PCM_WRITE_BITS, &arg);
    if (status == -1)
        perror("SOUND_PCM_WRITE_BITS ioctl failed");
    if (arg != SIZE)
        perror("unable to set sample size");

    arg = CHANNELS;  /* mono or stereo */
    status = ioctl(fd, SOUND_PCM_WRITE_CHANNELS, &arg);
    if (status == -1)
        perror("SOUND_PCM_WRITE_CHANNELS ioctl failed");
    if (arg != CHANNELS)
        perror("unable to set number of channels");

fprintf(stderr,"softrock_get_sample_rate: %d\n",softrock_get_sample_rate());
    arg = softrock_get_sample_rate();      /* sampling rate */
    status = ioctl(fd, SOUND_PCM_WRITE_RATE, &arg);
    if (status == -1)
        perror("SOUND_PCM_WRITE_WRITE ioctl failed");

    arg = AFMT_S24_LE;       /* signed little endian */
    status = ioctl(fd, SOUND_PCM_SETFMT, &arg);
    if (status == -1)
        perror("SOUND_PCM_SETFMTS ioctl failed");

    return 0;
}

int softrock_close() {
    close(fd);
}

int softrock_write(unsigned char* buffer,int buffer_size) {
    int rc;
    int bytes;

    rc = write(fd,buffer,buffer_size);
    if(rc!=buffer_size) {
        perror("error reading audio buffer");
        exit(1);
    }

    return rc;
}

int softrock_read(unsigned char* buffer,int buffer_size) {
    int rc;
    int bytes;

    rc = read(fd,buffer,buffer_size);
    if(rc!=buffer_size) {
        perror("error reading audio buffer");
        exit(1);
    }

    return rc;
}
