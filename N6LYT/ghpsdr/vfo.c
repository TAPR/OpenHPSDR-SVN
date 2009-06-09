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

#include "command.h"
#include "main.h"
#include "band.h"
#include "property.h"
#include "vfo.h"

GtkWidget* vfoFixed;

long long frequencyA=7100000LL;
long long frequencyB=7100000LL;

long long frequencyIF=0LL;

long long dspFrequency;
long long ddsFrequency;
long long ifFrequency;

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

long frequencyIncrement=100;
GtkWidget* buttonFrequencyUp;
GtkWidget* buttonIncrementPlus;
GtkWidget* incrementDisplay;
GdkPixmap* incrementPixmap;
GtkWidget* buttonIncrementMinus;
GtkWidget* buttonFrequencyDown;

void setIncrement(int increment);
void vfoSetRxFrequency();

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
    sprintf(temp,"<span foreground='#00FF00' background='#2C2C2C' font_desc='Sans Bold 24'>% 7lld.%03lld.%03lld </span>",frequencyA/1000000LL,(frequencyA%1000000LL)/1000LL,frequencyA%1000LL);
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
    sprintf(temp,"<span foreground='#C0C0C0' background='#2C2C2C' font_desc='Sans Bold 24'>% 7lld.%03lld.%03lld </span>",frequencyB/1000000LL,(frequencyB%1000000LL)/1000LL,frequencyB%1000LL);
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
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    dspFrequency=0;
    ddsFrequency=0;
    frequencyA=f;

    dspFrequency=0;
    ddsFrequency=f-frequencyIF;

    vfoSetRxFrequency();

//fprintf(stderr,"f=%ld LO=%ld %s\n",f,frequencyLO,temp);

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
        sprintf(temp,"<span foreground='#00FF00' background='#2C2C2C' font_desc='Sans Bold 24'>% 7lld.%03lld.%03lld </span>",frequencyA/1000000LL,(frequencyA%1000000LL)/1000LL,frequencyA%1000);
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

    // check the band
    int thisBand=getBand(f);
    if(band!=thisBand) {
        forceBand(thisBand);
    }

}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set VFO RX frequency
*/
void vfoSetRxFrequency() {
    fprintf(stderr,"setFrequency %lld\n",ddsFrequency);
    setFrequency((float)ddsFrequency/1000000.0f);
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

    frequencyB=f;
  
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
        sprintf(temp,"<span foreground='#C0C0C0' background='#2C2C2C' font_desc='Sans Bold 24'>% 7lld.%03lld.%03lld </span>",frequencyB/1000000LL,(frequencyB%1000000LL)/1000LL,frequencyB%1000LL);
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
* @brief  Callback when a vfo button is pressed
* 
* @param widget
* @param data
*/
void vfoCallback(GtkWidget* widget,gpointer data) {
    long long f;
    if(widget==buttonAtoB) {
        setBFrequency(frequencyA);
    } else if(widget==buttonBtoA) {
        setAFrequency(frequencyB);
    } else if(widget==buttonAswapB) {
        f=frequencyA;
        setAFrequency(frequencyB);
        setBFrequency(f);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Increment the frequency - only vfo A
* 
* @param increment
*/
void vfoIncrementFrequency(long increment) {
    setAFrequency(frequencyA+(long long)increment);
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
    vfoIncrementFrequency(frequencyIncrement);
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
    vfoIncrementFrequency(frequencyIncrement);
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
    vfoIncrementFrequency(-frequencyIncrement);
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
    vfoIncrementFrequency(-frequencyIncrement);
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
    if(event->direction==GDK_SCROLL_UP) {
        vfoIncrementFrequency(frequencyIncrement);
    } else {
        vfoIncrementFrequency(-frequencyIncrement);
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
    if(frequencyIncrement==1000000) {
        frequencyIncrement=1;
    } else {
        frequencyIncrement=frequencyIncrement*10;
    }
    drawIncrementDisplay(TRUE);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Previous increment
*/
void previousIncrement() {
    if(frequencyIncrement==1) {
        frequencyIncrement=1000000;
    } else {
        frequencyIncrement=frequencyIncrement/10;
    }
    drawIncrementDisplay(TRUE);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Set IF frequency
*/
void setIFFrequency(long long freq) {
    frequencyIF=freq;
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
    
    gtk_widget_set_size_request(GTK_WIDGET(vfoAFrame),300,50);
    vfoAFrequency=gtk_drawing_area_new();
    gtk_widget_set_size_request(GTK_WIDGET(vfoAFrequency),250,35);
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
    gtk_widget_set_size_request(GTK_WIDGET(vfoBFrame),300,50);
    vfoBFrequency=gtk_drawing_area_new();
    gtk_widget_set_size_request(GTK_WIDGET(vfoBFrequency),300,35);
    g_signal_connect(G_OBJECT (vfoBFrequency),"configure_event",G_CALLBACK(vfoBFrequency_configure_event),NULL);
    g_signal_connect(G_OBJECT (vfoBFrequency),"expose_event",G_CALLBACK(vfoBFrequency_expose_event),NULL);
    gtk_widget_show(vfoBFrequency);
    gtk_widget_show(vfoBFrame);
    gtk_container_add((GtkContainer*)vfoBFrame,vfoBFrequency);
    gtk_fixed_put((GtkFixed*)vfoFixed,vfoBFrame,500,0);

    // vfo control
    vfoControlFrame=gtk_frame_new(NULL);

    buttonAtoB = gtk_button_new_with_label ("A>B");
    gtk_widget_modify_bg(buttonAtoB, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonAtoB);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonAtoB),60,25);
    g_signal_connect(G_OBJECT(buttonAtoB),"clicked",G_CALLBACK(vfoCallback),NULL);
    gtk_widget_show(buttonAtoB);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonAtoB,300,0);

    buttonAswapB = gtk_button_new_with_label ("A<>B");
    gtk_widget_modify_bg(buttonAswapB, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonAswapB);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonAswapB),80,25);
    g_signal_connect(G_OBJECT(buttonAswapB),"clicked",G_CALLBACK(vfoCallback),NULL);
    gtk_widget_show(buttonAswapB);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonAswapB,360,0);

    buttonBtoA = gtk_button_new_with_label ("A<B");
    gtk_widget_modify_bg(buttonBtoA, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonBtoA);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonBtoA),60,25);
    g_signal_connect(G_OBJECT(buttonBtoA),"clicked",G_CALLBACK(vfoCallback),NULL);
    gtk_widget_show(buttonBtoA);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonBtoA,440,0);

    buttonFrequencyUp = gtk_button_new_with_label ("^");
    gtk_widget_modify_bg(buttonFrequencyUp, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonFrequencyUp);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonFrequencyUp),30,25);
    g_signal_connect(G_OBJECT(buttonFrequencyUp),"pressed",G_CALLBACK(frequencyUpCallback),NULL);
    g_signal_connect(G_OBJECT(buttonFrequencyUp),"released",G_CALLBACK(frequencyReleasedCallback),NULL);
    gtk_widget_show(buttonFrequencyUp);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonFrequencyUp,300,25);

    buttonIncrementPlus = gtk_button_new_with_label ("+");
    gtk_widget_modify_bg(buttonIncrementPlus, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonIncrementPlus);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonIncrementPlus),20,25);
    g_signal_connect(G_OBJECT(buttonIncrementPlus),"clicked",G_CALLBACK(buttonIncrementPlusCallback),NULL);
    gtk_widget_show(buttonIncrementPlus);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonIncrementPlus,330,25);

    incrementDisplay=gtk_drawing_area_new();
    gtk_widget_set_size_request(GTK_WIDGET(incrementDisplay),100,25);
    gtk_widget_show(incrementDisplay);
    g_signal_connect(G_OBJECT (incrementDisplay),"configure_event",G_CALLBACK(incrementDisplay_configure_event),NULL);
    g_signal_connect(G_OBJECT (incrementDisplay),"expose_event",G_CALLBACK(incrementDisplay_expose_event),NULL);
    g_signal_connect(G_OBJECT(incrementDisplay),"scroll_event",G_CALLBACK(increment_scroll_event),NULL);
    gtk_widget_set_events(incrementDisplay,GDK_EXPOSURE_MASK|GDK_SCROLL_MASK);
    gtk_widget_show(incrementDisplay);
    gtk_fixed_put((GtkFixed*)vfoFixed,incrementDisplay,350,25);

    buttonIncrementMinus = gtk_button_new_with_label ("-");
    gtk_widget_modify_bg(buttonIncrementMinus, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonIncrementMinus);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonIncrementMinus),20,25);
    g_signal_connect(G_OBJECT(buttonIncrementMinus),"clicked",G_CALLBACK(buttonIncrementMinusCallback),NULL);
    gtk_widget_show(buttonIncrementMinus);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonIncrementMinus,450,25);

    buttonFrequencyDown = gtk_button_new_with_label ("v");
    gtk_widget_modify_bg(buttonFrequencyDown, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonFrequencyDown);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonFrequencyDown),30,25);
    g_signal_connect(G_OBJECT(buttonFrequencyDown),"pressed",G_CALLBACK(frequencyDownCallback),NULL);
    g_signal_connect(G_OBJECT(buttonFrequencyDown),"released",G_CALLBACK(frequencyReleasedCallback),NULL);
    gtk_widget_show(buttonFrequencyDown);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonFrequencyDown,470,25);

    gtk_widget_set_size_request(GTK_WIDGET(vfoFixed),800,50);
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

    sprintf(string,"%lld",frequencyB);
    setProperty("vfoB",string);
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
  
    value=getProperty("vfoB");
    if(value) frequencyB=atoll(value);
}
