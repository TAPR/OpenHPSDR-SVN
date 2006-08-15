/*
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

// This program is a command line utility to write the I2C EEPROM through the FX2.
// It finds the FX2 by VID and PID
// It uses the write_i2c methods.
// Right now this just writes 1 byte at a time  

using System;
using System.Collections;
using System.Text;
using HPSDR_USB_LIB_V1;
using System.Globalization;
using System.IO;

namespace upload_FPGA
{
    class Program
    {
        static void Main(string[] args)
        {
            if ((args.Length != 3) || (args.Length == 0))
            {
                Console.WriteLine("usage: upload_FPGA <VID> <PID> <filename>");
                return;
            }

            if (args[0].Length > 2)
            {
                if (args[0].Substring(0, 2) != "0x")
                {
                    Console.WriteLine("You must specify VID in Hex (0x0)");
                    return;
                }
                else
                {
                    args[0] = args[0].Substring(2);
                }
            }
            else
            {
                Console.WriteLine("You must specify VID in Hex (0x0)");
                return;
            }


            int vid = int.Parse(args[0], NumberStyles.HexNumber);

            if (args[1].Length > 2)
            {
                if (args[1].Substring(0, 2) != "0x")
                {
                    Console.WriteLine("You must specify PID in Hex (0x0)");
                    return;
                }
                else
                    args[1] = args[1].Substring(2);
            }
            else
            {
                Console.WriteLine("You must specify PID in Hex (0x0)");
                return;
            }

            int pid = int.Parse(args[1], NumberStyles.HexNumber);

            string filename = args[2];

            if (!(File.Exists(filename)))
            {
                Console.WriteLine(filename + " does not exist!");
                return;
            }

            libUSB_Interface.usb_bus bus;

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

            IntPtr usb_dev_handle;

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

            if (!(OZY.Load_FPGA(usb_dev_handle, filename)))
            {
                Console.WriteLine("FPGA LOAD FAILED!");
            }
            else
                Console.WriteLine("FPGA LOAD SUCCEEDED!");

            Console.WriteLine("Closing device...");
            libUSB_Interface.usb_close(usb_dev_handle);
            Console.WriteLine("done...");
        }
    }
}

