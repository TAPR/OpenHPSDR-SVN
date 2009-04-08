// band.c

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

GtkWidget* bandFixed;

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

GtkWidget* currentBandButton;

BANDSTACK bandstack[BANDS][BANDSTACKS]= {
    {{1810000LL,modeCWL,5,-2800,-200,-2800,-200,100},
     {1835000LL,modeCWU,5,-2800,-200,-2800,-200,100},
     {1845000LL,modeUSB,2,-2800,-200,-2800,-200,100}},
    {{3501000LL,modeCWL,5,-2800,-200,-2800,-200,100},
     {3751000LL,modeLSB,2,-2800,-200,-2800,-200,100},
     {3850000LL,modeLSB,2,-2800,-200,-2800,-200,100}},
    {{5330500LL,modeUSB,2,-2800,-200,-2800,-200,100},
     {5346500LL,modeUSB,2,-2800,-200,-2800,-200,100},
     {5356500LL,modeUSB,2,-2800,-200,-2800,-200,100}},
    {{7001000LL,modeCWL,5,-2800,-200,-2800,-200,100},
     {7152000LL,modeLSB,2,-2800,-200,-2800,-200,100},
     {7255000LL,modeLSB,2,-2800,-200,-2800,-200,100}},
    {{10120000LL,modeCWU,5,200,2800,200,2800,100},
     {10130000LL,modeCWU,5,200,2800,200,2800,100},
     {10140000LL,modeCWU,5,200,2800,200,2800,100}},
    {{14010000LL,modeCWU,5,200,2800,200,2800,100},
     {14230000LL,modeUSB,2,200,2800,200,2800,100},
     {14336000LL,modeUSB,2,200,2800,200,2800,100}},
    {{18068600LL,modeCWU,5,200,2800,200,2800,100},
     {18125000LL,modeUSB,2,200,2800,200,2800,100},
     {18140000LL,modeUSB,2,200,2800,200,2800,100}},
    {{21001000LL,modeCWU,5,200,2800,200,2800,100},
     {21255000LL,modeUSB,2,200,2800,200,2800,100},
     {21300000LL,modeUSB,2,200,2800,200,2800,100}},
    {{24895000LL,modeCWU,5,200,2800,200,2800,100},
     {24900000LL,modeUSB,2,200,2800,200,2800,100},
     {24910000LL,modeUSB,2,200,2800,200,2800,100}},
    {{28010000LL,modeCWU,5,200,2800,200,2800,100},
     {28300000LL,modeUSB,2,200,2800,200,2800,100},
     {28400000LL,modeUSB,2,200,2800,200,2800,100}},
    {{50010000LL,modeCWU,5,200,2800,200,2800,100},
     {50125000LL,modeUSB,2,200,2800,200,2800,100},
     {50200000LL,modeUSB,2,200,2800,200,2800,100}},
    {{909000LL,modeAM,0,-6000,6000,-6000,6000,1000},
     {5975000LL,modeAM,0,-6000,6000,-6000,6000,1000},
     {13845000LL,modeAM,0,-6000,6000,-6000,6000,1000}},
    {{5000000LL,modeSAM,2,200,2800,200,2800,1000},
     {10000000LL,modeSAM,2,200,2800,200,2800,1000},
     {15000000LL,modeSAM,2,200,2800,200,2800,1000}}
};

int currentStack[BANDS]={0,0,0,0,0,0,0,0,0,0,0,0};

void setTuningMode(int mode);
void setBand(int band);
void setIncrement(int increment);

//-------------------------------------------------------------------------------------------
//
//  select the band
//
void selectBand(GtkWidget* widget) {
    GtkWidget* label;

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
        bandstack[band][currentStack[band]].frequencyA=frequencyA;
        bandstack[band][currentStack[band]].mode=mode;
        bandstack[band][currentStack[band]].filter=filter;
        bandstack[band][currentStack[band]].var1Low=filterVar1Low;
        bandstack[band][currentStack[band]].var1High=filterVar1High;
        bandstack[band][currentStack[band]].var2Low=filterVar2Low;
        bandstack[band][currentStack[band]].var2High=filterVar2High;
        bandstack[band][currentStack[band]].step=frequencyIncrement;
    }

    if(currentBandButton==widget) {
        currentStack[band]++;
        if(currentStack[band]>=BANDSTACKS) {
            currentStack[band]=0;
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
        }
    }

    setModeMode(bandstack[band][currentStack[band]].mode);
    filterVar1Low=bandstack[band][currentStack[band]].var1Low;
    filterVar1High=bandstack[band][currentStack[band]].var1High;
    filterVar2Low=bandstack[band][currentStack[band]].var2Low;
    filterVar2High=bandstack[band][currentStack[band]].var2High;
    setFilter(bandstack[band][currentStack[band]].filter);
    setAFrequency(bandstack[band][currentStack[band]].frequencyA);
    setIncrement(bandstack[band][currentStack[band]].step);

}

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
    }
    selectBand(widget);
}

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

//-------------------------------------------------------------------------------------------
//
//  callback when a band button is pressed
//
void bandCallback(GtkWidget* widget,gpointer data) {
    selectBand(widget);
}

//-------------------------------------------------------------------------------------------
//
// build the GUI
//
GtkWidget* buildBandUI() {
    GtkWidget* label;

    bandFixed=gtk_fixed_new();
    gtk_widget_modify_bg(bandFixed,GTK_STATE_NORMAL,&background);

    // band selection
    button160 = gtk_button_new_with_label ("160");
    gtk_widget_modify_bg(button160, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button160);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button160),34,25);
    g_signal_connect(G_OBJECT(button160),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button160);
    gtk_fixed_put((GtkFixed*)bandFixed,button160,0,0);

    button80 = gtk_button_new_with_label ("80");
    gtk_widget_modify_bg(button80, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button80);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button80),34,25);
    g_signal_connect(G_OBJECT(button80),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button80);
    gtk_fixed_put((GtkFixed*)bandFixed,button80,34,0);

    button60 = gtk_button_new_with_label ("60");
    gtk_widget_modify_bg(button60, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button60);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button60),34,25);
    g_signal_connect(G_OBJECT(button60),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button60);
    gtk_fixed_put((GtkFixed*)bandFixed,button60,68,0);

    button40 = gtk_button_new_with_label ("40");
    gtk_widget_modify_bg(button40, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button40);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button40),34,25);
    g_signal_connect(G_OBJECT(button40),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button40);
    gtk_fixed_put((GtkFixed*)bandFixed,button40,0,25);

    button30 = gtk_button_new_with_label ("30");
    gtk_widget_modify_bg(button30, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button30);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button30),34,25);
    g_signal_connect(G_OBJECT(button30),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button30);
    gtk_fixed_put((GtkFixed*)bandFixed,button30,34,25);

    button20 = gtk_button_new_with_label ("20");
    gtk_widget_modify_bg(button20, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button20);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button20),34,25);
    g_signal_connect(G_OBJECT(button20),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button20);
    gtk_fixed_put((GtkFixed*)bandFixed,button20,68,25);

    button17 = gtk_button_new_with_label ("17");
    gtk_widget_modify_bg(button17, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button17);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button17),34,25);
    g_signal_connect(G_OBJECT(button17),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button17);
    gtk_fixed_put((GtkFixed*)bandFixed,button17,0,50);

    button15 = gtk_button_new_with_label ("15");
    gtk_widget_modify_bg(button15, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button15);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button15),34,25);
    g_signal_connect(G_OBJECT(button15),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button15);
    gtk_fixed_put((GtkFixed*)bandFixed,button15,34,50);

    button12 = gtk_button_new_with_label ("12");
    gtk_widget_modify_bg(button12, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button12);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button12),34,25);
    g_signal_connect(G_OBJECT(button12),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button12);
    gtk_fixed_put((GtkFixed*)bandFixed,button12,68,50);

    button10 = gtk_button_new_with_label ("10");
    gtk_widget_modify_bg(button10, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button10);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button10),34,25);
    g_signal_connect(G_OBJECT(button10),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button10);
    gtk_fixed_put((GtkFixed*)bandFixed,button10,0,75);

    button6 = gtk_button_new_with_label ("6");
    gtk_widget_modify_bg(button6, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)button6);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(button6),34,25);
    g_signal_connect(G_OBJECT(button6),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(button6);
    gtk_fixed_put((GtkFixed*)bandFixed,button6,34,75);

    buttonGen = gtk_button_new_with_label ("GEN");
    gtk_widget_modify_bg(buttonGen, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonGen);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(buttonGen),34,25);
    g_signal_connect(G_OBJECT(buttonGen),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(buttonGen);
    gtk_fixed_put((GtkFixed*)bandFixed,buttonGen,68,75);

    buttonWWV = gtk_button_new_with_label ("WWV");
    gtk_widget_modify_bg(buttonWWV, GTK_STATE_NORMAL, &bandButtonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonWWV);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &black);
    gtk_widget_set_size_request(GTK_WIDGET(buttonWWV),34,25);
    g_signal_connect(G_OBJECT(buttonWWV),"clicked",G_CALLBACK(bandCallback),NULL);
    gtk_widget_show(buttonWWV);
    gtk_fixed_put((GtkFixed*)bandFixed,buttonWWV,0,100);

    gtk_widget_set_size_request(GTK_WIDGET(bandFixed),200,125);
    gtk_widget_show(bandFixed);

    return bandFixed;
  
}

void setBandstack(int band,int stack,long long lo,long long a,long long b,int mode,int filter) {
    bandstack[band][stack].frequencyA=a;
    bandstack[band][stack].mode=mode;
    bandstack[band][stack].filter=filter;
}

void bandSaveState() {
    char string[128];

    //save current band info
    bandstack[band][currentStack[band]].frequencyA=frequencyA;
    bandstack[band][currentStack[band]].mode=mode;
    bandstack[band][currentStack[band]].filter=filter;
    bandstack[band][currentStack[band]].var1Low=filterVar1Low;
    bandstack[band][currentStack[band]].var1High=filterVar1High;
    bandstack[band][currentStack[band]].var2Low=filterVar2Low;
    bandstack[band][currentStack[band]].var2High=filterVar2High;
    bandstack[band][currentStack[band]].step=frequencyIncrement;


    int b;
    int stack;
    char name[128];
    for(b=0;b<BANDS;b++) {
        for(stack=0;stack<BANDSTACKS;stack++) {

            sprintf(string,"%lld",bandstack[b][stack].frequencyA);
            sprintf(name,"band.%d.stack.%d.a",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",bandstack[b][stack].mode);
            sprintf(name,"band.%d.stack.%d.mode",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",bandstack[b][stack].filter);
            sprintf(name,"band.%d.stack.%d.filter",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",bandstack[b][stack].var1Low);
            sprintf(name,"band.%d.stack.%d.var1Low",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",bandstack[b][stack].var1High);
            sprintf(name,"band.%d.stack.%d.var1High",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",bandstack[b][stack].var2Low);
            sprintf(name,"band.%d.stack.%d.var2Low",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",bandstack[b][stack].var2High);
            sprintf(name,"band.%d.stack.%d.var2High",b,stack);
            setProperty(name,string);

            sprintf(string,"%d",bandstack[b][stack].step);
            sprintf(name,"band.%d.stack.%d.step",b,stack);
            setProperty(name,string);
        }
        sprintf(string,"%d",currentStack[b]);
        sprintf(name,"band.%d.current.stack",b);
        setProperty(name,string);
    }
    sprintf(string,"%d",band);
    setProperty("band",string);
}

void bandRestoreState() {
    char* value;

    int b;
    int stack;
    char name[128];
 
    for(b=0;b<BANDS;b++) {
        for(stack=0;stack<BANDSTACKS;stack++) {
            sprintf(name,"band.%d.stack.%d.a",b,stack);
            value=getProperty(name);
            if(value) bandstack[b][stack].frequencyA=atoll(value);

            sprintf(name,"band.%d.stack.%d.mode",b,stack);
            value=getProperty(name);
            if(value) bandstack[b][stack].mode=atoi(value);

            sprintf(name,"band.%d.stack.%d.filter",b,stack);
            value=getProperty(name);
            if(value) bandstack[b][stack].filter=atoi(value);

            sprintf(name,"band.%d.stack.%d.var1Low",b,stack);
            value=getProperty(name);
            if(value) bandstack[b][stack].var1Low=atoi(value);

            sprintf(name,"band.%d.stack.%d.var1High",b,stack);
            value=getProperty(name);
            if(value) bandstack[b][stack].var1High=atoi(value);

            sprintf(name,"band.%d.stack.%d.var2Low",b,stack);
            value=getProperty(name);
            if(value) bandstack[b][stack].var2Low=atoi(value);

            sprintf(name,"band.%d.stack.%d.var2High",b,stack);
            value=getProperty(name);
            if(value) bandstack[b][stack].var2High=atoi(value);

            sprintf(name,"band.%d.stack.%d.step",b,stack);
            value=getProperty(name);
            if(value) bandstack[b][stack].step=atoi(value);
        }
        sprintf(name,"band.%d.current.stack",b);
        value=getProperty(name);
        if(value) currentStack[b]=atoi(value);
    }
    value=getProperty("band");
    if(value) band=atoi(value);
}
