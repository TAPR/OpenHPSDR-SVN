// Copyright (C) Bill Tracey (KD5TFD) 2006 
// email: bill@ewjt.com 

// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the KD5TFDVK6APHAUDIO_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// KD5TFDVK6APHAUDIO_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.

#ifdef KD5TFDVK6APHAUDIO_EXPORTS
#define KD5TFDVK6APHAUDIO_API __declspec(dllexport)
#else
#define KD5TFDVK6APHAUDIO_API __declspec(dllimport)
#endif




#if 0 
// This class is exported from the KD5TFD-VK6APH-Audio.dll
class KD5TFDVK6APHAUDIO_API CKD5TFDVK6APHAudio {
public:
	CKD5TFDVK6APHAudio(void);
	// TODO: add your methods here.
};

extern KD5TFDVK6APHAUDIO_API int nKD5TFDVK6APHAudio;

KD5TFDVK6APHAUDIO_API int fnKD5TFDVK6APHAudio(void);
#endif 


extern KD5TFDVK6APHAUDIO_API int I2C_Cmd(unsigned char addr, unsigned char cmdbuf[], unsigned char cmdlen); 
extern KD5TFDVK6APHAUDIO_API char * I2C_RCtoString(int rc);


extern KD5TFDVK6APHAUDIO_API struct OzyHandle *OzyOpen(void); 
extern KD5TFDVK6APHAUDIO_API void OzyClose(struct OzyHandle *h); 
extern KD5TFDVK6APHAUDIO_API void *OzyHandleToRealHandle(struct OzyHandle *ozh);
