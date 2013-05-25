namespace PowerSDR
{
    partial class LincorForm
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblDelay = new System.Windows.Forms.LabelTS();
            this.labelTS3 = new System.Windows.Forms.LabelTS();
            this.labelTS2 = new System.Windows.Forms.LabelTS();
            this.udDSPXLPhnum = new System.Windows.Forms.NumericUpDownTS();
            this.grpLincorPS = new System.Windows.Forms.GroupBoxTS();
            this.btnDSPXLPrintSamples = new System.Windows.Forms.ButtonTS();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.txtSamplesFile = new System.Windows.Forms.TextBoxTS();
            this.chkDSPXLTwoTone = new System.Windows.Forms.CheckBoxTS();
            this.btnDSPTTCal = new System.Windows.Forms.ButtonTS();
            this.grpLincorInfo = new System.Windows.Forms.GroupBoxTS();
            this.lblInfo4 = new System.Windows.Forms.LabelTS();
            this.labelTS146 = new System.Windows.Forms.LabelTS();
            this.lblInfo3 = new System.Windows.Forms.LabelTS();
            this.lblInfo2 = new System.Windows.Forms.LabelTS();
            this.lblInfo1 = new System.Windows.Forms.LabelTS();
            this.lblInfo0 = new System.Windows.Forms.LabelTS();
            this.labelTS143 = new System.Windows.Forms.LabelTS();
            this.labelTS144 = new System.Windows.Forms.LabelTS();
            this.labelTS142 = new System.Windows.Forms.LabelTS();
            this.labelTS141 = new System.Windows.Forms.LabelTS();
            this.btnDSPXLReset = new System.Windows.Forms.ButtonTS();
            this.btnDSPXLCalibrate = new System.Windows.Forms.ButtonTS();
            this.chkDSPXLAutoCorrect = new System.Windows.Forms.CheckBoxTS();
            this.labelTS140 = new System.Windows.Forms.LabelTS();
            this.udDSPXLCalWait = new System.Windows.Forms.NumericUpDownTS();
            this.labelTS139 = new System.Windows.Forms.LabelTS();
            this.udDSPXLNsamps = new System.Windows.Forms.NumericUpDownTS();
            this.labelTS133 = new System.Windows.Forms.LabelTS();
            this.udDSPXLIntervals = new System.Windows.Forms.NumericUpDownTS();
            this.labelTS4 = new System.Windows.Forms.LabelTS();
            this.udDSPXLMoxDelay = new System.Windows.Forms.NumericUpDownTS();
            ((System.ComponentModel.ISupportInitialize)(this.udDSPXLPhnum)).BeginInit();
            this.grpLincorPS.SuspendLayout();
            this.grpLincorInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udDSPXLCalWait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDSPXLNsamps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDSPXLIntervals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDSPXLMoxDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.BackColor = System.Drawing.Color.Bisque;
            this.lblDelay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDelay.Image = null;
            this.lblDelay.Location = new System.Drawing.Point(519, 218);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(36, 15);
            this.lblDelay.TabIndex = 28;
            this.lblDelay.Text = "Delay";
            // 
            // labelTS3
            // 
            this.labelTS3.AutoSize = true;
            this.labelTS3.Image = null;
            this.labelTS3.Location = new System.Drawing.Point(426, 218);
            this.labelTS3.Name = "labelTS3";
            this.labelTS3.Size = new System.Drawing.Size(54, 13);
            this.labelTS3.TabIndex = 27;
            this.labelTS3.Text = "Delay (ns)";
            // 
            // labelTS2
            // 
            this.labelTS2.AutoSize = true;
            this.labelTS2.Image = null;
            this.labelTS2.Location = new System.Drawing.Point(426, 173);
            this.labelTS2.Name = "labelTS2";
            this.labelTS2.Size = new System.Drawing.Size(72, 13);
            this.labelTS2.TabIndex = 26;
            this.labelTS2.Text = "Delay number";
            // 
            // udDSPXLPhnum
            // 
            this.udDSPXLPhnum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDSPXLPhnum.Location = new System.Drawing.Point(522, 171);
            this.udDSPXLPhnum.Maximum = new decimal(new int[] {
            1040,
            0,
            0,
            0});
            this.udDSPXLPhnum.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDSPXLPhnum.Name = "udDSPXLPhnum";
            this.udDSPXLPhnum.Size = new System.Drawing.Size(66, 20);
            this.udDSPXLPhnum.TabIndex = 25;
            this.udDSPXLPhnum.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDSPXLPhnum.ValueChanged += new System.EventHandler(this.udDSPXLPhnum_ValueChanged);
            // 
            // grpLincorPS
            // 
            this.grpLincorPS.Controls.Add(this.btnDSPXLPrintSamples);
            this.grpLincorPS.Controls.Add(this.labelTS1);
            this.grpLincorPS.Controls.Add(this.txtSamplesFile);
            this.grpLincorPS.Location = new System.Drawing.Point(388, 255);
            this.grpLincorPS.Name = "grpLincorPS";
            this.grpLincorPS.Size = new System.Drawing.Size(200, 123);
            this.grpLincorPS.TabIndex = 24;
            this.grpLincorPS.TabStop = false;
            this.grpLincorPS.Text = "Print Samples";
            // 
            // btnDSPXLPrintSamples
            // 
            this.btnDSPXLPrintSamples.Image = null;
            this.btnDSPXLPrintSamples.Location = new System.Drawing.Point(64, 50);
            this.btnDSPXLPrintSamples.Name = "btnDSPXLPrintSamples";
            this.btnDSPXLPrintSamples.Size = new System.Drawing.Size(75, 23);
            this.btnDSPXLPrintSamples.TabIndex = 2;
            this.btnDSPXLPrintSamples.Text = "Print";
            this.btnDSPXLPrintSamples.UseVisualStyleBackColor = true;
            this.btnDSPXLPrintSamples.Click += new System.EventHandler(this.btnDSPXLPrintSamples_Click);
            // 
            // labelTS1
            // 
            this.labelTS1.AutoSize = true;
            this.labelTS1.Image = null;
            this.labelTS1.Location = new System.Drawing.Point(6, 27);
            this.labelTS1.Name = "labelTS1";
            this.labelTS1.Size = new System.Drawing.Size(57, 13);
            this.labelTS1.TabIndex = 1;
            this.labelTS1.Text = "File Name:";
            // 
            // txtSamplesFile
            // 
            this.txtSamplesFile.Location = new System.Drawing.Point(94, 24);
            this.txtSamplesFile.Name = "txtSamplesFile";
            this.txtSamplesFile.Size = new System.Drawing.Size(100, 20);
            this.txtSamplesFile.TabIndex = 0;
            this.txtSamplesFile.Text = "samples";
            // 
            // chkDSPXLTwoTone
            // 
            this.chkDSPXLTwoTone.AutoSize = true;
            this.chkDSPXLTwoTone.Image = null;
            this.chkDSPXLTwoTone.Location = new System.Drawing.Point(223, 63);
            this.chkDSPXLTwoTone.Name = "chkDSPXLTwoTone";
            this.chkDSPXLTwoTone.Size = new System.Drawing.Size(98, 17);
            this.chkDSPXLTwoTone.TabIndex = 23;
            this.chkDSPXLTwoTone.Text = "Gen Two Tone";
            this.chkDSPXLTwoTone.UseVisualStyleBackColor = true;
            this.chkDSPXLTwoTone.CheckedChanged += new System.EventHandler(this.chkDSPXLTwoTone_CheckedChanged);
            // 
            // btnDSPTTCal
            // 
            this.btnDSPTTCal.Image = null;
            this.btnDSPTTCal.Location = new System.Drawing.Point(38, 131);
            this.btnDSPTTCal.Name = "btnDSPTTCal";
            this.btnDSPTTCal.Size = new System.Drawing.Size(85, 23);
            this.btnDSPTTCal.TabIndex = 22;
            this.btnDSPTTCal.Text = "TT Cal->ON";
            this.btnDSPTTCal.UseVisualStyleBackColor = true;
            this.btnDSPTTCal.Click += new System.EventHandler(this.btnDSPTTCal_Click);
            // 
            // grpLincorInfo
            // 
            this.grpLincorInfo.Controls.Add(this.lblInfo4);
            this.grpLincorInfo.Controls.Add(this.labelTS146);
            this.grpLincorInfo.Controls.Add(this.lblInfo3);
            this.grpLincorInfo.Controls.Add(this.lblInfo2);
            this.grpLincorInfo.Controls.Add(this.lblInfo1);
            this.grpLincorInfo.Controls.Add(this.lblInfo0);
            this.grpLincorInfo.Controls.Add(this.labelTS143);
            this.grpLincorInfo.Controls.Add(this.labelTS144);
            this.grpLincorInfo.Controls.Add(this.labelTS142);
            this.grpLincorInfo.Controls.Add(this.labelTS141);
            this.grpLincorInfo.Location = new System.Drawing.Point(38, 255);
            this.grpLincorInfo.Name = "grpLincorInfo";
            this.grpLincorInfo.Size = new System.Drawing.Size(226, 123);
            this.grpLincorInfo.TabIndex = 21;
            this.grpLincorInfo.TabStop = false;
            this.grpLincorInfo.Text = "Calibration Information";
            // 
            // lblInfo4
            // 
            this.lblInfo4.AutoSize = true;
            this.lblInfo4.BackColor = System.Drawing.Color.Bisque;
            this.lblInfo4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInfo4.ForeColor = System.Drawing.Color.Black;
            this.lblInfo4.Image = null;
            this.lblInfo4.Location = new System.Drawing.Point(185, 24);
            this.lblInfo4.Name = "lblInfo4";
            this.lblInfo4.Size = new System.Drawing.Size(32, 15);
            this.lblInfo4.TabIndex = 9;
            this.lblInfo4.Text = "info4";
            // 
            // labelTS146
            // 
            this.labelTS146.AutoSize = true;
            this.labelTS146.Image = null;
            this.labelTS146.Location = new System.Drawing.Point(126, 24);
            this.labelTS146.Name = "labelTS146";
            this.labelTS146.Size = new System.Drawing.Size(45, 13);
            this.labelTS146.TabIndex = 8;
            this.labelTS146.Text = "cc.state";
            // 
            // lblInfo3
            // 
            this.lblInfo3.AutoSize = true;
            this.lblInfo3.BackColor = System.Drawing.Color.Bisque;
            this.lblInfo3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInfo3.ForeColor = System.Drawing.Color.Black;
            this.lblInfo3.Image = null;
            this.lblInfo3.Location = new System.Drawing.Point(72, 96);
            this.lblInfo3.Name = "lblInfo3";
            this.lblInfo3.Size = new System.Drawing.Size(32, 15);
            this.lblInfo3.TabIndex = 7;
            this.lblInfo3.Text = "info3";
            // 
            // lblInfo2
            // 
            this.lblInfo2.AutoSize = true;
            this.lblInfo2.BackColor = System.Drawing.Color.Bisque;
            this.lblInfo2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInfo2.ForeColor = System.Drawing.Color.Black;
            this.lblInfo2.Image = null;
            this.lblInfo2.Location = new System.Drawing.Point(72, 72);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(32, 15);
            this.lblInfo2.TabIndex = 6;
            this.lblInfo2.Text = "info2";
            // 
            // lblInfo1
            // 
            this.lblInfo1.AutoSize = true;
            this.lblInfo1.BackColor = System.Drawing.Color.Bisque;
            this.lblInfo1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInfo1.ForeColor = System.Drawing.Color.Black;
            this.lblInfo1.Image = null;
            this.lblInfo1.Location = new System.Drawing.Point(72, 48);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(32, 15);
            this.lblInfo1.TabIndex = 5;
            this.lblInfo1.Text = "info1";
            // 
            // lblInfo0
            // 
            this.lblInfo0.AutoSize = true;
            this.lblInfo0.BackColor = System.Drawing.Color.Bisque;
            this.lblInfo0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInfo0.ForeColor = System.Drawing.Color.Black;
            this.lblInfo0.Image = null;
            this.lblInfo0.Location = new System.Drawing.Point(72, 24);
            this.lblInfo0.Name = "lblInfo0";
            this.lblInfo0.Size = new System.Drawing.Size(32, 15);
            this.lblInfo0.TabIndex = 4;
            this.lblInfo0.Text = "info0";
            // 
            // labelTS143
            // 
            this.labelTS143.AutoSize = true;
            this.labelTS143.Image = null;
            this.labelTS143.Location = new System.Drawing.Point(6, 96);
            this.labelTS143.Name = "labelTS143";
            this.labelTS143.Size = new System.Drawing.Size(46, 13);
            this.labelTS143.TabIndex = 3;
            this.labelTS143.Text = "bldr.csin";
            // 
            // labelTS144
            // 
            this.labelTS144.AutoSize = true;
            this.labelTS144.Image = null;
            this.labelTS144.Location = new System.Drawing.Point(6, 72);
            this.labelTS144.Name = "labelTS144";
            this.labelTS144.Size = new System.Drawing.Size(50, 13);
            this.labelTS144.TabIndex = 2;
            this.labelTS144.Text = "bldr.ccos";
            // 
            // labelTS142
            // 
            this.labelTS142.AutoSize = true;
            this.labelTS142.Image = null;
            this.labelTS142.Location = new System.Drawing.Point(6, 48);
            this.labelTS142.Name = "labelTS142";
            this.labelTS142.Size = new System.Drawing.Size(53, 13);
            this.labelTS142.TabIndex = 1;
            this.labelTS142.Text = "bldr.cmag";
            // 
            // labelTS141
            // 
            this.labelTS141.AutoSize = true;
            this.labelTS141.Image = null;
            this.labelTS141.Location = new System.Drawing.Point(6, 24);
            this.labelTS141.Name = "labelTS141";
            this.labelTS141.Size = new System.Drawing.Size(60, 13);
            this.labelTS141.TabIndex = 0;
            this.labelTS141.Text = "bldr.rxscale";
            // 
            // btnDSPXLReset
            // 
            this.btnDSPXLReset.Image = null;
            this.btnDSPXLReset.Location = new System.Drawing.Point(38, 168);
            this.btnDSPXLReset.Name = "btnDSPXLReset";
            this.btnDSPXLReset.Size = new System.Drawing.Size(85, 23);
            this.btnDSPXLReset.TabIndex = 20;
            this.btnDSPXLReset.Text = "RESET->OFF";
            this.btnDSPXLReset.UseVisualStyleBackColor = true;
            this.btnDSPXLReset.Click += new System.EventHandler(this.btnDSPXLReset_Click);
            // 
            // btnDSPXLCalibrate
            // 
            this.btnDSPXLCalibrate.Image = null;
            this.btnDSPXLCalibrate.Location = new System.Drawing.Point(38, 95);
            this.btnDSPXLCalibrate.Name = "btnDSPXLCalibrate";
            this.btnDSPXLCalibrate.Size = new System.Drawing.Size(86, 23);
            this.btnDSPXLCalibrate.TabIndex = 19;
            this.btnDSPXLCalibrate.Text = "Calibrate->ON";
            this.btnDSPXLCalibrate.UseVisualStyleBackColor = true;
            this.btnDSPXLCalibrate.Click += new System.EventHandler(this.btnDSPXLCalibrate_Click);
            // 
            // chkDSPXLAutoCorrect
            // 
            this.chkDSPXLAutoCorrect.AutoSize = true;
            this.chkDSPXLAutoCorrect.Image = null;
            this.chkDSPXLAutoCorrect.Location = new System.Drawing.Point(38, 65);
            this.chkDSPXLAutoCorrect.Name = "chkDSPXLAutoCorrect";
            this.chkDSPXLAutoCorrect.Size = new System.Drawing.Size(85, 17);
            this.chkDSPXLAutoCorrect.TabIndex = 18;
            this.chkDSPXLAutoCorrect.Text = "Auto-Correct";
            this.chkDSPXLAutoCorrect.UseVisualStyleBackColor = true;
            this.chkDSPXLAutoCorrect.CheckedChanged += new System.EventHandler(this.chkDSPXLAutoCorrect_CheckedChanged);
            // 
            // labelTS140
            // 
            this.labelTS140.AutoSize = true;
            this.labelTS140.Image = null;
            this.labelTS140.Location = new System.Drawing.Point(426, 136);
            this.labelTS140.Name = "labelTS140";
            this.labelTS140.Size = new System.Drawing.Size(81, 13);
            this.labelTS140.TabIndex = 17;
            this.labelTS140.Text = "Calibration Wait";
            // 
            // udDSPXLCalWait
            // 
            this.udDSPXLCalWait.DecimalPlaces = 1;
            this.udDSPXLCalWait.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udDSPXLCalWait.Location = new System.Drawing.Point(522, 134);
            this.udDSPXLCalWait.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udDSPXLCalWait.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDSPXLCalWait.Name = "udDSPXLCalWait";
            this.udDSPXLCalWait.Size = new System.Drawing.Size(66, 20);
            this.udDSPXLCalWait.TabIndex = 16;
            this.udDSPXLCalWait.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.udDSPXLCalWait.ValueChanged += new System.EventHandler(this.udDSPXLCalWait_ValueChanged);
            // 
            // labelTS139
            // 
            this.labelTS139.AutoSize = true;
            this.labelTS139.Image = null;
            this.labelTS139.Location = new System.Drawing.Point(426, 100);
            this.labelTS139.Name = "labelTS139";
            this.labelTS139.Size = new System.Drawing.Size(87, 13);
            this.labelTS139.TabIndex = 15;
            this.labelTS139.Text = "Samples/Interval";
            // 
            // udDSPXLNsamps
            // 
            this.udDSPXLNsamps.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDSPXLNsamps.Location = new System.Drawing.Point(522, 98);
            this.udDSPXLNsamps.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udDSPXLNsamps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDSPXLNsamps.Name = "udDSPXLNsamps";
            this.udDSPXLNsamps.Size = new System.Drawing.Size(66, 20);
            this.udDSPXLNsamps.TabIndex = 14;
            this.udDSPXLNsamps.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.udDSPXLNsamps.ValueChanged += new System.EventHandler(this.udDSPXLNsamps_ValueChanged);
            // 
            // labelTS133
            // 
            this.labelTS133.AutoSize = true;
            this.labelTS133.Image = null;
            this.labelTS133.Location = new System.Drawing.Point(426, 64);
            this.labelTS133.Name = "labelTS133";
            this.labelTS133.Size = new System.Drawing.Size(79, 13);
            this.labelTS133.TabIndex = 13;
            this.labelTS133.Text = "Spline Intervals";
            // 
            // udDSPXLIntervals
            // 
            this.udDSPXLIntervals.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDSPXLIntervals.Location = new System.Drawing.Point(522, 62);
            this.udDSPXLIntervals.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.udDSPXLIntervals.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDSPXLIntervals.Name = "udDSPXLIntervals";
            this.udDSPXLIntervals.Size = new System.Drawing.Size(66, 20);
            this.udDSPXLIntervals.TabIndex = 12;
            this.udDSPXLIntervals.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.udDSPXLIntervals.ValueChanged += new System.EventHandler(this.udDSPXLIntervals_ValueChanged);
            // 
            // labelTS4
            // 
            this.labelTS4.AutoSize = true;
            this.labelTS4.Image = null;
            this.labelTS4.Location = new System.Drawing.Point(220, 136);
            this.labelTS4.Name = "labelTS4";
            this.labelTS4.Size = new System.Drawing.Size(61, 13);
            this.labelTS4.TabIndex = 30;
            this.labelTS4.Text = "MOX Delay";
            // 
            // udDSPXLMoxDelay
            // 
            this.udDSPXLMoxDelay.DecimalPlaces = 1;
            this.udDSPXLMoxDelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udDSPXLMoxDelay.Location = new System.Drawing.Point(316, 134);
            this.udDSPXLMoxDelay.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.udDSPXLMoxDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udDSPXLMoxDelay.Name = "udDSPXLMoxDelay";
            this.udDSPXLMoxDelay.Size = new System.Drawing.Size(66, 20);
            this.udDSPXLMoxDelay.TabIndex = 29;
            this.udDSPXLMoxDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udDSPXLMoxDelay.ValueChanged += new System.EventHandler(this.udDSPXLMoxDelay_ValueChanged);
            // 
            // LincorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 397);
            this.Controls.Add(this.labelTS4);
            this.Controls.Add(this.udDSPXLMoxDelay);
            this.Controls.Add(this.lblDelay);
            this.Controls.Add(this.labelTS3);
            this.Controls.Add(this.labelTS2);
            this.Controls.Add(this.udDSPXLPhnum);
            this.Controls.Add(this.grpLincorPS);
            this.Controls.Add(this.chkDSPXLTwoTone);
            this.Controls.Add(this.btnDSPTTCal);
            this.Controls.Add(this.grpLincorInfo);
            this.Controls.Add(this.btnDSPXLReset);
            this.Controls.Add(this.btnDSPXLCalibrate);
            this.Controls.Add(this.chkDSPXLAutoCorrect);
            this.Controls.Add(this.labelTS140);
            this.Controls.Add(this.udDSPXLCalWait);
            this.Controls.Add(this.labelTS139);
            this.Controls.Add(this.udDSPXLNsamps);
            this.Controls.Add(this.labelTS133);
            this.Controls.Add(this.udDSPXLIntervals);
            this.Name = "LincorForm";
            this.Text = "Linearity Correction";
            ((System.ComponentModel.ISupportInitialize)(this.udDSPXLPhnum)).EndInit();
            this.grpLincorPS.ResumeLayout(false);
            this.grpLincorPS.PerformLayout();
            this.grpLincorInfo.ResumeLayout(false);
            this.grpLincorInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udDSPXLCalWait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDSPXLNsamps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDSPXLIntervals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDSPXLMoxDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBoxTS chkDSPXLTwoTone;
        private System.Windows.Forms.ButtonTS btnDSPTTCal;
        private System.Windows.Forms.GroupBoxTS grpLincorInfo;
        private System.Windows.Forms.LabelTS lblInfo4;
        private System.Windows.Forms.LabelTS labelTS146;
        private System.Windows.Forms.LabelTS lblInfo3;
        private System.Windows.Forms.LabelTS lblInfo2;
        private System.Windows.Forms.LabelTS lblInfo1;
        private System.Windows.Forms.LabelTS lblInfo0;
        private System.Windows.Forms.LabelTS labelTS143;
        private System.Windows.Forms.LabelTS labelTS144;
        private System.Windows.Forms.LabelTS labelTS142;
        private System.Windows.Forms.LabelTS labelTS141;
        private System.Windows.Forms.ButtonTS btnDSPXLReset;
        private System.Windows.Forms.ButtonTS btnDSPXLCalibrate;
        private System.Windows.Forms.CheckBoxTS chkDSPXLAutoCorrect;
        private System.Windows.Forms.LabelTS labelTS140;
        private System.Windows.Forms.NumericUpDownTS udDSPXLCalWait;
        private System.Windows.Forms.LabelTS labelTS139;
        private System.Windows.Forms.NumericUpDownTS udDSPXLNsamps;
        private System.Windows.Forms.LabelTS labelTS133;
        private System.Windows.Forms.NumericUpDownTS udDSPXLIntervals;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBoxTS grpLincorPS;
        private System.Windows.Forms.TextBoxTS txtSamplesFile;
        private System.Windows.Forms.ButtonTS btnDSPXLPrintSamples;
        private System.Windows.Forms.LabelTS labelTS1;
        private System.Windows.Forms.LabelTS labelTS2;
        private System.Windows.Forms.NumericUpDownTS udDSPXLPhnum;
        private System.Windows.Forms.LabelTS labelTS3;
        private System.Windows.Forms.LabelTS lblDelay;
        private System.Windows.Forms.LabelTS labelTS4;
        private System.Windows.Forms.NumericUpDownTS udDSPXLMoxDelay;
    }
}