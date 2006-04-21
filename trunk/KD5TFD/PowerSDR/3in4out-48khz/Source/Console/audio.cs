//=================================================================
// audio.cs
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
//    FlexRadio Systems, 8900 Marybank Dr., Austin, TX  78750, USA.
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
			SINL_COSR,
			SINL_SINR,
			PIPE,
			SINL_NOR,
			NOL_SINR,
			NOL_NOR,
			SWITCH,
		}

		public enum SignalSource
		{
			SOUNDCARD,
			SINE,
			NOISE,
			TRIANGLE,
			SAWTOOTH,
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
#endif

		public static int callback_return = 0;

		private static double input_source_scale = 1.0;
		public static double InputSourceScale
		{
			get { return input_source_scale; }
			set { input_source_scale = value; }
		}

		private static SignalSource current_input_signal = SignalSource.SOUNDCARD;
		public static SignalSource CurrentInputSignal
		{
			get { return current_input_signal; }
			set { current_input_signal = value; }
		}

		private static SignalSource current_output_signal = SignalSource.SOUNDCARD;
		public static SignalSource CurrentOutputSignal
		{
			get { return current_output_signal; }
			set { current_output_signal = value; }
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

		public static double TXScale
		{
			get { return high_swr_scale * radio_volume; }
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
			}
		}

		private static int switch_count = 1;
		public static int SwitchCount
		{
			get {return switch_count; }
			set {switch_count = value; }
		}

		private static AudioState current_audio_state1 = AudioState.DTTSP;
		public static AudioState CurrentAudioState1
		{
			get { return current_audio_state1; }
			set { current_audio_state1 = value; }
		}

		private static AudioState next_audio_state1 = AudioState.NOL_NOR;
		public static AudioState NextAudioState1
		{
			get { return next_audio_state1; }
			set { next_audio_state1 = value; }
		}

		private static AudioState save_audio_state1 = AudioState.NOL_NOR;
		public static AudioState SaveAudioState1
		{
			get { return save_audio_state1; }
			set { save_audio_state1 = value; }
		}

		private static double sine_freq1 = 1250.0;
		private static double phase_step1 = sine_freq1/DttSP.SampleRate*2*Math.PI;
		private static double phase_accumulator1 = 0.0;

		private static double sine_freq2 = 1900.0;
		private static double phase_step2 = sine_freq2/DttSP.SampleRate*2*Math.PI;
		private static double phase_accumulator2 = 0.0;

		public static double SineFreq1
		{
			get { return sine_freq1; }
			set
			{
				sine_freq1 = value;
				phase_step1 = sine_freq1/DttSP.SampleRate*2*Math.PI;
			}
		}

		public static double SineFreq2
		{
			get { return sine_freq2; }
			set
			{
				sine_freq2 = value;
				phase_step2 = sine_freq2/DttSP.SampleRate*2*Math.PI;
			}
		}

		public static CW cw_form = null;
		public static Console console = null;
		unsafe private static void *stream1;
		unsafe private static void *stream2;
		private static int block_size2 = 2048;
		public static float[] phase_buf_l;
		public static float[] phase_buf_r;
		public static bool phase = false;
		public static bool wave_record = false;
		public static bool wave_playback = false;
		public static WaveFileWriter wave_file_writer;
		public static WaveFileReader wave_file_reader;
		public static bool two_tone = false;
		private static byte cw_delay = 0;
		public static Mutex phase_mutex = new Mutex();
		public static bool high_pwr_am = false;

		#region VAC Variables

		private static RingBuffer rb_vacIN_l;
		private static RingBuffer rb_vacIN_r;
		private static RingBuffer rb_vacOUT_l;
		private static RingBuffer rb_vacOUT_r;

		private static float[] res_inl;
		private static float[] res_inr;
		private static float[] res_outl;
		private static float[] res_outr;

		unsafe private static void *resampPtrIn_l;
		unsafe private static void *resampPtrIn_r;
		unsafe private static void *resampPtrOut_l;
		unsafe private static void *resampPtrOut_r;

		private static bool vac_resample = false;

		#endregion

		#endregion

		#region Local Copies of External Properties

		private static bool mox = false;
		public static bool MOX
		{
			set { mox = value; }
		}

		private static bool new_keyer = true;
		public static bool NewKeyer
		{
			set { new_keyer = value; }
		}

		private static bool upper_usb_tune = true;
		public static bool UpperSidebandTune 
		{
			set { upper_usb_tune = value; }
			get { return upper_usb_tune; }
		}

		private static bool vac_enabled = false;
		public static bool VACEnabled
		{
			set
			{
				vac_enabled = value;
				if(vac_enabled) InitVAC();
			}
			get { return vac_enabled; }
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

		private static DSPMode dsp_mode = DSPMode.LSB;
		public static DSPMode CurDSPMode
		{
			set { dsp_mode = value; }
		}

		private static int sample_rate1 = 48000;
		public static int SampleRate1
		{
			set
			{
				sample_rate1 = value;
				if(vac_enabled) InitVAC();
			}
			get { return sample_rate1; }
		}

		private static int sample_rate2 = 48000;
		public static int SampleRate2
		{
			set 
			{
				sample_rate2 = value; 
				if(vac_enabled) InitVAC();
			}
			get { return sample_rate2; }
		}

		private static int block_size1 = 1024;
		public static int BlockSize
		{
			set 
			{
				block_size1 = value; 
			}
			get { return block_size1; }
		}

		private static int block_size_vac = 2048;
		public static int BlockSizeVAC
		{
			set 
			{
				block_size_vac = value; 
			}
			get { return block_size_vac; }
		}

		private static bool vac_stereo = false;
		public static bool VACStereo
		{
			set { vac_stereo = value; }
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
		}

		private static int num_channels = 2;
		public static int NumChannels
		{
			set { num_channels = value; }
		}

		private static int host1 = 0;
		public static int Host1
		{
			set { host1 = value; }
		}

		private static int host2 = 0;
		public static int Host2
		{
			set { host2 = value; }
		}

		private static int input_dev1 = 0;
		public static int Input1
		{
			set { input_dev1 = value; }
		}

		private static int input_dev2 = 0;
		public static int Input2
		{
			set { input_dev2 = value; }
		}

		private static int output_dev1 = 0;
		public static int Output1
		{
			set { output_dev1 = value; }
		}

		private static int output_dev2 = 0;
		public static int Output2
		{
			set { output_dev2 = value; }
		}

		private static int latency1 = 0;
		public static int Latency1
		{
			set { latency1 = value; }
		}

		private static int latency2 = 0;
		public static int Latency2
		{
			set { latency2 = value; }
		}

		#endregion

		#region Non-Interleaved Callback Routines
		// ======================================================
		// Non-Interleaved Callback Routines
		// ======================================================
 		//
		// on entry to the callback input and output points to an array of pointers to buffers - one pointer per channel since the data is setup 
		// to be non interleaved.  See: http://www.portaudio.com/docs/proposals/006-NonInterleavedBuffers.html for more info 
		// 


//		public static bool CBtestMode = true; 
//		public static bool oneTimeTest = true; 

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
				phase_mutex.WaitOne();
				Marshal.Copy(new IntPtr(in_l_ptr1), phase_buf_l, 0, frameCount);
				Marshal.Copy(new IntPtr(in_r_ptr1), phase_buf_r, 0, frameCount);
				phase_mutex.ReleaseMutex();
			}

			float* in_l = null, in_r = null, out_l = null, out_r = null;

			in_l = in_l_ptr1;
			in_r = in_r_ptr1;

			out_l = out_l_ptr1;
			out_r = out_r_ptr1;

			switch(current_audio_state1)
			{
				case AudioState.DTTSP:
					if (new_keyer && ((dsp_mode == DSPMode.CWU)
						|| (dsp_mode == DSPMode.CWL)))
					{
						DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
					}

					// scale input with mic preamp
					if(mox && !vac_enabled &&
						(dsp_mode == DSPMode.LSB ||
						dsp_mode == DSPMode.USB ||
						dsp_mode == DSPMode.DSB ||
						dsp_mode == DSPMode.AM  ||
						dsp_mode == DSPMode.SAM ||
						dsp_mode == DSPMode.FMN))
					{
						ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
						ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);
					}

					#region Input Signal Source

				switch(current_input_signal)
				{
					case SignalSource.SOUNDCARD:
						break;
					case SignalSource.SINE:
						SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
						ScaleBuffer(in_l, in_l, frameCount, (float)input_source_scale);
						ScaleBuffer(in_r, in_r, frameCount, (float)input_source_scale);
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

							if(rb_vacIN_l.ReadSpace() >= frameCount) rb_vacIN_l.ReadPtr(in_l,frameCount);
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
						}
					} 
					else DttSP.ExchangeSamples(in_l, in_r, out_l, out_r, frameCount);

#if(MINMAX)
					Debug.WriteLine(MaxSample(out_l, out_r, frameCount));
#endif

					#region Output Signal Source

				switch(current_output_signal)
				{
					case SignalSource.SOUNDCARD:
						break;
					case SignalSource.SINE:
						SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
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
				}

					#endregion

					break;
				case AudioState.CW: 
					if(next_audio_state1 == AudioState.SWITCH) 
					{
						MSCVRT.memset(in_l_ptr1, 0, frameCount*sizeof(float));
						MSCVRT.memset(in_r_ptr1, 0, frameCount*sizeof(float));	
						if (vac_enabled) 
						{
							if((rb_vacIN_l.ReadSpace() >= frameCount)&&(rb_vacIN_r.ReadSpace() >= frameCount))
							{
								rb_vacIN_l.ReadPtr(in_l_ptr1,frameCount);
								rb_vacIN_r.ReadPtr(in_r_ptr1,frameCount);
							}
							else
							{
								VACDebug("rb_vacIN_l underflow 4inTX");
							}
						}

						DttSP.ExchangeSamples(in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);
						if (switch_count == 0) next_audio_state1 = AudioState.CW;
						switch_count--;
					}

					if (new_keyer)  
					{
						DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
					}
					else 
					{
						cw_form.CWGen(out_l_ptr1, out_r_ptr1, frameCount);
					}					
					break;
				case AudioState.SINL_COSR:
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
					break;
				case AudioState.SWITCH:
					ClearBuffer(in_l_ptr1, frameCount);
					ClearBuffer(in_r_ptr1, frameCount);
					if(vac_enabled)
					{
						if((rb_vacIN_l.ReadSpace() >= frameCount)&&(rb_vacIN_r.ReadSpace() >= frameCount))
						{
							rb_vacIN_l.ReadPtr(in_l_ptr1,frameCount);
							rb_vacIN_r.ReadPtr(in_r_ptr1,frameCount);
						}
						else
						{
							VACDebug("rb_vacIN_l underflow 4inTX");
						}
					}

					DttSP.ExchangeSamples(in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);
					ClearBuffer(out_l_ptr1, frameCount);
					ClearBuffer(out_r_ptr1, frameCount);
					if (new_keyer)
					{
						if (next_audio_state1  == AudioState.CW) 
						{
							cw_delay = 1;
							DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
						}  
						else DttSP.CWRingRestart();
					}

					if (switch_count-- == 0) current_audio_state1 = next_audio_state1;
					break;
			}

			double vol = monitor_volume;
			if(mox)
			{
				vol = TXScale;

				if(high_pwr_am)
				{
					if(dsp_mode == DSPMode.AM ||
						dsp_mode == DSPMode.SAM)
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
					if (rb_vacOUT_l.WriteSpace()>=frameCount) rb_vacOUT_l.WritePtr(out_l,frameCount);
					else VACDebug("rb_vacOUT_l overflow");
					if (rb_vacOUT_r.WriteSpace()>=frameCount) rb_vacOUT_r.WritePtr(out_r,frameCount);
					else VACDebug("rb_vacOUT_r overflow");
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
								if (rb_vacOUT_l.WriteSpace() >= outsamps) rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
								else VACDebug("rb_vacOUT_l overflow");
								if (rb_vacOUT_r.WriteSpace() >= outsamps) rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
								else VACDebug("rb_vacOUT_r overflow");
							}
					}
					else 
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))
						{
							int outsamps;
							DttSP.DoResamplerF(out_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
							if(rb_vacOUT_l.WriteSpace() >= outsamps) rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
							else VACDebug("rb_vacOUT_l overflow");
							if(rb_vacOUT_r.WriteSpace() >= outsamps) rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
							else VACDebug("rb_vacOUT_r overflow");
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

		unsafe public static int Callback4Port(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
#if(TIMER)
			t1.Start();
#endif
			float* in_l = null, in_r = null;
			float* out_l1 = null, out_r1 = null, out_l2 = null, out_r2 = null;

			int* array_ptr = (int *)input;
			float* in_l_ptr1 = (float *)array_ptr[0];
			float* in_r_ptr1 = (float *)array_ptr[1];
			float* in_l_ptr2 = (float *)array_ptr[2];
			float* in_r_ptr2 = (float *)array_ptr[3];
			array_ptr = (int *)output;
			float* out_l_ptr1 = (float *)array_ptr[0];
			float* out_r_ptr1 = (float *)array_ptr[1];
			float* out_l_ptr2 = (float *)array_ptr[2];
			float* out_r_ptr2 = (float *)array_ptr[3];

			switch(soundcard)
			{
				case SoundCard.FIREBOX:
					if(!mox)
					{
						in_l = in_l_ptr2;
						in_r = in_r_ptr2;
					}
					else
					{
						in_l = in_l_ptr1;
						in_r = in_r_ptr1;
					}
					break;
				case SoundCard.DELTA_44:
				default:
					if(!mox)
					{
						in_l = in_l_ptr1;
						in_r = in_r_ptr1;
					}
					else
					{
						in_l = in_l_ptr2;
						in_r = in_r_ptr2;
					}
					break;
			}

			if(wave_playback)
				wave_file_reader.GetPlayBuffer(in_l, in_r);
			if(wave_record)
			{
				if(!mox)
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
				phase_mutex.WaitOne();
				Marshal.Copy(new IntPtr(in_l), phase_buf_l, 0, frameCount);
				Marshal.Copy(new IntPtr(in_r), phase_buf_r, 0, frameCount);
				phase_mutex.ReleaseMutex();
			}

			switch(current_audio_state1)
			{
				case AudioState.DTTSP:
					
					#region VOX
					if(vox_enabled)
					{
						float* vox_l = null, vox_r = null;
						switch(soundcard)
						{
							case SoundCard.FIREBOX:
								vox_l = in_l_ptr1;
								vox_r = in_r_ptr1;
								break;
							case SoundCard.DELTA_44:
							default:
								vox_l = in_l_ptr2;
								vox_r = in_r_ptr2;
								break;
						}

						if(dsp_mode == DSPMode.LSB ||
							dsp_mode == DSPMode.USB ||
							dsp_mode == DSPMode.DSB ||
							dsp_mode == DSPMode.AM  ||
							dsp_mode == DSPMode.SAM ||
							dsp_mode == DSPMode.FMN)
						{
							double peak = MaxSample(vox_l, vox_r, frameCount);

							// compare power to threshold
							if(peak > vox_threshold) 
								vox_active = true;
							else 
								vox_active = false;
						}
					}
					#endregion

					if (new_keyer && ((dsp_mode == DSPMode.CWU)
						|| (dsp_mode == DSPMode.CWL)))
					{
						DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
					} 

					// scale input with mic preamp
					if(mox && !vac_enabled &&
						(dsp_mode == DSPMode.LSB ||
						dsp_mode == DSPMode.USB ||
						dsp_mode == DSPMode.DSB ||
						dsp_mode == DSPMode.AM  ||
						dsp_mode == DSPMode.SAM ||
						dsp_mode == DSPMode.FMN))
					{
						ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
						ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);
					}

					#region Input Signal Source

				switch(current_input_signal)
				{
					case SignalSource.SOUNDCARD:
						break;
					case SignalSource.SINE:
						SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
						ScaleBuffer(in_l, in_l, frameCount, (float)input_source_scale);
						ScaleBuffer(in_r, in_r, frameCount, (float)input_source_scale);
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
				}

					#endregion

#if(MINMAX)
					Debug.Write(MaxSample(in_l, in_r, frameCount).ToString("f6")+",");
#endif

					if(vac_enabled &&
						rb_vacIN_l != null && rb_vacIN_r != null && 
						rb_vacOUT_l != null && rb_vacOUT_r != null)
					{
						if(mox) // transmit mode
						{
							if(rb_vacIN_l.ReadSpace() >= frameCount) rb_vacIN_l.ReadPtr(in_l,frameCount);
							else
							{
								ClearBuffer(in_l,frameCount);
								VACDebug("rb_vacIN_l underflow 4inTX");
							}
							if(rb_vacIN_r.ReadSpace() >= frameCount) rb_vacIN_r.ReadPtr(in_r,frameCount);
							else
							{
								ClearBuffer(in_r,frameCount);
								VACDebug("rb_vacIN_r underflow 4inTX");
							}
							ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
							ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);						
							DttSP.ExchangeSamples(in_l, in_r, out_l_ptr1, out_r_ptr1, frameCount);
						}
						else // receive mode
						{
							DttSP.ExchangeSamples(in_l, in_r, out_l_ptr1, out_r_ptr1, frameCount);

							if ((rb_vacIN_l.ReadSpace() >= frameCount)&&(rb_vacIN_r.ReadSpace() >= frameCount))
							{
								rb_vacIN_l.ReadPtr(in_l,frameCount);
								rb_vacIN_r.ReadPtr(in_r,frameCount);
							}
							else
							{
								ClearBuffer(in_l,frameCount);
								ClearBuffer(in_r,frameCount);
								VACDebug("rb_vacIN_l underflow 4inRX");
							}
						}
					} 
					else DttSP.ExchangeSamples(in_l, in_r, out_l_ptr1, out_r_ptr1, frameCount);

#if(MINMAX)
					Debug.Write(MaxSample(out_l_ptr1, out_r_ptr1, frameCount).ToString("f6")+",");
#endif

					#region Output Signal Source

				switch(current_output_signal)
				{
					case SignalSource.SOUNDCARD:
						break;
					case SignalSource.SINE:
						SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
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
				}

					#endregion

					break;
				case AudioState.CW: 
					if(next_audio_state1 == AudioState.SWITCH)
					{
						ClearBuffer(in_l_ptr1, frameCount);
						ClearBuffer(in_r_ptr1, frameCount);
						if (vac_enabled) 
						{
							if((rb_vacIN_l.ReadSpace() >= frameCount)&&(rb_vacIN_r.ReadSpace() >= frameCount))
							{
								rb_vacIN_l.ReadPtr(in_l_ptr1,frameCount);
								rb_vacIN_r.ReadPtr(in_r_ptr1,frameCount);
							}
							else
							{
								VACDebug("rb_vacIN_l underflow 4inTX");
							}
						}
						DttSP.ExchangeSamples(in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);
						if (switch_count == 0) next_audio_state1 = AudioState.CW;
						switch_count--;
					}

					if (new_keyer)  
					{
						DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
						if (cw_delay == 1) cw_delay = 0;
					}
					else 
					{
						cw_form.CWGen(out_l_ptr1, out_r_ptr1, frameCount);
					}
					break;
				case AudioState.SINL_COSR:
					out_l1 = out_l_ptr1;
					out_r1 = out_r_ptr1;
					out_l2 = out_l_ptr2;
					out_r2 = out_r_ptr2;

					if(two_tone)
					{
						double dump;
						if (upper_usb_tune) SineWave2Tone(out_l1, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out dump, out dump);
						else SineWave2Tone(out_l1, frameCount,
								 phase_accumulator1, phase_accumulator2,
								 -sine_freq2, -sine_freq1,
								 out dump, out dump);

						CosineWave2Tone(out_r1, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2);
					}
					else
					{
						if (upper_usb_tune) SineWave(out_l1, frameCount, phase_accumulator1, sine_freq1);
						else SineWave(out_l1, frameCount, phase_accumulator1, -sine_freq1);
						phase_accumulator1 = CosineWave(out_r1, frameCount, phase_accumulator1, sine_freq1);
					}
					break;					
				case AudioState.SINL_SINR:
					out_l1 = out_l_ptr1;
					out_r1 = out_r_ptr1;
					out_l2 = out_l_ptr2;
					out_r2 = out_r_ptr2;

					if(two_tone)
					{
						SineWave2Tone(out_l1, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2);
						CopyBuffer(out_l1, out_r1, frameCount);							
					}
					else
					{
						phase_accumulator1 = SineWave(out_l1, frameCount, phase_accumulator1, sine_freq1);
						CopyBuffer(out_l1, out_r1, frameCount);
					}
					break;
				case AudioState.SINL_NOR:
					out_l1 = out_l_ptr1;
					out_r1 = out_r_ptr1;
					out_l2 = out_l_ptr2;
					out_r2 = out_r_ptr2;

					if(two_tone)
					{
						SineWave2Tone(out_l1, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2);
						ClearBuffer(out_r1, frameCount);		
					}
					else
					{
						phase_accumulator1 = SineWave(out_l1, frameCount, phase_accumulator1, sine_freq1);
						ClearBuffer(out_r1, frameCount);
					}
					break;
				case AudioState.NOL_SINR:
					out_l1 = out_l_ptr1;
					out_r1 = out_r_ptr1;
					out_l2 = out_l_ptr2;
					out_r2 = out_r_ptr2;

					if(two_tone)
					{
						ClearBuffer(out_l1, frameCount);
						SineWave2Tone(out_r1, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2);
					}
					else
					{
						ClearBuffer(out_l1, frameCount);
						phase_accumulator1 = SineWave(out_r1, frameCount, phase_accumulator1, sine_freq1);
					}
					break;
				case AudioState.NOL_NOR:
					ClearBuffer(out_l_ptr1, frameCount);
					ClearBuffer(out_r_ptr1, frameCount);
					break;
				case AudioState.PIPE:
					CopyBuffer(in_l_ptr1, out_l_ptr1, frameCount);
					CopyBuffer(in_r_ptr1, out_r_ptr1, frameCount);
					break;
				case AudioState.SWITCH:
					ClearBuffer(in_l_ptr1, frameCount);
					ClearBuffer(in_r_ptr1, frameCount);
					if(vac_enabled)
					{
						if((rb_vacIN_l.ReadSpace() >= frameCount)&&(rb_vacIN_r.ReadSpace() >= frameCount))
						{
							rb_vacIN_l.ReadPtr(in_l_ptr1,frameCount);
							rb_vacIN_r.ReadPtr(in_r_ptr1,frameCount);
						}
						else
						{
							VACDebug("rb_vacIN_l underflow 4inTX");
						}
					}
					DttSP.ExchangeSamples(in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);
					ClearBuffer(out_l_ptr1, frameCount);
					ClearBuffer(out_r_ptr1, frameCount);
					if (new_keyer)
					{
						if (next_audio_state1 == AudioState.CW) 
						{
							cw_delay = 1;
							DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
						}  
						else DttSP.CWRingRestart();
					}
					if (switch_count-- == 0)
						current_audio_state1 = next_audio_state1;
					break;
			}

			out_l1 = out_l_ptr1;
			out_r1 = out_r_ptr1;
			out_l2 = out_l_ptr2;
			out_r2 = out_r_ptr2;

			// scale output for VAC
			if(vac_enabled &&
				rb_vacIN_l != null && rb_vacIN_r != null && 
				rb_vacOUT_l != null && rb_vacOUT_r != null)
			{
				ScaleBuffer(out_l1, out_l2, frameCount, (float)vac_rx_scale);
				ScaleBuffer(out_r1, out_r2, frameCount, (float)vac_rx_scale);

				if (sample_rate2 == sample_rate1) 
				{
					if ((rb_vacOUT_l.WriteSpace()>=frameCount)&&(rb_vacOUT_r.WriteSpace()>=frameCount))
					{
						rb_vacOUT_l.WritePtr(out_l2,frameCount);
						rb_vacOUT_r.WritePtr(out_r2,frameCount);
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
								DttSP.DoResamplerF(out_l2,res_outl_ptr,frameCount,&outsamps, resampPtrOut_l);
								DttSP.DoResamplerF(out_r2,res_outr_ptr,frameCount,&outsamps, resampPtrOut_r);
								if ((rb_vacOUT_l.WriteSpace()>=outsamps)&&(rb_vacOUT_r.WriteSpace()>=outsamps))
								{
									rb_vacOUT_l.WritePtr(res_outl_ptr,outsamps);
									rb_vacOUT_r.WritePtr(res_outr_ptr,outsamps);
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
							DttSP.DoResamplerF(out_l2,res_outl_ptr,frameCount,&outsamps, resampPtrOut_l);
							if ((rb_vacOUT_l.WriteSpace() >= outsamps)&&(rb_vacOUT_r.WriteSpace() >= outsamps))
							{
								rb_vacOUT_l.WritePtr(res_outl_ptr,outsamps);
								rb_vacOUT_r.WritePtr(res_outl_ptr,outsamps);
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
			if(!mox)
			{
				ScaleBuffer(out_l1, out_l1, frameCount, (float)monitor_volume);
				ScaleBuffer(out_r1, out_r1, frameCount, (float)monitor_volume);
				// VAC handling for third party program


				ClearBuffer(out_l2, frameCount);
				ClearBuffer(out_r2, frameCount);
			}
			else
			{
				double tx_vol = TXScale;
				if(high_pwr_am)
				{
					if(dsp_mode == DSPMode.AM ||
						dsp_mode == DSPMode.SAM)
						tx_vol *= 1.414;
				}

				ScaleBuffer(out_l1, out_l2, frameCount, (float)tx_vol);
				ScaleBuffer(out_l1, out_l1, frameCount, (float)monitor_volume);
				ScaleBuffer(out_r1, out_r2, frameCount, (float)tx_vol);
				ScaleBuffer(out_r1, out_r1, frameCount, (float)monitor_volume);

				
				// clip radio output to prevent overdrive
				float clip_thresh = (float)(1.5f / console.AudioVolts1);
				for(int i=0; i<frameCount; i++)
				{
					if(out_l2[i] > clip_thresh)
					{
						Debug.WriteLine("Clip Left High: "+out_l2[i].ToString("f5"));
						out_l2[i] = clip_thresh;
					}
					else if(out_l2[i] < -clip_thresh)
					{
						Debug.WriteLine("Clip Left Low: "+out_l2[i].ToString("f5"));
						out_l2[i] = -clip_thresh;
					}

					if(out_r2[i] > clip_thresh) 
					{
						Debug.WriteLine("Clip Right High: "+out_l2[i].ToString("f5"));
						out_r2[i] = clip_thresh;							
					}
					else if(out_r2[i] < -clip_thresh)
					{
						Debug.WriteLine("Clip Right Low: "+out_l2[i].ToString("f5"));
						out_r2[i] = -clip_thresh;
					}
				}

				if(soundcard == SoundCard.FIREBOX)
				{
					// scale FireBox monitor output to prevent overdrive
					ScaleBuffer(out_l1, out_l1, frameCount, (float)(1.5f / console.AudioVolts1));
					ScaleBuffer(out_r1, out_r1, frameCount, (float)(1.5f / console.AudioVolts1));
				}
			}

#if(MINMAX)
			Debug.Write(MaxSample(out_l2, out_r2, frameCount).ToString("f6")+",");

			float current_max = MaxSample(out_l2, out_r2, frameCount);
			if(current_max > max) max = current_max;
			Debug.WriteLine(" max: "+max.ToString("f6"));
#endif

			if(wave_record)
			{
				if(!mox)
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
						wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);
					}
				}
			}
#if(TIMER)
			t1.Stop();
			Debug.WriteLine(t1.Duration);
#endif
			return callback_return;
		}


#if(MINMAX)
		private static float max = float.MinValue;
#endif

		unsafe public static int CallbackVAC(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
			int* array_ptr = (int *)input;
			float* in_l_ptr1 = (float *)array_ptr[0];
			float* in_r_ptr1 = null;
			if(vac_stereo) in_r_ptr1 = (float *)array_ptr[1];
			array_ptr = (int *)output;
			float* out_l_ptr1 = (float *)array_ptr[0];
			float* out_r_ptr1 = null;
			if(vac_stereo) out_r_ptr1 = (float *)array_ptr[1];

			if (vac_rb_reset)
			{
				vac_rb_reset = false;
				ClearBuffer(out_l_ptr1,frameCount);
				if (vac_stereo) ClearBuffer(out_r_ptr1,frameCount);
				rb_vacIN_l.Restart((block_size1*Audio.SampleRate1)/(block_size_vac*Audio.SampleRate2));
				rb_vacIN_r.Restart((block_size1*Audio.SampleRate1)/(block_size_vac*Audio.SampleRate2));
				rb_vacOUT_l.Restart(block_size_vac*2);
				rb_vacOUT_r.Restart(block_size_vac*2);
				int tmp = 2*block_size_vac;
				Debug.WriteLine(tmp.ToString());
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
							DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr,frameCount,&outsamps, resampPtrIn_l);
							DttSP.DoResamplerF(in_r_ptr1, res_inr_ptr,frameCount,&outsamps, resampPtrIn_r);
							if ((rb_vacIN_l.WriteSpace() >= outsamps)&&(rb_vacIN_r.WriteSpace() >= outsamps))
							{
								rb_vacIN_l.WritePtr(res_inl_ptr,outsamps);
								rb_vacIN_r.WritePtr(res_inr_ptr,outsamps);
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
						rb_vacIN_l.WritePtr(in_l_ptr1,frameCount);
						rb_vacIN_r.WritePtr(in_r_ptr1,frameCount);
					}
					else
					{
						vac_rb_reset = true;
                        VACDebug("rb_vacIN overflow mono CBvac");
					}
				}
				
				if ((rb_vacOUT_l.ReadSpace() >= frameCount)&&(rb_vacOUT_r.ReadSpace() >= frameCount))
				{
					rb_vacOUT_l.ReadPtr(out_l_ptr1,frameCount);
					rb_vacOUT_r.ReadPtr(out_r_ptr1,frameCount);
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
							rb_vacIN_l.WritePtr(res_inl_ptr,outsamps);
							rb_vacIN_r.WritePtr(res_inl_ptr,outsamps);
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
						rb_vacIN_l.WritePtr(in_l_ptr1,frameCount);
						rb_vacIN_r.WritePtr(in_l_ptr1,frameCount);
					}
					else
					{
						//vac_rb_reset = true;
						VACDebug("rb_vacIN_l overflow");
					}
				}
				if ((rb_vacOUT_l.ReadSpace() >= frameCount)&&(rb_vacOUT_r.ReadSpace() >= frameCount))
				{
					rb_vacOUT_l.ReadPtr(out_l_ptr1,frameCount);
					rb_vacOUT_r.ReadPtr(out_l_ptr1,frameCount);
				}
				else 
				{
					ClearBuffer(out_l_ptr1,frameCount);
					VACDebug("rb_vacOUT_l underflow");
				}
			}

			return 0;
		}

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

		#region Interleaved Callback Routines
		// ======================================================
		// Interleaved Callback Routines
		// ======================================================

#if(INTERLEAVED)
		private static float[] i_l1 = new float[2048];
		private static float[] i_r1 = new float[2048];
		private static float[] o_l1 = new float[2048];
		private static float[] o_r1 = new float[2048];
		private static float[] i_l2 = new float[2048];
		private static float[] i_r2 = new float[2048];
		private static float[] o_l2 = new float[2048];
		private static float[] o_r2 = new float[2048];

		unsafe public static int Callback1IL(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
#if(TIMER)
			t1.Start();
#endif

			float* in_buf = (float *)input;
			float* out_buf = (float *)output;
		/*
			if(wave_playback)
				wave_file_reader.GetPlayBuffer(in_l_ptr1, in_r_ptr1);
			if((wave_record && !mox && record_rx_preprocessed) ||
				(wave_record && mox && record_tx_preprocessed))
				wave_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1);			
*/ //FIXME: need to add IL functions to the wave file stuff

			switch(current_audio_state1)
			{
				case AudioState.DTTSP:
					if (new_keyer && ((dsp_mode == DSPMode.CWU)
						|| (dsp_mode == DSPMode.CWL)))
					{
						DttSP.CWtoneExchangeIL(out_buf, frameCount,2); //FIXME: Add new IL version of CWtoneExchange
					}

					// scale input with mic preamp
					if(mox && !vac_enabled &&
						(dsp_mode == DSPMode.LSB ||
						dsp_mode == DSPMode.USB ||
						dsp_mode == DSPMode.DSB ||
						dsp_mode == DSPMode.AM  ||
						dsp_mode == DSPMode.SAM ||
						dsp_mode == DSPMode.FMN))
					{
						ScaleBufferIL(in_buf, in_buf, frameCount, (float)mic_preamp, 0, 0, 2);
						ScaleBufferIL(in_buf, in_buf, frameCount, (float)mic_preamp, 1, 1, 2);
					}

					#region Input Signal Source

				switch(current_input_signal)
				{
					case SignalSource.SOUNDCARD:
						break;
					case SignalSource.SINE:
						SineWaveIL(in_buf, frameCount, phase_accumulator1, sine_freq1, 0, 2);
						phase_accumulator1 = CosineWaveIL(in_buf, frameCount, phase_accumulator1, sine_freq1, 1, 2);
						ScaleBufferIL(in_buf, in_buf, frameCount, (float)input_source_scale, 0, 0, 2);
						ScaleBufferIL(in_buf, in_buf, frameCount, (float)input_source_scale, 1, 1, 2);
						break;
					case SignalSource.NOISE:
						NoiseIL(in_buf, frameCount, 0, 2);
						NoiseIL(in_buf, frameCount, 1, 2);
						break;
					case SignalSource.TRIANGLE:
						TriangleIL(in_buf, frameCount, sine_freq1, 0, 2);
						CopyBufferIL(in_buf, in_buf, frameCount, 0, 1, 2);
						break;
					case SignalSource.SAWTOOTH:
						SawtoothIL(in_buf, frameCount, sine_freq1, 0, 2);
						CopyBufferIL(in_buf, in_buf, frameCount, 0, 1, 2);
						break;
				}

					#endregion

#if(MINMAX)
					Debug.Write(MaxSampleIL(in_buf, frameCount, 2).ToString("f6")+",");
#endif
/*
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
							DttSP.ExchangeSamplesIL(in_buf,out_buf, frameCount);
						}
						else
						{
							DttSP.ExchangeSamplesIL(in_buf,out_buf, frameCount);

							if(rb_vacIN_l.ReadSpace() >= frameCount) rb_vacIN_l.ReadPtr(in_l,frameCount);
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
						}
					} 
					else*/ DttSP.ExchangeSamplesIL(in_buf, out_buf, frameCount); //FIXME: Add IL version of ExchangeSamples

#if(MINMAX)
					Debug.WriteLine(MaxSampleIL(out_buf, frameCount, 2));
#endif

					#region Output Signal Source

				switch(current_output_signal)
				{
					case SignalSource.SOUNDCARD:
						break;
					case SignalSource.SINE:
						SineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 0, 2);
						phase_accumulator1 = CosineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 1, 2);
						break;
					case SignalSource.NOISE:
						NoiseIL(out_buf, frameCount, 0, 2);
						NoiseIL(out_buf, frameCount, 1, 2);
						break;
					case SignalSource.TRIANGLE:
						TriangleIL(out_buf, frameCount, sine_freq1, 0, 2);
						CopyBufferIL(out_buf, out_buf, frameCount, 0, 1, 2);
						break;
					case SignalSource.SAWTOOTH:
						SawtoothIL(out_buf, frameCount, sine_freq1, 0, 2);
						CopyBufferIL(out_buf, out_buf, frameCount, 0, 1, 2);
						break;
				}

					#endregion

					break;
				case AudioState.CW: 
					if(next_audio_state1 == AudioState.SWITCH) 
					{
						MSCVRT.memset(in_buf, 0, frameCount*sizeof(float)*2);						
						DttSP.ExchangeSamplesIL(in_buf, out_buf, frameCount); //FIXME: add IL version of ExchangeSamples
						if (switch_count == 0) next_audio_state1 = AudioState.CW;
						switch_count--;
					}

					if (new_keyer)  
					{
						DttSP.CWtoneExchangeIL(out_buf, frameCount,2); //FIXME: Add IL version of CWtoneExchange
					}
					else 
					{
						//cw_form.CWGen(out_l_ptr1, out_r_ptr1, frameCount); FIXME: Add IL version of CWGen
					}					
					break;
				case AudioState.SINL_COSR:
					if(two_tone)
					{
						double dump;
						SineWave2ToneIL(out_buf, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out dump, out dump, 0, 2);

						CosineWave2ToneIL(out_buf, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2, 1, 2);
					}
					else
					{
						SineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 0, 2);
						phase_accumulator1 = CosineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 1, 2);
					}
					break;					
				case AudioState.SINL_SINR:
					if(two_tone)
					{
						SineWave2ToneIL(out_buf, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2, 0, 2);

						CopyBufferIL(out_buf, out_buf, frameCount, 0, 1, 2);
					}
					else
					{
						phase_accumulator1 = SineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 0, 2);
						CopyBufferIL(out_buf, out_buf, frameCount, 0, 1, 2);
					}
					break;
				case AudioState.SINL_NOR:
					if(two_tone)
					{
						SineWave2ToneIL(out_buf, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2, 0, 2);
						ClearBufferIL(out_buf, frameCount, 1, 2);
					}
					else
					{
						phase_accumulator1 = SineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 0, 2);
						ClearBufferIL(out_buf, frameCount, 1, 2);
					}
					break;
				case AudioState.NOL_SINR:
					if(two_tone)
					{
						ClearBufferIL(out_buf, frameCount, 0, 2);
						SineWave2ToneIL(out_buf, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2, 1, 2);						
					}
					else
					{
						ClearBufferIL(out_buf, frameCount, 0, 2);
						phase_accumulator1 = SineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 1, 2);
					}
					break;
				case AudioState.NOL_NOR:
					ClearBufferIL(out_buf, frameCount, 0, 2);
					ClearBufferIL(out_buf, frameCount, 1, 2);
					break;
				case AudioState.PIPE:
					CopyBufferIL(in_buf, out_buf, frameCount, 0, 0, 2);
					CopyBufferIL(in_buf, out_buf, frameCount, 1, 1, 2);
					break;
				case AudioState.SWITCH:
					ClearBufferIL(in_buf, frameCount, 0, 2);
					ClearBufferIL(in_buf, frameCount, 1, 2);
					DttSP.ExchangeSamplesIL(in_buf, out_buf, frameCount); //FIXME: add IL version of ExchangeSamples
					ClearBufferIL(out_buf, frameCount, 0, 2);
					ClearBufferIL(out_buf, frameCount, 1, 2);
					if (new_keyer)
					{
						if (next_audio_state1  == AudioState.CW) 
						{
							cw_delay = 1;
							DttSP.CWtoneExchangeIL(out_buf, frameCount, 2); //FIXME: add IL version of CWtoneExchange
						}  
						else DttSP.CWRingRestart();
					}

					if (switch_count-- == 0) current_audio_state1 = next_audio_state1;
					break;
			}

			double vol = monitor_volume;
			if(mox)
			{
				vol = TXScale;

				if(high_pwr_am)
				{
					if(dsp_mode == DSPMode.AM ||
						dsp_mode == DSPMode.SAM)
						vol *= 1.414;
				}
			}

			ScaleBufferIL(out_buf, out_buf, frameCount, (float)vol, 0, 0, 2);
			ScaleBufferIL(out_buf, out_buf, frameCount, (float)vol, 1, 1, 2);

			/*
			if(vac_enabled &&
				rb_vacIN_l != null && rb_vacIN_r != null && 
				rb_vacOUT_l != null && rb_vacOUT_r != null)
			{
				if (sample_rate2 == sample_rate1) 
				{
					if (rb_vacOUT_l.WriteSpace()>=frameCount) rb_vacOUT_l.WritePtr(out_l,frameCount);
					else VACDebug("rb_vacOUT_l overflow");
					if (rb_vacOUT_r.WriteSpace()>=frameCount) rb_vacOUT_r.WritePtr(out_r,frameCount);
					else VACDebug("rb_vacOUT_r overflow");
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
								if (rb_vacOUT_l.WriteSpace() >= outsamps) rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
								else VACDebug("rb_vacOUT_l overflow");
								if (rb_vacOUT_r.WriteSpace() >= outsamps) rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
								else VACDebug("rb_vacOUT_r overflow");
							}
					}
					else 
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))
						{
							int outsamps;
							DttSP.DoResamplerF(out_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
							if(rb_vacOUT_l.WriteSpace() >= outsamps) rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
							else VACDebug("rb_vacOUT_l overflow");
							if(rb_vacOUT_r.WriteSpace() >= outsamps) rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
							else VACDebug("rb_vacOUT_r overflow");
						}
					}
				}
			}*/

#if(MINMAX)
			Debug.Write(MaxSampleIL(out_buf, frameCount, 2).ToString("f6")+",");

			float current_max = MaxSampleIL(out_buf, frameCount, 2);
			if(current_max > max) max = current_max;
			Debug.WriteLine(" max: "+max.ToString("f6"));
#endif

			/*if((wave_record && !mox && !record_rx_preprocessed) ||
				(wave_record && mox && !record_tx_preprocessed))
				wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);	*/
#if(TIMER)
			t1.Stop();
			Debug.WriteLine(t1.Duration);
#endif
			return callback_return;			
		}

		unsafe public static int Callback4PortIL(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
#if(TIMER)
			t1.Start();
#endif
			float* in_buf = (float*)input;
			float* out_buf = (float*)output;
			
			int in_l = 0, in_r = 1;
			switch(soundcard)
			{
				case SoundCard.FIREBOX:
					if(!mox)
					{
						in_l = 2;
						in_r = 3;
					}
					else
					{
						in_l = 0;
						in_r = 1;
					}
					break;
				case SoundCard.DELTA_44:
				default:
					if(!mox)
					{
						in_l = 0;
						in_r = 1;
					}
					else
					{
						in_l = 2;
						in_r = 3;
					}
					break;
			}
			/*
						if(wave_playback)
							wave_file_reader.GetPlayBuffer(in_buf);

						if(wave_record)
						{
							if(!mox)
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
			*/

			switch(current_audio_state1)
			{
				case AudioState.DTTSP:
					
					#region VOX
					if(vox_enabled)
					{
						int vox_l = 0, vox_r = 1;
						switch(soundcard)
						{
							case SoundCard.FIREBOX:
								vox_l = 0;
								vox_r = 1;
								break;
							case SoundCard.DELTA_44:
							default:
								vox_l = 2;
								vox_r = 3;
								break;
						}

						if(dsp_mode == DSPMode.LSB ||
							dsp_mode == DSPMode.USB ||
							dsp_mode == DSPMode.DSB ||
							dsp_mode == DSPMode.AM  ||
							dsp_mode == DSPMode.SAM ||
							dsp_mode == DSPMode.FMN)
						{
							double peak = MaxSampleIL(in_buf, frameCount, vox_l, 4);

							// compare power to threshold
							if(peak > vox_threshold) 
								vox_active = true;
							else 
								vox_active = false;
						}
					}
					#endregion

					if (new_keyer && ((dsp_mode == DSPMode.CWU)
						|| (dsp_mode == DSPMode.CWL)))
					{
						DttSP.CWtoneExchangeIL(out_buf, frameCount,4); //FIXME: Add IL version of CWtoneExchange
					} 

					// scale input with mic preamp
					if(mox && !vac_enabled &&
						(dsp_mode == DSPMode.LSB ||
						dsp_mode == DSPMode.USB ||
						dsp_mode == DSPMode.DSB ||
						dsp_mode == DSPMode.AM  ||
						dsp_mode == DSPMode.SAM ||
						dsp_mode == DSPMode.FMN))
					{
						ScaleBufferIL(in_buf, in_buf, frameCount, (float)mic_preamp, in_l, in_l, 4);
						ScaleBufferIL(in_buf, in_buf, frameCount, (float)mic_preamp, in_r, in_r, 4);
					}

					#region Input Signal Source

				switch(current_input_signal)
				{
					case SignalSource.SOUNDCARD:
						break;
					case SignalSource.SINE:
						SineWaveIL(in_buf, frameCount, phase_accumulator1, sine_freq1, in_l, 4);
						phase_accumulator1 = CosineWaveIL(in_buf, frameCount, phase_accumulator1, sine_freq1, in_r, 4);
						ScaleBufferIL(in_buf, in_buf, frameCount, (float)input_source_scale, in_l, in_l, 4);
						ScaleBufferIL(in_buf, in_buf, frameCount, (float)input_source_scale, in_r, in_r, 4);
						break;
					case SignalSource.NOISE:
						NoiseIL(in_buf, frameCount, in_l, 4);
						NoiseIL(in_buf, frameCount, in_r, 4);
						break;
					case SignalSource.TRIANGLE:
						TriangleIL(in_buf, frameCount, sine_freq1, in_l, 4);
						CopyBufferIL(in_buf, in_buf, frameCount, in_l, in_r, 4);
						break;
					case SignalSource.SAWTOOTH:
						SawtoothIL(in_buf, frameCount, sine_freq1, in_l, 4);
						CopyBufferIL(in_buf, in_buf, frameCount, in_l, in_r, 4);
						break;
				}

					#endregion

#if(MINMAX)
					Debug.Write(Math.Max(MaxSampleIL(in_buf, frameCount, in_l, 4),
						MaxSampleIL(in_buf, frameCount, in_r, 4)).ToString("f6")+",");
#endif

					/*if(vac_enabled &&
						rb_vacIN_l != null && rb_vacIN_r != null && 
						rb_vacOUT_l != null && rb_vacOUT_r != null)
					{
						if(mox) // transmit mode
						{
							if(rb_vacIN_l.ReadSpace() >= frameCount) rb_vacIN_l.ReadPtr(in_l,frameCount);
							else
							{
								ClearBuffer(in_l,frameCount);
								VACDebug("rb_vacIN_l underflow");
							}
							if(rb_vacIN_r.ReadSpace() >= frameCount) rb_vacIN_r.ReadPtr(in_r,frameCount);
							else
							{
								ClearBuffer(in_r,frameCount);
								VACDebug("rb_vacIN_r underflow");
							}
							ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
							ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);						
							DttSP.ExchangeSamples4IL(in_buf, out_buf, frameCount);
						}
						else // receive mode
						{
							DttSP.ExchangeSamples4IL(in_buf,, out_buf, frameCount);

							if(rb_vacIN_l.ReadSpace() >= frameCount) rb_vacIN_l.ReadPtr(in_l,frameCount);
							else
							{
								ClearBuffer(in_l,frameCount);
								VACDebug("rb_vacIN_l underflow");
							}
							if(rb_vacIN_r.ReadSpace() >= frameCount) rb_vacIN_r.ReadPtr(in_r,frameCount);
							else
							{
								ClearBuffer(in_r,frameCount);
								VACDebug("rb_vacIN_r underflow");
							}
						}
					} 
					else*/ DttSP.ExchangeSamples4IL(in_buf, out_buf, frameCount); //FIXME: add IL version of ExchangeSamples

#if(MINMAX)
					Debug.Write(Math.Max(MaxSampleIL(out_buf, frameCount, 0, 4),
						MaxSampleIL(out_buf, frameCount, 1, 4)).ToString("f6")+",");
#endif

					#region Output Signal Source

				switch(current_output_signal)
				{
					case SignalSource.SOUNDCARD:
						break;
					case SignalSource.SINE:
						SineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 0, 4);
						phase_accumulator1 = CosineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 1, 4);
						break;
					case SignalSource.NOISE:
						NoiseIL(out_buf, frameCount, 0, 4);
						NoiseIL(out_buf, frameCount, 1, 4);
						break;
					case SignalSource.TRIANGLE:
						TriangleIL(out_buf, frameCount, sine_freq1, 0, 4);
						CopyBufferIL(out_buf, out_buf, frameCount, 0, 1, 4);
						break;
					case SignalSource.SAWTOOTH:
						SawtoothIL(out_buf, frameCount, sine_freq1, 0, 4);
						CopyBufferIL(out_buf, out_buf, frameCount, 0, 1, 4);
						break;
				}

					#endregion

					break;
				case AudioState.CW: 
					if(next_audio_state1 == AudioState.SWITCH)
					{
						ClearBufferIL(in_buf, frameCount, in_l, 4);
						ClearBufferIL(in_buf, frameCount, in_r, 4);
						DttSP.ExchangeSamples4IL(in_buf, out_buf, frameCount); //FIXME: Add IL version of ExchangeSamples
						if (switch_count == 0) next_audio_state1 = AudioState.CW;
						switch_count--;
					}

					if (new_keyer)  
					{
						DttSP.CWtoneExchangeIL(out_buf, frameCount,4); //FIXME: add IL version of CWtoneExchange
						if (cw_delay == 1) cw_delay = 0;
					}
					else 
					{
						//cw_form.CWGen(out_l_ptr1, out_r_ptr1, frameCount); FIXME: add IL version of CWGen
					}
					break;
				case AudioState.SINL_COSR:
					if(two_tone)
					{
						double dump;
						SineWave2ToneIL(out_buf, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out dump, out dump, 0, 4);

						CosineWave2ToneIL(out_buf, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2, 1, 4);
					}
					else
					{
						SineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 0, 4);
						phase_accumulator1 = CosineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 1, 4);
					}
					break;					
				case AudioState.SINL_SINR:
					if(two_tone)
					{
						SineWave2ToneIL(out_buf, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2, 0, 4);
						CopyBufferIL(out_buf, out_buf, frameCount, 0, 1, 4);		
					}
					else
					{
						phase_accumulator1 = SineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 0, 4);
						CopyBufferIL(out_buf, out_buf, frameCount, 0, 1, 4);
					}
					break;
				case AudioState.SINL_NOR:
					if(two_tone)
					{
						SineWave2ToneIL(out_buf, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2, 0, 4);
						ClearBufferIL(out_buf, frameCount, 1, 4);		
					}
					else
					{
						phase_accumulator1 = SineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 0, 4);
						ClearBufferIL(out_buf, frameCount, 1, 4);
					}
					break;
				case AudioState.NOL_SINR:
					if(two_tone)
					{
						ClearBufferIL(out_buf, frameCount, 0, 4);
						SineWave2ToneIL(out_buf, frameCount,
							phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2,
							out phase_accumulator1, out phase_accumulator2, 1, 4);
					}
					else
					{
						ClearBufferIL(out_buf, frameCount, 0, 4);
						phase_accumulator1 = SineWaveIL(out_buf, frameCount, phase_accumulator1, sine_freq1, 1, 4);
					}
					break;
				case AudioState.NOL_NOR:
					ClearBufferIL(out_buf, frameCount, 0, 4);
					ClearBufferIL(out_buf, frameCount, 1, 4);
					break;
				case AudioState.PIPE:
					CopyBufferIL(in_buf, out_buf, frameCount, in_l, 0, 4);
					CopyBufferIL(in_buf, out_buf, frameCount, in_r, 1, 4);
					break;
				case AudioState.SWITCH:
					ClearBufferIL(in_buf, frameCount, 0, 4);
					ClearBufferIL(in_buf, frameCount, 1, 4);
					DttSP.ExchangeSamples4IL(in_buf, out_buf, frameCount); //FIXME: add IL version of ExchangeSamples
					ClearBufferIL(out_buf, frameCount, 0, 4);
					ClearBufferIL(out_buf, frameCount, 1, 4);
					if (new_keyer)
					{
						if (next_audio_state1 == AudioState.CW) 
						{
							cw_delay = 1;
							DttSP.CWtoneExchangeIL(out_buf, frameCount,4); //FIXME: add IL version of CWtoneExchange
						}  
						else DttSP.CWRingRestart();
					}
					if (switch_count-- == 0)
						current_audio_state1 = next_audio_state1;
					break;
			}

			// scale output for VAC
			/*if(vac_enabled &&
				rb_vacIN_l != null && rb_vacIN_r != null && 
				rb_vacOUT_l != null && rb_vacOUT_r != null)
			{
				ScaleBuffer(out_l1, out_l2, frameCount, (float)vac_rx_scale);
				ScaleBuffer(out_r1, out_r2, frameCount, (float)vac_rx_scale);

				if (sample_rate2 == sample_rate1) 
				{
					if (rb_vacOUT_l.WriteSpace()>=frameCount) rb_vacOUT_l.WritePtr(out_l2,frameCount);
					else VACDebug("rb_vacOUT_l overflow");
					if (rb_vacOUT_r.WriteSpace()>=frameCount) rb_vacOUT_r.WritePtr(out_r2,frameCount);
					else VACDebug("rb_vacOUT_r overflow");
				} 
				else 
				{
					if (vac_stereo) 
					{						
						fixed(float *res_outl_ptr = &(res_outl[0]))
							fixed(float *res_outr_ptr = &(res_outr[0])) 
							{
								int outsamps;
								DttSP.DoResamplerF(out_l2,res_outl_ptr,frameCount,&outsamps, resampPtrOut_l);
								DttSP.DoResamplerF(out_r2,res_outr_ptr,frameCount,&outsamps, resampPtrOut_r);
								if (rb_vacOUT_l.WriteSpace()>=outsamps) rb_vacOUT_l.WritePtr(res_outl_ptr,outsamps);
								else VACDebug("rb_vacOUT_l overflow");
								if (rb_vacOUT_r.WriteSpace()>=outsamps) rb_vacOUT_r.WritePtr(res_outr_ptr,outsamps);
								else VACDebug("rb_vacOUT_r overflow");
							}
					}
					else 
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))
						{
							int outsamps;
							DttSP.DoResamplerF(out_l2,res_outl_ptr,frameCount,&outsamps, resampPtrOut_l);
							if(rb_vacOUT_l.WriteSpace() >= outsamps) rb_vacOUT_l.WritePtr(res_outl_ptr,outsamps);
							else VACDebug("rb_vacOUT_l overflow");
							if(rb_vacOUT_r.WriteSpace() >= outsamps) rb_vacOUT_r.WritePtr(res_outl_ptr,outsamps);
							else VACDebug("rb_vacOUT_r overflow");
						}
					}
				}
			}*/

			// Scale output to SDR-1000
			if(!mox)
			{
				ScaleBufferIL(out_buf, out_buf, frameCount, (float)monitor_volume, 0, 0, 4);
				ScaleBufferIL(out_buf, out_buf, frameCount, (float)monitor_volume, 1, 1, 4);

				// VAC handling for third party program
				ClearBufferIL(out_buf, frameCount, 2, 4);
				ClearBufferIL(out_buf, frameCount, 3, 4);
			}
			else
			{
				double tx_vol = TXScale;
				if(high_pwr_am)
				{
					if(dsp_mode == DSPMode.AM ||
						dsp_mode == DSPMode.SAM)
						tx_vol *= 1.414;
				}

				ScaleBufferIL(out_buf, out_buf, frameCount, (float)tx_vol, 0, 2, 4);
				ScaleBufferIL(out_buf, out_buf, frameCount, (float)monitor_volume, 0, 0, 4);
				ScaleBufferIL(out_buf, out_buf, frameCount, (float)tx_vol, 1, 3, 4);
				ScaleBufferIL(out_buf, out_buf, frameCount, (float)monitor_volume, 1, 1, 4);

				// clip FireBox output to prevent overdrive
				/*if(soundcard == SoundCard.FIREBOX)
				{
					float clip_thresh = (float)(1.5f / console.AudioVolts1);
					for(int i=0; i<frameCount; i++)
					{
						if(out_l2[i] > clip_thresh)
						{
							Debug.WriteLine("Clip Left High: "+out_l2[i].ToString("f5"));
							out_l2[i] = clip_thresh;
						}
						else if(out_l2[i] < -clip_thresh)
						{
							Debug.WriteLine("Clip Left Low: "+out_l2[i].ToString("f5"));
							out_l2[i] = -clip_thresh;
						}

						if(out_r2[i] > clip_thresh) 
						{
							Debug.WriteLine("Clip Right High: "+out_l2[i].ToString("f5"));
							out_r2[i] = clip_thresh;							
						}
						else if(out_r2[i] < -clip_thresh)
						{
							Debug.WriteLine("Clip Right Low: "+out_l2[i].ToString("f5"));
							out_r2[i] = -clip_thresh;
						}
					}
				}*/
			}

#if(MINMAX)
			Debug.Write(Math.Max(MaxSampleIL(out_buf, frameCount, 2, 4),
				MaxSampleIL(out_buf, frameCount, 3, 4)).ToString("f6")+",");

			float current_max = Math.Max(MaxSampleIL(out_buf, frameCount, 2, 4),
				MaxSampleIL(out_buf, frameCount, 3, 4));
			if(current_max > max) max = current_max;
			Debug.WriteLine(" max: "+max.ToString("f6"));
#endif
			/*
						if(wave_record)
						{
							if(!mox)
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
									wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);
								}
							}
						}
			*/
#if(TIMER)
			t1.Stop();
			Debug.WriteLine(t1.Duration);
#endif
			return callback_return;
		}

		unsafe public static int CallbackVACIL(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
			float* in_ptr = (float*)input;
			float* out_ptr = (float*)output;

			for(int i=0; i<frameCount; i++)
			{
				i_l1[i] = *in_ptr++;
				if(vac_stereo) i_r1[i] = *in_ptr++;
			}
			
			fixed(float* in_l_ptr1 = &i_l1[0])
			{
				fixed(float* in_r_ptr1 = &i_r1[0])
				{
					fixed(float* out_l_ptr1 = &o_l1[0])
					{
						fixed(float* out_r_ptr1 = &o_r1[0])
						{
			if (vac_stereo)
			{
				if (vac_resample) 
				{
					int outsamps;
					fixed(float *res_inl_ptr = &(res_inl[0]))
						fixed(float *res_inr_ptr = &(res_inr[0])) 
						{
							DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr,frameCount,&outsamps, resampPtrIn_l);
							if (rb_vacIN_l.WriteSpace() >= outsamps)  rb_vacIN_l.WritePtr(res_inl_ptr,outsamps);
							else VACDebug("rb_vacIN_l overflow");
							DttSP.DoResamplerF(in_r_ptr1, res_inr_ptr,frameCount,&outsamps, resampPtrIn_r);
							if (rb_vacIN_r.WriteSpace() >= outsamps) rb_vacIN_r.WritePtr(res_inr_ptr,outsamps);
							else VACDebug("rb_vacIN_r overflow");
						}
				} 
				else 
				{
					if (rb_vacIN_l.WriteSpace() >= frameCount) rb_vacIN_l.WritePtr(in_l_ptr1,frameCount);
					else VACDebug("rb_vacIN_l overflow");
					if (rb_vacIN_r.WriteSpace() >= frameCount) rb_vacIN_r.WritePtr(in_r_ptr1,frameCount);
					else VACDebug("rb_vacIN_r overflow");
				}
				
				if (rb_vacOUT_l.ReadSpace() >= frameCount) rb_vacOUT_l.ReadPtr(out_l_ptr1,frameCount);
				else
				{
					ClearBuffer(out_l_ptr1,frameCount);
					VACDebug("rb_vacOUT_l underflow");
				}
				if (rb_vacOUT_r.ReadSpace() >= frameCount) rb_vacOUT_r.ReadPtr(out_r_ptr1,frameCount);
				else
				{
					ClearBuffer(out_r_ptr1,frameCount);
					VACDebug("rb_vacOUT_r underflow");
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
						if (rb_vacIN_l.WriteSpace() >= outsamps) rb_vacIN_l.WritePtr(res_inl_ptr,outsamps);
						else VACDebug("rb_vacIN_l overflow");
						if (rb_vacIN_r.WriteSpace() >= outsamps) rb_vacIN_r.WritePtr(res_inl_ptr,outsamps);
						else VACDebug("rb_vacIN_r overflow");
					}
				} 
				else 
				{
					if (rb_vacIN_l.WriteSpace() >= frameCount) rb_vacIN_l.WritePtr(in_l_ptr1,frameCount);
					else VACDebug("rb_vacIN_l overflow");
					if (rb_vacIN_r.WriteSpace() >= frameCount) rb_vacIN_r.WritePtr(in_l_ptr1,frameCount);
					else VACDebug("rb_vacIN_r overflow");
				}
				if (rb_vacOUT_l.ReadSpace() >= frameCount) rb_vacOUT_l.ReadPtr(out_l_ptr1,frameCount);
				else 
				{
					ClearBuffer(out_l_ptr1,frameCount);
					VACDebug("rb_vacOUT_l underflow");
				}
				if (rb_vacOUT_r.ReadSpace() >= frameCount) rb_vacOUT_r.ReadPtr(out_l_ptr1,frameCount);
				else
				{
					ClearBuffer(out_l_ptr1,frameCount);
					VACDebug("rb_vacOUT_r underflow");
				}
			}

						}
					}
				}
			}

			out_ptr = (float*)output;
			for(int i=0; i<frameCount; i++)
			{
				*out_ptr++ = o_l1[i];
				if(vac_stereo) *out_ptr++ = o_r1[i];
			}

			return 0;
		}

		unsafe public static int PipeIL(void* input, void* output, int frameCount, int frameSize,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
			float *inptr = (float *)input;
			float *outptr = (float *)output;

			for(int i=0; i<frameCount*frameSize; i++)
			{
				*outptr++ = *inptr++;
				*outptr++ = *inptr++;
			}
			return 0;
		}
#endif

		#endregion

		#region Interleaved Callbacks (with De-Interleaving built in)
#if(SPLIT_INTERLEAVED)
		unsafe public static int Callback1ILDI(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
#if(TIMER)
			t1.Start();
#endif

			float* in_ptr = (float*)input;
			float* out_ptr = (float*)output;
			for(int i=0; i<frameCount; i++)
			{
				i_l1[i] = *in_ptr++;
				i_r1[i] = *in_ptr++;
			}
			
			fixed(float* in_l_ptr1 = &i_l1[0])
			{
				fixed(float* in_r_ptr1 = &i_r1[0])
				{
					fixed(float* out_l_ptr1 = &o_l1[0])
					{
						fixed(float* out_r_ptr1 = &o_r1[0])
						{

							if(wave_playback)
								wave_file_reader.GetPlayBuffer(in_l_ptr1, in_r_ptr1);
							if((wave_record && !mox && record_rx_preprocessed) ||
								(wave_record && mox && record_tx_preprocessed))
								wave_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1);			

							if(phase)
							{
								phase_mutex.WaitOne();
								Marshal.Copy(new IntPtr(in_l_ptr1), phase_buf_l, 0, frameCount);
								Marshal.Copy(new IntPtr(in_r_ptr1), phase_buf_r, 0, frameCount);
								phase_mutex.ReleaseMutex();
							}

							float* in_l = null, in_r = null, out_l = null, out_r = null;

							in_l = in_l_ptr1;
							in_r = in_r_ptr1;

							out_l = out_l_ptr1;
							out_r = out_r_ptr1;

							switch(current_audio_state1)
							{
								case AudioState.DTTSP:
									if (new_keyer && ((dsp_mode == DSPMode.CWU)
										|| (dsp_mode == DSPMode.CWL)))
									{
										DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
									}

									// scale input with mic preamp
									if(mox && !vac_enabled &&
										(dsp_mode == DSPMode.LSB ||
										dsp_mode == DSPMode.USB ||
										dsp_mode == DSPMode.DSB ||
										dsp_mode == DSPMode.AM  ||
										dsp_mode == DSPMode.SAM ||
										dsp_mode == DSPMode.FMN))
									{
										ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
										ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);
									}

									#region Input Signal Source

								switch(current_input_signal)
								{
									case SignalSource.SOUNDCARD:
										break;
									case SignalSource.SINE:
										SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
										phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
										ScaleBuffer(in_l, in_l, frameCount, (float)input_source_scale);
										ScaleBuffer(in_r, in_r, frameCount, (float)input_source_scale);
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

											if(rb_vacIN_l.ReadSpace() >= frameCount) rb_vacIN_l.ReadPtr(in_l,frameCount);
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
										}
									} 
									else DttSP.ExchangeSamples(in_l, in_r, out_l, out_r, frameCount);

#if(MINMAX)
					Debug.WriteLine(MaxSample(out_l, out_r, frameCount));
#endif

									#region Output Signal Source

								switch(current_output_signal)
								{
									case SignalSource.SOUNDCARD:
										break;
									case SignalSource.SINE:
										SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
										phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
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
								}

									#endregion

									break;
								case AudioState.CW: 
									if(next_audio_state1 == AudioState.SWITCH) 
									{
										MSCVRT.memset(in_l_ptr1, 0, frameCount*sizeof(float));
										MSCVRT.memset(in_r_ptr1, 0, frameCount*sizeof(float));						
										DttSP.ExchangeSamples(in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);
										if (switch_count == 0) next_audio_state1 = AudioState.CW;
										switch_count--;
									}

									if (new_keyer)  
									{
										DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
									}
									else 
									{
										cw_form.CWGen(out_l_ptr1, out_r_ptr1, frameCount);
									}					
									break;
								case AudioState.SINL_COSR:
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
									break;
								case AudioState.SWITCH:
									ClearBuffer(in_l_ptr1, frameCount);
									ClearBuffer(in_r_ptr1, frameCount);
									DttSP.ExchangeSamples(in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);
									ClearBuffer(out_l_ptr1, frameCount);
									ClearBuffer(out_r_ptr1, frameCount);
									if (new_keyer)
									{
										if (next_audio_state1  == AudioState.CW) 
										{
											cw_delay = 1;
											DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
										}  
										else DttSP.CWRingRestart();
									}

									if (switch_count-- == 0) current_audio_state1 = next_audio_state1;
									break;
							}

							double vol = monitor_volume;
							if(mox)
							{
								vol = TXScale;

								if(high_pwr_am)
								{
									if(dsp_mode == DSPMode.AM ||
										dsp_mode == DSPMode.SAM)
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
									if (rb_vacOUT_l.WriteSpace()>=frameCount) rb_vacOUT_l.WritePtr(out_l,frameCount);
									else VACDebug("rb_vacOUT_l overflow");
									if (rb_vacOUT_r.WriteSpace()>=frameCount) rb_vacOUT_r.WritePtr(out_r,frameCount);
									else VACDebug("rb_vacOUT_r overflow");
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
												if (rb_vacOUT_l.WriteSpace() >= outsamps) rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
												else VACDebug("rb_vacOUT_l overflow");
												if (rb_vacOUT_r.WriteSpace() >= outsamps) rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
												else VACDebug("rb_vacOUT_r overflow");
											}
									}
									else 
									{
										fixed(float *res_outl_ptr = &(res_outl[0]))
										{
											int outsamps;
											DttSP.DoResamplerF(out_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
											if(rb_vacOUT_l.WriteSpace() >= outsamps) rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
											else VACDebug("rb_vacOUT_l overflow");
											if(rb_vacOUT_r.WriteSpace() >= outsamps) rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
											else VACDebug("rb_vacOUT_r overflow");
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

						}
					}
				}
			}

			out_ptr = (float*)output;
			for(int i=0; i<frameCount; i++)
			{
				*out_ptr++ = o_l1[i];
				*out_ptr++ = o_r1[i];
			}
			return callback_return;			
		}

		unsafe public static int Callback4PortILDI(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
#if(TIMER)
			t1.Start();
#endif
			float* in_l = null, in_r = null;
			float* out_l1 = null, out_r1 = null, out_l2 = null, out_r2 = null;
			float* in_ptr = (float*)input;
			float* out_ptr = (float*)output;
			for(int i=0; i<frameCount; i++)
			{
				i_l1[i] = *in_ptr++;
				i_r1[i] = *in_ptr++;
				i_l2[i] = *in_ptr++;
				i_r2[i] = *in_ptr++;
			}
			
			fixed(float* in_l_ptr1 = &i_l1[0])
			{
				fixed(float* in_r_ptr1 = &i_r1[0])
				{
					fixed(float* out_l_ptr1 = &o_l1[0])
					{
						fixed(float* out_r_ptr1 = &o_r1[0])
						{
							fixed(float* in_l_ptr2 = &i_l2[0])
							{
								fixed(float* in_r_ptr2 = &i_r2[0])
								{
									fixed(float* out_l_ptr2 = &o_l2[0])
									{
										fixed(float* out_r_ptr2 = &o_r2[0])
										{

											switch(soundcard)
											{
												case SoundCard.FIREBOX:
													if(!mox)
													{
														in_l = in_l_ptr2;
														in_r = in_r_ptr2;
													}
													else
													{
														in_l = in_l_ptr1;
														in_r = in_r_ptr1;
													}
													break;
												case SoundCard.DELTA_44:
												default:
													if(!mox)
													{
														in_l = in_l_ptr1;
														in_r = in_r_ptr1;
													}
													else
													{
														in_l = in_l_ptr2;
														in_r = in_r_ptr2;
													}
													break;
											}

											if(wave_playback)
												wave_file_reader.GetPlayBuffer(in_l, in_r);
											if(wave_record)
											{
												if(!mox)
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
												phase_mutex.WaitOne();
												Marshal.Copy(new IntPtr(in_l), phase_buf_l, 0, frameCount);
												Marshal.Copy(new IntPtr(in_r), phase_buf_r, 0, frameCount);
												phase_mutex.ReleaseMutex();
											}

											switch(current_audio_state1)
											{
												case AudioState.DTTSP:
					
													#region VOX
													if(vox_enabled)
													{
														float* vox_l = null, vox_r = null;
														switch(soundcard)
														{
															case SoundCard.FIREBOX:
																vox_l = in_l_ptr1;
																vox_r = in_r_ptr1;
																break;
															case SoundCard.DELTA_44:
															default:
																vox_l = in_l_ptr2;
																vox_r = in_r_ptr2;
																break;
														}

														if(dsp_mode == DSPMode.LSB ||
															dsp_mode == DSPMode.USB ||
															dsp_mode == DSPMode.DSB ||
															dsp_mode == DSPMode.AM  ||
															dsp_mode == DSPMode.SAM ||
															dsp_mode == DSPMode.FMN)
														{
															double peak = MaxSample(vox_l, vox_r, frameCount);

															// compare power to threshold
															if(peak > vox_threshold) 
																vox_active = true;
															else 
																vox_active = false;
														}
													}
													#endregion

													if (new_keyer && ((dsp_mode == DSPMode.CWU)
														|| (dsp_mode == DSPMode.CWL)))
													{
														DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
													} 

													// scale input with mic preamp
													if(mox && !vac_enabled &&
														(dsp_mode == DSPMode.LSB ||
														dsp_mode == DSPMode.USB ||
														dsp_mode == DSPMode.DSB ||
														dsp_mode == DSPMode.AM  ||
														dsp_mode == DSPMode.SAM ||
														dsp_mode == DSPMode.FMN))
													{
														ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
														ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);
													}

													#region Input Signal Source

												switch(current_input_signal)
												{
													case SignalSource.SOUNDCARD:
														break;
													case SignalSource.SINE:
														SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
														phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
														ScaleBuffer(in_l, in_l, frameCount, (float)input_source_scale);
														ScaleBuffer(in_r, in_r, frameCount, (float)input_source_scale);
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
												}

													#endregion

#if(MINMAX)
													Debug.Write(MaxSample(in_l, in_r, frameCount).ToString("f6")+",");
#endif

													if(vac_enabled &&
														rb_vacIN_l != null && rb_vacIN_r != null && 
														rb_vacOUT_l != null && rb_vacOUT_r != null)
													{
														if(mox) // transmit mode
														{
															if(rb_vacIN_l.ReadSpace() >= frameCount) rb_vacIN_l.ReadPtr(in_l,frameCount);
															else
															{
																ClearBuffer(in_l,frameCount);
																VACDebug("rb_vacIN_l underflow");
															}
															if(rb_vacIN_r.ReadSpace() >= frameCount) rb_vacIN_r.ReadPtr(in_r,frameCount);
															else
															{
																ClearBuffer(in_r,frameCount);
																VACDebug("rb_vacIN_r underflow");
															}
															ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
															ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);						
															DttSP.ExchangeSamples(in_l, in_r, out_l_ptr1, out_r_ptr1, frameCount);
														}
														else // receive mode
														{
															DttSP.ExchangeSamples(in_l, in_r, out_l_ptr1, out_r_ptr1, frameCount);

															if(rb_vacIN_l.ReadSpace() >= frameCount) rb_vacIN_l.ReadPtr(in_l,frameCount);
															else
															{
																ClearBuffer(in_l,frameCount);
																VACDebug("rb_vacIN_l underflow");
															}
															if(rb_vacIN_r.ReadSpace() >= frameCount) rb_vacIN_r.ReadPtr(in_r,frameCount);
															else
															{
																ClearBuffer(in_r,frameCount);
																VACDebug("rb_vacIN_r underflow");
															}
														}
													} 
													else DttSP.ExchangeSamples(in_l, in_r, out_l_ptr1, out_r_ptr1, frameCount);

#if(MINMAX)
													Debug.Write(MaxSample(out_l_ptr1, out_r_ptr1, frameCount).ToString("f6")+",");
#endif

													#region Output Signal Source

												switch(current_output_signal)
												{
													case SignalSource.SOUNDCARD:
														break;
													case SignalSource.SINE:
														SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
														phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
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
												}

													#endregion

													break;
												case AudioState.CW: 
													if(next_audio_state1 == AudioState.SWITCH)
													{
														ClearBuffer(in_l_ptr1, frameCount);
														ClearBuffer(in_r_ptr1, frameCount);
														DttSP.ExchangeSamples(in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);
														if (switch_count == 0) next_audio_state1 = AudioState.CW;
														switch_count--;
													}

													if (new_keyer)  
													{
														DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
														if (cw_delay == 1) cw_delay = 0;
													}
													else 
													{
														cw_form.CWGen(out_l_ptr1, out_r_ptr1, frameCount);
													}
													break;
												case AudioState.SINL_COSR:
													out_l1 = out_l_ptr1;
													out_r1 = out_r_ptr1;
													out_l2 = out_l_ptr2;
													out_r2 = out_r_ptr2;

													if(two_tone)
													{
														double dump;
														SineWave2Tone(out_l1, frameCount,
															phase_accumulator1, phase_accumulator2,
															sine_freq1, sine_freq2,
															out dump, out dump);

														CosineWave2Tone(out_r1, frameCount,
															phase_accumulator1, phase_accumulator2,
															sine_freq1, sine_freq2,
															out phase_accumulator1, out phase_accumulator2);
													}
													else
													{
														SineWave(out_l1, frameCount, phase_accumulator1, sine_freq1);
														phase_accumulator1 = CosineWave(out_r1, frameCount, phase_accumulator1, sine_freq1);
													}
													break;					
												case AudioState.SINL_SINR:
													out_l1 = out_l_ptr1;
													out_r1 = out_r_ptr1;
													out_l2 = out_l_ptr2;
													out_r2 = out_r_ptr2;

													if(two_tone)
													{
														SineWave2Tone(out_l1, frameCount,
															phase_accumulator1, phase_accumulator2,
															sine_freq1, sine_freq2,
															out phase_accumulator1, out phase_accumulator2);
														CopyBuffer(out_l1, out_r1, frameCount);							
													}
													else
													{
														phase_accumulator1 = SineWave(out_l1, frameCount, phase_accumulator1, sine_freq1);
														CopyBuffer(out_l1, out_r1, frameCount);
													}
													break;
												case AudioState.SINL_NOR:
													out_l1 = out_l_ptr1;
													out_r1 = out_r_ptr1;
													out_l2 = out_l_ptr2;
													out_r2 = out_r_ptr2;

													if(two_tone)
													{
														SineWave2Tone(out_l1, frameCount,
															phase_accumulator1, phase_accumulator2,
															sine_freq1, sine_freq2,
															out phase_accumulator1, out phase_accumulator2);
														ClearBuffer(out_r1, frameCount);		
													}
													else
													{
														phase_accumulator1 = SineWave(out_l1, frameCount, phase_accumulator1, sine_freq1);
														ClearBuffer(out_r1, frameCount);
													}
													break;
												case AudioState.NOL_SINR:
													out_l1 = out_l_ptr1;
													out_r1 = out_r_ptr1;
													out_l2 = out_l_ptr2;
													out_r2 = out_r_ptr2;

													if(two_tone)
													{
														ClearBuffer(out_l1, frameCount);
														SineWave2Tone(out_r1, frameCount,
															phase_accumulator1, phase_accumulator2,
															sine_freq1, sine_freq2,
															out phase_accumulator1, out phase_accumulator2);
													}
													else
													{
														ClearBuffer(out_l1, frameCount);
														phase_accumulator1 = SineWave(out_r1, frameCount, phase_accumulator1, sine_freq1);
													}
													break;
												case AudioState.NOL_NOR:
													ClearBuffer(out_l_ptr1, frameCount);
													ClearBuffer(out_r_ptr1, frameCount);
													break;
												case AudioState.PIPE:
													CopyBuffer(in_l_ptr1, out_l_ptr1, frameCount);
													CopyBuffer(in_r_ptr1, out_r_ptr1, frameCount);
													break;
												case AudioState.SWITCH:
													ClearBuffer(in_l_ptr1, frameCount);
													ClearBuffer(in_r_ptr1, frameCount);
													DttSP.ExchangeSamples(in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);
													ClearBuffer(out_l_ptr1, frameCount);
													ClearBuffer(out_r_ptr1, frameCount);
													if (new_keyer)
													{
														if (next_audio_state1 == AudioState.CW) 
														{
															cw_delay = 1;
															DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
														}  
														else DttSP.CWRingRestart();
													}
													if (switch_count-- == 0)
														current_audio_state1 = next_audio_state1;
													break;
											}

											out_l1 = out_l_ptr1;
											out_r1 = out_r_ptr1;
											out_l2 = out_l_ptr2;
											out_r2 = out_r_ptr2;

											// scale output for VAC
											if(vac_enabled &&
												rb_vacIN_l != null && rb_vacIN_r != null && 
												rb_vacOUT_l != null && rb_vacOUT_r != null)
											{
												ScaleBuffer(out_l1, out_l2, frameCount, (float)vac_rx_scale);
												ScaleBuffer(out_r1, out_r2, frameCount, (float)vac_rx_scale);

												if (sample_rate2 == sample_rate1) 
												{
													if (rb_vacOUT_l.WriteSpace()>=frameCount) rb_vacOUT_l.WritePtr(out_l2,frameCount);
													else VACDebug("rb_vacOUT_l overflow");
													if (rb_vacOUT_r.WriteSpace()>=frameCount) rb_vacOUT_r.WritePtr(out_r2,frameCount);
													else VACDebug("rb_vacOUT_r overflow");
												} 
												else 
												{
													if (vac_stereo) 
													{						
														fixed(float *res_outl_ptr = &(res_outl[0]))
															fixed(float *res_outr_ptr = &(res_outr[0])) 
															{
																int outsamps;
																DttSP.DoResamplerF(out_l2,res_outl_ptr,frameCount,&outsamps, resampPtrOut_l);
																DttSP.DoResamplerF(out_r2,res_outr_ptr,frameCount,&outsamps, resampPtrOut_r);
																if (rb_vacOUT_l.WriteSpace()>=outsamps) rb_vacOUT_l.WritePtr(res_outl_ptr,outsamps);
																else VACDebug("rb_vacOUT_l overflow");
																if (rb_vacOUT_r.WriteSpace()>=outsamps) rb_vacOUT_r.WritePtr(res_outr_ptr,outsamps);
																else VACDebug("rb_vacOUT_r overflow");
															}
													}
													else 
													{
														fixed(float *res_outl_ptr = &(res_outl[0]))
														{
															int outsamps;
															DttSP.DoResamplerF(out_l2,res_outl_ptr,frameCount,&outsamps, resampPtrOut_l);
															if(rb_vacOUT_l.WriteSpace() >= outsamps) rb_vacOUT_l.WritePtr(res_outl_ptr,outsamps);
															else VACDebug("rb_vacOUT_l overflow");
															if(rb_vacOUT_r.WriteSpace() >= outsamps) rb_vacOUT_r.WritePtr(res_outl_ptr,outsamps);
															else VACDebug("rb_vacOUT_r overflow");
														}
													}
												}
											}

											// Scale output to SDR-1000
											if(!mox)
											{
												ScaleBuffer(out_l1, out_l1, frameCount, (float)monitor_volume);
												ScaleBuffer(out_r1, out_r1, frameCount, (float)monitor_volume);
												// VAC handling for third party program


												ClearBuffer(out_l2, frameCount);
												ClearBuffer(out_r2, frameCount);
											}
											else
											{
												double tx_vol = TXScale;
												if(high_pwr_am)
												{
													if(dsp_mode == DSPMode.AM ||
														dsp_mode == DSPMode.SAM)
														tx_vol *= 1.414;
												}

												ScaleBuffer(out_l1, out_l2, frameCount, (float)tx_vol);
												ScaleBuffer(out_l1, out_l1, frameCount, (float)monitor_volume);
												ScaleBuffer(out_r1, out_r2, frameCount, (float)tx_vol);
												ScaleBuffer(out_r1, out_r1, frameCount, (float)monitor_volume);

												// clip FireBox output to prevent overdrive
												/*if(soundcard == SoundCard.FIREBOX)
																				{
																					float clip_thresh = (float)(1.5f / console.AudioVolts1);
																					for(int i=0; i<frameCount; i++)
																					{
																						if(out_l2[i] > clip_thresh)
																						{
																							Debug.WriteLine("Clip Left High: "+out_l2[i].ToString("f5"));
																							out_l2[i] = clip_thresh;
																						}
																						else if(out_l2[i] < -clip_thresh)
																						{
																							Debug.WriteLine("Clip Left Low: "+out_l2[i].ToString("f5"));
																							out_l2[i] = -clip_thresh;
																						}

																						if(out_r2[i] > clip_thresh) 
																						{
																							Debug.WriteLine("Clip Right High: "+out_l2[i].ToString("f5"));
																							out_r2[i] = clip_thresh;							
																						}
																						else if(out_r2[i] < -clip_thresh)
																						{
																							Debug.WriteLine("Clip Right Low: "+out_l2[i].ToString("f5"));
																							out_r2[i] = -clip_thresh;
																						}
																					}
																				}*/
											}

#if(MINMAX)
											Debug.Write(MaxSample(out_l2, out_r2, frameCount).ToString("f6")+",");

											float current_max = MaxSample(out_l2, out_r2, frameCount);
											if(current_max > max) max = current_max;
											Debug.WriteLine(" max: "+max.ToString("f6"));
#endif

											if(wave_record)
											{
												if(!mox)
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
														wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);
													}
												}
											}
#if(TIMER)
											t1.Stop();
											Debug.WriteLine(t1.Duration);
#endif

										}
									}
								}
							}
						}
					}
				}
			}

			out_ptr = (float*)output;
			for(int i=0; i<frameCount; i++)
			{
				*out_ptr++ = o_l1[i];
				*out_ptr++ = o_r1[i];
				*out_ptr++ = o_l2[i];
				*out_ptr++ = o_r2[i];
			}

			return callback_return;
		}

		unsafe public static int CallbackVACILDI(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
			float* in_ptr = (float*)input;
			float* out_ptr = (float*)output;

			for(int i=0; i<frameCount; i++)
			{
				i_l1[i] = *in_ptr++;
				if(vac_stereo) i_r1[i] = *in_ptr++;
			}
			
			fixed(float* in_l_ptr1 = &i_l1[0])
			{
				fixed(float* in_r_ptr1 = &i_r1[0])
				{
					fixed(float* out_l_ptr1 = &o_l1[0])
					{
						fixed(float* out_r_ptr1 = &o_r1[0])
						{
							if (vac_stereo)
							{
								if (vac_resample) 
								{
									int outsamps;
									fixed(float *res_inl_ptr = &(res_inl[0]))
										fixed(float *res_inr_ptr = &(res_inr[0])) 
										{
											DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr,frameCount,&outsamps, resampPtrIn_l);
											if (rb_vacIN_l.WriteSpace() >= outsamps)  rb_vacIN_l.WritePtr(res_inl_ptr,outsamps);
											else VACDebug("rb_vacIN_l overflow");
											DttSP.DoResamplerF(in_r_ptr1, res_inr_ptr,frameCount,&outsamps, resampPtrIn_r);
											if (rb_vacIN_r.WriteSpace() >= outsamps) rb_vacIN_r.WritePtr(res_inr_ptr,outsamps);
											else VACDebug("rb_vacIN_r overflow");
										}
								} 
								else 
								{
									if (rb_vacIN_l.WriteSpace() >= frameCount) rb_vacIN_l.WritePtr(in_l_ptr1,frameCount);
									else VACDebug("rb_vacIN_l overflow");
									if (rb_vacIN_r.WriteSpace() >= frameCount) rb_vacIN_r.WritePtr(in_r_ptr1,frameCount);
									else VACDebug("rb_vacIN_r overflow");
								}
				
								if (rb_vacOUT_l.ReadSpace() >= frameCount) rb_vacOUT_l.ReadPtr(out_l_ptr1,frameCount);
								else
								{
									ClearBuffer(out_l_ptr1,frameCount);
									VACDebug("rb_vacOUT_l underflow");
								}
								if (rb_vacOUT_r.ReadSpace() >= frameCount) rb_vacOUT_r.ReadPtr(out_r_ptr1,frameCount);
								else
								{
									ClearBuffer(out_r_ptr1,frameCount);
									VACDebug("rb_vacOUT_r underflow");
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
										if (rb_vacIN_l.WriteSpace() >= outsamps) rb_vacIN_l.WritePtr(res_inl_ptr,outsamps);
										else VACDebug("rb_vacIN_l overflow");
										if (rb_vacIN_r.WriteSpace() >= outsamps) rb_vacIN_r.WritePtr(res_inl_ptr,outsamps);
										else VACDebug("rb_vacIN_r overflow");
									}
								} 
								else 
								{
									if (rb_vacIN_l.WriteSpace() >= frameCount) rb_vacIN_l.WritePtr(in_l_ptr1,frameCount);
									else VACDebug("rb_vacIN_l overflow");
									if (rb_vacIN_r.WriteSpace() >= frameCount) rb_vacIN_r.WritePtr(in_l_ptr1,frameCount);
									else VACDebug("rb_vacIN_r overflow");
								}
								if (rb_vacOUT_l.ReadSpace() >= frameCount) rb_vacOUT_l.ReadPtr(out_l_ptr1,frameCount);
								else 
								{
									ClearBuffer(out_l_ptr1,frameCount);
									VACDebug("rb_vacOUT_l underflow");
								}
								if (rb_vacOUT_r.ReadSpace() >= frameCount) rb_vacOUT_r.ReadPtr(out_l_ptr1,frameCount);
								else
								{
									ClearBuffer(out_l_ptr1,frameCount);
									VACDebug("rb_vacOUT_r underflow");
								}
							}

						}
					}
				}
			}

			out_ptr = (float*)output;
			for(int i=0; i<frameCount; i++)
			{
				*out_ptr++ = o_l1[i];
				if(vac_stereo) *out_ptr++ = o_r1[i];
			}

			return 0;
		}

		unsafe public static int PipeILDI(void* input, void* output, int frameCount, int frameSize,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
			float *inptr = (float *)input;
			float *outptr = (float *)output;

			for(int i=0; i<frameCount*frameSize; i++)
			{
				*outptr++ = *inptr++;
				*outptr++ = *inptr++;
			}
			return 0;
		}
		#endif
		#endregion

		#region Non-Intereleaved Buffer Operations

		unsafe private static void ClearBuffer(float *buf, int samples)
		{
			MSCVRT.memset(buf, 0, samples*sizeof(float));
		}

		unsafe private static void CopyBuffer(float *inbuf, float *outbuf, int samples)
		{
			MSCVRT.memcpy(outbuf, inbuf, samples*sizeof(float));
		}

		unsafe private static void ScaleBuffer(float *inbuf, float *outbuf, int samples, float scale)
		{
			for(int i=0; i<samples; i++)
				outbuf[i] = inbuf[i] * scale;
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

		// returns updated phase accumulator
		unsafe public static double SineWave(float* buf, int samples, double phase, double freq)
		{
			double phase_step = freq/DttSP.SampleRate*2*Math.PI;
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
			double phase_step = freq/DttSP.SampleRate*2*Math.PI;
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
			double phase_step1 = freq1/DttSP.SampleRate*2*Math.PI;
			double cosval1 = Math.Cos(phase1);
			double sinval1 = Math.Sin(phase1);
			double cosdelta1 = Math.Cos(phase_step1);
			double sindelta1 = Math.Sin(phase_step1);

			double phase_step2 = freq2/DttSP.SampleRate*2*Math.PI;
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
			double phase_step1 = freq1/DttSP.SampleRate*2*Math.PI;
			double cosval1 = Math.Cos(phase1);
			double sinval1 = Math.Sin(phase1);
			double cosdelta1 = Math.Cos(phase_step1);
			double sindelta1 = Math.Sin(phase_step1);

			double phase_step2 = freq2/DttSP.SampleRate*2*Math.PI;
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
			double step = freq/DttSP.SampleRate*2*tri_direction;
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
			double step = freq/DttSP.SampleRate*saw_direction;
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

		#region Interleaved Buffer Operations

		unsafe private static void ClearBufferIL(float *buf, int frames, int chan, int num_chan)
		{
			for(int i=0; i<frames; i++)
				buf[i*num_chan+chan] = 0;
		}

		unsafe private static void CopyBufferIL(float *inbuf, float *outbuf, int frames,
			int in_chan, int out_chan, int num_chan)
		{
			for(int i=0; i<frames; i++)
				outbuf[i*num_chan+out_chan] = inbuf[i*num_chan+in_chan];
		}

		unsafe private static void ScaleBufferIL(float *inbuf, float *outbuf, int frames,
			float scale, int in_chan, int out_chan, int num_chan)
		{
			for(int i=0; i<frames; i++)
				outbuf[i*num_chan+in_chan] = inbuf[i*num_chan+out_chan] * scale;
		}

		unsafe public static float MaxSampleIL(float* buf, int frames, int chan, int num_chan)
		{
			float max = float.MinValue;
			for(int i=0; i<frames; i++)
				if(buf[i*num_chan+chan] > max) max = buf[i*num_chan+chan];
				
			return max;
		}

		unsafe public static float MaxSampleIL(float* buf, int frames, int num_chan)
		{
			float max = float.MinValue;
			for(int i=0; i<frames; i++)
				for(int j=0; j<num_chan; j++)
					if(buf[i*num_chan+j] > max) max = buf[i*num_chan+j];
				
			return max;
		}

		unsafe public static float MaxSampleIL(float* buf1, float* buf2, int frames, int num_chan)
		{
			float max = float.MinValue;
			for(int i=0; i<frames; i++)
				for(int j=0; j<num_chan; j++)
				{
					if(buf1[i*num_chan+j] > max) max = buf1[i*num_chan+j];
					if(buf2[i*num_chan+j] > max) max = buf2[i*num_chan+j];
				}
			return max;
		}

		unsafe public static float MinSampleIL(float* buf, int frames, int num_chan)
		{
			float min = float.MaxValue;
			for(int i=0; i<frames; i++)
				for(int j=0; j<num_chan; j++)
					if(buf[i*num_chan+j] < min) min = buf[i*num_chan+j];
				
			return min;
		}

		unsafe public static float MinSampleIL(float* buf1, float* buf2, int frames, int num_chan)
		{
			float min = float.MaxValue;
			for(int i=0; i<frames; i++)
				for(int j=0; j<num_chan; j++)
				{
					if(buf1[i*num_chan+j] < min) min = buf1[i*num_chan+j];
					if(buf2[i*num_chan+j] < min) min = buf2[i*num_chan+j];
				}
				
			return min;
		}

		// returns updated phase accumulator
		unsafe public static double SineWaveIL(float* buf, int frames, double phase, double freq, int chan, int num_chan)
		{
			double phase_step = freq/DttSP.SampleRate*2*Math.PI;
			double cosval = Math.Cos(phase);
			double sinval = Math.Sin(phase);
			double cosdelta = Math.Cos(phase_step);
			double sindelta = Math.Sin(phase_step);
			double tmpval;

			for(int i=0; i<frames; i++ )
			{
				tmpval = cosval*cosdelta - sinval*sindelta;
				sinval = cosval*sindelta + sinval*cosdelta;
				cosval = tmpval;
						
				buf[i*num_chan+chan] = (float)(sinval);
											 
				phase += phase_step;
			}

			return phase;
		}

		// returns updated phase accumulator
		unsafe public static double CosineWaveIL(float* buf, int frames, double phase, double freq, int chan, int num_chan)
		{
			double phase_step = freq/DttSP.SampleRate*2*Math.PI;
			double cosval = Math.Cos(phase);
			double sinval = Math.Sin(phase);
			double cosdelta = Math.Cos(phase_step);
			double sindelta = Math.Sin(phase_step);
			double tmpval;

			for(int i=0; i<frames; i++ )
			{
				tmpval = cosval*cosdelta - sinval*sindelta;
				sinval = cosval*sindelta + sinval*cosdelta;
				cosval = tmpval;
						
				buf[i*num_chan+chan] = (float)(cosval);
											 
				phase += phase_step;
			}

			return phase;
		}

		unsafe public static void SineWave2ToneIL(float* buf, int frames, 
			double phase1, double phase2, 
			double freq1, double freq2,
			out double updated_phase1, out double updated_phase2,
			int chan, int num_chan)
		{
			double phase_step1 = freq1/DttSP.SampleRate*2*Math.PI;
			double cosval1 = Math.Cos(phase1);
			double sinval1 = Math.Sin(phase1);
			double cosdelta1 = Math.Cos(phase_step1);
			double sindelta1 = Math.Sin(phase_step1);

			double phase_step2 = freq2/DttSP.SampleRate*2*Math.PI;
			double cosval2 = Math.Cos(phase2);
			double sinval2 = Math.Sin(phase2);
			double cosdelta2 = Math.Cos(phase_step2);
			double sindelta2 = Math.Sin(phase_step2);
			double tmpval;

			for(int i=0; i<frames; i++ )
			{
				tmpval = cosval1*cosdelta1 - sinval1*sindelta1;
				sinval1 = cosval1*sindelta1 + sinval1*cosdelta1;
				cosval1 = tmpval;

				tmpval = cosval2*cosdelta2 - sinval2*sindelta2;
				sinval2 = cosval2*sindelta2 + sinval2*cosdelta2;
				cosval2 = tmpval;
						
				buf[i*num_chan+chan] = (float)(sinval1*0.5 + sinval2*0.5);
											 
				phase1 += phase_step1;
				phase2 += phase_step2;
			}

			updated_phase1 = phase1;
			updated_phase2 = phase2;
		}

		unsafe public static void CosineWave2ToneIL(float* buf, int frames, 
			double phase1, double phase2, 
			double freq1, double freq2,
			out double updated_phase1, out double updated_phase2,
			int chan, int num_chan)
		{
			double phase_step1 = freq1/DttSP.SampleRate*2*Math.PI;
			double cosval1 = Math.Cos(phase1);
			double sinval1 = Math.Sin(phase1);
			double cosdelta1 = Math.Cos(phase_step1);
			double sindelta1 = Math.Sin(phase_step1);

			double phase_step2 = freq2/DttSP.SampleRate*2*Math.PI;
			double cosval2 = Math.Cos(phase2);
			double sinval2 = Math.Sin(phase2);
			double cosdelta2 = Math.Cos(phase_step2);
			double sindelta2 = Math.Sin(phase_step2);
			double tmpval;

			for(int i=0; i<frames; i++ )
			{
				tmpval = cosval1*cosdelta1 - sinval1*sindelta1;
				sinval1 = cosval1*sindelta1 + sinval1*cosdelta1;
				cosval1 = tmpval;

				tmpval = cosval2*cosdelta2 - sinval2*sindelta2;
				sinval2 = cosval2*sindelta2 + sinval2*cosdelta2;
				cosval2 = tmpval;
						
				buf[i*num_chan+chan] = (float)(cosval1*0.5 + cosval2*0.5);
											 
				phase1 += phase_step1;
				phase2 += phase_step2;
			}

			updated_phase1 = phase1;
			updated_phase2 = phase2;
		}

		unsafe public static void NoiseIL(float* buf, int frames, int chan, int num_chan)
		{
			for(int i=0; i<frames; i++)
			{
				buf[i*num_chan+chan] = (float)boxmuller(0.0,0.2);
			}
		}

		unsafe public static void TriangleIL(float* buf, int frames, double freq, int chan, int num_chan)
		{
			double step = freq/DttSP.SampleRate*2*tri_direction;
			for(int i=0; i<frames; i++)
			{
				buf[i*num_chan+chan] = (float)tri_val;
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

		unsafe public static void SawtoothIL(float* buf, int frames, double freq, int chan, int num_chan)
		{
			double step = freq/DttSP.SampleRate*saw_direction;
			for(int i=0; i<frames; i++)
			{
				buf[i*num_chan+chan] = (float)saw_val;
				saw_val += step;
				if(saw_val >= 1.0) saw_val -= 2.0;
				if(saw_val <= -1.0) saw_val += 2.0;
			}
		}

		unsafe public static void AddConstantIL(float* buf, int frames, float val, int chan, int num_chan)
		{
			for(int i=0; i<frames; i++)
				buf[i*num_chan+chan] += val;
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

		unsafe private static void InitVAC()
		{
			if(rb_vacOUT_l == null) rb_vacOUT_l = new RingBuffer(65536);
			rb_vacOUT_l.Restart(block_size2);
			
			if(rb_vacOUT_r == null) rb_vacOUT_r = new RingBuffer(65536);
			rb_vacOUT_r.Restart(block_size2);

			if(rb_vacIN_l == null) rb_vacIN_l = new RingBuffer(4*65536);
			rb_vacIN_l.Restart(block_size2);

			if(rb_vacIN_r == null) rb_vacIN_r = new RingBuffer(4*65536);
			rb_vacIN_r.Restart(block_size2);

			if (sample_rate2 != sample_rate1) 
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
			else vac_resample = false;
		}

		unsafe private static void CleanUpVAC()
		{
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
			//xylowolf -- add in host for xylowolson 
			a.Add("VK6APH/KD5TFD Wolfson FPGA"); 
			return a;
		}

		public static ArrayList GetPAInputDevices(int hostIndex)
		{
			ArrayList a = new ArrayList();
			if ( hostIndex >= PA19.PA_GetHostApiCount() ) //xylowolf 
			{ 
				a.Add("Wolfson"); 
				return a; 
			} 
			PA19.PaHostApiInfo hostInfo = PA19.PA_GetHostApiInfo(hostIndex);
			for(int i=0; i<hostInfo.deviceCount; i++)
			{
				int devIndex = PA19.PA_HostApiDeviceIndexToDeviceIndex(hostIndex, i);
				PA19.PaDeviceInfo devInfo = PA19.PA_GetDeviceInfo(devIndex);
				if(devInfo.maxInputChannels > 0)
					a.Add(devInfo.name/* + " - " + devIndex*/);
			}
			return a;
		}	

		public static ArrayList GetPAOutputDevices(int hostIndex)
		{
			ArrayList a = new ArrayList();
			if ( hostIndex >= PA19.PA_GetHostApiCount() ) //xylowolf 
			{ 
				a.Add("FPGA PWM D/A"); 
				return a; 
			} 
			PA19.PaHostApiInfo hostInfo = PA19.PA_GetHostApiInfo(hostIndex);
			for(int i=0; i<hostInfo.deviceCount; i++)
			{
				int devIndex = PA19.PA_HostApiDeviceIndexToDeviceIndex(hostIndex, i);
				PA19.PaDeviceInfo devInfo = PA19.PA_GetDeviceInfo(devIndex);
				if(devInfo.maxOutputChannels > 0)
					a.Add(devInfo.name/* + " - " + devIndex*/);
			}
			return a;
		}	

		public static bool Start()
		{
			bool retval = false;
			phase_buf_l = new float[block_size1];
			phase_buf_r = new float[block_size1];
			if(num_channels == 2)
				retval = StartAudio(ref callback1, (uint)block_size1, sample_rate1, host1, input_dev1, output_dev1, num_channels, 0, latency1);
			else
				retval = StartAudio(ref callback4port, (uint)block_size1, sample_rate1, host1, input_dev1, output_dev1, num_channels, 0, latency1);

			if(!retval) return retval;

			if(vac_enabled)
			{
				int num_chan = 1;
				if(vac_stereo) num_chan = 2;
				retval = StartAudio(ref callbackVAC, (uint)block_size2, sample_rate2, host2, input_dev2, output_dev2, num_chan, 1, latency2);
			}

			return retval;
		}


		private static bool using_tfdaph_audio = false; 

		public unsafe static bool StartAudio(ref PA19.PaStreamCallback callback,
			uint block_size, double sample_rate, int host_api_index, int input_dev_index,
			int output_dev_index, int num_channels, int callback_num, int latency_ms)
		{
			System.Console.WriteLine("StartAudio: block_size: " + block_size); 
			System.Console.WriteLine("StartAudio: sample_rate: " + sample_rate); 
			System.Console.WriteLine("StartAudio: num_channels: " + num_channels); 
			System.Console.WriteLine("StartAudio: latency_ms: " + latency_ms); 


			// !!KD5TFD FIXME TODO bug: this check is not right when the channel being started is VAC 
			// if VAC and FPGA audio is being used it sned up in the else branck -- bad! 
			//
			if ( host_api_index < PA19.PA_GetHostApiCount() )  // using classic portaudio 
			{ 
				using_tfdaph_audio = false; 
				return StartAudio_original(ref callback, block_size, sample_rate, host_api_index, input_dev_index, 
					                       output_dev_index, num_channels, callback_num, latency_ms); 
			}
			else  // using kd5tfd/vk6aph audio device 
			{ 
				using_tfdaph_audio = true; 
				int rc; 
				rc = TFDAPHaudio.StartAudio((int)block_size, callback); 
				if ( rc != 0 ) 
				{ 
					System.Console.WriteLine("TFDAPH.StartAudio failed w/ rc: " + rc); 
					return false; 
				} 
				return true; 
			} 
		}

		private unsafe static bool StartAudio_original(ref PA19.PaStreamCallback callback,
			uint block_size, double sample_rate, int host_api_index, int input_dev_index,
			int output_dev_index, int num_channels, int callback_num, int latency_ms)
		{
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
			if ( !using_tfdaph_audio   )  // using classic portaudio 
			{
				StopAudio1_original(); 
			}
			else  // using tfdaph audio 
			{ 
				TFDAPHaudio.StopAudio(); 
			}
		} 
		
		private unsafe static void StopAudio1_original()
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
	}
}
