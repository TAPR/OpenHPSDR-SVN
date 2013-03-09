/*  analyzer.h

This file is part of a program that implements a Spectrum Analyzer
used in conjunction with software-defined-radio hardware.

Copyright (C) 2012, 2013 Warren Pratt, NR0V

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

The author can be reached by email at  

warren@wpratt.com

*/

#include "fftw3.h"
#include <malloc.h>
#include <math.h>
#include <string.h>
#include <process.h>
//#include <intrin.h>
#include <Windows.h>
#include <WinCon.h>

#define PI					3.1415926535897932
#define MAX_SIZE			262144			//maximum number or real or complex input samples
#define SAMP_BUFF_MULT		2				//ratio of input sample buffer size to fft size (for overlap)
#define MAX_NUM_FFT			4				//maximum number of ffts for an elimination
#define MAX_PIXELS			2560			//maximum number of pixels that can be requested
#define NUM_PIXEL_BUFFS		3				//number of pixel output buffers
#define MAX_AVERAGE			60				//maximum number of pixel frames that will be averaged
#define MAX_M				1				//number of variables to calibrate
#define MAX_N				1000			//maximum number of frequencies at which to calibrate
#define MAX_CAL_SETS		2				//maximum number of calibration data sets

#define MAX_STITCH			16				//maximum number of sub-spans to stitch together

#define INREAL				float			//type to be used for samples copied in
#define OUTREAL				float			//type to be used for pixel values copied out

#define MAX_DISPLAYS		16				//maximum number of displays = max instances

typedef struct _dp
{
	int max_size;										//maximum fft size to be used
	int max_num_fft;									//maximum number of LO positions per sub-span to be used
	int max_stitch;										//maximum number of sub-spans to be concatenated
														//NOTE:  max_size, max_num_fft, and max_stitch MUST BE <= THE
															//	CORRESPONDING VALUES IN <analyzer.h>!!
	int num_fft;										//current number of ffts in use						
	int size;											//current size of fft input sample vector
	int out_size;										//current size of fft output vector
	int window_type;									//type of the window function to be applied
	int overlap;										//number of samples re-used per fft, range 0 to size-1
	int flip[MAX_NUM_FFT];								//0 for low-side LO => do NOT flip; 1 for high-side LO => FLIP
	int clip;											//number of bins to clip off on EACH end of the sub-span fft
														//	ASSUMES size/2 IS AN EVEN NUMBER!!!
	int fsclipL;										//number of bins to clip off the lower end of the TOTAL SPAN
	int fsclipH;										//number of bins to clip off the upper end of the TOTAL SPAN
	int fscL;											//fsclipL modulo (out_size - 2 * clip)
	int fscH;											//fsclipH modulo (out_size - 2 * clip)
	int begin_ss;										//number of first sub-span that is NOT completely clipped off
	int end_ss;											//number of last sub-span that is NOT completely clipped off
	int ss_bins[MAX_STITCH];							//number of bins delivered by eliminate()/Celiminate in each sub-span
	volatile LONG input_busy[MAX_STITCH][MAX_NUM_FFT];

	int num_pixels;										//number of pixels requested
	int num_stitch;										//number of results to be stitched together to generate the pixel frame
	unsigned long long stitch_flag;
	int spec_flag[MAX_STITCH];							//flags showing if all ffts for a sub-span are done so elimination can proceed
	double pix_per_bin;									//number of pixels per fft bin, note that this is fractional, not integral
	double bin_per_pix;									//number of fft bins per pixel, this is fractional and != 1.0/pix_per_bin
	double scale;										//output amplitude scale factor
	double PiAlpha;										//parameter for Kaiser window function

	int cal_set;										//specifies which set of calibration data to use
	double f_min;										//frequency at first pixel (for calibration)
	double f_max;										//frequency at last pixel (for calibration)
	int cal_changed;									//flag to indicate that the calibration data has changed

	double *window;										//pointer to buffer to hold window coefficients	
	double *result[MAX_STITCH];							//pointers to buffer to hold elimination results for each sub-span
	OUTREAL *pixels[NUM_PIXEL_BUFFS];					//pointers pixel output buffers
	double *t_pixels;									//pointer to temporary pixel buffer
	int w_pix_buff;										//number of pixel buffer owned by writing process
	int r_pix_buff;										//number of pixel buffer owned by reading process
	int last_pix_buff;									//number of the last pixel buffer written
	volatile LONG pb_ready[NUM_PIXEL_BUFFS];			//if value is 0, this data has already been read; 1 = fresh data to read
	int num_average;									//number of spans to average to create the pixels
	int avail_frames;									//number of pixel frames currently available to average
	int av_in_idx;										//input index in averaging pixel buffer ring
	int av_out_idx;										//output index in averaging pixel buffer ring
	double *av_sum;										//pointer to sum buffer for averaging
	double *av_buff[MAX_AVERAGE];						//pointers to ring of buffers to hold pixel frames for averaging
	int av_mode;
	double av_backmult;									//back multiplier for weighted averaging
	double *cd;											//pointer to amplitude calibration buffer
	int n_freqs[MAX_CAL_SETS];							//number of frequencies in each calibration set
	double *freqs[MAX_CAL_SETS];						//pointers to vectors of calibration frequencies
	double (*ac3[MAX_CAL_SETS][MAX_M]);					//pointers to amplitude interpolant coefficients
	double (*ac2[MAX_CAL_SETS][MAX_M]);
	double (*ac1[MAX_CAL_SETS][MAX_M]);
	double (*ac0[MAX_CAL_SETS][MAX_M]);

	fftw_plan plan[MAX_STITCH][MAX_NUM_FFT];			//fftw plans
	fftw_plan Cplan[MAX_STITCH][MAX_NUM_FFT];
	double *fft_in[MAX_STITCH][MAX_NUM_FFT];			//pointers to fftw real input vectors
	fftw_complex *Cfft_in[MAX_STITCH][MAX_NUM_FFT];		//pointers to fftw complex input vectors
	fftw_complex *fft_out[MAX_STITCH][MAX_NUM_FFT];		//pointers to fftw complex output vectors
	volatile LONG *pnum_threads;						//pointer to current number of active worker threads
	int stop;											//when set, fft threads will be returned to the pool
	int end_dispatcher;									//set this flag to one to destroy the dispatcher thread
	int dispatcher;										//one if the dispatcher thread is alive & active
	int ss;												//sub-span being processed
	int LO;												//LO (within current sub-span) being processed 
	int flag;
	int have_samples[MAX_STITCH][MAX_NUM_FFT];			//number of unused samples remaining in a buffer
	int type;											//0 for REAL, 1 for COMPLEX
	int incr;											//size - overlap
	int buff_size;										//amount of data to be stored each time an input buffer is opened and closed = JanusAudio/BlockSize
	INREAL* I_samples[MAX_STITCH][MAX_NUM_FFT];			//pointers to current input position in I/Q buffers
	INREAL* Q_samples[MAX_STITCH][MAX_NUM_FFT];
	int bsize;											//size of I_samples[][] and Q_samples[][] (number of samples they hold)
	int IQout_index[MAX_STITCH][MAX_NUM_FFT];			//current output index for I_samples[ss][LO] and Q_samples[ss][LO]
	int IQO_idx[MAX_STITCH][MAX_NUM_FFT];
	int IQin_index[MAX_STITCH][MAX_NUM_FFT];			//current input index for I_samples[ss][LO] and Q_samples[ss][LO]
	volatile LONG buff_ready[MAX_STITCH][MAX_NUM_FFT];	//1 if buffer ready to read; 0 if needs to be filled
	int max_writeahead;									//max allowed input samples ahead of where reading output samples

	volatile LONG snap[MAX_STITCH][MAX_NUM_FFT];		//set to 1 to allow a snap of raw spectrum data
	HANDLE hSnapEvent[MAX_STITCH][MAX_NUM_FFT];			//mutex handles; mutexes will be used to signal a snap is complete
	double *snap_buff[MAX_STITCH][MAX_NUM_FFT];			//pointers to buffers for the snap

	CRITICAL_SECTION PB_ControlsSection;
	CRITICAL_SECTION SetAnalyzerSection;
	CRITICAL_SECTION BufferControlSection[MAX_STITCH][MAX_NUM_FFT];
	CRITICAL_SECTION StitchSection;
	CRITICAL_SECTION EliminateSection[MAX_STITCH];
	CRITICAL_SECTION ResampleSection;
	CRITICAL_SECTION AverageSection;
}  dp, *DP;

DP pdisp[MAX_DISPLAYS];									//array of pointers to instance data

#if defined specHPSDR_EXPORT
#define specHPSDR_PORT		__declspec( dllexport )
#else
#define specHPSDR_PORT		__declspec( dllimport )
#endif

extern specHPSDR_PORT    
void SetAnalyzer (	int disp,
					int n_fft,			//number of LO frequencies = number of ffts used in elimination
					int typ,			//0 for real input data (I only); 1 for complex input data (I & Q)
					int *flp,			//vector with one elt for each LO frequency, 1 if high-side LO, 0 otherwise 
					int sz,				//size of the fft, i.e., number of input samples
					int bf_sz,			//number of samples transferred for each OpenBuffer()/CloseBuffer()
					int win_type,		//integer specifying which window function to use
					double pi,			//PiAlpha parameter for Kaiser window
					int ovrlp,			//number of samples each fft (other than the first) is to re-use from the previous 
					int clp,			//number of fft output bins to be clipped from EACH side of each sub-span
					int fscLin,			//number of bins to clip from low end of entire span
					int fscHin,			//number of bins to clip from high end of entire span
					int n_pix,			//number of pixel values to return.  may be either <= or > number of bins 
					int n_stch,			//number of sub-spans to concatenate to form a complete span 
					int av_m,			//averaging mode
					int n_av,			//number of spans to (moving) average for pixel result 
					double av_b,		//back multiplier for weighted averaging
					int calset,			//identifier of which set of calibration data to use 
					double fmin,		//frequency at first pixel value 
					double fmax,		//frequency at last pixel value
					int max_w			//max samples to hold in input ring buffers
				 );

extern specHPSDR_PORT
void CreateAnalyzer (	int disp,
						int *success,
						char *app_data_path);

extern specHPSDR_PORT
void XCreateAnalyzer (	int disp,
						int *success,	//writes '0' to success if all went well, <0 if mem alloc failed
						int m_size,		//maximum fft size to be used
						int m_LO,		//maximum number of LO positions per subspan
						int m_stitch,	//maximum number of subspans to be concatenated
						char *app_data_path
					 );

extern specHPSDR_PORT   
void DestroyAnalyzer(int disp);

extern specHPSDR_PORT   
void GetPixels	(	int disp,
					OUTREAL *pix,		//if new pixel values avail, copies to pix and sets flag = 1
					int *flag			//else, returns 0 (try again later)
				);

extern specHPSDR_PORT   
void SetCalibration (	int disp,
						int set_num,				//identifier for this calibration data set
						int n_points,				//number of calibration points in the set
						double (*cal)[MAX_M+1]		//pointer to the calibration table, first
					);

extern specHPSDR_PORT   
void OpenBuffer(int disp, int ss, int LO, void **Ipointer, void **Qpointer);

extern specHPSDR_PORT   
void CloseBuffer(int disp, int ss, int LO);

extern specHPSDR_PORT
void Spectrum(int disp, int ss, int LO, INREAL* pI, INREAL* pQ);

extern specHPSDR_PORT
void Spectrum2(int disp, int ss, int LO, INREAL* pbuff);

extern specHPSDR_PORT
void SnapSpectrum(	int disp,
					int ss,
					int LO,
					double *snap_buff);