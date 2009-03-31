//
// frequency.h
//

struct frequency_info {
        long long minFrequency;
        long long maxFrequency;
        unsigned char* info;
        int band;
        int transmit;
    };


char* getFrequencyInfo(long long frequency);
int getBand(long long frequency);
