/*
 * $Id: pa_win_ds.c,v 1.1.2.51 2006/01/26 01:13:18 rossbencina Exp $
 * Portable Audio I/O Library DirectSound implementation
 *
 * Based on the Open Source API proposed by Ross Bencina
 * Copyright (c) 1999-2002 Ross Bencina, Phil Burk
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files
 * (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * Any person wishing to distribute modifications to the Software is
 * requested to send the modifications to the original developer so that
 * they can be incorporated into the canonical version.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR
 * ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

/** @file

    @todo implement paInputOverflow callback status flag
    
    @todo implement paNeverDropInput.

    @todo implement host api specific extension to set i/o buffer sizes in frames

    @todo implement initialisation of PaDeviceInfo default*Latency fields (currently set to 0.)

    @todo implement ReadStream, WriteStream, GetStreamReadAvailable, GetStreamWriteAvailable

    @todo audit handling of DirectSound result codes - in many cases we could convert a HRESULT into
        a native portaudio error code. Standard DirectSound result codes are documented at msdn.

    @todo implement IsFormatSupported

    @todo check that CoInitialize() CoUninitialize() are always correctly
        paired, even in error cases.

    @todo call PaUtil_SetLastHostErrorInfo with a specific error string (currently just "DSound error").

    @todo make sure all buffers have been played before stopping the stream
        when the stream callback returns paComplete

    old TODOs from phil, need to work out if these have been done:
        O- fix "patest_stop.c"
*/

#include <stdio.h>
#include <string.h> /* strlen() */

#include "pa_util.h"
#include "pa_allocation.h"
#include "pa_hostapi.h"
#include "pa_stream.h"
#include "pa_cpuload.h"
#include "pa_process.h"

#include "dsound_wrapper.h"

#if (defined(WIN32) && (defined(_MSC_VER) && (_MSC_VER >= 1200))) /* MSC version 6 and above */
#pragma comment( lib, "dsound.lib" )
#pragma comment( lib, "winmm.lib" )
#endif

/*
 provided in newer platform sdks and x64
 */
#ifndef DWORD_PTR
#define DWORD_PTR DWORD
#endif

#define PRINT(x) PA_DEBUG(x);
#define ERR_RPT(x) PRINT(x)
#define DBUG(x)   PRINT(x)
#define DBUGX(x)  PRINT(x)

#define PA_USE_HIGH_LATENCY   (0)
#if PA_USE_HIGH_LATENCY
#define PA_WIN_9X_LATENCY     (500)
#define PA_WIN_NT_LATENCY     (600)
#else
#define PA_WIN_9X_LATENCY     (140)
#define PA_WIN_NT_LATENCY     (280)
#endif

#define PA_WIN_WDM_LATENCY       (120)

#define SECONDS_PER_MSEC      (0.001)
#define MSEC_PER_SECOND       (1000)

/* prototypes for functions declared in this file */

#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

PaError PaWinDs_Initialize( PaUtilHostApiRepresentation **hostApi, PaHostApiIndex index );

#ifdef __cplusplus
}
#endif /* __cplusplus */

static void Terminate( struct PaUtilHostApiRepresentation *hostApi );
static PaError OpenStream( struct PaUtilHostApiRepresentation *hostApi,
                           PaStream** s,
                           const PaStreamParameters *inputParameters,
                           const PaStreamParameters *outputParameters,
                           double sampleRate,
                           unsigned long framesPerBuffer,
                           PaStreamFlags streamFlags,
                           PaStreamCallback *streamCallback,
                           void *userData );
static PaError IsFormatSupported( struct PaUtilHostApiRepresentation *hostApi,
                                  const PaStreamParameters *inputParameters,
                                  const PaStreamParameters *outputParameters,
                                  double sampleRate );
static PaError CloseStream( PaStream* stream );
static PaError StartStream( PaStream *stream );
static PaError StopStream( PaStream *stream );
static PaError AbortStream( PaStream *stream );
static PaError IsStreamStopped( PaStream *s );
static PaError IsStreamActive( PaStream *stream );
static PaTime GetStreamTime( PaStream *stream );
static double GetStreamCpuLoad( PaStream* stream );
static PaError ReadStream( PaStream* stream, void *buffer, unsigned long frames );
static PaError WriteStream( PaStream* stream, const void *buffer, unsigned long frames );
static signed long GetStreamReadAvailable( PaStream* stream );
static signed long GetStreamWriteAvailable( PaStream* stream );


/* FIXME: should convert hr to a string */
#define PA_DS_SET_LAST_DIRECTSOUND_ERROR( hr ) \
    PaUtil_SetLastHostErrorInfo( paDirectSound, hr, "DirectSound error" )

/************************************************* DX Prototypes **********/
static BOOL CALLBACK CollectGUIDsProc(LPGUID lpGUID,
                                     LPCTSTR lpszDesc,
                                     LPCTSTR lpszDrvName,
                                     LPVOID lpContext );

/************************************************************************************/
/********************** Structures **************************************************/
/************************************************************************************/
/* PaWinDsHostApiRepresentation - host api datastructure specific to this implementation */

typedef struct PaWinDsDeviceInfo
{
    GUID                             guid;
    GUID                            *lpGUID;
    double                           sampleRates[3];
} PaWinDsDeviceInfo;

typedef struct
{
    PaUtilHostApiRepresentation inheritedHostApiRep;
    PaUtilStreamInterface    callbackStreamInterface;
    PaUtilStreamInterface    blockingStreamInterface;

    PaUtilAllocationGroup   *allocations;

    /* implementation specific data goes here */
    PaWinDsDeviceInfo       *winDsDeviceInfos;

} PaWinDsHostApiRepresentation;

/* PaWinDsStream - a stream data structure specifically for this implementation */

typedef struct PaWinDsStream
{
    PaUtilStreamRepresentation streamRepresentation;
    PaUtilCpuLoadMeasurer cpuLoadMeasurer;
    PaUtilBufferProcessor bufferProcessor;

/* DirectSound specific data. */
    DSoundWrapper    directSoundWrapper;
    MMRESULT         timerID;
    BOOL             ifInsideCallback;  /* Test for reentrancy. */
    int              framesPerDSBuffer;
    double           framesWritten;
    double           secondsPerHostByte; /* Used to optimize latency calculation for outTime */

    PaStreamCallbackFlags callbackFlags;
    
/* FIXME - move all below to PaUtilStreamRepresentation */
    volatile int     isStarted;
    volatile int     isActive;
    volatile int     stopProcessing; /* stop thread once existing buffers have been returned */
    volatile int     abortProcessing; /* stop thread immediately */
} PaWinDsStream;


/************************************************************************************
** Duplicate the input string using the allocations allocator.
** A NULL string is converted to a zero length string.
** If memory cannot be allocated, NULL is returned.
**/
static char *DuplicateDeviceNameString( PaUtilAllocationGroup *allocations, const char* src )
{
    char *result = 0;
    
    if( src != NULL )
    {
        size_t len = strlen(src);
        result = (char*)PaUtil_GroupAllocateMemory( allocations, (long)(len + 1) );
        if( result )
            memcpy( (void *) result, src, len+1 );
    }
    else
    {
        result = (char*)PaUtil_GroupAllocateMemory( allocations, 1 );
        if( result )
            result[0] = '\0';
    }

    return result;
}

/************************************************************************************
** DSDeviceNameAndGUID, DSDeviceNameAndGUIDVector used for collecting preliminary
** information during device enumeration.
*/
typedef struct DSDeviceNameAndGUID{
    char *name; // allocated from parent's allocations, never deleted by this structure
    GUID guid;
    LPGUID lpGUID;
} DSDeviceNameAndGUID;

typedef struct DSDeviceNameAndGUIDVector{
    PaUtilAllocationGroup *allocations;
    PaError enumerationError;

    int count;
    int free;
    DSDeviceNameAndGUID *items; // Allocated using LocalAlloc()
} DSDeviceNameAndGUIDVector;

static PaError InitializeDSDeviceNameAndGUIDVector(
        DSDeviceNameAndGUIDVector *guidVector, PaUtilAllocationGroup *allocations )
{
    PaError result = paNoError;

    guidVector->allocations = allocations;
    guidVector->enumerationError = paNoError;

    guidVector->count = 0;
    guidVector->free = 8;
    guidVector->items = (DSDeviceNameAndGUID*)LocalAlloc( LMEM_FIXED, sizeof(DSDeviceNameAndGUID) * guidVector->free );
    if( guidVector->items == NULL )
        result = paInsufficientMemory;
    
    return result;
}

static PaError ExpandDSDeviceNameAndGUIDVector( DSDeviceNameAndGUIDVector *guidVector )
{
    PaError result = paNoError;
    DSDeviceNameAndGUID *newItems;
    int i;
    
    /* double size of vector */
    int size = guidVector->count + guidVector->free;
    guidVector->free += size;

    newItems = (DSDeviceNameAndGUID*)LocalAlloc( LMEM_FIXED, sizeof(DSDeviceNameAndGUID) * size * 2 );
    if( newItems == NULL )
    {
        result = paInsufficientMemory;
    }
    else
    {
        for( i=0; i < guidVector->count; ++i )
        {
            newItems[i].name = guidVector->items[i].name;
            if( guidVector->items[i].lpGUID == NULL )
            {
                newItems[i].lpGUID = NULL;
            }
            else
            {
                newItems[i].lpGUID = &newItems[i].guid;
                memcpy( &newItems[i].guid, guidVector->items[i].lpGUID, sizeof(GUID) );;
            }
        }

        LocalFree( guidVector->items );
        guidVector->items = newItems;
    }                                

    return result;
}

/*
    it's safe to call DSDeviceNameAndGUIDVector multiple times
*/
static PaError TerminateDSDeviceNameAndGUIDVector( DSDeviceNameAndGUIDVector *guidVector )
{
    PaError result = paNoError;

    if( guidVector->items != NULL )
    {
        if( LocalFree( guidVector->items ) != NULL )
            result = paInsufficientMemory;              /** @todo this isn't the correct error to return from a deallocation failure */

        guidVector->items = NULL;
    }

    return result;
}

/************************************************************************************
** Collect preliminary device information during DirectSound enumeration 
*/
static BOOL CALLBACK CollectGUIDsProc(LPGUID lpGUID,
                                     LPCTSTR lpszDesc,
                                     LPCTSTR lpszDrvName,
                                     LPVOID lpContext )
{
    DSDeviceNameAndGUIDVector *namesAndGUIDs = (DSDeviceNameAndGUIDVector*)lpContext;
    PaError error;

    (void) lpszDrvName; /* unused variable */

    if( namesAndGUIDs->free == 0 )
    {
        error = ExpandDSDeviceNameAndGUIDVector( namesAndGUIDs );
        if( error != paNoError )
        {
            namesAndGUIDs->enumerationError = error;
            return FALSE;
        }
    }
    
    /* Set GUID pointer, copy GUID to storage in DSDeviceNameAndGUIDVector. */
    if( lpGUID == NULL )
    {
        namesAndGUIDs->items[namesAndGUIDs->count].lpGUID = NULL;
    }
    else
    {
        namesAndGUIDs->items[namesAndGUIDs->count].lpGUID =
                &namesAndGUIDs->items[namesAndGUIDs->count].guid;
      
        memcpy( &namesAndGUIDs->items[namesAndGUIDs->count].guid, lpGUID, sizeof(GUID) );
    }

    namesAndGUIDs->items[namesAndGUIDs->count].name =
            DuplicateDeviceNameString( namesAndGUIDs->allocations, lpszDesc );
    if( namesAndGUIDs->items[namesAndGUIDs->count].name == NULL )
    {
        namesAndGUIDs->enumerationError = paInsufficientMemory;
        return FALSE;
    }

    ++namesAndGUIDs->count;
    --namesAndGUIDs->free;
    
    return TRUE;
}


/* 
    GUIDs for emulated devices which we blacklist below.
    are there more than two of them??
*/

GUID IID_IRolandVSCEmulated1 = {0xc2ad1800, 0xb243, 0x11ce, 0xa8, 0xa4, 0x00, 0xaa, 0x00, 0x6c, 0x45, 0x01};
GUID IID_IRolandVSCEmulated2 = {0xc2ad1800, 0xb243, 0x11ce, 0xa8, 0xa4, 0x00, 0xaa, 0x00, 0x6c, 0x45, 0x02};


#define PA_DEFAULTSAMPLERATESEARCHORDER_COUNT_  (13) /* must match array length below */
static double defaultSampleRateSearchOrder_[] =
    { 44100.0, 48000.0, 32000.0, 24000.0, 22050.0, 88200.0, 96000.0, 192000.0,
        16000.0, 12000.0, 11025.0, 9600.0, 8000.0 };


/************************************************************************************
** Extract capabilities from an output device, and add it to the device info list
** if successful. This function assumes that there is enough room in the
** device info list to accomodate all entries.
**
** The device will not be added to the device list if any errors are encountered.
*/
static PaError AddOutputDeviceInfoFromDirectSound(
        PaWinDsHostApiRepresentation *winDsHostApi, char *name, LPGUID lpGUID )
{
    PaUtilHostApiRepresentation  *hostApi = &winDsHostApi->inheritedHostApiRep;
    PaDeviceInfo                 *deviceInfo = hostApi->deviceInfos[hostApi->info.deviceCount];
    PaWinDsDeviceInfo            *winDsDeviceInfo = &winDsHostApi->winDsDeviceInfos[hostApi->info.deviceCount];
    HRESULT                       hr;
    LPDIRECTSOUND                 lpDirectSound;
    DSCAPS                        caps;
    int                           deviceOK = TRUE;
    PaError                       result = paNoError;
    int                           i;
    
    /* Copy GUID to the device info structure. Set pointer. */
    if( lpGUID == NULL )
    {
        winDsDeviceInfo->lpGUID = NULL;
    }
    else
    {
        memcpy( &winDsDeviceInfo->guid, lpGUID, sizeof(GUID) );
        winDsDeviceInfo->lpGUID = &winDsDeviceInfo->guid;
    }

    
    /*if( lpGUID )
    {
        if (IsEqualGUID (&IID_IRolandVSCEmulated1,lpGUID) ||
            IsEqualGUID (&IID_IRolandVSCEmulated2,lpGUID) )
        {
            PA_DEBUG(("BLACKLISTED: %s \n",name));
            return paNoError;
        }
    }*/

    /* Create a DirectSound object for the specified GUID
        Note that using CoCreateInstance doesn't work on windows CE.
    */
    hr = dswDSoundEntryPoints.DirectSoundCreate( lpGUID, &lpDirectSound, NULL );

    /** try using CoCreateInstance because DirectSoundCreate was hanging under
        some circumstances - note this was probably related to the
        #define BOOL short bug which has now been fixed
        @todo delete this comment and the following code once we've ensured
        there is no bug.
    */
    /*
    hr = CoCreateInstance( &CLSID_DirectSound, NULL, CLSCTX_INPROC_SERVER,
            &IID_IDirectSound, (void**)&lpDirectSound );

    if( hr == S_OK )
    {
        hr = IDirectSound_Initialize( lpDirectSound, lpGUID );
    }
    */
    
    if( hr != DS_OK )
    {
        /*if (hr == DSERR_ALLOCATED)
            PA_DEBUG(("AddOutputDeviceInfoFromDirectSound %s DSERR_ALLOCATED\n",name));
        DBUG(("Cannot create DirectSound for %s. Result = 0x%x\n", name, hr ));
        if (lpGUID)
            DBUG(("%s's GUID: {0x%x,0x%x,0x%x,0x%x,0x%x,0x%x,0x%x,0x%x,0x%x,0x%x, 0x%x} \n",
                 name,
                 lpGUID->Data1,
                 lpGUID->Data2,
                 lpGUID->Data3,
                 lpGUID->Data4[0],
                 lpGUID->Data4[1],
                 lpGUID->Data4[2],
                 lpGUID->Data4[3],
                 lpGUID->Data4[4],
                 lpGUID->Data4[5],
                 lpGUID->Data4[6],
                 lpGUID->Data4[7]));

        deviceOK = FALSE;*/
    }
    else
    {
        /* Query device characteristics. */
        memset( &caps, 0, sizeof(caps) ); 
        caps.dwSize = sizeof(caps);
        hr = IDirectSound_GetCaps( lpDirectSound, &caps );
        if( hr != DS_OK )
        {
            DBUG(("Cannot GetCaps() for DirectSound device %s. Result = 0x%x\n", name, hr ));
            deviceOK = FALSE;
        }
        else
        {

#ifndef PA_NO_WMME
            if( caps.dwFlags & DSCAPS_EMULDRIVER )
            {
                /* If WMME supported, then reject Emulated drivers because they are lousy. */
                deviceOK = FALSE;
            }
#endif

            if( deviceOK )
            {
                deviceInfo->maxInputChannels = 0;
                /* Mono or stereo device? */
                deviceInfo->maxOutputChannels = ( caps.dwFlags & DSCAPS_PRIMARYSTEREO ) ? 2 : 1;

                deviceInfo->defaultLowInputLatency = 0.;    /** @todo IMPLEMENT ME */
                deviceInfo->defaultLowOutputLatency = 0.;   /** @todo IMPLEMENT ME */
                deviceInfo->defaultHighInputLatency = 0.;   /** @todo IMPLEMENT ME */
                deviceInfo->defaultHighOutputLatency = 0.;  /** @todo IMPLEMENT ME */
                
                /* initialize defaultSampleRate */
                
                if( caps.dwFlags & DSCAPS_CONTINUOUSRATE )
                {
                    /* initialize to caps.dwMaxSecondarySampleRate incase none of the standard rates match */
                    deviceInfo->defaultSampleRate = caps.dwMaxSecondarySampleRate;

                    for( i = 0; i < PA_DEFAULTSAMPLERATESEARCHORDER_COUNT_; ++i )
                    {
                        if( defaultSampleRateSearchOrder_[i] >= caps.dwMinSecondarySampleRate
                                && defaultSampleRateSearchOrder_[i] <= caps.dwMaxSecondarySampleRate ){

                            deviceInfo->defaultSampleRate = defaultSampleRateSearchOrder_[i];
                            break;
                        }
                    }
                }
                else if( caps.dwMinSecondarySampleRate == caps.dwMaxSecondarySampleRate )
                {
                    if( caps.dwMinSecondarySampleRate == 0 )
                    {
                        /*
                        ** On my Thinkpad 380Z, DirectSoundV6 returns min-max=0 !!
                        ** But it supports continuous sampling.
                        ** So fake range of rates, and hope it really supports it.
                        */
                        deviceInfo->defaultSampleRate = 44100.0f;

                        DBUG(("PA - Reported rates both zero. Setting to fake values for device #%s\n", name ));
                    }
                    else
                    {
	                    deviceInfo->defaultSampleRate = caps.dwMaxSecondarySampleRate;
                    }
                }
                else if( (caps.dwMinSecondarySampleRate < 1000.0) && (caps.dwMaxSecondarySampleRate > 50000.0) )
                {
                    /* The EWS88MT drivers lie, lie, lie. The say they only support two rates, 100 & 100000.
                    ** But we know that they really support a range of rates!
                    ** So when we see a ridiculous set of rates, assume it is a range.
                    */
                  deviceInfo->defaultSampleRate = 44100.0f;
                  DBUG(("PA - Sample rate range used instead of two odd values for device #%s\n", name ));
                }
                else deviceInfo->defaultSampleRate = caps.dwMaxSecondarySampleRate;


                //printf( "min %d max %d\n", caps.dwMinSecondarySampleRate, caps.dwMaxSecondarySampleRate );
                // dwFlags | DSCAPS_CONTINUOUSRATE 
            }
        }

        IDirectSound_Release( lpDirectSound );
    }

    if( deviceOK )
    {
        deviceInfo->name = name;

        if( lpGUID == NULL )
            hostApi->info.defaultOutputDevice = hostApi->info.deviceCount;
            
        hostApi->info.deviceCount++;
    }

    return result;
}


/************************************************************************************
** Extract capabilities from an input device, and add it to the device info list
** if successful. This function assumes that there is enough room in the
** device info list to accomodate all entries.
**
** The device will not be added to the device list if any errors are encountered.
*/
static PaError AddInputDeviceInfoFromDirectSoundCapture(
        PaWinDsHostApiRepresentation *winDsHostApi, char *name, LPGUID lpGUID )
{
    PaUtilHostApiRepresentation  *hostApi = &winDsHostApi->inheritedHostApiRep;
    PaDeviceInfo                 *deviceInfo = hostApi->deviceInfos[hostApi->info.deviceCount];
    PaWinDsDeviceInfo            *winDsDeviceInfo = &winDsHostApi->winDsDeviceInfos[hostApi->info.deviceCount];
    HRESULT                       hr;
    LPDIRECTSOUNDCAPTURE          lpDirectSoundCapture;
    DSCCAPS                       caps;
    int                           deviceOK = TRUE;
    PaError                       result = paNoError;
    
    /* Copy GUID to the device info structure. Set pointer. */
    if( lpGUID == NULL )
    {
        winDsDeviceInfo->lpGUID = NULL;
    }
    else
    {
        winDsDeviceInfo->lpGUID = &winDsDeviceInfo->guid;
        memcpy( &winDsDeviceInfo->guid, lpGUID, sizeof(GUID) );
    }


    hr = dswDSoundEntryPoints.DirectSoundCaptureCreate( lpGUID, &lpDirectSoundCapture, NULL );

    /** try using CoCreateInstance because DirectSoundCreate was hanging under
        some circumstances - note this was probably related to the
        #define BOOL short bug which has now been fixed
        @todo delete this comment and the following code once we've ensured
        there is no bug.
    */
    /*
    hr = CoCreateInstance( &CLSID_DirectSoundCapture, NULL, CLSCTX_INPROC_SERVER,
            &IID_IDirectSoundCapture, (void**)&lpDirectSoundCapture );
    */
    if( hr != DS_OK )
    {
        DBUG(("Cannot create Capture for %s. Result = 0x%x\n", name, hr ));
        deviceOK = FALSE;
    }
    else
    {
        /* Query device characteristics. */
        memset( &caps, 0, sizeof(caps) );
        caps.dwSize = sizeof(caps);
        hr = IDirectSoundCapture_GetCaps( lpDirectSoundCapture, &caps );
        if( hr != DS_OK )
        {
            DBUG(("Cannot GetCaps() for Capture device %s. Result = 0x%x\n", name, hr ));
            deviceOK = FALSE;
        }
        else
        {
#ifndef PA_NO_WMME
            if( caps.dwFlags & DSCAPS_EMULDRIVER )
            {
                /* If WMME supported, then reject Emulated drivers because they are lousy. */
                deviceOK = FALSE;
            }
#endif

            if( deviceOK )
            {
                deviceInfo->maxInputChannels = caps.dwChannels;
                deviceInfo->maxOutputChannels = 0;

                deviceInfo->defaultLowInputLatency = 0.;    /** @todo IMPLEMENT ME */
                deviceInfo->defaultLowOutputLatency = 0.;   /** @todo IMPLEMENT ME */
                deviceInfo->defaultHighInputLatency = 0.;   /** @todo IMPLEMENT ME */
                deviceInfo->defaultHighOutputLatency = 0.;  /** @todo IMPLEMENT ME */

/*  constants from a WINE patch by Francois Gouget, see:
    http://www.winehq.com/hypermail/wine-patches/2003/01/0290.html

    ---
    Date: Fri, 14 May 2004 10:38:12 +0200 (CEST)
    From: Francois Gouget <fgouget@ ... .fr>
    To: Ross Bencina <rbencina@ ... .au>
    Subject: Re: Permission to use wine 48/96 wave patch in BSD licensed library

    [snip]

    I give you permission to use the patch below under the BSD license.
    http://www.winehq.com/hypermail/wine-patches/2003/01/0290.html

    [snip]
*/
#ifndef WAVE_FORMAT_48M08
#define WAVE_FORMAT_48M08      0x00001000    /* 48     kHz, Mono,   8-bit  */
#define WAVE_FORMAT_48S08      0x00002000    /* 48     kHz, Stereo, 8-bit  */
#define WAVE_FORMAT_48M16      0x00004000    /* 48     kHz, Mono,   16-bit */
#define WAVE_FORMAT_48S16      0x00008000    /* 48     kHz, Stereo, 16-bit */
#define WAVE_FORMAT_96M08      0x00010000    /* 96     kHz, Mono,   8-bit  */
#define WAVE_FORMAT_96S08      0x00020000    /* 96     kHz, Stereo, 8-bit  */
#define WAVE_FORMAT_96M16      0x00040000    /* 96     kHz, Mono,   16-bit */
#define WAVE_FORMAT_96S16      0x00080000    /* 96     kHz, Stereo, 16-bit */
#endif

                /* defaultSampleRate */
                if( caps.dwChannels == 2 )
                {
                    if( caps.dwFormats & WAVE_FORMAT_4S16 )
                        deviceInfo->defaultSampleRate = 44100.0;
                    else if( caps.dwFormats & WAVE_FORMAT_48S16 )
                        deviceInfo->defaultSampleRate = 48000.0;
                    else if( caps.dwFormats & WAVE_FORMAT_2S16 )
                        deviceInfo->defaultSampleRate = 22050.0;
                    else if( caps.dwFormats & WAVE_FORMAT_1S16 )
                        deviceInfo->defaultSampleRate = 11025.0;
                    else if( caps.dwFormats & WAVE_FORMAT_96S16 )
                        deviceInfo->defaultSampleRate = 96000.0;
                    else
                        deviceInfo->defaultSampleRate = 0.;
                }
                else if( caps.dwChannels == 1 )
                {
                    if( caps.dwFormats & WAVE_FORMAT_4M16 )
                        deviceInfo->defaultSampleRate = 44100.0;
                    else if( caps.dwFormats & WAVE_FORMAT_48M16 )
                        deviceInfo->defaultSampleRate = 48000.0;
                    else if( caps.dwFormats & WAVE_FORMAT_2M16 )
                        deviceInfo->defaultSampleRate = 22050.0;
                    else if( caps.dwFormats & WAVE_FORMAT_1M16 )
                        deviceInfo->defaultSampleRate = 11025.0;
                    else if( caps.dwFormats & WAVE_FORMAT_96M16 )
                        deviceInfo->defaultSampleRate = 96000.0;
                    else
                        deviceInfo->defaultSampleRate = 0.;
                }
                else deviceInfo->defaultSampleRate = 0.;
            }
        }
        
        IDirectSoundCapture_Release( lpDirectSoundCapture );
    }

    if( deviceOK )
    {
        deviceInfo->name = name;

        if( lpGUID == NULL )
            hostApi->info.defaultInputDevice = hostApi->info.deviceCount;

        hostApi->info.deviceCount++;
    }

    return result;
}


/***********************************************************************************/
PaError PaWinDs_Initialize( PaUtilHostApiRepresentation **hostApi, PaHostApiIndex hostApiIndex )
{
    PaError result = paNoError;
    int i, deviceCount;
    PaWinDsHostApiRepresentation *winDsHostApi;
    DSDeviceNameAndGUIDVector inputNamesAndGUIDs, outputNamesAndGUIDs;
    PaDeviceInfo *deviceInfoArray;

    HRESULT hr = CoInitialize(NULL);        /** @todo: should uninitialize too */
    if( FAILED(hr) ){
        return paUnanticipatedHostError;
    }            

    /* initialise guid vectors so they can be safely deleted on error */
    inputNamesAndGUIDs.items = NULL;
    outputNamesAndGUIDs.items = NULL;

    DSW_InitializeDSoundEntryPoints();

    winDsHostApi = (PaWinDsHostApiRepresentation*)PaUtil_AllocateMemory( sizeof(PaWinDsHostApiRepresentation) );
    if( !winDsHostApi )
    {
        result = paInsufficientMemory;
        goto error;
    }

    winDsHostApi->allocations = PaUtil_CreateAllocationGroup();
    if( !winDsHostApi->allocations )
    {
        result = paInsufficientMemory;
        goto error;
    }

    *hostApi = &winDsHostApi->inheritedHostApiRep;
    (*hostApi)->info.structVersion = 1;
    (*hostApi)->info.type = paDirectSound;
    (*hostApi)->info.name = "Windows DirectSound";
    
    (*hostApi)->info.deviceCount = 0;
    (*hostApi)->info.defaultInputDevice = paNoDevice;
    (*hostApi)->info.defaultOutputDevice = paNoDevice;

    
/* DSound - enumerate devices to count them and to gather their GUIDs */


    result = InitializeDSDeviceNameAndGUIDVector( &inputNamesAndGUIDs, winDsHostApi->allocations );
    if( result != paNoError )
        goto error;

    result = InitializeDSDeviceNameAndGUIDVector( &outputNamesAndGUIDs, winDsHostApi->allocations );
    if( result != paNoError )
        goto error;

    dswDSoundEntryPoints.DirectSoundCaptureEnumerate( (LPDSENUMCALLBACK)CollectGUIDsProc, (void *)&inputNamesAndGUIDs );

    dswDSoundEntryPoints.DirectSoundEnumerate( (LPDSENUMCALLBACK)CollectGUIDsProc, (void *)&outputNamesAndGUIDs );

    if( inputNamesAndGUIDs.enumerationError != paNoError )
    {
        result = inputNamesAndGUIDs.enumerationError;
        goto error;
    }

    if( outputNamesAndGUIDs.enumerationError != paNoError )
    {
        result = outputNamesAndGUIDs.enumerationError;
        goto error;
    }

    deviceCount = inputNamesAndGUIDs.count + outputNamesAndGUIDs.count;

    if( deviceCount > 0 )
    {
        /* allocate array for pointers to PaDeviceInfo structs */
        (*hostApi)->deviceInfos = (PaDeviceInfo**)PaUtil_GroupAllocateMemory(
                winDsHostApi->allocations, sizeof(PaDeviceInfo*) * deviceCount );
        if( !(*hostApi)->deviceInfos )
        {
            result = paInsufficientMemory;
            goto error;
        }

        /* allocate all PaDeviceInfo structs in a contiguous block */
        deviceInfoArray = (PaDeviceInfo*)PaUtil_GroupAllocateMemory(
                winDsHostApi->allocations, sizeof(PaDeviceInfo) * deviceCount );
        if( !deviceInfoArray )
        {
            result = paInsufficientMemory;
            goto error;
        }

        /* allocate all DSound specific info structs in a contiguous block */
        winDsHostApi->winDsDeviceInfos = (PaWinDsDeviceInfo*)PaUtil_GroupAllocateMemory(
                winDsHostApi->allocations, sizeof(PaWinDsDeviceInfo) * deviceCount );
        if( !winDsHostApi->winDsDeviceInfos )
        {
            result = paInsufficientMemory;
            goto error;
        }

        for( i=0; i < deviceCount; ++i )
        {
            PaDeviceInfo *deviceInfo = &deviceInfoArray[i];
            deviceInfo->structVersion = 2;
            deviceInfo->hostApi = hostApiIndex;
            deviceInfo->name = 0;
            (*hostApi)->deviceInfos[i] = deviceInfo;
        }

        for( i=0; i< inputNamesAndGUIDs.count; ++i )
        {
            result = AddInputDeviceInfoFromDirectSoundCapture( winDsHostApi,
                    inputNamesAndGUIDs.items[i].name,
                    inputNamesAndGUIDs.items[i].lpGUID );
            if( result != paNoError )
                goto error;
        }

        for( i=0; i< outputNamesAndGUIDs.count; ++i )
        {
            result = AddOutputDeviceInfoFromDirectSound( winDsHostApi,
                    outputNamesAndGUIDs.items[i].name,
                    outputNamesAndGUIDs.items[i].lpGUID );
            if( result != paNoError )
                goto error;
        }
    }    

    result = TerminateDSDeviceNameAndGUIDVector( &inputNamesAndGUIDs );
    if( result != paNoError )
        goto error;

    result = TerminateDSDeviceNameAndGUIDVector( &outputNamesAndGUIDs );
    if( result != paNoError )
        goto error;

    
    (*hostApi)->Terminate = Terminate;
    (*hostApi)->OpenStream = OpenStream;
    (*hostApi)->IsFormatSupported = IsFormatSupported;

    PaUtil_InitializeStreamInterface( &winDsHostApi->callbackStreamInterface, CloseStream, StartStream,
                                      StopStream, AbortStream, IsStreamStopped, IsStreamActive,
                                      GetStreamTime, GetStreamCpuLoad,
                                      PaUtil_DummyRead, PaUtil_DummyWrite,
                                      PaUtil_DummyGetReadAvailable, PaUtil_DummyGetWriteAvailable );

    PaUtil_InitializeStreamInterface( &winDsHostApi->blockingStreamInterface, CloseStream, StartStream,
                                      StopStream, AbortStream, IsStreamStopped, IsStreamActive,
                                      GetStreamTime, PaUtil_DummyGetCpuLoad,
                                      ReadStream, WriteStream, GetStreamReadAvailable, GetStreamWriteAvailable );

    return result;

error:
    if( winDsHostApi )
    {
        if( winDsHostApi->allocations )
        {
            PaUtil_FreeAllAllocations( winDsHostApi->allocations );
            PaUtil_DestroyAllocationGroup( winDsHostApi->allocations );
        }
                
        PaUtil_FreeMemory( winDsHostApi );
    }

    TerminateDSDeviceNameAndGUIDVector( &inputNamesAndGUIDs );
    TerminateDSDeviceNameAndGUIDVector( &outputNamesAndGUIDs );

    return result;
}


/***********************************************************************************/
static void Terminate( struct PaUtilHostApiRepresentation *hostApi )
{
    PaWinDsHostApiRepresentation *winDsHostApi = (PaWinDsHostApiRepresentation*)hostApi;

    /*
        IMPLEMENT ME:
            - clean up any resources not handled by the allocation group
    */

    if( winDsHostApi->allocations )
    {
        PaUtil_FreeAllAllocations( winDsHostApi->allocations );
        PaUtil_DestroyAllocationGroup( winDsHostApi->allocations );
    }

    PaUtil_FreeMemory( winDsHostApi );

    DSW_TerminateDSoundEntryPoints();

    CoUninitialize();
}


/* Set minimal latency based on whether NT or Win95.
 * NT has higher latency.
 */
static int PaWinDS_GetMinSystemLatency( void )
{
    int minLatencyMsec;
    /* Set minimal latency based on whether NT or other OS.
     * NT has higher latency.
     */
    OSVERSIONINFO osvi;
	osvi.dwOSVersionInfoSize = sizeof( osvi );
	GetVersionEx( &osvi );
    DBUG(("PA - PlatformId = 0x%x\n", osvi.dwPlatformId ));
    DBUG(("PA - MajorVersion = 0x%x\n", osvi.dwMajorVersion ));
    DBUG(("PA - MinorVersion = 0x%x\n", osvi.dwMinorVersion ));
    /* Check for NT */
	if( (osvi.dwMajorVersion == 4) && (osvi.dwPlatformId == 2) )
	{
		minLatencyMsec = PA_WIN_NT_LATENCY;
	}
	else if(osvi.dwMajorVersion >= 5)
	{
		minLatencyMsec = PA_WIN_WDM_LATENCY;
	}
	else
	{
		minLatencyMsec = PA_WIN_9X_LATENCY;
	}
    return minLatencyMsec;
}

/***********************************************************************************/
static PaError IsFormatSupported( struct PaUtilHostApiRepresentation *hostApi,
                                  const PaStreamParameters *inputParameters,
                                  const PaStreamParameters *outputParameters,
                                  double sampleRate )
{
    int inputChannelCount, outputChannelCount;
    PaSampleFormat inputSampleFormat, outputSampleFormat;
    
    if( inputParameters )
    {
        inputChannelCount = inputParameters->channelCount;
        inputSampleFormat = inputParameters->sampleFormat;

        /* unless alternate device specification is supported, reject the use of
            paUseHostApiSpecificDeviceSpecification */

        if( inputParameters->device == paUseHostApiSpecificDeviceSpecification )
            return paInvalidDevice;

        /* check that input device can support inputChannelCount */
        if( inputChannelCount > hostApi->deviceInfos[ inputParameters->device ]->maxInputChannels )
            return paInvalidChannelCount;

        /* validate inputStreamInfo */
        if( inputParameters->hostApiSpecificStreamInfo )
            return paIncompatibleHostApiSpecificStreamInfo; /* this implementation doesn't use custom stream info */
    }
    else
    {
        inputChannelCount = 0;
    }

    if( outputParameters )
    {
        outputChannelCount = outputParameters->channelCount;
        outputSampleFormat = outputParameters->sampleFormat;
        
        /* unless alternate device specification is supported, reject the use of
            paUseHostApiSpecificDeviceSpecification */

        if( outputParameters->device == paUseHostApiSpecificDeviceSpecification )
            return paInvalidDevice;

        /* check that output device can support inputChannelCount */
        if( outputChannelCount > hostApi->deviceInfos[ outputParameters->device ]->maxOutputChannels )
            return paInvalidChannelCount;

        /* validate outputStreamInfo */
        if( outputParameters->hostApiSpecificStreamInfo )
            return paIncompatibleHostApiSpecificStreamInfo; /* this implementation doesn't use custom stream info */
    }
    else
    {
        outputChannelCount = 0;
    }
    
    /*
        IMPLEMENT ME:

            - if a full duplex stream is requested, check that the combination
                of input and output parameters is supported if necessary

            - check that the device supports sampleRate

        Because the buffer adapter handles conversion between all standard
        sample formats, the following checks are only required if paCustomFormat
        is implemented, or under some other unusual conditions.

            - check that input device can support inputSampleFormat, or that
                we have the capability to convert from outputSampleFormat to
                a native format

            - check that output device can support outputSampleFormat, or that
                we have the capability to convert from outputSampleFormat to
                a native format
    */

    return paFormatIsSupported;
}


/*************************************************************************
** Determine minimum number of buffers required for this host based
** on minimum latency. Latency can be optionally set by user by setting
** an environment variable. For example, to set latency to 200 msec, put:
**
**    set PA_MIN_LATENCY_MSEC=200
**
** in the AUTOEXEC.BAT file and reboot.
** If the environment variable is not set, then the latency will be determined
** based on the OS. Windows NT has higher latency than Win95.
*/
#define PA_LATENCY_ENV_NAME  ("PA_MIN_LATENCY_MSEC")
#define PA_ENV_BUF_SIZE  (32)

static int PaWinDs_GetMinLatencyFrames( double sampleRate )
{
    char      envbuf[PA_ENV_BUF_SIZE];
    DWORD     hresult;
    int       minLatencyMsec = 0;

    /* Let user determine minimal latency by setting environment variable. */
    hresult = GetEnvironmentVariable( PA_LATENCY_ENV_NAME, envbuf, PA_ENV_BUF_SIZE );
    if( (hresult > 0) && (hresult < PA_ENV_BUF_SIZE) )
    {
        minLatencyMsec = atoi( envbuf );
    }
    else
    {
        minLatencyMsec = PaWinDS_GetMinSystemLatency();
#if PA_USE_HIGH_LATENCY
        PRINT(("PA - Minimum Latency set to %d msec!\n", minLatencyMsec ));
#endif

    }

    return (int) (minLatencyMsec * sampleRate * SECONDS_PER_MSEC);
}

/***********************************************************************************/
/* see pa_hostapi.h for a list of validity guarantees made about OpenStream parameters */

static PaError OpenStream( struct PaUtilHostApiRepresentation *hostApi,
                           PaStream** s,
                           const PaStreamParameters *inputParameters,
                           const PaStreamParameters *outputParameters,
                           double sampleRate,
                           unsigned long framesPerBuffer,
                           PaStreamFlags streamFlags,
                           PaStreamCallback *streamCallback,
                           void *userData )
{
    PaError result = paNoError;
    PaWinDsHostApiRepresentation *winDsHostApi = (PaWinDsHostApiRepresentation*)hostApi;
    PaWinDsStream *stream = 0;
    int inputChannelCount, outputChannelCount;
    PaSampleFormat inputSampleFormat, outputSampleFormat;
    PaSampleFormat hostInputSampleFormat, hostOutputSampleFormat;
    unsigned long suggestedInputLatencyFrames, suggestedOutputLatencyFrames;

    if( inputParameters )
    {
        inputChannelCount = inputParameters->channelCount;
        inputSampleFormat = inputParameters->sampleFormat;
        suggestedInputLatencyFrames = (unsigned long)(inputParameters->suggestedLatency * sampleRate);

        /* IDEA: the following 3 checks could be performed by default by pa_front
            unless some flag indicated otherwise */
            
        /* unless alternate device specification is supported, reject the use of
            paUseHostApiSpecificDeviceSpecification */
        if( inputParameters->device == paUseHostApiSpecificDeviceSpecification )
            return paInvalidDevice;

        /* check that input device can support inputChannelCount */
        if( inputChannelCount > hostApi->deviceInfos[ inputParameters->device ]->maxInputChannels )
            return paInvalidChannelCount;
            
        /* validate hostApiSpecificStreamInfo */
        if( inputParameters->hostApiSpecificStreamInfo )
            return paIncompatibleHostApiSpecificStreamInfo; /* this implementation doesn't use custom stream info */
    }
    else
    {
        inputChannelCount = 0;
        suggestedInputLatencyFrames = 0;
    }


    if( outputParameters )
    {
        outputChannelCount = outputParameters->channelCount;
        outputSampleFormat = outputParameters->sampleFormat;
        suggestedOutputLatencyFrames = (unsigned long)(outputParameters->suggestedLatency * sampleRate);

        /* unless alternate device specification is supported, reject the use of
            paUseHostApiSpecificDeviceSpecification */
        if( outputParameters->device == paUseHostApiSpecificDeviceSpecification )
            return paInvalidDevice;

        /* check that output device can support inputChannelCount */
        if( outputChannelCount > hostApi->deviceInfos[ outputParameters->device ]->maxOutputChannels )
            return paInvalidChannelCount;

        /* validate hostApiSpecificStreamInfo */
        if( outputParameters->hostApiSpecificStreamInfo )
            return paIncompatibleHostApiSpecificStreamInfo; /* this implementation doesn't use custom stream info */            
    }
    else
    {
        outputChannelCount = 0;
        suggestedOutputLatencyFrames = 0;
    }


    /*
        IMPLEMENT ME:

        ( the following two checks are taken care of by PaUtil_InitializeBufferProcessor() )

            - check that input device can support inputSampleFormat, or that
                we have the capability to convert from outputSampleFormat to
                a native format

            - check that output device can support outputSampleFormat, or that
                we have the capability to convert from outputSampleFormat to
                a native format

            - if a full duplex stream is requested, check that the combination
                of input and output parameters is supported

            - check that the device supports sampleRate

            - alter sampleRate to a close allowable rate if possible / necessary

            - validate suggestedInputLatency and suggestedOutputLatency parameters,
                use default values where necessary
    */


    /* validate platform specific flags */
    if( (streamFlags & paPlatformSpecificFlags) != 0 )
        return paInvalidFlag; /* unexpected platform specific flag */


    stream = (PaWinDsStream*)PaUtil_AllocateMemory( sizeof(PaWinDsStream) );
    if( !stream )
    {
        result = paInsufficientMemory;
        goto error;
    }

    if( streamCallback )
    {
        PaUtil_InitializeStreamRepresentation( &stream->streamRepresentation,
                                               &winDsHostApi->callbackStreamInterface, streamCallback, userData );
    }
    else
    {
        PaUtil_InitializeStreamRepresentation( &stream->streamRepresentation,
                                               &winDsHostApi->blockingStreamInterface, streamCallback, userData );
    }
    
    PaUtil_InitializeCpuLoadMeasurer( &stream->cpuLoadMeasurer, sampleRate );


    if( inputParameters )
    {
        /* IMPLEMENT ME - establish which  host formats are available */
        hostInputSampleFormat =
            PaUtil_SelectClosestAvailableFormat( paInt16 /* native formats */, inputParameters->sampleFormat );
    }

    if( outputParameters )
    {
        /* IMPLEMENT ME - establish which  host formats are available */
        hostOutputSampleFormat =
            PaUtil_SelectClosestAvailableFormat( paInt16 /* native formats */, outputParameters->sampleFormat );
    }
    
    result =  PaUtil_InitializeBufferProcessor( &stream->bufferProcessor,
                    inputChannelCount, inputSampleFormat, hostInputSampleFormat,
                    outputChannelCount, outputSampleFormat, hostOutputSampleFormat,
                    sampleRate, streamFlags, framesPerBuffer,
                    framesPerBuffer, /* ignored in paUtilVariableHostBufferSizePartialUsageAllowed mode. */
                /* This next mode is required because DS can split the host buffer when it wraps around. */
                    paUtilVariableHostBufferSizePartialUsageAllowed,
                    streamCallback, userData );
    if( result != paNoError )
        goto error;


    stream->streamRepresentation.streamInfo.inputLatency =
            PaUtil_GetBufferProcessorInputLatency(&stream->bufferProcessor);   /* FIXME: not initialised anywhere else */
    stream->streamRepresentation.streamInfo.outputLatency =
            PaUtil_GetBufferProcessorOutputLatency(&stream->bufferProcessor);    /* FIXME: not initialised anywhere else */
    stream->streamRepresentation.streamInfo.sampleRate = sampleRate;

    
/* DirectSound specific initialization */
    {
        HRESULT          hr;
        int              bytesPerDirectSoundBuffer;
        DSoundWrapper   *dsw;
        int              userLatencyFrames;
        int              minLatencyFrames;

        stream->timerID = 0;
        dsw = &stream->directSoundWrapper;
        DSW_Init( dsw );

    /* Get system minimum latency. */
        minLatencyFrames = PaWinDs_GetMinLatencyFrames( sampleRate );

    /* Let user override latency by passing latency parameter. */
        userLatencyFrames = (suggestedInputLatencyFrames > suggestedOutputLatencyFrames)
                    ? suggestedInputLatencyFrames
                    : suggestedOutputLatencyFrames;
        if( userLatencyFrames > 0 ) minLatencyFrames = userLatencyFrames;

    /* Calculate stream->framesPerDSBuffer depending on framesPerBuffer */
        if( framesPerBuffer == paFramesPerBufferUnspecified )
        {
        /* App support variable framesPerBuffer */
            stream->framesPerDSBuffer = minLatencyFrames;

            stream->streamRepresentation.streamInfo.outputLatency = (double)(minLatencyFrames - 1) / sampleRate;
        }
        else
        {
        /* Round up to number of buffers needed to guarantee that latency. */
            int numUserBuffers = (minLatencyFrames + framesPerBuffer - 1) / framesPerBuffer;
            if( numUserBuffers < 1 ) numUserBuffers = 1;
            numUserBuffers += 1; /* So we have latency worth of buffers ahead of current buffer. */
            stream->framesPerDSBuffer = framesPerBuffer * numUserBuffers;

            stream->streamRepresentation.streamInfo.outputLatency = (double)(framesPerBuffer * (numUserBuffers-1)) / sampleRate;
        }

        {
            /** @todo REVIEW: this calculation seems incorrect to me - rossb. */
            int msecLatency = (int) ((stream->framesPerDSBuffer * MSEC_PER_SECOND) / sampleRate);
            PRINT(("PortAudio on DirectSound - Latency = %d frames, %d msec\n", stream->framesPerDSBuffer, msecLatency ));
        }


        /* ------------------ OUTPUT */
        if( outputParameters )
        {
            /*
            PaDeviceInfo *deviceInfo = hostApi->deviceInfos[ outputParameters->device ];
            DBUG(("PaHost_OpenStream: deviceID = 0x%x\n", outputParameters->device));
            */
            
            bytesPerDirectSoundBuffer = stream->framesPerDSBuffer * outputParameters->channelCount * sizeof(short);
            if( bytesPerDirectSoundBuffer < DSBSIZE_MIN )
            {
                result = paBufferTooSmall;
                goto error;
            }
            else if( bytesPerDirectSoundBuffer > DSBSIZE_MAX )
            {
                result = paBufferTooBig;
                goto error;
            }


            hr = dswDSoundEntryPoints.DirectSoundCreate( winDsHostApi->winDsDeviceInfos[outputParameters->device].lpGUID,
                &dsw->dsw_pDirectSound,   NULL );
            if( hr != DS_OK )
            {
                ERR_RPT(("PortAudio: DirectSoundCreate() failed!\n"));
                result = paUnanticipatedHostError;
                PA_DS_SET_LAST_DIRECTSOUND_ERROR( hr );
                goto error;
            }
            hr = DSW_InitOutputBuffer( dsw,
                                       (unsigned long) (sampleRate + 0.5),
                                       (WORD)outputParameters->channelCount, bytesPerDirectSoundBuffer );
            DBUG(("DSW_InitOutputBuffer() returns %x\n", hr));
            if( hr != DS_OK )
            {
                result = paUnanticipatedHostError;
                PA_DS_SET_LAST_DIRECTSOUND_ERROR( hr );
                goto error;
            }
            /* Calculate value used in latency calculation to avoid real-time divides. */
            stream->secondsPerHostByte = 1.0 /
                (stream->bufferProcessor.bytesPerHostOutputSample *
                outputChannelCount * sampleRate);
        }

        /* ------------------ INPUT */
        if( inputParameters )
        {
            /*
            PaDeviceInfo *deviceInfo = hostApi->deviceInfos[ inputParameters->device ];
            DBUG(("PaHost_OpenStream: deviceID = 0x%x\n", inputParameters->device));
            */
            
            bytesPerDirectSoundBuffer = stream->framesPerDSBuffer * inputParameters->channelCount * sizeof(short);
            if( bytesPerDirectSoundBuffer < DSBSIZE_MIN )
            {
                result = paBufferTooSmall;
                goto error;
            }
            else if( bytesPerDirectSoundBuffer > DSBSIZE_MAX )
            {
                result = paBufferTooBig;
                goto error;
            }

            hr = dswDSoundEntryPoints.DirectSoundCaptureCreate( winDsHostApi->winDsDeviceInfos[inputParameters->device].lpGUID,
                &dsw->dsw_pDirectSoundCapture,   NULL );
            if( hr != DS_OK )
            {
                ERR_RPT(("PortAudio: DirectSoundCaptureCreate() failed!\n"));
                result = paUnanticipatedHostError;
                PA_DS_SET_LAST_DIRECTSOUND_ERROR( hr );
                goto error;
            }
            hr = DSW_InitInputBuffer( dsw,
                                      (unsigned long) (sampleRate + 0.5),
                                      (WORD)inputParameters->channelCount, bytesPerDirectSoundBuffer );
            DBUG(("DSW_InitInputBuffer() returns %x\n", hr));
            if( hr != DS_OK )
            {
                ERR_RPT(("PortAudio: DSW_InitInputBuffer() returns %x\n", hr));
                result = paUnanticipatedHostError;
                PA_DS_SET_LAST_DIRECTSOUND_ERROR( hr );
                goto error;
            }
        }

    }

    *s = (PaStream*)stream;

    return result;

error:
    if( stream )
        PaUtil_FreeMemory( stream );

    return result;
}


/***********************************************************************************/
static PaError Pa_TimeSlice( PaWinDsStream *stream )
{
    PaError           result = 0;   /* FIXME: this should be declared int and this function should also return that type (same as stream callback return type)*/
    DSoundWrapper    *dsw;
    long              numFrames = 0;
    long              bytesEmpty = 0;
    long              bytesFilled = 0;
    long              bytesToXfer = 0;
    long              framesToXfer = 0;
    long              numInFramesReady = 0;
    long              numOutFramesReady = 0;
    long              bytesProcessed;
    HRESULT           hresult;
    double            outputLatency = 0;
    PaStreamCallbackTimeInfo timeInfo = {0,0,0}; /** @todo implement inputBufferAdcTime */
    
/* Input */
    LPBYTE            lpInBuf1 = NULL;
    LPBYTE            lpInBuf2 = NULL;
    DWORD             dwInSize1 = 0;
    DWORD             dwInSize2 = 0;
/* Output */
    LPBYTE            lpOutBuf1 = NULL;
    LPBYTE            lpOutBuf2 = NULL;
    DWORD             dwOutSize1 = 0;
    DWORD             dwOutSize2 = 0;

    dsw = &stream->directSoundWrapper;

    /* How much input data is available? */
    if( stream->bufferProcessor.inputChannelCount > 0 )
    {
        DSW_QueryInputFilled( dsw, &bytesFilled );
        framesToXfer = numInFramesReady = bytesFilled / dsw->dsw_BytesPerInputFrame;
        outputLatency = ((double)bytesFilled) * stream->secondsPerHostByte;

        /** @todo Check for overflow */
    }

    /* How much output room is available? */
    if( stream->bufferProcessor.outputChannelCount > 0 )
    {
        UINT previousUnderflowCount = dsw->dsw_OutputUnderflows;
        DSW_QueryOutputSpace( dsw, &bytesEmpty );
        framesToXfer = numOutFramesReady = bytesEmpty / dsw->dsw_BytesPerOutputFrame;

        /* Check for underflow */
        if( dsw->dsw_OutputUnderflows != previousUnderflowCount )
            stream->callbackFlags |= paOutputUnderflow;
    }

    if( (numInFramesReady > 0) && (numOutFramesReady > 0) )
    {
        framesToXfer = (numOutFramesReady < numInFramesReady) ? numOutFramesReady : numInFramesReady;
    }

    if( framesToXfer > 0 )
    {

        PaUtil_BeginCpuLoadMeasurement( &stream->cpuLoadMeasurer );

    /* The outputBufferDacTime parameter should indicates the time at which
        the first sample of the output buffer is heard at the DACs. */
        timeInfo.currentTime = PaUtil_GetTime();
        timeInfo.outputBufferDacTime = timeInfo.currentTime + outputLatency;


        PaUtil_BeginBufferProcessing( &stream->bufferProcessor, &timeInfo, stream->callbackFlags );
        stream->callbackFlags = 0;
        
    /* Input */
        if( stream->bufferProcessor.inputChannelCount > 0 )
        {
            bytesToXfer = framesToXfer * dsw->dsw_BytesPerInputFrame;
            hresult = IDirectSoundCaptureBuffer_Lock ( dsw->dsw_InputBuffer,
                dsw->dsw_ReadOffset, bytesToXfer,
                (void **) &lpInBuf1, &dwInSize1,
                (void **) &lpInBuf2, &dwInSize2, 0);
            if (hresult != DS_OK)
            {
                ERR_RPT(("DirectSound IDirectSoundCaptureBuffer_Lock failed, hresult = 0x%x\n",hresult));
                result = paUnanticipatedHostError;
                PA_DS_SET_LAST_DIRECTSOUND_ERROR( hresult );
                goto error2;
            }

            numFrames = dwInSize1 / dsw->dsw_BytesPerInputFrame;
            PaUtil_SetInputFrameCount( &stream->bufferProcessor, numFrames );
            PaUtil_SetInterleavedInputChannels( &stream->bufferProcessor, 0, lpInBuf1, 0 );
        /* Is input split into two regions. */
            if( dwInSize2 > 0 )
            {
                numFrames = dwInSize2 / dsw->dsw_BytesPerInputFrame;
                PaUtil_Set2ndInputFrameCount( &stream->bufferProcessor, numFrames );
                PaUtil_Set2ndInterleavedInputChannels( &stream->bufferProcessor, 0, lpInBuf2, 0 );
            }
        }

    /* Output */
        if( stream->bufferProcessor.outputChannelCount > 0 )
        {
            bytesToXfer = framesToXfer * dsw->dsw_BytesPerOutputFrame;
            hresult = IDirectSoundBuffer_Lock ( dsw->dsw_OutputBuffer,
                dsw->dsw_WriteOffset, bytesToXfer,
                (void **) &lpOutBuf1, &dwOutSize1,
                (void **) &lpOutBuf2, &dwOutSize2, 0);
            if (hresult != DS_OK)
            {
                ERR_RPT(("DirectSound IDirectSoundBuffer_Lock failed, hresult = 0x%x\n",hresult));
                result = paUnanticipatedHostError;
                PA_DS_SET_LAST_DIRECTSOUND_ERROR( hresult );
                goto error1;
            }

            numFrames = dwOutSize1 / dsw->dsw_BytesPerOutputFrame;
            PaUtil_SetOutputFrameCount( &stream->bufferProcessor, numFrames );
            PaUtil_SetInterleavedOutputChannels( &stream->bufferProcessor, 0, lpOutBuf1, 0 );

        /* Is output split into two regions. */
            if( dwOutSize2 > 0 )
            {
                numFrames = dwOutSize2 / dsw->dsw_BytesPerOutputFrame;
                PaUtil_Set2ndOutputFrameCount( &stream->bufferProcessor, numFrames );
                PaUtil_Set2ndInterleavedOutputChannels( &stream->bufferProcessor, 0, lpOutBuf2, 0 );
            }
        }

        result = paContinue;
        numFrames = PaUtil_EndBufferProcessing( &stream->bufferProcessor, &result );
        stream->framesWritten += numFrames;
        
        if( stream->bufferProcessor.outputChannelCount > 0 )
        {
        /* FIXME: an underflow could happen here */

        /* Update our buffer offset and unlock sound buffer */
            bytesProcessed = numFrames * dsw->dsw_BytesPerOutputFrame;
            dsw->dsw_WriteOffset = (dsw->dsw_WriteOffset + bytesProcessed) % dsw->dsw_OutputSize;
            IDirectSoundBuffer_Unlock( dsw->dsw_OutputBuffer, lpOutBuf1, dwOutSize1, lpOutBuf2, dwOutSize2);
            dsw->dsw_FramesWritten += numFrames;
        }

error1:
        if( stream->bufferProcessor.inputChannelCount > 0 )
        {
        /* FIXME: an overflow could happen here */

        /* Update our buffer offset and unlock sound buffer */
            bytesProcessed = numFrames * dsw->dsw_BytesPerInputFrame;
            dsw->dsw_ReadOffset = (dsw->dsw_ReadOffset + bytesProcessed) % dsw->dsw_InputSize;
            IDirectSoundCaptureBuffer_Unlock( dsw->dsw_InputBuffer, lpInBuf1, dwInSize1, lpInBuf2, dwInSize2);
        }
error2:

        PaUtil_EndCpuLoadMeasurement( &stream->cpuLoadMeasurer, numFrames );

    }
    
    return result;
}
/*******************************************************************/
static void CALLBACK Pa_TimerCallback(UINT uID, UINT uMsg, DWORD_PTR dwUser, DWORD dw1, DWORD dw2)
{
    PaWinDsStream *stream;

    /* suppress unused variable warnings */
    (void) uID;
    (void) uMsg;
    (void) dw1;
    (void) dw2;
    
    stream = (PaWinDsStream *) dwUser;
    if( stream == NULL ) return;

    if( stream->isActive )
    {
        if( stream->abortProcessing )
        {
            stream->isActive = 0;
        }
        else if( stream->stopProcessing )
        {
            DSoundWrapper   *dsw = &stream->directSoundWrapper;
            if( stream->bufferProcessor.outputChannelCount > 0 )
            {
                DSW_ZeroEmptySpace( dsw );
                /* clear isActive when all sound played */
                if( dsw->dsw_FramesPlayed >= stream->framesWritten )
                {
                    stream->isActive = 0;
                }
            }
            else
            {
                stream->isActive = 0;
            }
        }
        else
        {
            if( Pa_TimeSlice( stream ) != 0)  /* Call time slice independant of timing method. */
            {
                /* FIXME implement handling of paComplete and paAbort if possible */
                stream->stopProcessing = 1;
            }
        }

        if( !stream->isActive ){
            if( stream->streamRepresentation.streamFinishedCallback != 0 )
                stream->streamRepresentation.streamFinishedCallback( stream->streamRepresentation.userData );
        }
    }
}

/***********************************************************************************
    When CloseStream() is called, the multi-api layer ensures that
    the stream has already been stopped or aborted.
*/
static PaError CloseStream( PaStream* s )
{
    PaError result = paNoError;
    PaWinDsStream *stream = (PaWinDsStream*)s;

    DSW_Term( &stream->directSoundWrapper );

    PaUtil_TerminateBufferProcessor( &stream->bufferProcessor );
    PaUtil_TerminateStreamRepresentation( &stream->streamRepresentation );
    PaUtil_FreeMemory( stream );

    return result;
}

/***********************************************************************************/
static PaError StartStream( PaStream *s )
{
    PaError          result = paNoError;
    PaWinDsStream   *stream = (PaWinDsStream*)s;
    HRESULT          hr;

    PaUtil_ResetBufferProcessor( &stream->bufferProcessor );
    
    if( stream->bufferProcessor.inputChannelCount > 0 )
    {
        hr = DSW_StartInput( &stream->directSoundWrapper );
        DBUG(("StartStream: DSW_StartInput returned = 0x%X.\n", hr));
        if( hr != DS_OK )
        {
            result = paUnanticipatedHostError;
            PA_DS_SET_LAST_DIRECTSOUND_ERROR( hr );
            goto error;
        }
    }

    stream->framesWritten = 0;
    stream->callbackFlags = 0;

    stream->abortProcessing = 0;
    stream->stopProcessing = 0;
    stream->isActive = 1;

    if( stream->bufferProcessor.outputChannelCount > 0 )
    {
        /* Give user callback a chance to pre-fill buffer. REVIEW - i thought we weren't pre-filling, rb. */
        result = Pa_TimeSlice( stream );
        if( result != paNoError ) return result; // FIXME - what if finished?

        hr = DSW_StartOutput( &stream->directSoundWrapper );
        DBUG(("PaHost_StartOutput: DSW_StartOutput returned = 0x%X.\n", hr));
        if( hr != DS_OK )
        {
            result = paUnanticipatedHostError;
            PA_DS_SET_LAST_DIRECTSOUND_ERROR( hr );
            goto error;
        }
    }


    /* Create timer that will wake us up so we can fill the DSound buffer. */
    {
        int resolution;
        int framesPerWakeup = stream->framesPerDSBuffer / 4;
        int msecPerWakeup = MSEC_PER_SECOND * framesPerWakeup / (int) stream->streamRepresentation.streamInfo.sampleRate;
        if( msecPerWakeup < 10 ) msecPerWakeup = 10;
        else if( msecPerWakeup > 100 ) msecPerWakeup = 100;
        resolution = msecPerWakeup/4;
        stream->timerID = timeSetEvent( msecPerWakeup, resolution, (LPTIMECALLBACK) Pa_TimerCallback,
                                             (DWORD_PTR) stream, TIME_PERIODIC );
    }
    if( stream->timerID == 0 )
    {
        stream->isActive = 0;
        result = paUnanticipatedHostError;
        PA_DS_SET_LAST_DIRECTSOUND_ERROR( hr );
        goto error;
    }

    stream->isStarted = TRUE;

error:
    return result;
}


/***********************************************************************************/
static PaError StopStream( PaStream *s )
{
    PaError result = paNoError;
    PaWinDsStream *stream = (PaWinDsStream*)s;
    HRESULT          hr;
    int timeoutMsec;

    stream->stopProcessing = 1;
    /* Set timeout at 20% beyond maximum time we might wait. */
    timeoutMsec = (int) (1200.0 * stream->framesPerDSBuffer / stream->streamRepresentation.streamInfo.sampleRate);
    while( stream->isActive && (timeoutMsec > 0)  )
    {
        Sleep(10);
        timeoutMsec -= 10;
    }
    if( stream->timerID != 0 )
    {
        timeKillEvent(stream->timerID);  /* Stop callback timer. */
        stream->timerID = 0;
    }


    if( stream->bufferProcessor.outputChannelCount > 0 )
    {
        hr = DSW_StopOutput( &stream->directSoundWrapper );
    }

    if( stream->bufferProcessor.inputChannelCount > 0 )
    {
        hr = DSW_StopInput( &stream->directSoundWrapper );
    }

    stream->isStarted = FALSE;

    return result;
}


/***********************************************************************************/
static PaError AbortStream( PaStream *s )
{
    PaWinDsStream *stream = (PaWinDsStream*)s;

    stream->abortProcessing = 1;
    return StopStream( s );
}


/***********************************************************************************/
static PaError IsStreamStopped( PaStream *s )
{
    PaWinDsStream *stream = (PaWinDsStream*)s;

    return !stream->isStarted;
}


/***********************************************************************************/
static PaError IsStreamActive( PaStream *s )
{
    PaWinDsStream *stream = (PaWinDsStream*)s;

    return stream->isActive;
}

/***********************************************************************************/
static PaTime GetStreamTime( PaStream *s )
{
    /* suppress unused variable warnings */
    (void) s;

    
/*
    new behavior for GetStreamTime is to return a stream based seconds clock
    used for the outTime parameter to the callback.
    FIXME: delete this comment when the other unnecessary related code has
    been cleaned from this file.

    PaWinDsStream *stream = (PaWinDsStream*)s;
    DSoundWrapper   *dsw;
    dsw = &stream->directSoundWrapper;
    return dsw->dsw_FramesPlayed;
*/
    return PaUtil_GetTime();
}


/***********************************************************************************/
static double GetStreamCpuLoad( PaStream* s )
{
    PaWinDsStream *stream = (PaWinDsStream*)s;

    return PaUtil_GetCpuLoad( &stream->cpuLoadMeasurer );
}



/***********************************************************************************
    As separate stream interfaces are used for blocking and callback
    streams, the following functions can be guaranteed to only be called
    for blocking streams.
*/

static PaError ReadStream( PaStream* s,
                           void *buffer,
                           unsigned long frames )
{
    PaWinDsStream *stream = (PaWinDsStream*)s;

    /* suppress unused variable warnings */
    (void) buffer;
    (void) frames;
    (void) stream;

    /* IMPLEMENT ME, see portaudio.h for required behavior*/

    return paNoError;
}


/***********************************************************************************/
static PaError WriteStream( PaStream* s,
                            const void *buffer,
                            unsigned long frames )
{
    PaWinDsStream *stream = (PaWinDsStream*)s;

    /* suppress unused variable warnings */
    (void) buffer;
    (void) frames;
    (void) stream;

    /* IMPLEMENT ME, see portaudio.h for required behavior*/

    return paNoError;
}


/***********************************************************************************/
static signed long GetStreamReadAvailable( PaStream* s )
{
    PaWinDsStream *stream = (PaWinDsStream*)s;

    /* suppress unused variable warnings */
    (void) stream;

    /* IMPLEMENT ME, see portaudio.h for required behavior*/

    return 0;
}


/***********************************************************************************/
static signed long GetStreamWriteAvailable( PaStream* s )
{
    PaWinDsStream *stream = (PaWinDsStream*)s;

    /* suppress unused variable warnings */
    (void) stream;
    
    /* IMPLEMENT ME, see portaudio.h for required behavior*/

    return 0;
}



