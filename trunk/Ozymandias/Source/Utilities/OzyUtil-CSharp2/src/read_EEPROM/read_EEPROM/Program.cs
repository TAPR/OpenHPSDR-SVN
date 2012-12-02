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

// This program is a command line utility to read the I2C EEPROM through the FX2.
// It finds the FX2 by VID and PID
// It uses the read_i2c and write_i2c methods.

using System;
using System.Collections.Generic;
using System.Text;
using HPSDR_USB_LIB_V1;
using System.Globalization;

namespace read_EEPROM
{
    class Program
    {
        static void Main(string[] args)
        {
            if ((args.Length != 5) || (args.Length == 0))
            {
                Console.WriteLine("usage: read_EEPROM <VID> <PID> <i2c_address in hex> <start_pos in hex> <length in hex>");
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
                    Console.WriteLine("You must specify start_pos in Hex (0x0)");
                    return;
                }
                else
                    args[3] = args[3].Substring(2);
            }
            else
            {
                Console.WriteLine("You must specify start_pos in Hex (0x0)");
                return;
            }

            int start = int.Parse(args[3], NumberStyles.HexNumber);

            if (args[4].Length > 2)
            {
                if (args[4].Substring(0, 2) != "0x")
                {
                    Console.WriteLine("You must specify length in Hex (0x0)");
                    return;
                }
                else
                    args[4] = args[4].Substring(2);
            }
            else
            {
                Console.WriteLine("You must specify length in Hex (0x0)");
                return;
            }

            int length = int.Parse(args[4], NumberStyles.HexNumber);

            libUSB_Interface.usb_init();
            Console.WriteLine("finding busses...");
            libUSB_Interface.usb_find_busses();
            Console.WriteLine("finding devices...");
            libUSB_Interface.usb_find_devices();
            Console.WriteLine("usb_get_busses...");
            libUSB_Interface.usb_bus bus = libUSB_Interface.usb_get_busses();
            Console.WriteLine("bus location: " + bus.location.ToString());

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

            IntPtr usb_dev_handle = libUSB_Interface.usb_open(fdev);
            Console.WriteLine("Device handle is: " + usb_dev_handle.ToString());

            byte[] buf = new byte[length];

            if ((OZY.Read_EEPROM(usb_dev_handle, i2c_addr, start, ref buf)))
            {
                Console.WriteLine("Read from address " + i2c_addr + ":");
                for (int i = 0; i < buf.Length; i++)
                {
                    Console.Write(buf[i].ToString("X"));
                    if (i < buf.Length-1)
                    {
                        Console.Write(":");
                    }
                }        
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Failed to read address " + i2c_addr);
            }

            Console.WriteLine("EEPROM TYPE: " + OZY.Read_EEPROM_Type(usb_dev_handle).ToString());
            Console.WriteLine("I2C SPEED: " + OZY.Read_I2C_Speed(usb_dev_handle).ToString());

            Console.WriteLine("Closing device...");
            libUSB_Interface.usb_close(usb_dev_handle);
            Console.WriteLine("done...");
        }
    }
}

