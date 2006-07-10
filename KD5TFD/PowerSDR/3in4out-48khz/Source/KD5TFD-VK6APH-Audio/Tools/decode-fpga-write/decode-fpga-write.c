/* Copyright (c) 2006  Bill Tracey (bill@ejwt.com) */ 

#include <stdio.h>
#include <stdlib.h> 

#define FRAME_SIZE (512)


short getShortFromFrame(unsigned char *framep, int ofs) {
    short result;
    result = framep[ofs] << 8;
    result |= framep[ofs+1];
    return result;
}


void dumpFrame(unsigned char *framep) {
    short i, q, ml, mr;
    unsigned int sync;
    int ofs = 8;
    sync = framep[0] << 16;
    sync |= framep[1] << 8;
    sync |= framep[2];
    if ( sync != 0x7f7f7f ) {
        printf("bad sync\n");
    }
    while ( ofs < FRAME_SIZE ) {
        i = getShortFromFrame(framep, ofs);
        q = getShortFromFrame(framep, ofs+2);
        ml = getShortFromFrame(framep, ofs+4);
        mr = getShortFromFrame(framep, ofs+6);
        printf("%06d %06d %06d %06d\n", i, q, ml, mr);
        ofs += 8;
    }
    return;
}

int main(int argc, char *argv[]) {
    FILE *ifile;
    unsigned char ibuf[FRAME_SIZE];
    size_t numread;

    if ( argc != 2 ) {
        printf("need input filename!\n");
        exit(1);
    }
    ifile = fopen(argv[1], "rb");
    if ( ifile == NULL ) {
        printf("failed to open \'%s\'\n", argv[1]);
        exit(2);
    }
    numread = fread(ibuf, 1, FRAME_SIZE, ifile);
    while ( numread == FRAME_SIZE ) {
        dumpFrame(ibuf);
        numread = fread(ibuf, 1, FRAME_SIZE, ifile);
    }
    fclose(ifile);

}
