// meter.h

#define meterSIGNAL 0
#define meterOFF 1

#define MAX_RX 4
#define RX_METER_POINTS 5
#define METER_BUFFER_SIZE MAX_RX*RX_METER_POINTS

int meterMode;

void setMeterMode(int mode);
void updateMeter(float sample);

GtkWidget* buildMeterUI();
void updateMeter(float sample);
void meterSaveState();
void meterRestoreState();
