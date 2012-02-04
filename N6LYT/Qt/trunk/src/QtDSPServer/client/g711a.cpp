#include "g711a.h"

G711a::G711a()
{
    for (int i = 0; i < 256; i++) {
        int input = i ^ 85;
        int mantissa = (input & 15) << 4;
        int segment = (input & 112) >> 4;
        int value = mantissa + 8;
        if (segment >= 1) value += 256;
        if (segment > 1) value <<= (segment - 1);
        if ((input & 128) == 0) value = -value;
        decodetable[i]=(short)value;
    }

    for(int i=0;i<65536;i++) {
        short sample=(short)i;

        int sign=(sample&0x8000) >> 8;
        if(sign != 0){
            sample=(short)-sample;
            sign=0x80;
        }

        if(sample > 32635) sample = 32635;

        int exp=7;
        for(int expMask=0x4000;(sample&expMask)==0 && exp>0; exp--, expMask >>= 1) {
        }
        int mantis = (sample >> ((exp == 0) ? 4 : (exp + 3))) & 0x0f;
        unsigned char alaw = (unsigned char)(sign | exp << 4 | mantis);
        encodetable[i]=(unsigned char)(alaw^0xD5);
    }
}

unsigned char G711a::encode(short sample) {
    return encodetable[sample&0xFFFF];
}

short G711a::decode(unsigned char sample) {
    return decodetable[sample&0xFF];
}
