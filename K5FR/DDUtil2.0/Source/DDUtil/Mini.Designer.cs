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
            this.txtSWR = new System.Windows.Forms.TextBox();
            this.lblSWR = new System.Windows.Forms.Label();
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
            this.lblStepFreq = new System.Windows.Forms.Label();
            this.txtVolts = new System.Windows.Forms.TextBox();
            this.txtMemVfo = new System.Windows.Forms.TextBox();
            this.btnMV = new System.Windows.Forms.Button();
            this.btnMemLoad = new System.Windows.Forms.Button();
            this.txtMemIdx = new System.Windows.Forms.TextBox();
            this.btnMemClear = new System.Windows.Forms.Button();
            this.txtMemMode = new System.Windows.Forms.TextBox();
            this.btnReCall = new System.Windows.Forms.Button();
            this.txtMemFreq = new System.Windows.Forms.TextBox();
            this.btnMemSave = new System.Windows.Forms.Button();
            this.grpMacro = new System.Windows.Forms.GroupBox();
            this.btnMacro16 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnMacro15 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.btnMacro14 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.btnMacro13 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btnMacro12 = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.btnMacro11 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.btnMacro10 = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.btnMacro9 = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.btnMacro8 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnMacro7 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnMacro6 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnMacro5 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnMacro4 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnMacro3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMacro2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnMacro1 = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.TkRed = new System.Windows.Forms.Button();
            this.TkYel = new System.Windows.Forms.Button();
            this.TkGrn = new System.Windows.Forms.Button();
            this.grpStepCtrl = new System.Windows.Forms.GroupBox();
            this.grpRotor = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.grpAmp = new System.Windows.Forms.GroupBox();
            this.txtTune = new System.Windows.Forms.TextBox();
            this.lblTune = new System.Windows.Forms.Label();
            this.lblHF = new System.Windows.Forms.Label();
            this.lblSF = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.grpMacro.SuspendLayout();
            this.grpStepCtrl.SuspendLayout();
            this.grpRotor.SuspendLayout();
            this.grpAmp.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAvg
            // 
            this.lblAvg.AutoSize = true;
            this.lblAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvg.ForeColor = System.Drawing.Color.Navy;
            this.lblAvg.Location = new System.Drawing.Point(125, 9);
            this.lblAvg.Name = "lblAvg";
            this.lblAvg.Size = new System.Drawing.Size(24, 12);
            this.lblAvg.TabIndex = 83;
            this.lblAvg.Text = "Fwd";
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
            this.txtTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTemp.Location = new System.Drawing.Point(283, 5);
            this.txtTemp.Name = "txtTemp";
            this.txtTemp.Size = new System.Drawing.Size(49, 20);
            this.txtTemp.TabIndex = 77;
            this.txtTemp.Text = "PA Tmp";
            this.txtTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTemp, "PA Temp");
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
            this.btnSP.Location = new System.Drawing.Point(50, 13);
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
            this.txtSP.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtSP_MouseDoubleClick);
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
            this.btnCalib.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnCalib.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCalib.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnCalib.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnCalib.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalib.ForeColor = System.Drawing.Color.Navy;
            this.btnCalib.Location = new System.Drawing.Point(275, 13);
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
            this.btnHome.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.Navy;
            this.btnHome.Location = new System.Drawing.Point(225, 13);
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
            this.AOT.Location = new System.Drawing.Point(199, 140);
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
            this.btnHV.Location = new System.Drawing.Point(130, 28);
            this.btnHV.Name = "btnHV";
            this.btnHV.Size = new System.Drawing.Size(37, 22);
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
            this.btnOper.Location = new System.Drawing.Point(46, 28);
            this.btnOper.Name = "btnOper";
            this.btnOper.Size = new System.Drawing.Size(38, 22);
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
            this.btnTune.Location = new System.Drawing.Point(89, 28);
            this.btnTune.Name = "btnTune";
            this.btnTune.Size = new System.Drawing.Size(37, 22);
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
            this.label71.Location = new System.Drawing.Point(51, 13);
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
            this.btnPwr.Size = new System.Drawing.Size(35, 22);
            this.btnPwr.TabIndex = 0;
            this.btnPwr.Text = "Off";
            this.toolTip1.SetToolTip(this.btnPwr, "Press to turn On/Off amplifier.");
            this.btnPwr.UseVisualStyleBackColor = false;
            this.btnPwr.Click += new System.EventHandler(this.btnPwr_Click);
            // 
            // btnByp
            // 
            this.btnByp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnByp.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnByp.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnByp.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnByp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnByp.Location = new System.Drawing.Point(401, 5);
            this.btnByp.Name = "btnByp";
            this.btnByp.Size = new System.Drawing.Size(19, 19);
            this.btnByp.TabIndex = 89;
            this.toolTip1.SetToolTip(this.btnByp, "Click to toggle PTT on/off  (ctrl+O)");
            this.btnByp.UseVisualStyleBackColor = false;
            this.btnByp.Click += new System.EventHandler(this.btnByp_Click);
            // 
            // lblBCD
            // 
            this.lblBCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBCD.ForeColor = System.Drawing.Color.Red;
            this.lblBCD.Location = new System.Drawing.Point(369, 7);
            this.lblBCD.Name = "lblBCD";
            this.lblBCD.Size = new System.Drawing.Size(27, 15);
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
            this.btnFull.Location = new System.Drawing.Point(337, 5);
            this.btnFull.Name = "btnFull";
            this.btnFull.Size = new System.Drawing.Size(27, 19);
            this.btnFull.TabIndex = 83;
            this.btnFull.Text = "FZ";
            this.toolTip1.SetToolTip(this.btnFull, "Click to toggle DDUtil Main form On/Off.");
            this.btnFull.UseVisualStyleBackColor = false;
            this.btnFull.Click += new System.EventHandler(this.btnFull_Click);
            // 
            // lblStepFreq
            // 
            this.lblStepFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepFreq.ForeColor = System.Drawing.Color.Red;
            this.lblStepFreq.Location = new System.Drawing.Point(178, 19);
            this.lblStepFreq.Name = "lblStepFreq";
            this.lblStepFreq.Size = new System.Drawing.Size(41, 15);
            this.lblStepFreq.TabIndex = 104;
            this.lblStepFreq.Text = ". . . . .";
            this.lblStepFreq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.lblStepFreq, "Frequency reported from SteppIR controller.");
            // 
            // txtVolts
            // 
            this.txtVolts.BackColor = System.Drawing.Color.LightYellow;
            this.txtVolts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVolts.Location = new System.Drawing.Point(241, 5);
            this.txtVolts.Name = "txtVolts";
            this.txtVolts.Size = new System.Drawing.Size(36, 20);
            this.txtVolts.TabIndex = 104;
            this.txtVolts.Text = "Volts";
            this.txtVolts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtVolts, "Power Supply voltage.");
            // 
            // txtMemVfo
            // 
            this.txtMemVfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemVfo.ForeColor = System.Drawing.Color.Navy;
            this.txtMemVfo.Location = new System.Drawing.Point(3, 69);
            this.txtMemVfo.Name = "txtMemVfo";
            this.txtMemVfo.Size = new System.Drawing.Size(50, 20);
            this.txtMemVfo.TabIndex = 113;
            this.txtMemVfo.Text = "VFO";
            this.txtMemVfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMemVfo, "VFO frequency saved.");
            this.txtMemVfo.WordWrap = false;
            // 
            // btnMV
            // 
            this.btnMV.AutoSize = true;
            this.btnMV.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnMV.ForeColor = System.Drawing.Color.ForestGreen;
            this.btnMV.Location = new System.Drawing.Point(55, 68);
            this.btnMV.Name = "btnMV";
            this.btnMV.Size = new System.Drawing.Size(49, 23);
            this.btnMV.TabIndex = 107;
            this.btnMV.Text = "V<>M";
            this.toolTip1.SetToolTip(this.btnMV, "Press to toggle between VFO A and selected memory (Ctrl+Z) .");
            this.btnMV.UseVisualStyleBackColor = true;
            this.btnMV.Click += new System.EventHandler(this.btnMV_Click);
            // 
            // btnMemLoad
            // 
            this.btnMemLoad.AutoSize = true;
            this.btnMemLoad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMemLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnMemLoad.ForeColor = System.Drawing.Color.Firebrick;
            this.btnMemLoad.Location = new System.Drawing.Point(160, 68);
            this.btnMemLoad.Name = "btnMemLoad";
            this.btnMemLoad.Size = new System.Drawing.Size(45, 23);
            this.btnMemLoad.TabIndex = 111;
            this.btnMemLoad.Text = "Load";
            this.toolTip1.SetToolTip(this.btnMemLoad, "Press to load current memory data to PSDR (Ctrl+C).");
            this.btnMemLoad.UseVisualStyleBackColor = true;
            this.btnMemLoad.Click += new System.EventHandler(this.btnMemLoad_Click);
            // 
            // txtMemIdx
            // 
            this.txtMemIdx.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMemIdx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemIdx.ForeColor = System.Drawing.Color.Navy;
            this.txtMemIdx.Location = new System.Drawing.Point(208, 70);
            this.txtMemIdx.Name = "txtMemIdx";
            this.txtMemIdx.Size = new System.Drawing.Size(16, 20);
            this.txtMemIdx.TabIndex = 112;
            this.txtMemIdx.Text = "#";
            this.txtMemIdx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMemIdx, "Memory number");
            this.txtMemIdx.WordWrap = false;
            // 
            // btnMemClear
            // 
            this.btnMemClear.AutoSize = true;
            this.btnMemClear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMemClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnMemClear.ForeColor = System.Drawing.Color.Red;
            this.btnMemClear.Location = new System.Drawing.Point(374, 69);
            this.btnMemClear.Name = "btnMemClear";
            this.btnMemClear.Size = new System.Drawing.Size(46, 23);
            this.btnMemClear.TabIndex = 108;
            this.btnMemClear.Text = "Clear";
            this.toolTip1.SetToolTip(this.btnMemClear, "Press to clear selected memory. Ctrl+Shift+Clear to delete ALL memories.");
            this.btnMemClear.UseVisualStyleBackColor = true;
            this.btnMemClear.Click += new System.EventHandler(this.btnMemClear_Click);
            this.btnMemClear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnMemClear_KeyDown);
            // 
            // txtMemMode
            // 
            this.txtMemMode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMemMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemMode.ForeColor = System.Drawing.Color.Navy;
            this.txtMemMode.Location = new System.Drawing.Point(331, 71);
            this.txtMemMode.Name = "txtMemMode";
            this.txtMemMode.Size = new System.Drawing.Size(40, 20);
            this.txtMemMode.TabIndex = 110;
            this.txtMemMode.Text = "MODE";
            this.txtMemMode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMemMode, "Memory Mode");
            this.txtMemMode.WordWrap = false;
            // 
            // btnReCall
            // 
            this.btnReCall.AutoSize = true;
            this.btnReCall.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnReCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnReCall.ForeColor = System.Drawing.Color.Blue;
            this.btnReCall.Location = new System.Drawing.Point(105, 68);
            this.btnReCall.Name = "btnReCall";
            this.btnReCall.Size = new System.Drawing.Size(54, 23);
            this.btnReCall.TabIndex = 105;
            this.btnReCall.Text = "ReCall";
            this.toolTip1.SetToolTip(this.btnReCall, "Press to step-thru memories (Ctrl+X).");
            this.btnReCall.UseVisualStyleBackColor = true;
            this.btnReCall.Click += new System.EventHandler(this.btnReCall_Click);
            // 
            // txtMemFreq
            // 
            this.txtMemFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemFreq.ForeColor = System.Drawing.Color.Navy;
            this.txtMemFreq.Location = new System.Drawing.Point(228, 70);
            this.txtMemFreq.Name = "txtMemFreq";
            this.txtMemFreq.Size = new System.Drawing.Size(50, 20);
            this.txtMemFreq.TabIndex = 109;
            this.txtMemFreq.Text = "MHZ";
            this.txtMemFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtMemFreq, "Memory frequency (double-click [Ctrl+N] to add note).");
            this.txtMemFreq.WordWrap = false;
            this.txtMemFreq.DoubleClick += new System.EventHandler(this.txtMemFreq_DoubleClick);
            // 
            // btnMemSave
            // 
            this.btnMemSave.AutoSize = true;
            this.btnMemSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMemSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnMemSave.ForeColor = System.Drawing.Color.Blue;
            this.btnMemSave.Location = new System.Drawing.Point(282, 69);
            this.btnMemSave.Name = "btnMemSave";
            this.btnMemSave.Size = new System.Drawing.Size(46, 23);
            this.btnMemSave.TabIndex = 106;
            this.btnMemSave.Text = "Save";
            this.toolTip1.SetToolTip(this.btnMemSave, "Press to save current memory data (Ctrl+V).");
            this.btnMemSave.UseVisualStyleBackColor = true;
            this.btnMemSave.Click += new System.EventHandler(this.btnMemSave_Click);
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
            this.grpMacro.Controls.Add(this.btnMacro1);
            this.grpMacro.Controls.Add(this.label28);
            this.grpMacro.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold);
            this.grpMacro.ForeColor = System.Drawing.Color.Firebrick;
            this.grpMacro.Location = new System.Drawing.Point(3, 90);
            this.grpMacro.Name = "grpMacro";
            this.grpMacro.Size = new System.Drawing.Size(430, 43);
            this.grpMacro.TabIndex = 103;
            this.grpMacro.TabStop = false;
            this.toolTip1.SetToolTip(this.grpMacro, "Macro Buttons");
            // 
            // btnMacro16
            // 
            this.btnMacro16.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro16.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro16.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro16.Location = new System.Drawing.Point(404, 24);
            this.btnMacro16.Name = "btnMacro16";
            this.btnMacro16.Size = new System.Drawing.Size(20, 12);
            this.btnMacro16.TabIndex = 170;
            this.btnMacro16.UseVisualStyleBackColor = false;
            this.btnMacro16.Click += new System.EventHandler(this.btnMacro16_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Navy;
            this.label8.Location = new System.Drawing.Point(402, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 12);
            this.label8.TabIndex = 169;
            this.label8.Text = "^F4";
            // 
            // btnMacro15
            // 
            this.btnMacro15.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro15.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro15.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro15.Location = new System.Drawing.Point(378, 24);
            this.btnMacro15.Name = "btnMacro15";
            this.btnMacro15.Size = new System.Drawing.Size(20, 12);
            this.btnMacro15.TabIndex = 168;
            this.btnMacro15.UseVisualStyleBackColor = false;
            this.btnMacro15.Click += new System.EventHandler(this.btnMacro15_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Navy;
            this.label9.Location = new System.Drawing.Point(376, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 12);
            this.label9.TabIndex = 167;
            this.label9.Text = "^F3";
            // 
            // btnMacro14
            // 
            this.btnMacro14.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro14.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro14.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro14.Location = new System.Drawing.Point(352, 24);
            this.btnMacro14.Name = "btnMacro14";
            this.btnMacro14.Size = new System.Drawing.Size(20, 12);
            this.btnMacro14.TabIndex = 166;
            this.btnMacro14.UseVisualStyleBackColor = false;
            this.btnMacro14.Click += new System.EventHandler(this.btnMacro14_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Navy;
            this.label10.Location = new System.Drawing.Point(350, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 12);
            this.label10.TabIndex = 165;
            this.label10.Text = "^F2";
            // 
            // btnMacro13
            // 
            this.btnMacro13.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro13.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro13.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro13.Location = new System.Drawing.Point(325, 24);
            this.btnMacro13.Name = "btnMacro13";
            this.btnMacro13.Size = new System.Drawing.Size(20, 12);
            this.btnMacro13.TabIndex = 164;
            this.btnMacro13.UseVisualStyleBackColor = false;
            this.btnMacro13.Click += new System.EventHandler(this.btnMacro13_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Navy;
            this.label11.Location = new System.Drawing.Point(323, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 12);
            this.label11.TabIndex = 163;
            this.label11.Text = "^F1";
            // 
            // btnMacro12
            // 
            this.btnMacro12.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro12.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro12.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro12.Location = new System.Drawing.Point(297, 24);
            this.btnMacro12.Name = "btnMacro12";
            this.btnMacro12.Size = new System.Drawing.Size(20, 12);
            this.btnMacro12.TabIndex = 162;
            this.btnMacro12.UseVisualStyleBackColor = false;
            this.btnMacro12.Click += new System.EventHandler(this.btnMacro12_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Navy;
            this.label12.Location = new System.Drawing.Point(296, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(24, 12);
            this.label12.TabIndex = 161;
            this.label12.Text = "F12";
            // 
            // btnMacro11
            // 
            this.btnMacro11.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro11.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro11.Location = new System.Drawing.Point(270, 24);
            this.btnMacro11.Name = "btnMacro11";
            this.btnMacro11.Size = new System.Drawing.Size(20, 12);
            this.btnMacro11.TabIndex = 160;
            this.btnMacro11.UseVisualStyleBackColor = false;
            this.btnMacro11.Click += new System.EventHandler(this.btnMacro11_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Navy;
            this.label13.Location = new System.Drawing.Point(269, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 12);
            this.label13.TabIndex = 159;
            this.label13.Text = "F11";
            // 
            // btnMacro10
            // 
            this.btnMacro10.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro10.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro10.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro10.Location = new System.Drawing.Point(243, 24);
            this.btnMacro10.Name = "btnMacro10";
            this.btnMacro10.Size = new System.Drawing.Size(20, 12);
            this.btnMacro10.TabIndex = 158;
            this.btnMacro10.UseVisualStyleBackColor = false;
            this.btnMacro10.Click += new System.EventHandler(this.btnMacro10_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Navy;
            this.label14.Location = new System.Drawing.Point(242, 8);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(24, 12);
            this.label14.TabIndex = 157;
            this.label14.Text = "F10";
            // 
            // btnMacro9
            // 
            this.btnMacro9.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro9.Location = new System.Drawing.Point(217, 24);
            this.btnMacro9.Name = "btnMacro9";
            this.btnMacro9.Size = new System.Drawing.Size(20, 12);
            this.btnMacro9.TabIndex = 156;
            this.btnMacro9.UseVisualStyleBackColor = false;
            this.btnMacro9.Click += new System.EventHandler(this.btnMacro9_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Navy;
            this.label15.Location = new System.Drawing.Point(218, 8);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(18, 12);
            this.label15.TabIndex = 155;
            this.label15.Text = "F9";
            // 
            // btnMacro8
            // 
            this.btnMacro8.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro8.Location = new System.Drawing.Point(191, 24);
            this.btnMacro8.Name = "btnMacro8";
            this.btnMacro8.Size = new System.Drawing.Size(20, 12);
            this.btnMacro8.TabIndex = 154;
            this.btnMacro8.UseVisualStyleBackColor = false;
            this.btnMacro8.Click += new System.EventHandler(this.btnMacro8_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Navy;
            this.label7.Location = new System.Drawing.Point(192, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 12);
            this.label7.TabIndex = 153;
            this.label7.Text = "F8";
            // 
            // btnMacro7
            // 
            this.btnMacro7.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro7.Location = new System.Drawing.Point(165, 24);
            this.btnMacro7.Name = "btnMacro7";
            this.btnMacro7.Size = new System.Drawing.Size(20, 12);
            this.btnMacro7.TabIndex = 152;
            this.btnMacro7.UseVisualStyleBackColor = false;
            this.btnMacro7.Click += new System.EventHandler(this.btnMacro7_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Navy;
            this.label6.Location = new System.Drawing.Point(166, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 12);
            this.label6.TabIndex = 151;
            this.label6.Text = "F7";
            // 
            // btnMacro6
            // 
            this.btnMacro6.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro6.Location = new System.Drawing.Point(139, 24);
            this.btnMacro6.Name = "btnMacro6";
            this.btnMacro6.Size = new System.Drawing.Size(20, 12);
            this.btnMacro6.TabIndex = 150;
            this.btnMacro6.UseVisualStyleBackColor = false;
            this.btnMacro6.Click += new System.EventHandler(this.btnMacro6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Navy;
            this.label5.Location = new System.Drawing.Point(140, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 12);
            this.label5.TabIndex = 149;
            this.label5.Text = "F6";
            // 
            // btnMacro5
            // 
            this.btnMacro5.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro5.Location = new System.Drawing.Point(112, 24);
            this.btnMacro5.Name = "btnMacro5";
            this.btnMacro5.Size = new System.Drawing.Size(20, 12);
            this.btnMacro5.TabIndex = 148;
            this.btnMacro5.UseVisualStyleBackColor = false;
            this.btnMacro5.Click += new System.EventHandler(this.btnMacro5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Navy;
            this.label4.Location = new System.Drawing.Point(113, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 12);
            this.label4.TabIndex = 147;
            this.label4.Text = "F5";
            // 
            // btnMacro4
            // 
            this.btnMacro4.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro4.Location = new System.Drawing.Point(85, 24);
            this.btnMacro4.Name = "btnMacro4";
            this.btnMacro4.Size = new System.Drawing.Size(20, 12);
            this.btnMacro4.TabIndex = 146;
            this.btnMacro4.UseVisualStyleBackColor = false;
            this.btnMacro4.Click += new System.EventHandler(this.btnMacro4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(86, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 12);
            this.label3.TabIndex = 145;
            this.label3.Text = "F4";
            // 
            // btnMacro3
            // 
            this.btnMacro3.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro3.Location = new System.Drawing.Point(58, 24);
            this.btnMacro3.Name = "btnMacro3";
            this.btnMacro3.Size = new System.Drawing.Size(20, 12);
            this.btnMacro3.TabIndex = 144;
            this.btnMacro3.UseVisualStyleBackColor = false;
            this.btnMacro3.Click += new System.EventHandler(this.btnMacro3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(59, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 12);
            this.label2.TabIndex = 143;
            this.label2.Text = "F3";
            // 
            // btnMacro2
            // 
            this.btnMacro2.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro2.Location = new System.Drawing.Point(31, 24);
            this.btnMacro2.Name = "btnMacro2";
            this.btnMacro2.Size = new System.Drawing.Size(20, 12);
            this.btnMacro2.TabIndex = 142;
            this.btnMacro2.UseVisualStyleBackColor = false;
            this.btnMacro2.Click += new System.EventHandler(this.btnMacro2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(32, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 12);
            this.label1.TabIndex = 141;
            this.label1.Text = "F2";
            // 
            // btnMacro1
            // 
            this.btnMacro1.BackColor = System.Drawing.SystemColors.Info;
            this.btnMacro1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMacro1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMacro1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMacro1.Location = new System.Drawing.Point(5, 24);
            this.btnMacro1.Name = "btnMacro1";
            this.btnMacro1.Size = new System.Drawing.Size(20, 12);
            this.btnMacro1.TabIndex = 140;
            this.btnMacro1.UseVisualStyleBackColor = false;
            this.btnMacro1.Click += new System.EventHandler(this.btnMacro1_Click);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.Navy;
            this.label28.Location = new System.Drawing.Point(6, 8);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(18, 12);
            this.label28.TabIndex = 114;
            this.label28.Text = "F1";
            // 
            // TkRed
            // 
            this.TkRed.BackColor = System.Drawing.SystemColors.Control;
            this.TkRed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TkRed.Location = new System.Drawing.Point(417, 28);
            this.TkRed.Name = "TkRed";
            this.TkRed.Size = new System.Drawing.Size(10, 10);
            this.TkRed.TabIndex = 116;
            this.toolTip1.SetToolTip(this.TkRed, "Tuning Knob Long-Click (On = Mode A, Off = Mode B)");
            this.TkRed.UseVisualStyleBackColor = false;
            // 
            // TkYel
            // 
            this.TkYel.BackColor = System.Drawing.SystemColors.Control;
            this.TkYel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TkYel.Location = new System.Drawing.Point(417, 43);
            this.TkYel.Name = "TkYel";
            this.TkYel.Size = new System.Drawing.Size(10, 10);
            this.TkYel.TabIndex = 114;
            this.toolTip1.SetToolTip(this.TkYel, "Tuning Knob Short-Click (On/Off)");
            this.TkYel.UseVisualStyleBackColor = false;
            // 
            // TkGrn
            // 
            this.TkGrn.BackColor = System.Drawing.SystemColors.Control;
            this.TkGrn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TkGrn.Location = new System.Drawing.Point(417, 57);
            this.TkGrn.Name = "TkGrn";
            this.TkGrn.Size = new System.Drawing.Size(10, 10);
            this.TkGrn.TabIndex = 115;
            this.toolTip1.SetToolTip(this.TkGrn, "Tuning Knob Double-Click (On/Off)");
            this.TkGrn.UseVisualStyleBackColor = false;
            // 
            // grpStepCtrl
            // 
            this.grpStepCtrl.Controls.Add(this.lblStepFreq);
            this.grpStepCtrl.Controls.Add(this.rb34);
            this.grpStepCtrl.Controls.Add(this.rbBiDir);
            this.grpStepCtrl.Controls.Add(this.rb180);
            this.grpStepCtrl.Controls.Add(this.rbFwd);
            this.grpStepCtrl.Controls.Add(this.btnCalib);
            this.grpStepCtrl.Controls.Add(this.btnHome);
            this.grpStepCtrl.Enabled = false;
            this.grpStepCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold);
            this.grpStepCtrl.ForeColor = System.Drawing.Color.Firebrick;
            this.grpStepCtrl.Location = new System.Drawing.Point(3, 26);
            this.grpStepCtrl.Name = "grpStepCtrl";
            this.grpStepCtrl.Size = new System.Drawing.Size(311, 40);
            this.grpStepCtrl.TabIndex = 85;
            this.grpStepCtrl.TabStop = false;
            this.grpStepCtrl.Text = "SteppIR";
            // 
            // grpRotor
            // 
            this.grpRotor.Controls.Add(this.btnSP);
            this.grpRotor.Controls.Add(this.txtSP);
            this.grpRotor.Controls.Add(this.button4);
            this.grpRotor.Controls.Add(this.button5);
            this.grpRotor.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold);
            this.grpRotor.ForeColor = System.Drawing.Color.Firebrick;
            this.grpRotor.Location = new System.Drawing.Point(320, 26);
            this.grpRotor.Name = "grpRotor";
            this.grpRotor.Size = new System.Drawing.Size(89, 40);
            this.grpRotor.TabIndex = 86;
            this.grpRotor.TabStop = false;
            this.grpRotor.Text = "Rotor";
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
            // 
            // grpAmp
            // 
            this.grpAmp.Controls.Add(this.txtTune);
            this.grpAmp.Controls.Add(this.lblTune);
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
            this.grpAmp.Location = new System.Drawing.Point(437, 5);
            this.grpAmp.Name = "grpAmp";
            this.grpAmp.Size = new System.Drawing.Size(251, 80);
            this.grpAmp.TabIndex = 88;
            this.grpAmp.TabStop = false;
            this.grpAmp.Text = "Amplifier";
            // 
            // txtTune
            // 
            this.txtTune.BackColor = System.Drawing.SystemColors.Info;
            this.txtTune.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTune.Location = new System.Drawing.Point(173, 29);
            this.txtTune.Name = "txtTune";
            this.txtTune.Size = new System.Drawing.Size(26, 20);
            this.txtTune.TabIndex = 60;
            this.txtTune.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTune.DoubleClick += new System.EventHandler(this.txtTune_DoubleClick);
            this.txtTune.TextChanged += new System.EventHandler(this.txtTune_TextChanged);
            // 
            // lblTune
            // 
            this.lblTune.AutoSize = true;
            this.lblTune.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.lblTune.ForeColor = System.Drawing.Color.Navy;
            this.lblTune.Location = new System.Drawing.Point(174, 12);
            this.lblTune.Name = "lblTune";
            this.lblTune.Size = new System.Drawing.Size(23, 13);
            this.lblTune.TabIndex = 61;
            this.lblTune.Text = "Ant";
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
            this.label77.Location = new System.Drawing.Point(137, 13);
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
            this.label76.Location = new System.Drawing.Point(8, 13);
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
            this.label73.Location = new System.Drawing.Point(93, 13);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(30, 13);
            this.label73.TabIndex = 11;
            this.label73.Text = "Tune";
            // 
            // Mini
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(703, 160);
            this.Controls.Add(this.TkRed);
            this.Controls.Add(this.TkYel);
            this.Controls.Add(this.TkGrn);
            this.Controls.Add(this.txtMemVfo);
            this.Controls.Add(this.btnMV);
            this.Controls.Add(this.btnMemLoad);
            this.Controls.Add(this.txtMemIdx);
            this.Controls.Add(this.btnMemClear);
            this.Controls.Add(this.txtMemMode);
            this.Controls.Add(this.btnReCall);
            this.Controls.Add(this.txtMemFreq);
            this.Controls.Add(this.btnMemSave);
            this.Controls.Add(this.txtVolts);
            this.Controls.Add(this.grpMacro);
            this.Controls.Add(this.btnFull);
            this.Controls.Add(this.lblBCD);
            this.Controls.Add(this.btnByp);
            this.Controls.Add(this.grpAmp);
            this.Controls.Add(this.AOT);
            this.Controls.Add(this.grpRotor);
            this.Controls.Add(this.grpStepCtrl);
            this.Controls.Add(this.lblAvg);
            this.Controls.Add(this.txtAvg);
            this.Controls.Add(this.txtTemp);
            this.Controls.Add(this.txtSWR);
            this.Controls.Add(this.lblSWR);
            this.Controls.Add(this.txtFwd);
            this.Controls.Add(this.lblFwd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Mini";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DDUtil - (Not Connected)";
            this.toolTip1.SetToolTip(this, "Double-Click the background to reveal more/less controls.");
            this.Load += new System.EventHandler(this.Mini_Load);
            this.DoubleClick += new System.EventHandler(this.Mini_DoubleClick);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Mini_FormClosing);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Mini_FormClosing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Mini_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Mini_KeyDown);
            this.grpMacro.ResumeLayout(false);
            this.grpMacro.PerformLayout();
            this.grpStepCtrl.ResumeLayout(false);
            this.grpStepCtrl.PerformLayout();
            this.grpRotor.ResumeLayout(false);
            this.grpRotor.PerformLayout();
            this.grpAmp.ResumeLayout(false);
            this.grpAmp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSWR;
        private System.Windows.Forms.Button btnSP;
        private System.Windows.Forms.Button btnCalib;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.GroupBox grpRotor;
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
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.GroupBox grpMacro;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.Label lblStepFreq;
        public System.Windows.Forms.TextBox txtTune;
        public System.Windows.Forms.Label lblTune;
        public System.Windows.Forms.TextBox txtVolts;
        public System.Windows.Forms.TextBox txtMemVfo;
        public System.Windows.Forms.Button btnMV;
        public System.Windows.Forms.Button btnMemLoad;
        public System.Windows.Forms.TextBox txtMemIdx;
        public System.Windows.Forms.Button btnMemClear;
        public System.Windows.Forms.TextBox txtMemMode;
        public System.Windows.Forms.Button btnReCall;
        public System.Windows.Forms.TextBox txtMemFreq;
        public System.Windows.Forms.Button btnMemSave;
        public System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.Button btnMacro1;
        public System.Windows.Forms.Button btnMacro8;
        public System.Windows.Forms.Button btnMacro7;
        public System.Windows.Forms.Button btnMacro6;
        public System.Windows.Forms.Button btnMacro5;
        public System.Windows.Forms.Button btnMacro4;
        public System.Windows.Forms.Button btnMacro3;
        public System.Windows.Forms.Button btnMacro2;
        public System.Windows.Forms.Button btnMacro16;
        public System.Windows.Forms.Button btnMacro15;
        public System.Windows.Forms.Button btnMacro14;
        public System.Windows.Forms.Button btnMacro13;
        public System.Windows.Forms.Button btnMacro12;
        public System.Windows.Forms.Button btnMacro11;
        public System.Windows.Forms.Button btnMacro10;
        public System.Windows.Forms.Button btnMacro9;
        public System.Windows.Forms.Button TkRed;
        public System.Windows.Forms.Button TkYel;
        public System.Windows.Forms.Button TkGrn;
        public System.Windows.Forms.GroupBox grpStepCtrl;
    }
}