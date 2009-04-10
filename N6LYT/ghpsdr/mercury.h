// mercury.h

gboolean Preamp;

float preampOffset;

GtkWidget* buildMercuryUI();
void mercurySaveState();
void mercuryRestoreState();
void setPreamp(gboolean state);

