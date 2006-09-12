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

            bool rc; 
            rc = ResetDDS(); 
            if ( !rc ) 
            { 
                System.Console.WriteLine("ResetDDS failed\n"); 
                return; 
            } 

            while ( true ) { 

#if false 
                // this blob of code alternates all 0's, 1's, 10101..., 0101... to 
                // IC1 on the PIO board -- can be viewed at JP2 of the PIO board 
                // 
                Console.WriteLine("Latching all 0's"); 
                LatchRegister(PIO_IC1, 0); 
                Thread.Sleep(10000); 
                Console.WriteLine("Latching all 1's"); 
                LatchRegister(PIO_IC1, 0x7f); 
                Thread.Sleep(10000);
                Console.WriteLine("Latching all 1010101");
                LatchRegister(PIO_IC1, 0x55);
                Thread.Sleep(10000);
                Console.WriteLine("Latching all 0101010");
                LatchRegister(PIO_IC1, 0x2a);
                Thread.Sleep(10000); 
#endif



#if true
                Console.WriteLine("----------------------------------------------- Tuning 10 Mhz");
                long tuning_word = FreqToTuningWord(10000000); 
                DDSTune(tuning_word); 
                Thread.Sleep(10000);
                Console.WriteLine("----------------------------------------------- Tuning 10 000 010 hz");
                tuning_word = FreqToTuningWord(10000010);
                DDSTune(tuning_word); 
                Thread.Sleep(10000); 
                Console.WriteLine("----------------------------------------------- Tuning 5 Mhz");
                tuning_word = FreqToTuningWord(5000000);
                DDSTune(tuning_word); 
                Thread.Sleep(10000); 
#endif 
            } 




#if false
            Console.WriteLine("Closing device...");
            libUSB_Interface.usb_close(usb_dev_handle);
            Console.WriteLine("done...");
#endif 
        }
    }
}

