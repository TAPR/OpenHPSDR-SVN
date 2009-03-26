// mode.c

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
#include "mode.h"
#include "property.h"
#include "soundcard.h"

GtkWidget* modeFixed;

int mode;

GtkWidget* buttonLSB;
GtkWidget* buttonUSB;
GtkWidget* buttonDSB;
GtkWidget* buttonCWL;
GtkWidget* buttonCWU;
GtkWidget* buttonAM;
GtkWidget* buttonSAM;
GtkWidget* buttonFMN;
GtkWidget* buttonDIGU;
GtkWidget* buttonSPEC;
GtkWidget* buttonDIGL;

GtkWidget* currentModeButton;

void setMode(int mode);

//-------------------------------------------------------------------------------------------
//
//  select the mode - sends a setMode and setFilter to DSP
//
void selectMode(GtkWidget* widget) {
    GtkWidget* label;
    char temp[80];

    if(currentModeButton) {
        label=gtk_bin_get_child((GtkBin*)currentModeButton);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    }
    label=gtk_bin_get_child((GtkBin*)widget);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &modeButtonSelected);
    currentModeButton=widget;

    if(widget==buttonLSB) {
        mode=modeLSB;
    } else if(widget==buttonUSB) {
        mode=modeUSB;
    } else if(widget==buttonDSB) {
        mode=modeDSB;
    } else if(widget==buttonCWL) {
        mode=modeCWL;
    } else if(widget==buttonCWU) {
        mode=modeCWU;
    } else if(widget==buttonAM) {
        mode=modeAM;
    } else if(widget==buttonSAM) {
        mode=modeSAM;
    } else if(widget==buttonFMN) {
        mode=modeFMN;
    } else if(widget==buttonDIGL) {
        mode=modeDIGL;
    } else if(widget==buttonSPEC) {
        mode=modeSPEC;
    } else if(widget==buttonDIGU) {
        mode=modeDIGU;
    }


    // set RX mode
    sprintf(temp,"setMode %d 0",mode);
    writeCommand(temp);

    setFilter(filter);

}

void setModeMode(int mode) {
    GtkWidget* widget;
    switch(mode) {
        case modeLSB:
            widget=buttonLSB;
            break;
        case modeUSB:
            widget=buttonUSB;
            break;
        case modeDSB:
            widget=buttonDSB;
            break;
        case modeCWL:
            widget=buttonCWL;
            break;
        case modeCWU:
            widget=buttonCWU;
            break;
        case modeAM:
            widget=buttonAM;
            break;
        case modeSAM:
            widget=buttonSAM;
            break;
        case modeFMN:
            widget=buttonFMN;
            break;
        case modeDIGL:
            widget=buttonDIGL;
            break;
        case modeSPEC:
            widget=buttonSPEC;
            break;
        case modeDIGU:
            widget=buttonDIGU;
            break;
    }
    selectMode(widget);
}

//-------------------------------------------------------------------------------------------
//
//  callback when a mode button is pressed
//
void modeCallback(GtkWidget* widget,gpointer data) {
    selectMode(widget);
}

//-------------------------------------------------------------------------------------------
//
// build the GUI
//
GtkWidget* buildModeUI() {
    GtkWidget* label;

    modeFixed=gtk_fixed_new();
    gtk_widget_modify_bg(modeFixed,GTK_STATE_NORMAL,&background);

    // mode buttons
    buttonLSB = gtk_button_new_with_label ("LSB");
    gtk_widget_modify_bg(buttonLSB, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonLSB);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonLSB),50,25);
    g_signal_connect(G_OBJECT(buttonLSB),"clicked",G_CALLBACK(modeCallback),NULL);
    gtk_widget_show(buttonLSB);
    gtk_fixed_put((GtkFixed*)modeFixed,buttonLSB,0,0);

    buttonUSB = gtk_button_new_with_label ("USB");
    gtk_widget_modify_bg(buttonUSB, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonUSB);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonUSB),50,25);
    g_signal_connect(G_OBJECT(buttonUSB),"clicked",G_CALLBACK(modeCallback),NULL);
    gtk_widget_show(buttonUSB);
    gtk_fixed_put((GtkFixed*)modeFixed,buttonUSB,50,0);

    buttonCWL = gtk_button_new_with_label ("CW/L");
    gtk_widget_modify_bg(buttonCWL, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonCWL);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonCWL),50,25);
    g_signal_connect(G_OBJECT(buttonCWL),"clicked",G_CALLBACK(modeCallback),NULL);
    gtk_widget_show(buttonCWL);
    gtk_fixed_put((GtkFixed*)modeFixed,buttonCWL,0,25);

    buttonCWU = gtk_button_new_with_label ("CW/U");
    gtk_widget_modify_bg(buttonCWU, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonCWU);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonCWU),50,25);
    g_signal_connect(G_OBJECT(buttonCWU),"clicked",G_CALLBACK(modeCallback),NULL);
    gtk_widget_show(buttonCWU);
    gtk_fixed_put((GtkFixed*)modeFixed,buttonCWU,50,25);

    buttonDSB = gtk_button_new_with_label ("DSB");
    gtk_widget_modify_bg(buttonDSB, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonDSB);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonDSB),50,25);
    g_signal_connect(G_OBJECT(buttonDSB),"clicked",G_CALLBACK(modeCallback),NULL);
    gtk_widget_show(buttonDSB);
    gtk_fixed_put((GtkFixed*)modeFixed,buttonDSB,0,50);


    buttonFMN = gtk_button_new_with_label ("FM/N");
    gtk_widget_modify_bg(buttonFMN, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonFMN);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonFMN),50,25);
    g_signal_connect(G_OBJECT(buttonFMN),"clicked",G_CALLBACK(modeCallback),NULL);
    gtk_widget_show(buttonFMN);
    gtk_fixed_put((GtkFixed*)modeFixed,buttonFMN,50,50);

    buttonAM = gtk_button_new_with_label ("AM");
    gtk_widget_modify_bg(buttonAM, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonAM);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonAM),50,25);
    g_signal_connect(G_OBJECT(buttonAM),"clicked",G_CALLBACK(modeCallback),NULL);
    gtk_widget_show(buttonAM);
    gtk_fixed_put((GtkFixed*)modeFixed,buttonAM,0,75);

    buttonSAM = gtk_button_new_with_label ("SAM");
    gtk_widget_modify_bg(buttonSAM, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonSAM);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonSAM),50,25);
    g_signal_connect(G_OBJECT(buttonSAM),"clicked",G_CALLBACK(modeCallback),NULL);
    gtk_widget_show(buttonSAM);
    gtk_fixed_put((GtkFixed*)modeFixed,buttonSAM,50,75);

    buttonDIGU = gtk_button_new_with_label ("DIGU");
    gtk_widget_modify_bg(buttonDIGU, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonDIGU);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonDIGU),50,25);
    g_signal_connect(G_OBJECT(buttonDIGU),"clicked",G_CALLBACK(modeCallback),NULL);
    gtk_widget_show(buttonDIGU);
    gtk_fixed_put((GtkFixed*)modeFixed,buttonDIGU,0,100);

    buttonDIGL = gtk_button_new_with_label ("DIGL");
    gtk_widget_modify_bg(buttonDIGL, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonDIGL);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonDIGL),50,25);
    g_signal_connect(G_OBJECT(buttonDIGL),"clicked",G_CALLBACK(modeCallback),NULL);
    gtk_widget_show(buttonDIGL);
    gtk_fixed_put((GtkFixed*)modeFixed,buttonDIGL,50,100);

    buttonSPEC = gtk_button_new_with_label ("SPEC");
    gtk_widget_modify_bg(buttonSPEC, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonSPEC);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonSPEC),50,25);
    g_signal_connect(G_OBJECT(buttonSPEC),"clicked",G_CALLBACK(modeCallback),NULL);
    gtk_widget_show(buttonSPEC);
    gtk_fixed_put((GtkFixed*)modeFixed,buttonSPEC,0,125);

    gtk_widget_set_size_request(GTK_WIDGET(modeFixed),100,150);
    gtk_widget_show(modeFixed);

    //setModeMode(mode);

    return modeFixed;
  
}

void modeSaveState() {
    char string[128];
    sprintf(string,"%d",mode);
    setProperty("mode",string);
}

void modeRestoreState() {
    char* value;
    value=getProperty("mode");
    if(value) mode=atoi(value);
}
