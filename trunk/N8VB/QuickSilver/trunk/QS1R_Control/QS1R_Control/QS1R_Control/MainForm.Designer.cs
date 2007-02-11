namespace QS1R_Control
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose ( );
            }
            base.Dispose ( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            this.tabControl1 = new System.Windows.Forms.TabControl ( );
            this.tabPage1 = new System.Windows.Forms.TabPage ( );
            this.tabPage2 = new System.Windows.Forms.TabPage ( );
            this.btnConnect = new System.Windows.Forms.Button ( );
            this.txtVID = new System.Windows.Forms.TextBox ( );
            this.txtPID = new System.Windows.Forms.TextBox ( );
            this.label1 = new System.Windows.Forms.Label ( );
            this.label2 = new System.Windows.Forms.Label ( );
            this.listBox1 = new System.Windows.Forms.ListBox ( );
            this.listBox2 = new System.Windows.Forms.ListBox ( );
            this.btnReadRam = new System.Windows.Forms.Button ( );
            this.txtLength = new System.Windows.Forms.TextBox ( );
            this.txtStart = new System.Windows.Forms.TextBox ( );
            this.btnReadLTCREG = new System.Windows.Forms.Button ( );
            this.listBox3 = new System.Windows.Forms.ListBox ( );
            this.tabControl1.SuspendLayout ( );
            this.tabPage1.SuspendLayout ( );
            this.tabPage2.SuspendLayout ( );
            this.SuspendLayout ( );
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add ( this.tabPage1 );
            this.tabControl1.Controls.Add ( this.tabPage2 );
            this.tabControl1.Location = new System.Drawing.Point ( 12, 30 );
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size ( 781, 515 );
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Turquoise;
            this.tabPage1.Controls.Add ( this.txtLength );
            this.tabPage1.Controls.Add ( this.txtStart );
            this.tabPage1.Controls.Add ( this.btnReadRam );
            this.tabPage1.Controls.Add ( this.listBox2 );
            this.tabPage1.Controls.Add ( this.listBox1 );
            this.tabPage1.Controls.Add ( this.label2 );
            this.tabPage1.Controls.Add ( this.label1 );
            this.tabPage1.Controls.Add ( this.txtPID );
            this.tabPage1.Controls.Add ( this.txtVID );
            this.tabPage1.Controls.Add ( this.btnConnect );
            this.tabPage1.Location = new System.Drawing.Point ( 4, 22 );
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding ( 3 );
            this.tabPage1.Size = new System.Drawing.Size ( 773, 489 );
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Connect";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Cyan;
            this.tabPage2.Controls.Add ( this.listBox3 );
            this.tabPage2.Controls.Add ( this.btnReadLTCREG );
            this.tabPage2.Location = new System.Drawing.Point ( 4, 22 );
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding ( 3 );
            this.tabPage2.Size = new System.Drawing.Size ( 773, 489 );
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "LTC2208";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point ( 20, 26 );
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size ( 75, 23 );
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler ( this.btnConnect_Click );
            // 
            // txtVID
            // 
            this.txtVID.Location = new System.Drawing.Point ( 121, 26 );
            this.txtVID.Name = "txtVID";
            this.txtVID.Size = new System.Drawing.Size ( 69, 20 );
            this.txtVID.TabIndex = 1;
            this.txtVID.Text = "FFFE";
            this.txtVID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPID
            // 
            this.txtPID.Location = new System.Drawing.Point ( 196, 26 );
            this.txtPID.Name = "txtPID";
            this.txtPID.Size = new System.Drawing.Size ( 67, 20 );
            this.txtPID.TabIndex = 2;
            this.txtPID.Text = "00FF";
            this.txtPID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point ( 145, 10 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size ( 25, 13 );
            this.label1.TabIndex = 3;
            this.label1.Text = "VID";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point ( 218, 10 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size ( 25, 13 );
            this.label2.TabIndex = 4;
            this.label2.Text = "PID";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point ( 306, 26 );
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size ( 448, 134 );
            this.listBox1.TabIndex = 5;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point ( 306, 182 );
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size ( 448, 134 );
            this.listBox2.TabIndex = 6;
            // 
            // btnReadRam
            // 
            this.btnReadRam.Location = new System.Drawing.Point ( 20, 205 );
            this.btnReadRam.Name = "btnReadRam";
            this.btnReadRam.Size = new System.Drawing.Size ( 75, 23 );
            this.btnReadRam.TabIndex = 7;
            this.btnReadRam.Text = "Read RAM";
            this.btnReadRam.UseVisualStyleBackColor = true;
            this.btnReadRam.Click += new System.EventHandler ( this.btnReadRam_Click );
            // 
            // txtLength
            // 
            this.txtLength.Location = new System.Drawing.Point ( 196, 205 );
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size ( 67, 20 );
            this.txtLength.TabIndex = 9;
            this.txtLength.Text = "1";
            this.txtLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point ( 121, 205 );
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size ( 69, 20 );
            this.txtStart.TabIndex = 8;
            this.txtStart.Text = "0";
            this.txtStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnReadLTCREG
            // 
            this.btnReadLTCREG.Location = new System.Drawing.Point ( 23, 29 );
            this.btnReadLTCREG.Name = "btnReadLTCREG";
            this.btnReadLTCREG.Size = new System.Drawing.Size ( 75, 23 );
            this.btnReadLTCREG.TabIndex = 0;
            this.btnReadLTCREG.Text = "Read";
            this.btnReadLTCREG.UseVisualStyleBackColor = true;
            this.btnReadLTCREG.Click += new System.EventHandler ( this.btnReadLTCREG_Click );
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point ( 352, 29 );
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size ( 363, 95 );
            this.listBox3.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 805, 570 );
            this.Controls.Add ( this.tabControl1 );
            this.Name = "Main";
            this.Text = "QS1R Control";
            this.tabControl1.ResumeLayout ( false );
            this.tabPage1.ResumeLayout ( false );
            this.tabPage1.PerformLayout ( );
            this.tabPage2.ResumeLayout ( false );
            this.ResumeLayout ( false );

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtPID;
        private System.Windows.Forms.TextBox txtVID;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReadRam;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.TextBox txtLength;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.Button btnReadLTCREG;
        private System.Windows.Forms.ListBox listBox3;
    }
}

