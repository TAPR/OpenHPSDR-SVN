/** 
* @file clock10.c
* @brief 10 MHz clock source access functions
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
#include <string.h>
#include "bandstack.h"
#include "command.h"
#include "dttsp.h"
#include "filter.h"
#include "main.h"
#include "clock10.h"
#include "mode.h"
#include "property.h"
#include "ozy.h"
#include "soundcard.h"
#include "volume.h"

GtkWidget* clock10Frame;
GtkWidget* clock10Table;

GtkWidget* buttonClock10Atlas;
GtkWidget* buttonClock10Penelope;
GtkWidget* buttonClock10Mercury;

gboolean clock10Atlas=FALSE;
gboolean clock10Penelope=FALSE;
gboolean clock10Mercury=FALSE;

/* --------------------------------------------------------------------------*/
/** 
* @brief Clock 10 MHz button callback 
* 
* @param widget
* @param data
*/
void clock10ButtonCallback(GtkWidget* widget,gpointer data) {
    GtkWidget* label;

    if(widget==buttonClock10Atlas) {
        clock10Atlas=TRUE;
        clock10Penelope=FALSE;
        clock10Mercury=FALSE;
        label=gtk_bin_get_child((GtkBin*)buttonClock10Atlas);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);
        label=gtk_bin_get_child((GtkBin*)buttonClock10Penelope);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &white);
        label=gtk_bin_get_child((GtkBin*)buttonClock10Mercury);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
        set10MHzSource(0);
    } else if(widget==buttonClock10Penelope) {
        clock10Atlas=FALSE;
        clock10Penelope=TRUE;
        clock10Mercury=FALSE;
        label=gtk_bin_get_child((GtkBin*)buttonClock10Atlas);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &white);
        label=gtk_bin_get_child((GtkBin*)buttonClock10Penelope);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);
        label=gtk_bin_get_child((GtkBin*)buttonClock10Mercury);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
        set10MHzSource(1);
    } else if(widget==buttonClock10Mercury) {
        clock10Atlas=FALSE;
        clock10Penelope=FALSE;
        clock10Mercury=TRUE;
        label=gtk_bin_get_child((GtkBin*)buttonClock10Atlas);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &white);
        label=gtk_bin_get_child((GtkBin*)buttonClock10Penelope);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
        label=gtk_bin_get_child((GtkBin*)buttonClock10Mercury);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);
        set10MHzSource(2);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Build Hardware User Interface
* 
* @return 
*/
GtkWidget* buildClock10UI() {

    GtkWidget* label;

    clock10Frame=gtk_frame_new("10 MHz Clock Source");
    gtk_widget_modify_bg(clock10Frame,GTK_STATE_NORMAL,&background);
    gtk_widget_modify_fg(gtk_frame_get_label_widget(GTK_FRAME(clock10Frame)),GTK_STATE_NORMAL,&white);

    clock10Table=gtk_table_new(1,3,TRUE);

    // clock10 settings
    buttonClock10Atlas = gtk_button_new_with_label ("Atlas");
    gtk_widget_modify_bg(buttonClock10Atlas, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonClock10Atlas);
    gtk_widget_set_size_request(GTK_WIDGET(buttonClock10Atlas),75,25);
    g_signal_connect(G_OBJECT(buttonClock10Atlas),"clicked",G_CALLBACK(clock10ButtonCallback),NULL);
    gtk_widget_show(buttonClock10Atlas);
    gtk_table_attach_defaults(GTK_TABLE(clock10Table),buttonClock10Atlas,0,1,0,1);

    buttonClock10Penelope = gtk_button_new_with_label ("Penelope");
    gtk_widget_modify_bg(buttonClock10Penelope, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonClock10Penelope);
    gtk_widget_set_size_request(GTK_WIDGET(buttonClock10Penelope),75,25);
    g_signal_connect(G_OBJECT(buttonClock10Penelope),"clicked",G_CALLBACK(clock10ButtonCallback),NULL);
    gtk_widget_show(buttonClock10Penelope);
    gtk_table_attach_defaults(GTK_TABLE(clock10Table),buttonClock10Penelope,1,2,0,1);

    buttonClock10Mercury = gtk_button_new_with_label ("Mercury");
    gtk_widget_modify_bg(buttonClock10Mercury, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonClock10Mercury);
    gtk_widget_set_size_request(GTK_WIDGET(buttonClock10Mercury),75,25);
    g_signal_connect(G_OBJECT(buttonClock10Mercury),"clicked",G_CALLBACK(clock10ButtonCallback),NULL);
    gtk_widget_show(buttonClock10Mercury);
    gtk_table_attach_defaults(GTK_TABLE(clock10Table),buttonClock10Mercury,2,3,0,1);

    gtk_container_add(GTK_CONTAINER(clock10Frame),clock10Table);
    gtk_widget_show(clock10Table);
    gtk_widget_show(clock10Frame);

    return clock10Frame;

}

void clock10Init() {
    if(clock10Atlas) {
        clock10ButtonCallback(buttonClock10Atlas,NULL);
    } else if(clock10Penelope) {
        clock10ButtonCallback(buttonClock10Penelope,NULL);
    } else if(clock10Mercury) {
        clock10ButtonCallback(buttonClock10Mercury,NULL);
    } 
        
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Save the Hardware state
*/
void clock10SaveState() {
    char string[128];
    if(clock10Atlas) {
        strcpy(string,"Atlas");
    } else if(clock10Penelope) {
        strcpy(string,"Penelope");
    } else {
        strcpy(string,"Mercury");
    }
    setProperty("10MHzClock",string);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Restore the Hardware state
*/
void clock10RestoreState() {
    char* value;
            
    value=getProperty("10MHzClock");
    if(value) {
        if(strcmp(value,"Atlas")==0) {
            clock10Atlas=TRUE;
        } else if(strcmp(value,"Penelope")==0) {
            clock10Penelope=TRUE;
        } else {
            clock10Mercury=TRUE;
        }
    } else {
        clock10Mercury=TRUE;
    }
}

