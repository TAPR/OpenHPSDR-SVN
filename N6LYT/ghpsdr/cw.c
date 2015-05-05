#include <stdio.h>
#include <math.h>

#include "cw.h"
#include "ozy.h"

void init_cw() {
    int i;

    cwinternal=1;
    cwkeysreversed = 0; // 0=disabled 1=enabled
    cwkeyerspeed = 12; // 1-60 WPM
    cwkeyermode = KEYER_STRAIGHT;
    
    cwkeyerweight = 30; //0-100
    cwkeyerspacing = 0; // 0=on 1=off 
    
    cwinternal = 1;  // 0=external, 1=internal 
    cwsidetonevolume = 127; // 0-127
    cwpttdelay = 20; // 0-255ms 
    cwhangtime = 10; // ms
    cwsidetonefrequency = 400; // Hz 

    double deltaf = 600.0/(double)sampleRate*2*M_PI;
    //double deltaf = M_PI / 40.0;     // (2 PI f / 48k) gives an 600 Hz note at 48 ksps
    for (i=0;i<240;++i) {
        cwSin[i] = (short)((double)(pow(2, 15) -1)  * sin(deltaf * (double)i));
        cwCos[i] = (short)((double)(pow(2, 15) -1)  * cos(deltaf * (double)i));
    }
    cwIndex=0;
}


double sineWave(float* buf, int samples, double phase, double freq) {
    double phase_step = 2*M_PI*freq/sampleRate;
    int i;

    for(i=0; i<samples; i++ ) {
        buf[i] = (float)sin(phase);
        phase += phase_step;
    }

    return phase;
}

/*
double sineWave(float* buf, int samples, double phase, double freq) {
    double phase_step = 2*M_PI*freq/sampleRate;
    double cosval = cos(phase);
    double sinval = sin(phase);
    double cosdelta = cos(phase_step);
    double sindelta = sin(phase_step);
    double tmpval;
    int i;

    for(i=0; i<samples; i++ ) {
        tmpval = cosval*cosdelta - sinval*sindelta;
        sinval = cosval*sindelta + sinval*cosdelta;
        cosval = tmpval;
                                                
        buf[i] = (float)(sinval);
                                                                                         
//fprintf(stderr,"%d=%f\n",i,buf[i]);
        phase += phase_step;
    }

    return phase;
}
*/

double cosineWave(float* buf, int samples, double phase, double freq) {
    double phase_step = freq/sampleRate*2*M_PI;
    double cosval = cos(phase);
    double sinval = sin(phase);
    double cosdelta = cos(phase_step);
    double sindelta = sin(phase_step);
    double tmpval;
    int i;

    for(i=0; i<samples; i++ ) {
        tmpval = cosval*cosdelta - sinval*sindelta;
        sinval = cosval*sindelta + sinval*cosdelta;
        cosval = tmpval;
                                                
        buf[i] = (float)(cosval);
                                                                                         
        phase += phase_step;
    }

    return phase;
}

