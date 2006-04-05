// KD5TFD-VK6APH-Audio.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "KD5TFD-VK6APH-Audio.h"
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

// This is an example of an exported variable
KD5TFDVK6APHAUDIO_API int nKD5TFDVK6APHAudio=0;

// This is an example of an exported function.
KD5TFDVK6APHAUDIO_API int fnKD5TFDVK6APHAudio(void)
{
	return 42;
}


#if 0 
// This is the constructor of a class that has been exported.
// see KD5TFD-VK6APH-Audio.h for the class definition
CKD5TFDVK6APHAudio::CKD5TFDVK6APHAudio()
{ 
	return; 
}
#endif