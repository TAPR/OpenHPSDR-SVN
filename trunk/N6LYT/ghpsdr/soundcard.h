// soundcard.h

#define SANTA_CRUZ 0
#define AUDIGY_2_ZS 1
#define MP3_PLUS 2
#define EXTIGY 3
#define DELTA_44 4
#define FIREBOX 5
#define EDIROL_FA_66 6
#define UNSUPPORTED_CARD 7
#define HPSDR 8

char soundCardName[80];

int soundcard;
int sampleRate;

float multimeterCalibrationOffset;
float displayCalibrationOffset;

int getSoundcardId();

