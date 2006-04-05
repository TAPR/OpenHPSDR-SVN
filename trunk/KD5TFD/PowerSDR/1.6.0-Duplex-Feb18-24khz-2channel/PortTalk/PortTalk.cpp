/*   PortTalk Library Dll Creation Program  -- Copyright 2004 FlexRadio Systems  /*
 *   Written by Eric Wachsmann
 */

#include <windows.h>
#include "pt_ioctl.c"

#define API __declspec(dllexport)
#define CCONV __stdcall

int WINAPI DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
    switch (ul_reason_for_call)
	{
		case DLL_PROCESS_ATTACH:
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
		case DLL_PROCESS_DETACH:
			break;
    }
    return TRUE;
}

API void CCONV InitPortTalk()
{
	OpenPortTalk();
}

API void CCONV ExitPortTalk()
{
	ClosePortTalk();
}

API void CCONV outport(unsigned short PortAddress, unsigned char byte)
{
	outportb(PortAddress, byte);
}

API unsigned char CCONV inport(unsigned short PortAddress)
{
	return inportb(PortAddress);
}