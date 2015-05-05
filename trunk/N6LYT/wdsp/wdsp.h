#define PORT extern
#define INREAL float
#define OUTREAL float
#define dINREAL float

// channel
PORT void OpenChannel (int channel, int in_size, int dsp_size, int input_samplerate, int dsp_rate, int output_samplerate, int type, int state, double tdelayup, double tslewup, double tdelaydown, double tslewdown, int bfo);
PORT void CloseChannel (int channel);
PORT void SetType (int channel, int type);
PORT void SetInputBuffsize (int channel, int in_size);
PORT void SetDSPBuffsize (int channel, int dsp_size);
PORT void SetInputSamplerate  (int channel, int samplerate);
PORT void SetDSPSamplerate (int channel, int samplerate);
PORT void SetOutputSamplerate (int channel, int samplerate);
PORT void SetAllRates (int channel, int in_rate, int dsp_rate, int out_rate);
PORT void SetChannelState (int channel, int state, int dmode);
PORT void SetChannelTDelayUp (int channel, double time);
PORT void SetChannelTSlewUp (int channel, double time);
PORT void SetChannelTDelayDown (int channel, double time);
PORT void SetChannelTSlewDown (int channel, double time);

// iobuffs
PORT void fexchange0 (int channel, double* in, double* out, int* error);
PORT void fexchange2 (int channel, INREAL *Iin, INREAL *Qin, OUTREAL *Iout, OUTREAL *Qout, int* error);

PORT void Spectrum(int disp, int ss, int LO, dINREAL* pI, dINREAL* pQ);

// RXA
PORT void SetRXAMode (int channel, int mode);
PORT void SetRXABandpassRun (int channel, int run);
PORT void SetRXABandpassFreqs (int channel, double low, double high);
PORT void SetRXAFMSQRun (int channel, int run);
PORT void SetRXAEMNRRun (int channel, int run);
PORT void SetRXAEMNRgainMethod (int channel, int method);
PORT void SetRXAEMNRnpeMethod (int channel, int method);
PORT void SetRXAEMNRaeRun (int channel, int run);
PORT void SetRXAEMNRPosition (int channel, int position);
PORT void SetRXAANRRun(int channel, int run);
PORT void SetRXAANFRun(int channel, int run);
PORT double GetRXAMeter (int channel, int mt);
PORT void SetRXAPanelBinaural(int channel, int bin);
PORT void SetRXAPanelPan (int channel, double pan);




// TXA Prototypes^M
PORT void SetTXABandpassRun (int channel, int run);
PORT void SetTXABandpassFreqs (int channel, double low, double high);

// wisdom
PORT void WDSPwisdom (char* directory);

