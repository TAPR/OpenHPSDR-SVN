using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DataDecoder.Properties;


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
            // Restore window size and position 
            // this is the default
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.WindowsDefaultBounds;

            // check if the saved bounds are nonzero and visible on any screen
            if (Settings.Default.WindowPositionMini != Rectangle.Empty &&
                IsVisibleOnAnyScreen(Settings.Default.WindowPositionMini))
            {
                // first set the bounds
                this.StartPosition = FormStartPosition.Manual;
                this.DesktopBounds = Settings.Default.WindowPositionMini;

                // afterwards set the window state to the saved value (which could be Maximized)
                this.WindowState = Settings.Default.WindowStateMini;
            }
            else
            {
                // this resets the upper left corner of the window to windows standards
                this.StartPosition = FormStartPosition.WindowsDefaultLocation;

                // we can still apply the saved size
                // msorens: added gatekeeper, otherwise first time appears as just a title bar!
                if (Settings.Default.WindowPositionMini != Rectangle.Empty)
                {
                    this.Size = Settings.Default.WindowPositionMini.Size;
                }
            }
            windowInitialized = true;
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
        // Write SWR reading to txt box
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

        // the antenna value has changed
        private void txtTune_TextChanged(object sender, EventArgs e)
        {
            if (s.Amp == 0 && s.bAnt)
            {
                string ant = txtTune.Text;
                if (s.AlphaPort.IsOpen) s.AlphaPort.Write("AqW3" + ant + "\0");
                for (int k = 0; k < 5; k++)
                {
                    Thread.Sleep(100);
                    Application.DoEvents();
                }
                set.AlphaAnt = Convert.ToInt32(txtTune.Text);
                set.Save();
            }
        }
        // The ant has changed send a command to the amp
        private void txtTune_DoubleClick(object sender, EventArgs e)
        {
            if (s.Amp == 0 && s.bAnt)
            {
                int ant = Convert.ToInt32(txtTune.Text);
                if (ant < 9) ant += 1;
                else ant = 0;
                s.SetpaTune(ant.ToString());
            }
        }
        // The Oper/Stby button was pressed
        private void btnByp_Click(object sender, EventArgs e)
        {
            s.btnByp_Click(null, null);
        }
        private void btnFull_Click(object sender, EventArgs e)
        {
            if (s.WindowState == FormWindowState.Normal)
                s.WindowState = FormWindowState.Minimized;
            else if (s.WindowState == FormWindowState.Minimized)
                s.WindowState = FormWindowState.Normal;
        }

        private void chkRCP2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRCP2.Checked) s.chkRCP2.Checked = true;
            else s.chkRCP2.Checked = false;
        }

        private void chkRCP3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRCP3.Checked) s.chkRCP3.Checked = true;
            else s.chkRCP3.Checked = false;
        }

        private void chkRCP4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRCP4.Checked) s.chkRCP4.Checked = true;
            else s.chkRCP4.Checked = false;
        }

        private void Mini_DoubleClick(object sender, EventArgs e)
        {
            Size size = new Size();
            if (this.Size.Height <= 120) size.Height = 186;//this.Size.Height * 2;
            else if (this.Size.Height > 120) size.Height = 120;// this.Size.Height / 2;
            this.Size = new Size(this.Size.Width, size.Height);
        }

        private void Mini_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 && e.Modifiers == Keys.Shift)
            { s.ProcessMacroButton(13); }
            else if (e.KeyCode == Keys.F2 && e.Modifiers == Keys.Shift)
            { s.ProcessMacroButton(14); }
            else if (e.KeyCode == Keys.F3 && e.Modifiers == Keys.Shift)
            { s.ProcessMacroButton(15); }
            else if (e.KeyCode == Keys.F4 && e.Modifiers == Keys.Shift)
            { s.ProcessMacroButton(16); }
            else if (e.KeyCode == Keys.F1)
            { s.ProcessMacroButton(1); }
            else if (e.KeyCode == Keys.F2)
            { s.ProcessMacroButton(2); }
            else if (e.KeyCode == Keys.F3)
            { s.ProcessMacroButton(3); }
            else if (e.KeyCode == Keys.F4)
            { s.ProcessMacroButton(4); }
            else if (e.KeyCode == Keys.F5)
            { s.ProcessMacroButton(5); }
            else if (e.KeyCode == Keys.F6)
            { s.ProcessMacroButton(6); }
            else if (e.KeyCode == Keys.F7)
            { s.ProcessMacroButton(7); }
            else if (e.KeyCode == Keys.F8)
            { s.ProcessMacroButton(8); }
            else if (e.KeyCode == Keys.F9)
            { s.ProcessMacroButton(9); }
            else if (e.KeyCode == Keys.F10)
            { s.ProcessMacroButton(10); }
            else if (e.KeyCode == Keys.F11)
            { s.ProcessMacroButton(11); }
            else if (e.KeyCode == Keys.F12)
            { s.ProcessMacroButton(12); }
            else if (e.Control && e.KeyCode == Keys.Oemtilde) // Set Split
            { s.btnSplit_Click(null, null); }
            else if (e.Control && e.KeyCode == Keys.A) // Saves Auto Drive setting
            { s.btnDrive_Click(null, null); }
            else if (e.Control && e.KeyCode == Keys.B) // SteppIR to Bi Direction
            { s.rbBiDir.Checked = true; }
            else if (e.Control && e.KeyCode == Keys.F) // SteppIR to Forward
            { s.rbFwd.Checked = true; }
            else if (e.Control && e.KeyCode == Keys.R) // SteppIR to Reverse
            { s.rb180.Checked = true; }
            else if (e.Control && e.KeyCode == Keys.L) // Set drive to low power
            { s.LowPower(); }
            else if (e.Control && e.KeyCode == Keys.O) // Toggle PTT
            { s.btnByp_Click(null, null); }
            else if (e.Control && e.KeyCode == Keys.Z) // Toggle VFO/Memory
            { s.btnMV_Click(null, null); }  
            else if (e.Control && e.KeyCode == Keys.X) // Memory select (1-5)
            { s.btnReCall_Click(null, null); }
            else if (e.Control && e.KeyCode == Keys.C) // Memory Load
            { s.btnMemLoad_Click(null, null); }
            else if (e.Control && e.KeyCode == Keys.V) // Memory Save
            { s.btnMemSave_Click(null, null); }
            else if (e.Control && e.KeyCode == Keys.N) // Open Memory Note window
            { s.txtMemFreq_DoubleClick(null, null); }
        }

        private void btnMacro1_Click(object sender, EventArgs e)
        {
            s.btnMacro1_Click(null, null);
        }

        private void btnMacro2_Click(object sender, EventArgs e)
        {
            s.btnMacro2_Click(null, null);
        }

        private void btnMacro3_Click(object sender, EventArgs e)
        {
            s.btnMacro3_Click(null, null);
        }

        private void btnMacro4_Click(object sender, EventArgs e)
        {
            s.btnMacro4_Click(null, null);
        }

        private void btnMacro5_Click(object sender, EventArgs e)
        {
            s.btnMacro5_Click(null, null);
        }

        private void btnMacro6_Click(object sender, EventArgs e)
        {
            s.btnMacro6_Click(null, null);
        }

        private void btnMacro7_Click(object sender, EventArgs e)
        {
            s.btnMacro7_Click(null, null);
        }

        private void btnMacro8_Click(object sender, EventArgs e)
        {
            s.btnMacro8_Click(null, null);
        }

        private void btnMacro9_Click(object sender, EventArgs e)
        {
            s.btnMacro9_Click(null, null);
        }

        private void btnMacro10_Click(object sender, EventArgs e)
        {
            s.btnMacro10_Click(null, null);
        }

        private void btnMacro11_Click(object sender, EventArgs e)
        {
            s.btnMacro11_Click(null, null);
        }

        private void btnMacro12_Click(object sender, EventArgs e)
        {
            s.btnMacro12_Click(null, null);
        }

        private void btnMacro13_Click(object sender, EventArgs e)
        {
            s.btnMacro13_Click(null, null);
        }

        private void btnMacro14_Click(object sender, EventArgs e)
        {
            s.btnMacro14_Click(null, null);
        }

        private void btnMacro15_Click(object sender, EventArgs e)
        {
            s.btnMacro15_Click(null, null);
        }

        private void btnMacro16_Click(object sender, EventArgs e)
        {
            s.btnMacro16_Click(null, null);
        }

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
 //           set.miniGeo = DataDecoder.Setup.GeometryToString(this);
            e.Cancel = true;
            this.Hide();
        }
        private void Mini_FormClosing(object sender, FormClosedEventArgs e)
        {
     //       this.Close();
        }
        private void Mini_FormClosing(object sender, FormClosingEventArgs e)
        {
            s.WindowState = FormWindowState.Normal;
        }
        // form load
        private void Mini_Load(object sender, EventArgs e)
        {
            AOT.Checked = set.MiniTopMost;
            this.toolTip1.SetToolTip(this.btnMacro1, s.btnMacro1.Text);
            this.toolTip1.SetToolTip(this.btnMacro2, s.btnMacro2.Text);
            this.toolTip1.SetToolTip(this.btnMacro3, s.btnMacro3.Text);
            this.toolTip1.SetToolTip(this.btnMacro4, s.btnMacro4.Text);
            this.toolTip1.SetToolTip(this.btnMacro5, s.btnMacro5.Text);
            this.toolTip1.SetToolTip(this.btnMacro6, s.btnMacro6.Text);
            this.toolTip1.SetToolTip(this.btnMacro7, s.btnMacro7.Text);
            this.toolTip1.SetToolTip(this.btnMacro8, s.btnMacro8.Text);
            this.toolTip1.SetToolTip(this.btnMacro9, s.btnMacro9.Text);
            this.toolTip1.SetToolTip(this.btnMacro10, s.btnMacro10.Text);
            this.toolTip1.SetToolTip(this.btnMacro11, s.btnMacro11.Text);
            this.toolTip1.SetToolTip(this.btnMacro12, s.btnMacro12.Text);
            this.toolTip1.SetToolTip(this.btnMacro13, s.btnMacro13.Text);
            this.toolTip1.SetToolTip(this.btnMacro14, s.btnMacro14.Text);
            this.toolTip1.SetToolTip(this.btnMacro15, s.btnMacro15.Text);
            this.toolTip1.SetToolTip(this.btnMacro16, s.btnMacro16.Text);
            //            DataDecoder.Setup.GeometryFromString(set.miniGeo, this);
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
        //the alpha power button was pressed
        private void btnPwr_Click(object sender, EventArgs e)
        {
            s.btnPwr_Click(null, null);
        }
        // The Operate / Stand By button was pressed
        private void btnOper_Click(object sender, EventArgs e)
        {
            s.btnOper_Click(null, null);
        }
        // The Auto tune button was pressed.
        private void btnTune_Click(object sender, EventArgs e)
        {
            s.btnTune_Click(null, null);
        }
        // The High Voltage button was pressed
        private void btnHV_Click(object sender, EventArgs e)
        {
            s.btnHV_Click(null, null);
        }
        // The Soft Fault button has been pressed
        private void btnSF_Click(object sender, EventArgs e)
        {
            s.btnSF_Click(null, null);
        }
        // The Hard Fault button has been pressed
        private void btnHF_Click(object sender, EventArgs e)
        {
            s.btnHF_Click(null, null);
        }
        // The SF button label was double clicked
        private void lblSF_DoubleClick(object sender, EventArgs e)
        {
            s.lblSF_DoubleClick(null, null);
        }
        // The HF button label was double clicked
        private void lblHF_DoubleClick(object sender, EventArgs e)
        {
            s.lblHF_DoubleClick(null, null);
        }
        // the rotor window was double-clicked
        private void txtSP_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            s.ToggleRtrEnab();
        }
        // the mem vfo exchange button was pressed
        private void btnMV_Click(object sender, EventArgs e)
        {
            s.btnMV_Click(null, null);
        }
        // the memory recall button was pressed
        private void btnReCall_Click(object sender, EventArgs e)
        {
            s.btnReCall_Click(null, null);
        }
        // the memory save button was pressed
        private void btnMemSave_Click(object sender, EventArgs e)
        {
            s.btnMemSave_Click(null, null);
        }
        // the memory load button was pressed
        private void btnMemLoad_Click(object sender, EventArgs e)
        {
            s.btnMemLoad_Click(null, null);
        }
        // the memory clear button was pressed
        private void btnMemClear_Click(object sender, EventArgs e)
        {
            s.btnMemClear_Click(null, null);
        }
        // the Control + Clear button was pressed
        private void btnMemClear_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift)
            {
                for (int i = 0; i <= 4; i++)
                {
                    for (int j = 0; j <= 10; j++)
                    {
                        s.mem[i, j] = null;
                    }
                }
                s.idxMem = 0;
                s.SetMemIdx((s.idxMem + 1).ToString());
                s.SetMemFreq("Empty"); s.SetMemMode("");
            }
        }
        // the mem freq window was double-clicked
        private void txtMemFreq_DoubleClick(object sender, EventArgs e)
        {
            s.txtMemFreq_DoubleClick(null, null);
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
        //// See if Flex Profiler is running
        //public static bool IsFPRunning()
        //{
        //    bool IsRunning = false;
        //    string proc = "FlexProfiler";
        //    Process[] processes = Process.GetProcessesByName(proc);
        //    if (processes.Length > 0)
        //    {
        //        IsRunning = true;
        //    }
        //    else
        //    {
        //        IsRunning = false;
        //    }
        //    return IsRunning;
        //}
        
        #endregion Methods

        # region Window Geometry New

        private bool IsVisibleOnAnyScreen(Rectangle rect)
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(rect))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // only save the WindowState if Normal or Maximized
            switch (this.WindowState)
            {
                case FormWindowState.Normal:
                case FormWindowState.Maximized:
                    Settings.Default.WindowStateMini = this.WindowState;
                    break;

                default:
                    Settings.Default.WindowStateMini = FormWindowState.Normal;
                    break;
            }

            Settings.Default.Save();
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            TrackWindowState();
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            TrackWindowState();
        }
        bool windowInitialized;
        // On a move or resize in Normal state, record the new values as they occur.
        // This solves the problem of closing the app when minimized or maximized.
        private void TrackWindowState()
        {
            // Don't record the window setup, otherwise we lose the persistent values!
            if (!windowInitialized) { return; }

            if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.WindowPositionMini = this.DesktopBounds;
            }
        }

        # endregion Window Geometry New



    }// end Mini

}