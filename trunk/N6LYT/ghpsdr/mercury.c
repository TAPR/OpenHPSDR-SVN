#include <gtk/gtk.h>
#include <math.h>
#include <stdio.h>
#include "bandstack.h"
#include "command.h"
#include "dttsp.h"
#include "filter.h"
#include "main.h"
#include "mercury.h"
#include "mode.h"
#include "property.h"
#include "ozy.h"
#include "soundcard.h"

GtkWidget* mercuryFixed;

GtkWidget* buttonR48K;
GtkWidget* buttonR96K;
GtkWidget* buttonR192K;
GtkWidget* buttonDither;
GtkWidget* buttonRandom;
GtkWidget* buttonPreamp;

gboolean R48K=FALSE;
gboolean R96K=TRUE;
gboolean R192K=FALSE;
gboolean Dither=FALSE;
gboolean Random=FALSE;
gboolean Preamp=FALSE;

float preampOffset=0.0f;

void mercuryButtonCallback(GtkWidget* widget,gpointer data) {
    GtkWidget* label;
    char command[80];
    gboolean state;

    if(widget==buttonR48K) {
        R48K=TRUE;
        R96K=FALSE;
        R192K=FALSE;
        label=gtk_bin_get_child((GtkBin*)buttonR48K);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);
        label=gtk_bin_get_child((GtkBin*)buttonR96K);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
        label=gtk_bin_get_child((GtkBin*)buttonR192K);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
        sampleRate=48000;
        setSpeed(0);
        SetSampleRate((double)sampleRate);
        writeCommand("setOsc 0");
        setFilter(filter);
        setModeMode(mode);
    } else if(widget==buttonR96K) {
        R48K=FALSE;
        R96K=TRUE;
        R192K=FALSE;
        label=gtk_bin_get_child((GtkBin*)buttonR48K);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
        label=gtk_bin_get_child((GtkBin*)buttonR96K);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
        label=gtk_bin_get_child((GtkBin*)buttonR192K);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
        sampleRate=96000;
        setSpeed(1);
        SetSampleRate((double)sampleRate);
        writeCommand("setOsc 0");
        setFilter(filter);
        setModeMode(mode);
    } else if(widget==buttonR192K) {
        R48K=FALSE;
        R96K=FALSE;
        R192K=TRUE;
        label=gtk_bin_get_child((GtkBin*)buttonR48K);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
        label=gtk_bin_get_child((GtkBin*)buttonR96K);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
        label=gtk_bin_get_child((GtkBin*)buttonR192K);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);
        sampleRate=192000;
        setSpeed(2);
        SetSampleRate((double)sampleRate);
        writeCommand("setOsc 0");
        setFilter(filter);
        setModeMode(mode);
    } else {
        if(widget==buttonDither) {
            Dither=!Dither;
            setLT2208Dither(Dither);
            state=Dither;
        } else if(widget==buttonRandom) {
            Random=!Random;
            setLT2208Random(Random);
            state=Random;
        } else if(widget==buttonPreamp) {
            Preamp=!Preamp;
            setPreampGain(Preamp);
            state=Preamp;
            if(state) {
                preampOffset=-20.0f; // dB
            } else {
                preampOffset=0.0f; // dB
            }
        }

        label=gtk_bin_get_child((GtkBin*)widget);
        if(state) {
            gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
            gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);
        } else {
            gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
            gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
        }
    }
}

GtkWidget* buildMercuryUI() {

    GtkWidget* label;

    mercuryFixed=gtk_fixed_new();
    gtk_widget_modify_bg(mercuryFixed,GTK_STATE_NORMAL,&background);

    // mercury settings
    buttonR48K = gtk_button_new_with_label ("48K");
    gtk_widget_modify_bg(buttonR48K, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonR48K);
    gtk_widget_set_size_request(GTK_WIDGET(buttonR48K),50,25);
    g_signal_connect(G_OBJECT(buttonR48K),"clicked",G_CALLBACK(mercuryButtonCallback),NULL);
    gtk_widget_show(buttonR48K);
    gtk_fixed_put((GtkFixed*)mercuryFixed,buttonR48K,5,0);

    buttonR96K = gtk_button_new_with_label ("96K");
    gtk_widget_modify_bg(buttonR96K, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonR96K);
    gtk_widget_set_size_request(GTK_WIDGET(buttonR96K),50,25);
    g_signal_connect(G_OBJECT(buttonR96K),"clicked",G_CALLBACK(mercuryButtonCallback),NULL);
    gtk_widget_show(buttonR96K);
    gtk_fixed_put((GtkFixed*)mercuryFixed,buttonR96K,5,25);

    buttonR192K = gtk_button_new_with_label ("192K");
    gtk_widget_modify_bg(buttonR192K, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonR192K);
    gtk_widget_set_size_request(GTK_WIDGET(buttonR192K),50,25);
    g_signal_connect(G_OBJECT(buttonR192K),"clicked",G_CALLBACK(mercuryButtonCallback),NULL);
    gtk_widget_show(buttonR192K);
    gtk_fixed_put((GtkFixed*)mercuryFixed,buttonR192K,5,50);

    buttonDither = gtk_button_new_with_label ("Dither");
    gtk_widget_modify_bg(buttonDither, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonDither);
    if(Dither) {
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
    } else {
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    }
    gtk_widget_set_size_request(GTK_WIDGET(buttonDither),50,25);
    g_signal_connect(G_OBJECT(buttonDither),"clicked",G_CALLBACK(mercuryButtonCallback),NULL);
    gtk_widget_show(buttonDither);
    gtk_fixed_put((GtkFixed*)mercuryFixed,buttonDither,55,0);

    buttonRandom = gtk_button_new_with_label ("Random");
    gtk_widget_modify_bg(buttonRandom, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonRandom);
    if(Random) {
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
    } else {
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    }
    gtk_widget_set_size_request(GTK_WIDGET(buttonRandom),50,25);
    g_signal_connect(G_OBJECT(buttonRandom),"clicked",G_CALLBACK(mercuryButtonCallback),NULL);
    gtk_widget_show(buttonRandom);
    gtk_fixed_put((GtkFixed*)mercuryFixed,buttonRandom,55,25);

    buttonPreamp = gtk_button_new_with_label ("Preamp");
    gtk_widget_modify_bg(buttonPreamp, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonPreamp);
    if(Preamp) {
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
    } else {
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    }
    gtk_widget_set_size_request(GTK_WIDGET(buttonPreamp),50,25);
    g_signal_connect(G_OBJECT(buttonPreamp),"clicked",G_CALLBACK(mercuryButtonCallback),NULL);
    gtk_widget_show(buttonPreamp);
    gtk_fixed_put((GtkFixed*)mercuryFixed,buttonPreamp,55,50);

    gtk_widget_set_size_request(GTK_WIDGET(mercuryFixed),110,75);
    gtk_widget_show(mercuryFixed);


    // setup
    if(sampleRate==48000) {
        mercuryButtonCallback(buttonR48K,NULL) ;
    } else if(sampleRate==96000) {
        mercuryButtonCallback(buttonR96K,NULL) ;
    } else if(sampleRate==192000) {
        mercuryButtonCallback(buttonR192K,NULL) ;
    }
    
    return mercuryFixed;
}

void mercurySaveState() {
    char string[128];

    sprintf(string,"%d",sampleRate);
    setProperty("sampleRate",string);

    sprintf(string,"%d",Dither);
    setProperty("Dither",string);

    sprintf(string,"%d",Random);
    setProperty("Random",string);

    sprintf(string,"%d",Preamp);
    setProperty("Preamp",string);
    
}

void mercuryRestoreState() {
    char* value;

    value=getProperty("sampleRate");
    if(value) sampleRate=atoi(value);

    value=getProperty("Dither");
    if(value) Dither=atoi(value);
    setLT2208Dither(Dither);
    
    value=getProperty("Random");
    if(value) Random=atoi(value);
    setLT2208Random(Random);
    
    value=getProperty("Preamp");
    if(value) Preamp=atoi(value);
    setPreampGain(Preamp);
    if(Preamp) {
        preampOffset=-20.0f; // dB
    } else {
        preampOffset=0.0f; // dB
    }

}


void setPreamp(gboolean state) {
    if(Preamp!=state) {
        mercuryButtonCallback(buttonPreamp,NULL);
    }
}
