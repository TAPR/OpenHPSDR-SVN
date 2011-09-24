/******************************************************************************/
/*                                                                            */
/*                          IoExample for PortTalk V2.0                       */
/*                        Version 2.0, 12th January 2002                      */
/*                          http://www.beyondlogic.org                        */
/*                                                                            */
/* Copyright © 2002 Craig Peacock. Craig.Peacock@beyondlogic.org              */
/* Any publication or distribution of this code in source form is prohibited  */
/* without prior written permission of the copyright holder. This source code */
/* is provided "as is", without any guarantee made as to its suitability or   */
/* fitness for any particular use. Permission is herby granted to modify or   */
/* enhance this sample code to produce a derivative program which may only be */
/* distributed in compiled object form only.                                  */
/******************************************************************************/

#include <stdio.h>
#include <windows.h>
#include <pt_ioctl.c>

void __cdecl main(void)
{
    unsigned char value;
    printf("IoExample for PortTalk V2.0\nCopyright 2001 Craig Peacock\nhttp://www.beyondlogic.org\n");
    OpenPortTalk();
    outportb(0x378, 0xFF);
    value = inportb(0x378);
    printf("Value returned = 0x%02X \n",value);
    outp(0x378, 0xAA);
    value = inp(0x378);
    printf("Value returned = 0x%02X \n",value);
    ClosePortTalk();
}