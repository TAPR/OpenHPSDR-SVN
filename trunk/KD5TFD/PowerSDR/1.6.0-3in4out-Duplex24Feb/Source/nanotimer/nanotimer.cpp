// nanotimer.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "nanotimer.h"
BOOL APIENTRY DllMain( HANDLE hModule, 
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

#if 0 
// This is an example of an exported variable
NANOTIMER_API int nnanotimer=0;

// This is an example of an exported function.
NANOTIMER_API int fnnanotimer(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see nanotimer.h for the class definition
Cnanotimer::Cnanotimer()
{ 
	return; 
}

#endif