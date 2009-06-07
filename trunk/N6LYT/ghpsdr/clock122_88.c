/** 
* @file clock122_88.c
* @brief 122.88 MHz clock source access functions
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
#include "bandstack.h"
#include "command.h"
#include "dttsp.h"
#include "filter.h"
#include "main.h"
#include "clock122_88.h"
#include "mode.h"
#include "property.h"
#include "ozy.h"
#include "soundcard.h"
#include "volume.h"

GtkWidget* clock122_88Frame;
GtkWidget* clock122_88Table;

GtkWidget* buttonClock122_88Penelope;
GtkWidget* buttonClock122_88Mercury;

gboolean clock122_88Penelope=FALSE;
gboolean clock122_88Mercury=FALSE;

/* --------------------------------------------------------------------------*/
/** 
* @brief Clock 122.88 MHz button callback 
* 
* @param widget
* @param data
*/
void clock122_88ButtonCallback(GtkWidget* widget,gpointer data) {
    GtkWidget* label;

    if(widget==buttonClock122_88Penelope) {
        clock122_88Penelope=TRUE;
        clock122_88Mercury=FALSE;
        label=gtk_bin_get_child((GtkBin*)buttonClock122_88Penelope);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);
        label=gtk_bin_get_child((GtkBin*)buttonClock122_88Mercury);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
        set122MHzSource(0);
    } else if(widget==buttonClock122_88Mercury) {
        clock122_88Penelope=FALSE;
        clock122_88Mercury=TRUE;
        label=gtk_bin_get_child((GtkBin*)buttonClock122_88Mercury);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);
        label=gtk_bin_get_child((GtkBin*)buttonClock122_88Penelope);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
        set122MHzSource(1);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Build Hardware User Interface
* 
* @return 
*/
GtkWidget* buildClock122_88UI() {

    GtkWidget* label;

    clock122_88Frame=gtk_frame_new("122.88 MHz Clock Source");
    gtk_widget_modify_bg(clock122_88Frame,GTK_STATE_NORMAL,&background);
    gtk_widget_modify_fg(gtk_frame_get_label_widget(clock122_88Frame),GTK_STATE_NORMAL,&white);

    clock122_88Table=gtk_table_new(1,2,TRUE);

    // clock122_88 settings
    buttonClock122_88Penelope = gtk_button_new_with_label ("Penelope");
    gtk_widget_modify_bg(buttonClock122_88Penelope, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonClock122_88Penelope);
    gtk_widget_set_size_request(GTK_WIDGET(buttonClock122_88Penelope),75,25);
    g_signal_connect(G_OBJECT(buttonClock122_88Penelope),"clicked",G_CALLBACK(clock122_88ButtonCallback),NULL);
    gtk_widget_show(buttonClock122_88Penelope);
    gtk_table_attach_defaults(clock122_88Table,buttonClock122_88Penelope,0,1,0,1);

    buttonClock122_88Mercury = gtk_button_new_with_label ("Mercury");
    gtk_widget_modify_bg(buttonClock122_88Mercury, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonClock122_88Mercury);
    gtk_widget_set_size_request(GTK_WIDGET(buttonClock122_88Mercury),75,25);
    g_signal_connect(G_OBJECT(buttonClock122_88Mercury),"clicked",G_CALLBACK(clock122_88ButtonCallback),NULL);
    gtk_widget_show(buttonClock122_88Mercury);
    gtk_table_attach_defaults(clock122_88Table,buttonClock122_88Mercury,1,2,0,1);

    gtk_container_add(GTK_CONTAINER(clock122_88Frame),clock122_88Table);
    gtk_widget_show(clock122_88Table);
    gtk_widget_show(clock122_88Frame);

    return clock122_88Frame;

}

void clock122_88Init() {
    if(clock122_88Penelope) {
        clock122_88ButtonCallback(buttonClock122_88Penelope,NULL);
    } else if(clock122_88Mercury) {
        clock122_88ButtonCallback(buttonClock122_88Mercury,NULL);
    } 
        
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Save the Hardware state
*/
void clock122_88SaveState() {
    char string[128];
    if(clock122_88Penelope) {
        strcpy(string,"Penelope");
    } else {
        strcpy(string,"Mercury");
    }
    setProperty("122.88MHzClock",string);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Restore the Hardware state
*/
void clock122_88RestoreState() {
    char* value;
            
    value=getProperty("122.88MHzClock");
    if(value) {
        if(strcmp(value,"Penelope")==0) {
            clock122_88Penelope=TRUE;
        } else {
            clock122_88Mercury=TRUE;
        }
    } else {
        clock122_88Mercury=TRUE;
    }
}

