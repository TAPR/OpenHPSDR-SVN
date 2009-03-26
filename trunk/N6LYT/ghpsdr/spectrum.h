// spectrum.h


#define spectrumSPECTRUM 0
#define spectrumPANADAPTER 1
#define spectrumSCOPE 2
#define spectrumPHASE 3
#define spectrumPHASE2 4
#define spectrumPANWATER 5
#define spectrumHISTOGRAM 6
#define spectrumOFF 7

#define spectrumWIDTH 960
#define spectrumHEIGHT 400

int spectrumMAX;
int spectrumMIN;
int spectrumSTEP;

float waterfallHighThreshold;
float waterfallLowThreshold;

int spectrumMode;

GtkWidget* newSpectrumDisplay();
void setSpectrumMode(int mode);
void updateSpectrum(float* samples);

