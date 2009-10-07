#include <stdio.h>
#include <stdlib.h>
#include <getopt.h>

#include "debug.h"
#include "listener.h"
#include "ozy.h"
#include "version.h"

struct option longOptions[] = {
    {"receivers",required_argument, 0, 0},
    {"samplerate",required_argument, 0, 1},
    {"preamp",required_argument, 0, 2},
    {"lt2208dither",required_argument, 0, 3},
    {"lt2208random",required_argument, 0, 4},
    {"source_10mhz_clock",required_argument, 0, 5},
    {"source_122_88mhz_clock",required_argument, 0, 6},
};

char* shortOptions="";

int optionIndex;

void processCommands(int argc,char** argv) {
    int c;
    int temp;

    while((c=getopt_long(argc,argv,shortOptions,longOptions,&optionIndex)!=EOF)) {
        switch(optionIndex) {
            case 0:
                receivers=atoi(optarg);
                break;
            case 1:
                temp=atoi(optarg);
                if(temp==48000) {
                    setSpeed(SPEED_48KHZ);
                } else if(temp==96000) {
                    setSpeed(SPEED_96KHZ);
                } else if(temp==192000) {
                    setSpeed(SPEED_192KHZ);
                }
                break;
            case 2:
                setPreamp(atoi(optarg));
                break;
            case 3:
                setLT2208Dither(atoi(optarg));
                break;
            case 4:
                setLT2208Random(atoi(optarg));
                break;
            case 5:
                set10MHzSource(atoi(optarg));
                break;
            case 6:
                set122MHzSource(atoi(optarg));
                break;
        }
    }
}


int main(int argc,char** argv) {

    fprintf(stderr,"ghpsdr server version %s\n",VERSION);

    processCommands(argc,argv);

    // initialize the ozy interface
    ozy_init();

    // start the ozy interface
    ozy_start();

    // listen for client commands
    listener();

    return 0;
}
