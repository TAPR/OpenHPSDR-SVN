//=================================================================
// dsp.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2009  FlexRadio Systems Copyright (C) 2010-2012  Doug Wigley
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
//    8900 Marybank Dr.
//    Austin, TX 78750
//    USA
//=================================================================

namespace PowerSDR
{
    using System.Runtime.InteropServices;

    unsafe class DttSP
	{
		#region Enums

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
			MIC_PK,
			ALC_PK,
			EQ_PK,
			LEVELER_PK,
			COMP_PK,
			CPDR_PK,
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
        public static extern void SetupSDR(System.String app_data_path);

		[DllImport("DttSP.dll", EntryPoint="SetDSPBuflen")]
		public static extern void ResizeSDR(uint thread, int DSPsize);
		
		[DllImport("DttSP.dll", EntryPoint="Destroy_SDR")]
		public static extern void Exit();

		[DllImport("DttSP.dll", EntryPoint="process_samples_thread")]
		public static extern void ProcessSamplesThread(uint thread);

//		DllImport("DttSP.dll", EntryPoint="AudioReset")]
//		public static extern void AudioReset();

	//	[DllImport("DttSP.dll", EntryPoint="cwtimerfired")]
	//	public static extern void CWTimerFired();
 
	//	[DllImport("DttSP.dll", EntryPoint="StartKeyer")]
	//	public static extern void StartKeyer();

	//	[DllImport("DttSP.dll", EntryPoint="StopKeyer")]
	//	public static extern void StopKeyer();

	//	[DllImport("DttSP.dll", EntryPoint="CWRingRestart")]
	//	public static extern void CWRingRestart();
		///<summary>
		/// The KeyValue function sends timing data and key depressions and keying selection
		/// </summary>
		/// <param name="del">This is the time since the last call to this function</param>
		/// <param name="dash">This is a DttSP style boolean (a byte) which asserts dash or not-dash </param>
		/// <param name="dot">This is a DttSP style boolean (a byte) which asserts dot or not-dot</param>
		/// <param name="keyprog">This is a DttSP style boolean (a byte) which asserts iambic keyer or not-iambic</param>
		///
	//	[DllImport("DttSP.dll", EntryPoint="key_thread_process")]
	//	public static extern void KeyValue(float del, bool dash, bool dot, bool keyprog);

    //	[DllImport("DttSP.dll", EntryPoint="NewKeyer")]
	//	public static extern void NewKeyer(float freq, bool iambic, float gain, float ramp,
	//		float wpm, float SampleRate);

     //   [DllImport("DttSP.dll", EntryPoint = "SetSwchRiseThresh")]
     //   public static extern void SetSwchRiseThresh(uint thread, float val);

		[DllImport("DttSP.dll",EntryPoint="SetThreadProcessingMode")]
		public static extern void SetThreadProcessingMode(uint thread, int runmode);

	/*	[DllImport("DttSP.dll", EntryPoint="SetKeyerSpeed")]///
		public static extern void SetKeyerSpeed(float speed);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerFreq")]///
		public static extern void SetKeyerFreq(float freq);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerSampleRate")]///
		public static extern void SetKeyerSampleRate(float freq);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerRamp")]///
		public static extern void SetKeyerRamp(float ramp);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerIambic")]///
		public static extern void SetKeyerIambic(bool iambic);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerRevPdl")]///
		public static extern void SetKeyerRevPdl(bool revpdl);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerDeBounce")]///
		public static extern void SetKeyerDeBounce(int debounce);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerWeight")]///
		public static extern void SetKeyerWeight(int weight);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerResetSize")]///
		public static extern void SetKeyerResetSize(int size);

		[DllImport("DttSP.dll", EntryPoint="SetKeyerMode")]///
		public static extern void SetKeyerMode(int mode);

		[DllImport("DttSP.dll", EntryPoint="KeyerClockFireRelease")]
		public static extern void KeyerClockFireRelease();

		[DllImport("DttSP.dll", EntryPoint="KeyerPlaying")]
		[return:MarshalAs(UnmanagedType.I1)]
		public static extern bool KeyerPlaying();

		[DllImport("DttSP.dll", EntryPoint="KeyerRunning")]
		[return:MarshalAs(UnmanagedType.I1)]
		public static extern bool KeyerRunning();

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
        */
		[DllImport("DttSP.dll", EntryPoint="Audio_Callback")]
		public static extern void ExchangeSamples(void *input_l, void *input_r, void *output_l, void *output_r, int numsamples);

		[DllImport("DttSP.dll", EntryPoint="Audio_Callback2")]
		public static extern void ExchangeSamples2(void* input, void *output, int numsamples);

		[DllImport("DttSP.dll", EntryPoint="SetAudioSize")]///
		public static extern void SetAudioSize(int size);

		[DllImport("DttSP.dll", EntryPoint="SetMode")]///
		public static extern int SetMode(uint thread, uint subrx, DSPMode m);
				
		public static int SetTXMode(uint thread, DSPMode m)
		{
			return SetMode(thread, 0, m);
		}

		[DllImport("DttSP.dll", EntryPoint="SetRXFilter")]///
		public static extern int SetRXFilter(uint thread, uint subrx, double low, double high);
		/*public static int SetRXFilter(uint thread, uint subrx, double low, double high)
		{
			Debug.WriteLine("SetRXFilter("+thread+", "+subrx+", "+low+", "+high+")");
			return SetRXFilterdll(thread, subrx, low, high);
		}*/
	
		[DllImport("DttSP.dll", EntryPoint="SetTXFilter")]///
		public static extern int SetTXFilter(uint thread, double low, double high);

		[DllImport("DttSP.dll", EntryPoint="SetTXOsc")]///
		public static extern int SetTXOsc(uint thread, double freq);

		[DllImport("DttSP.dll", EntryPoint="SetSampleRate")]
		public static extern int SetSampleRate(double sampleRate);

		[DllImport("DttSP.dll", EntryPoint="SetANR")]///
		public static extern void SetNR(uint thread, uint subrx, bool setit);

		[DllImport("DttSP.dll", EntryPoint="SetANRvals")]///
		public static extern void SetNRvals(uint thread, uint subrx, int taps, int delay, double gain, double leak);

        [DllImport("DttSP.dll", EntryPoint = "SetANRposition")]///
        public static extern void SetNRposition(uint thread, uint subrx, int position);

        [DllImport("DttSP.dll", EntryPoint = "SetRXManualNotchEnable")]///
        public static extern void SetRXManualNotchEnable(uint thread, uint subrx, uint index, bool flag);

        [DllImport("DttSP.dll", EntryPoint = "SetRXManualNotchFreq")]///
        public static extern void SetRXManualNotchFreq(uint thread, uint subrx, uint index, double freq);

        [DllImport("DttSP.dll", EntryPoint = "SetRXManualNotchBW")]///
        public static extern void SetRXManualNotchBW(uint thread, uint subrx, uint index, double bw);

		[DllImport("DttSP.dll", EntryPoint="SetANF")]///
		public static extern void SetANF(uint thread, uint subrx, bool setit);

		[DllImport("DttSP.dll", EntryPoint="SetANFvals")]///
		public static extern void SetANFvals(uint thread, uint subrx, int taps, int delay, double gain, double leak);

        [DllImport("DttSP.dll", EntryPoint = "SetANFposition")]///
        public static extern void SetANFposition(uint thread, uint subrx, int position);

		[DllImport("DttSP.dll", EntryPoint="SetRXAGC")]///
		public static extern void SetRXAGC(uint thread, uint subrx, AGCMode setit);
		/*public static void SetRXAGC(uint thread, uint subrx, AGCMode setit)
		{
			Debug.WriteLine("SetRXAGC("+thread+", "+subrx+", "+setit.ToString()+")");
			SetRXAGCdll(thread, subrx, setit);
		}*/

		[DllImport("DttSP.dll", EntryPoint="SetTXAGCFF")]///
		public static extern void SetTXAGCFF(uint thread, bool setit);

		//		[DllImport("DttSP.dll", EntryPoint="SetTXAGCCompression")]///
		//		public static extern void SetTXAGCCompression(uint thread, double txcompression);

		/*[DllImport("DttSP.dll", EntryPoint="SetTXAGCLimit")]///
		public static extern void SetTXAGCLimit(uint thread, double limit);*/

		[DllImport("DttSP.dll", EntryPoint="SetTXAGCFFCompression")]///
		public static extern void SetTXAGCFFCompression(uint thread, double txcompression);

		[DllImport("DttSP.dll", EntryPoint="SetTXDCBlock")]///
		public static extern void SetTXDCBlock(uint thread, bool setit);

        [DllImport("DttSP.dll", EntryPoint = "SetRXDCBlock")]///
        public static extern void SetRXDCBlock(uint thread, uint subrx, bool setit);

        [DllImport("DttSP.dll", EntryPoint = "SetRXDCBlockGain")]///
        public static extern void SetRXDCBlockGain(uint thread, uint subrx,float gain);

		[DllImport("DttSP.dll", EntryPoint="SetTXEQ")]
		public static extern void SetTXEQ(uint thread, int[] txeq);

		[DllImport("DttSP.dll", EntryPoint="SetGrphTXEQcmd")]///
		public static extern void SetGrphTXEQcmd(uint thread, bool state);

		[DllImport("DttSP.dll", EntryPoint="SetGrphTXEQ")]///
		public static extern void SetGrphTXEQ(uint thread, int[] txeq);
		
		[DllImport("DttSP.dll", EntryPoint="SetGrphTXEQ10")]///
		public static extern void SetGrphTXEQ10(uint thread, int[] txeq);

        [DllImport("DttSP.dll", EntryPoint = "SetTXFMDeviation")]///
        public static extern void SetTXFMDeviation(uint thread, double deviation);

        [DllImport("DttSP.dll", EntryPoint = "SetRXFMDeviation")]///
        public static extern void SetRXFMDeviation(uint thread, uint subrx, double deviation);

        [DllImport("DttSP.dll", EntryPoint = "SetFMSquelchThreshold")]///
        public static extern void SetFMSquelchThreshold(uint thread, uint subrx, float threshold);
        
        [DllImport("DttSP.dll", EntryPoint = "SetCTCSSFreq")]///
        public static extern void SetCTCSSFreq(uint thread, double freq_hz);

        [DllImport("DttSP.dll", EntryPoint = "SetCTCSSFlag")]///
        public static extern void SetCTCSSFlag(uint thread, bool flag);

		[DllImport("DttSP.dll", EntryPoint="SetGrphRXEQcmd")]///
		public static extern void SetGrphRXEQcmd(uint thread, uint subrx, bool state);

		[DllImport("DttSP.dll", EntryPoint="SetGrphRXEQ")]///
		public static extern void SetGrphRXEQ(uint thread, uint subrx,int[] rxeq);

		[DllImport("DttSP.dll", EntryPoint="SetGrphRXEQ10")]///
		public static extern void SetGrphRXEQ10(uint thread, uint subrx,int[] rxeq);

		[DllImport("DttSP.dll", EntryPoint="SetNotch160")]///
		public static extern void SetNotch160(uint thread, bool state);

		[DllImport("DttSP.dll", EntryPoint="SetNB")]///
		public static extern void SetNB(uint thread, uint subrx, bool setit);

		[DllImport("DttSP.dll", EntryPoint="SetNBvals")]///
		public static extern void SetNBvals(uint thread, uint subrx, double threshold);

		//[DllImport("DttSP.dll", EntryPoint="GetSAMFreq")]///
		//public static extern void GetSAMFreq(uint thread, uint subrx, float *freq);

		//[DllImport("DttSP.dll", EntryPoint="GetSAMPLLvals")]///
		//public static extern void GetSAMPLLvals(uint thread, uint subrx, float *alpha, float *beta);

		//[DllImport("DttSP.dll", EntryPoint="SetSAMPLLvals")]///
		//public static extern void SetSAMPLLvals(uint thread, uint subrx, float alpha, float beta);

		[DllImport("DttSP.dll", EntryPoint="SetCorrectIQEnable")]
		public static extern void SetCorrectIQEnable(uint setit);

		[DllImport("DttSP.dll", EntryPoint="GetCorrectRXIQw")]
		public static extern void GetCorrectRXIQw(uint thread, uint subrx, float* real, float* imag, uint index);

		[DllImport("DttSP.dll", EntryPoint="SetCorrectRXIQwReal")]///
		public static extern void SetCorrectRXIQwReal(uint thread, uint subrx, float setit, uint index);

		[DllImport("DttSP.dll", EntryPoint="SetCorrectRXIQwImag")]///
		public static extern void SetCorrectRXIQwImag(uint thread, uint subrx, float setit, uint index);

		[DllImport("DttSP.dll", EntryPoint="SetCorrectRXIQw")]///
		public static extern void SetCorrectRXIQw(uint thread, uint subrx, float real, float imag, uint index);

		[DllImport("DttSP.dll", EntryPoint="SetCorrectRXIQMu")]///
		public static extern void SetCorrectIQMu(uint thread, uint subrx, double setit);

		[DllImport("DttSP.dll", EntryPoint="GetCorrectRXIQMu")]///
		public static extern float GetCorrectIQMu(uint thread, uint subrx);

		[DllImport("DttSP.dll", EntryPoint="SetCorrectTXIQGain")]///
		public static extern void SetTXIQGain(uint thread, double setit);

		[DllImport("DttSP.dll", EntryPoint="SetCorrectTXIQPhase")]///
		public static extern void SetTXIQPhase(uint thread, double setit);

		[DllImport("DttSP.dll", EntryPoint="SetCorrectTXIQMu")]///
		public static extern void SetTXIQMu(uint thread, double setit);

		[DllImport("DttSP.dll", EntryPoint="SetSDROM")]///
		public static extern void SetSDROM(uint thread, uint subrx, bool setit);

		[DllImport("DttSP.dll", EntryPoint="SetSDROMvals")]///
		public static extern void SetSDROMvals(uint thread, uint subrx, double threshold);

		[DllImport("DttSP.dll", EntryPoint="SetFixedAGC")]///
		public static extern void SetFixedAGC(uint thread, uint subrx, double fixed_agc);

		[DllImport("DttSP.dll", EntryPoint="SetRXAGCTop")]///
		public static extern void SetRXAGCMaxGain(uint thread, uint subrx, double max_agc);

        [DllImport("DttSP.dll", EntryPoint = "GetRXAGCTop")]///
        public static extern void GetRXAGCMaxGain(uint thread, uint subrx, double *max_agc);

		[DllImport("DttSP.dll", EntryPoint="SetRXAGCAttack")]///
		public static extern void SetRXAGCAttack(uint thread, uint subrx, int attack);
		/*public static void SetRXAGCAttack(uint thread, uint subrx, int attack)
		{
			Debug.WriteLine("SetRXAGCAttack("+thread+", "+subrx+", "+attack+")");
			SetRXAGCAttackdll(thread, subrx, attack);
		}*/

		[DllImport("DttSP.dll", EntryPoint="SetRXAGCDecay")]///
		public static extern void SetRXAGCDecay(uint thread, uint subrx, int decay);
		/*public static void SetRXAGCDecay(uint thread, uint subrx, int decay)
		{
			Debug.WriteLine("SetRXAGCDecay("+thread+", "+subrx+", "+decay+")");
			SetRXAGCDecaydll(thread, subrx, decay);
		}*/

		[DllImport("DttSP.dll", EntryPoint="SetRXAGCHang")]///
		public static extern void SetRXAGCHang(uint thread, uint subrx, int hang);
		/*public static void SetRXAGCHang(uint thread, uint subrx, int hang)
		{
			Debug.WriteLine("SetRXAGCHang("+thread+", "+subrx+", "+hang+")");
			SetRXAGCHangdll(thread, subrx, hang);
		}*/

		[DllImport("DttSP.dll", EntryPoint="SetRXOutputGain")]///
		public static extern void SetRXOutputGain(uint thread, uint subrx, double g);

		[DllImport("DttSP.dll", EntryPoint="SetRXAGCSlope")]///
		public static extern void SetRXAGCSlope(uint thread, uint subrx, int slope);

		[DllImport("DttSP.dll", EntryPoint="SetRXAGCHangThreshold")]///
		public static extern void SetRXAGCHangThreshold(uint thread, uint subrx, int hangthreshold);

        [DllImport("DttSP.dll", EntryPoint = "GetRXAGCHangThreshold")]///
        public static extern void GetRXAGCHangThreshold(uint thread, uint subrx, int *hangthreshold);

        [DllImport("DttSP.dll", EntryPoint = "GetRXAGCThresh")]
        public static extern void GetRXAGCThresh(uint thread, uint subrx, double *thresh);

        [DllImport("DttSP.dll", EntryPoint = "SetRXAGCThresh")]
        public static extern void SetRXAGCThresh(uint thread, uint subrx, double thresh);

        [DllImport("DttSP.dll", EntryPoint = "GetRXAGCHangLevel")]
        public static extern void GetRXAGCHangLevel(uint thread, uint subrx, double* hang_level);

        [DllImport("DttSP.dll", EntryPoint = "SetRXAGCHangLevel")]
        public static extern void SetRXAGCHangLevel(uint thread, uint subrx, double hang_level);

		[DllImport("DttSP.dll", EntryPoint="SetTXAMCarrierLevel")]///
		public static extern void SetTXAMCarrierLevel(uint thread, double carrier_level);

        [DllImport("DttSP.dll", EntryPoint = "SetSBMode")]      //for SAM receive
        unsafe public static extern int SetSBMode(uint thread, uint subrx, int sbmode);

        [DllImport("DttSP.dll", EntryPoint = "SetFadeLevel")]   //for AM & SAM receive
        unsafe public static extern int SetFadeLevel(uint thread, uint subrx, int fadelevel);

        [DllImport("DttSP.dll", EntryPoint = "SetTXALCBot")]///
		public static extern void SetTXALCBot(uint thread, double max_agc);

		[DllImport("DttSP.dll", EntryPoint="SetTXALCAttack")]///
		public static extern void SetTXALCAttack(uint thread, int attack);

		[DllImport("DttSP.dll", EntryPoint="SetTXALCDecay")]///
		public static extern void SetTXALCDecay(uint thread, int attack);

		[DllImport("DttSP.dll", EntryPoint="SetTXALCHang")]///
		public static extern void SetTXALCHang(uint thread, int hang);

		[DllImport("DttSP.dll", EntryPoint="SetTXLevelerTop")]
		public static extern void SetTXLevelerMaxGain(uint thread, double max_agc);

		[DllImport("DttSP.dll", EntryPoint="SetTXLevelerAttack")]///
		public static extern void SetTXLevelerAttack(uint thread, int attack);

		[DllImport("DttSP.dll", EntryPoint="SetTXLevelerDecay")]///
		public static extern void SetTXLevelerDecay(uint thread, int attack);

		[DllImport("DttSP.dll", EntryPoint="SetTXLevelerHang")]///
		public static extern void SetTXLevelerHang(uint thread, int hang);

		[DllImport("DttSP.dll", EntryPoint="SetTXLevelerSt")]///
		public static extern void SetTXLevelerSt(uint thread, bool state);

        [DllImport("DttSP.dll", EntryPoint = "SetTXALCSt")]///
        public static extern void SetTXALCSt(uint thread, bool state);

		[DllImport("DttSP.dll", EntryPoint="SetWindow")]///
		public static extern void SetWindow(uint thread, Window windowset);

		[DllImport("DttSP.dll", EntryPoint="SetSpectrumPolyphase")]///
		public static extern void SetSpectrumPolyphase(uint thread, bool state);

		[DllImport("DttSP.dll", EntryPoint="SetBIN")]///
		public static extern void SetBIN(uint thread, uint subrx, bool setit);

		[DllImport("DttSP.dll", EntryPoint="SetSquelchVal")]///
		public static extern void SetSquelchVal(uint thread, uint subrx, float setit);

		[DllImport("DttSP.dll", EntryPoint="SetSquelchState")]///
		public static extern void SetSquelchState(uint thread, uint subrx, bool state);

		[DllImport("DttSP.dll", EntryPoint="SetTXSquelchVal")]///
		public static extern void SetTXSquelchVal(uint thread, float setit);

		[DllImport("DttSP.dll", EntryPoint="SetTXSquelchAtt")]///
		public static extern void SetTXSquelchAtt(uint thread, float setit);

		[DllImport("DttSP.dll", EntryPoint="SetTXSquelchSt")]///
		public static extern void SetTXSquelchState(uint thread, bool state);
#if false
		[DllImport("DttSP.dll", EntryPoint="SetTXCompandSt")]///
		public static extern void SetTXCompandSt(uint thread, bool state);

		[DllImport("DttSP.dll", EntryPoint="SetTXCompand")]///
		public static extern void SetTXCompand(uint thread, double setit);
#endif
        [DllImport("DttSP.dll", EntryPoint = "SetTXCompressorSt")]///
        public static extern void SetTXCompressorSt(uint thread, bool state);

        [DllImport("DttSP.dll", EntryPoint = "SetTXCompressor")]///
        public static extern void SetTXCompressor(uint thread, double setit);

		[DllImport("DttSP.dll", EntryPoint="SetPWSmode")]///
		public static extern void SetPWSmode(uint thread, uint subrx, bool setit);
		public static void SetTXPWSmode(uint thread, bool setit)
		{
			SetPWSmode(thread, 0, setit);
		}

		[DllImport("DttSP.dll", EntryPoint="Process_Spectrum")]
		unsafe public static extern void GetSpectrum(uint thread, float* results);

		[DllImport("DttSP.dll", EntryPoint="Process_ComplexSpectrum")]
		unsafe public static extern void GetComplexSpectrum(uint thread, float* results);

		[DllImport("DttSP.dll", EntryPoint="Process_Panadapter")]
		unsafe public static extern void GetPanadapter(uint thread, float* results);

		[DllImport("DttSP.dll", EntryPoint="Process_Phase")]
		unsafe public static extern void GetPhase(uint thread, float* results, int numpoints);

		[DllImport("DttSP.dll", EntryPoint="Process_Scope")]
		unsafe public static extern void GetScope(uint thread, float* results, int numpoints);

		[DllImport("DttSP.dll", EntryPoint="SetTRX")]
		unsafe public static extern void SetTRX(uint thread, bool trx_on);

        [DllImport("DttSP.dll", EntryPoint = "FlushAllBufs")]
        unsafe public static extern void FlushAllBufs(uint thread, bool trx);

		[DllImport("DttSP.dll", EntryPoint="CalculateRXMeter")]
		public static extern float CalculateRXMeter(uint thread, uint subrx, MeterType MT);

		[DllImport("DttSP.dll", EntryPoint="CalculateTXMeter")]
		public static extern float CalculateTXMeter(uint thread,MeterType MT);

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

		[DllImport("DttSP.dll", EntryPoint="SetThreadNo")]///
		unsafe public static extern void SetThreadNo(uint threadno);

		[DllImport("DttSP.dll", EntryPoint="SetThreadCom")]///
		unsafe public static extern void SetThreadCom(uint thread_com);

		[DllImport("DttSP.dll", EntryPoint="SetSubRXSt")]///
		unsafe public static extern void SetRXOn(uint thread, uint subrx, bool setit);

		[DllImport("DttSP.dll", EntryPoint="SetRXPan")]///
		unsafe public static extern void SetRXPan(uint thread, uint subrx, float pan); // takes values from 0 to 1.0 for L to R.

		[DllImport("DttSP.dll", EntryPoint="SetRXOsc")]///
		unsafe public static extern int SetRXOsc(uint thread, uint subrx, double freq);
		/*public static int SetRXOsc(uint thread, uint subrx, double freq)
		{
			Debug.WriteLine("SetRXOsc("+thread+", "+subrx+", "+freq.ToString("f6")+")");
			return SetRXOscDll(thread, subrx, freq);
		}*/

		[DllImport("DttSP.dll", EntryPoint="SetOscPhase")]///
		unsafe public static extern int SetOscPhase(double phase);

        [DllImport("DttSP.dll", EntryPoint = "SetKeyerHarmonicRemove")]
        public static extern void SetKeyerHarmonicRemove(double harmonic, double phase, double amplitude);
 
        [DllImport("DttSP.dll", EntryPoint = "SetMercuryXmit")]
        public static extern void SetMercuryXmit_DLL(int setit);
        public static void SetMercuryXmit(bool setit)
        {
            int i_setit = 0;
            if (setit)
            {
                i_setit = 1;
            }
            SetMercuryXmit_DLL(i_setit);
            return;
        }

        [DllImport("DttSP.dll", EntryPoint = "SetEerXmit")]
        public static extern void SetEerXmit_DLL(int setit);
        public static void SetEerXmit(bool setit)
        {
            int i_setit = 0;
            if (setit)
            {
                i_setit = 1;
            }
            SetEerXmit_DLL(i_setit);
            return;
        }
        

        #region Diversity

        [DllImport("DttSP.dll", EntryPoint = "SetDiversity")]
        unsafe public static extern int SetDiversity(int on);

        [DllImport("DttSP.dll", EntryPoint = "SetDiversityScalar")]
        unsafe public static extern int SetDiversityScalar(float real, float imag);

        [DllImport("DttSP.dll", EntryPoint = "SetDiversityGain")]
        unsafe public static extern int SetDiversityGain(float gain); // valid 0.0 - 1.0

		#endregion
 
		#endregion
	}
}
