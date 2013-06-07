using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace PowerSDR
{
    public partial class LincorForm : Form
    {
        #region constructor
        public static Console console;
        //public Console konsole;
        //public LincorForm(Console form)
        public LincorForm()
        {
            //this.konsole = form;
            InitializeComponent();
        }

        #endregion

        #region variables

        double DT = 20.0e-9;
        int L, Delay;
        double samplerate;

        #endregion

        #region event handlers

        private void chkDSPXLAutoCorrect_CheckedChanged(object sender, EventArgs e)
        {
            lincor.WaitTime = (double)udDSPXLCalWait.Value;
            if (chkDSPXLAutoCorrect.Checked)
                lincor.automode();
            else
                lincor.reset();
        }

        private void btnDSPXLCalibrate_Click(object sender, EventArgs e)
        {
            lincor.mancal();
        }

        private void btnDSPXLReset_Click(object sender, EventArgs e)
        {
            chkDSPXLAutoCorrect.Checked = false;
            Thread.Sleep(10);
            lincor.reset();
        }

        private void udDSPXLIntervals_ValueChanged(object sender, EventArgs e)
        {
            lincor.Intervals = (int)udDSPXLIntervals.Value;
        }

        private void udDSPXLNsamps_ValueChanged(object sender, EventArgs e)
        {
            lincor.Nsamps = (int)udDSPXLNsamps.Value;
        }

        private void udDSPXLCalWait_ValueChanged(object sender, EventArgs e)
        {
            lincor.WaitTime = (double)udDSPXLCalWait.Value;
        }

        bool tt_cal = false;
        private void btnDSPTTCal_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(LincorTTCal));
            t.Name = "TwoTone Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void LincorTTCal()
        {
            tt_cal = true;
            lincor.reset();
            lincor.setLincorTwoTone(-750, +750, 0.500, 0.500, 1, 0);
            Thread.Sleep(100);
            bool oldmox = console.MOX;
            console.MOX = true;
            Thread.Sleep(200);
            lincor.mancal();
            Thread.Sleep(1100);
            console.MOX = oldmox;
            if (!chkDSPXLTwoTone.Checked)
                lincor.setLincorTwoTone(-750, +750, 0.500, 0.500, 0, 0);
            tt_cal = false;
        }

        private void chkDSPXLTwoTone_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDSPXLTwoTone.Checked)
            {
                lincor.setLincorTwoTone(-750, +750, 0.500, 0.500, 1, 0);
            }
            else
            {
                lincor.setLincorTwoTone(-750, +750, 0.500, 0.500, 0, 0);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lincor.getinfo();
            lblInfo0.Text = lincor.Info[0].ToString();
            lblInfo1.Text = lincor.Info[1].ToString();
            lblInfo2.Text = lincor.Info[2].ToString();
            lblInfo3.Text = lincor.Info[3].ToString();
            lblInfo4.Text = lincor.Info[4].ToString();
        }

        private void btnDSPXLPrintSamples_Click(object sender, EventArgs e)
        {
            lincor.doLincorPrintSamples(txtSamplesFile.Text + ".txt");
        }

        private void udDSPXLPhnum_ValueChanged(object sender, EventArgs e)
        {
            lincor.setTxDelayPhnum((int)udDSPXLPhnum.Value);
            L = (int)(0.5 + 1.0 / (samplerate * DT));
            Delay = (int)(1.0e9 / samplerate * (double)udDSPXLPhnum.Value / (double)L);
            lblDelay.Text = Delay.ToString();
        }

        private void udDSPXLMoxDelay_ValueChanged(object sender, EventArgs e)
        {
            lincor.setLincorMoxDelay((double)udDSPXLMoxDelay.Value);
        }

        private void btnDSPXLSweep_Click(object sender, EventArgs e)
        {
            chkDSPXLAutoCorrect.Checked = false;
            lincor.reset();
            console.MOX = true;
            Thread.Sleep(100);
            lincor.setLincorTwoTone(-750, +750, 0.500, 0.500, 1, 1);
        }

        private void btnDSPXLSave_Click(object sender, EventArgs e)
        {
            System.IO.Directory.CreateDirectory("Lincor");
            lincor.doLincorSaveCorrection("Lincor/" + txtDSPXLSave.Text + ".txt");
        }

        private void btnDSPXLRestore_Click(object sender, EventArgs e)
        {
            lincor.doLincorRestoreCorrection("Lincor/" + txtDSPXLRestore.Text + ".txt");
        }

        #endregion

        #region public methods

        public void ForceLincor()
        {
            EventArgs e = EventArgs.Empty;
            lincor.reset();
            chkDSPXLAutoCorrect_CheckedChanged(this, e);
            chkDSPXLTwoTone_CheckedChanged(this, e);
            udDSPXLIntervals_ValueChanged(this, e);
            udDSPXLNsamps_ValueChanged(this, e);
            udDSPXLCalWait_ValueChanged(this, e);
            udDSPXLPhnum_ValueChanged(this, e);
            udDSPXLMoxDelay_ValueChanged(this, e);
        }

        double vfoA_oldfreq;
        double vfoB_oldfreq;
        bool vfoA_tx;
        bool vfoB_tx;
        double rit;
        double xit;
        bool riton;
        bool xiton;
        bool console_set = false;

        public void fixConsole(bool tx)
        {
            if (tx)
                setConsole();
            else
                resetConsole();
        }

        /*private void setConsole()
        {
            //
             // This version of code assumes that the TX samples are being returned on receiver 2 and
             // that receiver 1 is being used to receive the OUTPUT of the amplifier to be corrected.
             // VFO-A + RIT is assumed to hold the desired frequency and VFO-B will be forced to this same
             // frequency during transmissions so that RX2 can capture and return the transmit sample
             // stream.  XIT is not allowed as receive and transmit frequencies must match.
           //
            if ((lincor.auto == 1) || tt_cal)
            {
                console.RX2Enabled = true;
                vfoA_oldfreq = console.VFOAFreq;
                vfoB_oldfreq = console.VFOBFreq;
                vfoA_tx = console.VFOATX;
                vfoB_tx = console.VFOBTX;
                rit = console.RITValue;
                xit = console.XITValue;
                riton = console.RITOn;
                xiton = console.XITOn;

                console.VFOBFreq = vfoA_oldfreq;
                if (riton) console.VFOBFreq += 0.000001 * rit;
                console.XITOn = false;
                console.VFOATX = false;
                console.VFOBTX = true;
                console_set = true;
            }
        }*/

        private void setConsole()
        {
            //Version assuming TX samples come back in rx5.  rx5 will be set to VFOA freq.
            //Duplex must be used to get RX samples back in rx1 while transmitting on VFOA.
            if ((lincor.auto == 1) || tt_cal)
            {
                if (console.VFOATX) 
                    JanusAudio.SetVFOfreqRX5(console.VFOAFreq);
                if (console.VFOBTX)
                    if (console.RX2Enabled == true)
                        JanusAudio.SetVFOfreqRX5(console.VFOBFreq);
                    else
                    {
                        console.VFOBTX = false;
                        console.VFOATX = true;
                    }
                rit = console.RITValue;
                xit = console.XITValue;
                riton = console.RITOn;
                xiton = console.XITOn;

                if (riton) console.XITValue = (int)rit;

                console_set = true;
            }
        }

        private void resetConsole()
        {
            if (console_set)
            {
                console.XITValue = (int)xit;
                console_set = false;
            }
        }

        public void setLincorSampleRate(double rate)
        {
            samplerate = rate;
            lincor.setLincorSampleRate(samplerate);
            L = (int)(0.5 + 1.0 / (samplerate * DT));
            Delay = (int)(1.0e9 / samplerate * (double)udDSPXLPhnum.Value / (double)L);
            lblDelay.Text = Delay.ToString();
            udDSPXLPhnum.Maximum = L - 1;
            if ((int)udDSPXLPhnum.Value > L - 1) udDSPXLPhnum.Value = L - 1;
        }

        #endregion

    }



    unsafe static class lincor
    {
        #region DllImport - Main

        [DllImport("LincorDLL.dll", EntryPoint = "newLincor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void newLincor();

        [DllImport("LincorDLL.dll", EntryPoint = "delLincor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void delLincor();

        [DllImport("LincorDLL.dll", EntryPoint = "setLincor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLincor(int intervals, int nsamps, int cset);

        [DllImport("LincorDLL.dll", EntryPoint = "controlLincor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void controlLincor(int reset, int mancal, int automode, double wait_time);

        [DllImport("LincorDLL.dll", EntryPoint = "getInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getInfo(IntPtr info);

        [DllImport("LincorDLL.dll", EntryPoint = "setLincorSampleRate", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLincorSampleRate(double rate);

        [DllImport("LincorDLL.dll", EntryPoint = "setTxDelayPhnum", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setTxDelayPhnum(int phnum);

        [DllImport("LincorDLL.dll", EntryPoint = "LincorControlAndCapture", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LincorControlAndCapture(int buffsize, float* Itxbuff, float* Qtxbuff, float* Irxbuff, float* Qrxbuff, bool mox, bool solidmox);

        [DllImport("LincorDLL.dll", EntryPoint = "LincorGenTwoTone", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LincorGenTwoTone(int size, float* Ibuff, float* Qbuff);

        [DllImport("LincorDLL.dll", EntryPoint = "lincorCorrect", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lincorCorrect(int size, float* Ibuff, float* Qbuff);

        [DllImport("LincorDLL.dll", EntryPoint = "setLincorMoxDelay", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLincorMoxDelay(double delay);

        #endregion

        #region DLLImport - Internals

        [DllImport("LincorDLL.dll", EntryPoint = "calcCorrection", CallingConvention = CallingConvention.Cdecl)]
        public static extern void calcCorrection();

        [DllImport("LincorDLL.dll", EntryPoint = "builder", CallingConvention = CallingConvention.Cdecl)]
        public static extern void builder(int points, IntPtr x, IntPtr y, int intervals, IntPtr t, ref int info, IntPtr coef);

        #endregion

        #region DLLImport - Utilities

        [DllImport("LincorDLL.dll", EntryPoint = "getInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getInfo(int* Info);

        [DllImport("LincorDLL.dll", EntryPoint = "getSamplePointers", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getSamplePointers(IntPtr* TXptr, IntPtr* RXptr);

        [DllImport("LincorDLL.dll", EntryPoint = "getCoefs", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getCoefs(int set, IntPtr cmag, IntPtr ccos, IntPtr csin);

        [DllImport("LincorDLL.dll", EntryPoint = "doLincorPrintSamples", CallingConvention = CallingConvention.Cdecl)]
        public static extern void doLincorPrintSamples(string filename);

        [DllImport("LincorDLL.dll", EntryPoint = "setLincorTwoTone", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLincorTwoTone(double ftone1, double ftone2, double amptone1, double amptone2, int doit, int mode);

        [DllImport("LincorDLL.dll", EntryPoint = "doLincorSaveCorrection", CallingConvention = CallingConvention.Cdecl)]
        public static extern void doLincorSaveCorrection(string filename);

        [DllImport("LincorDLL.dll", EntryPoint = "doLincorRestoreCorrection", CallingConvention = CallingConvention.Cdecl)]
        public static extern void doLincorRestoreCorrection(string filename);

        #endregion

        #region variables

        static int intervals = 32;
        static int nsamps = 10000;
        static int cset = 0;
        static double wait_time = 1.0;
        //static double samplerate = 192000;
        static int rst = 1;                    //reset assertion
        static int mcl = 0;                    //manual calibration request
        public static int auto = 0;                   //auto correct mode

        #endregion

        #region properties

        public static int Intervals
        {
            get { return intervals; }
            set
            {
                intervals = value;
                setLincor(intervals, nsamps, cset);
            }
        }

        public static int Nsamps
        {
            get { return nsamps; }
            set
            {
                nsamps = value;
                setLincor(intervals, nsamps, cset);
            }
        }

        public static double WaitTime
        {
            get { return wait_time; }
            set
            {
                wait_time = value;
                controlLincor(rst, mcl, auto, wait_time);
            }
        }

        #endregion

        #region public methods

        public static void reset()
        {
            rst = 1;
            mcl = 0;
            auto = 0;
            controlLincor(rst, mcl, auto, wait_time);
        }

        public static void mancal()
        {
            rst = 0;
            mcl = 1;
            auto = 0;
            controlLincor(rst, mcl, auto, wait_time);
        }

        public static void automode()
        {
            rst = 0;
            mcl = 0;
            auto = 1;
            controlLincor(rst, mcl, auto, wait_time);
        }

        public static int[] Info;
        public static void getinfo()
        {
            Info = new int[16];
            fixed (int* ptr = &(Info[0]))
                getInfo(ptr);
        }

        #endregion

    }
}
