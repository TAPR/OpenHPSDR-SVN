#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include "property.h"

PROPERTY* properties;

void loadProperties(char* filename) {
    char string[80];
    char* name;
    char* value;
    FILE* f=fopen(filename,"r");
    properties=NULL;
    PROPERTY* property;
    if(f) {
        while(fgets(string,sizeof(string),f)) {
            if(string[0]!='#') {
                name=strtok(string,"=");
                value=strtok(NULL,"\n");
                property=calloc(1,sizeof(PROPERTY));
                property->name=calloc(strlen(name)+1,1);
                strcpy(property->name,name);
                property->value=calloc(strlen(value)+1,1);
                strcpy(property->value,value);
                property->next_property=properties;
                properties=property;
            }
        }
        fclose(f);
    }
}

void saveProperties(char* filename) {
    PROPERTY* property=properties;
    FILE* f=fopen(filename,"w+");
    char line[512];
    if(!f) {
        fprintf(stderr,"can't open %s\n",filename);
        exit(1);
    }
    while(property) {
        sprintf(line,"%s=%s\n",property->name,property->value);
        fwrite(line,1,strlen(line),f);
        property=property->next_property;
    }
    fclose(f);
}

char* getProperty(char* name) {
    char* value=NULL;
    PROPERTY* property=properties;
    while(property) {
        if(strcmp(name,property->name)==0) {
            value=property->value;
            break;
        }
        property=property->next_property;
    }
    return value;
}

void setProperty(char* name,char* value) {
    PROPERTY* property=properties;
    while(property) {
        if(strcmp(name,property->name)==0) {
            break;
        }
        property=property->next_property;
    }
    if(property) {
        // just update
        cfree(property->value);
        property->value=calloc(strlen(value)+1,1);
        strcpy(property->value,value);
    } else {
        // new property
        property=calloc(1,sizeof(PROPERTY));
        property->name=calloc(strlen(name)+1,1);
        strcpy(property->name,name);
        property->value=calloc(strlen(value)+1,1);
        strcpy(property->value,value);
        property->next_property=properties;
        properties=property;
    }
}
