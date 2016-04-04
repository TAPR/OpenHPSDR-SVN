﻿/*  Midi2Cat

Description: A subsystem that facilitates mapping Windows MIDI devices to CAT commands.
 
Copyright (C) 2016 Andrew Mansfield, M0YGG

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

The author can be reached by email at:  midi2cat@cametrix.com

*/

using Midi2Cat.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerSDR;

namespace Midi2Cat.Data
{
    public static class CatCmdDb
    {
        public static CatCommandAttribute Get(CatCmd Id)
        {
            var type = typeof(CatCmd);
            var memInfo = type.GetMember(Id.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(CatCommandAttribute), false);
            var CatCommand = ((CatCommandAttribute)attributes[0]);
            CatCommand.CatCommandId = Id;
            return CatCommand;
        }       
    }
    
    public enum CatCmd
    {        
        [CatCommandAttribute("Not Mapped", ControlType.Unknown)]
        None = 0,
        [CatCommandAttribute("VfoA To B", ControlType.Button)]
        VfoAtoB = 01,
        [CatCommandAttribute("VfoB To A", ControlType.Button)]
        VfoBtoA = 02,
        [CatCommandAttribute("Vfo Swap", ControlType.Button)]
        VfoSwap = 03,
        [CatCommandAttribute("Split On Off", ControlType.Button,true)]  
        SplitOnOff = 04,
        [CatCommandAttribute("Zero Beat", ControlType.Button)]
        ZeroBeatPress = 05,
        [CatCommandAttribute("Rit On Off", ControlType.Button, true)]  
        RitOnOff = 06,
        [CatCommandAttribute("Xit On Off", ControlType.Button, true)]  
        XitOnOff = 07,
        [CatCommandAttribute("RIT Clear", ControlType.Button)]
        RIT_clear = 08,
        [CatCommandAttribute("XIT Clear", ControlType.Button)]
        XIT_clear = 09,
        [CatCommandAttribute("Multi Rx On Off", ControlType.Button, true)]  
        MultiRxOnOff = 10,
        [CatCommandAttribute("Vfo Sync On Off", ControlType.Button, true)]  
        VfoSyncOnOff = 11,
        [CatCommandAttribute("Lock VFO On Off", ControlType.Button, true)]
        LockVFOOnOff = 12,
        [CatCommandAttribute("MOX On Off", ControlType.Button, true)]
        MOXOnOff = 13,
        [CatCommandAttribute("VOX On Off", ControlType.Button, true)]  
        VOXOnOff = 14,
        [CatCommandAttribute("Mute On Off", ControlType.Button, true)]  
        MuteOnOff = 15,
        [CatCommandAttribute("Rx1 Noise Blanker1 On Off", ControlType.Button, true)]  
        Rx1NoiseBlanker1OnOff = 16,
        [CatCommandAttribute("Rx1 Noise Blanker2 On Off", ControlType.Button, true)]  
        Rx1Noiseblanker2OnOff = 17,
        [CatCommandAttribute("Auto Notch On Off", ControlType.Button, true)]  
        AutoNotchOnOff = 18,
        [CatCommandAttribute("Noise Reduction 1 On Off", ControlType.Button, true)]  
        NoiseReductionOnOff = 19,
        [CatCommandAttribute("Noise Reduction 2 On Off", ControlType.Button, true)]  
        Rx2NoiseReductionOnOff = 20,
        [CatCommandAttribute("Binaural On Off", ControlType.Button, true)]  
        BinauralOnOff = 21,
        [CatCommandAttribute("Rx1 Filter Wider", ControlType.Button)]
        Rx1FilterWider = 22,
        [CatCommandAttribute("Rx1 Filter Narrower", ControlType.Button)]
        Rx1FilterNarrower = 23,
        [CatCommandAttribute("Rx1 Mode Next", ControlType.Button)]
        Rx1ModeNext = 24,
        [CatCommandAttribute("Rx1 Mode Prev", ControlType.Button)]
        Rx1ModePrev = 25,
        [CatCommandAttribute("Tuning Step Up", ControlType.Button)]
        TuningStepUp = 26,
        [CatCommandAttribute("Tuning Step Down", ControlType.Button)]
        TuningStepDown = 27,
        [CatCommandAttribute("Band Up", ControlType.Button)]
        BandUp = 28,
        [CatCommandAttribute("Band Down", ControlType.Button)]
        BandDown = 29,
        [CatCommandAttribute("Start On Off", ControlType.Button, true)]  
        StartOnOff = 30,
        [CatCommandAttribute("Tuner On Off", ControlType.Button, true)]  
        TunerOnOff = 31,
        [CatCommandAttribute("Compander On Off", ControlType.Button, true)]  
        CompanderOnOff = 32,
        [CatCommandAttribute("Stereo Diversity On Off", ControlType.Button, true)]
        StereoDiversityOnOff = 33,
        [CatCommandAttribute("DEXP On Off", ControlType.Button, true)]  
        DEXPOnOff = 34,
        [CatCommandAttribute("RX2 On Off", ControlType.Button, true)]  
        RX2OnOff = 35,
        [CatCommandAttribute("Rx2 Pre Amp On Off", ControlType.Button, true)]  
        Rx2PreAmpOnOff = 36,
        [CatCommandAttribute("Rx2 Noise Blanker1 On Off", ControlType.Button, true)]  
        Rx2NoiseBlanker1OnOff = 37,
        [CatCommandAttribute("Rx2 Noise Blanker2 On Off", ControlType.Button, true)]
        Rx2Noiseblanker2OnOff = 38,
        [CatCommandAttribute("Rx2 Band Up", ControlType.Button)]
        Rx2BandUp = 39,
        [CatCommandAttribute("Rx2 Band Down", ControlType.Button)]
        Rx2BandDown = 40,
        [CatCommandAttribute("RX EQ On Off", ControlType.Button, true)]  
        RXEQOnOff = 41,
        [CatCommandAttribute("TX EQ On Off", ControlType.Button, true)]  
        TXEQOnOff = 42,
        [CatCommandAttribute("Squelch On Off", ControlType.Button, true)]
        SquelchOnOff = 43,
        [CatCommandAttribute("Spectral Noise Blanker On Off", ControlType.Button, true)]
        SpectralNoiseBlankerOnOff = 44,
        [CatCommandAttribute("AGC Mode Up", ControlType.Button)]
        AGCModeUp = 45,
        [CatCommandAttribute("AGC Mode Down", ControlType.Button)]
        AGCModeDown = 46,
        [CatCommandAttribute("Rx2 Spectral Noise Blanker On Off", ControlType.Button, true)]  
        SpectralNoiseBlankerRx2OnOff = 47,
        [CatCommandAttribute("Display Average", ControlType.Button)]
        DisplayAverage = 48,
        [CatCommandAttribute("Display Peak", ControlType.Button)]
        DisplayPeak = 49,
        [CatCommandAttribute("Display Tx Filter", ControlType.Button)]
        DisplayTxFilter = 50,
        [CatCommandAttribute("Display Mode Next", ControlType.Button)]
        DisplayModeNext = 51,
        [CatCommandAttribute("Display Mode Prev", ControlType.Button)]
        DisplayModePrev = 52,
        [CatCommandAttribute("Zoom Inc", ControlType.Button)]
        ZoomInc = 53,
        [CatCommandAttribute("Zoom Dec", ControlType.Button)]
        ZoomDec = 54,
        [CatCommandAttribute("Quick Mode Save", ControlType.Button)]
        QuickModeSave = 55,
        [CatCommandAttribute("Quick Mode Restore", ControlType.Button)]
        QuickModeRestore = 56,
        [CatCommandAttribute("CW XMacro 1", ControlType.Button)]
        CWXMacro1 = 57,
        [CatCommandAttribute("CWX Macro 2", ControlType.Button)]
        CWXMacro2 = 58,
        [CatCommandAttribute("CWX Macro 3", ControlType.Button)]
        CWXMacro3 = 59,
        [CatCommandAttribute("CWX Macro 4", ControlType.Button)]
        CWXMacro4 = 60,
        [CatCommandAttribute("CWX Macro 5", ControlType.Button)]
        CWXMacro5 = 61,
        [CatCommandAttribute("CWX Macro 6", ControlType.Button)]
        CWXMacro6 = 62,
        [CatCommandAttribute("CWX Macro 7", ControlType.Button)]
        CWXMacro7 = 63,
        [CatCommandAttribute("CWX Macro 8", ControlType.Button)]
        CWXMacro8 = 64,
        [CatCommandAttribute("CWX Macro 9", ControlType.Button)]
        CWXMacro9 = 65,
        [CatCommandAttribute("CWX Stop", ControlType.Button)]
        CWXStop = 66,
        [CatCommandAttribute("MON On Off", ControlType.Button, true)]  
        MONOnOff = 67,
        [CatCommandAttribute("Pan Center", ControlType.Button)]
        PanCenter = 68,
        [CatCommandAttribute("VAC On Off", ControlType.Button, true)]  
        VACOnOff = 69,
        [CatCommandAttribute("Rx1 IQ To VAC", ControlType.Button)]
        IQtoVAC = 70,
        [CatCommandAttribute("Rx2 IQ to VAC", ControlType.Button)]
        IQtoVACRX2 = 71,
        [CatCommandAttribute("VAC 2 On Off", ControlType.Button,true)]
        VAC2OnOff = 72,
        [CatCommandAttribute("Click Tune On Off", ControlType.Button, true)]
        CTunOnOff = 73,
        [CatCommandAttribute("ESC Form On Off", ControlType.Button, true)]  
        ESCFormOnOff = 74,
        [CatCommandAttribute("RX2 Mute On Off", ControlType.Button, true)]  
        MuteRX2OnOff = 75,
        [CatCommandAttribute("Tuner State", ControlType.Button,true)]
        TunOnOff = 76,
        [CatCommandAttribute("Tuner Bypass", ControlType.Button, true)]
        TunerBypassOnOff = 77,
        [CatCommandAttribute("Band 160m", ControlType.Button)]
        Band160m = 78,
        [CatCommandAttribute("Band 80m", ControlType.Button)]
        Band80m = 79,
        [CatCommandAttribute("Band 60m", ControlType.Button)]
        Band60m = 80,
        [CatCommandAttribute("Band 40m", ControlType.Button)]
        Band40m = 81,
        [CatCommandAttribute("Band 30m", ControlType.Button)]
        Band30m = 82,
        [CatCommandAttribute("Band 20m", ControlType.Button)]
        Band20m = 83,
        [CatCommandAttribute("Band 17m", ControlType.Button)]
        Band17m = 84,
        [CatCommandAttribute("Band 15m", ControlType.Button)]
        Band15m = 85,
        [CatCommandAttribute("Band 12m", ControlType.Button)]
        Band12m = 86,
        [CatCommandAttribute("Band 10m", ControlType.Button)]
        Band10m = 87,
        [CatCommandAttribute("Band 6m", ControlType.Button)]
        Band6m = 88,
        [CatCommandAttribute("Band 2m", ControlType.Button)]
        Band2m = 89,
        [CatCommandAttribute("RX2 Band 160m", ControlType.Button)]
        Band160mRX2 = 90,
        [CatCommandAttribute("RX2 Band 80m", ControlType.Button)]
        Band80mRX2 = 91,
        [CatCommandAttribute("RX2 Band 60m", ControlType.Button)]
        Band60mRX2 = 92,
        [CatCommandAttribute("RX2 Band 40m", ControlType.Button)]
        Band40mRX2 = 93,
        [CatCommandAttribute("RX2 Band 30m", ControlType.Button)]
        Band30mRX2 = 94,
        [CatCommandAttribute("RX2 Band 20m", ControlType.Button)]
        Band20mRX2 = 95,
        [CatCommandAttribute("RX2 Band 17m", ControlType.Button)]
        Band17mRX2 = 96,
        [CatCommandAttribute("RX2 Band 15m", ControlType.Button)]
        Band15mRX2 = 97,
        [CatCommandAttribute("RX2 Band 12m", ControlType.Button)]
        Band12mRX2 = 98,
        [CatCommandAttribute("RX2 Band 10m", ControlType.Button)]
        Band10mRX2 = 99,
        [CatCommandAttribute("RX2 Band 6m", ControlType.Button)]
        Band6mRX2 = 500,
        [CatCommandAttribute("Band2mRX2", ControlType.Button)]
        Band2mRX2 = 501,
        [CatCommandAttribute("Mode SSB", ControlType.Button)]
        ModeSSB = 502,
        [CatCommandAttribute("Mode LSB", ControlType.Button)]
        ModeLSB = 503,
        [CatCommandAttribute("Mode USB", ControlType.Button)]
        ModeUSB = 504,
        [CatCommandAttribute("Mode DSB", ControlType.Button)]
        ModeDSB = 505,
        [CatCommandAttribute("Mode CW", ControlType.Button)]
        ModeCW = 506,
        [CatCommandAttribute("Mode CWL", ControlType.Button)]
        ModeCWL = 507,
        [CatCommandAttribute("Mode CWU", ControlType.Button)]
        ModeCWU = 508,
        [CatCommandAttribute("Mode FM", ControlType.Button)]
        ModeFM = 509,
        [CatCommandAttribute("Mode AM", ControlType.Button)]
        ModeAM = 510,
        [CatCommandAttribute("Mode DIGU", ControlType.Button)]
        ModeDIGU = 511,
        [CatCommandAttribute("Mode SPEC", ControlType.Button)]
        ModeSPEC = 512,
        [CatCommandAttribute("Mode DIGL", ControlType.Button)]
        ModeDIGL = 513,
        [CatCommandAttribute("Mode SAM", ControlType.Button)]
        ModeSAM = 514,
        [CatCommandAttribute("Mode DRM", ControlType.Button)]
        ModeDRM = 515,
        [CatCommandAttribute("Move VFOA Down 100Khz", ControlType.Button)]
        MoveVFOADown100Khz = 520,
        [CatCommandAttribute("Move VFOA Up 100Khz", ControlType.Button)]
        MoveVFOAUp100Khz = 521,
        [CatCommandAttribute("Change Freq Vfo A", ControlType.Wheel)]
        ChangeFreqVfoA = 101,
        [CatCommandAttribute("Change Freq Vfo B", ControlType.Wheel)]
        ChangeFreqVfoB = 102,
        [CatCommandAttribute("FilterBandwidth", ControlType.Wheel)]
        FilterBandwidth = 103,
        [CatCommandAttribute("RIT", ControlType.Wheel)]
        RIT_inc = 104,
        [CatCommandAttribute("XIT", ControlType.Wheel)]
        XIT_inc = 105,
        [CatCommandAttribute("Zoom", ControlType.Wheel)]
        ZoomSliderInc = 106,
        [CatCommandAttribute("Filter High", ControlType.Wheel)]
        FilterHigh = 107,
        [CatCommandAttribute("Filter Low", ControlType.Wheel)]
        FilterLow = 108,
        [CatCommandAttribute("Pan", ControlType.Wheel)]
        PanSliderInc = 109,
        [CatCommandAttribute("Multi Step Vfo A", ControlType.Wheel)]
        MultiStepVfoA = 110,
        [CatCommandAttribute("RIT", ControlType.Knob_or_Slider)]
        RIT = 201,
        [CatCommandAttribute("XIT", ControlType.Knob_or_Slider)]
        XIT = 202,
        [CatCommandAttribute("Filter Shift", ControlType.Knob_or_Slider)]
        FilterShift = 203,
        [CatCommandAttribute("Volume VfoA", ControlType.Knob_or_Slider)]
        VolumeVfoA = 204,
        [CatCommandAttribute("Volume VfoB", ControlType.Knob_or_Slider)]
        VolumeVfoB = 205,
        [CatCommandAttribute("Ratio Main Sub Rx", ControlType.Knob_or_Slider)]
        RatioMainSubRx = 206,
        [CatCommandAttribute("PreAmp Setting", ControlType.Knob_or_Slider)]
        PreAmpSettingsKnob = 207,
        [CatCommandAttribute("CW Speed", ControlType.Knob_or_Slider)]
        CWSpeed = 208,
        [CatCommandAttribute("AF Gain", ControlType.Knob_or_Slider)]
        SetAFGain = 209,
        [CatCommandAttribute("RX1 AGC Level", ControlType.Knob_or_Slider)]
        AGCLevel = 210,
        [CatCommandAttribute("DriveLevel", ControlType.Knob_or_Slider)]
        DriveLevel = 211,
        [CatCommandAttribute("MicGain", ControlType.Knob_or_Slider)]
        MicGain = 212,
        [CatCommandAttribute("DXLevel", ControlType.Knob_or_Slider)]
        DXLevel = 213,
        [CatCommandAttribute("CPDRLevel", ControlType.Knob_or_Slider)]
        CPDRLevel = 214,
        [CatCommandAttribute("VOXGain", ControlType.Knob_or_Slider)]
        VOXGain = 215,
        [CatCommandAttribute("DEXP Threshold", ControlType.Knob_or_Slider)]
        DEXPThreshold = 216,
        [CatCommandAttribute("Squelch", ControlType.Knob_or_Slider)]
        SquelchControl = 217,
        [CatCommandAttribute("RX2 AGC Level", ControlType.Knob_or_Slider)]
        RX2AGCLevel = 218,
        [CatCommandAttribute("TX AF Monitor", ControlType.Knob_or_Slider)]
        TXAFMonitor = 219,
        [CatCommandAttribute("AGC Mode", ControlType.Knob_or_Slider)]
        AGCModeKnob = 220,
        [CatCommandAttribute("Zoom", ControlType.Knob_or_Slider)]
        ZoomSliderFix = 221,
        [CatCommandAttribute("RX2 Volume", ControlType.Knob_or_Slider)]
        RX2Volume = 222,
        [CatCommandAttribute("Pan", ControlType.Knob_or_Slider)]
        PanSlider = 223,
        [CatCommandAttribute("VAC Gain RX", ControlType.Knob_or_Slider)]
        VACGainRX = 224,
        [CatCommandAttribute("VAC Gain TX", ControlType.Knob_or_Slider)]
        VACGainTX = 225,
        [CatCommandAttribute("VAC2 Gain RX", ControlType.Knob_or_Slider)]
        VAC2GainRX = 226,
        [CatCommandAttribute("VAC2 Gain TX", ControlType.Knob_or_Slider)]
        VAC2GainTX = 227,
        [CatCommandAttribute("Waterfall Low Limit", ControlType.Knob_or_Slider)]
        WaterfallLowLimit = 228,
        [CatCommandAttribute("Waterfall High Limit", ControlType.Knob_or_Slider)]
        WaterfallHighLimit = 229,
        [CatCommandAttribute("Stereo Balance RX2 (PAN) ", ControlType.Knob_or_Slider)]
        RX2Pan = 230,
    }
}// namespace
