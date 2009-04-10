#include <stdio.h>
#include "soundcard.h"

int soundcard=UNSUPPORTED_CARD;

int sampleRate=48000;

float multimeterCalibrationOffset=0.0f;
float displayCalibrationOffset=0.0f;

void setSoundcard(int card) {
    soundcard=card;
    switch(soundcard) {
        case AUDIGY_2_ZS:
            multimeterCalibrationOffset=1.024933f;
            displayCalibrationOffset=-29.20928f;
            break;
        case MP3_PLUS:
            multimeterCalibrationOffset=-33.40224f;
            displayCalibrationOffset=-62.84578f;
            break;
        case EXTIGY:
            multimeterCalibrationOffset=-29.30501f;
            displayCalibrationOffset=-62.099f;
            break;
        case DELTA_44:
            multimeterCalibrationOffset=-25.13887f;
            displayCalibrationOffset=-57.467f;
            break;
        case FIREBOX:
            multimeterCalibrationOffset=-20.94611f;
            displayCalibrationOffset=-54.019f;
            break;
        case EDIROL_FA_66:
            multimeterCalibrationOffset=-46.82864f;
            displayCalibrationOffset=-80.429f;
            break;
        case UNSUPPORTED_CARD:
            multimeterCalibrationOffset=-52.43533f;
            displayCalibrationOffset=-82.62103f;
            break;
        case HPSDR:
            multimeterCalibrationOffset=0.0f;
            displayCalibrationOffset=0.0f;
            break;
    }

}

int getSoundcardId(char* name) {
    int id=UNSUPPORTED_CARD;
    if(strcmp(name,"SANTA_CRUZ")==0) {
        id=SANTA_CRUZ;
    } else if(strcmp(name,"AUDIGY_2_ZS")==0) {
        id=AUDIGY_2_ZS;
    } else if(strcmp(name,"MP3_PLUS")==0) {
        id=MP3_PLUS;
    } else if(strcmp(name,"EXTIGY")==0) {
        id=EXTIGY;
    } else if(strcmp(name,"DELTA_44")==0) {
        id=DELTA_44;
    } else if(strcmp(name,"FIREBOX")==0) {
        id=FIREBOX;
    } else if(strcmp(name,"EDIROL_FA_66")==0) {
        id=EDIROL_FA_66;
    } else if(strcmp(name,"HPSDR")==0) {
        id=HPSDR;
    }
    return id;
}
