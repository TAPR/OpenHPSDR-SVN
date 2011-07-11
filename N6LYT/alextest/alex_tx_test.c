/** 
* @file alex_tx_test.c
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
// alex_tx_test.c
//

#include <stdlib.h>
#include <string.h>
#include <gtk/gtk.h>
#include "alex_tx_test.h"
#include "vfo.h"
#include "meter.h"
#include "ozy.h"
#include "transmit.h"
#include "mode.h"
#include "filter.h"
#include "test.h"

#define MODE modeAM
#define FILTER filterF9
#define SAMPLES 50
#define TIMER 10

static GtkWidget* alexTxTestPage;
static GtkWidget* alexTxStartButton;
static GtkWidget* alexTxStepButton;
static GtkWidget* passFail;

static GtkWidget* dialog;

// tests to perform
static TX_TEST tx_test[]= {
    {"X1","X3","K17,K18,K8",2000000,0,-43,"6M bypass, DIR COUPLER, K8 NC",NULL,NULL,NULL,0},
    {"X1","X4","K17,K18,K8",2000000,0,0,"6M bypass, DIR COUPLER, K11 NO",NULL,NULL,NULL,0},
    {"X1","X5","K17,K18,K8",2000000,0,0,"6M bypass, K14 NO",NULL,NULL,NULL,0},
    {"X1","X3","K17,K18,K11",2000000,0,0,"6M bypass, K8 NO",NULL,NULL,NULL,0},
    {"X1","X4","K17,K18,K11",2000000,0,-43,"6M bypass, K11 NC",NULL,NULL,NULL,0},
    {"X1","X5","K17,K18,K11",2000000,0,0,"6M bypass, K14 NO",NULL,NULL,NULL,0},
    {"X1","X3","K17,K18,K14",2000000,0,0,"6M bypass, K8 NO",NULL,NULL,NULL,0},
    {"X1","X4","K17,K18,K14",2000000,0,0,"6M bypass, K11 NO",NULL,NULL,NULL,0},
    {"X1","X5","K17,K18,K14",2000000,0,-43,"6M bypass, K14 NC",NULL,NULL,NULL,0},
    {"X1","X5","K17,K18,K14",61400000,0,-43,"6M LPF",NULL,NULL,NULL,0},
    {"X1","X5","K17,K18,K14,K5",2000000,0,0,"K5 NC",NULL,NULL,NULL,0},
    {"X1","X5","K17,K18,K14,K5",2000000,0,-43,"K5 NO",NULL,NULL,NULL,0},
    {"X2","X5","K17,K18,K14",2000000,0,0,"K5 NO",NULL,NULL,NULL,0},
    {"X2","X5","K14",2000000,0,0,"all LPF NO",NULL,NULL,NULL,0},
    {"X1","X5","K14,K1/2",2400000,0,-44,"160M LPF pass",NULL,NULL,NULL,0},
    {"X1","X5","K14,K1/2",3160000,0,-74,"160M LPF reject",NULL,NULL,NULL,0},
    {"X1","X5","K14,K3/4",5000000,0,-44,"80M LPF padd",NULL,NULL,NULL,0},
    {"X1","X5","K14,K3/4",6420000,0,-74,"80M HPF reject",NULL,NULL,NULL,0},
    {"X1","X5","K14,K6/7",7900000,0,-44,"60/40M HPF pass",NULL,NULL,NULL,0},
    {"X1","X5","K14,K6/7",11420000,0,-74,"60/40M HPF reject",NULL,NULL,NULL,0},
    {"X1","X5","K14,K9/10",16500000,0,-44,"30/20M HPF pass",NULL,NULL,NULL,0},
    {"X1","X5","K14,K9/10",19560000,0,-74,"30/20M HPF reject",NULL,NULL,NULL,0},
    {"X1","X5","K14,K12/13",25400000,0,-44,"17/15M HPF pass",NULL,NULL,NULL,0},
    {"X1","X5","K14,K12/13",33960000,0,-74,"17/15M HPF reject",NULL,NULL,NULL,0},
    {"X1","X5","K14,K15/16",33400000,0,-44,"12/10M HPF pass",NULL,NULL,NULL,0},
    {"X1","X5","K14,K15/16",45480000,0,-74,"12/10M HPF reject",NULL,NULL,NULL,0},
    {"","","",0,0,0,"",NULL,NULL,NULL,0}
    };

// timer for steppiung through tests
static gint timerId;

// test number and stage within a test
static gint test;
static gint stage;
static gint samples;

static int level;

static int failures;
static gboolean stepping;

/* --------------------------------------------------------------------------*/
/**
* @brief Alex Tx Test next step
*
* @param data
*
* @return
*/
gint alexTxTest(gpointer data) {
    TX_TEST* t;
    char text[16];
    GdkColor color;
    int min,max;

//fprintf(stderr,"alexTxTest test=%d stage=%d\n",test,stage);
    t=&tx_test[test];
    if(t->frequency==0) {
        gtk_timeout_remove(timerId);
        stepping=FALSE;
        testing=0;
        test=0;
        stage=0;
        if(failures>0) {
            gdk_color_parse("red", &color);
            gtk_widget_modify_fg(passFail, GTK_STATE_NORMAL, &color);
            gtk_label_set_text(GTK_LABEL(passFail),"FAIL");
        } else {
            gdk_color_parse("green", &color);
            gtk_widget_modify_fg(passFail, GTK_STATE_NORMAL, &color);
            gtk_label_set_text(GTK_LABEL(passFail),"PASS");
        }
    } else {
        switch(stage) {
            case 0: // set frequency and test
//                fprintf(stderr,"Test: %d: %s\n",test,t->description);
//                fprintf(stderr,"Set Frequency %2.2f\n",(float)t->frequency/1000000.0f);
                gdk_color_parse("red", &color);
                gtk_widget_modify_fg(t->testWidget, GTK_STATE_NORMAL, &color);
                setMode(mode);
                setFilter(FILTER);
                setAFrequency(t->frequency+(long long)(test+1)+(long long)64);
                if(stepping) {
                    gtk_timeout_remove(timerId);
                }
                samples=0;
                stage++;
                break;
            case 1: // start RF
//                fprintf(stderr,"Start RF\n");
                gdk_color_parse("red", &color);
                gtk_widget_modify_fg(t->frequencyWidget, GTK_STATE_NORMAL, &color);
                testing=1;
                tuningPhase=0.0;
                level=-250;
                setMOX(1);
                stage++;
                break;
            case 2: // get level
                if(meterDbm>level) level=meterDbm;
                samples++;
                if(samples==SAMPLES) {
                    sprintf(text,"%d dBm",level);
                    if(t->rf_exp_level==0) {
                        if(level>-100) {
                            gdk_color_parse("red", &color);
                            gtk_widget_modify_fg(t->levelWidget, GTK_STATE_NORMAL, &color);
                            failures++;
                        } else {
                            gdk_color_parse("green", &color);
                            gtk_widget_modify_fg(t->levelWidget, GTK_STATE_NORMAL, &color);
                        }
                    } else {
                        // +/- 5%
                        min=t->rf_exp_level + ((t->rf_exp_level*5)/100);
                        max=t->rf_exp_level - ((t->rf_exp_level*5)/100);
                        if(level<min || level>max) {
                            gdk_color_parse("red", &color);
                            gtk_widget_modify_fg(t->levelWidget, GTK_STATE_NORMAL, &color);
                            failures++;
                        } else {
                            gdk_color_parse("green", &color);
                            gtk_widget_modify_fg(t->levelWidget, GTK_STATE_NORMAL, &color);
                        }
                    }
                    gtk_label_set_text(GTK_LABEL(t->levelWidget),text);
                    if(t->pause) {
                        //stop the timer
                        gtk_timeout_remove(timerId);
                        // dialog box to say adjust C94
                        dialog = gtk_message_dialog_new(GTK_WINDOW(testWindow),
                                     GTK_DIALOG_DESTROY_WITH_PARENT,
                                     GTK_MESSAGE_INFO,
                                     GTK_BUTTONS_OK,
                                     "Adjust C94", "Alex Rx Test");
                        gtk_window_set_title(GTK_WINDOW(dialog), "Alex Rx Test");
                        gtk_dialog_run(GTK_DIALOG(dialog));
                        gtk_widget_destroy(dialog);

                        // restart the timer
                        timerId=gtk_timeout_add(TIMER,alexTxTest,NULL);
                    }
                    if(stepping) {
                        gtk_timeout_remove(timerId);
                    }
                    stage++;
                }
                break;
            case 3: // stop RF
 //               fprintf(stderr,"Stop RF\n");
                gdk_color_parse("black", &color);
                gtk_widget_modify_fg(t->testWidget, GTK_STATE_NORMAL, &color);
                gtk_widget_modify_fg(t->frequencyWidget, GTK_STATE_NORMAL, &color);
                setMOX(0);
                testing=0;
                stage=0;
                test++;
                break;
        }
    }
    return TRUE;
}

/* --------------------------------------------------------------------------*/
/**
* @brief Alex Test Start Button Callback
*
* @param data
*
* @return
*/
void alexTxTestStartButtonCallback(GtkWidget* widget,gpointer data) {
    if(testing) {
        // stop the testing sequence
        gtk_timeout_remove(timerId);
        testing=0;
        test=0;
        stage=0;
    } else {
        // start the testing sequence
        testing=1;
        test=0;
        stage=0;
        stepping=FALSE;

        setMode(MODE);
        setFilter(FILTER);

        failures=0;
        gtk_label_set_text(GTK_LABEL(passFail),"");
        
        // start the timer
        timerId=gtk_timeout_add(TIMER,alexTxTest,NULL);
    }
}

/* --------------------------------------------------------------------------*/
/**
* @brief Alex Step Start Button Callback
*
* @param data
*
* @return
*/
void alexTxTestStepButtonCallback(GtkWidget* widget,gpointer data) {
    stepping=TRUE;
    if(test==0) {
        failures=0;
        gtk_label_set_text(GTK_LABEL(passFail),"");
    }
    timerId=gtk_timeout_add(TIMER,alexTxTest,NULL);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief  Alex Tx Test UI
*/
GtkWidget* alexTxTestUI() {
    GtkWidget* label;
    GtkWidget* table;
    GtkWidget* alignment;
    GtkWidget* box;
    char text[80];
    int i;

    alexTxTestPage=gtk_vbox_new(FALSE,1);

    box=gtk_hbox_new(FALSE,1);

    alexTxStartButton = gtk_button_new_with_label ("Start");
    g_signal_connect(G_OBJECT(alexTxStartButton),"clicked",G_CALLBACK(alexTxTestStartButtonCallback),NULL);
    gtk_widget_show(alexTxStartButton);
    gtk_box_pack_start(GTK_BOX(box),alexTxStartButton,FALSE,FALSE,2);

    alexTxStepButton = gtk_button_new_with_label ("Step");
    g_signal_connect(G_OBJECT(alexTxStepButton),"clicked",G_CALLBACK(alexTxTestStepButtonCallback),NULL);
    gtk_widget_show(alexTxStepButton);
    gtk_box_pack_start(GTK_BOX(box),alexTxStepButton,FALSE,FALSE,2);

    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(alexTxTestPage),box,FALSE,FALSE,2);

    table=gtk_table_new(8,26+1,FALSE);

    i=0;

    // titles
    label=gtk_label_new("test");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 0, 1, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    label=gtk_label_new("Stim");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 1, 2, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    label=gtk_label_new("Det");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 2, 3, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    label=gtk_label_new("Relay Activate");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 3, 4, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

    label=gtk_label_new("RF stim freq");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 4, 5, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    label=gtk_label_new("Description");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 5, 6, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    label=gtk_label_new("RF exp level");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 6, 7, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    label=gtk_label_new("RF det level");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 7, 8, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    i=0;
    while(tx_test[i].frequency!=0) {
        //fprintf(stderr,"%d %s\n",i+1,tx_test[i].description);

        sprintf(text,"%d",i+1);
        label=gtk_label_new(text);
        gtk_widget_show(label);
        tx_test[i].testWidget=label;
        alignment=gtk_alignment_new(1,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 0, 1, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
     
        sprintf(text,"%s",tx_test[i].stimulate_on);
        label=gtk_label_new(text);
        gtk_widget_show(label);
        alignment=gtk_alignment_new(1,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 1, 2, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        sprintf(text,"%s",tx_test[i].detect_on);
        label=gtk_label_new(text);
        gtk_widget_show(label);
        alignment=gtk_alignment_new(1,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 2, 3, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        sprintf(text,"%s",tx_test[i].relay_activate);
        label=gtk_label_new(text);
        gtk_widget_show(label);
        alignment=gtk_alignment_new(1,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 3, 4, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        sprintf(text,"%2.2fMHz",(float)tx_test[i].frequency/1000000.0f);
        label=gtk_label_new(text);
        gtk_widget_show(label);
        tx_test[i].frequencyWidget=label;
        alignment=gtk_alignment_new(1,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 4, 5, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        label=gtk_label_new(tx_test[i].description);
        gtk_widget_show(label);
        alignment=gtk_alignment_new(0,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 5, 6, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        if(tx_test[i].rf_exp_level==0) {
            sprintf(text,"%s","no output");
        } else {
            sprintf(text,"%ddBm",tx_test[i].rf_exp_level);
        }
        label=gtk_label_new(text);
        gtk_widget_show(label);
        alignment=gtk_alignment_new(0,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 6, 7, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        label=gtk_label_new("-dBm");
        tx_test[i].levelWidget=label;
        gtk_widget_show(label);
        alignment=gtk_alignment_new(0,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 7, 8, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        i++;
    }

        label=gtk_label_new("Result");
        gtk_widget_show(label);
        gtk_table_attach(GTK_TABLE(table), label, 6, 7, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        label=gtk_label_new("");
        gtk_widget_show(label);
        passFail=label;
        gtk_table_attach(GTK_TABLE(table), label, 7, 8, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);


    gtk_widget_show(table);
    gtk_box_pack_start(GTK_BOX(alexTxTestPage),table,FALSE,FALSE,2);
    gtk_widget_show(alexTxTestPage);

    testing=FALSE;

    return alexTxTestPage;
}
