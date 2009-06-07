/** 
* @file band.c
* @brief Files to define the Amateur Radio Bands.
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-11
*/
// band.c

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
#include <gdk/gdkkeysyms.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <getopt.h>

#include "bandstack.h"
#include "command.h"
#include "cw.h"
#include "filter.h"
#include "main.h"
#include "property.h"
#include "soundcard.h"
#include "mode.h"
#include "band.h"
#include "vfo.h"
#include "hardware.h"
#include "spectrum.h"

GtkWidget* bandFrame;
GtkWidget* bandTable;

int band=band40;

GtkWidget* button160;
GtkWidget* button80;
GtkWidget* button60;
GtkWidget* button40;
GtkWidget* button30;
GtkWidget* button20;
GtkWidget* button17;
GtkWidget* button15;
GtkWidget* button12;
GtkWidget* button10;
GtkWidget* button6;
GtkWidget* buttonGen;
GtkWidget* buttonWWV;
GtkWidget* buttonXVTR;

GtkWidget* currentBandButton;

/* --------------------------------------------------------------------------*/
/** 
* @brief bandstack
*/
/* ----------------------------------------------------------------------------*/
BANDSTACK_ENTRY bandstack160[] =
    {{1810000LL,modeCWL,filterF4,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140},
     {1835000LL,modeCWU,filterF0,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140},
     {1845000LL,modeUSB,filterF5,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140}};

BANDSTACK_ENTRY bandstack80[] =
    {{3501000LL,modeCWL,filterF0,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140},
     {3751000LL,modeLSB,filterF5,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140},
     {3850000LL,modeLSB,filterF5,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140}};

BANDSTACK_ENTRY bandstack60[] =
    {{5330500LL,modeUSB,filterF5,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140},
     {5346500LL,modeUSB,filterF5,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140},
     {5366500LL,modeUSB,filterF5,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140},
     {5371500LL,modeUSB,filterF5,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140},
     {5403500LL,modeUSB,filterF5,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140}};

BANDSTACK_ENTRY bandstack40[] =
    {{7001000LL,modeCWL,filterF0,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140},
     {7152000LL,modeLSB,filterF5,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140},
     {7255000LL,modeLSB,filterF5,-2800,-200,-2800,-200,100,0,-100,-180,10,-110,-140}};

BANDSTACK_ENTRY bandstack30[] =
    {{10120000LL,modeCWU,filterF0,200,2800,200,2800,100,0,-100,-180,10,-110,-140},
     {10130000LL,modeCWU,filterF0,200,2800,200,2800,100,0,-100,-180,10,-110,-140},
     {10140000LL,modeCWU,filterF4,200,2800,200,2800,100,0,-100,-180,10,-110,-140}};

BANDSTACK_ENTRY bandstack20[] =
    {{14010000LL,modeCWU,filterF0,200,2800,200,2800,100,1,-100,-180,10,-110,-140},
     {14230000LL,modeUSB,filterF5,200,2800,200,2800,100,1,-100,-180,10,-100,-140},
     {14336000LL,modeUSB,filterF5,200,2800,200,2800,100,1,-100,-180,10,-110,-140}};

BANDSTACK_ENTRY bandstack18[] =
    {{18068600LL,modeCWU,filterF0,200,2800,200,2800,100,1,-100,-180,10,-110,-140},
     {18125000LL,modeUSB,filterF5,200,2800,200,2800,100,1,-100,-180,10,-110,-140},
     {18140000LL,modeUSB,filterF5,200,2800,200,2800,100,1,-100,-180,10,-110,-140}};

BANDSTACK_ENTRY bandstack15[] =
    {{21001000LL,modeCWU,filterF0,200,2800,200,2800,100,1,-100,-180,10,-110,-140},
     {21255000LL,modeUSB,filterF5,200,2800,200,2800,100,1,-100,-180,10,-110,-140},
     {21300000LL,modeUSB,filterF5,200,2800,200,2800,100,1,-100,-180,10,-110,-140}};

BANDSTACK_ENTRY bandstack12[] =
    {{24895000LL,modeCWU,filterF0,200,2800,200,2800,100,1,-100,-180,10,-110,-140},
     {24900000LL,modeUSB,filterF5,200,2800,200,2800,100,1,-100,-180,10,-110,-140},
     {24910000LL,modeUSB,filterF5,200,2800,200,2800,100,1,-100,-180,10,-110,-140}};

BANDSTACK_ENTRY bandstack10[] =
    {{28010000LL,modeCWU,filterF0,200,2800,200,2800,100,1,-100,-180,10,-110,-140},
     {28300000LL,modeUSB,filterF5,200,2800,200,2800,100,1,-100,-180,10,-110,-140},
     {28400000LL,modeUSB,filterF5,200,2800,200,2800,100,1,-100,-180,10,-110,-140}};

BANDSTACK_ENTRY bandstack50[] =
    {{50010000LL,modeCWU,filterF0,200,2800,200,2800,100,1,-100,-180,10,-110,-140},
     {50125000LL,modeUSB,filterF5,200,2800,200,2800,100,1,-100,-180,10,-110,-150},
     {50200000LL,modeUSB,filterF5,200,2800,200,2800,100,1,-100,-180,10,-110,-140}};

BANDSTACK_ENTRY bandstackGEN[] =
    {{909000LL,modeAM,filterF6,-6000,6000,-6000,6000,1000,0,-100,-180,10,-110,-140},
     {5975000LL,modeAM,filterF6,-6000,6000,-6000,6000,1000,0,-100,-180,10,-110,-140},
     {13845000LL,modeAM,filterF6,-6000,6000,-6000,6000,1000,0,-100,-180,10,-110,-140}};

BANDSTACK_ENTRY bandstackWWV[] =
    {{2500000LL,modeSAM,filterF6,200,2800,200,2800,1000,0,-100,-180,10,-110,-140},
     {5000000LL,modeSAM,filterF6,200,2800,200,2800,1000,0,-100,-180,10,-110,-140},
     {10000000LL,modeSAM,filterF6,200,2800,200,2800,1000,0,-100,-180,10,-110,-140},
     {15000000LL,modeSAM,filterF6,200,2800,200,2800,1000,0,-100,-180,10,-110,-140},
     {20000000LL,modeSAM,filterF6,200,2800,200,2800,1000,0,-100,-180,10,-110,-140}};

BANDSTACK bandstack[13];

void setTuningMode(int mode);
void setBand(int band);
void setIncrement(int increment);

/* --------------------------------------------------------------------------*/
/** 
* @brief Select the band.
* 
* @param widget
*/
/* ----------------------------------------------------------------------------*/
void selectBand(GtkWidget* widget) {
    GtkWidget* label;
    BANDSTACK_ENTRY* entry;
    int current;

    if(currentBandButton) {
        label=gtk_bin_get_child((GtkBin*)currentBandButton);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &white);
    }
    label=gtk_bin_get_child((GtkBin*)widget);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
    gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);

    //save current
    if(currentBandButton) {
        current=bandstack[band].current_entry;
        entry=&bandstack[band].entry[current];
        entry->frequencyA=frequencyA;
        entry->mode=mode;
        entry->filter=filter;
        entry->var1Low=filterVar1Low;
        entry->var1High=filterVar1High;
        entry->var2Low=filterVar2Low;
        entry->var2High=filterVar2High;
        entry->step=frequencyIncrement;
        entry->preamp=Preamp;
        entry->spectrumHigh=spectrumMAX;
        entry->spectrumLow=spectrumMIN;
        entry->spectrumStep=spectrumSTEP;
        entry->waterfallHigh=waterfallHighThreshold;
        entry->waterfallLow=waterfallLowThreshold;
    }

    if(currentBandButton==widget) {
fprintf(stderr,"currentBandButton==widget\n");
        bandstack[band].current_entry++;
        if(bandstack[band].current_entry>=bandstack[band].entries) {
            bandstack[band].current_entry=0;
        }
    } else {
        currentBandButton=widget;
        if(widget==button160) {
            band=band160;
        } else if(widget==button80) {
            band=band80;
        } else if(widget==button60) {
            band=band60;
        } else if(widget==button40) {
            band=band40;
        } else if(widget==button30) {
            band=band30;
        } else if(widget==button20) {
            band=band20;
        } else if(widget==button17) {
            band=band17;
        } else if(widget==button15) {
            band=band15;
        } else if(widget==button12) {
            band=band12;
        } else if(widget==button10) {
            band=band10;
        } else if(widget==button6) {
            band=band6;
        } else if(widget==buttonGen) {
            band=bandGen;
        } else if(widget==buttonWWV) {
            band=bandWWV;
        } else if(widget==buttonXVTR) {
            band=bandXVTR;
        }
    }

    current=bandstack[band].current_entry;
    entry=&bandstack[band].entry[current];

    setModeMode(entry->mode);
    filterVar1Low=entry->var1Low;
    filterVar1High=entry->var1High;
    filterVar2Low=entry->var2Low;
    filterVar2High=entry->var2High;
    setFilter(entry->filter);
    setAFrequency(entry->frequencyA);
    setIncrement(entry->step);

    setPreamp(entry->preamp);
    spectrumMAX=entry->spectrumHigh;
    spectrumMIN=entry->spectrumLow;
    spectrumSTEP=entry->spectrumStep;
    waterfallHighThreshold=entry->waterfallHigh;
    waterfallLowThreshold=entry->waterfallLow;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the band.
* 
* @param band -- band value 
*/
/* ----------------------------------------------------------------------------*/
void setBand(int band) {
    GtkWidget* widget;
    switch(band) {
        case band160:
            widget=button160;
            break;
        case band80:
            widget=button80;
            break;
        case band60:
            widget=button60;
            break;
        case band40:
            widget=button40;
            break;
        case band30:
            widget=button30;
            break;
        case band20:
            widget=button20;
            break;
        case band17:
            widget=button17;
            break;
        case band15:
            widget=button15;
            break;
        case band12:
            widget=button12;
            break;
        case band10:
            widget=button10;
            break;
        case band6:
            widget=button6;
            break;
        case bandGen:
            widget=buttonGen;
            break;
        case bandWWV:
            widget=buttonWWV;
            break;
        case bandXVTR:
            widget=buttonXVTR;
            break;
    }
    selectBand(widget);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Force the band.
* 
* @param b -- band value.
*/
/* ----------------------------------------------------------------------------*/
void forceBand(int b) {
    GtkWidget* label;
    GtkWidget* widget;
    switch(b) {
        case band160:
            widget=button160;
            break;
        case band80:
            widget=button80;
            break;
        case band60:
            widget=button60;
            break;
        case band40:
            widget=button40;
            break;
        case band30:
            widget=button30;
            break;
        case band20:
            widget=button20;
            break;
        case band17:
            widget=button17;
            break;
        case band15:
            widget=button15;
            break;
        case band12:
            widget=button12;
            break;
        case band10:
            widget=button10;
            break;
        case band6:
            widget=button6;
            break;
        case bandGen:
            widget=buttonGen;
            break;
        case bandWWV:
            widget=buttonWWV;
            break;
        case bandXVTR:
            widget=buttonXVTR;
            break;
    }
    if(currentBandButton) {
        label=gtk_bin_get_child((GtkBin*)currentBandButton);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    }
    label=gtk_bin_get_child((GtkBin*)widget);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);

    currentBandButton=widget;
    band=b;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Callback when a band button is pressed.
* 
* @param widget
* @param data
*/
/* ----------------------------------------------------------------------------*/
void bandCallback(GtkWidget* widget,gpointer data) {
    selectBand(widget);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Build the GUI
* 
* @return 
*/
/* ----------------------------------------------------------------------------*/
GtkWidget* buildBandUI() {
    GtkWidget* label;

    bandFrame=gtk_frame_new("Band");
    gtk_widget_modify_bg(bandFrame,GTK_STATE_NORMAL,&background);
    gtk_widget_modify_fg(gtk_frame_get_label_widget(bandFrame),GTK_STATE_NORMAL,&white);

    bandTable=gtk_table_new(4,4,TRUE);

    // band selection
    button160 = gtk_button_new_with_label ("160");
    gtk_widget_modify_bg(button160, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button160);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button160),50,25);
    g_signal_connect(G_OBJECT(button160),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button160);
    gtk_table_attach_defaults(bandTable,button160,0,1,0,1);

    button80 = gtk_button_new_with_label ("80");
    gtk_widget_modify_bg(button80, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button80);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button80),50,25);
    g_signal_connect(G_OBJECT(button80),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button80);
    gtk_table_attach_defaults(bandTable,button80,1,2,0,1);

    button60 = gtk_button_new_with_label ("60");
    gtk_widget_modify_bg(button60, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button60);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button60),50,25);
    g_signal_connect(G_OBJECT(button60),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button60);
    gtk_table_attach_defaults(bandTable,button60,2,3,0,1);

    button40 = gtk_button_new_with_label ("40");
    gtk_widget_modify_bg(button40, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button40);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button40),50,25);
    g_signal_connect(G_OBJECT(button40),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button40);
    gtk_table_attach_defaults(bandTable,button40,3,4,0,1);

    button30 = gtk_button_new_with_label ("30");
    gtk_widget_modify_bg(button30, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button30);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button30),50,25);
    g_signal_connect(G_OBJECT(button30),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button30);
    gtk_table_attach_defaults(bandTable,button30,0,1,1,2);

    button20 = gtk_button_new_with_label ("20");
    gtk_widget_modify_bg(button20, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button20);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button20),50,25);
    g_signal_connect(G_OBJECT(button20),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button20);
    gtk_table_attach_defaults(bandTable,button20,1,2,1,2);

    button17 = gtk_button_new_with_label ("17");
    gtk_widget_modify_bg(button17, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button17);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button17),50,25);
    g_signal_connect(G_OBJECT(button17),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button17);
    gtk_table_attach_defaults(bandTable,button17,2,3,1,2);

    button15 = gtk_button_new_with_label ("15");
    gtk_widget_modify_bg(button15, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button15);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button15),50,25);
    g_signal_connect(G_OBJECT(button15),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button15);
    gtk_table_attach_defaults(bandTable,button15,3,4,1,2);

    button12 = gtk_button_new_with_label ("12");
    gtk_widget_modify_bg(button12, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button12);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button12),50,25);
    g_signal_connect(G_OBJECT(button12),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button12);
    gtk_table_attach_defaults(bandTable,button12,0,1,2,3);

    button10 = gtk_button_new_with_label ("10");
    gtk_widget_modify_bg(button10, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button10);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button10),50,25);
    g_signal_connect(G_OBJECT(button10),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button10);
    gtk_table_attach_defaults(bandTable,button10,1,2,2,3);

    button6 = gtk_button_new_with_label ("6");
    gtk_widget_modify_bg(button6, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button6);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button6),50,25);
    g_signal_connect(G_OBJECT(button6),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button6);
    gtk_table_attach_defaults(bandTable,button6,2,3,2,3);

    buttonGen = gtk_button_new_with_label ("GEN");
    gtk_widget_modify_bg(buttonGen, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonGen);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(buttonGen),50,25);
    g_signal_connect(G_OBJECT(buttonGen),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(buttonGen);
    gtk_table_attach_defaults(bandTable,buttonGen,3,4,2,3);

    buttonWWV = gtk_button_new_with_label ("WWV");
    gtk_widget_modify_bg(buttonWWV, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonWWV);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(buttonWWV),50,25);
    g_signal_connect(G_OBJECT(buttonWWV),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(buttonWWV);
    gtk_table_attach_defaults(bandTable,buttonWWV,0,1,3,4);

    buttonXVTR = gtk_button_new_with_label ("XVTR");
    gtk_widget_modify_bg(buttonXVTR, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonXVTR);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(buttonXVTR),50,25);
    g_signal_connect(G_OBJECT(buttonXVTR),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(buttonXVTR);
    gtk_table_attach_defaults(bandTable,buttonXVTR,1,2,3,4);

    gtk_container_add(GTK_CONTAINER(bandFrame),bandTable);
    gtk_widget_show(bandTable);
    gtk_widget_show(bandFrame);

    return bandFrame;
  
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the bandstack. 
* 
* @param band -- Band value
* @param stack -- Stack index
* @param lo
* @param a -- Frequency A
* @param b -- Frequency B
* @param mode -- Operating mode
* @param filter -- Default filter
*/
/* ----------------------------------------------------------------------------*/
void setBandstack(int band,int stack,long long lo,long long a,long long b,int mode,int filter) {
    int current;
    BANDSTACK_ENTRY* entry;

    current=bandstack[band].current_entry;
    entry=&bandstack[band].entry[current];
    entry->frequencyA=a;
    entry->mode=mode;
    entry->filter=filter;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Save the bandstack state. 
*/
/* ----------------------------------------------------------------------------*/
void bandSaveState() {
    char string[128];
    int current;
    BANDSTACK_ENTRY* entry;

    //save current band info
    current=bandstack[band].current_entry;
    entry=&bandstack[band].entry[current];
    entry->frequencyA=frequencyA;
    entry->mode=mode;
    entry->filter=filter;
    entry->var1Low=filterVar1Low;
    entry->var1High=filterVar1High;
    entry->var2Low=filterVar2Low;
    entry->var2High=filterVar2High;
    entry->step=frequencyIncrement;
    entry->preamp=Preamp;
    entry->spectrumHigh=spectrumMAX;
    entry->spectrumLow=spectrumMIN;
    entry->spectrumStep=spectrumSTEP;
    entry->waterfallHigh=waterfallHighThreshold;
    entry->waterfallLow=waterfallLowThreshold;


    int b;
    int stack;
    char name[128];
    for(b=0;b<BANDS;b++) {
        sprintf(string,"%d",bandstack[b].entries);
        sprintf(name,"band.%d.entries",b);
        setProperty(name,string);

        sprintf(string,"%d",bandstack[b].current_entry);
        sprintf(name,"band.%d.current",b);
        setProperty(name,string);

        for(stack=0;stack<bandstack[b].entries;stack++) {
            entry=bandstack[b].entry;
            entry+=stack;

            sprintf(string,"%lld",entry->frequencyA);
            sprintf(name,"band.%d.stack.%d.a",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->mode);
            sprintf(name,"band.%d.stack.%d.mode",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->filter);
            sprintf(name,"band.%d.stack.%d.filter",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->var1Low);
            sprintf(name,"band.%d.stack.%d.var1Low",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->var1High);
            sprintf(name,"band.%d.stack.%d.var1High",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->var2Low);
            sprintf(name,"band.%d.stack.%d.var2Low",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->var2High);
            sprintf(name,"band.%d.stack.%d.var2High",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->step);
            sprintf(name,"band.%d.stack.%d.step",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->preamp);
            sprintf(name,"band.%d.stack.%d.preamp",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->spectrumHigh);
            sprintf(name,"band.%d.stack.%d.spectrumHigh",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->spectrumLow);
            sprintf(name,"band.%d.stack.%d.spectrumLow",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->spectrumStep);
            sprintf(name,"band.%d.stack.%d.spectrumStep",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->waterfallHigh);
            sprintf(name,"band.%d.stack.%d.waterfallHigh",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",entry->waterfallLow);
            sprintf(name,"band.%d.stack.%d.waterfallLow",b,stack);
            setProperty(name,string);

        }
    }
    sprintf(string,"%d",band);
    setProperty("band",string);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Restore the bandstack state.
*/
/* ----------------------------------------------------------------------------*/
void bandRestoreState() {
    char* value;

    int b;
    int stack;
    char name[128];
    BANDSTACK_ENTRY* entry;
    int current;
 
    bandstack[0].entries=3;
    bandstack[0].current_entry=0;
    bandstack[0].entry=bandstack160;
    bandstack[1].entries=3;
    bandstack[1].current_entry=0;
    bandstack[1].entry=bandstack80;
    bandstack[2].entries=5;
    bandstack[2].current_entry=0;
    bandstack[2].entry=bandstack60;
    bandstack[3].entries=3;
    bandstack[3].current_entry=0;
    bandstack[3].entry=bandstack40;
    bandstack[4].entries=3;
    bandstack[4].current_entry=0;
    bandstack[4].entry=bandstack30;
    bandstack[5].entries=3;
    bandstack[5].current_entry=0;
    bandstack[5].entry=bandstack20;
    bandstack[6].entries=3;
    bandstack[6].current_entry=0;
    bandstack[6].entry=bandstack18;
    bandstack[7].entries=3;
    bandstack[7].current_entry=0;
    bandstack[7].entry=bandstack15;
    bandstack[8].entries=3;
    bandstack[8].current_entry=0;
    bandstack[8].entry=bandstack12;
    bandstack[9].entries=3;
    bandstack[9].current_entry=0;
    bandstack[9].entry=bandstack10;
    bandstack[10].entries=3;
    bandstack[10].current_entry=0;
    bandstack[10].entry=bandstack50;
    bandstack[11].entries=3;
    bandstack[11].current_entry=0;
    bandstack[11].entry=bandstackGEN;
    bandstack[12].entries=5;
    bandstack[12].current_entry=0;
    bandstack[12].entry=bandstackWWV;


    for(b=0;b<BANDS;b++) {

        sprintf(name,"band.%d.entries",b);
        value=getProperty(name);
        if(value) bandstack[b].entries=atoi(value);

        sprintf(name,"band.%d.current",b);
        value=getProperty(name);
        if(value) bandstack[b].current_entry=atoi(value);

        for(stack=0;stack<bandstack[b].entries;stack++) {
            entry=bandstack[b].entry;
            entry+=stack;

            sprintf(name,"band.%d.stack.%d.a",b,stack);
            value=getProperty(name);
            if(value) entry->frequencyA=atoll(value);

            sprintf(name,"band.%d.stack.%d.mode",b,stack);
            value=getProperty(name);
            if(value) entry->mode=atoi(value);

            sprintf(name,"band.%d.stack.%d.filter",b,stack);
            value=getProperty(name);
            if(value) entry->filter=atoi(value);

            sprintf(name,"band.%d.stack.%d.var1Low",b,stack);
            value=getProperty(name);
            if(value) entry->var1Low=atoi(value);

            sprintf(name,"band.%d.stack.%d.var1High",b,stack);
            value=getProperty(name);
            if(value) entry->var1High=atoi(value);

            sprintf(name,"band.%d.stack.%d.var2Low",b,stack);
            value=getProperty(name);
            if(value) entry->var2Low=atoi(value);

            sprintf(name,"band.%d.stack.%d.var2High",b,stack);
            value=getProperty(name);
            if(value) entry->var2High=atoi(value);

            sprintf(name,"band.%d.stack.%d.step",b,stack);
            value=getProperty(name);
            if(value) entry->step=atoi(value);

            sprintf(name,"band.%d.stack.%d.preamp",b,stack);
            value=getProperty(name);
            if(value) entry->preamp=atoi(value);

            sprintf(name,"band.%d.stack.%d.spectrumHigh",b,stack);
            value=getProperty(name);
            if(value) entry->spectrumHigh=atoi(value);

            sprintf(name,"band.%d.stack.%d.spectrumLow",b,stack);
            value=getProperty(name);
            if(value) entry->spectrumLow=atoi(value);

            sprintf(name,"band.%d.stack.%d.spectrumStep",b,stack);
            value=getProperty(name);
            if(value) entry->spectrumStep=atoi(value);

            sprintf(name,"band.%d.stack.%d.waterfallHigh",b,stack);
            value=getProperty(name);
            if(value) entry->waterfallHigh=atoi(value);

            sprintf(name,"band.%d.stack.%d.waterfallLow",b,stack);
            value=getProperty(name);
            if(value) entry->waterfallLow=atoi(value);

        }
    }
    value=getProperty("band");
    if(value) band=atoi(value);
}
