

/*
 * Routine to dump a buffer.
 *
 * ofile - open file descriptor of file to be dumped to.
 *         file is assumed to be open in write mode, with
 *         translation enabled (not binary).
 *
 * buf - a pointer to the buffer to be dumped
 *
 * count - number of bytes to be dumped
 *
 * comment - a pointer to a character string to be dsiplayed
 *           at the beginning of the dump.  A comment of NULL
 *           indicates no comment should be dumped.
 *
 */
#include <stdio.h>
#include <ctype.h>
#include "wjtdump.h"
void WJTDump(FILE *ofile,                /* file handle to dump to - assumed to be      */
                                         /* open.                                       */
          unsigned char *buf,            /* pointer to data to dump                     */
          unsigned int count,            /* number of bytes to dump                      */
          unsigned char *comment) {      /* comment put out at top of dump, may be NULL */
    static unsigned char HexTbl[] = "0123456789ABCDEF"; /* Hex Xlat tbl */
    static unsigned char FmtSpec[] = "%05u %04X  %s  [%s]\n";

    unsigned char obuf1[40];  /* buffer for the hex part of the output */
    unsigned char obuf2[20];  /* buffer for the text part of output    */
    unsigned char *optr1;     /* points to current position in obuf1   */
    unsigned char *optr2;     /* points to current position in obuf2   */
    unsigned int i,j;         /* counters                              */

    if ( comment ) fprintf(ofile,"%s\n",comment);
    if ( !buf ) {
        fprintf(ofile,"(null)\n");
        return;
    }
    /*
     * For each full block of 16 bytes
     */
    for ( i = 0; i < (count >> 4); i++ ) {
        optr1 = obuf1;
        optr2 = obuf2;
        for ( j = 0; j < 16; j++ ) {
            if ( isprint(buf[(i << 4) + j])) *optr2++ = buf[(i<<4)+j];
            else *optr2++ = '.';
            *optr1++ = HexTbl[buf[(i<<4) + j] >> 4];
            *optr1++ = HexTbl[buf[(i<<4) + j] & 0x0f];
            if ( (j == 3) || (j == 7) || (j == 11) ) *optr1++ = ' ';
        }
        *optr1 = 0; /* make them null */
        *optr2 = 0;
        fprintf(ofile,(char *)FmtSpec,i<<4,i<<4,(char *)obuf1,
                (char *)obuf2);
    }
    /*
     * Now do anything remaining (after all the even 16 byte blocks)
     */
    if ( count & 0x0f ) { /* there is a partial block at the end */
        optr1 = obuf1;
        optr2 = obuf2;
        for ( i = 0; i < (count & 0x0f); i++ ) {
            if ( isprint(buf[(count & 0xfff0) + i]) ) *optr2++ = buf[(count & 0xfff0) + i];
            else *optr2++ = '.';
            *optr1++ = HexTbl[buf[(count & 0xfff0) + i] >> 4];
            *optr1++ = HexTbl[buf[(count & 0xfff0) + i] & 0x0f];
            if ((i == 3) || (i == 7) || (i == 11) ) *optr1++ = ' ';
        }
        *optr2 = 0;
        for ( i = 0; i < 16 - (count & 0x0f); i++ ) {
            *optr1++ = ' ';
            *optr1++ = ' ';
        }
        if ( (count &0x0f ) < 4 ) *optr1++ = ' ';
        if ( (count &0x0f ) < 8 ) *optr1++ = ' ';
        if ( (count &0x0f ) < 12 ) *optr1++ = ' ';
        *optr1 = 0;
        fprintf(ofile,(char *)FmtSpec,count & 0xfff0,count & 0xfff0,
                (char *)obuf1,(char *)obuf2);
    }
    return;
}
