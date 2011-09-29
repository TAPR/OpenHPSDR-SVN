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

GtkWidget* rxAnt1;
GtkWidget* rxAnt2;
GtkWidget* rxAnt3;
GtkWidget* rxNone;
GtkWidget* rxR1;
GtkWidget* rxR2;
GtkWidget* rxRXV;
GtkWidget* txAnt1;
GtkWidget* txAnt2;
GtkWidget* txAnt3;

GtkWidget* alexAttenuation0dB;
GtkWidget* alexAttenuation10dB;
GtkWidget* alexAttenuation20dB;
GtkWidget* alexAttenuation30dB;

void rxAnt1Callback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexRxAntenna(0);
        bandSetAlexRxAntenna(0);
        setAlexRxOnlyAntenna(0);
        bandSetAlexRxOnlyAntenna(0);
    }
}

void rxAnt2Callback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexRxAntenna(1);
        bandSetAlexRxAntenna(1);
        setAlexRxOnlyAntenna(0);
        bandSetAlexRxOnlyAntenna(0);
    }
}

void rxAnt3Callback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexRxAntenna(2);
        bandSetAlexRxAntenna(2);
        setAlexRxOnlyAntenna(0);
        bandSetAlexRxOnlyAntenna(0);
    }
}

//void rxNoneCallback(GtkWidget* widget,gpointer data) {
//    if(GTK_TOGGLE_BUTTON(widget)->active) {
//        setAlexRxOnlyAntenna(0);
//        bandSetAlexRxOnlyAntenna(0);
//    }
//}

void rxR1Callback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexRxOnlyAntenna(1);
        bandSetAlexRxOnlyAntenna(1);
    }
}

void rxR2Callback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexRxOnlyAntenna(2);
        bandSetAlexRxOnlyAntenna(2);
    }
}

void rxRXVCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexRxOnlyAntenna(3);
        bandSetAlexRxOnlyAntenna(3);
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
    rxAnt1=gtk_radio_button_new_with_label(NULL,"Ant 1");
    gtk_widget_show(rxAnt1);
    gtk_box_pack_start(GTK_BOX(box),rxAnt1,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(rxAnt1),"clicked",G_CALLBACK(rxAnt1Callback),NULL);
    rxAnt2=gtk_radio_button_new_with_label(gtk_radio_button_group(GTK_RADIO_BUTTON(rxAnt1)),"Ant 2");
    gtk_widget_show(rxAnt2);
    gtk_box_pack_start(GTK_BOX(box),rxAnt2,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(rxAnt2),"clicked",G_CALLBACK(rxAnt2Callback),NULL);
    rxAnt3=gtk_radio_button_new_with_label(gtk_radio_button_group(GTK_RADIO_BUTTON(rxAnt2)),"Ant 3");
    gtk_widget_show(rxAnt3);
    gtk_box_pack_start(GTK_BOX(box),rxAnt3,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(rxAnt3),"clicked",G_CALLBACK(rxAnt3Callback),NULL);
//    gtk_widget_show(box);
//    gtk_box_pack_start(GTK_BOX(alexPage),box,FALSE,FALSE,2);

//    box=gtk_hbox_new(FALSE,2);
//    label=gtk_label_new("Rx Only");
//    gtk_widget_show(label);
//    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
//    gtk_widget_show(box);
//    gtk_box_pack_start(GTK_BOX(alexPage),box,FALSE,FALSE,2);

//    box=gtk_hbox_new(FALSE,2);
//    rxNone=gtk_radio_button_new_with_label(NULL,"None");
//    gtk_widget_show(rxNone);
//    gtk_box_pack_start(GTK_BOX(box),rxNone,FALSE,FALSE,2);
//    g_signal_connect(G_OBJECT(rxNone),"clicked",G_CALLBACK(rxNoneCallback),NULL);

    rxR1=gtk_radio_button_new_with_label(gtk_radio_button_group(GTK_RADIO_BUTTON(rxAnt3)),"Rx 1");
    gtk_widget_show(rxR1);
    gtk_box_pack_start(GTK_BOX(box),rxR1,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(rxR1),"clicked",G_CALLBACK(rxR1Callback),NULL);
    rxR2=gtk_radio_button_new_with_label(gtk_radio_button_group(GTK_RADIO_BUTTON(rxR1)),"Rx 2");
    gtk_widget_show(rxR2);
    gtk_box_pack_start(GTK_BOX(box),rxR2,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(rxR2),"clicked",G_CALLBACK(rxR2Callback),NULL);
    rxRXV=gtk_radio_button_new_with_label(gtk_radio_button_group(GTK_RADIO_BUTTON(rxR2)),"Rx XV");
    gtk_widget_show(rxRXV);
    gtk_box_pack_start(GTK_BOX(box),rxRXV,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(rxRXV),"clicked",G_CALLBACK(rxRXVCallback),NULL);
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
    label=gtk_label_new("Alex Attenuation");
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


    switch(alexRxAntenna) {
        case 0:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(rxAnt1),TRUE);
            break;
        case 1:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(rxAnt2),TRUE);
            break;
        case 2:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(rxAnt3),TRUE);
            break;
    }

    switch(alexRxOnlyAntenna) {
        case 0:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(rxNone),TRUE);
            break;
        case 1:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(rxR1),TRUE);
            break;
        case 2:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(rxR2),TRUE);
            break;
        case 3:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(rxRXV),TRUE);
            break;
    }


    switch(alexTxAntenna) {
        case 0:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(txAnt1),TRUE);
            break;
        case 1:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(txAnt2),TRUE);
            break;
        case 2:
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

    gtk_widget_show(alexPage);
    return alexPage;
}
