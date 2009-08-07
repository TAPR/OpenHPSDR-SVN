/** 
* @file subrx.c
* @brief Receiver 2 files for GHPSDR
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-11
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
#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include "bandstack.h"
#include "command.h"
#include "subrx.h"
#include "main.h"
#include "property.h"
#include "ozy.h"
#include "dttsp.h"
#include "vfo.h"

GtkWidget* subrxFrame;
GtkWidget* subrxTable;

GtkWidget* subrxEnabled;
GtkWidget* subrxGainFrame;
GtkWidget* subrxGainScale;
GtkWidget* subrxPanFrame;
GtkWidget* subrxPanScale;

float subrxGain=10.0;
float subrxPan=50.0;

gboolean subrx=FALSE;

/* --------------------------------------------------------------------------*/
/** 
* @brief Enabled button Callback 
* 
* @param widget -- pointer to the parent widget, 
* @param data -- pointer to the data.
*/
void subrxEnabledButtonCallback(GtkWidget* widget,gpointer data) {
    GtkWidget* label;
    char c[80];
    gboolean state;

fprintf(stderr,"subrxEnabledButtonCallback\n");
    if(subrx) {
        state=0;
    } else {
        state=1;
    }    

    label=gtk_bin_get_child((GtkBin*)widget);
    if(state) {
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);
    } else {
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
    }

    vfoSubRx(state);
    subrx=state;

    if(subrx) {
        long long diff=frequencyA-frequencyB;
        if(diff<0) diff=-diff;
        if(diff>sampleRate/2) {
            setBFrequency(frequencyA);
        }
        SetSubRXSt(0,1,TRUE);
    } else {
        SetSubRXSt(0,1,FALSE);
    }
}

/* --------------------------------------------------------------------------*/
/**
* @brief  Callback when subrx gain values changes
*
* @param widget
* @param data
*/
void subrxGainChanged(GtkWidget* widget,gpointer data) {
    char command[80];
    subrxGain=gtk_range_get_value((GtkRange*)subrxGainScale);
    SetRXOutputGain(0,1,subrxGain/100.0);
}

/* --------------------------------------------------------------------------*/
/**
* @brief  Callback when subrx pan values changes
*
* @param widget
* @param data
*/
void subrxPanChanged(GtkWidget* widget,gpointer data) {
    char command[80];
    subrxPan=gtk_range_get_value((GtkRange*)subrxPanScale);
    SetRXPan(0,1,subrxPan);

}



/* --------------------------------------------------------------------------*/
/** 
* @brief Build Transmit User Interface 
* 
* @return GtkWidget pointer 
*/
GtkWidget* buildSubRxUI() {

    GtkWidget* label;

fprintf(stderr,"buildSubRxUI\n");
    subrxFrame=gtk_frame_new("Sub RX");
    gtk_widget_modify_bg(subrxFrame,GTK_STATE_NORMAL,&background);
    gtk_widget_modify_fg(gtk_frame_get_label_widget(GTK_FRAME(subrxFrame)),GTK_STATE_NORMAL,&white);

    subrxTable=gtk_table_new(1,7,TRUE);

    // subrx settings
    subrxEnabled = gtk_button_new_with_label ("SubRX");
    gtk_widget_modify_bg(subrxEnabled, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)subrxEnabled);

    if(subrx) {
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);
    } else {
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
    }

    gtk_widget_set_size_request(GTK_WIDGET(subrxEnabled),50,25);
    g_signal_connect(G_OBJECT(subrxEnabled),"clicked",G_CALLBACK(subrxEnabledButtonCallback),NULL);
    gtk_widget_show(subrxEnabled);
    gtk_table_attach_defaults(GTK_TABLE(subrxTable),subrxEnabled,0,1,0,1);

    // subrx gain
    subrxGainFrame=gtk_frame_new("AF Gain");
    gtk_widget_modify_bg(subrxGainFrame,GTK_STATE_NORMAL,&background);
    gtk_widget_modify_fg(gtk_frame_get_label_widget(GTK_FRAME(subrxGainFrame)),GTK_STATE_NORMAL,&white);

    subrxGainScale=gtk_hscale_new_with_range(0.0,100.0,10.0);
    g_signal_connect(G_OBJECT(subrxGainScale),"value-changed",G_CALLBACK(subrxGainChanged),NULL);
    gtk_range_set_value((GtkRange*)subrxGainScale,subrxGain);
    gtk_widget_set_size_request(GTK_WIDGET(subrxGainScale),150,30);
    gtk_widget_show(subrxGainScale);
    gtk_container_add(GTK_CONTAINER(subrxGainFrame),subrxGainScale);
    gtk_widget_show(subrxGainFrame);
    gtk_table_attach_defaults(GTK_TABLE(subrxTable),subrxGainFrame,1,4,0,1);

    SetRXOutputGain(0,1,subrxGain/100.0);

    // subrx pan
    subrxPanFrame=gtk_frame_new("AF Pan");
    gtk_widget_modify_bg(subrxPanFrame,GTK_STATE_NORMAL,&background);
    gtk_widget_modify_fg(gtk_frame_get_label_widget(GTK_FRAME(subrxPanFrame)),GTK_STATE_NORMAL,&white);

    subrxPanScale=gtk_hscale_new_with_range(0.0,1.0,0.1);
    g_signal_connect(G_OBJECT(subrxPanScale),"value-changed",G_CALLBACK(subrxPanChanged),NULL);
    gtk_range_set_value((GtkRange*)subrxPanScale,subrxPan);
    gtk_widget_set_size_request(GTK_WIDGET(subrxPanScale),150,30);
    gtk_widget_show(subrxPanScale);
    gtk_container_add(GTK_CONTAINER(subrxPanFrame),subrxPanScale);
    gtk_widget_show(subrxPanFrame);
    gtk_table_attach_defaults(GTK_TABLE(subrxTable),subrxPanFrame,4,7,0,1);

    SetRXPan(0,1,subrxPan);

    gtk_container_add(GTK_CONTAINER(subrxFrame),subrxTable);
    gtk_widget_show(subrxTable);
    gtk_widget_show(subrxFrame);

    return subrxFrame;

}

/* --------------------------------------------------------------------------*/
/**
* @brief Save the sub rx state
*/
void subrxSaveState() {
    char string[128];
    sprintf(string,"%d",subrx);
    setProperty("subrx",string);
    sprintf(string,"%f",subrxGain);
    setProperty("subrxGain",string);
    sprintf(string,"%f",subrxPan);
    setProperty("subrxPan",string);
}

/* --------------------------------------------------------------------------*/
/**
* @brief Restore the sub rx state
*/
void subrxRestoreState() {
    char* value;
    value=getProperty("subrxGain");
    if(value) subrxGain=atof(value); else subrxGain=10.0f;
    value=getProperty("subrxPan");
    if(value) subrxPan=atof(value); else subrxPan=50.0f;
    value=getProperty("subrx");
    if(value) subrx=atoi(value); else subrx=0;

    SetRXPan(0,1,subrxPan);
    SetRXOutputGain(0,1,subrxGain/100.0);
}

void resetSubRx() {
    if(subrx) {
        subrxEnabledButtonCallback(subrxEnabled,NULL);
    }
}
