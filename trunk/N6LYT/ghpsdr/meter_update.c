#include <gtk/gtk.h>

#include "main.h"
#include "dttsp.h"
#include "meter.h"
#include "meter_update.h"

int updatingMeter;
int meterUpdatesPerSecond;

gint meterUpdate(gpointer data);
void meterUpdateSamples(char* command);

float meterBuffer[METER_BUFFER_SIZE];

gint timerId;

void newMeterUpdate() {
    timerId=gtk_timeout_add(1000/meterUpdatesPerSecond,meterUpdate,NULL);
}

gint meterUpdate(gpointer data) {
    switch(meterMode) {
        case meterSIGNAL:
            meterUpdateSamples("reqMeter 1");
            break;
        case meterOFF:
            updatingMeter=FALSE;
            break;
    }
    return TRUE;
}

void meterUpdateSamples(char* command) {
    int n;

    float m=CalculateRXMeter(0,0);
    updateMeter(m);
}
