#define MAX_RECEIVERS 2

#define BUFFER_SIZE 1024

#define BYTES_PER_SAMPLE 3

typedef struct _RECEIVER {
    int id;
    long address;
    unsigned short port;
    unsigned short spectrum_port;
    int  frequency_changed;
    long frequency;
    int keepalive;
    int audio_socket;
    pthread_t audio_thread_id;
    pthread_t watchdog_thread_id;
    int output_buffer_valid;
    float input_buffer[BUFFER_SIZE*2];
    float output_buffer[BUFFER_SIZE*2];
} RECEIVER;

RECEIVER receiver[MAX_RECEIVERS];
