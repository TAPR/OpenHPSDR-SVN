#include <unistd.h>
#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/ioctl.h>
#include <linux/soundcard.h>
#include <semaphore.h>

#define RATE 48000   /* the sampling rate */
#define SIZE 16      /* sample size: 8 or 16 bits or 24 bits */
#define CHANNELS 2  /* 1 = mono 2 = stereo */

static int fd;

static char local_audio_device[32];
static int sample_rate=48000;

static int insert=0;
static unsigned char audio_buffer[1024*sizeof(short)*2*2];
static int write_local_audio_thread_id;

static sem_t audio_sem;

void write_local_audio_thread(void* arg);

void set_local_audio_device(char * device) {
    strcpy(local_audio_device,device);
}

int open_local_audio() {
    int arg;
    int status;
    int rc;

fprintf(stderr,"init_local_audio\n");

    /* open sound device */
    fd = open(local_audio_device, O_RDWR);
    if (fd < 0) {
        perror("open of local audio device failed");
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

    arg = sample_rate;      /* sampling rate */
    status = ioctl(fd, SOUND_PCM_WRITE_RATE, &arg);
    if (status == -1)
        perror("SOUND_PCM_WRITE_WRITE ioctl failed");

    arg = AFMT_S16_BE;       /* signed big endian */
    status = ioctl(fd, SOUND_PCM_SETFMT, &arg);
    if (status == -1)
        perror("SOUND_PCM_SETFMTS ioctl failed");

    arg=(8<<16)|(14);
    status=ioctl(fd,SNDCTL_DSP_SETFRAGMENT,&arg); 
    if (status == -1)
        perror("SNDCTL_DSP_SETFRAGMENT ioctl failed");

    sem_init (&audio_sem, 0, 0);
    rc=pthread_create(&write_local_audio_thread_id,NULL,write_local_audio_thread,NULL);
    if(rc!=0) {
        fprintf(stderr,"pthread_create failed on write_local_audio_thread: rc=%d\n",rc);
    }

    return 0;

}

int close_local_audio() {
    close(fd);
}

int write_local_audio(float* left_samples,float* right_samples,int samples,int increment) {
    int rc;
    int bytes;
    int i;
    short sample;

    for(i=0;i<samples;i+=increment) {
        sample=(short)(left_samples[i]*32767.0F);
        audio_buffer[insert++]=sample>>8;
        audio_buffer[insert++]=sample;
        sample=(short)(right_samples[i]*32767.0F);
        audio_buffer[insert++]=sample>>8;
        audio_buffer[insert++]=sample;
    }

    if(insert==sizeof(audio_buffer)/2) {
        sem_post(&audio_sem);
    } else if(insert==sizeof(audio_buffer)) {
        insert=0;
        sem_post(&audio_sem);
    }

    return 0;
}

void write_local_audio_thread(void* arg) {
    int rc;

    while(1) {
        sem_wait(&audio_sem);
        rc=write(fd,audio_buffer,sizeof(audio_buffer)/2);
        if(rc!=sizeof(audio_buffer)/2) {
            perror("writing first audio_buffer");
        }

        sem_wait(&audio_sem);
        rc=write(fd,&audio_buffer[sizeof(audio_buffer)/2],sizeof(audio_buffer)/2);
        if(rc!=sizeof(audio_buffer)/2) {
            perror("writing second audio_buffer");
        }
    }

}

