// band.h

#define band160 0
#define band80 1
#define band60 2
#define band40 3
#define band30 4
#define band20 5
#define band17 6
#define band15 7
#define band12 8
#define band10 9
#define band6 10
#define bandGen 11
#define bandWWV 12

#define BANDS 13

int band;

void bandSaveState();
void bandRestoreState();

GtkWidget* buildBandUI();
