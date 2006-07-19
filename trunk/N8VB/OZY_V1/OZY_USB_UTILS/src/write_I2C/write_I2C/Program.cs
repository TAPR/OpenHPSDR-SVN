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

// This program is a command line utility to write the I2C bus through the FX2.
// It finds the FX2 by VID and PID, It writes only 1 byte for now.

using System;
using System.Collections.Generic;
using System.Text;
using HPSDR_USB_LIB_V1;
using System.Globalization;

namespace write_I2C
{
    class Program
    {
        static void Main(string[] args)
        {
            if ((args.Length != 7) || (args.Length == 0))
            {
                Console.WriteLine("usage: write_I2C <VID> <PID> <i2c_address in hex> <value1 in hex> <value2 in hex> <value3 in hex> <count in dec (1-3)>");
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

            if (args[2].Length > 2)
            {
                if (args[2].Substring(0, 2) != "0x")
                {
                    Console.WriteLine("You must specify i2c_addr in Hex (0x0)");
                    return;
                }
                else
                    args[2] = args[2].Substring(2);
            }
            else
            {
                Console.WriteLine("You must specify i2c_addr in Hex (0x0)");
                return;
            }

            int i2c_addr = int.Parse(args[2], NumberStyles.HexNumber);

            if (args[3].Length > 2)
            {
                if (args[3].Substring(0, 2) != "0x")
                {
                    Console.WriteLine("You must specify i2c_addr in Hex (0x0)");
                    return;
                }
                else
                    args[3] = args[3].Substring(2);
            }
            else
            {
                Console.WriteLine("You must specify value1 in Hex (0x0)");
                return;
            }
            
            int value1 = int.Parse(args[3], NumberStyles.HexNumber);

            if (args[4].Length > 2)
            {
                if (args[4].Substring(0, 2) != "0x")
                {
                    Console.WriteLine("You must specify value2 in Hex (0x0)");
                    return;
                }
                else
                    args[4] = args[4].Substring(2);
            }
            else
            {
                Console.WriteLine("You must specify value2 in Hex (0x0)");
                return;
            }

            int value2 = int.Parse(args[4], NumberStyles.HexNumber);

            if (args[5].Length > 2)
            {
                if (args[5].Substring(0, 2) != "0x")
                {
                    Console.WriteLine("You must specify value3 in Hex (0x0)");
                    return;
                }
                else
                    args[5] = args[5].Substring(2);
            }
            else
            {
                Console.WriteLine("You must specify value3 in Hex (0x0)");
                return;
            }

            int value3 = int.Parse(args[5], NumberStyles.HexNumber);
            int count = int.Parse(args[6], NumberStyles.HexNumber);

            if ((count < 1) || (count > 3))
            {
                Console.WriteLine("count must be 1, 2 or 3)");
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

            byte[] buf = new byte[count];

            switch (count)
            {
                case 1:
                    buf[0] = (byte)value1;
                    break;
                case 2:
                    buf[0] = (byte)value1;
                    buf[1] = (byte)value2;
                    break;
                case 3:
                    buf[0] = (byte)value1;
                    buf[1] = (byte)value2;
                    buf[2] = (byte)value3;
                    break;
                default:
                    Console.WriteLine("An undefined error occurred");
                    libUSB_Interface.usb_close(usb_dev_handle);
                    return;
            }            

            if ((OZY.Write_I2C(usb_dev_handle, i2c_addr, buf)))
            {
                Console.WriteLine("Wrote to address: " + i2c_addr);
            }
            else
            {
                Console.WriteLine("Failed to write address: " + i2c_addr);
            }
            Console.WriteLine("Closing device...");
            libUSB_Interface.usb_close(usb_dev_handle);
            Console.WriteLine("done...");
        }
    }
}
