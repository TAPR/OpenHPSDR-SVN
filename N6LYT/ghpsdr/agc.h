// agc.h

#define agcOFF 0
#define agcLONG 1
#define agcSLOW 2
#define agcMEDIUM 3
#define agcFAST 4

int agc;

void setAgc(int agc);
void agcSaveState();
void agcRestoreState();
GtkWidget* buildAgcUI();
