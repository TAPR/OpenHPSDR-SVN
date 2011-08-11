/** 
* @file alex_rx_test.c
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
// alex_rx_test.c
//

#include <stdlib.h>
#include <string.h>
#include <stdio.h>
#include <gtk/gtk.h>
#include "alex_rx_test.h"
#include "alex_test.h"
#include "vfo.h"
#include "meter.h"
#include "ozy.h"
#include "transmit.h"
#include "mode.h"
#include "filter.h"
#include "test.h"
#include "expression.h"
#include "dttsp.h"

#define MODE modeAM
#define FILTER filterF9
#define SAMPLES 40
#define TIMER 20

static GtkWidget* alexRxTestPage;
static GtkWidget* alexRxStartButton;
static GtkWidget* alexRxStepButton;
static GtkWidget* passFail;

static GtkWidget* dialog;

// tests to perform
static ALEX_TEST rx_test[100];
static int rx_test_index=0;

// timer for steppiung through tests
static gint timerId;

// test number and stage within a test
static gint test;
static gint stage;
static gint samples;
// test sample values
static int level;
static int forwardPower;
static int reversePower;

static int failures;
static gboolean stepping;

static int max_samples=SAMPLES;

void alex_rx_set_samples(int s) {
    max_samples=s;
}

int alex_rx_test_get_result(int id) {
    ALEX_TEST* t;
    int result=0;
    int i=0;
fprintf(stderr,"alex_rx_test_get_result: %d\n",id);
    while(rx_test[i].frequency!=0) {
        if(rx_test[i].id==id) {
            result=rx_test[i].rf_det_level;
fprintf(stderr,"alex_rx_test_get_result: result=%d\n",result);
            break;
        }
        i++;
    }
    return result;
}

int alex_rx_test_get_fwd(int id) {
    ALEX_TEST* t;
    int result=0;
    int i=0;
fprintf(stderr,"alex_rx_test_get_fwd: %d\n",id);
    while(rx_test[i].frequency!=0) {
        if(rx_test[i].id==id) {
            result=rx_test[i].rf_fwd_level;
fprintf(stderr,"alex_rx_test_get_fwd: result=%d\n",result);
            break;
        }
        i++;
    }
    return result;
}

int alex_rx_test_get_rev(int id) {
    ALEX_TEST* t;
    int result=0;
    int i=0;
fprintf(stderr,"alex_rx_test_get_rev: %d\n",id);
    while(rx_test[i].frequency!=0) {
        if(rx_test[i].id==id) {
            result=rx_test[i].rf_rev_level;
fprintf(stderr,"alex_rx_test_get_rev: result=%d\n",result);
            break;
        }
        i++;
    }
    return result;
}

/* --------------------------------------------------------------------------*/
/**
* @brief Alex Rx Test next step
*
* @param data
*
* @return
*/
gint alexRxTest(gpointer data) {
    ALEX_TEST* t;
    char text[16];
    GdkColor color;
    int min,max;
    

//fprintf(stderr,"alexRxTest test=%d stage=%d\n",test,stage);
    t=&rx_test[test];
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
                setAFrequency(t->frequency);
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
                forwardPower=0;
                reversePower=0;
                setMOX(1);
                stage++;
                break;
            case 2: // pause if required
                if(strcmp(t->pause,"NO")!=0) {
                    //stop the timer
                    gtk_timeout_remove(timerId);
                    // dialog box to say adjust C94
                    dialog = gtk_message_dialog_new(GTK_WINDOW(testWindow),
                                 GTK_DIALOG_DESTROY_WITH_PARENT,
                                 GTK_MESSAGE_INFO,
                                 GTK_BUTTONS_OK,
                                 t->pause, "Alex Rx Test");
                    gtk_window_set_title(GTK_WINDOW(dialog), "Alex Rx Test");
                    gtk_dialog_run(GTK_DIALOG(dialog));
                    gtk_widget_destroy(dialog);

                    // restart the timer
                    timerId=gtk_timeout_add(TIMER,alexRxTest,NULL);
                }
                stage++;
                break;
            case 3: // get level
                if(meterDbm>level) level=meterDbm;
                if(alexForwardPower>forwardPower) forwardPower=alexForwardPower;
                if(alexReversePower>reversePower) reversePower=alexReversePower;
                samples++;
                if(samples>=max_samples) {
                    //t->rf_det_level=level;
                    //t->rf_fwd_level=forwardPower;
                    //t->rf_rev_level=reversePower;
                    t->rf_det_level=getMeter();
                    t->rf_fwd_level=alexForwardPower;
                    t->rf_rev_level=alexReversePower;
                    min=evaluate(t->rf_min_level,&alex_rx_test_get_result,&alex_rx_test_get_fwd,&alex_rx_test_get_rev);
                    max=evaluate(t->rf_max_level,&alex_rx_test_get_result,&alex_rx_test_get_fwd,&alex_rx_test_get_rev);

fprintf(stderr,"level=%d fwd=%d rev=%d min=%d max=%d\n",level,forwardPower,reversePower,min,max);

                    if(strcmp(t->rf_source,"D")==0) {
                        if(level<min || level>max) {
                            gdk_color_parse("red", &color);
                            gtk_widget_modify_fg(t->levelWidget, GTK_STATE_NORMAL, &color);
                            failures++;
                        } else {
                            gdk_color_parse("green", &color);
                            gtk_widget_modify_fg(t->levelWidget, GTK_STATE_NORMAL, &color);
                        }
                        sprintf(text,"%d dBm",level);
                        gtk_label_set_text(GTK_LABEL(t->levelWidget),text);
                    } else if(strcmp(t->rf_source,"F")==0) {
                        if(forwardPower<min || forwardPower>max) {
                            gdk_color_parse("red", &color);
                            gtk_widget_modify_fg(t->levelWidget, GTK_STATE_NORMAL, &color);
                            failures++;
                        } else {
                            gdk_color_parse("green", &color);
                            gtk_widget_modify_fg(t->levelWidget, GTK_STATE_NORMAL, &color);
                        }
                        sprintf(text,"%d",forwardPower);
                        gtk_label_set_text(GTK_LABEL(t->levelWidget),text);
                    } else if(strcmp(t->rf_source,"R")==0) {
                        if(reversePower<min || reversePower>max) {
                            gdk_color_parse("red", &color);
                            gtk_widget_modify_fg(t->levelWidget, GTK_STATE_NORMAL, &color);
                            failures++;
                        } else {
                            gdk_color_parse("green", &color);
                            gtk_widget_modify_fg(t->levelWidget, GTK_STATE_NORMAL, &color);
                        }
                        sprintf(text,"%d",reversePower);
                        gtk_label_set_text(GTK_LABEL(t->levelWidget),text);
                    }
                    if(stepping) {
                        gtk_timeout_remove(timerId);
                    }
                    stage++;
                }
                break;
            case 4: // stop RF
 //               fprintf(stderr,"Stop RF\n");
                gdk_color_parse("black", &color);
                gtk_widget_modify_fg(t->frequencyWidget, GTK_STATE_NORMAL, &color);
                gtk_widget_modify_fg(t->testWidget, GTK_STATE_NORMAL, &color);
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
void alexRxTestStartButtonCallback(GtkWidget* widget,gpointer data) {
    int i;
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

        i=0;
        while(rx_test[i].frequency!=0) {
            gtk_label_set_text(GTK_LABEL(rx_test[i].levelWidget),"");
            i++;
        }
        failures=0;
        gtk_label_set_text(GTK_LABEL(passFail),"");
        
        // start the timer
        timerId=gtk_timeout_add(TIMER,alexRxTest,NULL);
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
void alexRxTestStepButtonCallback(GtkWidget* widget,gpointer data) {
    stepping=TRUE;
    if(test==0) {
        failures=0;
        gtk_label_set_text(GTK_LABEL(passFail),"");
    }
    timerId=gtk_timeout_add(TIMER,alexRxTest,NULL);
}

/* --------------------------------------------------------------------------*/
/** 
* @brief  Alex Rx Test UI
*/
GtkWidget* alexRxTestUI() {
    GtkWidget* label;
    GtkWidget* table;
    GtkWidget* alignment;
    GtkWidget* box;
    char text[80];
    int i;

    alexRxTestPage=gtk_vbox_new(FALSE,1);

    box=gtk_hbox_new(FALSE,1);

    alexRxStartButton = gtk_button_new_with_label ("Start");
    g_signal_connect(G_OBJECT(alexRxStartButton),"clicked",G_CALLBACK(alexRxTestStartButtonCallback),NULL);
    gtk_widget_show(alexRxStartButton);
    gtk_box_pack_start(GTK_BOX(box),alexRxStartButton,FALSE,FALSE,2);

    alexRxStepButton = gtk_button_new_with_label ("Step");
    g_signal_connect(G_OBJECT(alexRxStepButton),"clicked",G_CALLBACK(alexRxTestStepButtonCallback),NULL);
    gtk_widget_show(alexRxStepButton);
    gtk_box_pack_start(GTK_BOX(box),alexRxStepButton,FALSE,FALSE,2);

    gtk_widget_show(box);
    gtk_box_pack_start(GTK_BOX(alexRxTestPage),box,FALSE,FALSE,2);

    table=gtk_table_new(7,rx_test_index+1,FALSE);

    i=0;

    // titles
    label=gtk_label_new("test");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 0, 1, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    label=gtk_label_new("RF stim freq");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 1, 2, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    label=gtk_label_new("Description");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 2, 3, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

    label=gtk_label_new("Source");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 3, 4, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    label=gtk_label_new("RF min level");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 4, 5, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    label=gtk_label_new("RF max level");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 5, 6, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    label=gtk_label_new("RF det level");
    gtk_widget_show(label);
    gtk_table_attach(GTK_TABLE(table), label, 6, 7, i, i+1, 
        GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
    
    i=0;
    while(rx_test[i].frequency!=0) {
        //fprintf(stderr,"%d %s\n",i+1,rx_test[i].description);

        sprintf(text,"%d",i+1);
        label=gtk_label_new(text);
        gtk_widget_show(label);
        rx_test[i].testWidget=label;
        alignment=gtk_alignment_new(1,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 0, 1, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);
     
        sprintf(text,"%2.2fMHz",(float)rx_test[i].frequency/1000000.0f);
        label=gtk_label_new(text);
        gtk_widget_show(label);
        rx_test[i].frequencyWidget=label;
        alignment=gtk_alignment_new(1,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 1, 2, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        label=gtk_label_new(rx_test[i].description);
        gtk_widget_show(label);
        alignment=gtk_alignment_new(0,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 2, 3, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        label=gtk_label_new(rx_test[i].rf_source);
        gtk_widget_show(label);
        alignment=gtk_alignment_new(0,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 3, 4, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        label=gtk_label_new(rx_test[i].rf_min_level);
        gtk_widget_show(label);
        alignment=gtk_alignment_new(0,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 4, 5, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        label=gtk_label_new(rx_test[i].rf_max_level);
        gtk_widget_show(label);
        alignment=gtk_alignment_new(0,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 5, 6, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        label=gtk_label_new("-dBm");
        rx_test[i].levelWidget=label;
        gtk_widget_show(label);
        alignment=gtk_alignment_new(0,0,0,0);
        gtk_widget_show(alignment);
        gtk_container_add( GTK_CONTAINER(alignment), label );
        gtk_table_attach(GTK_TABLE(table), alignment, 6, 7, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        i++;
    }

        label=gtk_label_new("Result");
        gtk_widget_show(label);
        gtk_table_attach(GTK_TABLE(table), label, 4, 5, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);

        label=gtk_label_new("");
        gtk_widget_show(label);
        passFail=label;
        gtk_table_attach(GTK_TABLE(table), label, 5, 6, i+1, i+2, 
            GTK_FILL | GTK_SHRINK, GTK_FILL | GTK_SHRINK, 6, 1);


    gtk_widget_show(table);
    gtk_box_pack_start(GTK_BOX(alexRxTestPage),table,FALSE,FALSE,2);
    gtk_widget_show(alexRxTestPage);

    testing=FALSE;

    return alexRxTestPage;
}

void alex_rx_test_load(char* filename) {
    char string[128];
    char* token;
 
    FILE* f=fopen(filename,"r");
    if(f==NULL) {
        fprintf(stderr,"Error: cannot open %s\n",filename);
        exit(1);
    }

    while(fgets(string,sizeof(string),f)) {
fprintf(stderr,string);
        // id
        token=strtok(string,",");
        rx_test[rx_test_index].id=atoi(token);
        // description
        token=strtok(NULL,",");
        strcpy(rx_test[rx_test_index].description,token);
        // frequency
        token=strtok(NULL,",");
        rx_test[rx_test_index].frequency=atoll(token);
        // source
        token=strtok(NULL,",");
        if(token[0]=='"') {
            token[strlen(token)-1]='\0';
            strcpy(rx_test[rx_test_index].rf_source,&token[1]);
        } else {
            strcpy(rx_test[rx_test_index].rf_source,token);
        }
        // min expression
        token=strtok(NULL,",");
        strcpy(rx_test[rx_test_index].rf_min_level,token);
        // max expression
        token=strtok(NULL,",");
        strcpy(rx_test[rx_test_index].rf_max_level,token);
        // pause
        token=strtok(NULL,",");
        if(token[0]=='"') {
            token[strlen(token)-2]='\0';
            strcpy(rx_test[rx_test_index].pause,&token[1]);
        } else {
            strcpy(rx_test[rx_test_index].pause,token);
        }
        rx_test_index++;
    }

    rx_test[rx_test_index].frequency=0;

    fclose(f);
}
