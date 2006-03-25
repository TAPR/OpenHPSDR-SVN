#include <stdio.h>

#define STATE_LOOKING_FOR_LEFT (1)
#define STATE_LOOKING_FOR_SYNC (2)
#define STATE_LOOKING_FOR_RIGHT (3)
#define STATE_NOSYNC (4)
#define STATE_SYNC_FOUND (5)
#define STATE_SYNCED (6)
#define STATE_LOOKING_FOR_SYNC_HI (7)
#define STATE_LOOKING_FOR_SYNC_LO (8)

FILE *ifile;
#define BUF_SIZE (32768)
unsigned char buf[BUF_SIZE];

int main(int argc, char **argv) {
    int first_byte = 1;
    unsigned short this_num = 0;
    int i;
    int numread;
    int state = STATE_NOSYNC;
    int nosync_count = 0;
    unsigned char last_byte;
    unsigned int last_count;
    unsigned int fcount;
    unsigned char fbuf[6];
    ifile = fopen("usbsuck.dat", "rb");
    if ( ifile == NULL ) {
        fprintf(stderr, "Failed to open input file\n");
        exit(99);
    }
    while ( (numread = fread(buf, 1, BUF_SIZE, ifile)) != 0 ) {
        for ( i = 0; i < numread; i++ ) {
            // if we're not sync'd see if we can gain sync
            //
            if ( state == STATE_NOSYNC ) {  // not synced, just print the byte, 10 per line
                if ( nosync_count > 0 ) {
                    printf(" ");
                }
                printf("%d (0x%02x)", buf[i], buf[i]);
                ++nosync_count;
                if ( nosync_count > 10 ) {
                    printf("\n");
                    nosync_count = 0;
                }
            }
            if ( state == STATE_NOSYNC && last_byte == 0xff && buf[i] == 0xff  ) {
                printf("\nSync gained.\n");
                state = STATE_SYNCED;
                fcount = 0;
            }
            else if ( state == STATE_SYNCED ) {
                fbuf[fcount] = buf[i];
                ++fcount;
                if ( fcount == 6 ) { // frame is full
                    unsigned short sernum;
                    unsigned int count;
                    sernum = (fbuf[0] << 8) | fbuf[1];
                    count = fbuf[2] << 24 | fbuf[3] << 16 | fbuf[4] << 8 | fbuf[5];
                    printf("sernum: %hu  count=%d  delta=%d\n", sernum, count, count-last_count);
                    last_count = count;
                    state = STATE_LOOKING_FOR_SYNC_HI;
                    fcount = 0;
                }
            }
            else if ( state == STATE_LOOKING_FOR_SYNC_HI ) {
                if ( buf[i] != 0xff ) {
                    printf("\nSync lost.\n");
                    state = STATE_NOSYNC;
                }
                else {
                    state = STATE_LOOKING_FOR_SYNC_LO;
                }
            }
            else if ( state == STATE_LOOKING_FOR_SYNC_LO ) {
                if ( buf[i] != 0xff ) {
                    printf("\nSync lost.\n");
                    state = STATE_NOSYNC;
                }
                else {
                    state = STATE_SYNCED;
                    fcount = 0;
                }
            }
            last_byte = buf[i];
        }
    }
    if ( ferror(ifile) ) {
        fprintf(stderr, "\nHit I/O Error.\n");
    }
    fclose(ifile);
}
