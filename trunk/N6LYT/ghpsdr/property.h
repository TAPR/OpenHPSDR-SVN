// property.h

typedef struct _PROPERTY PROPERTY;

struct _PROPERTY {
    char* name;
    char* value;
    PROPERTY* next_property;
};

void loadProperties(char* filename);
char* getProperty(char* name);

