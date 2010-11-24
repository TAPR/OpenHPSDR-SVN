namespace DataDecoder
{
    partial class A9500
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(A9500));
            this.grpA95 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label149 = new System.Windows.Forms.Label();
            this.txtA95ig = new System.Windows.Forms.TextBox();
            this.txtA95ip = new System.Windows.Forms.TextBox();
            this.txtA95ant = new System.Windows.Forms.TextBox();
            this.btnA95ant = new System.Windows.Forms.Button();
            this.btnA95aux = new System.Windows.Forms.Button();
            this.label246 = new System.Windows.Forms.Label();
            this.txtA95temp = new System.Windows.Forms.TextBox();
            this.btnA95mode = new System.Windows.Forms.Button();
            this.btnA95on = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.grpA95.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpA95
            // 
            this.grpA95.Controls.Add(this.label1);
            this.grpA95.Controls.Add(this.label149);
            this.grpA95.Controls.Add(this.txtA95ig);
            this.grpA95.Controls.Add(this.txtA95ip);
            this.grpA95.Controls.Add(this.txtA95ant);
            this.grpA95.Controls.Add(this.btnA95ant);
            this.grpA95.Controls.Add(this.btnA95aux);
            this.grpA95.Controls.Add(this.label246);
            this.grpA95.Controls.Add(this.txtA95temp);
            this.grpA95.Controls.Add(this.btnA95mode);
            this.grpA95.Controls.Add(this.btnA95on);
            this.grpA95.Enabled = false;
            this.grpA95.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpA95.ForeColor = System.Drawing.Color.Blue;
            this.grpA95.Location = new System.Drawing.Point(5, 2);
            this.grpA95.Name = "grpA95";
            this.grpA95.Size = new System.Drawing.Size(178, 81);
            this.grpA95.TabIndex = 118;
            this.grpA95.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(98, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 120;
            this.label1.Text = "Igrid";
            // 
            // label149
            // 
            this.label149.AutoSize = true;
            this.label149.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label149.ForeColor = System.Drawing.Color.Navy;
            this.label149.Location = new System.Drawing.Point(52, 12);
            this.label149.Name = "label149";
            this.label149.Size = new System.Drawing.Size(33, 13);
            this.label149.TabIndex = 118;
            this.label149.Text = "Iplate";
            // 
            // txtA95ig
            // 
            this.txtA95ig.BackColor = System.Drawing.SystemColors.Info;
            this.txtA95ig.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtA95ig.Location = new System.Drawing.Point(92, 27);
            this.txtA95ig.Name = "txtA95ig";
            this.txtA95ig.Size = new System.Drawing.Size(39, 18);
            this.txtA95ig.TabIndex = 117;
            this.txtA95ig.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtA95ig, "Grid current (ma).");
            // 
            // txtA95ip
            // 
            this.txtA95ip.BackColor = System.Drawing.SystemColors.Info;
            this.txtA95ip.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtA95ip.Location = new System.Drawing.Point(49, 27);
            this.txtA95ip.Name = "txtA95ip";
            this.txtA95ip.Size = new System.Drawing.Size(39, 18);
            this.txtA95ip.TabIndex = 116;
            this.txtA95ip.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtA95ip, "Plate current (ma).");
            // 
            // txtA95ant
            // 
            this.txtA95ant.BackColor = System.Drawing.SystemColors.Info;
            this.txtA95ant.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtA95ant.Location = new System.Drawing.Point(146, 27);
            this.txtA95ant.Name = "txtA95ant";
            this.txtA95ant.Size = new System.Drawing.Size(15, 18);
            this.txtA95ant.TabIndex = 85;
            this.txtA95ant.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtA95ant, "Currently selected antenna.");
            // 
            // btnA95ant
            // 
            this.btnA95ant.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnA95ant.BackColor = System.Drawing.Color.Transparent;
            this.btnA95ant.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnA95ant.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnA95ant.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnA95ant.ForeColor = System.Drawing.Color.Navy;
            this.btnA95ant.Location = new System.Drawing.Point(135, 51);
            this.btnA95ant.Name = "btnA95ant";
            this.btnA95ant.Size = new System.Drawing.Size(39, 20);
            this.btnA95ant.TabIndex = 82;
            this.btnA95ant.Text = "ANT";
            this.toolTip1.SetToolTip(this.btnA95ant, "Press to select antenna.");
            this.btnA95ant.UseVisualStyleBackColor = false;
            this.btnA95ant.Click += new System.EventHandler(this.btnA95ant_Click);
            // 
            // btnA95aux
            // 
            this.btnA95aux.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnA95aux.BackColor = System.Drawing.Color.Transparent;
            this.btnA95aux.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnA95aux.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnA95aux.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnA95aux.ForeColor = System.Drawing.Color.Navy;
            this.btnA95aux.Location = new System.Drawing.Point(49, 51);
            this.btnA95aux.Name = "btnA95aux";
            this.btnA95aux.Size = new System.Drawing.Size(39, 20);
            this.btnA95aux.TabIndex = 80;
            this.btnA95aux.Text = "AUX";
            this.toolTip1.SetToolTip(this.btnA95aux, "Press to turn Aux power On/Off.");
            this.btnA95aux.UseVisualStyleBackColor = false;
            this.btnA95aux.Click += new System.EventHandler(this.btnA95aux_Click);
            // 
            // label246
            // 
            this.label246.AutoSize = true;
            this.label246.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.label246.ForeColor = System.Drawing.Color.Navy;
            this.label246.Location = new System.Drawing.Point(9, 12);
            this.label246.Name = "label246";
            this.label246.Size = new System.Drawing.Size(32, 13);
            this.label246.TabIndex = 62;
            this.label246.Text = "Temp";
            // 
            // txtA95temp
            // 
            this.txtA95temp.BackColor = System.Drawing.SystemColors.Window;
            this.txtA95temp.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtA95temp.Location = new System.Drawing.Point(6, 27);
            this.txtA95temp.Name = "txtA95temp";
            this.txtA95temp.Size = new System.Drawing.Size(39, 18);
            this.txtA95temp.TabIndex = 61;
            this.txtA95temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtA95temp, "Tube Cathode temperature (C).");
            // 
            // btnA95mode
            // 
            this.btnA95mode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnA95mode.BackColor = System.Drawing.Color.Transparent;
            this.btnA95mode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnA95mode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnA95mode.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnA95mode.ForeColor = System.Drawing.Color.Navy;
            this.btnA95mode.Location = new System.Drawing.Point(92, 51);
            this.btnA95mode.Name = "btnA95mode";
            this.btnA95mode.Size = new System.Drawing.Size(39, 20);
            this.btnA95mode.TabIndex = 73;
            this.btnA95mode.Text = "Mode";
            this.toolTip1.SetToolTip(this.btnA95mode, "Press to toggle Operate/StandBy mode.");
            this.btnA95mode.UseVisualStyleBackColor = false;
            this.btnA95mode.Click += new System.EventHandler(this.btnA95mode_Click);
            // 
            // btnA95on
            // 
            this.btnA95on.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnA95on.BackColor = System.Drawing.Color.Transparent;
            this.btnA95on.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnA95on.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnA95on.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.btnA95on.ForeColor = System.Drawing.Color.Navy;
            this.btnA95on.Location = new System.Drawing.Point(6, 51);
            this.btnA95on.Name = "btnA95on";
            this.btnA95on.Size = new System.Drawing.Size(39, 20);
            this.btnA95on.TabIndex = 72;
            this.btnA95on.Text = "PWR";
            this.toolTip1.SetToolTip(this.btnA95on, "Press to turn Amp Power On.");
            this.btnA95on.UseVisualStyleBackColor = false;
            this.btnA95on.Click += new System.EventHandler(this.btnA95on_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 15000;
            this.toolTip1.InitialDelay = 200;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.StripAmpersands = true;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            // 
            // A9500
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(190, 91);
            this.Controls.Add(this.grpA95);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "A9500";
            this.Text = "Alpha 9500";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.A9500_FormClosing);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.A9500_FormClosing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.A9500_FormClosing);
            this.grpA95.ResumeLayout(false);
            this.grpA95.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox grpA95;
        private System.Windows.Forms.Label label149;
        public System.Windows.Forms.TextBox txtA95ig;
        public System.Windows.Forms.TextBox txtA95ip;
        public System.Windows.Forms.TextBox txtA95ant;
        private System.Windows.Forms.Label label246;
        public System.Windows.Forms.TextBox txtA95temp;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnA95ant;
        public System.Windows.Forms.Button btnA95aux;
        public System.Windows.Forms.Button btnA95mode;
        public System.Windows.Forms.Button btnA95on;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}