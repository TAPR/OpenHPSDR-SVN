#include <stdio.h>

#define FRAME_SIZE (512)


#define MIC_DATA_BUF_SIZE (10240)
short MicDataBuf[MIC_DATA_BUF_SIZE];
int MicDataBufIdx;

FILE *MicDataFile = NULL;

void writeMicDataBuf() {
    int numw;
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
        mic = getShortFromFrame(framep, ofs+4);
        printf("%10d %10d %06d\n", i, q, mic);
        ofs += 8;
        addMicSample(mic);
    }
    return;
}

int main(int argc, char *argv[]) {
    FILE *ifile;
    unsigned char ibuf[FRAME_SIZE];
    int numread;

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
    writeMicDataBuf();
    if ( MicDataFile != NULL ) fclose(MicDataFile);
    fclose(ifile);

}
