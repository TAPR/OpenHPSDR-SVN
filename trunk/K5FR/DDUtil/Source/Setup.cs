//==
// Setup.cs
//==
// Copyright (C) 2007, 2008  Steve Nance - K5FR
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 3
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
// You may contact the author via email at: steve@k5fr.com
//==

using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.XPath;
using DataDecoder.Properties;
using Logger;
using TransDlg;

namespace DataDecoder
{
    public partial class Setup : Form
    {
        #region Enums

        public enum RotorMod
        { AlphaSpid, GreenHeron, Hygain, M2R2800PA, M2R2800PX, Prosistel, Yaesu };

        public enum SerialError
        { Frame, Overrun, RXOver, RXParity, TXFull };

        public enum PortMode
        { None, Kenwood, YaesuTypeI, YaesuTypeII, Icom };

        public enum Parity
        { Even, Mark, None, Odd, Space };

        public enum StopBits
        { None, One, OnePointFive, Two };

        public enum Ports
        { CAT, RCP1, RCP2, RCP3, RCP4 };

        private enum TempFormat
        { Celsius = 0, Fahrenheit, }

        #endregion Enums

        #region Variables

        ASCIIEncoding AE = new ASCIIEncoding();
        CATSerialPorts.CATSerialPort sp;
        DataSet ds = new DataSet();
        DataSet dsm = new DataSet();
        Hashtable flist = new Hashtable();
        Settings set = Settings.Default;
        PortMode portmode;
        RotorMod rotormod;
        Process process;
        private TempFormat temp_format = TempFormat.Celsius;
        private bool closing = false;
        bool logFlag = false;
        bool tempFlag = false;  // false = DDUtil, true = RCP1
        bool enableErrorLog = false;
        int keyValue = 0;
        int LPTnum = 0;         // decimal number of selected LPT port
        int iSleep = 0;         // Thread.Sleep var
        int StepCtr = 0;        // reps counter
        int reps = 7;           // how many times to test the SteppIR port
        double pollInt = 0;     // CAT port interval timer uses txtInv text box
        double temp = 26.4;
        string fileName = Application.StartupPath + "\\BandData.xml";
        string MacFileName = Application.StartupPath + "\\MacroData.xml";
        string LastFreq = "";
        string LastMode = "";
        string[] ports;
        string OutBuffer = "";
        public static string ver = "1.5.8";
        public static int errCtr = 0;
        public static int x;    // screen pos left
        public static int y;    // screen pos right
        string vfo = "";
        System.Timers.Timer pollTimer;
        System.Timers.Timer logTimer;
        System.Timers.Timer lpTimer;
        System.Timers.Timer tempTimer;
        System.Timers.Timer StepTimer;
        #endregion Variables

        #region Properties

        public bool RotorEnab
        {
            get { return chkRotorEnab.Checked; }
            set { chkRotorEnab.Checked = value; }
        }
        public bool StepEnab
        {
            get { return chkStep.Checked; }
            set { chkStep.Checked = value; }
        }
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
        public bool DisableToolTips
        {
            set { toolTip1.Active = value; }
        }
        /// <summary>
        /// The default radio serial port
        /// </summary>
        private string default_ComRadio = "";
        public string DefaultComRadio
        {
            get { return default_ComRadio; }
            set { default_ComRadio = value; }
        }
        /// <summary>
        /// The default accessory serial port
        /// </summary>
        private string default_ComAcc = "";
        public string DefaultComAcc
        {
            get { return default_ComAcc; }
            set { default_ComAcc = value; }
        }
        /// <summary>
        /// The default Logger (RCP1) port
        /// </summary>
        private string default_LogPort = "";
        public string DefaultLogPort
        {
            get { return default_LogPort; }
            set { default_LogPort = value; }
        }
        /// <summary>
        /// The default LP100 port
        /// </summary>
        private string default_LPport = "";
        public string DefaultLPport
        {
            get { return default_LPport; }
            set { default_LPport = value; }
        }
        /// <summary>
        /// The default SteppIR port
        /// </summary>
        private string default_StepPort = "";
        public string DefaultStepPort
        {
            get { return default_StepPort; }
            set { default_StepPort = value; }
        }
        /// <summary>
        /// The default RCP2 port
        /// </summary>
        private string default_rcp2 = "";
        public string DefaultRCP2port
        {
            get { return default_rcp2; }
            set { default_rcp2 = value; }
        }
        /// <summary>
        /// The default RCP3 port
        /// </summary>
        private string default_RCP3 = "";
        public string DefaultRCP3port
        {
            get { return default_RCP3; }
            set { default_RCP3 = value; }
        }
        /// <summary>
        /// The default RCP4 port
        /// </summary>
        private string default_RCP4 = "";
        public string DefaultRCP4port
        {
            get { return default_RCP4; }
            set { default_RCP4 = value; }
        }
        /// <summary>
        /// The default PW1 port
        /// </summary>
        private string default_PW1 = "";
        public string DefaultPW1port
        {
            get { return default_PW1; }
            set { default_PW1 = value; }
        }
        /// <summary>
        /// The default parallel port
        /// </summary>
        private string default_lpt = "";
        public string DefaultLPT
        {
            get { return default_lpt; }
            set { default_lpt = value; }
        }
        #endregion Properties

        #region Initialization

        public Setup()
        {
            if (IsAppAlreadyRunning())  // if the app is already don't start another one.
            {
                Environment.Exit(0);
                Console.Beep();
            }
            InitializeComponent();
            GeometryFromString(set.WindowGeometry, this);
            chkDevice.Checked = set.DevEnab;
            chkDev0.Checked = set.Dev0Enab;
            chkRCP2DisPol.Checked = set.RCP2DisPol;
            chkRCP3DisPol.Checked = set.RCP3DisPol;
            chkRCP4DisPol.Checked = set.RCP4DisPol;
            chkRCP2IF.Checked = set.RCP2IF;
            chkRCP3IF.Checked = set.RCP3IF;
            chkRCP4IF.Checked = set.RCP4IF;
            txtRCP.Text = set.RCPtext;
            txtSleep.Text = set.Sleep;
            iSleep = Convert.ToInt32(set.Sleep);
            rbFwd.Checked = true;
            chkTips.Checked = set.ToolTips;
            chkMode.Checked = set.slaveMode;
            txtCall.Text = set.Call;
            PortAccess.Output(LPTnum, 0);
            // setup error log parameters
            ErrorLog.LogFilePath = "ErrorLog.txt";
            enableErrorLog = set.ErrorLog;
            if (enableErrorLog)
                enableErrorLoggingToolStripMenuItem.Checked = true;
            else
                enableErrorLoggingToolStripMenuItem.Checked = false;

            //setup radio port timer for a 1 second interrupt
            pollTimer = new System.Timers.Timer();
            pollTimer.Elapsed += new System.Timers.ElapsedEventHandler(pollTimer_Elapsed);

            // set port interval timer to saved value, if 0 default to 1000
            string str = "";
            str = set.portInv;
            txtInv.Text = str;
            pollInt = Convert.ToDouble(set.portInv);
            if (pollInt == 0) pollInt = 1000.0;
            pollTimer.Interval = pollInt;  // 1000 = 1 second
            pollTimer.Enabled = false;

            // setup Log Port Timer for a 10 second interrupt
            logTimer = new System.Timers.Timer();
            logTimer.Elapsed += new System.Timers.ElapsedEventHandler(logTimer_Elapsed);
            logTimer.Interval = 5000;      // 1000 = 1 seconds
            logTimer.Enabled = false;

            // setup LP100 Port Timer for a 200 ms interrupt
            lpTimer = new System.Timers.Timer();
            lpTimer.Elapsed += new System.Timers.ElapsedEventHandler(lpTimer_Elapsed);
            lpTimer.Interval = 200;
            lpTimer.Enabled = false;

            // setup PA Temp Timer for a 1200 ms interrupt
            tempTimer = new System.Timers.Timer();
            tempTimer.Elapsed += new System.Timers.ElapsedEventHandler(tempTimer_Elapsed);
            if (pollInt >= 1000)
                tempTimer.Interval = Convert.ToDouble(txtInv.Text) * 1.05;
            else if (pollInt < 1000)
                tempTimer.Interval = 1000;
            tempTimer.Enabled = false;

            /* setup pw1 test timer; to be removed after testing complete */
            pw1Timer = new System.Timers.Timer();
            pw1Timer.Elapsed += new System.Timers.ElapsedEventHandler(pw1Timer_Elapsed);
            pw1Timer.Interval = 2500;  // 1000 = 1 second
            pw1Timer.Enabled = false;

            // setup SteppIR Data Timer
            StepTimer = new System.Timers.Timer();
            StepTimer.Elapsed += new System.Timers.ElapsedEventHandler(StepTimer_Elapsed);
            StepTimer.Interval = Convert.ToDouble(set.StepInv);
            StepTimer.Enabled = false;

            CreateSerialPort();
            GetPortNames();             // enumerate serial ports and load combo boxes
            InitRotor();
            str = set.CIVaddr;
            txtRadNum.Text = str;
            cboRadData.SelectedIndex = set.RadData;
            // set Radio Serial port to the last port used
            str = set.RadioPort;
            cboCAT.Text = str;
            DefaultComRadio = str;
            SetDefaultComRadio();
            txtInv.Text = Convert.ToString(pollInt);
            txtLPint.Text = Convert.ToString(set.LPint);
            txtProfLoc.Text = set.ProfLoc;

            // set Acc Serial (passive listener) port to the last port used
            str = set.AccPort;
            if (str != "")
            {
                cboSerAcc.Text = str;
                cboRadio.SelectedIndex = set.followRadio;
                cboFollow.Checked = set.followChk;
                if (cboFollow.Checked == false)
                { cboRadio.Enabled = false; chkMode.Enabled = false; }
                else
                { cboRadio.Enabled = true; chkMode.Enabled = true; }
                if (AccPort.IsOpen) AccPort.Close();
                try
                {   // try to open the selected AccPort:
                    AccPort.PortName = str;
                    DefaultComAcc = str;
                    SetDefaultComAcc();
                }
                catch
                {
                    // give a message, if the port is not available:
                    MessageBox.Show("The Passive Listener serial port " + AccPort.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboSerAcc.SelectedText = "";
                }
            }
            // set Logger Serial (radio control) port to the last port used
            str = set.LogPort;
            if (str != "")
            {
                cboLogPort.Text = str;
                if (LogPort.IsOpen) LogPort.Close();
                try
                {   // try to open the selected LogPort:
                    LogPort.PortName = str;
                    DefaultLogPort = str;
                    SetDefaultLogPort();
                }
                catch
                {
                    MessageBox.Show("The RCP1 serial port " + LogPort.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboLogPort.SelectedText = "";
                }
            }
            // set LP100 Serial port to the last port used
            if (set.LPportNum != "")
            {
                cboLPport.Text = set.LPportNum;
                if (LPport.IsOpen) LPport.Close();
                try
                {   // try to open the selected LPport:
                    LPport.PortName = set.LPportNum;
                    DefaultLPport = set.LPportNum;
                    SetDefaultLPport();
                    chkLPenab.Checked = set.LPenab;
                }
                catch
                {
                    MessageBox.Show("The LP-100 serial port " + LPport.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboLPport.SelectedText = "";
                }
            }
            // set StepData Serial port to the last port used
            if (set.StepPortNum != "")
            {
                cboStep.Text = set.StepPortNum;
                if (StepData.IsOpen) StepData.Close();
                try
                {   // try to open the selected StepPort:
                    StepData.PortName = set.StepPortNum;
                    DefaultStepPort = set.StepPortNum;
                    SetDefaultStepPort();
                    chkStep.Checked = set.StepEnab;
                    txtStepInv.Text = set.StepInv;
                    cboStepCom.SelectedIndex = set.StepCom;
                }
                catch
                {
                    MessageBox.Show("The SteppIR serial port " + StepData.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboStep.SelectedText = "";
                }
            }
            // Set RCP2 serial port to last port used
            if (set.cboRCP2 != "")
            {
                cboRCP2.Text = set.cboRCP2;
                if (RCP2port.IsOpen) RCP2port.Close();
                try
                {
                    // try to open the selected port:
                    RCP2port.PortName = set.cboRCP2;
                    DefaultRCP2port = set.cboRCP2;
                    SetDefaultRCP2port();
                    chkRCP2.Checked = set.chkRCP2;
                }
                catch
                {
                    MessageBox.Show("The RCP2 serial port " + RCP2port.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboRCP2.SelectedText = "";
                }
            }
            // Set RCP3 serial port to last port used
            if (set.cboRCP3 != "")
            {
                cboRCP3.Text = set.cboRCP3;
                if (RCP3port.IsOpen) RCP3port.Close();
                try
                {
                    // try to open the selected port:
                    RCP3port.PortName = set.cboRCP3;
                    DefaultRCP3port = set.cboRCP3;
                    SetDefaultRCP3port();
                    chkRCP3.Checked = set.chkRCP3;
                }
                catch
                {
                    MessageBox.Show("The RCP3 serial port " + RCP3port.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboRCP3.SelectedText = "";
                }
            }
            // Set RCP4 serial port to last port used
            if (set.cboRCP4 != "")
            {
                cboRCP4.Text = set.cboRCP4;
                if (RCP4port.IsOpen) RCP4port.Close();
                try
                {
                    // try to open the selected port:
                    RCP4port.PortName = set.cboRCP4;
                    DefaultRCP4port = set.cboRCP4;
                    SetDefaultRCP4port();
                    chkRCP4.Checked = set.chkRCP4;
                }
                catch
                {
                    MessageBox.Show("The RCP4 serial port " + RCP4port.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboRCP4.SelectedText = "";
                }
            }
            // Set PW1 serial port to last port used
            if (set.cboPW1 != "")
            {
                cboPW1.Text = set.cboPW1;
                if (PW1port.IsOpen) PW1port.Close();
                try
                {
                    // try to open the selected port:
                    PW1port.PortName = set.cboPW1;
                    DefaultPW1port = set.cboPW1;
                    SetDefaultPW1port();
                    chkPW1.Checked = set.chkPW1;
                    chkDisBcast.Checked = set.chkPW1db;
                    txtPW1ta.Text = set.txtPW1ta;
                }
                catch
                {
                    MessageBox.Show("The IC-PW1 serial port " + PW1port.PortName +
                                    " cannot be opened!", "Port Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboPW1.SelectedText = "";
                }
            }
            // set LPT port to last one used
            str = set.lptPort;
            try
            {
                switch (str)
                {
                    case "LPT1":
                        rb1.Checked = true; LPTnum = 888; DefaultLPT = str;
                        break;
                    case "LPT2":
                        rb2.Checked = true; LPTnum = 632; DefaultLPT = str;
                        break;
                    case "LPT3":
                        rb3.Checked = true; LPTnum = 636; DefaultLPT = str;
                        break;
                    case "LPT4":
                        rb4.Checked = true; LPTnum = 620; DefaultLPT = str;
                        break;
                    case "None":
                        rbNone.Checked = true; LPTnum = 0; DefaultLPT = str;
                        break;
                    case "Other":
                        rbOther.Checked = true; LPTnum = Convert.ToInt32(set.lptNum);
                        txtPort.Text = set.lptNum; DefaultLPT = str;
                        break;
                    default:
                        rbNone.Checked = true; LPTnum = 0; DefaultLPT = "NONE";
                        chkDevice.Checked = false;
                        break;
                }
                btnPortNum.BackColor = Color.Transparent;
                lblPortBtn.Visible = false;
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
            fileName = set.DataFile;       // Get saved band data file name
            txtFile0.Text = fileName;      // Put band data name to file name box
            GetBandData(fileName);         // Load band data file to grid
            MacFileName = set.MacDataFile; // Get saved macro file name
            txtMacFile.Text = MacFileName; // Put macro file name to file name box
            GetMacData(MacFileName);       // Load macro file to grid
            Dev0.Text = set.Device0;
            cboDevice.Items.Add(set.Device0);
            cboDevice.Text = set.Device;

            if (chkLPenab.Checked) lpTimer.Enabled = true;
            else lpTimer.Enabled = false;

            logTimer.Enabled = true;
            pollTimer.Enabled = true;
            tempTimer.Enabled = true;

        }// Setup
        #endregion Initialization

        #region Data Grid Events

        // A cell value changed in the BCD data grid 
        private void dg1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string dgData = dg1.CurrentRow.Cells["freq"].Value.ToString();
            dgData = dgData.Substring(0, dgData.Length - 2);
            if (dgData != null)
            {
                if (flist.ContainsKey(dgData))
                {
                    flist.Remove(dgData);
                    flist.Add(dgData, dg1.CurrentRow.Cells["data"].Value);
                }
            }
        }

        #endregion Data Grid Events

        #region Delegates

        // Show/Hide "Antenna Moving" caption
        delegate void AntCallback(bool bCmd);
        private void ShowAnt(bool bCmd)
        {
            if (this.lblAnt.InvokeRequired)
            {
                AntCallback d = new AntCallback(ShowAnt);
                this.Invoke(d, new object[] { bCmd });
            }
            else
                this.lblAnt.Visible = bCmd;
        }
        // Set Title Bar caption
        delegate void SetTitleCallback(string text);
        private void SetTitle(string text)
        {
            if (!closing)
            {
                if (this.txtPort.InvokeRequired)
                {
                    SetTitleCallback d = new SetTitleCallback(SetTitle);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.Text = text;
                    this.StatusBar.Text = text;
                }
            }
        }
        // Write IC-PW1 ra address
        delegate void SetTextCallback(string text);
        private void raSetText(string text)
        {
            if (!closing)
            {
                if (this.txtPW1ra.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(raSetText);
                    this.Invoke(d, new object[] { text });
                }
                else
                    this.txtPW1ra.Text = text;
            }
        }
        // Write Temp reading to txt box
        delegate void SetTempCallback(string text);
        private void SetTemp(string text)
        {
            if (!closing)
            {
                if (this.txtTemp.InvokeRequired)
                {
                    SetTempCallback d = new SetTempCallback(SetTemp);
                    this.Invoke(d, new object[] { text });
                }
                else
                    txtTemp.Text = text;
            }
        }
        // Write LP-100 fwd Power reading to txt box
        delegate void SetFwdCallback(string text);
        private void SetFwd(string text)
        {
            if (!closing)
            {
                if (this.txtFwd.InvokeRequired)
                {
                    SetFwdCallback d = new SetFwdCallback(SetFwd);
                    this.Invoke(d, new object[] { text });
                }
                else
                    txtFwd.Text = text;
            }
        }
        // Write LP-100 SWR reading to txt box
        delegate void SetSwrCallback(string text);
        private void SetSwr(string text)
        {
            if (!closing)
            {
                if (this.txtSWR.InvokeRequired)
                {
                    SetSwrCallback d = new SetSwrCallback(SetSwr);
                    this.Invoke(d, new object[] { text });
                }
                else
                    txtSWR.Text = text;
            }
        }
        // Write BCD Digit
        delegate void SetDigitCallback(string text);
        private void SetDigit(string text)
        {
            if (!closing)
            {
                if (this.lblBCD.InvokeRequired)
                {
                    SetDigitCallback d = new SetDigitCallback(SetDigit);
                    this.Invoke(d, new object[] { text });
                }
                else
                    this.lblBCD.Text = text;
            }
        }
        #endregion Delegates

        #region Form Events

        // The station call sign has changed
        private void txtCall_TextChanged(object sender, EventArgs e)
        {
            set.Call = txtCall.Text;
            set.Save();
        }
        // The station latitude has changes
        private void txtLat_TextChanged(object sender, EventArgs e)
        {
            set.Latitude = txtLat.Text;
            set.Save();
        }
        // The station longitude has changes
        private void txtLong_TextChanged(object sender, EventArgs e)
        {
            set.Longitude = txtLong.Text;
            set.Save();
        }
        // The station grid has changes
        private void txtGrid_TextChanged(object sender, EventArgs e)
        {
            set.Grid = txtGrid.Text;
            set.Save();
        }
        // Tempature text box has been clicked
        private void txtTemp_Click(object sender, EventArgs e)
        {
            switch (temp_format)
            {
                case TempFormat.Celsius:
                    temp_format = TempFormat.Fahrenheit;
                    break;
                case TempFormat.Fahrenheit:
                    temp_format = TempFormat.Celsius;
                    break;
            }
        }
        // BCD device name has been changed, change the device tab and save to settings
        private void Dev0_TextChanged(object sender, EventArgs e)
        {
            this.tabDevice.Text = Dev0.Text;
            set.Device0 = Dev0.Text;
            set.Save();                     // save new Device0 to system settings
        }
        // BCD device Drop Down open - re-load cboDevice combo box
        private void Dev0_DropDown(object sender, EventArgs e)
        {
            cboDevice.Items.Clear();
            cboDevice.Items.Add(Dev0.Text);
        }
        // The Select Band Decoder Data File button was pressed
        private void btnFile0_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "xml files|*.xml";
            openFileDialog1.Title = "Select a XML File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFile0.Text = openFileDialog1.FileName;
                fileName = txtFile0.Text;
                set.DataFile = fileName;
                set.Save();
                GetBandData(fileName);
            }
        }
        // Re-Load BCD Device Data File
        private void btnReLoad0_Click(object sender, EventArgs e)
        {
            GetBandData(fileName);
        }
        //Save current decoder file after creation/modification
        private void btnSave0_Click(object sender, EventArgs e)
        {
            try
            {
                // Write out the Band Data from the grid to the XML file
                dg1.DataSource = ds;
                dg1.DataMember = ("band");
                if (txtFile0.Text == null || txtFile0.Text == "")
                {
                    MessageBox.Show("Please enter a name for the file", "File Name Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                fileName = txtFile0.Text;
                File.Delete(fileName);
                ds.WriteXml(fileName);
                set.DataFile = fileName;
                set.Save();
                GetBandData(fileName);
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
        }
        // call the help file
        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("HelpDecoder.htm");
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
        }
        // Form Load
        private void Setup_Load(object sender, EventArgs e)
        {
            closing = false;
            tabControl.SelectedIndex = set.TabOpen;
            cboPrefix.SelectedIndex = RandomNumber(0, 300);
            cboRCP1Rotor.SelectedIndex = set.RCP1RotorPort;
            cboRCP2Rotor.SelectedIndex = set.RCP2RotorPort;
            cboRCP3Rotor.SelectedIndex = set.RCP3RotorPort;
            cboRCP4Rotor.SelectedIndex = set.RCP4RotorPort;
        }
        // Create a random nnumber between min and max
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        // program is about to close
        private void Setup_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
            if (bMacChg)
            {
                DialogResult result;
                result = MessageBox.Show(this,
                    "Are you sure you want to exit?\n\n" +
                    "Macro data has been changed and not saved!\n\n" +
                    "If you want to save your work press NO and\n" +
                    "return to the Macro tab and save your work.",
                    "Macro Data UnSaved", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No) e.Cancel = true;
            }
        }
        // program is closed, lets cleanup
        private void Setup_FormClosing(object sender, FormClosedEventArgs e)
        {
            try
            {
                closing = true;
                set.WindowGeometry = GeometryToString(this);
                set.TabOpen = tabControl.SelectedIndex;
                set.Save();
                pollTimer.Enabled = false;
                logTimer.Enabled = false;
                PortAccess.Output(LPTnum, 0);
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
        }
        // The LPT port has changed
        private void grpLPT_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == rbNone)
            {
                OutParallelPort(LPTnum, 0); LPTnum = 0; set.lptPort = "NONE";
                txtPort.Text = LPTnum.ToString(); set.Save();
            }
            else if (sender == rbOther)
            { txtPort.Text = ""; set.lptPort = "Other"; set.Save(); }
            else if (sender == rb1)
            { LPTnum = 888; set.lptPort = "LPT1"; txtPort.Text = LPTnum.ToString(); set.Save(); }
            else if (sender == rb2)
            { LPTnum = 632; set.lptPort = "LPT2"; txtPort.Text = LPTnum.ToString(); set.Save(); }
            else if (sender == rb3)
            { LPTnum = 636; set.lptPort = "LPT3"; txtPort.Text = LPTnum.ToString(); set.Save(); }
            else if (sender == rb4)
            { LPTnum = 620; set.lptPort = "LPT4"; txtPort.Text = LPTnum.ToString(); set.Save(); }
            else
            { LPTnum = 0; set.lptPort = "NONE"; txtPort.Text = LPTnum.ToString(); set.Save(); }
        }
        // LPT Port number changed
        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            if (rbOther.Checked == true)
            { btnPortNum.BackColor = Color.Orchid; lblPortBtn.Visible = true; }
            else
            { btnPortNum.BackColor = Color.Transparent; lblPortBtn.Visible = false; }
        }
        // The Save LPTPort Number button was pressed
        private void btnPortNum_Click(object sender, EventArgs e)
        {
            LPTnum = Convert.ToInt32(txtPort.Text);
            set.lptNum = LPTnum.ToString();
            set.Save();
            btnPortNum.BackColor = Color.Transparent;
            lblPortBtn.Visible = false;
        }
        // CAT port timer interval changed
        private void txtInv_TextChanged(object sender, EventArgs e)
        {
            try
            {
                pollTimer.Enabled = false;
                pollInt = Convert.ToDouble(txtInv.Text);
                set.portInv = txtInv.Text;
                set.Save();     // save new polling interval to system settings
                pollTimer.Interval = pollInt;
                pollTimer.Enabled = true;
                if (pollInt >= 1000)
                    tempTimer.Interval = Convert.ToDouble(txtInv.Text) * 1.05;
                else if (pollInt < 1000)
                    tempTimer.Interval = 1000;
            }
            catch { }
        }
        // cboDevice selection changed
        private void cboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            set.Device = cboDevice.Text;
            set.Save();     // save new LPT device to system settings
        }
        // Device 0 enabled condition changed
        private void chkDev0_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDev0.Checked == true)
            {
                set.Dev0Enab = true;
                set.Save();
            }
            else
            {
                set.Dev0Enab = false;
                set.Save();
            }
        }
        // LPT Device enabled condition changed
        private void chkDevice_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDevice.Checked == true)
            {
                set.DevEnab = true;
                set.Save();
            }
            else
            {
                set.DevEnab = false;
                set.Save();
            }
        }
        // CI-V Hex Address has changed
        private void txtRadNum_TextChanged(object sender, EventArgs e)
        {
            set.CIVaddr = txtRadNum.Text;
            set.Save();
        }
        // New RCP1 port was selected
        private void cboLogPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LogPort.IsOpen) LogPort.Close();
            if (cboLogPort.SelectedItem.ToString() != "")
            {
                LogPort.PortName = cboLogPort.SelectedItem.ToString();
                try
                {
                    LogPort.Open();
                }
                catch
                {
                    MessageBox.Show("The RCP1 serial port " + LogPort.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboLogPort.SelectedText = "";
                    return;
                }
            }
            // save new port setting
            string str = set.LogPort;
            set.LogPort = cboLogPort.SelectedItem.ToString();
            set.Save();
        }
        // RCP1 Rotor Port has changed
        private void cboRCP1Rotor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RCP1Rotor.IsOpen) RCP1Rotor.Close();
            if (cboRCP1Rotor.SelectedItem.ToString() != "")
            {
                RCP1Rotor.PortName = cboRCP1Rotor.SelectedItem.ToString();
                try
                {
                    RCP1Rotor.Open();
                }
                catch
                {
                    MessageBox.Show("The RCP1 Rotor port " + RCP1Rotor.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboRCP1Rotor.SelectedIndex = 0;
                    return;
                }
            }
            // save new port setting
            set.RCP1RotorPort = cboRCP1Rotor.SelectedIndex;
            set.Save();
        }
        // RCP2 Rotor Port has changed
        private void cboRCP2Rotor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RCP2Rotor.IsOpen) RCP2Rotor.Close();
            if (cboRCP2Rotor.SelectedItem.ToString() != "")
            {
                RCP2Rotor.PortName = cboRCP2Rotor.SelectedItem.ToString();
                try
                {
                    RCP2Rotor.Open();
                }
                catch
                {
                    MessageBox.Show("The RCP2 Rotor port " + RCP2Rotor.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboRCP2Rotor.SelectedIndex = 0;
                    return;
                }
            }
            // save new port setting
            set.RCP2RotorPort = cboRCP2Rotor.SelectedIndex;
            set.Save();
        }
        // RCP3 Rotor Port has changed
        private void cboRCP3Rotor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RCP3Rotor.IsOpen) RCP3Rotor.Close();
            if (cboRCP3Rotor.SelectedItem.ToString() != "")
            {
                RCP3Rotor.PortName = cboRCP3Rotor.SelectedItem.ToString();
                try
                {
                    RCP3Rotor.Open();
                }
                catch
                {
                    MessageBox.Show("The RCP3 Rotor port " + RCP3Rotor.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboRCP3Rotor.SelectedIndex = 0;
                    return;
                }
            }
            // save new port setting
            set.RCP3RotorPort = cboRCP3Rotor.SelectedIndex;
            set.Save();
        }
        // RCP4 Rotor Port has changed
        private void cboRCP4Rotor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RCP4Rotor.IsOpen) RCP4Rotor.Close();
            if (cboRCP4Rotor.SelectedItem.ToString() != "")
            {
                RCP4Rotor.PortName = cboRCP4Rotor.SelectedItem.ToString();
                try
                {
                    RCP4Rotor.Open();
                }
                catch
                {
                    MessageBox.Show("The RCP4 Rotor port " + RCP4Rotor.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboRCP4Rotor.SelectedIndex = 0;
                    return;
                }
            }
            // save new port setting
            set.RCP4RotorPort = cboRCP4Rotor.SelectedIndex;
            set.Save();
        }
        // new acc port was selected
        private void cboSerAcc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (AccPort.IsOpen) AccPort.Close();
            if (cboSerAcc.SelectedItem.ToString() != "")
            {
                AccPort.PortName = cboSerAcc.SelectedItem.ToString();
                try
                {
                    AccPort.Open();
                }
                catch
                {
                    MessageBox.Show("The Passive Listener serial port " + AccPort.PortName +
                       " cannot be opened!", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboSerAcc.SelectedText = "";
                    return;
                }
            }
            // save new port setting
            string str = set.AccPort;
            set.AccPort = cboSerAcc.SelectedItem.ToString();
            set.Save();
        }
        // new radio port was selected
        private void cboCAT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sp.isOpen) sp.Close();
            sp.Name = cboCAT.SelectedItem.ToString();
            try
            {
                sp.Open();
            }
            catch
            {
                MessageBox.Show("The Radio serial port " + sp.Name +
                   " cannot be opened!\n", "Port Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCAT.SelectedText = "";
                return;
            }
            // save new port setting
            string str = set.RadioPort;
            set.RadioPort = cboCAT.SelectedItem.ToString();
            set.Save();
        }
        // Follow radio check box has changed
        private void cboFollow_CheckedChanged(object sender, EventArgs e)
        {
            if (cboFollow.Checked == true)
            {
                cboRadio.Enabled = true;
                chkMode.Enabled = true;
                set.followChk = true;
            }
            else
            {
                cboRadio.SelectedIndex = 0;
                //                portmode = PortMode.None;
                cboRadio.Enabled = false;
                chkMode.Enabled = false;
                set.followChk = false;
            }
            set.Save();
        }
        // Follow Radio type has changed
        private void cboRadio_SelectedIndexChanged(object sender, EventArgs e)
        {
            set.followRadio = (int)cboRadio.SelectedIndex;
            set.Save();
            switch (cboRadio.SelectedIndex)
            {
                case 0: // None
                    //                    cboRadData.SelectedIndex = 0;
                    portmode = PortMode.None;
                    txtRadNum.Enabled = false;
                    break;
                case 1: // Kenwood
                    portmode = PortMode.Kenwood;
                    txtRadNum.Enabled = false;
                    break;
                case 2: // Yaesu Type I
                    portmode = PortMode.YaesuTypeI;
                    txtRadNum.Enabled = false;
                    break;
                case 3: // Yaesu Type II
                    portmode = PortMode.YaesuTypeII;
                    txtRadNum.Enabled = false;
                    break;
                case 4: // Icom
                    portmode = PortMode.Icom;
                    txtRadNum.Enabled = true;
                    break;
                default:
                    break;
            }
        }
        // Radio Data combo box has changed
        private void cboRadData_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboRadData.SelectedIndex)
            {
                case 0: // 9600 8N1
                    AccPort.BaudRate = 9600;
                    AccPort.DataBits = 8;
                    AccPort.Parity = System.IO.Ports.Parity.None;
                    AccPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 1: // 9600 8N2
                    AccPort.BaudRate = 9600;
                    AccPort.DataBits = 8;
                    AccPort.Parity = System.IO.Ports.Parity.None;
                    AccPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                case 2: // 4800 8N1
                    AccPort.BaudRate = 4800;
                    AccPort.DataBits = 8;
                    AccPort.Parity = System.IO.Ports.Parity.None;
                    AccPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 3: // 4800 8N2
                    AccPort.BaudRate = 4800;
                    AccPort.DataBits = 8;
                    AccPort.Parity = System.IO.Ports.Parity.None;
                    AccPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                case 4: // 2400 8N1
                    AccPort.BaudRate = 2400;
                    AccPort.DataBits = 8;
                    AccPort.Parity = System.IO.Ports.Parity.None;
                    AccPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 5: // 2400 8N2
                    AccPort.BaudRate = 2400;
                    AccPort.DataBits = 8;
                    AccPort.Parity = System.IO.Ports.Parity.None;
                    AccPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                case 6: // 1200 8N1
                    AccPort.BaudRate = 1200;
                    AccPort.DataBits = 8;
                    AccPort.Parity = System.IO.Ports.Parity.None;
                    AccPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 7: // 1200 8N2
                    AccPort.BaudRate = 1200;
                    AccPort.DataBits = 8;
                    AccPort.Parity = System.IO.Ports.Parity.None;
                    AccPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    break;
            }
            set.RadData = (int)cboRadData.SelectedIndex;
            set.Save();
        }
        // LP port number has changed
        private void cboLPport_SelectedIndexChanged(object sender, EventArgs e)
        {
            lpTimer.Enabled = false;
            if (LPport.IsOpen) LPport.Close();
            if (cboLPport.SelectedItem.ToString() != "")
            {
                LPport.PortName = cboLPport.SelectedItem.ToString();
                try
                {
                    LPport.Open();
                    lpTimer.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("The LP-100 serial port " + LPport.PortName +
                       " cannot be opened!\n", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboLPport.SelectedText = "";
                    return;
                }
            }
            else
            {
                lpTimer.Enabled = false;
                chkLPenab.Checked = false;
            }
            // save new port setting
            set.LPportNum = cboLPport.SelectedItem.ToString();
            set.Save();
        }
        // LP timer interval has changed
        private void txtLPint_TextChanged(object sender, EventArgs e)
        {
            try
            {
                set.LPint = txtLPint.Text;
                set.Save();
            }
            catch { }
        }
        // LP enabled check box has changed
        private void chkLPenab_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLPenab.Checked)
            {
                if (cboLPport.SelectedIndex > 0)
                {
                    lpTimer.Enabled = true;
                    set.LPenab = true;
                }
                else
                {
                    MessageBox.Show("No port has been selected for the LP-100.\n\n" +
                    "Please select a valid port number and try again.", "Port Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chkLPenab.Checked = false;
                }
            }
            else
            {
                lpTimer.Enabled = false;
                set.LPenab = false;
                txtFwd.Text = "";
                txtSWR.Text = "";
            }
            set.Save();
        }
        // Profiller button was pressed
        private void btnProfiler_Click(object sender, EventArgs e)
        {
            if (txtProfLoc.Text != "" && txtProfLoc.Text != null)
            {
                try
                {
                    sp.Close();
                    process = Process.Start(txtProfLoc.Text);
                    this.Text = "Starting Profiler";
                }
                catch (Exception ex)
                {
                    bool bReturnLog = false;
                    bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                    if (false == bReturnLog) MessageBox.Show("Unable to write to log");
                }
            }
            else
                MessageBox.Show("No location has been selected for the FlexProfiler.exe file.\n\n" +
                    "Please select a file location on the 'Other' tab and try again.", "File Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        // Re-Start button was pressed
        private void btnReStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsFPRunning())
                {
                    process.Kill();
                    process.WaitForExit();
                }
                if (!sp.isOpen)
                {
                    sp.Open();
                    this.Text = "DDUtil Re-Started";
                }
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
        }
        // The Flex Profiler File Location Select button was pressed
        private void btnPFfile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "exe files|*.exe";
            openFileDialog1.Title = "Select Flex Profiler File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtProfLoc.Text = openFileDialog1.FileName;
                set.ProfLoc = txtProfLoc.Text;
                set.Save();
            }
        }
        // RCP2 enabled check box changed
        private void chkRCP2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRCP2.Checked)
            {
                if (cboRCP2.SelectedIndex > 0)
                {
                    set.chkRCP2 = true;
                }
                else
                {
                    MessageBox.Show("No port has been selected for this RCP.\n\n" +
                    "Please select a valid port number and try again.", "Port Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    chkRCP2.Checked = false;
                    set.chkRCP2 = false;
                }
            }
            else
            {
                set.chkRCP2 = false;
            }
            set.Save();

        }
        // RCP3 enabled check box changed
        private void chkRCP3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRCP3.Checked)
            {
                if (cboRCP3.SelectedIndex > 0)
                {
                    set.chkRCP3 = true;
                }
                else
                {
                    MessageBox.Show("No port has been selected for this RCP.\n\n" +
                    "Please select a valid port number and try again.", "Port Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    chkRCP3.Checked = false;
                    set.chkRCP3 = false;
                }
            }
            else
            {
                set.chkRCP3 = false;
            }
            set.Save();

        }
        // RCP4 enabled check box changed
        private void chkRCP4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRCP4.Checked)
            {
                if (cboRCP4.SelectedIndex > 0)
                {
                    set.chkRCP4 = true;
                }
                else
                {
                    MessageBox.Show("No port has been selected for this RCP.\n\n" +
                    "Please select a valid port number and try again.", "Port Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    chkRCP4.Checked = false;
                    set.chkRCP4 = false;
                }
            }
            else
            {
                set.chkRCP4 = false;
            }
            set.Save();

        }
        // RCP2 port number changed
        private void cboRCP2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RCP2port.IsOpen) RCP2port.Close();
            if (cboRCP2.SelectedItem.ToString() != "")
            {
                RCP2port.PortName = cboRCP2.SelectedItem.ToString();
                try
                {
                    RCP2port.Open();
                }
                catch
                {
                    MessageBox.Show("The RCP2 serial port " + RCP2port.PortName +
                       " cannot be opened!\n", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboRCP2.SelectedText = "";
                    return;
                }
            }
            else
            {
                chkRCP2.Checked = false;
            }
            // save new port setting
            set.cboRCP2 = cboRCP2.SelectedItem.ToString();
            set.Save();
        }
        // RCP3 port number changed
        private void cboRCP3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RCP3port.IsOpen) RCP3port.Close();
            if (cboRCP3.SelectedItem.ToString() != "")
            {
                RCP3port.PortName = cboRCP3.SelectedItem.ToString();
                try
                {
                    RCP3port.Open();
                }
                catch
                {
                    MessageBox.Show("The RCP3 serial port " + RCP3port.PortName +
                       " cannot be opened!\n", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboRCP3.SelectedText = "";
                    return;
                }
            }
            else
            {
                chkRCP3.Checked = false;
            }
            // save new port setting
            set.cboRCP3 = cboRCP3.SelectedItem.ToString();
            set.Save();
        }
        // RCP4 port number changed
        private void cboRCP4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RCP4port.IsOpen) RCP4port.Close();
            if (cboRCP4.SelectedItem.ToString() != "")
            {
                RCP4port.PortName = cboRCP4.SelectedItem.ToString();
                try
                {
                    RCP4port.Open();
                }
                catch
                {
                    MessageBox.Show("The RCP4 serial port " + RCP4port.PortName +
                       " cannot be opened!\n", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboRCP4.SelectedText = "";
                    return;
                }
            }
            else
            {
                chkRCP4.Checked = false;
            }
            // save new port setting
            set.cboRCP4 = cboRCP4.SelectedItem.ToString();
            set.Save();
        }
        // PW1 Enable Checkbox has changed
        private void chkPW1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPW1.Checked)
            {
                if (cboPW1.SelectedIndex > 0)
                {
                    set.chkPW1 = true;
                }
                else
                {
                    MessageBox.Show("No port has been selected for PW1.\n\n" +
                    "Please select a valid port number and try again.", "Port Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    chkPW1.Checked = false;
                    set.chkPW1 = false;
                }
            }
            else
            {
                set.chkPW1 = false;
            }
            set.Save();

        }
        // PW1 Port Number has changed
        private void cboPW1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PW1port.IsOpen) PW1port.Close();
            if (cboPW1.SelectedItem.ToString() != "")
            {
                PW1port.PortName = cboPW1.SelectedItem.ToString();
                try
                {
                    PW1port.Open();
                }
                catch
                {
                    MessageBox.Show("The IC-PW1 serial port " + PW1port.PortName +
                       " cannot be opened!\n", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboPW1.SelectedText = "";
                    return;
                }
            }
            else
            {
                chkPW1.Checked = false;
            }
            // save new port setting
            set.cboPW1 = cboPW1.SelectedItem.ToString();
            set.Save();
        }
        // PW1ra CI-V address textbox has changed
        private void txtPW1ra_TextChanged(object sender, EventArgs e)
        {
            set.txtPW1ra = txtPW1ra.Text;
            set.Save();
        }
        // PW1ta CI-V address textbox has changed
        private void txtPW1ta_TextChanged(object sender, EventArgs e)
        {
            set.txtPW1ta = txtPW1ta.Text;
            set.Save();
        }
        // The PW1 Disable Broadcast check box has changed.
        private void chkDisBcast_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDisBcast.Checked) set.chkPW1db = true;

            else set.chkPW1db = false;
            set.Save();
        }
        // The RCP2 Disable Polling check box has changed
        private void chkRCP2DisPol_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRCP2DisPol.Checked) set.RCP2DisPol = true;

            else set.RCP2DisPol = false;
            set.Save();
        }
        // The RCP3 Disable Polling check box has changed
        private void chkRCP3DisPol_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRCP3DisPol.Checked) set.RCP3DisPol = true;

            else set.RCP3DisPol = false;
            set.Save();
        }
        // The RCP4 Disable Polling check box has changed
        private void chkRCP4DisPol_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRCP4DisPol.Checked) set.RCP4DisPol = true;

            else set.RCP4DisPol = false;
            set.Save();
        }
        // The RCP2IF check box has changed
        private void chkRCP2IF_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRCP2IF.Checked) set.RCP2IF = true;
            else set.RCP2IF = false;
            set.Save();
        }
        // The RCP3IF check box has changed
        private void chkRCP3IF_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRCP3IF.Checked) set.RCP3IF = true;
            else set.RCP3IF = false;
            set.Save();
        }
        // The RCP4IF check box has changed
        private void chkRCP4IF_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRCP4IF.Checked) set.RCP4IF = true;
            else set.RCP4IF = false;
            set.Save();
        }
        // The Thread.Sleep interval has changed
        private void txtSleep_TextChanged(object sender, EventArgs e)
        {
            try
            {
                set.Sleep = txtSleep.Text;
                set.Save();
                iSleep = Convert.ToInt32(txtSleep.Text);
            }
            catch { }
        }
        // Hide/UnHide Sleep text box
        private void tabOther_DoubleClick(object sender, EventArgs e)
        {
            if (txtSleep.Visible)
            { txtSleep.Visible = false; lblSleep.Visible = false; }
            else
            { txtSleep.Visible = true; lblSleep.Visible = true; }
        }
        // The Program Memo text box has changed.
        private void txtRCP_TextChanged(object sender, EventArgs e)
        {
            try
            {   set.RCPtext = txtRCP.Text; set.Save(); }
            catch { }
        }
        // The tool tips check box has changed
        private void chkTips_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTips.Checked)
            { toolTip1.Active = true; set.ToolTips = true; }
            else
            { toolTip1.Active = false;  set.ToolTips = false; }
            set.Save();
        }
        // Slave mode checkbox has changed
        private void chkMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMode.Checked) set.slaveMode = true;
            else set.slaveMode = false;
            set.Save();
        }

        #endregion Form Events

        #region Helper Methods

        private void OutParallelPort(int port, int num)
        {
            if (chkDevice.Checked == true)
            {
                PortAccess.Output(port, num);
            }
            else
            {
                PortAccess.Output(port, 0);
            }
        }
        // Load Data File
        private void GetBandData(string fileName)
        {
            try
            {
                // Read in the Band Data from the XML file and display in datagrid
                ds.Clear();
                ds.ReadXml(fileName);

                dg1.DataSource = ds;
                dg1.DataMember = ("band");
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
            // Read in the Band data from the XML file into the hash table
            // Uses format 14080 = 140, 3575 = 35. This placs a different value pair every 100 khz
            try
            {
                flist.Clear();
                string freq = "";
                string data = "";
                FileStream bdata = new FileStream(fileName, FileMode.Open);
                XPathDocument myDoc = new XPathDocument(bdata);
                XPathNavigator docNav = myDoc.CreateNavigator();
                foreach (XPathNavigator node in docNav.Select("//banddata/bandmulti/band/*"))
                {
                    switch (node.Name)
                    {
                        case "freq":
                            freq = node.Value.ToString();
                            freq = freq.Substring(0, freq.Length - 2);
                            break;
                        case "data":
                            data = node.Value.ToString();
                            flist.Add(freq, data);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
        }
        // Lookup freq data in hash table & output to LPT port
        private void LookUp(string freq)
        {
            if (flist.ContainsKey(freq) && chkDevice.Checked)
            {
                keyValue = Convert.ToInt16(flist[freq]);
                OutParallelPort(LPTnum, keyValue);    // port number(decimal), value(decimal)
            }
            else
            {
                OutParallelPort(LPTnum, 0);     // if freq key not found, turn off port
                keyValue = 0;
            }
        }
        // See if a copy of DDUtil is already running
        public static bool IsAppAlreadyRunning()
        {
            bool IsRunning = false;
            Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (currentProcess.Id != process.Id)
                {
                    if (currentProcess.ProcessName.Substring(0, 11) == process.ProcessName)
                    {
                        IsRunning = true;
                        break;
                    }
                }
            }
            return IsRunning;
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
        // Display the PA Temp
        private void WriteTemp()
        {
//            temp += 2; if (temp >= 110) temp = 40;

            if (temp >= 50 && temp < 70) txtTemp.BackColor = Color.Yellow;
            else if (temp >= 70 && temp < 80) txtTemp.BackColor = Color.Orange;
            else if (temp >= 80) txtTemp.BackColor = Color.Red;
            else txtTemp.BackColor = Color.LightGreen;
            switch (temp_format)
            {
                case TempFormat.Celsius:
                    SetTemp(temp.ToString("f1") + "° C");
                    break;
                case TempFormat.Fahrenheit:
                    SetTemp(((temp * 1.8) + 32).ToString("f1") + "° F");
                    break;
            }
        }
        #endregion Helper Methods

        #region Macro Routines

        /**************** Macro Events ****************/
        /// <summary>
        /// Hides/Un-hides the macro editor and file dialog controls.
        /// To use click in an open area of the Macro tab.
        /// </summary>
        private void tabMacro_Click(object sender, EventArgs e)
        {
            if (dgm.Visible)
            { dgm.Visible = false; grpMacro.Visible = false; }
            else
            { dgm.Visible = true; grpMacro.Visible = true; }
        }
        // A Data Grid cell value changed, change the button text.
        bool bMacChg; // flag for macro data changed & not saved.
        private void dgm_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            bMacChg = true;
            dgm.CurrentRow.Cells["Command"].Value = 
                dgm.CurrentRow.Cells["Command"].Value.ToString().ToUpper();
            string dgmData = dgm.CurrentRow.Cells["Button"].Value.ToString();
            if (dgmData != null)
            {
                switch (e.RowIndex)
                {
                    case 0: btnMacro1.Text = dgmData; break;
                    case 1: btnMacro2.Text = dgmData; break;
                    case 2: btnMacro3.Text = dgmData; break;
                    case 3: btnMacro4.Text = dgmData; break;
                    case 4: btnMacro5.Text = dgmData; break;
                    case 5: btnMacro6.Text = dgmData; break;
                    case 6: btnMacro7.Text = dgmData; break;
                    case 7: btnMacro8.Text = dgmData; break;
                    case 8: btnMacro9.Text = dgmData; break;
                    case 9: btnMacro10.Text = dgmData; break;
                    case 10: btnMacro11.Text = dgmData; break;
                    case 11: btnMacro12.Text = dgmData; break;
                    default: break;
                }
            }
        }
        /// <summary>
        /// Saves the macro commands displayed in the grid to the file name selected 
        /// in the File Name text box. If name is empty a file dialog is called.
        /// </summary>
        private void btnMacSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMacFile.Text == null || txtMacFile.Text == "")
                {
                    MessageBox.Show("Please enter a name for the file", "File Name Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                // Write out the Macro Data from the grid to the XML file
                dgm.DataSource = dsm;
                dgm.DataMember = ("macro");
                MacFileName = txtMacFile.Text;
                File.Delete(MacFileName);
                dsm.WriteXml(MacFileName);
                set.MacDataFile = MacFileName;
                set.Save();
                GetMacData(MacFileName);
                bMacChg = false;
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
        }
        /// <summary>
        /// Re-Loads the currently open Macro File. Calls GetMacData()
        /// The purpose of this function is to allow the user to recover 
        /// from a goof if he hasn't saved the file yet.
        /// </summary>
        private void btnMacReLoad_Click(object sender, EventArgs e)
        {
            GetMacData(MacFileName);
        }
        /// <summary>
        /// Opens file dialog to select the Macro Data file.
        /// Calls GetMacData()
        /// </summary>
        private void btnMacSelect_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "xml files|*.xml";
                openFileDialog1.Title = "Select a XML File";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtMacFile.Text = openFileDialog1.FileName;
                    MacFileName = txtMacFile.Text;
                    set.MacDataFile = MacFileName;
                    set.Save();
                    GetMacData(MacFileName);
                }
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
        }
        /// <summary>
        /// If SDR not transmitting and SteppIR is not moving
        /// Process the commands associated with a macro key, calls ParseBuffer()
        /// </summary>
        /// <param name="button"> index of the button that was pressed</param>
        private void ProcessMacroButton(int button)
        {
            if (StepCtr == 0 && xOn == "0")
            {   // 
                try
                {
                    if (dgm.Rows[button - 1].Cells[1].Value.ToString() == "")
                    {
                        throw new NullReferenceException();
                    }
                    string cmds = dgm.Rows[button - 1].Cells[1].Value.ToString();
                    ParseBuffer(cmds);
                }
                catch (NullReferenceException)
                {
                    int btn = button + 1;
                    MessageBox.Show("There are no commands setup for this Macro " + button);
                }
            }
        }
        // Macro button #1 was pressed
        private void btnMacro1_Click(object sender, EventArgs e)
        { ProcessMacroButton(1); }
        // Macro button #2 was pressed
        private void btnMacro2_Click(object sender, EventArgs e)
        { ProcessMacroButton(2); }
        // Macro button #3 was pressed
        private void btnMacro3_Click(object sender, EventArgs e)
        { ProcessMacroButton(3); }
        // Macro button #4 was pressed
        private void btnMacro4_Click(object sender, EventArgs e)
        { ProcessMacroButton(4); }
        // Macro button #5 was pressed
        private void btnMacro5_Click(object sender, EventArgs e)
        { ProcessMacroButton(5); }
        // Macro button #6 was pressed
        private void btnMacro6_Click(object sender, EventArgs e)
        { ProcessMacroButton(6); }
        // Macro button #7 was pressed
        private void btnMacro7_Click(object sender, EventArgs e)
        { ProcessMacroButton(7); }
        // Macro button #8 was pressed
        private void btnMacro8_Click(object sender, EventArgs e)
        { ProcessMacroButton(8); }
        // Macro button #9 was pressed
        private void btnMacro9_Click(object sender, EventArgs e)
        { ProcessMacroButton(9); }
        // Macro button #10 was pressed
        private void btnMacro10_Click(object sender, EventArgs e)
        { ProcessMacroButton(10); }
        // Macro button #11 was pressed
        private void btnMacro11_Click(object sender, EventArgs e)
        { ProcessMacroButton(11); }
        // Macro button #12 was pressed
        private void btnMacro12_Click(object sender, EventArgs e)
        { ProcessMacroButton(12); }

        // A key was pressed check for "F" key
        private void Setup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            { ProcessMacroButton(1); }
            else if (e.KeyCode == Keys.F2)
            { ProcessMacroButton(2); }
            else if (e.KeyCode == Keys.F3)
            { ProcessMacroButton(3); }
            else if (e.KeyCode == Keys.F4)
            { ProcessMacroButton(4); }
            else if (e.KeyCode == Keys.F5)
            { ProcessMacroButton(5); }
            else if (e.KeyCode == Keys.F6)
            { ProcessMacroButton(6); }
            else if (e.KeyCode == Keys.F7)
            { ProcessMacroButton(7); }
            else if (e.KeyCode == Keys.F8)
            { ProcessMacroButton(8); }
            else if (e.KeyCode == Keys.F9)
            { ProcessMacroButton(9); }
            else if (e.KeyCode == Keys.F10)
            { ProcessMacroButton(10); }
            else if (e.KeyCode == Keys.F11)
            { ProcessMacroButton(11); }
            else if (e.KeyCode == Keys.F12)
            { ProcessMacroButton(12); }
        }
        /// <summary>
        /// Adds macro number text to the row header
        /// </summary>
        /// <param name="button"> index of the button that was pressed</param>
        public void dgm_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string rowNum = (e.RowIndex + 1).ToString();

            //determine the display size of the row number string using
            //the DataGridView's current font.
            SizeF size = e.Graphics.MeasureString(rowNum, this.Font);

            //adjust the width of the column that contains the row header cells 
            if (dgm.RowHeadersWidth < (int)(size.Width + 20))
                dgm.RowHeadersWidth = (int)(size.Width + 20);

            // Set the brush for drawing the text
            Brush b = SystemBrushes.ControlText;

            // Draw the row number string using the brush defined above
            e.Graphics.DrawString("M" + rowNum, this.Font, b, e.RowBounds.Location.X + 15,
                       e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }
        /**************** Macro Methods ****************/
        /// <summary>
        /// Loads Macro Data File from disk and displays in datagrid, 
        /// adds name from button cell to macro buttons
        /// </summary>
        /// <param name="fileName"> the file to be loaded</param>
        private void GetMacData(string fileName)
        {
            try
            {
                // Read in the Band Data from the XML file 
                dsm.Clear();
                dsm.ReadXml(fileName);
                dgm.DataSource = dsm;
                dgm.DataMember = ("macro");
                for (int i = 0; i < dgm.RowCount - 1; i++)
                {   // write the names on the buttons
                    string btnName = dgm.Rows[i].Cells[0].Value.ToString();
                    if (btnName != null)
                    {
                        DataGridViewRowHeaderCell cell = new DataGridViewRowHeaderCell();
                        switch (i)
                        {
                            case 0: btnMacro1.Text = btnName; break;
                            case 1: btnMacro2.Text = btnName; break;
                            case 2: btnMacro3.Text = btnName; break;
                            case 3: btnMacro4.Text = btnName; break;
                            case 4: btnMacro5.Text = btnName; break;
                            case 5: btnMacro6.Text = btnName; break;
                            case 6: btnMacro7.Text = btnName; break;
                            case 7: btnMacro8.Text = btnName; break;
                            case 8: btnMacro9.Text = btnName; break;
                            case 9: btnMacro10.Text = btnName; break;
                            case 10: btnMacro11.Text = btnName; break;
                            case 11: btnMacro12.Text = btnName; break;
                            default: break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
        }
        /// <summary>
        /// Processes commands from the data grid, calls WriteToPort()
        /// which sends commands to the radio. This routine includes a buffer
        /// (queue)to hold multiple commands until the radio is ready.
        /// </summary>
        /// <param name="cmd"> string of commands to process</param>
        private void ParseBuffer(string cmd)
        {
            try
            {   // put the CAT command string into the comm buffer
                string CmdBuf = cmd;
                string OutBuf = "";
                Regex rex = new Regex(".*?;");  //accept any string ending in ";"		
                //loop thru the buffer and find matches
                for (Match m = rex.Match(CmdBuf); m.Success; m = m.NextMatch())
                {
                    OutBuf = m.Value;
                    // remove the match from the buffer if found
                    CmdBuf = CmdBuf.Replace(m.Value, "");
                    if (OutBuf.Substring(0, 2) == "DD") DDParse(OutBuf);
                    else WriteToPort(OutBuf, 100);
                }
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
        }
        // Macro command is DDUtil specific "DDSP045;"
        private void DDParse(string cmd)
        {
            cmd = cmd.Substring(0, cmd.Length - 1);
            if (cmd.Length < 4) { malform(cmd); return; }
            string pre = cmd.Substring(2, 2);             // Command prefix
            string ops = cmd.Substring(4, cmd.Length-4);  // command operators less ";"
            switch (pre)
            {   
                case "FW":  // Set SteppIR to Forward Mode
                    rbFwd.Checked = true; break;

                case "BI":  // Set SteppIR to Bi-Directional Mode
                    rbBiDir.Checked = true; break;

                case "RV":  // Set SteppIR to Reverse Mode
                    rb180.Checked = true; break;

                case "34":  // Set SteppIR to 3/4 wavelength Mode (verticle only)
                    rb34.Checked = true; break;

                case "RE":  // Enable Rotor Control
                    if (ops.Length == 1)
                    {
                        if (ops == "1") chkRotorEnab.Checked = true;
                        else chkRotorEnab.Checked = false;
                    }
                    else malform(pre);
                    break;

                case "SE":  // Enabled SteppIR
                    if (ops.Length == 1)
                    {
                        if (ops == "1") chkStep.Checked = true;
                        else chkStep.Checked = false;
                    }
                    else malform(pre);
                    break; 

                case "SP":  // Turn rotor
                    if (ops.Length == 3)
                    { 
                        txtSP.Text = cmd.Substring(4, 3); btnSP_Click(null, null); 
                    }
                    else malform(pre);                    
                    break;
                default: malform(cmd);
                    break; 
            }
        }
        private void malform(string pre)
        {
            Notification alert = new Notification();
            Notification.notiMsg =
                "\r\rThe command " + pre + " is not formed correctly or is not " + 
                "a valid DDUtil macro command.\r\r" +
                "Please re-format the command and try again";
            alert.Show();
        }

        #endregion Macro Routines

        #region Menu Events

        // Main Menu|File|Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
//            set.MyLoc = this.Location;
//            set.MySize = this.Size;
            set.WindowGeometry = GeometryToString(this);
            set.TabOpen = tabControl.SelectedIndex;
            set.Save();
            closing = true;
            this.Close();
        }
        // Main Menu|Tools|Enable Error Log
        private void enableErrorLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enableErrorLoggingToolStripMenuItem.Checked)
            {
                enableErrorLoggingToolStripMenuItem.Checked = false;
                enableErrorLog = false;
                set.ErrorLog = false;
            }
            else
            {
                enableErrorLoggingToolStripMenuItem.Checked = true;
                enableErrorLog = true;
                set.ErrorLog = true;
            }
            set.Save();
        }
        // Main Menu|Tools|Open Error Log
        private void openErrorLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("ErrorLog.txt");
        }
        // Main Menu|Help|DDUtil Help

        private void dDutilHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("HelpDecoder.htm");
        }
        // Main Menu|Help|OnLine Help
        private void onLineHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://k5fr.com/ddutilwiki/index.php?title=Setup");
        }
        // Main Menu|Help|Check for Updates
        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://k5fr.com/ddutilwiki/index.php?title=Latest_Revision");
        }
        // Main Menu|Help|Web Site
        private void webSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://k5fr.com/ddutilwiki/index.php");
        }

        /********************************************************************/
        // Context Menu|Restore Form Size
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Size = new Size(430, 445);
        }
        // Context Menu|Shrink Form Size
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Size = new Size(430, 60);
        }
        // Context Menu|About DDUtil
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.Show();
        }
        // Context Menu|Slave Radio Info
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Info for selecting a Slave Radio Type\n\n" +
                "- Select Kenwood for:\n" +
                "   All Kenwoods, K2, K3, FT9000, FT2000, FT950, FT450\n\n" +
                "- Select Yaesu Type I for: FT1000(all), FT990, FT920,\n" +
                "   FT900, FT890, FT840, FT757, FT747, FT100\n\n" +
                "- Select Yaesu Type II for: FT897, FT857, FT847, FT817\n\n" +
                "- Select Icom for:\n" +
                "   All Icom radios, TenTec emulating Icom\n",
                "Slave Radio Info", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        // Context Menu|LP-100
        private void lP100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "- Select the port the LP-100 is connected to.\n\n" +
                "- Select the polling interval in MS. This needs to\n" +
                "  be fairly fast or the readings will lag behind. \n" +
                "  Try between 100 - 500 MS (200 is a good choice).\n\n" +
                "- Check the Enabled Check Box to complete setup.\n",
                "LP-100 Setup Info", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        // Context Menu|ICOM IC-PW1
        private void iCOMICPW1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "- Select the port to which the IC-PW1 is attached.\n\n" +
                "- Select the CI-V (ta) address for DDUtil. This address is\n" +
                "  required to let the IC-PW1 know who is talking to it.\n" +
                "  Use the programmed default of 33 (hex) if unknown.\n\n" +
                "- Check the Enabled Check Box to complete the setup.\n",
                "IC-PW1 Setup Info", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        // Context Menu|Flex Profiler
        private void flexProfilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "- Select the file location where Profiler is installed.\n\n" +
                "- Select the PF Open button to start the Profiler.\n\n" +
                "- Work with profiles.\n\n" +
                "- Press the Re-Start button to activate DDUtil.\n",
                "Flex Profiler Setup & Operation", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        // Context Menu|Macro Commands
        private void macroFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
            "- To execute a macro command, press either the appropriate form button or desiginated 'F' key\n\n" +
            "- To display the Macro Editor screen, click any open spot on the Macro tab.\n\n" +
            "- To close the Macro Editor screen, click in the open spot at the end of the Data File Group\n\n" +
            "- See the documentation (Help Menu) for assistance with editing info and tips.\n\n",
            "Formatting and Using Macro Commands", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        // Context Menu|RCP ports
        private void rCPPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Setup procedure for using the bi-directional RCP ports.\n\n" +
                "These ports are for programs that need to talk to the radio\n" +
                "in order to change frequency, mode and other radio parameters.\n\n" +
                "- Select the desired port from the drop-down list-box.\n\n" +
                "- Check the Enabled check box to turn on a port.\n",
                "RCP Port Setup", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void rotorControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Setup procedure for using the Rotor Control feature.\n\n" +
                "- Select the Rotor Model and Speed (if applicable)\n\n" + 
                "- Select the desired Serial Port for your rotor.\n\n" +
                "- Select the Serial Port Comm data that matches your rotor.\n\n" +
                "- Check the Enabled check box to turn on a port.\n\n" +
                "- Enter your Latitude and Longitude for you location.\n",
                "Rotor Setup", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        #endregion Menu Events

        #region Rotor Control

        #region Rotor Events

        // The Short Path Rotor button has been pressed
        string rtrSpd = "";
        private void btnSP_Click(object sender, EventArgs e)
        {
            if (chkRotorEnab.Checked)
            { lblSP.Text = txtSP.Text; lblLP.Text = "SP"; TurnRotor(txtSP.Text); }
        }
        // The SP bearing window has changed, must be a manual entry.
        private void txtSP_TextChanged(object sender, EventArgs e)
        {
            int bearing = Convert.ToInt32(txtSP.Text);
            if (bearing < 180)
                txtLP.Text = Convert.ToInt32(bearing + 180).ToString();
            else
                txtLP.Text = Convert.ToInt32(bearing - 180).ToString();
        }

        // The Long Path Rotor button has been pressed
        private void btnLP_Click(object sender, EventArgs e)
        {
            if (chkRotorEnab.Checked)
            { lblSP.Text = txtLP.Text; lblLP.Text = "LP"; TurnRotor(txtLP.Text); }
        }
        // The Rotor Port number has changed
        private void cboRotorPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RotorPort.IsOpen) RotorPort.Close();
            if (cboRotorPort.SelectedItem.ToString() != "")
            {
                RotorPort.PortName = cboRotorPort.SelectedItem.ToString();
                try
                {
                    RotorPort.Open();
                }
                catch
                {
                    MessageBox.Show("The Rotor serial port " + RotorPort.PortName +
                       " cannot be opened!\n", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboRotorPort.SelectedText = "";
                    chkRotorEnab.Checked = false;
                    return;
                }
            }
            else
            {
                chkRotorEnab.Checked = false;
            }
            // save new port setting
            set.RotorPort = cboRotorPort.SelectedIndex;
            set.Save();
        }
        // The Rotor port com parameters have changed
        private void cboRotorCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboRotorCom.SelectedIndex)
            {
                case 0: // 9600 8N1
                    RotorPort.BaudRate = 9600;
                    RotorPort.DataBits = 8;
                    RotorPort.Parity = System.IO.Ports.Parity.None;
                    RotorPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 1: // 9600 8N2
                    RotorPort.BaudRate = 9600;
                    RotorPort.DataBits = 8;
                    RotorPort.Parity = System.IO.Ports.Parity.None;
                    RotorPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                case 2: // 4800 8N1
                    RotorPort.BaudRate = 4800;
                    RotorPort.DataBits = 8;
                    RotorPort.Parity = System.IO.Ports.Parity.None;
                    RotorPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 3: // 4800 8N2
                    RotorPort.BaudRate = 4800;
                    RotorPort.DataBits = 8;
                    RotorPort.Parity = System.IO.Ports.Parity.None;
                    RotorPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                case 4: // 2400 8N1
                    RotorPort.BaudRate = 2400;
                    RotorPort.DataBits = 8;
                    RotorPort.Parity = System.IO.Ports.Parity.None;
                    RotorPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 5: // 2400 8N2
                    RotorPort.BaudRate = 2400;
                    RotorPort.DataBits = 8;
                    RotorPort.Parity = System.IO.Ports.Parity.None;
                    RotorPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                case 6: // 1200 8N1
                    RotorPort.BaudRate = 1200;
                    RotorPort.DataBits = 8;
                    RotorPort.Parity = System.IO.Ports.Parity.None;
                    RotorPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 7: // 1200 8N2
                    RotorPort.BaudRate = 1200;
                    RotorPort.DataBits = 8;
                    RotorPort.Parity = System.IO.Ports.Parity.None;
                    RotorPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    break;
            }
            set.RotorCom = (int)cboRotorCom.SelectedIndex;
            set.Save();
        }
        // The Enable Rotor Check Box has changed
        private void chkRotorEnab_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRotorEnab.Checked)
            {
                btnSP.Enabled = true;
                btnLP.Enabled = true;
                cboRCP1Rotor.Enabled = true;
                cboRCP2Rotor.Enabled = true;
                cboRCP3Rotor.Enabled = true;
                cboRCP4Rotor.Enabled = true;
                if (cboRotorPort.SelectedIndex > 0)
                {
                    set.RotorEnab = true;
                }
                else
                {
                    MessageBox.Show("No port has been selected for the Rotor.\n\n" +
                    "Please select a valid port number and try again.", "Port Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chkRotorEnab.Checked = false;
                }
            }
            else
            {
                set.RotorEnab = false;
                btnSP.Enabled = false;
                btnLP.Enabled = false;
                cboRCP1Rotor.Enabled = false;
                cboRCP2Rotor.Enabled = false;
                cboRCP3Rotor.Enabled = false;
                cboRCP4Rotor.Enabled = false;

            }
            set.Save();
        }
        // the Rotor Model selection has changed
        string suffix = ""; // termination string for RCP1~4 Rotor_DataReceived() search
        private void grpModel_CheckedChanged(object sender, EventArgs e)
        {
                 if (rbRtrMod1.Checked) 
                 { 
                     set.rotorModel = 0; 
                     rotormod = RotorMod.AlphaSpid;
                     grpSpeed.Visible = false;
                     suffix = "/ "; // 2F, 20
                 }
                 else if (rbRtrMod2.Checked)
                 {
                     set.rotorModel = 1;
                     rotormod = RotorMod.GreenHeron;
                     grpSpeed.Visible = false;
                     suffix = ";";
                 }
                 else if (rbRtrMod3.Checked) 
                 { 
                     set.rotorModel = 2; 
                     rotormod = RotorMod.Hygain;
                     grpSpeed.Visible = false;
                     suffix = ";";
                 }
                 else if (rbRtrMod4.Checked)
                 {
                     set.rotorModel = 3; 
                     rotormod = RotorMod.M2R2800PA;
                     grpSpeed.Visible = true;
                     suffix = "\r";
                 }
                 else if (rbRtrMod5.Checked)
                 {
                     set.rotorModel = 4; 
                     rotormod = RotorMod.M2R2800PX;
                     grpSpeed.Visible = true;
                     suffix = "\r";
                 }
                 else if (rbRtrMod6.Checked) 
                 { 
                     set.rotorModel = 5; 
                     rotormod = RotorMod.Prosistel;
                     grpSpeed.Visible = false;
                     suffix = "\r";
                 }
                 else if (rbRtrMod7.Checked)
                 {
                     set.rotorModel = 6; rotormod = RotorMod.Yaesu;
                     grpSpeed.Visible = true;
                     suffix = "\r";
                 }
            set.Save();
        }
        // the rotor speed selection has changed
        private void grpSpeed_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == rbRtrSpd1) { set.RotorSpeed = 0; rtrSpd = "1"; }
            else if (sender == rbRtrSpd2) { set.RotorSpeed = 1; rtrSpd = "5"; }
            else if (sender == rbRtrSpd3) { set.RotorSpeed = 2; rtrSpd = "9"; }
            else return;
            set.Save();
            if (rotormod == RotorMod.M2R2800PA) RotorPort.Write("S" + rtrSpd + "\r");
            if (rotormod == RotorMod.M2R2800PX) RotorPort.Write("S" + rtrSpd + "\r");
            if (rotormod == RotorMod.Yaesu) RotorPort.Write("X" + rtrSpd + "\r");
        }
        // *** Prefix Combo Box Events ***
        string pre = "";
        private void cboPrefix_SelectedIndexChanged(object sender, EventArgs e)
        { GetPrefix(); }

        private void cboPrefix_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { GetPrefix(); pre = ""; }
        }
        private void cboPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {
            pre += e.KeyChar;
            if (pre == "\r" || pre == "") { pre = ""; goto Done; }
            string SQL = "SELECT * FROM DX WHERE DXCCPrefix LIKE '" + 
                            pre + "%' ORDER BY DXCCprefix";
            GetDXCC(SQL);
        Done: e.Handled = true; cboPrefix.Focus();
        }
        private void cboPrefix_Click(object sender, EventArgs e)
        { cboPrefix.DroppedDown = true; pre = ""; }

        private void cboPrefix_Enter(object sender, EventArgs e)
        { pre = ""; }

        private void cboPrefix_Leave(object sender, EventArgs e)
        { GetPrefix(); pre = ""; }

        // *** Entity Combo Box Events ***
        string ent = "";
        private void cboEntity_SelectedIndexChanged(object sender, EventArgs e)
        { GetEntity(); }

        private void cboEntity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { GetEntity(); ent = ""; }
        }
        private void cboEntity_KeyPress(object sender, KeyPressEventArgs e)
        { 
            ent += e.KeyChar;
            if (ent == "\r" || ent == "") { ent = ""; goto Done; }
            string SQL = "SELECT * FROM DX WHERE DXCCName LIKE '" + 
                            ent + "%' ORDER BY DXCCName";
            GetDXCC(SQL);
        Done: e.Handled = true; cboEntity.Focus();
        }
        private void cboEntity_Click(object sender, EventArgs e)
        { cboEntity.DroppedDown = true; ent = ""; }

        private void cboEntity_Enter(object sender, EventArgs e)
        { pre = ""; }

        private void cboEntity_Leave(object sender, EventArgs e)
        { GetEntity(); ent = ""; }
        // Country Code lookup has been requested
        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string SQL = "SELECT * FROM DX WHERE CountryCode = " +
                            Convert.ToInt32(txtCode.Text);
                txtCode.Text = "";
                GetDXCC(SQL);
            }
        }
        // IOTA lookup is requested
        private void txtDxIOTA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string SQL = "SELECT * FROM DX WHERE IOTA = '" +
                            txtDxIOTA.Text + "'";
                txtDxIOTA.Text = "";
                GetDXCC(SQL);
            }
        }
        // The distance box was double-clicked, get the reverse distance
        private void txtDxDist_DoubleClick(object sender, EventArgs e)
        {
            txtDxDist.Text = (24902 - Convert.ToInt32(txtDxDist.Text)).ToString();
            if (txtDxDist.ForeColor == Color.Blue) txtDxDist.ForeColor = Color.Firebrick;
            else txtDxDist.ForeColor = Color.Blue;
        }

       #endregion Rotor Events

        #region Rotor Methods

        // Turn the rotor
        private void TurnRotor(string heading)
        {
            switch (rotormod)
            {
                case RotorMod.AlphaSpid:
                    int circle = 360;
                    int bearing = Convert.ToInt32(heading);
                    bearing = bearing + circle;
                    RotorPort.Write("W" + bearing.ToString() + "0\x01     / ");
                    break;
                case RotorMod.GreenHeron:
                    RotorPort.Write("AP1" + heading + ";");
                    break;
                case RotorMod.Hygain:
                    RotorPort.Write("AP1" + heading + ";");
                    RotorPort.Write("AM1;");
                    break;
                case RotorMod.M2R2800PA:
                    RotorPort.Write("S\r");
                    RotorPort.Write(heading + "\r");
                    break;
                case RotorMod.M2R2800PX:
                    RotorPort.Write("A" + heading + "\r");
                    break;
                case RotorMod.Prosistel:
                    RotorPort.Write("\x02\x41\x47" + heading + "\r");
                    break;
                case RotorMod.Yaesu:
                    RotorPort.Write("M" + heading + "\r");
                    break;
                default: break;
            }
        }
        // Get DXCC data from the database
        private void GetDXCC(string SQL)
        {
            OleDbConnection conn = new OleDbConnection(
                "provider = microsoft.jet.oledb.4.0;data source = DDUtil.mdb;");
            OleDbCommand thisCommand = new OleDbCommand(SQL, conn);
            conn.Open();
            OleDbDataReader thisReader = thisCommand.ExecuteReader();
            try
            {
                int i = 1;
                while (thisReader.Read() && i > 0)
                {
                    try
                    {
                        // Write values to text boxes
                        cboPrefix.Text = thisReader.GetValue(0).ToString();
                        txtCode.Text = thisReader.GetValue(1).ToString();
                        cboEntity.Text = thisReader.GetValue(2).ToString();
                        txtRegion.Text = thisReader.GetValue(3).ToString();
                        txtDxCont.Text = thisReader.GetValue(4).ToString();
                        txtDxCQ.Text = thisReader.GetValue(5).ToString();
                        txtDxITU.Text = thisReader.GetValue(6).ToString();
                        txtDxIOTA.Text = thisReader.GetValue(7).ToString();

                        // Calc and display the time at dx station
                        DateTime dt = DateTime.Now;
                        TimeZone localZone = TimeZone.CurrentTimeZone;
                        DateTime currentUTC = localZone.ToUniversalTime(dt);
                        TimeSpan currentOffset = localZone.GetUtcOffset(dt);
                        string off = currentOffset.ToString();
                        off = off.Substring(0, 3);
                        double LocalOffset = Math.Abs(Convert.ToDouble(off));
                        double DxOffset = Math.Abs(Convert.ToDouble(thisReader.GetValue(8).ToString()));
                        double DxOffsetRaw = Convert.ToDouble(thisReader.GetValue(8).ToString());
                        if (Math.Sign(DxOffsetRaw) < 0)
                        {
                            lblDxTime.Text = dt.AddHours(DxOffset +
                                LocalOffset).ToString() + " Local Time";
                            txtDxTime.Text = "UTC+" + DxOffset;
                        }
                        else
                        {
                            lblDxTime.Text = dt.AddHours((DxOffsetRaw - (DxOffsetRaw * 2)) +
                                LocalOffset).ToString() + " Local Time";
                            txtDxTime.Text = "UTC-" + DxOffset;
                        }
                        if (thisReader.GetValue(8).ToString() == "0")
                            txtDxTime.Text = "UTC";

                        // Format and display the dx station's latitude and longitude
                        string lat = thisReader.GetValue(9).ToString();
                        decimal dLat = Convert.ToDecimal(lat);
                        lat = Math.Round(dLat).ToString();
                        if (lat.Substring(0, 1) == "-") txtDxLat.Text = lat.TrimStart('-') + " 0' S";
                        else txtDxLat.Text = lat + " 0' N";

                        string lon = thisReader.GetValue(10).ToString();
                        decimal dLon = Convert.ToDecimal(lon);
                        lon = Math.Round(dLon).ToString();
                        if (lon.Substring(0, 1) == "-") txtDxLong.Text = lon.TrimStart('-') + " 0' W";
                        else txtDxLong.Text = lon + " 0' E";

                        // Calc and display the distance to the dx station
                        txtDxDist.ForeColor = Color.Firebrick;
                        txtDxDist.Text = Dist(Convert.ToDouble(txtLat.Text), 
                            Convert.ToDouble(txtLong.Text),
                            Convert.ToDouble(lat), Convert.ToDouble(lon)).ToString();

                        // Calc the bearing
                        double lat1rad = Convert.ToDouble(txtLat.Text) * Math.PI / 180;
                        double lon1rad = Convert.ToDouble(txtLong.Text) * Math.PI / 180;
                        double lat2rad = Convert.ToDouble(lat) * Math.PI / 180;
                        double lon2rad = Convert.ToDouble(lon) * Math.PI / 180;
                        double bearing = 0.0;

                        if (Math.Sin(lon1rad - lon2rad) < 0)
                            bearing = Math.Acos((Math.Sin(lat2rad) - Math.Sin(lat1rad) *
                                      Math.Cos(distrad)) / (Math.Sin(distrad) * Math.Cos(lat1rad)));
                        else
                            bearing = 2 * Math.PI - Math.Acos((Math.Sin(lat2rad) - Math.Sin(lat1rad) *
                                       Math.Cos(distrad)) / (Math.Sin(distrad) * Math.Cos(lat1rad)));
                        bearing = bearing * (180 / Math.PI);
                        txtSP.Text = Convert.ToInt32(bearing).ToString();
                        if (bearing < 180)
                            txtLP.Text = Convert.ToInt32(bearing + 180).ToString();
                        else
                            txtLP.Text = Convert.ToInt32(bearing - 180).ToString();
                    }
                    catch (Exception e)
                    { MessageBox.Show(e.Message + "\n\n" +
                        "This error is generally due to latitude or\n" +
                        "longitude not being input on the Setup form\n\n" + 
                        "Please correct and try again.", "Data Error"); }
                    i = 0;
                }
            }
            catch (OleDbException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        // Calc DX Station Distance, returns staute miles
        double distrad = 0.0;
        private int Dist(double lat1, double lon1, double lat2, double lon2)
        {
            double lat1rad = lat1 * Math.PI / 180;
            double lon1rad = lon1 * Math.PI / 180;
            double lat2rad = lat2 * Math.PI / 180;
            double lon2rad = lon2 * Math.PI / 180;
            distrad = Math.Acos(Math.Sin(lat1rad)*Math.Sin(lat2rad)+
                Math.Cos(lat1rad)*Math.Cos(lat2rad)*Math.Cos(lon2rad-lon1rad));

            int naut = Convert.ToInt32(distrad *(180*60/Math.PI));
            return Convert.ToInt32(naut * 1.15);
        }
        // Get DXCC info for this Prefix
        private void GetPrefix()
        {
            string SQL = "SELECT DXCCPrefix, CountryCode, DXCCName, Location, " +
                "Continent, CQZone, ITUZone, IOTA, TimeZone, Latitude, Longitude " +
                "FROM DX WHERE DXCCPrefix = '" + cboPrefix.Text + "'";
            GetDXCC(SQL);
//            cboPrefix.Focus();
        }
        // Get DXCC info for this Entity
        private void GetEntity()
        {
            string SQL = "SELECT DXCCPrefix, CountryCode, DXCCName, Location, " +
                "Continent, CQZone, ITUZone, IOTA, TimeZone, Latitude, Longitude " +
                "FROM DX WHERE DXCCName = '" + cboEntity.Text + "'";
            GetDXCC(SQL);
//            cboEntity.Focus();
        }
        #endregion Rotor Methods

        #region Rotor Setup

        //  Initialize Rotor settings and controls (called from Setup())
        private void InitRotor()
        {   //Load DX Prefix and Entity controls from Database
            OleDbConnection conn = new OleDbConnection(
             "provider = microsoft.jet.oledb.4.0;data source = DDUtil.mdb;");
            OleDbCommand thisCommand = new OleDbCommand(
                "SELECT DISTINCT DXCCPrefix, DXCCName FROM DX " + 
                "GROUP BY DXCCPrefix, DXCCName, TimeZone " +
                "HAVING TimeZone Is Not Null", conn);

            conn.Open();
            OleDbDataReader thisReader = thisCommand.ExecuteReader();
            cboPrefix.Items.Add("");
            cboEntity.Items.Add("");
            while (thisReader.Read())
            {
                cboPrefix.Items.Add(thisReader.GetValue(0).ToString());
                cboEntity.Items.Add(thisReader.GetValue(1).ToString());
                cboEntity.Sorted = true;
            }
                conn.Close();
            // Load saved settings
            txtLat.Text = set.Latitude;
            txtLong.Text = set.Longitude;
            txtGrid.Text = set.Grid;
            cboRotorPort.SelectedIndex = set.RotorPort;
            cboRotorCom.SelectedIndex = set.RotorCom;
            chkRotorEnab.Checked = set.RotorEnab;
            switch (set.RotorSpeed)
            {
                case 0: rbRtrSpd1.Checked = true; break;
                case 1: rbRtrSpd2.Checked = true; break;
                case 2: rbRtrSpd3.Checked = true; break;
                default: break;
            }
            switch (set.rotorModel)
            {
                case 0: rbRtrMod1.Checked = true; break;  // Alpha Spid
                case 1: rbRtrMod2.Checked = true; break;  // Green Heron
                case 2: rbRtrMod3.Checked = true; break;  // HyGain
                case 3: rbRtrMod4.Checked = true;         // M2RC2900A-P
                    RotorPort.Write("S" + rtrSpd + "\r");
                    break;
                case 4: rbRtrMod5.Checked = true;         // M2RC2900AX
                    RotorPort.Write("S" + rtrSpd + "\r");
                    break;
                case 5: rbRtrMod6.Checked = true;         // Prosistel
                    RotorPort.Write("\x02\x41WENA_PWM=1\r");
                    break;
                case 6: rbRtrMod7.Checked = true;         // Yaesu
                    RotorPort.Write("X" + rtrSpd + "\r");
                    break;
                default: break;
            }
        }

        #endregion Rotor Setup

        #endregion Rotor Control

        #region Serial Port Events

        /// <summary>
        /// Radio CAT data has arrived
        /// </summary>
        string CommBuffer = "";
        string rawFreq = "";
        string sdrMode = "";
        string xOn = "";            // 1 = xmit on, 0 = xmit off
        string lastFreq = "";       // freq from last CATRxEvent
        void sp_CATRxEvent(object source, CATSerialPorts.SerialRXEvent e)
        {
            try
            {   // put the port data in the comm buffer
                CommBuffer += AE.GetString(e.buffer, 0, e.buffer.Length);
                Regex rex = new Regex(".*?;");  //accept any string ending in ";"		
                //loop thru the buffer and find matches
                for (Match m = rex.Match(CommBuffer); m.Success; m = m.NextMatch())
                {
                    OutBuffer = m.Value;
                    // remove the match from the buffer if found
                    CommBuffer = CommBuffer.Replace(m.Value, "");
                    string title = "DDUtil " + ver;
                    string id = "";
                    string logFreq = "";
                    string regex = "(?<cat>\\w{2})(?<mz>\\d{5})(?<kz>\\d{3})(?<hz>\\d{2})";
                    string mask = "${mz},${kz}.${hz}";
                    string freq = "";
                    string freqLook = "";
                    string mode = "";
                    rawFreq = OutBuffer;
                    OutBuffer = "";
                    if (rawFreq.Length > 4 && rawFreq.Substring(0, 4) == "ZZTS")
                    {   // this is a Temp cmd, write it to the temp window
                        temp = Convert.ToDouble(rawFreq.Substring(4, 5));
                        WriteTemp();
                        return;
                    }
                    // send radio's CAT reply back to RCP1
                    if (logFlag == true && LogPort.IsOpen) LogPort.Write(rawFreq);
                    // send CAT reply back to RCPn port
                    if (chkRCP2.Checked)    // RCP2 Enabled
                    {
                        if (!chkRCP2IF.Checked) // if unchecked send any command
                            RCP2port.Write(rawFreq);
                        else
                        {   // if checked only send command if 'IF'
                            if (rawFreq.Length > 4 && rawFreq.Substring(0, 4) == "IF00")
                                RCP2port.Write(rawFreq);
                        }
                    }
                    if (chkRCP3.Checked)
                    {
                        if (!chkRCP3IF.Checked) // if unchecked send any command
                            RCP3port.Write(rawFreq);
                        else
                        {   // if checked only send command if 'IF'
                            if (rawFreq.Length > 4 && rawFreq.Substring(0, 4) == "IF00")
                                RCP3port.Write(rawFreq);
                        }
                    }
                    if (chkRCP4.Checked)
                    {
                        if (!chkRCP4IF.Checked) // if unchecked send any command
                            RCP4port.Write(rawFreq);
                        else
                        {   // if checked only send command if 'IF'
                            if (rawFreq.Length > 4 && rawFreq.Substring(0, 4) == "IF00")
                                RCP4port.Write(rawFreq);
                        }
                    }
                    // start checking for specific cat responses
                    if (rawFreq.Length > 4 && rawFreq.Substring(0, 4) == "IF00")
                    {   // DDUtil or RCP IF; query
                        xOn = rawFreq.Substring(rawFreq.Length - 10, 1);
                        sdrMode = rawFreq.Substring(rawFreq.Length - 9, 1);
                        if (rawFreq.Substring(rawFreq.Length - 6, 1) == "0")
                        {   // if xmit vfo is "A" then send passive listener commands now
                            vfo = "VFO A";
                            goto SendData;
                        }
                        else // if not set vfo variable to "B"
                        {     // and wait for next FB command to arrive
                            vfo = "VFO B";
                            return;
                        }
                    }
                    else if (rawFreq.Length > 4 && rawFreq.Substring(0, 4) == "FB00" && vfo == "VFO B")
                    {   // this is the right command for the B vfo data
                        goto SendData;
                    }
                    else if (rawFreq.Length == 3 && rawFreq.Substring(0, 2) == "MD")
                    {   // this is a mode cmd, save it
                        sdrMode = rawFreq.Substring(2, 1);
                        return;
                    }
                    else return;
                SendData:
                    switch (sdrMode)
                    {   // Lookup the correct SDR mode
                        case "1": mode = "LSB"; break;
                        case "2": mode = "USB"; break;
                        case "3": mode = "CWU"; break;
                        case "4": mode = "FMN"; break;
                        case "5": mode = "AM"; break;
                        case "6": mode = "DIGL"; break;
                        case "7": mode = "CWL"; break;
                        case "9": mode = "DIGU"; break;
                        default: mode = "xxx"; break;
                    }
                    logFreq = rawFreq.Substring(2, 11);
                    PortSend(logFreq);  // send freq. to PLs
                    // If SteppIR enabled check for activity
                    if (chkStep.Checked)
                    {   // see if freq has changed
                        if (String.Compare(lastFreq, logFreq) != 0)
                        {   // yes go poll SteppIR controller
                            StepData.Write("?A\r");
                            StepTimer.Enabled = true;
                            StepCtr = reps; // counter to allow for delay
                        }   // see StepData_DataReceived() for return
                    }
                    lastFreq = logFreq; // save this freq
                    freq = Regex.Replace(rawFreq, regex, mask);
                    freq = freq.TrimStart('0');
                    freqLook = rawFreq.Substring(2, 8);
                    freqLook = freqLook.TrimStart('0');
                    freqLook = freqLook.Substring(0, freqLook.Length - 2);
                    if (logFlag == true) // RCP1 is active
                        id = title + " - " + freq.Substring(0, 9) + "  " + vfo + "  " + mode + "  RCP";
                    else
                        id = title + " - " + freq.Substring(0, 9) + "  " + vfo + "  " + mode;
                    LookUp(freqLook);   //decode freq data and output to LPT port
                    this.SetTitle(id);
                    this.SetDigit(keyValue.ToString());
                }//For
            }//Try
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
        }//CATRxEvent

        // The RCP1 CAT port has received data
        string sBuf1 = "";
        protected void OnReceive(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                logFlag = true;     // the logger is running
                if (!tempFlag)
                {   // set the tempTimer interval to a default of 1000
                    tempTimer.Interval = 1000;
                    tempFlag = true;
                }
                pollTimer.Enabled = false;
                string sCmd = "";
                SerialPort port = (SerialPort)sender;
                byte[] data = new byte[port.BytesToRead];
                port.Read(data, 0, data.Length);
                sBuf1 += AE.GetString(data, 0, data.Length);
                Regex rex = new Regex(".*?;");			//accept any string ending in ;		
                for (Match m = rex.Match(sBuf1); m.Success; m = m.NextMatch())
                {   //loop thru the buffer and find matches
                    sCmd = m.Value;
                    sBuf1 = sBuf1.Replace(m.Value, ""); //remove the match from the buffer
                    WriteToPort(sCmd, iSleep);
                }
                logTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
        }
        // RCP1 Rotor port has received data
        string sRtr1Buf = "";
        private void RCP1Rotor_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (chkRotorEnab.Checked)    // Rotor must be enabled
            {
                try
                {
                    string sCmd = "";
                    SerialPort port = (SerialPort)sender;
                    byte[] data = new byte[port.BytesToRead];
                    port.Read(data, 0, data.Length);
                    sRtr1Buf += AE.GetString(data, 0, data.Length);
                    Regex rex = new Regex(".*?" + suffix);
                    for (Match m = rex.Match(sRtr1Buf); m.Success; m = m.NextMatch())
                    {   //loop thru the buffer and find matches
                        sCmd = m.Value;
                        sRtr1Buf = sRtr1Buf.Replace(m.Value, "");//remove the match from the buffer
                        RotorPort.Write(sCmd);
                        Thread.Sleep(50);
                    }
                }
                catch (Exception ex)
                {
                    bool bReturnLog = false;
                    bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                    if (false == bReturnLog) MessageBox.Show("Unable to write to log");
                }
            }
        }
        // RCP2 Rotor port has received data
        string sRtr2Buf = "";
        private void RCP2Rotor_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (chkRotorEnab.Checked)    // Rotor must be enabled
            {
                try
                {
                    string sCmd = "";
                    SerialPort port = (SerialPort)sender;
                    byte[] data = new byte[port.BytesToRead];
                    port.Read(data, 0, data.Length);
                    sRtr2Buf += AE.GetString(data, 0, data.Length);
                    Regex rex = new Regex(".*?" + suffix);
                    for (Match m = rex.Match(sRtr2Buf); m.Success; m = m.NextMatch())
                    {   //loop thru the buffer and find matches
                        sCmd = m.Value;
                        sRtr2Buf = sRtr2Buf.Replace(m.Value, "");//remove the match from the buffer
                        RotorPort.Write(sCmd);
                        Thread.Sleep(50);
                    }
                }
                catch (Exception ex)
                {
                    bool bReturnLog = false;
                    bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                    if (false == bReturnLog) MessageBox.Show("Unable to write to log");
                }
            }
        }
        // RCP3 Rotor port has received data
        string sRtr3Buf = "";
        private void RCP3Rotor_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (chkRotorEnab.Checked)    // Rotor must be enabled
            {
                try
                {
                    string sCmd = "";
                    SerialPort port = (SerialPort)sender;
                    byte[] data = new byte[port.BytesToRead];
                    port.Read(data, 0, data.Length);
                    sRtr3Buf += AE.GetString(data, 0, data.Length);
                    Regex rex = new Regex(".*?" + suffix);
                    for (Match m = rex.Match(sRtr3Buf); m.Success; m = m.NextMatch())
                    {   //loop thru the buffer and find matches
                        sCmd = m.Value;
                        sRtr3Buf = sRtr3Buf.Replace(m.Value, "");//remove the match from the buffer
                        RotorPort.Write(sCmd);
                        Thread.Sleep(50);
                    }
                }
                catch (Exception ex)
                {
                    bool bReturnLog = false;
                    bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                    if (false == bReturnLog) MessageBox.Show("Unable to write to log");
                }
            }
        }
        // RCP4 Rotor port has received data
        string sRtr4Buf = "";
        private void RCP4Rotor_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (chkRotorEnab.Checked)    // Rotor must be enabled
            {
                try
                {
                    string sCmd = "";
                    SerialPort port = (SerialPort)sender;
                    byte[] data = new byte[port.BytesToRead];
                    port.Read(data, 0, data.Length);
                    sRtr4Buf += AE.GetString(data, 0, data.Length);
                    Regex rex = new Regex(".*?" + suffix);
                    for (Match m = rex.Match(sRtr4Buf); m.Success; m = m.NextMatch())
                    {   //loop thru the buffer and find matches
                        sCmd = m.Value;
                        sRtr4Buf = sRtr4Buf.Replace(m.Value, "");//remove the match from the buffer
                        RotorPort.Write(sCmd);
                        Thread.Sleep(50);
                    }
                }
                catch (Exception ex)
                {
                    bool bReturnLog = false;
                    bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                    if (false == bReturnLog) MessageBox.Show("Unable to write to log");
                }
            }
        }
        // RCP2 CAT port has received
        string sBuf2 = "";
        private void RCP2port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (chkRCP2.Checked)    // port must be enabled
            {
                try
                {
                    string sCmd = "";
                    SerialPort port = (SerialPort)sender;
                    byte[] data = new byte[port.BytesToRead];
                    port.Read(data, 0, data.Length);
                    sBuf2 += AE.GetString(data, 0, data.Length);
                    Regex rex = new Regex(".*?;");				//accept any string ending in ;		
                    for (Match m = rex.Match(sBuf2); m.Success; m = m.NextMatch())
                    {   //loop thru the buffer and find matches
                        sCmd = m.Value;
                        sBuf2 = sBuf2.Replace(m.Value, "");       //remove the match from the buffer
                        if (chkRCP2DisPol.Checked && sCmd.Length <= 3)
                            break;
                        WriteToPort(sCmd, iSleep);
                    }
                }
                catch (Exception ex)
                {
                    bool bReturnLog = false;
                    bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                    if (false == bReturnLog) MessageBox.Show("Unable to write to log");
                }
            }
        }
        // RCP3 CAT port has received data
        string sBuf3 = "";
        private void RCP3port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (chkRCP3.Checked)    // port must be enabled
            {
                try
                {
                    string sCmd = "";
                    SerialPort port = (SerialPort)sender;
                    byte[] data = new byte[port.BytesToRead];
                    port.Read(data, 0, data.Length);
                    sBuf3 += AE.GetString(data, 0, data.Length);
                    Regex rex = new Regex(".*?;");				//accept any string ending in ;		
                    for (Match m = rex.Match(sBuf3); m.Success; m = m.NextMatch())
                    {   //loop thru the buffer and find matches
                        sCmd = m.Value;
                        sBuf3 = sBuf3.Replace(m.Value, "");       //remove the match from the buffer
                        if (chkRCP3DisPol.Checked && sCmd.Length <= 3)
                            break;
                        WriteToPort(sCmd, iSleep);
                    }
                }
                catch (Exception ex)
                {
                    bool bReturnLog = false;
                    bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                    if (false == bReturnLog) MessageBox.Show("Unable to write to log");
                }
            }
        }
        // RCP4 CAT port has received data
        string sBuf4 = "";
        private void RCP4port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (chkRCP4.Checked)    // port must be enabled
            {
                try
                {
                    string sCmd = "";
                    SerialPort port = (SerialPort)sender;
                    byte[] data = new byte[port.BytesToRead];
                    port.Read(data, 0, data.Length);
                    sBuf4 += AE.GetString(data, 0, data.Length);
                    Regex rex = new Regex(".*?;");				//accept any string ending in ;		
                    for (Match m = rex.Match(sBuf4); m.Success; m = m.NextMatch())
                    {   //loop thru the buffer and find matches
                        sCmd = m.Value;
                        sBuf4 = sBuf4.Replace(m.Value, "");       //remove the match from the buffer
                        if (chkRCP2DisPol.Checked && sCmd.Length <= 3)
                            break;
                        WriteToPort(sCmd, iSleep);
                    }
                }
                catch (Exception ex)
                {
                    bool bReturnLog = false;
                    bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                    if (false == bReturnLog) MessageBox.Show("Unable to write to log");
                }
            }
        }
        // LP100 port has received data
        private void LPport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (chkLPenab.Checked)
            {
                SerialPort LPportMsgs = (SerialPort)sender;
                string LPportMsg = LPportMsgs.ReadExisting();
                if (LPportMsg.Length == 43)
                {
                    string fwd = LPportMsg.Substring(1, 7);
                    string swr = LPportMsg.Substring(LPportMsg.Length - 4, 4);
                    SetFwd(fwd);
                    SetSwr(swr);
                }
            }
        }
        // PW1 port has received data (Query from IC-PW1) i.e. FE FE 33 54 [03/04] FD
        // Reply messages are hard coded as it can only be a request for freq. or mode
        private void PW1port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (chkPW1.Checked)                 // If function is enabled proceed
            {
                try
                {
                    int len = PW1port.BytesToRead;  // How many bytes in port buffer
                    if (len < 6) return;            // ? is a valid command
                    byte[] inBuf = new byte[len];   // input message buffer
                    byte[] outBuf = new byte[11];   // output message buffer
                    string x = txtPW1ta.Text;       // get DDUtil address
                    string cn = "";                 // save command from caller
                    string ra = "";                 // save caller address
                    string ta = txtPW1ta.Text;      // get DDUtil address

                    // read the port message buffer; may be more than one command sequence
                    // so for loop may could be needed
                    PW1port.Read(inBuf, 0, len);
                    for (int i = 0; i <= len; i++)
                    {   // look for FE FE 33 sequence indicating message start.
                        if (inBuf[i].ToString("x2").ToUpper() == "FE")              // ? 1st preamble
                        {
                            i++; if (i == 0) return;
                            if (inBuf[i].ToString("x2").ToUpper() == "FE")          // ? 2nd preamble
                            {
                                i++; // increment inBuf position counter
                                if (inBuf[i].ToString("x2").ToUpper() == x)         // ? DDUtil addr (33h)
                                {
                                    i++; // increment inBuf position counter
                                    ra = inBuf[i].ToString("x2").ToUpper();         // save caller addr
                                    i++;
                                    cn = inBuf[i].ToString("x2").ToUpper();         // save command
                                    i++; // increment inBuf position counter
                                    if (inBuf[i].ToString("x2").ToUpper() == "FD")  // ? message end
                                    {
                                        i++; // increment inBuf position counter
                                        if (cn == "03")     // is cmd to read Xcvr Frequency
                                        {   // If yes, assemble reply and send to port
                                            raSetText(ra);
                                            string preamble = "FE";
                                            string EOM = "FD";
                                            string mystring = "";
                                            mystring = EOM + LastFreq.Substring(1, 10) +
                                                        cn + ta + ra + preamble + preamble;
                                            int j = 20;
                                            for (int k = 0; k < 11; k++)
                                            {
                                                string outtemp = mystring.Substring(j, 2);
                                                outBuf[k] = byte.Parse(outtemp, NumberStyles.HexNumber);
                                                j -= 2;
                                            }
                                            // send freq read reply for 
                                            // 14.234.56 Mhz = [FE FE ra ta cn 60 45 23 14 00 FD]
                                            PW1port.Write(outBuf, 0, 11);
                                        }
                                        else if (cn == "04")    // is cmd to read Xcvr Mode
                                        {   // If yes, assemble reply and send to port
                                            raSetText(ra);
                                            string preamble = "FE";
                                            string EOM = "FD";
                                            string mystring = "";
                                            string mode = "";
                                            switch (sdrMode)
                                            {   // Lookup PW1 equivalent mode for SDR mode
                                                case "1": mode = "00"; break;   // LSB
                                                case "2": mode = "01"; break;   // USB
                                                case "3": mode = "03"; break;   // CWU
                                                case "4": mode = "05"; break;   // FMN
                                                case "5": mode = "02"; break;   // AM
                                                case "6": mode = "04"; break;   // RTTY (DIGL)
                                                case "7": mode = "07"; break;   // CWL
                                                case "9": mode = "08"; break;   // RTTY-R (DIGU)
                                                 default: mode = "01"; break;   // USB
                                            }
                                            mystring = EOM + mode + "00" + cn + ta + ra + preamble + preamble;
                                            int j = 14;
                                            for (int k = 0; k < 8; k++)
                                            {
                                                string outtemp = mystring.Substring(j, 2);
                                                outBuf[k] = byte.Parse(outtemp, NumberStyles.HexNumber);
                                                j -= 2;
                                            }
                                            // Send mode command [FE FE ra ta cn 00 md FD]
                                            PW1port.Write(outBuf, 0, 8);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    bool bReturnLog = false;
                    bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                    if (false == bReturnLog) MessageBox.Show("Unable to write to log");
                }
            }
        }

        #endregion Serial Port Events

        #region Serial Port Methods

        /// <summary>
        /// send radio specific frequency data to the Passive Listener ports
        /// </summary>
        private void PortSend(string freq)
        {
            try
            {   // If SteppIR is selected and the freq. has changed
                // send new freq data to it x reps 
                if (chkStep.Checked && StepCtr != 0)
                {
                    if (bFwd) StepPortMsg(freq, "00", "31");
                    else if (b180) StepPortMsg(freq, "40", "31");
                    else if (bBiDir) StepPortMsg(freq, "80", "31");
                    else if (b34) StepPortMsg(freq, "20", "31");
                }
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
            LastFreq = freq;
            // If enabled send freq data to IC-PW1 port
            if (chkPW1.Checked && PW1port.IsOpen && !chkDisBcast.Checked)
            {
                try
                {
                    byte[] bytes = new byte[11];
                    string preamble = "FE";
                    string ta = txtPW1ta.Text;
                    string ra = "00";
                    string cn = "00";
                    string EOM = "FD";
                    string mystring = EOM + freq.Substring(1, 10) + cn + ta + ra + preamble + preamble;
                    int j = 20;
                    for (int i = 0; i < 11; i++)
                    {
                        string temp = mystring.Substring(j, 2);
                        bytes[i] = byte.Parse(temp, NumberStyles.HexNumber);
                        j -= 2;
                    }
                    // 14.234.56 Mhz = FE FE 00 nn 00 60 45 23 14 00 FD
                    PW1port.Write(bytes, 0, 11);
                }
                catch (Exception ex)
                {
                    bool bReturnLog = false;
                    bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                    if (false == bReturnLog) MessageBox.Show("Unable to write to log");
                }
            }
            if (AccPort.IsOpen)
            {
                byte[] bytes = new byte[11];
                string xfreq = freq;
                string mystring = "";
                string mode = "";
                int j;
                switch (portmode)
                {   // for a list of the radio types referenced below see 
                    // toolStripMenuItem4_Click() event handeler
                    case PortMode.None:
                        AccPort.WriteLine("IF" + freq + ";");
                        // 14.234.56 Mhz = IF00014234560;
                        break;
                    case PortMode.Kenwood:
                        if (lastFreq != freq)
                        {
                            AccPort.WriteLine("FA" + freq.Substring(3, 8) + ";");
                            // 14.234.56 Mhz = FA14234560;
                        }
                        if (LastMode != sdrMode && chkMode.Checked)
                        {
                            Thread.Sleep(50);
                            LastMode = sdrMode;
                            AccPort.WriteLine("MD" + sdrMode + ";");
                        }
                        break;
                    case PortMode.YaesuTypeI:
                        if (lastFreq != freq)
                        {
                            mystring = "0A0" + freq.Substring(3, 7);
                            j = 8;
                            for (int i = 0; i < 5; i++)
                            {
                                string temp = mystring.Substring(j, 2);
                                bytes[i] = byte.Parse(temp, NumberStyles.HexNumber);
                                j -= 2;
                            }
                            AccPort.Write(bytes, 0, 5);
                            // 14.234.56 Mhz = 56 34 42 01 0A
                        }
                        if (LastMode != sdrMode && chkMode.Checked)
                        {
                            Thread.Sleep(50);
                            LastMode = sdrMode;
                            switch (sdrMode)
                            {   // Lookup Yaesu Type I equivalent mode
                                case "1": mode = "00"; break;   // LSB
                                case "2": mode = "01"; break;   // USB
                                case "3": mode = "02"; break;   // CWU
                                case "4": mode = "06"; break;   // FMN
                                case "5": mode = "04"; break;   // AM
                                case "6": mode = "08"; break;   // RTTY (DIGL)
                                case "7": mode = "03"; break;   // CWL
                                case "9": mode = "09"; break;   // RTTY-R (DIGU)
                                default: mode = "01"; break;   // USB
                            }
                            mystring = "0C" + mode + "000000";
                            j = 8;
                            for (int i = 0; i < 5; i++)
                            {
                                string temp = mystring.Substring(j, 2);
                                bytes[i] = byte.Parse(temp, NumberStyles.HexNumber);
                                j -= 2;
                            }
                            AccPort.Write(bytes, 0, 5);
                            // send mode  USB = 00 00 00 01 0C
                        }
                        break;
                    case PortMode.YaesuTypeII:
                        if (lastFreq != freq)
                        {
                            mystring = "0" + freq.Substring(3, 7) + "01";
                            j = 0;
                            for (int i = 0; i < 5; i++)
                            {
                                string temp = mystring.Substring(j, 2);
                                bytes[i] = byte.Parse(temp, NumberStyles.HexNumber);
                                j += 2;
                            }
                            AccPort.Write(bytes, 0, 5);
                            // 14.234.56 Mhz = 01 42 34 56 01
                        }
                        if (LastMode != sdrMode && chkMode.Checked)
                        {
                            Thread.Sleep(50);
                            LastMode = sdrMode;
                            switch (sdrMode)
                            {   // Lookup Yaesu Type II equivalent mode
                                case "1": mode = "00"; break;   // LSB
                                case "2": mode = "01"; break;   // USB
                                case "3": mode = "02"; break;   // CWU
                                case "4": mode = "08"; break;   // FMN
                                case "5": mode = "04"; break;   // AM
                                case "6": mode = "0A"; break;   // RTTY (DIGL)
                                case "7": mode = "03"; break;   // CWL
                                case "9": mode = "0A"; break;   // RTTY-R (DIGU)
                                default: mode = "01"; break;   // USB
                            }
                            mystring = mode + "00000007";
                            j = 0;
                            for (int i = 0; i < 5; i++)
                            {
                                string temp = mystring.Substring(j, 2);
                                bytes[i] = byte.Parse(temp, NumberStyles.HexNumber);
                                j += 2;
                            }
                            AccPort.Write(bytes, 0, 5);
                        }
                        break;
                    case PortMode.Icom:
                        string preamble = "FE";
                        string radNum = txtRadNum.Text;
                        string EOM = "FD";
                        string ctrlAddr = "E0";
                        if (lastFreq != freq)
                        {
                            mystring = EOM + "00" + freq.Substring(3, 8) + "05" +
                                       ctrlAddr + radNum + preamble + preamble;
                            j = 20;
                            for (int i = 0; i < 11; i++)
                            {
                                string temp = mystring.Substring(j, 2);
                                bytes[i] = byte.Parse(temp, NumberStyles.HexNumber);
                                j -= 2;
                            }
                            // send freq 14.234.56 Mhz = FE FE 1E E0 05 60 45 23 14 00 FD
                            AccPort.Write(bytes, 0, 11);
                        }
                        // send mode info
                        if (LastMode != sdrMode && chkMode.Checked)
                        {
                            Thread.Sleep(50);
                            LastMode = sdrMode;
                            byte[] outBuf = new byte[8];
                            string cn = "06";
                            switch (sdrMode)
                            {   // Lookup ICOM equivalent mode
                                case "1": mode = "00"; break;   // LSB
                                case "2": mode = "01"; break;   // USB
                                case "3": mode = "03"; break;   // CWU
                                case "4": mode = "05"; break;   // FMN
                                case "5": mode = "02"; break;   // AM
                                case "6": mode = "04"; break;   // RTTY (DIGL)
                                case "7": mode = "07"; break;   // CWL
                                case "9": mode = "08"; break;   // RTTY-R (DIGU)
                                default: mode = "01"; break;   // USB
                            }
                            mystring = EOM + "00" + mode + cn + ctrlAddr +
                                       radNum + preamble + preamble;
                            j = 14;
                            for (int k = 0; k < 8; k++)
                            {
                                string outtemp = mystring.Substring(j, 2);
                                outBuf[k] = byte.Parse(outtemp, NumberStyles.HexNumber);
                                j -= 2;
                            }
                            // Send mode command [FE FE ra ta cn md pd FD]
                            AccPort.Write(outBuf, 0, 8);
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Creates a new serial port and rx data available event
        /// </summary>
        private void CreateSerialPort()
        {
            sp = new CATSerialPorts.CATSerialPort();
            sp.CATRxEvent += new CATSerialPorts.CATSerialPort.CATSerialRXEventHandler(sp_CATRxEvent);
            sp.WriteTimeout = 500;
            sp.ReadTimeout = 500;
        }
        /// <summary>
        /// Loads the list of available serial ports in the serial port combobox.
        /// </summary>
        private void GetPortNames()
        {
            ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                cboCAT.Items.Add(port);
                cboSerAcc.Items.Add(port);
                cboLogPort.Items.Add(port);
                cboLPport.Items.Add(port);
                cboRCP2.Items.Add(port);
                cboRCP3.Items.Add(port);
                cboRCP4.Items.Add(port);
                cboPW1.Items.Add(port);
                cboStep.Items.Add(port);
                cboRotorPort.Items.Add(port);
                cboRCP1Rotor.Items.Add(port);
                cboRCP2Rotor.Items.Add(port);
                cboRCP3Rotor.Items.Add(port);
                cboRCP4Rotor.Items.Add(port);
            }
            cboCAT.Sorted = true;
            cboSerAcc.Items.Add("");
            cboSerAcc.Sorted = true;
            cboLogPort.Items.Add("");
            cboLogPort.Sorted = true;
            cboLPport.Items.Add("");
            cboLPport.Sorted = true;
            cboRCP2.Items.Add("");
            cboRCP2.Sorted = true;
            cboRCP3.Items.Add("");
            cboRCP3.Sorted = true;
            cboRCP4.Items.Add("");
            cboRCP4.Sorted = true;
            cboPW1.Items.Add("");
            cboPW1.Sorted = true;
            cboStep.Items.Add("");
            cboStep.Sorted = true;
            cboRotorPort.Items.Add("");
            cboRotorPort.Sorted = true; 
            cboRCP1Rotor.Items.Add("");
            cboRCP1Rotor.Sorted = true;
            cboRCP2Rotor.Items.Add("");
            cboRCP2Rotor.Sorted = true;
            cboRCP3Rotor.Items.Add("");
            cboRCP3Rotor.Sorted = true;
            cboRCP4Rotor.Items.Add("");
            cboRCP4Rotor.Sorted = true;
        }
        /// <summary>
        /// Opens the default CAT port.
        /// </summary>
        private void SetDefaultComRadio()
        {
            int n = cboCAT.Items.IndexOf(DefaultComRadio);
            if (n >= 0)
            {
                cboCAT.SelectedIndex = n;
                OpenSerialPort();
            }
            else
            {
                MessageBox.Show("Default Radio Port is not valid.  Please select a port from the list.");
                cboCAT.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Opens the default Accessory (passive listener) port.
        /// </summary>
        private void SetDefaultComAcc()
        {
            int n = cboSerAcc.Items.IndexOf(DefaultComAcc);
            if (n >= 0)
            {
                cboSerAcc.SelectedIndex = n;
                AccPort.Open();
            }
            else
            {
                MessageBox.Show("Default Accessory Port is not valid.  Please select a port from the list.");
                cboSerAcc.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Opens the default LP100 port.
        /// </summary>
        private void SetDefaultLPport()
        {
            int n = cboLPport.Items.IndexOf(DefaultLPport);
            if (n >= 0)
            {
                cboLPport.SelectedIndex = n;
                LPport.Open();
            }
            else
            {
                MessageBox.Show("Default LP100 Port is not valid.  Please select a port from the list.");
                cboLPport.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Opens the default SteppIR port.
        /// </summary>
        private void SetDefaultStepPort()
        {
            int n = cboStep.Items.IndexOf(DefaultStepPort);
            if (n >= 0)
            {
                cboStep.SelectedIndex = n;
                StepData.Open();
            }
            else
            {
                MessageBox.Show("Default SteppIR Port is not valid.  Please select a port from the list.");
                cboStep.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Opens the default RCP2 (Radio control) port.
        /// </summary>
        private void SetDefaultRCP2port()
        {
            int n = cboRCP2.Items.IndexOf(DefaultRCP2port);
            if (n >= 0)
            {
                cboRCP2.SelectedIndex = n;
                RCP2port.Open();
            }
            else
            {
                MessageBox.Show("Default RCP2 Radio Control Port is not valid." +
                    "Please select a port from the list.");
                cboLogPort.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Opens the default RCP3 (Radio control) port.
        /// </summary>
        private void SetDefaultRCP3port()
        {
            int n = cboRCP3.Items.IndexOf(DefaultRCP3port);
            if (n >= 0)
            {
                cboRCP3.SelectedIndex = n;
                RCP3port.Open();
            }
            else
            {
                MessageBox.Show("Default RCP3 Radio Control Port is not valid." +
                                "Please select a port from the list.");
                cboLogPort.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Opens the default RCP4 (Radio control) port.
        /// </summary>
        private void SetDefaultRCP4port()
        {
            int n = cboRCP4.Items.IndexOf(DefaultRCP4port);
            if (n >= 0)
            {
                cboRCP4.SelectedIndex = n;
                RCP4port.Open();
            }
            else
            {
                MessageBox.Show("Default RCP4 Radio Control Port is not valid." +
                                "Please select a port from the list.");
                cboLogPort.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Opens the default IC-PW1 port.
        /// </summary>
        private void SetDefaultPW1port()
        {
            int n = cboPW1.Items.IndexOf(DefaultPW1port);
            if (n >= 0)
            {
                cboPW1.SelectedIndex = n;
                PW1port.Open();
            }
            else
            {
                MessageBox.Show("Default PW1 Radio Control Port is not valid." +
                    "Please select a port from the list.");
                cboLogPort.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Opens the default Radio control port.
        /// </summary>
        private void SetDefaultLogPort()
        {
            int n = cboLogPort.Items.IndexOf(DefaultLogPort);
            if (n >= 0)
            {
                cboLogPort.SelectedIndex = n;
                LogPort.Open();
            }
            else
            {
                MessageBox.Show("Default Radio Control Port is not valid." +
                    "Please select a port from the list.");
                cboLogPort.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Opens the CAT port name stored in the DefaultComRadio property.
        /// </summary>
        private void OpenSerialPort()
        {
            if (sp.isOpen)
                sp.Close();
            try
            {
                sp.Name = DefaultComRadio;
                sp.Open();
            }
            catch
            {
                // give a message, if the port is not available:
                MessageBox.Show("The Radio CAT serial port " + sp.Name +
                   " cannot be opened!", "Port Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSerAcc.SelectedText = "";
            }
        }
        /// <summary>
        /// Writes the current CAT command to the radio serial port.
        /// The keep-alive timer is disabled and restarted for each
        /// write.  The sleep period gives the radio CAT parser time
        /// to react and return an answer.'
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sleep"></param>
        private void WriteToPort(string cmd, int sleep)
        {
            try
            {
   //             pollTimer.Enabled = false;
                sp.Write(cmd);
                Thread.Sleep(sleep);
   //             pollTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                bool bReturnLog = false;
                bReturnLog = ErrorLog.ErrorRoutine(false, enableErrorLog, ex);
                if (false == bReturnLog) MessageBox.Show("Unable to write to log");
            }
        }
        #endregion Serial Port Methods

        #region SteppIR

        // The Home SteppIR button was pressed
//        public static string notiMsg;
        private void btnHome_Click(object sender, EventArgs e)
        {
            if (chkStep.Checked)
            {
                StepData.Write("@A\0\0\0\0\0\0S0\r"); // Home the antenna
                chkStep.Checked = false;
                rbFwd.Checked = true;
                x = this.Left;
                y = this.Top;
                Notification.notiMsg = 
                    "\rThe SteppIR Antenna elements are retracting.\r\r" +
                    "The SteppIR will NO longer follow PowerSDR.\r\r" +
                    "See the 'Other' tab to Re-Enable operation";
                Notification alert = new Notification();
                alert.Show();
            }
        }
        // The Calibrate SteppIR button was pressed
        bool bCal = false;
        private void btnCalib_Click(object sender, EventArgs e)
        {
            StepData.Write("@A\0\0\0\0\0\0V0\r");
            Thread.Sleep(100);
            StepData.Write("?A\r");
            StepTimer.Enabled = true;
            StepCtr = reps; // counter to allow for delay
            bCal = true;
            x = this.Left;
            y = this.Top;
            Notification.notiMsg =
                "\r\rPlease Standby the antenna is Calibrating\r\r\r" +
                "This may take a few moments!";
            Notification alert = new Notification();
            alert.Show();
        }
        // Send port data to SteppIR controller
        private void StepPortMsg(string freq, string dir, string cmd)
        {
            string preamble = "404100";
            string postamble = "300D";
            byte[] bytes = new byte[11];
            string decimalNumber = freq.Substring(3, 7);
            int number = int.Parse(decimalNumber);
            string hex = number.ToString("x6");
            string strOut = preamble + hex + "00" + dir + cmd + postamble;
            int j = 0;
            for (int i = 0; i < 11; i++)
            {
                string x = strOut.Substring(j, 2);
                bytes[i] = byte.Parse(x, NumberStyles.HexNumber);
                j += 2;
            }
            StepData.Write(bytes, 0, 11);
        }      
        // The SteppIR Data Port has received data from timer query
        private void StepData_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort StepMsgs = (SerialPort)sender;
            string StepMsg = StepMsgs.ReadExisting();
            if (StepMsg.Length == 11)
            {
                string ac = StepMsg.Substring(6, 1);
                string dir = StepMsg.Substring(7, 1);
                if (ac == "\0")
                {   // antenna is NOT moving, but a freq update has been sent to the
                    // controller that may cause movement. We are waiting for StepCtr
                    // reps to see if it will start. See the Reps var for the delay count.
                    WriteToPort("ZZTI0;", iSleep);
                    StepCtr -= 1; // decrement the reps counter
                    if (StepCtr == 0)
                    {   // if 
                        StepTimer.Enabled = false;
                        ShowAnt(false);

                        if (bCal)
                        {   // if the calibration is finished send freq data to the antenna
                            StepPortMsg(lastFreq, "00", "52");
                            Thread.Sleep(100);
                            StepData.Write("?A\r");
                            StepTimer.Enabled = true;
                            StepCtr = reps; // counter to allow for delay
                            bCal = false;
                        }
                    }
                }
                else
                {   // antenna is moving
                    WriteToPort("ZZTI1;", iSleep);
                    ShowAnt(true);
                }
            }
        }
        // The Forward radio button was selected
        bool bFwd = false;
        private void rbFwd_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFwd.Checked)
            {
                bFwd = true; b180 = false; bBiDir = false; b34 = false;
                if (lastFreq != "") StepPortMsg(lastFreq, "00", "31");
            }
        }
        // The 180 radio button was selected
        bool b180 = false;
        private void rb180_CheckedChanged(object sender, EventArgs e)
        {
            if (rb180.Checked)
            {
                bFwd = false; b180 = true; bBiDir = false; b34 = false;
                if (lastFreq != "") StepPortMsg(lastFreq, "40", "31");
            }
        }
        // The Bi-Directional radio button was selected
        bool bBiDir = false;
        private void rbBiDir_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBiDir.Checked)
            {
                bFwd = false; b180 = false; bBiDir = true; b34 = false;
                if (lastFreq != "") StepPortMsg(lastFreq, "80", "31");
            }
        }
        // The 3/4 wave length radio button was selected (vertical only)
        bool b34 = false;
        private void rb34_CheckedChanged(object sender, EventArgs e)
        {
            if (rb34.Checked)
            {
                bFwd = false; b180 = false; bBiDir = false; b34 = true;
                if (lastFreq != "") StepPortMsg(lastFreq, "20", "31");
            }
        }
        // The interval timer has fired, send status request to controller
        void StepTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            { StepData.Write("?A\r"); }
            catch { }
        }
        // SteppIR enabled check box has changed
        private void chkStep_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStep.Checked)
            {   // Enable Freq update
                grpStepCtrl.Enabled = true;
                if (lastFreq != "")
                {   // 
                    StepPortMsg(lastFreq, "00", "52");
                    Thread.Sleep(100);
                    StepData.Write("?A\r");
                    StepTimer.Enabled = true;
                    StepCtr = reps; // counter to allow for delay
                }
                if (cboStep.SelectedIndex > 0) set.StepEnab = true; 
                else
                {
                    MessageBox.Show("No port has been selected for the SteppIR.\n\n" +
                    "Please select a valid port number and try again.", "Port Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chkStep.Checked = false;
                }
            }
            else
            {
                set.StepEnab = false;
                grpStepCtrl.Enabled = false;
            }
            set.Save();
        }
        // SteppIR Port Number hsa changed
        private void cboStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StepData.IsOpen) StepData.Close();
            if (cboStep.SelectedItem.ToString() != "")
            {
                StepData.PortName = cboStep.SelectedItem.ToString();
                try
                {
                    StepData.Open();
                }
                catch
                {
                    MessageBox.Show("The STeppIR serial port " + StepData.PortName +
                       " cannot be opened!\n", "Port Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboStep.SelectedText = "";
                    chkStep.Checked = false;
                    return;
                }
            }
            else
            {
                chkStep.Checked = false;
            }
            set.StepPortNum = cboStep.SelectedItem.ToString();
            set.Save();
        }
        // SteppIR timer interval has changed
        private void txtStepInv_TextChanged(object sender, EventArgs e)
        {
            try
            {
                StepTimer.Interval = Convert.ToDouble(txtStepInv.Text);
                set.StepInv = txtStepInv.Text;
                set.Save();
            }
            catch { }
        }
        // the SteppIR Port Com setting has changed
        private void cboStepCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboStepCom.SelectedIndex)
            {
                case 0: // 9600 8N1
                    StepData.BaudRate = 9600;
                    StepData.DataBits = 8;
                    StepData.Parity = System.IO.Ports.Parity.None;
                    StepData.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 1: // 9600 8N2
                    StepData.BaudRate = 9600;
                    StepData.DataBits = 8;
                    StepData.Parity = System.IO.Ports.Parity.None;
                    StepData.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                case 2: // 4800 8N1
                    StepData.BaudRate = 4800;
                    StepData.DataBits = 8;
                    StepData.Parity = System.IO.Ports.Parity.None;
                    StepData.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 3: // 4800 8N2
                    StepData.BaudRate = 4800;
                    StepData.DataBits = 8;
                    StepData.Parity = System.IO.Ports.Parity.None;
                    StepData.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                case 4: // 2400 8N1
                    StepData.BaudRate = 2400;
                    StepData.DataBits = 8;
                    StepData.Parity = System.IO.Ports.Parity.None;
                    StepData.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 5: // 2400 8N2
                    StepData.BaudRate = 2400;
                    StepData.DataBits = 8;
                    StepData.Parity = System.IO.Ports.Parity.None;
                    StepData.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                case 6: // 1200 8N1
                    StepData.BaudRate = 1200;
                    StepData.DataBits = 8;
                    StepData.Parity = System.IO.Ports.Parity.None;
                    StepData.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 7: // 1200 8N2
                    StepData.BaudRate = 1200;
                    StepData.DataBits = 8;
                    StepData.Parity = System.IO.Ports.Parity.None;
                    StepData.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    break;
            }
            set.StepCom = (int)cboStepCom.SelectedIndex;
            set.Save();
        }

        #endregion SteppIR

        #region Timer Events

        // If this event fires, the logger port has been inactive for the logTimer 
        // interval (n sec.) and probably means the logger is disconnected and the 
        // pollTimer must be enabled for the Decoder functions to continue.
        void logTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                // make sure the pollTimer is enabled
                if (pollTimer.Enabled != true) pollTimer.Enabled = true;
                logFlag = false;
                if (tempFlag)
                {
                    if (pollInt >= 1000)
                        tempTimer.Interval = Convert.ToDouble(txtInv.Text) * 1.05;
                    else if (pollInt < 1000)
                        tempTimer.Interval = 1000;
                    tempFlag = false;
                }
            }
            catch { }
        }
        // LP100 interval timer has elapsed
        void lpTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (chkLPenab.Checked)
                    LPport.Write(";P?");
                else
                    lpTimer.Enabled = false;
            }
            catch { }
        }
        // This event only fires if the Radio Control Program (RCP) is not active
        void pollTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (logFlag != true)    // RCP not active
            {
                try
                {
                    logTimer.Enabled = false;
                    WriteToPort("IF;", iSleep);
                    WriteToPort("FB;", iSleep);
                }
                catch { }
            }
        }

        void tempTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                WriteToPort("ZZTS;", iSleep);
            }
            catch { }
        }
        
        #endregion Timer Events

        #region Test Routines

        // The PW1 Test button was pressed
        // simulate poll message from IC-PW1 to DDUtil for freq read
        // Note that additional code to initialize the PW1Timer is in 
        // the Setup() routine 
        System.Timers.Timer pw1Timer;
        private void btnTest_Click(object sender, EventArgs e)
        {
            // toggle the timer on & off
            if (!pw1Timer.Enabled)
            { pw1Timer.Enabled = true; btnTest.BackColor = Color.Yellow; }
            else
            { pw1Timer.Enabled = false; btnTest.BackColor = Color.Empty; }
        }
        void pw1Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!TestPort.IsOpen)
            {
                //                TestPort.PortName = "COM18";
                //                TestPort.Open();
            }
            byte[] bytes = new byte[8];
            string preamble = "FE";
            string EOM = "FD";
            string mystring = EOM + "03" + "54" + "33" + preamble + preamble + "DD" + "FF";
            int j = 14;
            for (int i = 0; i < 8; i++)
            {
                string temp = mystring.Substring(j, 2);
                bytes[i] = byte.Parse(temp, NumberStyles.HexNumber);
                //                Console.Write("{0:x2} ", bytes[i]);
                j -= 2;
            }
            //            Console.WriteLine();
            // FE FE 33 54 03 FD
            TestPort.Write(bytes, 0, 8);
            Thread.Sleep(iSleep);
            // Output the request for mode query
            mystring = EOM + "04" + "54" + "33" + preamble + preamble + "80" + "EF";
            j = 14;
            for (int i = 0; i < 8; i++)
            {
                string temp = mystring.Substring(j, 2);
                bytes[i] = byte.Parse(temp, NumberStyles.HexNumber);
                //                Console.Write("{0:x2} ", bytes[i]);
                j -= 2;
            }
            //            Console.WriteLine();
            // FE FE 33 54 04 FD
            TestPort.Write(bytes, 0, 8);
            if (pw1Timer.Enabled != true) pw1Timer.Enabled = true;
        }
        #endregion Test Routines

        #region Window Geometry

        /// <summary>
        /// Saves window size and location to Settings
        /// </summary>
        /// <param name="thisWindowGeometry"></param>
        /// <param name="formIn"></param>
        public static void GeometryFromString(string thisWindowGeometry, Form formIn)
        {
            if (string.IsNullOrEmpty(thisWindowGeometry) == true)
            {
                return;
            }
            string[] numbers = thisWindowGeometry.Split('|');
            string windowString = numbers[4];
            if (windowString == "Normal")
            {
                Point windowPoint = new Point(int.Parse(numbers[0]),
                    int.Parse(numbers[1]));
                Size windowSize = new Size(int.Parse(numbers[2]),
                    int.Parse(numbers[3]));

                bool locOkay = GeometryIsBizarreLocation(windowPoint, windowSize);
                bool sizeOkay = GeometryIsBizarreSize(windowSize);

                if (locOkay == true && sizeOkay == true)
                {
                    formIn.Location = windowPoint;
                    formIn.Size = windowSize;
                    formIn.StartPosition = FormStartPosition.Manual;
                    formIn.WindowState = FormWindowState.Normal;
                }
                else if (sizeOkay == true)
                {
                    formIn.Size = windowSize;
                }
            }
            else if (windowString == "Maximized")
            {
                formIn.Location = new Point(100, 100);
                formIn.StartPosition = FormStartPosition.Manual;
                formIn.WindowState = FormWindowState.Maximized;
            }
        }
        // See if the window location is funky 
        private static bool GeometryIsBizarreLocation(Point loc, Size size)
        {
            bool locOkay;
            if (loc.X < 0 || loc.Y < 0)
            {
                locOkay = false;
            }
            else if (loc.X + size.Width > Screen.PrimaryScreen.WorkingArea.Width)
            {
                locOkay = false;
            }
            else if (loc.Y + size.Height > Screen.PrimaryScreen.WorkingArea.Height)
            {
                locOkay = false;
            }
            else
            {
                locOkay = true;
            }
            return locOkay;
        }
        // See if the window size is funky 
        private static bool GeometryIsBizarreSize(Size size)
        {
            return (size.Height <= Screen.PrimaryScreen.WorkingArea.Height &&
                size.Width <= Screen.PrimaryScreen.WorkingArea.Width);
        }
        public static string GeometryToString(Form mainForm)
        {
            return mainForm.Location.X.ToString() + "|" +
                mainForm.Location.Y.ToString() + "|" +
                mainForm.Size.Width.ToString() + "|" +
                mainForm.Size.Height.ToString() + "|" +
                mainForm.WindowState.ToString();
        }
        #endregion Window Geometry

    }
}
