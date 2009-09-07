/*
 * JanusAudio.dll - Support library for HPSDR.org's Janus/Ozy Audio card
 * Copyright (C) 2006,2007  Bill Tracey (bill@ejwt.com) (KD5TFD)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 *
 */

// io-thread.c - this file contains routines reading and writing from the xylo
//


// this is updated for 4 channels
#include <stdio.h>


#ifdef LINUX
#include <unistd.h>
#endif


#include "KD5TFD-VK6APH-Audio.h"
// #define PERF_DEBUG 1
#include "private.h"

#ifdef PERF_DEBUG
#include "nanotimer.h"
HLA_COUNTER ReadHLA;
HLA_COUNTER WriteHLA;
#endif

// #define INBUF_SIZE (512)  // size of fpga buffers - incoming
// #define OUTBUF_SIZE (512) // size of fpga buffers - outgoing
// char read_buf[INBUF_SIZE];
// char write_buf[OUTBUF_SIZE];
int io_keep_running;

#define MAX_INBUF_SIZE (8192)  // max size of fpga buffers - incoming  - only used for debugging
#define MAX_OUTBUF_SIZE (2048) // max size of fpga buffers - outgoing  - only used for debugging


// obsoloete - fpga tells us free space in fifo now #define FPGA_INPUT_FIFO_SIZE (2048)

// save writes to the fpga
// #define SAVE_WRITES_TO_BUFFER  (1)
#ifdef SAVE_WRITES_TO_BUFFER
#define NUM_WRITE_BUFS_TO_SAVE (300)
char WriteSaveBuf[NUM_WRITE_BUFS_TO_SAVE][MAX_OUTBUF_SIZE];
int SaveWritesIdx = 0;  // idx of next buffer to be written
#endif

// save buffers read from the fpga
// #define SAVE_READS_TO_BUFFER (1)
#ifdef SAVE_READS_TO_BUFFER
#define NUM_READ_BUFS_TO_SAVE (1000)
char ReadSaveBuf[NUM_READ_BUFS_TO_SAVE][MAX_INBUF_SIZE];
int SaveReadsIdx = 0; // idx of next buf to write to
#endif


#if 0
// obsolete
#define STATE_LOOKING_FOR_LEFT (1)
#define STATE_LOOKING_FOR_SYNC (2)
#define STATE_LOOKING_FOR_RIGHT (3)
#define STATE_NOSYNC (4)
#define STATE_SYNC_HALF_FOUND (7)
#define STATE_SYNC_FOUND (5)
#define STATE_LOOKING_FOR_SAMPLE (6)
#define STATE_LOOKING_FOR_SYNC_FINISH (8)
#endif

#define STATE_NOSYNC (1)
#define STATE_CONTROL0 (2)
#define STATE_CONTROL1 (3)
#define STATE_CONTROL2 (4)
#define STATE_CONTROL3 (5)
#define STATE_CONTROL4 (6)
#define STATE_SYNC_HI (7)
#define STATE_SYNC_MID (8)
#define STATE_SYNC_LO (9)
#define STATE_SAMPLE_HI (10)
#define STATE_SAMPLE_MID (11)
#define STATE_SAMPLE_LO (12)
#define STATE_SAMPLE_MIC_HI (13)
#define STATE_SAMPLE_MIC_LO (14)

#define OUT_STATE_SYNC_HI_NEEDED (1)
#define OUT_STATE_LEFT_HI_NEEDED (2)
#define OUT_STATE_RIGHT_HI_NEEDED (3)
#define OUT_STATE_SYNC_LO_NEEDED (4)
#define OUT_STATE_LEFT_LO_NEEDED (5)
#define OUT_STATE_RIGHT_LO_NEEDED (6)
#define OUT_STATE_SYNC_ZERO_LO_NEEDED (7)
#define OUT_STATE_SYNC_ZERO_HI_NEEDED (8)
#define OUT_STATE_SYNC_MID_NEEDED (9)
#define OUT_STATE_CONTROL0 (10)
#define OUT_STATE_CONTROL1 (11)
#define OUT_STATE_CONTROL2 (12)
#define OUT_STATE_CONTROL3 (13)
#define OUT_STATE_CONTROL4 (14)
#define OUT_STATE_MON_LEFT_HI_NEEDED (15)
#define OUT_STATE_MON_LEFT_LO_NEEDED (16)
#define OUT_STATE_MON_RIGHT_HI_NEEDED (17)
#define OUT_STATE_MON_RIGHT_LO_NEEDED (18)


// #define TEST_READ (1)
// #define TEST_WRITE (1)

int complete_blocks;
// int LostSyncCount;
// int sync_gain_count;


// #define INCL_DUMP (1)
#ifdef INCL_DUMP
void Dump(FILE *ofile,                /* file handle to dump to - assumed to be      */
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
#endif

#ifdef INCL_DUMP
int read_dump_count = 0;
#endif

#ifdef TEST_READ
unsigned char test_data[4*32] = {
0x00, 0x00, 0x7f, 0xff,  /* 00000 0x0000 32767 0x7fff */
0x18, 0xf8, 0x7d, 0x89,  /* 06392 0x18f8 32137 0x7d89 */
0x30, 0xfb, 0x76, 0x40,  /* 12539 0x30fb 30272 0x7640 */
0x47, 0x1c, 0x6a, 0x6c,  /* 18204 0x471c 27244 0x6a6c */
0x5a, 0x81, 0x5a, 0x81,  /* 23169 0x5a81 23169 0x5a81 */
0x6a, 0x6c, 0x47, 0x1c,  /* 27244 0x6a6c 18204 0x471c */
0x76, 0x40, 0x30, 0xfb,  /* 30272 0x7640 12539 0x30fb */
0x7d, 0x89, 0x18, 0xf8,  /* 32137 0x7d89 06392 0x18f8 */
0x7f, 0xff, 0x00, 0x00,  /* 32767 0x7fff 00000 0x0000 */
0x7d, 0x89, 0xe7, 0x08,  /* 32137 0x7d89 -6392 0xe708 */
0x76, 0x40, 0xcf, 0x05,  /* 30272 0x7640 -12539 0xcf05 */
0x6a, 0x6c, 0xb8, 0xe4,  /* 27244 0x6a6c -18204 0xb8e4 */
0x5a, 0x81, 0xa5, 0x7f,  /* 23169 0x5a81 -23169 0xa57f */
0x47, 0x1c, 0x95, 0x94,  /* 18204 0x471c -27244 0x9594 */
0x30, 0xfb, 0x89, 0xc0,  /* 12539 0x30fb -30272 0x89c0 */
0x18, 0xf8, 0x82, 0x77,  /* 06392 0x18f8 -32137 0x8277 */
0x00, 0x00, 0x80, 0x01,  /* 00000 0x0000 -32767 0x8001 */
0xe7, 0x08, 0x82, 0x77,  /* -6392 0xe708 -32137 0x8277 */
0xcf, 0x05, 0x89, 0xc0,  /* -12539 0xcf05 -30272 0x89c0 */
0xb8, 0xe4, 0x95, 0x94,  /* -18204 0xb8e4 -27244 0x9594 */
0xa5, 0x7f, 0xa5, 0x7f,  /* -23169 0xa57f -23169 0xa57f */
0x95, 0x94, 0xb8, 0xe4,  /* -27244 0x9594 -18204 0xb8e4 */
0x89, 0xc0, 0xcf, 0x05,  /* -30272 0x89c0 -12539 0xcf05 */
0x82, 0x77, 0xe7, 0x08,  /* -32137 0x8277 -6392 0xe708 */
0x80, 0x01, 0x00, 0x00,  /* -32767 0x8001 00000 0x0000 */
0x82, 0x77, 0x18, 0xf8,  /* -32137 0x8277 06392 0x18f8 */
0x89, 0xc0, 0x30, 0xfb,  /* -30272 0x89c0 12539 0x30fb */
0x95, 0x94, 0x47, 0x1c,  /* -27244 0x9594 18204 0x471c */
0xa5, 0x7f, 0x5a, 0x81,  /* -23169 0xa57f 23169 0x5a81 */
0xb8, 0xe4, 0x6a, 0x6c,  /* -18204 0xb8e4 27244 0x6a6c */
0xcf, 0x05, 0x76, 0x40,  /* -12539 0xcf05 30272 0x7640 */
0xe7, 0x08, 0x7d, 0x89   /* -6392 0xe708 32137 0x7d89 */
};


int test_data_idx = 0;
int oframe_count = 0; // keep track of where we are in output so we can insert sync words

int testRead(unsigned char *bufp, int len) {
        int j;

        for ( j = 0; j < len; j++ ) {
                if ( oframe_count == 0 ) {  // need first byte of sync word
                        bufp[j] = 0x80;
                }
                else if ( oframe_count == 1 || oframe_count == 2 || oframe_count == 3 ) {  // 2nd byte of sync word
                        bufp[j] = 0x00;
                }
                else {  // data words
                        bufp[j] = test_data[test_data_idx];
                        ++test_data_idx;
                        if ( test_data_idx == sizeof(test_data) ) {
                        test_data_idx = 0;
                        }
                }
                ++oframe_count;
                if ( oframe_count >= 512 ) {
                        oframe_count = 0;
                }
        }

#if 0
        if ( read_dump_count < 4 ) {
                ++read_dump_count;
                Dump(stdout, bufp, len, "testRead: Buffer returned");
                fflush(stdout);
        }
#endif

#ifndef LINUX
        Sleep(12);  // assumes a 2048 byte read buffer
#else
        usleep(12000);
#endif
        return len;
 }
#endif

#ifdef TEST_WRITE
FILE *testOutF = NULL;

int testWrite(unsigned char *bufp, int len) {
        if ( testOutF == NULL ) return len;  // silently fail
        return (int)fwrite(bufp, 1, len, testOutF);
}
#endif

int bytes_processed = 0;

// swap bytes of count shorts pointed to by sp
void swapBytesShort(short *insp, int count) {

#ifndef LINUX
        if ( count <= 0 ) return;

        __asm {
                mov esi, insp
                mov ecx, count
top1:
                mov ax, [esi]
                xchg al, ah
                mov [esi], ax
                add esi, 2
                dec ecx
                jnz top1
        }


#else
        unsigned char *p;
        unsigned char tmp;
        int i;
        for ( i = 0; i < count; i++ ) {
                p = (unsigned char *)insp;
                tmp = *p;
                *p = *(p+1);
                *(p+1) = tmp;
                ++insp;
        }
        return;
#endif
}

//  get an output buffer from the fifo, don't wait, and make the buffer big endian
unsigned char *getNewOutBufFromFIFO() {
        int outbuflen;
        unsigned char *outbufp;
        int out_buf_len_needed;

        outbufp = (char *)getFIFO(OutSampleFIFOp, &outbuflen, 0); // don't wait
        if ( outbufp == NULL ) {
                return NULL;
        }
        /* else */
        out_buf_len_needed = BlockSize * 4 * sizeof(short);
        if  ( SampleRate == 96000 ) {
                out_buf_len_needed /= 2;
        }
        else if ( SampleRate == 192000 )  {
                out_buf_len_needed /= 4;
        }
        if ( outbuflen != out_buf_len_needed  ) {
                printf("Warning: IOThread short block from getFIFO on output, frame dropped obl=%d obln=%d\n", outbuflen, out_buf_len_needed);
                freeFIFOdata(outbufp);
                return NULL;
        }
        swapBytesShort((short *)outbufp, outbuflen/sizeof(short));
        return outbufp;
}

#ifdef PERF_DEBUG
typedef struct zperf_record {
                int RxFifoAvail;
                int RxFramePlaying;
                int wrote_frame;
                int ok_to_write;
                __int64 rt;
                __int64 wt;
                __int64 drt;
                __int64 dwt;
                __int64 ticks;
                int cb;
                int sync_lost;
                unsigned char RxOverrun;
                unsigned char TxOverrun;
                unsigned char frame_sending;
} PERF_RECORD;

#define PERF_DATA_COUNT (50000)

PERF_RECORD PerfData[PERF_DATA_COUNT];

int PerfDataIdx = 0;
#endif






unsigned short RxFifoAvail = 4096;  // make it large to start
unsigned char RxFramePlaying = 0;

// unsigned int NotOKtoSendCount = 0;
unsigned int SendOpprotunity = 0;

int IsOkToSendDataToFPGA(void) {
        if ( RxFifoAvail >= 512 ) {
                return 1;
        }
        else {
                ++NotOKtoSendCount;
                return 1;
        }
}

#if 0
int last_xmit_bit = 0;
#endif

// main loop of the iothread -- real work happens here
// read data out of the xylo, block it, and put it into a fifo  to go to the callback thread
//
void IOThreadMainLoop(void) {
        short *sample_bufp = NULL;
        int sample_count = 0;
        int i;
        int numread;
        int numwritten;
        int have_out_buf = 0; // do we have an output buffer to work on
        int state = STATE_NOSYNC;
        int out_state = OUT_STATE_SYNC_HI_NEEDED;
        unsigned char last_byte = 12;  // init to anything other than 0x80 or 0x00
        unsigned char second_last_byte = 12; // init to something other than 0x80 or 0x00
        int this_num = 0;
        int byte_num = 0;
        // short left_sample;
        // short right_sample;
        // short sample;
        // int in_stream_sync_count = 0; // count where we are in input stream to know when to look for sync
    // int out_stream_sync_count = 0; // where are we relative to sync in output stream
        int total_written = 0;
        unsigned char out_frame_idx = 0;

        int samples_this_sync = 0; // how many samples we've read on the current sync word - l+r = 2 samples
        int out_sample_pairs_this_sync = 0; // how many sample pairs we've written -- l+r = 1
        int frames_written_this_read = 0; // how many frames have we written on this read

#ifdef PERF_DEBUG
        __int64 read_start_t = 0;
        __int64 read_stop_t = 0;
        __int64 delta_t = 0;
        __int64 write_interval_t = 0;
        __int64 now_t = 0;
        __int64 read_t = 0;
        __int64 write_top_t;
        __int64 write_bottom_t = 0;
        __int64 last_write_t = 0;
        __int64 last_read_t = 0;
#endif

        int wrote_frame = 0;
        int ok_to_write = 0;

        int out_control_idx = 0;

        unsigned char *outbufp = NULL;
        int outbufpos; // position of next sample to be processed in outbufp
        int outbuflen; // size of *outbufp in bytes
        int writebufpos = 0;
        int sample_bufp_size;
        int sample_is_left;
        int buf_num = 0;
        unsigned char RxOverrun = 0;
        unsigned char TxOverrun = 0;
#if 1
        int dbggate = 0;
#endif

        sample_bufp_size = 4*sizeof(int)*BlockSize;

        // how big is outbuf ... account for us being fixed at 48 khz on output side
        outbuflen = 4 * sizeof(short) * BlockSize;
        if ( SampleRate == 96000 ) {
                outbuflen /= 2;
        }
        else if ( SampleRate == 192000 ) {
                outbuflen /= 4;
        }

        // printf("iot: main loop starting\n"); fflush(stdout);
        // main loop - read a buffer, processe it and then write a buffer if we have one to write
        while ( io_keep_running != 0 ) {
                // should we read - or just skip to writing
                // if we wrote a frame last time (saying we may still have data waiting to be written) around
                // and there is space in the fifo, skip the read and write another frame
                //
#ifdef WRITE_MULTI_PER_READ
                if ( !wrote_frame || (RxFifoAvail - (frames_written_this_read * 512)) < 512 ) { /* should we write */
                        printf("read top: fw: %d fa:%d\n", frames_written_this_read, RxFifoAvail); fflush(stdout);


                        frames_written_this_read = 0;
#endif

                        // first read data and process it
#ifdef PERF_DEBUG
                        read_start_t = getPerfTicks();
#endif
#ifndef TEST_READ
                        // numread = XyloBulkRead(XyloH, 4, read_buf, sizeof(read_buf));
#ifdef XYLO
                        numread = XyloBulkRead(XyloH, 4, FPGAReadBufp, FPGAReadBufSize);
#endif
#ifdef OZY
                        numread = OzyBulkRead(OzyH, 0x86, FPGAReadBufp, FPGAReadBufSize);
                        if ( numread <= 0 ) {
#ifndef LINUX
                                Sleep(10); /* this sleep is here to keep us from sucking all the CPU
                                                      on the machine when the USB cable is pullled out of the
                                                          computer while we're running -- without pausing here we get into
                                                          a tight loop reading from a device that is gone -- the sleep makes it
                                                          a less tight loop and leaves some cycles for the user to turn off PowerSDR
                                                          An ugly hack, but does work more or less

                                                          To fix this for real we need to be able to send some sort of error indication
                                                          thru the callback to tell the C# side that the sound card MIA
                                                        */
#else
                                usleep(10000);
#endif
                                fprintf(stdout, "OzyBulkRead failed rc=%d\n", numread);  fflush(stdout);
                        }
                        ++dbggate;
                        if ( dbggate == 10000 ) {
                                dbggate = 0;
                                fprintf(stdout, "OBR: numread=%d\n", numread); fflush(stdout);
                        }


#endif
                        // printf("iot: read %d bytes\n", numread); fflush(stdout);
                        ++buf_num;

#ifdef SAVE_READS_TO_BUFFER
                                memcpy(ReadSaveBuf[SaveReadsIdx], FPGAReadBufp, FPGAReadBufSize);
                                ++SaveReadsIdx;
                                if ( SaveReadsIdx >= NUM_READ_BUFS_TO_SAVE ) {
                                        SaveReadsIdx = 0;
                                }
#endif


#if 0
                        if ( read_dump_count < 5 ) {
                                ++read_dump_count;
                                Dump(stdout, FPGAReadBufp, FPGAReadBufSize, "Input Buffer from Xylo");  fflush(stdout);
                        }
#endif
                        // XyloBulkWrite(XyloH, 2, read_buf, numread);
#else
                        numread = testRead(FPGAReadBuf, FPGAReadBufSize);
#endif
#ifdef PERF_DEBUG
                        read_stop_t = getPerfTicks();
                        read_t = read_stop_t - read_start_t;
                        // printf("d1: %I64d\n", delta_t);
                        // delta_t = perfTicksToNanos(delta_t);
                        // printf("d2: %I64d\n", delta_t); fflush(stdout);
                        updateHLA(&ReadHLA, read_t);
#endif
                        for ( i = 0; i < numread; i++ ) {
                                ++bytes_processed;

#if 0
                                if ( state != STATE_NOSYNC && second_last_byte == 0x7f && last_byte == 0x7f && FPGAReadBufp[i] == 0x7f ) {
                                        printf("\nsync in data bufnum: %d i: %d samples_this_sync: %d\n", buf_num, i, samples_this_sync);
                                }
#endif
                                switch ( state ) {
                                        case STATE_NOSYNC:
                                                if ( second_last_byte == 0x7f && last_byte == 0x7f && FPGAReadBufp[i] == 0x7f ) {  // found sync
                                                        // in_stream_sync_count = 2; // we've consumed 3 bytes already, it will bump at bottom of loop to make 3
                                                        state = STATE_CONTROL0;
                                                        samples_this_sync = 0;
                                                        ++SyncGainedCount;
                                                        HaveSync = 1;
                                                        // printf("sync gained: bufnum: %d i: %d\n", buf_num, i);
                                                        // printf("\nSG");
                                                }
                                                break;

                                        case STATE_CONTROL0:
                                                // printf(" C0");
                                                ControlBytesIn[0] = FPGAReadBufp[i];
                                                DotDashBits = FPGAReadBufp[i] & 0x3;


#if 0
                                                ++dbggate;
                                                if ( dbggate == 1000 ) {
                                                        printf("cb0: %d ddb: %d\n", ControlBytesIn[0], DotDashBits); fflush(stdout);
                                                        dbggate = 0;
                                                }
#endif
                                                state = STATE_CONTROL1; // look for 2nd control byte
                                                break;
                                        case STATE_CONTROL1:
                                                // printf(" C1");
                                                ControlBytesIn[1] = FPGAReadBufp[i];
                                                TxOverrun = FPGAReadBufp[i];
                                                state = STATE_CONTROL2; // look for 3rd control byte
                                                break;
                                        case STATE_CONTROL2:
                                                // printf(" C2");
                                                ControlBytesIn[2] = FPGAReadBufp[i];
                                                RxOverrun = FPGAReadBufp[i];
                                                state = STATE_CONTROL3; // look for 4th control byte
                                                break;
                                        case STATE_CONTROL3:  //
                                                // printf(" C3");
                                                ControlBytesIn[3] = FPGAReadBufp[i];
                                                RxFifoAvail = ((unsigned char)FPGAReadBufp[i]) << 4;
                                                state = STATE_CONTROL4;  // look for 5th and last control byte
                                                break;
                                        case STATE_CONTROL4:
                                                // printf(" C4");
                                                ControlBytesIn[4] = FPGAReadBufp[i];
                                                RxFramePlaying = (unsigned char)FPGAReadBufp[i];
                                                state = STATE_SAMPLE_HI;
                                                sample_is_left = 1;
                                                // printf("fa: %d fp: %d\n", RxFifoAvail, RxFramePlaying);
                                                break;
                                        case STATE_SAMPLE_HI:
                                                // printf(" HI");
                                                this_num = ((int)FPGAReadBufp[i]) << 16; // start new sample, preserve the sign bit!
                                                state = STATE_SAMPLE_MID;
                                                break;
                                        case STATE_SAMPLE_MID:
                                                // printf(" MI");
                                                this_num = (((unsigned char)FPGAReadBufp[i]) <<8) | this_num;  // add in middle part of sample -- unsigned!
                                                state = STATE_SAMPLE_LO;
                                                break;
                                        case STATE_SAMPLE_LO:
                                                // printf(" LO");
                                                this_num = ((unsigned char)FPGAReadBufp[i]) | this_num; // add in last part of sample
                                                IOSampleInBufp[sample_count] = this_num;
                                                ++sample_count;
#if 0
                                                if ( sample_count >= 2*BlockSize ) {
                                                        sample_count = 0;
                                                        // putFIFO(InSampleFIFOp, IOSampleInBufp, sample_bufp_size);
                                                        Callback_ProcessBuffer(IOSampleInBufp, sample_bufp_size);
                                                        ++complete_blocks;
                                                }
#endif
                                                ++samples_this_sync;
#if 0
                                                if ( samples_this_sync == 168 ) {  // we're due for another sync word
                                                        state = STATE_SYNC_HI;
                                                }
#endif
                                                if ( sample_is_left ) {  // we just did the left sample - get the right one
                                                        state = STATE_SAMPLE_HI;
                                                }
                                                else {   // we just finished a 24 bit right sample - time to get the 16 bit mic sample
                                                        state = STATE_SAMPLE_MIC_HI;
                                                }

                                                //if ( in_stream_sync_count == 0 ) {
                                                //      state = STATE_SYNC_HI;
                                                //}
                                                //else {
                                                //      state = STATE_SAMPLE_HI;
                                                //}
                                                if ( sample_is_left ) {
                                                        sample_is_left = 0;
                                                }
                                                else {
                                                        sample_is_left = 1;
                                                }
                                                break;

                                        case STATE_SAMPLE_MIC_HI:
                                                this_num = ((int)FPGAReadBufp[i]) << 8;  // read hi word of sample - preserve sign moron!
                                                state = STATE_SAMPLE_MIC_LO;
                                                break;

                                        case STATE_SAMPLE_MIC_LO:
                                                this_num = ((unsigned char)FPGAReadBufp[i]) | this_num; // add in last part of sample
                                                IOSampleInBufp[sample_count] = this_num;
                                                ++sample_count;
                                                if ( sample_count >= 3*BlockSize ) {   // only 3 channels on input (i,q,nic), although buffer is sized for 4
                                                        sample_count = 0;
                                                        // printf("iot: calling proc buf\n"); fflush(stdout);
#if 1
                                                        Callback_ProcessBuffer(IOSampleInBufp, sample_bufp_size);
#endif
                                                        // printf("iot: proc buf returned\n"); fflush(stdout);
                                                        ++complete_blocks;
                                                }
                                                ++samples_this_sync;
                                                if ( samples_this_sync ==  189 ) {
                                                        state = STATE_SYNC_HI;
                                                }
                                                else {
                                                        state = STATE_SAMPLE_HI;
                                                }
                                                sample_is_left = 1;
                                                break;


                                        case STATE_SYNC_HI:
                                                // printf("\nSH");
#if 0
                                                ++dbggate;

/// dbg
                                                if ( dbggate >= 1024 ) {  // force sync loss
                                                        FPGAReadBufp[i] = 0xff;
                                                }
                                                if ( dbggate >= 1100 ) {
                                                        dbggate = 0;
                                                }
///
#endif
                                                samples_this_sync = 0;
                                                if ( (unsigned char)FPGAReadBufp[i] != 0x7f ) { // argh did not find sync
                                                        ++LostSyncCount;
                                                        state = STATE_NOSYNC;
                                                        HaveSync = 0;
                                                }
                                                else {
                                                        state = STATE_SYNC_MID;
                                                }
                                                break;
                                        case STATE_SYNC_MID:
                                                // printf(" SM");
                                                if ( FPGAReadBufp[i] != 0x7f ) { // argh did not find sync
                                                        ++LostSyncCount;
                                                        state = STATE_NOSYNC;
                                                        HaveSync = 0;
                                                }
                                                else {
                                                        state = STATE_SYNC_LO;
                                                }
                                                break;
                                        case STATE_SYNC_LO:
                                                // printf(" SL");
                                                if ( FPGAReadBufp[i] != 0x7f ) { // argh did not find sync
                                                        ++LostSyncCount;
                                                        state = STATE_NOSYNC;
                                                        HaveSync = 0;
                                                }
                                                else {
                                                        state = STATE_CONTROL0;
                                                }
                                                break;

                                        default:
                                                printf("io-thead: internal error, bad state in read loop\n");
                                                state = STATE_NOSYNC;
                                                break;

                                }
                                //++in_stream_sync_count;
                                //if ( in_stream_sync_count >= 512 ) {
                                //      in_stream_sync_count = 0;
                                //}
                                second_last_byte = last_byte;
                                last_byte = FPGAReadBufp[i];

                        } /* for numread = ... */
#ifdef WRITE_MULTI_PER_READ
                }/* should we write */
#endif


                // ok now that we've handled the inbound block send outbound data if we have any
                //
                // printf("out top: out_sync: %d out_state: %d total_written: %d\n", out_stream_sync_count, out_state, total_written); fflush(stdout);
#ifdef PERF_DEBUG
                write_top_t = getPerfTicks();
#endif
                wrote_frame = 0;

                if ( !have_out_buf ) {
                        outbufp = getNewOutBufFromFIFO();
                        if ( outbufp != NULL ) {
                                have_out_buf = 1;
                        }
                        outbufpos = 0;
                }

                ++SendOpprotunity;
                ok_to_write = IsOkToSendDataToFPGA();
                // printf("write top: okw: %d\n", ok_to_write); fflush(stdout);
                if ( have_out_buf && ok_to_write  ) {
                        // printf("enter out loop /w buf: out_sync: %d out_state: %d total_written: %d\n", out_stream_sync_count, out_state, total_written); fflush(stdout);
                        // while we have space in the output buffer put data in it
                        while ( writebufpos < FPGAWriteBufSize /*OUTBUF_SIZE*/ ) {
                                switch ( out_state ) {
                                                case OUT_STATE_SYNC_HI_NEEDED:
                                                        out_sample_pairs_this_sync = 0;
                                                        out_state = OUT_STATE_SYNC_MID_NEEDED;
                                                        FPGAWriteBufp[writebufpos] = 0x7f;
                                                        break;

                                                case OUT_STATE_SYNC_MID_NEEDED:
                                                        out_state = OUT_STATE_SYNC_LO_NEEDED;
                                                        FPGAWriteBufp[writebufpos] = 0x7f;
                                                        break;

                                                case OUT_STATE_LEFT_HI_NEEDED:
                                                        out_state = OUT_STATE_LEFT_LO_NEEDED;
                                                        FPGAWriteBufp[writebufpos] = outbufp[outbufpos];
                                                        ++outbufpos;
                                                        break;

                                                case OUT_STATE_RIGHT_HI_NEEDED:
                                                        out_state = OUT_STATE_RIGHT_LO_NEEDED;
                                                        FPGAWriteBufp[writebufpos] = outbufp[outbufpos];
                                                        ++outbufpos;
                                                        break;

                                                case OUT_STATE_SYNC_LO_NEEDED:
                                                        out_state = OUT_STATE_CONTROL0;
                                                        FPGAWriteBufp[writebufpos] = 0x7f;
                                                        break;

                                                case OUT_STATE_CONTROL0:
                                                        out_state = OUT_STATE_CONTROL1;
                                                        // write_buf[writebufpos] = ControlBytesIn[0];
#if 0
                                                        if ( XmitBit != last_xmit_bit ) {
                                                                last_xmit_bit = XmitBit;
                                                                printf("XmitBit changed to: %d\n", XmitBit); fflush(stdout);
                                                        }
#endif


                                                        FPGAWriteBufp[writebufpos] = (unsigned char)XmitBit;
                                                        FPGAWriteBufp[writebufpos] &= 1; // not needed ?
                                                        if ( out_control_idx == 1 ) {  // send freq
                                                                FPGAWriteBufp[writebufpos] |= 2;
                                                        }
                                                        ControlBytesOut[0] = FPGAWriteBufp[writebufpos];

                                                        break;

                                                case OUT_STATE_CONTROL1:
                                                        out_state = OUT_STATE_CONTROL2;
                                                        // send sample rate in C1 low 2 bits
                                                        // FPGAWriteBufp[writebufpos] = ((ControlBytesIn[1] & 0xfc) | (SampleRateIn2Bits & 3));
                                                        if ( out_control_idx == 1 ) {
                                                                        FPGAWriteBufp[writebufpos] =  (VFOfreq >> 24) & 0xff; // byte 0 of freq
                                                        }
                                                        else {
                                                                        FPGAWriteBufp[writebufpos] =  (SampleRateIn2Bits & 3);
                                                        }

                                                        ControlBytesOut[1] = FPGAWriteBufp[writebufpos];


#if 0
                                                        ++dbggate;
                                                        if ( dbggate == 1000 ) {
                                                                dbggate = 0;
                                                                printf("c1: 0x%x\n", FPGAWriteBufp[writebufpos]);  fflush(stdout);
                                                        }
#endif
                                                        break;

                                                case OUT_STATE_CONTROL2:
                                                        out_state = OUT_STATE_CONTROL3;
                                                        if ( out_control_idx == 1 ) {
                                                                FPGAWriteBufp[writebufpos] = (VFOfreq >> 16) & 0xff;
                                                        }
                                                        else {
                                                                FPGAWriteBufp[writebufpos] = 0; /* ControlBytesIn[2];  */
                                                        }
                                                        ControlBytesOut[2] = FPGAWriteBufp[writebufpos];
                                                        break;

                                                case OUT_STATE_CONTROL3:
                                                        out_state = OUT_STATE_CONTROL4;
                                                        if ( out_control_idx == 1 ) {
                                                                FPGAWriteBufp[writebufpos] = (VFOfreq >> 8) & 0xff;
                                                        }
                                                        else {
                                                                FPGAWriteBufp[writebufpos] = 0; /* ControlBytesIn[3];  */
                                                        }
                                                        ControlBytesOut[3] = FPGAWriteBufp[writebufpos];
                                                        break;

                                                case OUT_STATE_CONTROL4:
                                                        out_state = OUT_STATE_LEFT_HI_NEEDED;
                                                        // write_buf[writebufpos] = ControlBytesIn[4];
                                                        if ( out_control_idx == 1 ) {
                                                                FPGAWriteBufp[writebufpos] = VFOfreq & 0xff;
                                                        }
                                                        else {
                                                                FPGAWriteBufp[writebufpos] = out_frame_idx;
                                                        }
                                                        ControlBytesOut[4] = FPGAWriteBufp[writebufpos];
                                                        ++out_frame_idx;
                                                        if ( out_control_idx == 1 ) {
                                                                out_control_idx = 0;
                                                        }
                                                        else {
                                                                out_control_idx = 1;
                                                        }
                                                        break;

                                                case OUT_STATE_LEFT_LO_NEEDED:
                                                        out_state = OUT_STATE_RIGHT_HI_NEEDED;
                                                        FPGAWriteBufp[writebufpos] = outbufp[outbufpos];
                                                        ++outbufpos;
                                                        break;

                                                case OUT_STATE_RIGHT_LO_NEEDED:
                                                        //if ( out_stream_sync_count == 511 ) {
                                                        //      out_state = OUT_STATE_SYNC_HI_NEEDED;
                                                        //}
                                                        //else {
                                                        //      out_state = OUT_STATE_LEFT_HI_NEEDED;
                                                        //}
                                                                FPGAWriteBufp[writebufpos] = outbufp[outbufpos];
                                                                ++outbufpos;
#if 0
                                                                ++out_sample_pairs_this_sync;
                                                                if ( out_sample_pairs_this_sync == 126 ) {
                                                                        out_state = OUT_STATE_SYNC_HI_NEEDED;
                                                                }
                                                                else {
                                                                        out_state = OUT_STATE_LEFT_HI_NEEDED;
                                                                }
#else
                                                        out_state = OUT_STATE_MON_LEFT_HI_NEEDED;
#endif
                                                        break;

                                                case OUT_STATE_MON_LEFT_HI_NEEDED:
                                                        out_state = OUT_STATE_MON_LEFT_LO_NEEDED;
                                                        FPGAWriteBufp[writebufpos] = outbufp[outbufpos];
                                                        ++outbufpos;
                                                        break;

                                                case OUT_STATE_MON_LEFT_LO_NEEDED:
                                                        out_state = OUT_STATE_MON_RIGHT_HI_NEEDED;
                                                        FPGAWriteBufp[writebufpos] = outbufp[outbufpos];
                                                        ++outbufpos;
                                                        break;

                                                case OUT_STATE_MON_RIGHT_HI_NEEDED:
                                                        out_state = OUT_STATE_MON_RIGHT_LO_NEEDED;
                                                        FPGAWriteBufp[writebufpos] = outbufp[outbufpos];
                                                        ++outbufpos;
                                                        break;

                                                case OUT_STATE_MON_RIGHT_LO_NEEDED:
                                                        FPGAWriteBufp[writebufpos] = outbufp[outbufpos];
                                                        ++outbufpos;
                                                        ++out_sample_pairs_this_sync;
                                                        if ( out_sample_pairs_this_sync == 63 ) {
                                                                out_state = OUT_STATE_SYNC_HI_NEEDED;
                                                        }
                                                        else {
                                                                out_state = OUT_STATE_LEFT_HI_NEEDED;
                                                        }
                                                        break;

                                                default:
                                                        printf("internal error - bad out_state\n");
                                                        out_state = OUT_STATE_SYNC_HI_NEEDED;
                                                        break;
                                }       /* switch (out_state) */

                                ++writebufpos;
                                //++out_stream_sync_count;
                                //if ( out_stream_sync_count >=  512  ) {
                                //      out_stream_sync_count = 0;
                                //}
                                if ( outbufpos >= outbuflen ) {  // we've consumed output buf from powrsdr dsp -- get a new one if one avail
                                        freeFIFOdata(outbufp);
                                        outbufp = getNewOutBufFromFIFO();
                                        if ( outbufp == NULL ) {  // no data bail out
                                                have_out_buf = 0;
                                                // printf("no new buffer - bail out: out_sync: %d out_state: %d total_written: %d\n", out_stream_sync_count, out_state, total_written);  fflush(stdout);
                                                break;
                                        }
                                        else {
                                                have_out_buf = 1;
                                                outbufpos = 0;
                                                // printf("new buffer: out_sync: %d out_state: %d total_written: %d\n", out_stream_sync_count, out_state, total_written); fflush(stdout);
                                        }
                                }
                        }  /* while writebufpos < FPGAWriteBufSize */

                        if ( writebufpos >= FPGAWriteBufSize ) {  // write the buffer if we've filled it.
                                wrote_frame = 1;
                                if ( !ForceNoSend ) {
#ifdef XYLO
                                        numwritten = XyloBulkWrite(XyloH, 2, FPGAWriteBufp, FPGAWriteBufSize);
#endif
#ifdef OZY
                                        numwritten = OzyBulkWrite(OzyH, 0x02, FPGAWriteBufp, FPGAWriteBufSize);


#endif
                                }
                                else {
                                        numwritten = FPGAWriteBufSize;
                                }
                                // numwritten = OUTBUF_SIZE;

#ifdef SAVE_WRITES_TO_BUFFER
                                memcpy(WriteSaveBuf[SaveWritesIdx], FPGAWriteBufp, FPGAWriteBufSize);
                                ++SaveWritesIdx;
                                if ( SaveWritesIdx >= NUM_WRITE_BUFS_TO_SAVE ) {
                                        SaveWritesIdx = 0;
                                }

#endif


#ifdef PERF_DEBUG
                                now_t = getPerfTicks();
                                if ( last_write_t != 0 ) {
                                        write_interval_t = now_t - last_write_t;
                                        updateHLA(&WriteHLA, write_interval_t);
                                }
                                last_write_t = now_t;
#endif
#ifdef TEST_WRITE
                                /* numwritten = */  testWrite(write_buf, OUTBUF_SIZE);
#endif
                                total_written += numwritten;
#ifdef WRITE_MULTI_PER_READ
                                ++frames_written_this_read;
#endif
//                              printf("buffer write: out_sync: %d out_state: %d total_written: %d\n", out_stream_sync_count, out_state, total_written); fflush(stdout);

                                if ( numwritten != FPGAWriteBufSize ) {
                                        printf("warning: iothread short write! numwritten=%d\n", numwritten);
                                }
                                writebufpos = 0;
                        }
                } /* if have_out_buf */
#ifdef PERF_DEBUG
                write_bottom_t = getPerfTicks();


                if ( PerfDataIdx < PERF_DATA_COUNT ) {
                        PERF_RECORD *pdp = &(PerfData[PerfDataIdx]);
                        ++PerfDataIdx;
                        pdp->RxFifoAvail = RxFifoAvail;
                        pdp->RxFramePlaying = RxFramePlaying;
                        pdp->wrote_frame = wrote_frame;
                        pdp->ok_to_write = ok_to_write;
                        pdp->rt = read_t;
                        pdp->wt = write_bottom_t - write_top_t;
                        pdp->drt = read_stop_t - last_read_t;
                        pdp->dwt = write_bottom_t - last_write_t;
                        pdp->ticks = now_t;
                        pdp->cb = complete_blocks;
                        pdp->RxOverrun = RxOverrun;
                        pdp->TxOverrun = TxOverrun;
                        pdp->frame_sending = out_frame_idx;
                        pdp->sync_lost = LostSyncCount;
                }


                //printf("fa: %d fp: %d w: %d okw: %d rt: %I64d wt: %I64d drt: %I64d dwt: %I64d cb: %d\n",
                //          RxFifoAvail, RxFramePlaying, wrote_frame, ok_to_write, perfTicksToNanos(read_t), perfTicksToNanos(write_bottom_t - write_top_t),
                //              perfTicksToNanos(read_stop_t - last_read_t), perfTicksToNanos(write_bottom_t - last_write_t), complete_blocks);
                last_read_t = read_stop_t;
                last_write_t = read_stop_t;
#endif
        }  /* while keep_running */
}




// stops and kill's the IOThread
// returns 0 on success, !0 on failure
// error returns
// 1 - thread not running
// 2 - pthread_join failed
int IOThreadStop() {
        int rc;
        void *junk;

        if ( io_keep_running == 0 ) {  // not running
                return 1;
        }
        io_keep_running = 0;  // flag to stop
        rc = pthread_join(IOThreadID, &junk);
        if ( rc != 0 ) {
                fprintf(stderr, "Warning: io-thread.c, pthread_join failed with rc=%d\n", rc);
                return 2;
        }
        return 0;
}


// this is the main thread that reads/writes data to/from the Xylo
// when this is called the Xylo device is open
// this routine does setup, calls IOThreadMainLoop to do the real work and cleans up on termination
void *IOThreadMain(void *argp) {
                io_keep_running = 1;
                IOThreadRunning = 1;
#ifndef LINUX
                SetThreadPriority(GetCurrentThread(), /* THREAD_PRIORITY_ABOVE_NORMAL */  THREAD_PRIORITY_TIME_CRITICAL /* THREAD_PRIORITY_HIGHEST  */ );
#else
#warning message("info - LINUX code missing ... set priority!")
#endif
            sem_post(&IOThreadInitSem); // tell launching thread we're rockin and rollin
                complete_blocks = 0;
                LostSyncCount = 0;
                SyncGainedCount = 0;
                bytes_processed = 0;
                SendOpprotunity = 0;
                NotOKtoSendCount = 0;


#ifdef TEST_WRITE
                testOutF = fopen("testout.dat", "wb");
#endif
#ifdef PERF_DEBUG
                initHLA(&ReadHLA);
                initHLA(&InConvertHLA);
        initHLA(&OutConvertHLA);
        initHLA(&CallbackHLA);
                initHLA(&WriteHLA);
#endif

                IOThreadMainLoop();
                IOThreadRunning = 0;
#ifdef TEST_WRITE
                if ( testOutF != NULL )  {
                        fclose(testOutF);
                        testOutF = NULL;
                }
#endif
                printf("iothread: complete_blocks: %d\niothread: LostSyncCount: %d\nbytes_processed: %d\nSyncGainedCount: %d\nsend opprotunities: %d\nNotOKTosendCount: %d\n",
                                      complete_blocks, LostSyncCount, bytes_processed, SyncGainedCount, SendOpprotunity, NotOKtoSendCount);
                fflush(stdout);

#ifdef SAVE_WRITES_TO_BUFFER
                {
                        //
                        // dump out the last set of buffers we wrote to the fpga
                        // save_writes_idx points to next buffer to fill, so we start with the buffer immediately following it and
                        // dump buffs until we get
                        FILE *buf_file;
                        int curridx;

                        buf_file = fopen("fpga-writes.dat", "wb");
                        if ( buf_file != NULL ) {
                                curridx = SaveWritesIdx + 1;
                                if ( curridx >= NUM_WRITE_BUFS_TO_SAVE ) {
                                        curridx = 0;
                                }
                                while ( curridx != SaveWritesIdx ) {
                                        fwrite(WriteSaveBuf[curridx], 1, FPGAWriteBufSize /*OUTBUF_SIZE*/, buf_file);
                                        ++curridx;
                                        if ( curridx >= NUM_WRITE_BUFS_TO_SAVE ) {
                                                curridx = 0;
                                        }
                                }
                                fclose(buf_file);
                        }
                }
#endif

#ifdef SAVE_READS_TO_BUFFER
                {
                        //
                        // dump out the last set of buffers we wrote to the fpga
                        // save_writes_idx points to next buffer to fill, so we start with the buffer immediately following it and
                        // dump buffs until we get
                        FILE *buf_file;
                        int curridx;

                        buf_file = fopen("fpga-reads.dat", "wb");
                        if ( buf_file != NULL ) {
                                curridx = SaveReadsIdx + 1;
                                if ( curridx >= NUM_READ_BUFS_TO_SAVE ) {
                                        curridx = 0;
                                }
                                while ( curridx != SaveReadsIdx ) {
                                        fwrite(ReadSaveBuf[curridx], 1, FPGAReadBufSize /*INBUF_SIZE*/ , buf_file);
                                        ++curridx;
                                        if ( curridx >= NUM_READ_BUFS_TO_SAVE ) {
                                                curridx = 0;
                                        }
                                }
                                fclose(buf_file);
                        }
                }
#endif


#ifdef PERF_DEBUG
                // printHLA(&ReadHLA, /* stdout, */ "ReadHLA (perfticks): ");
                // ReadHLA.sum = perfTicksToNanos(ReadHLA.sum);
                // ReadHLA.hi = perfTicksToNanos(ReadHLA.hi);
                // ReadHLA.lo = perfTicksToNanos(ReadHLA.lo);

                printHLANano(&ReadHLA, /* stdout, */ "ReadHLA (ns): ");

                printHLANano(&InConvertHLA, "InConvert (ns): ");
                // printHLA(&InConvertHLA, "InConvert: ");

                printHLANano(&OutConvertHLA, "OutConvert (ns): ");
                // printHLA(&OutConvertHLA, "OutConvert: ");

                printHLANano(&CallbackHLA, "Callback (ns): ");
                printHLANano(&WriteHLA, "Write Interval (ns): ");
                // printHLA(&CallbackHLA, "Callback: ");

                // dump out the detailed data

                if ( PerfDataIdx > 0 ) {
                        FILE *f;
                        int i;
                        __int64 last_nanos = 0;
                        __int64 now_nanos;
                        PERF_RECORD *pdp;
                        f = fopen("perfdata.txt", "w");
                        if ( f != NULL ) {
                                fprintf(f, "iothread: complete_blocks: %d\niothread: LostSyncCount: %d\nbytes_processed: %d\nSyncGainedCount: %d\nsend opprotunities: %d\nNotOKTosendCount: %d\n",
                                     complete_blocks, LostSyncCount, bytes_processed, SyncGainedCount, SendOpprotunity, NotOKtoSendCount);
                                for ( i = 0; i < PerfDataIdx; i++ ) {
                                        pdp = &(PerfData[i]);
                                        now_nanos = perfTicksToNanos(pdp->ticks);
                                        fprintf(f,"i: %d fa: %d fp: %3d fs: %3d sl: %d txo: %3d rxo: %3d w: %d okw: %d rt: %8I64u wt: %8I64d drt: %8I64u dwt: %8I64u cb: %d t: %I64u dt: %8I64u\n",  i,
                                        pdp->RxFifoAvail, pdp->RxFramePlaying, pdp->frame_sending, pdp->sync_lost, pdp->TxOverrun, pdp->RxOverrun, pdp->wrote_frame, pdp->ok_to_write, perfTicksToNanos(pdp->rt), perfTicksToNanos(pdp->wt),
                                        perfTicksToNanos(pdp->drt), perfTicksToNanos(pdp->dwt), pdp->cb, now_nanos, now_nanos-last_nanos );
                                        last_nanos = now_nanos;
                                }
                                fclose(f);
                        }
                }
#endif
                return NULL;
}

