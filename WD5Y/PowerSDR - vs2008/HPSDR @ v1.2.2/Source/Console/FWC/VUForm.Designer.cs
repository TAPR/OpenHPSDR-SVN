//=================================================================
// VUForm.Designer.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2010  FlexRadio Systems
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact us via email at: sales@flex-radio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    8900 Marybank Dr.
//    Austin, TX 78750
//    USA
//=================================================================

namespace PowerSDR
{
    partial class VUForm
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
            this.chkVUFanHigh = new System.Windows.Forms.CheckBox();
            this.chkVUKeyV = new System.Windows.Forms.CheckBox();
            this.chkVUTXIFU = new System.Windows.Forms.CheckBox();
            this.chkVUKeyU = new System.Windows.Forms.CheckBox();
            this.chkVURXURX2 = new System.Windows.Forms.CheckBox();
            this.chkVURX2U = new System.Windows.Forms.CheckBox();
            this.chkVURXIFU = new System.Windows.Forms.CheckBox();
            this.chkVUK15 = new System.Windows.Forms.CheckBox();
            this.chkVUK18 = new System.Windows.Forms.CheckBox();
            this.chkVUK16 = new System.Windows.Forms.CheckBox();
            this.chkVUKeyVU = new System.Windows.Forms.CheckBox();
            this.chkVUDrvU = new System.Windows.Forms.CheckBox();
            this.chkVUDrvV = new System.Windows.Forms.CheckBox();
            this.chkVUK12 = new System.Windows.Forms.CheckBox();
            this.chkVUK13 = new System.Windows.Forms.CheckBox();
            this.chkVULPwrU = new System.Windows.Forms.CheckBox();
            this.chkVURX2V = new System.Windows.Forms.CheckBox();
            this.chkVUTXU = new System.Windows.Forms.CheckBox();
            this.chkVUTXV = new System.Windows.Forms.CheckBox();
            this.chkVUUIFHG1 = new System.Windows.Forms.CheckBox();
            this.chkVUVIFHG1 = new System.Windows.Forms.CheckBox();
            this.chkVULPwrV = new System.Windows.Forms.CheckBox();
            this.chkVUVIFHG2 = new System.Windows.Forms.CheckBox();
            this.chkVUUIFHG2 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkVUFanHigh
            // 
            this.chkVUFanHigh.AutoSize = true;
            this.chkVUFanHigh.Location = new System.Drawing.Point(33, 32);
            this.chkVUFanHigh.Name = "chkVUFanHigh";
            this.chkVUFanHigh.Size = new System.Drawing.Size(69, 17);
            this.chkVUFanHigh.TabIndex = 0;
            this.chkVUFanHigh.Text = "Fan High";
            this.chkVUFanHigh.UseVisualStyleBackColor = true;
            this.chkVUFanHigh.CheckedChanged += new System.EventHandler(this.chkFanHigh_CheckedChanged);
            // 
            // chkVUKeyV
            // 
            this.chkVUKeyV.AutoSize = true;
            this.chkVUKeyV.Location = new System.Drawing.Point(33, 55);
            this.chkVUKeyV.Name = "chkVUKeyV";
            this.chkVUKeyV.Size = new System.Drawing.Size(54, 17);
            this.chkVUKeyV.TabIndex = 1;
            this.chkVUKeyV.Text = "Key-V";
            this.chkVUKeyV.UseVisualStyleBackColor = true;
            this.chkVUKeyV.CheckedChanged += new System.EventHandler(this.chkVUKeyV_CheckedChanged);
            // 
            // chkVUTXIFU
            // 
            this.chkVUTXIFU.AutoSize = true;
            this.chkVUTXIFU.Location = new System.Drawing.Point(141, 55);
            this.chkVUTXIFU.Name = "chkVUTXIFU";
            this.chkVUTXIFU.Size = new System.Drawing.Size(57, 17);
            this.chkVUTXIFU.TabIndex = 2;
            this.chkVUTXIFU.Text = "TXIFU";
            this.chkVUTXIFU.UseVisualStyleBackColor = true;
            this.chkVUTXIFU.CheckedChanged += new System.EventHandler(this.chkVUTXIFU_CheckedChanged);
            // 
            // chkVUKeyU
            // 
            this.chkVUKeyU.AutoSize = true;
            this.chkVUKeyU.Location = new System.Drawing.Point(33, 78);
            this.chkVUKeyU.Name = "chkVUKeyU";
            this.chkVUKeyU.Size = new System.Drawing.Size(55, 17);
            this.chkVUKeyU.TabIndex = 3;
            this.chkVUKeyU.Text = "Key-U";
            this.chkVUKeyU.UseVisualStyleBackColor = true;
            this.chkVUKeyU.CheckedChanged += new System.EventHandler(this.chkVUKeyU_CheckedChanged);
            // 
            // chkVURXURX2
            // 
            this.chkVURXURX2.AutoSize = true;
            this.chkVURXURX2.Location = new System.Drawing.Point(140, 193);
            this.chkVURXURX2.Name = "chkVURXURX2";
            this.chkVURXURX2.Size = new System.Drawing.Size(70, 17);
            this.chkVURXURX2.TabIndex = 4;
            this.chkVURXURX2.Text = "RXURX2";
            this.chkVURXURX2.UseVisualStyleBackColor = true;
            this.chkVURXURX2.CheckedChanged += new System.EventHandler(this.chkVURXURX2_CheckedChanged);
            // 
            // chkVURX2U
            // 
            this.chkVURX2U.AutoSize = true;
            this.chkVURX2U.Location = new System.Drawing.Point(141, 32);
            this.chkVURX2U.Name = "chkVURX2U";
            this.chkVURX2U.Size = new System.Drawing.Size(55, 17);
            this.chkVURX2U.TabIndex = 5;
            this.chkVURX2U.Text = "RX2U";
            this.chkVURX2U.UseVisualStyleBackColor = true;
            this.chkVURX2U.CheckedChanged += new System.EventHandler(this.chkVURX2U_CheckedChanged);
            // 
            // chkVURXIFU
            // 
            this.chkVURXIFU.AutoSize = true;
            this.chkVURXIFU.Location = new System.Drawing.Point(141, 78);
            this.chkVURXIFU.Name = "chkVURXIFU";
            this.chkVURXIFU.Size = new System.Drawing.Size(58, 17);
            this.chkVURXIFU.TabIndex = 6;
            this.chkVURXIFU.Text = "RXIFU";
            this.chkVURXIFU.UseVisualStyleBackColor = true;
            this.chkVURXIFU.CheckedChanged += new System.EventHandler(this.chkVURXIFU_CheckedChanged);
            // 
            // chkVUK15
            // 
            this.chkVUK15.AutoSize = true;
            this.chkVUK15.Location = new System.Drawing.Point(33, 101);
            this.chkVUK15.Name = "chkVUK15";
            this.chkVUK15.Size = new System.Drawing.Size(45, 17);
            this.chkVUK15.TabIndex = 7;
            this.chkVUK15.Text = "K15";
            this.chkVUK15.UseVisualStyleBackColor = true;
            this.chkVUK15.CheckedChanged += new System.EventHandler(this.chkVUK15_CheckedChanged);
            // 
            // chkVUK18
            // 
            this.chkVUK18.AutoSize = true;
            this.chkVUK18.Location = new System.Drawing.Point(33, 147);
            this.chkVUK18.Name = "chkVUK18";
            this.chkVUK18.Size = new System.Drawing.Size(45, 17);
            this.chkVUK18.TabIndex = 8;
            this.chkVUK18.Text = "K18";
            this.chkVUK18.UseVisualStyleBackColor = true;
            this.chkVUK18.CheckedChanged += new System.EventHandler(this.chkVUK18_CheckedChanged);
            // 
            // chkVUK16
            // 
            this.chkVUK16.AutoSize = true;
            this.chkVUK16.Location = new System.Drawing.Point(33, 170);
            this.chkVUK16.Name = "chkVUK16";
            this.chkVUK16.Size = new System.Drawing.Size(45, 17);
            this.chkVUK16.TabIndex = 9;
            this.chkVUK16.Text = "K16";
            this.chkVUK16.UseVisualStyleBackColor = true;
            this.chkVUK16.CheckedChanged += new System.EventHandler(this.chkVUK16_CheckedChanged);
            // 
            // chkVUKeyVU
            // 
            this.chkVUKeyVU.AutoSize = true;
            this.chkVUKeyVU.Location = new System.Drawing.Point(33, 225);
            this.chkVUKeyVU.Name = "chkVUKeyVU";
            this.chkVUKeyVU.Size = new System.Drawing.Size(62, 17);
            this.chkVUKeyVU.TabIndex = 10;
            this.chkVUKeyVU.Text = "Key VU";
            this.chkVUKeyVU.UseVisualStyleBackColor = true;
            this.chkVUKeyVU.CheckedChanged += new System.EventHandler(this.chkVUKeyVU_CheckedChanged);
            // 
            // chkVUDrvU
            // 
            this.chkVUDrvU.AutoSize = true;
            this.chkVUDrvU.Location = new System.Drawing.Point(33, 248);
            this.chkVUDrvU.Name = "chkVUDrvU";
            this.chkVUDrvU.Size = new System.Drawing.Size(60, 17);
            this.chkVUDrvU.TabIndex = 11;
            this.chkVUDrvU.Text = "DRV-U";
            this.chkVUDrvU.UseVisualStyleBackColor = true;
            this.chkVUDrvU.CheckedChanged += new System.EventHandler(this.chkVUDrvU_CheckedChanged);
            // 
            // chkVUDrvV
            // 
            this.chkVUDrvV.AutoSize = true;
            this.chkVUDrvV.Location = new System.Drawing.Point(33, 271);
            this.chkVUDrvV.Name = "chkVUDrvV";
            this.chkVUDrvV.Size = new System.Drawing.Size(59, 17);
            this.chkVUDrvV.TabIndex = 12;
            this.chkVUDrvV.Text = "DRV-V";
            this.chkVUDrvV.UseVisualStyleBackColor = true;
            this.chkVUDrvV.CheckedChanged += new System.EventHandler(this.chkVUDrvV_CheckedChanged);
            // 
            // chkVUK12
            // 
            this.chkVUK12.AutoSize = true;
            this.chkVUK12.Location = new System.Drawing.Point(33, 193);
            this.chkVUK12.Name = "chkVUK12";
            this.chkVUK12.Size = new System.Drawing.Size(45, 17);
            this.chkVUK12.TabIndex = 13;
            this.chkVUK12.Text = "K12";
            this.chkVUK12.UseVisualStyleBackColor = true;
            this.chkVUK12.CheckedChanged += new System.EventHandler(this.chkVUK12_CheckedChanged);
            // 
            // chkVUK13
            // 
            this.chkVUK13.AutoSize = true;
            this.chkVUK13.Location = new System.Drawing.Point(33, 124);
            this.chkVUK13.Name = "chkVUK13";
            this.chkVUK13.Size = new System.Drawing.Size(45, 17);
            this.chkVUK13.TabIndex = 14;
            this.chkVUK13.Text = "K13";
            this.chkVUK13.UseVisualStyleBackColor = true;
            this.chkVUK13.CheckedChanged += new System.EventHandler(this.chkVUK13_CheckedChanged);
            // 
            // chkVULPwrU
            // 
            this.chkVULPwrU.AutoSize = true;
            this.chkVULPwrU.Location = new System.Drawing.Point(33, 294);
            this.chkVULPwrU.Name = "chkVULPwrU";
            this.chkVULPwrU.Size = new System.Drawing.Size(69, 17);
            this.chkVULPwrU.TabIndex = 15;
            this.chkVULPwrU.Text = "U-LPWR";
            this.chkVULPwrU.UseVisualStyleBackColor = true;
            this.chkVULPwrU.CheckedChanged += new System.EventHandler(this.chkVULPwrU_CheckedChanged);
            // 
            // chkVURX2V
            // 
            this.chkVURX2V.AutoSize = true;
            this.chkVURX2V.Location = new System.Drawing.Point(140, 216);
            this.chkVURX2V.Name = "chkVURX2V";
            this.chkVURX2V.Size = new System.Drawing.Size(54, 17);
            this.chkVURX2V.TabIndex = 16;
            this.chkVURX2V.Text = "RX2V";
            this.chkVURX2V.UseVisualStyleBackColor = true;
            this.chkVURX2V.CheckedChanged += new System.EventHandler(this.chkVURX2V_CheckedChanged);
            // 
            // chkVUTXU
            // 
            this.chkVUTXU.AutoSize = true;
            this.chkVUTXU.Location = new System.Drawing.Point(140, 262);
            this.chkVUTXU.Name = "chkVUTXU";
            this.chkVUTXU.Size = new System.Drawing.Size(51, 17);
            this.chkVUTXU.TabIndex = 17;
            this.chkVUTXU.Text = "TX-U";
            this.chkVUTXU.UseVisualStyleBackColor = true;
            this.chkVUTXU.CheckedChanged += new System.EventHandler(this.chkVUTXU_CheckedChanged);
            // 
            // chkVUTXV
            // 
            this.chkVUTXV.AutoSize = true;
            this.chkVUTXV.Location = new System.Drawing.Point(140, 239);
            this.chkVUTXV.Name = "chkVUTXV";
            this.chkVUTXV.Size = new System.Drawing.Size(50, 17);
            this.chkVUTXV.TabIndex = 18;
            this.chkVUTXV.Text = "TX-V";
            this.chkVUTXV.UseVisualStyleBackColor = true;
            this.chkVUTXV.CheckedChanged += new System.EventHandler(this.chkVUTXV_CheckedChanged);
            // 
            // chkVUUIFHG1
            // 
            this.chkVUUIFHG1.AutoSize = true;
            this.chkVUUIFHG1.Location = new System.Drawing.Point(141, 101);
            this.chkVUUIFHG1.Name = "chkVUUIFHG1";
            this.chkVUUIFHG1.Size = new System.Drawing.Size(71, 17);
            this.chkVUUIFHG1.TabIndex = 19;
            this.chkVUUIFHG1.Text = "U-IF HG1";
            this.chkVUUIFHG1.UseVisualStyleBackColor = true;
            this.chkVUUIFHG1.CheckedChanged += new System.EventHandler(this.chkVUUIFHG1_CheckedChanged);
            // 
            // chkVUVIFHG1
            // 
            this.chkVUVIFHG1.AutoSize = true;
            this.chkVUVIFHG1.Location = new System.Drawing.Point(140, 147);
            this.chkVUVIFHG1.Name = "chkVUVIFHG1";
            this.chkVUVIFHG1.Size = new System.Drawing.Size(70, 17);
            this.chkVUVIFHG1.TabIndex = 20;
            this.chkVUVIFHG1.Text = "V-IF HG1";
            this.chkVUVIFHG1.UseVisualStyleBackColor = true;
            this.chkVUVIFHG1.CheckedChanged += new System.EventHandler(this.chkVUVIFHG1_CheckedChanged);
            // 
            // chkVULPwrV
            // 
            this.chkVULPwrV.AutoSize = true;
            this.chkVULPwrV.Location = new System.Drawing.Point(108, 294);
            this.chkVULPwrV.Name = "chkVULPwrV";
            this.chkVULPwrV.Size = new System.Drawing.Size(68, 17);
            this.chkVULPwrV.TabIndex = 21;
            this.chkVULPwrV.Text = "V-LPWR";
            this.chkVULPwrV.UseVisualStyleBackColor = true;
            this.chkVULPwrV.CheckedChanged += new System.EventHandler(this.chkVULPwrV_CheckedChanged);
            // 
            // chkVUVIFHG2
            // 
            this.chkVUVIFHG2.AutoSize = true;
            this.chkVUVIFHG2.Location = new System.Drawing.Point(140, 170);
            this.chkVUVIFHG2.Name = "chkVUVIFHG2";
            this.chkVUVIFHG2.Size = new System.Drawing.Size(70, 17);
            this.chkVUVIFHG2.TabIndex = 23;
            this.chkVUVIFHG2.Text = "V-IF HG2";
            this.chkVUVIFHG2.UseVisualStyleBackColor = true;
            this.chkVUVIFHG2.CheckedChanged += new System.EventHandler(this.chkVUVIFHG2_CheckedChanged);
            // 
            // chkVUUIFHG2
            // 
            this.chkVUUIFHG2.AutoSize = true;
            this.chkVUUIFHG2.Location = new System.Drawing.Point(141, 124);
            this.chkVUUIFHG2.Name = "chkVUUIFHG2";
            this.chkVUUIFHG2.Size = new System.Drawing.Size(71, 17);
            this.chkVUUIFHG2.TabIndex = 22;
            this.chkVUUIFHG2.Text = "U-IF HG2";
            this.chkVUUIFHG2.UseVisualStyleBackColor = true;
            this.chkVUUIFHG2.CheckedChanged += new System.EventHandler(this.chkVUUIFHG2_CheckedChanged);
            // 
            // VUForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 325);
            this.Controls.Add(this.chkVUVIFHG2);
            this.Controls.Add(this.chkVUUIFHG2);
            this.Controls.Add(this.chkVULPwrV);
            this.Controls.Add(this.chkVUVIFHG1);
            this.Controls.Add(this.chkVUUIFHG1);
            this.Controls.Add(this.chkVUTXV);
            this.Controls.Add(this.chkVUTXU);
            this.Controls.Add(this.chkVURX2V);
            this.Controls.Add(this.chkVULPwrU);
            this.Controls.Add(this.chkVUK13);
            this.Controls.Add(this.chkVUK12);
            this.Controls.Add(this.chkVUDrvV);
            this.Controls.Add(this.chkVUDrvU);
            this.Controls.Add(this.chkVUKeyVU);
            this.Controls.Add(this.chkVUK16);
            this.Controls.Add(this.chkVUK18);
            this.Controls.Add(this.chkVUK15);
            this.Controls.Add(this.chkVURXIFU);
            this.Controls.Add(this.chkVURX2U);
            this.Controls.Add(this.chkVURXURX2);
            this.Controls.Add(this.chkVUKeyU);
            this.Controls.Add(this.chkVUTXIFU);
            this.Controls.Add(this.chkVUKeyV);
            this.Controls.Add(this.chkVUFanHigh);
            this.Name = "VUForm";
            this.Text = "VUForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VUForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkVUFanHigh;
        private System.Windows.Forms.CheckBox chkVUKeyV;
        private System.Windows.Forms.CheckBox chkVUTXIFU;
        private System.Windows.Forms.CheckBox chkVUKeyU;
        private System.Windows.Forms.CheckBox chkVURXURX2;
        private System.Windows.Forms.CheckBox chkVURX2U;
        private System.Windows.Forms.CheckBox chkVURXIFU;
        private System.Windows.Forms.CheckBox chkVUK15;
        private System.Windows.Forms.CheckBox chkVUK18;
        private System.Windows.Forms.CheckBox chkVUK16;
        private System.Windows.Forms.CheckBox chkVUKeyVU;
        private System.Windows.Forms.CheckBox chkVUDrvU;
        private System.Windows.Forms.CheckBox chkVUDrvV;
        private System.Windows.Forms.CheckBox chkVUK12;
        private System.Windows.Forms.CheckBox chkVUK13;
        private System.Windows.Forms.CheckBox chkVULPwrU;
        private System.Windows.Forms.CheckBox chkVURX2V;
        private System.Windows.Forms.CheckBox chkVUTXU;
        private System.Windows.Forms.CheckBox chkVUTXV;
        private System.Windows.Forms.CheckBox chkVUUIFHG1;
        private System.Windows.Forms.CheckBox chkVUVIFHG1;
        private System.Windows.Forms.CheckBox chkVULPwrV;
        private System.Windows.Forms.CheckBox chkVUVIFHG2;
        private System.Windows.Forms.CheckBox chkVUUIFHG2;
    }
}