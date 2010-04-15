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
using System.Threading;
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


		public static string setC1Opts(string opt) 
		{ 
			int bits; 
			int off_mask = 0xff;
			int on_mask = 0; 

			string result = null; 

			switch ( opt ) 
			{ 
				case "--Atlas10MHz": 
					off_mask =  0xf3;  // 11110011
					on_mask = 0;       // 10 meg atlas == 00xx
					result= "Atlas10"; 
					break; 


				case "--Penny10MHz": 
					off_mask =  0xf3;  // 11110011
					on_mask = 0x4;      // 10 meg penny == 01xx 
					result= "Penny10"; 
					break; 

				case "--Mercury10Mhz": 
					off_mask =  0xf3;  // 11110011
					on_mask = 0x8;      // 10 meg merc == 10xx 
					result = "Merc10"; 
					break; 

				case "--Mercury125MHz":
					off_mask =  0xef;     // 11101111
					on_mask = 0x10;
					result = "Merc125";
					break; 

				case "--CfgPenny": 
					off_mask =  0x9f;     // 10011111
					on_mask = 0x20;
					result = "CfgPenny";
					break; 

				case "--CfgMercury": 
					off_mask =  0x9f;     // 10011111
					on_mask = 0x40;
					result = "CfgMerc";
					break; 


				case "--CfgBoth": 				
					off_mask =  0x9f;     // 10011111
					on_mask = 0x60;
					result = "CfgBoth";
					break; 

				case "--PennyMic": 
					off_mask =  0x7f;     // 01111111
					on_mask = 0x80;
					result = "PennyMic";
					break; 
			} 

			bits = JanusAudio.GetC1Bits(); 
			bits &= off_mask; 
			bits |= on_mask; 
			JanusAudio.SetC1Bits(bits); 

			return result; 
		} 


		private static string fx2_fw_version = "n/a"; 

		public static string  getFX2FirmwareVersionString() 
		{ 
			return fx2_fw_version; 
		} 


		[DllImport("JanusAudio.dll")]
		unsafe public static extern void EnableHermesPower(int enable);

		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetOutputPowerFactor(int i);

		public static void SetOutputPower(float f) 
		{ 
			if ( f < 0.0 ) 
			{ 
				f = 0.0F; 
			} 
			if ( f >= 1.0 ) 
			{ 
				f = 1.0F; 
			} 

			int i = (int)(255*f); 
			SetOutputPowerFactor(i); 
		} 

		// returns 0 on success, !0 on failure 
		public static int initOzy()  
		{

			if ( !isOzyAttached() ) 
			{ 
				return 1; 
			} 

			string oz_fw_version = getOzyFirmwareString(); 

			if ( oz_fw_version == null )  // firmware not loaded -- load it
			{ 
				ProcessStartInfo start_info = new ProcessStartInfo(); 
				start_info.FileName = "initozy11.bat"; 
				Console c = Console.getConsole();
				if ( c != null && c.CurrentModel != Model.HPSDR )  
				{ 
					start_info.FileName = "initozy-janus.bat";
				} 
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

			fx2_fw_version = oz_fw_version; 

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

		private static bool fwVersionsChecked = false; 
		private static string fwVersionMsg = null; 

		public static string getFWVersionErrorMsg() 
		{ 
			return fwVersionMsg; 
		} 
		 
		public static bool forceFWGood = false;

		private static bool legacyDotDashPTT = false;

		// checks if the firmware versions are consistent - returns false if they are not 
		// and set fwVersionmsg to point to an appropriate message
		private static bool fwVersionsGood() 
		{ 
			bool result = true; 
			int ozy_ver = getOzyFWVersion(); 
			if ( ozy_ver <= 15 ) 
			{ 
				legacyDotDashPTT = true;
				JanusAudio.SetLegacyDotDashPTT(1);
			} 

			int merc_ver = getMercuryFWVersion();
			int penny_ver = getPenelopeFWVersion(); 
			string fx2_version_string = getFX2FirmwareVersionString(); 
			Console c = Console.getConsole(); 
			System.Console.WriteLine("fx2: " + fx2_version_string); 
			System.Console.WriteLine("ozy: " + ozy_ver); 
			System.Console.WriteLine("merc: " + merc_ver); 
			System.Console.WriteLine("penny: " + penny_ver); 
			if ( forceFWGood == true ) 
			{ 
				System.Console.WriteLine("Firmware ver check forced good!"); 
				return true; 
			} 

			if ( fx2_version_string.CompareTo("20090524") >= 0 ) 
			{ 
				if ( (ozy_ver < 13) || 
					 ( c != null && c.MercuryPresent && (merc_ver < 27) ) || 
					 ( c != null && c.PennyPresent && (penny_ver < 12) )
			       ) 
				{ 
					result = false; 
					fwVersionMsg = "Invalid Firmware Level.  Ozy >= 1.6, Penelope >= 1.2 and Mercury >= 2.7 required\n"; 
				}								
			} 
			else  
			{
				if (  ( ozy_ver > 12 )  || 
					( c != null && c.MercuryPresent && (merc_ver > 26) ) || 
					( c != null && c.PennyPresent && (penny_ver > 11) )
					) 
				{
					result = false; 
					fwVersionMsg = "Invalid Firmware Level.  Ozy < 1.3, Penelope < 1.2 and Mercury < 2.7 required\n"; 
				}				
		    }						
			return result; 
		} 

		 

		// returns -101 for firmware version error 
		unsafe public static int StartAudio(int sample_rate, int samples_per_block, PA19.PaStreamCallback cb, int sample_bits, int no_send) 
		{ 			
			if ( initOzy() != 0 )  
			{ 
				return 1; 
			}	 
			int result = StartAudioNative(sample_rate, samples_per_block, cb, sample_bits, no_send); 
			if ( result == 0 && !fwVersionsChecked ) 
			{ 
				Thread.Sleep(100); // wait for frames 
				if ( !fwVersionsGood() ) 
				{
					result = -101; 
				} 
				else 
				{ 
					fwVersionsChecked = true; 
				} 
			} 
			return result; 
				
		} 


		[DllImport("JanusAudio.dll")]
		unsafe public static extern int StartAudioNative(int sample_rate, int samples_per_block, PA19.PaStreamCallback cb, int sample_bits, int no_send); 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int StopAudio(); 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetC1Bits(int bits); 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int GetC1Bits(); 


		[DllImport("JanusAudio.dll")]
		unsafe public static extern int nativeGetDotDashPTT();  // bit 0 = ptt, bit1 = dash asserted, bit 2 = dot asserted 
		unsafe public static int GetDotDashPTT() 
		{
			int bits = nativeGetDotDashPTT(); 
			if ( legacyDotDashPTT )  // old style dot and ptt overloaded on 0x1 bit, new style dot on 0x4, ptt on 0x1 
			{
				if ( (bits & 0x1) != 0 )  
				{ 
					bits |= 0x4; 
				} 
			}
			return bits;
		}

		
		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetLegacyDotDashPTT(int bit); 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetXmitBit(int xmitbit);  // bit xmitbit ==0, recv mode, != 0, xmit mode

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int GetDiagData(int *a, int count);  // get diag data, count is how many slots are in array 


		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetVFOfreq_native(double f);  // tell aux hardware current freq -- in MHz 


		public static void freqCorrectionChanged() 
		{
			Console c = Console.getConsole(); 
			if ( c != null ) 
			{ 
				if ( !c.FreqCalibrationRunning )    // we can't be applying freq correction when cal is running 
				{ 
					SetVFOfreq(lastVFOfreq); 
				}
			}
		}


		private static double lastVFOfreq = 0; 

		unsafe public static void SetVFOfreq(double f) 
		{ 
			lastVFOfreq = f; 
			Console c; 
			double correction_factor; 
			c = Console.getConsole(); 
			if ( c != null && c.SetupForm != null ) 
			{ 
				correction_factor = (double)c.SetupForm.HPSDRFreqCorrectFactor;
				// System.Console.WriteLine("correct_factor: " + correction_factor); 
			} 
			else 
			{ 
				correction_factor = 1.0d; 
			} 
			f = f * 1e6;  // mhz -> hz 
			f = (float)((double)f * correction_factor); 
			// System.Console.WriteLine("corrected freq: " + f); 
			SetVFOfreq_native(f); 
		} 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern IntPtr OzyOpen();

		[DllImport("JanusAudio.dll")]
		unsafe public static extern void OzyClose(IntPtr ozyh);

		[DllImport("JanusAudio.dll")]
		unsafe public static extern IntPtr OzyHandleToRealHandle(IntPtr ozh);

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int IsOzyAttached(); 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetAlexAtten(int bits); 
							  
		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetMercDither(int bits); 
							  
		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetMercRandom(int bits);
							  
		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetMercPreamp(int bits);
							  
		[DllImport("JanusAudio.dll")]
		unsafe public static extern int getAndResetADC_Overload(); 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int getMercuryFWVersion();

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int getPenelopeFWVersion();

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int getOzyFWVersion();

		


		[DllImport("JanusAudio.dll")]

		unsafe public static extern int getFwdPower();



		// 
		// compute fwd power from Penny based on count returned 
		// this conversion is a linear interpolation of values measured on an 
		// actual penny board 		
		// 
		public static float computeFwdPower() 
		{ 
			int power_int = JanusAudio.getFwdPower(); 
			double power_f = (double)power_int; 
			double result; 


			if ( power_int <= 2095 ) 
			{ 
				if ( power_int <= 874 ) 
				{ 
					if ( power_int <= 113 ) 
					{
						result = 0.0; 
					} 
					else  // > 113 
					{ 
						result = (power_f - 113.0) * 0.065703; 
					} 
				} 
				else  // > 874 
				{ 
					if ( power_int <= 1380 ) 
					{ 
						result = 50.0 + ((power_f - 874.0) * 0.098814); 
					} 
					else  // > 1380 
					{ 
						result = 100.0 + ((power_f - 1380.0) * 0.13986); 
					} 
				} 
			} 
			else  // > 2095 
			{ 
				if ( power_int <= 3038 ) 
				{ 
					if ( power_int <= 2615 ) 
					{ 
						result = 200.0 + ((power_f - 2095.0) * 0.192308); 
					} 
					else  // > 2615, <3038 
					{
						result = 300.0 + ((power_f - 2615.0) * 0.236407); 
					} 
				} 
				else  // > 3038 
				{ 
					result = 400.0 + ((power_f - 3038.0) * 0.243902); 
				} 
			} 

			result = result/1000;  //convert to watts 
			return (float)result; 
		} 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int getControlByteIn(int n); 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern  void SetFPGATestMode(int i);
							  
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

		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetPennyOCBits(int b);

		[DllImport("JanusAudio.dll")]
		unsafe public static extern void SetAlexAntBits(int rx_ant, int tx_ant, int rx_out); 

		[DllImport("JanusAudio.dll")]
		unsafe public static extern int GetEP4Data(char *bufp);


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
