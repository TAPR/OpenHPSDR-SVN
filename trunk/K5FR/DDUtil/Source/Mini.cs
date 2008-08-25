using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DataDecoder.Properties;
using Logger;


namespace DataDecoder
{
    public partial class Mini : Form
    {
        #region Vars & Init
//        Setup s = new Setup();
        Setup s;
        public bool init;
        Process process;
        Settings set = Settings.Default;
        public string PFfile = "";
        bool enableErrorLog = false;


        public Mini()
        {
            InitializeComponent();
            init = true;
        }

        #endregion Vars & Init

        #region Delegates

        // Write Peak reading to txt box
        delegate void SetPeakCallback(string text);
        private void SetPeak(string text)
        {
            if (this.txtFwd.InvokeRequired)
            {
                SetPeakCallback d = new SetPeakCallback(SetPeak);
                this.Invoke(d, new object[] { text });
            }
            else
                txtFwd.Text = text;
        }
        // Write Avg reading to txt box
        delegate void SetAvgCallback(string text);
        public void SetAvg(string text)
        {
            if (this.txtAvg.InvokeRequired)
            {
                SetAvgCallback d = new SetAvgCallback(SetAvg);
                this.Invoke(d, new object[] { text });
            }
            else
                txtAvg.Text = text;
        }
        // Write LP-100 SWR reading to txt box
        delegate void SetSwrCallback(string text);
        public void SetSwr(string text)
        {
            if (this.txtSWR.InvokeRequired)
            {
                SetSwrCallback d = new SetSwrCallback(SetSwr);
                this.Invoke(d, new object[] { text });
            }
            else
                txtSWR.Text = text;
        }

        #endregion Delegates

        #region Properties

        public bool StepFwd
        {
            get { return rbFwd.Checked; }
            set { rbFwd.Checked = value; }
        }
        public bool StepRev
        {
            get { return rb180.Checked; }
            set { rb180.Checked = value; }
        }
        public bool StepBI
        {
            get { return rbBiDir.Checked; }
            set { rbBiDir.Checked = value; }
        }
        public bool Step34
        {
            get { return rb34.Checked; }
            set { rb34.Checked = value; }
        }
        #endregion Properties

        #region Form Events

        private void btnCalib_Click(object sender, EventArgs e)
        {
            NotImp();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            NotImp();
        }

        private void btnProfiler_Click(object sender, EventArgs e)
        {
            NotImp();

            //if (PFfile != "" && PFfile != null)
            //{
            //    try
            //    {
            //        s.sp.Close();
            //        process = Process.Start(PFfile);
            //        this.Text = "Starting Profiler";
            //    }
            //    catch (Exception ex)
            //    {
            //        bool bReturnLog = false;
            //        bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
            //        if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            //    }
            //}
            //else
            //    MessageBox.Show("No location has been selected for the FlexProfiler.exe file.\n\n" +
            //        "Please select a file location on the 'Other' tab and try again.", "File Error",
            //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnReStart_Click(object sender, EventArgs e)
        {
            NotImp();
            
            //try
            //{
            //    if (IsFPRunning())
            //    {
            //        process.Kill();
            //        process.WaitForExit();
            //    }
            //    if (!s.sp.isOpen)
            //    {
            //        s.sp.Open();
            //        this.Text = "DDUtil Re-Starting";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    bool bReturnLog = false;
            //    bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
            //    if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            //}
        }
        
        public void btnSP_Click(object sender, EventArgs e)
        {
            s.TurnRotor(txtSP.Text);            
        }
        
        private void Mini_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Settings set = Settings.Default;
            set.miniGeo = DataDecoder.Setup.GeometryToString(this);
            e.Cancel = true;
            this.Hide();
        }
        
        private void Mini_Load(object sender, EventArgs e)
        {
  //          s = new Setup();
            AOT.Checked = set.MiniTopMost;
            DataDecoder.Setup.GeometryFromString(set.miniGeo, this);
        }

        private void AOT_CheckedChanged(object sender, EventArgs e)
        {
            if (AOT.Checked)
            { this.TopMost = true; set.MiniTopMost = true; }
            else
            { this.TopMost = false; set.MiniTopMost = false; }
            set.Save();
        }
        #endregion Form Events

        #region Methods

        // send not implemented message
        private void NotImp()
        {
            Notification.notiMsg =
                "\r\r\rThis function not yet implemented";
            Notification alert = new Notification();
            alert.Show();
        }
        // See if Flex Profiler is running
        public static bool IsFPRunning()
        {
            bool IsRunning = false;
            string proc = "FlexProfiler";
            Process[] processes = Process.GetProcessesByName(proc);
            if (processes.Length > 0)
            {
                IsRunning = true;
            }
            else
            {
                IsRunning = false;
            }
            return IsRunning;
        }
        
        #endregion Methods



    }// end Mini
}