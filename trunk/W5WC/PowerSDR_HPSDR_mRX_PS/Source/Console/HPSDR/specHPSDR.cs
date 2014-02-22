﻿/*
*
* Copyright (C) 2010-2013  Doug Wigley 
* 
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 2 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PowerSDR
{
    public class SpecRX
    {
        private const int NUM_RX_DISP = 2;
        private SpecHPSDR[] spec_rx;

        public SpecRX()
        {
            spec_rx = new SpecHPSDR[NUM_RX_DISP];

            for (int i = 0; i < NUM_RX_DISP; i++)
            {
                spec_rx[i] = new SpecHPSDR(i);
            }
        }

        public SpecHPSDR GetSpecRX(int disp)
        {
            return spec_rx[disp];
        }
    }

    public class SpecHPSDR
    {
        private int disp;
        public SpecHPSDR(int d)
        {
            disp = d;
        }

        private bool update = false;
        public bool Update
        {
            get { return update; }
            set
            {
                update = value;
            }
        }

        private int spur_eliminationtion_ffts = 1;
        public int SpurEliminationFFTS
        {
            get { return spur_eliminationtion_ffts; }
            set
            {
                spur_eliminationtion_ffts = value;
            }
        }

        private int data_type = 1;
        public int DataType
        {
            get { return data_type; }
            set
            {
                data_type = value;
            }
        }

        private int fft_size = 32768;
        public int FFTSize
        {
            get { return fft_size; }
            set
            {
                fft_size = value;
                if (update) initAnalyzer();
            }
        }

        private int blocksize;
        public int BlockSize
        {
            get { return blocksize; }
            set
            {
                blocksize = value;
                if (update) initAnalyzer();
                if (disp == 0)
                    for (int i = 0; i < 3; i++)
                        SpecHPSDRDLL.SetEXTANBBuffsize(i, blocksize);
                if (disp == 1)
                    SpecHPSDRDLL.SetEXTANBBuffsize(3, blocksize);

            }
        }

        private int window_type = 4;
        public int WindowType
        {
            get { return window_type; }
            set
            {
                window_type = value;
                if (update) initAnalyzer();
            }
        }

        private double kaiser_pi = 14.0;
        public double KaiserPi
        {
            get { return kaiser_pi; }
            set
            {
                kaiser_pi = value;
            }
        }

        private int overlap = 30000;
        public int Overlap
        {
            get { return overlap; }
            set
            {
                overlap = value;
            }
        }

        private int clip = 0;
        public int Clip
        {
            get { return clip; }
            set
            {
                clip = value;
            }
        }

        private int span_clip_l = 0;
        public int SpanClipL
        {
            get { return span_clip_l; }
            set
            {
                span_clip_l = value;
            }
        }

        private int span_clip_h = 0;
        public int SpanClipH
        {
            get { return span_clip_h; }
            set
            {
                span_clip_h = value;
            }
        }

        private int pixels = 2048;
        public int Pixels
        {
            get { return pixels; }
            set
            {
                pixels = value;
                if (update) initAnalyzer();
            }
        }

        private int stitches = 1;
        public int Stitches
        {
            get { return stitches; }
            set
            {
                stitches = value;
            }
        }

        private int calibration_data_set = 0;
        public int CalibrationDataSet
        {
            get { return calibration_data_set; }
            set
            {
                calibration_data_set = value;
            }
        }

        private double span_min_freq = 0.0;
        public double SpanMinFreq
        {
            get { return span_min_freq; }
            set
            {
                span_min_freq = value;
            }
        }

        private double span_max_freq = 0.0;
        public double SpanMaxFreq
        {
            get { return span_max_freq; }
            set
            {
                span_max_freq = value;
            }
        }

        private int avm;
        private bool average_on;						// True if the Average button is pressed
        public bool AverageOn
        {
            get { return average_on; }
            set
            {
                average_on = value;
                if (peak_on) avm = -1;
                else if (average_on) avm = av_mode;
                else avm = 0;

                if (update) initAnalyzer();
            }
        }

        private bool peak_on;							// True if the Peak button is pressed
        public bool PeakOn
        {
            get { return peak_on; }
            set
            {
                peak_on = value;
                if (peak_on) avm = -1;
                else if (average_on) avm = av_mode;
                else avm = 0;

                if (update) initAnalyzer();
            }
        }

        private int frame_rate = 15;
        public int FrameRate
        {
            get { return frame_rate; }
            set
            {
                frame_rate = value;

                if (update) initAnalyzer();
            }
        }

        private double tau;                             //time-constant for averaging
        public double AvTau
        {
            get { return tau; }
            set
            {
                tau = value;

                if (update) initAnalyzer();
            }
        }

        private int av_mode;
        public int AverageMode
        {
            get { return av_mode; }
            set
            {
                av_mode = value;
                if (peak_on) avm = -1;
                else if (average_on) avm = av_mode;
                else avm = 0;

                if (update) initAnalyzer();
            }
        }

        private double z_slider;
        public double ZoomSlider
        {
            get { return z_slider; }
            set
            {
                z_slider = value;

                if (update) initAnalyzer();
            }
        }

        private double pan_slider;
        public double PanSlider
        {
            get { return pan_slider; }
            set
            {
                pan_slider = value;

                if (update) initAnalyzer();
            }
        }

        private int sample_rate;
        public int SampleRate
        {
            get { return sample_rate; }
            set
            {
                sample_rate = value;

                if (update) initAnalyzer();
                if (disp == 0)
                    for (int i = 0; i < 3; i++)
                        SpecHPSDRDLL.SetEXTANBSamplerate(i, sample_rate);
                if (disp == 1)
                    SpecHPSDRDLL.SetEXTANBSamplerate(3, sample_rate);
            }
        }

        private bool nb_on = false;
        public bool NBOn
        {
            get { return nb_on; }
            set { nb_on = value; }
        }

        const double KEEP_TIME = 0.1;
        private int max_w;
        private double freq_offset = 12000.0;

        public void initAnalyzer()
        {
            //maximum number of frames of pixels to average
            const int MAX_AV_FRAMES = 60;

            //compute multiplier for weighted averaging
            double avb = Math.Exp(-1.0 / (frame_rate * tau));
            //compute number of frames to average for window averaging
            int display_average = Math.Max(2, (int)Math.Min(MAX_AV_FRAMES, frame_rate * tau));

            //no spur elimination => only one spur_elim_fft and it's spectrum is not flipped
            int[] flip = { 0 };
            GCHandle handle = GCHandle.Alloc(flip, GCHandleType.Pinned);
            IntPtr h_flip = handle.AddrOfPinnedObject();

            int low = 0;
            int high = 0;
            double bw_per_subspan = 0.0;

            switch (data_type)
            {
                case 0:     //real fft - in case we want to use for wideband data in the future
                    {

                        break;
                    }
                case 1:     //complex fft
                    {
                        //fraction of the spectrum to clip off each side of each sub-span
                        const double CLIP_FRACTION = 0.017;

                        //set overlap as needed to achieve the desired frame_rate
                        overlap = (int)Math.Max(0.0, Math.Ceiling(fft_size - (double)sample_rate / (double)frame_rate));

                        //clip is the number of bins to clip off each side of each sub-span
                        clip = (int)Math.Floor(CLIP_FRACTION * fft_size);

                        //the amount of frequency in each fft bin (for complex samples) is given by:
                        double bin_width = (double)sample_rate / (double)fft_size;

                        //the number of useable bins per subspan is
                        int bins_per_subspan = fft_size - 2 * clip;

                        //the amount of useable bandwidth we get from each subspan is:
                        bw_per_subspan = bins_per_subspan * bin_width;

                        //the total number of bins available to display is:
                        int bins = stitches * bins_per_subspan;

                        //apply log function to zoom slider value
                        double zoom_slider = Math.Log10(9.0 * z_slider + 1.0);

                        //limits how much you can zoom in; higher value means you zoom more
                        const double zoom_limit = 100;

                        int width = (int)(bins * (1.0 - (1.0 - 1.0 / zoom_limit) * zoom_slider));

                        //FSCLIPL is 0 if pan_slider is 0; it's bins-width if pan_slider is 1
                        //FSCLIPH is bins-width if pan_slider is 0; it's 0 if pan_slider is 1
                        span_clip_l = (int)Math.Floor(pan_slider * (bins - width));
                        span_clip_h = bins - width - span_clip_l;

                        if (Display.RX1DSPMode == DSPMode.DRM)
                        {
                            //Apply any desired frequency offset
                            int bin_offset = (int)(freq_offset / bin_width);
                            if ((span_clip_h -= bin_offset) < 0) span_clip_h = 0;
                            span_clip_l = bins - width - span_clip_h;
                        }

                        //As for the low and high frequencies that are being displayed:
                        low = -(int)((double)stitches / 2.0 * bw_per_subspan - (double)span_clip_l * bin_width + bin_width / 2.0);
                        high = +(int)((double)stitches / 2.0 * bw_per_subspan - (double)span_clip_h * bin_width - bin_width / 2.0);
                        //Note that the bin_width/2.0 factors are included because the complex FFT has one more negative output bin
                        //  than positive output bin.
                        max_w = fft_size + (int)Math.Min(KEEP_TIME * sample_rate, KEEP_TIME * fft_size * frame_rate);
                        break;
                    }
            }

            switch (disp)
            {
                case 0:
                    Display.RXDisplayLow = low;
                    Display.RXDisplayHigh = high;
                   if (Display.CurrentDisplayMode == DisplayMode.WATERFALL ||
                       Display.CurrentDisplayMode == DisplayMode.PANAFALL)
                    {
                        Display.TXDisplayLow = low;
                        Display.TXDisplayHigh = high;
                    }
                    break;
                case 1:
                    Display.RX2DisplayLow = low;
                    Display.RX2DisplayHigh = high;
                    if (!(Display.CurrentDisplayMode == DisplayMode.WATERFALL ||
                        Display.CurrentDisplayMode == DisplayMode.PANAFALL))
                    {
                        Display.TXDisplayLow = low;
                        Display.TXDisplayHigh = high;
                    }
                    break;
            }

            JanusAudio.LowFreqOffset = bw_per_subspan;
            JanusAudio.HighFreqOffset = bw_per_subspan;

            if (disp == 0)
            {
                if (Display.CurrentDisplayMode != DisplayMode.PANADAPTER &&
                   Display.CurrentDisplayMode != DisplayMode.WATERFALL &&
                   Display.CurrentDisplayMode != DisplayMode.PANAFALL &&
                   Display.CurrentDisplayMode != DisplayMode.PANASCOPE)
                    return;
            }
     
            SpecHPSDRDLL.SetAnalyzer(disp,
                        spur_eliminationtion_ffts,
                        data_type,
                        h_flip,
                        fft_size,
                        blocksize,
                        window_type,
                        kaiser_pi,
                        overlap,
                        clip,
                        span_clip_l,
                        span_clip_h,
                        pixels,
                        stitches,
                        avm,
                        display_average,
                        avb,
                        calibration_data_set,
                        span_min_freq,
                        span_max_freq,
                        max_w);
        }

        public void CalcSpectrum(int filter_low, int filter_high, int spec_blocksize)
        {
            //filter_low is the low frequency setting for the filter
            //filter_high is the high frequency setting for the filter
            //samplerate is the current samplerate
            //fft_size is the current FFT size

            //maximum number of frames of pixels to average
            const int MAX_AV_FRAMES = 60;

            //compute multiplier for weighted averaging
            double avb = Math.Exp(-1.0 / (frame_rate * tau));
            //compute number of frames to average for window averaging
            int display_average = Math.Max(2, (int)Math.Min(MAX_AV_FRAMES, frame_rate * tau));

            //no spur elimination => only one spur_elim_fft and it's spectrum is not flipped
            int[] flip = { 0 };
            GCHandle handle = GCHandle.Alloc(flip, GCHandleType.Pinned);
            IntPtr h_flip = handle.AddrOfPinnedObject();

           // const int extra = 1000;
            //if we allow a little extra spectrum to be displayed on each side of
            //  the filter settings, then, you can look at filter rolloff.  This
            //  seems to happen at least some of the time with the old spectrum display.
            //  "extra" is the amount extra to leave on each side of the filter bandwidth
            //  and is in Hertz.

            //the upper and lower limits of the displayed spectrum would be
            int upper_freq = filter_high;// +extra;
            int lower_freq = filter_low;// -extra;

            //bandwidth to clip off on the high and low sides
            double high_clip_bw = 0.5 * sample_rate - upper_freq;
            double low_clip_bw = 0.5 * sample_rate + lower_freq;

            //calculate the width of each frequency bin
            double bin_width = (double)sample_rate / fft_size;

            //calculate span clip parameters
            int fsclipH = (int)Math.Floor(high_clip_bw / bin_width);
            int fsclipL = (int)Math.Ceiling(low_clip_bw / bin_width);

            //no need for any symmetrical clipping
            int sclip = 0;
            int stitch = 1;
            max_w = fft_size + (int)Math.Min(KEEP_TIME * sample_rate, KEEP_TIME * fft_size * frame_rate);
            Display.RXSpectrumDisplayLow = lower_freq;
            Display.RXSpectrumDisplayHigh = upper_freq;
  
            SpecHPSDRDLL.SetAnalyzer(disp,
          spur_eliminationtion_ffts,
          data_type,
          h_flip,
          fft_size,
          spec_blocksize,
          window_type,
          kaiser_pi,
          overlap,
          sclip,
          fsclipL,
          fsclipH,
          pixels,
          stitch,
          avm,
          display_average,
          avb,
          calibration_data_set,
          span_min_freq,
          span_max_freq,
          max_w);
        }
    }

    unsafe class SpecHPSDRDLL
    {
        #region DLL Method Declarations
        [DllImport("wdsp.dll")]
        public static extern void SetAnalyzer(int disp, int n_fft, int type, IntPtr flp, int sz, int buff_size, int win_type, double pi, int ovrlp, int clp,
            int fsclipL, int fsclipH, int n_pix, int n_stch, int av_mode, int n_av, double avb, int cal_set, double fmin, double fmax, int max_w);

        [DllImport("wdsp.dll")]
        public static extern void XCreateAnalyzer(int disp, ref int success, int m_size, int m_LO, int m_stitch, string app_data_path);
        // public static extern void XCreateAnalyzer(int disp, ref int success, int m_size, int m_LO, int m_stitch);

        [DllImport("wdsp.dll")]
        public static extern void DestroyAnalyzer(int disp);

        [DllImport("wdsp.dll")]
        public static extern void GetPixels(int disp, IntPtr pix, ref int flag);

        [DllImport("wdsp.dll")]
        public static extern void GetNAPixels(int disp, IntPtr pix, ref int flag);

        [DllImport("wdsp.dll")]
        public static extern void GetPixels(int disp, float* pix, ref int flag);

        [DllImport("wdsp.dll")]
        public static extern void GetNAPixels(int disp, float* pix, ref int flag);

        [DllImport("wdsp.dll")]
        public static extern void Spectrum(int disp, int ss, int LO, float* pI, float* pQ);

        [DllImport("wdsp.dll")]
        public static extern void SetCalibration(int disp, int set, int points, IntPtr cal);

        [DllImport("wdsp.dll")]
        public static extern void SnapSpectrum(int disp, int ss, int LO, double* snap_buff);

        [DllImport("wdsp.dll")]
        public static extern void create_anbEXT(
            int id,
            int run,
            int buffsize,
            double samplerate,
            double tau,
            double hangtime,
            double advtime,
            double backtau,
            double threshold
            );

        [DllImport("wdsp.dll")]
        public static extern void destroy_anbEXT(int id);

        [DllImport("wdsp.dll")]
        public static extern void xanbEXTF(int id, float* I, float* Q);

        [DllImport("wdsp.dll")]
        public static extern void SetEXTANBBuffsize(int id, int size);

        [DllImport("wdsp.dll")]
        public static extern void SetEXTANBSamplerate(int id, int rate);

        [DllImport("wdsp.dll")]
        public static extern void SetEXTANBTau(int id, double tau);

        [DllImport("wdsp.dll")]
        public static extern void SetEXTANBHangtime(int id, double time);

        [DllImport("wdsp.dll")]
        public static extern void SetEXTANBAdvtime(int id, double time);

        [DllImport("wdsp.dll")]
        public static extern void SetEXTANBBacktau(int id, double tau);

        [DllImport("wdsp.dll")]
        public static extern void SetEXTANBThreshold(int id, double thresh);

        #endregion
    }
}
