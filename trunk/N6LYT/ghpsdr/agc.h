// agc.h

#define agcLONG 0
#define agcSLOW 1
#define agcMEDIUM 2
#define agcFAST 3

int agc;

void setAgc(int agc);
void agcSaveState();
void agcRestoreState();
GtkWidget* buildAgcUI();
