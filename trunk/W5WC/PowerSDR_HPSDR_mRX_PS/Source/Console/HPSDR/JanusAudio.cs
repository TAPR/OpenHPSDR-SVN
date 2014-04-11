/*
*
* Copyright (C) 2006 Bill Tracey, KD5TFD, bill@ewjt.com 
* Copyright (C) 2010-2013  Doug Wigley
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

namespace PowerSDR
{
    using System;
    using System.Threading;
    using System.Runtime.InteropServices;
    using System.Diagnostics;

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


        public static bool isFirmwareLoaded = false;

        //private static void dummy_to_remove_warning() // added to remove fallacious warning about variable never being used
        //{
        //    isFirmwareLoaded = isFirmwareLoaded;
        //}

        // get ozy firmware version string - 8 bytes.  returns 
        // null for error 
        private static string getOzyFirmwareString()
        {
            IntPtr oz_h = OzyOpen();

            if (oz_h == (IntPtr)0)
            {
                return null;
            }
            IntPtr usb_h = JanusAudio.OzyHandleToRealHandle(oz_h);
            if (usb_h == (IntPtr)0)
            {
                JanusAudio.OzyClose(oz_h);
                return null;
            }

            byte[] buf = new byte[8];
            // int rc = WriteControlMsg(usb_h, 
            //OzySDR1kControl.VRT_VENDOR_IN, //0xC0
            // OzySDR1kControl.VRQ_SDR1K_CTL, //0x0d
            // OzySDR1kControl.SDR1KCTRL_READ_VERSION, // 0x7
            // 0, buf, buf.Length, 1000); 

            int rc = GetOzyID(usb_h, buf, buf.Length);

            // System.Console.WriteLine("read version rc: " + rc); 

            string result = null;

            if (rc == 8)    // got length we expected 
            {
                char[] cbuf = new char[8];
                for (int i = 0; i < 8; i++)
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

            switch (opt)
            {
                case "--Atlas10MHz":
                    off_mask = 0xf3;  // 11110011
                    on_mask = 0;       // 10 meg atlas == 00xx
                    result = "Atlas10";
                    break;

                case "--Penny10MHz":
                    off_mask = 0xf3;  // 11110011
                    on_mask = 0x4;      // 10 meg penny == 01xx 
                    result = "Penny10";
                    break;

                case "--Mercury10Mhz":
                    off_mask = 0xf3;  // 11110011
                    on_mask = 0x8;      // 10 meg merc == 10xx 
                    result = "Merc10";
                    break;

                case "--Mercury125MHz":
                    off_mask = 0xef;     // 11101111
                    on_mask = 0x10;
                    result = "Merc125";
                    break;

                case "--CfgPenny":
                    off_mask = 0x9f;     // 10011111
                    on_mask = 0x20;
                    result = "CfgPenny";
                    break;

                case "--CfgMercury":
                    off_mask = 0x9f;     // 10011111
                    on_mask = 0x40;
                    result = "CfgMerc";
                    break;

                case "--CfgBoth":
                    off_mask = 0x9f;     // 10011111
                    on_mask = 0x60;
                    result = "CfgBoth";
                    break;

                case "--PennyMic":
                    off_mask = 0x7f;     // 01111111
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

        public static string getFX2FirmwareVersionString()
        {
            return fx2_fw_version;
        }

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void EnableHermesPower(int enable);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetOutputPowerFactor(int i);

        public static void SetOutputPower(float f)
        {
            if (f < 0.0)
            {
                f = 0.0F;
            }
            if (f >= 1.0)
            {
                f = 1.0F;
            }

            int i = (int)(255 * f);
            //System.Console.WriteLine("output power i: " + i); 
            SetOutputPowerFactor(i);
        }

        // [DllImport("JanusAudio.dll")]
        // public static extern int getNetworkAddrs(Int32[] addrs, Int32 count); 

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DeInitMetisSockets();

        [DllImport("JanusAudio.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int nativeInitMetis(String netaddr, bool dostatic);

        //		private static bool MetisInitialized = false;
        // returns 0 on success, !0 on failure 
        public static bool enableStaticIP = false;
        public static int initMetis()
        {
            //			if( MetisInitialized ) 
            //			{ 
            //				return 0; 
            //			} 
            int rc;
            System.Console.WriteLine("MetisNetIPAddr: " + Console.getConsole().MetisNetworkIPAddr);
            rc = nativeInitMetis(Console.getConsole().MetisNetworkIPAddr, enableStaticIP);
            //			if  ( rc == 0 ) 
            //			{
            //				MetisInitialized = true;
            //			}
            System.Console.WriteLine("nativeInitMetis returned: " + rc);
            return -rc;
        }

        public static int initOzy()
        {
            Console c = Console.getConsole();

            if (c != null && c.HPSDRisMetis)
            {
                return initMetis();
            }

            if (!isOzyAttached())
            {
                System.Console.WriteLine("Ozy not attached!!");
                return 1;
            }

            string oz_fw_version = getOzyFirmwareString();

            if (oz_fw_version == null)  // firmware not loaded -- load it
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

            if (oz_fw_version == null)
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

        public static bool fwVersionsChecked = false;
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
            // return true;
            bool result = true;
            Console c = Console.getConsole();
            int penny_ver = 0;
            int mercury_ver = 0;
            byte[] metis_ver = new byte[1];
            int mercury2_ver = 0;

           // if (c.CurrentHPSDRModel == HPSDRModel.ANAN100D) c.RX2PreampPresent = true;
            if (forceFWGood == true || c.CurrentModel == Model.HERMES)
            {
                System.Console.WriteLine("Firmware ver check forced good!");
                return true;
            }

            if (c != null && c.HPSDRisMetis)
            {
                GetMetisCodeVersion(metis_ver);

                if (c.PowerOn)
                {
                    byte metis_vernum = metis_ver[0];
                    mercury_ver = getMercuryFWVersion();

                    if (c.PennyPresent || c.PennyLanePresent)
                    {
                        do
                        {
                            // Thread.Sleep(500);
                            penny_ver = getPenelopeFWVersion();
                            if (penny_ver < 16 || penny_ver > 80)
                            {
                                Thread.Sleep(500);
                                penny_ver = getPenelopeFWVersion();
                                if (penny_ver > 0) break;
                                penny_ver = getPenelopeFWVersion();
                                if (penny_ver == 0) break;
                            }
                        }
                        while (penny_ver <= 15);
                    }

                    switch (metis_vernum)
                    {
                        case 16:
                        case 17:
                            if ((c != null && (c.PennyPresent || c.PennyLanePresent) && (penny_ver != 16)) ||
                                (c != null && c.MercuryPresent && (mercury_ver != 31 && mercury_ver != 71)))
                            {
                                result = false;
                                c.SetupForm.alex_fw_good = false;
                            }
                            break;
                        case 18:
                            if ((c != null && (c.PennyPresent || c.PennyLanePresent) && (penny_ver != 17)) ||
                                (c != null && c.MercuryPresent && (mercury_ver != 32)))
                            {
                                result = false;
                                c.SetupForm.alex_fw_good = false;
                            }
                            break;
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                            if ((c != null && (c.PennyPresent || c.PennyLanePresent) && (penny_ver != 17)) ||
                                (c != null && c.MercuryPresent && (mercury_ver != 33)))
                            {
                                result = false;
                                c.SetupForm.alex_fw_good = false;
                            }
                            break;
                        case 26:
                        case 27:
                        case 28:
                            if ((c != null && (c.PennyPresent || c.PennyLanePresent) && (penny_ver != 18)) ||
                                (c != null && c.MercuryPresent && (mercury_ver != 34)))
                            {
                                result = false;
                                c.SetupForm.alex_fw_good = false;
                            }
                            break;
                        default:
                            // fwVersionMsg = "Invalid Firmware Level.\nPowerSDR requires Mercury v3.1\nYou have version: " + mercury_ver.ToString("0\\.0");
                            result = false;
                            c.SetupForm.alex_fw_good = false;
                            break;
                    }

                    mercury2_ver = getMercury2FWVersion();
                    if (mercury2_ver == 0)
                    {
                        Thread.Sleep(300);
                        mercury2_ver = getMercury2FWVersion();
                    }
                    if (mercury2_ver > 32) //check if physical rx2 present
                        c.RX2PreampPresent = true;
                    else
                        c.RX2PreampPresent = false;

                    if (c.SetupForm.FirmwareBypass == true) result = true;

                    if (!result)
                        fwVersionMsg = "Invalid Firmware.\nYou have Metis: " + metis_ver[0].ToString("0\\.0") +
                                                       "\nMercury:" + mercury_ver.ToString("0\\.0") +
                                                       "\nPenny:" + penny_ver.ToString("0\\.0");
                }
                return result;
            }

            string fx2_version_string = getFX2FirmwareVersionString();
            //int merc_ver = 0;
            int ozy_ver = 0;
            // int merc2_ver = 0;
            //System.Console.WriteLine("fx2: " + fx2_version_string); 
            //System.Console.WriteLine("ozy: " + ozy_ver); 
            //System.Console.WriteLine("merc: " + merc_ver); 
            //System.Console.WriteLine("penny: " + penny_ver); 
            //  c.SetI2CSpeed();
            // Thread.Sleep(100);

            if (fx2_version_string.CompareTo("20090524") >= 0)
            {
                //   do
                for (int i = 0; i < 5; i++)
                {
                    ozy_ver = getOzyFWVersion();
                    if (ozy_ver > 17) break;
                    Thread.Sleep(100);
                }
                //  while (ozy_ver < 12);
                // Thread.Sleep(2000);
                if (c.MercuryPresent)
                {
                    //  do
                    //  for (int i = 0; i < 2; i++)
                    {
                        mercury_ver = getMercuryFWVersion();
                        // if (mercury_ver > 0) break;
                        Thread.Sleep(100);
                    }
                    mercury_ver = getMercuryFWVersion();
                    mercury2_ver = getMercury2FWVersion();
                }

                if (c.PennyPresent || c.PennyLanePresent)
                {
                    do
                    {
                        // Thread.Sleep(500);
                        penny_ver = getPenelopeFWVersion();
                        if (penny_ver < 11)
                        {
                            Thread.Sleep(500);
                            penny_ver = getPenelopeFWVersion();
                            if (penny_ver > 0) break;
                            penny_ver = getPenelopeFWVersion();
                            if (penny_ver == 0) break;
                        }
                    }
                    while (penny_ver <= 10);
                }
                switch (ozy_ver)
                {
                    case 18:
                        // if ((c != null && (c.PennyPresent || c.PennyLanePresent) && (penny_ver != 13)) ||
                        //      (c != null && c.MercuryPresent && (mercury_ver != 29)))
                        {
                            result = false;
                            c.SetupForm.alex_fw_good = false;
                        }
                        break;
                    case 19:
                        // if ((c != null && (c.PennyPresent || c.PennyLanePresent) && (penny_ver != 14)) ||
                        //      (c != null && c.MercuryPresent && (mercury_ver != 29)))
                        {
                            result = false;
                            c.SetupForm.alex_fw_good = false;
                        }
                        break;
                    case 20:
                        // if ((c != null && (c.PennyPresent || c.PennyLanePresent) && (penny_ver != 15)) ||
                        //    (c != null && c.MercuryPresent && (mercury_ver != 30)))
                        {
                            result = false;
                            c.SetupForm.alex_fw_good = false;
                        }
                        break;
                    case 21:
                        if ((c != null && (c.PennyPresent || c.PennyLanePresent) && (penny_ver != 16)) ||
                            (c != null && c.MercuryPresent && (mercury_ver != 31)))
                        {
                            result = false;
                            c.SetupForm.alex_fw_good = false;
                        }
                        break;
                    case 22:
                        if ((c != null && (c.PennyPresent || c.PennyLanePresent) && (penny_ver != 17)) ||
                            (c != null && c.MercuryPresent && (mercury_ver != 32 && mercury_ver != 33)))
                        {
                            result = false;
                            c.SetupForm.alex_fw_good = false;
                        }
                        break;
                    case 23:
                    case 24:
                        if ((c != null && (c.PennyPresent || c.PennyLanePresent) && (penny_ver != 17)) ||
                            (c != null && c.MercuryPresent && (mercury_ver != 33)))
                        {
                            result = false;
                            c.SetupForm.alex_fw_good = false;
                        }
                        break;
                    case 25:
                        if ((c != null && (c.PennyPresent || c.PennyLanePresent) && (penny_ver != 18)) ||
                           (c != null && c.MercuryPresent && (mercury_ver != 34)))
                        {
                            result = false;
                            c.SetupForm.alex_fw_good = false;
                        }
                        break;
                    default:
                        result = false;
                        c.SetupForm.alex_fw_good = false;
                        break;
                }

                mercury2_ver = getMercury2FWVersion();
                if (mercury2_ver == 0)
                {
                    Thread.Sleep(300);
                    mercury2_ver = getMercury2FWVersion();
                }
                if (mercury2_ver < 32) //check if physical rx2 present
                    c.RX2PreampPresent = false;
                else
                    c.RX2PreampPresent = true;

                if (c.SetupForm.FirmwareBypass == true) result = true;

                if (!result)
                    fwVersionMsg = "Invalid Firmware.\nYou have Ozy: " + ozy_ver.ToString("0\\.0") +
                                                           "\nMercury: " + mercury_ver.ToString("0\\.0") +
                                                             "\nPenny: " + penny_ver.ToString("0\\.0");
            }
            return result;
        }

        // returns -101 for firmware version error 
        unsafe public static int StartAudio(int sample_rate, int samples_per_block, PA19.PaStreamCallback cb, int sample_bits, int no_send)
        {
            if (initOzy() != 0)
            {
                return 1;
            }
            int result = StartAudioNative(sample_rate, samples_per_block, cb, sample_bits, no_send);

            if (result == 0 && !fwVersionsChecked)
            {
                Thread.Sleep(100); // wait for frames 
                if (!fwVersionsGood())
                {
                    result = -101;
                }
                else
                {
                    fwVersionsChecked = true;
                }
            }
            //  InitMic();
            return result;
        }

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetMetisIPAddr();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetMetisMACAddr(byte[] addr_bytes);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetMetisCodeVersion(byte[] addr_bytes);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetMetisBoardID(byte[] addr_bytes);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int StartAudioNative(int sample_rate, int samples_per_block, PA19.PaStreamCallback cb, int sample_bits, int no_send);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int StopAudio();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetC1Bits(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetAlexManEnable(int bit);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetAlexEnabled(int bit);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetAlexHPFBits(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetAlexLPFBits(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetAlexTRRelayBit(int bit);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetAlex2HPFBits(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetAlex2LPFBits(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void EnableApolloFilter(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void EnableApolloTuner(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void EnableApolloAutoTune(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void EnableEClassModulation(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetHermesFilter(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetUserOut0(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetUserOut1(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetUserOut2(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetUserOut3(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern bool getUserI01(); // TX Inhibit input sense

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern bool getUserI02();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern bool getUserI03();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern bool getUserI04();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)] // sets number of receivers
        unsafe public static extern void SetNRx(int nrx);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)] // sets full or half duplex
        unsafe public static extern void SetDuplex(int dupx);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int GetC1Bits();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int nativeGetDotDashPTT();  // bit 0 = ptt, bit1 = dash asserted, bit 2 = dot asserted 
        unsafe public static int GetDotDashPTT()
        {
            int bits = nativeGetDotDashPTT();
            if (legacyDotDashPTT)  // old style dot and ptt overloaded on 0x1 bit, new style dot on 0x4, ptt on 0x1 
            {
                if ((bits & 0x1) != 0)
                {
                    bits |= 0x4;
                }
            }
            return bits;
        }

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetLegacyDotDashPTT(int bit);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetXmitBit(int xmitbit);  // bit xmitbit ==0, recv mode, != 0, xmit mode

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int GetDiagData(int* a, int count);  // get diag data, count is how many slots are in array 

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetRX1VFOfreq(int f);  // tell aux hardware current freq -- in MHz 

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetRX2VFOfreq(int f);  // tell aux hardware current freq -- in MHz 

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetRX3VFOfreq(int f);  // tell aux hardware current freq -- in MHz 

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetRX4VFOfreq(int f);  // tell aux hardware current freq -- in MHz 

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetRX5VFOfreq(int f);  // tell aux hardware current freq -- in MHz 

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetRX6VFOfreq(int f);  // tell aux hardware current freq -- in MHz 

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetRX7VFOfreq(int f);  // tell aux hardware current freq -- in MHz 

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetTXVFOfreq(int f);  // tell aux hardware current freq -- in MHz 

        private static double freq_correction_factor = 1.0;
        public static double FreqCorrectionFactor
        {
            get { return freq_correction_factor; }
            set
            {
                freq_correction_factor = value;
                freqCorrectionChanged();
            }
        }

        public static void freqCorrectionChanged()
        {
            if (!Console.FreqCalibrationRunning)    // we can't be applying freq correction when cal is running 
            {
                SetVFOfreqRX1(lastVFORX1freq, true);
                SetVFOfreqRX2(lastVFORX2freq, true);
                SetVFOfreqRX3(lastVFORX3freq, true);
                SetVFOfreqRX4(lastVFORX4freq, true);
                SetVFOfreqRX5(lastVFORX5freq, true);
                SetVFOfreqTX(lastVFOTXfreq);
            }
        }

        private static double lastVFORX1freq = 0.0;
        unsafe public static void SetVFOfreqRX1(double f, bool offset)
        {
            lastVFORX1freq = f;
            int f_freq;
            f_freq = (int)((f * 1e6) * freq_correction_factor);
            SetRX1VFOfreq(f_freq); // center freq
        }

        private static double lastVFORX2freq = 0.0;
        unsafe public static void SetVFOfreqRX2(double f, bool offset)
        {
            lastVFORX2freq = f;
            int f_freq;
            f_freq = (int)((f * 1e6) * freq_correction_factor);
            SetRX2VFOfreq(f_freq);
        }

        private static double lastVFORX3freq = 0.0;
        unsafe public static void SetVFOfreqRX3(double f, bool offset)
        {
            lastVFORX3freq = f;
            int f_freq;
            if (offset)
                f_freq = (int)((f * 1e6 * freq_correction_factor) - low_freq_offset);
            else
                f_freq = (int)((f * 1e6) * freq_correction_factor);
            SetRX3VFOfreq(f_freq); // low frq
        }

        private static double lastVFORX4freq = 0.0;
        unsafe public static void SetVFOfreqRX4(double f, bool offset)
        {
            lastVFORX4freq = f;
            int f_freq;
            if (offset)
                f_freq = (int)((f * 1e6 * freq_correction_factor) + high_freq_offset);
            else
                f_freq = (int)((f * 1e6) * freq_correction_factor);
            SetRX4VFOfreq(f_freq); // high freq
        }

        private static double lastVFORX5freq = 0.0;
        unsafe public static void SetVFOfreqRX5(double f, bool offset)
        {
            lastVFORX5freq = f;
            int f_freq;
            f_freq = (int)((f * 1e6 * freq_correction_factor)); // + high_freq_offset);
            SetRX5VFOfreq(f_freq); // highest freq
        }

        unsafe public static void SetVFOfreqRX6(double f)
        {
            int f_freq = (int)((f * 1e6) + (high_freq_offset * 2.0));
            SetRX6VFOfreq(f_freq);
        }

        unsafe public static void SetVFOfreqRX7(double f)
        {
            int f_freq = (int)((f * 1e6) + (high_freq_offset * 3.0));
            SetRX7VFOfreq(f_freq);
        }

        private static double low_freq_offset;
        public static double LowFreqOffset
        {
            get { return low_freq_offset; }
            set
            {
                low_freq_offset = value;
            }
        }

        private static double high_freq_offset;
        public static double HighFreqOffset
        {
            get { return high_freq_offset; }
            set
            {
                high_freq_offset = value;
            }
        }

        private static double lastVFOTXfreq = 0.0;
        unsafe public static void SetVFOfreqTX(double f)
        {
            lastVFOTXfreq = f;
            int f_freq = (int)((f * 1e6) * freq_correction_factor);
            SetTXVFOfreq(f_freq);
            // c.SetupForm.txtTXVFO.Text = f_freq.ToString();
        }

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern IntPtr OzyOpen();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void OzyClose(IntPtr ozyh);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern IntPtr OzyHandleToRealHandle(IntPtr ozh);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int IsOzyAttached();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetMicBoost(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetLineIn(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetLineBoost(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetAlexAtten(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetMercDither(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetMercRandom(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetRX1Preamp(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetRX2Preamp(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void EnableADC1StepAtten(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void EnableADC2StepAtten(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void EnableADC3StepAtten(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetADC1StepAttenData(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetADC2StepAttenData(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetADC3StepAttenData(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetMicTR(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetMicBias(int bits);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getAndResetADC_Overload();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getMercuryFWVersion();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getMercury2FWVersion();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getMercury3FWVersion();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getMercury4FWVersion();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getPenelopeFWVersion();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getOzyFWVersion();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getHaveSync();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getFwdPower();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getRefPower();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getAlexFwdPower();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getHermesDCVoltage();

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetCWKeyer(int enable);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetCWSidetoneVolume(int vol);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetCWPTTDelay(int delay);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetCWHangTime(int hang);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetCWSidetoneFreq(int freq);

        // 
        // compute fwd power from Penny based on count returned 
        // this conversion is a linear interpolation of values measured on an 
        // actual penny board 		
        // 
    /*    public static float computeFwdPower()
        {
            int power_int = getFwdPower();
            double computed_result = computePower(power_int);
            return (float)computed_result;
        }

        public static float computeRefPower()
        {
            Console c = Console.getConsole();
            int adc = JanusAudio.getRefPower();
            if (adc < 300) adc = 0;
            float volts = (float)adc * (3.3f / 4095.0f);
            float watts = (float)(Math.Pow(volts, 2) / 0.095f);

            if (c != null && c.PAValues)
            {
                c.SetupForm.txtRevADCValue.Text = adc.ToString();
                c.SetupForm.txtRevVoltage.Text = volts.ToString();
              //  c.SetupForm.txtPARevPower.Text = watts.ToString();              
            } 

            return watts;
        }

        public static float computeAlexFwdPower()
        {
            Console c = Console.getConsole();
            int adc = JanusAudio.getAlexFwdPower();
            if (adc < 300) adc = 0;
            float volts = (float)adc * (3.3f / 4095.0f);
            float watts = (float)(Math.Pow(volts, 2) / 0.095f);

            if (c != null && c.PAValues)
            {
                c.SetupForm.txtFwdADCValue.Text = adc.ToString();
                c.SetupForm.txtFwdVoltage.Text = volts.ToString();
               // c.SetupForm.txtPAFwdPower.Text = watts.ToString();
            } 

            return watts;
        }

        public static float computePower(int power_int)
        {
            // Console c = Console.getConsole();
            double power_f = (double)power_int;
            double result = 0.0;

            if (power_int <= 2095)
            {
                if (power_int <= 874)
                {
                    if (power_int <= 113)
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
                    if (power_int <= 1380)
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
                if (power_int <= 3038)
                {
                    if (power_int <= 2615)
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

            result = result / 1000;  //convert to watts 
            // c.SetupForm.txtFwdPower.Text = result.ToString();
            // c.SetupForm.txtFwdADC.Text = power_int.ToString();

            return (float)result;
        } */

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getControlByteIn(int n);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetFPGATestMode(int i);

        // return true if ozy vid/pid found on usb bus .. native code does all the real work 
        unsafe static bool isOzyAttached()
        {
            int rc;
            rc = IsOzyAttached();
            if (rc == 0)
            {
                return false;
            }
            /* else */
            return true;
        }

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetDiscoveryMode(int b);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetPennyOCBits(int b);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetSWRProtect(float g);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetAlexAntBits(int rx_ant, int tx_ant, int rx_out);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int GetEP4Data(char* bufp);

        // Diversity
        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void EnableDiversity2(int g);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetMercSource(int g);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetrefMerc(int g);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIQ_Rotate(double a, double b);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIQ_RotateA(double a, double b);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIQ_RotateB(double a, double b);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetTheta(double a); 

        // Ozyutils
        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe extern public static int GetOzyID(IntPtr usb_h, byte[] bytes, int length);

        //[DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        // unsafe extern public static bool Write_I2C(IntPtr usb_h, int i2c_addr, byte[] bytes, int length);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe extern public static bool WriteI2C(IntPtr usb_h, int i2c_addr, byte[] bytes, int length);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe extern public static bool ReadI2C(IntPtr usb_h, int i2c_addr, byte[] bytes, int length);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe extern public static bool Set_I2C_Speed(IntPtr hdev, int speed);

        [DllImport("JanusAudio.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe extern public static int WriteControlMsg(IntPtr hdev, int requesttype, int request, int value,
                                              int index, byte[] bytes, int length, int timeout);
#if false
        [DllImport("JanusAudio.dll")]
        extern public static int LoadFirmware(int VID, int PID, String filename);

        [DllImport("JanusAudio.dll")]
        extern public static int LoadFPGA(int VID, int PID, String filename);

        [DllImport("JanusAudio.dll")]
        unsafe extern public static int ReadI2C(int i2c_addr, byte* buffer, int length);

        //[DllImport("JanusAudio.dll")]
        //unsafe extern public static int WriteI2C(IntPtr usb_h, int i2c_addr, byte* buffer, int length);

        [DllImport("JanusAudio.dll")]
        unsafe extern public static int ReadEEPROM(int i2c_addr, int offset, byte* buffer, int length);

        [DllImport("JanusAudio.dll")]
        unsafe extern public static int WriteEEPROM(int i2c_addr, int offset, byte* buffer, int length);

        public static int LoadOzyFirmware(string path, string filename)
        {
            string firmnamepath = path + "\\" + filename;

            if (File.Exists(firmnamepath))
            {
                return LoadFirmware(0xfffe, 0x7, firmnamepath);
            }
            else return -1;
        }

        public static int LoadOzyFPGA(string path, string filename)
        {
            string fpganamepath = path + "\\" + filename;

            if (File.Exists(fpganamepath))
            {
                return LoadFPGA(0xfffe, 0x7, fpganamepath);
            }
            else return -1;
        }
#endif
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
