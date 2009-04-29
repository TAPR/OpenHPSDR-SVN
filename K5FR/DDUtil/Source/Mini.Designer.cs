using System;
using System.Diagnostics;

namespace DataDecoder
{
    partial class Mini
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
            this.lblAvg = new System.Windows.Forms.Label();
            this.txtAvg = new System.Windows.Forms.TextBox();
            this.txtTemp = new System.Windows.Forms.TextBox();
            this.btnReStart = new System.Windows.Forms.Button();
            this.txtSWR = new System.Windows.Forms.TextBox();
            this.lblSWR = new System.Windows.Forms.Label();
            this.btnProfiler = new System.Windows.Forms.Button();
            this.txtFwd = new System.Windows.Forms.TextBox();
            this.lblFwd = new System.Windows.Forms.Label();
            this.btnSP = new System.Windows.Forms.Button();
            this.txtSP = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rb34 = new System.Windows.Forms.RadioButton();
            this.rbBiDir = new System.Windows.Forms.RadioButton();
            this.rb180 = new System.Windows.Forms.RadioButton();
            this.rbFwd = new System.Windows.Forms.RadioButton();
            this.btnCalib = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.AOT = new System.Windows.Forms.CheckBox();
            this.btnHF = new System.Windows.Forms.Button();
            this.btnSF = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.btnHV = new System.Windows.Forms.Button();
            this.btnOper = new System.Windows.Forms.Button();
            this.btnTune = new System.Windows.Forms.Button();
            this.label71 = new System.Windows.Forms.Label();
            this.btnPwr = new System.Windows.Forms.Button();
            this.btnByp = new System.Windows.Forms.Button();
            this.lblBCD = new System.Windows.Forms.Label();
            this.btnFull = new System.Windows.Forms.Button();
            this.chkRCP2 = new System.Windows.Forms.CheckBox();
            this.chkRCP3 = new System.Windows.Forms.CheckBox();
            this.chkRCP4 = new System.Windows.Forms.CheckBox();
            this.btnMzcro1 = new System.Windows.Forms.Button();
            this.btnMacro2 = new System.Windows.Forms.Button();
            this.btnMacro3 = new System.Windows.Forms.Button();
            this.btnMacro4 = new System.Windows.Forms.Button();
            this.btnMacro5 = new System.Windows.Forms.Button();
            this.btnMacro6 = new System.Windows.Forms.Button();
            this.btnMacro7 = new System.Windows.Forms.Button();
            this.btnMacro8 = new System.Windows.Forms.Button();
            this.btnMacro16 = new System.Windows.Forms.Button();
            this.btnMacro15 = new System.Windows.Forms.Button();
            this.btnMacro14 = new System.Windows.Forms.Button();
            this.btnMacro13 = new System.Windows.Forms.Button();
            this.btnMacro12 = new System.Windows.Forms.Button();
            this.btnMacro11 = new System.Windows.Forms.Button();
            this.btnMacro10 = new System.Windows.Forms.Button();
            this.btnMacro9 = new System.Windows.Forms.Button();
            this.grpStepCtrl = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.grpAmp = new System.Windows.Forms.GroupBox();
            this.lblHF = new System.Windows.Forms.Label();
            this.lblSF = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.grpMacro = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpStepCtrl.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpAmp.SuspendLayout();
            this.grpMacro.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAvg
            // 
            this.lblAvg.AutoSize = true;
            this.lblAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvg.ForeColor = System.Drawing.Color.Navy;
            this.lblAvg.Location = new System.Drawing.Point(125, 9);
            this.lblAvg.Name = "lblAvg";
            this.lblAvg.Size = new System.Drawing.Size(23, 12);
            this.lblAvg.TabIndex = 83;
            this.lblAvg.Text = "Avg";
            this.lblAvg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtAvg
            // 
            this.txtAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAvg.Location = new System.Drawing.Point(79, 5);
            this.txtAvg.Name = "txtAvg";
            this.txtAvg.Size = new System.Drawing.Size(45, 20);
            this.txtAvg.TabIndex = 84;
            this.txtAvg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTemp
            // 
            this.txtTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F, System.Drawing.FontStyle.Bold);
            this.txtTemp.Location = new System.Drawing.Point(298, 6);
            this.txtTemp.Name = "txtTemp";
            this.txtTemp.Size = new System.Drawing.Size(49, 19);
            this.txtTemp.TabIndex = 77;
            this.txtTemp.Text = "PA Tmp";
            this.txtTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTemp, "PA Temp");
            // 
            // btnReStart
            // 
            this.btnReStart.AutoSize = true;
            this.btnReStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnReStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnReStart.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnReStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnReStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnReStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold);
            this.btnReStart.ForeColor = System.Drawing.Color.Black;
            this.btnReStart.Location = new System.Drawing.Point(262, 6);
            this.btnReStart.Name = "btnReStart";
            this.btnReStart.Size = new System.Drawing.Size(28, 19);
            this.btnReStart.TabIndex = 82;
            this.btnReStart.Text = "PC";
            this.toolTip1.SetToolTip(this.btnReStart, "Profiler Close");
            this.btnReStart.UseVisualStyleBackColor = false;
            this.btnReStart.Click += new System.EventHandler(this.btnReStart_Click);
            // 
            // txtSWR
            // 
            this.txtSWR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSWR.ForeColor = System.Drawing.Color.Red;
            this.txtSWR.Location = new System.Drawing.Point(150, 5);
            this.txtSWR.Name = "txtSWR";
            this.txtSWR.Size = new System.Drawing.Size(45, 20);
            this.txtSWR.TabIndex = 79;
            this.txtSWR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSWR
            // 
            this.lblSWR.AutoSize = true;
            this.lblSWR.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSWR.ForeColor = System.Drawing.Color.Navy;
            this.lblSWR.Location = new System.Drawing.Point(196, 9);
            this.lblSWR.Name = "lblSWR";
            this.lblSWR.Size = new System.Drawing.Size(27, 12);
            this.lblSWR.TabIndex = 80;
            this.lblSWR.Text = "SWR";
            this.lblSWR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnProfiler
            // 
            this.btnProfiler.AutoSize = true;
            this.btnProfiler.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnProfiler.BackColor = System.Drawing.Color.LightYellow;
            this.btnProfiler.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnProfiler.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this.btnProfiler.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnProfiler.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold);
            this.btnProfiler.ForeColor = System.Drawing.Color.Black;
            this.btnProfiler.Location = new System.Drawing.Point(226, 6);
            this.btnProfiler.Name = "btnProfiler";
            this.btnProfiler.Size = new System.Drawing.Size(28, 19);
            this.btnProfiler.TabIndex = 81;
            this.btnProfiler.Text = "PO";
            this.toolTip1.SetToolTip(this.btnProfiler, "Profiler Open");
            this.btnProfiler.UseVisualStyleBackColor = false;
            this.btnProfiler.Click += new System.EventHandler(this.btnProfiler_Click);
            // 
            // txtFwd
            // 
            this.txtFwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFwd.Location = new System.Drawing.Point(5, 5);
            this.txtFwd.Name = "txtFwd";
            this.txtFwd.Size = new System.Drawing.Size(45, 20);
            this.txtFwd.TabIndex = 78;
            this.txtFwd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFwd
            // 
            this.lblFwd.AutoSize = true;
            this.lblFwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFwd.ForeColor = System.Drawing.Color.Navy;
            this.lblFwd.Location = new System.Drawing.Point(50, 9);
            this.lblFwd.Name = "lblFwd";
            this.lblFwd.Size = new System.Drawing.Size(26, 12);
            this.lblFwd.TabIndex = 76;
            this.lblFwd.Text = "Peak";
            this.lblFwd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSP
            // 
            this.btnSP.AutoSize = true;
            this.btnSP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold);
            this.btnSP.ForeColor = System.Drawing.Color.Blue;
            this.btnSP.Location = new System.Drawing.Point(49, 14);
            this.btnSP.Name = "btnSP";
            this.btnSP.Size = new System.Drawing.Size(31, 22);
            this.btnSP.TabIndex = 0;
            this.btnSP.Text = "GO";
            this.toolTip1.SetToolTip(this.btnSP, "Click to turn rotor to heading (Ctrl+Click to stop rotor)");
            this.btnSP.UseVisualStyleBackColor = true;
            this.btnSP.Click += new System.EventHandler(this.btnSP_Click);
            this.btnSP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSP_KeyDown);
            // 
            // txtSP
            // 
            this.txtSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold);
            this.txtSP.ForeColor = System.Drawing.Color.Firebrick;
            this.txtSP.Location = new System.Drawing.Point(5, 15);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(39, 18);
            this.txtSP.TabIndex = 3;
            this.txtSP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSP, "Enter Rotor Heading in whole degrees.");
            // 
            // rb34
            // 
            this.rb34.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rb34.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rb34.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rb34.ForeColor = System.Drawing.Color.Navy;
            this.rb34.Location = new System.Drawing.Point(130, 17);
            this.rb34.Name = "rb34";
            this.rb34.Size = new System.Drawing.Size(45, 17);
            this.rb34.TabIndex = 102;
            this.rb34.Text = "3/4";
            this.toolTip1.SetToolTip(this.rb34, "Select to put vertical in 3/4 wavelength mode (15 & 10  meters only)");
            this.rb34.UseVisualStyleBackColor = true;
            this.rb34.CheckedChanged += new System.EventHandler(this.grpStepCtrl_CheckedChanged);
            // 
            // rbBiDir
            // 
            this.rbBiDir.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbBiDir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbBiDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbBiDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbBiDir.ForeColor = System.Drawing.Color.Navy;
            this.rbBiDir.Location = new System.Drawing.Point(89, 17);
            this.rbBiDir.Name = "rbBiDir";
            this.rbBiDir.Size = new System.Drawing.Size(45, 18);
            this.rbBiDir.TabIndex = 101;
            this.rbBiDir.Text = "<-->";
            this.toolTip1.SetToolTip(this.rbBiDir, "Select to put yagi in bi-directional mode");
            this.rbBiDir.UseVisualStyleBackColor = true;
            this.rbBiDir.CheckedChanged += new System.EventHandler(this.grpStepCtrl_CheckedChanged);
            // 
            // rb180
            // 
            this.rb180.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rb180.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rb180.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb180.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rb180.ForeColor = System.Drawing.Color.Navy;
            this.rb180.Location = new System.Drawing.Point(48, 17);
            this.rb180.Name = "rb180";
            this.rb180.Size = new System.Drawing.Size(45, 18);
            this.rb180.TabIndex = 100;
            this.rb180.Text = "180";
            this.toolTip1.SetToolTip(this.rb180, "Select to put yagi in 180 mode (reverse)");
            this.rb180.UseVisualStyleBackColor = true;
            this.rb180.CheckedChanged += new System.EventHandler(this.grpStepCtrl_CheckedChanged);
            // 
            // rbFwd
            // 
            this.rbFwd.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rbFwd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbFwd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbFwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbFwd.ForeColor = System.Drawing.Color.Navy;
            this.rbFwd.Location = new System.Drawing.Point(5, 17);
            this.rbFwd.Name = "rbFwd";
            this.rbFwd.Size = new System.Drawing.Size(45, 18);
            this.rbFwd.TabIndex = 99;
            this.rbFwd.Text = "Fwd";
            this.toolTip1.SetToolTip(this.rbFwd, "Select to put yagi in forward mode");
            this.rbFwd.UseVisualStyleBackColor = true;
            this.rbFwd.CheckedChanged += new System.EventHandler(this.grpStepCtrl_CheckedChanged);
            // 
            // btnCalib
            // 
            this.btnCalib.AutoSize = true;
            this.btnCalib.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCalib.BackColor = System.Drawing.SystemColors.Control;
            this.btnCalib.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCalib.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnCalib.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnCalib.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalib.ForeColor = System.Drawing.Color.Navy;
            this.btnCalib.Location = new System.Drawing.Point(225, 13);
            this.btnCalib.Name = "btnCalib";
            this.btnCalib.Size = new System.Drawing.Size(32, 22);
            this.btnCalib.TabIndex = 98;
            this.btnCalib.Text = "Cal";
            this.toolTip1.SetToolTip(this.btnCalib, "Calibrate antenna elements (home & return).");
            this.btnCalib.UseVisualStyleBackColor = false;
            this.btnCalib.Click += new System.EventHandler(this.btnCalib_Click);
            // 
            // btnHome
            // 
            this.btnHome.AutoSize = true;
            this.btnHome.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnHome.BackColor = System.Drawing.SystemColors.Control;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.Navy;
            this.btnHome.Location = new System.Drawing.Point(175, 13);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(44, 22);
            this.btnHome.TabIndex = 97;
            this.btnHome.Text = "Home";
            this.toolTip1.SetToolTip(this.btnHome, "Retract the antenna elements (Home).");
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // AOT
            // 
            this.AOT.AutoSize = true;
            this.AOT.Location = new System.Drawing.Point(286, 142);
            this.AOT.Name = "AOT";
            this.AOT.Size = new System.Drawing.Size(62, 17);
            this.AOT.TabIndex = 87;
            this.AOT.Text = "On Top";
            this.toolTip1.SetToolTip(this.AOT, "When selected causes this window to be on top of all other windows.");
            this.AOT.UseVisualStyleBackColor = true;
            this.AOT.CheckedChanged += new System.EventHandler(this.AOT_CheckedChanged);
            // 
            // btnHF
            // 
            this.btnHF.BackColor = System.Drawing.SystemColors.Control;
            this.btnHF.Enabled = false;
            this.btnHF.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnHF.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnHF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHF.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnHF.Location = new System.Drawing.Point(227, 31);
            this.btnHF.Name = "btnHF";
            this.btnHF.Size = new System.Drawing.Size(15, 15);
            this.btnHF.TabIndex = 52;
            this.toolTip1.SetToolTip(this.btnHF, "Press to display Hard Fault log (Double-Click button label to clear display).");
            this.btnHF.UseVisualStyleBackColor = false;
            this.btnHF.Click += new System.EventHandler(this.btnHF_Click);
            // 
            // btnSF
            // 
            this.btnSF.BackColor = System.Drawing.SystemColors.Control;
            this.btnSF.Enabled = false;
            this.btnSF.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnSF.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSF.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSF.Location = new System.Drawing.Point(206, 31);
            this.btnSF.Name = "btnSF";
            this.btnSF.Size = new System.Drawing.Size(15, 15);
            this.btnSF.TabIndex = 51;
            this.toolTip1.SetToolTip(this.btnSF, "Press to display Soft Fault log (Double-Click button label to clear display).");
            this.btnSF.UseVisualStyleBackColor = false;
            this.btnSF.Click += new System.EventHandler(this.btnSF_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.BackColor = System.Drawing.SystemColors.Info;
            this.txtMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMsg.Location = new System.Drawing.Point(7, 54);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ReadOnly = true;
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMsg.Size = new System.Drawing.Size(236, 20);
            this.txtMsg.TabIndex = 48;
            this.txtMsg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMsg, "Displays Status and Fault messages.");
            // 
            // btnHV
            // 
            this.btnHV.BackColor = System.Drawing.SystemColors.Control;
            this.btnHV.Enabled = false;
            this.btnHV.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnHV.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnHV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHV.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnHV.Location = new System.Drawing.Point(157, 28);
            this.btnHV.Name = "btnHV";
            this.btnHV.Size = new System.Drawing.Size(41, 22);
            this.btnHV.TabIndex = 39;
            this.btnHV.Text = "Off";
            this.toolTip1.SetToolTip(this.btnHV, "Press to toggle High/Low plate voltage .");
            this.btnHV.UseVisualStyleBackColor = false;
            this.btnHV.Click += new System.EventHandler(this.btnHV_Click);
            // 
            // btnOper
            // 
            this.btnOper.BackColor = System.Drawing.SystemColors.Control;
            this.btnOper.Enabled = false;
            this.btnOper.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOper.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOper.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOper.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOper.Location = new System.Drawing.Point(57, 28);
            this.btnOper.Name = "btnOper";
            this.btnOper.Size = new System.Drawing.Size(41, 22);
            this.btnOper.TabIndex = 9;
            this.btnOper.Text = "Off";
            this.toolTip1.SetToolTip(this.btnOper, "Press to toggle Operate/StandBy modes.");
            this.btnOper.UseVisualStyleBackColor = false;
            this.btnOper.Click += new System.EventHandler(this.btnOper_Click);
            // 
            // btnTune
            // 
            this.btnTune.BackColor = System.Drawing.SystemColors.Control;
            this.btnTune.Enabled = false;
            this.btnTune.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnTune.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTune.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTune.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTune.Location = new System.Drawing.Point(107, 28);
            this.btnTune.Name = "btnTune";
            this.btnTune.Size = new System.Drawing.Size(41, 22);
            this.btnTune.TabIndex = 8;
            this.btnTune.Text = "Off";
            this.toolTip1.SetToolTip(this.btnTune, "Press to toggle Autotune/Manual tuning modes.");
            this.btnTune.UseVisualStyleBackColor = false;
            this.btnTune.Click += new System.EventHandler(this.btnTune_Click);
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label71.ForeColor = System.Drawing.Color.Navy;
            this.label71.Location = new System.Drawing.Point(62, 13);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(30, 13);
            this.label71.TabIndex = 7;
            this.label71.Text = "Oper";
            this.toolTip1.SetToolTip(this.label71, "StdBy/Operate");
            // 
            // btnPwr
            // 
            this.btnPwr.BackColor = System.Drawing.SystemColors.Control;
            this.btnPwr.Enabled = false;
            this.btnPwr.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPwr.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPwr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPwr.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPwr.Location = new System.Drawing.Point(7, 28);
            this.btnPwr.Name = "btnPwr";
            this.btnPwr.Size = new System.Drawing.Size(41, 22);
            this.btnPwr.TabIndex = 0;
            this.btnPwr.Text = "Off";
            this.toolTip1.SetToolTip(this.btnPwr, "Press to turn On/Off amplifier.");
            this.btnPwr.UseVisualStyleBackColor = false;
            this.btnPwr.Click += new System.EventHandler(this.btnPwr_Click);
            // 
            // btnByp
            // 
            this.btnByp.AutoSize = true;
            this.btnByp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnByp.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnByp.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnByp.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.btnByp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnByp.Location = new System.Drawing.Point(368, 41);
            this.btnByp.Name = "btnByp";
            this.btnByp.Size = new System.Drawing.Size(46, 23);
            this.btnByp.TabIndex = 89;
            this.btnByp.Text = "OPER";
            this.toolTip1.SetToolTip(this.btnByp, "Click to toggle PTT on/off  (ctrl+O)");
            this.btnByp.UseVisualStyleBackColor = false;
            this.btnByp.Click += new System.EventHandler(this.btnByp_Click);
            // 
            // lblBCD
            // 
            this.lblBCD.AutoSize = true;
            this.lblBCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBCD.ForeColor = System.Drawing.Color.Red;
            this.lblBCD.Location = new System.Drawing.Point(389, 8);
            this.lblBCD.Name = "lblBCD";
            this.lblBCD.Size = new System.Drawing.Size(15, 15);
            this.lblBCD.TabIndex = 90;
            this.lblBCD.Text = "0";
            this.lblBCD.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.lblBCD, "BCD Device number");
            // 
            // btnFull
            // 
            this.btnFull.AutoSize = true;
            this.btnFull.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnFull.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold);
            this.btnFull.ForeColor = System.Drawing.Color.Black;
            this.btnFull.Location = new System.Drawing.Point(352, 4);
            this.btnFull.Name = "btnFull";
            this.btnFull.Size = new System.Drawing.Size(27, 19);
            this.btnFull.TabIndex = 83;
            this.btnFull.Text = "FZ";
            this.toolTip1.SetToolTip(this.btnFull, "Click to toggle DDUtil Main form On/Off.");
            this.btnFull.UseVisualStyleBackColor = false;
            this.btnFull.Click += new System.EventHandler(this.btnFull_Click);
            // 
            // chkRCP2
            // 
            this.chkRCP2.AutoSize = true;
            this.chkRCP2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP2.Location = new System.Drawing.Point(286, 81);
            this.chkRCP2.Name = "chkRCP2";
            this.chkRCP2.Size = new System.Drawing.Size(54, 17);
            this.chkRCP2.TabIndex = 91;
            this.chkRCP2.Text = "RCP2";
            this.toolTip1.SetToolTip(this.chkRCP2, "Check to enable RCP 2");
            this.chkRCP2.UseVisualStyleBackColor = true;
            this.chkRCP2.CheckedChanged += new System.EventHandler(this.chkRCP2_CheckedChanged);
            // 
            // chkRCP3
            // 
            this.chkRCP3.AutoSize = true;
            this.chkRCP3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP3.Location = new System.Drawing.Point(286, 101);
            this.chkRCP3.Name = "chkRCP3";
            this.chkRCP3.Size = new System.Drawing.Size(54, 17);
            this.chkRCP3.TabIndex = 93;
            this.chkRCP3.Text = "RCP3";
            this.toolTip1.SetToolTip(this.chkRCP3, "Check to enable RCP 3");
            this.chkRCP3.UseVisualStyleBackColor = true;
            this.chkRCP3.CheckedChanged += new System.EventHandler(this.chkRCP3_CheckedChanged);
            // 
            // chkRCP4
            // 
            this.chkRCP4.AutoSize = true;
            this.chkRCP4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.chkRCP4.Location = new System.Drawing.Point(286, 122);
            this.chkRCP4.Name = "chkRCP4";
            this.chkRCP4.Size = new System.Drawing.Size(54, 17);
            this.chkRCP4.TabIndex = 94;
            this.chkRCP4.Text = "RCP4";
            this.toolTip1.SetToolTip(this.chkRCP4, "Check to enable RCP 4");
            this.chkRCP4.UseVisualStyleBackColor = true;
            this.chkRCP4.CheckedChanged += new System.EventHandler(this.chkRCP4_CheckedChanged);
            // 
            // btnMzcro1
            // 
            this.btnMzcro1.BackColor = System.Drawing.SystemColors.Info;
            this.btnMzcro1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMzcro1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMzcro1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMzcro1.Location = new System.Drawing.Point(8, 33);
            this.btnMzcro1.Name = "btnMzcro1";
            this.btnMzcro1.Size = new System.Drawing.Size(20, 12);
            this.btnMzcro1.TabIndex = 140;
            this.toolTip1.SetToolTip(this.btnMzcro1, "Press to run macro for this key.");
            this.btnMzcro1.UseVisualStyleBackColor = false;
            this.btnMzcro1.Click += new System.EventHandler(this.btnMzcro1_Click);
            // 
            // btnMacro2
            // 
            this.btnMacro2.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro2.Location = new System.Drawing.Point(41, 33);
            this.btnMacro2.Name = "btnMacro2";
            this.btnMacro2.Size = new System.Drawing.Size(20, 12);
            this.btnMacro2.TabIndex = 142;
            this.toolTip1.SetToolTip(this.btnMacro2, "Press to run macro for this key.");
            this.btnMacro2.UseVisualStyleBackColor = false;
            this.btnMacro2.Click += new System.EventHandler(this.btnMacro2_Click);
            // 
            // btnMacro3
            // 
            this.btnMacro3.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro3.Location = new System.Drawing.Point(75, 33);
            this.btnMacro3.Name = "btnMacro3";
            this.btnMacro3.Size = new System.Drawing.Size(20, 12);
            this.btnMacro3.TabIndex = 144;
            this.toolTip1.SetToolTip(this.btnMacro3, "Press to run macro for this key.");
            this.btnMacro3.UseVisualStyleBackColor = false;
            this.btnMacro3.Click += new System.EventHandler(this.btnMacro3_Click);
            // 
            // btnMacro4
            // 
            this.btnMacro4.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro4.Location = new System.Drawing.Point(109, 33);
            this.btnMacro4.Name = "btnMacro4";
            this.btnMacro4.Size = new System.Drawing.Size(20, 12);
            this.btnMacro4.TabIndex = 146;
            this.toolTip1.SetToolTip(this.btnMacro4, "Press to run macro for this key.");
            this.btnMacro4.UseVisualStyleBackColor = false;
            this.btnMacro4.Click += new System.EventHandler(this.btnMacro4_Click);
            // 
            // btnMacro5
            // 
            this.btnMacro5.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro5.Location = new System.Drawing.Point(143, 33);
            this.btnMacro5.Name = "btnMacro5";
            this.btnMacro5.Size = new System.Drawing.Size(20, 12);
            this.btnMacro5.TabIndex = 148;
            this.toolTip1.SetToolTip(this.btnMacro5, "Press to run macro for this key.");
            this.btnMacro5.UseVisualStyleBackColor = false;
            this.btnMacro5.Click += new System.EventHandler(this.btnMacro5_Click);
            // 
            // btnMacro6
            // 
            this.btnMacro6.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro6.Location = new System.Drawing.Point(177, 33);
            this.btnMacro6.Name = "btnMacro6";
            this.btnMacro6.Size = new System.Drawing.Size(20, 12);
            this.btnMacro6.TabIndex = 150;
            this.toolTip1.SetToolTip(this.btnMacro6, "Press to run macro for this key.");
            this.btnMacro6.UseVisualStyleBackColor = false;
            this.btnMacro6.Click += new System.EventHandler(this.btnMacro6_Click);
            // 
            // btnMacro7
            // 
            this.btnMacro7.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro7.Location = new System.Drawing.Point(210, 33);
            this.btnMacro7.Name = "btnMacro7";
            this.btnMacro7.Size = new System.Drawing.Size(20, 12);
            this.btnMacro7.TabIndex = 152;
            this.toolTip1.SetToolTip(this.btnMacro7, "Press to run macro for this key.");
            this.btnMacro7.UseVisualStyleBackColor = false;
            this.btnMacro7.Click += new System.EventHandler(this.btnMacro7_Click);
            // 
            // btnMacro8
            // 
            this.btnMacro8.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro8.Location = new System.Drawing.Point(243, 33);
            this.btnMacro8.Name = "btnMacro8";
            this.btnMacro8.Size = new System.Drawing.Size(20, 12);
            this.btnMacro8.TabIndex = 154;
            this.toolTip1.SetToolTip(this.btnMacro8, "Press to run macro for this key.");
            this.btnMacro8.UseVisualStyleBackColor = false;
            this.btnMacro8.Click += new System.EventHandler(this.btnMacro8_Click);
            // 
            // btnMacro16
            // 
            this.btnMacro16.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro16.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro16.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro16.Location = new System.Drawing.Point(243, 64);
            this.btnMacro16.Name = "btnMacro16";
            this.btnMacro16.Size = new System.Drawing.Size(20, 12);
            this.btnMacro16.TabIndex = 170;
            this.toolTip1.SetToolTip(this.btnMacro16, "Press to run macro for this key.");
            this.btnMacro16.UseVisualStyleBackColor = false;
            this.btnMacro16.Click += new System.EventHandler(this.btnMacro16_Click);
            // 
            // btnMacro15
            // 
            this.btnMacro15.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro15.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro15.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro15.Location = new System.Drawing.Point(210, 64);
            this.btnMacro15.Name = "btnMacro15";
            this.btnMacro15.Size = new System.Drawing.Size(20, 12);
            this.btnMacro15.TabIndex = 168;
            this.toolTip1.SetToolTip(this.btnMacro15, "Press to run macro for this key.");
            this.btnMacro15.UseVisualStyleBackColor = false;
            this.btnMacro15.Click += new System.EventHandler(this.btnMacro15_Click);
            // 
            // btnMacro14
            // 
            this.btnMacro14.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro14.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro14.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro14.Location = new System.Drawing.Point(177, 64);
            this.btnMacro14.Name = "btnMacro14";
            this.btnMacro14.Size = new System.Drawing.Size(20, 12);
            this.btnMacro14.TabIndex = 166;
            this.toolTip1.SetToolTip(this.btnMacro14, "Press to run macro for this key.");
            this.btnMacro14.UseVisualStyleBackColor = false;
            this.btnMacro14.Click += new System.EventHandler(this.btnMacro14_Click);
            // 
            // btnMacro13
            // 
            this.btnMacro13.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro13.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro13.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro13.Location = new System.Drawing.Point(143, 64);
            this.btnMacro13.Name = "btnMacro13";
            this.btnMacro13.Size = new System.Drawing.Size(20, 12);
            this.btnMacro13.TabIndex = 164;
            this.toolTip1.SetToolTip(this.btnMacro13, "Press to run macro for this key.");
            this.btnMacro13.UseVisualStyleBackColor = false;
            this.btnMacro13.Click += new System.EventHandler(this.btnMacro13_Click);
            // 
            // btnMacro12
            // 
            this.btnMacro12.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro12.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro12.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro12.Location = new System.Drawing.Point(109, 64);
            this.btnMacro12.Name = "btnMacro12";
            this.btnMacro12.Size = new System.Drawing.Size(20, 12);
            this.btnMacro12.TabIndex = 162;
            this.toolTip1.SetToolTip(this.btnMacro12, "Press to run macro for this key.");
            this.btnMacro12.UseVisualStyleBackColor = false;
            this.btnMacro12.Click += new System.EventHandler(this.btnMacro12_Click);
            // 
            // btnMacro11
            // 
            this.btnMacro11.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro11.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro11.Location = new System.Drawing.Point(75, 64);
            this.btnMacro11.Name = "btnMacro11";
            this.btnMacro11.Size = new System.Drawing.Size(20, 12);
            this.btnMacro11.TabIndex = 160;
            this.toolTip1.SetToolTip(this.btnMacro11, "Press to run macro for this key.");
            this.btnMacro11.UseVisualStyleBackColor = false;
            this.btnMacro11.Click += new System.EventHandler(this.btnMacro11_Click);
            // 
            // btnMacro10
            // 
            this.btnMacro10.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro10.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro10.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro10.Location = new System.Drawing.Point(41, 64);
            this.btnMacro10.Name = "btnMacro10";
            this.btnMacro10.Size = new System.Drawing.Size(20, 12);
            this.btnMacro10.TabIndex = 158;
            this.toolTip1.SetToolTip(this.btnMacro10, "Press to run macro for this key.");
            this.btnMacro10.UseVisualStyleBackColor = false;
            this.btnMacro10.Click += new System.EventHandler(this.btnMacro10_Click);
            // 
            // btnMacro9
            // 
            this.btnMacro9.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro9.Location = new System.Drawing.Point(8, 64);
            this.btnMacro9.Name = "btnMacro9";
            this.btnMacro9.Size = new System.Drawing.Size(20, 12);
            this.btnMacro9.TabIndex = 156;
            this.toolTip1.SetToolTip(this.btnMacro9, "Press to run macro for this key.");
            this.btnMacro9.UseVisualStyleBackColor = false;
            this.btnMacro9.Click += new System.EventHandler(this.btnMacro9_Click);
            // 
            // grpStepCtrl
            // 
            this.grpStepCtrl.Controls.Add(this.rb34);
            this.grpStepCtrl.Controls.Add(this.rbBiDir);
            this.grpStepCtrl.Controls.Add(this.rb180);
            this.grpStepCtrl.Controls.Add(this.rbFwd);
            this.grpStepCtrl.Controls.Add(this.btnCalib);
            this.grpStepCtrl.Controls.Add(this.btnHome);
            this.grpStepCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold);
            this.grpStepCtrl.ForeColor = System.Drawing.Color.Firebrick;
            this.grpStepCtrl.Location = new System.Drawing.Point(5, 30);
            this.grpStepCtrl.Name = "grpStepCtrl";
            this.grpStepCtrl.Size = new System.Drawing.Size(266, 40);
            this.grpStepCtrl.TabIndex = 85;
            this.grpStepCtrl.TabStop = false;
            this.grpStepCtrl.Text = "SteppIR";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSP);
            this.groupBox1.Controls.Add(this.txtSP);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.Firebrick;
            this.groupBox1.Location = new System.Drawing.Point(274, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(89, 40);
            this.groupBox1.TabIndex = 86;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rotor";
            // 
            // button4
            // 
            this.button4.AutoSize = true;
            this.button4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold);
            this.button4.ForeColor = System.Drawing.Color.Navy;
            this.button4.Location = new System.Drawing.Point(-30, -24);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(32, 18);
            this.button4.TabIndex = 81;
            this.button4.Text = "PO";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.btnProfiler_Click);
            // 
            // button5
            // 
            this.button5.AutoSize = true;
            this.button5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button5.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold);
            this.button5.ForeColor = System.Drawing.Color.Navy;
            this.button5.Location = new System.Drawing.Point(7, -24);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(32, 18);
            this.button5.TabIndex = 82;
            this.button5.Text = "PC";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.btnReStart_Click);
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(42, -26);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(36, 21);
            this.textBox3.TabIndex = 77;
            this.textBox3.Text = "Tmp";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // grpAmp
            // 
            this.grpAmp.Controls.Add(this.btnHF);
            this.grpAmp.Controls.Add(this.btnSF);
            this.grpAmp.Controls.Add(this.lblHF);
            this.grpAmp.Controls.Add(this.txtMsg);
            this.grpAmp.Controls.Add(this.lblSF);
            this.grpAmp.Controls.Add(this.btnHV);
            this.grpAmp.Controls.Add(this.label77);
            this.grpAmp.Controls.Add(this.label76);
            this.grpAmp.Controls.Add(this.label73);
            this.grpAmp.Controls.Add(this.btnOper);
            this.grpAmp.Controls.Add(this.btnTune);
            this.grpAmp.Controls.Add(this.label71);
            this.grpAmp.Controls.Add(this.btnPwr);
            this.grpAmp.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold);
            this.grpAmp.ForeColor = System.Drawing.Color.Blue;
            this.grpAmp.Location = new System.Drawing.Point(426, 4);
            this.grpAmp.Name = "grpAmp";
            this.grpAmp.Size = new System.Drawing.Size(251, 80);
            this.grpAmp.TabIndex = 88;
            this.grpAmp.TabStop = false;
            this.grpAmp.Text = "Amplifier";
            // 
            // lblHF
            // 
            this.lblHF.AutoSize = true;
            this.lblHF.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHF.ForeColor = System.Drawing.Color.Navy;
            this.lblHF.Location = new System.Drawing.Point(226, 13);
            this.lblHF.Name = "lblHF";
            this.lblHF.Size = new System.Drawing.Size(18, 12);
            this.lblHF.TabIndex = 50;
            this.lblHF.Text = "HF";
            this.lblHF.Click += new System.EventHandler(this.lblHF_DoubleClick);
            // 
            // lblSF
            // 
            this.lblSF.AutoSize = true;
            this.lblSF.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSF.ForeColor = System.Drawing.Color.Navy;
            this.lblSF.Location = new System.Drawing.Point(205, 13);
            this.lblSF.Name = "lblSF";
            this.lblSF.Size = new System.Drawing.Size(17, 12);
            this.lblSF.TabIndex = 47;
            this.lblSF.Text = "SF";
            this.lblSF.Click += new System.EventHandler(this.lblSF_DoubleClick);
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label77.ForeColor = System.Drawing.Color.Navy;
            this.label77.Location = new System.Drawing.Point(167, 13);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(21, 13);
            this.label77.TabIndex = 38;
            this.label77.Text = "HV";
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label76.ForeColor = System.Drawing.Color.Navy;
            this.label76.Location = new System.Drawing.Point(9, 13);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(37, 13);
            this.label76.TabIndex = 37;
            this.label76.Text = "Power";
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label73.ForeColor = System.Drawing.Color.Navy;
            this.label73.Location = new System.Drawing.Point(112, 13);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(30, 13);
            this.label73.TabIndex = 11;
            this.label73.Text = "Tune";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.Navy;
            this.label28.Location = new System.Drawing.Point(9, 17);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(18, 12);
            this.label28.TabIndex = 114;
            this.label28.Text = "F1";
            // 
            // grpMacro
            // 
            this.grpMacro.Controls.Add(this.btnMacro16);
            this.grpMacro.Controls.Add(this.label8);
            this.grpMacro.Controls.Add(this.btnMacro15);
            this.grpMacro.Controls.Add(this.label9);
            this.grpMacro.Controls.Add(this.btnMacro14);
            this.grpMacro.Controls.Add(this.label10);
            this.grpMacro.Controls.Add(this.btnMacro13);
            this.grpMacro.Controls.Add(this.label11);
            this.grpMacro.Controls.Add(this.btnMacro12);
            this.grpMacro.Controls.Add(this.label12);
            this.grpMacro.Controls.Add(this.btnMacro11);
            this.grpMacro.Controls.Add(this.label13);
            this.grpMacro.Controls.Add(this.btnMacro10);
            this.grpMacro.Controls.Add(this.label14);
            this.grpMacro.Controls.Add(this.btnMacro9);
            this.grpMacro.Controls.Add(this.label15);
            this.grpMacro.Controls.Add(this.btnMacro8);
            this.grpMacro.Controls.Add(this.label7);
            this.grpMacro.Controls.Add(this.btnMacro7);
            this.grpMacro.Controls.Add(this.label6);
            this.grpMacro.Controls.Add(this.btnMacro6);
            this.grpMacro.Controls.Add(this.label5);
            this.grpMacro.Controls.Add(this.btnMacro5);
            this.grpMacro.Controls.Add(this.label4);
            this.grpMacro.Controls.Add(this.btnMacro4);
            this.grpMacro.Controls.Add(this.label3);
            this.grpMacro.Controls.Add(this.btnMacro3);
            this.grpMacro.Controls.Add(this.label2);
            this.grpMacro.Controls.Add(this.btnMacro2);
            this.grpMacro.Controls.Add(this.label1);
            this.grpMacro.Controls.Add(this.btnMzcro1);
            this.grpMacro.Controls.Add(this.label28);
            this.grpMacro.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold);
            this.grpMacro.ForeColor = System.Drawing.Color.Firebrick;
            this.grpMacro.Location = new System.Drawing.Point(5, 74);
            this.grpMacro.Name = "grpMacro";
            this.grpMacro.Size = new System.Drawing.Size(272, 86);
            this.grpMacro.TabIndex = 103;
            this.grpMacro.TabStop = false;
            this.grpMacro.Text = "Macro";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Navy;
            this.label8.Location = new System.Drawing.Point(241, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 12);
            this.label8.TabIndex = 169;
            this.label8.Text = "^F4";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Navy;
            this.label9.Location = new System.Drawing.Point(208, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 12);
            this.label9.TabIndex = 167;
            this.label9.Text = "^F3";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Navy;
            this.label10.Location = new System.Drawing.Point(175, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 12);
            this.label10.TabIndex = 165;
            this.label10.Text = "^F2";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Navy;
            this.label11.Location = new System.Drawing.Point(141, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 12);
            this.label11.TabIndex = 163;
            this.label11.Text = "^F1";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Navy;
            this.label12.Location = new System.Drawing.Point(108, 48);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(24, 12);
            this.label12.TabIndex = 161;
            this.label12.Text = "F12";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Navy;
            this.label13.Location = new System.Drawing.Point(74, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 12);
            this.label13.TabIndex = 159;
            this.label13.Text = "F11";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Navy;
            this.label14.Location = new System.Drawing.Point(40, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(24, 12);
            this.label14.TabIndex = 157;
            this.label14.Text = "F10";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Navy;
            this.label15.Location = new System.Drawing.Point(9, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(18, 12);
            this.label15.TabIndex = 155;
            this.label15.Text = "F9";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Navy;
            this.label7.Location = new System.Drawing.Point(244, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 12);
            this.label7.TabIndex = 153;
            this.label7.Text = "F8";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Navy;
            this.label6.Location = new System.Drawing.Point(211, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 12);
            this.label6.TabIndex = 151;
            this.label6.Text = "F7";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Navy;
            this.label5.Location = new System.Drawing.Point(178, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 12);
            this.label5.TabIndex = 149;
            this.label5.Text = "F6";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Navy;
            this.label4.Location = new System.Drawing.Point(144, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 12);
            this.label4.TabIndex = 147;
            this.label4.Text = "F5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(110, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 12);
            this.label3.TabIndex = 145;
            this.label3.Text = "F4";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(76, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 12);
            this.label2.TabIndex = 143;
            this.label2.Text = "F3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(42, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 12);
            this.label1.TabIndex = 141;
            this.label1.Text = "F2";
            // 
            // Mini
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(681, 174);
            this.Controls.Add(this.grpMacro);
            this.Controls.Add(this.chkRCP2);
            this.Controls.Add(this.chkRCP3);
            this.Controls.Add(this.chkRCP4);
            this.Controls.Add(this.btnFull);
            this.Controls.Add(this.lblBCD);
            this.Controls.Add(this.btnByp);
            this.Controls.Add(this.grpAmp);
            this.Controls.Add(this.AOT);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpStepCtrl);
            this.Controls.Add(this.lblAvg);
            this.Controls.Add(this.txtAvg);
            this.Controls.Add(this.txtTemp);
            this.Controls.Add(this.btnReStart);
            this.Controls.Add(this.txtSWR);
            this.Controls.Add(this.lblSWR);
            this.Controls.Add(this.btnProfiler);
            this.Controls.Add(this.txtFwd);
            this.Controls.Add(this.lblFwd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Mini";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DDUtil - (Not Connected)";
            this.toolTip1.SetToolTip(this, "Double Click the background to reveal more controls.");
            this.Load += new System.EventHandler(this.Mini_Load);
            this.DoubleClick += new System.EventHandler(this.Mini_DoubleClick);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Mini_FormClosing);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Mini_FormClosing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Mini_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Mini_KeyDown);
            this.grpStepCtrl.ResumeLayout(false);
            this.grpStepCtrl.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpAmp.ResumeLayout(false);
            this.grpAmp.PerformLayout();
            this.grpMacro.ResumeLayout(false);
            this.grpMacro.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReStart;
        private System.Windows.Forms.Label lblSWR;
        private System.Windows.Forms.Button btnProfiler;
        private System.Windows.Forms.Button btnSP;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox grpStepCtrl;
        private System.Windows.Forms.Button btnCalib;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox txtFwd;
        public System.Windows.Forms.TextBox txtAvg;
        public System.Windows.Forms.TextBox txtTemp;
        public System.Windows.Forms.TextBox txtSWR;
        public System.Windows.Forms.RadioButton rb34;
        public System.Windows.Forms.RadioButton rbBiDir;
        public System.Windows.Forms.RadioButton rb180;
        public System.Windows.Forms.RadioButton rbFwd;
        public System.Windows.Forms.Label lblFwd;
        public System.Windows.Forms.Label lblAvg;
        private System.Windows.Forms.CheckBox AOT;
        public System.Windows.Forms.TextBox txtSP;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.Label label71;
        public System.Windows.Forms.Button btnHF;
        public System.Windows.Forms.Button btnSF;
        public System.Windows.Forms.TextBox txtMsg;
        public System.Windows.Forms.Button btnHV;
        public System.Windows.Forms.Button btnOper;
        public System.Windows.Forms.Button btnTune;
        public System.Windows.Forms.Button btnPwr;
        public System.Windows.Forms.GroupBox grpAmp;
        public System.Windows.Forms.Label lblHF;
        public System.Windows.Forms.Label lblSF;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        public System.Windows.Forms.TextBox textBox3;
        public System.Windows.Forms.Button btnByp;
        public System.Windows.Forms.Label lblBCD;
        private System.Windows.Forms.Button btnFull;
        public System.Windows.Forms.CheckBox chkRCP2;
        public System.Windows.Forms.CheckBox chkRCP3;
        public System.Windows.Forms.CheckBox chkRCP4;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.GroupBox grpMacro;
        private System.Windows.Forms.Button btnMzcro1;
        private System.Windows.Forms.Button btnMacro8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnMacro7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnMacro6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnMacro5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnMacro4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnMacro3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMacro2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMacro16;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnMacro15;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnMacro14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnMacro13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnMacro12;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnMacro11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnMacro10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnMacro9;
        private System.Windows.Forms.Label label15;
    }
}