// bandscope_control.c

#include <gtk/gtk.h>
#include <gdk/gdkkeysyms.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <getopt.h>

#include "main.h"
#include "bandscope.h"
#include "bandscope_control.h"
#include "property.h"

GtkWidget* zoomFixed;

GtkWidget* buttonZoom1;
GtkWidget* buttonZoom2;
GtkWidget* buttonZoom4;

GtkWidget* currentZoomButton;

//-------------------------------------------------------------------------------------------
//
//  select the zoom
//
void selectZoom(GtkWidget* widget) {
    GtkWidget* label;
    char temp[80];

    if(currentZoomButton) {
        label=gtk_bin_get_child((GtkBin*)currentZoomButton);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
    }
    label=gtk_bin_get_child((GtkBin*)widget);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &modeButtonSelected);
    gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &modeButtonSelected);
    currentZoomButton=widget;

    if(widget==buttonZoom1) {
        bandscopeSetZoom(1);
    } else if(widget==buttonZoom2) {
        bandscopeSetZoom(2);
    } else if(widget==buttonZoom4) {
        bandscopeSetZoom(4);
    }
}

void setZoom(int zoom) {
    GtkWidget* widget;
    switch(zoom) {
        case 1:
            widget=buttonZoom1;
            break;
        case 2:
            widget=buttonZoom2;
            break;
        case 4:
            widget=buttonZoom4;
            break;
    }
    selectZoom(widget);
}

//-------------------------------------------------------------------------------------------
//
//  callback when a zoom button is pressed
//
void zoomCallback(GtkWidget* widget,gpointer data) {
    selectZoom(widget);
}

//-------------------------------------------------------------------------------------------
//
// build the GUI
//
GtkWidget* buildBandscope_controlUI() {
    GtkWidget* label;

    zoomFixed=gtk_fixed_new();
    gtk_widget_modify_bg(zoomFixed,GTK_STATE_NORMAL,&background);

    // zoom buttons
    buttonZoom1 = gtk_button_new_with_label ("Zoom x1");
    gtk_widget_modify_bg(buttonZoom1, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonZoom1);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonZoom1),75,25);
    g_signal_connect(G_OBJECT(buttonZoom1),"clicked",G_CALLBACK(zoomCallback),NULL);
    gtk_widget_show(buttonZoom1);
    gtk_fixed_put((GtkFixed*)zoomFixed,buttonZoom1,0,0);

    buttonZoom2 = gtk_button_new_with_label ("Zoom x2");
    gtk_widget_modify_bg(buttonZoom2, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonZoom2);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonZoom2),75,25);
    g_signal_connect(G_OBJECT(buttonZoom2),"clicked",G_CALLBACK(zoomCallback),NULL);
    gtk_widget_show(buttonZoom2);
    gtk_fixed_put((GtkFixed*)zoomFixed,buttonZoom2,75,0);

/*
    buttonZoom4 = gtk_button_new_with_label ("Zoom x4");
    gtk_widget_modify_bg(buttonZoom4, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonZoom4);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonZoom4),75,25);
    g_signal_connect(G_OBJECT(buttonZoom4),"clicked",G_CALLBACK(zoomCallback),NULL);
    gtk_widget_show(buttonZoom4);
    gtk_fixed_put((GtkFixed*)zoomFixed,buttonZoom4,150,0);
*/

    gtk_widget_set_size_request(GTK_WIDGET(zoomFixed),225,25);
    gtk_widget_show(zoomFixed);

    setZoom(bandscopeZoom);

    return zoomFixed;
  
}

void bandscope_controlSaveState() {
    char string[128];
    sprintf(string,"%d",bandscopeZoom);
    setProperty("bandscope.zoom",string);
}

void bandscope_controlRestoreState() {
    char* value;
    value=getProperty("bandscope.zoom");
    if(value) bandscopeZoom=atoi(value);
}
