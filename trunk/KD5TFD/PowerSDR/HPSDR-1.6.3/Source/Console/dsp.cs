//=================================================================
// dsp.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004, 2005, 2006  FlexRadio Systems
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact us via email at: sales@flex-radio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    12100 Technology Blvd.
//    Austin, TX 78727
//    USA
//=================================================================


using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace PowerSDR
{
	unsafe class DttSP
	{
		#region Enums

		public enum Mode
		{
			FIRST = -1,
			LSB,
			USB,
			DSB,
			CWL,
			CWU,
			FMN,
			AM,
			DIGU,
			SPEC,
			DIGL,
			SAM,
			DRM,
			LAST,
		}

		public enum MeterType
		{
			SIGNAL_STRENGTH=0, 
			AVG_SIGNAL_STRENGTH, 
			ADC_REAL, 
			ADC_IMAG,
			AGC_GAIN,
			MIC,  
			PWR,
			ALC,
			EQ,
			LEVELER,
			COMP,
			CPDR,
			ALC_G,
			LVL_G,
		}

		public enum TransmitState
		{
			OFF = 0,
			ON = 1,
		}

		public enum Window
		{
			FIRST = -1,
			RECTANGULAR,
			HANNING,
			WELCH,
			PARZEN,
			BARTLETT,
			HAMMING,
			BLACKMAN2,
			BLACKMAN3,
			BLACKMAN4,
			EXPONENTIAL,
			RIEMANN,
			BLKHARRIS,
			LAST,
		}

		#endregion

		#region Dll Method Definitions
		// ======================================================
		// DLL Method Definitions
		// ======================================================

		[DllImport("DttSP.dll", EntryPoint="Setup_SDR")]
		/// <summary>
		/// This function sets up the SDR functions and data structures
		/// </summary>
		/// <returns></returns>
		private static extern void SetupSDR();

		[DllImport("DttSP.dll", EntryPoint="SetDSPBuflen")]
		public static extern void ResizeSDR_dll(int DSPsize);
		public static int bufsize = 2048;
		public static void ResizeSDR(int DSPsize)
		{
			bufsize = DSPsize;
			ResizeSDR_dll(DSPsize);
			SetRXListen(1);
			ResizeSDR_dll(DSPsize);
			SetRXListen(0);
		}

		[DllImport("DttSP.dll", EntryPoint="Destroy_SDR")]
		public static extern void Exit();

		[DllImport("DttSP.dll", EntryPoint="process_samples_thread")]
		public static extern void ProcessSamplesThread();

		[DllImport("DttSP.dll", EntryPoint="AudioReset")]
		public static extern void AudioReset();

		[DllImport("DttSP.dll", EntryPoint="cwtimerfired")]
		public static extern void CWTimerFired();

		[DllImport("DttSP.dll", EntryPoint="StartKeyer")]
		public static extern void StartKeyer();

		[DllImport("DttSP.dll", EntryPoint="StopKeyer")]
		public static extern void StopKeyer();

		[DllImport("DttSP.dll", EntryPoint="CWRingRestart")]
		public static extern void CWRingRestart();
		///<summary>
		/// The KeyValue function sends timing data and key depressions and keying selection
		/// </summary>
		/// <param name="del">This is the time since the last call to this function</param>
		/// <param name="dash">This is a DttSP style boolean (a byte) which asserts dash or not-dash </param>
		/// <param name="dot">This is a DttSP style boolean (a byte) which asserts dot or not-dot</param>
		/// <param name="keyprog">This is a DttSP style boolean (a byte) which asserts iambic keyer or not-iambic</param>
		///
		[DllImport("DttSP.dll", EntryPoint="key_thread_process")]
		public static extern void KeyValue(float del, byte dash, byte dot, byte keyprog);

		[DllImport("DttSP.dll", EntryPoint="NewKeyer")]
		public static extern void NewKeyer(float freq, byte iambic, float gain, float ramp,
			float wpm, float SampleRate);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerPerf")]
		public static extern void SetKeyerPerf(byte hiperf);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerSpeed")]
		public static extern void SetKeyerSpeed(float speed);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerFreq")]
		public static extern void SetKeyerFreq(float freq);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerSampleRate")]
		public static extern void SetKeyerSampleRate(float freq);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerRamp")]
		public static extern void SetKeyerRamp(float ramp);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerIambic")]
		public static extern void SetKeyerIambic(byte iambic);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerRevPdl")]
		public static extern void SetKeyerRevPdl(byte revpdl);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerDeBounce")]
		public static extern void SetKeyerDeBounce(int debounce);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerWeight")]
		public static extern void SetKeyerWeight(int weight);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerResetSize")]
		public static extern void SetKeyerResetSize(int size);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerMode")]
		public static extern void SetKeyerMode(int mode);

		[DllImport("DttSP.dll", EntryPoint="KeyerClockFireRelease")]
		public static extern void KeyerClockFireRelease();

		[DllImport("DttSP.dll", EntryPoint="KeyerPlaying")]
		public static extern byte KeyerPlaying();

		[DllImport("DttSP.dll", EntryPoint="KeyerRunning")]
		public static extern byte KeyerRunning();

		[DllImport("DttSP.dll", EntryPoint="KeyerClockFireWait")]
		public static extern void KeyerClockFireWait();

		[DllImport("DttSP.dll", EntryPoint="KeyerStartedRelease")]
		public static extern void KeyerStartedRelease();

		[DllImport("DttSP.dll", EntryPoint="KeyerStartedWait")]
		public static extern void KeyerStartedWait();

		[DllImport("DttSP.dll", EntryPoint="SetWhichKey")]
		public static extern void SetWhichKey(byte keyselect);

		[DllImport("DttSP.dll", EntryPoint="PollTimerRelease")]
		public static extern void PollTimerRelease();

		[DllImport("DttSP.dll", EntryPoint="PollTimerWait")]
		public static extern void PollTimerWait();

		[DllImport("DttSP.dll", EntryPoint="DeleteKeyer")]
		public static extern void DeleteKeyer();

		[DllImport("DttSP.dll", EntryPoint="sound_thread_keyd")]
		public static extern void KeyerSoundThread();

		[DllImport("DttSP.dll", EntryPoint="CWtoneExchange")]
		public static extern void CWtoneExchange(float *bufl, float *bufr, int nframes);

		[DllImport("DttSP.dll", EntryPoint="CWtoneExchange")]
		public static extern void CWtoneExchangeIL(float *bufl, int nframes, int numchans);

		[DllImport("DttSP.dll", EntryPoint="Audio_Callback")]
		public static extern void ExchangeSamples(void *input_l, void *input_r, void *output_l, void *output_r, int numsamples);

		[DllImport("DttSP.dll", EntryPoint="SetAudioSize")]
		public static extern void SetAudioSize(int size);

		[DllImport("DttSP.dll", EntryPoint="SetMode")]
		private static extern int SetMode_DLL(Mode m);
		private static int SetMode(Mode m)
		{
			SetMode_DLL(m);
			SetRXListen(1);
			int i = SetMode_DLL(m);
			SetRXListen(0);
			return i;
		}

		[DllImport("DttSP.dll", EntryPoint="SetFilter")]
		private static extern int SetFilter_DLL(double low, double high, int taps, TransmitState trx_on);
		private static int SetFilter(double low, double high, int taps, TransmitState trx_on)
		{
			SetFilter_DLL(low, high, taps, trx_on);
			SetRXListen(1);
			int i = SetFilter_DLL(low, high, taps, trx_on);
			SetRXListen(0);
			return i;
		}

		[DllImport("DttSP.dll", EntryPoint="SetOsc")]
		public static extern int SetOscDll(double freq);
		public static double RXOsc;
		public static int SetOsc(double freq)
		{
			Console c = Console.getConsole(); 
			if ( c != null && c.label_DttSP_osc != null ) 
			{ 
				c.label_DttSP_osc.Text = "DttSP Osc: " + freq.ToString("f0");
			}
			RXOsc = freq;
			return SetOscDll(freq);
		}

		public static double GetOsc() 
		{
			return RXOsc; 
		}

		[DllImport("DttSP.dll", EntryPoint="SetTXOsc")]
		public static extern int SetTXOsc(double freq);

		[DllImport("DttSP.dll", EntryPoint="SetSampleRate")]
		private static extern int SetSampleRate(double sampleRate);

		[DllImport("DttSP.dll", EntryPoint="SetNR")]
		public static extern void SetNR_DLL(byte setit);
		public static void SetNR(byte setit)
		{
			SetNR_DLL(setit);
			SetRXListen(1);
			SetNR_DLL(setit);
			SetRXListen(0);
		}

		[DllImport("DttSP.dll", EntryPoint="SetNRvals")]
		public static extern void SetNRvals_DLL(int taps, int delay, double gain, double leak);
		public static void SetNRvals(int taps, int delay, double gain, double leak)
		{
			SetNRvals_DLL(taps, delay, gain, leak);
			SetRXListen(1);
			SetNRvals_DLL(taps, delay, gain, leak);
			SetRXListen(0);
		}

		[DllImport("DttSP.dll", EntryPoint="SetANF")]
		public static extern void SetANF_DLL(byte setit);
		public static void SetANF(byte setit)
		{
			SetANF_DLL(setit);
			SetRXListen(1);
			SetANF_DLL(setit);
			SetRXListen(0);
		}

		[DllImport("DttSP.dll", EntryPoint="SetANFvals")]
		public static extern void SetANFvals_DLL(int taps, int delay, double gain, double leak);
		public static void SetANFvals(int taps, int delay, double gain, double leak)
		{
			SetANFvals_DLL(taps, delay, gain, leak);
			SetRXListen(1);
			SetANFvals_DLL(taps, delay, gain, leak);
			SetRXListen(0);
		}

//		[DllImport("DttSP.dll", EntryPoint="SetRingBufferOffset")]
//		public static extern void SetRingBufferOffset(int offset);

		[DllImport("DttSP.dll", EntryPoint="SetRXAGC")]
		public static extern void SetRXAGC_DLL(AGCMode setit);
		public static void SetRXAGC(AGCMode setit)
		{
			SetRXAGC_DLL(setit);
			SetRXListen(1);
			SetRXAGC_DLL(setit);
			SetRXListen(0);
		}

		[DllImport("DttSP.dll", EntryPoint="SetTXAGCFF")]
		public static extern void SetTXAGCFF(byte setit);

		[DllImport("DttSP.dll", EntryPoint="SetTXAGCCompression")]
		public static extern void SetTXAGCCompression(double txcompression);

		[DllImport("DttSP.dll", EntryPoint="SetTXAGCLimit")]
		public static extern void SetTXAGCLimit(double limit);

		[DllImport("DttSP.dll", EntryPoint="SetTXAGCFFCompression")]
		public static extern void SetTXAGCFFCompression(double txcompression);

		[DllImport("DttSP.dll", EntryPoint="SetDCBlock")]
		public static extern void SetDCBlock(byte setit);

		[DllImport("DttSP.dll", EntryPoint="SetTXEQ")]
		public static extern void SetTXEQ(int[] txeq);

		[DllImport("DttSP.dll", EntryPoint="SetGrphTXEQ")]
		public static extern void SetGrphTXEQ(int[] txeq);

		[DllImport("DttSP.dll", EntryPoint="SetGrphRXEQ")]
		public static extern void SetGrphRXEQ_DLL(int[] rxeq);
		public static void SetGrphRXEQ(int[] rxeq)
		{
			SetGrphRXEQ_DLL(rxeq);
			SetRXListen(1);
			SetGrphRXEQ_DLL(rxeq);
			SetRXListen(0);
		}

		[DllImport("DttSP.dll", EntryPoint="SetGrphTXEQcmd")]
		public static extern void SetGrphTXEQcmd(byte state);

		[DllImport("DttSP.dll", EntryPoint="SetNotch160")]
		public static extern void SetNotch160(byte state);

		[DllImport("DttSP.dll", EntryPoint="SetGrphRXEQcmd")]
		public static extern void SetGrphRXEQcmd_DLL(byte state);
		public static void SetGrphRXEQcmd(byte state)
		{
			SetGrphRXEQcmd_DLL(state);
			SetRXListen(1);
			SetGrphRXEQcmd_DLL(state);
			SetRXListen(0);
		}

		[DllImport("DttSP.dll", EntryPoint="SetNB")]
		public static extern void SetNB_DLL(byte setit);
		public static void SetNB(byte setit)
		{
			SetNB_DLL(setit);
			SetRXListen(1);
			SetNB_DLL(setit);
			SetRXListen(0);
		}

		[DllImport("DttSP.dll", EntryPoint="SetNBvals")]
		public static extern void SetNBvals_DLL(double threshold);
		public static void SetNBvals(double threshold)
		{
			SetNBvals_DLL(threshold);
			SetRXListen(1);
			SetNBvals_DLL(threshold);
			SetRXListen(0);
		}

		[DllImport("DttSP.dll", EntryPoint="SetCorrectIQGain")]
		public static extern void SetCorrectIQGain(double setit);

		[DllImport("DttSP.dll", EntryPoint="SetCorrectTXIQGain")]
		public static extern void SetTXIQGain(double setit);

		[DllImport("DttSP.dll", EntryPoint="SetCorrectTXIQPhase")]
		public static extern void SetTXIQPhase(double setit);

		[DllImport("DttSP.dll", EntryPoint="SetSDROM")]
		public static extern void SetSDROM_DLL(byte setit);
		public static void SetSDROM(byte setit)
		{
			SetSDROM_DLL(setit);
			SetRXListen(1);
			SetSDROM_DLL(setit);
			SetRXListen(0);
		}

		[DllImport("DttSP.dll", EntryPoint="SetSDROMvals")]
		public static extern void SetSDROMvals_DLL(double threshold);
		public static void SetSDROMvals(double threshold)
		{
			SetSDROMvals_DLL(threshold);
			SetRXListen(1);
			SetSDROMvals_DLL(threshold);
			SetRXListen(0);
		}

		[DllImport("DttSP.dll", EntryPoint="SetFixedAGC")]
		public static extern void SetFixedAGC_DLL(double fixed_agc);
		public static void SetFixedAGC(double fixed_agc)
		{
			SetFixedAGC_DLL(fixed_agc);
			SetRXListen(1);
			SetFixedAGC_DLL(fixed_agc);
			SetRXListen(0);
		}

		[DllImport("DttSP.dll", EntryPoint="SetRXAGCTop")]
		public static extern void SetRXAGCMaxGain_DLL(double max_agc);
		public static void SetRXAGCMaxGain(double max_agc)
		{
			SetRXAGCMaxGain_DLL(max_agc);
			SetRXListen(1);
			SetRXAGCMaxGain_DLL(max_agc);
			SetRXListen(0);			
		}

		[DllImport("DttSP.dll", EntryPoint="SetRXAGCAttack")]
		public static extern void SetRXAGCAttack_DLL(int attack);
		public static void SetRXAGCAttack(int attack)
		{
			SetRXAGCAttack_DLL(attack);
			SetRXListen(1);
			SetRXAGCAttack_DLL(attack);
			SetRXListen(0);		
		}

		[DllImport("DttSP.dll", EntryPoint="SetRXAGCDecay")]
		public static extern void SetRXAGCDecay_DLL(int attack);
		public static void SetRXAGCDecay(int attack)
		{
			SetRXAGCDecay_DLL(attack);
			SetRXListen(1);
			SetRXAGCDecay_DLL(attack);
			SetRXListen(0);	
		}

		[DllImport("DttSP.dll", EntryPoint="SetRXAGCHang")]
		public static extern void SetRXAGCHang_DLL(int hang);
		public static void SetRXAGCHang(int hang)
		{
			SetRXAGCHang_DLL(hang);
			SetRXListen(1);
			SetRXAGCHang_DLL(hang);
			SetRXListen(0);	
		}

		[DllImport("DttSP.dll", EntryPoint="SetRXAGCSlope")]
		public static extern void SetRXAGCSlope_DLL(int slope);
		public static void SetRXAGCSlope(int slope)
		{
			SetRXAGCSlope_DLL(slope);
			SetRXListen(1);
			SetRXAGCSlope_DLL(slope);
			SetRXListen(0);	
		}

		[DllImport("DttSP.dll", EntryPoint="SetRXAGCHangThreshold")]
		public static extern void SetRXAGCHangThreshold_DLL(int hangthreshold);
		public static void SetRXAGCHangThreshold(int hangthreshold)
		{
			SetRXAGCHangThreshold_DLL(hangthreshold);
			SetRXListen(1);
			SetRXAGCHangThreshold_DLL(hangthreshold);
			SetRXListen(0);	
		}

		[DllImport("DttSP.dll", EntryPoint="SetTXCarrierLevel")]
		public static extern void SetTXCarrierLevel(double carrier_level);

		[DllImport("DttSP.dll", EntryPoint="SetTXALCBot")]
		public static extern void SetTXALCBot(double max_agc);

		[DllImport("DttSP.dll", EntryPoint="SetTXALCAttack")]
		public static extern void SetTXALCAttack(int attack);

		[DllImport("DttSP.dll", EntryPoint="SetTXALCDecay")]
		public static extern void SetTXALCDecay(int attack);

		[DllImport("DttSP.dll", EntryPoint="SetTXALCHang")]
		public static extern void SetTXALCHang(int hang);

		[DllImport("DttSP.dll", EntryPoint="SetTXLevelerTop")]
		public static extern void SetTXLevelerMaxGain(double max_agc);

		[DllImport("DttSP.dll", EntryPoint="SetTXLevelerAttack")]
		public static extern void SetTXLevelerAttack(int attack);

		[DllImport("DttSP.dll", EntryPoint="SetTXLevelerDecay")]
		public static extern void SetTXLevelerDecay(int attack);

		[DllImport("DttSP.dll", EntryPoint="SetTXLevelerHang")]
		public static extern void SetTXLevelerHang(int hang);

		[DllImport("DttSP.dll", EntryPoint="SetTXLevelerSt")]
		public static extern void SetTXLevelerSt(byte state);

		[DllImport("DttSP.dll", EntryPoint="SetWindow")]
		public static extern void SetWindow(Window windowset);

		[DllImport("DttSP.dll", EntryPoint="SetSpectrumPolyphase")]
		public static extern void SetSpectrumPolyphase(byte state);

		[DllImport("DttSP.dll", EntryPoint="SetBIN")]
		public static extern void SetBIN_DLL(byte setit);
		public static void SetBIN(byte setit)
		{
			SetBIN_DLL(setit);
			SetRXListen(1);
			SetBIN_DLL(setit);
			SetRXListen(0);	
		}

		[DllImport("DttSP.dll", EntryPoint="SetBlkNR")]
		public static extern void SetBlkNR_DLL(byte setit);
		public static void SetBlkNR(byte setit)
		{
			SetBlkNR_DLL(setit);
			SetRXListen(1);
			SetBlkNR_DLL(setit);
			SetRXListen(0);	
		}

		[DllImport("DttSP.dll", EntryPoint="SetBlkANF")]
		public static extern void SetBlkANF_DLL(byte setit);
		public static void SetBlkANF(byte setit)
		{
			SetBlkANF_DLL(setit);
			SetRXListen(1);
			SetBlkANF_DLL(setit);
			SetRXListen(0);	
		}

		[DllImport("DttSP.dll", EntryPoint="SetSquelchVal")]
		public static extern void SetSquelchVal_DLL(float setit);
		public static void SetSquelchVal(float setit)
		{
			SetSquelchVal_DLL(setit);
			SetRXListen(1);
			SetSquelchVal_DLL(setit);
			SetRXListen(0);	
		}			

		[DllImport("DttSP.dll", EntryPoint="SetSquelchState")]
		public static extern void SetSquelchState_DLL(byte state);
		public static void SetSquelchState(byte state)
		{
			SetSquelchState_DLL(state);
			SetRXListen(1);
			SetSquelchState_DLL(state);
			SetRXListen(0);	
		}

		[DllImport("DttSP.dll", EntryPoint="SetTXSquelchVal")]
		public static extern void SetTXSquelchVal(float setit);

		[DllImport("DttSP.dll", EntryPoint="SetTXSquelchSt")]
		public static extern void SetTXSquelchState(byte state);

		[DllImport("DttSP.dll", EntryPoint="SetTXCompandSt")]
		public static extern void SetTXCompandSt(int state);

		[DllImport("DttSP.dll", EntryPoint="SetTXCompand")]
		public static extern void SetTXCompand(double setit);

		[DllImport("DttSP.dll", EntryPoint="SetPWSmode")]
		public static extern void SetPWSmode(int setit);
//		public static void SetPWSmode(int setit)
//		{
//			Debug.WriteLine("SetPWSmode: "+setit);
//			SetPWSmodeDLL(setit);
//		}

		[DllImport("DttSP.dll", EntryPoint="SetCorrectIQPhase")]
		public static extern void SetCorrectIQPhase(double setit);

		[DllImport("DttSP.dll", EntryPoint="Process_Spectrum")]
		unsafe public static extern void GetSpectrum(float* results);

		[DllImport("DttSP.dll", EntryPoint="Process_Panadapter")]
		unsafe public static extern void GetPanadapter(float* results);

		[DllImport("DttSP.dll", EntryPoint="Process_Phase")]
		unsafe public static extern void GetPhase(float* results, int numpoints);

		[DllImport("DttSP.dll", EntryPoint="Process_Scope")]
		unsafe public static extern void GetScope(float* results, int numpoints);

		[DllImport("DttSP.dll", EntryPoint="SetTRX")]
		unsafe public static extern void SetTRX(TransmitState trx_on);

		[DllImport("DttSP.dll", EntryPoint="Calculate_Meters")]
		public static extern float CalculateMeter(MeterType MT);
/*		public static float CalculateMeter(MeterType MT)
		{
			System.Console.WriteLine(MT.ToString());
			return CalculateMeterDSP(MT);
		} */

		[DllImport("DttSP.dll", EntryPoint="Release_Update")]
		unsafe public static extern void ReleaseUpdate();
 
		[DllImport("DttSP.dll", EntryPoint="NewResampler")]
		unsafe public static extern void *NewResampler(int sampin, int sampout);

		[DllImport("DttSP.dll", EntryPoint="DoResampler")]
		unsafe public static extern void DoResampler(float *input, float *output, int numsamps, int *outsamps,void *ptr);

		[DllImport("DttSP.dll", EntryPoint="DelPolyPhaseFIR")]
		unsafe public static extern void DelResampler(void *ptr);

		[DllImport("DttSP.dll", EntryPoint="NewResamplerF")]
		unsafe public static extern void *NewResamplerF(int sampin, int sampout);

		[DllImport("DttSP.dll", EntryPoint="DoResamplerF")]
		unsafe public static extern void DoResamplerF(float *input, float *output, int numsamps, int *outsamps,void *ptr);

		[DllImport("DttSP.dll", EntryPoint="DelPolyPhaseFIRF")]
		unsafe public static extern void DelResamplerF(void *ptr);

        [DllImport("DttSP.dll", EntryPoint="SetRXListen")]
		unsafe public static extern void SetRXListen(int rx_index);

		[DllImport("DttSP.dll", EntryPoint="SetRXOn")]
		unsafe public static extern void SetRXOn();

		[DllImport("DttSP.dll", EntryPoint="SetRXOff")]
		unsafe public static extern void SetRXOff();

		[DllImport("DttSP.dll", EntryPoint="SetRXPan")]
		unsafe public static extern void SetRXPan(float pan); // takes values from 0 to 1.0 for L to R.

		#endregion

		#region Properties
// ======================================================
// Properties
// ======================================================

		private static bool not_pan = false;
		public static bool NotPan
		{
			get { return not_pan; }
			set { not_pan = value; }
		}

		private static int block_size;
		public static int BlockSize
		{
			get { return block_size; }
		}

		private static Mode current_mode;
		public static Mode CurrentMode
		{
			get	{ return current_mode;}
			set 
			{
				current_mode = value;
                int i = SetMode(current_mode);
				if(i != 0)
					MessageBox.Show("Error in DttSP.SetMode: "+i);
			}
		}

		private static Window current_window = Window.HANNING;
		public static Window CurrentWindow
		{
			get { return current_window; }
			set
			{
				current_window = value;
				SetWindow(current_window);
			}
		}

		private static int rx_display_low = -4000;
		public static int RXDisplayLow
		{
			get { return rx_display_low; }
			set
			{
				rx_display_low = value;
				Display.RXDisplayLow = value;
			}
		}

		private static int rx_display_high = 4000;
		public static int RXDisplayHigh
		{
			get { return rx_display_high; }
			set
			{
				rx_display_high = value;
				Display.RXDisplayHigh = value;
			}
		}

		private static int tx_display_low = -4000;
		public static int TXDisplayLow
		{
			get { return tx_display_low; }
			set
			{
				tx_display_low = value;
				Display.TXDisplayLow = value;
			}
		}

		private static int tx_display_high = 4000;
		public static int TXDisplayHigh
		{
			get { return tx_display_high; }
			set
			{
				tx_display_high = value;
				Display.TXDisplayHigh = value;
			}
		}

		private static int rx_filter_low_cut = 200;
		public static int RXFilterLowCut
		{
			get { return rx_filter_low_cut;}
			set 
			{
				rx_filter_low_cut = value;
				if(not_pan) UpdateRXDisplayVars();

				int i = SetFilter(rx_filter_low_cut, rx_filter_high_cut, filter_taps, 0);
				if(i != 0)
					MessageBox.Show("Error in DttSP.SetFilter(RXFilterLowCut): "+i);
			}
		}

		private static int rx_filter_high_cut = 2800;
		public static int RXFilterHighCut
		{
			get { return rx_filter_high_cut;}
			set 
			{
				rx_filter_high_cut = value;
				if(not_pan) UpdateRXDisplayVars();

				int i = SetFilter(rx_filter_low_cut, rx_filter_high_cut, filter_taps, 0);
				if(i != 0)
					MessageBox.Show("Error in DttSP.SetFilter(RXFilterHighCut): "+i);
			}
		}

		private static int tx_filter_low_cut = 300;
		public static int TXFilterLowCut
		{
			get { return tx_filter_low_cut;}
			set 
			{
				tx_filter_low_cut = value;
				if(not_pan) UpdateTXDisplayVars();

				int i = SetFilter(tx_filter_low_cut, tx_filter_high_cut, filter_taps, TransmitState.ON);
				if(i != 0)
					MessageBox.Show("Error in DttSP.SetFilter(TXFilterLowCut): "+i);
			}
		}

		private static int tx_filter_high_cut = 3000;
		public static int TXFilterHighCut
		{
			get { return tx_filter_high_cut;}
			set 
			{
				tx_filter_high_cut = value;
				if(not_pan) UpdateTXDisplayVars();

				int i = SetFilter(tx_filter_low_cut, tx_filter_high_cut, filter_taps, TransmitState.ON);
				if(i != 0)
					MessageBox.Show("Error in DttSP.SetFilter(TXFilterHighCut): "+i);
			}
		}

		private static double sample_rate;
		public static double SampleRate
		{
			get { return sample_rate;}
			set
			{
				sample_rate = value;
				int i = SetSampleRate(sample_rate);
				if(i != 0)
					MessageBox.Show("Error in DttSP.SetSampleRate: "+i);
			}
		}

		private static int filter_taps;
		public static int FilterTaps
		{
			get { return filter_taps;}
			set
			{
				filter_taps = value;
				int i = SetFilter(rx_filter_low_cut, rx_filter_high_cut, filter_taps, 0);
				if(i != 0)
					MessageBox.Show("Error in DttSP.SetFilter(FilterTaps): "+i);
			}
		}

		private static double second_if;
		public static double SecondIF
		{
			get { return second_if; }
			set
			{
				second_if = value;
				int i = SetOsc(second_if);
				if(i != 0)
					MessageBox.Show("Error in SetOsc(SecondIF): "+i);
			}
		}

		#endregion

		#region Misc Routines
// ======================================================
// Misc Routines
// ======================================================

		public static void Init()
		{
			block_size = 2048;
			SetupSDR();
			ReleaseUpdate();
			rx_filter_low_cut = 8175;
			rx_filter_high_cut = 10775;
			filter_taps = BlockSize;
			SampleRate = 48000.0;
			SecondIF = -11025.0;
		}

		public static void SetRXFilters(int low, int high)
		{
			rx_filter_low_cut = low;
			rx_filter_high_cut = high;
			if(not_pan) UpdateRXDisplayVars();

			int i = SetFilter(rx_filter_low_cut, rx_filter_high_cut, filter_taps, TransmitState.OFF);

			if(i != 0)
				MessageBox.Show("Error in DttSP.SetRXFilters (SetFilter): "+i);
		}

		public static void SetTXFilters(int low, int high)
		{
			switch(CurrentMode)
			{
				case DttSP.Mode.LSB:
				case DttSP.Mode.CWL:
				case DttSP.Mode.DIGL:
					tx_filter_low_cut = -high;
					tx_filter_high_cut = -low;
					break;
				case DttSP.Mode.USB:
				case DttSP.Mode.CWU:
				case DttSP.Mode.DIGU:
					tx_filter_low_cut = low;
					tx_filter_high_cut = high;
					break;
				case DttSP.Mode.DSB:
					tx_filter_low_cut = -high;
					tx_filter_high_cut = high;
					break;
				case DttSP.Mode.AM:
				case DttSP.Mode.SAM:
					tx_filter_low_cut = -high;
					tx_filter_high_cut = high;
					break;
				case DttSP.Mode.FMN:
					tx_filter_low_cut = -6000;
					tx_filter_high_cut = 6000;
					break;
			}
			if(not_pan) UpdateTXDisplayVars();

			int i = SetFilter(tx_filter_low_cut, tx_filter_high_cut, filter_taps, TransmitState.ON);
			if(i != 0)
				MessageBox.Show("Error in DttSP.SetTXFilters (SetFilter): "+i);
		}

		public static void UpdateRXDisplayVars()
		{
			int low = 0, high = 0;
			if(rx_filter_low_cut < 0 && rx_filter_high_cut <= 0)
			{
				high = 0;
				if(rx_filter_low_cut >= -910)
					low = -1000;
				else
					low = (int)(rx_filter_low_cut*1.1);
			}
			else if(rx_filter_low_cut >= 0 && rx_filter_high_cut > 0)
			{
				low = 0;
				if(rx_filter_high_cut <= 910)
					high = 1000;
				else
					high = (int)(rx_filter_high_cut*1.1);
			}
			else if(rx_filter_low_cut < 0 && rx_filter_high_cut > 0)
			{
				int max_edge = Math.Max(-rx_filter_low_cut, rx_filter_high_cut);
				low = (int)(max_edge*-1.1);
				high = (int)(max_edge*1.1);
			}
			RXDisplayLow = low;
			RXDisplayHigh = high;
		}

		public static void UpdateTXDisplayVars()
		{
			int low = 0, high = 0;
			if(tx_filter_low_cut < 0 && tx_filter_high_cut <= 0)
			{
				high = 0;
				if(tx_filter_low_cut >= -910)
					low = -1000;
				else
					low = (int)(tx_filter_low_cut*1.1);
			}
			else if(tx_filter_low_cut >= 0 && tx_filter_high_cut > 0)
			{
				low = 0;
				if(tx_filter_high_cut <= 910)
					high = 1000;
				else
					high = (int)(tx_filter_high_cut*1.1);
			}
			else if(tx_filter_low_cut < 0 && tx_filter_high_cut > 0)
			{
				int max_edge = Math.Max(-tx_filter_low_cut, tx_filter_high_cut);
				low = (int)(max_edge*-1.1);
				high = (int)(max_edge*1.1);
			}

			TXDisplayLow = low;
			TXDisplayHigh = high;
		}

		#endregion
	}
}