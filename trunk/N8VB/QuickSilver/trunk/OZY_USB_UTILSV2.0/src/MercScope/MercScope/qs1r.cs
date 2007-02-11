/*
 * Created by SharpDevelop.
 * User: Phil C
 * Date: 2/1/2007
 * Time: 8:53 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Threading;
using HPSDR_USB_LIB_V1;
using SharpDSP;

namespace MercScope
{			
	public class qs1r
	{	
		private IntPtr hdev = IntPtr.Zero;
		private SharpDSP.Filter f = new Filter(512 * 2);		
					
		public qs1r()
		{
			//f.MakeFilter(-96000.0, 96000.0, 100e6/520, FilterType.BandPass, WindowType.BLACKMANHARRIS_WINDOW);
			f.MakeFilterFromCoeffArray(fir_coeff.coeff_cfir);
		}
		
		public bool Connect(int VID, int PID)
		{
			try
			{
				hdev = USB.InitFindAndOpenDevice(VID, PID);
	            libUSB_Interface.usb_set_configuration(hdev, 1);
	            libUSB_Interface.usb_claim_interface(hdev, 0);
	            libUSB_Interface.usb_set_altinterface(hdev, 0);
	            libUSB_Interface.usb_clear_halt(hdev, 0x02);
	            libUSB_Interface.usb_clear_halt(hdev, 0x86);	            
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				return false;
			}
			return true;
		}
		
		public void Close()
		{
			libUSB_Interface.usb_clear_halt(hdev, 0x02);
	        libUSB_Interface.usb_clear_halt(hdev, 0x86);
			libUSB_Interface.usb_release_interface(hdev, 0);
            libUSB_Interface.usb_close(hdev); 
		}
						
		public bool SetFrequency(double frequency)
		{
			int ifrequency = (int)((frequency/100e6) * Math.Pow(2,32));
				
			byte[] buf = BitConverter.GetBytes(ifrequency); // gets 4 bytes
			byte[] tempbuf = new byte[4];
			
			if (buf.Length < 4) return false;
			
			// put bytes in proper order MSB to LSB
			for (int i = 0; i < buf.Length; i++)
			{
				tempbuf[buf.Length - 1 - i] = buf[i];
			}

			buf = tempbuf;

			if (hdev != IntPtr.Zero)
			{
				OZY.Write_SPI(hdev, 0, 0x01, 
					OZY.SPI_EN_FPGA, (OZY.SPI_FMT_MSB | OZY.SPI_FMT_HDR_1), buf);
				return true;
			}
			return false;
		}
		
		public bool SetOutputScale(int scale)
		{
			byte[] buf = BitConverter.GetBytes(scale);
            byte[] tempbuf = new byte[4];

            if (buf.Length < 4) return false;

            // put bytes in proper order MSB to LSB
            for (int i = 0; i < buf.Length; i++)
            {
                tempbuf[buf.Length - 1 - i] = buf[i];
            }
            
            buf = tempbuf;
            //send to OZY register 0x02, 32 bits
            if (hdev != IntPtr.Zero)
            {
            	OZY.Write_SPI(hdev, 0, 0x03, OZY.SPI_EN_FPGA, (OZY.SPI_FMT_MSB | OZY.SPI_FMT_HDR_1), buf);
            	return true;
            }
            return false;
		}
		
		public void GetBlock(ref double[] ibuffer, ref double[] qbuffer)
		{
			byte[] rbuf = new byte[ibuffer.Length * 8]; // 8 bytes per sample
						
			int ret = libUSB_Interface.usb_bulk_read(hdev, 0x86, rbuf, 500);
			
			if (ret == rbuf.Length)
			{
				for (int i = 0, j = 0; i < rbuf.Length; i += 8, j++)
				{
					ibuffer[j] = Convert.ToDouble((BitConverter.ToInt32(rbuf,i+0)/Math.Pow(2,31)));
					qbuffer[j] = Convert.ToDouble((BitConverter.ToInt32(rbuf,i+4)/Math.Pow(2,31)));
				}
				
				
				// expand bufffer to 2 x blocksize
				double[] temp_i = new double[ibuffer.Length * 2];
				double[] temp_q = new double[ibuffer.Length * 2];
				
				for (int i=0; i<ibuffer.Length; i++)
				{
					temp_i[i] = ibuffer[i];
					temp_q[i] = qbuffer[i];
				}
				
				f.DoFilter(ref temp_i, ref temp_q);
				f.DoOverlapAdd(ref temp_i, ref temp_q);
				
				for (int i=0; i<ibuffer.Length; i++)
				{
					ibuffer[i] = temp_i[i];
					qbuffer[i] = temp_q[i];
				}
				
			}
		}		
		
	}
}
