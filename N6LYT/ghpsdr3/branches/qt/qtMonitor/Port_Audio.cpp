#include "Port_Audio.h"

#include <portaudio.h>


#define CHANNELS 1
#define SAMPLE_RATE 8000
#define SAMPLES_PER_BUFFER 480


Port_Audio::Port_Audio() {
    int rc = Pa_Initialize();
    if (rc != paNoError) {
        fprintf(stderr, "Pa_Initialize failed: %s\n", Pa_GetErrorText(rc));
        exit(1);
    }

    rc = Pa_OpenDefaultStream(&stream, 0, CHANNELS, paInt16, SAMPLE_RATE, SAMPLES_PER_BUFFER,
            NULL, NULL);
    if (rc != paNoError) {
        fprintf(stderr, "Pa_OpenDefaultStream failed: %s\n", Pa_GetErrorText(rc));
        exit(1);
    }

    rc = Pa_StartStream(stream);
    if (rc != paNoError) {
        fprintf(stderr, "Pa_StartStream failed: %s\n", Pa_GetErrorText(rc));
        exit(1);
    }
}

Port_Audio::~Port_Audio() {
}

void Port_Audio::process_audio(char* buffer) {
    char decoded_buffer[480 * 2];
    
    aLawDecode(buffer, decoded_buffer);

    int rc = Pa_WriteStream(stream, buffer, SAMPLES_PER_BUFFER);
    if (rc != 0) {
        fprintf(stderr, "error writing first audio_buffer %s (rc=%d)\n", Pa_GetErrorText(rc), rc);
    }
}

void Port_Audio::aLawDecode(char* buffer, char*decoded_buffer) {
    int i;
    short v;
    for (int inIx = 48, outIx = 0; inIx < 48 + 480; inIx++) {
        i = buffer[inIx]&0xFF;
        v = decodetable[i];
        // assumes BIGENDIAN
        decoded_buffer[outIx++] = (char) ((v >> 8)&0xFF);
        decoded_buffer[outIx++] = (char) (v & 0xFF);
    }

}

void Port_Audio::init_decodetable() {
    qDebug() << "init_decodetable";
    for (int i = 0; i < 256; i++) {
        int input = i ^ 85;
        int mantissa = (input & 15) << 4;
        int segment = (input & 112) >> 4;
        int value = mantissa + 8;
        if (segment >= 1) {
            value += 256;
        }
        if (segment > 1) {
            value <<= (segment - 1);
        }
        if ((input & 128) == 0) {
            value = -value;
        }
        decodetable[i] = (short) value;
    }
}
