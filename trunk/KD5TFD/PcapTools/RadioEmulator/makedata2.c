#include <stdio.h>

short iData[] = {
 0,
 9658,
 18458,
 25618,
 30502,
 32675,
 31945,
 28377,
 22287,
 14217,
 4884,
 -4884,
 -14217,
 -22287,
 -28377,
 -31945,
 -32675,
 -30502,
 -25618,
 -18458,
 -9658,
 0,
 9658,
 18458,
 25618,
 30502,
 32675,
 31945,
 28377,
 22287,
 14217,
 4884,
 -4884,
 -14217,
 -22287,
 -28377,
 -31945,
 -32675,
 -30502,
 -25618,
 -18458,
 -9658,
 0,
 9658,
 18458,
 25618,
 30502,
 32675,
 31945,
 28377,
 22287,
 14217,
 4884,
 -4884,
 -14217,
 -22287,
 -28377,
 -31945,
 -32675,
 -30502,
 -25618,
 -18458,
 -9658
};

short qData[] =  {
   32767,
   31311,
   27073,
   20430,
   11971,
   2449,
   -7291,
   -16384,
   -24020,
   -29522,
   -32401,
   -32401,
   -29522,
   -24020,
   -16384,
   -7291,
   2449,
   11971,
   20430,
   27073,
   31311,
   32767,
   31311,
   27073,
   20430,
   11971,
   2449,
   -7291,
   -16384,
   -24020,
   -29522,
   -32401,
   -32401,
   -29522,
   -24020,
   -16384,
   -7291,
   2449,
   11971,
   20430,
   27073,
   31311,
   32767,
   31311,
   27073,
   20430,
   11971,
   2449,
   -7291,
   -16384,
   -24020,
   -29522,
   -32401,
   -32401,
   -29522,
   -24020,
   -16384,
   -7291,
   2449,
   11971,
   20430,
   27073,
   31311
};


void cvt24BitsIntTo3Bytes(int num, unsigned char *bytep) {
    *(bytep+2) = num & 0xff;
    num = num >> 8;
    *(bytep+1) = num & 0xff;
    num = num >> 8;
    *bytep = num & 0xff;
    return;
}

void cvt16BitsIntTo2Bytes(short num, unsigned char *bytep) {
    *(bytep+1) = num & 0xff;
    num = num >> 8;
    *bytep = num & 0xff;
    return;
}



int main(int argc, char *argv) {
    int i;
    int j;
    unsigned char bytes[8];
    for ( i = 0; i < 63; i++ ) {

        cvt16BitsIntTo2Bytes(iData[i], bytes);
        cvt16BitsIntTo2Bytes(qData[i], bytes+2);
        cvt16BitsIntTo2Bytes(-iData[i], bytes+4);
        cvt16BitsIntTo2Bytes(-qData[i], bytes+6);

        printf("    ");
        for ( j = 0; j < 8; j++ ) {
            printf("0x%02x, ", bytes[j]);
        }
        printf("/* %8d (0x%02x) %8d (0x%02x)  %8d (0x%02x) %8d (0x%02x)  */\n", iData[i], iData[i], qData[i], qData[i],
                                                                               -iData[i], -iData[i], -qData[i], -qData[i]);
    }
}

