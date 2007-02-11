/*
 * QS1R QuickSilver SDRx
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

// This program writes to the DEBUG LED register on QS1R

using System;
using System.Collections.Generic;
using System.Text;
using HPSDR_USB_LIB_V1;
using System.Globalization;

namespace Set_QS1R_LEDREG
{
    class Program
    {
        static void Main(string[] args)
        {

            if ((args.Length != 3) || (args.Length == 0))
            {
                Console.WriteLine ( "usage: Set_QS1R_LEDREG <VID> <PID> <value: 0-FF Hex>" );
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

            if ( args [ 2 ].Length > 2 )
            {
                if ( args [ 2 ].Substring ( 0, 2 ) != "0x" )
                {
                    Console.WriteLine ( "You must specify value in Hex (0x0)" );
                    return;
                }
                else
                    args [ 2 ] = args [ 2 ].Substring ( 2 );
            }
            else
            {
                Console.WriteLine ( "You must specify value in Hex (0x0)" );
                return;
            }

            int value = int.Parse(args[2], NumberStyles.HexNumber);
            

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

            Console.WriteLine("Setting LEDs " + value.ToString());
            //OZY.Set_LED_Register ( usb_dev_handle, (byte)value );
            OZY.Set_Passthrough ( usb_dev_handle, 0x01, value, 0, new byte [ 0 ] );

            byte [ ] buf1 = new byte [ 1 ];

            OZY.Get_Passthrough ( usb_dev_handle, 0x86, 0, 0, ref buf1 );
            Console.WriteLine ( "Readback: " + buf1[0] );

            Console.WriteLine("Closing device...");
            libUSB_Interface.usb_close(usb_dev_handle);
            Console.WriteLine("done...");            

        }
    }
}

