#include <math.h>
#include "ozy.h"
#include "sinewave.h"

double cwSignal(float* left_buf,float* right_buf, int samples, double phase, double freq) {
    double phase_step = freq/sampleRate*2.0*M_PI;
    double cosval = cos(phase);
    double sinval = sin(phase);
    double cosdelta = cos(phase_step);
    double sindelta = sin(phase_step);
    int i;

    for(i=0; i<samples; i++ )
    {
        left_buf[i] = (float)sin(phase);
        right_buf[i] = (float)cos(phase);
        phase += phase_step;
    }

    return phase;
}
