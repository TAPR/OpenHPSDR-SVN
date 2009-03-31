/* 
 * File:   util.c
 * Author: jm57878
 *
 * Created on 05 March 2009, 10:02
 */

#include <stdio.h>
#include <stdlib.h>

#include "ozy_buffers.h"
/*
 * 
 */

void dump_ozy_buffer(char* prefix,unsigned char* buffer) {
    int i;
    for(i=0;i<OZY_BUFFER_SIZE;i+=16) {
        fprintf(stderr, "%s [%04X] %02X%02X%02X%02X%02X%02X%02X%02X %02X%02X%02X%02X%02X%02X%02X%02X\n",
                prefix,
                i,
                buffer[i],buffer[i+1],buffer[i+2],buffer[i+3],buffer[i+4],buffer[i+5],buffer[i+6],buffer[i+7],
                buffer[i+8],buffer[i+9],buffer[i+10],buffer[i+11],buffer[i+12],buffer[i+13],buffer[i+14],buffer[i+15]
                );
    }
}
