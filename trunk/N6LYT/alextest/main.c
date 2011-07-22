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
* \section A Linux based, GTK2+, Radio Graphical User Interface to HPSDR boards through DttSP without Jack.  
* \author John Melton, G0ORX/N6LYT
* \version 0.1
* \date 2009-04-11
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
* Development of the system is documented at 
* http://javaguifordttsp.blogspot.com/
*
* This code is available at 
* svn://206.216.146.154/svn/repos_sdr_hpsdr/trunk/N6LYT/ghpsdr
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
#include "dttsp.h"
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
#include "test.h"
#include "alex_rx_test.h"
#include "alex_tx_test.h"

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

GtkWidget* buttonExit;
GtkWidget* buttonSetup;
GtkWidget* buttonTest;


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

extern int timing;

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
    // save state
    mainSaveState();
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

    exit(0);
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
* @brief Callback when exit button is pressed
* 
* @param widget
* @param data
*/
/* ----------------------------------------------------------------------------*/
void exitCallback(GtkWidget* widget,gpointer data) {
    quit();
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
* @brief Callback when test button is pressed
* 
* @param widget
* @param data
*/
/* ----------------------------------------------------------------------------*/
void testCallback(GtkWidget* widget,gpointer data) {
    test_setup();
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
                vfoIncrementFrequency(1000000L);
                break;
            case GDK_a:
                vfoIncrementFrequency(-1000000L);
                break;
            case GDK_w:
                vfoIncrementFrequency(100000L);
                break;
            case GDK_s:
                vfoIncrementFrequency(-100000L);
                break;
            case GDK_e:
                vfoIncrementFrequency(10000L);
                break;
            case GDK_d:
                vfoIncrementFrequency(-10000L);
                break;
            case GDK_r:
                vfoIncrementFrequency(1000L);
                break;
            case GDK_f:
                vfoIncrementFrequency(-1000L);
                break;
            case GDK_t:
                vfoIncrementFrequency(100L);
                break;
            case GDK_g:
                vfoIncrementFrequency(-100L);
                break;
            case GDK_y:
                vfoIncrementFrequency(10L);
                break;
            case GDK_h:
                vfoIncrementFrequency(-10L);
                break;
            case GDK_u:
                vfoIncrementFrequency(1L);
                break;
            case GDK_j:
                vfoIncrementFrequency(-1L);
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

    mainWindow = gtk_window_new (GTK_WINDOW_TOPLEVEL);
    gtk_window_set_title((GtkWindow*)mainWindow,"Gtk+ GUI for HPSDR");
    gtk_widget_modify_bg(mainWindow,GTK_STATE_NORMAL,&background);
    g_signal_connect(G_OBJECT(mainWindow),"destroy",G_CALLBACK(quit),NULL);

    mainFixed=gtk_fixed_new();
    gtk_widget_modify_bg(mainFixed,GTK_STATE_NORMAL,&background);

    buttonExit = gtk_button_new_with_label ("Quit");
    gtk_widget_modify_bg(buttonExit, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonExit);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonExit),LARGE_BUTTON_WIDTH,BUTTON_HEIGHT);
    g_signal_connect(G_OBJECT(buttonExit),"clicked",G_CALLBACK(exitCallback),NULL);
    gtk_widget_show(buttonExit);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,buttonExit,2,0);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,buttonExit,5,0);
#endif

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

    buttonTest = gtk_button_new_with_label ("Test");
    gtk_widget_modify_bg(buttonTest, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonTest);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonTest),LARGE_BUTTON_WIDTH,BUTTON_HEIGHT);
    g_signal_connect(G_OBJECT(buttonTest),"clicked",G_CALLBACK(testCallback),NULL);
    gtk_widget_show(buttonTest);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)mainFixed,buttonTest,162,0);
#else
    gtk_fixed_put((GtkFixed*)mainFixed,buttonTest,140,0);
#endif

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
    gtk_widget_set_size_request(GTK_WIDGET(mainFixed),1180,700);
#endif
    gtk_widget_show(mainFixed);
    gtk_container_add(GTK_CONTAINER(mainWindow), mainFixed);

    //gtk_window_set_position((GtkWindow*)mainWindow,GTK_WIN_POS_MOUSE);
    gtk_window_move((GtkWindow*)mainWindow,mainRootX,mainRootY);

    gtk_widget_show(mainWindow);
  

    // set the band
    if(displayHF) {
        setBand(band);
    } else {
        setBand(xvtr_band);
    }

    // get the display and meter running
    newSpectrumUpdate();
    newMeterUpdate();
    newBandscopeUpdate();
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
    {"metis",no_argument, NULL, 'm'},
    {"interface",required_argument, NULL, 'i'},
    {0,0,0,0}
};

static char* shortOptions="c:s:p:tmi:";

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
            case 'm':
                ozy_set_metis();
                break;
            case 'i':
                ozy_set_interface(optarg);
                break;
            case '?':
                fprintf(stderr,"Usage:\n");
                fprintf(stderr,"    alextest --metis\n");
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

    strcpy(propertyPath,".ghpsdr.properties");
    strcpy(soundCardName,"HPSDR");
    ozy_set_interface("eth0");
    processCommands(argc,argv);
    loadProperties(propertyPath);

    alex_rx_test_load("alex_rx_test.csv");
    alex_tx_test_load("alex_tx_test.csv");

    init_cw();

    // initialize DttSP
    Setup_SDR();
    Release_Update();
    SetTRX(0,FALSE); // thread 0 is for receive
    SetTRX(1,TRUE);  // thread 1 is for transmit
    SetThreadProcessingMode(0,2); // set thread 0 to RUN
    SetThreadProcessingMode(1,2); // set thread 1 to RUN
    SetSubRXSt(0,0,TRUE);

    reset_for_buflen(0,buffer_size);
    reset_for_buflen(1,buffer_size);


    // initialize ozy (default 48K)
    ozyRestoreState();
    do {
        switch(ozy_init()) {
            case -1: // cannot find ozy
                dialog = gtk_message_dialog_new (NULL,
                                                 GTK_DIALOG_DESTROY_WITH_PARENT,
                                                 GTK_MESSAGE_ERROR,
                                                 GTK_BUTTONS_YES_NO,
                                                 "Cannot locate Ozy!\n\nIs it powered on and plugged in?");
                gtk_message_dialog_format_secondary_text(GTK_MESSAGE_DIALOG(dialog),"Retry?");
                gtk_window_set_title(GTK_WINDOW(dialog),"GHPSDR");
                result = gtk_dialog_run (GTK_DIALOG (dialog));
                switch (result) {
                    case GTK_RESPONSE_YES:
                        retry=TRUE;
                        break;
                    default:
                        exit(1);
                        break;
                }
                gtk_widget_destroy (dialog);
                break;
            case -2: // found but needs initializing
                result=fork();
                if(result==0) {
                    // child process - exec initozy
                    fprintf(stderr,"exec initozy\n");
                    result=execl("initozy",NULL,NULL);
                    fprintf(stderr,"exec returned %d\n",result);
                    exit(result);
                } else if(result>0) {
                    // wait for the forked process to terminate
                    dialog = gtk_message_dialog_new (NULL,
                                                 GTK_DIALOG_DESTROY_WITH_PARENT,
                                                 GTK_MESSAGE_INFO,
                                                 GTK_BUTTONS_NONE,
                                                 "Initializing Ozy");
                    gtk_message_dialog_format_secondary_text(GTK_MESSAGE_DIALOG(dialog),"Please Wait ...");
                    gtk_window_set_title(GTK_WINDOW(dialog),"GHPSDR");
                    gtk_widget_show_all (dialog);
                    while (gtk_events_pending ())
                        gtk_main_iteration ();

                    wait(&result);
                    fprintf(stderr,"wait status=%d\n",result);
                    retry=TRUE;
                }
                gtk_widget_destroy (dialog);
                break;

            case -3: // did not find metis
                dialog = gtk_message_dialog_new (NULL,
                                                 GTK_DIALOG_DESTROY_WITH_PARENT,
                                                 GTK_MESSAGE_ERROR,
                                                 GTK_BUTTONS_YES_NO,
                                                 "Cannot locate Metis on interface %s!",
                                                 ozy_get_interface());
                gtk_message_dialog_format_secondary_text(GTK_MESSAGE_DIALOG(dialog),"Retry?");
                gtk_window_set_title(GTK_WINDOW(dialog),"GHPSDR");
                result = gtk_dialog_run (GTK_DIALOG (dialog));
                switch (result) {
                    case GTK_RESPONSE_YES:
                        retry=TRUE;
                        break;
                    default:
                        exit(1);
                        break;
                }
                gtk_widget_destroy (dialog);
                break;
            default:
                retry=FALSE;
                break;
        }
    } while(retry);

    mainRootX=0;
    mainRootY=0;

    cwPitch =600;

    restoreInitialState();

    SetKeyerResetSize(4096);
    NewKeyer(600.0f,TRUE,0.0f,3.0f,25.0f,48000.0f);
    SetKeyerPerf(FALSE);
    StartKeyer();

    initColors();
    //gtk_key_snooper_install((GtkKeySnoopFunc)keyboardSnooper,NULL);

    bandscopeRestoreState();
    bandscopeWindow=buildBandscopeUI();
   
    bandscope_controlRestoreState();
    bandscope_controlWindow=buildBandscope_controlUI();
   
    meterRestoreState();
    meterWindow=buildMeterUI();

    vfoRestoreState();
    vfoWindow=buildVfoUI();

    bandRestoreState();
    bandWindow=buildBandUI();

    modeRestoreState();
    modeWindow=buildModeUI();

    filterRestoreState();
    filterWindow=buildFilterUI();

    displayRestoreState();
    displayWindow=buildDisplayUI();

    audioRestoreState();
    audioWindow=buildAudioUI();


    agcRestoreState();
    agcWindow=buildAgcUI();

    preampWindow=buildPreampUI();

    receiverRestoreState();
    receiverWindow=buildReceiverUI();

    volumeRestoreState();
    volumeWindow=buildVolumeUI();

    transmitRestoreState();
    transmitWindow=buildTransmitUI();

    subrxRestoreState();
    subrxWindow=buildSubRxUI();

    restoreState();


    // build the Main UI
    buildMainUI();

    setSoundcard(getSoundcardId(soundCardName));

    audio_stream_init();


    gtk_main();

    return 0;
}
