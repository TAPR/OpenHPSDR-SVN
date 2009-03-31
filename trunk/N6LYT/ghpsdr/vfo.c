// vfo.c

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

long long dspFrequency;
long long ddsFrequency;
long long ifFrequency;

#define OFFSET 0

gint vfoTimerId;

GtkWidget* vfoAFrequency;
GdkPixmap* vfoAPixmap;
GtkWidget* vfoBFrequency;
GdkPixmap* vfoBPixmap;

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

//-------------------------------------------------------------------------------------------
//
//  callback when vfo A is is created
//
gboolean vfoAFrequency_configure_event(GtkWidget* widget,GdkEventConfigure* event) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    if(vfoAPixmap) g_object_unref(vfoAPixmap);

    vfoAPixmap=gdk_pixmap_new(widget->window,widget->allocation.width,widget->allocation.height,-1);

    gc=gdk_gc_new(widget->window);
    gdk_gc_set_rgb_fg_color(gc,&black);
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
    sprintf(temp,"<span foreground='#7AAA6E' background='#000000' font_desc='Sans Bold 14'>%0lld.%03lld.%03lld A</span>",frequencyA/1000000LL,(frequencyA%1000000LL)/1000LL,frequencyA%1000LL);
    pango_layout_set_markup(layout,temp,-1);
    gdk_draw_layout(GDK_DRAWABLE(vfoAPixmap),gc,0,0,layout);

    g_object_unref(context);
    g_object_unref(layout);
    g_object_unref(gc);

    return TRUE;
}

//-------------------------------------------------------------------------------------------
//
//  callback when vfo A is is exposed - need to paint it from the pixmap
//
gboolean vfoAFrequency_expose_event(GtkWidget* widget,GdkEventExpose* event) {
    gdk_draw_drawable(widget->window,
                    widget->style->fg_gc[GTK_WIDGET_STATE (widget)],
                    vfoAPixmap,
                    event->area.x, event->area.y,
                    event->area.x, event->area.y,
                    event->area.width, event->area.height);
    return FALSE;
}

//-------------------------------------------------------------------------------------------
//
//  callback when vfo B is created
//
gboolean vfoBFrequency_configure_event(GtkWidget* widget,GdkEventConfigure* event) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    if(vfoBPixmap) g_object_unref(vfoBPixmap);

    vfoBPixmap=gdk_pixmap_new(widget->window,widget->allocation.width,widget->allocation.height,-1);

    gc=gdk_gc_new(widget->window);
    gdk_gc_set_rgb_fg_color(gc,&black);
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
    sprintf(temp,"<span foreground='#707070' background='#000000' font_desc='Sans Bold 14'>%0lld.%03lld.%03lld B</span>",frequencyB/1000000LL,(frequencyB%1000000LL)/1000LL,frequencyB%1000LL);
    pango_layout_set_markup(layout,temp,-1);
    gdk_draw_layout(GDK_DRAWABLE(vfoBPixmap),gc,0,0,layout);

    g_object_unref(context);
    g_object_unref(layout);
    g_object_unref(gc);

    return TRUE;
}

//-------------------------------------------------------------------------------------------
//
//  callback when vfo B is exposed - need to paint it from the pixmap
//
gboolean vfoBFrequency_expose_event(GtkWidget* widget,GdkEventExpose* event) {
    gdk_draw_drawable(widget->window,
                    widget->style->fg_gc[GTK_WIDGET_STATE (widget)],
                    vfoBPixmap,
                    event->area.x, event->area.y,
                    event->area.x, event->area.y,
                    event->area.width, event->area.height);
    return FALSE;
}

//-------------------------------------------------------------------------------------------
//
//  set the A vfo frequency
//
void setAFrequency(long long f) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    dspFrequency=0;
    ddsFrequency=0;
    frequencyA=f;

    dspFrequency=0;
    ddsFrequency=f;

    vfoSetRxFrequency();

//fprintf(stderr,"f=%ld LO=%ld %s\n",f,frequencyLO,temp);

    if(vfoAFrequency->window) {
        gc=gdk_gc_new(vfoAFrequency->window);
        gdk_gc_set_rgb_fg_color(gc,&black);
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
        sprintf(temp,"<span foreground='#7AAA6E' background='#000000' font_desc='Sans Bold 14'>%0lld.%03lld.%03lld A</span>",frequencyA/1000000LL,(frequencyA%1000000LL)/1000LL,frequencyA%1000);
        pango_layout_set_markup(layout,temp,-1);
        gdk_draw_layout(GDK_DRAWABLE(vfoAPixmap),gc,0,0,layout);

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

void vfoSetRxFrequency() {
    //fprintf(stderr,"setFrequency %lld\n",ddsFrequency);
    setFrequency((float)ddsFrequency/1000000.0f);
}

//-------------------------------------------------------------------------------------------
//
//  set the B vfo frequency
//
void setBFrequency(long long f) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    frequencyB=f;
  
    if(vfoBFrequency->window) {
        gc=gdk_gc_new(vfoBFrequency->window);
        gdk_gc_set_rgb_fg_color(gc,&black);
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
        sprintf(temp,"<span foreground='#707070' background='#000000' font_desc='Sans Bold 14'>%0lld.%03lld.%03lld B</span>",frequencyB/1000000LL,(frequencyB%1000000LL)/1000LL,frequencyB%1000LL);
        pango_layout_set_markup(layout,temp,-1);
        gdk_draw_layout(GDK_DRAWABLE(vfoBPixmap),gc,0,0,layout);

        g_object_unref(context);
        g_object_unref(layout);
        g_object_unref(gc);

        gtk_widget_queue_draw(vfoBFrequency);
    }
}

//-------------------------------------------------------------------------------------------
//
//  callback when a vfo button is pressed
//
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

//-------------------------------------------------------------------------------------------
//
// increment the frequency - only vfo A
//
void vfoIncrementFrequency(long increment) {
    setAFrequency(frequencyA+(long long)increment);
}

//-------------------------------------------------------------------------------------------
//
//  callback when a frequencyUp button is pressed
//
gint frequencyUpTimer(gpointer data) {
    gtk_timeout_remove(vfoTimerId);
    vfoIncrementFrequency(frequencyIncrement);
    vfoTimerId=gtk_timeout_add(50,frequencyUpTimer,NULL);
    return TRUE;
}

void frequencyUpCallback(GtkWidget* widget,gpointer data) {
    vfoIncrementFrequency(frequencyIncrement);
    vfoTimerId=gtk_timeout_add(500,frequencyUpTimer,NULL);
}

//-------------------------------------------------------------------------------------------
//
//  callback when a frequencyDown button is pressed
//
gint frequencyDownTimer(gpointer data) {
    gtk_timeout_remove(vfoTimerId);
    vfoIncrementFrequency(-frequencyIncrement);
    vfoTimerId=gtk_timeout_add(50,frequencyDownTimer,NULL);
    return TRUE;
}

void frequencyDownCallback(GtkWidget* widget,gpointer data) {
    vfoIncrementFrequency(-frequencyIncrement);
    vfoTimerId=gtk_timeout_add(500,frequencyDownTimer,NULL);
}


//-------------------------------------------------------------------------------------------
//
//  callback when a frequencyUp/Down button is released
//
void frequencyReleasedCallback(GtkWidget* widget,gpointer data) {
    gtk_timeout_remove(vfoTimerId);
}

//-------------------------------------------------------------------------------------------
//
// frequency scroll wheel
//
gboolean frequency_scroll_event(GtkWidget* widget,GdkEventScroll* event) {
    if(event->direction==GDK_SCROLL_UP) {
        vfoIncrementFrequency(frequencyIncrement);
    } else {
        vfoIncrementFrequency(-frequencyIncrement);
    }
}

//-------------------------------------------------------------------------------------------
//
//  callback when a incrementDisplay is exposed
//
gboolean incrementDisplay_expose_event(GtkWidget* widget,GdkEventExpose* event) {
    gdk_draw_drawable(widget->window,
                    widget->style->fg_gc[GTK_WIDGET_STATE (widget)],
                    incrementPixmap,
                    event->area.x, event->area.y,
                    event->area.x, event->area.y,
                    event->area.width, event->area.height);
    return FALSE;
}

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
        sprintf(temp,"<span foreground='#7AAA6E' background='#000000' font_desc='Sans Bold 10'>%0ld</span>",frequencyIncrement);
        pango_layout_set_markup(layout,temp,-1);
        gdk_draw_layout(GDK_DRAWABLE(incrementPixmap),gc,0,0,layout);
    
        g_object_unref(context);
        g_object_unref(layout);
        g_object_unref(gc);

        if(queue) gtk_widget_queue_draw(incrementDisplay);
    }
}

gboolean incrementDisplay_configure_event(GtkWidget* widget,GdkEventConfigure* event) {
    drawIncrementDisplay(FALSE);
    return TRUE;
}

//-------------------------------------------------------------------------------------------
//
// increment up/down
//

void setIncrement(int increment) {
    frequencyIncrement=increment;
    drawIncrementDisplay(TRUE);
}

void nextIncrement() {
    if(frequencyIncrement==1000000) {
        frequencyIncrement=1;
    } else {
        frequencyIncrement=frequencyIncrement*10;
    }
    drawIncrementDisplay(TRUE);
}

void previousIncrement() {
    if(frequencyIncrement==1) {
        frequencyIncrement=1000000;
    } else {
        frequencyIncrement=frequencyIncrement/10;
    }
    drawIncrementDisplay(TRUE);
}

void buttonIncrementPlusCallback(GtkWidget* widget,gpointer data) {
    nextIncrement();
}

void buttonIncrementMinusCallback(GtkWidget* widget,gpointer data) {
    previousIncrement();
}
//-------------------------------------------------------------------------------------------
//
// increment scroll wheel
//
gboolean increment_scroll_event(GtkWidget* widget,GdkEventScroll* event) {
    if(event->direction==GDK_SCROLL_UP) {
        nextIncrement();
    } else {
        previousIncrement();
    }
    return TRUE;
}

//-------------------------------------------------------------------------------------------
//
// build the GUI
//
GtkWidget* buildVfoUI() {
    GtkWidget* label;

    vfoFixed=gtk_fixed_new();
    gtk_widget_modify_bg(vfoFixed,GTK_STATE_NORMAL,&background);

    // vfoA
    vfoAFrequency=gtk_drawing_area_new();
    gtk_widget_set_size_request(GTK_WIDGET(vfoAFrequency),200,20);
    g_signal_connect(G_OBJECT (vfoAFrequency),"configure_event",G_CALLBACK(vfoAFrequency_configure_event),NULL);
    g_signal_connect(G_OBJECT (vfoAFrequency),"expose_event",G_CALLBACK(vfoAFrequency_expose_event),NULL);
    g_signal_connect(G_OBJECT(vfoAFrequency),"scroll_event",G_CALLBACK(frequency_scroll_event),NULL);
    gtk_widget_set_events(vfoAFrequency,GDK_EXPOSURE_MASK|GDK_SCROLL_MASK);
    gtk_widget_show(vfoAFrequency);
    gtk_fixed_put((GtkFixed*)vfoFixed,vfoAFrequency,0,0);

    // vfoB
    vfoBFrequency=gtk_drawing_area_new();
    gtk_widget_set_size_request(GTK_WIDGET(vfoBFrequency),200,20);
    g_signal_connect(G_OBJECT (vfoBFrequency),"configure_event",G_CALLBACK(vfoBFrequency_configure_event),NULL);
    g_signal_connect(G_OBJECT (vfoBFrequency),"expose_event",G_CALLBACK(vfoBFrequency_expose_event),NULL);
    gtk_widget_show(vfoBFrequency);
    gtk_fixed_put((GtkFixed*)vfoFixed,vfoBFrequency,0,20);

    // vfo control
    buttonAtoB = gtk_button_new_with_label ("A>B");
    gtk_widget_modify_bg(buttonAtoB, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonAtoB);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonAtoB),60,25);
    g_signal_connect(G_OBJECT(buttonAtoB),"clicked",G_CALLBACK(vfoCallback),NULL);
    gtk_widget_show(buttonAtoB);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonAtoB,0,42);

    buttonAswapB = gtk_button_new_with_label ("A<>B");
    gtk_widget_modify_bg(buttonAswapB, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonAswapB);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonAswapB),80,25);
    g_signal_connect(G_OBJECT(buttonAswapB),"clicked",G_CALLBACK(vfoCallback),NULL);
    gtk_widget_show(buttonAswapB);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonAswapB,60,42);

    buttonBtoA = gtk_button_new_with_label ("A<B");
    gtk_widget_modify_bg(buttonBtoA, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonBtoA);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonBtoA),60,25);
    g_signal_connect(G_OBJECT(buttonBtoA),"clicked",G_CALLBACK(vfoCallback),NULL);
    gtk_widget_show(buttonBtoA);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonBtoA,140,42);

    buttonFrequencyUp = gtk_button_new_with_label ("^");
    gtk_widget_modify_bg(buttonFrequencyUp, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonFrequencyUp);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonFrequencyUp),30,25);
    g_signal_connect(G_OBJECT(buttonFrequencyUp),"pressed",G_CALLBACK(frequencyUpCallback),NULL);
    g_signal_connect(G_OBJECT(buttonFrequencyUp),"released",G_CALLBACK(frequencyReleasedCallback),NULL);
    gtk_widget_show(buttonFrequencyUp);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonFrequencyUp,0,70);

    buttonIncrementPlus = gtk_button_new_with_label ("+");
    gtk_widget_modify_bg(buttonIncrementPlus, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonIncrementPlus);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonIncrementPlus),20,25);
    g_signal_connect(G_OBJECT(buttonIncrementPlus),"clicked",G_CALLBACK(buttonIncrementPlusCallback),NULL);
    gtk_widget_show(buttonIncrementPlus);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonIncrementPlus,30,70);

    incrementDisplay=gtk_drawing_area_new();
    gtk_widget_set_size_request(GTK_WIDGET(incrementDisplay),100,25);
    gtk_widget_show(incrementDisplay);
    g_signal_connect(G_OBJECT (incrementDisplay),"configure_event",G_CALLBACK(incrementDisplay_configure_event),NULL);
    g_signal_connect(G_OBJECT (incrementDisplay),"expose_event",G_CALLBACK(incrementDisplay_expose_event),NULL);
    g_signal_connect(G_OBJECT(incrementDisplay),"scroll_event",G_CALLBACK(increment_scroll_event),NULL);
    gtk_widget_set_events(incrementDisplay,GDK_EXPOSURE_MASK|GDK_SCROLL_MASK);
    gtk_widget_show(incrementDisplay);
    gtk_fixed_put((GtkFixed*)vfoFixed,incrementDisplay,50,70);

    buttonIncrementMinus = gtk_button_new_with_label ("-");
    gtk_widget_modify_bg(buttonIncrementMinus, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonIncrementMinus);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonIncrementMinus),20,25);
    g_signal_connect(G_OBJECT(buttonIncrementMinus),"clicked",G_CALLBACK(buttonIncrementMinusCallback),NULL);
    gtk_widget_show(buttonIncrementMinus);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonIncrementMinus,150,70);

    buttonFrequencyDown = gtk_button_new_with_label ("v");
    gtk_widget_modify_bg(buttonFrequencyDown, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonFrequencyDown);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonFrequencyDown),30,25);
    g_signal_connect(G_OBJECT(buttonFrequencyDown),"pressed",G_CALLBACK(frequencyDownCallback),NULL);
    g_signal_connect(G_OBJECT(buttonFrequencyDown),"released",G_CALLBACK(frequencyReleasedCallback),NULL);
    gtk_widget_show(buttonFrequencyDown);
    gtk_fixed_put((GtkFixed*)vfoFixed,buttonFrequencyDown,170,70);

    gtk_widget_set_size_request(GTK_WIDGET(vfoFixed),185,95);
    gtk_widget_show(vfoFixed);

    return vfoFixed;
  
}

void vfoSaveState() {
    char string[128];
    char name[128];

    sprintf(string,"%lld",frequencyA);
    setProperty("vfoA",string);

    sprintf(string,"%lld",frequencyB);
    setProperty("vfoB",string);
}

void vfoRestoreState() {
    char* value;
    char name[128];
 
    value=getProperty("vfoA");
    if(value) frequencyA=atoll(value);
  
    value=getProperty("vfoB");
    if(value) frequencyB=atoll(value);
}
