// bandstack.h

#define BANDSTACKS 3

struct _BANDSTACK {
    long long frequencyA;
    int mode;
    int filter;
    int var1Low;
    int var1High;
    int var2Low;
    int var2High;
    int step;
};

typedef struct _BANDSTACK BANDSTACK;

