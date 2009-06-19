/** 
* @file xvtr_setup.c
* @brief Setup functions
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-12
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


//
// xvtr_setup.c
//

#include <stdlib.h>
#include <string.h>
#include <gtk/gtk.h>
#include "xvtr.h"
#include "band.h"

GtkWidget* xvtrPage;

GtkWidget* buttonLabel[12];
GtkWidget* minFrequency[12];
GtkWidget* maxFrequency[12];
GtkWidget* loFrequency[12];

/* --------------------------------------------------------------------------*/
/** 
* @brief display setup UI
*/
GtkWidget* xvtrSetupUI() {
    GtkWidget* item;
    int i;
    char temp[32];
    XVTR_ENTRY* xvtr_entry;

    xvtrPage=gtk_table_new(14,5,FALSE);

    gtk_table_set_col_spacings(GTK_TABLE(xvtrPage),10);

    item=gtk_label_new("Button");
    gtk_widget_show(item);
    gtk_table_attach_defaults(GTK_TABLE(xvtrPage),item,0,1,0,1);
    item=gtk_label_new("Button Label");
    gtk_widget_show(item);
    gtk_table_attach_defaults(GTK_TABLE(xvtrPage),item,1,2,0,1);
    item=gtk_label_new("Min Frequency");
    gtk_widget_show(item);
    gtk_table_attach_defaults(GTK_TABLE(xvtrPage),item,2,3,0,1);
    item=gtk_label_new("Max Frequency");
    gtk_widget_show(item);
    gtk_table_attach_defaults(GTK_TABLE(xvtrPage),item,3,4,0,1);
    item=gtk_label_new("LO Frequency");
    gtk_widget_show(item);
    gtk_table_attach_defaults(GTK_TABLE(xvtrPage),item,4,5,0,1);

    for(i=0;i<12;i++) {
        sprintf(temp,"%d",i);
        item=gtk_label_new(temp);
        gtk_widget_show(item);
        gtk_table_attach_defaults(GTK_TABLE(xvtrPage),item,0,1,i+1,i+2);

        xvtr_entry=getXvtrEntry(i);

        item=gtk_entry_new();
        gtk_editable_set_editable(GTK_EDITABLE(item),TRUE);
        gtk_entry_set_text(GTK_ENTRY(item),xvtr_entry->name);
        gtk_widget_show(item);
        gtk_table_attach_defaults(GTK_TABLE(xvtrPage),item,1,2,i+1,i+2);
        buttonLabel[i]=item;
        
        sprintf(temp,"%lld",xvtr_entry->frequencyMin);
        item=gtk_entry_new();
        gtk_editable_set_editable(GTK_EDITABLE(item),TRUE);
        gtk_entry_set_text(GTK_ENTRY(item),temp);
        gtk_widget_show(item);
        gtk_table_attach_defaults(GTK_TABLE(xvtrPage),item,2,3,i+1,i+2);
        minFrequency[i]=item;
        
        sprintf(temp,"%lld",xvtr_entry->frequencyMax);
        item=gtk_entry_new();
        gtk_editable_set_editable(GTK_EDITABLE(item),TRUE);
        gtk_entry_set_text(GTK_ENTRY(item),temp);
        gtk_widget_show(item);
        gtk_table_attach_defaults(GTK_TABLE(xvtrPage),item,3,4,i+1,i+2);
        maxFrequency[i]=item;
        
        sprintf(temp,"%lld",xvtr_entry->frequencyLO);
        item=gtk_entry_new();
        gtk_editable_set_editable(GTK_EDITABLE(item),TRUE);
        gtk_entry_set_text(GTK_ENTRY(item),temp);
        gtk_widget_show(item);
        gtk_table_attach_defaults(GTK_TABLE(xvtrPage),item,4,5,i+1,i+2);
        loFrequency[i]=item;
    }

    gtk_widget_show(xvtrPage);
    return xvtrPage;

}

void saveXvtrSettings() {
    int i;
    const gchar* temp;
    XVTR_ENTRY* xvtr_entry;

    for(i=0;i<12;i++) {
        xvtr_entry=getXvtrEntry(i);
        temp=gtk_entry_get_text(GTK_ENTRY(buttonLabel[i]));
        strcpy(xvtr_entry->name,temp);

        temp=gtk_entry_get_text(GTK_ENTRY(minFrequency[i]));
        if(strcmp(temp,"")==0) {
            xvtr_entry->frequencyMin=0LL;
        } else {
            xvtr_entry->frequencyMin=atoll(temp);
        }

        temp=gtk_entry_get_text(GTK_ENTRY(maxFrequency[i]));
        if(strcmp(temp,"")==0) {
            xvtr_entry->frequencyMax=0LL;
        } else {
            xvtr_entry->frequencyMax=atoll(temp);
        }

        temp=gtk_entry_get_text(GTK_ENTRY(loFrequency[i]));
        if(strcmp(temp,"")==0) {
            xvtr_entry->frequencyLO=0LL;
        } else {
            xvtr_entry->frequencyLO=atoll(temp);
        }

        if((xvtr_entry->frequency<xvtr_entry->frequencyMin) ||
           (xvtr_entry->frequency>xvtr_entry->frequencyMax)) {
            xvtr_entry->frequency=xvtr_entry->frequencyMin;
        }
    }
}

