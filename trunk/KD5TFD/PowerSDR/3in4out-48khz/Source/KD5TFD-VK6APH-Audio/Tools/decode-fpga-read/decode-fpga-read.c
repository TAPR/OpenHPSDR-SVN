/* Copyright (c) 2006  Bill Tracey (bill@ejwt.com) */ 

// 
// quick and dirty formatted dump of the fpga-reads.dat file 
// 
#include <stdio.h>
#include <stdlib.h> 

#define FRAME_SIZE (512)


#define MIC_DATA_BUF_SIZE (10240)
short MicDataBuf[MIC_DATA_BUF_SIZE];
int MicDataBufIdx;

FILE *MicDataFile = NULL;

void writeMicDataBuf() {
    if ( MicDataBufIdx == 0 ) return;
    if ( MicDataFile == NULL ) {
        MicDataFile = fopen("micdata.raw", "wb");
        if ( MicDataFile == NULL ) return;
    }
    fwrite(MicDataBuf, sizeof(short), MicDataBufIdx, MicDataFile);
    MicDataBufIdx = 0;
    return;
}

void addMicSample(short s) {
    MicDataBuf[MicDataBufIdx] = s;
    ++MicDataBufIdx;
    if ( MicDataBufIdx >= MIC_DATA_BUF_SIZE ) {
        writeMicDataBuf();
    }
    return;
}

short getShortFromFrame(unsigned char *framep, int ofs) {
    short result;
    result = framep[ofs] << 8;
    result |= framep[ofs+1];
    return result;
}

int getInt24FromFrame(unsigned char *framep, int ofs) {
    int result;
    result = framep[ofs] <<16;
    result |= framep[ofs+1] << 8;
    result |= framep[ofs+2];
    return result;
}


void dumpFrame(unsigned char *framep) {
    short i, q, mic;
    unsigned int sync;
    int ofs = 8;
    sync = framep[0] << 16;
    sync |= framep[1] << 8;
    sync |= framep[2];
    if ( sync != 0x800000 ) {
        printf("bad sync\n");
    }
    while ( ofs < FRAME_SIZE ) {
        i = getInt24FromFrame(framep, ofs);
        q = getInt24FromFrame(framep, ofs+3);
        mic = getShortFromFrame(framep, ofs+6);
        printf("%10d %10d %6d\n", i, q, mic);
        ofs += 8;
        addMicSample(mic);
    }
    return;
}

int main(int argc, char *argv[]) {
    FILE *ifile;
    unsigned char ibuf[FRAME_SIZE];
	unsigned char fbuf[FRAME_SIZE]; 
	unsigned char l0, l1;
    size_t numread;
	int no_sync_bytes; 
	int missing_sync_count = 0; 
	int good_frame_count = 0; 

    if ( argc != 2 ) {
        printf("need input filename!\n");
        exit(1);
    }
    ifile = fopen(argv[1], "rb");
    if ( ifile == NULL ) {
        printf("failed to open \'%s\'\n", argv[1]);
        exit(2);
    }


	l0 = 0; l1 = 0; 

	no_sync_bytes = 0; 

	while ( 1 ) { 
		numread = fread(ibuf, 1, 1, ifile); 
		if ( numread != 1 ) break; // short read or end of file 
		if ( l0 == 0x80 && l1 == 0 && ibuf[0] == 0 ) {  // gained sync 
			fbuf[0] = 0x80; fbuf[1] = 0x00; fbuf[2] = 0x00; 
			numread = fread(fbuf+3, 1, FRAME_SIZE-3, ifile); 
			if ( numread != FRAME_SIZE-3 ) break; // no more data 
			dumpFrame(fbuf); 
			if ( no_sync_bytes > 2 ) { 
				++missing_sync_count;
			} 
			no_sync_bytes = 0; 
			++good_frame_count; 
			
		} 
		else { 
			l0 = l1; 
			l1 = ibuf[0]; 
			++no_sync_bytes; 
		} 
	} 

	printf("\n\ngood frame count: %d\nmissing sync count: %d\n", good_frame_count, missing_sync_count); 

    writeMicDataBuf();
    if ( MicDataFile != NULL ) fclose(MicDataFile);
    fclose(ifile);
}
