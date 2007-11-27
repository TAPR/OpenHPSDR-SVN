//=================================================================
// Setup.cs
//=================================================================
// Copyright (C) 2007  Steve Nance
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
// You may contact the author via email at: k5fr@arrl.net
//=================================================================

using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.XPath;
using DataDecoder.Properties;

namespace DataDecoder
{
    public partial class Setup : Form
    {
        #region Variables

        ASCIIEncoding AE = new ASCIIEncoding();
        CATSerialPorts.CATSerialPort sp;
        DataSet ds = new DataSet();
        Hashtable flist = new Hashtable();
        Settings set = Settings.Default;
        bool logFlag = false;
        int LPTnum = 0;     // decimal number of selected LPT port
        double pollInt = 0;    // CAT port interval timer uses txtInv text box
        string fileName = "BandData.xml";
        string[] ports;
        string OutBuffer;
        string ver = "1.0.0 Beta";
        string vfo = "";
        System.Timers.Timer pollTimer;
        System.Timers.Timer logTimer;

        #endregion Variables

        #region Properties

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
        /// The default Logger port
        /// </summary>
        private string default_LogPort = "";
        public string DefaultLogPort
        {
            get { return default_LogPort; }
            set { default_LogPort = value; }
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
            InitializeComponent();
            fileName = set.DataFile;
            txtFile0.Text = fileName;
            chkDevice.Checked = set.DevEnab;
            chkDev0.Checked = set.Dev0Enab;
            PortAccess.Output(LPTnum, 0);
            
            // set port interval timer to saved value, if 0 default to 1000
            string str = "";        
            str = set.portInv;
            txtInv.Text = str;
            pollInt = Convert.ToDouble(set.portInv);
            if (pollInt == 0) pollInt = 1000.0;

            //setup radio port timer for a 1 second interrupt
            pollTimer = new System.Timers.Timer();
            pollTimer.Elapsed += new System.Timers.ElapsedEventHandler(pollTimer_Elapsed);
            pollTimer.Interval = pollInt;  // 1000 = 1 second
            pollTimer.Enabled = true;

            // setup Log Port Timer for a 10 second interrupt
            logTimer = new System.Timers.Timer();
            logTimer.Elapsed += new System.Timers.ElapsedEventHandler(logTimer_Elapsed);
            logTimer.Interval = 10000;      // 10000 = 10 seconds
            logTimer.Enabled = true;

            CreateSerialPort();
            GetPortNames();             // enumerate serial ports and load combo boxes

            // set Radio Serial port to the last port used
            str = set.RadioPort;
            cboCAT.Text = str;
            DefaultComRadio = str;
            SetDefaultComRadio();
            txtInv.Text = Convert.ToString(pollInt);

            // set Acc Serial port to the last port used
            str = set.AccPort;
            cboSerAcc.Text = str;
            if (AccPort.IsOpen) AccPort.Close();
            try
            {   // try to open the selected AccPort:
                AccPort.PortName = str;
                DefaultComAcc = str;
                SetDefaultComAcc();
            }
            catch
            {   // give a message, if the port is not available:
                MessageBox.Show("Serial port " + AccPort.PortName +
                   " cannot be opened!", "Port Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSerAcc.SelectedText = "";
            }

            // set Logger Serial port to the last port used
            str = set.LogPort;
            cboLogPort.Text = str;
            if (LogPort.IsOpen) LogPort.Close();
            try
            {   // try to open the selected LogPort:
                LogPort.PortName = str;
                DefaultLogPort = str;
                SetDefaultLogPort();
            }
            catch
            {   // give a message, if the port is not available:
                MessageBox.Show("Serial port " + LogPort.PortName +
                   " cannot be opened!", "Port Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLogPort.SelectedText = "";
            }

            // set LPT port to last one used
            str = set.lptPort;
            try
            {
                if (str == "LPT1")
                {
                    rb1.Checked = true; LPTnum = 888; DefaultLPT = str;
                }
                else if (str == "LPT2")
                {
                    rb2.Checked = true; LPTnum = 632; DefaultLPT = str;
                }
                else if (str == "LPT3")
                {
                    rb3.Checked = true; LPTnum = 636; DefaultLPT = str;
                }
                else if (str == "LPT4")
                {
                    rb4.Checked = true; LPTnum = 620; DefaultLPT = str;
                }
                else if (str == "None")
                {
                    rbNone.Checked = true; LPTnum = 0; DefaultLPT = str;
                }
                else
                {
                    rbNone.Checked = true; LPTnum = 0; DefaultLPT = "NONE"; chkDevice.Checked = false;
                }
            }
            catch
            {
            }
            GetBandData(fileName);
            Dev0.Text = set.Device0;
            cboDevice.Items.Add(set.Device0);
            cboDevice.Items.Add(set.Device1);
            cboDevice.Items.Add(set.Device2);
            cboDevice.Text = set.Device;
        }

        #endregion Initialization

        #region Methods
        
        // output data to parallel port
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
                MessageBox.Show(ex.ToString(), "File Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            // Read in the Band data from the XML file into the hash table
            // Uses format 14080 = 140, 3575 = 35. This placs a different value pair every 100 khz
            try
            {
                flist.Clear();
                string freq="";
                string data="";
                FileStream bdata = new FileStream(fileName, FileMode.Open);
                XPathDocument myDoc = new XPathDocument(bdata);
                XPathNavigator docNav = myDoc.CreateNavigator();
                foreach (XPathNavigator node in docNav.Select ("//banddata/bandmulti/band/*"))
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
                MessageBox.Show(ex.ToString(), "File Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion Methods

        #region Helper Methods
        
        // Lookup freq data in hash table & output to LPT port
        int keyValue;
        private void LookUp(string freq)
        {
            if (flist.ContainsKey(freq))
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
        delegate void SetTitleCallback(string text);
        private void SetTitle(string text)
        {
            // InvokeRequired compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtTitle.InvokeRequired)
            {
                SetTitleCallback d = new SetTitleCallback(SetTitle);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.Text = text;
            }
        }
        #endregion Helper Methods

        #region Data Grid Events
        // A cell value changed in the data grid 
        private void dg1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string dgData = dg1.CurrentRow.Cells["freq"].Value.ToString();
            dgData = dgData.Substring(0, dgData.Length-2);
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

        #region Form Events

        // Device 0 name has been changed, change the device tab and save to settings
        private void Dev0_TextChanged(object sender, EventArgs e)
        {
            this.tabDev0.Text = Dev0.Text;
            set.Device0 = Dev0.Text;
            set.Save();                     // save new Device0 to system settings
        }
        // Device 0 Drop Down open - re-load cboDevice combo box
        private void Dev0_DropDown(object sender, EventArgs e)
        {
            cboDevice.Items.Clear();
            cboDevice.Items.Add(Dev0.Text);
            cboDevice.Items.Add("Device 1");
            cboDevice.Items.Add("Device 2");
        }
        // Select Device 0 Decoder Data File
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
        // Re-Load Device Data File
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
                MessageBox.Show(ex.ToString(), "File Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        // call the help file
        private void btnHelp_Click(object sender, EventArgs e)
        {
            Process.Start("HelpDecoder.htm");
        }
        // program closing
        private void Setup_FormClosing(object sender, FormClosedEventArgs e)
        {
            pollTimer.Enabled = false;
            logTimer.Enabled = false;
            PortAccess.Output(LPTnum, 0);
            LogPort.Close();
            sp.Close();
            AccPort.Close();
        }

        // The LPT port changed
        private void grpLPT_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == rbNone)
            { OutParallelPort(LPTnum,0); LPTnum = 0; set.lptPort = "NONE"; }
            else if (sender == rb1)
            { LPTnum = 888; set.lptPort = "LPT1"; }
            else if (sender == rb2)
            { LPTnum = 632; set.lptPort = "LPT2"; }
            else if (sender == rb3)
            { LPTnum = 636; set.lptPort = "LPT3"; }
            else if (sender == rb4)
            { LPTnum = 620; set.lptPort = "LPT4"; }
            else
            { LPTnum = 0; set.lptPort = "NONE"; }
            this.txtTitle.Text = LPTnum.ToString();
            set.Save();     // save new LPT port to system settings
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
            }
            catch
            {
            }
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

        #endregion Form Events

        #region Serial Port Events

        /// <summary>
        /// Radio CAT data has arrived
        /// </summary>
        string CommBuffer = "";
        string rawFreq = "";
        void sp_CATRxEvent(object source, CATSerialPorts.SerialRXEvent e)
        {
            CommBuffer += AE.GetString(e.buffer, 0, e.buffer.Length);			//put the data in the string
            Regex rex = new Regex(".*?;");										//accept any string ending in ;
            for (Match m = rex.Match(CommBuffer); m.Success; m = m.NextMatch())	//loop thru the buffer and find matches
            {
                OutBuffer = m.Value;
                CommBuffer = CommBuffer.Replace(m.Value, "");					//remove the match from the buffer
                if (logFlag == true) // the logger is active
                {
                    string title = "DDUtil " + ver;
                    string id = "";
                    string logFreq = "";
                    string regex = "(?<cat>\\w{2})(?<mz>\\d{5})(?<kz>\\d{3})(?<hz>\\d{2})";
                    string mask = "${mz},${kz}.${hz}";
                    string freq = "";
                    string freqLook = "";

                    rawFreq = OutBuffer;
                    LogPort.Write(OutBuffer); // send radio's CAT reply back to logger
                    rawFreq = OutBuffer;
                    OutBuffer = "";
                    if (rawFreq.Length > 4 && rawFreq.Substring(0, 4) == "IF00")
                    {
                        if (rawFreq.Substring(rawFreq.Length - 6, 1) == "0")
                        {   // if xmit vfo is "A" then send passive listener commands now
                            vfo = "VFO A";
                            logFreq = rawFreq.Substring(2, 13);
                            PortSend("IF" + logFreq + ";");
                            freq = Regex.Replace(rawFreq, regex, mask);
                            freq = freq.TrimStart('0');
                            freqLook = rawFreq.Substring(2, 8);
                            freqLook = freqLook.TrimStart('0');
                            freqLook = freqLook.Substring(0, freqLook.Length - 2);
                            id = title + "  - [RC] [" + freq.Substring(0, 9) + "] (" + vfo + ")";
//                            this.SetText(freq);
                            LookUp(freqLook);   //decode freq data and output to port
                            this.SetTitle(id + "   " + keyValue);
                        }
                        else // if not, set vfo to "B" and wait for next FB command to arrive
                        {
                            vfo = "VFO B";
                        }
                    }
                    else if (rawFreq.Length > 4 && rawFreq.Substring(0, 4) == "FB00" && vfo == "VFO B")
                    {   // see if this is the right command for the B vfo
                        logFreq = rawFreq.Substring(2, 11);
                        PortSend("IF" + logFreq + ";");
                        freq = Regex.Replace(rawFreq, regex, mask);
                        freq = freq.TrimStart('0');
                        freqLook = rawFreq.Substring(2, 8);
                        freqLook = freqLook.TrimStart('0');
                        freqLook = freqLook.Substring(0, freqLook.Length - 2);
                        id = title + "  - [RC] [" + freq.Substring(0, 9) + "] (" + vfo + ")";
//                        this.SetText(freq);
                        LookUp(freqLook);   //decode freq data and output to port
                        this.SetTitle(id + "   " + keyValue);
                    }
                }
            }
        }

        /// <summary>
        /// Process data from the Logger port
        /// </summary>
        public delegate void SerialDataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e);

        // The logger is active
        protected void OnReceive(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort logPortMsgs = (SerialPort)sender;    // send incoming messae to the radio
            string logMsg = logPortMsgs.ReadExisting();
            logFlag = true;
            // send the data on to the radio 
            sp.Write(logMsg);               //pass log port msg to radio port
            logTimer.Enabled = true;
        }

        // New Logger port was selected
        private void cboLogPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LogPort.IsOpen) LogPort.Close();
            LogPort.PortName = cboLogPort.SelectedItem.ToString();
            try
            {
                LogPort.Open();
            }
            catch
            {
                MessageBox.Show("Serial port " + LogPort.PortName +
                   " cannot be opened!", "Port Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLogPort.SelectedText = "";
                return;
            }
            // save new port setting
            string str = set.LogPort;
            set.LogPort = cboLogPort.SelectedItem.ToString();
            set.Save();

        }

        // new acc port was selected
        private void cboSerAcc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (AccPort.IsOpen) AccPort.Close();
            AccPort.PortName = cboSerAcc.SelectedItem.ToString();
            try
            {
                AccPort.Open();
            }
            catch
            {
                MessageBox.Show("Serial port " + AccPort.PortName +
                   " cannot be opened!", "Port Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSerAcc.SelectedText = "";
                this.Text = "CmdrSDR - Select serial port!";
                return;
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
                MessageBox.Show("Serial port " + sp.Name +
                   " cannot be opened!/n", "Port Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCAT.SelectedText = "";
                return;
            }
            // save new port setting
            string str = set.RadioPort;
            set.RadioPort = cboCAT.SelectedItem.ToString();
            set.Save();
        }

       
        #endregion Serial Port Events

        #region Serial Port Methods

        /// <summary>
        /// send data to the Passive Listener port
        /// </summary>
        private void PortSend(string xmitFreq)
        {
            if (AccPort.IsOpen)
            {
                AccPort.WriteLine(xmitFreq);
            }
            else
            {
                MessageBox.Show("Serial port is closed!", "Port Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Creates a new serial port and rx data available event
        /// </summary>
        private void CreateSerialPort()
        {
            sp = new CATSerialPorts.CATSerialPort();
            sp.CATRxEvent += new CATSerialPorts.CATSerialPort.CATSerialRXEventHandler(sp_CATRxEvent);
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
            }
            cboCAT.Sorted = true;
            cboSerAcc.Sorted = true;
            cboLogPort.Sorted = true;
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
        /// Opens the default Accessory port.
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
        /// Opens the default Logger port.
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
                MessageBox.Show("Default Logger Port is not valid.  Please select a port from the list.");
                cboLogPort.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Opens the serial port name stored in the DefaultComRadio property.
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
            catch (Exception spex)
            {
                MessageBox.Show(spex.Message);
            }
        }

        /// <summary>
        /// Writes the current CAT command to the radio serial port.
        /// The keepalive timer is disabled and restarted for each
        /// write.  The sleep period gives the radio CAT parser time
        /// to react and return an answer.'
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sleep"></param>
        private void WriteToPort(string cmd, int sleep)
        {
            try
            {
                pollTimer.Enabled = false;
                sp.Write(cmd);
                Thread.Sleep(sleep);
                pollTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Serial Port Methods

        #region Timer Events

        // If this event fires the logger port has been inactive for the logTimer 
        // interval (10 sec.) and probably means the logger is disconnected and the 
        // pollTimer must be enabled for the Decoder functions to continue.
        void logTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // make sure the pollTimer is enabled
            if (pollTimer.Enabled != true) pollTimer.Enabled = true;
            logFlag = false;
        }
        // This event only fires if the logger is not active
        void pollTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string ans = "";
            string title = "DDUtil " + ver;
            string id = "";
            string rawFreq;
            string regex = "(?<cat>\\w{4})(?<mz>\\d{5})(?<kz>\\d{3})(?<hz>\\d{2})";
            string mask = "${mz},${kz}.${hz}";
            string freq = "";
            string vfo = "";
            string freqLook = "";

            if (logFlag != true)    // logger not active
            {
                WriteToPort("ZZSP;", 100);
                ans = OutBuffer;
                OutBuffer = "";
                if (ans != "" && ans != null)
                {
                    switch (ans)
                    {
                        case "ZZSP0;":
                            WriteToPort("ZZFA;", 100);
                            vfo = "VFO A";
                            break;
                        case "ZZSP1;":
                            WriteToPort("ZZFB;", 100);
                            vfo = "VFO B";
                            break;
                        default:
                            break;
                    }
                    rawFreq = OutBuffer;
                    if (rawFreq.Length == 16)
                    {
                        try
                        {
                            PortSend("IF" + rawFreq.Substring(4, 11) + ";"); //send rs-232 data
                            freq = Regex.Replace(rawFreq, regex, mask);
                            freq = freq.TrimStart('0');
                            freqLook = rawFreq.Substring(4, 8);
                            freqLook = freqLook.TrimStart('0');
                            freqLook = freqLook.Substring(0, freqLook.Length - 2);
                            OutBuffer = "";
                            id = title + "  - [" + freq.Substring(0, 9) + "] (" + vfo + ")";
                            LookUp(freqLook);   //decode freq data and output to LPT port
                        }
                        catch
                        {
                            // catch for unexpected CAT commands shorter than what we want
                            // see if() above
                        }
                    }
                }
                else
                {
                    id = title + " - [not connected]";
                }
                this.SetTitle(id + "   " + keyValue);
            }
        }
        #endregion Timer Events 

        private void button2_Click(object sender, EventArgs e)
        {
            this.Size = new Size(442,34);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Size = new Size(442, 420);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Size = new Size(442, 57);
        }

    }
}