using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using DataDecoder.Properties;
//using Logger;


namespace DataDecoder
{
    public partial class Mini : Form
    {
        #region Vars & Init
        Setup s;
        public bool init;
        Settings set = Settings.Default;
        public string PFfile = "";

        public Mini(Setup setup)
        {
            InitializeComponent();
            s = setup;
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
        // SteppIR Calibrate button was pressed
        private void btnCalib_Click(object sender, EventArgs e)
        {
            s.btnCalib_Click(null, null);
        }
        // SteppIR Home button was pressed
        private void btnHome_Click(object sender, EventArgs e)
        {
            s.btnHome_Click(null, null);
        }
        // Profiler button was pressed
        private void btnProfiler_Click(object sender, EventArgs e)
        {
            s.btnProfiler_Click(null, null);
        }
        // Profiler Re start button was pressed
        private void btnReStart_Click(object sender, EventArgs e)
        {
            s.btnReStart_Click(null, null);
        }
        // Rotor button was pressed
        public void btnSP_Click(object sender, EventArgs e)
        {
            txtSP.Focus(); 
            s.lblSP.Text = txtSP.Text;
            s.TurnRotor(txtSP.Text);            
        }
        // Stop the rotor if moving (Ctrl+SP button)
        private void btnSP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            { txtSP.Focus(); s.RotorStop(); e.Handled = true; }
        }
        // form is closing
        private void Mini_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            set.miniGeo = DataDecoder.Setup.GeometryToString(this);
            e.Cancel = true;
            this.Hide();
        }
        // form load
        private void Mini_Load(object sender, EventArgs e)
        {
            AOT.Checked = set.MiniTopMost;
            DataDecoder.Setup.GeometryFromString(set.miniGeo, this);
        }
        // the always on top chk box has changed
        private void AOT_CheckedChanged(object sender, EventArgs e)
        {
            if (AOT.Checked)
            { this.TopMost = true; set.MiniTopMost = true; }
            else
            { this.TopMost = false; set.MiniTopMost = false; }
            set.Save();
        }
        // one of the SteppIR buttons has changed
        private void grpStepCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if (init)
            {
                if (sender == rbFwd) s.StepFwd = true;
                else if (sender == rb180) s.StepRev = true;
                else if (sender == rbBiDir) s.StepBI = true;
                else if (sender == rb34) s.Step34 = true;
            }
            init = true;
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