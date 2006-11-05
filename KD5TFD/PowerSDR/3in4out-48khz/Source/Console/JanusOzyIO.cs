/*
*
* Copyright (C) 2006 Bill Tracey, KD5TFD 
* Copyright (C) 2006 Philip A. Covington, N8VB
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 2 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/


// Substantial portions of this code are derived from Phil Covington's  (N8VB) load_firmwware, upload_FPGA and 
// write_i2c utilities 
// 

// 
// This class provides access to control the JanusOzy  device 
// 

using System;
using System.Threading; 
using HPSDR_USB_LIB_V1;

namespace PowerSDR
{
	/// <summary>
	/// Summary description for JanusOzyIO.
	/// </summary>
	public class JanusOzyIO
	{
		static JanusOzyIO Singleton = null; 
		private readonly static int OZY_VID = 0xfffe; 
		private readonly static int OZY_PID = 0x7; 


		public static JanusOzyIO getJanusOzy() 
		{ 
			JanusOzyIO result; 
			if ( Singleton == null ) 
			{ 
				result = new JanusOzyIO(); 
				Singleton = result; 
			} 
			return Singleton; 
		} 

		// 
		// returns 0 on success !0 on failure 
		// 
		// rc -- -1 libusb initialization failed 		
		//       -3 Open on Ozy failed 
		//       -4 failed to load firmware 
		//       -5 FPGA load failed 
		//       -6 Open after renumerate failed 
		//       -7 initialization of TLV320AIC23B failed 

		public int init() 
		{ 
			libUSB_Interface.usb_bus bus = null; 
			try 
			{
				libUSB_Interface.usb_init();
				System.Console.WriteLine("finding busses...");
				libUSB_Interface.usb_find_busses();
				System.Console.WriteLine("finding devices...");
				libUSB_Interface.usb_find_devices();
				System.Console.WriteLine("usb_get_busses...");
				bus = libUSB_Interface.usb_get_busses();
				if ( bus != null ) 
				{ 
					System.Console.WriteLine("bus location: " + bus.location.ToString());
				}
			}
			catch ( Exception ex ) 
			{ 
				System.Console.WriteLine("libusb initialization failed: " + ex.Message); 
				bus = null; 
			} 
			if ( bus == null ) 
			{ 
				return -1; 
			} 

			IntPtr usb_devh; 
			usb_devh = findAndOpenOzy(bus); 

			if ( usb_devh == (IntPtr)0 ) 
			{ 
				return -3; 
			} 
			int rc = 0; 
			do 
			{ 
				if ( ( rc = loadFirmware(usb_devh, "ozyfw.hex")) != 0  )  
				{	
					System.Console.WriteLine("loadFirmware failed.  rc: " + rc); 
					rc = -4;
					break;						
				}

				// close interface and get it again since it's renumerate'd 
				libUSB_Interface.usb_close(usb_devh);			
				// sleep a bit to wait for the FX2 to come out of reset and renumerate 

				int total_sleep = 0; 
				usb_devh = (IntPtr)0; 				

				while ( total_sleep < 5000 ) 
				{ 
					Thread.Sleep(200); 
					usb_devh = findAndOpenOzy(bus); 
					total_sleep += 200; 
					if ( usb_devh != (IntPtr)0 ) 
					{
						break; 
					}
				}

				Thread.Sleep(200);  // sleep a little more - sometimes FX2 not quite 
				                    // ready to rock and roll right away 


				System.Console.WriteLine("total_sleep: " + total_sleep); 

				// usb_devh = findAndOpenOzy(bus); 
				if ( usb_devh == (IntPtr)0 ) 
				{ 
					return -6; 
				} 
				
				if ( ( rc = loadFPGA(usb_devh, "ozy.rbf")) != 0 )  
				{
					System.Console.WriteLine("loadFPGA failed. rc: " + rc); 
					rc = -5; 
				}

				if ( ( rc = initTLV320AIC23B(usb_devh)) != 0 ) 
				{
					System.Console.WriteLine("initTLV320AIC23B failed. rc: " + rc); 
					rc = -7; 
				}


			} while ( false ); 

			libUSB_Interface.usb_close(usb_devh);						
			return rc; 				
		}



		private readonly int TLV_I2C_ADDR = 0x1a; 
																																																											 
		private readonly byte[][] TLV_INIT_COMMANDS =	

	     {	
			new byte[] { 0x1e, 0x00 },		// reset chip  
			new byte[] { 0x12, 0x01 },		// set digital interface active       
			new byte[] { 0x08, 0x15 },		// D/A on, mic input, mic 20dB boost  
		 // new byte[] { 0x08, 0x14 },      // ditto but no mic boost             
			new byte[] { 0x0c, 0x00 },      // All chip power on                  
			new byte[] { 0x0e, 0x02 },		// Slave, 16 bit, I2S                 
			new byte[] { 0x10, 0x00 },      // 48k, Normal mode                   
			new byte[] { 0x0a, 0x00 },		// turn D/A mute off                  
	     };

		private int initTLV320AIC23B(IntPtr devh) 
		{
			int rc = 0; 
			for ( int i = 0; i < 7 /* TLV_INIT_COMMANDS.Length/TLV_INIT_COMMANDS[0].Length */ ; i++ ) 
			{	
				// Thread.Sleep(500); 
				// System.Console.WriteLine("initTLV i: " + i + " " + TLV_INIT_COMMANDS[i][0] + " " + TLV_INIT_COMMANDS[i][1]); 
				if ( ! (OZY.Write_I2C(devh, TLV_I2C_ADDR, TLV_INIT_COMMANDS[i])) )				
				{ 
					rc = -1; 
					break; 
				} 
			} 
			System.Console.WriteLine("initTLV returns: " + rc); 
			return rc; 
		}

		private IntPtr findAndOpenOzy(libUSB_Interface.usb_bus bus) 
		{
			libUSB_Interface.usb_device fdev;
			try 
			{ 
				fdev = 	HPSDR_USB_LIB_V1.USB.FindDevice(bus, OZY_VID, OZY_PID);
			}
			catch ( Exception ex ) 
			{ 
				System.Console.WriteLine("FindDevice exception: " + ex); 
				fdev = null; 
			} 
			if ( fdev == null ) 
			{ 
				System.Console.WriteLine("Error - can't find Ozy!\n"); 
				return (IntPtr)0; 
			}			
			IntPtr usb_devh; 
			try
			{
				usb_devh = libUSB_Interface.usb_open(fdev);				
			}
			catch (Exception e)
			{
				System.Console.WriteLine("usb_open exception:  " + e.Message);
				usb_devh = (IntPtr)0; 
			}
			return usb_devh; 
		}

		private int loadFPGA(IntPtr devh, string fname) 
		{
			int rc = 0; 
			if (!(OZY.Load_FPGA(devh, fname)))
			{
				System.Console.WriteLine("FPGA LOAD FAILED!");
				rc = -1; 
			}
//			else 
//			{
//				System.Console.WriteLine("FPGA LOAD SUCCEEDED!");
//
//			}	
			return rc; 
		}

		private int loadFirmware(IntPtr devh, string fname) 
		{ 
			bool rc; 
			rc = HPSDR_USB_LIB_V1.USB.Reset_CPU(devh, true);
			System.Console.WriteLine("reset on: " + rc); 
			try
			{
				rc = HPSDR_USB_LIB_V1.USB.Upload_Firmware(devh, fname);
			}
			catch (Exception e)
			{				
				System.Console.WriteLine("Upload_Firmware failed: " + e.Message);
				return -1; 
			}					
			System.Console.WriteLine("Upload_Firmware: " + rc); 

			HPSDR_USB_LIB_V1.USB.Reset_CPU(devh, false);			
			System.Console.WriteLine("reset off: " + rc); 
			return 0; 
		}

		private JanusOzyIO()
		{			
		}
	}
}
