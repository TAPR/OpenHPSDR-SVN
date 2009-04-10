// meter.c

#include <gtk/gtk.h>
#include <gdk/gdkkeysyms.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

#include "main.h"
#include "mercury.h"
#include "meter.h"
#include "meter_update.h"
#include "property.h"
#include "soundcard.h"

GtkWidget* meterFixed;
GtkWidget* meter;
GdkPixmap* meterPixmap;
GtkWidget* dbm;
GdkPixmap* dbmPixmap;

GtkWidget* buttonSIGNAL;
GtkWidget* buttonSAV;

int meterDbm;

int meterMode=meterSIGNAL;

int meterPeak=0;
int meterPeakCount=0;
int meterPeakSamples=10;

int meterX=0;

void plotSignal(float* samples);
void drawSignal();
void updateOff();


float getFilterSizeCalibrationOffset() {
    int size=1024; // dspBufferSize
    float i=log10((float)size);
    return 3.0f*(11.0f-i);
}

//-------------------------------------------------------------------------------------------
//
//  callback when meter is created
//
gboolean meter_configure_event(GtkWidget* widget,GdkEventConfigure* event) {
    GdkGC* gc;

    if(meterPixmap) g_object_unref(meterPixmap);

    meterPixmap=gdk_pixmap_new(widget->window,widget->allocation.width,widget->allocation.height,-1);

    gc=gdk_gc_new(widget->window);
    gdk_gc_set_rgb_fg_color(gc,&black);
    gdk_draw_rectangle(meterPixmap,
                       gc,
                       TRUE,
                       0,0,
                       widget->allocation.width,
                       widget->allocation.height);

    g_object_unref(gc);

    return TRUE;
}

//-------------------------------------------------------------------------------------------
//
//  callback when meter is exposed - paint it from the pixmap
//
gboolean meter_expose_event(GtkWidget* widget,GdkEventExpose* event) {
    gdk_draw_drawable(widget->window,
                    widget->style->fg_gc[GTK_WIDGET_STATE (widget)],
                    meterPixmap,
                    event->area.x, event->area.y,
                    event->area.x, event->area.y,
                    event->area.width, event->area.height);
    return FALSE;
}

//-------------------------------------------------------------------------------------------
//
//  callback when dbm is is created
//
gboolean dbm_configure_event(GtkWidget* widget,GdkEventConfigure* event) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;

    if(dbmPixmap) g_object_unref(dbmPixmap);

    dbmPixmap=gdk_pixmap_new(widget->window,widget->allocation.width,widget->allocation.height,-1);

    gc=gdk_gc_new(widget->window);
    gdk_gc_set_rgb_fg_color(gc,&black);
    gdk_draw_rectangle(dbmPixmap,
                       gc,
                       TRUE,
                       0,0,
                       widget->allocation.width,
                       widget->allocation.height);

    context = gdk_pango_context_get_for_screen (gdk_screen_get_default ());
    layout = pango_layout_new (context);
    pango_layout_set_width(layout,170*PANGO_SCALE);
    pango_layout_set_alignment(layout,PANGO_ALIGN_RIGHT);
    pango_layout_set_markup (layout, "<span foreground='#7AAA6E' background='#000000' font_desc='Sans Bold 12'>dBm    </span>", -1);
    gdk_draw_layout(GDK_DRAWABLE(dbmPixmap),gc,0,0,layout);

    g_object_unref(context);
    g_object_unref(layout);
    g_object_unref(gc);

    return TRUE;
}

//-------------------------------------------------------------------------------------------
//
//  callback when dbm is exposed - need to paint it from the pixmap
//
gboolean dbm_expose_event(GtkWidget* widget,GdkEventExpose* event) {
    gdk_draw_drawable(widget->window,
                    widget->style->fg_gc[GTK_WIDGET_STATE (widget)],
                    dbmPixmap,
                    event->area.x, event->area.y,
                    event->area.x, event->area.y,
                    event->area.width, event->area.height);
    return FALSE;
}

void meterDrawSignal() {

    // get the meter context - just copy the window GC and modify
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];
    int i;
    if(meter->window) {

        gc=gdk_gc_new(meter->window);
        gdk_gc_copy(gc,meter->style->black_gc);

        // black background
        gdk_draw_rectangle(meterPixmap,gc,TRUE,0,0,meter->allocation.width,meter->allocation.height);

        // draw the meter
        gdk_gc_set_rgb_fg_color(gc,&green);
        gdk_draw_rectangle(meterPixmap,gc,TRUE,43,0,meterX,meter->allocation.height/2);

        // draw the S-Unit markers
        gdk_gc_set_rgb_fg_color(gc,&black);
        for(i=6;i<=54;i+=6) {
            gdk_draw_line(meterPixmap,gc,i+43,0,i+43,meter->allocation.height/2); // 1 -- 9
        }
        for(i=64;i<120;i+=10) {
            gdk_draw_line(meterPixmap,gc,i+43,0,i+43,meter->allocation.height/2); // +10 +60
        }

        // draw the levels
        context = gdk_pango_context_get_for_screen(gdk_screen_get_default ());
        layout = pango_layout_new(context);
        pango_layout_set_width(layout,120*PANGO_SCALE);
        pango_layout_set_alignment(layout,PANGO_ALIGN_LEFT);
        sprintf(temp,"<span foreground='#7AAA6E' background='#000000' font_desc='Sans 5'>  1    3    5    7    9    +20   +40   +60</span>",meterDbm);
        pango_layout_set_markup(layout,temp,-1);
        gdk_draw_layout(GDK_DRAWABLE(meterPixmap),gc,43,20,layout);

        // update the peak
        if(meterX>meterPeak) {
            meterPeak=meterX;
            meterPeakCount=0;
        }
        if(meterPeakCount++ >= meterPeakSamples) {
            meterPeak=meterX;
            meterPeakCount=0;
        }
        gdk_gc_set_rgb_fg_color(gc,&red);
        gdk_draw_line(meterPixmap,gc,43+meterPeak,0,43+meterPeak,meter->allocation.height/2);
        gdk_draw_line(meterPixmap,gc,43+meterPeak-1,0,43+meterPeak-1,meter->allocation.height/2);

        g_object_unref(context);
        g_object_unref(layout);
        g_object_unref(gc);

        gtk_widget_queue_draw(meter);
    }
}

void meterDbmDrawSignal() {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];
    if(dbm->window) {

        gc=gdk_gc_new(dbm->window);
        gdk_gc_set_rgb_fg_color(gc,&black);
        gdk_draw_rectangle(dbmPixmap,
                           gc,
                           TRUE,
                           0,0,
                           dbm->allocation.width,
                           dbm->allocation.height);

        context = gdk_pango_context_get_for_screen(gdk_screen_get_default ());
        layout = pango_layout_new(context);
        pango_layout_set_width(layout,120*PANGO_SCALE);
        pango_layout_set_alignment(layout,PANGO_ALIGN_RIGHT);
        sprintf(temp,"<span foreground='#7AAA6E' background='#000000' font_desc='Sans Bold 12'>%d dBm    </span>",meterDbm);
        pango_layout_set_markup(layout,temp,-1);
        gdk_draw_layout(GDK_DRAWABLE(dbmPixmap),gc,43,0,layout);

        g_object_unref(context);
        g_object_unref(layout);
        g_object_unref(gc);

        gtk_widget_queue_draw(dbm);
    }
}

void meterPlotSignal(float sample) {

    // plot the meter
    float val=sample;
    val+=multimeterCalibrationOffset + preampOffset + getFilterSizeCalibrationOffset();

    meterDbm=(int)val;

    meterX=0;

    if(val <= -97.0f)
        meterX = (int)(0+(val+100.0)/3.0*10);
    else if(val <= -91.0f)
        meterX = (int)(10+(val+97.0)/6.0*17);
    else if(val <= -85.0f)
        meterX = (int)(27+(val+91.0)/6.0*16);
    else if(val <= -79.0f)
        meterX = (int)(43+(val+85.0)/6.0*17);
    else if(val <= -73.0f)
        meterX = (int)(60+(val+79.0)/6.0*16);
    else if(val <= -53.0f)
        meterX = (int)(76+(val+73.0)/20.0*24);
    else if(val <= -33.0f)
        meterX = (int)(100+(val+53.0)/20.0*24);
    else if(val <= -13.0f)
        meterX = (int)(124+(val+33.0)/20.0*24);
    else
        meterX = (int)(148 + (val+13.0)/20.0*19);
    if(meterX<=0) meterX=1;
}

void meterUpdateOff() {

    // get the meter context - just copy the window GC and modify
    GdkGC* gc;
    if(meter->window) {
        gc=gdk_gc_new(meter->window);
        gdk_gc_copy(gc,meter->style->black_gc);
        gdk_draw_rectangle(meterPixmap,gc,TRUE,0,0,meter->allocation.width,meter->allocation.height);

        // update the meter
        gtk_widget_queue_draw_area(meter,0,0,meter->allocation.width,meter->allocation.height);
    }
}

void updateMeter(float sample) {
    switch(meterMode) {
        case meterSIGNAL:
            meterPlotSignal(sample);
            meterDrawSignal();
            meterDbmDrawSignal();
            break;
        case meterOFF:
            meterUpdateOff();
            break;
    }
}

void setMeterMode(int mode) {
    char command[80];
    meterMode=mode;

    switch(meterMode) {
        case meterSIGNAL:
            sprintf(command,"setMeterType 0");
            writeCommand(command);
            break;
        case meterOFF:
            break;
    }
}


GtkWidget* buildMeterUI() {
   meterFixed=gtk_fixed_new();
    gtk_widget_modify_bg(meterFixed,GTK_STATE_NORMAL,&background);

    // meter
    meter=gtk_drawing_area_new();
    gtk_widget_set_size_request(GTK_WIDGET(meter),200,30);
    g_signal_connect(G_OBJECT (meter),"configure_event",G_CALLBACK(meter_configure_event),NULL);
    g_signal_connect(G_OBJECT (meter),"expose_event",G_CALLBACK(meter_expose_event),NULL);
    gtk_widget_show(meter);
    gtk_fixed_put((GtkFixed*)meterFixed,meter,0,0);

    // dbm
    dbm=gtk_drawing_area_new();
    gtk_widget_set_size_request(GTK_WIDGET(dbm),200,20);
    g_signal_connect(G_OBJECT (dbm),"configure_event",G_CALLBACK(dbm_configure_event),NULL);
    g_signal_connect(G_OBJECT (dbm),"expose_event",G_CALLBACK(dbm_expose_event),NULL);
    gtk_widget_show(dbm);
    gtk_fixed_put((GtkFixed*)meterFixed,dbm,0,30);

    gtk_widget_set_size_request(GTK_WIDGET(meterFixed),200,60);
    gtk_widget_show(meterFixed);

    return meterFixed;
}

void meterSaveState() {
    char string[128];
    sprintf(string,"%d",meterMode);
    setProperty("meter.mode",string);
}

void meterRestoreState() {
    char* value;
    value=getProperty("meter.mode");
    if(value) meterMode=atoi(value);
}


