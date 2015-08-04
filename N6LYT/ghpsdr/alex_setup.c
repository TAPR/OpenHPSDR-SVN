/** 
* @file alex_setup.c
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
// alex_setup.c
//

#include <stdlib.h>
#include <string.h>
#include <gtk/gtk.h>
#include "filter.h"
#include "alex_setup.h"
#include "xvtr.h"
#include "band.h"
#include "ozy.h"
#include "soundcard.h"
#include "volume.h"
#include "transmit.h"

GtkWidget* alexPage;

GtkWidget* alexEnable;

GtkWidget* rxNone;
GtkWidget* rxExt1;
GtkWidget* rxExt2;
GtkWidget* rxRXXV;
GtkWidget* txAnt1;
GtkWidget* txAnt2;
GtkWidget* txAnt3;

GtkWidget* alexAttenuation0dB;
GtkWidget* alexAttenuation10dB;
GtkWidget* alexAttenuation20dB;
GtkWidget* alexAttenuation30dB;

void rxNoneCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexRxAntenna(0);
        bandSetAlexRxAntenna(0);
    }
}
void rxExt1Callback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexRxAntenna(0);
        bandSetAlexRxAntenna(0);
    }
}

void rxExt2Callback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexRxAntenna(1);
        bandSetAlexRxAntenna(1);
    }
}

void rxRXXVCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexRxAntenna(2);
        bandSetAlexRxAntenna(2);
    }
}

void txAnt1Callback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexTxAntenna(0);
        bandSetAlexTxAntenna(0);
    }
}

void txAnt2Callback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexTxAntenna(1);
        bandSetAlexTxAntenna(1);
    }
}

void txAnt3Callback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexTxAntenna(2);
        bandSetAlexTxAntenna(2);
    }
}

void alexAttenuation0dBButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexAttenuation(0);
    }
}

void alexAttenuation10dBButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexAttenuation(1);
    }
}

void alexAttenuation20dBButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexAttenuation(2);
    }
}

void alexAttenuation30dBButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexAttenuation(3);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief alex setup UI
*/
GtkWidget* alexSetupUI() {
    GtkWidget* box;
    GtkWidget* label;
    GtkWidget* item;
    char text[80];

    alexPage=gtk_vbox_new(FALSE,8);

    box=gtk_hbox_new(FALSE,2);
    label=gtk_label_new("Alex setup for current Band(stack) or XVTR");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    label=gtk_label_new("");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(alexPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,2);
    label=gtk_label_new("Receive");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(alexPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,2);
    rxNone=gtk_radio_button_new_with_label(NULL,"None (use Tx)");
    gtk_widget_show(rxNone);
    gtk_box_pack_start(GTK_BOX(box),rxNone,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(rxNone),"clicked",G_CALLBACK(rxNoneCallback),NULL);

    rxExt1=gtk_radio_button_new_with_label(gtk_radio_button_group(GTK_RADIO_BUTTON(rxNone)),"Ext 1");
    gtk_widget_show(rxExt1);
    gtk_box_pack_start(GTK_BOX(box),rxExt1,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(rxExt1),"clicked",G_CALLBACK(rxExt1Callback),NULL);

    rxExt2=gtk_radio_button_new_with_label(gtk_radio_button_group(GTK_RADIO_BUTTON(rxExt1)),"Ext 2");
    gtk_widget_show(rxExt2);
    gtk_box_pack_start(GTK_BOX(box),rxExt2,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(rxExt2),"clicked",G_CALLBACK(rxExt2Callback),NULL);
    rxRXXV=gtk_radio_button_new_with_label(gtk_radio_button_group(GTK_RADIO_BUTTON(rxExt2)),"Rx XV");
    gtk_widget_show(rxRXXV);
    gtk_box_pack_start(GTK_BOX(box),rxRXXV,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(rxRXXV),"clicked",G_CALLBACK(rxRXXVCallback),NULL);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(alexPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,2);
    label=gtk_label_new("Transmit");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(alexPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,2);
    txAnt1=gtk_radio_button_new_with_label(NULL,"Ant 1");
    gtk_widget_show(txAnt1);
    gtk_box_pack_start(GTK_BOX(box),txAnt1,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(txAnt1),"clicked",G_CALLBACK(txAnt1Callback),NULL);
    txAnt2=gtk_radio_button_new_with_label(gtk_radio_button_group(GTK_RADIO_BUTTON(txAnt1)),"Ant 2");
    gtk_widget_show(txAnt2);
    gtk_box_pack_start(GTK_BOX(box),txAnt2,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(txAnt2),"clicked",G_CALLBACK(txAnt2Callback),NULL);
    txAnt3=gtk_radio_button_new_with_label(gtk_radio_button_group(GTK_RADIO_BUTTON(txAnt2)),"Ant 3");
    gtk_widget_show(txAnt3);
    gtk_box_pack_start(GTK_BOX(box),txAnt3,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(txAnt3),"clicked",G_CALLBACK(txAnt3Callback),NULL);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(alexPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,5);
    label=gtk_label_new("Attenuation");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(alexPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,2);
    alexAttenuation0dB=gtk_radio_button_new_with_label(NULL,"0dB");
    gtk_widget_show(alexAttenuation0dB);
    gtk_box_pack_start(GTK_BOX(box),alexAttenuation0dB,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(alexAttenuation0dB),"clicked",G_CALLBACK(alexAttenuation0dBButtonCallback),NULL);
    alexAttenuation10dB=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(alexAttenuation0dB),"10dB");
    gtk_widget_show(alexAttenuation10dB);
    gtk_box_pack_start(GTK_BOX(box),alexAttenuation10dB,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(alexAttenuation10dB),"clicked",G_CALLBACK(alexAttenuation10dBButtonCallback),NULL);
    alexAttenuation20dB=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(alexAttenuation0dB),"20dB");
    gtk_widget_show(alexAttenuation20dB);
    gtk_box_pack_start(GTK_BOX(box),alexAttenuation20dB,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(alexAttenuation20dB),"clicked",G_CALLBACK(alexAttenuation20dBButtonCallback),NULL);
    alexAttenuation30dB=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(alexAttenuation0dB),"30dB");
    gtk_widget_show(alexAttenuation30dB);
    gtk_box_pack_start(GTK_BOX(box),alexAttenuation30dB,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(alexAttenuation30dB),"clicked",G_CALLBACK(alexAttenuation30dBButtonCallback),NULL);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(alexPage),box,FALSE,FALSE,2);

    updateAlexSetup();

    gtk_widget_show(alexPage);
    return alexPage;
}

void updateAlexSetup() {
    switch(alexRxAntenna) {
        case NONE:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(rxNone),TRUE);
            break;
        case RX1:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(rxExt1),TRUE);
            break;
        case RX2:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(rxExt2),TRUE);
            break;
        case RXXV:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(rxRXXV),TRUE);
            break;
    }

    switch(alexTxAntenna) {
        case ANT1:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(txAnt1),TRUE);
            break;
        case ANT2:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(txAnt2),TRUE);
            break;
        case ANT3:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(txAnt3),TRUE);
            break;
    }

    switch(alexAttenuation) {
        case 0:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(alexAttenuation0dB),TRUE);
            break;
        case 1:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(alexAttenuation10dB),TRUE);
            break;
        case 2:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(alexAttenuation20dB),TRUE);
            break;
        case 3:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(alexAttenuation30dB),TRUE);
            break;
    }

}
