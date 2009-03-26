#include <gtk/gtk.h>

#include "main.h"
#include "dttsp.h"
#include "bandscope.h"
#include "bandscope_update.h"

int updatingBandscope;
int bandscopeUpdatesPerSecond=BANDSCOPE_UPDATES_PER_SECOND;

gint bandscopeUpdate(gpointer data);
void bandscopeUpdateSamples();

unsigned char bandscopeBuffer[BANDSCOPE_BUFFER_SIZE*2];
float bandscopeSamples[BANDSCOPE_BUFFER_SIZE];

gint bandscopeTimerId;

void newBandscopeUpdate() {
    bandscopeTimerId=gtk_timeout_add(1000/bandscopeUpdatesPerSecond,bandscopeUpdate,NULL);
}

void stopBandscopeUpdate() {
    gtk_timeout_remove(bandscopeTimerId);
}

gint bandscopeUpdate(gpointer data) {
    getSpectrumSamples((char*)bandscopeBuffer);
    bandscopeUpdateSamples();
    return TRUE;
}

void bandscopeUpdateSamples() {
    int i;
    short s;
    for(i=0;i<BANDSCOPE_BUFFER_SIZE;i++) {
        s=(bandscopeBuffer[i*2]<<8)+bandscopeBuffer[(i*2)+1];
        bandscopeSamples[i]=(float)s/32767.0f; // get into range -1..+1
    }
    updateBandscope(bandscopeSamples);
}
