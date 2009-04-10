// agc.c

#include <gtk/gtk.h>
#include <gdk/gdkkeysyms.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <getopt.h>

#include "agc.h"
#include "bandstack.h"
#include "command.h"
#include "main.h"
#include "property.h"

GtkWidget* agcFixed;

int agc=agcLONG;

GtkWidget* buttonSLOW;
GtkWidget* buttonMEDIUM;
GtkWidget* buttonFAST;
GtkWidget* buttonLONG;

GtkWidget* currentAgcButton;

void setAgc(int agc);

//-------------------------------------------------------------------------------------------
//
//  select the agc - sends a setAgc
//
void selectAgc(GtkWidget* widget) {
    GtkWidget* label;
    char temp[80];

    if(currentAgcButton) {
        label=gtk_bin_get_child((GtkBin*)currentAgcButton);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
    }
    label=gtk_bin_get_child((GtkBin*)widget);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
    gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);
    currentAgcButton=widget;

    if(widget==buttonSLOW) {
        agc=agcSLOW;
    } else if(widget==buttonMEDIUM) {
        agc=agcMEDIUM;
    } else if(widget==buttonFAST) {
        agc=agcFAST;
    } else if(widget==buttonLONG) {
        agc=agcLONG;
    }


    // set RX agc
    sprintf(temp,"setRXAGC %d",agc);
    writeCommand(temp);

}

void setAgc(int agc) {
    GtkWidget* widget;
    switch(agc) {
        case agcSLOW:
            widget=buttonSLOW;
            break;
        case agcMEDIUM:
            widget=buttonMEDIUM;
            break;
        case agcFAST:
            widget=buttonFAST;
            break;
        case agcLONG:
            widget=buttonLONG;
            break;
    }
    selectAgc(widget);
}

//-------------------------------------------------------------------------------------------
//
//  callback when a agc button is pressed
//
void agcCallback(GtkWidget* widget,gpointer data) {
    selectAgc(widget);
}

//-------------------------------------------------------------------------------------------
//
// build the GUI
//
GtkWidget* buildAgcUI() {
    GtkWidget* label;

    agcFixed=gtk_fixed_new();
    gtk_widget_modify_bg(agcFixed,GTK_STATE_NORMAL,&background);

    // agc buttons
    buttonSLOW = gtk_button_new_with_label ("SLOW");
    gtk_widget_modify_bg(buttonSLOW, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonSLOW);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonSLOW),50,25);
    g_signal_connect(G_OBJECT(buttonSLOW),"clicked",G_CALLBACK(agcCallback),NULL);
    gtk_widget_show(buttonSLOW);
    gtk_fixed_put((GtkFixed*)agcFixed,buttonSLOW,0,0);

    buttonMEDIUM = gtk_button_new_with_label ("MEDIUM");
    gtk_widget_modify_bg(buttonMEDIUM, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonMEDIUM);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonMEDIUM),50,25);
    g_signal_connect(G_OBJECT(buttonMEDIUM),"clicked",G_CALLBACK(agcCallback),NULL);
    gtk_widget_show(buttonMEDIUM);
    gtk_fixed_put((GtkFixed*)agcFixed,buttonMEDIUM,50,0);

    buttonFAST = gtk_button_new_with_label ("FAST");
    gtk_widget_modify_bg(buttonFAST, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonFAST);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonFAST),50,25);
    g_signal_connect(G_OBJECT(buttonFAST),"clicked",G_CALLBACK(agcCallback),NULL);
    gtk_widget_show(buttonFAST);
    gtk_fixed_put((GtkFixed*)agcFixed,buttonFAST,0,25);

    buttonLONG = gtk_button_new_with_label ("LONG");
    gtk_widget_modify_bg(buttonLONG, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonLONG);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonLONG),50,25);
    g_signal_connect(G_OBJECT(buttonLONG),"clicked",G_CALLBACK(agcCallback),NULL);
    gtk_widget_show(buttonLONG);
    gtk_fixed_put((GtkFixed*)agcFixed,buttonLONG,50,25);

    gtk_widget_set_size_request(GTK_WIDGET(agcFixed),100,50);
    gtk_widget_show(agcFixed);

    setAgc(agc);

    return agcFixed;
  
}

void agcSaveState() {
    char string[128];
    sprintf(string,"%d",agc);
    setProperty("agc",string);
}

void agcRestoreState() {
    char* value;
    value=getProperty("agc");
    if(value) agc=atoi(value);
}
