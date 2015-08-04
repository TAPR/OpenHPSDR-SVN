/** 

* \file main.c
* \brief Main file for the GHPSDR Software Defined Radio Graphic Interface. 
* \author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* \version 0.1
* \date 2009-04-11
*
*
* \mainpage GHPSDR 
*  \image html ../ghpsdr.png
*  \image latex ../ghpsdr.png "Screen shot of GHPSDR" width=10cm
*
* \section A Linux based, GTK2+, Radio Graphical User Interface to HPSDR boards.  
* \author John Melton, G0ORX/N6LYT
* \version 0.1
* \date 2009-04-11
* \updated 2015-05-01  (changed from DttSP to WDSP and added device selection)
* 
* \author Dave Larsen, KV0S, Doxygen comments
*
* These files are design to build a simple 
* high performance  interface under the Linux  operating system.  
*
* This is still very much an Alpha version. It does still have problems and not everything is 
* completed.
*
* To build the application there is a simple Makefile.
*
* To run the application just start ghpsdr once it is built.
*
* Currently it does not include any code to load the FPGA so you must run initozy before 
* running the application. You must also have the latest FPGA code.
*
* Functionally, each band has 3 bandstacks. The frequency/mode/filter settings will be 
* saved when exiting the application for all the bandstack entries.
*
* Tuning can be accomplished by left mouse clicking in the Panadapter/Waterfall window to 
* move the selected frequency to the center of the current filter. A right mouse click will 
* move the selected frequency to the cursor. You can also use the left mouse button to drag 
* the frequency by holding it down while dragging. If you have a scroll wheel, moving the 
* scroll wheel will increment/decrement the frequency by the current step amount.
*
* You can also left mouse click on the bandscope display and it will move to the selected frequency.
* 
* The Setup button pops up a window to adjust the display settings. There are no tests 
* currently if these are set to invalid values.
*
*
* There are some problems when running at other than 48000. Sometimes the audio output will 
* stop although the Panadapter/Waterfall and bandscope continue to function. It usually 
* requires intiozy to be run again to get the audio back.
*
*
* This code is available at 
* svn://svn.tar.org/svn/repos_sdr_hpsdr/trunk/N6LYT/ghpsdr
*
* More information on the HPSDR project is availble at 
* http://openhpsdr.info
*
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


// main.c
//
// GTK+ 2.0 implementation of Beppe's Main control panel
// see http://www.radioamatore.it/sdr1000/mypowersdr.html for the original

#include <arpa/inet.h>
#include <gtk/gtk.h>
#include <gdk/gdkkeysyms.h>
#include <fcntl.h>
#include <sys/types.h>
#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <getopt.h>

#include "screensize.h"
#include "audio.h"
#include "agc.h"
#include "bandstack.h"
#include "bandscope_control.h"
#include "bandscope_update.h"
#include "command.h"
#include "cw.h"
#include "display.h"
#include "wdsp.h"
#include "channel.h"
#include "filter.h"
#include "main.h"
#include "meter.h"
#include "meter_update.h"
#include "ozy.h"
#include "preamp.h"
#include "property.h"
#include "soundcard.h"
#include "spectrum.h"
#include "spectrum_update.h"
#include "version.h"
#include "vfo.h"
#include "xvtr.h"
#include "band.h"
#include "mode.h"
#include "bandscope.h"
#include "setup.h"
#include "receiver.h"
#include "volume.h"
#include "transmit.h"
#include "subrx.h"
#include "audiostream.h"
#include "metis.h"
#include "cw.h"

#include "discovery.h"
#include "controller.h"


GdkColor background;
GdkColor buttonBackground;
GdkColor bandButtonBackground;
GdkColor buttonSelected;
GdkColor black;
GdkColor white;
GdkColor green;
GdkColor red;
GdkColor grey;
GdkColor plotColor;
GdkColor filterColor;
GdkColor subrxFilterColor;
GdkColor verticalColor;
GdkColor horizontalColor;
GdkColor spectrumTextColor;

GtkWidget* mainWindow;
GtkWidget* mainFixed;

GtkWidget* buttonDiscover;
GtkWidget* buttonSetup;
GtkWidget* buttonStart;


GtkWidget* vfoWindow;
GtkWidget* bandWindow;
GtkWidget* modeWindow;
GtkWidget* displayWindow;
GtkWidget* filterWindow;
GtkWidget* audioWindow;
GtkWidget* meterWindow;
GtkWidget* bandscopeWindow;
GtkWidget* bandscope_controlWindow;
GtkWidget* agcWindow;
GtkWidget* preampWindow;
GtkWidget* receiverWindow;
GtkWidget* volumeWindow;
GtkWidget* transmitWindow;
GtkWidget* subrxWindow;

gint mainStartX;
gint mainStartY;

gint mainRootX;
gint mainRootY;

char propertyPath[128];

gboolean running=FALSE;
gboolean started=FALSE;

extern int timing;

void saveState(void);
void restoreInitialState(void);
void restoreState(void);

void initAnalyzer(int channel);
void initBSAnalyzer(int channel);

/* --------------------------------------------------------------------------*/
/*
 * USB controller
 */

extern int vfoStep(void * data);
extern int setAFGain(void * data);
extern int nextBand(void * data);
extern int previousBand(void * data);

void controller_update(char* command) {
    char* p=command;
fprintf(stderr,"controller: %s\n", command);
    if(strncmp(p,"ZZSB",4)==0) {
        int *vfoinc=malloc(sizeof(int));
        *vfoinc=-1;
        g_idle_add(vfoStep,(gpointer)vfoinc);
    } else if(strncmp(p,"ZZSA",4)==0) {
        int *vfoinc=malloc(sizeof(int));
        *vfoinc=1;
        g_idle_add(vfoStep,(gpointer)vfoinc);
    } else if(strncmp(p,"ZZBD",4)==0) {
        g_idle_add(previousBand,NULL);
    } else if(strncmp(p,"ZZBU",4)==0) {
        g_idle_add(nextBand,NULL);
    } else if(strncmp(p,"ZZLA",4)==0) {
        p+=4; // skip over command
        int *gain=malloc(sizeof(int));
        *gain=(*p-'0') * 100;
        p++;
        *gain+=(*p-'0') * 10;
        p++;
        *gain+=*p-'0';
        g_idle_add(setAFGain,(gpointer)gain);
    } else if(strncmp(p,"ZZPC",4)==0) {
    } else {
        fprintf(stderr, "controller invalid command: '%s'\n",command);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Save the main GUI State. 
*/
/* ----------------------------------------------------------------------------*/
void mainSaveState() {
    char string[128];
    // save our location
    int x;
    int y;

    gtk_window_get_position((GtkWindow*)mainWindow,&x,&y);
    sprintf(string,"%d",x);
    setProperty("main.x",string);
    sprintf(string,"%d",y);
    setProperty("main.y",string);
    sprintf(string,"%d",meterUpdatesPerSecond);
    setProperty("meter.updates.per.second",string);
    sprintf(string,"%d",cwPitch);
    setProperty("cw.pitch",string);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Callback when application quits
*/
/* ----------------------------------------------------------------------------*/
void quit() {
    saveState();
    exit(0);
}

void saveState() {
    // save state
    mainSaveState();
    if(started) {
        vfoSaveState();
        bandSaveState();
        modeSaveState();
        displaySaveState();
        filterSaveState();
        audioSaveState();
        bandscopeSaveState();
        bandscope_controlSaveState();
        agcSaveState();
        receiverSaveState();
        volumeSaveState();
        ozySaveState();
        transmitSaveState();
        subrxSaveState();
        saveProperties(propertyPath);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief  Callback when on button is pressed
* 
* @param widget
* @param data
*/
/* ----------------------------------------------------------------------------*/
void onCallback(GtkWidget* widget,gpointer data) {
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Callback when discover button is pressed
* 
* @param widget
* @param data
*/
/* ----------------------------------------------------------------------------*/
void discoverCallback(GtkWidget* widget,gpointer data) {
    char title[128];
    strcpy(title,"Gtk+ GUI for HPSDR - Discovering devices ...");
    gtk_window_set_title((GtkWindow*)mainWindow,title);
    GtkWidget* dialog = gtk_message_dialog_new (NULL,
                                                 GTK_DIALOG_DESTROY_WITH_PARENT,
                                                 GTK_MESSAGE_INFO,
                                                 GTK_BUTTONS_NONE,
                                                 "Discovering ... Please Wait");
    gtk_widget_show(dialog);
    discovery();
    gtk_widget_hide(dialog);
    gtk_widget_destroy(dialog);
    if(devices==0) {
        strcpy(title,"Gtk+ GUI for HPSDR - No HPSDR devices found");
    } else {
        if(devices==1) {
            selected_device=0;
        } else {
            GtkWidget* dialog = gtk_message_dialog_new (NULL,
                                                 GTK_DIALOG_DESTROY_WITH_PARENT,
                                                 GTK_MESSAGE_QUESTION,
                                                 GTK_BUTTONS_OK_CANCEL,
                                                 "Discovery found %d devices!",
                                                 devices);
            gtk_message_dialog_format_secondary_text(GTK_MESSAGE_DIALOG(dialog),"Select required device");
            gtk_window_set_title(GTK_WINDOW(dialog),"GHPSDR");
            GtkWidget* area=gtk_message_dialog_get_message_area ((GtkMessageDialog*)dialog);
            GSList* group=NULL;
            char label[128];
            int i;
            GtkWidget* radioButton[devices];
            for(i=0;i<devices;i++) {
                DISCOVERED* d=&discovered[i];
fprintf(stderr,"main:  adding radio button device=%d software_version=%d status=%d\n",
                            d->device,
                            d->software_version,
                            d->status);

fprintf(stderr,"   address=%d\n", d->address.sin_addr.s_addr);
char* a;

fprintf(stderr,"%s\n", inet_ntoa(d->address.sin_addr));

fprintf(stderr,"    (%02X:%02X:%02X:%02X:%02X:%02X)\n",
                            d->mac_address[0],
                            d->mac_address[1],
                            d->mac_address[2],
                            d->mac_address[3],
                            d->mac_address[4],
                            d->mac_address[5]);

                sprintf(label,"%s %s (%02X:%02X:%02X:%02X:%02X:%02X) on %s\n",

                        d->name,
                        inet_ntoa(d->address.sin_addr),
                        d->mac_address[0],
                        d->mac_address[1],
                        d->mac_address[2],
                        d->mac_address[3],
                        d->mac_address[4],
                        d->mac_address[5],
                        d->interface_name);
               radioButton[i]=gtk_radio_button_new_with_label(group,label);
               gtk_toggle_button_set_active((GtkToggleButton*)radioButton[i],(gboolean)(i==0));
               gtk_widget_show(radioButton[i]);
               gtk_container_add((GtkContainer*)area,(GtkWidget*)radioButton[i]);
               group=gtk_radio_button_group (GTK_RADIO_BUTTON (radioButton[i]));
            }
            int result = gtk_dialog_run (GTK_DIALOG (dialog));
            switch (result) {
                case GTK_RESPONSE_OK:
                    for(i=0;i<devices;i++) {
                        if(gtk_toggle_button_get_active((GtkToggleButton *)radioButton[i])) {
                            selected_device=i;
                            break;
                        }
                    }
                    break;
                default:
                    exit(1);
                    break;
            }
            gtk_widget_hide (dialog);
            gtk_widget_destroy (dialog);
        }

        DISCOVERED* d=&discovered[selected_device];
        char *a=inet_ntoa(d->address.sin_addr);
        sprintf(title,"Gtk+ GUI for HPSDR: %s %s (%02X:%02X:%02X:%02X:%02X:%02X)\n",
                            d->name,
                            a,
                            d->mac_address[0],
                            d->mac_address[1],
                            d->mac_address[2],
                            d->mac_address[3],
                            d->mac_address[4],
                            d->mac_address[5]);
        gtk_widget_set_sensitive(buttonSetup, TRUE);
        gtk_widget_set_sensitive(buttonStart, TRUE);
        gtk_window_set_title((GtkWindow*)mainWindow,title);

        sprintf(propertyPath,"%02X-%02X-%02X-%02X-%02X-%02X.properties",
                        d->mac_address[0],
                        d->mac_address[1],
                        d->mac_address[2],
                        d->mac_address[3],
                        d->mac_address[4],
                        d->mac_address[5]);
        loadProperties(propertyPath);

        restoreInitialState();

        bandscopeRestoreState();
        bandscope_controlRestoreState();
        meterRestoreState();
        vfoRestoreState();
        bandRestoreState();
        modeRestoreState();
        filterRestoreState();
        displayRestoreState();
        audioRestoreState();
        agcRestoreState();
        receiverRestoreState();
        volumeRestoreState();
        transmitRestoreState();
        subrxRestoreState();
        restoreState();
        audio_stream_init();

        ozyRestoreState();
    }

}

/* --------------------------------------------------------------------------*/
/** 
* @brief Callback when setup button is pressed
* 
* @param widget
* @param data
*/
/* ----------------------------------------------------------------------------*/
void setupCallback(GtkWidget* widget,gpointer data) {
    ghpsdr_setup();
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Callback when start button is pressed
* 
* @param widget
* @param data
*/
/* ----------------------------------------------------------------------------*/
void startCallback(GtkWidget* widget,gpointer data) {
    gboolean retry;
    GtkWidget* label;
    gint result;

    fprintf(stderr,"main: startCallback\n");
    if(running) {
        // actually the Stop button
        gtk_button_set_label(GTK_BUTTON(buttonStart), "Start");
        label=gtk_bin_get_child((GtkBin*)buttonStart);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_set_sensitive(buttonDiscover, TRUE);
        stopSpectrumUpdate();
        stopMeterUpdate();
        stopBandscopeUpdate();
        ozy_stop();
        saveState();
        running=FALSE;
    } else {
        gtk_button_set_label(GTK_BUTTON(buttonStart), "Stop");
        label=gtk_bin_get_child((GtkBin*)buttonStart);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_set_sensitive(buttonDiscover, FALSE);
        running=TRUE;

        if(ozy_init()<0) {
            fprintf(stderr,"GHPSDR: cannot connect to USB device\n");
            exit(-1);
        }
    
        started=TRUE;

        if(displayHF) {
            setBand(band);
        } else {
            setBand(xvtr_band);
        }

        SetRXABandpassRun(CHANNEL_RX0, 1);
        SetRXAAMDSBMode(CHANNEL_RX0, 0);
        SetRXAShiftRun(CHANNEL_RX0, 0);
        SetRXAEMNRgainMethod(CHANNEL_RX0, 1);
        SetRXAEMNRnpeMethod(CHANNEL_RX0, 0);
        SetRXAEMNRaeRun(CHANNEL_RX0, 1);
        SetRXAEMNRPosition(CHANNEL_RX0, 0);
        SetRXAEMNRRun(CHANNEL_RX0, 0);
        SetRXAANRRun(CHANNEL_RX0, 0);
        SetRXAANFRun(CHANNEL_RX0, 0);

        SetRXABandpassRun(CHANNEL_SUBRX, 1);
        SetRXAAMDSBMode(CHANNEL_SUBRX, 0);
        SetRXAShiftRun(CHANNEL_SUBRX, 1);
        SetRXAEMNRgainMethod(CHANNEL_SUBRX, 1);
        SetRXAEMNRnpeMethod(CHANNEL_SUBRX, 0);
        SetRXAEMNRaeRun(CHANNEL_SUBRX, 1);
        SetRXAEMNRPosition(CHANNEL_SUBRX, 0);
        SetRXAEMNRRun(CHANNEL_SUBRX, 0);
        SetRXAANRRun(CHANNEL_SUBRX, 0);
        SetRXAANFRun(CHANNEL_SUBRX, 0);

        // get the display and meter running
        newSpectrumUpdate();
        newMeterUpdate();
        newBandscopeUpdate();

    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief KeyboardSnooper - intercepts all keyboard input and processes it
* 
* @param widget
* @param event
* 
* @return 
*/
/* ----------------------------------------------------------------------------*/
gint keyboardSnooper(GtkWidget* widget,GdkEventKey* event) {
    if(event->type==GDK_KEY_PRESS) {
        switch(event->keyval) {
            case GDK_q:
                vfoIncrementFrequency(1000000L,FALSE);
                break;
            case GDK_a:
                vfoIncrementFrequency(-1000000L,FALSE);
                break;
            case GDK_w:
                vfoIncrementFrequency(100000L,FALSE);
                break;
            case GDK_s:
                vfoIncrementFrequency(-100000L,FALSE);
                break;
            case GDK_e:
                vfoIncrementFrequency(10000L,FALSE);
                break;
            case GDK_d:
                vfoIncrementFrequency(-10000L,FALSE);
                break;
            case GDK_r:
                vfoIncrementFrequency(1000L,FALSE);
                break;
            case GDK_f:
                vfoIncrementFrequency(-1000L,FALSE);
                break;
            case GDK_t:
                vfoIncrementFrequency(100L,FALSE);
                break;
            case GDK_g:
                vfoIncrementFrequency(-100L,FALSE);
                break;
            case GDK_y:
                vfoIncrementFrequency(10L,FALSE);
                break;
            case GDK_h:
                vfoIncrementFrequency(-10L,FALSE);
                break;
            case GDK_u:
                vfoIncrementFrequency(1L,FALSE);
                break;
            case GDK_j:
                vfoIncrementFrequency(-1L,FALSE);
                break;
        }
    }
    return TRUE;
}


/* --------------------------------------------------------------------------*/
/** 
* @brief Move the window - no frame so we have to do it ourselves
* 
* @param widget
* @param event
* 
* @return 
*/
/* ----------------------------------------------------------------------------*/
gboolean mainTitle_button_press_event(GtkWidget* widget,GdkEventButton* event) {
    mainStartX=(int)event->x_root;
    mainStartY=(int)event->y_root;
    gtk_window_get_position((GtkWindow*)mainWindow,&mainRootX,&mainRootY);
    return TRUE;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief 
* 
* @param widget
* @param event
* 
* @return 
*/
/* ----------------------------------------------------------------------------*/
gboolean mainTitle_motion_notify_event(GtkWidget* widget,GdkEventMotion* event) {
    int incX;
    int incY;
    if(event->state & GDK_BUTTON1_MASK) {
        incX=(int)event->x_root-mainStartX;
        incY=(int)event->y_root-mainStartY;
        gtk_window_move((GtkWindow*)mainWindow,mainRootX+incX,mainRootY+incY);
    }
    return TRUE;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Build the GUI
*/
/* ----------------------------------------------------------------------------*/
void buildMainUI() {
    GtkWidget* label;
    char title[64];

    mainWindow = gtk_window_new (GTK_WINDOW_TOPLEVEL);

    if(ozy_use_metis()!=1) {
        strcpy(title,"Gtk+ GUI for HPSDR - USB");
    } else {
        strcpy(title,"Gtk+ GUI for HPSDR");
    }

    gtk_window_set_title((GtkWindow*)mainWindow,title);
    gtk_widget_modify_bg(mainWindow,GTK_STATE_NORMAL,&background);
    g_signal_connect(G_OBJECT(mainWindow),"destroy",G_CALLBACK(quit),NULL);

    mainFixed=gtk_fixed_new();
    gtk_widget_modify_bg(mainFixed,GTK_STATE_NORMAL,&background);

    buttonDiscover = gtk_button_new_with_label ("Discover");
    gtk_widget_modify_bg(buttonDiscover, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonDiscover);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonDiscover),LARGE_BUTTON_WIDTH,BUTTON_HEIGHT);
    g_signal_connect(G_OBJECT(buttonDiscover),"clicked",G_CALLBACK(discoverCallback),NULL);
    gtk_widget_show(buttonDiscover);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,buttonDiscover,2,0);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,buttonDiscover,5,0);
#endif
    if(ozy_use_metis()!=1) {
        gtk_widget_set_sensitive(buttonDiscover, FALSE);
    }

    buttonSetup = gtk_button_new_with_label ("Setup");
    gtk_widget_modify_bg(buttonSetup, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonSetup);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonSetup),LARGE_BUTTON_WIDTH,BUTTON_HEIGHT);
    g_signal_connect(G_OBJECT(buttonSetup),"clicked",G_CALLBACK(setupCallback),NULL);
    gtk_widget_show(buttonSetup);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,buttonSetup,82,0);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,buttonSetup,70,0);
#endif
    if(ozy_use_metis()==1) {
        gtk_widget_set_sensitive(buttonSetup, FALSE);
    }

    buttonStart = gtk_button_new_with_label ("Start");
    gtk_widget_modify_bg(buttonStart, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonStart);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonStart),LARGE_BUTTON_WIDTH,BUTTON_HEIGHT);
    g_signal_connect(G_OBJECT(buttonStart),"clicked",G_CALLBACK(startCallback),NULL);
    gtk_widget_show(buttonStart);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,buttonStart,162,0);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,buttonStart,140,0);
#endif
    if(ozy_use_metis()) {
        gtk_widget_set_sensitive(buttonStart, FALSE);
    }

    // add the vfo window
    gtk_widget_show(vfoWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,vfoWindow,164,0);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,vfoWindow,210,0);
#endif

    // add the meter window
    gtk_widget_show(meterWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,meterWindow,878,0);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,meterWindow,1010,0);
#endif

    // add the band window
    gtk_widget_show(bandWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,bandWindow,1,23);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,bandWindow,5,25);
#endif

    // add the mode window
    gtk_widget_show(modeWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,modeWindow,1,135);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,modeWindow,5,150);
#endif

    // add the filter window
    gtk_widget_show(filterWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,filterWindow,1,225);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,filterWindow,5,250);
#endif

    // add the audio window
    gtk_widget_show(audioWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,audioWindow,1,370);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,audioWindow,5,400);
#endif

    // add the agc window
    gtk_widget_show(agcWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,agcWindow,1,437);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,agcWindow,5,475);
#endif

    // add the preamp window
    gtk_widget_show(preampWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,preampWindow,1,483);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,preampWindow,5,525);
#endif

    // add the volume window
    gtk_widget_show(volumeWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,volumeWindow,164,483);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,volumeWindow,5,575);
#endif

    // add the receiver window
    gtk_widget_show(receiverWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,receiverWindow,254,483);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,receiverWindow,5,635);
#endif


    // add the display window
    gtk_widget_show(displayWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,displayWindow,164,40);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,displayWindow,210,50);
#endif

    // add the bandscope display
    gtk_widget_show(bandscopeWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,bandscopeWindow,164,364);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,bandscopeWindow,210,475);
#endif
    gtk_widget_show(bandscope_controlWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,bandscope_controlWindow,164,454);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,bandscope_controlWindow,210,575);
#endif

    // add the transmit window
    gtk_widget_show(transmitWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,transmitWindow,344,483);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,transmitWindow,210,625);
#endif

    // add the subrx window
    gtk_widget_show(subrxWindow);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,subrxWindow,616,483);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,subrxWindow,710,575);
#endif

#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(mainFixed),1024,576);
#else
    //gtk_widget_set_size_request(GTK_WIDGET(mainFixed),1180,700);
    gtk_widget_set_size_request(GTK_WIDGET(mainFixed),1244,700);
#endif
    gtk_widget_show(mainFixed);
    gtk_container_add(GTK_CONTAINER(mainWindow), mainFixed);

    //gtk_window_set_position((GtkWindow*)mainWindow,GTK_WIN_POS_MOUSE);
    gtk_window_move((GtkWindow*)mainWindow,mainRootX,mainRootY);

    gtk_widget_show(mainWindow);

}

/* --------------------------------------------------------------------------*/
/** 
* @brief Initialize the colors 
*/
/* ----------------------------------------------------------------------------*/
void initColors() {
    background.red=65535*55/256;
    background.green=65535*55/256;
    background.blue=65535*55/256;

    black.red=0;
    black.green=0;
    black.blue=0;

    white.red=65535;
    white.green=65535;
    white.blue=65535;

    red.red=65535;
    red.green=0;
    red.blue=0;

    grey.red=65535*64/256;
    grey.green=65535*64/256;
    grey.blue=65535*64/256;

    green.red=65535*97/256;
    green.green=65535*153/256;
    green.blue=65535*82/256;

    displayButton.red=65535*36/256;
    displayButton.green=65535*104/256;
    displayButton.blue=65535*107/256;

    buttonBackground.red=65535*32/256;
    buttonBackground.green=65535*32/256;
    buttonBackground.blue=65535*32/256;

    bandButtonBackground.red=65535*125/256;
    bandButtonBackground.green=65535*125/256;
    bandButtonBackground.blue=65535*125/256;

    buttonSelected.red=65535*206/256;
    buttonSelected.green=65535*138/256;
    buttonSelected.blue=65535*77/256;

    plotColor.red=65535*169/256;
    plotColor.green=65535*199/256;
    plotColor.blue=65535*171/256;

    filterColor.red=65535*35/256;
    filterColor.green=65535*129/256;
    filterColor.blue=65535*53/256;

    subrxFilterColor.red=65535*64/256;
    subrxFilterColor.green=65535*64/256;
    subrxFilterColor.blue=65535*64/256;

    verticalColor.red=65535*82/256;
    verticalColor.green=65535*34/256;
    verticalColor.blue=65535*82/256;
    
    horizontalColor.red=65535*39/256;
    horizontalColor.green=65535*15/256;
    horizontalColor.blue=65535*41/256;

    spectrumTextColor.red=65535*207/256;
    spectrumTextColor.green=65535*210/256;
    spectrumTextColor.blue=65535*179/256;

}

/* --------------------------------------------------------------------------*/
/** 
* @brief 
*/
/* ----------------------------------------------------------------------------*/
void restoreInitialState() {
    char* value;
    value=getProperty("main.x");
    if(value) mainRootX=atoi(value);
    value=getProperty("main.y");
    if(value) mainRootY=atoi(value);
    meterUpdatesPerSecond=METER_UPDATES_PER_SECOND;
    value=getProperty("meter.updates.per.second");
    if(value) meterUpdatesPerSecond=atoi(value);
    value=getProperty("cw.pitch");
    if(value) cwPitch=atoi(value);
}

void restoreState() {
}

//-------------------------------------------------------------------------------------------

/* --------------------------------------------------------------------------*/
/** 
* @brief Options structure
*/
/* ----------------------------------------------------------------------------*/
static struct option longOptions[] = {
    {"sound-card",required_argument, NULL, 'c'},
    {"sample-rate",required_argument, NULL, 's'},
    {"property-path",required_argument, NULL, 'p'},
    {"timing",no_argument, NULL, 't'},
    {"usb",no_argument, NULL, 'u'},
    {"interface",required_argument, NULL, 'i'},
    {0,0,0,0}
};

static char* shortOptions="c:s:p:t:u:m:i:";

static int optionIndex=0;

/* --------------------------------------------------------------------------*/
/** 
* @brief Process program arguments 
* 
* @param argc
* @param argv
*/
/* ----------------------------------------------------------------------------*/
void processCommands(int argc,char* argv[]) {
    int c;
    while(1) {
        c=getopt_long(argc,argv,shortOptions,longOptions,&optionIndex);
        if(c==-1) break;
        switch(c) {
            case 'c':
                strcpy(soundCardName,optarg);
                break;
            case 's':
                sampleRate=atoi(optarg);
                break;
            case 'p':
                strcpy(propertyPath,optarg);
                break;
            case 't':
                timing=1;
                break;
            case 'u':
                ozy_set_usb();
                break;
            case 'i':
                //ozy_set_interface(optarg);
                break;
            case '?':
                fprintf(stderr,"Usage:\n");
                fprintf(stderr,"    ghpsdr --metis\n");
                fprintf(stderr,"             --interface if\n");
                fprintf(stderr,"             --interface if\n");
                fprintf(stderr,"             --sound-card s\n");
                fprintf(stderr,"             --property-path path\n");
                fprintf(stderr,"             --sample-rate 48000|96000|192000\n");
                fprintf(stderr,"             --timing\n");
                exit(0);
                break;
            default:
                exit(0);
        }
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief  Main - it all starts here
* 
* @param argc
* @param argv[]
* 
* @return 
*/
/* ----------------------------------------------------------------------------*/
int main(int argc,char* argv[]) {
    gboolean retry;
    GtkWidget* dialog;
    gint result;
    int i;

    gtk_init(&argc,&argv);

    fprintf(stderr,"ghpsdr Version %s\n",VERSION);

    strcpy(soundCardName,"HPSDR");
    processCommands(argc,argv);

    if(init_controller()<0) {
        fprintf(stderr,"init_controller failed\n");
    }


    fprintf(stderr,"OpenChannel CHANNEL_RX0 buffer_size=%d fft_size=%d sampleRate=%d dspRate=%d outputRate=%d\n",
                buffer_size,
                fft_size,
                sampleRate,
                dspRate,
                outputRate);
    OpenChannel(CHANNEL_RX0,
                buffer_size,
                fft_size,
                sampleRate,
                dspRate,
                outputRate,
                0, // receive
                1, // run
                0.010, 0.025, 0.0, 0.010, 0);

    int success;
    fprintf(stderr,"XCreateAnalyzer CHANNEL_RX0\n");
    XCreateAnalyzer(CHANNEL_RX0, &success, 262144, 1, 1, "");
        if (success != 0) {
            fprintf(stderr, "XCreateAnalyzer CHANNEL_RX0 failed: %d\n" ,success);
        }

    initAnalyzer(CHANNEL_RX0);

    fprintf(stderr,"OpenChannel CHANNEL_TX buffer_size=%d fft_size=%d sampleRate=%d dspRate=%d outputRate=%d\n",
                buffer_size,
                fft_size,
                sampleRate,
                dspRate,
                outputRate);
    OpenChannel(CHANNEL_TX,
                buffer_size,
                fft_size,
                sampleRate,
                dspRate,
                outputRate,
                1, // transmit
                1, // run
                0.010, 0.025, 0.0, 0.010, 0);

    fprintf(stderr,"XCreateAnalyzer CHANNEL_TX\n");
    XCreateAnalyzer(CHANNEL_TX, &success, 262144, 1, 1, "");
        if (success != 0) {
            fprintf(stderr, "XCreateAnalyzer CHANNEL_TX failed: %d\n" ,success);
        }

    initAnalyzer(CHANNEL_TX);

    fprintf(stderr,"OpenChannel CHANNEL_SUBRX buffer_size=%d fft_size=%d sampleRate=%d dspRate=%d outputRate=%d\n",
                buffer_size,
                fft_size,
                sampleRate,
                dspRate,
                outputRate);
    OpenChannel(CHANNEL_SUBRX,
                buffer_size,
                fft_size,
                sampleRate,
                dspRate,
                outputRate,
                0, // receive
                1, // run
                0.010, 0.025, 0.0, 0.010, 0);


    fprintf(stderr,"XCreateAnalyzer CHANNEL_BS\n");
    XCreateAnalyzer(CHANNEL_BS, &success, 262144, 1, 1, "");
        if (success != 0) {
            fprintf(stderr, "XCreateAnalyzer CHANNEL_BS failed: %d\n" ,success);
        }

    initBSAnalyzer(CHANNEL_BS);

    create_divEXT(0, 0, 2, 1024);
    create_eerEXT(0, 0, 1024, 48000, 1.0, 1.0, FALSE, 0.0, 0.0, 1);


    mainRootX=0;
    mainRootY=0;

    cwPitch =600;

    restoreInitialState();

    initColors();
    //gtk_key_snooper_install((GtkKeySnoopFunc)keyboardSnooper,NULL);

    //bandscopeRestoreState();
    bandscopeWindow=buildBandscopeUI();
   
    //bandscope_controlRestoreState();
    bandscope_controlWindow=buildBandscope_controlUI();
   
    //meterRestoreState();
    meterWindow=buildMeterUI();

    //vfoRestoreState();
    vfoWindow=buildVfoUI();

    //bandRestoreState();
    bandWindow=buildBandUI();

    //modeRestoreState();
    modeWindow=buildModeUI();

    //filterRestoreState();
    filterWindow=buildFilterUI();

    //displayRestoreState();
    displayWindow=buildDisplayUI();

    //audioRestoreState();
    audioWindow=buildAudioUI();

    //agcRestoreState();
    agcWindow=buildAgcUI();

    preampWindow=buildPreampUI();

    //receiverRestoreState();
    receiverWindow=buildReceiverUI();

    //volumeRestoreState();
    volumeWindow=buildVolumeUI();

    //transmitRestoreState();
    transmitWindow=buildTransmitUI();

    //subrxRestoreState();
    subrxWindow=buildSubRxUI();

    restoreState();


    // build the Main UI
    buildMainUI();

    setSoundcard(getSoundcardId(soundCardName));

    if(ozy_use_metis()!=1) {
        sprintf(propertyPath,"usb.properties");
    }
    loadProperties(propertyPath);

    gtk_main();

    return 0;
}

void initAnalyzer(int channel) {
    int flp[] = {0};
    double KEEP_TIME = 0.1;
    int spur_elimination_ffts = 1;
    int data_type = 1;
    int fft_size = 8192;
    int window_type = 4;
    double kaiser_pi = 14.0;
    int overlap = 2048;
    int clip = 0;
    int span_clip_l = 0;
    int span_clip_h = 0;
    int pixels=spectrumWIDTH;
    int stitches = 1;
    int avm = 0;
    double tau = 0.001 * 120.0;
    int MAX_AV_FRAMES = 60;
    int display_average = MAX(2, (int) MIN((double) MAX_AV_FRAMES, (double) SPECTRUM_UPDATES_PER_SECOND * tau));
    double avb = exp(-1.0 / (SPECTRUM_UPDATES_PER_SECOND * tau));
    int calibration_data_set = 0;
    double span_min_freq = 0.0;
    double span_max_freq = 0.0;

    int max_w = fft_size + (int) MIN(KEEP_TIME * (double) SPECTRUM_UPDATES_PER_SECOND, KEEP_TIME * (double) fft_size * (double) SPECTRUM_UPDATES_PER_SECOND);

    fprintf(stderr,"SetAnalyzer channel=%d\n",channel);
    SetAnalyzer(channel,
            spur_elimination_ffts, //number of LO frequencies = number of ffts used in elimination
            data_type, //0 for real input data (I only); 1 for complex input data (I & Q)
            flp, //vector with one elt for each LO frequency, 1 if high-side LO, 0 otherwise
            fft_size, //size of the fft, i.e., number of input samples
            buffer_size, //number of samples transferred for each OpenBuffer()/CloseBuffer()
            window_type, //integer specifying which window function to use
            kaiser_pi, //PiAlpha parameter for Kaiser window
            overlap, //number of samples each fft (other than the first) is to re-use from the previous
            clip, //number of fft output bins to be clipped from EACH side of each sub-span
            span_clip_l, //number of bins to clip from low end of entire span
            span_clip_h, //number of bins to clip from high end of entire span
            pixels, //number of pixel values to return.  may be either <= or > number of bins
            stitches, //number of sub-spans to concatenate to form a complete span
            avm, //averaging mode
            display_average, //number of spans to (moving) average for pixel result
            avb, //back multiplier for weighted averaging
            calibration_data_set, //identifier of which set of calibration data to use
            span_min_freq, //frequency at first pixel value8192
            span_max_freq, //frequency at last pixel value
            max_w //max samples to hold in input ring buffers
    );
}

void initBSAnalyzer(int channel) {
    int flp[] = {0};
    double KEEP_TIME = 0.1;
    int spur_elimination_ffts = 1;
    int data_type = 1;
    int fft_size = 16384;
    int window_type = 1;
    double kaiser_pi = 14.0;
    int overlap = 2048;
    int clip = 0;
    int span_clip_l = 0;
    int span_clip_h = 0;
    int pixels=bandscopeWIDTH*2;
    int stitches = 1;
    int avm = 1;
    double tau = 0.001 * 120.0;
    int MAX_AV_FRAMES = 60;
    int display_average = MAX(2, (int) MIN((double) MAX_AV_FRAMES, (double) BANDSCOPE_UPDATES_PER_SECOND * tau));
    double avb = exp(-1.0 / (BANDSCOPE_UPDATES_PER_SECOND * tau));
    int calibration_data_set = 0;
    double span_min_freq = 0.0;
    double span_max_freq = 0.0;

    int max_w = fft_size + (int) MIN(KEEP_TIME * (double) BANDSCOPE_UPDATES_PER_SECOND, KEEP_TIME * (double) fft_size * (double) BANDSCOPE_UPDATES_PER_SECOND);


    SetAnalyzer(channel,
            spur_elimination_ffts, //number of LO frequencies = number of ffts used in elimination
            0, //0 for real input data (I only); 1 for complex input data (I & Q)
            flp, //vector with one elt for each LO frequency, 1 if high-side LO, 0 otherwise
            fft_size, //size of the fft, i.e., number of input samples
            2048, //number of samples transferred for each OpenBuffer()/CloseBuffer()
            1, //integer specifying which window function to use (Blackman-Harris)
            kaiser_pi, //PiAlpha parameter for Kaiser window
            overlap, //number of samples each fft (other than the first) is to re-use from the previous
            clip, //number of fft output bins to be clipped from EACH side of each sub-span
            span_clip_l, //number of bins to clip from low end of entire span
            span_clip_h, //number of bins to clip from high end of entire span
            pixels, //number of pixel values to return.  may be either <= or > number of bins
            stitches, //number of sub-spans to concatenate to form a complete span
            0, //averaging mode
            display_average, //number of spans to (moving) average for pixel result
            avb, //back multiplier for weighted averaging
            calibration_data_set, //identifier of which set of calibration data to use
            span_min_freq, //frequency at first pixel value8192
            span_max_freq, //frequency at last pixel value
            max_w //max samples to hold in input ring buffers
        );
}

