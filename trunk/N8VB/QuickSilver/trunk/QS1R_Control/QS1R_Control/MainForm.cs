/*
 * Created by SharpDevelop.
 * User: Phil
 * Date: 12/24/2006
 * Time: 8:16 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HPSDR_USB_LIB_V1;
using QS1R_Control;
using System.IO;

namespace QS1R_Control
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public IntPtr pdev = IntPtr.Zero;
		
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			
		}
		
		void BtnConnectClick(object sender, EventArgs e)
		{
			this.listBox1.Items.Add("Finding QS1R at 0xFFFE 0x00FF...");
			
			int VID = 0xFFFE;
			int PID = 0x00FF;
			
			pdev = USB.InitFindAndOpenDevice(VID, PID);
			
			if (pdev != IntPtr.Zero)
			{
				listBox1.Items.Add("Connected to QS1R: " + pdev.ToInt32());
				this.BtnReadLEDSClick(sender, new System.EventArgs());
				this.BtnReadATTNClick(sender, new System.EventArgs());
				this.BtnReadADCClick(sender, new System.EventArgs());
				this.BtnReadRFFEClick(sender, new System.EventArgs());
				this.BtnReadDDCCNTRLClick(sender, new System.EventArgs());
				this.BtnReadGPIOClick(sender, new System.EventArgs());
				timer1.Enabled = true;
			}
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_close(pdev);			
		}
		
		void BtnDisconnectClick(object sender, EventArgs e)
		{
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_close(pdev);
			pdev = IntPtr.Zero;
		
		}
						
		void BtnWriteLEDSClick(object sender, EventArgs e)
		{
			int value = 0;
			
			if (cbLED0.Checked)
				value += 0x01;
			
			if (cbLED1.Checked)
				value += 0x02;
			
			if (cbLED2.Checked)
				value += 0x04;
			
			if (cbLED3.Checked)
				value += 0x08;
			
			if (cbLED4.Checked)
				value += 0x10;
			
			value ^= 0xFF;
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.DEBUG_LED_REG_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);			
		}
		
		void BtnReadLEDSClick(object sender, EventArgs e)
		{
			byte[] buff = new byte[1];
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_IN, 
			                                 QS1R_Commands.DEBUG_LED_REG_READ, 
			                                 0x00,
			                                 0x00,
			                                 buff,
			                                 buff.Length,
			                                 1000);
			
			cbLED0.Checked = !((buff[0] & 0x01) == 0x01);
			cbLED1.Checked = !((buff[0] & 0x02) == 0x02);
			cbLED2.Checked = !((buff[0] & 0x04) == 0x04);
			cbLED3.Checked = !((buff[0] & 0x08) == 0x08);
			cbLED4.Checked = !((buff[0] & 0x10) == 0x10);			
		}
		
		void BtnReadADCClick(object sender, EventArgs e)
		{
			byte[] buff = new byte[1];
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_IN, 
			                                 QS1R_Commands.ADC_CONTROL_REG_READ, 
			                                 0x00,
			                                 0x00,
			                                 buff,
			                                 buff.Length,
			                                 1000);
			
			cbDITH.Checked = ((buff[0] & 0x01) == 0x01);
			cbSHDN.Checked = ((buff[0] & 0x02) == 0x02);
			cbRAND.Checked = ((buff[0] & 0x04) == 0x04);
			cbPGA.Checked = ((buff[0] & 0x08) == 0x08);			
		}
		
		void BtnWriteADCClick(object sender, EventArgs e)
		{
			int value = 0;
			
			if (cbDITH.Checked)
				value += 0x01;
			
			if (cbSHDN.Checked)
				value += 0x02;
			
			if (cbRAND.Checked)
				value += 0x04;
			
			if (cbPGA.Checked)
				value += 0x08;
						
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.ADC_CONTROL_REG_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);			
		}
		
		void BtnReadATTNClick(object sender, EventArgs e)
		{
			byte[] buff = new byte[1];
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_IN, 
			                                 QS1R_Commands.ATTN_REG_READ, 
			                                 0x00,
			                                 0x00,
			                                 buff,
			                                 buff.Length,
			                                 1000);
			
			cbATTN0.Checked = !((buff[0] & 0x01) == 0x01);
			cbATTN1.Checked = !((buff[0] & 0x02) == 0x02);
			cbATTN2.Checked = !((buff[0] & 0x04) == 0x04);
			cbATTN3.Checked = !((buff[0] & 0x08) == 0x08);
			cbATTN4.Checked = !((buff[0] & 0x10) == 0x10);
			cbATTN5.Checked = !((buff[0] & 0x20) == 0x20);
			
			int value = buff[0] ^ 0xff;
			
			lblATTNVal.Text = "Attn Val: " + CalculateAttenuation(value);
		}
		
		void BtnWriteATTNClick(object sender, EventArgs e)
		{
			int value = 0;
			
			if (cbATTN0.Checked)
				value += 0x01;
			
			if (cbATTN1.Checked)
				value += 0x02;
			
			if (cbATTN2.Checked)
				value += 0x04;
			
			if (cbATTN3.Checked)
				value += 0x08;
			
			if (cbATTN4.Checked)
				value += 0x10;
			
			if (cbATTN5.Checked)
				value += 0x20;
			
			value ^= 0xff;
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.ATTN_REG_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);
			
			value ^= 0xff;
			
			lblATTNVal.Text = "Attn Val: " + CalculateAttenuation(value);
			
		}
		
		double CalculateAttenuation(int value)
		{
			double atten = 0;
			
			if ((value & 0x01) == 0x01)
				atten += 16.0;
			if ((value & 0x02) == 0x02)
				atten += 8.0;
			if ((value & 0x04) == 0x04)
				atten += 4.0;
			if ((value & 0x08) == 0x08)
				atten += 2.0;
			if ((value & 0x10) == 0x10)
				atten += 1.0;
			if ((value & 0x20) == 0x20)
				atten += 0.5;
			
			return atten;
		}
		
			
		void BtnReadRFFEClick(object sender, EventArgs e)
		{
			byte[] buff = new byte[1];
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_IN, 
			                                 QS1R_Commands.RFFE_REG_READ, 
			                                 0x00,
			                                 0x00,
			                                 buff,
			                                 buff.Length,
			                                 1000);
			
			cbHPFBP.Checked = ((buff[0] & 0x01) == 0x01);
			cbLPFBP.Checked = ((buff[0] & 0x02) == 0x02);
			cbRFABP.Checked = ((buff[0] & 0x04) == 0x04);					
		}
		
		void BtnWriteRFFEClick(object sender, System.EventArgs e)
		{
			int value = 0;
			
			if (cbHPFBP.Checked)
				value += 0x01;
			
			if (cbLPFBP.Checked)
				value += 0x02;
			
			if (cbRFABP.Checked)	
				value += 0x04;			
						
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.RFFE_REG_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);			
		}
						
		
		void BtnReadDDCCNTRLClick(object sender, EventArgs e)
		{
			byte[] buff = new byte[1];
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_IN, 
			                                 QS1R_Commands.DDC_CNTRL_READ, 
			                                 0x00,
			                                 0x00,
			                                 buff,
			                                 buff.Length,
			                                 1000);
			
			cbDDC0PARSER.Checked = ((buff[0] & 0x01) == 0x01);
			cbDDC0RESET.Checked = ((buff[0] & 0x02) == 0x02);
			cbDDC1PARSER.Checked = ((buff[0] & 0x04) == 0x04);
			cbDDC1RESET.Checked = ((buff[0] & 0x08) == 0x08);
			cbRCFSYNC.Checked = ((buff[0] & 0x10) == 0x10);
			cbCICSYNC.Checked = ((buff[0] & 0x20) == 0x20);
			cbNCOSYNC.Checked = ((buff[0] & 0x40) == 0x40);
		}
		
		void BtnWriteDDCCNTRLClick(object sender, EventArgs e)
		{
			int value = 0;
			
			if (cbDDC0PARSER.Checked)
				value += 0x01;
			
			if (cbDDC0RESET.Checked)
				value += 0x02;
			
			if (cbDDC1PARSER.Checked)
				value += 0x04;
			
			if (cbDDC1RESET.Checked)
				value += 0x08;
			
			if (cbRCFSYNC.Checked)
				value += 0x10;
			
			if (cbCICSYNC.Checked)
				value += 0x20;
			
			if (cbNCOSYNC.Checked)
				value += 0x40;
						
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.DDC_CNTRL_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);
		}
		
		void BtnReadGPIOClick(object sender, EventArgs e)
		{
			byte[] buff = new byte[1];
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_IN, 
			                                 QS1R_Commands.CPLD_GPIO_READ, 
			                                 0x00,
			                                 0x00,
			                                 buff,
			                                 buff.Length,
			                                 1000);
			
			cbGPIO0.Checked = ((buff[0] & 0x01) == 0x01);
			cbGPIO1.Checked = ((buff[0] & 0x02) == 0x02);
			cbGPIO2.Checked = ((buff[0] & 0x04) == 0x04);
			cbGPIO3.Checked = ((buff[0] & 0x08) == 0x08);
			cbGPIO0DDR.Checked = ((buff[0] & 0x10) == 0x10);
			cbGPIO1DDR.Checked = ((buff[0] & 0x20) == 0x20);
			cbGPIO2DDR.Checked = ((buff[0] & 0x40) == 0x40);
			cbGPIO3DDR.Checked = ((buff[0] & 0x80) == 0x80);
		}	
				
		void Timer1Tick(object sender, EventArgs e)
		{
			//DoIt();
		}
		
		void BtnReadDDC1Click(object sender, EventArgs e)
		{
			int value = Int32.Parse ( txtDDC1AMR.Text, System.Globalization.NumberStyles.HexNumber );
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.DDC0_AMR_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);
			
			value = Int32.Parse ( txtDDC1LAR.Text, System.Globalization.NumberStyles.HexNumber );
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.DDC0_LAR_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);
			
			txtDDC1DR0.Text = "";
			txtDDC1DR1.Text = "";
			txtDDC1DR2.Text = "";
			txtDDC1DR3.Text = "";
			txtDDC1DR4.Text = "";
			
			byte[] buff = new byte[1];
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_IN, 
			                                 QS1R_Commands.DDC0_DR0_READ, 
			                                 0x00,
			                                 0x00,
			                                 buff,
			                                 buff.Length,
			                                 1000);
			
			txtDDC1DR0.Text = buff[0].ToString("X");
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_IN, 
			                                 QS1R_Commands.DDC0_DR1_READ, 
			                                 0x00,
			                                 0x00,
			                                 buff,
			                                 buff.Length,
			                                 1000);
			
			txtDDC1DR1.Text = buff[0].ToString("X");
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_IN, 
			                                 QS1R_Commands.DDC0_DR2_READ, 
			                                 0x00,
			                                 0x00,
			                                 buff,
			                                 buff.Length,
			                                 1000);
			
			txtDDC1DR2.Text = buff[0].ToString("X");
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_IN, 
			                                 QS1R_Commands.DDC0_DR3_READ, 
			                                 0x00,
			                                 0x00,
			                                 buff,
			                                 buff.Length,
			                                 1000);
			
			txtDDC1DR3.Text = buff[0].ToString("X");
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_IN, 
			                                 QS1R_Commands.DDC0_DR4_READ, 
			                                 0x00,
			                                 0x00,
			                                 buff,
			                                 buff.Length,
			                                 1000);
			
			txtDDC1DR4.Text = buff[0].ToString("X");
		}
		
		void BtnWriteDDC1Click(object sender, EventArgs e)
		{
			int value = Int32.Parse ( txtDDC1AMR.Text, System.Globalization.NumberStyles.HexNumber );
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.DDC1_AMR_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);
			
			value = Int32.Parse ( txtDDC1LAR.Text, System.Globalization.NumberStyles.HexNumber );
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.DDC1_LAR_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);
			
			value = Int32.Parse ( txtDDC1DR4.Text, System.Globalization.NumberStyles.HexNumber );
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.DDC1_DR4_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);
			
			value = Int32.Parse ( txtDDC1DR3.Text, System.Globalization.NumberStyles.HexNumber );
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.DDC1_DR3_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);
			
			value = Int32.Parse ( txtDDC1DR2.Text, System.Globalization.NumberStyles.HexNumber );
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.DDC1_DR2_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);
			
			
			value = Int32.Parse ( txtDDC1DR1.Text, System.Globalization.NumberStyles.HexNumber );
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.DDC1_DR1_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);
			
			value = Int32.Parse ( txtDDC1DR0.Text, System.Globalization.NumberStyles.HexNumber );
			
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.DDC1_DR0_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);
		}
		
		void BtnWriteGPIOClick(object sender, EventArgs e)
		{
			int value = 0;
			
			if (cbGPIO0.Checked)
				value += 0x01;
			
			if (cbGPIO1.Checked)
				value += 0x02;
			
			if (cbGPIO2.Checked)
				value += 0x04;
			
			if (cbGPIO3.Checked)
				value += 0x08;
			
			if (cbGPIO0DDR.Checked)
				value += 0x10;
			
			if (cbGPIO1DDR.Checked)
				value += 0x20;
			
			if (cbGPIO2DDR.Checked)
				value += 0x40;
			
			if (cbGPIO3DDR.Checked)
				value += 0x80;
						
			if (pdev != IntPtr.Zero)
				libUSB_Interface.usb_control_msg(pdev, 
			                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT, 
			                                 QS1R_Commands.CPLD_GPIO_WRITE, 
			                                 value,
			                                 0x00,
			                                 new byte[0],
			                                 0,
			                                 1000);
		}
		
		void BtnMCRReadClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[1];
			int ret = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_MCR_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			this.listBox2.Items.Add("MCR=" + buf[0].ToString("X"));
			
			this.cbMCRRST.Checked = false;
			this.cbMCRDCh.Checked = false;
			this.cbMCRComplx.Checked = false;
			this.cbMCRMS.Checked = false;
			
			if ((buf[0] & 0x01) == 0x01) this.cbMCRRST.Checked = true;
			if ((buf[0] & 0x02) == 0x02) this.cbMCRDCh.Checked = true;
			if ((buf[0] & 0x04) == 0x04) this.cbMCRComplx.Checked = true;
			if ((buf[0] & 0x08) == 0x08) this.cbMCRMS.Checked = true;
		}
		
		void BtnReadNCRClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[1];
			int ret = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_NCR_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			this.listBox2.Items.Add("NCR=" + buf[0].ToString("X"));
			
			this.cbNCOBypass.Checked = false;
			this.cbNCRDITH.Checked = false;
			this.cbNCRADith.Checked = false;
						
			if ((buf[0] & 0x01) == 0x01) this.cbNCOBypass.Checked = true;
			if ((buf[0] & 0x02) == 0x02) this.cbNCRDITH.Checked = true;
			if ((buf[0] & 0x04) == 0x04) this.cbNCRADith.Checked = true;
						
		}
		
		void BtnReadNSCRClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[4];
			int ret = 0;
			int value = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_NSCR_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			value = (buf[3]<<24) + (buf[2]<<16) + (buf[1]<<8) + (buf[0]);
			
			this.listBox2.Items.Add("NSCR=" + value.ToString("X"));
			this.txtNSCR.Text = value.ToString("X");
		}
		
		void BtnReadNFREClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[4];
			int ret = 0;
			int value = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_NFRE_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			value = (buf[3]<<24) + (buf[2]<<16) + (buf[1]<<8) + (buf[0]);
			
			this.listBox2.Items.Add("NFRE=" + value.ToString("X"));
			this.txtNFRE.Text = value.ToString("X");
		}
		
		void BtnReadNPOClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[2];
			int ret = 0;
			int value = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_NPO_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			value = (buf[1]<<8) + (buf[0]);
			
			this.listBox2.Items.Add("NPO=" + value.ToString("X"));
			this.txtNPO.Text = value.ToString("X");
		}
		
		void BtnReadC2SRClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[1];
			int ret = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_C2SR_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			this.listBox2.Items.Add("C2SR=" + buf[0].ToString("X"));
			this.txtC2SR.Text = buf[0].ToString("X");
		}
		
		void BtnReadC2DECClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[1];
			int ret = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_C2DEC_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			this.listBox2.Items.Add("C2DEC=" + buf[0].ToString("X"));
			this.txtC2DEC.Text = buf[0].ToString("X");
		}
		
		void BtnReadC5SRClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[1];
			int ret = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_C5SR_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			this.listBox2.Items.Add("C5SR=" + buf[0].ToString("X"));
			this.txtC5SR.Text = buf[0].ToString("X");
		}
		
		void BtnReadC5DECClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[1];
			int ret = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_C5DEC_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			this.listBox2.Items.Add("C5DEC=" + buf[0].ToString("X"));
			this.txtC5DEC.Text = buf[0].ToString("X");
		}
		
		void BtnReadRCRClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[1];
			int ret = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_RCR_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			this.listBox2.Items.Add("RCR=" + buf[0].ToString("X"));
			this.txtRCR.Text = buf[0].ToString("X");
		}
		
		void BtnReadRDECClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[1];
			int ret = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_RDEC_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			this.listBox2.Items.Add("RDEC=" + buf[0].ToString("X"));
			this.txtRDEC.Text = buf[0].ToString("X");
		}
		
		void BtnReadRAOClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[1];
			int ret = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_RAO_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			this.listBox2.Items.Add("RAO=" + buf[0].ToString("X"));
			this.txtRAO.Text = buf[0].ToString("X");
		}
		
		void BtnReadRTAPClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[1];
			int ret = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_RTAPS_READ,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			
			this.listBox2.Items.Add("RTAP=" + buf[0].ToString("X"));
			this.txtRTAP.Text = buf[0].ToString("X");
		}
		
		void BtnClrRamClick(object sender, EventArgs e)
		{
			int ret = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_RAM_CLR,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 new byte[0],
				                                 0,
				                                 1000);
			
			if (ret != 0)
				this.listBox2.Items.Add("RAM CLEARED");
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnCLRCOEFClick(object sender, EventArgs e)
		{
			int ret = 0;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_COEF_CLR,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 new byte[0],
				                                 0,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("COEF CLEARED");
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		
		
		void btnWriteMCRClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[1];
			
			if (this.cbMCRRST.Checked) value |= 0x01;
			if (this.cbMCRDCh.Checked) value |= 0x02;
			if (this.cbMCRComplx.Checked) value |= 0x04;
			if (this.cbMCRMS.Checked) value |= 0x08;
			
			buf[0] = (byte)value;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_MCR_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("MCR SET:" + value);
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnWriteNCRClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[1];
			
			if (this.cbNCOBypass.Checked) value |= 0x01;
			if (this.cbNCRDITH.Checked) value |= 0x02;
			if (this.cbNCRADith.Checked) value |= 0x04;
						
			buf[0] = (byte)value;
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_NCR_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("NCR SET:" + value);
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnWriteNSCRClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[4];
			
			value =  int.Parse(this.txtNSCR.Text, System.Globalization.NumberStyles.HexNumber);
			
			buf[0] = (byte)(value & 0xFF);
			buf[1] = (byte)((value & 0xFF00) >> 8);
			buf[2] = (byte)((value & 0xFF0000) >> 16);
			buf[3] = (byte)((value & 0xFF000000) >> 24);
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_NSCR_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("NSCR SET:" + value.ToString("X"));
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnWriteNFREClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[4];
			
			value =  int.Parse(this.txtNFRE.Text, System.Globalization.NumberStyles.HexNumber);
			
			buf[0] = (byte)(value & 0xFF);
			buf[1] = (byte)((value & 0xFF00) >> 8);
			buf[2] = (byte)((value & 0xFF0000) >> 16);
			buf[3] = (byte)((value & 0xFF000000) >> 24);
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_NFRE_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("NFRE SET:" + value.ToString("X"));
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnWriteNPOClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[2];
			
			value =  int.Parse(this.txtNPO.Text, System.Globalization.NumberStyles.HexNumber);
			
			buf[0] = (byte)(value & 0xFF);
			buf[1] = (byte)((value & 0xFF00) >> 8);			
			
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_NPO_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("NPO SET:" + value.ToString("X"));
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnWriteC2SRClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[1];
			
			value =  int.Parse(this.txtC2SR.Text, System.Globalization.NumberStyles.HexNumber);
			
			buf[0] = (byte)(value & 0xFF);
						
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_C2SR_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("C2SR SET:" + value.ToString("X"));
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnWriteC2DECClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[1];
			
			value =  int.Parse(this.txtC2DEC.Text, System.Globalization.NumberStyles.HexNumber);
			
			buf[0] = (byte)(value & 0xFF);
						
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_C2DEC_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("C2DEC SET:" + value.ToString("X"));
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnWriteC5SRClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[1];
			
			value =  int.Parse(this.txtC5SR.Text, System.Globalization.NumberStyles.HexNumber);
			
			buf[0] = (byte)(value & 0xFF);
						
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_C5SR_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("C5SR SET:" + value.ToString("X"));
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnWriteC5DECClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[1];
			
			value =  int.Parse(this.txtC5DEC.Text, System.Globalization.NumberStyles.HexNumber);
			
			buf[0] = (byte)(value & 0xFF);
						
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_C5DEC_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("C5DEC SET:" + value.ToString("X"));
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnWriteRCRClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[1];
			
			value =  int.Parse(this.txtRCR.Text, System.Globalization.NumberStyles.HexNumber);
			
			buf[0] = (byte)(value & 0xFF);
						
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_RCR_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("RCR SET:" + value.ToString("X"));
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnWriteRDECClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[1];
			
			value =  int.Parse(this.txtRDEC.Text, System.Globalization.NumberStyles.HexNumber);
			
			buf[0] = (byte)(value & 0xFF);
						
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_RDEC_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("RDEC SET:" + value.ToString("X"));
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnWriteRAOClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[1];
			
			value =  int.Parse(this.txtRAO.Text, System.Globalization.NumberStyles.HexNumber);
			
			buf[0] = (byte)(value & 0xFF);
						
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_RAO_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("RAO SET:" + value.ToString("X"));
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void BtnWriteRTAPClick(object sender, EventArgs e)
		{
			int value = 0;
			int ret = 0;
			byte[] buf = new byte[1];
			
			value =  int.Parse(this.txtRTAP.Text, System.Globalization.NumberStyles.HexNumber);
			
			buf[0] = (byte)(value & 0xFF);
						
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_RTAPS_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("RTAP SET:" + value.ToString("X"));
			else
				this.listBox2.Items.Add("ERROR");
		}
		
		void TxtCLRRSVRClick(object sender, EventArgs e)
		{
			int ret = 0;
			byte[] buf = new byte[1];
			
			buf[0] = (byte)(0x0);
						
			if (pdev != IntPtr.Zero)
				ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_RSVR_WRITE,
				                                 0x0,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
			if (ret != 0)
				this.listBox2.Items.Add("RSRV CLEARED");
			else
				this.listBox2.Items.Add("ERROR");
			
		}
		
		void BtnLoadCoefClick(object sender, EventArgs e)
		{
			int value = 0;
			byte[] buf = new byte[48];
			int index = 0;
						
			this.openFileDialog1.FileName = "";
			this.openFileDialog1.ShowDialog();
			System.IO.StreamReader sr = null;
			
			try {
				sr = new StreamReader(this.openFileDialog1.FileName);
				
				string[] strarray = new string[3];
				
				for (int r=0; r<3; r++)
				{
					if (sr.EndOfStream) break;
					string txt = sr.ReadLine();
					strarray[r] = txt.Substring(0,txt.IndexOf("&", 0));
				}
				
				this.txtC2DEC.Text = (Int32.Parse(strarray[0])-1).ToString("X");
				this.txtC5DEC.Text = (Int32.Parse(strarray[1])-1).ToString("X");
				this.txtRDEC.Text = (Int32.Parse(strarray[2])-1).ToString("X");
								
				// read and discard next three entries in file
				for (int t=0; t < 3; t++)
				{
					if (sr.EndOfStream) break;
					sr.ReadLine();
				}
				
				while (!sr.EndOfStream)
				{
					for (int i=0; i < 48; i+=3)
					{
						if (sr.EndOfStream) break;
						value = int.Parse(sr.ReadLine());
						buf[i+0] = (byte)((value & 0xFF));
						buf[i+1] = (byte)((value & 0xFF00) >> 8);
						buf[i+2] = (byte)((value & 0x0F0000) >> 16);
						this.listBox2.Items.Add(buf[i+2].ToString("X") + buf[i+1].ToString("X") + buf[i].ToString("X"));
					}
					this.listBox2.Items.Add("Writing index:" + index);
					if (pdev != IntPtr.Zero)
						libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_OUT,
				                                 QS1R_Commands.DDC_COEF_WRITE,
				                                 (byte)index,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
					index++;
					for (int c=0; c<48; c++)
					{
						buf[c] = 0;
					}
				}
				this.txtRTAP.Text = ((index * 16) - 1).ToString("X");
			}
			catch (Exception ex)
			{
				System.Console.WriteLine("Error: " + ex.Message);
			}
			sr.Close();
		}
		
		void BtnReadCoefClick(object sender, EventArgs e)
		{
			byte[] buf = new byte[48];
			int ret = 0;
			int[] value = new int[256];
			int index = 0;
			
			for (int i=0; i<16; i++)
			{
				if (pdev != IntPtr.Zero)
					ret = libUSB_Interface.usb_control_msg(pdev,
				                                 QS1R_Commands.VENDOR_REQ_TYPE_IN,
				                                 QS1R_Commands.DDC_COEF_READ,
				                                 (byte)i,
				                                 (byte)this.nudDDC.Value,
				                                 buf,
				                                 buf.Length,
				                                 1000);
				for (int j=0, m=0; j<48; j+=3, m++)
				{
					value[m+index] = (buf[j+2]<<16) + (buf[j+1]<<8) + buf[j];
				}
				index+=16;
			}
			
			for (int k=0; k < 256; k++)
			{
				this.listBox2.Items.Add("C" + k + ":" + value[k].ToString("X"));
			}
		}
		
		void BtnClearClick(object sender, EventArgs e)
		{
			this.listBox2.Items.Clear();
		}
	}
}
