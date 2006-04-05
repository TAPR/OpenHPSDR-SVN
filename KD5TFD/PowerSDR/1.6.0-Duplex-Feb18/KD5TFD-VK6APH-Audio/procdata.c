#include <stdio.h>

#define STATE_LOOKING_FOR_LEFT (1)
#define STATE_LOOKING_FOR_SYNC (2)
#define STATE_LOOKING_FOR_RIGHT (3)
#define STATE_NOSYNC (4)
#define STATE_SYNC_FOUND (5)

FILE *ifile;
#define BUF_SIZE (32768)
unsigned char buf[BUF_SIZE];
int main(int argc, char **argv) {
    int first_byte = 1;
    int this_num = 0;
    int i;
    int numread;
    int state = STATE_NOSYNC;
    int nosync_count = 0;
    unsigned char last_byte;
    ifile = fopen("usbsuck.dat", "rb");
    if ( ifile == NULL ) {
        fprintf(stderr, "Failed to open input file\n");
        exit(99);
    }
    while ( (numread = fread(buf, 1, BUF_SIZE, ifile)) != 0 ) {
        for ( i = 0; i < numread; i++ ) {
            if ( last_byte == 0x80 && buf[i] == 0x00 ) { // found sync byte -- force sync
                first_byte = 0;
                this_num = 0x8000;
            }
            if ( first_byte ) { // getting first byte of word
                first_byte = 0;
                this_num = (buf[i] << 8) & 0xff00;
            }
            else { // we have a full word
                first_byte = 1;
                this_num = this_num | (buf[i] & 0xff);
            }
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
            if ( first_byte == 1 ) { // we've completed a word a think we have sync, print words
                if ( this_num == 0x8000 && state != STATE_LOOKING_FOR_SYNC ) {
                    state = STATE_SYNC_FOUND;
                }
                switch ( state ) {
                    case STATE_LOOKING_FOR_LEFT:
                        printf(" %d (0x%04x)", this_num, this_num);
                        state = STATE_LOOKING_FOR_RIGHT;
                        break;

                    case STATE_LOOKING_FOR_RIGHT:
                        printf(" %d (0x%04x)", this_num, this_num);
                        state = STATE_LOOKING_FOR_SYNC;
                        break;

                    case STATE_LOOKING_FOR_SYNC:
                        if ( this_num != 0x8000 ) {
                            unsigned char b1;
                            unsigned char b2;
                            b1 = ( this_num >> 8 ) & 0xff;
                            b2 = this_num & 0xff;
                            printf("\nSync Lost.\n%d (0x%02x)\n%d (0x%02x)",b1, b1, b2, b2);
                            nosync_count = 2;
                            state = STATE_NOSYNC;
                        }
                        else {  // sync found
                            printf("\n%d (0x%04x)", this_num, this_num);
                            state = STATE_LOOKING_FOR_LEFT;
                        }
                        break;

                    case STATE_SYNC_FOUND:
                        printf("\nsync found\n%d (0x%04x)", this_num, this_num);
                        state = STATE_LOOKING_FOR_LEFT;
                        break;

                    case STATE_NOSYNC: /* nothing to do -- did it above */
                        break;

                    default:
                        fprintf(stderr, "Internal logic error - invalid state!\n");
                        exit(9);
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
