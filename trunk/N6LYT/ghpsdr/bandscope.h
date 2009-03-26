// spectrum.h

#define bandscopeWIDTH 960
#define bandscopeHEIGHT 100

#define BANDSCOPE_BUFFER_SIZE 4096

GtkWidget* buildBandscopeUI();
void updateBandscope(float* samples);
void bandscopeSaveState();
void bandscopeRestoreState();
