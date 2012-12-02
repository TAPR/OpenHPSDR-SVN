namespace Test1
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
            this.SendData = new System.Windows.Forms.RichTextBox();
            this.ReceiveData = new System.Windows.Forms.RichTextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ClearButton = new System.Windows.Forms.Button();
            this.FromIP = new System.Windows.Forms.TextBox();
            this.FromMAC = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ToMAC = new System.Windows.Forms.TextBox();
            this.ToIP = new System.Windows.Forms.TextBox();
            this.Frame1 = new System.Windows.Forms.Button();
            this.Frame2 = new System.Windows.Forms.Button();
            this.Frame3 = new System.Windows.Forms.Button();
            this.chkRaw = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // SendData
            // 
            this.SendData.Location = new System.Drawing.Point(108, 32);
            this.SendData.Name = "SendData";
            this.SendData.Size = new System.Drawing.Size(306, 96);
            this.SendData.TabIndex = 0;
            this.SendData.Text = "";
            // 
            // ReceiveData
            // 
            this.ReceiveData.Location = new System.Drawing.Point(108, 246);
            this.ReceiveData.Name = "ReceiveData";
            this.ReceiveData.Size = new System.Drawing.Size(306, 96);
            this.ReceiveData.TabIndex = 1;
            this.ReceiveData.Text = "";
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(227, 134);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 23);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(229, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Send Data";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(229, 230);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Receive Data";
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(227, 348);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(75, 23);
            this.ClearButton.TabIndex = 5;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // FromIP
            // 
            this.FromIP.Location = new System.Drawing.Point(159, 163);
            this.FromIP.Name = "FromIP";
            this.FromIP.Size = new System.Drawing.Size(81, 20);
            this.FromIP.TabIndex = 6;
            this.FromIP.Text = "192.168.0.44";
            // 
            // FromMAC
            // 
            this.FromMAC.Location = new System.Drawing.Point(303, 163);
            this.FromMAC.Name = "FromMAC";
            this.FromMAC.Size = new System.Drawing.Size(98, 20);
            this.FromMAC.TabIndex = 7;
            this.FromMAC.Text = "00 12 34 56 78 90";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(105, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "From IP";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(246, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "From MAC";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(246, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "To  MAC";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(105, 192);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "To  IP";
            // 
            // ToMAC
            // 
            this.ToMAC.Location = new System.Drawing.Point(303, 189);
            this.ToMAC.Name = "ToMAC";
            this.ToMAC.Size = new System.Drawing.Size(98, 20);
            this.ToMAC.TabIndex = 11;
            this.ToMAC.Text = "00 1C C0 30 81 FD";
            // 
            // ToIP
            // 
            this.ToIP.Location = new System.Drawing.Point(159, 189);
            this.ToIP.Name = "ToIP";
            this.ToIP.Size = new System.Drawing.Size(81, 20);
            this.ToIP.TabIndex = 10;
            this.ToIP.Text = "192.168.0.4";
            // 
            // Frame1
            // 
            this.Frame1.Location = new System.Drawing.Point(47, 32);
            this.Frame1.Name = "Frame1";
            this.Frame1.Size = new System.Drawing.Size(55, 23);
            this.Frame1.TabIndex = 14;
            this.Frame1.Text = "Frame 1";
            this.Frame1.UseVisualStyleBackColor = true;
            this.Frame1.Click += new System.EventHandler(this.Frame1_Click);
            // 
            // Frame2
            // 
            this.Frame2.Location = new System.Drawing.Point(47, 61);
            this.Frame2.Name = "Frame2";
            this.Frame2.Size = new System.Drawing.Size(55, 23);
            this.Frame2.TabIndex = 15;
            this.Frame2.Text = "Frame 2";
            this.Frame2.UseVisualStyleBackColor = true;
            this.Frame2.Click += new System.EventHandler(this.Frame2_Click);
            // 
            // Frame3
            // 
            this.Frame3.Location = new System.Drawing.Point(47, 90);
            this.Frame3.Name = "Frame3";
            this.Frame3.Size = new System.Drawing.Size(55, 23);
            this.Frame3.TabIndex = 16;
            this.Frame3.Text = "Frame 3";
            this.Frame3.UseVisualStyleBackColor = true;
            this.Frame3.Click += new System.EventHandler(this.Frame3_Click);
            // 
            // chkRaw
            // 
            this.chkRaw.AutoSize = true;
            this.chkRaw.Location = new System.Drawing.Point(334, 352);
            this.chkRaw.Name = "chkRaw";
            this.chkRaw.Size = new System.Drawing.Size(74, 17);
            this.chkRaw.TabIndex = 17;
            this.chkRaw.Text = "Raw Data";
            this.chkRaw.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 428);
            this.Controls.Add(this.chkRaw);
            this.Controls.Add(this.Frame3);
            this.Controls.Add(this.Frame2);
            this.Controls.Add(this.Frame1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ToMAC);
            this.Controls.Add(this.ToIP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.FromMAC);
            this.Controls.Add(this.FromIP);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.ReceiveData);
            this.Controls.Add(this.SendData);
            this.Name = "Form1";
            this.Text = "Test1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox SendData;
        private System.Windows.Forms.RichTextBox ReceiveData;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.TextBox FromIP;
        private System.Windows.Forms.TextBox FromMAC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ToMAC;
        private System.Windows.Forms.TextBox ToIP;
        private System.Windows.Forms.Button Frame1;
        private System.Windows.Forms.Button Frame2;
        private System.Windows.Forms.Button Frame3;
        private System.Windows.Forms.CheckBox chkRaw;
    }
}

