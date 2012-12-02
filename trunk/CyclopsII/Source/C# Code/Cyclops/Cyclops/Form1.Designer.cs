namespace Cyclops
{
    partial class Form1
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
            this.chkSpec = new System.Windows.Forms.CheckBox();
            this.display_freq = new System.Windows.Forms.TextBox();
            this.set_frequency = new System.Windows.Forms.TrackBar();
            this.BandSelect = new System.Windows.Forms.ComboBox();
            this.stepSize = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SyncLED = new System.Windows.Forms.Button();
            this.chkWideSpec = new System.Windows.Forms.CheckBox();
            this.ADCoverloadButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxSpectrum = new System.Windows.Forms.PictureBox();
            this.pictureBoxWideband = new System.Windows.Forms.PictureBox();
            this.OnOffButton = new System.Windows.Forms.Button();
            this.UpdateRate = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.Smoothing = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Atlas10MHz = new System.Windows.Forms.RadioButton();
            this.Mercury10MHz = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Mercury = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SecondIF = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FirstIFFreq = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.FirstLO = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.Tune = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.set_frequency)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpectrum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWideband)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Smoothing)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkSpec
            // 
            this.chkSpec.AutoSize = true;
            this.chkSpec.Location = new System.Drawing.Point(468, 614);
            this.chkSpec.Name = "chkSpec";
            this.chkSpec.Size = new System.Drawing.Size(101, 17);
            this.chkSpec.TabIndex = 23;
            this.chkSpec.Text = "Show Spectrum";
            this.chkSpec.UseVisualStyleBackColor = true;
            this.chkSpec.CheckedChanged += new System.EventHandler(this.chkSpec_CheckedChanged);
            // 
            // display_freq
            // 
            this.display_freq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.display_freq.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.display_freq.ForeColor = System.Drawing.Color.Crimson;
            this.display_freq.Location = new System.Drawing.Point(410, 49);
            this.display_freq.Name = "display_freq";
            this.display_freq.ReadOnly = true;
            this.display_freq.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.display_freq.Size = new System.Drawing.Size(187, 38);
            this.display_freq.TabIndex = 43;
            // 
            // set_frequency
            // 
            this.set_frequency.LargeChange = 0;
            this.set_frequency.Location = new System.Drawing.Point(4, 564);
            this.set_frequency.Maximum = 0;
            this.set_frequency.Name = "set_frequency";
            this.set_frequency.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.set_frequency.Size = new System.Drawing.Size(1004, 45);
            this.set_frequency.SmallChange = 0;
            this.set_frequency.TabIndex = 44;
            this.set_frequency.TickFrequency = 1000000;
            this.set_frequency.Scroll += new System.EventHandler(this.set_frequency_Scroll_1);
            // 
            // BandSelect
            // 
            this.BandSelect.Location = new System.Drawing.Point(0, 0);
            this.BandSelect.Name = "BandSelect";
            this.BandSelect.Size = new System.Drawing.Size(121, 21);
            this.BandSelect.TabIndex = 110;
            // 
            // stepSize
            // 
            this.stepSize.FormattingEnabled = true;
            this.stepSize.Items.AddRange(new object[] {
            "1Hz",
            "10Hz",
            "100Hz",
            "1kHz"});
            this.stepSize.Location = new System.Drawing.Point(243, 616);
            this.stepSize.Name = "stepSize";
            this.stepSize.Size = new System.Drawing.Size(61, 21);
            this.stepSize.TabIndex = 48;
            this.stepSize.Text = "100Hz";
            this.stepSize.SelectedIndexChanged += new System.EventHandler(this.stepSize_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(185, 619);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 49;
            this.label4.Text = "Step Size";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(447, 573);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 13);
            this.label10.TabIndex = 54;
            this.label10.Text = "Frequency";
            // 
            // SyncLED
            // 
            this.SyncLED.Location = new System.Drawing.Point(81, 614);
            this.SyncLED.Margin = new System.Windows.Forms.Padding(2);
            this.SyncLED.Name = "SyncLED";
            this.SyncLED.Size = new System.Drawing.Size(39, 23);
            this.SyncLED.TabIndex = 63;
            this.SyncLED.Text = "Sync";
            this.SyncLED.UseVisualStyleBackColor = true;
            // 
            // chkWideSpec
            // 
            this.chkWideSpec.AutoSize = true;
            this.chkWideSpec.Location = new System.Drawing.Point(468, 639);
            this.chkWideSpec.Name = "chkWideSpec";
            this.chkWideSpec.Size = new System.Drawing.Size(129, 17);
            this.chkWideSpec.TabIndex = 85;
            this.chkWideSpec.Text = "Show Wide Spectrum";
            this.chkWideSpec.UseVisualStyleBackColor = true;
            this.chkWideSpec.CheckedChanged += new System.EventHandler(this.chkWideSpec_CheckedChanged);
            // 
            // ADCoverloadButton
            // 
            this.ADCoverloadButton.Location = new System.Drawing.Point(128, 614);
            this.ADCoverloadButton.Margin = new System.Windows.Forms.Padding(2);
            this.ADCoverloadButton.Name = "ADCoverloadButton";
            this.ADCoverloadButton.Size = new System.Drawing.Size(41, 23);
            this.ADCoverloadButton.TabIndex = 86;
            this.ADCoverloadButton.Text = "ADC";
            this.ADCoverloadButton.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1036, 24);
            this.menuStrip1.TabIndex = 101;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(12, 20);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(4, 49);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBoxSpectrum);
            this.splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBoxWideband);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(1024, 520);
            this.splitContainer1.SplitterDistance = 256;
            this.splitContainer1.TabIndex = 103;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // pictureBoxSpectrum
            // 
            this.pictureBoxSpectrum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSpectrum.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSpectrum.Name = "pictureBoxSpectrum";
            this.pictureBoxSpectrum.Size = new System.Drawing.Size(1024, 256);
            this.pictureBoxSpectrum.TabIndex = 0;
            this.pictureBoxSpectrum.TabStop = false;
            this.pictureBoxSpectrum.MouseLeave += new System.EventHandler(this.pictureBoxSpectrum_MouseLeave);
            this.pictureBoxSpectrum.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSpectrum_MouseMove);
            this.pictureBoxSpectrum.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSpectrum_MouseDown);
            this.pictureBoxSpectrum.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxSpectrum_Paint);
            this.pictureBoxSpectrum.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSpectrum_MouseUp);
            this.pictureBoxSpectrum.MouseEnter += new System.EventHandler(this.pictureBoxSpectrum_MouseEnter);
            // 
            // pictureBoxWideband
            // 
            this.pictureBoxWideband.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxWideband.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxWideband.Name = "pictureBoxWideband";
            this.pictureBoxWideband.Size = new System.Drawing.Size(1024, 260);
            this.pictureBoxWideband.TabIndex = 0;
            this.pictureBoxWideband.TabStop = false;
            this.pictureBoxWideband.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxWideband_Paint);
            // 
            // OnOffButton
            // 
            this.OnOffButton.Location = new System.Drawing.Point(29, 614);
            this.OnOffButton.Margin = new System.Windows.Forms.Padding(2);
            this.OnOffButton.Name = "OnOffButton";
            this.OnOffButton.Size = new System.Drawing.Size(39, 23);
            this.OnOffButton.TabIndex = 109;
            this.OnOffButton.Text = "OFF";
            this.OnOffButton.UseVisualStyleBackColor = true;
            this.OnOffButton.Click += new System.EventHandler(this.OnOffButton_Click);
            // 
            // UpdateRate
            // 
            this.UpdateRate.Location = new System.Drawing.Point(29, 658);
            this.UpdateRate.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.UpdateRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpdateRate.Name = "UpdateRate";
            this.UpdateRate.Size = new System.Drawing.Size(39, 20);
            this.UpdateRate.TabIndex = 111;
            this.UpdateRate.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 660);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 112;
            this.label1.Text = "Update Rate (FPS)";
            // 
            // Smoothing
            // 
            this.Smoothing.Location = new System.Drawing.Point(29, 684);
            this.Smoothing.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.Smoothing.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Smoothing.Name = "Smoothing";
            this.Smoothing.Size = new System.Drawing.Size(39, 20);
            this.Smoothing.TabIndex = 113;
            this.Smoothing.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.Smoothing.ValueChanged += new System.EventHandler(this.Smoothing_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 686);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 114;
            this.label2.Text = "Smoothing";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Atlas10MHz);
            this.groupBox1.Controls.Add(this.Mercury10MHz);
            this.groupBox1.Location = new System.Drawing.Point(327, 614);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(114, 64);
            this.groupBox1.TabIndex = 115;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "10MHz Reference";
            // 
            // Atlas10MHz
            // 
            this.Atlas10MHz.AutoSize = true;
            this.Atlas10MHz.Checked = true;
            this.Atlas10MHz.Location = new System.Drawing.Point(15, 41);
            this.Atlas10MHz.Name = "Atlas10MHz";
            this.Atlas10MHz.Size = new System.Drawing.Size(48, 17);
            this.Atlas10MHz.TabIndex = 1;
            this.Atlas10MHz.TabStop = true;
            this.Atlas10MHz.Text = "Atlas";
            this.Atlas10MHz.UseVisualStyleBackColor = true;
            // 
            // Mercury10MHz
            // 
            this.Mercury10MHz.AutoSize = true;
            this.Mercury10MHz.Location = new System.Drawing.Point(15, 20);
            this.Mercury10MHz.Name = "Mercury10MHz";
            this.Mercury10MHz.Size = new System.Drawing.Size(63, 17);
            this.Mercury10MHz.TabIndex = 0;
            this.Mercury10MHz.Text = "Mercury";
            this.Mercury10MHz.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(940, 720);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 130;
            this.label3.Text = "Hz";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(817, 720);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 129;
            this.label9.Text = "Mercury";
            // 
            // Mercury
            // 
            this.Mercury.Location = new System.Drawing.Point(868, 717);
            this.Mercury.Name = "Mercury";
            this.Mercury.Size = new System.Drawing.Size(66, 20);
            this.Mercury.TabIndex = 128;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(773, 694);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 127;
            this.label8.Text = "2nd IF  Frequency";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(940, 694);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 126;
            this.label7.Text = "Hz";
            // 
            // SecondIF
            // 
            this.SecondIF.Location = new System.Drawing.Point(868, 691);
            this.SecondIF.Name = "SecondIF";
            this.SecondIF.Size = new System.Drawing.Size(66, 20);
            this.SecondIF.TabIndex = 125;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(777, 668);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 124;
            this.label6.Text = "1st IF  Frequency";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(946, 668);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 123;
            this.label5.Text = "MHz";
            // 
            // FirstIFFreq
            // 
            this.FirstIFFreq.Location = new System.Drawing.Point(868, 665);
            this.FirstIFFreq.Name = "FirstIFFreq";
            this.FirstIFFreq.Size = new System.Drawing.Size(77, 20);
            this.FirstIFFreq.TabIndex = 122;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(940, 642);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 13);
            this.label11.TabIndex = 121;
            this.label11.Text = "MHz";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(777, 642);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 13);
            this.label12.TabIndex = 120;
            this.label12.Text = "1st LO Frequency";
            // 
            // FirstLO
            // 
            this.FirstLO.Location = new System.Drawing.Point(868, 639);
            this.FirstLO.Name = "FirstLO";
            this.FirstLO.Size = new System.Drawing.Size(66, 20);
            this.FirstLO.TabIndex = 119;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(940, 616);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 13);
            this.label13.TabIndex = 118;
            this.label13.Text = "MHz";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(777, 616);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 13);
            this.label14.TabIndex = 117;
            this.label14.Text = "Tune Frequency";
            // 
            // Tune
            // 
            this.Tune.Location = new System.Drawing.Point(868, 613);
            this.Tune.Name = "Tune";
            this.Tune.Size = new System.Drawing.Size(66, 20);
            this.Tune.TabIndex = 116;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1036, 766);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Mercury);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.SecondIF);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FirstIFFreq);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.FirstLO);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.Tune);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Smoothing);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UpdateRate);
            this.Controls.Add(this.OnOffButton);
            this.Controls.Add(this.display_freq);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ADCoverloadButton);
            this.Controls.Add(this.chkWideSpec);
            this.Controls.Add(this.SyncLED);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.stepSize);
            this.Controls.Add(this.BandSelect);
            this.Controls.Add(this.set_frequency);
            this.Controls.Add(this.chkSpec);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Cyclops - VK6APH";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.set_frequency)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpectrum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWideband)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Smoothing)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSpec;
        private System.Windows.Forms.TextBox display_freq;
        private System.Windows.Forms.TrackBar set_frequency;
        private System.Windows.Forms.ComboBox BandSelect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button SyncLED;
        private System.Windows.Forms.CheckBox chkWideSpec;
        private System.Windows.Forms.Button ADCoverloadButton;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBoxSpectrum;
        private System.Windows.Forms.PictureBox pictureBoxWideband;
        private System.Windows.Forms.Button OnOffButton;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown UpdateRate;
        public System.Windows.Forms.ComboBox stepSize;
        private System.Windows.Forms.NumericUpDown Smoothing;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Atlas10MHz;
        private System.Windows.Forms.RadioButton Mercury10MHz;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox Mercury;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox SecondIF;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox FirstIFFreq;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox FirstLO;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox Tune;
    }
}

