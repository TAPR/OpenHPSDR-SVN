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
            this.grpStepCtrl = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpStepCtrl.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAvg
            // 
            this.lblAvg.AutoSize = true;
            this.lblAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvg.ForeColor = System.Drawing.Color.Navy;
            this.lblAvg.Location = new System.Drawing.Point(125, 10);
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
            this.txtTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTemp.Location = new System.Drawing.Point(367, 4);
            this.txtTemp.Name = "txtTemp";
            this.txtTemp.Size = new System.Drawing.Size(50, 22);
            this.txtTemp.TabIndex = 77;
            this.txtTemp.Text = "Temp";
            this.txtTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTemp, "F5000 PA Temp");
            // 
            // btnReStart
            // 
            this.btnReStart.AutoSize = true;
            this.btnReStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnReStart.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnReStart.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnReStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnReStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnReStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold);
            this.btnReStart.ForeColor = System.Drawing.Color.Navy;
            this.btnReStart.Location = new System.Drawing.Point(299, 5);
            this.btnReStart.Name = "btnReStart";
            this.btnReStart.Size = new System.Drawing.Size(62, 21);
            this.btnReStart.TabIndex = 82;
            this.btnReStart.Text = "PFClose";
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
            this.lblSWR.Location = new System.Drawing.Point(196, 10);
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
            this.btnProfiler.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnProfiler.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnProfiler.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this.btnProfiler.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnProfiler.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnProfiler.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold);
            this.btnProfiler.ForeColor = System.Drawing.Color.Navy;
            this.btnProfiler.Location = new System.Drawing.Point(230, 5);
            this.btnProfiler.Name = "btnProfiler";
            this.btnProfiler.Size = new System.Drawing.Size(62, 21);
            this.btnProfiler.TabIndex = 81;
            this.btnProfiler.Text = "PF Open";
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
            this.lblFwd.Location = new System.Drawing.Point(50, 10);
            this.lblFwd.Name = "lblFwd";
            this.lblFwd.Size = new System.Drawing.Size(26, 12);
            this.lblFwd.TabIndex = 76;
            this.lblFwd.Text = "Peak";
            this.lblFwd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSP
            // 
            this.btnSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSP.ForeColor = System.Drawing.Color.Blue;
            this.btnSP.Location = new System.Drawing.Point(55, 15);
            this.btnSP.Name = "btnSP";
            this.btnSP.Size = new System.Drawing.Size(36, 20);
            this.btnSP.TabIndex = 0;
            this.btnSP.Text = "Go";
            this.btnSP.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.btnSP, "Click to turn rotor to heading (Ctrl+Click to stop rotor)");
            this.btnSP.UseVisualStyleBackColor = true;
            this.btnSP.Click += new System.EventHandler(this.btnSP_Click);
            this.btnSP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSP_KeyDown);
            // 
            // txtSP
            // 
            this.txtSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSP.ForeColor = System.Drawing.Color.Firebrick;
            this.txtSP.Location = new System.Drawing.Point(2, 15);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(45, 20);
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
            this.rb34.Location = new System.Drawing.Point(155, 17);
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
            this.rbBiDir.Location = new System.Drawing.Point(105, 17);
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
            this.rb180.Location = new System.Drawing.Point(55, 17);
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
            this.btnCalib.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCalib.BackColor = System.Drawing.SystemColors.Control;
            this.btnCalib.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCalib.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnCalib.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnCalib.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCalib.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalib.ForeColor = System.Drawing.Color.Navy;
            this.btnCalib.Location = new System.Drawing.Point(250, 11);
            this.btnCalib.Name = "btnCalib";
            this.btnCalib.Size = new System.Drawing.Size(55, 21);
            this.btnCalib.TabIndex = 98;
            this.btnCalib.Text = "Calibrate";
            this.btnCalib.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.btnCalib, "Calibrate antenna elements (home & return).");
            this.btnCalib.UseVisualStyleBackColor = false;
            this.btnCalib.Click += new System.EventHandler(this.btnCalib_Click);
            // 
            // btnHome
            // 
            this.btnHome.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnHome.BackColor = System.Drawing.SystemColors.Control;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.Navy;
            this.btnHome.Location = new System.Drawing.Point(205, 11);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(40, 21);
            this.btnHome.TabIndex = 97;
            this.btnHome.Text = "Home";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.btnHome, "Retract the antenna elements (Home).");
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // AOT
            // 
            this.AOT.AutoSize = true;
            this.AOT.Location = new System.Drawing.Point(5, 80);
            this.AOT.Name = "AOT";
            this.AOT.Size = new System.Drawing.Size(62, 17);
            this.AOT.TabIndex = 87;
            this.AOT.Text = "On Top";
            this.toolTip1.SetToolTip(this.AOT, "When selected causes this window to be on top of all other windows.");
            this.AOT.UseVisualStyleBackColor = true;
            this.AOT.CheckedChanged += new System.EventHandler(this.AOT_CheckedChanged);
            // 
            // grpStepCtrl
            // 
            this.grpStepCtrl.Controls.Add(this.rb34);
            this.grpStepCtrl.Controls.Add(this.rbBiDir);
            this.grpStepCtrl.Controls.Add(this.rb180);
            this.grpStepCtrl.Controls.Add(this.rbFwd);
            this.grpStepCtrl.Controls.Add(this.btnCalib);
            this.grpStepCtrl.Controls.Add(this.btnHome);
            this.grpStepCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpStepCtrl.ForeColor = System.Drawing.Color.Firebrick;
            this.grpStepCtrl.Location = new System.Drawing.Point(5, 30);
            this.grpStepCtrl.Name = "grpStepCtrl";
            this.grpStepCtrl.Size = new System.Drawing.Size(315, 40);
            this.grpStepCtrl.TabIndex = 85;
            this.grpStepCtrl.TabStop = false;
            this.grpStepCtrl.Text = "SteppIR";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSP);
            this.groupBox1.Controls.Add(this.txtSP);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Firebrick;
            this.groupBox1.Location = new System.Drawing.Point(325, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(95, 40);
            this.groupBox1.TabIndex = 86;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rotor";
            // 
            // Mini
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 102);
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
            this.Load += new System.EventHandler(this.Mini_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Mini_FormClosing);
            this.grpStepCtrl.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
    }
}