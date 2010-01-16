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
            if ((args.Length < 5) || (args.Length == 0))
            {
                Console.WriteLine("usage: write_I2C <VID> <PID> <i2c_address in hex> <count in hex> <bytes in hex...>");
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
                    Console.WriteLine("You must specify count in Hex (0x0)");
                    return;
                }
                else
                    args[3] = args[3].Substring(2);
            }
            else
            {
                Console.WriteLine("You must specify count in Hex (0x0)");
                return;
            }

            int count = int.Parse(args[3], NumberStyles.HexNumber);

            byte[] bytes = new byte[args.Length - 4];

            for (int i = 4; i < args.Length; i++)
            {
                if (args[i].Length > 2)
                {
                    if (args[i].Substring(0, 2) != "0x")
                    {
                        Console.WriteLine("You must specify bytes in Hex (0x0)");
                        return;
                    }
                    else
                        args[i] = args[i].Substring(2);
                }
                else
                {
                    Console.WriteLine("You must specify bytes in Hex (0x0)");
                    return;
                }

                bytes[i-4] = byte.Parse(args[i], NumberStyles.HexNumber);
            }

            IntPtr usb_dev_handle = IntPtr.Zero;

            try
            {
                usb_dev_handle = USB.InitFindAndOpenDevice(vid, pid);
                Console.WriteLine("Device handle is: " + usb_dev_handle.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return;
            }
                        
            if ((OZY.Write_I2C(usb_dev_handle, i2c_addr, bytes)))
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
