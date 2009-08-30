/*
 * Cyclops for High Performance Software Defined Radio
 *
 * Developed from original code  Copyright 2006 (C) Phil Covington, N8VB
 * 
 * Copyright (C) 2009 Phil Harman, VK6APH
 * Copyright (C) 2009 David McQuate WA8YWQ
 * Copyright (C) 2009 Joe Martin K5SO
 * Copyright (C) 2009 George Byrkit K9TRV
 * Copyright (C) 2009 Mark Amos W8XR
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
 * 
 * 
 */

/*
 * Change log 
 * 
 *  23 Aug 2009 - Started coding using KK V1.0.6 as a base
 *  24 Aug 2009 - Added 10MHz clock selection
 *              - Added display of system frequencies
 *  29 Aug 2009 - Allow keyboard entry of frequency 0-1GHz
 *              - Save settings to Cyclops.csv
 *                  - 10MHz reference source
 *                  - Last frequency used
 *              - Uses Cyclops.rbf V1.0
 *              - Release as V1.0.1
 * 30 Aug 2009  - Fixed bug where spectrum tunes backwards
 *              - Made frequency display read only so can't edit for now
 *               
 *
 *  
 *              
 *    
 * 
 * TODO:        - Find why PageUp, PageDown, Up/Down Arrows work in reverse 
 *              - Allow frequency display to accept edit
 *              
 *
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HPSDR_USB_LIB_V1;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using SharpDSP;
using System.Diagnostics;   // use View > Output to see debug messages
using System.Globalization;  // so we don't have country issues with '.' and ',' in KK.csv files


namespace Cyclops
{
    public partial class Form1 : Form
    {
        string version = "V1.0.2";  // change this for each release!
        string Ozy_version = null;  // holds version of Ozy code loaded into FX2

        // are these really rbufSize / 2???  If so, we should make them defined that way, and tidy up the '1024' values
        // so they are not 'magic numbers'...
        const int iqsize = 1024;
        IntPtr hdev = IntPtr.Zero;                  // USB Ozy device handle
        const int rbufSize = 2048;
        byte[] rbuf = new byte[rbufSize];           // Ozy EP6 samples read buffer for real-time receiver
        const int EP4BufSize = 8192;
        byte[] EP4buf = new byte[EP4BufSize];       // Ozy EP4 samples read buffer for full-bandwidth spectrum display
        byte[] frequency = new byte[4];             // holds Mercury frequency 
        byte[] FirstLOfreq = new byte[2];           // holds first Local Oscillator frequency

        byte rc0, rc1, rc2, rc3, rc4;               // Command & Control bytes received from Ozy

        public SharpDSP2._1.DSPState state;         // public so that the Setup form can use it
        SharpDSP2._1.DSPBuffer SignalBuffer;
        public SharpDSP2._1.Receiver rcvr;          // public so that the Setup form can use it 
        SharpDSP2._1.RingBuffer AudioRing;
        float[] PowerSpectrumData = new float[rbufSize];    // data acquired from rcvr.PowerSpectrumSignal event

        // for full bandwidth power spectrum display--
        SharpDSP2._1.DSPState FullBandwidthSpectrumState;
        SharpDSP2._1.DSPBuffer FullBandwidthSpectrumBuffer;
        SharpDSP2._1.OutbandPowerSpectrumSignal FullBandwidthSpectrum;
        float[] FullBandwidthPowerSpectrumData = new float[EP4BufSize];

        int step_size; // the frequency step using the mouse scroll wheel
        bool read_ready; // set when we have read 1024 samples from USB port
        int[] I_sample = new int[iqsize]; // holds the I samples from Ozy
        int[] Q_sample = new int[iqsize]; // holds the Q samples from Ozy
   
        public string ModeText;
        int sample_no = 0;
        const byte sync = 0x7F;
        int previous_value;
        byte C0 = 0x00; byte C1, C2, C3, C4;
        public int SampleRate = 192000;
        int SampleCounter = 0;      // used to pace calls to Ozy_send() in ProcessData512 when SampleRate > 48000

        bool USB_thread_running = false;

        int Ozy_FPGA_version;       // Version number of Ozy FPGA code 

        public int Rate;            // sets bandscope update rate in F.P.S.

        public bool updateSpectrumGraphBackground = true;
        public bool updateWidebandGraphBackground = true;

        bool ShowI = false;
        bool ShowQ = false;

        private void WriteCYCLPOSCSV()
        {
            // save the current settings to CYCLOPS.csv
            List<string> lines = new List<string>();        // create a List<string> to hold all the output lines

            // each line of the output file, in output order
            lines.Add("Atlas 10MHz," + Atlas10MHz.Checked.ToString(nfi));
            lines.Add("Mercury 10MHz," + Mercury10MHz.Checked.ToString(nfi));
            lines.Add("Last Frequency," + set_frequency.Value.ToString(nfi));

            
            // write all the lines
            File.WriteAllLines(@"CYCLOPS.csv", lines.ToArray());
        }

        private void SetSplitterBar()
        {
            if ((chkSpec.Checked || ShowI || ShowQ) && chkWideSpec.Checked)
            {
                // multi-part display.  If not yet multi-part, change the splitter point
                if (splitContainer1.IsSplitterFixed)
                {
                    splitContainer1.SplitterDistance = 256;
                }
                // allow user to choose how much wideband and how much spectrum
                splitContainer1.IsSplitterFixed = false;               

                UpdateGraphBackgrounds();
            }
            else if (chkSpec.Checked || ShowI || ShowQ)
            {
                // top display only
                splitContainer1.SplitterDistance = 512;
                splitContainer1.IsSplitterFixed = true;
                UpdateGraphBackgrounds();
            }
            else if (chkWideSpec.Checked)
            {
                // bottom display only
                splitContainer1.SplitterDistance = 0;
                splitContainer1.IsSplitterFixed = true;
                UpdateGraphBackgrounds();
            }
        }

        private void UpdateGraphBackgrounds()
        {
            updateSpectrumGraphBackground = true;
            updateWidebandGraphBackground = true;
        }


        public int IQScale = 6;     // sets default display scale of I and Q signals

        public int GridMax = 0;     // sets maximum value of Bandscope display
        public int GridMin = -160;  // sets minimum value of Bandscope display
        public int GridStep = 20;   // sets distance between grid lines on bandscope 

        int delta;                  // difference between the current and previous mouse position on the bandscope
        int loop_count = 0;         // delay until we read FPGA code version from Ozy

        public string Your_call;           // holds the users call sign
        string KeypadFrequency = null;     // holds the frequency entered using the keypad
        int keycount = 0;           // holds number of key presses of the  keypad
        bool ShowCross = false;     // when set show cross at current mouse location
        int MousePositionX;         // as it says
        int MousePositionY;         // you guessed it!
        int pixels_per_GridStep;    // how many pixels a Grid Step represents in the narrow bandscope 
        bool IsSynced = false;      // true when we have sync from Ozy
        bool ADCOverload = false;   // true when ADC is overloaded
        bool Cyclops_on = false;    // true when Cyclops is running

        // define various reference and oscillator frequences
        const double FirstIF = 1030;
        const double SecondLO = 1125.5e6;
        const double MercuryClock = 122.88e6;
        int TuneFreq;                        // Frequency Cyclops is tuned to
        int FirstLOFrequency;
        double FirstIFFrequency;
        double SecondIFFrequency;
        int MercuryFrequency;       // frequecy we send to Mercury
        bool TenMHzMercury;         // set if 10MHz clock from Mercury
        bool TenMHzAtlas;           // set of 10MHz clock from Atlas/Exchalibur
        string LastFrequency;       // last frquency that was used so we can restore on startup 

        private Thread USB_thread;	// runs the loop facility 

        Point MouseOld = new Point(); // keep location of mouse for drag tuning 

        //public SetupForm Setup_form = null;  // get reference to the Setup form.

        // to what is the size of this buffer (1024) related????
        const int outbufferSize = 1024;
        SharpDSP2._1.CPX[] outbuffer = new SharpDSP2._1.CPX[outbufferSize];     // allocate a buffer for DSP output

        NumberFormatInfo nfi = NumberFormatInfo.InvariantInfo;  // so we are region independant in terms of ',' and '.' for floats

        public Form1()
        {
            //Control.CheckForIllegalCrossThreadCalls = false;  // leave on so we catch these
            InitializeComponent();
            Debug.Indent();                         // Indent Debug messages to make them easier to see

            // draw 
            rcvr = new SharpDSP2._1.Receiver();
            SharpDSP2._1.PowerSpectrumSignal.ps_event += new SharpDSP2._1.PowerSpectrumSignal.PSpectrumEventHandler(PowerSpectrumSignal_ps_event);
            SharpDSP2._1.OutbandPowerSpectrumSignal.ps_event += new SharpDSP2._1.OutbandPowerSpectrumSignal.PSpectrumEventHandler(OutbandPowerSpectrumSignal_ps_event);
            state = rcvr.DSPStateObj;       // fetch state that Receiver constructor created
            state.DSPBlockSize = iqsize;     // put in some initial values

            // Set up for full bandwidth spectrum display
            FullBandwidthSpectrumState = new SharpDSP2._1.DSPState();
            FullBandwidthSpectrumState.DSPBlockSize = EP4BufSize / 2;
            FullBandwidthSpectrumBuffer = new SharpDSP2._1.DSPBuffer(FullBandwidthSpectrumState);
            FullBandwidthSpectrum = new SharpDSP2._1.OutbandPowerSpectrumSignal(ref FullBandwidthSpectrumBuffer);

            state.DSPSampleRate = 192000;           // set the DSP sample rate
            rcvr.PowerSpectrumUpdateRate = 50;      // Set maximum PowerSpectrum update rate
            rcvr.WindowType = SharpDSP2._1.WindowType_e.BLACKMAN3_WINDOW;

            AudioRing = new SharpDSP2._1.RingBuffer(EP4BufSize);          // create a ring buffer for rcvr audio output complex values

            SignalBuffer = new SharpDSP2._1.DSPBuffer(state);

            stepSize.Text = "100Hz";  // force mouse wheel step size for now
            stepSize_SelectedIndexChanged(this, EventArgs.Empty);  // force contol to update 

            rcvr.PowerSpectrumAveragingOn = true;
            rcvr.PowerSpectrumSmoothingFactor = 0.6f;

            // set the max and minimum frequencies that can be selected
            set_frequency.Minimum = 0;
            set_frequency.Maximum = 1000000000;

            // force narrow band spectrum scope on 
            chkSpec.Checked = true;

            // Load the previous radio settings from the KK.csv file and allocate values.
            // First check that the file exists
            if (!File.Exists("CYCLOPS.csv"))   //if file doesn't exist, create it
            {
                //CreateKKCSV();
            }

            // now read the lines in the config file.  Warn if action above didn't create one!
            // This allows CYCLKPS.CSV to be no longer part of the SVN tree.  This means that the CYCLPOS.CSV
            // file won't get mangled/updated by SVN when someone updates the code!
            if (File.Exists("CYCLOPS.csv"))   //if file exists open it
            {
                ReadCYCLOPSCSV();

                // set the various values to those read from CYCLOPS.CSV
                Mercury10MHz.Checked = TenMHzMercury;
                Atlas10MHz.Checked = TenMHzAtlas;
                set_frequency.Value = Convert.ToInt32(LastFrequency);
            }

            else  // TODO: can't find KK.csv file - should add some error handling or default values here.
            {
                MessageBox.Show("Can't find CYCLPS.csv, should be in the same directory as Cyclops.exe?", "File Error");
            }

            Frequency_change();  // show last used frequency on display
        }

        public void ReadCYCLOPSCSV()
        {
            // read the file into an array so we can process each entry
            var lines = File.ReadAllLines("CYCLOPS.csv");
            List<string> value = new List<string>();

            // process the string array of data that was read
            foreach (string text in lines)
            {
                /* This gets around the problem of the locale of the user as far as saving floating point
                 * values which might (because of locale) have commas in the numbers, thus creating
                 * more entries in the array than desired, because of mis-parsing based on the extra commas.
                 * We also get to use a List<string> so that we don't need to know how big it is before we process
                 * data.  It indexes the same as 'string[] value' did.
                 */
                int p = text.IndexOf(",");      // search for the comma after the data element name
                if (p != -1)
                {
                    // we found a comma.  Split the line into 'before' and 'after' the comma
                    // before the comma is the data name
                    // after the comma is its value
                    value.Add(text.Substring(0, p));
                    value.Add(text.Substring(p + 1));
                }
            }

            // Assign the values in CYCLOPS.csv to their respective variables

            TenMHzMercury = LookupCYCLOPSCSVValue("Mercury 10MHz", TenMHzMercury, value);
            TenMHzAtlas = LookupCYCLOPSCSVValue("Atlas 10MHz", TenMHzAtlas, value);
            LastFrequency = LookupCYCLOPSCSVValue("Last Frequency", LastFrequency, value);

        }       

        public string LookupCYCLOPSCSVValue(string key, string defaultValue, List<string> value)
        {
            for (int i = 0; i < value.Count; i += 2)
            {
                string s = value[i];
                // search all the even index entries for a key that matches.  If there is one, return
                // the next list entry
                if (s.Equals(key, StringComparison.InvariantCulture))
                {
                    // they match.  return the next element in value
                    return value[i + 1];
                }
            }

            // not found.  Return default value instead
            return defaultValue;
        }

        public float LookupCYCLOPSCSVValue(string key, float defaultValue, List<string> value)
        {
            for (int i = 0; i < value.Count; i += 2)
            {
                string s = value[i];
                // search all the even index entries for a key that matches.  If there is one, return
                // the next list entry
                if (s.Equals(key, StringComparison.InvariantCulture))
                {
                    // they match.  return the next element in value
                    try
                    {
                        return (float)Convert.ToDouble(value[i + 1], nfi);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }

            // not found.  Return default value instead
            return defaultValue;
        }

        public int LookupCYCLOPSCSVValue(string key, int defaultValue, List<string> value)
        {
            for (int i = 0; i < value.Count; i += 2)
            {
                string s = value[i];
                // search all the even index entries for a key that matches.  If there is one, return
                // the next list entry
                if (s.Equals(key, StringComparison.InvariantCulture))
                {
                    // they match.  return the next element in value
                    try
                    {
                        return Convert.ToInt32(value[i + 1], nfi);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }

            // not found.  Return default value instead
            return defaultValue;
        }

        public bool LookupCYCLOPSCSVValue(string key, bool defaultValue, List<string> value)
        {
            for (int i = 0; i < value.Count; i += 2)
            {
                string s = value[i];
                // search all the even index entries for a key that matches.  If there is one, return
                // the next list entry
                if (s.Equals(key, StringComparison.InvariantCulture))
                {
                    // they match.  return the next element in value
                    try
                    {
                        return Convert.ToBoolean(value[i + 1], nfi);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }

            // not found.  Return default value instead
            return defaultValue;
        }

        void PowerSpectrumSignal_ps_event(object source, SharpDSP2._1.PowerSpectrumSignal.PSpectrumEvent e)
        {
            Array.Copy(e.buffer, PowerSpectrumData, rbufSize);
        }

        void OutbandPowerSpectrumSignal_ps_event(object source, SharpDSP2._1.OutbandPowerSpectrumSignal.PSpectrumEvent e)
        {
            Array.Copy(e.buffer, FullBandwidthPowerSpectrumData, EP4BufSize);
        }

        private void ProcessWideBandData()
        {
            if (chkWideSpec.Checked) // display wide band spectrum
            {
                // Read new buffer of full-bandwidth
                int ret = libUSB_Interface.usb_bulk_read(hdev, 0x84, EP4buf, EP4BufSize, 100);
                if (ret == EP4BufSize)
                {
                    byte[] temp = new byte[4];
                    float scaleIn = (float)(1.0 / Math.Pow(2, 15));
                    float Sample, RealAverage;
                    float[] SamplesReal = new float[EP4BufSize / 2];
                    float[] SamplesImag = new float[EP4BufSize / 2];

                    temp[2] = 0;
                    temp[3] = 0;
                    RealAverage = 0.0f;
                    int i, k;
                    for (i = 0, k = 0; i < EP4BufSize; i += 2, ++k)
                    {
                        // use this rather than BitConverter.ToInt16()...since its faster and less CPU intensive
                        // without the '(short)' in there, the value doesn't get sign-extended!
                        // so what should be small negative values show up as (almost) positive values?
                        Sample = scaleIn * (float)(short)((EP4buf[i] << 8) | (EP4buf[i + 1]));

                        RealAverage += Sample;
                        SamplesReal[k] = Sample;
                    }

                    RealAverage /= (float)k;
                    for (i = 0; i < k; ++i)
                    {
                        SamplesReal[i] -= RealAverage;      // Subtract average
                        SamplesImag[i] = SamplesReal[i];    // temporary -- soon will do digital down-conversion
                        // with sin & cos, so we'll actually have I & Q data to enter.
                    }

                    FullBandwidthSpectrum.Process(SamplesReal, SamplesImag, EP4BufSize / 2);
                }
            }
        }


        public void UpdateFormTitle()
        {
            this.Text = "HPSDR  Cyclops Spectrum Analyser   " + version + "   " + Your_call;  // text on top of form 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateFormTitle();

            this.StartPosition = FormStartPosition.CenterScreen;  // TODO save last postion and restore from KK.csv
            this.Width = 1040;
            this.Height = 800;
        }


        private void DrawSpectrumGrid(Graphics g)
        {
            #region How DrawGrid works
            /* 
             * We want to display a grid spaced 5/10/20kHz apart horizontally depending on the sampling rate.
             * We also want to display a vertical grid spaced zdB apart. We also want to indicate the 
             * current filter width on the display.
             * 
             * The screen is 1024 pixels wide so filter width =  bandwidth / sampling_rate
             * 
             * To display the grid, first calculate the vertical grid spacing. We want to display from +/-XdB to -YdBm in ZdB steps 
             * which means we need 160/20 = 8 horizontal lines.
             * 
             * We know the frequency at the left hand edge of the bandscope since it will be the tuned frequency less 
             * (sampling rate/2).  This will be displayed at the top of the first vertical grid line. We want the vertical 
             * grid to have spacings of 5/10/20kHz depending on the sampling rate. We also want the frequency markings to
             * be in these increments. So we need to find the first location from the left hand side of the screen that we
             * can divide by 5/10/20 and get no remainder.  We use the % function to do this. 
             * 
             * Since the minimium grid size is 5kHz we are going to have to test in finer increments than this such that
             * frequency/5 = 0 remainder.  So we use 100Hz increments - this gives a nice smooth display when we drag it.
             * 
             * Once we know the first frequency from the left hand edge of the bandscope we can convert this to pixels. 
             * We know the bandscope is 1024 pixels wide and the frequency width = sampling rate. So we can calculate the x 
             * location of the first vertical bar from  
             * 
             *  pixel_indent = Hz offset from LHS * width of bandscope/sample rate
             *  
             * We can now format the frequency text and display the lines. 
             * 
             * 
             */

            #endregion

            // get the height of our panel
            int BSHeight = pictureBoxSpectrum.Height;

            // calculate the number of horizontal lines and the distance between them
            double lines = (float)(GridMax - GridMin) / (float)GridStep;
            lines = Math.Ceiling((double)lines); // if lines is a fraction round up to next integer 
            int x_locate = 0;
            string displayFreq;
            int display_frequency = 0;
            int offset = 15; // how many pixels from the top to start drawing the vertical lines

            // determine vertical grid spacing
            int vertical_grid_spacing = (int)(BSHeight / lines);
            pixels_per_GridStep = vertical_grid_spacing;

            // determine horizontal grid spacing
            int grid_spacing = SampleRate / 960;  // gives 50/100/200 x 100Hz
            int pixel_spacing = pictureBoxSpectrum.Width * grid_spacing * 100 / SampleRate;  // how many pixels this spacing represents
            float pixels_per_100Hz = pictureBoxSpectrum.Width * 100.0f / SampleRate; // how many pixels per 100Hz across the screen 

            // Draw the background color
            g.FillRectangle(Brushes.DarkGreen, pictureBoxSpectrum.ClientRectangle);
            Font font = new Font(FontFamily.GenericMonospace, 12);

            if (chkSpec.Checked) // if the narrow banscope is selected display it
            {
                // truncate tuned frequency to 100Hz digits 
                int truncate_freq = set_frequency.Value / 100;

                // the frequency at the left edge of the screen will be the tuned frequency - sample rate/(2 x 100Hz)
                int left_edge_frequency = truncate_freq - SampleRate / 200;

                // we now step in from the left edge frequency until we have a frequency that divides by 
                // the grid spacing with no remainder. This will be the first frequency we display
                for (display_frequency = left_edge_frequency; display_frequency < truncate_freq - 12; display_frequency++)
                {
                    if ((display_frequency % grid_spacing) == 0)
                        break;
                }

                // how many pixels is this from the left edge of the screen
                float indent_pixels = (display_frequency - left_edge_frequency) * pixels_per_100Hz;

                // draw the vertical grid lines with their associated frequency
                for (float i = indent_pixels; i < pictureBoxSpectrum.Width; i += pixels_per_100Hz)
                {
                    if (display_frequency % grid_spacing == 0)  // are we on a grid boundry
                    {
                        // draw the vertical lines
                        g.DrawLine(Pens.DarkGray, i, offset, i, BSHeight); // offset stops the line going through the text

                        displayFreq = String.Format("{0:0.000}", (float)display_frequency / 10000);
                        if (display_frequency < 100000)  // so the decimal point in the frequency aligns with the grid line - perfectionist!
                            x_locate = (int)i - 17;
                        else
                            x_locate = (int)i - 26;
                        // draw the text above each line
                        g.DrawString(displayFreq, font, Brushes.White, x_locate, 0);
                    }
                    // highlight band edge in Red
                    if (display_frequency == set_frequency.Minimum / 100 || display_frequency == set_frequency.Maximum / 100)
                        g.DrawLine(Pens.Red, i, offset, i, BSHeight);

                    display_frequency++; // increment frequency in 100s of Hz 
                }

                // draw a single vertical line in the middle of the screen
                g.DrawLine(Pens.Yellow, pictureBoxSpectrum.Width / 2, offset, pictureBoxSpectrum.Width / 2, BSHeight);

                // Draw the horizontal lines and signal calibration 
                // First draw the horizontal lines so they don't cover the calibration text 
                // Only draw them for the narrow bandscope
                for (int k = 0; k < BSHeight; k += vertical_grid_spacing)
                    g.DrawLine(Pens.DarkGray, 0, k, pictureBoxSpectrum.Width, k);

                // The dBm value for the top of the screen is held in Grid.Max, we don't indicate this but we do the next line
                // This will be Grid.Max - Grid.Step
                int textSpacing = vertical_grid_spacing;
                for (int x = GridMax - GridStep; x > GridMin; x -= GridStep)
                {
                    string level = x.ToString() + "dBm";
                    g.DrawString(level, font, Brushes.White, 0, textSpacing - 9);
                    textSpacing += vertical_grid_spacing;
                }
            }

            // Draw horizontal lines  if we need to show I or Q and no bandscope is selected
            if (ShowI || ShowQ)
            {
                if (chkSpec.Checked)  // have lines alread so return
                    return;
                else
                {
                    for (int k = 0; k < pictureBoxSpectrum.Height; k += vertical_grid_spacing)
                        g.DrawLine(Pens.DarkGray, 0, k, pictureBoxSpectrum.Width, k);
                }
            }

            // draw cross if required, only used on narrow bandscope at the moment. 
            if (ShowCross)
            {
                g.DrawLine(Pens.Yellow, MousePositionX, 0, MousePositionX, BSHeight); // draw cross vertical line
                if (MousePositionY < BSHeight)  // with both displays check we are within the screen boundries
                    g.DrawLine(Pens.Yellow, 0, MousePositionY, pictureBoxSpectrum.Width, MousePositionY); // draw cross horizontal line
            }
        }

        private void DrawWidebandGrid(Graphics g)
        {
            #region How DrawGrid works
            /* 
             * We want to display a grid spaced 5/10/20kHz apart horizontally depending on the sampling rate.
             * We also want to display a vertical grid spaced 20dB apart. We also want to indicate the 
             * current filter width on the display.
             * 
             * The screen is 1024 pixels wide so filter width =  bandwidth / sampling_rate
             * 
             * To display the grid, first calculate the vertical grid spacing. We want to display from 0 to -160dBm in 20dB steps 
             * which means we need 160/20 = 8 horizontal lines.
             * 
             * We know the frequency at the left hand edge of the bandscope since it will be the tuned frequency less 
             * (sampling rate/2).  This will be displayed at the top of the first vertical grid line. We want the vertical 
             * grid to have spacings of 5/10/20kHz depending on the sampling rate. We also want the frequency markings to
             * be in these increments. So we need to find the first location from the left hand side of the screen that we
             * can divide by 5/10/20 and get no remainder.  We use the % function to do this. 
             * 
             * Since the minimium grid size is 5kHz we are going to have to test in finer increments than this such that
             * frequency/5 = 0 remainder.  So we use 100Hz increments - this gives a nice smooth display when we drag it.
             * 
             * Once we know the first frequency from the left hand edge of the bandscope we can convert this to pixels. 
             * We know the bandscope is 1024 pixels wide and the frequency width = sampling rate. So we can calculate the x 
             * location of the first vertical bar from  
             * 
             *  pixel_indent = Hz offset from LHS * width of bandscope/sample rate
             *  
             * We can now format the frequency text and display the lines. 
             * 
             * 
             */

            #endregion

            // get height from our pictureBox
            int WBHeight = pictureBoxWideband.Height;

            // calculate the number of horizontal lines and the distance between them
            double lines = (float)(GridMax - GridMin) / (float)GridStep;
            lines = Math.Ceiling((double)lines); // if lines is a fraction round up to next integer 
            int x_locate = 0;
            string displayFreq;
            // determine grid spacing
            int vertical_grid_spacing = (int)(WBHeight / lines);
            // determine horizontal grid spacing
            int grid_spacing = SampleRate / 960;  // gives 50/100/200 x 100Hz
            int pixel_spacing = pictureBoxWideband.Width * grid_spacing * 100 / SampleRate;  // how many pixels this spacing represents
            float pixels_per_100Hz = pictureBoxWideband.Width * 100.0f / SampleRate; // how many pixels per 100Hz across the screen 

            // Draw the background color
            Font font = new Font(FontFamily.GenericMonospace, 12);

            // if wide bandscope in use draw vertical lines for it plus frequency 
            if (chkWideSpec.Checked)
            {
                g.FillRectangle(Brushes.DarkGreen, pictureBoxWideband.ClientRectangle);

                int start = 0;

                float MHz = 0.0f;
                for (int i = 0; i < pictureBoxWideband.Width; i += 84)  // 84 gives 5MHz line spacing
                {
                    // draw the vertical lines
                    g.DrawLine(Pens.DarkGray, i, start + 15, i, 512); // start offset stops the line going through the text

                    displayFreq = String.Format("{0:0.0}", MHz);
                    if (MHz < 10)  // so the decimal point in the frequency aligns with the grid line 
                        x_locate = (int)i - 17;
                    else
                        x_locate = (int)i - 26;
                    // draw the text on top of each line
                    g.DrawString(displayFreq, font, Brushes.White, x_locate, start);
                    MHz += 5.0f;
                }

                // Draw the horizontal lines and signal calibration 
                // First draw the horizontal lines so they dont cover the calibration text 
                // Only draw them for the wide bandscope
                for (int k = start + vertical_grid_spacing; k < pictureBoxWideband.Height; k += vertical_grid_spacing)
                    g.DrawLine(Pens.DarkGray, 0, k, pictureBoxWideband.Width, k);

                // The dBm value for the top of the screen is held in Grid.Max, we don't indicate this but we do the next line
                // This will be Grid.Max - Grid.Step
                int textSpacing = vertical_grid_spacing;
                for (int x = GridMax - GridStep; x > GridMin; x -= GridStep)
                {
                    string level = x.ToString() + "dBm";
                    g.DrawString(level, font, Brushes.White, 0, start + textSpacing - 9);
                    textSpacing += vertical_grid_spacing;
                }
            }
        }

        private void GetIQSamples()
        {
            float scaleIQDisplay = 20 * (float)Math.Pow(2, 15); // scale factor to convert DSP output to 16-bit short int audio output sample

            // read I & Q values from the buffer and plot the sample on the scope
            for (int i = 0; i < iqsize; i++)
            {
                I_sample[i] = (int)(scaleIQDisplay * SignalBuffer.cpx[i].real);
                Q_sample[i] = (int)(scaleIQDisplay * SignalBuffer.cpx[i].imag);
            }
        }

        Point[] IDraw;
        Point[] QDraw;
        Point[] bandDraw;
        Point[] wideBandDraw;

        private void ComputeDisplay()
        {
            int xmax = pictureBoxSpectrum.Width; // max number of samples across the screen

            int BSHeight = pictureBoxSpectrum.Height;
            int WBHeight = pictureBoxWideband.Height;
            double lines = (float)(GridMax - GridMin) / (float)GridStep;
            lines = Math.Ceiling((double)lines); // if lines is a fraction round up to next integer 

            int ymax = BSHeight; // maximum y value

            int yscale = IQScale;  // held on Setup form, sets X axis gain when displaying I and Q signals

            // assume the preamp has been set on in the Ozy code. 
            float PreampOffset = -20.0f; // remove 20dB from the signal level

            int xWidth = Math.Min(xmax, pictureBoxSpectrum.Width);

            IDraw = new Point[xWidth];
            QDraw = new Point[xWidth];
            bandDraw = new Point[xWidth];
            wideBandDraw = new Point[xWidth];

            for (int xpos = 0; xpos < xWidth; xpos++)
            {
                float Sample;
                float SampleMax = 0.0f;
                int nI = I_sample[xpos];
                int nQ = Q_sample[xpos];

                #region How scaling the bandscope works
                /*
                 * We need to scale the bandscope display depending on the max,min and step values
                 * the uses has selected on the Setup Form.
                 * We know the number of pixels that each grid line represents since this is 
                 * calculated in DrawGrid() as vertical_grid_spacing.
                 * 
                 * Hence we can calculate the number of pixles/dB of vertical resolution from
                 * 
                 *  pixels_per_dB = vertical_grid_spacing/GridStep.
                 *  
                 * We know the signal level at the top of the bandscope since this is held in GridMax.
                 * 
                 * We need to know what each sample represents in terms of dBs.
                 * To do this we feed a known signal into Mercury with the preamp off. 
                 * 
                 * With this signal as input we  determine the maximum signal in the array and note the value.
                 * The signal processing to this point has been:
                 * 
                 *       Po =  (FFT(Pin) * G)  
                 * 
                 * where G = the overall gain from the antenna socket to here
                 *     Pin = the power level at the antenns socket in dBm and 
                 *     Po  = the  level we wish  to display on the bandscope
                 *     FFT = the fast fourier transform of the input signal 
                 *     
                 * We want the banscope to the scalled in dBm so we take log and multiply by 10 i.e.
                 * 
                 *     10 log Po = 10log (FFT(Pin) * G)
                 *  
                 *  or 10 log Po = 10log (FFT(Pin) + 10Log G 
                 * 
                 * Since the relations between signal input and (FFT(Pin) is linear 
                 * if we know Pin and measure Po we can calculate 10log G.
                 * 
                 * For example, with my Mercury board, 0dBm gives a peak value of  85 (This value will be calibrated in future releases)
                 *
                 *  hence  85 = 0 + 10log G 
                 * 
                 * We can now calculate the dB value of every sample.  We need to map these to the screen.
                 * 
                 * For example, if we assume the top of the screen is 0dBm then we can calculate each samples
                 * pixel position as follows 
                 * 
                 *  yintPS = -(nPS - 85)* pixels_per_db  or -(nPS  - 85) * vertical_grid_spacing/GridStep
                 *  
                 * For values of GridMax other than 0dBm we need need to subtract this value, hence
                 * 
                 *  yintPS = -(nPS - 85 - GridMax) * vertical_grid_spacing / GridStep;
                 *  
                 * We do exactly the same calculations for the wide bandscope - in this case 10log G = 56
                 * 
                 */
                #endregion

                int logGain = 85;  // see above as to why this value TODO: allow user to calibrate this value

                // TODO: we have 2048 samples in the FFT we could average these to 1024 pixels perhaps 

                // IMPORTANT: Since we use an alias for receiving the 96MHz IF signal from Cyclops the signals 
                // will tune backwards. So display the FFT in the reverse order starting from 2047 and going to 0. 

                int nPS = (int)(PowerSpectrumData[2047 - (2 * xpos)] + PreampOffset);

                // Display FullBandwidthPowerSpectrumData on the same grid.
                // FullBandwidthPowerSpectrumData are a block of 4096 contiguous undecimated samples taken at 122.88 Msps.
                // PowerSpectrumData comes from the decimated samples.
                // Decimation results in processing gain, so, while the two spectra
                // have the same scale, when plotting, their offsets differ.
                // Since we have more data points than horizontal pixels in our graph,
                // search for & display the largest signal in each group of four.

                SampleMax = -999.0f;
                for (int k = 0; k < 4; ++k)
                {
                    // we have 2 x 4095 samples since we use a real signal so start half way 
                    Sample = FullBandwidthPowerSpectrumData[4095 + 4 * xpos + k];
                    if (Sample > SampleMax) SampleMax = Sample;
                }
                int nFBPS = (int)(SampleMax + PreampOffset);

                int yintI = (int)ymax / 2 + (int)(nI >> yscale);
                int yintQ = (int)ymax / 2 + (int)(nQ >> yscale);

                int yintPS = -(nPS - logGain - GridMax) * (int)(BSHeight / lines) / GridStep;  // see explanation above

                // if the both bandscopes are in use with need to move this one down the screen by BSHeight
                int offset = 0;
                int yintFBPS = offset - (nFBPS - 56 - GridMax) * (int)(WBHeight / lines) / GridStep; // see explanation above

                // Draw the sample point by adding a point to the respective point lists
                // when we've exited the for loop, use DrawCurve for each that we are drawing
                if (ShowI) IDraw[xpos] = new Point(xpos, yintI); // g.DrawLine(YellowPen, xlast, ylastI, xpos, yintI);
                if (ShowQ) QDraw[xpos] = new Point(xpos, yintQ); // g.DrawLine(RedPen, xlast, ylastQ, xpos, yintQ);
                if (chkSpec.Checked) bandDraw[xpos] = new Point(xpos, yintPS); // g.DrawLine(WhitePen, xlast, ylastPS, xpos, yintPS);
                if (chkWideSpec.Checked) wideBandDraw[xpos] = new Point(xpos, yintFBPS); // g.DrawLine(ChartreusePen, xlast, ylastFBPS, xpos, yintFBPS);

           } // end for each x data point
        }


        private void RefreshScope(Graphics g)
        {
            if (ShowI && (IDraw != null)) g.DrawCurve(YellowPen, IDraw);
            if (ShowQ && (QDraw != null)) g.DrawCurve(RedPen, QDraw);
            if (chkSpec.Checked && (bandDraw != null)) g.DrawCurve(WhitePen, bandDraw);
            if (chkWideSpec.Checked && (wideBandDraw != null)) g.DrawCurve(ChartreusePen, wideBandDraw);
        }


        private void RefreshSpectrumScope(Graphics g)
        {
            if (ShowI && (IDraw != null)) g.DrawCurve(YellowPen, IDraw);
            if (ShowQ && (QDraw != null)) g.DrawCurve(RedPen, QDraw);
            if (chkSpec.Checked && (bandDraw != null)) g.DrawCurve(WhitePen, bandDraw);
        }

        private void RefreshWidebandScope(Graphics g)
        {
            if (chkWideSpec.Checked && (wideBandDraw != null)) g.DrawCurve(ChartreusePen, wideBandDraw);
        }

        Pen YellowPen = Pens.Yellow;
        Pen RedPen = Pens.Red;
        Pen WhitePen = Pens.White;
        Pen ChartreusePen = Pens.Chartreuse;

        private bool start_USB()
        {
            // look for USB connection to Ozy
            hdev = USB.InitFindAndOpenDevice(0xfffe, 0x0007);
            if (hdev == IntPtr.Zero)
                return false;

            int ret;
            ret = libUSB_Interface.usb_set_configuration(hdev, 1);
            ret = libUSB_Interface.usb_claim_interface(hdev, 0);
            ret = libUSB_Interface.usb_set_altinterface(hdev, 0);
            ret = libUSB_Interface.usb_clear_halt(hdev, 0x02);
            ret = libUSB_Interface.usb_clear_halt(hdev, 0x86);
            ret = libUSB_Interface.usb_clear_halt(hdev, 0x84);
            return true;
        }

        // stop USB interface when we exit the program
        private void stop_USB()
        {
            if (hdev != IntPtr.Zero) // check we have an open USB port 
            {
                try
                {
                    libUSB_Interface.usb_release_interface(hdev, 0);
                    libUSB_Interface.usb_close(hdev);
                }
                catch
                {
                }
            }
            // Save the current settings to CYCLOPS.CSV
            WriteCYCLPOSCSV();
        }

        // This thread runs at program load and reads data from Ozy
        public void USBLoop()
        {
            int ret;
            for (; ; ) // do this forever 
            {
                if (Cyclops_on)  // only read ADC data if USB is selected
                {
                    ret = libUSB_Interface.usb_bulk_read(hdev, 0x86, rbuf, rbufSize, 1000); // rbuf.Length = 2048
                    if (ret == rbufSize)
                        Process_Data(ref rbuf);
                    else
                    {
                        // we can't read from EP6 for some reason - perhaps Ozy not primed so 
                        // send some C&C frames to prime the pump.
                        bool force = true;
                        Thread.Sleep(10); // so we don't end up in a tight loop 
                        Debug.WriteLine("EP6 Read Error -  returns \t" + ret);
                        Debug.WriteLine("EP6 Read Error -  hdev \t" + hdev);
                        Ozy_send(force);  // force C&C data to Ozy to get started 
                    }
                }
                else Thread.Sleep(100);// Nothing selected so sleep for 100mS so we don't hog all the CPU
            } // for ever 
        } // USB_loop 

        // this is called when we exit the program - terminate the USB connection and save the 
        // current state of the Radio in the KK.csv file.
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (USB_thread_running)
                USB_thread.Abort();  // stop USB thread

            if (hdev != IntPtr.Zero)  // check we have an active USB port
                stop_USB();

            // save the last frequency used 
            // save program settings in KK.csv file
            //WriteKKCSV();
        }

        // get data from Ozy via the USB port
        public void Process_Data(ref byte[] rbuf)
        {
            #region how Process_Data works
            /*
             * 
             Process_Data  capitalizes on the fact that the USB data from Ozy is well ordered and
             consistent as it arrives from the USB bulk read with respect to where sync pulses and Cn, I, Q,
             and L,R values are located in the array. A simple initial check for the presence of sync pulses
             at the beginning of rbuf is all that is done to establish that the data within rbuf is likely
             to be valid Rx data.

            C0 bits 7-3 is command and control "address"
            C0 bit 0 is PTT from Penelope
            C0 bit 1 is "dash"
            C0 bit 2 is "dot" 

            when C0 "address" == 00000xxx then
            C1 bit 0 is Mercury ADC overload bit
            C2 is Mercury software serial number
            C3 is Penelope software serial number
            C4 is Ozy software serial number

            when C0 is binary 00001xxx then 
            C1 is bits 15-8 of penelope forward power (only 12 bits used)
            C2 is bits 7-0 of penelope forward power

            For a full description of the USB protocol see the document in \trunk\Documents.
            
            If the force flag is set then we send C&C data to Ozy even if we don't have 
            valid I&Q or Microphone  audio. This is so that Ozy knows what clocks to use
            and will start sending data. 
              
             */

            #endregion

            byte[] temp = new byte[4];
            float scaleIn = (float)(1.0 / Math.Pow(2, 23));     // scale factor for converting 24-bit int from ADC to float
            bool ADC_bit = false;

            // check that sync pulses are present in the front of rbuf...JAM
            if (rbuf[0] == sync && rbuf[1] == sync && rbuf[2] == sync)
            {
                IsSynced = true;  // use this to set the colour of the Sync LED when the timer fires
            }
            else
            {
                IsSynced = false;
                Debug.WriteLine(String.Format("Sync Failed - rbuf = \t{0}\t{1}\t{2}", rbuf[0], rbuf[1], rbuf[2]));
                return;
            }

            for (int frame = 0; frame < 4; frame++)
            {
                int coarse_pointer = frame * 512; //512 bytes total in each frame
                rc0 = rbuf[coarse_pointer + 3];
                rc1 = rbuf[coarse_pointer + 4];
                rc2 = rbuf[coarse_pointer + 5];
                rc3 = rbuf[coarse_pointer + 6];
                rc4 = rbuf[coarse_pointer + 7];

                ADC_bit = ((rc1 & 1) != 0) ? true : false;

                if ((rc0 & 0xf8) == 0)
                {
                    ADCOverload = ADC_bit ? true : false;

                    // get serial # Ozy  software, only need to do this once when Cyclops  starts
                    if (loop_count == 99)  // wait for 100 reads so Ozy data is stable
                    {
                        Ozy_FPGA_version = (int)rc4;    // Version number of Ozy FPGA code
                        Debug.WriteLine("Ozy version = \t" + Ozy_FPGA_version);
                    }
                    if (loop_count < 100)
                        loop_count++;
                }

                // get the I, and Q data from rbuf, convert to float, & put into SignalBuffer.cpx for DSP processing
                for (int i = 8; i < 512; i += 8)
                {
                    int k = coarse_pointer + i;
                    // get the I, Q, and mic bytes and form the integer data...JAM

                    // use the following rather than BitConverter.ToInt32 since uses much less CPU

                    // get an I sample...JAM
                    SignalBuffer.cpx[sample_no].real = scaleIn * (float)((rbuf[k + 2] << 8) | (rbuf[k + 1] << 16) | (rbuf[k] << 24));

                    // get a Q sample
                    SignalBuffer.cpx[sample_no].imag = scaleIn * (float)((rbuf[k + 5] << 8) | (rbuf[k + 4] << 16) | (rbuf[k + 3] << 24));

                    // This single sample is now complete.
                    // The routine will generate 504 such samples from each rbuf that is processed.
                    // Whenever we've accumulated iqsize samples, process them.
                    sample_no++;            // increment the sample number counter
                    if (sample_no == iqsize)
                    {
                        sample_no = 0;
                        read_ready = false;           // just in case we try to display whilst data is changing
                        rcvr.DoDSPProcess(ref SignalBuffer.cpx, ref outbuffer);  // Do all the DSP processing for rcvr

                        // If SampleRate > 48000, decimate by skipping samples as we place them into the AudioRing buffer
                        // since sample rate to Ozy is always 48k.
                        int SampleSpacing = SampleRate / 48000;

                        for (int sample = 0; sample < outbufferSize; sample += SampleSpacing)
                        {
                            AudioRing.Write(outbuffer[sample]);
                        }

                        read_ready = true;  // flag used to indicate to the graphic display code we have data ready

                    }
                    // If SampleRate is 48000, send a frame to Ozy for every frame read
                    // If SampleRate is 96000, send a frame for every two read (audio is decimated by factor of two)
                    // If SampleRate is 192000, send a frame for every four read (audio is decimated by factor of four)
                    SampleCounter += 48000;
                    if (SampleCounter >= SampleRate)
                    {
                        SampleCounter = 0;
                        bool force = false;   // conventional write to Ozy
                        Ozy_send(force);  // send the data to Ozy over USB
                    }
                }
            }
        }

        [FlagsAttribute]
        enum C1Bits
        {
            TenMHzAtlas = 0x00,
            TenMHzMercury = 0x08,
            Clock122Mercury = 0x10,
            MercuryPresent = 0x40,

        }
        int send_state = 0;
        // Send four frames of 512 bytes to Ozy
        // The left & right audio to be sent comes from AudioRing buffer, which is filled by ProcessData()
        private void Ozy_send(bool force)
        {
            #region how Ozy_send works
            /*          
             
             Send frames to Ozy -- frames consist of 512 bytes as follows:
             <0x7f><0x7f><0x7f><C0><C1><C2><C3><C4><Left><Left><Right><Right><I><I><Q><Q><Left>..... 

             C0 bit 0 is "PTT"
             
             if CO = 0x0000_000x then
             C1 bits:
             7      Mic source: 0 = Janus, 1 = Penelope
             6,5    boards present:  00 neither, 01 Penelope, 10 Mercury, 11 both
             4      122.88MHz source: 0 Penelope, 1 Mercury
             3,2    10MHz ref source: 00 atlas, 01 Penelope, 10 Mercury
             1,0    sampling rate:    00 48kHz, 01 96kHz, 10 192 kHz
             * 
             C2 bits:
             7-1    Penelope open collector outputs 6-0
             0      xmt mode: 1 class E, 0 all other modes

             C3 bits:
             7    Alex Rx out: 0 off, 1 on
             6,5  Alex Rx antenna: 00 none, 01 Rx1, 10 Rx2, 11 XV
             4    Mercury ADC random: 0 off, 1 on
             3    Mercury ADC dither:  0 off, 1 on
             2    Mercury preamp: 0 off, 1 on
             1,0  Alex attenuator: 00 0dB, 01 10dB, 10 20dB, 11 30dB

             C4 bits
             7-3  unused
             1,0  Alex Tx relay: 00 Tx1, 01 Tx2, 10 Tx3
             2    1 = full duplex, 0 = simplex 
             if C4[2] = 0 and C0 = 0x0000_001x then C1..C4 hold frequency in Hz for Penny and Mercury, C1 = MSB
             if C4[2] = 1 and C0 = 0x0000_001x then C1..C4 hold frequency in Hz for Penny, C1 = MSB
             if C4[2] = 1 and C0 = 0x0000_010x then C1..C4 hold frequency in Hz for Mercury, C1 = MSB
             

             For a full description of the USB protocol see the document in \trunk\Documents
             */

            #endregion

            // if force is set then send C&C anyway even if no data available
            if (!force)
            {
                if (AudioRing.Count < 252)
                {
                    return;  // need enough data for 4 frames
                }
            }

            float SampleReal = 1.0f;
            float SampleImag = 1.0f;
            float scaleOut = (float)Math.Pow(2, 15);  // scale factor to convert DSP output to 16-bit short int audio output sample\
            // is this linked to the rbufSize???
            const int toOzySize = 2048;
            byte[] to_Ozy = new byte[toOzySize];       // array to send to Ozy via USB bulk write
            //short I_data = 0;
            //short Q_data = 0;
            int frame_number = 0;
            int pntr; int x = 0;


            // send 4 frames of 512 bytes for a total of 2048 bytes to Ozy via a USB bulk write...JAM
            for (frame_number = 0; frame_number < 4; frame_number++)
            {
                // cycle between C0 = 0x00, 0x02 and 0x04 using a state machine so its easy to add other states in the future
                switch (send_state)
                {
                    case 0:
                        {
                            C0 = 0x00; C1 = 0x00; C2 = 0x00; C4 = 0x00;

                            // Select 10MHz reference clock
                            if(Mercury10MHz.Checked)
                            {
                                C1 = (byte)(C1Bits.MercuryPresent | C1Bits.Clock122Mercury | C1Bits.TenMHzMercury);
                            }
                            else  // 10MHz comes from Atlas bus 
                            {
                                C1 = (byte)(C1Bits.MercuryPresent | C1Bits.Clock122Mercury | C1Bits.TenMHzAtlas);
                            }

                            // set sampling rate bits in C1
                            switch (SampleRate)
                            {
                                case 480000: break;
                                case 96000:
                                    C1 = (byte)(C1 | 0x01);
                                    break;
                                case 192000:
                                    C1 = (byte)(C1 | 0x02);
                                    break;
                            }

                            // set Preamp on
                            C3 = 0x04;
                            C2 = 0x00;  // default Open Collector outputs to off
                            
                            send_state = 1; // select next state
                            break;
                        }
                    case 1:
                        {
                            // set CO  to  0x02 and send frequency Mercury
                            C0 = 0x02; C1 = frequency[3]; C2 = frequency[2]; C3 = frequency[1]; C4 = frequency[0]; // send frequency data
                            send_state = 0;
                            break;
                        }
                }


                pntr = frame_number * 512;
                to_Ozy[pntr] = sync; to_Ozy[pntr + 1] = sync; to_Ozy[pntr + 2] = sync;
                to_Ozy[pntr + 3] = C0; to_Ozy[pntr + 4] = C1; to_Ozy[pntr + 5] = C2;
                to_Ozy[pntr + 6] = C3; to_Ozy[pntr + 7] = C4;

                for (x = 8; x < 512; x += 8)        // fill out one 512-byte frame
                {
                    AudioRing.Read(ref SampleReal, ref SampleImag);

                    // FirstLOfreq is sent in the what is normally the I_data position 

                    // use the following rather than BitConverter.GetBytes since it uses less CPU 
                    int IntValue;
                    IntValue = (int)(scaleOut * SampleReal);
                    to_Ozy[pntr + x] = (byte)(IntValue >> 8);        // left hi
                    to_Ozy[pntr + x + 1] = (byte)(IntValue & 0xff);  // left lo
                    IntValue = (int)(scaleOut * SampleImag);
                    to_Ozy[pntr + x + 2] = (byte)(IntValue >> 8);    // right hi
                    to_Ozy[pntr + x + 3] = (byte)(IntValue & 0xff);  // right lo

                    // send I & Q data to Qzy 
                    to_Ozy[pntr + x + 4] = FirstLOfreq[1];          // Send first LO frequency 
                    to_Ozy[pntr + x + 5] = FirstLOfreq[0];   
                    to_Ozy[pntr + x + 6] = (byte) 0;                // Q_data[0]
                    to_Ozy[pntr + x + 7] = (byte) 0;                // Q_data[1]  
                }
            }

            if (Cyclops_on)  // send the frames to Ozy via the USB
            {
                int ret;
                //do
                //{
                ret = libUSB_Interface.usb_bulk_write(hdev, 0x02, to_Ozy, toOzySize, 100);
                //} while (ret != toOzySize);
                // TODO: Need to add error routine if write to USB fails
                if (ret != toOzySize)
                {
                    Debug.WriteLine("Write to Ozy failed - returned \t" + ret);
                }
            }
        }

        // Slider control sets transceiver center frequency
        private void set_frequency_Scroll_1(object sender, EventArgs e)
        {
            // when the frequency slider has focus the mouse scroll wheel
            // can be used to increment the frequency by +/- step_size.

            // the scroll bar can also be moved with the arrow and Up/Down keys 
            // make the scroll bar large and small step sizes a function of the step size 
            set_frequency.LargeChange = ((step_size + 1) * 100) - step_size;
            set_frequency.SmallChange = (step_size + 1);
            // check we are still within the band and update the tune and display frequencies
            Frequency_change();
        }

        // select the step size that the mouse scroll wheel does
        public void stepSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            // since we have alreade taken a step use (step size - 1)
            // TODO: this works OK for the mousewheel on my mouse but check this is universal
            switch (stepSize.Text)
            {
                case "1Hz": step_size = 0; break;
                case "10Hz": step_size = 9; break;
                case "100Hz": step_size = 99; break;
                case "1kHz": step_size = 999; break;
            }
            set_frequency.Focus(); // return focus to the frequency slider
        }

        private void Frequency_change()
        {
            UpdateGraphBackgrounds();

            // we need to check if we are too close to the band edge to take a step 
            if (set_frequency.Value > previous_value && set_frequency.Value < (set_frequency.Maximum - step_size))
                set_frequency.Value += step_size;  // add step_size since scroll wheel has added 1 already
            else if (set_frequency.Value < previous_value && set_frequency.Value > (set_frequency.Minimum + step_size + 1))
                set_frequency.Value -= step_size; // subtract step_size since scroll wheel has subtracted 1 already
            // else use the current value

            // round frequency to step size
            int temp_frequency = set_frequency.Value; // use a temp value so we don't have to worry about the band edges.
            temp_frequency = temp_frequency / (step_size + 1);
            temp_frequency = temp_frequency * (step_size + 1);
            set_frequency.Value = temp_frequency;

            // display the new frequency and send to Ozy
            DisplayFrequency(set_frequency.Value);
            // save current value so we can compare with next value so we can determine tune direction
            previous_value = set_frequency.Value;
        }


        // when ever timer1 fires we update the bandscope
        private void timer1_Tick(object sender, EventArgs e)
        {
            //UpdateRate sets the display FPS rate, its located on Form1
            timer1.Interval = 1000 / (int)UpdateRate.Value;  //timer value from 1000 to 50 i.e. 1 second to 20mS

            // display the status of Sync and ADC overload on the 'LEDs'
            SyncLED.BackColor = IsSynced ? Color.Green : Color.Red;
            ADCoverloadButton.BackColor = ADCOverload ? Color.Red : SystemColors.Control;


            if (read_ready)  // update the display if data is available 
            {
                read_ready = false;

                // get a wide-bandwidth sample
                ProcessWideBandData();

                // get bandwidth samples as IQ data
                GetIQSamples();

                ComputeDisplay();

                if (chkSpec.Checked || ShowI || ShowQ)
                {
                    useSpectrumBackground = true;

                    pictureBoxSpectrum.Invalidate(); // causes RefreshScope to run 
                }
                else
                {
                    useSpectrumBackground = false;
                    pictureBoxSpectrum.BackgroundImage = null;
                }

                if (chkWideSpec.Checked)
                {
                    useWidebandBackground = true;
                    pictureBoxWideband.Invalidate(); // causes RefreshScope to run
                }
            }
        }

        // Get ozy firmware version string - 8 bytes,  returns null for error - thanks to Bill KD5TFD for this code
        private string getOzyFirmwareString()
        {
            if (hdev == IntPtr.Zero)
            {
                return null;
            }

            // the following set of declarations MUST match the values used in the FX2 code - hpsdr_commands.h
            byte VRQ_SDR1K_CTL = 0x0d;
            byte SDR1KCTRL_READ_VERSION = 0x7;
            byte VRT_VENDOR_IN = 0xC0;
            byte[] buf = new byte[8];
            int rc = libUSB_Interface.usb_control_msg(hdev, VRT_VENDOR_IN, VRQ_SDR1K_CTL, SDR1KCTRL_READ_VERSION, 0, buf, buf.Length, 1000);
            Debug.WriteLine("read version rc: " + rc);
            Debug.WriteLine("read version hdev = \t" + hdev);

            string result = null;

            if (rc == 8)    // got length we expected
            {
                char[] cbuf = new char[8];
                for (int i = 0; i < 8; i++)
                {
                    cbuf[i] = (char)(buf[i]);
                }
                result = new string(cbuf);
                Debug.WriteLine("version: >" + result + "<");
            }
            return result;
        }

        private void Start()
        {
            // start the thread that reads the USB port on Ozy
            if (!start_USB())
            {
                MessageBox.Show("No Ozy board found  - Check HPSDR is connected and powered");
                OnOffButton_Click(this, EventArgs.Empty); // Toggle ON/OFF Button to OFF
                return;
            }

            // Check that FX2 has been loaded with software, if not call initozy11.bat to load it
            Ozy_version = getOzyFirmwareString(); // Get ozy firmware version string - 8 bytes,  returns null for error

            if (Ozy_version == null)
            {
                this.Cursor = Cursors.WaitCursor;  // show hour glass cursor whilst we load Ozy FPGA
                // call a process to run initozy11.bat in current directory
                ProcessStartInfo start_info = new ProcessStartInfo();
                start_info.FileName = "initozy11.bat";
                start_info.UseShellExecute = true;
                Process p = new Process();
                p.StartInfo = start_info;
                bool rc = p.Start();
                System.Console.WriteLine("start returned: " + rc);
                p.WaitForExit();
                System.Console.WriteLine("OzyInit completes");
                stop_USB();  // need to close and re-open USB port since it renumerated
                start_USB();
                this.Cursor = Cursors.Default;  // revert to normal cursor

                // get ozy code version now
                Ozy_version = getOzyFirmwareString();
            }

            // check that we are using the correct version of FX2 code
            if (Ozy_version != "20090524")
            {
                // note that the Ozy_version may be null.  If so, the old code here would have generated an exception
                // and not failed softly as intended.  The current/new code handles that case reasonably...
                MessageBox.Show(" Wrong version of Ozy code found " + (Ozy_version == null ? "(Not Found!)" : Ozy_version.ToString()) + "\n-should be 20090524");

                OnOffButton_Click(this, EventArgs.Empty); // Toggle ON/OFF Button to OFF
                stop_USB();
                return;
            }

            Debug.WriteLine("Starting USB Thread");

            // start thread to read Ozy data from USB.   USBLoop merely calls Process_Data, 
            // which calls usb_bulk_read() and rcvr.Process(),
            // and stuffs the demodulated audio into AudioRing buffer
            USB_thread = new Thread(new ThreadStart(USBLoop));
            USB_thread.Name = "USB Loop";
            USB_thread.Priority = ThreadPriority.Highest; // run USB thread at high priority
            //USB_thread.IsBackground = true;  // do we need this ?
            USB_thread.Start();
            USB_thread_running = true;
            timer1.Enabled = true;  // start timer for bandscope update.
        }


        private void Stop()
        {
            if (USB_thread_running)
            {
                USB_thread.Abort();  // stop USB thread
                USB_thread_running = false;
            }
            timer1.Enabled = false;
            stop_USB();          // kill the USB port 
            SyncLED.BackColor = SystemColors.Control;  // no sync so set LED to background
        }



        private void chkSpec_CheckedChanged(object sender, EventArgs e)
        {
            SetSplitterBar();
            set_frequency.Focus(); // return focus to the frequency slider
        }

        private void chkWideSpec_CheckedChanged(object sender, EventArgs e)
        {
            SetSplitterBar();
            set_frequency.Focus(); // return focus to the frequency slider
        }

        // process key strokes from the keyboard 
        protected override void OnKeyDown(KeyEventArgs e)
        {
            bool display = false;
            Debug.WriteLine("got a key \t" + e.KeyCode);

            // process key presses from the numeric keypad
            switch (e.KeyCode)
            {
                case Keys.NumPad0: KeypadFrequency += "0"; keycount++; break;
                case Keys.NumPad1: KeypadFrequency += "1"; keycount++; break;
                case Keys.NumPad2: KeypadFrequency += "2"; keycount++; break;
                case Keys.NumPad3: KeypadFrequency += "3"; keycount++; break;
                case Keys.NumPad4: KeypadFrequency += "4"; keycount++; break;
                case Keys.NumPad5: KeypadFrequency += "5"; keycount++; break;
                case Keys.NumPad6: KeypadFrequency += "6"; keycount++; break;
                case Keys.NumPad7: KeypadFrequency += "7"; keycount++; break;
                case Keys.NumPad8: KeypadFrequency += "8"; keycount++; break;
                case Keys.NumPad9: KeypadFrequency += "9"; keycount++; break;
                case Keys.Back:
                    if (keycount > 0)  // remove the last character entered
                    {
                        KeypadFrequency = KeypadFrequency.Remove(KeypadFrequency.Length - 1);
                        --keycount;
                    }
                    break;
                case Keys.Enter:
                    if (keycount > 0)   // so we don't trigger on the main Enter Key 
                        display = true;
                    break;
                case Keys.Decimal: KeypadFrequency += "."; break;
            }

            // display the frequency as it is entered but only if entered on the keypad 
            if (keycount > 0)
                display_freq.Text = KeypadFrequency;

            float temp = 0.0f;
            int setfrequency;
            if (display)  // user has pressed enter key 
            {
                try  // data format may be incorrect so check
                {
                    temp = (float)Convert.ToDecimal(KeypadFrequency); // convert to a float 
                }

                catch // format error so tidy up and return
                {
                    Frequency_change();     // display the previous frequency
                    set_frequency.Focus(); // return focus to the frequency slider
                    KeypadFrequency = null;
                    keycount = 0;
                    return;
                }
                float temp2 = temp * 1000000.0F;  // Multiply by 10e6 to give Hz
                setfrequency = (int)temp2; // convert to an integer
                // check frequency is valid
                if (keycount > 0 && keycount < 9 && setfrequency < 1000000001)
                {
                    Debug.WriteLine(setfrequency);
                    set_frequency.Value = setfrequency;
                    // display the new frequency and send to Ozy
                    DisplayFrequency(setfrequency);
                    KeypadFrequency = null;
                    keycount = 0;
                    set_frequency.Focus(); // return focus to the frequency slider
                    return;
                }
                else  // error, abort
                {
                    Frequency_change();     // display the previous frequency
                    set_frequency.Focus(); // return focus to the frequency slider
                    KeypadFrequency = null;
                    keycount = 0;
                    return;
                }
            }
        }

        // Calculate required frequencies and display 
        private void DisplayFrequency(int setfrequency)
        {
            TuneFreq = set_frequency.Value;
            FirstLOFrequency = (int)(TuneFreq / 1e6 + FirstIF);
            FirstIFFrequency = (FirstLOFrequency * 1e6) - TuneFreq;
            SecondIFFrequency = Math.Abs((int)(FirstIFFrequency - SecondLO));
            MercuryFrequency = (int)(MercuryClock - SecondIFFrequency);

            // display the various frequencies
            Tune.Text = (TuneFreq / 1e6).ToString();
            FirstLO.Text = FirstLOFrequency.ToString();
            FirstIFFreq.Text = (FirstIFFrequency / 1e6).ToString();
            SecondIF.Text = SecondIFFrequency.ToString();
            Mercury.Text = MercuryFrequency.ToString();
                   
            
            // convert Mercury frequency to an array of bytes ready to send to Ozy
            frequency = BitConverter.GetBytes(MercuryFrequency);
            // convert First Local Oscillator Frequency to an array of bytes also
            FirstLOfreq = BitConverter.GetBytes(FirstLOFrequency);
            // format the integer frequency to have a decimal point between the MHz digits.
            double temp_freq = (double)setfrequency / 1000000.0f;
            // convert to a string with 6 digits to the right of the decimal point 
            string freq = String.Format(nfi, "{0:F6}", (double)temp_freq);
            // strip off the last 3 digits, add a space then add them back 
            display_freq.Text = freq.Remove(freq.Length - 3) + ' ' + freq.Substring(freq.Length - 3, 3);
        }

        Image pictureBoxWidebandBackground = null;
        bool useWidebandBackground = false;
        private void pictureBoxWideband_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (useWidebandBackground)
            {
                if ((pictureBoxWidebandBackground == null) || (pictureBoxWideband.BackgroundImage == null) || updateWidebandGraphBackground)
                {
                    pictureBoxWideband.BackgroundImage = null;
                    DrawWidebandGrid(g);            // draw the background grid
                    pictureBoxWidebandBackground = pictureBoxWideband.Image;
                    pictureBoxWideband.BackgroundImage = pictureBoxWidebandBackground;
                    updateWidebandGraphBackground = false;
                    UpdateGraphBackgrounds();
                }
            }
            else
            {
                pictureBoxWideband.BackgroundImage = null;
            }

            if (chkWideSpec.Checked) // if a bandscope etc is selected then display it
                RefreshWidebandScope(g);
        }

        Image pictureBoxSpectrumBackground = null;
        bool useSpectrumBackground = false;
        private void pictureBoxSpectrum_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (useSpectrumBackground)
            {
                if ((pictureBoxSpectrumBackground == null) || (pictureBoxSpectrum.BackgroundImage == null) || updateSpectrumGraphBackground)
                {
                    pictureBoxSpectrum.BackgroundImage = null;
                    DrawSpectrumGrid(g);            // draw the background grid
                    pictureBoxSpectrumBackground = pictureBoxSpectrum.Image;
                    pictureBoxSpectrum.BackgroundImage = pictureBoxSpectrumBackground;
                    updateSpectrumGraphBackground = false;
                }
            }
            else
            {
                pictureBoxSpectrum.BackgroundImage = null;
            }

            if (chkSpec.Checked || ShowI || ShowQ) // if a bandscope etc is selected then display it
                RefreshSpectrumScope(g);
        }

        private void pictureBoxSpectrum_MouseDown(object sender, MouseEventArgs e)
        {

            // Change the mouse cursor to a hand and enable frequency drag. If we have right clicked then 
            // ShowCross will be true in which case set the frequency to the current mouse position.
            if (Control.MouseButtons == MouseButtons.Left) 
            {
                pictureBoxSpectrum.Cursor = Cursors.Hand; // change cursor to a hand

                // if the cross is shown then set frequency to the X location of the mouse
                if (ShowCross)
                {
                    // calculate the number of Hz each pixel represents, will be  sample rate/screen bandscope width
                    int HzPerPixel = SampleRate / pictureBoxSpectrum.Width;
                    // calculate the  how many Hz the X mouse location represents from the left edge of the screen
                    int offset = MousePositionX * HzPerPixel;
                    // the frequency at the left edge of the screen will be the tuned frequency - sample rate/2
                    int left_edge_frequency = set_frequency.Value - SampleRate / 2;
                    // add the offset to the left edge frequency
                    // do so in a way that does NOT generate an exception when going too low in frequency
                    if (left_edge_frequency + offset <= set_frequency.Minimum)
                    {
                        set_frequency.Value = set_frequency.Minimum;
                    }
                    else if (left_edge_frequency + offset >= set_frequency.Maximum)
                    {
                        set_frequency.Value = set_frequency.Maximum;
                    }
                    // display the new frequency and send to Ozy
                    DisplayFrequency(set_frequency.Value);
                    set_frequency.Focus(); // return focus to the frequency slider
                }
            }
            // if the right mouse button is down then show large cross 
            else if (Control.MouseButtons == MouseButtons.Right)  // change the cursor to to a large cross
            {
                pictureBoxSpectrum.Cursor = Cursors.Cross;         // change cursor to a small cross
                // set the ShowCross flag, this is read in the DrawGrid() Method
                ShowCross = !ShowCross;                     // toggle the cross each right click
            }
        }

        private void pictureBoxSpectrum_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxSpectrum.Cursor = Cursors.Cross;
        }

        private void pictureBoxSpectrum_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxSpectrum.Cursor = Cursors.Default;
        }


        int mouse_delta_Y;
        private void pictureBoxSpectrum_MouseMove(object sender, MouseEventArgs e)
        {
            // get the current cursor shape so we can restore it upon leaving 
            Cursor CurrentCursor = pictureBoxSpectrum.Cursor;

            MousePositionX = e.X;   // get the mouse X and Y coordintes with respect to the bandscope
            MousePositionY = e.Y;   // these are used by the right mouse click over bandscope             
            Point MouseX = new Point();  // the mouse coordinates have a type Point
            MouseX = Control.MousePosition;  // get current mouse postion (X,Y) with respect to the screen
            // so we can drag tune outside the width of the bandscope area 
            delta = MouseOld.X - MouseX.X;  // get the difference between the last and current Mouse X coordinate
            int abs_delta = Math.Abs(delta);
            mouse_delta_Y = MouseOld.Y - MouseX.Y; // get the diffetence between the last and current Mouse Y coordinate
            int abs_mouse_delta_Y = Math.Abs(mouse_delta_Y);

            if (Control.MouseButtons == MouseButtons.Left) // && FrequencyDrag) // do this if the left mouse button is down
            {
                // if mouse has moved left then decrease frequency but first 
                // we need to check if we are too close to the band edges to move 
                if (delta < 0)
                {
                    if (set_frequency.Value > (set_frequency.Minimum + abs_delta * (SampleRate / pictureBoxSpectrum.Width)))
                        set_frequency.Value -= abs_delta * (SampleRate / pictureBoxSpectrum.Width);
                }
                // if mouse has moved right then decrease frequency but first
                // we need to check if we are too close to the band edges to move  
                else if (delta > 0)
                {
                    if (set_frequency.Value < (set_frequency.Maximum - abs_delta * (SampleRate / pictureBoxSpectrum.Width)))
                        set_frequency.Value += abs_delta * (SampleRate / pictureBoxSpectrum.Width);
                }

                DisplayFrequency(set_frequency.Value);

                set_frequency.Focus(); // return focus to the frequency slider
            }
            MouseOld = MouseX; // save the last mouse X location for next time the mouse moves
        }

        private void pictureBoxSpectrum_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBoxSpectrum.Cursor = Cursors.Cross;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            UpdateGraphBackgrounds();
        }

        public void OnOffButton_Click(object sender, EventArgs e)
        {
            if (OnOffButton.BackColor == SystemColors.Control)
            {
                OnOffButton.BackColor = Color.Green;
                OnOffButton.Text = "ON";
                Cyclops_on = true;
                Start();
                set_frequency.Focus(); // set focus to the frequency slider
            }
            else
            {
                OnOffButton.BackColor = SystemColors.Control;
                OnOffButton.Text = "OFF";
                Thread.Sleep(100);                          // Delay so the PTT off is sent to Ozy
                Stop();
                Cyclops_on = false;                              // set this last so PTT off gets sent
                set_frequency.Focus(); // set focus to the frequency slider
            }
        }

        private void Smoothing_ValueChanged(object sender, EventArgs e)
        {
            rcvr.PowerSpectrumSmoothingFactor = (float)((float)Smoothing.Value / 100f); 
        }





    }
}


