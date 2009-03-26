//
// command.c
//
// write commands to DttSP
//

#include <gtk/gtk.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

extern void do_update(char* command,FILE* log);

void writeCommand(char *command) {
    char s[80];
//fprintf(stderr,"writeCommand: %s\n",command);
    sprintf(s,"%s\n",command);
    if(command!=NULL) {
        do_update(s,NULL);
    }
}
