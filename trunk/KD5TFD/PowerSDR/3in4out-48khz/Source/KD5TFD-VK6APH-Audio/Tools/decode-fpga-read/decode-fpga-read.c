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
    result = ((char)framep[ofs]) <<16;  /* need to sign extend! */ 
    result |= framep[ofs+1] << 8;
    result |= framep[ofs+2];
    return result;
}


float IQConversionDivisor; 


struct ControlFrame { 
	int frame_num; 
	unsigned char c0; 
	unsigned char c1; 
	unsigned char c2; 
	unsigned char c3; 
	unsigned char c4; 
	unsigned char pad; /* even align things */ 
}; 


#define MAX_CONTROL_FRAMES (1000) 
struct ControlFrame ControlFrames[1000]; 
int NumControlFrames = 0; 


void addControlFrame(unsigned char *framep, int fnum) { 
	if ( NumControlFrames >= MAX_CONTROL_FRAMES ) return; 
	
	ControlFrames[NumControlFrames].frame_num = fnum; 
	ControlFrames[NumControlFrames].c0 = framep[3];
	ControlFrames[NumControlFrames].c1 = framep[4];
	ControlFrames[NumControlFrames].c2 = framep[5];
	ControlFrames[NumControlFrames].c3 = framep[6];
	ControlFrames[NumControlFrames].c4 = framep[7];
	++NumControlFrames; 
	return; 
} 

int I_Histo[256]; 
int Q_Histo[256]; 
int I_pos = 0; 
int I_neg = 0; 
int Q_pos = 0; 
int Q_neg = 0; 
int I_odd = 0; 
int I_even = 0; 
int Q_odd = 0; 
int Q_even = 0; 
long I_sum = 0; 
long Q_sum = 0; 
int SampleCount = 0; 


void dumpFrame(unsigned char *framep, int frame_num) {
    int i, q, mic;
	float i_f, q_f; 
    unsigned int sync;
    int ofs = 8;
	unsigned char histo_idx; 
    sync = framep[0] << 16;
    sync |= framep[1] << 8;
    sync |= framep[2];
    if ( sync != 0x7f7f7f ) {
        printf("bad sync\n");
    }
	else { 
		addControlFrame(framep, frame_num); 
	} 
    while ( ofs < FRAME_SIZE ) {
        i = getInt24FromFrame(framep, ofs);
        q = getInt24FromFrame(framep, ofs+3);
        mic = getShortFromFrame(framep, ofs+6);
		i_f = (float)i / IQConversionDivisor; 
		q_f = (float)q / IQConversionDivisor; 
#if 0 
		CallbackInLbufp[i] = ((float)(bufp[3*i]))/IQConversionDivisor;  
		CallbackInRbufp[i] =  ((float)(bufp[(3*i)+1]))/IQConversionDivisor; 
#endif 
        printf("%10d %10d %10d %6d %10f %10f\n", frame_num, i, q, mic, i_f, q_f);

		if ( i < 0 ) { 
			++I_neg; 
		}
		else if ( i > 0 )  { 
			++I_pos; 
		}
		if ( i & 1 ) { 
			++I_odd; 
		} 
		else { 
			++I_even; 
		} 
		if ( q & 1 ) { 
			++Q_odd; 
		} 
		else { 
			++Q_even; 
		}
		if ( q < 0 ) { 
			++Q_neg; 
		}
		else if ( q > 0 )  { 
			++Q_pos; 
		}
		histo_idx = ( i & 0xff); 
		++I_Histo[histo_idx]; 
		histo_idx = ( q & 0xff ); 
		++Q_Histo[histo_idx]; 
		++SampleCount; 
		I_sum += i; 
		Q_sum += q; 

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
	int frame_num = 0; 
	int i; 
	int i_avg; 
	int q_avg; 
	IQConversionDivisor = (float)8388607.0; /* (2**24)-1 */ 
	for ( i = 0; i < 256; i++ ) { 
		I_Histo[i] = 0; 
		Q_Histo[i] = 0; 
	} 

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

	printf("    Frame#          I          Q    Mic    I float    Q float\n"); 

	while ( 1 ) { 
		numread = fread(ibuf, 1, 1, ifile); 
		// printf("read 1: 0x%x\n", ibuf[0]); 
		if ( numread != 1 ) break; // short read or end of file 
		if ( l0 == 0x7f && l1 == 0x7f && ibuf[0] == 0x7f ) {  // gained sync 
			fbuf[0] = 0x7f; fbuf[1] = 0x7f; fbuf[2] = 0x7f; 
			numread = fread(fbuf+3, 1, FRAME_SIZE-3, ifile); 
			// printf("read FS-3: numread=%d\n", numread); 
			if ( numread != FRAME_SIZE-3 ) break; // no more data 
			dumpFrame(fbuf, ++frame_num); 
			if ( no_sync_bytes > 2 ) { 
				++missing_sync_count;
				// printf("bumped missing_sync_count no_sync_bytes=%d\n", no_sync_bytes); 
			} 
			no_sync_bytes = 0; 
			++good_frame_count; 			
			l0 = 0; l1 = 0; 
		} 
		else { 
			l0 = l1; 
			l1 = ibuf[0]; 
			++no_sync_bytes;
			// printf("bumped no sync\n"); 
		} 
	} 

	printf("\n\ngood frame count: %d\nmissing sync count: %d\n\nStatistics\n", good_frame_count, missing_sync_count); 
	i_avg = (int)((long)I_sum/SampleCount); 
	q_avg = (int)((long)Q_sum/SampleCount); 

	printf("I: Avg: %d Positive: %d Negative: %d Odd: %d Even: %d\n", i_avg, I_pos, I_neg, I_odd, I_even); 
	printf("Q: Avg: %d Positive: %d Negative: %d Odd: %d Even: %d\n", q_avg, Q_pos, Q_neg, Q_odd, Q_even); 
	printf("\nHistogran of low 8 bits\n"); 
	printf("value I count  Q count\n");          	

	for ( i = 0; i < 256; i++ ) { 
		printf("%4d %8d %8d\n", i, I_Histo[i], Q_Histo[i]); 
	} 
	

	printf("\nControl Data\n Frame   c0   c1   c2   c3   c4\n"); 
	for ( i = 0; i <  NumControlFrames; i++ ) { 
		printf("%6d 0x%02x 0x%02x 0x%02x 0x%02x 0x%02x\n", ControlFrames[i].frame_num, ControlFrames[i].c0,  ControlFrames[i].c1, 
			                                                ControlFrames[i].c2,  ControlFrames[i].c3,  ControlFrames[i].c4); 

	} 



    writeMicDataBuf();
    if ( MicDataFile != NULL ) fclose(MicDataFile);
    fclose(ifile);
}
