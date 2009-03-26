//
// setup.c
//

#include <gtk/gtk.h>
#include "bandscope.h"
#include "spectrum.h"

GtkWidget* setupWindow=NULL;
GtkWidget* setupFixed;

GtkWidget* spectrumHighLabel;
GtkWidget* spectrumHighSpinButton;
GtkWidget* spectrumLowLabel;
GtkWidget* spectrumLowSpinButton;
GtkWidget* spectrumStepLabel;
GtkWidget* spectrumStepSpinButton;

GtkWidget* waterfallHighLabel;
GtkWidget* waterfallHighSpinButton;
GtkWidget* waterfallLowLabel;
GtkWidget* waterfallLowSpinButton;

GtkWidget* bandscopeHighLabel;
GtkWidget* bandscopeHighSpinButton;
GtkWidget* bandscopeLowLabel;
GtkWidget* bandscopeLowSpinButton;

void quitSetup();
void spectrumHighChanged(GtkSpinButton* spinButton,gpointer data);
void spectrumLowChanged(GtkSpinButton* spinButton,gpointer data);
void spectrumStepChanged(GtkSpinButton* spinButton,gpointer data);
void waterfallHighChanged(GtkSpinButton* spinButton,gpointer data);
void waterfallLowChanged(GtkSpinButton* spinButton,gpointer data);
void bandscopeHighChanged(GtkSpinButton* spinButton,gpointer data);
void bandscopeLowChanged(GtkSpinButton* spinButton,gpointer data);

void setup() {
    if(setupWindow==NULL) {
        setupWindow=gtk_window_new(GTK_WINDOW_TOPLEVEL);
        gtk_window_set_title((GtkWindow*)setupWindow,(gchar*)"Gtk+ HPSDR: setup");
        g_signal_connect(G_OBJECT(setupWindow),"destroy",G_CALLBACK(quitSetup),NULL);
        setupFixed=gtk_fixed_new();

        // add specturm controls
        spectrumHighLabel=gtk_label_new("Spectrum High");
        gtk_widget_show(spectrumHighLabel);
        gtk_fixed_put((GtkFixed*)setupFixed,spectrumHighLabel,10,10);
        spectrumHighSpinButton=gtk_spin_button_new_with_range(-200,200,10);
        gtk_spin_button_set_value((GtkSpinButton*)spectrumHighSpinButton,(double)spectrumMAX);
        g_signal_connect(G_OBJECT(spectrumHighSpinButton),"value-changed",G_CALLBACK(spectrumHighChanged),NULL);
        gtk_widget_show(spectrumHighSpinButton);
        gtk_fixed_put((GtkFixed*)setupFixed,spectrumHighSpinButton,150,10);

        spectrumLowLabel=gtk_label_new("Spectrum Low");
        gtk_widget_show(spectrumLowLabel);
        gtk_fixed_put((GtkFixed*)setupFixed,spectrumLowLabel,10,40);
        spectrumLowSpinButton=gtk_spin_button_new_with_range(-200,200,10);
        gtk_spin_button_set_value((GtkSpinButton*)spectrumLowSpinButton,(double)spectrumMIN);
        g_signal_connect(G_OBJECT(spectrumLowSpinButton),"value-changed",G_CALLBACK(spectrumLowChanged),NULL);
        gtk_widget_show(spectrumLowSpinButton);
        gtk_fixed_put((GtkFixed*)setupFixed,spectrumLowSpinButton,150,40);

        spectrumStepLabel=gtk_label_new("Spectrum Step");
        gtk_widget_show(spectrumStepLabel);
        gtk_fixed_put((GtkFixed*)setupFixed,spectrumStepLabel,10,70);
        spectrumStepSpinButton=gtk_spin_button_new_with_range(10,50,10);
        gtk_spin_button_set_value((GtkSpinButton*)spectrumStepSpinButton,(double)spectrumSTEP);
        g_signal_connect(G_OBJECT(spectrumStepSpinButton),"value-changed",G_CALLBACK(spectrumStepChanged),NULL);
        gtk_widget_show(spectrumStepSpinButton);
        gtk_fixed_put((GtkFixed*)setupFixed,spectrumStepSpinButton,150,70);


        // add waterfall controls
        waterfallHighLabel=gtk_label_new("Waterfall High");
        gtk_widget_show(waterfallHighLabel);
        gtk_fixed_put((GtkFixed*)setupFixed,waterfallHighLabel,10,110);
        waterfallHighSpinButton=gtk_spin_button_new_with_range(-200,200,10);
        gtk_spin_button_set_value((GtkSpinButton*)waterfallHighSpinButton,(double)waterfallHighThreshold);
        g_signal_connect(G_OBJECT(waterfallHighSpinButton),"value-changed",G_CALLBACK(waterfallHighChanged),NULL);
        gtk_widget_show(waterfallHighSpinButton);
        gtk_fixed_put((GtkFixed*)setupFixed,waterfallHighSpinButton,150,110);

        waterfallLowLabel=gtk_label_new("Waterfall Low");
        gtk_widget_show(waterfallLowLabel);
        gtk_fixed_put((GtkFixed*)setupFixed,waterfallLowLabel,10,140);
        waterfallLowSpinButton=gtk_spin_button_new_with_range(-200,200,10);
        gtk_spin_button_set_value((GtkSpinButton*)waterfallLowSpinButton,(double)waterfallLowThreshold);
        g_signal_connect(G_OBJECT(waterfallLowSpinButton),"value-changed",G_CALLBACK(waterfallLowChanged),NULL);
        gtk_widget_show(waterfallLowSpinButton);
        gtk_fixed_put((GtkFixed*)setupFixed,waterfallLowSpinButton,150,140);

        // add bandscope controls
        bandscopeHighLabel=gtk_label_new("Bandscope High");
        gtk_widget_show(bandscopeHighLabel);
        gtk_fixed_put((GtkFixed*)setupFixed,bandscopeHighLabel,10,180);
        bandscopeHighSpinButton=gtk_spin_button_new_with_range(-200,200,10);
        gtk_spin_button_set_value((GtkSpinButton*)bandscopeHighSpinButton,(double)bandscopeMAX);
        g_signal_connect(G_OBJECT(bandscopeHighSpinButton),"value-changed",G_CALLBACK(bandscopeHighChanged),NULL);
        gtk_widget_show(bandscopeHighSpinButton);
        gtk_fixed_put((GtkFixed*)setupFixed,bandscopeHighSpinButton,150,180);

        bandscopeLowLabel=gtk_label_new("Bandscope Low");
        gtk_widget_show(bandscopeLowLabel);
        gtk_fixed_put((GtkFixed*)setupFixed,bandscopeLowLabel,10,210);
        bandscopeLowSpinButton=gtk_spin_button_new_with_range(-200,200,10);
        gtk_spin_button_set_value((GtkSpinButton*)bandscopeLowSpinButton,(double)bandscopeMIN);
        g_signal_connect(G_OBJECT(bandscopeLowSpinButton),"value-changed",G_CALLBACK(bandscopeLowChanged),NULL);
        gtk_widget_show(bandscopeLowSpinButton);
        gtk_fixed_put((GtkFixed*)setupFixed,bandscopeLowSpinButton,150,210);


        gtk_widget_set_size_request(GTK_WIDGET(setupFixed),400,400);
        gtk_widget_show(setupFixed);
        gtk_container_add(GTK_CONTAINER(setupWindow),setupFixed);

        gtk_widget_show(setupWindow);
    }

}

void spectrumHighChanged(GtkSpinButton* spinButton,gpointer data) {
    spectrumMAX=gtk_spin_button_get_value(spinButton);
}

void spectrumLowChanged(GtkSpinButton* spinButton,gpointer data) {
    spectrumMIN=gtk_spin_button_get_value(spinButton);
}

void spectrumStepChanged(GtkSpinButton* spinButton,gpointer data) {
    spectrumSTEP=gtk_spin_button_get_value(spinButton);
}

void waterfallHighChanged(GtkSpinButton* spinButton,gpointer data) {
    waterfallHighThreshold=gtk_spin_button_get_value(spinButton);
}

void waterfallLowChanged(GtkSpinButton* spinButton,gpointer data) {
    waterfallLowThreshold=gtk_spin_button_get_value(spinButton);
}

void bandscopeHighChanged(GtkSpinButton* spinButton,gpointer data) {
    bandscopeMAX=gtk_spin_button_get_value(spinButton);
}

void bandscopeLowChanged(GtkSpinButton* spinButton,gpointer data) {
    bandscopeMIN=gtk_spin_button_get_value(spinButton);
}

void quitSetup() {
    gtk_widget_destroy(setupWindow);
    setupWindow=NULL;
}
