#include <stdio.h>
#include <math.h>

#include "cw.h"

void init_cw() {
    int i;

    double deltaf = M_PI / 40.0;     // (2 PI f / 48k) gives an 600 Hz note at 48 ksps
    for (i=0;i<240;++i) {
        cwSin[i] = (short)((double)(pow(2, 15) -1)  * sin(deltaf * (double)i));
        cwCos[i] = (short)((double)(pow(2, 15) -1)  * cos(deltaf * (double)i));

        fprintf(stderr,"%d sin=%d cos=%d\n",i,cwSin[i],cwCos[i]);
    }

}
