// bandscope_update.h

#define BANDSCOPE_BUFFER_SIZE 4096
#define BANDSCOPE_UPDATES_PER_SECOND 15

int bandscopeUpdatesPerSecond;

void newBandscopeUpdate();
void stopBandscopeUpdate();
