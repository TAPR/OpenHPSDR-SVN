/** 
* @file test.c
* @brief Test functions
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
// test.c
//

#include <gtk/gtk.h>
#include "alex_rx_test.h"
#include "alex_tx_test.h"

GtkWidget* testWindow=NULL;
GtkWidget* testNotebook;


void quitTest();

void test_update() {
    if(testWindow!=NULL) {
        updateDisplaySetup();
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief ghpsdr_test
*/
void test_setup() {
    GtkWidget* page;
    if(testWindow==NULL) {
        testWindow=gtk_window_new(GTK_WINDOW_TOPLEVEL);
        gtk_window_set_title((GtkWindow*)testWindow,(gchar*)"HPSDR: Alex Test");
        g_signal_connect(G_OBJECT(testWindow),"destroy",G_CALLBACK(quitTest),NULL);

        testNotebook=gtk_notebook_new();

        page=alexRxTestUI();
        gtk_notebook_append_page(GTK_NOTEBOOK(testNotebook),page,gtk_label_new("Alex Rx Test"));
        gtk_widget_show(page);

        page=alexTxTestUI();
        gtk_notebook_append_page(GTK_NOTEBOOK(testNotebook),page,gtk_label_new("Alex Tx Test"));
        gtk_widget_show(page);

        gtk_widget_show(testNotebook);
        gtk_container_add(GTK_CONTAINER(testWindow), testNotebook);
        gtk_widget_show(testWindow);
    } else {
        gtk_window_set_keep_above(GTK_WINDOW(testWindow),TRUE);
    }

}

/* --------------------------------------------------------------------------*/
/** 
* @brief Quit test
*/
void quitTest() {
    gtk_widget_destroy(testWindow);
    testWindow=NULL;
}
