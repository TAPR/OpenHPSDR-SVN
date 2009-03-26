#include <gtk/gtk.h>

#include "main.h"
#include "dttsp.h"
#include "spectrum.h"
#include "spectrum_update.h"

int updatingSpectrum;
int spectrumUpdatesPerSecond;

gint spectrumUpdate(gpointer data);
void updateSamples();

float spectrumBuffer[SPECTRUM_BUFFER_SIZE];

gint spectrumTimerId;

void newSpectrumUpdate() {
    spectrumTimerId=gtk_timeout_add(1000/spectrumUpdatesPerSecond,spectrumUpdate,NULL);
}

void stopSpectrumUpdate() {
    gtk_timeout_remove(spectrumTimerId);
}

gint spectrumUpdate(gpointer data) {
    switch(spectrumMode) {
        case spectrumOFF:
            updatingSpectrum=FALSE;
            break;
        default:
            updateSamples();
            break;
    }
    return TRUE;
}

void updateSamples() {
    int n;

    switch(spectrumMode) {
        case spectrumSPECTRUM:
            Process_Spectrum(spectrumBuffer);
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumPANADAPTER:
            Process_Panadapter(spectrumBuffer);
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumSCOPE:
            Process_Scope(spectrumBuffer,4096);
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumPHASE:
            Process_Scope(spectrumBuffer,100);
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumPHASE2:
            Process_Scope(spectrumBuffer,100);
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumPANWATER:
            Process_Panadapter(spectrumBuffer);
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumHISTOGRAM:
            Process_Panadapter(spectrumBuffer);
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumOFF:
            break;
    }

}
