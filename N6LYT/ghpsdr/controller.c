#include <stdio.h>      // standard input / output functions
#include <stdlib.h>
#include <string.h>     // string function definitions
#include <errno.h>      // Error number definitions
#include <fcntl.h>      // File control definitions
#include <sys/ioctl.h>
#include <sys/unistd.h>
#include <pthread.h>
#include <termios.h>    // POSIX terminal control definitions

#include "controller.h"

static int controller;
static pthread_t controller_thread_id;

void* controller_thread(void* arg);

extern void controller_update(char* command);

int init_controller() {

    fprintf(stderr,"init_controller\n");
    controller = open( "/dev/ttyUSB0", O_RDWR | O_EXCL | O_NONBLOCK );
    if(controller<0) {
        fprintf(stderr,"cannot open /dev/ttyUSB0\n");
        return -1;
    }
    fcntl( controller, F_SETFL, fcntl( controller, F_GETFL ) & ~O_NONBLOCK );

    /* *** Configure Port *** */
    struct termios tty;

    /* Error Handling */
    if ( tcgetattr ( controller, &tty ) != 0 )
    {
        fprintf(stderr, "Error %d from tcgetattr: %s\n", errno, strerror(errno));
        return -1;
    }


    cfmakeraw( &tty );
    tty.c_cflag |= ( CLOCAL | CREAD );

    cfsetispeed( &tty, B9600 );
    cfsetospeed( &tty, B9600 );

    if ( tcsetattr ( controller, TCSAFLUSH, &tty ) != 0)
    {
        fprintf(stderr, "Error %d from tcsetattr\n", errno);
        return -1;
    }


    int rc=pthread_create(&controller_thread_id,NULL,controller_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on controller_thread: rc=%d\n", rc);
        return -1;
    }

    return 0;
}

void* controller_thread(void* arg) {
    char buf[64];
    int index=0;
    char c;
    fprintf(stderr,"controller_thread\n");
    while(1) {
        int n = read( controller, &c , 1 );
        if(n<0) {
            fprintf(stderr,"controller_thread: read error %d\n",n);
        } else if(n==0) {
            // eof?
        } else {
            buf[index++]=c;
            if(c==';') {
                buf[index]='\0';
                controller_update(buf);
                index=0;
            }
        }
    }
    fprintf(stderr,"controller_thread ... exit\n");
}
