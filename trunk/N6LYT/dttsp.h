// dttsp.h
//
// what we know about DttSP
//

extern void dttsp_main(int argc,char* argv[]);

extern void audio_callback (float *input_l, float *input_r, float *output_l,
                            float *output_r, unsigned int nframes);

extern void Process_Spectrum (float *results);
extern void Process_Panadapter (float *results);
extern void Process_Phase (float *results, int numpoints);
extern void Process_Scope (float *results, int numpoints);
extern float CalculateRXMeter(unsigned int subrx, int mt);

extern int SetSampleRate(double sampleRate);
