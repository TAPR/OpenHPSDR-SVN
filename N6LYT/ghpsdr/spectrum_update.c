/** 
* @file spectrum_update.c
* @brief Spectrum update functions
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-12
*/

/* Copyright (C) 
* 2009 - John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
* 
*/


#include <gtk/gtk.h>

#include "main.h"
#include "wdsp.h"
#include "channel.h"
#include "spectrum.h"
#include "spectrum_update.h"
#include "transmit.h"
#include "ozy.h"

int updatingSpectrum;
int spectrumUpdatesPerSecond;

gint spectrumUpdate(gpointer data);
void updateSamples();

float spectrumBuffer[spectrumWIDTH];

gint spectrumTimerId;

/* --------------------------------------------------------------------------*/
/** 
* @brief New spectrum update
*/
void newSpectrumUpdate() {
    spectrumTimerId=gtk_timeout_add(1000/spectrumUpdatesPerSecond,spectrumUpdate,NULL);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Stop spectrum update
*/
void stopSpectrumUpdate() {
    gtk_timeout_remove(spectrumTimerId);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief set spectrum update rate
*/
void setSpectrumUpdateRate(int rate) {
    stopSpectrumUpdate();
    spectrumUpdatesPerSecond=rate;
    newSpectrumUpdate();
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Spectrum update 
* 
* @param data
* 
* @return 
*/
gint spectrumUpdate(gpointer data) {
    switch(spectrumMode) {
        case spectrumNONE:
            updatingSpectrum=FALSE;
            break;
        default:
            updateSamples();
            break;
    }
    return TRUE;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Update samples
*/
void updateSamples() {
    int thread;
    thread=0;
/*
    if(xmit && !fullDuplex) thread=1;
*/

    int result;
    switch(spectrumMode) {
        case spectrumSPECTRUM:
            if(xmit | tuning) {
                GetPixels(CHANNEL_TX, spectrumBuffer, &result);
            } else {
                GetPixels(CHANNEL_RX0, spectrumBuffer, &result);
            }
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumPANADAPTER:
            if(xmit | tuning) {
                GetPixels(CHANNEL_TX, spectrumBuffer, &result);
            } else {
                GetPixels(CHANNEL_RX0, spectrumBuffer, &result);
            }
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumSCOPE:
            if(xmit | tuning) {
                TXAGetaSipF(CHANNEL_TX, spectrumBuffer, spectrumWIDTH);
            } else {
                RXAGetaSipF(CHANNEL_RX0, spectrumBuffer, spectrumWIDTH);
            }
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumPHASE:
            if(xmit | tuning) {
                TXAGetaSipF1(CHANNEL_TX, spectrumBuffer, spectrumWIDTH);
            } else {
                RXAGetaSipF(CHANNEL_RX0, spectrumBuffer, spectrumWIDTH);
            }
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumPHASE2:
            if(xmit | tuning) {
                TXAGetaSipF1(CHANNEL_TX, spectrumBuffer, spectrumWIDTH);
            } else {
                RXAGetaSipF(CHANNEL_RX0, spectrumBuffer, spectrumWIDTH);
            }
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumPANWATER:
            if(xmit | tuning) {
                GetPixels(CHANNEL_TX, spectrumBuffer, &result);
            } else {
                GetPixels(CHANNEL_RX0, spectrumBuffer, &result);
            }
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumHISTOGRAM:
            updateSpectrum(spectrumBuffer);
            break;
        case spectrumNONE:
            break;
    }

}
