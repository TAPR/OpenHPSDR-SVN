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
    public unsafe partial class PSForm : Form
    {
        #region constructor

        private Console console;

        public PSForm(Console c)
        {
            InitializeComponent();
            Common.RestoreForm(this, "PureSignal", false);
            console = c;
        }

        #endregion

        #region variables

        private int gcolor = (0xFF << 24) | (0xFF << 8);
        private static bool autoON = false;
        private static bool singlecalON = false;
        private int oldCalCount = 0;
        private int alpha, red, green, blue;
        private int splashcount = 0;
        private static double PShwpeak;
        private static double GetPSpeakval;

        public static AmpView ampv = null;
        public static Thread ampvThread = null;

        #endregion

        #region properties

        private bool dismissAmpv = false;
        public bool DismissAmpv
        {
            get { return dismissAmpv; }
            set
            {
                dismissAmpv = value;
            }
        }

        private static bool psenabled = false;
        public bool PSEnabled
        {
            get { return psenabled; }
            set
            {
                psenabled = value;
                if (!psenabled) ResetPureSignal();
                fixFreqs();
                ChangeDispRcvr();
            }
        }

        private static bool ttgenON = false;
        public bool TTgenON
        {
            get { return ttgenON; }
            set
            {
                ttgenON = value;
                if (ttgenON)
                    btnPSTwoToneGen.BackColor = Color.FromArgb(gcolor);
                else
                    btnPSTwoToneGen.BackColor = SystemColors.Control;
            }
        }

        private static int txachannel = 4;
        public int TXAchannel
        {
            get { return txachannel; }
            set { txachannel = value; }
        }

        private static int rxRCVR = 1;
        public int RXrcvr
        {
            get { return rxRCVR; }
            set { rxRCVR = value; }
        }

        private static int txRCVR = 2;
        public int TXrcvr
        {
            get { return txRCVR; }
            set { txRCVR = value; }
        }

        // receiver to display on bottom panadapter for debug
        private static int oldDispRcvr = 2;
        private static int dispRCVR = 2;
        public int DISPrcvr
        {
            get { return dispRCVR; }
            set { dispRCVR = value; }
        }

        static double[] rxfreqs = new double[16];
        static double txfreq = 0.0;

        public double RX1freq
        {
            get { return rxfreqs[1]; }
            set
            {
                rxfreqs[1] = value;
                if ((rxRCVR == 1) || (txRCVR == 1))
                    fixFreqs();
                else
                    SetRXFreq(1, rxfreqs[1], true);
            }
        }

        public double RX2freq
        {
            get { return rxfreqs[2]; }
            set
            {
                rxfreqs[2] = value;
                if ((rxRCVR == 2) || (txRCVR == 2))
                    fixFreqs();
                else
                    SetRXFreq(2, rxfreqs[2], true);
            }
        }

        public double RX3freq
        {
            get { return rxfreqs[3]; }
            set
            {
                rxfreqs[3] = value;
                if ((rxRCVR == 3) || (txRCVR == 3))
                    fixFreqs();
                else
                    SetRXFreq(3, rxfreqs[3], true);
            }
        }

        public double RX4freq
        {
            get { return rxfreqs[4]; }
            set
            {
                rxfreqs[4] = value;
                if ((rxRCVR == 4) || (txRCVR == 4))
                    fixFreqs();
                else
                    SetRXFreq(4, rxfreqs[4], true);
            }
        }

        public double RX5freq
        {
            get
            { return rxfreqs[5]; }
            set
            {
                rxfreqs[5] = value;
                if ((rxRCVR == 5) || (txRCVR == 5))
                    fixFreqs();
                else
                    SetRXFreq(5, rxfreqs[5], true);
            }
        }

        public double TXfreq
        {
            get { return txfreq; }
            set
            {
                txfreq = value;
                JanusAudio.SetVFOfreqTX(txfreq);
                fixFreqs();
            }
        }

        private static bool mox = false;
        public bool Mox
        {
            get { return mox; }
            set
            {
                mox = value;
                fixFreqs();
                ChangeDispRcvr();
            }
        }

        #endregion

        #region event handlers

        private void PSForm_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;     // needed to use KeyDown event
            if (autoON == true)
                btnPSAutoCalibrate.BackColor = Color.FromArgb(gcolor);
            if (ttgenON == true)
                btnPSTwoToneGen.BackColor = Color.FromArgb(gcolor);
            btnPSTwoToneGen.Enabled = !mox;
            fixed (double* ptr = &PShwpeak)
                puresignal.GetPSHWPeak(txachannel, ptr);
            PSpeak.Text = PShwpeak.ToString();
            PSdispRX.Text = dispRCVR.ToString();
        }

        private void PSForm_Closing(object sender, FormClosingEventArgs e)
        {
            if (ampv != null)
            {
                dismissAmpv = true;
                ampvThread.Join();
                ampv.Close();
                ampv = null;
            }
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "PureSignal");
        }

        public void RunAmpv()
        {
            ampv = new AmpView(this);
            Application.Run(ampv);
        }

        private void btnPSAmpView_Click(object sender, EventArgs e)
        {
            if (ampv == null || (ampv != null && ampv.IsDisposed))
            {
                dismissAmpv = false;
                ampvThread = new Thread(RunAmpv);
                ampvThread.SetApartmentState(ApartmentState.STA);
                ampvThread.Name = "Ampv Thread";
                ampvThread.Start();
            }
        }

        private void btnPSInformation_Click(object sender, EventArgs e)
        {
            string CurrentAppPath = Environment.CurrentDirectory;
            System.Diagnostics.Process.Start(CurrentAppPath + "\\PureSignal.pdf");
        }

        private void btnPSAutoCalibrate_Click(object sender, EventArgs e)
        {
            if (autoON == false)
            {
                btnPSAutoCalibrate.BackColor = Color.FromArgb(gcolor);
                autoON = true;
                singlecalON = false;
                puresignal.SetPSControl(txachannel, 0, 0, 1, 0);
            }
            else
            {
                puresignal.SetPSControl(txachannel, 1, 0, 0, 0);
                btnPSAutoCalibrate.BackColor = SystemColors.Control;
                autoON = false;
            }
        }

        private void btnPSCalibrate_Click(object sender, EventArgs e)
        {
            btnPSAutoCalibrate.BackColor = SystemColors.Control;
            singlecalON = true;
            autoON = false;
            puresignal.SetPSControl(txachannel, 0, 1, 0, 0);
        }

        private void btnPSReset_Click(object sender, EventArgs e)
        {
            ResetPureSignal();
        }

        private void udPSMoxDelay_ValueChanged(object sender, EventArgs e)
        {
            puresignal.SetPSMoxDelay(txachannel, (double)udPSMoxDelay.Value);
        }

        private void udPSCalWait_ValueChanged(object sender, EventArgs e)
        {
            puresignal.SetPSLoopDelay(txachannel, (double)udPSCalWait.Value);
        }

        private void udPSPhnum_ValueChanged(object sender, EventArgs e)
        {
            double actual_delay = puresignal.SetPSTXDelay(txachannel, (double)udPSPhnum.Value * 1.0e-09);
        }

        private void btnPSTwoToneGen_Click(object sender, EventArgs e)
        {
            if (ttgenON == false)
            {
                btnPSTwoToneGen.BackColor = Color.FromArgb(gcolor);
                ttgenON = true;
                console.SetupForm.TTgenrun = true;
            }
            else
            {
                btnPSTwoToneGen.BackColor = SystemColors.Control;
                ttgenON = false;
                console.SetupForm.TTgenrun = false;
            }
        }

        private void btnPSSave_Click(object sender, EventArgs e)
        {
            System.IO.Directory.CreateDirectory(console.AppDataPath + "\\PureSignal\\");
            SaveFileDialog savefile1 = new SaveFileDialog();
            savefile1.InitialDirectory = console.AppDataPath + "PureSignal\\";
            savefile1.RestoreDirectory = true;
            if (savefile1.ShowDialog() == DialogResult.OK)
                puresignal.PSSaveCorr(txachannel, savefile1.FileName);
        }

        private void btnPSRestore_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile1 = new OpenFileDialog();
            openfile1.InitialDirectory = console.AppDataPath + "PureSignal\\";
            openfile1.RestoreDirectory = true;
            if (openfile1.ShowDialog() == DialogResult.OK)
            {
                puresignal.PSRestoreCorr(txachannel, openfile1.FileName);
                btnPSAutoCalibrate.BackColor = SystemColors.Control;
                singlecalON = false;
                autoON = false;
            }
        }

        private void splashtrans(int ct, int ct0, int ct1, int a0, int r0, int g0, int b0, int a1, int r1, int g1, int b1)
        {
            if ((ct0 <= ct) && (ct <= ct1))
            {
                int ict = ct - ct0;
                double splashfrac = (double)ict / (double)(ct1 - ct0 + 1);
                int ax = (int)(a0 + (a1 - a0) * splashfrac);
                int rx = (int)(r0 + (r1 - r0) * splashfrac);
                int gx = (int)(g0 + (g1 - g0) * splashfrac);
                int bx = (int)(b0 + (b1 - b0) * splashfrac);
                lblPSWelcome.ForeColor = Color.FromArgb((ax << 24) | (rx << 16) | (gx << 8) | bx);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            splashtrans(splashcount, 0, 10, 255, 255, 255, 255, 255, 255, 220, 0);
            splashtrans(splashcount, 10, 20, 255, 255, 220, 0, 255, 255, 20, 0);
            splashtrans(splashcount, 20, 120, 255, 255, 20, 0, 255, 0, 0, 0);
            if (splashcount > 120)
            {
                lblPSWelcome.Visible = false;
                lblPSWarning.Visible = true;
            }
            if (splashcount < 1000) splashcount++;
            puresignal.getinfo(txachannel);
            lblPSInfo0.Text = puresignal.Info[0].ToString();
            lblPSInfo1.Text = puresignal.Info[1].ToString();
            lblPSInfo2.Text = puresignal.Info[2].ToString();
            lblPSInfo3.Text = puresignal.Info[3].ToString();

            if (puresignal.Info[14] == 1)
            {
                lblPSInfoCO.BackColor = Color.FromArgb(gcolor);
                btnPSSave.Enabled = true;
                btnPSSave.BackColor = SystemColors.Control;
            }
            else
            {
                lblPSInfoCO.BackColor = Color.Black;
                btnPSSave.Enabled = false;
                btnPSSave.BackColor = SystemColors.ButtonShadow;
            }
            lblPSInfo15.Text = puresignal.Info[15].ToString();
            if ((alpha -= 5) < 0) alpha = 0;
            if (puresignal.Info[5] != oldCalCount)
            {
                oldCalCount = puresignal.Info[5];
                alpha = 255;
                if (puresignal.Info[4] > 181)
                {
                    red = 0; green = 0; blue = 255;
                }
                else if (puresignal.Info[4] > 128)
                {
                    red = 000; green = 255; blue = 000;
                }
                else if (puresignal.Info[4] > 90)
                {
                    red = 255; green = 255; blue = 000;
                }
                else
                {
                    red = 255; green = 000; blue = 000;
                }
            }
            lblPSInfoFB.BackColor = Color.FromArgb((alpha << 24) | (red << 16) | (green << 8) | blue);
            lblPSfb2.Text = puresignal.Info[4].ToString();

            if (!psenabled)
                lblDisabled.Visible = true;
            else
                lblDisabled.Visible = false;
            fixed (double* ptr = &GetPSpeakval)
                puresignal.GetPSMaxTX(txachannel, ptr);
            GetPSpeak.Text = GetPSpeakval.ToString();
        }

        private void PS_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control == true && e.Alt == true)
                switch (e.KeyCode)
                {
                    case Keys.I:
                        grpPSInfo.Visible = true;
                        break;
                    case Keys.N:
                        grpPSInfo.Visible = false;
                        break;
                }
        }

        private void PSpeak_TextChanged(object sender, EventArgs e)
        {
            PShwpeak = Convert.ToDouble(PSpeak.Text);
            puresignal.SetPSHWPeak(txachannel, PShwpeak);
        }

        private void PSdispRX_TextChanged(object sender, EventArgs e)
        {
            const int max_rcvrs = 5;
            int new_rcvr = dispRCVR;
            if (PSdispRX.Text != String.Empty)
            {
                new_rcvr = Convert.ToInt32(PSdispRX.Text);
                if (new_rcvr >= 0 && new_rcvr <= max_rcvrs)
                {
                    dispRCVR = new_rcvr;
                    oldDispRcvr = new_rcvr;
                }
                else
                    dispRCVR = -1;
                PSdispRX.Text = new_rcvr.ToString();
            }
        }

        private void chkPSRelaxPtol_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPSRelaxPtol.Checked)
                puresignal.SetPSPtol(txachannel, 0.400);
            else
                puresignal.SetPSPtol(txachannel, 0.900);
        }

        #endregion

        #region methods

        public void ForcePS()
        {
            EventArgs e = EventArgs.Empty;
            if (!autoON)
                puresignal.SetPSControl(txachannel, 1, 0, 0, 0);
            else
                puresignal.SetPSControl(txachannel, 0, 0, 1, 0);
            if (!ttgenON)
                wdsp.SetTXAPostGenRun(txachannel, 0);
            else
            {
                wdsp.SetTXAPostGenMode(txachannel, 1);
                wdsp.SetTXAPostGenRun(txachannel, 1);
            }
            udPSCalWait_ValueChanged(this, e);
            udPSPhnum_ValueChanged(this, e);
            udPSMoxDelay_ValueChanged(this, e);
            SetPSReceivers(console.CurrentHPSDRModel);
            chkPSRelaxPtol_CheckedChanged(this, e);
        }

        public void ResetPureSignal()
        {
            btnPSAutoCalibrate.BackColor = SystemColors.Control;
            singlecalON = false;
            autoON = false;
            fixFreqs();
            puresignal.SetPSControl(txachannel, 1, 0, 0, 0);
        }

        private void ChangeDispRcvr()
        {
            switch (console.CurrentHPSDRModel)
            {
                case HPSDRModel.ANAN10E:
                    if (psenabled && mox)
                    {
                        dispRCVR = -1;                     // comment this line to display  
                        // Display.BlankBottomDisplay = true; // comment this line to display
                    }
                    else
                    {
                        dispRCVR = oldDispRcvr;
                        // Display.BlankBottomDisplay = false;
                    }
                    break;
                default:
                    // Display.BlankBottomDisplay = false;
                    break;
            }
        }

        public void SetPSReceivers(HPSDRModel model)
        {
            switch (model)
            {
                case HPSDRModel.HERMES:
                    rxRCVR = 3;
                    txRCVR = 4;
                    break;
                case HPSDRModel.ANAN10:
                    rxRCVR = 3;
                    txRCVR = 4;
                    break;
                case HPSDRModel.ANAN10E:
                    rxRCVR = 1;
                    txRCVR = 2;
                    break;
                case HPSDRModel.ANAN100:
                case HPSDRModel.ANAN100B:
                    rxRCVR = 3;
                    txRCVR = 4;
                    break;
                case HPSDRModel.ANAN100D:
                    rxRCVR = 4;
                    txRCVR = 5;
                    break;
                case HPSDRModel.ORION:
                    rxRCVR = 4;
                    txRCVR = 5;
                    break;
                case HPSDRModel.HPSDR:
                    rxRCVR = 1;
                    txRCVR = 4;
                    break;
                default:
                    rxRCVR = 1;
                    txRCVR = 2;
                    break;
            }
        }

        public int NRX(int nr, HPSDRModel model)
        {
            int newnr;
            switch (model)
            {
                case HPSDRModel.HERMES:
                    newnr = Math.Max(4, nr);
                    break;
                case HPSDRModel.ANAN10:
                    newnr = Math.Max(4, nr);
                    break;
                case HPSDRModel.ANAN10E:
                    newnr = Math.Max(2, nr);
                    break;
                case HPSDRModel.ANAN100:
                case HPSDRModel.ANAN100B:
                    newnr = Math.Max(4, nr);
                    break;
                case HPSDRModel.ANAN100D:
                    newnr = Math.Max(5, nr);
                    break;
                case HPSDRModel.ORION:
                    newnr = Math.Max(5, nr);
                    break;
                case HPSDRModel.HPSDR:
                    newnr = Math.Max(4, nr);
                    break;
                default:
                    newnr = 2;
                    break;
            }
            return newnr;
        }

        /*private void fixFreqs()
        {
            if (psenabled)
            {
                SetRXFreq(rxRCVR, txfreq, false);
                SetRXFreq(txRCVR, txfreq, false);
            }
            else
            {
                SetRXFreq(rxRCVR, rxfreqs[rxRCVR], true);
                SetRXFreq(txRCVR, rxfreqs[txRCVR], true);
            }
        }*/

        private void fixFreqs()
        {
            if (psenabled)
            {
                switch (console.CurrentHPSDRModel)
                {
                    case HPSDRModel.HERMES:
                        SetRXFreq(rxRCVR, txfreq, false);
                        SetRXFreq(txRCVR, txfreq, false);
                        break;
                    case HPSDRModel.ANAN10:
                        SetRXFreq(rxRCVR, txfreq, false);
                        SetRXFreq(txRCVR, txfreq, false);
                        break;
                    case HPSDRModel.ANAN10E:
                        if (mox)
                        {
                            SetRXFreq(rxRCVR, txfreq, false);
                            SetRXFreq(txRCVR, txfreq, false);
                        }
                        else
                        {
                            SetRXFreq(rxRCVR, rxfreqs[rxRCVR], true);
                            SetRXFreq(txRCVR, rxfreqs[txRCVR], true);
                        }
                        break;
                    case HPSDRModel.ANAN100:
                    case HPSDRModel.ANAN100B:
                        SetRXFreq(rxRCVR, txfreq, false);
                        SetRXFreq(txRCVR, txfreq, false);
                        break;
                    case HPSDRModel.ANAN100D:
                        SetRXFreq(rxRCVR, txfreq, false);
                        SetRXFreq(txRCVR, txfreq, false);
                        break;
                    case HPSDRModel.ORION:
                        SetRXFreq(rxRCVR, txfreq, false);
                        SetRXFreq(txRCVR, txfreq, false);
                        break;
                    case HPSDRModel.HPSDR:
                        SetRXFreq(rxRCVR, txfreq, false);
                        SetRXFreq(txRCVR, txfreq, false);
                        break;
                    default:
                        SetRXFreq(rxRCVR, txfreq, false);
                        SetRXFreq(txRCVR, txfreq, false);
                        break;
                }
            }
            else
            {
                SetRXFreq(rxRCVR, rxfreqs[rxRCVR], true);
                SetRXFreq(txRCVR, rxfreqs[txRCVR], true);
            }
        }

        private void SetRXFreq(int rcvr, double freq, bool offset)
        {
            switch (rcvr)
            {
                case 1:
                    JanusAudio.SetVFOfreqRX1(freq, offset);
                    break;
                case 2:
                    JanusAudio.SetVFOfreqRX2(freq, offset);
                    break;
                case 3:
                    JanusAudio.SetVFOfreqRX3(freq, offset);
                    break;
                case 4:
                    JanusAudio.SetVFOfreqRX4(freq, offset);
                    break;
                case 5:
                    JanusAudio.SetVFOfreqRX5(freq, offset);
                    break;
            }
        }

        #endregion
    }

    unsafe static class puresignal
    {
        #region DllImport - Main

        [DllImport("wdsp.dll", EntryPoint = "GetPSInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetPSInfo(int channel, int* info);

        [DllImport("wdsp.dll", EntryPoint = "SetPSReset", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPSReset(int channel, int reset);

        [DllImport("wdsp.dll", EntryPoint = "SetPSMancal", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPSMancal(int channel, int mancal);

        [DllImport("wdsp.dll", EntryPoint = "SetPSAutomode", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPSAutomode(int channel, int automode);

        [DllImport("wdsp.dll", EntryPoint = "SetPSTurnon", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPSTurnon(int channel, int turnon);

        [DllImport("wdsp.dll", EntryPoint = "SetPSControl", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPSControl(int channel, int reset, int mancal, int automode, int turnon);

        [DllImport("wdsp.dll", EntryPoint = "SetPSLoopDelay", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPSLoopDelay(int channel, double delay);

        [DllImport("wdsp.dll", EntryPoint = "SetPSMoxDelay", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPSMoxDelay(int channel, double delay);

        [DllImport("wdsp.dll", EntryPoint = "SetPSTXDelay", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SetPSTXDelay(int channel, double delay);

        [DllImport("wdsp.dll", EntryPoint = "psccF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void psccF(int channel, int size, float* Itxbuff, float* Qtxbuff, float* Irxbuff, float* Qrxbuff, bool mox, bool solidmox);

        [DllImport("wdsp.dll", EntryPoint = "PSSaveCorr", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PSSaveCorr(int channel, string filename);

        [DllImport("wdsp.dll", EntryPoint = "PSRestoreCorr", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PSRestoreCorr(int channel, string filename);

        [DllImport("wdsp.dll", EntryPoint = "SetPSHWPeak", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPSHWPeak(int channel, double peak);

        [DllImport("wdsp.dll", EntryPoint = "GetPSHWPeak", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetPSHWPeak(int channel, double* peak);

        [DllImport("wdsp.dll", EntryPoint = "GetPSMaxTX", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetPSMaxTX(int channel, double* maxtx);

        [DllImport("wdsp.dll", EntryPoint = "SetPSPtol", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPSPtol(int channel, double ptol);

        [DllImport("wdsp.dll", EntryPoint = "GetPSDisp", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetPSDisp(int channel, IntPtr x, IntPtr ym, IntPtr yc, IntPtr ys, IntPtr cm, IntPtr cc, IntPtr cs);

        #endregion

        #region public methods

        public static int[] Info;
        public static void getinfo(int txachannel)
        {
            Info = new int[16];
            fixed (int* ptr = &(Info[0]))
                GetPSInfo(txachannel, ptr);
        }

        #endregion
    }
}
