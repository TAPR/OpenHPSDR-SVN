// Copyright (C) 2006 Bill Tracey, KD5TFD (bill@ewwjt.com) 
// 
// Quick test driver to test DDS tuning via OZY Bit bang 
// Most of the tuning logic lifted from PowerSDR hardware_v2.cs 
// 

// This needs the GPIO_Control FPGA image from N8BV's Ozy tree. 
// to be loaded into the Ozy. 

/*
 * 
 * 
 * HPSDR/OZY - High Performance Software Defined Radio, OZY_USB_LIB_V1
 *
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

// This program is a command line utility to write the I2C bus through the FX2.
// It finds the FX2 by VID and PID, It writes only 1 byte for now.

using System;
using System.Collections.Generic;
using System.Text;
using HPSDR_USB_LIB_V1;
using System.Globalization;
using System.Threading;

namespace DDSTuneTest
{
    class Program
    {
        // PIO Register Address
        private const int PIO_IC1 = 0x2;
        private const int PIO_IC3 = 0x1;
        private const int PIO_IC8 = 0x8;
        private const int PIO_IC11 = 0x4;
        private const int PIO_NONE = 0x0;

        // DDS Variables
        private const byte DDSWRB = 0x40;
        private const byte DDSRESET = 0x80;
        private const byte COMP_PD = 0x10;		// DDS Comparator power down
        private const byte BYPASS_PLL = 0x20;		// Bypass DDS PLL
        private const byte BYPASS_SINC = 0x40;

        private const int ADDR_DATA = 0x1; 
        private const int ADDR_CTRL = 0x2; 


        private const double REFCLK_HZ = 200000000.0; 

        // latch data into one of the PIO registers. 
        // addr needs to be on of the PIO_xxx constants 
        // returns true on success, false on failure         
        private bool LatchRegister(byte addr, byte data)
        {
            bool rc; 
            byte[] buf = new byte[1]; 
            buf[0] = data; 
            rc = OZY.Write_SPI(usb_dev_handle, 0, ADDR_DATA, OZY.SPI_EN_FPGA, (OZY.SPI_FMT_MSB | OZY.SPI_FMT_HDR_1), buf); 
            if ( !rc ) { 
                System.Console.WriteLine("Latch to ADDR_DATA failed\n"); 
                return false;
            } 
            buf[0] = addr; 
            rc = OZY.Write_SPI(usb_dev_handle, 0, ADDR_CTRL, OZY.SPI_EN_FPGA, (OZY.SPI_FMT_MSB | OZY.SPI_FMT_HDR_1), buf); 
            if ( !rc ) { 
                System.Console.WriteLine("Latch to ADDR_CTRL failed\n"); 
                return false;
            }                 
            buf[0] = PIO_NONE; 
            rc = OZY.Write_SPI(usb_dev_handle, 0, ADDR_CTRL, OZY.SPI_EN_FPGA, (OZY.SPI_FMT_MSB | OZY.SPI_FMT_HDR_1), buf); 
            if ( !rc ) { 
                System.Console.WriteLine("Latch to ADDR_CTRL #2 failed\n"); 
                return false;
            }                 
            return true;
        }

        private bool ResetDDS()
        {
            bool rc; 
            rc = LatchRegister(PIO_IC8, DDSRESET | DDSWRB);
            if ( !rc ) return rc; 
            rc = LatchRegister(PIO_IC8, DDSWRB);
            if ( !rc ) return rc;
            rc = DDSWrite(COMP_PD, 0x1d);
            if ( !rc ) return rc;
            rc = DDSWrite(BYPASS_PLL, 0x1e);
            if ( !rc ) return rc; 
            rc = DDSWrite(BYPASS_SINC, 0x20); 
            return rc; 
        }


        private bool DDSTune(long tuning_word)
        {
            bool rc; 
            for (int i = 0; i < 6; i++)
            {
                byte b = (byte)( tuning_word >> (40-i*8)); 
                rc = DDSWrite(b, (byte)(4+i));
                if ( !rc ) return rc; 
            } 
            return true; 
        }

        private bool DDSWrite(byte data, byte addr)
		{

            bool rc; 
			//Set up data bits
			rc = LatchRegister(PIO_IC11, data);
            if ( !rc ) return rc; 
                        		
			//Set up address bits with WRB high
			rc = LatchRegister(PIO_IC8, (byte)(addr | DDSWRB));
            if ( !rc ) return rc; 
            
			//Send write command with WRB low
			rc = LatchRegister(PIO_IC8, addr);
            if ( !rc ) return rc; 
            
			//Return WRB high
			LatchRegister(PIO_IC8, DDSWRB);
            return rc; 
		}


		
        private long FreqToTuningWord(int f_in_hz) { 
            double twd = (f_in_hz / REFCLK_HZ) * Math.Pow(2,48); 
            return (long)twd;             
        }



        private static readonly byte VRQ_SDR1K_CTL = 0x0d;
        private static readonly byte SDR1KCTRL_SET_DATA_REG = 0x1;
        private static readonly byte SDR1KCTRL_LATCH = 0x2;
        private static readonly byte SDR1KCTRL_DDS_RESET = 0x3;
        private static readonly byte SDR1KCTRL_DDS_WRITE = 0x4;
        private static readonly byte SDR1KCTRL_SR_LOAD = 0x5;
        private static readonly byte SDR1KCTRL_READ_STATUS = 0x6;
        private static readonly byte VRT_VENDOR_IN = 0xC0;
        private static readonly byte VRT_VENDOR_OUT = 0x40; 


        public static readonly byte LATCH_EXT = 0x1;
        public static readonly byte LATCH_BPF = 0x2;
        public static readonly byte LATCH_DAT = 0x4;
        public static readonly byte LATCH_ADR = 0x8;



        int writeDataReg(IntPtr hdev, byte val)
        {
            int ret = libUSB_Interface.usb_control_msg(hdev, 
                                                       VRT_VENDOR_OUT, 
                                                       VRQ_SDR1K_CTL, 
                                                       SDR1KCTRL_SET_DATA_REG, 
                                                       val, 
                                                       null, 
                                                       0,
                                                       1000);
            System.Console.WriteLine("writeDataReg ret=" + ret); 
            return ret;                                     
        }


        bool writeDataSPI(IntPtr hdev, byte val)
        {
            bool rc; 
            byte[] buf = new byte[1];
            buf[0] = val;
            rc = OZY.Write_SPI(usb_dev_handle, 0, ADDR_DATA, OZY.SPI_EN_FPGA, ( OZY.SPI_FMT_MSB | OZY.SPI_FMT_HDR_1 ), buf);
            Console.WriteLine("WriteSPI rc: " + rc);            
            return rc; 
        }


        int LatchRegisterFX2(IntPtr hdev, byte latch, byte val)
        {

            int rc = libUSB_Interface.usb_control_msg(hdev,
                                                  VRT_VENDOR_OUT,
                                                  VRQ_SDR1K_CTL,
                                                  SDR1KCTRL_LATCH,
                                                  ( latch << 8 ) | val,
                                                  null,
                                                  0,
                                                  1000);
            System.Console.WriteLine("latch: usb_control_msg rc=" + rc);
            return rc; 		
        }


        static byte[] AllLatches = new byte[] { LATCH_EXT, LATCH_BPF, LATCH_DAT, LATCH_ADR };


        void LatchEmAll(IntPtr hdev, byte val)
        {
            int rc; 
            for ( int i = 0; i <  AllLatches.Length ; i++ ) { 
                rc = LatchRegisterFX2(hdev, AllLatches[i], val); 
                Console.WriteLine("Latch: " + i + " val: " + val + " rc: " + rc); 
                Thread.Sleep(10); 
            }
            return;
        }

        int readStatus(IntPtr hdev)
        {
            byte[] buf = new byte[1];

            int rc = libUSB_Interface.usb_control_msg(hdev,
                VRT_VENDOR_IN,
                VRQ_SDR1K_CTL,
                SDR1KCTRL_READ_STATUS,
                0,
                buf,
                buf.Length,
                1000);
            if (rc == buf.Length)
            {
                rc = (int)( (uint)( buf[0] ) );
                // ++read_ok_count;
            }
            else
            {
                rc = -1;
                // ++read_notok_count;

            }
            // Thread.Sleep(10);             
            return rc; 	
        }

        static void Main(string[] args)
        {
            Program p = new Program(args);
            p.run(); 
        }

        // instance vars 
        string[] Args; 
        int vid; 
        int pid; 
        IntPtr usb_dev_handle; 

        public  Program(string[] args) { 
            Args = args; 
        }

        int readStatusSPI(IntPtr usb_dev_handle)
        {
            int rc; 
            byte[] buf = new byte[1];

            buf[0] = 0x00;
            byte addr =  0x43;

            if (( OZY.Read_SPI(usb_dev_handle, 0, addr, OZY.SPI_EN_FPGA, OZY.SPI_FMT_MSB | OZY.SPI_FMT_HDR_1, ref buf) ))
            {
                Console.WriteLine("Read from address " + addr.ToString("X") + " : " + buf[0].ToString("X"));
                Console.WriteLine("");
                rc = buf[0]; 
            }
            else
            {
                Console.WriteLine("Failed to read address " + addr);
                rc = -1; 
            }
            return rc; 
        }


        public void run() { 
            libUSB_Interface.usb_bus bus;
            vid = 0xfffe;
            pid = 0x7; 

            try
            {
                libUSB_Interface.usb_init();
                Console.WriteLine("finding busses...");
                libUSB_Interface.usb_find_busses();
                Console.WriteLine("finding devices...");
                libUSB_Interface.usb_find_devices();
                Console.WriteLine("usb_get_busses...");
                bus = libUSB_Interface.usb_get_busses();
                Console.WriteLine("bus location: " + bus.location.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return;
            }

            Console.WriteLine("Checking for VID PID...");
            libUSB_Interface.usb_device fdev = USB.FindDevice(bus, vid, pid);
            if (fdev != null)
                Console.WriteLine("Found VID PID: " + vid.ToString("x") + " " + pid.ToString("x"));
            else
            {
                Console.WriteLine("did not find VID PID: " + vid.ToString("x") + " " + pid.ToString("x"));
                return;
            }

            Console.WriteLine("Trying to open device...");

            // IntPtr usb_dev_handle;

            try
            {
                usb_dev_handle = libUSB_Interface.usb_open(fdev);
                Console.WriteLine("Device handle is: " + usb_dev_handle.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return;
            }

            while (true)
            {

                int status; 
#if false 
                Console.WriteLine("none on"); 
                LatchEmAll(usb_dev_handle, 0x0);
                status = readStatus(usb_dev_handle); 
                Console.WriteLine("Status: " + status); 
                Thread.Sleep(2000);
                Console.WriteLine("half on");
                LatchEmAll(usb_dev_handle, 0x55);
                status = readStatus(usb_dev_handle); 
                Console.WriteLine("Status: " + status); 
                Thread.Sleep(2000);
                Console.WriteLine("other half on");                
                LatchEmAll(usb_dev_handle, 0xaa);
                status = readStatus(usb_dev_handle); 
                Console.WriteLine("Status: " + status); 
                Thread.Sleep(2000);
                Console.WriteLine("all on");
                LatchEmAll(usb_dev_handle, 0xff);
                status = readStatus(usb_dev_handle); 
                Console.WriteLine("Status: " + status); 
                Thread.Sleep(2000);                 
#endif

#if false 
                int rc; 
                for ( int i = 0; i < 64; i++ ) { 
                    Console.WriteLine("i: " + i + " (0x" + i.ToString("X") + ")");
                    writeDataSPI(usb_dev_handle, (byte)i);
                    Thread.Sleep(1000); 
                    // rc = libUSB_Interface.usb_clear_halt(usb_dev_handle, 0); 
                    // Console.WriteLine("clear_halt rc=" + rc); 
                    status = readStatusSPI(usb_dev_handle);
                    Console.WriteLine("Status: " + status.ToString("X") + " i: " + i.ToString("X"));
                    Console.WriteLine(); 
                } 
#endif 


#if true

                Console.WriteLine("all on");
                // rc = libUSB_Interface.usb_clear_halt(usb_dev_handle, 0);
                // Console.WriteLine("clear_halt rc=" + rc); 
                writeDataReg(usb_dev_handle, 0xff);
                // writeDataSPI(usb_dev_handle, 0xff);
                Thread.Sleep(1000);
                // rc = libUSB_Interface.usb_clear_halt(usb_dev_handle, 0);
                //Console.WriteLine("clear_halt rc=" + rc); 
                status = readStatus(usb_dev_handle);
                Console.WriteLine("Status: " + status.ToString("X"));
                
#endif

#if false
                int rc; 
                Console.WriteLine("none on"); 
                // writeDataSPI(usb_dev_handle, 0x00);
                writeDataReg(usb_dev_handle, 0x0);
                Thread.Sleep(1000); 
                // rc = libUSB_Interface.usb_clear_halt(usb_dev_handle, 0); 
                // Console.WriteLine("clear_halt rc=" + rc); 
                status = readStatus(usb_dev_handle);
                Console.WriteLine("Status: " + status.ToString("X") );
                status = readStatus(usb_dev_handle);
                Console.WriteLine("Status: " + status.ToString("X")); 
                Thread.Sleep(2000);
                Console.WriteLine("half on");
                // rc = libUSB_Interface.usb_clear_halt(usb_dev_handle, 0);
                // Console.WriteLine("clear_halt rc=" + rc); 
                // writeDataSPI(usb_dev_handle, 0x55);
                writeDataReg(usb_dev_handle, 0x55);
                Thread.Sleep(1000);
                // rc = libUSB_Interface.usb_clear_halt(usb_dev_handle, 0);
                // Console.WriteLine("clear_halt rc=" + rc); 
                status = readStatus(usb_dev_handle);
                Console.WriteLine("Status: " + status.ToString("X"));
                status = readStatus(usb_dev_handle);
                Console.WriteLine("Status: " + status.ToString("X")); 
                Thread.Sleep(2000);
                Console.WriteLine("other half on");
                // rc = libUSB_Interface.usb_clear_halt(usb_dev_handle, 0);
                // Console.WriteLine("clear_halt rc=" + rc); 
                writeDataReg(usb_dev_handle, 0xaa);
                // writeDataSPI(usb_dev_handle, 0xaa);
                Thread.Sleep(1000);
                // rc = libUSB_Interface.usb_clear_halt(usb_dev_handle, 0);
                // Console.WriteLine("clear_halt rc=" + rc); 
                status = readStatus(usb_dev_handle);
                Console.WriteLine("Status: " + status.ToString("X"));
                status = readStatus(usb_dev_handle);
                Console.WriteLine("Status: " + status.ToString("X")); 
                Thread.Sleep(2000);
                Console.WriteLine("all on");
                // rc = libUSB_Interface.usb_clear_halt(usb_dev_handle, 0);
                // Console.WriteLine("clear_halt rc=" + rc); 
                writeDataReg(usb_dev_handle, 0xff);
                // writeDataSPI(usb_dev_handle, 0xff);
                Thread.Sleep(1000);
                // rc = libUSB_Interface.usb_clear_halt(usb_dev_handle, 0);
                //Console.WriteLine("clear_halt rc=" + rc); 
                status = readStatus(usb_dev_handle);
                Console.WriteLine("Status: " + status.ToString("X"));
                status = readStatus(usb_dev_handle);
                Console.WriteLine("Status: " + status.ToString("X")); 
                Thread.Sleep(2000); 
#endif 
            }
        }
    }
}

