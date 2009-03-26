// vfo.h

long long frequencyA;
long long frequencyB;
long long frequencyMin;
long long frequencyMax;

long frequencyIncrement;

void vfoSaveState();
void vfoRestoreState();

GtkWidget* buildVfoUI();
void vfoIncrementFrequency(long increment);
void vfoSetRxFrequency();
