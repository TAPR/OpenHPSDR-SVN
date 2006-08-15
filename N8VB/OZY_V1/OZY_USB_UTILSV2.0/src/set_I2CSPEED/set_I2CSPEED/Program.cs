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

// This program is a command line utility to set the I2C Speed on the FX2.
// It finds the FX2 by VID and PID

using System;
using System.Collections.Generic;
using System.Text;
using HPSDR_USB_LIB_V1;
using System.Globalization;

namespace set_I2CSPEED
{
    class Program
    {
        static void Main(string[] args)
        {
            if ((args.Length != 3) || (args.Length == 0))
            {
                Console.WriteLine("usage: read_EEPROM <VID> <PID> <value in hex {0x00=100kHz, 0x01=400kHz}> ");
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
                    Console.WriteLine("You must specify value in Hex (0x0)");
                    return;
                }
                else
                    args[2] = args[2].Substring(2);
            }
            else
            {
                Console.WriteLine("You must specify value in Hex (0x0)");
                return;
            }

            int value = int.Parse(args[2], NumberStyles.HexNumber);
                        
            IntPtr usb_dev_handle = USB.InitFindAndOpenDevice(vid, pid);

            if ((OZY.Set_I2C_Speed(usb_dev_handle, value)))
            {
                Console.WriteLine("Set I2C speed successful");                
            }
            else
            {
                Console.WriteLine("Failed to set I2C speed");
            }

            Console.WriteLine("I2C SPEED: " + OZY.Read_I2C_Speed(usb_dev_handle).ToString());

            Console.WriteLine("Closing device...");
            libUSB_Interface.usb_close(usb_dev_handle);
            Console.WriteLine("done...");
        }
    }
}
