/**
* @file cusdr_audio_spectrum.h
* @brief cuSDR audio engine spectrum header file
* @author adaptation for cuSDR by Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-04-02
*/

/****************************************************************************
**
** Copyright (C) 2011 Nokia Corporation and/or its subsidiary(-ies).
** All rights reserved.
** Contact: Nokia Corporation (qt-info@nokia.com)
**
** This file is part of the examples of the Qt Toolkit.
**
** $QT_BEGIN_LICENSE:BSD$
** You may use this file under the terms of the BSD license as follows:
**
** "Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions are
** met:
**   * Redistributions of source code must retain the above copyright
**     notice, this list of conditions and the following disclaimer.
**   * Redistributions in binary form must reproduce the above copyright
**     notice, this list of conditions and the following disclaimer in
**     the documentation and/or other materials provided with the
**     distribution.
**   * Neither the name of Nokia Corporation and its Subsidiary(-ies) nor
**     the names of its contributors may be used to endorse or promote
**     products derived from this software without specific prior written
**     permission.
**
** THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
** "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
** LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
** A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
** OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
** SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
** LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
** OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE."
** $QT_END_LICENSE$
**
****************************************************************************/

#ifndef _CUSDR_AUDIO_SPECTRUM_H
#define _CUSDR_AUDIO_SPECTRUM_H

#include <QtCore/qglobal.h>
#include "cusdr_audio_utils.h"
//#include "fftreal_wrapper.h" // For FFTLengthPowerOfTwo

//-----------------------------------------------------------------------------
// Constants
//-----------------------------------------------------------------------------

// Number of audio samples used to calculate the frequency spectrum
//const int    SpectrumLengthSamples  = PowerOfTwo<FFTLengthPowerOfTwo>::Result;
const int    SpectrumLengthSamples  = 2048;//4096;//1024;//256;//512;//

// Number of bands in the frequency spectrum
const int    SpectrumNumBands       = 10;

// Lower bound of first band in the spectrum
const qreal  SpectrumLowFreq        = 0.0; // Hz

// Upper band of last band in the spectrum
const qreal  SpectrumHighFreq       = 1000.0; // Hz

// Waveform window size in microseconds
const qint64 WaveformWindowDuration = 500 * 1000;

// Length of waveform tiles in bytes
// Ideally, these would match the QAudio*::bufferSize(), but that isn't
// available until some time after QAudio*::start() has been called, and we
// need this value in order to initialize the waveform display.
// We therefore just choose a sensible value.
const int   WaveformTileLength      = 4096;

// Fudge factor used to calculate the spectrum bar heights
const qreal SpectrumAnalyserMultiplier = 0.15;

// Disable message timeout
const int   NullMessageTimeout      = -1;


//-----------------------------------------------------------------------------
// Types and data structures
//-----------------------------------------------------------------------------

enum WindowFunction {
    NoWindow,
    HannWindow,
	BlackmanHarrisWindow
};

const WindowFunction DefaultWindowFunction = HannWindow;//NoWindow;//BlackmanHarrisWindow;//

struct Tone {
    Tone(qreal freq = 0.0, qreal amp = 0.0, qint64 dur = 0)
    :   frequency(freq), amplitude(amp), duration(dur)
    { }

    // Start and end frequencies for swept tone generation
    qreal   frequency;

    // Amplitude in range [0.0, 1.0]
    qreal   amplitude;

	// tone duration in micro seconds
	qint64	duration;
};

struct SweptTone {
    SweptTone(qreal start = 0.0, qreal end = 0.0, qreal amp = 0.0, qint64 dur = 0)
    :   startFreq(start), endFreq(end), amplitude(amp), duration(dur)
    { Q_ASSERT(end >= start); }

    SweptTone(const Tone &tone)
    :   startFreq(tone.frequency), endFreq(tone.frequency), amplitude(tone.amplitude), duration(tone.duration)
    { }

    // Start and end frequencies for swept tone generation
    qreal   startFreq;
    qreal   endFreq;

    // Amplitude in range [0.0, 1.0]
    qreal   amplitude;

	// tone duration in micro seconds
	qint64	duration;
};

#ifdef DISABLE_WAVEFORM
#undef SUPERIMPOSE_PROGRESS_ON_WAVEFORM
#endif

#endif // _CUSDR_AUDIO_SPECTRUM_H

