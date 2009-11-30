/** 
* @file property.c
* @brief Property functions
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-12
*/

/* Copyright (C) 
* 2009 - John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
* 
*/


#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include "property.h"

PROPERTY* properties;

/* --------------------------------------------------------------------------*/
/** 
* @brief Load Properties
* 
* @param filename
*/
void properties_load(char* filename) {
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
                property=malloc(sizeof(PROPERTY));
                property->name=malloc(strlen(name)+1);
                strcpy(property->name,name);
                property->value=malloc(strlen(value)+1);
                strcpy(property->value,value);
                property->next_property=properties;
                properties=property;
            }
        }
        fclose(f);
    }
}

/* --------------------------------------------------------------------------*/
/** 
* @brief Save Properties
* 
* @param filename
*/
void properties_save(char* filename) {
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

/* --------------------------------------------------------------------------*/
/** 
* @brief Get Properties
* 
* @param name
* 
* @return 
*/
char* property_get(char* name) {
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

/* --------------------------------------------------------------------------*/
/** 
* @brief Set Properties
* 
* @param name
* @param value
*/
void property_put(char* name,char* value) {
    PROPERTY* property=properties;
    while(property) {
        if(strcmp(name,property->name)==0) {
            break;
        }
        property=property->next_property;
    }
    if(property) {
        // just update
        free(property->value);
        property->value=malloc(strlen(value)+1);
        strcpy(property->value,value);
    } else {
        // new property
        property=malloc(sizeof(PROPERTY));
        property->name=malloc(strlen(name)+1);
        strcpy(property->name,name);
        property->value=malloc(strlen(value)+1);
        strcpy(property->value,value);
        property->next_property=properties;
        properties=property;
    }
}
