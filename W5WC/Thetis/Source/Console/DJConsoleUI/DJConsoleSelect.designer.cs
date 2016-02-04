namespace Thetis.DJConsoleUI
{
    partial class DJConsoleSelect
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbConsoleSelect = new System.Windows.Forms.ComboBox();
            this.btnConfigure = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Controller:";
            // 
            // cbConsoleSelect
            // 
            this.cbConsoleSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConsoleSelect.FormattingEnabled = true;
            this.cbConsoleSelect.Location = new System.Drawing.Point(86, 31);
            this.cbConsoleSelect.Name = "cbConsoleSelect";
            this.cbConsoleSelect.Size = new System.Drawing.Size(215, 21);
            this.cbConsoleSelect.TabIndex = 0;
            // 
            // btnConfigure
            // 
            this.btnConfigure.Location = new System.Drawing.Point(129, 77);
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Size = new System.Drawing.Size(71, 23);
            this.btnConfigure.TabIndex = 1;
            this.btnConfigure.Text = "Configure";
            this.btnConfigure.UseVisualStyleBackColor = true;
            this.btnConfigure.Click += new System.EventHandler(this.btnConfigure_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(226, 77);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Save";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // DJConsoleSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 116);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbConsoleSelect);
            this.Controls.Add(this.btnConfigure);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DJConsoleSelect";
            this.Text = "Select DJ Console";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbConsoleSelect;
        private System.Windows.Forms.Button btnConfigure;
        private System.Windows.Forms.Button btnClose;

    }
}