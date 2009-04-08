#include <gtk/gtk.h>
#include <math.h>
#include <stdio.h>
#include "bandstack.h"
#include "command.h"
#include "cw.h"
#include "filter.h"
#include "main.h"
#include "property.h"
#include "mode.h"

int filterLow;
int filterHigh;

int txFilterLowCut=300;
int txFilterHighCut=3000;

int filterVar1Low;
int filterVar1High;
int filterVar2Low;
int filterVar2High;

gint filterTimerId;

int filter;

GtkWidget* filterFixed;

GtkWidget* buttonHighPlus;
GtkWidget* filterHighLabel;
GdkPixmap* filterHighLabelPixmap;
GtkWidget* filterHighDisplay;
GdkPixmap* filterHighDisplayPixmap;
GtkWidget* buttonHighMinus;
GtkWidget* buttonLowPlus;
GtkWidget* filterLowLabel;
GdkPixmap* filterLowLabelPixmap;
GtkWidget* filterLowDisplay;
GdkPixmap* filterLowDisplayPixmap;
GtkWidget* buttonLowMinus;
GtkWidget* buttonVar1;
GtkWidget* buttonVar2;

GtkWidget* button6000;
GtkWidget* button4000;
GtkWidget* button2600;
GtkWidget* button2100;
GtkWidget* button1000;
GtkWidget* button500;
GtkWidget* button250;
GtkWidget* button100;
GtkWidget* button50;
GtkWidget* button25;
GtkWidget* currentFilterButton=NULL;

gint filterRootX=0;
gint filterRootY=0;

int max(int a,int b) {
   if(a>b) return a;
   return b;
}

int min(int a,int b) {
   if(a<b) return a;
   return b;
}

void drawFilterLow(gboolean queue) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];
    if(filterLowDisplay) {
        if(filterLowDisplayPixmap) g_object_unref(filterLowDisplayPixmap);

        filterLowDisplayPixmap=gdk_pixmap_new(filterLowDisplay->window,filterLowDisplay->allocation.width,filterLowDisplay->allocation.height,-1);

        gc=gdk_gc_new(filterLowDisplay->window);
        gdk_gc_set_rgb_fg_color(gc,&black);
        gdk_draw_rectangle(filterLowDisplayPixmap,
                           gc,
                           TRUE,
                           0,0,
                           filterLowDisplay->allocation.width,
                           filterLowDisplay->allocation.height);

        context = gdk_pango_context_get_for_screen (gdk_screen_get_default ());
        layout = pango_layout_new (context);
        pango_layout_set_width(layout,filterLowDisplay->allocation.width*PANGO_SCALE);
        pango_layout_set_alignment(layout,PANGO_ALIGN_RIGHT);
        sprintf(temp,"<span foreground='#7AAA6E' background='#000000' font_desc='Sans Bold 10'>%d</span>",filterLow);
        pango_layout_set_markup (layout,temp, -1);
        gdk_draw_layout(GDK_DRAWABLE(filterLowDisplayPixmap),gc,0,2,layout);

        g_object_unref(context);
        g_object_unref(layout);
        g_object_unref(gc);

        if(queue) gtk_widget_queue_draw(filterLowDisplay);
    }

}

void drawFilterHigh(gboolean queue) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    if(filterHighDisplay) {
        if(filterHighDisplayPixmap) g_object_unref(filterHighDisplayPixmap);

        filterHighDisplayPixmap=gdk_pixmap_new(filterHighDisplay->window,filterHighDisplay->allocation.width,filterHighDisplay->allocation.height,-1);

        gc=gdk_gc_new(filterHighDisplay->window);
        gdk_gc_set_rgb_fg_color(gc,&black);
        gdk_draw_rectangle(filterHighDisplayPixmap,
                           gc,
                           TRUE,
                           0,0,
                           filterHighDisplay->allocation.width,
                           filterHighDisplay->allocation.height);

        context = gdk_pango_context_get_for_screen (gdk_screen_get_default ());
        layout = pango_layout_new (context);
        pango_layout_set_width(layout,filterHighDisplay->allocation.width*PANGO_SCALE);
        pango_layout_set_alignment(layout,PANGO_ALIGN_RIGHT);
        sprintf(temp,"<span foreground='#7AAA6E' background='#000000' font_desc='Sans Bold 10'>%d</span>",filterHigh);
        pango_layout_set_markup (layout,temp, -1);
        gdk_draw_layout(GDK_DRAWABLE(filterHighDisplayPixmap),gc,0,2,layout);

        g_object_unref(context);
        g_object_unref(layout);
        g_object_unref(gc);

        if(queue) gtk_widget_queue_draw(filterHighDisplay);
    }

}

void selectFilter(GtkWidget* widget) {
    GtkWidget* label;
    char temp[128];

    if(currentFilterButton) {
        label=gtk_bin_get_child((GtkBin*)currentFilterButton);
        gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
        gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &black);
    }
    label=gtk_bin_get_child((GtkBin*)widget);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &buttonSelected);
    gtk_widget_modify_fg(label, GTK_STATE_PRELIGHT, &buttonSelected);
    currentFilterButton=widget;

    if(widget==button6000) {
        filter=filter6000;
    } else if(widget==button4000) {
        filter=filter4000;
    } else if(widget==button2600) {
        filter=filter2600;
    } else if(widget==button2100) {
        filter=filter2100;
    } else if(widget==button1000) {
        filter=filter1000;
    } else if(widget==button500) {
        filter=filter500;
    } else if(widget==button250) {
        filter=filter250;
    } else if(widget==button100) {
        filter=filter100;
    } else if(widget==button50) {
        filter=filter50;
    } else if(widget==button25) {
        filter=filter25;
    } else if(widget==buttonVar1) {
        filter=filterVar1;
    } else if(widget==buttonVar2) {
        filter=filterVar2;
    }

    switch(filter) {
        case filter6000:
            switch(mode) {
                case modeLSB:
                case modeDIGL:
                    filterLow=-6000;
                    filterHigh=-100;
                    break;
                case modeUSB:
                case modeDIGU:
                    filterLow=100;
                    filterHigh=6000;
                    break;
                case modeCWL:
                    filterHigh=min(-cwPitch+3000,-100);
                    filterLow=filterHigh-6000;
                    break;
                case modeCWU:
                    filterLow=max(cwPitch-3000,100);
                    filterHigh=filterLow+6000;
                    break;
                case modeDSB:
                case modeAM:
                case modeFMN:
                case modeSAM:
                    filterLow=-6000;
                    filterHigh=6000;
                    break;
                default:
                    filterLow=-6000;
                    filterHigh=6000;
                    break;
            }
            break;
        case filter4000:
            switch(mode) {
                case modeLSB:
                case modeDIGL:
                    filterLow=-4000;
                    filterHigh=-100;
                    break;
                case modeUSB:
                case modeDIGU:
                    filterLow=100;
                    filterHigh=4000;
                    break;
                case modeCWL:
                    filterHigh=min(-cwPitch+2000,-100);
                    filterLow=filterHigh-4000;
                    break;
                case modeCWU:
                    filterLow=max(cwPitch-2000,100);
                    filterHigh=filterLow+4000;
                    break;
                case modeDSB:
                case modeAM:
                case modeFMN:
                case modeSAM:
                    filterLow=-4000;
                    filterHigh=4000;
                    break;
                default:
                    filterLow=-4000;
                    filterHigh=4000;
                    break;
            }
            break;
        case filter2600:
            switch(mode) {
                case modeLSB:
                case modeDIGL:
                    filterLow=-2800;
                    filterHigh=-200;
                    break;
                case modeUSB:
                case modeDIGU:
                    filterLow=200;
                    filterHigh=2800;
                    break;
                case modeCWL:
                    filterHigh=min(-cwPitch+1300,-100);
                    filterLow=filterHigh-2600;
                    break;
                case modeCWU:
                    filterLow=max(cwPitch-1300,100);
                    filterHigh=filterLow+2600;
                    break;
                case modeDSB:
                case modeAM:
                case modeFMN:
                case modeSAM:
                    filterLow=-2600;
                    filterHigh=2600;
                    break;
                default:
                    filterLow=-2600;
                    filterHigh=2600;
                    break;
            }
            break;
        case filter2100:
            switch(mode) {
                case modeLSB:
                case modeDIGL:
                    filterLow=-2300;
                    filterHigh=-200;
                    break;
                case modeUSB:
                case modeDIGU:
                    filterLow=200;
                    filterHigh=2300;
                    break;
                case modeCWL:
                    filterHigh=min(-cwPitch+1050,-100);
                    filterLow=filterHigh-2100;
                    break;
                case modeCWU:
                    filterLow=max(cwPitch-1050,100);
                    filterHigh=filterLow+2100;
                    break;
                case modeDSB:
                case modeAM:
                case modeFMN:
                case modeSAM:
                    filterLow=-2100;
                    filterHigh=2100;
                    break;
                default:
                    filterLow=-2100;
                    filterHigh=2100;
                    break;
            }
            break;
        case filter1000:
            switch(mode) {
                case modeLSB:
                case modeDIGL:
                    filterLow=-1200;
                    filterHigh=-200;
                    break;
                case modeUSB:
                case modeDIGU:
                    filterLow=200;
                    filterHigh=1200;
                    break;
                case modeCWL:
                    filterHigh=min(-cwPitch+500,-100);
                    filterLow=filterHigh-1000;
                    break;
                case modeCWU:
                    filterLow=max(cwPitch-500,100);
                    filterHigh=filterLow+1000;
                    break;
                case modeDSB:
                case modeAM:
                case modeFMN:
                case modeSAM:
                    filterLow=-1000;
                    filterHigh=1000;
                    break;
                default:
                    filterLow=-1000;
                    filterHigh=1000;
                    break;
            }
            break;
        case filter500:
            switch(mode) {
                case modeLSB:
                case modeDIGL:
                    filterLow=-850;
                    filterHigh=-350;
                    break;
                case modeUSB:
                case modeDIGU:
                    filterLow=350;
                    filterHigh=850;
                    break;
                case modeCWL:
                    filterHigh=min(-cwPitch+250,-100);
                    filterLow=filterHigh-500;
                    break;
                case modeCWU:
                    filterLow=max(cwPitch-250,100);
                    filterHigh=filterLow+500;
                    break;
                case modeDSB:
                case modeAM:
                case modeFMN:
                case modeSAM:
                    filterLow=-500;
                    filterHigh=500;
                    break;
                default:
                    filterLow=-500;
                    filterHigh=500;
                    break;
            }
            break;
        case filter250:
            switch(mode) {
                case modeLSB:
                case modeDIGL:
                    filterLow=-725;
                    filterHigh=-475;
                    break;
                case modeUSB:
                case modeDIGU:
                    filterLow=475;
                    filterHigh=725;
                    break;
                case modeCWL:
                    filterHigh=min(-cwPitch+125,-100);
                    filterLow=filterHigh-250;
                    break;
                case modeCWU:
                    filterLow=max(cwPitch-125,100);
                    filterHigh=filterLow+250;
                    break;
                case modeDSB:
                case modeAM:
                case modeFMN:
                case modeSAM:
                    filterLow=-250;
                    filterHigh=250;
                    break;
                default:
                    filterLow=-250;
                    filterHigh=250;
                    break;
            }
            break;
        case filter100:
            switch(mode) {
                case modeLSB:
                case modeDIGL:
                    filterLow=-650;
                    filterHigh=-550;
                    break;
                case modeUSB:
                case modeDIGU:
                    filterLow=550;
                    filterHigh=650;
                    break;
                case modeCWL:
                    filterHigh=min(-cwPitch+50,-100);
                    filterLow=filterHigh-100;
                    break;
                case modeCWU:
                    filterLow=max(cwPitch-50,100);
                    filterHigh=filterLow+100;
                    break;
                case modeDSB:
                case modeAM:
                case modeFMN:
                case modeSAM:
                    filterLow=-100;
                    filterHigh=100;
                    break;
                default:
                    filterLow=-100;
                    filterHigh=100;
                    break;
            }
            break;
        case filter50:
            switch(mode) {
                case modeLSB:
                case modeDIGL:
                    filterLow=-625;
                    filterHigh=-575;
                    break;
                case modeUSB:
                case modeDIGU:
                    filterLow=575;
                    filterHigh=627;
                    break;
                case modeCWL:
                    filterHigh=min(-cwPitch+25,-100);
                    filterLow=filterHigh-50;
                    break;
                case modeCWU:
                    filterLow=max(cwPitch-25,100);
                    filterHigh=filterLow+50;
                    break;
                case modeDSB:
                case modeAM:
                case modeFMN:
                case modeSAM:
                    filterLow=-50;
                    filterHigh=50;
                    break;
                default:
                    filterLow=-50;
                    filterHigh=50;
                    break;
            }
            break;
        case filter25:
            switch(mode) {
                case modeLSB:
                case modeDIGL:
                    filterLow=-613;
                    filterHigh=-587;
                    break;
                case modeUSB:
                case modeDIGU:
                    filterLow=587;
                    filterHigh=613;
                    break;
                case modeCWL:
                    filterHigh=min(-cwPitch+13,-100);
                    filterLow=filterHigh-25;
                    break;
                case modeCWU:
                    filterLow=max(cwPitch-13,100);
                    filterHigh=filterLow+25;
                    break;
                case modeDSB:
                case modeAM:
                case modeFMN:
                case modeSAM:
                    filterLow=-25;
                    filterHigh=25;
                    break;
                default:
                    filterLow=-25;
                    filterHigh=25;
                    break;
            }
            break;
        case filterVar1:
            filterLow=filterVar1Low;
            filterHigh=filterVar1High;
            break;
        case filterVar2:
            filterLow=filterVar2Low;
            filterHigh=filterVar2High;
            break;
    }
    
    sprintf(temp,"setFilter %d %d",filterLow,filterHigh);
    writeCommand(temp);

    drawFilterHigh(TRUE);
    drawFilterLow(TRUE);

}

void setFilter(int f) {
    GtkWidget* widget;
        switch(f) {
            case filter6000:
                widget=button6000;
                break;
            case filter4000:
                widget=button4000;
                break;
            case filter2600:
                widget=button2600;
                break;
            case filter2100:
                widget=button2100;
                break;
            case filter1000:
                widget=button1000;
                break;
            case filter500:
                widget=button500;
                break;
            case filter250:
                widget=button250;
                break;
            case filter100:
                widget=button100;
                break;
            case filter50:
                widget=button50;
                break;
            case filter25:
                widget=button25;
                break;
            case filterVar1:
                widget=buttonVar1;
                break;
            case filterVar2:
                widget=buttonVar2;
                break;
        }
        selectFilter(widget);
}

void filterButtonCallback(GtkWidget* widget,gpointer data) {
    selectFilter(widget);
}

gboolean filterHighLabel_configure_event(GtkWidget* widget,GdkEventConfigure* event) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;

    if(filterHighLabelPixmap) g_object_unref(filterHighLabelPixmap);

    filterHighLabelPixmap=gdk_pixmap_new(widget->window,widget->allocation.width,widget->allocation.height,-1);

    gc=gdk_gc_new(widget->window);
    gdk_gc_set_rgb_fg_color(gc,&black);
    gdk_draw_rectangle(filterHighLabelPixmap,
                       gc,
                       TRUE,
                       0,0,
                       widget->allocation.width,
                       widget->allocation.height);

    context = gdk_pango_context_get_for_screen (gdk_screen_get_default ());
    layout = pango_layout_new (context);
    pango_layout_set_width(layout,widget->allocation.width*PANGO_SCALE);
    pango_layout_set_alignment(layout,PANGO_ALIGN_LEFT);
    pango_layout_set_markup (layout, "<span foreground='#7AAA6E' background='#000000' font_desc='Sans Bold 12'>H</span>", -1);
    gdk_draw_layout(GDK_DRAWABLE(filterHighLabelPixmap),gc,0,2,layout);

    g_object_unref(context);
    g_object_unref(layout);
    g_object_unref(gc);

    return TRUE;
}

gboolean filterHighLabel_expose_event(GtkWidget* widget,GdkEventExpose* event) {
    gdk_draw_drawable(widget->window,
                    widget->style->fg_gc[GTK_WIDGET_STATE (widget)],
                    filterHighLabelPixmap,
                    event->area.x, event->area.y,
                    event->area.x, event->area.y,
                    event->area.width, event->area.height);
    return FALSE;
}

gboolean filterHighDisplay_configure_event(GtkWidget* widget,GdkEventConfigure* event) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    if(filterHighDisplayPixmap) g_object_unref(filterHighDisplayPixmap);

    filterHighDisplayPixmap=gdk_pixmap_new(widget->window,widget->allocation.width,widget->allocation.height,-1);

    drawFilterHigh(FALSE);

    return TRUE;
}

gboolean filterHighDisplay_expose_event(GtkWidget* widget,GdkEventExpose* event) {
    gdk_draw_drawable(widget->window,
                    widget->style->fg_gc[GTK_WIDGET_STATE (widget)],
                    filterHighDisplayPixmap,
                    event->area.x, event->area.y,
                    event->area.x, event->area.y,
                    event->area.width, event->area.height);
    return FALSE;
}

gboolean filterLowLabel_configure_event(GtkWidget* widget,GdkEventConfigure* event) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;

    if(filterLowLabelPixmap) g_object_unref(filterLowLabelPixmap);

    filterLowLabelPixmap=gdk_pixmap_new(widget->window,widget->allocation.width,widget->allocation.height,-1);

    gc=gdk_gc_new(widget->window);
    gdk_gc_set_rgb_fg_color(gc,&black);
    gdk_draw_rectangle(filterLowLabelPixmap,
                       gc,
                       TRUE,
                       0,0,
                       widget->allocation.width,
                       widget->allocation.height);

    context = gdk_pango_context_get_for_screen (gdk_screen_get_default ());
    layout = pango_layout_new (context);
    pango_layout_set_width(layout,widget->allocation.width*PANGO_SCALE);
    pango_layout_set_alignment(layout,PANGO_ALIGN_LEFT);
    pango_layout_set_markup (layout, "<span foreground='#7AAA6E' background='#000000' font_desc='Sans Bold 12'>L</span>", -1);
    gdk_draw_layout(GDK_DRAWABLE(filterLowLabelPixmap),gc,0,2,layout);

    g_object_unref(context);
    g_object_unref(layout);
    g_object_unref(gc);

    return TRUE;
}

gboolean filterLowLabel_expose_event(GtkWidget* widget,GdkEventExpose* event) {
    gdk_draw_drawable(widget->window,
                    widget->style->fg_gc[GTK_WIDGET_STATE (widget)],
                    filterLowLabelPixmap,
                    event->area.x, event->area.y,
                    event->area.x, event->area.y,
                    event->area.width, event->area.height);
    return FALSE;
}

gboolean filterLowDisplay_configure_event(GtkWidget* widget,GdkEventConfigure* event) {
    GdkGC* gc;
    PangoContext *context;
    PangoLayout *layout;
    char temp[128];

    if(filterLowDisplayPixmap) g_object_unref(filterLowDisplayPixmap);

    filterLowDisplayPixmap=gdk_pixmap_new(widget->window,widget->allocation.width,widget->allocation.height,-1);

    drawFilterLow(FALSE);
    return TRUE;
}

gboolean filterLowDisplay_expose_event(GtkWidget* widget,GdkEventExpose* event) {
    gdk_draw_drawable(widget->window,
                    widget->style->fg_gc[GTK_WIDGET_STATE (widget)],
                    filterLowDisplayPixmap,
                    event->area.x, event->area.y,
                    event->area.x, event->area.y,
                    event->area.width, event->area.height);
    return FALSE;
}


void updateFilter(GtkWidget* widget) {
    char temp[80];
    switch(filter) {
        case filterVar1:
            if(widget==buttonHighPlus) {
                filterVar1High+=10;
                filterHigh=filterVar1High;
                drawFilterHigh(TRUE);
            } else if(widget==buttonHighMinus) {
                filterVar1High-=10;
                filterHigh=filterVar1High;
                drawFilterHigh(TRUE);
            } else if(widget==buttonLowPlus) {
                filterVar1Low+=10;
                filterLow=filterVar1Low;
                drawFilterLow(TRUE);
            } else if(widget==buttonLowMinus) {
                filterVar1Low-=10;
                filterLow=filterVar1Low;
                drawFilterLow(TRUE);
            }
            break;
        case filterVar2:
            if(widget==buttonHighPlus) {
                filterVar2High+=10;
                filterHigh=filterVar2High;
                drawFilterHigh(TRUE);
            } else if(widget==buttonHighMinus) {
                filterVar2High-=10;
                filterHigh=filterVar2High;
                drawFilterHigh(TRUE);
            } else if(widget==buttonLowPlus) {
                filterVar2Low+=10;
                filterLow=filterVar2Low;
                drawFilterLow(TRUE);
            } else if(widget==buttonLowMinus) {
                filterVar2Low-=10;
                filterLow=filterVar2Low;
                drawFilterLow(TRUE);
            }
            break;
    }
    sprintf(temp,"setFilter %d %d",filterLow,filterHigh);
    writeCommand(temp);
}

gint filterIncrementTimer(gpointer widget) {
    // stop the current timer
    gtk_timeout_remove(filterTimerId);        
    // update the filter
    updateFilter((GtkWidget*)widget);
    // start a timer
    filterTimerId=gtk_timeout_add(50,filterIncrementTimer,widget);
}

gint filterIncrementCallback(GtkWidget* widget,gpointer data) {
    // increment/decrement the filter high/low
    updateFilter(widget);
    // start a timer
    filterTimerId=gtk_timeout_add(500,filterIncrementTimer,widget);
}

void filterReleasedCallback(GtkWidget* widget,gpointer data) {
    // stop the timer
    gtk_timeout_remove(filterTimerId);        
}

gboolean filter_scroll_event(GtkWidget* widget,GdkEventScroll* event) {
    char temp[80];
    switch(filter) {
        case filterVar1:
            if(event->direction==GDK_SCROLL_UP) {
                if(widget==filterHighLabel || widget==filterHighDisplay) {
                    filterVar1High+=10;
                    filterHigh=filterVar1High;
                    drawFilterHigh(TRUE);
                } else if(widget==filterLowLabel || widget==filterLowDisplay) {
                    filterVar1Low+=10;
                    filterLow=filterVar1Low;
                    drawFilterLow(TRUE);
                }
            } else {
                if(widget==filterHighLabel || widget==filterHighDisplay) {
                    filterVar1High-=10;
                    filterHigh=filterVar1High;
                    drawFilterHigh(TRUE);
                } else if(widget==filterLowLabel || widget==filterLowDisplay) {
                    filterVar1Low-=10;
                    filterLow=filterVar1Low;
                    drawFilterLow(TRUE);
                }
            }
            break;
        case filterVar2:
            if(event->direction==GDK_SCROLL_UP) {
                if(widget==filterHighLabel || widget==filterHighDisplay) {
                    filterVar2High+=10;
                    filterHigh=filterVar1High;
                    drawFilterHigh(TRUE);
                } else if(widget==filterLowLabel || widget==filterLowDisplay) {
                    filterVar2Low+=10;
                    filterLow=filterVar1Low;
                    drawFilterLow(TRUE);
                }
            } else {
                if(widget==filterHighLabel || widget==filterHighDisplay) {
                    filterVar2High-=10;
                    filterHigh=filterVar1High;
                    drawFilterHigh(TRUE);
                } else if(widget==filterLowLabel || widget==filterLowDisplay) {
                    filterVar2Low-=10;
                    filterLow=filterVar1Low;
                    drawFilterLow(TRUE);
                }
            }
            break;
    }
    sprintf(temp,"setFilter %d %d",filterLow,filterHigh);
    writeCommand(temp);
}

GtkWidget* buildFilterUI() {

    GtkWidget* label;

    filterFixed=gtk_fixed_new();
    gtk_widget_modify_bg(filterFixed,GTK_STATE_NORMAL,&background);

    // filter settings
    buttonHighPlus = gtk_button_new_with_label ("+");
    gtk_widget_modify_bg(buttonHighPlus, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonHighPlus);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonHighPlus),20,25);
    g_signal_connect(G_OBJECT(buttonHighPlus),"pressed",G_CALLBACK(filterIncrementCallback),NULL);
    g_signal_connect(G_OBJECT(buttonHighPlus),"released",G_CALLBACK(filterReleasedCallback),NULL);
    gtk_widget_show(buttonHighPlus);
    gtk_fixed_put((GtkFixed*)filterFixed,buttonHighPlus,5,0);

    filterHighLabel=gtk_drawing_area_new();
    gtk_widget_set_size_request(GTK_WIDGET(filterHighLabel),15,25);
    gtk_widget_show(filterHighLabel);
    g_signal_connect(G_OBJECT (filterHighLabel),"configure_event",G_CALLBACK(filterHighLabel_configure_event),NULL);
    g_signal_connect(G_OBJECT (filterHighLabel),"expose_event",G_CALLBACK(filterHighLabel_expose_event),NULL);
    g_signal_connect(G_OBJECT(filterHighLabel),"scroll_event",G_CALLBACK(filter_scroll_event),NULL);
    gtk_widget_set_events(filterHighLabel,GDK_EXPOSURE_MASK|GDK_SCROLL_MASK);
    gtk_widget_show(filterHighLabel);
    gtk_fixed_put((GtkFixed*)filterFixed,filterHighLabel,25,0);

    filterHighDisplay=gtk_drawing_area_new();
    gtk_widget_set_size_request(GTK_WIDGET(filterHighDisplay),45,25);
    gtk_widget_show(filterHighDisplay);
    g_signal_connect(G_OBJECT (filterHighDisplay),"configure_event",G_CALLBACK(filterHighDisplay_configure_event),NULL);
    g_signal_connect(G_OBJECT (filterHighDisplay),"expose_event",G_CALLBACK(filterHighDisplay_expose_event),NULL);
    g_signal_connect(G_OBJECT(filterHighDisplay),"scroll_event",G_CALLBACK(filter_scroll_event),NULL);
    gtk_widget_set_events(filterHighDisplay,GDK_EXPOSURE_MASK|GDK_SCROLL_MASK);
    gtk_widget_show(filterHighDisplay);
    gtk_fixed_put((GtkFixed*)filterFixed,filterHighDisplay,40,0);

    buttonHighMinus = gtk_button_new_with_label ("-");
    gtk_widget_modify_bg(buttonHighMinus, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonHighMinus);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonHighMinus),20,25);
    g_signal_connect(G_OBJECT(buttonHighMinus),"pressed",G_CALLBACK(filterIncrementCallback),NULL);
    g_signal_connect(G_OBJECT(buttonHighMinus),"released",G_CALLBACK(filterReleasedCallback),NULL);
    gtk_widget_show(buttonHighMinus);
    gtk_fixed_put((GtkFixed*)filterFixed,buttonHighMinus,85,0);

    buttonLowPlus = gtk_button_new_with_label ("+");
    gtk_widget_modify_bg(buttonLowPlus, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonLowPlus);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonLowPlus),20,25);
    g_signal_connect(G_OBJECT(buttonLowPlus),"pressed",G_CALLBACK(filterIncrementCallback),NULL);
    g_signal_connect(G_OBJECT(buttonLowPlus),"released",G_CALLBACK(filterReleasedCallback),NULL);
    gtk_widget_show(buttonLowPlus);
    gtk_fixed_put((GtkFixed*)filterFixed,buttonLowPlus,5,27);

    filterLowLabel=gtk_drawing_area_new();
    gtk_widget_set_size_request(GTK_WIDGET(filterLowLabel),15,25);
    gtk_widget_show(filterLowLabel);
    g_signal_connect(G_OBJECT (filterLowLabel),"configure_event",G_CALLBACK(filterLowLabel_configure_event),NULL);
    g_signal_connect(G_OBJECT (filterLowLabel),"expose_event",G_CALLBACK(filterLowLabel_expose_event),NULL);
    g_signal_connect(G_OBJECT(filterLowLabel),"scroll_event",G_CALLBACK(filter_scroll_event),NULL);
    gtk_widget_set_events(filterLowLabel,GDK_EXPOSURE_MASK|GDK_SCROLL_MASK);
    gtk_widget_show(filterLowLabel);
    gtk_fixed_put((GtkFixed*)filterFixed,filterLowLabel,25,27);

    filterLowDisplay=gtk_drawing_area_new();
    gtk_widget_set_size_request(GTK_WIDGET(filterLowDisplay),45,25);
    gtk_widget_show(filterLowDisplay);
    g_signal_connect(G_OBJECT (filterLowDisplay),"configure_event",G_CALLBACK(filterLowDisplay_configure_event),NULL);
    g_signal_connect(G_OBJECT (filterLowDisplay),"expose_event",G_CALLBACK(filterLowDisplay_expose_event),NULL);
    g_signal_connect(G_OBJECT(filterLowDisplay),"scroll_event",G_CALLBACK(filter_scroll_event),NULL);
    gtk_widget_set_events(filterLowDisplay,GDK_EXPOSURE_MASK|GDK_SCROLL_MASK);
    gtk_widget_show(filterLowDisplay);
    gtk_fixed_put((GtkFixed*)filterFixed,filterLowDisplay,40,27);

    buttonLowMinus = gtk_button_new_with_label ("-");
    gtk_widget_modify_bg(buttonLowMinus, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonLowMinus);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonLowMinus),20,25);
    g_signal_connect(G_OBJECT(buttonLowMinus),"pressed",G_CALLBACK(filterIncrementCallback),NULL);
    g_signal_connect(G_OBJECT(buttonLowMinus),"released",G_CALLBACK(filterReleasedCallback),NULL);
    gtk_widget_show(buttonLowMinus);
    gtk_fixed_put((GtkFixed*)filterFixed,buttonLowMinus,85,27);

    buttonVar1 = gtk_button_new_with_label ("VAR1");
    gtk_widget_modify_bg(buttonVar1, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonVar1);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonVar1),50,25);
    g_signal_connect(G_OBJECT(buttonVar1),"clicked",G_CALLBACK(filterButtonCallback),NULL);
    gtk_widget_show(buttonVar1);
    gtk_fixed_put((GtkFixed*)filterFixed,buttonVar1,5,54);

    buttonVar2 = gtk_button_new_with_label ("VAR2");
    gtk_widget_modify_bg(buttonVar2, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)buttonVar2);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(buttonVar2),50,25);
    g_signal_connect(G_OBJECT(buttonVar2),"clicked",G_CALLBACK(filterButtonCallback),NULL);
    gtk_widget_show(buttonVar2);
    gtk_fixed_put((GtkFixed*)filterFixed,buttonVar2,55,54);


    // predefined filters
    button6000 = gtk_button_new_with_label ("6.0K");
    gtk_widget_modify_bg(button6000, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)button6000);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(button6000),50,25);
    g_signal_connect(G_OBJECT(button6000),"clicked",G_CALLBACK(filterButtonCallback),NULL);
    gtk_widget_show(button6000);
    gtk_fixed_put((GtkFixed*)filterFixed,button6000,110,0);

    button4000 = gtk_button_new_with_label ("4.0K");
    gtk_widget_modify_bg(button4000, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)button4000);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(button4000),50,25);
    g_signal_connect(G_OBJECT(button4000),"clicked",G_CALLBACK(filterButtonCallback),NULL);
    gtk_widget_show(button4000);
    gtk_fixed_put((GtkFixed*)filterFixed,button4000,160,0);

    button2600 = gtk_button_new_with_label ("2.6K");
    gtk_widget_modify_bg(button2600, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)button2600);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(button2600),50,25);
    g_signal_connect(G_OBJECT(button2600),"clicked",G_CALLBACK(filterButtonCallback),NULL);
    gtk_widget_show(button2600);
    gtk_fixed_put((GtkFixed*)filterFixed,button2600,110,27);

    button2100 = gtk_button_new_with_label ("2.1K");
    gtk_widget_modify_bg(button2100, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)button2100);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(button2100),50,25);
    g_signal_connect(G_OBJECT(button2100),"clicked",G_CALLBACK(filterButtonCallback),NULL);
    gtk_widget_show(button2100);
    gtk_fixed_put((GtkFixed*)filterFixed,button2100,160,27);

    button1000 = gtk_button_new_with_label ("1.0K");
    gtk_widget_modify_bg(button1000, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)button1000);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(button1000),50,25);
    g_signal_connect(G_OBJECT(button1000),"clicked",G_CALLBACK(filterButtonCallback),NULL);
    gtk_widget_show(button1000);
    gtk_fixed_put((GtkFixed*)filterFixed,button1000,110,54);

    button500 = gtk_button_new_with_label ("500");
    gtk_widget_modify_bg(button500, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)button500);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(button500),50,25);
    g_signal_connect(G_OBJECT(button500),"clicked",G_CALLBACK(filterButtonCallback),NULL);
    gtk_widget_show(button500);
    gtk_fixed_put((GtkFixed*)filterFixed,button500,160,54);

    button250 = gtk_button_new_with_label ("250");
    gtk_widget_modify_bg(button250, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)button250);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(button250),50,25);
    g_signal_connect(G_OBJECT(button250),"clicked",G_CALLBACK(filterButtonCallback),NULL);
    gtk_widget_show(button250);
    gtk_fixed_put((GtkFixed*)filterFixed,button250,110,81);

    button100 = gtk_button_new_with_label ("100");
    gtk_widget_modify_bg(button100, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)button100);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(button100),50,25);
    g_signal_connect(G_OBJECT(button100),"clicked",G_CALLBACK(filterButtonCallback),NULL);
    gtk_widget_show(button100);
    gtk_fixed_put((GtkFixed*)filterFixed,button100,160,81);

    button50 = gtk_button_new_with_label ("50");
    gtk_widget_modify_bg(button50, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)button50);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(button50),50,25);
    g_signal_connect(G_OBJECT(button50),"clicked",G_CALLBACK(filterButtonCallback),NULL);
    gtk_widget_show(button50);
    gtk_fixed_put((GtkFixed*)filterFixed,button50,110,108);

    button25 = gtk_button_new_with_label ("25");
    gtk_widget_modify_bg(button25, GTK_STATE_NORMAL, &buttonBackground);
    label=gtk_bin_get_child((GtkBin*)button25);
    gtk_widget_modify_fg(label, GTK_STATE_NORMAL, &white);
    gtk_widget_set_size_request(GTK_WIDGET(button25),50,25);
    g_signal_connect(G_OBJECT(button25),"clicked",G_CALLBACK(filterButtonCallback),NULL);
    gtk_widget_show(button25);
    gtk_fixed_put((GtkFixed*)filterFixed,button25,160,108);

    gtk_widget_set_size_request(GTK_WIDGET(filterFixed),230,140);
    gtk_widget_show(filterFixed);

    return filterFixed;
}

void filterSaveState() {
    char string[128];
    sprintf(string,"%d",filter);
    setProperty("filter",string);
}

void filterRestoreState() {
    char* value;
    value=getProperty("filter");
    if(value) filter=atoi(value);
}

