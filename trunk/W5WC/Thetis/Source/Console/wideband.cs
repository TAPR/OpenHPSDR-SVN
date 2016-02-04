using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Thetis
{
    public partial class wideband : Form
    {
        public wideband(int i)
        {
            InitializeComponent();
            wbdisplay.init = true;
            wbdisplay.ADC = i;
            wbdisplay.create_wideband(i);
            wbdisplay.initWideband();
        }

        public wbDisplay WBdisplay
        {
            get { return wbdisplay; }
            set { wbdisplay = value; }
        }

        private void wideband_Resize(object sender, EventArgs e)
        {
            wbdisplay.pauseDisplayThread = true;
            wbdisplay.Init();
            wbdisplay.UpdateGraphicsBuffer();
            wbdisplay.pauseDisplayThread = false;
        }

        private void wideband_FormClosing(object sender, FormClosingEventArgs e)
        {
            wbdisplay.Cancel_Display();
            NetworkIO.SetWBEnable(0, 0);
            this.Hide();
            e.Cancel = true;
        }
        
        //private int adc = 0;
        //public int ADC
        //{
        //    get { return adc; }
        //    set
        //    {
        //        adc = value;
        //    }
        //}

        //private int sample_rate = 122880000;
        //public int SampleRate
        //{
        //    get { return sample_rate; }
        //    set
        //    {
        //        sample_rate = value;
        //        initWideband();
        //    }
        //}

        //private int fft_size = 4096;
        //public int FFTSize
        //{
        //    get { return fft_size; }
        //    set
        //    {
        //        fft_size = value;
        //        initWideband();
        //    }
        //}

        //private int window_type = 6;
        //public int WindowType                           // set from Window Type control
        //{
        //    get { return window_type; }
        //    set
        //    {
        //        window_type = value;
        //        initWideband();
        //    }
        //}

        //private double kaiser_pi = 14.0;
        //public double KaiserPi                          // set from Kaiser PiAlpha control
        //{
        //    get { return kaiser_pi; }
        //    set
        //    {
        //        kaiser_pi = value;
        //        initWideband();
        //    }
        //}

        //private int avm = 0;
        //private bool average_on = false;				// true if the Average button is depressed
        //public bool AverageOn                           // set from Average button
        //{
        //    get { return average_on; }
        //    set
        //    {
        //        average_on = value;
        //        if (peak_on) avm = -1;                  // -1 => peak_detect
        //        else if (average_on) avm = 6;           //  6 => low_noise_floor, time_weighted, log_data
        //        else avm = 0;                           //  0 => averaging & peak_detect are both OFF
        //        initWideband();
        //    }
        //}

        //private bool peak_on = false;					// true if the Peak button is depressed
        //public bool PeakOn                              // set from Peak button
        //{
        //    get { return peak_on; }
        //    set
        //    {
        //        peak_on = value;
        //        if (peak_on) avm = -1;
        //        else if (average_on) avm = 6;
        //        else avm = 0;
        //        initWideband();
        //    }
        //}

        //private double tau = 0.120;                     // time-constant for averaging
        //public double AvTau                             // set from Averaging Time Constant control
        //{
        //    get { return tau; }
        //    set
        //    {
        //        tau = value;
        //        initWideband();
        //    }
        //}

        //private int frame_rate = 50;
        //public int FrameRate                            // set from Frame Rate control
        //{
        //    get { return frame_rate; }
        //    set
        //    {
        //        frame_rate = value;
        //        initWideband();
        //    }
        //}

        //private int pixels = 2048;
        //public int Pixels                               // display code must set the number of pixel values it needs
        //{
        //    get { return pixels; }
        //    set
        //    {
        //        pixels = value;
        //        initWideband();
        //    }
        //}

        //private double z_slider = 0.0;                  // range is 0.0 to 1.0
        //public double ZoomSlider                        // set by Zoom Slider position
        //{
        //    get { return z_slider; }
        //    set
        //    {
        //        z_slider = value;
        //        initWideband();
        //    }
        //}

        //private double p_slider = 0.5;                  // range is 0.0 to 1.0
        //public double PanSlider                         // set by Pan Slider position
        //{
        //    get { return p_slider; }
        //    set
        //    {
        //        p_slider = value;
        //        initWideband();
        //    }
        //}

        //private int low_freq;
        //public int LowFreq                              // get the lowest freq that's being displayed
        //{
        //    get { return low_freq; }
        //}

        //private int high_freq;
        //public int HighFreq                             // get the highest freq that's being displayed
        //{
        //    get { return high_freq; }
        //}
        
        //void create_wideband(int adc)
        //{
        //    int success = 0;
        //    int wbid = 32 + adc;
        //    SpecHPSDRDLL.XCreateAnalyzer(wbid, ref success, 16384, 1, 1, "");
        //}

        //void initWideband()
        //{
        //    int wbid = adc + 32;
        //    int clip = 0, span_clip_l = 0, span_clip_h = 0, max_w = 0;

        //    // no spur elimination => only one spur_elim_fft and it's spectrum is not flipped
        //    int[] flip = { 0 };
        //    GCHandle handle = GCHandle.Alloc(flip, GCHandleType.Pinned);
        //    IntPtr h_flip = handle.AddrOfPinnedObject();

        //    //compute multiplier for weighted averaging
        //    double avb = Math.Exp(-1.0 / (frame_rate * tau));

        //    // fraction of the spectrum to clip off each side
        //    const double CLIP_FRACTION = 0.0;

        //    // clip is the number of bins to clip off each side of each sub-span
        //    clip = (int)Math.Floor(CLIP_FRACTION * fft_size);

        //    // the amount of frequency in each fft bin (for real samples) is given by:
        //    double bin_width = (double)sample_rate / (double)fft_size;

        //    // the number of useable bins
        //    int bins = fft_size / 2 - 2 * clip;

        //    // the amount of useable bandwidth we get is:
        //    double bw = bins * bin_width;

        //    // apply log function to zoom slider value
        //    double zoom_slider = Math.Log10(9.0 * z_slider + 1.0);

        //    // limits how much you can zoom in; higher value means you zoom more
        //    const double zoom_limit = 100.0;

        //    // width = number of bins to use AFTER zooming
        //    int width = (int)(bins * (1.0 - (1.0 - 1.0 / zoom_limit) * zoom_slider));

        //    // FSCLIPL is 0 if pan_slider is 0; it's bins-width if pan_slider is 1
        //    // FSCLIPH is bins-width if pan_slider is 0; it's 0 if pan_slider is 1
        //    span_clip_l = (int)Math.Floor(p_slider * (bins - width));
        //    span_clip_h = bins - width - span_clip_l;

        //    // the low and high frequencies that are being displayed:
        //    low_freq  = sample_rate / 4 - (int)(0.5 * bw - (double)span_clip_l * bin_width);
        //    high_freq = sample_rate / 4 + (int)(0.5 * bw - (double)span_clip_h * bin_width);

        //    SpecHPSDRDLL.SetAnalyzer (
        //        wbid,                       // id of this analyzer
        //        1,                          // one fft, no spur elimination for this application
        //        0,                          // REAL data
        //        h_flip,                     // flip settings for spur elimination ffts
        //        fft_size,                   // fft_size
        //        512,                        // input buffer size
        //        window_type,                // window_type
        //        kaiser_pi,                  // piAlpha for Kaiser window
        //        0,                          // overlap can't be used for discontinuous data
        //        clip,                       // bins to clip on EACH side
        //        span_clip_l,                // additional bins to clip on the low side
        //        span_clip_h,                // additional bins to clip on the high side
        //        pixels,                     // number of pixel values to generate
        //        1,                          // number of sub-spans to stitch
        //        avm,                        // averaging mode
        //        1,                          // number of ffts to average for window-averaging
        //        avb,                        // back multiplier for time-averaging
        //        0,                          // calibration set
        //        0.0,                        // calibration fmin
        //        0.0,                        // calibration fmax
        //        2 * fft_size                // maximum write-ahead
        //        );
        //}
    }
}
