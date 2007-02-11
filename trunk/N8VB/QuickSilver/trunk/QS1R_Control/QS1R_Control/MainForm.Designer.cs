/*
 * Created by SharpDevelop.
 * User: Phil
 * Date: 12/24/2006
 * Time: 8:16 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace QS1R_Control
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.btnConnect = new System.Windows.Forms.Button();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.btnReadLEDS = new System.Windows.Forms.Button();
			this.btnWriteLEDS = new System.Windows.Forms.Button();
			this.cbLED4 = new System.Windows.Forms.CheckBox();
			this.cbLED3 = new System.Windows.Forms.CheckBox();
			this.cbLED2 = new System.Windows.Forms.CheckBox();
			this.cbLED1 = new System.Windows.Forms.CheckBox();
			this.cbLED0 = new System.Windows.Forms.CheckBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbPGA = new System.Windows.Forms.CheckBox();
			this.cbRAND = new System.Windows.Forms.CheckBox();
			this.cbSHDN = new System.Windows.Forms.CheckBox();
			this.cbDITH = new System.Windows.Forms.CheckBox();
			this.btnReadADC = new System.Windows.Forms.Button();
			this.btnWriteADC = new System.Windows.Forms.Button();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.lblATTNVal = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.cbATTN5 = new System.Windows.Forms.CheckBox();
			this.cbATTN4 = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.btnReadATTN = new System.Windows.Forms.Button();
			this.btnWriteATTN = new System.Windows.Forms.Button();
			this.cbATTN3 = new System.Windows.Forms.CheckBox();
			this.cbATTN2 = new System.Windows.Forms.CheckBox();
			this.cbATTN1 = new System.Windows.Forms.CheckBox();
			this.cbATTN0 = new System.Windows.Forms.CheckBox();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.btnReadRFFE = new System.Windows.Forms.Button();
			this.btnWriteRFFE = new System.Windows.Forms.Button();
			this.cbRFABP = new System.Windows.Forms.CheckBox();
			this.cbLPFBP = new System.Windows.Forms.CheckBox();
			this.cbHPFBP = new System.Windows.Forms.CheckBox();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.label17 = new System.Windows.Forms.Label();
			this.cbNCOSYNC = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.cbCICSYNC = new System.Windows.Forms.CheckBox();
			this.cbRCFSYNC = new System.Windows.Forms.CheckBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.btnReadDDCCNTRL = new System.Windows.Forms.Button();
			this.btnWriteDDCCNTRL = new System.Windows.Forms.Button();
			this.cbDDC1RESET = new System.Windows.Forms.CheckBox();
			this.cbDDC1PARSER = new System.Windows.Forms.CheckBox();
			this.cbDDC0RESET = new System.Windows.Forms.CheckBox();
			this.cbDDC0PARSER = new System.Windows.Forms.CheckBox();
			this.tabPage7 = new System.Windows.Forms.TabPage();
			this.label25 = new System.Windows.Forms.Label();
			this.cbGPIO3DDR = new System.Windows.Forms.CheckBox();
			this.label18 = new System.Windows.Forms.Label();
			this.cbGPIO2DDR = new System.Windows.Forms.CheckBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.cbGPIO1DDR = new System.Windows.Forms.CheckBox();
			this.cbGPIO0DDR = new System.Windows.Forms.CheckBox();
			this.btnReadGPIO = new System.Windows.Forms.Button();
			this.btnWriteGPIO = new System.Windows.Forms.Button();
			this.cbGPIO3 = new System.Windows.Forms.CheckBox();
			this.cbGPIO2 = new System.Windows.Forms.CheckBox();
			this.cbGPIO1 = new System.Windows.Forms.CheckBox();
			this.cbGPIO0 = new System.Windows.Forms.CheckBox();
			this.tabPage8 = new System.Windows.Forms.TabPage();
			this.btnReadCoef = new System.Windows.Forms.Button();
			this.btnLoadCoef = new System.Windows.Forms.Button();
			this.label36 = new System.Windows.Forms.Label();
			this.nudDDC = new System.Windows.Forms.NumericUpDown();
			this.txtCLRRSVR = new System.Windows.Forms.Button();
			this.txtRTAP = new System.Windows.Forms.TextBox();
			this.txtRAO = new System.Windows.Forms.TextBox();
			this.txtRDEC = new System.Windows.Forms.TextBox();
			this.txtRCR = new System.Windows.Forms.TextBox();
			this.txtC5DEC = new System.Windows.Forms.TextBox();
			this.txtC5SR = new System.Windows.Forms.TextBox();
			this.txtC2DEC = new System.Windows.Forms.TextBox();
			this.txtC2SR = new System.Windows.Forms.TextBox();
			this.txtNPO = new System.Windows.Forms.TextBox();
			this.txtNFRE = new System.Windows.Forms.TextBox();
			this.txtNSCR = new System.Windows.Forms.TextBox();
			this.cbNCRADith = new System.Windows.Forms.CheckBox();
			this.cbNCRDITH = new System.Windows.Forms.CheckBox();
			this.cbNCOBypass = new System.Windows.Forms.CheckBox();
			this.cbMCRMS = new System.Windows.Forms.CheckBox();
			this.cbMCRComplx = new System.Windows.Forms.CheckBox();
			this.cbMCRDCh = new System.Windows.Forms.CheckBox();
			this.cbMCRRST = new System.Windows.Forms.CheckBox();
			this.btnWriteRTAP = new System.Windows.Forms.Button();
			this.btnWriteRAO = new System.Windows.Forms.Button();
			this.btnWriteRDEC = new System.Windows.Forms.Button();
			this.btnWriteRCR = new System.Windows.Forms.Button();
			this.btnWriteC5DEC = new System.Windows.Forms.Button();
			this.btnWriteC5SR = new System.Windows.Forms.Button();
			this.btnWriteC2DEC = new System.Windows.Forms.Button();
			this.btnWriteC2SR = new System.Windows.Forms.Button();
			this.btnWriteNPO = new System.Windows.Forms.Button();
			this.btnWriteNFRE = new System.Windows.Forms.Button();
			this.btnWriteNSCR = new System.Windows.Forms.Button();
			this.btnWriteNCR = new System.Windows.Forms.Button();
			this.btnWriteMCR = new System.Windows.Forms.Button();
			this.btnCLRCOEF = new System.Windows.Forms.Button();
			this.btnClrRam = new System.Windows.Forms.Button();
			this.btnReadRTAP = new System.Windows.Forms.Button();
			this.btnReadRAO = new System.Windows.Forms.Button();
			this.btnReadRDEC = new System.Windows.Forms.Button();
			this.btnReadRCR = new System.Windows.Forms.Button();
			this.btnReadC5DEC = new System.Windows.Forms.Button();
			this.btnReadC5SR = new System.Windows.Forms.Button();
			this.btnReadC2DEC = new System.Windows.Forms.Button();
			this.btnReadC2SR = new System.Windows.Forms.Button();
			this.btnReadNPO = new System.Windows.Forms.Button();
			this.btnReadNFRE = new System.Windows.Forms.Button();
			this.btnReadNSCR = new System.Windows.Forms.Button();
			this.btnReadNCR = new System.Windows.Forms.Button();
			this.btnMCRRead = new System.Windows.Forms.Button();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.tabPage9 = new System.Windows.Forms.TabPage();
			this.txtDDC1AMR = new System.Windows.Forms.TextBox();
			this.txtDDC1LAR = new System.Windows.Forms.TextBox();
			this.txtDDC1DR4 = new System.Windows.Forms.TextBox();
			this.txtDDC1DR3 = new System.Windows.Forms.TextBox();
			this.txtDDC1DR2 = new System.Windows.Forms.TextBox();
			this.txtDDC1DR1 = new System.Windows.Forms.TextBox();
			this.txtDDC1DR0 = new System.Windows.Forms.TextBox();
			this.label29 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.label32 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.btnReadDDC1 = new System.Windows.Forms.Button();
			this.btnWriteDDC1 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.btnClear = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.tabPage6.SuspendLayout();
			this.tabPage7.SuspendLayout();
			this.tabPage8.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudDDC)).BeginInit();
			this.tabPage9.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Controls.Add(this.tabPage6);
			this.tabControl1.Controls.Add(this.tabPage7);
			this.tabControl1.Controls.Add(this.tabPage8);
			this.tabControl1.Controls.Add(this.tabPage9);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(703, 419);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.Black;
			this.tabPage1.Controls.Add(this.btnDisconnect);
			this.tabPage1.Controls.Add(this.listBox1);
			this.tabPage1.Controls.Add(this.btnConnect);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(695, 393);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Device";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Location = new System.Drawing.Point(19, 71);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
			this.btnDisconnect.TabIndex = 2;
			this.btnDisconnect.Text = "Disconnect";
			this.btnDisconnect.UseVisualStyleBackColor = true;
			this.btnDisconnect.Click += new System.EventHandler(this.BtnDisconnectClick);
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(131, 25);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(272, 160);
			this.listBox1.TabIndex = 1;
			// 
			// btnConnect
			// 
			this.btnConnect.Location = new System.Drawing.Point(19, 25);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(75, 23);
			this.btnConnect.TabIndex = 0;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.BtnConnectClick);
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.Transparent;
			this.tabPage2.Controls.Add(this.btnReadLEDS);
			this.tabPage2.Controls.Add(this.btnWriteLEDS);
			this.tabPage2.Controls.Add(this.cbLED4);
			this.tabPage2.Controls.Add(this.cbLED3);
			this.tabPage2.Controls.Add(this.cbLED2);
			this.tabPage2.Controls.Add(this.cbLED1);
			this.tabPage2.Controls.Add(this.cbLED0);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(695, 393);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Debug_Leds";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// btnReadLEDS
			// 
			this.btnReadLEDS.Location = new System.Drawing.Point(19, 62);
			this.btnReadLEDS.Name = "btnReadLEDS";
			this.btnReadLEDS.Size = new System.Drawing.Size(75, 23);
			this.btnReadLEDS.TabIndex = 6;
			this.btnReadLEDS.Text = "Read";
			this.btnReadLEDS.UseVisualStyleBackColor = true;
			this.btnReadLEDS.Click += new System.EventHandler(this.BtnReadLEDSClick);
			// 
			// btnWriteLEDS
			// 
			this.btnWriteLEDS.Location = new System.Drawing.Point(19, 24);
			this.btnWriteLEDS.Name = "btnWriteLEDS";
			this.btnWriteLEDS.Size = new System.Drawing.Size(75, 23);
			this.btnWriteLEDS.TabIndex = 5;
			this.btnWriteLEDS.Text = "Write";
			this.btnWriteLEDS.UseVisualStyleBackColor = true;
			this.btnWriteLEDS.Click += new System.EventHandler(this.BtnWriteLEDSClick);
			// 
			// cbLED4
			// 
			this.cbLED4.Location = new System.Drawing.Point(6, 235);
			this.cbLED4.Name = "cbLED4";
			this.cbLED4.Size = new System.Drawing.Size(104, 24);
			this.cbLED4.TabIndex = 4;
			this.cbLED4.Text = "debug Led 4";
			this.cbLED4.UseVisualStyleBackColor = true;
			// 
			// cbLED3
			// 
			this.cbLED3.Location = new System.Drawing.Point(6, 205);
			this.cbLED3.Name = "cbLED3";
			this.cbLED3.Size = new System.Drawing.Size(104, 24);
			this.cbLED3.TabIndex = 3;
			this.cbLED3.Text = "debug Led 3";
			this.cbLED3.UseVisualStyleBackColor = true;
			// 
			// cbLED2
			// 
			this.cbLED2.Location = new System.Drawing.Point(6, 175);
			this.cbLED2.Name = "cbLED2";
			this.cbLED2.Size = new System.Drawing.Size(104, 24);
			this.cbLED2.TabIndex = 2;
			this.cbLED2.Text = "debug Led 2";
			this.cbLED2.UseVisualStyleBackColor = true;
			// 
			// cbLED1
			// 
			this.cbLED1.Location = new System.Drawing.Point(6, 145);
			this.cbLED1.Name = "cbLED1";
			this.cbLED1.Size = new System.Drawing.Size(104, 24);
			this.cbLED1.TabIndex = 1;
			this.cbLED1.Text = "debug Led 1";
			this.cbLED1.UseVisualStyleBackColor = true;
			// 
			// cbLED0
			// 
			this.cbLED0.Location = new System.Drawing.Point(6, 115);
			this.cbLED0.Name = "cbLED0";
			this.cbLED0.Size = new System.Drawing.Size(104, 24);
			this.cbLED0.TabIndex = 0;
			this.cbLED0.Text = "debug Led 0";
			this.cbLED0.UseVisualStyleBackColor = true;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.label1);
			this.tabPage3.Controls.Add(this.label2);
			this.tabPage3.Controls.Add(this.label3);
			this.tabPage3.Controls.Add(this.label4);
			this.tabPage3.Controls.Add(this.cbPGA);
			this.tabPage3.Controls.Add(this.cbRAND);
			this.tabPage3.Controls.Add(this.cbSHDN);
			this.tabPage3.Controls.Add(this.cbDITH);
			this.tabPage3.Controls.Add(this.btnReadADC);
			this.tabPage3.Controls.Add(this.btnWriteADC);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(695, 393);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "ADC_CNTRL";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(84, 202);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 20);
			this.label1.TabIndex = 30;
			this.label1.Text = "1=x1.5, 0=x1.0";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(84, 174);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 20);
			this.label2.TabIndex = 29;
			this.label2.Text = "1=RAND, 0=OFF";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(84, 141);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 20);
			this.label3.TabIndex = 28;
			this.label3.Text = "1=SHDN, 0=RUN";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(84, 111);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 20);
			this.label4.TabIndex = 27;
			this.label4.Text = "1=DITH, 0=OFF";
			// 
			// cbPGA
			// 
			this.cbPGA.Location = new System.Drawing.Point(14, 197);
			this.cbPGA.Name = "cbPGA";
			this.cbPGA.Size = new System.Drawing.Size(104, 24);
			this.cbPGA.TabIndex = 20;
			this.cbPGA.Text = "PGA";
			this.cbPGA.UseVisualStyleBackColor = true;
			// 
			// cbRAND
			// 
			this.cbRAND.Location = new System.Drawing.Point(14, 167);
			this.cbRAND.Name = "cbRAND";
			this.cbRAND.Size = new System.Drawing.Size(104, 24);
			this.cbRAND.TabIndex = 19;
			this.cbRAND.Text = "RAND";
			this.cbRAND.UseVisualStyleBackColor = true;
			// 
			// cbSHDN
			// 
			this.cbSHDN.Location = new System.Drawing.Point(14, 137);
			this.cbSHDN.Name = "cbSHDN";
			this.cbSHDN.Size = new System.Drawing.Size(104, 24);
			this.cbSHDN.TabIndex = 18;
			this.cbSHDN.Text = "SHDN";
			this.cbSHDN.UseVisualStyleBackColor = true;
			// 
			// cbDITH
			// 
			this.cbDITH.Location = new System.Drawing.Point(14, 107);
			this.cbDITH.Name = "cbDITH";
			this.cbDITH.Size = new System.Drawing.Size(104, 24);
			this.cbDITH.TabIndex = 17;
			this.cbDITH.Text = "DITH";
			this.cbDITH.UseVisualStyleBackColor = true;
			// 
			// btnReadADC
			// 
			this.btnReadADC.Location = new System.Drawing.Point(28, 64);
			this.btnReadADC.Name = "btnReadADC";
			this.btnReadADC.Size = new System.Drawing.Size(75, 23);
			this.btnReadADC.TabIndex = 12;
			this.btnReadADC.Text = "Read";
			this.btnReadADC.UseVisualStyleBackColor = true;
			this.btnReadADC.Click += new System.EventHandler(this.BtnReadADCClick);
			// 
			// btnWriteADC
			// 
			this.btnWriteADC.Location = new System.Drawing.Point(28, 26);
			this.btnWriteADC.Name = "btnWriteADC";
			this.btnWriteADC.Size = new System.Drawing.Size(75, 23);
			this.btnWriteADC.TabIndex = 11;
			this.btnWriteADC.Text = "Write";
			this.btnWriteADC.UseVisualStyleBackColor = true;
			this.btnWriteADC.Click += new System.EventHandler(this.BtnWriteADCClick);
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.lblATTNVal);
			this.tabPage4.Controls.Add(this.label9);
			this.tabPage4.Controls.Add(this.label10);
			this.tabPage4.Controls.Add(this.cbATTN5);
			this.tabPage4.Controls.Add(this.cbATTN4);
			this.tabPage4.Controls.Add(this.label5);
			this.tabPage4.Controls.Add(this.label6);
			this.tabPage4.Controls.Add(this.label7);
			this.tabPage4.Controls.Add(this.label8);
			this.tabPage4.Controls.Add(this.btnReadATTN);
			this.tabPage4.Controls.Add(this.btnWriteATTN);
			this.tabPage4.Controls.Add(this.cbATTN3);
			this.tabPage4.Controls.Add(this.cbATTN2);
			this.tabPage4.Controls.Add(this.cbATTN1);
			this.tabPage4.Controls.Add(this.cbATTN0);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(695, 393);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "ATTN_REG";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// lblATTNVal
			// 
			this.lblATTNVal.Location = new System.Drawing.Point(21, 309);
			this.lblATTNVal.Name = "lblATTNVal";
			this.lblATTNVal.Size = new System.Drawing.Size(100, 17);
			this.lblATTNVal.TabIndex = 31;
			this.lblATTNVal.Text = "Total Attn:";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(90, 270);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(100, 20);
			this.label9.TabIndex = 30;
			this.label9.Text = "0.5 db";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(90, 242);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(100, 20);
			this.label10.TabIndex = 29;
			this.label10.Text = "1 db";
			// 
			// cbATTN5
			// 
			this.cbATTN5.Location = new System.Drawing.Point(21, 265);
			this.cbATTN5.Name = "cbATTN5";
			this.cbATTN5.Size = new System.Drawing.Size(104, 24);
			this.cbATTN5.TabIndex = 28;
			this.cbATTN5.Text = "ATTN[5]";
			this.cbATTN5.UseVisualStyleBackColor = true;
			// 
			// cbATTN4
			// 
			this.cbATTN4.Location = new System.Drawing.Point(21, 235);
			this.cbATTN4.Name = "cbATTN4";
			this.cbATTN4.Size = new System.Drawing.Size(104, 24);
			this.cbATTN4.TabIndex = 27;
			this.cbATTN4.Text = "ATTN[4]";
			this.cbATTN4.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(90, 212);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 20);
			this.label5.TabIndex = 26;
			this.label5.Text = "2 db";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(90, 184);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 20);
			this.label6.TabIndex = 25;
			this.label6.Text = "4 db";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(90, 151);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(100, 20);
			this.label7.TabIndex = 24;
			this.label7.Text = "8 db";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(90, 121);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(100, 20);
			this.label8.TabIndex = 23;
			this.label8.Text = "16 db";
			// 
			// btnReadATTN
			// 
			this.btnReadATTN.Location = new System.Drawing.Point(34, 64);
			this.btnReadATTN.Name = "btnReadATTN";
			this.btnReadATTN.Size = new System.Drawing.Size(75, 23);
			this.btnReadATTN.TabIndex = 22;
			this.btnReadATTN.Text = "Read";
			this.btnReadATTN.UseVisualStyleBackColor = true;
			this.btnReadATTN.Click += new System.EventHandler(this.BtnReadATTNClick);
			// 
			// btnWriteATTN
			// 
			this.btnWriteATTN.Location = new System.Drawing.Point(34, 26);
			this.btnWriteATTN.Name = "btnWriteATTN";
			this.btnWriteATTN.Size = new System.Drawing.Size(75, 23);
			this.btnWriteATTN.TabIndex = 21;
			this.btnWriteATTN.Text = "Write";
			this.btnWriteATTN.UseVisualStyleBackColor = true;
			this.btnWriteATTN.Click += new System.EventHandler(this.BtnWriteATTNClick);
			// 
			// cbATTN3
			// 
			this.cbATTN3.Location = new System.Drawing.Point(21, 207);
			this.cbATTN3.Name = "cbATTN3";
			this.cbATTN3.Size = new System.Drawing.Size(104, 24);
			this.cbATTN3.TabIndex = 20;
			this.cbATTN3.Text = "ATTN[3]";
			this.cbATTN3.UseVisualStyleBackColor = true;
			// 
			// cbATTN2
			// 
			this.cbATTN2.Location = new System.Drawing.Point(21, 177);
			this.cbATTN2.Name = "cbATTN2";
			this.cbATTN2.Size = new System.Drawing.Size(104, 24);
			this.cbATTN2.TabIndex = 19;
			this.cbATTN2.Text = "ATTN[2]";
			this.cbATTN2.UseVisualStyleBackColor = true;
			// 
			// cbATTN1
			// 
			this.cbATTN1.Location = new System.Drawing.Point(21, 147);
			this.cbATTN1.Name = "cbATTN1";
			this.cbATTN1.Size = new System.Drawing.Size(104, 24);
			this.cbATTN1.TabIndex = 18;
			this.cbATTN1.Text = "ATTN[1]";
			this.cbATTN1.UseVisualStyleBackColor = true;
			// 
			// cbATTN0
			// 
			this.cbATTN0.Location = new System.Drawing.Point(21, 117);
			this.cbATTN0.Name = "cbATTN0";
			this.cbATTN0.Size = new System.Drawing.Size(104, 24);
			this.cbATTN0.TabIndex = 17;
			this.cbATTN0.Text = "ATTN[0]";
			this.cbATTN0.UseVisualStyleBackColor = true;
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add(this.btnReadRFFE);
			this.tabPage5.Controls.Add(this.btnWriteRFFE);
			this.tabPage5.Controls.Add(this.cbRFABP);
			this.tabPage5.Controls.Add(this.cbLPFBP);
			this.tabPage5.Controls.Add(this.cbHPFBP);
			this.tabPage5.Location = new System.Drawing.Point(4, 22);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Size = new System.Drawing.Size(695, 393);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "RFFE_CNTRL";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// btnReadRFFE
			// 
			this.btnReadRFFE.Location = new System.Drawing.Point(27, 58);
			this.btnReadRFFE.Name = "btnReadRFFE";
			this.btnReadRFFE.Size = new System.Drawing.Size(75, 23);
			this.btnReadRFFE.TabIndex = 22;
			this.btnReadRFFE.Text = "Read";
			this.btnReadRFFE.UseVisualStyleBackColor = true;
			this.btnReadRFFE.Click += new System.EventHandler(this.BtnReadRFFEClick);
			// 
			// btnWriteRFFE
			// 
			this.btnWriteRFFE.Location = new System.Drawing.Point(27, 20);
			this.btnWriteRFFE.Name = "btnWriteRFFE";
			this.btnWriteRFFE.Size = new System.Drawing.Size(75, 23);
			this.btnWriteRFFE.TabIndex = 21;
			this.btnWriteRFFE.Text = "Write";
			this.btnWriteRFFE.UseVisualStyleBackColor = true;
			this.btnWriteRFFE.Click += new System.EventHandler(this.BtnWriteRFFEClick);
			// 
			// cbRFABP
			// 
			this.cbRFABP.Location = new System.Drawing.Point(14, 171);
			this.cbRFABP.Name = "cbRFABP";
			this.cbRFABP.Size = new System.Drawing.Size(104, 24);
			this.cbRFABP.TabIndex = 19;
			this.cbRFABP.Text = "RFA BYPASS";
			this.cbRFABP.UseVisualStyleBackColor = true;
			// 
			// cbLPFBP
			// 
			this.cbLPFBP.Location = new System.Drawing.Point(14, 141);
			this.cbLPFBP.Name = "cbLPFBP";
			this.cbLPFBP.Size = new System.Drawing.Size(104, 24);
			this.cbLPFBP.TabIndex = 18;
			this.cbLPFBP.Text = "LPF BYPASS";
			this.cbLPFBP.UseVisualStyleBackColor = true;
			// 
			// cbHPFBP
			// 
			this.cbHPFBP.Location = new System.Drawing.Point(14, 111);
			this.cbHPFBP.Name = "cbHPFBP";
			this.cbHPFBP.Size = new System.Drawing.Size(104, 24);
			this.cbHPFBP.TabIndex = 17;
			this.cbHPFBP.Text = "HPF BYPASS";
			this.cbHPFBP.UseVisualStyleBackColor = true;
			// 
			// tabPage6
			// 
			this.tabPage6.Controls.Add(this.label17);
			this.tabPage6.Controls.Add(this.cbNCOSYNC);
			this.tabPage6.Controls.Add(this.label11);
			this.tabPage6.Controls.Add(this.label12);
			this.tabPage6.Controls.Add(this.cbCICSYNC);
			this.tabPage6.Controls.Add(this.cbRCFSYNC);
			this.tabPage6.Controls.Add(this.label13);
			this.tabPage6.Controls.Add(this.label14);
			this.tabPage6.Controls.Add(this.label15);
			this.tabPage6.Controls.Add(this.label16);
			this.tabPage6.Controls.Add(this.btnReadDDCCNTRL);
			this.tabPage6.Controls.Add(this.btnWriteDDCCNTRL);
			this.tabPage6.Controls.Add(this.cbDDC1RESET);
			this.tabPage6.Controls.Add(this.cbDDC1PARSER);
			this.tabPage6.Controls.Add(this.cbDDC0RESET);
			this.tabPage6.Controls.Add(this.cbDDC0PARSER);
			this.tabPage6.Location = new System.Drawing.Point(4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Size = new System.Drawing.Size(695, 393);
			this.tabPage6.TabIndex = 5;
			this.tabPage6.Text = "DDC_CNTRL";
			this.tabPage6.UseVisualStyleBackColor = true;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(135, 294);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(118, 20);
			this.label17.TabIndex = 46;
			this.label17.Text = "1=NCO SYNC, 0=OFF";
			// 
			// cbNCOSYNC
			// 
			this.cbNCOSYNC.Location = new System.Drawing.Point(15, 290);
			this.cbNCOSYNC.Name = "cbNCOSYNC";
			this.cbNCOSYNC.Size = new System.Drawing.Size(104, 24);
			this.cbNCOSYNC.TabIndex = 45;
			this.cbNCOSYNC.Text = "NCO SYNC";
			this.cbNCOSYNC.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(135, 264);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(118, 20);
			this.label11.TabIndex = 44;
			this.label11.Text = "1=CIC SYNC, 0=OFF";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(135, 235);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(118, 20);
			this.label12.TabIndex = 43;
			this.label12.Text = "1=RCF SNYC, 0=OFF";
			// 
			// cbCICSYNC
			// 
			this.cbCICSYNC.Location = new System.Drawing.Point(15, 260);
			this.cbCICSYNC.Name = "cbCICSYNC";
			this.cbCICSYNC.Size = new System.Drawing.Size(104, 24);
			this.cbCICSYNC.TabIndex = 42;
			this.cbCICSYNC.Text = "CIC SYNC";
			this.cbCICSYNC.UseVisualStyleBackColor = true;
			// 
			// cbRCFSYNC
			// 
			this.cbRCFSYNC.Location = new System.Drawing.Point(15, 230);
			this.cbRCFSYNC.Name = "cbRCFSYNC";
			this.cbRCFSYNC.Size = new System.Drawing.Size(104, 24);
			this.cbRCFSYNC.TabIndex = 41;
			this.cbRCFSYNC.Text = "RCF SYNC";
			this.cbRCFSYNC.UseVisualStyleBackColor = true;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(135, 207);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(100, 20);
			this.label13.TabIndex = 40;
			this.label13.Text = "1-RUN, 0=RESET";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(135, 177);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(100, 20);
			this.label14.TabIndex = 39;
			this.label14.Text = "1=PAR, 0=SER";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(135, 147);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(100, 20);
			this.label15.TabIndex = 38;
			this.label15.Text = "1=RUN, 0=RESET";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(135, 117);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(100, 20);
			this.label16.TabIndex = 37;
			this.label16.Text = "1=PAR, 0=SER";
			// 
			// btnReadDDCCNTRL
			// 
			this.btnReadDDCCNTRL.Location = new System.Drawing.Point(28, 59);
			this.btnReadDDCCNTRL.Name = "btnReadDDCCNTRL";
			this.btnReadDDCCNTRL.Size = new System.Drawing.Size(75, 23);
			this.btnReadDDCCNTRL.TabIndex = 36;
			this.btnReadDDCCNTRL.Text = "Read";
			this.btnReadDDCCNTRL.UseVisualStyleBackColor = true;
			this.btnReadDDCCNTRL.Click += new System.EventHandler(this.BtnReadDDCCNTRLClick);
			// 
			// btnWriteDDCCNTRL
			// 
			this.btnWriteDDCCNTRL.Location = new System.Drawing.Point(28, 21);
			this.btnWriteDDCCNTRL.Name = "btnWriteDDCCNTRL";
			this.btnWriteDDCCNTRL.Size = new System.Drawing.Size(75, 23);
			this.btnWriteDDCCNTRL.TabIndex = 35;
			this.btnWriteDDCCNTRL.Text = "Write";
			this.btnWriteDDCCNTRL.UseVisualStyleBackColor = true;
			this.btnWriteDDCCNTRL.Click += new System.EventHandler(this.BtnWriteDDCCNTRLClick);
			// 
			// cbDDC1RESET
			// 
			this.cbDDC1RESET.Location = new System.Drawing.Point(15, 202);
			this.cbDDC1RESET.Name = "cbDDC1RESET";
			this.cbDDC1RESET.Size = new System.Drawing.Size(104, 24);
			this.cbDDC1RESET.TabIndex = 34;
			this.cbDDC1RESET.Text = "DDC1_RESET";
			this.cbDDC1RESET.UseVisualStyleBackColor = true;
			// 
			// cbDDC1PARSER
			// 
			this.cbDDC1PARSER.Location = new System.Drawing.Point(15, 172);
			this.cbDDC1PARSER.Name = "cbDDC1PARSER";
			this.cbDDC1PARSER.Size = new System.Drawing.Size(114, 24);
			this.cbDDC1PARSER.TabIndex = 33;
			this.cbDDC1PARSER.Text = "DDC1_PAR/SER";
			this.cbDDC1PARSER.UseVisualStyleBackColor = true;
			// 
			// cbDDC0RESET
			// 
			this.cbDDC0RESET.Location = new System.Drawing.Point(15, 142);
			this.cbDDC0RESET.Name = "cbDDC0RESET";
			this.cbDDC0RESET.Size = new System.Drawing.Size(104, 24);
			this.cbDDC0RESET.TabIndex = 32;
			this.cbDDC0RESET.Text = "DDC0_RESET";
			this.cbDDC0RESET.UseVisualStyleBackColor = true;
			// 
			// cbDDC0PARSER
			// 
			this.cbDDC0PARSER.Location = new System.Drawing.Point(15, 112);
			this.cbDDC0PARSER.Name = "cbDDC0PARSER";
			this.cbDDC0PARSER.Size = new System.Drawing.Size(114, 24);
			this.cbDDC0PARSER.TabIndex = 31;
			this.cbDDC0PARSER.Text = "DDC0_PAR/SER";
			this.cbDDC0PARSER.UseVisualStyleBackColor = true;
			// 
			// tabPage7
			// 
			this.tabPage7.Controls.Add(this.label25);
			this.tabPage7.Controls.Add(this.cbGPIO3DDR);
			this.tabPage7.Controls.Add(this.label18);
			this.tabPage7.Controls.Add(this.cbGPIO2DDR);
			this.tabPage7.Controls.Add(this.label19);
			this.tabPage7.Controls.Add(this.label20);
			this.tabPage7.Controls.Add(this.cbGPIO1DDR);
			this.tabPage7.Controls.Add(this.cbGPIO0DDR);
			this.tabPage7.Controls.Add(this.btnReadGPIO);
			this.tabPage7.Controls.Add(this.btnWriteGPIO);
			this.tabPage7.Controls.Add(this.cbGPIO3);
			this.tabPage7.Controls.Add(this.cbGPIO2);
			this.tabPage7.Controls.Add(this.cbGPIO1);
			this.tabPage7.Controls.Add(this.cbGPIO0);
			this.tabPage7.Location = new System.Drawing.Point(4, 22);
			this.tabPage7.Name = "tabPage7";
			this.tabPage7.Size = new System.Drawing.Size(695, 393);
			this.tabPage7.TabIndex = 6;
			this.tabPage7.Text = "CPLD_GPIO";
			this.tabPage7.UseVisualStyleBackColor = true;
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(137, 322);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(118, 20);
			this.label25.TabIndex = 64;
			this.label25.Text = "1=in, 0=out";
			// 
			// cbGPIO3DDR
			// 
			this.cbGPIO3DDR.Location = new System.Drawing.Point(17, 318);
			this.cbGPIO3DDR.Name = "cbGPIO3DDR";
			this.cbGPIO3DDR.Size = new System.Drawing.Size(104, 24);
			this.cbGPIO3DDR.TabIndex = 63;
			this.cbGPIO3DDR.Text = "GPIO3 DDR";
			this.cbGPIO3DDR.UseVisualStyleBackColor = true;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(137, 292);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(118, 20);
			this.label18.TabIndex = 62;
			this.label18.Text = "1=in, 0=out";
			// 
			// cbGPIO2DDR
			// 
			this.cbGPIO2DDR.Location = new System.Drawing.Point(17, 288);
			this.cbGPIO2DDR.Name = "cbGPIO2DDR";
			this.cbGPIO2DDR.Size = new System.Drawing.Size(104, 24);
			this.cbGPIO2DDR.TabIndex = 61;
			this.cbGPIO2DDR.Text = "GPIO2 DDR";
			this.cbGPIO2DDR.UseVisualStyleBackColor = true;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(137, 262);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(118, 20);
			this.label19.TabIndex = 60;
			this.label19.Text = "1=in, 0=out";
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(137, 233);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(118, 20);
			this.label20.TabIndex = 59;
			this.label20.Text = "1=in, 0=out";
			// 
			// cbGPIO1DDR
			// 
			this.cbGPIO1DDR.Location = new System.Drawing.Point(17, 258);
			this.cbGPIO1DDR.Name = "cbGPIO1DDR";
			this.cbGPIO1DDR.Size = new System.Drawing.Size(104, 24);
			this.cbGPIO1DDR.TabIndex = 58;
			this.cbGPIO1DDR.Text = "GPIO1 DDR";
			this.cbGPIO1DDR.UseVisualStyleBackColor = true;
			// 
			// cbGPIO0DDR
			// 
			this.cbGPIO0DDR.Location = new System.Drawing.Point(17, 228);
			this.cbGPIO0DDR.Name = "cbGPIO0DDR";
			this.cbGPIO0DDR.Size = new System.Drawing.Size(104, 24);
			this.cbGPIO0DDR.TabIndex = 57;
			this.cbGPIO0DDR.Text = "GPIO0 DDR";
			this.cbGPIO0DDR.UseVisualStyleBackColor = true;
			// 
			// btnReadGPIO
			// 
			this.btnReadGPIO.Location = new System.Drawing.Point(30, 57);
			this.btnReadGPIO.Name = "btnReadGPIO";
			this.btnReadGPIO.Size = new System.Drawing.Size(75, 23);
			this.btnReadGPIO.TabIndex = 52;
			this.btnReadGPIO.Text = "Read";
			this.btnReadGPIO.UseVisualStyleBackColor = true;
			this.btnReadGPIO.Click += new System.EventHandler(this.BtnReadGPIOClick);
			// 
			// btnWriteGPIO
			// 
			this.btnWriteGPIO.Location = new System.Drawing.Point(30, 19);
			this.btnWriteGPIO.Name = "btnWriteGPIO";
			this.btnWriteGPIO.Size = new System.Drawing.Size(75, 23);
			this.btnWriteGPIO.TabIndex = 51;
			this.btnWriteGPIO.Text = "Write";
			this.btnWriteGPIO.UseVisualStyleBackColor = true;
			this.btnWriteGPIO.Click += new System.EventHandler(this.BtnWriteGPIOClick);
			// 
			// cbGPIO3
			// 
			this.cbGPIO3.Location = new System.Drawing.Point(17, 200);
			this.cbGPIO3.Name = "cbGPIO3";
			this.cbGPIO3.Size = new System.Drawing.Size(104, 24);
			this.cbGPIO3.TabIndex = 50;
			this.cbGPIO3.Text = "GPIO3";
			this.cbGPIO3.UseVisualStyleBackColor = true;
			// 
			// cbGPIO2
			// 
			this.cbGPIO2.Location = new System.Drawing.Point(17, 170);
			this.cbGPIO2.Name = "cbGPIO2";
			this.cbGPIO2.Size = new System.Drawing.Size(114, 24);
			this.cbGPIO2.TabIndex = 49;
			this.cbGPIO2.Text = "GPIO2";
			this.cbGPIO2.UseVisualStyleBackColor = true;
			// 
			// cbGPIO1
			// 
			this.cbGPIO1.Location = new System.Drawing.Point(17, 140);
			this.cbGPIO1.Name = "cbGPIO1";
			this.cbGPIO1.Size = new System.Drawing.Size(104, 24);
			this.cbGPIO1.TabIndex = 48;
			this.cbGPIO1.Text = "GPIO1";
			this.cbGPIO1.UseVisualStyleBackColor = true;
			// 
			// cbGPIO0
			// 
			this.cbGPIO0.Location = new System.Drawing.Point(17, 110);
			this.cbGPIO0.Name = "cbGPIO0";
			this.cbGPIO0.Size = new System.Drawing.Size(114, 24);
			this.cbGPIO0.TabIndex = 47;
			this.cbGPIO0.Text = "GPIO0";
			this.cbGPIO0.UseVisualStyleBackColor = true;
			// 
			// tabPage8
			// 
			this.tabPage8.Controls.Add(this.btnClear);
			this.tabPage8.Controls.Add(this.btnReadCoef);
			this.tabPage8.Controls.Add(this.btnLoadCoef);
			this.tabPage8.Controls.Add(this.label36);
			this.tabPage8.Controls.Add(this.nudDDC);
			this.tabPage8.Controls.Add(this.txtCLRRSVR);
			this.tabPage8.Controls.Add(this.txtRTAP);
			this.tabPage8.Controls.Add(this.txtRAO);
			this.tabPage8.Controls.Add(this.txtRDEC);
			this.tabPage8.Controls.Add(this.txtRCR);
			this.tabPage8.Controls.Add(this.txtC5DEC);
			this.tabPage8.Controls.Add(this.txtC5SR);
			this.tabPage8.Controls.Add(this.txtC2DEC);
			this.tabPage8.Controls.Add(this.txtC2SR);
			this.tabPage8.Controls.Add(this.txtNPO);
			this.tabPage8.Controls.Add(this.txtNFRE);
			this.tabPage8.Controls.Add(this.txtNSCR);
			this.tabPage8.Controls.Add(this.cbNCRADith);
			this.tabPage8.Controls.Add(this.cbNCRDITH);
			this.tabPage8.Controls.Add(this.cbNCOBypass);
			this.tabPage8.Controls.Add(this.cbMCRMS);
			this.tabPage8.Controls.Add(this.cbMCRComplx);
			this.tabPage8.Controls.Add(this.cbMCRDCh);
			this.tabPage8.Controls.Add(this.cbMCRRST);
			this.tabPage8.Controls.Add(this.btnWriteRTAP);
			this.tabPage8.Controls.Add(this.btnWriteRAO);
			this.tabPage8.Controls.Add(this.btnWriteRDEC);
			this.tabPage8.Controls.Add(this.btnWriteRCR);
			this.tabPage8.Controls.Add(this.btnWriteC5DEC);
			this.tabPage8.Controls.Add(this.btnWriteC5SR);
			this.tabPage8.Controls.Add(this.btnWriteC2DEC);
			this.tabPage8.Controls.Add(this.btnWriteC2SR);
			this.tabPage8.Controls.Add(this.btnWriteNPO);
			this.tabPage8.Controls.Add(this.btnWriteNFRE);
			this.tabPage8.Controls.Add(this.btnWriteNSCR);
			this.tabPage8.Controls.Add(this.btnWriteNCR);
			this.tabPage8.Controls.Add(this.btnWriteMCR);
			this.tabPage8.Controls.Add(this.btnCLRCOEF);
			this.tabPage8.Controls.Add(this.btnClrRam);
			this.tabPage8.Controls.Add(this.btnReadRTAP);
			this.tabPage8.Controls.Add(this.btnReadRAO);
			this.tabPage8.Controls.Add(this.btnReadRDEC);
			this.tabPage8.Controls.Add(this.btnReadRCR);
			this.tabPage8.Controls.Add(this.btnReadC5DEC);
			this.tabPage8.Controls.Add(this.btnReadC5SR);
			this.tabPage8.Controls.Add(this.btnReadC2DEC);
			this.tabPage8.Controls.Add(this.btnReadC2SR);
			this.tabPage8.Controls.Add(this.btnReadNPO);
			this.tabPage8.Controls.Add(this.btnReadNFRE);
			this.tabPage8.Controls.Add(this.btnReadNSCR);
			this.tabPage8.Controls.Add(this.btnReadNCR);
			this.tabPage8.Controls.Add(this.btnMCRRead);
			this.tabPage8.Controls.Add(this.listBox2);
			this.tabPage8.Location = new System.Drawing.Point(4, 22);
			this.tabPage8.Name = "tabPage8";
			this.tabPage8.Size = new System.Drawing.Size(695, 393);
			this.tabPage8.TabIndex = 7;
			this.tabPage8.Text = "DDC REGS";
			this.tabPage8.UseVisualStyleBackColor = true;
			// 
			// btnReadCoef
			// 
			this.btnReadCoef.Location = new System.Drawing.Point(117, 129);
			this.btnReadCoef.Name = "btnReadCoef";
			this.btnReadCoef.Size = new System.Drawing.Size(88, 23);
			this.btnReadCoef.TabIndex = 126;
			this.btnReadCoef.Text = "READ COEF";
			this.btnReadCoef.UseVisualStyleBackColor = true;
			this.btnReadCoef.Click += new System.EventHandler(this.BtnReadCoefClick);
			// 
			// btnLoadCoef
			// 
			this.btnLoadCoef.Location = new System.Drawing.Point(117, 100);
			this.btnLoadCoef.Name = "btnLoadCoef";
			this.btnLoadCoef.Size = new System.Drawing.Size(88, 23);
			this.btnLoadCoef.TabIndex = 125;
			this.btnLoadCoef.Text = "LOAD COEF";
			this.btnLoadCoef.UseVisualStyleBackColor = true;
			this.btnLoadCoef.Click += new System.EventHandler(this.BtnLoadCoefClick);
			// 
			// label36
			// 
			this.label36.Location = new System.Drawing.Point(3, 15);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(36, 18);
			this.label36.TabIndex = 124;
			this.label36.Text = "DDC";
			// 
			// nudDDC
			// 
			this.nudDDC.Location = new System.Drawing.Point(45, 13);
			this.nudDDC.Maximum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.nudDDC.Name = "nudDDC";
			this.nudDDC.Size = new System.Drawing.Size(57, 20);
			this.nudDDC.TabIndex = 123;
			// 
			// txtCLRRSVR
			// 
			this.txtCLRRSVR.Location = new System.Drawing.Point(117, 71);
			this.txtCLRRSVR.Name = "txtCLRRSVR";
			this.txtCLRRSVR.Size = new System.Drawing.Size(88, 23);
			this.txtCLRRSVR.TabIndex = 122;
			this.txtCLRRSVR.Text = "CLR RSRV";
			this.txtCLRRSVR.UseVisualStyleBackColor = true;
			this.txtCLRRSVR.Click += new System.EventHandler(this.TxtCLRRSVRClick);
			// 
			// txtRTAP
			// 
			this.txtRTAP.Location = new System.Drawing.Point(412, 362);
			this.txtRTAP.Name = "txtRTAP";
			this.txtRTAP.Size = new System.Drawing.Size(112, 20);
			this.txtRTAP.TabIndex = 121;
			this.txtRTAP.Text = "0";
			// 
			// txtRAO
			// 
			this.txtRAO.Location = new System.Drawing.Point(412, 331);
			this.txtRAO.Name = "txtRAO";
			this.txtRAO.Size = new System.Drawing.Size(112, 20);
			this.txtRAO.TabIndex = 120;
			this.txtRAO.Text = "0";
			// 
			// txtRDEC
			// 
			this.txtRDEC.Location = new System.Drawing.Point(412, 305);
			this.txtRDEC.Name = "txtRDEC";
			this.txtRDEC.Size = new System.Drawing.Size(112, 20);
			this.txtRDEC.TabIndex = 119;
			this.txtRDEC.Text = "0";
			// 
			// txtRCR
			// 
			this.txtRCR.Location = new System.Drawing.Point(412, 277);
			this.txtRCR.Name = "txtRCR";
			this.txtRCR.Size = new System.Drawing.Size(112, 20);
			this.txtRCR.TabIndex = 118;
			this.txtRCR.Text = "0";
			// 
			// txtC5DEC
			// 
			this.txtC5DEC.Location = new System.Drawing.Point(412, 247);
			this.txtC5DEC.Name = "txtC5DEC";
			this.txtC5DEC.Size = new System.Drawing.Size(112, 20);
			this.txtC5DEC.TabIndex = 117;
			this.txtC5DEC.Text = "0";
			// 
			// txtC5SR
			// 
			this.txtC5SR.Location = new System.Drawing.Point(412, 218);
			this.txtC5SR.Name = "txtC5SR";
			this.txtC5SR.Size = new System.Drawing.Size(112, 20);
			this.txtC5SR.TabIndex = 116;
			this.txtC5SR.Text = "0";
			// 
			// txtC2DEC
			// 
			this.txtC2DEC.Location = new System.Drawing.Point(412, 190);
			this.txtC2DEC.Name = "txtC2DEC";
			this.txtC2DEC.Size = new System.Drawing.Size(112, 20);
			this.txtC2DEC.TabIndex = 115;
			this.txtC2DEC.Text = "0";
			// 
			// txtC2SR
			// 
			this.txtC2SR.Location = new System.Drawing.Point(412, 160);
			this.txtC2SR.Name = "txtC2SR";
			this.txtC2SR.Size = new System.Drawing.Size(112, 20);
			this.txtC2SR.TabIndex = 114;
			this.txtC2SR.Text = "0";
			// 
			// txtNPO
			// 
			this.txtNPO.Location = new System.Drawing.Point(412, 131);
			this.txtNPO.Name = "txtNPO";
			this.txtNPO.Size = new System.Drawing.Size(112, 20);
			this.txtNPO.TabIndex = 113;
			this.txtNPO.Text = "0";
			// 
			// txtNFRE
			// 
			this.txtNFRE.Location = new System.Drawing.Point(412, 100);
			this.txtNFRE.Name = "txtNFRE";
			this.txtNFRE.Size = new System.Drawing.Size(112, 20);
			this.txtNFRE.TabIndex = 112;
			this.txtNFRE.Text = "0";
			// 
			// txtNSCR
			// 
			this.txtNSCR.Location = new System.Drawing.Point(412, 71);
			this.txtNSCR.Name = "txtNSCR";
			this.txtNSCR.Size = new System.Drawing.Size(112, 20);
			this.txtNSCR.TabIndex = 111;
			this.txtNSCR.Text = "0";
			// 
			// cbNCRADith
			// 
			this.cbNCRADith.Location = new System.Drawing.Point(530, 41);
			this.cbNCRADith.Name = "cbNCRADith";
			this.cbNCRADith.Size = new System.Drawing.Size(60, 24);
			this.cbNCRADith.TabIndex = 110;
			this.cbNCRADith.Text = "ADith";
			this.cbNCRADith.UseVisualStyleBackColor = true;
			// 
			// cbNCRDITH
			// 
			this.cbNCRDITH.Location = new System.Drawing.Point(473, 41);
			this.cbNCRDITH.Name = "cbNCRDITH";
			this.cbNCRDITH.Size = new System.Drawing.Size(51, 24);
			this.cbNCRDITH.TabIndex = 109;
			this.cbNCRDITH.Text = "Dith";
			this.cbNCRDITH.UseVisualStyleBackColor = true;
			// 
			// cbNCOBypass
			// 
			this.cbNCOBypass.Location = new System.Drawing.Point(412, 41);
			this.cbNCOBypass.Name = "cbNCOBypass";
			this.cbNCOBypass.Size = new System.Drawing.Size(76, 24);
			this.cbNCOBypass.TabIndex = 108;
			this.cbNCOBypass.Text = "Bypass";
			this.cbNCOBypass.UseVisualStyleBackColor = true;
			// 
			// cbMCRMS
			// 
			this.cbMCRMS.Location = new System.Drawing.Point(583, 12);
			this.cbMCRMS.Name = "cbMCRMS";
			this.cbMCRMS.Size = new System.Drawing.Size(51, 24);
			this.cbMCRMS.TabIndex = 107;
			this.cbMCRMS.Text = "M/S";
			this.cbMCRMS.UseVisualStyleBackColor = true;
			// 
			// cbMCRComplx
			// 
			this.cbMCRComplx.Location = new System.Drawing.Point(530, 12);
			this.cbMCRComplx.Name = "cbMCRComplx";
			this.cbMCRComplx.Size = new System.Drawing.Size(60, 24);
			this.cbMCRComplx.TabIndex = 106;
			this.cbMCRComplx.Text = "Cmplx";
			this.cbMCRComplx.UseVisualStyleBackColor = true;
			// 
			// cbMCRDCh
			// 
			this.cbMCRDCh.Location = new System.Drawing.Point(473, 11);
			this.cbMCRDCh.Name = "cbMCRDCh";
			this.cbMCRDCh.Size = new System.Drawing.Size(51, 24);
			this.cbMCRDCh.TabIndex = 105;
			this.cbMCRDCh.Text = "DCh";
			this.cbMCRDCh.UseVisualStyleBackColor = true;
			// 
			// cbMCRRST
			// 
			this.cbMCRRST.Location = new System.Drawing.Point(412, 12);
			this.cbMCRRST.Name = "cbMCRRST";
			this.cbMCRRST.Size = new System.Drawing.Size(76, 24);
			this.cbMCRRST.TabIndex = 104;
			this.cbMCRRST.Text = "SRst";
			this.cbMCRRST.UseVisualStyleBackColor = true;
			// 
			// btnWriteRTAP
			// 
			this.btnWriteRTAP.Location = new System.Drawing.Point(305, 359);
			this.btnWriteRTAP.Name = "btnWriteRTAP";
			this.btnWriteRTAP.Size = new System.Drawing.Size(88, 23);
			this.btnWriteRTAP.TabIndex = 103;
			this.btnWriteRTAP.Text = "Write RTAP";
			this.btnWriteRTAP.UseVisualStyleBackColor = true;
			this.btnWriteRTAP.Click += new System.EventHandler(this.BtnWriteRTAPClick);
			// 
			// btnWriteRAO
			// 
			this.btnWriteRAO.Location = new System.Drawing.Point(305, 331);
			this.btnWriteRAO.Name = "btnWriteRAO";
			this.btnWriteRAO.Size = new System.Drawing.Size(88, 23);
			this.btnWriteRAO.TabIndex = 102;
			this.btnWriteRAO.Text = "Write RAO";
			this.btnWriteRAO.UseVisualStyleBackColor = true;
			this.btnWriteRAO.Click += new System.EventHandler(this.BtnWriteRAOClick);
			// 
			// btnWriteRDEC
			// 
			this.btnWriteRDEC.Location = new System.Drawing.Point(305, 303);
			this.btnWriteRDEC.Name = "btnWriteRDEC";
			this.btnWriteRDEC.Size = new System.Drawing.Size(88, 23);
			this.btnWriteRDEC.TabIndex = 101;
			this.btnWriteRDEC.Text = "Write RDEC";
			this.btnWriteRDEC.UseVisualStyleBackColor = true;
			this.btnWriteRDEC.Click += new System.EventHandler(this.BtnWriteRDECClick);
			// 
			// btnWriteRCR
			// 
			this.btnWriteRCR.Location = new System.Drawing.Point(305, 274);
			this.btnWriteRCR.Name = "btnWriteRCR";
			this.btnWriteRCR.Size = new System.Drawing.Size(88, 23);
			this.btnWriteRCR.TabIndex = 100;
			this.btnWriteRCR.Text = "Write RCR";
			this.btnWriteRCR.UseVisualStyleBackColor = true;
			this.btnWriteRCR.Click += new System.EventHandler(this.BtnWriteRCRClick);
			// 
			// btnWriteC5DEC
			// 
			this.btnWriteC5DEC.Location = new System.Drawing.Point(305, 245);
			this.btnWriteC5DEC.Name = "btnWriteC5DEC";
			this.btnWriteC5DEC.Size = new System.Drawing.Size(88, 23);
			this.btnWriteC5DEC.TabIndex = 99;
			this.btnWriteC5DEC.Text = "Write C5DEC";
			this.btnWriteC5DEC.UseVisualStyleBackColor = true;
			this.btnWriteC5DEC.Click += new System.EventHandler(this.BtnWriteC5DECClick);
			// 
			// btnWriteC5SR
			// 
			this.btnWriteC5SR.Location = new System.Drawing.Point(305, 216);
			this.btnWriteC5SR.Name = "btnWriteC5SR";
			this.btnWriteC5SR.Size = new System.Drawing.Size(88, 23);
			this.btnWriteC5SR.TabIndex = 98;
			this.btnWriteC5SR.Text = "Write C5SR";
			this.btnWriteC5SR.UseVisualStyleBackColor = true;
			this.btnWriteC5SR.Click += new System.EventHandler(this.BtnWriteC5SRClick);
			// 
			// btnWriteC2DEC
			// 
			this.btnWriteC2DEC.Location = new System.Drawing.Point(305, 187);
			this.btnWriteC2DEC.Name = "btnWriteC2DEC";
			this.btnWriteC2DEC.Size = new System.Drawing.Size(88, 23);
			this.btnWriteC2DEC.TabIndex = 97;
			this.btnWriteC2DEC.Text = "Write C2DEC";
			this.btnWriteC2DEC.UseVisualStyleBackColor = true;
			this.btnWriteC2DEC.Click += new System.EventHandler(this.BtnWriteC2DECClick);
			// 
			// btnWriteC2SR
			// 
			this.btnWriteC2SR.Location = new System.Drawing.Point(305, 158);
			this.btnWriteC2SR.Name = "btnWriteC2SR";
			this.btnWriteC2SR.Size = new System.Drawing.Size(88, 23);
			this.btnWriteC2SR.TabIndex = 96;
			this.btnWriteC2SR.Text = "Write C2SR";
			this.btnWriteC2SR.UseVisualStyleBackColor = true;
			this.btnWriteC2SR.Click += new System.EventHandler(this.BtnWriteC2SRClick);
			// 
			// btnWriteNPO
			// 
			this.btnWriteNPO.Location = new System.Drawing.Point(305, 129);
			this.btnWriteNPO.Name = "btnWriteNPO";
			this.btnWriteNPO.Size = new System.Drawing.Size(88, 23);
			this.btnWriteNPO.TabIndex = 95;
			this.btnWriteNPO.Text = "Write NPO";
			this.btnWriteNPO.UseVisualStyleBackColor = true;
			this.btnWriteNPO.Click += new System.EventHandler(this.BtnWriteNPOClick);
			// 
			// btnWriteNFRE
			// 
			this.btnWriteNFRE.Location = new System.Drawing.Point(305, 100);
			this.btnWriteNFRE.Name = "btnWriteNFRE";
			this.btnWriteNFRE.Size = new System.Drawing.Size(88, 23);
			this.btnWriteNFRE.TabIndex = 94;
			this.btnWriteNFRE.Text = "Write NFRE";
			this.btnWriteNFRE.UseVisualStyleBackColor = true;
			this.btnWriteNFRE.Click += new System.EventHandler(this.BtnWriteNFREClick);
			// 
			// btnWriteNSCR
			// 
			this.btnWriteNSCR.Location = new System.Drawing.Point(305, 71);
			this.btnWriteNSCR.Name = "btnWriteNSCR";
			this.btnWriteNSCR.Size = new System.Drawing.Size(88, 23);
			this.btnWriteNSCR.TabIndex = 93;
			this.btnWriteNSCR.Text = "Write NSCR";
			this.btnWriteNSCR.UseVisualStyleBackColor = true;
			this.btnWriteNSCR.Click += new System.EventHandler(this.BtnWriteNSCRClick);
			// 
			// btnWriteNCR
			// 
			this.btnWriteNCR.Location = new System.Drawing.Point(305, 42);
			this.btnWriteNCR.Name = "btnWriteNCR";
			this.btnWriteNCR.Size = new System.Drawing.Size(88, 23);
			this.btnWriteNCR.TabIndex = 92;
			this.btnWriteNCR.Text = "Write NCR";
			this.btnWriteNCR.UseVisualStyleBackColor = true;
			this.btnWriteNCR.Click += new System.EventHandler(this.BtnWriteNCRClick);
			// 
			// btnWriteMCR
			// 
			this.btnWriteMCR.Location = new System.Drawing.Point(305, 13);
			this.btnWriteMCR.Name = "btnWriteMCR";
			this.btnWriteMCR.Size = new System.Drawing.Size(88, 23);
			this.btnWriteMCR.TabIndex = 90;
			this.btnWriteMCR.Text = "Write MCR";
			this.btnWriteMCR.UseVisualStyleBackColor = true;
			this.btnWriteMCR.Click += new System.EventHandler(this.btnWriteMCRClick);
			// 
			// btnCLRCOEF
			// 
			this.btnCLRCOEF.Location = new System.Drawing.Point(117, 42);
			this.btnCLRCOEF.Name = "btnCLRCOEF";
			this.btnCLRCOEF.Size = new System.Drawing.Size(88, 23);
			this.btnCLRCOEF.TabIndex = 89;
			this.btnCLRCOEF.Text = "CLR COEF";
			this.btnCLRCOEF.UseVisualStyleBackColor = true;
			this.btnCLRCOEF.Click += new System.EventHandler(this.BtnCLRCOEFClick);
			// 
			// btnClrRam
			// 
			this.btnClrRam.Location = new System.Drawing.Point(117, 13);
			this.btnClrRam.Name = "btnClrRam";
			this.btnClrRam.Size = new System.Drawing.Size(88, 23);
			this.btnClrRam.TabIndex = 88;
			this.btnClrRam.Text = "CLR RAM";
			this.btnClrRam.UseVisualStyleBackColor = true;
			this.btnClrRam.Click += new System.EventHandler(this.BtnClrRamClick);
			// 
			// btnReadRTAP
			// 
			this.btnReadRTAP.Location = new System.Drawing.Point(211, 359);
			this.btnReadRTAP.Name = "btnReadRTAP";
			this.btnReadRTAP.Size = new System.Drawing.Size(88, 23);
			this.btnReadRTAP.TabIndex = 87;
			this.btnReadRTAP.Text = "Read RTAP";
			this.btnReadRTAP.UseVisualStyleBackColor = true;
			this.btnReadRTAP.Click += new System.EventHandler(this.BtnReadRTAPClick);
			// 
			// btnReadRAO
			// 
			this.btnReadRAO.Location = new System.Drawing.Point(211, 331);
			this.btnReadRAO.Name = "btnReadRAO";
			this.btnReadRAO.Size = new System.Drawing.Size(88, 23);
			this.btnReadRAO.TabIndex = 86;
			this.btnReadRAO.Text = "Read RAO";
			this.btnReadRAO.UseVisualStyleBackColor = true;
			this.btnReadRAO.Click += new System.EventHandler(this.BtnReadRAOClick);
			// 
			// btnReadRDEC
			// 
			this.btnReadRDEC.Location = new System.Drawing.Point(211, 303);
			this.btnReadRDEC.Name = "btnReadRDEC";
			this.btnReadRDEC.Size = new System.Drawing.Size(88, 23);
			this.btnReadRDEC.TabIndex = 85;
			this.btnReadRDEC.Text = "Read RDEC";
			this.btnReadRDEC.UseVisualStyleBackColor = true;
			this.btnReadRDEC.Click += new System.EventHandler(this.BtnReadRDECClick);
			// 
			// btnReadRCR
			// 
			this.btnReadRCR.Location = new System.Drawing.Point(211, 274);
			this.btnReadRCR.Name = "btnReadRCR";
			this.btnReadRCR.Size = new System.Drawing.Size(88, 23);
			this.btnReadRCR.TabIndex = 84;
			this.btnReadRCR.Text = "Read RCR";
			this.btnReadRCR.UseVisualStyleBackColor = true;
			this.btnReadRCR.Click += new System.EventHandler(this.BtnReadRCRClick);
			// 
			// btnReadC5DEC
			// 
			this.btnReadC5DEC.Location = new System.Drawing.Point(211, 245);
			this.btnReadC5DEC.Name = "btnReadC5DEC";
			this.btnReadC5DEC.Size = new System.Drawing.Size(88, 23);
			this.btnReadC5DEC.TabIndex = 83;
			this.btnReadC5DEC.Text = "Read C5DEC";
			this.btnReadC5DEC.UseVisualStyleBackColor = true;
			this.btnReadC5DEC.Click += new System.EventHandler(this.BtnReadC5DECClick);
			// 
			// btnReadC5SR
			// 
			this.btnReadC5SR.Location = new System.Drawing.Point(211, 216);
			this.btnReadC5SR.Name = "btnReadC5SR";
			this.btnReadC5SR.Size = new System.Drawing.Size(88, 23);
			this.btnReadC5SR.TabIndex = 82;
			this.btnReadC5SR.Text = "Read C5SR";
			this.btnReadC5SR.UseVisualStyleBackColor = true;
			this.btnReadC5SR.Click += new System.EventHandler(this.BtnReadC5SRClick);
			// 
			// btnReadC2DEC
			// 
			this.btnReadC2DEC.Location = new System.Drawing.Point(211, 187);
			this.btnReadC2DEC.Name = "btnReadC2DEC";
			this.btnReadC2DEC.Size = new System.Drawing.Size(88, 23);
			this.btnReadC2DEC.TabIndex = 81;
			this.btnReadC2DEC.Text = "Read C2DEC";
			this.btnReadC2DEC.UseVisualStyleBackColor = true;
			this.btnReadC2DEC.Click += new System.EventHandler(this.BtnReadC2DECClick);
			// 
			// btnReadC2SR
			// 
			this.btnReadC2SR.Location = new System.Drawing.Point(211, 158);
			this.btnReadC2SR.Name = "btnReadC2SR";
			this.btnReadC2SR.Size = new System.Drawing.Size(88, 23);
			this.btnReadC2SR.TabIndex = 80;
			this.btnReadC2SR.Text = "Read C2SR";
			this.btnReadC2SR.UseVisualStyleBackColor = true;
			this.btnReadC2SR.Click += new System.EventHandler(this.BtnReadC2SRClick);
			// 
			// btnReadNPO
			// 
			this.btnReadNPO.Location = new System.Drawing.Point(211, 129);
			this.btnReadNPO.Name = "btnReadNPO";
			this.btnReadNPO.Size = new System.Drawing.Size(88, 23);
			this.btnReadNPO.TabIndex = 79;
			this.btnReadNPO.Text = "Read NPO";
			this.btnReadNPO.UseVisualStyleBackColor = true;
			this.btnReadNPO.Click += new System.EventHandler(this.BtnReadNPOClick);
			// 
			// btnReadNFRE
			// 
			this.btnReadNFRE.Location = new System.Drawing.Point(211, 100);
			this.btnReadNFRE.Name = "btnReadNFRE";
			this.btnReadNFRE.Size = new System.Drawing.Size(88, 23);
			this.btnReadNFRE.TabIndex = 78;
			this.btnReadNFRE.Text = "Read NFRE";
			this.btnReadNFRE.UseVisualStyleBackColor = true;
			this.btnReadNFRE.Click += new System.EventHandler(this.BtnReadNFREClick);
			// 
			// btnReadNSCR
			// 
			this.btnReadNSCR.Location = new System.Drawing.Point(211, 71);
			this.btnReadNSCR.Name = "btnReadNSCR";
			this.btnReadNSCR.Size = new System.Drawing.Size(88, 23);
			this.btnReadNSCR.TabIndex = 77;
			this.btnReadNSCR.Text = "Read NSCR";
			this.btnReadNSCR.UseVisualStyleBackColor = true;
			this.btnReadNSCR.Click += new System.EventHandler(this.BtnReadNSCRClick);
			// 
			// btnReadNCR
			// 
			this.btnReadNCR.Location = new System.Drawing.Point(211, 42);
			this.btnReadNCR.Name = "btnReadNCR";
			this.btnReadNCR.Size = new System.Drawing.Size(88, 23);
			this.btnReadNCR.TabIndex = 76;
			this.btnReadNCR.Text = "Read NCR";
			this.btnReadNCR.UseVisualStyleBackColor = true;
			this.btnReadNCR.Click += new System.EventHandler(this.BtnReadNCRClick);
			// 
			// btnMCRRead
			// 
			this.btnMCRRead.Location = new System.Drawing.Point(211, 13);
			this.btnMCRRead.Name = "btnMCRRead";
			this.btnMCRRead.Size = new System.Drawing.Size(88, 23);
			this.btnMCRRead.TabIndex = 75;
			this.btnMCRRead.Text = "Read MCR";
			this.btnMCRRead.UseVisualStyleBackColor = true;
			this.btnMCRRead.Click += new System.EventHandler(this.BtnMCRReadClick);
			// 
			// listBox2
			// 
			this.listBox2.FormattingEnabled = true;
			this.listBox2.Location = new System.Drawing.Point(530, 71);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(162, 303);
			this.listBox2.TabIndex = 74;
			// 
			// tabPage9
			// 
			this.tabPage9.Controls.Add(this.txtDDC1AMR);
			this.tabPage9.Controls.Add(this.txtDDC1LAR);
			this.tabPage9.Controls.Add(this.txtDDC1DR4);
			this.tabPage9.Controls.Add(this.txtDDC1DR3);
			this.tabPage9.Controls.Add(this.txtDDC1DR2);
			this.tabPage9.Controls.Add(this.txtDDC1DR1);
			this.tabPage9.Controls.Add(this.txtDDC1DR0);
			this.tabPage9.Controls.Add(this.label29);
			this.tabPage9.Controls.Add(this.label30);
			this.tabPage9.Controls.Add(this.label31);
			this.tabPage9.Controls.Add(this.label32);
			this.tabPage9.Controls.Add(this.label33);
			this.tabPage9.Controls.Add(this.label34);
			this.tabPage9.Controls.Add(this.label35);
			this.tabPage9.Controls.Add(this.btnReadDDC1);
			this.tabPage9.Controls.Add(this.btnWriteDDC1);
			this.tabPage9.Location = new System.Drawing.Point(4, 22);
			this.tabPage9.Name = "tabPage9";
			this.tabPage9.Size = new System.Drawing.Size(695, 393);
			this.tabPage9.TabIndex = 8;
			this.tabPage9.Text = "DDC DRx";
			this.tabPage9.UseVisualStyleBackColor = true;
			// 
			// txtDDC1AMR
			// 
			this.txtDDC1AMR.Location = new System.Drawing.Point(83, 293);
			this.txtDDC1AMR.MaxLength = 2;
			this.txtDDC1AMR.Name = "txtDDC1AMR";
			this.txtDDC1AMR.Size = new System.Drawing.Size(100, 20);
			this.txtDDC1AMR.TabIndex = 89;
			this.txtDDC1AMR.Text = "03";
			this.txtDDC1AMR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// txtDDC1LAR
			// 
			this.txtDDC1LAR.Location = new System.Drawing.Point(83, 262);
			this.txtDDC1LAR.MaxLength = 2;
			this.txtDDC1LAR.Name = "txtDDC1LAR";
			this.txtDDC1LAR.Size = new System.Drawing.Size(100, 20);
			this.txtDDC1LAR.TabIndex = 88;
			this.txtDDC1LAR.Text = "00";
			this.txtDDC1LAR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// txtDDC1DR4
			// 
			this.txtDDC1DR4.Location = new System.Drawing.Point(83, 231);
			this.txtDDC1DR4.MaxLength = 2;
			this.txtDDC1DR4.Name = "txtDDC1DR4";
			this.txtDDC1DR4.Size = new System.Drawing.Size(100, 20);
			this.txtDDC1DR4.TabIndex = 87;
			this.txtDDC1DR4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// txtDDC1DR3
			// 
			this.txtDDC1DR3.Location = new System.Drawing.Point(83, 202);
			this.txtDDC1DR3.MaxLength = 2;
			this.txtDDC1DR3.Name = "txtDDC1DR3";
			this.txtDDC1DR3.Size = new System.Drawing.Size(100, 20);
			this.txtDDC1DR3.TabIndex = 86;
			this.txtDDC1DR3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// txtDDC1DR2
			// 
			this.txtDDC1DR2.Location = new System.Drawing.Point(83, 173);
			this.txtDDC1DR2.MaxLength = 2;
			this.txtDDC1DR2.Name = "txtDDC1DR2";
			this.txtDDC1DR2.Size = new System.Drawing.Size(100, 20);
			this.txtDDC1DR2.TabIndex = 85;
			this.txtDDC1DR2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// txtDDC1DR1
			// 
			this.txtDDC1DR1.Location = new System.Drawing.Point(83, 141);
			this.txtDDC1DR1.MaxLength = 2;
			this.txtDDC1DR1.Name = "txtDDC1DR1";
			this.txtDDC1DR1.Size = new System.Drawing.Size(100, 20);
			this.txtDDC1DR1.TabIndex = 84;
			this.txtDDC1DR1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// txtDDC1DR0
			// 
			this.txtDDC1DR0.Location = new System.Drawing.Point(83, 112);
			this.txtDDC1DR0.MaxLength = 2;
			this.txtDDC1DR0.Name = "txtDDC1DR0";
			this.txtDDC1DR0.Size = new System.Drawing.Size(100, 20);
			this.txtDDC1DR0.TabIndex = 83;
			this.txtDDC1DR0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(27, 293);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(36, 20);
			this.label29.TabIndex = 82;
			this.label29.Text = "AMR:";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(27, 262);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(36, 20);
			this.label30.TabIndex = 81;
			this.label30.Text = "LAR:";
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(27, 231);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(36, 20);
			this.label31.TabIndex = 80;
			this.label31.Text = "DR4:";
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(27, 202);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(36, 20);
			this.label32.TabIndex = 79;
			this.label32.Text = "DR3:";
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(27, 173);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(36, 20);
			this.label33.TabIndex = 78;
			this.label33.Text = "DR2:";
			// 
			// label34
			// 
			this.label34.Location = new System.Drawing.Point(27, 144);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(36, 20);
			this.label34.TabIndex = 77;
			this.label34.Text = "DR1:";
			// 
			// label35
			// 
			this.label35.Location = new System.Drawing.Point(27, 115);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(36, 20);
			this.label35.TabIndex = 76;
			this.label35.Text = "DR0:";
			// 
			// btnReadDDC1
			// 
			this.btnReadDDC1.Location = new System.Drawing.Point(27, 66);
			this.btnReadDDC1.Name = "btnReadDDC1";
			this.btnReadDDC1.Size = new System.Drawing.Size(75, 23);
			this.btnReadDDC1.TabIndex = 75;
			this.btnReadDDC1.Text = "Read";
			this.btnReadDDC1.UseVisualStyleBackColor = true;
			this.btnReadDDC1.Click += new System.EventHandler(this.BtnReadDDC1Click);
			// 
			// btnWriteDDC1
			// 
			this.btnWriteDDC1.Location = new System.Drawing.Point(27, 28);
			this.btnWriteDDC1.Name = "btnWriteDDC1";
			this.btnWriteDDC1.Size = new System.Drawing.Size(75, 23);
			this.btnWriteDDC1.TabIndex = 74;
			this.btnWriteDDC1.Text = "Write";
			this.btnWriteDDC1.UseVisualStyleBackColor = true;
			this.btnWriteDDC1.Click += new System.EventHandler(this.BtnWriteDDC1Click);
			// 
			// timer1
			// 
			this.timer1.Interval = 50;
			this.timer1.Tick += new System.EventHandler(this.Timer1Tick);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(530, 367);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(162, 23);
			this.btnClear.TabIndex = 127;
			this.btnClear.Text = "CLEAR";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.BtnClearClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(720, 432);
			this.Controls.Add(this.tabControl1);
			this.Name = "MainForm";
			this.Text = "QS1R_Control";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.tabPage5.ResumeLayout(false);
			this.tabPage6.ResumeLayout(false);
			this.tabPage7.ResumeLayout(false);
			this.tabPage8.ResumeLayout(false);
			this.tabPage8.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudDDC)).EndInit();
			this.tabPage9.ResumeLayout(false);
			this.tabPage9.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Button btnReadCoef;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button btnLoadCoef;
		private System.Windows.Forms.NumericUpDown nudDDC;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Button txtCLRRSVR;
		private System.Windows.Forms.TextBox txtRCR;
		private System.Windows.Forms.TextBox txtRDEC;
		private System.Windows.Forms.TextBox txtRAO;
		private System.Windows.Forms.TextBox txtRTAP;
		private System.Windows.Forms.TextBox txtC5DEC;
		private System.Windows.Forms.TextBox txtC5SR;
		private System.Windows.Forms.TextBox txtC2DEC;
		private System.Windows.Forms.TextBox txtC2SR;
		private System.Windows.Forms.TextBox txtNPO;
		private System.Windows.Forms.TextBox txtNFRE;
		private System.Windows.Forms.TextBox txtNSCR;
		private System.Windows.Forms.CheckBox cbNCOBypass;
		private System.Windows.Forms.CheckBox cbNCRDITH;
		private System.Windows.Forms.CheckBox cbNCRADith;
		private System.Windows.Forms.CheckBox cbMCRRST;
		private System.Windows.Forms.CheckBox cbMCRDCh;
		private System.Windows.Forms.CheckBox cbMCRComplx;
		private System.Windows.Forms.CheckBox cbMCRMS;
		private System.Windows.Forms.Button btnWriteNCR;
		private System.Windows.Forms.Button btnWriteNSCR;
		private System.Windows.Forms.Button btnWriteNFRE;
		private System.Windows.Forms.Button btnWriteNPO;
		private System.Windows.Forms.Button btnWriteC2SR;
		private System.Windows.Forms.Button btnWriteC2DEC;
		private System.Windows.Forms.Button btnWriteC5SR;
		private System.Windows.Forms.Button btnWriteC5DEC;
		private System.Windows.Forms.Button btnWriteRCR;
		private System.Windows.Forms.Button btnWriteRDEC;
		private System.Windows.Forms.Button btnWriteRAO;
		private System.Windows.Forms.Button btnWriteRTAP;
		private System.Windows.Forms.Button btnWriteMCR;
		private System.Windows.Forms.Button btnCLRCOEF;
		private System.Windows.Forms.Button btnClrRam;
		private System.Windows.Forms.Button btnReadRAO;
		private System.Windows.Forms.Button btnReadRTAP;
		private System.Windows.Forms.Button btnReadRCR;
		private System.Windows.Forms.Button btnReadRDEC;
		private System.Windows.Forms.Button btnReadC5SR;
		private System.Windows.Forms.Button btnReadC5DEC;
		private System.Windows.Forms.Button btnReadC2SR;
		private System.Windows.Forms.Button btnReadC2DEC;
		private System.Windows.Forms.Button btnReadNPO;
		private System.Windows.Forms.Button btnReadNFRE;
		private System.Windows.Forms.Button btnReadNSCR;
		private System.Windows.Forms.Button btnReadNCR;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.Button btnMCRRead;
		private System.Windows.Forms.Button btnWriteDDC1;
		private System.Windows.Forms.Button btnReadDDC1;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.TextBox txtDDC1DR0;
		private System.Windows.Forms.TextBox txtDDC1DR1;
		private System.Windows.Forms.TextBox txtDDC1DR2;
		private System.Windows.Forms.TextBox txtDDC1DR3;
		private System.Windows.Forms.TextBox txtDDC1DR4;
		private System.Windows.Forms.TextBox txtDDC1LAR;
		private System.Windows.Forms.TextBox txtDDC1AMR;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.TabPage tabPage9;
		private System.Windows.Forms.TabPage tabPage8;
		private System.Windows.Forms.Button btnWriteGPIO;
		private System.Windows.Forms.Button btnReadGPIO;
		private System.Windows.Forms.CheckBox cbGPIO0;
		private System.Windows.Forms.CheckBox cbGPIO1;
		private System.Windows.Forms.CheckBox cbGPIO2;
		private System.Windows.Forms.CheckBox cbGPIO3;
		private System.Windows.Forms.CheckBox cbGPIO0DDR;
		private System.Windows.Forms.CheckBox cbGPIO1DDR;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.CheckBox cbGPIO2DDR;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.CheckBox cbGPIO3DDR;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.CheckBox cbDDC0RESET;
		private System.Windows.Forms.CheckBox cbNCOSYNC;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.CheckBox cbDDC0PARSER;
		private System.Windows.Forms.CheckBox cbDDC1PARSER;
		private System.Windows.Forms.CheckBox cbDDC1RESET;
		private System.Windows.Forms.CheckBox cbRCFSYNC;
		private System.Windows.Forms.CheckBox cbCICSYNC;
		private System.Windows.Forms.Button btnWriteDDCCNTRL;
		private System.Windows.Forms.Button btnReadDDCCNTRL;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TabPage tabPage7;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.CheckBox cbPGA;
		private System.Windows.Forms.CheckBox cbRAND;
		private System.Windows.Forms.CheckBox cbSHDN;
		private System.Windows.Forms.CheckBox cbDITH;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox cbHPFBP;
		private System.Windows.Forms.CheckBox cbLPFBP;
		private System.Windows.Forms.CheckBox cbRFABP;
		private System.Windows.Forms.Button btnWriteRFFE;
		private System.Windows.Forms.Button btnReadRFFE;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.Label lblATTNVal;
		private System.Windows.Forms.CheckBox cbATTN0;
		private System.Windows.Forms.CheckBox cbATTN1;
		private System.Windows.Forms.CheckBox cbATTN2;
		private System.Windows.Forms.CheckBox cbATTN3;
		private System.Windows.Forms.Button btnWriteATTN;
		private System.Windows.Forms.Button btnReadATTN;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox cbATTN4;
		private System.Windows.Forms.CheckBox cbATTN5;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.Button btnWriteADC;
		private System.Windows.Forms.Button btnReadADC;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Button btnWriteLEDS;
		private System.Windows.Forms.Button btnReadLEDS;
		private System.Windows.Forms.CheckBox cbLED0;
		private System.Windows.Forms.CheckBox cbLED1;
		private System.Windows.Forms.CheckBox cbLED2;
		private System.Windows.Forms.CheckBox cbLED3;
		private System.Windows.Forms.CheckBox cbLED4;
		private System.Windows.Forms.Button btnDisconnect;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabControl tabControl1;
		
		
	}
}
