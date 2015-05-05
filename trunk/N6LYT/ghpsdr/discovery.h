#include "discovered.h"

#define MAX_DEVICES 16

int selected_device;
int devices;
DISCOVERED discovered[MAX_DEVICES];

void discovery(void);
