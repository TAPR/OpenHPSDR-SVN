//=================================================================
// radio.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2009  FlexRadio Systems
// Copyright (C) 2010-2015  Doug Wigley
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;

    #region Radio Class 

	public class Radio
	{
		private const int NUM_RX_THREADS = 2;
		private const int NUM_RX_PER_THREAD = 2;
		private RadioDSPRX[][] dsp_rx;
		private RadioDSPTX[] dsp_tx;

		public Radio()
		{
			RadioDSP.CreateDSP();
            Thread.Sleep(100);

			dsp_rx = new RadioDSPRX[NUM_RX_THREADS][];
			for(int i=0; i<NUM_RX_THREADS; i++)
			{
				dsp_rx[i] = new RadioDSPRX[NUM_RX_PER_THREAD];
				for(int j=0; j<NUM_RX_PER_THREAD; j++)
					dsp_rx[i][j] = new RadioDSPRX((uint)i*2, (uint)j);
			}

			dsp_tx = new RadioDSPTX[1];
			dsp_tx[0] = new RadioDSPTX(1);

			dsp_rx[0][0].Active = true; // enable main RX
		}

		public RadioDSPRX GetDSPRX(int thread, int subrx)
		{
			return dsp_rx[thread][subrx];
		}

		public RadioDSPTX GetDSPTX(int thread)
		{
			return dsp_tx[thread];
		}
	}

	#endregion

	#region RadioDSP Class

	public class RadioDSP
	{
		public static void SyncStatic()
		{
			RadioDSP.SampleRate = RadioDSP.SampleRate;
		}

		#region Static Properties and Routines

		public static void CreateDSP()
		{
            String app_data_path = "";
            app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                //+ "\\FlexRadio Systems\\PowerSDR mRX\\wisdom";
                + "\\FlexRadio Systems\\PowerSDR mRX PS\\";
            wdsp.WDSPwisdom(app_data_path);
            wdsp.OpenChannel(wdsp.id(0, 0), 1024, 4096, 192000, 48000, 48000, 0, 0, 0.010, 0.025, 0.000, 0.010, 0);
            wdsp.OpenChannel(wdsp.id(0, 1), 1024, 4096, 192000, 48000, 48000, 0, 0, 0.010, 0.025, 0.000, 0.010, 0);
            wdsp.OpenChannel(wdsp.id(2, 0), 1024, 4096, 192000, 48000, 48000, 0, 0, 0.010, 0.025, 0.000, 0.010, 0);
            wdsp.OpenChannel(wdsp.id(2, 1), 1024, 4096, 192000, 48000, 48000, 0, 0, 0.010, 0.025, 0.000, 0.010, 0);
            wdsp.OpenChannel(wdsp.id(1, 0), 1024, 4096,  48000, 96000, 48000, 1, 0, 0.010, 0.025, 0.000, 0.010, 0);
            wdsp.create_divEXT(0, 0, 2, 1024);
            wdsp.create_eerEXT(0, 0, 1024, 48000, 1.0, 1.0, false, 0.0, 0.0, 1);
		}

		public static void DestroyDSP()
		{
            wdsp.CloseChannel(wdsp.id(1, 0));
            wdsp.CloseChannel(wdsp.id(2, 1));
            wdsp.CloseChannel(wdsp.id(2, 0));
            wdsp.CloseChannel(wdsp.id(0, 1));
            wdsp.CloseChannel(wdsp.id(0, 0));
            wdsp.destroy_divEXT(0);
            wdsp.destroy_eerEXT(0);
		}

        private static DSPMode rx1_dsp_mode = DSPMode.FIRST;
        public static DSPMode RX1DSPMode
        {
            get { return rx1_dsp_mode; }
            set { rx1_dsp_mode = value; }
        }

        private static DSPMode rx2_dsp_mode = DSPMode.FIRST;
        public static DSPMode RX2DSPMode
        {
            get { return rx2_dsp_mode; }
            set { rx2_dsp_mode = value; }
        }
        
        private static double sample_rate = 48000.0;
        private static int rx1_dsp_rate = 48000;
        private static int rx2_dsp_rate = 48000;
		public static double SampleRate
		{
			get { return sample_rate; }
			set 
			{
                switch (rx1_dsp_mode)
                {
                    case DSPMode.FM:
                        rx1_dsp_rate = 192000;
                        break;
                    default:
                        rx1_dsp_rate = 48000;
                        break;
                }

                switch (rx2_dsp_mode)
                {
                    case DSPMode.FM:
                        rx2_dsp_rate = 192000;
                        break;
                    default:
                        rx2_dsp_rate = 48000;
                        break;
                }
                
                sample_rate = value;
                wdsp.SetAllRates(wdsp.id(0, 0), (int)value, rx1_dsp_rate, 48000);
                wdsp.SetAllRates(wdsp.id(0, 1), (int)value, rx1_dsp_rate, 48000);
                wdsp.SetAllRates(wdsp.id(2, 0), (int)value, rx2_dsp_rate, 48000);
                wdsp.SetAllRates(wdsp.id(2, 1), (int)value, rx2_dsp_rate, 48000);
                wdsp.SetAllRates(wdsp.id(1, 0), (int)value, 96000, 48000);
                puresignal.SetPSFeedbackRate(wdsp.id(1, 0), (int)value);
                wdsp.SetEERSamplerate(0, 48000);
			}		
		}

		//public static void SetThreadNumber(uint num)
		//{
		//	DttSP.SetThreadNo(num);
		//}

		//public static void SetThreadCom(uint num)
		//{
		//	DttSP.SetThreadCom(num);
		//}

		#endregion
	}

	#endregion

	#region RadioDSPRX Class

	public class RadioDSPRX
	{
		private uint thread;
		private uint subrx;

		public RadioDSPRX(uint t, uint rx)
		{
			thread = t;
			subrx = rx;
			//DttSP.SetTRX(t, false);
		}

        public void Copy(RadioDSPRX rx)
        {
            this.AudioSize = rx.audio_size;
            this.DSPMode = rx.dsp_mode;
            this.FilterSize = rx.filter_size;
            this.FilterType = rx.filter_type;
            this.SetRXFilter(rx.rx_filter_low, rx.rx_filter_high);
            this.NoiseReduction = rx.noise_reduction;
            this.SetNRVals(rx.nr_taps, rx.nr_delay, rx.nr_gain, rx.nr_leak);
            this.AutoNotchFilter = rx.auto_notch_filter;
            this.SetANFVals(rx.anf_taps, rx.anf_delay, rx.anf_gain, rx.anf_leak);
            this.RXAGCMode = rx.rx_agc_mode;
            this.RXEQNumBands = rx_eq_num_bands;
            if (this.rx_eq_num_bands == 3)
            {
                this.RXEQ10 = rx.rx_eq10;
                this.RXEQ3 = rx.rx_eq3;
            }
            else
            {
                this.RXEQ3 = rx.rx_eq3;
                this.RXEQ10 = rx.rx_eq10;
            }
            this.RXEQOn = rx.rx_eq_on;
            this.NBOn = rx.nb_on;
            this.NBThreshold = rx.nb_threshold;
            this.NBTau = rx.nb_tau;
            this.NBAdvTime = rx.nb_advtime;
            this.NBHangTime = rx.nb_hangtime;
            this.NBMode = rx.nb_mode;
            this.RXFixedAGC = rx.rx_fixed_agc;
            this.RXAGCMaxGain = rx.rx_agc_max_gain;
            this.RXAGCDecay = rx.rx_agc_decay;
            this.RXAGCHang = rx.rx_agc_hang;
            this.RXOutputGain = rx.rx_output_gain;
            this.RXAGCSlope = rx.rx_agc_slope;
            this.RXAGCHangThreshold = rx.rx_agc_hang_threshold;
            this.BinOn = rx.bin_on;
            this.RXSquelchThreshold = rx.rx_squelch_threshold;
            this.FMSquelchThreshold = rx.fm_squelch_threshold;
            this.RXAMSquelchMaxTail = rx.rx_am_squelch_max_tail;
            this.RXAMSquelchOn = rx.rx_am_squelch_on;
            this.SpectrumPreFilter = rx.spectrum_pre_filter;
            this.Active = rx.active;
            this.Pan = rx.pan;
            this.RXOsc = rx.rx_osc;
            this.RXFMSquelchOn = rx.rx_fm_squelch_on;
            this.RXFMDeviation = rx.rx_fm_deviation;
            this.RXFMCTCSSFilter = rx.rx_fm_ctcss_filter;
            this.RXANFPosition = rx.rx_anf_position;
            this.RXANRPosition = rx.rx_anr_position;
            this.RXCBLRun = rx.rx_cbl_run;
            this.RXAMDFadeLevel = rx.rx_amd_fadelevel;
            this.RXAMDSBMode = rx.rx_amd_sbmode;
            this.RXBandpassWindow = rx.rx_bandpass_window;
            this.RXPreGenRun = rx.rx_pregen_run;
            this.RXPreGenMode = rx.rx_pregen_mode;
            this.RXPreGenToneMag = rx.rx_pregen_tone_mag;
            this.RXPreGenToneFreq = rx.rx_pregen_tone_freq;
            this.RXPreGenNoiseMag = rx.rx_pregen_noise_mag;
            this.RXPreGenSweepMag = rx.rx_pregen_sweep_mag;
            this.RXPreGenSweepFreq1 = rx.rx_pregen_sweep_freq1;
            this.RXPreGenSweepFreq2 = rx.rx_pregen_sweep_freq2;
            this.RXPreGenSweepRate = rx.rx_pregen_sweep_rate;
            this.RXAPFRun = rx.rx_apf_run;
            this.RXAPFFreq = rx.rx_apf_freq;
            this.RXAPFBw = rx.rx_apf_bw;
            this.RXAPFGain = rx.rx_apf_gain;
            this.RXADollyRun = rx.rx_dolly_run;
            this.RXADollyFreq0 = rx.rx_dolly_freq0;
            this.RXADollyFreq1 = rx.rx_dolly_freq1;
            this.RXANR2GainMethod = rx.rx_nr2_gain_method;
            this.RXANR2NPEMethod = rx.rx_nr2_npe_method;
            this.RXANR2AERun = rx.rx_nr2_ae_run;
            this.RXANR2Run = rx.rx_nr2_run;
            this.RXANR2Position = rx.rx_nr2_position;
        }

		private void SyncAll()
		{
			//BufferSize = buffer_size;
			//SetRXCorrectIQW(rx_correct_iq_w_real, rx_correct_iq_w_imag);
			AudioSize = audio_size;			
			DSPMode = dsp_mode;
            FilterSize = filter_size;
            FilterType = filter_type;
			SetRXFilter(rx_filter_low, rx_filter_high);
            NoiseReduction = noise_reduction;
			SetNRVals(nr_taps, nr_delay, nr_gain, nr_leak);
			AutoNotchFilter = auto_notch_filter;
			SetANFVals(anf_taps, anf_delay, anf_gain, anf_leak);
			RXAGCMode = rx_agc_mode;
			if(rx_eq_num_bands == 3)
			{
				RXEQ10 = rx_eq10;
				RXEQ3 = rx_eq3;
			}
			else
			{
				RXEQ3 = rx_eq3;
				RXEQ10 = rx_eq10;
			}
			RXEQOn = rx_eq_on;
			NBOn = nb_on;
			NBThreshold = nb_threshold;
            NBTau = nb_tau;
            NBAdvTime = nb_advtime;
            NBHangTime = nb_hangtime;
            NBMode = nb_mode;
			RXFixedAGC = rx_fixed_agc;
			RXAGCMaxGain = rx_agc_max_gain;
			RXAGCDecay = rx_agc_decay;
			RXAGCHang = rx_agc_hang;
			RXOutputGain = rx_output_gain;
			RXAGCSlope = rx_agc_slope;
			RXAGCHangThreshold = rx_agc_hang_threshold;
			BinOn = bin_on;
			RXSquelchThreshold = rx_squelch_threshold;
            RXAMSquelchMaxTail = rx_am_squelch_max_tail;
			RXAMSquelchOn = rx_am_squelch_on;
            FMSquelchThreshold = fm_squelch_threshold;
            SpectrumPreFilter = spectrum_pre_filter;
			Active = active;
			Pan = pan;
			RXOsc = rx_osc;
            RXFMDeviation = rx_fm_deviation;
            RXFMCTCSSFilter = rx_fm_ctcss_filter;
            RXANFPosition = rx_anf_position;
            RXANRPosition = rx_anr_position;
            RXCBLRun = rx_cbl_run;
            RXAMDFadeLevel = rx_amd_fadelevel;
            RXAMDSBMode = rx_amd_sbmode;
            RXBandpassWindow = rx_bandpass_window;
            RXPreGenRun = rx_pregen_run;
            RXPreGenMode = rx_pregen_mode;
            RXPreGenToneMag = rx_pregen_tone_mag;
            RXPreGenToneFreq = rx_pregen_tone_freq;
            RXPreGenNoiseMag = rx_pregen_noise_mag;
            RXPreGenSweepMag = rx_pregen_sweep_mag;
            RXPreGenSweepFreq1 = rx_pregen_sweep_freq1;
            RXPreGenSweepFreq2 = rx_pregen_sweep_freq2;
            RXPreGenSweepRate = rx_pregen_sweep_rate;
            RXAPFRun = rx_apf_run;
            RXAPFFreq = rx_apf_freq;
            RXAPFBw = rx_apf_bw;
            RXAPFGain = rx_apf_gain;
            RXADollyRun = rx_dolly_run;
            RXADollyFreq0 = rx_dolly_freq0;
            RXADollyFreq1 = rx_dolly_freq1;
            RXANR2GainMethod = rx_nr2_gain_method;
            RXANR2NPEMethod = rx_nr2_npe_method;
            RXANR2AERun = rx_nr2_ae_run;
            RXANR2Run = rx_nr2_run;
            RXANR2Position = rx_nr2_position;

            // for (uint i = 0; i < 9; i++)
            //     SetNotchOn(i, notch_on[i]);

        }

		#region Non-Static Properties & Routines

		/// <summary>
		/// Controls whether updates to following properties call the DSP.  
		/// Each property uses this value and a copy of the last thing sent to
		/// the DSP to update in a minimal fashion.
		/// </summary>
		private bool update = false;
		public bool Update
		{
			get { return update; }
			set
			{
				update = value;
				if(value) SyncAll();
			}
		}

		/// <summary>
		/// Used to force properties to update even if the DSP copy matches the
		/// new setting.  Mainly used to resync the DSP after having to rebuild
		/// when resetting DSP Block Size or Sample Rate.
		/// </summary>
		private bool force = false;
		public bool Force
		{
			get { return force; }
			set { force = value; }
		}

		private int buffer_size_dsp = 64;
		private int buffer_size = 64;
		public int BufferSize
		{
			get { return buffer_size; }
			set
			{				
				buffer_size = value;
				if(update)
				{
					if(value != buffer_size_dsp || force)
					{
                        wdsp.SetDSPBuffsize(wdsp.id(thread, subrx), value);
						buffer_size_dsp = value;
					}
				}
			}
		}

        private int filter_size_dsp = 2048;
        private int filter_size = 2048;
        public int FilterSize
        {
            get { return filter_size; }
            set
            {
                filter_size = value;
                if (update)
                {
                    if (value != filter_size_dsp || force)
                    {
                        wdsp.RXASetNC(wdsp.id(thread, subrx), value);
                        filter_size_dsp = value;
                    }
                }
            }
        }

        private DSPFilterType filter_type_dsp = DSPFilterType.Linear_Phase;
        private DSPFilterType filter_type = DSPFilterType.Linear_Phase;
        public DSPFilterType FilterType
        {
            get { return filter_type; }
            set
            {
                filter_type = value;
                if (update)
                {
                    if (value != filter_type_dsp || force)
                    {
                        wdsp.RXASetMP(wdsp.id(thread, subrx), Convert.ToBoolean(value));
                        filter_type_dsp = value;
                    }
                }
            }
        }

        private int audio_size_dsp = 1024;
		private int audio_size = 1024;
		public int AudioSize
		{
			get { return audio_size; }
			set
			{
				audio_size = value;
				if(update)
				{
					if(value != audio_size_dsp || force)
					{
                        wdsp.SetInputBuffsize(wdsp.id(thread, subrx), value);
                       // wdsp.SetInputBuffsize(wdsp.id(0, 1), value);
                       // wdsp.SetInputBuffsize(wdsp.id(2, 0), value);
                       // wdsp.SetInputBuffsize(wdsp.id(2, 1), value);
                       // wdsp.SetInputBuffsize(wdsp.id(1, 0), value);
						audio_size_dsp = value;
					}
				}
			}
		}

		private DSPMode dsp_mode_dsp = DSPMode.USB;
		private DSPMode dsp_mode = DSPMode.USB;
		public DSPMode DSPMode
		{
			get { return dsp_mode; }
			set
			{
				dsp_mode = value;
				if(update)
				{
					if(value != dsp_mode_dsp || force)
					{
                        wdsp.SetRXAMode(wdsp.id(thread, subrx), value);
						dsp_mode_dsp = value;
					}
				}
			}
		}

		public void SetRXFilter(int low, int high)
		{
			rx_filter_low = low;
			rx_filter_high = high;
			if(update)
			{
				if(low != rx_filter_low_dsp || high != rx_filter_high_dsp || force)
				{
                    wdsp.RXANBPSetFreqs(wdsp.id(thread, subrx), low, high);
                    wdsp.SetRXABandpassFreqs(wdsp.id(thread, subrx), low, high);
                    wdsp.SetRXASNBAOutputBandwidth(wdsp.id(thread, subrx), low, high);
					rx_filter_low_dsp = low;
					rx_filter_high_dsp = high;
				}
			}
		}

		private int rx_filter_low_dsp;
		private int rx_filter_low;
		public int RXFilterLow
		{
			get { return rx_filter_low; }
			set 
			{
				rx_filter_low = value;
				if(update)
				{
					if(value != rx_filter_low_dsp || force)
					{
                        wdsp.RXANBPSetFreqs(wdsp.id(thread, subrx), value, rx_filter_high);
                        wdsp.SetRXABandpassFreqs(wdsp.id(thread, subrx), value, rx_filter_high);
                        wdsp.SetRXASNBAOutputBandwidth(wdsp.id(thread, subrx), value, rx_filter_high);
						rx_filter_low_dsp = value;
					}
				}
			}
		}

		private int rx_filter_high_dsp;
		private int rx_filter_high;
		public int RXFilterHigh
		{
			get { return rx_filter_high; }
			set
			{
				rx_filter_high = value;
				if(update)
				{
					if(value != rx_filter_high_dsp || force)
					{
                        wdsp.RXANBPSetFreqs(wdsp.id(thread, subrx), rx_filter_low, value);
                        wdsp.SetRXABandpassFreqs(wdsp.id(thread, subrx), rx_filter_low, value);
                        wdsp.SetRXASNBAOutputBandwidth(wdsp.id(thread, subrx), rx_filter_low, value);
						rx_filter_high_dsp = value;
					}
				}
			}
		}

		private bool noise_reduction_dsp = false;
		private bool noise_reduction = false;
		public bool NoiseReduction
		{
			get { return noise_reduction; }
			set
			{
				noise_reduction = value;
				if(update)
				{
					if(value != noise_reduction_dsp || force)
					{
                        wdsp.SetRXAANRRun(wdsp.id(thread, subrx), value);
						noise_reduction_dsp = value;
					}
				}
			}
		}

		private int nr_taps_dsp = 64;
		private int nr_taps = 64;
		private int nr_delay_dsp = 16;
		private int nr_delay = 16;
        private double nr_gain_dsp = 16e-4;
        private double nr_gain = 16e-4;
        private double nr_leak_dsp = 10e-7;
        private double nr_leak = 10e-7;
        public void SetNRVals(int taps, int delay, double gain, double leak)
		{
			nr_taps = taps;
			nr_delay = delay;
			nr_gain = gain;
			nr_leak = leak;
			if(update)
			{
				if(taps != nr_taps_dsp || delay != nr_delay_dsp ||
					gain != nr_gain_dsp || leak != nr_leak_dsp || force)
				{
                    wdsp.SetRXAANRVals(wdsp.id(thread, subrx), taps, delay, gain, leak);
					nr_taps_dsp = taps;
					nr_delay_dsp = delay;
					nr_gain_dsp = gain;
					nr_leak_dsp = leak;
				}
			}
		}

		private bool auto_notch_filter_dsp = false;
		private bool auto_notch_filter = false;
		public bool AutoNotchFilter
		{
			get { return auto_notch_filter; }
			set 
			{
				auto_notch_filter = value;
				if(update)
				{
					if(value != auto_notch_filter_dsp || force)
					{
                        wdsp.SetRXAANFRun(wdsp.id(thread, subrx), value);
						auto_notch_filter_dsp = value;
					}
				}
			}
		}

		private int anf_taps_dsp = 64;
		private int anf_taps = 64;
		private int anf_delay_dsp = 16;
		private int anf_delay = 16;
        private double anf_gain_dsp = 10e-4;
        private double anf_gain = 10e-4;
        private double anf_leak_dsp = 1e-7;
        private double anf_leak = 1e-7;
		public void SetANFVals(int taps, int delay, double gain, double leak)
		{
			anf_taps = taps;
			anf_delay = delay;
			anf_gain = gain;
			anf_leak = leak;
			if(update)
			{
				if(taps != anf_taps_dsp || delay != anf_delay_dsp ||
					gain != anf_gain_dsp || leak != anf_leak_dsp || force)
				{
                    wdsp.SetRXAANFVals(wdsp.id(thread, subrx), taps, delay, gain, leak);
					anf_taps_dsp = taps;
					anf_delay_dsp = delay;
					anf_gain_dsp = gain;
					anf_leak_dsp = leak;
				}
			}
		}

		private AGCMode rx_agc_mode_dsp = AGCMode.MED;
		private AGCMode rx_agc_mode = AGCMode.MED;
		public AGCMode RXAGCMode
		{
			get { return rx_agc_mode; }
			set
			{
				rx_agc_mode = value;
				if(update)
				{
					if(value != rx_agc_mode_dsp || force)
					{
                        wdsp.SetRXAAGCMode(wdsp.id(thread, subrx), value);
						rx_agc_mode_dsp = value;
					}
				}
			}
		}

		private int rx_eq_num_bands = 3;
		public int RXEQNumBands
		{
			get { return rx_eq_num_bands; }
			set { rx_eq_num_bands = value; }
		}
        
		private int[] rx_eq3_dsp = new int[4];
		private int[] rx_eq3 = new int[4];
		public int[] RXEQ3
		{
			get { return rx_eq3; }
			set 
			{
				for(int i=0; i<rx_eq3.Length && i<value.Length; i++)
					rx_eq3[i] = value[i];
				if(update)
				{
                    unsafe
                    {
                        fixed (int* ptr = &(rx_eq3[0]))
                            wdsp.SetRXAGrphEQ(wdsp.id(thread, subrx), ptr);
                    }
						for(int i=0; i<rx_eq3_dsp.Length && i<value.Length; i++)
							rx_eq3_dsp[i] = value[i];
				}
			}
		}

		private int[] rx_eq10_dsp = new int[11];
		private int[] rx_eq10 = new int[11];
		public int[] RXEQ10
		{
			get { return rx_eq10; }
			set 
			{
				for(int i=0; i<rx_eq10.Length && i<value.Length; i++)
					rx_eq10[i] = value[i];
				if(update)
				{
                    unsafe
                    {
                        fixed (int* ptr = &(rx_eq10[0]))
                            wdsp.SetRXAGrphEQ10(wdsp.id(thread, subrx), ptr);
                    }
						for(int i=0; i<rx_eq10_dsp.Length && i<value.Length; i++)
							rx_eq10_dsp[i] = value[i];
				}
			}
		}

		private bool rx_eq_on_dsp = false;
		private bool rx_eq_on = false;
		public bool RXEQOn
		{
			get { return rx_eq_on; }
			set 
			{
				rx_eq_on = value;
				if(update)
				{
					if(value != rx_eq_on_dsp || force)
					{
                        wdsp.SetRXAEQRun(wdsp.id(thread, subrx), value);
						rx_eq_on_dsp = value;
					}
				}
			}
		}

		private bool nb_on_dsp = false;
		private bool nb_on = false;
		public bool NBOn
		{
			get { return nb_on; }
			set
			{
				nb_on = value;
				if(update)
				{
					if(value != nb_on_dsp || force)
					{
						//DttSP.SetNB(thread, subrx, value);        // NOT USED
						nb_on_dsp = value;
					}
				}
			}
		}

		private double nb_threshold_dsp = 3.3;
		private double nb_threshold = 3.3;
		public double NBThreshold
		{
			get { return nb_threshold; }
			set
			{
				nb_threshold = value;
				if(update)
				{
					if(value != nb_threshold_dsp || force)
					{
                        if (thread == 0 && subrx == 0)
                            for (int i = 0; i < 3; i++)
                            {
                                SpecHPSDRDLL.SetEXTANBThreshold(i, value);
                                SpecHPSDRDLL.SetEXTNOBThreshold(i, value);
                            }
                        else if (thread == 2 && subrx == 0)
                        {
                            SpecHPSDRDLL.SetEXTANBThreshold(3, value);
                            SpecHPSDRDLL.SetEXTNOBThreshold(3, value);
                        }
						nb_threshold_dsp = value;
					}
				}
			}
		}

        private double nb_tau_dsp = 0.00005;
        private double nb_tau = 0.00005;
        public double NBTau
        {
            get { return nb_tau; }
            set
            {
                nb_tau = value;
                if (update)
                {
                    if (value != nb_tau_dsp || force)
                    {
                        if (thread == 0 && subrx == 0)
                            for (int i = 0; i < 3; i++)
                            {
                                SpecHPSDRDLL.SetEXTANBTau(i, value);
                                SpecHPSDRDLL.SetEXTNOBTau(i, value);
                            }
                        else if (thread == 2 && subrx == 0)
                        {
                            SpecHPSDRDLL.SetEXTANBTau(3, value);
                            SpecHPSDRDLL.SetEXTNOBTau(3, value);
                        }
                        nb_tau_dsp = value;
                    }
                }
            }
        }

        private double nb_advtime_dsp = 0.00005;
        private double nb_advtime = 0.00005;
        public double NBAdvTime
        {
            get { return nb_advtime; }
            set
            {
                nb_advtime = value;
                if (update)
                {
                    if (value != nb_advtime_dsp || force)
                    {
                        if (thread == 0 && subrx == 0)
                            for (int i = 0; i < 3; i++)
                            {
                                SpecHPSDRDLL.SetEXTANBAdvtime(i, value);
                                SpecHPSDRDLL.SetEXTNOBAdvtime(i, value);
                            }
                        else if (thread == 2 && subrx == 0)
                        {
                            SpecHPSDRDLL.SetEXTANBAdvtime(3, value);
                            SpecHPSDRDLL.SetEXTNOBAdvtime(3, value);
                        }
                        nb_advtime_dsp = value;
                    }
                }
            }
        }

        private double nb_hangtime_dsp = 0.00005;
        private double nb_hangtime = 0.00005;
        public double NBHangTime
        {
            get { return nb_hangtime; }
            set
            {
                nb_hangtime = value;
                if (update)
                {
                    if (value != nb_hangtime_dsp || force)
                    {
                        if (thread == 0 && subrx == 0)
                            for (int i = 0; i < 3; i++)
                            {
                                SpecHPSDRDLL.SetEXTANBHangtime(i, value);
                                SpecHPSDRDLL.SetEXTNOBHangtime(i, value);
                            }
                        else if (thread == 2 && subrx == 0)
                        {
                            SpecHPSDRDLL.SetEXTANBHangtime(3, value);
                            SpecHPSDRDLL.SetEXTNOBHangtime(3, value);
                        }
                        nb_hangtime_dsp = value;
                    }
                }
            }
        }

        private int nb_mode_dsp = 0;
        private int nb_mode = 0;
        public int NBMode
        {
            get { return nb_mode; }
            set
            {
                nb_mode = value;
                if (update)
                {
                    if (value != nb_mode_dsp || force)
                    {
                        if (thread == 0 && subrx == 0)
                            for (int i = 0; i < 3; i++)
                                SpecHPSDRDLL.SetEXTNOBMode(i, value);
                        else if (thread == 2 && subrx == 0)
                            SpecHPSDRDLL.SetEXTNOBMode(3, value);
                        nb_mode_dsp = value;
                    }
                }
            }
        }

		private double rx_fixed_agc_dsp = 20.0;
		private double rx_fixed_agc = 20.0;
		public double RXFixedAGC
		{
			get { return rx_fixed_agc; }
			set
			{
				rx_fixed_agc = value;
				if(update)
				{
					if(value != rx_fixed_agc_dsp || force)
					{
                        wdsp.SetRXAAGCFixed(wdsp.id(thread, subrx), value);
						rx_fixed_agc_dsp = value;
					}
				}
			}
		}

		private double rx_agc_max_gain_dsp = 90.0;
		private double rx_agc_max_gain = 90.0;
		public double RXAGCMaxGain
		{
			get { return rx_agc_max_gain; }
			set
			{
				rx_agc_max_gain = value;
				if(update)
				{
					if(value != rx_agc_max_gain_dsp || force)
					{
                        wdsp.SetRXAAGCTop(wdsp.id(thread, subrx), value);
						rx_agc_max_gain_dsp = value;
					}
				}
			}
		}

		private int rx_agc_decay_dsp = 250;
		private int rx_agc_decay = 250;
		public int RXAGCDecay
		{
			get { return rx_agc_decay; }
			set
			{
				rx_agc_decay = value;
				if(update)
				{
					if(value != rx_agc_decay_dsp || force)
					{
                        wdsp.SetRXAAGCDecay(wdsp.id(thread, subrx), value);
						rx_agc_decay_dsp = value;
					}
				}
			}
		}

		private int rx_agc_hang_dsp = 250;
		private int rx_agc_hang = 250;
		public int RXAGCHang
		{
			get { return rx_agc_hang; }
			set
			{
				rx_agc_hang = value;
				if(update)
				{
					if(value != rx_agc_hang_dsp || force)
					{
                        wdsp.SetRXAAGCHang(wdsp.id(thread, subrx), value);
						rx_agc_hang_dsp = value;
					}
				}
			}
		}

		private double rx_output_gain_dsp = 1.0;
		private double rx_output_gain = 1.0;
		public double RXOutputGain
		{
			get { return rx_output_gain; }
			set
			{
				rx_output_gain = value;
				if(update)
				{
					if(value != rx_output_gain_dsp || force)
					{
                        wdsp.SetRXAPanelGain1(wdsp.id(thread, subrx), value);
						rx_output_gain_dsp = value;
					}
				}
			}
		}

		private int rx_agc_slope_dsp = 0;
		private int rx_agc_slope = 0;
		public int RXAGCSlope
		{
			get { return rx_agc_slope; }
			set
			{
				rx_agc_slope = value;
				if(update)
				{
					if(value != rx_agc_slope_dsp || force)
					{
                        wdsp.SetRXAAGCSlope(wdsp.id(thread, subrx), value);
						rx_agc_slope_dsp = value;
					}
				}
			}
		}

		private int rx_agc_hang_threshold_dsp = 0;
		private int rx_agc_hang_threshold = 0;
		public int RXAGCHangThreshold
		{
			get { return rx_agc_hang_threshold; }
			set
			{
				rx_agc_hang_threshold = value;
				if(update)
				{
					if(value != rx_agc_hang_threshold_dsp || force)
					{
                        wdsp.SetRXAAGCHangThreshold(wdsp.id(thread, subrx), value);
						rx_agc_hang_threshold_dsp = value;
					}
				}
			}
		}

		private bool bin_on_dsp = false;
		private bool bin_on = false;
		public bool BinOn
		{
			get { return bin_on; }
			set
			{
				bin_on = value;
				if(update)
				{
					if(value != bin_on_dsp || force)
					{
                        wdsp.SetRXAPanelBinaural(wdsp.id(thread, subrx), value);
						bin_on_dsp = value;
					}
				}
			}
		}

        private float rx_squelch_threshold_dsp = -150.0f;
        private float rx_squelch_threshold = -150.0f;
        public float RXSquelchThreshold
        {
            get { return rx_squelch_threshold; }
            set
            {
                rx_squelch_threshold = value;
                if (update)
                {
                    if (value != rx_squelch_threshold_dsp || force)
                    {
                        wdsp.SetRXAAMSQThreshold(wdsp.id(thread, subrx), value);
                        rx_squelch_threshold_dsp = value;
                    }
                }
            }
        }
       
        private float fm_squelch_threshold = 1.0f;
        private float fm_squelch_threshold_dsp = 1.0f;
        public float FMSquelchThreshold
        {
            get { return fm_squelch_threshold; }
            set
            {
                fm_squelch_threshold = value;
                if (update)
                    if (value != fm_squelch_threshold_dsp || force)
                    {
                        {
                            wdsp.SetRXAFMSQThreshold(wdsp.id(thread, subrx), value);
                            fm_squelch_threshold_dsp = value;
                        }
                    }
            }
        }

		private bool rx_am_squelch_on_dsp = false;
		private bool rx_am_squelch_on = false;
		public bool RXAMSquelchOn
		{
			get { return rx_am_squelch_on; }
			set
			{
				rx_am_squelch_on = value;
				if(update)
				{
					if(value != rx_am_squelch_on_dsp || force)
					{
                        wdsp.SetRXAAMSQRun(wdsp.id(thread, subrx), value);
						rx_am_squelch_on_dsp = value;
					}
				}
			}
		}

        private bool rx_fm_squelch_on_dsp = false;
        private bool rx_fm_squelch_on = false;
        public bool RXFMSquelchOn
        {
            get { return rx_fm_squelch_on; }
            set
            {
                rx_fm_squelch_on = value;
                if (update)
                {
                    if (value != rx_fm_squelch_on_dsp || force)
                    {
                        wdsp.SetRXAFMSQRun(wdsp.id(thread, subrx), value);
                        rx_fm_squelch_on_dsp = value;
                    }
                }
            }
        }

        private double rx_am_squelch_max_tail_dsp = 1.5;
        private double rx_am_squelch_max_tail = 1.5;
        public double RXAMSquelchMaxTail
        {
            get { return rx_am_squelch_max_tail; }
            set
            {
                rx_am_squelch_max_tail = value;
                if (update)
                {
                    if (value != rx_am_squelch_max_tail_dsp || force)
                    {
                        wdsp.SetRXAAMSQMaxTail(wdsp.id(thread, subrx), value);
                        rx_am_squelch_max_tail_dsp = value;
                    }
                }
            }
        }
 
        private bool spectrum_pre_filter_dsp = true;
		private bool spectrum_pre_filter = true;
		public bool SpectrumPreFilter
		{
			get { return spectrum_pre_filter; }
			set
			{
				spectrum_pre_filter = value;
				if(update)
				{
					if(value != spectrum_pre_filter_dsp || force)
					{
						//Debug.WriteLine(thread+" "+subrx+" SpectrumPreFilter: "+value);
						//DttSP.SetPWSmode(thread, subrx, value);       // NOT USED
						spectrum_pre_filter_dsp = value;
					}
				}
			}
		}

		private bool active_dsp = false;
		private bool active = false;
		public bool Active
		{
			get { return active; }
			set
			{
				active = value;
				if(update)
				{
					if(value != active_dsp || force)
					{
						active_dsp = value;
					}
				}
			}
		}

		private float pan_dsp = 0.5f;
		private float pan = 0.5f;
		public float Pan
		{
			get { return pan; }
			set
			{
				pan = value;
				if(update)
				{
					if(value != pan_dsp || force)
					{
                        wdsp.SetRXAPanelPan (wdsp.id(thread, subrx), (double)value);
						pan_dsp = value;
					}
				}
			}
		}

		private double rx_osc_dsp = 0.0;//11025;
		private double rx_osc = 0.0;//11025;
		public double RXOsc
		{
			get { return rx_osc; }
			set
			{
				rx_osc = value;
				if(update)
				{
					if(value != rx_osc_dsp || force)
					{
                        wdsp.SetRXAShiftFreq(wdsp.id(thread, subrx), -value);
                        wdsp.RXANBPSetShiftFrequency(wdsp.id(thread, subrx), -value);
						rx_osc_dsp = value;
					}
				}
			}
		}

        private double rx_fm_deviation = 5000.0;
        private double rx_fm_deviation_dsp = 5000.0;
        public double RXFMDeviation
        {
            get { return rx_fm_deviation; }
            set
            {
                rx_fm_deviation = value;
                if (update)
                {
                    if (value != rx_fm_deviation_dsp || force)
                    {
                        wdsp.SetRXAFMDeviation(wdsp.id(thread, subrx), value);
                        rx_fm_deviation_dsp = value;
                    }
                }
            }
        }


        private bool[] notch_on = new bool[9];
        private bool[] notch_on_dsp = new bool[9];
        public bool GetNotchOn(int index)
        {
            return notch_on[index];
        }

        public void SetNotchOn(uint index, bool b)
        {
            notch_on[index] = b;
            if (update)
            {
                if (b != notch_on_dsp[index] || force)
                {
                    //DttSP.SetRXManualNotchEnable(thread, subrx, index, b);    // NOT USED
                    notch_on_dsp[index] = b;
                }
            }
        }

        private double[] notch_freq = new double[9];
        private double[] notch_freq_dsp = new double[9];
        public double GetNotchFreq(uint index)
        {
            return notch_freq[index];
        }

        public void SetNotchFreq(uint index, double freq)
        {
            notch_freq[index] = freq;
            if (update)
            {
                if (freq != notch_freq_dsp[index] || force)
                {
                    //DttSP.SetRXManualNotchFreq(thread, subrx, index, freq);   // NOT USED
                    notch_freq_dsp[index] = freq;
                }
            }
        }

        private double[] notch_bw = new double[9];
        private double[] notch_bw_dsp = new double[9];
        public double GetNotchBW(uint index)
        {
            return notch_bw[index];
        }

        /// <summary>
        /// Sets the notch bandwidth
        /// </summary>
        /// <param name="index">index of notch to set</param>
        /// <param name="bw">Bandwidth in Hz</param>
        public void SetNotchBW(uint index, double bw)
        {
            notch_bw[index] = bw;
            if (update)
            {
                if (bw != notch_bw_dsp[index] || force)
                {
                    //DttSP.SetRXManualNotchBW(thread, subrx, index, bw);       // NOT USED
                    notch_bw_dsp[index] = bw;
                }
            }
        }

        private bool rx_fm_ctcss_filter_dsp = true;
        private bool rx_fm_ctcss_filter = true;
        public bool RXFMCTCSSFilter
        {
            get { return rx_fm_ctcss_filter; }
            set
            {
                rx_fm_ctcss_filter = value;
                if (update)
                {
                    if (value != rx_fm_ctcss_filter_dsp || force)
                    {
                        wdsp.SetRXACTCSSRun(wdsp.id(thread, subrx), value);
                        rx_fm_ctcss_filter_dsp = value;
                    }
                }
            }

        }

        private int rx_anf_position_dsp = 1;
        private int rx_anf_position = 1;
        public int RXANFPosition
        {
            get { return rx_anf_position; }
            set
            {
                rx_anf_position = value;
                if (update)
                {
                    if (value != rx_anf_position_dsp || force)
                    {
                        wdsp.SetRXAANFPosition(wdsp.id(thread, subrx), value);
                        rx_anf_position_dsp = value;
                    }
                }
            }
        }

        private int rx_anr_position_dsp = 1;
        private int rx_anr_position = 1;
        public int RXANRPosition
        {
            get { return rx_anr_position; }
            set
            {
                rx_anr_position = value;
                if (update)
                {
                    if (value != rx_anr_position_dsp || force)
                    {
                        wdsp.SetRXAANRPosition(wdsp.id(thread, subrx), value);
                        rx_anr_position_dsp = value;
                    }
                }
            }
        }

        private bool rx_cbl_run_dsp = true;
        private bool rx_cbl_run = true;
        public bool RXCBLRun
        {
            get { return rx_cbl_run; }
            set
            {
                rx_cbl_run = value;
                if (update)
                {
                    if (value != rx_cbl_run_dsp || force)
                    {
                        wdsp.SetRXACBLRun(wdsp.id(thread, subrx), value);
                        rx_cbl_run_dsp = value;
                    }
                }
            }
        }

        private int rx_amd_fadelevel_dsp = 1;
        private int rx_amd_fadelevel = 1;
        public int RXAMDFadeLevel
        {
            get { return rx_amd_fadelevel; }
            set
            {
                rx_amd_fadelevel = value;
                if (update)
                {
                    if (value != rx_amd_fadelevel_dsp || force)
                    {
                        wdsp.SetRXAAMDFadeLevel(wdsp.id(thread, subrx), value);
                        rx_amd_fadelevel_dsp = value;
                    }
                }
            }
        }

        private int rx_amd_sbmode_dsp = 0;
        private int rx_amd_sbmode = 0;
        public int RXAMDSBMode
        {
            get { return rx_amd_sbmode; }
            set
            {
                rx_amd_sbmode = value;
                if (update)
                {
                    if (value != rx_amd_sbmode_dsp || force)
                    {
                        wdsp.SetRXAAMDSBMode(wdsp.id(thread, subrx), value);
                        rx_amd_sbmode_dsp = value;
                    }
                }
            }
        }

        private int rx_bandpass_window_dsp = 0;
        private int rx_bandpass_window = 0;
        public int RXBandpassWindow
        {
            get { return rx_bandpass_window; }
            set
            {
                rx_bandpass_window = value;
                if (update)
                {
                    if (value != rx_bandpass_window_dsp || force)
                    {
                        wdsp.RXANBPSetWindow(wdsp.id(thread, subrx), value);
                        wdsp.SetRXABandpassWindow(wdsp.id(thread, subrx), value);
                        rx_bandpass_window_dsp = value;
                    }
                }
            }
        }

        private int rx_pregen_run_dsp = 0;
        private int rx_pregen_run = 0;
        public int RXPreGenRun
        {
            get { return rx_pregen_run; }
            set
            {
                rx_pregen_run = value;
                if (update)
                {
                    if (value != rx_pregen_run_dsp || force)
                    {
                        wdsp.SetRXAPreGenRun(wdsp.id(thread,subrx), value);
                        rx_pregen_run_dsp = value;
                    }
                }
            }
        }

        private int rx_pregen_mode_dsp = 0;
        private int rx_pregen_mode = 0;
        public int RXPreGenMode
        {
            get { return rx_pregen_mode; }
            set
            {
                rx_pregen_mode = value;
                if (update)
                {
                    if (value != rx_pregen_mode_dsp || force)
                    {
                        wdsp.SetRXAPreGenMode(wdsp.id(thread, subrx), value);
                        rx_pregen_mode_dsp = value;
                    }
                }
            }
        }

        private double rx_pregen_tone_mag_dsp = 0.0;
        private double rx_pregen_tone_mag = 0.0;
        public double RXPreGenToneMag
        {
            get { return rx_pregen_tone_mag; }
            set
            {
                rx_pregen_tone_mag = value;
                if (update)
                {
                    if (value != rx_pregen_tone_mag_dsp || force)
                    {
                        wdsp.SetRXAPreGenToneMag(wdsp.id(thread, subrx), value);
                        rx_pregen_tone_mag_dsp = value;
                    }
                }
            }
        }

        private double rx_pregen_tone_freq_dsp = 0.0;
        private double rx_pregen_tone_freq = 0.0;
        public double RXPreGenToneFreq
        {
            get { return rx_pregen_tone_freq; }
            set
            {
                rx_pregen_tone_freq = value;
                if (update)
                {
                    if (value != rx_pregen_tone_freq_dsp || force)
                    {
                        wdsp.SetRXAPreGenToneFreq(wdsp.id(thread, subrx), value);
                        rx_pregen_tone_freq_dsp = value;
                    }
                }
            }
        }

        private double rx_pregen_noise_mag_dsp = 0.0;
        private double rx_pregen_noise_mag = 0.0;
        public double RXPreGenNoiseMag
        {
            get { return rx_pregen_noise_mag; }
            set
            {
                rx_pregen_noise_mag = value;
                if (update)
                {
                    if (value != rx_pregen_noise_mag_dsp || force)
                    {
                        wdsp.SetRXAPreGenNoiseMag(wdsp.id(thread, subrx), value);
                        rx_pregen_noise_mag_dsp = value;
                    }
                }
            }
        }

        private double rx_pregen_sweep_mag_dsp = 0.0;
        private double rx_pregen_sweep_mag = 0.0;
        public double RXPreGenSweepMag
        {
            get { return rx_pregen_sweep_mag; }
            set
            {
                rx_pregen_sweep_mag = value;
                if (update)
                {
                    if (value != rx_pregen_sweep_mag_dsp || force)
                    {
                        wdsp.SetRXAPreGenSweepMag(wdsp.id(thread, subrx), value);
                        rx_pregen_sweep_mag_dsp = value;
                    }
                }
            }
        }

        private double rx_pregen_sweep_freq1_dsp = 0.0;
        private double rx_pregen_sweep_freq1 = 0.0;
        public double RXPreGenSweepFreq1
        {
            get { return rx_pregen_sweep_freq1; }
            set
            {
                rx_pregen_sweep_freq1 = value;
                if (update)
                {
                    if (value != rx_pregen_sweep_freq1_dsp || force)
                    {
                        rx_pregen_sweep_freq1_dsp = value;
                        wdsp.SetRXAPreGenSweepFreq(wdsp.id(thread, subrx), rx_pregen_sweep_freq1_dsp, rx_pregen_sweep_freq2_dsp);
                        
                    }
                }
            }
        }

        private double rx_pregen_sweep_freq2_dsp = 0.0;
        private double rx_pregen_sweep_freq2 = 0.0;
        public double RXPreGenSweepFreq2
        {
            get { return rx_pregen_sweep_freq2; }
            set
            {
                rx_pregen_sweep_freq2 = value;
                if (update)
                {
                    if (value != rx_pregen_sweep_freq2_dsp || force)
                    {
                        rx_pregen_sweep_freq2_dsp = value;
                        wdsp.SetRXAPreGenSweepFreq(wdsp.id(thread, subrx), rx_pregen_sweep_freq1_dsp, rx_pregen_sweep_freq2_dsp);
                    }
                }
            }
        }

        private double rx_pregen_sweep_rate_dsp = 0.0;
        private double rx_pregen_sweep_rate = 0.0;
        public double RXPreGenSweepRate
        {
            get { return rx_pregen_sweep_rate; }
            set
            {
                rx_pregen_sweep_rate = value;
                if (update)
                {
                    if (value != rx_pregen_sweep_rate_dsp || force)
                    {
                        wdsp.SetRXAPreGenSweepRate(wdsp.id(thread, subrx), value);
                        rx_pregen_sweep_rate_dsp = value;
                    }
                }
            }
        }

        private bool rx_apf_run_dsp = false;
        private bool rx_apf_run = false;
        public bool RXAPFRun
        {
            get { return rx_apf_run; }
            set
            {
                rx_apf_run = value;
                if (update)
                {
                    if (value != rx_apf_run_dsp || force)
                    {
                        wdsp.SetRXASPCWRun(wdsp.id(thread, subrx), value);
                        rx_apf_run_dsp = value;
                    }
                }
            }
        }

        private double rx_apf_freq_dsp = 600.0;
        private double rx_apf_freq = 600.0;
        public double RXAPFFreq
        {
            get { return rx_apf_freq; }
            set
            {
                rx_apf_freq = value;
                if (update)
                {
                    if (value != rx_apf_freq_dsp || force)
                    {
                        wdsp.SetRXASPCWFreq(wdsp.id(thread, subrx), value);
                        rx_apf_freq_dsp = value;
                    }
                }
            }
        }

        private double rx_apf_bw_dsp = 600.0;
        private double rx_apf_bw = 600.0;
        public double RXAPFBw
        {
            get { return rx_apf_bw; }
            set
            {
                rx_apf_bw = value;
                if (update)
                {
                    if (value != rx_apf_bw_dsp || force)
                    {
                        wdsp.SetRXASPCWBandwidth(wdsp.id(thread, subrx), value);
                        rx_apf_bw_dsp = value;
                    }
                }
            }
        }

        private double rx_apf_gain_dsp = 1.0;
        private double rx_apf_gain = 1.0;
        public double RXAPFGain
        {
            get { return rx_apf_gain; }
            set
            {
                rx_apf_gain = value;
                if (update)
                {
                    if (value != rx_apf_gain_dsp || force)
                    {
                        wdsp.SetRXASPCWGain(wdsp.id(thread, subrx), value);
                        rx_apf_gain_dsp = value;
                    }
                }
            }
        }

        private bool rx_dolly_run_dsp = false;
        private bool rx_dolly_run = false;
        public bool RXADollyRun
        {
            get { return rx_dolly_run; }
            set
            {
                rx_dolly_run = value;
                if (update)
                {
                    if (value != rx_dolly_run_dsp || force)
                    {
                        wdsp.SetRXAmpeakRun(wdsp.id(thread, subrx), value);
                        rx_dolly_run_dsp = value;
                    }
                }
            }
        }

        private double rx_dolly_freq0_dsp = 2125.0;
        private double rx_dolly_freq0 = 2125.0;
        public double RXADollyFreq0
        {
            get { return rx_dolly_freq0; }
            set
            {
                rx_dolly_freq0 = value;
                if (update)
                {
                    if (value != rx_dolly_freq0_dsp || force)
                    {
                        wdsp.SetRXAmpeakFilFreq(wdsp.id(thread, subrx), 0, value);
                        rx_dolly_freq0_dsp = value;
                    }
                }
            }
        }

        private double rx_dolly_freq1_dsp = 2295.0;
        private double rx_dolly_freq1 = 2295.0;
        public double RXADollyFreq1
        {
            get { return rx_dolly_freq1; }
            set
            {
                rx_dolly_freq1 = value;
                if (update)
                {
                    if (value != rx_dolly_freq1_dsp || force)
                    {
                        wdsp.SetRXAmpeakFilFreq(wdsp.id(thread, subrx), 1, value);
                        rx_dolly_freq1_dsp = value;
                    }
                }
            }
        }

        private int rx_nr2_gain_method = 2;
        private int rx_nr2_gain_method_dsp = 2;
        public int RXANR2GainMethod
        {
            get { return rx_nr2_gain_method; }
            set
            {
                rx_nr2_gain_method = value;
                if (update)
                {
                    if (value != rx_nr2_gain_method_dsp || force)
                    {
                        wdsp.SetRXAEMNRgainMethod(wdsp.id(thread, subrx), value);
                        rx_nr2_gain_method_dsp = value;
                    }
                }
            }
        }

        private int rx_nr2_npe_method = 0;
        private int rx_nr2_npe_method_dsp = 0;
        public int RXANR2NPEMethod
        {
            get { return rx_nr2_npe_method; }
            set
            {
                rx_nr2_npe_method = value;
                if (update)
                {
                    if (value != rx_nr2_npe_method_dsp || force)
                    {
                        wdsp.SetRXAEMNRnpeMethod(wdsp.id(thread, subrx), value);
                        rx_nr2_npe_method_dsp = value;
                    }
                }
            }
        }

        private int rx_nr2_ae_run = 1;
        private int rx_nr2_ae_run_dsp = 1;
        public int RXANR2AERun
        {
            get { return rx_nr2_ae_run; }
            set
            {
                rx_nr2_ae_run = value;
                if (update)
                {
                    if (value != rx_nr2_ae_run_dsp || force)
                    {
                        wdsp.SetRXAEMNRaeRun(wdsp.id(thread, subrx), value);
                        rx_nr2_ae_run_dsp = value;
                    }
                }
            }
        }

        private int rx_nr2_run = 0;
        private int rx_nr2_run_dsp = 0;
        public int RXANR2Run
        {
            get { return rx_nr2_run; }
            set
            {
                rx_nr2_run = value;
                if (update)
                {
                    if (value != rx_nr2_run_dsp || force)
                    {
                        wdsp.SetRXAEMNRRun(wdsp.id(thread, subrx), value);
                        rx_nr2_run_dsp = value;
                    }
                }
            }
        }

        private int rx_nr2_position = 1;
        private int rx_nr2_position_dsp = 1;
        public int RXANR2Position
        {
            get { return rx_nr2_position; }
            set
            {
                rx_nr2_position = value;
                if (update)
                {
                    if (value != rx_nr2_position_dsp || force)
                    {
                        wdsp.SetRXAEMNRPosition(wdsp.id(thread, subrx), value);
                        rx_nr2_position_dsp = value;
                    }
                }
            }
        }

		#endregion
	}

	#endregion

	#region RadioDSPTX Class

	public class RadioDSPTX
	{
		private uint thread;

		public RadioDSPTX(uint t)
		{
			thread = t;
			//DttSP.SetTRX(t, true);
		}

		private void SyncAll()
		{
			//BufferSize = buffer_size;
			AudioSize = audio_size;			
			CurrentDSPMode = current_dsp_mode;
			SetTXFilter(tx_filter_low, tx_filter_high);
            FilterSize = filter_size;
            FilterType = filter_type;
			TXOsc = tx_osc;
			DCBlock = dc_block;
			if(tx_eq_num_bands == 3)
			{
				TXEQ10 = tx_eq10;
				TXEQ3 = tx_eq3;				
			}
			else
			{
				TXEQ3 = tx_eq3;
				TXEQ10 = tx_eq10;
			}
			TXEQOn = tx_eq_on;
			Notch160 = notch_160;
			TXCorrectIQGain = tx_correct_iq_gain;
			TXAMCarrierLevel = tx_am_carrier_level;
			TXALCDecay = tx_alc_decay;
			TXLevelerMaxGain = tx_leveler_max_gain;
			TXLevelerDecay = tx_leveler_decay;
			TXLevelerOn = tx_leveler_on;
			TXSquelchThreshold = tx_squelch_threshold;
			TXSquelchAttenuate = tx_squelch_attenuate;
			TXSquelchOn = tx_squelch_on;
			TXCompandOn = tx_compand_on;
			TXCompandLevel = tx_compand_level;
            TXOsctrlOn = tx_osctrl_on;
			SpectrumPreFilter = spectrum_pre_filter;
            CTCSSFreqHz = ctcss_freq_hz;
            TXFMDeviation = tx_fm_deviation;
            CTCSSFlag = ctcss_flag;
            TXFMEmphOn = tx_fm_emph_on;
            TXEERModeRun = tx_eer_mode_run;
            TXEERModeAMIQ = tx_eer_mode_am_iq;
            TXEERModeMgain = tx_eer_mode_mgain;
            TXEERModePgain = tx_eer_mode_pgain;
            TXEERModeRunDelays = tx_eer_mode_rundelays;
            TXEERModeMdelay = tx_eer_mode_mdelay;
            TXEERModePdelay = tx_eer_mode_pdelay;
            // TXEERModeSamplerate = tx_eer_mode_samplerate;
            TXBandpassWindow = tx_bandpass_window;
            TXPreGenRun = tx_pregen_run;
            TXPreGenMode = tx_pregen_mode;
            TXPreGenToneMag = tx_pregen_tone_mag;
            TXPreGenToneFreq = tx_pregen_tone_freq;
            TXPreGenNoiseMag = tx_pregen_noise_mag;
            TXPreGenSweepMag = tx_pregen_sweep_mag;
            TXPreGenSweepFreq1 = tx_pregen_sweep_freq1;
            TXPreGenSweepFreq2 = tx_pregen_sweep_freq2;
            TXPreGenSweepRate = tx_pregen_sweep_rate;
            TXPreGenSawtoothMag = tx_pregen_sawtooth_mag;
            TXPreGenSawtoothFreq = tx_pregen_sawtooth_freq;
            TXPreGenTriangleMag = tx_pregen_triangle_mag;
            TXPreGenTriangleFreq = tx_pregen_triangle_freq;
            TXPreGenPulseMag = tx_pregen_pulse_mag;
            TXPreGenPulseFreq = tx_pregen_pulse_freq;
            TXPreGenPulseDutyCycle = tx_pregen_pulse_dutycycle;
            TXPreGenPulseToneFreq = tx_pregen_pulse_tonefreq;
            TXPreGenPulseTransition = tx_pregen_pulse_transition;
            TXPostGenRun = tx_postgen_run;
            TXPostGenMode = tx_postgen_mode;
            TXPostGenToneMag = tx_postgen_tone_mag;
            TXPostGenToneFreq = tx_postgen_tone_freq;
            TXPostGenTTMag1 = tx_postgen_tt_mag1;
            TXPostGenTTMag2 = tx_postgen_tt_mag2;
            TXPostGenTTFreq1 = tx_postgen_tt_freq1;
            TXPostGenTTFreq2 = tx_postgen_tt_freq2;
            TXPostGenSweepMag = tx_postgen_sweep_mag;
            TXPostGenSweepFreq1 = tx_postgen_sweep_freq1;
            TXPostGenSweepFreq2 = tx_postgen_sweep_freq2;
            TXPostGenSweepRate = tx_postgen_sweep_rate;
		}

		#region Non-Static Properties & Routines

		/// <summary>
		/// Controls whether updates to following properties call the DSP.  
		/// Each property uses this value and a copy of the last thing sent to
		/// the DSP to update in a minimal fashion.
		/// </summary>
		private bool update = false;
		public bool Update
		{
			get { return update; }
			set
			{
				update = value;
				if(value) SyncAll();
			}
		}

		/// <summary>
		/// Used to force properties to update even if the DSP copy matches the
		/// new setting.  Mainly used to resync the DSP after having to rebuild
		/// when resetting DSP Block Size or Sample Rate.
		/// </summary>
		private bool force = false;
		public bool Force
		{
			get { return force; }
			set { force = value; }
		}

		private int buffer_size_dsp = 128;
		private int buffer_size = 128;
		public int BufferSize
		{
			get { return buffer_size; }
			set
			{				
				buffer_size = value;
				if(update)
				{
					if(value != buffer_size_dsp || force)
					{
                        wdsp.SetDSPBuffsize(wdsp.id(thread, 0), value);
						buffer_size_dsp = value;
					}
				}
			}
		}


        private int filter_size_dsp = 2048;
        private int filter_size = 2048;
        public int FilterSize
        {
            get { return filter_size; }
            set
            {
                filter_size = value;
                if (update)
                {
                    if (value != filter_size_dsp || force)
                    {
                        wdsp.TXASetNC(wdsp.id(thread, 0), value);
                        filter_size_dsp = value;
                    }
                }
            }
        }

        private DSPFilterType filter_type_dsp = DSPFilterType.Linear_Phase;
        private DSPFilterType filter_type = DSPFilterType.Linear_Phase;
        public DSPFilterType FilterType
        {
            get { return filter_type; }
            set
            {
                filter_type = value;
                if (update)
                {
                    if (value != filter_type_dsp || force)
                    {
                        wdsp.TXASetMP(wdsp.id(thread, 0), Convert.ToBoolean(value));
                        filter_type_dsp = value;
                    }
                }
            }
        }

        private int audio_size_dsp = 2048;
		private int audio_size = 2048;
		public int AudioSize
		{
			get { return audio_size; }
			set
			{
				audio_size = value;
				if(update)
				{
					if(value != audio_size_dsp || force)
					{
                        wdsp.SetInputBuffsize(wdsp.id(thread, 0), value);
						audio_size_dsp = value;
					}
				}
			}
		}

		private DSPMode current_dsp_mode_dsp = DSPMode.USB;
		private DSPMode current_dsp_mode = DSPMode.USB;
		public DSPMode CurrentDSPMode
		{
			get { return current_dsp_mode; }
			set
			{
				current_dsp_mode = value;
				if(update)
				{
					if(value != current_dsp_mode_dsp || force)
					{
                        wdsp.SetTXAMode(wdsp.id(thread, 0), value);
						current_dsp_mode_dsp = value;
					}
				}
			}
		}

		public void SetTXFilter(int low, int high)
		{
			tx_filter_low = low;
			tx_filter_high = high;
			if(update)
			{
				if(low != tx_filter_low_dsp || high != tx_filter_high_dsp || force)
				{
                    wdsp.SetTXABandpassFreqs(wdsp.id(thread, 0), low, high);
					tx_filter_low_dsp = low;
					tx_filter_high_dsp = high;
				}
			}
		}

		private int tx_filter_low_dsp;
		private int tx_filter_low;
		public int TXFilterLow
		{
			get { return tx_filter_low; }
			set 
			{
				tx_filter_low = value;
				if(update)
				{
					if(value != tx_filter_low_dsp || force)
					{
                        wdsp.SetTXABandpassFreqs(wdsp.id(thread, 0), value, tx_filter_high);
						tx_filter_low_dsp = value;
					}
				}
			}
		}

		private int tx_filter_high_dsp;
		private int tx_filter_high;
		public int TXFilterHigh
		{
			get { return tx_filter_high; }
			set
			{
				tx_filter_high = value;
				if(update)
				{
					if(value != tx_filter_high_dsp || force)
					{
                        wdsp.SetTXABandpassFreqs(wdsp.id(thread, 0), tx_filter_low, value);
						tx_filter_high_dsp = value;
					}
				}
			}
		}

		private double tx_osc_dsp = 0.0f;
		private double tx_osc = 0.0;
		public double TXOsc
		{
			get { return tx_osc; }
			set 
			{
				tx_osc = value;
				if(update)
				{
					if(value != tx_osc_dsp || force)
					{
						//DttSP.SetTXOsc(thread, value);    // NOT USED!
						tx_osc_dsp = value;
					}
				}
			}
		}

		private bool dc_block_dsp;
		private bool dc_block;
		public bool DCBlock
		{
			get { return dc_block; }
			set
			{
				dc_block = value;
				if(update)
				{
					if(value != dc_block_dsp || force)
					{
						//DttSP.SetTXDCBlock(thread, value);        // NOT USED
						dc_block_dsp = value;
					}
				}
			}
		}

		private int tx_eq_num_bands = 3;
		public int TXEQNumBands
		{
			get { return tx_eq_num_bands; }
			set { tx_eq_num_bands = value; }
		}

		private int[] tx_eq3_dsp = new int[4];
		private int[] tx_eq3 = new int[4];
		public int[] TXEQ3
		{
			get { return tx_eq3; }
			set
			{
				for(int i=0; i<tx_eq3.Length && i<value.Length; i++)
					tx_eq3[i] = value[i];
				if(update)
				{
                    unsafe
                    {
                        fixed (int* ptr = &(tx_eq3[0]))
                            wdsp.SetTXAGrphEQ(wdsp.id(thread, 0), ptr);
                    }
						for(int i=0; i<tx_eq3_dsp.Length && i<value.Length; i++)
							tx_eq3_dsp[i] = value[i];
				}
			}
		}

		private int[] tx_eq10_dsp = new int[11];
		private int[] tx_eq10 = new int[11];
		public int[] TXEQ10
		{
			get { return tx_eq10; }
			set
			{
				for(int i=0; i<tx_eq10.Length && i<value.Length; i++)
					tx_eq10[i] = value[i];
				if(update)
				{
                    unsafe
                    {
                        fixed (int* ptr = &(tx_eq10[0]))
                            wdsp.SetTXAGrphEQ10(wdsp.id(thread, 0), ptr);
                    }
						for(int i=0; i<tx_eq10_dsp.Length && i<value.Length; i++)
							tx_eq10_dsp[i] = value[i];
				}
			}
		}

		private bool tx_eq_on_dsp = false;
		private bool tx_eq_on = false;
		public bool TXEQOn
		{
			get { return tx_eq_on; }
			set
			{
				tx_eq_on = value;
				if(update)
				{
					if(value != tx_eq_on_dsp || force)
					{
                        wdsp.SetTXAEQRun(wdsp.id(thread, 0), value);
						tx_eq_on_dsp = value;
					}
				}
			}
		}

		private bool notch_160_dsp = false;
		private bool notch_160 = false;
		public bool Notch160
		{
			get { return notch_160; }
			set
			{
				notch_160 = value;
				if(update)
				{
					if(value != notch_160_dsp || force)
					{
						//DttSP.SetNotch160(thread, value);     // NOT USED
						notch_160_dsp = value;
					}
				}
			}
		}

		private double tx_correct_iq_gain_dsp = 0.0;
		private double tx_correct_iq_gain = 0.0;
		public double TXCorrectIQGain
		{
			get { return tx_correct_iq_gain; }
			set
			{
				tx_correct_iq_gain = value;
				if(update)
				{
					if(value != tx_correct_iq_gain_dsp || force)
					{
						//DttSP.SetTXIQGain(thread, value);     // NOT USED
						tx_correct_iq_gain_dsp = value;
					}
				}
			}
		}

        private double tx_fm_deviation = 5000.0;
        private double tx_fm_deviation_dsp = 5000.0;
        public double TXFMDeviation
        {
            get { return tx_fm_deviation; }
            set
            {
                tx_fm_deviation = value;
                if (update)
                {
                    if (value != tx_fm_deviation_dsp || force)
                    {
                        wdsp.SetTXAFMDeviation(wdsp.id(thread, 0), value);
                        tx_fm_deviation_dsp = value;
                    }
                }
            }
        }

        private double ctcss_freq_hz = 100.0;
        private double ctcss_freq_hz_dsp = 100.0;
        public double CTCSSFreqHz
        {
            get { return ctcss_freq_hz; }
            set
            {
                ctcss_freq_hz = value;
                if (update)
                {
                    if (value != ctcss_freq_hz_dsp || force)
                    {
                        wdsp.SetTXACTCSSFreq(wdsp.id(thread, 0), value);
                        wdsp.SetRXACTCSSFreq(wdsp.id(0, 0), value);
                        wdsp.SetRXACTCSSFreq(wdsp.id(0, 1), value);
                        wdsp.SetRXACTCSSFreq(wdsp.id(2, 0), value);
                        ctcss_freq_hz_dsp = value;
                    }
                }
            }
        }

        private bool ctcss_flag = false;
        private bool ctcss_flag_dsp = false;
        public bool CTCSSFlag
        {
            get { return ctcss_flag; }
            set
            {
                ctcss_flag = value;
                if (update)
                    if (value != ctcss_flag_dsp || force)
                    {
                        {
                            wdsp.SetTXACTCSSRun(wdsp.id(thread, 0), value);
                            ctcss_flag_dsp = value;
                        }
                    }
            }
        }

		private double tx_am_carrier_level_dsp = 0.4;
		private double tx_am_carrier_level = 0.4;
		public double TXAMCarrierLevel
		{
			get { return tx_am_carrier_level; }
			set 
			{
				tx_am_carrier_level = value;
				if(update)
				{
					if(value != tx_am_carrier_level_dsp || force)
					{
                        wdsp.SetTXAAMCarrierLevel(wdsp.id(thread, 0), value);
						tx_am_carrier_level_dsp = value;
					}
				}
			}
		}

		private int tx_alc_decay_dsp = 10;
		private int tx_alc_decay = 10;
		public int TXALCDecay
		{
			get { return tx_alc_decay; }
			set
			{
				tx_alc_decay = value;
				if(update)
				{
					if(value != tx_alc_decay_dsp || force)
					{
                        wdsp.SetTXAALCDecay(wdsp.id(thread, 0), value);
						tx_alc_decay_dsp = value;
					}
				}
			}
		}

		private double tx_leveler_max_gain_dsp = 15.0;
		private double tx_leveler_max_gain = 15.0;
		public double TXLevelerMaxGain
		{
			get { return tx_leveler_max_gain; }
			set
			{
				tx_leveler_max_gain = value;
				if(update)
				{
					if(value != tx_leveler_max_gain_dsp || force)
					{
                        wdsp.SetTXALevelerTop(wdsp.id(thread, 0), value);
						tx_leveler_max_gain_dsp = value;
					}
				}
			}
		}


		private int tx_leveler_decay_dsp = 500;
		private int tx_leveler_decay = 500;
		public int TXLevelerDecay
		{
			get { return tx_leveler_decay; }
			set
			{
				tx_leveler_decay = value;
				if(update)
				{
					if(value != tx_leveler_decay_dsp || force)
					{
                        wdsp.SetTXALevelerDecay(wdsp.id(thread, 0), value);
						tx_leveler_decay_dsp = value;
					}
				}
			}
		}

		private bool tx_leveler_on_dsp = true;
		private bool tx_leveler_on = true;
		public bool TXLevelerOn
		{
			get { return tx_leveler_on; }
			set
			{
				tx_leveler_on = value;
				if(update)
				{
					if(value != tx_leveler_on_dsp || force)
					{
                        wdsp.SetTXALevelerSt(wdsp.id(thread, 0), value);
						tx_leveler_on_dsp = value;
					}
				}
			}
		}

		private float tx_squelch_threshold_dsp = -40.0f;
		private float tx_squelch_threshold = -40.0f;
		public float TXSquelchThreshold
		{
			get { return tx_squelch_threshold; }
			set
			{
				tx_squelch_threshold = value;
				if(update)
				{
					if(value != tx_squelch_threshold_dsp || force)
					{
                        wdsp.SetTXAAMSQThreshold(wdsp.id(thread, 0), (double)value);
						tx_squelch_threshold_dsp = value;
					}
				}
			}
		}

		private float tx_squelch_attenuate_dsp = 80.0f;
		private float tx_squelch_attenuate = 80.0f;
		public float TXSquelchAttenuate
		{
			get { return tx_squelch_attenuate; }
			set
			{
				tx_squelch_attenuate = value;
				if(update)
				{
					if(value != tx_squelch_attenuate_dsp || force)
					{
                        wdsp.SetTXAAMSQMutedGain(wdsp.id(thread, 0), 20.0 * Math.Log10(1.0 - value / 100.0));
						tx_squelch_attenuate_dsp = value;
					}
				}
			}
		}

		private bool tx_squelch_on_dsp = false;
		private bool tx_squelch_on = false;
		public bool TXSquelchOn
		{
			get { return tx_squelch_on; }
			set
			{
				tx_squelch_on = value;
				if(update)
				{
					if(value != tx_squelch_on_dsp || force)
					{
                        wdsp.SetTXAAMSQRun(wdsp.id(thread, 0), value);
						tx_squelch_on_dsp = value;
					}
				}
			}
		}

		private bool tx_compand_on_dsp = false;
		private bool tx_compand_on = false;
		public bool TXCompandOn
		{
			get { return tx_compand_on; }
			set
			{
				tx_compand_on = value;

				if(update)
				{
					if(value != tx_compand_on_dsp || force)
					{
                        wdsp.SetTXACompressorRun(wdsp.id(thread, 0), value);
						tx_compand_on_dsp = value;
					}
				}
			}
		}

		private double tx_compand_level_dsp = 0.1;
		private double tx_compand_level = 0.1;
		public double TXCompandLevel
		{
			get { return tx_compand_level; }
			set
			{
				tx_compand_level = value;

				if(update)
				{
					if(value != tx_compand_level_dsp || force)
					{
                        wdsp.SetTXACompressorGain(wdsp.id(thread, 0), value);
						tx_compand_level_dsp = value;
					}
				}
			}
		}

        private bool tx_osctrl_on_dsp = false;
        private bool tx_osctrl_on = false;
        public bool TXOsctrlOn
        {
            get { return tx_osctrl_on; }
            set
            {
                tx_osctrl_on = value;

                if (update)
                {
                    if (value != tx_osctrl_on_dsp || force)
                    {
                        wdsp.SetTXAosctrlRun(wdsp.id(thread, 0), value);
                        tx_osctrl_on_dsp = value;
                    }
                }
            }
        }

		private bool spectrum_pre_filter_dsp = true;
		private bool spectrum_pre_filter = true;
		public bool SpectrumPreFilter
		{
			get { return spectrum_pre_filter; }
			set
			{
				spectrum_pre_filter = value;
				if(update)
				{
					if(value != spectrum_pre_filter_dsp || force)
					{
						//DttSP.SetTXPWSmode(thread, value);        // NOT USED
						spectrum_pre_filter_dsp = value;
					}
				}
			}
		}

        private bool tx_fm_emph_on_dsp = true;
        private bool tx_fm_emph_on = true;
        public bool TXFMEmphOn
        {
            get { return tx_fm_emph_on; }
            set
            {
                tx_fm_emph_on = value;

                if (update)
                {
                    if (value != tx_fm_emph_on_dsp || force)
                    {
                        wdsp.SetTXAFMEmphPosition(wdsp.id(thread, 0), value);
                        tx_fm_emph_on_dsp = value;
                    }
                }
            }
        }

        private bool tx_eer_mode_run_dsp = false;
        private bool tx_eer_mode_run = false;
        public bool TXEERModeRun
        {
            get { return tx_eer_mode_run; }
            set
            {
                tx_eer_mode_run = value;

                if (update)
                {
                    if (value != tx_eer_mode_run_dsp || force)
                    {
                        wdsp.SetEERRun(0, value);
                        tx_eer_mode_run_dsp = value;
                    }
                }
            }
        }

        private bool tx_eer_mode_am_iq_dsp = true;
        private bool tx_eer_mode_am_iq = true;
        public bool TXEERModeAMIQ
        {
            get { return tx_eer_mode_am_iq; }
            set
            {
                tx_eer_mode_am_iq = value;

                if (update)
                {
                    if (value != tx_eer_mode_am_iq_dsp || force)
                    {
                        wdsp.SetEERAMIQ(0, value);
                        tx_eer_mode_am_iq_dsp = value;
                    }
                }
            }
        }

        private double tx_eer_mode_mgain_dsp = 0.5;
        private double tx_eer_mode_mgain = 0.5;
        public double TXEERModeMgain
        {
            get { return tx_eer_mode_mgain; }
            set
            {
                tx_eer_mode_mgain = value;

                if (update)
                {
                    if (value != tx_eer_mode_mgain_dsp || force)
                    {
                        wdsp.SetEERMgain(0, value);
                        tx_eer_mode_mgain_dsp = value;
                    }
                }
            }
        }

        private double tx_eer_mode_pgain_dsp = 0.5;
        private double tx_eer_mode_pgain = 0.5;
        public double TXEERModePgain
        {
            get { return tx_eer_mode_pgain; }
            set
            {
                tx_eer_mode_pgain = value;

                if (update)
                {
                    if (value != tx_eer_mode_pgain_dsp || force)
                    {
                        wdsp.SetEERPgain(0, value);
                        tx_eer_mode_pgain_dsp = value;
                    }
                }
            }
        }

        private bool tx_eer_mode_rundelays_dsp = true;
        private bool tx_eer_mode_rundelays = true;
        public bool TXEERModeRunDelays
        {
            get { return tx_eer_mode_rundelays; }
            set
            {
                tx_eer_mode_rundelays = value;

                if (update)
                {
                    if (value != tx_eer_mode_rundelays_dsp || force)
                    {
                        wdsp.SetEERRunDelays(0, value);
                        tx_eer_mode_rundelays_dsp = value;
                    }
                }
            }
        }

        private double tx_eer_mode_mdelay_dsp = 0.0;
        private double tx_eer_mode_mdelay = 0.0;
        public double TXEERModeMdelay
        {
            get { return tx_eer_mode_mdelay; }
            set
            {
                tx_eer_mode_mdelay = value;

                if (update)
                {
                    if (value != tx_eer_mode_mdelay_dsp || force)
                    {
                        wdsp.SetEERMdelay(0, value);
                        tx_eer_mode_mdelay_dsp = value;
                    }
                }
            }
        }

        private double tx_eer_mode_pdelay_dsp = 0.0;
        private double tx_eer_mode_pdelay = 0.0;
        public double TXEERModePdelay
        {
            get { return tx_eer_mode_pdelay; }
            set
            {
                tx_eer_mode_pdelay = value;

                if (update)
                {
                    if (value != tx_eer_mode_pdelay_dsp || force)
                    {
                        wdsp.SetEERPdelay(0, value);
                        tx_eer_mode_pdelay_dsp = value;
                    }
                }
            }
        }

        private int tx_eer_mode_samplerate_dsp = 48000;
        private int tx_eer_mode_samplerate = 48000;
        public int TXEERModeSamplerate
        {
            get { return tx_eer_mode_samplerate; }
            set
            {
                tx_eer_mode_samplerate = value;

                if (update)
                {
                    if (value != tx_eer_mode_samplerate_dsp || force)
                    {
                        wdsp.SetEERSamplerate(0, value);
                        tx_eer_mode_samplerate_dsp = value;
                    }
                }
            }
        }

        private int tx_bandpass_window_dsp = 1;
        private int tx_bandpass_window = 1;
        public int TXBandpassWindow
        {
            get { return tx_bandpass_window; }
            set
            {
                tx_bandpass_window = value;
                if (update)
                {
                    if (value != tx_bandpass_window_dsp || force)
                    {
                        wdsp.SetTXABandpassWindow(wdsp.id(1, 0), value);
                        tx_bandpass_window_dsp = value;
                    }
                }
            }
        }

        private int tx_pregen_run_dsp = 0;
        private int tx_pregen_run = 0;
        public int TXPreGenRun
        {
            get { return tx_pregen_run; }
            set
            {
                tx_pregen_run = value;
                if (update)
                {
                    if (value != tx_pregen_run_dsp || force)
                    {
                        wdsp.SetTXAPreGenRun(wdsp.id(1, 0), value);
                        tx_pregen_run_dsp = value;
                    }
                }
            }
        }

        private int tx_pregen_mode_dsp = 0;
        private int tx_pregen_mode = 0;
        public int TXPreGenMode
        {
            get { return tx_pregen_mode; }
            set
            {
                tx_pregen_mode = value;
                if (update)
                {
                    if (value != tx_pregen_mode_dsp || force)
                    {
                        wdsp.SetTXAPreGenMode(wdsp.id(1, 0), value);
                        tx_pregen_mode_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_tone_mag_dsp = 0.0;
        private double tx_pregen_tone_mag = 0.0;
        public double TXPreGenToneMag
        {
            get { return tx_pregen_tone_mag; }
            set
            {
                tx_pregen_tone_mag = value;
                if (update)
                {
                    if (value != tx_pregen_tone_mag_dsp || force)
                    {
                        wdsp.SetTXAPreGenToneMag(wdsp.id(1, 0), value);
                        tx_pregen_tone_mag_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_tone_freq_dsp = 0.0;
        private double tx_pregen_tone_freq = 0.0;
        public double TXPreGenToneFreq
        {
            get { return tx_pregen_tone_freq; }
            set
            {
                tx_pregen_tone_freq = value;
                if (update)
                {
                    if (value != tx_pregen_tone_freq_dsp || force)
                    {
                        wdsp.SetTXAPreGenToneFreq(wdsp.id(1, 0), value);
                        tx_pregen_tone_freq_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_noise_mag_dsp = 0.0;
        private double tx_pregen_noise_mag = 0.0;
        public double TXPreGenNoiseMag
        {
            get { return tx_pregen_noise_mag; }
            set
            {
                tx_pregen_noise_mag = value;
                if (update)
                {
                    if (value != tx_pregen_noise_mag_dsp || force)
                    {
                        wdsp.SetTXAPreGenNoiseMag(wdsp.id(1, 0), value);
                        tx_pregen_noise_mag_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_sweep_mag_dsp = 0.0;
        private double tx_pregen_sweep_mag = 0.0;
        public double TXPreGenSweepMag
        {
            get { return tx_pregen_sweep_mag; }
            set
            {
                tx_pregen_sweep_mag = value;
                if (update)
                {
                    if (value != tx_pregen_sweep_mag_dsp || force)
                    {
                        wdsp.SetTXAPreGenSweepMag(wdsp.id(1, 0), value);
                        tx_pregen_sweep_mag_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_sweep_freq1_dsp = 0.0;
        private double tx_pregen_sweep_freq1 = 0.0;
        public double TXPreGenSweepFreq1
        {
            get { return tx_pregen_sweep_freq1; }
            set
            {
                tx_pregen_sweep_freq1 = value;
                if (update)
                {
                    if (value != tx_pregen_sweep_freq1_dsp || force)
                    {
                        tx_pregen_sweep_freq1_dsp = value;
                        wdsp.SetTXAPreGenSweepFreq(wdsp.id(1, 0), tx_pregen_sweep_freq1_dsp, tx_pregen_sweep_freq2_dsp);

                    }
                }
            }
        }

        private double tx_pregen_sweep_freq2_dsp = 0.0;
        private double tx_pregen_sweep_freq2 = 0.0;
        public double TXPreGenSweepFreq2
        {
            get { return tx_pregen_sweep_freq2; }
            set
            {
                tx_pregen_sweep_freq2 = value;
                if (update)
                {
                    if (value != tx_pregen_sweep_freq2_dsp || force)
                    {
                        tx_pregen_sweep_freq2_dsp = value;
                        wdsp.SetTXAPreGenSweepFreq(wdsp.id(1, 0), tx_pregen_sweep_freq1_dsp, tx_pregen_sweep_freq2_dsp);
                    }
                }
            }
        }

        private double tx_pregen_sweep_rate_dsp = 0.0;
        private double tx_pregen_sweep_rate = 0.0;
        public double TXPreGenSweepRate
        {
            get { return tx_pregen_sweep_rate; }
            set
            {
                tx_pregen_sweep_rate = value;
                if (update)
                {
                    if (value != tx_pregen_sweep_rate_dsp || force)
                    {
                        wdsp.SetTXAPreGenSweepRate(wdsp.id(1, 0), value);
                        tx_pregen_sweep_rate_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_sawtooth_mag_dsp = 0.0;
        private double tx_pregen_sawtooth_mag = 0.0;
        public double TXPreGenSawtoothMag
        {
            get { return tx_pregen_sawtooth_mag; }
            set
            {
                tx_pregen_sawtooth_mag = value;
                if (update)
                {
                    if (value != tx_pregen_sawtooth_mag_dsp || force)
                    {
                        wdsp.SetTXAPreGenSawtoothMag(wdsp.id(1, 0), value);
                        tx_pregen_sawtooth_mag_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_sawtooth_freq_dsp = 0.0;
        private double tx_pregen_sawtooth_freq = 0.0;
        public double TXPreGenSawtoothFreq
        {
            get { return tx_pregen_sawtooth_freq; }
            set
            {
                tx_pregen_sawtooth_freq = value;
                if (update)
                {
                    if (value != tx_pregen_sawtooth_freq_dsp || force)
                    {
                        wdsp.SetTXAPreGenSawtoothFreq(wdsp.id(1, 0), value);
                        tx_pregen_sawtooth_freq_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_triangle_mag_dsp = 0.0;
        private double tx_pregen_triangle_mag = 0.0;
        public double TXPreGenTriangleMag
        {
            get { return tx_pregen_triangle_mag; }
            set
            {
                tx_pregen_triangle_mag = value;
                if (update)
                {
                    if (value != tx_pregen_triangle_mag_dsp || force)
                    {
                        wdsp.SetTXAPreGenTriangleMag(wdsp.id(1, 0), value);
                        tx_pregen_triangle_mag_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_triangle_freq_dsp = 0.0;
        private double tx_pregen_triangle_freq = 0.0;
        public double TXPreGenTriangleFreq
        {
            get { return tx_pregen_triangle_freq; }
            set
            {
                tx_pregen_triangle_freq = value;
                if (update)
                {
                    if (value != tx_pregen_triangle_freq_dsp || force)
                    {
                        wdsp.SetTXAPreGenTriangleFreq(wdsp.id(1, 0), value);
                        tx_pregen_triangle_freq_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_pulse_mag_dsp = 0.0;
        private double tx_pregen_pulse_mag = 0.0;
        public double TXPreGenPulseMag
        {
            get { return tx_pregen_pulse_mag; }
            set
            {
                tx_pregen_pulse_mag = value;
                if (update)
                {
                    if (value != tx_pregen_pulse_mag_dsp || force)
                    {
                        wdsp.SetTXAPreGenPulseMag(wdsp.id(1, 0), value);
                        tx_pregen_pulse_mag_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_pulse_freq_dsp = 0.0;
        private double tx_pregen_pulse_freq = 0.0;
        public double TXPreGenPulseFreq
        {
            get { return tx_pregen_pulse_freq; }
            set
            {
                tx_pregen_pulse_freq = value;
                if (update)
                {
                    if (value != tx_pregen_pulse_freq_dsp || force)
                    {
                        wdsp.SetTXAPreGenPulseFreq(wdsp.id(1, 0), value);
                        tx_pregen_pulse_freq_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_pulse_dutycycle_dsp = 0.0;
        private double tx_pregen_pulse_dutycycle = 0.0;
        public double TXPreGenPulseDutyCycle
        {
            get { return tx_pregen_pulse_dutycycle; }
            set
            {
                tx_pregen_pulse_dutycycle = value;
                if (update)
                {
                    if (value != tx_pregen_pulse_dutycycle_dsp || force)
                    {
                        wdsp.SetTXAPreGenPulseDutyCycle(wdsp.id(1, 0), value);
                        tx_pregen_pulse_dutycycle_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_pulse_tonefreq_dsp = 0.0;
        private double tx_pregen_pulse_tonefreq = 0.0;
        public double TXPreGenPulseToneFreq
        {
            get { return tx_pregen_pulse_tonefreq; }
            set
            {
                tx_pregen_pulse_tonefreq = value;
                if (update)
                {
                    if (value != tx_pregen_pulse_tonefreq_dsp || force)
                    {
                        wdsp.SetTXAPreGenPulseToneFreq(wdsp.id(1, 0), value);
                        tx_pregen_pulse_tonefreq_dsp = value;
                    }
                }
            }
        }

        private double tx_pregen_pulse_transition_dsp = 0.0;
        private double tx_pregen_pulse_transition = 0.0;
        public double TXPreGenPulseTransition
        {
            get { return tx_pregen_pulse_transition; }
            set
            {
                tx_pregen_pulse_transition = value;
                if (update)
                {
                    if (value != tx_pregen_pulse_transition_dsp || force)
                    {
                        wdsp.SetTXAPreGenPulseTransition(wdsp.id(1, 0), value);
                        tx_pregen_pulse_transition_dsp = value;
                    }
                }
            }
        }

        private int tx_postgen_run_dsp = 0;
        private int tx_postgen_run = 0;
        public int TXPostGenRun
        {
            get { return tx_postgen_run; }
            set
            {
                tx_postgen_run = value;
                if (update)
                {
                    if (value != tx_postgen_run_dsp || force)
                    {
                        wdsp.SetTXAPostGenRun(wdsp.id(1, 0), value);
                        tx_postgen_run_dsp = value;
                    }
                }
            }
        }

        private int tx_postgen_mode_dsp = 0;
        private int tx_postgen_mode = 0;
        public int TXPostGenMode
        {
            get { return tx_postgen_mode; }
            set
            {
                tx_postgen_mode = value;
                if (update)
                {
                    if (value != tx_postgen_mode_dsp || force)
                    {
                        wdsp.SetTXAPostGenMode(wdsp.id(1, 0), value);
                        tx_postgen_mode_dsp = value;
                    }
                }
            }
        }

        private double tx_postgen_tone_mag_dsp = 0.0;
        private double tx_postgen_tone_mag = 0.0;
        public double TXPostGenToneMag
        {
            get { return tx_postgen_tone_mag; }
            set
            {
                tx_postgen_tone_mag = value;
                if (update)
                {
                    if (value != tx_postgen_tone_mag_dsp || force)
                    {
                        wdsp.SetTXAPostGenToneMag(wdsp.id(1, 0), value);
                        tx_postgen_tone_mag_dsp = value;
                    }
                }
            }
        }

        private double tx_postgen_tone_freq_dsp = 0.0;
        private double tx_postgen_tone_freq = 0.0;
        public double TXPostGenToneFreq
        {
            get { return tx_postgen_tone_freq; }
            set
            {
                tx_postgen_tone_freq = value;
                if (update)
                {
                    if (value != tx_postgen_tone_freq_dsp || force)
                    {
                        wdsp.SetTXAPostGenToneFreq(wdsp.id(1, 0), value);
                        tx_postgen_tone_freq_dsp = value;
                    }
                }
            }
        }

        private double tx_postgen_tt_mag1_dsp = 0.0;
        private double tx_postgen_tt_mag1 = 0.0;
        public double TXPostGenTTMag1
        {
            get { return tx_postgen_tt_mag1; }
            set
            {
                tx_postgen_tt_mag1 = value;
                if (update)
                {
                    if (value != tx_postgen_tt_mag1_dsp || force)
                    {
                        tx_postgen_tt_mag1_dsp = value;
                        wdsp.SetTXAPostGenTTMag(wdsp.id(1, 0), tx_postgen_tt_mag1_dsp, tx_postgen_tt_mag2_dsp);
                    }
                }
            }
        }

        private double tx_postgen_tt_mag2_dsp = 0.0;
        private double tx_postgen_tt_mag2 = 0.0;
        public double TXPostGenTTMag2
        {
            get { return tx_postgen_tt_mag2; }
            set
            {
                tx_postgen_tt_mag2 = value;
                if (update)
                {
                    if (value != tx_postgen_tt_mag2_dsp || force)
                    {
                        tx_postgen_tt_mag2_dsp = value;
                        wdsp.SetTXAPostGenTTMag(wdsp.id(1, 0), tx_postgen_tt_mag1_dsp, tx_postgen_tt_mag2_dsp);
                    }
                }
            }
        }

        private double tx_postgen_tt_freq1_dsp = 0.0;
        private double tx_postgen_tt_freq1 = 0.0;
        public double TXPostGenTTFreq1
        {
            get { return tx_postgen_tt_freq1; }
            set
            {
                tx_postgen_tt_freq1 = value;
                if (update)
                {
                    if (value != tx_postgen_tt_freq1_dsp || force)
                    {
                        tx_postgen_tt_freq1_dsp = value;
                        wdsp.SetTXAPostGenTTFreq(wdsp.id(1, 0), tx_postgen_tt_freq1_dsp, tx_postgen_tt_freq2_dsp);

                    }
                }
            }
        }

        private double tx_postgen_tt_freq2_dsp = 0.0;
        private double tx_postgen_tt_freq2 = 0.0;
        public double TXPostGenTTFreq2
        {
            get { return tx_postgen_tt_freq2; }
            set
            {
                tx_postgen_tt_freq2 = value;
                if (update)
                {
                    if (value != tx_postgen_tt_freq2_dsp || force)
                    {
                        tx_postgen_tt_freq2_dsp = value;
                        wdsp.SetTXAPostGenTTFreq(wdsp.id(1, 0), tx_postgen_tt_freq1_dsp, tx_postgen_tt_freq2_dsp);

                    }
                }
            }
        }

        private double tx_postgen_sweep_mag_dsp = 0.0;
        private double tx_postgen_sweep_mag = 0.0;
        public double TXPostGenSweepMag
        {
            get { return tx_postgen_sweep_mag; }
            set
            {
                tx_postgen_sweep_mag = value;
                if (update)
                {
                    if (value != tx_postgen_sweep_mag_dsp || force)
                    {
                        wdsp.SetTXAPostGenSweepMag(wdsp.id(1, 0), value);
                        tx_postgen_sweep_mag_dsp = value;
                    }
                }
            }
        }

        private double tx_postgen_sweep_freq1_dsp = 0.0;
        private double tx_postgen_sweep_freq1 = 0.0;
        public double TXPostGenSweepFreq1
        {
            get { return tx_postgen_sweep_freq1; }
            set
            {
                tx_postgen_sweep_freq1 = value;
                if (update)
                {
                    if (value != tx_postgen_sweep_freq1_dsp || force)
                    {
                        tx_postgen_sweep_freq1_dsp = value;
                        wdsp.SetTXAPostGenSweepFreq(wdsp.id(1, 0), tx_postgen_sweep_freq1_dsp, tx_postgen_sweep_freq2_dsp);

                    }
                }
            }
        }

        private double tx_postgen_sweep_freq2_dsp = 0.0;
        private double tx_postgen_sweep_freq2 = 0.0;
        public double TXPostGenSweepFreq2
        {
            get { return tx_postgen_sweep_freq2; }
            set
            {
                tx_postgen_sweep_freq2 = value;
                if (update)
                {
                    if (value != tx_postgen_sweep_freq2_dsp || force)
                    {
                        tx_postgen_sweep_freq2_dsp = value;
                        wdsp.SetTXAPostGenSweepFreq(wdsp.id(1, 0), tx_postgen_sweep_freq1_dsp, tx_postgen_sweep_freq2_dsp);
                    }
                }
            }
        }

        private double tx_postgen_sweep_rate_dsp = 0.0;
        private double tx_postgen_sweep_rate = 0.0;
        public double TXPostGenSweepRate
        {
            get { return tx_postgen_sweep_rate; }
            set
            {
                tx_postgen_sweep_rate = value;
                if (update)
                {
                    if (value != tx_postgen_sweep_rate_dsp || force)
                    {
                        wdsp.SetTXAPostGenSweepRate(wdsp.id(1, 0), value);
                        tx_postgen_sweep_rate_dsp = value;
                    }
                }
            }
        }
		
		#endregion
	}

	#endregion

    class MNotchDB
    {
        private static List<MNotch> list = new List<MNotch>();
        public static List<MNotch> List
        {
            get { return list; }
        }
    }

    class MNotch : IComparable
    {
        private double fcenter = 0.0;
        public double FCenter
        {
            get { return fcenter; }
            set { fcenter = value; }
        }

        private double fwidth = 0.0;
        public double FWidth
        {
            get { return fwidth; }
            set { fwidth = value; }
        }

        private bool active = false;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public MNotch(double freq, double width, bool act)
        {
            fcenter = freq;
            fwidth = width;
            active = act;            
        }

        public static MNotch Parse(string s)
        {
            int index = s.IndexOf("MHz");
            double freq = double.Parse(s.Substring(0, index));

            int index2 = s.LastIndexOf("Hz");
            double width = double.Parse(s.Substring(index + 5, index2 - (index + 5)));

            index = s.IndexOf("active:");

            bool active;
            string a;
            //if (index2 != -1)
            //{
            //    active = bool.Parse(s.Substring(index + 5, index2 - (index + 5)));               
            //}
            //else
           // {
                a = s.Substring(index + 7);
                active = bool.Parse(s.Substring(index + 7));
           // }

            return new MNotch(freq, width, active);
        }

        public override string ToString()
        {
            return fcenter.ToString("R") + " MHz| " + fwidth + " Hz| active: " + active;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            return this.fcenter.CompareTo(((MNotch)obj).fcenter);
        }

    }
}