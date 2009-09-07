/*
*
* Copyright (C) 2006 Bill Tracey, KD5TFD, bill@ewjt.com 
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


using System;
using System.Runtime.InteropServices;
using System.Diagnostics; 
using HPSDR_USB_LIB_V1;

namespace PowerSDR
{
	//
	// routines to access audio from kd5tfd/vk6aph fpga based audio 
	// 
	public class JanusAudio
	{
		public JanusAudio()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private static bool isOzyInitialized = false; 

		private static bool isFirmwareLoaded = false;

		private static void dummy_to_remove_warning() // added to remove fallacious warning about variable never being used
		{
			isFirmwareLoaded = isFirmwareLoaded;
		}

		// get ozy firmware version string - 8 bytes.  returns 
		// null for error 
		private static string getOzyFirmwareString() 
		{ 
			IntPtr oz_h = JanusAudio.OzyOpen(); 
			
			if ( oz_h == (IntPtr)0 ) 
			{ 
				return null; 
			} 
			IntPtr usb_h = JanusAudio.OzyHandleToRealHandle(oz_h); 
			if ( usb_h == (IntPtr)0 ) 
			{ 
				JanusAudio.OzyClose(oz_h); 
				return null; 
			} 

			byte[] buf = new byte[8]; 
			int rc = libUSB_Interface.usb_control_msg(usb_h, 
				                                      OzySDR1kControl.VRT_VENDOR_IN, 
				                                      OzySDR1kControl.VRQ_SDR1K_CTL, 
				                                      OzySDR1kControl.SDR1KCTRL_READ_VERSION, 
				                                      0, buf, buf.Length, 1000); 
			// System.Console.WriteLine("read version rc: " + rc); 

			string result = null;
			
			if ( rc == 8 )    // got length we expected 
			{
				char[] cbuf = new char[8]; 
				for ( int i = 0; i < 8; i++ ) 
				{ 
					cbuf[i] = (char)(buf[i]); 
				} 
				result = new string(cbuf); 			
				System.Console.WriteLine("version: >" + result + "<"); 
			}
			JanusAudio.OzyClose(oz_h); 
			return result; 
		} 

		// returns 0 on success, !0 on failure 
		public static int initOzy()  
		{

			if ( !isOzyAttached() ) 
			{ 
				return 1; 
			} 

			string oz_fw_version = getOzyFirmwareString(); 

			if ( oz_fw_version == null )  // formware not loaded -- load it
			{ 
				ProcessStartInfo start_info = new ProcessStartInfo(); 
				start_info.FileName = "initozy11.bat"; 
				start_info.UseShellExecute = true; 
				Process p = new Process(); 
				p.StartInfo = start_info; 
				bool rc = p.Start(); 
				// System.Console.WriteLine("start returned: " + rc); 
				p.WaitForExit(); 
				// System.Console.WriteLine("OzyInit completes"); 				
			

				// load it again 
				oz_fw_version = getOzyFirmwareString(); 
			}

			if ( oz_fw_version == null ) 
			{
				return 1; 
			}

			/* else */ 
			isFirmwareLoaded = true; 
			return 0; 
		}

		
#if false 
		// old  obsolete code follows 

		public static int oz_start() 
		{
			libUSB_Interface.usb_bus bus;

			try
			{
				libUSB_Interface.usb_init();
				System.Console.WriteLine("finding busses...");
				libUSB_Interface.usb_find_busses();
				System.Console.WriteLine("finding devices...");
				libUSB_Interface.usb_find_devices();
				System.Console.WriteLine("usb_get_busses...");
				bus = libUSB_Interface.usb_get_busses();
				System.Console.WriteLine("bus location: " + bus.location.ToString());
			}
			catch (Exception e)
			{
				System.Console.WriteLine("An error occurred: " + e.Message);
				return 1;
			}

			int vid = 0xfffe; 
			int pid = 0x7; 

			System.Console.WriteLine("Checking for VID PID...");

			libUSB_Interface.usb_device fdev = HPSDR_USB_LIB_V1.USB.FindDevice(bus, vid, pid);
			if (fdev != null)
				System.Console.WriteLine("Found VID PID: " + vid.ToString("x") + " " + pid.ToString("x"));
			else
			{
				System.Console.WriteLine("did not find VID PID: " + vid.ToString("x") + " " + pid.ToString("x"));
				return 1;
			}
			return 1; 
		}
#endif
		 						  
		unsafe public static int StartAudio(int sample_rate, int samples_per_block, PA19.PaStreamCallback cb, int sample_bits, int no_send) 
		{ 
			if ( !isOzyInitialized ) 
			{ 
				// int rc; 
				// oz_start(); 
				// rc = JanusOzyIO.getJanusOzy().init(); 				
				// System.Console.WriteLine("JanusOzyIO.init returned: " + rc); 
				
				// if ( rc != 0 ) 
				// { 
				//	return 1; 
				// }


  				if ( initOzy() != 0 )  
				{ 
					return 1; 
				} 
				isOzyInitialized = true; 
			} 
			return StartAudioNative(sample_rate, samples_per_block, cb, sample_bits, no_send); 
		} 


		[DllImport("JanusAudio.dll")]
		unsafe public static extern int StartAudioNative(int sample_rate, int samples_per_block, PA19.PaStreamCallback cb, int sample_bits, int no_send); 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int StopAudio(); 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int GetDotDash();  // bit 0 = dot asserted, bit 1 = dash asserted 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetXmitBit(int xmitbit);  // bit xmitbit ==0, recv mode, != 0, xmit mode

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int GetDiagData(int *a, int count);  // get diag data, count is how many slots are in array 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetVFOfreq(double f);  // tell aux hardware current freq -- in MHz 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern IntPtr OzyOpen();

		[DllImport("JanusAudio.dll")]
		unsafe public static extern void OzyClose(IntPtr ozyh);

		[DllImport("JanusAudio.dll")]
		unsafe public static extern IntPtr OzyHandleToRealHandle(IntPtr ozh);

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int IsOzyAttached(); 


		// return true if ozy vid/pid found on usb bus .. native code does all the real work 
		unsafe static bool isOzyAttached() 
		{ 
			int rc; 
			rc = IsOzyAttached(); 
			if ( rc == 0 ) 
			{ 
				return false; 
			}
			/* else */ 
			return true;
		} 


//		public static bool CWptt() 
//		{ 			
//			if ( ( GetDotDash() & 0x3 ) != 0  ) 
//			{
//				return true; 
//			}
//			/* else */ 
//			return false; 									 									 
//		} 
	}
}
