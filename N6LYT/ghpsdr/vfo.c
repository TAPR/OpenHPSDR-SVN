/** 
* @file vfo.c
* @brief VFO functions
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-12
*/
// vfo.c

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

#include "screensize.h"
#include "command.h"
#include "main.h"
#include "xvtr.h"
#include "band.h"
#include "ozy.h"
#include "property.h"
#include "vfo.h"
#include "subrx.h"

GtkWidget* vfoFixed;

long long frequencyA=7100000LL;
long long frequencyB=7100000LL;

int frequencyAChanged=0;
int frequencyBChanged=0;

long long frequencyALO=0LL;
long long frequencyBLO=0LL;

long long dspAFrequency;
long long ddsAFrequency;

long long dspBFrequency;
long long ddsBFrequency;

#define OFFSET 0

gint vfoTimerId;

GtkWidget* vfoAFrame;
GtkWidget* vfoAFrequency;
GdkPixmap* vfoAPixmap;
GtkWidget* vfoBFrame;
GtkWidget* vfoBFrequency;
GdkPixmap* vfoBPixmap;

GtkWidget* vfoControlFrame;
GtkWidget* buttonAtoB;
GtkWidget* buttonBtoA;
GtkWidget* buttonAswapB;
GtkWidget* buttonSplit;

long frequencyIncrement=1000;
GtkWidget* buttonFrequencyUp;
GtkWidget* buttonIncrementPlus;
GtkWidget* incrementDisplay;
GdkPixmap* incrementPixmap;
GtkWidget* buttonIncrementMinus;
GtkWidget* buttonFrequencyDown;

int aTransmitting=0;
int bTransmitting=0;
int bSplit=0;
int splitChanged=0;

void setIncrement(int increment);
void vfoIncrementFrequency(long increment,gboolean round);

int vfoStep(void * data) {
    int step=*(int*)data;
    vfoIncrementFrequency(step*frequencyIncrement,TRUE);
    free(data);
    return 0;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief update vfo a display
* 
* @param widget
* @param event
* 
* @return 
*/
void updateVfoADisplay() {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    if(vfoAFrequency->window) {
        gc=gdk_gc_new(vfoAFrequency->window);
        gdk_gc_set_rgb_fg_color(gc,&background);
        gdk_draw_rectangle(vfoAPixmap,
                           gc,
                           TRUE,
                           0,0,
                           vfoAFrequency->allocation.width,
                           vfoAFrequency->allocation.height);

        context = gdk_pango_context_get_for_screen (gdk_screen_get_default ());
        layout = pango_layout_new (context);
        pango_layout_set_width(layout,vfoAFrequency->allocation.width*PANGO_SCALE);
        pango_layout_set_alignment(layout,PANGO_ALIGN_RIGHT);
#ifdef NETBOOK
        sprintf(temp,"<span foreground='%s' background='#2C2C2C' font_desc='Sans Bold 12'>% 7lld.%03lld.%03lld </span>",aTransmitting?"#FF0000":"#00FF00",frequencyA/1000000LL,(frequencyA%1000000LL)/1000LL,frequencyA%1000LL);
#else
        sprintf(temp,"<span foreground='%s' background='#2C2C2C' font_desc='Sans Bold 24'>% 7lld.%03lld.%03lld </span>",aTransmitting?"#FF0000":"#00FF00",frequencyA/1000000LL,(frequencyA%1000000LL)/1000LL,frequencyA%1000LL);
#endif
        pango_layout_set_markup(layout,temp,-1);
        gdk_draw_layout(GDK_DRAWABLE(vfoAPixmap),gc,0,0,layout);

        gdk_gc_set_rgb_fg_color(gc,&grey);
        gdk_draw_rectangle(vfoAPixmap,
                           gc,
                           FALSE,
                           0,0,
                           vfoAFrequency->allocation.width-1,
                           vfoAFrequency->allocation.height-1);

        g_object_unref(context);
        g_object_unref(layout);
        g_object_unref(gc);

        gtk_widget_queue_draw(vfoAFrequency);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief update vfo b display
* 
* @param widget
* @param event
* 
* @return 
*/
void updateVfoBDisplay() {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    if(vfoBFrequency->window) {
        gc=gdk_gc_new(vfoBFrequency->window);
        gdk_gc_set_rgb_fg_color(gc,&background);
        gdk_draw_rectangle(vfoBPixmap,
                           gc,
                           TRUE,
                           0,0,
                           vfoBFrequency->allocation.width,
                           vfoBFrequency->allocation.height);

        context = gdk_pango_context_get_for_screen (gdk_screen_get_default ());
        layout = pango_layout_new (context);
        pango_layout_set_width(layout,vfoBFrequency->allocation.width*PANGO_SCALE);
        pango_layout_set_alignment(layout,PANGO_ALIGN_RIGHT);
#ifdef NETBOOK
        sprintf(temp,"<span foreground='%s' background='#2C2C2C' font_desc='Sans Bold 12'>% 7lld.%03lld.%03lld </span>",bTransmitting?"#FF0000":"#C0C0C0",frequencyB/1000000LL,(frequencyB%1000000LL)/1000LL,frequencyB%1000LL);
#else
        sprintf(temp,"<span foreground='%s' background='#2C2C2C' font_desc='Sans Bold 24'>% 7lld.%03lld.%03lld </span>",bTransmitting?"#FF0000":"#C0C0C0",frequencyB/1000000LL,(frequencyB%1000000LL)/1000LL,frequencyB%1000LL);
#endif

        pango_layout_set_markup(layout,temp,-1);
        gdk_draw_layout(GDK_DRAWABLE(vfoBPixmap),gc,0,0,layout);

        gdk_gc_set_rgb_fg_color(gc,&grey);
        gdk_draw_rectangle(vfoBPixmap,
                           gc,
                           FALSE,
                           0,0,
                           vfoBFrequency->allocation.width-1,
                           vfoBFrequency->allocation.height-1);

        g_object_unref(context);
        g_object_unref(layout);
        g_object_unref(gc);

        gtk_widget_queue_draw(vfoBFrequency);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Callback when vfo A is is created
* 
* @param widget
* @param event
* 
* @return 
*/
gboolean vfoAFrequency_configure_event(GtkWidget* widget,GdkEventConfigure* event) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    if(vfoAPixmap) g_object_unref(vfoAPixmap);

    vfoAPixmap=gdk_pixmap_new(widget->window,widget->allocation.width,widget->allocation.height,-1);

    gc=gdk_gc_new(widget->window);
    gdk_gc_set_rgb_fg_color(gc,&background);
    gdk_draw_rectangle(vfoAPixmap,
                       gc,
                       TRUE,
                       0,0,
                       widget->allocation.width,
                       widget->allocation.height);

    context = gdk_pango_context_get_for_screen(gdk_screen_get_default ());
    layout = pango_layout_new(context);
    pango_layout_set_width(layout,widget->allocation.width*PANGO_SCALE);
    pango_layout_set_alignment(layout,PANGO_ALIGN_RIGHT);
#ifdef NETBOOK
    sprintf(temp,"<span foreground='%s' background='#2C2C2C' font_desc='Sans Bold 12'>% 7lld.%03lld.%03lld </span>",aTransmitting?"#FF0000":"#00FF00",frequencyA/1000000LL,(frequencyA%1000000LL)/1000LL,frequencyA%1000LL);
#else
    sprintf(temp,"<span foreground='%s' background='#2C2C2C' font_desc='Sans Bold 24'>% 7lld.%03lld.%03lld </span>",aTransmitting?"#FF0000":"#00FF00",frequencyA/1000000LL,(frequencyA%1000000LL)/1000LL,frequencyA%1000LL);
#endif
    pango_layout_set_markup(layout,temp,-1);
    gdk_draw_layout(GDK_DRAWABLE(vfoAPixmap),gc,0,0,layout);

    gdk_gc_set_rgb_fg_color(gc,&grey);
    gdk_draw_rectangle(vfoAPixmap,
                       gc,
                       FALSE,
                       0,0,
                       widget->allocation.width-1,
                       widget->allocation.height-1);

    g_object_unref(context);
    g_object_unref(layout);
    g_object_unref(gc);

    return TRUE;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Callback when vfo A is is exposed - need to paint it from the pixmap
* 
* @param widget
* @param event
* 
* @return 
*/
gboolean vfoAFrequency_expose_event(GtkWidget* widget,GdkEventExpose* event) {
    gdk_draw_drawable(widget->window,
                    widget->style->fg_gc[GTK_WIDGET_STATE (widget)],
                    vfoAPixmap,
                    event->area.x, event->area.y,
                    event->area.x, event->area.y,
                    event->area.width, event->area.height);
    return FALSE;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief  Callback when vfo B is created
* 
* @param widget
* @param event
* 
* @return 
*/
gboolean vfoBFrequency_configure_event(GtkWidget* widget,GdkEventConfigure* event) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    if(vfoBPixmap) g_object_unref(vfoBPixmap);

    vfoBPixmap=gdk_pixmap_new(widget->window,widget->allocation.width,widget->allocation.height,-1);

    gc=gdk_gc_new(widget->window);
    gdk_gc_set_rgb_fg_color(gc,&background);
    gdk_draw_rectangle(vfoBPixmap,
                       gc,
                       TRUE,
                       0,0,
                       widget->allocation.width,
                       widget->allocation.height);

    context = gdk_pango_context_get_for_screen (gdk_screen_get_default ());
    layout = pango_layout_new (context);
    pango_layout_set_width(layout,widget->allocation.width*PANGO_SCALE);
    pango_layout_set_alignment(layout,PANGO_ALIGN_RIGHT);
#ifdef NETBOOK
    sprintf(temp,"<span foreground='#C0C0C0' background='#2C2C2C' font_desc='Sans Bold 12'>% 7lld.%03lld.%03lld </span>",frequencyB/1000000LL,(frequencyB%1000000LL)/1000LL,frequencyB%1000LL);
#else
    sprintf(temp,"<span foreground='#C0C0C0' background='#2C2C2C' font_desc='Sans Bold 24'>% 7lld.%03lld.%03lld </span>",frequencyB/1000000LL,(frequencyB%1000000LL)/1000LL,frequencyB%1000LL);
#endif

    pango_layout_set_markup(layout,temp,-1);
    gdk_draw_layout(GDK_DRAWABLE(vfoBPixmap),gc,0,0,layout);

    gdk_gc_set_rgb_fg_color(gc,&grey);
    gdk_draw_rectangle(vfoBPixmap,
                       gc,
                       FALSE,
                       0,0,
                       widget->allocation.width-1,
                       widget->allocation.height-1);

    g_object_unref(context);
    g_object_unref(layout);
    g_object_unref(gc);

    return TRUE;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Callback when vfo B is exposed - need to paint it from the pixmap
* 
* @param widget
* @param event
* 
* @return 
*/
gboolean vfoBFrequency_expose_event(GtkWidget* widget,GdkEventExpose* event) {
    gdk_draw_drawable(widget->window,
                    widget->style->fg_gc[GTK_WIDGET_STATE (widget)],
                    vfoBPixmap,
                    event->area.x, event->area.y,
                    event->area.x, event->area.y,
                    event->area.width, event->area.height);
    return FALSE;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set the A vfo frequency
* 
* @param f
*/
void setAFrequency(long long f) {

    dspAFrequency=0;
    ddsAFrequency=0;
    frequencyA=f;

    dspAFrequency=0;
    ddsAFrequency=f-frequencyALO;

    updateVfoADisplay();

    frequencyAChanged=1;

    // check the band
    if(displayHF) {
        int thisBand=getBand(f);
        if(band!=thisBand) {
            if(band!=-1) {
fprintf(stderr,"setAFrequency\n");
                forceBand(thisBand,FALSE);
            }
        }
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief  Set the B vfo frequency
* 
* @param f
*/
void setBFrequency(long long f) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    dspBFrequency=0;
    ddsBFrequency=0;
    frequencyB=f;

    dspBFrequency=0;
    ddsBFrequency=f-frequencyBLO;

    updateVfoBDisplay();

    frequencyBChanged=1;

//    if(bSubRx) {
//        /* sub rx can only run within the sampled spectrum range */
//        if((f>=(frequencyA-(sampleRate/2)))&& (f<=(frequencyA+(sampleRate/2)))) {
//            frequencyB=f;
//        }
//    } else {
//        frequencyB=f;
//    }
//    updateVfoBDisplay();
//    frequencyBChanged=1;
//
//    if(bSubRx) {
//        long long diff=frequencyA-frequencyB;
//        SetRXOsc(0,1,(double)diff);
//    }

}

/* --------------------------------------------------------------------------*/
/** 
* @brief  Callback when a vfo button is pressed
* 
* @param widget
* @param data
*/
void vfoCallback(GtkWidget* widget,gpointer data) {
    long long f;
    GtkWidget* label;

    if(widget==buttonAtoB) {
        setBFrequency(frequencyA);
    } else if(widget==buttonBtoA) {
        setAFrequency(frequencyB);
    } else if(widget==buttonAswapB) {
        f=frequencyA;
        setAFrequency(frequencyB);
        setBFrequency(f);
    } else if(widget==buttonSplit) {
        label=gtk_bin_get_child((GtkBin*)buttonSplit);
        if(bSplit) {
            bSplit=0;
            gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
            gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &white);
            splitChanged=1;
            frequencyAChanged=1;
        } else {
            bSplit=1;
            gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &red);
            gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &red);
            splitChanged=1;
            frequencyBChanged=1;
        }
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Increment the frequency
* 
* @param increment
*/
void vfoIncrementFrequency(long increment,gboolean round) {
    if(subrx) {
        subrxIncrementFrequency(increment,round);
    } else {
        if(round) {
            setAFrequency(((frequencyA+(long long)increment)/(long long)frequencyIncrement)*(long long)frequencyIncrement);
        } else {
            setAFrequency(frequencyA+(long long)increment);
        }
    }
}

/* --------------------------------------------------------------------------*/
/** 
/* --------------------------------------------------------------------------*/
/** 
* @brief Increment the A frequency
* 
* @param increment
*/
void vfoIncrementAFrequency(long increment,gboolean round) {
    if(round) {
        setAFrequency(((frequencyA+(long long)increment)/(long long)frequencyIncrement)*(long long)frequencyIncrement);
    } else {
        setAFrequency(frequencyA+(long long)increment);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Increment the B frequency
* 
* @param increment
*/
void vfoIncrementBFrequency(long increment,gboolean round) {
    if(round) {
        setBFrequency(((frequencyB+(long long)increment)/(long long)frequencyIncrement)*(long long)frequencyIncrement);
    } else {
        setBFrequency(frequencyB+(long long)increment);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief interface to allow calling from iphone using g_idle_add
* 
* @param increment
*/
int vfoStepFrequency(gpointer data) {
    long step=*(long*)data;
    vfoIncrementFrequency(step*50,FALSE);
    free(data);
    return 0;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Frequency up timer 
* 
* @param data
* 
* @return 
*/
gint frequencyUpTimer(gpointer data) {
    gtk_timeout_remove(vfoTimerId);
    vfoIncrementFrequency(frequencyIncrement,FALSE);
    vfoTimerId=gtk_timeout_add(50,frequencyUpTimer,NULL);
    return TRUE;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief  Callback when a frequency Up button is pressed
* 
* @param widget
* @param data
*/
void frequencyUpCallback(GtkWidget* widget,gpointer data) {
    vfoIncrementFrequency(frequencyIncrement,FALSE);
    vfoTimerId=gtk_timeout_add(500,frequencyUpTimer,NULL);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Frequency down timer 
* 
* @param data
* 
* @return 
*/
gint frequencyDownTimer(gpointer data) {
    gtk_timeout_remove(vfoTimerId);
    vfoIncrementFrequency(-frequencyIncrement,FALSE);
    vfoTimerId=gtk_timeout_add(50,frequencyDownTimer,NULL);
    return TRUE;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Callback when a frequencyDown button is pressed
* 
* @param widget
* @param data
*/
void frequencyDownCallback(GtkWidget* widget,gpointer data) {
    vfoIncrementFrequency(-frequencyIncrement,FALSE);
    vfoTimerId=gtk_timeout_add(500,frequencyDownTimer,NULL);
}


/* --------------------------------------------------------------------------*/
/** 
* @brief  Callback when a frequencyUp/Down button is released
* 
* @param widget
* @param data
*/
void frequencyReleasedCallback(GtkWidget* widget,gpointer data) {
    gtk_timeout_remove(vfoTimerId);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Frequency scroll wheel
* 
* @param widget
* @param event
* 
* @return 
*/
gboolean frequency_scroll_event(GtkWidget* widget,GdkEventScroll* event) {
    long increment=frequencyIncrement;

    if(event->direction==GDK_SCROLL_UP) {
        increment=frequencyIncrement;
    } else {
        increment=-frequencyIncrement;
    }
    if(widget==vfoAFrequency) {
        vfoIncrementAFrequency(increment,FALSE);
    } else {
        vfoIncrementBFrequency(increment,FALSE);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief  Callback when a incrementDisplay is exposed
* 
* @param widget
* @param event
* 
* @return 
*/
gboolean incrementDisplay_expose_event(GtkWidget* widget,GdkEventExpose* event) {
    gdk_draw_drawable(widget->window,
                    widget->style->fg_gc[GTK_WIDGET_STATE (widget)],
                    incrementPixmap,
                    event->area.x, event->area.y,
                    event->area.x, event->area.y,
                    event->area.width, event->area.height);
    return FALSE;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Draw increment display
* 
* @param queue
*/
void drawIncrementDisplay(gboolean queue) {

    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    if(incrementDisplay->window) {
        if(incrementPixmap) g_object_unref(incrementPixmap);

        incrementPixmap=gdk_pixmap_new(incrementDisplay->window,incrementDisplay->allocation.width,incrementDisplay->allocation.height,-1);

        gc=gdk_gc_new(incrementDisplay->window);
        gdk_gc_set_rgb_fg_color(gc,&black);
        gdk_draw_rectangle(incrementPixmap,
                           gc,
                           TRUE,
                           0,0,
                           incrementDisplay->allocation.width,
                           incrementDisplay->allocation.height);

        context = gdk_pango_context_get_for_screen (gdk_screen_get_default ());
        layout = pango_layout_new (context);
        pango_layout_set_width(layout,incrementDisplay->allocation.width*PANGO_SCALE);
        pango_layout_set_alignment(layout,PANGO_ALIGN_CENTER);
        sprintf(temp,"<span foreground='#7AAA6E' background='#2C2C2C' font_desc='Sans Bold 10'>%0ld</span>",frequencyIncrement);
        pango_layout_set_markup(layout,temp,-1);
        gdk_draw_layout(GDK_DRAWABLE(incrementPixmap),gc,0,0,layout);
    
        g_object_unref(context);
        g_object_unref(layout);
        g_object_unref(gc);

        if(queue) gtk_widget_queue_draw(incrementDisplay);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Callback when display configure event occurs
* 
* @param widget
* @param event
* 
* @return 
*/
gboolean incrementDisplay_configure_event(GtkWidget* widget,GdkEventConfigure* event) {
    drawIncrementDisplay(FALSE);
    return TRUE;
}


/* --------------------------------------------------------------------------*/
/** 
* @brief  Increment up/down
* 
* @param increment
*/
void setIncrement(int increment) {
    frequencyIncrement=increment;
    drawIncrementDisplay(TRUE);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Next increment
*/
void nextIncrement() {

    switch(frequencyIncrement) {
        case 1:
            frequencyIncrement=10;
            break;
        case 10:
            frequencyIncrement=25;
            break;
        case 25:
            frequencyIncrement=50;
            break;
        case 50:
            frequencyIncrement=100;
            break;
        case 100:
            frequencyIncrement=250;
            break;
        case 250:
            frequencyIncrement=500;
            break;
        case 500:
            frequencyIncrement=1000;
            break;
        case 1000:
            frequencyIncrement=5000;
            break;
        case 5000:
            frequencyIncrement=9000;
            break;
        case 9000:
            frequencyIncrement=10000;
            break;
        case 10000:
            frequencyIncrement=100000;
            break;
        case 100000:
            frequencyIncrement=250000;
            break;
        case 250000:
            frequencyIncrement=500000;
            break;
        case 500000:
            frequencyIncrement=1000000;
            break;
        case 1000000:
            frequencyIncrement=1;
            break;
    }
    drawIncrementDisplay(TRUE);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Previous increment
*/
void previousIncrement() {
    switch(frequencyIncrement) {
        case 1:
            frequencyIncrement=1000000;
            break;
        case 10:
            frequencyIncrement=1;
            break;
        case 25:
            frequencyIncrement=10;
            break;
        case 50:
            frequencyIncrement=25;
            break;
        case 100:
            frequencyIncrement=50;
            break;
        case 250:
            frequencyIncrement=100;
            break;
        case 500:
            frequencyIncrement=250;
            break;
        case 1000:
            frequencyIncrement=500;
            break;
        case 5000:
            frequencyIncrement=1000;
            break;
        case 9000:
            frequencyIncrement=5000;
            break;
        case 10000:
            frequencyIncrement=9000;
            break;
        case 100000:
            frequencyIncrement=10000;
            break;
        case 250000:
            frequencyIncrement=100000;
            break;
        case 500000:
            frequencyIncrement=250000;
            break;
        case 1000000:
            frequencyIncrement=500000;
            break;
    }
    drawIncrementDisplay(TRUE);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set IF frequency
*/
void setLOFrequency(long long freq) {
    frequencyALO=freq;
    frequencyBLO=freq;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Callback on button increment plus 
* 
* @param widget
* @param data
*/
void buttonIncrementPlusCallback(GtkWidget* widget,gpointer data) {
    nextIncrement();
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Callback on button increment minus 
* 
* @param widget
* @param data
*/
void buttonIncrementMinusCallback(GtkWidget* widget,gpointer data) {
    previousIncrement();
}
/* --------------------------------------------------------------------------*/
/** 
* @brief Increment scroll wheel
* 
* @param widget
* @param event
* 
* @return 
*/
gboolean increment_scroll_event(GtkWidget* widget,GdkEventScroll* event) {
    if(event->direction==GDK_SCROLL_UP) {
        nextIncrement();
    } else {
        previousIncrement();
    }
    return TRUE;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Build the GUI
* 
* @return 
*/
GtkWidget* buildVfoUI() {
    GtkWidget* label;

    vfoFixed=gtk_fixed_new();
    gtk_widget_modify_bg(vfoFixed,GTK_STATE_NORMAL,&background);

    // vfoA
    vfoAFrame=gtk_frame_new("VFO A");
    label=gtk_frame_get_label_widget((GtkFrame*)vfoAFrame);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(vfoAFrame),258,40);
#else
    gtk_widget_set_size_request(GTK_WIDGET(vfoAFrame),300,50);
#endif
    vfoAFrequency=gtk_drawing_area_new();
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(vfoAFrequency),240,35);
#else
    gtk_widget_set_size_request(GTK_WIDGET(vfoAFrequency),250,35);
#endif
    g_signal_connect(G_OBJECT (vfoAFrequency),"configure_event",G_CALLBACK(vfoAFrequency_configure_event),NULL);
    g_signal_connect(G_OBJECT (vfoAFrequency),"expose_event",G_CALLBACK(vfoAFrequency_expose_event),NULL);
    g_signal_connect(G_OBJECT(vfoAFrequency),"scroll_event",G_CALLBACK(frequency_scroll_event),NULL);
    gtk_widget_set_events(vfoAFrequency,GDK_EXPOSURE_MASK|GDK_SCROLL_MASK);
    gtk_widget_show(vfoAFrequency);
    gtk_widget_show(vfoAFrame);
    gtk_container_add((GtkContainer*)vfoAFrame,vfoAFrequency);
    gtk_fixed_put((GtkFixed*)vfoFixed,vfoAFrame,0,0);

    // vfoB
    vfoBFrame=gtk_frame_new("VFO B");
    label=gtk_frame_get_label_widget((GtkFrame*)vfoBFrame);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(vfoBFrame),258,40);
#else
    gtk_widget_set_size_request(GTK_WIDGET(vfoBFrame),300,50);
#endif
    vfoBFrequency=gtk_drawing_area_new();
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(vfoBFrequency),240,35);
#else
    gtk_widget_set_size_request(GTK_WIDGET(vfoBFrequency),300,35);
#endif
    g_signal_connect(G_OBJECT (vfoBFrequency),"configure_event",G_CALLBACK(vfoBFrequency_configure_event),NULL);
    g_signal_connect(G_OBJECT (vfoBFrequency),"expose_event",G_CALLBACK(vfoBFrequency_expose_event),NULL);
    g_signal_connect(G_OBJECT(vfoBFrequency),"scroll_event",G_CALLBACK(frequency_scroll_event),NULL);
    gtk_widget_set_events(vfoBFrequency,GDK_EXPOSURE_MASK|GDK_SCROLL_MASK);
    gtk_widget_show(vfoBFrequency);
    gtk_widget_show(vfoBFrame);
    gtk_container_add((GtkContainer*)vfoBFrame,vfoBFrequency);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)vfoFixed,vfoBFrame,456,0);
#else
    gtk_fixed_put((GtkFixed*)vfoFixed,vfoBFrame,500,0);
#endif

    // vfo control
    vfoControlFrame=gtk_frame_new(NULL);

    buttonAtoB = gtk_button_new_with_label ("A>B");
    gtk_widget_modify_bg(buttonAtoB, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonAtoB);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(buttonAtoB),50,20);
#else
    gtk_widget_set_size_request(GTK_WIDGET(buttonAtoB),45,25);
#endif
    g_signal_connect(G_OBJECT(buttonAtoB),"clicked",G_CALLBACK(vfoCallback),NULL);
    gtk_widget_show(buttonAtoB);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonAtoB,258,0);
#else
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonAtoB,300,0);
#endif

    buttonAswapB = gtk_button_new_with_label ("A<>B");
    gtk_widget_modify_bg(buttonAswapB, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonAswapB);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(buttonAswapB),49,20);
#else
    gtk_widget_set_size_request(GTK_WIDGET(buttonAswapB),60,25);
#endif
    g_signal_connect(G_OBJECT(buttonAswapB),"clicked",G_CALLBACK(vfoCallback),NULL);
    gtk_widget_show(buttonAswapB);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonAswapB,308,0);
#else
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonAswapB,345,0);
#endif

    buttonBtoA = gtk_button_new_with_label ("A<B");
    gtk_widget_modify_bg(buttonBtoA, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonBtoA);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(buttonBtoA),49,20);
#else
    gtk_widget_set_size_request(GTK_WIDGET(buttonBtoA),45,25);
#endif
    g_signal_connect(G_OBJECT(buttonBtoA),"clicked",G_CALLBACK(vfoCallback),NULL);
    gtk_widget_show(buttonBtoA);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonBtoA,357,0);
#else
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonBtoA,405,0);
#endif

    buttonSplit = gtk_button_new_with_label ("Split");
    gtk_widget_modify_bg(buttonSplit, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonSplit);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(buttonSplit),50,20);
#else
    gtk_widget_set_size_request(GTK_WIDGET(buttonSplit),50,25);
#endif
    g_signal_connect(G_OBJECT(buttonSplit),"clicked",G_CALLBACK(vfoCallback),NULL);
    gtk_widget_show(buttonSplit);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonSplit,406,0);
#else
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonSplit,450,0);
#endif

    buttonFrequencyUp = gtk_button_new_with_label ("^");
    gtk_widget_modify_bg(buttonFrequencyUp, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonFrequencyUp);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(buttonFrequencyUp),30,20);
#else
    gtk_widget_set_size_request(GTK_WIDGET(buttonFrequencyUp),30,25);
#endif
    g_signal_connect(G_OBJECT(buttonFrequencyUp),"pressed",G_CALLBACK(frequencyUpCallback),NULL);
    g_signal_connect(G_OBJECT(buttonFrequencyUp),"released",G_CALLBACK(frequencyReleasedCallback),NULL);
    gtk_widget_show(buttonFrequencyUp);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonFrequencyUp,258,20);
#else
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonFrequencyUp,300,25);
#endif

    buttonIncrementPlus = gtk_button_new_with_label ("+");
    gtk_widget_modify_bg(buttonIncrementPlus, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonIncrementPlus);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(buttonIncrementPlus),20,20);
#else
    gtk_widget_set_size_request(GTK_WIDGET(buttonIncrementPlus),20,25);
#endif
    g_signal_connect(G_OBJECT(buttonIncrementPlus),"clicked",G_CALLBACK(buttonIncrementPlusCallback),NULL);
    gtk_widget_show(buttonIncrementPlus);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonIncrementPlus,288,20);
#else
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonIncrementPlus,330,25);
#endif

    incrementDisplay=gtk_drawing_area_new();
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(incrementDisplay),98,20);
#else
    gtk_widget_set_size_request(GTK_WIDGET(incrementDisplay),100,25);
#endif
    gtk_widget_show(incrementDisplay);
    g_signal_connect(G_OBJECT (incrementDisplay),"configure_event",G_CALLBACK(incrementDisplay_configure_event),NULL);
    g_signal_connect(G_OBJECT (incrementDisplay),"expose_event",G_CALLBACK(incrementDisplay_expose_event),NULL);
    g_signal_connect(G_OBJECT(incrementDisplay),"scroll_event",G_CALLBACK(increment_scroll_event),NULL);
    gtk_widget_set_events(incrementDisplay,GDK_EXPOSURE_MASK|GDK_SCROLL_MASK);
    gtk_widget_show(incrementDisplay);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)vfoFixed,incrementDisplay,308,20);
#else
    gtk_fixed_put((GtkFixed*)vfoFixed,incrementDisplay,350,25);
#endif

    buttonIncrementMinus = gtk_button_new_with_label ("-");
    gtk_widget_modify_bg(buttonIncrementMinus, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonIncrementMinus);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(buttonIncrementMinus),20,20);
#else
    gtk_widget_set_size_request(GTK_WIDGET(buttonIncrementMinus),20,25);
#endif
    g_signal_connect(G_OBJECT(buttonIncrementMinus),"clicked",G_CALLBACK(buttonIncrementMinusCallback),NULL);
    gtk_widget_show(buttonIncrementMinus);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonIncrementMinus,406,20);
#else
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonIncrementMinus,450,25);
#endif

    buttonFrequencyDown = gtk_button_new_with_label ("v");
    gtk_widget_modify_bg(buttonFrequencyDown, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonFrequencyDown);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(buttonFrequencyDown),30,20);
#else
    gtk_widget_set_size_request(GTK_WIDGET(buttonFrequencyDown),30,25);
#endif
    g_signal_connect(G_OBJECT(buttonFrequencyDown),"pressed",G_CALLBACK(frequencyDownCallback),NULL);
    g_signal_connect(G_OBJECT(buttonFrequencyDown),"released",G_CALLBACK(frequencyReleasedCallback),NULL);
    gtk_widget_show(buttonFrequencyDown);
#ifdef NETBOOK
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonFrequencyDown,426,20);
#else
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonFrequencyDown,470,25);
#endif

#ifdef NETBOOK
    gtk_widget_set_size_request(GTK_WIDGET(vfoFixed),714,40);
#else
    gtk_widget_set_size_request(GTK_WIDGET(vfoFixed),800,50);
#endif
    gtk_widget_show(vfoFixed);

    return vfoFixed;
  
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Save the VFO state
*/
void vfoSaveState() {
    char string[128];
    char name[128];

    sprintf(string,"%lld",frequencyA);
    setProperty("vfoA",string);
    sprintf(string,"%lld",frequencyALO);
    setProperty("vfoALO",string);
    sprintf(string,"%lld",dspAFrequency);
    setProperty("vfoDspAFrequency",string);
    sprintf(string,"%lld",ddsAFrequency);
    setProperty("vfoDdsAFrequency",string);

    sprintf(string,"%lld",frequencyB);
    setProperty("vfoB",string);
    sprintf(string,"%lld",frequencyBLO);
    setProperty("vfoBLO",string);
    sprintf(string,"%lld",dspBFrequency);
    setProperty("vfoDspBFrequency",string);
    sprintf(string,"%lld",ddsBFrequency);
    setProperty("vfoDdsBFrequency",string);

    sprintf(string,"%ld",frequencyIncrement);
    setProperty("frequencyIncrement",string);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Restore the VFO state
*/
void vfoRestoreState() {
    char* value;
    char name[128];
 
    value=getProperty("vfoA");
    if(value) frequencyA=atoll(value);
    value=getProperty("vfoALO");
    if(value) frequencyALO=atoll(value);
    value=getProperty("vfoDspAFrequency");
    if(value) dspAFrequency=atoll(value);
    value=getProperty("vfoDdsAFrequency");
    if(value) ddsAFrequency=atoll(value);
  
    value=getProperty("vfoB");
    if(value) frequencyB=atoll(value);
    value=getProperty("vfoBLO");
    if(value) frequencyBLO=atoll(value);
    value=getProperty("vfoDspBFrequency");
    if(value) dspBFrequency=atoll(value);
    value=getProperty("vfoDdsBFrequency");
    if(value) ddsBFrequency=atoll(value);

    value=getProperty("frequencyIncrement");
    if(value) setIncrement(atol(value));

    frequencyAChanged=1;
    frequencyBChanged=1;
}

/* --------------------------------------------------------------------------*/
/** 
* @brief transmit on VFO A frequency
*/
int vfoTransmit(gpointer data) {
    int state=*(int*)data;

//fprintf(stderr,"vfoTransmit: %d\n",state);

    free(data);

    if(bSplit) {
        bTransmitting=state;
        updateVfoBDisplay();
    } else {
        aTransmitting=state;
        updateVfoADisplay();
    }
    return 0;
}

void vfoSplit(int state) {
    bSplit=state;
    updateVfoBDisplay();
    setBFrequency(frequencyB);
}
