// spectrum.h

#define bandscopeWIDTH 1024
#define bandscopeHEIGHT 100

#define BANDSCOPE_BUFFER_SIZE 4096

int bandscopeMAX;
int bandscopeMIN;
int bandscopeZoom;

GtkWidget* buildBandscopeUI();
void updateBandscope(float* samples);
void bandscopeSaveState();
void bandscopeRestoreState();
void bandscopeSetZoom(int zoom);
