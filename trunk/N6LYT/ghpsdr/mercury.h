// mercury.h

gboolean Preamp;

GtkWidget* buildMercuryUI();
void mercurySaveState();
void mercuryRestoreState();
void setPreamp(gboolean state);

