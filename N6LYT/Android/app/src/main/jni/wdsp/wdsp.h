#ifndef export
#define export

extern "C" {
//Channel

void OpenChannel (int channel, int in_size, int dsp_size, int input_samplerate, int dsp_rate, int output_samplerate, int type, int state, double tdelayup, double tslewup, double tdelaydown, double tslewdown);

void CloseChannel (int channel);

void SetType (int channel, int type);

void SetInputBuffsize (int channel, int in_size);

void SetDSPBuffsize (int channel, int dsp_size);

void SetInputSamplerate  (int channel, int samplerate);

void SetDSPSamplerate (int channel, int samplerate);

void SetOutputSamplerate (int channel, int samplerate);

void SetAllRates (int channel, int in_rate, int dsp_rate, int out_rate);

void SetChannelState (int channel, int state, int dmode);

// RXA
enum rxaMode
{
	RXA_LSB,
	RXA_USB,
	RXA_DSB,
	RXA_CWL,
	RXA_CWU,
	RXA_FM,
	RXA_AM,
	RXA_DIGU,
	RXA_SPEC,
	RXA_DIGL,
	RXA_SAM,
	RXA_DRM
};

enum rxaMeterType
{
	RXA_S_PK,
	RXA_S_AV,
	RXA_ADC_PK,
	RXA_ADC_AV,
	RXA_AGC_GAIN,
	RXA_AGC_PK,
	RXA_AGC_AV,
	RXA_METERTYPE_LAST
};

void SetRXAMode (int channel, int mode);

double GetRXAMeter (int channel, int mt);


// TXA
enum txaMode
{
	TXA_LSB,
	TXA_USB,
	TXA_DSB,
	TXA_CWL,
	TXA_CWU,
	TXA_FM,
	TXA_AM,
	TXA_DIGU,
	TXA_SPEC,
	TXA_DIGL,
	TXA_SAM,
	TXA_DRM
};

enum txaMeterType
{
	TXA_MIC_PK,
	TXA_MIC_AV,
	TXA_EQ_PK,
	TXA_EQ_AV,
	TXA_LVLR_PK,
	TXA_LVLR_AV,
	TXA_LVLR_GAIN,
	TXA_COMP_PK,
	TXA_COMP_AV,
	TXA_ALC_PK,
	TXA_ALC_AV,
	TXA_ALC_GAIN,
	TXA_OUT_PK,
	TXA_OUT_AV,
	TXA_METERTYPE_LAST
};

void SetTXAMode (int channel, int mode);

double GetTXAMeter (int channel, int mt);

// bandpass
void SetRXABandpassRun (int channel, int run);

void SetRXABandpassFreqs (int channel, double low, double high);

// TXA Prototypes

void SetTXABandpassRun (int channel, int run);

void SetTXABandpassFreqs (int channel, double low, double high);



void fexchange0 (int channel, double* in, double* out, int* error);

void fexchange2 (int channel, float *Iin, float *Qin, float *Iout, float *Qout, int* error);



// spectrum

void XCreateAnalyzer (int disp,
	int *success,   //writes '0' to success if all went well, <0 if mem alloc failed
	int m_size,             //maximum fft size to be used
	int m_LO,               //maximum number of LO positions per subspan
	int m_stitch,   //maximum number of subspans to be concatenated
	char *app_data_path
	);

void DestroyAnalyzer(int disp);


void SetAnalyzer(int disp,
	int n_fft,                      //number of LO frequencies = number of ffts used in elimination
	int typ,                        //0 for real input data (I only); 1 for complex input data (I & Q)
	int *flp,                       //vector with one elt for each LO frequency, 1 if high-side LO, 0 otherwise 
	int sz,                         //size of the fft, i.e., number of input samples
	int bf_sz,                      //number of samples transferred for each OpenBuffer()/CloseBuffer()
	int win_type,           //integer specifying which window function to use
	double pi,                      //PiAlpha parameter for Kaiser window
	int ovrlp,                      //number of samples each fft (other than the first) is to re-use from the previous 
	int clp,                        //number of fft output bins to be clipped from EACH side of each sub-span
	int fscLin,                     //number of bins to clip from low end of entire span
	int fscHin,                     //number of bins to clip from high end of entire span
	int n_pix,                      //number of pixel values to return.  may be either <= or > number of bins 
	int n_stch,                     //number of sub-spans to concatenate to form a complete span 
	int av_m,                       //averaging mode
	int n_av,                       //number of spans to (moving) average for pixel result 
	double av_b,            //back multiplier for weighted averaging
	int calset,                     //identifier of which set of calibration data to use 
	double fmin,            //frequency at first pixel value 
	double fmax,            //frequency at last pixel value
	int max_w                       //max samples to hold in input ring buffers
	);


void Spectrum(int disp, int ss, int LO, float* i,float* q);

void Spectrum2(int disp, int ss, int LO, double* pbuff);

void GetPixels  (int disp,
	float *pix,          //if new pixel values avail, copies to pix and sets flag = 1
	int *flag                       //else, returns 0 (try again later)
	);

void GetNAPixels  (int disp,
	float *pix,          //if new pixel values avail, copies to pix and sets flag = 1
	int *flag                       //else, returns 0 (try again later)
	);

void WDSPwisdom(char *directory);

}

#endif
