//=================================================================
// audio.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2009  FlexRadio Systems
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


//#define VAC_DEBUG
//#define MINMAX
//#define TIMER
//#define INTERLEAVED
//#define SPLIT_INTERLEAVED


using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
	public class Audio
	{
		#region PowerSDR Specific Variables
		// ======================================================
		// PowerSDR Specific Variables
		// ======================================================

		public enum AudioState
		{
			DTTSP = 0,
			CW,
			/*SINL_COSR,
			SINL_SINR,			
			SINL_NOR,
			COSL_SINR,
			NOL_SINR,
			NOL_NOR,
			PIPE,*/
			//SWITCH,
		}

		public enum SignalSource
		{
			RADIO,
			SINE,
			SINE_TWO_TONE,
			SINE_LEFT_ONLY,
			SINE_RIGHT_ONLY,
			NOISE,
			TRIANGLE,
			SAWTOOTH,
			SILENCE,
		}

#if(INTERLEAVED)
#if(SPLIT_INTERLEAVED)
		unsafe private static PA19.PaStreamCallback callback1 = new PA19.PaStreamCallback(Callback1ILDI);	// Init callbacks to prevent GC
		unsafe private static PA19.PaStreamCallback callbackVAC = new PA19.PaStreamCallback(CallbackVACILDI);
		unsafe private static PA19.PaStreamCallback callback4port = new PA19.PaStreamCallback(Callback4PortILDI);
#else
		unsafe private static PA19.PaStreamCallback callback1 = new PA19.PaStreamCallback(Callback1IL);	// Init callbacks to prevent GC
		unsafe private static PA19.PaStreamCallback callbackVAC = new PA19.PaStreamCallback(CallbackVACIL);
		unsafe private static PA19.PaStreamCallback callback4port = new PA19.PaStreamCallback(Callback4PortIL);
#endif
#else
		unsafe private static PA19.PaStreamCallback callback1 = new PA19.PaStreamCallback(Callback1);	// Init callbacks to prevent GC
		unsafe private static PA19.PaStreamCallback callbackVAC = new PA19.PaStreamCallback(CallbackVAC);
		unsafe private static PA19.PaStreamCallback callback4port = new PA19.PaStreamCallback(Callback4Port);
		unsafe private static PA19.PaStreamCallback callback8 = new PA19.PaStreamCallback(Callback2);
#endif

		public static int callback_return = 0;

		/*private static bool spike = false;
		public static bool Spike
		{
			get { return spike; }
			set { spike = value; }
		}*/

		private static bool rx2_auto_mute_tx = true;
		public static bool RX2AutoMuteTX
		{
			get { return rx2_auto_mute_tx; }
			set { rx2_auto_mute_tx = value; }
		}

		private static double source_scale = 1.0;
		public static double SourceScale
		{
			get { return source_scale; }
			set { source_scale = value; }
		}

		private static SignalSource rx1_input_signal = SignalSource.RADIO;
		public static SignalSource RX1InputSignal
		{
			get { return rx1_input_signal; }
			set { rx1_input_signal = value; }
		}

		private static SignalSource rx1_output_signal = SignalSource.RADIO;
		public static SignalSource RX1OutputSignal
		{
			get { return rx1_output_signal; }
			set { rx1_output_signal = value; }
		}

		private static SignalSource rx2_input_signal = SignalSource.RADIO;
		public static SignalSource RX2InputSignal
		{
			get { return rx2_input_signal; }
			set { rx2_input_signal = value; }
		}

		private static SignalSource rx2_output_signal = SignalSource.RADIO;
		public static SignalSource RX2OutputSignal
		{
			get { return rx2_output_signal; }
			set { rx2_output_signal = value; }
		}

		private static SignalSource tx_input_signal = SignalSource.RADIO;
		public static SignalSource TXInputSignal
		{
			get { return tx_input_signal; }
			set { tx_input_signal = value; }
		}

		private static SignalSource tx_output_signal = SignalSource.RADIO;
		public static SignalSource TXOutputSignal
		{
			get { return tx_output_signal; }
			set { tx_output_signal = value; }
		}

		private static bool record_rx_preprocessed = true;
		public static bool RecordRXPreProcessed
		{
			get { return record_rx_preprocessed; }
			set { record_rx_preprocessed = value; }
		}

		private static bool record_tx_preprocessed = false;
		public static bool RecordTXPreProcessed
		{
			get { return record_tx_preprocessed; }
			set { record_tx_preprocessed = value; }
		}

		private static float peak = float.MinValue;
		public static float Peak
		{
			get { return peak; }
			set { peak = value; }
		}

		private static bool vox_enabled = false;
		public static bool VOXEnabled
		{
			get { return vox_enabled; }
			set { vox_enabled = value; }
		}

		private static float vox_threshold = 0.001f;
		public static float VOXThreshold
		{
			get { return vox_threshold; }
			set { vox_threshold = value; }
		}

        private static float vox_gain = 0.001f;
        public static float VOXGain
        {
            get { return vox_gain; }
            set { vox_gain = value; }
        }
        
		public static double TXScale
		{
			get { return high_swr_scale * radio_volume; }
		}

		public static double FWCTXScale
		{
			get { return high_swr_scale * temp_scale * radio_volume; }
		}

		private static double temp_scale = 1.0;
		public static double TempScale
		{
			get { return temp_scale; }
			set { temp_scale = value; }
		}

		private static double high_swr_scale = 1.0;
		public static double HighSWRScale
		{
			get { return high_swr_scale; }
			set	{ high_swr_scale = value; } 
		}

		private static double mic_preamp = 1.0;
		public static double MicPreamp
		{
			get { return mic_preamp; }
			set { mic_preamp = value; }
		}

		private static double wave_preamp = 1.0;
		public static double WavePreamp
		{
			get { return wave_preamp; }
			set { wave_preamp = value; }
		}

		private static double monitor_volume = 0.0;
		public static double MonitorVolume
		{
			get { return monitor_volume; }
			set
			{
				//Debug.WriteLine("monitor_volume: "+value.ToString("f3"));
				monitor_volume = value;
			}
		}

		private static double radio_volume = 0.0;
		public static double RadioVolume
		{
			get { return radio_volume; }
			set
			{
				//Debug.WriteLine("radio_volume: "+value.ToString("f3"));
				radio_volume = value;
                if (console.CurrentModel == Model.HERMES || console.PennyLanePresent )
                {
                    JanusAudio.SetOutputPower((float)radio_volume);
                }
                else
                {
                    JanusAudio.SetOutputPower((float)0.0);
                }
             }
		}

		/*private static bool next_mox = false;
		public static bool NextMox
		{
			get { return next_mox; }
			set { next_mox = value; }
		}*/

		/*private static int ramp_samples = (int)(sample_rate1*0.005);
		private static double ramp_step = 1.0/ramp_samples;
		private static int ramp_count = 0;
		private static double ramp_val = 0.0;*/

		/*private static bool ramp_down = false;
		public static bool RampDown
		{
			get { return ramp_down; }
			set
			{
				ramp_down = value;
				ramp_samples = (int)(sample_rate1*0.005);
				ramp_step = 1.0/ramp_samples;
				ramp_count = 0;
				ramp_val = 1.0;
			}
		}

		private static bool ramp_up = false;
		public static bool RampUp
		{
			get { return ramp_up; }
			set
			{
				ramp_up = value;
				ramp_samples = (int)(sample_rate1*0.005);
				ramp_step = 1.0/ramp_samples;
				ramp_count = 0;
				ramp_val = 0.0;
			}
		}

		

		private static int ramp_up_num = 1;
		public static int RampUpNum
		{
			get { return ramp_up_num; }
			set	{ ramp_up_num = value; }
		}

		private static int switch_count = 1;
		public static int SwitchCount
		{
			get {return switch_count; }
			set {switch_count = value; }
		}*/

		private static AudioState current_audio_state1 = AudioState.DTTSP;
		public static AudioState CurrentAudioState1
		{
			get { return current_audio_state1; }
			set { current_audio_state1 = value; }
		}

		/*private static AudioState next_audio_state1 = AudioState.NOL_NOR;
		public static AudioState NextAudioState1
		{
			get { return next_audio_state1; }
			set { next_audio_state1 = value; }
		}*/

		/*private static AudioState save_audio_state1 = AudioState.NOL_NOR;
		public static AudioState SaveAudioState1
		{
			get { return save_audio_state1; }
			set { save_audio_state1 = value; }
		}*/

		private static double sine_freq1 = 1250.0;
		private static double phase_step1 = sine_freq1/sample_rate1*2*Math.PI;
		private static double phase_accumulator1 = 0.0;

		private static double sine_freq2 = 1900.0;
		private static double phase_step2 = sine_freq2/sample_rate1*2*Math.PI;
		private static double phase_accumulator2 = 0.0;

		public static double SineFreq1
		{
			get { return sine_freq1; }
			set
			{
				sine_freq1 = value;
				phase_step1 = sine_freq1/sample_rate1*2*Math.PI;
			}
		}

		public static double SineFreq2
		{
			get { return sine_freq2; }
			set
			{
				sine_freq2 = value;
				phase_step2 = sine_freq2/sample_rate1*2*Math.PI;
			}
		}

        private static double fm_pl_tone = 254.1;
        private static double phase_accumulator3 = 0.0;
        public static double FMPLTone
        {
            get { return fm_pl_tone; }
            set { fm_pl_tone = value; }
        }

        private static float fm_pl_dev = 0.005f;
        public static float FMPLDev
        {
            get { return fm_pl_dev; }
            set { fm_pl_dev = value; }
        }

		private static int in_rx1_l = 0;
		public static int IN_RX1_L
		{
			get { return in_rx1_l; }
			set { in_rx1_l = value; }
		}

		private static int in_rx1_r = 1;
		public static int IN_RX1_R
		{
			get { return in_rx1_r; }
			set { in_rx1_r = value; }
		}

		private static int in_rx2_l = 4;
		public static int IN_RX2_L
		{
			get { return in_rx2_l; }
			set { in_rx2_l = value; }
		}

		private static int in_rx2_r = 5;
		public static int IN_RX2_R
		{
			get { return in_rx2_r; }
			set { in_rx2_r = value; }
		}

		private static int in_tx_l = 2;
		public static int IN_TX_L
		{
			get { return in_tx_l; }
			set 
			{
				in_tx_l = value;
				switch(in_tx_l)
				{
					case 4:
					case 5:
					case 6:
						in_tx_r = in_tx_l+1;
						break;
					case 7:
						in_tx_r = 4;
						break;
				}
			}
		}

		private static int in_tx_r = 3;
		public static int IN_TX_R
		{
			get { return in_tx_r; }
			set { in_tx_r = value; }
		}

		private static bool rx2_enabled = false;
		public static bool RX2Enabled
		{
			get { return rx2_enabled; }
			set { rx2_enabled = value; }
		}

		public static Console console = null;
		unsafe private static void *stream1;
		unsafe private static void *stream2;
		//private static int block_size2 = 2048;
		public static float[] phase_buf_l;
		public static float[] phase_buf_r;
		public static bool phase = false;
		public static bool wave_record = false;
		public static bool wave_playback = false;
		public static WaveFileWriter wave_file_writer;
		public static WaveFileReader wave_file_reader;
		public static bool two_tone = false;
		//public static Mutex phase_mutex = new Mutex();
		public static bool high_pwr_am = false;
		public static bool testing = false;
		private static bool localmox;

        private static int empty_buffers = 0;
        public static int EmptyBuffers
        {
            get { return empty_buffers; }
        }

        private static bool test_mute = false;
        public static bool TestMute
        {
            get { return test_mute; }
            set { test_mute = value; }
        }

		#region VAC Variables

		private static RingBufferFloat rb_vacIN_l;
		private static RingBufferFloat rb_vacIN_r;
		private static RingBufferFloat rb_vacOUT_l;
		private static RingBufferFloat rb_vacOUT_r;

		private static float[] res_inl;
		private static float[] res_inr;
		private static float[] res_outl;
		private static float[] res_outr;

		unsafe private static void *resampPtrIn_l;
		unsafe private static void *resampPtrIn_r;
		unsafe private static void *resampPtrOut_l;
		unsafe private static void *resampPtrOut_r;

		private static bool vac_resample = false;
		private static bool vac_combine_input = false;
		public static bool VACCombineInput
		{
			get { return vac_combine_input; }
			set { vac_combine_input = value; }
		}

		#endregion

		#endregion

		#region Local Copies of External Properties

		private static bool mox = false;
		public static bool MOX
		{
			set { mox = value; }
		}

		unsafe private static void *cs_vac;

		private static bool mon = false;
		public static bool MON
		{
			set { mon = value; }
		}

		private static bool full_duplex = false;
		public static bool FullDuplex
		{
			set { full_duplex = value; }
		}

		private static bool vac_enabled = false;
		public static bool VACEnabled
		{
			set
			{
				vac_enabled = value;
				if(vac_enabled) InitVAC();
				else CleanUpVAC();
			}
			get { return vac_enabled; }
		}

		private static bool vac_bypass = false;
		public static bool VACBypass
		{
			get { return vac_bypass; }
			set { vac_bypass = value; }
		}

		private static bool vac_rb_reset = false;
		public static bool VACRBReset
		{
			set
			{
				vac_rb_reset = value;
			}
			get { return vac_rb_reset; }
		}

		private static double vac_preamp = 1.0;
		public static double VACPreamp
		{
			get { return vac_preamp; }
			set
			{
				//Debug.WriteLine("vac_preamp: "+value.ToString("f3"));
				vac_preamp = value;
			}
		}

		private static double vac_rx_scale = 1.0;
		public static double VACRXScale
		{
			get { return vac_rx_scale; }
			set
			{
				//Debug.WriteLine("vac_rx_scale: "+value.ToString("f3"));
				vac_rx_scale = value;
			}
		}

		private static DSPMode tx_dsp_mode = DSPMode.LSB;
		public static DSPMode TXDSPMode
		{
			get { return tx_dsp_mode; }
			set { tx_dsp_mode = value; }
		}

		private static int sample_rate1 = 48000;
		public static int SampleRate1
		{
			get { return sample_rate1; }
			set	
			{
				sample_rate1 = value;
				SineFreq1 = sine_freq1;
				SineFreq2 = sine_freq2;
			}
		}

		private static int sample_rate2 = 48000;
		public static int SampleRate2
		{
			get { return sample_rate2; }
			set 
			{
				sample_rate2 = value; 
				if(vac_enabled) InitVAC();
			}			
		}

		private static int block_size1 = 1024;
		public static int BlockSize
		{
			get { return block_size1; }
			set { block_size1 = value; }			
		}

		private static int block_size_vac = 2048;
		public static int BlockSizeVAC
		{
			get { return block_size_vac; }
			set { block_size_vac = value; }			
		}

		private static double audio_volts1 = 2.23;
		public static double AudioVolts1
		{
			get { return audio_volts1; }
			set { audio_volts1 = value; }
		}

		private static bool vac_stereo = false;
		public static bool VACStereo
		{
			set { vac_stereo = value; }
		}

		private static bool vac_output_iq = false;
		public static bool VACOutputIQ
		{
			set { vac_output_iq = value; }
		}

        private static bool vac_output_rx2 = false;
        public static bool VACOutputRX2
        {
            set { vac_output_rx2 = value; }
        }

		private static float iq_phase = 0.0f;
		public static float IQPhase
		{
			set { iq_phase = value; }
		}

		private static float iq_gain = 1.0f;
		public static float IQGain
		{
			set { iq_gain = value; }
		}

        private static float iq_phase2 = 0.0f;
        public static float IQPhase2
        {
            set { iq_phase2 = value; }
        }

        private static float iq_gain2 = 1.0f;
        public static float IQGain2
        {
            set { iq_gain2 = value; }
        }

		private static bool vac_correct_iq = true;
		public static bool VACCorrectIQ
		{
			set { vac_correct_iq = value; }
		}

		private static SoundCard soundcard = SoundCard.UNSUPPORTED_CARD;
		public static SoundCard CurSoundCard
		{
			set { soundcard = value; }
		}

		private static bool vox_active = false;
		public static bool VOXActive
		{
			get { return vox_active; }
			set { vox_active = value; }
		}

		private static int num_channels = 2;
		public static int NumChannels
		{
			get { return num_channels; }
			set { num_channels = value; }
		}

		private static int host1 = 0;
		public static int Host1
		{
			get { return host1; }
			set { host1 = value; }
		}

		private static int host2 = 0;
		public static int Host2
		{
			get { return host2; }
			set { host2 = value; }
		}

		private static int input_dev1 = 0;
		public static int Input1
		{
			get { return input_dev1; }
			set { input_dev1 = value; }
		}

		private static int input_dev2 = 0;
		public static int Input2
		{
			get { return input_dev2; }
			set { input_dev2 = value; }
		}

		private static int output_dev1 = 0;
		public static int Output1
		{
			get { return output_dev1; }
			set { output_dev1 = value; }
		}

		private static int output_dev2 = 0;
		public static int Output2
		{
			get { return output_dev2; }
			set { output_dev2 = value; }
		}

		private static int latency1 = 0;
		public static int Latency1
		{
			get { return latency1; }
			set { latency1 = value; }
		}

		private static int latency2 = 120;
		public static int Latency2
		{
			set { latency2 = value; }
		}

		#endregion

		#region Callback Routines
		// ======================================================
		// Callback Routines
		// ======================================================

		unsafe public static int Callback1(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
#if(TIMER)
			t1.Start();
#endif

			int* array_ptr = (int *)input;
			float* in_l_ptr1 = (float *)array_ptr[0];
			float* in_r_ptr1 = (float *)array_ptr[1];
			array_ptr = (int *)output;
			float* out_l_ptr1 = (float *)array_ptr[0];
			float* out_r_ptr1 = (float *)array_ptr[1];


			if(wave_playback)
				wave_file_reader.GetPlayBuffer(in_l_ptr1, in_r_ptr1);
			if((wave_record && !mox && record_rx_preprocessed) ||
				(wave_record && mox && record_tx_preprocessed))
				wave_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1);			

			if(phase)
			{
				//phase_mutex.WaitOne();
				Marshal.Copy(new IntPtr(in_l_ptr1), phase_buf_l, 0, frameCount);
				Marshal.Copy(new IntPtr(in_r_ptr1), phase_buf_r, 0, frameCount);
				//phase_mutex.ReleaseMutex();
			}

			float* in_l = null, in_r = null, out_l = null, out_r = null;

			in_l = in_l_ptr1;
			in_r = in_r_ptr1;

			out_l = out_l_ptr1;
			out_r = out_r_ptr1;

			switch(current_audio_state1)
			{
				case AudioState.DTTSP:
					if (tx_dsp_mode == DSPMode.CWU	|| tx_dsp_mode == DSPMode.CWL)
					{
						DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
					}

					// scale input with mic preamp
					if(mox && !vac_enabled &&
						(tx_dsp_mode == DSPMode.LSB ||
						tx_dsp_mode == DSPMode.USB ||
						tx_dsp_mode == DSPMode.DSB ||
						tx_dsp_mode == DSPMode.AM  ||
						tx_dsp_mode == DSPMode.SAM ||
						tx_dsp_mode == DSPMode.FMN))
					{
						if(wave_playback)
						{
							ScaleBuffer(in_l, in_l, frameCount, (float)wave_preamp);
							ScaleBuffer(in_r, in_r, frameCount, (float)wave_preamp);
						}
						else
						{
							ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
							ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);
						}
					}

					#region Input Signal Source

					if(!mox)
					{
						switch(rx1_input_signal)
						{
							case SignalSource.RADIO:
								break;
							case SignalSource.SINE:
								SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
								phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
								ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_TWO_TONE:
								double dump;
								SineWave2Tone(in_l, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out dump, out dump);
								CosineWave2Tone(in_r, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
								ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
								ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
								break;
							case SignalSource.NOISE:
								Noise(in_l, frameCount);
								Noise(in_r, frameCount);
								break;
							case SignalSource.TRIANGLE:
								Triangle(in_l, frameCount, sine_freq1);
								CopyBuffer(in_l, in_r, frameCount);
								break;
							case SignalSource.SAWTOOTH:
								Sawtooth(in_l, frameCount, sine_freq1);
								CopyBuffer(in_l, in_r, frameCount);
								break;
							case SignalSource.SILENCE:
								ClearBuffer(in_l, frameCount);
								ClearBuffer(in_r, frameCount);
								break;
						}
					}
					else
					{
						switch(tx_input_signal)
						{
							case SignalSource.RADIO:
								break;
							case SignalSource.SINE:
								SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
								phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
								ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_TWO_TONE:
								double dump;
								SineWave2Tone(in_l, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out dump, out dump);
								CosineWave2Tone(in_r, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
								ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
								ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
								break;
							case SignalSource.NOISE:
								Noise(in_l, frameCount);
								Noise(in_r, frameCount);
								break;
							case SignalSource.TRIANGLE:
								Triangle(in_l, frameCount, sine_freq1);
								CopyBuffer(in_l, in_r, frameCount);
								break;
							case SignalSource.SAWTOOTH:
								Sawtooth(in_l, frameCount, sine_freq1);
								CopyBuffer(in_l, in_r, frameCount);
								break;
							case SignalSource.SILENCE:
								ClearBuffer(in_l, frameCount);
								ClearBuffer(in_r, frameCount);
								break;
						}
					}

					#endregion

#if(MINMAX)
					Debug.Write(MaxSample(in_l, in_r, frameCount).ToString("f6")+",");
#endif

					if (vac_enabled &&
						rb_vacIN_l != null && rb_vacIN_r != null && 
						rb_vacOUT_l != null && rb_vacOUT_r != null)
					{
						if (mox)
						{
							if (rb_vacIN_l.ReadSpace() >= frameCount) rb_vacIN_l.ReadPtr(in_l,frameCount);
							else
							{
								ClearBuffer(in_l,frameCount);
								VACDebug("rb_vacIN_l underflow");
							}
							if (rb_vacIN_r.ReadSpace() >= frameCount) rb_vacIN_r.ReadPtr(in_r,frameCount);
							else
							{
								ClearBuffer(in_r,frameCount);
								VACDebug("rb_vacIN_r underflow");
							}
							ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
							ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
							DttSP.ExchangeSamples(in_l, in_r, out_l, out_r, frameCount);
						}
						else
						{
							DttSP.ExchangeSamples(in_l, in_r, out_l, out_r, frameCount);

							if((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
							{
								Win32.EnterCriticalSection(cs_vac);
								rb_vacIN_l.ReadPtr(in_l,frameCount);
								rb_vacIN_r.ReadPtr(in_r,frameCount);
								Win32.LeaveCriticalSection(cs_vac);
							}
							else
							{
								ClearBuffer(in_l,frameCount);
								ClearBuffer(in_r,frameCount);
								VACDebug("rb_vacIN_l underflow");
								VACDebug("rb_vacIN_r underflow");
							}
						}
					} 
					else DttSP.ExchangeSamples(in_l, in_r, out_l, out_r, frameCount);

#if(MINMAX)
					Debug.WriteLine(MaxSample(out_l, out_r, frameCount));
#endif

					#region Output Signal Source

					if(!mox)
					{
						switch(rx1_output_signal)
						{
							case SignalSource.RADIO:
								break;
							case SignalSource.SINE:
								SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
								phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
								ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_TWO_TONE:
								double dump;
								SineWave2Tone(out_l_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out dump, out dump);
								CosineWave2Tone(out_r_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
								ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
								ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
								break;
							case SignalSource.NOISE:
								Noise(out_l_ptr1, frameCount);
								Noise(out_r_ptr1, frameCount);
								break;
							case SignalSource.TRIANGLE:
								Triangle(out_l_ptr1, frameCount, sine_freq1);
								CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
								break;
							case SignalSource.SAWTOOTH:
								Sawtooth(out_l_ptr1, frameCount, sine_freq1);
								CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
								break;
							case SignalSource.SILENCE:
								ClearBuffer(out_l_ptr1, frameCount);
								ClearBuffer(out_r_ptr1, frameCount);
								break;
						}
					}
					else
					{
						switch(tx_output_signal)
						{
							case SignalSource.RADIO:
								break;
							case SignalSource.SINE:
								SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
								phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
								ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_TWO_TONE:
								double dump;
								SineWave2Tone(out_l_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out dump, out dump);
								CosineWave2Tone(out_r_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
								ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
								ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
								break;
							case SignalSource.NOISE:
								Noise(out_l_ptr1, frameCount);
								Noise(out_r_ptr1, frameCount);
								break;
							case SignalSource.TRIANGLE:
								Triangle(out_l_ptr1, frameCount, sine_freq1);
								CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
								break;
							case SignalSource.SAWTOOTH:
								Sawtooth(out_l_ptr1, frameCount, sine_freq1);
								CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
								break;
							case SignalSource.SILENCE:
								ClearBuffer(out_l_ptr1, frameCount);
								ClearBuffer(out_r_ptr1, frameCount);
								break;
						}
					}

					#endregion

					break;
				case AudioState.CW: 
					DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
					break;
				/*case AudioState.SINL_COSR:
					if(two_tone)
					{
						double dump;

						SineWave2Tone(out_l_ptr1, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out dump, out dump);

						CosineWave2Tone(out_r_ptr1, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2);
					}
					else
					{
						SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
					}
					break;
				case AudioState.SINL_SINR:
					if(two_tone)
					{
						SineWave2Tone(out_l_ptr1, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2);

						CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
					}
					else
					{
						phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
						CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
					}
					break;
				case AudioState.SINL_NOR:
					if(two_tone)
					{
						SineWave2Tone(out_l_ptr1, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2);
						ClearBuffer(out_r_ptr1, frameCount);
					}
					else
					{
						phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
						ClearBuffer(out_r_ptr1, frameCount);
					}
					break;
				case AudioState.COSL_SINR:
					if(two_tone)
					{
						double dump;
						
						CosineWave2Tone(out_l_ptr1, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out dump, out dump);

						SineWave2Tone(out_r_ptr1, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2);
					}
					else
					{
						CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
					}
					break;
				case AudioState.NOL_SINR:
					if(two_tone)
					{
						ClearBuffer(out_l_ptr1, frameCount);
						SineWave2Tone(out_r_ptr1, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2);						
					}
					else
					{
						ClearBuffer(out_l_ptr1, frameCount);
						phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
					}
					break;
				case AudioState.NOL_NOR:
					ClearBuffer(out_l_ptr1, frameCount);
					ClearBuffer(out_r_ptr1, frameCount);
					break;
				case AudioState.PIPE:
					CopyBuffer(in_l_ptr1, out_l_ptr1, frameCount);
					CopyBuffer(in_r_ptr1, out_r_ptr1, frameCount);
					break;*/
			}

			DoScope(out_l_ptr1, frameCount);

			double vol = monitor_volume;
			if(mox)
			{
                if (tx_output_signal != SignalSource.RADIO)
                    vol = 1.0f;
                else
    				vol = TXScale;

				if(high_pwr_am)
				{
					if(tx_dsp_mode == DSPMode.AM ||
						tx_dsp_mode == DSPMode.SAM)
						vol *= 1.414;
				}
			}

			ScaleBuffer(out_l, out_l, frameCount, (float)vol);
			ScaleBuffer(out_r, out_r, frameCount, (float)vol);

			if(vac_enabled &&
				rb_vacIN_l != null && rb_vacIN_r != null && 
				rb_vacOUT_l != null && rb_vacOUT_r != null)
			{
				if (sample_rate2 == sample_rate1) 
				{
					if ((rb_vacOUT_l.WriteSpace()>=frameCount) && (rb_vacOUT_r.WriteSpace()>=frameCount))
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacOUT_l.WritePtr(out_l,frameCount);
						rb_vacOUT_r.WritePtr(out_r,frameCount);
						Win32.LeaveCriticalSection(cs_vac);
					}
					else 
					{
						VACDebug("rb_vacOUT_l overflow");
						VACDebug("rb_vacOUT_r overflow");
					}
				} 
				else 
				{
					if (vac_stereo) 
					{						
						fixed(float *res_outl_ptr = &(res_outl[0]))
							fixed(float *res_outr_ptr = &(res_outr[0])) 
							{
								int outsamps;
								DttSP.DoResamplerF(out_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
								DttSP.DoResamplerF(out_r, res_outr_ptr, frameCount, &outsamps, resampPtrOut_r);
								if ((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
								{
									Win32.EnterCriticalSection(cs_vac);
									rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
									rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
									Win32.LeaveCriticalSection(cs_vac);
								}
								else 
								{
									VACDebug("rb_vacOUT_l overflow");
									VACDebug("rb_vacOUT_r overflow");
								}
							}
					}
					else 
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))
						{
							int outsamps;
							DttSP.DoResamplerF(out_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
							if ((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
							{
								Win32.EnterCriticalSection(cs_vac);
								rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
								rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
								Win32.LeaveCriticalSection(cs_vac);
							}
							else
							{
								VACDebug("rb_vacOUT_l overflow");
								VACDebug("rb_vacOUT_r overflow");
							}
						}
					}
				}
			}

#if(MINMAX)
			Debug.Write(MaxSample(out_l, out_r, frameCount).ToString("f6")+",");

			float current_max = MaxSample(out_l, out_r, frameCount);
			if(current_max > max) max = current_max;
			Debug.WriteLine(" max: "+max.ToString("f6"));
#endif

			if((wave_record && !mox && !record_rx_preprocessed) ||
				(wave_record && mox && !record_tx_preprocessed))
				wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);	
#if(TIMER)
			t1.Stop();
			Debug.WriteLine(t1.Duration);
#endif

			return callback_return;			
		}
		
#if(TIMER)
		private static HiPerfTimer t1 = new HiPerfTimer();
#endif
		//private static int count = 0;
		unsafe public static int Callback4Port(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
#if(TIMER)
			t1.Start();
#endif
			float* in_l = null, in_r = null, out_l = null, out_r = null;
			float* out_l1 = null, out_r1 = null, out_l2 = null, out_r2 = null;
            float* ctcss_l = null, ctcss_r = null;
			localmox = mox;

			void* ex_input  = (void *)input;
			void* ex_output = (void *)output;
			
			int* array_ptr_input = (int *)input;
			float* in_l_ptr1 = (float *)array_ptr_input[0];
			float* in_r_ptr1 = (float *)array_ptr_input[1];
			float* in_l_ptr2 = (float *)array_ptr_input[2];
			float* in_r_ptr2 = (float *)array_ptr_input[3];
			
			int* array_ptr_output = (int *)output;
			float* out_l_ptr1 = (float *)array_ptr_output[0];
			float* out_r_ptr1 = (float *)array_ptr_output[1];
			float* out_l_ptr2 = (float *)array_ptr_output[2];
			float* out_r_ptr2 = (float *)array_ptr_output[3];

			// arrange input buffers in the following order:
			// RX1 Left, RX1 Right, TX Left, TX Right, RX2 Left, RX2 Right
			int* array_ptr = (int *)input;
			switch(in_rx1_l)
			{
				case 0: array_ptr[0] = (int)in_l_ptr1; break;
				case 1: array_ptr[0] = (int)in_r_ptr1; break;
				case 2: array_ptr[0] = (int)in_l_ptr2; break;
				case 3: array_ptr[0] = (int)in_r_ptr2; break;
			}
			
			switch(in_rx1_r)
			{
				case 0: array_ptr[1] = (int)in_l_ptr1; break;
				case 1: array_ptr[1] = (int)in_r_ptr1; break;
				case 2: array_ptr[1] = (int)in_l_ptr2; break;
				case 3: array_ptr[1] = (int)in_r_ptr2; break;
			}
			
			switch(in_tx_l)
			{
				case 0: array_ptr[2] = (int)in_l_ptr1; break;
				case 1: array_ptr[2] = (int)in_r_ptr1; break;
				case 2: array_ptr[2] = (int)in_l_ptr2; break;
				case 3: array_ptr[2] = (int)in_r_ptr2; break;
			}
				
			switch(in_tx_r)
			{
				case 0: array_ptr[3] = (int)in_l_ptr1; break;
				case 1: array_ptr[3] = (int)in_r_ptr1; break;
				case 2: array_ptr[3] = (int)in_l_ptr2; break;
				case 3: array_ptr[3] = (int)in_r_ptr2; break;
			}

			/*switch(in_rx2_l)
			{
				case 0: break;
				case 1: array_ptr[4] = (int)in_r_ptr1; break;
				case 2: array_ptr[4] = (int)in_l_ptr2; break;
				case 3: array_ptr[4] = (int)in_r_ptr2; break;
			}
			switch(in_rx2_r)
			{
				case 0: break;
				case 1: array_ptr[5] = (int)in_r_ptr1; break;
				case 2: array_ptr[5] = (int)in_l_ptr2; break;
				case 3: array_ptr[5] = (int)in_r_ptr2; break;
			}*/
	
			if(!localmox)
			{
				in_l = (float *)array_ptr_input[0];
				in_r = (float *)array_ptr_input[1];
			}
			else
			{
				in_l = (float *)array_ptr_input[2];
				in_r = (float *)array_ptr_input[3];
                ctcss_l = (float *)array_ptr_input[0];
                ctcss_r = (float *)array_ptr_input[1];
			}

			if(wave_playback)
				wave_file_reader.GetPlayBuffer(in_l, in_r);
			if(wave_record)
			{
				if(!localmox)
				{
					if(record_rx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(in_l, in_r);
					}
				}
				else
				{
					if(record_tx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(in_l, in_r);
					}
				}
			}				

			if(phase)
			{
				//phase_mutex.WaitOne();
				Marshal.Copy(new IntPtr(in_l), phase_buf_l, 0, frameCount);
				Marshal.Copy(new IntPtr(in_r), phase_buf_r, 0, frameCount);
				//phase_mutex.ReleaseMutex();
			}

			// handle VAC Input
			if(vac_enabled && 
				rb_vacIN_l != null && rb_vacIN_r != null && 
				rb_vacOUT_l != null && rb_vacOUT_r != null)
			{
				if(vac_bypass || !localmox) // drain VAC Input ring buffer
				{
					if ((rb_vacIN_l.ReadSpace() >= frameCount)&&(rb_vacIN_r.ReadSpace() >= frameCount))
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacIN_l.ReadPtr(out_l_ptr2, frameCount);
						rb_vacIN_r.ReadPtr(out_r_ptr2, frameCount);
						Win32.LeaveCriticalSection(cs_vac);
					}
				}
				else
				{
					if(rb_vacIN_l.ReadSpace() >= frameCount) 
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacIN_l.ReadPtr(in_l, frameCount);
						rb_vacIN_r.ReadPtr(in_r, frameCount);
						Win32.LeaveCriticalSection(cs_vac);
						if(vac_combine_input)
							AddBuffer(in_l, in_r, frameCount);
					}
					else
					{
						ClearBuffer(in_l, frameCount);
						ClearBuffer(in_r, frameCount);
						VACDebug("rb_vacIN underflow 4inTX");
					}
					ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
					ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
				}
			}

			switch(current_audio_state1)
			{
				case AudioState.DTTSP:                    
					#region VOX
 					if(vox_enabled)
					{
                        if (console.CurrentModel == Model.HPSDR || console.CurrentModel == Model.HERMES)
                        {                       
                            if (tx_dsp_mode == DSPMode.LSB ||
                                tx_dsp_mode == DSPMode.USB ||
                                tx_dsp_mode == DSPMode.DSB ||
                                tx_dsp_mode == DSPMode.AM ||
                                tx_dsp_mode == DSPMode.SAM ||
                                tx_dsp_mode == DSPMode.FMN)
                            {                                
                                peak = MaxSample(in_l_ptr2, in_r_ptr2, frameCount);

                                // compare power to threshold
                                if (peak > vox_threshold)
                                    vox_active = true;
                                else
                                    vox_active = false;
                            }
                        }
                        else
                        {
                            float* vox_l = null, vox_r = null;
                            switch (soundcard)
                            {
                                case SoundCard.FIREBOX:
                                case SoundCard.EDIROL_FA_66:
                                    vox_l = in_l_ptr1;
                                    vox_r = in_r_ptr1;
                                    break;
                                case SoundCard.DELTA_44:
                                default:
                                    vox_l = in_l_ptr2;
                                    vox_r = in_r_ptr2;
                                    break;
                            }

                            if (tx_dsp_mode == DSPMode.LSB ||
                                tx_dsp_mode == DSPMode.USB ||
                                tx_dsp_mode == DSPMode.DSB ||
                                tx_dsp_mode == DSPMode.AM ||
                                tx_dsp_mode == DSPMode.SAM ||
                                tx_dsp_mode == DSPMode.FMN)
                            {
                                peak = MaxSample(vox_l, vox_r, frameCount);

                                // compare power to threshold
                                if (peak > vox_threshold)
                                    vox_active = true;
                                else
                                    vox_active = false;
                            }
                        }                            
					}

					#endregion

					if (tx_dsp_mode == DSPMode.CWU || tx_dsp_mode == DSPMode.CWL)
					{
						DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
					} 

					// scale input with mic preamp
					if((!vac_enabled &&
						(tx_dsp_mode == DSPMode.LSB ||
						tx_dsp_mode == DSPMode.USB ||
						tx_dsp_mode == DSPMode.DSB ||
						tx_dsp_mode == DSPMode.AM  ||
						tx_dsp_mode == DSPMode.SAM ||
						tx_dsp_mode == DSPMode.FMN ||
						tx_dsp_mode == DSPMode.DIGL ||
						tx_dsp_mode == DSPMode.DIGU)) ||
						(vac_enabled && vac_bypass &&
						(tx_dsp_mode == DSPMode.DIGL ||
						tx_dsp_mode == DSPMode.DIGU ||
						tx_dsp_mode == DSPMode.LSB ||
						tx_dsp_mode == DSPMode.USB ||
						tx_dsp_mode == DSPMode.DSB ||
						tx_dsp_mode == DSPMode.AM ||
						tx_dsp_mode == DSPMode.SAM ||
						tx_dsp_mode == DSPMode.FMN)))
					{
						if(wave_playback)
						{
							ScaleBuffer(in_l, in_l, frameCount, (float)wave_preamp);
							ScaleBuffer(in_r, in_r, frameCount, (float)wave_preamp);
						}
						else
						{
							if(localmox)
							{
								if(!vac_enabled && (tx_dsp_mode == DSPMode.DIGL || tx_dsp_mode == DSPMode.DIGU))
								{
									ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
									ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
								}
								else
								
                                    if (tx_dsp_mode == DSPMode.FMN && console.PLTone == true)
                                    {
                                        SineWave(ctcss_l, frameCount, phase_accumulator3, fm_pl_tone);
                                        phase_accumulator3 = CosineWave(ctcss_r, frameCount, phase_accumulator3, fm_pl_tone);

                                        MixBuffer(ctcss_l, in_l, in_l, frameCount, (float)fm_pl_dev, (float)mic_preamp);
                                        MixBuffer(ctcss_r, in_r, in_r, frameCount, (float)fm_pl_dev, (float)mic_preamp);
                                    }

                                    else

									{
									ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
									ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);
								}
							}
						}
					}
                    
 
					#region Input Signal Source

					if(!mox)
					{
						switch(rx1_input_signal)
						{
							case SignalSource.RADIO:
								break;
							case SignalSource.SINE:
								SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
								phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
								ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_TWO_TONE:
								double dump;
								SineWave2Tone(in_l, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out dump, out dump);
								CosineWave2Tone(in_r, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
								ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
								ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_LEFT_ONLY:
								phase_accumulator1 = SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
								ClearBuffer(in_r, frameCount);
								break;
							case SignalSource.SINE_RIGHT_ONLY:
								phase_accumulator1 = SineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
								ClearBuffer(in_l, frameCount);
								break;
							case SignalSource.NOISE:
								Noise(in_l, frameCount);
								Noise(in_r, frameCount);
								break;
							case SignalSource.TRIANGLE:
								Triangle(in_l, frameCount, sine_freq1);
								CopyBuffer(in_l, in_r, frameCount);
								break;
							case SignalSource.SAWTOOTH:
								Sawtooth(in_l, frameCount, sine_freq1);
								CopyBuffer(in_l, in_r, frameCount);
								break;
							case SignalSource.SILENCE:
								ClearBuffer(in_l, frameCount);
								ClearBuffer(in_r, frameCount);
								break;
						}
					}
					else
					{
						switch(tx_input_signal)
						{
							case SignalSource.RADIO:
								break;
							case SignalSource.SINE:
								SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
								phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
								ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_TWO_TONE:
								double dump;
								SineWave2Tone(in_l, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out dump, out dump);
								CosineWave2Tone(in_r, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
								ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
								ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
								break;
							case SignalSource.NOISE:
								Noise(in_l, frameCount);
								Noise(in_r, frameCount);
								break;
							case SignalSource.TRIANGLE:
								Triangle(in_l, frameCount, sine_freq1);
								CopyBuffer(in_l, in_r, frameCount);
								break;
							case SignalSource.SAWTOOTH:
								Sawtooth(in_l, frameCount, sine_freq1);
								CopyBuffer(in_l, in_r, frameCount);
								break;
							case SignalSource.SILENCE:
								ClearBuffer(in_l, frameCount);
								ClearBuffer(in_r, frameCount);
								break;
						}
					}

					#endregion

#if(MINMAX)
					Debug.Write(MaxSample(in_l, in_r, frameCount).ToString("f6")+",");
#endif

					DttSP.ExchangeSamples2(ex_input, ex_output, frameCount);

#if(MINMAX)
					Debug.Write(MaxSample(out_l_ptr1, out_r_ptr1, frameCount).ToString("f6")+",");
#endif

					#region Output Signal Source

					if(!mox)
					{
						switch(rx1_output_signal)
						{
							case SignalSource.RADIO:
								break;
							case SignalSource.SINE:
								SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
								phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
								ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_TWO_TONE:
								double dump;
								SineWave2Tone(out_l_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out dump, out dump);
								CosineWave2Tone(out_r_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
								ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
								ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_LEFT_ONLY:
								phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
								ClearBuffer(out_r_ptr1, frameCount);
								break;
							case SignalSource.SINE_RIGHT_ONLY:
								phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
								ClearBuffer(out_l_ptr1, frameCount);
								break;
							case SignalSource.NOISE:
								Noise(out_l_ptr1, frameCount);
								Noise(out_r_ptr1, frameCount);
								break;
							case SignalSource.TRIANGLE:
								Triangle(out_l_ptr1, frameCount, sine_freq1);
								CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
								break;
							case SignalSource.SAWTOOTH:
								Sawtooth(out_l_ptr1, frameCount, sine_freq1);
								CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
								break;
							case SignalSource.SILENCE:
								ClearBuffer(out_l_ptr1, frameCount);
								ClearBuffer(out_r_ptr1, frameCount);
								break;
						}
					}
					else
					{
						switch(tx_output_signal)
						{
							case SignalSource.RADIO:
								break;
							case SignalSource.SINE:
								SineWave(out_l_ptr2, frameCount, phase_accumulator1, sine_freq1);
								phase_accumulator1 = CosineWave(out_r_ptr2, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(out_l_ptr2, out_l_ptr2, frameCount, (float)source_scale);
								ScaleBuffer(out_r_ptr2, out_r_ptr2, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_TWO_TONE:
								double dump;
								SineWave2Tone(out_l_ptr2, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out dump, out dump);
								CosineWave2Tone(out_r_ptr2, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
								ScaleBuffer(out_l_ptr2, out_l_ptr2, frameCount, (float)source_scale);
								ScaleBuffer(out_r_ptr2, out_r_ptr2, frameCount, (float)source_scale);
								break;
							case SignalSource.NOISE:
								Noise(out_l_ptr2, frameCount);
								Noise(out_r_ptr2, frameCount);
								break;
							case SignalSource.TRIANGLE:
								Triangle(out_l_ptr2, frameCount, sine_freq1);
								CopyBuffer(out_l_ptr2, out_r_ptr2, frameCount);
								break;
							case SignalSource.SAWTOOTH:
								Sawtooth(out_l_ptr2, frameCount, sine_freq1);
								CopyBuffer(out_l_ptr2, out_r_ptr2, frameCount);
								break;
							case SignalSource.SILENCE:
								ClearBuffer(out_l_ptr2, frameCount);
								ClearBuffer(out_r_ptr2, frameCount);
								break;
						}
                        
					}

					#endregion

					break;
				case AudioState.CW:
					DttSP.CWtoneExchange(out_l_ptr2, out_r_ptr2, frameCount);
                    
					break;
			}

			if(!localmox) DoScope(out_l_ptr1, frameCount);
			else DoScope(out_l_ptr2, frameCount);

			out_l1 = out_l_ptr1;
			out_r1 = out_r_ptr1;
			out_l2 = out_l_ptr2;
			out_r2 = out_r_ptr2;
            //ctcss_l = out_l_ptr1;
            //ctcss_r = out_r_ptr1;

			if(wave_record)
			{
				if(!localmox)
				{
					if(!record_rx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);
					}
				}
				else
				{
					if(!record_tx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(out_l_ptr2, out_r_ptr2);
					}
				}
			}

			// scale output for VAC
			if(vac_enabled &&
				rb_vacIN_l != null && rb_vacIN_r != null && 
				rb_vacOUT_l != null && rb_vacOUT_r != null)
			{
				if(!localmox)
				{
					ScaleBuffer(out_l1, out_l2, frameCount, (float)vac_rx_scale);
					ScaleBuffer(out_r1, out_r2, frameCount, (float)vac_rx_scale);
				}
				else if(mon)
				{
					ScaleBuffer(out_l2, out_l1, frameCount, (float)vac_rx_scale);
					ScaleBuffer(out_r2, out_r1, frameCount, (float)vac_rx_scale);
				}
				else // zero samples going back to VAC since TX monitor is off
				{
					ScaleBuffer(out_l2, out_l1, frameCount, 0.0f);
					ScaleBuffer(out_r2, out_r1, frameCount, 0.0f);
				}

				float* vac_l, vac_r;
				if(!localmox)
				{
					vac_l = out_l2;
					vac_r = out_r2;
				}
				else
				{
					vac_l = out_l1;
					vac_r = out_r1;
				}

				if (sample_rate2 == sample_rate1) 
				{
					if ((rb_vacOUT_l.WriteSpace()>=frameCount)&&(rb_vacOUT_r.WriteSpace()>=frameCount))
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacOUT_l.WritePtr(vac_l, frameCount);
						rb_vacOUT_r.WritePtr(vac_r, frameCount);
						Win32.LeaveCriticalSection(cs_vac);
					}
					else 
					{
						VACDebug("rb_vacOUT_l overflow ");
						vac_rb_reset = true;
					}
				} 
				else 
				{
					if (vac_stereo)
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))
							fixed(float *res_outr_ptr = &(res_outr[0])) 
							{
								int outsamps;
								DttSP.DoResamplerF(vac_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
								DttSP.DoResamplerF(vac_r, res_outr_ptr, frameCount, &outsamps, resampPtrOut_r);
								//Debug.WriteLine("Outsamps: "+outsamps.ToString());
								if ((rb_vacOUT_l.WriteSpace()>=outsamps)&&(rb_vacOUT_r.WriteSpace()>=outsamps))
								{
									Win32.EnterCriticalSection(cs_vac);
									rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
									rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
									Win32.LeaveCriticalSection(cs_vac);
								}
								else
								{
									vac_rb_reset = true;
									VACDebug("rb_vacOUT_l overflow");
								}
							}
					}
					else 
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))
						{
							int outsamps;
							DttSP.DoResamplerF(vac_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
							//Debug.WriteLine("Framecount: "+frameCount.ToString() + " Outsamps: "+outsamps.ToString());
							if ((rb_vacOUT_l.WriteSpace() >= outsamps)&&(rb_vacOUT_r.WriteSpace() >= outsamps))
							{
								Win32.EnterCriticalSection(cs_vac);
								rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
								rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
								Win32.LeaveCriticalSection(cs_vac);
							}
							else
							{
								vac_rb_reset = true;
								VACDebug("rb_vacOUT_l overflow");
							}
						}
					}
				}
			}

            
			// Scale output to SDR-1000
			if(!localmox)
			{
				ScaleBuffer(out_l1, out_l1, frameCount, (float)monitor_volume);
				ScaleBuffer(out_r1, out_r1, frameCount, (float)monitor_volume);
				//ClearBuffer(out_l2, frameCount);
				//ClearBuffer(out_r2, frameCount);
			}
			else
			{
				double tx_vol = TXScale;
				if(tx_output_signal != SignalSource.RADIO)
					tx_vol = 1.0;
				if(high_pwr_am)
				{
					if(tx_dsp_mode == DSPMode.AM ||
						tx_dsp_mode == DSPMode.SAM)
						tx_vol *= 1.414;
				}

				
//				              if (!testing && tx_dsp_mode == DSPMode.FMN && console.PLTone == true)
 //               {
 //                   SineWave(ctcss_l, frameCount, phase_accumulator1, 254.1);
   //                 phase_accumulator1 = CosineWave(ctcss_r, frameCount, phase_accumulator1, 254.1);
//
 //                   MixBuffer(ctcss_l, out_l2, out_l1, frameCount, 0.2f);//(float)monitor_volume);
  //                  MixBuffer(ctcss_l, out_l2, out_l2, frameCount, (float)tx_vol);

//                    MixBuffer(ctcss_r, out_r2, out_r1, frameCount, 0.2f);//(float)monitor_volume);
 //                   MixBuffer(ctcss_r, out_r2, out_r2, frameCount, (float)tx_vol);
  //              }

//                else 
 // 

                /* hermes hack needed */
                ScaleBuffer(out_l2, out_l1, frameCount, (float)monitor_volume);
                if (console.CurrentModel != Model.HERMES && !console.PennyLanePresent ) /* Hermes power level set by command and control to programmable gain amp .. no need to do digital scaling  for power */
                {
                    ScaleBuffer(out_l2, out_l2, frameCount,(float)tx_vol);
                }
                ScaleBuffer(out_r2, out_r1, frameCount, (float)monitor_volume);
                if (console.CurrentModel != Model.HERMES && !console.PennyLanePresent ) /* Hermes power level set by command and control to programmable gain amp .. no need to do digital scaling  for power */
                {
                    ScaleBuffer(out_r2, out_r2, frameCount, (float)tx_vol);
                }	
				
				/*ScaleBuffer(out_l2, out_l1, frameCount, (float)monitor_volume);
				ScaleBuffer(out_l2, out_l2, frameCount, (float)tx_vol);
				ScaleBuffer(out_r2, out_r1, frameCount, (float)monitor_volume);
				ScaleBuffer(out_r2, out_r2, frameCount, (float)tx_vol);*/
			}

 //           float sum = SumBuffer(out_l2, frameCount);
           
//            if (sum == 0.0f)
//                console.buffiszero = true;
           

			/*Debug.WriteLine("Max 1L: "+MaxSample(out_l1, frameCount).ToString("f5")+" 1R: "+MaxSample(out_r1, frameCount).ToString("f5")+
				" 2L: "+MaxSample(out_l2, frameCount).ToString("f5")+" 2R: "+MaxSample(out_r2, frameCount).ToString("f5"));*/

            if (!testing && soundcard != SoundCard.DELTA_44 || console.CurrentModel != Model.HPSDR || console.CurrentModel != Model.HERMES)
			{
				// clip radio output to prevent overdrive
				float clip_thresh = (float)(1.5f / audio_volts1);
				for(int i=0; i<frameCount; i++)
				{
										if(out_l2[i] > clip_thresh)
										{
											//Debug.WriteLine("Clip Left High: "+out_l2[i].ToString("f5"));
											out_l2[i] = clip_thresh;
										}
										else if(out_l2[i] < -clip_thresh)
										{
											//Debug.WriteLine("Clip Left Low: "+out_l2[i].ToString("f5"));
											out_l2[i] = -clip_thresh;
										}

										if(out_r2[i] > clip_thresh) 
										{
											//Debug.WriteLine("Clip Right High: "+out_l2[i].ToString("f5"));
											out_r2[i] = clip_thresh;							
										}
										else if(out_r2[i] < -clip_thresh)
										{
											//Debug.WriteLine("Clip Right Low: "+out_l2[i].ToString("f5"));
											out_r2[i] = -clip_thresh;
										} 

//					Branchless clipping -- testing found this was more costly overall and especially when 
//					  when dealing with samples that mostly do not need clipping

//					float x1 = Math.Abs(out_l2[i]+clip_thresh);
//					float x2 = Math.Abs(out_l2[i]-clip_thresh);
//					x1 -= x2;
//					out_l2[i] = x1 * 0.5f;
//					x1 = Math.Abs(out_r2[i]+clip_thresh);
//					x2 = Math.Abs(out_r2[i]-clip_thresh);
//					x1 -= x2;
//					out_r2[i] = x1 * 0.5f;					
				}

				if(audio_volts1 > 1.5f)
				{
					// scale FireBox monitor output to prevent overdrive
					ScaleBuffer(out_l1, out_l1, frameCount, (float)(1.5f / audio_volts1));
					ScaleBuffer(out_r1, out_r1, frameCount, (float)(1.5f / audio_volts1));
				}

 			}
            if (!localmox)
            {
               ClearBuffer(out_l2, frameCount);
               ClearBuffer(out_r2, frameCount);
               console.buffiszero = true;
            }


#if(MINMAX)
			Debug.Write(MaxSample(out_l2, out_r2, frameCount).ToString("f6")+",");

			float current_max = MaxSample(out_l2, out_r2, frameCount);
			if(current_max > max) max = current_max;
			Debug.WriteLine(" max: "+max.ToString("f6"));
#endif

#if(TIMER)
			t1.Stop();
			Debug.WriteLine(t1.Duration);
#endif
                    
			return callback_return;
		}

#if(MINMAX)
		private static float max = float.MinValue;
#endif
        //private static HiPerfTimer t2 = new HiPerfTimer();
        //private static double period = 0.0;

		unsafe public static int Callback2(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
#if(TIMER)
			t1.Start();
#endif
            //t2.Start();
			float* in_l = null, in_r = null;
			float* out_l1 = null, out_r1 = null, out_l2 = null, out_r2 = null;
			float* out_l3 = null, out_r3 = null, out_l4 = null, out_r4 = null;
			float* rx1_in_l = null, rx1_in_r = null, tx_in_l = null, tx_in_r = null;
			float* rx2_in_l = null, rx2_in_r = null;
			float* rx1_out_l = null, rx1_out_r = null, tx_out_l = null, tx_out_r = null;
			float* rx2_out_l = null, rx2_out_r = null;
			localmox = mox;

			void* ex_input  = (int *)input;
			void* ex_output = (int *)output;
			int* array_ptr_input = (int *)input;
			float* in_l_ptr1 = (float *)array_ptr_input[0];
			float* in_r_ptr1 = (float *)array_ptr_input[1];
			float* in_l_ptr2 = (float *)array_ptr_input[2];
			float* in_r_ptr2 = (float *)array_ptr_input[3];
			float* in_l_ptr3 = (float *)array_ptr_input[4];
			float* in_r_ptr3 = (float *)array_ptr_input[5];
			float* in_l_ptr4 = (float *)array_ptr_input[6];
			float* in_r_ptr4 = (float *)array_ptr_input[7];
			int* array_ptr_output = (int *)output;
			float* out_l_ptr1 = (float *)array_ptr_output[0];
			float* out_r_ptr1 = (float *)array_ptr_output[1];
			float* out_l_ptr2 = (float *)array_ptr_output[2];
			float* out_r_ptr2 = (float *)array_ptr_output[3];
			float* out_l_ptr3 = (float *)array_ptr_output[4];
			float* out_r_ptr3 = (float *)array_ptr_output[5];
			float* out_l_ptr4 = (float *)array_ptr_output[6];
			float* out_r_ptr4 = (float *)array_ptr_output[7];
			
			// arrange input buffers in the following order:
			// RX1 Left, RX1 Right, TX Left, TX Right, RX2 Left, RX2 Right
			//int* array_ptr = (int *)input;
			switch(in_rx1_l)
			{
				case 0: array_ptr_input[0] = (int)in_l_ptr1; break;
				case 1: array_ptr_input[0] = (int)in_r_ptr1; break;
				case 2: array_ptr_input[0] = (int)in_l_ptr2; break;
				case 3: array_ptr_input[0] = (int)in_r_ptr2; break;
				case 4: array_ptr_input[0] = (int)in_l_ptr3; break;
				case 5: array_ptr_input[0] = (int)in_r_ptr3; break;
				case 6: array_ptr_input[0] = (int)in_l_ptr4; break;
				case 7: array_ptr_input[0] = (int)in_r_ptr4; break;
			}

			switch(in_rx1_r)
			{
				case 0: array_ptr_input[1] = (int)in_l_ptr1; break;
				case 1: array_ptr_input[1] = (int)in_r_ptr1; break;
				case 2: array_ptr_input[1] = (int)in_l_ptr2; break;
				case 3: array_ptr_input[1] = (int)in_r_ptr2; break;
				case 4: array_ptr_input[1] = (int)in_l_ptr3; break;
				case 5: array_ptr_input[1] = (int)in_r_ptr3; break;
				case 6: array_ptr_input[1] = (int)in_l_ptr4; break;
				case 7: array_ptr_input[1] = (int)in_r_ptr4; break;
			}
			
			switch(in_tx_l)
			{
				case 0: array_ptr_input[2] = (int)in_l_ptr1; break;
				case 1: array_ptr_input[2] = (int)in_r_ptr1; break;
				case 2: array_ptr_input[2] = (int)in_l_ptr2; break;
				case 3: array_ptr_input[2] = (int)in_r_ptr2; break;
				case 4: array_ptr_input[2] = (int)in_l_ptr3; break;
				case 5: array_ptr_input[2] = (int)in_r_ptr3; break;
				case 6: array_ptr_input[2] = (int)in_l_ptr4; break;
				case 7: array_ptr_input[2] = (int)in_r_ptr4; break;
			}

			switch(in_tx_r)
			{
				case 0: array_ptr_input[3] = (int)in_l_ptr1; break;
				case 1: array_ptr_input[3] = (int)in_r_ptr1; break;
				case 2: array_ptr_input[3] = (int)in_l_ptr2; break;
				case 3: array_ptr_input[3] = (int)in_r_ptr2; break;
				case 4: array_ptr_input[3] = (int)in_l_ptr3; break;
				case 5: array_ptr_input[3] = (int)in_r_ptr3; break;
				case 6: array_ptr_input[3] = (int)in_l_ptr4; break;
				case 7: array_ptr_input[3] = (int)in_r_ptr4; break;
			}
			
			switch(in_rx2_l)
			{
				case 0: array_ptr_input[4] = (int)in_l_ptr1; break;
				case 1: array_ptr_input[4] = (int)in_r_ptr1; break;
				case 2: array_ptr_input[4] = (int)in_l_ptr2; break;
				case 3: array_ptr_input[4] = (int)in_r_ptr2; break;
				case 4: array_ptr_input[4] = (int)in_l_ptr3; break;
				case 5: array_ptr_input[4] = (int)in_r_ptr3; break;
				case 6: array_ptr_input[4] = (int)in_l_ptr4; break;
				case 7: array_ptr_input[4] = (int)in_r_ptr4; break;
			}
			switch(in_rx2_r)
			{
				case 0: array_ptr_input[5] = (int)in_l_ptr1; break;
				case 1: array_ptr_input[5] = (int)in_r_ptr1; break;
				case 2: array_ptr_input[5] = (int)in_l_ptr2; break;
				case 3: array_ptr_input[5] = (int)in_r_ptr2; break;
				case 4: array_ptr_input[5] = (int)in_l_ptr3; break;
				case 5: array_ptr_input[5] = (int)in_r_ptr3; break;
				case 6: array_ptr_input[5] = (int)in_l_ptr4; break;
				case 7: array_ptr_input[5] = (int)in_r_ptr4; break;
			}

			rx1_in_l = (float *)array_ptr_input[0];
			rx1_in_r = (float *)array_ptr_input[1];
			tx_in_l = (float *)array_ptr_input[2];
			tx_in_r = (float *)array_ptr_input[3];
			rx2_in_l = (float *)array_ptr_input[4];
			rx2_in_r = (float *)array_ptr_input[5];

			rx1_out_l = (float *)array_ptr_output[0];
			rx1_out_r = (float *)array_ptr_output[1];
			tx_out_l = (float *)array_ptr_output[2];
			tx_out_r = (float *)array_ptr_output[3];
			rx2_out_l = (float *)array_ptr_output[4];
			rx2_out_r = (float *)array_ptr_output[5];

			if(!localmox)
			{
				in_l = rx1_in_l;
				in_r = rx1_in_r;
			}
			else
			{
				in_l = tx_in_l;
				in_r = tx_in_r;
			}

            if (test_mute)
            {
                ClearBuffer(rx1_in_l, frameCount);
                ClearBuffer(rx1_in_r, frameCount);
            }

            float sum = SumBuffer(rx1_in_l, frameCount);
            if (sum == 0.0f)
                empty_buffers++;
            else empty_buffers = 0;

#if true // EHR RX2 QSK
            if (localmox && rx2_enabled && rx2_auto_mute_tx)
            {
                ClearBuffer(rx2_in_l, frameCount);
                ClearBuffer(rx2_in_r, frameCount);
            }
#endif

			if(wave_playback)
			{
				wave_file_reader.GetPlayBuffer(in_l, in_r);
				if(rx2_enabled && !localmox)
				{
					CopyBuffer(in_l, rx2_in_l, frameCount);
					CopyBuffer(in_r, rx2_in_r, frameCount);
				}
			}
			
            if(wave_record)
			{
				if(!localmox)
				{
					if(record_rx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(in_l, in_r);
					}
				}
				else
				{
					if(record_tx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(in_l, in_r);
					}
				}
			}

			if(phase)
			{
				//phase_mutex.WaitOne();
				Marshal.Copy(new IntPtr(in_l), phase_buf_l, 0, frameCount);
				Marshal.Copy(new IntPtr(in_r), phase_buf_r, 0, frameCount);
				//phase_mutex.ReleaseMutex();
			}

			// handle VAC Input
			if(vac_enabled && 
				rb_vacIN_l != null && rb_vacIN_r != null && 
				rb_vacOUT_l != null && rb_vacOUT_r != null)
			{
				if(vac_bypass || !localmox) // drain VAC Input ring buffer
				{
					if ((rb_vacIN_l.ReadSpace() >= frameCount)&&(rb_vacIN_r.ReadSpace() >= frameCount))
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacIN_l.ReadPtr(out_l_ptr2, frameCount);
						rb_vacIN_r.ReadPtr(out_r_ptr2, frameCount);
						Win32.LeaveCriticalSection(cs_vac);
					}
				}
				else
				{
					if(rb_vacIN_l.ReadSpace() >= frameCount) 
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacIN_l.ReadPtr(tx_in_l, frameCount);
						rb_vacIN_r.ReadPtr(tx_in_r, frameCount);
						Win32.LeaveCriticalSection(cs_vac);
						if(vac_combine_input)
							AddBuffer(tx_in_l, tx_in_r, frameCount);
					}
					else
					{
						ClearBuffer(tx_in_l, frameCount);
						ClearBuffer(tx_in_r, frameCount);
						VACDebug("rb_vacIN underflow 4inTX");
					}
					ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)vac_preamp);
					ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)vac_preamp);
				}
			}

			switch(current_audio_state1)
			{
				case AudioState.DTTSP:
					
					#region VOX
					if(vox_enabled)
					{
						if(tx_dsp_mode == DSPMode.LSB ||
							tx_dsp_mode == DSPMode.USB ||
							tx_dsp_mode == DSPMode.DSB ||
							tx_dsp_mode == DSPMode.AM  ||
							tx_dsp_mode == DSPMode.SAM ||
                            tx_dsp_mode == DSPMode.FMN ||
                            tx_dsp_mode == DSPMode.DIGL ||
                            tx_dsp_mode == DSPMode.DIGU)
						{
							peak = MaxSample(tx_in_l, tx_in_r, frameCount);

							// compare power to threshold
							if(peak > vox_threshold) 
								vox_active = true;
							else 
								vox_active = false;
						}
					}
					#endregion

					// scale input with mic preamp
					if((!vac_enabled &&
						(tx_dsp_mode == DSPMode.LSB ||
						tx_dsp_mode == DSPMode.USB ||
						tx_dsp_mode == DSPMode.DSB ||
						tx_dsp_mode == DSPMode.AM  ||
						tx_dsp_mode == DSPMode.SAM ||
						tx_dsp_mode == DSPMode.FMN ||
						tx_dsp_mode == DSPMode.DIGL ||
						tx_dsp_mode == DSPMode.DIGU)) ||
						(vac_enabled && vac_bypass &&
						(tx_dsp_mode == DSPMode.DIGL ||
						tx_dsp_mode == DSPMode.DIGU ||
						tx_dsp_mode == DSPMode.LSB ||
						tx_dsp_mode == DSPMode.USB ||
						tx_dsp_mode == DSPMode.DSB ||
						tx_dsp_mode == DSPMode.AM ||
						tx_dsp_mode == DSPMode.SAM ||
						tx_dsp_mode == DSPMode.FMN)))
					{
						if(wave_playback)
						{
							ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)wave_preamp);
							ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)wave_preamp);
						}
						else
						{
							if(!vac_enabled && (tx_dsp_mode == DSPMode.DIGL || tx_dsp_mode == DSPMode.DIGU))
							{
								ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)vac_preamp);
								ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)vac_preamp);
							}
							else
							{
								ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)mic_preamp);
								ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)mic_preamp);
							}
						}
					}

					#region Input Signal Source

					switch(rx1_input_signal)
					{
						case SignalSource.RADIO:
							break;
						case SignalSource.SINE:
							SineWave(rx1_in_l, frameCount, phase_accumulator1, sine_freq1);
							phase_accumulator1 = CosineWave(rx1_in_r, frameCount, phase_accumulator1, sine_freq1);
							ScaleBuffer(rx1_in_l, rx1_in_l, frameCount, (float)source_scale);
							ScaleBuffer(rx1_in_r, rx1_in_r, frameCount, (float)source_scale);
							break;
						case SignalSource.SINE_TWO_TONE:
							double dump;
							SineWave2Tone(rx1_in_l, frameCount, phase_accumulator1, phase_accumulator2,
								sine_freq1, sine_freq2, out dump, out dump);
							CosineWave2Tone(rx1_in_r, frameCount, phase_accumulator1, phase_accumulator2,
								sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
							ScaleBuffer(rx1_in_l, rx1_in_l, frameCount, (float)source_scale);
							ScaleBuffer(rx1_in_r, rx1_in_r, frameCount, (float)source_scale);
							break;
						case SignalSource.SINE_LEFT_ONLY:
							phase_accumulator1 = SineWave(rx1_in_l, frameCount, phase_accumulator1, sine_freq1);
							ScaleBuffer(rx1_in_l, rx1_in_l, frameCount, (float)source_scale);
							ClearBuffer(rx1_in_r, frameCount);
							break;
						case SignalSource.SINE_RIGHT_ONLY:
							phase_accumulator1 = SineWave(rx1_in_r, frameCount, phase_accumulator1, sine_freq1);
							ScaleBuffer(rx1_in_r, rx1_in_r, frameCount, (float)source_scale);
							ClearBuffer(rx1_in_l, frameCount);
							break;
						case SignalSource.NOISE:
							Noise(rx1_in_l, frameCount);
							Noise(rx1_in_r, frameCount);
							break;
						case SignalSource.TRIANGLE:
							Triangle(rx1_in_l, frameCount, sine_freq1);
							CopyBuffer(rx1_in_l, rx1_in_r, frameCount);
							break;
						case SignalSource.SAWTOOTH:
							Sawtooth(rx1_in_l, frameCount, sine_freq1);
							CopyBuffer(rx1_in_l, rx1_in_r, frameCount);
							break;
						case SignalSource.SILENCE:
							ClearBuffer(rx1_in_l, frameCount);
							ClearBuffer(rx1_in_r, frameCount);
							break;
					}

					if(rx2_enabled)
					{
						switch(rx2_input_signal)
						{
							case SignalSource.RADIO:
								break;
							case SignalSource.SINE:
								SineWave(rx2_in_l, frameCount, phase_accumulator1, sine_freq1);
								phase_accumulator1 = CosineWave(rx2_in_r, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(rx2_in_l, rx2_in_l, frameCount, (float)source_scale);
								ScaleBuffer(rx2_in_r, rx2_in_r, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_TWO_TONE:
								double dump;
								SineWave2Tone(rx2_in_l, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out dump, out dump);
								CosineWave2Tone(rx2_in_r, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
								ScaleBuffer(rx2_in_l, rx2_in_l, frameCount, (float)source_scale);
								ScaleBuffer(rx2_in_r, rx2_in_r, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_LEFT_ONLY:
								phase_accumulator1 = SineWave(rx2_in_l, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(rx2_in_l, rx2_in_l, frameCount, (float)source_scale);
								ClearBuffer(rx2_in_r, frameCount);
								break;
							case SignalSource.SINE_RIGHT_ONLY:
								phase_accumulator1 = SineWave(rx2_in_r, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(rx2_in_r, rx2_in_r, frameCount, (float)source_scale);
								ClearBuffer(rx2_in_l, frameCount);
								break;
							case SignalSource.NOISE:
								Noise(rx2_in_l, frameCount);
								Noise(rx2_in_r, frameCount);
								break;
							case SignalSource.TRIANGLE:
								Triangle(rx2_in_l, frameCount, sine_freq1);
								CopyBuffer(rx2_in_l, rx2_in_r, frameCount);
								break;
							case SignalSource.SAWTOOTH:
								Sawtooth(rx2_in_l, frameCount, sine_freq1);
								CopyBuffer(rx2_in_l, rx2_in_r, frameCount);
								break;
							case SignalSource.SILENCE:
								ClearBuffer(rx2_in_l, frameCount);
								ClearBuffer(rx2_in_r, frameCount);
								break;
						}
					}

					switch(tx_input_signal)
					{
						case SignalSource.RADIO:
							break;
						case SignalSource.SINE:
							SineWave(tx_in_l, frameCount, phase_accumulator1, sine_freq1);
							phase_accumulator1 = CosineWave(tx_in_r, frameCount, phase_accumulator1, sine_freq1);
							ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)source_scale);
							ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)source_scale);
							break;
						case SignalSource.SINE_TWO_TONE:
							double dump;
							SineWave2Tone(tx_in_l, frameCount, phase_accumulator1, phase_accumulator2,
								sine_freq1, sine_freq2, out dump, out dump);
							CosineWave2Tone(tx_in_r, frameCount, phase_accumulator1, phase_accumulator2,
								sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
							ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)source_scale);
							ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)source_scale);
							break;
						case SignalSource.NOISE:
							Noise(tx_in_l, frameCount);
							Noise(tx_in_r, frameCount);
							break;
						case SignalSource.TRIANGLE:
							Triangle(tx_in_l, frameCount, sine_freq1);
							CopyBuffer(tx_in_l, tx_in_r, frameCount);
							break;
						case SignalSource.SAWTOOTH:
							Sawtooth(tx_in_l, frameCount, sine_freq1);
							CopyBuffer(tx_in_l, tx_in_r, frameCount);
							break;
						case SignalSource.SILENCE:
							ClearBuffer(tx_in_l, frameCount);
							ClearBuffer(tx_in_r, frameCount);
							break;
					}

					#endregion
	
#if(MINMAX)
					/*float local_max = MaxSample(in_l, in_r, frameCount);
					if(local_max > max)
					{
						max = local_max;
						Debug.WriteLine("max in: "+max.ToString("f6"));
					}*/

					Debug.Write(MaxSample(in_l, in_r, frameCount).ToString("f6")+",");
#endif

					if (vac_enabled && vac_output_iq &&
						rb_vacOUT_l != null && rb_vacOUT_r != null &&
						rx1_in_l != null && rx1_in_r != null)
					{
						if ((rb_vacOUT_l.WriteSpace()>=frameCount)&&(rb_vacOUT_r.WriteSpace()>=frameCount))
						{
							if (vac_correct_iq)
								fixed(float *res_outl_ptr = &(res_outl[0]))
									fixed(float *res_outr_ptr = &(res_outr[0]))
									{
                                        if (vac_output_rx2)
                                            CorrectIQBuffer(rx2_in_l, rx2_in_r, res_outl_ptr, res_outr_ptr, frameCount);
                                        else
                                            CorrectIQBuffer(rx1_in_l, rx1_in_r, res_outl_ptr, res_outr_ptr, frameCount);

										Win32.EnterCriticalSection(cs_vac);
										rb_vacOUT_l.WritePtr(res_outr_ptr, frameCount);
										rb_vacOUT_r.WritePtr(res_outl_ptr, frameCount);
										Win32.LeaveCriticalSection(cs_vac);

									}
							else
							{
								Win32.EnterCriticalSection(cs_vac);
                                if (vac_output_rx2)
                                {
                                    rb_vacOUT_l.WritePtr(rx2_in_r, frameCount);
                                    rb_vacOUT_r.WritePtr(rx2_in_l, frameCount);
                                }
                                else
                                {
                                    rb_vacOUT_l.WritePtr(rx1_in_r, frameCount);
                                    rb_vacOUT_r.WritePtr(rx1_in_l, frameCount);
                                }
								Win32.LeaveCriticalSection(cs_vac);
							}
						}
						else 
						{
							VACDebug("rb_vacOUT_l I/Q overflow ");
							vac_rb_reset = true;
						}
					}

					DttSP.ExchangeSamples2(ex_input, ex_output, frameCount);

					if (tx_dsp_mode == DSPMode.CWU || tx_dsp_mode == DSPMode.CWL)
					{
                        //t2.Stop();
                        //period = t2.DurationMsec;
						DttSP.CWtoneExchange(out_l_ptr2, out_r_ptr2, frameCount);
                        //t2.Start();
					} 

#if(MINMAX)
					Debug.Write(MaxSample(out_l_ptr2, frameCount).ToString("f6")+",");
					Debug.Write(MaxSample(out_r_ptr2, frameCount).ToString("f6")+"\n");
#endif

					#region Output Signal Source

					switch(rx1_output_signal)
					{
						case SignalSource.RADIO:
							break;
						case SignalSource.SINE:
							SineWave(rx1_out_l, frameCount, phase_accumulator1, sine_freq1);
							phase_accumulator1 = CosineWave(rx1_out_r, frameCount, phase_accumulator1, sine_freq1);
							ScaleBuffer(rx1_out_l, rx1_out_l, frameCount, (float)source_scale);
							ScaleBuffer(rx1_out_r, rx1_out_r, frameCount, (float)source_scale);
							break;
						case SignalSource.SINE_TWO_TONE:
							double dump;
							SineWave2Tone(rx1_out_l, frameCount, phase_accumulator1, phase_accumulator2,
								sine_freq1, sine_freq2, out dump, out dump);
							CosineWave2Tone(rx1_out_r, frameCount, phase_accumulator1, phase_accumulator2,
								sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
							ScaleBuffer(rx1_out_l, rx1_out_l, frameCount, (float)source_scale);
							ScaleBuffer(rx1_out_r, rx1_out_r, frameCount, (float)source_scale);
							break;
						case SignalSource.SINE_LEFT_ONLY:
							phase_accumulator1 = SineWave(rx1_out_l, frameCount, phase_accumulator1, sine_freq1);
							ScaleBuffer(rx1_out_l, rx1_out_l, frameCount, (float)source_scale);
							ClearBuffer(rx1_out_r, frameCount);
							break;
						case SignalSource.SINE_RIGHT_ONLY:
							phase_accumulator1 = SineWave(rx1_out_r, frameCount, phase_accumulator1, sine_freq1);
							ScaleBuffer(rx1_out_r, rx1_out_r, frameCount, (float)source_scale);
							ClearBuffer(rx1_out_l, frameCount);
							break;
						case SignalSource.NOISE:
							Noise(rx1_out_l, frameCount);
							Noise(rx1_out_r, frameCount);
							break;
						case SignalSource.TRIANGLE:
							Triangle(rx1_out_l, frameCount, sine_freq1);
							CopyBuffer(rx1_out_l, rx1_out_r, frameCount);
							break;
						case SignalSource.SAWTOOTH:
							Sawtooth(rx1_out_l, frameCount, sine_freq1);
							CopyBuffer(rx1_out_l, rx1_out_r, frameCount);
							break;
						case SignalSource.SILENCE:
							ClearBuffer(rx1_out_l, frameCount);
							ClearBuffer(rx1_out_r, frameCount);
							break;
					}

					if(rx2_enabled)
					{
						switch(rx2_output_signal)
						{
							case SignalSource.RADIO:
								break;
							case SignalSource.SINE:
								SineWave(rx2_out_l, frameCount, phase_accumulator1, sine_freq1);
								phase_accumulator1 = CosineWave(rx2_out_r, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(rx2_out_l, rx2_out_l, frameCount, (float)source_scale);
								ScaleBuffer(rx2_out_r, rx2_out_r, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_TWO_TONE:
								double dump;
								SineWave2Tone(rx2_out_l, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out dump, out dump);
								CosineWave2Tone(rx2_out_r, frameCount, phase_accumulator1, phase_accumulator2,
									sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
								ScaleBuffer(rx2_out_l, rx2_out_l, frameCount, (float)source_scale);
								ScaleBuffer(rx2_out_r, rx2_out_r, frameCount, (float)source_scale);
								break;
							case SignalSource.SINE_LEFT_ONLY:
								phase_accumulator1 = SineWave(rx2_out_l, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(rx2_out_l, rx2_out_l, frameCount, (float)source_scale);
								ClearBuffer(rx2_out_r, frameCount);
								break;
							case SignalSource.SINE_RIGHT_ONLY:
								phase_accumulator1 = SineWave(rx2_out_r, frameCount, phase_accumulator1, sine_freq1);
								ScaleBuffer(rx2_out_r, rx2_out_r, frameCount, (float)source_scale);
								ClearBuffer(rx2_out_l, frameCount);
								break;
							case SignalSource.NOISE:
								Noise(rx2_out_l, frameCount);
								Noise(rx2_out_r, frameCount);
								break;
							case SignalSource.TRIANGLE:
								Triangle(rx2_out_l, frameCount, sine_freq1);
								CopyBuffer(rx2_out_l, rx2_out_r, frameCount);
								break;
							case SignalSource.SAWTOOTH:
								Sawtooth(rx2_out_l, frameCount, sine_freq1);
								CopyBuffer(rx2_out_l, rx2_out_r, frameCount);
								break;
							case SignalSource.SILENCE:
								ClearBuffer(rx2_out_l, frameCount);
								ClearBuffer(rx2_out_r, frameCount);
								break;
						}
					}

					switch(tx_output_signal)
					{
						case SignalSource.RADIO:
							break;
						case SignalSource.SINE:
							SineWave(tx_out_l, frameCount, phase_accumulator1, sine_freq1);
							phase_accumulator1 = CosineWave(tx_out_r, frameCount, phase_accumulator1, sine_freq1);
							ScaleBuffer(tx_out_l, tx_out_l, frameCount, (float)source_scale);
							ScaleBuffer(tx_out_r, tx_out_r, frameCount, (float)source_scale);
							break;
						case SignalSource.SINE_TWO_TONE:
							double dump;
							SineWave2Tone(tx_out_l, frameCount, phase_accumulator1, phase_accumulator2,
								sine_freq1, sine_freq2, out dump, out dump);
							CosineWave2Tone(tx_out_r, frameCount, phase_accumulator1, phase_accumulator2,
								sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
							ScaleBuffer(tx_out_l, tx_out_l, frameCount, (float)source_scale);
							ScaleBuffer(tx_out_r, tx_out_r, frameCount, (float)source_scale);
							break;
						case SignalSource.NOISE:
							Noise(tx_out_l, frameCount);
							Noise(tx_out_r, frameCount);
							break;
						case SignalSource.TRIANGLE:
							Triangle(tx_out_l, frameCount, sine_freq1);
							CopyBuffer(tx_out_l, tx_out_r, frameCount);
							break;
						case SignalSource.SAWTOOTH:
							Sawtooth(tx_out_l, frameCount, sine_freq1);
							CopyBuffer(tx_out_l, tx_out_r, frameCount);
							break;
						case SignalSource.SILENCE:
							ClearBuffer(tx_out_l, frameCount);
							ClearBuffer(tx_out_r, frameCount);
							break;
					}

					#endregion

					break;
				case AudioState.CW: 
					//DttSP.ExchangeSamples2(array_ptr_input, array_ptr_output, frameCount);
					DttSP.ExchangeSamples2(ex_input, ex_output, frameCount);

                    //t2.Stop();
                    //period = t2.DurationMsec;
					DttSP.CWtoneExchange(out_l_ptr2, out_r_ptr2, frameCount);
                    //t2.Start();
					break;
			}

			if(!localmox) DoScope(out_l_ptr1, frameCount);
			else DoScope(out_l_ptr2, frameCount);

			if(wave_record)
			{
				if(!localmox)
				{
					if(!record_rx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);
					}
				}
				else
				{
					if(!record_tx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(out_l_ptr2, out_r_ptr2);
					}
				}
			}

			out_l1 = rx1_out_l;
			out_r1 = rx1_out_r;
			out_l2 = out_l_ptr2;
			out_r2 = out_r_ptr2;
			out_l3 = out_l_ptr3;
			out_r3 = out_r_ptr3;
			out_l4 = out_l_ptr4;
			out_r4 = out_r_ptr4;

			// scale output for VAC -- use chan 4 as spare buffer
			if(vac_enabled && !vac_output_iq && 
				rb_vacIN_l != null && rb_vacIN_r != null && 
				rb_vacOUT_l != null && rb_vacOUT_r != null)
			{
				if(!localmox)
				{
					ScaleBuffer(out_l1, out_l4, frameCount, (float)vac_rx_scale);
					ScaleBuffer(out_r1, out_r4, frameCount, (float)vac_rx_scale);
				}
				else if(mon)
				{
					ScaleBuffer(out_l2, out_l4, frameCount, (float)vac_rx_scale);
					ScaleBuffer(out_r2, out_r4, frameCount, (float)vac_rx_scale);
				}
				else // zero samples going back to VAC since TX monitor is off
				{
					ScaleBuffer(out_l2, out_l4, frameCount, 0.0f);
					ScaleBuffer(out_r2, out_r4, frameCount, 0.0f);
				}

				if (sample_rate2 == sample_rate1) 
				{
					if ((rb_vacOUT_l.WriteSpace()>=frameCount)&&(rb_vacOUT_r.WriteSpace()>=frameCount))
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacOUT_l.WritePtr(out_l4, frameCount);
						rb_vacOUT_r.WritePtr(out_r4, frameCount);
						Win32.LeaveCriticalSection(cs_vac);
					}
					else 
					{
						VACDebug("rb_vacOUT_l overflow ");
						vac_rb_reset = true;
					}
				} 
				else 
				{
					if (vac_stereo)
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))
							fixed(float *res_outr_ptr = &(res_outr[0])) 
							{
								int outsamps;
								DttSP.DoResamplerF(out_l4, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
								DttSP.DoResamplerF(out_r4, res_outr_ptr, frameCount, &outsamps, resampPtrOut_r);
								if((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
								{
									Win32.EnterCriticalSection(cs_vac);
									rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
									rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
									Win32.LeaveCriticalSection(cs_vac);
								}
								else
								{
									vac_rb_reset = true;
									VACDebug("rb_vacOUT_l overflow");
								}
							}
					}
					else 
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))
						{
							int outsamps;
							DttSP.DoResamplerF(out_l4, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
							if ((rb_vacOUT_l.WriteSpace() >= outsamps)&&(rb_vacOUT_r.WriteSpace() >= outsamps))
							{
								Win32.EnterCriticalSection(cs_vac);
								rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
								rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
								Win32.LeaveCriticalSection(cs_vac);
							}
							else
							{
								vac_rb_reset = true;
								VACDebug("rb_vacOUT_l overflow");
							}
						}
					}
				}
			}

			double tx_vol = FWCTXScale;
			if(tx_output_signal != SignalSource.RADIO)
				tx_vol = 1.0;
			// Scale output to FLEX-5000
			if(full_duplex)
			{
				if(mon)
				{
					ScaleBuffer(out_l2, out_l1, frameCount, (float)tx_vol);
					ScaleBuffer(out_l2, out_l2, frameCount, (float)monitor_volume);
					CopyBuffer(out_l2, out_l3, frameCount);
					CopyBuffer(out_l2, out_l4, frameCount);
					ScaleBuffer(out_r2, out_r1, frameCount, (float)tx_vol);	
					ScaleBuffer(out_r2, out_r2, frameCount, (float)monitor_volume);
					CopyBuffer(out_r2, out_r3, frameCount);
					CopyBuffer(out_r2, out_r4, frameCount);	
				}
				else
				{
					ScaleBuffer(out_l1, out_r3, frameCount, (float)monitor_volume);
					ScaleBuffer(out_r1, out_l3, frameCount, (float)monitor_volume);
					ScaleBuffer(out_l2, out_l1, frameCount, (float)tx_vol);
					ScaleBuffer(out_r2, out_r1, frameCount, (float)tx_vol);
					CopyBuffer(out_l3, out_l2, frameCount);
					CopyBuffer(out_r3, out_r2, frameCount);
					CopyBuffer(out_l3, out_l4, frameCount);
					CopyBuffer(out_r3, out_r4, frameCount);
				}
			}
			else if(!localmox)
			{				
				if(rx2_enabled)
				{
					AddBuffer(out_l1, out_l3, frameCount);
					AddBuffer(out_r1, out_r3, frameCount);
				}
				ScaleBuffer(out_l1, out_r3, frameCount, (float)monitor_volume);
				ScaleBuffer(out_r1, out_l3, frameCount, (float)monitor_volume);
				ScaleBuffer(out_l2, out_l1, frameCount, (float)tx_vol);
				ScaleBuffer(out_r2, out_r1, frameCount, (float)tx_vol);
				CopyBuffer(out_l3, out_l2, frameCount);
				CopyBuffer(out_r3, out_r2, frameCount);
				CopyBuffer(out_l3, out_l4, frameCount);				
				CopyBuffer(out_r3, out_r4, frameCount);				
				//ClearBuffer(out_l1, frameCount);
				//ClearBuffer(out_r1, frameCount);
			}
			else
			{
				if(high_pwr_am)
				{
					if(tx_dsp_mode == DSPMode.AM ||
						tx_dsp_mode == DSPMode.SAM)
						tx_vol *= 1.414;
				}
				
				ScaleBuffer(out_l2, out_l1, frameCount, (float)tx_vol);
				if(mon)
				{
					ScaleBuffer(out_l2, out_l2, frameCount, (float)monitor_volume);
					CopyBuffer(out_l2, out_l3, frameCount);
					CopyBuffer(out_l2, out_l4, frameCount);
				}
				else
				{
					if(rx2_enabled && !rx2_auto_mute_tx)
					{
						ScaleBuffer(out_l3, out_l2, frameCount, (float)monitor_volume);
						CopyBuffer(out_l2, out_l3, frameCount);
                        CopyBuffer(out_l2, out_l4, frameCount);
					}
					else
					{
						ClearBuffer(out_l2, frameCount);
						ClearBuffer(out_l3, frameCount);
						ClearBuffer(out_l4, frameCount);
					}
				}

				ScaleBuffer(out_r2, out_r1, frameCount, (float)tx_vol);	
				if(mon)
				{
					ScaleBuffer(out_r2, out_r2, frameCount, (float)monitor_volume);
					CopyBuffer(out_r2, out_r3, frameCount);
					CopyBuffer(out_r2, out_r4, frameCount);							
				}
				else
				{
					if(rx2_enabled && !rx2_auto_mute_tx)
					{
						ScaleBuffer(out_r3, out_r2, frameCount, (float)monitor_volume);
						CopyBuffer(out_r2, out_r3, frameCount);
						CopyBuffer(out_r2, out_r4, frameCount);
					}
					else
					{
						ClearBuffer(out_r2, frameCount);
						ClearBuffer(out_r3, frameCount);
						ClearBuffer(out_r4, frameCount);
					}
				}
			}

#if(MINMAX)
			/*Debug.Write(MaxSample(out_l2, out_r2, frameCount).ToString("f6")+",");

			float current_max = MaxSample(out_l2, out_r2, frameCount);
			if(current_max > max) max = current_max;
			Debug.WriteLine(" max: "+max.ToString("f6"));*/
#endif

			//Debug.WriteLine(MaxSample(out_l2, out_r2, frameCount).ToString("f6"));
            //if(period > 8) Debug.
            //    WriteLine("period: " + period.ToString("f2"));
#if(TIMER)
			t1.Stop();
			Debug.WriteLine(t1.Duration);
#endif
            /*t2.Stop();
            period = t2.DurationMsec;
            if(period > 1.0 || statusFlags != 0)
                Debug.WriteLine("flags: " + statusFlags.ToString("X") + "  period: " + period.ToString("f2"));*/

			return callback_return;
		}
				// The VAC callback from 1.8.0 untouched in any way.
		unsafe public static int CallbackVAC(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
			int* array_ptr = (int *)input;
			float* in_l_ptr1 = (float *)array_ptr[0];
			float* in_r_ptr1 = null;
			if(vac_stereo || vac_output_iq) in_r_ptr1 = (float *)array_ptr[1];
			array_ptr = (int *)output;
			float* out_l_ptr1 = (float *)array_ptr[0];
			float* out_r_ptr1 = null;
			if(vac_stereo || vac_output_iq) out_r_ptr1 = (float *)array_ptr[1];

			if (vac_rb_reset)
			{
				vac_rb_reset = false;
				ClearBuffer(out_l_ptr1,frameCount);
				if (vac_stereo || vac_output_iq) ClearBuffer(out_r_ptr1,frameCount);
				Win32.EnterCriticalSection(cs_vac);
				rb_vacIN_l.Reset();
				rb_vacIN_r.Reset();
				rb_vacOUT_l.Reset();
				rb_vacOUT_r.Reset();
				Win32.LeaveCriticalSection(cs_vac);
				return 0;
			}
			if (vac_stereo || vac_output_iq)
			{
				if (vac_resample) 
				{
					int outsamps;
					fixed(float *res_inl_ptr = &(res_inl[0]))
						fixed(float *res_inr_ptr = &(res_inr[0])) 
						{
							DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr,frameCount,&outsamps, resampPtrIn_l);
							DttSP.DoResamplerF(in_r_ptr1, res_inr_ptr,frameCount,&outsamps, resampPtrIn_r);
							if ((rb_vacIN_l.WriteSpace() >= outsamps)&&(rb_vacIN_r.WriteSpace() >= outsamps))
							{
								Win32.EnterCriticalSection(cs_vac);
								rb_vacIN_l.WritePtr(res_inl_ptr,outsamps);
								rb_vacIN_r.WritePtr(res_inr_ptr,outsamps);
								Win32.LeaveCriticalSection(cs_vac);
							}
							else 
							{
								vac_rb_reset = true;
								VACDebug("rb_vacIN overflow stereo CBvac");
							}
						}
				} 
				else 
				{
					if ((rb_vacIN_l.WriteSpace() >= frameCount)&&(rb_vacIN_r.WriteSpace() >= frameCount))
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacIN_l.WritePtr(in_l_ptr1,frameCount);
						rb_vacIN_r.WritePtr(in_r_ptr1,frameCount);
						Win32.LeaveCriticalSection(cs_vac);
					}
					else
					{
						//vac_rb_reset = true;
						VACDebug("rb_vacIN overflow mono CBvac");
					}
				}
				
				if ((rb_vacOUT_l.ReadSpace() >= frameCount)&&(rb_vacOUT_r.ReadSpace() >= frameCount))
				{
					Win32.EnterCriticalSection(cs_vac);
					rb_vacOUT_l.ReadPtr(out_l_ptr1,frameCount);
					rb_vacOUT_r.ReadPtr(out_r_ptr1,frameCount);
					Win32.LeaveCriticalSection(cs_vac);
				}
				else
				{
					ClearBuffer(out_l_ptr1,frameCount);
					ClearBuffer(out_r_ptr1,frameCount);
					VACDebug("rb_vacOUT underflow");
				}
			} 
			else 
			{
				if (vac_resample) 
				{
					int outsamps;
					fixed(float *res_inl_ptr = &(res_inl[0]))
					{
						DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr,frameCount,&outsamps, resampPtrIn_l);
						if ((rb_vacIN_l.WriteSpace() >= outsamps)&&(rb_vacIN_r.WriteSpace() >= outsamps))
						{
							Win32.EnterCriticalSection(cs_vac);
							rb_vacIN_l.WritePtr(res_inl_ptr,outsamps);
							rb_vacIN_r.WritePtr(res_inl_ptr,outsamps);
							Win32.LeaveCriticalSection(cs_vac);
						}
						else 
						{
							//vac_rb_reset = true;
							VACDebug("rb_vacIN_l overflow");
						}
					}
				} 
				else 
				{
					if ((rb_vacIN_l.WriteSpace() >= frameCount)&& (rb_vacIN_r.WriteSpace() >= frameCount))
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacIN_l.WritePtr(in_l_ptr1,frameCount);
						rb_vacIN_r.WritePtr(in_l_ptr1,frameCount);
						Win32.LeaveCriticalSection(cs_vac);
					}
					else
					{
						//vac_rb_reset = true;
						VACDebug("rb_vacIN_l overflow");
					}
				}
				if ((rb_vacOUT_l.ReadSpace() >= frameCount)&&(rb_vacOUT_r.ReadSpace() >= frameCount))
				{
					Win32.EnterCriticalSection(cs_vac);
					rb_vacOUT_l.ReadPtr(out_l_ptr1,frameCount);
					rb_vacOUT_r.ReadPtr(out_l_ptr1,frameCount);
					Win32.LeaveCriticalSection(cs_vac);
				}
				else 
				{
					ClearBuffer(out_l_ptr1,frameCount);
					VACDebug("rb_vacOUT_l underflow");
				}
			}

			return 0;
		}


		#region NewVacCallBack
		/*  The broken new stuff.
				unsafe public static int CallbackVAC(void* input, void* output, int frameCount,
					PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
				{
					int* array_ptr = (int *)input;
					float* in_l_ptr1 = (float *)array_ptr[0];
					float* in_r_ptr1 = (float *)array_ptr[1];
					array_ptr = (int *)output;
					float* out_l_ptr1 = (float *)array_ptr[0];
					float* out_r_ptr1 = (float *)array_ptr[1];

					if ((statusFlags & PA19.paInputOverflow) != 0) VACDebug("callback input overflow");
					if ((statusFlags & PA19.paInputUnderflow) != 0) VACDebug("callback input underflow");
					if ((statusFlags & PA19.paOutputOverflow) != 0) VACDebug("callback output overflow");
					if ((statusFlags & PA19.paOutputUnderflow) != 0) VACDebug("callback output underflow");
					//Debug.WriteLine("statusFlags = "+statusFlags.ToString());


					if (vac_rb_reset)
					{
						vac_rb_reset = false;
						ClearBuffer(out_l_ptr1, frameCount);
						if (vac_stereo) ClearBuffer(out_r_ptr1, frameCount);
						Win32.EnterCriticalSection(cs_vac);

						rb_vacIN_l.Reset();
						rb_vacIN_r.Reset();
						rb_vacOUT_l.Restart(frameCount);
						rb_vacOUT_r.Restart(frameCount);
						Win32.LeaveCriticalSection(cs_vac);
						return 0;
					}

					if (vac_stereo)
					{
						if (vac_resample) 
						{
							int outsamps;
							fixed(float *res_inl_ptr = &(res_inl[0]))
								fixed(float *res_inr_ptr = &(res_inr[0])) 
								{
									DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr, frameCount, &outsamps, resampPtrIn_l);
									DttSP.DoResamplerF(in_r_ptr1, res_inr_ptr, frameCount, &outsamps, resampPtrIn_r);
									Debug.WriteLine("number samples from resampler " + outsamps.ToString());
									if ((rb_vacIN_l.WriteSpace() >= outsamps) && (rb_vacIN_r.WriteSpace() >= outsamps))
									{
										Win32.EnterCriticalSection(cs_vac);
										rb_vacIN_l.WritePtr(res_inl_ptr, outsamps);
										rb_vacIN_r.WritePtr(res_inr_ptr, outsamps);
										Win32.LeaveCriticalSection(cs_vac);
									}
									else 
									{
										vac_rb_reset = true;
										VACDebug("rb_vacIN overflow stereo CBvac");
									}
								}
						} 
						else 
						{
							if((rb_vacIN_l.WriteSpace() >= frameCount) && (rb_vacIN_r.WriteSpace() >= frameCount))
							{
								Win32.EnterCriticalSection(cs_vac);
								rb_vacIN_l.WritePtr(in_l_ptr1, frameCount);
								rb_vacIN_r.WritePtr(in_r_ptr1, frameCount);
								Win32.LeaveCriticalSection(cs_vac);
							}
							else
							{
								//vac_rb_reset = true;
								VACDebug("rb_vacIN overflow mono CBvac");
							}
						}
				
						if((rb_vacOUT_l.ReadSpace() >= frameCount) && (rb_vacOUT_r.ReadSpace() >= frameCount))
						{
							Win32.EnterCriticalSection(cs_vac);
							rb_vacOUT_l.ReadPtr(out_l_ptr1, frameCount);
							rb_vacOUT_r.ReadPtr(out_r_ptr1, frameCount);
							Win32.LeaveCriticalSection(cs_vac);
						}
						else
						{
							ClearBuffer(out_l_ptr1, frameCount);
							ClearBuffer(out_r_ptr1, frameCount);
							VACDebug("rb_vacOUT underflow");
						}
					} 
					else 
					{
						if(vac_resample) 
						{
							int outsamps;
							fixed(float *res_inl_ptr = &(res_inl[0]))
							{
								DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr, frameCount, &outsamps, resampPtrIn_l);
								//Debug.WriteLine("number samples from resampler " + outsamps.ToString());
								if ((rb_vacIN_l.WriteSpace() >= outsamps) && (rb_vacIN_r.WriteSpace() >= outsamps))
								{
									Win32.EnterCriticalSection(cs_vac);
									rb_vacIN_l.WritePtr(res_inl_ptr, outsamps);
									rb_vacIN_r.WritePtr(res_inl_ptr, outsamps);
									Win32.LeaveCriticalSection(cs_vac);
								}
								else 
								{
									//vac_rb_reset = true;
									VACDebug("rb_vacIN_l overflow");
								}
							}
						} 
						else 
						{
							if((rb_vacIN_l.WriteSpace() >= frameCount) && (rb_vacIN_r.WriteSpace() >= frameCount))
							{
								Win32.EnterCriticalSection(cs_vac);
								rb_vacIN_l.WritePtr(in_l_ptr1, frameCount);
								rb_vacIN_r.WritePtr(in_l_ptr1, frameCount);
								Win32.LeaveCriticalSection(cs_vac);
							}
							else
							{
								//vac_rb_reset = true;
								VACDebug("rb_vacIN_l overflow");
							}
						}
						if((rb_vacOUT_l.ReadSpace() >= frameCount) && (rb_vacOUT_r.ReadSpace() >= frameCount))
						{
							//Debug.WriteLine("vacOut Readspace "+rb_vacOUT_l.ReadSpace().ToString());
							Win32.EnterCriticalSection(cs_vac);
							rb_vacOUT_l.ReadPtr(out_l_ptr1, frameCount);
							rb_vacOUT_r.ReadPtr(out_l_ptr1, frameCount);
							Win32.LeaveCriticalSection(cs_vac);
						}
						else 
						{
							ClearBuffer(out_l_ptr1,frameCount);
							VACDebug("rb_vacOUT_l underflow");
						}
					}

					return 0;
				}
		*/

		#endregion

		unsafe public static int Pipe(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
			float *inptr = (float *)input;
			float *outptr = (float *)output;

			for(int i=0; i<frameCount; i++)
			{
				*outptr++ = *inptr++;
				*outptr++ = *inptr++;
			}
			return 0;
		}

		#endregion

		#region Buffer Operations

        unsafe private static float SumBuffer(float* buf, int samples)
        {
            float temp = 0.0f;
            for (int i = 0; i < samples; i++)
                temp += buf[i];
            return temp;
        }

        unsafe private static void ClearBuffer(float* buf, int samples)
		{
			Win32.memset(buf, 0, samples*sizeof(float));
		}

		unsafe private static void CopyBuffer(float *inbuf, float *outbuf, int samples)
		{
			Win32.memcpy(outbuf, inbuf, samples*sizeof(float));
		}

		unsafe private static void ScaleBuffer(float *inbuf, float *outbuf, int samples, float scale)
		{
			for(int i=0; i<samples; i++)
				outbuf[i] = inbuf[i] * scale;
		}

        unsafe private static void MixBuffer(float* inbuf, float* buf2mix, float* outbuf, int samples, float scale, float scale2)
        {
            for (int i = 0; i < samples; i++)
                outbuf[i] = (inbuf[i] * scale) + (buf2mix[i] * scale2);
        }

		unsafe private static void AddBuffer(float* dest, float *buftoadd, int samples)
		{
			for(int i=0; i<samples; i++)
				dest[i] += buftoadd[i];
		}

		unsafe public static float MaxSample(float* buf, int samples)
		{
			float max = float.MinValue;
			for(int i=0; i<samples; i++)
				if(buf[i] > max) max = buf[i];
				
			return max;
		}

		unsafe public static float MaxSample(float* buf1, float* buf2, int samples)
		{
			float max = float.MinValue;
			for(int i=0; i<samples; i++)
			{
				if(buf1[i] > max) max = buf1[i];
				if(buf2[i] > max) max = buf2[i];
			}
			return max;
		}

		unsafe public static float MinSample(float* buf, int samples)
		{
			float min = float.MaxValue;
			for(int i=0; i<samples; i++)
				if(buf[i] < min) min = buf[i];
				
			return min;
		}

		unsafe public static float MinSample(float* buf1, float* buf2, int samples)
		{
			float min = float.MaxValue;
			for(int i=0; i<samples; i++)
			{
				if(buf1[i] < min) min = buf1[i];
				if(buf2[i] < min) min = buf2[i];
			}
				
			return min;
		}

		unsafe private static void CorrectIQBuffer(float *inbufI, float *inbufQ, float *outbufI, float *outbufQ, int samples)
		{
			//float phase = (float) (0.001 * console.radio.GetDSPRX(0, 0).RXCorrectIQPhase);
			//float gain = (float) (1.0 + 0.001 * console.radio.GetDSPRX(0, 0).RXCorrectIQGain);
			for (int i=0; i<samples; i++)
			{
                if (vac_output_rx2)
                {
                    outbufI[i] = inbufI[i] + iq_phase2 * inbufQ[i];
                    outbufQ[i] = inbufQ[i] * iq_gain2;
                }
                else
                {
                    outbufI[i] = inbufI[i] + iq_phase * inbufQ[i];
                    outbufQ[i] = inbufQ[i] * iq_gain;
                }
			}
		}

		// returns updated phase accumulator
		unsafe public static double SineWave(float* buf, int samples, double phase, double freq)
		{
			double phase_step = freq/sample_rate1*2*Math.PI;
			double cosval = Math.Cos(phase);
			double sinval = Math.Sin(phase);
			double cosdelta = Math.Cos(phase_step);
			double sindelta = Math.Sin(phase_step);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval*cosdelta - sinval*sindelta;
				sinval = cosval*sindelta + sinval*cosdelta;
				cosval = tmpval;
						
				buf[i] = (float)(sinval);
											 
				phase += phase_step;
			}

			return phase;
		}

		// returns updated phase accumulator
		unsafe public static double CosineWave(float* buf, int samples, double phase, double freq)
		{
			double phase_step = freq/sample_rate1*2*Math.PI;
			double cosval = Math.Cos(phase);
			double sinval = Math.Sin(phase);
			double cosdelta = Math.Cos(phase_step);
			double sindelta = Math.Sin(phase_step);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval*cosdelta - sinval*sindelta;
				sinval = cosval*sindelta + sinval*cosdelta;
				cosval = tmpval;
						
				buf[i] = (float)(cosval);
											 
				phase += phase_step;
			}

			return phase;
		}

		unsafe public static void SineWave2Tone(float* buf, int samples, 
			double phase1, double phase2, 
			double freq1, double freq2,
			out double updated_phase1, out double updated_phase2)
		{
			double phase_step1 = freq1/sample_rate1*2*Math.PI;
			double cosval1 = Math.Cos(phase1);
			double sinval1 = Math.Sin(phase1);
			double cosdelta1 = Math.Cos(phase_step1);
			double sindelta1 = Math.Sin(phase_step1);

			double phase_step2 = freq2/sample_rate1*2*Math.PI;
			double cosval2 = Math.Cos(phase2);
			double sinval2 = Math.Sin(phase2);
			double cosdelta2 = Math.Cos(phase_step2);
			double sindelta2 = Math.Sin(phase_step2);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval1*cosdelta1 - sinval1*sindelta1;
				sinval1 = cosval1*sindelta1 + sinval1*cosdelta1;
				cosval1 = tmpval;

				tmpval = cosval2*cosdelta2 - sinval2*sindelta2;
				sinval2 = cosval2*sindelta2 + sinval2*cosdelta2;
				cosval2 = tmpval;
						
				buf[i] = (float)(sinval1*0.5 + sinval2*0.5);
											 
				phase1 += phase_step1;
				phase2 += phase_step2;
			}

			updated_phase1 = phase1;
			updated_phase2 = phase2;
		}

		unsafe public static void CosineWave2Tone(float* buf, int samples, 
			double phase1, double phase2, 
			double freq1, double freq2,
			out double updated_phase1, out double updated_phase2)
		{
			double phase_step1 = freq1/sample_rate1*2*Math.PI;
			double cosval1 = Math.Cos(phase1);
			double sinval1 = Math.Sin(phase1);
			double cosdelta1 = Math.Cos(phase_step1);
			double sindelta1 = Math.Sin(phase_step1);

			double phase_step2 = freq2/sample_rate1*2*Math.PI;
			double cosval2 = Math.Cos(phase2);
			double sinval2 = Math.Sin(phase2);
			double cosdelta2 = Math.Cos(phase_step2);
			double sindelta2 = Math.Sin(phase_step2);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval1*cosdelta1 - sinval1*sindelta1;
				sinval1 = cosval1*sindelta1 + sinval1*cosdelta1;
				cosval1 = tmpval;

				tmpval = cosval2*cosdelta2 - sinval2*sindelta2;
				sinval2 = cosval2*sindelta2 + sinval2*cosdelta2;
				cosval2 = tmpval;
						
				buf[i] = (float)(cosval1*0.5 + cosval2*0.5);
											 
				phase1 += phase_step1;
				phase2 += phase_step2;
			}

			updated_phase1 = phase1;
			updated_phase2 = phase2;
		}

		
		private static Random r = new Random();
		private static double y2=0.0;
		private static bool use_last = false;
		private static double boxmuller (double m,double s)
		{
			double x1,x2,w,y1;
			if (use_last)		        /* use value from previous call */
			{
				y1 = y2;
				use_last = false;
			}
			else
			{
				do 
				{
					x1 = (2.0 * r.NextDouble() - 1.0);
					x2 = (2.0 * r.NextDouble() - 1.0);
					w = x1 * x1 + x2 * x2;
				} while ( w >= 1.0 );

				w = Math.Sqrt( (-2.0 * Math.Log( w ) ) / w );
				y1 = x1 * w;
				y2 = x2 * w;
				use_last = true;
			}

			return( m + y1 * s );
		}
		unsafe public static void Noise(float* buf, int samples)
		{
			for(int i=0; i<samples; i++)
			{
				buf[i] = (float)boxmuller(0.0,0.2);
			}
		}

		private static double tri_val = 0.0;
		private static int tri_direction = 1;
		unsafe public static void Triangle(float* buf, int samples, double freq)
		{
			double step = freq/sample_rate1*2*tri_direction;
			for(int i=0; i<samples; i++)
			{
				buf[i] = (float)tri_val;
				tri_val += step;
				if(tri_val >= 1.0 || tri_val <= -1.0)
				{
					step = -step;
					tri_val += 2*step;
					if(step < 0) tri_direction = -1;
					else tri_direction = 1;
				}
			}
		}

		private static double saw_val = 0.0;
		private static int saw_direction = 1;
		unsafe public static void Sawtooth(float* buf, int samples, double freq)
		{
			double step = freq/sample_rate1*saw_direction;
			for(int i=0; i<samples; i++)
			{
				buf[i] = (float)saw_val;
				saw_val += step;
				if(saw_val >= 1.0) saw_val -= 2.0;
				if(saw_val <= -1.0) saw_val += 2.0;
			}
		}

		unsafe public static void AddConstant(float* buf, int samples, float val)
		{
			for(int i=0; i<samples; i++)
				buf[i] += val;
		}

		#endregion

		#region Misc Routines
		// ======================================================
		// Misc Routines
		// ======================================================

		private static void VACDebug(string s)
		{
#if(VAC_DEBUG)
			Debug.WriteLine(s);
#endif
		}

#if (NewVAC)
		#region NewInitVac
		unsafe private static void InitVAC()
		{
			
			if(rb_vacOUT_l == null) rb_vacOUT_l = new RingBufferFloat(4*block_size2);
			rb_vacOUT_l.Restart(block_size2);
			
			if(rb_vacOUT_r == null) rb_vacOUT_r = new RingBufferFloat(4*block_size2);
			rb_vacOUT_r.Restart(block_size2);
			int buf_size = rb_vacOUT_r.nblock2(4*(block_size2>block_size1 ? block_size2:block_size1)*sample_rate1/sample_rate2);
			Debug.WriteLine("Vac Bufsize = "+buf_size.ToString());
			if(rb_vacIN_l == null) rb_vacIN_l = new RingBufferFloat(buf_size);
			rb_vacIN_l.Reset();

			if(rb_vacIN_r == null) rb_vacIN_r = new RingBufferFloat(buf_size);
			rb_vacIN_r.Reset();

			if (sample_rate2 != sample_rate1) 
			{
				vac_resample = true;
				if(res_outl == null) res_outl = new float [buf_size];
				if(res_outr == null) res_outr = new float [buf_size];
				if(res_inl == null) res_inl  = new float [buf_size];
				if(res_inr == null) res_inr  = new float [buf_size];

				resampPtrIn_l  = DttSP.NewResamplerF(sample_rate2, sample_rate1);
				resampPtrIn_r  = DttSP.NewResamplerF(sample_rate2, sample_rate1);
				resampPtrOut_l = DttSP.NewResamplerF(sample_rate1, sample_rate2);
				resampPtrOut_r = DttSP.NewResamplerF(sample_rate1, sample_rate2);
			}
			else vac_resample = false;
			cs_vac = (void *)0x0;
			cs_vac = Win32.NewCriticalSection();
			if (Win32.InitializeCriticalSectionAndSpinCount(cs_vac, 0x00000080) == 0)
			{
				vac_enabled = false;
				Debug.WriteLine("CriticalSection Failed");
			}
		}
		#endregion
#else
		unsafe private static void InitVAC()
		{
			if(rb_vacOUT_l == null) rb_vacOUT_l = new RingBufferFloat(2*65536);
			rb_vacOUT_l.Restart(vac_output_iq ? block_size1 : block_size_vac);
			
			if(rb_vacOUT_r == null) rb_vacOUT_r = new RingBufferFloat(2*65536);
			rb_vacOUT_r.Restart(vac_output_iq ? block_size1 : block_size_vac);

			if(rb_vacIN_l == null) rb_vacIN_l = new RingBufferFloat(4*65536);
			rb_vacIN_l.Restart(block_size_vac);

			if(rb_vacIN_r == null) rb_vacIN_r = new RingBufferFloat(4*65536);
			rb_vacIN_r.Restart(block_size_vac);

			if (sample_rate2 != sample_rate1 && !vac_output_iq) 
			{
				vac_resample = true;
				if(res_outl == null) res_outl = new float [65536];
				if(res_outr == null) res_outr = new float [65536];
				if(res_inl == null) res_inl  = new float [4*65536];
				if(res_inr == null) res_inr  = new float [4*65536];

				resampPtrIn_l  = DttSP.NewResamplerF(sample_rate2,sample_rate1);
				resampPtrIn_r  = DttSP.NewResamplerF(sample_rate2,sample_rate1);
				resampPtrOut_l = DttSP.NewResamplerF(sample_rate1,sample_rate2);
				resampPtrOut_r = DttSP.NewResamplerF(sample_rate1,sample_rate2);
			}
			else
			{
				vac_resample = false;
				if (vac_output_iq)
				{
					if(res_outl == null) res_outl = new float [65536];
					if(res_outr == null) res_outr = new float [65536];
				}
			}
			cs_vac = (void *)0x0;
			cs_vac = Win32.NewCriticalSection();
			if (Win32.InitializeCriticalSectionAndSpinCount(cs_vac, 0x00000080) == 0)
			{
				vac_enabled = false;
				Debug.WriteLine("CriticalSection Failed");
			}
		}

#endif
		unsafe private static void CleanUpVAC()
		{
			Win32.DeleteCriticalSection(cs_vac);
			rb_vacOUT_l = null;
			rb_vacOUT_r = null;
			rb_vacIN_l = null;
			rb_vacIN_r = null;

			res_outl = null;
			res_outr = null;
			res_inl = null;
			res_inr = null;

			resampPtrIn_l = null;
			resampPtrIn_r = null;
			resampPtrOut_l = null;
			resampPtrOut_r = null;
			Win32.DestroyCriticalSection(cs_vac);
		}

		unsafe public static double GetCPULoad()
		{
			return PA19.PA_GetStreamCpuLoad(stream1);
		}

		public static ArrayList GetPAHosts() // returns a text list of driver types
		{
			ArrayList a = new ArrayList();
			for(int i=0; i<PA19.PA_GetHostApiCount(); i++)
			{
				PA19.PaHostApiInfo info = PA19.PA_GetHostApiInfo(i);
				a.Add(info.name);
			}
            a.Add("Janus/Ozy");
            return a;
		}

		public static ArrayList GetPAInputDevices(int hostIndex)
		{
			ArrayList a = new ArrayList();

            if (hostIndex >= PA19.PA_GetHostApiCount()) //xylowolf 
            {
                a.Add(new PADeviceInfo("AK5394A", 0));
                return a;
            }

            PA19.PaHostApiInfo hostInfo = PA19.PA_GetHostApiInfo(hostIndex);
			for(int i=0; i<hostInfo.deviceCount; i++)
			{
				int devIndex = PA19.PA_HostApiDeviceIndexToDeviceIndex(hostIndex, i);
				PA19.PaDeviceInfo devInfo = PA19.PA_GetDeviceInfo(devIndex);
				if(devInfo.maxInputChannels > 0)
					a.Add(new PADeviceInfo(devInfo.name, i)/* + " - " + devIndex*/);
			}
			return a;
		}	

		public static ArrayList GetPAOutputDevices(int hostIndex)
		{
			ArrayList a = new ArrayList();

            if (hostIndex >= PA19.PA_GetHostApiCount()) 
            {
                a.Add(new PADeviceInfo("FPGA PWM D/A", 0));
                return a;
            }

            PA19.PaHostApiInfo hostInfo = PA19.PA_GetHostApiInfo(hostIndex);
			for(int i=0; i<hostInfo.deviceCount; i++)
			{
				int devIndex = PA19.PA_HostApiDeviceIndexToDeviceIndex(hostIndex, i);
				PA19.PaDeviceInfo devInfo = PA19.PA_GetDeviceInfo(devIndex);
				if(devInfo.maxOutputChannels > 0)
					a.Add(new PADeviceInfo(devInfo.name, i)/* + " - " + devIndex*/);
			}
			return a;
		}	

		public static bool Start()
		{
			bool retval = false;
			phase_buf_l = new float[block_size1];
			phase_buf_r = new float[block_size1];
			if(console.fwc_init && (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX3000))
			{
				switch(console.CurrentModel)
				{
					case Model.FLEX5000:
						in_rx1_l = 0;
						in_rx1_r = 1;
						break;
					case Model.FLEX3000:
						in_rx1_l = 1;
						in_rx1_r = 0;
						break;
				}				
				in_rx2_l = 2;
				in_rx2_r = 3;
				//in_tx_l = 5;
				//in_tx_r = 6;
				retval = StartAudio(ref callback8, (uint)block_size1, sample_rate1, host1, input_dev1, output_dev1, 8, 0, latency1);
			}
			else
			{
				if(num_channels == 2)
					retval = StartAudio(ref callback1, (uint)block_size1, sample_rate1, host1, input_dev1, output_dev1, num_channels, 0, latency1);
				else if(num_channels == 4)
					retval = StartAudio(ref callback4port, (uint)block_size1, sample_rate1, host1, input_dev1, output_dev1, num_channels, 0, latency1);
			}
			
			if(!retval) return retval;

			if(vac_enabled)
			{
				int num_chan = 1;
				// ehr add for multirate iq to vac
				int sample_rate = sample_rate2;
				int block_size = block_size_vac;
				int latency = latency2;
				if (vac_output_iq)
				{
					num_chan = 2;
					sample_rate = sample_rate1;
					block_size = block_size1;
					//latency = 250;
				}
				else if(vac_stereo) num_chan = 2;
				// ehr end				
				vac_rb_reset = true;
				retval = StartAudio_NonJanus(ref callbackVAC, (uint)block_size, sample_rate, host2, input_dev2, output_dev2, num_chan, 1, latency);
			}

			return retval;
		}
        
		private static bool using_janus_audio = false;

        public unsafe static bool StartAudio(ref PA19.PaStreamCallback callback,
            uint block_size, double sample_rate, int host_api_index, int input_dev_index,
            int output_dev_index, int num_channels, int callback_num, int latency_ms)
        {


            // !!KD5TFD FIXME TODO bug: this check is not right when the channel being started is VAC 
            // if VAC and FPGA audio is being used it ends up in the else branch -- bad! 
            //
            if (host_api_index < PA19.PA_GetHostApiCount())  // using classic NonJanus audio 
            {
                using_janus_audio = false;
                System.Console.WriteLine("using classic audio");
                return StartAudio_NonJanus(ref callback, block_size, sample_rate, host_api_index, input_dev_index,
                    output_dev_index, num_channels, callback_num, latency_ms);
            }
            else  // using janus audio device 
            {
                using_janus_audio = true;
                System.Console.WriteLine("using Ozy/Janus callback");
                int rc;
                int no_send = 0;
                int sample_bits = 24;
                if (console.Force16bitIQ)
                {
                    sample_bits = 16;
                }
                if (console.NoJanusSend)
                {
                    no_send = 1;
                }
                rc = JanusAudio.StartAudio((int)sample_rate, (int)block_size, callback, sample_bits, no_send);
                if (rc != 0)
                {
                    System.Console.WriteLine("JanusAudio.StartAudio failed w/ rc: " + rc);
                    if (rc == -101)  // firmware version error; 
                    {
                        string fw_err = JanusAudio.getFWVersionErrorMsg();
                        if (fw_err == null)
                        {
                            fw_err = "Bad Firmware levels";
                        }
                        MessageBox.Show(fw_err, "HPSDR Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        MessageBox.Show("Error starting HPSDR hardware, is it connected and powered?", "HPSDR Error",
                                MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return false;
                    }
                }
                return true;
            }
        }

		public unsafe static bool StartAudio_NonJanus(ref PA19.PaStreamCallback callback,
			uint block_size, double sample_rate, int host_api_index, int input_dev_index,
			int output_dev_index, int num_channels, int callback_num, int latency_ms)
		{
            empty_buffers = 0;

			int in_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, input_dev_index);
			int out_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, output_dev_index);

			PA19.PaStreamParameters inparam = new PA19.PaStreamParameters();
			PA19.PaStreamParameters outparam = new PA19.PaStreamParameters();
				
			inparam.device = in_dev;
			inparam.channelCount = num_channels;
#if(INTERLEAVED)
			inparam.sampleFormat = PA19.paFloat32;
#else
			inparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
#endif
			inparam.suggestedLatency = ((float)latency_ms/1000);

			outparam.device = out_dev;
			outparam.channelCount = num_channels;
#if(INTERLEAVED)
			outparam.sampleFormat = PA19.paFloat32;
#else
			outparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
#endif
			outparam.suggestedLatency = ((float)latency_ms/1000);
			
			int error = 0;
			if(callback_num == 0)
				error = PA19.PA_OpenStream(out stream1, &inparam, &outparam, sample_rate, block_size, 0, callback, 0);
			else	
				error = PA19.PA_OpenStream(out stream2, &inparam, &outparam, sample_rate, block_size, 0, callback, 1);
			
			if(error != 0)
			{
				MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			
			if(callback_num == 0)
				error = PA19.PA_StartStream(stream1);
			else
				error = PA19.PA_StartStream(stream2);

			if(error != 0)
			{
				MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			return true;
		}

        public unsafe static void StopAudio1()
        {
            System.Console.WriteLine("Stop Audio called");
            if (!using_janus_audio)  // using classic portaudio 
            {
                StopAudio1_NonJanus();
            }
            else  // using tfdaph audio 
            {
                JanusAudio.StopAudio();

            }
        } 

        public unsafe static void StopAudio1_NonJanus()
		{
			PA19.PA_AbortStream(stream1);
			PA19.PA_CloseStream(stream1);
		}

		public unsafe static void StopAudioVAC()
		{
			PA19.PA_AbortStream(stream2);
			PA19.PA_CloseStream(stream2);
		}

		#endregion		

		#region Scope Stuff

		private static int scope_samples_per_pixel = 512;
		public static int ScopeSamplesPerPixel
		{
			get { return scope_samples_per_pixel; }
			set { scope_samples_per_pixel = value; }
		}

		private static int scope_display_width = 704;
		public static int ScopeDisplayWidth
		{
			get { return scope_display_width; }
			set { scope_display_width = value; }
		}

		private static int scope_sample_index = 0;
		private static int scope_pixel_index = 0;
		private static float scope_pixel_min = float.MaxValue;
		private static float scope_pixel_max = float.MinValue;
		private static float[] scope_min;
		public static float[] ScopeMin
		{
			set { scope_min = value; }
		}
		public static float[] scope_max;
		public static float[] ScopeMax
		{
			set { scope_max = value; }
		}

		unsafe private static void DoScope(float* buf, int frameCount)
		{
			if(scope_min == null || scope_min.Length < scope_display_width) 
			{
				if(Display.ScopeMin == null || Display.ScopeMin.Length < scope_display_width)
					Display.ScopeMin = new float[scope_display_width];
				scope_min = Display.ScopeMin;
			}
			if(scope_max == null || scope_max.Length < scope_display_width)
			{
				if(Display.ScopeMax == null || Display.ScopeMax.Length < scope_display_width)
					Display.ScopeMax = new float[scope_display_width];
				scope_max = Display.ScopeMax;
			}

			for(int i=0; i<frameCount; i++)
			{
				if(buf[i] < scope_pixel_min) scope_pixel_min = buf[i];
				if(buf[i] > scope_pixel_max) scope_pixel_max = buf[i];

				scope_sample_index++;
				if(scope_sample_index >= scope_samples_per_pixel)
				{
					scope_sample_index = 0;
					scope_min[scope_pixel_index] = scope_pixel_min;
					scope_max[scope_pixel_index] = scope_pixel_max;

					scope_pixel_min = float.MaxValue;
					scope_pixel_max = float.MinValue;

					scope_pixel_index++;
					if(scope_pixel_index >= scope_display_width)
						scope_pixel_index = 0;
				}
			}
		}

		#endregion
	}
}
