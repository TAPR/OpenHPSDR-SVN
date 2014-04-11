﻿namespace PowerSDR
{
    partial class PSForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PSForm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblPSWelcome = new System.Windows.Forms.Label();
            this.chkPSRelaxPtol = new System.Windows.Forms.CheckBoxTS();
            this.lblDisabled = new System.Windows.Forms.LabelTS();
            this.btnPSTwoToneGen = new System.Windows.Forms.ButtonTS();
            this.labelTS8 = new System.Windows.Forms.LabelTS();
            this.lblPSInfoFB = new System.Windows.Forms.LabelTS();
            this.lblPSInfoCO = new System.Windows.Forms.LabelTS();
            this.lblPSWarning = new System.Windows.Forms.LabelTS();
            this.labelTS9 = new System.Windows.Forms.LabelTS();
            this.btnPSAutoCalibrate = new System.Windows.Forms.ButtonTS();
            this.btnPSInformation = new System.Windows.Forms.ButtonTS();
            this.grpLincorSave = new System.Windows.Forms.GroupBoxTS();
            this.btnPSSave = new System.Windows.Forms.ButtonTS();
            this.btnPSRestore = new System.Windows.Forms.ButtonTS();
            this.labelTS4 = new System.Windows.Forms.LabelTS();
            this.udPSMoxDelay = new System.Windows.Forms.NumericUpDownTS();
            this.labelTS2 = new System.Windows.Forms.LabelTS();
            this.udPSPhnum = new System.Windows.Forms.NumericUpDownTS();
            this.grpPSInfo = new System.Windows.Forms.GroupBoxTS();
            this.PSdispRX = new System.Windows.Forms.TextBoxTS();
            this.labelTS6 = new System.Windows.Forms.LabelTS();
            this.GetPSpeak = new System.Windows.Forms.TextBoxTS();
            this.labelTS3 = new System.Windows.Forms.LabelTS();
            this.PSpeak = new System.Windows.Forms.TextBoxTS();
            this.labelTS5 = new System.Windows.Forms.LabelTS();
            this.lblPSfb2 = new System.Windows.Forms.LabelTS();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.lblPSInfo15 = new System.Windows.Forms.LabelTS();
            this.labelTS146 = new System.Windows.Forms.LabelTS();
            this.lblPSInfo3 = new System.Windows.Forms.LabelTS();
            this.lblPSInfo2 = new System.Windows.Forms.LabelTS();
            this.lblPSInfo1 = new System.Windows.Forms.LabelTS();
            this.lblPSInfo0 = new System.Windows.Forms.LabelTS();
            this.labelTS143 = new System.Windows.Forms.LabelTS();
            this.labelTS144 = new System.Windows.Forms.LabelTS();
            this.labelTS142 = new System.Windows.Forms.LabelTS();
            this.labelTS141 = new System.Windows.Forms.LabelTS();
            this.btnPSReset = new System.Windows.Forms.ButtonTS();
            this.btnPSCalibrate = new System.Windows.Forms.ButtonTS();
            this.labelTS140 = new System.Windows.Forms.LabelTS();
            this.udPSCalWait = new System.Windows.Forms.NumericUpDownTS();
            this.btnPSAmpView = new System.Windows.Forms.ButtonTS();
            this.grpLincorSave.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udPSMoxDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPSPhnum)).BeginInit();
            this.grpPSInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udPSCalWait)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblPSWelcome
            // 
            this.lblPSWelcome.AutoSize = true;
            this.lblPSWelcome.Font = new System.Drawing.Font("Cambria", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPSWelcome.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPSWelcome.Location = new System.Drawing.Point(31, 9);
            this.lblPSWelcome.Name = "lblPSWelcome";
            this.lblPSWelcome.Size = new System.Drawing.Size(349, 37);
            this.lblPSWelcome.TabIndex = 33;
            this.lblPSWelcome.Text = "Welcome to PureSignal";
            // 
            // chkPSRelaxPtol
            // 
            this.chkPSRelaxPtol.AutoSize = true;
            this.chkPSRelaxPtol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPSRelaxPtol.ForeColor = System.Drawing.SystemColors.Highlight;
            this.chkPSRelaxPtol.Image = null;
            this.chkPSRelaxPtol.Location = new System.Drawing.Point(38, 368);
            this.chkPSRelaxPtol.Name = "chkPSRelaxPtol";
            this.chkPSRelaxPtol.Size = new System.Drawing.Size(104, 17);
            this.chkPSRelaxPtol.TabIndex = 39;
            this.chkPSRelaxPtol.Text = "Relax Tolerance";
            this.chkPSRelaxPtol.UseVisualStyleBackColor = true;
            this.chkPSRelaxPtol.CheckedChanged += new System.EventHandler(this.chkPSRelaxPtol_CheckedChanged);
            // 
            // lblDisabled
            // 
            this.lblDisabled.AutoSize = true;
            this.lblDisabled.Font = new System.Drawing.Font("Cambria", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisabled.ForeColor = System.Drawing.Color.Crimson;
            this.lblDisabled.Image = null;
            this.lblDisabled.Location = new System.Drawing.Point(153, 80);
            this.lblDisabled.Name = "lblDisabled";
            this.lblDisabled.Size = new System.Drawing.Size(238, 19);
            this.lblDisabled.TabIndex = 38;
            this.lblDisabled.Text = "PureSignal is Disabled in Setup!";
            // 
            // btnPSTwoToneGen
            // 
            this.btnPSTwoToneGen.BackColor = System.Drawing.SystemColors.Control;
            this.btnPSTwoToneGen.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPSTwoToneGen.Image = null;
            this.btnPSTwoToneGen.Location = new System.Drawing.Point(461, 246);
            this.btnPSTwoToneGen.Name = "btnPSTwoToneGen";
            this.btnPSTwoToneGen.Size = new System.Drawing.Size(95, 23);
            this.btnPSTwoToneGen.TabIndex = 37;
            this.btnPSTwoToneGen.Text = "Two-tone Gen";
            this.btnPSTwoToneGen.UseVisualStyleBackColor = false;
            this.btnPSTwoToneGen.Click += new System.EventHandler(this.btnPSTwoToneGen_Click);
            // 
            // labelTS8
            // 
            this.labelTS8.AutoSize = true;
            this.labelTS8.ForeColor = System.Drawing.Color.White;
            this.labelTS8.Image = null;
            this.labelTS8.Location = new System.Drawing.Point(459, 205);
            this.labelTS8.Name = "labelTS8";
            this.labelTS8.Size = new System.Drawing.Size(84, 13);
            this.labelTS8.TabIndex = 10;
            this.labelTS8.Text = "Feedback Level";
            // 
            // lblPSInfoFB
            // 
            this.lblPSInfoFB.BackColor = System.Drawing.Color.Black;
            this.lblPSInfoFB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPSInfoFB.ForeColor = System.Drawing.Color.Black;
            this.lblPSInfoFB.Image = null;
            this.lblPSInfoFB.Location = new System.Drawing.Point(418, 200);
            this.lblPSInfoFB.Name = "lblPSInfoFB";
            this.lblPSInfoFB.Size = new System.Drawing.Size(25, 25);
            this.lblPSInfoFB.TabIndex = 11;
            // 
            // lblPSInfoCO
            // 
            this.lblPSInfoCO.BackColor = System.Drawing.Color.Black;
            this.lblPSInfoCO.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPSInfoCO.ForeColor = System.Drawing.Color.Black;
            this.lblPSInfoCO.Image = null;
            this.lblPSInfoCO.Location = new System.Drawing.Point(38, 200);
            this.lblPSInfoCO.Name = "lblPSInfoCO";
            this.lblPSInfoCO.Size = new System.Drawing.Size(25, 25);
            this.lblPSInfoCO.TabIndex = 13;
            // 
            // lblPSWarning
            // 
            this.lblPSWarning.AutoSize = true;
            this.lblPSWarning.Font = new System.Drawing.Font("Cambria", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPSWarning.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblPSWarning.Image = null;
            this.lblPSWarning.Location = new System.Drawing.Point(35, 21);
            this.lblPSWarning.Name = "lblPSWarning";
            this.lblPSWarning.Size = new System.Drawing.Size(376, 30);
            this.lblPSWarning.TabIndex = 36;
            this.lblPSWarning.Text = "PureSignal requires special hardware setup and caution in use.\r\nIf you are not fa" +
    "miliar with this technology, click \'Information\'.";
            this.lblPSWarning.Visible = false;
            // 
            // labelTS9
            // 
            this.labelTS9.AutoSize = true;
            this.labelTS9.ForeColor = System.Drawing.Color.White;
            this.labelTS9.Image = null;
            this.labelTS9.Location = new System.Drawing.Point(79, 205);
            this.labelTS9.Name = "labelTS9";
            this.labelTS9.Size = new System.Drawing.Size(55, 13);
            this.labelTS9.TabIndex = 12;
            this.labelTS9.Text = "Correcting";
            // 
            // btnPSAutoCalibrate
            // 
            this.btnPSAutoCalibrate.BackColor = System.Drawing.SystemColors.Control;
            this.btnPSAutoCalibrate.Image = null;
            this.btnPSAutoCalibrate.Location = new System.Drawing.Point(38, 80);
            this.btnPSAutoCalibrate.Name = "btnPSAutoCalibrate";
            this.btnPSAutoCalibrate.Size = new System.Drawing.Size(95, 23);
            this.btnPSAutoCalibrate.TabIndex = 35;
            this.btnPSAutoCalibrate.Text = "AutoCalibrate";
            this.btnPSAutoCalibrate.UseVisualStyleBackColor = false;
            this.btnPSAutoCalibrate.Click += new System.EventHandler(this.btnPSAutoCalibrate_Click);
            // 
            // btnPSInformation
            // 
            this.btnPSInformation.BackColor = System.Drawing.SystemColors.Control;
            this.btnPSInformation.Image = null;
            this.btnPSInformation.Location = new System.Drawing.Point(461, 28);
            this.btnPSInformation.Name = "btnPSInformation";
            this.btnPSInformation.Size = new System.Drawing.Size(95, 23);
            this.btnPSInformation.TabIndex = 34;
            this.btnPSInformation.Text = "Information";
            this.btnPSInformation.UseVisualStyleBackColor = false;
            this.btnPSInformation.Click += new System.EventHandler(this.btnPSInformation_Click);
            // 
            // grpLincorSave
            // 
            this.grpLincorSave.Controls.Add(this.btnPSSave);
            this.grpLincorSave.Controls.Add(this.btnPSRestore);
            this.grpLincorSave.ForeColor = System.Drawing.Color.White;
            this.grpLincorSave.Location = new System.Drawing.Point(418, 290);
            this.grpLincorSave.Name = "grpLincorSave";
            this.grpLincorSave.Size = new System.Drawing.Size(176, 95);
            this.grpLincorSave.TabIndex = 32;
            this.grpLincorSave.TabStop = false;
            this.grpLincorSave.Text = "Correction Save / Restore";
            // 
            // btnPSSave
            // 
            this.btnPSSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnPSSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPSSave.Image = null;
            this.btnPSSave.Location = new System.Drawing.Point(43, 22);
            this.btnPSSave.Name = "btnPSSave";
            this.btnPSSave.Size = new System.Drawing.Size(95, 23);
            this.btnPSSave.TabIndex = 4;
            this.btnPSSave.Text = "Save";
            this.btnPSSave.UseVisualStyleBackColor = false;
            this.btnPSSave.Click += new System.EventHandler(this.btnPSSave_Click);
            // 
            // btnPSRestore
            // 
            this.btnPSRestore.BackColor = System.Drawing.SystemColors.Control;
            this.btnPSRestore.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPSRestore.Image = null;
            this.btnPSRestore.Location = new System.Drawing.Point(43, 62);
            this.btnPSRestore.Name = "btnPSRestore";
            this.btnPSRestore.Size = new System.Drawing.Size(95, 23);
            this.btnPSRestore.TabIndex = 0;
            this.btnPSRestore.Text = "Restore";
            this.btnPSRestore.UseVisualStyleBackColor = false;
            this.btnPSRestore.Click += new System.EventHandler(this.btnPSRestore_Click);
            // 
            // labelTS4
            // 
            this.labelTS4.AutoSize = true;
            this.labelTS4.ForeColor = System.Drawing.Color.White;
            this.labelTS4.Image = null;
            this.labelTS4.Location = new System.Drawing.Point(415, 85);
            this.labelTS4.Name = "labelTS4";
            this.labelTS4.Size = new System.Drawing.Size(82, 13);
            this.labelTS4.TabIndex = 30;
            this.labelTS4.Text = "MOX Wait (sec)";
            // 
            // udPSMoxDelay
            // 
            this.udPSMoxDelay.DecimalPlaces = 1;
            this.udPSMoxDelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPSMoxDelay.Location = new System.Drawing.Point(528, 83);
            this.udPSMoxDelay.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.udPSMoxDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPSMoxDelay.Name = "udPSMoxDelay";
            this.udPSMoxDelay.Size = new System.Drawing.Size(66, 20);
            this.udPSMoxDelay.TabIndex = 29;
            this.udPSMoxDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPSMoxDelay.ValueChanged += new System.EventHandler(this.udPSMoxDelay_ValueChanged);
            // 
            // labelTS2
            // 
            this.labelTS2.AutoSize = true;
            this.labelTS2.ForeColor = System.Drawing.Color.White;
            this.labelTS2.Image = null;
            this.labelTS2.Location = new System.Drawing.Point(415, 165);
            this.labelTS2.Name = "labelTS2";
            this.labelTS2.Size = new System.Drawing.Size(96, 13);
            this.labelTS2.TabIndex = 26;
            this.labelTS2.Text = "Amplifier Delay (ns)";
            // 
            // udPSPhnum
            // 
            this.udPSPhnum.Increment = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.udPSPhnum.Location = new System.Drawing.Point(528, 163);
            this.udPSPhnum.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.udPSPhnum.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udPSPhnum.Name = "udPSPhnum";
            this.udPSPhnum.Size = new System.Drawing.Size(66, 20);
            this.udPSPhnum.TabIndex = 25;
            this.udPSPhnum.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udPSPhnum.ValueChanged += new System.EventHandler(this.udPSPhnum_ValueChanged);
            // 
            // grpPSInfo
            // 
            this.grpPSInfo.Controls.Add(this.PSdispRX);
            this.grpPSInfo.Controls.Add(this.labelTS6);
            this.grpPSInfo.Controls.Add(this.GetPSpeak);
            this.grpPSInfo.Controls.Add(this.labelTS3);
            this.grpPSInfo.Controls.Add(this.PSpeak);
            this.grpPSInfo.Controls.Add(this.labelTS5);
            this.grpPSInfo.Controls.Add(this.lblPSfb2);
            this.grpPSInfo.Controls.Add(this.labelTS1);
            this.grpPSInfo.Controls.Add(this.lblPSInfo15);
            this.grpPSInfo.Controls.Add(this.labelTS146);
            this.grpPSInfo.Controls.Add(this.lblPSInfo3);
            this.grpPSInfo.Controls.Add(this.lblPSInfo2);
            this.grpPSInfo.Controls.Add(this.lblPSInfo1);
            this.grpPSInfo.Controls.Add(this.lblPSInfo0);
            this.grpPSInfo.Controls.Add(this.labelTS143);
            this.grpPSInfo.Controls.Add(this.labelTS144);
            this.grpPSInfo.Controls.Add(this.labelTS142);
            this.grpPSInfo.Controls.Add(this.labelTS141);
            this.grpPSInfo.ForeColor = System.Drawing.Color.White;
            this.grpPSInfo.Location = new System.Drawing.Point(164, 125);
            this.grpPSInfo.Name = "grpPSInfo";
            this.grpPSInfo.Size = new System.Drawing.Size(226, 172);
            this.grpPSInfo.TabIndex = 21;
            this.grpPSInfo.TabStop = false;
            this.grpPSInfo.Text = "Calibration Information";
            this.grpPSInfo.Visible = false;
            // 
            // PSdispRX
            // 
            this.PSdispRX.BackColor = System.Drawing.Color.Bisque;
            this.PSdispRX.Location = new System.Drawing.Point(170, 139);
            this.PSdispRX.Name = "PSdispRX";
            this.PSdispRX.Size = new System.Drawing.Size(38, 20);
            this.PSdispRX.TabIndex = 17;
            this.PSdispRX.TextChanged += new System.EventHandler(this.PSdispRX_TextChanged);
            // 
            // labelTS6
            // 
            this.labelTS6.AutoSize = true;
            this.labelTS6.Image = null;
            this.labelTS6.Location = new System.Drawing.Point(6, 142);
            this.labelTS6.Name = "labelTS6";
            this.labelTS6.Size = new System.Drawing.Size(158, 13);
            this.labelTS6.TabIndex = 16;
            this.labelTS6.Text = "Bottom Panadapter Displays RX";
            // 
            // GetPSpeak
            // 
            this.GetPSpeak.BackColor = System.Drawing.Color.Bisque;
            this.GetPSpeak.Location = new System.Drawing.Point(163, 69);
            this.GetPSpeak.Name = "GetPSpeak";
            this.GetPSpeak.Size = new System.Drawing.Size(57, 20);
            this.GetPSpeak.TabIndex = 15;
            // 
            // labelTS3
            // 
            this.labelTS3.AutoSize = true;
            this.labelTS3.Image = null;
            this.labelTS3.Location = new System.Drawing.Point(126, 72);
            this.labelTS3.Name = "labelTS3";
            this.labelTS3.Size = new System.Drawing.Size(37, 13);
            this.labelTS3.TabIndex = 14;
            this.labelTS3.Text = "GetPk";
            // 
            // PSpeak
            // 
            this.PSpeak.BackColor = System.Drawing.Color.Bisque;
            this.PSpeak.Location = new System.Drawing.Point(163, 93);
            this.PSpeak.Name = "PSpeak";
            this.PSpeak.Size = new System.Drawing.Size(57, 20);
            this.PSpeak.TabIndex = 13;
            this.PSpeak.TextChanged += new System.EventHandler(this.PSpeak_TextChanged);
            // 
            // labelTS5
            // 
            this.labelTS5.AutoSize = true;
            this.labelTS5.Image = null;
            this.labelTS5.Location = new System.Drawing.Point(126, 96);
            this.labelTS5.Name = "labelTS5";
            this.labelTS5.Size = new System.Drawing.Size(36, 13);
            this.labelTS5.TabIndex = 12;
            this.labelTS5.Text = "SetPk";
            // 
            // lblPSfb2
            // 
            this.lblPSfb2.AutoSize = true;
            this.lblPSfb2.BackColor = System.Drawing.Color.Bisque;
            this.lblPSfb2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPSfb2.ForeColor = System.Drawing.Color.Black;
            this.lblPSfb2.Image = null;
            this.lblPSfb2.Location = new System.Drawing.Point(185, 48);
            this.lblPSfb2.Name = "lblPSfb2";
            this.lblPSfb2.Size = new System.Drawing.Size(2, 15);
            this.lblPSfb2.TabIndex = 11;
            // 
            // labelTS1
            // 
            this.labelTS1.AutoSize = true;
            this.labelTS1.Image = null;
            this.labelTS1.Location = new System.Drawing.Point(126, 48);
            this.labelTS1.Name = "labelTS1";
            this.labelTS1.Size = new System.Drawing.Size(40, 13);
            this.labelTS1.TabIndex = 10;
            this.labelTS1.Text = "feedbk";
            // 
            // lblPSInfo15
            // 
            this.lblPSInfo15.AutoSize = true;
            this.lblPSInfo15.BackColor = System.Drawing.Color.Bisque;
            this.lblPSInfo15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPSInfo15.ForeColor = System.Drawing.Color.Black;
            this.lblPSInfo15.Image = null;
            this.lblPSInfo15.Location = new System.Drawing.Point(185, 24);
            this.lblPSInfo15.Name = "lblPSInfo15";
            this.lblPSInfo15.Size = new System.Drawing.Size(2, 15);
            this.lblPSInfo15.TabIndex = 9;
            // 
            // labelTS146
            // 
            this.labelTS146.AutoSize = true;
            this.labelTS146.Image = null;
            this.labelTS146.Location = new System.Drawing.Point(126, 24);
            this.labelTS146.Name = "labelTS146";
            this.labelTS146.Size = new System.Drawing.Size(30, 13);
            this.labelTS146.TabIndex = 8;
            this.labelTS146.Text = "state";
            // 
            // lblPSInfo3
            // 
            this.lblPSInfo3.AutoSize = true;
            this.lblPSInfo3.BackColor = System.Drawing.Color.Bisque;
            this.lblPSInfo3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPSInfo3.ForeColor = System.Drawing.Color.Black;
            this.lblPSInfo3.Image = null;
            this.lblPSInfo3.Location = new System.Drawing.Point(72, 96);
            this.lblPSInfo3.Name = "lblPSInfo3";
            this.lblPSInfo3.Size = new System.Drawing.Size(2, 15);
            this.lblPSInfo3.TabIndex = 7;
            // 
            // lblPSInfo2
            // 
            this.lblPSInfo2.AutoSize = true;
            this.lblPSInfo2.BackColor = System.Drawing.Color.Bisque;
            this.lblPSInfo2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPSInfo2.ForeColor = System.Drawing.Color.Black;
            this.lblPSInfo2.Image = null;
            this.lblPSInfo2.Location = new System.Drawing.Point(72, 72);
            this.lblPSInfo2.Name = "lblPSInfo2";
            this.lblPSInfo2.Size = new System.Drawing.Size(2, 15);
            this.lblPSInfo2.TabIndex = 6;
            // 
            // lblPSInfo1
            // 
            this.lblPSInfo1.AutoSize = true;
            this.lblPSInfo1.BackColor = System.Drawing.Color.Bisque;
            this.lblPSInfo1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPSInfo1.ForeColor = System.Drawing.Color.Black;
            this.lblPSInfo1.Image = null;
            this.lblPSInfo1.Location = new System.Drawing.Point(72, 48);
            this.lblPSInfo1.Name = "lblPSInfo1";
            this.lblPSInfo1.Size = new System.Drawing.Size(2, 15);
            this.lblPSInfo1.TabIndex = 5;
            // 
            // lblPSInfo0
            // 
            this.lblPSInfo0.AutoSize = true;
            this.lblPSInfo0.BackColor = System.Drawing.Color.Bisque;
            this.lblPSInfo0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPSInfo0.ForeColor = System.Drawing.Color.Black;
            this.lblPSInfo0.Image = null;
            this.lblPSInfo0.Location = new System.Drawing.Point(72, 24);
            this.lblPSInfo0.Name = "lblPSInfo0";
            this.lblPSInfo0.Size = new System.Drawing.Size(2, 15);
            this.lblPSInfo0.TabIndex = 4;
            // 
            // labelTS143
            // 
            this.labelTS143.AutoSize = true;
            this.labelTS143.Image = null;
            this.labelTS143.Location = new System.Drawing.Point(6, 96);
            this.labelTS143.Name = "labelTS143";
            this.labelTS143.Size = new System.Drawing.Size(38, 13);
            this.labelTS143.TabIndex = 3;
            this.labelTS143.Text = "bldr.cs";
            // 
            // labelTS144
            // 
            this.labelTS144.AutoSize = true;
            this.labelTS144.Image = null;
            this.labelTS144.Location = new System.Drawing.Point(6, 72);
            this.labelTS144.Name = "labelTS144";
            this.labelTS144.Size = new System.Drawing.Size(39, 13);
            this.labelTS144.TabIndex = 2;
            this.labelTS144.Text = "bldr.cc";
            // 
            // labelTS142
            // 
            this.labelTS142.AutoSize = true;
            this.labelTS142.Image = null;
            this.labelTS142.Location = new System.Drawing.Point(6, 48);
            this.labelTS142.Name = "labelTS142";
            this.labelTS142.Size = new System.Drawing.Size(41, 13);
            this.labelTS142.TabIndex = 1;
            this.labelTS142.Text = "bldr.cm";
            // 
            // labelTS141
            // 
            this.labelTS141.AutoSize = true;
            this.labelTS141.Image = null;
            this.labelTS141.Location = new System.Drawing.Point(6, 24);
            this.labelTS141.Name = "labelTS141";
            this.labelTS141.Size = new System.Drawing.Size(35, 13);
            this.labelTS141.TabIndex = 0;
            this.labelTS141.Text = "bldr.rx";
            // 
            // btnPSReset
            // 
            this.btnPSReset.BackColor = System.Drawing.SystemColors.Control;
            this.btnPSReset.Image = null;
            this.btnPSReset.Location = new System.Drawing.Point(38, 160);
            this.btnPSReset.Name = "btnPSReset";
            this.btnPSReset.Size = new System.Drawing.Size(95, 23);
            this.btnPSReset.TabIndex = 20;
            this.btnPSReset.Text = "OFF";
            this.btnPSReset.UseVisualStyleBackColor = false;
            this.btnPSReset.Click += new System.EventHandler(this.btnPSReset_Click);
            // 
            // btnPSCalibrate
            // 
            this.btnPSCalibrate.BackColor = System.Drawing.SystemColors.Control;
            this.btnPSCalibrate.Image = null;
            this.btnPSCalibrate.Location = new System.Drawing.Point(38, 120);
            this.btnPSCalibrate.Name = "btnPSCalibrate";
            this.btnPSCalibrate.Size = new System.Drawing.Size(95, 23);
            this.btnPSCalibrate.TabIndex = 19;
            this.btnPSCalibrate.Text = "Single Calibrate";
            this.btnPSCalibrate.UseVisualStyleBackColor = false;
            this.btnPSCalibrate.Click += new System.EventHandler(this.btnPSCalibrate_Click);
            // 
            // labelTS140
            // 
            this.labelTS140.AutoSize = true;
            this.labelTS140.ForeColor = System.Drawing.Color.White;
            this.labelTS140.Image = null;
            this.labelTS140.Location = new System.Drawing.Point(415, 125);
            this.labelTS140.Name = "labelTS140";
            this.labelTS140.Size = new System.Drawing.Size(107, 13);
            this.labelTS140.TabIndex = 17;
            this.labelTS140.Text = "Calibration Wait (sec)";
            // 
            // udPSCalWait
            // 
            this.udPSCalWait.DecimalPlaces = 1;
            this.udPSCalWait.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPSCalWait.Location = new System.Drawing.Point(528, 123);
            this.udPSCalWait.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udPSCalWait.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udPSCalWait.Name = "udPSCalWait";
            this.udPSCalWait.Size = new System.Drawing.Size(66, 20);
            this.udPSCalWait.TabIndex = 16;
            this.udPSCalWait.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.udPSCalWait.ValueChanged += new System.EventHandler(this.udPSCalWait_ValueChanged);
            // 
            // btnPSAmpView
            // 
            this.btnPSAmpView.BackColor = System.Drawing.SystemColors.Control;
            this.btnPSAmpView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPSAmpView.Image = null;
            this.btnPSAmpView.Location = new System.Drawing.Point(38, 246);
            this.btnPSAmpView.Name = "btnPSAmpView";
            this.btnPSAmpView.Size = new System.Drawing.Size(95, 23);
            this.btnPSAmpView.TabIndex = 40;
            this.btnPSAmpView.Text = "AmpView";
            this.btnPSAmpView.UseVisualStyleBackColor = false;
            this.btnPSAmpView.Click += new System.EventHandler(this.btnPSAmpView_Click);
            // 
            // PSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(627, 397);
            this.Controls.Add(this.btnPSAmpView);
            this.Controls.Add(this.chkPSRelaxPtol);
            this.Controls.Add(this.lblDisabled);
            this.Controls.Add(this.btnPSTwoToneGen);
            this.Controls.Add(this.labelTS8);
            this.Controls.Add(this.lblPSInfoFB);
            this.Controls.Add(this.lblPSInfoCO);
            this.Controls.Add(this.lblPSWarning);
            this.Controls.Add(this.labelTS9);
            this.Controls.Add(this.btnPSAutoCalibrate);
            this.Controls.Add(this.btnPSInformation);
            this.Controls.Add(this.lblPSWelcome);
            this.Controls.Add(this.grpLincorSave);
            this.Controls.Add(this.labelTS4);
            this.Controls.Add(this.udPSMoxDelay);
            this.Controls.Add(this.labelTS2);
            this.Controls.Add(this.udPSPhnum);
            this.Controls.Add(this.grpPSInfo);
            this.Controls.Add(this.btnPSReset);
            this.Controls.Add(this.btnPSCalibrate);
            this.Controls.Add(this.labelTS140);
            this.Controls.Add(this.udPSCalWait);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PSForm";
            this.Text = "PureSignal 1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PSForm_Closing);
            this.Load += new System.EventHandler(this.PSForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PS_KeyDown);
            this.grpLincorSave.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udPSMoxDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPSPhnum)).EndInit();
            this.grpPSInfo.ResumeLayout(false);
            this.grpPSInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udPSCalWait)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBoxTS grpPSInfo;
        private System.Windows.Forms.LabelTS lblPSInfo15;
        private System.Windows.Forms.LabelTS labelTS146;
        private System.Windows.Forms.LabelTS lblPSInfo3;
        private System.Windows.Forms.LabelTS lblPSInfo2;
        private System.Windows.Forms.LabelTS lblPSInfo1;
        private System.Windows.Forms.LabelTS lblPSInfo0;
        private System.Windows.Forms.LabelTS labelTS143;
        private System.Windows.Forms.LabelTS labelTS144;
        private System.Windows.Forms.LabelTS labelTS142;
        private System.Windows.Forms.LabelTS labelTS141;
        private System.Windows.Forms.ButtonTS btnPSReset;
        private System.Windows.Forms.ButtonTS btnPSCalibrate;
        private System.Windows.Forms.LabelTS labelTS140;
        private System.Windows.Forms.NumericUpDownTS udPSCalWait;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.LabelTS labelTS2;
        private System.Windows.Forms.NumericUpDownTS udPSPhnum;
        private System.Windows.Forms.LabelTS labelTS4;
        private System.Windows.Forms.NumericUpDownTS udPSMoxDelay;
        private System.Windows.Forms.GroupBoxTS grpLincorSave;
        private System.Windows.Forms.ButtonTS btnPSSave;
        private System.Windows.Forms.ButtonTS btnPSRestore;
        private System.Windows.Forms.LabelTS lblPSInfoFB;
        private System.Windows.Forms.LabelTS labelTS8;
        private System.Windows.Forms.LabelTS lblPSInfoCO;
        private System.Windows.Forms.LabelTS labelTS9;
        private System.Windows.Forms.Label lblPSWelcome;
        private System.Windows.Forms.ButtonTS btnPSInformation;
        private System.Windows.Forms.ButtonTS btnPSAutoCalibrate;
        private System.Windows.Forms.LabelTS lblPSWarning;
        private System.Windows.Forms.ButtonTS btnPSTwoToneGen;
        private System.Windows.Forms.LabelTS lblPSfb2;
        private System.Windows.Forms.LabelTS labelTS1;
        private System.Windows.Forms.LabelTS lblDisabled;
        private System.Windows.Forms.LabelTS labelTS5;
        private System.Windows.Forms.TextBoxTS PSpeak;
        private System.Windows.Forms.TextBoxTS GetPSpeak;
        private System.Windows.Forms.LabelTS labelTS3;
        private System.Windows.Forms.TextBoxTS PSdispRX;
        private System.Windows.Forms.LabelTS labelTS6;
        private System.Windows.Forms.CheckBoxTS chkPSRelaxPtol;
        private System.Windows.Forms.ButtonTS btnPSAmpView;
    }
}