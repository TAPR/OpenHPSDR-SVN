/** 
* @file receiver.c
* @brief Mode functions
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-12
*/
// receiver.c

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
#include "property.h"

#include "command.h"
#include "dttsp.h"
#include "main.h"

GtkWidget* receiverFrame;

int receiver;

float pan[MAXRX];

GtkWidget* panScale;

/* --------------------------------------------------------------------------*/
/** 
* @brief  Select the receiver
* 
* @param widget
*/
void selectReceiver(GtkWidget* widget) {
    GtkWidget* label;
    char temp[80];
}

/* --------------------------------------------------------------------------*/
/** 
* @brief  Callback when pan values changes
* 
* @param widget
* @param data
*/
void panChanged(GtkWidget* widget,gpointer data) {
    char command[80];
    pan[0]=gtk_range_get_value((GtkRange*)panScale);
    sprintf(command,"setRXPan %f",pan[0]);
    fprintf(stderr,"%s\n",command);
    writeCommand(command);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief  Build the GUI
* 
* @return 
*/
GtkWidget* buildReceiverUI() {
    GtkWidget* label;

    receiverFrame=gtk_frame_new("Pan");
    //gtk_widget_modify_bg(receiverFrame,GTK_STATE_NORMAL,&background);
    label=gtk_frame_get_label_widget((GtkFrame*)receiverFrame);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_frame_set_shadow_type((GtkFrame*)receiverFrame,GTK_SHADOW_ETCHED_IN);

    panScale=gtk_hscale_new_with_range(0.0,1.0,0.1);
    g_signal_connect(G_OBJECT(panScale),"value-changed",G_CALLBACK(panChanged),NULL);
    gtk_range_set_value((GtkRange*)panScale,pan[0]);
    gtk_widget_set_size_request(GTK_WIDGET(panScale),150,25);
    gtk_widget_show(panScale);
    gtk_container_add((GtkFrame*)receiverFrame,panScale);

    gtk_widget_set_size_request(GTK_WIDGET(receiverFrame),200,55);
    gtk_widget_show(receiverFrame);

    return receiverFrame;
  
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Save the receiver state
*/
void receiverSaveState() {
    char string[128];
    sprintf(string,"%d",receiver);
    setProperty("receiver",string);
    sprintf(string,"%f",pan[0]);
    setProperty("pan.0",string);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Restore the receiver state
*/
void receiverRestoreState() {
    char* value;
    value=getProperty("receiver");
    if(value) receiver=atoi(value);
    value=getProperty("pan.0");
    if(value) pan[0]=atof(value); else pan[0]=0.5f;
}
