#include "g711u.h"

G711u::G711u()
{
    for(int i=0;i<256; i++) {
        short input=(short)(~i);
        short mantissa=(short)(((input & 15)<<3)+132);
        short segment=(short)((input & 112)>>4);
        short value=(short)(mantissa<<segment);
        value-=132;
        if((input & 128)==0) value=(short)-value;
        decodetable[i]=value;
    }

    for(int i=0;i<65536;i++) {
        short sample=(short)i;

        int sign=sample & 0x8000;
        if(sign != 0){
            sample=(short)-sample;
            sign=0x80;
        }

        if(sample > 32635) sample = 32635;

        sample += 132;

        int exp;
        short temp = (short)(sample << 1);
        for(exp = 7; exp > 0; exp--){
            if((temp & 0x8000) != 0) break;
            temp = (short)(temp << 1);
        }

        temp = (short)(sample >> (exp + 3));
        int mantis = temp & 0x000f;
        unsigned char ulawByte = (unsigned char)(sign | (exp << 4) | mantis);
        encodetable[i]=(unsigned char)~ulawByte;
    }
}

unsigned char G711u::encode(short sample) {
    return encodetable[sample&0xFFFF];
}

short G711u::decode(unsigned char sample) {
    return decodetable[sample&0xFF];
}
