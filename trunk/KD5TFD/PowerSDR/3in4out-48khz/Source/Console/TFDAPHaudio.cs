// Copyright (c) Bill Tracey (kd5tfd) 2006
// email: bill@ewjt.com
//
using System;
using System.Runtime.InteropServices;
using System.Diagnostics; 
using HPSDR_USB_LIB_V1;

namespace PowerSDR
{
	//
	// routines to access audio from kd5tfd/vk6aph fpga based audio 
	// 
	public class TFDAPHaudio
	{
		public TFDAPHaudio()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private static bool isOzyInitialized = false; 

		// returns 0 on success, !0 on failure 
		private static int initOzy()  
		{
			ProcessStartInfo start_info = new ProcessStartInfo(); 
			start_info.FileName = "initozy.bat"; 
			start_info.UseShellExecute = true; 
			Process p = new Process(); 
			p.StartInfo = start_info; 
			bool rc = p.Start(); 
			System.Console.WriteLine("start returned: " + rc); 
			p.WaitForExit(); 
			System.Console.WriteLine("OzyInit completes"); 

			return 0; 
		}

		
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
		 						  
		unsafe public static int StartAudio(int sample_rate, int samples_per_block, PA19.PaStreamCallback cb, int sample_bits) 
		{ 
			if ( !isOzyInitialized ) 
			{ 
				// oz_start(); 

				if ( initOzy() != 0 )  
				{ 
					return 1; 
				} 
				isOzyInitialized = true; 
			} 
			return StartAudioNative(sample_rate, samples_per_block, cb, sample_bits); 
		} 


		[DllImport("KD5TFD-VK6APH-Audio.dll")]
		unsafe public static extern int StartAudioNative(int sample_rate, int samples_per_block, PA19.PaStreamCallback cb, int sample_bits); 

		[DllImport("KD5TFD-VK6APH-Audio.dll")]
		unsafe public static extern int StopAudio(); 

		[DllImport("KD5TFD-VK6APH-Audio.dll")]
		unsafe public static extern int GetDotDash();  // bit 0 = dot asserted, bit 1 = dash asserted 

		[DllImport("KD5TFD-VK6APH-Audio.dll")]
		unsafe public static extern void SetXmitBit(int xmitbit);  // bit xmitbit ==0, recv mode, != 0, xmit mode

		[DllImport("KD5TFD-VK6APH-Audio.dll")]
		unsafe public static extern int GetDiagData(int *a, int count);  // get diag data, count is how many slots are in array 



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
