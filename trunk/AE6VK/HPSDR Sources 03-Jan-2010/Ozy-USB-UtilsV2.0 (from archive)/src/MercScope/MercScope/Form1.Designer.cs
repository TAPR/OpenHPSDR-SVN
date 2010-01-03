namespace MercScope
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.vScrollBar2 = new System.Windows.Forms.VScrollBar();
            this.vScrollBar3 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.chkI = new System.Windows.Forms.CheckBox();
            this.chkQ = new System.Windows.Forms.CheckBox();
            this.chkSpec = new System.Windows.Forms.CheckBox();
            this.txtMax = new System.Windows.Forms.TextBox();
            this.txtMin = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtQMin = new System.Windows.Forms.TextBox();
            this.txtQMax = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtPSMin = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtPSMax = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPMin = new System.Windows.Forms.TextBox();
            this.txtPMax = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.nudSpecScale = new System.Windows.Forms.NumericUpDown();
            this.hscrollGain = new System.Windows.Forms.HScrollBar();
            this.hscrollPhase = new System.Windows.Forms.HScrollBar();
            this.txtSR = new System.Windows.Forms.TextBox();
            this.btnSendSR = new System.Windows.Forms.Button();
            this.btnRd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpecScale)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(31, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 512);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(546, 24);
            this.vScrollBar1.Maximum = 500;
            this.vScrollBar1.Minimum = -500;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 500);
            this.vScrollBar1.TabIndex = 1;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(385, 543);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(41, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(31, 524);
            this.hScrollBar1.Maximum = 10;
            this.hScrollBar1.Minimum = -1000;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(515, 14);
            this.hScrollBar1.TabIndex = 3;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(273, 543);
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(43, 20);
            this.numericUpDown2.TabIndex = 4;
            this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(208, 548);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Horz Scale";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(334, 548);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "IQScale";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(31, 594);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(113, 134);
            this.listBox1.TabIndex = 7;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(31, 542);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(75, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "ADC RUN";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(112, 541);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 9;
            this.button1.Text = "Single";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(430, 698);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(41, 20);
            this.numericUpDown3.TabIndex = 10;
            this.numericUpDown3.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            // 
            // vScrollBar2
            // 
            this.vScrollBar2.Location = new System.Drawing.Point(563, 24);
            this.vScrollBar2.Maximum = 500;
            this.vScrollBar2.Minimum = -500;
            this.vScrollBar2.Name = "vScrollBar2";
            this.vScrollBar2.Size = new System.Drawing.Size(17, 500);
            this.vScrollBar2.TabIndex = 11;
            // 
            // vScrollBar3
            // 
            this.vScrollBar3.Location = new System.Drawing.Point(580, 24);
            this.vScrollBar3.Maximum = 500;
            this.vScrollBar3.Minimum = -500;
            this.vScrollBar3.Name = "vScrollBar3";
            this.vScrollBar3.Size = new System.Drawing.Size(17, 500);
            this.vScrollBar3.TabIndex = 12;
            // 
            // hScrollBar2
            // 
            this.hScrollBar2.LargeChange = 101;
            this.hScrollBar2.Location = new System.Drawing.Point(31, 577);
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(566, 14);
            this.hScrollBar2.SmallChange = 100;
            this.hScrollBar2.TabIndex = 13;
            this.hScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Red;
            this.textBox1.Location = new System.Drawing.Point(359, 608);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(74, 20);
            this.textBox1.TabIndex = 14;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(359, 638);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(74, 20);
            this.textBox2.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(439, 611);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "MHz";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(439, 641);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "FPDW";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(356, 701);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Update Rate";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(359, 664);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(92, 17);
            this.checkBox2.TabIndex = 20;
            this.checkBox2.Text = "Random Data";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // chkI
            // 
            this.chkI.AutoSize = true;
            this.chkI.Checked = true;
            this.chkI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkI.Location = new System.Drawing.Point(485, 611);
            this.chkI.Name = "chkI";
            this.chkI.Size = new System.Drawing.Size(101, 17);
            this.chkI.TabIndex = 21;
            this.chkI.Text = "Show I Channel";
            this.chkI.UseVisualStyleBackColor = true;
            // 
            // chkQ
            // 
            this.chkQ.AutoSize = true;
            this.chkQ.Checked = true;
            this.chkQ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkQ.Location = new System.Drawing.Point(485, 633);
            this.chkQ.Name = "chkQ";
            this.chkQ.Size = new System.Drawing.Size(106, 17);
            this.chkQ.TabIndex = 22;
            this.chkQ.Text = "Show Q Channel";
            this.chkQ.UseVisualStyleBackColor = true;
            // 
            // chkSpec
            // 
            this.chkSpec.AutoSize = true;
            this.chkSpec.Checked = true;
            this.chkSpec.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSpec.Location = new System.Drawing.Point(485, 656);
            this.chkSpec.Name = "chkSpec";
            this.chkSpec.Size = new System.Drawing.Size(101, 17);
            this.chkSpec.TabIndex = 23;
            this.chkSpec.Text = "Show Spectrum";
            this.chkSpec.UseVisualStyleBackColor = true;
            // 
            // txtMax
            // 
            this.txtMax.Location = new System.Drawing.Point(151, 594);
            this.txtMax.Name = "txtMax";
            this.txtMax.Size = new System.Drawing.Size(52, 20);
            this.txtMax.TabIndex = 24;
            // 
            // txtMin
            // 
            this.txtMin.Location = new System.Drawing.Point(150, 620);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(52, 20);
            this.txtMin.TabIndex = 25;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(208, 597);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "IMax";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(208, 623);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "IMin";
            // 
            // txtQMin
            // 
            this.txtQMin.Location = new System.Drawing.Point(244, 620);
            this.txtQMin.Name = "txtQMin";
            this.txtQMin.Size = new System.Drawing.Size(52, 20);
            this.txtQMin.TabIndex = 29;
            // 
            // txtQMax
            // 
            this.txtQMax.Location = new System.Drawing.Point(244, 594);
            this.txtQMax.Name = "txtQMax";
            this.txtQMax.Size = new System.Drawing.Size(52, 20);
            this.txtQMax.TabIndex = 28;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(302, 623);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "QMin";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(302, 597);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "QMax";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(302, 649);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 33;
            this.label11.Text = "PSMin";
            // 
            // txtPSMin
            // 
            this.txtPSMin.Location = new System.Drawing.Point(150, 646);
            this.txtPSMin.Name = "txtPSMin";
            this.txtPSMin.Size = new System.Drawing.Size(145, 20);
            this.txtPSMin.TabIndex = 32;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(302, 675);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 35;
            this.label12.Text = "PSMax";
            // 
            // txtPSMax
            // 
            this.txtPSMax.Location = new System.Drawing.Point(150, 672);
            this.txtPSMax.Name = "txtPSMax";
            this.txtPSMax.Size = new System.Drawing.Size(145, 20);
            this.txtPSMax.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(302, 701);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "PMin";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(208, 701);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 13);
            this.label13.TabIndex = 38;
            this.label13.Text = "PMax";
            // 
            // txtPMin
            // 
            this.txtPMin.Location = new System.Drawing.Point(244, 698);
            this.txtPMin.Name = "txtPMin";
            this.txtPMin.Size = new System.Drawing.Size(52, 20);
            this.txtPMin.TabIndex = 37;
            // 
            // txtPMax
            // 
            this.txtPMax.Location = new System.Drawing.Point(150, 698);
            this.txtPMax.Name = "txtPMax";
            this.txtPMax.Size = new System.Drawing.Size(52, 20);
            this.txtPMax.TabIndex = 36;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(439, 548);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "SpecScale";
            // 
            // nudSpecScale
            // 
            this.nudSpecScale.Location = new System.Drawing.Point(500, 543);
            this.nudSpecScale.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nudSpecScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudSpecScale.Name = "nudSpecScale";
            this.nudSpecScale.Size = new System.Drawing.Size(41, 20);
            this.nudSpecScale.TabIndex = 40;
            this.nudSpecScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSpecScale.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // hscrollGain
            // 
            this.hscrollGain.LargeChange = 101;
            this.hscrollGain.Location = new System.Drawing.Point(31, 741);
            this.hscrollGain.Maximum = 10000;
            this.hscrollGain.Minimum = -10000;
            this.hscrollGain.Name = "hscrollGain";
            this.hscrollGain.Size = new System.Drawing.Size(566, 14);
            this.hscrollGain.SmallChange = 100;
            this.hscrollGain.TabIndex = 42;
            // 
            // hscrollPhase
            // 
            this.hscrollPhase.LargeChange = 101;
            this.hscrollPhase.Location = new System.Drawing.Point(31, 755);
            this.hscrollPhase.Maximum = 10000;
            this.hscrollPhase.Minimum = -10000;
            this.hscrollPhase.Name = "hscrollPhase";
            this.hscrollPhase.Size = new System.Drawing.Size(566, 14);
            this.hscrollPhase.SmallChange = 100;
            this.hscrollPhase.TabIndex = 43;
            // 
            // txtSR
            // 
            this.txtSR.Location = new System.Drawing.Point(32, 781);
            this.txtSR.Name = "txtSR";
            this.txtSR.Size = new System.Drawing.Size(74, 20);
            this.txtSR.TabIndex = 44;
            // 
            // btnSendSR
            // 
            this.btnSendSR.Location = new System.Drawing.Point(127, 781);
            this.btnSendSR.Name = "btnSendSR";
            this.btnSendSR.Size = new System.Drawing.Size(75, 20);
            this.btnSendSR.TabIndex = 45;
            this.btnSendSR.Text = "Send";
            this.btnSendSR.UseVisualStyleBackColor = true;
            this.btnSendSR.Click += new System.EventHandler(this.btnSendSR_Click);
            // 
            // btnRd
            // 
            this.btnRd.Location = new System.Drawing.Point(127, 807);
            this.btnRd.Name = "btnRd";
            this.btnRd.Size = new System.Drawing.Size(75, 20);
            this.btnRd.TabIndex = 46;
            this.btnRd.Text = "Read";
            this.btnRd.UseVisualStyleBackColor = true;
            this.btnRd.Click += new System.EventHandler(this.btnRd_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 859);
            this.Controls.Add(this.btnRd);
            this.Controls.Add(this.btnSendSR);
            this.Controls.Add(this.txtSR);
            this.Controls.Add(this.hscrollPhase);
            this.Controls.Add(this.hscrollGain);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.nudSpecScale);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtPMin);
            this.Controls.Add(this.txtPMax);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtPSMax);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtPSMin);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtQMin);
            this.Controls.Add(this.txtQMax);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtMin);
            this.Controls.Add(this.txtMax);
            this.Controls.Add(this.chkSpec);
            this.Controls.Add(this.chkQ);
            this.Controls.Add(this.chkI);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.hScrollBar2);
            this.Controls.Add(this.vScrollBar3);
            this.Controls.Add(this.vScrollBar2);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Merc Test Scope";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpecScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.VScrollBar vScrollBar2;
        private System.Windows.Forms.VScrollBar vScrollBar3;
        private System.Windows.Forms.HScrollBar hScrollBar2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox chkI;
        private System.Windows.Forms.CheckBox chkQ;
        private System.Windows.Forms.CheckBox chkSpec;
        private System.Windows.Forms.TextBox txtMax;
        private System.Windows.Forms.TextBox txtMin;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtQMin;
        private System.Windows.Forms.TextBox txtQMax;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtPSMin;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtPSMax;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtPMin;
        private System.Windows.Forms.TextBox txtPMax;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nudSpecScale;
        private System.Windows.Forms.HScrollBar hscrollGain;
        private System.Windows.Forms.HScrollBar hscrollPhase;
        private System.Windows.Forms.TextBox txtSR;
        private System.Windows.Forms.Button btnSendSR;
        private System.Windows.Forms.Button btnRd;
    }
}

