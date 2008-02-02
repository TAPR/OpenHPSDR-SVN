namespace DataDecoder
{
    partial class Setup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setup));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPorts = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.cboRadData = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRadNum = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cboRadio = new System.Windows.Forms.ComboBox();
            this.cboFollow = new System.Windows.Forms.CheckBox();
            this.grpBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cboLogPort = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtInv = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboSerAcc = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboCAT = new System.Windows.Forms.ComboBox();
            this.grpBox2 = new System.Windows.Forms.GroupBox();
            this.lblPortBtn = new System.Windows.Forms.Label();
            this.btnPortNum = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkDevice = new System.Windows.Forms.CheckBox();
            this.grpLPT = new System.Windows.Forms.GroupBox();
            this.rbOther = new System.Windows.Forms.RadioButton();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.rb4 = new System.Windows.Forms.RadioButton();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rb3 = new System.Windows.Forms.RadioButton();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.tabDev0 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave0 = new System.Windows.Forms.Button();
            this.btnReLoad0 = new System.Windows.Forms.Button();
            this.btnFile0 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFile0 = new System.Windows.Forms.TextBox();
            this.dg1 = new System.Windows.Forms.DataGridView();
            this.chkDev0 = new System.Windows.Forms.CheckBox();
            this.Dev0 = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.tabDev1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnPFfile = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.txtProfLoc = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboLPport = new System.Windows.Forms.ComboBox();
            this.txtLPint = new System.Windows.Forms.TextBox();
            this.chkLPenab = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.AccPort = new System.IO.Ports.SerialPort(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtFwd = new System.Windows.Forms.TextBox();
            this.txtSWR = new System.Windows.Forms.TextBox();
            this.btnProfiler = new System.Windows.Forms.Button();
            this.btnReStart = new System.Windows.Forms.Button();
            this.LogPort = new System.IO.Ports.SerialPort(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.lP100ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.LPport = new System.IO.Ports.SerialPort(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.flexProfilerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl.SuspendLayout();
            this.tabPorts.SuspendLayout();
            this.grpBox1.SuspendLayout();
            this.grpBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.grpLPT.SuspendLayout();
            this.tabDev0.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg1)).BeginInit();
            this.tabDev1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPorts);
            this.tabControl.Controls.Add(this.tabDev0);
            this.tabControl.Controls.Add(this.tabDev1);
            this.tabControl.Location = new System.Drawing.Point(6, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(423, 327);
            this.tabControl.TabIndex = 31;
            this.toolTip1.SetToolTip(this.tabControl, "Right-Click for Options");
            // 
            // tabPorts
            // 
            this.tabPorts.Controls.Add(this.label12);
            this.tabPorts.Controls.Add(this.cboRadData);
            this.tabPorts.Controls.Add(this.label9);
            this.tabPorts.Controls.Add(this.txtRadNum);
            this.tabPorts.Controls.Add(this.label8);
            this.tabPorts.Controls.Add(this.cboRadio);
            this.tabPorts.Controls.Add(this.cboFollow);
            this.tabPorts.Controls.Add(this.grpBox1);
            this.tabPorts.Controls.Add(this.grpBox2);
            this.tabPorts.Location = new System.Drawing.Point(4, 22);
            this.tabPorts.Name = "tabPorts";
            this.tabPorts.Padding = new System.Windows.Forms.Padding(3);
            this.tabPorts.Size = new System.Drawing.Size(415, 301);
            this.tabPorts.TabIndex = 0;
            this.tabPorts.Text = "Ports";
            this.toolTip1.SetToolTip(this.tabPorts, "Right-Click for Options");
            this.tabPorts.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(223, 282);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 13);
            this.label12.TabIndex = 29;
            this.label12.Text = "PL/Radio Comm";
            // 
            // cboRadData
            // 
            this.cboRadData.FormattingEnabled = true;
            this.cboRadData.Items.AddRange(new object[] {
            "9600 8-N-1",
            "9600 8-N-2",
            "4800 8-N-1",
            "4800 8-N-2",
            "2400 8-N-1",
            "2400 8-N-2",
            "1200 8-N-1",
            "1200 8-N-2"});
            this.cboRadData.Location = new System.Drawing.Point(222, 258);
            this.cboRadData.Name = "cboRadData";
            this.cboRadData.Size = new System.Drawing.Size(83, 21);
            this.cboRadData.TabIndex = 28;
            this.toolTip1.SetToolTip(this.cboRadData, "Sets Passive Listener / Slave Radio port comm parameters.");
            this.cboRadData.SelectedIndexChanged += new System.EventHandler(this.cboRadData_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(336, 281);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "CI-V Addr";
            // 
            // txtRadNum
            // 
            this.txtRadNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRadNum.Location = new System.Drawing.Point(339, 258);
            this.txtRadNum.Name = "txtRadNum";
            this.txtRadNum.Size = new System.Drawing.Size(32, 20);
            this.txtRadNum.TabIndex = 15;
            this.txtRadNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtRadNum, "Icom CI-V Hex Address");
            this.txtRadNum.TextChanged += new System.EventHandler(this.txtRadNum_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(107, 282);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Slave Radio Type";
            // 
            // cboRadio
            // 
            this.cboRadio.FormattingEnabled = true;
            this.cboRadio.Items.AddRange(new object[] {
            "None",
            "Kenwood",
            "Yaesu Type 1",
            "Yaesu Type 2",
            "Icom"});
            this.cboRadio.Location = new System.Drawing.Point(107, 258);
            this.cboRadio.Name = "cboRadio";
            this.cboRadio.Size = new System.Drawing.Size(89, 21);
            this.cboRadio.TabIndex = 26;
            this.toolTip1.SetToolTip(this.cboRadio, "Select Radio to Follow PowerSDR (right click on background for menu). ");
            this.cboRadio.SelectedIndexChanged += new System.EventHandler(this.cboRadio_SelectedIndexChanged);
            // 
            // cboFollow
            // 
            this.cboFollow.AutoSize = true;
            this.cboFollow.Location = new System.Drawing.Point(11, 262);
            this.cboFollow.Name = "cboFollow";
            this.cboFollow.Size = new System.Drawing.Size(82, 17);
            this.cboFollow.TabIndex = 26;
            this.cboFollow.Text = "Follow SDR";
            this.toolTip1.SetToolTip(this.cboFollow, "When selected, directs a Slave Radio connected to the Passive Listener port to fo" +
                    "llow the PowerSDR frequency .");
            this.cboFollow.UseVisualStyleBackColor = true;
            this.cboFollow.CheckedChanged += new System.EventHandler(this.cboFollow_CheckedChanged);
            // 
            // grpBox1
            // 
            this.grpBox1.Controls.Add(this.label11);
            this.grpBox1.Controls.Add(this.cboLogPort);
            this.grpBox1.Controls.Add(this.label6);
            this.grpBox1.Controls.Add(this.txtInv);
            this.grpBox1.Controls.Add(this.label5);
            this.grpBox1.Controls.Add(this.cboSerAcc);
            this.grpBox1.Controls.Add(this.label3);
            this.grpBox1.Controls.Add(this.cboCAT);
            this.grpBox1.Location = new System.Drawing.Point(11, 14);
            this.grpBox1.Name = "grpBox1";
            this.grpBox1.Size = new System.Drawing.Size(381, 78);
            this.grpBox1.TabIndex = 19;
            this.grpBox1.TabStop = false;
            this.grpBox1.Text = "Serial Ports";
            this.toolTip1.SetToolTip(this.grpBox1, "Right-Click for Options");
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(214, 44);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 32);
            this.label11.TabIndex = 14;
            this.label11.Text = "Radio Control Port";
            // 
            // cboLogPort
            // 
            this.cboLogPort.FormattingEnabled = true;
            this.cboLogPort.Location = new System.Drawing.Point(215, 19);
            this.cboLogPort.Name = "cboLogPort";
            this.cboLogPort.Size = new System.Drawing.Size(85, 21);
            this.cboLogPort.TabIndex = 13;
            this.toolTip1.SetToolTip(this.cboLogPort, "Select Auxillary Radio Control Port (HRD, MixW, DXLab)");
            this.cboLogPort.SelectedIndexChanged += new System.EventHandler(this.cboLogPort_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(316, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 32);
            this.label6.TabIndex = 12;
            this.label6.Text = "CAT Polling Interval";
            // 
            // txtInv
            // 
            this.txtInv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInv.Location = new System.Drawing.Point(316, 19);
            this.txtInv.Name = "txtInv";
            this.txtInv.Size = new System.Drawing.Size(55, 22);
            this.txtInv.TabIndex = 11;
            this.toolTip1.SetToolTip(this.txtInv, "Select CAT port polling interval (1000 = 1 secoond)");
            this.txtInv.TextChanged += new System.EventHandler(this.txtInv_TextChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(109, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 32);
            this.label5.TabIndex = 3;
            this.label5.Text = "Passive Listener Port";
            // 
            // cboSerAcc
            // 
            this.cboSerAcc.FormattingEnabled = true;
            this.cboSerAcc.Location = new System.Drawing.Point(109, 19);
            this.cboSerAcc.Name = "cboSerAcc";
            this.cboSerAcc.Size = new System.Drawing.Size(85, 21);
            this.cboSerAcc.TabIndex = 2;
            this.toolTip1.SetToolTip(this.cboSerAcc, "Select Passive Listener Port (your hardware serial port)");
            this.cboSerAcc.SelectedIndexChanged += new System.EventHandler(this.cboSerAcc_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 33);
            this.label3.TabIndex = 1;
            this.label3.Text = "Radio CAT Port";
            // 
            // cboCAT
            // 
            this.cboCAT.FormattingEnabled = true;
            this.cboCAT.Location = new System.Drawing.Point(7, 19);
            this.cboCAT.Name = "cboCAT";
            this.cboCAT.Size = new System.Drawing.Size(85, 21);
            this.cboCAT.TabIndex = 0;
            this.toolTip1.SetToolTip(this.cboCAT, "Select Radio CAT Serial Port");
            this.cboCAT.SelectedIndexChanged += new System.EventHandler(this.cboCAT_SelectedIndexChanged);
            // 
            // grpBox2
            // 
            this.grpBox2.Controls.Add(this.lblPortBtn);
            this.grpBox2.Controls.Add(this.btnPortNum);
            this.grpBox2.Controls.Add(this.label7);
            this.grpBox2.Controls.Add(this.groupBox5);
            this.grpBox2.Controls.Add(this.grpLPT);
            this.grpBox2.Controls.Add(this.txtPort);
            this.grpBox2.Location = new System.Drawing.Point(11, 102);
            this.grpBox2.Name = "grpBox2";
            this.grpBox2.Size = new System.Drawing.Size(381, 139);
            this.grpBox2.TabIndex = 18;
            this.grpBox2.TabStop = false;
            this.grpBox2.Text = "Parallel Port";
            this.toolTip1.SetToolTip(this.grpBox2, "Right-Click for Options");
            // 
            // lblPortBtn
            // 
            this.lblPortBtn.AutoSize = true;
            this.lblPortBtn.Location = new System.Drawing.Point(247, 100);
            this.lblPortBtn.Name = "lblPortBtn";
            this.lblPortBtn.Size = new System.Drawing.Size(132, 13);
            this.lblPortBtn.TabIndex = 30;
            this.lblPortBtn.Text = "Press to save port address";
            this.lblPortBtn.Visible = false;
            // 
            // btnPortNum
            // 
            this.btnPortNum.BackColor = System.Drawing.Color.Transparent;
            this.btnPortNum.Location = new System.Drawing.Point(222, 96);
            this.btnPortNum.Name = "btnPortNum";
            this.btnPortNum.Size = new System.Drawing.Size(19, 19);
            this.btnPortNum.TabIndex = 29;
            this.toolTip1.SetToolTip(this.btnPortNum, "Press to save non-standard port number");
            this.btnPortNum.UseVisualStyleBackColor = false;
            this.btnPortNum.Click += new System.EventHandler(this.btnPortNum_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(145, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "Port Number";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cboDevice);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.chkDevice);
            this.groupBox5.Location = new System.Drawing.Point(150, 23);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(210, 60);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Data Signals";
            this.toolTip1.SetToolTip(this.groupBox5, "Right-Click for Options");
            // 
            // cboDevice
            // 
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(88, 16);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(116, 21);
            this.cboDevice.TabIndex = 22;
            this.toolTip1.SetToolTip(this.cboDevice, "Select Device to use for LPT Data Port.");
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            this.cboDevice.DropDown += new System.EventHandler(this.Dev0_DropDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Device";
            // 
            // chkDevice
            // 
            this.chkDevice.AutoSize = true;
            this.chkDevice.Location = new System.Drawing.Point(23, 20);
            this.chkDevice.Name = "chkDevice";
            this.chkDevice.Size = new System.Drawing.Size(59, 17);
            this.chkDevice.TabIndex = 19;
            this.chkDevice.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkDevice, "Selects to output data on seleced LPT port from selected Device.");
            this.chkDevice.UseVisualStyleBackColor = true;
            this.chkDevice.CheckedChanged += new System.EventHandler(this.chkDevice_CheckedChanged);
            // 
            // grpLPT
            // 
            this.grpLPT.Controls.Add(this.rbOther);
            this.grpLPT.Controls.Add(this.rb1);
            this.grpLPT.Controls.Add(this.rb4);
            this.grpLPT.Controls.Add(this.rb2);
            this.grpLPT.Controls.Add(this.rbNone);
            this.grpLPT.Controls.Add(this.rb3);
            this.grpLPT.Location = new System.Drawing.Point(6, 23);
            this.grpLPT.Name = "grpLPT";
            this.grpLPT.Size = new System.Drawing.Size(122, 100);
            this.grpLPT.TabIndex = 0;
            this.grpLPT.TabStop = false;
            this.grpLPT.Text = "Port Selector";
            this.toolTip1.SetToolTip(this.grpLPT, "Select Parallel Port");
            // 
            // rbOther
            // 
            this.rbOther.AutoSize = true;
            this.rbOther.Location = new System.Drawing.Point(63, 20);
            this.rbOther.Name = "rbOther";
            this.rbOther.Size = new System.Drawing.Size(51, 17);
            this.rbOther.TabIndex = 27;
            this.rbOther.Text = "Other";
            this.rbOther.UseVisualStyleBackColor = true;
            this.rbOther.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // rb1
            // 
            this.rb1.AutoSize = true;
            this.rb1.Location = new System.Drawing.Point(6, 43);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(51, 17);
            this.rb1.TabIndex = 17;
            this.rb1.Text = "LPT1";
            this.rb1.UseVisualStyleBackColor = true;
            this.rb1.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // rb4
            // 
            this.rb4.AutoSize = true;
            this.rb4.BackColor = System.Drawing.Color.Transparent;
            this.rb4.Location = new System.Drawing.Point(63, 66);
            this.rb4.Name = "rb4";
            this.rb4.Size = new System.Drawing.Size(51, 17);
            this.rb4.TabIndex = 23;
            this.rb4.Text = "LPT4";
            this.rb4.UseVisualStyleBackColor = false;
            this.rb4.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // rb2
            // 
            this.rb2.AutoSize = true;
            this.rb2.Location = new System.Drawing.Point(6, 66);
            this.rb2.Name = "rb2";
            this.rb2.Size = new System.Drawing.Size(51, 17);
            this.rb2.TabIndex = 21;
            this.rb2.Text = "LPT2";
            this.rb2.UseVisualStyleBackColor = true;
            this.rb2.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(6, 20);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 26;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            this.rbNone.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // rb3
            // 
            this.rb3.AutoSize = true;
            this.rb3.Location = new System.Drawing.Point(63, 43);
            this.rb3.Name = "rb3";
            this.rb3.Size = new System.Drawing.Size(51, 17);
            this.rb3.TabIndex = 20;
            this.rb3.Text = "LPT3";
            this.rb3.UseVisualStyleBackColor = true;
            this.rb3.CheckedChanged += new System.EventHandler(this.grpLPT_CheckedChanged);
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.HideSelection = false;
            this.txtPort.Location = new System.Drawing.Point(149, 92);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(54, 22);
            this.txtPort.TabIndex = 10;
            this.toolTip1.SetToolTip(this.txtPort, "Decimal number of selected port. You can change this to any number (see help file" +
                    ").");
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // tabDev0
            // 
            this.tabDev0.Controls.Add(this.groupBox1);
            this.tabDev0.Controls.Add(this.dg1);
            this.tabDev0.Controls.Add(this.chkDev0);
            this.tabDev0.Controls.Add(this.Dev0);
            this.tabDev0.Controls.Add(this.label27);
            this.tabDev0.Location = new System.Drawing.Point(4, 22);
            this.tabDev0.Name = "tabDev0";
            this.tabDev0.Padding = new System.Windows.Forms.Padding(3);
            this.tabDev0.Size = new System.Drawing.Size(415, 301);
            this.tabDev0.TabIndex = 1;
            this.tabDev0.Text = "Device 0";
            this.toolTip1.SetToolTip(this.tabDev0, "Right-Click for Options");
            this.tabDev0.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSave0);
            this.groupBox1.Controls.Add(this.btnReLoad0);
            this.groupBox1.Controls.Add(this.btnFile0);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtFile0);
            this.groupBox1.Location = new System.Drawing.Point(6, 223);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 79);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data File";
            this.toolTip1.SetToolTip(this.groupBox1, "Right-Click for Options");
            // 
            // btnSave0
            // 
            this.btnSave0.Location = new System.Drawing.Point(253, 47);
            this.btnSave0.Name = "btnSave0";
            this.btnSave0.Size = new System.Drawing.Size(57, 23);
            this.btnSave0.TabIndex = 62;
            this.btnSave0.Text = "Save";
            this.toolTip1.SetToolTip(this.btnSave0, "Saves modified data file to file name listed in the data file box above.");
            this.btnSave0.UseVisualStyleBackColor = true;
            this.btnSave0.Click += new System.EventHandler(this.btnSave0_Click);
            // 
            // btnReLoad0
            // 
            this.btnReLoad0.Location = new System.Drawing.Point(175, 47);
            this.btnReLoad0.Name = "btnReLoad0";
            this.btnReLoad0.Size = new System.Drawing.Size(57, 23);
            this.btnReLoad0.TabIndex = 61;
            this.btnReLoad0.Text = "Re-Load";
            this.toolTip1.SetToolTip(this.btnReLoad0, "Re-loads selected data file.");
            this.btnReLoad0.UseVisualStyleBackColor = true;
            this.btnReLoad0.Click += new System.EventHandler(this.btnReLoad0_Click);
            // 
            // btnFile0
            // 
            this.btnFile0.Location = new System.Drawing.Point(95, 47);
            this.btnFile0.Name = "btnFile0";
            this.btnFile0.Size = new System.Drawing.Size(57, 23);
            this.btnFile0.TabIndex = 60;
            this.btnFile0.Text = "Select";
            this.toolTip1.SetToolTip(this.btnFile0, "Opens file dialog to select file to use for this device.");
            this.btnFile0.UseVisualStyleBackColor = true;
            this.btnFile0.Click += new System.EventHandler(this.btnFile0_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 59;
            this.label4.Text = "File Name";
            // 
            // txtFile0
            // 
            this.txtFile0.Location = new System.Drawing.Point(6, 19);
            this.txtFile0.Name = "txtFile0";
            this.txtFile0.Size = new System.Drawing.Size(391, 20);
            this.txtFile0.TabIndex = 58;
            this.toolTip1.SetToolTip(this.txtFile0, "Data file to use for this device.");
            this.txtFile0.WordWrap = false;
            // 
            // dg1
            // 
            this.dg1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg1.Location = new System.Drawing.Point(217, 6);
            this.dg1.Name = "dg1";
            this.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dg1.Size = new System.Drawing.Size(186, 205);
            this.dg1.TabIndex = 42;
            this.dg1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg1_CellValueChanged);
            // 
            // chkDev0
            // 
            this.chkDev0.AutoSize = true;
            this.chkDev0.Location = new System.Drawing.Point(11, 41);
            this.chkDev0.Name = "chkDev0";
            this.chkDev0.Size = new System.Drawing.Size(65, 17);
            this.chkDev0.TabIndex = 44;
            this.chkDev0.Text = "Enabled";
            this.toolTip1.SetToolTip(this.chkDev0, "Check to enable this device");
            this.chkDev0.UseVisualStyleBackColor = true;
            this.chkDev0.CheckedChanged += new System.EventHandler(this.chkDev0_CheckedChanged);
            // 
            // Dev0
            // 
            this.Dev0.Location = new System.Drawing.Point(84, 37);
            this.Dev0.Name = "Dev0";
            this.Dev0.Size = new System.Drawing.Size(100, 20);
            this.Dev0.TabIndex = 51;
            this.toolTip1.SetToolTip(this.Dev0, "Device name (Amp, Antenna Switch)");
            this.Dev0.TextChanged += new System.EventHandler(this.Dev0_TextChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(86, 60);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(72, 13);
            this.label27.TabIndex = 52;
            this.label27.Text = "Device Name";
            // 
            // tabDev1
            // 
            this.tabDev1.Controls.Add(this.groupBox3);
            this.tabDev1.Controls.Add(this.groupBox2);
            this.tabDev1.Location = new System.Drawing.Point(4, 22);
            this.tabDev1.Name = "tabDev1";
            this.tabDev1.Size = new System.Drawing.Size(415, 301);
            this.tabDev1.TabIndex = 2;
            this.tabDev1.Text = "Other";
            this.tabDev1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.btnPFfile);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.txtProfLoc);
            this.groupBox3.Location = new System.Drawing.Point(9, 145);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(403, 79);
            this.groupBox3.TabIndex = 61;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Flex Profiler";
            this.toolTip1.SetToolTip(this.groupBox3, "Right-Click for Options");
            // 
            // btnPFfile
            // 
            this.btnPFfile.Location = new System.Drawing.Point(158, 45);
            this.btnPFfile.Name = "btnPFfile";
            this.btnPFfile.Size = new System.Drawing.Size(57, 23);
            this.btnPFfile.TabIndex = 60;
            this.btnPFfile.Text = "Select";
            this.toolTip1.SetToolTip(this.btnPFfile, "Opens file dialog to select Profiler path.");
            this.btnPFfile.UseVisualStyleBackColor = true;
            this.btnPFfile.Click += new System.EventHandler(this.btnPFfile_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 42);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 13);
            this.label15.TabIndex = 59;
            this.label15.Text = "File Location";
            // 
            // txtProfLoc
            // 
            this.txtProfLoc.Location = new System.Drawing.Point(6, 19);
            this.txtProfLoc.Name = "txtProfLoc";
            this.txtProfLoc.Size = new System.Drawing.Size(391, 20);
            this.txtProfLoc.TabIndex = 58;
            this.toolTip1.SetToolTip(this.txtProfLoc, "Data file to use for this device.");
            this.txtProfLoc.WordWrap = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboLPport);
            this.groupBox2.Controls.Add(this.txtLPint);
            this.groupBox2.Controls.Add(this.chkLPenab);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(9, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(403, 81);
            this.groupBox2.TabIndex = 47;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "LP-100";
            // 
            // cboLPport
            // 
            this.cboLPport.FormattingEnabled = true;
            this.cboLPport.Location = new System.Drawing.Point(114, 29);
            this.cboLPport.Name = "cboLPport";
            this.cboLPport.Size = new System.Drawing.Size(85, 21);
            this.cboLPport.TabIndex = 17;
            this.toolTip1.SetToolTip(this.cboLPport, "Select Auxillary Radio Control Port (HRD, MixW, DXLab)");
            this.cboLPport.SelectedIndexChanged += new System.EventHandler(this.cboLPport_SelectedIndexChanged);
            // 
            // txtLPint
            // 
            this.txtLPint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLPint.Location = new System.Drawing.Point(244, 29);
            this.txtLPint.Name = "txtLPint";
            this.txtLPint.Size = new System.Drawing.Size(55, 22);
            this.txtLPint.TabIndex = 15;
            this.toolTip1.SetToolTip(this.txtLPint, "Select CAT port polling interval (1000 = 1 secoond)");
            this.txtLPint.TextChanged += new System.EventHandler(this.txtLPint_TextChanged);
            // 
            // chkLPenab
            // 
            this.chkLPenab.AutoSize = true;
            this.chkLPenab.Location = new System.Drawing.Point(17, 34);
            this.chkLPenab.Name = "chkLPenab";
            this.chkLPenab.Size = new System.Drawing.Size(65, 17);
            this.chkLPenab.TabIndex = 45;
            this.chkLPenab.Text = "Enabled";
            this.toolTip1.SetToolTip(this.chkLPenab, "Check to enable LP100 polling");
            this.chkLPenab.UseVisualStyleBackColor = true;
            this.chkLPenab.CheckedChanged += new System.EventHandler(this.chkLPenab_CheckedChanged);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(244, 54);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 15);
            this.label13.TabIndex = 16;
            this.label13.Text = "Interval";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(113, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 15);
            this.label2.TabIndex = 18;
            this.label2.Text = "Port";
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(386, 337);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 23);
            this.button1.TabIndex = 64;
            this.button1.Text = "Help";
            this.toolTip1.SetToolTip(this.button1, "A help file for DDUtil");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // AccPort
            // 
            this.AccPort.BaudRate = 4800;
            this.AccPort.StopBits = System.IO.Ports.StopBits.Two;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 200;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.StripAmpersands = true;
            // 
            // txtFwd
            // 
            this.txtFwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFwd.Location = new System.Drawing.Point(12, 338);
            this.txtFwd.Name = "txtFwd";
            this.txtFwd.Size = new System.Drawing.Size(61, 21);
            this.txtFwd.TabIndex = 65;
            this.toolTip1.SetToolTip(this.txtFwd, "Transmitter Forward Power from LP100");
            // 
            // txtSWR
            // 
            this.txtSWR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSWR.ForeColor = System.Drawing.Color.Red;
            this.txtSWR.Location = new System.Drawing.Point(145, 337);
            this.txtSWR.Name = "txtSWR";
            this.txtSWR.Size = new System.Drawing.Size(43, 21);
            this.txtSWR.TabIndex = 66;
            this.toolTip1.SetToolTip(this.txtSWR, "Antenna SWR from LP100");
            // 
            // btnProfiler
            // 
            this.btnProfiler.AutoSize = true;
            this.btnProfiler.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnProfiler.BackColor = System.Drawing.SystemColors.Control;
            this.btnProfiler.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProfiler.ForeColor = System.Drawing.Color.Blue;
            this.btnProfiler.Location = new System.Drawing.Point(240, 337);
            this.btnProfiler.Name = "btnProfiler";
            this.btnProfiler.Size = new System.Drawing.Size(59, 23);
            this.btnProfiler.TabIndex = 68;
            this.btnProfiler.Text = "PF Open";
            this.toolTip1.SetToolTip(this.btnProfiler, "Starts Flex Profiler program");
            this.btnProfiler.UseVisualStyleBackColor = false;
            this.btnProfiler.Click += new System.EventHandler(this.btnProfiler_Click);
            // 
            // btnReStart
            // 
            this.btnReStart.AutoSize = true;
            this.btnReStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnReStart.BackColor = System.Drawing.SystemColors.Control;
            this.btnReStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReStart.ForeColor = System.Drawing.Color.Blue;
            this.btnReStart.Location = new System.Drawing.Point(308, 337);
            this.btnReStart.Name = "btnReStart";
            this.btnReStart.Size = new System.Drawing.Size(56, 23);
            this.btnReStart.TabIndex = 69;
            this.btnReStart.Text = "Re-Start";
            this.toolTip1.SetToolTip(this.btnReStart, "Restarts DDUtil after closing Profiler");
            this.btnReStart.UseVisualStyleBackColor = false;
            this.btnReStart.Click += new System.EventHandler(this.btnReStart_Click);
            // 
            // LogPort
            // 
            this.LogPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.OnReceive);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem4,
            this.lP100ToolStripMenuItem,
            this.flexProfilerToolStripMenuItem,
            this.toolStripMenuItem3});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 158);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.toolStripMenuItem1.Text = "Restore Form Size";
            this.toolStripMenuItem1.ToolTipText = "Restore form to full size";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(172, 22);
            this.toolStripMenuItem2.Text = "Shrink Form Size";
            this.toolStripMenuItem2.ToolTipText = "Shrink form to title bar only";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(172, 22);
            this.toolStripMenuItem4.Text = "Slave Radio Info";
            this.toolStripMenuItem4.ToolTipText = "Info on choosing Slave Radio";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // lP100ToolStripMenuItem
            // 
            this.lP100ToolStripMenuItem.Name = "lP100ToolStripMenuItem";
            this.lP100ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.lP100ToolStripMenuItem.Text = "LP-100";
            this.lP100ToolStripMenuItem.Click += new System.EventHandler(this.lP100ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(172, 22);
            this.toolStripMenuItem3.Text = "About DDUtil";
            this.toolStripMenuItem3.ToolTipText = "About DDUtil";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // LPport
            // 
            this.LPport.BaudRate = 38400;
            this.LPport.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.LPport_DataReceived);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(74, 341);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 15);
            this.label10.TabIndex = 19;
            this.label10.Text = "Fwd Power";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(194, 341);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 15);
            this.label14.TabIndex = 67;
            this.label14.Text = "SWR";
            // 
            // flexProfilerToolStripMenuItem
            // 
            this.flexProfilerToolStripMenuItem.Name = "flexProfilerToolStripMenuItem";
            this.flexProfilerToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.flexProfilerToolStripMenuItem.Text = "Flex Profiler";
            this.flexProfilerToolStripMenuItem.Click += new System.EventHandler(this.flexProfilerToolStripMenuItem_Click);
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(434, 366);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.btnReStart);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.txtSWR);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.btnProfiler);
            this.Controls.Add(this.txtFwd);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Setup";
            this.Text = "DDUtil";
            this.toolTip1.SetToolTip(this, "Right-Click for Options");
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Setup_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabPorts.ResumeLayout(false);
            this.tabPorts.PerformLayout();
            this.grpBox1.ResumeLayout(false);
            this.grpBox1.PerformLayout();
            this.grpBox2.ResumeLayout(false);
            this.grpBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.grpLPT.ResumeLayout(false);
            this.grpLPT.PerformLayout();
            this.tabDev0.ResumeLayout(false);
            this.tabDev0.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg1)).EndInit();
            this.tabDev1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPorts;
        private System.Windows.Forms.GroupBox grpBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkDevice;
        private System.Windows.Forms.GroupBox grpLPT;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.RadioButton rb4;
        private System.Windows.Forms.RadioButton rb2;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rb3;
        private System.Windows.Forms.TabPage tabDev0;
        private System.Windows.Forms.CheckBox chkDev0;
        private System.Windows.Forms.DataGridView dg1;
        private System.Windows.Forms.TextBox Dev0;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.GroupBox grpBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboCAT;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFile0;
        private System.Windows.Forms.Button btnSave0;
        private System.Windows.Forms.Button btnReLoad0;
        private System.Windows.Forms.Button btnFile0;
        private System.Windows.Forms.ComboBox cboSerAcc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPort;
        private System.IO.Ports.SerialPort AccPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtInv;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabDev1;
        private System.IO.Ports.SerialPort LogPort;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboLogPort;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.RadioButton rbOther;
        private System.Windows.Forms.Button btnPortNum;
        private System.Windows.Forms.Label lblPortBtn;
        private System.Windows.Forms.CheckBox cboFollow;
        private System.Windows.Forms.ComboBox cboRadio;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.TextBox txtRadNum;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cboRadData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboLPport;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtLPint;
        private System.IO.Ports.SerialPort LPport;
        private System.Windows.Forms.TextBox txtFwd;
        private System.Windows.Forms.TextBox txtSWR;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkLPenab;
        private System.Windows.Forms.ToolStripMenuItem lP100ToolStripMenuItem;
        private System.Windows.Forms.Button btnProfiler;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnPFfile;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtProfLoc;
        private System.Windows.Forms.Button btnReStart;
        private System.Windows.Forms.ToolStripMenuItem flexProfilerToolStripMenuItem;

    }
}

