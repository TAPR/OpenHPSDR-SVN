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

// This program toggles LED 0 or LED 1 connected to the FX2 chip
// On OZY V1, LED 0 is connected to Port C.6 and LED 1 is 
// connected to Port C.7

using System;
using System.Collections.Generic;
using System.Text;
using HPSDR_USB_LIB_V1;
using System.Globalization;

namespace set_LED
{
    class Program
    {
        static void Main(string[] args)
        {

            if ((args.Length != 4) || (args.Length == 0))
            {
                Console.WriteLine("usage: set_LED <VID> <PID> <which_led = 0|1> <state = ON|OFF>");
                return;
            }

            if (args[0].Substring(0, 2) == "0x")
                args[0] = args[0].Substring(2);

            if (args[1].Substring(0, 2) == "0x")
                args[1] = args[1].Substring(2);

            int vid = int.Parse(args[0], NumberStyles.HexNumber);
            int pid = int.Parse(args[1], NumberStyles.HexNumber);

            int which = int.Parse(args[2], NumberStyles.HexNumber);
            if (which > 1 || which < 0)
            {
                Console.WriteLine("Valid values for which_led are 0 or 1");
                return;
            }

            bool state;

            if (args[3] == "ON" 
                || args[3] == "on" 
                || args[3] == "On" 
                || args[3] == "1" 
                || args[3] == "true"
                || args[3] == "TRUE"
                || args[3] == "True")
            {
                state = true;
            }
            else if (args[3] == "OFF"
                || args[3] == "off"
                || args[3] == "Off"
                || args[3] == "0"
                || args[3] == "false"
                || args[3] == "FALSE"
                || args[3] == "False")
            {
                state = false;
            }
            else
            {
                Console.WriteLine("Valid values for state are ON or OFF");
                return;
            }

            libUSB_Interface.usb_init();
            Console.WriteLine("finding busses...");
            libUSB_Interface.usb_find_busses();
            Console.WriteLine("finding devices...");
            libUSB_Interface.usb_find_devices();
            Console.WriteLine("usb_get_busses...");
            libUSB_Interface.usb_bus bus = libUSB_Interface.usb_get_busses();
            Console.WriteLine("bus location: " + bus.location.ToString());

            USB usb = new USB();

            Console.WriteLine("Checking for VID PID...");
            libUSB_Interface.usb_device fdev = usb.FindDevice(bus, vid, pid);
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

            Console.WriteLine("Setting LED " + which.ToString() + " " + state.ToString());
            OZY.Set_LED(usb_dev_handle, which, state);

            Console.WriteLine("Closing device...");
            libUSB_Interface.usb_close(usb_dev_handle);
            Console.WriteLine("done...");            

        }
    }
}
