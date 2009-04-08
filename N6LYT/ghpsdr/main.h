// main.h

GdkColor background;
GdkColor buttonBackground;
GdkColor bandButtonBackground;
GdkColor buttonSelected;
GdkColor black;
GdkColor white;
GdkColor displayButton;
GdkColor green;
GdkColor red;
GdkColor grey;
GdkColor plotColor;
GdkColor filterColor;
GdkColor verticalColor;
GdkColor horizontalColor;
GdkColor spectrumTextColor;


char propertiesPath[128];

FILE* rxCommandFifo;
char rxCommandPath[128];
char rxMeterPath[128];
char rxSpectrumPath[128];

FILE* txCommandFifo;
char txCommandPath[128];
char txMeterPath[128];
char txSpectrumPath[128];

float preampOffset;


