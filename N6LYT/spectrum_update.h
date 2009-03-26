// spectrum_update.h

#define SPEC_SEMI_RAW 0
#define SPEC_PRE_FILT 1
#define SPEC_POST_FILT 2
#define SPEC_POST_AGC 3
#define SPEC_POST_DET 4
#define SPEC_PREMOD 4

#define SPEC_MAG 0
#define SPEC_PWR 1

#define SPECTRUM_BUFFER_SIZE 4096
#define SPECTRUM_UPDATES_PER_SECOND 15

int spectrumUpdatesPerSecond;

void newSpectrumUpdate();
void stopSpectrumUpdate();
