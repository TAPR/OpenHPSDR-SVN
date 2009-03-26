// main.c
//
// GTK+ 2.0 implementation of Beppe's Main control panel
// see http://www.radioamatore.it/sdr1000/mypowersdr.html for the original

#include <gtk/gtk.h>
#include <gdk/gdkkeysyms.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <getopt.h>

#include "audio.h"
#include "bandstack.h"
#include "command.h"
#include "display.h"
#include "dttsp.h"
#include "filter.h"
#include "main.h"
#include "mercury.h"
#include "meter.h"
#include "meter_update.h"
#include "property.h"
#include "soundcard.h"
#include "spectrum.h"
#include "spectrum_update.h"
#include "version.h"
#include "vfo.h"
#include "band.h"
#include "mode.h"
#include "bandscope.h"
#include "setup.h"

GdkColor background;
GdkColor buttonBackground;
GdkColor bandButtonBackground;
GdkColor bandButtonSelected;
GdkColor modeButtonSelected;
GdkColor black;
GdkColor white;
GdkColor mainButtonSelected;
GdkColor mainButtonUnselected;
GdkColor green;
GdkColor red;
GdkColor plotColor;
GdkColor filterColor;
GdkColor verticalColor;
GdkColor horizontalColor;
GdkColor spectrumTextColor;

GtkWidget* mainWindow;
GtkWidget* mainFixed;

GtkWidget* buttonExit;
GtkWidget* buttonSetup;


GtkWidget* vfoWindow;
GtkWidget* bandWindow;
GtkWidget* modeWindow;
GtkWidget* displayWindow;
GtkWidget* filterWindow;
GtkWidget* audioWindow;
GtkWidget* mercuryWindow;
GtkWidget* meterWindow;
GtkWidget* bandscopeWindow;

gint mainStartX;
gint mainStartY;

gint mainRootX;
gint mainRootY;

char propertyPath[128];

int meterDbm;
float preampOffset;

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
}

//-------------------------------------------------------------------------------------------
//
//  callback when application quits
//
void quit() {
    // save state
    mainSaveState();
    vfoSaveState();
    bandSaveState();
    modeSaveState();
    displaySaveState();
    filterSaveState();
    audioSaveState();
    mercurySaveState();
    bandscopeSaveState();

    saveProperties(propertyPath);

    exit(0);
}

//-------------------------------------------------------------------------------------------
//
//  callback when on button is pressed
//
void onCallback(GtkWidget* widget,gpointer data) {
}

//-------------------------------------------------------------------------------------------
//
//  callback when exit button is pressed
//
void exitCallback(GtkWidget* widget,gpointer data) {
    quit();
}
//-------------------------------------------------------------------------------------------
//
//  callback when setup button is pressed
//
void setupCallback(GtkWidget* widget,gpointer data) {
    setup();
}

//-------------------------------------------------------------------------------------------
//
// keyboardSnooper - intercepts all keyboard input and processes it
//
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

//-------------------------------------------------------------------------------------------
//
// move the window - no frame so we have to do it ourselves
//

gboolean mainTitle_button_press_event(GtkWidget* widget,GdkEventButton* event) {
    mainStartX=(int)event->x_root;
    mainStartY=(int)event->y_root;
    gtk_window_get_position((GtkWindow*)mainWindow,&mainRootX,&mainRootY);
    return TRUE;
}

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

//-------------------------------------------------------------------------------------------
//
// build the GUI
//
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
    gtk_widget_set_size_request(GTK_WIDGET(buttonExit),100,25);
    g_signal_connect(G_OBJECT(buttonExit),"clicked",G_CALLBACK(exitCallback),NULL);
    gtk_widget_show(buttonExit);
    gtk_fixed_put((GtkFixed*)mainFixed,buttonExit,0,0);

    buttonSetup = gtk_button_new_with_label ("Setup");
    gtk_widget_modify_bg(buttonSetup, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonSetup);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonSetup),100,25);
    g_signal_connect(G_OBJECT(buttonSetup),"clicked",G_CALLBACK(setupCallback),NULL);
    gtk_widget_show(buttonSetup);
    gtk_fixed_put((GtkFixed*)mainFixed,buttonSetup,100,0);

    gtk_widget_show(meterWindow);
    gtk_fixed_put((GtkFixed*)mainFixed,meterWindow,0,25);

    // add the filter window
    gtk_widget_show(filterWindow);
    gtk_fixed_put((GtkFixed*)mainFixed,filterWindow,520,spectrumHEIGHT+10);

    // add the audio window
    gtk_widget_show(audioWindow);
    gtk_fixed_put((GtkFixed*)mainFixed,audioWindow,730,spectrumHEIGHT+10);

    // add the mercury window
    gtk_widget_show(mercuryWindow);
    gtk_fixed_put((GtkFixed*)mainFixed,mercuryWindow,850,spectrumHEIGHT+10);

    // add the vfo window
    gtk_widget_show(vfoWindow);
    gtk_fixed_put((GtkFixed*)mainFixed,vfoWindow,0,80);

    // add the band window
    gtk_widget_show(bandWindow);
    gtk_fixed_put((GtkFixed*)mainFixed,bandWindow,0,180);

    // add the mode window
    gtk_widget_show(modeWindow);
    gtk_fixed_put((GtkFixed*)mainFixed,modeWindow,102,180);

    // add the display window
    gtk_widget_show(displayWindow);
    gtk_fixed_put((GtkFixed*)mainFixed,displayWindow,200,0);


    // add the bandscope display
    gtk_widget_show(bandscopeWindow);
    gtk_fixed_put((GtkFixed*)mainFixed,bandscopeWindow,200,spectrumHEIGHT+160);

    gtk_widget_set_size_request(GTK_WIDGET(mainFixed),1170,spectrumHEIGHT+160+bandscopeHEIGHT);
    gtk_widget_show(mainFixed);
    gtk_container_add(GTK_CONTAINER(mainWindow), mainFixed);

    //gtk_window_set_position((GtkWindow*)mainWindow,GTK_WIN_POS_MOUSE);
    gtk_window_move((GtkWindow*)mainWindow,mainRootX,mainRootY);

    gtk_widget_show(mainWindow);
  

    // set the band
    setBand(band);

    // get the display and meter running
    newSpectrumUpdate();
    newMeterUpdate();
    newBandscopeUpdate();
}

void initColors() {
    background.red=65535*44/256;
    background.green=65535*44/256;
    background.blue=65535*44/256;

    black.red=0;
    black.green=0;
    black.blue=0;

    white.red=65535;
    white.green=65535;
    white.blue=65535;

    red.red=65535;
    red.green=0;
    red.blue=0;

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

    bandButtonSelected.red=65535*206/256;
    bandButtonSelected.green=65535*138/256;
    bandButtonSelected.blue=65535*77/256;

    modeButtonSelected.red=65535*206/256;
    modeButtonSelected.green=65535*138/256;
    modeButtonSelected.blue=65535*77/256;

    mainButtonUnselected.red=65535*67/256;
    mainButtonUnselected.green=65535*144/256;
    mainButtonUnselected.blue=65535*160/256;

    mainButtonSelected.red=65535*206/256;
    mainButtonSelected.green=65535*138/256;
    mainButtonSelected.blue=65535*77/256;

    plotColor.red=65535*169/256;
    plotColor.green=65535*199/256;
    plotColor.blue=65535*171/256;

    filterColor.red=65535*35/256;
    filterColor.green=65535*129/256;
    filterColor.blue=65535*53/256;

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

void restoreInitialState() {
    char* value;
    value=getProperty("main.x");
    if(value) mainRootX=atoi(value);
    value=getProperty("main.y");
    if(value) mainRootY=atoi(value);
    meterUpdatesPerSecond=METER_UPDATES_PER_SECOND;
    value=getProperty("meter.updates.per.second");
    if(value) meterUpdatesPerSecond=atoi(value);
}

void restoreState() {
}

//-------------------------------------------------------------------------------------------

struct option longOptions[] = {
    {"sound-card",required_argument, 0, 0},
    {"sample-rate",required_argument, 0, 1},
    {"property-path",required_argument, 0, 2},
};

char* shortOptions="";

int optionIndex;

void processCommands(int argc,char** argv) {
    int c;
    while((c=getopt_long(argc,argv,shortOptions,longOptions,&optionIndex)!=EOF)) {
        switch(optionIndex) {
            case 0:
                strcpy(soundCardName,optarg);
                break;
            case 1:
                sampleRate=atoi(optarg);
                break;
            case 2:
                strcpy(propertyPath,optarg);
                break;
        }
    }
}

//-------------------------------------------------------------------------------------------
//
// main - it all starts here
//
int main(int argc,char* argv[]) {

    fprintf(stderr,"ghpsdr Version %s\n",VERSION);

    dttsp_main(argc,argv);
    writeCommand("setOsc 0");

    ozy_init(96000);

    gtk_init(&argc,&argv);

    strcpy(propertyPath,".ghpsdr.properties");

    strcpy(soundCardName,"UNSUPPORTED_CARD");

    processCommands(argc,argv);

    loadProperties(propertyPath);

    mainRootX=0;
    mainRootY=0;

    preampOffset=-16.0F;

    restoreInitialState();

    initColors();
    gtk_key_snooper_install((GtkKeySnoopFunc)keyboardSnooper,NULL);

    bandscopeRestoreState();
    bandscopeWindow=buildBandscopeUI();
   
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

    mercuryRestoreState();
    mercuryWindow=buildMercuryUI();

    restoreState();

    setSoundcard(getSoundcardId(soundCardName));

    // build the Main UI
    buildMainUI();

    gtk_main();
}
