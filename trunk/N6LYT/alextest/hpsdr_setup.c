/** 
* @file hpsdr_setup.c
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
// hpsdr_setup.c
//

#include <stdlib.h>
#include <string.h>
#include <gtk/gtk.h>
#include "filter.h"
#include "hpsdr_setup.h"
#include "ozy.h"
#include "soundcard.h"
#include "volume.h"
#include "transmit.h"

GtkWidget* hpsdrPage;

GtkWidget* speed48K;
GtkWidget* speed96K;
GtkWidget* speed192K;

GtkWidget* LT2208Dither;
GtkWidget* LT2208Random;

GtkWidget* penelope122_88MHz;
GtkWidget* mercury122_88MHz;

GtkWidget* atlas10MHz;
GtkWidget* penelope10MHz;
GtkWidget* mercury10MHz;

GtkWidget* modeOther;
GtkWidget* modeClassE;

GtkWidget* janusMicSource;
GtkWidget* penelopeMicSource;

GtkWidget* alexAttenuation0Db;
GtkWidget* alexAttenuation10Db;
GtkWidget* alexAttenuation20Db;
GtkWidget* alexAttenuation30Db;

GtkWidget* hpsdrHalfDuplex;
GtkWidget* hpsdrFullDuplex;

GtkWidget* pennyLaneWidget;

void speed48ButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setSpeed(0);
    }
}

void speed96ButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setSpeed(1);
    }
}

void speed192ButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setSpeed(2);
    }
}

void ditherButtonCallback(GtkWidget* widget,gpointer data) {
    setLT2208Dither(GTK_TOGGLE_BUTTON(widget)->active);
}

void randomButtonCallback(GtkWidget* widget,gpointer data) {
    setLT2208Random(GTK_TOGGLE_BUTTON(widget)->active);
}

void penelope122_88MHzButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        set122MHzSource(0);
    }
}

void mercury122_88MHzButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        set122MHzSource(1);
    }
}


void atlas10MHzButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        set10MHzSource(0);
    }
}

void penelope10MHzButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        set10MHzSource(1);
    }
}

void mercury10MHzButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        set10MHzSource(2);
    }
}

void modeOtherButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setClass(0);
    }
}

void modeClassEButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setClass(1);
    }
}

void janusMicSourceButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setMicSource(0);
    }
}

void penelopeMicSourceButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setMicSource(1);
    }
}

void alexAttenuation0DbButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexAttenuation(0);
    }
}

void alexAttenuation10DbButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexAttenuation(1);
    }
}

void alexAttenuation20DbButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexAttenuation(2);
    }
}

void alexAttenuation30DbButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setAlexAttenuation(3);
    }
}

void hpsdrHalfDuplexButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        fullDuplex=0;
    }
}

void hpsdrFullDuplexButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        fullDuplex=1;
    }
}

void pennyLaneButtonCallback(GtkWidget* widget,gpointer data) {
    if(GTK_TOGGLE_BUTTON(widget)->active) {
        setPennyLane(1);
    } else {
        setPennyLane(0);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief hpsdr setup UI
*/
GtkWidget* hpsdrSetupUI() {
    GtkWidget* box;
    GtkWidget* label;
    GtkWidget* item;
    char text[80];

    hpsdrPage=gtk_vbox_new(FALSE,8);

    box=gtk_vbox_new(TRUE,4);
    sprintf(text,"Ozy FX2 version: %s",get_ozy_firmware_version());
    label=gtk_label_new(text);
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    sprintf(text,"Ozy software version: %d",get_ozy_software_version());
    label=gtk_label_new(text);
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    sprintf(text,"Mercury software version: %d",get_mercury_software_version());
    label=gtk_label_new(text);
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    sprintf(text,"Penelope software version: %d",get_penelope_software_version());
    label=gtk_label_new(text);
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(hpsdrPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,5);
    label=gtk_label_new("Speed:					");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    speed48K=gtk_radio_button_new_with_label(NULL,"48K		");
    gtk_widget_show(speed48K);
    gtk_box_pack_start(GTK_BOX(box),speed48K,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(speed48K),"clicked",G_CALLBACK(speed48ButtonCallback),NULL);
    speed96K=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(speed48K),"96K		");
    gtk_widget_show(speed96K);
    gtk_box_pack_start(GTK_BOX(box),speed96K,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(speed96K),"clicked",G_CALLBACK(speed96ButtonCallback),NULL);
    speed192K=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(speed48K),"192K	");
    gtk_widget_show(speed192K);
    gtk_box_pack_start(GTK_BOX(box),speed192K,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(speed192K),"clicked",G_CALLBACK(speed192ButtonCallback),NULL);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(hpsdrPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,5);
    label=gtk_label_new("LT2208:					");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    LT2208Dither=gtk_check_button_new_with_label("Dither	");
    gtk_widget_show(LT2208Dither);
    gtk_box_pack_start(GTK_BOX(box),LT2208Dither,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(LT2208Dither),"clicked",G_CALLBACK(ditherButtonCallback),NULL);
    LT2208Random=gtk_check_button_new_with_label("Random	");
    gtk_widget_show(LT2208Random);
    gtk_box_pack_start(GTK_BOX(box),LT2208Random,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(LT2208Random),"clicked",G_CALLBACK(randomButtonCallback),NULL);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(hpsdrPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,5);
    label=gtk_label_new("122.88MHz Clock Source:	");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    penelope122_88MHz=gtk_radio_button_new_with_label(NULL,"Penelope	");
    gtk_widget_show(penelope122_88MHz);
    gtk_box_pack_start(GTK_BOX(box),penelope122_88MHz,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(LT2208Random),"clicked",G_CALLBACK(penelope122_88MHzButtonCallback),NULL);
    mercury122_88MHz=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(penelope122_88MHz),"Mercury	");
    gtk_widget_show(mercury122_88MHz);
    gtk_box_pack_start(GTK_BOX(box),mercury122_88MHz,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(LT2208Random),"clicked",G_CALLBACK(mercury122_88MHzButtonCallback),NULL);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(hpsdrPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,5);
    label=gtk_label_new("10MHz Clock Source:		");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    atlas10MHz=gtk_radio_button_new_with_label(NULL,"Atlas	");
    gtk_widget_show(atlas10MHz);
    gtk_box_pack_start(GTK_BOX(box),atlas10MHz,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(atlas10MHz),"clicked",G_CALLBACK(atlas10MHzButtonCallback),NULL);
    penelope10MHz=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(atlas10MHz),"Penelope	");
    gtk_widget_show(penelope10MHz);
    gtk_box_pack_start(GTK_BOX(box),penelope10MHz,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(penelope10MHz),"clicked",G_CALLBACK(penelope10MHzButtonCallback),NULL);
    mercury10MHz=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(atlas10MHz),"Mercury	");
    gtk_widget_show(mercury10MHz);
    gtk_box_pack_start(GTK_BOX(box),mercury10MHz,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(mercury10MHz),"clicked",G_CALLBACK(mercury10MHzButtonCallback),NULL);
    gtk_widget_show(penelope122_88MHz);
    gtk_widget_show(box);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(hpsdrPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,5);
    label=gtk_label_new("Mode:					");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    modeOther=gtk_radio_button_new_with_label(NULL,"Other	");
    gtk_widget_show(modeOther);
    gtk_box_pack_start(GTK_BOX(box),modeOther,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(modeOther),"clicked",G_CALLBACK(modeOtherButtonCallback),NULL);
    modeClassE=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(modeOther),"Class E	");
    gtk_widget_show(modeClassE);
    gtk_box_pack_start(GTK_BOX(box),modeClassE,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(modeClassE),"clicked",G_CALLBACK(modeClassEButtonCallback),NULL);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(hpsdrPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,5);
    label=gtk_label_new("Mic Source:				");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    janusMicSource=gtk_radio_button_new_with_label(NULL,"Janus	");
    gtk_widget_show(janusMicSource);
    gtk_box_pack_start(GTK_BOX(box),janusMicSource,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(janusMicSource),"clicked",G_CALLBACK(janusMicSourceButtonCallback),NULL);
    penelopeMicSource=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(janusMicSource),"Penelope	");
    gtk_widget_show(penelopeMicSource);
    gtk_box_pack_start(GTK_BOX(box),penelopeMicSource,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(penelopeMicSource),"clicked",G_CALLBACK(penelopeMicSourceButtonCallback),NULL);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(hpsdrPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,5);
    label=gtk_label_new("Alex Attenuation:			");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    alexAttenuation0Db=gtk_radio_button_new_with_label(NULL,"0Db		");
    gtk_widget_show(alexAttenuation0Db);
    gtk_box_pack_start(GTK_BOX(box),alexAttenuation0Db,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(alexAttenuation0Db),"clicked",G_CALLBACK(alexAttenuation0DbButtonCallback),NULL);
    alexAttenuation10Db=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(alexAttenuation0Db),"10Db	");
    gtk_widget_show(alexAttenuation10Db);
    gtk_box_pack_start(GTK_BOX(box),alexAttenuation10Db,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(alexAttenuation10Db),"clicked",G_CALLBACK(alexAttenuation10DbButtonCallback),NULL);
    alexAttenuation20Db=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(alexAttenuation0Db),"20Db	");
    gtk_widget_show(alexAttenuation20Db);
    gtk_box_pack_start(GTK_BOX(box),alexAttenuation20Db,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(alexAttenuation20Db),"clicked",G_CALLBACK(alexAttenuation20DbButtonCallback),NULL);
    alexAttenuation30Db=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(alexAttenuation0Db),"30Db	");
    gtk_widget_show(alexAttenuation30Db);
    gtk_box_pack_start(GTK_BOX(box),alexAttenuation30Db,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(alexAttenuation30Db),"clicked",G_CALLBACK(alexAttenuation30DbButtonCallback),NULL);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(hpsdrPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,3);
    label=gtk_label_new("HPSDR Duplex:			");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    hpsdrHalfDuplex=gtk_radio_button_new_with_label(NULL,"Half		");
    gtk_widget_show(hpsdrHalfDuplex);
    gtk_box_pack_start(GTK_BOX(box),hpsdrHalfDuplex,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(hpsdrHalfDuplex),"clicked",G_CALLBACK(hpsdrHalfDuplexButtonCallback),NULL);
    hpsdrFullDuplex=gtk_radio_button_new_with_label_from_widget(GTK_RADIO_BUTTON(hpsdrHalfDuplex),"Full	");
    gtk_widget_show(hpsdrFullDuplex);
    gtk_box_pack_start(GTK_BOX(box),hpsdrFullDuplex,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(hpsdrFullDuplex),"clicked",G_CALLBACK(hpsdrFullDuplexButtonCallback),NULL);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(hpsdrPage),box,FALSE,FALSE,2);

    box=gtk_hbox_new(FALSE,3);
    label=gtk_label_new("PennyLane/Hermes:		");
    gtk_widget_show(label);
    gtk_box_pack_start(GTK_BOX(box),label,FALSE,FALSE,2);
    pennyLaneWidget=gtk_check_button_new_with_label("PennyLane/Hermes");
    gtk_widget_show(pennyLaneWidget);
    gtk_box_pack_start(GTK_BOX(box),pennyLaneWidget,FALSE,FALSE,2);
    g_signal_connect(G_OBJECT(pennyLaneWidget),"clicked",G_CALLBACK(pennyLaneButtonCallback),NULL);
    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(hpsdrPage),box,FALSE,FALSE,2);

    switch(speed) {
        case 0:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(speed48K),TRUE);
            break;
        case 1:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(speed96K),TRUE);
            break;
        case 2:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(speed192K),TRUE);
            break;
    }

    
    gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(LT2208Dither),lt2208Dither);
    gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(LT2208Random),lt2208Random);

    switch(clock122_88MHz) {
        case 0:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(penelope122_88MHz),TRUE);
            break;
        case 1:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(mercury122_88MHz),TRUE);
            break;
    }

    switch(clock10MHz) {
        case 0:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(atlas10MHz),TRUE);
            break;
        case 1:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(penelope10MHz),TRUE);
            break;
        case 2:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(mercury10MHz),TRUE);
            break;
    }

    switch(class) {
        case 0:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(modeOther),TRUE);
            break;
        case 1:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(modeClassE),TRUE);
            break;
    }

    switch(micSource) {
        case 0:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(janusMicSource),TRUE);
            break;
        case 1:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(penelopeMicSource),TRUE);
            break;
    }

    switch(fullDuplex) {
        case 0:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(hpsdrHalfDuplex),TRUE);
            break;
        case 1:
            gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(hpsdrFullDuplex),TRUE);
            break;
    }

    gtk_toggle_button_set_active(GTK_TOGGLE_BUTTON(pennyLaneWidget),pennyLane);

    gtk_widget_show(hpsdrPage);
    return hpsdrPage;
}
